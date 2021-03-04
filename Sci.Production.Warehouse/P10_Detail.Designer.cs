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
            this.labelID = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.displayColorID = new Sci.Win.UI.DisplayBox();
            this.labelColorID = new Sci.Win.UI.Label();
            this.displayDesc = new Sci.Win.UI.DisplayBox();
            this.labelDesc = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label1 = new Sci.Win.UI.Label();
            this.displaySCIRefno = new Sci.Win.UI.DisplayBox();
            this.displaySizeSpec = new Sci.Win.UI.DisplayBox();
            this.labelSizeSpec = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.numVariance = new Sci.Win.UI.NumericBox();
            this.numIssueQty = new Sci.Win.UI.NumericBox();
            this.numBalanceQty = new Sci.Win.UI.NumericBox();
            this.numAccuIssue = new Sci.Win.UI.NumericBox();
            this.numRequestQty = new Sci.Win.UI.NumericBox();
            this.labelVariance = new Sci.Win.UI.Label();
            this.labelRequestQty = new Sci.Win.UI.Label();
            this.labelIssueQty = new Sci.Win.UI.Label();
            this.labelAccuIssue = new Sci.Win.UI.Label();
            this.labelBalanceQty = new Sci.Win.UI.Label();
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
            // delete
            // 
            this.delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(783, 5);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(728, 5);
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(14, 29);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(75, 23);
            this.labelID.TabIndex = 99;
            this.labelID.Text = "ID";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(14, 64);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 100;
            this.labelSPNo.Text = "SP#";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(92, 29);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(124, 23);
            this.displayID.TabIndex = 101;
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(92, 64);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(124, 23);
            this.displaySPNo.TabIndex = 102;
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(618, 29);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(124, 23);
            this.displayRefno.TabIndex = 104;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(540, 29);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 103;
            this.labelRefno.Text = "Refno";
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(836, 29);
            this.displayColorID.Name = "displayColorID";
            this.displayColorID.Size = new System.Drawing.Size(124, 23);
            this.displayColorID.TabIndex = 106;
            // 
            // labelColorID
            // 
            this.labelColorID.Location = new System.Drawing.Point(758, 29);
            this.labelColorID.Name = "labelColorID";
            this.labelColorID.Size = new System.Drawing.Size(75, 23);
            this.labelColorID.TabIndex = 105;
            this.labelColorID.Text = "ColorID";
            // 
            // displayDesc
            // 
            this.displayDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDesc.Location = new System.Drawing.Point(308, 64);
            this.displayDesc.Name = "displayDesc";
            this.displayDesc.Size = new System.Drawing.Size(652, 23);
            this.displayDesc.TabIndex = 108;
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(230, 64);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(75, 23);
            this.labelDesc.TabIndex = 107;
            this.labelDesc.Text = "Desc";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.displaySCIRefno);
            this.groupBox1.Controls.Add(this.labelID);
            this.groupBox1.Controls.Add(this.displayColorID);
            this.groupBox1.Controls.Add(this.displayDesc);
            this.groupBox1.Controls.Add(this.labelColorID);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.labelDesc);
            this.groupBox1.Controls.Add(this.displayID);
            this.groupBox1.Controls.Add(this.displaySPNo);
            this.groupBox1.Controls.Add(this.labelRefno);
            this.groupBox1.Controls.Add(this.displayRefno);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 100);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Isse Item";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(230, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 114;
            this.label1.Text = "SCIRefno";
            // 
            // displaySCIRefno
            // 
            this.displaySCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Location = new System.Drawing.Point(308, 29);
            this.displaySCIRefno.Name = "displaySCIRefno";
            this.displaySCIRefno.Size = new System.Drawing.Size(220, 23);
            this.displaySCIRefno.TabIndex = 115;
            // 
            // displaySizeSpec
            // 
            this.displaySizeSpec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.Location = new System.Drawing.Point(798, 29);
            this.displaySizeSpec.Name = "displaySizeSpec";
            this.displaySizeSpec.Size = new System.Drawing.Size(124, 23);
            this.displaySizeSpec.TabIndex = 110;
            this.displaySizeSpec.Visible = false;
            // 
            // labelSizeSpec
            // 
            this.labelSizeSpec.Location = new System.Drawing.Point(720, 29);
            this.labelSizeSpec.Name = "labelSizeSpec";
            this.labelSizeSpec.Size = new System.Drawing.Size(75, 23);
            this.labelSizeSpec.TabIndex = 109;
            this.labelSizeSpec.Text = "SizeSpec";
            this.labelSizeSpec.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numVariance);
            this.groupBox2.Controls.Add(this.numIssueQty);
            this.groupBox2.Controls.Add(this.displaySizeSpec);
            this.groupBox2.Controls.Add(this.labelSizeSpec);
            this.groupBox2.Controls.Add(this.numBalanceQty);
            this.groupBox2.Controls.Add(this.numAccuIssue);
            this.groupBox2.Controls.Add(this.numRequestQty);
            this.groupBox2.Controls.Add(this.labelVariance);
            this.groupBox2.Controls.Add(this.labelRequestQty);
            this.groupBox2.Controls.Add(this.labelIssueQty);
            this.groupBox2.Controls.Add(this.labelAccuIssue);
            this.groupBox2.Controls.Add(this.labelBalanceQty);
            this.groupBox2.Location = new System.Drawing.Point(10, 117);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(984, 100);
            this.groupBox2.TabIndex = 111;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cutting Plan Info.";
            // 
            // numVariance
            // 
            this.numVariance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numVariance.DecimalPlaces = 2;
            this.numVariance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numVariance.IsSupportEditMode = false;
            this.numVariance.Location = new System.Drawing.Point(563, 62);
            this.numVariance.Name = "numVariance";
            this.numVariance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numVariance.ReadOnly = true;
            this.numVariance.Size = new System.Drawing.Size(111, 23);
            this.numVariance.TabIndex = 113;
            this.numVariance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numIssueQty
            // 
            this.numIssueQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numIssueQty.DecimalPlaces = 2;
            this.numIssueQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numIssueQty.IsSupportEditMode = false;
            this.numIssueQty.Location = new System.Drawing.Point(336, 62);
            this.numIssueQty.Name = "numIssueQty";
            this.numIssueQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numIssueQty.ReadOnly = true;
            this.numIssueQty.Size = new System.Drawing.Size(111, 23);
            this.numIssueQty.TabIndex = 112;
            this.numIssueQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numBalanceQty
            // 
            this.numBalanceQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numBalanceQty.DecimalPlaces = 2;
            this.numBalanceQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numBalanceQty.IsSupportEditMode = false;
            this.numBalanceQty.Location = new System.Drawing.Point(563, 29);
            this.numBalanceQty.Name = "numBalanceQty";
            this.numBalanceQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numBalanceQty.ReadOnly = true;
            this.numBalanceQty.Size = new System.Drawing.Size(111, 23);
            this.numBalanceQty.TabIndex = 111;
            this.numBalanceQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numAccuIssue
            // 
            this.numAccuIssue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAccuIssue.DecimalPlaces = 2;
            this.numAccuIssue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAccuIssue.IsSupportEditMode = false;
            this.numAccuIssue.Location = new System.Drawing.Point(336, 29);
            this.numAccuIssue.Name = "numAccuIssue";
            this.numAccuIssue.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAccuIssue.ReadOnly = true;
            this.numAccuIssue.Size = new System.Drawing.Size(111, 23);
            this.numAccuIssue.TabIndex = 110;
            this.numAccuIssue.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numRequestQty
            // 
            this.numRequestQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numRequestQty.DecimalPlaces = 2;
            this.numRequestQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numRequestQty.IsSupportEditMode = false;
            this.numRequestQty.Location = new System.Drawing.Point(107, 29);
            this.numRequestQty.Name = "numRequestQty";
            this.numRequestQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numRequestQty.ReadOnly = true;
            this.numRequestQty.Size = new System.Drawing.Size(111, 23);
            this.numRequestQty.TabIndex = 109;
            this.numRequestQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelVariance
            // 
            this.labelVariance.Location = new System.Drawing.Point(470, 62);
            this.labelVariance.Name = "labelVariance";
            this.labelVariance.Size = new System.Drawing.Size(90, 23);
            this.labelVariance.TabIndex = 107;
            this.labelVariance.Text = "Variance";
            // 
            // labelRequestQty
            // 
            this.labelRequestQty.Location = new System.Drawing.Point(14, 29);
            this.labelRequestQty.Name = "labelRequestQty";
            this.labelRequestQty.Size = new System.Drawing.Size(90, 23);
            this.labelRequestQty.TabIndex = 99;
            this.labelRequestQty.Text = "Request Qty";
            // 
            // labelIssueQty
            // 
            this.labelIssueQty.Location = new System.Drawing.Point(243, 62);
            this.labelIssueQty.Name = "labelIssueQty";
            this.labelIssueQty.Size = new System.Drawing.Size(90, 23);
            this.labelIssueQty.TabIndex = 105;
            this.labelIssueQty.Text = "Issue Qty";
            // 
            // labelAccuIssue
            // 
            this.labelAccuIssue.Location = new System.Drawing.Point(243, 29);
            this.labelAccuIssue.Name = "labelAccuIssue";
            this.labelAccuIssue.Size = new System.Drawing.Size(90, 23);
            this.labelAccuIssue.TabIndex = 100;
            this.labelAccuIssue.Text = "Accu. Issued";
            // 
            // labelBalanceQty
            // 
            this.labelBalanceQty.Location = new System.Drawing.Point(470, 29);
            this.labelBalanceQty.Name = "labelBalanceQty";
            this.labelBalanceQty.Size = new System.Drawing.Size(90, 23);
            this.labelBalanceQty.TabIndex = 103;
            this.labelBalanceQty.Text = "Bal. Qty";
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
            this.btnAutoPick.Click += new System.EventHandler(this.BtnAutoPick_Click);
            // 
            // P10_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 557);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "P10_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Input8A";
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

        private Win.UI.Label labelID;
        private Win.UI.Label labelSPNo;
        private Win.UI.DisplayBox displayID;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.Label labelRefno;
        private Win.UI.DisplayBox displayColorID;
        private Win.UI.Label labelColorID;
        private Win.UI.DisplayBox displayDesc;
        private Win.UI.Label labelDesc;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Label labelVariance;
        private Win.UI.Label labelRequestQty;
        private Win.UI.Label labelIssueQty;
        private Win.UI.Label labelAccuIssue;
        private Win.UI.Label labelBalanceQty;
        private Win.UI.DisplayBox displaySizeSpec;
        private Win.UI.Label labelSizeSpec;
        private Win.UI.NumericBox numVariance;
        private Win.UI.NumericBox numIssueQty;
        private Win.UI.NumericBox numBalanceQty;
        private Win.UI.NumericBox numAccuIssue;
        private Win.UI.NumericBox numRequestQty;
        private Win.UI.Button btnAutoPick;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displaySCIRefno;
    }
}
