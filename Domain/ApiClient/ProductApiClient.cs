using System.Net.Http.Headers;
using Domain.ApiClient.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Domain.ApiClient;

class ProductFromJson
{
    public string Permalink { get; init; }
}

public class ProductApiClient : IProductApiClient
{
    private readonly HttpClient _client;

    public ProductApiClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<string> GetProductLinkBySku(string sku, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync($"/wp-json/wc/v3/products/?sku={sku}", cancellationToken);

        if(!response.IsSuccessStatusCode)
        {
            return "";
        } 
        
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var productsJson = JArray.Parse(content);

        if (productsJson.Count() < 1)
            return "";

        try
        {
            string? productLink = (string?)productsJson[0].SelectToken("['permalink']");

            return productLink;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> GetProductSkuBySlug(string slug, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync($"/wp-json/wc/v3/products/?slug={slug}", cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        var productsJson = JArray.Parse(content);

        string? productSku = (string?)productsJson[0].SelectToken("['sku']");

        return productSku;
    }
}