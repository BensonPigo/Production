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
using System.IO;

namespace Sci.Production.Shipping
{
    public partial class B42_Print : Sci.Win.Tems.PrintForm
    {
        string reportType, customSP1, customSP2;
        DateTime? date1, date2;
        DataTable printData;
        int recCount;
        public B42_Print()
        {
            InitializeComponent();
            radioButton1.Checked = true;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2))
            {
                MyUtility.Msg.WarningBox("Date can empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(textBox1.Text) != MyUtility.Check.Empty(textBox2.Text))
            {
                MyUtility.Msg.WarningBox("Custom SP# can empty!!");
                if (MyUtility.Check.Empty(textBox1.Text))
                {
                    textBox1.Focus();
                }
                else
                {
                    textBox2.Focus();
                }
                return false;
            }

            date1 = dateRange1.Value1;
            date2 = dateRange1.Value2;
            customSP1 = textBox1.Text;
            customSP2 = textBox2.Text;
            reportType = radioButton1.Checked ? "1" : radioButton2.Checked ? "2" : "3";

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;
            if (reportType == "1")
            {
                sqlCmd.Append(string.Format(@"select vcd.NLCode,vcd.Qty,isnull(vd.Waste,0)*100 as Waste,'' as Orignal,vc.CustomSP,vc.VNContractID
from VNConsumption vc
inner join VNConsumption_Detail vcd on vc.ID = vcd.ID
left join VNContract_Detail vd on vc.VNContractID = vd.ID and vcd.NLCode = vd.NLCode
where vc.CDate between '{0}' and '{1}'
and vc.Status = 'Confirmed'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
                if (!MyUtility.Check.Empty(customSP1))
                {
                    sqlCmd.Append(string.Format(" and vc.CustomSP between '{0}' and '{1}'", customSP1, customSP2));
                }
                sqlCmd.Append(" order by CONVERT(int,SUBSTRING(vcd.NLCode,3,3))");

                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            else if (reportType == "2")
            {
                sqlCmd.Append(string.Format(@"select count(CustomSP) as RecCount
from VNConsumption
where Status = 'Confirmed'
and CDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
                if (!MyUtility.Check.Empty(customSP1))
                {
                    sqlCmd.Append(string.Format(" and CustomSP between '{0}' and '{1}'", customSP1, customSP2));
                }
                recCount = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlCmd.ToString()));
                sqlCmd.Clear();
                sqlCmd.Append(string.Format(@"select vc.VNContractID,v.StartDate,v.EndDate,isnull(v.SubConName,'') as SubConName,
isnull(v.SubConAddress,'') as SubConAddress,isnull(v.TotalQty,0) as TotalQty,
vc.CustomSP,vc.Qty as GMTQty,isnull(vn.DescVI,'') as DescVI,isnull(vcd.NLCode,'') as NLCode,
isnull(vn.UnitVI,'') as UnitVI,isnull(vcd.Qty,0) as Qty,isnull(vd.Waste,0)*100 as Waste,
isnull(IIF(vd.LocalPurchase = 1,(select vn.DescVI from VNNLCodeDesc where NLCode = 'VNBUY'),(select vn.DescVI from VNNLCodeDesc where NLCode = 'NOVNBUY')),'') as Original,
isnull(s.Picture1,'') as Picture1,isnull(s.Picture2,'') as Picture2,(select PicPath from System) as PicPath,vc.StyleID
from VNConsumption vc
left join VNConsumption_Detail vcd on vcd.ID = vc.ID
left join VNContract v on v.ID = vc.VNContractID
left join VNContract_Detail vd on vd.ID = vc.VNContractID and vd.NLCode = vcd.NLCode
left join VNNLCodeDesc vn on vn.NLCode = vcd.NLCode
left join Style s on s.Ukey = vc.StyleUKey
where vc.Status = 'Confirmed'
and vc.CDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
                if (!MyUtility.Check.Empty(customSP1))
                {
                    sqlCmd.Append(string.Format(" and vc.CustomSP between '{0}' and '{1}'", customSP1, customSP2));
                }

                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            else
            {
                sqlCmd.Append(string.Format(@"Select ROW_NUMBER() OVER (ORDER BY v.CustomSP) as STT,v.StyleID, v.SeasonID, v.Version, 
v.BrandID, v.Category,isnull(s.Type,'') as StyleType, isnull(r1.Name,'') as FabricType,
isnull(r2.Name,'') as ApparelType, isnull(s.Lining,'') as Lining,v.SizeCode, v.CustomSP,
v.Qty,isnull(s.StyleUnit,'') as StyleUnit, (v.CPU*v.VNMultiple) as CMP,(v.CPU*v.VNMultiple*v.Qty) as TtlCMP,
[dbo].getOrderUnitPrice(1,v.StyleUKey,'','',v.SizeCode) as FOB,
[dbo].getOrderUnitPrice(1,v.StyleUKey,'','',v.SizeCode)*v.Qty as TtlFOB
from VNConsumption v
left join Style s on s.Ukey = v.StyleUKey
left join Reason r1 on r1.ReasonTypeID = 'Fabric_Kind' and r1.ID = s.FabricType
left join Reason r2 on r2.ReasonTypeID = 'Style_Apparel_Type' and r2.ID = s.ApparelType
where v.Status = 'Confirmed' 
and v.CDate between '{0}' and '{1}'", Convert.ToDateTime(date1).ToString("d"), Convert.ToDateTime(date2).ToString("d")));
                if (!MyUtility.Check.Empty(customSP1))
                {
                    sqlCmd.Append(string.Format(" and v.CustomSP between '{0}' and '{1}'", customSP1, customSP2));
                }

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

            //填內容值
            if (reportType == "1")
            {
                bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: "Shipping_B42_FormForCustomSystem.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            else if (reportType == "2")
            {
                string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_B42_EachConsumption.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null) return false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                //有幾筆Custom SP就新增幾個WorkSheet
                for (int i = 1; i < recCount; i++)
                {
                    worksheet.Copy(Type.Missing, worksheet);
                }
                worksheet.Select();
                string customSP = MyUtility.Convert.GetString(printData.Rows[0]["CustomSP"]);
                worksheet.Name = MyUtility.Convert.GetString(printData.Rows[0]["CustomSP"]) + "-" + MyUtility.Convert.GetString(printData.Rows[0]["StyleID"]); //更改Sheet Name
                int customSPCount = 1;
                int stt = 0;
                string picPath = "",pic1 = "", pic2 = "";
                object[,]  objArray = new object[1, 10];
                foreach (DataRow dr in printData.Rows)
                {
                    if (customSP != MyUtility.Convert.GetString(dr["CustomSP"]))
                    {
                        //刪除多的一行
                        Microsoft.Office.Interop.Excel.Range rngToDelete = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(stt + 14)), Type.Missing).EntireRow;
                        rngToDelete.Delete(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        //貼圖
                        if (!MyUtility.Check.Empty(pic1) && File.Exists(picPath + pic1))
                        {
                            string excelRng1 = string.Format("B{0}",MyUtility.Convert.GetString(stt+20));
                            Microsoft.Office.Interop.Excel.Range rngToInsert1 = worksheet.get_Range(excelRng1, Type.Missing);
                            rngToInsert1.Select();
                            float PicLeft, PicTop;
                            PicLeft = Convert.ToSingle(rngToInsert1.Left);
                            PicTop = Convert.ToSingle(rngToInsert1.Top);
                            string targetFile = picPath+pic1;
                            worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, 450, 400);
                        }
                        if (!MyUtility.Check.Empty(pic2) && File.Exists(picPath + pic2))
                        {
                            string excelRng2 = string.Format("F{0}", MyUtility.Convert.GetString(stt + 20));
                            Microsoft.Office.Interop.Excel.Range rngToInsert2 = worksheet.get_Range(excelRng2, Type.Missing);
                            rngToInsert2.Select();
                            float PicLeft, PicTop;
                            PicLeft = Convert.ToSingle(rngToInsert2.Left);
                            PicTop = Convert.ToSingle(rngToInsert2.Top);
                            string targetFile = picPath+pic2;
                            worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, 450, 400);
                        }

                        customSPCount++;
                        stt = 0;
                        customSP = MyUtility.Convert.GetString(dr["CustomSP"]);
                        worksheet = excel.ActiveWorkbook.Worksheets[customSPCount];
                        worksheet.Name = MyUtility.Convert.GetString(dr["CustomSP"]) + "-" + MyUtility.Convert.GetString(dr["StyleID"]); //更改Sheet Name
                        worksheet.Select();
                    }
                    if (stt == 0)
                    {
                        worksheet.Cells[3, 3] = MyUtility.Convert.GetString(dr["VNContractID"]);
                        worksheet.Cells[3, 7] = MyUtility.Check.Empty(dr["StartDate"]) ? "" : Convert.ToDateTime(dr["StartDate"]).ToString("d");
                        worksheet.Cells[3, 9] = MyUtility.Check.Empty(dr["EndDate"]) ? "" : Convert.ToDateTime(dr["EndDate"]).ToString("d");
                        worksheet.Cells[5, 2] = "Bªn thuª gia c«ng: " + MyUtility.Convert.GetString(dr["SubConName"]);
                        worksheet.Cells[5, 6] = "§Þa chØ: " + MyUtility.Convert.GetString(dr["SubConAddress"]);
                        worksheet.Cells[7, 7] = MyUtility.Convert.GetString(dr["TotalQty"]);
                        worksheet.Cells[8, 2] = "M· hµng gia c«ng: " + MyUtility.Convert.GetString(dr["CustomSP"]);
                        worksheet.Cells[8, 7] = MyUtility.Convert.GetString(dr["GMTQty"]);
                    }
                    stt++;
                    if (stt > 1)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(stt+13)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                    objArray[0, 0] = stt;
                    objArray[0, 1] = dr["DescVI"];
                    objArray[0, 2] = dr["NLCode"];
                    objArray[0, 3] = dr["UnitVI"];
                    objArray[0, 4] = dr["Qty"];
                    objArray[0, 5] = "";
                    objArray[0, 6] = dr["Waste"];
                    objArray[0, 7] = string.Format("=E{0}+G{0}",MyUtility.Convert.GetString(stt+13));
                    objArray[0, 8] = dr["Original"];
                    objArray[0, 9] = "";
                    worksheet.Range[String.Format("A{0}:J{0}", stt + 13)].Value2 = objArray;
                    pic1 = MyUtility.Convert.GetString(dr["Picture1"]);
                    pic2 = MyUtility.Convert.GetString(dr["Picture2"]);
                    picPath = MyUtility.Convert.GetString(dr["PicPath"]);
                }

                //刪除多的一行
                Microsoft.Office.Interop.Excel.Range rngToDelete1 = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(stt + 14)), Type.Missing).EntireRow;
                rngToDelete1.Delete(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                //貼圖
                if (!MyUtility.Check.Empty(pic1) && File.Exists(picPath + pic1))
                {
                    string excelRng1 = string.Format("B{0}", MyUtility.Convert.GetString(stt + 20));
                    Microsoft.Office.Interop.Excel.Range rngToInsert1 = worksheet.get_Range(excelRng1, Type.Missing);
                    rngToInsert1.Select();
                    float PicLeft, PicTop;
                    PicLeft = Convert.ToSingle(rngToInsert1.Left);
                    PicTop = Convert.ToSingle(rngToInsert1.Top);
                    string targetFile = picPath + pic1;
                    worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, 450, 400);
                }
                if (!MyUtility.Check.Empty(pic2) && File.Exists(picPath + pic2))
                {
                    string excelRng2 = string.Format("F{0}", MyUtility.Convert.GetString(stt + 20));
                    Microsoft.Office.Interop.Excel.Range rngToInsert2 = worksheet.get_Range(excelRng2, Type.Missing);
                    rngToInsert2.Select();
                    float PicLeft, PicTop;
                    PicLeft = Convert.ToSingle(rngToInsert2.Left);
                    PicTop = Convert.ToSingle(rngToInsert2.Top);
                    string targetFile = picPath + pic2;
                    worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, PicLeft, PicTop, 450, 400);
                }

                //最後顯示前將Sheet切換到第一頁
                worksheet = excel.ActiveWorkbook.Worksheets[1];
                worksheet.Select();

                excel.Visible = true;
            }
            else
            {
                bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: "Shipping_B42_ANNEX.xltx", headerRow: 1);
                if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            }
            MyUtility.Msg.WaitClear();
            return true;
        }
    }
}
