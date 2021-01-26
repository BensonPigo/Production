
namespace Sci.Production.PPIC
{
    partial class P22_Print
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
            this.components = new System.ComponentModel.Container();
            this.labelDelivery = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.dropDownListTableAdapter1 = new Sci.Production.Planning.GSchemas.GLOTableAdapters.DropDownListTableAdapter();
            this.dateCreate = new Sci.Win.UI.DateRange();
            this.dateApv = new Sci.Win.UI.DateRange();
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.txtDepartment = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(395, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(395, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(395, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(354, 136);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(320, 142);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(287, 143);
            // 
            // labelDelivery
            // 
            this.labelDelivery.Location = new System.Drawing.Point(19, 12);
            this.labelDelivery.Name = "labelDelivery";
            this.labelDelivery.Size = new System.Drawing.Size(80, 23);
            this.labelDelivery.TabIndex = 97;
            this.labelDelivery.Text = "Create Date";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 98;
            this.label1.Text = "M";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 99;
            this.label2.Text = "Apv. Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 100;
            this.label3.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 101;
            this.label4.Text = "Department";
            // 
            // dropDownListTableAdapter1
            // 
            this.dropDownListTableAdapter1.ClearBeforeFill = true;
            // 
            // dateCreate
            // 
            // 
            // 
            // 
            this.dateCreate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCreate.DateBox1.Name = "";
            this.dateCreate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCreate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCreate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCreate.DateBox2.Name = "";
            this.dateCreate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCreate.DateBox2.TabIndex = 1;
            this.dateCreate.Location = new System.Drawing.Point(102, 12);
            this.dateCreate.Name = "dateCreate";
            this.dateCreate.Size = new System.Drawing.Size(280, 23);
            this.dateCreate.TabIndex = 102;
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
            this.dateApv.Location = new System.Drawing.Point(102, 48);
            this.dateApv.Name = "dateApv";
            this.dateApv.Size = new System.Drawing.Size(280, 23);
            this.dateApv.TabIndex = 103;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(102, 84);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.OldText = "";
            this.comboMDivision.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision.TabIndex = 104;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(102, 118);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 105;
            // 
            // txtDepartment
            // 
            this.txtDepartment.BackColor = System.Drawing.Color.White;
            this.txtDepartment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDepartment.Location = new System.Drawing.Point(102, 157);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(141, 23);
            this.txtDepartment.TabIndex = 106;
            // 
            // P22_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 221);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboMDivision);
            this.Controls.Add(this.dateApv);
            this.Controls.Add(this.dateCreate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelDelivery);
            this.IsSupportPrint = false;
            this.Name = "P22_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P22. Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelDelivery, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateCreate, 0);
            this.Controls.SetChildIndex(this.dateApv, 0);
            this.Controls.SetChildIndex(this.comboMDivision, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtDepartment, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDelivery;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Planning.GSchemas.GLOTableAdapters.DropDownListTableAdapter dropDownListTableAdapter1;
        private Win.UI.DateRange dateCreate;
        private Win.UI.DateRange dateApv;
        private Class.ComboMDivision comboMDivision;
        private Class.ComboFactory comboFactory;
        private Win.UI.TextBox txtDepartment;
    }
}