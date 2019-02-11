namespace Sci.Production.Cutting
{
    partial class B06
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
            this.numPreparationTime = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            this.numChangeOverRollTime = new Sci.Win.UI.NumericBox();
            this.numChangeOverUnRollTime = new Sci.Win.UI.NumericBox();
            this.numSetupTime = new Sci.Win.UI.NumericBox();
            this.numSeparatorTime = new Sci.Win.UI.NumericBox();
            this.numSpreadingTime = new Sci.Win.UI.NumericBox();
            this.numForwardTime = new Sci.Win.UI.NumericBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.txtWeaveTypeID = new Sci.Win.UI.TextBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
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
            this.detail.Size = new System.Drawing.Size(828, 411);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.numForwardTime);
            this.detailcont.Controls.Add(this.numSpreadingTime);
            this.detailcont.Controls.Add(this.numSeparatorTime);
            this.detailcont.Controls.Add(this.numSetupTime);
            this.detailcont.Controls.Add(this.numChangeOverUnRollTime);
            this.detailcont.Controls.Add(this.numChangeOverRollTime);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.numPreparationTime);
            this.detailcont.Controls.Add(this.txtWeaveTypeID);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(828, 373);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 373);
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 411);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 440);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // numPreparationTime
            // 
            this.numPreparationTime.BackColor = System.Drawing.Color.White;
            this.numPreparationTime.DecimalPlaces = 3;
            this.numPreparationTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPreparationTime.Location = new System.Drawing.Point(206, 59);
            this.numPreparationTime.MaxLength = 99999;
            this.numPreparationTime.Name = "numPreparationTime";
            this.numPreparationTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPreparationTime.Size = new System.Drawing.Size(100, 23);
            this.numPreparationTime.TabIndex = 9;
            this.numPreparationTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "PreparationTime(secs/yds)";
            // 
            // numChangeOverRollTime
            // 
            this.numChangeOverRollTime.BackColor = System.Drawing.Color.White;
            this.numChangeOverRollTime.DecimalPlaces = 3;
            this.numChangeOverRollTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numChangeOverRollTime.Location = new System.Drawing.Point(206, 100);
            this.numChangeOverRollTime.MaxLength = 99999;
            this.numChangeOverRollTime.Name = "numChangeOverRollTime";
            this.numChangeOverRollTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numChangeOverRollTime.Size = new System.Drawing.Size(100, 23);
            this.numChangeOverRollTime.TabIndex = 11;
            this.numChangeOverRollTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numChangeOverUnRollTime
            // 
            this.numChangeOverUnRollTime.BackColor = System.Drawing.Color.White;
            this.numChangeOverUnRollTime.DecimalPlaces = 3;
            this.numChangeOverUnRollTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numChangeOverUnRollTime.Location = new System.Drawing.Point(519, 100);
            this.numChangeOverUnRollTime.MaxLength = 99999;
            this.numChangeOverUnRollTime.Name = "numChangeOverUnRollTime";
            this.numChangeOverUnRollTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numChangeOverUnRollTime.Size = new System.Drawing.Size(100, 23);
            this.numChangeOverUnRollTime.TabIndex = 12;
            this.numChangeOverUnRollTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numSetupTime
            // 
            this.numSetupTime.BackColor = System.Drawing.Color.White;
            this.numSetupTime.DecimalPlaces = 3;
            this.numSetupTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSetupTime.Location = new System.Drawing.Point(206, 139);
            this.numSetupTime.MaxLength = 99999;
            this.numSetupTime.Name = "numSetupTime";
            this.numSetupTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSetupTime.Size = new System.Drawing.Size(100, 23);
            this.numSetupTime.TabIndex = 13;
            this.numSetupTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numSeparatorTime
            // 
            this.numSeparatorTime.BackColor = System.Drawing.Color.White;
            this.numSeparatorTime.DecimalPlaces = 3;
            this.numSeparatorTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSeparatorTime.Location = new System.Drawing.Point(206, 180);
            this.numSeparatorTime.MaxLength = 99999;
            this.numSeparatorTime.Name = "numSeparatorTime";
            this.numSeparatorTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSeparatorTime.Size = new System.Drawing.Size(100, 23);
            this.numSeparatorTime.TabIndex = 14;
            this.numSeparatorTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numSpreadingTime
            // 
            this.numSpreadingTime.BackColor = System.Drawing.Color.White;
            this.numSpreadingTime.DecimalPlaces = 3;
            this.numSpreadingTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSpreadingTime.Location = new System.Drawing.Point(206, 221);
            this.numSpreadingTime.MaxLength = 99999;
            this.numSpreadingTime.Name = "numSpreadingTime";
            this.numSpreadingTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSpreadingTime.Size = new System.Drawing.Size(100, 23);
            this.numSpreadingTime.TabIndex = 15;
            this.numSpreadingTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numForwardTime
            // 
            this.numForwardTime.BackColor = System.Drawing.Color.White;
            this.numForwardTime.DecimalPlaces = 3;
            this.numForwardTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numForwardTime.Location = new System.Drawing.Point(206, 262);
            this.numForwardTime.MaxLength = 99999;
            this.numForwardTime.Name = "numForwardTime";
            this.numForwardTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numForwardTime.Size = new System.Drawing.Size(100, 23);
            this.numForwardTime.TabIndex = 16;
            this.numForwardTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "ChangeOverRollTime(sec)";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(324, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "ChangeOverUnRollTime(sec)";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(21, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(182, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "SetupTime(sec)";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(21, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 23);
            this.label5.TabIndex = 20;
            this.label5.Text = "SeparatorTime(secs)";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(21, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 23);
            this.label6.TabIndex = 21;
            this.label6.Text = "SpreadingTime(secs/yds)";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(21, 262);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(182, 23);
            this.label7.TabIndex = 22;
            this.label7.Text = "ForwardTime(sec)";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(21, 18);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(163, 23);
            this.labelID.TabIndex = 8;
            this.labelID.Text = "WeaveTypeID";
            // 
            // txtWeaveTypeID
            // 
            this.txtWeaveTypeID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWeaveTypeID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtWeaveTypeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWeaveTypeID.IsSupportEditMode = false;
            this.txtWeaveTypeID.Location = new System.Drawing.Point(187, 18);
            this.txtWeaveTypeID.Name = "txtWeaveTypeID";
            this.txtWeaveTypeID.ReadOnly = true;
            this.txtWeaveTypeID.Size = new System.Drawing.Size(199, 23);
            this.txtWeaveTypeID.TabIndex = 0;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(21, 291);
            this.displayBox1.Multiline = true;
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(616, 76);
            this.displayBox1.TabIndex = 24;
            // 
            // B06
            // 
            this.ClientSize = new System.Drawing.Size(836, 473);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B06";
            this.Text = "B06 Spreading Time";
            this.WorkAlias = "WeaveType";
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
        private Win.UI.Label label1;
        private Win.UI.NumericBox numPreparationTime;
        private Win.UI.NumericBox numForwardTime;
        private Win.UI.NumericBox numSpreadingTime;
        private Win.UI.NumericBox numSeparatorTime;
        private Win.UI.NumericBox numSetupTime;
        private Win.UI.NumericBox numChangeOverUnRollTime;
        private Win.UI.NumericBox numChangeOverRollTime;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtWeaveTypeID;
        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayBox1;
    }
}
