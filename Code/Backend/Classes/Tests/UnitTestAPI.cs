using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
//using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

/*
public class AuthenticationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthenticationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {

        var loginRequest = new
        {
            Email = "testuser@example.com",
            Password = "TestPassword123"
        };


        var response = await _client.PostAsJsonAsync("/api/login", loginRequest);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var responseJson = JsonConvert.DeserializeObject<dynamic>(responseBody);

        Assert.NotNull(responseJson?.token);

    }
}

//[Fact]
//public async Task Login_InvalidCredentials_ReturnsUnauthorized()
//{
//    var loginRequest = new
//    {
//        Email = "nonexistent@example.com",
//        Password = "WrongPassword"
//    };

//    var response = await _client.PostAsJsonAsync("/api/login", loginRequest);

//    Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
//}

//// Testowanie pobierania konta
//[Fact]
//public async Task GetAccount_Authorized_ReturnsAccountInfo()
//{
//    var loginRequest = new
//    {
//        Email = "testuser@example.com",
//        Password = "TestPassword123"
//    };

//    var loginResponse = await _client.PostAsJsonAsync("/api/login", loginRequest);
//    var loginResponseBody = await loginResponse.Content.ReadAsStringAsync();
//    var loginJson = JsonConvert.DeserializeObject<dynamic>(loginResponseBody);
//    string token = loginJson?.token;

//    _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

//    var response = await _client.GetAsync("/api/account");

//    response.EnsureSuccessStatusCode();

//    var responseBody = await response.Content.ReadAsStringAsync();
//    var account = JsonConvert.DeserializeObject<dynamic>(responseBody);

//    Assert.Equal("testuser@example.com", account?.Email);
//}

//[Fact]
//public async Task GetAccount_Unauthorized_ReturnsUnauthorized()
//{
//    var response = await _client.GetAsync("/api/account");

//    Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
//}

//// Testowanie pobierania produktów
//[Fact]
//public async Task GetProducts_ValidCategory_ReturnsFilteredProducts()
//{
//    var response = await _client.GetAsync("/api/products?category=Electronics");

//    response.EnsureSuccessStatusCode();

//    var responseBody = await response.Content.ReadAsStringAsync();
//    var products = JsonConvert.DeserializeObject<dynamic>(responseBody);

//    Assert.NotEmpty(products);
//}

//[Fact]
//public async Task GetProducts_PriceRange_ReturnsFilteredProducts()
//{
//    var response = await _client.GetAsync("/api/products?min_price=10&max_price=100");

//    response.EnsureSuccessStatusCode();

//    var responseBody = await response.Content.ReadAsStringAsync();
//    var products = JsonConvert.DeserializeObject<dynamic>(responseBody);

//    Assert.NotEmpty(products);
//}

//[Fact]
//public async Task GetProducts_Pagination_ReturnsPagedResults()
//{
//    var response = await _client.GetAsync("/api/products?page=1&limit=10");

//    response.EnsureSuccessStatusCode();

//    var responseBody = await response.Content.ReadAsStringAsync();
//    var products = JsonConvert.DeserializeObject<dynamic>(responseBody);

//    Assert.Equal(10, products.Count);
//}

//// Testowanie pobierania zdjęcia
//[Fact]
//public async Task GetPhoto_ExistingPhoto_ReturnsPhoto()
//{
//    var response = await _client.GetAsync("/api/photo/1");

//    response.EnsureSuccessStatusCode();

//    Assert.Equal("image/jpeg", response.Content.Headers.ContentType.MediaType);
//}

//[Fact]
//public async Task GetPhoto_NonExistingPhoto_ReturnsNotFound()
//{
//    var response = await _client.GetAsync("/api/photo/999");

//    Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
//}

*/