namespace Sci.Production.Logistic
{
    partial class P02_ImportFromBarCode
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource();
            this.button3 = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.button1 = new Sci.Win.UI.Button();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource();
            this.panel2 = new Sci.Win.UI.Panel();
            this.grid2 = new Sci.Win.UI.Grid();
            this.button6 = new Sci.Win.UI.Button();
            this.button5 = new Sci.Win.UI.Button();
            this.button4 = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.label3 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.label1 = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(7, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(698, 101);
            this.panel1.TabIndex = 0;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.AutoGenerateColumns = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(698, 101);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button3.Location = new System.Drawing.Point(585, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 30);
            this.button3.TabIndex = 3;
            this.button3.Text = "Import Data";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(94, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(7, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Append";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 1;
            this.lineShape2.X2 = 704;
            this.lineShape2.Y1 = 37;
            this.lineShape2.Y2 = 37;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(7, 204);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(698, 236);
            this.panel2.TabIndex = 1;
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.AutoGenerateColumns = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(0, 0);
            this.grid2.Name = "grid2";
            this.grid2.RowHeadersVisible = false;
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.Size = new System.Drawing.Size(698, 236);
            this.grid2.TabIndex = 2;
            this.grid2.TabStop = false;
            // 
            // button6
            // 
            this.button6.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button6.Location = new System.Drawing.Point(613, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(80, 30);
            this.button6.TabIndex = 7;
            this.button6.Text = "Cancel";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button5.Location = new System.Drawing.Point(519, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(80, 30);
            this.button5.TabIndex = 6;
            this.button5.Text = "Save";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button4.Location = new System.Drawing.Point(350, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 30);
            this.button4.TabIndex = 5;
            this.button4.Text = "To Excel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(7, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total Carton Scanned";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBox1.IsSupportEditMode = false;
            this.numericBox1.Location = new System.Drawing.Point(148, 7);
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.ReadOnly = true;
            this.numericBox1.Size = new System.Drawing.Size(100, 23);
            this.numericBox1.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "File List";
            // 
            // dateBox1
            // 
            this.dateBox1.Location = new System.Drawing.Point(99, 43);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(7, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Receive Date";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.numericBox1);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.button6);
            this.panel3.Controls.Add(this.button5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(7, 440);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(698, 39);
            this.panel3.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.dateBox1);
            this.panel4.Controls.Add(this.button1);
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.shapeContainer1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(7, 131);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(698, 73);
            this.panel4.TabIndex = 10;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape2});
            this.shapeContainer1.Size = new System.Drawing.Size(698, 73);
            this.shapeContainer1.TabIndex = 4;
            this.shapeContainer1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(7, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(698, 30);
            this.panel5.TabIndex = 11;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(7, 479);
            this.panel6.TabIndex = 6;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(705, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(7, 479);
            this.panel7.TabIndex = 12;
            // 
            // P02_ImportFromBarCode
            // 
            this.ClientSize = new System.Drawing.Size(712, 479);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel7);
            this.Name = "P02_ImportFromBarCode";
            this.Text = "Import From Bar Code";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Button button3;
        private Win.UI.Button button2;
        private Win.UI.Button button1;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel2;
        private Win.UI.Button button6;
        private Win.UI.Button button5;
        private Win.UI.Button button4;
        private Win.UI.Label label2;
        private Win.UI.Grid grid2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.Label label3;
        private Win.UI.DateBox dateBox1;
        private Win.UI.Label label1;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel6;
        private Win.UI.Panel panel7;
    }
}
