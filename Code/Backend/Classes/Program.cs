
using ReMarket.Models;
using ReMarket.Services;

//var databaseConnection = new DatabaseConnection("ReMarket", "root", "toor1234");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
//    ));

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
string filePath = "C:\\Users\\Zuzia\\Aleksander\\Remarket_SE2\\Code\\Backend\\example.jpg"; // path to an image file
string name = Path.GetFileName(filePath);
byte[] imageBytes = File.ReadAllBytes(filePath);
string base64String = Convert.ToBase64String(imageBytes);

Photo photo = new Photo(name, base64String, false);
clothesListings[0].Thumbnail = photo;
app.MapGet("/api/clothes", () => Results.Json(clothesListings));
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

app.Run();
