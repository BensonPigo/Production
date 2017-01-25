namespace Sci.Production.Subcon
{
    partial class R43
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
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboSubProcess = new Sci.Win.UI.ComboBox();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.dateBundleReceive = new Sci.Win.UI.DateRange();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(473, 12);
            this.print.TabIndex = 3;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(473, 48);
            this.toexcel.TabIndex = 4;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(473, 84);
            this.close.TabIndex = 5;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(169, 78);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 2;
            // 
            // comboSubProcess
            // 
            this.comboSubProcess.BackColor = System.Drawing.Color.White;
            this.comboSubProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubProcess.FormattingEnabled = true;
            this.comboSubProcess.IsSupportUnselect = true;
            this.comboSubProcess.Location = new System.Drawing.Point(169, 48);
            this.comboSubProcess.Name = "comboSubProcess";
            this.comboSubProcess.Size = new System.Drawing.Size(121, 24);
            this.comboSubProcess.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(32, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 23);
            this.label5.TabIndex = 115;
            this.label5.Text = "M";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(32, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 23);
            this.label4.TabIndex = 114;
            this.label4.Text = "Sub Process";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(32, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 23);
            this.label1.TabIndex = 118;
            this.label1.Text = "Bundle Receive Date";
            // 
            // dateBundleReceive
            // 
            this.dateBundleReceive.Location = new System.Drawing.Point(169, 19);
            this.dateBundleReceive.Name = "dateBundleReceive";
            this.dateBundleReceive.Size = new System.Drawing.Size(280, 23);
            this.dateBundleReceive.TabIndex = 0;
            // 
            // R43
            // 
            this.ClientSize = new System.Drawing.Size(565, 145);
            this.Controls.Add(this.dateBundleReceive);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboSubProcess);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.DefaultControl = "dateBundleReceive";
            this.DefaultControlForEdit = "dateBundleReceive";
            this.Name = "R43";
            this.Text = "R43.Sub-process BCS report (RFID)";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.comboSubProcess, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateBundleReceive, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboSubProcess;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateBundleReceive;
    }
}
