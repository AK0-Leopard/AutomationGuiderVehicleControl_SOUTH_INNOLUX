using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using Newtonsoft.Json;
using ObjectConverterInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectConverter_OHTC_AT_S_MALASYIA
{
    public class WebApiIp
    {
        public String Line_ID { set; get; }
        public String NANCY_IP { set; get; }
        public String ELASTIC_IP { set; get; }
        public String NATS_IP { set; get; }
    }
}
