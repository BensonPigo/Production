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
            this.components = new System.ComponentModel.Container();
            this.panel3 = new Sci.Win.UI.Panel();
            this.gridIcon1 = new Sci.Win.UI.GridIcon();
            this.btnThreadRatio = new Sci.Win.UI.Button();
            this.checkThreadcons = new Sci.Win.UI.CheckBox();
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
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(831, 505);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.gridDetail);
            this.detailcont.Controls.Add(this.panel3);
            this.detailcont.Size = new System.Drawing.Size(831, 467);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 467);
            this.detailbtm.Size = new System.Drawing.Size(831, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(831, 505);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(839, 534);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gridIcon1);
            this.panel3.Controls.Add(this.btnThreadRatio);
            this.panel3.Controls.Add(this.checkThreadcons);
            this.panel3.Controls.Add(this.checkJunk);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.numManualAllowance);
            this.panel3.Controls.Add(this.numMachineAllowance);
            this.panel3.Controls.Add(this.displayArtworkType);
            this.panel3.Controls.Add(this.displayDescription);
            this.panel3.Controls.Add(this.displayCode);
            this.panel3.Controls.Add(this.labelManualAllowance);
            this.panel3.Controls.Add(this.labelMachineAllowance);
            this.panel3.Controls.Add(this.labelArtworkType);
            this.panel3.Controls.Add(this.labelDescription);
            this.panel3.Controls.Add(this.labelCode);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(831, 190);
            this.panel3.TabIndex = 4;
            // 
            // gridIcon1
            // 
            this.gridIcon1.Location = new System.Drawing.Point(708, 152);
            this.gridIcon1.Name = "gridIcon1";
            this.gridIcon1.Size = new System.Drawing.Size(100, 32);
            this.gridIcon1.TabIndex = 63;
            this.gridIcon1.Text = "gridIcon1";
            this.gridIcon1.AppendClick += new System.EventHandler(this.GridIcon1_AppendClick);
            this.gridIcon1.RemoveClick += new System.EventHandler(this.GridIcon1_RemoveClick);
            // 
            // btnThreadRatio
            // 
            this.btnThreadRatio.Location = new System.Drawing.Point(642, 10);
            this.btnThreadRatio.Name = "btnThreadRatio";
            this.btnThreadRatio.Size = new System.Drawing.Size(111, 30);
            this.btnThreadRatio.TabIndex = 62;
            this.btnThreadRatio.Text = "Thread Ratio";
            this.btnThreadRatio.UseVisualStyleBackColor = true;
            this.btnThreadRatio.Click += new System.EventHandler(this.BtnThreadRatio_Click);
            // 
            // checkThreadcons
            // 
            this.checkThreadcons.AutoSize = true;
            this.checkThreadcons.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "isthread", true));
            this.checkThreadcons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkThreadcons.IsSupportEditMode = false;
            this.checkThreadcons.Location = new System.Drawing.Point(452, 10);
            this.checkThreadcons.Name = "checkThreadcons";
            this.checkThreadcons.ReadOnly = true;
            this.checkThreadcons.Size = new System.Drawing.Size(173, 21);
            this.checkThreadcons.TabIndex = 61;
            this.checkThreadcons.Text = "Calculate Thread cons.";
            this.checkThreadcons.UseVisualStyleBackColor = true;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(389, 10);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 58;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(215, 153);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 60;
            this.label10.Text = "%";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(215, 118);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 23);
            this.label9.TabIndex = 59;
            this.label9.Text = "%";
            // 
            // numManualAllowance
            // 
            this.numManualAllowance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numManualAllowance.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ManAllow", true));
            this.numManualAllowance.DecimalPlaces = 2;
            this.numManualAllowance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numManualAllowance.IsSupportEditMode = false;
            this.numManualAllowance.Location = new System.Drawing.Point(162, 153);
            this.numManualAllowance.Name = "numManualAllowance";
            this.numManualAllowance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numManualAllowance.ReadOnly = true;
            this.numManualAllowance.Size = new System.Drawing.Size(50, 23);
            this.numManualAllowance.TabIndex = 56;
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
            this.numMachineAllowance.Location = new System.Drawing.Point(162, 118);
            this.numMachineAllowance.Name = "numMachineAllowance";
            this.numMachineAllowance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numMachineAllowance.ReadOnly = true;
            this.numMachineAllowance.Size = new System.Drawing.Size(50, 23);
            this.numMachineAllowance.TabIndex = 54;
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
            this.displayArtworkType.Location = new System.Drawing.Point(162, 83);
            this.displayArtworkType.Name = "displayArtworkType";
            this.displayArtworkType.Size = new System.Drawing.Size(140, 23);
            this.displayArtworkType.TabIndex = 52;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(162, 45);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(380, 23);
            this.displayDescription.TabIndex = 50;
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(162, 10);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(80, 23);
            this.displayCode.TabIndex = 48;
            // 
            // labelManualAllowance
            // 
            this.labelManualAllowance.Location = new System.Drawing.Point(33, 153);
            this.labelManualAllowance.Name = "labelManualAllowance";
            this.labelManualAllowance.Size = new System.Drawing.Size(125, 23);
            this.labelManualAllowance.TabIndex = 57;
            this.labelManualAllowance.Text = "Manual Allowance";
            // 
            // labelMachineAllowance
            // 
            this.labelMachineAllowance.Location = new System.Drawing.Point(33, 118);
            this.labelMachineAllowance.Name = "labelMachineAllowance";
            this.labelMachineAllowance.Size = new System.Drawing.Size(125, 23);
            this.labelMachineAllowance.TabIndex = 55;
            this.labelMachineAllowance.Text = "Machine Allowance";
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(33, 83);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(125, 23);
            this.labelArtworkType.TabIndex = 53;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(33, 45);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(125, 23);
            this.labelDescription.TabIndex = 51;
            this.labelDescription.Text = "Description";
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(33, 10);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(125, 23);
            this.labelCode.TabIndex = 49;
            this.labelCode.Text = "ST/MC type";
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 190);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(831, 277);
            this.gridDetail.TabIndex = 5;
            this.gridDetail.TabStop = false;
            // 
            // IE_B05
            // 
            this.ClientSize = new System.Drawing.Size(839, 567);
            this.ConnectionName = "Trade";
            this.DefaultControl = "displayCode";
            this.DefaultControlForEdit = "displayCode";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B05";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "IE_B05. ST/MC type";
            this.WorkAlias = "MachineType";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Grid gridDetail;
        private Win.UI.CheckBox checkThreadcons;
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
        private Win.UI.Button btnThreadRatio;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.GridIcon gridIcon1;
    }
}
