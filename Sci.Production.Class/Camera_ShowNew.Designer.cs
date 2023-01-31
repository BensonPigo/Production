
namespace Sci.Production.Class
{
    partial class Camera_ShowNew
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
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnOK.Location = new System.Drawing.Point(0, 614);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(548, 47);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(548, 614);
            this.panel1.TabIndex = 2;
            // 
            // Camera_ShowNew
            // 
            this.ClientSize = new System.Drawing.Size(548, 661);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOK);
            this.Name = "Camera_ShowNew";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Resize += new System.EventHandler(this.Camera_ShowNew_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
    }
}
