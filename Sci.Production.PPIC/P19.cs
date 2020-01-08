using Ict;
using Sci.Data;
using System;
using System.Windows.Forms;

namespace Sci.Production.PPIC
{
    public partial class P19 : Sci.Win.Tems.QueryForm
    {
        public P19(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateInputDate.Text = DateTime.Now.ToShortDateString();
        }

        private void BtnPMS_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Loading....");
            string sqlCmd = "SELECT 1 FROM sysdatabases WHERE name='FPS'";

            bool isFpsEXISTS = MyUtility.Check.Seek(sqlCmd);
            DateTime inputDate;
            if (!isFpsEXISTS)
            {
                MyUtility.Msg.InfoBox("Cannot find database [FPS], please connect to IT admin.");
                this.HideWaitMessage();
                return;
            }

            if (!DateTime.TryParse(this.dateInputDate.Text, out inputDate))
            {
                inputDate = DateTime.Now;
            }

            sqlCmd = $"exec [FPS].dbo.exp_finishingprocess '{inputDate.ToString("yyyy/MM/dd")}';";
            DBProxy.Current.DefaultTimeout = 1200;
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(sqlCmd, result);
                this.HideWaitMessage();
                return;
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Complete.");
        }

        private void BtnMWS_Click(object sender, EventArgs e)
        {

            this.ShowWaitMessage("Loading....");
            string sqlCmd = "SELECT 1 FROM sysdatabases WHERE name='FPS'";

            bool isFpsEXISTS = MyUtility.Check.Seek(sqlCmd);

            if (!isFpsEXISTS)
            {
                MyUtility.Msg.InfoBox("Cannot find database [FPS], please connect to IT admin.");
                this.HideWaitMessage();
                return;
            }

            sqlCmd = "exec [FPS].dbo.imp_finishingprocess ;";
            DBProxy.Current.DefaultTimeout = 1200;
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                MyUtility.Msg.WaitClear();
                this.ShowErr(sqlCmd, result);
                this.HideWaitMessage();
                return;
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Complete.");
        }
    }
}
