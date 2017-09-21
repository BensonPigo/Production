namespace RFID_Monitor_Board
{
    partial class RFID_Monitor_Board
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.lbPages = new System.Windows.Forms.Label();
            this.txtMorningOutput = new Sci.Win.UI.TextBox();
            this.txtNightOutput = new Sci.Win.UI.TextBox();
            this.txtPreviousDateTotalOutput = new Sci.Win.UI.TextBox();
            this.txtPreviousDateWIP = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.txtSunProcess = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Win.UI.TextBox();
            this.txtDaterange = new Sci.Win.UI.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Location = new System.Drawing.Point(269, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 730);
            this.panel1.TabIndex = 1;
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
            this.grid1.RowTemplate.Height = 42;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(739, 730);
            this.grid1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(9, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Date range";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label2.Location = new System.Drawing.Point(9, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Morning Output";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label3.Location = new System.Drawing.Point(9, 325);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(257, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Night Output";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label4.Location = new System.Drawing.Point(9, 404);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(257, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Previous Date Total Output";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label5.Location = new System.Drawing.Point(9, 483);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(257, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "Previous Date WIP";
            // 
            // lbPages
            // 
            this.lbPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbPages.AutoSize = true;
            this.lbPages.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.lbPages.Location = new System.Drawing.Point(177, 692);
            this.lbPages.Name = "lbPages";
            this.lbPages.Size = new System.Drawing.Size(58, 29);
            this.lbPages.TabIndex = 1;
            this.lbPages.Text = "1 / 1";
            this.lbPages.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMorningOutput
            // 
            this.txtMorningOutput.BackColor = System.Drawing.Color.White;
            this.txtMorningOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtMorningOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMorningOutput.Location = new System.Drawing.Point(9, 272);
            this.txtMorningOutput.Name = "txtMorningOutput";
            this.txtMorningOutput.Size = new System.Drawing.Size(254, 30);
            this.txtMorningOutput.TabIndex = 3;
            this.txtMorningOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNightOutput
            // 
            this.txtNightOutput.BackColor = System.Drawing.Color.White;
            this.txtNightOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtNightOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNightOutput.Location = new System.Drawing.Point(9, 351);
            this.txtNightOutput.Name = "txtNightOutput";
            this.txtNightOutput.Size = new System.Drawing.Size(254, 30);
            this.txtNightOutput.TabIndex = 4;
            this.txtNightOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPreviousDateTotalOutput
            // 
            this.txtPreviousDateTotalOutput.BackColor = System.Drawing.Color.White;
            this.txtPreviousDateTotalOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtPreviousDateTotalOutput.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPreviousDateTotalOutput.Location = new System.Drawing.Point(9, 430);
            this.txtPreviousDateTotalOutput.Name = "txtPreviousDateTotalOutput";
            this.txtPreviousDateTotalOutput.Size = new System.Drawing.Size(254, 30);
            this.txtPreviousDateTotalOutput.TabIndex = 5;
            this.txtPreviousDateTotalOutput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPreviousDateWIP
            // 
            this.txtPreviousDateWIP.BackColor = System.Drawing.Color.White;
            this.txtPreviousDateWIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtPreviousDateWIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPreviousDateWIP.Location = new System.Drawing.Point(9, 509);
            this.txtPreviousDateWIP.Name = "txtPreviousDateWIP";
            this.txtPreviousDateWIP.Size = new System.Drawing.Size(254, 30);
            this.txtPreviousDateWIP.TabIndex = 6;
            this.txtPreviousDateWIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label7.Location = new System.Drawing.Point(9, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(257, 23);
            this.label7.TabIndex = 14;
            this.label7.Text = "Factory";
            // 
            // txtSunProcess
            // 
            this.txtSunProcess.BackColor = System.Drawing.Color.White;
            this.txtSunProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtSunProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSunProcess.Location = new System.Drawing.Point(9, 114);
            this.txtSunProcess.Name = "txtSunProcess";
            this.txtSunProcess.Size = new System.Drawing.Size(254, 30);
            this.txtSunProcess.TabIndex = 1;
            this.txtSunProcess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label8.Location = new System.Drawing.Point(9, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(257, 23);
            this.label8.TabIndex = 16;
            this.label8.Text = "SubProcess";
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(9, 35);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(254, 30);
            this.txtFactory.TabIndex = 0;
            this.txtFactory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDaterange
            // 
            this.txtDaterange.BackColor = System.Drawing.Color.White;
            this.txtDaterange.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtDaterange.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDaterange.Location = new System.Drawing.Point(9, 193);
            this.txtDaterange.Name = "txtDaterange";
            this.txtDaterange.Size = new System.Drawing.Size(254, 30);
            this.txtDaterange.TabIndex = 1;
            this.txtDaterange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 10000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // RFID_Monitor_Board
            // 
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.txtDaterange);
            this.Controls.Add(this.txtSunProcess);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtPreviousDateWIP);
            this.Controls.Add(this.txtPreviousDateTotalOutput);
            this.Controls.Add(this.txtNightOutput);
            this.Controls.Add(this.txtMorningOutput);
            this.Controls.Add(this.lbPages);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "RFID_Monitor_Board";
            this.Text = "RFID_Monitor_Board";
            this.Load += new System.EventHandler(this.RFID_Monitor_Board_Load);
            this.Resize += new System.EventHandler(this.RFID_Monitor_Board_Resize);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.lbPages, 0);
            this.Controls.SetChildIndex(this.txtMorningOutput, 0);
            this.Controls.SetChildIndex(this.txtNightOutput, 0);
            this.Controls.SetChildIndex(this.txtPreviousDateTotalOutput, 0);
            this.Controls.SetChildIndex(this.txtPreviousDateWIP, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txtSunProcess, 0);
            this.Controls.SetChildIndex(this.txtDaterange, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.Panel panel1;
        private Sci.Win.UI.Grid grid1;
        private Sci.Win.UI.ListControlBindingSource listControlBindingSource1;
        private Sci.Win.UI.Label label1;
        private Sci.Win.UI.Label label2;
        private Sci.Win.UI.Label label3;
        private Sci.Win.UI.Label label4;
        private Sci.Win.UI.Label label5;
        private System.Windows.Forms.Label lbPages;
        private Sci.Win.UI.TextBox txtMorningOutput;
        private Sci.Win.UI.TextBox txtNightOutput;
        private Sci.Win.UI.TextBox txtPreviousDateTotalOutput;
        private Sci.Win.UI.TextBox txtPreviousDateWIP;
        private Sci.Win.UI.Label label7;
        private Sci.Win.UI.TextBox txtSunProcess;
        private Sci.Win.UI.Label label8;
        private Sci.Win.UI.TextBox txtFactory;
        private Sci.Win.UI.TextBox txtDaterange;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}
