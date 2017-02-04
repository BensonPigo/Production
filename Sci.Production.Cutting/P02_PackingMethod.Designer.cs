namespace Sci.Production.Cutting
{
    partial class P02_PackingMethod
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
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.numericBox1 = new Sci.Win.UI.NumericBox();
            this.button1 = new Sci.Win.UI.Button();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 415);
            this.btmcont.Size = new System.Drawing.Size(632, 40);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 51);
            this.gridcont.Size = new System.Drawing.Size(172, 353);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(542, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(462, 5);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 23);
            this.label1.TabIndex = 98;
            this.label1.Text = "Order List";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(190, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "Packing Method";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(368, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 23);
            this.label3.TabIndex = 101;
            this.label3.Text = "Qty/Carton";
            // 
            // numericBox1
            // 
            this.numericBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numericBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CtnQty", true));
            this.numericBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numericBox1.Location = new System.Drawing.Point(368, 49);
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBox1.ReadOnly = true;
            this.numericBox1.Size = new System.Drawing.Size(100, 23);
            this.numericBox1.TabIndex = 102;
            this.numericBox1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(474, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 30);
            this.button1.TabIndex = 103;
            this.button1.Text = "Breakdown";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "packing", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editBox1.Location = new System.Drawing.Point(190, 78);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.ReadOnly = true;
            this.editBox1.Size = new System.Drawing.Size(418, 331);
            this.editBox1.TabIndex = 104;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Packingmethod", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(190, 49);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(172, 23);
            this.displayBox1.TabIndex = 105;
            // 
            // P02_PackingMethod
            // 
            this.ClientSize = new System.Drawing.Size(632, 455);
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.editBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.GridEdit = false;
            this.GridPopUp = false;
            this.KeyField1 = "cuttingsp";
            this.Name = "P02_PackingMethod";
            this.Text = "Packing Method";
            this.WorkAlias = "orders";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.numericBox1, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.editBox1, 0);
            this.Controls.SetChildIndex(this.displayBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.NumericBox numericBox1;
        private Win.UI.Button button1;
        private Win.UI.EditBox editBox1;
        private Win.UI.DisplayBox displayBox1;
    }
}
