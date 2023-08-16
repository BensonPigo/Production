namespace Sci.Production.Shipping
{
    partial class R47
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
            this.lbWK = new Sci.Win.UI.Label();
            this.dateCDDATE = new Sci.Win.UI.DateRange();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCustomSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtCustomSPNoStart = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.lbConsignee = new Sci.Win.UI.Label();
            this.txtCustomsContract1 = new Sci.Production.Class.TxtCustomsContract();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(387, 90);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(406, 11);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(406, 52);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(374, 81);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(400, 90);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(393, 88);
            // 
            // lbWK
            // 
            this.lbWK.Location = new System.Drawing.Point(9, 16);
            this.lbWK.Name = "lbWK";
            this.lbWK.Size = new System.Drawing.Size(108, 23);
            this.lbWK.TabIndex = 99;
            this.lbWK.Text = "Date";
            // 
            // dateCDDATE
            // 
            // 
            // 
            // 
            this.dateCDDATE.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCDDATE.DateBox1.Name = "";
            this.dateCDDATE.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCDDATE.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCDDATE.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCDDATE.DateBox2.Name = "";
            this.dateCDDATE.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCDDATE.DateBox2.TabIndex = 1;
            this.dateCDDATE.Location = new System.Drawing.Point(120, 16);
            this.dateCDDATE.Name = "dateCDDATE";
            this.dateCDDATE.Size = new System.Drawing.Size(280, 23);
            this.dateCDDATE.TabIndex = 100;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 17);
            this.label5.TabIndex = 109;
            this.label5.Text = "～";
            // 
            // txtCustomSPNoEnd
            // 
            this.txtCustomSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtCustomSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomSPNoEnd.Location = new System.Drawing.Point(264, 52);
            this.txtCustomSPNoEnd.Name = "txtCustomSPNoEnd";
            this.txtCustomSPNoEnd.Size = new System.Drawing.Size(110, 23);
            this.txtCustomSPNoEnd.TabIndex = 107;
            // 
            // txtCustomSPNoStart
            // 
            this.txtCustomSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtCustomSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomSPNoStart.Location = new System.Drawing.Point(120, 52);
            this.txtCustomSPNoStart.Name = "txtCustomSPNoStart";
            this.txtCustomSPNoStart.Size = new System.Drawing.Size(110, 23);
            this.txtCustomSPNoStart.TabIndex = 106;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 23);
            this.label1.TabIndex = 108;
            this.label1.Text = "Custom SP#";
            // 
            // lbConsignee
            // 
            this.lbConsignee.Location = new System.Drawing.Point(9, 84);
            this.lbConsignee.Name = "lbConsignee";
            this.lbConsignee.Size = new System.Drawing.Size(108, 23);
            this.lbConsignee.TabIndex = 111;
            this.lbConsignee.Text = "Contract no.";
            // 
            // txtCustomsContract1
            // 
            this.txtCustomsContract1.BackColor = System.Drawing.Color.White;
            this.txtCustomsContract1.CheckDate = false;
            this.txtCustomsContract1.CheckStatus = true;
            this.txtCustomsContract1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCustomsContract1.Location = new System.Drawing.Point(120, 84);
            this.txtCustomsContract1.Name = "txtCustomsContract1";
            this.txtCustomsContract1.Size = new System.Drawing.Size(254, 23);
            this.txtCustomsContract1.TabIndex = 112;
            // 
            // R47
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 147);
            this.Controls.Add(this.txtCustomsContract1);
            this.Controls.Add(this.lbConsignee);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCustomSPNoEnd);
            this.Controls.Add(this.txtCustomSPNoStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateCDDATE);
            this.Controls.Add(this.lbWK);
            this.Name = "R47";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R47. Custom SP# and Consumption List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbWK, 0);
            this.Controls.SetChildIndex(this.dateCDDATE, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtCustomSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtCustomSPNoEnd, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.lbConsignee, 0);
            this.Controls.SetChildIndex(this.txtCustomsContract1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbWK;
        private Win.UI.DateRange dateCDDATE;
        private System.Windows.Forms.Label label5;
        private Win.UI.TextBox txtCustomSPNoEnd;
        private Win.UI.TextBox txtCustomSPNoStart;
        private Win.UI.Label label1;
        private Win.UI.Label lbConsignee;
        private Class.TxtCustomsContract txtCustomsContract1;
    }
}