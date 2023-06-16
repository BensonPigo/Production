namespace Sci.Production.Warehouse
{
    partial class P68
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
            this.lblCuttingDate = new Sci.Win.UI.Label();
            this.cutingDate = new Sci.Win.UI.DateRange();
            this.lblFactory = new Sci.Win.UI.Label();
            this.lblCutpalnID = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtPOID = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.cbStatus = new Sci.Win.UI.ComboBox();
            this.lblCutCell = new Sci.Win.UI.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCutCell_Value1 = new Sci.Win.UI.TextBox();
            this.txtCutCell_Value2 = new Sci.Win.UI.TextBox();
            this.txtmultifactory1 = new Sci.Production.Class.Txtmultifactory();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid2 = new Sci.Win.UI.Grid();
            this.txtCutplanID = new Sci.Win.UI.TextBox();
            this.bindingDetail = new Sci.Win.UI.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCuttingDate
            // 
            this.lblCuttingDate.Location = new System.Drawing.Point(9, 9);
            this.lblCuttingDate.Name = "lblCuttingDate";
            this.lblCuttingDate.Size = new System.Drawing.Size(94, 23);
            this.lblCuttingDate.TabIndex = 2;
            this.lblCuttingDate.Text = "Cutting Date";
            // 
            // cutingDate
            // 
            // 
            // 
            // 
            this.cutingDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.cutingDate.DateBox1.Name = "";
            this.cutingDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.cutingDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.cutingDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.cutingDate.DateBox2.Name = "";
            this.cutingDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.cutingDate.DateBox2.TabIndex = 1;
            this.cutingDate.Location = new System.Drawing.Point(106, 9);
            this.cutingDate.Name = "cutingDate";
            this.cutingDate.Size = new System.Drawing.Size(280, 23);
            this.cutingDate.TabIndex = 3;
            // 
            // lblFactory
            // 
            this.lblFactory.Location = new System.Drawing.Point(9, 46);
            this.lblFactory.Name = "lblFactory";
            this.lblFactory.Size = new System.Drawing.Size(94, 23);
            this.lblFactory.TabIndex = 4;
            this.lblFactory.Text = "Factory";
            // 
            // lblCutpalnID
            // 
            this.lblCutpalnID.Location = new System.Drawing.Point(400, 9);
            this.lblCutpalnID.Name = "lblCutpalnID";
            this.lblCutpalnID.Size = new System.Drawing.Size(72, 23);
            this.lblCutpalnID.TabIndex = 5;
            this.lblCutpalnID.Text = "Cutplan ID";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(400, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "POID";
            // 
            // txtPOID
            // 
            this.txtPOID.BackColor = System.Drawing.Color.White;
            this.txtPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPOID.IsSupportEditMode = false;
            this.txtPOID.Location = new System.Drawing.Point(475, 46);
            this.txtPOID.Name = "txtPOID";
            this.txtPOID.Size = new System.Drawing.Size(121, 23);
            this.txtPOID.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(611, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "Status";
            // 
            // cbStatus
            // 
            this.cbStatus.BackColor = System.Drawing.Color.White;
            this.cbStatus.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cbStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.IsSupportUnselect = true;
            this.cbStatus.Location = new System.Drawing.Point(686, 46);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.OldText = "";
            this.cbStatus.Size = new System.Drawing.Size(121, 24);
            this.cbStatus.TabIndex = 12;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.CbStatus_SelectedIndexChanged);
            // 
            // lblCutCell
            // 
            this.lblCutCell.Location = new System.Drawing.Point(611, 9);
            this.lblCutCell.Name = "lblCutCell";
            this.lblCutCell.Size = new System.Drawing.Size(72, 23);
            this.lblCutCell.TabIndex = 13;
            this.lblCutCell.Text = "Cut Cell";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(813, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 17);
            this.label6.TabIndex = 14;
            this.label6.Text = "～";
            // 
            // txtCutCell_Value1
            // 
            this.txtCutCell_Value1.BackColor = System.Drawing.Color.White;
            this.txtCutCell_Value1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell_Value1.IsSupportEditMode = false;
            this.txtCutCell_Value1.Location = new System.Drawing.Point(686, 9);
            this.txtCutCell_Value1.Name = "txtCutCell_Value1";
            this.txtCutCell_Value1.Size = new System.Drawing.Size(121, 23);
            this.txtCutCell_Value1.TabIndex = 15;
            // 
            // txtCutCell_Value2
            // 
            this.txtCutCell_Value2.BackColor = System.Drawing.Color.White;
            this.txtCutCell_Value2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell_Value2.IsSupportEditMode = false;
            this.txtCutCell_Value2.Location = new System.Drawing.Point(841, 9);
            this.txtCutCell_Value2.Name = "txtCutCell_Value2";
            this.txtCutCell_Value2.Size = new System.Drawing.Size(121, 23);
            this.txtCutCell_Value2.TabIndex = 16;
            // 
            // txtmultifactory1
            // 
            this.txtmultifactory1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtmultifactory1.CheckFtyGroup = false;
            this.txtmultifactory1.CheckProduceFty = false;
            this.txtmultifactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtmultifactory1.IsDataFromA2B = false;
            this.txtmultifactory1.IsForPackingA2B = false;
            this.txtmultifactory1.IsMDivisionID = true;
            this.txtmultifactory1.IsSupportEditMode = false;
            this.txtmultifactory1.Location = new System.Drawing.Point(107, 46);
            this.txtmultifactory1.MDivisionID = "";
            this.txtmultifactory1.Name = "txtmultifactory1";
            this.txtmultifactory1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtmultifactory1.ReadOnly = true;
            this.txtmultifactory1.Size = new System.Drawing.Size(279, 23);
            this.txtmultifactory1.SystemName = "";
            this.txtmultifactory1.TabIndex = 17;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(981, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 18;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(981, 42);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Location = new System.Drawing.Point(9, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1052, 244);
            this.panel1.TabIndex = 21;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(3, 3);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(1046, 238);
            this.grid1.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.grid1.TabIndex = 3;
            this.grid1.TabStop = false;
            this.grid1.SelectionChanged += new System.EventHandler(this.Grid1_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.grid2);
            this.panel2.Location = new System.Drawing.Point(9, 325);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1052, 293);
            this.panel2.TabIndex = 22;
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(3, 3);
            this.grid2.Name = "grid2";
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.ShowCellToolTips = false;
            this.grid2.Size = new System.Drawing.Size(1046, 287);
            this.grid2.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.grid2.TabIndex = 4;
            this.grid2.TabStop = false;
            // 
            // txtCutplanID
            // 
            this.txtCutplanID.BackColor = System.Drawing.Color.White;
            this.txtCutplanID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutplanID.IsSupportEditMode = false;
            this.txtCutplanID.Location = new System.Drawing.Point(475, 9);
            this.txtCutplanID.Name = "txtCutplanID";
            this.txtCutplanID.Size = new System.Drawing.Size(121, 23);
            this.txtCutplanID.TabIndex = 23;
            // 
            // P68
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 630);
            this.Controls.Add(this.txtCutplanID);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtmultifactory1);
            this.Controls.Add(this.txtCutCell_Value2);
            this.Controls.Add(this.txtCutCell_Value1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblCutCell);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPOID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCutpalnID);
            this.Controls.Add(this.lblFactory);
            this.Controls.Add(this.cutingDate);
            this.Controls.Add(this.lblCuttingDate);
            this.Name = "P68";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P68. Fabric issuance status";
            this.Controls.SetChildIndex(this.lblCuttingDate, 0);
            this.Controls.SetChildIndex(this.cutingDate, 0);
            this.Controls.SetChildIndex(this.lblFactory, 0);
            this.Controls.SetChildIndex(this.lblCutpalnID, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtPOID, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cbStatus, 0);
            this.Controls.SetChildIndex(this.lblCutCell, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtCutCell_Value1, 0);
            this.Controls.SetChildIndex(this.txtCutCell_Value2, 0);
            this.Controls.SetChildIndex(this.txtmultifactory1, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.txtCutplanID, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lblCuttingDate;
        private Win.UI.DateRange cutingDate;
        private Win.UI.Label lblFactory;
        private Win.UI.Label lblCutpalnID;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtPOID;
        private Win.UI.Label label4;
        private Win.UI.ComboBox cbStatus;
        private Win.UI.Label lblCutCell;
        private System.Windows.Forms.Label label6;
        private Win.UI.TextBox txtCutCell_Value1;
        private Win.UI.TextBox txtCutCell_Value2;
        private Class.Txtmultifactory txtmultifactory1;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private Win.UI.Grid grid1;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Grid grid2;
        private Win.UI.TextBox txtCutplanID;
        private Win.UI.BindingSource bindingDetail;
    }
}