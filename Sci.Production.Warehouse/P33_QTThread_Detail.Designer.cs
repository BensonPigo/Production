namespace Sci.Production.Warehouse
{
    partial class P33_QTThread_Detail
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel1 = new Sci.Win.UI.Panel();
            this.displayFabricPanelCode = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.labelSeason = new Sci.Win.UI.Label();
            this.displayStyleNo = new Sci.Win.UI.DisplayBox();
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new Sci.Win.UI.SplitContainer();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.gridDetail2 = new Sci.Win.UI.Grid();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 437);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 50);
            this.panel2.TabIndex = 12;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(856, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.displayFabricPanelCode);
            this.panel1.Controls.Add(this.displaySeason);
            this.panel1.Controls.Add(this.labelSeason);
            this.panel1.Controls.Add(this.displayStyleNo);
            this.panel1.Controls.Add(this.labelStyleNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 58);
            this.panel1.TabIndex = 13;
            // 
            // displayFabricPanelCode
            // 
            this.displayFabricPanelCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFabricPanelCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFabricPanelCode.Location = new System.Drawing.Point(584, 17);
            this.displayFabricPanelCode.Name = "displayFabricPanelCode";
            this.displayFabricPanelCode.Size = new System.Drawing.Size(121, 23);
            this.displayFabricPanelCode.TabIndex = 21;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(352, 17);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(64, 23);
            this.displaySeason.TabIndex = 20;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(286, 17);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(63, 23);
            this.labelSeason.TabIndex = 19;
            this.labelSeason.Text = "Season";
            // 
            // displayStyleNo
            // 
            this.displayStyleNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleNo.Location = new System.Drawing.Point(92, 17);
            this.displayStyleNo.Name = "displayStyleNo";
            this.displayStyleNo.Size = new System.Drawing.Size(165, 23);
            this.displayStyleNo.TabIndex = 18;
            // 
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(20, 17);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(69, 23);
            this.labelStyleNo.TabIndex = 17;
            this.labelStyleNo.Text = "Style No";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(448, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Fabric Panel Code";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 58);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(946, 379);
            this.panel3.TabIndex = 14;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridDetail);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridDetail2);
            this.splitContainer1.Size = new System.Drawing.Size(946, 379);
            this.splitContainer1.SplitterDistance = 188;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.TabStop = false;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersWidth = 21;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(946, 188);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // gridDetail2
            // 
            this.gridDetail2.AllowUserToAddRows = false;
            this.gridDetail2.AllowUserToDeleteRows = false;
            this.gridDetail2.AllowUserToResizeRows = false;
            this.gridDetail2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail2.Location = new System.Drawing.Point(0, 0);
            this.gridDetail2.Name = "gridDetail2";
            this.gridDetail2.RowHeadersWidth = 21;
            this.gridDetail2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail2.RowTemplate.Height = 24;
            this.gridDetail2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail2.ShowCellToolTips = false;
            this.gridDetail2.Size = new System.Drawing.Size(946, 187);
            this.gridDetail2.TabIndex = 1;
            this.gridDetail2.TabStop = false;
            // 
            // P33_QTThread_Detail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 487);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "P33_QTThread_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P33. QTThread_Detail";
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel1;
        private Win.UI.DisplayBox displayFabricPanelCode;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.Label labelSeason;
        private Win.UI.DisplayBox displayStyleNo;
        private Win.UI.Label labelStyleNo;
        private Win.UI.Label label2;
        private System.Windows.Forms.Panel panel3;
        private Win.UI.SplitContainer splitContainer1;
        private Win.UI.Grid gridDetail;
        private Win.UI.Grid gridDetail2;
    }
}