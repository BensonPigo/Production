namespace Sci.Production.Logistic
{
    partial class B01_Print
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
            this.labelCode = new Sci.Win.UI.Label();
            this.txtCodeStart = new Sci.Win.UI.TextBox();
            this.txtCodeEnd = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(297, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(297, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(297, 84);
            // 
            // labelCode
            // 
            this.labelCode.Lines = 0;
            this.labelCode.Location = new System.Drawing.Point(29, 48);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(42, 23);
            this.labelCode.TabIndex = 94;
            this.labelCode.Text = "Code:";
            // 
            // txtCodeStart
            // 
            this.txtCodeStart.BackColor = System.Drawing.Color.White;
            this.txtCodeStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCodeStart.Location = new System.Drawing.Point(75, 48);
            this.txtCodeStart.Name = "txtCodeStart";
            this.txtCodeStart.Size = new System.Drawing.Size(80, 23);
            this.txtCodeStart.TabIndex = 95;
            // 
            // txtCodeEnd
            // 
            this.txtCodeEnd.BackColor = System.Drawing.Color.White;
            this.txtCodeEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCodeEnd.Location = new System.Drawing.Point(179, 48);
            this.txtCodeEnd.Name = "txtCodeEnd";
            this.txtCodeEnd.Size = new System.Drawing.Size(80, 23);
            this.txtCodeEnd.TabIndex = 96;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(157, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 23);
            this.label2.TabIndex = 97;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            this.label2.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // B01_Print
            // 
            this.ClientSize = new System.Drawing.Size(389, 151);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCodeEnd);
            this.Controls.Add(this.txtCodeStart);
            this.Controls.Add(this.labelCode);
            this.IsSupportToExcel = false;
            this.Name = "B01_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelCode, 0);
            this.Controls.SetChildIndex(this.txtCodeStart, 0);
            this.Controls.SetChildIndex(this.txtCodeEnd, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCode;
        private Win.UI.TextBox txtCodeStart;
        private Win.UI.TextBox txtCodeEnd;
        private Win.UI.Label label2;
    }
}
