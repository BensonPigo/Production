namespace Sci.Production.PPIC
{
    partial class P01_QtyShip
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel8 = new Sci.Win.UI.Panel();
            this.panel10 = new Sci.Win.UI.Panel();
            this.gridQtyBreakDownbyArticleSizeDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel9 = new Sci.Win.UI.Panel();
            this.label1 = new Sci.Win.UI.Label();
            this.panel7 = new Sci.Win.UI.Panel();
            this.radioOriQty = new System.Windows.Forms.RadioButton();
            this.radioQty = new System.Windows.Forms.RadioButton();
            this.button1 = new Sci.Win.UI.Button();
            this.panel6 = new Sci.Win.UI.Panel();
            this.gridQtyBreakDownByShipmode = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDownbyArticleSizeDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel9.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDownByShipmode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 466);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(825, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 466);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(815, 10);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 461);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(815, 5);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(815, 451);
            this.panel5.TabIndex = 4;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel10);
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 135);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(815, 276);
            this.panel8.TabIndex = 2;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.gridQtyBreakDownbyArticleSizeDetail);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(0, 35);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(815, 241);
            this.panel10.TabIndex = 1;
            // 
            // gridQtyBreakDownbyArticleSizeDetail
            // 
            this.gridQtyBreakDownbyArticleSizeDetail.AllowUserToAddRows = false;
            this.gridQtyBreakDownbyArticleSizeDetail.AllowUserToDeleteRows = false;
            this.gridQtyBreakDownbyArticleSizeDetail.AllowUserToOrderColumns = false;
            this.gridQtyBreakDownbyArticleSizeDetail.AllowUserToResizeRows = false;
            this.gridQtyBreakDownbyArticleSizeDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQtyBreakDownbyArticleSizeDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridQtyBreakDownbyArticleSizeDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQtyBreakDownbyArticleSizeDetail.DataSource = this.listControlBindingSource2;
            this.gridQtyBreakDownbyArticleSizeDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridQtyBreakDownbyArticleSizeDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQtyBreakDownbyArticleSizeDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQtyBreakDownbyArticleSizeDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQtyBreakDownbyArticleSizeDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQtyBreakDownbyArticleSizeDetail.Location = new System.Drawing.Point(0, 0);
            this.gridQtyBreakDownbyArticleSizeDetail.Name = "gridQtyBreakDownbyArticleSizeDetail";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridQtyBreakDownbyArticleSizeDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridQtyBreakDownbyArticleSizeDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQtyBreakDownbyArticleSizeDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQtyBreakDownbyArticleSizeDetail.RowTemplate.Height = 24;
            this.gridQtyBreakDownbyArticleSizeDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQtyBreakDownbyArticleSizeDetail.Size = new System.Drawing.Size(815, 241);
            this.gridQtyBreakDownbyArticleSizeDetail.TabIndex = 0;
            this.gridQtyBreakDownbyArticleSizeDetail.TabStop = false;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.label1);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(815, 35);
            this.panel9.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Qty breakdown by Article/Size detail";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label1.TextStyle.Color = System.Drawing.Color.Red;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.radioOriQty);
            this.panel7.Controls.Add(this.radioQty);
            this.panel7.Controls.Add(this.button1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 411);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(815, 40);
            this.panel7.TabIndex = 1;
            // 
            // radioOriQty
            // 
            this.radioOriQty.AutoSize = true;
            this.radioOriQty.ForeColor = System.Drawing.Color.Red;
            this.radioOriQty.Location = new System.Drawing.Point(391, 10);
            this.radioOriQty.Name = "radioOriQty";
            this.radioOriQty.Size = new System.Drawing.Size(75, 21);
            this.radioOriQty.TabIndex = 4;
            this.radioOriQty.Text = "Ori. Qty";
            this.radioOriQty.UseVisualStyleBackColor = true;
            this.radioOriQty.CheckedChanged += new System.EventHandler(this.RadioOriQty_CheckedChanged);
            // 
            // radioQty
            // 
            this.radioQty.AutoSize = true;
            this.radioQty.Checked = true;
            this.radioQty.ForeColor = System.Drawing.Color.Red;
            this.radioQty.Location = new System.Drawing.Point(319, 10);
            this.radioQty.Name = "radioQty";
            this.radioQty.Size = new System.Drawing.Size(48, 21);
            this.radioQty.TabIndex = 3;
            this.radioQty.TabStop = true;
            this.radioQty.Text = "Qty";
            this.radioQty.UseVisualStyleBackColor = true;
            this.radioQty.CheckedChanged += new System.EventHandler(this.RadioOriQty_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(726, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.gridQtyBreakDownByShipmode);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(815, 135);
            this.panel6.TabIndex = 0;
            // 
            // gridQtyBreakDownByShipmode
            // 
            this.gridQtyBreakDownByShipmode.AllowUserToAddRows = false;
            this.gridQtyBreakDownByShipmode.AllowUserToDeleteRows = false;
            this.gridQtyBreakDownByShipmode.AllowUserToOrderColumns = false;
            this.gridQtyBreakDownByShipmode.AllowUserToResizeRows = false;
            this.gridQtyBreakDownByShipmode.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridQtyBreakDownByShipmode.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridQtyBreakDownByShipmode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridQtyBreakDownByShipmode.DataSource = this.listControlBindingSource1;
            this.gridQtyBreakDownByShipmode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridQtyBreakDownByShipmode.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridQtyBreakDownByShipmode.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridQtyBreakDownByShipmode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridQtyBreakDownByShipmode.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridQtyBreakDownByShipmode.Location = new System.Drawing.Point(0, 0);
            this.gridQtyBreakDownByShipmode.Name = "gridQtyBreakDownByShipmode";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridQtyBreakDownByShipmode.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridQtyBreakDownByShipmode.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridQtyBreakDownByShipmode.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridQtyBreakDownByShipmode.RowTemplate.Height = 24;
            this.gridQtyBreakDownByShipmode.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridQtyBreakDownByShipmode.Size = new System.Drawing.Size(815, 135);
            this.gridQtyBreakDownByShipmode.TabIndex = 0;
            this.gridQtyBreakDownByShipmode.TabStop = false;
            // 
            // P01_QtyShip
            // 
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(835, 466);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P01_QtyShip";
            this.Text = "Qty breakdown by shipmode";
            this.panel5.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDownbyArticleSizeDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridQtyBreakDownByShipmode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel8;
        private Win.UI.Panel panel10;
        private Win.UI.Grid gridQtyBreakDownbyArticleSizeDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel9;
        private Win.UI.Label label1;
        private Win.UI.Panel panel7;
        private Win.UI.Button button1;
        private Win.UI.Panel panel6;
        private Win.UI.Grid gridQtyBreakDownByShipmode;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.RadioButton radioOriQty;
        private System.Windows.Forms.RadioButton radioQty;
    }
}
