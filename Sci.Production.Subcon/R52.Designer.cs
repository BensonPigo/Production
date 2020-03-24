namespace Sci.Production.Subcon
{
    partial class R52
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtseason = new Sci.Production.Class.txtseason();
            this.labelSeason = new Sci.Win.UI.Label();
            this.btnSketchDownload = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(320, 21);
            this.print.Size = new System.Drawing.Size(100, 30);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(320, 21);
            this.toexcel.Size = new System.Drawing.Size(100, 30);
            this.toexcel.TabIndex = 1;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(320, 114);
            this.close.Size = new System.Drawing.Size(100, 30);
            this.close.TabIndex = 3;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(37, 93);
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(89, 25);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(89, 23);
            this.txtseason.TabIndex = 0;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(19, 25);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(67, 23);
            this.labelSeason.TabIndex = 99;
            this.labelSeason.Text = "Season";
            // 
            // btnSketchDownload
            // 
            this.btnSketchDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSketchDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSketchDownload.Location = new System.Drawing.Point(320, 57);
            this.btnSketchDownload.Name = "btnSketchDownload";
            this.btnSketchDownload.Size = new System.Drawing.Size(100, 51);
            this.btnSketchDownload.TabIndex = 2;
            this.btnSketchDownload.Text = "Sketch download";
            this.btnSketchDownload.UseVisualStyleBackColor = true;
            this.btnSketchDownload.Click += new System.EventHandler(this.BtnSketchDownload_Click);
            // 
            // R52
            // 
            this.ClientSize = new System.Drawing.Size(437, 208);
            this.Controls.Add(this.btnSketchDownload);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.labelSeason);
            this.Name = "R52";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R52.Transfer Subcon Style Data To Printing System";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.btnSketchDownload, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      #endregion

      private Class.txtseason txtseason;
      private Win.UI.Label labelSeason;
        private Win.UI.Button btnSketchDownload;
    }
}
