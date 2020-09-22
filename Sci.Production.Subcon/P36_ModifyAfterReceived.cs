using System;
using System.Data;
using Ict;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P36_ModifyAfterReceived : Win.Subs.Base
    {
        private DataRow dr;
        private DataTable dtData;

        public P36_ModifyAfterReceived(DataRow data)
        {
            this.InitializeComponent();
            DualResult result;
            this.dr = data;
            if (!(result = DBProxy.Current.Select(null, string.Format(@"select * from localdebit WITH (NOLOCK) where id = '{0}'", this.dr["id"]), out this.dtData)))
            {
                this.ShowErr(result);
                return;
            }

            this.mtbs.DataSource = this.dtData;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!MyUtility.Tool.CursorUpdateTable(this.dtData, "localdebit", null))
            {
                MyUtility.Msg.WarningBox("Save failed!");
            }
            else
            {
                this.Close();
            }
        }
    }
}
