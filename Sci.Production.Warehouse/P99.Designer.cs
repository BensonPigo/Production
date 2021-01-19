namespace Sci.Production.Warehouse
{
    partial class P99
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.UpdateCommand = new System.Windows.Forms.TabPage();
            this.panel7 = new Sci.Win.UI.Panel();
            this.gridSewingOutput = new Sci.Win.UI.Grid();
            this.panel6 = new Sci.Win.UI.Panel();
            this.UnLock = new System.Windows.Forms.TabPage();
            this.panel9 = new Sci.Win.UI.Panel();
            this.gridCutting = new Sci.Win.UI.Grid();
            this.panel8 = new Sci.Win.UI.Panel();
            this.numCuttingQty = new Sci.Win.UI.NumericBox();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.dateCreate = new Sci.Win.UI.DateRange();
            this.labSPNo = new Sci.Win.UI.Label();
            this.labCreateDate = new Sci.Win.UI.Label();
            this.labelFunction = new Sci.Win.UI.Label();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.tabControl1.SuspendLayout();
            this.UpdateCommand.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSewingOutput)).BeginInit();
            this.panel6.SuspendLayout();
            this.UnLock.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutting)).BeginInit();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.UpdateCommand);
            this.tabControl1.Controls.Add(this.UnLock);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(960, 546);
            this.tabControl1.TabIndex = 1;
            // 
            // UpdateCommand
            // 
            this.UpdateCommand.Controls.Add(this.panel7);
            this.UpdateCommand.Controls.Add(this.panel6);
            this.UpdateCommand.Location = new System.Drawing.Point(4, 25);
            this.UpdateCommand.Name = "UpdateCommand";
            this.UpdateCommand.Padding = new System.Windows.Forms.Padding(3);
            this.UpdateCommand.Size = new System.Drawing.Size(952, 517);
            this.UpdateCommand.TabIndex = 0;
            this.UpdateCommand.Text = "Update Command";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gridSewingOutput);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 96);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(946, 418);
            this.panel7.TabIndex = 1;
            // 
            // gridSewingOutput
            // 
            this.gridSewingOutput.AllowUserToAddRows = false;
            this.gridSewingOutput.AllowUserToDeleteRows = false;
            this.gridSewingOutput.AllowUserToResizeRows = false;
            this.gridSewingOutput.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSewingOutput.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSewingOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSewingOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSewingOutput.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSewingOutput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSewingOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSewingOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSewingOutput.Location = new System.Drawing.Point(0, 0);
            this.gridSewingOutput.Name = "gridSewingOutput";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSewingOutput.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSewingOutput.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSewingOutput.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSewingOutput.RowTemplate.Height = 24;
            this.gridSewingOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSewingOutput.ShowCellToolTips = false;
            this.gridSewingOutput.Size = new System.Drawing.Size(946, 418);
            this.gridSewingOutput.TabIndex = 0;
            this.gridSewingOutput.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.comboStockType);
            this.panel6.Controls.Add(this.labelFunction);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.txtSPNoEnd);
            this.panel6.Controls.Add(this.txtSPNoStart);
            this.panel6.Controls.Add(this.dateCreate);
            this.panel6.Controls.Add(this.labSPNo);
            this.panel6.Controls.Add(this.labCreateDate);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(946, 93);
            this.panel6.TabIndex = 0;
            // 
            // UnLock
            // 
            this.UnLock.Controls.Add(this.panel9);
            this.UnLock.Controls.Add(this.panel8);
            this.UnLock.Location = new System.Drawing.Point(4, 25);
            this.UnLock.Name = "UnLock";
            this.UnLock.Padding = new System.Windows.Forms.Padding(3);
            this.UnLock.Size = new System.Drawing.Size(952, 517);
            this.UnLock.TabIndex = 1;
            this.UnLock.Text = "UnLock";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.gridCutting);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(3, 3);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(946, 452);
            this.panel9.TabIndex = 1;
            // 
            // gridCutting
            // 
            this.gridCutting.AllowUserToAddRows = false;
            this.gridCutting.AllowUserToDeleteRows = false;
            this.gridCutting.AllowUserToResizeRows = false;
            this.gridCutting.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCutting.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCutting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCutting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCutting.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCutting.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCutting.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCutting.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCutting.Location = new System.Drawing.Point(0, 0);
            this.gridCutting.Name = "gridCutting";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridCutting.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridCutting.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCutting.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCutting.RowTemplate.Height = 24;
            this.gridCutting.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCutting.ShowCellToolTips = false;
            this.gridCutting.Size = new System.Drawing.Size(946, 452);
            this.gridCutting.TabIndex = 0;
            this.gridCutting.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.numCuttingQty);
            this.panel8.Controls.Add(this.numOrderQty);
            this.panel8.Controls.Add(this.label6);
            this.panel8.Controls.Add(this.label7);
            this.panel8.Controls.Add(this.label8);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(3, 455);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(946, 59);
            this.panel8.TabIndex = 0;
            // 
            // numCuttingQty
            // 
            this.numCuttingQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numCuttingQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numCuttingQty.IsSupportEditMode = false;
            this.numCuttingQty.Location = new System.Drawing.Point(271, 29);
            this.numCuttingQty.Name = "numCuttingQty";
            this.numCuttingQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numCuttingQty.ReadOnly = true;
            this.numCuttingQty.Size = new System.Drawing.Size(75, 23);
            this.numCuttingQty.TabIndex = 13;
            this.numCuttingQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(184, 29);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(75, 23);
            this.numOrderQty.TabIndex = 12;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(133, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Total";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.Color = System.Drawing.Color.Red;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(271, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 23);
            this.label7.TabIndex = 9;
            this.label7.Text = "Cutting Q\'ty";
            this.label7.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            this.label7.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label7.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(184, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 23);
            this.label8.TabIndex = 8;
            this.label8.Text = "Order Q\'ty";
            this.label8.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            this.label8.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label8.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 17);
            this.label2.TabIndex = 143;
            this.label2.Text = " ～";
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(289, 61);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 140;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(128, 61);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 139;
            // 
            // dateCreate
            // 
            // 
            // 
            // 
            this.dateCreate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCreate.DateBox1.Name = "";
            this.dateCreate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCreate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCreate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCreate.DateBox2.Name = "";
            this.dateCreate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCreate.DateBox2.TabIndex = 1;
            this.dateCreate.Location = new System.Drawing.Point(128, 33);
            this.dateCreate.Name = "dateCreate";
            this.dateCreate.Size = new System.Drawing.Size(280, 23);
            this.dateCreate.TabIndex = 138;
            // 
            // labSPNo
            // 
            this.labSPNo.Location = new System.Drawing.Point(14, 61);
            this.labSPNo.Name = "labSPNo";
            this.labSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labSPNo.Size = new System.Drawing.Size(111, 23);
            this.labSPNo.TabIndex = 142;
            this.labSPNo.Text = "SP#";
            this.labSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labCreateDate
            // 
            this.labCreateDate.Location = new System.Drawing.Point(14, 33);
            this.labCreateDate.Name = "labCreateDate";
            this.labCreateDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labCreateDate.RectStyle.BorderWidth = 1F;
            this.labCreateDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labCreateDate.RectStyle.ExtBorderWidth = 1F;
            this.labCreateDate.Size = new System.Drawing.Size(111, 23);
            this.labCreateDate.TabIndex = 141;
            this.labCreateDate.Text = "Create Date";
            this.labCreateDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labCreateDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelFunction
            // 
            this.labelFunction.Location = new System.Drawing.Point(14, 5);
            this.labelFunction.Name = "labelFunction";
            this.labelFunction.Size = new System.Drawing.Size(111, 23);
            this.labelFunction.TabIndex = 144;
            this.labelFunction.Text = "Function";
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboStockType.Location = new System.Drawing.Point(128, 5);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 145;
            // 
            // P99
            // 
            this.ClientSize = new System.Drawing.Size(960, 546);
            this.Controls.Add(this.tabControl1);
            this.Name = "P99";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P99. Send To WMS Command Status";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.tabControl1.ResumeLayout(false);
            this.UpdateCommand.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSewingOutput)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.UnLock.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCutting)).EndInit();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage UpdateCommand;
        private Win.UI.Panel panel7;
        private Win.UI.Grid gridSewingOutput;
        private Win.UI.Panel panel6;
        public System.Windows.Forms.TabPage UnLock;
        private Win.UI.Panel panel9;
        private Win.UI.Grid gridCutting;
        private Win.UI.Panel panel8;
        private Win.UI.NumericBox numCuttingQty;
        private Win.UI.NumericBox numOrderQty;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private System.Windows.Forms.Label label2;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.DateRange dateCreate;
        private Win.UI.Label labSPNo;
        private Win.UI.Label labCreateDate;
        private Win.UI.Label labelFunction;
        private Win.UI.ComboBox comboStockType;
    }
}
