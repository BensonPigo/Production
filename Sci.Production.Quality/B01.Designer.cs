namespace Sci.Production.Quality
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.labeltype = new Sci.Win.UI.Label();
            this.txttype = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.editBox1 = new Sci.Win.UI.EditBox();
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
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txttype);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labeltype);
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
            this.labelCode.Location = new System.Drawing.Point(73, 46);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(80, 23);
            this.labelCode.TabIndex = 4;
            this.labelCode.Text = "Code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(73, 110);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(80, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(156, 46);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 23);
            this.txtCode.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(280, 48);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescriptionEN", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(156, 110);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(292, 50);
            this.editDescription.TabIndex = 5;
            // 
            // labeltype
            // 
            this.labeltype.Location = new System.Drawing.Point(73, 78);
            this.labeltype.Name = "labeltype";
            this.labeltype.Size = new System.Drawing.Size(80, 23);
            this.labeltype.TabIndex = 6;
            this.labeltype.Text = "type";
            // 
            // txttype
            // 
            this.txttype.BackColor = System.Drawing.Color.White;
            this.txttype.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Type", true));
            this.txttype.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txttype.Location = new System.Drawing.Point(156, 78);
            this.txttype.Name = "txttype";
            this.txttype.Size = new System.Drawing.Size(100, 23);
            this.txttype.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(70, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Local Desc.";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LocalDesc", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(156, 166);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(292, 50);
            this.editBox1.TabIndex = 8;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(837, 457);
            this.DefaultControl = "txtCode";
            this.DefaultControlForEdit = "editDescription";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B01. Defect Code For Fabric Inspection";
            this.WorkAlias = "FabricDefect";
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

        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
        private Win.UI.TextBox txttype;
        private Win.UI.Label labeltype;
        private Win.UI.EditBox editBox1;
        private Win.UI.Label label1;
    }
}
