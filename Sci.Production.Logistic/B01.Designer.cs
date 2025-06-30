namespace Sci.Production.Logistic
{
    partial class B01
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
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtLine = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtHeight = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtPoint = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(829, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtPoint);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.txtHeight);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.txtLine);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(829, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(829, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(829, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(837, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(47, 41);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(75, 23);
            this.labelCode.TabIndex = 3;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(47, 80);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(126, 41);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(260, 23);
            this.txtCode.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(126, 80);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(260, 23);
            this.txtDescription.TabIndex = 1;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(423, 41);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtLine
            // 
            this.txtLine.BackColor = System.Drawing.Color.White;
            this.txtLine.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtLine.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Line", true));
            this.txtLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLine.Location = new System.Drawing.Point(126, 119);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(260, 23);
            this.txtLine.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(47, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Line";
            // 
            // txtHeight
            // 
            this.txtHeight.BackColor = System.Drawing.Color.White;
            this.txtHeight.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtHeight.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Height", true));
            this.txtHeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtHeight.Location = new System.Drawing.Point(126, 159);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(80, 23);
            this.txtHeight.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(47, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Height";
            // 
            // txtPoint
            // 
            this.txtPoint.BackColor = System.Drawing.Color.White;
            this.txtPoint.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtPoint.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Point", true));
            this.txtPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPoint.Location = new System.Drawing.Point(126, 201);
            this.txtPoint.Name = "txtPoint";
            this.txtPoint.Size = new System.Drawing.Size(80, 23);
            this.txtPoint.TabIndex = 9;
            this.txtPoint.TextChanged += new System.EventHandler(this.txtPoint_TextChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(47, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Point";
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "txtCode";
            this.DefaultOrder = "ID";
            this.Name = "B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B01. Clog Location Index";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ClogLocation";
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

        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelCode;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtPoint;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtHeight;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtLine;
        private Win.UI.Label label1;
    }
}
