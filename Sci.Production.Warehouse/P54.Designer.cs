namespace Sci.Production.Warehouse
{
    partial class P54
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
            this.labelRemark = new Sci.Win.UI.Label();
            this.transferOutDate = new Sci.Win.UI.DateBox();
            this.labelTreansferOutDate = new Sci.Win.UI.Label();
            this.labelD = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.btnImport = new Sci.Win.UI.Button();
            this.labelNotApprove = new Sci.Win.UI.Label();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.labelSubCon = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtSubcon = new Sci.Win.UI.TextBox();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.shapeContainer3 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.labelDisplay = new Sci.Win.UI.Label();
            this.BtnFind = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.BtnFind);
            this.masterpanel.Controls.Add(this.labelDisplay);
            this.masterpanel.Controls.Add(this.txtLocateForSP);
            this.masterpanel.Controls.Add(this.labelSP);
            this.masterpanel.Controls.Add(this.txtSubcon);
            this.masterpanel.Controls.Add(this.txtID);
            this.masterpanel.Controls.Add(this.labelSubCon);
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.labelNotApprove);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.labelTreansferOutDate);
            this.masterpanel.Controls.Add(this.labelD);
            this.masterpanel.Controls.Add(this.transferOutDate);
            this.masterpanel.Size = new System.Drawing.Size(892, 163);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.transferOutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelD, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelTreansferOutDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSubCon, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSubcon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocateForSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDisplay, 0);
            this.masterpanel.Controls.SetChildIndex(this.BtnFind, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 163);
            this.detailpanel.Size = new System.Drawing.Size(892, 186);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(822, 123);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 186);
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
            this.browse.Size = new System.Drawing.Size(933, 509);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(941, 538);
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(97, 39);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(658, 87);
            this.editRemark.TabIndex = 82;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(16, 40);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 84;
            this.labelRemark.Text = "Remark";
            // 
            // transferOutDate
            // 
            this.transferOutDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "TransferOutDate", true));
            this.transferOutDate.Location = new System.Drawing.Point(353, 10);
            this.transferOutDate.Name = "transferOutDate";
            this.transferOutDate.Size = new System.Drawing.Size(130, 23);
            this.transferOutDate.TabIndex = 80;
            // 
            // labelTreansferOutDate
            // 
            this.labelTreansferOutDate.Location = new System.Drawing.Point(231, 10);
            this.labelTreansferOutDate.Name = "labelTreansferOutDate";
            this.labelTreansferOutDate.Size = new System.Drawing.Size(119, 23);
            this.labelTreansferOutDate.TabIndex = 83;
            this.labelTreansferOutDate.Text = "Transfer Out Date";
            // 
            // labelD
            // 
            this.labelD.Location = new System.Drawing.Point(16, 10);
            this.labelD.Name = "labelD";
            this.labelD.Size = new System.Drawing.Size(75, 23);
            this.labelD.TabIndex = 81;
            this.labelD.Text = "ID";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(954, 186);
            this.shapeContainer1.TabIndex = 75;
            this.shapeContainer1.TabStop = false;
            // 
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(829, 39);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(89, 31);
            this.btnImport.TabIndex = 88;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // labelNotApprove
            // 
            this.labelNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.labelNotApprove.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.labelNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelNotApprove.Location = new System.Drawing.Point(803, 10);
            this.labelNotApprove.Name = "labelNotApprove";
            this.labelNotApprove.Size = new System.Drawing.Size(115, 23);
            this.labelNotApprove.TabIndex = 89;
            this.labelNotApprove.Text = "Not Approve";
            this.labelNotApprove.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Size = new System.Drawing.Size(954, 186);
            this.shapeContainer2.TabIndex = 75;
            this.shapeContainer2.TabStop = false;
            // 
            // labelSubCon
            // 
            this.labelSubCon.Location = new System.Drawing.Point(486, 10);
            this.labelSubCon.Name = "labelSubCon";
            this.labelSubCon.Size = new System.Drawing.Size(75, 23);
            this.labelSubCon.TabIndex = 91;
            this.labelSubCon.Text = "Sub con";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(94, 10);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(134, 23);
            this.txtID.TabIndex = 92;
            // 
            // txtSubcon
            // 
            this.txtSubcon.BackColor = System.Drawing.Color.White;
            this.txtSubcon.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Subcon", true));
            this.txtSubcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubcon.Location = new System.Drawing.Point(564, 10);
            this.txtSubcon.Name = "txtSubcon";
            this.txtSubcon.Size = new System.Drawing.Size(134, 23);
            this.txtSubcon.TabIndex = 93;
            this.txtSubcon.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSubcon_PopUp);
            this.txtSubcon.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubcon_Validating);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(128, 133);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 95;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(16, 133);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(109, 23);
            this.labelSP.TabIndex = 97;
            this.labelSP.Text = "Locate for SP#";
            // 
            // shapeContainer3
            // 
            this.shapeContainer3.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer3.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer3.Name = "shapeContainer3";
            this.shapeContainer3.Size = new System.Drawing.Size(977, 184);
            this.shapeContainer3.TabIndex = 54;
            this.shapeContainer3.TabStop = false;
            // 
            // labelDisplay
            // 
            this.labelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDisplay.BackColor = System.Drawing.Color.Transparent;
            this.labelDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.labelDisplay.Location = new System.Drawing.Point(353, 133);
            this.labelDisplay.Name = "labelDisplay";
            this.labelDisplay.Size = new System.Drawing.Size(157, 23);
            this.labelDisplay.TabIndex = 98;
            this.labelDisplay.Text = "Display Fabric data only.";
            this.labelDisplay.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // BtnFind
            // 
            this.BtnFind.Location = new System.Drawing.Point(279, 133);
            this.BtnFind.Name = "BtnFind";
            this.BtnFind.Size = new System.Drawing.Size(67, 24);
            this.BtnFind.TabIndex = 99;
            this.BtnFind.Text = "Find";
            this.BtnFind.UseVisualStyleBackColor = true;
            this.BtnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // P54
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 571);
            this.GridAlias = "TransferToSubcon_Detail";
            this.GridNew = 0;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P54";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P54. Transfer to Sub con";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "TransferToSubcon";
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
        private Win.UI.Label labelRemark;
        private Win.UI.DateBox transferOutDate;
        private Win.UI.Label labelTreansferOutDate;
        private Win.UI.Label labelD;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.Button btnImport;
        private Win.UI.Label labelNotApprove;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Win.UI.Label labelSubCon;
        private Win.UI.TextBox txtID;
        private Win.UI.TextBox txtSubcon;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelSP;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer3;
        private Win.UI.Label labelDisplay;
        private Win.UI.Button BtnFind;
    }
}