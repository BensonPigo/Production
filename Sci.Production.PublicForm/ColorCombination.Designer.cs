namespace Sci.Production.PublicForm
{
    partial class ColorCombination
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridFabric = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.gridColorDesc = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelFabric = new Sci.Win.UI.Label();
            this.labelColorDescription = new Sci.Win.UI.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelMiddleLeft = new Sci.Win.UI.Panel();
            this.panelFabric = new Sci.Win.UI.Panel();
            this.panelMiddleRight = new Sci.Win.UI.Panel();
            this.panelColor = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColorDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelMiddleLeft.SuspendLayout();
            this.panelFabric.SuspendLayout();
            this.panelMiddleRight.SuspendLayout();
            this.panelColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridFabric
            // 
            this.gridFabric.AllowUserToAddRows = false;
            this.gridFabric.AllowUserToDeleteRows = false;
            this.gridFabric.AllowUserToResizeRows = false;
            this.gridFabric.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFabric.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFabric.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFabric.DataSource = this.listControlBindingSource1;
            this.gridFabric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFabric.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFabric.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFabric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFabric.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFabric.Location = new System.Drawing.Point(0, 0);
            this.gridFabric.Name = "gridFabric";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFabric.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFabric.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFabric.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFabric.RowTemplate.Height = 24;
            this.gridFabric.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFabric.Size = new System.Drawing.Size(661, 459);
            this.gridFabric.TabIndex = 0;
            this.gridFabric.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(839, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // gridColorDesc
            // 
            this.gridColorDesc.AllowUserToAddRows = false;
            this.gridColorDesc.AllowUserToDeleteRows = false;
            this.gridColorDesc.AllowUserToResizeRows = false;
            this.gridColorDesc.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridColorDesc.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridColorDesc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridColorDesc.DataSource = this.listControlBindingSource2;
            this.gridColorDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridColorDesc.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridColorDesc.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridColorDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridColorDesc.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridColorDesc.Location = new System.Drawing.Point(0, 0);
            this.gridColorDesc.Name = "gridColorDesc";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridColorDesc.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridColorDesc.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridColorDesc.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridColorDesc.RowTemplate.Height = 24;
            this.gridColorDesc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridColorDesc.Size = new System.Drawing.Size(257, 459);
            this.gridColorDesc.TabIndex = 2;
            this.gridColorDesc.TabStop = false;
            // 
            // labelFabric
            // 
            this.labelFabric.Lines = 0;
            this.labelFabric.Location = new System.Drawing.Point(0, 0);
            this.labelFabric.Name = "labelFabric";
            this.labelFabric.Size = new System.Drawing.Size(75, 23);
            this.labelFabric.TabIndex = 3;
            this.labelFabric.Text = "Fabric";
            // 
            // labelColorDescription
            // 
            this.labelColorDescription.Lines = 0;
            this.labelColorDescription.Location = new System.Drawing.Point(0, -1);
            this.labelColorDescription.Name = "labelColorDescription";
            this.labelColorDescription.Size = new System.Drawing.Size(111, 23);
            this.labelColorDescription.TabIndex = 4;
            this.labelColorDescription.Text = "Color Description";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 481);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(922, 39);
            this.panelBottom.TabIndex = 6;
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.splitContainer1);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 0);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(922, 481);
            this.panelMiddle.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelMiddleLeft);
            this.splitContainer1.Panel1.Controls.Add(this.panelFabric);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelMiddleRight);
            this.splitContainer1.Panel2.Controls.Add(this.panelColor);
            this.splitContainer1.Size = new System.Drawing.Size(922, 481);
            this.splitContainer1.SplitterDistance = 661;
            this.splitContainer1.TabIndex = 0;
            // 
            // panelMiddleLeft
            // 
            this.panelMiddleLeft.Controls.Add(this.gridFabric);
            this.panelMiddleLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddleLeft.Location = new System.Drawing.Point(0, 22);
            this.panelMiddleLeft.Name = "panelMiddleLeft";
            this.panelMiddleLeft.Size = new System.Drawing.Size(661, 459);
            this.panelMiddleLeft.TabIndex = 2;
            // 
            // panelFabric
            // 
            this.panelFabric.Controls.Add(this.labelFabric);
            this.panelFabric.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFabric.Location = new System.Drawing.Point(0, 0);
            this.panelFabric.Name = "panelFabric";
            this.panelFabric.Size = new System.Drawing.Size(661, 22);
            this.panelFabric.TabIndex = 1;
            // 
            // panelMiddleRight
            // 
            this.panelMiddleRight.Controls.Add(this.gridColorDesc);
            this.panelMiddleRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddleRight.Location = new System.Drawing.Point(0, 22);
            this.panelMiddleRight.Name = "panelMiddleRight";
            this.panelMiddleRight.Size = new System.Drawing.Size(257, 459);
            this.panelMiddleRight.TabIndex = 4;
            // 
            // panelColor
            // 
            this.panelColor.Controls.Add(this.labelColorDescription);
            this.panelColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelColor.Location = new System.Drawing.Point(0, 0);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(257, 22);
            this.panelColor.TabIndex = 3;
            // 
            // ColorCombination
            // 
            this.ClientSize = new System.Drawing.Size(922, 520);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelBottom);
            this.Name = "ColorCombination";
            this.Text = "Color Combination";
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColorDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelMiddleLeft.ResumeLayout(false);
            this.panelFabric.ResumeLayout(false);
            this.panelMiddleRight.ResumeLayout(false);
            this.panelColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridFabric;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridColorDesc;
        private Win.UI.Label labelFabric;
        private Win.UI.Label labelColorDescription;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Panel panelMiddleLeft;
        private Win.UI.Panel panelFabric;
        private Win.UI.Panel panelMiddleRight;
        private Win.UI.Panel panelColor;
    }
}
