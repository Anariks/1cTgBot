using Domain.Database;
using Domain.XmlData;
using Domain.XmlData.Models;
using System.Linq;

namespace Domain.Services;
public class DatabaseService
{
    private readonly DataToDatabase _dataToDatabase;

    public DatabaseService(DataToDatabase dataToDatabase)
    {
        _dataToDatabase = dataToDatabase;
    }

    public async Task FillDatabase()
    {
        //await _dataToDatabase.ClearDatabase();
        await _dataToDatabase.PushBrandsToDatabase();
        await _dataToDatabase.PushCategoriesToDatabase();
        await _dataToDatabase.PushProductsToDatabase();
        await _dataToDatabase.PushPriceTypesToDatabase();
        await _dataToDatabase.PushVariationPricesToDatabase();
        await _dataToDatabase.PushStoragesToDatabase();
        await _dataToDatabase.PushVariationStocksToDatabase();
        await _dataToDatabase.PushVariationsToDatabase();
    }
}