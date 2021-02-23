namespace Sci.Production.PPIC
{
    partial class P01_MNoticePrint
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
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.checkAdditionally = new Sci.Win.UI.CheckBox();
            this.radioByOrderCombo = new Sci.Win.UI.RadioButton();
            this.radioMNotice = new Sci.Win.UI.RadioButton();
            this.btnToPDF = new Sci.Win.UI.Button();
            this.btn_toExccel = new Sci.Win.UI.Button();
            this.radioByOrderComboNew = new Sci.Win.UI.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioByOrderComboNew);
            this.groupBox1.Controls.Add(this.checkAdditionally);
            this.groupBox1.Controls.Add(this.radioByOrderCombo);
            this.groupBox1.Controls.Add(this.radioMNotice);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 188);
            this.groupBox1.TabIndex = 96;
            this.groupBox1.TabStop = false;
            // 
            // checkAdditionally
            // 
            this.checkAdditionally.AutoSize = true;
            this.checkAdditionally.Checked = true;
            this.checkAdditionally.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAdditionally.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkAdditionally.Location = new System.Drawing.Point(18, 148);
            this.checkAdditionally.Name = "checkAdditionally";
            this.checkAdditionally.Size = new System.Drawing.Size(329, 22);
            this.checkAdditionally.TabIndex = 8;
            this.checkAdditionally.Text = "Additionally print [Size Spec] with \"z\" beginning";
            this.checkAdditionally.UseVisualStyleBackColor = true;
            // 
            // radioByOrderCombo
            // 
            this.radioByOrderCombo.AutoSize = true;
            this.radioByOrderCombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByOrderCombo.Location = new System.Drawing.Point(18, 64);
            this.radioByOrderCombo.Name = "radioByOrderCombo";
            this.radioByOrderCombo.Size = new System.Drawing.Size(265, 22);
            this.radioByOrderCombo.TabIndex = 7;
            this.radioByOrderCombo.Text = "M/Notice (Combo by OrderCombo )";
            this.radioByOrderCombo.UseVisualStyleBackColor = true;
            // 
            // radioMNotice
            // 
            this.radioMNotice.AutoSize = true;
            this.radioMNotice.Checked = true;
            this.radioMNotice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMNotice.Location = new System.Drawing.Point(18, 22);
            this.radioMNotice.Name = "radioMNotice";
            this.radioMNotice.Size = new System.Drawing.Size(86, 22);
            this.radioMNotice.TabIndex = 6;
            this.radioMNotice.TabStop = true;
            this.radioMNotice.Text = "M/Notice";
            this.radioMNotice.UseVisualStyleBackColor = true;
            // 
            // btnToPDF
            // 
            this.btnToPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToPDF.Location = new System.Drawing.Point(535, 13);
            this.btnToPDF.Name = "btnToPDF";
            this.btnToPDF.Size = new System.Drawing.Size(80, 30);
            this.btnToPDF.TabIndex = 97;
            this.btnToPDF.Text = "To PDF";
            this.btnToPDF.UseVisualStyleBackColor = true;
            this.btnToPDF.Click += new System.EventHandler(this.BtnToPDF_Click);
            // 
            // btn_toExccel
            // 
            this.btn_toExccel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_toExccel.Location = new System.Drawing.Point(535, 48);
            this.btn_toExccel.Name = "btn_toExccel";
            this.btn_toExccel.Size = new System.Drawing.Size(80, 30);
            this.btn_toExccel.TabIndex = 98;
            this.btn_toExccel.Text = "To Excel";
            this.btn_toExccel.UseVisualStyleBackColor = true;
            this.btn_toExccel.Click += new System.EventHandler(this.Btn_toExccel_Click);
            // 
            // radioByOrderComboNew
            // 
            this.radioByOrderComboNew.AutoSize = true;
            this.radioByOrderComboNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByOrderComboNew.Location = new System.Drawing.Point(18, 106);
            this.radioByOrderComboNew.Name = "radioByOrderComboNew";
            this.radioByOrderComboNew.Size = new System.Drawing.Size(360, 22);
            this.radioByOrderComboNew.TabIndex = 9;
            this.radioByOrderComboNew.Text = "M/Notice (Combo by OrderCombo - New Format )";
            this.radioByOrderComboNew.UseVisualStyleBackColor = true;
            // 
            // P01_MNoticePrint
            // 
            this.ClientSize = new System.Drawing.Size(627, 226);
            this.Controls.Add(this.btn_toExccel);
            this.Controls.Add(this.btnToPDF);
            this.Controls.Add(this.groupBox1);
            this.Name = "P01_MNoticePrint";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P01.M/Notice Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.btnToPDF, 0);
            this.Controls.SetChildIndex(this.btn_toExccel, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.GroupBox groupBox1;
        private Win.UI.CheckBox checkAdditionally;
        private Win.UI.RadioButton radioByOrderCombo;
        private Win.UI.RadioButton radioMNotice;
        private Win.UI.Button btnToPDF;
        private Win.UI.Button btn_toExccel;
        private Win.UI.RadioButton radioByOrderComboNew;
    }
}
