namespace Sci.Production.Quality
{
    partial class P02_Detail
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
            this.btnAmend = new Sci.Win.UI.Button();
            this.txtInspectedQty = new Sci.Win.UI.TextBox();
            this.txtUnit = new Sci.Win.UI.TextBox();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.txtArriveQty = new Sci.Win.UI.TextBox();
            this.txtWKNO = new Sci.Win.UI.TextBox();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.txtSCIRefno = new Sci.Win.UI.TextBox();
            this.txtSEQ = new Sci.Win.UI.TextBox();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.labelWKNO = new Sci.Win.UI.Label();
            this.labelSCIRefno = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelArriveQty = new Sci.Win.UI.Label();
            this.labelSize = new Sci.Win.UI.Label();
            this.labelColor = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.labelDefect = new Sci.Win.UI.Label();
            this.labelInspectedQty = new Sci.Win.UI.Label();
            this.txtRejectedQty = new Sci.Win.UI.TextBox();
            this.labelRejectedQty = new Sci.Win.UI.Label();
            this.labelInspectDate = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.txtInspector = new Sci.Win.UI.TextBox();
            this.labelInspector = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.editDefect = new Sci.Win.UI.EditBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.dateInspectDate = new Sci.Win.UI.DateBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.txtsupplier = new Sci.Production.Class.txtsupplier();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnClose);
            this.btmcont.Location = new System.Drawing.Point(0, 416);
            this.btmcont.Size = new System.Drawing.Size(669, 40);
            this.btmcont.Controls.SetChildIndex(this.left, 0);
            this.btmcont.Controls.SetChildIndex(this.right, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnClose, 0);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(579, 5);
            this.undo.Text = "Close";
            this.undo.Click += new System.EventHandler(this.undo_Click);
            // 
            // save
            // 
            this.save.Enabled = true;
            this.save.Location = new System.Drawing.Point(499, 5);
            this.save.Text = "Edit";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // left
            // 
            this.left.Enabled = true;
            this.left.Visible = false;
            // 
            // right
            // 
            this.right.Enabled = true;
            this.right.Visible = false;
            // 
            // btnAmend
            // 
            this.btnAmend.Location = new System.Drawing.Point(499, 30);
            this.btnAmend.Name = "btnAmend";
            this.btnAmend.Size = new System.Drawing.Size(91, 30);
            this.btnAmend.TabIndex = 117;
            this.btnAmend.Text = "Amend";
            this.btnAmend.UseVisualStyleBackColor = true;
            this.btnAmend.Click += new System.EventHandler(this.Encode_Click);
            // 
            // txtInspectedQty
            // 
            this.txtInspectedQty.BackColor = System.Drawing.Color.White;
            this.txtInspectedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInspectedQty.Location = new System.Drawing.Point(112, 188);
            this.txtInspectedQty.Name = "txtInspectedQty";
            this.txtInspectedQty.Size = new System.Drawing.Size(124, 23);
            this.txtInspectedQty.TabIndex = 116;
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.Color.White;
            this.txtUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUnit.Location = new System.Drawing.Point(95, 125);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(144, 23);
            this.txtUnit.TabIndex = 115;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(328, 125);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(125, 23);
            this.txtColor.TabIndex = 114;
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.White;
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSize.Location = new System.Drawing.Point(534, 125);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(119, 23);
            this.txtSize.TabIndex = 113;
            // 
            // txtArriveQty
            // 
            this.txtArriveQty.BackColor = System.Drawing.Color.White;
            this.txtArriveQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArriveQty.Location = new System.Drawing.Point(95, 94);
            this.txtArriveQty.Name = "txtArriveQty";
            this.txtArriveQty.Size = new System.Drawing.Size(144, 23);
            this.txtArriveQty.TabIndex = 112;
            // 
            // txtWKNO
            // 
            this.txtWKNO.BackColor = System.Drawing.Color.White;
            this.txtWKNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKNO.Location = new System.Drawing.Point(95, 60);
            this.txtWKNO.Name = "txtWKNO";
            this.txtWKNO.Size = new System.Drawing.Size(144, 23);
            this.txtWKNO.TabIndex = 109;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(328, 60);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(147, 23);
            this.txtRefno.TabIndex = 108;
            // 
            // txtSCIRefno
            // 
            this.txtSCIRefno.BackColor = System.Drawing.Color.White;
            this.txtSCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSCIRefno.Location = new System.Drawing.Point(328, 30);
            this.txtSCIRefno.Name = "txtSCIRefno";
            this.txtSCIRefno.Size = new System.Drawing.Size(147, 23);
            this.txtSCIRefno.TabIndex = 107;
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.White;
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSEQ.Location = new System.Drawing.Point(95, 30);
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.Size = new System.Drawing.Size(144, 23);
            this.txtSEQ.TabIndex = 106;
            // 
            // labelSEQ
            // 
            this.labelSEQ.Lines = 0;
            this.labelSEQ.Location = new System.Drawing.Point(15, 30);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(75, 23);
            this.labelSEQ.TabIndex = 95;
            this.labelSEQ.Text = "SEQ#";
            // 
            // labelWKNO
            // 
            this.labelWKNO.Lines = 0;
            this.labelWKNO.Location = new System.Drawing.Point(15, 60);
            this.labelWKNO.Name = "labelWKNO";
            this.labelWKNO.Size = new System.Drawing.Size(75, 23);
            this.labelWKNO.TabIndex = 96;
            this.labelWKNO.Text = "WKNO";
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Lines = 0;
            this.labelSCIRefno.Location = new System.Drawing.Point(248, 30);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(75, 23);
            this.labelSCIRefno.TabIndex = 97;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // labelRefno
            // 
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(247, 60);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 98;
            this.labelRefno.Text = "Refno";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(248, 94);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 99;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelArriveQty
            // 
            this.labelArriveQty.Lines = 0;
            this.labelArriveQty.Location = new System.Drawing.Point(15, 94);
            this.labelArriveQty.Name = "labelArriveQty";
            this.labelArriveQty.Size = new System.Drawing.Size(75, 23);
            this.labelArriveQty.TabIndex = 100;
            this.labelArriveQty.Text = "Arrive Qty";
            // 
            // labelSize
            // 
            this.labelSize.Lines = 0;
            this.labelSize.Location = new System.Drawing.Point(456, 125);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(75, 23);
            this.labelSize.TabIndex = 101;
            this.labelSize.Text = "Size";
            // 
            // labelColor
            // 
            this.labelColor.Lines = 0;
            this.labelColor.Location = new System.Drawing.Point(248, 125);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 102;
            this.labelColor.Text = "Color";
            // 
            // labelUnit
            // 
            this.labelUnit.Lines = 0;
            this.labelUnit.Location = new System.Drawing.Point(15, 125);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(75, 23);
            this.labelUnit.TabIndex = 103;
            this.labelUnit.Text = "Unit";
            // 
            // labelDefect
            // 
            this.labelDefect.Lines = 0;
            this.labelDefect.Location = new System.Drawing.Point(236, 32);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(91, 23);
            this.labelDefect.TabIndex = 104;
            this.labelDefect.Text = "Defect";
            // 
            // labelInspectedQty
            // 
            this.labelInspectedQty.Lines = 0;
            this.labelInspectedQty.Location = new System.Drawing.Point(18, 188);
            this.labelInspectedQty.Name = "labelInspectedQty";
            this.labelInspectedQty.Size = new System.Drawing.Size(91, 23);
            this.labelInspectedQty.TabIndex = 105;
            this.labelInspectedQty.Text = "Inspected Qty";
            // 
            // txtRejectedQty
            // 
            this.txtRejectedQty.BackColor = System.Drawing.Color.White;
            this.txtRejectedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRejectedQty.Location = new System.Drawing.Point(113, 223);
            this.txtRejectedQty.Name = "txtRejectedQty";
            this.txtRejectedQty.Size = new System.Drawing.Size(121, 23);
            this.txtRejectedQty.TabIndex = 120;
            // 
            // labelRejectedQty
            // 
            this.labelRejectedQty.Lines = 0;
            this.labelRejectedQty.Location = new System.Drawing.Point(18, 223);
            this.labelRejectedQty.Name = "labelRejectedQty";
            this.labelRejectedQty.Size = new System.Drawing.Size(91, 23);
            this.labelRejectedQty.TabIndex = 119;
            this.labelRejectedQty.Text = "Rejected Qty";
            // 
            // labelInspectDate
            // 
            this.labelInspectDate.Lines = 0;
            this.labelInspectDate.Location = new System.Drawing.Point(18, 257);
            this.labelInspectDate.Name = "labelInspectDate";
            this.labelInspectDate.Size = new System.Drawing.Size(91, 23);
            this.labelInspectDate.TabIndex = 121;
            this.labelInspectDate.Text = "Inspect Date";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(340, 160);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(266, 23);
            this.txtRemark.TabIndex = 124;
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(236, 160);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(91, 23);
            this.labelRemark.TabIndex = 123;
            this.labelRemark.Text = "Remark";
            // 
            // txtInspector
            // 
            this.txtInspector.BackColor = System.Drawing.Color.White;
            this.txtInspector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInspector.Location = new System.Drawing.Point(115, 288);
            this.txtInspector.Name = "txtInspector";
            this.txtInspector.Size = new System.Drawing.Size(121, 23);
            this.txtInspector.TabIndex = 126;
            // 
            // labelInspector
            // 
            this.labelInspector.Lines = 0;
            this.labelInspector.Location = new System.Drawing.Point(17, 288);
            this.labelInspector.Name = "labelInspector";
            this.labelInspector.Size = new System.Drawing.Size(91, 23);
            this.labelInspector.TabIndex = 125;
            this.labelInspector.Text = "Inspector";
            // 
            // labelResult
            // 
            this.labelResult.Lines = 0;
            this.labelResult.Location = new System.Drawing.Point(18, 318);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(91, 23);
            this.labelResult.TabIndex = 127;
            this.labelResult.Text = "Result";
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.White;
            this.comboResult.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "result", true));
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(114, 316);
            this.comboResult.Name = "comboResult";
            this.comboResult.Size = new System.Drawing.Size(121, 24);
            this.comboResult.TabIndex = 129;
            // 
            // editDefect
            // 
            this.editDefect.BackColor = System.Drawing.Color.White;
            this.editDefect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDefect.Location = new System.Drawing.Point(340, 32);
            this.editDefect.Multiline = true;
            this.editDefect.Name = "editDefect";
            this.editDefect.Size = new System.Drawing.Size(266, 105);
            this.editDefect.TabIndex = 132;
            this.editDefect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.editBox1_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.panel1.Controls.Add(this.dateInspectDate);
            this.panel1.Controls.Add(this.editDefect);
            this.panel1.Controls.Add(this.labelDefect);
            this.panel1.Controls.Add(this.labelRemark);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Location = new System.Drawing.Point(10, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(639, 234);
            this.panel1.TabIndex = 133;
            // 
            // dateInspectDate
            // 
            this.dateInspectDate.Location = new System.Drawing.Point(103, 101);
            this.dateInspectDate.Name = "dateInspectDate";
            this.dateInspectDate.Size = new System.Drawing.Size(145, 23);
            this.dateInspectDate.TabIndex = 135;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(413, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 135;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(328, 94);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(214, 23);
            this.txtsupplier.TabIndex = 134;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // P02_Detail
            // 
            this.ClientSize = new System.Drawing.Size(669, 456);
            this.Controls.Add(this.txtsupplier);
            this.Controls.Add(this.comboResult);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.txtInspector);
            this.Controls.Add(this.labelInspector);
            this.Controls.Add(this.labelInspectDate);
            this.Controls.Add(this.txtRejectedQty);
            this.Controls.Add(this.labelRejectedQty);
            this.Controls.Add(this.btnAmend);
            this.Controls.Add(this.txtInspectedQty);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.txtArriveQty);
            this.Controls.Add(this.txtWKNO);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.txtSCIRefno);
            this.Controls.Add(this.txtSEQ);
            this.Controls.Add(this.labelSEQ);
            this.Controls.Add(this.labelWKNO);
            this.Controls.Add(this.labelSCIRefno);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.labelArriveQty);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.labelInspectedQty);
            this.Controls.Add(this.panel1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "defect", true));
            this.Name = "P02_Detail";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Accessory Inspection- SP+SEQ+Detail";
            this.WorkAlias = "AIR";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelInspectedQty, 0);
            this.Controls.SetChildIndex(this.labelUnit, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.labelSize, 0);
            this.Controls.SetChildIndex(this.labelArriveQty, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.labelSCIRefno, 0);
            this.Controls.SetChildIndex(this.labelWKNO, 0);
            this.Controls.SetChildIndex(this.labelSEQ, 0);
            this.Controls.SetChildIndex(this.txtSEQ, 0);
            this.Controls.SetChildIndex(this.txtSCIRefno, 0);
            this.Controls.SetChildIndex(this.txtRefno, 0);
            this.Controls.SetChildIndex(this.txtWKNO, 0);
            this.Controls.SetChildIndex(this.txtArriveQty, 0);
            this.Controls.SetChildIndex(this.txtSize, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.txtUnit, 0);
            this.Controls.SetChildIndex(this.txtInspectedQty, 0);
            this.Controls.SetChildIndex(this.btnAmend, 0);
            this.Controls.SetChildIndex(this.labelRejectedQty, 0);
            this.Controls.SetChildIndex(this.txtRejectedQty, 0);
            this.Controls.SetChildIndex(this.labelInspectDate, 0);
            this.Controls.SetChildIndex(this.labelInspector, 0);
            this.Controls.SetChildIndex(this.txtInspector, 0);
            this.Controls.SetChildIndex(this.labelResult, 0);
            this.Controls.SetChildIndex(this.comboResult, 0);
            this.Controls.SetChildIndex(this.txtsupplier, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnAmend;
        private Win.UI.TextBox txtInspectedQty;
        private Win.UI.TextBox txtUnit;
        private Win.UI.TextBox txtColor;
        private Win.UI.TextBox txtSize;
        private Win.UI.TextBox txtArriveQty;
        private Win.UI.TextBox txtWKNO;
        private Win.UI.TextBox txtRefno;
        private Win.UI.TextBox txtSCIRefno;
        private Win.UI.TextBox txtSEQ;
        private Win.UI.Label labelSEQ;
        private Win.UI.Label labelWKNO;
        private Win.UI.Label labelSCIRefno;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelArriveQty;
        private Win.UI.Label labelSize;
        private Win.UI.Label labelColor;
        private Win.UI.Label labelUnit;
        private Win.UI.Label labelDefect;
        private Win.UI.Label labelInspectedQty;
        private Win.UI.TextBox txtRejectedQty;
        private Win.UI.Label labelRejectedQty;
        private Win.UI.Label labelInspectDate;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.TextBox txtInspector;
        private Win.UI.Label labelInspector;
        private Win.UI.Label labelResult;
        private Win.UI.ComboBox comboResult;
        private Win.UI.EditBox editDefect;
        private Win.UI.Panel panel1;
        private Class.txtsupplier txtsupplier;
        private Win.UI.DateBox dateInspectDate;
        private Win.UI.Button btnClose;

    }
}
