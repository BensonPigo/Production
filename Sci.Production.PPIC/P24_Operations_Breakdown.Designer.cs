namespace Sci.Production.PPIC
{
    partial class P24_Operations_Breakdown
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
            this.btn_StandardGSDList = new Sci.Win.UI.Button();
            this.btn_FactoryGSDList = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // btn_StandardGSDList
            // 
            this.btn_StandardGSDList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btn_StandardGSDList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_StandardGSDList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_StandardGSDList.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_StandardGSDList.ForeColor = System.Drawing.Color.White;
            this.btn_StandardGSDList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_StandardGSDList.Location = new System.Drawing.Point(44, 57);
            this.btn_StandardGSDList.Name = "btn_StandardGSDList";
            this.btn_StandardGSDList.Size = new System.Drawing.Size(243, 51);
            this.btn_StandardGSDList.TabIndex = 16;
            this.btn_StandardGSDList.Text = "Standard GSD List";
            this.btn_StandardGSDList.UseVisualStyleBackColor = false;
            this.btn_StandardGSDList.Click += new System.EventHandler(this.btn_StandardGSDList_Click);
            // 
            // btn_FactoryGSDList
            // 
            this.btn_FactoryGSDList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.btn_FactoryGSDList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_FactoryGSDList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_FactoryGSDList.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_FactoryGSDList.ForeColor = System.Drawing.Color.White;
            this.btn_FactoryGSDList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_FactoryGSDList.Location = new System.Drawing.Point(309, 57);
            this.btn_FactoryGSDList.Name = "btn_FactoryGSDList";
            this.btn_FactoryGSDList.Size = new System.Drawing.Size(249, 51);
            this.btn_FactoryGSDList.TabIndex = 17;
            this.btn_FactoryGSDList.Text = "Factory GSD List";
            this.btn_FactoryGSDList.UseVisualStyleBackColor = false;
            this.btn_FactoryGSDList.Click += new System.EventHandler(this.btn_FactoryGSDList_Click);
            // 
            // P24_Operations_Breakdown
            // 
            this.ClientSize = new System.Drawing.Size(590, 220);
            this.Controls.Add(this.btn_FactoryGSDList);
            this.Controls.Add(this.btn_StandardGSDList);
            this.Name = "P24_Operations_Breakdown";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Operations Breakdown";
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btn_StandardGSDList;
        private Win.UI.Button btn_FactoryGSDList;
    }
}
