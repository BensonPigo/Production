namespace Sci.Production.Quality
{
    partial class P05_Detail
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
            this.labelRemark = new Sci.Win.UI.Label();
            this.labelNoofTest = new Sci.Win.UI.Label();
            this.labelInspector = new Sci.Win.UI.Label();
            this.labelArticle = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.labelTestDate = new Sci.Win.UI.Label();
            this.txtNoofTest = new Sci.Win.UI.TextBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.dateTestDate = new Sci.Win.UI.DateBox();
            this.btnToPDF = new Sci.Win.UI.Button();
            this.txtuserInspector = new Sci.Production.Class.Txtuser();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnToPDF);
            this.btmcont.Controls.Add(this.btnToExcel);
            this.btmcont.Size = new System.Drawing.Size(1008, 40);
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
            this.gridcont.Location = new System.Drawing.Point(12, 88);
            this.gridcont.Size = new System.Drawing.Size(984, 359);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(918, 5);
            this.undo.TabIndex = 2;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(832, 5);
            this.save.Size = new System.Drawing.Size(86, 30);
            this.save.TabIndex = 1;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(436, 47);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 1;
            this.labelRemark.Text = "Remark";
            // 
            // labelNoofTest
            // 
            this.labelNoofTest.Location = new System.Drawing.Point(15, 12);
            this.labelNoofTest.Name = "labelNoofTest";
            this.labelNoofTest.Size = new System.Drawing.Size(108, 23);
            this.labelNoofTest.TabIndex = 2;
            this.labelNoofTest.Text = "No of Test";
            // 
            // labelInspector
            // 
            this.labelInspector.Location = new System.Drawing.Point(15, 47);
            this.labelInspector.Name = "labelInspector";
            this.labelInspector.Size = new System.Drawing.Size(108, 23);
            this.labelInspector.TabIndex = 3;
            this.labelInspector.Text = "Inspector";
            // 
            // labelArticle
            // 
            this.labelArticle.Location = new System.Drawing.Point(651, 12);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(53, 23);
            this.labelArticle.TabIndex = 4;
            this.labelArticle.Text = "Article";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(244, 12);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(43, 23);
            this.labelSP.TabIndex = 5;
            this.labelSP.Text = "SP #";
            // 
            // labelResult
            // 
            this.labelResult.Location = new System.Drawing.Point(843, 12);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(55, 23);
            this.labelResult.TabIndex = 6;
            this.labelResult.Text = "Result";
            // 
            // labelTestDate
            // 
            this.labelTestDate.Location = new System.Drawing.Point(436, 12);
            this.labelTestDate.Name = "labelTestDate";
            this.labelTestDate.Size = new System.Drawing.Size(75, 23);
            this.labelTestDate.TabIndex = 7;
            this.labelTestDate.Text = "TestDate ";
            // 
            // txtNoofTest
            // 
            this.txtNoofTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtNoofTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtNoofTest.IsSupportEditMode = false;
            this.txtNoofTest.Location = new System.Drawing.Point(126, 12);
            this.txtNoofTest.Name = "txtNoofTest";
            this.txtNoofTest.ReadOnly = true;
            this.txtNoofTest.Size = new System.Drawing.Size(100, 23);
            this.txtNoofTest.TabIndex = 0;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSP.IsSupportEditMode = false;
            this.txtSP.Location = new System.Drawing.Point(290, 12);
            this.txtSP.Name = "txtSP";
            this.txtSP.ReadOnly = true;
            this.txtSP.Size = new System.Drawing.Size(132, 23);
            this.txtSP.TabIndex = 1;
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(707, 12);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(122, 23);
            this.txtArticle.TabIndex = 3;
            this.txtArticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TxtArticle_MouseDown);
            this.txtArticle.Validating += new System.ComponentModel.CancelEventHandler(this.TxtArticle_Validating);
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboResult.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(901, 12);
            this.comboResult.Name = "comboResult";
            this.comboResult.OldText = "";
            this.comboResult.ReadOnly = true;
            this.comboResult.Size = new System.Drawing.Size(98, 24);
            this.comboResult.TabIndex = 4;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(514, 47);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(315, 23);
            this.txtRemark.TabIndex = 6;
            // 
            // btnEncode
            // 
            this.btnEncode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnEncode.Location = new System.Drawing.Point(843, 42);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(96, 30);
            this.btnEncode.TabIndex = 7;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.BtnEncode_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnToExcel.Location = new System.Drawing.Point(724, 5);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(101, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // dateTestDate
            // 
            this.dateTestDate.Location = new System.Drawing.Point(515, 12);
            this.dateTestDate.Name = "dateTestDate";
            this.dateTestDate.Size = new System.Drawing.Size(130, 23);
            this.dateTestDate.TabIndex = 98;
            this.dateTestDate.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // btnToPDF
            // 
            this.btnToPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnToPDF.Location = new System.Drawing.Point(617, 5);
            this.btnToPDF.Name = "btnToPDF";
            this.btnToPDF.Size = new System.Drawing.Size(101, 30);
            this.btnToPDF.TabIndex = 93;
            this.btnToPDF.Text = "To PDF";
            this.btnToPDF.UseVisualStyleBackColor = true;
            this.btnToPDF.Click += new System.EventHandler(this.BtnToPDF_Click);
            // 
            // txtuserInspector
            // 
            this.txtuserInspector.DisplayBox1Binding = "";
            this.txtuserInspector.Location = new System.Drawing.Point(126, 47);
            this.txtuserInspector.Name = "txtuserInspector";
            this.txtuserInspector.Size = new System.Drawing.Size(296, 23);
            this.txtuserInspector.TabIndex = 5;
            this.txtuserInspector.TextBox1Binding = "";
            this.txtuserInspector.Validating += new System.ComponentModel.CancelEventHandler(this.TxtuserInspector_Validating);
            // 
            // P05_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 497);
            this.Controls.Add(this.dateTestDate);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.txtuserInspector);
            this.Controls.Add(this.comboResult);
            this.Controls.Add(this.txtArticle);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.txtNoofTest);
            this.Controls.Add(this.labelRemark);
            this.Controls.Add(this.labelInspector);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.labelTestDate);
            this.Controls.Add(this.labelSP);
            this.Controls.Add(this.labelArticle);
            this.Controls.Add(this.labelNoofTest);
            this.DefaultOrder = "ovenGroup,SEQ1,SEQ2";
            this.EditMode = true;
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.Name = "P05_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Oven Test";
            this.WorkAlias = "Oven_Detail";
            this.Controls.SetChildIndex(this.labelNoofTest, 0);
            this.Controls.SetChildIndex(this.labelArticle, 0);
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelTestDate, 0);
            this.Controls.SetChildIndex(this.labelResult, 0);
            this.Controls.SetChildIndex(this.labelInspector, 0);
            this.Controls.SetChildIndex(this.labelRemark, 0);
            this.Controls.SetChildIndex(this.txtNoofTest, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.txtArticle, 0);
            this.Controls.SetChildIndex(this.comboResult, 0);
            this.Controls.SetChildIndex(this.txtuserInspector, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.dateTestDate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelRemark;
        private Win.UI.Label labelNoofTest;
        private Win.UI.Label labelInspector;
        private Win.UI.Label labelArticle;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelResult;
        private Win.UI.Label labelTestDate;
        private Win.UI.Button btnToExcel;
        private Win.UI.TextBox txtNoofTest;
        private Win.UI.TextBox txtSP;
        private Win.UI.TextBox txtArticle;
        private Win.UI.ComboBox comboResult;
        private Class.Txtuser txtuserInspector;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Button btnEncode;
        private Win.UI.DateBox dateTestDate;
        private Win.UI.Button btnToPDF;
    }
}
