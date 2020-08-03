namespace Sci.Production.Basic
{
    partial class R01
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
            this.label4 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dateAdd = new Sci.Win.UI.DateRange();
            this.dateApv = new Sci.Win.UI.DateRange();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(458, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(458, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(458, 84);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(324, 160);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(324, 143);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(26, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 23);
            this.label4.TabIndex = 110;
            this.label4.Text = "Add Date";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(26, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 23);
            this.label1.TabIndex = 111;
            this.label1.Text = "Status";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 23);
            this.label2.TabIndex = 112;
            this.label2.Text = "Code";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(26, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 23);
            this.label3.TabIndex = 113;
            this.label3.Text = "Approve Date";
            // 
            // dateAdd
            // 
            // 
            // 
            // 
            this.dateAdd.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateAdd.DateBox1.Name = "";
            this.dateAdd.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateAdd.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateAdd.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateAdd.DateBox2.Name = "";
            this.dateAdd.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateAdd.DateBox2.TabIndex = 1;
            this.dateAdd.IsRequired = false;
            this.dateAdd.Location = new System.Drawing.Point(144, 30);
            this.dateAdd.Name = "dateAdd";
            this.dateAdd.Size = new System.Drawing.Size(280, 23);
            this.dateAdd.TabIndex = 1;
            // 
            // dateApv
            // 
            // 
            // 
            // 
            this.dateApv.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApv.DateBox1.Name = "";
            this.dateApv.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApv.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApv.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApv.DateBox2.Name = "";
            this.dateApv.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApv.DateBox2.TabIndex = 1;
            this.dateApv.IsRequired = false;
            this.dateApv.Location = new System.Drawing.Point(144, 66);
            this.dateApv.Name = "dateApv";
            this.dateApv.Size = new System.Drawing.Size(280, 23);
            this.dateApv.TabIndex = 2;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(144, 103);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(122, 23);
            this.txtCode.TabIndex = 3;
            this.txtCode.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCode_PopUp);
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Location = new System.Drawing.Point(145, 140);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 4;
            // 
            // R01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 222);
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.dateApv);
            this.Controls.Add(this.dateAdd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.IsSupportPrint = false;
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Supplier Bank Detail List (Local)";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateAdd, 0);
            this.Controls.SetChildIndex(this.dateApv, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.comboStatus, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateAdd;
        private Win.UI.DateRange dateApv;
        private Win.UI.TextBox txtCode;
        private Win.UI.ComboBox comboStatus;
    }
}