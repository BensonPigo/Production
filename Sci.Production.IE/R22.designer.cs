namespace Sci.Production.IE
{
    partial class R22
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
            this.txtRD = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dtInline = new Sci.Win.UI.DateRange();
            this.txtCell = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCategory = new Sci.Win.UI.TextBox();
            this.txtProductType = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSPNO = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.chkOutstanding = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.lbl1 = new Sci.Win.UI.Label();
            this.lblAddEditDate = new Sci.Win.UI.Label();
            this.dtAddEdit = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(434, 77);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(434, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(434, 45);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(386, 164);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(392, 164);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(412, 197);
            // 
            // txtRD
            // 
            this.txtRD.BackColor = System.Drawing.Color.White;
            this.txtRD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRD.Location = new System.Drawing.Point(116, 226);
            this.txtRD.Name = "txtRD";
            this.txtRD.Size = new System.Drawing.Size(166, 26);
            this.txtRD.TabIndex = 203;
            this.txtRD.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtRD_PopUp);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 228);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 23);
            this.label4.TabIndex = 202;
            this.label4.Text = "Response Dep.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 44);
            this.label3.Name = "label3";
            this.label3.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.BorderWidth = 1F;
            this.label3.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label3.RectStyle.ExtBorderWidth = 1F;
            this.label3.Size = new System.Drawing.Size(104, 23);
            this.label3.TabIndex = 201;
            this.label3.Text = "Inline Date";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dtInline
            // 
            // 
            // 
            // 
            this.dtInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dtInline.DateBox1.Name = "";
            this.dtInline.DateBox1.Size = new System.Drawing.Size(127, 26);
            this.dtInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dtInline.DateBox2.Location = new System.Drawing.Point(153, 0);
            this.dtInline.DateBox2.Name = "";
            this.dtInline.DateBox2.Size = new System.Drawing.Size(127, 26);
            this.dtInline.DateBox2.TabIndex = 1;
            this.dtInline.IsRequired = false;
            this.dtInline.Location = new System.Drawing.Point(116, 42);
            this.dtInline.Name = "dtInline";
            this.dtInline.Size = new System.Drawing.Size(280, 26);
            this.dtInline.TabIndex = 200;
            // 
            // txtCell
            // 
            this.txtCell.BackColor = System.Drawing.Color.White;
            this.txtCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell.Location = new System.Drawing.Point(116, 195);
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(166, 26);
            this.txtCell.TabIndex = 199;
            this.txtCell.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtCell_PopUp);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 23);
            this.label2.TabIndex = 198;
            this.label2.Text = "Cell";
            // 
            // txtCategory
            // 
            this.txtCategory.BackColor = System.Drawing.Color.White;
            this.txtCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCategory.Location = new System.Drawing.Point(116, 164);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(166, 26);
            this.txtCategory.TabIndex = 197;
            this.txtCategory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtCategory_PopUp);
            // 
            // txtProductType
            // 
            this.txtProductType.BackColor = System.Drawing.Color.White;
            this.txtProductType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtProductType.Location = new System.Drawing.Point(116, 133);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(166, 26);
            this.txtProductType.TabIndex = 196;
            this.txtProductType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtProductType_PopUp);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 23);
            this.label1.TabIndex = 195;
            this.label1.Text = "Product Type";
            // 
            // txtSPNO
            // 
            this.txtSPNO.BackColor = System.Drawing.Color.White;
            this.txtSPNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNO.Location = new System.Drawing.Point(116, 73);
            this.txtSPNO.Name = "txtSPNO";
            this.txtSPNO.Size = new System.Drawing.Size(166, 26);
            this.txtSPNO.TabIndex = 194;
            this.txtSPNO.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSPNO_PopUp);
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(116, 103);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(166, 26);
            this.txtStyle.TabIndex = 193;
            this.txtStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtStyle_PopUp);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 192;
            this.label5.Text = "Category";
            // 
            // chkOutstanding
            // 
            this.chkOutstanding.AutoSize = true;
            this.chkOutstanding.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOutstanding.IsSupportEditMode = false;
            this.chkOutstanding.Location = new System.Drawing.Point(10, 260);
            this.chkOutstanding.Name = "chkOutstanding";
            this.chkOutstanding.Size = new System.Drawing.Size(366, 24);
            this.chkOutstanding.TabIndex = 191;
            this.chkOutstanding.Text = "Outstanding (Only Show Overdue CheckList)";
            this.chkOutstanding.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 106);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 23);
            this.label6.TabIndex = 190;
            this.label6.Text = "Style";
            // 
            // lbl1
            // 
            this.lbl1.Location = new System.Drawing.Point(9, 75);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(104, 23);
            this.lbl1.TabIndex = 189;
            this.lbl1.Text = "SP";
            // 
            // lblAddEditDate
            // 
            this.lblAddEditDate.Location = new System.Drawing.Point(9, 9);
            this.lblAddEditDate.Name = "lblAddEditDate";
            this.lblAddEditDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lblAddEditDate.RectStyle.BorderWidth = 1F;
            this.lblAddEditDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lblAddEditDate.RectStyle.ExtBorderWidth = 1F;
            this.lblAddEditDate.Size = new System.Drawing.Size(104, 23);
            this.lblAddEditDate.TabIndex = 188;
            this.lblAddEditDate.Text = "Deadline";
            this.lblAddEditDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lblAddEditDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dtAddEdit
            // 
            // 
            // 
            // 
            this.dtAddEdit.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dtAddEdit.DateBox1.Name = "";
            this.dtAddEdit.DateBox1.Size = new System.Drawing.Size(127, 26);
            this.dtAddEdit.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dtAddEdit.DateBox2.Location = new System.Drawing.Point(153, 0);
            this.dtAddEdit.DateBox2.Name = "";
            this.dtAddEdit.DateBox2.Size = new System.Drawing.Size(127, 26);
            this.dtAddEdit.DateBox2.TabIndex = 1;
            this.dtAddEdit.IsRequired = false;
            this.dtAddEdit.Location = new System.Drawing.Point(116, 9);
            this.dtAddEdit.Name = "dtAddEdit";
            this.dtAddEdit.Size = new System.Drawing.Size(280, 26);
            this.dtAddEdit.TabIndex = 187;
            // 
            // R22
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 383);
            this.Controls.Add(this.txtRD);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtInline);
            this.Controls.Add(this.txtCell);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.txtProductType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSPNO);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkOutstanding);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.lblAddEditDate);
            this.Controls.Add(this.dtAddEdit);
            this.Name = "R22";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R22. Changeover CheckList Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.dtAddEdit, 0);
            this.Controls.SetChildIndex(this.lblAddEditDate, 0);
            this.Controls.SetChildIndex(this.lbl1, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.chkOutstanding, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.txtSPNO, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtProductType, 0);
            this.Controls.SetChildIndex(this.txtCategory, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtCell, 0);
            this.Controls.SetChildIndex(this.dtInline, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtRD, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtRD;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.DateRange dtInline;
        private Win.UI.TextBox txtCell;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtCategory;
        private Win.UI.TextBox txtProductType;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSPNO;
        private Win.UI.TextBox txtStyle;
        private Win.UI.Label label5;
        private Win.UI.CheckBox chkOutstanding;
        private Win.UI.Label label6;
        private Win.UI.Label lbl1;
        private Win.UI.Label lblAddEditDate;
        private Win.UI.DateRange dtAddEdit;
    }
}