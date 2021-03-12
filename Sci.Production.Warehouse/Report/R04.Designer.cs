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
            this.components = new System.ComponentModel.Container();
            this.dateCFMDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelOperation = new Sci.Win.UI.Label();
            this.labelCFMDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtdropdownlistOperation = new Sci.Production.Class.Txtdropdownlist();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSpStart = new Sci.Win.UI.TextBox();
            this.txtSpEnd = new Sci.Win.UI.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.comboFabricType = new Sci.Production.Class.ComboDropDownList(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(454, 12);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(454, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(454, 84);
            this.close.TabIndex = 9;
            // 
            // dateCFMDate
            // 
            // 
            // 
            // 
            this.dateCFMDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCFMDate.DateBox1.Name = "";
            this.dateCFMDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCFMDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCFMDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCFMDate.DateBox2.Name = "";
            this.dateCFMDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCFMDate.DateBox2.TabIndex = 1;
            this.dateCFMDate.IsRequired = false;
            this.dateCFMDate.Location = new System.Drawing.Point(116, 12);
            this.dateCFMDate.Name = "dateCFMDate";
            this.dateCFMDate.Size = new System.Drawing.Size(280, 23);
            this.dateCFMDate.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(15, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 103;
            this.labelFactory.Text = "Factory";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(15, 156);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(98, 23);
            this.labelBrand.TabIndex = 118;
            this.labelBrand.Text = "Brand";
            // 
            // labelOperation
            // 
            this.labelOperation.Location = new System.Drawing.Point(15, 192);
            this.labelOperation.Name = "labelOperation";
            this.labelOperation.Size = new System.Drawing.Size(98, 23);
            this.labelOperation.TabIndex = 125;
            this.labelOperation.Text = "Operation";
            // 
            // labelCFMDate
            // 
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
            this.labelM.Location = new System.Drawing.Point(15, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 131;
            this.labelM.Text = "M";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(116, 156);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 5;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(116, 84);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 3;
            this.txtMdivision.Validated += new System.EventHandler(this.TxtMdivision_Validated);
            // 
            // txtdropdownlistOperation
            // 
            this.txtdropdownlistOperation.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistOperation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistOperation.FormattingEnabled = true;
            this.txtdropdownlistOperation.IsSupportUnselect = true;
            this.txtdropdownlistOperation.Location = new System.Drawing.Point(116, 191);
            this.txtdropdownlistOperation.Name = "txtdropdownlistOperation";
            this.txtdropdownlistOperation.OldText = "";
            this.txtdropdownlistOperation.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistOperation.TabIndex = 6;
            this.txtdropdownlistOperation.Type = "InvtransType";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(116, 120);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 48);
            this.label1.Name = "label1";
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 133;
            this.label1.Text = "Bulk SP";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSpStart
            // 
            this.txtSpStart.BackColor = System.Drawing.Color.White;
            this.txtSpStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpStart.Location = new System.Drawing.Point(117, 48);
            this.txtSpStart.Name = "txtSpStart";
            this.txtSpStart.Size = new System.Drawing.Size(120, 23);
            this.txtSpStart.TabIndex = 1;
            // 
            // txtSpEnd
            // 
            this.txtSpEnd.BackColor = System.Drawing.Color.White;
            this.txtSpEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpEnd.Location = new System.Drawing.Point(271, 48);
            this.txtSpEnd.Name = "txtSpEnd";
            this.txtSpEnd.Size = new System.Drawing.Size(120, 23);
            this.txtSpEnd.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 136;
            this.label2.Text = "～";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(15, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 137;
            this.label3.Text = "Material Type";
            // 
            // comboFabricType
            // 
            this.comboFabricType.BackColor = System.Drawing.Color.White;
            this.comboFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.IsSupportUnselect = true;
            this.comboFabricType.Location = new System.Drawing.Point(116, 234);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.OldText = "";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 7;
            this.comboFabricType.Type = null;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(546, 305);
            this.Controls.Add(this.comboFabricType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSpEnd);
            this.Controls.Add(this.txtSpStart);
            this.Controls.Add(this.label1);
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
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R04. FTY Weekly Stock Transaction  List";
            this.Controls.SetChildIndex(this.dateCFMDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.labelOperation, 0);
            this.Controls.SetChildIndex(this.txtdropdownlistOperation, 0);
            this.Controls.SetChildIndex(this.labelCFMDate, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtSpStart, 0);
            this.Controls.SetChildIndex(this.txtSpEnd, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.comboFabricType, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateCFMDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelOperation;
        private Class.Txtdropdownlist txtdropdownlistOperation;
        private Win.UI.Label labelCFMDate;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtbrand txtbrand;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSpStart;
        private Win.UI.TextBox txtSpEnd;
        private System.Windows.Forms.Label label2;
        private Win.UI.Label label3;
        private Class.ComboDropDownList comboFabricType;
    }
}
