namespace Sci.Production.Warehouse
{
    partial class R12
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
            this.lblIssuedate = new Sci.Win.UI.Label();
            this.lblFabricType = new Sci.Win.UI.Label();
            this.lblRequestType = new Sci.Win.UI.Label();
            this.lblM = new Sci.Win.UI.Label();
            this.lblFactory = new Sci.Win.UI.Label();
            this.dateRangeIssueDate = new Sci.Win.UI.DateRange();
            this.cbFabricType = new Sci.Win.UI.ComboBox();
            this.cbRequestType = new Sci.Win.UI.ComboBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(436, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(436, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(436, 84);
            // 
            // lblIssuedate
            // 
            this.lblIssuedate.Location = new System.Drawing.Point(9, 12);
            this.lblIssuedate.Name = "lblIssuedate";
            this.lblIssuedate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lblIssuedate.Size = new System.Drawing.Size(97, 23);
            this.lblIssuedate.TabIndex = 97;
            this.lblIssuedate.Text = "Issue Date";
            this.lblIssuedate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lblFabricType
            // 
            this.lblFabricType.Location = new System.Drawing.Point(9, 47);
            this.lblFabricType.Name = "lblFabricType";
            this.lblFabricType.Size = new System.Drawing.Size(97, 23);
            this.lblFabricType.TabIndex = 98;
            this.lblFabricType.Text = "Fabric Type";
            // 
            // lblRequestType
            // 
            this.lblRequestType.Location = new System.Drawing.Point(9, 82);
            this.lblRequestType.Name = "lblRequestType";
            this.lblRequestType.Size = new System.Drawing.Size(97, 23);
            this.lblRequestType.TabIndex = 99;
            this.lblRequestType.Text = "Request Type";
            // 
            // lblM
            // 
            this.lblM.Location = new System.Drawing.Point(9, 117);
            this.lblM.Name = "lblM";
            this.lblM.Size = new System.Drawing.Size(97, 23);
            this.lblM.TabIndex = 100;
            this.lblM.Text = "M";
            // 
            // lblFactory
            // 
            this.lblFactory.Location = new System.Drawing.Point(9, 152);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(97, 23);
            this.lblFactory.TabIndex = 101;
            this.lblFactory.Text = "Factory";
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
            this.dateRangeIssueDate.Location = new System.Drawing.Point(109, 12);
            this.dateRangeIssueDate.Name = "dateRangeIssueDate";
            this.dateRangeIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeIssueDate.TabIndex = 102;
            // 
            // cbFabricType
            // 
            this.cbFabricType.BackColor = System.Drawing.Color.White;
            this.cbFabricType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbFabricType.FormattingEnabled = true;
            this.cbFabricType.IsSupportUnselect = true;
            this.cbFabricType.Items.AddRange(new object[] {
            "All",
            "Fabric",
            "Accessory"});
            this.cbFabricType.Location = new System.Drawing.Point(109, 47);
            this.cbFabricType.Name = "cbFabricType";
            this.cbFabricType.OldText = "";
            this.cbFabricType.Size = new System.Drawing.Size(121, 24);
            this.cbFabricType.TabIndex = 103;
            // 
            // cbRequestType
            // 
            this.cbRequestType.BackColor = System.Drawing.Color.White;
            this.cbRequestType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbRequestType.FormattingEnabled = true;
            this.cbRequestType.IsSupportUnselect = true;
            this.cbRequestType.Items.AddRange(new object[] {
            "All",
            "Lacking",
            "Replacement"});
            this.cbRequestType.Location = new System.Drawing.Point(109, 82);
            this.cbRequestType.Name = "cbRequestType";
            this.cbRequestType.OldText = "";
            this.cbRequestType.Size = new System.Drawing.Size(121, 24);
            this.cbRequestType.TabIndex = 104;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(109, 117);
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
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(109, 152);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 106;
            // 
            // R12
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 225);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.cbRequestType);
            this.Controls.Add(this.cbFabricType);
            this.Controls.Add(this.dateRangeIssueDate);
            this.Controls.Add(this.lblFactory);
            this.Controls.Add(this.lblM);
            this.Controls.Add(this.lblRequestType);
            this.Controls.Add(this.lblFabricType);
            this.Controls.Add(this.lblIssuedate);
            this.Name = "R12";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R12. Issue Lacking & Replacement Transaction List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lblIssuedate, 0);
            this.Controls.SetChildIndex(this.lblFabricType, 0);
            this.Controls.SetChildIndex(this.lblRequestType, 0);
            this.Controls.SetChildIndex(this.lblM, 0);
            this.Controls.SetChildIndex(this.lblFactory, 0);
            this.Controls.SetChildIndex(this.dateRangeIssueDate, 0);
            this.Controls.SetChildIndex(this.cbFabricType, 0);
            this.Controls.SetChildIndex(this.cbRequestType, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lblIssuedate;
        private Win.UI.Label lblFabricType;
        private Win.UI.Label lblRequestType;
        private Win.UI.Label lblM;
        private Win.UI.Label lblFactory;
        private Win.UI.DateRange dateRangeIssueDate;
        private Win.UI.ComboBox cbFabricType;
        private Win.UI.ComboBox cbRequestType;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
    }
}