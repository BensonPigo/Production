namespace Sci.Production.Basic
{
    partial class B02_MailTo_Detail
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.editMailTo = new Sci.Win.UI.EditBox();
            this.editCC = new Sci.Win.UI.EditBox();
            this.txtSubject = new Sci.Win.UI.TextBox();
            this.editContents = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 431);
            this.btmcont.Size = new System.Drawing.Size(713, 40);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(623, 5);
            this.undo.TabIndex = 3;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(543, 5);
            this.save.TabIndex = 2;
            // 
            // left
            // 
            this.left.TabIndex = 0;
            // 
            // right
            // 
            this.right.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "Code";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 96;
            this.label2.Text = "Description";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(13, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 97;
            this.label3.Text = "Mail to";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 98;
            this.label4.Text = "C.C.";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(13, 179);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "Subject";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(13, 207);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 100;
            this.label6.Text = "Contents";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(92, 13);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(40, 23);
            this.txtCode.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(92, 41);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(600, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // editMailTo
            // 
            this.editMailTo.BackColor = System.Drawing.Color.White;
            this.editMailTo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ToAddress", true));
            this.editMailTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editMailTo.Location = new System.Drawing.Point(92, 69);
            this.editMailTo.Multiline = true;
            this.editMailTo.Name = "editMailTo";
            this.editMailTo.Size = new System.Drawing.Size(600, 50);
            this.editMailTo.TabIndex = 2;
            // 
            // editCC
            // 
            this.editCC.BackColor = System.Drawing.Color.White;
            this.editCC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CcAddress", true));
            this.editCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editCC.Location = new System.Drawing.Point(92, 124);
            this.editCC.Multiline = true;
            this.editCC.Name = "editCC";
            this.editCC.Size = new System.Drawing.Size(600, 50);
            this.editCC.TabIndex = 3;
            // 
            // txtSubject
            // 
            this.txtSubject.BackColor = System.Drawing.Color.White;
            this.txtSubject.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Subject", true));
            this.txtSubject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubject.Location = new System.Drawing.Point(92, 179);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(600, 23);
            this.txtSubject.TabIndex = 4;
            // 
            // editContents
            // 
            this.editContents.BackColor = System.Drawing.Color.White;
            this.editContents.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Content", true));
            this.editContents.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editContents.Location = new System.Drawing.Point(92, 207);
            this.editContents.Multiline = true;
            this.editContents.Name = "editContents";
            this.editContents.Size = new System.Drawing.Size(600, 218);
            this.editContents.TabIndex = 5;
            // 
            // B02_MailTo_Detail
            // 
            this.ClientSize = new System.Drawing.Size(713, 471);
            this.Controls.Add(this.editContents);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.editCC);
            this.Controls.Add(this.editMailTo);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "B02_MailTo_Detail";
            this.Text = "Mail to detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtCode, 0);
            this.Controls.SetChildIndex(this.txtDescription, 0);
            this.Controls.SetChildIndex(this.editMailTo, 0);
            this.Controls.SetChildIndex(this.editCC, 0);
            this.Controls.SetChildIndex(this.txtSubject, 0);
            this.Controls.SetChildIndex(this.editContents, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtCode;
        private Win.UI.TextBox txtDescription;
        private Win.UI.EditBox editMailTo;
        private Win.UI.EditBox editCC;
        private Win.UI.TextBox txtSubject;
        private Win.UI.EditBox editContents;
    }
}
