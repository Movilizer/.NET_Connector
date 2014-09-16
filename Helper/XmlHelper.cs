using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

using MWS.Log;

namespace MWS.Helper
{
    public static class XmlHelper
    {
        /**
         * SERIALIZATION
         */
        public static void Serialize(TextWriter writer, object attribute)
        {
            XmlSerializer serializer = new XmlSerializer(attribute.GetType());
            serializer.Serialize(writer, attribute);
        }

        public static string SerializeToString(object attribute)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(attribute.GetType());

            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            // we want the output formatted
            xmlTextWriter.Formatting = Formatting.Indented;

            serializer.Serialize(xmlTextWriter, attribute);

            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            return StringHelper.UTF8ByteArrayToString(memoryStream.ToArray());
        }

        public static void SerializeToConsole(object attribute)
        {
            Console.WriteLine(SerializeToString(attribute));
        }

        public static void SerializeToFile(string filename, object attribute)
        {
            XmlSerializer serializer = new XmlSerializer(attribute.GetType());
            XmlTextWriter textWriter = null;

            try
            {
                textWriter = new XmlTextWriter(filename, Encoding.UTF8);

                // we want the output formatted
                textWriter.Formatting = Formatting.Indented;

                serializer.Serialize(textWriter, attribute);
            }
            catch (Exception e)
            {
                LogFactory.WriteWarning(e.ToString());
            }
            finally
            {
                if (textWriter != null)
                {
                    textWriter.Flush();
                    textWriter.Close();
                }
            }
        }


        /**
         * DESERIALIZATION
         */
        public static object DeserializeFromFile(string filename, Type attrType, string defaultNameSpace)
        {             
            XmlSerializer serializer = new XmlSerializer(attrType, defaultNameSpace);
            XmlTextReader textReader = null;

            try
            {
                textReader = new XmlTextReader(filename);
                return serializer.Deserialize(textReader);
            }
            catch (Exception e)
            {
                Console.WriteLine("XmlHelper:DeserializeFromFile exception caught:\nfilename: {0}\nexcepion: {1}", filename, e);
                throw;
            }
            finally
            {
                if (textReader != null)
                {
                    textReader.Close();
                }
            }
        }

        public static object DeserializeFromResourceCode(string resourceCode, Type attrType, string defaultNameSpace)
        {
            XmlSerializer serializer = new XmlSerializer(attrType, defaultNameSpace);
            Stream resourceStream = null;

            try
            {
                resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceCode);
                return serializer.Deserialize(resourceStream);
            }
            catch (Exception e)
            {
                Console.WriteLine("XmlHelper:DeserializeFromResourceCode exception caught:\nresourceCode: {0}\nexcepion: {1}", resourceCode, e);
                throw;
            }
            finally
            {
                if (resourceStream != null)
                {
                    resourceStream.Flush();
                    resourceStream.Close();
                }
            }
        }

        public static object DeserializeFromString(string str, Type attrType, string defaultNameSpace)
        {
            XmlSerializer serializer = new XmlSerializer(attrType, defaultNameSpace);

            try
            {
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(str));
                return serializer.Deserialize(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine("XmlHelper:DeserializeFromString exception caught:\nstring: {0}\nexcepion: {1}", str, e);
                throw;
            }
        }

        public static object DeserializeFromStream(Stream stream, Type attrType, string defaultNameSpace)
        {
            XmlSerializer serializer = new XmlSerializer(attrType, defaultNameSpace);

            try
            {
                return serializer.Deserialize(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine("XmlHelper:DeserializeFromStream exception caught:\nexcepion: {0}", e);
                throw;
            }
        }

        /**
         * NAMESPACE
         */
        public static string GetXmlNamespace(string s)
        {
            XPathDocument x = new XPathDocument(new StringReader(s));
            XPathNavigator foo = x.CreateNavigator();
            foo.MoveToFollowing(XPathNodeType.Element);
            IDictionary<string, string> whatever = foo.GetNamespacesInScope(XmlNamespaceScope.All);

            return (string)whatever["xmlns"];
        }
    }
}
