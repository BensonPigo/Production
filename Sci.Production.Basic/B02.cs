using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Sci.Production.Basic
{
    /// <summary>
    /// B02
    /// </summary>
    public partial class B02 : Win.Tems.Input7
    {
        /// <summary>
        /// B02
        /// </summary>
        /// <param name="menuitem"> menuitem</param>
        public B02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            DataTable dtPMS_FabricQRCode_LabelSize;
            DataTable dtPDA_FabricQRCode_LabelSize;

            DualResult result = DBProxy.Current.Select(null, "select ID, Name from dropdownlist where Type = 'PMS_Fab_LabSize' order by Seq", out dtPMS_FabricQRCode_LabelSize);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            result = DBProxy.Current.Select(null, "select ID, Name from dropdownlist where Type = 'PMS_PDA_Fab_LabSize' order by Seq", out dtPDA_FabricQRCode_LabelSize);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.comboPDA_FabricQRCode_LabelSize.DisplayMember = "Name";
            this.comboPDA_FabricQRCode_LabelSize.ValueMember = "ID";
            this.comboPDA_FabricQRCode_LabelSize.DataSource = dtPDA_FabricQRCode_LabelSize;

            this.comboPMS_FabricQRCode_LabelSize.DisplayMember = "Name";
            this.comboPMS_FabricQRCode_LabelSize.ValueMember = "ID";
            this.comboPMS_FabricQRCode_LabelSize.DataSource = dtPMS_FabricQRCode_LabelSize;
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.btnMailTo.ForeColor = MyUtility.GetValue.Lookup("select isnull(count(ID),0) from MailTo WITH (NOLOCK) ") == "0" ? Color.Black : Color.Blue;
            MyUtility.Check.Seek("SELECT SpreadingMachine_Target FROM SYSTEM", out DataRow dr, "ManufacturingExecution");
            this.numSpreadingMachine_Target.Value = MyUtility.Convert.GetInt(dr["SpreadingMachine_Target"]);
        }

        // 取Sketch目錄的路徑
        private void BtnSketchFilesPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtSketchFilesPath.Text = dir;
            }
        }

        // 取Clip目錄的路徑
        private void BtnCilpFilesPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtCilpFilesPath.Text = dir;
            }
        }

        private string GetDir()
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            return path.SelectedPath;
        }

        // Mail To
        private void BtnMailTo_Click(object sender, EventArgs e)
        {
            B02_MailTo callNextForm = new B02_MailTo(this.IsSupportEdit, null, null, null);
            callNextForm.ShowDialog(this);
        }

        /// <summary>
        /// ClickEditAfter
        /// </summary>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtUserPOApproved.TextBox1.Select();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            DualResult result = DBProxy.Current.Execute("ManufacturingExecution", $"UPDATE SYSTEM SET SpreadingMachine_Target = {this.numSpreadingMachine_Target.Value}");
            if (!result)
            {
                return result;
            }

            return base.ClickSavePost();
        }

        // 取Pic Files Path 路徑
        private void BtnPicFilesPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtPicFilesPath.Text = dir;
            }
        }

        private void BtnMarkerInputPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtMarkerInputPath.Text = dir;
            }
        }

        private void BtnMarkerOutputPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtMarkerOutputPath.Text = dir;
            }
        }

        private void BtnReplacementReport_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtReplacementReport.Text = dir;
            }
        }

        private void BtnHandoverATPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtHandoverATPath.Text = dir;
            }
        }

        private void BtnHandoverSpecialToolsPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtHandoverSpecialToolsPath.Text = dir;
            }
        }

        private void BtnCriticalOperationPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtCriticalOperationPath.Text = dir;
            }
        }

        private void BtnFinalPatternPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtFinalPatternPath.Text = dir;
            }
        }

        private void BtnPadPrintPath_Click(object sender, EventArgs e)
        {
            string dir = this.GetDir();
            if (!MyUtility.Check.Empty(dir))
            {
                this.txtPadPrintPath.Text = dir;
            }
        }
    }
}
