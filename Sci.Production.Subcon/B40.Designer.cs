namespace Sci.Production.Subcon
{
    partial class B40
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
            this.textID = new Sci.Win.UI.TextBox();
            this.comboSubprocess = new Sci.Win.UI.ComboBox();
            this.comboType = new Sci.Win.UI.ComboBox();
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
            this.detail.Size = new System.Drawing.Size(676, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.comboType);
            this.detailcont.Controls.Add(this.comboSubprocess);
            this.detailcont.Controls.Add(this.textID);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(676, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(676, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(676, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(684, 424);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(35, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(35, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sub-process";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(35, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Stock Type";
            // 
            // textID
            // 
            this.textID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.textID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textID.IsSupportEditMode = false;
            this.textID.Location = new System.Drawing.Point(124, 37);
            this.textID.Name = "textID";
            this.textID.ReadOnly = true;
            this.textID.Size = new System.Drawing.Size(201, 23);
            this.textID.TabIndex = 3;
            // 
            // comboSubprocess
            // 
            this.comboSubprocess.BackColor = System.Drawing.Color.White;
            this.comboSubprocess.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ProcessId", true));
            this.comboSubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubprocess.FormattingEnabled = true;
            this.comboSubprocess.IsSupportUnselect = true;
            this.comboSubprocess.Location = new System.Drawing.Point(124, 94);
            this.comboSubprocess.Name = "comboSubprocess";
            this.comboSubprocess.Size = new System.Drawing.Size(121, 24);
            this.comboSubprocess.TabIndex = 4;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(124, 150);
            this.comboType.Name = "comboType";
            this.comboType.Size = new System.Drawing.Size(121, 24);
            this.comboType.TabIndex = 5;
            // 
            // B40
            // 
            this.ClientSize = new System.Drawing.Size(684, 457);
            this.IsSupportPrint = false;
            this.Name = "B40";
            this.Text = "B40.RFID Reader setting";
            this.UniqueExpress = "ID";
            this.WorkAlias = "RFIDReader";
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
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox textID;
        private Win.UI.ComboBox comboType;
        private Win.UI.ComboBox comboSubprocess;
    }
}
