namespace Sci.Production.Shipping
{
    partial class R44
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
            this.dateOnBoardDate = new Sci.Win.UI.DateRange();
            this.dateArrivalPortDate = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(438, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(438, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(438, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(12, 114);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(144, 120);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(250, 121);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "On Board Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Arrival Port Date";
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
            this.dateOnBoardDate.Location = new System.Drawing.Point(126, 12);
            this.dateOnBoardDate.Name = "dateOnBoardDate";
            this.dateOnBoardDate.Size = new System.Drawing.Size(280, 23);
            this.dateOnBoardDate.TabIndex = 99;
            // 
            // dateArrivalPortDate
            // 
            // 
            // 
            // 
            this.dateArrivalPortDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArrivalPortDate.DateBox1.Name = "";
            this.dateArrivalPortDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArrivalPortDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArrivalPortDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArrivalPortDate.DateBox2.Name = "";
            this.dateArrivalPortDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArrivalPortDate.DateBox2.TabIndex = 1;
            this.dateArrivalPortDate.Location = new System.Drawing.Point(126, 41);
            this.dateArrivalPortDate.Name = "dateArrivalPortDate";
            this.dateArrivalPortDate.Size = new System.Drawing.Size(280, 23);
            this.dateArrivalPortDate.TabIndex = 100;
            // 
            // R44
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 176);
            this.Controls.Add(this.dateArrivalPortDate);
            this.Controls.Add(this.dateOnBoardDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R44";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R44. Non Declaration Report - Import";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateOnBoardDate, 0);
            this.Controls.SetChildIndex(this.dateArrivalPortDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DateRange dateOnBoardDate;
        private Win.UI.DateRange dateArrivalPortDate;
    }
}