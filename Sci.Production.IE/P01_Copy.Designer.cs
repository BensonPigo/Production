using System;
using Sci.Win.UI;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_P01_Copy
    /// </summary>
    partial class P01_Copy
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
            this.label1 = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.comboStyle = new Sci.Win.UI.ComboBox();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "New FTY GSD";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(25, 36);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(51, 23);
            this.labelStyle.TabIndex = 1;
            this.labelStyle.Text = "Style";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(25, 73);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(51, 23);
            this.labelSeason.TabIndex = 2;
            this.labelSeason.Text = "Season";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(25, 111);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(51, 23);
            this.labelBrand.TabIndex = 3;
            this.labelBrand.Text = "Brand";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(79, 111);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(80, 23);
            this.txtBrand.TabIndex = 3;
            this.txtBrand.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtBrand_PopUp1);
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(79, 36);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(130, 23);
            this.txtStyle.TabIndex = 0;
            this.txtStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtStyle_PopUp1);
            this.txtStyle.Validated += new System.EventHandler(this.TxtStyle_Validated1);
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(79, 73);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 2;
            this.txtseason.Validated += new System.EventHandler(this.Txtseason_Validated1);
            // 
            // comboStyle
            // 
            this.comboStyle.BackColor = System.Drawing.Color.White;
            this.comboStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStyle.FormattingEnabled = true;
            this.comboStyle.IsSupportUnselect = true;
            this.comboStyle.Location = new System.Drawing.Point(213, 35);
            this.comboStyle.Name = "comboStyle";
            this.comboStyle.Size = new System.Drawing.Size(40, 24);
            this.comboStyle.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(288, 36);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click1);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(288, 72);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // P01_Copy
            // 
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(374, 152);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.comboStyle);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.label1);
            this.DefaultControl = "txtStyle";
            this.Name = "P01_Copy";
            this.Text = "Copy to";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelBrand;
        private Win.UI.TextBox txtBrand;
        private Win.UI.TextBox txtStyle;
        private Class.Txtseason txtseason;
        private Win.UI.ComboBox comboStyle;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;
    }
}
