namespace Sci.Production.Basic
{
    partial class B12
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
            this.labelUnit = new Sci.Win.UI.Label();
            this.labelPricerate = new Sci.Win.UI.Label();
            this.labelRound = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelExtendUnit = new Sci.Win.UI.Label();
            this.displayUnit = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.displayExtendUnit = new Sci.Win.UI.DisplayBox();
            this.numPriceRate = new Sci.Win.UI.NumericBox();
            this.numRound = new Sci.Win.UI.NumericBox();
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
            this.masterpanel.Controls.Add(this.numRound);
            this.masterpanel.Controls.Add(this.numPriceRate);
            this.masterpanel.Controls.Add(this.displayExtendUnit);
            this.masterpanel.Controls.Add(this.displayDescription);
            this.masterpanel.Controls.Add(this.displayUnit);
            this.masterpanel.Controls.Add(this.labelExtendUnit);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.labelRound);
            this.masterpanel.Controls.Add(this.labelPricerate);
            this.masterpanel.Controls.Add(this.labelUnit);
            this.masterpanel.Size = new System.Drawing.Size(916, 134);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelUnit, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelPricerate, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRound, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelExtendUnit, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayUnit, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayExtendUnit, 0);
            this.masterpanel.Controls.SetChildIndex(this.numPriceRate, 0);
            this.masterpanel.Controls.SetChildIndex(this.numRound, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 134);
            this.detailpanel.Size = new System.Drawing.Size(916, 215);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(653, 95);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(828, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(916, 215);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(916, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(910, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(910, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(916, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(916, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(916, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(916, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(924, 416);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(473, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(425, 13);
            // 
            // labelUnit
            // 
            this.labelUnit.Lines = 0;
            this.labelUnit.Location = new System.Drawing.Point(24, 14);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(75, 23);
            this.labelUnit.TabIndex = 1;
            this.labelUnit.Text = "Unit";
            // 
            // labelPricerate
            // 
            this.labelPricerate.Lines = 0;
            this.labelPricerate.Location = new System.Drawing.Point(24, 44);
            this.labelPricerate.Name = "labelPricerate";
            this.labelPricerate.Size = new System.Drawing.Size(75, 23);
            this.labelPricerate.TabIndex = 2;
            this.labelPricerate.Text = "Price rate";
            // 
            // labelRound
            // 
            this.labelRound.Lines = 0;
            this.labelRound.Location = new System.Drawing.Point(24, 74);
            this.labelRound.Name = "labelRound";
            this.labelRound.Size = new System.Drawing.Size(75, 23);
            this.labelRound.TabIndex = 3;
            this.labelRound.Text = "Round";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(24, 104);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(413, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 5;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelExtendUnit
            // 
            this.labelExtendUnit.Lines = 0;
            this.labelExtendUnit.Location = new System.Drawing.Point(413, 44);
            this.labelExtendUnit.Name = "labelExtendUnit";
            this.labelExtendUnit.Size = new System.Drawing.Size(80, 23);
            this.labelExtendUnit.TabIndex = 6;
            this.labelExtendUnit.Text = "Extend Unit";
            // 
            // displayUnit
            // 
            this.displayUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnit.Location = new System.Drawing.Point(103, 14);
            this.displayUnit.Name = "displayUnit";
            this.displayUnit.Size = new System.Drawing.Size(70, 23);
            this.displayUnit.TabIndex = 7;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(103, 104);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(316, 23);
            this.displayDescription.TabIndex = 10;
            // 
            // displayExtendUnit
            // 
            this.displayExtendUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayExtendUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ExtensionUnit", true));
            this.displayExtendUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayExtendUnit.Location = new System.Drawing.Point(498, 44);
            this.displayExtendUnit.Name = "displayExtendUnit";
            this.displayExtendUnit.Size = new System.Drawing.Size(70, 23);
            this.displayExtendUnit.TabIndex = 11;
            // 
            // numPriceRate
            // 
            this.numPriceRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numPriceRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PriceRate", true));
            this.numPriceRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numPriceRate.IsSupportEditMode = false;
            this.numPriceRate.Location = new System.Drawing.Point(103, 44);
            this.numPriceRate.Name = "numPriceRate";
            this.numPriceRate.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPriceRate.ReadOnly = true;
            this.numPriceRate.Size = new System.Drawing.Size(50, 23);
            this.numPriceRate.TabIndex = 12;
            this.numPriceRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numRound
            // 
            this.numRound.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numRound.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Round", true));
            this.numRound.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numRound.IsSupportEditMode = false;
            this.numRound.Location = new System.Drawing.Point(103, 74);
            this.numRound.Name = "numRound";
            this.numRound.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numRound.ReadOnly = true;
            this.numRound.Size = new System.Drawing.Size(30, 23);
            this.numRound.TabIndex = 13;
            this.numRound.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B12
            // 
            this.ClientSize = new System.Drawing.Size(924, 449);
            this.DefaultDetailOrder = "UnitTo";
            this.DefaultOrder = "ID";
            this.GridAlias = "Unit_Rate";
            this.IsGridIconVisible = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.KeyField2 = "UnitFrom";
            this.Name = "B12";
            this.Text = "B12. Unit";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Unit";
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

        private Win.UI.DisplayBox displayExtendUnit;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayUnit;
        private Win.UI.Label labelExtendUnit;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelRound;
        private Win.UI.Label labelPricerate;
        private Win.UI.Label labelUnit;
        private Win.UI.NumericBox numRound;
        private Win.UI.NumericBox numPriceRate;
    }
}
