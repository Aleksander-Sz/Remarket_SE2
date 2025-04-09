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
