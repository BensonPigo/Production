namespace Sci.Production.PPIC
{
    partial class P15
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelDate = new Sci.Win.UI.Label();
            this.dateRangeDate = new Sci.Win.UI.DateRange();
            this.labelM = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.labelShift = new Sci.Win.UI.Label();
            this.labelStatus = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.comboBoxShift = new Sci.Win.UI.ComboBox();
            this.comboBoxFbType = new Sci.Win.UI.ComboBox();
            this.comboBoxStatus = new Sci.Win.UI.ComboBox();
            this.checkBoxRotate = new Sci.Win.UI.CheckBox();
            this.comboBoxRotate = new Sci.Win.UI.ComboBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.displayUseStock = new Sci.Win.UI.DisplayBox();
            this.displayBoxPrepairing = new Sci.Win.UI.DisplayBox();
            this.displayBoxReady = new Sci.Win.UI.DisplayBox();
            this.displayBoxFinish = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.detailbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.timerRotate = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailbs)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(9, 9);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(48, 23);
            this.labelDate.TabIndex = 1;
            this.labelDate.Text = "Date";
            // 
            // dateRangeDate
            // 
            // 
            // 
            // 
            this.dateRangeDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeDate.DateBox1.Name = "";
            this.dateRangeDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeDate.DateBox2.Name = "";
            this.dateRangeDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeDate.DateBox2.TabIndex = 1;
            this.dateRangeDate.IsRequired = false;
            this.dateRangeDate.Location = new System.Drawing.Point(60, 9);
            this.dateRangeDate.Name = "dateRangeDate";
            this.dateRangeDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeDate.TabIndex = 2;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(9, 44);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(48, 23);
            this.labelM.TabIndex = 3;
            this.labelM.Text = "M";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(168, 44);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(59, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(351, 9);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(82, 23);
            this.labelFabricType.TabIndex = 5;
            this.labelFabricType.Text = "Fabric Type";
            // 
            // labelShift
            // 
            this.labelShift.Location = new System.Drawing.Point(351, 44);
            this.labelShift.Name = "labelShift";
            this.labelShift.Size = new System.Drawing.Size(82, 23);
            this.labelShift.TabIndex = 6;
            this.labelShift.Text = "Shift";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(571, 8);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(69, 23);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Status";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(60, 44);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 8;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(230, 44);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 9;
            // 
            // comboBoxShift
            // 
            this.comboBoxShift.BackColor = System.Drawing.Color.White;
            this.comboBoxShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxShift.FormattingEnabled = true;
            this.comboBoxShift.IsSupportUnselect = true;
            this.comboBoxShift.Location = new System.Drawing.Point(436, 44);
            this.comboBoxShift.Name = "comboBoxShift";
            this.comboBoxShift.OldText = "";
            this.comboBoxShift.Size = new System.Drawing.Size(121, 24);
            this.comboBoxShift.TabIndex = 10;
            // 
            // comboBoxFbType
            // 
            this.comboBoxFbType.BackColor = System.Drawing.Color.White;
            this.comboBoxFbType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxFbType.FormattingEnabled = true;
            this.comboBoxFbType.IsSupportUnselect = true;
            this.comboBoxFbType.Location = new System.Drawing.Point(436, 8);
            this.comboBoxFbType.Name = "comboBoxFbType";
            this.comboBoxFbType.OldText = "";
            this.comboBoxFbType.Size = new System.Drawing.Size(121, 24);
            this.comboBoxFbType.TabIndex = 11;
            // 
            // comboBoxStatus
            // 
            this.comboBoxStatus.BackColor = System.Drawing.Color.White;
            this.comboBoxStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxStatus.FormattingEnabled = true;
            this.comboBoxStatus.IsSupportUnselect = true;
            this.comboBoxStatus.Location = new System.Drawing.Point(643, 7);
            this.comboBoxStatus.Name = "comboBoxStatus";
            this.comboBoxStatus.OldText = "";
            this.comboBoxStatus.Size = new System.Drawing.Size(182, 24);
            this.comboBoxStatus.TabIndex = 12;
            // 
            // checkBoxRotate
            // 
            this.checkBoxRotate.AutoSize = true;
            this.checkBoxRotate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxRotate.Location = new System.Drawing.Point(571, 50);
            this.checkBoxRotate.Name = "checkBoxRotate";
            this.checkBoxRotate.Size = new System.Drawing.Size(69, 21);
            this.checkBoxRotate.TabIndex = 13;
            this.checkBoxRotate.Text = "Rotate";
            this.checkBoxRotate.UseVisualStyleBackColor = true;
            this.checkBoxRotate.CheckedChanged += new System.EventHandler(this.CheckBoxRotate_CheckedChanged);
            // 
            // comboBoxRotate
            // 
            this.comboBoxRotate.BackColor = System.Drawing.Color.White;
            this.comboBoxRotate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxRotate.FormattingEnabled = true;
            this.comboBoxRotate.IsSupportUnselect = true;
            this.comboBoxRotate.Location = new System.Drawing.Point(643, 47);
            this.comboBoxRotate.Name = "comboBoxRotate";
            this.comboBoxRotate.OldText = "";
            this.comboBoxRotate.Size = new System.Drawing.Size(121, 24);
            this.comboBoxRotate.TabIndex = 14;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(916, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 15;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridDetail.DefaultCellStyle = dataGridViewCellStyle1;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(9, 94);
            this.gridDetail.Name = "gridDetail";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(995, 366);
            this.gridDetail.TabIndex = 16;
            this.gridDetail.Sorted += new System.EventHandler(this.GridDetail_Sorted);
            // 
            // displayUseStock
            // 
            this.displayUseStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUseStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayUseStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayUseStock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUseStock.Location = new System.Drawing.Point(9, 74);
            this.displayUseStock.Name = "displayUseStock";
            this.displayUseStock.Size = new System.Drawing.Size(14, 14);
            this.displayUseStock.TabIndex = 41;
            // 
            // displayBoxPrepairing
            // 
            this.displayBoxPrepairing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxPrepairing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayBoxPrepairing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayBoxPrepairing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxPrepairing.Location = new System.Drawing.Point(82, 74);
            this.displayBoxPrepairing.Name = "displayBoxPrepairing";
            this.displayBoxPrepairing.Size = new System.Drawing.Size(14, 14);
            this.displayBoxPrepairing.TabIndex = 42;
            // 
            // displayBoxReady
            // 
            this.displayBoxReady.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxReady.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayBoxReady.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayBoxReady.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxReady.Location = new System.Drawing.Point(171, 74);
            this.displayBoxReady.Name = "displayBoxReady";
            this.displayBoxReady.Size = new System.Drawing.Size(14, 14);
            this.displayBoxReady.TabIndex = 43;
            // 
            // displayBoxFinish
            // 
            this.displayBoxFinish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxFinish.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayBoxFinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayBoxFinish.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxFinish.Location = new System.Drawing.Point(238, 74);
            this.displayBoxFinish.Name = "displayBoxFinish";
            this.displayBoxFinish.Size = new System.Drawing.Size(14, 14);
            this.displayBoxFinish.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(26, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 23);
            this.label1.TabIndex = 45;
            this.label1.Text = "Waiting";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(99, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 23);
            this.label2.TabIndex = 46;
            this.label2.Text = "Preparing";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(188, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 23);
            this.label3.TabIndex = 47;
            this.label3.Text = "Ready";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(255, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 23);
            this.label4.TabIndex = 48;
            this.label4.Text = "Finished";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // timerRotate
            // 
            this.timerRotate.Interval = 60000;
            this.timerRotate.Tick += new System.EventHandler(this.TimerRotate_Tick);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(916, 43);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 49;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P15
            // 
            this.ClientSize = new System.Drawing.Size(1008, 463);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.displayBoxFinish);
            this.Controls.Add(this.displayBoxReady);
            this.Controls.Add(this.displayBoxPrepairing);
            this.Controls.Add(this.displayUseStock);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.comboBoxRotate);
            this.Controls.Add(this.checkBoxRotate);
            this.Controls.Add(this.comboBoxStatus);
            this.Controls.Add(this.comboBoxFbType);
            this.Controls.Add(this.comboBoxShift);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelShift);
            this.Controls.Add(this.labelFabricType);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.dateRangeDate);
            this.Controls.Add(this.labelDate);
            this.Name = "P15";
            this.Text = "P15 Monitor Fabric/Accessory Lacking & Replacement Slip Status";
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.dateRangeDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelFabricType, 0);
            this.Controls.SetChildIndex(this.labelShift, 0);
            this.Controls.SetChildIndex(this.labelStatus, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.comboBoxShift, 0);
            this.Controls.SetChildIndex(this.comboBoxFbType, 0);
            this.Controls.SetChildIndex(this.comboBoxStatus, 0);
            this.Controls.SetChildIndex(this.checkBoxRotate, 0);
            this.Controls.SetChildIndex(this.comboBoxRotate, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.gridDetail, 0);
            this.Controls.SetChildIndex(this.displayUseStock, 0);
            this.Controls.SetChildIndex(this.displayBoxPrepairing, 0);
            this.Controls.SetChildIndex(this.displayBoxReady, 0);
            this.Controls.SetChildIndex(this.displayBoxFinish, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.DateRange dateRangeDate;
        private Win.UI.Label labelM;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelFabricType;
        private Win.UI.Label labelShift;
        private Win.UI.Label labelStatus;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Win.UI.ComboBox comboBoxShift;
        private Win.UI.ComboBox comboBoxFbType;
        private Win.UI.ComboBox comboBoxStatus;
        private Win.UI.CheckBox checkBoxRotate;
        private Win.UI.ComboBox comboBoxRotate;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridDetail;
        private Win.UI.DisplayBox displayUseStock;
        private Win.UI.DisplayBox displayBoxPrepairing;
        private Win.UI.DisplayBox displayBoxReady;
        private Win.UI.DisplayBox displayBoxFinish;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.ListControlBindingSource detailbs;
        private System.Windows.Forms.Timer timerRotate;
        private Win.UI.Button btnClose;
    }
}
