namespace Sci.Production.Quality
{
    partial class P10_ToReport
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioWash20 = new Sci.Win.UI.RadioButton();
            this.radioPhysical = new Sci.Win.UI.RadioButton();
            this.radioWash18 = new Sci.Win.UI.RadioButton();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnToPDF = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioWash20);
            this.groupBox1.Controls.Add(this.radioPhysical);
            this.groupBox1.Controls.Add(this.radioWash18);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(398, 128);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // radioWash20
            // 
            this.radioWash20.AutoSize = true;
            this.radioWash20.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioWash20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioWash20.Location = new System.Drawing.Point(17, 52);
            this.radioWash20.Name = "radioWash20";
            this.radioWash20.Size = new System.Drawing.Size(307, 24);
            this.radioWash20.TabIndex = 7;
            this.radioWash20.Text = "Finished Garment Wash Test Ver. 2020";
            this.radioWash20.UseVisualStyleBackColor = true;
            // 
            // radioPhysical
            // 
            this.radioPhysical.AutoSize = true;
            this.radioPhysical.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioPhysical.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioPhysical.Location = new System.Drawing.Point(17, 82);
            this.radioPhysical.Name = "radioPhysical";
            this.radioPhysical.Size = new System.Drawing.Size(250, 24);
            this.radioPhysical.TabIndex = 8;
            this.radioPhysical.Text = "Finished Garment Physical Test";
            this.radioPhysical.UseVisualStyleBackColor = true;
            // 
            // radioWash18
            // 
            this.radioWash18.AutoSize = true;
            this.radioWash18.Checked = true;
            this.radioWash18.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioWash18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioWash18.Location = new System.Drawing.Point(17, 22);
            this.radioWash18.Name = "radioWash18";
            this.radioWash18.Size = new System.Drawing.Size(307, 24);
            this.radioWash18.TabIndex = 6;
            this.radioWash18.TabStop = true;
            this.radioWash18.Text = "Finished Garment Wash Test Ver. 2018";
            this.radioWash18.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(451, 84);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(451, 48);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 7;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnToPDF
            // 
            this.btnToPDF.Location = new System.Drawing.Point(451, 12);
            this.btnToPDF.Name = "btnToPDF";
            this.btnToPDF.Size = new System.Drawing.Size(80, 30);
            this.btnToPDF.TabIndex = 9;
            this.btnToPDF.Text = "To PDF";
            this.btnToPDF.UseVisualStyleBackColor = true;
            this.btnToPDF.Click += new System.EventHandler(this.BtnToPDF_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(451, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 183;
            this.label5.Text = "Pager Size A4";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // P04_ToReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 162);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnToPDF);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.groupBox1);
            this.Name = "P04_ToReport";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "To Report";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.btnToExcel, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnToPDF, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.RadioButton radioWash20;
        private Win.UI.RadioButton radioPhysical;
        private Win.UI.RadioButton radioWash18;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnToPDF;
        private Win.UI.Label label5;
    }
}