namespace Sci.Production.Thread
{
    partial class R22
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
            this.label3 = new Sci.Win.UI.Label();
            this.dateRange = new Sci.Win.UI.DateRange();
            this.RefNo_Start = new Sci.Win.UI.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RefNo_End = new Sci.Win.UI.TextBox();
            this.textShade = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(441, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(441, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(441, 84);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(26, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "RefNo";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(26, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "Shade";
            // 
            // dateRange
            // 
            // 
            // 
            // 
            this.dateRange.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange.DateBox1.Name = "";
            this.dateRange.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRange.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRange.DateBox2.Name = "";
            this.dateRange.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRange.DateBox2.TabIndex = 1;
            this.dateRange.Location = new System.Drawing.Point(104, 19);
            this.dateRange.Name = "dateRange";
            this.dateRange.Size = new System.Drawing.Size(280, 23);
            this.dateRange.TabIndex = 97;
            // 
            // RefNo_Start
            // 
            this.RefNo_Start.BackColor = System.Drawing.Color.White;
            this.RefNo_Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RefNo_Start.Location = new System.Drawing.Point(104, 55);
            this.RefNo_Start.Name = "RefNo_Start";
            this.RefNo_Start.Size = new System.Drawing.Size(100, 23);
            this.RefNo_Start.TabIndex = 98;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(210, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 17);
            this.label4.TabIndex = 99;
            this.label4.Text = "～";
            // 
            // RefNo_End
            // 
            this.RefNo_End.BackColor = System.Drawing.Color.White;
            this.RefNo_End.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RefNo_End.Location = new System.Drawing.Point(238, 55);
            this.RefNo_End.Name = "RefNo_End";
            this.RefNo_End.Size = new System.Drawing.Size(100, 23);
            this.RefNo_End.TabIndex = 100;
            // 
            // textShade
            // 
            this.textShade.BackColor = System.Drawing.Color.White;
            this.textShade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textShade.Location = new System.Drawing.Point(104, 91);
            this.textShade.Name = "textShade";
            this.textShade.Size = new System.Drawing.Size(100, 23);
            this.textShade.TabIndex = 101;
            // 
            // R22
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 181);
            this.Controls.Add(this.textShade);
            this.Controls.Add(this.RefNo_End);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RefNo_Start);
            this.Controls.Add(this.dateRange);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R22";
            this.Text = "R22.Thread Stock Summary";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateRange, 0);
            this.Controls.SetChildIndex(this.RefNo_Start, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.RefNo_End, 0);
            this.Controls.SetChildIndex(this.textShade, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateRange;
        private Win.UI.TextBox RefNo_Start;
        private System.Windows.Forms.Label label4;
        private Win.UI.TextBox RefNo_End;
        private Win.UI.TextBox textShade;
    }
}