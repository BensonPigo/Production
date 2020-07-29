namespace Sci.Production.Cutting
{
    partial class P02_AutoDistToSP
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.BtnDist = new Sci.Win.UI.Button();
            this.BtnClose = new Sci.Win.UI.Button();
            this.numDistQty = new Sci.Win.UI.NumericBox();
            this.numBalQty = new Sci.Win.UI.NumericBox();
            this.Chknotyetallocation = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Dist. Qty";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(202, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bal. Qty";
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(9, 38);
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
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(779, 360);
            this.grid1.TabIndex = 5;
            this.grid1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Grid1_CellFormatting);
            this.grid1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Grid1_ColumnHeaderMouseClick);
            // 
            // BtnDist
            // 
            this.BtnDist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDist.Location = new System.Drawing.Point(622, 404);
            this.BtnDist.Name = "BtnDist";
            this.BtnDist.Size = new System.Drawing.Size(80, 30);
            this.BtnDist.TabIndex = 2;
            this.BtnDist.Text = "Dist.";
            this.BtnDist.UseVisualStyleBackColor = true;
            this.BtnDist.Click += new System.EventHandler(this.BtnDist_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClose.Location = new System.Drawing.Point(708, 404);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(80, 30);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // numDistQty
            // 
            this.numDistQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numDistQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numDistQty.IsSupportEditMode = false;
            this.numDistQty.Location = new System.Drawing.Point(87, 9);
            this.numDistQty.Name = "numDistQty";
            this.numDistQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDistQty.ReadOnly = true;
            this.numDistQty.Size = new System.Drawing.Size(100, 23);
            this.numDistQty.TabIndex = 0;
            this.numDistQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBalQty
            // 
            this.numBalQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBalQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBalQty.IsSupportEditMode = false;
            this.numBalQty.Location = new System.Drawing.Point(280, 9);
            this.numBalQty.Name = "numBalQty";
            this.numBalQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBalQty.ReadOnly = true;
            this.numBalQty.Size = new System.Drawing.Size(100, 23);
            this.numBalQty.TabIndex = 1;
            this.numBalQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // Chknotyetallocation
            // 
            this.Chknotyetallocation.AutoSize = true;
            this.Chknotyetallocation.Checked = true;
            this.Chknotyetallocation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Chknotyetallocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Chknotyetallocation.Location = new System.Drawing.Point(386, 9);
            this.Chknotyetallocation.Name = "Chknotyetallocation";
            this.Chknotyetallocation.Size = new System.Drawing.Size(233, 21);
            this.Chknotyetallocation.TabIndex = 6;
            this.Chknotyetallocation.Text = "Only show not yet allocation SP#";
            this.Chknotyetallocation.UseVisualStyleBackColor = true;
            this.Chknotyetallocation.CheckedChanged += new System.EventHandler(this.Chknotyetallocation_CheckedChanged);
            // 
            // P02_AutoDistToSP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 446);
            this.Controls.Add(this.Chknotyetallocation);
            this.Controls.Add(this.numBalQty);
            this.Controls.Add(this.numDistQty);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnDist);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P02_AutoDistToSP";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P02. Auto Distribute to SP#";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.BtnDist, 0);
            this.Controls.SetChildIndex(this.BtnClose, 0);
            this.Controls.SetChildIndex(this.numDistQty, 0);
            this.Controls.SetChildIndex(this.numBalQty, 0);
            this.Controls.SetChildIndex(this.Chknotyetallocation, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Grid grid1;
        private Win.UI.Button BtnDist;
        private Win.UI.Button BtnClose;
        private Win.UI.NumericBox numDistQty;
        private Win.UI.NumericBox numBalQty;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.CheckBox Chknotyetallocation;
    }
}