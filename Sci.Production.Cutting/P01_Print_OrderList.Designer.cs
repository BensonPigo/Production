namespace Sci.Production.Cutting
{
    partial class P01_Print_OrderList
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioGroupTTLConsumption = new Sci.Win.UI.RadioGroup();
            this.radioButton1 = new Sci.Win.UI.RadioButton();
            this.radioButton2 = new Sci.Win.UI.RadioButton();
            this.label1 = new Sci.Win.UI.Label();
            this.comboTapeSort = new Sci.Win.UI.ComboBox();
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.rdIncludeCancelQty = new Sci.Win.UI.RadioButton();
            this.rdExcludeCancelQty = new Sci.Win.UI.RadioButton();
            this.radioCuttingTape = new Sci.Win.UI.RadioButton();
            this.radioCuttingschedule = new Sci.Win.UI.RadioButton();
            this.radioQtyBreakdown_PoCombbySPList = new Sci.Win.UI.RadioButton();
            this.radioCuttingWorkOrder = new Sci.Win.UI.RadioButton();
            this.radioEachConsumption = new Sci.Win.UI.RadioButton();
            this.radioConsumptionCalculateByMarkerListConsPerPC = new Sci.Win.UI.RadioButton();
            this.radioTTLConsumption = new Sci.Win.UI.RadioButton();
            this.radioMarkerList = new Sci.Win.UI.RadioButton();
            this.radioColorQtyBDown = new Sci.Win.UI.RadioButton();
            this.radioEachConsVSOrderQtyBDown = new Sci.Win.UI.RadioButton();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
            this.radioForPlanning = new Sci.Win.UI.RadioButton();
            this.groupBox1.SuspendLayout();
            this.radioGroupTTLConsumption.SuspendLayout();
            this.radioGroup1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioForPlanning);
            this.groupBox1.Controls.Add(this.radioGroupTTLConsumption);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboTapeSort);
            this.groupBox1.Controls.Add(this.radioGroup1);
            this.groupBox1.Controls.Add(this.radioCuttingTape);
            this.groupBox1.Controls.Add(this.radioCuttingschedule);
            this.groupBox1.Controls.Add(this.radioQtyBreakdown_PoCombbySPList);
            this.groupBox1.Controls.Add(this.radioCuttingWorkOrder);
            this.groupBox1.Controls.Add(this.radioEachConsumption);
            this.groupBox1.Controls.Add(this.radioConsumptionCalculateByMarkerListConsPerPC);
            this.groupBox1.Controls.Add(this.radioTTLConsumption);
            this.groupBox1.Controls.Add(this.radioMarkerList);
            this.groupBox1.Controls.Add(this.radioColorQtyBDown);
            this.groupBox1.Controls.Add(this.radioEachConsVSOrderQtyBDown);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 408);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // radioGroupTTLConsumption
            // 
            this.radioGroupTTLConsumption.Controls.Add(this.radioButton1);
            this.radioGroupTTLConsumption.Controls.Add(this.radioButton2);
            this.radioGroupTTLConsumption.Location = new System.Drawing.Point(29, 187);
            this.radioGroupTTLConsumption.Name = "radioGroupTTLConsumption";
            this.radioGroupTTLConsumption.Size = new System.Drawing.Size(337, 36);
            this.radioGroupTTLConsumption.TabIndex = 186;
            this.radioGroupTTLConsumption.TabStop = false;
            this.radioGroupTTLConsumption.Value = "S";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton1.Location = new System.Drawing.Point(1, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(156, 21);
            this.radioButton1.TabIndex = 188;
            this.radioButton1.Text = "Trade system format";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Value = "T";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButton2.Location = new System.Drawing.Point(163, 12);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(157, 21);
            this.radioButton2.TabIndex = 189;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Segmentation format";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Value = "S";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(145, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 185;
            this.label1.Text = "Sort By";
            // 
            // comboTapeSort
            // 
            this.comboTapeSort.BackColor = System.Drawing.Color.White;
            this.comboTapeSort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTapeSort.FormattingEnabled = true;
            this.comboTapeSort.IsSupportUnselect = true;
            this.comboTapeSort.Items.AddRange(new object[] {
            "",
            "Color"});
            this.comboTapeSort.Location = new System.Drawing.Point(203, 75);
            this.comboTapeSort.Name = "comboTapeSort";
            this.comboTapeSort.OldText = "";
            this.comboTapeSort.Size = new System.Drawing.Size(99, 24);
            this.comboTapeSort.TabIndex = 184;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.rdIncludeCancelQty);
            this.radioGroup1.Controls.Add(this.rdExcludeCancelQty);
            this.radioGroup1.Location = new System.Drawing.Point(28, 244);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(298, 36);
            this.radioGroup1.TabIndex = 183;
            this.radioGroup1.TabStop = false;
            // 
            // rdIncludeCancelQty
            // 
            this.rdIncludeCancelQty.AutoSize = true;
            this.rdIncludeCancelQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdIncludeCancelQty.Location = new System.Drawing.Point(1, 12);
            this.rdIncludeCancelQty.Name = "rdIncludeCancelQty";
            this.rdIncludeCancelQty.Size = new System.Drawing.Size(144, 21);
            this.rdIncludeCancelQty.TabIndex = 188;
            this.rdIncludeCancelQty.Text = "Include Cancel Qty";
            this.rdIncludeCancelQty.UseVisualStyleBackColor = true;
            // 
            // rdExcludeCancelQty
            // 
            this.rdExcludeCancelQty.AutoSize = true;
            this.rdExcludeCancelQty.Checked = true;
            this.rdExcludeCancelQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdExcludeCancelQty.Location = new System.Drawing.Point(148, 12);
            this.rdExcludeCancelQty.Name = "rdExcludeCancelQty";
            this.rdExcludeCancelQty.Size = new System.Drawing.Size(148, 21);
            this.rdExcludeCancelQty.TabIndex = 189;
            this.rdExcludeCancelQty.TabStop = true;
            this.rdExcludeCancelQty.Text = "Exclude Cancel Qty";
            this.rdExcludeCancelQty.UseVisualStyleBackColor = true;
            // 
            // radioCuttingTape
            // 
            this.radioCuttingTape.AutoSize = true;
            this.radioCuttingTape.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioCuttingTape.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCuttingTape.Location = new System.Drawing.Point(6, 75);
            this.radioCuttingTape.Name = "radioCuttingTape";
            this.radioCuttingTape.Size = new System.Drawing.Size(118, 24);
            this.radioCuttingTape.TabIndex = 1;
            this.radioCuttingTape.Text = "Cutting Tape";
            this.radioCuttingTape.UseVisualStyleBackColor = true;
            // 
            // radioCuttingschedule
            // 
            this.radioCuttingschedule.AutoSize = true;
            this.radioCuttingschedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioCuttingschedule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCuttingschedule.Location = new System.Drawing.Point(6, 105);
            this.radioCuttingschedule.Name = "radioCuttingschedule";
            this.radioCuttingschedule.Size = new System.Drawing.Size(146, 24);
            this.radioCuttingschedule.TabIndex = 2;
            this.radioCuttingschedule.Text = "Cutting schedule";
            this.radioCuttingschedule.UseVisualStyleBackColor = true;
            // 
            // radioQtyBreakdown_PoCombbySPList
            // 
            this.radioQtyBreakdown_PoCombbySPList.AutoSize = true;
            this.radioQtyBreakdown_PoCombbySPList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioQtyBreakdown_PoCombbySPList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioQtyBreakdown_PoCombbySPList.Location = new System.Drawing.Point(6, 281);
            this.radioQtyBreakdown_PoCombbySPList.Name = "radioQtyBreakdown_PoCombbySPList";
            this.radioQtyBreakdown_PoCombbySPList.Size = new System.Drawing.Size(296, 24);
            this.radioQtyBreakdown_PoCombbySPList.TabIndex = 6;
            this.radioQtyBreakdown_PoCombbySPList.Text = "Q\'ty Breakdown(Po Comb by SP# List)";
            this.radioQtyBreakdown_PoCombbySPList.UseVisualStyleBackColor = true;
            // 
            // radioCuttingWorkOrder
            // 
            this.radioCuttingWorkOrder.AutoSize = true;
            this.radioCuttingWorkOrder.Checked = true;
            this.radioCuttingWorkOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioCuttingWorkOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCuttingWorkOrder.Location = new System.Drawing.Point(6, 20);
            this.radioCuttingWorkOrder.Name = "radioCuttingWorkOrder";
            this.radioCuttingWorkOrder.Size = new System.Drawing.Size(244, 24);
            this.radioCuttingWorkOrder.TabIndex = 0;
            this.radioCuttingWorkOrder.TabStop = true;
            this.radioCuttingWorkOrder.Text = "Cutting Work Order For Output";
            this.radioCuttingWorkOrder.UseVisualStyleBackColor = true;
            // 
            // radioEachConsumption
            // 
            this.radioEachConsumption.AutoSize = true;
            this.radioEachConsumption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioEachConsumption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioEachConsumption.Location = new System.Drawing.Point(6, 135);
            this.radioEachConsumption.Name = "radioEachConsumption";
            this.radioEachConsumption.Size = new System.Drawing.Size(282, 24);
            this.radioEachConsumption.TabIndex = 3;
            this.radioEachConsumption.Text = "Each Consumption (Cutting Combo)";
            this.radioEachConsumption.UseVisualStyleBackColor = true;
            // 
            // radioConsumptionCalculateByMarkerListConsPerPC
            // 
            this.radioConsumptionCalculateByMarkerListConsPerPC.AutoSize = true;
            this.radioConsumptionCalculateByMarkerListConsPerPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioConsumptionCalculateByMarkerListConsPerPC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioConsumptionCalculateByMarkerListConsPerPC.Location = new System.Drawing.Point(6, 371);
            this.radioConsumptionCalculateByMarkerListConsPerPC.Name = "radioConsumptionCalculateByMarkerListConsPerPC";
            this.radioConsumptionCalculateByMarkerListConsPerPC.Size = new System.Drawing.Size(383, 24);
            this.radioConsumptionCalculateByMarkerListConsPerPC.TabIndex = 9;
            this.radioConsumptionCalculateByMarkerListConsPerPC.Text = "Consumption Calculate by Marker List Cons/Per pc";
            this.radioConsumptionCalculateByMarkerListConsPerPC.UseVisualStyleBackColor = true;
            // 
            // radioTTLConsumption
            // 
            this.radioTTLConsumption.AutoSize = true;
            this.radioTTLConsumption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioTTLConsumption.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTTLConsumption.Location = new System.Drawing.Point(6, 165);
            this.radioTTLConsumption.Name = "radioTTLConsumption";
            this.radioTTLConsumption.Size = new System.Drawing.Size(240, 24);
            this.radioTTLConsumption.TabIndex = 4;
            this.radioTTLConsumption.Text = "TTL consumption (PO Combo)";
            this.radioTTLConsumption.UseVisualStyleBackColor = true;
            // 
            // radioMarkerList
            // 
            this.radioMarkerList.AutoSize = true;
            this.radioMarkerList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioMarkerList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMarkerList.Location = new System.Drawing.Point(6, 341);
            this.radioMarkerList.Name = "radioMarkerList";
            this.radioMarkerList.Size = new System.Drawing.Size(105, 24);
            this.radioMarkerList.TabIndex = 8;
            this.radioMarkerList.Text = "Marker List";
            this.radioMarkerList.UseVisualStyleBackColor = true;
            // 
            // radioColorQtyBDown
            // 
            this.radioColorQtyBDown.AutoSize = true;
            this.radioColorQtyBDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioColorQtyBDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioColorQtyBDown.Location = new System.Drawing.Point(6, 224);
            this.radioColorQtyBDown.Name = "radioColorQtyBDown";
            this.radioColorQtyBDown.Size = new System.Drawing.Size(249, 24);
            this.radioColorQtyBDown.TabIndex = 5;
            this.radioColorQtyBDown.Text = "Color & Q\'ty B\'Down (PO Combo)";
            this.radioColorQtyBDown.UseVisualStyleBackColor = true;
            // 
            // radioEachConsVSOrderQtyBDown
            // 
            this.radioEachConsVSOrderQtyBDown.AutoSize = true;
            this.radioEachConsVSOrderQtyBDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioEachConsVSOrderQtyBDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioEachConsVSOrderQtyBDown.Location = new System.Drawing.Point(6, 311);
            this.radioEachConsVSOrderQtyBDown.Name = "radioEachConsVSOrderQtyBDown";
            this.radioEachConsVSOrderQtyBDown.Size = new System.Drawing.Size(350, 24);
            this.radioEachConsVSOrderQtyBDown.TabIndex = 7;
            this.radioEachConsVSOrderQtyBDown.Text = "Each cons. vs Order Q\'ty B\'Down (PO Combo)";
            this.radioEachConsVSOrderQtyBDown.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(439, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(439, 26);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(439, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 182;
            this.label5.Text = "Pager Size A4";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // radioForPlanning
            // 
            this.radioForPlanning.AutoSize = true;
            this.radioForPlanning.Checked = false;
            this.radioForPlanning.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.radioForPlanning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioForPlanning.Location = new System.Drawing.Point(6, 45);
            this.radioForPlanning.Name = "radioForPlanning";
            this.radioForPlanning.Size = new System.Drawing.Size(256, 24);
            this.radioForPlanning.TabIndex = 187;
            this.radioForPlanning.TabStop = true;
            this.radioForPlanning.Text = "Cutting Work Order For Planning";
            this.radioForPlanning.UseVisualStyleBackColor = true;
            // 
            // P01_Print_OrderList
            // 
            this.ClientSize = new System.Drawing.Size(558, 432);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Name = "P01_Print_OrderList";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.btnToExcel, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.radioGroupTTLConsumption.ResumeLayout(false);
            this.radioGroupTTLConsumption.PerformLayout();
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.RadioButton radioEachConsumption;
        private Win.UI.RadioButton radioConsumptionCalculateByMarkerListConsPerPC;
        private Win.UI.RadioButton radioTTLConsumption;
        private Win.UI.RadioButton radioMarkerList;
        private Win.UI.RadioButton radioColorQtyBDown;
        private Win.UI.RadioButton radioEachConsVSOrderQtyBDown;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnToExcel;
        private Win.UI.Label label5;
        private Win.UI.RadioButton radioQtyBreakdown_PoCombbySPList;
        private Win.UI.RadioButton radioCuttingWorkOrder;
        private Win.UI.RadioButton radioCuttingschedule;
        private Win.UI.RadioButton radioCuttingTape;
        private Win.UI.RadioButton rdExcludeCancelQty;
        private Win.UI.RadioButton rdIncludeCancelQty;
        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboTapeSort;
        private Win.UI.RadioGroup radioGroupTTLConsumption;
        private Win.UI.RadioButton radioButton1;
        private Win.UI.RadioButton radioButton2;
        private Win.UI.RadioButton radioForPlanning;
    }
}
