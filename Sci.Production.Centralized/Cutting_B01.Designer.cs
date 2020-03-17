namespace Sci.Production.Centralized
{
    partial class Cutting_B01
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelShowSeq = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkDisplayonreport = new Sci.Win.UI.CheckBox();
            this.checkSelected = new Sci.Win.UI.CheckBox();
            this.checkProcess = new Sci.Win.UI.CheckBox();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtShowSeq = new Sci.Win.UI.TextBox();
            this.txtBcsDate = new Sci.Win.UI.TextBox();
            this.labelBcsDate = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.combInOutRule = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.chkIsLackingAndReplacement = new Sci.Win.UI.CheckBox();
            this.txtFullName = new Sci.Win.UI.TextBox();
            this.txtartworktype_ftyArtworkType = new Sci.Production.Class.txtartworktype_fty();
            this.chkIsBoundedProcess = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(828, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkIsBoundedProcess);
            this.detailcont.Controls.Add(this.txtFullName);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.chkIsLackingAndReplacement);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.combInOutRule);
            this.detailcont.Controls.Add(this.txtartworktype_ftyArtworkType);
            this.detailcont.Controls.Add(this.labelBcsDate);
            this.detailcont.Controls.Add(this.txtBcsDate);
            this.detailcont.Controls.Add(this.txtShowSeq);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labelArtworkType);
            this.detailcont.Controls.Add(this.checkProcess);
            this.detailcont.Controls.Add(this.checkSelected);
            this.detailcont.Controls.Add(this.checkDisplayonreport);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelShowSeq);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(828, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(70, 57);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(89, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelShowSeq
            // 
            this.labelShowSeq.Location = new System.Drawing.Point(70, 167);
            this.labelShowSeq.Name = "labelShowSeq";
            this.labelShowSeq.Size = new System.Drawing.Size(89, 23);
            this.labelShowSeq.TabIndex = 1;
            this.labelShowSeq.Text = "Show Seq";
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
            // labelArtworkType
            // 
            this.labelArtworkType.Location = new System.Drawing.Point(70, 112);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(89, 23);
            this.labelArtworkType.TabIndex = 8;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.Location = new System.Drawing.Point(162, 57);
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
            // labelBcsDate
            // 
            this.labelBcsDate.Location = new System.Drawing.Point(70, 221);
            this.labelBcsDate.Name = "labelBcsDate";
            this.labelBcsDate.Size = new System.Drawing.Size(89, 23);
            this.labelBcsDate.TabIndex = 16;
            this.labelBcsDate.Text = "BcsDate";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(70, 276);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 20;
            this.label1.Text = "In Out Rule";
            // 
            // combInOutRule
            // 
            this.combInOutRule.BackColor = System.Drawing.Color.White;
            this.combInOutRule.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "InOutRule", true));
            this.combInOutRule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.combInOutRule.FormattingEnabled = true;
            this.combInOutRule.IsSupportUnselect = true;
            this.combInOutRule.Location = new System.Drawing.Point(162, 276);
            this.combInOutRule.Name = "combInOutRule";
            this.combInOutRule.OldText = "";
            this.combInOutRule.Size = new System.Drawing.Size(121, 24);
            this.combInOutRule.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(315, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 24;
            this.label2.Text = "Full Name";
            // 
            // chkIsLackingAndReplacement
            // 
            this.chkIsLackingAndReplacement.AutoSize = true;
            this.chkIsLackingAndReplacement.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "IsLackingAndReplacement", true));
            this.chkIsLackingAndReplacement.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsLackingAndReplacement.Location = new System.Drawing.Point(315, 167);
            this.chkIsLackingAndReplacement.Name = "chkIsLackingAndReplacement";
            this.chkIsLackingAndReplacement.Size = new System.Drawing.Size(191, 21);
            this.chkIsLackingAndReplacement.TabIndex = 23;
            this.chkIsLackingAndReplacement.Text = "Lacking and Replacement";
            this.chkIsLackingAndReplacement.UseVisualStyleBackColor = true;
            // 
            // txtFullName
            // 
            this.txtFullName.BackColor = System.Drawing.Color.White;
            this.txtFullName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FullName", true));
            this.txtFullName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFullName.Location = new System.Drawing.Point(407, 277);
            this.txtFullName.MaxLength = 15;
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(156, 23);
            this.txtFullName.TabIndex = 25;
            // 
            // txtartworktype_ftyArtworkType
            // 
            this.txtartworktype_ftyArtworkType.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyArtworkType.cClassify = "";
            this.txtartworktype_ftyArtworkType.cSubprocess = "t";
            this.txtartworktype_ftyArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "artworktypeid", true));
            this.txtartworktype_ftyArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyArtworkType.Location = new System.Drawing.Point(162, 112);
            this.txtartworktype_ftyArtworkType.Name = "txtartworktype_ftyArtworkType";
            this.txtartworktype_ftyArtworkType.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_ftyArtworkType.TabIndex = 1;
            // 
            // chkIsBoundedProcess
            // 
            this.chkIsBoundedProcess.AutoSize = true;
            this.chkIsBoundedProcess.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "IsBoundedProcess", true));
            this.chkIsBoundedProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsBoundedProcess.Location = new System.Drawing.Point(315, 221);
            this.chkIsBoundedProcess.Name = "chkIsBoundedProcess";
            this.chkIsBoundedProcess.Size = new System.Drawing.Size(153, 21);
            this.chkIsBoundedProcess.TabIndex = 26;
            this.chkIsBoundedProcess.Text = "Is Bounded Process";
            this.chkIsBoundedProcess.UseVisualStyleBackColor = true;
            // 
            // Cutting_B01
            // 
            this.ClientSize = new System.Drawing.Size(836, 457);
            this.ConnectionName = "ProductionTPE";
            this.DefaultControl = "txtID";
            this.DefaultControlForEdit = "txtartworktype_ftyArtworkType";
            this.DefaultOrder = "id";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "Cutting_B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Cutting_B01.SubProcess Data";
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

        private Win.UI.Label labelArtworkType;
        private Win.UI.CheckBox checkProcess;
        private Win.UI.CheckBox checkSelected;
        private Win.UI.CheckBox checkDisplayonreport;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelShowSeq;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtShowSeq;
        private Win.UI.TextBox txtID;
        private Win.UI.Label labelBcsDate;
        private Win.UI.TextBox txtBcsDate;
        private Class.txtartworktype_fty txtartworktype_ftyArtworkType;
        private Win.UI.Label label1;
        private Win.UI.ComboBox combInOutRule;
        private Win.UI.Label label2;
        private Win.UI.CheckBox chkIsLackingAndReplacement;
        private Win.UI.TextBox txtFullName;
        private Win.UI.CheckBox chkIsBoundedProcess;
    }
}
