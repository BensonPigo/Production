namespace Sci.Production.Quality
{
    partial class B33
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
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.dateStartDate = new Sci.Win.UI.DateBox();
            this.txtRest1Start = new Sci.Win.UI.TextBox();
            this.txtRest1End = new Sci.Win.UI.TextBox();
            this.txtRest2Start = new Sci.Win.UI.TextBox();
            this.txtRest2End = new Sci.Win.UI.TextBox();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtsubprocess = new Sci.Production.Class.Txtsubprocess();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(884, 348);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtsubprocess);
            this.detailcont.Controls.Add(this.txtRest2End);
            this.detailcont.Controls.Add(this.txtRest2Start);
            this.detailcont.Controls.Add(this.txtRest1End);
            this.detailcont.Controls.Add(this.txtRest1Start);
            this.detailcont.Controls.Add(this.dateStartDate);
            this.detailcont.Controls.Add(this.comboShift);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.txtfactory);
            this.detailcont.Size = new System.Drawing.Size(884, 310);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 310);
            this.detailbtm.Size = new System.Drawing.Size(884, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(884, 348);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(892, 377);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(29, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(256, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Shift";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(29, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Start Date";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(256, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Subprocess";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(29, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Rest1 Start";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(256, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Rest1 End";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(29, 127);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Rest2 Start";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(256, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Rest2 End";
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(469, 20);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 8;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Shift", true));
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(342, 20);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(121, 24);
            this.comboShift.TabIndex = 10;
            // 
            // dateStartDate
            // 
            this.dateStartDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StartDate", true));
            this.dateStartDate.Location = new System.Drawing.Point(107, 56);
            this.dateStartDate.Name = "dateStartDate";
            this.dateStartDate.Size = new System.Drawing.Size(130, 23);
            this.dateStartDate.TabIndex = 15;
            // 
            // txtRest1Start
            // 
            this.txtRest1Start.BackColor = System.Drawing.Color.White;
            this.txtRest1Start.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Rest1Start", true));
            this.txtRest1Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRest1Start.Location = new System.Drawing.Point(107, 92);
            this.txtRest1Start.Mask = "90:00";
            this.txtRest1Start.Name = "txtRest1Start";
            this.txtRest1Start.Size = new System.Drawing.Size(53, 23);
            this.txtRest1Start.TabIndex = 16;
            // 
            // txtRest1End
            // 
            this.txtRest1End.BackColor = System.Drawing.Color.White;
            this.txtRest1End.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Rest1End", true));
            this.txtRest1End.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRest1End.Location = new System.Drawing.Point(342, 92);
            this.txtRest1End.Mask = "90:00";
            this.txtRest1End.Name = "txtRest1End";
            this.txtRest1End.Size = new System.Drawing.Size(53, 23);
            this.txtRest1End.TabIndex = 17;
            // 
            // txtRest2Start
            // 
            this.txtRest2Start.BackColor = System.Drawing.Color.White;
            this.txtRest2Start.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Rest2Start", true));
            this.txtRest2Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRest2Start.Location = new System.Drawing.Point(107, 127);
            this.txtRest2Start.Mask = "90:00";
            this.txtRest2Start.Name = "txtRest2Start";
            this.txtRest2Start.Size = new System.Drawing.Size(53, 23);
            this.txtRest2Start.TabIndex = 18;
            // 
            // txtRest2End
            // 
            this.txtRest2End.BackColor = System.Drawing.Color.White;
            this.txtRest2End.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Rest2End", true));
            this.txtRest2End.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRest2End.Location = new System.Drawing.Point(342, 127);
            this.txtRest2End.Mask = "90:00";
            this.txtRest2End.Name = "txtRest2End";
            this.txtRest2End.Size = new System.Drawing.Size(53, 23);
            this.txtRest2End.TabIndex = 19;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = true;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(107, 20);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 9;
            // 
            // txtsubprocess
            // 
            this.txtsubprocess.BackColor = System.Drawing.Color.White;
            this.txtsubprocess.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubprocessID", true));
            this.txtsubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsubprocess.IsRFIDProcess = false;
            this.txtsubprocess.Location = new System.Drawing.Point(342, 56);
            this.txtsubprocess.MultiSelect = false;
            this.txtsubprocess.Name = "txtsubprocess";
            this.txtsubprocess.Size = new System.Drawing.Size(122, 23);
            this.txtsubprocess.TabIndex = 20;
            // 
            // B33
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 410);
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B33";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B33. Sub-Process Break Time Setting";
            this.WorkAlias = "SubProRestTime";
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

        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.CheckBox chkJunk;
        private Class.Txtfactory txtfactory;
        private Win.UI.ComboBox comboShift;
        private Win.UI.DateBox dateStartDate;
        private Win.UI.TextBox txtRest2End;
        private Win.UI.TextBox txtRest2Start;
        private Win.UI.TextBox txtRest1End;
        private Win.UI.TextBox txtRest1Start;
        private Class.Txtsubprocess txtsubprocess;
    }
}