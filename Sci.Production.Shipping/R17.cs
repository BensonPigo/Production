﻿using Ict;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    public partial class R17 : Win.Tems.PrintForm
    {
        private string BuyerDelivery1;
        private string BuyerDelivery2;
        private string PulloutDate1;
        private string PulloutDate2;
        private string Brand;
        private DataTable PrintTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="R17"/> class.
        /// </summary>
        /// <param name="menuitem"></param>
        public R17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery can not empty!");
                return false;
            }

            this.BuyerDelivery1 = ((DateTime)this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            this.BuyerDelivery2 = ((DateTime)this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd");

            if (!MyUtility.Check.Empty(this.datePulloutDate.Value1))
            {
                this.PulloutDate1 = ((DateTime)this.datePulloutDate.Value1).ToString("yyyy/MM/dd");
                this.PulloutDate2 = ((DateTime)this.datePulloutDate.Value2).ToString("yyyy/MM/dd");
            }
            else
            {
                this.PulloutDate1 = string.Empty;
                this.PulloutDate2 = string.Empty;
            }

            this.Brand = this.txtbrand.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string where = string.Empty;
            where += $@" and o.BuyerDelivery between '{this.BuyerDelivery1}' and '{this.BuyerDelivery2}'";
            if (!MyUtility.Check.Empty(this.PulloutDate1))
            {
                where += $@" and pl.PulloutDate between '{this.PulloutDate1}' and '{this.PulloutDate2}'";
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                where += $@" and o.BrandID = '{this.Brand}' ";
            }

            string sqlcmd = $@"
select distinct
	o.FactoryID,
	o.ID,
	seq=pld.OrderShipmodeSeq,
	o.BrandID,
	o.CustPONo,
	PackingListID=pl.ID,
	GMTBookingID=g.ID,
	pl.ShipPlanID,
	pl.PulloutID,
	qty=isnull(oq.Qty,0),
	ShipQty=sum(pld.ShipQty)over(partition by pld.id,pld.OrderID,pld.OrderShipmodeSeq),
	Order_QtyShip_ShipmodeID=oq.ShipmodeID,
	GMTBooking_ShipModeID=g.ShipModeID,
	PackingCreateBy =(select concat(pass1.id,'-',pass1.name ) from pass1 with(nolock) where pass1.id = pl.AddName),
	GBHandle =(select concat(pass1.id,'-',pass1.name ) from pass1 with(nolock) where pass1.id = g.Handle)
into #tmp
from PackingList_Detail pld with(nolock)
inner join PackingList pl with(nolock) on pld.id=pl.id and pl.Type in('B','S')
inner join orders o with(nolock) on pld.OrderID=o.id
left join Order_QtyShip oq with(nolock) on pld.OrderID=oq.id and pld.OrderShipmodeSeq = oq.Seq
left join {"{0}"} g with(nolock) on g.id = pl.INVNo
where 1=1
{where}
{"{1}"}
 
select 
	o.ID,
	oq.Seq
into #DistinctSeq
from PackingList_Detail pld with(nolock)
inner join PackingList pl with(nolock) on pld.id=pl.id and pl.Type in('B','S')
inner join orders o with(nolock) on pld.OrderID=o.id
inner join Order_QtyShip oq with(nolock) on pld.OrderID=oq.id
where 1=1
{where}
{"{1}"}
union
select 
	pld.OrderID,
	pld.OrderShipmodeSeq
from PackingList_Detail pld with(nolock)
inner join PackingList pl with(nolock) on pld.id=pl.id and pl.Type in('B','S')
inner join orders o with(nolock) on pld.OrderID=o.id
where 1=1
{where}
{"{1}"}

select d.ID,d.Seq
into #diffSeq
from #DistinctSeq d
where not exists(select 1 from #tmp t where d.ID=t.ID and d.Seq=t.Seq)


select distinct
	o.FactoryID,
	o.ID,
	oq.Seq,
	o.BrandID,
	o.CustPONo,
	PackingListID=pl.ID,
	GMTBookingID=g.ID,
	pl.ShipPlanID,
	pl.PulloutID,
	oq.Qty,
	ShipQty=sum(pld.ShipQty)over(partition by pld.id,pld.OrderID,pld.OrderShipmodeSeq),
	Order_QtyShip_ShipmodeID=oq.ShipmodeID,
	GMTBooking_ShipModeID=g.ShipModeID,
	PackingCreateBy =(select concat(pass1.id,'-',pass1.name ) from pass1 with(nolock) where pass1.id = pl.AddName),
	GBHandle =(select concat(pass1.id,'-',pass1.name ) from pass1 with(nolock) where pass1.id = g.Handle)
into #diffSeqData
from #diffSeq d
inner join Order_QtyShip oq with(nolock) on oq.id = d.ID and oq.Seq = d.Seq
inner join orders o with(nolock) on oq.ID=o.id
left join PackingList_Detail pld with(nolock) on pld.OrderID=oq.id and pld.OrderShipmodeSeq = oq.Seq
left join PackingList pl with(nolock) on pld.id=pl.id and pl.Type in('B','S')
left join {"{0}"} g with(nolock) on g.id = pl.INVNo

union all

select distinct
	o.FactoryID,
	o.ID,
	 pld.OrderShipmodeSeq,
	o.BrandID,
	o.CustPONo,
	PackingListID=pl.ID,
	GMTBookingID=g.ID,
	pl.ShipPlanID,
	pl.PulloutID,
	qty=0,
	ShipQty=sum(pld.ShipQty)over(partition by pld.id,pld.OrderID,pld.OrderShipmodeSeq),
	Order_QtyShip_ShipmodeID='',
	GMTBooking_ShipModeID=g.ShipModeID,
	PackingCreateBy =(select concat(pass1.id,'-',pass1.name ) from pass1 with(nolock) where pass1.id = pl.AddName),
	GBHandle =(select concat(pass1.id,'-',pass1.name ) from pass1 with(nolock) where pass1.id = g.Handle)
from #diffSeq d
inner join PackingList_Detail pld with(nolock) on pld.OrderID = d.ID and pld.OrderShipmodeSeq = d.Seq
inner join PackingList pl with(nolock) on pld.id=pl.id and pl.Type in('B','S')
inner join orders o with(nolock) on pld.OrderID=o.id
left join {"{0}"} g with(nolock) on g.id = pl.INVNo


select *
from #tmp t
where Qty<> t.ShipQty or t.Qty is null

union
select*
from #diffSeqData
order by FactoryID, ID, Seq

drop table #tmp,#DistinctSeq,#diffSeq,#diffSeqData
";
            DualResult result = DBProxy.Current.Select(null, string.Format(sqlcmd, "GMTBooking", string.Empty), out this.PrintTable);
            if (!result)
            {
                return result;
            }

            #region get A2B data
            List<string> listPLFromRgCode = PackingA2BWebAPI.GetAllPLFromRgCode();
            if (listPLFromRgCode.Count > 0)
            {
                string sqlWhereA2B = string.Empty;
                if (!MyUtility.Check.Empty(this.PulloutDate1))
                {
                    sqlWhereA2B += $@" and gd.PulloutDate between '{this.PulloutDate1}' and '{this.PulloutDate2}'";
                }

                string sqlGetGMTBookingA2B = $@"
select  g.ID,
        g.ShipModeID,
        g.Handle
from GMTBooking g with (nolock)
where exists (select 1 from GMTBooking_Detail gd with (nolock) where g.ID = gd.ID {sqlWhereA2B})
";

                DataTable dtGMT;

                result = DBProxy.Current.Select(null, sqlGetGMTBookingA2B, out dtGMT);
                if (!result)
                {
                    return result;
                }

                if (dtGMT.Rows.Count > 0)
                {
                    string whereForA2B = @" and exists (select 1 from #tmpGMT t where t.ID = pl.INVNo )";
                    string sqlcmdA2B = @"
alter table #tmpGMT alter column ID varchar(25)
" + string.Format(sqlcmd, "#tmpGMT", whereForA2B);

                    PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
                    {
                        SqlString = sqlcmdA2B,
                        TmpTable = JsonConvert.SerializeObject(dtGMT),
                        TmpTableName = "#tmpGMT",
                    };
                    foreach (string plFromRgCode in listPLFromRgCode)
                    {
                        result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, dataBySql, out DataTable dtResultA2B);

                        if (!result)
                        {
                            return result;
                        }

                        dtResultA2B.MergeTo(ref this.PrintTable);
                    }
                }
            }
            #endregion

            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.PrintTable.Rows.Count);
            if (this.PrintTable.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string excelName = "Shipping_R17";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            MyUtility.Excel.CopyToXls(this.PrintTable, string.Empty, $"{excelName}.xltx", 1, false, null, excelApp, wSheet: excelApp.Sheets[1]); // 將datatable copy to excel
            excelApp.DisplayAlerts = false;
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1]; // 取得工作表

            worksheet.Columns.AutoFit();
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion

            this.HideWaitMessage();
            strExcelName.OpenFile();
            return true;
        }
    }
}
