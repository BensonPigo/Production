namespace Sci.Production.Logistic
{
    partial class P02_BatchReceiving
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
            this.labelTransferClogNo = new Sci.Win.UI.Label();
            this.txtTransferClogNoStart = new Sci.Win.UI.TextBox();
            this.btnFind = new Sci.Win.UI.Button();
            this.labelTTLCTN = new Sci.Win.UI.Label();
            this.numTTLCTN = new Sci.Win.UI.NumericBox();
            this.labelReceivedCTN = new Sci.Win.UI.Label();
            this.numReceivedCTN = new Sci.Win.UI.NumericBox();
            this.labelNotYetReceivedCTN = new Sci.Win.UI.Label();
            this.numNotYetReceivedCTN = new Sci.Win.UI.NumericBox();
            this.lineShape5 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape4 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.labelLocationNo = new Sci.Win.UI.Label();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridCartonReceiving = new Sci.Win.UI.Grid();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.labelNoOfSelected = new Sci.Win.UI.Label();
            this.numNoOfSelected = new Sci.Win.UI.NumericBox();
            this.txtTransferClogNoEnd = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.txtcloglocationLocationNo = new Sci.Production.Class.Txtcloglocation();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.panel4 = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonReceiving)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTransferClogNo
            // 
            this.labelTransferClogNo.Lines = 0;
            this.labelTransferClogNo.Location = new System.Drawing.Point(22, 16);
            this.labelTransferClogNo.Name = "labelTransferClogNo";
            this.labelTransferClogNo.Size = new System.Drawing.Size(111, 23);
            this.labelTransferClogNo.TabIndex = 0;
            this.labelTransferClogNo.Text = "Transfer Clog No";
            // 
            // txtTransferClogNoStart
            // 
            this.txtTransferClogNoStart.BackColor = System.Drawing.Color.White;
            this.txtTransferClogNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransferClogNoStart.Location = new System.Drawing.Point(137, 16);
            this.txtTransferClogNoStart.Name = "txtTransferClogNoStart";
            this.txtTransferClogNoStart.Size = new System.Drawing.Size(120, 23);
            this.txtTransferClogNoStart.TabIndex = 1;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(413, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // labelTTLCTN
            // 
            this.labelTTLCTN.Lines = 0;
            this.labelTTLCTN.Location = new System.Drawing.Point(22, 49);
            this.labelTTLCTN.Name = "labelTTLCTN";
            this.labelTTLCTN.Size = new System.Drawing.Size(64, 23);
            this.labelTTLCTN.TabIndex = 3;
            this.labelTTLCTN.Text = "TTL CTN";
            // 
            // numTTLCTN
            // 
            this.numTTLCTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTTLCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTTLCTN.IsSupportEditMode = false;
            this.numTTLCTN.Location = new System.Drawing.Point(90, 49);
            this.numTTLCTN.Name = "numTTLCTN";
            this.numTTLCTN.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTTLCTN.ReadOnly = true;
            this.numTTLCTN.Size = new System.Drawing.Size(70, 23);
            this.numTTLCTN.TabIndex = 4;
            this.numTTLCTN.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelReceivedCTN
            // 
            this.labelReceivedCTN.Lines = 0;
            this.labelReceivedCTN.Location = new System.Drawing.Point(212, 49);
            this.labelReceivedCTN.Name = "labelReceivedCTN";
            this.labelReceivedCTN.Size = new System.Drawing.Size(97, 23);
            this.labelReceivedCTN.TabIndex = 5;
            this.labelReceivedCTN.Text = "Received CTN";
            // 
            // numReceivedCTN
            // 
            this.numReceivedCTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numReceivedCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numReceivedCTN.IsSupportEditMode = false;
            this.numReceivedCTN.Location = new System.Drawing.Point(313, 49);
            this.numReceivedCTN.Name = "numReceivedCTN";
            this.numReceivedCTN.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numReceivedCTN.ReadOnly = true;
            this.numReceivedCTN.Size = new System.Drawing.Size(70, 23);
            this.numReceivedCTN.TabIndex = 6;
            this.numReceivedCTN.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelNotYetReceivedCTN
            // 
            this.labelNotYetReceivedCTN.Lines = 0;
            this.labelNotYetReceivedCTN.Location = new System.Drawing.Point(429, 49);
            this.labelNotYetReceivedCTN.Name = "labelNotYetReceivedCTN";
            this.labelNotYetReceivedCTN.Size = new System.Drawing.Size(139, 23);
            this.labelNotYetReceivedCTN.TabIndex = 7;
            this.labelNotYetReceivedCTN.Text = "Not yet received CTN";
            // 
            // numNotYetReceivedCTN
            // 
            this.numNotYetReceivedCTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numNotYetReceivedCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numNotYetReceivedCTN.IsSupportEditMode = false;
            this.numNotYetReceivedCTN.Location = new System.Drawing.Point(572, 49);
            this.numNotYetReceivedCTN.Name = "numNotYetReceivedCTN";
            this.numNotYetReceivedCTN.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNotYetReceivedCTN.ReadOnly = true;
            this.numNotYetReceivedCTN.Size = new System.Drawing.Size(70, 23);
            this.numNotYetReceivedCTN.TabIndex = 8;
            this.numNotYetReceivedCTN.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lineShape5
            // 
            this.lineShape5.Name = "lineShape5";
            this.lineShape5.X1 = 689;
            this.lineShape5.X2 = 689;
            this.lineShape5.Y1 = 7;
            this.lineShape5.Y2 = 119;
            // 
            // lineShape4
            // 
            this.lineShape4.Name = "lineShape4";
            this.lineShape4.X1 = 12;
            this.lineShape4.X2 = 12;
            this.lineShape4.Y1 = 7;
            this.lineShape4.Y2 = 119;
            // 
            // lineShape3
            // 
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = 12;
            this.lineShape3.X2 = 688;
            this.lineShape3.Y1 = 119;
            this.lineShape3.Y2 = 119;
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 12;
            this.lineShape2.X2 = 688;
            this.lineShape2.Y1 = 80;
            this.lineShape2.Y2 = 80;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 12;
            this.lineShape1.X2 = 688;
            this.lineShape1.Y1 = 7;
            this.lineShape1.Y2 = 7;
            // 
            // labelLocationNo
            // 
            this.labelLocationNo.Lines = 0;
            this.labelLocationNo.Location = new System.Drawing.Point(22, 89);
            this.labelLocationNo.Name = "labelLocationNo";
            this.labelLocationNo.Size = new System.Drawing.Size(81, 23);
            this.labelLocationNo.TabIndex = 10;
            this.labelLocationNo.Text = "Location No";
            // 
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(203, 85);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(163, 30);
            this.btnUpdateAllLocation.TabIndex = 12;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.BtnUpdateAllLocation_Click);
            // 
            // gridCartonReceiving
            // 
            this.gridCartonReceiving.AllowUserToAddRows = false;
            this.gridCartonReceiving.AllowUserToDeleteRows = false;
            this.gridCartonReceiving.AllowUserToResizeRows = false;
            this.gridCartonReceiving.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCartonReceiving.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCartonReceiving.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCartonReceiving.DataSource = this.bindingSource1;
            this.gridCartonReceiving.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCartonReceiving.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCartonReceiving.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCartonReceiving.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCartonReceiving.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCartonReceiving.Location = new System.Drawing.Point(0, 0);
            this.gridCartonReceiving.Name = "gridCartonReceiving";
            this.gridCartonReceiving.RowHeadersVisible = false;
            this.gridCartonReceiving.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCartonReceiving.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCartonReceiving.RowTemplate.Height = 24;
            this.gridCartonReceiving.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCartonReceiving.Size = new System.Drawing.Size(676, 326);
            this.gridCartonReceiving.TabIndex = 13;
            this.gridCartonReceiving.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(508, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(594, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 17;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // labelNoOfSelected
            // 
            this.labelNoOfSelected.Lines = 0;
            this.labelNoOfSelected.Location = new System.Drawing.Point(65, 6);
            this.labelNoOfSelected.Name = "labelNoOfSelected";
            this.labelNoOfSelected.Size = new System.Drawing.Size(94, 23);
            this.labelNoOfSelected.TabIndex = 18;
            this.labelNoOfSelected.Text = "No of selected";
            // 
            // numNoOfSelected
            // 
            this.numNoOfSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numNoOfSelected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numNoOfSelected.IsSupportEditMode = false;
            this.numNoOfSelected.Location = new System.Drawing.Point(163, 6);
            this.numNoOfSelected.Name = "numNoOfSelected";
            this.numNoOfSelected.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoOfSelected.ReadOnly = true;
            this.numNoOfSelected.Size = new System.Drawing.Size(70, 23);
            this.numNoOfSelected.TabIndex = 19;
            this.numNoOfSelected.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtTransferClogNoEnd
            // 
            this.txtTransferClogNoEnd.BackColor = System.Drawing.Color.White;
            this.txtTransferClogNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransferClogNoEnd.Location = new System.Drawing.Point(276, 16);
            this.txtTransferClogNoEnd.Name = "txtTransferClogNoEnd";
            this.txtTransferClogNoEnd.Size = new System.Drawing.Size(120, 23);
            this.txtTransferClogNoEnd.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(260, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 23);
            this.label7.TabIndex = 21;
            this.label7.Text = "~";
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridCartonReceiving);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(13, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(689, 326);
            this.panel1.TabIndex = 22;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(676, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(13, 326);
            this.panel5.TabIndex = 14;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelNoOfSelected);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.numNoOfSelected);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 448);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(702, 42);
            this.panel2.TabIndex = 23;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelTTLCTN);
            this.panel3.Controls.Add(this.labelTransferClogNo);
            this.panel3.Controls.Add(this.txtTransferClogNoStart);
            this.panel3.Controls.Add(this.txtcloglocationLocationNo);
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.numTTLCTN);
            this.panel3.Controls.Add(this.txtTransferClogNoEnd);
            this.panel3.Controls.Add(this.labelReceivedCTN);
            this.panel3.Controls.Add(this.btnUpdateAllLocation);
            this.panel3.Controls.Add(this.numReceivedCTN);
            this.panel3.Controls.Add(this.labelLocationNo);
            this.panel3.Controls.Add(this.labelNotYetReceivedCTN);
            this.panel3.Controls.Add(this.numNotYetReceivedCTN);
            this.panel3.Controls.Add(this.shapeContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(702, 122);
            this.panel3.TabIndex = 24;
            // 
            // txtcloglocationLocationNo
            // 
            this.txtcloglocationLocationNo.BackColor = System.Drawing.Color.White;
            this.txtcloglocationLocationNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcloglocationLocationNo.IsSupportSytsemContextMenu = false;
            this.txtcloglocationLocationNo.Location = new System.Drawing.Point(107, 89);
            this.txtcloglocationLocationNo.MDivisionObjectName = null;
            this.txtcloglocationLocationNo.Name = "txtcloglocationLocationNo";
            this.txtcloglocationLocationNo.Size = new System.Drawing.Size(80, 23);
            this.txtcloglocationLocationNo.TabIndex = 11;
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
            this.shapeContainer2.Size = new System.Drawing.Size(702, 122);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 122);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(13, 326);
            this.panel4.TabIndex = 14;
            // 
            // P02_BatchReceiving
            // 
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(702, 490);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Name = "P02_BatchReceiving";
            this.Text = "Carton Receiving - Batch Receive";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonReceiving)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labelTransferClogNo;
        private Win.UI.TextBox txtTransferClogNoStart;
        private Win.UI.Button btnFind;
        private Win.UI.Label labelTTLCTN;
        private Win.UI.NumericBox numTTLCTN;
        private Win.UI.Label labelReceivedCTN;
        private Win.UI.NumericBox numReceivedCTN;
        private Win.UI.Label labelNotYetReceivedCTN;
        private Win.UI.NumericBox numNotYetReceivedCTN;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Label labelLocationNo;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape5;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape4;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        private Class.Txtcloglocation txtcloglocationLocationNo;
        private Win.UI.Button btnUpdateAllLocation;
        //private Win.UI.BindingSource bindingSource1;
        private Win.UI.ListControlBindingSource bindingSource1;
        private Win.UI.Grid gridCartonReceiving;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnCancel;
        private Win.UI.Label labelNoOfSelected;
        private Win.UI.NumericBox numNoOfSelected;
        private Win.UI.TextBox txtTransferClogNoEnd;
        private Win.UI.Label label7;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel4;
    }
}
