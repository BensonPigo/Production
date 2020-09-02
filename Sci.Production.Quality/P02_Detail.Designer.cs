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
            this.labelInspector = new Sci.Win.UI.Label();
            this.labelResult = new Sci.Win.UI.Label();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.editDefect = new Sci.Win.UI.EditBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.dateInspectDate = new Sci.Win.UI.DateBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnEdit = new Sci.Win.UI.Button();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.txtInspector = new Sci.Production.Class.Txtuser();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnEdit);
            this.btmcont.Controls.Add(this.btnClose);
            this.btmcont.Location = new System.Drawing.Point(0, 376);
            this.btmcont.Size = new System.Drawing.Size(670, 40);
            this.btmcont.Controls.SetChildIndex(this.left, 0);
            this.btmcont.Controls.SetChildIndex(this.right, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnClose, 0);
            this.btmcont.Controls.SetChildIndex(this.btnEdit, 0);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(580, 5);
            this.undo.Text = "Close";
            this.undo.Visible = false;
            // 
            // save
            // 
            this.save.Enabled = true;
            this.save.Location = new System.Drawing.Point(500, 5);
            this.save.Text = "Edit";
            this.save.Visible = false;
            // 
            // left
            // 
            this.left.Enabled = true;
            this.left.Click += new System.EventHandler(this.Left_Click);
            // 
            // right
            // 
            this.right.Enabled = true;
            this.right.Click += new System.EventHandler(this.Right_Click);
            // 
            // btnAmend
            // 
            this.btnAmend.Location = new System.Drawing.Point(532, 30);
            this.btnAmend.Name = "btnAmend";
            this.btnAmend.Size = new System.Drawing.Size(117, 30);
            this.btnAmend.TabIndex = 117;
            this.btnAmend.Text = "Amend";
            this.btnAmend.UseVisualStyleBackColor = true;
            this.btnAmend.Click += new System.EventHandler(this.BtnAmend_Click);
            // 
            // txtInspectedQty
            // 
            this.txtInspectedQty.BackColor = System.Drawing.Color.White;
            this.txtInspectedQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InspQty", true));
            this.txtInspectedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInspectedQty.Location = new System.Drawing.Point(112, 188);
            this.txtInspectedQty.Name = "txtInspectedQty";
            this.txtInspectedQty.Size = new System.Drawing.Size(124, 23);
            this.txtInspectedQty.TabIndex = 116;
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "unit", true));
            this.txtUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtUnit.IsSupportEditMode = false;
            this.txtUnit.Location = new System.Drawing.Point(95, 125);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(144, 23);
            this.txtUnit.TabIndex = 115;
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtColor.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "colorid", true));
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtColor.IsSupportEditMode = false;
            this.txtColor.Location = new System.Drawing.Point(328, 125);
            this.txtColor.Name = "txtColor";
            this.txtColor.ReadOnly = true;
            this.txtColor.Size = new System.Drawing.Size(125, 23);
            this.txtColor.TabIndex = 114;
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSize.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "size", true));
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSize.IsSupportEditMode = false;
            this.txtSize.Location = new System.Drawing.Point(534, 125);
            this.txtSize.Name = "txtSize";
            this.txtSize.ReadOnly = true;
            this.txtSize.Size = new System.Drawing.Size(119, 23);
            this.txtSize.TabIndex = 113;
            // 
            // txtArriveQty
            // 
            this.txtArriveQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtArriveQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ArriveQty", true));
            this.txtArriveQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtArriveQty.IsSupportEditMode = false;
            this.txtArriveQty.Location = new System.Drawing.Point(95, 94);
            this.txtArriveQty.Name = "txtArriveQty";
            this.txtArriveQty.ReadOnly = true;
            this.txtArriveQty.Size = new System.Drawing.Size(144, 23);
            this.txtArriveQty.TabIndex = 112;
            // 
            // txtWKNO
            // 
            this.txtWKNO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWKNO.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExportID", true));
            this.txtWKNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWKNO.IsSupportEditMode = false;
            this.txtWKNO.Location = new System.Drawing.Point(95, 60);
            this.txtWKNO.Name = "txtWKNO";
            this.txtWKNO.ReadOnly = true;
            this.txtWKNO.Size = new System.Drawing.Size(144, 23);
            this.txtWKNO.TabIndex = 109;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Refno", true));
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtRefno.IsSupportEditMode = false;
            this.txtRefno.Location = new System.Drawing.Point(328, 60);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.ReadOnly = true;
            this.txtRefno.Size = new System.Drawing.Size(147, 23);
            this.txtRefno.TabIndex = 108;
            // 
            // txtSCIRefno
            // 
            this.txtSCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SCIRefno", true));
            this.txtSCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSCIRefno.IsSupportEditMode = false;
            this.txtSCIRefno.Location = new System.Drawing.Point(328, 30);
            this.txtSCIRefno.Name = "txtSCIRefno";
            this.txtSCIRefno.ReadOnly = true;
            this.txtSCIRefno.Size = new System.Drawing.Size(147, 23);
            this.txtSCIRefno.TabIndex = 107;
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSEQ.IsSupportEditMode = false;
            this.txtSEQ.Location = new System.Drawing.Point(95, 30);
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.ReadOnly = true;
            this.txtSEQ.Size = new System.Drawing.Size(144, 23);
            this.txtSEQ.TabIndex = 106;
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(15, 30);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(75, 23);
            this.labelSEQ.TabIndex = 95;
            this.labelSEQ.Text = "SEQ#";
            // 
            // labelWKNO
            // 
            this.labelWKNO.Location = new System.Drawing.Point(15, 60);
            this.labelWKNO.Name = "labelWKNO";
            this.labelWKNO.Size = new System.Drawing.Size(75, 23);
            this.labelWKNO.TabIndex = 96;
            this.labelWKNO.Text = "WKNO";
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Location = new System.Drawing.Point(248, 30);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(75, 23);
            this.labelSCIRefno.TabIndex = 97;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(248, 60);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 98;
            this.labelRefno.Text = "Refno";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(248, 94);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 99;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelArriveQty
            // 
            this.labelArriveQty.Location = new System.Drawing.Point(15, 94);
            this.labelArriveQty.Name = "labelArriveQty";
            this.labelArriveQty.Size = new System.Drawing.Size(75, 23);
            this.labelArriveQty.TabIndex = 100;
            this.labelArriveQty.Text = "Arrive Qty";
            // 
            // labelSize
            // 
            this.labelSize.Location = new System.Drawing.Point(456, 125);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(75, 23);
            this.labelSize.TabIndex = 101;
            this.labelSize.Text = "Size";
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(248, 125);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 102;
            this.labelColor.Text = "Color";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(15, 125);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(75, 23);
            this.labelUnit.TabIndex = 103;
            this.labelUnit.Text = "Unit";
            // 
            // labelDefect
            // 
            this.labelDefect.Location = new System.Drawing.Point(237, 32);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(91, 23);
            this.labelDefect.TabIndex = 104;
            this.labelDefect.Text = "Defect";
            // 
            // labelInspectedQty
            // 
            this.labelInspectedQty.Location = new System.Drawing.Point(18, 188);
            this.labelInspectedQty.Name = "labelInspectedQty";
            this.labelInspectedQty.Size = new System.Drawing.Size(91, 23);
            this.labelInspectedQty.TabIndex = 105;
            this.labelInspectedQty.Text = "Inspected Qty";
            // 
            // txtRejectedQty
            // 
            this.txtRejectedQty.BackColor = System.Drawing.Color.White;
            this.txtRejectedQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "RejectQty", true));
            this.txtRejectedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRejectedQty.Location = new System.Drawing.Point(112, 223);
            this.txtRejectedQty.Name = "txtRejectedQty";
            this.txtRejectedQty.Size = new System.Drawing.Size(124, 23);
            this.txtRejectedQty.TabIndex = 120;
            // 
            // labelRejectedQty
            // 
            this.labelRejectedQty.Location = new System.Drawing.Point(18, 223);
            this.labelRejectedQty.Name = "labelRejectedQty";
            this.labelRejectedQty.Size = new System.Drawing.Size(91, 23);
            this.labelRejectedQty.TabIndex = 119;
            this.labelRejectedQty.Text = "Rejected Qty";
            // 
            // labelInspectDate
            // 
            this.labelInspectDate.Location = new System.Drawing.Point(18, 257);
            this.labelInspectDate.Name = "labelInspectDate";
            this.labelInspectDate.Size = new System.Drawing.Size(91, 23);
            this.labelInspectDate.TabIndex = 121;
            this.labelInspectDate.Text = "Inspect Date";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(332, 166);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(287, 23);
            this.txtRemark.TabIndex = 124;
            // 
            // labelRemark
            // 
            this.labelRemark.Location = new System.Drawing.Point(238, 166);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(91, 23);
            this.labelRemark.TabIndex = 123;
            this.labelRemark.Text = "Remark";
            // 
            // labelInspector
            // 
            this.labelInspector.Location = new System.Drawing.Point(18, 288);
            this.labelInspector.Name = "labelInspector";
            this.labelInspector.Size = new System.Drawing.Size(91, 23);
            this.labelInspector.TabIndex = 125;
            this.labelInspector.Text = "Inspector";
            // 
            // labelResult
            // 
            this.labelResult.Location = new System.Drawing.Point(18, 321);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(91, 23);
            this.labelResult.TabIndex = 127;
            this.labelResult.Text = "Result";
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.White;
            this.comboResult.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Result1", true));
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(112, 321);
            this.comboResult.Name = "comboResult";
            this.comboResult.Size = new System.Drawing.Size(121, 24);
            this.comboResult.TabIndex = 129;
            // 
            // editDefect
            // 
            this.editDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDefect.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Defect", true));
            this.editDefect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDefect.IsSupportEditMode = false;
            this.editDefect.Location = new System.Drawing.Point(331, 32);
            this.editDefect.Multiline = true;
            this.editDefect.Name = "editDefect";
            this.editDefect.ReadOnly = true;
            this.editDefect.Size = new System.Drawing.Size(288, 92);
            this.editDefect.TabIndex = 132;
            this.editDefect.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EditDefect_MouseDown);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.panel1.Controls.Add(this.txtInspector);
            this.panel1.Controls.Add(this.dateInspectDate);
            this.panel1.Controls.Add(this.editDefect);
            this.panel1.Controls.Add(this.labelDefect);
            this.panel1.Controls.Add(this.labelRemark);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Location = new System.Drawing.Point(10, 156);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(639, 209);
            this.panel1.TabIndex = 133;
            // 
            // dateInspectDate
            // 
            this.dateInspectDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InspDate", true));
            this.dateInspectDate.Location = new System.Drawing.Point(102, 101);
            this.dateInspectDate.Name = "dateInspectDate";
            this.dateInspectDate.Size = new System.Drawing.Size(145, 23);
            this.dateInspectDate.TabIndex = 135;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(580, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 135;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(499, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 136;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // txtsupplier
            // 
            this.txtsupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Suppid", true));
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(328, 94);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(214, 23);
            this.txtsupplier.TabIndex = 134;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // txtInspector
            // 
            this.txtInspector.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Inspector", true));
            this.txtInspector.DisplayBox1Binding = "";
            this.txtInspector.Location = new System.Drawing.Point(102, 132);
            this.txtInspector.Name = "txtInspector";
            this.txtInspector.Size = new System.Drawing.Size(301, 23);
            this.txtInspector.TabIndex = 135;
            this.txtInspector.TextBox1Binding = "";
            // 
            // P02_Detail
            // 
            this.ClientSize = new System.Drawing.Size(670, 416);
            this.Controls.Add(this.txtsupplier);
            this.Controls.Add(this.comboResult);
            this.Controls.Add(this.labelResult);
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
        private Win.UI.Label labelInspector;
        private Win.UI.Label labelResult;
        private Win.UI.ComboBox comboResult;
        private Win.UI.EditBox editDefect;
        private Win.UI.Panel panel1;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.DateBox dateInspectDate;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnEdit;
        private Class.Txtuser txtInspector;

    }
}
