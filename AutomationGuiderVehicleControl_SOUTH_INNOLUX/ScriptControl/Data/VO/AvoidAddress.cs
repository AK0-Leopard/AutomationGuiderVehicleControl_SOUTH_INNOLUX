using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.sc.Service;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.Data.VO
{
    public class AvoidAddress
    {
        public AvoidAddress ParentAvoidData;
        public string AddressID;
        public AvoidAddress(string addressID)
        {
            AddressID = addressID;
        }
        public AvoidAddress(AvoidAddress parentAvoidData, string address)
        {
            ParentAvoidData = parentAvoidData;
            AddressID = address;
        }
        public bool isFirst
        {
            get
            {
                return ParentAvoidData == null;
            }
        }

        public (bool isSuccess, List<string> guideSection, List<string> guideAddresses, double distance) tryGetGuideInfo(BLL.SectionBLL sectionBLL, BLL.ReserveBLL reserveBLL, string vhCurrentAdr)
        {
            if (ParentAvoidData == null)
            {
                return (false, null, null, 0);
            }
            List<string> addresses = new List<string>();


            addresses.Add(AddressID);
            AvoidAddress avoidAddressTemp = ParentAvoidData;

            while (!avoidAddressTemp.isFirst)
            {
                addresses.Add(avoidAddressTemp.AddressID);
                avoidAddressTemp = avoidAddressTemp.ParentAvoidData;
            }
            //結束以後，要再加入第一點的Address
            addresses.Add(avoidAddressTemp.AddressID);

            //do
            //{
            //    addresses.Add(avoidAddressTemp.AddressID);
            //    avoidAddressTemp = avoidAddressTemp.ParentAvoidData;
            //    if (avoidAddressTemp.isFirst)
            //    {
            //        addresses.Add(avoidAddressTemp.AddressID);
            //    }
            //} while (!avoidAddressTemp.isFirst);
            addresses.Reverse();
            if (!SCUtility.isMatche(addresses[0], vhCurrentAdr))
            {
                addresses.Insert(0, vhCurrentAdr);
            }

            List<string> sections = new List<string>();
            double distance = 0;
            for (int i = 1; i < addresses.Count; i++)
            {
                string first_adr = addresses[i - 1];
                string second_adr = addresses[i];
                ASECTION sec = sectionBLL.cache.GetSection(first_adr, second_adr);
                if (sec == null)
                {
                    LogHelper.Log(logger: NLog.LogManager.GetCurrentClassLogger(), LogLevel: LogLevel.Warn, Class: nameof(VehicleService), Device: "AGV",
                       Data: $"can't find section ,first adr:{first_adr} second adr:{second_adr} ");
                    return (false, null, null, 0);
                }
                sections.Add(SCUtility.Trim(sec.SEC_ID, true));

                distance += sec.SEC_DIS;

            }
            return (true, sections, addresses, distance);
        }
    }
}
