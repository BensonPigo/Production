namespace Sci.Production.PPIC
{
    partial class P01_PrintFabricSticker
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
            this.btnClose = new Sci.Win.UI.Button();
            this.gridPrintFabricSticker = new Sci.Win.UI.Grid();
            this.btnPrint = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridPrintFabricSticker)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(790, 338);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // gridPrintFabricSticker
            // 
            this.gridPrintFabricSticker.AllowUserToAddRows = false;
            this.gridPrintFabricSticker.AllowUserToDeleteRows = false;
            this.gridPrintFabricSticker.AllowUserToResizeRows = false;
            this.gridPrintFabricSticker.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPrintFabricSticker.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPrintFabricSticker.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPrintFabricSticker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPrintFabricSticker.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPrintFabricSticker.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPrintFabricSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPrintFabricSticker.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPrintFabricSticker.Location = new System.Drawing.Point(12, 12);
            this.gridPrintFabricSticker.Name = "gridPrintFabricSticker";
            this.gridPrintFabricSticker.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPrintFabricSticker.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPrintFabricSticker.RowTemplate.Height = 24;
            this.gridPrintFabricSticker.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPrintFabricSticker.ShowCellToolTips = false;
            this.gridPrintFabricSticker.Size = new System.Drawing.Size(858, 320);
            this.gridPrintFabricSticker.TabIndex = 3;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(704, 338);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // P01_PrintFabricSticker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 380);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridPrintFabricSticker);
            this.Name = "P01_PrintFabricSticker";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P01_PrintFabricSticker";
            this.Controls.SetChildIndex(this.gridPrintFabricSticker, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnPrint, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridPrintFabricSticker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnClose;
        private Win.UI.Grid gridPrintFabricSticker;
        private Win.UI.Button btnPrint;
    }
}