namespace Sci.Production.Shipping
{
    partial class R18
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
            this.chkIncludeJunk = new Sci.Win.UI.CheckBox();
            this.txtShippingExpenseID_s = new Sci.Win.UI.TextBox();
            this.txtShippingExpenseID_e = new Sci.Win.UI.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtsubcon = new Sci.Production.Class.TxtsubconNoConfirm();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(396, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(396, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(396, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(260, 93);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(272, 102);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(286, 102);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Code";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(27, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Supplier";
            // 
            // chkIncludeJunk
            // 
            this.chkIncludeJunk.AutoSize = true;
            this.chkIncludeJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeJunk.Location = new System.Drawing.Point(27, 93);
            this.chkIncludeJunk.Name = "chkIncludeJunk";
            this.chkIncludeJunk.Size = new System.Drawing.Size(106, 21);
            this.chkIncludeJunk.TabIndex = 4;
            this.chkIncludeJunk.Text = "Include Junk";
            this.chkIncludeJunk.UseVisualStyleBackColor = true;
            // 
            // txtShippingExpenseID_s
            // 
            this.txtShippingExpenseID_s.BackColor = System.Drawing.Color.White;
            this.txtShippingExpenseID_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShippingExpenseID_s.Location = new System.Drawing.Point(114, 19);
            this.txtShippingExpenseID_s.Name = "txtShippingExpenseID_s";
            this.txtShippingExpenseID_s.Size = new System.Drawing.Size(118, 23);
            this.txtShippingExpenseID_s.TabIndex = 1;
            this.txtShippingExpenseID_s.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtShippingExpenseID_s_PopUp);
            // 
            // txtShippingExpenseID_e
            // 
            this.txtShippingExpenseID_e.BackColor = System.Drawing.Color.White;
            this.txtShippingExpenseID_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShippingExpenseID_e.Location = new System.Drawing.Point(260, 19);
            this.txtShippingExpenseID_e.Name = "txtShippingExpenseID_e";
            this.txtShippingExpenseID_e.Size = new System.Drawing.Size(118, 23);
            this.txtShippingExpenseID_e.TabIndex = 2;
            this.txtShippingExpenseID_e.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtShippingExpenseID_e_PopUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 17);
            this.label3.TabIndex = 102;
            this.label3.Text = "~";
            // 
            // txtsubcon
            // 
            this.txtsubcon.DisplayBox1Binding = "";
            this.txtsubcon.IsIncludeJunk = true;
            this.txtsubcon.IsMisc = false;
            this.txtsubcon.IsShipping = true;
            this.txtsubcon.IsSubcon = false;
            this.txtsubcon.Location = new System.Drawing.Point(114, 55);
            this.txtsubcon.Name = "txtsubcon";
            this.txtsubcon.Size = new System.Drawing.Size(170, 23);
            this.txtsubcon.TabIndex = 3;
            this.txtsubcon.TextBox1Binding = "";
            // 
            // R18
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 161);
            this.Controls.Add(this.txtsubcon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtShippingExpenseID_e);
            this.Controls.Add(this.txtShippingExpenseID_s);
            this.Controls.Add(this.chkIncludeJunk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportLocate = false;
            this.IsSupportMove = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.IsSupportToPrint = false;
            this.Name = "R18";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R18. Shipping Expense List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.chkIncludeJunk, 0);
            this.Controls.SetChildIndex(this.txtShippingExpenseID_s, 0);
            this.Controls.SetChildIndex(this.txtShippingExpenseID_e, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtsubcon, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.CheckBox chkIncludeJunk;
        private Win.UI.TextBox txtShippingExpenseID_s;
        private Win.UI.TextBox txtShippingExpenseID_e;
        private System.Windows.Forms.Label label3;
        private Class.TxtsubconNoConfirm txtsubcon;
    }
}