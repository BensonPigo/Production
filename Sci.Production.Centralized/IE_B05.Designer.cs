namespace Sci.Production.Centralized
{
    partial class IE_B05
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.checkThreadcons = new Sci.Win.UI.CheckBox();
            this.btnThreadRatio = new Sci.Win.UI.Button();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.numManualAllowance = new Sci.Win.UI.NumericBox();
            this.numMachineAllowance = new Sci.Win.UI.NumericBox();
            this.displayArtworkType = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.labelManualAllowance = new Sci.Win.UI.Label();
            this.labelMachineAllowance = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelCode = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.checkThreadcons);
            this.masterpanel.Controls.Add(this.btnThreadRatio);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.label10);
            this.masterpanel.Controls.Add(this.label9);
            this.masterpanel.Controls.Add(this.numManualAllowance);
            this.masterpanel.Controls.Add(this.numMachineAllowance);
            this.masterpanel.Controls.Add(this.displayArtworkType);
            this.masterpanel.Controls.Add(this.displayDescription);
            this.masterpanel.Controls.Add(this.displayCode);
            this.masterpanel.Controls.Add(this.labelManualAllowance);
            this.masterpanel.Controls.Add(this.labelMachineAllowance);
            this.masterpanel.Controls.Add(this.labelArtworkType);
            this.masterpanel.Controls.Add(this.labelDescription);
            this.masterpanel.Controls.Add(this.labelCode);
            this.masterpanel.Size = new System.Drawing.Size(829, 197);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelCode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelMachineAllowance, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelManualAllowance, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayCode, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayDescription, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.numMachineAllowance, 0);
            this.masterpanel.Controls.SetChildIndex(this.numManualAllowance, 0);
            this.masterpanel.Controls.SetChildIndex(this.label9, 0);
            this.masterpanel.Controls.SetChildIndex(this.label10, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnThreadRatio, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkThreadcons, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 197);
            this.detailpanel.Size = new System.Drawing.Size(829, 221);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(702, 162);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(829, 221);
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
            this.detail.Size = new System.Drawing.Size(829, 456);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(829, 418);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 418);
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 456);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 485);
            // 
            // checkThreadcons
            // 
            this.checkThreadcons.AutoSize = true;
            this.checkThreadcons.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "isthread", true));
            this.checkThreadcons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkThreadcons.IsSupportEditMode = false;
            this.checkThreadcons.Location = new System.Drawing.Point(498, 14);
            this.checkThreadcons.Name = "checkThreadcons";
            this.checkThreadcons.ReadOnly = true;
            this.checkThreadcons.Size = new System.Drawing.Size(173, 21);
            this.checkThreadcons.TabIndex = 47;
            this.checkThreadcons.Text = "Calculate Thread cons.";
            this.checkThreadcons.UseVisualStyleBackColor = true;
            // 
            // btnThreadRatio
            // 
            this.btnThreadRatio.Location = new System.Drawing.Point(691, 10);
            this.btnThreadRatio.Name = "btnThreadRatio";
            this.btnThreadRatio.Size = new System.Drawing.Size(111, 30);
            this.btnThreadRatio.TabIndex = 46;
            this.btnThreadRatio.Text = "Thread Ratio";
            this.btnThreadRatio.UseVisualStyleBackColor = true;
            this.btnThreadRatio.Click += new System.EventHandler(this.BtnThreadRatio_Click);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(435, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 43;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(215, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 45;
            this.label10.Text = "%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(215, 122);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 23);
            this.label9.TabIndex = 44;
            this.label9.Text = "%";
            // 
            // numManualAllowance
            // 
            this.numManualAllowance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numManualAllowance.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ManAllow", true));
            this.numManualAllowance.DecimalPlaces = 2;
            this.numManualAllowance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numManualAllowance.IsSupportEditMode = false;
            this.numManualAllowance.Location = new System.Drawing.Point(162, 157);
            this.numManualAllowance.Name = "numManualAllowance";
            this.numManualAllowance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManualAllowance.ReadOnly = true;
            this.numManualAllowance.Size = new System.Drawing.Size(50, 23);
            this.numManualAllowance.TabIndex = 41;
            this.numManualAllowance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numMachineAllowance
            // 
            this.numMachineAllowance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numMachineAllowance.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MachineAllow", true));
            this.numMachineAllowance.DecimalPlaces = 2;
            this.numMachineAllowance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numMachineAllowance.IsSupportEditMode = false;
            this.numMachineAllowance.Location = new System.Drawing.Point(162, 122);
            this.numMachineAllowance.Name = "numMachineAllowance";
            this.numMachineAllowance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numMachineAllowance.ReadOnly = true;
            this.numMachineAllowance.Size = new System.Drawing.Size(50, 23);
            this.numMachineAllowance.TabIndex = 39;
            this.numMachineAllowance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayArtworkType
            // 
            this.displayArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ArtworkTypeID", true));
            this.displayArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArtworkType.Location = new System.Drawing.Point(162, 87);
            this.displayArtworkType.Name = "displayArtworkType";
            this.displayArtworkType.Size = new System.Drawing.Size(140, 23);
            this.displayArtworkType.TabIndex = 37;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(162, 49);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(380, 23);
            this.displayDescription.TabIndex = 35;
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(162, 14);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(80, 23);
            this.displayCode.TabIndex = 33;
            // 
            // labelManualAllowance
            // 
            this.labelManualAllowance.Location = new System.Drawing.Point(33, 157);
            this.labelManualAllowance.Name = "labelManualAllowance";
            this.labelManualAllowance.Size = new System.Drawing.Size(125, 23);
            this.labelManualAllowance.TabIndex = 42;
            this.labelManualAllowance.Text = "Manual Allowance";
            // 
            // labelMachineAllowance
            // 
            this.labelMachineAllowance.Location = new System.Drawing.Point(33, 122);
            this.labelMachineAllowance.Name = "labelMachineAllowance";
            this.labelMachineAllowance.Size = new System.Drawing.Size(125, 23);
            this.labelMachineAllowance.TabIndex = 40;
            this.labelMachineAllowance.Text = "Machine Allowance";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(33, 87);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(125, 23);
            this.labelArtworkType.TabIndex = 38;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(33, 49);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(125, 23);
            this.labelDescription.TabIndex = 36;
            this.labelDescription.Text = "Description";
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(33, 14);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(125, 23);
            this.labelCode.TabIndex = 34;
            this.labelCode.Text = "ST/MC type";
            // 
            // IE_B05
            // 
            this.ClientSize = new System.Drawing.Size(837, 518);
            this.DefaultControl = "displayCode";
            this.DefaultControlForEdit = "displayCode";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.GridAlias = "MachineType_Detail";
            this.GridUniqueKey = "ID,FactoryID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B05";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "IE_B05. ST/MC type";
            this.UniqueExpress = "ID";
            this.WorkAlias = "MachineType";
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

        private Win.UI.CheckBox checkThreadcons;
        private Win.UI.Button btnThreadRatio;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.NumericBox numManualAllowance;
        private Win.UI.NumericBox numMachineAllowance;
        private Win.UI.DisplayBox displayArtworkType;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label labelManualAllowance;
        private Win.UI.Label labelMachineAllowance;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelCode;
    }
}
