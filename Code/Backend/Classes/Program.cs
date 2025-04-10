using Microsoft.EntityFrameworkCore;
using Remarket_SE2; 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/info", () =>
{
    var time = DateTime.Now.ToString("HH:mm:ss"); 
    var userId = new Random().Next(1000, 9999); 

    return Results.Json(new { time, userId }); 
});

app.Run();
