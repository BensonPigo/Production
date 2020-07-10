namespace Sci.Production.PPIC
{
    partial class R09
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
            this.labelUpdatedDate = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.dateUpdate = new Sci.Win.UI.DateRange();
            this.textSP = new Sci.Win.UI.TextBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(971, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(413, 9);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(413, 44);
            // 
            // labelUpdatedDate
            // 
            this.labelUpdatedDate.Location = new System.Drawing.Point(18, 9);
            this.labelUpdatedDate.Name = "labelUpdatedDate";
            this.labelUpdatedDate.Size = new System.Drawing.Size(99, 23);
            this.labelUpdatedDate.TabIndex = 0;
            this.labelUpdatedDate.Text = "Updated Date";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(18, 41);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(99, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(18, 73);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(99, 23);
            this.labelM.TabIndex = 0;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(18, 105);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(99, 23);
            this.labelFactory.TabIndex = 0;
            this.labelFactory.Text = "Factory";
            // 
            // dateUpdate
            // 
            this.dateUpdate.IsRequired = false;
            this.dateUpdate.Location = new System.Drawing.Point(120, 9);
            this.dateUpdate.Name = "dateUpdate";
            this.dateUpdate.Size = new System.Drawing.Size(280, 23);
            this.dateUpdate.TabIndex = 1;
            // 
            // textSP
            // 
            this.textSP.BackColor = System.Drawing.Color.White;
            this.textSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSP.Location = new System.Drawing.Point(120, 41);
            this.textSP.Name = "textSP";
            this.textSP.Size = new System.Drawing.Size(100, 23);
            this.textSP.TabIndex = 2;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(120, 73);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 3;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = false;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(120, 105);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // R09
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 174);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.textSP);
            this.Controls.Add(this.dateUpdate);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSP);
            this.Controls.Add(this.labelUpdatedDate);
            this.Name = "R09";
            this.Text = "R09. Comparison List Report";
            this.Controls.SetChildIndex(this.labelUpdatedDate, 0);
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.dateUpdate, 0);
            this.Controls.SetChildIndex(this.textSP, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelUpdatedDate;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.TextBox textSP;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Win.UI.DateRange dateUpdate;
    }
}