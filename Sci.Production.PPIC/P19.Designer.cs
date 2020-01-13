namespace Sci.Production.PPIC
{
    partial class P19
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.btnPMS = new Sci.Win.UI.Button();
            this.btnMWS = new Sci.Win.UI.Button();
            this.dateInputDate = new Sci.Win.UI.DateBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(26, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Export Finishing Process data from PMS ";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Import Finishing Process data from MWS";
            // 
            // btnPMS
            // 
            this.btnPMS.Location = new System.Drawing.Point(431, 19);
            this.btnPMS.Name = "btnPMS";
            this.btnPMS.Size = new System.Drawing.Size(80, 30);
            this.btnPMS.TabIndex = 1;
            this.btnPMS.Text = "Update";
            this.btnPMS.UseVisualStyleBackColor = true;
            this.btnPMS.Click += new System.EventHandler(this.BtnPMS_Click);
            // 
            // btnMWS
            // 
            this.btnMWS.Location = new System.Drawing.Point(431, 67);
            this.btnMWS.Name = "btnMWS";
            this.btnMWS.Size = new System.Drawing.Size(80, 30);
            this.btnMWS.TabIndex = 2;
            this.btnMWS.Text = "Update";
            this.btnMWS.UseVisualStyleBackColor = true;
            this.btnMWS.Click += new System.EventHandler(this.BtnMWS_Click);
            // 
            // dateInputDate
            // 
            this.dateInputDate.IsSupportEditMode = false;
            this.dateInputDate.Location = new System.Drawing.Point(295, 26);
            this.dateInputDate.Name = "dateInputDate";
            this.dateInputDate.Size = new System.Drawing.Size(130, 23);
            this.dateInputDate.TabIndex = 3;
            // 
            // P19
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 125);
            this.Controls.Add(this.dateInputDate);
            this.Controls.Add(this.btnMWS);
            this.Controls.Add(this.btnPMS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P19";
            this.OnLineHelpID = "Sci.Win.Tems.Base";
            this.Text = "P19. Update Finishing Process data (For SNP)";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnPMS, 0);
            this.Controls.SetChildIndex(this.btnMWS, 0);
            this.Controls.SetChildIndex(this.dateInputDate, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Button btnPMS;
        private Win.UI.Button btnMWS;
        private Win.UI.DateBox dateInputDate;
    }
}