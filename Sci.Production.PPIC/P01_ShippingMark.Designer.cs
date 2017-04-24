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
            this.editA = new Sci.Win.UI.EditBox();
            this.labelA = new Sci.Win.UI.Label();
            this.labelC = new Sci.Win.UI.Label();
            this.editC = new Sci.Win.UI.EditBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.labelD = new Sci.Win.UI.Label();
            this.editD = new Sci.Win.UI.EditBox();
            this.labelB = new Sci.Win.UI.Label();
            this.editB = new Sci.Win.UI.EditBox();
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
            // editA
            // 
            this.editA.BackColor = System.Drawing.Color.White;
            this.editA.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkFront", true));
            this.editA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editA.Location = new System.Drawing.Point(31, 4);
            this.editA.Multiline = true;
            this.editA.Name = "editA";
            this.editA.Size = new System.Drawing.Size(285, 172);
            this.editA.TabIndex = 0;
            // 
            // labelA
            // 
            this.labelA.Lines = 0;
            this.labelA.Location = new System.Drawing.Point(3, 4);
            this.labelA.Name = "labelA";
            this.labelA.Size = new System.Drawing.Size(23, 23);
            this.labelA.TabIndex = 95;
            this.labelA.Text = "(A)";
            // 
            // labelC
            // 
            this.labelC.Lines = 0;
            this.labelC.Location = new System.Drawing.Point(3, 182);
            this.labelC.Name = "labelC";
            this.labelC.Size = new System.Drawing.Size(23, 23);
            this.labelC.TabIndex = 97;
            this.labelC.Text = "(C)";
            // 
            // editC
            // 
            this.editC.BackColor = System.Drawing.Color.White;
            this.editC.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkLeft", true));
            this.editC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editC.Location = new System.Drawing.Point(31, 182);
            this.editC.Multiline = true;
            this.editC.Name = "editC";
            this.editC.Size = new System.Drawing.Size(285, 172);
            this.editC.TabIndex = 2;
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
            // labelD
            // 
            this.labelD.Lines = 0;
            this.labelD.Location = new System.Drawing.Point(547, 182);
            this.labelD.Name = "labelD";
            this.labelD.Size = new System.Drawing.Size(23, 23);
            this.labelD.TabIndex = 102;
            this.labelD.Text = "(D)";
            // 
            // editD
            // 
            this.editD.BackColor = System.Drawing.Color.White;
            this.editD.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkRight", true));
            this.editD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editD.Location = new System.Drawing.Point(575, 182);
            this.editD.Multiline = true;
            this.editD.Name = "editD";
            this.editD.Size = new System.Drawing.Size(285, 172);
            this.editD.TabIndex = 3;
            // 
            // labelB
            // 
            this.labelB.Lines = 0;
            this.labelB.Location = new System.Drawing.Point(547, 4);
            this.labelB.Name = "labelB";
            this.labelB.Size = new System.Drawing.Size(23, 23);
            this.labelB.TabIndex = 100;
            this.labelB.Text = "(B)";
            // 
            // editB
            // 
            this.editB.BackColor = System.Drawing.Color.White;
            this.editB.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MarkBack", true));
            this.editB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editB.Location = new System.Drawing.Point(575, 4);
            this.editB.Multiline = true;
            this.editB.Name = "editB";
            this.editB.Size = new System.Drawing.Size(285, 172);
            this.editB.TabIndex = 1;
            // 
            // P01_ShippingMark
            // 
            this.ClientSize = new System.Drawing.Size(869, 402);
            this.Controls.Add(this.labelD);
            this.Controls.Add(this.editD);
            this.Controls.Add(this.labelB);
            this.Controls.Add(this.editB);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelC);
            this.Controls.Add(this.editC);
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.editA);
            this.Name = "P01_ShippingMark";
            this.Text = "Shipping Mark";
            this.WorkAlias = "Orders";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.editA, 0);
            this.Controls.SetChildIndex(this.labelA, 0);
            this.Controls.SetChildIndex(this.editC, 0);
            this.Controls.SetChildIndex(this.labelC, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.editB, 0);
            this.Controls.SetChildIndex(this.labelB, 0);
            this.Controls.SetChildIndex(this.editD, 0);
            this.Controls.SetChildIndex(this.labelD, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.EditBox editA;
        private Win.UI.Label labelA;
        private Win.UI.Label labelC;
        private Win.UI.EditBox editC;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label labelD;
        private Win.UI.EditBox editD;
        private Win.UI.Label labelB;
        private Win.UI.EditBox editB;
    }
}
