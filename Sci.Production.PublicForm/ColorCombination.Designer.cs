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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.gridColorDesc = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelColorDescription = new Sci.Win.UI.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panelMiddleLeft = new Sci.Win.UI.Panel();
            this.gridColCombin = new Sci.Win.UI.Grid();
            this.panelFabric = new Sci.Win.UI.Panel();
            this.labelFabric = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.label1 = new Sci.Win.UI.Label();
            this.panel2 = new Sci.Win.UI.Panel();
            this.gridFabric = new Sci.Win.UI.Grid();
            this.listControlBindingSource3 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panelMiddleRight = new Sci.Win.UI.Panel();
            this.panelColor = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColorDesc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panelMiddleLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridColCombin)).BeginInit();
            this.panelFabric.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource3)).BeginInit();
            this.panelMiddleRight.SuspendLayout();
            this.panelColor.SuspendLayout();
            this.SuspendLayout();
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
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridColorDesc.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridColorDesc.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridColorDesc.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridColorDesc.RowTemplate.Height = 24;
            this.gridColorDesc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridColorDesc.ShowCellToolTips = false;
            this.gridColorDesc.Size = new System.Drawing.Size(257, 459);
            this.gridColorDesc.TabIndex = 2;
            this.gridColorDesc.TabStop = false;
            // 
            // labelColorDescription
            // 
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelMiddleRight);
            this.splitContainer1.Panel2.Controls.Add(this.panelColor);
            this.splitContainer1.Size = new System.Drawing.Size(922, 481);
            this.splitContainer1.SplitterDistance = 661;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panelMiddleLeft);
            this.splitContainer2.Panel1.Controls.Add(this.panelFabric);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(661, 481);
            this.splitContainer2.SplitterDistance = 235;
            this.splitContainer2.TabIndex = 5;
            // 
            // panelMiddleLeft
            // 
            this.panelMiddleLeft.Controls.Add(this.gridColCombin);
            this.panelMiddleLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddleLeft.Location = new System.Drawing.Point(0, 22);
            this.panelMiddleLeft.Name = "panelMiddleLeft";
            this.panelMiddleLeft.Size = new System.Drawing.Size(661, 213);
            this.panelMiddleLeft.TabIndex = 3;
            // 
            // gridColCombin
            // 
            this.gridColCombin.AllowUserToAddRows = false;
            this.gridColCombin.AllowUserToDeleteRows = false;
            this.gridColCombin.AllowUserToResizeRows = false;
            this.gridColCombin.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridColCombin.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridColCombin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridColCombin.DataSource = this.listControlBindingSource1;
            this.gridColCombin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridColCombin.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridColCombin.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridColCombin.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridColCombin.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridColCombin.Location = new System.Drawing.Point(0, 0);
            this.gridColCombin.Name = "gridColCombin";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridColCombin.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridColCombin.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridColCombin.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridColCombin.RowTemplate.Height = 24;
            this.gridColCombin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridColCombin.ShowCellToolTips = false;
            this.gridColCombin.Size = new System.Drawing.Size(661, 213);
            this.gridColCombin.TabIndex = 0;
            this.gridColCombin.TabStop = false;
            // 
            // panelFabric
            // 
            this.panelFabric.Controls.Add(this.labelFabric);
            this.panelFabric.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFabric.Location = new System.Drawing.Point(0, 0);
            this.panelFabric.Name = "panelFabric";
            this.panelFabric.Size = new System.Drawing.Size(661, 22);
            this.panelFabric.TabIndex = 2;
            // 
            // labelFabric
            // 
            this.labelFabric.Location = new System.Drawing.Point(0, 0);
            this.labelFabric.Name = "labelFabric";
            this.labelFabric.Size = new System.Drawing.Size(125, 23);
            this.labelFabric.TabIndex = 3;
            this.labelFabric.Text = "Color Combination";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(661, 25);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Fabric";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridFabric);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(661, 217);
            this.panel2.TabIndex = 5;
            // 
            // gridFabric
            // 
            this.gridFabric.AllowUserToAddRows = false;
            this.gridFabric.AllowUserToDeleteRows = false;
            this.gridFabric.AllowUserToResizeRows = false;
            this.gridFabric.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFabric.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFabric.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFabric.DataSource = this.listControlBindingSource3;
            this.gridFabric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFabric.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFabric.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFabric.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFabric.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFabric.Location = new System.Drawing.Point(0, 0);
            this.gridFabric.Name = "gridFabric";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFabric.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridFabric.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFabric.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFabric.RowTemplate.Height = 24;
            this.gridFabric.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFabric.ShowCellToolTips = false;
            this.gridFabric.Size = new System.Drawing.Size(661, 217);
            this.gridFabric.TabIndex = 0;
            this.gridFabric.TabStop = false;
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
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Color Combination";
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColorDesc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panelMiddleLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridColCombin)).EndInit();
            this.panelFabric.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource3)).EndInit();
            this.panelMiddleRight.ResumeLayout(false);
            this.panelColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridColorDesc;
        private Win.UI.Label labelColorDescription;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Panel panelMiddleRight;
        private Win.UI.Panel panelColor;
        private Win.UI.ListControlBindingSource listControlBindingSource3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Panel panelMiddleLeft;
        private Win.UI.Grid gridColCombin;
        private Win.UI.Panel panelFabric;
        private Win.UI.Label labelFabric;
        private Win.UI.Panel panel1;
        private Win.UI.Label label1;
        private Win.UI.Panel panel2;
        private Win.UI.Grid gridFabric;
    }
}
