namespace Sci.Production.Subcon
{
    partial class R32
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
            this.dateFarmOutDate = new Sci.Win.UI.DateRange();
            this.labelFarmOutDate = new Sci.Win.UI.Label();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.dateBundleCdate = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.dateBundleScan = new Sci.Win.UI.DateRange();
            this.txtsubprocess = new Sci.Production.Class.Txtsubprocess();
            this.labspno = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(495, 12);
            this.print.TabIndex = 6;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(495, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(495, 84);
            this.close.TabIndex = 8;
            // 
            // dateFarmOutDate
            // 
            // 
            // 
            // 
            this.dateFarmOutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFarmOutDate.DateBox1.Name = "";
            this.dateFarmOutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFarmOutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFarmOutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFarmOutDate.DateBox2.Name = "";
            this.dateFarmOutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFarmOutDate.DateBox2.TabIndex = 1;
            this.dateFarmOutDate.IsRequired = false;
            this.dateFarmOutDate.Location = new System.Drawing.Point(133, 9);
            this.dateFarmOutDate.Name = "dateFarmOutDate";
            this.dateFarmOutDate.Size = new System.Drawing.Size(280, 23);
            this.dateFarmOutDate.TabIndex = 0;
            // 
            // labelFarmOutDate
            // 
            this.labelFarmOutDate.Location = new System.Drawing.Point(9, 9);
            this.labelFarmOutDate.Name = "labelFarmOutDate";
            this.labelFarmOutDate.Size = new System.Drawing.Size(121, 23);
            this.labelFarmOutDate.TabIndex = 95;
            this.labelFarmOutDate.Text = "Farm Out Date";
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(9, 126);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(121, 23);
            this.labelSubProcess.TabIndex = 97;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(133, 156);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 5;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 156);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(121, 23);
            this.labelFactory.TabIndex = 99;
            this.labelFactory.Text = "Factory";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 23);
            this.label1.TabIndex = 100;
            this.label1.Text = "Bundle CDate";
            // 
            // dateBundleCdate
            // 
            // 
            // 
            // 
            this.dateBundleCdate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBundleCdate.DateBox1.Name = "";
            this.dateBundleCdate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBundleCdate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBundleCdate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBundleCdate.DateBox2.Name = "";
            this.dateBundleCdate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBundleCdate.DateBox2.TabIndex = 1;
            this.dateBundleCdate.IsRequired = false;
            this.dateBundleCdate.Location = new System.Drawing.Point(133, 38);
            this.dateBundleCdate.Name = "dateBundleCdate";
            this.dateBundleCdate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleCdate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 23);
            this.label2.TabIndex = 102;
            this.label2.Text = "Bundle Scan Date";
            // 
            // dateBundleScan
            // 
            // 
            // 
            // 
            this.dateBundleScan.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBundleScan.DateBox1.Name = "";
            this.dateBundleScan.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBundleScan.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBundleScan.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBundleScan.DateBox2.Name = "";
            this.dateBundleScan.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBundleScan.DateBox2.TabIndex = 1;
            this.dateBundleScan.IsRequired = false;
            this.dateBundleScan.Location = new System.Drawing.Point(133, 67);
            this.dateBundleScan.Name = "dateBundleScan";
            this.dateBundleScan.Size = new System.Drawing.Size(280, 23);
            this.dateBundleScan.TabIndex = 2;
            // 
            // txtsubprocess
            // 
            this.txtsubprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtsubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtsubprocess.IsSupportEditMode = false;
            this.txtsubprocess.Location = new System.Drawing.Point(133, 126);
            this.txtsubprocess.Name = "txtsubprocess";
            this.txtsubprocess.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtsubprocess.ReadOnly = true;
            this.txtsubprocess.Size = new System.Drawing.Size(280, 23);
            this.txtsubprocess.TabIndex = 4;
            // 
            // labspno
            // 
            this.labspno.Location = new System.Drawing.Point(9, 96);
            this.labspno.Name = "labspno";
            this.labspno.Size = new System.Drawing.Size(121, 23);
            this.labspno.TabIndex = 110;
            this.labspno.Text = "SP#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(133, 96);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(127, 23);
            this.txtSPNo.TabIndex = 3;
            // 
            // R32
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 217);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labspno);
            this.Controls.Add(this.txtsubprocess);
            this.Controls.Add(this.dateBundleScan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateBundleCdate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.dateFarmOutDate);
            this.Controls.Add(this.labelFarmOutDate);
            this.Name = "R32";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R32. Farm out Bundle Tracking List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFarmOutDate, 0);
            this.Controls.SetChildIndex(this.dateFarmOutDate, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateBundleCdate, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateBundleScan, 0);
            this.Controls.SetChildIndex(this.txtsubprocess, 0);
            this.Controls.SetChildIndex(this.labspno, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateFarmOutDate;
        private Win.UI.Label labelFarmOutDate;
        private Win.UI.Label labelSubProcess;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateBundleCdate;
        private Win.UI.Label label2;
        private Win.UI.DateRange dateBundleScan;
        private Class.Txtsubprocess txtsubprocess;
        private Win.UI.Label labspno;
        private Win.UI.TextBox txtSPNo;
    }
}