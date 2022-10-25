using Domain;
using System.Linq;
using Contracts.Database;
using Microsoft.EntityFrameworkCore;

namespace Domain.Database;
public class GetDataFromDb
{
    private readonly BotDbContext _context;
    
    public GetDataFromDb(BotDbContext botDbContext) 
    {
        _context = botDbContext;
    }

    internal List<Product> GetOneProductFromDbBySku(string sku)
    {
        var products = _context.Products
        .Where(p => p.Sku.Equals(sku))
        .Take(5)
        .Include(p => p.Category)
        .Include(p => p.Category.PartentCategory)
        .Include(p => p.Brand)
        .Include(p => p.Variations)
            .ThenInclude(x => x.VariationPrices)
                .ThenInclude(pt =>pt.PriceType)
            .Include(x =>x.Variations)
                .ThenInclude(x => x.VariationStocks)
                    .ThenInclude(x => x.Storage)
        .ToList();
        return products;
    }

    internal List<Product> GetProductsFromDbBySku(string sku)
    {
        var products = _context.Products
        .Where(p => p.Sku.Contains(sku))
        .Take(5)
        .Include(p => p.Category)
        .Include(p => p.Category.PartentCategory)
        .Include(p => p.Brand)
        .Include(p => p.Variations)
            .ThenInclude(x => x.VariationPrices)
                .ThenInclude(pt =>pt.PriceType)
            .Include(x =>x.Variations)
                .ThenInclude(x => x.VariationStocks)
                    .ThenInclude(x => x.Storage)
        .ToList();
        return products;
    }

}