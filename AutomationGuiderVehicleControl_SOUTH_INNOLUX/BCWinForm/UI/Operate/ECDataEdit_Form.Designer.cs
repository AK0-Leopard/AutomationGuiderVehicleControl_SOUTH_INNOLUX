namespace com.mirle.ibg3k0.bc.winform.UI
{
    partial class ECDataEdit_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ECDataEdit_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_detailGbx = new CCWin.SkinControl.SkinGroupBox();
            this.m_detailLTP = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_ecidLbl = new System.Windows.Forms.Label();
            this.m_ecNameTxb = new System.Windows.Forms.TextBox();
            this.m_ecidTxb = new System.Windows.Forms.TextBox();
            this.m_ecNameLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.m_ecValueLbl = new System.Windows.Forms.Label();
            this.m_maxTxb = new System.Windows.Forms.TextBox();
            this.m_ecValueTxb = new System.Windows.Forms.TextBox();
            this.m_minTxb = new System.Windows.Forms.TextBox();
            this.m_maxLbl = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new CCWin.SkinControl.SkinButton();
            this.m_modifyBtn = new CCWin.SkinControl.SkinButton();
            this.groupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.m_ecDataDGV = new System.Windows.Forms.DataGridView();
            this.cLLEcID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLLEcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLLEcMin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLLEcMax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLLEcV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_ecValueCmb = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.m_detailGbx.SuspendLayout();
            this.m_detailLTP.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_ecDataDGV)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_detailGbx
            // 
            resources.ApplyResources(this.m_detailGbx, "m_detailGbx");
            this.m_detailGbx.BackColor = System.Drawing.Color.Transparent;
            this.m_detailGbx.BorderColor = System.Drawing.Color.Black;
            this.m_detailGbx.Controls.Add(this.m_detailLTP);
            this.m_detailGbx.ForeColor = System.Drawing.Color.Black;
            this.m_detailGbx.Name = "m_detailGbx";
            this.m_detailGbx.Radius = 20;
            this.m_detailGbx.RectBackColor = System.Drawing.SystemColors.Control;
            this.m_detailGbx.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.m_detailGbx.TabStop = false;
            this.m_detailGbx.TitleBorderColor = System.Drawing.Color.Black;
            this.m_detailGbx.TitleRadius = 10;
            this.m_detailGbx.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.m_detailGbx.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.m_detailGbx.Enter += new System.EventHandler(this.m_detailGbx_Enter);
            // 
            // m_detailLTP
            // 
            resources.ApplyResources(this.m_detailLTP, "m_detailLTP");
            this.m_detailLTP.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.m_detailLTP.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.m_detailLTP.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.m_detailLTP.Name = "m_detailLTP";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.m_ecidLbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_ecNameTxb, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_ecidTxb, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_ecNameLbl, 2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // m_ecidLbl
            // 
            resources.ApplyResources(this.m_ecidLbl, "m_ecidLbl");
            this.m_ecidLbl.Name = "m_ecidLbl";
            // 
            // m_ecNameTxb
            // 
            resources.ApplyResources(this.m_ecNameTxb, "m_ecNameTxb");
            this.m_ecNameTxb.Name = "m_ecNameTxb";
            this.m_ecNameTxb.ReadOnly = true;
            // 
            // m_ecidTxb
            // 
            resources.ApplyResources(this.m_ecidTxb, "m_ecidTxb");
            this.m_ecidTxb.Name = "m_ecidTxb";
            this.m_ecidTxb.ReadOnly = true;
            this.m_ecidTxb.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m_ecidTxb_KeyPress);
            // 
            // m_ecNameLbl
            // 
            resources.ApplyResources(this.m_ecNameLbl, "m_ecNameLbl");
            this.m_ecNameLbl.Name = "m_ecNameLbl";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.m_ecValueLbl, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_maxTxb, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_ecValueTxb, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_minTxb, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.m_maxLbl, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // m_ecValueLbl
            // 
            resources.ApplyResources(this.m_ecValueLbl, "m_ecValueLbl");
            this.m_ecValueLbl.Name = "m_ecValueLbl";
            // 
            // m_maxTxb
            // 
            resources.ApplyResources(this.m_maxTxb, "m_maxTxb");
            this.m_maxTxb.Name = "m_maxTxb";
            this.m_maxTxb.ReadOnly = true;
            // 
            // m_ecValueTxb
            // 
            resources.ApplyResources(this.m_ecValueTxb, "m_ecValueTxb");
            this.m_ecValueTxb.Name = "m_ecValueTxb";
            // 
            // m_minTxb
            // 
            resources.ApplyResources(this.m_minTxb, "m_minTxb");
            this.m_minTxb.Name = "m_minTxb";
            this.m_minTxb.ReadOnly = true;
            // 
            // m_maxLbl
            // 
            resources.ApplyResources(this.m_maxLbl, "m_maxLbl");
            this.m_maxLbl.Name = "m_maxLbl";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_modifyBtn, 2, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BaseColor = System.Drawing.Color.LightGray;
            this.btnClose.BorderColor = System.Drawing.Color.Black;
            this.btnClose.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnClose.DownBack = null;
            this.btnClose.DownBaseColor = System.Drawing.Color.RoyalBlue;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Image = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.Exit;
            this.btnClose.MouseBack = null;
            this.btnClose.Name = "btnClose";
            this.btnClose.NormlBack = null;
            this.btnClose.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // m_modifyBtn
            // 
            this.m_modifyBtn.BackColor = System.Drawing.Color.Transparent;
            this.m_modifyBtn.BaseColor = System.Drawing.Color.LightGray;
            this.m_modifyBtn.BorderColor = System.Drawing.Color.Black;
            this.m_modifyBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.m_modifyBtn.DownBack = null;
            resources.ApplyResources(this.m_modifyBtn, "m_modifyBtn");
            this.m_modifyBtn.Image = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.Save;
            this.m_modifyBtn.ImageSize = new System.Drawing.Size(24, 24);
            this.m_modifyBtn.MouseBack = null;
            this.m_modifyBtn.Name = "m_modifyBtn";
            this.m_modifyBtn.NormlBack = null;
            this.m_modifyBtn.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.m_modifyBtn.UseVisualStyleBackColor = true;
            this.m_modifyBtn.Click += new System.EventHandler(this.m_modifyBtn_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.BorderColor = System.Drawing.Color.Black;
            this.groupBox1.Controls.Add(this.m_ecDataDGV);
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Radius = 20;
            this.groupBox1.RectBackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.groupBox1.TabStop = false;
            this.groupBox1.TitleBorderColor = System.Drawing.Color.Black;
            this.groupBox1.TitleRadius = 10;
            this.groupBox1.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.groupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // m_ecDataDGV
            // 
            this.m_ecDataDGV.AllowUserToAddRows = false;
            this.m_ecDataDGV.AllowUserToDeleteRows = false;
            this.m_ecDataDGV.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PaleTurquoise;
            this.m_ecDataDGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.m_ecDataDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.m_ecDataDGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.m_ecDataDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.m_ecDataDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_ecDataDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cLLEcID,
            this.cLLEcName,
            this.cLLEcMin,
            this.cLLEcMax,
            this.cLLEcV});
            resources.ApplyResources(this.m_ecDataDGV, "m_ecDataDGV");
            this.m_ecDataDGV.Name = "m_ecDataDGV";
            this.m_ecDataDGV.ReadOnly = true;
            this.m_ecDataDGV.RowHeadersVisible = false;
            this.m_ecDataDGV.RowTemplate.Height = 24;
            this.m_ecDataDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_ecDataDGV.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_ecDataDGV_CellDoubleClick);
            // 
            // cLLEcID
            // 
            this.cLLEcID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.cLLEcID, "cLLEcID");
            this.cLLEcID.Name = "cLLEcID";
            this.cLLEcID.ReadOnly = true;
            // 
            // cLLEcName
            // 
            this.cLLEcName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.cLLEcName, "cLLEcName");
            this.cLLEcName.Name = "cLLEcName";
            this.cLLEcName.ReadOnly = true;
            // 
            // cLLEcMin
            // 
            this.cLLEcMin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.cLLEcMin, "cLLEcMin");
            this.cLLEcMin.Name = "cLLEcMin";
            this.cLLEcMin.ReadOnly = true;
            // 
            // cLLEcMax
            // 
            this.cLLEcMax.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.cLLEcMax, "cLLEcMax");
            this.cLLEcMax.Name = "cLLEcMax";
            this.cLLEcMax.ReadOnly = true;
            // 
            // cLLEcV
            // 
            this.cLLEcV.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.cLLEcV, "cLLEcV");
            this.cLLEcV.Name = "cLLEcV";
            this.cLLEcV.ReadOnly = true;
            // 
            // m_ecValueCmb
            // 
            resources.ApplyResources(this.m_ecValueCmb, "m_ecValueCmb");
            this.m_ecValueCmb.Name = "m_ecValueCmb";
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.m_detailGbx, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // skinLabel1
            // 
            this.skinLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.skinLabel1.BorderColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.skinLabel1, "skinLabel1");
            this.skinLabel1.ForeColor = System.Drawing.Color.White;
            this.skinLabel1.Name = "skinLabel1";
            // 
            // ECDataEdit_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.BackgroundImage = global::com.mirle.ibg3k0.bc.winform.Properties.Resources.main;
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.skinLabel1);
            this.DoubleBuffered = true;
            this.MinimizeBox = false;
            this.Name = "ECDataEdit_Form";
            this.ShowIcon = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ECDataEdit_Form_FormClosed);
            this.Load += new System.EventHandler(this.ECDataEdit_Form_Load);
            this.m_detailGbx.ResumeLayout(false);
            this.m_detailLTP.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_ecDataDGV)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinGroupBox groupBox1;
        private System.Windows.Forms.DataGridView m_ecDataDGV;
        private CCWin.SkinControl.SkinGroupBox m_detailGbx;
        private System.Windows.Forms.TableLayoutPanel m_detailLTP;
        private System.Windows.Forms.Label m_ecValueLbl;
        private System.Windows.Forms.TextBox m_ecNameTxb;
        private System.Windows.Forms.Label m_ecNameLbl;
        private System.Windows.Forms.Label m_ecidLbl;
        private System.Windows.Forms.TextBox m_ecidTxb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox m_ecValueTxb;
        private CCWin.SkinControl.SkinButton m_modifyBtn;
        private System.Windows.Forms.TextBox m_minTxb;
        private System.Windows.Forms.TextBox m_maxTxb;
        private System.Windows.Forms.Label m_maxLbl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox m_ecValueCmb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private CCWin.SkinControl.SkinButton btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLLEcID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLLEcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLLEcMin;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLLEcMax;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLLEcV;
    }
}