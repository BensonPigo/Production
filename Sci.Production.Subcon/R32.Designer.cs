namespace Sci.Production.Subcon
{
    partial class R32
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
            this.dateFarmOutDate = new Sci.Win.UI.DateRange();
            this.labelFarmOutDate = new Sci.Win.UI.Label();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.labelSubProcess = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(431, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(431, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(431, 84);
            // 
            // dateFarmOutDate
            // 
            this.dateFarmOutDate.IsRequired = false;
            this.dateFarmOutDate.Location = new System.Drawing.Point(119, 9);
            this.dateFarmOutDate.Name = "dateFarmOutDate";
            this.dateFarmOutDate.Size = new System.Drawing.Size(280, 23);
            this.dateFarmOutDate.TabIndex = 94;
            // 
            // labelFarmOutDate
            // 
            this.labelFarmOutDate.Location = new System.Drawing.Point(9, 9);
            this.labelFarmOutDate.Name = "labelFarmOutDate";
            this.labelFarmOutDate.Size = new System.Drawing.Size(103, 23);
            this.labelFarmOutDate.TabIndex = 95;
            this.labelFarmOutDate.Text = "Farm Out Date";
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(119, 38);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(121, 24);
            this.comboSubProcess.TabIndex = 96;
            // 
            // labelSubProcess
            // 
            this.labelSubProcess.Location = new System.Drawing.Point(9, 38);
            this.labelSubProcess.Name = "labelSubProcess";
            this.labelSubProcess.Size = new System.Drawing.Size(103, 23);
            this.labelSubProcess.TabIndex = 97;
            this.labelSubProcess.Text = "Sub Process";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(119, 68);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 98;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 68);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(103, 23);
            this.labelFactory.TabIndex = 99;
            this.labelFactory.Text = "Factory";
            // 
            // R32
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 144);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.labelSubProcess);
            this.Controls.Add(this.dateFarmOutDate);
            this.Controls.Add(this.labelFarmOutDate);
            this.Name = "R32";
            this.Text = "R32. Farm out Bundle Tracking List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFarmOutDate, 0);
            this.Controls.SetChildIndex(this.dateFarmOutDate, 0);
            this.Controls.SetChildIndex(this.labelSubProcess, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateFarmOutDate;
        private Win.UI.Label labelFarmOutDate;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.Label labelSubProcess;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelFactory;
    }
}