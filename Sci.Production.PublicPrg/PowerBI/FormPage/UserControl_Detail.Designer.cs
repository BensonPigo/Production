namespace Sci.Production.Prg.PowerBI.FormPage
{
    partial class UserControl_Detail
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.chkRun = new System.Windows.Forms.CheckBox();
            this.panel_Date = new System.Windows.Forms.Panel();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.lbDate = new Sci.Win.UI.Label();
            this.panel_DateRange1 = new System.Windows.Forms.Panel();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.lbDateRange1 = new Sci.Win.UI.Label();
            this.panel_DateRange2 = new System.Windows.Forms.Panel();
            this.dateRange2 = new Sci.Win.UI.DateRange();
            this.lbDate2 = new Sci.Win.UI.Label();
            this.panelBase = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel_BIname = new System.Windows.Forms.Panel();
            this.txtBIname = new System.Windows.Forms.TextBox();
            this.lbBIName = new Sci.Win.UI.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel_Date.SuspendLayout();
            this.panel_DateRange1.SuspendLayout();
            this.panel_DateRange2.SuspendLayout();
            this.panelBase.SuspendLayout();
            this.panel_BIname.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.90786F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.09214F));
            this.tableLayoutPanel1.Controls.Add(this.txtRemark, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkRun, 1, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 140);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(369, 81);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtRemark.Location = new System.Drawing.Point(3, 3);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ReadOnly = true;
            this.txtRemark.Size = new System.Drawing.Size(311, 75);
            this.txtRemark.TabIndex = 7;
            // 
            // chkRun
            // 
            this.chkRun.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkRun.AutoSize = true;
            this.chkRun.Location = new System.Drawing.Point(322, 3);
            this.chkRun.Name = "chkRun";
            this.chkRun.Size = new System.Drawing.Size(44, 75);
            this.chkRun.TabIndex = 8;
            this.chkRun.Text = "Run";
            this.chkRun.UseVisualStyleBackColor = true;
            // 
            // panel_Date
            // 
            this.panel_Date.Controls.Add(this.dateBox1);
            this.panel_Date.Controls.Add(this.lbDate);
            this.panel_Date.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel_Date.Location = new System.Drawing.Point(3, 111);
            this.panel_Date.Name = "panel_Date";
            this.panel_Date.Size = new System.Drawing.Size(224, 28);
            this.panel_Date.TabIndex = 9;
            // 
            // dateBox1
            // 
            this.dateBox1.Location = new System.Drawing.Point(78, 1);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 22);
            this.dateBox1.TabIndex = 1;
            // 
            // lbDate
            // 
            this.lbDate.Location = new System.Drawing.Point(0, 0);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(75, 23);
            this.lbDate.TabIndex = 0;
            this.lbDate.Text = "Date";
            // 
            // panel_DateRange1
            // 
            this.panel_DateRange1.Controls.Add(this.dateRange1);
            this.panel_DateRange1.Controls.Add(this.lbDateRange1);
            this.panel_DateRange1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel_DateRange1.Location = new System.Drawing.Point(3, 47);
            this.panel_DateRange1.Name = "panel_DateRange1";
            this.panel_DateRange1.Size = new System.Drawing.Size(369, 26);
            this.panel_DateRange1.TabIndex = 7;
            // 
            // dateRange1
            // 
            // 
            // 
            // 
            this.dateRange1.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange1.DateBox1.Name = "";
            this.dateRange1.DateBox1.Size = new System.Drawing.Size(131, 22);
            this.dateRange1.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange1.DateBox2.Location = new System.Drawing.Point(148, 0);
            this.dateRange1.DateBox2.Name = "";
            this.dateRange1.DateBox2.Size = new System.Drawing.Size(131, 22);
            this.dateRange1.DateBox2.TabIndex = 1;
            this.dateRange1.Location = new System.Drawing.Point(78, 0);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 22);
            this.dateRange1.TabIndex = 1;
            // 
            // lbDateRange1
            // 
            this.lbDateRange1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbDateRange1.Location = new System.Drawing.Point(0, 0);
            this.lbDateRange1.Name = "lbDateRange1";
            this.lbDateRange1.Size = new System.Drawing.Size(75, 23);
            this.lbDateRange1.TabIndex = 0;
            this.lbDateRange1.Text = "Date";
            // 
            // panel_DateRange2
            // 
            this.panel_DateRange2.Controls.Add(this.dateRange2);
            this.panel_DateRange2.Controls.Add(this.lbDate2);
            this.panel_DateRange2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.panel_DateRange2.Location = new System.Drawing.Point(3, 79);
            this.panel_DateRange2.Name = "panel_DateRange2";
            this.panel_DateRange2.Size = new System.Drawing.Size(369, 26);
            this.panel_DateRange2.TabIndex = 8;
            // 
            // dateRange2
            // 
            // 
            // 
            // 
            this.dateRange2.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange2.DateBox1.Name = "";
            this.dateRange2.DateBox1.Size = new System.Drawing.Size(131, 22);
            this.dateRange2.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange2.DateBox2.Location = new System.Drawing.Point(148, 0);
            this.dateRange2.DateBox2.Name = "";
            this.dateRange2.DateBox2.Size = new System.Drawing.Size(131, 22);
            this.dateRange2.DateBox2.TabIndex = 1;
            this.dateRange2.Location = new System.Drawing.Point(78, 0);
            this.dateRange2.Name = "dateRange2";
            this.dateRange2.Size = new System.Drawing.Size(280, 22);
            this.dateRange2.TabIndex = 1;
            // 
            // lbDate2
            // 
            this.lbDate2.Location = new System.Drawing.Point(0, 0);
            this.lbDate2.Name = "lbDate2";
            this.lbDate2.Size = new System.Drawing.Size(75, 23);
            this.lbDate2.TabIndex = 0;
            this.lbDate2.Text = "Date2";
            // 
            // panelBase
            // 
            this.panelBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBase.Controls.Add(this.flowLayoutPanel1);
            this.panelBase.Controls.Add(this.panel_BIname);
            this.panelBase.Controls.Add(this.tableLayoutPanel1);
            this.panelBase.Controls.Add(this.panel_Date);
            this.panelBase.Controls.Add(this.panel_DateRange2);
            this.panelBase.Controls.Add(this.panel_DateRange1);
            this.panelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBase.Location = new System.Drawing.Point(0, 0);
            this.panelBase.Name = "panelBase";
            this.panelBase.Size = new System.Drawing.Size(383, 233);
            this.panelBase.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(381, 231);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // panel_BIname
            // 
            this.panel_BIname.Controls.Add(this.txtBIname);
            this.panel_BIname.Controls.Add(this.lbBIName);
            this.panel_BIname.Location = new System.Drawing.Point(3, 15);
            this.panel_BIname.Name = "panel_BIname";
            this.panel_BIname.Size = new System.Drawing.Size(369, 26);
            this.panel_BIname.TabIndex = 11;
            // 
            // txtBIname
            // 
            this.txtBIname.Location = new System.Drawing.Point(78, 0);
            this.txtBIname.Name = "txtBIname";
            this.txtBIname.ReadOnly = true;
            this.txtBIname.Size = new System.Drawing.Size(291, 22);
            this.txtBIname.TabIndex = 3;
            // 
            // lbBIName
            // 
            this.lbBIName.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbBIName.Location = new System.Drawing.Point(0, 0);
            this.lbBIName.Name = "lbBIName";
            this.lbBIName.Size = new System.Drawing.Size(75, 23);
            this.lbBIName.TabIndex = 2;
            this.lbBIName.Text = "Name";
            // 
            // UserControl_Detail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBase);
            this.Name = "UserControl_Detail";
            this.Size = new System.Drawing.Size(383, 233);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel_Date.ResumeLayout(false);
            this.panel_DateRange1.ResumeLayout(false);
            this.panel_DateRange2.ResumeLayout(false);
            this.panelBase.ResumeLayout(false);
            this.panel_BIname.ResumeLayout(false);
            this.panel_BIname.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.CheckBox chkRun;
        private System.Windows.Forms.Panel panel_Date;
        private Win.UI.DateBox dateBox1;
        private Win.UI.Label lbDate;
        private System.Windows.Forms.Panel panel_DateRange1;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Label lbDateRange1;
        private System.Windows.Forms.Panel panel_DateRange2;
        private Win.UI.DateRange dateRange2;
        private Win.UI.Label lbDate2;
        private System.Windows.Forms.Panel panelBase;
        private System.Windows.Forms.Panel panel_BIname;
        private System.Windows.Forms.TextBox txtBIname;
        private Win.UI.Label lbBIName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
