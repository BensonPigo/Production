namespace Sci.Production.SubProcess
{
    partial class B02
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
            this.labelStyleName = new Sci.Win.UI.Label();
            this.displayStyleName = new Sci.Win.UI.DisplayBox();
            this.labelSeason = new Sci.Win.UI.Label();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.labelStyle = new Sci.Win.UI.Label();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
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
            this.masterpanel.Controls.Add(this.labelStyleName);
            this.masterpanel.Controls.Add(this.displayStyleName);
            this.masterpanel.Controls.Add(this.labelSeason);
            this.masterpanel.Controls.Add(this.displaySeason);
            this.masterpanel.Controls.Add(this.labelStyle);
            this.masterpanel.Controls.Add(this.displayStyle);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.displayBrand);
            this.masterpanel.Size = new System.Drawing.Size(892, 73);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyleName, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyleName, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 73);
            this.detailpanel.Size = new System.Drawing.Size(892, 276);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(784, 41);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 276);
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
            this.detail.Size = new System.Drawing.Size(892, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(892, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(892, 38);
            // 
            // labelStyleName
            // 
            this.labelStyleName.Location = new System.Drawing.Point(14, 39);
            this.labelStyleName.Name = "labelStyleName";
            this.labelStyleName.Size = new System.Drawing.Size(75, 23);
            this.labelStyleName.TabIndex = 17;
            this.labelStyleName.Text = "Style Name";
            // 
            // displayStyleName
            // 
            this.displayStyleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "stylename", true));
            this.displayStyleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleName.Location = new System.Drawing.Point(92, 39);
            this.displayStyleName.Name = "displayStyleName";
            this.displayStyleName.Size = new System.Drawing.Size(353, 23);
            this.displayStyleName.TabIndex = 11;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(487, 10);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 16;
            this.labelSeason.Text = "Season";
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "seasonid", true));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(565, 10);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(100, 23);
            this.displaySeason.TabIndex = 13;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(232, 10);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 15;
            this.labelStyle.Text = "Style";
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(310, 10);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(135, 23);
            this.displayStyle.TabIndex = 12;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(14, 10);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 23);
            this.labelBrand.TabIndex = 14;
            this.labelBrand.Text = "Brand";
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "brandid", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(92, 10);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(100, 23);
            this.displayBrand.TabIndex = 10;
            // 
            // B02
            // 
            this.ClientSize = new System.Drawing.Size(900, 449);
            this.GridAlias = "Style_Feature";
            this.GridUniqueKey = "styleukey,Type,Feature";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "Ukey";
            this.KeyField2 = "StyleUkey";
            this.Name = "B02";
            this.Text = "B02.Style Feature Data";
            this.WorkAlias = "Style";
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

        private Win.UI.Label labelStyleName;
        private Win.UI.DisplayBox displayStyleName;
        private Win.UI.Label labelSeason;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.Label labelStyle;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.Label labelBrand;
        private Win.UI.DisplayBox displayBrand;
    }
}
