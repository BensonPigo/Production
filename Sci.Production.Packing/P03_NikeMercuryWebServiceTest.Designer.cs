namespace Sci.Production.Class
{
    partial class P03_NikeMercuryWebServiceTest
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
            this.editBoxRequestXml = new Sci.Win.UI.EditBox();
            this.splitContainerXml = new System.Windows.Forms.SplitContainer();
            this.editBoxResponseXml = new Sci.Win.UI.EditBox();
            this.btnLabelsPackPlanCreate = new Sci.Win.UI.Button();
            this.btnLabelsPackPlanCartonAdd = new Sci.Win.UI.Button();
            this.btnLabelsPackPlanDelete = new Sci.Win.UI.Button();
            this.btnLabelsPackPlanCartonUpdate = new Sci.Win.UI.Button();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.txtOrderInfo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.btnTestDeserialize = new Sci.Win.UI.Button();
            this.label3 = new Sci.Win.UI.Label();
            this.txtCartonNumber = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerXml)).BeginInit();
            this.splitContainerXml.Panel1.SuspendLayout();
            this.splitContainerXml.Panel2.SuspendLayout();
            this.splitContainerXml.SuspendLayout();
            this.SuspendLayout();
            // 
            // editBoxRequestXml
            // 
            this.editBoxRequestXml.BackColor = System.Drawing.Color.White;
            this.editBoxRequestXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editBoxRequestXml.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxRequestXml.Location = new System.Drawing.Point(0, 0);
            this.editBoxRequestXml.Multiline = true;
            this.editBoxRequestXml.Name = "editBoxRequestXml";
            this.editBoxRequestXml.Size = new System.Drawing.Size(429, 373);
            this.editBoxRequestXml.TabIndex = 1;
            // 
            // splitContainerXml
            // 
            this.splitContainerXml.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerXml.Location = new System.Drawing.Point(12, 140);
            this.splitContainerXml.Name = "splitContainerXml";
            // 
            // splitContainerXml.Panel1
            // 
            this.splitContainerXml.Panel1.Controls.Add(this.editBoxRequestXml);
            // 
            // splitContainerXml.Panel2
            // 
            this.splitContainerXml.Panel2.Controls.Add(this.editBoxResponseXml);
            this.splitContainerXml.Size = new System.Drawing.Size(887, 373);
            this.splitContainerXml.SplitterDistance = 429;
            this.splitContainerXml.TabIndex = 2;
            // 
            // editBoxResponseXml
            // 
            this.editBoxResponseXml.BackColor = System.Drawing.Color.White;
            this.editBoxResponseXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editBoxResponseXml.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxResponseXml.Location = new System.Drawing.Point(0, 0);
            this.editBoxResponseXml.Multiline = true;
            this.editBoxResponseXml.Name = "editBoxResponseXml";
            this.editBoxResponseXml.Size = new System.Drawing.Size(454, 373);
            this.editBoxResponseXml.TabIndex = 2;
            // 
            // btnLabelsPackPlanCreate
            // 
            this.btnLabelsPackPlanCreate.Location = new System.Drawing.Point(12, 68);
            this.btnLabelsPackPlanCreate.Name = "btnLabelsPackPlanCreate";
            this.btnLabelsPackPlanCreate.Size = new System.Drawing.Size(198, 30);
            this.btnLabelsPackPlanCreate.TabIndex = 3;
            this.btnLabelsPackPlanCreate.Text = "LabelsPackPlanCreate";
            this.btnLabelsPackPlanCreate.UseVisualStyleBackColor = true;
            this.btnLabelsPackPlanCreate.Click += new System.EventHandler(this.BtnLabelsPackPlanCreate_Click);
            // 
            // btnLabelsPackPlanCartonAdd
            // 
            this.btnLabelsPackPlanCartonAdd.Location = new System.Drawing.Point(12, 104);
            this.btnLabelsPackPlanCartonAdd.Name = "btnLabelsPackPlanCartonAdd";
            this.btnLabelsPackPlanCartonAdd.Size = new System.Drawing.Size(198, 30);
            this.btnLabelsPackPlanCartonAdd.TabIndex = 4;
            this.btnLabelsPackPlanCartonAdd.Text = "LabelsPackPlanCartonAdd";
            this.btnLabelsPackPlanCartonAdd.UseVisualStyleBackColor = true;
            this.btnLabelsPackPlanCartonAdd.Click += new System.EventHandler(this.BtnLabelsPackPlanCartonAdd_Click);
            // 
            // btnLabelsPackPlanDelete
            // 
            this.btnLabelsPackPlanDelete.Location = new System.Drawing.Point(216, 68);
            this.btnLabelsPackPlanDelete.Name = "btnLabelsPackPlanDelete";
            this.btnLabelsPackPlanDelete.Size = new System.Drawing.Size(212, 30);
            this.btnLabelsPackPlanDelete.TabIndex = 5;
            this.btnLabelsPackPlanDelete.Text = "LabelsPackPlanDelete";
            this.btnLabelsPackPlanDelete.UseVisualStyleBackColor = true;
            this.btnLabelsPackPlanDelete.Click += new System.EventHandler(this.BtnLabelsPackPlanDelete_Click);
            // 
            // btnLabelsPackPlanCartonUpdate
            // 
            this.btnLabelsPackPlanCartonUpdate.Location = new System.Drawing.Point(216, 104);
            this.btnLabelsPackPlanCartonUpdate.Name = "btnLabelsPackPlanCartonUpdate";
            this.btnLabelsPackPlanCartonUpdate.Size = new System.Drawing.Size(212, 30);
            this.btnLabelsPackPlanCartonUpdate.TabIndex = 6;
            this.btnLabelsPackPlanCartonUpdate.Text = "LabelsPackPlanCartonUpdate";
            this.btnLabelsPackPlanCartonUpdate.UseVisualStyleBackColor = true;
            this.btnLabelsPackPlanCartonUpdate.Click += new System.EventHandler(this.BtnLabelsPackPlanCartonUpdate_Click);
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(96, 9);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(100, 23);
            this.txtPackID.TabIndex = 7;
            // 
            // txtOrderInfo
            // 
            this.txtOrderInfo.BackColor = System.Drawing.Color.White;
            this.txtOrderInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderInfo.Location = new System.Drawing.Point(96, 39);
            this.txtOrderInfo.Name = "txtOrderInfo";
            this.txtOrderInfo.Size = new System.Drawing.Size(219, 23);
            this.txtOrderInfo.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "PackID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Order";
            // 
            // btnTestDeserialize
            // 
            this.btnTestDeserialize.Location = new System.Drawing.Point(649, 5);
            this.btnTestDeserialize.Name = "btnTestDeserialize";
            this.btnTestDeserialize.Size = new System.Drawing.Size(137, 30);
            this.btnTestDeserialize.TabIndex = 11;
            this.btnTestDeserialize.Text = "Deserialize";
            this.btnTestDeserialize.UseVisualStyleBackColor = true;
            this.btnTestDeserialize.Click += new System.EventHandler(this.BtnTestDeserialize_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(430, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "CartonNumber";
            // 
            // txtCartonNumber
            // 
            this.txtCartonNumber.BackColor = System.Drawing.Color.White;
            this.txtCartonNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCartonNumber.Location = new System.Drawing.Point(534, 108);
            this.txtCartonNumber.Name = "txtCartonNumber";
            this.txtCartonNumber.Size = new System.Drawing.Size(219, 23);
            this.txtCartonNumber.TabIndex = 12;
            // 
            // P03_NikeMercuryWebServiceTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 525);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCartonNumber);
            this.Controls.Add(this.btnTestDeserialize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOrderInfo);
            this.Controls.Add(this.txtPackID);
            this.Controls.Add(this.btnLabelsPackPlanCartonUpdate);
            this.Controls.Add(this.btnLabelsPackPlanDelete);
            this.Controls.Add(this.btnLabelsPackPlanCartonAdd);
            this.Controls.Add(this.btnLabelsPackPlanCreate);
            this.Controls.Add(this.splitContainerXml);
            this.Name = "P03_NikeMercuryWebServiceTest";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "NikeMercuryWebServiceTest";
            this.Controls.SetChildIndex(this.splitContainerXml, 0);
            this.Controls.SetChildIndex(this.btnLabelsPackPlanCreate, 0);
            this.Controls.SetChildIndex(this.btnLabelsPackPlanCartonAdd, 0);
            this.Controls.SetChildIndex(this.btnLabelsPackPlanDelete, 0);
            this.Controls.SetChildIndex(this.btnLabelsPackPlanCartonUpdate, 0);
            this.Controls.SetChildIndex(this.txtPackID, 0);
            this.Controls.SetChildIndex(this.txtOrderInfo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnTestDeserialize, 0);
            this.Controls.SetChildIndex(this.txtCartonNumber, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.splitContainerXml.Panel1.ResumeLayout(false);
            this.splitContainerXml.Panel1.PerformLayout();
            this.splitContainerXml.Panel2.ResumeLayout(false);
            this.splitContainerXml.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerXml)).EndInit();
            this.splitContainerXml.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.EditBox editBoxRequestXml;
        private System.Windows.Forms.SplitContainer splitContainerXml;
        private Win.UI.EditBox editBoxResponseXml;
        private Win.UI.Button btnLabelsPackPlanCreate;
        private Win.UI.Button btnLabelsPackPlanCartonAdd;
        private Win.UI.Button btnLabelsPackPlanDelete;
        private Win.UI.Button btnLabelsPackPlanCartonUpdate;
        private Win.UI.TextBox txtPackID;
        private Win.UI.TextBox txtOrderInfo;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Button btnTestDeserialize;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtCartonNumber;
    }
}