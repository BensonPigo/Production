namespace Sci.Production.PPIC
{
    partial class P04_WeightData_CopySeason
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
            this.labelFromSeason = new Sci.Win.UI.Label();
            this.txtFromSeason = new Sci.Win.UI.TextBox();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // labelFromSeason
            // 
            this.labelFromSeason.Lines = 0;
            this.labelFromSeason.Location = new System.Drawing.Point(13, 21);
            this.labelFromSeason.Name = "labelFromSeason";
            this.labelFromSeason.Size = new System.Drawing.Size(88, 23);
            this.labelFromSeason.TabIndex = 0;
            this.labelFromSeason.Text = "From Season";
            // 
            // txtFromSeason
            // 
            this.txtFromSeason.BackColor = System.Drawing.Color.White;
            this.txtFromSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFromSeason.Location = new System.Drawing.Point(105, 21);
            this.txtFromSeason.Name = "txtFromSeason";
            this.txtFromSeason.Size = new System.Drawing.Size(90, 23);
            this.txtFromSeason.TabIndex = 1;
            this.txtFromSeason.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFromSeason_PopUp);
            this.txtFromSeason.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFromSeason_Validating);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(72, 62);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(158, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // P04_WeightData_CopySeason
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(251, 98);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtFromSeason);
            this.Controls.Add(this.labelFromSeason);
            this.DefaultControl = "txtFromSeason";
            this.Name = "P04_WeightData_CopySeason";
            this.Text = "Copy Season";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelFromSeason;
        private Win.UI.TextBox txtFromSeason;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;
    }
}
