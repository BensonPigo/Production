namespace Sci.Production.Shipping
{
    partial class P43
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbDate = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.lbID = new Sci.Win.UI.Label();
            this.txtid = new Sci.Win.UI.TextBox();
            this.lbremark = new Sci.Win.UI.Label();
            this.editBoxremark = new Sci.Win.UI.EditBox();
            this.lbstatus = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.lbstatus);
            this.masterpanel.Controls.Add(this.editBoxremark);
            this.masterpanel.Controls.Add(this.lbremark);
            this.masterpanel.Controls.Add(this.txtid);
            this.masterpanel.Controls.Add(this.lbID);
            this.masterpanel.Controls.Add(this.lbDate);
            this.masterpanel.Controls.Add(this.dateDate);
            this.masterpanel.Size = new System.Drawing.Size(920, 103);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtid, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbremark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editBoxremark, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbstatus, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 103);
            this.detailpanel.Size = new System.Drawing.Size(920, 347);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(675, 65);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(920, 347);
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
            this.detail.Size = new System.Drawing.Size(920, 488);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(920, 450);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 450);
            this.detailbtm.Size = new System.Drawing.Size(920, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(920, 488);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(928, 517);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // lbDate
            // 
            this.lbDate.Location = new System.Drawing.Point(213, 9);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(80, 23);
            this.lbDate.TabIndex = 1;
            this.lbDate.Text = "Date";
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IssueDate", true));
            this.dateDate.Location = new System.Drawing.Point(297, 9);
            this.dateDate.Name = "dateDate";
            this.dateDate.ReadOnly = true;
            this.dateDate.Size = new System.Drawing.Size(100, 23);
            this.dateDate.TabIndex = 9;
            // 
            // lbID
            // 
            this.lbID.Location = new System.Drawing.Point(5, 9);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(80, 23);
            this.lbID.TabIndex = 10;
            this.lbID.Text = "ID";
            // 
            // txtid
            // 
            this.txtid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtid.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtid.IsSupportEditMode = false;
            this.txtid.Location = new System.Drawing.Point(90, 9);
            this.txtid.Name = "txtid";
            this.txtid.ReadOnly = true;
            this.txtid.Size = new System.Drawing.Size(120, 23);
            this.txtid.TabIndex = 11;
            // 
            // lbremark
            // 
            this.lbremark.Location = new System.Drawing.Point(5, 44);
            this.lbremark.Name = "lbremark";
            this.lbremark.Size = new System.Drawing.Size(80, 23);
            this.lbremark.TabIndex = 12;
            this.lbremark.Text = "Rematk";
            // 
            // editBoxremark
            // 
            this.editBoxremark.BackColor = System.Drawing.Color.White;
            this.editBoxremark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.editBoxremark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxremark.Location = new System.Drawing.Point(90, 44);
            this.editBoxremark.Multiline = true;
            this.editBoxremark.Name = "editBoxremark";
            this.editBoxremark.Size = new System.Drawing.Size(414, 50);
            this.editBoxremark.TabIndex = 13;
            // 
            // lbstatus
            // 
            this.lbstatus.BackColor = System.Drawing.Color.Transparent;
            this.lbstatus.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "status", true));
            this.lbstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.lbstatus.Location = new System.Drawing.Point(523, 9);
            this.lbstatus.Name = "lbstatus";
            this.lbstatus.Size = new System.Drawing.Size(133, 34);
            this.lbstatus.TabIndex = 31;
            this.lbstatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // P43
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(928, 550);
            this.DefaultOrder = "ID";
            this.GridAlias = "AdjustGMT_Detail";
            this.GridNew = 0;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P43";
            this.Text = "P43. Adjust Garment Qty";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "AdjustGMT";
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
        private Win.UI.DateBox dateDate;
        private Win.UI.Label lbDate;
        private Win.UI.EditBox editBoxremark;
        private Win.UI.Label lbremark;
        private Win.UI.TextBox txtid;
        private Win.UI.Label lbID;
        private Win.UI.Label lbstatus;
    }
}
