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
            this.components = new System.ComponentModel.Container();
            this.labelID = new Sci.Win.UI.Label();
            this.labelSubprocess = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSewingLine = new Sci.Win.UI.TextBox();
            this.contextMenuStrip1 = new Sci.Win.UI.ContextMenuStrip();
            this.label2 = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labLocation = new Sci.Win.UI.Label();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new Sci.Win.UI.Label();
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.label4 = new Sci.Win.UI.Label();
            this.comboRFIDProcessLocation = new Sci.Production.Class.ComboRFIDProcessLocation();
            this.btnSetPanelCutcell = new Sci.Win.UI.Button();
            this.txtSubprocess = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(844, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtSubprocess);
            this.detailcont.Controls.Add(this.btnSetPanelCutcell);
            this.detailcont.Controls.Add(this.comboRFIDProcessLocation);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.comboMDivision);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.groupBox1);
            this.detailcont.Controls.Add(this.txtSewingLine);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.comboStockType);
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
            this.labelID.Location = new System.Drawing.Point(21, 37);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(111, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelSubprocess
            // 
            this.labelSubprocess.Location = new System.Drawing.Point(21, 71);
            this.labelSubprocess.Name = "labelSubprocess";
            this.labelSubprocess.Size = new System.Drawing.Size(111, 23);
            this.labelSubprocess.TabIndex = 1;
            this.labelSubprocess.Text = "Sub-process";
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(21, 110);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(111, 23);
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
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Location = new System.Drawing.Point(135, 109);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sewing Line ID";
            // 
            // txtSewingLine
            // 
            this.txtSewingLine.BackColor = System.Drawing.Color.White;
            this.txtSewingLine.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingLineID", true));
            this.txtSewingLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSewingLine.Location = new System.Drawing.Point(135, 148);
            this.txtSewingLine.Name = "txtSewingLine";
            this.txtSewingLine.Size = new System.Drawing.Size(67, 23);
            this.txtSewingLine.TabIndex = 3;
            this.txtSewingLine.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSewingLine_PopUp);
            this.txtSewingLine.Validating += new System.ComponentModel.CancelEventHandler(this.txtSewingLine_Validating);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(118, 34);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // labLocation
            // 
            this.labLocation.Location = new System.Drawing.Point(18, 73);
            this.labLocation.Name = "labLocation";
            this.labLocation.Size = new System.Drawing.Size(97, 23);
            this.labLocation.TabIndex = 7;
            this.labLocation.Text = "Location";
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Location", true));
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(118, 73);
            this.txtLocation.MaxLength = 50;
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(201, 23);
            this.txtLocation.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtLocation);
            this.groupBox1.Controls.Add(this.txtfactory);
            this.groupBox1.Controls.Add(this.labLocation);
            this.groupBox1.Location = new System.Drawing.Point(391, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 113);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Physical Location Information";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(21, 186);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "M";
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "MDivisionID", true));
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(136, 186);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.OldText = "";
            this.comboMDivision.Size = new System.Drawing.Size(120, 24);
            this.comboMDivision.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(21, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "Process Location";
            // 
            // comboRFIDProcessLocation
            // 
            this.comboRFIDProcessLocation.BackColor = System.Drawing.Color.White;
            this.comboRFIDProcessLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "RFIDProcessLocationID", true));
            this.comboRFIDProcessLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRFIDProcessLocation.FormattingEnabled = true;
            this.comboRFIDProcessLocation.IncludeJunk = false;
            this.comboRFIDProcessLocation.IsSupportUnselect = true;
            this.comboRFIDProcessLocation.Location = new System.Drawing.Point(135, 225);
            this.comboRFIDProcessLocation.Name = "comboRFIDProcessLocation";
            this.comboRFIDProcessLocation.OldText = "";
            this.comboRFIDProcessLocation.Size = new System.Drawing.Size(121, 24);
            this.comboRFIDProcessLocation.TabIndex = 12;
            // 
            // btnSetPanelCutcell
            // 
            this.btnSetPanelCutcell.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSetPanelCutcell.Location = new System.Drawing.Point(749, 30);
            this.btnSetPanelCutcell.Name = "btnSetPanelCutcell";
            this.btnSetPanelCutcell.Size = new System.Drawing.Size(72, 55);
            this.btnSetPanelCutcell.TabIndex = 13;
            this.btnSetPanelCutcell.Text = "Panel Cut Cell";
            this.btnSetPanelCutcell.UseVisualStyleBackColor = true;
            this.btnSetPanelCutcell.Click += new System.EventHandler(this.btnSetPanelCutcell_Click);
            // 
            // txtSubprocess
            // 
            this.txtSubprocess.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSubprocess.IsSupportEditMode = false;
            this.txtSubprocess.Location = new System.Drawing.Point(135, 71);
            this.txtSubprocess.Name = "txtSubprocess";
            this.txtSubprocess.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtSubprocess.ReadOnly = true;
            this.txtSubprocess.Size = new System.Drawing.Size(250, 23);
            this.txtSubprocess.TabIndex = 14;
            this.txtSubprocess.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSubprocess_PopUp);
            // 
            // B40
            // 
            this.ClientSize = new System.Drawing.Size(852, 457);
            this.DefaultControl = "txtID";
            this.DefaultControlForEdit = "comboSubprocess";
            this.IsSupportPrint = false;
            this.Name = "B40";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelStockType;
        private Win.UI.Label labelSubprocess;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtID;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.TextBox txtSewingLine;
        private Win.UI.Label label1;
        private Win.UI.ContextMenuStrip contextMenuStrip1;
        private Win.UI.Label label2;
        private Win.UI.Label labLocation;
        private Class.Txtfactory txtfactory;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private Class.ComboMDivision comboMDivision;
        private Class.ComboRFIDProcessLocation comboRFIDProcessLocation;
        private Win.UI.Label label4;
        private Win.UI.Button btnSetPanelCutcell;
        private Win.UI.TextBox txtSubprocess;
    }
}
