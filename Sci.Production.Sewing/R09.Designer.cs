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
            this.chkNotComplete = new Sci.Win.UI.CheckBox();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(406, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(406, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(406, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(360, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(386, 156);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(386, 183);
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
            this.label2.Location = new System.Drawing.Point(22, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(22, 84);
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
            this.dateConfirm.Location = new System.Drawing.Point(120, 12);
            this.dateConfirm.Name = "dateConfirm";
            this.dateConfirm.Size = new System.Drawing.Size(280, 23);
            this.dateConfirm.TabIndex = 0;
            // 
            // chkNotComplete
            // 
            this.chkNotComplete.AutoSize = true;
            this.chkNotComplete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNotComplete.Location = new System.Drawing.Point(22, 120);
            this.chkNotComplete.Name = "chkNotComplete";
            this.chkNotComplete.Size = new System.Drawing.Size(224, 21);
            this.chkNotComplete.TabIndex = 3;
            this.chkNotComplete.Text = "Sewing output not yet complete";
            this.chkNotComplete.UseVisualStyleBackColor = true;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.boolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(120, 84);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 2;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(120, 48);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 1;
            // 
            // R09
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 230);
            this.Controls.Add(this.chkNotComplete);
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
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateConfirm, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.chkNotComplete, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateConfirm;
        private Class.txtMdivision txtMdivision1;
        private Class.txtfactory txtfactory1;
        private Win.UI.CheckBox chkNotComplete;
    }
}