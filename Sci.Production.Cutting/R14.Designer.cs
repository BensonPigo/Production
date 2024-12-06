namespace Sci.Production.Cutting
{
    partial class R14
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
            this.label1 = new System.Windows.Forms.Label();
            this.dateCreateDate = new Sci.Win.UI.DateBox();
            this.labGenerateDate = new Sci.Win.UI.Label();
            this.txtNLCode = new Sci.Win.UI.TextBox();
            this.txtHSCode = new Sci.Win.UI.TextBox();
            this.txtContractNo = new Sci.Win.UI.TextBox();
            this.labelNLCode = new Sci.Win.UI.Label();
            this.labelHSCode = new Sci.Win.UI.Label();
            this.labelContractNo = new Sci.Win.UI.Label();
            this.dateGenerateDate = new Sci.Win.UI.DateBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(463, 79);
            this.print.TabIndex = 7;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(463, 7);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(463, 43);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(416, 125);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(402, 134);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(420, 132);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "～";
            // 
            // dateCreateDate
            // 
            this.dateCreateDate.Location = new System.Drawing.Point(273, 108);
            this.dateCreateDate.Name = "dateCreateDate";
            this.dateCreateDate.Size = new System.Drawing.Size(130, 23);
            this.dateCreateDate.TabIndex = 4;
            // 
            // labGenerateDate
            // 
            this.labGenerateDate.Location = new System.Drawing.Point(9, 108);
            this.labGenerateDate.Name = "labGenerateDate";
            this.labGenerateDate.Size = new System.Drawing.Size(96, 23);
            this.labGenerateDate.TabIndex = 11;
            this.labGenerateDate.Text = "Generate Date";
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(109, 76);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(150, 23);
            this.txtNLCode.TabIndex = 2;
            // 
            // txtHSCode
            // 
            this.txtHSCode.BackColor = System.Drawing.Color.White;
            this.txtHSCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtHSCode.Location = new System.Drawing.Point(109, 44);
            this.txtHSCode.Name = "txtHSCode";
            this.txtHSCode.Size = new System.Drawing.Size(150, 23);
            this.txtHSCode.TabIndex = 1;
            // 
            // txtContractNo
            // 
            this.txtContractNo.BackColor = System.Drawing.Color.White;
            this.txtContractNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContractNo.Location = new System.Drawing.Point(109, 12);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(150, 23);
            this.txtContractNo.TabIndex = 0;
            this.txtContractNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtContractNo_PopUp);
            // 
            // labelNLCode
            // 
            this.labelNLCode.Location = new System.Drawing.Point(9, 76);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(96, 23);
            this.labelNLCode.TabIndex = 10;
            this.labelNLCode.Text = "Customs Code";
            // 
            // labelHSCode
            // 
            this.labelHSCode.Location = new System.Drawing.Point(9, 44);
            this.labelHSCode.Name = "labelHSCode";
            this.labelHSCode.Size = new System.Drawing.Size(96, 23);
            this.labelHSCode.TabIndex = 9;
            this.labelHSCode.Text = "HS Code";
            // 
            // labelContractNo
            // 
            this.labelContractNo.Location = new System.Drawing.Point(9, 12);
            this.labelContractNo.Name = "labelContractNo";
            this.labelContractNo.Size = new System.Drawing.Size(96, 23);
            this.labelContractNo.TabIndex = 8;
            this.labelContractNo.Text = "Contract No.";
            // 
            // dateGenerateDate
            // 
            this.dateGenerateDate.Location = new System.Drawing.Point(109, 109);
            this.dateGenerateDate.Name = "dateGenerateDate";
            this.dateGenerateDate.Size = new System.Drawing.Size(130, 23);
            this.dateGenerateDate.TabIndex = 3;
            // 
            // R14
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 181);
            this.Controls.Add(this.dateGenerateDate);
            this.Controls.Add(this.dateCreateDate);
            this.Controls.Add(this.labGenerateDate);
            this.Controls.Add(this.txtNLCode);
            this.Controls.Add(this.txtHSCode);
            this.Controls.Add(this.txtContractNo);
            this.Controls.Add(this.labelNLCode);
            this.Controls.Add(this.labelHSCode);
            this.Controls.Add(this.labelContractNo);
            this.Controls.Add(this.label1);
            this.DefaultControl = "txtContractNo";
            this.IsSupportToPrint = false;
            this.Name = "R14";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R14. Loading-Subprocess Manual Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.labelContractNo, 0);
            this.Controls.SetChildIndex(this.labelHSCode, 0);
            this.Controls.SetChildIndex(this.labelNLCode, 0);
            this.Controls.SetChildIndex(this.txtContractNo, 0);
            this.Controls.SetChildIndex(this.txtHSCode, 0);
            this.Controls.SetChildIndex(this.txtNLCode, 0);
            this.Controls.SetChildIndex(this.labGenerateDate, 0);
            this.Controls.SetChildIndex(this.dateCreateDate, 0);
            this.Controls.SetChildIndex(this.dateGenerateDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private Win.UI.DateBox dateCreateDate;
        private Win.UI.Label labGenerateDate;
        private Win.UI.TextBox txtNLCode;
        private Win.UI.TextBox txtHSCode;
        private Win.UI.TextBox txtContractNo;
        private Win.UI.Label labelNLCode;
        private Win.UI.Label labelHSCode;
        private Win.UI.Label labelContractNo;
        private Win.UI.DateBox dateGenerateDate;
    }
}