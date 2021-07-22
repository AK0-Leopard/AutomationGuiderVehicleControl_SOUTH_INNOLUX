namespace com.mirle.ibg3k0.bc.winform.UI
{
    partial class HistoryTransferCommandForm
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
            this.skinGroupBox2 = new CCWin.SkinControl.SkinGroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_EqptIDCbx = new System.Windows.Forms.ComboBox();
            this.skinGroupBox4 = new CCWin.SkinControl.SkinGroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_EndDTCbx = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.m_StartDTCbx = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.m_AlarmCodeTbl = new System.Windows.Forms.MaskedTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.m_exportBtn = new CCWin.SkinControl.SkinButton();
            this.btnlSearch = new CCWin.SkinControl.SkinButton();
            this.dgv_TransferCommand = new System.Windows.Forms.DataGridView();
            this.cMDIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cARRIERIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tRANSFERSTATEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hOSTSOURCEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hOSTDESTINATIONDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pRIORITYDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMDINSERTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMDSTARTTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cMDFINISHTIMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rEPLACEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hCMDMCSObjToShowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.skinGroupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.skinGroupBox4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.skinGroupBox1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TransferCommand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hCMDMCSObjToShowBindingSource)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.skinGroupBox2, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.skinGroupBox4, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.skinGroupBox1, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.panel2, 3, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1596, 110);
            this.tableLayoutPanel6.TabIndex = 85;
            // 
            // skinGroupBox2
            // 
            this.skinGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.skinGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox2.BorderColor = System.Drawing.Color.Black;
            this.skinGroupBox2.Controls.Add(this.tableLayoutPanel4);
            this.skinGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.skinGroupBox2.Font = new System.Drawing.Font("Arial", 15F);
            this.skinGroupBox2.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox2.Location = new System.Drawing.Point(803, 3);
            this.skinGroupBox2.Name = "skinGroupBox2";
            this.skinGroupBox2.RectBackColor = System.Drawing.SystemColors.Control;
            this.skinGroupBox2.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox2.Size = new System.Drawing.Size(393, 104);
            this.skinGroupBox2.TabIndex = 75;
            this.skinGroupBox2.TabStop = false;
            this.skinGroupBox2.Text = "Equipment ID ";
            this.skinGroupBox2.TitleBorderColor = System.Drawing.Color.Black;
            this.skinGroupBox2.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.skinGroupBox2.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox2.Visible = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.m_EqptIDCbx, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 26);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(387, 75);
            this.tableLayoutPanel4.TabIndex = 84;
            // 
            // m_EqptIDCbx
            // 
            this.m_EqptIDCbx.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.m_EqptIDCbx.Font = new System.Drawing.Font("Arial", 14F);
            this.m_EqptIDCbx.FormattingEnabled = true;
            this.m_EqptIDCbx.Location = new System.Drawing.Point(80, 22);
            this.m_EqptIDCbx.Name = "m_EqptIDCbx";
            this.m_EqptIDCbx.Size = new System.Drawing.Size(227, 30);
            this.m_EqptIDCbx.TabIndex = 53;
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
            this.skinGroupBox4.Text = "    Set Cmd Insert Period";
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
            this.label3.Size = new System.Drawing.Size(96, 22);
            this.label3.TabIndex = 57;
            this.label3.Text = "Start Time";
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
            this.skinGroupBox1.Size = new System.Drawing.Size(393, 104);
            this.skinGroupBox1.TabIndex = 76;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "Alarm Code ";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.Black;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Visible = false;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.m_AlarmCodeTbl, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(387, 76);
            this.tableLayoutPanel5.TabIndex = 84;
            // 
            // m_AlarmCodeTbl
            // 
            this.m_AlarmCodeTbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.m_AlarmCodeTbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_AlarmCodeTbl.Font = new System.Drawing.Font("Arial", 14F);
            this.m_AlarmCodeTbl.Location = new System.Drawing.Point(80, 23);
            this.m_AlarmCodeTbl.Name = "m_AlarmCodeTbl";
            this.m_AlarmCodeTbl.PromptChar = ' ';
            this.m_AlarmCodeTbl.Size = new System.Drawing.Size(227, 29);
            this.m_AlarmCodeTbl.TabIndex = 64;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.m_exportBtn);
            this.panel2.Controls.Add(this.btnlSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1203, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(390, 104);
            this.panel2.TabIndex = 78;
            // 
            // m_exportBtn
            // 
            this.m_exportBtn.BackColor = System.Drawing.Color.Transparent;
            this.m_exportBtn.BaseColor = System.Drawing.Color.LightGray;
            this.m_exportBtn.BorderColor = System.Drawing.Color.Black;
            this.m_exportBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.m_exportBtn.DownBack = null;
            this.m_exportBtn.DownBaseColor = System.Drawing.Color.RoyalBlue;
            this.m_exportBtn.Font = new System.Drawing.Font("Arial", 14.25F);
            this.m_exportBtn.Image = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.export;
            this.m_exportBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.m_exportBtn.ImageSize = new System.Drawing.Size(24, 24);
            this.m_exportBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_exportBtn.Location = new System.Drawing.Point(3, 42);
            this.m_exportBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.m_exportBtn.MouseBack = null;
            this.m_exportBtn.Name = "m_exportBtn";
            this.m_exportBtn.NormlBack = null;
            this.m_exportBtn.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.m_exportBtn.Size = new System.Drawing.Size(127, 33);
            this.m_exportBtn.TabIndex = 85;
            this.m_exportBtn.Text = "   Export";
            this.m_exportBtn.UseVisualStyleBackColor = false;
            this.m_exportBtn.Click += new System.EventHandler(this.m_exportBtn_Click);
            // 
            // btnlSearch
            // 
            this.btnlSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnlSearch.BaseColor = System.Drawing.Color.LightGray;
            this.btnlSearch.BorderColor = System.Drawing.Color.Black;
            this.btnlSearch.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnlSearch.DownBack = null;
            this.btnlSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnlSearch.Font = new System.Drawing.Font("Arial", 14F);
            this.btnlSearch.ForeColor = System.Drawing.Color.Black;
            this.btnlSearch.Image = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.se;
            this.btnlSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnlSearch.ImageSize = new System.Drawing.Size(24, 24);
            this.btnlSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnlSearch.Location = new System.Drawing.Point(3, 3);
            this.btnlSearch.MouseBack = null;
            this.btnlSearch.Name = "btnlSearch";
            this.btnlSearch.NormlBack = null;
            this.btnlSearch.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnlSearch.Size = new System.Drawing.Size(127, 32);
            this.btnlSearch.TabIndex = 83;
            this.btnlSearch.Text = "   Search";
            this.btnlSearch.UseVisualStyleBackColor = false;
            this.btnlSearch.Click += new System.EventHandler(this.btnlSearch_Click);
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
            this.tRANSFERSTATEDataGridViewTextBoxColumn,
            this.hOSTSOURCEDataGridViewTextBoxColumn,
            this.hOSTDESTINATIONDataGridViewTextBoxColumn,
            this.pRIORITYDataGridViewTextBoxColumn,
            this.cMDINSERTIMEDataGridViewTextBoxColumn,
            this.cMDSTARTTIMEDataGridViewTextBoxColumn,
            this.cMDFINISHTIMEDataGridViewTextBoxColumn,
            this.rEPLACEDataGridViewTextBoxColumn});
            this.dgv_TransferCommand.DataSource = this.hCMDMCSObjToShowBindingSource;
            this.dgv_TransferCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_TransferCommand.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgv_TransferCommand.Location = new System.Drawing.Point(3, 119);
            this.dgv_TransferCommand.MultiSelect = false;
            this.dgv_TransferCommand.Name = "dgv_TransferCommand";
            this.dgv_TransferCommand.ReadOnly = true;
            this.dgv_TransferCommand.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgv_TransferCommand.RowTemplate.Height = 24;
            this.dgv_TransferCommand.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_TransferCommand.Size = new System.Drawing.Size(1596, 594);
            this.dgv_TransferCommand.TabIndex = 9;
            // 
            // cMDIDDataGridViewTextBoxColumn
            // 
            this.cMDIDDataGridViewTextBoxColumn.DataPropertyName = "CMD_ID";
            this.cMDIDDataGridViewTextBoxColumn.HeaderText = "CMD_ID";
            this.cMDIDDataGridViewTextBoxColumn.Name = "cMDIDDataGridViewTextBoxColumn";
            this.cMDIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cARRIERIDDataGridViewTextBoxColumn
            // 
            this.cARRIERIDDataGridViewTextBoxColumn.DataPropertyName = "CARRIER_ID";
            this.cARRIERIDDataGridViewTextBoxColumn.HeaderText = "CARRIER_ID";
            this.cARRIERIDDataGridViewTextBoxColumn.Name = "cARRIERIDDataGridViewTextBoxColumn";
            this.cARRIERIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tRANSFERSTATEDataGridViewTextBoxColumn
            // 
            this.tRANSFERSTATEDataGridViewTextBoxColumn.DataPropertyName = "TRANSFERSTATE";
            this.tRANSFERSTATEDataGridViewTextBoxColumn.HeaderText = "TRANSFERSTATE";
            this.tRANSFERSTATEDataGridViewTextBoxColumn.Name = "tRANSFERSTATEDataGridViewTextBoxColumn";
            this.tRANSFERSTATEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hOSTSOURCEDataGridViewTextBoxColumn
            // 
            this.hOSTSOURCEDataGridViewTextBoxColumn.DataPropertyName = "HOSTSOURCE";
            this.hOSTSOURCEDataGridViewTextBoxColumn.HeaderText = "HOSTSOURCE";
            this.hOSTSOURCEDataGridViewTextBoxColumn.Name = "hOSTSOURCEDataGridViewTextBoxColumn";
            this.hOSTSOURCEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hOSTDESTINATIONDataGridViewTextBoxColumn
            // 
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.DataPropertyName = "HOSTDESTINATION";
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.HeaderText = "HOSTDESTINATION";
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.Name = "hOSTDESTINATIONDataGridViewTextBoxColumn";
            this.hOSTDESTINATIONDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // pRIORITYDataGridViewTextBoxColumn
            // 
            this.pRIORITYDataGridViewTextBoxColumn.DataPropertyName = "PRIORITY";
            this.pRIORITYDataGridViewTextBoxColumn.HeaderText = "PRIORITY";
            this.pRIORITYDataGridViewTextBoxColumn.Name = "pRIORITYDataGridViewTextBoxColumn";
            this.pRIORITYDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cMDINSERTIMEDataGridViewTextBoxColumn
            // 
            this.cMDINSERTIMEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cMDINSERTIMEDataGridViewTextBoxColumn.DataPropertyName = "CMD_INSER_TIME";
            this.cMDINSERTIMEDataGridViewTextBoxColumn.HeaderText = "CMD_INSER_TIME";
            this.cMDINSERTIMEDataGridViewTextBoxColumn.Name = "cMDINSERTIMEDataGridViewTextBoxColumn";
            this.cMDINSERTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.cMDINSERTIMEDataGridViewTextBoxColumn.Width = 175;
            // 
            // cMDSTARTTIMEDataGridViewTextBoxColumn
            // 
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.DataPropertyName = "CMD_START_TIME";
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.HeaderText = "CMD_START_TIME";
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.Name = "cMDSTARTTIMEDataGridViewTextBoxColumn";
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.cMDSTARTTIMEDataGridViewTextBoxColumn.Width = 175;
            // 
            // cMDFINISHTIMEDataGridViewTextBoxColumn
            // 
            this.cMDFINISHTIMEDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cMDFINISHTIMEDataGridViewTextBoxColumn.DataPropertyName = "CMD_FINISH_TIME";
            this.cMDFINISHTIMEDataGridViewTextBoxColumn.HeaderText = "CMD_FINISH_TIME";
            this.cMDFINISHTIMEDataGridViewTextBoxColumn.Name = "cMDFINISHTIMEDataGridViewTextBoxColumn";
            this.cMDFINISHTIMEDataGridViewTextBoxColumn.ReadOnly = true;
            this.cMDFINISHTIMEDataGridViewTextBoxColumn.Width = 185;
            // 
            // rEPLACEDataGridViewTextBoxColumn
            // 
            this.rEPLACEDataGridViewTextBoxColumn.DataPropertyName = "REPLACE";
            this.rEPLACEDataGridViewTextBoxColumn.HeaderText = "REPLACE";
            this.rEPLACEDataGridViewTextBoxColumn.Name = "rEPLACEDataGridViewTextBoxColumn";
            this.rEPLACEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // hCMDMCSObjToShowBindingSource
            // 
            this.hCMDMCSObjToShowBindingSource.DataSource = typeof(com.mirle.ibg3k0.sc.ObjectRelay.HCMD_MCSObjToShow);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_TransferCommand, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.80447F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.97765F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1602, 716);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // HistoryTransferCommandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1602, 716);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "HistoryTransferCommandForm";
            this.Text = "History Transfer Command";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TransferCommandQureyListForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.skinGroupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.skinGroupBox4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.skinGroupBox1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_TransferCommand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hCMDMCSObjToShowBindingSource)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox m_EqptIDCbx;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DateTimePicker m_EndDTCbx;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker m_StartDTCbx;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinButton btnlSearch;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.MaskedTextBox m_AlarmCodeTbl;
        private System.Windows.Forms.DataGridView dgv_TransferCommand;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private CCWin.SkinControl.SkinButton m_exportBtn;
        private System.Windows.Forms.BindingSource hCMDMCSObjToShowBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMDIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cARRIERIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tRANSFERSTATEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hOSTSOURCEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hOSTDESTINATIONDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pRIORITYDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMDINSERTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMDSTARTTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cMDFINISHTIMEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rEPLACEDataGridViewTextBoxColumn;
    }
}