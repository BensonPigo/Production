using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P10_BatchDelete : Win.Subs.Base
    {
        private DataTable dtQuery;

        /// <inheritdoc/>
        public P10_BatchDelete()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridBatchDelete.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridBatchDelete)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("id", header: "ID", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("POID", header: "POID", iseditingreadonly: true, width: Widths.AnsiChars(12))
                .Text("MDivisionid", header: "M", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Factory", header: "Factory", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("StyleID", header: "Style", iseditingreadonly: true, width: Widths.AnsiChars(17))
                .Text("CutRef", header: "Cut Ref#", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Orderid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("Sizecode", header: "Size", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("Colorid", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Article", header: "Article", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("PatternPanel", header: "PatternPanel", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Cutno", header: "Cut#", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Date("Cdate", header: "Create Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Date("EstCutDate", header: "Est. cut date", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("Sewinglineid", header: "Line#", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Item", header: "Item", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("SewingCell", header: "SewingCell", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Ratio", header: "Ratio", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Date("PrintDate", header: "PrintDate", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("oldid", header: "oldid", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .DateTime("AddDate", header: "Add Date", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .DateTime("EditDate", header: "Edit Date", iseditingreadonly: true, width: Widths.AnsiChars(20))
                .Text("Excess", header: "Excess", iseditingreadonly: true, width: Widths.AnsiChars(6));
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void Query()
        {
            this.ShowWaitMessage("Data Loading....");
            this.QueryData();
            this.HideWaitMessage();
        }

        private void QueryData()
        {
            DateTime? addDate1 = this.dateAddDate.Value1;
            DateTime? addDate2 = this.dateAddDate.Value2;
            DateTime? estCutDate = this.dateEstCutDate.Value;
            if (MyUtility.Check.Empty(addDate1) && MyUtility.Check.Empty(addDate2) && MyUtility.Check.Empty(estCutDate) &&
                MyUtility.Check.Empty(this.txtCutRef.Text) &&
                MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtSPNo1.Text))
            {
                MyUtility.Msg.WarningBox("search condition can't be all empty!!");
                return;
            }

            string sqlwhere = string.Empty;
            string nl = Environment.NewLine;
            if (!MyUtility.Check.Empty(addDate1))
            {
                sqlwhere += nl + $@" and convert(date, b.adddate) between '{((DateTime)addDate1).ToString("d")}' and '{((DateTime)addDate2).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(estCutDate))
            {
                sqlwhere += nl + $@" and estdate.estcutdate = '{((DateTime)estCutDate).ToString("d")}'";
            }

            if (!MyUtility.Check.Empty(this.txtCutRef.Text))
            {
                sqlwhere += nl + $@" and b.CutRef = '{this.txtCutRef.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text) && !MyUtility.Check.Empty(this.txtSPNo1.Text))
            {
                sqlwhere += nl + $@" and exists(select 1 from Bundle_Detail_Order bdo WITH (NOLOCK) where bdo.ID = b.ID and bdo.OrderID >= '{this.txtSPNo.Text}' and bdo.OrderID <= '{this.txtSPNo1.Text}')";
            }
            else if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlwhere += nl + $@"and exists(select 1 from Bundle_Detail_Order bdo WITH (NOLOCK) where bdo.ID = b.ID and bdo.OrderID like '{this.txtSPNo.Text}%')";
            }
            else if (!MyUtility.Check.Empty(this.txtSPNo1.Text))
            {
                sqlwhere += nl + $@"and exists(select 1 from Bundle_Detail_Order bdo WITH (NOLOCK) where bdo.ID = b.ID and bdo.OrderID like '{this.txtSPNo1.Text}%')";
            }

            string sqlcmd = $@"
select 
    [Selected]=cast(0 as bit),
	b.ID,
	b.POID,
	b.MDivisionid,
	[Factory] = o.FtyGroup,
	o.StyleID,
	b.CutRef,
	[OrderID] = dbo.GetSinglelineSP((select distinct OrderID from Bundle_Detail_Order where id = b.id order by OrderID for XML RAW)),
	b.Sizecode,
	b.Colorid,
	b.Article,
	b.PatternPanel,
	b.Cutno,
	b.Cdate,
	[EstCutDate] = EstCutDate.EstCutDate,
	b.Sewinglineid,
	b.Item,
	b.SewingCell,
	b.Ratio,
	b.PrintDate,
	b.oldid,
	b.AddDate,
	b.EditDate,
	[Excess] = iif(b.IsExcess=1,'Y','') 
from bundle b
left join Orders o on b.Orderid=o.ID
outer apply
(
	Select EstCutDate = MAX(EstCutDate)
	from workorder w WITH (NOLOCK) 
	where w.id = b.POID 
	and w.cutref = b.CutRef
	and w.MDivisionID = b.MDivisionID
) EstCutDate
where 1=1
and o.mDivisionid='{Env.User.Keyword}'
{sqlwhere}
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtQuery);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtQuery.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            this.listControlBindingSource1.DataSource = this.dtQuery;
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
delete bundle where id = '{0}';
delete Bundle_Detail where id = '{0}';
delete Bundle_Detail_Art where id = '{0}';
delete Bundle_Detail_AllPart where id = '{0}';
delete Bundle_Detail_qty where id = '{0}';
delete Bundle_Detail_Order where id = '{0}';
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
            this.Query();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
