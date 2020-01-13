namespace Sci.Production.Quality
{
    partial class P50
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelOutputDate = new Sci.Win.UI.Label();
            this.dateBoxOutputDate = new Sci.Win.UI.DateBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridDefectOutput = new Sci.Win.UI.Grid();
            this.bindingGridDefectOutput = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Production.Class.txtfactory();
            this.lbLine = new Sci.Win.UI.Label();
            this.txtLine = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridDefectOutput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingGridDefectOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // labelOutputDate
            // 
            this.labelOutputDate.Location = new System.Drawing.Point(9, 9);
            this.labelOutputDate.Name = "labelOutputDate";
            this.labelOutputDate.Size = new System.Drawing.Size(78, 23);
            this.labelOutputDate.TabIndex = 1;
            this.labelOutputDate.Text = "OutputDate";
            // 
            // dateBoxOutputDate
            // 
            this.dateBoxOutputDate.IsSupportEditMode = false;
            this.dateBoxOutputDate.Location = new System.Drawing.Point(90, 9);
            this.dateBoxOutputDate.Name = "dateBoxOutputDate";
            this.dateBoxOutputDate.Size = new System.Drawing.Size(130, 23);
            this.dateBoxOutputDate.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(560, 2);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 3;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // gridDefectOutput
            // 
            this.gridDefectOutput.AllowUserToAddRows = false;
            this.gridDefectOutput.AllowUserToDeleteRows = false;
            this.gridDefectOutput.AllowUserToResizeRows = false;
            this.gridDefectOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDefectOutput.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDefectOutput.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDefectOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDefectOutput.DataSource = this.bindingGridDefectOutput;
            this.gridDefectOutput.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDefectOutput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDefectOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDefectOutput.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDefectOutput.Location = new System.Drawing.Point(0, 38);
            this.gridDefectOutput.Name = "gridDefectOutput";
            this.gridDefectOutput.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDefectOutput.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridDefectOutput.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDefectOutput.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDefectOutput.RowTemplate.Height = 24;
            this.gridDefectOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDefectOutput.ShowCellToolTips = false;
            this.gridDefectOutput.Size = new System.Drawing.Size(1008, 456);
            this.gridDefectOutput.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridDefectOutput.TabIndex = 4;
            this.gridDefectOutput.Sorted += new System.EventHandler(this.gridDefectOutput_Sorted);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(227, 9);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(78, 23);
            this.labelFactory.TabIndex = 5;
            this.labelFactory.Text = "Factory";
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.boolFtyGroupList = true;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.IsSupportEditMode = false;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(308, 9);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 6;
            // 
            // lbLine
            // 
            this.lbLine.Location = new System.Drawing.Point(377, 9);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new System.Drawing.Size(78, 23);
            this.lbLine.TabIndex = 7;
            this.lbLine.Text = "Line";
            // 
            // txtLine
            // 
            this.txtLine.BackColor = System.Drawing.Color.White;
            this.txtLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLine.IsSupportEditMode = false;
            this.txtLine.Location = new System.Drawing.Point(458, 9);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(100, 23);
            this.txtLine.TabIndex = 8;
            // 
            // P50
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 494);
            this.Controls.Add(this.txtLine);
            this.Controls.Add(this.lbLine);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.gridDefectOutput);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateBoxOutputDate);
            this.Controls.Add(this.labelOutputDate);
            this.Name = "P50";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P50. Query For Output Defect Hourly";
            this.Controls.SetChildIndex(this.labelOutputDate, 0);
            this.Controls.SetChildIndex(this.dateBoxOutputDate, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.gridDefectOutput, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.lbLine, 0);
            this.Controls.SetChildIndex(this.txtLine, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridDefectOutput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingGridDefectOutput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelOutputDate;
        private Win.UI.DateBox dateBoxOutputDate;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridDefectOutput;
        private Win.UI.ListControlBindingSource bindingGridDefectOutput;
        private Win.UI.Label labelFactory;
        private Class.txtfactory txtFactory;
        private Win.UI.Label lbLine;
        private Win.UI.TextBox txtLine;
    }
}