namespace Sci.Production.Tools
{
    partial class Notification
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
            this.cmbMenu = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label2 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Size = new System.Drawing.Size(539, 40);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 45);
            this.gridcont.Size = new System.Drawing.Size(515, 402);
            // 
            // append
            // 
            this.append.Size = new System.Drawing.Size(0, 30);
            // 
            // revise
            // 
            this.revise.Location = new System.Drawing.Point(10, 5);
            this.revise.Size = new System.Drawing.Size(0, 30);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(10, 5);
            this.delete.Size = new System.Drawing.Size(0, 30);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(449, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(369, 5);
            // 
            // cmbMenu
            // 
            this.cmbMenu._Type = Sci.Production.Class.ComboDropDownList.ComboDropDownList_Type._None;
            this.cmbMenu.AddAllItem = false;
            this.cmbMenu.AddEmpty = false;
            this.cmbMenu.BackColor = System.Drawing.Color.White;
            this.cmbMenu.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cmbMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbMenu.FormattingEnabled = true;
            this.cmbMenu.IsSupportUnselect = true;
            this.cmbMenu.Location = new System.Drawing.Point(61, 11);
            this.cmbMenu.Name = "cmbMenu";
            this.cmbMenu.OldText = "";
            this.cmbMenu.Size = new System.Drawing.Size(139, 24);
            this.cmbMenu.TabIndex = 98;
            this.cmbMenu.Type = "";
            this.cmbMenu.SelectedIndexChanged += new System.EventHandler(this.CmbModule_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 23);
            this.label2.TabIndex = 99;
            this.label2.Text = "Filter";
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 497);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbMenu);
            this.GridPopUp = false;
            this.Name = "Notification";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Notification";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.cmbMenu, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Class.ComboDropDownList cmbMenu;
        private Win.UI.Label label2;
    }
}