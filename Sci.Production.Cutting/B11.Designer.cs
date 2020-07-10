namespace Sci.Production.Cutting
{
    partial class B11
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(B11));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtID = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.txtseason1 = new Sci.Production.Class.Txtseason();
            this.txtstyle1 = new Sci.Production.Class.Txtstyle();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.pictureBoxdown = new Sci.Win.UI.PictureBox();
            this.pictureBoxup = new Sci.Win.UI.PictureBox();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnCopy = new Sci.Win.UI.Button();
            this.btnBatchCreate = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxdown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(903, 352);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.btnCopy);
            this.detailcont.Controls.Add(this.pictureBoxdown);
            this.detailcont.Controls.Add(this.pictureBoxup);
            this.detailcont.Controls.Add(this.grid1);
            this.detailcont.Controls.Add(this.txtbrand1);
            this.detailcont.Controls.Add(this.txtseason1);
            this.detailcont.Controls.Add(this.txtstyle1);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(903, 314);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 314);
            this.detailbtm.Size = new System.Drawing.Size(903, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(903, 352);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(911, 381);
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.Tabs_SelectedIndexChanged);
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
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(93, 13);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 22);
            this.txtID.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "ID";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(93, 42);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(708, 22);
            this.txtDescription.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Description";
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(236, 15);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 8;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(407, 13);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(96, 23);
            this.txtbrand1.TabIndex = 17;
            // 
            // txtseason1
            // 
            this.txtseason1.BackColor = System.Drawing.Color.White;
            this.txtseason1.BrandObjectName = null;
            this.txtseason1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SeasonID", true));
            this.txtseason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason1.Location = new System.Drawing.Point(262, 13);
            this.txtseason1.Name = "txtseason1";
            this.txtseason1.Size = new System.Drawing.Size(80, 23);
            this.txtseason1.TabIndex = 16;
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "StyleID", true));
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(67, 13);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 15;
            this.txtstyle1.TarBrand = null;
            this.txtstyle1.TarSeason = null;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(345, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 23);
            this.label5.TabIndex = 14;
            this.label5.Text = "Brand";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(200, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Season";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "Style#";
            // 
            // pictureBoxdown
            // 
            this.pictureBoxdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxdown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxdown.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxdown.Image")));
            this.pictureBoxdown.InitialImage = global::Sci.Production.Cutting.Properties.Resources.trffc152;
            this.pictureBoxdown.Location = new System.Drawing.Point(870, 202);
            this.pictureBoxdown.Name = "pictureBoxdown";
            this.pictureBoxdown.Size = new System.Drawing.Size(25, 31);
            this.pictureBoxdown.TabIndex = 40;
            this.pictureBoxdown.TabStop = false;
            this.pictureBoxdown.WaitOnLoad = true;
            this.pictureBoxdown.Click += new System.EventHandler(this.PictureBoxdown_Click);
            // 
            // pictureBoxup
            // 
            this.pictureBoxup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxup.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxup.Image")));
            this.pictureBoxup.InitialImage = global::Sci.Production.Cutting.Properties.Resources.trffc15;
            this.pictureBoxup.Location = new System.Drawing.Point(870, 105);
            this.pictureBoxup.Name = "pictureBoxup";
            this.pictureBoxup.Size = new System.Drawing.Size(25, 31);
            this.pictureBoxup.TabIndex = 39;
            this.pictureBoxup.TabStop = false;
            this.pictureBoxup.WaitOnLoad = true;
            this.pictureBoxup.Click += new System.EventHandler(this.PictureBoxup_Click);
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
            this.grid1.IsSupportCancelSorting = true;
            this.grid1.Location = new System.Drawing.Point(5, 69);
            this.grid1.MultiSelect = false;
            this.grid1.Name = "grid1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(859, 239);
            this.grid1.TabIndex = 38;
            // 
            // btnCopy
            // 
            this.btnCopy.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCopy.Location = new System.Drawing.Point(648, 33);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(216, 30);
            this.btnCopy.TabIndex = 41;
            this.btnCopy.Text = "Copy From Std. Sequence";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // btnBatchCreate
            // 
            this.btnBatchCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchCreate.Location = new System.Drawing.Point(773, 3);
            this.btnBatchCreate.Name = "btnBatchCreate";
            this.btnBatchCreate.Size = new System.Drawing.Size(126, 30);
            this.btnBatchCreate.TabIndex = 3;
            this.btnBatchCreate.Text = "Batch Create";
            this.btnBatchCreate.UseVisualStyleBackColor = true;
            this.btnBatchCreate.Click += new System.EventHandler(this.BtnBatchCreate_Click);
            // 
            // B11
            // 
            this.ClientSize = new System.Drawing.Size(911, 414);
            this.Controls.Add(this.btnBatchCreate);
            this.IsSupportClip = false;
            this.IsSupportPrint = false;
            this.Name = "B11";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B11. SubProcess Sequence By Style";
            this.WorkAlias = "SubProcessSeq";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchCreate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxdown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox chkJunk;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtDescription;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtID;
        private Class.Txtbrand txtbrand1;
        private Class.Txtseason txtseason1;
        private Class.Txtstyle txtstyle1;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.PictureBox pictureBoxdown;
        private Win.UI.PictureBox pictureBoxup;
        private Win.UI.Grid grid1;
        private Win.UI.Button btnCopy;
        private Win.UI.Button btnBatchCreate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
