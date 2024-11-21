namespace Sci.Production.Class.PublicForm
{
    partial class MachineEditCalendar
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
            this.displayBoxMachineID = new Sci.Win.UI.DisplayBox();
            this.dateStart = new Sci.Win.UI.DateBox();
            this.label4 = new Sci.Win.UI.Label();
            this.calendarGrid1 = new Sci.Production.Class.Controls.CalendarGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.displayCrossday = new Sci.Win.UI.DisplayBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.comboBoxImportMachineID = new Sci.Win.UI.ComboBox();
            this.btnImport = new Sci.Win.UI.Button();
            this.dateImportStartDate = new Sci.Win.UI.DateBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Machine ID";
            // 
            // displayBoxMachineID
            // 
            this.displayBoxMachineID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxMachineID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxMachineID.Location = new System.Drawing.Point(93, 9);
            this.displayBoxMachineID.Name = "displayBoxMachineID";
            this.displayBoxMachineID.Size = new System.Drawing.Size(100, 23);
            this.displayBoxMachineID.TabIndex = 2;
            // 
            // dateStart
            // 
            this.dateStart.Location = new System.Drawing.Point(291, 9);
            this.dateStart.Name = "dateStart";
            this.dateStart.Size = new System.Drawing.Size(130, 23);
            this.dateStart.TabIndex = 59;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(196, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 23);
            this.label4.TabIndex = 58;
            this.label4.Text = "Start Date";
            // 
            // calendarGrid1
            // 
            this.calendarGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calendarGrid1.ColorCrossDay = System.Drawing.Color.Pink;
            this.calendarGrid1.EnableHearder = true;
            this.calendarGrid1.Location = new System.Drawing.Point(9, 49);
            this.calendarGrid1.Margin = new System.Windows.Forms.Padding(4);
            this.calendarGrid1.Name = "calendarGrid1";
            this.calendarGrid1.Size = new System.Drawing.Size(963, 322);
            this.calendarGrid1.TabIndex = 62;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 380);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 17);
            this.label3.TabIndex = 64;
            this.label3.Text = "Cross-day";
            // 
            // displayCrossday
            // 
            this.displayCrossday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.displayCrossday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCrossday.Enabled = false;
            this.displayCrossday.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayCrossday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCrossday.Location = new System.Drawing.Point(9, 378);
            this.displayCrossday.Name = "displayCrossday";
            this.displayCrossday.Size = new System.Drawing.Size(20, 21);
            this.displayCrossday.TabIndex = 63;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.comboBoxImportMachineID);
            this.groupBox1.Controls.Add(this.btnImport);
            this.groupBox1.Controls.Add(this.dateImportStartDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(9, 405);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(590, 58);
            this.groupBox1.TabIndex = 65;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy Calendar";
            // 
            // comboBoxImportMachineID
            // 
            this.comboBoxImportMachineID.BackColor = System.Drawing.Color.White;
            this.comboBoxImportMachineID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxImportMachineID.FormattingEnabled = true;
            this.comboBoxImportMachineID.IsSupportUnselect = true;
            this.comboBoxImportMachineID.Location = new System.Drawing.Point(101, 23);
            this.comboBoxImportMachineID.Name = "comboBoxImportMachineID";
            this.comboBoxImportMachineID.OldText = "";
            this.comboBoxImportMachineID.Size = new System.Drawing.Size(121, 24);
            this.comboBoxImportMachineID.TabIndex = 67;
            // 
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Location = new System.Drawing.Point(456, 20);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(111, 30);
            this.btnImport.TabIndex = 66;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // dateImportStartDate
            // 
            this.dateImportStartDate.Location = new System.Drawing.Point(320, 24);
            this.dateImportStartDate.Name = "dateImportStartDate";
            this.dateImportStartDate.Size = new System.Drawing.Size(130, 23);
            this.dateImportStartDate.TabIndex = 63;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(225, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 23);
            this.label2.TabIndex = 62;
            this.label2.Text = "Start Date";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(17, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 23);
            this.label5.TabIndex = 60;
            this.label5.Text = "Machine ID";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnSave.Location = new System.Drawing.Point(744, 421);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(103, 30);
            this.btnSave.TabIndex = 66;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnClose.Location = new System.Drawing.Point(853, 421);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(118, 30);
            this.btnClose.TabIndex = 67;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // MachineEditCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 467);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.displayCrossday);
            this.Controls.Add(this.calendarGrid1);
            this.Controls.Add(this.dateStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.displayBoxMachineID);
            this.Controls.Add(this.label1);
            this.EditMode = true;
            this.Name = "MachineEditCalendar";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Edit Calendar";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.displayBoxMachineID, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateStart, 0);
            this.Controls.SetChildIndex(this.calendarGrid1, 0);
            this.Controls.SetChildIndex(this.displayCrossday, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayBoxMachineID;
        private Win.UI.DateBox dateStart;
        private Win.UI.Label label4;
        private Controls.CalendarGrid calendarGrid1;
        private System.Windows.Forms.Label label3;
        private Win.UI.DisplayBox displayCrossday;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnImport;
        private Win.UI.DateBox dateImportStartDate;
        private Win.UI.Label label2;
        private Win.UI.Label label5;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.ComboBox comboBoxImportMachineID;
    }
}