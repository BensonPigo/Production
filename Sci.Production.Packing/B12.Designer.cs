namespace Sci.Production.Packing
{
    partial class B12
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMachineID = new Sci.Win.UI.TextBox();
            this.labMachineID = new Sci.Win.UI.Label();
            this.labOperator = new Sci.Win.UI.Label();
            this.labDescription = new Sci.Win.UI.Label();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.txtOperator = new Sci.Production.Class.Txtuser();
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
            this.detail.Size = new System.Drawing.Size(845, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.labDescription);
            this.detailcont.Controls.Add(this.txtOperator);
            this.detailcont.Controls.Add(this.labOperator);
            this.detailcont.Controls.Add(this.labMachineID);
            this.detailcont.Controls.Add(this.txtMachineID);
            this.detailcont.Size = new System.Drawing.Size(845, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(845, 38);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(853, 424);
            // 
            // txtMachineID
            // 
            this.txtMachineID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMachineID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MachineID", true));
            this.txtMachineID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMachineID.IsSupportEditMode = false;
            this.txtMachineID.Location = new System.Drawing.Point(124, 33);
            this.txtMachineID.Name = "txtMachineID";
            this.txtMachineID.ReadOnly = true;
            this.txtMachineID.Size = new System.Drawing.Size(164, 23);
            this.txtMachineID.TabIndex = 0;
            // 
            // labMachineID
            // 
            this.labMachineID.Location = new System.Drawing.Point(36, 33);
            this.labMachineID.Name = "labMachineID";
            this.labMachineID.Size = new System.Drawing.Size(85, 23);
            this.labMachineID.TabIndex = 4;
            this.labMachineID.Text = "Machine ID";
            // 
            // labOperator
            // 
            this.labOperator.Location = new System.Drawing.Point(36, 68);
            this.labOperator.Name = "labOperator";
            this.labOperator.Size = new System.Drawing.Size(85, 23);
            this.labOperator.TabIndex = 5;
            this.labOperator.Text = "Operator";
            // 
            // labDescription
            // 
            this.labDescription.Location = new System.Drawing.Point(36, 106);
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new System.Drawing.Size(85, 23);
            this.labDescription.TabIndex = 6;
            this.labDescription.Text = "Description";
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(124, 106);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(300, 144);
            this.editDescription.TabIndex = 2;
            this.editDescription.TabStop = false;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(294, 35);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 3;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // txtOperator
            // 
            this.txtOperator.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Operator", true));
            this.txtOperator.DisplayBox1Binding = "";
            this.txtOperator.Location = new System.Drawing.Point(124, 68);
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.Size = new System.Drawing.Size(300, 23);
            this.txtOperator.TabIndex = 1;
            this.txtOperator.TextBox1Binding = "";
            // 
            // B12
            // 
            this.ClientSize = new System.Drawing.Size(853, 457);
            this.DefaultControl = "txtMachineID";
            this.DefaultControlForEdit = "txtOperator";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B12";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B12. MD Machine";
            this.UniqueExpress = "MachineID";
            this.WorkAlias = "MDMachineBasic";
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

        private Win.UI.TextBox txtMachineID;
        private Win.UI.Label labDescription;
        private Class.Txtuser txtOperator;
        private Win.UI.Label labOperator;
        private Win.UI.Label labMachineID;
        private Win.UI.EditBox editDescription;
        private Win.UI.CheckBox chkJunk;
    }
}
