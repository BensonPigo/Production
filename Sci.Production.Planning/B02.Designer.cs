namespace Sci.Production.Planning
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelBeginStitch = new Sci.Win.UI.Label();
            this.labelEndStitch = new Sci.Win.UI.Label();
            this.labelBatchNumber = new Sci.Win.UI.Label();
            this.numBeginStitch = new Sci.Win.UI.NumericBox();
            this.numEndStitch = new Sci.Win.UI.NumericBox();
            this.numBatchNumber = new Sci.Win.UI.NumericBox();
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
            this.detailcont.Controls.Add(this.numBatchNumber);
            this.detailcont.Controls.Add(this.numEndStitch);
            this.detailcont.Controls.Add(this.numBeginStitch);
            this.detailcont.Controls.Add(this.labelBatchNumber);
            this.detailcont.Controls.Add(this.labelEndStitch);
            this.detailcont.Controls.Add(this.labelBeginStitch);
            this.detailcont.Size = new System.Drawing.Size(834, 357);
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
            // labelBeginStitch
            // 
            this.labelBeginStitch.Lines = 0;
            this.labelBeginStitch.Location = new System.Drawing.Point(23, 20);
            this.labelBeginStitch.Name = "labelBeginStitch";
            this.labelBeginStitch.Size = new System.Drawing.Size(105, 23);
            this.labelBeginStitch.TabIndex = 2;
            this.labelBeginStitch.Text = "Begin Stitch";
            // 
            // labelEndStitch
            // 
            this.labelEndStitch.Lines = 0;
            this.labelEndStitch.Location = new System.Drawing.Point(23, 53);
            this.labelEndStitch.Name = "labelEndStitch";
            this.labelEndStitch.Size = new System.Drawing.Size(105, 23);
            this.labelEndStitch.TabIndex = 3;
            this.labelEndStitch.Text = "End Stitch";
            // 
            // labelBatchNumber
            // 
            this.labelBatchNumber.Lines = 0;
            this.labelBatchNumber.Location = new System.Drawing.Point(23, 86);
            this.labelBatchNumber.Name = "labelBatchNumber";
            this.labelBatchNumber.Size = new System.Drawing.Size(105, 23);
            this.labelBatchNumber.TabIndex = 4;
            this.labelBatchNumber.Text = "Batch Number";
            // 
            // numBeginStitch
            // 
            this.numBeginStitch.BackColor = System.Drawing.Color.White;
            this.numBeginStitch.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BeginStitch", true));
            this.numBeginStitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numBeginStitch.Location = new System.Drawing.Point(131, 20);
            this.numBeginStitch.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numBeginStitch.MaxLength = 7;
            this.numBeginStitch.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBeginStitch.Name = "numBeginStitch";
            this.numBeginStitch.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBeginStitch.Size = new System.Drawing.Size(100, 23);
            this.numBeginStitch.TabIndex = 0;
            this.numBeginStitch.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBeginStitch.Validating += new System.ComponentModel.CancelEventHandler(this.NumBeginStitch_Validating);
            // 
            // numEndStitch
            // 
            this.numEndStitch.BackColor = System.Drawing.Color.White;
            this.numEndStitch.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EndStitch", true));
            this.numEndStitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numEndStitch.Location = new System.Drawing.Point(131, 53);
            this.numEndStitch.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numEndStitch.MaxLength = 7;
            this.numEndStitch.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEndStitch.Name = "numEndStitch";
            this.numEndStitch.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEndStitch.Size = new System.Drawing.Size(100, 23);
            this.numEndStitch.TabIndex = 1;
            this.numEndStitch.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numEndStitch.Validating += new System.ComponentModel.CancelEventHandler(this.NumEndStitch_Validating);
            // 
            // numBatchNumber
            // 
            this.numBatchNumber.BackColor = System.Drawing.Color.White;
            this.numBatchNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BatchNo", true));
            this.numBatchNumber.DecimalPlaces = 1;
            this.numBatchNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numBatchNumber.Location = new System.Drawing.Point(131, 86);
            this.numBatchNumber.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            65536});
            this.numBatchNumber.MaxLength = 4;
            this.numBatchNumber.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.numBatchNumber.Name = "numBatchNumber";
            this.numBatchNumber.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBatchNumber.Size = new System.Drawing.Size(100, 23);
            this.numBatchNumber.TabIndex = 2;
            this.numBatchNumber.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B02
            // 
            this.ClientSize = new System.Drawing.Size(842, 457);
            this.DefaultControl = "numBeginStitch";
            this.DefaultControlForEdit = "numBeginStitch";
            this.IsSupportClip = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.Name = "B02";
            this.Text = "B02. Embroidery\'s number of Setting";
            this.WorkAlias = "EmbBatch";
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

        private Win.UI.Label labelBatchNumber;
        private Win.UI.Label labelEndStitch;
        private Win.UI.Label labelBeginStitch;
        private Win.UI.NumericBox numBatchNumber;
        private Win.UI.NumericBox numEndStitch;
        private Win.UI.NumericBox numBeginStitch;
    }
}
