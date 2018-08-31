namespace Sci.Production.PPIC
{
    partial class P17
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
            this.dateRangeImport = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.btn_Update = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // dateRangeImport
            // 
            // 
            // 
            // 
            this.dateRangeImport.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeImport.DateBox1.Name = "";
            this.dateRangeImport.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeImport.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeImport.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeImport.DateBox2.Name = "";
            this.dateRangeImport.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeImport.DateBox2.TabIndex = 1;
            this.dateRangeImport.Location = new System.Drawing.Point(143, 12);
            this.dateRangeImport.Name = "dateRangeImport";
            this.dateRangeImport.Size = new System.Drawing.Size(280, 23);
            this.dateRangeImport.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Update Date Range";
            // 
            // btn_Update
            // 
            this.btn_Update.Location = new System.Drawing.Point(343, 41);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(80, 30);
            this.btn_Update.TabIndex = 3;
            this.btn_Update.Text = "Update";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.Btn_import_Click);
            // 
            // P17
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 82);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateRangeImport);
            this.Name = "P17";
            this.Text = "P17. Update SunRise Data";
            this.Controls.SetChildIndex(this.dateRangeImport, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btn_Update, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.DateRange dateRangeImport;
        private Win.UI.Label label1;
        private Win.UI.Button btn_Update;
    }
}