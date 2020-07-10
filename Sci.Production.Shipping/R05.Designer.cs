namespace Sci.Production.Shipping
{
    partial class R05
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
            this.labelM = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelOrderby = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboOrderby = new Sci.Win.UI.ComboBox();
            this.txtSubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(382, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(382, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(382, 84);
            // 
            // labelDate
            // 
            this.labelDate.Lines = 0;
            this.labelDate.Location = new System.Drawing.Point(13, 13);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(61, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 48);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(61, 23);
            this.labelM.TabIndex = 95;
            this.labelM.Text = "M";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(13, 84);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(61, 23);
            this.labelSupplier.TabIndex = 96;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelOrderby
            // 
            this.labelOrderby.Lines = 0;
            this.labelOrderby.Location = new System.Drawing.Point(13, 120);
            this.labelOrderby.Name = "labelOrderby";
            this.labelOrderby.Size = new System.Drawing.Size(61, 23);
            this.labelOrderby.TabIndex = 97;
            this.labelOrderby.Text = "Order by";
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(78, 13);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 98;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(78, 48);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(68, 24);
            this.comboM.TabIndex = 99;
            // 
            // comboOrderby
            // 
            this.comboOrderby.BackColor = System.Drawing.Color.White;
            this.comboOrderby.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderby.FormattingEnabled = true;
            this.comboOrderby.IsSupportUnselect = true;
            this.comboOrderby.Location = new System.Drawing.Point(78, 120);
            this.comboOrderby.Name = "comboOrderby";
            this.comboOrderby.Size = new System.Drawing.Size(103, 24);
            this.comboOrderby.TabIndex = 100;
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = true;
            this.txtSubconSupplier.Location = new System.Drawing.Point(78, 84);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 101;
            this.txtSubconSupplier.TextBox1Binding = "";
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(474, 185);
            this.Controls.Add(this.txtSubconSupplier);
            this.Controls.Add(this.comboOrderby);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelOrderby);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelDate);
            this.IsSupportToPrint = false;
            this.Name = "R05";
            this.Text = "R05. Outstanding Payment List - Shipping";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.labelOrderby, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboOrderby, 0);
            this.Controls.SetChildIndex(this.txtSubconSupplier, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelOrderby;
        private Win.UI.DateRange dateDate;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboOrderby;
        private Class.TxtsubconNoConfirm txtSubconSupplier;
    }
}
