namespace Sci.Production.Centralized
{
    partial class IE_B05_CopyFromOtherFactory
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtToFty = new Sci.Win.UI.TextBox();
            this.txtFromFty = new Sci.Win.UI.TextBox();
            this.labToFactory = new Sci.Win.UI.Label();
            this.labFromFty = new Sci.Win.UI.Label();
            this.btnCopy = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // txtToFty
            // 
            this.txtToFty.BackColor = System.Drawing.Color.White;
            this.txtToFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtToFty.Location = new System.Drawing.Point(145, 57);
            this.txtToFty.Name = "txtToFty";
            this.txtToFty.Size = new System.Drawing.Size(120, 23);
            this.txtToFty.TabIndex = 1;
            this.txtToFty.Validating += new System.ComponentModel.CancelEventHandler(this.TxtToFty_Validating);
            // 
            // txtFromFty
            // 
            this.txtFromFty.BackColor = System.Drawing.Color.White;
            this.txtFromFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFromFty.Location = new System.Drawing.Point(145, 25);
            this.txtFromFty.Name = "txtFromFty";
            this.txtFromFty.Size = new System.Drawing.Size(120, 23);
            this.txtFromFty.TabIndex = 0;
            this.txtFromFty.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFromFty_Validating);
            // 
            // labToFactory
            // 
            this.labToFactory.Location = new System.Drawing.Point(40, 57);
            this.labToFactory.Name = "labToFactory";
            this.labToFactory.Size = new System.Drawing.Size(102, 23);
            this.labToFactory.TabIndex = 5;
            this.labToFactory.Text = "To Factory";
            // 
            // labFromFty
            // 
            this.labFromFty.Location = new System.Drawing.Point(40, 25);
            this.labFromFty.Name = "labFromFty";
            this.labFromFty.Size = new System.Drawing.Size(102, 23);
            this.labFromFty.TabIndex = 4;
            this.labFromFty.Text = "From Factory";
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(308, 21);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(77, 30);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(308, 57);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // IE_B05_CopyFromOtherFactory
            // 
            this.ClientSize = new System.Drawing.Size(459, 132);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.txtToFty);
            this.Controls.Add(this.txtFromFty);
            this.Controls.Add(this.labToFactory);
            this.Controls.Add(this.labFromFty);
            this.DefaultControl = "txtFromFty";
            this.DefaultControlForEdit = "txtFromFty";
            this.Name = "IE_B05_CopyFromOtherFactory";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Copy From Other Factory";
            this.Controls.SetChildIndex(this.labFromFty, 0);
            this.Controls.SetChildIndex(this.labToFactory, 0);
            this.Controls.SetChildIndex(this.txtFromFty, 0);
            this.Controls.SetChildIndex(this.txtToFty, 0);
            this.Controls.SetChildIndex(this.btnCopy, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtToFty;
        private Win.UI.TextBox txtFromFty;
        private Win.UI.Label labToFactory;
        private Win.UI.Label labFromFty;
        private Win.UI.Button btnCopy;
        private Win.UI.Button btnClose;
    }
}
