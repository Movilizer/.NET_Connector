using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace MWS.Helper
{
    public static class StringHelper
    {
        public static String UTF8ByteArrayToString(Byte[] characters)
        {
            return new UTF8Encoding().GetString(characters);
        }

        public static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            return new UTF8Encoding().GetBytes(pXmlString);
        }

        public static string NormalizeString(string str)
        {
            return str.Trim().Replace("\"", "''").Replace("\r\n", " ");
        }

        public static string ToNoDate(string dateStr)
        {
            return dateStr.Length > 0 ? dateStr : "<kein Datum>";
        }

        public static string ToHexString(byte[] ba)
        {
            // speed optimized
            byte b;
            int i, j, k;
            int l = ba.Length;
            char[] r = new char[l * 2];
            for (i = 0, j = 0; i < l; ++i)
            {
                b = ba[i];
                k = b >> 4;
                r[j++] = (char)(k > 9 ? k + 0x37 : k + 0x30);
                k = b & 15;
                r[j++] = (char)(k > 9 ? k + 0x37 : k + 0x30);
            }
            return new string(r);
        }

        public static byte[] HexToByteArray(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            int bl = bytes.Length;
            for (int i = 0; i < bl; ++i)
            {
                bytes[i] = (byte)((hex[2 * i] > 'F' ? hex[2 * i] - 0x57 : hex[2 * i] > '9' ? hex[2 * i] - 0x37 : hex[2 * i] - 0x30) << 4);
                bytes[i] |= (byte)(hex[2 * i + 1] > 'F' ? hex[2 * i + 1] - 0x57 : hex[2 * i + 1] > '9' ? hex[2 * i + 1] - 0x37 : hex[2 * i + 1] - 0x30);
            }
            return bytes;
        } 

        public static string ToHexStringFromFile(string filePath)
        {
            Stream fileStream = null;

            try
            {
                fileStream = File.OpenRead(filePath);
                return ToHexStringFromStream(fileStream);
            }
            catch (Exception e)
            {
                Console.WriteLine("StringHelper:ToHexStringFromFile exception caught:\nfile: {0}\nexcepion: {1}", filePath, e);
                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
        }

        public static string ToHexStringFromResourceCode(string resourceCode)
        {
            Stream resourceStream = null;

            try
            {
                resourceStream = Assembly.GetEntryAssembly().GetManifestResourceStream(resourceCode);
                return ToHexStringFromStream(resourceStream);
            }
            catch (Exception e)
            {
                Console.WriteLine("StringHelper:ToHexStringFromResourceCode exception caught:\nresourceCode: {0}\nexcepion: {1}", resourceCode, e);
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

        private static string ToHexStringFromStream(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);

            return ToHexString(buffer);
        }

        public static bool IsEmpty(string str)
        {
            return str == null || str.Trim().Length == 0;
        }

        public static string ToNotNull(string str)
        {
            return str != null ? str : "";
        }
    }
}
