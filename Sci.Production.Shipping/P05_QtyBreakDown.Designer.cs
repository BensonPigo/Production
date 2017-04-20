namespace Sci.Production.Shipping
{
    partial class P05_QtyBreakDown
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
            this.labelInvoice = new Sci.Win.UI.Label();
            this.labelPacking = new Sci.Win.UI.Label();
            this.labelQty = new Sci.Win.UI.Label();
            this.labelCTNQty = new Sci.Win.UI.Label();
            this.displayInvoice = new Sci.Win.UI.DisplayBox();
            this.displayPacking = new Sci.Win.UI.DisplayBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.numCBM = new Sci.Win.UI.NumericBox();
            this.numGW = new Sci.Win.UI.NumericBox();
            this.labelCBM = new Sci.Win.UI.Label();
            this.labelGW = new Sci.Win.UI.Label();
            this.numCTNQty = new Sci.Win.UI.NumericBox();
            this.numQty = new Sci.Win.UI.NumericBox();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.gridQtyBreakDown = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 389);
            this.btmcont.Size = new System.Drawing.Size(626, 42);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(536, 5);
            this.undo.Size = new System.Drawing.Size(80, 32);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(456, 5);
            this.save.Size = new System.Drawing.Size(80, 32);
            // 
            // left
            // 
            this.left.Size = new System.Drawing.Size(80, 32);
            // 
            // right
            // 
            this.right.Size = new System.Drawing.Size(80, 32);
            // 
            // labelInvoice
            // 
            this.labelInvoice.Lines = 0;
            this.labelInvoice.Location = new System.Drawing.Point(6, 6);
            this.labelInvoice.Name = "labelInvoice";
            this.labelInvoice.Size = new System.Drawing.Size(65, 23);
            this.labelInvoice.TabIndex = 95;
            this.labelInvoice.Text = "Invoice#";
            // 
            // labelPacking
            // 
            this.labelPacking.Lines = 0;
            this.labelPacking.Location = new System.Drawing.Point(6, 33);
            this.labelPacking.Name = "labelPacking";
            this.labelPacking.Size = new System.Drawing.Size(65, 23);
            this.labelPacking.TabIndex = 96;
            this.labelPacking.Text = "Packing#";
            // 
            // labelQty
            // 
            this.labelQty.Lines = 0;
            this.labelQty.Location = new System.Drawing.Point(290, 6);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(63, 23);
            this.labelQty.TabIndex = 97;
            this.labelQty.Text = "Q\'ty";
            // 
            // labelCTNQty
            // 
            this.labelCTNQty.Lines = 0;
            this.labelCTNQty.Location = new System.Drawing.Point(290, 33);
            this.labelCTNQty.Name = "labelCTNQty";
            this.labelCTNQty.Size = new System.Drawing.Size(63, 23);
            this.labelCTNQty.TabIndex = 98;
            this.labelCTNQty.Text = "CTN Q\'ty";
            // 
            // displayInvoice
            // 
            this.displayInvoice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayInvoice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayInvoice.Location = new System.Drawing.Point(73, 6);
            this.displayInvoice.Name = "displayInvoice";
            this.displayInvoice.Size = new System.Drawing.Size(186, 23);
            this.displayInvoice.TabIndex = 99;
            // 
            // displayPacking
            // 
            this.displayPacking.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPacking.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayPacking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPacking.Location = new System.Drawing.Point(73, 33);
            this.displayPacking.Name = "displayPacking";
            this.displayPacking.Size = new System.Drawing.Size(120, 23);
            this.displayPacking.TabIndex = 100;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numCBM);
            this.panel1.Controls.Add(this.numGW);
            this.panel1.Controls.Add(this.labelCBM);
            this.panel1.Controls.Add(this.labelGW);
            this.panel1.Controls.Add(this.numCTNQty);
            this.panel1.Controls.Add(this.numQty);
            this.panel1.Controls.Add(this.labelInvoice);
            this.panel1.Controls.Add(this.displayPacking);
            this.panel1.Controls.Add(this.labelPacking);
            this.panel1.Controls.Add(this.displayInvoice);
            this.panel1.Controls.Add(this.labelQty);
            this.panel1.Controls.Add(this.labelCTNQty);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(626, 62);
            this.panel1.TabIndex = 101;
            // 
            // numCBM
            // 
            this.numCBM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCBM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CBM", true));
            this.numCBM.DecimalPlaces = 3;
            this.numCBM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCBM.IsSupportEditMode = false;
            this.numCBM.Location = new System.Drawing.Point(505, 33);
            this.numCBM.Name = "numCBM";
            this.numCBM.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCBM.ReadOnly = true;
            this.numCBM.Size = new System.Drawing.Size(70, 23);
            this.numCBM.TabIndex = 106;
            this.numCBM.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numGW
            // 
            this.numGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numGW.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "GW", true));
            this.numGW.DecimalPlaces = 3;
            this.numGW.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numGW.IsSupportEditMode = false;
            this.numGW.Location = new System.Drawing.Point(505, 6);
            this.numGW.Name = "numGW";
            this.numGW.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numGW.ReadOnly = true;
            this.numGW.Size = new System.Drawing.Size(70, 23);
            this.numGW.TabIndex = 105;
            this.numGW.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelCBM
            // 
            this.labelCBM.Lines = 0;
            this.labelCBM.Location = new System.Drawing.Point(464, 33);
            this.labelCBM.Name = "labelCBM";
            this.labelCBM.Size = new System.Drawing.Size(38, 23);
            this.labelCBM.TabIndex = 104;
            this.labelCBM.Text = "CBM";
            // 
            // labelGW
            // 
            this.labelGW.Lines = 0;
            this.labelGW.Location = new System.Drawing.Point(464, 6);
            this.labelGW.Name = "labelGW";
            this.labelGW.Size = new System.Drawing.Size(38, 23);
            this.labelGW.TabIndex = 103;
            this.labelGW.Text = "G.W.";
            // 
            // numCTNQty
            // 
            this.numCTNQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCTNQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNQty", true));
            this.numCTNQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCTNQty.IsSupportEditMode = false;
            this.numCTNQty.Location = new System.Drawing.Point(355, 33);
            this.numCTNQty.Name = "numCTNQty";
            this.numCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCTNQty.ReadOnly = true;
            this.numCTNQty.Size = new System.Drawing.Size(60, 23);
            this.numCTNQty.TabIndex = 102;
            this.numCTNQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numQty
            // 
            this.numQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ShipQty", true));
            this.numQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numQty.IsSupportEditMode = false;
            this.numQty.Location = new System.Drawing.Point(355, 6);
            this.numQty.Name = "numQty";
            this.numQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQty.ReadOnly = true;
            this.numQty.Size = new System.Drawing.Size(60, 23);
            this.numQty.TabIndex = 101;
            this.numQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(7, 327);
            this.panel2.TabIndex = 102;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(619, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(7, 327);
            this.panel3.TabIndex = 103;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.gridQtyBreakDown);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(7, 62);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(612, 327);
            this.panel4.TabIndex = 104;
            // 
            // gridQtyBreakDown
            // 
            this.gridQtyBreakDown.AllowUserToAddRows = false;
            this.gridQtyBreakDown.AllowUserToDeleteRows = false;
            this.gridQtyBreakDown.AllowUserToResizeRows = false;
            this.gridQtyBreakDown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQtyBreakDown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridQtyBreakDown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQtyBreakDown.DataSource = this.listControlBindingSource1;
            this.gridQtyBreakDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridQtyBreakDown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQtyBreakDown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQtyBreakDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQtyBreakDown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQtyBreakDown.Location = new System.Drawing.Point(0, 0);
            this.gridQtyBreakDown.Name = "gridQtyBreakDown";
            this.gridQtyBreakDown.RowHeadersVisible = false;
            this.gridQtyBreakDown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQtyBreakDown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQtyBreakDown.RowTemplate.Height = 24;
            this.gridQtyBreakDown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQtyBreakDown.Size = new System.Drawing.Size(612, 327);
            this.gridQtyBreakDown.TabIndex = 0;
            this.gridQtyBreakDown.TabStop = false;
            // 
            // P05_QtyBreakDown
            // 
            this.ClientSize = new System.Drawing.Size(626, 431);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P05_QtyBreakDown";
            this.Text = "Q\'ty B\'Down";
            this.WorkAlias = "PackingList";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labelInvoice;
        private Win.UI.Label labelPacking;
        private Win.UI.Label labelQty;
        private Win.UI.Label labelCTNQty;
        private Win.UI.DisplayBox displayInvoice;
        private Win.UI.DisplayBox displayPacking;
        private Win.UI.Panel panel1;
        private Win.UI.NumericBox numCBM;
        private Win.UI.NumericBox numGW;
        private Win.UI.Label labelCBM;
        private Win.UI.Label labelGW;
        private Win.UI.NumericBox numCTNQty;
        private Win.UI.NumericBox numQty;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Grid gridQtyBreakDown;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
