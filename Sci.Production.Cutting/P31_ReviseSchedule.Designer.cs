namespace Sci.Production.Cutting
{
    partial class P31_ReviseSchedule
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
            this.btnRevise = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.label1 = new System.Windows.Forms.Label();
            this.labelFty = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtCell1 = new Sci.Production.Class.TxtCell();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.dateEstCut = new Sci.Win.UI.DateBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRevise
            // 
            this.btnRevise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRevise.Location = new System.Drawing.Point(916, 422);
            this.btnRevise.Name = "btnRevise";
            this.btnRevise.Size = new System.Drawing.Size(80, 30);
            this.btnRevise.TabIndex = 16;
            this.btnRevise.Text = "Revise";
            this.btnRevise.UseVisualStyleBackColor = true;
            this.btnRevise.Click += new System.EventHandler(this.BtnRevise_Click);
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
            this.grid1.Location = new System.Drawing.Point(8, 55);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(988, 361);
            this.grid1.TabIndex = 15;
            this.grid1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Revise To";
            // 
            // labelFty
            // 
            this.labelFty.Location = new System.Drawing.Point(87, 9);
            this.labelFty.Name = "labelFty";
            this.labelFty.Size = new System.Drawing.Size(57, 23);
            this.labelFty.TabIndex = 6;
            this.labelFty.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(259, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Est. Cut Date";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(483, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Cut Cell";
            // 
            // txtCell1
            // 
            this.txtCell1.BackColor = System.Drawing.Color.White;
            this.txtCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell1.Location = new System.Drawing.Point(543, 9);
            this.txtCell1.MDivisionID = "";
            this.txtCell1.Name = "txtCell1";
            this.txtCell1.Size = new System.Drawing.Size(30, 23);
            this.txtCell1.TabIndex = 9;
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(147, 9);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(109, 23);
            this.displayFactory.TabIndex = 11;
            // 
            // dateEstCut
            // 
            this.dateEstCut.Location = new System.Drawing.Point(350, 9);
            this.dateEstCut.Name = "dateEstCut";
            this.dateEstCut.Size = new System.Drawing.Size(130, 23);
            this.dateEstCut.TabIndex = 1;
            this.dateEstCut.Validating += new System.ComponentModel.CancelEventHandler(this.DateEstCut_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(12, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(948, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "* Once [Revise] is selected and [Est. Cut Date] or [Cut Cell] is modified, The sy" +
    "stem will automatically assign the [SCHDL Seq] and send it to the WMS.";
            // 
            // P31_ReviseSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 461);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dateEstCut);
            this.Controls.Add(this.displayFactory);
            this.Controls.Add(this.txtCell1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelFty);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.btnRevise);
            this.EditMode = true;
            this.Name = "P31_ReviseSchedule";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P31. Revise Schedule";
            this.Controls.SetChildIndex(this.btnRevise, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.labelFty, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtCell1, 0);
            this.Controls.SetChildIndex(this.displayFactory, 0);
            this.Controls.SetChildIndex(this.dateEstCut, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Button btnRevise;
        private Win.UI.Grid grid1;
        private System.Windows.Forms.Label label1;
        private Win.UI.Label labelFty;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Class.TxtCell txtCell1;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.DateBox dateEstCut;
        private System.Windows.Forms.Label label4;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}