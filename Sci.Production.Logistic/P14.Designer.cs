namespace Sci.Production.Logistic
{
    partial class P14
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
            this.gridImport = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel1 = new Sci.Win.UI.Panel();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4 = new Sci.Win.UI.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.labelPONo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.labelLocationNo = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.comboTransferTo = new Sci.Win.UI.ComboBox();
            this.numTotalPLQty = new Sci.Win.UI.NumericBox();
            this.numSelectedPLQty = new Sci.Win.UI.NumericBox();
            this.lbTotalCTNQty = new Sci.Win.UI.Label();
            this.lbSelectedCTNQty = new Sci.Win.UI.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.txtfactory = new Sci.Win.UI.TextBox();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridImport);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 107);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(857, 395);
            this.panel5.TabIndex = 21;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowHeadersVisible = false;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(857, 395);
            this.gridImport.TabIndex = 10;
            this.gridImport.TabStop = false;
            this.gridImport.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridImport_ColumnHeaderMouseClick);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(867, 107);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 395);
            this.panel2.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 107);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 395);
            this.panel1.TabIndex = 17;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 502);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(877, 10);
            this.panel4.TabIndex = 20;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Size = new System.Drawing.Size(877, 107);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(773, 2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.ButtonFind_Click);
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(411, 5);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(120, 23);
            this.txtPackID.TabIndex = 5;
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(355, 5);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(52, 23);
            this.labelPackID.TabIndex = 4;
            this.labelPackID.Text = "PackID";
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(155, 5);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(40, 23);
            this.labelPONo.TabIndex = 2;
            this.labelPONo.Text = "PO#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(52, 5);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(100, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(8, 5);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(40, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.White;
            this.txtPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONo.Location = new System.Drawing.Point(199, 5);
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.Size = new System.Drawing.Size(153, 23);
            this.txtPONo.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(777, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(691, 70);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // labelLocationNo
            // 
            this.labelLocationNo.Location = new System.Drawing.Point(12, 74);
            this.labelLocationNo.Name = "labelLocationNo";
            this.labelLocationNo.Size = new System.Drawing.Size(81, 23);
            this.labelLocationNo.TabIndex = 13;
            this.labelLocationNo.Text = "Transfer To";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Buyer Delivery";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(429, 34);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(52, 23);
            this.lbFactory.TabIndex = 35;
            this.lbFactory.Text = "Factory";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboTransferTo);
            this.panel3.Controls.Add(this.numTotalPLQty);
            this.panel3.Controls.Add(this.numSelectedPLQty);
            this.panel3.Controls.Add(this.lbTotalCTNQty);
            this.panel3.Controls.Add(this.lbSelectedCTNQty);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.labelLocationNo);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.shapeContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(877, 107);
            this.panel3.TabIndex = 19;
            // 
            // comboTransferTo
            // 
            this.comboTransferTo.BackColor = System.Drawing.Color.White;
            this.comboTransferTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTransferTo.FormattingEnabled = true;
            this.comboTransferTo.IsSupportUnselect = true;
            this.comboTransferTo.Location = new System.Drawing.Point(96, 73);
            this.comboTransferTo.Name = "comboTransferTo";
            this.comboTransferTo.OldText = "";
            this.comboTransferTo.Size = new System.Drawing.Size(78, 24);
            this.comboTransferTo.TabIndex = 46;
            this.comboTransferTo.SelectedIndexChanged += new System.EventHandler(this.ComboTransferTo_SelectedIndexChanged);
            // 
            // numTotalPLQty
            // 
            this.numTotalPLQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalPLQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalPLQty.IsSupportEditMode = false;
            this.numTotalPLQty.Location = new System.Drawing.Point(641, 74);
            this.numTotalPLQty.Name = "numTotalPLQty";
            this.numTotalPLQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalPLQty.ReadOnly = true;
            this.numTotalPLQty.Size = new System.Drawing.Size(44, 23);
            this.numTotalPLQty.TabIndex = 45;
            this.numTotalPLQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numSelectedPLQty
            // 
            this.numSelectedPLQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSelectedPLQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSelectedPLQty.IsSupportEditMode = false;
            this.numSelectedPLQty.Location = new System.Drawing.Point(482, 74);
            this.numSelectedPLQty.Name = "numSelectedPLQty";
            this.numSelectedPLQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSelectedPLQty.ReadOnly = true;
            this.numSelectedPLQty.Size = new System.Drawing.Size(41, 23);
            this.numSelectedPLQty.TabIndex = 44;
            this.numSelectedPLQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbTotalCTNQty
            // 
            this.lbTotalCTNQty.Location = new System.Drawing.Point(538, 74);
            this.lbTotalCTNQty.Name = "lbTotalCTNQty";
            this.lbTotalCTNQty.Size = new System.Drawing.Size(100, 23);
            this.lbTotalCTNQty.TabIndex = 43;
            this.lbTotalCTNQty.Text = "Total PL. Qty:";
            // 
            // lbSelectedCTNQty
            // 
            this.lbSelectedCTNQty.Location = new System.Drawing.Point(359, 74);
            this.lbSelectedCTNQty.Name = "lbSelectedCTNQty";
            this.lbSelectedCTNQty.Size = new System.Drawing.Size(120, 23);
            this.lbSelectedCTNQty.TabIndex = 41;
            this.lbSelectedCTNQty.Text = "Selected PL. Qty:";
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.txtfactory);
            this.panel6.Controls.Add(this.dateBuyerDelivery);
            this.panel6.Controls.Add(this.labelSPNo);
            this.panel6.Controls.Add(this.btnFind);
            this.panel6.Controls.Add(this.txtPackID);
            this.panel6.Controls.Add(this.labelPackID);
            this.panel6.Controls.Add(this.lbFactory);
            this.panel6.Controls.Add(this.labelPONo);
            this.panel6.Controls.Add(this.txtSPNo);
            this.panel6.Controls.Add(this.txtPONo);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(864, 63);
            this.panel6.TabIndex = 39;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(112, 34);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 37;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(484, 34);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(100, 23);
            this.txtfactory.TabIndex = 38;
            this.txtfactory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.Txtfactory_PopUp);
            this.txtfactory.Validating += new System.ComponentModel.CancelEventHandler(this.Txtfactory_Validating);
            // 
            // P14
            // 
            this.ClientSize = new System.Drawing.Size(877, 512);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.EditMode = true;
            this.Name = "P14";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P14. Carton Transfer To Sister Factory";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel5;
        private Win.UI.Grid gridImport;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labelPackID;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Label labelLocationNo;
        private Win.UI.Label label1;
        private Win.UI.Label lbFactory;
        private Win.UI.Panel panel3;
        private System.Windows.Forms.Panel panel6;
        private Win.UI.NumericBox numTotalPLQty;
        private Win.UI.NumericBox numSelectedPLQty;
        private Win.UI.Label lbTotalCTNQty;
        private Win.UI.Label lbSelectedCTNQty;
        private Win.UI.ComboBox comboTransferTo;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.TextBox txtfactory;
    }
}
