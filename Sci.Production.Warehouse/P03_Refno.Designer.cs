namespace Sci.Production.Warehouse
{
    partial class P03_Refno
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridRefNo = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.cmbFactory = new Sci.Win.UI.ComboBox();
            this.lbfactory = new Sci.Win.UI.Label();
            this.comboSize = new Sci.Win.UI.ComboBox();
            this.labelSize = new Sci.Win.UI.Label();
            this.comboColor = new Sci.Win.UI.ComboBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.labelFilterCondition = new Sci.Win.UI.Label();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRefNo)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridRefNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 503);
            this.panel1.TabIndex = 0;
            // 
            // gridRefNo
            // 
            this.gridRefNo.AllowUserToAddRows = false;
            this.gridRefNo.AllowUserToDeleteRows = false;
            this.gridRefNo.AllowUserToResizeRows = false;
            this.gridRefNo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRefNo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridRefNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRefNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRefNo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridRefNo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridRefNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridRefNo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridRefNo.Location = new System.Drawing.Point(0, 0);
            this.gridRefNo.Name = "gridRefNo";
            this.gridRefNo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRefNo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRefNo.RowTemplate.Height = 24;
            this.gridRefNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRefNo.Size = new System.Drawing.Size(1008, 503);
            this.gridRefNo.TabIndex = 0;
            this.gridRefNo.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.labelFilterCondition);
            this.panel2.Controls.Add(this.btnToExcel);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 503);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.cmbFactory);
            this.panel3.Controls.Add(this.lbfactory);
            this.panel3.Controls.Add(this.comboSize);
            this.panel3.Controls.Add(this.labelSize);
            this.panel3.Controls.Add(this.comboColor);
            this.panel3.Controls.Add(this.labelColor);
            this.panel3.Location = new System.Drawing.Point(114, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(545, 34);
            this.panel3.TabIndex = 4;
            // 
            // cmbFactory
            // 
            this.cmbFactory.BackColor = System.Drawing.Color.White;
            this.cmbFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbFactory.FormattingEnabled = true;
            this.cmbFactory.IsSupportUnselect = true;
            this.cmbFactory.Location = new System.Drawing.Point(67, 6);
            this.cmbFactory.Name = "cmbFactory";
            this.cmbFactory.Size = new System.Drawing.Size(121, 24);
            this.cmbFactory.TabIndex = 10;
            this.cmbFactory.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
            // 
            // lbfactory
            // 
            this.lbfactory.Location = new System.Drawing.Point(9, 6);
            this.lbfactory.Name = "lbfactory";
            this.lbfactory.Size = new System.Drawing.Size(55, 23);
            this.lbfactory.TabIndex = 9;
            this.lbfactory.Text = "Factory";
            // 
            // comboSize
            // 
            this.comboSize.BackColor = System.Drawing.Color.White;
            this.comboSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSize.FormattingEnabled = true;
            this.comboSize.IsSupportUnselect = true;
            this.comboSize.Location = new System.Drawing.Point(416, 7);
            this.comboSize.Name = "comboSize";
            this.comboSize.Size = new System.Drawing.Size(121, 24);
            this.comboSize.TabIndex = 8;
            this.comboSize.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
            // 
            // labelSize
            // 
            this.labelSize.Location = new System.Drawing.Point(375, 7);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(38, 23);
            this.labelSize.TabIndex = 7;
            this.labelSize.Text = "Size";
            // 
            // comboColor
            // 
            this.comboColor.BackColor = System.Drawing.Color.White;
            this.comboColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboColor.FormattingEnabled = true;
            this.comboColor.IsSupportUnselect = true;
            this.comboColor.Location = new System.Drawing.Point(246, 6);
            this.comboColor.Name = "comboColor";
            this.comboColor.Size = new System.Drawing.Size(121, 24);
            this.comboColor.TabIndex = 6;
            this.comboColor.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(199, 6);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(44, 23);
            this.labelColor.TabIndex = 5;
            this.labelColor.Text = "Color";
            // 
            // labelFilterCondition
            // 
            this.labelFilterCondition.Location = new System.Drawing.Point(9, 11);
            this.labelFilterCondition.Name = "labelFilterCondition";
            this.labelFilterCondition.Size = new System.Drawing.Size(102, 23);
            this.labelFilterCondition.TabIndex = 2;
            this.labelFilterCondition.Text = "Filter Condition";
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(830, 11);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 1;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(916, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P03_Refno
            // 
            this.ClientSize = new System.Drawing.Size(1008, 551);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "P03_Refno";
            this.Text = "Ref#";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRefNo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid gridRefNo;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnToExcel;
        private Win.UI.Panel panel3;
        private Win.UI.ComboBox comboSize;
        private Win.UI.Label labelSize;
        private Win.UI.ComboBox comboColor;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelFilterCondition;
        private Win.UI.ComboBox cmbFactory;
        private Win.UI.Label lbfactory;
    }
}
