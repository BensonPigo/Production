namespace Sci.Production
{
    partial class Main
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
            this.menus = new Sci.Win.UI.MenuStrip();
            this.mainNotification1 = new Sci.Production.Win.MainNotification();
            this.SuspendLayout();
            // 
            // menus
            // 
            this.menus.CanOverflow = true;
            this.menus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.menus.Location = new System.Drawing.Point(0, 0);
            this.menus.Name = "menus";
            this.menus.Padding = new System.Windows.Forms.Padding(0);
            this.menus.Size = new System.Drawing.Size(769, 24);
            this.menus.TabIndex = 1;
            this.menus.Text = "menuStrip1";
            // 
            // mainNotification1
            // 
            this.mainNotification1.Dock = System.Windows.Forms.DockStyle.Right;
            this.mainNotification1.Font = new System.Drawing.Font("新細明體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.mainNotification1.Location = new System.Drawing.Point(743, 24);
            this.mainNotification1.Name = "mainNotification1";
            this.mainNotification1.Size = new System.Drawing.Size(26, 566);
            this.mainNotification1.TabIndex = 2;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 590);
            this.Controls.Add(this.mainNotification1);
            this.Controls.Add(this.menus);
            this.MainMenuStrip = this.menus;
            this.Name = "Main";
            this.Text = "Login";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Controls.SetChildIndex(this.menus, 0);
            this.Controls.SetChildIndex(this.mainNotification1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.MenuStrip menus;
        private Win.MainNotification mainNotification1;
    }
}

