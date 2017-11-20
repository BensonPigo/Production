namespace Sci.Production.Thread
{
    partial class P01
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
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.displayStyleNo = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.btnGenerate = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnGenerate);
            this.masterpanel.Controls.Add(this.displayBrand);
            this.masterpanel.Controls.Add(this.displaySeason);
            this.masterpanel.Controls.Add(this.displayStyleNo);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.labelSeason);
            this.masterpanel.Controls.Add(this.labelStyleNo);
            this.masterpanel.Size = new System.Drawing.Size(914, 55);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyleNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyleNo, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnGenerate, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 55);
            this.detailpanel.Size = new System.Drawing.Size(914, 294);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(5, 20);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(827, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(914, 294);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(914, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(914, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(914, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(914, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(922, 416);
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
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(39, 22);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(75, 23);
            this.labelStyleNo.TabIndex = 1;
            this.labelStyleNo.Text = "Style No";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(303, 22);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 2;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(520, 22);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 23);
            this.labelBrand.TabIndex = 3;
            this.labelBrand.Text = "Brand";
            // 
            // displayStyleNo
            // 
            this.displayStyleNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleNo.Location = new System.Drawing.Point(117, 22);
            this.displayStyleNo.Name = "displayStyleNo";
            this.displayStyleNo.Size = new System.Drawing.Size(141, 23);
            this.displayStyleNo.TabIndex = 4;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "seasonid", true));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(381, 22);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(73, 23);
            this.displaySeason.TabIndex = 5;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "brandid", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(598, 22);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(100, 23);
            this.displayBrand.TabIndex = 6;
            // 
            // btnGenerate
            // 
            this.btnGenerate.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnGenerate.Location = new System.Drawing.Point(790, 15);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(80, 30);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // P01
            // 
            this.ClientSize = new System.Drawing.Size(922, 449);
            this.DefaultDetailOrder = "ThreadCombid,machinetypeid,ConsPC";
            this.DefaultOrder = "ID,Seasonid";
            this.GridAlias = "ThreadColorComb";
            this.GridUniqueKey = "ID";
            this.IsGridIconVisible = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "Ukey";
            this.KeyField2 = "StyleUkey";
            this.Name = "P01";
            this.SubDetailKeyField1 = "id";
            this.SubGridAlias = "ThreadColorcomb_Operation";
            this.SubKeyField1 = "id";
            this.Text = "P01.Thread Color Combination";
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

        private Win.UI.DisplayBox displayBrand;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyleNo;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyleNo;
        private Win.UI.Button btnGenerate;
    }
}
