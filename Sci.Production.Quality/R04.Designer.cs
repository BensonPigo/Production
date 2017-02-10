namespace Sci.Production.Quality
{
    partial class R04
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.checkOutstandingOnly = new Sci.Win.UI.CheckBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.label3 = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.label10 = new Sci.Win.UI.Label();
            this.DateArriveWH = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.DateReceivedSample = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.label4 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.checkOutstandingOnly);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.DateArriveWH);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DateReceivedSample);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(25, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 262);
            this.panel1.TabIndex = 94;
            // 
            // checkOutstandingOnly
            // 
            this.checkOutstandingOnly.AutoSize = true;
            this.checkOutstandingOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkOutstandingOnly.Location = new System.Drawing.Point(192, 220);
            this.checkOutstandingOnly.Name = "checkOutstandingOnly";
            this.checkOutstandingOnly.Size = new System.Drawing.Size(134, 21);
            this.checkOutstandingOnly.TabIndex = 52;
            this.checkOutstandingOnly.Text = "Outstanding only";
            this.checkOutstandingOnly.UseVisualStyleBackColor = true;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(192, 176);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 51;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(27, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 22);
            this.label3.TabIndex = 50;
            this.label3.Text = "Factory";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(192, 98);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(125, 24);
            this.comboCategory.TabIndex = 49;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(27, 98);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(158, 23);
            this.label10.TabIndex = 48;
            this.label10.Text = "Category";
            // 
            // DateArriveWH
            // 
            this.DateArriveWH.Location = new System.Drawing.Point(192, 58);
            this.DateArriveWH.Name = "DateArriveWH";
            this.DateArriveWH.Size = new System.Drawing.Size(280, 23);
            this.DateArriveWH.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(27, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 23);
            this.label2.TabIndex = 14;
            this.label2.Text = "Arrive W/H Date";
            // 
            // DateReceivedSample
            // 
            this.DateReceivedSample.Location = new System.Drawing.Point(192, 20);
            this.DateReceivedSample.Name = "DateReceivedSample";
            this.DateReceivedSample.Size = new System.Drawing.Size(280, 23);
            this.DateReceivedSample.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(27, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Received Sample Date";
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(192, 138);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(27, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(158, 22);
            this.label4.TabIndex = 53;
            this.label4.Text = "M";
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(627, 319);
            this.Controls.Add(this.panel1);
            this.Name = "R04";
            this.Text = "R04.Laboratory-Fabric Test Report";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.DateRange DateReceivedSample;
        private Win.UI.Label label1;
        private Win.UI.DateRange DateArriveWH;
        private Win.UI.Label label2;
        private Win.UI.CheckBox checkOutstandingOnly;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label label3;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label label10;
        private Win.UI.ComboBox comboM;
        private Win.UI.Label label4;
    }
}
