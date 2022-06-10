
namespace Sci.Production.Centralized
{
    partial class PPIC_B03
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.labFactory = new Sci.Win.UI.Label();
            this.gridMailto = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnBatchImport = new Sci.Win.UI.Button();
            this.gridIcon1 = new Sci.Win.UI.GridIcon();
            this.txtFactory = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMailto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(814, 257);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtFactory);
            this.detailcont.Controls.Add(this.gridIcon1);
            this.detailcont.Controls.Add(this.btnBatchImport);
            this.detailcont.Controls.Add(this.gridMailto);
            this.detailcont.Controls.Add(this.labFactory);
            this.detailcont.Size = new System.Drawing.Size(814, 219);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 219);
            this.detailbtm.Size = new System.Drawing.Size(814, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(814, 257);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(822, 286);
            // 
            // labFactory
            // 
            this.labFactory.BackColor = System.Drawing.Color.Transparent;
            this.labFactory.Location = new System.Drawing.Point(10, 10);
            this.labFactory.Name = "labFactory";
            this.labFactory.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labFactory.Size = new System.Drawing.Size(109, 23);
            this.labFactory.TabIndex = 3;
            this.labFactory.Text = "Factory";
            this.labFactory.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // gridMailto
            // 
            this.gridMailto.AllowUserToAddRows = false;
            this.gridMailto.AllowUserToDeleteRows = false;
            this.gridMailto.AllowUserToResizeRows = false;
            this.gridMailto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridMailto.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMailto.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMailto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMailto.DataSource = this.listControlBindingSource1;
            this.gridMailto.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMailto.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMailto.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMailto.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMailto.Location = new System.Drawing.Point(8, 39);
            this.gridMailto.Name = "gridMailto";
            this.gridMailto.RowHeadersVisible = false;
            this.gridMailto.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMailto.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMailto.RowTemplate.Height = 24;
            this.gridMailto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMailto.ShowCellToolTips = false;
            this.gridMailto.Size = new System.Drawing.Size(798, 174);
            this.gridMailto.TabIndex = 5;
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnBatchImport.Location = new System.Drawing.Point(562, 3);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(138, 30);
            this.btnBatchImport.TabIndex = 6;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.BtnBatchImport_Click);
            // 
            // gridIcon1
            // 
            this.gridIcon1.Location = new System.Drawing.Point(706, 3);
            this.gridIcon1.Name = "gridIcon1";
            this.gridIcon1.Size = new System.Drawing.Size(100, 32);
            this.gridIcon1.TabIndex = 7;
            this.gridIcon1.Text = "gridIcon1";
            this.gridIcon1.AppendClick += new System.EventHandler(this.GridIcon1_AppendClick);
            this.gridIcon1.InsertClick += new System.EventHandler(this.GridIcon1_InsertClick);
            this.gridIcon1.RemoveClick += new System.EventHandler(this.GridIcon1_RemoveClick);
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(122, 10);
            this.txtFactory.MaxLength = 10;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(106, 23);
            this.txtFactory.TabIndex = 14;
            this.txtFactory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFactory_PopUp);
            this.txtFactory.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFactory_Validating);
            // 
            // PPIC_B03
            // 
            this.ClientSize = new System.Drawing.Size(822, 319);
            this.ConnectionName = "ProductionTPE";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "PPIC_B03";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B03. Exception Notification";
            this.WorkAlias = "MailGroup";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMailto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Sci.Win.UI.Label labFactory;
        private Win.UI.GridIcon gridIcon1;
        private Win.UI.Button btnBatchImport;
        private Win.UI.Grid gridMailto;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtFactory;
    }
}
