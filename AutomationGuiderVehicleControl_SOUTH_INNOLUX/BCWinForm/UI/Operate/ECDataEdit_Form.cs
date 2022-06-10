//*********************************************************************************
//      ECDataEdit_Form.cs
//*********************************************************************************
// File Name: ECDataEdit_Form.cs
// Description: ECDataEdit Form
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
//               Hayes Chen     N/A            N/A     Initial Release
// 2015/01/02    Kevin Wei      N/A            A0.01   增加DCS相關條件的ECID
// 2015/01/02    Kevin Wei      N/A            A0.02   增加DSC Mode的選擇(標準模式、效能模式)
// 2015/05/03    Kevin Wei      N/A            A0.03   增加，不允許在線修改Rounte Mode
// 2015/07/05    Kevin Wei      N/A            A0.04   增加，新增、修改、刪除的Log
// 2015/07/29    Kevin Wei      N/A            A0.05   增加提示，當DCS Mode修改成功後，需要重啟MPC。
// 2016/09/29    Steven Hong    N/A            A0.06   解決畫面開啟緩慢問題、將所有Message Box換為自製元件、Data Gird View修改資料Input方式
// 2016/12/06    Eric Chiang    N/A            A0.07   修改資料時檢查ECValue是否為空值
// 2016/10/21    Bob   Yan      N/A            A0.08   add pre move config、修正若ECID的Value不存在時，會有Null Exection的提。
// 2017/01/23    Eric Chiang    N/A            A0.09   新增資料完成後以Message通知User
//                                                     刪除資料改為直接刪除選定的資料, 刪除前會進行通知
//                                                     新增修改資料時多檢查ECID/ECName/MaxValue/MinValue不得為空值
//**********************************************************************************
// 2017/04/12    Eric Chiang    N/A            B0.01   Alarm/Warning Message改以onMainAlarm/getMainAlarm的方式取得
// 2017/05/16    Boan Chen    N/A            B0.02   調整顯示於低解析度的NB也不會有截掉畫面的現象。
// 2017/06/07    Boan Chen    N/A            B0.03   調整視窗切換時會從介面"最大化"變為介面"視窗化"。
// 2018/08/20    Kevin Wei     N/A            B0.04    在不是DifferenceExchange的機台，過濾掉ECID_Exchange_Wait_Time。
//**********************************************************************************
using com.mirle.ibg3k0.bc.winform.App;
using com.mirle.ibg3k0.bcf.App;
using com.mirle.ibg3k0.bcf.Common;
using com.mirle.ibg3k0.sc;
using com.mirle.ibg3k0.sc.App;
using com.mirle.ibg3k0.sc.Data.ValueDefMapAction;
using com.mirle.ibg3k0.sc.Data.VO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;  //A0.06
using System.Threading.Tasks;
using System.Windows.Forms;

namespace com.mirle.ibg3k0.bc.winform.UI
{
    public partial class ECDataEdit_Form : Form
    {
        private BCMainForm mainForm;
        private BCApplication bcApp = null;
        private string bcID = string.Empty;
        public ECDataEdit_Form(BCMainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            bcApp = BCApplication.getInstance();
        }

        private void ECDataEdit_Form_Load(object sender, EventArgs e)
        {
            m_ecidTxb.MaxLength = 4;
            m_ecNameTxb.MaxLength = 40;
            m_ecValueTxb.MaxLength = 10;
            bcID = bcApp.SCApplication.BC_ID;
            m_ecDataDGV.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14);
            m_ecDataDGV.DefaultCellStyle.Font = new Font("Arial", 14);
            refresh();
        }



        private void ECDataEdit_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.removeForm(nameof(ECDataEdit_Form));
        }

        List<AECDATAMAP> ecDataList = new List<AECDATAMAP>();
        private void refresh()
        {
            ecDataList = bcApp.SCApplication.LineBLL.loadECDataList(bcID);

            //A0.06 Start
            m_ecDataDGV.Rows.Clear();

            foreach (AECDATAMAP ecData in ecDataList)
            {
                Adapter.Invoke(new SendOrPostCallback((o1) =>
                {
                    m_ecDataDGV.Rows.Add(
                    new string[] {
                            ecData.ECID,
                            ecData.ECNAME,
                            ecData.ECMIN,
                            ecData.ECMAX,
                            ecData.ECV,
                        });
                }), null);
            }
        }

        private void m_ecidTxb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 | (int)e.KeyChar > 57) & (int)e.KeyChar != 8)
            {
                e.Handled = true;

            }
        }

        private void m_ecDataDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.RowIndex < 0 || e.RowIndex >= dgv.Rows.Count) { return; }

            //A0.06 Start
            m_ecidTxb.Text = dgv.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
            m_ecNameTxb.Text = dgv.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
            m_minTxb.Text = dgv.Rows[e.RowIndex].Cells[2].Value.ToString().Trim();
            m_maxTxb.Text = dgv.Rows[e.RowIndex].Cells[3].Value.ToString().Trim();
            m_ecValueTxb.Text = dgv.Rows[e.RowIndex].Cells[4].Value.ToString().Trim();

            bulidECValueTextBox();
        }

        private void m_modifyBtn_Click(object sender, EventArgs e)
        {
            string ecid = string.Format(SCAppConstants.ECID_Format, m_ecidTxb.Text);
            string ecName = m_ecNameTxb.Text;
            string ecValue;
            string ecMax = m_maxTxb.Text; //A0.09
            string ecMin = m_minTxb.Text; //A0.09

            ecValue = m_ecValueTxb.Text;

            //A0.09 Add Start
            if (BCFUtility.isEmpty(ecid))
            {
                MessageBox.Show(this, SCApplication.getMessageString("ECDataEdit_ECID_Cannot_Empty"));

                return;
            }
            if (BCFUtility.isEmpty(ecName))
            {
                MessageBox.Show(this, SCApplication.getMessageString("ECDataEdit_ECName_Cannot_Empty"));
                return;
            }
            //A0.09 Add End

            // A0.07 Add Start 修改資料時檢查ECValue是否為空值
            if (BCFUtility.isEmpty(ecValue))
            {
                MessageBox.Show(this, SCApplication.getMessageString("ECDataEdit_ECV_Cannot_Empty"));

                return;
            }
            // A0.07 Add End

            //A0.09 Add Start
            if (BCFUtility.isEmpty(ecMax))
            {
                MessageBox.Show(this, SCApplication.getMessageString("ECDataEdit_MaxValue_Cannot_Empty"));
                return;
            }
            if (BCFUtility.isEmpty(ecMin))
            {
                MessageBox.Show(this, SCApplication.getMessageString("ECDataEdit_MinValue_Cannot_Empty"));

                return;
            }
            //A0.09 Add end

            //A0.09 string ecMax = m_maxTxb.Text;
            //A0.09 string ecMin = m_minTxb.Text;
            Boolean isFound = false;
            foreach (AECDATAMAP data in ecDataList)
            {
                if (BCFUtility.isMatche(data.ECID, ecid))
                {
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
            {
                MessageBox.Show(this, SCApplication.getMessageString("ECDataEdit_ECV_Not_Found_ECID_{0}", ecid));
                return;
            }
            AECDATAMAP ecData = new AECDATAMAP()
            {
                ECID = ecid,
                ECNAME = ecName,
                ECV = ecValue,
                ECMAX = ecMax,
                ECMIN = ecMin,
                EQPT_REAL_ID = bcApp.SCApplication.BC_ID
            };

            string action = string.Format("Modify ECID, ECID:[{0}],ECNAME:[{1}],ECV:[{2}],ECMAX:[{3}],ECMIN:[{4}],EQPT_REAL_ID:[{5}]" //A0.04
            , ecData.ECID, ecData.ECNAME, ecData.ECV, ecData.ECMAX, ecData.ECMIN, ecData.EQPT_REAL_ID);                               //A0.04
            bcApp.SCApplication.BCSystemBLL.addOperationHis(bcApp.LoginUserID, this.Name, action);                                    //A0.04


            List<AECDATAMAP> updateEcDataList = new List<AECDATAMAP>();
            updateEcDataList.Add(ecData);
            string updateEcRtnMsg = string.Empty;
            Boolean result = bcApp.SCApplication.LineBLL.updateECData(updateEcDataList, out updateEcRtnMsg, false); //A0.03
            //progress.End();
            if (result)
            {
                MessageBox.Show(this, "Update Success !");
            }
            else
            {
                BCFApplication.onInfoMsg(this.Name, updateEcRtnMsg);
            }
            refresh();

        }
        private void bulidECValueTextBox()
        {
            this.tableLayoutPanel2.Controls.RemoveByKey("m_ecValueCmb");
            this.tableLayoutPanel2.Controls.Add(this.m_ecValueTxb, 1, 0);
        }

        //A0.01 Strat
        private void bulidECValueCombobox(string ec_name, string ec_value, string ec_maxValue)
        {
            this.tableLayoutPanel2.Controls.RemoveByKey("m_ecValueTxb");
            this.m_ecValueCmb.Items.Clear();
            ComboData cmbVo;
            m_ecValueCmb.DisplayMember = "Display";
            m_ecValueCmb.ValueMember = "Value";
            this.m_ecValueCmb.DropDownStyle = ComboBoxStyle.DropDownList;//A0.08
            this.tableLayoutPanel2.Controls.Add(this.m_ecValueCmb, 1, 0);
            this.m_ecValueCmb.Text = selectComboValue(ec_value);//A0.08
        }

        private string selectComboValue(string ec_value)
        {
            foreach (ComboData data in this.m_ecValueCmb.Items.Cast<ComboData>().ToList())
            {
                if (BCFUtility.isMatche(data.Value, ec_value))
                {
                    return data.Display;
                }
            }
            return "";

        }

        //A0.06 Start
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        //A0.06 End

        # region 自訂ComboData物件
        ///
        /// 設定取的自訂ComboData內容
        ///
        public class ComboData
        {
            private string m_display = string.Empty;
            private string m_value = string.Empty;

            public ComboData(string display, string value)
            {
                this.m_display = display;
                this.m_value = value;
            }
            public string Display
            {
                get { return this.m_display; }
                set { this.m_display = value; }
            }
            public string Value
            {
                get { return this.m_value; }
                set { this.m_value = value; }
            }
        }
        # endregion

        private void m_detailGbx_Enter(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //A0.01 End
    }
}
