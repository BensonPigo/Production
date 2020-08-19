namespace Sci.Production.Shipping
{
    partial class P05_SOCFMDate
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
            this.labelSOCfmDate = new Sci.Win.UI.Label();
            this.dateSOCfmDate = new Sci.Win.UI.DateBox();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // labelSOCfmDate
            // 
            this.labelSOCfmDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSOCfmDate.Location = new System.Drawing.Point(9, 9);
            this.labelSOCfmDate.Name = "labelSOCfmDate";
            this.labelSOCfmDate.Size = new System.Drawing.Size(94, 23);
            this.labelSOCfmDate.TabIndex = 57;
            this.labelSOCfmDate.Text = "S/O CFM Date";
            // 
            // dateSOCfmDate
            // 
            this.dateSOCfmDate.Location = new System.Drawing.Point(106, 9);
            this.dateSOCfmDate.Name = "dateSOCfmDate";
            this.dateSOCfmDate.Size = new System.Drawing.Size(130, 23);
            this.dateSOCfmDate.TabIndex = 58;
            this.dateSOCfmDate.Validating += new System.ComponentModel.CancelEventHandler(this.DateSOCfmDate_Validating);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnConfirm.Location = new System.Drawing.Point(9, 38);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(94, 32);
            this.btnConfirm.TabIndex = 59;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(137, 38);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 32);
            this.btnCancel.TabIndex = 60;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // P05_SOCFMDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 88);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.dateSOCfmDate);
            this.Controls.Add(this.labelSOCfmDate);
            this.Name = "P05_SOCFMDate";
            this.Text = "P05_SOCFMDate";
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labelSOCfmDate;
        private Win.UI.DateBox dateSOCfmDate;
        private Win.UI.Button btnConfirm;
        private Win.UI.Button btnCancel;
    }
}