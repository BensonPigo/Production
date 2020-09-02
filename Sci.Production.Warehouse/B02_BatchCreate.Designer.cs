namespace Sci.Production.Warehouse
{
    partial class B02_BatchCreate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.txtID = new Sci.Win.UI.TextBox();
            this.labelCode = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.btnCreate = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.chkBulk = new Sci.Win.UI.CheckBox();
            this.chkInventory = new Sci.Win.UI.CheckBox();
            this.chkScrap = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(100, 63);
            this.txtDescription.MaxLength = 40;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(478, 23);
            this.txtDescription.TabIndex = 2;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(100, 21);
            this.txtID.MaxLength = 10;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(78, 23);
            this.txtID.TabIndex = 1;
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(22, 21);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(75, 23);
            this.labelCode.TabIndex = 10;
            this.labelCode.Text = "Code";
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(22, 106);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(75, 23);
            this.labelStockType.TabIndex = 11;
            this.labelStockType.Text = "Stock Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(22, 63);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 12;
            this.labelDescription.Text = "Description";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(397, 146);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(80, 30);
            this.btnCreate.TabIndex = 6;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(498, 146);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // chkBulk
            // 
            this.chkBulk.AutoSize = true;
            this.chkBulk.Checked = true;
            this.chkBulk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulk.Location = new System.Drawing.Point(100, 106);
            this.chkBulk.Name = "chkBulk";
            this.chkBulk.Size = new System.Drawing.Size(54, 21);
            this.chkBulk.TabIndex = 3;
            this.chkBulk.Text = "Bulk";
            this.chkBulk.UseVisualStyleBackColor = true;
            // 
            // chkInventory
            // 
            this.chkInventory.AutoSize = true;
            this.chkInventory.Checked = true;
            this.chkInventory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInventory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkInventory.Location = new System.Drawing.Point(160, 106);
            this.chkInventory.Name = "chkInventory";
            this.chkInventory.Size = new System.Drawing.Size(85, 21);
            this.chkInventory.TabIndex = 4;
            this.chkInventory.Text = "Inventory";
            this.chkInventory.UseVisualStyleBackColor = true;
            // 
            // chkScrap
            // 
            this.chkScrap.AutoSize = true;
            this.chkScrap.Checked = true;
            this.chkScrap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScrap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkScrap.Location = new System.Drawing.Point(251, 108);
            this.chkScrap.Name = "chkScrap";
            this.chkScrap.Size = new System.Drawing.Size(64, 21);
            this.chkScrap.TabIndex = 5;
            this.chkScrap.Text = "Scrap";
            this.chkScrap.UseVisualStyleBackColor = true;
            // 
            // B02_BatchCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 197);
            this.Controls.Add(this.chkScrap);
            this.Controls.Add(this.chkInventory);
            this.Controls.Add(this.chkBulk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.labelCode);
            this.Controls.Add(this.labelStockType);
            this.Controls.Add(this.labelDescription);
            this.EditMode = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportMove = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.IsToolbarVisible = false;
            this.Name = "B02_BatchCreate";
            this.OnLineHelpID = "Sci.Win.Tems.Base";
            this.Text = "B02. Batch Create";
            this.Controls.SetChildIndex(this.labelDescription, 0);
            this.Controls.SetChildIndex(this.labelStockType, 0);
            this.Controls.SetChildIndex(this.labelCode, 0);
            this.Controls.SetChildIndex(this.txtID, 0);
            this.Controls.SetChildIndex(this.txtDescription, 0);
            this.Controls.SetChildIndex(this.btnCreate, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.chkBulk, 0);
            this.Controls.SetChildIndex(this.chkInventory, 0);
            this.Controls.SetChildIndex(this.chkScrap, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtID;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelStockType;
        private Win.UI.Label labelDescription;
        private Win.UI.Button btnCreate;
        private Win.UI.Button btnCancel;
        private Win.UI.CheckBox chkBulk;
        private Win.UI.CheckBox chkInventory;
        private Win.UI.CheckBox chkScrap;
    }
}