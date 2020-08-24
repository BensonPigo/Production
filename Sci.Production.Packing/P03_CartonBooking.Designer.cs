using Sci.Production.Class;

namespace Sci.Production.Packing
{
    partial class P03_CartonBooking
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
            this.lbCartonEstBooking = new Sci.Win.UI.Label();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.lbCartonEstArrived = new Sci.Win.UI.Label();
            this.dateBoxCartonEstBooking = new Sci.Win.UI.DateBox();
            this.dateBoxCartonEstArrived = new Sci.Win.UI.DateBox();
            this.SuspendLayout();
            // 
            // lbCartonEstBooking
            // 
            this.lbCartonEstBooking.Location = new System.Drawing.Point(12, 5);
            this.lbCartonEstBooking.Name = "lbCartonEstBooking";
            this.lbCartonEstBooking.Size = new System.Drawing.Size(139, 23);
            this.lbCartonEstBooking.TabIndex = 2;
            this.lbCartonEstBooking.Text = "Carton Est. Booking";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(120, 66);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(206, 66);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 30;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lbCartonEstArrived
            // 
            this.lbCartonEstArrived.Location = new System.Drawing.Point(12, 37);
            this.lbCartonEstArrived.Name = "lbCartonEstArrived";
            this.lbCartonEstArrived.Size = new System.Drawing.Size(139, 23);
            this.lbCartonEstArrived.TabIndex = 31;
            this.lbCartonEstArrived.Text = "Carton Est. Arrived";
            // 
            // dateBoxCartonEstBooking
            // 
            this.dateBoxCartonEstBooking.Location = new System.Drawing.Point(154, 5);
            this.dateBoxCartonEstBooking.Name = "dateBoxCartonEstBooking";
            this.dateBoxCartonEstBooking.Size = new System.Drawing.Size(134, 23);
            this.dateBoxCartonEstBooking.TabIndex = 32;
            // 
            // dateBoxCartonEstArrived
            // 
            this.dateBoxCartonEstArrived.Location = new System.Drawing.Point(154, 37);
            this.dateBoxCartonEstArrived.Name = "dateBoxCartonEstArrived";
            this.dateBoxCartonEstArrived.Size = new System.Drawing.Size(134, 23);
            this.dateBoxCartonEstArrived.TabIndex = 33;
            // 
            // P03_CartonBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(307, 107);
            this.Controls.Add(this.dateBoxCartonEstArrived);
            this.Controls.Add(this.dateBoxCartonEstBooking);
            this.Controls.Add(this.lbCartonEstArrived);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lbCartonEstBooking);
            this.Name = "P03_CartonBooking";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Carton Booking";
            this.Controls.SetChildIndex(this.lbCartonEstBooking, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.lbCartonEstArrived, 0);
            this.Controls.SetChildIndex(this.dateBoxCartonEstBooking, 0);
            this.Controls.SetChildIndex(this.dateBoxCartonEstArrived, 0);
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Label lbCartonEstBooking;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.Label lbCartonEstArrived;
        private Win.UI.DateBox dateBoxCartonEstBooking;
        private Win.UI.DateBox dateBoxCartonEstArrived;
    }
}