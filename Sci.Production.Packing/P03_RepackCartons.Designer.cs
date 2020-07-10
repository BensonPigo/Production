using Sci.Production.Class;

namespace Sci.Production.Packing
{
    partial class P03_RepackCartons
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridOriPack = new Sci.Production.Class.MergeRowGrid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.btnSelect = new Sci.Win.UI.Button();
            this.btnDown = new Sci.Win.UI.Button();
            this.btnUp = new Sci.Win.UI.Button();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.txtDest = new Sci.Production.Class.Txtcountry();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.txtSeq = new Sci.Win.UI.TextBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.gridNewPack = new Sci.Production.Class.MergeRowGrid();
            this.btnRepack = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.numSelectCtnFrom = new Sci.Win.UI.NumericBox();
            this.numSelectCtnTo = new Sci.Win.UI.NumericBox();
            this.txtshipmode = new Sci.Production.Class.Txtshipmode();
            this.numStartFromCtn = new Sci.Win.UI.NumericBox();
            this.btnUpdateNewCtn = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridOriPack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridNewPack)).BeginInit();
            this.SuspendLayout();
            // 
            // gridOriPack
            // 
            this.gridOriPack.AllowUserToAddRows = false;
            this.gridOriPack.AllowUserToDeleteRows = false;
            this.gridOriPack.AllowUserToResizeRows = false;
            this.gridOriPack.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridOriPack.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridOriPack.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridOriPack.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridOriPack.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridOriPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridOriPack.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridOriPack.Location = new System.Drawing.Point(12, 38);
            this.gridOriPack.Name = "gridOriPack";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridOriPack.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridOriPack.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridOriPack.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridOriPack.RowTemplate.Height = 24;
            this.gridOriPack.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOriPack.ShowCellToolTips = false;
            this.gridOriPack.Size = new System.Drawing.Size(939, 192);
            this.gridOriPack.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridOriPack.TabIndex = 1;
            this.gridOriPack.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.GridOriPack_CellPainting);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(77, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select Original CTN#";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(322, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "～";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(457, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(80, 30);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(416, 233);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(80, 30);
            this.btnDown.TabIndex = 7;
            this.btnDown.Text = "˅";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(502, 233);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(80, 30);
            this.btnUp.TabIndex = 8;
            this.btnUp.Text = "˄";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 262);
            this.label3.Name = "label3";
            this.label3.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.BorderColors.Bottom = System.Drawing.Color.Black;
            this.label3.RectStyle.BorderWidth = 1F;
            this.label3.RectStyle.BorderWidths.Bottom = 1F;
            this.label3.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label3.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.ExtBorderWidths.Bottom = 2F;
            this.label3.Size = new System.Drawing.Size(939, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "Selected Repack Cartons";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(9, 345);
            this.label4.Name = "label4";
            this.label4.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.RectStyle.BorderColors.Bottom = System.Drawing.Color.Black;
            this.label4.RectStyle.BorderWidth = 1F;
            this.label4.RectStyle.BorderWidths.Bottom = 1F;
            this.label4.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label4.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label4.RectStyle.ExtBorderWidths.Bottom = 2F;
            this.label4.Size = new System.Drawing.Size(939, 10);
            this.label4.TabIndex = 10;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 23);
            this.label6.TabIndex = 12;
            this.label6.Text = "Brand";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 321);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 23);
            this.label7.TabIndex = 13;
            this.label7.Text = "Ship Mode";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(224, 293);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 23);
            this.label8.TabIndex = 14;
            this.label8.Text = "CustCD";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(224, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 23);
            this.label9.TabIndex = 15;
            this.label9.Text = "Destination";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(544, 293);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 23);
            this.label10.TabIndex = 16;
            this.label10.Text = "Remark";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(12, 360);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(95, 23);
            this.label11.TabIndex = 17;
            this.label11.Text = "Seq";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(224, 360);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 23);
            this.label12.TabIndex = 18;
            this.label12.Text = "Start from CTN#";
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(110, 293);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(100, 23);
            this.displayBrand.TabIndex = 20;
            // 
            // txtDest
            // 
            this.txtDest.DisplayBox1Binding = "";
            this.txtDest.Location = new System.Drawing.Point(322, 321);
            this.txtDest.Name = "txtDest";
            this.txtDest.Size = new System.Drawing.Size(215, 22);
            this.txtDest.TabIndex = 21;
            this.txtDest.TextBox1Binding = "";
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(322, 293);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 22;
            // 
            // txtSeq
            // 
            this.txtSeq.BackColor = System.Drawing.Color.White;
            this.txtSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq.Location = new System.Drawing.Point(110, 360);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Size = new System.Drawing.Size(100, 23);
            this.txtSeq.TabIndex = 24;
            this.txtSeq.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSeq_PopUp);
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(658, 293);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(281, 50);
            this.editRemark.TabIndex = 27;
            // 
            // gridNewPack
            // 
            this.gridNewPack.AllowUserToAddRows = false;
            this.gridNewPack.AllowUserToDeleteRows = false;
            this.gridNewPack.AllowUserToResizeRows = false;
            this.gridNewPack.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridNewPack.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridNewPack.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridNewPack.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridNewPack.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridNewPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridNewPack.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridNewPack.Location = new System.Drawing.Point(12, 389);
            this.gridNewPack.Name = "gridNewPack";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridNewPack.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridNewPack.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridNewPack.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridNewPack.RowTemplate.Height = 24;
            this.gridNewPack.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridNewPack.ShowCellToolTips = false;
            this.gridNewPack.Size = new System.Drawing.Size(939, 204);
            this.gridNewPack.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridNewPack.TabIndex = 28;
            this.gridNewPack.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.GridOriPack_CellPainting);
            // 
            // btnRepack
            // 
            this.btnRepack.Location = new System.Drawing.Point(785, 599);
            this.btnRepack.Name = "btnRepack";
            this.btnRepack.Size = new System.Drawing.Size(80, 30);
            this.btnRepack.TabIndex = 29;
            this.btnRepack.Text = "Repack";
            this.btnRepack.UseVisualStyleBackColor = true;
            this.btnRepack.Click += new System.EventHandler(this.BtnRepack_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(871, 599);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // numSelectCtnFrom
            // 
            this.numSelectCtnFrom.BackColor = System.Drawing.Color.White;
            this.numSelectCtnFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSelectCtnFrom.Location = new System.Drawing.Point(219, 9);
            this.numSelectCtnFrom.Name = "numSelectCtnFrom";
            this.numSelectCtnFrom.NullValue = null;
            this.numSelectCtnFrom.Size = new System.Drawing.Size(100, 23);
            this.numSelectCtnFrom.TabIndex = 31;
            // 
            // numSelectCtnTo
            // 
            this.numSelectCtnTo.BackColor = System.Drawing.Color.White;
            this.numSelectCtnTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSelectCtnTo.Location = new System.Drawing.Point(347, 9);
            this.numSelectCtnTo.Name = "numSelectCtnTo";
            this.numSelectCtnTo.NullValue = null;
            this.numSelectCtnTo.Size = new System.Drawing.Size(100, 23);
            this.numSelectCtnTo.TabIndex = 32;
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(110, 319);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(100, 24);
            this.txtshipmode.TabIndex = 33;
            this.txtshipmode.UseFunction = null;
            this.txtshipmode.SelectedIndexChanged += new System.EventHandler(this.Txtshipmode_SelectedIndexChanged);
            // 
            // numStartFromCtn
            // 
            this.numStartFromCtn.BackColor = System.Drawing.Color.White;
            this.numStartFromCtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numStartFromCtn.Location = new System.Drawing.Point(322, 360);
            this.numStartFromCtn.Name = "numStartFromCtn";
            this.numStartFromCtn.NullValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numStartFromCtn.Size = new System.Drawing.Size(100, 23);
            this.numStartFromCtn.TabIndex = 34;
            this.numStartFromCtn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnUpdateNewCtn
            // 
            this.btnUpdateNewCtn.Location = new System.Drawing.Point(544, 358);
            this.btnUpdateNewCtn.Name = "btnUpdateNewCtn";
            this.btnUpdateNewCtn.Size = new System.Drawing.Size(156, 27);
            this.btnUpdateNewCtn.TabIndex = 35;
            this.btnUpdateNewCtn.Text = "Update Cartons ↓";
            this.btnUpdateNewCtn.UseVisualStyleBackColor = true;
            this.btnUpdateNewCtn.Click += new System.EventHandler(this.BtnUpdateNewCtn_Click);
            // 
            // P03_RepackCartons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 641);
            this.Controls.Add(this.btnUpdateNewCtn);
            this.Controls.Add(this.numStartFromCtn);
            this.Controls.Add(this.txtshipmode);
            this.Controls.Add(this.numSelectCtnTo);
            this.Controls.Add(this.numSelectCtnFrom);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRepack);
            this.Controls.Add(this.gridNewPack);
            this.Controls.Add(this.editRemark);
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.txtcustcd);
            this.Controls.Add(this.txtDest);
            this.Controls.Add(this.displayBrand);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridOriPack);
            this.Controls.Add(this.label4);
            this.Name = "P03_RepackCartons";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Select Repack Cartons";
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.gridOriPack, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnSelect, 0);
            this.Controls.SetChildIndex(this.btnDown, 0);
            this.Controls.SetChildIndex(this.btnUp, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.displayBrand, 0);
            this.Controls.SetChildIndex(this.txtDest, 0);
            this.Controls.SetChildIndex(this.txtcustcd, 0);
            this.Controls.SetChildIndex(this.txtSeq, 0);
            this.Controls.SetChildIndex(this.editRemark, 0);
            this.Controls.SetChildIndex(this.gridNewPack, 0);
            this.Controls.SetChildIndex(this.btnRepack, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.numSelectCtnFrom, 0);
            this.Controls.SetChildIndex(this.numSelectCtnTo, 0);
            this.Controls.SetChildIndex(this.txtshipmode, 0);
            this.Controls.SetChildIndex(this.numStartFromCtn, 0);
            this.Controls.SetChildIndex(this.btnUpdateNewCtn, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridOriPack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridNewPack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MergeRowGrid gridOriPack;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Button btnSelect;
        private Win.UI.Button btnDown;
        private Win.UI.Button btnUp;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.Label label11;
        private Win.UI.Label label12;
        private Win.UI.DisplayBox displayBrand;
        private Class.Txtcountry txtDest;
        private Class.Txtcustcd txtcustcd;
        private Win.UI.TextBox txtSeq;
        private Win.UI.EditBox editRemark;
        private MergeRowGrid gridNewPack;
        private Win.UI.Button btnRepack;
        private Win.UI.Button btnClose;
        private Win.UI.NumericBox numSelectCtnFrom;
        private Win.UI.NumericBox numSelectCtnTo;
        private Class.Txtshipmode txtshipmode;
        private Win.UI.NumericBox numStartFromCtn;
        private Win.UI.Button btnUpdateNewCtn;
    }
}