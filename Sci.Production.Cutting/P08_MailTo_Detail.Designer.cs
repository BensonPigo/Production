namespace Sci.Production.Cutting
{
    partial class P08_MailTo_Detail
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelMailTo = new Sci.Win.UI.Label();
            this.labelCC = new Sci.Win.UI.Label();
            this.labelSubject = new Sci.Win.UI.Label();
            this.labelContents = new Sci.Win.UI.Label();
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
            // labelCode
            // 
            this.labelCode.Lines = 0;
            this.labelCode.Location = new System.Drawing.Point(13, 13);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(75, 23);
            this.labelCode.TabIndex = 95;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(13, 41);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 96;
            this.labelDescription.Text = "Description";
            // 
            // labelMailTo
            // 
            this.labelMailTo.Lines = 0;
            this.labelMailTo.Location = new System.Drawing.Point(13, 69);
            this.labelMailTo.Name = "labelMailTo";
            this.labelMailTo.Size = new System.Drawing.Size(75, 23);
            this.labelMailTo.TabIndex = 97;
            this.labelMailTo.Text = "Mail to";
            // 
            // labelCC
            // 
            this.labelCC.Lines = 0;
            this.labelCC.Location = new System.Drawing.Point(13, 124);
            this.labelCC.Name = "labelCC";
            this.labelCC.Size = new System.Drawing.Size(75, 23);
            this.labelCC.TabIndex = 98;
            this.labelCC.Text = "C.C.";
            // 
            // labelSubject
            // 
            this.labelSubject.Lines = 0;
            this.labelSubject.Location = new System.Drawing.Point(13, 179);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(75, 23);
            this.labelSubject.TabIndex = 99;
            this.labelSubject.Text = "Subject";
            // 
            // labelContents
            // 
            this.labelContents.Lines = 0;
            this.labelContents.Location = new System.Drawing.Point(13, 207);
            this.labelContents.Name = "labelContents";
            this.labelContents.Size = new System.Drawing.Size(75, 23);
            this.labelContents.TabIndex = 100;
            this.labelContents.Text = "Contents";
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
            // P08_MailTo_Detail
            // 
            this.ClientSize = new System.Drawing.Size(713, 471);
            this.Controls.Add(this.editContents);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.editCC);
            this.Controls.Add(this.editMailTo);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.labelContents);
            this.Controls.Add(this.labelSubject);
            this.Controls.Add(this.labelCC);
            this.Controls.Add(this.labelMailTo);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelCode);
            this.Name = "P08_MailTo_Detail";
            this.Text = "Mail to detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelCode, 0);
            this.Controls.SetChildIndex(this.labelDescription, 0);
            this.Controls.SetChildIndex(this.labelMailTo, 0);
            this.Controls.SetChildIndex(this.labelCC, 0);
            this.Controls.SetChildIndex(this.labelSubject, 0);
            this.Controls.SetChildIndex(this.labelContents, 0);
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

        private Win.UI.Label labelCode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelMailTo;
        private Win.UI.Label labelCC;
        private Win.UI.Label labelSubject;
        private Win.UI.Label labelContents;
        private Win.UI.TextBox txtCode;
        private Win.UI.TextBox txtDescription;
        private Win.UI.EditBox editMailTo;
        private Win.UI.EditBox editCC;
        private Win.UI.TextBox txtSubject;
        private Win.UI.EditBox editContents;
    }
}
