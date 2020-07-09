namespace Sci.Production.Thread
{
    partial class R20
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.comboMDivision = new Sci.Production.Class.ComboMDivision(this.components);
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.dateEstArrived = new Sci.Win.UI.DateRange();
            this.dateEstBooking = new Sci.Win.UI.DateRange();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelEstArrived = new Sci.Win.UI.Label();
            this.labelEstBooking = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelStatus = new Sci.Win.UI.Label();
            this.comboBoxStatus = new Sci.Win.UI.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(459, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(459, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(459, 84);
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxStatus);
            this.panel1.Controls.Add(this.txtSPNoStart);
            this.panel1.Controls.Add(this.comboMDivision);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.dateEstArrived);
            this.panel1.Controls.Add(this.dateEstBooking);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtSPNoEnd);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelEstArrived);
            this.panel1.Controls.Add(this.labelEstBooking);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(437, 230);
            this.panel1.TabIndex = 0;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(89, 0);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(149, 23);
            this.txtSPNoStart.TabIndex = 8;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(89, 166);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision.TabIndex = 5;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(89, 124);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 4;
            // 
            // dateEstArrived
            // 
            // 
            // 
            // 
            this.dateEstArrived.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstArrived.DateBox1.Name = "";
            this.dateEstArrived.DateBox1.Size = new System.Drawing.Size(157, 23);
            this.dateEstArrived.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstArrived.DateBox2.Location = new System.Drawing.Point(179, 0);
            this.dateEstArrived.DateBox2.Name = "";
            this.dateEstArrived.DateBox2.Size = new System.Drawing.Size(157, 23);
            this.dateEstArrived.DateBox2.TabIndex = 1;
            this.dateEstArrived.IsRequired = false;
            this.dateEstArrived.Location = new System.Drawing.Point(89, 81);
            this.dateEstArrived.Name = "dateEstArrived";
            this.dateEstArrived.Size = new System.Drawing.Size(336, 23);
            this.dateEstArrived.TabIndex = 3;
            // 
            // dateEstBooking
            // 
            // 
            // 
            // 
            this.dateEstBooking.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstBooking.DateBox1.Name = "";
            this.dateEstBooking.DateBox1.Size = new System.Drawing.Size(157, 23);
            this.dateEstBooking.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstBooking.DateBox2.Location = new System.Drawing.Point(179, 0);
            this.dateEstBooking.DateBox2.Name = "";
            this.dateEstBooking.DateBox2.Size = new System.Drawing.Size(157, 23);
            this.dateEstBooking.DateBox2.TabIndex = 1;
            this.dateEstBooking.IsRequired = false;
            this.dateEstBooking.Location = new System.Drawing.Point(89, 39);
            this.dateEstBooking.Name = "dateEstBooking";
            this.dateEstBooking.Size = new System.Drawing.Size(336, 23);
            this.dateEstBooking.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(249, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 16);
            this.label6.TabIndex = 7;
            this.label6.Text = "~";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(276, 0);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(149, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(0, 166);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(86, 23);
            this.labelM.TabIndex = 4;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(0, 124);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(86, 23);
            this.labelFactory.TabIndex = 3;
            this.labelFactory.Text = "Factory";
            // 
            // labelEstArrived
            // 
            this.labelEstArrived.Location = new System.Drawing.Point(0, 81);
            this.labelEstArrived.Name = "labelEstArrived";
            this.labelEstArrived.Size = new System.Drawing.Size(86, 23);
            this.labelEstArrived.TabIndex = 2;
            this.labelEstArrived.Text = "Est. Arrived";
            // 
            // labelEstBooking
            // 
            this.labelEstBooking.Location = new System.Drawing.Point(0, 41);
            this.labelEstBooking.Name = "labelEstBooking";
            this.labelEstBooking.Size = new System.Drawing.Size(86, 23);
            this.labelEstBooking.TabIndex = 1;
            this.labelEstBooking.Text = "Est. Booking";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(0, 0);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(86, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP No";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(0, 205);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(86, 23);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "Status";
            // 
            // comboBox1
            // 
            this.comboBoxStatus.BackColor = System.Drawing.Color.White;
            this.comboBoxStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.IsSupportUnselect = true;
            this.comboBoxStatus.Items.AddRange(new object[] {
            "All",
            "New",
            "Approved"});
            this.comboBoxStatus.Location = new System.Drawing.Point(90, 205);
            this.comboBoxStatus.Name = "comboBox1";
            this.comboBoxStatus.Size = new System.Drawing.Size(95, 24);
            this.comboBoxStatus.TabIndex = 9;
            // 
            // R20
            // 
            this.ClientSize = new System.Drawing.Size(551, 275);
            this.Controls.Add(this.panel1);
            this.Name = "R20";
            this.Text = "R20.Thread request List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.DateRange dateEstArrived;
        private Win.UI.DateRange dateEstBooking;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelEstArrived;
        private Win.UI.Label labelEstBooking;
        private Win.UI.Label labelSPNo;
        private Class.ComboMDivision comboMDivision;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.ComboBox comboBoxStatus;
        private Win.UI.Label labelStatus;
    }
}
