namespace com.mirle.ibg3k0.bc.winform.UI
{
    partial class AlarmListForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.m_AlarmCodeTbl = new System.Windows.Forms.MaskedTextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.eQPTREALIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aLARMIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aLARMLVLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aLARMDESCDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pOSSIBLECAUSESDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sUGGESTIONDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.alarmMapBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.aLARMObjToShowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.skinGroupBox1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alarmMapBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aLARMObjToShowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1596, 73);
            this.panel1.TabIndex = 11;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.panel2, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.skinGroupBox1, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(1596, 73);
            this.tableLayoutPanel6.TabIndex = 85;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1203, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(390, 67);
            this.panel2.TabIndex = 78;
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
            this.skinGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.SystemColors.Control;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(393, 67);
            this.skinGroupBox1.TabIndex = 76;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "Alarm Code ";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.Color.Black;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.LightSteelBlue;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
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
            this.tableLayoutPanel5.Size = new System.Drawing.Size(387, 39);
            this.tableLayoutPanel5.TabIndex = 84;
            // 
            // m_AlarmCodeTbl
            // 
            this.m_AlarmCodeTbl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.m_AlarmCodeTbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_AlarmCodeTbl.Font = new System.Drawing.Font("Arial", 14F);
            this.m_AlarmCodeTbl.Location = new System.Drawing.Point(80, 5);
            this.m_AlarmCodeTbl.Name = "m_AlarmCodeTbl";
            this.m_AlarmCodeTbl.PromptChar = ' ';
            this.m_AlarmCodeTbl.Size = new System.Drawing.Size(227, 29);
            this.m_AlarmCodeTbl.TabIndex = 64;
            this.m_AlarmCodeTbl.TextChanged += new System.EventHandler(this.m_AlarmCodeTbl_TextChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.03352F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.96648F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1602, 716);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.eQPTREALIDDataGridViewTextBoxColumn,
            this.aLARMIDDataGridViewTextBoxColumn,
            this.aLARMLVLDataGridViewTextBoxColumn,
            this.aLARMDESCDataGridViewTextBoxColumn,
            this.pOSSIBLECAUSESDataGridViewTextBoxColumn,
            this.sUGGESTIONDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.alarmMapBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dataGridView1.Location = new System.Drawing.Point(3, 82);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1596, 631);
            this.dataGridView1.TabIndex = 9;
            // 
            // eQPTREALIDDataGridViewTextBoxColumn
            // 
            this.eQPTREALIDDataGridViewTextBoxColumn.DataPropertyName = "EQPT_REAL_ID";
            this.eQPTREALIDDataGridViewTextBoxColumn.HeaderText = "EQ ID";
            this.eQPTREALIDDataGridViewTextBoxColumn.Name = "eQPTREALIDDataGridViewTextBoxColumn";
            this.eQPTREALIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.eQPTREALIDDataGridViewTextBoxColumn.Width = 60;
            // 
            // aLARMIDDataGridViewTextBoxColumn
            // 
            this.aLARMIDDataGridViewTextBoxColumn.DataPropertyName = "ALARM_ID";
            this.aLARMIDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.aLARMIDDataGridViewTextBoxColumn.Name = "aLARMIDDataGridViewTextBoxColumn";
            this.aLARMIDDataGridViewTextBoxColumn.ReadOnly = true;
            this.aLARMIDDataGridViewTextBoxColumn.Width = 55;
            // 
            // aLARMLVLDataGridViewTextBoxColumn
            // 
            this.aLARMLVLDataGridViewTextBoxColumn.DataPropertyName = "ALARM_LVL";
            this.aLARMLVLDataGridViewTextBoxColumn.HeaderText = "Level";
            this.aLARMLVLDataGridViewTextBoxColumn.Name = "aLARMLVLDataGridViewTextBoxColumn";
            this.aLARMLVLDataGridViewTextBoxColumn.ReadOnly = true;
            this.aLARMLVLDataGridViewTextBoxColumn.Width = 85;
            // 
            // aLARMDESCDataGridViewTextBoxColumn
            // 
            this.aLARMDESCDataGridViewTextBoxColumn.DataPropertyName = "ALARM_DESC";
            this.aLARMDESCDataGridViewTextBoxColumn.HeaderText = "Description";
            this.aLARMDESCDataGridViewTextBoxColumn.Name = "aLARMDESCDataGridViewTextBoxColumn";
            this.aLARMDESCDataGridViewTextBoxColumn.ReadOnly = true;
            this.aLARMDESCDataGridViewTextBoxColumn.Width = 145;
            // 
            // pOSSIBLECAUSESDataGridViewTextBoxColumn
            // 
            this.pOSSIBLECAUSESDataGridViewTextBoxColumn.DataPropertyName = "POSSIBLE_CAUSES";
            this.pOSSIBLECAUSESDataGridViewTextBoxColumn.HeaderText = "Possible Causes";
            this.pOSSIBLECAUSESDataGridViewTextBoxColumn.Name = "pOSSIBLECAUSESDataGridViewTextBoxColumn";
            this.pOSSIBLECAUSESDataGridViewTextBoxColumn.ReadOnly = true;
            this.pOSSIBLECAUSESDataGridViewTextBoxColumn.Width = 168;
            // 
            // sUGGESTIONDataGridViewTextBoxColumn
            // 
            this.sUGGESTIONDataGridViewTextBoxColumn.DataPropertyName = "SUGGESTION";
            this.sUGGESTIONDataGridViewTextBoxColumn.HeaderText = "Suggestion";
            this.sUGGESTIONDataGridViewTextBoxColumn.Name = "sUGGESTIONDataGridViewTextBoxColumn";
            this.sUGGESTIONDataGridViewTextBoxColumn.ReadOnly = true;
            this.sUGGESTIONDataGridViewTextBoxColumn.Width = 135;
            // 
            // alarmMapBindingSource
            // 
            this.alarmMapBindingSource.DataSource = typeof(com.mirle.ibg3k0.sc.Data.VO.AlarmMap);
            // 
            // aLARMObjToShowBindingSource
            // 
            this.aLARMObjToShowBindingSource.DataSource = typeof(com.mirle.ibg3k0.sc.ObjectRelay.ALARMObjToShow);
            // 
            // AlarmListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1602, 716);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "AlarmListForm";
            this.Text = "Alarm List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TransferCommandQureyListForm_FormClosed);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.skinGroupBox1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alarmMapBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aLARMObjToShowBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource aLARMObjToShowBindingSource;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.MaskedTextBox m_AlarmCodeTbl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.BindingSource alarmMapBindingSource;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn eQPTREALIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aLARMIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aLARMLVLDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aLARMDESCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pOSSIBLECAUSESDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sUGGESTIONDataGridViewTextBoxColumn;
    }
}