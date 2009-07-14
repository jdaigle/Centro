using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace OpenEntity.Helpers
{
    public static class Serializer
    {
        /// <summary>
        /// Converts an object of a generic type to an XML string.
        /// </summary>
        public static string ConvertToXml<TObject>(this object objectToConvert)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(TObject));
            StringWriter writer = new StringWriter();
            xmlserializer.Serialize(writer, objectToConvert);
            string xmlstring = writer.ToString();
            writer.Close();
            return xmlstring;
        }

        /// <summary>
        /// Converts a serialized string back to a generic object.
        /// </summary>
        public static TObject ConvertFromXml<TObject>(this string xmlString)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(TObject));
            StringReader reader = new StringReader(xmlString);
            TObject deserializedObject = (TObject)xmlserializer.Deserialize(reader);
            reader.Close();
            return deserializedObject;
        }

    }
}
