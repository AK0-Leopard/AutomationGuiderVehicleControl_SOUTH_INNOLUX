namespace com.mirle.ibg3k0.bc.winform.UI
{
    partial class CurrentEqLogInfoForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.skinGroupBox4 = new CCWin.SkinControl.SkinGroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_EndDTCbx = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.m_StartDTCbx = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_commandID = new System.Windows.Forms.MaskedTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnlSearch = new CCWin.SkinControl.SkinButton();
            this.skinGroupBox2 = new CCWin.SkinControl.SkinGroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_mcsCmdID = new System.Windows.Forms.MaskedTextBox();
            this.dgv_eqLogInfos = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txt_message = new System.Windows.Forms.TextBox();
            this.aLARMObjToShowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VH_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VH_STATUS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FUN_NAME = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SENDRECEIVE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EVENT_TYPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SEQ_NUM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OHTC_CMD_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MCS_CMD_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.skinGroupBox4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.skinGroupBox1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel2.SuspendLayout();
            this.skinGroupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_eqLogInfos)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aLARMObjToShowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1596, 110);
            this.panel1.TabIndex = 11;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 5;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 254F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 257F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 309F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.skinGroupBox4, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.skinGroupBox1, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.panel2, 4, 0);
            this.tableLayoutPanel6.Controls.Add(this.skinGroupBox2, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1596, 110);
            this.tableLayoutPanel6.TabIndex = 85;
            // 
            // skinGroupBox4
            // 
            this.skinGroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.skinGroupBox4.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox4.BorderColor = System.Drawing.Color.Black;
            this.skinGroupBox4.Controls.Add(this.tableLayoutPanel3);
            this.skinGroupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.skinGroupBox4.Font = new System.Drawing.Font("Arial", 14F);
            this.skinGroupBox4.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox4.Location = new System.Drawing.Point(3, 3);
            this.skinGroupBox4.Name = "skinGroupBox4";
            this.skinGroupBox4.RectBackColor = System.Drawing.SystemColors.Control;
            this.skinGroupBox4.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox4.Size = new System.Drawing.Size(393, 104);
            this.skinGroupBox4.TabIndex = 77;
            this.skinGroupBox4.TabStop = false;
            this.skinGroupBox4.Text = "Time";
            this.skinGroupBox4.TitleBorderColor = System.Drawing.Color.Black;
            this.skinGroupBox4.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.skinGroupBox4.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.12903F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.87097F));
            this.tableLayoutPanel3.Controls.Add(this.m_EndDTCbx, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_StartDTCbx, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(387, 76);
            this.tableLayoutPanel3.TabIndex = 79;
            // 
            // m_EndDTCbx
            // 
            this.m_EndDTCbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.m_EndDTCbx.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.m_EndDTCbx.Font = new System.Drawing.Font("Arial", 14F);
            this.m_EndDTCbx.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_EndDTCbx.Location = new System.Drawing.Point(142, 42);
            this.m_EndDTCbx.Name = "m_EndDTCbx";
            this.m_EndDTCbx.Size = new System.Drawing.Size(242, 29);
            this.m_EndDTCbx.TabIndex = 66;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 14F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(3, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 22);
            this.label3.TabIndex = 57;
            this.label3.Text = "From Time";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_StartDTCbx
            // 
            this.m_StartDTCbx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.m_StartDTCbx.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.m_StartDTCbx.Font = new System.Drawing.Font("Arial", 14F);
            this.m_StartDTCbx.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_StartDTCbx.Location = new System.Drawing.Point(142, 4);
            this.m_StartDTCbx.Name = "m_StartDTCbx";
            this.m_StartDTCbx.Size = new System.Drawing.Size(242, 29);
            this.m_StartDTCbx.TabIndex = 65;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 14F);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(3, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 22);
            this.label1.TabIndex = 61;
            this.label1.Text = "End Time ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Controls.Add(this.tableLayoutPanel5);
            this.skinGroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.skinGroupBox1.Font = new System.Drawing.Font("Arial", 14F);
            this.skinGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Location = new System.Drawing.Point(403, 3);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.SystemColors.Control;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(248, 104);
            this.skinGroupBox1.TabIndex = 76;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "Command ID";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.Black;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.txt_commandID, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(242, 76);
            this.tableLayoutPanel5.TabIndex = 84;
            // 
            // txt_commandID
            // 
            this.txt_commandID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_commandID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_commandID.Font = new System.Drawing.Font("Arial", 14F);
            this.txt_commandID.Location = new System.Drawing.Point(7, 23);
            this.txt_commandID.Name = "txt_commandID";
            this.txt_commandID.PromptChar = ' ';
            this.txt_commandID.Size = new System.Drawing.Size(227, 29);
            this.txt_commandID.TabIndex = 64;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnlSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1223, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(370, 104);
            this.panel2.TabIndex = 78;
            // 
            // btnlSearch
            // 
            this.btnlSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnlSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnlSearch.BaseColor = System.Drawing.Color.LightGray;
            this.btnlSearch.BorderColor = System.Drawing.Color.Black;
            this.btnlSearch.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnlSearch.DownBack = null;
            this.btnlSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnlSearch.Font = new System.Drawing.Font("Arial", 14F);
            this.btnlSearch.ForeColor = System.Drawing.Color.Black;
            this.btnlSearch.Image = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.se;
            this.btnlSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnlSearch.ImageSize = new System.Drawing.Size(24, 24);
            this.btnlSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnlSearch.Location = new System.Drawing.Point(237, 61);
            this.btnlSearch.MouseBack = null;
            this.btnlSearch.Name = "btnlSearch";
            this.btnlSearch.NormlBack = null;
            this.btnlSearch.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnlSearch.Size = new System.Drawing.Size(127, 32);
            this.btnlSearch.TabIndex = 83;
            this.btnlSearch.Text = "Search";
            this.btnlSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnlSearch.UseVisualStyleBackColor = false;
            this.btnlSearch.Click += new System.EventHandler(this.btnlSearch_Click);
            // 
            // skinGroupBox2
            // 
            this.skinGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox2.BorderColor = System.Drawing.Color.Black;
            this.skinGroupBox2.Controls.Add(this.tableLayoutPanel4);
            this.skinGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.skinGroupBox2.Font = new System.Drawing.Font("Arial", 14F);
            this.skinGroupBox2.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox2.Location = new System.Drawing.Point(657, 3);
            this.skinGroupBox2.Name = "skinGroupBox2";
            this.skinGroupBox2.RectBackColor = System.Drawing.SystemColors.Control;
            this.skinGroupBox2.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox2.Size = new System.Drawing.Size(251, 104);
            this.skinGroupBox2.TabIndex = 79;
            this.skinGroupBox2.TabStop = false;
            this.skinGroupBox2.Text = "MCS Cmd ID";
            this.skinGroupBox2.TitleBorderColor = System.Drawing.Color.Black;
            this.skinGroupBox2.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.skinGroupBox2.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.txt_mcsCmdID, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(245, 76);
            this.tableLayoutPanel4.TabIndex = 84;
            // 
            // txt_mcsCmdID
            // 
            this.txt_mcsCmdID.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_mcsCmdID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txt_mcsCmdID.Font = new System.Drawing.Font("Arial", 14F);
            this.txt_mcsCmdID.Location = new System.Drawing.Point(9, 23);
            this.txt_mcsCmdID.Name = "txt_mcsCmdID";
            this.txt_mcsCmdID.PromptChar = ' ';
            this.txt_mcsCmdID.Size = new System.Drawing.Size(227, 29);
            this.txt_mcsCmdID.TabIndex = 64;
            // 
            // dgv_eqLogInfos
            // 
            this.dgv_eqLogInfos.AllowUserToAddRows = false;
            this.dgv_eqLogInfos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgv_eqLogInfos.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_eqLogInfos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_eqLogInfos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.VH_ID,
            this.VH_STATUS,
            this.FUN_NAME,
            this.SENDRECEIVE,
            this.EVENT_TYPE,
            this.SEQ_NUM,
            this.OHTC_CMD_ID,
            this.MCS_CMD_ID});
            this.dgv_eqLogInfos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_eqLogInfos.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgv_eqLogInfos.Location = new System.Drawing.Point(3, 3);
            this.dgv_eqLogInfos.MultiSelect = false;
            this.dgv_eqLogInfos.Name = "dgv_eqLogInfos";
            this.dgv_eqLogInfos.ReadOnly = true;
            this.dgv_eqLogInfos.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_eqLogInfos.RowTemplate.Height = 24;
            this.dgv_eqLogInfos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_eqLogInfos.Size = new System.Drawing.Size(1098, 588);
            this.dgv_eqLogInfos.TabIndex = 9;
            this.dgv_eqLogInfos.SelectionChanged += new System.EventHandler(this.dgv_eqLogInfos_SelectionChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.80447F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.97765F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1602, 716);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.23559F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.76441F));
            this.tableLayoutPanel2.Controls.Add(this.dgv_eqLogInfos, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txt_message, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 119);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1596, 594);
            this.tableLayoutPanel2.TabIndex = 79;
            // 
            // txt_message
            // 
            this.txt_message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_message.Location = new System.Drawing.Point(1107, 3);
            this.txt_message.Multiline = true;
            this.txt_message.Name = "txt_message";
            this.txt_message.ReadOnly = true;
            this.txt_message.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_message.Size = new System.Drawing.Size(486, 588);
            this.txt_message.TabIndex = 10;
            // 
            // aLARMObjToShowBindingSource
            // 
            this.aLARMObjToShowBindingSource.DataSource = typeof(com.mirle.ibg3k0.sc.ObjectRelay.ALARMObjToShow);
            // 
            // Time
            // 
            this.Time.DataPropertyName = "sTIME";
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Width = 75;
            // 
            // VH_ID
            // 
            this.VH_ID.DataPropertyName = "VHID";
            this.VH_ID.HeaderText = "Vh ID";
            this.VH_ID.Name = "VH_ID";
            this.VH_ID.ReadOnly = true;
            this.VH_ID.Width = 85;
            // 
            // VH_STATUS
            // 
            this.VH_STATUS.DataPropertyName = "VHSTATUS";
            this.VH_STATUS.HeaderText = "Vh Status";
            this.VH_STATUS.Name = "VH_STATUS";
            this.VH_STATUS.ReadOnly = true;
            this.VH_STATUS.Width = 125;
            // 
            // FUN_NAME
            // 
            this.FUN_NAME.DataPropertyName = "FUNNAME";
            this.FUN_NAME.HeaderText = "Fun. Name";
            this.FUN_NAME.Name = "FUN_NAME";
            this.FUN_NAME.ReadOnly = true;
            this.FUN_NAME.Width = 125;
            // 
            // SENDRECEIVE
            // 
            this.SENDRECEIVE.DataPropertyName = "SENDRECEIVE";
            this.SENDRECEIVE.HeaderText = "";
            this.SENDRECEIVE.Name = "SENDRECEIVE";
            this.SENDRECEIVE.ReadOnly = true;
            this.SENDRECEIVE.Width = 19;
            // 
            // EVENT_TYPE
            // 
            this.EVENT_TYPE.DataPropertyName = "EVENTTYPE";
            this.EVENT_TYPE.HeaderText = "Event Type";
            this.EVENT_TYPE.Name = "EVENT_TYPE";
            this.EVENT_TYPE.ReadOnly = true;
            this.EVENT_TYPE.Width = 135;
            // 
            // SEQ_NUM
            // 
            this.SEQ_NUM.DataPropertyName = "SEQNO";
            this.SEQ_NUM.HeaderText = "Seq.";
            this.SEQ_NUM.Name = "SEQ_NUM";
            this.SEQ_NUM.ReadOnly = true;
            this.SEQ_NUM.Width = 75;
            // 
            // OHTC_CMD_ID
            // 
            this.OHTC_CMD_ID.DataPropertyName = "OHTCCMDID";
            this.OHTC_CMD_ID.HeaderText = "Command ID";
            this.OHTC_CMD_ID.Name = "OHTC_CMD_ID";
            this.OHTC_CMD_ID.ReadOnly = true;
            this.OHTC_CMD_ID.Width = 135;
            // 
            // MCS_CMD_ID
            // 
            this.MCS_CMD_ID.DataPropertyName = "MCSCMDID";
            this.MCS_CMD_ID.HeaderText = "MCS Cmd. ID";
            this.MCS_CMD_ID.Name = "MCS_CMD_ID";
            this.MCS_CMD_ID.ReadOnly = true;
            this.MCS_CMD_ID.Width = 145;
            // 
            // CurrentEqLogInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1602, 716);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "CurrentEqLogInfoForm";
            this.Text = "EQ Log Info";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TransferCommandQureyListForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.skinGroupBox4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.skinGroupBox1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.skinGroupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_eqLogInfos)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aLARMObjToShowBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource aLARMObjToShowBindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DateTimePicker m_EndDTCbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker m_StartDTCbx;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinButton btnlSearch;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.MaskedTextBox txt_commandID;
        private System.Windows.Forms.DataGridView dgv_eqLogInfos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txt_message;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.MaskedTextBox txt_mcsCmdID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn VH_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn VH_STATUS;
        private System.Windows.Forms.DataGridViewTextBoxColumn FUN_NAME;
        private System.Windows.Forms.DataGridViewTextBoxColumn SENDRECEIVE;
        private System.Windows.Forms.DataGridViewTextBoxColumn EVENT_TYPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn SEQ_NUM;
        private System.Windows.Forms.DataGridViewTextBoxColumn OHTC_CMD_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn MCS_CMD_ID;
    }
}