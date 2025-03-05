using Sci.Production.Class;

namespace Sci.Production.Cutting
{
    partial class P02_ActionCutRef
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
            this.label2 = new Sci.Win.UI.Label();
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.txtSeq2 = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtRefNo = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtTone = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.txtMarkerName = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtMarkerNo = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label15 = new Sci.Win.UI.Label();
            this.dateBoxEstCutDate = new Sci.Production.Class.DateEstCutDate();
            this.gridPatternPanel = new Sci.Win.UI.Grid();
            this.cmsPatternPanel = new Sci.Win.UI.ContextMenuStrip();
            this.MenuItemInsertPatternPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDeletePatternPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.patternpanelbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label19 = new System.Windows.Forms.Label();
            this.gridSizeRatio = new Sci.Win.UI.Grid();
            this.cmsSizeRatio = new Sci.Win.UI.ContextMenuStrip();
            this.MenuItemInsertSizeRatio = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDeleteSizeRatio = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeRatiobs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label20 = new System.Windows.Forms.Label();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnModify = new Sci.Win.UI.Button();
            this.numLayers = new Sci.Win.UI.NumericBox();
            this.numConsPC = new Sci.Win.UI.NumericBox();
            this.label16 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.txtWKETA = new Sci.Win.UI.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.gridDistributeToSP = new Sci.Win.UI.Grid();
            this.cmsDistribute = new Sci.Win.UI.ContextMenuStrip();
            this.MenuItemInsertDistribute = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDeleteDistribute = new System.Windows.Forms.ToolStripMenuItem();
            this.distributebs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtMarkerLength = new Sci.Production.Class.TxtMarkerLength();
            ((System.ComponentModel.ISupportInitialize)(this.gridPatternPanel)).BeginInit();
            this.cmsPatternPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.patternpanelbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSizeRatio)).BeginInit();
            this.cmsSizeRatio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeRatiobs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDistributeToSP)).BeginInit();
            this.cmsDistribute.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.distributebs)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(9, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 23);
            this.label2.TabIndex = 18;
            this.label2.Text = "Layers";
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(135, 140);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(96, 23);
            this.txtSeq1.TabIndex = 21;
            this.txtSeq1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            this.txtSeq1.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeq_Validating);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(9, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 23);
            this.label3.TabIndex = 20;
            this.label3.Text = "Seq1";
            // 
            // txtSeq2
            // 
            this.txtSeq2.BackColor = System.Drawing.Color.White;
            this.txtSeq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq2.Location = new System.Drawing.Point(135, 174);
            this.txtSeq2.Name = "txtSeq2";
            this.txtSeq2.Size = new System.Drawing.Size(96, 23);
            this.txtSeq2.TabIndex = 23;
            this.txtSeq2.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            this.txtSeq2.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeq_Validating);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(9, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 23);
            this.label4.TabIndex = 22;
            this.label4.Text = "Seq2";
            // 
            // txtRefNo
            // 
            this.txtRefNo.BackColor = System.Drawing.Color.White;
            this.txtRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefNo.Location = new System.Drawing.Point(135, 208);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(114, 23);
            this.txtRefNo.TabIndex = 25;
            this.txtRefNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(9, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 23);
            this.label5.TabIndex = 24;
            this.label5.Text = "RefNo";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(135, 242);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(96, 23);
            this.txtColor.TabIndex = 27;
            this.txtColor.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(9, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 23);
            this.label6.TabIndex = 26;
            this.label6.Text = "Color";
            // 
            // txtTone
            // 
            this.txtTone.BackColor = System.Drawing.Color.White;
            this.txtTone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTone.Location = new System.Drawing.Point(135, 276);
            this.txtTone.Name = "txtTone";
            this.txtTone.Size = new System.Drawing.Size(96, 23);
            this.txtTone.TabIndex = 29;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(9, 276);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 23);
            this.label7.TabIndex = 28;
            this.label7.Text = "Tone";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(9, 310);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 23);
            this.label8.TabIndex = 30;
            this.label8.Text = "Unit Cons";
            // 
            // txtMarkerName
            // 
            this.txtMarkerName.BackColor = System.Drawing.Color.White;
            this.txtMarkerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerName.Location = new System.Drawing.Point(135, 344);
            this.txtMarkerName.Name = "txtMarkerName";
            this.txtMarkerName.Size = new System.Drawing.Size(114, 23);
            this.txtMarkerName.TabIndex = 33;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(9, 344);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 23);
            this.label9.TabIndex = 32;
            this.label9.Text = "Marker Name";
            // 
            // txtMarkerNo
            // 
            this.txtMarkerNo.BackColor = System.Drawing.Color.White;
            this.txtMarkerNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerNo.Location = new System.Drawing.Point(135, 378);
            this.txtMarkerNo.Name = "txtMarkerNo";
            this.txtMarkerNo.Size = new System.Drawing.Size(114, 23);
            this.txtMarkerNo.TabIndex = 35;
            this.txtMarkerNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtMarkerNo_PopUp);
            this.txtMarkerNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMarkerNo_Validating);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(9, 378);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 23);
            this.label10.TabIndex = 34;
            this.label10.Text = "Pattern No.";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(9, 412);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 23);
            this.label11.TabIndex = 36;
            this.label11.Text = "Marker Length";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(9, 11);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(102, 23);
            this.label15.TabIndex = 44;
            this.label15.Text = "Est. Cut Date";
            // 
            // dateBoxEstCutDate
            // 
            this.dateBoxEstCutDate.Location = new System.Drawing.Point(114, 11);
            this.dateBoxEstCutDate.Name = "dateBoxEstCutDate";
            this.dateBoxEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateBoxEstCutDate.TabIndex = 47;
            // 
            // gridPatternPanel
            // 
            this.gridPatternPanel.AllowUserToAddRows = false;
            this.gridPatternPanel.AllowUserToDeleteRows = false;
            this.gridPatternPanel.AllowUserToResizeRows = false;
            this.gridPatternPanel.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPatternPanel.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPatternPanel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPatternPanel.ContextMenuStrip = this.cmsPatternPanel;
            this.gridPatternPanel.DataSource = this.patternpanelbs;
            this.gridPatternPanel.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPatternPanel.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPatternPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPatternPanel.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPatternPanel.Location = new System.Drawing.Point(279, 36);
            this.gridPatternPanel.Name = "gridPatternPanel";
            this.gridPatternPanel.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPatternPanel.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPatternPanel.RowTemplate.Height = 24;
            this.gridPatternPanel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPatternPanel.ShowCellToolTips = false;
            this.gridPatternPanel.Size = new System.Drawing.Size(261, 136);
            this.gridPatternPanel.TabIndex = 56;
            // 
            // cmsPatternPanel
            // 
            this.cmsPatternPanel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemInsertPatternPanel,
            this.MenuItemDeletePatternPanel});
            this.cmsPatternPanel.Name = "sizeratioMenuStrip";
            this.cmsPatternPanel.Size = new System.Drawing.Size(182, 48);
            // 
            // MenuItemInsertPatternPanel
            // 
            this.MenuItemInsertPatternPanel.Name = "MenuItemInsertPatternPanel";
            this.MenuItemInsertPatternPanel.Size = new System.Drawing.Size(181, 22);
            this.MenuItemInsertPatternPanel.Text = "Insert Pattern Panel";
            this.MenuItemInsertPatternPanel.Click += new System.EventHandler(this.MenuItemInsertPatternPanel_Click);
            // 
            // MenuItemDeletePatternPanel
            // 
            this.MenuItemDeletePatternPanel.Name = "MenuItemDeletePatternPanel";
            this.MenuItemDeletePatternPanel.Size = new System.Drawing.Size(181, 22);
            this.MenuItemDeletePatternPanel.Text = "Delete Record";
            this.MenuItemDeletePatternPanel.Click += new System.EventHandler(this.MenuItemDeletePatternPanel_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(276, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(94, 17);
            this.label19.TabIndex = 55;
            this.label19.Text = "Pattern Panel";
            // 
            // gridSizeRatio
            // 
            this.gridSizeRatio.AllowUserToAddRows = false;
            this.gridSizeRatio.AllowUserToDeleteRows = false;
            this.gridSizeRatio.AllowUserToResizeRows = false;
            this.gridSizeRatio.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSizeRatio.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSizeRatio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSizeRatio.ContextMenuStrip = this.cmsSizeRatio;
            this.gridSizeRatio.DataSource = this.sizeRatiobs;
            this.gridSizeRatio.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSizeRatio.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSizeRatio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSizeRatio.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSizeRatio.Location = new System.Drawing.Point(546, 36);
            this.gridSizeRatio.Name = "gridSizeRatio";
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSizeRatio.RowTemplate.Height = 24;
            this.gridSizeRatio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSizeRatio.ShowCellToolTips = false;
            this.gridSizeRatio.Size = new System.Drawing.Size(175, 136);
            this.gridSizeRatio.TabIndex = 54;
            // 
            // cmsSizeRatio
            // 
            this.cmsSizeRatio.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemInsertSizeRatio,
            this.MenuItemDeleteSizeRatio});
            this.cmsSizeRatio.Name = "sizeratioMenuStrip";
            this.cmsSizeRatio.Size = new System.Drawing.Size(164, 48);
            // 
            // MenuItemInsertSizeRatio
            // 
            this.MenuItemInsertSizeRatio.Name = "MenuItemInsertSizeRatio";
            this.MenuItemInsertSizeRatio.Size = new System.Drawing.Size(163, 22);
            this.MenuItemInsertSizeRatio.Text = "Insert Size Ratio";
            this.MenuItemInsertSizeRatio.Click += new System.EventHandler(this.MenuItemInsertSizeRatio_Click);
            // 
            // MenuItemDeleteSizeRatio
            // 
            this.MenuItemDeleteSizeRatio.Name = "MenuItemDeleteSizeRatio";
            this.MenuItemDeleteSizeRatio.Size = new System.Drawing.Size(163, 22);
            this.MenuItemDeleteSizeRatio.Text = "Delete Record";
            this.MenuItemDeleteSizeRatio.Click += new System.EventHandler(this.MenuItemDeleteSizeRatio_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(543, 16);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 17);
            this.label20.TabIndex = 53;
            this.label20.Text = "Size Ratio";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(639, 437);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 28);
            this.btnCancel.TabIndex = 60;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnModify.Location = new System.Drawing.Point(546, 437);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(92, 28);
            this.btnModify.TabIndex = 59;
            this.btnModify.Text = "Create";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.BtnModify_Click);
            // 
            // numLayers
            // 
            this.numLayers.BackColor = System.Drawing.Color.White;
            this.numLayers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numLayers.IsSupportNegative = false;
            this.numLayers.Location = new System.Drawing.Point(135, 78);
            this.numLayers.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numLayers.Name = "numLayers";
            this.numLayers.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLayers.Size = new System.Drawing.Size(100, 23);
            this.numLayers.TabIndex = 1;
            this.numLayers.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLayers.Validating += new System.ComponentModel.CancelEventHandler(this.NumLayers_Validating);
            // 
            // numConsPC
            // 
            this.numConsPC.BackColor = System.Drawing.Color.White;
            this.numConsPC.DecimalPlaces = 4;
            this.numConsPC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numConsPC.Location = new System.Drawing.Point(135, 310);
            this.numConsPC.Name = "numConsPC";
            this.numConsPC.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numConsPC.Size = new System.Drawing.Size(94, 23);
            this.numConsPC.TabIndex = 64;
            this.numConsPC.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numConsPC.Validated += new System.EventHandler(this.NumConsPC_Validated);
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(9, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(102, 23);
            this.label16.TabIndex = 65;
            this.label16.Text = "WK ETA";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(9, 109);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(123, 23);
            this.label12.TabIndex = 67;
            this.label12.Text = "SP#";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(135, 109);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(109, 23);
            this.txtSP.TabIndex = 68;
            this.txtSP.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSP_PopUp);
            this.txtSP.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSP_Validating);
            // 
            // txtWKETA
            // 
            this.txtWKETA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWKETA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWKETA.Location = new System.Drawing.Point(114, 46);
            this.txtWKETA.Name = "txtWKETA";
            this.txtWKETA.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtWKETA.ReadOnly = true;
            this.txtWKETA.Size = new System.Drawing.Size(109, 23);
            this.txtWKETA.TabIndex = 69;
            this.txtWKETA.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtWKETA_PopUp);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(276, 180);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(119, 17);
            this.label21.TabIndex = 71;
            this.label21.Text = "Distribute To SP#";
            // 
            // gridDistributeToSP
            // 
            this.gridDistributeToSP.AllowUserToAddRows = false;
            this.gridDistributeToSP.AllowUserToDeleteRows = false;
            this.gridDistributeToSP.AllowUserToResizeRows = false;
            this.gridDistributeToSP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDistributeToSP.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDistributeToSP.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDistributeToSP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDistributeToSP.ContextMenuStrip = this.cmsDistribute;
            this.gridDistributeToSP.DataSource = this.distributebs;
            this.gridDistributeToSP.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDistributeToSP.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDistributeToSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDistributeToSP.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDistributeToSP.Location = new System.Drawing.Point(278, 200);
            this.gridDistributeToSP.Name = "gridDistributeToSP";
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDistributeToSP.RowTemplate.Height = 24;
            this.gridDistributeToSP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDistributeToSP.ShowCellToolTips = false;
            this.gridDistributeToSP.Size = new System.Drawing.Size(442, 230);
            this.gridDistributeToSP.TabIndex = 70;
            // 
            // cmsDistribute
            // 
            this.cmsDistribute.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemInsertDistribute,
            this.MenuItemDeleteDistribute});
            this.cmsDistribute.Name = "contextMenuStrip1";
            this.cmsDistribute.Size = new System.Drawing.Size(162, 48);
            // 
            // MenuItemInsertDistribute
            // 
            this.MenuItemInsertDistribute.Name = "MenuItemInsertDistribute";
            this.MenuItemInsertDistribute.Size = new System.Drawing.Size(161, 22);
            this.MenuItemInsertDistribute.Text = "Insert Distribute";
            this.MenuItemInsertDistribute.Click += new System.EventHandler(this.MenuItemInsertDistribute_Click);
            // 
            // MenuItemDeleteDistribute
            // 
            this.MenuItemDeleteDistribute.Name = "MenuItemDeleteDistribute";
            this.MenuItemDeleteDistribute.Size = new System.Drawing.Size(161, 22);
            this.MenuItemDeleteDistribute.Text = "Delete Record";
            this.MenuItemDeleteDistribute.Click += new System.EventHandler(this.MenuItemDeleteDistribute_Click);
            // 
            // txtMarkerLength
            // 
            this.txtMarkerLength.BackColor = System.Drawing.Color.White;
            this.txtMarkerLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerLength.Location = new System.Drawing.Point(135, 412);
            this.txtMarkerLength.Mask = "00Y00-0/0+0\"";
            this.txtMarkerLength.Name = "txtMarkerLength";
            this.txtMarkerLength.Size = new System.Drawing.Size(114, 23);
            this.txtMarkerLength.TabIndex = 72;
            this.txtMarkerLength.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.txtMarkerLength.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMarkerLength_Validating);
            // 
            // P02_ActionCutRef
            // 
            this.ClientSize = new System.Drawing.Size(739, 477);
            this.Controls.Add(this.txtMarkerLength);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.gridDistributeToSP);
            this.Controls.Add(this.txtWKETA);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.numConsPC);
            this.Controls.Add(this.numLayers);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.gridPatternPanel);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.gridSizeRatio);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.dateBoxEstCutDate);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtMarkerNo);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtMarkerName);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtTone);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtRefNo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSeq2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSeq1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.DefaultControl = "txtCutRef";
            this.Name = "P02_ActionCutRef";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P02 {0} CutRef";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtSeq1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtSeq2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtRefNo, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txtTone, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtMarkerName, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtMarkerNo, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.label15, 0);
            this.Controls.SetChildIndex(this.dateBoxEstCutDate, 0);
            this.Controls.SetChildIndex(this.label20, 0);
            this.Controls.SetChildIndex(this.gridSizeRatio, 0);
            this.Controls.SetChildIndex(this.label19, 0);
            this.Controls.SetChildIndex(this.gridPatternPanel, 0);
            this.Controls.SetChildIndex(this.btnModify, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.numLayers, 0);
            this.Controls.SetChildIndex(this.numConsPC, 0);
            this.Controls.SetChildIndex(this.label16, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.txtWKETA, 0);
            this.Controls.SetChildIndex(this.gridDistributeToSP, 0);
            this.Controls.SetChildIndex(this.label21, 0);
            this.Controls.SetChildIndex(this.txtMarkerLength, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridPatternPanel)).EndInit();
            this.cmsPatternPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.patternpanelbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSizeRatio)).EndInit();
            this.cmsSizeRatio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sizeRatiobs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDistributeToSP)).EndInit();
            this.cmsDistribute.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.distributebs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label label2;
        private Win.UI.TextBox txtSeq1;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtSeq2;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtRefNo;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtTone;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtMarkerName;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtMarkerNo;
        private Win.UI.Label label10;
        private Win.UI.Label label11;
        private Win.UI.Label label15;
        private Win.UI.Grid gridPatternPanel;
        private System.Windows.Forms.Label label19;
        private Win.UI.Grid gridSizeRatio;
        private System.Windows.Forms.Label label20;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnModify;
        private Win.UI.NumericBox numLayers;
        private Win.UI.NumericBox numConsPC;
        private Win.UI.ListControlBindingSource patternpanelbs;
        private Win.UI.ListControlBindingSource sizeRatiobs;
        private Win.UI.ContextMenuStrip cmsSizeRatio;
        private System.Windows.Forms.ToolStripMenuItem MenuItemInsertSizeRatio;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeleteSizeRatio;
        private Win.UI.ContextMenuStrip cmsPatternPanel;
        private System.Windows.Forms.ToolStripMenuItem MenuItemInsertPatternPanel;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeletePatternPanel;
        private Win.UI.Label label16;
        private Win.UI.Label label12;
        private Win.UI.TextBox txtSP;
        private Win.UI.TextBox txtWKETA;
        private DateEstCutDate dateBoxEstCutDate;
        private System.Windows.Forms.Label label21;
        private Win.UI.Grid gridDistributeToSP;
        private Win.UI.ListControlBindingSource distributebs;
        private Win.UI.ContextMenuStrip cmsDistribute;
        private System.Windows.Forms.ToolStripMenuItem MenuItemInsertDistribute;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeleteDistribute;
        private TxtMarkerLength txtMarkerLength;
    }
}
