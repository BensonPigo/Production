namespace Sci.Production.Cutting
{
    partial class R01
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
            this.labelCuttingDate = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.SewingDate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(454, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(454, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(454, 84);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(311, 19);
            this.buttonCustomized.Size = new System.Drawing.Size(100, 30);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(311, 27);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(311, 12);
            // 
            // labelCuttingDate
            // 
            this.labelCuttingDate.Location = new System.Drawing.Point(9, 19);
            this.labelCuttingDate.Name = "labelCuttingDate";
            this.labelCuttingDate.Size = new System.Drawing.Size(93, 23);
            this.labelCuttingDate.TabIndex = 98;
            this.labelCuttingDate.Text = "M";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "Factory";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label2.Location = new System.Drawing.Point(9, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "Sewing Date";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(105, 19);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 1;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.boolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(105, 55);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 2;
            // 
            // SewingDate
            // 
            // 
            // 
            // 
            this.SewingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.SewingDate.DateBox1.Name = "";
            this.SewingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.SewingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.SewingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.SewingDate.DateBox2.Name = "";
            this.SewingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.SewingDate.DateBox2.TabIndex = 1;
            this.SewingDate.Location = new System.Drawing.Point(105, 91);
            this.SewingDate.Name = "SewingDate";
            this.SewingDate.Size = new System.Drawing.Size(280, 23);
            this.SewingDate.TabIndex = 3;
            // 
            // R01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 167);
            this.Controls.Add(this.SewingDate);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCuttingDate);
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Planned Cutting WIP Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelCuttingDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.SewingDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCuttingDate;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Class.txtMdivision txtMdivision;
        private Class.txtfactory txtfactory;
        private Win.UI.DateRange SewingDate;
    }
}