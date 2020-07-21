namespace Sci.Production.Warehouse
{
    partial class R08
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
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.dateArrive = new Sci.Win.UI.DateRange();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(456, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(456, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(456, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(317, 138);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(343, 144);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(375, 142);
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(32, 12);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.RectStyle.BorderWidth = 1F;
            this.labelBuyerDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelBuyerDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelBuyerDelivery.Size = new System.Drawing.Size(111, 23);
            this.labelBuyerDelivery.TabIndex = 106;
            this.labelBuyerDelivery.Text = "Arrive W/H Date";
            this.labelBuyerDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(32, 48);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(111, 23);
            this.labelSPNo.TabIndex = 115;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(32, 121);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(111, 23);
            this.labelFactory.TabIndex = 131;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(32, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(111, 23);
            this.labelM.TabIndex = 130;
            this.labelM.Text = "M";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 23);
            this.label1.TabIndex = 132;
            this.label1.Text = "Brand";
            // 
            // dateArrive
            // 
            // 
            // 
            // 
            this.dateArrive.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArrive.DateBox1.Name = "";
            this.dateArrive.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArrive.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArrive.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArrive.DateBox2.Name = "";
            this.dateArrive.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArrive.DateBox2.TabIndex = 1;
            this.dateArrive.Location = new System.Drawing.Point(146, 12);
            this.dateArrive.Name = "dateArrive";
            this.dateArrive.Size = new System.Drawing.Size(280, 23);
            this.dateArrive.TabIndex = 133;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(307, 48);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 135;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(146, 48);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 134;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(275, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 17);
            this.label2.TabIndex = 137;
            this.label2.Text = " ～";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(146, 121);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 139;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(146, 84);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 138;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(146, 157);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 140;
            // 
            // R08
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 225);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.dateArrive);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Name = "R08";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R08. Fabric Release Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateArrive, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateArrive;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private System.Windows.Forms.Label label2;
        private Class.Txtfactory txtfactory;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtbrand txtBrand;
    }
}