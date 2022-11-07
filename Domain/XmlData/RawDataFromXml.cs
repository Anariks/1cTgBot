using System.Xml.Serialization;
using System;
using Domain.XmlData.Models;
using Domain.Services;
using Microsoft.Extensions.Logging;

namespace Domain.XmlData;

public class RawDataFromXml
{
    public readonly XmlImportData XmlImportRawData;
    public readonly XmlOffersData XmlOffersRawData;
    public const string DirPath = "Uploads\\";
    //private readonly ILogger<DataFromXml> _logger;

    public RawDataFromXml()
    {   
        XmlImportRawData = DeserializeImportXmlToObject();
        XmlOffersRawData = DeserializeOffersXmlToObject();
       // _logger = logger;
    }

    private XmlImportData DeserializeImportXmlToObject()
    {   
        var fullImportPath = Path.Combine(DirPath, "import.xml");

        var xmlSerializer = new XmlSerializer(typeof(XmlImportData));
        if(!File.Exists(fullImportPath))
        {
            //_logger.LogWarning("No such file Import.xml file by address {1}", fullImportPath);
            return null;
        }
        using (var reader  = new StreamReader(fullImportPath)) 
        {
            return (XmlImportData)xmlSerializer.Deserialize(reader);
        } 
    }

    private XmlOffersData DeserializeOffersXmlToObject()
    {
        var xmlSerializer = new XmlSerializer(typeof(XmlOffersData));
        
        var fullOffersPath = Path.Combine(DirPath, "offers.xml");
        if(!File.Exists(fullOffersPath))
        {
           //_logger.LogWarning("No such Offers.xml file by address {1}", fullOffersPath);
            return null;
        }

        using (var reader  = new StreamReader(fullOffersPath)) 
        {
            return (XmlOffersData)xmlSerializer.Deserialize(reader);
        } 
    } 
}
