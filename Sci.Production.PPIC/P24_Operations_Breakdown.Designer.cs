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
            this.btn_Factory_GSD_List = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // btn_StandardGSDList
            // 
            this.btn_StandardGSDList.BackColor = System.Drawing.Color.Gray;
            this.btn_StandardGSDList.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_StandardGSDList.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_StandardGSDList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StandardGSDList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            this.btn_StandardGSDList.ForeColor = System.Drawing.Color.White;
            this.btn_StandardGSDList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_StandardGSDList.Location = new System.Drawing.Point(33, 9);
            this.btn_StandardGSDList.Margin = new System.Windows.Forms.Padding(0);
            this.btn_StandardGSDList.Name = "btn_StandardGSDList";
            this.btn_StandardGSDList.Size = new System.Drawing.Size(243, 48);
            this.btn_StandardGSDList.TabIndex = 21;
            this.btn_StandardGSDList.Text = "Standard GSD List";
            this.btn_StandardGSDList.UseVisualStyleBackColor = false;
            this.btn_StandardGSDList.Click += new System.EventHandler(this.btn_StandardGSDList_Click);
            // 
            // btn_Factory_GSD_List
            // 
            this.btn_Factory_GSD_List.BackColor = System.Drawing.Color.Gray;
            this.btn_Factory_GSD_List.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Factory_GSD_List.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Factory_GSD_List.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Factory_GSD_List.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold);
            this.btn_Factory_GSD_List.ForeColor = System.Drawing.Color.White;
            this.btn_Factory_GSD_List.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Factory_GSD_List.Location = new System.Drawing.Point(289, 9);
            this.btn_Factory_GSD_List.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Factory_GSD_List.Name = "btn_Factory_GSD_List";
            this.btn_Factory_GSD_List.Size = new System.Drawing.Size(243, 48);
            this.btn_Factory_GSD_List.TabIndex = 22;
            this.btn_Factory_GSD_List.Text = "Factory GSD List";
            this.btn_Factory_GSD_List.UseVisualStyleBackColor = false;
            this.btn_Factory_GSD_List.Click += new System.EventHandler(this.btn_FactoryGSDList_Click);
            // 
            // P24_Operations_Breakdown
            // 
            this.ClientSize = new System.Drawing.Size(590, 220);
            this.Controls.Add(this.btn_Factory_GSD_List);
            this.Controls.Add(this.btn_StandardGSDList);
            this.Name = "P24_Operations_Breakdown";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Operations Breakdown";
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btn_StandardGSDList;
        private Win.UI.Button btn_Factory_GSD_List;
    }
}
