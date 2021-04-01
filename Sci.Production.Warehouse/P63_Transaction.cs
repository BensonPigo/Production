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
            this.displayRefno.Text = this.drMain["Refno"].ToString();
            this.displayDesc.Text = this.drMain["Description"].ToString();
            this.displayInQty.Text = this.drMain["InQty"].ToString();
            this.displayOutQty.Text = this.drMain["OutQty"].ToString();
            this.displayBalQty.Text = this.drMain["Balance"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridLeft)
            .Text("Roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Text("Dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
            .Numeric("InQty", header: "In Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("OutQty", header: "Out Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("AdjustQty", header: "Adjust Qty", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4))
            .Numeric("Balance", header: "Balance", iseditingreadonly: true, decimal_places: 2, width: Widths.AnsiChars(4));

            this.Helper.Controls.Grid.Generator(this.gridRight)
            .Text("IssueDate", header: "Date", iseditingreadonly: true, width: Widths.AnsiChars(13))
            .Text("ID", header: "Transaction ID", iseditingreadonly: true, width: Widths.AnsiChars(16))
            .Text("Name", header: "Name", iseditingreadonly: true, width: Widths.AnsiChars(20))
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
        InQty,
        OutQty,
        AdjustQty,
        [Balance] = InQty - OutQty + AdjustQty
from    SemiFinishedInventory with (nolock)
where   POID = '{this.drMain["POID"]}' and
        Refno = '{this.drMain["Refno"]}' and
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
            sfrd.Refno,
            sfrd.Roll,
            sfrd.Dyelot,
            sfrd.StockType
    from    SemiFinishedReceiving sfr with (nolock)
    inner   join  SemiFinishedReceiving_Detail sfrd with (nolock) on sfr.ID = sfrd.ID
    where   sfrd.POID = '{this.drMain["POID"]}' and
            sfrd.Refno = '{this.drMain["Refno"]}' and
            sfrd.StockType = '{this.drMain["StockType"]}'
    union all
    select  sfi.IssueDate,
            sfi.ID,
            [Name] = 'P65. Issue semi-finished material',
            [InQty] = 0,
            [OutQty] = sfid.Qty,
            [AdjustQty] = 0,
            [Location] = '',
            sfid.POID,
            sfid.Refno,
            sfid.Roll,
            sfid.Dyelot,
            sfid.StockType
    from    SemiFinishedIssue sfi with (nolock)
    inner   join  SemiFinishedIssue_Detail sfid with (nolock) on sfi.ID = sfid.ID
    where   sfid.POID = '{this.drMain["POID"]}' and
            sfid.Refno = '{this.drMain["Refno"]}' and
            sfid.StockType = '{this.drMain["StockType"]}'
    union all
    select  sfa.IssueDate,
            sfa.ID,
            [Name] = 'P66. Adjust Bulk Qty (Semi-finished)',
            [InQty] = 0,
            [OutQty] = 0,
            [AdjustQty] = sfad.QtyAfter - sfad.QtyBefore,
            [Location] = '',
            sfad.POID,
            sfad.Refno,
            sfad.Roll,
            sfad.Dyelot,
            sfad.StockType
    from    SemiFinishedAdjust sfa with (nolock)
    inner   join  SemiFinishedAdjust_Detail sfad with (nolock) on sfa.ID = sfad.ID
    where   sfad.POID = '{this.drMain["POID"]}' and
            sfad.Refno = '{this.drMain["Refno"]}' and
            sfad.StockType = '{this.drMain["StockType"]}'
) a

select  *,
        [Balance] = SUM(InQty - OutQty + AdjustQty) OVER (PARTITION BY POID, Refno, Roll, Dyelot, StockType ORDER BY IssueDate,ID)
from    #tmpDetail

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
        }

        private void GridLeft_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridLeft.SelectedRows.Count == 0)
            {
                return;
            }

            string roll = this.gridLeft.SelectedRows[0].Cells["Roll"].Value.ToString();
            string dyelot = this.gridLeft.SelectedRows[0].Cells["Dyelot"].Value.ToString();
            var srcRight = this.dtRight.AsEnumerable()
                                        .Where(s => s["Roll"].ToString() == roll && s["Dyelot"].ToString() == dyelot)
                                        .OrderBy(s => s["IssueDate"]);

            if (!srcRight.Any())
            {
                return;
            }

            this.gridRight.DataSource = srcRight.CopyToDataTable();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
