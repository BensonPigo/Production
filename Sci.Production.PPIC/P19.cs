using Ict;
using Sci.Data;
using System;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P19 : Win.Tems.QueryForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="P19"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateInputDate.Text = DateTime.Now.ToShortDateString();
        }

        private bool IsCanDoProcess()
        {
            string sqlCmd = "SELECT 1 FROM sysdatabases WHERE name='FPS'";

            bool isFpsEXISTS = MyUtility.Check.Seek(sqlCmd);

            if (!isFpsEXISTS)
            {
                MyUtility.Msg.InfoBox("Cannot find database [FPS], please connect to IT admin.");
                this.HideWaitMessage();
                return false;
            }

            return true;
        }

        private void ExecProcess(string execSql)
        {
            this.ShowWaitMessage("Loading....");

            DBProxy.Current.DefaultTimeout = 1200;
            DualResult result = DBProxy.Current.Execute(null, execSql);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(execSql, result);
                this.HideWaitMessage();
                return;
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Complete.");
        }

        private void BtnPMS_Click(object sender, EventArgs e)
        {
            if (!this.IsCanDoProcess())
            {
                return;
            }

            string sqlCmd = string.Empty;

            if (!DateTime.TryParse(this.dateInputDate.Text, out DateTime inputDate))
            {
                inputDate = DateTime.Now;
            }

            sqlCmd = $"exec [FPS].dbo.exp_finishingprocess '{inputDate.ToString("yyyy/MM/dd")}';";
            this.ExecProcess(sqlCmd);
        }

        private void BtnMWS_Click(object sender, EventArgs e)
        {
            if (!this.IsCanDoProcess())
            {
                return;
            }

            string sqlCmd = string.Empty;
            sqlCmd = "exec [FPS].dbo.imp_finishingprocess ;";
            this.ExecProcess(sqlCmd);
        }

        private void Btn_exp_AutoFabric_Click(object sender, EventArgs e)
        {
            if (!this.IsCanDoProcess())
            {
                return;
            }

            string sqlCmd = string.Empty;
            sqlCmd = "exec [FPS].dbo.exp_AutoFabric;";
            this.ExecProcess(sqlCmd);
        }

        private void Btn_imp_AutoFabric_Click(object sender, EventArgs e)
        {
            if (!this.IsCanDoProcess())
            {
                return;
            }

            string sqlCmd = string.Empty;
            sqlCmd = "exec [FPS].dbo.imp_AutoFabric;";
            this.ExecProcess(sqlCmd);
        }
    }
}
