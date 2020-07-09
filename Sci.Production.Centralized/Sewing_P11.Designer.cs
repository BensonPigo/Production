namespace Sci.Production.Centralized
{
    partial class Sewing_P11
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
            this.rdbtnLock = new System.Windows.Forms.RadioButton();
            this.rdbtnUnlock = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbM = new Sci.Win.UI.Label();
            this.lbFty = new Sci.Win.UI.Label();
            this.lbDate = new Sci.Win.UI.Label();
            this.dateLock = new Sci.Win.UI.DateBox();
            this.btnlock = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.txtCentralizedmulitM1 = new Sci.Production.Class.TxtCentralizedmulitM();
            this.txtCentralizedmulitFactory1 = new Sci.Production.Class.TxtCentralizedmulitFactory();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdbtnLock
            // 
            this.rdbtnLock.AutoSize = true;
            this.rdbtnLock.Location = new System.Drawing.Point(3, 3);
            this.rdbtnLock.Name = "rdbtnLock";
            this.rdbtnLock.Size = new System.Drawing.Size(56, 21);
            this.rdbtnLock.TabIndex = 0;
            this.rdbtnLock.Text = "Lock";
            this.rdbtnLock.UseVisualStyleBackColor = true;
            this.rdbtnLock.CheckedChanged += new System.EventHandler(this.RdbtnLock_CheckedChanged);
            // 
            // rdbtnUnlock
            // 
            this.rdbtnUnlock.AutoSize = true;
            this.rdbtnUnlock.Checked = true;
            this.rdbtnUnlock.Location = new System.Drawing.Point(116, 3);
            this.rdbtnUnlock.Name = "rdbtnUnlock";
            this.rdbtnUnlock.Size = new System.Drawing.Size(69, 21);
            this.rdbtnUnlock.TabIndex = 1;
            this.rdbtnUnlock.TabStop = true;
            this.rdbtnUnlock.Text = "Unlock";
            this.rdbtnUnlock.UseVisualStyleBackColor = true;
            this.rdbtnUnlock.CheckedChanged += new System.EventHandler(this.RdbtnLock_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbtnUnlock);
            this.panel1.Controls.Add(this.rdbtnLock);
            this.panel1.Location = new System.Drawing.Point(27, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 28);
            this.panel1.TabIndex = 2;
            // 
            // lbM
            // 
            this.lbM.Location = new System.Drawing.Point(30, 43);
            this.lbM.Name = "lbM";
            this.lbM.Size = new System.Drawing.Size(89, 23);
            this.lbM.TabIndex = 3;
            this.lbM.Text = "M";
            // 
            // lbFty
            // 
            this.lbFty.Location = new System.Drawing.Point(30, 72);
            this.lbFty.Name = "lbFty";
            this.lbFty.Size = new System.Drawing.Size(89, 23);
            this.lbFty.TabIndex = 4;
            this.lbFty.Text = "Factory";
            // 
            // lbDate
            // 
            this.lbDate.Location = new System.Drawing.Point(30, 101);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(89, 23);
            this.lbDate.TabIndex = 5;
            this.lbDate.Text = "Unlock Date";
            // 
            // dateLock
            // 
            this.dateLock.IsSupportEditMode = false;
            this.dateLock.Location = new System.Drawing.Point(122, 101);
            this.dateLock.Name = "dateLock";
            this.dateLock.Size = new System.Drawing.Size(130, 23);
            this.dateLock.TabIndex = 8;
            // 
            // btnlock
            // 
            this.btnlock.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnlock.Location = new System.Drawing.Point(184, 134);
            this.btnlock.Name = "btnlock";
            this.btnlock.Size = new System.Drawing.Size(121, 30);
            this.btnlock.TabIndex = 9;
            this.btnlock.Text = "Lock/Unlock";
            this.btnlock.UseVisualStyleBackColor = true;
            this.btnlock.Click += new System.EventHandler(this.Btnlock_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(311, 134);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // txtCentralizedmulitM1
            // 
            this.txtCentralizedmulitM1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitM1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitM1.IsSupportEditMode = false;
            this.txtCentralizedmulitM1.Location = new System.Drawing.Point(122, 43);
            this.txtCentralizedmulitM1.Name = "txtCentralizedmulitM1";
            this.txtCentralizedmulitM1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtCentralizedmulitM1.ReadOnly = true;
            this.txtCentralizedmulitM1.Size = new System.Drawing.Size(433, 23);
            this.txtCentralizedmulitM1.TabIndex = 13;
            this.txtCentralizedmulitM1.TextChanged += new System.EventHandler(this.TxtCentralizedmulitM1_TextChanged);
            // 
            // txtCentralizedmulitFactory1
            // 
            this.txtCentralizedmulitFactory1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitFactory1.IsSupportEditMode = false;
            this.txtCentralizedmulitFactory1.Location = new System.Drawing.Point(122, 72);
            this.txtCentralizedmulitFactory1.MObjectName = this.txtCentralizedmulitM1;
            this.txtCentralizedmulitFactory1.Name = "txtCentralizedmulitFactory1";
            this.txtCentralizedmulitFactory1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtCentralizedmulitFactory1.ReadOnly = true;
            this.txtCentralizedmulitFactory1.Size = new System.Drawing.Size(571, 23);
            this.txtCentralizedmulitFactory1.TabIndex = 12;
            // 
            // Sewing_P11
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 176);
            this.Controls.Add(this.txtCentralizedmulitM1);
            this.Controls.Add(this.txtCentralizedmulitFactory1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnlock);
            this.Controls.Add(this.dateLock);
            this.Controls.Add(this.lbDate);
            this.Controls.Add(this.lbFty);
            this.Controls.Add(this.lbM);
            this.Controls.Add(this.panel1);
            this.Name = "Sewing_P11";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Sewing_P11 Sewing Monthly Lock or Unlock";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.lbM, 0);
            this.Controls.SetChildIndex(this.lbFty, 0);
            this.Controls.SetChildIndex(this.lbDate, 0);
            this.Controls.SetChildIndex(this.dateLock, 0);
            this.Controls.SetChildIndex(this.btnlock, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.txtCentralizedmulitFactory1, 0);
            this.Controls.SetChildIndex(this.txtCentralizedmulitM1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbtnLock;
        private System.Windows.Forms.RadioButton rdbtnUnlock;
        private System.Windows.Forms.Panel panel1;
        private Win.UI.Label lbM;
        private Win.UI.Label lbFty;
        private Win.UI.Label lbDate;
        private Win.UI.DateBox dateLock;
        private Win.UI.Button btnlock;
        private Win.UI.Button btnClose;
        private Class.TxtCentralizedmulitFactory txtCentralizedmulitFactory1;
        private Class.TxtCentralizedmulitM txtCentralizedmulitM1;
    }
}