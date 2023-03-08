namespace Sci.Production.Warehouse
{
    partial class R46
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
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.dateIssue = new Sci.Win.UI.DateRange();
            this.lblSP = new Sci.Win.UI.Label();
            this.lblMdivision = new Sci.Win.UI.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(424, 81);
            this.print.TabIndex = 7;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(424, 9);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(424, 45);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(231, 101);
            this.buttonCustomized.TabIndex = 11;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(398, 117);
            this.checkUseCustomized.TabIndex = 8;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(270, 72);
            this.txtVersion.TabIndex = 9;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(120, 101);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(120, 72);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(253, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 23);
            this.label6.TabIndex = 10;
            this.label6.Text = "～";
            this.label6.TextStyle.Color = System.Drawing.Color.White;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(277, 44);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(123, 23);
            this.txtSP2.TabIndex = 2;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(120, 44);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(130, 23);
            this.txtSP1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 16);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(108, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "Issue Date";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
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
            this.dateIssue.Location = new System.Drawing.Point(120, 16);
            this.dateIssue.Name = "dateIssue";
            this.dateIssue.Size = new System.Drawing.Size(280, 23);
            this.dateIssue.TabIndex = 0;
            // 
            // lblSP
            // 
            this.lblSP.Location = new System.Drawing.Point(9, 44);
            this.lblSP.Name = "lblSP";
            this.lblSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lblSP.Size = new System.Drawing.Size(108, 23);
            this.lblSP.TabIndex = 13;
            this.lblSP.Text = "SP#";
            this.lblSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lblMdivision
            // 
            this.lblMdivision.Location = new System.Drawing.Point(9, 72);
            this.lblMdivision.Name = "lblMdivision";
            this.lblMdivision.Size = new System.Drawing.Size(108, 23);
            this.lblMdivision.TabIndex = 14;
            this.lblMdivision.Text = "M";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(9, 101);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(108, 23);
            this.lbFactory.TabIndex = 15;
            this.lbFactory.Text = "Factory";
            // 
            // R46
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 177);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateIssue);
            this.Controls.Add(this.lblSP);
            this.Controls.Add(this.lblMdivision);
            this.Controls.Add(this.lbFactory);
            this.Name = "R46";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R46. Remove from Scrap Whse Transaction List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.lblMdivision, 0);
            this.Controls.SetChildIndex(this.lblSP, 0);
            this.Controls.SetChildIndex(this.dateIssue, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Class.Txtfactory txtfactory;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtSP2;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateIssue;
        private Win.UI.Label lblSP;
        private Win.UI.Label lblMdivision;
        private Win.UI.Label lbFactory;
    }
}