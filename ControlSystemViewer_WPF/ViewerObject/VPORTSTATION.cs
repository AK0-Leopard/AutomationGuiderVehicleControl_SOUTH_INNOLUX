using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerObject
{
    public class VPORTSTATION : ViewerObjectBase
    {
        public event EventHandler StatusChange;
        public void onStatusChange()
        {
            StatusChange?.Invoke(this, null);
        }

        public VPORTSTATION(string portID, string adrID, int stageCount, string unitType = "", string zoneName = "")
        {
            PORT_ID = portID?.Trim() ?? "";
            ADR_ID = adrID?.Trim() ?? "";
            COUNT_STAGE = stageCount;
            UNIT_TYPE = unitType?.Trim() ?? "";
            ZONE_NAME = zoneName?.Trim() ?? "";
        }

        public string PORT_ID { get; private set; } = "";
        public string ADR_ID { get; private set; } = "";
        public string UNIT_TYPE { get; private set; } = "";
        public string ZONE_NAME { get; private set; } = "";
        public bool IS_MONITORING { get; set; } = false;

        private VPORTSTATION_Def.PortStatus pORT_STATUS = VPORTSTATION_Def.PortStatus.Down;
        public VPORTSTATION_Def.PortStatus PORT_STATUS
        {
            get { return pORT_STATUS; }
            set
            {
                if (pORT_STATUS != value)
                {
                    pORT_STATUS = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_IN_SERVICE = false;
        public bool IS_IN_SERVICE
        {
            get { return iS_IN_SERVICE; }
            set
            {
                if (iS_IN_SERVICE != value)
                {
                    iS_IN_SERVICE = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_INPUT_MODE = false;
        public bool IS_INPUT_MODE
        {
            get { return iS_INPUT_MODE; }
            set
            {
                if (iS_INPUT_MODE != value)
                {
                    iS_INPUT_MODE = value;
                    OnPropertyChanged();
                }
            }
        }

        private int cOUNT_STAGE = 0;
        public int COUNT_STAGE
        {
            get { return cOUNT_STAGE; }
            set
            {
                if (cOUNT_STAGE != value)
                {
                    cOUNT_STAGE = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool iS_PRESENCE_LOC_1 = false;
        public bool IS_PRESENCE_LOC_1
        {
            get { return iS_PRESENCE_LOC_1; }
            set
            {
                if (iS_PRESENCE_LOC_1 != value)
                {
                    iS_PRESENCE_LOC_1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_PRESENCE_LOC_2 = false;
        public bool IS_PRESENCE_LOC_2
        {
            get { return iS_PRESENCE_LOC_2; }
            set
            {
                if (iS_PRESENCE_LOC_2 != value)
                {
                    iS_PRESENCE_LOC_2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_PRESENCE_LOC_3 = false;
        public bool IS_PRESENCE_LOC_3
        {
            get { return iS_PRESENCE_LOC_3; }
            set
            {
                if (iS_PRESENCE_LOC_3 != value)
                {
                    iS_PRESENCE_LOC_3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_PRESENCE_LOC_4 = false;
        public bool IS_PRESENCE_LOC_4
        {
            get { return iS_PRESENCE_LOC_4; }
            set
            {
                if (iS_PRESENCE_LOC_4 != value)
                {
                    iS_PRESENCE_LOC_4 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_PRESENCE_LOC_5 = false;
        public bool IS_PRESENCE_LOC_5
        {
            get { return iS_PRESENCE_LOC_5; }
            set
            {
                if (iS_PRESENCE_LOC_5 != value)
                {
                    iS_PRESENCE_LOC_5 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_PRESENCE_LOC_6 = false;
        public bool IS_PRESENCE_LOC_6
        {
            get { return iS_PRESENCE_LOC_6; }
            set
            {
                if (iS_PRESENCE_LOC_6 != value)
                {
                    iS_PRESENCE_LOC_6 = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool iS_PRESENCE_LOC_7 = false;
        public bool IS_PRESENCE_LOC_7
        {
            get { return iS_PRESENCE_LOC_7; }
            set
            {
                if (iS_PRESENCE_LOC_7 != value)
                {
                    iS_PRESENCE_LOC_7 = value;
                    OnPropertyChanged();
                }
            }
        }

        private string[] cst_id = { "", "", "", "", "", "", "", "" };
        public string CST_ID // backup display if CST_ID_LOC_X = empty while IS_PRESENCE_LOC_X = true
        {
            get { return cst_id[0]; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[0] != new_cst_id)
                {
                    cst_id[0] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }
        public string CST_ID_LOC_1
        {
            get { return IS_PRESENCE_LOC_1 ? !string.IsNullOrWhiteSpace(cst_id[1]) ? cst_id[1] : CST_ID : ""; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[1] != new_cst_id)
                {
                    cst_id[1] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }
        public string CST_ID_LOC_2
        {
            get { return IS_PRESENCE_LOC_2 ? !string.IsNullOrWhiteSpace(cst_id[2]) ? cst_id[2] : CST_ID : ""; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[2] != new_cst_id)
                {
                    cst_id[2] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }
        public string CST_ID_LOC_3
        {
            get { return IS_PRESENCE_LOC_3 ? !string.IsNullOrWhiteSpace(cst_id[3]) ? cst_id[3] : CST_ID : ""; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[3] != new_cst_id)
                {
                    cst_id[3] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }
        public string CST_ID_LOC_4
        {
            get { return IS_PRESENCE_LOC_4 ? !string.IsNullOrWhiteSpace(cst_id[4]) ? cst_id[4] : CST_ID : ""; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[4] != new_cst_id)
                {
                    cst_id[4] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }
        public string CST_ID_LOC_5
        {
            get { return IS_PRESENCE_LOC_5 ? !string.IsNullOrWhiteSpace(cst_id[5]) ? cst_id[5] : CST_ID : ""; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[5] != new_cst_id)
                {
                    cst_id[5] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }
        public string CST_ID_LOC_6
        {
            get { return IS_PRESENCE_LOC_6 ? !string.IsNullOrWhiteSpace(cst_id[6]) ? cst_id[6] : CST_ID : ""; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[6] != new_cst_id)
                {
                    cst_id[6] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }
        public string CST_ID_LOC_7
        {
            get { return IS_PRESENCE_LOC_7 ? !string.IsNullOrWhiteSpace(cst_id[7]) ? cst_id[7] : CST_ID : ""; }
            set
            {
                string new_cst_id = value?.Trim() ?? "";
                if (cst_id[7] != new_cst_id)
                {
                    cst_id[7] = new_cst_id;
                    OnPropertyChanged();
                }
            }
        }

        private string[] box_id = { "", "", "", "", "", "", "", "" };
        public string BOX_ID // backup display if BOX_ID_LOC_X = empty while IS_PRESENCE_LOC_X = true
        {
            get { return box_id[0]; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[0] != new_box_id)
                {
                    box_id[0] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }
        public string BOX_ID_LOC_1
        {
            get { return IS_PRESENCE_LOC_1 ? !string.IsNullOrWhiteSpace(box_id[1]) ? box_id[1] : BOX_ID : ""; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[1] != new_box_id)
                {
                    box_id[1] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }
        public string BOX_ID_LOC_2
        {
            get { return IS_PRESENCE_LOC_2 ? !string.IsNullOrWhiteSpace(box_id[2]) ? box_id[2] : BOX_ID : ""; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[2] != new_box_id)
                {
                    box_id[2] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }
        public string BOX_ID_LOC_3
        {
            get { return IS_PRESENCE_LOC_3 ? !string.IsNullOrWhiteSpace(box_id[3]) ? box_id[3] : BOX_ID : ""; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[3] != new_box_id)
                {
                    box_id[3] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }
        public string BOX_ID_LOC_4
        {
            get { return IS_PRESENCE_LOC_4 ? !string.IsNullOrWhiteSpace(box_id[4]) ? box_id[4] : BOX_ID : ""; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[4] != new_box_id)
                {
                    box_id[4] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }
        public string BOX_ID_LOC_5
        {
            get { return IS_PRESENCE_LOC_5 ? !string.IsNullOrWhiteSpace(box_id[5]) ? box_id[5] : BOX_ID : ""; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[5] != new_box_id)
                {
                    box_id[5] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }
        public string BOX_ID_LOC_6
        {
            get { return IS_PRESENCE_LOC_6 ? !string.IsNullOrWhiteSpace(box_id[6]) ? box_id[6] : BOX_ID : ""; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[6] != new_box_id)
                {
                    box_id[6] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }
        public string BOX_ID_LOC_7
        {
            get { return IS_PRESENCE_LOC_7 ? !string.IsNullOrWhiteSpace(box_id[7]) ? box_id[7] : BOX_ID : ""; }
            set
            {
                string new_box_id = value?.Trim() ?? "";
                if (box_id[7] != new_box_id)
                {
                    box_id[7] = new_box_id;
                    OnPropertyChanged();
                }
            }
        }

        public void SetLocPresenceCstID(bool loc1_presence, string loc1_cst_id)
        {
            IS_PRESENCE_LOC_1 = loc1_presence;
            CST_ID_LOC_1 = loc1_cst_id;
            CST_ID = loc1_cst_id;
        }
        public void SetLocPresenceCstID(string loc1_cst_id)
        {
            SetLocPresenceCstID(!string.IsNullOrWhiteSpace(loc1_cst_id), loc1_cst_id);
        }
        public void SetLocPresenceCstID(bool loc1_presence, string loc1_cst_id,
                                        bool loc2_presence, string loc2_cst_id,
                                        bool loc3_presence, string loc3_cst_id,
                                        bool loc4_presence, string loc4_cst_id,
                                        bool loc5_presence, string loc5_cst_id,
                                        bool loc6_presence, string loc6_cst_id,
                                        bool loc7_presence, string loc7_cst_id,
                                        string cst_id)
        {
            IS_PRESENCE_LOC_1 = loc1_presence;
            IS_PRESENCE_LOC_2 = loc2_presence;
            IS_PRESENCE_LOC_3 = loc3_presence;
            IS_PRESENCE_LOC_4 = loc4_presence;
            IS_PRESENCE_LOC_5 = loc5_presence;
            IS_PRESENCE_LOC_6 = loc6_presence;
            IS_PRESENCE_LOC_7 = loc7_presence;
            CST_ID_LOC_1 = loc1_cst_id;
            CST_ID_LOC_2 = loc2_cst_id;
            CST_ID_LOC_3 = loc3_cst_id;
            CST_ID_LOC_4 = loc4_cst_id;
            CST_ID_LOC_5 = loc5_cst_id;
            CST_ID_LOC_6 = loc6_cst_id;
            CST_ID_LOC_7 = loc7_cst_id;
            CST_ID = cst_id;
        }
        public void SetLocPresenceCstIDBoxID(bool loc1_presence, string loc1_cst_id, string loc1_box_id,
                                             bool loc2_presence, string loc2_cst_id, string loc2_box_id,
                                             bool loc3_presence, string loc3_cst_id, string loc3_box_id,
                                             bool loc4_presence, string loc4_cst_id, string loc4_box_id,
                                             bool loc5_presence, string loc5_cst_id, string loc5_box_id,
                                             bool loc6_presence, string loc6_cst_id, string loc6_box_id,
                                             bool loc7_presence, string loc7_cst_id, string loc7_box_id,
                                             string cst_id, string box_id)
        {
            IS_PRESENCE_LOC_1 = loc1_presence;
            IS_PRESENCE_LOC_2 = loc2_presence;
            IS_PRESENCE_LOC_3 = loc3_presence;
            IS_PRESENCE_LOC_4 = loc4_presence;
            IS_PRESENCE_LOC_5 = loc5_presence;
            IS_PRESENCE_LOC_6 = loc6_presence;
            IS_PRESENCE_LOC_7 = loc7_presence;
            CST_ID_LOC_1 = loc1_cst_id;
            CST_ID_LOC_2 = loc2_cst_id;
            CST_ID_LOC_3 = loc3_cst_id;
            CST_ID_LOC_4 = loc4_cst_id;
            CST_ID_LOC_5 = loc5_cst_id;
            CST_ID_LOC_6 = loc6_cst_id;
            CST_ID_LOC_7 = loc7_cst_id;
            CST_ID = cst_id;
            BOX_ID_LOC_1 = loc1_box_id;
            BOX_ID_LOC_2 = loc2_box_id;
            BOX_ID_LOC_3 = loc3_box_id;
            BOX_ID_LOC_4 = loc4_box_id;
            BOX_ID_LOC_5 = loc5_box_id;
            BOX_ID_LOC_6 = loc6_box_id;
            BOX_ID_LOC_7 = loc7_box_id;
            BOX_ID = box_id;
        }

        public PortData PORT_DATA { get; set; } = new PortData();
    }

    public static class VPORTSTATION_Def
    {
        public enum PortStatus
        {
            Down = 0,
            LoadRequest = 1,
            UnloadRequest = 2,
            Wait = 3,
            Disabled = 4
        }

        public enum PortServiceStatus
        {
            NoDefinition = 0,
            OutOfService = 1,
            InService = 2
        }
    }

    public class PortData
    {
        public enum PlcPortStatus
        {
            RUN = 0,
            IsAutoMode,
            ErrorBit,
            ErrorCode,
            IsModeChangable = 5,
            IsInputMode,
            IsOutputMode,
            IsReadyToLoad = 9,
            IsReadyToUnload,
            PortWaitIn = 12,
            PortWaitOut,
            CIM_ON = 15,
            PreLoadOK,
            Count
        }
        public List<string> ListPlcPortStatus { get; set; }

        public enum StageInfo
        {
            Remove = 0,
            BCRReadDone = 2,
            BoxID,
            LoadPosition1 = 5,
            LoadPosition2,
            LoadPosition3,
            LoadPosition4,
            LoadPosition5,
            LoadPosition6,
            LoadPosition7,
            Count
        }
        public List<string> ListStageInfo { get; set; }
        public List<string> ListStageBoxID { get; set; }

        public enum AgvPortSignal
        {
            openAGV_Station = 0,
            openAGV_AutoPortType,
            IsAGVMode,
            IsMGVMode,
            AGVPortReady = 5,
            AGVPortMismatch,
            CanOpenBox = 8,
            IsBoxOpen,
            CassetteID = 11,
            IsCSTPresence,
            Count
        }
        public List<string> ListAgvPortSignal { get; set; }

        public PortData()
        {
            ListPlcPortStatus = new List<string>();
            for (int i = 0; i < (int)PlcPortStatus.Count; i++)
            {
                ListPlcPortStatus.Add("");
            }

            ListStageInfo = new List<string>();
            ListStageBoxID = new List<string>();
            for (int i = 0; i < (int)StageInfo.Count; i++)
            {
                ListStageInfo.Add("");
                ListStageBoxID.Add("");
            }

            ListAgvPortSignal = new List<string>();
            for (int i = 0; i < (int)AgvPortSignal.Count; i++)
            {
                ListAgvPortSignal.Add("");
            }
        }
    }
}
