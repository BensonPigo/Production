namespace Sci.Production.Basic
{
    partial class B03
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.textM = new Sci.Win.UI.TextBox();
            this.txtuser1 = new Sci.Production.Class.txtuser();
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
            this.detail.Size = new System.Drawing.Size(676, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtuser1);
            this.detailcont.Controls.Add(this.textM);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(676, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(676, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(676, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(684, 424);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(53, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "M";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(53, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Manager";
            // 
            // textM
            // 
            this.textM.BackColor = System.Drawing.Color.White;
            this.textM.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.textM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textM.Location = new System.Drawing.Point(118, 31);
            this.textM.Name = "textM";
            this.textM.Size = new System.Drawing.Size(80, 23);
            this.textM.TabIndex = 2;
            // 
            // txtuser1
            // 
            this.txtuser1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Manager", true));
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Location = new System.Drawing.Point(118, 69);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(300, 23);
            this.txtuser1.TabIndex = 3;
            this.txtuser1.TextBox1Binding = "";
            // 
            // B03
            // 
            this.ClientSize = new System.Drawing.Size(684, 457);
            this.DefaultControl = "textM";
            this.DefaultControlForEdit = "txtuser1";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B03";
            this.Text = "B03. M";
            this.UniqueExpress = "ID";
            this.WorkAlias = "MDivision";
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

        private Class.txtuser txtuser1;
        private Win.UI.TextBox textM;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}
