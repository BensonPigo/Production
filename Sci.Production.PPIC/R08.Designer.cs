namespace Sci.Production.PPIC
{
    partial class R08
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
            this.labelCreateDate = new Sci.Win.UI.Label();
            this.labelApvDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.dateCreateDate = new Sci.Win.UI.DateRange();
            this.dateApvDate = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(403, 12);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(403, 48);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(403, 84);
            this.close.TabIndex = 7;
            // 
            // labelCreateDate
            // 
            this.labelCreateDate.Lines = 0;
            this.labelCreateDate.Location = new System.Drawing.Point(13, 12);
            this.labelCreateDate.Name = "labelCreateDate";
            this.labelCreateDate.Size = new System.Drawing.Size(80, 23);
            this.labelCreateDate.TabIndex = 94;
            this.labelCreateDate.Text = "Create Date";
            // 
            // labelApvDate
            // 
            this.labelApvDate.Lines = 0;
            this.labelApvDate.Location = new System.Drawing.Point(13, 48);
            this.labelApvDate.Name = "labelApvDate";
            this.labelApvDate.Size = new System.Drawing.Size(80, 23);
            this.labelApvDate.TabIndex = 95;
            this.labelApvDate.Text = "Apv. Date";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(80, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // labelType
            // 
            this.labelType.Lines = 0;
            this.labelType.Location = new System.Drawing.Point(13, 159);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(80, 23);
            this.labelType.TabIndex = 97;
            this.labelType.Text = "Type";
            // 
            // dateCreateDate
            // 
            this.dateCreateDate.IsRequired = false;
            this.dateCreateDate.Location = new System.Drawing.Point(97, 12);
            this.dateCreateDate.Name = "dateCreateDate";
            this.dateCreateDate.Size = new System.Drawing.Size(280, 23);
            this.dateCreateDate.TabIndex = 0;
            // 
            // dateApvDate
            // 
            this.dateApvDate.IsRequired = false;
            this.dateApvDate.Location = new System.Drawing.Point(97, 48);
            this.dateApvDate.Name = "dateApvDate";
            this.dateApvDate.Size = new System.Drawing.Size(280, 23);
            this.dateApvDate.TabIndex = 1;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(97, 84);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(66, 24);
            this.comboM.TabIndex = 2;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(97, 158);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(112, 24);
            this.comboType.TabIndex = 4;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(80, 23);
            this.labelFactory.TabIndex = 102;
            this.labelFactory.Text = "Factory";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(97, 120);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(66, 24);
            this.comboFactory.TabIndex = 3;
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(495, 227);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.comboType);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateApvDate);
            this.Controls.Add(this.dateCreateDate);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelApvDate);
            this.Controls.Add(this.labelCreateDate);
            this.DefaultControl = "dateCreateDate";
            this.DefaultControlForEdit = "dateCreateDate";
            this.IsSupportToPrint = false;
            this.Name = "R08";
            this.Text = "R08. Replacement Report List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelCreateDate, 0);
            this.Controls.SetChildIndex(this.labelApvDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelType, 0);
            this.Controls.SetChildIndex(this.dateCreateDate, 0);
            this.Controls.SetChildIndex(this.dateApvDate, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboType, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCreateDate;
        private Win.UI.Label labelApvDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelType;
        private Win.UI.DateRange dateCreateDate;
        private Win.UI.DateRange dateApvDate;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboType;
        private Win.UI.Label labelFactory;
        private Win.UI.ComboBox comboFactory;
    }
}
