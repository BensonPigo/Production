namespace Sci.Production.Warehouse
{
    partial class B07
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
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.labM = new Sci.Win.UI.Label();
            this.labStartDate = new Sci.Win.UI.Label();
            this.labBeginTime = new Sci.Win.UI.Label();
            this.labEndTime = new Sci.Win.UI.Label();
            this.dateStartDate = new Sci.Win.UI.DateBox();
            this.txtBeginTime = new Sci.Win.UI.TextBox();
            this.txtEndTime = new Sci.Win.UI.TextBox();
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
            this.detailcont.Controls.Add(this.txtBeginTime);
            this.detailcont.Controls.Add(this.dateStartDate);
            this.detailcont.Controls.Add(this.labEndTime);
            this.detailcont.Controls.Add(this.labBeginTime);
            this.detailcont.Controls.Add(this.labStartDate);
            this.detailcont.Controls.Add(this.labM);
            this.detailcont.Controls.Add(this.txtMdivision);
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMdivision.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Mdivision", true));
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMdivision.IsSupportEditMode = false;
            this.txtMdivision.Location = new System.Drawing.Point(108, 34);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.ReadOnly = true;
            this.txtMdivision.Size = new System.Drawing.Size(90, 23);
            this.txtMdivision.TabIndex = 7;
            // 
            // labM
            // 
            this.labM.Location = new System.Drawing.Point(26, 34);
            this.labM.Name = "labM";
            this.labM.Size = new System.Drawing.Size(79, 23);
            this.labM.TabIndex = 3;
            this.labM.Text = "M";
            // 
            // labStartDate
            // 
            this.labStartDate.Location = new System.Drawing.Point(26, 73);
            this.labStartDate.Name = "labStartDate";
            this.labStartDate.Size = new System.Drawing.Size(79, 23);
            this.labStartDate.TabIndex = 4;
            this.labStartDate.Text = "Start Date";
            // 
            // labBeginTime
            // 
            this.labBeginTime.Location = new System.Drawing.Point(26, 115);
            this.labBeginTime.Name = "labBeginTime";
            this.labBeginTime.Size = new System.Drawing.Size(79, 23);
            this.labBeginTime.TabIndex = 5;
            this.labBeginTime.Text = "Begin Time";
            // 
            // labEndTime
            // 
            this.labEndTime.Location = new System.Drawing.Point(26, 156);
            this.labEndTime.Name = "labEndTime";
            this.labEndTime.Size = new System.Drawing.Size(79, 23);
            this.labEndTime.TabIndex = 6;
            this.labEndTime.Text = "End Time";
            // 
            // dateStartDate
            // 
            this.dateStartDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StartDate", true));
            this.dateStartDate.Location = new System.Drawing.Point(108, 73);
            this.dateStartDate.Name = "dateStartDate";
            this.dateStartDate.Size = new System.Drawing.Size(123, 23);
            this.dateStartDate.TabIndex = 0;
            // 
            // txtBeginTime
            // 
            this.txtBeginTime.BackColor = System.Drawing.Color.White;
            this.txtBeginTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BeginTime", true));
            this.txtBeginTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBeginTime.Location = new System.Drawing.Point(108, 115);
            this.txtBeginTime.Mask = "90:00";
            this.txtBeginTime.Name = "txtBeginTime";
            this.txtBeginTime.Size = new System.Drawing.Size(90, 23);
            this.txtBeginTime.TabIndex = 8;
            this.txtBeginTime.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtBeginTime.ValidatingType = typeof(System.DateTime);
            this.txtBeginTime.Validating += new System.ComponentModel.CancelEventHandler(this.TxtBeginTime_Validating);
            // 
            // txtEndTime
            // 
            this.txtEndTime.BackColor = System.Drawing.Color.White;
            this.txtEndTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EndTime", true));
            this.txtEndTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEndTime.IsAutoTrim = false;
            this.txtEndTime.Location = new System.Drawing.Point(108, 156);
            this.txtEndTime.Mask = "00:00";
            this.txtEndTime.MaxLength = 4;
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Size = new System.Drawing.Size(90, 23);
            this.txtEndTime.TabIndex = 9;
            this.txtEndTime.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtEndTime.ValidatingType = typeof(System.DateTime);
            this.txtEndTime.TextChanged += new System.EventHandler(this.TxtEndTime_TextChanged);
            this.txtEndTime.Validating += new System.ComponentModel.CancelEventHandler(this.TxtEndTime_Validating);
            // 
            // B07
            // 
            this.ClientSize = new System.Drawing.Size(905, 457);
            this.DefaultControl = "dateStartDate";
            this.DefaultControlForEdit = "dateStartDate";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B07. Warehouse working calendar";
            this.WorkAlias = "WHWorkingCalendar";
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

        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labM;
        private Win.UI.Label labEndTime;
        private Win.UI.Label labBeginTime;
        private Win.UI.Label labStartDate;
        private Win.UI.DateBox dateStartDate;
        private Win.UI.TextBox txtBeginTime;
        protected Win.UI.TextBox txtEndTime;
    }
}
