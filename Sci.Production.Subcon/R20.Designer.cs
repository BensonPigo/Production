namespace Sci.Production.Subcon
{
    partial class R20
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labSupp = new Sci.Win.UI.Label();
            this.labType = new Sci.Win.UI.Label();
            this.comboBoxType = new Sci.Win.UI.ComboBox();
            this.txtlocalSupp = new Sci.Production.Class.TxtLocalSuppNoConfirm();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(325, 92);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(325, 20);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(325, 56);
            // 
            // labSupp
            // 
            this.labSupp.Location = new System.Drawing.Point(18, 26);
            this.labSupp.Name = "labSupp";
            this.labSupp.Size = new System.Drawing.Size(75, 23);
            this.labSupp.TabIndex = 95;
            this.labSupp.Text = "Supplier";
            // 
            // labType
            // 
            this.labType.Location = new System.Drawing.Point(18, 72);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(75, 23);
            this.labType.TabIndex = 96;
            this.labType.Text = "Type";
            // 
            // comboBoxType
            // 
            this.comboBoxType.BackColor = System.Drawing.Color.White;
            this.comboBoxType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.IsSupportUnselect = true;
            this.comboBoxType.Location = new System.Drawing.Point(96, 71);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.OldText = "";
            this.comboBoxType.Size = new System.Drawing.Size(117, 24);
            this.comboBoxType.TabIndex = 97;
            // 
            // txtlocalSupp
            // 
            this.txtlocalSupp.DisplayBox1Binding = "";
            this.txtlocalSupp.Location = new System.Drawing.Point(96, 26);
            this.txtlocalSupp.Name = "txtlocalSupp";
            this.txtlocalSupp.Size = new System.Drawing.Size(223, 23);
            this.txtlocalSupp.TabIndex = 98;
            this.txtlocalSupp.TextBox1Binding = "";
            // 
            // R20
            // 
            this.ClientSize = new System.Drawing.Size(417, 153);
            this.Controls.Add(this.txtlocalSupp);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.labType);
            this.Controls.Add(this.labSupp);
            this.Name = "R20";
            this.Text = "R20 Local Purchase Item Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labSupp, 0);
            this.Controls.SetChildIndex(this.labType, 0);
            this.Controls.SetChildIndex(this.comboBoxType, 0);
            this.Controls.SetChildIndex(this.txtlocalSupp, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label labSupp;
        private Win.UI.Label labType;
        private Win.UI.ComboBox comboBoxType;
        private Class.TxtLocalSuppNoConfirm txtlocalSupp;
    }
}
