using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.PPIC
{
    public partial class P07 : Sci.Win.Tems.QueryForm
    {
        string useAPS;
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            string sqlCommand = "select UseAPS from factory where ID = '" + Sci.Env.User.Factory + "'";
            useAPS = MyUtility.GetValue.Lookup(sqlCommand, null);
            if (useAPS.ToUpper() == "FALSE")
            {
                MyUtility.Msg.WarningBox("Not yet use the APS, so can't use this function!!");
                button1.Enabled = false;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (useAPS.ToUpper() == "FALSE")
            {
                this.Close();
            }
        }

        //Download
        private void button1_Click(object sender, EventArgs e)
        {
            DataRow dr;
            MyUtility.Check.Seek(string.Format("select SQLServerName,APSDatabaseName from MDivision where ID = '{0}'", Sci.Env.User.Keyword), out dr);
            if (MyUtility.Check.Empty(dr["SQLServerName"]) || MyUtility.Check.Empty(dr["APSDatabaseName"]))
            {
                MyUtility.Msg.WarningBox("Still not yet set APS Server data, Please contact Taipei MIS. Thank you.");
                return;
            }
            string sqlCmd = string.Format("exec dbo.usp_APSDataDownLoad '{0}','{1}','{2}','{3}'", MyUtility.Convert.GetString(dr["SQLServerName"]), MyUtility.Convert.GetString(dr["APSDatabaseName"]), Sci.Env.User.Factory, Sci.Env.User.UserID);
            DualResult Result = DBProxy.Current.Execute(null, sqlCmd);
            if (!Result)
            {
                ShowErr(sqlCmd, Result);
                return;
            }
        }

        //Close
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
