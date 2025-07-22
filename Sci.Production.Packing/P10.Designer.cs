namespace Sci.Production.Packing
{
    partial class P10
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
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.txtPO = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelPO = new Sci.Win.UI.Label();
            this.labelPackID = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.btnFind = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel1 = new Sci.Win.UI.Panel();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.panel4 = new Sci.Win.UI.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel3 = new Sci.Win.UI.Panel();
            this.labProcessingBar = new System.Windows.Forms.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.progressBarProcessing = new System.Windows.Forms.ProgressBar();
            this.ckOnlyDisplay = new Sci.Win.UI.CheckBox();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape5 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape4 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.backgroundDownloadSticker = new System.ComponentModel.BackgroundWorker();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 123);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(692, 345);
            this.panel5.TabIndex = 21;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(692, 345);
            this.gridDetail.TabIndex = 10;
            this.gridDetail.TabStop = false;
            // 
            // txtPO
            // 
            this.txtPO.BackColor = System.Drawing.Color.White;
            this.txtPO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPO.Location = new System.Drawing.Point(240, 15);
            this.txtPO.Name = "txtPO";
            this.txtPO.Size = new System.Drawing.Size(153, 23);
            this.txtPO.TabIndex = 3;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(12, 15);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(40, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(56, 15);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(100, 23);
            this.txtSP.TabIndex = 1;
            // 
            // labelPO
            // 
            this.labelPO.Location = new System.Drawing.Point(196, 15);
            this.labelPO.Name = "labelPO";
            this.labelPO.Size = new System.Drawing.Size(40, 23);
            this.labelPO.TabIndex = 2;
            this.labelPO.Text = "PO#";
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(433, 15);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(52, 23);
            this.labelPackID.TabIndex = 4;
            this.labelPackID.Text = "PackID";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(489, 15);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(120, 23);
            this.txtPackID.TabIndex = 5;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(623, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(702, 123);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 345);
            this.panel2.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(539, 88);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(625, 88);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 123);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 345);
            this.panel1.TabIndex = 17;
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(527, 51);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 7;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 468);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(712, 10);
            this.panel4.TabIndex = 20;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labProcessingBar);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.progressBarProcessing);
            this.panel3.Controls.Add(this.ckOnlyDisplay);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.txtPO);
            this.panel3.Controls.Add(this.labelSP);
            this.panel3.Controls.Add(this.txtSP);
            this.panel3.Controls.Add(this.labelPO);
            this.panel3.Controls.Add(this.labelPackID);
            this.panel3.Controls.Add(this.txtPackID);
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Controls.Add(this.btnImportFromBarcode);
            this.panel3.Controls.Add(this.shapeContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(712, 123);
            this.panel3.TabIndex = 19;
            // 
            // labProcessingBar
            // 
            this.labProcessingBar.AutoSize = true;
            this.labProcessingBar.BackColor = System.Drawing.Color.Transparent;
            this.labProcessingBar.Location = new System.Drawing.Point(232, 97);
            this.labProcessingBar.Name = "labProcessingBar";
            this.labProcessingBar.Size = new System.Drawing.Size(28, 17);
            this.labProcessingBar.TabIndex = 13;
            this.labProcessingBar.Text = "0/0";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 23);
            this.label4.TabIndex = 12;
            this.label4.Text = "Processing progress";
            // 
            // progressBarProcessing
            // 
            this.progressBarProcessing.BackColor = System.Drawing.SystemColors.Control;
            this.progressBarProcessing.Location = new System.Drawing.Point(148, 94);
            this.progressBarProcessing.Name = "progressBarProcessing";
            this.progressBarProcessing.Size = new System.Drawing.Size(208, 23);
            this.progressBarProcessing.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarProcessing.TabIndex = 11;
            // 
            // ckOnlyDisplay
            // 
            this.ckOnlyDisplay.AutoSize = true;
            this.ckOnlyDisplay.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ckOnlyDisplay.Location = new System.Drawing.Point(12, 57);
            this.ckOnlyDisplay.Name = "ckOnlyDisplay";
            this.ckOnlyDisplay.Size = new System.Drawing.Size(244, 21);
            this.ckOnlyDisplay.TabIndex = 10;
            this.ckOnlyDisplay.Text = "Only display Scan ＆ Pack cartons";
            this.ckOnlyDisplay.UseVisualStyleBackColor = true;
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape5,
            this.lineShape4,
            this.lineShape3,
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer2.Size = new System.Drawing.Size(712, 123);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // lineShape5
            // 
            this.lineShape5.Name = "lineShape5";
            this.lineShape5.X1 = 7;
            this.lineShape5.X2 = 706;
            this.lineShape5.Y1 = 84;
            this.lineShape5.Y2 = 84;
            // 
            // lineShape4
            // 
            this.lineShape4.Name = "lineShape4";
            this.lineShape4.X1 = 7;
            this.lineShape4.X2 = 706;
            this.lineShape4.Y1 = 46;
            this.lineShape4.Y2 = 46;
            // 
            // lineShape3
            // 
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = 706;
            this.lineShape3.X2 = 706;
            this.lineShape3.Y1 = 7;
            this.lineShape3.Y2 = 84;
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 6;
            this.lineShape2.X2 = 6;
            this.lineShape2.Y1 = 7;
            this.lineShape2.Y2 = 84;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 7;
            this.lineShape1.X2 = 706;
            this.lineShape1.Y1 = 7;
            this.lineShape1.Y2 = 7;
            // 
            // backgroundDownloadSticker
            // 
            this.backgroundDownloadSticker.WorkerReportsProgress = true;
            this.backgroundDownloadSticker.WorkerSupportsCancellation = true;
            this.backgroundDownloadSticker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundDownloadSticker_DoWork);
            this.backgroundDownloadSticker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundDownloadSticker_ProgressChanged);
            this.backgroundDownloadSticker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundDownloadSticker_RunWorkerCompleted);
            // 
            // P10
            // 
            this.ClientSize = new System.Drawing.Size(712, 478);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.EditMode = true;
            this.Name = "P10";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P10. Carton Transfer To Clog Input";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P10_FormClosed);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel5;
        private Win.UI.Grid gridDetail;
        private Win.UI.TextBox txtPO;
        private Win.UI.Label labelSP;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelPO;
        private Win.UI.Label labelPackID;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Button btnFind;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnImportFromBarcode;
        private Win.UI.Panel panel4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.Panel panel3;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape5;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape4;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.CheckBox ckOnlyDisplay;
        private System.Windows.Forms.Label labProcessingBar;
        private Win.UI.Label label4;
        private System.Windows.Forms.ProgressBar progressBarProcessing;
        private System.ComponentModel.BackgroundWorker backgroundDownloadSticker;
    }
}
