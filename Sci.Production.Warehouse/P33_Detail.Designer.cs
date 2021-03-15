namespace Sci.Production.Warehouse
{
    partial class P33_Detail
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
            this.labelSPNo = new Sci.Win.UI.Label();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.displaySuppColor = new Sci.Win.UI.DisplayBox();
            this.labelColorID = new Sci.Win.UI.Label();
            this.labelDesc = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.displayColorID = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.displaySCIRefno = new Sci.Win.UI.DisplayBox();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.numIssueQty = new Sci.Win.UI.NumericBox();
            this.numAccuIssue = new Sci.Win.UI.NumericBox();
            this.numRequestQty = new Sci.Win.UI.NumericBox();
            this.labelRequestQty = new Sci.Win.UI.Label();
            this.labelIssueQty = new Sci.Win.UI.Label();
            this.labelAccuIssue = new Sci.Win.UI.Label();
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
            // append
            // 
            this.append.TabIndex = 1;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(838, 5);
            this.save.TabIndex = 7;
            this.save.Click += new System.EventHandler(this.Save_Click);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(918, 5);
            this.undo.TabIndex = 8;
            // 
            // revise
            // 
            this.revise.TabIndex = 2;
            // 
            // delete
            // 
            this.delete.TabIndex = 3;
            this.delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(783, 5);
            this.next.TabIndex = 6;
            this.next.Visible = false;
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(728, 5);
            this.prev.TabIndex = 5;
            this.prev.Visible = false;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(12, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 100;
            this.labelSPNo.Text = "PO#";
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(90, 19);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(124, 23);
            this.displaySPNo.TabIndex = 102;
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(521, 19);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(124, 23);
            this.displayRefno.TabIndex = 104;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(473, 19);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(45, 23);
            this.labelRefno.TabIndex = 103;
            this.labelRefno.Text = "Refno";
            // 
            // displaySuppColor
            // 
            this.displaySuppColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySuppColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySuppColor.Location = new System.Drawing.Point(849, 19);
            this.displaySuppColor.Name = "displaySuppColor";
            this.displaySuppColor.Size = new System.Drawing.Size(124, 23);
            this.displaySuppColor.TabIndex = 106;
            // 
            // labelColorID
            // 
            this.labelColorID.Location = new System.Drawing.Point(771, 19);
            this.labelColorID.Name = "labelColorID";
            this.labelColorID.Size = new System.Drawing.Size(75, 23);
            this.labelColorID.TabIndex = 105;
            this.labelColorID.Text = "SuppColor";
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(12, 52);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(75, 23);
            this.labelDesc.TabIndex = 107;
            this.labelDesc.Text = "Desc";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.displayColorID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.displaySCIRefno);
            this.groupBox1.Controls.Add(this.editDesc);
            this.groupBox1.Controls.Add(this.displaySuppColor);
            this.groupBox1.Controls.Add(this.labelColorID);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.labelDesc);
            this.groupBox1.Controls.Add(this.displaySPNo);
            this.groupBox1.Controls.Add(this.labelRefno);
            this.groupBox1.Controls.Add(this.displayRefno);
            this.groupBox1.Location = new System.Drawing.Point(12, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 146);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(695, 19);
            this.displayColorID.Name = "displayColorID";
            this.displayColorID.Size = new System.Drawing.Size(73, 23);
            this.displayColorID.TabIndex = 113;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(648, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 23);
            this.label2.TabIndex = 112;
            this.label2.Text = "Color";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(217, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 23);
            this.label1.TabIndex = 110;
            this.label1.Text = "SCIRefno";
            // 
            // displaySCIRefno
            // 
            this.displaySCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Location = new System.Drawing.Point(287, 19);
            this.displaySCIRefno.Name = "displaySCIRefno";
            this.displaySCIRefno.Size = new System.Drawing.Size(183, 23);
            this.displaySCIRefno.TabIndex = 111;
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDesc.IsSupportEditMode = false;
            this.editDesc.Location = new System.Drawing.Point(90, 48);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.ReadOnly = true;
            this.editDesc.Size = new System.Drawing.Size(849, 94);
            this.editDesc.TabIndex = 109;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numIssueQty);
            this.groupBox2.Controls.Add(this.numAccuIssue);
            this.groupBox2.Controls.Add(this.numRequestQty);
            this.groupBox2.Controls.Add(this.labelRequestQty);
            this.groupBox2.Controls.Add(this.labelIssueQty);
            this.groupBox2.Controls.Add(this.labelAccuIssue);
            this.groupBox2.Location = new System.Drawing.Point(10, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(984, 68);
            this.groupBox2.TabIndex = 111;
            this.groupBox2.TabStop = false;
            // 
            // numIssueQty
            // 
            this.numIssueQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numIssueQty.DecimalPlaces = 2;
            this.numIssueQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numIssueQty.IsSupportEditMode = false;
            this.numIssueQty.Location = new System.Drawing.Point(569, 29);
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
            // numAccuIssue
            // 
            this.numAccuIssue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAccuIssue.DecimalPlaces = 2;
            this.numAccuIssue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAccuIssue.IsSupportEditMode = false;
            this.numAccuIssue.Location = new System.Drawing.Point(107, 29);
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
            this.numRequestQty.Location = new System.Drawing.Point(353, 29);
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
            // labelRequestQty
            // 
            this.labelRequestQty.Location = new System.Drawing.Point(241, 29);
            this.labelRequestQty.Name = "labelRequestQty";
            this.labelRequestQty.Size = new System.Drawing.Size(109, 23);
            this.labelRequestQty.TabIndex = 99;
            this.labelRequestQty.Text = "Issue By Output";
            // 
            // labelIssueQty
            // 
            this.labelIssueQty.Location = new System.Drawing.Point(476, 29);
            this.labelIssueQty.Name = "labelIssueQty";
            this.labelIssueQty.Size = new System.Drawing.Size(90, 23);
            this.labelIssueQty.TabIndex = 105;
            this.labelIssueQty.Text = "Issue Qty";
            // 
            // labelAccuIssue
            // 
            this.labelAccuIssue.Location = new System.Drawing.Point(14, 29);
            this.labelAccuIssue.Name = "labelAccuIssue";
            this.labelAccuIssue.Size = new System.Drawing.Size(90, 23);
            this.labelAccuIssue.TabIndex = 100;
            this.labelAccuIssue.Text = "Accu. Issued";
            // 
            // btnAutoPick
            // 
            this.btnAutoPick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAutoPick.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAutoPick.Location = new System.Drawing.Point(642, 5);
            this.btnAutoPick.Name = "btnAutoPick";
            this.btnAutoPick.Size = new System.Drawing.Size(80, 30);
            this.btnAutoPick.TabIndex = 4;
            this.btnAutoPick.Text = "AutoPick";
            this.btnAutoPick.UseVisualStyleBackColor = true;
            this.btnAutoPick.Click += new System.EventHandler(this.BtnAutoPick_Click);
            // 
            // P33_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 557);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "P33_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Input8A";
            this.Text = "P33. Issue Thread Detail";
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
        private Win.UI.Label labelSPNo;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.Label labelRefno;
        private Win.UI.DisplayBox displaySuppColor;
        private Win.UI.Label labelColorID;
        private Win.UI.Label labelDesc;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Label labelRequestQty;
        private Win.UI.Label labelIssueQty;
        private Win.UI.Label labelAccuIssue;
        private Win.UI.NumericBox numIssueQty;
        private Win.UI.NumericBox numAccuIssue;
        private Win.UI.NumericBox numRequestQty;
        private Win.UI.Button btnAutoPick;
        private Win.UI.EditBox editDesc;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displaySCIRefno;
        private Win.UI.DisplayBox displayColorID;
        private Win.UI.Label label2;
    }
}
