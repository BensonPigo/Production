namespace Sci.Production.PPIC
{
    partial class P24_Sketch
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
            this.components = new System.ComponentModel.Container();
            this.comboPictureSize2 = new Sci.Production.Class.ComboPictureSize(this.components);
            this.comboPictureSize1 = new Sci.Production.Class.ComboPictureSize(this.components);
            this.pictureBox2 = new Sci.Win.UI.PictureBox();
            this.labelPicture2 = new Sci.Win.UI.Label();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.labelPicture1 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboPictureSize2
            // 
            this.comboPictureSize2.BackColor = System.Drawing.Color.White;
            this.comboPictureSize2.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboPictureSize2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPictureSize2.FormattingEnabled = true;
            this.comboPictureSize2.IsSupportUnselect = true;
            this.comboPictureSize2.Location = new System.Drawing.Point(484, 10);
            this.comboPictureSize2.Name = "comboPictureSize2";
            this.comboPictureSize2.OldText = "";
            this.comboPictureSize2.Size = new System.Drawing.Size(172, 24);
            this.comboPictureSize2.TabIndex = 19;
            this.comboPictureSize2.TargetPictureBox = null;
            // 
            // comboPictureSize1
            // 
            this.comboPictureSize1.BackColor = System.Drawing.Color.White;
            this.comboPictureSize1.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboPictureSize1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboPictureSize1.FormattingEnabled = true;
            this.comboPictureSize1.IsSupportUnselect = true;
            this.comboPictureSize1.Location = new System.Drawing.Point(74, 11);
            this.comboPictureSize1.Name = "comboPictureSize1";
            this.comboPictureSize1.OldText = "";
            this.comboPictureSize1.Size = new System.Drawing.Size(140, 24);
            this.comboPictureSize1.TabIndex = 18;
            this.comboPictureSize1.TargetPictureBox = null;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = null;
            this.pictureBox2.Location = new System.Drawing.Point(421, 37);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(386, 428);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 17;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.WaitOnLoad = true;
            // 
            // labelPicture2
            // 
            this.labelPicture2.Location = new System.Drawing.Point(421, 11);
            this.labelPicture2.Name = "labelPicture2";
            this.labelPicture2.Size = new System.Drawing.Size(60, 23);
            this.labelPicture2.TabIndex = 15;
            this.labelPicture2.Text = "Picture2";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(11, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(386, 428);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // labelPicture1
            // 
            this.labelPicture1.Location = new System.Drawing.Point(11, 11);
            this.labelPicture1.Name = "labelPicture1";
            this.labelPicture1.Size = new System.Drawing.Size(60, 23);
            this.labelPicture1.TabIndex = 12;
            this.labelPicture1.Text = "Picture1";
            // 
            // P24_Sketch
            // 
            this.ClientSize = new System.Drawing.Size(816, 499);
            this.Controls.Add(this.comboPictureSize2);
            this.Controls.Add(this.comboPictureSize1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelPicture2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelPicture1);
            this.Name = "P24_Sketch";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Class.ComboPictureSize comboPictureSize2;
        private Class.ComboPictureSize comboPictureSize1;
        private Win.UI.PictureBox pictureBox2;
        private Win.UI.Label labelPicture2;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label labelPicture1;
    }
}
