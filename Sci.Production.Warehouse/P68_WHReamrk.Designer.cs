namespace Sci.Production.Warehouse
{
    partial class P68_WHReamrk
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
            this.components = new System.ComponentModel.Container();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.grid1BS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid2BS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1BS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2BS)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 438);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(546, 38);
            this.panel2.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(463, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(0, 0);
            this.displayBox1.Multiline = true;
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(546, 438);
            this.displayBox1.TabIndex = 1;
            // 
            // P68_WHReamrk
            // 
            this.ClientSize = new System.Drawing.Size(546, 476);
            this.Controls.Add(this.displayBox1);
            this.Controls.Add(this.panel2);
            this.Name = "P68_WHReamrk";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P68 W/H Reamrk";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.displayBox1, 0);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1BS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2BS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.ListControlBindingSource grid1BS;
        private Win.UI.ListControlBindingSource grid2BS;
        private Win.UI.DisplayBox displayBox1;
    }
}
