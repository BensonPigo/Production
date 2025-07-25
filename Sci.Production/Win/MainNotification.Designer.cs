namespace Sci.Production.Win
{
    partial class MainNotification
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
            this.panel_Main = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnRefresh = new Sci.Win.UI.Button();
            this.lblRefreshTime = new Sci.Win.UI.Label();
            this.panel_Set = new Sci.Win.UI.Panel();
            this.label5 = new Sci.Win.UI.Label();
            this.btnSet = new Sci.Win.UI.Button();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.numAuto = new Sci.Win.UI.NumericBox();
            this.numClick = new Sci.Win.UI.NumericBox();
            this.panel_Side = new Sci.Win.UI.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblsumCount = new Sci.Win.UI.Label();
            this.timerClick = new System.Windows.Forms.Timer(this.components);
            this.timerAuto = new System.Windows.Forms.Timer(this.components);
            this.panel_Main.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel_Set.SuspendLayout();
            this.panel_Side.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_Main
            // 
            this.panel_Main.AutoScroll = true;
            this.panel_Main.AutoScrollMinSize = new System.Drawing.Size(0, 210);
            this.panel_Main.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Main.Controls.Add(this.panel3);
            this.panel_Main.Controls.Add(this.panel1);
            this.panel_Main.Controls.Add(this.panel_Set);
            this.panel_Main.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel_Main.DrawBorder = true;
            this.panel_Main.Location = new System.Drawing.Point(26, 0);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.RectStyle.BorderColor = System.Drawing.Color.Transparent;
            this.panel_Main.Size = new System.Drawing.Size(212, 531);
            this.panel_Main.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(212, 416);
            this.panel3.TabIndex = 21;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.lblRefreshTime);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 34);
            this.panel1.TabIndex = 37;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.SystemColors.Control;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnRefresh.Location = new System.Drawing.Point(16, 8);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(63, 19);
            this.btnRefresh.TabIndex = 25;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // lblRefreshTime
            // 
            this.lblRefreshTime.BackColor = System.Drawing.Color.Transparent;
            this.lblRefreshTime.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRefreshTime.Location = new System.Drawing.Point(133, 8);
            this.lblRefreshTime.Name = "lblRefreshTime";
            this.lblRefreshTime.RectStyle.Color = System.Drawing.Color.Transparent;
            this.lblRefreshTime.RectStyle.Shadow = true;
            this.lblRefreshTime.Size = new System.Drawing.Size(52, 21);
            this.lblRefreshTime.TabIndex = 24;
            this.lblRefreshTime.Text = "23:59:59";
            this.lblRefreshTime.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRefreshTime.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.lblRefreshTime.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // panel_Set
            // 
            this.panel_Set.Controls.Add(this.label5);
            this.panel_Set.Controls.Add(this.btnSet);
            this.panel_Set.Controls.Add(this.label4);
            this.panel_Set.Controls.Add(this.label3);
            this.panel_Set.Controls.Add(this.label2);
            this.panel_Set.Controls.Add(this.numAuto);
            this.panel_Set.Controls.Add(this.numClick);
            this.panel_Set.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Set.Location = new System.Drawing.Point(0, 450);
            this.panel_Set.Name = "panel_Set";
            this.panel_Set.Size = new System.Drawing.Size(212, 81);
            this.panel_Set.TabIndex = 36;
            this.panel_Set.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(163, 34);
            this.label5.Name = "label5";
            this.label5.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label5.RectStyle.Shadow = true;
            this.label5.Size = new System.Drawing.Size(29, 21);
            this.label5.TabIndex = 42;
            this.label5.Text = "sec";
            this.label5.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.SystemColors.Control;
            this.btnSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.btnSet.Location = new System.Drawing.Point(116, 58);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(63, 19);
            this.btnSet.TabIndex = 41;
            this.btnSet.Text = "set";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.BtnSet_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(163, 7);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label4.RectStyle.Shadow = true;
            this.label4.Size = new System.Drawing.Size(29, 21);
            this.label4.TabIndex = 40;
            this.label4.Text = "sec";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(19, 34);
            this.label3.Name = "label3";
            this.label3.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label3.RectStyle.Shadow = true;
            this.label3.Size = new System.Drawing.Size(52, 21);
            this.label3.TabIndex = 39;
            this.label3.Text = "自動刷新";
            this.label3.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 7);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label2.RectStyle.Shadow = true;
            this.label2.Size = new System.Drawing.Size(52, 21);
            this.label2.TabIndex = 38;
            this.label2.Text = "手動刷新";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // numAuto
            // 
            this.numAuto.Location = new System.Drawing.Point(77, 34);
            this.numAuto.Name = "numAuto";
            this.numAuto.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAuto.Size = new System.Drawing.Size(83, 21);
            this.numAuto.TabIndex = 37;
            this.numAuto.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numClick
            // 
            this.numClick.Location = new System.Drawing.Point(77, 7);
            this.numClick.Name = "numClick";
            this.numClick.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numClick.Size = new System.Drawing.Size(83, 21);
            this.numClick.TabIndex = 36;
            this.numClick.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // panel_Side
            // 
            this.panel_Side.BackColor = System.Drawing.SystemColors.Control;
            this.panel_Side.BorderSide = ((System.Windows.Forms.Border3DSide)((((System.Windows.Forms.Border3DSide.Left | System.Windows.Forms.Border3DSide.Top) 
            | System.Windows.Forms.Border3DSide.Bottom) 
            | System.Windows.Forms.Border3DSide.Middle)));
            this.panel_Side.Controls.Add(this.pictureBox1);
            this.panel_Side.Controls.Add(this.lblsumCount);
            this.panel_Side.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_Side.DrawBorder = true;
            this.panel_Side.Location = new System.Drawing.Point(0, 0);
            this.panel_Side.Name = "panel_Side";
            this.panel_Side.RectStyle.BorderColor = System.Drawing.Color.Transparent;
            this.panel_Side.RectStyle.ExtBorderColor = System.Drawing.Color.Transparent;
            this.panel_Side.Size = new System.Drawing.Size(26, 531);
            this.panel_Side.TabIndex = 6;
            this.panel_Side.TitleStyle.BorderColor = System.Drawing.Color.Transparent;
            this.panel_Side.TitleStyle.Color = System.Drawing.Color.Transparent;
            this.panel_Side.Click += new System.EventHandler(this.Panel_Side_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 23);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.Panel_Side_Click);
            // 
            // lblsumCount
            // 
            this.lblsumCount.BackColor = System.Drawing.Color.Transparent;
            this.lblsumCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblsumCount.Location = new System.Drawing.Point(0, 29);
            this.lblsumCount.Name = "lblsumCount";
            this.lblsumCount.Size = new System.Drawing.Size(26, 23);
            this.lblsumCount.TabIndex = 3;
            this.lblsumCount.Text = "99+";
            this.lblsumCount.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblsumCount.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.lblsumCount.TextStyle.Color = System.Drawing.Color.Red;
            this.lblsumCount.TextStyle.ExtBorderColor = System.Drawing.Color.Maroon;
            this.lblsumCount.Visible = false;
            // 
            // timerClick
            // 
            this.timerClick.Tick += new System.EventHandler(this.TimerClick_Tick);
            // 
            // timerAuto
            // 
            this.timerAuto.Enabled = true;
            this.timerAuto.Interval = 600000;
            this.timerAuto.Tick += new System.EventHandler(this.TimerAuto_Tick);
            // 
            // MainNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 11F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_Side);
            this.Controls.Add(this.panel_Main);
            this.Font = new System.Drawing.Font("新細明體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "MainNotification";
            this.Size = new System.Drawing.Size(238, 531);
            this.panel_Main.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel_Set.ResumeLayout(false);
            this.panel_Set.PerformLayout();
            this.panel_Side.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Sci.Win.UI.Panel panel_Main;
        private Sci.Win.UI.Panel panel3;
        private Sci.Win.UI.Panel panel_Side;
        private Sci.Win.UI.Label lblsumCount;
        private System.Windows.Forms.Timer timerClick;
        private System.Windows.Forms.Timer timerAuto;
        private Sci.Win.UI.Panel panel_Set;
        private Sci.Win.UI.Label label5;
        private Sci.Win.UI.Button btnSet;
        private Sci.Win.UI.Label label4;
        private Sci.Win.UI.Label label3;
        private Sci.Win.UI.Label label2;
        private Sci.Win.UI.NumericBox numAuto;
        private Sci.Win.UI.NumericBox numClick;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Sci.Win.UI.Panel panel1;
        private Sci.Win.UI.Button btnRefresh;
        private Sci.Win.UI.Label lblRefreshTime;
    }
}
