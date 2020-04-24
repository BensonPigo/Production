namespace Sci.Production.Cutting
{
    partial class P02_StandardQtyPlannedCuttingWIP
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabByGarment = new System.Windows.Forms.TabPage();
            this.gridGarment = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabByFabric_Panel_Code = new System.Windows.Forms.TabPage();
            this.gridFabric_Panel_Code = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.tabControl1.SuspendLayout();
            this.tabByGarment.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGarment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabByFabric_Panel_Code.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric_Panel_Code)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabByGarment);
            this.tabControl1.Controls.Add(this.tabByFabric_Panel_Code);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1006, 340);
            this.tabControl1.TabIndex = 1;
            // 
            // tabByGarment
            // 
            this.tabByGarment.Controls.Add(this.gridGarment);
            this.tabByGarment.Location = new System.Drawing.Point(4, 25);
            this.tabByGarment.Name = "tabByGarment";
            this.tabByGarment.Padding = new System.Windows.Forms.Padding(3);
            this.tabByGarment.Size = new System.Drawing.Size(998, 311);
            this.tabByGarment.TabIndex = 0;
            this.tabByGarment.Text = "By Garment";
            // 
            // gridGarment
            // 
            this.gridGarment.AllowUserToAddRows = false;
            this.gridGarment.AllowUserToDeleteRows = false;
            this.gridGarment.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridGarment.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.gridGarment.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridGarment.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridGarment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGarment.DataSource = this.listControlBindingSource1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridGarment.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridGarment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGarment.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridGarment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridGarment.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridGarment.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridGarment.Location = new System.Drawing.Point(3, 3);
            this.gridGarment.Name = "gridGarment";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridGarment.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridGarment.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridGarment.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridGarment.RowTemplate.Height = 24;
            this.gridGarment.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGarment.ShowCellToolTips = false;
            this.gridGarment.Size = new System.Drawing.Size(992, 305);
            this.gridGarment.TabIndex = 0;
            // 
            // tabByFabric_Panel_Code
            // 
            this.tabByFabric_Panel_Code.Controls.Add(this.gridFabric_Panel_Code);
            this.tabByFabric_Panel_Code.Location = new System.Drawing.Point(4, 25);
            this.tabByFabric_Panel_Code.Name = "tabByFabric_Panel_Code";
            this.tabByFabric_Panel_Code.Padding = new System.Windows.Forms.Padding(3);
            this.tabByFabric_Panel_Code.Size = new System.Drawing.Size(998, 311);
            this.tabByFabric_Panel_Code.TabIndex = 1;
            this.tabByFabric_Panel_Code.Text = "By Fabric_Panel_Code";
            // 
            // gridFabric_Panel_Code
            // 
            this.gridFabric_Panel_Code.AllowUserToAddRows = false;
            this.gridFabric_Panel_Code.AllowUserToDeleteRows = false;
            this.gridFabric_Panel_Code.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridFabric_Panel_Code.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gridFabric_Panel_Code.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFabric_Panel_Code.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFabric_Panel_Code.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFabric_Panel_Code.DataSource = this.listControlBindingSource2;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridFabric_Panel_Code.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridFabric_Panel_Code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFabric_Panel_Code.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFabric_Panel_Code.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFabric_Panel_Code.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFabric_Panel_Code.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFabric_Panel_Code.Location = new System.Drawing.Point(3, 3);
            this.gridFabric_Panel_Code.Name = "gridFabric_Panel_Code";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFabric_Panel_Code.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridFabric_Panel_Code.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFabric_Panel_Code.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFabric_Panel_Code.RowTemplate.Height = 24;
            this.gridFabric_Panel_Code.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFabric_Panel_Code.ShowCellToolTips = false;
            this.gridFabric_Panel_Code.Size = new System.Drawing.Size(992, 305);
            this.gridFabric_Panel_Code.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(919, 342);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P02_StandardQtyPlannedCuttingWIP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 376);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.Name = "P02_StandardQtyPlannedCuttingWIP";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P02_StandardQtyPlannedCuttingWIP";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.tabControl1.ResumeLayout(false);
            this.tabByGarment.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridGarment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabByFabric_Panel_Code.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFabric_Panel_Code)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabByGarment;
        private System.Windows.Forms.TabPage tabByFabric_Panel_Code;
        private Win.UI.Grid gridGarment;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridFabric_Panel_Code;
    }
}