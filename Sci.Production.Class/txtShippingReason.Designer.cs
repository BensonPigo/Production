﻿namespace Sci.Production.Class
{
    partial class TxtShippingReason
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.DisplayBox1 = new Sci.Win.UI.DisplayBox();
            this.TextBox1 = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // displayBox1
            // 
            this.DisplayBox1.Location = new System.Drawing.Point(73, 2);
            this.DisplayBox1.Name = "displayBox1";
            this.DisplayBox1.Size = new System.Drawing.Size(391, 22);
            this.DisplayBox1.TabIndex = 3;
            this.DisplayBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // textBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(1, 2);
            this.TextBox1.Name = "textBox1";
            this.TextBox1.Size = new System.Drawing.Size(70, 22);
            this.TextBox1.TabIndex = 2;
            this.TextBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TextBox1_PopUp);
            this.TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            this.TextBox1.Validating += new System.ComponentModel.CancelEventHandler(this.TextBox1_Validating);
            // 
            // txtShippingReason
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.DisplayBox1);
            this.Controls.Add(this.TextBox1);
            this.Name = "txtShippingReason";
            this.Size = new System.Drawing.Size(467, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
