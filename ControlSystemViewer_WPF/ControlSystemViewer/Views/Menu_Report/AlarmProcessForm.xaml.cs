using com.mirle.ibg3k0.bc.wpf.App;
using com.mirle.ibg3k0.ohxc.wpf.App;
using MirleGO_UIFrameWork.UI.uc_Button;
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
using System.Windows.Shapes;

namespace ControlSystemViewer.Views.Menu_Report
{
    /// <summary>
    /// AlarmProcessForm.xaml 的互動邏輯
    /// </summary>
    public partial class AlarmProcessForm : Window
    {

        AlarmDetail parent;
        WindownApplication app;
        ViewerObject.VALARM VALARM;
        List<string> alarmRemarkHistoryList;
        private int alarmHistoryKeepCount = 5;
        public AlarmProcessForm(AlarmDetail AlarmDetail, ViewerObject.VALARM vALARM, string EQPT_ID, string AlarmCode, string AlarmDesc, DateTime RPT_DATE_TIME, string classification, string AlarmRemark, List<string> AlarmRemarkHistoryList = null)
        {
            InitializeComponent();

            parent = AlarmDetail;
            VALARM = vALARM;
            app = WindownApplication.getInstance();
            CB_AlarmModule.ItemsSource = app.ObjCacheManager.GetAlarmModule();
            CB_AlarmModule.DisplayMemberPath = "Module_TW";
            CB_AlarmModule.SelectedValuePath = "Number";

            CB_AlarmImportanceLevel.ItemsSource = Enum.GetValues(typeof(ViewerObject.VALARM_Def.AlarmImportanceLevel)).Cast<ViewerObject.VALARM_Def.AlarmImportanceLevel>();

            this.TB_EQPT_ID.Text = EQPT_ID;
            this.TB_AlarmCode.Text = AlarmCode;
            this.TB_AlarmDesc.Text = AlarmDesc;
            this.TB_Happend.Text = RPT_DATE_TIME.ToString("yyyy/MM/dd HH:mm:ss.fffffff");
            if (!string.IsNullOrWhiteSpace(AlarmRemark))
                this.CB_Remark.Items.Add(AlarmRemark);
            if (AlarmRemarkHistoryList != null && AlarmRemarkHistoryList.Count > 0)
                foreach (string alarmRemark in AlarmRemarkHistoryList)
                    this.CB_Remark.Items.Add(alarmRemark);
            alarmRemarkHistoryList = AlarmRemarkHistoryList;

            if (!string.IsNullOrWhiteSpace(AlarmRemark))
                CB_Remark.SelectedIndex = 0;
            else
                CB_Remark.SelectedIndex = CB_Remark.Items.Count - 1;

            //TB_AlarmCode.Text = FindResource( "ALARM_PROCESS_EQPT_ID").ToString();

            //由於傳來的分類為文字，但為求方便都第一碼帶數字所以這邊取第一碼做判斷
            //這邊不用Convert轉數字直接填入是不希望他跳Exception~~~大不了就轉0就好
            if (classification.Length <= 0)
                CB_Classification.SelectedIndex = 0;
            else
            {
                switch (classification.Substring(0, 1))
                {
                    case "1":
                        CB_Classification.SelectedIndex = 1;
                        break;
                    case "2":
                        CB_Classification.SelectedIndex = 2;
                        break;
                    case "3":
                        CB_Classification.SelectedIndex = 3;
                        break;
                    case "4":
                        CB_Classification.SelectedIndex = 4;
                        break;
                    case "5":
                        CB_Classification.SelectedIndex = 5;
                        break;
                    case "6":
                        CB_Classification.SelectedIndex = 6;
                        break;
                    case "7":
                        CB_Classification.SelectedIndex = 7;
                        break;
                    default:
                        CB_Classification.SelectedIndex = 0;
                        break;
                }
            }
            CB_AlarmImportanceLevel.SelectedItem = VALARM.IMPORTANCE_LEVEL;
            CB_AlarmModule.SelectedIndex = tryGetCurrentAlarmModuleSelectedIndex();
        }
        private int tryGetCurrentAlarmModuleSelectedIndex()
        {
            var item_source = CB_AlarmModule.ItemsSource as List<ViewerObject.AlarmModule>;
            if (item_source == null) return 0;
            var item = item_source.Where(module => module.Number == VALARM.ALARM_MODULE).FirstOrDefault();
            if (item == null) return 0;
            return item_source.IndexOf(item);
        }

        private void BT_Commit_Click(object sender, RoutedEventArgs e)
        {
            bool isSuccess = false;
            string result;
            DateTime dt = DateTime.MinValue;
            string alarmCode = "";
            if (CB_Classification.SelectedIndex == 0)
            {
                this.LB_Warring.Visibility = Visibility.Visible;
            }
            else
            {
                //發送Grpc給Controller並通知Report要做更新
                string remark = CB_Remark.Text;
                int classification = CB_Classification.SelectedIndex;
                Enum.TryParse(CB_AlarmImportanceLevel.SelectedValue.ToString(), out ViewerObject.VALARM_Def.AlarmImportanceLevel importance);
                int alarm_module = (int)CB_AlarmModule.SelectedValue;
                string salarm_module = CB_AlarmModule.Text;
                try
                {
                    dt = Convert.ToDateTime(TB_Happend.Text);
                    alarmCode = TB_AlarmCode.Text;
                    isSuccess = app.ObjCacheManager.alarmUpdate(TB_EQPT_ID.Text, dt, alarmCode, "PC", classification, remark, (uint)importance, (uint)alarm_module);
                }
                catch (Exception ex)
                {

                }
                if (isSuccess)
                {
                    VALARM.ALARM_REMARK = remark;
                    VALARM._ALARM_CLASSIFICATION = classification;
                    VALARM.IMPORTANCE_LEVEL = importance;
                    VALARM.ALARM_MODULE = alarm_module;
                    VALARM.sALARM_MODULE = salarm_module;

                    if (!alarmRemarkHistoryList.Contains(remark))
                    {
                        //此字串不在清單內，所以要加進去歷史清單
                        if (alarmRemarkHistoryList.Count >= alarmHistoryKeepCount)
                        {
                            for (int i = 0; i < alarmHistoryKeepCount - 1; i++)
                                alarmRemarkHistoryList[i] = alarmRemarkHistoryList[i + 1];
                            alarmRemarkHistoryList[alarmHistoryKeepCount - 1] = remark;
                        }
                        else
                            alarmRemarkHistoryList.Add(CB_Remark.Text);
                    }
                    else
                    {
                        //此字串在清單內，但不確定他是不是能被"預設"，故將現在這個remark與清單中最後一個做交換
                        alarmRemarkHistoryList[alarmRemarkHistoryList.IndexOf(remark)] = alarmRemarkHistoryList[alarmRemarkHistoryList.Count - 1];
                        alarmRemarkHistoryList[alarmRemarkHistoryList.Count - 1] = remark;
                    }
                    this.Close();
                }
                //app.ObjCacheManager.alarmControl( EQPT_ID,  RPT_DATE_TIME,  ALARM_CODE,  UserID,  REMARK,  ALARM_REAL_DESC);

            }
        }

        private void BT_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
