namespace Sci.Production.Cutting
{
    partial class P02_BatchAssignCellCutDate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid1 = new Sci.Win.UI.Grid();
            this.button1 = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.SP_textbox = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.article_textbox = new Sci.Win.UI.TextBox();
            this.cutno_numericbox = new Sci.Win.UI.NumericBox();
            this.sizecode_textbox = new Sci.Win.UI.TextBox();
            this.estcutdate_textbox1 = new Sci.Win.UI.DateBox();
            this.fabriccombo_textbox = new Sci.Win.UI.TextBox();
            this.markername_textbox = new Sci.Win.UI.TextBox();
            this.only_checkBox = new Sci.Win.UI.CheckBox();
            this.filter_button = new Sci.Win.UI.Button();
            this.label9 = new Sci.Win.UI.Label();
            this.estcutdate_textbox2 = new Sci.Win.UI.DateBox();
            this.label10 = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.batchcutcell_button = new Sci.Win.UI.Button();
            this.batchestcutdate_button = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.txtCell2 = new Sci.Production.Class.txtCell();
            this.txtcell = new Sci.Production.Class.txtCell();
            this.btn_Confirm = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
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
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(6, 123);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(996, 338);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            this.grid1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid1_CellContentClick);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(916, 467);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "SP#";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Article";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(197, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "SizeCode";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(197, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Cut#";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(359, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "FabricCombo";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(359, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 23);
            this.label6.TabIndex = 6;
            this.label6.Text = "Est. Cut Date";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(517, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "Marker Name";
            // 
            // SP_textbox
            // 
            this.SP_textbox.BackColor = System.Drawing.Color.White;
            this.SP_textbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.SP_textbox.Location = new System.Drawing.Point(80, 9);
            this.SP_textbox.Name = "SP_textbox";
            this.SP_textbox.Size = new System.Drawing.Size(108, 23);
            this.SP_textbox.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(598, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 23);
            this.label8.TabIndex = 9;
            this.label8.Text = "Cut Cell";
            // 
            // article_textbox
            // 
            this.article_textbox.BackColor = System.Drawing.Color.White;
            this.article_textbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.article_textbox.Location = new System.Drawing.Point(80, 46);
            this.article_textbox.Name = "article_textbox";
            this.article_textbox.Size = new System.Drawing.Size(78, 23);
            this.article_textbox.TabIndex = 1;
            // 
            // cutno_numericbox
            // 
            this.cutno_numericbox.BackColor = System.Drawing.Color.White;
            this.cutno_numericbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cutno_numericbox.Location = new System.Drawing.Point(273, 9);
            this.cutno_numericbox.Name = "cutno_numericbox";
            this.cutno_numericbox.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.cutno_numericbox.Size = new System.Drawing.Size(45, 23);
            this.cutno_numericbox.TabIndex = 2;
            this.cutno_numericbox.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // sizecode_textbox
            // 
            this.sizecode_textbox.BackColor = System.Drawing.Color.White;
            this.sizecode_textbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.sizecode_textbox.Location = new System.Drawing.Point(273, 46);
            this.sizecode_textbox.Name = "sizecode_textbox";
            this.sizecode_textbox.Size = new System.Drawing.Size(78, 23);
            this.sizecode_textbox.TabIndex = 3;
            // 
            // estcutdate_textbox1
            // 
            this.estcutdate_textbox1.Location = new System.Drawing.Point(450, 46);
            this.estcutdate_textbox1.Name = "estcutdate_textbox1";
            this.estcutdate_textbox1.Size = new System.Drawing.Size(130, 23);
            this.estcutdate_textbox1.TabIndex = 5;
            // 
            // fabriccombo_textbox
            // 
            this.fabriccombo_textbox.BackColor = System.Drawing.Color.White;
            this.fabriccombo_textbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.fabriccombo_textbox.Location = new System.Drawing.Point(450, 9);
            this.fabriccombo_textbox.Name = "fabriccombo_textbox";
            this.fabriccombo_textbox.Size = new System.Drawing.Size(44, 23);
            this.fabriccombo_textbox.TabIndex = 4;
            // 
            // markername_textbox
            // 
            this.markername_textbox.BackColor = System.Drawing.Color.White;
            this.markername_textbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.markername_textbox.Location = new System.Drawing.Point(612, 9);
            this.markername_textbox.Name = "markername_textbox";
            this.markername_textbox.Size = new System.Drawing.Size(60, 23);
            this.markername_textbox.TabIndex = 6;
            // 
            // only_checkBox
            // 
            this.only_checkBox.AutoSize = true;
            this.only_checkBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.only_checkBox.Location = new System.Drawing.Point(729, 48);
            this.only_checkBox.Name = "only_checkBox";
            this.only_checkBox.Size = new System.Drawing.Size(221, 21);
            this.only_checkBox.TabIndex = 8;
            this.only_checkBox.Text = "Only show empty Est. Cut Date";
            this.only_checkBox.UseVisualStyleBackColor = true;
            // 
            // filter_button
            // 
            this.filter_button.Location = new System.Drawing.Point(693, 9);
            this.filter_button.Name = "filter_button";
            this.filter_button.Size = new System.Drawing.Size(80, 30);
            this.filter_button.TabIndex = 9;
            this.filter_button.Text = "Filter";
            this.filter_button.UseVisualStyleBackColor = true;
            this.filter_button.Click += new System.EventHandler(this.filter_button_Click);
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(12, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 23);
            this.label9.TabIndex = 19;
            this.label9.Text = "Cut Cell";
            // 
            // estcutdate_textbox2
            // 
            this.estcutdate_textbox2.Location = new System.Drawing.Point(450, 87);
            this.estcutdate_textbox2.Name = "estcutdate_textbox2";
            this.estcutdate_textbox2.Size = new System.Drawing.Size(130, 23);
            this.estcutdate_textbox2.TabIndex = 12;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(359, 87);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 23);
            this.label10.TabIndex = 21;
            this.label10.Text = "Est. Cut Date";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(6, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(996, 75);
            this.panel1.TabIndex = 23;
            // 
            // batchcutcell_button
            // 
            this.batchcutcell_button.Location = new System.Drawing.Point(143, 83);
            this.batchcutcell_button.Name = "batchcutcell_button";
            this.batchcutcell_button.Size = new System.Drawing.Size(175, 30);
            this.batchcutcell_button.TabIndex = 11;
            this.batchcutcell_button.Text = "Batch update Cut Cell";
            this.batchcutcell_button.UseVisualStyleBackColor = true;
            this.batchcutcell_button.Click += new System.EventHandler(this.batchcutcell_button_Click);
            // 
            // batchestcutdate_button
            // 
            this.batchestcutdate_button.Location = new System.Drawing.Point(586, 83);
            this.batchestcutdate_button.Name = "batchestcutdate_button";
            this.batchestcutdate_button.Size = new System.Drawing.Size(199, 30);
            this.batchestcutdate_button.TabIndex = 13;
            this.batchestcutdate_button.Text = "Batch update Est. Cut Date";
            this.batchestcutdate_button.UseVisualStyleBackColor = true;
            this.batchestcutdate_button.Click += new System.EventHandler(this.batchestcutdate_button_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(1010, 503);
            this.shapeContainer1.TabIndex = 24;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 2;
            this.lineShape1.X2 = 1004;
            this.lineShape1.Y1 = 79;
            this.lineShape1.Y2 = 78;
            // 
            // txtCell2
            // 
            this.txtCell2.BackColor = System.Drawing.Color.White;
            this.txtCell2.FactoryId = "";
            this.txtCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell2.Location = new System.Drawing.Point(107, 87);
            this.txtCell2.Name = "txtCell2";
            this.txtCell2.Size = new System.Drawing.Size(30, 23);
            this.txtCell2.TabIndex = 10;
            // 
            // txtcell
            // 
            this.txtcell.BackColor = System.Drawing.Color.White;
            this.txtcell.FactoryId = "";
            this.txtcell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcell.Location = new System.Drawing.Point(693, 46);
            this.txtcell.Name = "txtcell";
            this.txtcell.Size = new System.Drawing.Size(30, 23);
            this.txtcell.TabIndex = 7;
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Confirm.AutoSize = true;
            this.btn_Confirm.Location = new System.Drawing.Point(819, 467);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(80, 30);
            this.btn_Confirm.TabIndex = 25;
            this.btn_Confirm.Text = "Confirm";
            this.btn_Confirm.UseVisualStyleBackColor = true;
            this.btn_Confirm.Click += new System.EventHandler(this.btn_Confirm_Click);
            // 
            // P02_BatchAssignCellCutDate
            // 
            this.ClientSize = new System.Drawing.Size(1010, 503);
            this.Controls.Add(this.btn_Confirm);
            this.Controls.Add(this.batchestcutdate_button);
            this.Controls.Add(this.batchcutcell_button);
            this.Controls.Add(this.estcutdate_textbox2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtCell2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.filter_button);
            this.Controls.Add(this.only_checkBox);
            this.Controls.Add(this.markername_textbox);
            this.Controls.Add(this.fabriccombo_textbox);
            this.Controls.Add(this.estcutdate_textbox1);
            this.Controls.Add(this.sizecode_textbox);
            this.Controls.Add(this.cutno_numericbox);
            this.Controls.Add(this.txtcell);
            this.Controls.Add(this.article_textbox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.SP_textbox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "P02_BatchAssignCellCutDate";
            this.Text = "Batch Assign Cell/Est. Cut Date";
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid grid1;
        private Win.UI.Button button1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.TextBox SP_textbox;
        private Win.UI.Label label8;
        private Win.UI.TextBox article_textbox;
        private Class.txtCell txtcell;
        private Win.UI.NumericBox cutno_numericbox;
        private Win.UI.TextBox sizecode_textbox;
        private Win.UI.DateBox estcutdate_textbox1;
        private Win.UI.TextBox fabriccombo_textbox;
        private Win.UI.TextBox markername_textbox;
        private Win.UI.CheckBox only_checkBox;
        private Win.UI.Button filter_button;
        private Class.txtCell txtCell2;
        private Win.UI.Label label9;
        private Win.UI.DateBox estcutdate_textbox2;
        private Win.UI.Label label10;
        private Win.UI.Panel panel1;
        private Win.UI.Button batchcutcell_button;
        private Win.UI.Button batchestcutdate_button;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btn_Confirm;
    }
}
