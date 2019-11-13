namespace Sci.Production.Shipping
{
    partial class R43
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
            this.lbOnBoardDate = new Sci.Win.UI.Label();
            this.dateOnBoardDate = new Sci.Win.UI.DateRange();
            this.lbFCRDate = new Sci.Win.UI.Label();
            this.dateFCRDate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(472, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(472, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(472, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(214, 12);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(346, 12);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(452, 12);
            // 
            // lbOnBoardDate
            // 
            this.lbOnBoardDate.Location = new System.Drawing.Point(9, 48);
            this.lbOnBoardDate.Name = "lbOnBoardDate";
            this.lbOnBoardDate.Size = new System.Drawing.Size(114, 23);
            this.lbOnBoardDate.TabIndex = 98;
            this.lbOnBoardDate.Text = "On Board Date";
            // 
            // dateOnBoardDate
            // 
            // 
            // 
            // 
            this.dateOnBoardDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOnBoardDate.DateBox1.Name = "";
            this.dateOnBoardDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoardDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOnBoardDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOnBoardDate.DateBox2.Name = "";
            this.dateOnBoardDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoardDate.DateBox2.TabIndex = 1;
            this.dateOnBoardDate.Location = new System.Drawing.Point(126, 48);
            this.dateOnBoardDate.Name = "dateOnBoardDate";
            this.dateOnBoardDate.Size = new System.Drawing.Size(280, 23);
            this.dateOnBoardDate.TabIndex = 100;
            // 
            // lbFCRDate
            // 
            this.lbFCRDate.Location = new System.Drawing.Point(9, 84);
            this.lbFCRDate.Name = "lbFCRDate";
            this.lbFCRDate.Size = new System.Drawing.Size(114, 23);
            this.lbFCRDate.TabIndex = 101;
            this.lbFCRDate.Text = "FCR Date";
            // 
            // dateFCRDate
            // 
            // 
            // 
            // 
            this.dateFCRDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFCRDate.DateBox1.Name = "";
            this.dateFCRDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFCRDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFCRDate.DateBox2.Name = "";
            this.dateFCRDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox2.TabIndex = 1;
            this.dateFCRDate.Location = new System.Drawing.Point(126, 84);
            this.dateFCRDate.Name = "dateFCRDate";
            this.dateFCRDate.Size = new System.Drawing.Size(280, 23);
            this.dateFCRDate.TabIndex = 102;
            // 
            // R43
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 223);
            this.Controls.Add(this.dateFCRDate);
            this.Controls.Add(this.lbFCRDate);
            this.Controls.Add(this.dateOnBoardDate);
            this.Controls.Add(this.lbOnBoardDate);
            this.Name = "R43";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R43. Non Declaration Report-Export";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbOnBoardDate, 0);
            this.Controls.SetChildIndex(this.dateOnBoardDate, 0);
            this.Controls.SetChildIndex(this.lbFCRDate, 0);
            this.Controls.SetChildIndex(this.dateFCRDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbOnBoardDate;
        private Win.UI.DateRange dateOnBoardDate;
        private Win.UI.Label lbFCRDate;
        private Win.UI.DateRange dateFCRDate;
    }
}