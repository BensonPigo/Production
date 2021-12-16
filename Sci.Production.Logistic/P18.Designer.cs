namespace Sci.Production.Logistic
{
    partial class P18
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.dateTransfer = new Sci.Win.UI.DateRange();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.txtFactory = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.comboTransferTo = new Sci.Win.UI.ComboBox();
            this.label5 = new Sci.Win.UI.Label();
            this.gridTransferSisterFty = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferSisterFty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 471);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(803, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 471);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dateTransfer);
            this.panel3.Controls.Add(this.dateBuyerDelivery);
            this.panel3.Controls.Add(this.txtFactory);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.txtPONo);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Controls.Add(this.txtPackID);
            this.panel3.Controls.Add(this.labelPackID);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(793, 105);
            this.panel3.TabIndex = 3;
            // 
            // dateTransfer
            // 
            // 
            // 
            // 
            this.dateTransfer.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateTransfer.DateBox1.Name = "";
            this.dateTransfer.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateTransfer.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateTransfer.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateTransfer.DateBox2.Name = "";
            this.dateTransfer.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateTransfer.DateBox2.TabIndex = 1;
            this.dateTransfer.Location = new System.Drawing.Point(109, 74);
            this.dateTransfer.Name = "dateTransfer";
            this.dateTransfer.Size = new System.Drawing.Size(280, 23);
            this.dateTransfer.TabIndex = 6;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(109, 41);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 4;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(474, 41);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(130, 23);
            this.txtFactory.TabIndex = 5;
            this.txtFactory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFactory_PopUp);
            this.txtFactory.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFactory_Validating);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(414, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "Factory";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Transfer Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Buyer Delivery";
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.White;
            this.txtPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONo.Location = new System.Drawing.Point(281, 7);
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.Size = new System.Drawing.Size(130, 23);
            this.txtPONo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(242, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "PO#";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(700, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(109, 7);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(130, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(7, 7);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(99, 23);
            this.labelSPNo.TabIndex = 4;
            this.labelSPNo.Text = "SP#";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(474, 7);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(130, 23);
            this.txtPackID.TabIndex = 3;
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(414, 7);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(56, 23);
            this.labelPackID.TabIndex = 2;
            this.labelPackID.Text = "Pack ID";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 424);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(793, 47);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(701, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.comboTransferTo);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.gridTransferSisterFty);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 105);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(793, 319);
            this.panel5.TabIndex = 5;
            // 
            // comboTransferTo
            // 
            this.comboTransferTo.BackColor = System.Drawing.Color.White;
            this.comboTransferTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTransferTo.FormattingEnabled = true;
            this.comboTransferTo.IsSupportUnselect = true;
            this.comboTransferTo.Location = new System.Drawing.Point(109, 3);
            this.comboTransferTo.Name = "comboTransferTo";
            this.comboTransferTo.OldText = "";
            this.comboTransferTo.Size = new System.Drawing.Size(121, 24);
            this.comboTransferTo.TabIndex = 12;
            this.comboTransferTo.SelectedIndexChanged += new System.EventHandler(this.ComboTransferTo_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(7, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 23);
            this.label5.TabIndex = 11;
            this.label5.Text = "Transfer To";
            // 
            // gridTransferSisterFty
            // 
            this.gridTransferSisterFty.AllowUserToAddRows = false;
            this.gridTransferSisterFty.AllowUserToDeleteRows = false;
            this.gridTransferSisterFty.AllowUserToResizeRows = false;
            this.gridTransferSisterFty.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTransferSisterFty.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransferSisterFty.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransferSisterFty.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransferSisterFty.DataSource = this.listControlBindingSource1;
            this.gridTransferSisterFty.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransferSisterFty.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransferSisterFty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransferSisterFty.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransferSisterFty.Location = new System.Drawing.Point(0, 29);
            this.gridTransferSisterFty.Name = "gridTransferSisterFty";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridTransferSisterFty.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridTransferSisterFty.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransferSisterFty.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransferSisterFty.RowTemplate.Height = 24;
            this.gridTransferSisterFty.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransferSisterFty.ShowCellToolTips = false;
            this.gridTransferSisterFty.Size = new System.Drawing.Size(793, 290);
            this.gridTransferSisterFty.TabIndex = 0;
            this.gridTransferSisterFty.TabStop = false;
            // 
            // P18
            // 
            this.ClientSize = new System.Drawing.Size(813, 471);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P18";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P18. Query For Transfer To Sister Factory Record";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferSisterFty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Label labelPackID;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridTransferSisterFty;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Button btnClose;
        private Win.UI.DateRange dateTransfer;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.TextBox txtFactory;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboTransferTo;
        private Win.UI.Label label5;
    }
}
