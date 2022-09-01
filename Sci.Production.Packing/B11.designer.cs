namespace Sci.Production.Packing
{
    partial class B11
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
            this.labFty = new Sci.Win.UI.Label();
            this.txtFty = new Sci.Production.Class.Txtfactory();
            this.label2 = new Sci.Win.UI.Label();
            this.labToAddress = new Sci.Win.UI.Label();
            this.labCcAddress = new Sci.Win.UI.Label();
            this.editBoxToAddress = new Sci.Win.UI.EditBox();
            this.editBoxCcAddress = new Sci.Win.UI.EditBox();
            this.labDescription = new Sci.Win.UI.Label();
            this.editBoxDescription = new Sci.Win.UI.EditBox();
            this.labFrequency = new Sci.Win.UI.Label();
            this.labEndTime = new Sci.Win.UI.Label();
            this.labStartTime = new Sci.Win.UI.Label();
            this.txtStartTime = new Sci.Win.UI.TextBox();
            this.txtEndTime = new Sci.Win.UI.TextBox();
            this.comboFrequency = new Sci.Win.UI.ComboBox();
            this.dateBuyerDlv = new Sci.Win.UI.DateBox();
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
            this.detail.Size = new System.Drawing.Size(854, 374);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.dateBuyerDlv);
            this.detailcont.Controls.Add(this.comboFrequency);
            this.detailcont.Controls.Add(this.txtEndTime);
            this.detailcont.Controls.Add(this.txtStartTime);
            this.detailcont.Controls.Add(this.labFrequency);
            this.detailcont.Controls.Add(this.labEndTime);
            this.detailcont.Controls.Add(this.labStartTime);
            this.detailcont.Controls.Add(this.editBoxDescription);
            this.detailcont.Controls.Add(this.labDescription);
            this.detailcont.Controls.Add(this.editBoxCcAddress);
            this.detailcont.Controls.Add(this.editBoxToAddress);
            this.detailcont.Controls.Add(this.labCcAddress);
            this.detailcont.Controls.Add(this.labToAddress);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.txtFty);
            this.detailcont.Controls.Add(this.labFty);
            this.detailcont.Size = new System.Drawing.Size(854, 336);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 336);
            this.detailbtm.Size = new System.Drawing.Size(854, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(854, 374);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(862, 403);
            // 
            // labFty
            // 
            this.labFty.Location = new System.Drawing.Point(21, 18);
            this.labFty.Name = "labFty";
            this.labFty.Size = new System.Drawing.Size(101, 23);
            this.labFty.TabIndex = 8;
            this.labFty.Text = "Factory";
            // 
            // txtFty
            // 
            this.txtFty.BackColor = System.Drawing.Color.White;
            this.txtFty.BoolFtyGroupList = true;
            this.txtFty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtFty.FilteMDivision = true;
            this.txtFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFty.IsProduceFty = true;
            this.txtFty.IssupportJunk = false;
            this.txtFty.Location = new System.Drawing.Point(125, 18);
            this.txtFty.MDivision = null;
            this.txtFty.Name = "txtFty";
            this.txtFty.Size = new System.Drawing.Size(100, 23);
            this.txtFty.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Buyer Delivery";
            // 
            // labToAddress
            // 
            this.labToAddress.Location = new System.Drawing.Point(21, 78);
            this.labToAddress.Name = "labToAddress";
            this.labToAddress.Size = new System.Drawing.Size(101, 23);
            this.labToAddress.TabIndex = 10;
            this.labToAddress.Text = "To Address";
            // 
            // labCcAddress
            // 
            this.labCcAddress.Location = new System.Drawing.Point(21, 147);
            this.labCcAddress.Name = "labCcAddress";
            this.labCcAddress.Size = new System.Drawing.Size(101, 23);
            this.labCcAddress.TabIndex = 11;
            this.labCcAddress.Text = "Cc Address";
            // 
            // editBoxToAddress
            // 
            this.editBoxToAddress.BackColor = System.Drawing.Color.White;
            this.editBoxToAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ToAddress", true));
            this.editBoxToAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxToAddress.Location = new System.Drawing.Point(125, 78);
            this.editBoxToAddress.Multiline = true;
            this.editBoxToAddress.Name = "editBoxToAddress";
            this.editBoxToAddress.Size = new System.Drawing.Size(490, 63);
            this.editBoxToAddress.TabIndex = 2;
            // 
            // editBoxCcAddress
            // 
            this.editBoxCcAddress.BackColor = System.Drawing.Color.White;
            this.editBoxCcAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CcAddress", true));
            this.editBoxCcAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxCcAddress.Location = new System.Drawing.Point(125, 147);
            this.editBoxCcAddress.Multiline = true;
            this.editBoxCcAddress.Name = "editBoxCcAddress";
            this.editBoxCcAddress.Size = new System.Drawing.Size(490, 70);
            this.editBoxCcAddress.TabIndex = 3;
            // 
            // labDescription
            // 
            this.labDescription.Location = new System.Drawing.Point(21, 231);
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new System.Drawing.Size(101, 23);
            this.labDescription.TabIndex = 12;
            this.labDescription.Text = "Description";
            // 
            // editBoxDescription
            // 
            this.editBoxDescription.BackColor = System.Drawing.Color.White;
            this.editBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editBoxDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxDescription.Location = new System.Drawing.Point(125, 231);
            this.editBoxDescription.Multiline = true;
            this.editBoxDescription.Name = "editBoxDescription";
            this.editBoxDescription.Size = new System.Drawing.Size(490, 84);
            this.editBoxDescription.TabIndex = 4;
            // 
            // labFrequency
            // 
            this.labFrequency.Location = new System.Drawing.Point(632, 88);
            this.labFrequency.Name = "labFrequency";
            this.labFrequency.Size = new System.Drawing.Size(81, 23);
            this.labFrequency.TabIndex = 15;
            this.labFrequency.Text = "Frequency";
            // 
            // labEndTime
            // 
            this.labEndTime.Location = new System.Drawing.Point(632, 53);
            this.labEndTime.Name = "labEndTime";
            this.labEndTime.Size = new System.Drawing.Size(81, 23);
            this.labEndTime.TabIndex = 14;
            this.labEndTime.Text = "End Time";
            // 
            // labStartTime
            // 
            this.labStartTime.Location = new System.Drawing.Point(632, 18);
            this.labStartTime.Name = "labStartTime";
            this.labStartTime.Size = new System.Drawing.Size(81, 23);
            this.labStartTime.TabIndex = 13;
            this.labStartTime.Text = "Start Time";
            // 
            // txtStartTime
            // 
            this.txtStartTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStartTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "StartTime", true));
            this.txtStartTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStartTime.IsSupportEditMode = false;
            this.txtStartTime.Location = new System.Drawing.Point(716, 18);
            this.txtStartTime.Mask = "90:00";
            this.txtStartTime.Name = "txtStartTime";
            this.txtStartTime.ReadOnly = true;
            this.txtStartTime.Size = new System.Drawing.Size(100, 23);
            this.txtStartTime.TabIndex = 5;
            // 
            // txtEndTime
            // 
            this.txtEndTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtEndTime.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EndTime", true));
            this.txtEndTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtEndTime.IsSupportEditMode = false;
            this.txtEndTime.Location = new System.Drawing.Point(716, 53);
            this.txtEndTime.Mask = "90:00";
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.ReadOnly = true;
            this.txtEndTime.Size = new System.Drawing.Size(100, 23);
            this.txtEndTime.TabIndex = 6;
            // 
            // comboFrequency
            // 
            this.comboFrequency.BackColor = System.Drawing.Color.White;
            this.comboFrequency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Frequency", true));
            this.comboFrequency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFrequency.FormattingEnabled = true;
            this.comboFrequency.IsSupportUnselect = true;
            this.comboFrequency.Location = new System.Drawing.Point(716, 88);
            this.comboFrequency.Name = "comboFrequency";
            this.comboFrequency.OldText = "";
            this.comboFrequency.Size = new System.Drawing.Size(97, 24);
            this.comboFrequency.TabIndex = 7;
            this.comboFrequency.SelectedValueChanged += new System.EventHandler(this.ComboFrequency_SelectedValueChanged);
            // 
            // dateBuyerDlv
            // 
            this.dateBuyerDlv.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BuyerDeliveryDate", true));
            this.dateBuyerDlv.Location = new System.Drawing.Point(125, 49);
            this.dateBuyerDlv.Name = "dateBuyerDlv";
            this.dateBuyerDlv.Size = new System.Drawing.Size(131, 23);
            this.dateBuyerDlv.TabIndex = 1;
            // 
            // B11
            // 
            this.ClientSize = new System.Drawing.Size(862, 436);
            this.DefaultControl = "txtFty";
            this.DefaultControlForEdit = "txtFty";
            this.IsSupportClip = false;
            this.IsSupportPrint = false;
            this.Name = "B11";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B11. Lost 1st MD Notification";
            this.UniqueExpress = "FactoryID";
            this.WorkAlias = "Lost1stMDNotification";
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

        private Class.Txtfactory txtFty;
        private Win.UI.Label labFty;
        private Win.UI.Label label2;
        private Win.UI.Label labCcAddress;
        private Win.UI.Label labToAddress;
        private Win.UI.EditBox editBoxToAddress;
        private Win.UI.EditBox editBoxDescription;
        private Win.UI.Label labDescription;
        private Win.UI.EditBox editBoxCcAddress;
        private Win.UI.Label labFrequency;
        private Win.UI.Label labEndTime;
        private Win.UI.Label labStartTime;
        private Win.UI.TextBox txtStartTime;
        private Win.UI.TextBox txtEndTime;
        private Win.UI.ComboBox comboFrequency;
        private Win.UI.DateBox dateBuyerDlv;
    }
}
