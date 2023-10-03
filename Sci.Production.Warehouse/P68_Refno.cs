using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Data.SqlTypes;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P68_Refno : Win.Tems.QueryForm
    {
        private DataRow dr;

        /// <inheritdoc/>
        public P68_Refno(DataRow dr)
        {
            this.InitializeComponent();
            this.dr = dr;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
            this.Query();
            this.comboxStatus.SelectedIndex = 0;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("WKNO", header: "WK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ArriveWHDate", header: "Arrive W/H Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ID", header: "SP", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("SEQ", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("WeaveTypeID", header: "Weave Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.grid2)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("StockType", header: "Stock\r\nType", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Location", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("InQty", header: "In Qty", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                .Numeric("OutQty", header: "Out Qty", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                .Numeric("ReturnQty", header: "Return Qty", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                .Numeric("BalanceQty", header: "Balance  Qty", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                .Text("Lock", header: "Status", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Date("LockDate", header: "Lock/UnLock\r\nDate", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;
        }

        private void Query()
        {
            string sqlcmd = $@"
USE Production
SELECT
    [WKNO] = ex.ID
   ,[ArriveWHDate] = ex.WhseArrival
   ,PS.ID
   ,PSD.SEQ1
   ,PSD.SEQ2
   ,PSD.Refno
   ,PSD.Qty
   ,o.BrandID
   ,Fabric.WeaveTypeID
   ,Color = dbo.GetColorMultipleID_MtlType(psd.BrandID, ISNULL(psdsC.SpecValue, ''), Fabric.MtlTypeID, psd.SuppColor)
INTO #tmp
FROM dbo.PO_Supp_Detail PSD
JOIN dbo.PO_Supp PS ON PSD.id = PS.id  AND PSD.Seq1 = PS.Seq1
JOIN dbo.Supp S ON S.id = PS.SuppID
JOIN dbo.Orders O ON o.id = PSD.id
LEFT JOIN dbo.Fabric ON fabric.SciRefno = psd.SciRefno
LEFT JOIN PO_Supp_Detail_Spec psdsC WITH (NOLOCK) ON psdsC.ID = psd.id AND psdsC.seq1 = psd.seq1 AND psdsC.seq2 = psd.seq2 AND psdsC.SpecColumnID = 'Color'
LEFT JOIN Export_Detail exd WITH (NOLOCK) ON exd.POID = psd.id AND exd.Seq1 = psd.SEQ1 AND exd.Seq2 = psd.SEQ2
LEFT JOIN Export ex WITH (NOLOCK) ON ex.ID = exd.ID
WHERE 1 = 1
AND ps.ID = '{this.dr["POID"]}'
AND PSD.Refno = '{this.dr["Refno"]}'

SELECT *, SEQ = CONCAT(SEQ1, '-', SEQ2) FROM #tmp

SELECT
    fi.Roll
   ,fi.Dyelot
   ,StockType = Case fi.StockType
                when 'i' then 
                    'Inventory' 
                when 'b' then 
                    'Bulk' 
                when 'o' then 
                    'Scrap' 
                End
   ,[Location] = dbo.Getlocation(fi.ukey)
   ,fi.InQty
   ,fi.OutQty
   ,fi.AdjustQty
   ,fi.ReturnQty
   ,BalanceQty = fi.InQty - fi.OutQty + fi.AdjustQty - fi.ReturnQty
   ,Lock = IIF(fi.Lock = 1, 'Lock', 'UnLock')
   ,fi.LockDate
   ,fi.SEQ1
   ,fi.SEQ2
FROM #tmp t
INNER JOIN FtyInventory fi ON fi.POID = t.ID AND fi.Seq1 = t.SEQ1 AND fi.Seq2 = t.SEQ2
WHERE fi.StockType = 'B' --只看B倉

DROP TABLE #tmp

            ";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable[] dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.grid2BS.DataSource = dt[1];
            this.grid1BS.DataSource = dt[0];

            this.Grid2Filter();
        }

        private void ComboxStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Grid2Filter();
        }

        private void Grid1_SelectionChanged(object sender, EventArgs e)
        {
            this.Grid2Filter();
        }

        private void Grid2Filter()
        {
            if (this.grid1.CurrentDataRow == null)
            {
                return;
            }

            string filter = $"SEQ1 = '{this.grid1.CurrentDataRow["SEQ1"]}' AND SEQ2 = '{this.grid1.CurrentDataRow["SEQ2"]}'";
            switch (this.comboxStatus.Text)
            {
                case "All":
                    break;
                case "Released":
                    filter += " AND BalanceQty = 0";
                    break;
                case "Release Not Done":
                    filter += " AND BalanceQty > 0";
                    break;
            }

            this.grid2BS.Filter = filter;
        }
    }
}