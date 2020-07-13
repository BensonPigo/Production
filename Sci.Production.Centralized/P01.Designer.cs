namespace Sci.Production.Centralized
{
    partial class P01
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.btnQuery = new Sci.Win.UI.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dateSewingOutput = new Sci.Win.UI.DateBox();
            this.dateRangeBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelSewingOutput = new Sci.Win.UI.Label();
            this.labelRegion = new Sci.Win.UI.Label();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel2 = new Sci.Win.UI.Panel();
            this.comboBoxValue = new Sci.Win.UI.ComboBox();
            this.comboBoxDisplayBy = new Sci.Win.UI.ComboBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.labelValue = new Sci.Win.UI.Label();
            this.labelDisplayBy = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtcountry);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.dateSewingOutput);
            this.panel1.Controls.Add(this.dateRangeBuyerDelivery);
            this.panel1.Controls.Add(this.labelSewingOutput);
            this.panel1.Controls.Add(this.labelRegion);
            this.panel1.Controls.Add(this.labelBuyerDelivery);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(833, 63);
            this.panel1.TabIndex = 1;
            // 
            // txtcountry
            // 
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(478, 4);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 22);
            this.txtcountry.TabIndex = 2;
            this.txtcountry.TextBox1Binding = "";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(750, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(256, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(338, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "** The sewing output date is base date of calculation";
            // 
            // dateSewingOutput
            // 
            this.dateSewingOutput.Location = new System.Drawing.Point(104, 36);
            this.dateSewingOutput.Name = "dateSewingOutput";
            this.dateSewingOutput.Size = new System.Drawing.Size(130, 23);
            this.dateSewingOutput.TabIndex = 1;
            // 
            // dateRangeBuyerDelivery
            // 
            this.dateRangeBuyerDelivery.IsRequired = false;
            this.dateRangeBuyerDelivery.Location = new System.Drawing.Point(104, 4);
            this.dateRangeBuyerDelivery.Name = "dateRangeBuyerDelivery";
            this.dateRangeBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBuyerDelivery.TabIndex = 0;
            // 
            // labelSewingOutput
            // 
            this.labelSewingOutput.Location = new System.Drawing.Point(4, 36);
            this.labelSewingOutput.Name = "labelSewingOutput";
            this.labelSewingOutput.Size = new System.Drawing.Size(97, 23);
            this.labelSewingOutput.TabIndex = 0;
            this.labelSewingOutput.Text = "Sewing Output";
            // 
            // labelRegion
            // 
            this.labelRegion.Location = new System.Drawing.Point(399, 4);
            this.labelRegion.Name = "labelRegion";
            this.labelRegion.Size = new System.Drawing.Size(75, 23);
            this.labelRegion.TabIndex = 0;
            this.labelRegion.Text = "Region";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(4, 4);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(97, 23);
            this.labelBuyerDelivery.TabIndex = 0;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(2, 67);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(833, 361);
            this.grid1.TabIndex = 2;
            this.grid1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid1_CellDoubleClick);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.comboBoxValue);
            this.panel2.Controls.Add(this.comboBoxDisplayBy);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.labelValue);
            this.panel2.Controls.Add(this.labelDisplayBy);
            this.panel2.Location = new System.Drawing.Point(2, 429);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(833, 41);
            this.panel2.TabIndex = 3;
            // 
            // comboBoxValue
            // 
            this.comboBoxValue.BackColor = System.Drawing.Color.White;
            this.comboBoxValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxValue.FormattingEnabled = true;
            this.comboBoxValue.IsSupportUnselect = true;
            this.comboBoxValue.Items.AddRange(new object[] {
            "CPU",
            "Qty",
            "Delay CPU / Total CPU",
            "Delay Qty / Total Qty",
            "% (delay cpu / total cpu)"});
            this.comboBoxValue.Location = new System.Drawing.Point(263, 6);
            this.comboBoxValue.Name = "comboBoxValue";
            this.comboBoxValue.Size = new System.Drawing.Size(174, 24);
            this.comboBoxValue.TabIndex = 1;
            this.comboBoxValue.SelectedIndexChanged += new System.EventHandler(this.ComboBoxValue_SelectedIndexChanged);
            // 
            // comboBoxDisplayBy
            // 
            this.comboBoxDisplayBy.BackColor = System.Drawing.Color.White;
            this.comboBoxDisplayBy.DisplayMember = "M";
            this.comboBoxDisplayBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxDisplayBy.FormattingEnabled = true;
            this.comboBoxDisplayBy.IsSupportUnselect = true;
            this.comboBoxDisplayBy.Items.AddRange(new object[] {
            "M",
            "Brand"});
            this.comboBoxDisplayBy.Location = new System.Drawing.Point(83, 5);
            this.comboBoxDisplayBy.Name = "comboBoxDisplayBy";
            this.comboBoxDisplayBy.Size = new System.Drawing.Size(98, 24);
            this.comboBoxDisplayBy.TabIndex = 0;
            this.comboBoxDisplayBy.SelectedIndexChanged += new System.EventHandler(this.ComboBoxDisplayBy_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(750, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // labelValue
            // 
            this.labelValue.Location = new System.Drawing.Point(184, 6);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(75, 23);
            this.labelValue.TabIndex = 0;
            this.labelValue.Text = "Value";
            // 
            // labelDisplayBy
            // 
            this.labelDisplayBy.Location = new System.Drawing.Point(4, 5);
            this.labelDisplayBy.Name = "labelDisplayBy";
            this.labelDisplayBy.Size = new System.Drawing.Size(75, 23);
            this.labelDisplayBy.TabIndex = 0;
            this.labelDisplayBy.Text = "Display By";
            // 
            // P01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 470);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P01";
            this.Text = "P01. Potential delay Summary";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Class.Txtcountry txtcountry;
        private Win.UI.Button btnQuery;
        private System.Windows.Forms.Label label3;
        private Win.UI.DateBox dateSewingOutput;
        private Win.UI.DateRange dateRangeBuyerDelivery;
        private Win.UI.Label labelSewingOutput;
        private Win.UI.Label labelRegion;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel2;
        private Win.UI.ComboBox comboBoxValue;
        private Win.UI.ComboBox comboBoxDisplayBy;
        private Win.UI.Button btnClose;
        private Win.UI.Label labelValue;
        private Win.UI.Label labelDisplayBy;
    }
}