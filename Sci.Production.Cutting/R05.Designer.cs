namespace Sci.Production.Cutting
{
    partial class R05
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.cmb_Factory = new Sci.Win.UI.ComboBox();
            this.cmb_M = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dateR_EstCutDate = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(461, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(461, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(461, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmb_Factory);
            this.panel1.Controls.Add(this.cmb_M);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.dateR_EstCutDate);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 127);
            this.panel1.TabIndex = 94;
            // 
            // cmb_Factory
            // 
            this.cmb_Factory.BackColor = System.Drawing.Color.White;
            this.cmb_Factory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmb_Factory.FormattingEnabled = true;
            this.cmb_Factory.IsSupportUnselect = true;
            this.cmb_Factory.Location = new System.Drawing.Point(107, 78);
            this.cmb_Factory.Name = "cmb_Factory";
            this.cmb_Factory.Size = new System.Drawing.Size(121, 24);
            this.cmb_Factory.TabIndex = 100;
            // 
            // cmb_M
            // 
            this.cmb_M.BackColor = System.Drawing.Color.White;
            this.cmb_M.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmb_M.FormattingEnabled = true;
            this.cmb_M.IsSupportUnselect = true;
            this.cmb_M.Location = new System.Drawing.Point(107, 42);
            this.cmb_M.Name = "cmb_M";
            this.cmb_M.Size = new System.Drawing.Size(121, 24);
            this.cmb_M.TabIndex = 99;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(10, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Factory";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(10, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "M";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(10, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Est. Cut Date";
            // 
            // dateR_EstCutDate
            // 
            this.dateR_EstCutDate.Location = new System.Drawing.Point(107, 7);
            this.dateR_EstCutDate.Name = "dateR_EstCutDate";
            this.dateR_EstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateR_EstCutDate.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(449, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 22);
            this.label4.TabIndex = 95;
            this.label4.Text = "Paper Size A4";
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(553, 183);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Name = "R05";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.DateRange dateR_EstCutDate;
        private Win.UI.Label label3;
        private Win.UI.ComboBox cmb_Factory;
        private Win.UI.ComboBox cmb_M;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label label4;
    }
}
