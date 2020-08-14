namespace Sci.Production.Warehouse
{
    partial class R27
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateRangeIssue = new Sci.Win.UI.DateRange();
            this.txtSPFrom = new Sci.Win.UI.TextBox();
            this.txtSPTo = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.comboIssueType = new Sci.Win.UI.ComboBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(191, 129);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(396, 8);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(396, 44);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(254, 85);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Aqua;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Issue Date";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 40);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "SP#";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Issue Type";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "M";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 101;
            this.label5.Text = "Factory";
            // 
            // dateRangeIssue
            // 
            // 
            // 
            // 
            this.dateRangeIssue.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeIssue.DateBox1.Name = "";
            this.dateRangeIssue.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeIssue.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeIssue.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeIssue.DateBox2.Name = "";
            this.dateRangeIssue.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeIssue.DateBox2.TabIndex = 1;
            this.dateRangeIssue.IsRequired = false;
            this.dateRangeIssue.Location = new System.Drawing.Point(87, 8);
            this.dateRangeIssue.Name = "dateRangeIssue";
            this.dateRangeIssue.Size = new System.Drawing.Size(280, 23);
            this.dateRangeIssue.TabIndex = 102;
            // 
            // txtSPFrom
            // 
            this.txtSPFrom.BackColor = System.Drawing.Color.White;
            this.txtSPFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPFrom.Location = new System.Drawing.Point(87, 40);
            this.txtSPFrom.Name = "txtSPFrom";
            this.txtSPFrom.Size = new System.Drawing.Size(128, 23);
            this.txtSPFrom.TabIndex = 103;
            // 
            // txtSPTo
            // 
            this.txtSPTo.BackColor = System.Drawing.Color.White;
            this.txtSPTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPTo.Location = new System.Drawing.Point(237, 40);
            this.txtSPTo.Name = "txtSPTo";
            this.txtSPTo.Size = new System.Drawing.Size(130, 23);
            this.txtSPTo.TabIndex = 104;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(212, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 23);
            this.label6.TabIndex = 105;
            this.label6.Text = " ～";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboIssueType
            // 
            this.comboIssueType.BackColor = System.Drawing.Color.White;
            this.comboIssueType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboIssueType.FormattingEnabled = true;
            this.comboIssueType.IsSupportUnselect = true;
            this.comboIssueType.Items.AddRange(new object[] {
            "All",
            "Sewing",
            "Packing"});
            this.comboIssueType.Location = new System.Drawing.Point(87, 72);
            this.comboIssueType.Name = "comboIssueType";
            this.comboIssueType.OldText = "";
            this.comboIssueType.Size = new System.Drawing.Size(121, 24);
            this.comboIssueType.TabIndex = 106;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(87, 104);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 107;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(87, 136);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 108;
            // 
            // R27
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 205);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.comboIssueType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSPTo);
            this.Controls.Add(this.txtSPFrom);
            this.Controls.Add(this.dateRangeIssue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R27";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R27. Issue Sewing/Packing Material";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateRangeIssue, 0);
            this.Controls.SetChildIndex(this.txtSPFrom, 0);
            this.Controls.SetChildIndex(this.txtSPTo, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.comboIssueType, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DateRange dateRangeIssue;
        private Win.UI.TextBox txtSPFrom;
        private Win.UI.TextBox txtSPTo;
        private Win.UI.Label label6;
        private Win.UI.ComboBox comboIssueType;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
    }
}