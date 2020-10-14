namespace Sci.Production.Class
{
    partial class ComboxMaterialTypeAndID
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
         this.comboMaterialType = new Sci.Win.UI.ComboBox();
         this.comboMtlTypeID = new Sci.Win.UI.ComboBox();
         this.labelMaterialType = new Sci.Win.UI.Label();
         this.SuspendLayout();
         // 
         // comboMaterialType
         // 
         this.comboMaterialType.FormattingEnabled = true;
         this.comboMaterialType.Location = new System.Drawing.Point(78, 3);
         this.comboMaterialType.Name = "comboMaterialType";
         this.comboMaterialType.OldText = "";
         this.comboMaterialType.Size = new System.Drawing.Size(76, 20);
         this.comboMaterialType.TabIndex = 0;
         this.comboMaterialType.SelectedIndexChanged += new System.EventHandler(this.ComboMaterialType_SelectedIndexChanged);
         // 
         // comboMtlTypeID
         // 
         this.comboMtlTypeID.FormattingEnabled = true;
         this.comboMtlTypeID.Location = new System.Drawing.Point(160, 3);
         this.comboMtlTypeID.Name = "comboMtlTypeID";
         this.comboMtlTypeID.OldText = "";
         this.comboMtlTypeID.Size = new System.Drawing.Size(121, 20);
         this.comboMtlTypeID.TabIndex = 1;
         // 
         // labelMaterialType
         // 
         this.labelMaterialType.Location = new System.Drawing.Point(0, 3);
         this.labelMaterialType.Name = "labelMaterialType";
         this.labelMaterialType.Size = new System.Drawing.Size(75, 20);
         this.labelMaterialType.TabIndex = 129;
         this.labelMaterialType.Text = "Material Type";
         // 
         // ComboxMaterialTypeAndID
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.labelMaterialType);
         this.Controls.Add(this.comboMtlTypeID);
         this.Controls.Add(this.comboMaterialType);
         this.Name = "ComboxMaterialTypeAndID";
         this.Size = new System.Drawing.Size(287, 25);
         this.ResumeLayout(false);

        }

        #endregion

        public Win.UI.ComboBox comboMaterialType;
        public Win.UI.ComboBox comboMtlTypeID;
        private Win.UI.Label labelMaterialType;
    }
}
