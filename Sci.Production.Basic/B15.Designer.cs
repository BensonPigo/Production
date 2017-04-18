namespace Sci.Production.Basic
{
    partial class B15
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(836, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(836, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(836, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(836, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(844, 424);
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
            // labelID
            // 
            this.labelID.Lines = 0;
            this.labelID.Location = new System.Drawing.Point(70, 25);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(70, 63);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(149, 25);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(50, 23);
            this.displayID.TabIndex = 2;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(149, 63);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(300, 23);
            this.displayDescription.TabIndex = 3;
            // 
            // B15
            // 
            this.ClientSize = new System.Drawing.Size(844, 457);
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B15";
            this.Text = "B15. Shipment Term";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ShipTerm";
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

        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelDescription;
    }
}
