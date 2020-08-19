namespace Sci.Production.Shipping
{
    partial class P13
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.btnFind = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(229, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 23);
            this.label6.TabIndex = 109;
            this.label6.Text = "～";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(253, 12);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(130, 23);
            this.txtSP2.TabIndex = 1;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(98, 12);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(130, 23);
            this.txtSP1.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(14, 12);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(80, 23);
            this.labelSPNo.TabIndex = 106;
            this.labelSPNo.Text = "SP#";
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(866, 12);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(66, 23);
            this.txtbrand1.TabIndex = 110;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(782, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 111;
            this.label1.Text = "Brand";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(397, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 112;
            this.label2.Text = "Buyer Delivery";
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(499, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 113;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(938, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 114;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(938, 48);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 116;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
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
            this.grid1.Location = new System.Drawing.Point(14, 84);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(1001, 300);
            this.grid1.TabIndex = 117;
            // 
            // P13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 396);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.labelSPNo);
            this.Name = "P13";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P13. Query for Finished FOC Stock-in Entry";
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.btnFind, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label6;
        private Win.UI.TextBox txtSP2;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label labelSPNo;
        private Class.Txtbrand txtbrand1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnClose;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}