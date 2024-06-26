﻿namespace com.mirle.ibg3k0.bc.winform.UI
{
    partial class RoadControlBySectionForm
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
            this.btn_disable = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_enable = new System.Windows.Forms.Button();
            this.dgv_section = new System.Windows.Forms.DataGridView();
            this.sec_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_disable_flag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pre_disable_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.disable_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pel_button = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_section)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.pel_button.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_disable
            // 
            this.btn_disable.Location = new System.Drawing.Point(90, 12);
            this.btn_disable.Name = "btn_disable";
            this.btn_disable.Size = new System.Drawing.Size(75, 23);
            this.btn_disable.TabIndex = 2;
            this.btn_disable.Text = "Disable";
            this.btn_disable.UseVisualStyleBackColor = true;
            this.btn_disable.Click += new System.EventHandler(this.btn_disable_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(171, 12);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Visible = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_enable
            // 
            this.btn_enable.Location = new System.Drawing.Point(9, 12);
            this.btn_enable.Name = "btn_enable";
            this.btn_enable.Size = new System.Drawing.Size(75, 23);
            this.btn_enable.TabIndex = 2;
            this.btn_enable.Text = "Enable";
            this.btn_enable.UseVisualStyleBackColor = true;
            this.btn_enable.Click += new System.EventHandler(this.btn_enable_Click);
            // 
            // dgv_section
            // 
            this.dgv_section.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_section.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sec_num,
            this.status,
            this.pre_disable_flag,
            this.pre_disable_time,
            this.disable_time});
            this.dgv_section.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_section.Location = new System.Drawing.Point(3, 3);
            this.dgv_section.Name = "dgv_section";
            this.dgv_section.ReadOnly = true;
            this.dgv_section.RowTemplate.Height = 24;
            this.dgv_section.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_section.Size = new System.Drawing.Size(970, 643);
            this.dgv_section.TabIndex = 5;
            this.dgv_section.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_segment_CellClick);
            // 
            // sec_num
            // 
            this.sec_num.DataPropertyName = "SEC_ID";
            this.sec_num.HeaderText = "SEC NUM";
            this.sec_num.Name = "sec_num";
            this.sec_num.ReadOnly = true;
            // 
            // status
            // 
            this.status.DataPropertyName = "STATUS";
            this.status.HeaderText = "STATUS";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // pre_disable_flag
            // 
            this.pre_disable_flag.DataPropertyName = "PRE_DISABLE_FLAG";
            this.pre_disable_flag.HeaderText = "PRE DISABLE FLAG";
            this.pre_disable_flag.Name = "pre_disable_flag";
            this.pre_disable_flag.ReadOnly = true;
            this.pre_disable_flag.Width = 200;
            // 
            // pre_disable_time
            // 
            this.pre_disable_time.DataPropertyName = "PRE_DISABLE_TIME";
            this.pre_disable_time.HeaderText = "PRE DISABLE TIME";
            this.pre_disable_time.Name = "pre_disable_time";
            this.pre_disable_time.ReadOnly = true;
            this.pre_disable_time.Width = 200;
            // 
            // disable_time
            // 
            this.disable_time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.disable_time.DataPropertyName = "DISABLE_TIME";
            this.disable_time.HeaderText = "DISABLE TIME";
            this.disable_time.Name = "disable_time";
            this.disable_time.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dgv_section, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pel_button, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(976, 699);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // pel_button
            // 
            this.pel_button.Controls.Add(this.btn_enable);
            this.pel_button.Controls.Add(this.btn_cancel);
            this.pel_button.Controls.Add(this.btn_disable);
            this.pel_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pel_button.Location = new System.Drawing.Point(3, 652);
            this.pel_button.Name = "pel_button";
            this.pel_button.Size = new System.Drawing.Size(970, 44);
            this.pel_button.TabIndex = 6;
            // 
            // RoadControlBySectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 699);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RoadControlBySectionForm";
            this.Text = "RoadControlForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RoadControlForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RoadControlForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_section)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pel_button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_disable;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_enable;
        private System.Windows.Forms.DataGridView dgv_section;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pel_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn sec_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_disable_flag;
        private System.Windows.Forms.DataGridViewTextBoxColumn pre_disable_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn disable_time;
    }
}