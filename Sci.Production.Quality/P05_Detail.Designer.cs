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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.testno = new Sci.Win.UI.TextBox();
            this.poid = new Sci.Win.UI.TextBox();
            this.article = new Sci.Win.UI.TextBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.remark = new Sci.Win.UI.TextBox();
            this.encode_btn = new Sci.Win.UI.Button();
            this.ToExcel = new Sci.Win.UI.Button();
            this.txtuser1 = new Sci.Production.Class.txtuser();
            this.inspdate = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.ToExcel);
            this.btmcont.Size = new System.Drawing.Size(1080, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.ToExcel, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 113);
            this.gridcont.Size = new System.Drawing.Size(1056, 334);
            // 
            // append
            // 
            this.append.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // revise
            // 
            this.revise.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // delete
            // 
            this.delete.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // undo
            // 
            this.undo.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.undo.Location = new System.Drawing.Point(990, 5);
            this.undo.TabIndex = 2;
            // 
            // save
            // 
            this.save.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.save.Location = new System.Drawing.Point(904, 5);
            this.save.Size = new System.Drawing.Size(86, 30);
            this.save.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(449, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Remark :";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(28, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "No of Test :";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(28, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Inspector :";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(664, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Article :";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(248, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "SP #: ";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(865, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 6;
            this.label6.Text = "Result :";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(449, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "TestDate :";
            // 
            // testno
            // 
            this.testno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.testno.Enabled = false;
            this.testno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.testno.Location = new System.Drawing.Point(117, 23);
            this.testno.Name = "testno";
            this.testno.ReadOnly = true;
            this.testno.Size = new System.Drawing.Size(100, 23);
            this.testno.TabIndex = 0;
            // 
            // poid
            // 
            this.poid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.poid.Enabled = false;
            this.poid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.poid.Location = new System.Drawing.Point(326, 23);
            this.poid.Name = "poid";
            this.poid.ReadOnly = true;
            this.poid.Size = new System.Drawing.Size(100, 23);
            this.poid.TabIndex = 1;
            // 
            // article
            // 
            this.article.BackColor = System.Drawing.Color.White;
            this.article.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.article.Location = new System.Drawing.Point(742, 23);
            this.article.Name = "article";
            this.article.Size = new System.Drawing.Size(100, 23);
            this.article.TabIndex = 3;
            this.article.TextChanged += new System.EventHandler(this.TextChanged);
            this.article.MouseDown += new System.Windows.Forms.MouseEventHandler(this.article_MouseDown);
            this.article.Validated += new System.EventHandler(this.article_Validated);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboBox1.Enabled = false;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(943, 23);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.ReadOnly = true;
            this.comboBox1.Size = new System.Drawing.Size(103, 24);
            this.comboBox1.TabIndex = 4;
            // 
            // remark
            // 
            this.remark.BackColor = System.Drawing.Color.White;
            this.remark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.remark.Location = new System.Drawing.Point(527, 67);
            this.remark.Name = "remark";
            this.remark.Size = new System.Drawing.Size(315, 23);
            this.remark.TabIndex = 6;
            this.remark.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // encode_btn
            // 
            this.encode_btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.encode_btn.Location = new System.Drawing.Point(865, 60);
            this.encode_btn.Name = "encode_btn";
            this.encode_btn.Size = new System.Drawing.Size(96, 30);
            this.encode_btn.TabIndex = 7;
            this.encode_btn.Text = "Encode";
            this.encode_btn.UseVisualStyleBackColor = true;
            this.encode_btn.Click += new System.EventHandler(this.encode_btn_Click);
            // 
            // ToExcel
            // 
            this.ToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.ToExcel.Location = new System.Drawing.Point(784, 5);
            this.ToExcel.Name = "ToExcel";
            this.ToExcel.Size = new System.Drawing.Size(101, 30);
            this.ToExcel.TabIndex = 0;
            this.ToExcel.Text = "To Excel";
            this.ToExcel.UseVisualStyleBackColor = true;
            this.ToExcel.Click += new System.EventHandler(this.ToExcel_Click);
            // 
            // txtuser1
            // 
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Location = new System.Drawing.Point(117, 67);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(296, 23);
            this.txtuser1.TabIndex = 5;
            this.txtuser1.TextBox1Binding = "";
            this.txtuser1.Validating += new System.ComponentModel.CancelEventHandler(this.txtuser1_Validating);
            // 
            // inspdate
            // 
            this.inspdate.Location = new System.Drawing.Point(528, 23);
            this.inspdate.Name = "inspdate";
            this.inspdate.Size = new System.Drawing.Size(130, 23);
            this.inspdate.TabIndex = 98;
            this.inspdate.TextChanged += new System.EventHandler(this.TextChanged);
            // 
            // P05_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1080, 497);
            this.Controls.Add(this.inspdate);
            this.Controls.Add(this.encode_btn);
            this.Controls.Add(this.remark);
            this.Controls.Add(this.txtuser1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.article);
            this.Controls.Add(this.poid);
            this.Controls.Add(this.testno);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.DefaultOrder = "ovenGroup,SEQ1,SEQ2";
            this.EditMode = true;
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.Name = "P05_Detail";
            this.Text = "P05_Detail";
            this.WorkAlias = "Oven_Detail";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.testno, 0);
            this.Controls.SetChildIndex(this.poid, 0);
            this.Controls.SetChildIndex(this.article, 0);
            this.Controls.SetChildIndex(this.comboBox1, 0);
            this.Controls.SetChildIndex(this.txtuser1, 0);
            this.Controls.SetChildIndex(this.remark, 0);
            this.Controls.SetChildIndex(this.encode_btn, 0);
            this.Controls.SetChildIndex(this.inspdate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Button ToExcel;
        private Win.UI.TextBox testno;
        private Win.UI.TextBox poid;
        private Win.UI.TextBox article;
        private Win.UI.ComboBox comboBox1;
        private Class.txtuser txtuser1;
        private Win.UI.TextBox remark;
        private Win.UI.Button encode_btn;
        private Win.UI.DateBox inspdate;
    }
}
