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
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(461, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(461, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(461, 84);
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.labelEstCutDate);
            this.panel1.Controls.Add(this.dateEstCutDate);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 127);
            this.panel1.TabIndex = 0;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(107, 78);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 2;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(107, 42);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 1;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(10, 79);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(95, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(10, 43);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(94, 23);
            this.labelM.TabIndex = 97;
            this.labelM.Text = "M";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Lines = 0;
            this.labelEstCutDate.Location = new System.Drawing.Point(10, 7);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(94, 23);
            this.labelEstCutDate.TabIndex = 3;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.IsRequired = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(107, 7);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 0;
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
            this.DefaultControl = "dateR_EstCutDate";
            this.DefaultControlForEdit = "dateR_EstCutDate";
            this.Name = "R05";
            this.Text = "R05.Cutting Monthly Forecast";
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
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.ComboBox comboM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.Label label4;
    }
}
