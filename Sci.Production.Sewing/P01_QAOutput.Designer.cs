namespace Sci.Production.Sewing
{
    partial class P01_QAOutput
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
            this.labelArticle = new Sci.Win.UI.Label();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.displaySPNo2 = new Sci.Win.UI.DisplayBox();
            this.displayArticle = new Sci.Win.UI.DisplayBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.displayColor = new Sci.Win.UI.DisplayBox();
            this.labelTotal = new Sci.Win.UI.Label();
            this.numTotalOrderQty = new Sci.Win.UI.NumericBox();
            this.numTotalAccumQty = new Sci.Win.UI.NumericBox();
            this.numTotalVariance = new Sci.Win.UI.NumericBox();
            this.numTotalQAQty = new Sci.Win.UI.NumericBox();
            this.numTotalBalQty = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 61);
            this.gridcont.Size = new System.Drawing.Size(566, 301);
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 394);
            this.btmcont.Size = new System.Drawing.Size(590, 40);
            // 
            // append
            // 
            this.append.Visible = false;
            // 
            // save
            // 
            this.save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.save.Location = new System.Drawing.Point(420, 5);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(500, 5);
            // 
            // revise
            // 
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.Visible = false;
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(365, 5);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(310, 5);
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(12, 5);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(45, 23);
            this.labelSPNo.TabIndex = 99;
            this.labelSPNo.Text = "SP#";
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(12, 32);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(45, 23);
            this.labelArticle.TabIndex = 100;
            this.labelArticle.Text = "Article";
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(61, 5);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(120, 23);
            this.displaySPNo.TabIndex = 101;
            // 
            // displaySPNo2
            // 
            this.displaySPNo2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo2.Location = new System.Drawing.Point(182, 5);
            this.displaySPNo2.Name = "displaySPNo2";
            this.displaySPNo2.Size = new System.Drawing.Size(22, 23);
            this.displaySPNo2.TabIndex = 102;
            // 
            // displayArticle
            // 
            this.displayArticle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArticle.Location = new System.Drawing.Point(61, 32);
            this.displayArticle.Name = "displayArticle";
            this.displayArticle.Size = new System.Drawing.Size(68, 23);
            this.displayArticle.TabIndex = 103;
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(211, 32);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(39, 23);
            this.labelColor.TabIndex = 104;
            this.labelColor.Text = "Color";
            // 
            // displayColor
            // 
            this.displayColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColor.Location = new System.Drawing.Point(254, 32);
            this.displayColor.Name = "displayColor";
            this.displayColor.Size = new System.Drawing.Size(59, 23);
            this.displayColor.TabIndex = 105;
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(61, 365);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(38, 23);
            this.labelTotal.TabIndex = 106;
            this.labelTotal.Text = "Total";
            // 
            // numTotalOrderQty
            // 
            this.numTotalOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalOrderQty.IsSupportEditMode = false;
            this.numTotalOrderQty.Location = new System.Drawing.Point(109, 365);
            this.numTotalOrderQty.Name = "numTotalOrderQty";
            this.numTotalOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalOrderQty.ReadOnly = true;
            this.numTotalOrderQty.Size = new System.Drawing.Size(86, 23);
            this.numTotalOrderQty.TabIndex = 107;
            this.numTotalOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotalAccumQty
            // 
            this.numTotalAccumQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalAccumQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalAccumQty.IsSupportEditMode = false;
            this.numTotalAccumQty.Location = new System.Drawing.Point(201, 365);
            this.numTotalAccumQty.Name = "numTotalAccumQty";
            this.numTotalAccumQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalAccumQty.ReadOnly = true;
            this.numTotalAccumQty.Size = new System.Drawing.Size(86, 23);
            this.numTotalAccumQty.TabIndex = 108;
            this.numTotalAccumQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotalVariance
            // 
            this.numTotalVariance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalVariance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalVariance.IsSupportEditMode = false;
            this.numTotalVariance.Location = new System.Drawing.Point(294, 365);
            this.numTotalVariance.Name = "numTotalVariance";
            this.numTotalVariance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalVariance.ReadOnly = true;
            this.numTotalVariance.Size = new System.Drawing.Size(86, 23);
            this.numTotalVariance.TabIndex = 109;
            this.numTotalVariance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotalQAQty
            // 
            this.numTotalQAQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalQAQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalQAQty.IsSupportEditMode = false;
            this.numTotalQAQty.Location = new System.Drawing.Point(388, 365);
            this.numTotalQAQty.Name = "numTotalQAQty";
            this.numTotalQAQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalQAQty.ReadOnly = true;
            this.numTotalQAQty.Size = new System.Drawing.Size(86, 23);
            this.numTotalQAQty.TabIndex = 110;
            this.numTotalQAQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numTotalBalQty
            // 
            this.numTotalBalQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalBalQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalBalQty.IsSupportEditMode = false;
            this.numTotalBalQty.Location = new System.Drawing.Point(480, 365);
            this.numTotalBalQty.Name = "numTotalBalQty";
            this.numTotalBalQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalBalQty.ReadOnly = true;
            this.numTotalBalQty.Size = new System.Drawing.Size(86, 23);
            this.numTotalBalQty.TabIndex = 111;
            this.numTotalBalQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P01_QAOutput
            // 
            this.ClientSize = new System.Drawing.Size(590, 434);
            this.Controls.Add(this.numTotalBalQty);
            this.Controls.Add(this.numTotalQAQty);
            this.Controls.Add(this.numTotalVariance);
            this.Controls.Add(this.numTotalAccumQty);
            this.Controls.Add(this.numTotalOrderQty);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.displayColor);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.displayArticle);
            this.Controls.Add(this.displaySPNo2);
            this.Controls.Add(this.displaySPNo);
            this.Controls.Add(this.labelArticle);
            this.Controls.Add(this.labelSPNo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.Name = "P01_QAOutput";
            this.OnLineHelpID = "Sci.Win.Subs.Input8A";
            this.Text = "QA Output";
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelArticle, 0);
            this.Controls.SetChildIndex(this.displaySPNo, 0);
            this.Controls.SetChildIndex(this.displaySPNo2, 0);
            this.Controls.SetChildIndex(this.displayArticle, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.displayColor, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelTotal, 0);
            this.Controls.SetChildIndex(this.numTotalOrderQty, 0);
            this.Controls.SetChildIndex(this.numTotalAccumQty, 0);
            this.Controls.SetChildIndex(this.numTotalVariance, 0);
            this.Controls.SetChildIndex(this.numTotalQAQty, 0);
            this.Controls.SetChildIndex(this.numTotalBalQty, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelArticle;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.DisplayBox displaySPNo2;
        private Win.UI.DisplayBox displayArticle;
        private Win.UI.Label labelColor;
        private Win.UI.DisplayBox displayColor;
        private Win.UI.Label labelTotal;
        private Win.UI.NumericBox numTotalOrderQty;
        private Win.UI.NumericBox numTotalAccumQty;
        private Win.UI.NumericBox numTotalVariance;
        private Win.UI.NumericBox numTotalQAQty;
        private Win.UI.NumericBox numTotalBalQty;
    }
}
