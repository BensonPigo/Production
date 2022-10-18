namespace Sci.Production.PPIC
{
    partial class P24_TemplateList
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridTemplateList = new Sci.Win.UI.Grid();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btn_Remove = new Sci.Win.UI.Button();
            this.btn_New = new Sci.Win.UI.Button();
            this.btn_Download = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridTemplateList)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridTemplateList
            // 
            this.gridTemplateList.AllowUserToAddRows = false;
            this.gridTemplateList.AllowUserToDeleteRows = false;
            this.gridTemplateList.AllowUserToResizeRows = false;
            this.gridTemplateList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTemplateList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTemplateList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTemplateList.DataSource = this.listControlBindingSource1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridTemplateList.DefaultCellStyle = dataGridViewCellStyle1;
            this.gridTemplateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTemplateList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTemplateList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTemplateList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTemplateList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTemplateList.Location = new System.Drawing.Point(0, 46);
            this.gridTemplateList.Name = "gridTemplateList";
            this.gridTemplateList.RowHeadersVisible = false;
            this.gridTemplateList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTemplateList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTemplateList.RowTemplate.Height = 24;
            this.gridTemplateList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTemplateList.ShowCellToolTips = false;
            this.gridTemplateList.Size = new System.Drawing.Size(777, 469);
            this.gridTemplateList.TabIndex = 5;
            this.gridTemplateList.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btn_Remove);
            this.panel3.Controls.Add(this.btn_New);
            this.panel3.Controls.Add(this.btn_Download);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(777, 46);
            this.panel3.TabIndex = 6;
            // 
            // btn_Remove
            // 
            this.btn_Remove.BackColor = System.Drawing.Color.Gray;
            this.btn_Remove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Remove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Remove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Remove.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Remove.ForeColor = System.Drawing.Color.White;
            this.btn_Remove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Remove.Location = new System.Drawing.Point(438, 7);
            this.btn_Remove.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Remove.Name = "btn_Remove";
            this.btn_Remove.Size = new System.Drawing.Size(111, 32);
            this.btn_Remove.TabIndex = 24;
            this.btn_Remove.Text = "Remove";
            this.btn_Remove.UseVisualStyleBackColor = false;
            this.btn_Remove.Click += new System.EventHandler(this.btn_Remove_Click);
            // 
            // btn_New
            // 
            this.btn_New.BackColor = System.Drawing.Color.Gray;
            this.btn_New.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_New.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_New.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_New.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_New.ForeColor = System.Drawing.Color.White;
            this.btn_New.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_New.Location = new System.Drawing.Point(329, 7);
            this.btn_New.Margin = new System.Windows.Forms.Padding(0);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(95, 32);
            this.btn_New.TabIndex = 23;
            this.btn_New.Text = "New";
            this.btn_New.UseVisualStyleBackColor = false;
            this.btn_New.Click += new System.EventHandler(this.btn_New_Click);
            // 
            // btn_Download
            // 
            this.btn_Download.BackColor = System.Drawing.Color.Gray;
            this.btn_Download.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Download.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btn_Download.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Download.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Download.ForeColor = System.Drawing.Color.White;
            this.btn_Download.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Download.Location = new System.Drawing.Point(625, 7);
            this.btn_Download.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Download.Name = "btn_Download";
            this.btn_Download.Size = new System.Drawing.Size(132, 32);
            this.btn_Download.TabIndex = 22;
            this.btn_Download.Text = "Download";
            this.btn_Download.UseVisualStyleBackColor = false;
            this.btn_Download.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // P24_TemplateList
            // 
            this.ClientSize = new System.Drawing.Size(777, 515);
            this.Controls.Add(this.gridTemplateList);
            this.Controls.Add(this.panel3);
            this.Name = "P24_TemplateList";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Template/Auto Template List";
            ((System.ComponentModel.ISupportInitialize)(this.gridTemplateList)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Grid gridTemplateList;
        private Win.UI.Panel panel3;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btn_Remove;
        private Win.UI.Button btn_New;
        private Win.UI.Button btn_Download;
    }
}
