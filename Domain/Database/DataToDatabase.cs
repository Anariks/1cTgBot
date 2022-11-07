using Domain;
using System.Linq;
using Contracts.Database;
using Microsoft.EntityFrameworkCore;
using Domain.XmlData.Models;
using System.Globalization;
using System.Threading;
using Domain.XmlData;
using Domain.Services;

namespace Domain.Database;
public class DataToDatabase
{
    private readonly BotDbContext _context;
    private readonly XmlImportData _xmlImportData;
    private readonly XmlOffersData _xmlOffersData;

    public DataToDatabase(BotDbContext botDbContext, OptimizeDataFromXml optimizeDataFromXml)
    {
        _context = botDbContext;
        _xmlImportData = optimizeDataFromXml.GetOptimizedImportData();
        _xmlOffersData = optimizeDataFromXml.GetOptimizedOffersData();
    }

    public async Task PushBrandsToDatabase()
    {
        _context.Brands.RemoveRange(_context.Brands);
        await _context.SaveChangesAsync();
        var resultBrands = _xmlImportData.Каталог.Товары.Товар.Where(x => x.Изготовитель != null).Select(x => (x.Изготовитель.Ид, x.Изготовитель.Наименование)).Distinct();

        await _context.Brands.AddRangeAsync(resultBrands
            .Select(item => new Brand
            {
                Id = item.Ид,
                Name = item.Наименование
            })
        );
        await _context.SaveChangesAsync();
    }

    public async Task PushCategoriesToDatabase(List<Category1c> resultCat = null, string parentCategoryGuid = null)
    {
        if (resultCat == null)
        {
            _context.Categories.RemoveRange(_context.Categories);
            resultCat = _xmlImportData.Классификатор.Categories1c.Category1c;
        }

        var categories = resultCat;

        foreach (var item in categories)
        {
            await _context.Categories.AddAsync(
                 new Category
                 {
                     Id = item.Ид,
                     ParentCategoryId = parentCategoryGuid,
                     Name = item.Наименование
                 }
             );

            if (item.Categories1c.Category1c.Count() > 0) await PushCategoriesToDatabase(item.Categories1c.Category1c, item.Ид);
        }

        await _context.SaveChangesAsync();
    }

    public async Task PushProductsToDatabase()
    {
        _context.Products.RemoveRange(_context.Products);
        var resultProducts = _xmlImportData.Каталог.Товары.Товар;

        foreach (var item in resultProducts)
        {
            string brandGuid = "";
            if (item.Изготовитель != null)
            {
                brandGuid = item.Изготовитель.Ид;
            }

            await _context.Products.AddAsync(new Product
            {
                Id = item.Ид,
                Name = item.Наименование,
                Sku = item.Артикул,
                CategoryId = item.Categories.Ид[0],
                BrandId = brandGuid
            }
        );
        }
        await _context.SaveChangesAsync();
    }

    public async Task PushVariationsToDatabase()
    {
        _context.Variations.RemoveRange(_context.Variations);

        var result = _xmlOffersData.ПакетПредложений.Предложения.Предложение;

        foreach (var item in result)
        {
            var splittedItem = item.Ид.Split('#');

            var quantity = (item.Количество != null) ? float.Parse(item.Количество, CultureInfo.InvariantCulture) : 0;

            try
            {
                await _context.Variations.AddAsync(new Variation
                {
                    Id = splittedItem[1],
                    Name = item.ХарактеристикиТовара.ХарактеристикаТовара.Значение,
                    ProductId = splittedItem[0],
                    Quantity = quantity
                }
             );
            }
            catch (Exception)
            {
                throw;
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task PushVariationPricesToDatabase()
    {
        _context.VariationPrices.RemoveRange(_context.VariationPrices);
        var result = _xmlOffersData.ПакетПредложений.Предложения.Предложение;

        foreach (var item in result)
        {
            var splittedItem = item.Ид.Split('#');

            var itemPricesResult = item.Цены.Цена;

            foreach (var itemPrice in itemPricesResult)
            {
                await _context.VariationPrices.AddAsync(new VariationPrice
                {
                    VariationId = splittedItem[1],
                    PriceTypeId = itemPrice.ИдТипаЦены,
                    Price = float.Parse(itemPrice.ЦенаЗаЕдиницу, CultureInfo.InvariantCulture)
                }
         );
            }

        }
        await _context.SaveChangesAsync();
    }

    internal async Task PushPriceTypesToDatabase()
    {
        _context.PriceTypes.RemoveRange(_context.PriceTypes);
        var result = _xmlOffersData.ПакетПредложений.ТипыЦен.ТипЦены;

        foreach (var item in result)
        {
            await _context.PriceTypes.AddAsync(new PriceType
            {
                Id = item.Ид,
                Name = item.Наименование,
                CurrencyCode = item.Валюта
            }
          );
        }
        await _context.SaveChangesAsync();
    }

    internal async Task PushVariationStocksToDatabase()
    {
        _context.VariationStocks.RemoveRange(_context.VariationStocks);
        var result = _xmlOffersData.ПакетПредложений.Предложения.Предложение;

        foreach (var item in result)
        {
            var splittedItem = item.Ид.Split('#');

            var innerResult = item.Склад;

            foreach (var subItem in innerResult)
            {
                float quantity;
                if (subItem.КоличествоНаСкладе == null || subItem.КоличествоНаСкладе?.Length == 0)
                {
                    continue;
                }
                else quantity = float.Parse(subItem.КоличествоНаСкладе, CultureInfo.InvariantCulture);

                await _context.VariationStocks.AddAsync(new VariationStock
                {
                    VariationId = splittedItem[1],
                    StorageId = subItem.ИдСклада,
                    Stock = quantity
                }
        );
            }
        }
        await _context.SaveChangesAsync();
    }

    internal async Task PushStoragesToDatabase()
    {
        _context.Storages.RemoveRange(_context.Storages);
        var result = _xmlOffersData.ПакетПредложений.Склады.Склад;

        foreach (var item in result)
        {
            await _context.Storages.AddAsync(new Storage
            {
                Id = item.Ид,
                Name = item.Наименование
            }
          );
        }
        await _context.SaveChangesAsync();
    }
}