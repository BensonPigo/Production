namespace Sci.Production.Shipping
{
    partial class R07
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.dateApvDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.txtSubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(380, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(380, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(380, 84);
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(13, 12);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(65, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Lines = 0;
            this.labelApvDate.Location = new System.Drawing.Point(13, 48);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(65, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(65, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(13, 120);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(65, 23);
            this.labelSupplier.TabIndex = 98;
            this.labelSupplier.Text = "Supplier";
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(82, 12);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 100;
            // 
            // dateApvDate
            // 
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(82, 48);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateApvDate.TabIndex = 101;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(82, 84);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(73, 24);
            this.comboM.TabIndex = 104;
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = true;
            this.txtSubconSupplier.Location = new System.Drawing.Point(82, 120);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 105;
            this.txtSubconSupplier.TextBox1Binding = "";
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(472, 181);
            this.Controls.Add(this.txtSubconSupplier);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateApvDate);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelApvDate);
            this.Controls.Add(this.labelDate);
            this.IsSupportToPrint = false;
            this.Name = "R07";
            this.Text = "R07. Payment Summary – Shipping";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelApvDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.dateApvDate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.txtSubconSupplier, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSupplier;
        private Win.UI.DateRange dateDate;
        private Win.UI.DateRange dateApvDate;
        private Win.UI.ComboBox comboM;
        private Class.TxtsubconNoConfirm txtSubconSupplier;
    }
}
