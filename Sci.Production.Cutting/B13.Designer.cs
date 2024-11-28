
namespace Sci.Production.Cutting
{
    partial class B13
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
            this.label1 = new Sci.Win.UI.Label();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label2 = new Sci.Win.UI.Label();
            this.dateWorkingDate = new Sci.Win.UI.DateRange();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnBatchAssign = new Sci.Win.UI.Button();
            this.panel1 = new Sci.Win.UI.Panel();
            this.display2 = new Sci.Win.UI.DisplayBox();
            this.label7 = new Sci.Win.UI.Label();
            this.display1 = new Sci.Win.UI.DisplayBox();
            this.label6 = new Sci.Win.UI.Label();
            this.panel2 = new Sci.Win.UI.Panel();
            this.label4 = new Sci.Win.UI.Label();
            this.grid2 = new Sci.Win.UI.Grid();
            this.grid2bs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnSet = new Sci.Win.UI.Button();
            this.checkBoxIsSpecialTime = new Sci.Win.UI.CheckBox();
            this.checkBoxIsHoliday = new Sci.Win.UI.CheckBox();
            this.displayBoxWorkingHours = new Sci.Win.UI.DisplayBox();
            this.label5 = new Sci.Win.UI.Label();
            this.displayWorkingDate = new Sci.Win.UI.DateBox();
            this.displayBoxMachineID = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 28;
            this.label1.Text = "Machine Type";
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = false;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(103, 8);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 0;
            this.comboDropDownList1.Type = "PMS_CutMachIoTType";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(232, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 23);
            this.label2.TabIndex = 30;
            this.label2.Text = "Working Date";
            // 
            // dateWorkingDate
            // 
            // 
            // 
            // 
            this.dateWorkingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateWorkingDate.DateBox1.Name = "";
            this.dateWorkingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateWorkingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateWorkingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateWorkingDate.DateBox2.Name = "";
            this.dateWorkingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateWorkingDate.DateBox2.TabIndex = 1;
            this.dateWorkingDate.Location = new System.Drawing.Point(327, 9);
            this.dateWorkingDate.Name = "dateWorkingDate";
            this.dateWorkingDate.Size = new System.Drawing.Size(280, 23);
            this.dateWorkingDate.TabIndex = 1;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(705, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(89, 30);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnBatchAssign
            // 
            this.btnBatchAssign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchAssign.Location = new System.Drawing.Point(800, 5);
            this.btnBatchAssign.Name = "btnBatchAssign";
            this.btnBatchAssign.Size = new System.Drawing.Size(202, 30);
            this.btnBatchAssign.TabIndex = 3;
            this.btnBatchAssign.Text = "Batch Assign Special Time";
            this.btnBatchAssign.UseVisualStyleBackColor = true;
            this.btnBatchAssign.Click += new System.EventHandler(this.BtnBatchAssign_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.display2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.display1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBatchAssign);
            this.panel1.Controls.Add(this.comboDropDownList1);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateWorkingDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 64);
            this.panel1.TabIndex = 57;
            // 
            // display2
            // 
            this.display2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.display2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.display2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.display2.Location = new System.Drawing.Point(133, 39);
            this.display2.Name = "display2";
            this.display2.Size = new System.Drawing.Size(20, 21);
            this.display2.TabIndex = 210;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(156, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 23);
            this.label7.TabIndex = 211;
            this.label7.Text = "Working hours is 0";
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // display1
            // 
            this.display1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.display1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.display1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.display1.Location = new System.Drawing.Point(10, 39);
            this.display1.Name = "display1";
            this.display1.Size = new System.Drawing.Size(20, 21);
            this.display1.TabIndex = 208;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(33, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 23);
            this.label6.TabIndex = 209;
            this.label6.Text = "Is Special Time";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.grid2);
            this.panel2.Controls.Add(this.btnSet);
            this.panel2.Controls.Add(this.checkBoxIsSpecialTime);
            this.panel2.Controls.Add(this.checkBoxIsHoliday);
            this.panel2.Controls.Add(this.displayBoxWorkingHours);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.displayWorkingDate);
            this.panel2.Controls.Add(this.displayBoxMachineID);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(800, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(208, 597);
            this.panel2.TabIndex = 58;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 23);
            this.label4.TabIndex = 59;
            this.label4.Text = "Working Date";
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.grid2bs;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(9, 225);
            this.grid2.Name = "grid2";
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.ShowCellToolTips = false;
            this.grid2.Size = new System.Drawing.Size(196, 206);
            this.grid2.TabIndex = 58;
            this.grid2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Grid2_CellFormatting);
            // 
            // btnSet
            // 
            this.btnSet.Enabled = false;
            this.btnSet.Location = new System.Drawing.Point(137, 189);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(65, 30);
            this.btnSet.TabIndex = 57;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.BtnSet_Click);
            // 
            // checkBoxIsSpecialTime
            // 
            this.checkBoxIsSpecialTime.AutoSize = true;
            this.checkBoxIsSpecialTime.Enabled = false;
            this.checkBoxIsSpecialTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxIsSpecialTime.Location = new System.Drawing.Point(9, 195);
            this.checkBoxIsSpecialTime.Name = "checkBoxIsSpecialTime";
            this.checkBoxIsSpecialTime.Size = new System.Drawing.Size(122, 21);
            this.checkBoxIsSpecialTime.TabIndex = 36;
            this.checkBoxIsSpecialTime.Text = "Is Special Time";
            this.checkBoxIsSpecialTime.UseVisualStyleBackColor = true;
            this.checkBoxIsSpecialTime.CheckedChanged += new System.EventHandler(this.CheckBoxIsSpecialTime_CheckedChanged);
            this.checkBoxIsSpecialTime.Click += new System.EventHandler(this.CheckBoxIsSpecialTime_Click);
            // 
            // checkBoxIsHoliday
            // 
            this.checkBoxIsHoliday.AutoSize = true;
            this.checkBoxIsHoliday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBoxIsHoliday.IsSupportEditMode = false;
            this.checkBoxIsHoliday.Location = new System.Drawing.Point(9, 168);
            this.checkBoxIsHoliday.Name = "checkBoxIsHoliday";
            this.checkBoxIsHoliday.ReadOnly = true;
            this.checkBoxIsHoliday.Size = new System.Drawing.Size(88, 21);
            this.checkBoxIsHoliday.TabIndex = 35;
            this.checkBoxIsHoliday.Text = "Is Holiday";
            this.checkBoxIsHoliday.UseVisualStyleBackColor = true;
            // 
            // displayBoxWorkingHours
            // 
            this.displayBoxWorkingHours.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxWorkingHours.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxWorkingHours.Location = new System.Drawing.Point(9, 139);
            this.displayBoxWorkingHours.Name = "displayBoxWorkingHours";
            this.displayBoxWorkingHours.Size = new System.Drawing.Size(129, 23);
            this.displayBoxWorkingHours.TabIndex = 34;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 23);
            this.label5.TabIndex = 33;
            this.label5.Text = "Working Hours";
            // 
            // displayWorkingDate
            // 
            this.displayWorkingDate.IsSupportEditMode = false;
            this.displayWorkingDate.Location = new System.Drawing.Point(9, 87);
            this.displayWorkingDate.Name = "displayWorkingDate";
            this.displayWorkingDate.ReadOnly = true;
            this.displayWorkingDate.Size = new System.Drawing.Size(130, 23);
            this.displayWorkingDate.TabIndex = 32;
            // 
            // displayBoxMachineID
            // 
            this.displayBoxMachineID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxMachineID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxMachineID.Location = new System.Drawing.Point(9, 35);
            this.displayBoxMachineID.Name = "displayBoxMachineID";
            this.displayBoxMachineID.Size = new System.Drawing.Size(129, 23);
            this.displayBoxMachineID.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 23);
            this.label3.TabIndex = 29;
            this.label3.Text = "Machine ID";
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
            this.grid1.Location = new System.Drawing.Point(0, 64);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(800, 597);
            this.grid1.TabIndex = 4;
            this.grid1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.Grid1_CellFormatting);
            this.grid1.SelectionChanged += new System.EventHandler(this.Grid1_SelectionChanged);
            // 
            // B13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B13";
            this.OnLineHelpID = "";
            this.Text = "B13. Machine Working Calendar";
            this.WorkAlias = "MachineIoT";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.Label label2;
        private Win.UI.DateRange dateWorkingDate;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnBatchAssign;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnSet;
        private Win.UI.CheckBox checkBoxIsSpecialTime;
        private Win.UI.CheckBox checkBoxIsHoliday;
        private Win.UI.DisplayBox displayBoxWorkingHours;
        private Win.UI.Label label5;
        private Win.UI.DateBox displayWorkingDate;
        private Win.UI.DisplayBox displayBoxMachineID;
        private Win.UI.Label label3;
        private Win.UI.Grid grid2;
        private Win.UI.Grid grid1;
        private Win.UI.Label label4;
        private Win.UI.ListControlBindingSource grid2bs;
        private Win.UI.DisplayBox display1;
        private Win.UI.Label label6;
        private Win.UI.DisplayBox display2;
        private Win.UI.Label label7;
    }
}