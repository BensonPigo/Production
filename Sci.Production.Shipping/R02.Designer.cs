namespace Sci.Production.Shipping
{
    partial class R02
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelSDPDate = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.datePulloutDate = new Sci.Win.UI.DateRange();
            this.dateSDPDate = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(413, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(413, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(413, 84);
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Lines = 0;
            this.labelPulloutDate.Location = new System.Drawing.Point(13, 12);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(80, 23);
            this.labelPulloutDate.TabIndex = 94;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(13, 48);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(80, 23);
            this.labelSPNo.TabIndex = 95;
            this.labelSPNo.Text = "SP No";
            // 
            // labelSDPDate
            // 
            this.labelSDPDate.Lines = 0;
            this.labelSDPDate.Location = new System.Drawing.Point(13, 84);
            this.labelSDPDate.Name = "labelSDPDate";
            this.labelSDPDate.Size = new System.Drawing.Size(80, 23);
            this.labelSDPDate.TabIndex = 96;
            this.labelSDPDate.Text = "SDP Date";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 120);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(80, 23);
            this.labelSCIDelivery.TabIndex = 97;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 156);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(80, 23);
            this.labelM.TabIndex = 98;
            this.labelM.Text = "M";
            // 
            // datePulloutDate
            // 
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(97, 12);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 99;
            // 
            // dateSDPDate
            // 
            this.dateSDPDate.IsRequired = false;
            this.dateSDPDate.Location = new System.Drawing.Point(96, 84);
            this.dateSDPDate.Name = "dateSDPDate";
            this.dateSDPDate.Size = new System.Drawing.Size(280, 23);
            this.dateSDPDate.TabIndex = 100;
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(97, 120);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 101;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(97, 156);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(81, 24);
            this.comboM.TabIndex = 102;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(97, 48);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(130, 23);
            this.txtSPNoStart.TabIndex = 103;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(252, 48);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(130, 23);
            this.txtSPNoEnd.TabIndex = 104;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(228, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 23);
            this.label6.TabIndex = 105;
            this.label6.Text = "～";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // R02
            // 
            this.ClientSize = new System.Drawing.Size(505, 217);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.dateSDPDate);
            this.Controls.Add(this.datePulloutDate);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelSDPDate);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelPulloutDate);
            this.IsSupportToPrint = false;
            this.Name = "R02";
            this.Text = "R02. Pullout Report List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelPulloutDate, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelSDPDate, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.datePulloutDate, 0);
            this.Controls.SetChildIndex(this.dateSDPDate, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelPulloutDate;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelSDPDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelM;
        private Win.UI.DateRange datePulloutDate;
        private Win.UI.DateRange dateSDPDate;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.ComboBox comboM;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Label label6;
    }
}
