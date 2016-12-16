namespace Sci.Production.Subcon
{
    partial class R41
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.textSP = new Sci.Win.UI.TextBox();
            this.dateBundle = new Sci.Win.UI.DateRange();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textCutRef_Start = new System.Windows.Forms.TextBox();
            this.textCutRef_End = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(425, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(425, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(425, 84);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(27, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Cut Ref#";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(27, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "SP#";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(27, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "Bundle CDate";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(27, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Sub Process";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(27, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "M";
            // 
            // textSP
            // 
            this.textSP.BackColor = System.Drawing.Color.White;
            this.textSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSP.Location = new System.Drawing.Point(125, 48);
            this.textSP.Name = "textSP";
            this.textSP.Size = new System.Drawing.Size(143, 23);
            this.textSP.TabIndex = 100;
            // 
            // dateBundle
            // 
            this.dateBundle.Location = new System.Drawing.Point(125, 77);
            this.dateBundle.Name = "dateBundle";
            this.dateBundle.Size = new System.Drawing.Size(280, 23);
            this.dateBundle.TabIndex = 101;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(125, 106);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(121, 24);
            this.comboSubProcess.TabIndex = 102;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(125, 136);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 103;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(252, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 106;
            this.label6.Text = "~";
            // 
            // textCutRef_Start
            // 
            this.textCutRef_Start.BackColor = System.Drawing.Color.White;
            this.textCutRef_Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textCutRef_Start.Location = new System.Drawing.Point(125, 19);
            this.textCutRef_Start.Name = "textCutRef_Start";
            this.textCutRef_Start.Size = new System.Drawing.Size(121, 23);
            this.textCutRef_Start.TabIndex = 107;
            // 
            // textCutRef_End
            // 
            this.textCutRef_End.BackColor = System.Drawing.Color.White;
            this.textCutRef_End.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textCutRef_End.Location = new System.Drawing.Point(274, 19);
            this.textCutRef_End.Name = "textCutRef_End";
            this.textCutRef_End.Size = new System.Drawing.Size(131, 23);
            this.textCutRef_End.TabIndex = 108;
            // 
            // R41
            // 
            this.ClientSize = new System.Drawing.Size(517, 205);
            this.Controls.Add(this.textCutRef_End);
            this.Controls.Add(this.textCutRef_Start);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.dateBundle);
            this.Controls.Add(this.textSP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R41";
            this.Text = "Bundle tracking list (RFID)";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.textSP, 0);
            this.Controls.SetChildIndex(this.dateBundle, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.textCutRef_Start, 0);
            this.Controls.SetChildIndex(this.textCutRef_End, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.TextBox textSP;
        private Win.UI.DateRange dateBundle;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.ComboBox comboM;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textCutRef_Start;
        private System.Windows.Forms.TextBox textCutRef_End;

    }
}
