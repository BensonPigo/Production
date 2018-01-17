namespace Sci.Production.SubProcess
{
    partial class R01
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
            this.txtType = new Sci.Win.UI.TextBox();
            this.dateRange = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(397, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(397, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(397, 84);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Type";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 94;
            this.label2.Text = "Date";
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtType.IsSupportEditMode = false;
            this.txtType.Location = new System.Drawing.Point(92, 12);
            this.txtType.Name = "txtType";
            this.txtType.ReadOnly = true;
            this.txtType.Size = new System.Drawing.Size(100, 23);
            this.txtType.TabIndex = 95;
            this.txtType.Text = "PPA";
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
            this.dateRange.Location = new System.Drawing.Point(92, 42);
            this.dateRange.Name = "dateRange";
            this.dateRange.Size = new System.Drawing.Size(280, 23);
            this.dateRange.TabIndex = 96;
            // 
            // R01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 165);
            this.Controls.Add(this.dateRange);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R01";
            this.Text = "R01. Monthly SubProcess Output Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtType, 0);
            this.Controls.SetChildIndex(this.dateRange, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtType;
        private Win.UI.DateRange dateRange;
    }
}