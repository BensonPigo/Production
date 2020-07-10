namespace Sci.Production.Planning
{
    partial class R02
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateInLineDate = new Sci.Win.UI.DateRange();
            this.labelInLineDate = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.dateCutInline = new Sci.Win.UI.DateRange();
            this.labelCutInline = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.checkIncludeFarmOutInDate = new Sci.Win.UI.CheckBox();
            this.labelSubcon = new Sci.Win.UI.Label();
            this.txtMultiSubconSubcon = new Sci.Production.Class.TxtMultiSubconNoConfirm();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtartworktype_ftySubProcess = new Sci.Production.Class.Txtartworktype_fty();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 11;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 12;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 13;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSCIDelivery.TabIndex = 96;
            this.labelSCIDelivery.Text = "SCI Delivery";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.Location = new System.Drawing.Point(115, 12);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // dateInLineDate
            // 
            // 
            // 
            // 
            this.dateInLineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInLineDate.DateBox1.Name = "";
            this.dateInLineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInLineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInLineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInLineDate.DateBox2.Name = "";
            this.dateInLineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInLineDate.DateBox2.TabIndex = 1;
            this.dateInLineDate.Location = new System.Drawing.Point(115, 48);
            this.dateInLineDate.Name = "dateInLineDate";
            this.dateInLineDate.Size = new System.Drawing.Size(280, 23);
            this.dateInLineDate.TabIndex = 1;
            // 
            // labelInLineDate
            // 
            this.labelInLineDate.Location = new System.Drawing.Point(13, 48);
            this.labelInLineDate.Name = "labelInLineDate";
            this.labelInLineDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelInLineDate.RectStyle.BorderWidth = 1F;
            this.labelInLineDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelInLineDate.RectStyle.ExtBorderWidth = 1F;
            this.labelInLineDate.Size = new System.Drawing.Size(98, 23);
            this.labelInLineDate.TabIndex = 105;
            this.labelInLineDate.Text = "InLine Date";
            this.labelInLineDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelInLineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(267, 155);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 5;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(114, 155);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(245, 155);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 115;
            this.label10.Text = "～";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(13, 155);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 114;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateCutInline
            // 
            // 
            // 
            // 
            this.dateCutInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCutInline.DateBox1.Name = "";
            this.dateCutInline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCutInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCutInline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCutInline.DateBox2.Name = "";
            this.dateCutInline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCutInline.DateBox2.TabIndex = 1;
            this.dateCutInline.Location = new System.Drawing.Point(115, 120);
            this.dateCutInline.Name = "dateCutInline";
            this.dateCutInline.Size = new System.Drawing.Size(280, 23);
            this.dateCutInline.TabIndex = 3;
            // 
            // labelCutInline
            // 
            this.labelCutInline.Location = new System.Drawing.Point(13, 120);
            this.labelCutInline.Name = "labelCutInline";
            this.labelCutInline.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCutInline.RectStyle.BorderWidth = 1F;
            this.labelCutInline.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelCutInline.RectStyle.ExtBorderWidth = 1F;
            this.labelCutInline.Size = new System.Drawing.Size(98, 23);
            this.labelCutInline.TabIndex = 128;
            this.labelCutInline.Text = "Cut Inline";
            this.labelCutInline.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCutInline.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(115, 84);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 2;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 84);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.RectStyle.BorderWidth = 1F;
            this.labelBuyerDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelBuyerDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelBuyerDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelBuyerDelivery.TabIndex = 127;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            this.labelBuyerDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(13, 302);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(98, 23);
            this.labelSubProcess.TabIndex = 95;
            this.labelSubProcess.Text = "SubProcess";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 226);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 264);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 132;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(13, 338);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 141;
            this.labelCategory.Text = "Category";
            // 
            // checkIncludeFarmOutInDate
            // 
            this.checkIncludeFarmOutInDate.AutoSize = true;
            this.checkIncludeFarmOutInDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeFarmOutInDate.Location = new System.Drawing.Point(326, 339);
            this.checkIncludeFarmOutInDate.Name = "checkIncludeFarmOutInDate";
            this.checkIncludeFarmOutInDate.Size = new System.Drawing.Size(184, 21);
            this.checkIncludeFarmOutInDate.TabIndex = 10;
            this.checkIncludeFarmOutInDate.Text = "Include Farm Out/In Date";
            this.checkIncludeFarmOutInDate.UseVisualStyleBackColor = true;
            // 
            // labelSubcon
            // 
            this.labelSubcon.Location = new System.Drawing.Point(13, 192);
            this.labelSubcon.Name = "labelSubcon";
            this.labelSubcon.Size = new System.Drawing.Size(98, 23);
            this.labelSubcon.TabIndex = 143;
            this.labelSubcon.Text = "Subcon";
            // 
            // txtMultiSubconSubcon
            // 
            this.txtMultiSubconSubcon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMultiSubconSubcon.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMultiSubconSubcon.IsSupportEditMode = false;
            this.txtMultiSubconSubcon.Location = new System.Drawing.Point(115, 192);
            this.txtMultiSubconSubcon.Name = "txtMultiSubconSubcon";
            this.txtMultiSubconSubcon.ReadOnly = true;
            this.txtMultiSubconSubcon.Size = new System.Drawing.Size(281, 23);
            this.txtMultiSubconSubcon.Subcons = null;
            this.txtMultiSubconSubcon.TabIndex = 142;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(115, 264);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 7;
            // 
            // txtartworktype_ftySubProcess
            // 
            this.txtartworktype_ftySubProcess.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftySubProcess.CClassify = "";
            this.txtartworktype_ftySubProcess.CSubprocess = "Y";
            this.txtartworktype_ftySubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftySubProcess.Location = new System.Drawing.Point(114, 302);
            this.txtartworktype_ftySubProcess.Name = "txtartworktype_ftySubProcess";
            this.txtartworktype_ftySubProcess.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftySubProcess.TabIndex = 8;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 227);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 6;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(115, 336);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(205, 24);
            this.comboCategory.TabIndex = 144;
            this.comboCategory.Type = "Pms_ReportForProduct";
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(522, 410);
            this.Controls.Add(this.comboCategory);
            this.Controls.Add(this.labelSubcon);
            this.Controls.Add(this.txtMultiSubconSubcon);
            this.Controls.Add(this.checkIncludeFarmOutInDate);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtartworktype_ftySubProcess);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateCutInline);
            this.Controls.Add(this.labelCutInline);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.dateInLineDate);
            this.Controls.Add(this.labelInLineDate);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelSubProcess);
            this.DefaultControl = "dateSCIDelivery";
            this.IsSupportToPrint = false;
            this.Name = "R02";
            this.Text = "R02. Subprocess Schedule Report";
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelInLineDate, 0);
            this.Controls.SetChildIndex(this.dateInLineDate, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelCutInline, 0);
            this.Controls.SetChildIndex(this.dateCutInline, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtartworktype_ftySubProcess, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labelCategory, 0);
            this.Controls.SetChildIndex(this.checkIncludeFarmOutInDate, 0);
            this.Controls.SetChildIndex(this.txtMultiSubconSubcon, 0);
            this.Controls.SetChildIndex(this.labelSubcon, 0);
            this.Controls.SetChildIndex(this.comboCategory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateInLineDate;
        private Win.UI.Label labelInLineDate;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label label10;
        private Win.UI.Label labelSPNo;
        private Win.UI.DateRange dateCutInline;
        private Win.UI.Label labelCutInline;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSubProcess;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Class.Txtartworktype_fty txtartworktype_ftySubProcess;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelCategory;
        private Win.UI.CheckBox checkIncludeFarmOutInDate;
        private Class.TxtMultiSubconNoConfirm txtMultiSubconSubcon;
        private Win.UI.Label labelSubcon;
        private Class.ComboDropDownList comboCategory;
    }
}
