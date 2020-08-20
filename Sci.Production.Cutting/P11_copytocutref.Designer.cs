namespace Sci.Production.Cutting
{
    partial class P11_copytocutref
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
            this.labelCutRef = new Sci.Win.UI.Label();
            this.txtCutRef = new Sci.Win.UI.TextBox();
            this.btnCopy = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // labelCutRef
            // 
            this.labelCutRef.Lines = 0;
            this.labelCutRef.Location = new System.Drawing.Point(35, 19);
            this.labelCutRef.Name = "labelCutRef";
            this.labelCutRef.Size = new System.Drawing.Size(75, 23);
            this.labelCutRef.TabIndex = 6;
            this.labelCutRef.Text = "CutRef#";
            // 
            // txtCutRef
            // 
            this.txtCutRef.BackColor = System.Drawing.Color.White;
            this.txtCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRef.Location = new System.Drawing.Point(114, 19);
            this.txtCutRef.Name = "txtCutRef";
            this.txtCutRef.Size = new System.Drawing.Size(100, 23);
            this.txtCutRef.TabIndex = 0;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(35, 64);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(80, 30);
            this.btnCopy.TabIndex = 1;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(134, 64);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // P11_copytocutref
            // 
            this.ClientSize = new System.Drawing.Size(254, 110);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelCutRef);
            this.Controls.Add(this.txtCutRef);
            this.Controls.Add(this.btnCopy);
            this.DefaultControl = "txtCutRef";
            this.Name = "P11_copytocutref";
            this.Text = "Copy Cutpart/Artwork to other CutRef#";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCutRef;
        private Win.UI.TextBox txtCutRef;
        private Win.UI.Button btnCopy;
        private Win.UI.Button btnCancel;
    }
}
