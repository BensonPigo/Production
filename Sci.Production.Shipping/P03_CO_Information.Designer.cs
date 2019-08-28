namespace Sci.Production.Shipping
{
    partial class P03_CO_Information
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
            this.panel6 = new Sci.Win.UI.Panel();
            this.button1 = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridCO = new Sci.Win.UI.Grid();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCO)).BeginInit();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 358);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(916, 54);
            this.panel6.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(798, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // gridCO
            // 
            this.gridCO.AllowUserToAddRows = false;
            this.gridCO.AllowUserToDeleteRows = false;
            this.gridCO.AllowUserToResizeRows = false;
            this.gridCO.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCO.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCO.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCO.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCO.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCO.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCO.Location = new System.Drawing.Point(0, 0);
            this.gridCO.Name = "gridCO";
            this.gridCO.RowHeadersVisible = false;
            this.gridCO.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCO.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCO.RowTemplate.Height = 24;
            this.gridCO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCO.ShowCellToolTips = false;
            this.gridCO.Size = new System.Drawing.Size(916, 358);
            this.gridCO.TabIndex = 11;
            this.gridCO.TabStop = false;
            // 
            // P03_CO_Information
            // 
            this.ClientSize = new System.Drawing.Size(916, 412);
            this.Controls.Add(this.gridCO);
            this.Controls.Add(this.panel6);
            this.Name = "P03_CO_Information";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Shipping P03. Certificate of Origin information";
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Panel panel6;
        private Win.UI.Button button1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid gridCO;
    }
}
