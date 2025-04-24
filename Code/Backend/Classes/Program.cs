
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

app.MapGet("/api/info", () =>
{
    var time = DateTime.Now.ToString("HH:mm:ss"); 
    var userId = new Random().Next(1000, 9999); 

    return Results.Json(new { time, userId }); 
});

app.Run();
