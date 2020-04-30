namespace Sci.Production.Packing
{
    partial class B04
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.numericLength = new Sci.Win.UI.NumericBox();
            this.numericWidth = new Sci.Win.UI.NumericBox();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.contextMenuStrip1 = new Sci.Win.UI.ContextMenuStrip();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtSize);
            this.detailcont.Controls.Add(this.numericWidth);
            this.detailcont.Controls.Add(this.numericLength);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            this.tabs.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(70, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sticker Size";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(70, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Width(mm)";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(400, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Length(mm)";
            // 
            // numericLength
            // 
            this.numericLength.BackColor = System.Drawing.Color.White;
            this.numericLength.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Length", true));
            this.numericLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericLength.Location = new System.Drawing.Point(496, 98);
            this.numericLength.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericLength.MaxLength = 7;
            this.numericLength.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericLength.Name = "numericLength";
            this.numericLength.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericLength.Size = new System.Drawing.Size(186, 23);
            this.numericLength.TabIndex = 3;
            this.numericLength.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericWidth
            // 
            this.numericWidth.BackColor = System.Drawing.Color.White;
            this.numericWidth.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Width", true));
            this.numericWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericWidth.Location = new System.Drawing.Point(166, 98);
            this.numericWidth.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.numericWidth.MaxLength = 7;
            this.numericWidth.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericWidth.Name = "numericWidth";
            this.numericWidth.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericWidth.Size = new System.Drawing.Size(186, 23);
            this.numericWidth.TabIndex = 2;
            this.numericWidth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.White;
            this.txtSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Size", true));
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSize.Location = new System.Drawing.Point(166, 38);
            this.txtSize.MaxLength = 20;
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(186, 23);
            this.txtSize.TabIndex = 4;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // B04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportPrint = false;
            this.IsSupportUnJunk = true;
            this.Name = "B04";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B04. Sticker Size Setting (For SNP)";
            this.WorkAlias = "StickerSize";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericBox numericWidth;
        private Win.UI.NumericBox numericLength;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSize;
        private Win.UI.ContextMenuStrip contextMenuStrip1;
    }
}