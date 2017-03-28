namespace Sci.Production.Warehouse
{
    partial class P04_LocalTransaction
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel2 = new Sci.Win.UI.Panel();
            this.numBalance = new Sci.Win.UI.NumericBox();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.numReleaced = new Sci.Win.UI.NumericBox();
            this.numArrived = new Sci.Win.UI.NumericBox();
            this.btnReCalculate = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numBalance);
            this.panel2.Controls.Add(this.btnToExcel);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.numReleaced);
            this.panel2.Controls.Add(this.numArrived);
            this.panel2.Controls.Add(this.btnReCalculate);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 395);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(902, 48);
            this.panel2.TabIndex = 1;
            // 
            // numBalance
            // 
            this.numBalance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numBalance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBalance.IsSupportEditMode = false;
            this.numBalance.Location = new System.Drawing.Point(627, 15);
            this.numBalance.Name = "numBalance";
            this.numBalance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBalance.ReadOnly = true;
            this.numBalance.Size = new System.Drawing.Size(100, 23);
            this.numBalance.TabIndex = 5;
            this.numBalance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Location = new System.Drawing.Point(733, 11);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 1;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(339, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Total";
            // 
            // numReleaced
            // 
            this.numReleaced.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numReleaced.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numReleaced.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numReleaced.IsSupportEditMode = false;
            this.numReleaced.Location = new System.Drawing.Point(523, 15);
            this.numReleaced.Name = "numReleaced";
            this.numReleaced.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numReleaced.ReadOnly = true;
            this.numReleaced.Size = new System.Drawing.Size(100, 23);
            this.numReleaced.TabIndex = 2;
            this.numReleaced.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numArrived
            // 
            this.numArrived.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numArrived.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numArrived.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numArrived.IsSupportEditMode = false;
            this.numArrived.Location = new System.Drawing.Point(417, 15);
            this.numArrived.Name = "numArrived";
            this.numArrived.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numArrived.ReadOnly = true;
            this.numArrived.Size = new System.Drawing.Size(100, 23);
            this.numArrived.TabIndex = 1;
            this.numArrived.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnReCalculate
            // 
            this.btnReCalculate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnReCalculate.Location = new System.Drawing.Point(12, 11);
            this.btnReCalculate.Name = "btnReCalculate";
            this.btnReCalculate.Size = new System.Drawing.Size(113, 30);
            this.btnReCalculate.TabIndex = 0;
            this.btnReCalculate.Text = "Re-Calculate";
            this.btnReCalculate.UseVisualStyleBackColor = true;
            this.btnReCalculate.Click += new System.EventHandler(this.btnReCalculate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(819, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(902, 395);
            this.grid1.TabIndex = 2;
            this.grid1.TabStop = false;
            // 
            // P04_LocalTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 443);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel2);
            this.Name = "P04_LocalTransaction";
            this.Text = "P04_LocalTransaction";
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel2;
        private Win.UI.NumericBox numBalance;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label label1;
        private Win.UI.NumericBox numReleaced;
        private Win.UI.NumericBox numArrived;
        private Win.UI.Button btnReCalculate;
        private Win.UI.Button btnClose;
        private Win.UI.Grid grid1;
    }
}