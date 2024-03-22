using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using System.IO;

namespace com.mirle.ibg3k0.ohxc.wpf.Common
{
    public class ElasticSearchManager
    {

        public const string ELASTIC_URL = "elastic.viewer.mirle.com.tw";
        //[LinkObject(nameof(Service.ASYSEXCUTEQUALITY) , nameof(sysexcutequality))] 
        //public const string ELASTIC_TABLE_INDEX_SYSEXCUTEQUALITY = "mfoht100-ohtc1-sysexcutequality";
        public string ELASTIC_TABLE_INDEX_SYSEXCUTEQUALITY { get; private set; }

        //[LinkObject(nameof(RecordReportInfo))]
        //public const string ELASTIC_TABLE_INDEX_RECODEREPORTINFO = "ase-line1-recodereportinfo";
        public string ELASTIC_TABLE_INDEX_RECODEREPORTINFO { get; private set; }

        //[LinkObject(nameof(PortDef))]
        //public const string ELASTIC_TABLE_INDEX_PLCHistory = "mfoht100-ohtc1-plchistory";
        public string ELASTIC_TABLE_INDEX_PLCHistory { get; private set; }

        //[LinkObject(nameof(AVEHICLE))]
        //public const string ELASTIC_TABLE_INDEX_OHTHistory = "mfoht100-ohtc1-ohthistory";
        public string ELASTIC_TABLE_INDEX_OHTHistory { get; private set; }

        //[LinkObject(nameof(ACMD_DETAIL))]
        //public const string ELASTIC_TABLE_INDEX_TaskCommandHistory = "mfoht100-ohtc1-taskcommandhistory";
        public string ELASTIC_TABLE_INDEX_TaskCommandHistory { get; private set; }

  //      [LinkObject(nameof(sc.Data.VO.UserOperationLog))]
		////public const string ELASTIC_TABLE_INDEX_UserOperationLog = "ase-line1-operationinfo";
  //      public string ELASTIC_TABLE_INDEX_UserOperationLog { get; private set; }

        #region init
        private readonly XmlDocument xmlDocument = new XmlDocument();
        private string xmlPath;
        private string SettingPath = @"\Config\ElasticSettings.config";

        public ElasticSearchManager()
        {
            xmlPath = Directory.GetCurrentDirectory() + SettingPath;
            if (!File.Exists(xmlPath))
            {
                System.Windows.MessageBox.Show("Load ElasticSearch config fail.");
                return;
            }
            loadConfig();
        }
        private void loadConfig()
        {
            
            xmlDocument.Load(xmlPath);
            XmlElement urlTemp = (XmlElement)xmlDocument.SelectSingleNode("ElasticSearchTable/url");
            //ELASTIC_URL = urlTemp?.GetAttribute("ELASTIC_URL");

            XmlNodeList nodeList = xmlDocument.SelectNodes("ElasticSearchTable/config");

            foreach (XmlNode node in nodeList)
            {
                //2020.07.14 TODO 讀取config
                string attrValue = node.Attributes["item"].Value;
                string attrInner = node.InnerText;

                switch (attrValue)
                {
                    case "ELASTIC_TABLE_INDEX_SYSEXCUTEQUALITY":
                        ELASTIC_TABLE_INDEX_SYSEXCUTEQUALITY = attrInner;
                        break;
                    case "ELASTIC_TABLE_INDEX_RECODEREPORTINFO":
                        ELASTIC_TABLE_INDEX_RECODEREPORTINFO = attrInner;
                        break;
                    case "ELASTIC_TABLE_INDEX_PLCHistory":
                        ELASTIC_TABLE_INDEX_PLCHistory = attrInner;
                        break;
                    case "ELASTIC_TABLE_INDEX_OHTHistory":
                        ELASTIC_TABLE_INDEX_OHTHistory = attrInner;
                        break;
                    case "ELASTIC_TABLE_INDEX_TaskCommandHistory":
                        ELASTIC_TABLE_INDEX_TaskCommandHistory = attrInner;
                        break;
                    //case "ELASTIC_TABLE_INDEX_UserOperationLog":
                    //    ELASTIC_TABLE_INDEX_UserOperationLog = attrInner;
                    //    break;
                }
            }
        }
        #endregion

        public List<T> Search<T>(DateRangeQuery dq, TermsQuery[] tsqs, string[] includes_column, int start_index, int each_search_size)
            where T : class, new()
        {
            T t = new T();
            var node = new Uri($"http://{ELASTIC_URL}:9200");
            var settings = new ConnectionSettings(node).DefaultIndex("default");
            settings.DisableDirectStreaming();
            var client = new ElasticClient(settings);

            //var index = GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            var index = GetType().GetProperties();
            var TName = t.GetType().Name;
            var index_tabel = index.Where(x => (x.GetCustomAttribute(typeof(LinkObject), false) as LinkObject).type.Contains(TName))
                .FirstOrDefault()?.GetValue(this);

            SearchRequest sr = new SearchRequest($"{index_tabel}*");
            sr.From = start_index;
            sr.Size = each_search_size;

            if (tsqs != null)
            {
                foreach (var tsq in tsqs)
                {
                    if (tsq != null)
                        sr.Query &= tsq;
                }
            }
            sr.Query &= dq;
            sr.Source = new SourceFilter()
            {
                Includes = includes_column,
            };
            var result = client.Search<T>(sr);
            return result.Documents.ToList();
        }

        public List<T> Search<T>(DateRangeQuery dq, TermsQuery[] tsqs, int start_index, int each_search_size)
           where T : class, new()
        {
            T t = new T();
            var node = new Uri($"http://{ELASTIC_URL}:9200");
            var settings = new ConnectionSettings(node).DefaultIndex("default");
            settings.DisableDirectStreaming();
            var client = new ElasticClient(settings);

            //var index = GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            var index = GetType().GetProperties();
            var TName = t.GetType().Name;
            var index_tabel = index.Where(x => ((x.GetCustomAttribute(typeof(LinkObject), false) as LinkObject)?.type ?? new string[] { "" }).Contains(TName))
                .FirstOrDefault()?.GetValue(this);
            if (string.IsNullOrEmpty((string)index_tabel))
            {
                return null;
            }

            SearchRequest sr = new SearchRequest($"{index_tabel}*");
            sr.From = start_index;
            sr.Size = each_search_size;
            var tmpPropertiesAry = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (tsqs != null)
            {
                foreach (var tsq in tsqs)
                {
                    if (tsq != null)
                        sr.Query &= tsq;
                }
            }
			if (dq != null)
			{
                sr.Query &= dq;
			}
            sr.Source = new SourceFilter()
            {
                Includes = tmpPropertiesAry,
            };
            var result = client.Search<T>(sr);
            return result.Documents.ToList();
        }

        public bool insertData<T>(T item) where T : class
        {
            var node = new Uri($"http://{ELASTIC_URL}:9200");
            var settings = new ConnectionSettings(node).DefaultIndex("default");
            settings.DisableDirectStreaming();
            var client = new ElasticClient(settings);

            //var index = GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            var index = GetType().GetProperties();
            var TName = item.GetType().Name;
            var index_tabel = index.Where(x => ((x.GetCustomAttribute(typeof(LinkObject), false) as LinkObject)?.type ?? new string[] {""}).Contains(TName))
                .FirstOrDefault()?.GetValue(this);
            if(string.IsNullOrEmpty((string)index_tabel))
            {
                return false;
            }
            client.Index(item, i => i.Index($"{index_tabel}"));
            return true;
        }
    }

    class LinkObject : Attribute
    {
        public string[] type;
        public LinkObject(params string[] _type)
        {
            if(_type == null)
            {
                type = new string[] { "" };
                return;
            }
            type = new string[_type.Length];
            type = _type;
        }
    }
}
