namespace Sci.Production.IE
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
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
            this.detail.Size = new System.Drawing.Size(681, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.numericBox1);
            this.detailcont.Controls.Add(this.comboBox1);
            this.detailcont.Controls.Add(this.dateBox1);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(681, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(681, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(681, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(689, 424);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(70, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Date";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(70, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Type";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(352, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(18, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "M";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(70, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 23);
            this.label6.TabIndex = 3;
            this.label6.Text = "Target";
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EffectiveDate", true));
            this.dateBox1.Location = new System.Drawing.Point(161, 37);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(161, 84);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(188, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Target", true));
            this.numericBox1.DecimalPlaces = 2;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(161, 129);
            this.numericBox1.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            this.numericBox1.MaxLength = 7;
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Size = new System.Drawing.Size(66, 23);
            this.numericBox1.TabIndex = 2;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(374, 37);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(40, 23);
            this.displayBox1.TabIndex = 8;
            // 
            // B02
            // 
            this.ClientSize = new System.Drawing.Size(689, 457);
            this.DefaultOrder = "EffectiveDate";
            this.IsSupportPrint = false;
            this.Name = "B02";
            this.Text = "B02. Lean Measurement Target Setting";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ChgOverTarget";
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

        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.DateBox dateBox1;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.DisplayBox displayBox1;
    }
}
