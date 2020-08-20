namespace Sci.Production.PublicForm
{
    partial class EditRemark
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
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.labelRemark = new Sci.Win.UI.Label();
            this.edit_Remark = new Sci.Win.UI.EditBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(415, 182);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(329, 182);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(9, 14);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 2;
            this.labelRemark.Text = "Remark";
            // 
            // edit_Remark
            // 
            this.edit_Remark.BackColor = System.Drawing.Color.White;
            this.edit_Remark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.edit_Remark.Location = new System.Drawing.Point(87, 14);
            this.edit_Remark.Multiline = true;
            this.edit_Remark.Name = "edit_Remark";
            this.edit_Remark.Size = new System.Drawing.Size(423, 155);
            this.edit_Remark.TabIndex = 4;
            // 
            // EditRemark
            // 
            this.ClientSize = new System.Drawing.Size(522, 228);
            this.Controls.Add(this.edit_Remark);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Name = "EditRemark";
            this.Text = "Edit Remark";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Label labelRemark;
        private Win.UI.EditBox edit_Remark;
    }
}
