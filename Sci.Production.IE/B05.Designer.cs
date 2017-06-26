﻿namespace Sci.Production.IE
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelGroupID = new Sci.Win.UI.Label();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.labelMachineAllowance = new Sci.Win.UI.Label();
            this.labelManualAllowance = new Sci.Win.UI.Label();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.displayGroupID = new Sci.Win.UI.DisplayBox();
            this.displayArtworkType = new Sci.Win.UI.DisplayBox();
            this.numMachineAllowance = new Sci.Win.UI.NumericBox();
            this.numManualAllowance = new Sci.Win.UI.NumericBox();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.btnThreadRatio = new Sci.Win.UI.Button();
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
            this.detail.Size = new System.Drawing.Size(829, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.btnThreadRatio);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.numManualAllowance);
            this.detailcont.Controls.Add(this.numMachineAllowance);
            this.detailcont.Controls.Add(this.displayArtworkType);
            this.detailcont.Controls.Add(this.displayGroupID);
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.labelManualAllowance);
            this.detailcont.Controls.Add(this.labelMachineAllowance);
            this.detailcont.Controls.Add(this.labelArtworkType);
            this.detailcont.Controls.Add(this.labelGroupID);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(829, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(30, 30);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(125, 23);
            this.labelCode.TabIndex = 0;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(30, 65);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(125, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // labelGroupID
            // 
            this.labelGroupID.Location = new System.Drawing.Point(30, 100);
            this.labelGroupID.Name = "labelGroupID";
            this.labelGroupID.Size = new System.Drawing.Size(125, 23);
            this.labelGroupID.TabIndex = 2;
            this.labelGroupID.Text = "Group ID";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(30, 135);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(125, 23);
            this.labelArtworkType.TabIndex = 3;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // labelMachineAllowance
            // 
            this.labelMachineAllowance.Location = new System.Drawing.Point(30, 170);
            this.labelMachineAllowance.Name = "labelMachineAllowance";
            this.labelMachineAllowance.Size = new System.Drawing.Size(125, 23);
            this.labelMachineAllowance.TabIndex = 4;
            this.labelMachineAllowance.Text = "Machine Allowance";
            // 
            // labelManualAllowance
            // 
            this.labelManualAllowance.Location = new System.Drawing.Point(30, 205);
            this.labelManualAllowance.Name = "labelManualAllowance";
            this.labelManualAllowance.Size = new System.Drawing.Size(125, 23);
            this.labelManualAllowance.TabIndex = 5;
            this.labelManualAllowance.Text = "Manual Allowance";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(159, 30);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(80, 23);
            this.displayCode.TabIndex = 0;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(159, 65);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(380, 23);
            this.displayDescription.TabIndex = 1;
            // 
            // displayGroupID
            // 
            this.displayGroupID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayGroupID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MachineGroupID", true));
            this.displayGroupID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayGroupID.Location = new System.Drawing.Point(159, 100);
            this.displayGroupID.Name = "displayGroupID";
            this.displayGroupID.Size = new System.Drawing.Size(35, 23);
            this.displayGroupID.TabIndex = 2;
            // 
            // displayArtworkType
            // 
            this.displayArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ArtworkTypeID", true));
            this.displayArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArtworkType.Location = new System.Drawing.Point(159, 135);
            this.displayArtworkType.Name = "displayArtworkType";
            this.displayArtworkType.Size = new System.Drawing.Size(140, 23);
            this.displayArtworkType.TabIndex = 3;
            // 
            // numMachineAllowance
            // 
            this.numMachineAllowance.BackColor = System.Drawing.Color.White;
            this.numMachineAllowance.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MachineAllow", true));
            this.numMachineAllowance.DecimalPlaces = 2;
            this.numMachineAllowance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numMachineAllowance.Location = new System.Drawing.Point(159, 170);
            this.numMachineAllowance.Name = "numMachineAllowance";
            this.numMachineAllowance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numMachineAllowance.Size = new System.Drawing.Size(50, 23);
            this.numMachineAllowance.TabIndex = 4;
            this.numMachineAllowance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numManualAllowance
            // 
            this.numManualAllowance.BackColor = System.Drawing.Color.White;
            this.numManualAllowance.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ManAllow", true));
            this.numManualAllowance.DecimalPlaces = 2;
            this.numManualAllowance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numManualAllowance.Location = new System.Drawing.Point(159, 205);
            this.numManualAllowance.Name = "numManualAllowance";
            this.numManualAllowance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManualAllowance.Size = new System.Drawing.Size(50, 23);
            this.numManualAllowance.TabIndex = 5;
            this.numManualAllowance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(212, 170);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 23);
            this.label9.TabIndex = 12;
            this.label9.Text = "%";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(212, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 13;
            this.label10.Text = "%";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(432, 30);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 6;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // btnThreadRatio
            // 
            this.btnThreadRatio.Location = new System.Drawing.Point(688, 26);
            this.btnThreadRatio.Name = "btnThreadRatio";
            this.btnThreadRatio.Size = new System.Drawing.Size(111, 30);
            this.btnThreadRatio.TabIndex = 15;
            this.btnThreadRatio.Text = "Thread Ratio";
            this.btnThreadRatio.UseVisualStyleBackColor = true;
            this.btnThreadRatio.Click += new System.EventHandler(this.btnThreadRatio_Click);
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.DefaultControl = "displayCode";
            this.DefaultControlForEdit = "displayCode";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.Text = "B05. Machine Type";
            this.UniqueExpress = "ID";
            this.WorkAlias = "MachineType";
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

        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.NumericBox numManualAllowance;
        private Win.UI.NumericBox numMachineAllowance;
        private Win.UI.DisplayBox displayArtworkType;
        private Win.UI.DisplayBox displayGroupID;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label labelManualAllowance;
        private Win.UI.Label labelMachineAllowance;
        private Win.UI.Label labelArtworkType;
        private Win.UI.Label labelGroupID;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelCode;
        private Win.UI.Button btnThreadRatio;
    }
}
