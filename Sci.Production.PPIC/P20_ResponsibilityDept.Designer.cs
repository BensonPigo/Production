﻿namespace Sci.Production.PPIC
{
    partial class P20_ResponsibilityDept
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
            this.txtID = new Sci.Win.UI.TextBox();
            this.labICRNo = new Sci.Win.UI.Label();
            this.numTotalAmt = new Sci.Win.UI.NumericBox();
            this.labTotal = new Sci.Win.UI.Label();
            this.numPercentage = new Sci.Win.UI.NumericBox();
            this.numAmount = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 426);
            this.btmcont.Size = new System.Drawing.Size(528, 40);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 39);
            this.gridcont.Size = new System.Drawing.Size(504, 352);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(438, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(358, 5);
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.Location = new System.Drawing.Point(98, 8);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(131, 23);
            this.txtID.TabIndex = 99;
            // 
            // labICRNo
            // 
            this.labICRNo.Location = new System.Drawing.Point(14, 8);
            this.labICRNo.Name = "labICRNo";
            this.labICRNo.Size = new System.Drawing.Size(81, 23);
            this.labICRNo.TabIndex = 98;
            this.labICRNo.Text = "ICR No.";
            // 
            // numTotalAmt
            // 
            this.numTotalAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalAmt.DecimalPlaces = 2;
            this.numTotalAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalAmt.IsSupportEditMode = false;
            this.numTotalAmt.Location = new System.Drawing.Point(412, 8);
            this.numTotalAmt.Name = "numTotalAmt";
            this.numTotalAmt.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalAmt.ReadOnly = true;
            this.numTotalAmt.Size = new System.Drawing.Size(104, 23);
            this.numTotalAmt.TabIndex = 101;
            this.numTotalAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labTotal
            // 
            this.labTotal.Location = new System.Drawing.Point(300, 8);
            this.labTotal.Name = "labTotal";
            this.labTotal.Size = new System.Drawing.Size(110, 23);
            this.labTotal.TabIndex = 100;
            this.labTotal.Text = "Total Amt (USD)";
            // 
            // numPercentage
            // 
            this.numPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numPercentage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numPercentage.DecimalPlaces = 2;
            this.numPercentage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numPercentage.IsSupportEditMode = false;
            this.numPercentage.Location = new System.Drawing.Point(301, 397);
            this.numPercentage.Name = "numPercentage";
            this.numPercentage.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPercentage.ReadOnly = true;
            this.numPercentage.Size = new System.Drawing.Size(95, 23);
            this.numPercentage.TabIndex = 102;
            this.numPercentage.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numAmount
            // 
            this.numAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAmount.DecimalPlaces = 2;
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAmount.IsSupportEditMode = false;
            this.numAmount.Location = new System.Drawing.Point(401, 397);
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.ReadOnly = true;
            this.numAmount.Size = new System.Drawing.Size(104, 23);
            this.numAmount.TabIndex = 103;
            this.numAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // P20_ResponsibilityDept
            // 
            this.ClientSize = new System.Drawing.Size(528, 466);
            this.Controls.Add(this.numAmount);
            this.Controls.Add(this.numPercentage);
            this.Controls.Add(this.numTotalAmt);
            this.Controls.Add(this.labTotal);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.labICRNo);
            this.KeyField1 = "ID";
            this.Name = "P20_ResponsibilityDept";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "P20_ResponsibilityDept";
            this.WorkAlias = "ICR_ResponsibilityDept";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labICRNo, 0);
            this.Controls.SetChildIndex(this.txtID, 0);
            this.Controls.SetChildIndex(this.labTotal, 0);
            this.Controls.SetChildIndex(this.numTotalAmt, 0);
            this.Controls.SetChildIndex(this.numPercentage, 0);
            this.Controls.SetChildIndex(this.numAmount, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtID;
        private Win.UI.Label labICRNo;
        private Win.UI.NumericBox numTotalAmt;
        private Win.UI.Label labTotal;
        private Win.UI.NumericBox numPercentage;
        private Win.UI.NumericBox numAmount;
    }
}
