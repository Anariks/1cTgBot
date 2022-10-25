using Contracts.Database;

namespace Domain.Services.Interfaces;

public interface IProductInfoService 
{
    List<Product> GetProductInfoByQuery(string query);
}