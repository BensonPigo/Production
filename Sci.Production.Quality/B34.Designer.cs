namespace Sci.Production.Quality
{
    partial class B34
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
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.CcAddress = new Sci.Win.UI.EditBox();
            this.ToAddress = new Sci.Win.UI.EditBox();
            this.txtStandard = new Sci.Win.UI.TextBox();
            this.Description = new Sci.Win.UI.EditBox();
            this.comboFrequency = new Sci.Win.UI.ComboBox();
            this.txtEndTime = new Sci.Win.UI.TextBox();
            this.txtStartTime = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtEndTime);
            this.detailcont.Controls.Add(this.txtStartTime);
            this.detailcont.Controls.Add(this.comboFrequency);
            this.detailcont.Controls.Add(this.Description);
            this.detailcont.Controls.Add(this.txtStandard);
            this.detailcont.Controls.Add(this.ToAddress);
            this.detailcont.Controls.Add(this.CcAddress);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.txtfactory);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(894, 407);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(902, 436);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(31, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "CC Address";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(31, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "To Address";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(31, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Standard";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(31, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Factory";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(647, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 23);
            this.label5.TabIndex = 11;
            this.label5.Text = "Frequency";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(31, 262);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 23);
            this.label8.TabIndex = 8;
            this.label8.Text = "Description";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsIE = false;
            this.txtfactory.IsMultiselect = false;
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(120, 30);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.NeedInitialFactory = false;
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(647, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 23);
            this.label6.TabIndex = 17;
            this.label6.Text = "End Time";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(647, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 23);
            this.label7.TabIndex = 16;
            this.label7.Text = "Start Time";
            // 
            // CcAddress
            // 
            this.CcAddress.BackColor = System.Drawing.Color.White;
            this.CcAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CcAddress", true));
            this.CcAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CcAddress.Location = new System.Drawing.Point(120, 177);
            this.CcAddress.Multiline = true;
            this.CcAddress.Name = "CcAddress";
            this.CcAddress.Size = new System.Drawing.Size(465, 77);
            this.CcAddress.TabIndex = 21;
            // 
            // ToAddress
            // 
            this.ToAddress.BackColor = System.Drawing.Color.White;
            this.ToAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ToAddress", true));
            this.ToAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ToAddress.Location = new System.Drawing.Point(120, 94);
            this.ToAddress.Multiline = true;
            this.ToAddress.Name = "ToAddress";
            this.ToAddress.Size = new System.Drawing.Size(465, 77);
            this.ToAddress.TabIndex = 22;
            // 
            // txtStandard
            // 
            this.txtStandard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStandard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStandard.IsSupportEditMode = false;
            this.txtStandard.Location = new System.Drawing.Point(120, 62);
            this.txtStandard.Name = "txtStandard";
            this.txtStandard.ReadOnly = true;
            this.txtStandard.Size = new System.Drawing.Size(465, 23);
            this.txtStandard.TabIndex = 23;
            this.txtStandard.Text = "1 day before the buyer delivery date; 3 day before the buyer delivery date";
            // 
            // Description
            // 
            this.Description.BackColor = System.Drawing.Color.White;
            this.Description.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.Description.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Description.Location = new System.Drawing.Point(120, 262);
            this.Description.Multiline = true;
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(465, 77);
            this.Description.TabIndex = 24;
            // 
            // comboFrequency
            // 
            this.comboFrequency.BackColor = System.Drawing.Color.White;
            this.comboFrequency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Frequency", true));
            this.comboFrequency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFrequency.FormattingEnabled = true;
            this.comboFrequency.IsSupportUnselect = true;
            this.comboFrequency.Location = new System.Drawing.Point(736, 93);
            this.comboFrequency.Name = "comboFrequency";
            this.comboFrequency.OldText = "";
            this.comboFrequency.Size = new System.Drawing.Size(102, 24);
            this.comboFrequency.TabIndex = 25;
            // 
            // txtEndTime
            // 
            this.txtEndTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtEndTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EndTime", true));
            this.txtEndTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtEndTime.IsSupportEditMode = false;
            this.txtEndTime.Location = new System.Drawing.Point(736, 62);
            this.txtEndTime.Mask = "90:00";
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.ReadOnly = true;
            this.txtEndTime.Size = new System.Drawing.Size(100, 23);
            this.txtEndTime.TabIndex = 27;
            // 
            // txtStartTime
            // 
            this.txtStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStartTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "StartTime", true));
            this.txtStartTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStartTime.IsSupportEditMode = false;
            this.txtStartTime.Location = new System.Drawing.Point(736, 30);
            this.txtStartTime.Mask = "90:00";
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.ReadOnly = true;
            this.txtStartTime.Size = new System.Drawing.Size(100, 23);
            this.txtStartTime.TabIndex = 26;
            // 
            // B34
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 469);
            this.IsSupportClip = false;
            this.IsSupportPrint = false;
            this.Name = "B34";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B31. Notification for Sort Out";
            this.UniqueExpress = "FactoryID";
            this.WorkAlias = "NotificationForSortOut";
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

        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label label5;
        private Win.UI.Label label8;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.EditBox Description;
        private Win.UI.TextBox txtStandard;
        private Win.UI.EditBox ToAddress;
        private Win.UI.EditBox CcAddress;
        private Win.UI.ComboBox comboFrequency;
        private Win.UI.TextBox txtEndTime;
        private Win.UI.TextBox txtStartTime;
    }
}