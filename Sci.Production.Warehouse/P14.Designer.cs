namespace Sci.Production.Warehouse
{
    partial class P14
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
            this.txtOrderID = new Sci.Win.UI.TextBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.dateIssueDate = new Sci.Win.UI.DateBox();
            this.checkToSisterFty = new Sci.Win.UI.CheckBox();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.labelNotApprove = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Win.UI.TextBox();
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
            this.masterpanel.Controls.Add(this.txtfactory);
            this.masterpanel.Controls.Add(this.labelNotApprove);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.displayID);
            this.masterpanel.Controls.Add(this.checkToSisterFty);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.txtOrderID);
            this.masterpanel.Controls.Add(this.dateIssueDate);
            this.masterpanel.Size = new System.Drawing.Size(792, 131);
            this.masterpanel.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtOrderID, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkToSisterFty, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayID, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelNotApprove, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtfactory, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 131);
            this.detailpanel.Size = new System.Drawing.Size(792, 219);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(684, 91);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(792, 219);
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
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(792, 388);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(792, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // txtOrderID
            // 
            this.txtOrderID.BackColor = System.Drawing.Color.White;
            this.txtOrderID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderId", true));
            this.txtOrderID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderID.Location = new System.Drawing.Point(107, 44);
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(136, 23);
            this.txtOrderID.TabIndex = 2;
            this.txtOrderID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtOrderID_PopUp);
            this.txtOrderID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtOrderID_Validating);
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(107, 73);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(571, 50);
            this.editRemark.TabIndex = 3;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateIssueDate.Location = new System.Drawing.Point(400, 15);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(130, 23);
            this.dateIssueDate.TabIndex = 4;
            // 
            // checkToSisterFty
            // 
            this.checkToSisterFty.AutoSize = true;
            this.checkToSisterFty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ToSisterFty", true));
            this.checkToSisterFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkToSisterFty.Location = new System.Drawing.Point(298, 46);
            this.checkToSisterFty.Name = "checkToSisterFty";
            this.checkToSisterFty.Size = new System.Drawing.Size(129, 21);
            this.checkToSisterFty.TabIndex = 5;
            this.checkToSisterFty.Text = "To sister factory";
            this.checkToSisterFty.UseVisualStyleBackColor = true;
            this.checkToSisterFty.CheckedChanged += new System.EventHandler(this.CheckToSisterFty_CheckedChanged);
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(107, 15);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(136, 23);
            this.displayID.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Order ID";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "Remark";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(298, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "Issue Date";
            // 
            // labelNotApprove
            // 
            this.labelNotApprove.BackColor = System.Drawing.Color.Transparent;
            this.labelNotApprove.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Status", true));
            this.labelNotApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labelNotApprove.Location = new System.Drawing.Point(669, 15);
            this.labelNotApprove.Name = "labelNotApprove";
            this.labelNotApprove.Size = new System.Drawing.Size(115, 23);
            this.labelNotApprove.TabIndex = 44;
            this.labelNotApprove.Text = "Status";
            this.labelNotApprove.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ToFactory", true));
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(433, 46);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(97, 23);
            this.txtfactory.TabIndex = 45;
            this.txtfactory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.Txtfactory_PopUp);
            this.txtfactory.Validating += new System.ComponentModel.CancelEventHandler(this.Txtfactory_Validating);
            // 
            // P14
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DefaultDetailOrder = "Seq1,Seq2";
            this.DefaultOrder = "IssueDate,ID";
            this.GridAlias = "Issue_Detail";
            this.GridUniqueKey = "Poid,Seq1,Seq2";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.Name = "P14";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P14. Issue Thread Allowance";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Issue";
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

        private Win.UI.DateBox dateIssueDate;
        private Win.UI.EditBox editRemark;
        private Win.UI.TextBox txtOrderID;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayID;
        private Win.UI.CheckBox checkToSisterFty;
        private Win.UI.Label labelNotApprove;
        private Win.UI.TextBox txtfactory;
    }
}