namespace Sci.Production.Shipping
{
    partial class P02_BatchPrint
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
            this.labelReceiver = new Sci.Win.UI.Label();
            this.labelInCharge = new Sci.Win.UI.Label();
            this.labelCNo = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelSeq = new Sci.Win.UI.Label();
            this.txtReceiver = new Sci.Win.UI.TextBox();
            this.txtUserInCharge = new Sci.Production.Class.Txtuser();
            this.txtCNo = new Sci.Win.UI.TextBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtSeqStart = new Sci.Win.UI.TextBox();
            this.txtSeqEnd = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(418, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(418, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(418, 84);
            // 
            // labelReceiver
            // 
            this.labelReceiver.Lines = 0;
            this.labelReceiver.Location = new System.Drawing.Point(13, 13);
            this.labelReceiver.Name = "labelReceiver";
            this.labelReceiver.Size = new System.Drawing.Size(65, 23);
            this.labelReceiver.TabIndex = 94;
            this.labelReceiver.Text = "Receiver";
            // 
            // labelInCharge
            // 
            this.labelInCharge.Lines = 0;
            this.labelInCharge.Location = new System.Drawing.Point(13, 48);
            this.labelInCharge.Name = "labelInCharge";
            this.labelInCharge.Size = new System.Drawing.Size(65, 23);
            this.labelInCharge.TabIndex = 95;
            this.labelInCharge.Text = "In Charge";
            // 
            // labelCNo
            // 
            this.labelCNo.Lines = 0;
            this.labelCNo.Location = new System.Drawing.Point(13, 83);
            this.labelCNo.Name = "labelCNo";
            this.labelCNo.Size = new System.Drawing.Size(65, 23);
            this.labelCNo.TabIndex = 96;
            this.labelCNo.Text = "C/No.";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(13, 118);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(65, 23);
            this.labelSPNo.TabIndex = 97;
            this.labelSPNo.Text = "SP#";
            // 
            // labelSeq
            // 
            this.labelSeq.Lines = 0;
            this.labelSeq.Location = new System.Drawing.Point(13, 153);
            this.labelSeq.Name = "labelSeq";
            this.labelSeq.Size = new System.Drawing.Size(65, 23);
            this.labelSeq.TabIndex = 98;
            this.labelSeq.Text = "Seq1#";
            // 
            // txtReceiver
            // 
            this.txtReceiver.BackColor = System.Drawing.Color.White;
            this.txtReceiver.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtReceiver.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceiver.Location = new System.Drawing.Point(82, 13);
            this.txtReceiver.Name = "txtReceiver";
            this.txtReceiver.Size = new System.Drawing.Size(245, 23);
            this.txtReceiver.TabIndex = 99;
            this.txtReceiver.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtReceiver_PopUp);
            // 
            // txtUserInCharge
            // 
            this.txtUserInCharge.DisplayBox1Binding = "";
            this.txtUserInCharge.Location = new System.Drawing.Point(82, 48);
            this.txtUserInCharge.Name = "txtUserInCharge";
            this.txtUserInCharge.Size = new System.Drawing.Size(300, 23);
            this.txtUserInCharge.TabIndex = 100;
            this.txtUserInCharge.TextBox1Binding = "";
            // 
            // txtCNo
            // 
            this.txtCNo.BackColor = System.Drawing.Color.White;
            this.txtCNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCNo.Location = new System.Drawing.Point(82, 83);
            this.txtCNo.Name = "txtCNo";
            this.txtCNo.Size = new System.Drawing.Size(77, 23);
            this.txtCNo.TabIndex = 101;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(82, 118);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(130, 23);
            this.txtSPNo.TabIndex = 102;
            // 
            // txtSeqStart
            // 
            this.txtSeqStart.BackColor = System.Drawing.Color.White;
            this.txtSeqStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeqStart.Location = new System.Drawing.Point(82, 153);
            this.txtSeqStart.Name = "txtSeqStart";
            this.txtSeqStart.Size = new System.Drawing.Size(48, 23);
            this.txtSeqStart.TabIndex = 103;
            // 
            // txtSeqEnd
            // 
            this.txtSeqEnd.BackColor = System.Drawing.Color.White;
            this.txtSeqEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeqEnd.Location = new System.Drawing.Point(155, 153);
            this.txtSeqEnd.Name = "txtSeqEnd";
            this.txtSeqEnd.Size = new System.Drawing.Size(48, 23);
            this.txtSeqEnd.TabIndex = 104;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(133, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(19, 23);
            this.label6.TabIndex = 105;
            this.label6.Text = "～";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // P02_BatchPrint
            // 
            this.ClientSize = new System.Drawing.Size(510, 211);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSeqEnd);
            this.Controls.Add(this.txtSeqStart);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.txtCNo);
            this.Controls.Add(this.txtUserInCharge);
            this.Controls.Add(this.txtReceiver);
            this.Controls.Add(this.labelSeq);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelCNo);
            this.Controls.Add(this.labelInCharge);
            this.Controls.Add(this.labelReceiver);
            this.IsSupportToExcel = false;
            this.Name = "P02_BatchPrint";
            this.Text = "Batch Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelReceiver, 0);
            this.Controls.SetChildIndex(this.labelInCharge, 0);
            this.Controls.SetChildIndex(this.labelCNo, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelSeq, 0);
            this.Controls.SetChildIndex(this.txtReceiver, 0);
            this.Controls.SetChildIndex(this.txtUserInCharge, 0);
            this.Controls.SetChildIndex(this.txtCNo, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.txtSeqStart, 0);
            this.Controls.SetChildIndex(this.txtSeqEnd, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelReceiver;
        private Win.UI.Label labelInCharge;
        private Win.UI.Label labelCNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelSeq;
        private Win.UI.TextBox txtReceiver;
        private Class.Txtuser txtUserInCharge;
        private Win.UI.TextBox txtCNo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtSeqStart;
        private Win.UI.TextBox txtSeqEnd;
        private Win.UI.Label label6;
    }
}
