namespace Sci.Production.Shipping
{
    partial class B52
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
            this.displayBox6 = new Sci.Win.UI.DisplayBox();
            this.numericBox4 = new Sci.Win.UI.NumericBox();
            this.label14 = new Sci.Win.UI.Label();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.displayBox5 = new Sci.Win.UI.DisplayBox();
            this.txtunit1 = new Sci.Production.Class.txtunit();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.textBox1 = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(771, 338);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.textBox1);
            this.detailcont.Controls.Add(this.displayBox6);
            this.detailcont.Controls.Add(this.numericBox4);
            this.detailcont.Controls.Add(this.label14);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.displayBox5);
            this.detailcont.Controls.Add(this.txtunit1);
            this.detailcont.Controls.Add(this.displayBox3);
            this.detailcont.Controls.Add(this.comboBox1);
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.displayBox2);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(771, 300);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 300);
            this.detailbtm.Size = new System.Drawing.Size(771, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(779, 338);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(779, 367);
            // 
            // displayBox6
            // 
            this.displayBox6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox6.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BrandID", true));
            this.displayBox6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox6.Location = new System.Drawing.Point(500, 13);
            this.displayBox6.Name = "displayBox6";
            this.displayBox6.Size = new System.Drawing.Size(94, 23);
            this.displayBox6.TabIndex = 66;
            // 
            // numericBox4
            // 
            this.numericBox4.BackColor = System.Drawing.Color.White;
            this.numericBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsKg", true));
            this.numericBox4.DecimalPlaces = 4;
            this.numericBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox4.Location = new System.Drawing.Point(550, 218);
            this.numericBox4.Name = "numericBox4";
            this.numericBox4.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox4.Size = new System.Drawing.Size(80, 23);
            this.numericBox4.TabIndex = 41;
            this.numericBox4.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(454, 218);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(92, 40);
            this.label14.TabIndex = 61;
            this.label14.Text = "Weight (kgs/PX)";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(325, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 57;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // displayBox5
            // 
            this.displayBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox5.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CustomsUnit", true));
            this.displayBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox5.Location = new System.Drawing.Point(550, 186);
            this.displayBox5.Name = "displayBox5";
            this.displayBox5.Size = new System.Drawing.Size(80, 23);
            this.displayBox5.TabIndex = 56;
            // 
            // txtunit1
            // 
            this.txtunit1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "UsageUnit", true));
            this.txtunit1.DisplayBox1Binding = "";
            this.txtunit1.Location = new System.Drawing.Point(109, 186);
            this.txtunit1.Name = "txtunit1";
            this.txtunit1.Size = new System.Drawing.Size(320, 23);
            this.txtunit1.TabIndex = 54;
            this.txtunit1.TextBox1Binding = "";
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MtlTypeID", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(550, 154);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(190, 23);
            this.displayBox3.TabIndex = 53;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(109, 154);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 52;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescDetail", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(109, 75);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(654, 70);
            this.editBox1.TabIndex = 51;
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(109, 45);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(654, 23);
            this.displayBox2.TabIndex = 50;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RefNo", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(109, 13);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(190, 23);
            this.displayBox1.TabIndex = 49;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(411, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 23);
            this.label10.TabIndex = 48;
            this.label10.Text = "Brand";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(454, 186);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 23);
            this.label9.TabIndex = 47;
            this.label9.Text = "Customs Unit";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(13, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 23);
            this.label7.TabIndex = 45;
            this.label7.Text = "Good\'s Description";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(13, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 23);
            this.label6.TabIndex = 44;
            this.label6.Text = "Usage Unit";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(454, 154);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 23);
            this.label5.TabIndex = 42;
            this.label5.Text = "Material Type";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(13, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 23);
            this.label4.TabIndex = 40;
            this.label4.Text = "Type";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(13, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 46);
            this.label3.TabIndex = 39;
            this.label3.Text = "Description";
            this.label3.TextStyle.Alignment = System.Drawing.ContentAlignment.TopLeft;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 23);
            this.label2.TabIndex = 36;
            this.label2.Text = "Description";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 35;
            this.label1.Text = "RefNo";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(138, 218);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(263, 23);
            this.textBox1.TabIndex = 67;
            this.textBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // B52
            // 
            this.ClientSize = new System.Drawing.Size(779, 400);
            this.DefaultOrder = "RefNo";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B52";
            this.Text = "B52. Material Basic data - Fabric/Accessory";
            this.UniqueExpress = "SCIRefno";
            this.WorkAlias = "Fabric";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayBox6;
        private Win.UI.NumericBox numericBox4;
        private Win.UI.Label label14;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.DisplayBox displayBox5;
        private Class.txtunit txtunit1;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.EditBox editBox1;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox textBox1;
    }
}
