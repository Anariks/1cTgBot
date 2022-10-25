namespace Domain.ApiClient.Interfaces;

public interface IProductApiClient 
{
    Task<string> GetProductLinkBySku(string sku, CancellationToken cancellationToken = default);
    Task<string> GetProductSkuBySlug (string slug, CancellationToken cancellationToken = default);
}