namespace com.mirle.ibg3k0.bc.winform.UI
{
    partial class VehicleOperationForm
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
            this.tlp_VehicleOperationBlock = new System.Windows.Forms.TableLayoutPanel();
            this.grpb_VehicleInstallRemove = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_changeToRemove = new com.mirle.ibg3k0.bc.winform.UI.Components.uctlButton();
            this.btn_changeToInstall = new com.mirle.ibg3k0.bc.winform.UI.Components.uctlButton();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_isinstalled_value = new System.Windows.Forms.Label();
            this.grpb_vehicleMode = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_auto_remote = new System.Windows.Forms.Button();
            this.btn_auto_local = new System.Windows.Forms.Button();
            this.btn_auto_charge = new System.Windows.Forms.Button();
            this.grpb_speciallyOperation = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_refsh_vh_status = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.grbp_CancelAbort = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_abort = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_id_cancel_cmdID_value = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_vhs = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_isremote_value = new System.Windows.Forms.Label();
            this.tlp_VehicleOperationBlock.SuspendLayout();
            this.grpb_VehicleInstallRemove.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.grpb_vehicleMode.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.grpb_speciallyOperation.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.grbp_CancelAbort.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp_VehicleOperationBlock
            // 
            this.tlp_VehicleOperationBlock.ColumnCount = 4;
            this.tlp_VehicleOperationBlock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_VehicleOperationBlock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_VehicleOperationBlock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_VehicleOperationBlock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_VehicleOperationBlock.Controls.Add(this.grpb_VehicleInstallRemove, 0, 0);
            this.tlp_VehicleOperationBlock.Controls.Add(this.grpb_vehicleMode, 1, 0);
            this.tlp_VehicleOperationBlock.Controls.Add(this.grpb_speciallyOperation, 3, 0);
            this.tlp_VehicleOperationBlock.Controls.Add(this.grbp_CancelAbort, 2, 0);
            this.tlp_VehicleOperationBlock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tlp_VehicleOperationBlock.Enabled = false;
            this.tlp_VehicleOperationBlock.Location = new System.Drawing.Point(0, 34);
            this.tlp_VehicleOperationBlock.Name = "tlp_VehicleOperationBlock";
            this.tlp_VehicleOperationBlock.RowCount = 1;
            this.tlp_VehicleOperationBlock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_VehicleOperationBlock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 273F));
            this.tlp_VehicleOperationBlock.Size = new System.Drawing.Size(880, 273);
            this.tlp_VehicleOperationBlock.TabIndex = 0;
            // 
            // grpb_VehicleInstallRemove
            // 
            this.grpb_VehicleInstallRemove.Controls.Add(this.tableLayoutPanel2);
            this.grpb_VehicleInstallRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpb_VehicleInstallRemove.Location = new System.Drawing.Point(3, 3);
            this.grpb_VehicleInstallRemove.Name = "grpb_VehicleInstallRemove";
            this.grpb_VehicleInstallRemove.Size = new System.Drawing.Size(214, 267);
            this.grpb_VehicleInstallRemove.TabIndex = 0;
            this.grpb_VehicleInstallRemove.TabStop = false;
            this.grpb_VehicleInstallRemove.Text = "Install / Remove";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btn_changeToRemove, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btn_changeToInstall, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 26);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(208, 238);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btn_changeToRemove
            // 
            this.btn_changeToRemove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_changeToRemove.Location = new System.Drawing.Point(3, 53);
            this.btn_changeToRemove.Name = "btn_changeToRemove";
            this.btn_changeToRemove.Size = new System.Drawing.Size(202, 44);
            this.btn_changeToRemove.TabIndex = 54;
            this.btn_changeToRemove.Text = "Remove";
            this.btn_changeToRemove.UseVisualStyleBackColor = true;
            this.btn_changeToRemove.Click += new System.EventHandler(this.btn_changeToRemove_Click);
            // 
            // btn_changeToInstall
            // 
            this.btn_changeToInstall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_changeToInstall.Location = new System.Drawing.Point(3, 103);
            this.btn_changeToInstall.Name = "btn_changeToInstall";
            this.btn_changeToInstall.Size = new System.Drawing.Size(202, 44);
            this.btn_changeToInstall.TabIndex = 55;
            this.btn_changeToInstall.Text = "Install";
            this.btn_changeToInstall.UseVisualStyleBackColor = true;
            this.btn_changeToInstall.Click += new System.EventHandler(this.btn_changeToInstall_Click);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.80198F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.19802F));
            this.tableLayoutPanel7.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.lbl_isinstalled_value, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(202, 44);
            this.tableLayoutPanel7.TabIndex = 56;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 44);
            this.label2.TabIndex = 0;
            this.label2.Text = "Is Installed:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_isinstalled_value
            // 
            this.lbl_isinstalled_value.AutoSize = true;
            this.lbl_isinstalled_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_isinstalled_value.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_isinstalled_value.Location = new System.Drawing.Point(144, 0);
            this.lbl_isinstalled_value.Name = "lbl_isinstalled_value";
            this.lbl_isinstalled_value.Size = new System.Drawing.Size(55, 44);
            this.lbl_isinstalled_value.TabIndex = 1;
            this.lbl_isinstalled_value.Text = "false";
            this.lbl_isinstalled_value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpb_vehicleMode
            // 
            this.grpb_vehicleMode.Controls.Add(this.tableLayoutPanel3);
            this.grpb_vehicleMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpb_vehicleMode.Location = new System.Drawing.Point(223, 3);
            this.grpb_vehicleMode.Name = "grpb_vehicleMode";
            this.grpb_vehicleMode.Size = new System.Drawing.Size(214, 267);
            this.grpb_vehicleMode.TabIndex = 1;
            this.grpb_vehicleMode.TabStop = false;
            this.grpb_vehicleMode.Text = "Vehicle Mode";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btn_auto_remote, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btn_auto_local, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.btn_auto_charge, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 26);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(208, 238);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // btn_auto_remote
            // 
            this.btn_auto_remote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_auto_remote.Location = new System.Drawing.Point(3, 52);
            this.btn_auto_remote.Name = "btn_auto_remote";
            this.btn_auto_remote.Size = new System.Drawing.Size(202, 43);
            this.btn_auto_remote.TabIndex = 39;
            this.btn_auto_remote.Text = "Auto Remote";
            this.btn_auto_remote.UseVisualStyleBackColor = true;
            this.btn_auto_remote.Click += new System.EventHandler(this.btn_auto_remote_Click);
            // 
            // btn_auto_local
            // 
            this.btn_auto_local.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_auto_local.Location = new System.Drawing.Point(3, 101);
            this.btn_auto_local.Name = "btn_auto_local";
            this.btn_auto_local.Size = new System.Drawing.Size(202, 43);
            this.btn_auto_local.TabIndex = 48;
            this.btn_auto_local.Text = "Auto Local";
            this.btn_auto_local.UseVisualStyleBackColor = true;
            this.btn_auto_local.Click += new System.EventHandler(this.btn_auto_local_Click);
            // 
            // btn_auto_charge
            // 
            this.btn_auto_charge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_auto_charge.Location = new System.Drawing.Point(3, 150);
            this.btn_auto_charge.Name = "btn_auto_charge";
            this.btn_auto_charge.Size = new System.Drawing.Size(202, 43);
            this.btn_auto_charge.TabIndex = 49;
            this.btn_auto_charge.Text = "Auto Charge";
            this.btn_auto_charge.UseVisualStyleBackColor = true;
            this.btn_auto_charge.Visible = false;
            this.btn_auto_charge.Click += new System.EventHandler(this.btn_auto_charge_Click);
            // 
            // grpb_speciallyOperation
            // 
            this.grpb_speciallyOperation.Controls.Add(this.tableLayoutPanel4);
            this.grpb_speciallyOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpb_speciallyOperation.Location = new System.Drawing.Point(663, 3);
            this.grpb_speciallyOperation.Name = "grpb_speciallyOperation";
            this.grpb_speciallyOperation.Size = new System.Drawing.Size(214, 267);
            this.grpb_speciallyOperation.TabIndex = 2;
            this.grpb_speciallyOperation.TabStop = false;
            this.grpb_speciallyOperation.Text = "Specially Operation";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.btn_refsh_vh_status, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.button6, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 26);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(208, 238);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // btn_refsh_vh_status
            // 
            this.btn_refsh_vh_status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_refsh_vh_status.Location = new System.Drawing.Point(3, 3);
            this.btn_refsh_vh_status.Name = "btn_refsh_vh_status";
            this.btn_refsh_vh_status.Size = new System.Drawing.Size(202, 44);
            this.btn_refsh_vh_status.TabIndex = 18;
            this.btn_refsh_vh_status.Text = "Refresh Vh Status";
            this.btn_refsh_vh_status.UseVisualStyleBackColor = true;
            this.btn_refsh_vh_status.Click += new System.EventHandler(this.btn_refsh_vh_status_Click);
            // 
            // button6
            // 
            this.button6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button6.Location = new System.Drawing.Point(3, 53);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(202, 44);
            this.button6.TabIndex = 19;
            this.button6.Text = "Force finish Cmd";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // grbp_CancelAbort
            // 
            this.grbp_CancelAbort.Controls.Add(this.tableLayoutPanel6);
            this.grbp_CancelAbort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grbp_CancelAbort.Location = new System.Drawing.Point(443, 3);
            this.grbp_CancelAbort.Name = "grbp_CancelAbort";
            this.grbp_CancelAbort.Size = new System.Drawing.Size(214, 267);
            this.grbp_CancelAbort.TabIndex = 4;
            this.grbp_CancelAbort.TabStop = false;
            this.grbp_CancelAbort.Text = "Cancel / Abort";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.btn_cancel, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.btn_abort, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.lbl_id_cancel_cmdID_value, 0, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 26);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 5;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(208, 238);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(3, 53);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(202, 43);
            this.btn_cancel.TabIndex = 28;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_abort
            // 
            this.btn_abort.Location = new System.Drawing.Point(3, 102);
            this.btn_abort.Name = "btn_abort";
            this.btn_abort.Size = new System.Drawing.Size(202, 43);
            this.btn_abort.TabIndex = 29;
            this.btn_abort.Text = "Abort";
            this.btn_abort.UseVisualStyleBackColor = true;
            this.btn_abort.Click += new System.EventHandler(this.btn_abort_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 25);
            this.label3.TabIndex = 30;
            this.label3.Text = "Command ID";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_id_cancel_cmdID_value
            // 
            this.lbl_id_cancel_cmdID_value.AutoSize = true;
            this.lbl_id_cancel_cmdID_value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_id_cancel_cmdID_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_id_cancel_cmdID_value.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_id_cancel_cmdID_value.Location = new System.Drawing.Point(3, 25);
            this.lbl_id_cancel_cmdID_value.Name = "lbl_id_cancel_cmdID_value";
            this.lbl_id_cancel_cmdID_value.Size = new System.Drawing.Size(202, 25);
            this.lbl_id_cancel_cmdID_value.TabIndex = 27;
            this.lbl_id_cancel_cmdID_value.Text = "                    ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vh:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmb_vhs
            // 
            this.cmb_vhs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_vhs.FormattingEnabled = true;
            this.cmb_vhs.Location = new System.Drawing.Point(49, 0);
            this.cmb_vhs.Name = "cmb_vhs";
            this.cmb_vhs.Size = new System.Drawing.Size(143, 30);
            this.cmb_vhs.TabIndex = 1;
            this.cmb_vhs.SelectedIndexChanged += new System.EventHandler(this.cmb_vhs_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.80198F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.19802F));
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbl_isremote_value, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(202, 43);
            this.tableLayoutPanel1.TabIndex = 57;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 43);
            this.label4.TabIndex = 0;
            this.label4.Text = "Is Remote:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_isremote_value
            // 
            this.lbl_isremote_value.AutoSize = true;
            this.lbl_isremote_value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_isremote_value.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_isremote_value.Location = new System.Drawing.Point(144, 0);
            this.lbl_isremote_value.Name = "lbl_isremote_value";
            this.lbl_isremote_value.Size = new System.Drawing.Size(55, 43);
            this.lbl_isremote_value.TabIndex = 1;
            this.lbl_isremote_value.Text = "false";
            this.lbl_isremote_value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VehicleOperationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 307);
            this.Controls.Add(this.cmb_vhs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tlp_VehicleOperationBlock);
            this.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.Name = "VehicleOperationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vehicle Operation Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VehicleOperationForm_FormClosed);
            this.tlp_VehicleOperationBlock.ResumeLayout(false);
            this.grpb_VehicleInstallRemove.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.grpb_vehicleMode.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.grpb_speciallyOperation.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.grbp_CancelAbort.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_VehicleOperationBlock;
        private System.Windows.Forms.GroupBox grpb_VehicleInstallRemove;
        private System.Windows.Forms.GroupBox grpb_vehicleMode;
        private System.Windows.Forms.GroupBox grpb_speciallyOperation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Components.uctlButton btn_changeToRemove;
        private Components.uctlButton btn_changeToInstall;
        private System.Windows.Forms.Button btn_auto_remote;
        private System.Windows.Forms.Button btn_auto_local;
        private System.Windows.Forms.Button btn_auto_charge;
        private System.Windows.Forms.Button btn_refsh_vh_status;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_vhs;
        private System.Windows.Forms.GroupBox grbp_CancelAbort;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label lbl_id_cancel_cmdID_value;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_abort;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_isinstalled_value;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_isremote_value;
    }
}