using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject.REPORT
{
    static public class Define
    {
        //報表顯示用的OHTC_CMS_STATUS狀態整合版
        public enum E_OHTC_CMD_STATUS
        {
            UnDefine = -1,
            Queue = 0,
            Sending = 1,
            Execution = 2,
            Aborting = 3,
            Canceling = 4,
            NormalEnd = 5,
            AbnormalEndByOHT = 6,
            AbnormalEndByMCS = 7,
            AbnormalEndByOHTC = 8,
            CancelEndByOHTC = 9
        }

        //預設的共同轉換Function，如有缺少可以補上，若專案Status定義與其他客戶不同，請獨立寫一隻轉換Function在專案的ObjectConverter中，否則動這隻會讓其他客戶的顯示跑掉
        static public E_OHTC_CMD_STATUS DefineInt_To_EOHTC_CMD_STATUS(int iCmdStatus)
        {
            try
            {
                switch (iCmdStatus)
                {
                    case 0:
                        return E_OHTC_CMD_STATUS.Queue;
                    case 1:
                        return E_OHTC_CMD_STATUS.Sending;
                    case 2:
                        return E_OHTC_CMD_STATUS.Execution;
                    case 3:
                        return E_OHTC_CMD_STATUS.Aborting;
                    case 4:
                        return E_OHTC_CMD_STATUS.Canceling;
                    case 5:
                        return E_OHTC_CMD_STATUS.NormalEnd;
                    case 6:
                        return E_OHTC_CMD_STATUS.AbnormalEndByOHT;
                    case 7:
                        return E_OHTC_CMD_STATUS.AbnormalEndByMCS;
                    case 8:
                        return E_OHTC_CMD_STATUS.AbnormalEndByOHTC;
                    case 9:
                        return E_OHTC_CMD_STATUS.CancelEndByOHTC;
                    case -1:
                        return E_OHTC_CMD_STATUS.UnDefine;
                    default:
                        throw new Exception("E_OHTC_CMD_STATUS not Define iCmdStatus.Value in DefineInt_To_EOHTC_CMD_STATUS");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }      

        }
    }
}
