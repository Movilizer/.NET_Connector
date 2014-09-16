using System;
using System.IO;

using MWS.Log;

namespace MWS.Helper
{
    public static class FileHelper
    {
        public static void WriteByteArrayToFile(string fileName, byte[] byteArray)
        {
            FileStream fStream = null;

            try
            {
                // open file for writing
                fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                // writes a block of bytes to this stream using data from a byte array.
                fStream.Write(byteArray, 0, byteArray.Length);
            }
            catch (Exception e)
            {
                LogFactory.WriteError(e.ToString());
            }
            finally
            {
                // close the file stream
                if (fStream != null)
                {
                    fStream.Flush();
                    fStream.Close();
                }
            }
        }

        public static byte[] ReadFileToByteArray(string fileName)
        {
            FileStream fs = null;

            try
            {
                // open file for reading
                byte[] buff = null;
                fs = new FileStream(fileName, 
                                               FileMode.Open, 
                                               FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(fileName).Length;
                buff = br.ReadBytes((int) numBytes);
                return buff;
            }
            catch (Exception e)
            {
                LogFactory.WriteError(e.ToString());
                return null;
            }
            finally
            {
                // close the file stream
                if (fs != null)
                {
                    fs.Flush();
                    fs.Close();
                }
            }
        }

    }
}
