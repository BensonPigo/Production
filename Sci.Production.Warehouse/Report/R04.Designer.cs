namespace Sci.Production.Warehouse
{
    partial class R04
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
            this.dateCFMDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelOperation = new Sci.Win.UI.Label();
            this.labelCFMDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.txtdropdownlistOperation = new Sci.Production.Class.txtdropdownlist();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(439, 12);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 48);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 84);
            this.close.TabIndex = 7;
            // 
            // dateCFMDate
            // 
            this.dateCFMDate.IsRequired = false;
            this.dateCFMDate.Location = new System.Drawing.Point(116, 12);
            this.dateCFMDate.Name = "dateCFMDate";
            this.dateCFMDate.Size = new System.Drawing.Size(280, 23);
            this.dateCFMDate.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(15, 84);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 103;
            this.labelFactory.Text = "Factory";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(15, 120);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 118;
            this.labelBrand.Text = "Brand";
            // 
            // labelOperation
            // 
            this.labelOperation.Lines = 0;
            this.labelOperation.Location = new System.Drawing.Point(15, 156);
            this.labelOperation.Name = "labelOperation";
            this.labelOperation.Size = new System.Drawing.Size(98, 23);
            this.labelOperation.TabIndex = 125;
            this.labelOperation.Text = "Operation";
            // 
            // labelCFMDate
            // 
            this.labelCFMDate.Lines = 0;
            this.labelCFMDate.Location = new System.Drawing.Point(15, 12);
            this.labelCFMDate.Name = "labelCFMDate";
            this.labelCFMDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelCFMDate.Size = new System.Drawing.Size(98, 23);
            this.labelCFMDate.TabIndex = 128;
            this.labelCFMDate.Text = "CFM Date";
            this.labelCFMDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(15, 48);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 131;
            this.labelM.Text = "M";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(116, 120);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 3;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(116, 48);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 1;
            this.txtMdivision.Validated += new System.EventHandler(this.txtMdivision_Validated);
            // 
            // txtdropdownlistOperation
            // 
            this.txtdropdownlistOperation.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistOperation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistOperation.FormattingEnabled = true;
            this.txtdropdownlistOperation.IsSupportUnselect = true;
            this.txtdropdownlistOperation.Location = new System.Drawing.Point(116, 155);
            this.txtdropdownlistOperation.Name = "txtdropdownlistOperation";
            this.txtdropdownlistOperation.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistOperation.TabIndex = 4;
            this.txtdropdownlistOperation.Type = "InvtransType";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(116, 84);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 132;
            this.txtfactory.IssupportJunk = true;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(531, 221);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.labelCFMDate);
            this.Controls.Add(this.txtdropdownlistOperation);
            this.Controls.Add(this.labelOperation);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateCFMDate);
            this.IsSupportToPrint = false;
            this.Name = "R04";
            this.Text = "R04. FTY Weekly Stock Transaction  List";
            this.Controls.SetChildIndex(this.dateCFMDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelOperation, 0);
            this.Controls.SetChildIndex(this.txtdropdownlistOperation, 0);
            this.Controls.SetChildIndex(this.labelCFMDate, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateCFMDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelOperation;
        private Class.txtdropdownlist txtdropdownlistOperation;
        private Win.UI.Label labelCFMDate;
        private Win.UI.Label labelM;
        private Class.txtMdivision txtMdivision;
        private Class.txtbrand txtbrand;
        private Class.txtfactory txtfactory;
    }
}
