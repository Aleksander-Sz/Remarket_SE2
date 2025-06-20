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

app.MapGet("/api/products", async (
    AppDbContext db,
    string? category,
    string? min_price,
    string? max_price,
    string? page,
    string? limit,
    string? id,
    string? ownerId) =>
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
                Owner = new { l.Owner.Id, l.Owner.Username },
                PhotoIds = l.ListingPhotos.Select(lp => lp.PhotoId).ToList()
            })
            .FirstOrDefaultAsync();

        return listing != null ? Results.Json(listing) : Results.NotFound();
    }

    if (decimal.TryParse(min_price, out var minVal))
        query = query.Where(l => l.Price >= minVal);

    if (decimal.TryParse(max_price, out var maxVal))
        query = query.Where(l => l.Price <= maxVal);

    //ownerId:
    if (int.TryParse(ownerId, out var ownerIdVal))
        query = query.Where(l => l.OwnerId == ownerIdVal);

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

app.MapGet("/api/reviews", async (
    AppDbContext db,
    int? userId,
    int? listingId) =>
{
    var query = db.Reviews
        .Include(r => r.Listing)     // Assuming a navigation property exists
        .Include(r => r.Account)     // Assuming a navigation property exists
        .AsQueryable();

    if (userId.HasValue)
        query = query.Where(r => r.AccountId == userId.Value);

    if (listingId.HasValue)
        query = query.Where(r => r.ListingId == listingId.Value);

    var reviews = await query
        .Select(r => new
        {
            r.Id,
            r.Title,
            r.Score,
            r.Description,
            Account = new { r.AccountId, r.Account.Username },
            Listing = new { r.ListingId, r.Listing.Title }
        })
        .ToListAsync();

    return Results.Json(reviews);
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
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Role, user.Role == 'A' ? "admin" : "user")

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
    if (user != null)
        return Results.BadRequest("A user with this email already exists.");

    user = await db.Accounts.FirstOrDefaultAsync(u => u.Username == username);
    //return Results.Json(user);
    if (user != null)
        return Results.BadRequest("A user with this name already exists.");

    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
    var newUser = new Account(username, email, hashedPassword);
    db.Accounts.Add(newUser);
    await db.SaveChangesAsync();

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

app.MapPost("/api/addListing", async (ListingDto data, AppDbContext db, ClaimsPrincipal user) =>
{
    if (data.Title == null || data.Category == null || data.Price == null)
        return Results.BadRequest("Missing required fields.");

    var userIdClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Results.Unauthorized();

    var ownerId = int.Parse(userIdClaim.Value);

    var description = new Description
    {
        Header = data.Header ?? string.Empty,
        Paragraph = data.Paragraph ?? string.Empty
    };
    db.Descriptions.Add(description);
    await db.SaveChangesAsync();

    var listing = new Listing
    {
        Title = data.Title,
        Price = data.Price.Value,
        Status = "available", // or some default string like "active"
        DescriptionId = description.Id, // hardcoded for now
        CategoryId = data.Category.Value,
        OwnerId = ownerId
    };

    db.Listings.Add(listing);
    await db.SaveChangesAsync(); // Save first to get the auto-incremented ID

    // Link the photo (if provided)
    if (data.PhotoId != null)
    {
        var link = new ListingPhoto
        {
            ListingId = listing.Id,
            PhotoId = data.PhotoId.Value
        };
        db.ListingPhotos.Add(link);
        await db.SaveChangesAsync();
    }

    return Results.Ok(new { listingId = listing.Id });
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
        .Select(a => new { a.Id, a.Email, a.Username, a.Role}) // don't return password
        .FirstOrDefaultAsync();

    return account is not null ? Results.Ok(account) : Results.NotFound();
}).RequireAuthorization();

app.MapGet("/api/user/{id}", async (
    int id,
    AppDbContext db) =>
{
    var account = await db.Accounts
        .Where(a => a.Id == id)
        .Select(a => new { a.Id, a.Username, PhotoId = (int?)a.PhotoId, a.Description }) // don't return password
        .FirstOrDefaultAsync();

    return account is not null ? Results.Ok(account) : Results.NotFound();
});

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



app.MapGet("/api/accounts", async (ClaimsPrincipal user, AppDbContext db) =>
{
    var roleClaim = user.FindFirst(ClaimTypes.Role);
    if (roleClaim == null || roleClaim.Value != "admin")
        return Results.Forbid();

    var users = await db.Accounts
        .Select(a => new
        {
            a.Id,
            a.Username,
            a.Email,
            a.Role
        })
        .ToListAsync();

    return Results.Ok(users);
}).RequireAuthorization();

app.MapDelete("/api/accounts/{id}", async (int id, ClaimsPrincipal user, AppDbContext db) =>
{
    var roleClaim = user.FindFirst(ClaimTypes.Role);
    if (roleClaim == null || roleClaim.Value != "admin")
        return Results.Forbid();

    var userToDelete = await db.Accounts.FindAsync(id);
    if (userToDelete == null)
        return Results.NotFound();

    db.Accounts.Remove(userToDelete);
    await db.SaveChangesAsync();

    return Results.Ok(new { message = $"User {id} deleted" });
}).RequireAuthorization();

app.MapPost("/api/changeRole/{UserId}", async (int UserId, RoleChangeRequest data, AppDbContext db, ClaimsPrincipal user) =>
{
    var roleClaim = user.FindFirst(ClaimTypes.Role);
    if (roleClaim == null || roleClaim.Value != "admin")
        return Results.Forbid();
    var userToUpdate = await db.Accounts.FindAsync(UserId);
    if (userToUpdate == null)
        return Results.NotFound();
    userToUpdate.Role = data.NewRole;
    await db.SaveChangesAsync();

    return Results.Ok(new { message = $"User {UserId} role changed to '{data.NewRole}'" });
}).RequireAuthorization();

app.MapPost("/api/changeProfile/{UserId}", async (int UserId, ProfileChangeRequest data, AppDbContext db, ClaimsPrincipal user) =>
{
    var userIdClaim = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Results.Unauthorized();

    int loggedInUserId = int.Parse(userIdClaim.Value);
    if (loggedInUserId != UserId)
        return Results.Forbid();

    var userToUpdate = await db.Accounts.FindAsync(UserId);
    if (userIdClaim == null)
        return Results.Unauthorized();

    string message = "";
    if (!string.IsNullOrWhiteSpace(data.NewUsername))
    {
        userToUpdate.Username = data.NewUsername;
        message += "username, ";
    }
    if (!string.IsNullOrWhiteSpace(data.NewDescription))
    {
        userToUpdate.Description = data.NewDescription;
        message += "description, ";
    }
    if (data.NewPhotoId.HasValue)
    {
        userToUpdate.PhotoId = data.NewPhotoId.Value;
        message += "photo";
    }
    if (!string.IsNullOrWhiteSpace(data.NewPassword))
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(data.NewPassword);
        userToUpdate.Password = hashedPassword;
        message += "password, ";
    }
    await db.SaveChangesAsync();

    if(message=="")
        return Results.Ok(new { message = $"Nothing to change" });
    else
        return Results.Ok(new { message = $"User {UserId} profile updated, things changed: " + message + "." });
}).RequireAuthorization();

app.MapPost("/api/addReview", async (
    ReviewRequest data,
    AppDbContext db,
    ClaimsPrincipal user) =>
{
    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Results.Unauthorized();

    int accountId = int.Parse(userIdClaim.Value);

    // Prevent duplicate reviews per user per listing
    var existing = await db.Reviews.FirstOrDefaultAsync(r => r.AccountId == accountId && r.ListingId == data.ListingId);
    if (existing != null)
        return Results.BadRequest("You have already submitted a review for this listing.");

    var review = new Review
    {
        Title = data.Title,
        Score = data.Score,
        Description = data.Description,
        ListingId = data.ListingId,
        AccountId = accountId
    };

    db.Reviews.Add(review);
    await db.SaveChangesAsync();

    // include username in response
    var account = await db.Accounts.FindAsync(accountId);

    return Results.Ok(new
    {
        review.Id,
        review.Title,
        review.Score,
        review.Description,
        Account = new { account.Id, account.Username },
        Listing = new { Id = data.ListingId }
    });
}).RequireAuthorization();

app.MapGet("/api/reviews/forUser/{userId}", async (
    int userId,
    AppDbContext db) =>
{
    var reviews = await db.Reviews
        .Include(r => r.Account) // the person who wrote the review
        .Include(r => r.Listing)
        .Where(r => r.Listing.OwnerId == userId)
        .Select(r => new
        {
            r.Id,
            r.Title,
            r.Score,
            r.Description,
            Reviewer = new { r.Account.Id, r.Account.Username },
            Listing = new { r.Listing.Id, r.Listing.Title }
        })
        .ToListAsync();

    return Results.Json(reviews);
});

app.MapGet("/api/orders", async (AppDbContext db, string? sellerId, string? buyerId, string? orderId) =>
{
    // Try parse single orderId
    if (int.TryParse(orderId, out var orderIdVal))
    {
        var singleOrder = await db.Orders
            .Where(o => o.Id == orderIdVal)
            .Select(o => new
            {
                o.Id,
                o.ShipTo,
                o.Shipped,
                o.Description,
                o.SellerId,
                o.BuyerId,
                o.PaymentId,
                Product = db.OrderListings
                    .Where(ol => ol.OrderId == o.Id)
                    .Select(ol => new
                    {
                        ProductId = ol.Listing.Id,
                        ProductName = ol.Listing.Title,
                        ProductPrice = ol.Listing.Price
                    })
                    .FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        return singleOrder != null ? Results.Json(singleOrder) : Results.NotFound();
    }

    // Multiple orders
    var query = db.Orders.AsQueryable();

    if (int.TryParse(sellerId, out var sellerIdVal))
    {
        query = query.Where(o => o.SellerId == sellerIdVal);
    }

    if (int.TryParse(buyerId, out var buyerIdVal))
    {
        query = query.Where(o => o.BuyerId == buyerIdVal);
    }

    var orders = await query
        .Select(o => new
        {
            o.Id,
            o.ShipTo,
            o.Shipped,
            o.Description,
            o.SellerId,
            o.BuyerId,
            o.PaymentId,
            Product = db.OrderListings
                .Where(ol => ol.OrderId == o.Id)
                .Select(ol => new {
                    ProductId = ol.Listing.Id,
                    ProductName = ol.Listing.Title
                })
                .FirstOrDefault()
        })
        .ToListAsync();

    return Results.Json(orders);
});


app.MapPost("/api/createOrder", async (
    CreateOrderRequest data,
    AppDbContext db,
    ClaimsPrincipal user) =>
{
    // Ensure user is authenticated
    var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Results.Unauthorized();

    int buyerId = int.Parse(userIdClaim.Value);

    var newOrder = new ReMarket.Models.Order
    {
        ShipTo = data.ShipTo,
        Description = data.Description,
        SellerId = data.SellerId,
        BuyerId = buyerId,
        PaymentId = null,
        Shipped = null
    };

    db.Orders.Add(newOrder);
    await db.SaveChangesAsync();

    return Results.Created($"/api/orders/{newOrder.Id}", new
    {
        newOrder.Id,
        newOrder.ShipTo,
        newOrder.Description,
        newOrder.SellerId,
        newOrder.BuyerId,
        newOrder.PaymentId,
        newOrder.Shipped
    });
}).RequireAuthorization();

app.MapGet("/api/payment/{paymentId:int}", async (
    int paymentId,
    AppDbContext db
) =>
{
    var payment = await db.Payments
        .Where(p => p.Id == paymentId)
        .Select(p => new
        {
            p.Id,
            p.PaidOn,
            p.Total,
            p.AccountId
        })
        .FirstOrDefaultAsync();

    return payment is not null ? Results.Ok(payment) : Results.NotFound();
});

app.MapPost("/api/makePayment", async (
    PaymentRequest request,
    HttpContext http,
    AppDbContext db) =>
{
    // 1. check authentication
    if (!http.User.Identity?.IsAuthenticated ?? true)
        return Results.Unauthorized();

    var userIdClaim = http.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
    if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var accountId))
        return Results.Unauthorized();

    // 2. Validate payment fields
    if (string.IsNullOrWhiteSpace(request.CardNumber) ||
        string.IsNullOrWhiteSpace(request.CardCVC) ||
        string.IsNullOrWhiteSpace(request.CardExpirationMonth) ||
        string.IsNullOrWhiteSpace(request.CardExpirationYear))
    {
        return Results.BadRequest("All card details must be provided.");
    }

    // 3. Find the order
    var order = await db.Orders.FindAsync(request.OrderId);
    if (order == null)
        return Results.NotFound("Order not found.");

    //  4. Get listing price ( assuming 1 product per order)
    var listing = await db.OrderListings
        .Where(ol => ol.OrderId == request.OrderId)
        .Select(ol => ol.Listing)
        .FirstOrDefaultAsync();

    if (listing == null)
        return Results.BadRequest("Associated listing not found.");

    var amount = listing.Price;

    // 5. Create new payment
    var payment = new Payment
    {
        AccountId = accountId,
        Total = amount,
        PaidOn = DateTime.UtcNow
    };

    db.Payments.Add(payment);
    await db.SaveChangesAsync(); // save to get payment ID

    // 6. Update orrder with paymentId
    order.PaymentId = payment.Id;
    await db.SaveChangesAsync();

    return Results.Ok(new { payment.Id, payment.Total, payment.PaidOn });
});


app.Run();

public partial class Program { }
