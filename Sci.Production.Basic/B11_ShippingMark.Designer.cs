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
            this.editA = new Sci.Win.UI.EditBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.editB = new Sci.Win.UI.EditBox();
            this.editC = new Sci.Win.UI.EditBox();
            this.editD = new Sci.Win.UI.EditBox();
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
            this.edit.TabIndex = 0;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(694, 5);
            this.save.Size = new System.Drawing.Size(80, 34);
            this.save.TabIndex = 1;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(774, 5);
            this.undo.Size = new System.Drawing.Size(80, 34);
            this.undo.TabIndex = 2;
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
            // editA
            // 
            this.editA.BackColor = System.Drawing.Color.White;
            this.editA.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkFront", true));
            this.editA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editA.Location = new System.Drawing.Point(31, 7);
            this.editA.Multiline = true;
            this.editA.Name = "editA";
            this.editA.Size = new System.Drawing.Size(285, 172);
            this.editA.TabIndex = 0;
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
            // editB
            // 
            this.editB.BackColor = System.Drawing.Color.White;
            this.editB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkBack", true));
            this.editB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editB.Location = new System.Drawing.Point(575, 7);
            this.editB.Multiline = true;
            this.editB.Name = "editB";
            this.editB.Size = new System.Drawing.Size(285, 172);
            this.editB.TabIndex = 1;
            // 
            // editC
            // 
            this.editC.BackColor = System.Drawing.Color.White;
            this.editC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkLeft", true));
            this.editC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editC.Location = new System.Drawing.Point(31, 185);
            this.editC.Multiline = true;
            this.editC.Name = "editC";
            this.editC.Size = new System.Drawing.Size(285, 172);
            this.editC.TabIndex = 2;
            // 
            // editD
            // 
            this.editD.BackColor = System.Drawing.Color.White;
            this.editD.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkRight", true));
            this.editD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editD.Location = new System.Drawing.Point(575, 185);
            this.editD.Multiline = true;
            this.editD.Name = "editD";
            this.editD.Size = new System.Drawing.Size(285, 172);
            this.editD.TabIndex = 3;
            // 
            // B11_ShippingMark
            // 
            this.ClientSize = new System.Drawing.Size(864, 408);
            this.Controls.Add(this.editD);
            this.Controls.Add(this.editC);
            this.Controls.Add(this.editB);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.editA);
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
            this.Controls.SetChildIndex(this.editA, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.editB, 0);
            this.Controls.SetChildIndex(this.editC, 0);
            this.Controls.SetChildIndex(this.editD, 0);
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
        private Win.UI.EditBox editA;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.EditBox editB;
        private Win.UI.EditBox editC;
        private Win.UI.EditBox editD;
    }
}
