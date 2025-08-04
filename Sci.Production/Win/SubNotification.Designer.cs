namespace Sci.Production.Win
{
    partial class SubNotification
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel3 = new Sci.Win.UI.Panel();
            this.lblCount = new Sci.Win.UI.Label();
            this.btnOpen = new Sci.Win.UI.Button();
            this.lblName = new Sci.Win.UI.Label();
            this.UI_tipName = new System.Windows.Forms.ToolTip(this.components);
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblCount);
            this.panel3.Controls.Add(this.btnOpen);
            this.panel3.Controls.Add(this.lblName);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.RectStyle.BorderColor = System.Drawing.Color.Silver;
            this.panel3.RectStyle.BorderWidth = 2F;
            this.panel3.Size = new System.Drawing.Size(183, 52);
            this.panel3.TabIndex = 7;
            // 
            // lblCount
            // 
            this.lblCount.BackColor = System.Drawing.Color.LightPink;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblCount.Location = new System.Drawing.Point(4, 27);
            this.lblCount.Name = "lblCount";
            this.lblCount.RectStyle.BorderColor = System.Drawing.Color.Transparent;
            this.lblCount.RectStyle.Color = System.Drawing.Color.Transparent;
            this.lblCount.RectStyle.Shadow = true;
            this.lblCount.Size = new System.Drawing.Size(69, 21);
            this.lblCount.TabIndex = 16;
            this.lblCount.Text = "99";
            this.lblCount.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCount.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.lblCount.TextStyle.Color = System.Drawing.Color.Black;
            this.lblCount.DoubleClick += new System.EventHandler(this.LblCount_DoubleClick);
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOpen.Location = new System.Drawing.Point(106, 27);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(73, 23);
            this.btnOpen.TabIndex = 15;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // lblName
            // 
            this.lblName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(215)))), ((int)(((byte)(155)))));
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblName.Location = new System.Drawing.Point(4, 4);
            this.lblName.Name = "lblName";
            this.lblName.RectStyle.Color = System.Drawing.Color.Transparent;
            this.lblName.RectStyle.Shadow = true;
            this.lblName.Size = new System.Drawing.Size(174, 21);
            this.lblName.TabIndex = 14;
            this.lblName.Text = "HC approve remind";
            this.lblName.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // UI_tipName
            // 
            this.UI_tipName.AutomaticDelay = 100;
            this.UI_tipName.IsBalloon = true;
            // 
            // SubNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Name = "SubNotification";
            this.Size = new System.Drawing.Size(183, 52);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sci.Win.UI.Panel panel3;
        private Sci.Win.UI.Label lblCount;
        private Sci.Win.UI.Button btnOpen;
        private Sci.Win.UI.Label lblName;
        private System.Windows.Forms.ToolTip UI_tipName;
    }
}
