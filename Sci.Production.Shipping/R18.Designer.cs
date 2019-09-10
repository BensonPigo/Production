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
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(396, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(396, 48);
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
            this.chkIncludeJunk.TabIndex = 99;
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
            this.txtShippingExpenseID_s.TabIndex = 100;
            // 
            // txtShippingExpenseID_e
            // 
            this.txtShippingExpenseID_e.BackColor = System.Drawing.Color.White;
            this.txtShippingExpenseID_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShippingExpenseID_e.Location = new System.Drawing.Point(260, 19);
            this.txtShippingExpenseID_e.Name = "txtShippingExpenseID_e";
            this.txtShippingExpenseID_e.Size = new System.Drawing.Size(118, 23);
            this.txtShippingExpenseID_e.TabIndex = 101;
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
            // txtsubcon1
            // 
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = true;
            this.txtsubcon1.isMisc = false;
            this.txtsubcon1.isShipping = true;
            this.txtsubcon1.isSubcon = false;
            this.txtsubcon1.Location = new System.Drawing.Point(114, 55);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(170, 23);
            this.txtsubcon1.TabIndex = 103;
            this.txtsubcon1.TextBox1Binding = "";
            // 
            // R18
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 161);
            this.Controls.Add(this.txtsubcon1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtShippingExpenseID_e);
            this.Controls.Add(this.txtShippingExpenseID_s);
            this.Controls.Add(this.chkIncludeJunk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R18";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R18. Shipping expense list";
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
            this.Controls.SetChildIndex(this.txtsubcon1, 0);
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
        private Class.txtsubcon txtsubcon1;
    }
}