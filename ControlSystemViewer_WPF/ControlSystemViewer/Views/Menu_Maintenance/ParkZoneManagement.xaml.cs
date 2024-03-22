using com.mirle.ibg3k0.Utility.uc;
using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewerObject;
using ControlSystemViewer.Views;
using NLog;
using MirleGO_UIFrameWork.UI.uc_Button;

namespace ControlSystemViewer.Views.Menu_Maintenance
{
    public partial class ParkZoneManagement : UserControl
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WindownApplication app = null;
        public event EventHandler CloseEvent;
        private List<ParkingZone> parkZoneLst = null;

        public ParkZoneManagement()
        {
            InitializeComponent();

            combo_VhType.Items.Add("0");
            combo_VhType.Items.Add("1");
            combo_VhType.Items.Add("2");
            combo_VhType.Items.Add("3");
            combo_VhType.Items.Add("4");

            combo_IsAct.Items.Add("Y");
            combo_IsAct.Items.Add("N");

            combo_VhType.SelectedIndex = -1;
            combo_IsAct.SelectedIndex = -1;
        }

        public void startUI()
        {
            app = WindownApplication.getInstance();

            Map_Base.Start(app, this);

            refreshParkZone();
            parkZoneLst = app.ParkZoneBLL.GetAllParkingZoneData();
        }

        public void SetSelectAdr(string address, bool is_select)
        {
            try
            {
                if(address == null || address.Trim().Equals(string.Empty))
                {
                    return;
                }

                List<ParkZoneAddVo> addLst = (List<ParkZoneAddVo>)this.dgv_address.ItemsSource;

                if(is_select)
                {
                    bool found = false;
                    if(addLst != null && addLst.Count > 0)
                    {
                        foreach (ParkZoneAddVo vo in addLst)
                        {
                            if (vo.ADDRESS.Trim().Equals(address.Trim()))
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if(!found)
                    {
                        ParkZoneAddVo vo = new ParkZoneAddVo();
                        vo.ADDRESS = address;

                        if(addLst == null)
                        {
                            addLst = new List<ParkZoneAddVo>();
                        }

                        addLst.Add(vo);
                    }
                }
                else
                {
                    if (addLst != null && addLst.Count > 0)
                    {
                        foreach (ParkZoneAddVo vo in addLst)
                        {
                            if (vo.ADDRESS.Trim().Equals(address.Trim()))
                            {
                                addLst.Remove(vo);
                                break;
                            }
                        }
                    } 
                }

                Adapter.Invoke((obj) =>
                {
                    dgv_address.ItemsSource = addLst;
                    dgv_address.Items.Refresh();
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if(dgv_parkzone.SelectedItem != null)
                {
                    ParkZoneMasterVo vPZ = (ParkZoneMasterVo)dgv_parkzone.SelectedItem;

                    txt_ParkZoneID.Text = vPZ.PARK_ZONE_ID == null ? string.Empty : vPZ.PARK_ZONE_ID.Trim();
                    txt_WaterLvl.Text = vPZ.LOWER_BORDER.ToString();
                    txt_PullDist.Text = vPZ.PULL_DIST.ToString();

                    int newSelectedIndex = combo_VhType.Items.IndexOf(vPZ.VEHICLE_TYPE.ToString());
                    if (newSelectedIndex >= 0)
                    {
                        combo_VhType.SelectedIndex = newSelectedIndex;
                        combo_VhType.Focus();
                    }

                    string act;
                    if (vPZ.IS_ACTIVE)
                    {
                        act = "Y";
                    }
                    else
                    {
                        act = "N";
                    }
                    newSelectedIndex = combo_IsAct.Items.IndexOf(act);
                    if (newSelectedIndex >= 0)
                    {
                        combo_IsAct.SelectedIndex = newSelectedIndex;
                        combo_IsAct.Focus();
                    }

                    dgv_address.ItemsSource = null;
                    dgv_address.Items.Refresh();

                    if (!txt_ParkZoneID.Text.Trim().Equals(string.Empty))
                    {
                        List<string> addLst = null;
                        parkZoneLst = app.ParkZoneBLL.GetAllParkingZoneData();
                        foreach (ParkingZone pz in parkZoneLst)
                        {
                            if (pz.ParkingZoneID.Trim().Equals(txt_ParkZoneID.Text.Trim()))
                            {
                                addLst = pz.ParkAddressIDs;
                                break;
                            }
                        }

                        if (addLst != null && addLst.Count > 0)
                        {
                            List<ParkZoneAddVo> pzAddLst = new List<ParkZoneAddVo>();
                            foreach (string add in addLst)
                            {
                                ParkZoneAddVo vo = new ParkZoneAddVo();
                                vo.ADDRESS = add;

                                pzAddLst.Add(vo);
                            }

                            dgv_address.ItemsSource = pzAddLst;
                        }

                        Map_Base.Map.setAddress(addLst);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!checkDataInput())
                {
                    return;
                }

                string result;
                if (sendModifyData(BCAppConstants.ACT_FLAG_ADD, out result))
                {
                    TipMessage_Type_Light.Show("Add success", "", BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show(result, "", BCAppConstants.WARN_MSG);
                }

                refreshParkZone();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!checkDataInput())
                {
                    return;
                }

                string result;
                if (sendModifyData(BCAppConstants.ACT_FLAG_UPDATE, out result))
                {
                    TipMessage_Type_Light.Show("Update success", "", BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show(result, "", BCAppConstants.WARN_MSG);
                }

                refreshParkZone();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_ParkZoneID.Text == null || txt_ParkZoneID.Text.Trim().Equals(string.Empty))
                {
                    TipMessage_Type_Light.Show("Parking zone ID is empty", "", BCAppConstants.INFO_MSG);
                    return;
                }

                string result;
                if (sendModifyData(BCAppConstants.ACT_FLAG_DELETE, out result))
                {
                    TipMessage_Type_Light.Show("Delete success", "", BCAppConstants.INFO_MSG);
                }
                else
                {
                    TipMessage_Type_Light.Show(result, "", BCAppConstants.WARN_MSG);
                }


                refreshParkZone();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private bool sendModifyData(string modify, out string result)
        {
            bool rtn_code;
            result = string.Empty;

            try
            {
                ParkingZoneData pz = new ParkingZoneData();
                pz.ParkZone_ID = txt_ParkZoneID.Text.Trim();
                pz.Vh_Type = combo_VhType.SelectedItem.ToString();
                pz.Water_Level = txt_WaterLvl.Text.Trim();
                pz.Pull_Dist = txt_PullDist.Text.Trim();

                if (combo_IsAct.SelectedItem.ToString().Equals("Y"))
                {
                    pz.Is_Active = "True";
                }
                else
                {
                    pz.Is_Active = "False";
                }

                pz.Address_List = new List<string>();
                if (dgv_address.Items.Count > 0)
                {
                    for (int i = 0; i < dgv_address.Items.Count; i++)
                    {
                        ParkZoneAddVo address = dgv_address.Items[i] as ParkZoneAddVo;
                        if (address != null && !address.ADDRESS.Trim().Equals(string.Empty))
                        {
                            pz.Address_List.Add(address.ADDRESS);
                        }
                    }
                }

                rtn_code = app.ParkZoneBLL.SendModifyParkZone(modify, pz, out result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");

                return false;
            }

            return rtn_code;
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseEvent?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private bool checkDataInput()
        {
            try
            {
                if(txt_ParkZoneID.Text == null || txt_ParkZoneID.Text.Trim().Equals(string.Empty))
                {
                    TipMessage_Type_Light.Show("Parking zone ID is empty", "", BCAppConstants.INFO_MSG);
                    return false;
                }

                if (combo_VhType.SelectedIndex == -1)
                {
                    TipMessage_Type_Light.Show("Please select vehicle type", "", BCAppConstants.INFO_MSG);
                    return false;
                }

                if (txt_WaterLvl.Text == null || txt_WaterLvl.Text.Trim().Equals(string.Empty))
                {
                    TipMessage_Type_Light.Show("Water level is empty", "", BCAppConstants.INFO_MSG);
                    return false;
                }

                string water_lvl = txt_WaterLvl.Text.Trim();
                int lvl;
                if(!int.TryParse(water_lvl, out lvl))
                {
                    TipMessage_Type_Light.Show("Water level can only intput number", "", BCAppConstants.INFO_MSG);
                    return false;
                }

                if (txt_PullDist.Text == null || txt_PullDist.Text.Trim().Equals(string.Empty))
                {
                    TipMessage_Type_Light.Show("Pull distance is empty", "", BCAppConstants.INFO_MSG);
                    return false;
                }

                string pull_dist = txt_PullDist.Text.Trim();
                int dist;
                if (!int.TryParse(pull_dist, out dist))
                {
                    TipMessage_Type_Light.Show("Pull distance can only intput number", "", BCAppConstants.INFO_MSG);
                    return false;
                }

                if (combo_IsAct.SelectedIndex == -1)
                {
                    TipMessage_Type_Light.Show("Please select is active", "", BCAppConstants.INFO_MSG);
                    return false;
                }

                if(dgv_address.Items.Count == 0)
                {
                    TipMessage_Type_Light.Show("Please set address", "", BCAppConstants.INFO_MSG);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");

                return false;
            }

            return true;
        }

        private void refreshParkZone()
        {
            try
            {
                Adapter.Invoke((obj) =>
                {
                    this.dgv_parkzone.ItemsSource = changeVo(app.ParkZoneBLL.LoadParkingZoneMaster());
                }, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private List<ParkZoneMasterVo> changeVo(List<ViewerObject.ParkZoneMaster> parkZoneLst)
        {
            List<ParkZoneMasterVo> voLst = new List<ParkZoneMasterVo>();

            try
            {
                if(parkZoneLst != null && parkZoneLst.Count > 0)
                {
                    foreach(ViewerObject.ParkZoneMaster pz in parkZoneLst)
                    {
                        ParkZoneMasterVo vo = new ParkZoneMasterVo();
                        vo.PARK_ZONE_ID = pz.PARK_ZONE_ID;
                        vo.IS_ACTIVE = pz.IS_ACTIVE;
                        vo.VEHICLE_TYPE = pz.VEHICLE_TYPE;
                        vo.PULL_DIST = pz.PULL_DEST;
                        vo.TOTAL_BORDER = pz.TOTAL_BORDER;
                        vo.LOWER_BORDER = pz.LOWER_BORDER;
                        vo.ENTRY_ADR_ID = pz.ENTRY_ADR_ID;

                        voLst.Add(vo);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }

            return voLst;
        }

        public void Close()
        {
            try
            {
                unRegisterEvent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception");
            }
        }

        private void unRegisterEvent()
        {

        }

    }

    public class ParkZoneMasterVo
    {
        public string PARK_ZONE_ID { get; set; }

        public bool IS_ACTIVE { get; set; }

        public int VEHICLE_TYPE { get; set; }

        public int PULL_DIST { get; set; }

        public int TOTAL_BORDER { get; set; }

        public int LOWER_BORDER { get; set; }

        public string ENTRY_ADR_ID { get; set; }
    }

    public class ParkZoneAddVo
    {
        public string ADDRESS { get; set; }
    }

}
