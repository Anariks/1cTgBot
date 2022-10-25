using Domain.ApiClient;
using Domain.Services;
using RichardSzalay.MockHttp;
using Shouldly;

namespace UnitTests;

public class ProductApiClientTests
{
    [Fact]
    public async void GetProductLinkBySkuShouldReturnUrl()
    {
        //Arrange
        var mockHttp = new MockHttpMessageHandler();

          using (StreamReader r = new StreamReader("../../../response.json"))
        {
            string json = r.ReadToEnd();
        
        mockHttp.When("https://yonex.ua/*")
            .Respond("application/json", json);
        }

        var client = new HttpClient(mockHttp);
        client.BaseAddress = new Uri("https://yonex.ua");
        var productApiClient = new ProductApiClient(client);

        //Act
        var result = await productApiClient.GetProductLinkBySku("07EZ100SBL");

        //Assert
        result.ShouldBeEquivalentTo("https://yonex.ua/products/raketka-dlya-tennysa-yonex-07-ezone-100-300g-sky-blue/");
    }

    [Fact]
    public async void GetProductLinkBySkuShouldReturnNothing()
    {
        //Arrange
        var mockHttp = new MockHttpMessageHandler();
        
        mockHttp.When("https://yonex.ua/*")
            .Respond("application/json", "[]");

        var client = new HttpClient(mockHttp);
        client.BaseAddress = new Uri("https://yonex.ua");
        var productApiClient = new ProductApiClient(client);

        //Act
        string? result = await productApiClient.GetProductLinkBySku("NOTEXISTARTICLE");

        //Assert
        result.ShouldBeEquivalentTo("");
    }
}