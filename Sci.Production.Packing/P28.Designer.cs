namespace Sci.Production.Packing
{
    partial class P28
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnNike = new Sci.Win.UI.Button();
            this.btnUA = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // btnNike
            // 
            this.btnNike.Location = new System.Drawing.Point(123, 12);
            this.btnNike.Name = "btnNike";
            this.btnNike.Size = new System.Drawing.Size(114, 30);
            this.btnNike.TabIndex = 0;
            this.btnNike.Text = "Nike";
            this.btnNike.UseVisualStyleBackColor = true;
            this.btnNike.Click += new System.EventHandler(this.BtnNike_Click);
            // 
            // btnUA
            // 
            this.btnUA.Location = new System.Drawing.Point(123, 58);
            this.btnUA.Name = "btnUA";
            this.btnUA.Size = new System.Drawing.Size(114, 30);
            this.btnUA.TabIndex = 0;
            this.btnUA.Text = "U. ARMOUR";
            this.btnUA.UseVisualStyleBackColor = true;
            this.btnUA.Click += new System.EventHandler(this.BtnUA_Click);
            // 
            // P28
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 103);
            this.Controls.Add(this.btnUA);
            this.Controls.Add(this.btnNike);
            this.IsToolbarVisible = false;
            this.Name = "P28";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P28. Upload Packing List - Cust CTN#";
            this.Controls.SetChildIndex(this.btnNike, 0);
            this.Controls.SetChildIndex(this.btnUA, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnNike;
        private Win.UI.Button btnUA;
    }
}