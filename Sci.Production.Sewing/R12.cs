using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class R12 : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private DateTime? InspectionDate1;
        private DateTime? InspectionDate2;
        private DateTime? SCIDelivery1;
        private DateTime? SCIDelivery2;
        private DateTime? Delivery1;
        private DateTime? Delivery2;

        private string MDivisionID;
        private string FactoryID;
        private bool IsExcludeSister;

        /// <inheritdoc/>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!(this.dateRangeEndlineInspectionDate.Value1.HasValue && this.dateRangeEndlineInspectionDate.Value2.HasValue) &&
               !(this.dateRangeSCIDelivery.Value1.HasValue && this.dateRangeSCIDelivery.Value2.HasValue) &&
               !(this.dateRangeBuyerDelivery.Value1.HasValue && this.dateRangeBuyerDelivery.Value2.HasValue))
            {
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            this.InspectionDate1 = this.dateRangeEndlineInspectionDate.Value1;
            this.InspectionDate2 = this.dateRangeEndlineInspectionDate.Value2;
            this.SCIDelivery1 = this.dateRangeSCIDelivery.Value1;
            this.SCIDelivery2 = this.dateRangeSCIDelivery.Value2;
            this.Delivery1 = this.dateRangeBuyerDelivery.Value1;
            this.Delivery2 = this.dateRangeBuyerDelivery.Value2;
            this.MDivisionID = this.txtMdivision.Text;
            this.FactoryID = this.txtfactory.Text;
            this.IsExcludeSister = this.chkExcludeSis.Checked;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;
            string sqlWhere = string.Empty;

            #region Where 條件
            if (this.InspectionDate1.HasValue && this.InspectionDate2.HasValue)
            {
                sqlCmd = string.Format(
                    @"
select distinct OrderId
into #tmp_InspectionOrderId
from [ExtendServer].ManufacturingExecution.dbo.Inspection i with(nolock)
where i.InspectionDate between '{0}' and '{1}'
and i.Status in ('Pass', 'Fixed');
",
                    this.InspectionDate1.Value.ToString("yyyyMMdd"),
                    this.InspectionDate2.Value.ToString("yyyyMMdd"));

                sqlWhere = "and exists (select 1 from #tmp_InspectionOrderId where OrderId = o.ID)";
            }

            if (this.SCIDelivery1.HasValue && this.SCIDelivery2.HasValue)
            {
                sqlWhere += string.Format(
                    "and o.SCIDelivery between '{0}' and '{1}'",
                    this.SCIDelivery1.Value.ToString("yyyyMMdd"),
                    this.SCIDelivery2.Value.ToString("yyyyMMdd")) + Environment.NewLine;
            }

            if (this.Delivery1.HasValue && this.Delivery2.HasValue)
            {
                sqlWhere += string.Format(
                    "and o.BuyerDelivery between '{0}' and '{1}'",
                    this.Delivery1.Value.ToString("yyyyMMdd"),
                    this.Delivery2.Value.ToString("yyyyMMdd")) + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                sqlWhere += $"AND o.MDivisionID = '{this.MDivisionID}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                sqlWhere += $"AND o.FtyGroup = '{this.FactoryID}'" + Environment.NewLine;
            }

            if (this.IsExcludeSister)
            {
                sqlWhere += $"AND f.IsProduceFty=1" + Environment.NewLine;
            }
            #endregion

            sqlCmd += string.Format(
                @"
select o.ID
        , o.CustPONo
        , o.BrandID
        , o.SciDelivery
        , o.BuyerDelivery
        , o.Qty
        , QCQty = i.tCnt
        --, LocationQty = iif(ol.LocationQty = 0, sl.LocationQty, ol.LocationQty)
into #Orders
from Orders o
left JOIN Factory f WITH(NOLOCK) ON f.ID = o.FactoryID
-- ISP20220443 要用 Complete set 為單位, 所以不需要乘上下部位數量
--outer apply
--(
--	select [LocationQty] = count(distinct Location)
--	from Order_Location with(nolock)
--	where OrderId = o.ID
--)ol
--outer apply
--(
--	select [LocationQty] = count(distinct Location)
--	from Style_Location with(nolock)
--	where StyleUkey = o.StyleUkey
--)sl
outer apply(
	select [tCnt] = count(1)
	from [ExtendServer].ManufacturingExecution.dbo.Inspection i with(nolock)
	where i.OrderId = o.ID
			and i.Status in ('Pass', 'Fixed') 
) i
where 1 = 1
and o.Category in ('B','G')
        {0}

select pd.ID
        , pd.CtnStartNo
        , pd.OrderID
        , [ShipBuyerDelivery] = max (oq.BuyerDelivery)
        , MDFailQty = Max (pd.MDFailQty)                
		, f.KPICode
		, o1.FactoryID
		, o1.StyleID
		, oq.Seq
		, DryRoomRecdDate = Max(dryR.AddDate)
		, MDScanDate = Max (md.AddDate)
		, DRYReceiveDate = Max (pd.DRYReceiveDate)
		, DRYTransferDate = Max (dryT.AddDate)
		, ScanEditDate = Max (pd.ScanEditDate)
		, ClogReceive = MAX(cr.AddDate)
into #PD_Detail
from #Orders o with(nolock)
inner join Orders o1 on o1.ID = o.ID
inner join PackingList_Detail pd with(nolock) on o.ID = pd.OrderID
left join Order_QtyShip oq with(nolock) on pd.OrderID = oq.ID
                                            and pd.OrderShipmodeSeq = oq.Seq
left JOIN Factory f WITH(NOLOCK) ON f.ID = o1.FactoryID
left join DRYReceive dryR WITH(NOLOCK) on dryR.OrderID = pd.OrderID and dryR.PackingListID = pd.ID and dryR.CTNStartNo = pd.CTNStartNo
left join MDScan md WITH(NOLOCK) on md.OrderID = pd.OrderID and md.PackingListID = pd.ID and md.CTNStartNo = pd.CTNStartNo
left join DRYTransfer dryT WITH(NOLOCK) on dryT.OrderID = pd.OrderID and dryT.PackingListID = pd.ID and dryT.CTNStartNo = pd.CTNStartNo
left join ClogReceive cr WITH(NOLOCK) on cr.OrderID = pd.OrderID and cr.PackingListID = pd.ID and cr.CTNStartNo = pd.CTNStartNo
group by pd.ID, pd.CtnStartNo, pd.OrderID, f.KPICode
		, o1.FactoryID
		, o1.StyleID
		, oq.Seq

----------------------------------------------------------------
--- Detail
----------------------------------------------------------------
select  
        pd.KPICode
        ,pd.FactoryID
        ,o.ID
        ,o.CustPONo
        ,pd.StyleID
        ,pd.Seq
        ,pd.ShipBuyerDelivery
        ,o.SciDelivery
        ,o.BrandID
        ,pd.CTNStartNo 
        ,[CartonQty] = pl_Qty.CtnQty
        ,[TTLQcOutput] = o.QCQty
        ,MDPassQty = iif (pd.MDScanDate is null, 0, pl_Qty.CtnQty - isnull(pd.MDFailQty,0))
        ,[ScanQty] = pl_Qty.ScanQty
        ,pd.DryRoomRecdDate
        ,pd.MDScanDate
        ,pd.DRYTransferDate
        ,pd.ScanEditDate
        ,pd.ClogReceive
        ,[PackID] = pd.ID
into #Detail
from #Orders o with(nolock)
left join #PD_Detail pd on o.id = pd.OrderID
outer apply(
	select [CtnQty] = SUM(pdd.ShipQty) --* o.LocationQty
            , [ScanQty] = sum (pdd.ScanQty) --* o.LocationQty
	from PackingList_Detail pdd  with(nolock)
	inner join PackingList p with(nolock) on pdd.ID = p.ID
	where o.ID = pdd.OrderID
	        and pd.ID = pdd.ID
	        and pd.CTNStartNo = pdd.CTNStartNo
)pl_Qty


----------------------------------------------------------------
--- Summary
----------------------------------------------------------------
select o.ID
	,o.CustPONo
	,o.BrandID
	,o.BuyerDelivery
	,o.SciDelivery
	,[OrderQty] = o.Qty --* o.LocationQty
	,[TTLQcOutput] = o.QCQty
	,[MDpassQty] = d.MDPassQty
	,[MDpassBalance] = d.MDPassQty - o.QCQty
	,[Scan and Pack Qty] = d.ScanQty
	,[Scan and Pack Balance] = d.ScanQty - d.MDPassQty
from #Orders o with(nolock)
outer apply (
    select CartonQty = sum(CartonQty)
            , MDPassQty = sum(MDPassQty)
            , ScanQty = sum(ScanQty)
    from #Detail d
    where o.ID = d.ID
) d

select *
from #Detail

drop table #Orders, #PD_Detail, #Detail

IF object_id('tempdb..#tmp_InspectionOrderId') IS NOT NULL drop table #tmp_InspectionOrderId
",
                sqlWhere);

            DBProxy.Current.DefaultTimeout = 900;  // timeout時間改為15分鐘
            DualResult result = DBProxy.Current.Select(string.Empty, sqlCmd, out this.printData);
            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData[0].Rows.Count);

            if (this.printData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Sewing_R12.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, "Sewing_R12.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, "Sewing_R12.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[2]); // 將datatable copy to excel

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Sewing_R12");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion

            return true;
        }
    }
}
