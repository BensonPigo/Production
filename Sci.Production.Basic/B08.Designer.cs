namespace Sci.Production.Basic
{
    partial class B08
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.displayCDCode = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.displayCPUUnit = new Sci.Win.UI.DisplayBox();
            this.displayCombinationPieces = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.btnProdFabricType = new Sci.Win.UI.Button();
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
            this.detail.Size = new System.Drawing.Size(687, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.btnProdFabricType);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayCombinationPieces);
            this.detailcont.Controls.Add(this.displayCPUUnit);
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.displayCDCode);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(687, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(687, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(687, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(695, 424);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(70, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "CD Code";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(70, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Description";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(71, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "CPU/Unit";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(70, 195);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 23);
            this.label6.TabIndex = 3;
            this.label6.Text = "Combination Pieces";
            // 
            // displayCDCode
            // 
            this.displayCDCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCDCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCDCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCDCode.Location = new System.Drawing.Point(149, 39);
            this.displayCDCode.Name = "displayCDCode";
            this.displayCDCode.Size = new System.Drawing.Size(54, 23);
            this.displayCDCode.TabIndex = 4;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Description", true));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(149, 91);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(288, 23);
            this.displayDescription.TabIndex = 5;
            // 
            // displayCPUUnit
            // 
            this.displayCPUUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCPUUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CPU", true));
            this.displayCPUUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCPUUnit.Location = new System.Drawing.Point(149, 143);
            this.displayCPUUnit.Name = "displayCPUUnit";
            this.displayCPUUnit.Size = new System.Drawing.Size(60, 23);
            this.displayCPUUnit.TabIndex = 6;
            // 
            // displayCombinationPieces
            // 
            this.displayCombinationPieces.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCombinationPieces.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ComboPcs", true));
            this.displayCombinationPieces.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCombinationPieces.Location = new System.Drawing.Point(201, 195);
            this.displayCombinationPieces.Name = "displayCombinationPieces";
            this.displayCombinationPieces.Size = new System.Drawing.Size(36, 23);
            this.displayCombinationPieces.TabIndex = 7;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(352, 39);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 8;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // btnProdFabricType
            // 
            this.btnProdFabricType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnProdFabricType.Location = new System.Drawing.Point(518, 35);
            this.btnProdFabricType.Name = "btnProdFabricType";
            this.btnProdFabricType.Size = new System.Drawing.Size(151, 30);
            this.btnProdFabricType.TabIndex = 9;
            this.btnProdFabricType.Text = "Prod./Fabric Type";
            this.btnProdFabricType.UseVisualStyleBackColor = true;
            this.btnProdFabricType.Click += new System.EventHandler(this.button1_Click);
            // 
            // B08
            // 
            this.ClientSize = new System.Drawing.Size(695, 457);
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B08";
            this.Text = "B08. CD Code";
            this.UniqueExpress = "ID";
            this.WorkAlias = "CDCode";
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

        private Win.UI.Button btnProdFabricType;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayCombinationPieces;
        private Win.UI.DisplayBox displayCPUUnit;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.DisplayBox displayCDCode;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
    }
}
