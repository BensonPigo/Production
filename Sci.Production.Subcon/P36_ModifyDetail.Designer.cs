namespace Sci.Production.Subcon
{
    partial class P36_ModifyDetail
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
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtUnit = new Sci.Production.Class.Txtunit();
            this.labelAffectQty = new Sci.Win.UI.Label();
            this.numAffectQty = new Sci.Win.UI.NumericBox();
            this.numClaimAmt = new Sci.Win.UI.NumericBox();
            this.labelClaimAmt = new Sci.Win.UI.Label();
            this.numAdditionCharge = new Sci.Win.UI.NumericBox();
            this.labelAdditionCharge = new Sci.Win.UI.Label();
            this.labelReason = new Sci.Win.UI.Label();
            this.txtReason = new Sci.Production.Class.TxtReason();
            this.labelUnit = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.editDescription = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.TabIndex = 7;
            // 
            // undo
            // 
            this.undo.TabIndex = 3;
            // 
            // save
            // 
            this.save.TabIndex = 2;
            // 
            // left
            // 
            this.left.TabIndex = 0;
            // 
            // right
            // 
            this.right.TabIndex = 1;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(10, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 95;
            this.labelSPNo.Text = "SP#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "orderid", true));
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(88, 9);
            this.txtSPNo.MaxLength = 13;
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(138, 23);
            this.txtSPNo.TabIndex = 0;
            this.txtSPNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSPNo_Validating);
            // 
            // txtUnit
            // 
            this.txtUnit.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "unitid", true));
            this.txtUnit.DisplayBox1Binding = "";
            this.txtUnit.Location = new System.Drawing.Point(88, 111);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(320, 23);
            this.txtUnit.TabIndex = 5;
            this.txtUnit.TextBox1Binding = "";
            // 
            // labelAffectQty
            // 
            this.labelAffectQty.Location = new System.Drawing.Point(273, 9);
            this.labelAffectQty.Name = "labelAffectQty";
            this.labelAffectQty.Size = new System.Drawing.Size(109, 23);
            this.labelAffectQty.TabIndex = 98;
            this.labelAffectQty.Text = "Affect Qty";
            // 
            // numAffectQty
            // 
            this.numAffectQty.BackColor = System.Drawing.Color.White;
            this.numAffectQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "qty", true));
            this.numAffectQty.DecimalPlaces = 2;
            this.numAffectQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numAffectQty.Location = new System.Drawing.Point(385, 9);
            this.numAffectQty.MaxBytes = 11;
            this.numAffectQty.Maximum = new decimal(new int[] {
            1215752191,
            23,
            0,
            131072});
            this.numAffectQty.Minimum = new decimal(new int[] {
            1215752191,
            23,
            0,
            -2147352576});
            this.numAffectQty.Name = "numAffectQty";
            this.numAffectQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAffectQty.Size = new System.Drawing.Size(100, 23);
            this.numAffectQty.TabIndex = 1;
            this.numAffectQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numClaimAmt
            // 
            this.numClaimAmt.BackColor = System.Drawing.Color.White;
            this.numClaimAmt.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "amount", true));
            this.numClaimAmt.DecimalPlaces = 2;
            this.numClaimAmt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numClaimAmt.Location = new System.Drawing.Point(88, 42);
            this.numClaimAmt.MaxBytes = 12;
            this.numClaimAmt.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            131072});
            this.numClaimAmt.Minimum = new decimal(new int[] {
            -727379969,
            232,
            0,
            -2147352576});
            this.numClaimAmt.Name = "numClaimAmt";
            this.numClaimAmt.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numClaimAmt.Size = new System.Drawing.Size(100, 23);
            this.numClaimAmt.TabIndex = 2;
            this.numClaimAmt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelClaimAmt
            // 
            this.labelClaimAmt.Location = new System.Drawing.Point(10, 42);
            this.labelClaimAmt.Name = "labelClaimAmt";
            this.labelClaimAmt.Size = new System.Drawing.Size(75, 23);
            this.labelClaimAmt.TabIndex = 100;
            this.labelClaimAmt.Text = "Claim Amt";
            // 
            // numAdditionCharge
            // 
            this.numAdditionCharge.BackColor = System.Drawing.Color.White;
            this.numAdditionCharge.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "addition", true));
            this.numAdditionCharge.DecimalPlaces = 2;
            this.numAdditionCharge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numAdditionCharge.Location = new System.Drawing.Point(385, 42);
            this.numAdditionCharge.MaxBytes = 12;
            this.numAdditionCharge.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            131072});
            this.numAdditionCharge.Minimum = new decimal(new int[] {
            -727379969,
            232,
            0,
            -2147352576});
            this.numAdditionCharge.Name = "numAdditionCharge";
            this.numAdditionCharge.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAdditionCharge.Size = new System.Drawing.Size(100, 23);
            this.numAdditionCharge.TabIndex = 3;
            this.numAdditionCharge.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelAdditionCharge
            // 
            this.labelAdditionCharge.Location = new System.Drawing.Point(273, 42);
            this.labelAdditionCharge.Name = "labelAdditionCharge";
            this.labelAdditionCharge.Size = new System.Drawing.Size(109, 23);
            this.labelAdditionCharge.TabIndex = 102;
            this.labelAdditionCharge.Text = "Addition Charge";
            // 
            // labelReason
            // 
            this.labelReason.Location = new System.Drawing.Point(10, 76);
            this.labelReason.Name = "labelReason";
            this.labelReason.Size = new System.Drawing.Size(75, 23);
            this.labelReason.TabIndex = 104;
            this.labelReason.Text = "Reason";
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "reasonid", true));
            this.txtReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason.FormattingEnabled = true;
            this.txtReason.IsSupportUnselect = true;
            this.txtReason.Location = new System.Drawing.Point(88, 76);
            this.txtReason.Name = "txtReason";
            this.txtReason.OldText = "";
            this.txtReason.ReasonTypeID = "DebitNote_Factory";
            this.txtReason.Size = new System.Drawing.Size(168, 24);
            this.txtReason.TabIndex = 4;
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(10, 111);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(75, 23);
            this.labelUnit.TabIndex = 106;
            this.labelUnit.Text = "Unit";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(10, 146);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 107;
            this.labelDescription.Text = "Description";
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(88, 146);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(406, 162);
            this.editDescription.TabIndex = 6;
            // 
            // P36_ModifyDetail
            // 
            this.ClientSize = new System.Drawing.Size(584, 365);
            this.Controls.Add(this.editDescription);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelUnit);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.labelReason);
            this.Controls.Add(this.numAdditionCharge);
            this.Controls.Add(this.labelAdditionCharge);
            this.Controls.Add(this.numClaimAmt);
            this.Controls.Add(this.labelClaimAmt);
            this.Controls.Add(this.numAffectQty);
            this.Controls.Add(this.labelAffectQty);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelSPNo);
            this.DefaultControl = "txtSPNo";
            this.Name = "P36_ModifyDetail";
            this.OnLineHelpID = "Sci.Win.Subs.Input6A";
            this.Text = "P36 Detail";
            this.WorkAlias = "Localdebit_detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.txtUnit, 0);
            this.Controls.SetChildIndex(this.labelAffectQty, 0);
            this.Controls.SetChildIndex(this.numAffectQty, 0);
            this.Controls.SetChildIndex(this.labelClaimAmt, 0);
            this.Controls.SetChildIndex(this.numClaimAmt, 0);
            this.Controls.SetChildIndex(this.labelAdditionCharge, 0);
            this.Controls.SetChildIndex(this.numAdditionCharge, 0);
            this.Controls.SetChildIndex(this.labelReason, 0);
            this.Controls.SetChildIndex(this.txtReason, 0);
            this.Controls.SetChildIndex(this.labelUnit, 0);
            this.Controls.SetChildIndex(this.labelDescription, 0);
            this.Controls.SetChildIndex(this.editDescription, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtSPNo;
        private Class.Txtunit txtUnit;
        private Win.UI.Label labelAffectQty;
        private Win.UI.NumericBox numAffectQty;
        private Win.UI.NumericBox numClaimAmt;
        private Win.UI.Label labelClaimAmt;
        private Win.UI.NumericBox numAdditionCharge;
        private Win.UI.Label labelAdditionCharge;
        private Win.UI.Label labelReason;
        private Class.TxtReason txtReason;
        private Win.UI.Label labelUnit;
        private Win.UI.Label labelDescription;
        private Win.UI.EditBox editDescription;
    }
}
