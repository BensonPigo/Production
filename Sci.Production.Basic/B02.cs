using System;
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
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.btnMailTo.ForeColor = MyUtility.GetValue.Lookup("select isnull(count(ID),0) from MailTo WITH (NOLOCK) ") == "0" ? Color.Black : Color.Blue;
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
    }
}
