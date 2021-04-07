namespace Sci.Production.Warehouse
{
    partial class R39
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
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtSP_From = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.txtSP_To = new Sci.Win.UI.TextBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtFactory = new Sci.Production.Class.Txtfactory();
            this.txtRefnoFrom = new Sci.Win.UI.TextBox();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.radioSummary = new Sci.Win.UI.RadioButton();
            this.checkQty = new Sci.Win.UI.CheckBox();
            this.txtRefnoTo = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(415, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(415, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(415, 84);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(320, 4);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "SP#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "Refno";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 23);
            this.label5.TabIndex = 101;
            this.label5.Text = "Report Type";
            // 
            // txtSP_From
            // 
            this.txtSP_From.BackColor = System.Drawing.Color.White;
            this.txtSP_From.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_From.Location = new System.Drawing.Point(110, 19);
            this.txtSP_From.Name = "txtSP_From";
            this.txtSP_From.Size = new System.Drawing.Size(114, 24);
            this.txtSP_From.TabIndex = 102;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(227, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 31);
            this.label8.TabIndex = 103;
            this.label8.Text = "~";
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSP_To
            // 
            this.txtSP_To.BackColor = System.Drawing.Color.White;
            this.txtSP_To.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_To.Location = new System.Drawing.Point(250, 19);
            this.txtSP_To.Name = "txtSP_To";
            this.txtSP_To.Size = new System.Drawing.Size(114, 24);
            this.txtSP_To.TabIndex = 104;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(110, 48);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 24);
            this.txtMdivision.TabIndex = 105;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.BoolFtyGroupList = true;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(110, 78);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 24);
            this.txtFactory.TabIndex = 106;
            // 
            // txtRefnoFrom
            // 
            this.txtRefnoFrom.BackColor = System.Drawing.Color.White;
            this.txtRefnoFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoFrom.Location = new System.Drawing.Point(110, 109);
            this.txtRefnoFrom.Name = "txtRefnoFrom";
            this.txtRefnoFrom.Size = new System.Drawing.Size(114, 24);
            this.txtRefnoFrom.TabIndex = 107;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.Checked = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(110, 140);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(63, 22);
            this.radioDetail.TabIndex = 108;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // radioSummary
            // 
            this.radioSummary.AutoSize = true;
            this.radioSummary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummary.Location = new System.Drawing.Point(179, 140);
            this.radioSummary.Name = "radioSummary";
            this.radioSummary.Size = new System.Drawing.Size(90, 22);
            this.radioSummary.TabIndex = 109;
            this.radioSummary.Text = "Summary";
            this.radioSummary.UseVisualStyleBackColor = true;
            // 
            // checkQty
            // 
            this.checkQty.AutoSize = true;
            this.checkQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkQty.Location = new System.Drawing.Point(9, 177);
            this.checkQty.Name = "checkQty";
            this.checkQty.Size = new System.Drawing.Size(75, 22);
            this.checkQty.TabIndex = 110;
            this.checkQty.Text = "Qty > 0";
            this.checkQty.UseVisualStyleBackColor = true;
            // 
            // txtRefnoTo
            // 
            this.txtRefnoTo.BackColor = System.Drawing.Color.White;
            this.txtRefnoTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoTo.Location = new System.Drawing.Point(250, 109);
            this.txtRefnoTo.Name = "txtRefnoTo";
            this.txtRefnoTo.Size = new System.Drawing.Size(114, 24);
            this.txtRefnoTo.TabIndex = 112;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(227, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 31);
            this.label6.TabIndex = 111;
            this.label6.Text = "~";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R39
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 235);
            this.Controls.Add(this.txtRefnoTo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkQty);
            this.Controls.Add(this.radioSummary);
            this.Controls.Add(this.radioDetail);
            this.Controls.Add(this.txtRefnoFrom);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtSP_To);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSP_From);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R39";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R39. RStock List Report (Semi-finished)";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtSP_From, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txtSP_To, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.txtRefnoFrom, 0);
            this.Controls.SetChildIndex(this.radioDetail, 0);
            this.Controls.SetChildIndex(this.radioSummary, 0);
            this.Controls.SetChildIndex(this.checkQty, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtRefnoTo, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtSP_From;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtSP_To;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtFactory;
        private Win.UI.TextBox txtRefnoFrom;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.RadioButton radioSummary;
        private Win.UI.CheckBox checkQty;
        private Win.UI.TextBox txtRefnoTo;
        private Win.UI.Label label6;
    }

}