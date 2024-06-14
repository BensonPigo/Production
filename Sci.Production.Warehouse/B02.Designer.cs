﻿namespace Sci.Production.Warehouse
{
    partial class B02
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.btnBatchCreate = new Sci.Win.UI.Button();
            this.chkIsWMS = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtCapacity = new Sci.Win.UI.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Location = new System.Drawing.Point(4, 29);
            this.detail.Size = new System.Drawing.Size(976, 391);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtCapacity);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.chkIsWMS);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.comboStockType);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.labelStockType);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Size = new System.Drawing.Size(976, 353);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 353);
            this.detailbtm.Size = new System.Drawing.Size(976, 38);
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(4, 29);
            this.browse.Size = new System.Drawing.Size(976, 391);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(984, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 26);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 26);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(20, 14);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(75, 23);
            this.labelCode.TabIndex = 4;
            this.labelCode.Text = "Code";
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(20, 49);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(75, 23);
            this.labelStockType.TabIndex = 5;
            this.labelStockType.Text = "Stock Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(20, 84);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 6;
            this.labelDescription.Text = "Description";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(98, 14);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(121, 26);
            this.txtCode.TabIndex = 7;
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "stocktype", true));
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Location = new System.Drawing.Point(98, 49);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(121, 28);
            this.comboStockType.TabIndex = 8;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(98, 84);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(258, 26);
            this.txtDescription.TabIndex = 9;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(299, 14);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(66, 24);
            this.checkJunk.TabIndex = 8;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // btnBatchCreate
            // 
            this.btnBatchCreate.Location = new System.Drawing.Point(831, 12);
            this.btnBatchCreate.Name = "btnBatchCreate";
            this.btnBatchCreate.Size = new System.Drawing.Size(112, 30);
            this.btnBatchCreate.TabIndex = 2;
            this.btnBatchCreate.Text = "Batch Create";
            this.btnBatchCreate.UseVisualStyleBackColor = true;
            this.btnBatchCreate.Click += new System.EventHandler(this.BtnBatchCreate_Click);
            // 
            // chkIsWMS
            // 
            this.chkIsWMS.AutoSize = true;
            this.chkIsWMS.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsWMS", true));
            this.chkIsWMS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsWMS.Location = new System.Drawing.Point(299, 49);
            this.chkIsWMS.Name = "chkIsWMS";
            this.chkIsWMS.Size = new System.Drawing.Size(159, 24);
            this.chkIsWMS.TabIndex = 10;
            this.chkIsWMS.Text = "Is WMS Location";
            this.chkIsWMS.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "Capacity";
            // 
            // txtCapacity
            // 
            this.txtCapacity.BackColor = System.Drawing.Color.White;
            this.txtCapacity.DecimalPlaces = 1;
            this.txtCapacity.DisplayStyle = Ict.Win.UI.NumericBoxDisplayStyle.ThousandSeparator;
            this.txtCapacity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCapacity.Location = new System.Drawing.Point(98, 117);
            this.txtCapacity.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.txtCapacity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtCapacity.Name = "txtCapacity";
            this.txtCapacity.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtCapacity.Size = new System.Drawing.Size(100, 26);
            this.txtCapacity.TabIndex = 14;
            this.txtCapacity.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B02
            // 
            this.ClientSize = new System.Drawing.Size(984, 457);
            this.Controls.Add(this.btnBatchCreate);
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B02";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B02.Material Location Index";
            this.WorkAlias = "mtlLocation";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchCreate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtDescription;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelStockType;
        private Win.UI.Label labelDescription;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Button btnBatchCreate;
        private Win.UI.CheckBox chkIsWMS;
        private Win.UI.Label label1;
        private Win.UI.NumericBox txtCapacity;
    }
}
