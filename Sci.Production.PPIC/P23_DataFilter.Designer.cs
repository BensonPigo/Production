namespace Sci.Production.PPIC
{
    partial class P23_DataFilter
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.label2 = new System.Windows.Forms.Label();
            this.comboSeason = new System.Windows.Forms.ComboBox();
            this.btn_OK = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 31);
            this.label2.TabIndex = 10;
            this.label2.Text = "Season";
            // 
            // comboSeason
            // 
            this.comboSeason.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboSeason.FormattingEnabled = true;
            this.comboSeason.Location = new System.Drawing.Point(136, 45);
            this.comboSeason.Name = "comboSeason";
            this.comboSeason.Size = new System.Drawing.Size(136, 32);
            this.comboSeason.TabIndex = 11;
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_OK.BackColor = System.Drawing.Color.Gray;
            this.btn_OK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_OK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_OK.Location = new System.Drawing.Point(158, 107);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(114, 42);
            this.btn_OK.TabIndex = 13;
            this.btn_OK.Text = "OK";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // P23_DataFilter
            // 
            this.ClientSize = new System.Drawing.Size(323, 182);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.comboSeason);
            this.Controls.Add(this.label2);
            this.Name = "P23_DataFilter";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Data Filter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboSeason;
        private Win.UI.Button btn_OK;
    }
}
