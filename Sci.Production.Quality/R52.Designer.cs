namespace Sci.Production.Quality
{
    partial class R52
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.labelM = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.dateBundleInspectDate = new Sci.Win.UI.DateRange();
            this.labelBundleInspectDate = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.comboLastResult = new Sci.Win.UI.ComboBox();
            this.dateBundleCDate = new Sci.Win.UI.DateRange();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.label9 = new Sci.Win.UI.Label();
            this.txtCutRefEnd = new Sci.Win.UI.TextBox();
            this.txtCutRefStart = new Sci.Win.UI.TextBox();
            this.labelLastResult = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelBundleCDate = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.labelCutRef = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.txtsubprocess = new Sci.Production.Class.Txtsubprocess();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(469, 87);
            this.print.TabIndex = 0;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(469, 15);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(469, 51);
            this.close.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.txtsubprocess);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.labelSubProcess);
            this.panel1.Controls.Add(this.dateBundleInspectDate);
            this.panel1.Controls.Add(this.labelBundleInspectDate);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Controls.Add(this.comboLastResult);
            this.panel1.Controls.Add(this.dateBundleCDate);
            this.panel1.Controls.Add(this.dateEstCutDate);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtCutRefEnd);
            this.panel1.Controls.Add(this.txtCutRefStart);
            this.panel1.Controls.Add(this.labelLastResult);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelBundleCDate);
            this.panel1.Controls.Add(this.labelEstCutDate);
            this.panel1.Controls.Add(this.labelCutRef);
            this.panel1.Location = new System.Drawing.Point(15, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(439, 289);
            this.panel1.TabIndex = 97;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(144, 184);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 7;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(12, 185);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(129, 23);
            this.labelM.TabIndex = 16;
            this.labelM.Text = "M";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(12, 157);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(129, 23);
            this.labelSubProcess.TabIndex = 15;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // dateBundleInspectDate
            // 
            // 
            // 
            // 
            this.dateBundleInspectDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBundleInspectDate.DateBox1.Name = "";
            this.dateBundleInspectDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBundleInspectDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBundleInspectDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBundleInspectDate.DateBox2.Name = "";
            this.dateBundleInspectDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBundleInspectDate.DateBox2.TabIndex = 1;
            this.dateBundleInspectDate.IsRequired = false;
            this.dateBundleInspectDate.Location = new System.Drawing.Point(144, 128);
            this.dateBundleInspectDate.Name = "dateBundleInspectDate";
            this.dateBundleInspectDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleInspectDate.TabIndex = 5;
            // 
            // labelBundleInspectDate
            // 
            this.labelBundleInspectDate.Location = new System.Drawing.Point(12, 129);
            this.labelBundleInspectDate.Name = "labelBundleInspectDate";
            this.labelBundleInspectDate.Size = new System.Drawing.Size(129, 23);
            this.labelBundleInspectDate.TabIndex = 14;
            this.labelBundleInspectDate.Text = "Bundle Inspect Date";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(144, 45);
            this.txtSP.MaxLength = 13;
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(139, 23);
            this.txtSP.TabIndex = 2;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(12, 45);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(129, 23);
            this.labelSP.TabIndex = 11;
            this.labelSP.Text = "SP#";
            // 
            // comboLastResult
            // 
            this.comboLastResult.BackColor = System.Drawing.Color.White;
            this.comboLastResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLastResult.FormattingEnabled = true;
            this.comboLastResult.IsSupportUnselect = true;
            this.comboLastResult.Location = new System.Drawing.Point(144, 241);
            this.comboLastResult.Name = "comboLastResult";
            this.comboLastResult.OldText = "";
            this.comboLastResult.Size = new System.Drawing.Size(121, 24);
            this.comboLastResult.TabIndex = 9;
            // 
            // dateBundleCDate
            // 
            // 
            // 
            // 
            this.dateBundleCDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBundleCDate.DateBox1.Name = "";
            this.dateBundleCDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBundleCDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBundleCDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBundleCDate.DateBox2.Name = "";
            this.dateBundleCDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBundleCDate.DateBox2.TabIndex = 1;
            this.dateBundleCDate.IsRequired = false;
            this.dateBundleCDate.Location = new System.Drawing.Point(144, 101);
            this.dateBundleCDate.Name = "dateBundleCDate";
            this.dateBundleCDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleCDate.TabIndex = 4;
            // 
            // dateEstCutDate
            // 
            // 
            // 
            // 
            this.dateEstCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCutDate.DateBox1.Name = "";
            this.dateEstCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCutDate.DateBox2.Name = "";
            this.dateEstCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox2.TabIndex = 1;
            this.dateEstCutDate.IsRequired = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(144, 73);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(263, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 110;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtCutRefEnd
            // 
            this.txtCutRefEnd.BackColor = System.Drawing.Color.White;
            this.txtCutRefEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefEnd.Location = new System.Drawing.Point(286, 17);
            this.txtCutRefEnd.MaxLength = 13;
            this.txtCutRefEnd.Name = "txtCutRefEnd";
            this.txtCutRefEnd.Size = new System.Drawing.Size(116, 23);
            this.txtCutRefEnd.TabIndex = 1;
            // 
            // txtCutRefStart
            // 
            this.txtCutRefStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefStart.Location = new System.Drawing.Point(144, 17);
            this.txtCutRefStart.MaxLength = 13;
            this.txtCutRefStart.Name = "txtCutRefStart";
            this.txtCutRefStart.Size = new System.Drawing.Size(116, 23);
            this.txtCutRefStart.TabIndex = 0;
            // 
            // labelLastResult
            // 
            this.labelLastResult.Location = new System.Drawing.Point(12, 242);
            this.labelLastResult.Name = "labelLastResult";
            this.labelLastResult.Size = new System.Drawing.Size(129, 23);
            this.labelLastResult.TabIndex = 18;
            this.labelLastResult.Text = "Last Result";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 213);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(129, 23);
            this.labelFactory.TabIndex = 17;
            this.labelFactory.Text = "Factory";
            // 
            // labelBundleCDate
            // 
            this.labelBundleCDate.Location = new System.Drawing.Point(12, 101);
            this.labelBundleCDate.Name = "labelBundleCDate";
            this.labelBundleCDate.Size = new System.Drawing.Size(129, 23);
            this.labelBundleCDate.TabIndex = 13;
            this.labelBundleCDate.Text = "Bundle CDate";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(12, 73);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(129, 23);
            this.labelEstCutDate.TabIndex = 12;
            this.labelEstCutDate.Text = "Est Cut Date";
            // 
            // labelCutRef
            // 
            this.labelCutRef.Location = new System.Drawing.Point(12, 17);
            this.labelCutRef.Name = "labelCutRef";
            this.labelCutRef.Size = new System.Drawing.Size(129, 23);
            this.labelCutRef.TabIndex = 10;
            this.labelCutRef.Text = "Cut Ref#";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = false;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(144, 213);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 8;
            // 
            // txtsubprocess
            // 
            this.txtsubprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtsubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtsubprocess.IsRFIDProcess = true;
            this.txtsubprocess.IsSupportEditMode = false;
            this.txtsubprocess.Location = new System.Drawing.Point(144, 157);
            this.txtsubprocess.MultiSelect = true;
            this.txtsubprocess.Name = "txtsubprocess";
            this.txtsubprocess.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtsubprocess.ReadOnly = true;
            this.txtsubprocess.Size = new System.Drawing.Size(280, 23);
            this.txtsubprocess.TabIndex = 6;
            // 
            // R52
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 326);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtCutRefStart";
            this.DefaultControlForEdit = "txtCutRefStart";
            this.IsSupportToPrint = false;
            this.Name = "R52";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R52. Subprocess MD Inspection Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.ComboBox comboLastResult;
        private Win.UI.DateRange dateBundleCDate;
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtCutRefEnd;
        private Win.UI.TextBox txtCutRefStart;
        private Win.UI.Label labelLastResult;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelBundleCDate;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.Label labelCutRef;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSP;
        private Win.UI.DateRange dateBundleInspectDate;
        private Win.UI.Label labelBundleInspectDate;
        private Class.Txtsubprocess txtsubprocess;
        private Win.UI.Label labelSubProcess;
        private Win.UI.ComboBox comboM;
        private Win.UI.Label labelM;
        private Class.ComboFactory comboFactory;
    }
}