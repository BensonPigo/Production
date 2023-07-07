namespace Sci.Production.Warehouse
{
    partial class P69_CreateSeq
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
            this.btnClose = new Sci.Win.UI.Button();
            this.btnCreate = new Sci.Win.UI.Button();
            this.EditDesc = new Sci.Win.UI.EditBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtUnit = new Sci.Win.UI.TextBox();
            this.lblUnit = new Sci.Win.UI.Label();
            this.lblMaterialType = new Sci.Win.UI.Label();
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.lblSeq = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.lblSP = new Sci.Win.UI.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSeq2 = new Sci.Win.UI.TextBox();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.lblSize = new Sci.Win.UI.Label();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.lblColor = new Sci.Win.UI.Label();
            this.lblWeaveType = new Sci.Win.UI.Label();
            this.lblFabricType = new Sci.Win.UI.Label();
            this.cbFabricType = new Sci.Win.UI.ComboBox();
            this.lblRefno = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.txtWeaveType1 = new Sci.Production.Class.TxtWeaveType();
            this.txtMtlType = new Sci.Production.Class.TxtMtlType();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(405, 223);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Location = new System.Drawing.Point(313, 223);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(80, 30);
            this.btnCreate.TabIndex = 17;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // EditDesc
            // 
            this.EditDesc.BackColor = System.Drawing.Color.White;
            this.EditDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.EditDesc.Location = new System.Drawing.Point(99, 140);
            this.EditDesc.Multiline = true;
            this.EditDesc.Name = "EditDesc";
            this.EditDesc.Size = new System.Drawing.Size(386, 77);
            this.EditDesc.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 23);
            this.label4.TabIndex = 21;
            this.label4.Text = "Descrition";
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.Color.White;
            this.txtUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUnit.Location = new System.Drawing.Point(99, 114);
            this.txtUnit.MaxLength = 8;
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(151, 23);
            this.txtUnit.TabIndex = 9;
            // 
            // lblUnit
            // 
            this.lblUnit.Location = new System.Drawing.Point(9, 114);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(87, 23);
            this.lblUnit.TabIndex = 23;
            this.lblUnit.Text = "Unit";
            // 
            // lblMaterialType
            // 
            this.lblMaterialType.Location = new System.Drawing.Point(9, 88);
            this.lblMaterialType.Name = "lblMaterialType";
            this.lblMaterialType.Size = new System.Drawing.Size(87, 23);
            this.lblMaterialType.TabIndex = 22;
            this.lblMaterialType.Text = "Material Type";
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.IsSupportEditMode = false;
            this.txtSeq1.Location = new System.Drawing.Point(343, 9);
            this.txtSeq1.MaxLength = 3;
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(64, 23);
            this.txtSeq1.TabIndex = 2;
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(253, 9);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(87, 23);
            this.lblSeq.TabIndex = 20;
            this.lblSeq.Text = "Seq";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.IsSupportEditMode = false;
            this.txtSP.Location = new System.Drawing.Point(99, 9);
            this.txtSP.MaxLength = 13;
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(151, 23);
            this.txtSP.TabIndex = 1;
            // 
            // lblSP
            // 
            this.lblSP.Location = new System.Drawing.Point(9, 9);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(87, 23);
            this.lblSP.TabIndex = 19;
            this.lblSP.Text = "SP#";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(408, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 17);
            this.label5.TabIndex = 24;
            this.label5.Text = "-";
            // 
            // txtSeq2
            // 
            this.txtSeq2.BackColor = System.Drawing.Color.White;
            this.txtSeq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq2.IsSupportEditMode = false;
            this.txtSeq2.Location = new System.Drawing.Point(421, 9);
            this.txtSeq2.MaxLength = 2;
            this.txtSeq2.Name = "txtSeq2";
            this.txtSeq2.Size = new System.Drawing.Size(64, 23);
            this.txtSeq2.TabIndex = 3;
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.White;
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSize.Location = new System.Drawing.Point(343, 114);
            this.txtSize.MaxLength = 15;
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(142, 23);
            this.txtSize.TabIndex = 10;
            // 
            // lblSize
            // 
            this.lblSize.Location = new System.Drawing.Point(253, 114);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(87, 23);
            this.lblSize.TabIndex = 27;
            this.lblSize.Text = "Size";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.IsSupportEditMode = false;
            this.txtColor.Location = new System.Drawing.Point(343, 88);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(142, 23);
            this.txtColor.TabIndex = 8;
            // 
            // lblColor
            // 
            this.lblColor.Location = new System.Drawing.Point(253, 88);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(87, 23);
            this.lblColor.TabIndex = 29;
            this.lblColor.Text = "Color";
            // 
            // lblWeaveType
            // 
            this.lblWeaveType.Location = new System.Drawing.Point(253, 62);
            this.lblWeaveType.Name = "lblWeaveType";
            this.lblWeaveType.Size = new System.Drawing.Size(87, 23);
            this.lblWeaveType.TabIndex = 33;
            this.lblWeaveType.Text = "Weave Type";
            // 
            // lblFabricType
            // 
            this.lblFabricType.Location = new System.Drawing.Point(9, 62);
            this.lblFabricType.Name = "lblFabricType";
            this.lblFabricType.Size = new System.Drawing.Size(87, 23);
            this.lblFabricType.TabIndex = 31;
            this.lblFabricType.Text = "Fabric Type";
            // 
            // cbFabricType
            // 
            this.cbFabricType.BackColor = System.Drawing.Color.White;
            this.cbFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbFabricType.FormattingEnabled = true;
            this.cbFabricType.IsSupportUnselect = true;
            this.cbFabricType.Location = new System.Drawing.Point(99, 61);
            this.cbFabricType.Name = "cbFabricType";
            this.cbFabricType.OldText = "";
            this.cbFabricType.Size = new System.Drawing.Size(151, 24);
            this.cbFabricType.TabIndex = 5;
            // 
            // lblRefno
            // 
            this.lblRefno.Location = new System.Drawing.Point(9, 35);
            this.lblRefno.Name = "lblRefno";
            this.lblRefno.Size = new System.Drawing.Size(87, 23);
            this.lblRefno.TabIndex = 35;
            this.lblRefno.Text = "Refno";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.IsSupportEditMode = false;
            this.txtRefno.Location = new System.Drawing.Point(99, 35);
            this.txtRefno.MaxLength = 36;
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(386, 23);
            this.txtRefno.TabIndex = 4;
            // 
            // txtWeaveType1
            // 
            this.txtWeaveType1.BackColor = System.Drawing.Color.White;
            this.txtWeaveType1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWeaveType1.IsSupportEditMode = false;
            this.txtWeaveType1.Location = new System.Drawing.Point(343, 62);
            this.txtWeaveType1.MaxLength = 20;
            this.txtWeaveType1.Name = "txtWeaveType1";
            this.txtWeaveType1.Size = new System.Drawing.Size(142, 23);
            this.txtWeaveType1.TabIndex = 6;
            // 
            // txtMtlType
            // 
            this.txtMtlType.BackColor = System.Drawing.Color.White;
            this.txtMtlType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlType.IsSupportEditMode = false;
            this.txtMtlType.Location = new System.Drawing.Point(99, 88);
            this.txtMtlType.Name = "txtMtlType";
            this.txtMtlType.Size = new System.Drawing.Size(151, 23);
            this.txtMtlType.TabIndex = 7;
            // 
            // P69_CreateSeq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 260);
            this.Controls.Add(this.txtMtlType);
            this.Controls.Add(this.txtWeaveType1);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.lblRefno);
            this.Controls.Add(this.cbFabricType);
            this.Controls.Add(this.lblWeaveType);
            this.Controls.Add(this.lblFabricType);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.txtSeq2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.EditDesc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.lblUnit);
            this.Controls.Add(this.lblMaterialType);
            this.Controls.Add(this.txtSeq1);
            this.Controls.Add(this.lblSeq);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.lblSP);
            this.Name = "P69_CreateSeq";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.Button btnCreate;
        private Win.UI.EditBox EditDesc;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtUnit;
        private Win.UI.Label lblUnit;
        private Win.UI.Label lblMaterialType;
        private Win.UI.TextBox txtSeq1;
        private Win.UI.Label lblSeq;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label lblSP;
        private System.Windows.Forms.Label label5;
        private Win.UI.TextBox txtSeq2;
        private Win.UI.TextBox txtSize;
        private Win.UI.Label lblSize;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label lblColor;
        private Win.UI.Label lblWeaveType;
        private Win.UI.Label lblFabricType;
        private Win.UI.ComboBox cbFabricType;
        private Win.UI.Label lblRefno;
        private Win.UI.TextBox txtRefno;
        private Class.TxtWeaveType txtWeaveType1;
        private Class.TxtMtlType txtMtlType;
    }
}