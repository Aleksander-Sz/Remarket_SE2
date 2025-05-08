using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using ReMarket.Models;
using ReMarket.Services;
using System.Net.Http;

//var databaseConnection = new DatabaseConnection("ReMarket", "root", "toor1234");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

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
    new Listing(0,"Test Jacked",100,new Category(0,"Clothes"),new Description("Example desc", "more useful text")),
    new Listing(0,"Test Trousers",50,new Category(0,"Clothes"),new Description("Example desc", "more useful text lorem ipsum")),
    new Listing(0,"Test Blazer",70,new Category(0,"Clothes"),new Description("Example desc", "more useful text")),
    new Listing(0,"Test T-Shirt",20,new Category(0,"Clothes"),new Description("Example desc hello", "more useful text")),
    new Listing(0,"Test Coat",600,new Category(0,"Clothes"),new Description("Example desc 123", "more useful text"))
};

//clothesListings[0].Thumbnail = new Photo();
//app.MapGet("/api/clothes", () => Results.Json(clothesListings));
app.MapGet("/api/clothes", async (AppDbContext db) =>
{
    var listings = await db.Listings
        .Include(l => l.Category)
        .Include(l => l.Description)
        .ToListAsync();
    return Results.Json(listings);
});

/*string filePath = "C:\\Users\\Zuzia\\Aleksander\\Remarket_SE2\\Code\\Backend\\example.jpg"; // path to an image file
string name = Path.GetFileName(filePath);
byte[] imageBytes = File.ReadAllBytes(filePath);
string base64String = Convert.ToBase64String(imageBytes);

Photo photo = new Photo(name, base64String, false);
clothesListings[0].Thumbnail = photo;
app.MapGet("/api/clothes", () => Results.Json(clothesListings));*/

app.MapGet("/api/accessories", () => Results.Json(clothesListings));
app.MapGet("/api/toys", () => Results.Json(clothesListings));
app.MapGet("/api/kids", () => Results.Json(clothesListings));
app.MapGet("/api/women", () => Results.Json(clothesListings));
app.MapGet("/api/men", () => Results.Json(clothesListings));

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


//FROM HERE CHANGES BY ALEKSANDRA FROM 7.05

var accountService = new AccountService(new DummyEmailService());
app.MapPost("/api/login", async (HttpContext context) =>
{
    var loginRequest = await context.Request.ReadFromJsonAsync<LoginRequest>();
    if (loginRequest == null)
        return Results.BadRequest("Invalid request");

    var result = accountService.Login(loginRequest.Email, loginRequest.Password);

    if (!result.IsSuccess)
        return Results.Json(new { error = "Email or password is wrong" }, statusCode: 401);

    return Results.Ok(new
    {
        message = "Login successful",
        user = new
        {
            result.Account.Username,
            result.Account.Email.Value,
            result.Account.Name
        }
    });
});

AccountRepository.Add(new Account(
    username: "testuser",
    name: "Test User",
    email: new Email("test@example.com"),
    passwordHash: BCrypt.Net.BCrypt.HashPassword("password123")
));


app.Run();

record LoginRequest(string Email, string Password);