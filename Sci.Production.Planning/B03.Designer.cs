namespace Sci.Production.Planning
{
    partial class B03
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridArtworkType = new Sci.Win.UI.Grid();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.labelSeason = new Sci.Win.UI.Label();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.labelStyleName = new Sci.Win.UI.Label();
            this.displayStyleName = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkType)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.labelStyleName);
            this.masterpanel.Controls.Add(this.displayStyleName);
            this.masterpanel.Controls.Add(this.labelSeason);
            this.masterpanel.Controls.Add(this.displaySeason);
            this.masterpanel.Controls.Add(this.labelStyle);
            this.masterpanel.Controls.Add(this.displayStyle);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.displayBrand);
            this.masterpanel.Controls.Add(this.gridArtworkType);
            this.masterpanel.Size = new System.Drawing.Size(923, 349);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridArtworkType, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyleName, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyleName, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 349);
            this.detailpanel.Size = new System.Drawing.Size(923, 145);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(784, 311);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(834, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(923, 145);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(923, 532);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(923, 494);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 494);
            this.detailbtm.Size = new System.Drawing.Size(923, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(923, 532);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(931, 561);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(478, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(430, 13);
            // 
            // gridArtworkType
            // 
            this.gridArtworkType.AllowUserToAddRows = false;
            this.gridArtworkType.AllowUserToDeleteRows = false;
            this.gridArtworkType.AllowUserToResizeRows = false;
            this.gridArtworkType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridArtworkType.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridArtworkType.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridArtworkType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridArtworkType.DataSource = this.listControlBindingSource1;
            this.gridArtworkType.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridArtworkType.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridArtworkType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridArtworkType.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridArtworkType.Location = new System.Drawing.Point(5, 72);
            this.gridArtworkType.Name = "gridArtworkType";
            this.gridArtworkType.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridArtworkType.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridArtworkType.RowTemplate.Height = 24;
            this.gridArtworkType.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridArtworkType.ShowCellToolTips = false;
            this.gridArtworkType.Size = new System.Drawing.Size(918, 233);
            this.gridArtworkType.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridArtworkType.TabIndex = 1;
            this.gridArtworkType.TabStop = false;
            this.gridArtworkType.RowSelecting += new System.EventHandler<Ict.Win.UI.DataGridViewRowSelectingEventArgs>(this.GridArtworkType_RowSelecting);
            this.gridArtworkType.Sorted += new System.EventHandler(this.GridArtworkType_Sorted);
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "brandid", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(94, 10);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(100, 23);
            this.displayBrand.TabIndex = 0;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(16, 10);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 23);
            this.labelBrand.TabIndex = 3;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(234, 10);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 5;
            this.labelStyle.Text = "Style";
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(312, 10);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(135, 23);
            this.displayStyle.TabIndex = 2;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(489, 10);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 7;
            this.labelSeason.Text = "Season";
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "seasonid", true));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(567, 10);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(100, 23);
            this.displaySeason.TabIndex = 3;
            // 
            // labelStyleName
            // 
            this.labelStyleName.Location = new System.Drawing.Point(16, 39);
            this.labelStyleName.Name = "labelStyleName";
            this.labelStyleName.Size = new System.Drawing.Size(75, 23);
            this.labelStyleName.TabIndex = 9;
            this.labelStyleName.Text = "Style Name";
            // 
            // displayStyleName
            // 
            this.displayStyleName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "stylename", true));
            this.displayStyleName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleName.Location = new System.Drawing.Point(94, 39);
            this.displayStyleName.Name = "displayStyleName";
            this.displayStyleName.Size = new System.Drawing.Size(353, 23);
            this.displayStyleName.TabIndex = 1;
            // 
            // B03
            // 
            this.ClientSize = new System.Drawing.Size(931, 594);
            this.DefaultOrder = "id,seasonid";
            this.GridAlias = "style_artwork_quot";
            this.GridUniqueKey = "styleukey,ukey,localsuppid";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "Ukey";
            this.KeyField2 = "styleukey";
            this.Name = "B03";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "B03. Local Quotation";
            this.UniqueExpress = "ID,BRANDID,SEASONID";
            this.WorkAlias = "style";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridArtworkType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelStyleName;
        private Win.UI.DisplayBox displayStyleName;
        private Win.UI.Label labelSeason;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.Label labelStyle;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.Label labelBrand;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid gridArtworkType;
    }
}
