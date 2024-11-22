namespace Sci.Production.Cutting
{
    partial class B05
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.labelID = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.label5 = new Sci.Win.UI.Label();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnCreateNewCalendar = new Sci.Win.UI.Button();
            this.txtCell1 = new Sci.Production.Class.TxtCell();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.btnRemoveCalendar = new Sci.Win.UI.Button();
            this.btnEditCalendar = new Sci.Win.UI.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.displayCrossday = new Sci.Win.UI.DisplayBox();
            this.dateStart = new Sci.Win.UI.DateBox();
            this.label4 = new Sci.Win.UI.Label();
            this.calendarGrid1 = new Sci.Production.Class.Controls.CalendarGrid();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(1000, 599);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.calendarGrid1);
            this.detailcont.Controls.Add(this.btnRemoveCalendar);
            this.detailcont.Controls.Add(this.btnEditCalendar);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.displayCrossday);
            this.detailcont.Controls.Add(this.dateStart);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.panel1);
            this.detailcont.Size = new System.Drawing.Size(1000, 561);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 561);
            this.detailbtm.Size = new System.Drawing.Size(1000, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1000, 599);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1008, 628);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(224, 10);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 1;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(11, 85);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(104, 23);
            this.labelDescription.TabIndex = 10;
            this.labelDescription.Text = "Description";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(118, 10);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 23);
            this.txtID.TabIndex = 0;
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(11, 10);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(104, 23);
            this.labelID.TabIndex = 8;
            this.labelID.Text = "Spreading No.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "M";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 23);
            this.label2.TabIndex = 18;
            this.label2.Text = "Cut Cell";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtfactory1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.editBox1);
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnCreateNewCalendar);
            this.panel1.Controls.Add(this.txtCell1);
            this.panel1.Controls.Add(this.labelID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtID);
            this.panel1.Controls.Add(this.txtMdivision1);
            this.panel1.Controls.Add(this.labelDescription);
            this.panel1.Controls.Add(this.checkJunk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 170);
            this.panel1.TabIndex = 19;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory1.FilteMDivision = true;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsMultiselect = false;
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(118, 60);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 23);
            this.label5.TabIndex = 19;
            this.label5.Text = "Factory";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(118, 85);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.editBox1.Size = new System.Drawing.Size(265, 23);
            this.editBox1.TabIndex = 3;
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
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(389, 10);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(603, 152);
            this.grid1.TabIndex = 6;
            this.grid1.SelectionChanged += new System.EventHandler(this.Grid1_SelectionChanged);
            // 
            // btnCreateNewCalendar
            // 
            this.btnCreateNewCalendar.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnCreateNewCalendar.Location = new System.Drawing.Point(11, 135);
            this.btnCreateNewCalendar.Name = "btnCreateNewCalendar";
            this.btnCreateNewCalendar.Size = new System.Drawing.Size(207, 30);
            this.btnCreateNewCalendar.TabIndex = 5;
            this.btnCreateNewCalendar.Text = "Create new Calendar";
            this.btnCreateNewCalendar.UseVisualStyleBackColor = true;
            this.btnCreateNewCalendar.Click += new System.EventHandler(this.BtnCreateNewCalendar_Click);
            // 
            // txtCell1
            // 
            this.txtCell1.BackColor = System.Drawing.Color.White;
            this.txtCell1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CutCellID", true));
            this.txtCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell1.Location = new System.Drawing.Point(118, 110);
            this.txtCell1.MDivisionID = "";
            this.txtCell1.Name = "txtCell1";
            this.txtCell1.Size = new System.Drawing.Size(100, 23);
            this.txtCell1.TabIndex = 4;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMdivision1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MDivisionid", true));
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMdivision1.IsSupportEditMode = false;
            this.txtMdivision1.Location = new System.Drawing.Point(118, 35);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.ReadOnly = true;
            this.txtMdivision1.Size = new System.Drawing.Size(100, 23);
            this.txtMdivision1.TabIndex = 2;
            // 
            // btnRemoveCalendar
            // 
            this.btnRemoveCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveCalendar.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnRemoveCalendar.Location = new System.Drawing.Point(836, 173);
            this.btnRemoveCalendar.Name = "btnRemoveCalendar";
            this.btnRemoveCalendar.Size = new System.Drawing.Size(158, 30);
            this.btnRemoveCalendar.TabIndex = 9;
            this.btnRemoveCalendar.Text = "Remove Calendar";
            this.btnRemoveCalendar.UseVisualStyleBackColor = true;
            this.btnRemoveCalendar.Click += new System.EventHandler(this.BtnRemoveCalendar_Click);
            // 
            // btnEditCalendar
            // 
            this.btnEditCalendar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditCalendar.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnEditCalendar.Location = new System.Drawing.Point(672, 173);
            this.btnEditCalendar.Name = "btnEditCalendar";
            this.btnEditCalendar.Size = new System.Drawing.Size(158, 30);
            this.btnEditCalendar.TabIndex = 8;
            this.btnEditCalendar.Text = "Edit Calendar";
            this.btnEditCalendar.UseVisualStyleBackColor = true;
            this.btnEditCalendar.Click += new System.EventHandler(this.BtnEditCalendar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(269, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 17);
            this.label3.TabIndex = 59;
            this.label3.Text = "Cross-day";
            // 
            // displayCrossday
            // 
            this.displayCrossday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCrossday.Enabled = false;
            this.displayCrossday.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayCrossday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCrossday.Location = new System.Drawing.Point(243, 179);
            this.displayCrossday.Name = "displayCrossday";
            this.displayCrossday.Size = new System.Drawing.Size(20, 21);
            this.displayCrossday.TabIndex = 58;
            // 
            // dateStart
            // 
            this.dateStart.IsSupportEditMode = false;
            this.dateStart.Location = new System.Drawing.Point(107, 178);
            this.dateStart.Name = "dateStart";
            this.dateStart.ReadOnly = true;
            this.dateStart.Size = new System.Drawing.Size(130, 23);
            this.dateStart.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 23);
            this.label4.TabIndex = 56;
            this.label4.Text = "Start Date";
            // 
            // calendarGrid1
            // 
            this.calendarGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.calendarGrid1.ColorCrossDay = System.Drawing.Color.Pink;
            this.calendarGrid1.EnableHearder = false;
            this.calendarGrid1.Location = new System.Drawing.Point(12, 207);
            this.calendarGrid1.Name = "calendarGrid1";
            this.calendarGrid1.Size = new System.Drawing.Size(981, 348);
            this.calendarGrid1.TabIndex = 10;
            // 
            // B05
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B05 Spreading No.";
            this.WorkAlias = "SpreadingNo";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelDescription;
        private Win.UI.TextBox txtID;
        private Win.UI.Label labelID;
        private Win.UI.Label label2;
        private Class.TxtCell txtCell1;
        private Win.UI.Label label1;
        private Class.TxtMdivision txtMdivision1;
        private System.Windows.Forms.Panel panel1;
        private Win.UI.Grid grid1;
        private Win.UI.Button btnCreateNewCalendar;
        private Win.UI.EditBox editBox1;
        private Win.UI.Button btnRemoveCalendar;
        private Win.UI.Button btnEditCalendar;
        private System.Windows.Forms.Label label3;
        private Win.UI.DisplayBox displayCrossday;
        private Win.UI.DateBox dateStart;
        private Win.UI.Label label4;
        private Class.Controls.CalendarGrid calendarGrid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Class.Txtfactory txtfactory1;
        private Win.UI.Label label5;
    }
}
