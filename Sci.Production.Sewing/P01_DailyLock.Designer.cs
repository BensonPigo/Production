namespace Sci.Production.Sewing
{
    partial class P01_DailyLock
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
            this.label1 = new Sci.Win.UI.Label();
            this.dateLock = new Sci.Win.UI.DateBox();
            this.btnLock = new Sci.Win.UI.Button();
            this.BtnUndo = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(22, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Lock  Date";
            // 
            // dateLock
            // 
            this.dateLock.Location = new System.Drawing.Point(100, 9);
            this.dateLock.Name = "dateLock";
            this.dateLock.Size = new System.Drawing.Size(130, 23);
            this.dateLock.TabIndex = 2;
            this.dateLock.Validating += new System.ComponentModel.CancelEventHandler(this.DateLock_Validating);
            // 
            // btnLock
            // 
            this.btnLock.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLock.Location = new System.Drawing.Point(100, 38);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(80, 30);
            this.btnLock.TabIndex = 3;
            this.btnLock.Text = "Lock";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.BtnLock_Click);
            // 
            // BtnUndo
            // 
            this.BtnUndo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnUndo.Location = new System.Drawing.Point(186, 38);
            this.BtnUndo.Name = "BtnUndo";
            this.BtnUndo.Size = new System.Drawing.Size(80, 30);
            this.BtnUndo.TabIndex = 4;
            this.BtnUndo.Text = "Undo";
            this.BtnUndo.UseVisualStyleBackColor = true;
            this.BtnUndo.Click += new System.EventHandler(this.BtnUndo_Click);
            // 
            // P01_DailyLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 78);
            this.Controls.Add(this.BtnUndo);
            this.Controls.Add(this.btnLock);
            this.Controls.Add(this.dateLock);
            this.Controls.Add(this.label1);
            this.EditMode = true;
            this.Name = "P01_DailyLock";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P01.Daily Lock";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateLock, 0);
            this.Controls.SetChildIndex(this.btnLock, 0);
            this.Controls.SetChildIndex(this.BtnUndo, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.DateBox dateLock;
        private Win.UI.Button btnLock;
        private Win.UI.Button BtnUndo;
    }
}