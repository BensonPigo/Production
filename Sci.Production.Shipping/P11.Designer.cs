namespace Sci.Production.Shipping
{
    partial class P11
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
            this.components = new System.ComponentModel.Container();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnimport = new Sci.Win.UI.Button();
            this.txtInvSerial = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.lbExVoucherID = new Sci.Win.UI.Label();
            this.disExVoucherID = new Sci.Win.UI.DisplayBox();
            this.btnBatchApprove = new Sci.Win.UI.Button();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.disExVoucherID);
            this.masterpanel.Controls.Add(this.lbExVoucherID);
            this.masterpanel.Controls.Add(this.txtbrand);
            this.masterpanel.Controls.Add(this.btnimport);
            this.masterpanel.Controls.Add(this.txtInvSerial);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Size = new System.Drawing.Size(779, 99);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtInvSerial, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnimport, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtbrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbExVoucherID, 0);
            this.masterpanel.Controls.SetChildIndex(this.disExVoucherID, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Controls.Add(this.panel4);
            this.detailpanel.Location = new System.Drawing.Point(0, 99);
            this.detailpanel.Size = new System.Drawing.Size(779, 387);
            this.detailpanel.Controls.SetChildIndex(this.panel4, 0);
            this.detailpanel.Controls.SetChildIndex(this.detailgridcont, 0);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(430, 63);
            this.gridicon.TabIndex = 1;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(779, 387);
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
            this.detail.Size = new System.Drawing.Size(779, 524);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(779, 486);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 486);
            this.detailbtm.Size = new System.Drawing.Size(779, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(779, 524);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(787, 553);
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
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(779, 387);
            this.panel4.TabIndex = 5;
            // 
            // btnimport
            // 
            this.btnimport.Location = new System.Drawing.Point(284, 63);
            this.btnimport.Name = "btnimport";
            this.btnimport.Size = new System.Drawing.Size(80, 30);
            this.btnimport.TabIndex = 9;
            this.btnimport.Text = "Import";
            this.btnimport.UseVisualStyleBackColor = true;
            this.btnimport.Click += new System.EventHandler(this.Btnimport_Click);
            // 
            // txtInvSerial
            // 
            this.txtInvSerial.BackColor = System.Drawing.Color.White;
            this.txtInvSerial.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InvSerial", true));
            this.txtInvSerial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvSerial.Location = new System.Drawing.Point(124, 9);
            this.txtInvSerial.Name = "txtInvSerial";
            this.txtInvSerial.Size = new System.Drawing.Size(100, 23);
            this.txtInvSerial.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Brand";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Invoice Serial";
            // 
            // lbExVoucherID
            // 
            this.lbExVoucherID.Location = new System.Drawing.Point(14, 70);
            this.lbExVoucherID.Name = "lbExVoucherID";
            this.lbExVoucherID.Size = new System.Drawing.Size(107, 23);
            this.lbExVoucherID.TabIndex = 12;
            this.lbExVoucherID.Text = "Ex Voucher No.";
            // 
            // disExVoucherID
            // 
            this.disExVoucherID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disExVoucherID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disExVoucherID.Location = new System.Drawing.Point(124, 70);
            this.disExVoucherID.Name = "disExVoucherID";
            this.disExVoucherID.Size = new System.Drawing.Size(153, 23);
            this.disExVoucherID.TabIndex = 13;
            // 
            // btnBatchApprove
            // 
            this.btnBatchApprove.Location = new System.Drawing.Point(631, 3);
            this.btnBatchApprove.Name = "btnBatchApprove";
            this.btnBatchApprove.Size = new System.Drawing.Size(144, 30);
            this.btnBatchApprove.TabIndex = 3;
            this.btnBatchApprove.Text = "Batch Approve";
            this.btnBatchApprove.UseVisualStyleBackColor = true;
            this.btnBatchApprove.Visible = false;
            this.btnBatchApprove.Click += new System.EventHandler(this.BtnBatchApprove);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(124, 40);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 11;
            // 
            // P11
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(787, 586);
            this.Controls.Add(this.btnBatchApprove);
            this.GridAlias = "GMTBooking";
            this.GridNew = 0;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.KeyField1 = "ID";
            this.KeyField2 = "BIRID";
            this.Name = "P11";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P11 .BIR Invoice (PH)";
            this.UnApvChkValue = "Approved";
            this.UniqueExpress = "ID";
            this.WorkAlias = "BIRInvoice";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchApprove, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnimport;
        private Win.UI.TextBox txtInvSerial;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.Txtbrand txtbrand;
        private Win.UI.DisplayBox disExVoucherID;
        private Win.UI.Label lbExVoucherID;
        private Win.UI.Button btnBatchApprove;
    }
}
