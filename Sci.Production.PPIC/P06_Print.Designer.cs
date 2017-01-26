namespace Sci.Production.PPIC
{
    partial class P06_Print
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
            this.label1 = new Sci.Win.UI.Label();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.dateRange2 = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(401, 12);
            this.print.TabIndex = 3;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(401, 48);
            this.toexcel.TabIndex = 4;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(401, 84);
            this.close.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(23, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Delivery";
            // 
            // dateRange1
            // 
            this.dateRange1.IsSupportEditMode = false;
            this.dateRange1.Location = new System.Drawing.Point(106, 22);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 0;
            // 
            // dateRange2
            // 
            this.dateRange2.IsSupportEditMode = false;
            this.dateRange2.Location = new System.Drawing.Point(106, 58);
            this.dateRange2.Name = "dateRange2";
            this.dateRange2.Size = new System.Drawing.Size(280, 23);
            this.dateRange2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(23, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "SCI Delivery";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(23, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 98;
            this.label3.Text = "Brand";
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.IsSupportEditMode = false;
            this.txtbrand1.Location = new System.Drawing.Point(107, 94);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtbrand1.Size = new System.Drawing.Size(85, 23);
            this.txtbrand1.TabIndex = 2;
            // 
            // P06_Print
            // 
            this.ClientSize = new System.Drawing.Size(493, 162);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateRange2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.label1);
            this.DefaultControl = "dateRange1";
            this.DefaultControlForEdit = "dateRange1";
            this.IsSupportToPrint = false;
            this.Name = "P06_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateRange2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.DateRange dateRange1;
        private Win.UI.DateRange dateRange2;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Class.txtbrand txtbrand1;
    }
}
