namespace Sci.Production.PPIC
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelCPU = new Sci.Win.UI.Label();
            this.labelSMV = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.numCPU = new Sci.Win.UI.NumericBox();
            this.numSMV = new Sci.Win.UI.NumericBox();
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
            this.detail.Size = new System.Drawing.Size(834, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.numSMV);
            this.detailcont.Controls.Add(this.numCPU);
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.labelSMV);
            this.detailcont.Controls.Add(this.labelCPU);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(834, 357);
            this.detailcont.TabIndex = 1;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(834, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(834, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(842, 424);
            this.tabs.TabIndex = 0;
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
            // labelID
            // 
            this.labelID.Lines = 0;
            this.labelID.Location = new System.Drawing.Point(31, 40);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 4;
            this.labelID.Text = "ID";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(31, 80);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // labelCPU
            // 
            this.labelCPU.Lines = 0;
            this.labelCPU.Location = new System.Drawing.Point(31, 120);
            this.labelCPU.Name = "labelCPU";
            this.labelCPU.Size = new System.Drawing.Size(75, 23);
            this.labelCPU.TabIndex = 6;
            this.labelCPU.Text = "CPU";
            // 
            // labelSMV
            // 
            this.labelSMV.Lines = 0;
            this.labelSMV.Location = new System.Drawing.Point(31, 160);
            this.labelSMV.Name = "labelSMV";
            this.labelSMV.Size = new System.Drawing.Size(75, 23);
            this.labelSMV.TabIndex = 7;
            this.labelSMV.Text = "SMV";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(110, 40);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(110, 23);
            this.displayID.TabIndex = 0;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(110, 80);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(380, 23);
            this.displayDescription.TabIndex = 1;
            // 
            // numCPU
            // 
            this.numCPU.BackColor = System.Drawing.Color.White;
            this.numCPU.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CPU", true));
            this.numCPU.DecimalPlaces = 3;
            this.numCPU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numCPU.Location = new System.Drawing.Point(110, 120);
            this.numCPU.Name = "numCPU";
            this.numCPU.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCPU.Size = new System.Drawing.Size(60, 23);
            this.numCPU.TabIndex = 2;
            this.numCPU.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numSMV
            // 
            this.numSMV.BackColor = System.Drawing.Color.White;
            this.numSMV.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SMV", true));
            this.numSMV.DecimalPlaces = 4;
            this.numSMV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSMV.Location = new System.Drawing.Point(110, 160);
            this.numSMV.Name = "numSMV";
            this.numSMV.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSMV.Size = new System.Drawing.Size(60, 23);
            this.numSMV.TabIndex = 3;
            this.numSMV.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(842, 457);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.Text = "B05. Mockup";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Mockup";
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

        private Win.UI.Label labelSMV;
        private Win.UI.Label labelCPU;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelID;
        private Win.UI.NumericBox numSMV;
        private Win.UI.NumericBox numCPU;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayID;
    }
}
