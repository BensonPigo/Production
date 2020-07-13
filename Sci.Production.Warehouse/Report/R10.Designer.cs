namespace Sci.Production.Warehouse
{
    partial class R10
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
            this.labelDeadLine = new Sci.Win.UI.Label();
            this.dateDeadLine = new Sci.Win.UI.DateRange();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.txtRefnoEnd = new Sci.Win.UI.TextBox();
            this.txtRefnoStart = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.dateInventoryETA = new Sci.Win.UI.DateRange();
            this.labelInventoryETA = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 8;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 9;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 10;
            // 
            // labelDeadLine
            // 
            this.labelDeadLine.Lines = 0;
            this.labelDeadLine.Location = new System.Drawing.Point(13, 12);
            this.labelDeadLine.Name = "labelDeadLine";
            this.labelDeadLine.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelDeadLine.RectStyle.BorderWidth = 1F;
            this.labelDeadLine.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelDeadLine.RectStyle.ExtBorderWidth = 1F;
            this.labelDeadLine.Size = new System.Drawing.Size(98, 23);
            this.labelDeadLine.TabIndex = 96;
            this.labelDeadLine.Text = "Dead Line";
            this.labelDeadLine.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelDeadLine.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateDeadLine
            // 
            this.dateDeadLine.IsRequired = false;
            this.dateDeadLine.Location = new System.Drawing.Point(115, 12);
            this.dateDeadLine.Name = "dateDeadLine";
            this.dateDeadLine.Size = new System.Drawing.Size(280, 23);
            this.dateDeadLine.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 148);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 148);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 5;
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(115, 46);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Lines = 0;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(13, 46);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.RectStyle.BorderWidth = 1F;
            this.labelBuyerDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelBuyerDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelBuyerDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelBuyerDelivery.TabIndex = 105;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            this.labelBuyerDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(267, 80);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 3;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(114, 80);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(245, 80);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 115;
            this.label10.Text = "～";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(13, 80);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 114;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelRefno
            // 
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(13, 214);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(98, 23);
            this.labelRefno.TabIndex = 120;
            this.labelRefno.Text = "Refno";
            // 
            // txtRefnoEnd
            // 
            this.txtRefnoEnd.BackColor = System.Drawing.Color.White;
            this.txtRefnoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoEnd.Location = new System.Drawing.Point(303, 214);
            this.txtRefnoEnd.Name = "txtRefnoEnd";
            this.txtRefnoEnd.Size = new System.Drawing.Size(164, 23);
            this.txtRefnoEnd.TabIndex = 7;
            // 
            // txtRefnoStart
            // 
            this.txtRefnoStart.BackColor = System.Drawing.Color.White;
            this.txtRefnoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoStart.Location = new System.Drawing.Point(114, 214);
            this.txtRefnoStart.Name = "txtRefnoStart";
            this.txtRefnoStart.Size = new System.Drawing.Size(164, 23);
            this.txtRefnoStart.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(281, 214);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 23);
            this.label12.TabIndex = 123;
            this.label12.Text = "～";
            // 
            // dateInventoryETA
            // 
            this.dateInventoryETA.IsRequired = false;
            this.dateInventoryETA.Location = new System.Drawing.Point(115, 114);
            this.dateInventoryETA.Name = "dateInventoryETA";
            this.dateInventoryETA.Size = new System.Drawing.Size(280, 23);
            this.dateInventoryETA.TabIndex = 4;
            // 
            // labelInventoryETA
            // 
            this.labelInventoryETA.Lines = 0;
            this.labelInventoryETA.Location = new System.Drawing.Point(13, 114);
            this.labelInventoryETA.Name = "labelInventoryETA";
            this.labelInventoryETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelInventoryETA.RectStyle.BorderWidth = 1F;
            this.labelInventoryETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelInventoryETA.RectStyle.ExtBorderWidth = 1F;
            this.labelInventoryETA.Size = new System.Drawing.Size(98, 23);
            this.labelInventoryETA.TabIndex = 125;
            this.labelInventoryETA.Text = "Inventory ETA";
            this.labelInventoryETA.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelInventoryETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 182);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 126;
            this.labelFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(115, 182);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 127;
            this.txtfactory.IssupportJunk = true;
            // 
            // R10
            // 
            this.ClientSize = new System.Drawing.Size(522, 285);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateInventoryETA);
            this.Controls.Add(this.labelInventoryETA);
            this.Controls.Add(this.txtRefnoEnd);
            this.Controls.Add(this.txtRefnoStart);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.dateDeadLine);
            this.Controls.Add(this.labelDeadLine);
            this.IsSupportToPrint = false;
            this.Name = "R10";
            this.Text = "R10. Material aging analysis for Inventory";
            this.Controls.SetChildIndex(this.labelDeadLine, 0);
            this.Controls.SetChildIndex(this.dateDeadLine, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtRefnoStart, 0);
            this.Controls.SetChildIndex(this.txtRefnoEnd, 0);
            this.Controls.SetChildIndex(this.labelInventoryETA, 0);
            this.Controls.SetChildIndex(this.dateInventoryETA, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDeadLine;
        private Win.UI.DateRange dateDeadLine;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label label10;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelRefno;
        private Win.UI.TextBox txtRefnoEnd;
        private Win.UI.TextBox txtRefnoStart;
        private Win.UI.Label label12;
        private Win.UI.DateRange dateInventoryETA;
        private Win.UI.Label labelInventoryETA;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
    }
}
