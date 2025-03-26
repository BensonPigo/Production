namespace Sci.Production.PPIC
{
    partial class R23
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateRangeSewDate = new Sci.Win.UI.DateRange();
            this.dateRangeBDate = new Sci.Win.UI.DateRange();
            this.dropDownListTableAdapter1 = new Sci.Production.Planning.GSchemas.GLOTableAdapters.DropDownListTableAdapter();
            this.txtSP1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSP2 = new System.Windows.Forms.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(426, 160);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(426, 12);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(426, 48);
            this.close.TabIndex = 9;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(400, 131);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Sewing Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Buyer Delivery";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 23);
            this.label5.TabIndex = 101;
            this.label5.Text = "SP#";
            // 
            // dateRangeSewDate
            // 
            // 
            // 
            // 
            this.dateRangeSewDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeSewDate.DateBox1.Name = "";
            this.dateRangeSewDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSewDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeSewDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeSewDate.DateBox2.Name = "";
            this.dateRangeSewDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSewDate.DateBox2.TabIndex = 1;
            this.dateRangeSewDate.IsRequired = false;
            this.dateRangeSewDate.Location = new System.Drawing.Point(115, 12);
            this.dateRangeSewDate.Name = "dateRangeSewDate";
            this.dateRangeSewDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeSewDate.TabIndex = 0;
            // 
            // dateRangeBDate
            // 
            // 
            // 
            // 
            this.dateRangeBDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeBDate.DateBox1.Name = "";
            this.dateRangeBDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeBDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeBDate.DateBox2.Name = "";
            this.dateRangeBDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBDate.DateBox2.TabIndex = 1;
            this.dateRangeBDate.IsRequired = false;
            this.dateRangeBDate.Location = new System.Drawing.Point(115, 41);
            this.dateRangeBDate.Name = "dateRangeBDate";
            this.dateRangeBDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBDate.TabIndex = 1;
            // 
            // dropDownListTableAdapter1
            // 
            this.dropDownListTableAdapter1.ClearBeforeFill = true;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(115, 74);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(100, 23);
            this.txtSP1.TabIndex = 111;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(221, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 17);
            this.label3.TabIndex = 112;
            this.label3.Text = "～";
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(244, 74);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(100, 23);
            this.txtSP2.TabIndex = 113;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 23);
            this.label4.TabIndex = 114;
            this.label4.Text = "Style";
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(115, 107);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.SeasonObjectName = null;
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 116;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // R23
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 219);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.dateRangeBDate);
            this.Controls.Add(this.dateRangeSewDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R23";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R23. Accessory Consumption Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateRangeSewDate, 0);
            this.Controls.SetChildIndex(this.dateRangeBDate, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label5;
        private Win.UI.DateRange dateRangeSewDate;
        private Win.UI.DateRange dateRangeBDate;
        private Planning.GSchemas.GLOTableAdapters.DropDownListTableAdapter dropDownListTableAdapter1;
        private System.Windows.Forms.TextBox txtSP1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSP2;
        private Win.UI.Label label4;
        private Class.Txtstyle txtstyle;
    }
}