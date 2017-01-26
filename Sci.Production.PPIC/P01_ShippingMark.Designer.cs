namespace Sci.Production.PPIC
{
    partial class P01_ShippingMark
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
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.editBox2 = new Sci.Win.UI.EditBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.label3 = new Sci.Win.UI.Label();
            this.editBox3 = new Sci.Win.UI.EditBox();
            this.label4 = new Sci.Win.UI.Label();
            this.editBox4 = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 362);
            this.btmcont.Size = new System.Drawing.Size(869, 40);
            // 
            // edit
            // 
            this.edit.TabIndex = 0;
            this.edit.Visible = false;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(699, 5);
            this.save.TabIndex = 1;
            this.save.Visible = false;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(779, 5);
            this.undo.TabIndex = 2;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkFront", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(31, 4);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(285, 172);
            this.editBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(3, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 23);
            this.label1.TabIndex = 95;
            this.label1.Text = "(A)";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(3, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 23);
            this.label2.TabIndex = 97;
            this.label2.Text = "(C)";
            // 
            // editBox2
            // 
            this.editBox2.BackColor = System.Drawing.Color.White;
            this.editBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkLeft", true));
            this.editBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox2.Location = new System.Drawing.Point(31, 182);
            this.editBox2.Multiline = true;
            this.editBox2.Name = "editBox2";
            this.editBox2.Size = new System.Drawing.Size(285, 172);
            this.editBox2.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Sci.Production.PPIC.Properties.Resources.Carton;
            this.pictureBox1.Location = new System.Drawing.Point(322, 115);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(219, 129);
            this.pictureBox1.TabIndex = 98;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(547, 182);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 23);
            this.label3.TabIndex = 102;
            this.label3.Text = "(D)";
            // 
            // editBox3
            // 
            this.editBox3.BackColor = System.Drawing.Color.White;
            this.editBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkRight", true));
            this.editBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox3.Location = new System.Drawing.Point(575, 182);
            this.editBox3.Multiline = true;
            this.editBox3.Name = "editBox3";
            this.editBox3.Size = new System.Drawing.Size(285, 172);
            this.editBox3.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(547, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "(B)";
            // 
            // editBox4
            // 
            this.editBox4.BackColor = System.Drawing.Color.White;
            this.editBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkBack", true));
            this.editBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox4.Location = new System.Drawing.Point(575, 4);
            this.editBox4.Multiline = true;
            this.editBox4.Name = "editBox4";
            this.editBox4.Size = new System.Drawing.Size(285, 172);
            this.editBox4.TabIndex = 1;
            // 
            // P01_ShippingMark
            // 
            this.ClientSize = new System.Drawing.Size(869, 402);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.editBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.editBox4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editBox1);
            this.Name = "P01_ShippingMark";
            this.Text = "Shipping Mark";
            this.WorkAlias = "Orders";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.editBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.editBox2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.editBox4, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.editBox3, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.EditBox editBox1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.EditBox editBox2;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label label3;
        private Win.UI.EditBox editBox3;
        private Win.UI.Label label4;
        private Win.UI.EditBox editBox4;
    }
}
