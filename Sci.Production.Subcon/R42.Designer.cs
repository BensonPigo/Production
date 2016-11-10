namespace Sci.Production.Subcon
{
    partial class R42
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.dateBundle = new Sci.Win.UI.DateRange();
            this.textSP = new Sci.Win.UI.TextBox();
            this.dateCutRef = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.dateBundleTransDate = new Sci.Win.UI.DateRange();
            this.label6 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(507, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(507, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(507, 84);
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(120, 158);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 113;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(120, 128);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(121, 24);
            this.comboSubProcess.TabIndex = 112;
            // 
            // dateBundle
            // 
            this.dateBundle.Location = new System.Drawing.Point(120, 70);
            this.dateBundle.Name = "dateBundle";
            this.dateBundle.Size = new System.Drawing.Size(280, 23);
            this.dateBundle.TabIndex = 111;
            // 
            // textSP
            // 
            this.textSP.BackColor = System.Drawing.Color.White;
            this.textSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSP.Location = new System.Drawing.Point(120, 41);
            this.textSP.Name = "textSP";
            this.textSP.Size = new System.Drawing.Size(143, 23);
            this.textSP.TabIndex = 110;
            // 
            // dateCutRef
            // 
            this.dateCutRef.Location = new System.Drawing.Point(120, 12);
            this.dateCutRef.Name = "dateCutRef";
            this.dateCutRef.Size = new System.Drawing.Size(280, 23);
            this.dateCutRef.TabIndex = 109;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(22, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 23);
            this.label5.TabIndex = 108;
            this.label5.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(22, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 23);
            this.label4.TabIndex = 107;
            this.label4.Text = "Sub Process";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(22, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 106;
            this.label3.Text = "Bundle CDate";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(22, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 105;
            this.label2.Text = "SP#";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(22, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 104;
            this.label1.Text = "Cut Ref#";
            // 
            // dateBundleTransDate
            // 
            this.dateBundleTransDate.Location = new System.Drawing.Point(120, 99);
            this.dateBundleTransDate.Name = "dateBundleTransDate";
            this.dateBundleTransDate.Size = new System.Drawing.Size(280, 23);
            this.dateBundleTransDate.TabIndex = 115;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(22, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 23);
            this.label6.TabIndex = 114;
            this.label6.Text = "Bundle Trans Date";
            // 
            // R42
            // 
            this.ClientSize = new System.Drawing.Size(599, 240);
            this.Controls.Add(this.dateBundleTransDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.dateBundle);
            this.Controls.Add(this.textSP);
            this.Controls.Add(this.dateCutRef);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R42";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateCutRef, 0);
            this.Controls.SetChildIndex(this.textSP, 0);
            this.Controls.SetChildIndex(this.dateBundle, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.dateBundleTransDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.ComboBox comboFactory;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.DateRange dateBundle;
        private Win.UI.TextBox textSP;
        private Win.UI.DateRange dateCutRef;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateBundleTransDate;
        private Win.UI.Label label6;
    }
}
