namespace Sci.Production.Cutting
{
    partial class P16
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridCuttingReasonInput = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateBox();
            this.labRemark = new Sci.Win.UI.Label();
            this.btnBatchUpdate = new Sci.Win.UI.Button();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.txtReason = new Sci.Win.UI.TextBox();
            this.labReason = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            ((System.ComponentModel.ISupportInitialize)(this.gridCuttingReasonInput)).BeginInit();
            this.SuspendLayout();
            // 
            // gridCuttingReasonInput
            // 
            this.gridCuttingReasonInput.AllowUserToAddRows = false;
            this.gridCuttingReasonInput.AllowUserToDeleteRows = false;
            this.gridCuttingReasonInput.AllowUserToResizeRows = false;
            this.gridCuttingReasonInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCuttingReasonInput.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCuttingReasonInput.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCuttingReasonInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCuttingReasonInput.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCuttingReasonInput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCuttingReasonInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCuttingReasonInput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCuttingReasonInput.Location = new System.Drawing.Point(12, 47);
            this.gridCuttingReasonInput.Name = "gridCuttingReasonInput";
            this.gridCuttingReasonInput.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCuttingReasonInput.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCuttingReasonInput.RowTemplate.Height = 24;
            this.gridCuttingReasonInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCuttingReasonInput.ShowCellToolTips = false;
            this.gridCuttingReasonInput.Size = new System.Drawing.Size(984, 436);
            this.gridCuttingReasonInput.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridCuttingReasonInput.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(916, 489);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Est. Cut Date";
            // 
            // dateEstCutDate
            // 
            // 
            // 
            // 
            this.dateEstCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCutDate.DateBox1.Name = "";
            this.dateEstCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCutDate.DateBox2.Name = "";
            this.dateEstCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox2.TabIndex = 1;
            this.dateEstCutDate.IsRequired = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(107, 9);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(820, 489);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(882, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(403, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Factory";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(543, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "SP#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(590, 9);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(100, 23);
            this.txtSPNo.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(698, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "ETA";
            // 
            // dateETA
            // 
            this.dateETA.Location = new System.Drawing.Point(746, 9);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(130, 23);
            this.dateETA.TabIndex = 3;
            // 
            // labRemark
            // 
            this.labRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labRemark.Location = new System.Drawing.Point(322, 496);
            this.labRemark.Name = "labRemark";
            this.labRemark.Size = new System.Drawing.Size(65, 23);
            this.labRemark.TabIndex = 15;
            this.labRemark.Text = "Remark";
            // 
            // btnBatchUpdate
            // 
            this.btnBatchUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBatchUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchUpdate.Location = new System.Drawing.Point(513, 493);
            this.btnBatchUpdate.Name = "btnBatchUpdate";
            this.btnBatchUpdate.Size = new System.Drawing.Size(118, 30);
            this.btnBatchUpdate.TabIndex = 7;
            this.btnBatchUpdate.Text = "Batch Update";
            this.btnBatchUpdate.UseVisualStyleBackColor = true;
            this.btnBatchUpdate.Click += new System.EventHandler(this.btnBatchUpdate_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(390, 496);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(117, 23);
            this.txtRemark.TabIndex = 5;
            // 
            // txtReason
            // 
            this.txtReason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason.Location = new System.Drawing.Point(80, 496);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(239, 23);
            this.txtReason.TabIndex = 4;
            this.txtReason.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtReason_PopUp);
            this.txtReason.Validating += new System.ComponentModel.CancelEventHandler(this.txtReason_Validating);
            // 
            // labReason
            // 
            this.labReason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labReason.Location = new System.Drawing.Point(12, 496);
            this.labReason.Name = "labReason";
            this.labReason.Size = new System.Drawing.Size(65, 23);
            this.labReason.TabIndex = 14;
            this.labReason.Text = "Reason";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsMultiselect = false;
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(467, 9);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 1;
            // 
            // P16
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 531);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.labReason);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.labRemark);
            this.Controls.Add(this.btnBatchUpdate);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridCuttingReasonInput);
            this.Name = "P16";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P16. Unfinished Cutting Reason Input";
            this.Controls.SetChildIndex(this.gridCuttingReasonInput, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.btnBatchUpdate, 0);
            this.Controls.SetChildIndex(this.labRemark, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.labReason, 0);
            this.Controls.SetChildIndex(this.txtReason, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridCuttingReasonInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridCuttingReasonInput;
        private Win.UI.Button btnClose;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnQuery;
        private Win.UI.Label label2;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label label4;
        private Win.UI.DateBox dateETA;
        private Win.UI.Label labRemark;
        private Win.UI.Button btnBatchUpdate;
        private Win.UI.TextBox txtRemark;
        private Win.UI.TextBox txtReason;
        private Win.UI.Label labReason;
    }
}