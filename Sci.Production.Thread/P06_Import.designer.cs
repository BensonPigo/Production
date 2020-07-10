namespace Sci.Production.Thread
{
    partial class P06_Import
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
            this.btnQuery = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtlocalitemEnd = new Sci.Production.Class.Txtlocalitem();
            this.label6 = new Sci.Win.UI.Label();
            this.txtthreadcolorEnd = new Sci.Production.Class.Txtthreadcolor();
            this.label8 = new Sci.Win.UI.Label();
            this.txtthreadcolorStart = new Sci.Production.Class.Txtthreadcolor();
            this.labelColor = new Sci.Win.UI.Label();
            this.txtthreadlocationEnd = new Sci.Production.Class.Txtthreadlocation();
            this.txtthreadlocationStart = new Sci.Production.Class.Txtthreadlocation();
            this.txtlocalitemStart = new Sci.Production.Class.Txtlocalitem();
            this.label1 = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(750, 13);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnImport);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 376);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(840, 44);
            this.panel2.TabIndex = 11;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(750, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(664, 7);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 81);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(840, 295);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtlocalitemEnd);
            this.panel1.Controls.Add(this.txtthreadcolorEnd);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtthreadcolorStart);
            this.panel1.Controls.Add(this.labelColor);
            this.panel1.Controls.Add(this.txtthreadlocationEnd);
            this.panel1.Controls.Add(this.txtthreadlocationStart);
            this.panel1.Controls.Add(this.txtlocalitemStart);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelLocation);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.labelRefno);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(840, 81);
            this.panel1.TabIndex = 10;
            // 
            // txtlocalitemEnd
            // 
            this.txtlocalitemEnd.BackColor = System.Drawing.Color.White;
            this.txtlocalitemEnd.CategoryObjectName = this.label6;
            this.txtlocalitemEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtlocalitemEnd.LocalSuppObjectName = null;
            this.txtlocalitemEnd.Location = new System.Drawing.Point(265, 17);
            this.txtlocalitemEnd.Name = "txtlocalitemEnd";
            this.txtlocalitemEnd.Size = new System.Drawing.Size(150, 23);
            this.txtlocalitemEnd.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(70, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 23;
            this.label6.Text = "Thread";
            this.label6.Visible = false;
            // 
            // txtthreadcolorEnd
            // 
            this.txtthreadcolorEnd.BackColor = System.Drawing.Color.White;
            this.txtthreadcolorEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadcolorEnd.IsSupportSytsemContextMenu = false;
            this.txtthreadcolorEnd.Location = new System.Drawing.Point(200, 49);
            this.txtthreadcolorEnd.Name = "txtthreadcolorEnd";
            this.txtthreadcolorEnd.Size = new System.Drawing.Size(90, 23);
            this.txtthreadcolorEnd.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(183, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 15);
            this.label8.TabIndex = 28;
            this.label8.Text = "~";
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtthreadcolorStart
            // 
            this.txtthreadcolorStart.BackColor = System.Drawing.Color.White;
            this.txtthreadcolorStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadcolorStart.IsSupportSytsemContextMenu = false;
            this.txtthreadcolorStart.Location = new System.Drawing.Point(90, 49);
            this.txtthreadcolorStart.Name = "txtthreadcolorStart";
            this.txtthreadcolorStart.Size = new System.Drawing.Size(90, 23);
            this.txtthreadcolorStart.TabIndex = 4;
            // 
            // labelColor
            // 
            this.labelColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelColor.Lines = 0;
            this.labelColor.Location = new System.Drawing.Point(13, 49);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 26;
            this.labelColor.Text = "Color";
            // 
            // txtthreadlocationEnd
            // 
            this.txtthreadlocationEnd.BackColor = System.Drawing.Color.White;
            this.txtthreadlocationEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadlocationEnd.IsSupportSytsemContextMenu = false;
            this.txtthreadlocationEnd.Location = new System.Drawing.Point(629, 17);
            this.txtthreadlocationEnd.Name = "txtthreadlocationEnd";
            this.txtthreadlocationEnd.Size = new System.Drawing.Size(90, 23);
            this.txtthreadlocationEnd.TabIndex = 3;
            // 
            // txtthreadlocationStart
            // 
            this.txtthreadlocationStart.BackColor = System.Drawing.Color.White;
            this.txtthreadlocationStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadlocationStart.IsSupportSytsemContextMenu = false;
            this.txtthreadlocationStart.Location = new System.Drawing.Point(519, 17);
            this.txtthreadlocationStart.Name = "txtthreadlocationStart";
            this.txtthreadlocationStart.Size = new System.Drawing.Size(90, 23);
            this.txtthreadlocationStart.TabIndex = 2;
            // 
            // txtlocalitemStart
            // 
            this.txtlocalitemStart.BackColor = System.Drawing.Color.White;
            this.txtlocalitemStart.CategoryObjectName = this.label6;
            this.txtlocalitemStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtlocalitemStart.LocalSuppObjectName = null;
            this.txtlocalitemStart.Location = new System.Drawing.Point(90, 17);
            this.txtlocalitemStart.Name = "txtlocalitemStart";
            this.txtlocalitemStart.Size = new System.Drawing.Size(150, 23);
            this.txtlocalitemStart.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(612, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "~";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelLocation
            // 
            this.labelLocation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelLocation.Lines = 0;
            this.labelLocation.Location = new System.Drawing.Point(436, 17);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(75, 23);
            this.labelLocation.TabIndex = 14;
            this.labelLocation.Text = "Location";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(248, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "~";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelRefno
            // 
            this.labelRefno.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(12, 17);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 5;
            this.labelRefno.Text = "Refno";
            // 
            // P06_Import
            // 
            this.ClientSize = new System.Drawing.Size(840, 420);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label6);
            this.Name = "P06_Import";
            this.Text = "Import from Thread Stock";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnQuery;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Grid grid1;
        private Win.UI.Panel panel1;
        private Win.UI.Label labelRefno;
        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.Label labelLocation;
        private Production.Class.Txtlocalitem txtlocalitemStart;
        private Win.UI.Label label6;
        private Class.Txtthreadlocation txtthreadlocationEnd;
        private Class.Txtthreadlocation txtthreadlocationStart;
        private Win.UI.Label labelColor;
        private Class.Txtthreadcolor txtthreadcolorEnd;
        private Win.UI.Label label8;
        private Class.Txtthreadcolor txtthreadcolorStart;
        private Class.Txtlocalitem txtlocalitemEnd;
    }
}
