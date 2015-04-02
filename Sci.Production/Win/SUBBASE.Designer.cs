﻿namespace Sci.Win
{
    partial class SUBBASE
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
            this.grid = new Sci.Win.UI.Grid();
            this.chk_yes = new Sci.Win.UI.Button();
            this.chk_no = new Sci.Win.UI.Button();
            this.gridbs = new Sci.Win.UI.ListControlBindingSource();
            this.get = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoGenerateColumns = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.gridbs;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(54, 53);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(380, 196);
            this.grid.TabIndex = 0;
            this.grid.TabStop = false;
            // 
            // chk_yes
            // 
            this.chk_yes.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.chk_yes.Location = new System.Drawing.Point(54, 286);
            this.chk_yes.Name = "chk_yes";
            this.chk_yes.Size = new System.Drawing.Size(80, 30);
            this.chk_yes.TabIndex = 1;
            this.chk_yes.Text = "yes";
            this.chk_yes.UseVisualStyleBackColor = true;
            // 
            // chk_no
            // 
            this.chk_no.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.chk_no.Location = new System.Drawing.Point(153, 286);
            this.chk_no.Name = "chk_no";
            this.chk_no.Size = new System.Drawing.Size(80, 30);
            this.chk_no.TabIndex = 1;
            this.chk_no.Text = "no";
            this.chk_no.UseVisualStyleBackColor = true;
            // 
            // get
            // 
            this.get.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.get.Location = new System.Drawing.Point(273, 286);
            this.get.Name = "get";
            this.get.Size = new System.Drawing.Size(80, 30);
            this.get.TabIndex = 1;
            this.get.Text = "get";
            this.get.UseVisualStyleBackColor = true;
            // 
            // SUBBASE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 395);
            this.Controls.Add(this.get);
            this.Controls.Add(this.chk_no);
            this.Controls.Add(this.chk_yes);
            this.Controls.Add(this.grid);
            this.Name = "SUBBASE";
            this.Text = "SUBBASE";
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Grid grid;
        private UI.Button chk_yes;
        private UI.Button chk_no;
        private UI.ListControlBindingSource gridbs;
        private UI.Button get;
    }
}