namespace Sci.Production.Shipping
{
    partial class B53
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
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.txtsubcon1 = new Sci.Production.Class.txtsubcon();
            this.numericBox4 = new Sci.Win.UI.NumericBox();
            this.label14 = new Sci.Win.UI.Label();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.displayBox5 = new Sci.Win.UI.DisplayBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(751, 312);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayBox2);
            this.detailcont.Controls.Add(this.txtsubcon1);
            this.detailcont.Controls.Add(this.numericBox4);
            this.detailcont.Controls.Add(this.label14);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.displayBox5);
            this.detailcont.Controls.Add(this.textBox1);
            this.detailcont.Controls.Add(this.displayBox3);
            this.detailcont.Controls.Add(this.editBox1);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(751, 274);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 274);
            this.detailbtm.Size = new System.Drawing.Size(751, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(751, 312);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(759, 341);
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UnitID", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(109, 154);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(80, 23);
            this.displayBox2.TabIndex = 60;
            // 
            // txtsubcon1
            // 
            this.txtsubcon1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppid", true));
            this.txtsubcon1.DisplayBox1Binding = "";
            this.txtsubcon1.IsIncludeJunk = false;
            this.txtsubcon1.Location = new System.Drawing.Point(474, 13);
            this.txtsubcon1.Name = "txtsubcon1";
            this.txtsubcon1.Size = new System.Drawing.Size(170, 23);
            this.txtsubcon1.TabIndex = 59;
            this.txtsubcon1.TextBox1Binding = "";
            // 
            // numericBox4
            // 
            this.numericBox4.BackColor = System.Drawing.Color.White;
            this.numericBox4.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsKg", true));
            this.numericBox4.DecimalPlaces = 4;
            this.numericBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBox4.Location = new System.Drawing.Point(474, 186);
            this.numericBox4.Name = "numericBox4";
            this.numericBox4.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox4.Size = new System.Drawing.Size(99, 23);
            this.numericBox4.TabIndex = 40;
            this.numericBox4.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(350, 186);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 40);
            this.label14.TabIndex = 56;
            this.label14.Text = "Weight";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(325, 13);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 53;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // displayBox5
            // 
            this.displayBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox5.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CustomsUnit", true));
            this.displayBox5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox5.Location = new System.Drawing.Point(474, 154);
            this.displayBox5.Name = "displayBox5";
            this.displayBox5.Size = new System.Drawing.Size(80, 23);
            this.displayBox5.TabIndex = 52;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(474, 122);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(263, 23);
            this.textBox1.TabIndex = 36;
            this.textBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Category", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(109, 122);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(190, 23);
            this.displayBox3.TabIndex = 50;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(109, 43);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(628, 70);
            this.editBox1.TabIndex = 49;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RefNo", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(109, 13);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(190, 23);
            this.displayBox1.TabIndex = 48;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(394, 13);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 23);
            this.label10.TabIndex = 47;
            this.label10.Text = "Supplier";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(350, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 23);
            this.label9.TabIndex = 46;
            this.label9.Text = "Customs Unit";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(350, 122);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(120, 23);
            this.label7.TabIndex = 44;
            this.label7.Text = "Good\'s Description";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(15, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 23);
            this.label6.TabIndex = 43;
            this.label6.Text = "Unit";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(15, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 23);
            this.label5.TabIndex = 41;
            this.label5.Text = "Material Type";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(15, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 23);
            this.label3.TabIndex = 39;
            this.label3.Text = "Description";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 23);
            this.label1.TabIndex = 35;
            this.label1.Text = "RefNo";
            // 
            // B53
            // 
            this.ClientSize = new System.Drawing.Size(759, 374);
            this.DefaultOrder = "RefNo";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B53";
            this.Text = "B53. Material Basic data - Local Purchase Item";
            this.UniqueExpress = "RefNo";
            this.WorkAlias = "LocalItem";
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

        private Win.UI.DisplayBox displayBox2;
        private Class.txtsubcon txtsubcon1;
        private Win.UI.NumericBox numericBox4;
        private Win.UI.Label label14;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.DisplayBox displayBox5;
        private Win.UI.TextBox textBox1;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.EditBox editBox1;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
    }
}
