namespace Sci.Production.Packing
{
    partial class P18_InputWeight
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
            this.numWeight = new Sci.Win.UI.NumericBox();
            this.lbWeight = new Sci.Win.UI.Label();
            this.btn_Save = new Sci.Win.UI.Button();
            this.btnReadWeight = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // numWeight
            // 
            this.numWeight.BackColor = System.Drawing.Color.White;
            this.numWeight.DecimalPlaces = 3;
            this.numWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWeight.Location = new System.Drawing.Point(157, 22);
            this.numWeight.Name = "numWeight";
            this.numWeight.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWeight.Size = new System.Drawing.Size(146, 23);
            this.numWeight.TabIndex = 113;
            this.numWeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbWeight
            // 
            this.lbWeight.Location = new System.Drawing.Point(22, 22);
            this.lbWeight.Name = "lbWeight";
            this.lbWeight.Size = new System.Drawing.Size(131, 23);
            this.lbWeight.TabIndex = 112;
            this.lbWeight.Text = "Actual CTN# Weight";
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(223, 51);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(80, 30);
            this.btn_Save.TabIndex = 114;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // btnReadWeight
            // 
            this.btnReadWeight.Location = new System.Drawing.Point(106, 51);
            this.btnReadWeight.Name = "btnReadWeight";
            this.btnReadWeight.Size = new System.Drawing.Size(111, 30);
            this.btnReadWeight.TabIndex = 115;
            this.btnReadWeight.Text = "Read Weight";
            this.btnReadWeight.UseVisualStyleBackColor = true;
            this.btnReadWeight.Click += new System.EventHandler(this.BtnReadWeight_Click);
            // 
            // P18_InputWeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 93);
            this.Controls.Add(this.btnReadWeight);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.numWeight);
            this.Controls.Add(this.lbWeight);
            this.Name = "P18_InputWeight";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "P18_InputWeight";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericBox numWeight;
        private Win.UI.Label lbWeight;
        private Win.UI.Button btn_Save;
        private Win.UI.Button btnReadWeight;
    }
}