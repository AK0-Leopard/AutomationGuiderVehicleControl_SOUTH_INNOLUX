//#define IS_FOR_OHTC_NOT_AGVC // 若對應AGVC，則註解此行

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.ohxc.wpf.Common
{
    public class WebClientManager
    {
        private static readonly string control_web_address = "http://ohxcv.ha.ohxc.mirle.com.tw";
        private static string control_port = "3280";
        public static string OHxC_CONTROL_URI => $"{control_web_address}:{control_port}";
        //public static string OHxC_CONTROL_URI = "http://ohxcv.ha.ohxc.mirle.com.tw:3280"; // OHxC
        //public static string OHxC_CONTROL_URI = "http://ohxcv.ha.ohxc.mirle.com.tw:3281"; // AGVC
        public static string OHxC_SYSEXCUTEQUALITY_URI = "http://ohxc.query.mirle.com.tw:5000";

        public enum HTTP_METHOD
        {
            GET,
            POST,
            DELET,
            PUT,
            PATCH
        }

        ConcurrentDictionary<string, HttpWebRequest> httpWebRequests = null;


        private static Object _lock = new Object();
        private static WebClientManager manager;
        public static WebClientManager getInstance()
        {
            if (manager == null)
            {
                lock (_lock)
                {
                    if (manager == null)
                    {
                        manager = new WebClientManager();
                    }
                }
            }
            return manager;
        }

        private WebClientManager()
        {
            httpWebRequests = new ConcurrentDictionary<string, HttpWebRequest>();
            //httpWebRequest = (HttpWebRequest)WebRequest.Create("http://127.0.0.1:3280");
        }

        public void SetControlPort(string port)
        {
            if (!string.IsNullOrWhiteSpace(port))
                control_port = port;
        }

        public string GetInfoFromServer(string uri, string[] action_targets, string param)
        {
            try
            {
                string result = string.Empty;
                string action_target = string.Join("/", action_targets);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uri}/{action_target}/{param}");
                httpWebRequest.Method = HTTP_METHOD.GET.ToString();
                //指定 request 的 content type
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    httpResponse.Close();
                }
                httpWebRequest.Abort();
                return result;
            }
            catch
            {
                return "";
            }
        }


        public System.Drawing.Image GetImageFromServerByCondition(string uri, string[] conditions, string param)
        {
            try
            {
                string result = string.Empty;
                string condition = string.Join("&", conditions);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uri}/get_image?{condition}");
                httpWebRequest.Method = HTTP_METHOD.GET.ToString();
                //指定 request 的 content type
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                System.Drawing.Image img;
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        img = System.Drawing.Image.FromStream(streamReader.BaseStream);
                    }
                    httpResponse.Close();
                }
                httpWebRequest.Abort();
                return img;
            }
            catch
            {
                return null;
            }
        }

        public string PostInfoToServer(string uri, string[] action_targets, HTTP_METHOD methed, byte[] byteArray)
        {
            string result = string.Empty;
            string action_target = string.Join("/", action_targets);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uri}/{action_target}");
            httpWebRequest.Method = methed.ToString();
            httpWebRequest.ContentLength = byteArray.Length;
            //指定 request 的 content type
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            //httpWebRequest.Timeout = 10000;
            try
            {
                using (Stream reqStream = httpWebRequest.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                    reqStream.Close();
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    httpResponse.Close();
                }
                httpWebRequest.Abort();

                return result;
            }
            catch
            {
                return "Sever no response";
            }
        }

        public string PostInfoToServer(string uri, string action_target, HTTP_METHOD methed, byte[] byteArray)
        {
            string result = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uri}/{action_target}");
            httpWebRequest.Method = methed.ToString();
            httpWebRequest.ContentLength = byteArray.Length;
            //指定 request 的 content type
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            //httpWebRequest.Timeout = 10000;
            try
            {
                using (Stream reqStream = httpWebRequest.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                    reqStream.Close();
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    httpResponse.Close();
                }
                httpWebRequest.Abort();

                return result;
            }
            catch (Exception ex)
            {
                return "Sever no response";
            }
        }

        public string PostInfoToServer(string uri, string[] action_targets, HTTP_METHOD methed)
        {
            string result = string.Empty;
            string action_target = string.Join("/", action_targets);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uri}/{action_target}");
            httpWebRequest.Method = methed.ToString();
            //httpWebRequest.ContentLength = byteArray.Length;
            //指定 request 的 content type
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            //httpWebRequest.Timeout = 10000;
            try
            {
                using (Stream reqStream = httpWebRequest.GetRequestStream())
                {
                    //reqStream.Write(byteArray, 0, byteArray.Length);
                    reqStream.Close();
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    httpResponse.Close();
                }
                httpWebRequest.Abort();

                return result;
            }
            catch
            {
                return "Sever no response";
            }
        }

        public string PostJsonToServer(string uri, string[] action_targets, HTTP_METHOD methed, byte[] byteArray)
        {
            try
            {
                string result = string.Empty;
                string action_target = string.Join("/", action_targets);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create($"{uri}/{action_target}");
                httpWebRequest.Method = methed.ToString();
                httpWebRequest.ContentLength = byteArray.Length;
                //指定 request 的 content type
                httpWebRequest.ContentType = "application/json";

                using (Stream reqStream = httpWebRequest.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                    reqStream.Close();
                }
                using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    httpResponse.Close();
                }
                httpWebRequest.Abort();
                return result;
            }
            catch
            {
                return "Sever no response";
            }
        }



        private HttpWebRequest getWebRequest(string[] action_targets)
        {
            string action_target = string.Join("/", action_targets);
            return httpWebRequests.GetOrAdd(action_target, (HttpWebRequest)WebRequest.Create($"http://ohxc2.ohxc.mirle.com.tw:3280/{action_target}"));

        }
    }
}
