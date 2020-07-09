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
            this.displaySP = new Sci.Win.UI.DisplayBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.displayWKNO = new Sci.Win.UI.DisplayBox();
            this.labelWKNO = new Sci.Win.UI.Label();
            this.labelActiveQty = new Sci.Win.UI.Label();
            this.displayUnit = new Sci.Win.UI.DisplayBox();
            this.labelUnit = new Sci.Win.UI.Label();
            this.displaySize = new Sci.Win.UI.DisplayBox();
            this.labelSize = new Sci.Win.UI.Label();
            this.displaySCIRefno = new Sci.Win.UI.DisplayBox();
            this.labelSCIRefno = new Sci.Win.UI.Label();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelColor = new Sci.Win.UI.Label();
            this.displayColor = new Sci.Win.UI.DisplayBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.dateInspectDate = new Sci.Win.UI.DateBox();
            this.labelInspectDate = new Sci.Win.UI.Label();
            this.txtuserLabTech = new Sci.Production.Class.Txtuser();
            this.labelLabTech = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.labelResult = new Sci.Win.UI.Label();
            this.txtScale = new Sci.Win.UI.TextBox();
            this.labelScale = new Sci.Win.UI.Label();
            this.btnClose = new Sci.Win.UI.Button();
            this.numActiveQty = new Sci.Win.UI.NumericBox();
            this.displaySupplier = new Sci.Win.UI.DisplayBox();
            this.btnEdit = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnEdit);
            this.btmcont.Controls.Add(this.btnClose);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnClose, 0);
            this.btmcont.Controls.SetChildIndex(this.btnEdit, 0);
            // 
            // save
            // 
            this.save.Enabled = false;
            this.save.Text = "Edit";
            this.save.Visible = false;
            // 
            // displaySP
            // 
            this.displaySP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP.Location = new System.Drawing.Point(82, 11);
            this.displaySP.Name = "displaySP";
            this.displaySP.Size = new System.Drawing.Size(118, 21);
            this.displaySP.TabIndex = 102;
            // 
            // labelSP
            // 
            this.labelSP.Lines = 0;
            this.labelSP.Location = new System.Drawing.Point(9, 9);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(70, 23);
            this.labelSP.TabIndex = 101;
            this.labelSP.Text = "SP#";
            // 
            // displayWKNO
            // 
            this.displayWKNO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayWKNO.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayWKNO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayWKNO.Location = new System.Drawing.Point(82, 40);
            this.displayWKNO.Name = "displayWKNO";
            this.displayWKNO.Size = new System.Drawing.Size(118, 21);
            this.displayWKNO.TabIndex = 104;
            // 
            // labelWKNO
            // 
            this.labelWKNO.Lines = 0;
            this.labelWKNO.Location = new System.Drawing.Point(9, 38);
            this.labelWKNO.Name = "labelWKNO";
            this.labelWKNO.Size = new System.Drawing.Size(70, 23);
            this.labelWKNO.TabIndex = 103;
            this.labelWKNO.Text = "WKNO";
            // 
            // labelActiveQty
            // 
            this.labelActiveQty.Lines = 0;
            this.labelActiveQty.Location = new System.Drawing.Point(9, 67);
            this.labelActiveQty.Name = "labelActiveQty";
            this.labelActiveQty.Size = new System.Drawing.Size(70, 23);
            this.labelActiveQty.TabIndex = 105;
            this.labelActiveQty.Text = "Active Qty";
            // 
            // displayUnit
            // 
            this.displayUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnit.Location = new System.Drawing.Point(82, 97);
            this.displayUnit.Name = "displayUnit";
            this.displayUnit.Size = new System.Drawing.Size(118, 21);
            this.displayUnit.TabIndex = 108;
            // 
            // labelUnit
            // 
            this.labelUnit.Lines = 0;
            this.labelUnit.Location = new System.Drawing.Point(9, 95);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(70, 23);
            this.labelUnit.TabIndex = 107;
            this.labelUnit.Text = "Unit";
            // 
            // displaySize
            // 
            this.displaySize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySize.Location = new System.Drawing.Point(82, 126);
            this.displaySize.Name = "displaySize";
            this.displaySize.Size = new System.Drawing.Size(118, 21);
            this.displaySize.TabIndex = 110;
            // 
            // labelSize
            // 
            this.labelSize.Lines = 0;
            this.labelSize.Location = new System.Drawing.Point(9, 124);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(70, 23);
            this.labelSize.TabIndex = 109;
            this.labelSize.Text = "Size";
            // 
            // displaySCIRefno
            // 
            this.displaySCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Location = new System.Drawing.Point(288, 13);
            this.displaySCIRefno.Name = "displaySCIRefno";
            this.displaySCIRefno.Size = new System.Drawing.Size(150, 21);
            this.displaySCIRefno.TabIndex = 112;
            // 
            // labelSCIRefno
            // 
            this.labelSCIRefno.Lines = 0;
            this.labelSCIRefno.Location = new System.Drawing.Point(210, 11);
            this.labelSCIRefno.Name = "labelSCIRefno";
            this.labelSCIRefno.Size = new System.Drawing.Size(75, 23);
            this.labelSCIRefno.TabIndex = 111;
            this.labelSCIRefno.Text = "SCI Refno";
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(288, 40);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(150, 21);
            this.displayRefno.TabIndex = 114;
            // 
            // labelRefno
            // 
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(210, 40);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 113;
            this.labelRefno.Text = "Refno";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(210, 69);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 115;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelColor
            // 
            this.labelColor.Lines = 0;
            this.labelColor.Location = new System.Drawing.Point(210, 97);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 117;
            this.labelColor.Text = "Color";
            // 
            // displayColor
            // 
            this.displayColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColor.Location = new System.Drawing.Point(288, 99);
            this.displayColor.Name = "displayColor";
            this.displayColor.Size = new System.Drawing.Size(150, 21);
            this.displayColor.TabIndex = 118;
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
            this.groupBox1.Controls.Add(this.dateInspectDate);
            this.groupBox1.Controls.Add(this.labelInspectDate);
            this.groupBox1.Controls.Add(this.txtuserLabTech);
            this.groupBox1.Controls.Add(this.labelLabTech);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.labelRemark);
            this.groupBox1.Controls.Add(this.comboResult);
            this.groupBox1.Controls.Add(this.labelResult);
            this.groupBox1.Controls.Add(this.txtScale);
            this.groupBox1.Controls.Add(this.labelScale);
            this.groupBox1.Location = new System.Drawing.Point(9, 153);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 190);
            this.groupBox1.TabIndex = 120;
            this.groupBox1.TabStop = false;
            // 
            // dateInspectDate
            // 
            this.dateInspectDate.CausesValidation = false;
            this.dateInspectDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OvenDate", true));
            this.dateInspectDate.Enabled = false;
            this.dateInspectDate.Location = new System.Drawing.Point(102, 145);
            this.dateInspectDate.Name = "dateInspectDate";
            this.dateInspectDate.ReadOnly = true;
            this.dateInspectDate.Size = new System.Drawing.Size(120, 23);
            this.dateInspectDate.TabIndex = 123;
            // 
            // labelInspectDate
            // 
            this.labelInspectDate.Lines = 0;
            this.labelInspectDate.Location = new System.Drawing.Point(11, 145);
            this.labelInspectDate.Name = "labelInspectDate";
            this.labelInspectDate.Size = new System.Drawing.Size(85, 23);
            this.labelInspectDate.TabIndex = 122;
            this.labelInspectDate.Text = "Inspect Date";
            // 
            // txtuserLabTech
            // 
            this.txtuserLabTech.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "OvenInspector", true));
            this.txtuserLabTech.DisplayBox1Binding = "";
            this.txtuserLabTech.Enabled = false;
            this.txtuserLabTech.Location = new System.Drawing.Point(102, 112);
            this.txtuserLabTech.Name = "txtuserLabTech";
            this.txtuserLabTech.Size = new System.Drawing.Size(296, 23);
            this.txtuserLabTech.TabIndex = 121;
            this.txtuserLabTech.TextBox1Binding = "";
            // 
            // labelLabTech
            // 
            this.labelLabTech.Lines = 0;
            this.labelLabTech.Location = new System.Drawing.Point(11, 112);
            this.labelLabTech.Name = "labelLabTech";
            this.labelLabTech.Size = new System.Drawing.Size(85, 23);
            this.labelLabTech.TabIndex = 120;
            this.labelLabTech.Text = "Lab Tech";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OvenRemark", true));
            this.txtRemark.Enabled = false;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(99, 82);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(419, 23);
            this.txtRemark.TabIndex = 119;
            // 
            // labelRemark
            // 
            this.labelRemark.Lines = 0;
            this.labelRemark.Location = new System.Drawing.Point(11, 82);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(84, 23);
            this.labelRemark.TabIndex = 118;
            this.labelRemark.Text = "Remark";
            // 
            // comboResult
            // 
            this.comboResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboResult.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Oven", true));
            this.comboResult.Enabled = false;
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(99, 51);
            this.comboResult.Name = "comboResult";
            this.comboResult.ReadOnly = true;
            this.comboResult.Size = new System.Drawing.Size(103, 24);
            this.comboResult.TabIndex = 117;
            // 
            // labelResult
            // 
            this.labelResult.Lines = 0;
            this.labelResult.Location = new System.Drawing.Point(11, 51);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(85, 23);
            this.labelResult.TabIndex = 112;
            this.labelResult.Text = "Result";
            // 
            // txtScale
            // 
            this.txtScale.BackColor = System.Drawing.Color.White;
            this.txtScale.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OvenScale", true));
            this.txtScale.Enabled = false;
            this.txtScale.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScale.Location = new System.Drawing.Point(99, 19);
            this.txtScale.Name = "txtScale";
            this.txtScale.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtScale.Size = new System.Drawing.Size(103, 23);
            this.txtScale.TabIndex = 111;
            this.txtScale.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtScale_PopUp);
            this.txtScale.Validating += new System.ComponentModel.CancelEventHandler(this.txtScale_Validating);
            // 
            // labelScale
            // 
            this.labelScale.Lines = 0;
            this.labelScale.Location = new System.Drawing.Point(11, 19);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(84, 23);
            this.labelScale.TabIndex = 110;
            this.labelScale.Text = "Scale";
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
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // numActiveQty
            // 
            this.numActiveQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numActiveQty.DecimalPlaces = 2;
            this.numActiveQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numActiveQty.IsSupportEditMode = false;
            this.numActiveQty.Location = new System.Drawing.Point(82, 67);
            this.numActiveQty.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numActiveQty.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numActiveQty.Name = "numActiveQty";
            this.numActiveQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numActiveQty.ReadOnly = true;
            this.numActiveQty.Size = new System.Drawing.Size(118, 23);
            this.numActiveQty.TabIndex = 121;
            this.numActiveQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displaySupplier
            // 
            this.displaySupplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySupplier.Location = new System.Drawing.Point(288, 69);
            this.displaySupplier.Name = "displaySupplier";
            this.displaySupplier.Size = new System.Drawing.Size(150, 21);
            this.displaySupplier.TabIndex = 122;
            // 
            // btnEdit
            // 
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Location = new System.Drawing.Point(379, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 124;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // P07_Oven
            // 
            this.ClientSize = new System.Drawing.Size(549, 390);
            this.Controls.Add(this.displaySupplier);
            this.Controls.Add(this.numActiveQty);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.displayColor);
            this.Controls.Add(this.labelColor);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.displayRefno);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.displaySCIRefno);
            this.Controls.Add(this.labelSCIRefno);
            this.Controls.Add(this.displaySize);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.displayUnit);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.labelActiveQty);
            this.Controls.Add(this.displayWKNO);
            this.Controls.Add(this.labelWKNO);
            this.Controls.Add(this.displaySP);
            this.Controls.Add(this.labelSP);
            this.Name = "P07_Oven";
            this.Text = "Accessory Oven Test";
            this.WorkAlias = "AIR_Laboratory";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.displaySP, 0);
            this.Controls.SetChildIndex(this.labelWKNO, 0);
            this.Controls.SetChildIndex(this.displayWKNO, 0);
            this.Controls.SetChildIndex(this.labelActiveQty, 0);
            this.Controls.SetChildIndex(this.labelUnit, 0);
            this.Controls.SetChildIndex(this.displayUnit, 0);
            this.Controls.SetChildIndex(this.labelSize, 0);
            this.Controls.SetChildIndex(this.displaySize, 0);
            this.Controls.SetChildIndex(this.labelSCIRefno, 0);
            this.Controls.SetChildIndex(this.displaySCIRefno, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.displayRefno, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.labelColor, 0);
            this.Controls.SetChildIndex(this.displayColor, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.numActiveQty, 0);
            this.Controls.SetChildIndex(this.displaySupplier, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displaySP;
        private Win.UI.Label labelSP;
        private Win.UI.DisplayBox displayWKNO;
        private Win.UI.Label labelWKNO;
        private Win.UI.Label labelActiveQty;
        private Win.UI.DisplayBox displayUnit;
        private Win.UI.Label labelUnit;
        private Win.UI.DisplayBox displaySize;
        private Win.UI.Label labelSize;
        private Win.UI.DisplayBox displaySCIRefno;
        private Win.UI.Label labelSCIRefno;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelColor;
        private Win.UI.DisplayBox displayColor;
        private Win.UI.Button btnEncode;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.NumericBox numActiveQty;
        private Win.UI.Label labelScale;
        private Win.UI.TextBox txtScale;
        private Win.UI.Label labelResult;
        private Win.UI.ComboBox comboResult;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.Label labelLabTech;
        private Class.Txtuser txtuserLabTech;
        private Win.UI.DisplayBox displaySupplier;
        private Win.UI.Label labelInspectDate;
        private Win.UI.DateBox dateInspectDate;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnEdit;
    }
}
