namespace Sci.Production.Cutting
{
    partial class P21_QueryRevised
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
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.btnDelete = new Sci.Win.UI.Button();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCutCell_e = new Sci.Win.UI.TextBox();
            this.txtCutCell_s = new Sci.Win.UI.TextBox();
            this.txtCutRefNo_e = new Sci.Win.UI.TextBox();
            this.txtCutRefNo_s = new Sci.Win.UI.TextBox();
            this.txtSpNo = new Sci.Win.UI.TextBox();
            this.dateCutOutput = new Sci.Win.UI.DateRange();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.panelBottom = new Sci.Win.UI.Panel();
            this.panelFill = new Sci.Win.UI.Panel();
            this.gridP21Query = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panelTop.SuspendLayout();
            this.panelFill.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridP21Query)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.txtfactory);
            this.panelTop.Controls.Add(this.btnDelete);
            this.panelTop.Controls.Add(this.btnUpdate);
            this.panelTop.Controls.Add(this.btnQuery);
            this.panelTop.Controls.Add(this.label7);
            this.panelTop.Controls.Add(this.label6);
            this.panelTop.Controls.Add(this.txtCutCell_e);
            this.panelTop.Controls.Add(this.txtCutCell_s);
            this.panelTop.Controls.Add(this.txtCutRefNo_e);
            this.panelTop.Controls.Add(this.txtCutRefNo_s);
            this.panelTop.Controls.Add(this.txtSpNo);
            this.panelTop.Controls.Add(this.dateCutOutput);
            this.panelTop.Controls.Add(this.label5);
            this.panelTop.Controls.Add(this.label4);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1217, 121);
            this.panelTop.TabIndex = 1;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(75, 44);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 17;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(1125, 80);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(1125, 45);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.TabIndex = 15;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(1125, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 14;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(587, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "~";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(335, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "~";
            // 
            // txtCutCell_e
            // 
            this.txtCutCell_e.BackColor = System.Drawing.Color.White;
            this.txtCutCell_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell_e.Location = new System.Drawing.Point(357, 44);
            this.txtCutCell_e.Name = "txtCutCell_e";
            this.txtCutCell_e.Size = new System.Drawing.Size(60, 23);
            this.txtCutCell_e.TabIndex = 11;
            // 
            // txtCutCell_s
            // 
            this.txtCutCell_s.BackColor = System.Drawing.Color.White;
            this.txtCutCell_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell_s.Location = new System.Drawing.Point(269, 44);
            this.txtCutCell_s.Name = "txtCutCell_s";
            this.txtCutCell_s.Size = new System.Drawing.Size(60, 23);
            this.txtCutCell_s.TabIndex = 10;
            // 
            // txtCutRefNo_e
            // 
            this.txtCutRefNo_e.BackColor = System.Drawing.Color.White;
            this.txtCutRefNo_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNo_e.Location = new System.Drawing.Point(609, 9);
            this.txtCutRefNo_e.Name = "txtCutRefNo_e";
            this.txtCutRefNo_e.Size = new System.Drawing.Size(70, 23);
            this.txtCutRefNo_e.TabIndex = 9;
            // 
            // txtCutRefNo_s
            // 
            this.txtCutRefNo_s.BackColor = System.Drawing.Color.White;
            this.txtCutRefNo_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNo_s.Location = new System.Drawing.Point(511, 9);
            this.txtCutRefNo_s.Name = "txtCutRefNo_s";
            this.txtCutRefNo_s.Size = new System.Drawing.Size(70, 23);
            this.txtCutRefNo_s.TabIndex = 8;
            // 
            // txtSpNo
            // 
            this.txtSpNo.BackColor = System.Drawing.Color.White;
            this.txtSpNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpNo.Location = new System.Drawing.Point(511, 41);
            this.txtSpNo.Name = "txtSpNo";
            this.txtSpNo.Size = new System.Drawing.Size(168, 23);
            this.txtSpNo.TabIndex = 7;
            // 
            // dateCutOutput
            // 
            // 
            // 
            // 
            this.dateCutOutput.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCutOutput.DateBox1.Name = "";
            this.dateCutOutput.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCutOutput.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCutOutput.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCutOutput.DateBox2.Name = "";
            this.dateCutOutput.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCutOutput.DateBox2.TabIndex = 1;
            this.dateCutOutput.IsRequired = false;
            this.dateCutOutput.Location = new System.Drawing.Point(147, 9);
            this.dateCutOutput.Name = "dateCutOutput";
            this.dateCutOutput.Size = new System.Drawing.Size(280, 23);
            this.dateCutOutput.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(446, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "CutRef#";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(446, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "SP#";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(204, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Cut Cell";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Factory";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cutting Output Date";
            // 
            // panelBottom
            // 
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 528);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1217, 2);
            this.panelBottom.TabIndex = 2;
            // 
            // panelFill
            // 
            this.panelFill.Controls.Add(this.gridP21Query);
            this.panelFill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFill.Location = new System.Drawing.Point(0, 121);
            this.panelFill.Name = "panelFill";
            this.panelFill.Size = new System.Drawing.Size(1217, 407);
            this.panelFill.TabIndex = 3;
            // 
            // gridP21Query
            // 
            this.gridP21Query.AllowUserToAddRows = false;
            this.gridP21Query.AllowUserToDeleteRows = false;
            this.gridP21Query.AllowUserToResizeRows = false;
            this.gridP21Query.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridP21Query.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridP21Query.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridP21Query.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridP21Query.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridP21Query.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridP21Query.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridP21Query.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridP21Query.Location = new System.Drawing.Point(0, 0);
            this.gridP21Query.Name = "gridP21Query";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridP21Query.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridP21Query.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridP21Query.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridP21Query.RowTemplate.Height = 24;
            this.gridP21Query.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridP21Query.ShowCellToolTips = false;
            this.gridP21Query.Size = new System.Drawing.Size(1217, 407);
            this.gridP21Query.TabIndex = 0;
            // 
            // P21_QueryRevised
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 530);
            this.Controls.Add(this.panelFill);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "P21_QueryRevised";
            this.Text = "P21.Query/Revised Cutting Output Fabric Record Data";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelFill.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridP21Query)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panelTop;
        private Win.UI.Button btnDelete;
        private Win.UI.Button btnUpdate;
        private Win.UI.Button btnQuery;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private Win.UI.TextBox txtCutCell_e;
        private Win.UI.TextBox txtCutCell_s;
        private Win.UI.TextBox txtCutRefNo_e;
        private Win.UI.TextBox txtCutRefNo_s;
        private Win.UI.TextBox txtSpNo;
        private Win.UI.DateRange dateCutOutput;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Panel panelBottom;
        private Win.UI.Panel panelFill;
        private Class.Txtfactory txtfactory;
        private Win.UI.Grid gridP21Query;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}