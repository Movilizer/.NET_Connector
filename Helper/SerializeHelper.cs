using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MWS
{
    public static class SerializeHelper
    {
        // deserialize from XML stream
        public static T DeserializeXML<T>(Stream xmlDataStream) where T : new()
        {
            if (xmlDataStream == null)
                return default(T);

            T DocItms = new T();

            XmlSerializer xms = new XmlSerializer(DocItms.GetType());
            DocItms = (T)xms.Deserialize(xmlDataStream);

            return DocItms == null ? default(T) : DocItms;
        }

        // deserialize from XML string
        public static T DeserializeXML<T>(string xmlData) where T : new()
        {
            if (string.IsNullOrEmpty(xmlData))
                return default(T);

            TextReader tr = new StringReader(xmlData);
            T DocItms = new T();

            XmlSerializer xms = new XmlSerializer(DocItms.GetType());
            DocItms = (T)xms.Deserialize(tr);

            return DocItms == null ? default(T) : DocItms;
        }

        public static string SeralizeObjectToXML<T>(T xmlObject)
        {
            StringBuilder sbTR = new StringBuilder();
            XmlSerializer xmsTR = new XmlSerializer(xmlObject.GetType());

            XmlWriterSettings xwsTR = new XmlWriterSettings();
            XmlWriter xmwTR = XmlWriter.Create(sbTR, xwsTR);
            xmsTR.Serialize(xmwTR, xmlObject);

            return sbTR.ToString();
        }

        public static T CloneObject<T>(T objClone) where T : new()
        {
            string GetString = SeralizeObjectToXML<T>(objClone);
            return DeserializeXML<T>(GetString);
        }
    }
}
