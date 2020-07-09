using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P10_1_BatchDelete : Win.Subs.Base
    {
        private DataTable dtQuery;

        public P10_1_BatchDelete()
        {
            this.InitializeComponent();
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.QueryData();
        }

        private void QueryData()
        {
            DateTime? AddDate1, AddDate2;
            AddDate1 = this.dateAddDate.Value1;
            AddDate2 = this.dateAddDate.Value2;
            if ((MyUtility.Check.Empty(AddDate1) && MyUtility.Check.Empty(AddDate2)) &&
                MyUtility.Check.Empty(this.txtSPNo.Text) &&
                MyUtility.Check.Empty(this.txtSPNo1.Text))
            {
                MyUtility.Msg.WarningBox("search condition can't be all empty!!");
                return;
            }

            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(AddDate1))
            {
                sqlwhere += $@" and convert(date, b.adddate)  between '{Convert.ToDateTime(AddDate1).ToString("d")}' and '{Convert.ToDateTime(AddDate2).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text) && !MyUtility.Check.Empty(this.txtSPNo1.Text))
            {
                sqlwhere += $@" and b.OrderID >= '{this.txtSPNo.Text}' and b.OrderID <= '{this.txtSPNo1.Text}'";
            }
            else if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlwhere += $@" and b.OrderID like '{this.txtSPNo.Text}%'";
            }
            else if (!MyUtility.Check.Empty(this.txtSPNo1.Text))
            {
                sqlwhere += $@" and b.OrderID like '{this.txtSPNo1.Text}%'";
            }

            string sqlcmd = $@"
select 
[Selected]=0
,b.ID
,b.POID,b.MDivisionid
,[Factory] = o.FtyGroup
,o.StyleID
,b.Orderid,b.Sizecode,b.Colorid,b.Article,b.PatternPanel,b.Cutno,b.Cdate
,b.Sewinglineid,b.Item,b.SewingCell,b.Ratio,b.PrintDate,b.oldid,b.AddDate,b.EditDate
,[Remake] = iif(b.Remake=1,'Y','') 
from BundleReplacement b
left join Orders o on b.Orderid=o.ID
where 1=1
      and o.mDivisionid='{Env.User.Keyword}'
      {sqlwhere}
";
            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, out this.dtQuery))
            {
                if (this.dtQuery.Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtQuery;
            }
            else
            {
                this.ShowErr(sqlcmd, result);
            }

            this.HideWaitMessage();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchDelete.IsEditingReadOnly = false;
            this.gridBatchDelete.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchDelete)
                 .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("id", header: "ID", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("POID", header: "POID", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .Text("MDivisionid", header: "M", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(17))
                .Text("Orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("Sizecode", header: "Size", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("Colorid", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("PatternPanel", header: "PatternPanel", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Cutno", header: "Cut#", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Date("Cdate", header: "Create Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Sewinglineid", header: "Line#", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Item", header: "Item", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("SewingCell", header: "SewingCell", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Ratio", header: "Ratio", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Date("PrintDate", header: "PrintDate", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("oldid", header: "oldid", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .DateTime("AddDate", header: "Add Date", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .DateTime("EditDate", header: "Edit Date", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("Remake", header: "Remake", iseditingreadonly: true, width: Widths.AnsiChars(6));
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            this.gridBatchDelete.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DualResult result;
            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to delete these Bundle Card?");
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            string sqldelete = string.Empty;
            this.ShowWaitMessage("Data Process....");
            foreach (DataRow dr in dr2)
            {
                sqldelete += string.Format(
                    @"
delete from BundleReplacement where id = '{0}';
delete from BundleReplacement_Detail where id = '{0}';
delete from BundleReplacement_Detail_Art where id = '{0}';
delete from BundleReplacement_Detail_Allpart where id = '{0}';
delete from BundleReplacement_Detail_qty where id = '{0}';
", dr["ID"]);
            }

            if (!(result = DBProxy.Current.Execute(string.Empty, sqldelete)))
            {
                this.ShowErr(result);
                this.HideWaitMessage();
                return;
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Finish batch delete!!");
            this.QueryData();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
