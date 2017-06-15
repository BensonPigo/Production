namespace Sci.Production.Subcon
{
    partial class R51
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelLocalSupplier = new Sci.Win.UI.Label();
            this.textID = new Sci.Win.UI.TextBox();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.txtLocalSupp1 = new Sci.Production.Class.txtLocalSupp();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(422, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(422, 12);
            this.toexcel.TabIndex = 4;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(422, 48);
            this.close.TabIndex = 5;
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(9, 9);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(96, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(9, 41);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(96, 23);
            this.labelID.TabIndex = 94;
            this.labelID.Text = "ID";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 72);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(96, 23);
            this.labelFactory.TabIndex = 94;
            this.labelFactory.Text = "Factory";
            // 
            // labelLocalSupplier
            // 
            this.labelLocalSupplier.Location = new System.Drawing.Point(9, 103);
            this.labelLocalSupplier.Name = "labelLocalSupplier";
            this.labelLocalSupplier.Size = new System.Drawing.Size(96, 23);
            this.labelLocalSupplier.TabIndex = 94;
            this.labelLocalSupplier.Text = "Local Supplier";
            // 
            // textID
            // 
            this.textID.BackColor = System.Drawing.Color.White;
            this.textID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textID.Location = new System.Drawing.Point(109, 41);
            this.textID.Name = "textID";
            this.textID.Size = new System.Drawing.Size(129, 23);
            this.textID.TabIndex = 1;
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(108, 9);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(280, 23);
            this.dateDate.TabIndex = 0;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(109, 71);
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 2;
            // 
            // txtLocalSupp1
            // 
            this.txtLocalSupp1.DisplayBox1Binding = "";
            this.txtLocalSupp1.Location = new System.Drawing.Point(108, 103);
            this.txtLocalSupp1.Name = "txtLocalSupp1";
            this.txtLocalSupp1.Size = new System.Drawing.Size(252, 23);
            this.txtLocalSupp1.TabIndex = 3;
            this.txtLocalSupp1.TextBox1Binding = "";
            // 
            // R51
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 166);
            this.Controls.Add(this.txtLocalSupp1);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.textID);
            this.Controls.Add(this.labelLocalSupplier);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelID);
            this.Controls.Add(this.labelDate);
            this.Name = "R51";
            this.Text = "R51. Transfer Subcon PO Data To Printing System";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelID, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelLocalSupplier, 0);
            this.Controls.SetChildIndex(this.textID, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.txtLocalSupp1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelID;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelLocalSupplier;
        private Win.UI.TextBox textID;
        private Win.UI.DateRange dateDate;
        private Class.txtfactory txtfactory1;
        private Class.txtLocalSupp txtLocalSupp1;
    }
}