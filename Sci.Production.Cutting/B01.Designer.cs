namespace Sci.Production.Cutting
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkDisplayonreport = new Sci.Win.UI.CheckBox();
            this.checkSelected = new Sci.Win.UI.CheckBox();
            this.checkProcess = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtShowSeq = new Sci.Win.UI.TextBox();
            this.txtBcsDate = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtartworktype_fty1 = new Sci.Production.Class.txtartworktype_fty();
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
            this.detail.Size = new System.Drawing.Size(675, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtartworktype_fty1);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.txtBcsDate);
            this.detailcont.Controls.Add(this.txtShowSeq);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.checkProcess);
            this.detailcont.Controls.Add(this.checkSelected);
            this.detailcont.Controls.Add(this.checkDisplayonreport);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(675, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(675, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(676, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(684, 424);
            // 
            // editby
            // 
            this.editby.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(70, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(70, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Show Seq";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(315, 59);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkDisplayonreport
            // 
            this.checkDisplayonreport.AutoSize = true;
            this.checkDisplayonreport.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "IsRFIDDefault", true));
            this.checkDisplayonreport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkDisplayonreport.Location = new System.Drawing.Point(412, 112);
            this.checkDisplayonreport.Name = "checkDisplayonreport";
            this.checkDisplayonreport.Size = new System.Drawing.Size(135, 21);
            this.checkDisplayonreport.TabIndex = 7;
            this.checkDisplayonreport.Text = "Display on report";
            this.checkDisplayonreport.UseVisualStyleBackColor = true;
            // 
            // checkSelected
            // 
            this.checkSelected.AutoSize = true;
            this.checkSelected.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "isSelection", true));
            this.checkSelected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSelected.Location = new System.Drawing.Point(315, 112);
            this.checkSelected.Name = "checkSelected";
            this.checkSelected.Size = new System.Drawing.Size(82, 21);
            this.checkSelected.TabIndex = 6;
            this.checkSelected.Text = "Selected";
            this.checkSelected.UseVisualStyleBackColor = true;
            // 
            // checkProcess
            // 
            this.checkProcess.AutoSize = true;
            this.checkProcess.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "IsRFIDProcess", true));
            this.checkProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkProcess.Location = new System.Drawing.Point(412, 57);
            this.checkProcess.Name = "checkProcess";
            this.checkProcess.Size = new System.Drawing.Size(78, 21);
            this.checkProcess.TabIndex = 5;
            this.checkProcess.Text = "Process";
            this.checkProcess.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(70, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Artwork Type";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(162, 55);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(90, 23);
            this.txtID.TabIndex = 0;
            // 
            // txtShowSeq
            // 
            this.txtShowSeq.BackColor = System.Drawing.Color.White;
            this.txtShowSeq.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "showseq", true));
            this.txtShowSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShowSeq.Location = new System.Drawing.Point(162, 167);
            this.txtShowSeq.Name = "txtShowSeq";
            this.txtShowSeq.Size = new System.Drawing.Size(53, 23);
            this.txtShowSeq.TabIndex = 2;
            // 
            // txtBcsDate
            // 
            this.txtBcsDate.BackColor = System.Drawing.Color.White;
            this.txtBcsDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "bcsdate", true));
            this.txtBcsDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBcsDate.Location = new System.Drawing.Point(162, 221);
            this.txtBcsDate.MaxLength = 2;
            this.txtBcsDate.Name = "txtBcsDate";
            this.txtBcsDate.Size = new System.Drawing.Size(66, 23);
            this.txtBcsDate.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(70, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 23);
            this.label5.TabIndex = 16;
            this.label5.Text = "BcsDate";
            // 
            // txtartworktype_fty1
            // 
            this.txtartworktype_fty1.BackColor = System.Drawing.Color.White;
            this.txtartworktype_fty1.cClassify = "";
            this.txtartworktype_fty1.cSubprocess = "t";
            this.txtartworktype_fty1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "artworktypeid", true));
            this.txtartworktype_fty1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_fty1.Location = new System.Drawing.Point(162, 112);
            this.txtartworktype_fty1.Name = "txtartworktype_fty1";
            this.txtartworktype_fty1.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_fty1.TabIndex = 1;
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(684, 457);
            this.DefaultControl = "txtID";
            this.DefaultControlForEdit = "txtartworktype_fty1";
            this.DefaultOrder = "id";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B01";
            this.Text = "B01.SubProcess Data";
            this.WorkAlias = "Subprocess";
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

        private Win.UI.Label label3;
        private Win.UI.CheckBox checkProcess;
        private Win.UI.CheckBox checkSelected;
        private Win.UI.CheckBox checkDisplayonreport;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtShowSeq;
        private Win.UI.TextBox txtID;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtBcsDate;
        private Class.txtartworktype_fty txtartworktype_fty1;
    }
}
