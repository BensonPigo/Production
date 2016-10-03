namespace Sci.Production.Warehouse
{
    partial class P10_Detail
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
            this.dis_ID = new Sci.Win.UI.DisplayBox();
            this.dis_poid = new Sci.Win.UI.DisplayBox();
            this.dis_scirefno = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.dis_colorid = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.dis_desc = new Sci.Win.UI.DisplayBox();
            this.label5 = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.dis_sizespec = new Sci.Win.UI.DisplayBox();
            this.label11 = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.num_variance = new Sci.Win.UI.NumericBox();
            this.num_issue = new Sci.Win.UI.NumericBox();
            this.num_balance = new Sci.Win.UI.NumericBox();
            this.num_accuIssue = new Sci.Win.UI.NumericBox();
            this.num_requestqty = new Sci.Win.UI.NumericBox();
            this.label9 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.btnAutoPick = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 223);
            this.gridcont.Size = new System.Drawing.Size(984, 284);
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnAutoPick);
            this.btmcont.Location = new System.Drawing.Point(0, 517);
            this.btmcont.Size = new System.Drawing.Size(1008, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.next, 0);
            this.btmcont.Controls.SetChildIndex(this.prev, 0);
            this.btmcont.Controls.SetChildIndex(this.btnAutoPick, 0);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(838, 5);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(918, 5);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(783, 5);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(728, 5);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(14, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(243, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "SP#";
            // 
            // dis_ID
            // 
            this.dis_ID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_ID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_ID.Location = new System.Drawing.Point(92, 29);
            this.dis_ID.Name = "dis_ID";
            this.dis_ID.Size = new System.Drawing.Size(124, 23);
            this.dis_ID.TabIndex = 101;
            // 
            // dis_poid
            // 
            this.dis_poid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_poid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_poid.Location = new System.Drawing.Point(321, 29);
            this.dis_poid.Name = "dis_poid";
            this.dis_poid.Size = new System.Drawing.Size(124, 23);
            this.dis_poid.TabIndex = 102;
            // 
            // dis_scirefno
            // 
            this.dis_scirefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_scirefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_scirefno.Location = new System.Drawing.Point(548, 29);
            this.dis_scirefno.Name = "dis_scirefno";
            this.dis_scirefno.Size = new System.Drawing.Size(124, 23);
            this.dis_scirefno.TabIndex = 104;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(470, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 103;
            this.label3.Text = "SciRefno";
            // 
            // dis_colorid
            // 
            this.dis_colorid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_colorid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_colorid.Location = new System.Drawing.Point(774, 29);
            this.dis_colorid.Name = "dis_colorid";
            this.dis_colorid.Size = new System.Drawing.Size(124, 23);
            this.dis_colorid.TabIndex = 106;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(696, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 105;
            this.label4.Text = "ColorID";
            // 
            // dis_desc
            // 
            this.dis_desc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_desc.Location = new System.Drawing.Point(321, 62);
            this.dis_desc.Name = "dis_desc";
            this.dis_desc.Size = new System.Drawing.Size(453, 23);
            this.dis_desc.TabIndex = 108;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(243, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 107;
            this.label5.Text = "Desc";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dis_sizespec);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dis_colorid);
            this.groupBox1.Controls.Add(this.dis_desc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dis_ID);
            this.groupBox1.Controls.Add(this.dis_poid);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dis_scirefno);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 100);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Isse Item";
            // 
            // dis_sizespec
            // 
            this.dis_sizespec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_sizespec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_sizespec.Location = new System.Drawing.Point(92, 62);
            this.dis_sizespec.Name = "dis_sizespec";
            this.dis_sizespec.Size = new System.Drawing.Size(124, 23);
            this.dis_sizespec.TabIndex = 110;
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(14, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 23);
            this.label11.TabIndex = 109;
            this.label11.Text = "SizeSpec";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.num_variance);
            this.groupBox2.Controls.Add(this.num_issue);
            this.groupBox2.Controls.Add(this.num_balance);
            this.groupBox2.Controls.Add(this.num_accuIssue);
            this.groupBox2.Controls.Add(this.num_requestqty);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(10, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(984, 100);
            this.groupBox2.TabIndex = 111;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cutting Plan Info.";
            // 
            // num_variance
            // 
            this.num_variance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.num_variance.DecimalPlaces = 2;
            this.num_variance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.num_variance.IsSupportEditMode = false;
            this.num_variance.Location = new System.Drawing.Point(563, 62);
            this.num_variance.Name = "num_variance";
            this.num_variance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.num_variance.ReadOnly = true;
            this.num_variance.Size = new System.Drawing.Size(111, 23);
            this.num_variance.TabIndex = 113;
            this.num_variance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // num_issue
            // 
            this.num_issue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.num_issue.DecimalPlaces = 2;
            this.num_issue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.num_issue.IsSupportEditMode = false;
            this.num_issue.Location = new System.Drawing.Point(336, 62);
            this.num_issue.Name = "num_issue";
            this.num_issue.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.num_issue.ReadOnly = true;
            this.num_issue.Size = new System.Drawing.Size(111, 23);
            this.num_issue.TabIndex = 112;
            this.num_issue.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // num_balance
            // 
            this.num_balance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.num_balance.DecimalPlaces = 2;
            this.num_balance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.num_balance.IsSupportEditMode = false;
            this.num_balance.Location = new System.Drawing.Point(563, 29);
            this.num_balance.Name = "num_balance";
            this.num_balance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.num_balance.ReadOnly = true;
            this.num_balance.Size = new System.Drawing.Size(111, 23);
            this.num_balance.TabIndex = 111;
            this.num_balance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // num_accuIssue
            // 
            this.num_accuIssue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.num_accuIssue.DecimalPlaces = 2;
            this.num_accuIssue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.num_accuIssue.IsSupportEditMode = false;
            this.num_accuIssue.Location = new System.Drawing.Point(336, 29);
            this.num_accuIssue.Name = "num_accuIssue";
            this.num_accuIssue.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.num_accuIssue.ReadOnly = true;
            this.num_accuIssue.Size = new System.Drawing.Size(111, 23);
            this.num_accuIssue.TabIndex = 110;
            this.num_accuIssue.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // num_requestqty
            // 
            this.num_requestqty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.num_requestqty.DecimalPlaces = 2;
            this.num_requestqty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.num_requestqty.IsSupportEditMode = false;
            this.num_requestqty.Location = new System.Drawing.Point(107, 29);
            this.num_requestqty.Name = "num_requestqty";
            this.num_requestqty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.num_requestqty.ReadOnly = true;
            this.num_requestqty.Size = new System.Drawing.Size(111, 23);
            this.num_requestqty.TabIndex = 109;
            this.num_requestqty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(470, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 23);
            this.label9.TabIndex = 107;
            this.label9.Text = "Variance";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(14, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 23);
            this.label6.TabIndex = 99;
            this.label6.Text = "Request Qty";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(243, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 23);
            this.label7.TabIndex = 105;
            this.label7.Text = "Issue Qty";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(243, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 23);
            this.label8.TabIndex = 100;
            this.label8.Text = "Accu. Issued";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(470, 29);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 23);
            this.label10.TabIndex = 103;
            this.label10.Text = "Bal. Qty";
            // 
            // btnAutoPick
            // 
            this.btnAutoPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoPick.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAutoPick.Location = new System.Drawing.Point(642, 5);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 97;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.btnAutoPick_Click);
            // 
            // P10_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 557);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "P10_Detail";
            this.Text = "P10. Issue Fabric Detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox dis_ID;
        private Win.UI.DisplayBox dis_poid;
        private Win.UI.DisplayBox dis_scirefno;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox dis_colorid;
        private Win.UI.Label label4;
        private Win.UI.DisplayBox dis_desc;
        private Win.UI.Label label5;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Label label9;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.Label label10;
        private Win.UI.DisplayBox dis_sizespec;
        private Win.UI.Label label11;
        private Win.UI.NumericBox num_variance;
        private Win.UI.NumericBox num_issue;
        private Win.UI.NumericBox num_balance;
        private Win.UI.NumericBox num_accuIssue;
        private Win.UI.NumericBox num_requestqty;
        private Win.UI.Button btnAutoPick;
    }
}
