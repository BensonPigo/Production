namespace Sci.Production.Quality
{
    partial class P11_Detail
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
            this.panelup = new Sci.Win.UI.Panel();
            this.txtTechnician = new Sci.Production.Class.TxtTechnician();
            this.dateBoxReleasedDate = new Sci.Win.UI.DateBox();
            this.dateBoxReceivedDate = new Sci.Win.UI.DateBox();
            this.dateBoxSubmitDate = new Sci.Win.UI.DateBox();
            this.txtMR = new Sci.Production.Class.Txtuser();
            this.txtCombineStyle = new Sci.Win.UI.TextBox();
            this.displayResult = new Sci.Win.UI.DisplayBox();
            this.displayReportNo = new Sci.Win.UI.DisplayBox();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.displayArticle = new Sci.Win.UI.DisplayBox();
            this.displayBrandID = new Sci.Win.UI.DisplayBox();
            this.displaySeasonID = new Sci.Win.UI.DisplayBox();
            this.displayStyleID = new Sci.Win.UI.DisplayBox();
            this.label13 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.btnPDF = new Sci.Win.UI.Button();
            this.btnSendMR = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.panelup.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnSendMR);
            this.btmcont.Controls.Add(this.btnPDF);
            this.btmcont.Location = new System.Drawing.Point(0, 389);
            this.btmcont.Size = new System.Drawing.Size(995, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnPDF, 0);
            this.btmcont.Controls.SetChildIndex(this.btnSendMR, 0);
            // 
            // gridcont
            // 
            this.gridcont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.gridcont.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridcont.Location = new System.Drawing.Point(0, 152);
            this.gridcont.Size = new System.Drawing.Size(995, 237);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(905, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(825, 5);
            // 
            // panelup
            // 
            this.panelup.Controls.Add(this.txtTechnician);
            this.panelup.Controls.Add(this.dateBoxReleasedDate);
            this.panelup.Controls.Add(this.dateBoxReceivedDate);
            this.panelup.Controls.Add(this.dateBoxSubmitDate);
            this.panelup.Controls.Add(this.txtMR);
            this.panelup.Controls.Add(this.txtCombineStyle);
            this.panelup.Controls.Add(this.displayResult);
            this.panelup.Controls.Add(this.displayReportNo);
            this.panelup.Controls.Add(this.displayNo);
            this.panelup.Controls.Add(this.displayArticle);
            this.panelup.Controls.Add(this.displayBrandID);
            this.panelup.Controls.Add(this.displaySeasonID);
            this.panelup.Controls.Add(this.displayStyleID);
            this.panelup.Controls.Add(this.label13);
            this.panelup.Controls.Add(this.label12);
            this.panelup.Controls.Add(this.label11);
            this.panelup.Controls.Add(this.label10);
            this.panelup.Controls.Add(this.label9);
            this.panelup.Controls.Add(this.label8);
            this.panelup.Controls.Add(this.label7);
            this.panelup.Controls.Add(this.label6);
            this.panelup.Controls.Add(this.label5);
            this.panelup.Controls.Add(this.label4);
            this.panelup.Controls.Add(this.label3);
            this.panelup.Controls.Add(this.label2);
            this.panelup.Controls.Add(this.label1);
            this.panelup.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelup.Location = new System.Drawing.Point(0, 0);
            this.panelup.Name = "panelup";
            this.panelup.Size = new System.Drawing.Size(995, 152);
            this.panelup.TabIndex = 98;
            // 
            // txtTechnician
            // 
            this.txtTechnician.CheckColumn = "MockupCrocking";
            this.txtTechnician.DisplayBox1Binding = "";
            this.txtTechnician.Location = new System.Drawing.Point(154, 117);
            this.txtTechnician.Name = "txtTechnician";
            this.txtTechnician.Size = new System.Drawing.Size(300, 23);
            this.txtTechnician.TabIndex = 29;
            this.txtTechnician.TextBox1Binding = "";
            // 
            // dateBoxReleasedDate
            // 
            this.dateBoxReleasedDate.Location = new System.Drawing.Point(625, 81);
            this.dateBoxReleasedDate.Name = "dateBoxReleasedDate";
            this.dateBoxReleasedDate.Size = new System.Drawing.Size(130, 23);
            this.dateBoxReleasedDate.TabIndex = 28;
            // 
            // dateBoxReceivedDate
            // 
            this.dateBoxReceivedDate.Location = new System.Drawing.Point(390, 81);
            this.dateBoxReceivedDate.Name = "dateBoxReceivedDate";
            this.dateBoxReceivedDate.Size = new System.Drawing.Size(130, 23);
            this.dateBoxReceivedDate.TabIndex = 27;
            // 
            // dateBoxSubmitDate
            // 
            this.dateBoxSubmitDate.Location = new System.Drawing.Point(154, 81);
            this.dateBoxSubmitDate.Name = "dateBoxSubmitDate";
            this.dateBoxSubmitDate.Size = new System.Drawing.Size(130, 23);
            this.dateBoxSubmitDate.TabIndex = 26;
            // 
            // txtMR
            // 
            this.txtMR.DisplayBox1Binding = "";
            this.txtMR.Location = new System.Drawing.Point(625, 117);
            this.txtMR.Name = "txtMR";
            this.txtMR.Size = new System.Drawing.Size(300, 23);
            this.txtMR.TabIndex = 25;
            this.txtMR.TextBox1Binding = "";
            // 
            // txtCombineStyle
            // 
            this.txtCombineStyle.BackColor = System.Drawing.Color.White;
            this.txtCombineStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCombineStyle.Location = new System.Drawing.Point(154, 45);
            this.txtCombineStyle.MaxLength = 120;
            this.txtCombineStyle.Name = "txtCombineStyle";
            this.txtCombineStyle.Size = new System.Drawing.Size(366, 23);
            this.txtCombineStyle.TabIndex = 20;
            this.txtCombineStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtCombineStyle_PopUp);
            this.txtCombineStyle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCombineStyle_KeyPress);
            // 
            // displayResult
            // 
            this.displayResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayResult.Location = new System.Drawing.Point(871, 81);
            this.displayResult.Name = "displayResult";
            this.displayResult.Size = new System.Drawing.Size(118, 23);
            this.displayResult.TabIndex = 19;
            // 
            // displayReportNo
            // 
            this.displayReportNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayReportNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayReportNo.Location = new System.Drawing.Point(871, 45);
            this.displayReportNo.Name = "displayReportNo";
            this.displayReportNo.Size = new System.Drawing.Size(118, 23);
            this.displayReportNo.TabIndex = 18;
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(625, 45);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(129, 23);
            this.displayNo.TabIndex = 17;
            // 
            // displayArticle
            // 
            this.displayArticle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArticle.Location = new System.Drawing.Point(871, 9);
            this.displayArticle.Name = "displayArticle";
            this.displayArticle.Size = new System.Drawing.Size(118, 23);
            this.displayArticle.TabIndex = 16;
            // 
            // displayBrandID
            // 
            this.displayBrandID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrandID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrandID.Location = new System.Drawing.Point(625, 9);
            this.displayBrandID.Name = "displayBrandID";
            this.displayBrandID.Size = new System.Drawing.Size(129, 23);
            this.displayBrandID.TabIndex = 15;
            // 
            // displaySeasonID
            // 
            this.displaySeasonID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeasonID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeasonID.Location = new System.Drawing.Point(390, 9);
            this.displaySeasonID.Name = "displaySeasonID";
            this.displaySeasonID.Size = new System.Drawing.Size(130, 23);
            this.displaySeasonID.TabIndex = 14;
            // 
            // displayStyleID
            // 
            this.displayStyleID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleID.Location = new System.Drawing.Point(154, 9);
            this.displayStyleID.Name = "displayStyleID";
            this.displayStyleID.Size = new System.Drawing.Size(130, 23);
            this.displayStyleID.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(757, 81);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(111, 23);
            this.label13.TabIndex = 12;
            this.label13.Text = "Result";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(757, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(111, 23);
            this.label12.TabIndex = 11;
            this.label12.Text = "Report No.";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(757, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(111, 23);
            this.label11.TabIndex = 10;
            this.label11.Text = "Article / Colorway";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(523, 81);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Released Date";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(10, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(141, 23);
            this.label9.TabIndex = 8;
            this.label9.Text = "Submit Date";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(9, 117);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Technician";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(287, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Season";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(523, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(287, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Received Date";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(523, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "No. of Test";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(522, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "MR";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Combine Test Style#";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Style#";
            // 
            // btnPDF
            // 
            this.btnPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnPDF.Location = new System.Drawing.Point(739, 5);
            this.btnPDF.Name = "btnPDF";
            this.btnPDF.Size = new System.Drawing.Size(80, 30);
            this.btnPDF.TabIndex = 95;
            this.btnPDF.Text = "To PDF";
            this.btnPDF.UseVisualStyleBackColor = true;
            this.btnPDF.Click += new System.EventHandler(this.btnPDF_Click);
            // 
            // btnSendMR
            // 
            this.btnSendMR.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnSendMR.Location = new System.Drawing.Point(625, 5);
            this.btnSendMR.Name = "btnSendMR";
            this.btnSendMR.Size = new System.Drawing.Size(108, 30);
            this.btnSendMR.TabIndex = 96;
            this.btnSendMR.Text = "Send to MR";
            this.btnSendMR.UseVisualStyleBackColor = true;
            this.btnSendMR.Click += new System.EventHandler(this.btnSendMR_Click);
            // 
            // P11_Detail
            // 
            this.ClientSize = new System.Drawing.Size(995, 429);
            this.Controls.Add(this.panelup);
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.KeyField2 = "ReportNo";
            this.Name = "P11_Detail";
            this.Text = "Mockup - Crocking Test";
            this.WorkAlias = "MockupCrocking_Detail_Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.panelup, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.panelup.ResumeLayout(false);
            this.panelup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panelup;
        private Win.UI.Label label13;
        private Win.UI.Label label12;
        private Win.UI.Label label11;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.Txtuser txtMR;
        private Win.UI.DisplayBox displayResult;
        private Win.UI.DisplayBox displayReportNo;
        private Win.UI.DisplayBox displayNo;
        private Win.UI.DisplayBox displayArticle;
        private Win.UI.DisplayBox displayBrandID;
        private Win.UI.DisplayBox displaySeasonID;
        private Win.UI.DisplayBox displayStyleID;
        private Win.UI.DateBox dateBoxReleasedDate;
        private Win.UI.DateBox dateBoxReceivedDate;
        private Win.UI.DateBox dateBoxSubmitDate;
        private Class.TxtTechnician txtTechnician;
        private Win.UI.TextBox txtCombineStyle;
        private Win.UI.Button btnSendMR;
        private Win.UI.Button btnPDF;
    }
}
