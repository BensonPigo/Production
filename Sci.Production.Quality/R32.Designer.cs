namespace Sci.Production.Quality
{
    partial class R32
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.comboStage = new Sci.Win.UI.ComboBox();
            this.label8 = new Sci.Win.UI.Label();
            this.AuditDate = new Sci.Win.UI.DateRange();
            this.dateBuyerDev = new Sci.Win.UI.DateRange();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.comboM = new Sci.Production.Class.ComboMDivision(this.components);
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtSP_s = new Sci.Win.UI.TextBox();
            this.txtSP_e = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(500, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(500, 48);
            this.toexcel.TabIndex = 9;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(500, 84);
            this.close.TabIndex = 10;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(454, 147);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(480, 182);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(480, 209);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.comboStage);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.AuditDate);
            this.groupBox1.Controls.Add(this.dateBuyerDev);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboFactory);
            this.groupBox1.Controls.Add(this.comboM);
            this.groupBox1.Controls.Add(this.txtBrand);
            this.groupBox1.Controls.Add(this.txtSP_s);
            this.groupBox1.Controls.Add(this.txtSP_e);
            this.groupBox1.Location = new System.Drawing.Point(28, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 300);
            this.groupBox1.TabIndex = 109;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 99);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.BorderWidth = 1F;
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.RectStyle.ExtBorderWidth = 1F;
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 114;
            this.label1.Text = "SP#";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(17, 63);
            this.label7.Name = "label7";
            this.label7.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.RectStyle.BorderWidth = 1F;
            this.label7.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label7.RectStyle.ExtBorderWidth = 1F;
            this.label7.Size = new System.Drawing.Size(99, 23);
            this.label7.TabIndex = 113;
            this.label7.Text = "Buyer Delivery";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(17, 25);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(99, 23);
            this.labelSCIDelivery.TabIndex = 112;
            this.labelSCIDelivery.Text = "Audit Date";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboStage
            // 
            this.comboStage.BackColor = System.Drawing.Color.White;
            this.comboStage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStage.FormattingEnabled = true;
            this.comboStage.IsSupportUnselect = true;
            this.comboStage.Items.AddRange(new object[] {
            "",
            "Inline",
            "Staggered",
            "Final",
            "3rd party"});
            this.comboStage.Location = new System.Drawing.Point(128, 250);
            this.comboStage.Name = "comboStage";
            this.comboStage.OldText = "";
            this.comboStage.Size = new System.Drawing.Size(100, 24);
            this.comboStage.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(17, 250);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 23);
            this.label8.TabIndex = 111;
            this.label8.Text = "Insp. Stage";
            // 
            // AuditDate
            // 
            // 
            // 
            // 
            this.AuditDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.AuditDate.DateBox1.Name = "";
            this.AuditDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.AuditDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.AuditDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.AuditDate.DateBox2.Name = "";
            this.AuditDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.AuditDate.DateBox2.TabIndex = 1;
            this.AuditDate.Location = new System.Drawing.Point(128, 25);
            this.AuditDate.Name = "AuditDate";
            this.AuditDate.Size = new System.Drawing.Size(280, 23);
            this.AuditDate.TabIndex = 1;
            // 
            // dateBuyerDev
            // 
            // 
            // 
            // 
            this.dateBuyerDev.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDev.DateBox1.Name = "";
            this.dateBuyerDev.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDev.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDev.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDev.DateBox2.Name = "";
            this.dateBuyerDev.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDev.DateBox2.TabIndex = 1;
            this.dateBuyerDev.Location = new System.Drawing.Point(128, 63);
            this.dateBuyerDev.Name = "dateBuyerDev";
            this.dateBuyerDev.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDev.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(234, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 107;
            this.label6.Text = "~";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(17, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(17, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "M";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(17, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 23);
            this.label5.TabIndex = 101;
            this.label5.Text = "Brand";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(128, 174);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 6;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(128, 135);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 5;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(128, 211);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 7;
            // 
            // txtSP_s
            // 
            this.txtSP_s.BackColor = System.Drawing.Color.White;
            this.txtSP_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_s.Location = new System.Drawing.Point(128, 99);
            this.txtSP_s.Name = "txtSP_s";
            this.txtSP_s.Size = new System.Drawing.Size(100, 23);
            this.txtSP_s.TabIndex = 3;
            // 
            // txtSP_e
            // 
            this.txtSP_e.BackColor = System.Drawing.Color.White;
            this.txtSP_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_e.Location = new System.Drawing.Point(256, 99);
            this.txtSP_e.Name = "txtSP_e";
            this.txtSP_e.Size = new System.Drawing.Size(100, 23);
            this.txtSP_e.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(488, 119);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 22);
            this.label10.TabIndex = 110;
            this.label10.Text = "Paper Size A4";
            // 
            // R32
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 359);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.Name = "R32";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R32. CFA Inspection Record Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label label8;
        private Win.UI.DateRange AuditDate;
        private Win.UI.DateRange dateBuyerDev;
        private System.Windows.Forms.Label label6;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Class.ComboFactory comboFactory;
        private Class.ComboMDivision comboM;
        private Class.Txtbrand txtBrand;
        private Win.UI.TextBox txtSP_s;
        private Win.UI.TextBox txtSP_e;
        private Win.UI.ComboBox comboStage;
        private Win.UI.Label label10;
        private Win.UI.Label label1;
        private Win.UI.Label label7;
        private Win.UI.Label labelSCIDelivery;
    }
}