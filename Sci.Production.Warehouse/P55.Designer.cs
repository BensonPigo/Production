namespace Sci.Production.Warehouse
{
    partial class P55
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
            this.editRemark = new Sci.Win.UI.EditBox();
            this.lblRemark = new Sci.Win.UI.Label();
            this.dateReturnDate = new Sci.Win.UI.DateBox();
            this.txtID = new Sci.Win.UI.DisplayBox();
            this.lblReturnDate = new Sci.Win.UI.Label();
            this.lblID = new Sci.Win.UI.Label();
            this.lblNotApprove = new Sci.Win.UI.Label();
            this.BtnFind = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.lblSubcon = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.labelDisplay = new Sci.Win.UI.Label();
            this.txtSubcon = new Sci.Win.UI.TextBox();
            this.BtnImport = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.BtnImport);
            this.masterpanel.Controls.Add(this.txtSubcon);
            this.masterpanel.Controls.Add(this.labelDisplay);
            this.masterpanel.Controls.Add(this.lblSubcon);
            this.masterpanel.Controls.Add(this.BtnFind);
            this.masterpanel.Controls.Add(this.txtLocateForSP);
            this.masterpanel.Controls.Add(this.labelSP);
            this.masterpanel.Controls.Add(this.lblNotApprove);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.lblRemark);
            this.masterpanel.Controls.Add(this.txtID);
            this.masterpanel.Controls.Add(this.lblReturnDate);
            this.masterpanel.Controls.Add(this.lblID);
            this.masterpanel.Controls.Add(this.dateReturnDate);
            this.masterpanel.Size = new System.Drawing.Size(892, 164);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateReturnDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblID, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblReturnDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtID, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.BtnFind, 0);
            this.masterpanel.Controls.SetChildIndex(this.lblSubcon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDisplay, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubcon, 0);
            this.masterpanel.Controls.SetChildIndex(this.BtnImport, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 164);
            this.detailpanel.Size = new System.Drawing.Size(892, 185);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(782, 127);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 185);
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
            this.browse.Size = new System.Drawing.Size(888, 567);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(896, 596);
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(94, 50);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(658, 76);
            this.editRemark.TabIndex = 70;
            // 
            // lblRemark
            // 
            this.lblRemark.Location = new System.Drawing.Point(13, 50);
            this.lblRemark.Name = "lblRemark";
            this.lblRemark.Size = new System.Drawing.Size(75, 48);
            this.lblRemark.TabIndex = 72;
            this.lblRemark.Text = "Remark";
            // 
            // dateReturnDate
            // 
            this.dateReturnDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ReturnDate", true));
            this.dateReturnDate.Location = new System.Drawing.Point(342, 14);
            this.dateReturnDate.Name = "dateReturnDate";
            this.dateReturnDate.Size = new System.Drawing.Size(130, 23);
            this.dateReturnDate.TabIndex = 66;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.Location = new System.Drawing.Point(95, 14);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(120, 23);
            this.txtID.TabIndex = 67;
            // 
            // lblReturnDate
            // 
            this.lblReturnDate.Location = new System.Drawing.Point(231, 14);
            this.lblReturnDate.Name = "lblReturnDate";
            this.lblReturnDate.Size = new System.Drawing.Size(108, 23);
            this.lblReturnDate.TabIndex = 71;
            this.lblReturnDate.Text = "Return Date";
            // 
            // lblID
            // 
            this.lblID.Location = new System.Drawing.Point(13, 14);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(75, 23);
            this.lblID.TabIndex = 69;
            this.lblID.Text = "ID";
            // 
            // lblNotApprove
            // 
            this.lblNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.lblNotApprove.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.lblNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblNotApprove.Location = new System.Drawing.Point(768, 14);
            this.lblNotApprove.Name = "lblNotApprove";
            this.lblNotApprove.Size = new System.Drawing.Size(115, 23);
            this.lblNotApprove.TabIndex = 76;
            this.lblNotApprove.Text = "Not Approve";
            this.lblNotApprove.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // BtnFind
            // 
            this.BtnFind.Location = new System.Drawing.Point(276, 133);
            this.BtnFind.Name = "BtnFind";
            this.BtnFind.Size = new System.Drawing.Size(67, 24);
            this.BtnFind.TabIndex = 103;
            this.BtnFind.Text = "Find";
            this.BtnFind.UseVisualStyleBackColor = true;
            this.BtnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(125, 133);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 100;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(13, 133);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(109, 23);
            this.labelSP.TabIndex = 101;
            this.labelSP.Text = "Locate for SP#";
            // 
            // lblSubcon
            // 
            this.lblSubcon.Location = new System.Drawing.Point(486, 14);
            this.lblSubcon.Name = "lblSubcon";
            this.lblSubcon.Size = new System.Drawing.Size(75, 23);
            this.lblSubcon.TabIndex = 105;
            this.lblSubcon.Text = "Sub con";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(977, 184);
            this.shapeContainer1.TabIndex = 54;
            this.shapeContainer1.TabStop = false;
            // 
            // labelDisplay
            // 
            this.labelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDisplay.BackColor = System.Drawing.Color.Transparent;
            this.labelDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelDisplay.Location = new System.Drawing.Point(352, 133);
            this.labelDisplay.Name = "labelDisplay";
            this.labelDisplay.Size = new System.Drawing.Size(157, 23);
            this.labelDisplay.TabIndex = 106;
            this.labelDisplay.Text = "Display Fabric data only.";
            this.labelDisplay.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtSubcon
            // 
            this.txtSubcon.BackColor = System.Drawing.Color.White;
            this.txtSubcon.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Subcon", true));
            this.txtSubcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubcon.Location = new System.Drawing.Point(564, 14);
            this.txtSubcon.Name = "txtSubcon";
            this.txtSubcon.Size = new System.Drawing.Size(145, 23);
            this.txtSubcon.TabIndex = 107;
            this.txtSubcon.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSubcon_PopUp);
            this.txtSubcon.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubcon_Validating);
            // 
            // BtnImport
            // 
            this.BtnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.BtnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnImport.Location = new System.Drawing.Point(782, 50);
            this.BtnImport.Name = "BtnImport";
            this.BtnImport.Size = new System.Drawing.Size(89, 31);
            this.BtnImport.TabIndex = 108;
            this.BtnImport.Text = "Import";
            this.BtnImport.UseVisualStyleBackColor = true;
            this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // P55
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 629);
            this.GridAlias = "SubconReturn_Detail";
            this.GridNew = 0;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P55";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P55. Sub con Return";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "SubconReturn";
            this.Controls.SetChildIndex(this.tabs, 0);
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
        private Win.UI.EditBox editRemark;
        private Win.UI.Label lblRemark;
        private Win.UI.DateBox dateReturnDate;
        private Win.UI.DisplayBox txtID;
        private Win.UI.Label lblReturnDate;
        private Win.UI.Label lblID;
        private Win.UI.Label lblNotApprove;
        private Win.UI.Button BtnFind;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelDisplay;
        private Win.UI.Label lblSubcon;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.TextBox txtSubcon;
        private Win.UI.Button BtnImport;
    }
}