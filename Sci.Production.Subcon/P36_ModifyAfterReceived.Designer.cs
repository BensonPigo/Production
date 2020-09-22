namespace Sci.Production.Subcon
{
    partial class P36_ModifyAfterReceived
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
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.mtbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelAccountNo = new Sci.Win.UI.Label();
            this.txtAccountNo = new Sci.Production.Class.TxtAccountNo();
            this.txtuserReceived = new Sci.Production.Class.Txtuser();
            this.labelReceived = new Sci.Win.UI.Label();
            this.dateReceived = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(472, 302);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(386, 302);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Description";
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(87, 9);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(459, 155);
            this.editDescription.TabIndex = 0;
            // 
            // labelAccountNo
            // 
            this.labelAccountNo.Lines = 0;
            this.labelAccountNo.Location = new System.Drawing.Point(9, 242);
            this.labelAccountNo.Name = "labelAccountNo";
            this.labelAccountNo.Size = new System.Drawing.Size(75, 23);
            this.labelAccountNo.TabIndex = 90;
            this.labelAccountNo.Text = "Account No";
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "AccountID", true));
            this.txtAccountNo.DisplayBox1Binding = "";
            this.txtAccountNo.Location = new System.Drawing.Point(87, 242);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(300, 23);
            this.txtAccountNo.TabIndex = 3;
            this.txtAccountNo.TextBox1Binding = "";
            // 
            // txtuserReceived
            // 
            this.txtuserReceived.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "receiveName", true));
            this.txtuserReceived.DisplayBox1Binding = "";
            this.txtuserReceived.Location = new System.Drawing.Point(87, 178);
            this.txtuserReceived.Name = "txtuserReceived";
            this.txtuserReceived.Size = new System.Drawing.Size(300, 23);
            this.txtuserReceived.TabIndex = 1;
            this.txtuserReceived.TextBox1Binding = "";
            // 
            // labelReceived
            // 
            this.labelReceived.Lines = 0;
            this.labelReceived.Location = new System.Drawing.Point(9, 178);
            this.labelReceived.Name = "labelReceived";
            this.labelReceived.Size = new System.Drawing.Size(75, 23);
            this.labelReceived.TabIndex = 91;
            this.labelReceived.Text = "Received";
            // 
            // dateReceived
            // 
            this.dateReceived.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "receiveDate", true));
            this.dateReceived.Location = new System.Drawing.Point(87, 207);
            this.dateReceived.Name = "dateReceived";
            this.dateReceived.Size = new System.Drawing.Size(130, 23);
            this.dateReceived.TabIndex = 2;
            // 
            // P36_ModifyAfterReceived
            // 
            this.ClientSize = new System.Drawing.Size(565, 344);
            this.Controls.Add(this.dateReceived);
            this.Controls.Add(this.txtuserReceived);
            this.Controls.Add(this.labelReceived);
            this.Controls.Add(this.labelAccountNo);
            this.Controls.Add(this.txtAccountNo);
            this.Controls.Add(this.editDescription);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.DefaultControl = "editDescription";
            this.Name = "P36_ModifyAfterReceived";
            this.Text = "P36. Modify After Received";
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Label label1;
        private Win.UI.EditBox editDescription;
        private Win.UI.Label labelAccountNo;
        private Class.TxtAccountNo txtAccountNo;
        private Class.Txtuser txtuserReceived;
        private Win.UI.Label labelReceived;
        private Win.UI.DateBox dateReceived;
        private Win.UI.ListControlBindingSource mtbs;
    }
}
