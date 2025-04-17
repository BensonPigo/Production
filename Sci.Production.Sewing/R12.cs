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
            this.txtMdivision.Enabled = false;
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
            #region Where 條件
            string where = string.Empty;
            string wheredetail = string.Empty;
            if (this.InspectionDate1.HasValue && this.InspectionDate2.HasValue)
            {
                where += $@"
and exists(
	select 1
	from [ExtendServer].ManufacturingExecution.dbo.Inspection i with(nolock)
	where i.InspectionDate between '{this.InspectionDate1.Value.ToString("yyyyMMdd")}' and '{this.InspectionDate2.Value.ToString("yyyyMMdd")}'
    and i.Status in ('Pass', 'Fixed')
	and orderid = o.id
)
";
            }

            if (this.SCIDelivery1.HasValue && this.SCIDelivery2.HasValue)
            {
                where += $"and o.SCIDelivery between '{this.SCIDelivery1.Value.ToString("yyyyMMdd")}' and '{this.SCIDelivery2.Value.ToString("yyyyMMdd")}'" + Environment.NewLine;
            }

            if (this.Delivery1.HasValue && this.Delivery2.HasValue)
            {
                where += $"and exists(select 1 from Order_QtyShip oq with(nolock) where oq.id = o.id and oq.BuyerDelivery between '{this.Delivery1.Value.ToString("yyyyMMdd")}' and '{this.Delivery2.Value.ToString("yyyyMMdd")}')" + Environment.NewLine;
                wheredetail += $"and oq.BuyerDelivery between '{this.Delivery1.Value.ToString("yyyyMMdd")}' and '{this.Delivery2.Value.ToString("yyyyMMdd")}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                where += $"AND o.MDivisionID = '{this.MDivisionID}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                where += $"AND o.FtyGroup = '{this.FactoryID}'" + Environment.NewLine;
            }

            if (this.IsExcludeSister)
            {
                where += $"AND f.IsProduceFty=1" + Environment.NewLine;
            }
            #endregion

            string sqlCmd = $@"
select
	o.ID
	,o.CustPONo
	,o.BrandID
	,o.BuyerDelivery -- Summary 分頁 顯示 Orders.BuyerDelivery
	,o.SciDelivery
	,o.Qty
	,o.FactoryID
	,o.StyleID
	,QCQty = isnull(i.tCnt, 0)
	, f.KPICode
into #base
from Orders o with(nolock)
left JOIN Factory f WITH(NOLOCK) ON f.ID = o.FactoryID
outer apply(
	select [tCnt] = count(1)
	from [ExtendServer].ManufacturingExecution.dbo.Inspection i with(nolock)
	where i.OrderId = o.ID
	and i.Status in ('Pass', 'Fixed') 
) i
where 1 = 1
and o.Category in ('B','G')
{where}

--- Detail ---
select
	o.ID
	,o.CustPONo
	,o.BrandID
	,oq.Seq
	,oq.BuyerDelivery -- Detail 顯示 Order_QtyShip.BuyerDelivery
	,o.SciDelivery
	,o.FactoryID
	,o.StyleID
	,o.QCQty
	,o.KPICode
into #base_Detail
from #base o
left join Order_QtyShip oq with(nolock) on o.ID = oq.ID
where 1=1
{wheredetail}

select
		o.*
		, pd.PackingListID
        , pd.CtnStartNo
        , MDFailQty = isnull(pd.DryRoomMDFailQty, 0)
		, pd.ScanEditDate
		, pd.DRYReceiveDate
into #tmp_Detail_P
from #base_Detail o with(nolock)
outer apply(
	select
		PackingListID = pd.ID
        ,pd.CtnStartNo
		--這邊取 max 是因為 OrderID,OrderShipmodeSeq,PackingListID,CtnStartNo 有多筆(混Size)時, 數量會double
        ,DryRoomMDFailQty = Max (pd.DryRoomMDFailQty)      
		,ScanEditDate = Max (pd.ScanEditDate)   
		,DRYReceiveDate = Max (pd.DRYReceiveDate)
	from PackingList_Detail pd with(nolock)
	where pd.OrderID = o.id and pd.OrderShipmodeSeq = o.Seq
	group by pd.ID, pd.CtnStartNo
)pd

select
	o.*	
	-- OrderID,PackingListID,CTNStartNo 並非這些資料表的 Pkey, 以防萬一分別用子查詢
	, DryRoomRecdDate = (select Max(dryR.AddDate) from DRYReceive dryR WITH(NOLOCK) where dryR.OrderID = o.ID and dryR.PackingListID = o.PackingListID and dryR.CTNStartNo = o.CTNStartNo)
	, DRYTransferDate = (select Max (dryT.AddDate) from DRYTransfer dryT WITH(NOLOCK) where dryT.OrderID = o.ID and dryT.PackingListID = o.PackingListID and dryT.CTNStartNo = o.CTNStartNo)
	, MDScanDate = (select Max (md.AddDate) from MDScan md WITH(NOLOCK) where md.OrderID = o.ID and md.PackingListID = o.PackingListID and md.CTNStartNo = o.CTNStartNo)
	, ClogReceive = (select MAX(cr.AddDate) from ClogReceive cr WITH(NOLOCK) where cr.OrderID = o.ID and cr.PackingListID = o.PackingListID and cr.CTNStartNo = o.CTNStartNo)
into #tmp_Detail_Date
from #tmp_Detail_P o

select  
        o.KPICode
        ,o.FactoryID
        ,o.ID
        ,o.CustPONo
        ,o.StyleID
        ,o.Seq
        ,o.BuyerDelivery
        ,o.SciDelivery
        ,o.BrandID
        ,o.CTNStartNo
        ,[CartonQty] = pl_Qty.ShipQty
        ,[TTLQcOutput] = o.QCQty
        ,MDPassQty = iif (o.MDScanDate is null, 0, isnull(pl_Qty.ShipQty, 0) - isnull(o.MDFailQty,0))
        ,[ScanQty] = pl_Qty.ScanQty
        ,o.DryRoomRecdDate
        ,o.MDScanDate
        ,o.DRYTransferDate
        ,o.ScanEditDate
        ,o.ClogReceive
        ,o.PackingListID
into #Detail
from #tmp_Detail_Date o with(nolock)
outer apply(
	select
		ShipQty = SUM(pd.ShipQty)
		, ScanQty = sum (pd.ScanQty)
	from PackingList_Detail pd  with(nolock)
	inner join PackingList p with(nolock) on pd.ID = p.ID
	where pd.OrderID = o.ID
	and pd.id =o.PackingListID
	and pd.CTNStartNo = o.CTNStartNo
)pl_Qty

--- Summary ---
select
	o.*
	,MDPassQty
	,ScanQty
into #Summary
from #base o
outer apply(
	--此處要用Detail計算完加總 -- 因 MDPassQty 計算用到 MDFailQty, 此欄位遇到混Size時是取Max
	--若直接串PackingList_Detail分別計算後才加總, MDFailQty 會被重複減去
	select MDPassQty = sum(d.MDPassQty), ScanQty = sum(d.ScanQty)
	from #Detail d
	where d.ID = o.ID
)d

select
	o.ID
	,o.CustPONo
	,o.BrandID
	,o.BuyerDelivery
	,o.SciDelivery
	,[OrderQty] = o.Qty
	,[TTLQcOutput] = o.QCQty
	,[MDpassQty] = o.MDPassQty
	,[MDpassBalance] = o.MDPassQty - o.QCQty
	,[Scan and Pack Qty] = o.ScanQty
	,[Scan and Pack Balance] = o.ScanQty - o.MDPassQty
from #Summary o
order by o.ID

--- Detail ---
select * from #Detail order by ID, Seq

drop table #base,#Summary
drop table #base_Detail,#tmp_Detail_P,#tmp_Detail_Date,#Detail
";

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
