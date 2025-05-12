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
                Encoding.UTF8.GetBytes("YOUR_SECRET_KEY_HERE"))
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

var clothesListings = new List<Listing>
{
    //new Listing(0,"Test Jacked",100,new Category(0,"Clothes"),new Description("Example desc", "more useful text")),
    //new Listing(0,"Test Trousers",50,new Category(0,"Clothes"),new Description("Example desc", "more useful text lorem ipsum")),
    //new Listing(0,"Test Blazer",70,new Category(0,"Clothes"),new Description("Example desc", "more useful text")),
    //new Listing(0,"Test T-Shirt",20,new Category(0,"Clothes"),new Description("Example desc hello", "more useful text")),
    //new Listing(0,"Test Coat",600,new Category(0,"Clothes"),new Description("Example desc 123", "more useful text"))
};

//clothesListings[0].Thumbnail = new Photo();
//app.MapGet("/api/clothes", () => Results.Json(clothesListings));
app.MapGet("/api/products", async (
    AppDbContext db,
    string? category,
    string? min_price,
    string? max_price) =>
{
    var query = db.Listings
        .Include(l => l.Category)
        .Include(l => l.Description)
        .AsQueryable();
    if (!string.IsNullOrEmpty(category))
    {
        query = query.Where(l => l.Category.Name.ToLower() == category.ToLower());
    }

    if (decimal.TryParse(min_price, out var minVal))
        query = query.Where(l => l.Price >= minVal);

    if (decimal.TryParse(max_price, out var maxVal))
        query = query.Where(l => l.Price <= maxVal);

    var listings = await query
        .Select(l => new
        {
            l.Id,
            l.Title,
            l.Price,
            Category = new { l.Category.Id, l.Category.Name },
            Description = new { l.Description.Id, l.Description.Header, l.Description.Paragraph },
            Status = l.Status.ToString()
        })
        .ToListAsync();

    return Results.Json(listings);
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

/*app.MapPost("/api/login", async (HttpContext context, AppDbContext db) =>
{
    var loginRequest = await context.Request.ReadFromJsonAsync<LoginRequest>();
    if (loginRequest == null) return Results.BadRequest();

    var user = db.Accounts.FirstOrDefault(a => a.Username == loginRequest.Username);
    if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        return Results.Unauthorized();

    SessionManager.Login(user);
    return Results.Ok(new { message = "Login successful" });
});

record LoginRequest(string Username, string Password);*/

/*app.MapGet("/api/user", () =>
{
    var user = SessionManager.CurrentUser;
    if (user == null)
        return Results.Unauthorized();

    return Results.Json(new
    {
        user.Id,
        user.Username,
        // inne pola je�li chcesz
    });
});


app.MapPost("/api/logout", () =>
{
    SessionManager.Logout();
    return Results.Ok(new { message = "Logged out" });
});*/

app.Run();
