namespace Sci.Production.Thread
{
    partial class R21
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboMDivision = new Sci.Production.Class.comboMDivision(this.components);
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.textLOC2 = new Sci.Win.UI.TextBox();
            this.textLOC1 = new Sci.Win.UI.TextBox();
            this.textITEM = new Sci.Win.UI.TextBox();
            this.textTYPE = new Sci.Win.UI.TextBox();
            this.textSHA = new Sci.Win.UI.TextBox();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(463, 12);
            this.print.TabIndex = 1;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(463, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(463, 84);
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboMDivision);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textLOC2);
            this.panel1.Controls.Add(this.textLOC1);
            this.panel1.Controls.Add(this.textITEM);
            this.panel1.Controls.Add(this.textTYPE);
            this.panel1.Controls.Add(this.textSHA);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 190);
            this.panel1.TabIndex = 0;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(99, 153);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(16, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 23);
            this.label8.TabIndex = 14;
            this.label8.Text = "M";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(220, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 23);
            this.label7.TabIndex = 13;
            this.label7.Text = "~";
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(251, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 23);
            this.label6.TabIndex = 12;
            this.label6.Text = "~";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // textLOC2
            // 
            this.textLOC2.BackColor = System.Drawing.Color.White;
            this.textLOC2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textLOC2.Location = new System.Drawing.Point(243, 125);
            this.textLOC2.Name = "textLOC2";
            this.textLOC2.Size = new System.Drawing.Size(113, 23);
            this.textLOC2.TabIndex = 6;
            this.textLOC2.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textLOC2_PopUp);
            this.textLOC2.Validating += new System.ComponentModel.CancelEventHandler(this.textLOC2_Validating);
            // 
            // textLOC1
            // 
            this.textLOC1.BackColor = System.Drawing.Color.White;
            this.textLOC1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textLOC1.Location = new System.Drawing.Point(99, 125);
            this.textLOC1.Name = "textLOC1";
            this.textLOC1.Size = new System.Drawing.Size(113, 23);
            this.textLOC1.TabIndex = 5;
            this.textLOC1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textLOC1_PopUp);
            this.textLOC1.Validating += new System.ComponentModel.CancelEventHandler(this.textLOC1_Validating);
            // 
            // textITEM
            // 
            this.textITEM.BackColor = System.Drawing.Color.White;
            this.textITEM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textITEM.Location = new System.Drawing.Point(99, 96);
            this.textITEM.Name = "textITEM";
            this.textITEM.Size = new System.Drawing.Size(127, 23);
            this.textITEM.TabIndex = 4;
            this.textITEM.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textITEM_PopUp);
            this.textITEM.Validating += new System.ComponentModel.CancelEventHandler(this.textITEM_Validating);
            // 
            // textTYPE
            // 
            this.textTYPE.BackColor = System.Drawing.Color.White;
            this.textTYPE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textTYPE.Location = new System.Drawing.Point(99, 65);
            this.textTYPE.Name = "textTYPE";
            this.textTYPE.Size = new System.Drawing.Size(135, 23);
            this.textTYPE.TabIndex = 3;
            this.textTYPE.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textTYPE_PopUp);
            this.textTYPE.Validating += new System.ComponentModel.CancelEventHandler(this.textTYPE_Validating);
            // 
            // textSHA
            // 
            this.textSHA.BackColor = System.Drawing.Color.White;
            this.textSHA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSHA.Location = new System.Drawing.Point(99, 36);
            this.textSHA.Name = "textSHA";
            this.textSHA.Size = new System.Drawing.Size(122, 23);
            this.textSHA.TabIndex = 2;
            this.textSHA.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textSHA_PopUp);
            this.textSHA.Validating += new System.ComponentModel.CancelEventHandler(this.textSHA_Validating);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.Location = new System.Drawing.Point(267, 8);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(148, 23);
            this.textBox2.TabIndex = 1;
            this.textBox2.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox2_PopUp);
            this.textBox2.Validating += new System.ComponentModel.CancelEventHandler(this.textBox2_Validating);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(99, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(16, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Location";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(16, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Thread Item";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(16, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Type";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(16, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Shade";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(16, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Refno";
            // 
            // R21
            // 
            this.ClientSize = new System.Drawing.Size(555, 240);
            this.Controls.Add(this.panel1);
            this.Name = "R21";
            this.Text = "R21.Thread Stock List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.TextBox textLOC2;
        private Win.UI.TextBox textLOC1;
        private Win.UI.TextBox textITEM;
        private Win.UI.TextBox textTYPE;
        private Win.UI.TextBox textSHA;
        private Win.UI.TextBox textBox2;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label label8;
        private Class.comboMDivision comboMDivision;
    }
}
