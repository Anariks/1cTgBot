using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace Domain.XmlData.Models;

[XmlRoot(ElementName="Владелец")]
public class Владелец {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
}

[XmlRoot(ElementName="Группа")]
public class Category1c {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="Группы")]
	public Categories1c Categories1c { get; set; }
}

[XmlRoot(ElementName="Группы")]
public class Categories1c {
	[XmlElement(ElementName="Группа")]
	public List<Category1c> Category1c { get; set; }
	[XmlElement(ElementName="Ид")]
	public List<string> Ид { get; set; }
}

[XmlRoot(ElementName="Свойство")]
public class Свойство {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="ТипЗначений")]
	public string ТипЗначений { get; set; }
	[XmlElement(ElementName="ВариантыЗначений")]
	public ВариантыЗначений ВариантыЗначений { get; set; }
}

[XmlRoot(ElementName="Справочник")]
public class Справочник {
	[XmlElement(ElementName="ИдЗначения")]
	public string ИдЗначения { get; set; }
	[XmlElement(ElementName="Значение")]
	public string Значение { get; set; }
}

[XmlRoot(ElementName="ВариантыЗначений")]
public class ВариантыЗначений {
	[XmlElement(ElementName="Справочник")]
	public List<Справочник> Справочник { get; set; }
}

[XmlRoot(ElementName="Свойства")]
public class Свойства {
	[XmlElement(ElementName="Свойство")]
	public List<Свойство> Свойство { get; set; }
}

[XmlRoot(ElementName="Классификатор")]
public class Классификатор {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="Владелец")]
	public Владелец Владелец { get; set; }
	[XmlElement(ElementName="Группы")]
	public Categories1c Categories1c { get; set; }
	[XmlElement(ElementName="Свойства")]
	public Свойства Свойства { get; set; }
}

[XmlRoot(ElementName="БазоваяЕдиница")]
public class БазоваяЕдиница {
	[XmlAttribute(AttributeName="Код")]
	public string Код { get; set; }
	[XmlAttribute(AttributeName="НаименованиеПолное")]
	public string НаименованиеПолное { get; set; }
	[XmlAttribute(AttributeName="МеждународноеСокращение")]
	public string МеждународноеСокращение { get; set; }
	[XmlText]
	public string Text { get; set; }
}

[XmlRoot(ElementName="Изготовитель")]
public class Изготовитель {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="ОфициальноеНаименование")]
	public string ОфициальноеНаименование { get; set; }
}

[XmlRoot(ElementName="ЗначенияСвойства")]
public class ЗначенияСвойства {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="Значение")]
	public string Значение { get; set; }
}

[XmlRoot(ElementName="ЗначенияСвойств")]
public class ЗначенияСвойств {
	[XmlElement(ElementName="ЗначенияСвойства")]
	public List<ЗначенияСвойства> ЗначенияСвойства { get; set; }
}

[XmlRoot(ElementName="СтавкаНалога")]
public class СтавкаНалога {
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="Ставка")]
	public string Ставка { get; set; }
}

[XmlRoot(ElementName="СтавкиНалогов")]
public class СтавкиНалогов {
	[XmlElement(ElementName="СтавкаНалога")]
	public СтавкаНалога СтавкаНалога { get; set; }
}

[XmlRoot(ElementName="ЗначениеРеквизита")]
public class ЗначениеРеквизита {
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="Значение")]
	public string Значение { get; set; }
}

[XmlRoot(ElementName="ЗначенияРеквизитов")]
public class ЗначенияРеквизитов {
	[XmlElement(ElementName="ЗначениеРеквизита")]
	public List<ЗначениеРеквизита> ЗначениеРеквизита { get; set; }
}

[XmlRoot(ElementName="Товар")]
public class Товар {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="Артикул")]
	public string Артикул { get; set; }
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="БазоваяЕдиница")]
	public БазоваяЕдиница БазоваяЕдиница { get; set; }
	[XmlElement(ElementName="Группы")]
	public Categories1c Categories { get; set; }
	[XmlElement(ElementName="Изготовитель")]
	public Изготовитель Изготовитель { get; set; }
	[XmlElement(ElementName="ЗначенияСвойств")]
	public ЗначенияСвойств ЗначенияСвойств { get; set; }
	[XmlElement(ElementName="СтавкиНалогов")]
	public СтавкиНалогов СтавкиНалогов { get; set; }
	[XmlElement(ElementName="ЗначенияРеквизитов")]
	public ЗначенияРеквизитов ЗначенияРеквизитов { get; set; }
	[XmlElement(ElementName="Описание")]
	public string Описание { get; set; }
}

[XmlRoot(ElementName="Товары")]
public class Товары {
	[XmlElement(ElementName="Товар")]
	public List<Товар> Товар { get; set; }
}

[XmlRoot(ElementName="Каталог")]
public class Каталог {
	[XmlElement(ElementName="Ид")]
	public string Ид { get; set; }
	[XmlElement(ElementName="ИдКлассификатора")]
	public string ИдКлассификатора { get; set; }
	[XmlElement(ElementName="Наименование")]
	public string Наименование { get; set; }
	[XmlElement(ElementName="Владелец")]
	public Владелец Владелец { get; set; }
	[XmlElement(ElementName="Товары")]
	public Товары Товары { get; set; }
	[XmlAttribute(AttributeName="СодержитТолькоИзменения")]
	public string СодержитТолькоИзменения { get; set; }
}

[XmlRoot(ElementName="КоммерческаяИнформация")]
public class XmlImportData {
	[XmlElement(ElementName="Классификатор")]
	public Классификатор Классификатор { get; set; }
	[XmlElement(ElementName="Каталог")]
	public Каталог Каталог { get; set; }
	[XmlAttribute(AttributeName="ВерсияСхемы")]
	public string ВерсияСхемы { get; set; }
	[XmlAttribute(AttributeName="ДатаФормирования")]
	public string ДатаФормирования { get; set; }
}