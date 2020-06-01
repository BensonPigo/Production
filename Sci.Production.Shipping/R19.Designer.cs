namespace Sci.Production.Shipping
{
    partial class R19
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.txtWKno_s = new Sci.Win.UI.TextBox();
            this.txtWKno_e = new Sci.Win.UI.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.txtConsignee = new Sci.Win.UI.TextBox();
            this.txtshipmode = new Sci.Production.Class.txtshipmode();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(422, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(422, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(422, 84);
            this.close.TabIndex = 8;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(346, 138);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(372, 144);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(402, 145);
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(94, 23);
            this.labelSCIDelivery.TabIndex = 97;
            this.labelSCIDelivery.Text = "ETA";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 23);
            this.label1.TabIndex = 98;
            this.label1.Text = "WK#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 23);
            this.label2.TabIndex = 99;
            this.label2.Text = "Factory";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 100;
            this.label3.Text = "Consignee";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 23);
            this.label4.TabIndex = 101;
            this.label4.Text = "Shipe Mode";
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.Location = new System.Drawing.Point(110, 12);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 1;
            // 
            // txtWKno_s
            // 
            this.txtWKno_s.BackColor = System.Drawing.Color.White;
            this.txtWKno_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKno_s.Location = new System.Drawing.Point(110, 48);
            this.txtWKno_s.Name = "txtWKno_s";
            this.txtWKno_s.Size = new System.Drawing.Size(100, 23);
            this.txtWKno_s.TabIndex = 2;
            // 
            // txtWKno_e
            // 
            this.txtWKno_e.BackColor = System.Drawing.Color.White;
            this.txtWKno_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKno_e.Location = new System.Drawing.Point(244, 48);
            this.txtWKno_e.Name = "txtWKno_e";
            this.txtWKno_e.Size = new System.Drawing.Size(100, 23);
            this.txtWKno_e.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(216, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 17);
            this.label5.TabIndex = 105;
            this.label5.Text = "～";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(110, 84);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // txtConsignee
            // 
            this.txtConsignee.BackColor = System.Drawing.Color.White;
            this.txtConsignee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtConsignee.Location = new System.Drawing.Point(110, 121);
            this.txtConsignee.Name = "txtConsignee";
            this.txtConsignee.Size = new System.Drawing.Size(100, 23);
            this.txtConsignee.TabIndex = 5;
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(110, 158);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(80, 24);
            this.txtshipmode.TabIndex = 6;
            this.txtshipmode.UseFunction = null;
            // 
            // R19
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 224);
            this.Controls.Add(this.txtshipmode);
            this.Controls.Add(this.txtConsignee);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtWKno_e);
            this.Controls.Add(this.txtWKno_s);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSCIDelivery);
            this.IsSupportPrint = false;
            this.Name = "R19";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R19. Import Schedule Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.txtWKno_s, 0);
            this.Controls.SetChildIndex(this.txtWKno_e, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtConsignee, 0);
            this.Controls.SetChildIndex(this.txtshipmode, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateETA;
        private Win.UI.TextBox txtWKno_s;
        private Win.UI.TextBox txtWKno_e;
        private System.Windows.Forms.Label label5;
        private Class.txtfactory txtfactory;
        private Win.UI.TextBox txtConsignee;
        private Class.txtshipmode txtshipmode;
    }
}