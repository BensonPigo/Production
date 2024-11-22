namespace Sci.Production.Cutting
{
    partial class B12_ImportFromPMS
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
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelDate = new Sci.Win.UI.Label();
            this.btnCancel = new Sci.Win.UI.Button();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.btnImport = new Sci.Win.UI.Button();
            this.comboFactory1 = new Sci.Production.Class.ComboFactory(this.components);
            this.SuspendLayout();
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(29, 48);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 12;
            this.labelDescription.Text = "Factory";
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(29, 13);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(75, 23);
            this.labelDate.TabIndex = 8;
            this.labelDate.Text = "Date";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(307, 84);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // dateRange1
            // 
            // 
            // 
            // 
            this.dateRange1.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange1.DateBox1.Name = "";
            this.dateRange1.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRange1.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange1.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRange1.DateBox2.Name = "";
            this.dateRange1.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRange1.DateBox2.TabIndex = 1;
            this.dateRange1.IsSupportEditMode = false;
            this.dateRange1.Location = new System.Drawing.Point(107, 13);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 13;
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(221, 84);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 14;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // comboFactory1
            // 
            this.comboFactory1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboFactory1.FilteMDivision = false;
            this.comboFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboFactory1.FormattingEnabled = true;
            this.comboFactory1.IssupportJunk = false;
            this.comboFactory1.IsSupportUnselect = true;
            this.comboFactory1.Location = new System.Drawing.Point(107, 48);
            this.comboFactory1.Name = "comboFactory1";
            this.comboFactory1.OldText = "";
            this.comboFactory1.ReadOnly = true;
            this.comboFactory1.Size = new System.Drawing.Size(80, 24);
            this.comboFactory1.TabIndex = 16;
            // 
            // B12_ImportFromPMS
            // 
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(421, 127);
            this.Controls.Add(this.comboFactory1);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.dateRange1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelDate);
            this.DefaultControl = "txtDescription";
            this.Name = "B12_ImportFromPMS";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Import from PMS Basic B05";
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelDescription, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.dateRange1, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.comboFactory1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labelDescription;
        private Win.UI.Label labelDate;
        private Win.UI.Button btnCancel;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Button btnImport;
        private Class.ComboFactory comboFactory1;
    }
}
