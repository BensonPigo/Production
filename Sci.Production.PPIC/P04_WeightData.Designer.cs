namespace Sci.Production.PPIC
{
    partial class P04_WeightData
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
            this.button1 = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.button1);
            this.btmcont.Location = new System.Drawing.Point(0, 323);
            this.btmcont.Size = new System.Drawing.Size(635, 44);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.button1, 0);
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(611, 305);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(544, 8);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(458, 8);
            // 
            // button1
            // 
            this.button1.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.button1.Location = new System.Drawing.Point(272, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 30);
            this.button1.TabIndex = 95;
            this.button1.Text = "Copy Season";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // P04_WeightData
            // 
            this.ClientSize = new System.Drawing.Size(635, 367);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.GridPopUp = false;
            this.GridUniqueKey = "SizeCode,Article";
            this.KeyField1 = "StyleUkey";
            this.Name = "P04_WeightData";
            this.Text = "Weight data";
            this.WorkAlias = "Style_WeightData";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button button1;
    }
}
