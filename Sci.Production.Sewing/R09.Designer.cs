namespace Sci.Production.Sewing
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dateConfirm = new Sci.Win.UI.DateRange();
            this.chkOnlyshowBalanceQty = new Sci.Win.UI.CheckBox();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.label4 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(406, 84);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(406, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(406, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(360, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(386, 129);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(386, 129);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Confirm Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(22, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(22, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Factory";
            // 
            // dateConfirm
            // 
            // 
            // 
            // 
            this.dateConfirm.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateConfirm.DateBox1.Name = "";
            this.dateConfirm.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateConfirm.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateConfirm.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateConfirm.DateBox2.Name = "";
            this.dateConfirm.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateConfirm.DateBox2.TabIndex = 1;
            this.dateConfirm.DateBox2.Validating += new System.ComponentModel.CancelEventHandler(this.DateRange1_DateBox2_Validating);
            this.dateConfirm.IsRequired = false;
            this.dateConfirm.Location = new System.Drawing.Point(120, 12);
            this.dateConfirm.Name = "dateConfirm";
            this.dateConfirm.Size = new System.Drawing.Size(280, 23);
            this.dateConfirm.TabIndex = 0;
            // 
            // chkOnlyshowBalanceQty
            // 
            this.chkOnlyshowBalanceQty.AutoSize = true;
            this.chkOnlyshowBalanceQty.Checked = true;
            this.chkOnlyshowBalanceQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnlyshowBalanceQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOnlyshowBalanceQty.Location = new System.Drawing.Point(22, 129);
            this.chkOnlyshowBalanceQty.Name = "chkOnlyshowBalanceQty";
            this.chkOnlyshowBalanceQty.Size = new System.Drawing.Size(419, 21);
            this.chkOnlyshowBalanceQty.TabIndex = 3;
            this.chkOnlyshowBalanceQty.Text = "Only show Balance Qty>0 (Should transfer output to new SP#)";
            this.chkOnlyshowBalanceQty.UseVisualStyleBackColor = true;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(120, 70);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 2;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(120, 41);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(22, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "Affected SP#";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(242, 99);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 23);
            this.label9.TabIndex = 106;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(120, 99);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(116, 23);
            this.txtSP1.TabIndex = 107;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(266, 99);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(116, 23);
            this.txtSP2.TabIndex = 108;
            // 
            // R09
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 189);
            this.Controls.Add(this.chkOnlyshowBalanceQty);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.dateConfirm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R09";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R09. Order Qty Transfer List";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateConfirm, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.chkOnlyshowBalanceQty, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateConfirm;
        private Class.TxtMdivision txtMdivision1;
        private Class.Txtfactory txtfactory1;
        private Win.UI.CheckBox chkOnlyshowBalanceQty;
        private Win.UI.Label label4;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtSP1;
        private Win.UI.TextBox txtSP2;
    }
}