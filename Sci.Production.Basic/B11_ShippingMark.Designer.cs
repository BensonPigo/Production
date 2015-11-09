namespace Sci.Production.Basic
{
    partial class B11_ShippingMark
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
            this.label4 = new Sci.Win.UI.Label();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.editBox2 = new Sci.Win.UI.EditBox();
            this.editBox3 = new Sci.Win.UI.EditBox();
            this.editBox4 = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 364);
            this.btmcont.Size = new System.Drawing.Size(864, 44);
            // 
            // edit
            // 
            this.edit.Size = new System.Drawing.Size(80, 34);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(694, 5);
            this.save.Size = new System.Drawing.Size(80, 34);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(774, 5);
            this.undo.Size = new System.Drawing.Size(80, 34);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "(A)";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(547, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "(B)";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(3, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "(C)";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(547, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "(D)";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkFront", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(31, 7);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(285, 172);
            this.editBox1.TabIndex = 98;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sci.Production.Basic.Properties.Resources.Carton;
            this.pictureBox1.Location = new System.Drawing.Point(322, 115);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(219, 129);
            this.pictureBox1.TabIndex = 99;
            this.pictureBox1.TabStop = false;
            // 
            // editBox2
            // 
            this.editBox2.BackColor = System.Drawing.Color.White;
            this.editBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkBack", true));
            this.editBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox2.Location = new System.Drawing.Point(575, 7);
            this.editBox2.Multiline = true;
            this.editBox2.Name = "editBox2";
            this.editBox2.Size = new System.Drawing.Size(285, 172);
            this.editBox2.TabIndex = 100;
            // 
            // editBox3
            // 
            this.editBox3.BackColor = System.Drawing.Color.White;
            this.editBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkLeft", true));
            this.editBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox3.Location = new System.Drawing.Point(31, 185);
            this.editBox3.Multiline = true;
            this.editBox3.Name = "editBox3";
            this.editBox3.Size = new System.Drawing.Size(285, 172);
            this.editBox3.TabIndex = 101;
            // 
            // editBox4
            // 
            this.editBox4.BackColor = System.Drawing.Color.White;
            this.editBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkRight", true));
            this.editBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox4.Location = new System.Drawing.Point(575, 185);
            this.editBox4.Multiline = true;
            this.editBox4.Name = "editBox4";
            this.editBox4.Size = new System.Drawing.Size(285, 172);
            this.editBox4.TabIndex = 102;
            // 
            // B11_ShippingMark
            // 
            this.ClientSize = new System.Drawing.Size(864, 408);
            this.Controls.Add(this.editBox4);
            this.Controls.Add(this.editBox3);
            this.Controls.Add(this.editBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.editBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "B11_ShippingMark";
            this.WorkAlias = "CustCD";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.editBox1, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.editBox2, 0);
            this.Controls.SetChildIndex(this.editBox3, 0);
            this.Controls.SetChildIndex(this.editBox4, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.EditBox editBox1;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.EditBox editBox2;
        private Win.UI.EditBox editBox3;
        private Win.UI.EditBox editBox4;
    }
}
