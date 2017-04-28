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
            this.txtuserInspector = new Sci.Production.Class.txtuser();
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
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnToExcel);
            this.btmcont.Size = new System.Drawing.Size(1080, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnToExcel, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 113);
            this.gridcont.Size = new System.Drawing.Size(1056, 322);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(990, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(910, 5);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnToExcel.Location = new System.Drawing.Point(784, 5);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(101, 30);
            this.btnToExcel.TabIndex = 95;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // dateTestDate
            // 
            this.dateTestDate.Location = new System.Drawing.Point(528, 23);
            this.dateTestDate.Name = "dateTestDate";
            this.dateTestDate.Size = new System.Drawing.Size(130, 23);
            this.dateTestDate.TabIndex = 112;
            this.dateTestDate.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // btnEncode
            // 
            this.btnEncode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnEncode.Location = new System.Drawing.Point(865, 62);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(96, 30);
            this.btnEncode.TabIndex = 110;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(527, 67);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(315, 23);
            this.txtRemark.TabIndex = 108;
            // 
            // txtuserInspector
            // 
            this.txtuserInspector.DisplayBox1Binding = "";
            this.txtuserInspector.Location = new System.Drawing.Point(109, 67);
            this.txtuserInspector.Name = "txtuserInspector";
            this.txtuserInspector.Size = new System.Drawing.Size(296, 23);
            this.txtuserInspector.TabIndex = 106;
            this.txtuserInspector.TextBox1Binding = "";
            this.txtuserInspector.Validating += new System.ComponentModel.CancelEventHandler(this.txtuserInspector_Validating);
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboResult.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(922, 23);
            this.comboResult.Name = "comboResult";
            this.comboResult.ReadOnly = true;
            this.comboResult.Size = new System.Drawing.Size(89, 24);
            this.comboResult.TabIndex = 104;
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.Location = new System.Drawing.Point(719, 23);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(123, 23);
            this.txtArticle.TabIndex = 102;
            this.txtArticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtArticle_MouseDown);
            this.txtArticle.Validating += new System.ComponentModel.CancelEventHandler(this.txtArticle_Validating);
            // 
            // txtNoofTest
            // 
            this.txtNoofTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtNoofTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtNoofTest.IsSupportEditMode = false;
            this.txtNoofTest.Location = new System.Drawing.Point(109, 23);
            this.txtNoofTest.Name = "txtNoofTest";
            this.txtNoofTest.ReadOnly = true;
            this.txtNoofTest.Size = new System.Drawing.Size(81, 23);
            this.txtNoofTest.TabIndex = 99;
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(449, 67);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(75, 23);
            this.labelRemark.TabIndex = 100;
            this.labelRemark.Text = "Remark ";
            // 
            // labelInspector
            // 
            this.labelInspector.Lines = 0;
            this.labelInspector.Location = new System.Drawing.Point(28, 67);
            this.labelInspector.Name = "labelInspector";
            this.labelInspector.Size = new System.Drawing.Size(74, 23);
            this.labelInspector.TabIndex = 103;
            this.labelInspector.Text = "Inspector";
            // 
            // labelResult
            // 
            this.labelResult.Lines = 0;
            this.labelResult.Location = new System.Drawing.Point(865, 23);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(54, 23);
            this.labelResult.TabIndex = 109;
            this.labelResult.Text = "Result ";
            // 
            // labelTestDate
            // 
            this.labelTestDate.Lines = 0;
            this.labelTestDate.Location = new System.Drawing.Point(449, 23);
            this.labelTestDate.Name = "labelTestDate";
            this.labelTestDate.Size = new System.Drawing.Size(75, 23);
            this.labelTestDate.TabIndex = 111;
            this.labelTestDate.Text = "TestDate ";
            // 
            // labelSP
            // 
            this.labelSP.Lines = 0;
            this.labelSP.Location = new System.Drawing.Point(205, 23);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(45, 23);
            this.labelSP.TabIndex = 107;
            this.labelSP.Text = "SP #";
            // 
            // labelArticle
            // 
            this.labelArticle.Lines = 0;
            this.labelArticle.Location = new System.Drawing.Point(664, 23);
            this.labelArticle.Name = "labelArticle";
            this.labelArticle.Size = new System.Drawing.Size(52, 23);
            this.labelArticle.TabIndex = 105;
            this.labelArticle.Text = "Article ";
            // 
            // labelNoofTest
            // 
            this.labelNoofTest.Lines = 0;
            this.labelNoofTest.Location = new System.Drawing.Point(28, 23);
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
            this.txtSP.Location = new System.Drawing.Point(253, 23);
            this.txtSP.Name = "txtSP";
            this.txtSP.ReadOnly = true;
            this.txtSP.Size = new System.Drawing.Size(153, 23);
            this.txtSP.TabIndex = 113;
            // 
            // P06_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1080, 497);
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
        private Class.txtuser txtuserInspector;
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
    }
}
