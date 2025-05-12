using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using ReMarket.Models;
using ReMarket.Services;
using System.Net.Http;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;
using Mysqlx.Crud;
using System.Drawing;

//var databaseConnection = new DatabaseConnection("ReMarket", "root", "toor1234");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("p8ZfQsR2Xj6sDk93YtBLu7cV1gX9aEfM"))
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (!context.Request.Path.Value.Contains('.') &&
        !context.Request.Path.StartsWithSegments("/api"))
    {
        context.Response.ContentType = "text/html";
        await context.Response.SendFileAsync(Path.Combine(app.Environment.WebRootPath, "index.html"));
    }
    else
    {
        await next();
    }
});

//clothesListings[0].Thumbnail = new Photo();
//app.MapGet("/api/clothes", () => Results.Json(clothesListings));
app.MapGet("/api/products", async (
    AppDbContext db,
    string? category,
    string? min_price,
    string? max_price,
    string? page,
    string? limit) =>
{
    var query = db.Listings
        .Include(l => l.Category)
        .Include(l => l.Description)
        .Include(l => l.ListingPhotos)
        .ThenInclude(lp => lp.Photo)
        .AsQueryable();
    if (!string.IsNullOrEmpty(category))
    {
        query = query.Where(l => l.Category.Name.ToLower() == category.ToLower());
    }

    if (decimal.TryParse(min_price, out var minVal))
        query = query.Where(l => l.Price >= minVal);

    if (decimal.TryParse(max_price, out var maxVal))
        query = query.Where(l => l.Price <= maxVal);

    // page and limit

    if (Int32.TryParse(page, out var pageVal) && Int32.TryParse(limit, out var limitVal))
    {
        int skip = (pageVal - 1) * limitVal;
        query = query.Skip(skip).Take(limitVal);
    }

    var listings = await query
        .Select(l => new
        {
            l.Id,
            l.Title,
            l.Price,
            Category = new { l.Category.Id, l.Category.Name },
            Description = new { l.Description.Id, l.Description.Header, l.Description.Paragraph },
            Status = l.Status.ToString(),
            PhotoIds = l.ListingPhotos.Select(lp => lp.PhotoId).ToList()
        })
        .ToListAsync();

    return Results.Json(listings);
});

app.MapGet("/api/photo/{id}", async (int id, AppDbContext db) =>
{
    var photo = await db.Photos.FindAsync(id);
    if (photo == null || photo.Bytes == null)
        return Results.NotFound();

    return Results.File(photo.Bytes, "image/jpeg"); // or image/png if needed
});

app.MapPost("/api/login", async (
    AppDbContext db,
    LoginRequest request) =>
{
    var email = request.Email;
    var password = request.Password;
    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        return Results.BadRequest("Email and password are required.");

    var user = await db.Accounts.FirstOrDefaultAsync(u => u.Email == email);
    //return Results.Json(user);
    if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        return Results.Unauthorized();

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("p8ZfQsR2Xj6sDk93YtBLu7cV1gX9aEfM"));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds);

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

    return Results.Ok(new { token = tokenString });
});

app.MapGet("/api/account", async (
    ClaimsPrincipal user,
    AppDbContext db) =>
{
    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Results.Unauthorized();

    int userId = int.Parse(userIdClaim.Value);

    var account = await db.Accounts
        .Where(a => a.Id == userId)
        .Select(a => new { a.Id, a.Email, a.Username }) // don't return password
        .FirstOrDefaultAsync();

    return account is not null ? Results.Ok(account) : Results.NotFound();
}).RequireAuthorization();

app.MapGet("/api/connection_string", () => builder.Configuration.GetConnectionString("DefaultConnection"));


app.MapGet("/api/info", () =>
{
    var time = DateTime.Now.ToString("HH:mm:ss");
    var userId = new Random().Next(1000, 9999);

    return Results.Json(new { time, userId });
});


app.Run();
