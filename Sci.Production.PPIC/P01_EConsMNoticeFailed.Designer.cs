namespace Sci.Production.PPIC
{
    partial class P01_EConsMNoticeFailed
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
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.rdbEachCons = new Sci.Win.UI.RadioButton();
            this.UI_rdbMNotice = new Sci.Win.UI.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 453);
            this.btmcont.Size = new System.Drawing.Size(908, 44);
            this.btmcont.TabIndex = 5;
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 48);
            this.gridcont.Size = new System.Drawing.Size(884, 399);
            // 
            // append
            // 
            this.append.Size = new System.Drawing.Size(80, 34);
            this.append.TabIndex = 2;
            this.append.Visible = false;
            // 
            // revise
            // 
            this.revise.Size = new System.Drawing.Size(80, 34);
            this.revise.TabIndex = 1;
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.Size = new System.Drawing.Size(80, 34);
            this.delete.TabIndex = 0;
            this.delete.Visible = false;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(818, 5);
            this.undo.Size = new System.Drawing.Size(80, 34);
            this.undo.TabIndex = 5;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(738, 5);
            this.save.Size = new System.Drawing.Size(80, 34);
            this.save.TabIndex = 4;
            this.save.Visible = false;
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.rdbEachCons);
            this.radioPanel1.Controls.Add(this.UI_rdbMNotice);
            this.radioPanel1.IsSupportEditMode = false;
            this.radioPanel1.Location = new System.Drawing.Point(12, 12);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(205, 30);
            this.radioPanel1.TabIndex = 137;
            // 
            // rdbEachCons
            // 
            this.rdbEachCons.AutoSize = true;
            this.rdbEachCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbEachCons.Location = new System.Drawing.Point(105, 5);
            this.rdbEachCons.Name = "rdbEachCons";
            this.rdbEachCons.Size = new System.Drawing.Size(94, 21);
            this.rdbEachCons.TabIndex = 103;
            this.rdbEachCons.Text = "Each Cons";
            this.rdbEachCons.UseVisualStyleBackColor = true;
            this.rdbEachCons.CheckedChanged += new System.EventHandler(this.Rdb_CheckedChanged);
            // 
            // UI_rdbMNotice
            // 
            this.UI_rdbMNotice.AutoSize = true;
            this.UI_rdbMNotice.Checked = true;
            this.UI_rdbMNotice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UI_rdbMNotice.Location = new System.Drawing.Point(5, 5);
            this.UI_rdbMNotice.Name = "UI_rdbMNotice";
            this.UI_rdbMNotice.Size = new System.Drawing.Size(81, 21);
            this.UI_rdbMNotice.TabIndex = 102;
            this.UI_rdbMNotice.TabStop = true;
            this.UI_rdbMNotice.Text = "M/Notice";
            this.UI_rdbMNotice.UseVisualStyleBackColor = true;
            this.UI_rdbMNotice.CheckedChanged += new System.EventHandler(this.Rdb_CheckedChanged);
            // 
            // P01_EConsMNoticeFailed
            // 
            this.ClientSize = new System.Drawing.Size(908, 497);
            this.Controls.Add(this.radioPanel1);
            this.Name = "P01_EConsMNoticeFailed";
            this.Text = "Each Cons/M.Notice Failed";
            this.WorkAlias = "ECons_MNoticeFailed";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.radioPanel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton rdbEachCons;
        private Win.UI.RadioButton UI_rdbMNotice;
    }
}
