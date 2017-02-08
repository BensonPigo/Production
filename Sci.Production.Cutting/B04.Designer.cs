namespace Sci.Production.Cutting
{
    partial class B04
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
            this.checkBoxJunk = new Sci.Win.UI.CheckBox();
            this.editBoxRemark = new Sci.Win.UI.EditBox();
            this.label3 = new Sci.Win.UI.Label();
            this.txt_desc = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txt_ID = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(676, 228);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkBoxJunk);
            this.detailcont.Controls.Add(this.editBoxRemark);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.txt_desc);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.txt_ID);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(676, 190);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 190);
            this.detailbtm.Size = new System.Drawing.Size(676, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(676, 380);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(684, 257);
            // 
            // checkBoxJunk
            // 
            this.checkBoxJunk.AutoSize = true;
            this.checkBoxJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkBoxJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxJunk.Location = new System.Drawing.Point(233, 18);
            this.checkBoxJunk.Name = "checkBoxJunk";
            this.checkBoxJunk.Size = new System.Drawing.Size(57, 21);
            this.checkBoxJunk.TabIndex = 14;
            this.checkBoxJunk.Text = "Junk";
            this.checkBoxJunk.UseVisualStyleBackColor = true;
            // 
            // editBoxRemark
            // 
            this.editBoxRemark.BackColor = System.Drawing.Color.White;
            this.editBoxRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editBoxRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxRemark.Location = new System.Drawing.Point(116, 114);
            this.editBoxRemark.Multiline = true;
            this.editBoxRemark.Name = "editBoxRemark";
            this.editBoxRemark.Size = new System.Drawing.Size(473, 70);
            this.editBoxRemark.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(21, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "Remark";
            // 
            // txt_desc
            // 
            this.txt_desc.BackColor = System.Drawing.Color.White;
            this.txt_desc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txt_desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_desc.Location = new System.Drawing.Point(116, 66);
            this.txt_desc.Name = "txt_desc";
            this.txt_desc.Size = new System.Drawing.Size(379, 23);
            this.txt_desc.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(21, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Description";
            // 
            // txt_ID
            // 
            this.txt_ID.BackColor = System.Drawing.Color.White;
            this.txt_ID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txt_ID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_ID.Location = new System.Drawing.Point(116, 18);
            this.txt_ID.Name = "txt_ID";
            this.txt_ID.Size = new System.Drawing.Size(100, 23);
            this.txt_ID.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(21, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "ID";
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(684, 290);
            this.DefaultFilter = "Type=\'RC\'";
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B04";
            this.Text = "B04.CutReason";
            this.WorkAlias = "CutReason";
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

        private Win.UI.CheckBox checkBoxJunk;
        private Win.UI.EditBox editBoxRemark;
        private Win.UI.Label label3;
        private Win.UI.TextBox txt_desc;
        private Win.UI.Label label2;
        private Win.UI.TextBox txt_ID;
        private Win.UI.Label label1;
    }
}
