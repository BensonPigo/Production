namespace Sci.Production.Basic
{
    partial class B04_AccountNo
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
            this.labelCode = new Sci.Win.UI.Label();
            this.labelAbbreviation = new Sci.Win.UI.Label();
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.displayAbbreviation = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 453);
            this.btmcont.Size = new System.Drawing.Size(433, 44);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 42);
            this.gridcont.Size = new System.Drawing.Size(409, 405);
            // 
            // append
            // 
            this.append.Size = new System.Drawing.Size(80, 34);
            this.append.TabIndex = 0;
            this.append.Visible = false;
            // 
            // revise
            // 
            this.revise.Size = new System.Drawing.Size(80, 34);
            this.revise.TabIndex = 1;
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.Size = new System.Drawing.Size(80, 34);
            this.delete.TabIndex = 2;
            this.delete.Visible = false;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(343, 5);
            this.undo.Size = new System.Drawing.Size(80, 34);
            this.undo.TabIndex = 4;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(263, 5);
            this.save.Size = new System.Drawing.Size(80, 34);
            this.save.TabIndex = 3;
            // 
            // labelCode
            // 
            this.labelCode.Lines = 0;
            this.labelCode.Location = new System.Drawing.Point(13, 13);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(47, 23);
            this.labelCode.TabIndex = 98;
            this.labelCode.Text = "Code";
            // 
            // labelAbbreviation
            // 
            this.labelAbbreviation.Lines = 0;
            this.labelAbbreviation.Location = new System.Drawing.Point(204, 13);
            this.labelAbbreviation.Name = "labelAbbreviation";
            this.labelAbbreviation.Size = new System.Drawing.Size(83, 23);
            this.labelAbbreviation.TabIndex = 99;
            this.labelAbbreviation.Text = "Abbreviation";
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(64, 13);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(66, 23);
            this.displayCode.TabIndex = 0;
            // 
            // displayAbbreviation
            // 
            this.displayAbbreviation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAbbreviation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAbbreviation.Location = new System.Drawing.Point(291, 13);
            this.displayAbbreviation.Name = "displayAbbreviation";
            this.displayAbbreviation.Size = new System.Drawing.Size(120, 23);
            this.displayAbbreviation.TabIndex = 1;
            // 
            // B04_AccountNo
            // 
            this.ClientSize = new System.Drawing.Size(433, 497);
            this.Controls.Add(this.displayAbbreviation);
            this.Controls.Add(this.displayCode);
            this.Controls.Add(this.labelAbbreviation);
            this.Controls.Add(this.labelCode);
            this.GridPopUp = false;
            this.KeyField1 = "ID";
            this.Name = "B04_AccountNo";
            this.Text = "Accounting chart no";
            this.WorkAlias = "LocalSupp_AccountNo";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelCode, 0);
            this.Controls.SetChildIndex(this.labelAbbreviation, 0);
            this.Controls.SetChildIndex(this.displayCode, 0);
            this.Controls.SetChildIndex(this.displayAbbreviation, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelCode;
        private Win.UI.Label labelAbbreviation;
        private Win.UI.DisplayBox displayCode;
        private Win.UI.DisplayBox displayAbbreviation;
    }
}
