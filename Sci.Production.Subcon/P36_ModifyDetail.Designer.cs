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
            this.label1 = new Sci.Win.UI.Label();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.txtunit1 = new Sci.Production.Class.txtunit();
            this.label2 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.numericBox2 = new Sci.Win.UI.NumericBox();
            this.label3 = new Sci.Win.UI.Label();
            this.numericBox3 = new Sci.Win.UI.NumericBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtReason1 = new Sci.Production.Class.txtReason();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.editBox1 = new Sci.Win.UI.EditBox();
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
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "SP#";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "orderid", true));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(88, 9);
            this.textBox1.MaxLength = 13;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(138, 23);
            this.textBox1.TabIndex = 0;
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // txtunit1
            // 
            this.txtunit1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "unitid", true));
            this.txtunit1.DisplayBox1Binding = "";
            this.txtunit1.Location = new System.Drawing.Point(88, 111);
            this.txtunit1.Name = "txtunit1";
            this.txtunit1.Size = new System.Drawing.Size(320, 23);
            this.txtunit1.TabIndex = 5;
            this.txtunit1.TextBox1Binding = "";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(273, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Affect Qty";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.White;
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "qty", true));
            this.numericBox1.DecimalPlaces = 2;
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox1.Location = new System.Drawing.Point(385, 9);
            this.numericBox1.MaxBytes = 8;
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.Size = new System.Drawing.Size(100, 23);
            this.numericBox1.TabIndex = 1;
            this.numericBox1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numericBox2
            // 
            this.numericBox2.BackColor = System.Drawing.Color.White;
            this.numericBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "amount", true));
            this.numericBox2.DecimalPlaces = 2;
            this.numericBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox2.Location = new System.Drawing.Point(88, 42);
            this.numericBox2.MaxBytes = 12;
            this.numericBox2.Name = "numericBox2";
            this.numericBox2.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox2.Size = new System.Drawing.Size(100, 23);
            this.numericBox2.TabIndex = 2;
            this.numericBox2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox2.Validated += new System.EventHandler(this.numericBox2_Validated);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(10, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 100;
            this.label3.Text = "Claim Amt";
            // 
            // numericBox3
            // 
            this.numericBox3.BackColor = System.Drawing.Color.White;
            this.numericBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "addition", true));
            this.numericBox3.DecimalPlaces = 2;
            this.numericBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox3.Location = new System.Drawing.Point(385, 42);
            this.numericBox3.MaxBytes = 12;
            this.numericBox3.Name = "numericBox3";
            this.numericBox3.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox3.Size = new System.Drawing.Size(100, 23);
            this.numericBox3.TabIndex = 3;
            this.numericBox3.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox3.Validated += new System.EventHandler(this.numericBox2_Validated);
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(273, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 23);
            this.label4.TabIndex = 102;
            this.label4.Text = "Addition Charge";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(10, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 104;
            this.label5.Text = "Reason";
            // 
            // txtReason1
            // 
            this.txtReason1.BackColor = System.Drawing.Color.White;
            this.txtReason1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "reasonid", true));
            this.txtReason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason1.FormattingEnabled = true;
            this.txtReason1.IsSupportUnselect = true;
            this.txtReason1.Location = new System.Drawing.Point(88, 76);
            this.txtReason1.Name = "txtReason1";
            this.txtReason1.ReasonTypeID = "DebitNote_Factory";
            this.txtReason1.Size = new System.Drawing.Size(168, 24);
            this.txtReason1.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(10, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 106;
            this.label6.Text = "Unit";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(10, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 107;
            this.label7.Text = "Description";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(88, 146);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(406, 162);
            this.editBox1.TabIndex = 6;
            // 
            // P36_ModifyDetail
            // 
            this.ClientSize = new System.Drawing.Size(584, 365);
            this.Controls.Add(this.editBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtReason1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtunit1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.DefaultControl = "textBox1";
            this.Name = "P36_ModifyDetail";
            this.Text = "P36 Detail";
            this.WorkAlias = "Localdebit_detail";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.txtunit1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.numericBox1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.numericBox2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.numericBox3, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtReason1, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.editBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.TextBox textBox1;
        private Class.txtunit txtunit1;
        private Win.UI.Label label2;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.NumericBox numericBox2;
        private Win.UI.Label label3;
        private Win.UI.NumericBox numericBox3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Class.txtReason txtReason1;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.EditBox editBox1;
    }
}
