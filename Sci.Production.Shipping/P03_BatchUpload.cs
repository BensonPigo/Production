using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;

namespace Sci.Production.Shipping
{
    public partial class P03_BatchUpload : Win.Subs.Base
    {
        private DataTable dtQuery;

        public P03_BatchUpload()
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridUpload.DataSource = this.listControlBindingSource1;
            this.gridUpload.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridUpload)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("ID", header: "WK No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("InvNo", header: "Invoice#", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .Date("Eta", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Blno", header: "B/L No.", width: Widths.AnsiChars(18), iseditingreadonly: true)
            .Date("PortArrival", header: "Arrive Port Date", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Date("DocArrival", header: "Dox Rcv Date", width: Widths.AnsiChars(25), iseditingreadonly: true);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                if (MyUtility.Check.Empty(this.dateArrivePortDate.Value))
                {
                    item["PortArrival"] = DBNull.Value;
                }
                else
                {
                    item["PortArrival"] = this.dateArrivePortDate.Value;
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                if (MyUtility.Check.Empty(this.datedocRcvDate.Value))
                {
                    item["DocArrival"] = DBNull.Value;
                }
                else
                {
                    item["DocArrival"] = this.datedocRcvDate.Value;
                }
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select [selected] = 0 ,*
from Export
where junk = 0 and (PortArrival is null or DocArrival is null)");

            if (!MyUtility.Check.Empty(this.txtWKNo1.Text))
            {
                sqlCmd.Append($@" and id >= '{this.txtWKNo1.Text}'");
            }

            if (!MyUtility.Check.Empty(this.txtWKNo2.Text))
            {
                sqlCmd.Append($@" and id <= '{this.txtWKNo2.Text}'");
            }

            if (!MyUtility.Check.Empty(this.dateETA.Value1))
            {
                sqlCmd.Append($@" and Eta >= '{((DateTime)this.dateETA.Value1).ToString("yyyy/MM/dd")}'");
            }

            if (!MyUtility.Check.Empty(this.dateETA.Value2))
            {
                sqlCmd.Append($@" and Eta <= '{((DateTime)this.dateETA.Value2).ToString("yyyy/MM/dd")}'");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.dtQuery);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error:" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.dtQuery;
            if (this.dtQuery.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found! ");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.gridUpload.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            StringBuilder warningmsg = new StringBuilder();
            IList<string> updateCmds = new List<string>();
            this.ShowWaitMessage("Data Processing...");
            foreach (DataRow dr in selectedData)
            {
                // 到港日不可晚於到W/H日期
                if (!MyUtility.Check.Empty(dr["PortArrival"]) && !MyUtility.Check.Empty(dr["WhseArrival"]))
                {
                    if (Convert.ToDateTime(dr["PortArrival"]) > Convert.ToDateTime(dr["WhseArrival"]))
                    {
                        warningmsg.Append($@"WK#: {dr["ID"]} < Arrive Port Date > can't later than < Arrive W/H Date >." + Environment.NewLine);
                        continue;
                    }
                }

                if (!MyUtility.Check.Empty(dr["PortArrival"]) && !MyUtility.Check.Empty(dr["Eta"]))
                {
                    if (DateTime.Compare(Convert.ToDateTime(dr["PortArrival"]).AddDays(10), (DateTime)dr["Eta"]) < 0 ||
                        DateTime.Compare(Convert.ToDateTime(dr["PortArrival"]).AddDays(-10), (DateTime)dr["Eta"]) > 0)
                    {
                        warningmsg.Append($@"WK#: {dr["ID"]} < Arrive Port Date > earlier or later more than <ETA> 10 days, Cannot be saved." + Environment.NewLine);
                        continue;
                    }
                }

                string strPortArrival = MyUtility.Check.Empty(dr["PortArrival"]) ? "null" : "'" + ((DateTime)dr["PortArrival"]).ToString("yyyy/MM/dd") + "'";
                string strDocArrival = MyUtility.Check.Empty(dr["DocArrival"]) ? "null" : "'" + ((DateTime)dr["DocArrival"]).ToString("yyyy/MM/dd") + "'";
                updateCmds.Add($@"
                update Export 
                set PortArrival = {strPortArrival}
                ,DocArrival = {strDocArrival}
                where id='{dr["ID"]}'");
            }

            DualResult result = Ict.Result.True;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (updateCmds.Count > 0)
                    {
                        result = DBProxy.Current.Executes(null, updateCmds);
                        if (result)
                        {
                            transactionScope.Complete();
                            transactionScope.Dispose();
                            MyUtility.Msg.InfoBox("Complete!!");
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("Save failed, Pleaes re-try");
                            this.HideWaitMessage();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    this.HideWaitMessage();
                    return;
                }
            }

            this.Query();
            this.HideWaitMessage();
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
