namespace Sci.Production.Warehouse
{
    partial class R43
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateIssue = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtTransfer = new Sci.Win.UI.TextBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(415, 84);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(415, 9);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(415, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(277, 44);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(277, 80);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(277, 107);
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(21, 15);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSCIDelivery.TabIndex = 97;
            this.labelSCIDelivery.Text = "Issue Date";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateIssue
            // 
            // 
            // 
            // 
            this.dateIssue.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssue.DateBox1.Name = "";
            this.dateIssue.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssue.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssue.DateBox2.Name = "";
            this.dateIssue.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox2.TabIndex = 1;
            this.dateIssue.Location = new System.Drawing.Point(123, 15);
            this.dateIssue.Name = "dateIssue";
            this.dateIssue.Size = new System.Drawing.Size(280, 23);
            this.dateIssue.TabIndex = 98;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "Transfer#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(21, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 101;
            this.label3.Text = "Factory";
            // 
            // txtTransfer
            // 
            this.txtTransfer.BackColor = System.Drawing.Color.White;
            this.txtTransfer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransfer.Location = new System.Drawing.Point(122, 41);
            this.txtTransfer.Name = "txtTransfer";
            this.txtTransfer.Size = new System.Drawing.Size(108, 23);
            this.txtTransfer.TabIndex = 102;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(123, 68);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(107, 23);
            this.txtMdivision.TabIndex = 103;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(123, 95);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(107, 23);
            this.txtfactory.TabIndex = 104;
            // 
            // R43
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 163);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtTransfer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateIssue);
            this.Controls.Add(this.labelSCIDelivery);
            this.Name = "R43";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R43. R/Mtl Return List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateIssue, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtTransfer, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateIssue;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtTransfer;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
    }
}