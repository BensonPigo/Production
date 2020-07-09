namespace Sci.Production.Centralized
{
    partial class R20
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
            this.radioFOB = new System.Windows.Forms.RadioButton();
            this.label7 = new Sci.Win.UI.Label();
            this.radioCPU = new System.Windows.Forms.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.checkExportbySP = new Sci.Win.UI.CheckBox();
            this.dateQueryDateStart = new Sci.Win.UI.DateBox();
            this.dateQueryDateEnd = new Sci.Win.UI.DateBox();
            this.label5 = new Sci.Win.UI.Label();
            this.comboDropdownlist = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtCentralizedFactory = new Sci.Production.Class.TxtCentralizedFactory();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(446, 12);
            this.print.TabIndex = 9;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(446, 48);
            this.toexcel.TabIndex = 10;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(446, 84);
            this.close.TabIndex = 11;
            // 
            // radioFOB
            // 
            this.radioFOB.AutoSize = true;
            this.radioFOB.Location = new System.Drawing.Point(199, 12);
            this.radioFOB.Name = "radioFOB";
            this.radioFOB.Size = new System.Drawing.Size(54, 21);
            this.radioFOB.TabIndex = 1;
            this.radioFOB.TabStop = true;
            this.radioFOB.Text = "FOB";
            this.radioFOB.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 23);
            this.label7.TabIndex = 175;
            this.label7.Text = "Create by";
            // 
            // radioCPU
            // 
            this.radioCPU.AutoSize = true;
            this.radioCPU.Location = new System.Drawing.Point(125, 12);
            this.radioCPU.Name = "radioCPU";
            this.radioCPU.Size = new System.Drawing.Size(51, 21);
            this.radioCPU.TabIndex = 0;
            this.radioCPU.TabStop = true;
            this.radioCPU.Text = "Cpu";
            this.radioCPU.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 23);
            this.label1.TabIndex = 177;
            this.label1.Text = "Query Date";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 23);
            this.label8.TabIndex = 185;
            this.label8.Text = "Category";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 23);
            this.label2.TabIndex = 188;
            this.label2.Text = "Factory";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 23);
            this.label3.TabIndex = 189;
            this.label3.Text = "Brand";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 192);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 23);
            this.label4.TabIndex = 191;
            this.label4.Text = "Region";
            // 
            // checkExportbySP
            // 
            this.checkExportbySP.AutoSize = true;
            this.checkExportbySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExportbySP.Location = new System.Drawing.Point(9, 229);
            this.checkExportbySP.Name = "checkExportbySP";
            this.checkExportbySP.Size = new System.Drawing.Size(116, 21);
            this.checkExportbySP.TabIndex = 8;
            this.checkExportbySP.Text = "Export by SP#";
            this.checkExportbySP.UseVisualStyleBackColor = true;
            // 
            // dateQueryDateStart
            // 
            this.dateQueryDateStart.Location = new System.Drawing.Point(125, 48);
            this.dateQueryDateStart.Name = "dateQueryDateStart";
            this.dateQueryDateStart.Size = new System.Drawing.Size(98, 23);
            this.dateQueryDateStart.TabIndex = 2;
            // 
            // dateQueryDateEnd
            // 
            this.dateQueryDateEnd.Location = new System.Drawing.Point(243, 48);
            this.dateQueryDateEnd.Name = "dateQueryDateEnd";
            this.dateQueryDateEnd.Size = new System.Drawing.Size(98, 23);
            this.dateQueryDateEnd.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(226, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 23);
            this.label5.TabIndex = 199;
            this.label5.Text = "~";
            // 
            // comboDropdownlist
            // 
            this.comboDropdownlist.BackColor = System.Drawing.Color.White;
            this.comboDropdownlist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropdownlist.FormattingEnabled = true;
            this.comboDropdownlist.IsSupportUnselect = true;
            this.comboDropdownlist.Location = new System.Drawing.Point(125, 84);
            this.comboDropdownlist.Name = "comboDropdownlist";
            this.comboDropdownlist.Size = new System.Drawing.Size(183, 24);
            this.comboDropdownlist.TabIndex = 200;
            this.comboDropdownlist.Type = "Pms_ReportCategory";
            // 
            // txtCentralizedFactory
            // 
            this.txtCentralizedFactory.BackColor = System.Drawing.Color.White;
            this.txtCentralizedFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCentralizedFactory.Location = new System.Drawing.Point(125, 120);
            this.txtCentralizedFactory.Name = "txtCentralizedFactory";
            this.txtCentralizedFactory.Size = new System.Drawing.Size(137, 23);
            this.txtCentralizedFactory.TabIndex = 201;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(125, 156);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(137, 23);
            this.txtbrand.TabIndex = 202;
            // 
            // txtcountry
            // 
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(125, 192);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 22);
            this.txtcountry.TabIndex = 203;
            this.txtcountry.TextBox1Binding = "";
            // 
            // R20
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 288);
            this.Controls.Add(this.txtcountry);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtCentralizedFactory);
            this.Controls.Add(this.comboDropdownlist);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateQueryDateEnd);
            this.Controls.Add(this.dateQueryDateStart);
            this.Controls.Add(this.checkExportbySP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioFOB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.radioCPU);
            this.Name = "R20";
            this.Text = "R20.製成品進銷存明細表";
            this.Controls.SetChildIndex(this.radioCPU, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.radioFOB, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.checkExportbySP, 0);
            this.Controls.SetChildIndex(this.dateQueryDateStart, 0);
            this.Controls.SetChildIndex(this.dateQueryDateEnd, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.comboDropdownlist, 0);
            this.Controls.SetChildIndex(this.txtCentralizedFactory, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtcountry, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioFOB;
        private Win.UI.Label label7;
        private System.Windows.Forms.RadioButton radioCPU;
        private Win.UI.Label label1;
        private Win.UI.Label label8;        
        private Win.UI.Label label2;
        private Win.UI.Label label3;        
        private Win.UI.Label label4;
        private Win.UI.CheckBox checkExportbySP;
        private Win.UI.DateBox dateQueryDateStart;
        private Win.UI.DateBox dateQueryDateEnd;
        private Win.UI.Label label5;
        private Class.ComboDropDownList comboDropdownlist;
        private Class.TxtCentralizedFactory txtCentralizedFactory;
        private Class.Txtbrand txtbrand;
        private Class.Txtcountry txtcountry;
    }
}