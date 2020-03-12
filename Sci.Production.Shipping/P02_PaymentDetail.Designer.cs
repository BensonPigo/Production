namespace Sci.Production.Shipping
{
    partial class P02_PaymentDetail
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.displayInvNo = new Sci.Win.UI.DisplayBox();
            this.numAmount = new Sci.Win.UI.NumericBox();
            this.displayAmount = new Sci.Win.UI.DisplayBox();
            this.labelInvNo = new Sci.Win.UI.Label();
            this.labelAmount = new Sci.Win.UI.Label();
            this.labelPayDate = new Sci.Win.UI.Label();
            this.datePayDate = new Sci.Win.UI.DateBox();
            this.SuspendLayout();
            // 
            // displayInvNo
            // 
            this.displayInvNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInvNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInvNo.Location = new System.Drawing.Point(95, 79);
            this.displayInvNo.Name = "displayInvNo";
            this.displayInvNo.Size = new System.Drawing.Size(230, 23);
            this.displayInvNo.TabIndex = 18;
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAmount.DecimalPlaces = 2;
            this.numAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAmount.IsSupportEditMode = false;
            this.numAmount.Location = new System.Drawing.Point(137, 49);
            this.numAmount.Name = "numAmount";
            this.numAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAmount.ReadOnly = true;
            this.numAmount.Size = new System.Drawing.Size(70, 23);
            this.numAmount.TabIndex = 17;
            this.numAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayAmount
            // 
            this.displayAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAmount.Location = new System.Drawing.Point(95, 49);
            this.displayAmount.Name = "displayAmount";
            this.displayAmount.Size = new System.Drawing.Size(40, 23);
            this.displayAmount.TabIndex = 16;
            // 
            // labelInvNo
            // 
            this.labelInvNo.Location = new System.Drawing.Point(29, 79);
            this.labelInvNo.Name = "labelInvNo";
            this.labelInvNo.Size = new System.Drawing.Size(62, 23);
            this.labelInvNo.TabIndex = 14;
            this.labelInvNo.Text = "Inv No.";
            // 
            // labelAmount
            // 
            this.labelAmount.Location = new System.Drawing.Point(29, 49);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(62, 23);
            this.labelAmount.TabIndex = 13;
            this.labelAmount.Text = "Amount";
            // 
            // labelPayDate
            // 
            this.labelPayDate.Location = new System.Drawing.Point(29, 20);
            this.labelPayDate.Name = "labelPayDate";
            this.labelPayDate.Size = new System.Drawing.Size(62, 23);
            this.labelPayDate.TabIndex = 12;
            this.labelPayDate.Text = "Pay Date";
            // 
            // datePayDate
            // 
            this.datePayDate.IsSupportEditMode = false;
            this.datePayDate.Location = new System.Drawing.Point(95, 20);
            this.datePayDate.Name = "datePayDate";
            this.datePayDate.ReadOnly = true;
            this.datePayDate.Size = new System.Drawing.Size(110, 23);
            this.datePayDate.TabIndex = 15;
            // 
            // P02_PaymentDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 123);
            this.Controls.Add(this.displayInvNo);
            this.Controls.Add(this.numAmount);
            this.Controls.Add(this.displayAmount);
            this.Controls.Add(this.labelInvNo);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.labelPayDate);
            this.Controls.Add(this.datePayDate);
            this.Name = "P02_PaymentDetail";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P02_PaymentDetail";
            this.Controls.SetChildIndex(this.datePayDate, 0);
            this.Controls.SetChildIndex(this.labelPayDate, 0);
            this.Controls.SetChildIndex(this.labelAmount, 0);
            this.Controls.SetChildIndex(this.labelInvNo, 0);
            this.Controls.SetChildIndex(this.displayAmount, 0);
            this.Controls.SetChildIndex(this.numAmount, 0);
            this.Controls.SetChildIndex(this.displayInvNo, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayInvNo;
        private Win.UI.NumericBox numAmount;
        private Win.UI.DisplayBox displayAmount;
        private Win.UI.Label labelInvNo;
        private Win.UI.Label labelAmount;
        private Win.UI.Label labelPayDate;
        private Win.UI.DateBox datePayDate;
    }
}