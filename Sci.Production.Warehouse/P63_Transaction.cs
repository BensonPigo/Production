using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P63_Transaction : Win.Subs.Base
    {
        private readonly DataRow drMain;
        private DataTable dtRight;

        /// <inheritdoc/>
        public P63_Transaction(DataRow data)
        {
            this.InitializeComponent();
            this.drMain = data;
            this.displaySeq.Text = this.drMain["Seq"].ToString();
            this.displayDesc.Text = this.drMain["Desc"].ToString();
            this.displayInQty.Text = this.drMain["InQty"].ToString();
            this.displayOutQty.Text = this.drMain["OutQty"].ToString();
            this.displayBalQty.Text = this.drMain["Balance"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region 開窗
            DataGridViewGeneratorTextColumnSettings openOtherWH = new DataGridViewGeneratorTextColumnSettings();
            openOtherWH.CellMouseDoubleClick += (s, e) =>
            {
                var dr = this.gridDetail.CurrentDataRow;
                if (dr == null)
                {
                    return;
                }

                switch (dr["name"].ToString().Substring(0, 3))
                {
                    case "P64":
                        var p64 = new P64(null, dr["id"].ToString());
                        p64.ShowDialog(this);
                        break;
                    case "P65":
                        var p65 = new P65(null, dr["id"].ToString());
                        p65.ShowDialog(this);
                        break;
                    case "P66":
                        var p66 = new P66(null, dr["id"].ToString());
                        p66.ShowDialog(this);
                        break;
                }
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .Date("IssueDate", header: "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("ID", header: "Transaction#", width: Widths.AnsiChars(14), iseditingreadonly: true, settings: openOtherWH)
            .Text("Name", header: "Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Numeric("OutQty", header: "Released Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2)
            .Text("Location", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(30), iseditingreadonly: true)
            ;

            this.Helper.Controls.Grid.Generator(this.gridLeft)
            .Text("Roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("Dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("Tone", header: "Tone/Grp", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Numeric("InQty", header: "In Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("OutQty", header: "Out Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("Balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4));

            this.Helper.Controls.Grid.Generator(this.gridRight)
            .Text("IssueDate", header: "Date", iseditingreadonly: true, width: Widths.AnsiChars(9))
            .Text("ID", header: "Transaction ID", iseditingreadonly: true, width: Widths.AnsiChars(14), settings: openOtherWH)
            .Text("Name", header: "Name", iseditingreadonly: true, width: Widths.AnsiChars(30))
            .Numeric("InQty", header: "In Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("OutQty", header: "Out Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("Balance", header: "Bal. Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Text("Location", header: "Bulk Location", iseditingreadonly: true);

            this.Query();
        }

        private void Query()
        {
            string sqlQuery = $@"
select  Roll,
        Dyelot,
        Tone,
        InQty,
        OutQty,
        AdjustQty,
        [Balance] = InQty - OutQty + AdjustQty
from    SemiFinishedInventory with (nolock)
where   POID = '{this.drMain["POID"]}' and
        Seq = '{this.drMain["Seq"]}' and
        StockType = '{this.drMain["StockType"]}'

select  *   
into    #tmpDetail
from (
    select  sfr.IssueDate,
            sfr.ID,
            [Name] = 'P64. Material Receiving (Semi-finished)',
            [InQty] = sfrd.Qty,
            [OutQty] = 0,
            [AdjustQty] = 0,
            sfrd.Location,
            sfrd.POID,
            sfrd.Seq,
            sfrd.Roll,
            sfrd.Dyelot,
            sfrd.Tone,
            sfrd.StockType,
            sfr.Remark
    from    SemiFinishedReceiving sfr with (nolock)
    inner   join  SemiFinishedReceiving_Detail sfrd with (nolock) on sfr.ID = sfrd.ID
    where   sfrd.POID = '{this.drMain["POID"]}' and
            sfrd.Seq = '{this.drMain["Seq"]}' and
            sfrd.StockType = '{this.drMain["StockType"]}' and
            sfr.Status = 'Confirmed'
    union all
    select  sfi.IssueDate,
            sfi.ID,
            [Name] = 'P65. Issue semi-finished material',
            [InQty] = 0,
            [OutQty] = sfid.Qty,
            [AdjustQty] = 0,
            [Location] = '',
            sfid.POID,
            sfid.Seq,
            sfid.Roll,
            sfid.Dyelot,
            sfid.Tone,
            sfid.StockType,
            sfi.Remark
    from    SemiFinishedIssue sfi with (nolock)
    inner   join  SemiFinishedIssue_Detail sfid with (nolock) on sfi.ID = sfid.ID
    where   sfid.POID = '{this.drMain["POID"]}' and
            sfid.Seq = '{this.drMain["Seq"]}' and
            sfid.StockType = '{this.drMain["StockType"]}' and
            sfi.Status = 'Confirmed'
    union all
    select  sfa.IssueDate,
            sfa.ID,
            [Name] = 'P66. Adjust Bulk Qty (Semi-finished)',
            [InQty] = 0,
            [OutQty] = 0,
            [AdjustQty] = sfad.QtyAfter - sfad.QtyBefore,
            [Location] = '',
            sfad.POID,
            sfad.Seq,
            sfad.Roll,
            sfad.Dyelot,
            sfad.Tone,
            sfad.StockType,
            sfa.Remark
    from    SemiFinishedAdjust sfa with (nolock)
    inner   join  SemiFinishedAdjust_Detail sfad with (nolock) on sfa.ID = sfad.ID
    where   sfad.POID = '{this.drMain["POID"]}' and
            sfad.Seq = '{this.drMain["Seq"]}' and
            sfad.StockType = '{this.drMain["StockType"]}' and
            sfa.Status = 'Confirmed'
) a

select  *,
        [Balance] = SUM(InQty - OutQty + AdjustQty) OVER (PARTITION BY POID, Seq, Roll, Dyelot, Tone, StockType ORDER BY IssueDate,ID)
from    #tmpDetail


select IssueDate, ID, Name,
    InQty = sum(InQty),
    OutQty = sum(OutQty),
    AdjustQty = sum(AdjustQty),
    Location,
    Remark,
    POID
into #tmpDetailSum
from #tmpDetail
group by IssueDate, ID, Name, Location, Remark, POID

select *,
        [Balance] = SUM(InQty - OutQty + AdjustQty) OVER (ORDER BY IssueDate,ID)
from #tmpDetailSum

select IssueDate='', ID='', Name='Total',InQty = sum(InQty),OutQty = sum(OutQty), AdjustQty = sum(AdjustQty), Balance = sum(InQty) - sum(OutQty) + sum(AdjustQty)
from #tmpDetail


drop table #tmpDetail
";
            DataTable[] dtResults;
            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResults);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.dtRight = dtResults[1];
            this.gridLeft.DataSource = dtResults[0];
            this.gridDetail.DataSource = dtResults[2];
            if (dtResults[3].Rows.Count > 0)
            {
                this.dispTotalArrivedQty.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["InQty"]).ToString();
                this.dispTotalReleasedQty.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["OutQty"]).ToString();
                this.dispTotalAdjustQty.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["AdjustQty"]).ToString();
                this.dispTotalBalance.Text = MyUtility.Convert.GetDecimal(dtResults[3].Rows[0]["Balance"]).ToString();

                this.displayInQty.Text = this.dispTotalArrivedQty.Text;
                this.displayOutQty.Text = this.dispTotalReleasedQty.Text;
                this.displayBalQty.Text = this.dispTotalBalance.Text;
            }
        }

        private void GridLeft_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridLeft.SelectedRows.Count == 0)
            {
                this.gridRight.DataSource = null;
                return;
            }

            string roll = this.gridLeft.SelectedRows[0].Cells["Roll"].Value.ToString();
            string dyelot = this.gridLeft.SelectedRows[0].Cells["Dyelot"].Value.ToString();
            string tone = this.gridLeft.SelectedRows[0].Cells["Tone"].Value.ToString();
            var srcRight = this.dtRight.AsEnumerable()
                                        .Where(s => s["Roll"].ToString() == roll && s["Dyelot"].ToString() == dyelot && s["Tone"].ToString() == tone)
                                        .OrderBy(s => s["IssueDate"]);

            if (!srcRight.Any())
            {
                this.gridRight.DataSource = null;
                return;
            }

            this.gridRight.DataSource = srcRight.CopyToDataTable();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnReCalculate_Click(object sender, EventArgs e)
        {
            string sqlcmd = $@"
select POID,Seq,Roll,Dyelot,Tone,StockType, InQty = sum(InQty), OutQty = sum(OutQty), AdjustQty = sum(AdjustQty)
into #tmp
from (
    select  sfr.IssueDate,
            sfr.ID,
            [Name] = 'P64. Material Receiving (Semi-finished)',
            [InQty] = sfrd.Qty,
            [OutQty] = 0,
            [AdjustQty] = 0,
            sfrd.Location,
            sfrd.POID,
            sfrd.Seq,
            sfrd.Roll,
            sfrd.Dyelot,
            sfrd.Tone,
            sfrd.StockType,
            sfr.Remark
    from    SemiFinishedReceiving sfr with (nolock)
    inner   join  SemiFinishedReceiving_Detail sfrd with (nolock) on sfr.ID = sfrd.ID
    where   sfrd.POID = '{this.drMain["POID"]}' and
            sfrd.Seq = '{this.drMain["Seq"]}' and
            sfrd.StockType = '{this.drMain["StockType"]}' and
            sfr.Status = 'Confirmed'
    union all
    select  sfi.IssueDate,
            sfi.ID,
            [Name] = 'P65. Issue semi-finished material',
            [InQty] = 0,
            [OutQty] = sfid.Qty,
            [AdjustQty] = 0,
            [Location] = '',
            sfid.POID,
            sfid.Seq,
            sfid.Roll,
            sfid.Dyelot,
            sfid.Tone,
            sfid.StockType,
            sfi.Remark
    from    SemiFinishedIssue sfi with (nolock)
    inner   join  SemiFinishedIssue_Detail sfid with (nolock) on sfi.ID = sfid.ID
    where   sfid.POID = '{this.drMain["POID"]}' and
            sfid.Seq = '{this.drMain["Seq"]}' and
            sfid.StockType = '{this.drMain["StockType"]}' and
            sfi.Status = 'Confirmed'
    union all
    select  sfa.IssueDate,
            sfa.ID,
            [Name] = 'P66. Adjust Bulk Qty (Semi-finished)',
            [InQty] = 0,
            [OutQty] = 0,
            [AdjustQty] = sfad.QtyAfter - sfad.QtyBefore,
            [Location] = '',
            sfad.POID,
            sfad.Seq,
            sfad.Roll,
            sfad.Dyelot,
            sfad.Tone,
            sfad.StockType,
            sfa.Remark
    from    SemiFinishedAdjust sfa with (nolock)
    inner   join  SemiFinishedAdjust_Detail sfad with (nolock) on sfa.ID = sfad.ID
    where   sfad.POID = '{this.drMain["POID"]}' and
            sfad.Seq = '{this.drMain["Seq"]}' and
            sfad.StockType = '{this.drMain["StockType"]}' and
            sfa.Status = 'Confirmed'
) a
group by POID,Seq,Roll,Dyelot,Tone,StockType

update sfi
set
    InQty = t.InQty,
    OutQty = t.OutQty,
    AdjustQty = t.AdjustQty
from SemiFinishedInventory sfi
inner join #tmp t on sfi.POID         = t.POID        and
                     sfi.Seq          = t.Seq         and
                     sfi.Roll         = t.Roll        and
                     sfi.Dyelot       = t.Dyelot      and
                     sfi.Tone         = t.Tone        and
                     sfi.StockType    = t.StockType
";

            DualResult result = DBProxy.Current.Execute(null, sqlcmd);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Finished!");
            this.Query();
        }
    }
}
