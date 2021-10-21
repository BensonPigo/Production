namespace Sci.Production.Centralized
{
    partial class QA_B21
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
            this.labelDefectcode = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelDefectType = new Sci.Win.UI.Label();
            this.txtDefectcode = new Sci.Win.UI.TextBox();
            this.txtDefectType = new Sci.Win.UI.TextBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.chk_isCriticalDefect = new Sci.Win.UI.CheckBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.txtHangerFailCode = new Sci.Win.UI.TextBox();
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
            this.detail.Size = new System.Drawing.Size(863, 300);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtHangerFailCode);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.chk_isCriticalDefect);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.txtDefectType);
            this.detailcont.Controls.Add(this.txtDefectcode);
            this.detailcont.Controls.Add(this.labelDefectcode);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelDefectType);
            this.detailcont.Size = new System.Drawing.Size(863, 262);
            this.detailcont.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Detailcont_MouseDown);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 262);
            this.detailbtm.Size = new System.Drawing.Size(863, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(831, 441);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(871, 329);
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
            // labelDefectcode
            // 
            this.labelDefectcode.Location = new System.Drawing.Point(58, 66);
            this.labelDefectcode.Name = "labelDefectcode";
            this.labelDefectcode.Size = new System.Drawing.Size(133, 23);
            this.labelDefectcode.TabIndex = 2;
            this.labelDefectcode.Text = "Defect code";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(58, 132);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(133, 23);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "Description";
            // 
            // labelDefectType
            // 
            this.labelDefectType.Location = new System.Drawing.Point(58, 99);
            this.labelDefectType.Name = "labelDefectType";
            this.labelDefectType.Size = new System.Drawing.Size(133, 23);
            this.labelDefectType.TabIndex = 4;
            this.labelDefectType.Text = "Defect Type";
            // 
            // txtDefectcode
            // 
            this.txtDefectcode.BackColor = System.Drawing.Color.White;
            this.txtDefectcode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtDefectcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDefectcode.Location = new System.Drawing.Point(194, 66);
            this.txtDefectcode.Name = "txtDefectcode";
            this.txtDefectcode.Size = new System.Drawing.Size(135, 23);
            this.txtDefectcode.TabIndex = 0;
            this.txtDefectcode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtDefectcode_Validating);
            this.txtDefectcode.Validated += new System.EventHandler(this.TxtDefectcode_Validated);
            // 
            // txtDefectType
            // 
            this.txtDefectType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtDefectType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "GarmentDefectTypeID", true));
            this.txtDefectType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDefectType.IsSupportEditMode = false;
            this.txtDefectType.Location = new System.Drawing.Point(194, 99);
            this.txtDefectType.Name = "txtDefectType";
            this.txtDefectType.ReadOnly = true;
            this.txtDefectType.Size = new System.Drawing.Size(135, 23);
            this.txtDefectType.TabIndex = 1;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.CausesValidation = false;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(194, 132);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(460, 94);
            this.editDescription.TabIndex = 2;
            this.editDescription.Leave += new System.EventHandler(this.EditDescription_Leave);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(344, 68);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 10;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(58, 232);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "Hanger-Reject Code";
            // 
            // chk_isCriticalDefect
            // 
            this.chk_isCriticalDefect.AutoSize = true;
            this.chk_isCriticalDefect.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsCriticalDefect", true));
            this.chk_isCriticalDefect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_isCriticalDefect.Location = new System.Drawing.Point(344, 232);
            this.chk_isCriticalDefect.Name = "chk_isCriticalDefect";
            this.chk_isCriticalDefect.Size = new System.Drawing.Size(128, 21);
            this.chk_isCriticalDefect.TabIndex = 13;
            this.chk_isCriticalDefect.Text = "Is Critical Defect";
            this.chk_isCriticalDefect.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsCFA", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(478, 232);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(111, 21);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "For CFA Only";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtHangerFailCode
            // 
            this.txtHangerFailCode.BackColor = System.Drawing.Color.White;
            this.txtHangerFailCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ReworkTotalFailCode", true));
            this.txtHangerFailCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtHangerFailCode.Location = new System.Drawing.Point(194, 232);
            this.txtHangerFailCode.Name = "txtHangerFailCode";
            this.txtHangerFailCode.Size = new System.Drawing.Size(135, 23);
            this.txtHangerFailCode.TabIndex = 15;
            // 
            // QA_B21
            // 
            this.ClientSize = new System.Drawing.Size(871, 362);
            this.DefaultControl = "txtDefectcode";
            this.DefaultControlForEdit = "editDescription";
            this.DefaultOrder = "GarmentDefectTypeID,Seq";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.KeyPreview = true;
            this.Name = "QA_B21";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "QA_B21. Defect Detail for RFT/CFA(Garment)       ";
            this.WorkAlias = "GarmentDefectCode";
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

        private Win.UI.Label labelDefectcode;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelDefectType;
        private Win.UI.TextBox txtDefectType;
        private Win.UI.TextBox txtDefectcode;
        private Win.UI.EditBox editDescription;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chk_isCriticalDefect;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.TextBox txtHangerFailCode;
    }
}
