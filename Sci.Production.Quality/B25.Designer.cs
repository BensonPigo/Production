
namespace Sci.Production.Quality
{
    partial class B25
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioInspected = new Sci.Win.UI.RadioButton();
            this.radioAQL = new Sci.Win.UI.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.numInspectRate = new Sci.Win.UI.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInspectRate)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(792, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.numInspectRate);
            this.detailcont.Controls.Add(this.radioPanel1);
            this.detailcont.Controls.Add(this.comboDropDownList1);
            this.detailcont.Controls.Add(this.checkBox1);
            this.detailcont.Controls.Add(this.comboBox1);
            this.detailcont.Controls.Add(this.displayBox2);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(792, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(40, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type Name";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(40, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Material Type";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(40, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Inspection Qty";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(153, 28);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(195, 23);
            this.displayBox1.TabIndex = 4;
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FullName", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(153, 57);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(195, 23);
            this.displayBox2.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboBox1.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(153, 86);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.OldText = "";
            this.comboBox1.ReadOnly = true;
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 6;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "junk", true));
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkBox1.IsSupportEditMode = false;
            this.checkBox1.Location = new System.Drawing.Point(481, 28);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.ReadOnly = true;
            this.checkBox1.Size = new System.Drawing.Size(57, 21);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Junk";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = false;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "AQL_InspectionLevels", true));
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(249, 145);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 9;
            this.comboDropDownList1.Type = "PMS_QA_AQL";
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioAQL);
            this.radioPanel1.Controls.Add(this.radioInspected);
            this.radioPanel1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "AOS_InspQtyOption", true));
            this.radioPanel1.Location = new System.Drawing.Point(153, 116);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(96, 54);
            this.radioPanel1.TabIndex = 10;
            // 
            // radioInspected
            // 
            this.radioInspected.AutoSize = true;
            this.radioInspected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioInspected.Location = new System.Drawing.Point(3, 3);
            this.radioInspected.Name = "radioInspected";
            this.radioInspected.Size = new System.Drawing.Size(87, 21);
            this.radioInspected.TabIndex = 0;
            this.radioInspected.TabStop = true;
            this.radioInspected.Text = "Inspected";
            this.radioInspected.UseVisualStyleBackColor = true;
            this.radioInspected.Value = "1";
            // 
            // radioAQL
            // 
            this.radioAQL.AutoSize = true;
            this.radioAQL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAQL.Location = new System.Drawing.Point(3, 30);
            this.radioAQL.Name = "radioAQL";
            this.radioAQL.Size = new System.Drawing.Size(54, 21);
            this.radioAQL.TabIndex = 1;
            this.radioAQL.TabStop = true;
            this.radioAQL.Text = "AQL";
            this.radioAQL.UseVisualStyleBackColor = true;
            this.radioAQL.Value = "2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 119);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "%";
            // 
            // numInspectRate
            // 
            this.numInspectRate.BackColor = System.Drawing.Color.White;
            this.numInspectRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InspectedPercentage", true));
            this.numInspectRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numInspectRate.Location = new System.Drawing.Point(249, 117);
            this.numInspectRate.Name = "numInspectRate";
            this.numInspectRate.Size = new System.Drawing.Size(93, 23);
            this.numInspectRate.TabIndex = 18;
            this.numInspectRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // B25
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DefaultFilter = "Type = \'A\'";
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B25";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B25. Accessory Material Type Setting";
            this.WorkAlias = "QAMtlTypeSetting";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInspectRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.DisplayBox displayBox1;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioAQL;
        private Win.UI.RadioButton radioInspected;
        private System.Windows.Forms.Label label6;
        private Win.UI.NumericUpDown numInspectRate;
    }
}