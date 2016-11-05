namespace Sci.Production.Quality
{
    partial class P07_Oven
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
            this.sp_text = new Sci.Win.UI.DisplayBox();
            this.lbSP = new Sci.Win.UI.Label();
            this.WKNO_text = new Sci.Win.UI.DisplayBox();
            this.lbWKNO = new Sci.Win.UI.Label();
            this.lbQty = new Sci.Win.UI.Label();
            this.Unit_text = new Sci.Win.UI.DisplayBox();
            this.lbUnit = new Sci.Win.UI.Label();
            this.Size_text = new Sci.Win.UI.DisplayBox();
            this.lbSize = new Sci.Win.UI.Label();
            this.SciRefno = new Sci.Win.UI.DisplayBox();
            this.lbSciRefno = new Sci.Win.UI.Label();
            this.BrandRefno_text = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.lbSupplier = new Sci.Win.UI.Label();
            this.lbColor = new Sci.Win.UI.Label();
            this.Color_text = new Sci.Win.UI.DisplayBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.OvenDate = new Sci.Win.UI.DateBox();
            this.lbDate = new Sci.Win.UI.Label();
            this.txtuser1 = new Sci.Production.Class.txtuser();
            this.label2 = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.lbRemark = new Sci.Win.UI.Label();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.lbResult = new Sci.Win.UI.Label();
            this.txtScale = new Sci.Win.UI.TextBox();
            this.lbScale = new Sci.Win.UI.Label();
            this.btnClose = new Sci.Win.UI.Button();
            this.Qty_text = new Sci.Win.UI.NumericBox();
            this.Supplier_text = new Sci.Win.UI.DisplayBox();
            this.btnSave = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnSave);
            this.btmcont.Controls.Add(this.btnClose);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnClose, 0);
            this.btmcont.Controls.SetChildIndex(this.btnSave, 0);
            // 
            // undo
            // 
            this.undo.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // save
            // 
            this.save.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.save.Enabled = false;
            this.save.Text = "Edit";
            this.save.Visible = false;
            // 
            // sp_text
            // 
            this.sp_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.sp_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.sp_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.sp_text.Location = new System.Drawing.Point(62, 11);
            this.sp_text.Name = "sp_text";
            this.sp_text.Size = new System.Drawing.Size(118, 21);
            this.sp_text.TabIndex = 102;
            // 
            // lbSP
            // 
            this.lbSP.Lines = 0;
            this.lbSP.Location = new System.Drawing.Point(9, 9);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(50, 23);
            this.lbSP.TabIndex = 101;
            this.lbSP.Text = "SP#";
            // 
            // WKNO_text
            // 
            this.WKNO_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.WKNO_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.WKNO_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.WKNO_text.Location = new System.Drawing.Point(62, 40);
            this.WKNO_text.Name = "WKNO_text";
            this.WKNO_text.Size = new System.Drawing.Size(118, 21);
            this.WKNO_text.TabIndex = 104;
            // 
            // lbWKNO
            // 
            this.lbWKNO.Lines = 0;
            this.lbWKNO.Location = new System.Drawing.Point(9, 38);
            this.lbWKNO.Name = "lbWKNO";
            this.lbWKNO.Size = new System.Drawing.Size(50, 23);
            this.lbWKNO.TabIndex = 103;
            this.lbWKNO.Text = "WKNO";
            // 
            // lbQty
            // 
            this.lbQty.Lines = 0;
            this.lbQty.Location = new System.Drawing.Point(9, 67);
            this.lbQty.Name = "lbQty";
            this.lbQty.Size = new System.Drawing.Size(70, 23);
            this.lbQty.TabIndex = 105;
            this.lbQty.Text = "Active Qty";
            // 
            // Unit_text
            // 
            this.Unit_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Unit_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Unit_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Unit_text.Location = new System.Drawing.Point(62, 97);
            this.Unit_text.Name = "Unit_text";
            this.Unit_text.Size = new System.Drawing.Size(118, 21);
            this.Unit_text.TabIndex = 108;
            // 
            // lbUnit
            // 
            this.lbUnit.Lines = 0;
            this.lbUnit.Location = new System.Drawing.Point(9, 95);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(50, 23);
            this.lbUnit.TabIndex = 107;
            this.lbUnit.Text = "Unit";
            // 
            // Size_text
            // 
            this.Size_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Size_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Size_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Size_text.Location = new System.Drawing.Point(62, 126);
            this.Size_text.Name = "Size_text";
            this.Size_text.Size = new System.Drawing.Size(118, 21);
            this.Size_text.TabIndex = 110;
            // 
            // lbSize
            // 
            this.lbSize.Lines = 0;
            this.lbSize.Location = new System.Drawing.Point(9, 124);
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(50, 23);
            this.lbSize.TabIndex = 109;
            this.lbSize.Text = "Size";
            // 
            // SciRefno
            // 
            this.SciRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.SciRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.SciRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.SciRefno.Location = new System.Drawing.Point(288, 13);
            this.SciRefno.Name = "SciRefno";
            this.SciRefno.Size = new System.Drawing.Size(150, 21);
            this.SciRefno.TabIndex = 112;
            // 
            // lbSciRefno
            // 
            this.lbSciRefno.Lines = 0;
            this.lbSciRefno.Location = new System.Drawing.Point(210, 11);
            this.lbSciRefno.Name = "lbSciRefno";
            this.lbSciRefno.Size = new System.Drawing.Size(75, 23);
            this.lbSciRefno.TabIndex = 111;
            this.lbSciRefno.Text = "SCI Ref#";
            // 
            // BrandRefno_text
            // 
            this.BrandRefno_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.BrandRefno_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.BrandRefno_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.BrandRefno_text.Location = new System.Drawing.Point(288, 40);
            this.BrandRefno_text.Name = "BrandRefno_text";
            this.BrandRefno_text.Size = new System.Drawing.Size(150, 21);
            this.BrandRefno_text.TabIndex = 114;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(210, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 113;
            this.label1.Text = "Brand Ref#";
            // 
            // lbSupplier
            // 
            this.lbSupplier.Lines = 0;
            this.lbSupplier.Location = new System.Drawing.Point(210, 69);
            this.lbSupplier.Name = "lbSupplier";
            this.lbSupplier.Size = new System.Drawing.Size(75, 23);
            this.lbSupplier.TabIndex = 115;
            this.lbSupplier.Text = "Supplier";
            // 
            // lbColor
            // 
            this.lbColor.Lines = 0;
            this.lbColor.Location = new System.Drawing.Point(210, 97);
            this.lbColor.Name = "lbColor";
            this.lbColor.Size = new System.Drawing.Size(75, 23);
            this.lbColor.TabIndex = 117;
            this.lbColor.Text = "Color";
            // 
            // Color_text
            // 
            this.Color_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Color_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Color_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Color_text.Location = new System.Drawing.Point(288, 99);
            this.Color_text.Name = "Color_text";
            this.Color_text.Size = new System.Drawing.Size(150, 21);
            this.Color_text.TabIndex = 118;
            // 
            // btnEncode
            // 
            this.btnEncode.Enabled = false;
            this.btnEncode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEncode.Location = new System.Drawing.Point(457, 11);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 23);
            this.btnEncode.TabIndex = 119;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.OvenDate);
            this.groupBox1.Controls.Add(this.lbDate);
            this.groupBox1.Controls.Add(this.txtuser1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.lbRemark);
            this.groupBox1.Controls.Add(this.comboResult);
            this.groupBox1.Controls.Add(this.lbResult);
            this.groupBox1.Controls.Add(this.txtScale);
            this.groupBox1.Controls.Add(this.lbScale);
            this.groupBox1.Location = new System.Drawing.Point(9, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 190);
            this.groupBox1.TabIndex = 120;
            this.groupBox1.TabStop = false;
            // 
            // OvenDate
            // 
            this.OvenDate.CausesValidation = false;
            this.OvenDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OvenDate", true));
            this.OvenDate.Enabled = false;
            this.OvenDate.Location = new System.Drawing.Point(103, 145);
            this.OvenDate.Name = "OvenDate";
            this.OvenDate.ReadOnly = true;
            this.OvenDate.Size = new System.Drawing.Size(120, 23);
            this.OvenDate.TabIndex = 123;
            // 
            // lbDate
            // 
            this.lbDate.Lines = 0;
            this.lbDate.Location = new System.Drawing.Point(15, 145);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(85, 23);
            this.lbDate.TabIndex = 122;
            this.lbDate.Text = "Inspect Date";
            // 
            // txtuser1
            // 
            this.txtuser1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "OvenInspector", true));
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Enabled = false;
            this.txtuser1.Location = new System.Drawing.Point(83, 112);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(296, 23);
            this.txtuser1.TabIndex = 121;
            this.txtuser1.TextBox1Binding = "";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(15, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 23);
            this.label2.TabIndex = 120;
            this.label2.Text = "Lab Tech";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OvenRemark", true));
            this.txtRemark.Enabled = false;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(73, 82);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(430, 23);
            this.txtRemark.TabIndex = 119;
            // 
            // lbRemark
            // 
            this.lbRemark.Lines = 0;
            this.lbRemark.Location = new System.Drawing.Point(15, 82);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(55, 23);
            this.lbRemark.TabIndex = 118;
            this.lbRemark.Text = "Remark";
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboResult.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Oven", true));
            this.comboResult.Enabled = false;
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(68, 51);
            this.comboResult.Name = "comboResult";
            this.comboResult.ReadOnly = true;
            this.comboResult.Size = new System.Drawing.Size(103, 24);
            this.comboResult.TabIndex = 117;
            // 
            // lbResult
            // 
            this.lbResult.Lines = 0;
            this.lbResult.Location = new System.Drawing.Point(15, 51);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(50, 23);
            this.lbResult.TabIndex = 112;
            this.lbResult.Text = "Result";
            // 
            // txtScale
            // 
            this.txtScale.BackColor = System.Drawing.Color.White;
            this.txtScale.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OvenScale", true));
            this.txtScale.Enabled = false;
            this.txtScale.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScale.Location = new System.Drawing.Point(68, 19);
            this.txtScale.Name = "txtScale";
            this.txtScale.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtScale.Size = new System.Drawing.Size(103, 23);
            this.txtScale.TabIndex = 111;
            this.txtScale.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtScale_PopUp);
            this.txtScale.Validating += new System.ComponentModel.CancelEventHandler(this.txtScale_Validating);
            // 
            // lbScale
            // 
            this.lbScale.Lines = 0;
            this.lbScale.Location = new System.Drawing.Point(15, 19);
            this.lbScale.Name = "lbScale";
            this.lbScale.Size = new System.Drawing.Size(50, 23);
            this.lbScale.TabIndex = 110;
            this.lbScale.Text = "Scale";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(459, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 123;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // Qty_text
            // 
            this.Qty_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Qty_text.DecimalPlaces = 2;
            this.Qty_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Qty_text.IsSupportEditMode = false;
            this.Qty_text.Location = new System.Drawing.Point(82, 67);
            this.Qty_text.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.Qty_text.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.Qty_text.Name = "Qty_text";
            this.Qty_text.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.Qty_text.ReadOnly = true;
            this.Qty_text.Size = new System.Drawing.Size(98, 23);
            this.Qty_text.TabIndex = 121;
            this.Qty_text.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // Supplier_text
            // 
            this.Supplier_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.Supplier_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.Supplier_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Supplier_text.Location = new System.Drawing.Point(288, 69);
            this.Supplier_text.Name = "Supplier_text";
            this.Supplier_text.Size = new System.Drawing.Size(150, 21);
            this.Supplier_text.TabIndex = 122;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(379, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 124;
            this.btnSave.Text = "Edit";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // P07_Oven
            // 
            this.ClientSize = new System.Drawing.Size(549, 390);
            this.Controls.Add(this.Supplier_text);
            this.Controls.Add(this.Qty_text);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.Color_text);
            this.Controls.Add(this.lbColor);
            this.Controls.Add(this.lbSupplier);
            this.Controls.Add(this.BrandRefno_text);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SciRefno);
            this.Controls.Add(this.lbSciRefno);
            this.Controls.Add(this.Size_text);
            this.Controls.Add(this.lbSize);
            this.Controls.Add(this.Unit_text);
            this.Controls.Add(this.lbUnit);
            this.Controls.Add(this.lbQty);
            this.Controls.Add(this.WKNO_text);
            this.Controls.Add(this.lbWKNO);
            this.Controls.Add(this.sp_text);
            this.Controls.Add(this.lbSP);
            this.Name = "P07_Oven";
            this.Text = "Accessory Oven Test";
            this.WorkAlias = "AIR_Laboratory";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.lbSP, 0);
            this.Controls.SetChildIndex(this.sp_text, 0);
            this.Controls.SetChildIndex(this.lbWKNO, 0);
            this.Controls.SetChildIndex(this.WKNO_text, 0);
            this.Controls.SetChildIndex(this.lbQty, 0);
            this.Controls.SetChildIndex(this.lbUnit, 0);
            this.Controls.SetChildIndex(this.Unit_text, 0);
            this.Controls.SetChildIndex(this.lbSize, 0);
            this.Controls.SetChildIndex(this.Size_text, 0);
            this.Controls.SetChildIndex(this.lbSciRefno, 0);
            this.Controls.SetChildIndex(this.SciRefno, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.BrandRefno_text, 0);
            this.Controls.SetChildIndex(this.lbSupplier, 0);
            this.Controls.SetChildIndex(this.lbColor, 0);
            this.Controls.SetChildIndex(this.Color_text, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.Qty_text, 0);
            this.Controls.SetChildIndex(this.Supplier_text, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox sp_text;
        private Win.UI.Label lbSP;
        private Win.UI.DisplayBox WKNO_text;
        private Win.UI.Label lbWKNO;
        private Win.UI.Label lbQty;
        private Win.UI.DisplayBox Unit_text;
        private Win.UI.Label lbUnit;
        private Win.UI.DisplayBox Size_text;
        private Win.UI.Label lbSize;
        private Win.UI.DisplayBox SciRefno;
        private Win.UI.Label lbSciRefno;
        private Win.UI.DisplayBox BrandRefno_text;
        private Win.UI.Label label1;
        private Win.UI.Label lbSupplier;
        private Win.UI.Label lbColor;
        private Win.UI.DisplayBox Color_text;
        private Win.UI.Button btnEncode;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.NumericBox Qty_text;
        private Win.UI.Label lbScale;
        private Win.UI.TextBox txtScale;
        private Win.UI.Label lbResult;
        private Win.UI.ComboBox comboResult;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Label lbRemark;
        private Win.UI.Label label2;
        private Class.txtuser txtuser1;
        private Win.UI.DisplayBox Supplier_text;
        private Win.UI.Label lbDate;
        private Win.UI.DateBox OvenDate;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
    }
}
