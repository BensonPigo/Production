using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;

namespace Sci.Production.Subcon
{
    public partial class P36_ModifyAfterSent : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable dtData;
        public P36_ModifyAfterSent(DataRow Data)
        {
            InitializeComponent();
            DualResult result;
            dr = Data;
            if (!(result = DBProxy.Current.Select(null, string.Format(@"select * from localdebit WITH (NOLOCK) where id = '{0}'", dr["id"]), out dtData)))
            {
                ShowErr(result);
                return;
            }
            mtbs.DataSource = dtData;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Currency =USD, Exchange 只能為1
            if (dr["currencyid"].ToString().ToUpper() == "USD" && numExchange.Value != 1)
            {
                MyUtility.Msg.WarningBox("If the currency is USD, then exchange must be 1 !!");
                return ;
            }

            if (!MyUtility.Tool.CursorUpdateTable(dtData, "localdebit", null))
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
