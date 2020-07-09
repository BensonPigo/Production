namespace Sci.Production.Quality
{
    partial class P06_Detail
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
            this.btnToExcel = new Sci.Win.UI.Button();
            this.dateTestDate = new Sci.Win.UI.DateBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.txtuserInspector = new Sci.Production.Class.Txtuser();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.txtNoofTest = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelInspector = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.labelTestDate = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelArticle = new Sci.Win.UI.Label();
            this.labelNoofTest = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.comboTempt = new Sci.Win.UI.ComboBox();
            this.comboMachineUs = new Sci.Win.UI.ComboBox();
            this.comboCycle = new Sci.Win.UI.ComboBox();
            this.comboDryProcess = new Sci.Win.UI.ComboBox();
            this.comboDetergent = new Sci.Win.UI.ComboBox();
            this.btnToPDF = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnToPDF);
            this.btmcont.Controls.Add(this.btnToExcel);
            this.btmcont.Location = new System.Drawing.Point(0, 571);
            this.btmcont.Size = new System.Drawing.Size(1027, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnToExcel, 0);
            this.btmcont.Controls.SetChildIndex(this.btnToPDF, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 131);
            this.gridcont.Size = new System.Drawing.Size(1003, 418);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(937, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(857, 5);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnToExcel.Location = new System.Drawing.Point(733, 5);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(101, 30);
            this.btnToExcel.TabIndex = 95;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // dateTestDate
            // 
            this.dateTestDate.Location = new System.Drawing.Point(487, 10);
            this.dateTestDate.Name = "dateTestDate";
            this.dateTestDate.Size = new System.Drawing.Size(152, 23);
            this.dateTestDate.TabIndex = 112;
            // 
            // btnEncode
            // 
            this.btnEncode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnEncode.Location = new System.Drawing.Point(857, 95);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(120, 30);
            this.btnEncode.TabIndex = 110;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(97, 97);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(297, 23);
            this.txtRemark.TabIndex = 108;
            // 
            // txtuserInspector
            // 
            this.txtuserInspector.DisplayBox1Binding = "";
            this.txtuserInspector.Location = new System.Drawing.Point(97, 39);
            this.txtuserInspector.Name = "txtuserInspector";
            this.txtuserInspector.Size = new System.Drawing.Size(296, 23);
            this.txtuserInspector.TabIndex = 106;
            this.txtuserInspector.TextBox1Binding = "";
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboResult.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(930, 10);
            this.comboResult.Name = "comboResult";
            this.comboResult.ReadOnly = true;
            this.comboResult.Size = new System.Drawing.Size(89, 24);
            this.comboResult.TabIndex = 104;
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(707, 10);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(123, 23);
            this.txtArticle.TabIndex = 102;
            this.txtArticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtArticle_MouseDown);
            // 
            // txtNoofTest
            // 
            this.txtNoofTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtNoofTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtNoofTest.IsSupportEditMode = false;
            this.txtNoofTest.Location = new System.Drawing.Point(97, 10);
            this.txtNoofTest.Name = "txtNoofTest";
            this.txtNoofTest.ReadOnly = true;
            this.txtNoofTest.Size = new System.Drawing.Size(81, 23);
            this.txtNoofTest.TabIndex = 99;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(16, 97);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(74, 23);
            this.labelRemark.TabIndex = 100;
            this.labelRemark.Text = "Remark ";
            // 
            // labelInspector
            // 
            this.labelInspector.Location = new System.Drawing.Point(16, 39);
            this.labelInspector.Name = "labelInspector";
            this.labelInspector.Size = new System.Drawing.Size(74, 23);
            this.labelInspector.TabIndex = 103;
            this.labelInspector.Text = "Inspector";
            // 
            // labelResult
            // 
            this.labelResult.Location = new System.Drawing.Point(857, 10);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(70, 23);
            this.labelResult.TabIndex = 109;
            this.labelResult.Text = "Result ";
            // 
            // labelTestDate
            // 
            this.labelTestDate.Location = new System.Drawing.Point(408, 10);
            this.labelTestDate.Name = "labelTestDate";
            this.labelTestDate.Size = new System.Drawing.Size(75, 23);
            this.labelTestDate.TabIndex = 111;
            this.labelTestDate.Text = "TestDate ";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(193, 10);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(45, 23);
            this.labelSP.TabIndex = 107;
            this.labelSP.Text = "SP #";
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(652, 10);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(52, 23);
            this.labelArticle.TabIndex = 105;
            this.labelArticle.Text = "Article ";
            // 
            // labelNoofTest
            // 
            this.labelNoofTest.Location = new System.Drawing.Point(16, 10);
            this.labelNoofTest.Name = "labelNoofTest";
            this.labelNoofTest.Size = new System.Drawing.Size(74, 23);
            this.labelNoofTest.TabIndex = 101;
            this.labelNoofTest.Text = "No of Test";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSP.IsSupportEditMode = false;
            this.txtSP.Location = new System.Drawing.Point(241, 10);
            this.txtSP.Name = "txtSP";
            this.txtSP.ReadOnly = true;
            this.txtSP.Size = new System.Drawing.Size(153, 23);
            this.txtSP.TabIndex = 113;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 23);
            this.label1.TabIndex = 114;
            this.label1.Text = "Detergent";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(408, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 23);
            this.label2.TabIndex = 115;
            this.label2.Text = "Temperature(˚C)";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(408, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 23);
            this.label3.TabIndex = 116;
            this.label3.Text = "Machine Used";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(652, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 23);
            this.label4.TabIndex = 117;
            this.label4.Text = "Cycle";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(652, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 23);
            this.label5.TabIndex = 118;
            this.label5.Text = "Drying Process";
            // 
            // comboTempt
            // 
            this.comboTempt.BackColor = System.Drawing.Color.White;
            this.comboTempt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTempt.FormattingEnabled = true;
            this.comboTempt.IsSupportUnselect = true;
            this.comboTempt.Items.AddRange(new object[] {
            "0",
            "30",
            "40",
            "50",
            "60"});
            this.comboTempt.Location = new System.Drawing.Point(520, 39);
            this.comboTempt.Name = "comboTempt";
            this.comboTempt.Size = new System.Drawing.Size(119, 24);
            this.comboTempt.TabIndex = 119;
            // 
            // comboMachineUs
            // 
            this.comboMachineUs.BackColor = System.Drawing.Color.White;
            this.comboMachineUs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMachineUs.FormattingEnabled = true;
            this.comboMachineUs.IsSupportUnselect = true;
            this.comboMachineUs.Items.AddRange(new object[] {
            "",
            "Top Load",
            "Front Load"});
            this.comboMachineUs.Location = new System.Drawing.Point(520, 68);
            this.comboMachineUs.Name = "comboMachineUs";
            this.comboMachineUs.Size = new System.Drawing.Size(119, 24);
            this.comboMachineUs.TabIndex = 120;
            // 
            // comboCycle
            // 
            this.comboCycle.BackColor = System.Drawing.Color.White;
            this.comboCycle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCycle.FormattingEnabled = true;
            this.comboCycle.IsSupportUnselect = true;
            this.comboCycle.Items.AddRange(new object[] {
            "0",
            "3",
            "5",
            "10",
            "15",
            "25"});
            this.comboCycle.Location = new System.Drawing.Point(756, 38);
            this.comboCycle.Name = "comboCycle";
            this.comboCycle.Size = new System.Drawing.Size(171, 24);
            this.comboCycle.TabIndex = 121;
            // 
            // comboDryProcess
            // 
            this.comboDryProcess.BackColor = System.Drawing.Color.White;
            this.comboDryProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDryProcess.FormattingEnabled = true;
            this.comboDryProcess.IsSupportUnselect = true;
            this.comboDryProcess.Items.AddRange(new object[] {
            "",
            "Line dry",
            "Tumble Dry Low",
            "Tumble dry Medium",
            "Tumble dry High"});
            this.comboDryProcess.Location = new System.Drawing.Point(756, 65);
            this.comboDryProcess.Name = "comboDryProcess";
            this.comboDryProcess.Size = new System.Drawing.Size(171, 24);
            this.comboDryProcess.TabIndex = 122;
            // 
            // comboDetergent
            // 
            this.comboDetergent.BackColor = System.Drawing.Color.White;
            this.comboDetergent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDetergent.FormattingEnabled = true;
            this.comboDetergent.IsSupportUnselect = true;
            this.comboDetergent.Items.AddRange(new object[] {
            "",
            "Woolite",
            "Tide",
            "AATCC"});
            this.comboDetergent.Location = new System.Drawing.Point(97, 68);
            this.comboDetergent.Name = "comboDetergent";
            this.comboDetergent.Size = new System.Drawing.Size(141, 24);
            this.comboDetergent.TabIndex = 123;
            // 
            // btnToPDF
            // 
            this.btnToPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnToPDF.Location = new System.Drawing.Point(626, 5);
            this.btnToPDF.Name = "btnToPDF";
            this.btnToPDF.Size = new System.Drawing.Size(101, 30);
            this.btnToPDF.TabIndex = 96;
            this.btnToPDF.Text = "To PDF";
            this.btnToPDF.UseVisualStyleBackColor = true;
            this.btnToPDF.Click += new System.EventHandler(this.btnToPDF_Click);
            // 
            // P06_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1027, 611);
            this.Controls.Add(this.comboDetergent);
            this.Controls.Add(this.comboDryProcess);
            this.Controls.Add(this.comboCycle);
            this.Controls.Add(this.comboMachineUs);
            this.Controls.Add(this.comboTempt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.dateTestDate);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.txtuserInspector);
            this.Controls.Add(this.comboResult);
            this.Controls.Add(this.txtArticle);
            this.Controls.Add(this.txtNoofTest);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.labelInspector);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelTestDate);
            this.Controls.Add(this.labelSP);
            this.Controls.Add(this.labelArticle);
            this.Controls.Add(this.labelNoofTest);
            this.DefaultOrder = "ColorFastnessGroup,SEQ1,SEQ2";
            this.EditMode = true;
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.Name = "P06_Detail";
            this.Text = "Color Fastness";
            this.WorkAlias = "ColorFastness_Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelNoofTest, 0);
            this.Controls.SetChildIndex(this.labelArticle, 0);
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.labelTestDate, 0);
            this.Controls.SetChildIndex(this.labelResult, 0);
            this.Controls.SetChildIndex(this.labelInspector, 0);
            this.Controls.SetChildIndex(this.labelRemark, 0);
            this.Controls.SetChildIndex(this.txtNoofTest, 0);
            this.Controls.SetChildIndex(this.txtArticle, 0);
            this.Controls.SetChildIndex(this.comboResult, 0);
            this.Controls.SetChildIndex(this.txtuserInspector, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.dateTestDate, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.comboTempt, 0);
            this.Controls.SetChildIndex(this.comboMachineUs, 0);
            this.Controls.SetChildIndex(this.comboCycle, 0);
            this.Controls.SetChildIndex(this.comboDryProcess, 0);
            this.Controls.SetChildIndex(this.comboDetergent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnToExcel;
        private Win.UI.DateBox dateTestDate;
        private Win.UI.Button btnEncode;
        private Win.UI.TextBox txtRemark;
        private Class.Txtuser txtuserInspector;
        private Win.UI.ComboBox comboResult;
        private Win.UI.TextBox txtArticle;
        private Win.UI.TextBox txtNoofTest;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelInspector;
        private Win.UI.Label labelResult;
        private Win.UI.Label labelTestDate;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelArticle;
        private Win.UI.Label labelNoofTest;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.ComboBox comboTempt;
        private Win.UI.ComboBox comboMachineUs;
        private Win.UI.ComboBox comboCycle;
        private Win.UI.ComboBox comboDryProcess;
        private Win.UI.ComboBox comboDetergent;
        private Win.UI.Button btnToPDF;
    }
}
