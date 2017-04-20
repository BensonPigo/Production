namespace Sci.Production.Shipping
{
    partial class P06_ShipQtyDetail
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
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelseq = new Sci.Win.UI.Label();
            this.labelPackingNo = new Sci.Win.UI.Label();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.displaySEQ = new Sci.Win.UI.DisplayBox();
            this.displayPackingNo = new Sci.Win.UI.DisplayBox();
            this.displayShipMode = new Sci.Win.UI.DisplayBox();
            this.labelStatus = new Sci.Win.UI.Label();
            this.labelOrderQty = new Sci.Win.UI.Label();
            this.labelOrderQtybySeq = new Sci.Win.UI.Label();
            this.labelShipQty = new Sci.Win.UI.Label();
            this.displayStatus = new Sci.Win.UI.DisplayBox();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
            this.numOrderQtybySeq = new Sci.Win.UI.NumericBox();
            this.numShipQty = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 115);
            this.gridcont.Size = new System.Drawing.Size(511, 230);
            // 
            // btmcont
            // 
            this.btmcont.Size = new System.Drawing.Size(535, 40);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(365, 5);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(445, 5);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(310, 5);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(255, 5);
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(10, 5);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(70, 23);
            this.labelSPNo.TabIndex = 99;
            this.labelSPNo.Text = "SP No.";
            // 
            // labelseq
            // 
            this.labelseq.Lines = 0;
            this.labelseq.Location = new System.Drawing.Point(10, 32);
            this.labelseq.Name = "labelseq";
            this.labelseq.Size = new System.Drawing.Size(70, 23);
            this.labelseq.TabIndex = 100;
            this.labelseq.Text = "Seq";
            // 
            // labelPackingNo
            // 
            this.labelPackingNo.Lines = 0;
            this.labelPackingNo.Location = new System.Drawing.Point(10, 59);
            this.labelPackingNo.Name = "labelPackingNo";
            this.labelPackingNo.Size = new System.Drawing.Size(70, 23);
            this.labelPackingNo.TabIndex = 101;
            this.labelPackingNo.Text = "Packing#";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Lines = 0;
            this.labelShipMode.Location = new System.Drawing.Point(10, 86);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(70, 23);
            this.labelShipMode.TabIndex = 102;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(84, 5);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(130, 23);
            this.displaySPNo.TabIndex = 103;
            // 
            // displaySEQ
            // 
            this.displaySEQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySEQ.Location = new System.Drawing.Point(84, 32);
            this.displaySEQ.Name = "displaySEQ";
            this.displaySEQ.Size = new System.Drawing.Size(45, 23);
            this.displaySEQ.TabIndex = 104;
            // 
            // displayPackingNo
            // 
            this.displayPackingNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPackingNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPackingNo.Location = new System.Drawing.Point(84, 59);
            this.displayPackingNo.Name = "displayPackingNo";
            this.displayPackingNo.Size = new System.Drawing.Size(130, 23);
            this.displayPackingNo.TabIndex = 105;
            // 
            // displayShipMode
            // 
            this.displayShipMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipMode.Location = new System.Drawing.Point(84, 86);
            this.displayShipMode.Name = "displayShipMode";
            this.displayShipMode.Size = new System.Drawing.Size(100, 23);
            this.displayShipMode.TabIndex = 106;
            // 
            // labelStatus
            // 
            this.labelStatus.Lines = 0;
            this.labelStatus.Location = new System.Drawing.Point(320, 5);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(115, 23);
            this.labelStatus.TabIndex = 107;
            this.labelStatus.Text = "Status";
            // 
            // labelOrderQty
            // 
            this.labelOrderQty.Lines = 0;
            this.labelOrderQty.Location = new System.Drawing.Point(320, 32);
            this.labelOrderQty.Name = "labelOrderQty";
            this.labelOrderQty.Size = new System.Drawing.Size(115, 23);
            this.labelOrderQty.TabIndex = 108;
            this.labelOrderQty.Text = "Order Q\'ty";
            // 
            // labelOrderQtybySeq
            // 
            this.labelOrderQtybySeq.Lines = 0;
            this.labelOrderQtybySeq.Location = new System.Drawing.Point(320, 59);
            this.labelOrderQtybySeq.Name = "labelOrderQtybySeq";
            this.labelOrderQtybySeq.Size = new System.Drawing.Size(115, 23);
            this.labelOrderQtybySeq.TabIndex = 109;
            this.labelOrderQtybySeq.Text = "Order Q\'ty by Seq";
            // 
            // labelShipQty
            // 
            this.labelShipQty.Lines = 0;
            this.labelShipQty.Location = new System.Drawing.Point(320, 86);
            this.labelShipQty.Name = "labelShipQty";
            this.labelShipQty.Size = new System.Drawing.Size(115, 23);
            this.labelShipQty.TabIndex = 110;
            this.labelShipQty.Text = "Ship Q\'ty";
            // 
            // displayStatus
            // 
            this.displayStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStatus.Location = new System.Drawing.Point(440, 5);
            this.displayStatus.Name = "displayStatus";
            this.displayStatus.Size = new System.Drawing.Size(71, 23);
            this.displayStatus.TabIndex = 111;
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(440, 32);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(71, 23);
            this.numOrderQty.TabIndex = 112;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numOrderQtybySeq
            // 
            this.numOrderQtybySeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQtybySeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQtybySeq.IsSupportEditMode = false;
            this.numOrderQtybySeq.Location = new System.Drawing.Point(440, 59);
            this.numOrderQtybySeq.Name = "numOrderQtybySeq";
            this.numOrderQtybySeq.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQtybySeq.ReadOnly = true;
            this.numOrderQtybySeq.Size = new System.Drawing.Size(71, 23);
            this.numOrderQtybySeq.TabIndex = 113;
            this.numOrderQtybySeq.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numShipQty
            // 
            this.numShipQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numShipQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numShipQty.IsSupportEditMode = false;
            this.numShipQty.Location = new System.Drawing.Point(440, 86);
            this.numShipQty.Name = "numShipQty";
            this.numShipQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numShipQty.ReadOnly = true;
            this.numShipQty.Size = new System.Drawing.Size(71, 23);
            this.numShipQty.TabIndex = 114;
            this.numShipQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P06_ShipQtyDetail
            // 
            this.ClientSize = new System.Drawing.Size(535, 395);
            this.Controls.Add(this.numShipQty);
            this.Controls.Add(this.numOrderQtybySeq);
            this.Controls.Add(this.numOrderQty);
            this.Controls.Add(this.displayStatus);
            this.Controls.Add(this.labelShipQty);
            this.Controls.Add(this.labelOrderQtybySeq);
            this.Controls.Add(this.labelOrderQty);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.displayShipMode);
            this.Controls.Add(this.displayPackingNo);
            this.Controls.Add(this.displaySEQ);
            this.Controls.Add(this.displaySPNo);
            this.Controls.Add(this.labelShipMode);
            this.Controls.Add(this.labelPackingNo);
            this.Controls.Add(this.labelseq);
            this.Controls.Add(this.labelSPNo);
            this.Name = "P06_ShipQtyDetail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelseq, 0);
            this.Controls.SetChildIndex(this.labelPackingNo, 0);
            this.Controls.SetChildIndex(this.labelShipMode, 0);
            this.Controls.SetChildIndex(this.displaySPNo, 0);
            this.Controls.SetChildIndex(this.displaySEQ, 0);
            this.Controls.SetChildIndex(this.displayPackingNo, 0);
            this.Controls.SetChildIndex(this.displayShipMode, 0);
            this.Controls.SetChildIndex(this.labelStatus, 0);
            this.Controls.SetChildIndex(this.labelOrderQty, 0);
            this.Controls.SetChildIndex(this.labelOrderQtybySeq, 0);
            this.Controls.SetChildIndex(this.labelShipQty, 0);
            this.Controls.SetChildIndex(this.displayStatus, 0);
            this.Controls.SetChildIndex(this.numOrderQty, 0);
            this.Controls.SetChildIndex(this.numOrderQtybySeq, 0);
            this.Controls.SetChildIndex(this.numShipQty, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelseq;
        private Win.UI.Label labelPackingNo;
        private Win.UI.Label labelShipMode;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.DisplayBox displaySEQ;
        private Win.UI.DisplayBox displayPackingNo;
        private Win.UI.DisplayBox displayShipMode;
        private Win.UI.Label labelStatus;
        private Win.UI.Label labelOrderQty;
        private Win.UI.Label labelOrderQtybySeq;
        private Win.UI.Label labelShipQty;
        private Win.UI.DisplayBox displayStatus;
        private Win.UI.NumericBox numOrderQty;
        private Win.UI.NumericBox numOrderQtybySeq;
        private Win.UI.NumericBox numShipQty;
    }
}
