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
            this.labelID = new Sci.Win.UI.Label();
            this.labelSubprocess = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.comboSubprocess = new Sci.Win.UI.ComboBox();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSewingLine = new Sci.Win.UI.TextBox();
            this.contextMenuStrip1 = new Sci.Win.UI.ContextMenuStrip();
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
            this.detail.Size = new System.Drawing.Size(844, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtSewingLine);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.comboStockType);
            this.detailcont.Controls.Add(this.comboSubprocess);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labelStockType);
            this.detailcont.Controls.Add(this.labelSubprocess);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(844, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(844, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(844, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(852, 424);
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
            this.labelID.Location = new System.Drawing.Point(35, 37);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(97, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelSubprocess
            // 
            this.labelSubprocess.Location = new System.Drawing.Point(35, 94);
            this.labelSubprocess.Name = "labelSubprocess";
            this.labelSubprocess.Size = new System.Drawing.Size(97, 23);
            this.labelSubprocess.TabIndex = 1;
            this.labelSubprocess.Text = "Sub-process";
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(35, 151);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(97, 23);
            this.labelStockType.TabIndex = 2;
            this.labelStockType.Text = "Stock Type";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(135, 37);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(201, 23);
            this.txtID.TabIndex = 0;
            // 
            // comboSubprocess
            // 
            this.comboSubprocess.BackColor = System.Drawing.Color.White;
            this.comboSubprocess.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ProcessId", true));
            this.comboSubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubprocess.FormattingEnabled = true;
            this.comboSubprocess.IsSupportUnselect = true;
            this.comboSubprocess.Location = new System.Drawing.Point(135, 94);
            this.comboSubprocess.Name = "comboSubprocess";
            this.comboSubprocess.OldText = "";
            this.comboSubprocess.Size = new System.Drawing.Size(121, 24);
            this.comboSubprocess.TabIndex = 1;
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Location = new System.Drawing.Point(135, 150);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(35, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sewing Line ID";
            // 
            // txtSewingLine
            // 
            this.txtSewingLine.BackColor = System.Drawing.Color.White;
            this.txtSewingLine.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingLineID", true));
            this.txtSewingLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLine.Location = new System.Drawing.Point(135, 199);
            this.txtSewingLine.Name = "txtSewingLine";
            this.txtSewingLine.Size = new System.Drawing.Size(67, 23);
            this.txtSewingLine.TabIndex = 4;
            this.txtSewingLine.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSewingLine_PopUp);
            this.txtSewingLine.Validating += new System.ComponentModel.CancelEventHandler(this.txtSewingLine_Validating);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // B40
            // 
            this.ClientSize = new System.Drawing.Size(852, 457);
            this.DefaultControl = "txtID";
            this.DefaultControlForEdit = "comboSubprocess";
            this.IsSupportPrint = false;
            this.Name = "B40";
            this.Text = "B40.RFID Reader setting";
            this.UniqueExpress = "ID";
            this.WorkAlias = "RFIDReader";
            this.Controls.SetChildIndex(this.tabs, 0);
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

        private Win.UI.Label labelStockType;
        private Win.UI.Label labelSubprocess;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtID;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.ComboBox comboSubprocess;
        private Win.UI.TextBox txtSewingLine;
        private Win.UI.Label label1;
        private Win.UI.ContextMenuStrip contextMenuStrip1;
    }
}
