namespace Sci.Production.Cutting
{
    partial class P01_Date
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
            this.labelSewingInLineDateBefore = new Sci.Win.UI.Label();
            this.dateSewingInLineDateBefore = new Sci.Win.UI.DateBox();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // labelSewingInLineDateBefore
            // 
            this.labelSewingInLineDateBefore.Lines = 0;
            this.labelSewingInLineDateBefore.Location = new System.Drawing.Point(26, 22);
            this.labelSewingInLineDateBefore.Name = "labelSewingInLineDateBefore";
            this.labelSewingInLineDateBefore.Size = new System.Drawing.Size(174, 23);
            this.labelSewingInLineDateBefore.TabIndex = 0;
            this.labelSewingInLineDateBefore.Text = "Sewing in-Line date before";
            // 
            // dateSewingInLineDateBefore
            // 
            this.dateSewingInLineDateBefore.Location = new System.Drawing.Point(41, 61);
            this.dateSewingInLineDateBefore.Name = "dateSewingInLineDateBefore";
            this.dateSewingInLineDateBefore.Size = new System.Drawing.Size(130, 23);
            this.dateSewingInLineDateBefore.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(218, 22);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(218, 61);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // P01_Date
            // 
            this.ClientSize = new System.Drawing.Size(310, 121);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dateSewingInLineDateBefore);
            this.Controls.Add(this.labelSewingInLineDateBefore);
            this.DefaultControl = "dateSewingInLineDateBefore";
            this.Name = "P01_Date";
            this.Text = "Cutting Generate";
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labelSewingInLineDateBefore;
        private Win.UI.DateBox dateSewingInLineDateBefore;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;
    }
}
