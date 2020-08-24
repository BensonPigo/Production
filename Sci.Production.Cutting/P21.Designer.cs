namespace Sci.Production.Cutting
{
    partial class P21
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTop = new Sci.Win.UI.Panel();
            this.gridIcon1 = new Sci.Win.UI.GridIcon();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.panelFill = new Sci.Win.UI.Panel();
            this.gridP21 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panelTop.SuspendLayout();
            this.panelFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridP21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.gridIcon1);
            this.panelTop.Controls.Add(this.btnSave);
            this.panelTop.Controls.Add(this.btnQuery);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(915, 48);
            this.panelTop.TabIndex = 1;
            // 
            // gridIcon1
            // 
            this.gridIcon1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gridIcon1.Location = new System.Drawing.Point(783, 9);
            this.gridIcon1.Name = "gridIcon1";
            this.gridIcon1.Size = new System.Drawing.Size(100, 32);
            this.gridIcon1.TabIndex = 2;
            this.gridIcon1.Text = "gridIcon1";
            this.gridIcon1.AppendClick += new System.EventHandler(this.GridIcon1_AppendClick);
            this.gridIcon1.InsertClick += new System.EventHandler(this.GridIcon1_InsertClick);
            this.gridIcon1.RemoveClick += new System.EventHandler(this.GridIcon1_RemoveClick);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(667, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(10, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(202, 30);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "Query/Revised Old Data";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // panelFill
            // 
            this.panelFill.Controls.Add(this.gridP21);
            this.panelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFill.Location = new System.Drawing.Point(0, 48);
            this.panelFill.Name = "panelFill";
            this.panelFill.Size = new System.Drawing.Size(915, 395);
            this.panelFill.TabIndex = 2;
            // 
            // gridP21
            // 
            this.gridP21.AllowUserToAddRows = false;
            this.gridP21.AllowUserToDeleteRows = false;
            this.gridP21.AllowUserToResizeRows = false;
            this.gridP21.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridP21.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridP21.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridP21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridP21.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridP21.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridP21.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridP21.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridP21.Location = new System.Drawing.Point(0, 0);
            this.gridP21.MultiSelect = false;
            this.gridP21.Name = "gridP21";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridP21.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridP21.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridP21.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridP21.RowTemplate.Height = 24;
            this.gridP21.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridP21.ShowCellToolTips = false;
            this.gridP21.Size = new System.Drawing.Size(915, 395);
            this.gridP21.TabIndex = 0;
            this.gridP21.EditingKeyProcessing += new System.EventHandler<Ict.Win.UI.DataGridViewEditingKeyProcessingEventArgs>(this.GridP21_EditingKeyProcessing);
            // 
            // P21
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 443);
            this.Controls.Add(this.panelFill);
            this.Controls.Add(this.panelTop);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportLocate = false;
            this.IsSupportMove = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.IsToolbarVisible = false;
            this.Name = "P21";
            this.Text = "P21. Cutting Daily Output Fabric Record";
            this.Controls.SetChildIndex(this.panelTop, 0);
            this.Controls.SetChildIndex(this.panelFill, 0);
            this.panelTop.ResumeLayout(false);
            this.panelFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridP21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panelTop;
        private Win.UI.GridIcon gridIcon1;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnQuery;
        private Win.UI.Panel panelFill;
        private Win.UI.Grid gridP21;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}