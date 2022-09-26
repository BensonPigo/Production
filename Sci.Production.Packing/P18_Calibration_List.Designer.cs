namespace Sci.Production.Packing
{
    partial class P18_Calibration_List
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labMDMachine = new Sci.Win.UI.Label();
            this.comboMDMachineID = new System.Windows.Forms.ComboBox();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnClose);
            this.btmcont.Location = new System.Drawing.Point(0, 487);
            this.btmcont.Size = new System.Drawing.Size(805, 40);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.btnClose, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 59);
            this.gridcont.Size = new System.Drawing.Size(781, 418);
            // 
            // revise
            // 
            this.revise.Enabled = false;
            this.revise.Visible = false;
            // 
            // undo
            // 
            this.undo.Dock = System.Windows.Forms.DockStyle.None;
            this.undo.Location = new System.Drawing.Point(269, 5);
            this.undo.Size = new System.Drawing.Size(72, 30);
            this.undo.Visible = false;
            // 
            // save
            // 
            this.save.Dock = System.Windows.Forms.DockStyle.None;
            this.save.Location = new System.Drawing.Point(623, 5);
            this.save.Size = new System.Drawing.Size(84, 30);
            this.save.Text = "Submit";
            // 
            // labMDMachine
            // 
            this.labMDMachine.Location = new System.Drawing.Point(33, 20);
            this.labMDMachine.Name = "labMDMachine";
            this.labMDMachine.Size = new System.Drawing.Size(96, 23);
            this.labMDMachine.TabIndex = 150;
            this.labMDMachine.Text = "MD Machine#";
            // 
            // comboMDMachineID
            // 
            this.comboMDMachineID.FormattingEnabled = true;
            this.comboMDMachineID.Location = new System.Drawing.Point(132, 19);
            this.comboMDMachineID.Name = "comboMDMachineID";
            this.comboMDMachineID.Size = new System.Drawing.Size(166, 24);
            this.comboMDMachineID.TabIndex = 151;
            this.comboMDMachineID.SelectedValueChanged += new System.EventHandler(this.comboMDMachineID_SelectedValueChanged);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(713, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 125;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // P18_Calibration_List
            // 
            this.ClientSize = new System.Drawing.Size(805, 527);
            this.Controls.Add(this.comboMDMachineID);
            this.Controls.Add(this.labMDMachine);
            this.Name = "P18_Calibration_List";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Calibration List";
            this.WorkAlias = "MDCalibrationList ";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labMDMachine, 0);
            this.Controls.SetChildIndex(this.comboMDMachineID, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label labMDMachine;
        private System.Windows.Forms.ComboBox comboMDMachineID;
        private Win.UI.Button btnClose;
    }
}
