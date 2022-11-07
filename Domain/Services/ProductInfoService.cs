using System.Linq;
using Contracts.Database;
using Domain.ApiClient.Interfaces;
using Domain.Database;
using Domain.Services;
using Domain.Services.Interfaces;

namespace Domain.Services;

public class ProductInfoService : IProductInfoService
{
    private readonly IProductApiClient _productApiClient;
    private readonly GetDataFromDb _getDataFromDb;

    public ProductInfoService(IProductApiClient productApiClient, GetDataFromDb getDataFromDb)
    {
        _productApiClient = productApiClient;
        _getDataFromDb = getDataFromDb;
    }
    public List<Product> GetProductInfoByQuery(string query)
    {
        List<Product> products;
        var isLink = IsQueryALink(query);

        if (isLink)
        {
            var slug = LinkToSlug(query);
            var sku = _productApiClient.GetProductSkuBySlug(slug).Result;
            return _getDataFromDb.GetOneProductFromDbBySku(sku);
        }

        products = _getDataFromDb.GetProductsFromDbBySku(query);

        products.Where(x => x.Url.Length < 10).ToList()
            .ForEach(x =>
                {
                    x.Url = _productApiClient.GetProductLinkBySku(x.Sku).Result;
                });
        return products;
    }

    private bool IsQueryALink(string query)
    {
        Uri uriResult;
        return Uri.TryCreate(query, UriKind.Absolute, out uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private string LinkToSlug(string query)
    {
        var queryArr = query.Split('/');
        return queryArr.SkipLast(1).Last();
    }

}


