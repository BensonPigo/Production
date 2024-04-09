using System;
using System.Data;
using System.Text;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using Sci.Production.CallPmsAPI;
using System.Linq;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P41_Print
    /// </summary>
    public partial class P41_Print : Win.Tems.PrintForm
    {
        private DataTable[] printData;
        private DateTime? date1;
        private DateTime? date2;
        private string brand;
        private string invno;
        private string declarationNo;
        private string declarationStatus;

        /// <summary>
        /// P41_Print
        /// </summary>
        public P41_Print()
        {
            this.InitializeComponent();
            this.dateDate.Value1 = DateTime.Today;
            this.dateDate.Value2 = DateTime.Today;
            this.comboDeclarationStatus.Add(string.Empty, string.Empty);
            this.comboDeclarationStatus.Add("Confirmed", "Confirmed");
            this.comboDeclarationStatus.Add("New", "New");
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value1) || MyUtility.Check.Empty(this.dateDate.Value2))
            {
                this.dateDate.TextBox1.Focus();
                MyUtility.Msg.WarningBox("Date can't empty");
                return false;
            }

            this.date1 = this.dateDate.Value1;
            this.date2 = this.dateDate.Value2;
            this.brand = this.txtbrand.Text;
            this.invno = this.txtInvNo.Text;
            this.declarationNo = this.txtDeclarationNo.Text;
            this.declarationStatus = this.comboDeclarationStatus.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCondition = new StringBuilder();
            string sqlWhereBreakdownChanged = string.Empty;
            string sqlWhereBreakdownChangedBrand = string.Empty;

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCondition.Append(string.Format(" and e.CDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("yyyy/MM/dd")));
                sqlWhereBreakdownChanged += $"  and e.CDate >= '{Convert.ToDateTime(this.date1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCondition.Append(string.Format(" and e.CDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("yyyy/MM/dd")));
                sqlWhereBreakdownChanged += $"  and e.CDate <= '{Convert.ToDateTime(this.date2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.invno))
            {
                sqlCondition.Append($" and e.InvNo = '{this.invno}'");
                sqlWhereBreakdownChanged += $" and e.InvNo = '{this.invno}'";
            }

            if (!MyUtility.Check.Empty(this.declarationNo))
            {
                sqlCondition.Append($" and e.DeclareNo = '{this.declarationNo}'");
                sqlWhereBreakdownChanged += $" and e.DeclareNo = '{this.declarationNo}'";
            }

            if (!MyUtility.Check.Empty(this.declarationStatus))
            {
                sqlCondition.Append($" and e.Status = '{this.declarationStatus}'");
                sqlWhereBreakdownChanged += $" and e.Status = '{this.declarationStatus}'";
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCondition.Append($@" and (
pl.BrandID = '{this.brand}') or g.BrandID = '{this.brand}'
)");
                sqlWhereBreakdownChangedBrand += $" and p.BrandID = '{this.brand}'";
            }

            string sqlGetA2B = $@"
select  distinct gd.ID, gd.PLFromRgCode
from    GMTBooking_Detail gd with (nolock)
where exists(select 1 from VNExportDeclaration e WITH (NOLOCK) where gd.ID = e.InvNo {sqlWhereBreakdownChanged})
";
            DataTable dtA2BBase;
            DualResult result = DBProxy.Current.Select("Production", sqlGetA2B, out dtA2BBase);

            if (!result)
            {
                return result;
            }

            DataTable dtA2BPackResult = new DataTable();

            // 給預設結構
            string sqlInitialA2BPackResult = @"
select  p.InvNo,
        p.ID,
        p.OrderID,
        pd.Article,
        pd.SizeCode,
        pd.ShipQty
from Packinglist p with (nolock)
inner join Packinglist_Detail pd with (nolock) on p.ID = pd.ID
where 1 = 0
";
            result = DBProxy.Current.Select("Production", sqlInitialA2BPackResult, out dtA2BPackResult);

            if (!result)
            {
                return result;
            }

            if (dtA2BBase.Rows.Count > 0)
            {
                string sqlGetA2BPackDetail = string.Empty;
                var groupPLFromRgCode = dtA2BBase.AsEnumerable()
                                                .GroupBy(s => s["PLFromRgCode"].ToString());
                foreach (var itemPLFromRgCode in groupPLFromRgCode)
                {
                    string whereInvno = itemPLFromRgCode.Select(s => $"'{s["ID"]}'").JoinToString(",");

                    sqlGetA2BPackDetail = $@"
select  p.InvNo,
        p.ID,
        pd.OrderID,
        pd.Article,
        pd.SizeCode,
        pd.ShipQty
from    Packinglist p with (nolock)
inner join Packinglist_Detail pd with (nolock) on p.ID = pd.ID
where   p.InvNo in  ({whereInvno}) {sqlWhereBreakdownChangedBrand}
";
                    DataTable dtA2BResult;
                    result = PackingA2BWebAPI.GetDataBySql(itemPLFromRgCode.Key, sqlGetA2BPackDetail, out dtA2BResult);

                    if (!result)
                    {
                        return result;
                    }

                    dtA2BResult.MergeTo(ref dtA2BPackResult);
                }
            }

            string sqlCmd =
                $@"
alter table #tmpPackA2B alter column InvNo varchar(25)
alter table #tmpPackA2B alter column ID varchar(13)
alter table #tmpPackA2B alter column OrderID varchar(13)
alter table #tmpPackA2B alter column Article varchar(8)
alter table #tmpPackA2B alter column SizeCode varchar(8)

--Summary
;with FirstStepFilterData
as (
select  e.ID,
e.CDate,
e.InvNo,
e.VNContractID,
e.VNExportPortID,
e.DataFrom,
[ExportPort] = isnull(ep.Name,''),
[GMTBookingStatus] = g.Status,
[ExportDeclarationStatus] = e.Status,
g.BrandID,
g.TotalShipQty as ShipQty,
g.TotalCTNQty as CTNQty,
g.TotalGW as GW,
g.TotalNW as NW,
g.Dest as Dest,
g.ShipTermID as ShipTerm
from VNExportDeclaration e WITH (NOLOCK) 
left join VNExportPort ep WITH (NOLOCK) on e.VNExportPortID = ep.ID
left join GMTBooking g WITH (NOLOCK) on e.InvNo = g.ID
where e.DataFrom = 'GMTBOOKING' {sqlCondition}
union all
select  e.ID,
e.CDate,
e.InvNo,
e.VNContractID,
e.VNExportPortID,
e.DataFrom,
[ExportPort] = isnull(ep.Name,''),
[GMTBookingStatus] = '',
[ExportDeclarationStatus] = e.Status,
p.BrandID,
p.ShipQty,
p.CTNQty,
p.GW,
p.NW,
p.Dest,
[ShipTerm] = (select ShipTermID from Orders WITH (NOLOCK) where ID = (select top 1 OrderID from PackingList_Detail WITH (NOLOCK) where ID = p.ID))
from VNExportDeclaration e WITH (NOLOCK) 
left join VNExportPort ep WITH (NOLOCK) on e.VNExportPortID = ep.ID
left join Packinglist p WITH (NOLOCK) on e.InvNo = p.INVNo
where e.DataFrom = 'PACKINGLIST' {sqlCondition}
),
SecondStepFilterData
as (
select *,(select sum(ROUND(ed.ExportQty*c.CPU*c.VNMultiple,2))
from VNExportDeclaration_Detail ed WITH (NOLOCK) 
inner join VNConsumption c WITH (NOLOCK) on c.CustomSP = ed.CustomSP
where ed.ID = FirstStepFilterData.ID
and c.VNContractID = FirstStepFilterData.VNContractID) as CMP from FirstStepFilterData
),
tmpDetail
as (
select distinct e.InvNo,ed.OrderID,ed.StyleID,ed.SizeCode,ed.CustomSP,ed.ExportQty,[dbo].getOrderUnitPrice(2,ed.StyleUKey,ed.OrderID,ed.Article,ed.SizeCode) as FOB
from SecondStepFilterData e
inner join VNExportDeclaration_Detail ed WITH (NOLOCK) on e.ID = ed.ID
),
tmpSumDetail
as (
select InvNo,OrderID,StyleID,SizeCode,CustomSP,sum(ExportQty) as TtlExportQty,FOB
from tmpDetail
group by InvNo,OrderID,StyleID,SizeCode,CustomSP,FOB
),
tmpSummaryData
as (
select distinct sd.GMTBookingStatus, sd.ExportDeclarationStatus, sd.InvNo,sd.VNExportPortID,sd.ExportPort,sd.Dest,isnull(c.Alias,'') as CountryAlias,sd.ShipTerm,sd.ShipQty,
CTNQty,sd.GW,sd.NW,sd.CMP
from SecondStepFilterData sd
left join Country c WITH (NOLOCK) on sd.Dest = c.ID
)
select '0' as Type,count(InvNo)  as rno,'' as GMTBookingStatus,'' as ExportDeclarationStatus,'' as InvNo,'' as VNExportPortID,'' as ExportPort,'' as Dest,'' as CountryAlias,'' as ShipTerm,0 as ShipQty,0 as CTNQty,0 as GW,0 as NW,0 as CMP,
'' as InvNo1,'' as OrderID,'' as StyleID,'' as SizeCode,'' as CustomSP,0 as TtlExportQty,0.0 as FOB from tmpSummaryData
union all
select '1' as Type,ROW_NUMBER() OVER (ORDER BY InvNo) as rno,*,
'' as InvNo1,'' as OrderID,'' as StyleID,'' as SizeCode,'' as CustomSP,0 as TtlExportQty,0.0 as FOB from tmpSummaryData
union all
select '2' as Type,0 as rno,'' as GMTBookingStatus,'' as ExportDeclarationStatus,'' as InvNo,'' as VNExportPortID,'' as ExportPort,'' as Dest,'' as CountryAlias,'' as ShipTerm,0 as ShipQty,0 as CTNQty,0 as GW,0 as NW,0 as CMP,
InvNo as InvNo1,OrderID,StyleID,SizeCode,CustomSP,TtlExportQty,FOB
from tmpSumDetail

--Breakdown changed
SELECT	distinct
		e.InvNo,
		vdd.OrderID,
		vdd.Article,
		vdd.SizeCode,
		vdd.ExportQty
into #tmpDeclaration
from VNExportDeclaration e with (nolock)
inner join VNExportDeclaration_Detail vdd with (nolock) on vdd.ID = e.ID
where 1 = 1 {sqlWhereBreakdownChanged}

select * into #tmpPack
from (
    select  p.ID,
            t.InvNo,
    		t.OrderID,
    		t.Article,
    		t.SizeCode,
    		t.ExportQty,
            [PackQty] = isnull(sum(pd.ShipQty), 0)
    from    #tmpDeclaration t
    inner join  Packinglist p with (nolock) on p.InvNo = t.InvNo
    inner join  PackingList_Detail pd with (nolock) on  pd.ID = p.ID and
                                                        pd.OrderID = t.OrderID and
                                                        pd.Article = t.Article and
                                                        pd.SizeCode = t.SizeCode
    where 1 = 1 {sqlWhereBreakdownChangedBrand}
    group by    p.ID,
                t.InvNo,
    		    t.OrderID,
    		    t.Article,
    		    t.SizeCode,
    		    t.ExportQty
    union all
    select  ta.ID,
            t.InvNo,
    		t.OrderID,
    		t.Article,
    		t.SizeCode,
    		t.ExportQty,
            [PackQty] = isnull(sum(ta.ShipQty), 0)
    from    #tmpDeclaration t
    inner join  #tmpPackA2B ta on  ta.InvNo = t.InvNo and
                                   ta.OrderID = t.OrderID and
                                   ta.Article = t.Article and
                                   ta.SizeCode = t.SizeCode
    group by    ta.ID,
                t.InvNo,
    		    t.OrderID,
    		    t.Article,
    		    t.SizeCode,
    		    t.ExportQty
) a

select  t.InvNo,
		t.OrderID,
		t.Article,
		t.SizeCode,
		t.ExportQty,
        [PackID] = PackID.val,
        [PackQty] = PackQty.val
from    #tmpDeclaration t
outer apply (select val = isnull(sum(p.PackQty), 0)
             from #tmpPack p 
             where   p.InvNo = t.InvNo and
                     p.OrderID = t.OrderID and
                     p.Article = t.Article and
                     p.SizeCode = t.SizeCode)  PackQty
outer apply (select val = Stuff((select distinct concat( ',', p.ID)
                                from #tmpPack p 
                                where   p.InvNo = t.InvNo and
                                        p.OrderID = t.OrderID and
                                        p.Article = t.Article and
                                        p.SizeCode = t.SizeCode FOR XML PATH('')),1,1,'')
            )  PackID
where   t.ExportQty <> PackQty.val

drop table #tmpDeclaration, #tmpPack

";

            result = MyUtility.Tool.ProcessWithDatatable(dtA2BPackResult, null, sqlCmd, out this.printData, temptablename: "#tmpPackA2B");

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
            if (this.printData[0].Rows.Count <= 1)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData[0].Rows.Count);

            this.ShowWaitMessage("Starting EXCEL...");

            // 填內容值
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P41_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            if (this.printData[1].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.printData[1], string.Empty, "Shipping_P41_Print.xltx", 1, false, null, excel, wSheet: excel.Sheets[1]);
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[3];
            int row = 1, sheetcount = 2;
            string invNo = "XXX";
            object[,] objArray = new object[1, 15];
            object[,] objArray1 = new object[1, 7];
            foreach (DataRow dr in this.printData[0].Rows)
            {
                if (MyUtility.Convert.GetString(dr["Type"]) == "0")
                {
                    // 有幾筆Custom SP就新增幾個WorkSheet
                    if (MyUtility.Convert.GetInt(dr["rno"]) > 1)
                    {
                        for (int i = 1; i < MyUtility.Convert.GetInt(dr["rno"]); i++)
                        {
                            worksheet.Copy(Type.Missing, worksheet);
                        }
                    }
                }

                if (MyUtility.Convert.GetString(dr["Type"]) == "1")
                {
                    worksheet = excel.ActiveWorkbook.Worksheets[2];
                    row++;
                    objArray[0, 0] = dr["rno"];
                    objArray[0, 1] = dr["InvNo"];
                    objArray[0, 2] = dr["GMTBookingStatus"];
                    objArray[0, 3] = dr["ExportDeclarationStatus"];
                    objArray[0, 4] = dr["VNExportPortID"];
                    objArray[0, 5] = dr["ExportPort"];
                    objArray[0, 6] = dr["CountryAlias"];
                    objArray[0, 7] = MyUtility.Check.Empty(dr["CountryAlias"]) ? string.Empty : MyUtility.Convert.GetString(dr["CountryAlias"]).Substring(0, 1);
                    objArray[0, 8] = MyUtility.Check.Empty(dr["CountryAlias"]) ? string.Empty : MyUtility.Convert.GetString(dr["CountryAlias"]).Substring(1, 1);
                    objArray[0, 9] = dr["ShipTerm"];
                    objArray[0, 10] = dr["ShipQty"];
                    objArray[0, 11] = dr["CTNQty"];
                    objArray[0, 12] = dr["GW"];
                    objArray[0, 13] = dr["NW"];
                    objArray[0, 14] = dr["CMP"];
                    worksheet.Range[string.Format("A{0}:O{0}", row)].Value2 = objArray;
                }

                if (MyUtility.Convert.GetString(dr["Type"]) == "2")
                {
                    if (invNo != MyUtility.Convert.GetString(dr["InvNo1"]))
                    {
                        excel.Cells.EntireColumn.AutoFit();
                        excel.Cells.EntireRow.AutoFit();
                        sheetcount++;
                        invNo = MyUtility.Convert.GetString(dr["InvNo1"]);
                        row = 1;
                        worksheet = excel.ActiveWorkbook.Worksheets[sheetcount];
                        worksheet.Name = MyUtility.Convert.GetString(dr["InvNo1"]); // 更改Sheet Name
                        worksheet.Select();
                    }

                    row++;
                    objArray1[0, 0] = dr["OrderID"];
                    objArray1[0, 1] = dr["StyleID"];
                    objArray1[0, 2] = dr["SizeCode"];
                    objArray1[0, 3] = dr["CustomSP"];
                    objArray1[0, 4] = dr["TtlExportQty"];
                    objArray1[0, 5] = dr["FOB"];
                    objArray1[0, 6] = MyUtility.Convert.GetDecimal(dr["TtlExportQty"]) * MyUtility.Convert.GetDecimal(dr["FOB"]);

                    worksheet.Range[string.Format("A{0}:G{0}", row)].Value2 = objArray1;
                }
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();

            // 最後顯示前將Sheet切換到第一頁
            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Select();
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_P41_Print");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
