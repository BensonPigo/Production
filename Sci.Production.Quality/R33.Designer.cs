namespace Sci.Production.Quality
{
    partial class R33
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.comboStage = new Sci.Win.UI.ComboBox();
            this.label8 = new Sci.Win.UI.Label();
            this.AuditDate = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.comboM = new Sci.Production.Class.ComboMDivision(this.components);
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(445, 19);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(445, 55);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(445, 91);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(409, 165);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(435, 165);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(391, 169);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.comboStage);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.AuditDate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboFactory);
            this.groupBox1.Controls.Add(this.comboM);
            this.groupBox1.Controls.Add(this.txtBrand);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 215);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
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
            this.comboStage.Enabled = false;
            this.comboStage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStage.FormattingEnabled = true;
            this.comboStage.IsSupportUnselect = true;
            this.comboStage.Items.AddRange(new object[] {
            "Staggered"});
            this.comboStage.Location = new System.Drawing.Point(128, 175);
            this.comboStage.Name = "comboStage";
            this.comboStage.OldText = "";
            this.comboStage.Size = new System.Drawing.Size(100, 24);
            this.comboStage.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label8.Location = new System.Drawing.Point(17, 175);
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
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label3.Location = new System.Drawing.Point(17, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.Location = new System.Drawing.Point(17, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "M";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label5.Location = new System.Drawing.Point(17, 135);
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
            this.comboFactory.Location = new System.Drawing.Point(128, 99);
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
            this.comboM.Location = new System.Drawing.Point(128, 60);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 5;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(128, 136);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 7;
            // 
            // R33
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 269);
            this.Controls.Add(this.groupBox1);
            this.IsSupportPrint = false;
            this.Name = "R33";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R33. CFA insp. Final and inline Pass rate summary";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.ComboBox comboStage;
        private Win.UI.Label label8;
        private Win.UI.DateRange AuditDate;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Class.ComboFactory comboFactory;
        private Class.ComboMDivision comboM;
        private Class.Txtbrand txtBrand;
    }
}