
namespace Sci.Production.Cutting
{
    partial class P31_Import
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
            this.dateEstCut = new Sci.Win.UI.DateBox();
            this.txtCell1 = new Sci.Production.Class.TxtCell();
            this.label3 = new Sci.Win.UI.Label();
            this.labEstCut = new Sci.Win.UI.Label();
            this.txtCutPlanID = new Sci.Production.Class.TxtCell();
            this.labCutPlanID = new Sci.Win.UI.Label();
            this.txtOrderID = new Sci.Production.Class.TxtCell();
            this.labOrderID = new Sci.Win.UI.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dateEstCut
            // 
            this.dateEstCut.IsSupportEditMode = false;
            this.dateEstCut.Location = new System.Drawing.Point(112, 18);
            this.dateEstCut.Name = "dateEstCut";
            this.dateEstCut.Size = new System.Drawing.Size(130, 23);
            this.dateEstCut.TabIndex = 10;
            // 
            // txtCell1
            // 
            this.txtCell1.BackColor = System.Drawing.Color.White;
            this.txtCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell1.IsSupportEditMode = false;
            this.txtCell1.Location = new System.Drawing.Point(305, 18);
            this.txtCell1.MDivisionID = "";
            this.txtCell1.Name = "txtCell1";
            this.txtCell1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtCell1.Size = new System.Drawing.Size(30, 23);
            this.txtCell1.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(245, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "Cut Cell";
            // 
            // labEstCut
            // 
            this.labEstCut.Location = new System.Drawing.Point(21, 18);
            this.labEstCut.Name = "labEstCut";
            this.labEstCut.Size = new System.Drawing.Size(88, 23);
            this.labEstCut.TabIndex = 11;
            this.labEstCut.Text = "Est. Cut Date";
            // 
            // txtCutPlanID
            // 
            this.txtCutPlanID.BackColor = System.Drawing.Color.White;
            this.txtCutPlanID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutPlanID.IsSupportEditMode = false;
            this.txtCutPlanID.Location = new System.Drawing.Point(431, 18);
            this.txtCutPlanID.MDivisionID = "";
            this.txtCutPlanID.Name = "txtCutPlanID";
            this.txtCutPlanID.Size = new System.Drawing.Size(100, 23);
            this.txtCutPlanID.TabIndex = 15;
            // 
            // labCutPlanID
            // 
            this.labCutPlanID.Location = new System.Drawing.Point(350, 18);
            this.labCutPlanID.Name = "labCutPlanID";
            this.labCutPlanID.Size = new System.Drawing.Size(78, 23);
            this.labCutPlanID.TabIndex = 14;
            this.labCutPlanID.Text = "CutPlan#";
            // 
            // txtOrderID
            // 
            this.txtOrderID.BackColor = System.Drawing.Color.White;
            this.txtOrderID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderID.IsSupportEditMode = false;
            this.txtOrderID.Location = new System.Drawing.Point(594, 18);
            this.txtOrderID.MDivisionID = "";
            this.txtOrderID.Name = "txtOrderID";
            this.txtOrderID.Size = new System.Drawing.Size(120, 23);
            this.txtOrderID.TabIndex = 17;
            // 
            // labOrderID
            // 
            this.labOrderID.Location = new System.Drawing.Point(534, 18);
            this.labOrderID.Name = "labOrderID";
            this.labOrderID.Size = new System.Drawing.Size(57, 23);
            this.labOrderID.TabIndex = 16;
            this.labOrderID.Text = "SP#";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(720, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(94, 32);
            this.btnFind.TabIndex = 18;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(720, 50);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(94, 32);
            this.btnImport.TabIndex = 19;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(720, 88);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 32);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(21, 51);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(693, 377);
            this.grid.TabIndex = 21;
            // 
            // P31_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 450);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.txtOrderID);
            this.Controls.Add(this.labOrderID);
            this.Controls.Add(this.txtCutPlanID);
            this.Controls.Add(this.labCutPlanID);
            this.Controls.Add(this.dateEstCut);
            this.Controls.Add(this.txtCell1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labEstCut);
            this.Name = "P31_Import";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P31.Import From WorkOrder For Planning";
            this.Controls.SetChildIndex(this.labEstCut, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtCell1, 0);
            this.Controls.SetChildIndex(this.dateEstCut, 0);
            this.Controls.SetChildIndex(this.labCutPlanID, 0);
            this.Controls.SetChildIndex(this.txtCutPlanID, 0);
            this.Controls.SetChildIndex(this.labOrderID, 0);
            this.Controls.SetChildIndex(this.txtOrderID, 0);
            this.Controls.SetChildIndex(this.btnFind, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateBox dateEstCut;
        private Class.TxtCell txtCell1;
        private Win.UI.Label label3;
        private Win.UI.Label labEstCut;
        private Class.TxtCell txtCutPlanID;
        private Win.UI.Label labCutPlanID;
        private Class.TxtCell txtOrderID;
        private Win.UI.Label labOrderID;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnClose;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource listControlBindingSource;
    }
}