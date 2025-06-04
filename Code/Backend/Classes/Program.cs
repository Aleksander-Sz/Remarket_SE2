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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;




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
    string? limit,
    string? id,
    string? userId) =>
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

    if (Int32.TryParse(id, out var idVal))
    {
        var listing = await query
            .Where(l => l.Id == idVal)
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
            .FirstOrDefaultAsync();

        return listing != null ? Results.Json(listing) : Results.NotFound();
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

app.MapGet("/api/categories", async (AppDbContext db) =>
{
    var categories = await db.Categories.ToListAsync();
    return Results.Json(categories);
});

app.MapGet("/api/photo/{id}", async (int id, AppDbContext db) =>
{
    var photo = await db.Photos.FindAsync(id);
    if (photo == null || photo.Bytes == null)
        return Results.NotFound();

    return Results.File(photo.Bytes, "image/jpeg"); // or image/png if needed
});

app.MapPost("/api/photo", async (HttpRequest request, AppDbContext db) =>
{
    var form = await request.ReadFormAsync();
    var file = form.Files.GetFile("image");

    if (file == null || file.Length == 0)
        return Results.BadRequest("No image uploaded.");

    using var memoryStream = new MemoryStream();
    await file.CopyToAsync(memoryStream);
    var photoBytes = memoryStream.ToArray();
    var photo = new Photo("placeholder name", photoBytes);
    db.Photos.Add(photo);
    await db.SaveChangesAsync();

    return Results.Ok(new { id = photo.Id });
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

app.MapPost("/api/register", async (
    AppDbContext db,
    RegisterRequest request) =>
{
    var email = request.Email?.Trim();
    var username = request.Username?.Trim();
    var password = request.Password;
    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        return Results.BadRequest("Email, username and password are required.");

    var user = await db.Accounts.FirstOrDefaultAsync(u => u.Email == email);
    //return Results.Json(user);
    if (user == null)
        return Results.BadRequest("A user with this email already exists.");

    user = await db.Accounts.FirstOrDefaultAsync(u => u.Username == username);
    //return Results.Json(user);
    if (user == null)
        return Results.BadRequest("A user with this name already exists.");

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
    var newUser = new Account(username, email, password);
    db.Accounts.Add(newUser);

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
        new Claim(ClaimTypes.Email, newUser.Email)
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

app.MapPost("/api/addListing", async (
    AppDbContext db,
    string? title,
    string? header,
    string? paragraph,
    int? category,
    int? price,
    int? photoId) =>
{
    var listing = new Listing
    {
        Title = title,
        DescriptionId = 0,
        CategoryId = category ?? 0,
        Price = price ?? 0,
        ThumbnailId = photoId
    };

    db.Listings.Add(listing);
    await db.SaveChangesAsync();

    return Results.Ok(listing.Id);
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

/* payment system - not tested, not connected, hardcoded
StripeConfiguration.ApiKey = //change here
app.MapPost("/api/", async context => //change api call here
{
    var options = new Stripe.Checkout.SessionCreateOptions
    {
        PaymentMethodTypes = new List<string>
        {
            "card",
        },
        LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        {
            new Stripe.Checkout.SessionLineItemOptions
            {
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    UnitAmount = 1000, //this is 10zł, it is in smallest unit of the currency
                    Currency = "pln",
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Mock Product",
                        Description = "This is a fake product for demonstration purposes only"
                    },
                },
                Quantity = 1,
            },
        },
        Mode = "payment",
        SuccessUrl = "https://example.com/success", //change here
        CancelUrl = "https://example.com/cancel", //change here
    };

    var service = new Stripe.Checkout.SessionService();
    var session = service.Create(options);

    await context.Response.WriteAsJsonAsync(new { sessionId = session.Id });
});
});
*/


app.Run();

public partial class Program { }
