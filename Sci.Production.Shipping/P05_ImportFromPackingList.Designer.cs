namespace Sci.Production.Shipping
{
    partial class P05_ImportFromPackingList
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.txtSpStart = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateDelivery = new Sci.Win.UI.DateRange();
            this.labelDelivery = new Sci.Win.UI.Label();
            this.dateSDPDate = new Sci.Win.UI.DateRange();
            this.txtmultifactoryFactory = new Sci.Production.Class.Txtmultifactory();
            this.labelSDPDate = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.labCancelOrder = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 458);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(934, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 458);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labCancelOrder);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtSPEnd);
            this.panel3.Controls.Add(this.txtSpStart);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.dateDelivery);
            this.panel3.Controls.Add(this.labelDelivery);
            this.panel3.Controls.Add(this.dateSDPDate);
            this.panel3.Controls.Add(this.txtmultifactoryFactory);
            this.panel3.Controls.Add(this.labelSDPDate);
            this.panel3.Controls.Add(this.labelFactory);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(929, 97);
            this.panel3.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(691, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "~";
            // 
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(713, 13);
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(100, 23);
            this.txtSPEnd.TabIndex = 9;
            // 
            // txtSpStart
            // 
            this.txtSpStart.BackColor = System.Drawing.Color.White;
            this.txtSpStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpStart.Location = new System.Drawing.Point(585, 13);
            this.txtSpStart.Name = "txtSpStart";
            this.txtSpStart.Size = new System.Drawing.Size(100, 23);
            this.txtSpStart.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(538, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "SP#";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(834, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateDelivery
            // 
            // 
            // 
            // 
            this.dateDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDelivery.DateBox1.Name = "";
            this.dateDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDelivery.DateBox2.Name = "";
            this.dateDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDelivery.DateBox2.TabIndex = 1;
            this.dateDelivery.IsRequired = false;
            this.dateDelivery.Location = new System.Drawing.Point(444, 44);
            this.dateDelivery.Name = "dateDelivery";
            this.dateDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateDelivery.TabIndex = 5;
            // 
            // labelDelivery
            // 
            this.labelDelivery.Location = new System.Drawing.Point(383, 44);
            this.labelDelivery.Name = "labelDelivery";
            this.labelDelivery.Size = new System.Drawing.Size(57, 23);
            this.labelDelivery.TabIndex = 4;
            this.labelDelivery.Text = "Delivery";
            // 
            // dateSDPDate
            // 
            // 
            // 
            // 
            this.dateSDPDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSDPDate.DateBox1.Name = "";
            this.dateSDPDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSDPDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSDPDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSDPDate.DateBox2.Name = "";
            this.dateSDPDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSDPDate.DateBox2.TabIndex = 1;
            this.dateSDPDate.IsRequired = false;
            this.dateSDPDate.Location = new System.Drawing.Point(75, 44);
            this.dateSDPDate.Name = "dateSDPDate";
            this.dateSDPDate.Size = new System.Drawing.Size(280, 23);
            this.dateSDPDate.TabIndex = 3;
            // 
            // txtmultifactoryFactory
            // 
            this.txtmultifactoryFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmultifactoryFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmultifactoryFactory.IsSupportEditMode = false;
            this.txtmultifactoryFactory.Location = new System.Drawing.Point(75, 13);
            this.txtmultifactoryFactory.Name = "txtmultifactoryFactory";
            this.txtmultifactoryFactory.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtmultifactoryFactory.ReadOnly = true;
            this.txtmultifactoryFactory.Size = new System.Drawing.Size(450, 23);
            this.txtmultifactoryFactory.TabIndex = 2;
            // 
            // labelSDPDate
            // 
            this.labelSDPDate.Location = new System.Drawing.Point(4, 44);
            this.labelSDPDate.Name = "labelSDPDate";
            this.labelSDPDate.Size = new System.Drawing.Size(67, 23);
            this.labelSDPDate.TabIndex = 1;
            this.labelSDPDate.Text = "SDP Date";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(4, 13);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(67, 23);
            this.labelFactory.TabIndex = 0;
            this.labelFactory.Text = "Factory";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 416);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(929, 42);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(755, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(664, 7);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridImport);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 97);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(929, 319);
            this.panel5.TabIndex = 5;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowHeadersVisible = false;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(929, 319);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // labCancelOrder
            // 
            this.labCancelOrder.AutoSize = true;
            this.labCancelOrder.ForeColor = System.Drawing.Color.Red;
            this.labCancelOrder.Location = new System.Drawing.Point(7, 72);
            this.labCancelOrder.Name = "labCancelOrder";
            this.labCancelOrder.Size = new System.Drawing.Size(246, 17);
            this.labCancelOrder.TabIndex = 11;
            this.labCancelOrder.Text = "Cancel order cannot import in the GB.";
            // 
            // P05_ImportFromPackingList
            // 
            this.ClientSize = new System.Drawing.Size(939, 458);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P05_ImportFromPackingList";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Import from Packing List";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Label labelSDPDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Panel panel4;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridImport;
        private Win.UI.Button btnQuery;
        private Win.UI.DateRange dateDelivery;
        private Win.UI.Label labelDelivery;
        private Win.UI.DateRange dateSDPDate;
        private Class.Txtmultifactory txtmultifactoryFactory;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.TextBox txtSpStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labCancelOrder;
    }
}
