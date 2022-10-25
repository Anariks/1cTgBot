using Domain;
using System.Linq;
using Contracts.Database;
using Microsoft.EntityFrameworkCore;
using Domain.XmlData.Models;
using System.Globalization;
using System.Threading;
using Domain.XmlData;

namespace Domain.Database;
public class DataToDatabase
{
    private readonly BotDbContext _context;
    private readonly XmlImportData _xmlImportData;
    private readonly XmlOffersData _xmlOffersData;

    public DataToDatabase(BotDbContext botDbContext, DataFromXml dataFromXml)
    {
        _context = botDbContext;
        _xmlImportData = dataFromXml.XmlImportTempData;
        var xmlOffersDataTemp = dataFromXml.XmlOffersTempData;

        if (xmlOffersDataTemp != null)
        {
            xmlOffersDataTemp.ПакетПредложений.Предложения.Предложение =
                xmlOffersDataTemp.ПакетПредложений.Предложения.Предложение.
                    Where(x => x.Количество != null).ToList();

            var resultVariations = xmlOffersDataTemp.ПакетПредложений.Предложения.Предложение;

            foreach (var item in resultVariations)
            {
                string[]? itemId = new string[2];

                var splittedItem = item.Ид.Split('#');

                itemId[1] = Guid.NewGuid().ToString();
                item.Ид = splittedItem[0] + "#" + itemId[1];

                if (splittedItem.Count() == 1)
                {
                    item.ХарактеристикиТовара = new ХарактеристикиТовара()
                    {
                        ХарактеристикаТовара = new ХарактеристикаТовара()
                        {
                            Наименование = null,
                            Значение = item.Наименование
                        }
                    };
                }
            }
        }
        _xmlOffersData = xmlOffersDataTemp;
    }

    public async Task ClearDatabase()
    {
        _context.Brands.RemoveRange(_context.Brands);
        _context.Categories.RemoveRange(_context.Categories);
        _context.PriceTypes.RemoveRange(_context.PriceTypes);
        _context.Products.RemoveRange(_context.Products);
        _context.Storages.RemoveRange(_context.Storages);
        _context.Variations.RemoveRange(_context.Variations);
        _context.VariationPrices.RemoveRange(_context.VariationPrices);
        _context.VariationStocks.RemoveRange(_context.VariationStocks);
        await _context.SaveChangesAsync();
        // await _context.Database.ExecuteSqlRawAsync(@"
        //     TRUNCATE tbl_brands;
        //     TRUNCATE tbl_categories;
        //     TRUNCATE tbl_price_types;
        //     TRUNCATE tbl_products;
        //     TRUNCATE tbl_storages;
        //     TRUNCATE tbl_variations;
        //     TRUNCATE tbl_variation_prices;
        //     TRUNCATE tbl_variation_stocks;");

    }

    public async Task PushBrandsToDatabase()
    {
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
        var resultProducts = _xmlImportData.Каталог.Товары.Товар;

        foreach (var item in resultProducts)
        {

            string? brandGuid = (item.Изготовитель != null)
                ? item.Изготовитель.Ид
                : null;

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