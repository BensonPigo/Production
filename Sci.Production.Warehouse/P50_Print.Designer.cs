namespace Sci.Production.Warehouse
{
    partial class P50_Print
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
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.radioStocktakingList = new Sci.Win.UI.RadioButton();
            this.radioBookQty = new Sci.Win.UI.RadioButton();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.radioStocktakingList);
            this.radioGroup1.Controls.Add(this.radioBookQty);
            this.radioGroup1.Location = new System.Drawing.Point(33, 28);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(434, 202);
            this.radioGroup1.TabIndex = 95;
            this.radioGroup1.TabStop = false;
            this.radioGroup1.Text = "Report Type";
            this.radioGroup1.Value = "Forward Stocktaking without Book Qty";
            this.radioGroup1.ValueChanged += new System.EventHandler(this.RadioGroup1_ValueChanged);
            // 
            // radioStocktakingList
            // 
            this.radioStocktakingList.AutoSize = true;
            this.radioStocktakingList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioStocktakingList.Location = new System.Drawing.Point(7, 97);
            this.radioStocktakingList.Name = "radioStocktakingList";
            this.radioStocktakingList.Size = new System.Drawing.Size(180, 21);
            this.radioStocktakingList.TabIndex = 1;
            this.radioStocktakingList.TabStop = true;
            this.radioStocktakingList.Text = "Forward Stocktaking List";
            this.radioStocktakingList.UseVisualStyleBackColor = true;
            this.radioStocktakingList.Value = "Forward Stocktaking List";
            // 
            // radioBookQty
            // 
            this.radioBookQty.AutoSize = true;
            this.radioBookQty.Checked = true;
            this.radioBookQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBookQty.Location = new System.Drawing.Point(7, 56);
            this.radioBookQty.Name = "radioBookQty";
            this.radioBookQty.Size = new System.Drawing.Size(264, 21);
            this.radioBookQty.TabIndex = 0;
            this.radioBookQty.TabStop = true;
            this.radioBookQty.Text = "Forward Stocktaking without Book Qty";
            this.radioBookQty.UseVisualStyleBackColor = true;
            this.radioBookQty.Value = "Forward Stocktaking without Book Qty";
            // 
            // P50_Print
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.radioGroup1);
            this.Name = "P50_Print";
            this.Text = "() ";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioGroup1, 0);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton radioStocktakingList;
        private Win.UI.RadioButton radioBookQty;

    }
}
