using Sci.Production.Class;

namespace Sci.Production.Cutting
{
    partial class P09_ActionCutRef
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
            this.label1 = new Sci.Win.UI.Label();
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
            this.txtActCuttingPerimeter = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.txtStraightLength = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.txtCurvedLength = new Sci.Win.UI.TextBox();
            this.label14 = new Sci.Win.UI.Label();
            this.label15 = new Sci.Win.UI.Label();
            this.label16 = new Sci.Win.UI.Label();
            this.dateBoxEstCutDate = new Sci.Production.Class.DateEstCutDate();
            this.label17 = new Sci.Win.UI.Label();
            this.label18 = new Sci.Win.UI.Label();
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
            this.label21 = new System.Windows.Forms.Label();
            this.gridDistributeToSP = new Sci.Win.UI.Grid();
            this.cmsDistribute = new Sci.Win.UI.ContextMenuStrip();
            this.MenuItemInsertDistribute = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemDeleteDistribute = new System.Windows.Forms.ToolStripMenuItem();
            this.distributebs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnModify = new Sci.Win.UI.Button();
            this.numCutno = new Sci.Win.UI.NumericBox();
            this.numLayers = new Sci.Win.UI.NumericBox();
            this.txtDropDownList1 = new Sci.Production.Class.TxtDropDownList();
            this.txtCell = new Sci.Production.Class.TxtCell();
            this.txtSpreadingNo = new Sci.Win.UI.TextBox();
            this.numConsPC = new Sci.Win.UI.NumericBox();
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
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Cut#";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 23);
            this.label2.TabIndex = 18;
            this.label2.Text = "Layers";
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(135, 77);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(96, 23);
            this.txtSeq1.TabIndex = 2;
            this.txtSeq1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            this.txtSeq1.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeq_Validating);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(9, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 23);
            this.label3.TabIndex = 20;
            this.label3.Text = "Seq1";
            // 
            // txtSeq2
            // 
            this.txtSeq2.BackColor = System.Drawing.Color.White;
            this.txtSeq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq2.Location = new System.Drawing.Point(135, 111);
            this.txtSeq2.Name = "txtSeq2";
            this.txtSeq2.Size = new System.Drawing.Size(96, 23);
            this.txtSeq2.TabIndex = 3;
            this.txtSeq2.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            this.txtSeq2.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSeq_Validating);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(9, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 23);
            this.label4.TabIndex = 22;
            this.label4.Text = "Seq2";
            // 
            // txtRefNo
            // 
            this.txtRefNo.BackColor = System.Drawing.Color.White;
            this.txtRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefNo.Location = new System.Drawing.Point(135, 145);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(114, 23);
            this.txtRefNo.TabIndex = 4;
            this.txtRefNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(9, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 23);
            this.label5.TabIndex = 24;
            this.label5.Text = "RefNo";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(135, 179);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(96, 23);
            this.txtColor.TabIndex = 5;
            this.txtColor.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(9, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 23);
            this.label6.TabIndex = 26;
            this.label6.Text = "Color";
            // 
            // txtTone
            // 
            this.txtTone.BackColor = System.Drawing.Color.White;
            this.txtTone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTone.Location = new System.Drawing.Point(135, 213);
            this.txtTone.Name = "txtTone";
            this.txtTone.Size = new System.Drawing.Size(96, 23);
            this.txtTone.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(9, 213);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 23);
            this.label7.TabIndex = 28;
            this.label7.Text = "Tone";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(9, 247);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 23);
            this.label8.TabIndex = 30;
            this.label8.Text = "Unit Cons";
            // 
            // txtMarkerName
            // 
            this.txtMarkerName.BackColor = System.Drawing.Color.White;
            this.txtMarkerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerName.Location = new System.Drawing.Point(135, 281);
            this.txtMarkerName.Name = "txtMarkerName";
            this.txtMarkerName.Size = new System.Drawing.Size(114, 23);
            this.txtMarkerName.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(9, 281);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 23);
            this.label9.TabIndex = 32;
            this.label9.Text = "Marker Name";
            // 
            // txtMarkerNo
            // 
            this.txtMarkerNo.BackColor = System.Drawing.Color.White;
            this.txtMarkerNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerNo.Location = new System.Drawing.Point(135, 315);
            this.txtMarkerNo.Name = "txtMarkerNo";
            this.txtMarkerNo.Size = new System.Drawing.Size(114, 23);
            this.txtMarkerNo.TabIndex = 9;
            this.txtMarkerNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtMarkerNo_PopUp);
            this.txtMarkerNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMarkerNo_Validating);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(9, 315);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 23);
            this.label10.TabIndex = 34;
            this.label10.Text = "Pattern No.";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(9, 349);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 23);
            this.label11.TabIndex = 36;
            this.label11.Text = "Marker Length";
            // 
            // txtActCuttingPerimeter
            // 
            this.txtActCuttingPerimeter.BackColor = System.Drawing.Color.White;
            this.txtActCuttingPerimeter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtActCuttingPerimeter.Location = new System.Drawing.Point(153, 383);
            this.txtActCuttingPerimeter.Mask = "000Yd00\"00";
            this.txtActCuttingPerimeter.Name = "txtActCuttingPerimeter";
            this.txtActCuttingPerimeter.Size = new System.Drawing.Size(96, 23);
            this.txtActCuttingPerimeter.TabIndex = 11;
            this.txtActCuttingPerimeter.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMasked_Validating);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(9, 383);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(141, 23);
            this.label12.TabIndex = 38;
            this.label12.Text = "Act Cutting Perimeter";
            // 
            // txtStraightLength
            // 
            this.txtStraightLength.BackColor = System.Drawing.Color.White;
            this.txtStraightLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStraightLength.Location = new System.Drawing.Point(153, 417);
            this.txtStraightLength.Mask = "000Yd00\"00";
            this.txtStraightLength.Name = "txtStraightLength";
            this.txtStraightLength.Size = new System.Drawing.Size(96, 23);
            this.txtStraightLength.TabIndex = 12;
            this.txtStraightLength.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMasked_Validating);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(9, 417);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(141, 23);
            this.label13.TabIndex = 40;
            this.label13.Text = "Straight Length";
            // 
            // txtCurvedLength
            // 
            this.txtCurvedLength.BackColor = System.Drawing.Color.White;
            this.txtCurvedLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCurvedLength.Location = new System.Drawing.Point(153, 451);
            this.txtCurvedLength.Mask = "000Yd00\"00";
            this.txtCurvedLength.Name = "txtCurvedLength";
            this.txtCurvedLength.Size = new System.Drawing.Size(96, 23);
            this.txtCurvedLength.TabIndex = 13;
            this.txtCurvedLength.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMasked_Validating);
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(9, 451);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(141, 23);
            this.label14.TabIndex = 42;
            this.label14.Text = "Curved Length";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(280, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(102, 23);
            this.label15.TabIndex = 44;
            this.label15.Text = "Est. Cut Date";
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(280, 43);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(102, 23);
            this.label16.TabIndex = 46;
            this.label16.Text = "Spreading No";
            // 
            // dateBoxEstCutDate
            // 
            this.dateBoxEstCutDate.Location = new System.Drawing.Point(385, 9);
            this.dateBoxEstCutDate.Name = "dateBoxEstCutDate";
            this.dateBoxEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateBoxEstCutDate.TabIndex = 14;
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(521, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 23);
            this.label17.TabIndex = 49;
            this.label17.Text = "Cut Cell";
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label18.Location = new System.Drawing.Point(521, 43);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(67, 23);
            this.label18.TabIndex = 51;
            this.label18.Text = "Shift";
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
            this.gridPatternPanel.Location = new System.Drawing.Point(279, 90);
            this.gridPatternPanel.Name = "gridPatternPanel";
            this.gridPatternPanel.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPatternPanel.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPatternPanel.RowTemplate.Height = 24;
            this.gridPatternPanel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPatternPanel.ShowCellToolTips = false;
            this.gridPatternPanel.Size = new System.Drawing.Size(261, 102);
            this.gridPatternPanel.TabIndex = 18;
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
            this.label19.Location = new System.Drawing.Point(276, 70);
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
            this.gridSizeRatio.Location = new System.Drawing.Point(546, 90);
            this.gridSizeRatio.Name = "gridSizeRatio";
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSizeRatio.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSizeRatio.RowTemplate.Height = 24;
            this.gridSizeRatio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSizeRatio.ShowCellToolTips = false;
            this.gridSizeRatio.Size = new System.Drawing.Size(175, 102);
            this.gridSizeRatio.TabIndex = 19;
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
            this.label20.Location = new System.Drawing.Point(543, 70);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 17);
            this.label20.TabIndex = 53;
            this.label20.Text = "Size Ratio";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(277, 195);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(119, 17);
            this.label21.TabIndex = 57;
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
            this.gridDistributeToSP.Location = new System.Drawing.Point(279, 215);
            this.gridDistributeToSP.Name = "gridDistributeToSP";
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDistributeToSP.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDistributeToSP.RowTemplate.Height = 24;
            this.gridDistributeToSP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDistributeToSP.ShowCellToolTips = false;
            this.gridDistributeToSP.Size = new System.Drawing.Size(442, 230);
            this.gridDistributeToSP.TabIndex = 20;
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(639, 451);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 28);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnModify.Location = new System.Drawing.Point(546, 451);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(92, 28);
            this.btnModify.TabIndex = 21;
            this.btnModify.Text = "Create";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.BtnModify_Click);
            // 
            // numCutno
            // 
            this.numCutno.BackColor = System.Drawing.Color.White;
            this.numCutno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numCutno.IsSupportNegative = false;
            this.numCutno.Location = new System.Drawing.Point(135, 9);
            this.numCutno.Name = "numCutno";
            this.numCutno.NullValue = null;
            this.numCutno.Size = new System.Drawing.Size(100, 23);
            this.numCutno.TabIndex = 0;
            this.numCutno.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numLayers
            // 
            this.numLayers.BackColor = System.Drawing.Color.White;
            this.numLayers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numLayers.IsSupportNegative = false;
            this.numLayers.Location = new System.Drawing.Point(135, 43);
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
            // txtDropDownList1
            // 
            this.txtDropDownList1.BackColor = System.Drawing.Color.White;
            this.txtDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDropDownList1.Location = new System.Drawing.Point(591, 43);
            this.txtDropDownList1.Name = "txtDropDownList1";
            this.txtDropDownList1.Size = new System.Drawing.Size(130, 23);
            this.txtDropDownList1.TabIndex = 17;
            this.txtDropDownList1.Type = "Pms_WorkOrderShift";
            // 
            // txtCell
            // 
            this.txtCell.BackColor = System.Drawing.Color.White;
            this.txtCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell.Location = new System.Drawing.Point(591, 9);
            this.txtCell.MDivisionID = "";
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(130, 23);
            this.txtCell.TabIndex = 16;
            // 
            // txtSpreadingNo
            // 
            this.txtSpreadingNo.BackColor = System.Drawing.Color.White;
            this.txtSpreadingNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpreadingNo.Location = new System.Drawing.Point(385, 43);
            this.txtSpreadingNo.Name = "txtSpreadingNo";
            this.txtSpreadingNo.Size = new System.Drawing.Size(130, 23);
            this.txtSpreadingNo.TabIndex = 15;
            this.txtSpreadingNo.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSpreadingNo_PopUp);
            this.txtSpreadingNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSpreadingNo_Validating);
            // 
            // numConsPC
            // 
            this.numConsPC.BackColor = System.Drawing.Color.White;
            this.numConsPC.DecimalPlaces = 4;
            this.numConsPC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numConsPC.Location = new System.Drawing.Point(135, 247);
            this.numConsPC.Name = "numConsPC";
            this.numConsPC.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numConsPC.Size = new System.Drawing.Size(94, 23);
            this.numConsPC.TabIndex = 7;
            this.numConsPC.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numConsPC.Validated += new System.EventHandler(this.NumConsPC_Validated);
            // 
            // txtMarkerLength
            // 
            this.txtMarkerLength.BackColor = System.Drawing.Color.White;
            this.txtMarkerLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkerLength.Location = new System.Drawing.Point(135, 349);
            this.txtMarkerLength.Mask = "00Y00-0/0+0\"";
            this.txtMarkerLength.Name = "txtMarkerLength";
            this.txtMarkerLength.Size = new System.Drawing.Size(114, 23);
            this.txtMarkerLength.TabIndex = 10;
            this.txtMarkerLength.TextMaskFormat = System.Windows.Forms.MaskFormat.IncludePromptAndLiterals;
            this.txtMarkerLength.Validating += new System.ComponentModel.CancelEventHandler(this.TxtMarkerLength_Validating);
            // 
            // P09_ActionCutRef
            // 
            this.ClientSize = new System.Drawing.Size(739, 488);
            this.Controls.Add(this.txtMarkerLength);
            this.Controls.Add(this.numConsPC);
            this.Controls.Add(this.txtSpreadingNo);
            this.Controls.Add(this.txtCell);
            this.Controls.Add(this.txtDropDownList1);
            this.Controls.Add(this.numLayers);
            this.Controls.Add(this.numCutno);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.gridDistributeToSP);
            this.Controls.Add(this.gridPatternPanel);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.gridSizeRatio);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.dateBoxEstCutDate);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.txtCurvedLength);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtStraightLength);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtActCuttingPerimeter);
            this.Controls.Add(this.label12);
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
            this.Controls.Add(this.label1);
            this.DefaultControl = "txtCutRef";
            this.Name = "P09_ActionCutRef";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P09 {0} CutRef";
            this.Controls.SetChildIndex(this.label1, 0);
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
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtActCuttingPerimeter, 0);
            this.Controls.SetChildIndex(this.label13, 0);
            this.Controls.SetChildIndex(this.txtStraightLength, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.txtCurvedLength, 0);
            this.Controls.SetChildIndex(this.label15, 0);
            this.Controls.SetChildIndex(this.label16, 0);
            this.Controls.SetChildIndex(this.dateBoxEstCutDate, 0);
            this.Controls.SetChildIndex(this.label17, 0);
            this.Controls.SetChildIndex(this.label18, 0);
            this.Controls.SetChildIndex(this.label20, 0);
            this.Controls.SetChildIndex(this.gridSizeRatio, 0);
            this.Controls.SetChildIndex(this.label19, 0);
            this.Controls.SetChildIndex(this.gridPatternPanel, 0);
            this.Controls.SetChildIndex(this.gridDistributeToSP, 0);
            this.Controls.SetChildIndex(this.label21, 0);
            this.Controls.SetChildIndex(this.btnModify, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.numCutno, 0);
            this.Controls.SetChildIndex(this.numLayers, 0);
            this.Controls.SetChildIndex(this.txtDropDownList1, 0);
            this.Controls.SetChildIndex(this.txtCell, 0);
            this.Controls.SetChildIndex(this.txtSpreadingNo, 0);
            this.Controls.SetChildIndex(this.numConsPC, 0);
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

        private Win.UI.Label label1;
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
        private Win.UI.TextBox txtActCuttingPerimeter;
        private Win.UI.Label label12;
        private Win.UI.TextBox txtStraightLength;
        private Win.UI.Label label13;
        private Win.UI.TextBox txtCurvedLength;
        private Win.UI.Label label14;
        private Win.UI.Label label15;
        private Win.UI.Label label16;
        private Win.UI.Label label17;
        private Win.UI.Label label18;
        private Win.UI.Grid gridPatternPanel;
        private System.Windows.Forms.Label label19;
        private Win.UI.Grid gridSizeRatio;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private Win.UI.Grid gridDistributeToSP;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnModify;
        private Win.UI.NumericBox numCutno;
        private Win.UI.NumericBox numLayers;
        private Class.TxtDropDownList txtDropDownList1;
        private Class.TxtCell txtCell;
        private Win.UI.TextBox txtSpreadingNo;
        private Win.UI.NumericBox numConsPC;
        private Win.UI.ListControlBindingSource patternpanelbs;
        private Win.UI.ListControlBindingSource sizeRatiobs;
        private Win.UI.ListControlBindingSource distributebs;
        private Win.UI.ContextMenuStrip cmsSizeRatio;
        private System.Windows.Forms.ToolStripMenuItem MenuItemInsertSizeRatio;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeleteSizeRatio;
        private Win.UI.ContextMenuStrip cmsPatternPanel;
        private System.Windows.Forms.ToolStripMenuItem MenuItemInsertPatternPanel;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeletePatternPanel;
        private Win.UI.ContextMenuStrip cmsDistribute;
        private System.Windows.Forms.ToolStripMenuItem MenuItemInsertDistribute;
        private System.Windows.Forms.ToolStripMenuItem MenuItemDeleteDistribute;
        private DateEstCutDate dateBoxEstCutDate;
        private TxtMarkerLength txtMarkerLength;
    }
}
