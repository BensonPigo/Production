namespace Sci.Production.Quality
{
    partial class B05
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
            this.txtCode = new Sci.Win.UI.TextBox();
            this.labelCode = new Sci.Win.UI.Label();
            this.labelWeight = new Sci.Win.UI.Label();
            this.numWeight = new Sci.Win.UI.NumericBox();
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
            this.detail.Size = new System.Drawing.Size(831, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.numWeight);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Controls.Add(this.labelWeight);
            this.detailcont.Size = new System.Drawing.Size(831, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(831, 38);
            //  
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(831, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(839, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCode.IsSupportEditMode = false;
            this.txtCode.Location = new System.Drawing.Point(140, 53);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(129, 23);
            this.txtCode.TabIndex = 10;
            // 
            // labelCode
            // 
            this.labelCode.Lines = 0;
            this.labelCode.Location = new System.Drawing.Point(82, 53);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(55, 23);
            this.labelCode.TabIndex = 8;
            this.labelCode.Text = "Code ";
            // 
            // labelWeight
            // 
            this.labelWeight.Lines = 0;
            this.labelWeight.Location = new System.Drawing.Point(82, 86);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(55, 23);
            this.labelWeight.TabIndex = 9;
            this.labelWeight.Text = "Weight";
            // 
            // numWeight
            // 
            this.numWeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numWeight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "weight", true));
            this.numWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numWeight.Location = new System.Drawing.Point(140, 86);
            this.numWeight.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numWeight.Name = "numWeight";
            this.numWeight.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWeight.ReadOnly = true;
            this.numWeight.Size = new System.Drawing.Size(129, 23);
            this.numWeight.TabIndex = 12;
            this.numWeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWeight.TextChanged += new System.EventHandler(this.NumWeight_TextChanged);
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(839, 457);
            this.DefaultControlForEdit = "numWeight";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.Text = "B05 .Inspection Weight";
            this.WorkAlias = "InspWeight";
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

        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelCode;
        private Win.UI.Label labelWeight;
        private Win.UI.NumericBox numWeight;
    }
}
