namespace Sci.Production.IE
{
    partial class P01_Sketch
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
            this.labelPicture1 = new Sci.Win.UI.Label();
            this.displayPicture1 = new Sci.Win.UI.DisplayBox();
            this.picture1 = new Sci.Win.UI.PictureBox();
            this.picture2 = new Sci.Win.UI.PictureBox();
            this.displayPicture2 = new Sci.Win.UI.DisplayBox();
            this.labelPicture2 = new Sci.Win.UI.Label();
            this.btnClose = new Sci.Win.UI.Button();
            this.comboPictureSize2 = new Sci.Production.Class.ComboPictureSize(this.components);
            this.comboPictureSize1 = new Sci.Production.Class.ComboPictureSize(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture2)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPicture1
            // 
            this.labelPicture1.Location = new System.Drawing.Point(9, 9);
            this.labelPicture1.Name = "labelPicture1";
            this.labelPicture1.Size = new System.Drawing.Size(56, 23);
            this.labelPicture1.TabIndex = 0;
            this.labelPicture1.Text = "Picture1";
            // 
            // displayPicture1
            // 
            this.displayPicture1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPicture1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPicture1.Location = new System.Drawing.Point(69, 9);
            this.displayPicture1.Name = "displayPicture1";
            this.displayPicture1.Size = new System.Drawing.Size(202, 23);
            this.displayPicture1.TabIndex = 0;
            // 
            // picture1
            // 
            this.picture1.Image = null;
            this.picture1.Location = new System.Drawing.Point(9, 50);
            this.picture1.Name = "picture1";
            this.picture1.Size = new System.Drawing.Size(364, 329);
            this.picture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture1.TabIndex = 2;
            this.picture1.TabStop = false;
            this.picture1.WaitOnLoad = true;
            // 
            // picture2
            // 
            this.picture2.Image = null;
            this.picture2.Location = new System.Drawing.Point(402, 50);
            this.picture2.Name = "picture2";
            this.picture2.Size = new System.Drawing.Size(364, 329);
            this.picture2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picture2.TabIndex = 5;
            this.picture2.TabStop = false;
            this.picture2.WaitOnLoad = true;
            // 
            // displayPicture2
            // 
            this.displayPicture2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPicture2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPicture2.Location = new System.Drawing.Point(462, 9);
            this.displayPicture2.Name = "displayPicture2";
            this.displayPicture2.Size = new System.Drawing.Size(202, 23);
            this.displayPicture2.TabIndex = 1;
            // 
            // labelPicture2
            // 
            this.labelPicture2.Location = new System.Drawing.Point(402, 9);
            this.labelPicture2.Name = "labelPicture2";
            this.labelPicture2.Size = new System.Drawing.Size(56, 23);
            this.labelPicture2.TabIndex = 3;
            this.labelPicture2.Text = "Picture2";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(671, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // comboPictureSize2
            // 
            this.comboPictureSize2.BackColor = System.Drawing.Color.White;
            this.comboPictureSize2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPictureSize2.FormattingEnabled = true;
            this.comboPictureSize2.IsSupportUnselect = true;
            this.comboPictureSize2.Location = new System.Drawing.Point(666, 9);
            this.comboPictureSize2.Name = "comboPictureSize2";
            this.comboPictureSize2.OldText = "";
            this.comboPictureSize2.Size = new System.Drawing.Size(100, 24);
            this.comboPictureSize2.TabIndex = 7;
            this.comboPictureSize2.TargetPictureBox = null;
            // 
            // comboPictureSize1
            // 
            this.comboPictureSize1.BackColor = System.Drawing.Color.White;
            this.comboPictureSize1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPictureSize1.FormattingEnabled = true;
            this.comboPictureSize1.IsSupportUnselect = true;
            this.comboPictureSize1.Location = new System.Drawing.Point(272, 8);
            this.comboPictureSize1.Name = "comboPictureSize1";
            this.comboPictureSize1.OldText = "";
            this.comboPictureSize1.Size = new System.Drawing.Size(100, 24);
            this.comboPictureSize1.TabIndex = 6;
            this.comboPictureSize1.TargetPictureBox = null;
            // 
            // P01_Sketch
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(776, 430);
            this.Controls.Add(this.comboPictureSize2);
            this.Controls.Add(this.comboPictureSize1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.picture2);
            this.Controls.Add(this.displayPicture2);
            this.Controls.Add(this.labelPicture2);
            this.Controls.Add(this.picture1);
            this.Controls.Add(this.displayPicture1);
            this.Controls.Add(this.labelPicture1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "P01_Sketch";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Sketch";
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelPicture1;
        private Win.UI.DisplayBox displayPicture1;
        private Win.UI.PictureBox picture1;
        private Win.UI.PictureBox picture2;
        private Win.UI.DisplayBox displayPicture2;
        private Win.UI.Label labelPicture2;
        private Win.UI.Button btnClose;
        private Class.ComboPictureSize comboPictureSize1;
        private Class.ComboPictureSize comboPictureSize2;
    }
}
