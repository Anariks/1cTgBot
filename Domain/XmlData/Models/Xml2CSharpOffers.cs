using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Domain.XmlData.Models;

[XmlRoot(ElementName="Владелец")]
public class ВладелецOffers {
    [XmlElement(ElementName="Ид")]
    public string Ид { get; set; }
    [XmlElement(ElementName="Наименование")]
    public string Наименование { get; set; }
}

[XmlRoot(ElementName="Налог")]
public class Налог {
    [XmlElement(ElementName="Наименование")]
    public string Наименование { get; set; }
    [XmlElement(ElementName="УчтеноВСумме")]
    public string УчтеноВСумме { get; set; }
}

[XmlRoot(ElementName="ТипЦены")]
public class ТипЦены {
    [XmlElement(ElementName="Ид")]
    public string Ид { get; set; }
    [XmlElement(ElementName="Наименование")]
    public string Наименование { get; set; }
    [XmlElement(ElementName="Валюта")]
    public string Валюта { get; set; }
    [XmlElement(ElementName="Налог")]
    public Налог Налог { get; set; }
}

[XmlRoot(ElementName="ТипыЦен")]
public class ТипыЦен {
    [XmlElement(ElementName="ТипЦены")]
    public List<ТипЦены> ТипЦены { get; set; }
}

[XmlRoot(ElementName="Склад")]
public class Склад {
    [XmlElement(ElementName="Ид")]
    public string Ид { get; set; }
    [XmlElement(ElementName="Наименование")]
    public string Наименование { get; set; }
    [XmlAttribute(AttributeName="ИдСклада")]
    public string ИдСклада { get; set; }
    [XmlAttribute(AttributeName="КоличествоНаСкладе")]
    public string КоличествоНаСкладе { get; set; }
}

[XmlRoot(ElementName="Склады")]
public class Склады {
    [XmlElement(ElementName="Склад")]
    public List<Склад> Склад { get; set; }
}

[XmlRoot(ElementName="БазоваяЕдиница")]
public class БазоваяЕдиницаOffers {
    [XmlAttribute(AttributeName="Код")]
    public string Код { get; set; }
    [XmlAttribute(AttributeName="НаименованиеПолное")]
    public string НаименованиеПолное { get; set; }
    [XmlAttribute(AttributeName="МеждународноеСокращение")]
    public string МеждународноеСокращение { get; set; }
    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName="ХарактеристикаТовара")]
public class ХарактеристикаТовара {
    [XmlElement(ElementName="Наименование")]
    public string Наименование { get; set; }
    [XmlElement(ElementName="Значение")]
    public string Значение { get; set; }
}

[XmlRoot(ElementName="ХарактеристикиТовара")]
public class ХарактеристикиТовара {
    [XmlElement(ElementName="ХарактеристикаТовара")]
    public ХарактеристикаТовара ХарактеристикаТовара { get; set; }
}

[XmlRoot(ElementName="Цена")]
public class Цена {
    [XmlElement(ElementName="Представление")]
    public string Представление { get; set; }
    [XmlElement(ElementName="ИдТипаЦены")]
    public string ИдТипаЦены { get; set; }
    [XmlElement(ElementName="ЦенаЗаЕдиницу")]
    public string ЦенаЗаЕдиницу { get; set; }
    [XmlElement(ElementName="Валюта")]
    public string Валюта { get; set; }
    [XmlElement(ElementName="Единица")]
    public string Единица { get; set; }
    [XmlElement(ElementName="Коэффициент")]
    public string Коэффициент { get; set; }
}

[XmlRoot(ElementName="Цены")]
public class Цены {
    [XmlElement(ElementName="Цена")]
    public List<Цена> Цена { get; set; }
}

[XmlRoot(ElementName="Предложение")]
public class Предложение {
    [XmlElement(ElementName="Ид")]
    public string Ид { get; set; }
    [XmlElement(ElementName="Наименование")]
    public string Наименование { get; set; }
    [XmlElement(ElementName="БазоваяЕдиница")]
    public БазоваяЕдиница БазоваяЕдиница { get; set; }
    [XmlElement(ElementName="ХарактеристикиТовара")]
    public ХарактеристикиТовара ХарактеристикиТовара { get; set; }
    [XmlElement(ElementName="Склад")]
    public List<Склад> Склад { get; set; }
    [XmlElement(ElementName="Цены")]
    public Цены Цены { get; set; }
    [XmlElement(ElementName="Количество")]
    public string Количество { get; set; }
}

[XmlRoot(ElementName="Предложения")]
public class Предложения {
    [XmlElement(ElementName="Предложение")]
    public List<Предложение> Предложение { get; set; }
}

[XmlRoot(ElementName="ПакетПредложений")]
public class ПакетПредложений {
    [XmlElement(ElementName="Ид")]
    public string Ид { get; set; }
    [XmlElement(ElementName="Наименование")]
    public string Наименование { get; set; }
    [XmlElement(ElementName="ИдКаталога")]
    public string ИдКаталога { get; set; }
    [XmlElement(ElementName="ИдКлассификатора")]
    public string ИдКлассификатора { get; set; }
    [XmlElement(ElementName="Владелец")]
    public Владелец Владелец { get; set; }
    [XmlElement(ElementName="ТипыЦен")]
    public ТипыЦен ТипыЦен { get; set; }
    [XmlElement(ElementName="Склады")]
    public Склады Склады { get; set; }
    [XmlElement(ElementName="Предложения")]
    public Предложения Предложения { get; set; }
    [XmlAttribute(AttributeName="СодержитТолькоИзменения")]
    public string СодержитТолькоИзменения { get; set; }
}

[XmlRoot(ElementName="КоммерческаяИнформация")]
public class XmlOffersData {
    [XmlElement(ElementName="ПакетПредложений")]
    public ПакетПредложений ПакетПредложений { get; set; }
    [XmlAttribute(AttributeName="ВерсияСхемы")]
    public string ВерсияСхемы { get; set; }
    [XmlAttribute(AttributeName="ДатаФормирования")]
    public string ДатаФормирования { get; set; }
}
