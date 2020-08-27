namespace Sci.Production.Sewing
{
    partial class P11
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.lbstatus = new Sci.Win.UI.Label();
            this.disID = new Sci.Win.UI.DisplayBox();
            this.dateCreateDate = new Sci.Win.UI.DateBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.disFactoryID = new Sci.Win.UI.DisplayBox();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnTransferInOutInformation = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.disFactoryID);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.disID);
            this.masterpanel.Controls.Add(this.dateCreateDate);
            this.masterpanel.Controls.Add(this.lbstatus);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Size = new System.Drawing.Size(892, 88);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbstatus, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateCreateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.disID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.disFactoryID, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 88);
            this.detailpanel.Size = new System.Drawing.Size(892, 261);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(792, 50);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 261);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(900, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(908, 417);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(216, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(427, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(5, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Remark";
            // 
            // lbstatus
            // 
            this.lbstatus.AutoSize = true;
            this.lbstatus.BackColor = System.Drawing.Color.Transparent;
            this.lbstatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.lbstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lbstatus.Location = new System.Drawing.Point(619, 12);
            this.lbstatus.MaximumSize = new System.Drawing.Size(61, 24);
            this.lbstatus.MinimumSize = new System.Drawing.Size(61, 24);
            this.lbstatus.Name = "lbstatus";
            this.lbstatus.Size = new System.Drawing.Size(61, 24);
            this.lbstatus.TabIndex = 36;
            this.lbstatus.Text = "Status";
            this.lbstatus.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.lbstatus.TextStyle.Color = System.Drawing.Color.Red;
            this.lbstatus.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.lbstatus.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // disID
            // 
            this.disID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.disID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disID.Location = new System.Drawing.Point(70, 12);
            this.disID.Name = "disID";
            this.disID.Size = new System.Drawing.Size(143, 23);
            this.disID.TabIndex = 0;
            // 
            // dateCreateDate
            // 
            this.dateCreateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CreateDate", true));
            this.dateCreateDate.IsSupportEditMode = false;
            this.dateCreateDate.Location = new System.Drawing.Point(294, 12);
            this.dateCreateDate.Name = "dateCreateDate";
            this.dateCreateDate.ReadOnly = true;
            this.dateCreateDate.Size = new System.Drawing.Size(130, 23);
            this.dateCreateDate.TabIndex = 1;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(70, 52);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(719, 23);
            this.txtRemark.TabIndex = 3;
            // 
            // disFactoryID
            // 
            this.disFactoryID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disFactoryID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.disFactoryID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disFactoryID.Location = new System.Drawing.Point(505, 12);
            this.disFactoryID.Name = "disFactoryID";
            this.disFactoryID.Size = new System.Drawing.Size(100, 23);
            this.disFactoryID.TabIndex = 2;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Location = new System.Drawing.Point(784, 12);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 30);
            this.btnImport.TabIndex = 37;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnTransferInOutInformation
            // 
            this.btnTransferInOutInformation.Location = new System.Drawing.Point(678, 3);
            this.btnTransferInOutInformation.Name = "btnTransferInOutInformation";
            this.btnTransferInOutInformation.Size = new System.Drawing.Size(226, 30);
            this.btnTransferInOutInformation.TabIndex = 36;
            this.btnTransferInOutInformation.Text = "Transfer In/Out Information";
            this.btnTransferInOutInformation.UseVisualStyleBackColor = true;
            this.btnTransferInOutInformation.Click += new System.EventHandler(this.BtnTransferInOutInformation_Click);
            // 
            // P11
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 450);
            this.Controls.Add(this.btnTransferInOutInformation);
            this.GridAlias = "SewingOutputTransfer_Detail";
            this.IsSupportClip = false;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "P11";
            this.OnLineHelpID = "Sci.Win.Tems.Input8";
            this.Text = "P11. Sewing Output Transfer";
            this.WorkAlias = "SewingOutputTransfer";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnTransferInOutInformation, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox disID;
        private Win.UI.Label lbstatus;
        private Win.UI.TextBox txtRemark;
        private Win.UI.DateBox dateCreateDate;
        private Win.UI.DisplayBox disFactoryID;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnTransferInOutInformation;
    }
}