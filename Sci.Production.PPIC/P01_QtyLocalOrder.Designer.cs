namespace Sci.Production.PPIC
{
    partial class P01_QtyLocalOrder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnEdit = new Sci.Win.UI.Button();
            this.btnHorizontalDelete = new Sci.Win.UI.Button();
            this.btnVerticalDelete = new Sci.Win.UI.Button();
            this.btnHorizontalInsert = new Sci.Win.UI.Button();
            this.btnVerticalInsert = new Sci.Win.UI.Button();
            this.btnHorizontalAdd = new Sci.Win.UI.Button();
            this.btnVerticalAdd = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridLocalOrder = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLocalOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 428);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(798, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 428);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(788, 10);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnEdit);
            this.panel4.Controls.Add(this.btnHorizontalDelete);
            this.panel4.Controls.Add(this.btnVerticalDelete);
            this.panel4.Controls.Add(this.btnHorizontalInsert);
            this.panel4.Controls.Add(this.btnVerticalInsert);
            this.panel4.Controls.Add(this.btnHorizontalAdd);
            this.panel4.Controls.Add(this.btnVerticalAdd);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 353);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(788, 75);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(704, 41);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(704, 7);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnHorizontalDelete
            // 
            this.btnHorizontalDelete.Location = new System.Drawing.Point(320, 41);
            this.btnHorizontalDelete.Name = "btnHorizontalDelete";
            this.btnHorizontalDelete.Size = new System.Drawing.Size(140, 30);
            this.btnHorizontalDelete.TabIndex = 5;
            this.btnHorizontalDelete.Text = "Horizontal Delete";
            this.btnHorizontalDelete.UseVisualStyleBackColor = true;
            // 
            // btnVerticalDelete
            // 
            this.btnVerticalDelete.Location = new System.Drawing.Point(320, 7);
            this.btnVerticalDelete.Name = "btnVerticalDelete";
            this.btnVerticalDelete.Size = new System.Drawing.Size(140, 30);
            this.btnVerticalDelete.TabIndex = 2;
            this.btnVerticalDelete.Text = "Vertical Delete";
            this.btnVerticalDelete.UseVisualStyleBackColor = true;
            // 
            // btnHorizontalInsert
            // 
            this.btnHorizontalInsert.Location = new System.Drawing.Point(176, 41);
            this.btnHorizontalInsert.Name = "btnHorizontalInsert";
            this.btnHorizontalInsert.Size = new System.Drawing.Size(140, 30);
            this.btnHorizontalInsert.TabIndex = 4;
            this.btnHorizontalInsert.Text = "Horizontal Insert";
            this.btnHorizontalInsert.UseVisualStyleBackColor = true;
            // 
            // btnVerticalInsert
            // 
            this.btnVerticalInsert.Location = new System.Drawing.Point(176, 7);
            this.btnVerticalInsert.Name = "btnVerticalInsert";
            this.btnVerticalInsert.Size = new System.Drawing.Size(140, 30);
            this.btnVerticalInsert.TabIndex = 1;
            this.btnVerticalInsert.Text = "Vertical Insert";
            this.btnVerticalInsert.UseVisualStyleBackColor = true;
            // 
            // btnHorizontalAdd
            // 
            this.btnHorizontalAdd.Location = new System.Drawing.Point(32, 41);
            this.btnHorizontalAdd.Name = "btnHorizontalAdd";
            this.btnHorizontalAdd.Size = new System.Drawing.Size(140, 30);
            this.btnHorizontalAdd.TabIndex = 3;
            this.btnHorizontalAdd.Text = "Horizontal Add";
            this.btnHorizontalAdd.UseVisualStyleBackColor = true;
            // 
            // btnVerticalAdd
            // 
            this.btnVerticalAdd.Location = new System.Drawing.Point(32, 7);
            this.btnVerticalAdd.Name = "btnVerticalAdd";
            this.btnVerticalAdd.Size = new System.Drawing.Size(140, 30);
            this.btnVerticalAdd.TabIndex = 0;
            this.btnVerticalAdd.Text = "Vertical Add";
            this.btnVerticalAdd.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridLocalOrder);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(788, 343);
            this.panel5.TabIndex = 4;
            // 
            // gridLocalOrder
            // 
            this.gridLocalOrder.AllowUserToAddRows = false;
            this.gridLocalOrder.AllowUserToDeleteRows = false;
            this.gridLocalOrder.AllowUserToResizeRows = false;
            this.gridLocalOrder.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridLocalOrder.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridLocalOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLocalOrder.DataSource = this.listControlBindingSource1;
            this.gridLocalOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridLocalOrder.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridLocalOrder.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridLocalOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridLocalOrder.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridLocalOrder.Location = new System.Drawing.Point(0, 0);
            this.gridLocalOrder.Name = "gridLocalOrder";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLocalOrder.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridLocalOrder.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLocalOrder.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLocalOrder.RowTemplate.Height = 24;
            this.gridLocalOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLocalOrder.Size = new System.Drawing.Size(788, 343);
            this.gridLocalOrder.TabIndex = 0;
            this.gridLocalOrder.TabStop = false;
            // 
            // P01_QtyLocalOrder
            // 
            this.ClientSize = new System.Drawing.Size(808, 428);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P01_QtyLocalOrder";
            this.Text = "Quantity Breakdown for Local order";
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridLocalOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Button btnHorizontalAdd;
        private Win.UI.Button btnVerticalAdd;
        private Win.UI.Button btnHorizontalDelete;
        private Win.UI.Button btnVerticalDelete;
        private Win.UI.Button btnHorizontalInsert;
        private Win.UI.Button btnVerticalInsert;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnEdit;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid gridLocalOrder;
    }
}
