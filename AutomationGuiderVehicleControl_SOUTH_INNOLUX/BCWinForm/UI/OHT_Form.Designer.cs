﻿namespace com.mirle.ibg3k0.bc.winform.UI
{
    partial class OHT_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_SCState = new System.Windows.Forms.Label();
            this.lbl_detectionSystemExist = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbl_earthqualeHappend = new System.Windows.Forms.Label();
            this.lbl_hostconnAndMode = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_RediStat = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_isMaster = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_creatMCSCommandManual = new System.Windows.Forms.Button();
            this.cb_autoOverride = new System.Windows.Forms.CheckBox();
            this.txt_cst_id = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.gb_PortNameType = new System.Windows.Forms.GroupBox();
            this.Raid_PortNameType_AdrID = new System.Windows.Forms.RadioButton();
            this.Raid_PortNameType_PortID = new System.Windows.Forms.RadioButton();
            this.cmb_cycRunZone = new System.Windows.Forms.ComboBox();
            this.btn_pause = new System.Windows.Forms.Button();
            this.btn_continuous = new System.Windows.Forms.Button();
            this.btn_Avoid = new System.Windows.Forms.Button();
            this.btn_start = new System.Windows.Forms.Button();
            this.cmb_fromAddress = new System.Windows.Forms.ComboBox();
            this.cmb_toAddress = new System.Windows.Forms.ComboBox();
            this.cb_sectionThroughTimes = new System.Windows.Forms.CheckBox();
            this.ck_montor_vh = new System.Windows.Forms.CheckBox();
            this.cbm_Action = new System.Windows.Forms.ComboBox();
            this.cmb_Vehicle = new System.Windows.Forms.ComboBox();
            this.cb_autoTip = new System.Windows.Forms.CheckBox();
            this.lbl_sourceName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_destinationName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uc_StatusTreeViewer1 = new com.mirle.ibg3k0.bc.winform.UI.Components.MyUserControl.uc_StatusTreeViewer();
            this.pnl_Map = new System.Windows.Forms.Panel();
            this.uctl_Map = new com.mirle.ibg3k0.bc.winform.UI.Components.uctl_Map();
            this.tbcList = new System.Windows.Forms.TabControl();
            this.tab_vhStatus = new System.Windows.Forms.TabPage();
            this.dgv_vhStatus = new System.Windows.Forms.DataGridView();
            this.vehicleObjToShowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tapTransferCmd = new System.Windows.Forms.TabPage();
            this.dgv_TransferCommand = new System.Windows.Forms.DataGridView();
            this.cMDIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cARRIERIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VEHICLE_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tRANSFERSTATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hOSTSOURCEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hOSTDESTINATIONDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pRIORITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMDINSERTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMDSTARTTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEPLACEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMDMCSObjToShowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tapDetail = new System.Windows.Forms.TabPage();
            this.dgv_TaskCommand = new System.Windows.Forms.DataGridView();
            this.tapCurrentAlarm = new System.Windows.Forms.TabPage();
            this.tlp_crtAlarm = new System.Windows.Forms.TableLayoutPanel();
            this.dgv_Alarm = new System.Windows.Forms.DataGridView();
            this.eqpt_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarm_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarm_lvl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.report_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarm_desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer_TimedUpdates = new System.Windows.Forms.Timer(this.components);
            this.uPDTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BATTERYCAPACITY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cYCLERUNTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iSCYCLINGDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pARKTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iSPARKINGDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oBSDIST2ShowDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.oHTCCMDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mCSCMDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aCTSTATUSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mODESTATUSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vEHICLEIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vehicleObjToShowBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.aCMDMCSBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.vEHICLEIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mODESTATUSDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aCTSTATUSDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mCSCMDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oHTCCMDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iSPARKINGDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pARKTIMEDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iSCYCLINGDataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bATTERYCAPACITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uPDTIMEDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadingInterlockErrorRetryTimes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnloadingInterlockErrorRetryTimes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentVhSettingOfBattryLowLevelValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb_PortNameType.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.pnl_Map.SuspendLayout();
            this.tbcList.SuspendLayout();
            this.tab_vhStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_vhStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vehicleObjToShowBindingSource)).BeginInit();
            this.tapTransferCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TransferCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDMCSObjToShowBindingSource)).BeginInit();
            this.tapDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TaskCommand)).BeginInit();
            this.tapCurrentAlarm.SuspendLayout();
            this.tlp_crtAlarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Alarm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vehicleObjToShowBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCMDMCSBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbcList);
            this.splitContainer1.Size = new System.Drawing.Size(1924, 1054);
            this.splitContainer1.SplitterDistance = 861;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.95219F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.04782F));
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnl_Map, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.89655F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1924, 861);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(1598, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(323, 855);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Black;
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(315, 823);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Control";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.lbl_SCState);
            this.panel1.Controls.Add(this.lbl_detectionSystemExist);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.lbl_earthqualeHappend);
            this.panel1.Controls.Add(this.lbl_hostconnAndMode);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.lbl_RediStat);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lbl_isMaster);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(309, 817);
            this.panel1.TabIndex = 1;
            // 
            // lbl_SCState
            // 
            this.lbl_SCState.AutoSize = true;
            this.lbl_SCState.BackColor = System.Drawing.Color.Gray;
            this.lbl_SCState.Location = new System.Drawing.Point(11, 101);
            this.lbl_SCState.Name = "lbl_SCState";
            this.lbl_SCState.Size = new System.Drawing.Size(54, 19);
            this.lbl_SCState.TabIndex = 12;
            this.lbl_SCState.Text = "     ";
            // 
            // lbl_detectionSystemExist
            // 
            this.lbl_detectionSystemExist.AutoSize = true;
            this.lbl_detectionSystemExist.BackColor = System.Drawing.Color.Gray;
            this.lbl_detectionSystemExist.Location = new System.Drawing.Point(11, 124);
            this.lbl_detectionSystemExist.Name = "lbl_detectionSystemExist";
            this.lbl_detectionSystemExist.Size = new System.Drawing.Size(54, 19);
            this.lbl_detectionSystemExist.TabIndex = 10;
            this.lbl_detectionSystemExist.Text = "     ";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(71, 101);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 19);
            this.label11.TabIndex = 13;
            this.label11.Text = ":SC State";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(71, 124);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(162, 19);
            this.label10.TabIndex = 11;
            this.label10.Text = ":Detection System";
            // 
            // lbl_earthqualeHappend
            // 
            this.lbl_earthqualeHappend.AutoSize = true;
            this.lbl_earthqualeHappend.BackColor = System.Drawing.Color.Gray;
            this.lbl_earthqualeHappend.Location = new System.Drawing.Point(11, 6);
            this.lbl_earthqualeHappend.Name = "lbl_earthqualeHappend";
            this.lbl_earthqualeHappend.Size = new System.Drawing.Size(54, 19);
            this.lbl_earthqualeHappend.TabIndex = 9;
            this.lbl_earthqualeHappend.Text = "     ";
            // 
            // lbl_hostconnAndMode
            // 
            this.lbl_hostconnAndMode.AutoSize = true;
            this.lbl_hostconnAndMode.BackColor = System.Drawing.Color.Gray;
            this.lbl_hostconnAndMode.Location = new System.Drawing.Point(11, 77);
            this.lbl_hostconnAndMode.Name = "lbl_hostconnAndMode";
            this.lbl_hostconnAndMode.Size = new System.Drawing.Size(54, 19);
            this.lbl_hostconnAndMode.TabIndex = 4;
            this.lbl_hostconnAndMode.Text = "     ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(71, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 19);
            this.label4.TabIndex = 5;
            this.label4.Text = ":Host Conn";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(71, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(180, 19);
            this.label9.TabIndex = 8;
            this.label9.Text = ":Earthquake Happend";
            // 
            // lbl_RediStat
            // 
            this.lbl_RediStat.AutoSize = true;
            this.lbl_RediStat.BackColor = System.Drawing.Color.Gray;
            this.lbl_RediStat.Location = new System.Drawing.Point(11, 30);
            this.lbl_RediStat.Name = "lbl_RediStat";
            this.lbl_RediStat.Size = new System.Drawing.Size(54, 19);
            this.lbl_RediStat.TabIndex = 7;
            this.lbl_RediStat.Text = "     ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(71, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 19);
            this.label7.TabIndex = 6;
            this.label7.Text = ":Redis";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(71, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 19);
            this.label2.TabIndex = 5;
            this.label2.Text = ":Active";
            // 
            // lbl_isMaster
            // 
            this.lbl_isMaster.AutoSize = true;
            this.lbl_isMaster.BackColor = System.Drawing.Color.Gray;
            this.lbl_isMaster.Location = new System.Drawing.Point(11, 54);
            this.lbl_isMaster.Name = "lbl_isMaster";
            this.lbl_isMaster.Size = new System.Drawing.Size(54, 19);
            this.lbl_isMaster.TabIndex = 4;
            this.lbl_isMaster.Text = "     ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_creatMCSCommandManual);
            this.groupBox1.Controls.Add(this.cb_autoOverride);
            this.groupBox1.Controls.Add(this.txt_cst_id);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.gb_PortNameType);
            this.groupBox1.Controls.Add(this.cmb_cycRunZone);
            this.groupBox1.Controls.Add(this.btn_pause);
            this.groupBox1.Controls.Add(this.btn_continuous);
            this.groupBox1.Controls.Add(this.btn_Avoid);
            this.groupBox1.Controls.Add(this.btn_start);
            this.groupBox1.Controls.Add(this.cmb_fromAddress);
            this.groupBox1.Controls.Add(this.cmb_toAddress);
            this.groupBox1.Controls.Add(this.cb_sectionThroughTimes);
            this.groupBox1.Controls.Add(this.ck_montor_vh);
            this.groupBox1.Controls.Add(this.cbm_Action);
            this.groupBox1.Controls.Add(this.cmb_Vehicle);
            this.groupBox1.Controls.Add(this.cb_autoTip);
            this.groupBox1.Controls.Add(this.lbl_sourceName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lbl_destinationName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox1.Location = new System.Drawing.Point(15, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 753);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Specify Path";
            // 
            // btn_creatMCSCommandManual
            // 
            this.btn_creatMCSCommandManual.ForeColor = System.Drawing.Color.Black;
            this.btn_creatMCSCommandManual.Location = new System.Drawing.Point(24, 538);
            this.btn_creatMCSCommandManual.Name = "btn_creatMCSCommandManual";
            this.btn_creatMCSCommandManual.Size = new System.Drawing.Size(137, 31);
            this.btn_creatMCSCommandManual.TabIndex = 13;
            this.btn_creatMCSCommandManual.Text = "Creat MCS CMD";
            this.btn_creatMCSCommandManual.UseVisualStyleBackColor = true;
            this.btn_creatMCSCommandManual.Click += new System.EventHandler(this.btn_creatMCSCommandManual_Click);
            // 
            // cb_autoOverride
            // 
            this.cb_autoOverride.AutoSize = true;
            this.cb_autoOverride.Checked = true;
            this.cb_autoOverride.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_autoOverride.Location = new System.Drawing.Point(23, 604);
            this.cb_autoOverride.Name = "cb_autoOverride";
            this.cb_autoOverride.Size = new System.Drawing.Size(145, 23);
            this.cb_autoOverride.TabIndex = 12;
            this.cb_autoOverride.Text = "Auto override";
            this.cb_autoOverride.UseVisualStyleBackColor = true;
            this.cb_autoOverride.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged_1);
            this.cb_autoOverride.Click += new System.EventHandler(this.cb_autoOverride_Click);
            // 
            // txt_cst_id
            // 
            this.txt_cst_id.Location = new System.Drawing.Point(10, 362);
            this.txt_cst_id.Name = "txt_cst_id";
            this.txt_cst_id.Size = new System.Drawing.Size(163, 26);
            this.txt_cst_id.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 340);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 19);
            this.label8.TabIndex = 9;
            this.label8.Text = "CST ID";
            // 
            // gb_PortNameType
            // 
            this.gb_PortNameType.Controls.Add(this.Raid_PortNameType_AdrID);
            this.gb_PortNameType.Controls.Add(this.Raid_PortNameType_PortID);
            this.gb_PortNameType.ForeColor = System.Drawing.Color.White;
            this.gb_PortNameType.Location = new System.Drawing.Point(10, 143);
            this.gb_PortNameType.Name = "gb_PortNameType";
            this.gb_PortNameType.Size = new System.Drawing.Size(162, 90);
            this.gb_PortNameType.TabIndex = 5;
            this.gb_PortNameType.TabStop = false;
            this.gb_PortNameType.Text = "Port ID Type";
            // 
            // Raid_PortNameType_AdrID
            // 
            this.Raid_PortNameType_AdrID.AutoSize = true;
            this.Raid_PortNameType_AdrID.Checked = true;
            this.Raid_PortNameType_AdrID.Location = new System.Drawing.Point(6, 25);
            this.Raid_PortNameType_AdrID.Name = "Raid_PortNameType_AdrID";
            this.Raid_PortNameType_AdrID.Size = new System.Drawing.Size(81, 23);
            this.Raid_PortNameType_AdrID.TabIndex = 0;
            this.Raid_PortNameType_AdrID.TabStop = true;
            this.Raid_PortNameType_AdrID.Text = "Adr ID";
            this.Raid_PortNameType_AdrID.UseVisualStyleBackColor = true;
            this.Raid_PortNameType_AdrID.CheckedChanged += new System.EventHandler(this.Raid_PortNameType_CheckedChanged);
            // 
            // Raid_PortNameType_PortID
            // 
            this.Raid_PortNameType_PortID.AutoSize = true;
            this.Raid_PortNameType_PortID.Location = new System.Drawing.Point(6, 54);
            this.Raid_PortNameType_PortID.Name = "Raid_PortNameType_PortID";
            this.Raid_PortNameType_PortID.Size = new System.Drawing.Size(90, 23);
            this.Raid_PortNameType_PortID.TabIndex = 0;
            this.Raid_PortNameType_PortID.TabStop = true;
            this.Raid_PortNameType_PortID.Text = "Port ID";
            this.Raid_PortNameType_PortID.UseVisualStyleBackColor = true;
            this.Raid_PortNameType_PortID.CheckedChanged += new System.EventHandler(this.Raid_PortNameType_CheckedChanged);
            // 
            // cmb_cycRunZone
            // 
            this.cmb_cycRunZone.FormattingEnabled = true;
            this.cmb_cycRunZone.Location = new System.Drawing.Point(11, 310);
            this.cmb_cycRunZone.Name = "cmb_cycRunZone";
            this.cmb_cycRunZone.Size = new System.Drawing.Size(162, 27);
            this.cmb_cycRunZone.TabIndex = 4;
            // 
            // btn_pause
            // 
            this.btn_pause.ForeColor = System.Drawing.Color.Black;
            this.btn_pause.Location = new System.Drawing.Point(24, 430);
            this.btn_pause.Name = "btn_pause";
            this.btn_pause.Size = new System.Drawing.Size(133, 29);
            this.btn_pause.TabIndex = 3;
            this.btn_pause.Text = "Pause";
            this.btn_pause.UseVisualStyleBackColor = true;
            this.btn_pause.Click += new System.EventHandler(this.btn_pause_Click);
            // 
            // btn_continuous
            // 
            this.btn_continuous.ForeColor = System.Drawing.Color.Black;
            this.btn_continuous.Location = new System.Drawing.Point(24, 465);
            this.btn_continuous.Name = "btn_continuous";
            this.btn_continuous.Size = new System.Drawing.Size(133, 30);
            this.btn_continuous.TabIndex = 3;
            this.btn_continuous.Text = "Continuous";
            this.btn_continuous.UseVisualStyleBackColor = true;
            this.btn_continuous.Click += new System.EventHandler(this.btn_continuous_Click);
            // 
            // btn_Avoid
            // 
            this.btn_Avoid.Enabled = false;
            this.btn_Avoid.ForeColor = System.Drawing.Color.Black;
            this.btn_Avoid.Location = new System.Drawing.Point(24, 501);
            this.btn_Avoid.Name = "btn_Avoid";
            this.btn_Avoid.Size = new System.Drawing.Size(133, 31);
            this.btn_Avoid.TabIndex = 2;
            this.btn_Avoid.Text = "Avoid";
            this.btn_Avoid.UseVisualStyleBackColor = true;
            this.btn_Avoid.Click += new System.EventHandler(this.btn_Avoid_Click);
            // 
            // btn_start
            // 
            this.btn_start.Enabled = false;
            this.btn_start.ForeColor = System.Drawing.Color.Black;
            this.btn_start.Location = new System.Drawing.Point(24, 397);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(133, 27);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // cmb_fromAddress
            // 
            this.cmb_fromAddress.FormattingEnabled = true;
            this.cmb_fromAddress.Location = new System.Drawing.Point(10, 258);
            this.cmb_fromAddress.Name = "cmb_fromAddress";
            this.cmb_fromAddress.Size = new System.Drawing.Size(162, 27);
            this.cmb_fromAddress.TabIndex = 1;
            // 
            // cmb_toAddress
            // 
            this.cmb_toAddress.FormattingEnabled = true;
            this.cmb_toAddress.Location = new System.Drawing.Point(10, 310);
            this.cmb_toAddress.Name = "cmb_toAddress";
            this.cmb_toAddress.Size = new System.Drawing.Size(162, 27);
            this.cmb_toAddress.TabIndex = 1;
            // 
            // cb_sectionThroughTimes
            // 
            this.cb_sectionThroughTimes.AutoSize = true;
            this.cb_sectionThroughTimes.ForeColor = System.Drawing.Color.Transparent;
            this.cb_sectionThroughTimes.Location = new System.Drawing.Point(23, 662);
            this.cb_sectionThroughTimes.Name = "cb_sectionThroughTimes";
            this.cb_sectionThroughTimes.Size = new System.Drawing.Size(217, 23);
            this.cb_sectionThroughTimes.TabIndex = 3;
            this.cb_sectionThroughTimes.Text = "Section Through Times";
            this.cb_sectionThroughTimes.UseVisualStyleBackColor = true;
            this.cb_sectionThroughTimes.Visible = false;
            this.cb_sectionThroughTimes.CheckedChanged += new System.EventHandler(this.cb_sectionThroughTimes_CheckedChanged);
            this.cb_sectionThroughTimes.Click += new System.EventHandler(this.cb_sectionThroughTimes_Click);
            // 
            // ck_montor_vh
            // 
            this.ck_montor_vh.AutoSize = true;
            this.ck_montor_vh.ForeColor = System.Drawing.Color.Transparent;
            this.ck_montor_vh.Location = new System.Drawing.Point(23, 633);
            this.ck_montor_vh.Name = "ck_montor_vh";
            this.ck_montor_vh.Size = new System.Drawing.Size(118, 23);
            this.ck_montor_vh.TabIndex = 2;
            this.ck_montor_vh.Text = "Monitor Vh";
            this.ck_montor_vh.UseVisualStyleBackColor = true;
            this.ck_montor_vh.Visible = false;
            this.ck_montor_vh.CheckedChanged += new System.EventHandler(this.ck_montor_vh_CheckedChanged);
            // 
            // cbm_Action
            // 
            this.cbm_Action.FormattingEnabled = true;
            this.cbm_Action.Location = new System.Drawing.Point(11, 110);
            this.cbm_Action.Name = "cbm_Action";
            this.cbm_Action.Size = new System.Drawing.Size(162, 27);
            this.cbm_Action.TabIndex = 1;
            this.cbm_Action.SelectedIndexChanged += new System.EventHandler(this.cbm_Action_SelectedIndexChanged);
            // 
            // cmb_Vehicle
            // 
            this.cmb_Vehicle.FormattingEnabled = true;
            this.cmb_Vehicle.Location = new System.Drawing.Point(6, 58);
            this.cmb_Vehicle.Name = "cmb_Vehicle";
            this.cmb_Vehicle.Size = new System.Drawing.Size(162, 27);
            this.cmb_Vehicle.TabIndex = 1;
            // 
            // cb_autoTip
            // 
            this.cb_autoTip.AutoSize = true;
            this.cb_autoTip.Checked = true;
            this.cb_autoTip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_autoTip.ForeColor = System.Drawing.Color.Transparent;
            this.cb_autoTip.Location = new System.Drawing.Point(24, 575);
            this.cb_autoTip.Name = "cb_autoTip";
            this.cb_autoTip.Size = new System.Drawing.Size(127, 23);
            this.cb_autoTip.TabIndex = 1;
            this.cb_autoTip.Text = "Tip Message";
            this.cb_autoTip.UseVisualStyleBackColor = true;
            this.cb_autoTip.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // lbl_sourceName
            // 
            this.lbl_sourceName.AutoSize = true;
            this.lbl_sourceName.Location = new System.Drawing.Point(6, 236);
            this.lbl_sourceName.Name = "lbl_sourceName";
            this.lbl_sourceName.Size = new System.Drawing.Size(117, 19);
            this.lbl_sourceName.TabIndex = 0;
            this.lbl_sourceName.Text = "From Address";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 19);
            this.label5.TabIndex = 0;
            this.label5.Text = "Action";
            // 
            // lbl_destinationName
            // 
            this.lbl_destinationName.AutoSize = true;
            this.lbl_destinationName.Location = new System.Drawing.Point(6, 288);
            this.lbl_destinationName.Name = "lbl_destinationName";
            this.lbl_destinationName.Size = new System.Drawing.Size(99, 19);
            this.lbl_destinationName.TabIndex = 0;
            this.lbl_destinationName.Text = "To Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "Vehicle";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uc_StatusTreeViewer1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(315, 829);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Status Viewer";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uc_StatusTreeViewer1
            // 
            this.uc_StatusTreeViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uc_StatusTreeViewer1.Location = new System.Drawing.Point(3, 3);
            this.uc_StatusTreeViewer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uc_StatusTreeViewer1.Name = "uc_StatusTreeViewer1";
            this.uc_StatusTreeViewer1.Size = new System.Drawing.Size(309, 823);
            this.uc_StatusTreeViewer1.TabIndex = 0;
            // 
            // pnl_Map
            // 
            this.pnl_Map.AutoScroll = true;
            this.pnl_Map.Controls.Add(this.uctl_Map);
            this.pnl_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Map.Location = new System.Drawing.Point(3, 3);
            this.pnl_Map.Name = "pnl_Map";
            this.pnl_Map.Size = new System.Drawing.Size(1589, 855);
            this.pnl_Map.TabIndex = 0;
            // 
            // uctl_Map
            // 
            this.uctl_Map.AutoScroll = true;
            this.uctl_Map.BackColor = System.Drawing.Color.MidnightBlue;
            this.uctl_Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctl_Map.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uctl_Map.Location = new System.Drawing.Point(0, 0);
            this.uctl_Map.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uctl_Map.Name = "uctl_Map";
            this.uctl_Map.Size = new System.Drawing.Size(1589, 855);
            this.uctl_Map.TabIndex = 0;
            this.uctl_Map.Load += new System.EventHandler(this.uctl_Map_Load);
            // 
            // tbcList
            // 
            this.tbcList.Controls.Add(this.tab_vhStatus);
            this.tbcList.Controls.Add(this.tapTransferCmd);
            this.tbcList.Controls.Add(this.tapDetail);
            this.tbcList.Controls.Add(this.tapCurrentAlarm);
            this.tbcList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.tbcList.ItemSize = new System.Drawing.Size(160, 23);
            this.tbcList.Location = new System.Drawing.Point(0, 0);
            this.tbcList.Name = "tbcList";
            this.tbcList.SelectedIndex = 0;
            this.tbcList.Size = new System.Drawing.Size(1924, 187);
            this.tbcList.TabIndex = 2;
            // 
            // tab_vhStatus
            // 
            this.tab_vhStatus.Controls.Add(this.dgv_vhStatus);
            this.tab_vhStatus.Location = new System.Drawing.Point(4, 27);
            this.tab_vhStatus.Name = "tab_vhStatus";
            this.tab_vhStatus.Padding = new System.Windows.Forms.Padding(3);
            this.tab_vhStatus.Size = new System.Drawing.Size(1916, 156);
            this.tab_vhStatus.TabIndex = 5;
            this.tab_vhStatus.Text = "Vehicle Status            ";
            this.tab_vhStatus.UseVisualStyleBackColor = true;
            // 
            // dgv_vhStatus
            // 
            this.dgv_vhStatus.AllowUserToAddRows = false;
            this.dgv_vhStatus.AllowUserToDeleteRows = false;
            this.dgv_vhStatus.AutoGenerateColumns = false;
            this.dgv_vhStatus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_vhStatus.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_vhStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_vhStatus.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.vEHICLEIDDataGridViewTextBoxColumn1,
            this.mODESTATUSDataGridViewTextBoxColumn1,
            this.aCTSTATUSDataGridViewTextBoxColumn1,
            this.mCSCMDDataGridViewTextBoxColumn1,
            this.oHTCCMDDataGridViewTextBoxColumn1,
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1,
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1,
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1,
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1,
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1,
            this.iSPARKINGDataGridViewCheckBoxColumn1,
            this.pARKTIMEDataGridViewTextBoxColumn1,
            this.iSCYCLINGDataGridViewCheckBoxColumn1,
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1,
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn1,
            this.bATTERYCAPACITYDataGridViewTextBoxColumn,
            this.uPDTIMEDataGridViewTextBoxColumn1,
            this.LoadingInterlockErrorRetryTimes,
            this.UnloadingInterlockErrorRetryTimes,
            this.CurrentVhSettingOfBattryLowLevelValue,
            this.Column1});
            this.dgv_vhStatus.DataSource = this.vehicleObjToShowBindingSource;
            this.dgv_vhStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_vhStatus.Location = new System.Drawing.Point(3, 3);
            this.dgv_vhStatus.MultiSelect = false;
            this.dgv_vhStatus.Name = "dgv_vhStatus";
            this.dgv_vhStatus.ReadOnly = true;
            this.dgv_vhStatus.RowTemplate.Height = 24;
            this.dgv_vhStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_vhStatus.Size = new System.Drawing.Size(1910, 150);
            this.dgv_vhStatus.TabIndex = 0;
            this.dgv_vhStatus.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_vhStatus_CellClick);
            this.dgv_vhStatus.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_vhStatus_DataError);
            // 
            // vehicleObjToShowBindingSource
            // 
            this.vehicleObjToShowBindingSource.DataSource = typeof(com.mirle.ibg3k0.sc.ObjectRelay.VehicleObjToShow);
            // 
            // tapTransferCmd
            // 
            this.tapTransferCmd.BackColor = System.Drawing.SystemColors.Control;
            this.tapTransferCmd.Controls.Add(this.dgv_TransferCommand);
            this.tapTransferCmd.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.tapTransferCmd.Location = new System.Drawing.Point(4, 27);
            this.tapTransferCmd.Margin = new System.Windows.Forms.Padding(0);
            this.tapTransferCmd.Name = "tapTransferCmd";
            this.tapTransferCmd.Size = new System.Drawing.Size(1916, 156);
            this.tapTransferCmd.TabIndex = 0;
            this.tapTransferCmd.Text = "Transfer Command            ";
            // 
            // dgv_TransferCommand
            // 
            this.dgv_TransferCommand.AllowUserToAddRows = false;
            this.dgv_TransferCommand.AutoGenerateColumns = false;
            this.dgv_TransferCommand.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_TransferCommand.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_TransferCommand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_TransferCommand.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cMDIDDataGridViewTextBoxColumn,
            this.cARRIERIDDataGridViewTextBoxColumn,
            this.VEHICLE_ID,
            this.tRANSFERSTATEDataGridViewTextBoxColumn,
            this.hOSTSOURCEDataGridViewTextBoxColumn,
            this.hOSTDESTINATIONDataGridViewTextBoxColumn,
            this.pRIORITYDataGridViewTextBoxColumn,
            this.cMDINSERTIMEDataGridViewTextBoxColumn,
            this.cMDSTARTTIMEDataGridViewTextBoxColumn,
            this.rEPLACEDataGridViewTextBoxColumn});
            this.dgv_TransferCommand.DataSource = this.cMDMCSObjToShowBindingSource;
            this.dgv_TransferCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_TransferCommand.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgv_TransferCommand.Location = new System.Drawing.Point(0, 0);
            this.dgv_TransferCommand.Name = "dgv_TransferCommand";
            this.dgv_TransferCommand.ReadOnly = true;
            this.dgv_TransferCommand.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_TransferCommand.RowTemplate.Height = 24;
            this.dgv_TransferCommand.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_TransferCommand.Size = new System.Drawing.Size(1916, 156);
            this.dgv_TransferCommand.TabIndex = 0;
            // 
            // cMDIDDataGridViewTextBoxColumn
            // 
            this.cMDIDDataGridViewTextBoxColumn.DataPropertyName = "CMD_ID";
            this.cMDIDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.cMDIDDataGridViewTextBoxColumn.Name = "cMDIDDataGridViewTextBoxColumn";
            this.cMDIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cARRIERIDDataGridViewTextBoxColumn
            // 
            this.cARRIERIDDataGridViewTextBoxColumn.DataPropertyName = "CARRIER_ID";
            this.cARRIERIDDataGridViewTextBoxColumn.HeaderText = "Carrier ID";
            this.cARRIERIDDataGridViewTextBoxColumn.Name = "cARRIERIDDataGridViewTextBoxColumn";
            this.cARRIERIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // VEHICLE_ID
            // 
            this.VEHICLE_ID.DataPropertyName = "VEHICLE_ID";
            this.VEHICLE_ID.HeaderText = "Vh";
            this.VEHICLE_ID.Name = "VEHICLE_ID";
            this.VEHICLE_ID.ReadOnly = true;
            // 
            // tRANSFERSTATEDataGridViewTextBoxColumn
            // 
            this.tRANSFERSTATEDataGridViewTextBoxColumn.DataPropertyName = "TRANSFERSTATEToShow";
            this.tRANSFERSTATEDataGridViewTextBoxColumn.FillWeight = 60F;
            this.tRANSFERSTATEDataGridViewTextBoxColumn.HeaderText = "State";
            this.tRANSFERSTATEDataGridViewTextBoxColumn.Name = "tRANSFERSTATEDataGridViewTextBoxColumn";
            this.tRANSFERSTATEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hOSTSOURCEDataGridViewTextBoxColumn
            // 
            this.hOSTSOURCEDataGridViewTextBoxColumn.DataPropertyName = "HOSTSOURCE";
            this.hOSTSOURCEDataGridViewTextBoxColumn.FillWeight = 150F;
            this.hOSTSOURCEDataGridViewTextBoxColumn.HeaderText = "L Port";
            this.hOSTSOURCEDataGridViewTextBoxColumn.Name = "hOSTSOURCEDataGridViewTextBoxColumn";
            this.hOSTSOURCEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hOSTDESTINATIONDataGridViewTextBoxColumn
            // 
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.DataPropertyName = "HOSTDESTINATION";
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.FillWeight = 150F;
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.HeaderText = "U Port";
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.Name = "hOSTDESTINATIONDataGridViewTextBoxColumn";
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pRIORITYDataGridViewTextBoxColumn
            // 
            this.pRIORITYDataGridViewTextBoxColumn.DataPropertyName = "PRIORITY";
            this.pRIORITYDataGridViewTextBoxColumn.FillWeight = 70F;
            this.pRIORITYDataGridViewTextBoxColumn.HeaderText = "Priority";
            this.pRIORITYDataGridViewTextBoxColumn.Name = "pRIORITYDataGridViewTextBoxColumn";
            this.pRIORITYDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cMDINSERTIMEDataGridViewTextBoxColumn
            // 
            this.cMDINSERTIMEDataGridViewTextBoxColumn.DataPropertyName = "CMD_INSER_TIME";
            this.cMDINSERTIMEDataGridViewTextBoxColumn.FillWeight = 120F;
            this.cMDINSERTIMEDataGridViewTextBoxColumn.HeaderText = "Inser Time";
            this.cMDINSERTIMEDataGridViewTextBoxColumn.Name = "cMDINSERTIMEDataGridViewTextBoxColumn";
            this.cMDINSERTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cMDSTARTTIMEDataGridViewTextBoxColumn
            // 
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "CMD_START_TIME";
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.FillWeight = 120F;
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.HeaderText = "Start Time";
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.Name = "cMDSTARTTIMEDataGridViewTextBoxColumn";
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // rEPLACEDataGridViewTextBoxColumn
            // 
            this.rEPLACEDataGridViewTextBoxColumn.DataPropertyName = "REPLACE";
            this.rEPLACEDataGridViewTextBoxColumn.FillWeight = 50F;
            this.rEPLACEDataGridViewTextBoxColumn.HeaderText = "Replace";
            this.rEPLACEDataGridViewTextBoxColumn.Name = "rEPLACEDataGridViewTextBoxColumn";
            this.rEPLACEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cMDMCSObjToShowBindingSource
            // 
            this.cMDMCSObjToShowBindingSource.DataSource = typeof(com.mirle.ibg3k0.sc.ObjectRelay.CMD_MCSObjToShow);
            // 
            // tapDetail
            // 
            this.tapDetail.Controls.Add(this.dgv_TaskCommand);
            this.tapDetail.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.tapDetail.Location = new System.Drawing.Point(4, 27);
            this.tapDetail.Margin = new System.Windows.Forms.Padding(0);
            this.tapDetail.Name = "tapDetail";
            this.tapDetail.Size = new System.Drawing.Size(1916, 156);
            this.tapDetail.TabIndex = 1;
            this.tapDetail.Text = "Command Detail            ";
            this.tapDetail.UseVisualStyleBackColor = true;
            // 
            // dgv_TaskCommand
            // 
            this.dgv_TaskCommand.AllowUserToAddRows = false;
            this.dgv_TaskCommand.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_TaskCommand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_TaskCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_TaskCommand.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgv_TaskCommand.Location = new System.Drawing.Point(0, 0);
            this.dgv_TaskCommand.Name = "dgv_TaskCommand";
            this.dgv_TaskCommand.ReadOnly = true;
            this.dgv_TaskCommand.RowTemplate.Height = 24;
            this.dgv_TaskCommand.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_TaskCommand.Size = new System.Drawing.Size(1916, 156);
            this.dgv_TaskCommand.TabIndex = 0;
            // 
            // tapCurrentAlarm
            // 
            this.tapCurrentAlarm.Controls.Add(this.tlp_crtAlarm);
            this.tapCurrentAlarm.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.tapCurrentAlarm.Location = new System.Drawing.Point(4, 27);
            this.tapCurrentAlarm.Name = "tapCurrentAlarm";
            this.tapCurrentAlarm.Size = new System.Drawing.Size(1916, 156);
            this.tapCurrentAlarm.TabIndex = 2;
            this.tapCurrentAlarm.Text = "Current  Alarm            ";
            this.tapCurrentAlarm.UseVisualStyleBackColor = true;
            // 
            // tlp_crtAlarm
            // 
            this.tlp_crtAlarm.ColumnCount = 1;
            this.tlp_crtAlarm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_crtAlarm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_crtAlarm.Controls.Add(this.dgv_Alarm, 0, 0);
            this.tlp_crtAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_crtAlarm.Location = new System.Drawing.Point(0, 0);
            this.tlp_crtAlarm.Name = "tlp_crtAlarm";
            this.tlp_crtAlarm.RowCount = 1;
            this.tlp_crtAlarm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_crtAlarm.Size = new System.Drawing.Size(1916, 156);
            this.tlp_crtAlarm.TabIndex = 1;
            // 
            // dgv_Alarm
            // 
            this.dgv_Alarm.AllowUserToAddRows = false;
            this.dgv_Alarm.AllowUserToDeleteRows = false;
            this.dgv_Alarm.AllowUserToOrderColumns = true;
            this.dgv_Alarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_Alarm.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_Alarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Alarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eqpt_id,
            this.alarm_code,
            this.alarm_lvl,
            this.report_time,
            this.alarm_desc});
            this.dgv_Alarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Alarm.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgv_Alarm.Location = new System.Drawing.Point(3, 3);
            this.dgv_Alarm.Name = "dgv_Alarm";
            this.dgv_Alarm.ReadOnly = true;
            this.dgv_Alarm.RowTemplate.Height = 24;
            this.dgv_Alarm.Size = new System.Drawing.Size(1910, 150);
            this.dgv_Alarm.TabIndex = 0;
            // 
            // eqpt_id
            // 
            this.eqpt_id.DataPropertyName = "EQPT_ID";
            this.eqpt_id.HeaderText = "EQPT ID";
            this.eqpt_id.Name = "eqpt_id";
            this.eqpt_id.ReadOnly = true;
            // 
            // alarm_code
            // 
            this.alarm_code.DataPropertyName = "ALAM_CODE";
            this.alarm_code.HeaderText = "Code";
            this.alarm_code.Name = "alarm_code";
            this.alarm_code.ReadOnly = true;
            // 
            // alarm_lvl
            // 
            this.alarm_lvl.DataPropertyName = "ALAM_LVL";
            this.alarm_lvl.HeaderText = "Level";
            this.alarm_lvl.Name = "alarm_lvl";
            this.alarm_lvl.ReadOnly = true;
            // 
            // report_time
            // 
            this.report_time.DataPropertyName = "RPT_DATE_TIME";
            this.report_time.HeaderText = "Time";
            this.report_time.Name = "report_time";
            this.report_time.ReadOnly = true;
            // 
            // alarm_desc
            // 
            this.alarm_desc.DataPropertyName = "ALAM_DESC";
            this.alarm_desc.FillWeight = 200F;
            this.alarm_desc.HeaderText = "Description";
            this.alarm_desc.Name = "alarm_desc";
            this.alarm_desc.ReadOnly = true;
            // 
            // timer_TimedUpdates
            // 
            this.timer_TimedUpdates.Interval = 1000;
            this.timer_TimedUpdates.Tick += new System.EventHandler(this.timer_TimedUpdates_Tick);
            // 
            // uPDTIMEDataGridViewTextBoxColumn
            // 
            this.uPDTIMEDataGridViewTextBoxColumn.DataPropertyName = "UPD_TIME";
            this.uPDTIMEDataGridViewTextBoxColumn.HeaderText = "UPD_TIME";
            this.uPDTIMEDataGridViewTextBoxColumn.Name = "uPDTIMEDataGridViewTextBoxColumn";
            this.uPDTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.uPDTIMEDataGridViewTextBoxColumn.Width = 116;
            // 
            // BATTERYCAPACITY
            // 
            this.BATTERYCAPACITY.DataPropertyName = "BATTERYCAPACITY";
            this.BATTERYCAPACITY.HeaderText = "Battery Capacity";
            this.BATTERYCAPACITY.Name = "BATTERYCAPACITY";
            this.BATTERYCAPACITY.ReadOnly = true;
            this.BATTERYCAPACITY.Width = 160;
            // 
            // aCCSECDIST2ShowDataGridViewTextBoxColumn
            // 
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn.DataPropertyName = "ACC_SEC_DIST2Show";
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn.HeaderText = "Sec DIST(m)";
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn.Name = "aCCSECDIST2ShowDataGridViewTextBoxColumn";
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn.ReadOnly = true;
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn.Width = 128;
            // 
            // cYCLERUNTIMEDataGridViewTextBoxColumn
            // 
            this.cYCLERUNTIMEDataGridViewTextBoxColumn.DataPropertyName = "CYCLERUN_TIME";
            this.cYCLERUNTIMEDataGridViewTextBoxColumn.HeaderText = "Cycling time";
            this.cYCLERUNTIMEDataGridViewTextBoxColumn.Name = "cYCLERUNTIMEDataGridViewTextBoxColumn";
            this.cYCLERUNTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.cYCLERUNTIMEDataGridViewTextBoxColumn.Width = 128;
            // 
            // iSCYCLINGDataGridViewCheckBoxColumn
            // 
            this.iSCYCLINGDataGridViewCheckBoxColumn.DataPropertyName = "IS_CYCLING";
            this.iSCYCLINGDataGridViewCheckBoxColumn.HeaderText = "Cycling";
            this.iSCYCLINGDataGridViewCheckBoxColumn.Name = "iSCYCLINGDataGridViewCheckBoxColumn";
            this.iSCYCLINGDataGridViewCheckBoxColumn.ReadOnly = true;
            this.iSCYCLINGDataGridViewCheckBoxColumn.Width = 73;
            // 
            // pARKTIMEDataGridViewTextBoxColumn
            // 
            this.pARKTIMEDataGridViewTextBoxColumn.DataPropertyName = "PARK_TIME";
            this.pARKTIMEDataGridViewTextBoxColumn.HeaderText = "Park time";
            this.pARKTIMEDataGridViewTextBoxColumn.Name = "pARKTIMEDataGridViewTextBoxColumn";
            this.pARKTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.pARKTIMEDataGridViewTextBoxColumn.Width = 105;
            // 
            // iSPARKINGDataGridViewCheckBoxColumn
            // 
            this.iSPARKINGDataGridViewCheckBoxColumn.DataPropertyName = "IS_PARKING";
            this.iSPARKINGDataGridViewCheckBoxColumn.HeaderText = "Parking";
            this.iSPARKINGDataGridViewCheckBoxColumn.Name = "iSPARKINGDataGridViewCheckBoxColumn";
            this.iSPARKINGDataGridViewCheckBoxColumn.ReadOnly = true;
            this.iSPARKINGDataGridViewCheckBoxColumn.Width = 74;
            // 
            // vEHICLEACCDIST2ShowDataGridViewTextBoxColumn
            // 
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn.DataPropertyName = "vEHICLE_ACC_DIST2Show";
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn.HeaderText = "ODO(km)";
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn.Name = "vEHICLEACCDIST2ShowDataGridViewTextBoxColumn";
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn.ReadOnly = true;
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn.Width = 103;
            // 
            // oBSDIST2ShowDataGridViewTextBoxColumn
            // 
            this.oBSDIST2ShowDataGridViewTextBoxColumn.DataPropertyName = "oBS_DIST2Show";
            this.oBSDIST2ShowDataGridViewTextBoxColumn.HeaderText = "OBS DIST(m)";
            this.oBSDIST2ShowDataGridViewTextBoxColumn.Name = "oBSDIST2ShowDataGridViewTextBoxColumn";
            this.oBSDIST2ShowDataGridViewTextBoxColumn.ReadOnly = true;
            this.oBSDIST2ShowDataGridViewTextBoxColumn.Width = 134;
            // 
            // hIDPAUSE2ShowDataGridViewCheckBoxColumn
            // 
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn.DataPropertyName = "hID_PAUSE2Show";
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn.HeaderText = "HID pause";
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn.Name = "hIDPAUSE2ShowDataGridViewCheckBoxColumn";
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn.ReadOnly = true;
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn.Width = 94;
            // 
            // oBSPAUSE2ShowDataGridViewCheckBoxColumn
            // 
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn.DataPropertyName = "oBS_PAUSE2Show";
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn.HeaderText = "OBS pause";
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn.Name = "oBSPAUSE2ShowDataGridViewCheckBoxColumn";
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn.ReadOnly = true;
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn.Width = 101;
            // 
            // cMDPAUSE2ShowDataGridViewCheckBoxColumn
            // 
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn.DataPropertyName = "cMD_PAUSE2Show";
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn.HeaderText = "CMD pause";
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn.Name = "cMDPAUSE2ShowDataGridViewCheckBoxColumn";
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn.ReadOnly = true;
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn.Width = 103;
            // 
            // bLOCKPAUSE2ShowDataGridViewCheckBoxColumn
            // 
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn.DataPropertyName = "bLOCK_PAUSE2Show";
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn.HeaderText = "Block pause";
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn.Name = "bLOCKPAUSE2ShowDataGridViewCheckBoxColumn";
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn.ReadOnly = true;
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn.Width = 110;
            // 
            // oHTCCMDDataGridViewTextBoxColumn
            // 
            this.oHTCCMDDataGridViewTextBoxColumn.DataPropertyName = "OHTC_CMD";
            this.oHTCCMDDataGridViewTextBoxColumn.HeaderText = "OHTC CMD";
            this.oHTCCMDDataGridViewTextBoxColumn.Name = "oHTCCMDDataGridViewTextBoxColumn";
            this.oHTCCMDDataGridViewTextBoxColumn.ReadOnly = true;
            this.oHTCCMDDataGridViewTextBoxColumn.Width = 121;
            // 
            // mCSCMDDataGridViewTextBoxColumn
            // 
            this.mCSCMDDataGridViewTextBoxColumn.DataPropertyName = "MCS_CMD";
            this.mCSCMDDataGridViewTextBoxColumn.HeaderText = "MCS CMD";
            this.mCSCMDDataGridViewTextBoxColumn.Name = "mCSCMDDataGridViewTextBoxColumn";
            this.mCSCMDDataGridViewTextBoxColumn.ReadOnly = true;
            this.mCSCMDDataGridViewTextBoxColumn.Width = 111;
            // 
            // aCTSTATUSDataGridViewTextBoxColumn
            // 
            this.aCTSTATUSDataGridViewTextBoxColumn.DataPropertyName = "ACT_STATUS";
            this.aCTSTATUSDataGridViewTextBoxColumn.HeaderText = "Action";
            this.aCTSTATUSDataGridViewTextBoxColumn.Name = "aCTSTATUSDataGridViewTextBoxColumn";
            this.aCTSTATUSDataGridViewTextBoxColumn.ReadOnly = true;
            this.aCTSTATUSDataGridViewTextBoxColumn.Width = 83;
            // 
            // mODESTATUSDataGridViewTextBoxColumn
            // 
            this.mODESTATUSDataGridViewTextBoxColumn.DataPropertyName = "MODE_STATUS";
            this.mODESTATUSDataGridViewTextBoxColumn.HeaderText = "Mode";
            this.mODESTATUSDataGridViewTextBoxColumn.Name = "mODESTATUSDataGridViewTextBoxColumn";
            this.mODESTATUSDataGridViewTextBoxColumn.ReadOnly = true;
            this.mODESTATUSDataGridViewTextBoxColumn.Width = 76;
            // 
            // vEHICLEIDDataGridViewTextBoxColumn
            // 
            this.vEHICLEIDDataGridViewTextBoxColumn.DataPropertyName = "VEHICLE_ID";
            this.vEHICLEIDDataGridViewTextBoxColumn.HeaderText = "Vh ID";
            this.vEHICLEIDDataGridViewTextBoxColumn.Name = "vEHICLEIDDataGridViewTextBoxColumn";
            this.vEHICLEIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.vEHICLEIDDataGridViewTextBoxColumn.Width = 75;
            // 
            // vehicleObjToShowBindingSource1
            // 
            this.vehicleObjToShowBindingSource1.AllowNew = true;
            this.vehicleObjToShowBindingSource1.DataSource = typeof(com.mirle.ibg3k0.sc.ObjectRelay.VehicleObjToShow);
            // 
            // aCMDMCSBindingSource
            // 
            this.aCMDMCSBindingSource.DataSource = typeof(com.mirle.ibg3k0.sc.ACMD_MCS);
            // 
            // vEHICLEIDDataGridViewTextBoxColumn1
            // 
            this.vEHICLEIDDataGridViewTextBoxColumn1.DataPropertyName = "VEHICLE_ID";
            this.vEHICLEIDDataGridViewTextBoxColumn1.HeaderText = "Vh ID";
            this.vEHICLEIDDataGridViewTextBoxColumn1.Name = "vEHICLEIDDataGridViewTextBoxColumn1";
            this.vEHICLEIDDataGridViewTextBoxColumn1.ReadOnly = true;
            this.vEHICLEIDDataGridViewTextBoxColumn1.Width = 75;
            // 
            // mODESTATUSDataGridViewTextBoxColumn1
            // 
            this.mODESTATUSDataGridViewTextBoxColumn1.DataPropertyName = "MODE_STATUS";
            this.mODESTATUSDataGridViewTextBoxColumn1.FillWeight = 150F;
            this.mODESTATUSDataGridViewTextBoxColumn1.HeaderText = "Mode               ";
            this.mODESTATUSDataGridViewTextBoxColumn1.Name = "mODESTATUSDataGridViewTextBoxColumn1";
            this.mODESTATUSDataGridViewTextBoxColumn1.ReadOnly = true;
            this.mODESTATUSDataGridViewTextBoxColumn1.Width = 136;
            // 
            // aCTSTATUSDataGridViewTextBoxColumn1
            // 
            this.aCTSTATUSDataGridViewTextBoxColumn1.DataPropertyName = "ACT_STATUS";
            this.aCTSTATUSDataGridViewTextBoxColumn1.HeaderText = "Action";
            this.aCTSTATUSDataGridViewTextBoxColumn1.Name = "aCTSTATUSDataGridViewTextBoxColumn1";
            this.aCTSTATUSDataGridViewTextBoxColumn1.ReadOnly = true;
            this.aCTSTATUSDataGridViewTextBoxColumn1.Width = 83;
            // 
            // mCSCMDDataGridViewTextBoxColumn1
            // 
            this.mCSCMDDataGridViewTextBoxColumn1.DataPropertyName = "MCS_CMD";
            this.mCSCMDDataGridViewTextBoxColumn1.HeaderText = "MCS CMD";
            this.mCSCMDDataGridViewTextBoxColumn1.Name = "mCSCMDDataGridViewTextBoxColumn1";
            this.mCSCMDDataGridViewTextBoxColumn1.ReadOnly = true;
            this.mCSCMDDataGridViewTextBoxColumn1.Width = 111;
            // 
            // oHTCCMDDataGridViewTextBoxColumn1
            // 
            this.oHTCCMDDataGridViewTextBoxColumn1.DataPropertyName = "OHTC_CMD";
            this.oHTCCMDDataGridViewTextBoxColumn1.HeaderText = "OHTC CMD";
            this.oHTCCMDDataGridViewTextBoxColumn1.Name = "oHTCCMDDataGridViewTextBoxColumn1";
            this.oHTCCMDDataGridViewTextBoxColumn1.ReadOnly = true;
            this.oHTCCMDDataGridViewTextBoxColumn1.Width = 121;
            // 
            // bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1
            // 
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1.DataPropertyName = "bLOCK_PAUSE2Show";
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1.HeaderText = "Block pause";
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1.Name = "bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1";
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1.Visible = false;
            this.bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1.Width = 110;
            // 
            // cMDPAUSE2ShowDataGridViewCheckBoxColumn1
            // 
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1.DataPropertyName = "cMD_PAUSE2Show";
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1.HeaderText = "CMD pause";
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1.Name = "cMDPAUSE2ShowDataGridViewCheckBoxColumn1";
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1.Visible = false;
            this.cMDPAUSE2ShowDataGridViewCheckBoxColumn1.Width = 103;
            // 
            // oBSPAUSE2ShowDataGridViewCheckBoxColumn1
            // 
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1.DataPropertyName = "oBS_PAUSE2Show";
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1.HeaderText = "OBS pause";
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1.Name = "oBSPAUSE2ShowDataGridViewCheckBoxColumn1";
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1.Visible = false;
            this.oBSPAUSE2ShowDataGridViewCheckBoxColumn1.Width = 101;
            // 
            // hIDPAUSE2ShowDataGridViewCheckBoxColumn1
            // 
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1.DataPropertyName = "hID_PAUSE2Show";
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1.HeaderText = "HID pause";
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1.Name = "hIDPAUSE2ShowDataGridViewCheckBoxColumn1";
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1.Visible = false;
            this.hIDPAUSE2ShowDataGridViewCheckBoxColumn1.Width = 94;
            // 
            // vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1
            // 
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1.DataPropertyName = "vEHICLE_ACC_DIST2Show";
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1.HeaderText = "ODO(km)";
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1.Name = "vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1";
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1.ReadOnly = true;
            this.vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1.Width = 103;
            // 
            // iSPARKINGDataGridViewCheckBoxColumn1
            // 
            this.iSPARKINGDataGridViewCheckBoxColumn1.DataPropertyName = "IS_PARKING";
            this.iSPARKINGDataGridViewCheckBoxColumn1.HeaderText = "Parking";
            this.iSPARKINGDataGridViewCheckBoxColumn1.Name = "iSPARKINGDataGridViewCheckBoxColumn1";
            this.iSPARKINGDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.iSPARKINGDataGridViewCheckBoxColumn1.Visible = false;
            this.iSPARKINGDataGridViewCheckBoxColumn1.Width = 74;
            // 
            // pARKTIMEDataGridViewTextBoxColumn1
            // 
            this.pARKTIMEDataGridViewTextBoxColumn1.DataPropertyName = "PARK_TIME";
            this.pARKTIMEDataGridViewTextBoxColumn1.HeaderText = "Park time";
            this.pARKTIMEDataGridViewTextBoxColumn1.Name = "pARKTIMEDataGridViewTextBoxColumn1";
            this.pARKTIMEDataGridViewTextBoxColumn1.ReadOnly = true;
            this.pARKTIMEDataGridViewTextBoxColumn1.Visible = false;
            this.pARKTIMEDataGridViewTextBoxColumn1.Width = 105;
            // 
            // iSCYCLINGDataGridViewCheckBoxColumn1
            // 
            this.iSCYCLINGDataGridViewCheckBoxColumn1.DataPropertyName = "IS_CYCLING";
            this.iSCYCLINGDataGridViewCheckBoxColumn1.HeaderText = "Cycling";
            this.iSCYCLINGDataGridViewCheckBoxColumn1.Name = "iSCYCLINGDataGridViewCheckBoxColumn1";
            this.iSCYCLINGDataGridViewCheckBoxColumn1.ReadOnly = true;
            this.iSCYCLINGDataGridViewCheckBoxColumn1.Visible = false;
            this.iSCYCLINGDataGridViewCheckBoxColumn1.Width = 73;
            // 
            // cYCLERUNTIMEDataGridViewTextBoxColumn1
            // 
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1.DataPropertyName = "CYCLERUN_TIME";
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1.HeaderText = "Cycling time";
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1.Name = "cYCLERUNTIMEDataGridViewTextBoxColumn1";
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1.ReadOnly = true;
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1.Visible = false;
            this.cYCLERUNTIMEDataGridViewTextBoxColumn1.Width = 128;
            // 
            // aCCSECDIST2ShowDataGridViewTextBoxColumn1
            // 
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn1.DataPropertyName = "ACC_SEC_DIST2Show";
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn1.HeaderText = "Sec DIST(m)";
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn1.Name = "aCCSECDIST2ShowDataGridViewTextBoxColumn1";
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn1.ReadOnly = true;
            this.aCCSECDIST2ShowDataGridViewTextBoxColumn1.Width = 128;
            // 
            // bATTERYCAPACITYDataGridViewTextBoxColumn
            // 
            this.bATTERYCAPACITYDataGridViewTextBoxColumn.DataPropertyName = "BATTERYCAPACITY";
            this.bATTERYCAPACITYDataGridViewTextBoxColumn.HeaderText = "BATTERYCAPACITY";
            this.bATTERYCAPACITYDataGridViewTextBoxColumn.Name = "bATTERYCAPACITYDataGridViewTextBoxColumn";
            this.bATTERYCAPACITYDataGridViewTextBoxColumn.ReadOnly = true;
            this.bATTERYCAPACITYDataGridViewTextBoxColumn.Width = 188;
            // 
            // uPDTIMEDataGridViewTextBoxColumn1
            // 
            this.uPDTIMEDataGridViewTextBoxColumn1.DataPropertyName = "UPD_TIME";
            this.uPDTIMEDataGridViewTextBoxColumn1.HeaderText = "UPD_TIME";
            this.uPDTIMEDataGridViewTextBoxColumn1.Name = "uPDTIMEDataGridViewTextBoxColumn1";
            this.uPDTIMEDataGridViewTextBoxColumn1.ReadOnly = true;
            this.uPDTIMEDataGridViewTextBoxColumn1.Width = 116;
            // 
            // LoadingInterlockErrorRetryTimes
            // 
            this.LoadingInterlockErrorRetryTimes.DataPropertyName = "LoadingInterlockErrorRetryTimes";
            this.LoadingInterlockErrorRetryTimes.HeaderText = "Interlock error retry times(L)";
            this.LoadingInterlockErrorRetryTimes.Name = "LoadingInterlockErrorRetryTimes";
            this.LoadingInterlockErrorRetryTimes.ReadOnly = true;
            this.LoadingInterlockErrorRetryTimes.Width = 245;
            // 
            // UnloadingInterlockErrorRetryTimes
            // 
            this.UnloadingInterlockErrorRetryTimes.DataPropertyName = "UnloadingInterlockErrorRetryTimes";
            this.UnloadingInterlockErrorRetryTimes.HeaderText = "Interlock error retry times(U)";
            this.UnloadingInterlockErrorRetryTimes.Name = "UnloadingInterlockErrorRetryTimes";
            this.UnloadingInterlockErrorRetryTimes.ReadOnly = true;
            this.UnloadingInterlockErrorRetryTimes.Width = 247;
            // 
            // CurrentVhSettingOfBattryLowLevelValue
            // 
            this.CurrentVhSettingOfBattryLowLevelValue.DataPropertyName = "CurrentVhSettingOfBattryLowLevelValue";
            this.CurrentVhSettingOfBattryLowLevelValue.HeaderText = "Battry Level(Low)";
            this.CurrentVhSettingOfBattryLowLevelValue.Name = "CurrentVhSettingOfBattryLowLevelValue";
            this.CurrentVhSettingOfBattryLowLevelValue.ReadOnly = true;
            this.CurrentVhSettingOfBattryLowLevelValue.Width = 168;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OHT_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1924, 1054);
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OHT_Form";
            this.Load += new System.EventHandler(this.OHT_Form_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb_PortNameType.ResumeLayout(false);
            this.gb_PortNameType.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.pnl_Map.ResumeLayout(false);
            this.tbcList.ResumeLayout(false);
            this.tab_vhStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_vhStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vehicleObjToShowBindingSource)).EndInit();
            this.tapTransferCmd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TransferCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cMDMCSObjToShowBindingSource)).EndInit();
            this.tapDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TaskCommand)).EndInit();
            this.tapCurrentAlarm.ResumeLayout(false);
            this.tlp_crtAlarm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Alarm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vehicleObjToShowBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCMDMCSBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnl_Map;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbl_destinationName;
        private System.Windows.Forms.ComboBox cmb_toAddress;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmb_Vehicle;
        private System.Windows.Forms.ComboBox cmb_fromAddress;
        private System.Windows.Forms.Label lbl_sourceName;
        private System.Windows.Forms.Timer timer_TimedUpdates;
        private System.Windows.Forms.Button btn_continuous;
        private System.Windows.Forms.Button btn_pause;
        private System.Windows.Forms.CheckBox cb_autoTip;
        private System.Windows.Forms.ComboBox cbm_Action;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmb_cycRunZone;
        private System.Windows.Forms.RadioButton Raid_PortNameType_PortID;
        private System.Windows.Forms.GroupBox gb_PortNameType;
        private System.Windows.Forms.RadioButton Raid_PortNameType_AdrID;
        private System.Windows.Forms.CheckBox ck_montor_vh;
        private System.Windows.Forms.CheckBox cb_sectionThroughTimes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_isMaster;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_hostconnAndMode;
        private System.Windows.Forms.Label lbl_RediStat;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_earthqualeHappend;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_detectionSystemExist;
        private System.Windows.Forms.Label label10;
        private Components.uctl_Map uctl_Map;
        private System.Windows.Forms.BindingSource vehicleObjToShowBindingSource1;
        private System.Windows.Forms.Label lbl_SCState;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.BindingSource vehicleObjToShowBindingSource;
        private System.Windows.Forms.BindingSource aCMDMCSBindingSource;
        private System.Windows.Forms.BindingSource cMDMCSObjToShowBindingSource;
        private System.Windows.Forms.TextBox txt_cst_id;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_Avoid;
        private System.Windows.Forms.CheckBox cb_autoOverride;
        private System.Windows.Forms.TabControl tbcList;
        private System.Windows.Forms.TabPage tab_vhStatus;
        private System.Windows.Forms.DataGridView dgv_vhStatus;
        private System.Windows.Forms.TabPage tapTransferCmd;
        private System.Windows.Forms.DataGridView dgv_TransferCommand;
        private System.Windows.Forms.TabPage tapDetail;
        private System.Windows.Forms.DataGridView dgv_TaskCommand;
        private System.Windows.Forms.TabPage tapCurrentAlarm;
        private System.Windows.Forms.TableLayoutPanel tlp_crtAlarm;
        private System.Windows.Forms.DataGridView dgv_Alarm;
        private System.Windows.Forms.DataGridViewTextBoxColumn eqpt_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarm_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarm_lvl;
        private System.Windows.Forms.DataGridViewTextBoxColumn report_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn alarm_desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn uPDTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn BATTERYCAPACITY;
        private System.Windows.Forms.DataGridViewTextBoxColumn aCCSECDIST2ShowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cYCLERUNTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn iSCYCLINGDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pARKTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn iSPARKINGDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vEHICLEACCDIST2ShowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oBSDIST2ShowDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hIDPAUSE2ShowDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn oBSPAUSE2ShowDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cMDPAUSE2ShowDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bLOCKPAUSE2ShowDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn oHTCCMDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mCSCMDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aCTSTATUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mODESTATUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vEHICLEIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button btn_creatMCSCommandManual;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Components.MyUserControl.uc_StatusTreeViewer uc_StatusTreeViewer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMDIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cARRIERIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn VEHICLE_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn tRANSFERSTATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hOSTSOURCEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hOSTDESTINATIONDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pRIORITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMDINSERTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMDSTARTTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEPLACEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vEHICLEIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mODESTATUSDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn aCTSTATUSDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mCSCMDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn oHTCCMDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bLOCKPAUSE2ShowDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cMDPAUSE2ShowDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn oBSPAUSE2ShowDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn hIDPAUSE2ShowDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn vEHICLEACCDIST2ShowDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn iSPARKINGDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pARKTIMEDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn iSCYCLINGDataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn cYCLERUNTIMEDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn aCCSECDIST2ShowDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn bATTERYCAPACITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uPDTIMEDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoadingInterlockErrorRetryTimes;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnloadingInterlockErrorRetryTimes;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentVhSettingOfBattryLowLevelValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}