using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    public partial class P41_Print : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? date1, date2;
        string brand;
        public P41_Print()
        {
            InitializeComponent();
            dateRange1.Value1 = DateTime.Today;
            dateRange1.Value2 = DateTime.Today;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) || MyUtility.Check.Empty(dateRange1.Value2))
            {
                MyUtility.Msg.WarningBox("Date can't empty");
                dateRange1.TextBox1.Focus();

                return false;
            }
            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            brand = txtbrand1.Text;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(@"with FirstStepFilterData
as (
select e.ID,e.CDate,e.InvNo,e.VNContractID,e.VNExportPortID,e.DataFrom, isnull(ep.Name,'') as ExportPort,
IIF(e.DataFrom = 'PACKINGLIST',pl.BrandID,g.BrandID) as BrandID,
IIF(e.DataFrom = 'PACKINGLIST',pl.ShipQty,g.TotalShipQty) as ShipQty,
IIF(e.DataFrom = 'PACKINGLIST',pl.CTNQty,g.TotalCTNQty) as CTNQty,
IIF(e.DataFrom = 'PACKINGLIST',pl.GW,g.TotalGW) as GW,
IIF(e.DataFrom = 'PACKINGLIST',pl.NW,g.TotalNW) as NW,
IIF(e.DataFrom = 'PACKINGLIST',(select Dest from Orders where ID = (select top 1 OrderID from PackingList_Detail where ID = pl.ID)),g.Dest) as Dest,
IIF(e.DataFrom = 'PACKINGLIST',(select ShipTermID from Orders where ID = (select top 1 OrderID from PackingList_Detail where ID = pl.ID)),g.ShipTermID) as ShipTerm
from VNExportDeclaration e
left join VNExportPort ep on e.VNExportPortID = ep.ID
left join GMTBooking g on e.InvNo = g.ID
left join PackingList pl on e.InvNo = pl.INVNo
where e.CDate between '{0}' and '{1}'
and e.Status = 'Confirmed'
),
SecondStepFilterData
as (
select *,(select sum(ROUND(ed.ExportQty*c.CPU*c.VNMultiple,2))
from VNExportDeclaration_Detail ed
inner join VNConsumption c on c.CustomSP = ed.CustomSP
where ed.ID = FirstStepFilterData.ID
and c.VNContractID = FirstStepFilterData.VNContractID) as CMP from FirstStepFilterData where {2}
),
tmpDetail
as (
select e.InvNo,ed.OrderID,ed.StyleID,ed.SizeCode,ed.CustomSP,ed.ExportQty,[dbo].getOrderUnitPrice(2,ed.StyleUKey,ed.OrderID,ed.Article,ed.SizeCode) as FOB
from SecondStepFilterData e
inner join VNExportDeclaration_Detail ed on e.ID = ed.ID
),
tmpSumDetail
as (
select InvNo,OrderID,StyleID,SizeCode,CustomSP,sum(ExportQty) as TtlExportQty,FOB
from tmpDetail
group by InvNo,OrderID,StyleID,SizeCode,CustomSP,FOB
),
tmpSummaryData
as (
select distinct sd.InvNo,sd.VNExportPortID,sd.ExportPort,sd.Dest,isnull(c.Alias,'') as CountryAlias,sd.ShipTerm,sd.ShipQty,
CTNQty,sd.GW,sd.NW,sd.CMP
from SecondStepFilterData sd
left join Country c on sd.Dest = c.ID
)
select '0' as Type,count(InvNo)  as rno,'' as InvNo,'' as VNExportPortID,'' as ExportPort,'' as Dest,'' as CountryAlias,'' as ShipTerm,0 as ShipQty,0 as CTNQty,0 as GW,0 as NW,0 as CMP,
'' as InvNo1,'' as OrderID,'' as StyleID,'' as SizeCode,'' as CustomSP,0 as TtlExportQty,0.0 as FOB from tmpSummaryData
union all
select '1' as Type,ROW_NUMBER() OVER (ORDER BY InvNo) as rno,*,
'' as InvNo1,'' as OrderID,'' as StyleID,'' as SizeCode,'' as CustomSP,0 as TtlExportQty,0.0 as FOB from tmpSummaryData
union all
select '2' as Type,0 as rno,'' as InvNo,'' as VNExportPortID,'' as ExportPort,'' as Dest,'' as CountryAlias,'' as ShipTerm,0 as ShipQty,0 as CTNQty,0 as GW,0 as NW,0 as CMP,
InvNo as InvNo1,OrderID,StyleID,SizeCode,CustomSP,TtlExportQty,FOB
from tmpSumDetail

", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d"), MyUtility.Check.Empty(brand) ? "1=1" : string.Format("BrandID = '{0}'", brand));
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 1)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Msg.WaitWindows("Starting EXCEL...");

            //填內容值
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_P41_Print.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[2];
            int row = 1, sheetcount = 1;
            string invNo = "XXX";
            object[,] objArray = new object[1, 13];
            object[,] objArray1 = new object[1, 7];
            foreach (DataRow dr in printData.Rows)
            {
                if (MyUtility.Convert.GetString(dr["Type"]) == "0")
                {
                    //有幾筆Custom SP就新增幾個WorkSheet
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
                    worksheet = excel.ActiveWorkbook.Worksheets[1];
                    row++;
                    objArray[0, 0] = dr["rno"];
                    objArray[0, 1] = dr["InvNo"];
                    objArray[0, 2] = dr["VNExportPortID"];
                    objArray[0, 3] = dr["ExportPort"];
                    objArray[0, 4] = dr["CountryAlias"];
                    objArray[0, 5] = MyUtility.Check.Empty(dr["CountryAlias"]) ? "" : MyUtility.Convert.GetString(dr["CountryAlias"]).Substring(0, 1);
                    objArray[0, 6] = MyUtility.Check.Empty(dr["CountryAlias"]) ? "" : MyUtility.Convert.GetString(dr["CountryAlias"]).Substring(1, 1);
                    objArray[0, 7] = dr["ShipTerm"];
                    objArray[0, 8] = dr["ShipQty"];
                    objArray[0, 9] = dr["CTNQty"];
                    objArray[0, 10] = dr["GW"];
                    objArray[0, 11] = dr["NW"];
                    objArray[0, 12] = dr["CMP"];
                    worksheet.Range[String.Format("A{0}:M{0}", row)].Value2 = objArray;
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
                        worksheet.Name = MyUtility.Convert.GetString(dr["InvNo1"]); //更改Sheet Name
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

                    worksheet.Range[String.Format("A{0}:G{0}", row)].Value2 = objArray1;
                }
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            //最後顯示前將Sheet切換到第一頁
            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Select();
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            excel.Visible = true;


            MyUtility.Msg.WaitClear();
            return true;
        }
    }
}
