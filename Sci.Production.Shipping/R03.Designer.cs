namespace Sci.Production.Shipping
{
    partial class R03
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
            this.components = new System.ComponentModel.Container();
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.datePulloutDate = new Sci.Win.UI.DateRange();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(413, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(413, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(413, 84);
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 12);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(80, 23);
            this.labelPulloutDate.TabIndex = 94;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(13, 48);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(80, 23);
            this.labelBrand.TabIndex = 95;
            this.labelBrand.Text = "Brand";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(80, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(80, 23);
            this.labelFactory.TabIndex = 97;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 156);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(80, 23);
            this.labelCategory.TabIndex = 98;
            this.labelCategory.Text = "Category";
            // 
            // datePulloutDate
            // 
            // 
            // 
            // 
            this.datePulloutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePulloutDate.DateBox1.Name = "";
            this.datePulloutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePulloutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePulloutDate.DateBox2.Name = "";
            this.datePulloutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox2.TabIndex = 1;
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(97, 12);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 99;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(97, 48);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(83, 23);
            this.txtbrand.TabIndex = 100;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(97, 84);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(66, 24);
            this.comboM.TabIndex = 101;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(97, 120);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(66, 24);
            this.comboFactory.TabIndex = 102;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(97, 155);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(280, 24);
            this.comboCategory.TabIndex = 138;
            this.comboCategory.Type = "Pms_GMT_Simple";
            // 
            // R03
            // 
            this.ClientSize = new System.Drawing.Size(505, 213);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.datePulloutDate);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelPulloutDate);
            this.IsSupportToPrint = false;
            this.Name = "R03";
            this.Text = "R03. Actual Shipment Record";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelPulloutDate, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.datePulloutDate, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelPulloutDate;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.DateRange datePulloutDate;
        private Class.Txtbrand txtbrand;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboFactory;
        private Class.ComboDropDownList comboCategory;
    }
}
