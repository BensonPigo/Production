namespace Sci.Production.Warehouse
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
            this.lblIssueDate = new Sci.Win.UI.Label();
            this.lblSP = new Sci.Win.UI.Label();
            this.lblM = new Sci.Win.UI.Label();
            this.lblFactory = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateRangeIssueDate = new Sci.Win.UI.DateRange();
            this.txtSPFrom = new Sci.Win.UI.TextBox();
            this.txtSPTo = new Sci.Win.UI.TextBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(246, 117);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(436, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(436, 48);
            // 
            // lblIssueDate
            // 
            this.lblIssueDate.BackColor = System.Drawing.Color.SkyBlue;
            this.lblIssueDate.Location = new System.Drawing.Point(18, 12);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.Size = new System.Drawing.Size(75, 23);
            this.lblIssueDate.TabIndex = 97;
            this.lblIssueDate.Text = "Issue Date";
            this.lblIssueDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lblSP
            // 
            this.lblSP.BackColor = System.Drawing.Color.SkyBlue;
            this.lblSP.Location = new System.Drawing.Point(18, 48);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(75, 23);
            this.lblSP.TabIndex = 98;
            this.lblSP.Text = "SP#";
            this.lblSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lblM
            // 
            this.lblM.Location = new System.Drawing.Point(18, 80);
            this.lblM.Name = "lblM";
            this.lblM.Size = new System.Drawing.Size(75, 23);
            this.lblM.TabIndex = 99;
            this.lblM.Text = "M";
            // 
            // lblFactory
            // 
            this.lblFactory.Location = new System.Drawing.Point(18, 117);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(75, 23);
            this.lblFactory.TabIndex = 100;
            this.lblFactory.Text = "Factory";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(216, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 23);
            this.label5.TabIndex = 101;
            this.label5.Text = " ～";
            // 
            // dateRangeIssueDate
            // 
            // 
            // 
            // 
            this.dateRangeIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeIssueDate.DateBox1.Name = "";
            this.dateRangeIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeIssueDate.DateBox2.Name = "";
            this.dateRangeIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeIssueDate.DateBox2.TabIndex = 1;
            this.dateRangeIssueDate.Location = new System.Drawing.Point(96, 12);
            this.dateRangeIssueDate.Name = "dateRangeIssueDate";
            this.dateRangeIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeIssueDate.TabIndex = 102;
            // 
            // txtSPFrom
            // 
            this.txtSPFrom.BackColor = System.Drawing.Color.White;
            this.txtSPFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPFrom.Location = new System.Drawing.Point(96, 48);
            this.txtSPFrom.Name = "txtSPFrom";
            this.txtSPFrom.Size = new System.Drawing.Size(117, 23);
            this.txtSPFrom.TabIndex = 103;
            // 
            // txtSPTo
            // 
            this.txtSPTo.BackColor = System.Drawing.Color.White;
            this.txtSPTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPTo.Location = new System.Drawing.Point(246, 48);
            this.txtSPTo.Name = "txtSPTo";
            this.txtSPTo.Size = new System.Drawing.Size(131, 23);
            this.txtSPTo.TabIndex = 104;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(96, 80);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 105;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(96, 117);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 106;
            // 
            // R33
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 186);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtSPTo);
            this.Controls.Add(this.txtSPFrom);
            this.Controls.Add(this.dateRangeIssueDate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblFactory);
            this.Controls.Add(this.lblM);
            this.Controls.Add(this.lblSP);
            this.Controls.Add(this.lblIssueDate);
            this.Name = "R33";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R33. Issue Thread Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lblIssueDate, 0);
            this.Controls.SetChildIndex(this.lblSP, 0);
            this.Controls.SetChildIndex(this.lblM, 0);
            this.Controls.SetChildIndex(this.lblFactory, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateRangeIssueDate, 0);
            this.Controls.SetChildIndex(this.txtSPFrom, 0);
            this.Controls.SetChildIndex(this.txtSPTo, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lblIssueDate;
        private Win.UI.Label lblSP;
        private Win.UI.Label lblM;
        private Win.UI.Label lblFactory;
        private Win.UI.Label label5;
        private Win.UI.DateRange dateRangeIssueDate;
        private Win.UI.TextBox txtSPFrom;
        private Win.UI.TextBox txtSPTo;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
    }
}