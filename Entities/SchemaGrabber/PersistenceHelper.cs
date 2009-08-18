using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SchemaGrabber
{
    public class PersistenceHelper
    {
        public static PersistenceSchema GetSchema(string xmlfilepath)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PersistenceSchema));
            return (PersistenceSchema)xmlSerializer.Deserialize(System.IO.File.OpenRead(xmlfilepath));
        }

        public static void WriteSchema(string xmlfilepath, PersistenceSchema schema)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PersistenceSchema));
            xmlSerializer.Serialize(System.IO.File.Create(xmlfilepath), schema);
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlRootAttribute("persistenceschema")]
    public class PersistenceSchema
    {
        public PersistenceSchema()
        {
            this.Tables = new List<PersistenceTable>();
        }
        [System.Xml.Serialization.XmlElementAttribute("table")]
        public List<PersistenceTable> Tables { get; set; }
    }

    [System.SerializableAttribute()]
    public class PersistenceTable
    {
	    public PersistenceTable()
	    {
		    this.Fields = new List<PersistenceField>();
	    }
        [System.Xml.Serialization.XmlAttribute("name")]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlAttribute("entityname")]
        public string EntityName { get; set; }
        [System.Xml.Serialization.XmlAttribute("schema")]
        public string Schema { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("field")]
        public List<PersistenceField> Fields { get; set; }
    }

    [System.SerializableAttribute()]
    public class PersistenceField
    {
        [System.Xml.Serialization.XmlAttribute("name")]
        public string Name { get; set; }
        [System.Xml.Serialization.XmlAttribute("propertyname")]
        public string PropertyName { get; set; }
        [System.Xml.Serialization.XmlAttribute("dotnettype")]
        public string DotNetType { get; set; }
        [System.Xml.Serialization.XmlAttribute("dbtype")]
        public string DbType { get; set; }
        [System.Xml.Serialization.XmlAttribute("identity")]
        public string Identity { get; set; }
        [System.Xml.Serialization.XmlAttribute("primarykey")]
        public string PrimaryKey { get; set; }
        [System.Xml.Serialization.XmlAttribute("foreignkey")]
        public string ForeignKey { get; set; }
        [System.Xml.Serialization.XmlAttribute("readonly")]
        public string Readonly { get; set; }
        [System.Xml.Serialization.XmlAttribute("nullable")]
        public string Nullable { get; set; }
        [System.Xml.Serialization.XmlAttribute("length")]
        public string Length { get; set; }
        [System.Xml.Serialization.XmlAttribute("scale")]
        public string Scale { get; set; }
        [System.Xml.Serialization.XmlAttribute("precision")]
        public string Precision { get; set; }
    }
}
