using Domain;
using System.Linq;
using Contracts.Database;
using Microsoft.EntityFrameworkCore;
using Domain.XmlData.Models;
using System.Globalization;
using System.Threading;
using Domain.XmlData;

namespace Domain.Services;
public class OptimizeDataFromXml
{
    private readonly RawDataFromXml _rawDataFromXml;

    public OptimizeDataFromXml(RawDataFromXml rawDataFromXml)
    {
        _rawDataFromXml = rawDataFromXml;
    }

    public XmlImportData GetOptimizedImportData()
    {
        return _rawDataFromXml.XmlImportRawData;
    }

    public XmlOffersData GetOptimizedOffersData()
    {
        var xmlOffersDataTemp = _rawDataFromXml.XmlOffersRawData;
        if (xmlOffersDataTemp == null)
        {
            return null;
        }

        xmlOffersDataTemp.ПакетПредложений.Предложения.Предложение =
            xmlOffersDataTemp.ПакетПредложений.Предложения.Предложение.
                Where(x => x.Количество != null).ToList();

        var resultVariations = xmlOffersDataTemp.ПакетПредложений.Предложения.Предложение;

        foreach (var item in resultVariations)
        {
            var splittedItem = item.Ид.Split('#');

            var itemId = Guid.NewGuid().ToString();
            item.Ид = splittedItem[0] + "#" + itemId;

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

        return xmlOffersDataTemp;
    }
}