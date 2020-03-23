using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private DateTime? InspectionDate1;
        private DateTime? InspectionDate2;
        private DateTime? SCIDelivery1;
        private DateTime? SCIDelivery2;
        private DateTime? Delivery1;
        private DateTime? Delivery2;

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
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Empty;
            string sqlWhere = string.Empty;
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

            sqlCmd += string.Format(
                @"
select distinct o.ID
	,o.CustPONo
	,o.BrandID
	,o.BuyerDelivery
	,o.SciDelivery
	,[OrderQty] = o.Qty * isnull(ol.LocationQty, sl.LocationQty)
	,[TTLQcOutput] = i.tCnt
	,[MDFailQty] = isnull(pd.MDFailQty,0)
	,[MDpassBalance] = isnull(pd.MDFailQty,0) - i.tCnt
	,[Scan and Pack Qty] = (isnull(pd.ScanQty,0) * isnull(ol.LocationQty, sl.LocationQty))
	,[Scan and Pack Balance] = (isnull(pd.ScanQty,0) * isnull(ol.LocationQty, sl.LocationQty)) - isnull(pd.MDFailQty,0)
from Orders o with(nolock)
outer apply(
	select pd.id
		, pd.MDFailQty
		,[ScanQty] = sum(pd.ScanQty)
	from PackingList_Detail pd
	inner join PackingList p on pd.ID = p.ID
	where o.ID = pd.OrderID
	and p.Status = 'Confirmed'
	group by  pd.id, pd.MDFailQty
)pd
outer apply(
	select [tCnt] = count(*)
	from [ExtendServer].ManufacturingExecution.dbo.Inspection i with(nolock)
	where i.OrderId = o.ID
	and i.Status in ('Pass', 'Fixed') 
)i
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Order_Location
	where OrderId = o.ID
)ol
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Style_Location
	where StyleUkey = o.StyleUkey
)sl
where 1=1
{0}

select distinct o.ID
	,o.CustPONo
	,o.BrandID
	,[PackID] = pd.ID
	,pd.CTNStartNo
	,[CatronQty] = pd.ShipQty * isnull(ol.LocationQty, sl.LocationQty)
	,[TTLQcOutput] = i.tCnt
	,pd.MDFailQty
	,pd.DRYReceiveDate
	,pd.MDScanDate
	,[ScanQty] = pd.ScanQty * isnull(ol.LocationQty, sl.LocationQty)
	,pd.ScanEditDate
	,os.BuyerDelivery
	,o.SciDelivery
from Orders o with(nolock)
left join PackingList_Detail pd with(nolock) on o.ID = pd.OrderID
												and pd.CTNQty > 0
left join Order_QtyShip os on pd.OrderID = os.Id
							and pd.OrderShipmodeSeq = os.Seq
outer apply(
	select [tCnt] = count(*)
	from [ExtendServer].ManufacturingExecution.dbo.Inspection i with(nolock)
	where i.OrderId = o.ID
	and i.Status in ('Pass', 'Fixed') 
)i
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Order_Location
	where OrderId = o.ID
)ol
outer apply
(
	select [LocationQty] = count(distinct Location)
	from Style_Location
	where StyleUkey = o.StyleUkey
)sl
where 1=1
{0}

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

            return Result.True;
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Sewing_R12.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData[0], string.Empty, "Sewing_R12.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, "Sewing_R12.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[2]); // 將datatable copy to excel

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Sewing_R12");
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
