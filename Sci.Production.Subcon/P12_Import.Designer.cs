namespace Sci.Production.Subcon
{
    partial class P12_Import
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.lblContractNo = new Sci.Win.UI.Label();
            this.lblSP = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.lblSupplier = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtFactory = new Sci.Production.Class.Txtfactory();
            this.chBalQty = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.lblFactory = new Sci.Win.UI.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtContractNo_2 = new Sci.Win.UI.TextBox();
            this.txtContractNo_1 = new Sci.Win.UI.TextBox();
            this.txtSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 397);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(911, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(815, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(719, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // lblContractNo
            // 
            this.lblContractNo.Location = new System.Drawing.Point(9, 48);
            this.lblContractNo.Name = "lblContractNo";
            this.lblContractNo.Size = new System.Drawing.Size(95, 23);
            this.lblContractNo.TabIndex = 11;
            this.lblContractNo.Text = "Contract No.";
            // 
            // lblSP
            // 
            this.lblSP.Location = new System.Drawing.Point(407, 13);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(77, 23);
            this.lblSP.TabIndex = 8;
            this.lblSP.Text = "SP#";
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(798, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(101, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // lblSupplier
            // 
            this.lblSupplier.Location = new System.Drawing.Point(9, 15);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(95, 23);
            this.lblSupplier.TabIndex = 0;
            this.lblSupplier.Text = "Supplier";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFactory);
            this.groupBox1.Controls.Add(this.chBalQty);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtSP2);
            this.groupBox1.Controls.Add(this.txtSP1);
            this.groupBox1.Controls.Add(this.lblFactory);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtContractNo_2);
            this.groupBox1.Controls.Add(this.txtContractNo_1);
            this.groupBox1.Controls.Add(this.txtSupplier);
            this.groupBox1.Controls.Add(this.lblContractNo);
            this.groupBox1.Controls.Add(this.lblSP);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.lblSupplier);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(911, 86);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtFactory.BoolFtyGroupList = true;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtFactory.IsMultiselect = false;
            this.txtFactory.IsProduceFty = true;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(340, 12);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.ReadOnly = true;
            this.txtFactory.Size = new System.Drawing.Size(64, 23);
            this.txtFactory.TabIndex = 28;
            // 
            // chBalQty
            // 
            this.chBalQty.AutoSize = true;
            this.chBalQty.Checked = true;
            this.chBalQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chBalQty.Location = new System.Drawing.Point(407, 50);
            this.chBalQty.Name = "chBalQty";
            this.chBalQty.Size = new System.Drawing.Size(170, 21);
            this.chBalQty.TabIndex = 27;
            this.chBalQty.Text = "Only show Bal. Qty > 0";
            this.chBalQty.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(626, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 17);
            this.label1.TabIndex = 26;
            this.label1.Text = "～";
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(648, 15);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(136, 23);
            this.txtSP2.TabIndex = 25;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(487, 15);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(136, 23);
            this.txtSP1.TabIndex = 24;
            // 
            // lblFactory
            // 
            this.lblFactory.Location = new System.Drawing.Point(284, 14);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(53, 23);
            this.lblFactory.TabIndex = 22;
            this.lblFactory.Text = "Factory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 21;
            this.label2.Text = "～";
            // 
            // txtContractNo_2
            // 
            this.txtContractNo_2.BackColor = System.Drawing.Color.White;
            this.txtContractNo_2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContractNo_2.Location = new System.Drawing.Point(268, 48);
            this.txtContractNo_2.Name = "txtContractNo_2";
            this.txtContractNo_2.Size = new System.Drawing.Size(136, 23);
            this.txtContractNo_2.TabIndex = 20;
            // 
            // txtContractNo_1
            // 
            this.txtContractNo_1.BackColor = System.Drawing.Color.White;
            this.txtContractNo_1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContractNo_1.Location = new System.Drawing.Point(107, 48);
            this.txtContractNo_1.Name = "txtContractNo_1";
            this.txtContractNo_1.Size = new System.Drawing.Size(136, 23);
            this.txtContractNo_1.TabIndex = 19;
            // 
            // txtSupplier
            // 
            this.txtSupplier.DisplayBox1Binding = "";
            this.txtSupplier.IsFreightForwarder = false;
            this.txtSupplier.IsIncludeJunk = false;
            this.txtSupplier.IsMisc = false;
            this.txtSupplier.IsShipping = false;
            this.txtSupplier.IsSubcon = false;
            this.txtSupplier.Location = new System.Drawing.Point(107, 15);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(174, 23);
            this.txtSupplier.TabIndex = 18;
            this.txtSupplier.TextBox1Binding = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 86);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(911, 311);
            this.panel1.TabIndex = 23;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(911, 311);
            this.gridImport.TabIndex = 1;
            this.gridImport.TabStop = false;
            // 
            // P12_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "P12_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P12_Import";
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Label lblContractNo;
        private Win.UI.Label lblSP;
        private Win.UI.Button btnFind;
        private Win.UI.Label lblSupplier;
        private Win.UI.GroupBox groupBox1;
        private Class.TxtsubconNoConfirm txtSupplier;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtContractNo_2;
        private Win.UI.TextBox txtContractNo_1;
        private System.Windows.Forms.Label label1;
        private Win.UI.TextBox txtSP2;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label lblFactory;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private System.Windows.Forms.CheckBox chBalQty;
        private Class.Txtfactory txtFactory;
    }
}