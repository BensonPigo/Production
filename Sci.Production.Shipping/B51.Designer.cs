namespace Sci.Production.Shipping
{
    partial class B51
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
            this.labelStartDate = new Sci.Win.UI.Label();
            this.labelEndDate = new Sci.Win.UI.Label();
            this.labelCDCNo = new Sci.Win.UI.Label();
            this.dateStartDate = new Sci.Win.UI.DateBox();
            this.dateEndDate = new Sci.Win.UI.DateBox();
            this.txtCDCNo = new Sci.Win.UI.TextBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Win.UI.TextBox();
            this.btnImportData = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnImportData);
            this.masterpanel.Controls.Add(this.txtFactory);
            this.masterpanel.Controls.Add(this.labelFactory);
            this.masterpanel.Controls.Add(this.txtCDCNo);
            this.masterpanel.Controls.Add(this.dateEndDate);
            this.masterpanel.Controls.Add(this.dateStartDate);
            this.masterpanel.Controls.Add(this.labelCDCNo);
            this.masterpanel.Controls.Add(this.labelEndDate);
            this.masterpanel.Controls.Add(this.labelStartDate);
            this.masterpanel.Size = new System.Drawing.Size(907, 96);
            this.masterpanel.Controls.SetChildIndex(this.labelStartDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelEndDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCDCNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateStartDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateEndDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCDCNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImportData, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 96);
            this.detailpanel.Size = new System.Drawing.Size(907, 253);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(651, 61);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(819, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(907, 253);
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
            this.detail.Size = new System.Drawing.Size(907, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(907, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(907, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(907, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(915, 416);
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
            // labelStartDate
            // 
            this.labelStartDate.Location = new System.Drawing.Point(14, 10);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(70, 23);
            this.labelStartDate.TabIndex = 1;
            this.labelStartDate.Text = "Start Date";
            // 
            // labelEndDate
            // 
            this.labelEndDate.Location = new System.Drawing.Point(14, 37);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(70, 23);
            this.labelEndDate.TabIndex = 2;
            this.labelEndDate.Text = "End Date";
            // 
            // labelCDCNo
            // 
            this.labelCDCNo.Location = new System.Drawing.Point(14, 64);
            this.labelCDCNo.Name = "labelCDCNo";
            this.labelCDCNo.Size = new System.Drawing.Size(70, 23);
            this.labelCDCNo.TabIndex = 3;
            this.labelCDCNo.Text = "CDC No.";
            // 
            // dateStartDate
            // 
            this.dateStartDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StartDate", true));
            this.dateStartDate.Location = new System.Drawing.Point(87, 10);
            this.dateStartDate.Name = "dateStartDate";
            this.dateStartDate.Size = new System.Drawing.Size(100, 23);
            this.dateStartDate.TabIndex = 4;
            // 
            // dateEndDate
            // 
            this.dateEndDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EndDate", true));
            this.dateEndDate.Location = new System.Drawing.Point(87, 37);
            this.dateEndDate.Name = "dateEndDate";
            this.dateEndDate.Size = new System.Drawing.Size(100, 23);
            this.dateEndDate.TabIndex = 5;
            // 
            // txtCDCNo
            // 
            this.txtCDCNo.BackColor = System.Drawing.Color.White;
            this.txtCDCNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCDCNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCNo.Location = new System.Drawing.Point(87, 64);
            this.txtCDCNo.Name = "txtCDCNo";
            this.txtCDCNo.Size = new System.Drawing.Size(130, 23);
            this.txtCDCNo.TabIndex = 6;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(352, 10);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(54, 23);
            this.labelFactory.TabIndex = 7;
            this.labelFactory.Text = "Factory";
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(410, 10);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(70, 23);
            this.txtFactory.TabIndex = 8;
            // 
            // btnImportData
            // 
            this.btnImportData.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImportData.Location = new System.Drawing.Point(633, 55);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(108, 30);
            this.btnImportData.TabIndex = 9;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.BtnImportData_Click);
            // 
            // B51
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(915, 449);
            this.GridAlias = "KHContract_Detail";
            this.GridEdit = false;
            this.GridNew = 0;
            this.GridUniqueKey = "ID,NLCode";
            this.IsGridIconVisible = false;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "B51";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "B51. CDC";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "KHContract";
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

        private Win.UI.TextBox txtFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.TextBox txtCDCNo;
        private Win.UI.DateBox dateEndDate;
        private Win.UI.DateBox dateStartDate;
        private Win.UI.Label labelCDCNo;
        private Win.UI.Label labelEndDate;
        private Win.UI.Label labelStartDate;
        private Win.UI.Button btnImportData;
    }
}
