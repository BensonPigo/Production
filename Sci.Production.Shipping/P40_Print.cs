﻿using System;
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
    public partial class P40_Print : Sci.Win.Tems.PrintForm
    {
        string type;
        DataTable printData, summaryData;
        DataRow masterData;

        public P40_Print(DataRow MasterData)
        {
            InitializeComponent();
            radioButton1.Checked = true;
            masterData = MasterData;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            type = radioButton1.Checked ? "1" : "2";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;
            if (type == "1")
            {
                sqlCmd.Append(string.Format(@"select idd.ID,idd.NLCode,idd.Qty,idd.UnitID,idd.Price,nd.DescEN,vc.SubConName,vc.SubConAddress,f.NameEN,f.AddressEN,c.Alias
from VNImportDeclaration_Detail idd
inner join VNImportDeclaration id on idd.ID = id.ID
left join VNNLCodeDesc nd on idd.NLCode = nd.NLCode
left join VNContract vc on vc.ID = '{0}'
left join Factory f on f.ID = '{1}'
left join Country c on id.FromSite = c.ID
where idd.ID = '{2}'
order by idd.NLCode", MyUtility.Convert.GetString(masterData["VNContractID"]), Sci.Env.User.Keyword, MyUtility.Convert.GetString(masterData["ID"])));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }

                sqlCmd.Clear();
                sqlCmd.Append(string.Format(@"select sum(Qty) as Qty,UnitID
from VNImportDeclaration_Detail
where ID = '{0}'
group by UnitID", MyUtility.Convert.GetString(masterData["ID"])));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out summaryData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query summary data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            else
            {
                sqlCmd.Append(string.Format(@"select HSCode,NLCode,Qty,UnitID,Price,Round(Qty*Price,2) as Amount,Remark
from VNImportDeclaration_Detail
where ID = '{0}'", MyUtility.Convert.GetString(masterData["ID"])));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + (type == "1" ? "\\Shipping_P40_CommercialInvoice.xltx" : "\\Shipping_P40_FormForCustomSystem.xltx");
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            if (type == "1")
            {
                worksheet.Cells[1, 1] = MyUtility.Convert.GetString(printData.Rows[0]["SubConName"]);
                worksheet.Cells[2, 1] = MyUtility.Convert.GetString(printData.Rows[0]["SubConAddress"]);
                worksheet.Cells[4, 2] = MyUtility.Convert.GetString(masterData["ID"]);
                worksheet.Cells[4, 5] = Convert.ToDateTime(masterData["CDate"]).ToString("d");
                worksheet.Cells[6, 2] = MyUtility.Convert.GetString(printData.Rows[0]["NameEN"]);
                worksheet.Cells[7, 2] = MyUtility.Convert.GetString(printData.Rows[0]["AddressEN"]);
                worksheet.Cells[8, 2] = MyUtility.Convert.GetString(masterData["ShipModeID"]);
                worksheet.Cells[8, 4] = MyUtility.GetValue.Lookup(string.Format("select Vessel from {0} where {1}", MyUtility.Convert.GetString(masterData["IsFtyExport"]).ToUpper() == "TRUE" ? "FtyExport" : "Export", MyUtility.Check.Empty(masterData["BLNo"]) ? "ID = '" + MyUtility.Convert.GetString(masterData["WKNo"]) + "'" : "BLNo = '" + MyUtility.Convert.GetString(masterData["BLNo"]) + "'"));
                worksheet.Cells[9, 1] = "COUNTRY OF ORIGIN : " + MyUtility.Convert.GetString(printData.Rows[0]["Alias"]);
                worksheet.Cells[9, 4] = MyUtility.Convert.GetString(printData.Rows[0]["Alias"]);

                int intRowsStart = 11;
                decimal totalAmount = 0;
                object[,] objArray = new object[1, 6];
                foreach (DataRow dr in printData.Rows)
                {
                    objArray[0, 0] = intRowsStart-10;
                    objArray[0, 1] = dr["DescEN"];
                    objArray[0, 2] = dr["Qty"];
                    objArray[0, 3] = dr["UnitID"];
                    objArray[0, 4] = dr["Price"];
                    objArray[0, 5] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["Price"]),2);
                    
                    worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].Value2 = objArray;

                    //畫線
                    DrowLine(worksheet, intRowsStart);

                    totalAmount = totalAmount + MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["Qty"]) * MyUtility.Convert.GetDecimal(dr["Price"]), 2);
                    intRowsStart++;
                }

                for (int j = 0; j < 3; j++)
                {
                    //畫線
                    DrowLine(worksheet, intRowsStart);
                   
                    intRowsStart++;
                }

                int totalStart = intRowsStart;
                object[,] objArray1 = new object[1, 6];
                foreach (DataRow dr in summaryData.Rows)
                {
                    objArray1[0, 0] = "";
                    objArray1[0, 1] = "";
                    objArray1[0, 2] = dr["Qty"];
                    objArray1[0, 3] = dr["UnitID"];
                    objArray1[0, 4] = "";
                    objArray1[0, 5] = "";
                    worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].Value2 = objArray1;

                    //畫線
                    DrowLine(worksheet, intRowsStart);
                   
                    worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].Font.Bold = true;
                    intRowsStart++;
                }
                //合併儲存格
                worksheet.Range[String.Format("A{0}:A{1}", MyUtility.Convert.GetString(totalStart), MyUtility.Convert.GetString(intRowsStart-1))].Merge(Type.Missing);
                worksheet.Cells[totalStart, 1] = "TOTAL";
                worksheet.Range[String.Format("B{0}:B{1}", MyUtility.Convert.GetString(totalStart), MyUtility.Convert.GetString(intRowsStart - 1))].Merge(Type.Missing);
                worksheet.Range[String.Format("E{0}:E{1}", MyUtility.Convert.GetString(totalStart), MyUtility.Convert.GetString(intRowsStart - 1))].Merge(Type.Missing);
                worksheet.Range[String.Format("F{0}:F{1}", MyUtility.Convert.GetString(totalStart), MyUtility.Convert.GetString(intRowsStart - 1))].Merge(Type.Missing);
                worksheet.Cells[totalStart, 6] = MyUtility.Convert.GetString(totalAmount);
                
                //畫線
                DrowLine(worksheet, intRowsStart);
                
                intRowsStart = intRowsStart + 2;

                worksheet.Range[String.Format("A{0}:F{0}", MyUtility.Convert.GetString(intRowsStart))].Merge(Type.Missing);
                worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].Font.Bold = true;
                worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].Font.Italic = true;
                worksheet.Range[String.Format("A{0}:A{0}", intRowsStart)].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;  //文字靠左顯示

                worksheet.Cells[intRowsStart, 1] = "Declare total : " + MyUtility.Convert.USDMoney(totalAmount);
            }
            else
            {
                int intRowsStart = 2;
                object[,] objArray = new object[1, 18];
                foreach (DataRow dr in printData.Rows)
                {
                    objArray[0, 0] = dr["NLCode"];
                    objArray[0, 1] = "";
                    objArray[0, 2] = dr["HSCode"];
                    objArray[0, 3] = masterData["FromSite"];
                    objArray[0, 4] = "NEW";
                    objArray[0, 5] = dr["Qty"];
                    objArray[0, 6] = dr["UnitID"];
                    objArray[0, 7] = dr["Price"];
                    objArray[0, 8] = dr["Amount"];
                    objArray[0, 9] = "B61";
                    objArray[0, 10] = "0";
                    objArray[0, 11] = "B41";
                    objArray[0, 12] = "0";
                    objArray[0, 13] = "B21";
                    objArray[0, 14] = "0";
                    objArray[0, 15] = "B51";
                    objArray[0, 16] = "0";
                    objArray[0, 17] = dr["Remark"];
                    worksheet.Range[String.Format("A{0}:R{0}", intRowsStart)].Value2 = objArray;
                    intRowsStart++;
                }
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }

        //Excel畫線
        private void DrowLine(Microsoft.Office.Interop.Excel.Worksheet worksheet, int row)
        {
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).Weight = 2; //1: 虛線, 2:實線, 3:粗體線
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop).LineStyle = 1;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).Weight = 2;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom).LineStyle = 1;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).Weight = 2;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = 1;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).Weight = 2;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = 1;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal).Weight = 2;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = 1;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical).Weight = 2;
            worksheet.Range[String.Format("A{0}:F{0}", row)].Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical).LineStyle = 1;
        }
    }
}
