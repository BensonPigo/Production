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
            this.labelDelivery = new Sci.Win.UI.Label();
            this.dateDelivery = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
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
            // labelDelivery
            // 
            this.labelDelivery.Lines = 0;
            this.labelDelivery.Location = new System.Drawing.Point(23, 22);
            this.labelDelivery.Name = "labelDelivery";
            this.labelDelivery.Size = new System.Drawing.Size(80, 23);
            this.labelDelivery.TabIndex = 94;
            this.labelDelivery.Text = "Delivery";
            // 
            // dateDelivery
            // 
            this.dateDelivery.IsRequired = false;
            this.dateDelivery.IsSupportEditMode = false;
            this.dateDelivery.Location = new System.Drawing.Point(106, 22);
            this.dateDelivery.Name = "dateDelivery";
            this.dateDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateDelivery.TabIndex = 0;
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.IsSupportEditMode = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(106, 58);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 1;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(23, 58);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(80, 23);
            this.labelSCIDelivery.TabIndex = 96;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(23, 94);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(80, 23);
            this.labelBrand.TabIndex = 98;
            this.labelBrand.Text = "Brand";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(107, 94);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtbrand.Size = new System.Drawing.Size(85, 23);
            this.txtbrand.TabIndex = 2;
            // 
            // P06_Print
            // 
            this.ClientSize = new System.Drawing.Size(493, 162);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.dateDelivery);
            this.Controls.Add(this.labelDelivery);
            this.DefaultControl = "dateDelivery";
            this.DefaultControlForEdit = "dateDelivery";
            this.IsSupportToPrint = false;
            this.Name = "P06_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDelivery, 0);
            this.Controls.SetChildIndex(this.dateDelivery, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDelivery;
        private Win.UI.DateRange dateDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelBrand;
        private Class.Txtbrand txtbrand;
    }
}
