namespace Sci.Production.Shipping
{
    partial class B05
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.displayEditDate = new Sci.Win.UI.DisplayBox();
            this.labelEditDate = new Sci.Win.UI.Label();
            this.txtuserShipLeader = new Sci.Production.Class.Txtuser();
            this.label1 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtuserShipLeader);
            this.detailcont.Controls.Add(this.displayEditDate);
            this.detailcont.Controls.Add(this.labelEditDate);
            this.detailcont.Controls.Add(this.displayBrand);
            this.detailcont.Controls.Add(this.labelBrand);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(723, 344);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(731, 373);
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(160, 27);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(200, 23);
            this.displayBrand.TabIndex = 8;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(42, 27);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(115, 23);
            this.labelBrand.TabIndex = 7;
            this.labelBrand.Text = "Brand";
            // 
            // displayEditDate
            // 
            this.displayEditDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayEditDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipLeaderEditDate", true));
            this.displayEditDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayEditDate.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.displayEditDate.Location = new System.Drawing.Point(160, 116);
            this.displayEditDate.Name = "displayEditDate";
            this.displayEditDate.Size = new System.Drawing.Size(200, 23);
            this.displayEditDate.TabIndex = 10;
            // 
            // labelEditDate
            // 
            this.labelEditDate.Location = new System.Drawing.Point(42, 116);
            this.labelEditDate.Name = "labelEditDate";
            this.labelEditDate.Size = new System.Drawing.Size(115, 23);
            this.labelEditDate.TabIndex = 9;
            this.labelEditDate.Text = "Edit Date";
            // 
            // txtuserShipLeader
            // 
            this.txtuserShipLeader.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ShipLeader", true));
            this.txtuserShipLeader.DisplayBox1Binding = "";
            this.txtuserShipLeader.Location = new System.Drawing.Point(160, 71);
            this.txtuserShipLeader.Name = "txtuserShipLeader";
            this.txtuserShipLeader.Size = new System.Drawing.Size(300, 23);
            this.txtuserShipLeader.TabIndex = 11;
            this.txtuserShipLeader.TextBox1Binding = "";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(42, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Shipping leader";
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(731, 406);
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.Text = "B05. Shipping Leader";
            this.WorkAlias = "Brand";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayBrand;
        private Win.UI.Label labelBrand;
        private Win.UI.Label label1;
        private Class.Txtuser txtuserShipLeader;
        private Win.UI.DisplayBox displayEditDate;
        private Win.UI.Label labelEditDate;
    }
}
