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
    public partial class P36_ModifyAfterReceived : Sci.Win.Subs.Base
    {
        DataRow dr;
        DataTable dtData;
        public P36_ModifyAfterReceived(DataRow Data)
        {
            InitializeComponent();
            DualResult result;
            dr = Data;
            if (!(result = DBProxy.Current.Select(null, string.Format(@"select * from localdebit where id = '{0}'", dr["id"]), out dtData)))
            {
                ShowErr(result);
                return;
            }
            mtbs.DataSource = dtData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
