namespace Sci.Production.Subcon
{
    partial class R51
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
            this.txtLocalSupp1 = new Sci.Production.Class.TxtLocalSupp();
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(388, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(388, 12);
            this.toexcel.TabIndex = 4;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(388, 48);
            this.close.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 66);
            this.label1.TabIndex = 94;
            this.label1.Text = "Generate PO Data";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.Color = System.Drawing.Color.Blue;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Blue;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Blue;
            // 
            // txtLocalSupp1
            // 
            this.txtLocalSupp1.DisplayBox1Binding = "";
            this.txtLocalSupp1.IsFactory = false;
            this.txtLocalSupp1.IsMisc = false;
            this.txtLocalSupp1.IsMiscOverseas = false;
            this.txtLocalSupp1.IsSintexSubcon = true;
            this.txtLocalSupp1.Location = new System.Drawing.Point(87, 82);
            this.txtLocalSupp1.Name = "txtLocalSupp1";
            this.txtLocalSupp1.Size = new System.Drawing.Size(252, 23);
            this.txtLocalSupp1.TabIndex = 97;
            this.txtLocalSupp1.TextBox1Binding = "";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Supplier";
            // 
            // R51
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 185);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLocalSupp1);
            this.Controls.Add(this.label1);
            this.Name = "R51";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R51. Transfer Subcon PO Data To Printing System";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtLocalSupp1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private Win.UI.Label label1;
        private Class.TxtLocalSupp txtLocalSupp1;
        private Win.UI.Label label2;
    }
}