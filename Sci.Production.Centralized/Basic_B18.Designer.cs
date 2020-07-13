namespace Sci.Production.Centralized
{
    partial class Basic_B18
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
            this.disAccountNoname = new Sci.Win.UI.DisplayBox();
            this.chkUnselected = new Sci.Win.UI.CheckBox();
            this.txtAccountNo = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(768, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtAccountNo);
            this.detailcont.Controls.Add(this.chkUnselected);
            this.detailcont.Controls.Add(this.disAccountNoname);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(768, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(768, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(776, 417);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "A/C No.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "A/C Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(32, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Unselectable in PMS";
            // 
            // disAccountNoname
            // 
            this.disAccountNoname.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disAccountNoname.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disAccountNoname.Location = new System.Drawing.Point(183, 62);
            this.disAccountNoname.Name = "disAccountNoname";
            this.disAccountNoname.Size = new System.Drawing.Size(214, 23);
            this.disAccountNoname.TabIndex = 4;
            // 
            // chkUnselected
            // 
            this.chkUnselected.AutoSize = true;
            this.chkUnselected.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UnselectableShipB03", true));
            this.chkUnselected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkUnselected.Location = new System.Drawing.Point(183, 100);
            this.chkUnselected.Name = "chkUnselected";
            this.chkUnselected.Size = new System.Drawing.Size(111, 21);
            this.chkUnselected.TabIndex = 2;
            this.chkUnselected.Text = "Shipping B03";
            this.chkUnselected.UseVisualStyleBackColor = true;
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.BackColor = System.Drawing.Color.White;
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtAccountNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAccountNo.Location = new System.Drawing.Point(183, 24);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(100, 23);
            this.txtAccountNo.TabIndex = 1;
            this.txtAccountNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtAccountNo_PopUp);
            this.txtAccountNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtAccountNo_Validating);
            // 
            // Basic_B18
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 450);
            this.ConnectionName = "ProductionTPE";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "Basic_B18";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Basic_B18. A/C Setting";
            this.WorkAlias = "AccountNoSetting";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtAccountNo;
        private Win.UI.CheckBox chkUnselected;
        private Win.UI.DisplayBox disAccountNoname;
    }
}