using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFunction
{
    public static class Compress
    {
        /// <summary>
        /// 壓縮ByteArray資料
        /// </summary>
        /// <param name="arrayByte"></param>
        /// <returns></returns>
        public static string Compress_ArrayByte2String(byte[] arrayByte)
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress, true);
                compressedzipStream.Write(arrayByte, 0, arrayByte.Length);
                compressedzipStream.Close();
                string compressStr = (string)(Convert.ToBase64String(ms.ToArray()));
                return compressStr;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 解壓縮BytyArray資料
        /// </summary>
        /// <param name="compressString">The compress string.</param>
        /// <returns>System.String.</returns>
        public static byte[] Uncompress_String2ArrayByte(string compressString)
        {
            try
            {
                byte[] zippedData = Convert.FromBase64String(compressString.ToString());
                System.IO.MemoryStream ms = new System.IO.MemoryStream(zippedData);
                System.IO.Compression.GZipStream compressedzipStream = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Decompress);
                System.IO.MemoryStream outBuffer = new System.IO.MemoryStream();
                byte[] block = new byte[1024];
                while (true)
                {
                    int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                    if (bytesRead <= 0)
                        break;
                    else
                        outBuffer.Write(block, 0, bytesRead);
                }
                compressedzipStream.Close();
                return outBuffer.ToArray();
            }
            catch
            {
                return null;
            }
        }
    }
}
