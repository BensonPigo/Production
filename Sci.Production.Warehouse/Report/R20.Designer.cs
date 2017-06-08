namespace Sci.Production.Warehouse
{
    partial class R20
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
            this.labelDeadLine = new Sci.Win.UI.Label();
            this.dateDeadLine = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelInventoryETA = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.textStartSP = new Sci.Win.UI.TextBox();
            this.textEndSP = new Sci.Win.UI.TextBox();
            this.dateInventoryETA = new Sci.Win.UI.DateRange();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.textStartRefno = new Sci.Win.UI.TextBox();
            this.textEndRefno = new Sci.Win.UI.TextBox();
            this.txtmfactory1 = new Sci.Production.Class.txtmfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(511, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(407, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(407, 48);
            // 
            // labelDeadLine
            // 
            this.labelDeadLine.Location = new System.Drawing.Point(9, 9);
            this.labelDeadLine.Name = "labelDeadLine";
            this.labelDeadLine.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelDeadLine.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelDeadLine.Size = new System.Drawing.Size(97, 23);
            this.labelDeadLine.TabIndex = 94;
            this.labelDeadLine.Text = "DeadLine";
            this.labelDeadLine.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateDeadLine
            // 
            this.dateDeadLine.Location = new System.Drawing.Point(109, 9);
            this.dateDeadLine.Name = "dateDeadLine";
            this.dateDeadLine.Size = new System.Drawing.Size(280, 23);
            this.dateDeadLine.TabIndex = 95;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(9, 38);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelBuyerDelivery.Size = new System.Drawing.Size(97, 23);
            this.labelBuyerDelivery.TabIndex = 94;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            this.labelBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(109, 38);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 95;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 67);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(97, 23);
            this.labelSPNo.TabIndex = 96;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelInventoryETA
            // 
            this.labelInventoryETA.Location = new System.Drawing.Point(9, 95);
            this.labelInventoryETA.Name = "labelInventoryETA";
            this.labelInventoryETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelInventoryETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelInventoryETA.Size = new System.Drawing.Size(97, 23);
            this.labelInventoryETA.TabIndex = 96;
            this.labelInventoryETA.Text = "InventoryETA";
            this.labelInventoryETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(9, 124);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(97, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 151);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(97, 23);
            this.labelFactory.TabIndex = 96;
            this.labelFactory.Text = "Factory";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(9, 181);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(97, 23);
            this.labelRefno.TabIndex = 96;
            this.labelRefno.Text = "Refno";
            // 
            // textStartSP
            // 
            this.textStartSP.BackColor = System.Drawing.Color.White;
            this.textStartSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textStartSP.Location = new System.Drawing.Point(110, 67);
            this.textStartSP.Name = "textStartSP";
            this.textStartSP.Size = new System.Drawing.Size(121, 23);
            this.textStartSP.TabIndex = 97;
            // 
            // textEndSP
            // 
            this.textEndSP.BackColor = System.Drawing.Color.White;
            this.textEndSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEndSP.Location = new System.Drawing.Point(259, 67);
            this.textEndSP.Name = "textEndSP";
            this.textEndSP.Size = new System.Drawing.Size(121, 23);
            this.textEndSP.TabIndex = 97;
            // 
            // dateInventoryETA
            // 
            this.dateInventoryETA.Location = new System.Drawing.Point(109, 95);
            this.dateInventoryETA.Name = "dateInventoryETA";
            this.dateInventoryETA.Size = new System.Drawing.Size(280, 23);
            this.dateInventoryETA.TabIndex = 95;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(110, 124);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 99;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(234, 67);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label1.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.Size = new System.Drawing.Size(22, 23);
            this.label1.TabIndex = 100;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(259, 180);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label2.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.Size = new System.Drawing.Size(22, 23);
            this.label2.TabIndex = 103;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // textStartRefno
            // 
            this.textStartRefno.BackColor = System.Drawing.Color.White;
            this.textStartRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textStartRefno.Location = new System.Drawing.Point(110, 180);
            this.textStartRefno.Name = "textStartRefno";
            this.textStartRefno.Size = new System.Drawing.Size(146, 23);
            this.textStartRefno.TabIndex = 102;
            // 
            // textEndRefno
            // 
            this.textEndRefno.BackColor = System.Drawing.Color.White;
            this.textEndRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textEndRefno.Location = new System.Drawing.Point(284, 181);
            this.textEndRefno.Name = "textEndRefno";
            this.textEndRefno.Size = new System.Drawing.Size(146, 23);
            this.textEndRefno.TabIndex = 102;
            // 
            // txtmfactory1
            // 
            this.txtmfactory1.BackColor = System.Drawing.Color.White;
            this.txtmfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory1.Location = new System.Drawing.Point(110, 151);
            this.txtmfactory1.Name = "txtmfactory1";
            this.txtmfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory1.TabIndex = 104;
            // 
            // R20
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 262);
            this.Controls.Add(this.txtmfactory1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textEndRefno);
            this.Controls.Add(this.textStartRefno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.textEndSP);
            this.Controls.Add(this.textStartSP);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelInventoryETA);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.dateInventoryETA);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.dateDeadLine);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.labelDeadLine);
            this.Name = "R20";
            this.Text = "R20 Stock List Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDeadLine, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.dateDeadLine, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.dateInventoryETA, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelInventoryETA, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.textStartSP, 0);
            this.Controls.SetChildIndex(this.textEndSP, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textStartRefno, 0);
            this.Controls.SetChildIndex(this.textEndRefno, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtmfactory1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDeadLine;
        private Win.UI.DateRange dateDeadLine;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelInventoryETA;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelRefno;
        private Win.UI.TextBox textStartSP;
        private Win.UI.TextBox textEndSP;
        private Win.UI.DateRange dateInventoryETA;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.TextBox textStartRefno;
        private Win.UI.TextBox textEndRefno;
        private Class.txtmfactory txtmfactory1;
    }
}