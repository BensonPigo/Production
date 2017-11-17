namespace Sci.Production.Shipping
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelNLCode = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.labelContractNo = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateRange();
            this.txtNLCode = new Sci.Win.UI.TextBox();
            this.txtContractNo = new Sci.Win.UI.TextBox();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(376, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(376, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(376, 84);
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(19, 12);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(101, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelNLCode
            // 
            this.labelNLCode.Location = new System.Drawing.Point(19, 48);
            this.labelNLCode.Name = "labelNLCode";
            this.labelNLCode.Size = new System.Drawing.Size(101, 23);
            this.labelNLCode.TabIndex = 95;
            this.labelNLCode.Text = "Customs Code";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(19, 84);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(101, 23);
            this.labelType.TabIndex = 96;
            this.labelType.Text = "Type";
            // 
            // labelContractNo
            // 
            this.labelContractNo.Location = new System.Drawing.Point(19, 120);
            this.labelContractNo.Name = "labelContractNo";
            this.labelContractNo.Size = new System.Drawing.Size(101, 23);
            this.labelContractNo.TabIndex = 97;
            this.labelContractNo.Text = "Contract no.";
            // 
            // dateDate
            // 
            this.dateDate.IsRequired = false;
            this.dateDate.Location = new System.Drawing.Point(123, 12);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(226, 23);
            this.dateDate.TabIndex = 98;
            // 
            // txtNLCode
            // 
            this.txtNLCode.BackColor = System.Drawing.Color.White;
            this.txtNLCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNLCode.Location = new System.Drawing.Point(123, 48);
            this.txtNLCode.Name = "txtNLCode";
            this.txtNLCode.Size = new System.Drawing.Size(70, 23);
            this.txtNLCode.TabIndex = 99;
            // 
            // txtContractNo
            // 
            this.txtContractNo.BackColor = System.Drawing.Color.White;
            this.txtContractNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContractNo.Location = new System.Drawing.Point(123, 120);
            this.txtContractNo.Name = "txtContractNo";
            this.txtContractNo.Size = new System.Drawing.Size(150, 23);
            this.txtContractNo.TabIndex = 100;
            this.txtContractNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtContractNo_PopUp);
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(123, 83);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(100, 24);
            this.comboType.TabIndex = 101;
            // 
            // R41
            // 
            this.ClientSize = new System.Drawing.Size(468, 183);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.txtContractNo);
            this.Controls.Add(this.txtNLCode);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelContractNo);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelNLCode);
            this.Controls.Add(this.labelDate);
            this.IsSupportToPrint = false;
            this.Name = "R41";
            this.Text = "R41. Customs Code Import/Export/Adjust detail list";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelNLCode, 0);
            this.Controls.SetChildIndex(this.labelType, 0);
            this.Controls.SetChildIndex(this.labelContractNo, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.txtNLCode, 0);
            this.Controls.SetChildIndex(this.txtContractNo, 0);
            this.Controls.SetChildIndex(this.comboType, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelNLCode;
        private Win.UI.Label labelType;
        private Win.UI.Label labelContractNo;
        private Win.UI.DateRange dateDate;
        private Win.UI.TextBox txtNLCode;
        private Win.UI.TextBox txtContractNo;
        private Win.UI.ComboBox comboType;
    }
}
