namespace Sci.Production.Packing
{
    partial class P18_MessageBox
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
            this.btnLacking = new Sci.Win.UI.Button();
            this.btnContinue = new Sci.Win.UI.Button();
            this.labelmsg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLacking
            // 
            this.btnLacking.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLacking.Location = new System.Drawing.Point(19, 90);
            this.btnLacking.Name = "btnLacking";
            this.btnLacking.Size = new System.Drawing.Size(99, 44);
            this.btnLacking.TabIndex = 1;
            this.btnLacking.Text = "Lacking Pieces";
            this.btnLacking.UseVisualStyleBackColor = true;
            this.btnLacking.Click += new System.EventHandler(this.BtnLacking_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnContinue.Location = new System.Drawing.Point(154, 90);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(98, 44);
            this.btnContinue.TabIndex = 5;
            this.btnContinue.Text = "Continue Scanning";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // labelmsg
            // 
            this.labelmsg.AutoSize = true;
            this.labelmsg.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelmsg.Location = new System.Drawing.Point(12, 18);
            this.labelmsg.Name = "labelmsg";
            this.labelmsg.Size = new System.Drawing.Size(240, 20);
            this.labelmsg.TabIndex = 6;
            this.labelmsg.Text = "This CTN# is not scan finished. ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(15, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Please select option.";
            // 
            // P18_MessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 146);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelmsg);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.btnLacking);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "P18_MessageBox";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Question";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P18_MessageBox_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnLacking;
        private Win.UI.Button btnContinue;
        private System.Windows.Forms.Label labelmsg;
        private System.Windows.Forms.Label label1;
    }
}