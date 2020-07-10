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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboM = new Sci.Win.UI.ComboBox();
            this.labelM = new Sci.Win.UI.Label();
            this.checkOutstandingOnly = new Sci.Win.UI.CheckBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.dateArriveWHDate = new Sci.Win.UI.DateRange();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.dateReceivedSampleDate = new Sci.Win.UI.DateRange();
            this.labelReceivedSampleDate = new Sci.Win.UI.Label();
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkIncludeCancelOrder);
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.checkOutstandingOnly);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.dateArriveWHDate);
            this.panel1.Controls.Add(this.labelArriveWHDate);
            this.panel1.Controls.Add(this.dateReceivedSampleDate);
            this.panel1.Controls.Add(this.labelReceivedSampleDate);
            this.panel1.Location = new System.Drawing.Point(25, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 277);
            this.panel1.TabIndex = 94;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(192, 98);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(185, 24);
            this.comboCategory.TabIndex = 55;
            this.comboCategory.Type = "Pms_MtlCategory";
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(192, 138);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 54;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(27, 138);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(158, 22);
            this.labelM.TabIndex = 53;
            this.labelM.Text = "M";
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
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 51;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(27, 176);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(158, 22);
            this.labelFactory.TabIndex = 50;
            this.labelFactory.Text = "Factory";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(27, 98);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(158, 23);
            this.labelCategory.TabIndex = 48;
            this.labelCategory.Text = "Category";
            // 
            // dateArriveWHDate
            // 
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArriveWHDate.DateBox1.Name = "";
            this.dateArriveWHDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArriveWHDate.DateBox2.Name = "";
            this.dateArriveWHDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox2.TabIndex = 1;
            this.dateArriveWHDate.IsRequired = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(192, 58);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(280, 23);
            this.dateArriveWHDate.TabIndex = 15;
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(27, 58);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(158, 23);
            this.labelArriveWHDate.TabIndex = 14;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // dateReceivedSampleDate
            // 
            // 
            // 
            // 
            this.dateReceivedSampleDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateReceivedSampleDate.DateBox1.Name = "";
            this.dateReceivedSampleDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateReceivedSampleDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateReceivedSampleDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateReceivedSampleDate.DateBox2.Name = "";
            this.dateReceivedSampleDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateReceivedSampleDate.DateBox2.TabIndex = 1;
            this.dateReceivedSampleDate.IsRequired = false;
            this.dateReceivedSampleDate.Location = new System.Drawing.Point(192, 20);
            this.dateReceivedSampleDate.Name = "dateReceivedSampleDate";
            this.dateReceivedSampleDate.Size = new System.Drawing.Size(280, 23);
            this.dateReceivedSampleDate.TabIndex = 1;
            // 
            // labelReceivedSampleDate
            // 
            this.labelReceivedSampleDate.Location = new System.Drawing.Point(27, 20);
            this.labelReceivedSampleDate.Name = "labelReceivedSampleDate";
            this.labelReceivedSampleDate.Size = new System.Drawing.Size(158, 23);
            this.labelReceivedSampleDate.TabIndex = 0;
            this.labelReceivedSampleDate.Text = "Received Sample Date";
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(192, 247);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 167;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(627, 331);
            this.Controls.Add(this.panel1);
            this.Name = "R04";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R04.Laboratory-Fabric Test Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
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
        private Win.UI.DateRange dateReceivedSampleDate;
        private Win.UI.Label labelReceivedSampleDate;
        private Win.UI.DateRange dateArriveWHDate;
        private Win.UI.Label labelArriveWHDate;
        private Win.UI.CheckBox checkOutstandingOnly;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCategory;
        private Win.UI.ComboBox comboM;
        private Win.UI.Label labelM;
        private Class.ComboDropDownList comboCategory;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
