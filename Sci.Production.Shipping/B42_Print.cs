using System;
using System.Data;
using System.Text;
using Ict;
using Sci.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B42_Print
    /// </summary>
    public partial class B42_Print : Sci.Win.Tems.PrintForm
    {
        private string reportType;
        private string customSP1;
        private string customSP2;
        private string contractNo;
        private DateTime? date1;
        private DateTime? date2;
        private DataTable printData;
        private int recCount;

        /// <summary>
        /// B42_Print
        /// </summary>
        public B42_Print()
        {
            this.InitializeComponent();
            this.radioFormForCustomSystem.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateDate.Value1) && MyUtility.Check.Empty(this.dateDate.Value2))
            {
                MyUtility.Msg.WarningBox("Date can't be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtCustomSPNoStart.Text) != MyUtility.Check.Empty(this.txtCustomSPNoEnd.Text))
            {
                if (MyUtility.Check.Empty(this.txtCustomSPNoStart.Text))
                {
                    this.txtCustomSPNoStart.Focus();
                }
                else
                {
                    this.txtCustomSPNoEnd.Focus();
                }

                MyUtility.Msg.WarningBox("Custom SP# can't be empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtCustomsContract1.Text))
            {
                MyUtility.Msg.WarningBox("Contract no can't be empty!!");
                return false;
            }

            this.date1 = this.dateDate.Value1;
            this.date2 = this.dateDate.Value2;
            this.customSP1 = this.txtCustomSPNoStart.Text;
            this.customSP2 = this.txtCustomSPNoEnd.Text;
            this.contractNo = this.txtCustomsContract1.Text;
            this.reportType = this.radioFormForCustomSystem.Checked ? "1" : this.radioEachConsumption.Checked ? "2" : "3";

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;

            // Form for custom system
            if (this.reportType == "1")
            {
                sqlCmd.Append(string.Format(@"
select vc.CustomSP,vcd.NLCode,vcd.Qty,isnull(vcd.Waste,0)*100 as Waste,Round(vcd.Qty * (isnull(vcd.Waste,0)+1),4) as CCOA,vncd.DescVI as Remark
from VNConsumption vc WITH (NOLOCK) 
inner join VNConsumption_Detail vcd WITH (NOLOCK) on vc.ID = vcd.ID
left join VNContract_Detail vd WITH (NOLOCK) on vc.VNContractID = vd.ID and vcd.NLCode = vd.NLCode
left join (select iif(NLCode = 'VNBUY' ,1,0) as LocalPurchase,DescVI from VNNLCodeDesc  WITH (NOLOCK)  where NLCode in ('VNBUY','NOVNBUY')) vncd on vd.LocalPurchase = vncd.LocalPurchase
where 1=1 and vc.Status = 'Confirmed'"));

                if (!MyUtility.Check.Empty(this.contractNo))
                {
                    sqlCmd.Append(string.Format(" and vc.VNContractID = '{0}' ", this.contractNo));
                }

                if (!MyUtility.Check.Empty(this.date1))
                {
                    sqlCmd.Append(string.Format(" and vc.CDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.date2))
                {
                    sqlCmd.Append(string.Format(" and vc.CDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.customSP1))
                {
                    sqlCmd.Append(string.Format(" and vc.CustomSP between '{0}' and '{1}'", this.customSP1, this.customSP2));
                }

                sqlCmd.Append(" order by CustomSP, TRY_CONVERT(int, SUBSTRING(vcd.NLCode, 3, LEN(vcd.NLCode))), vcd.NLCode");

                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            // Each consumption
            else if (this.reportType == "2")
            {
                sqlCmd.Append(string.Format(@"
select count(CustomSP) as RecCount
from VNConsumption WITH (NOLOCK) 
where Status = 'Confirmed'
and 1=1"));

                if (!MyUtility.Check.Empty(this.contractNo))
                {
                    sqlCmd.Append(string.Format(" and VNContractID = '{0}' ", this.contractNo));
                }

                if (!MyUtility.Check.Empty(this.date1))
                {
                    sqlCmd.Append(string.Format(" and CDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.date2))
                {
                    sqlCmd.Append(string.Format(" and CDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.customSP1))
                {
                    sqlCmd.Append(string.Format(" and CustomSP between '{0}' and '{1}'", this.customSP1, this.customSP2));
                }

                this.recCount = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlCmd.ToString()));
                sqlCmd.Clear();
                sqlCmd.Append(string.Format(@"
select vc.VNContractID,v.StartDate,v.EndDate,isnull(v.SubConName,'') as SubConName,
isnull(v.SubConAddress,'') as SubConAddress,isnull(v.TotalQty,0) as TotalQty,
vc.CustomSP,vc.Qty as GMTQty,isnull(vn.DescVI,'') as DescVI,isnull(vcd.NLCode,'') as NLCode,
isnull(vn.UnitVI,'') as UnitVI,isnull(vcd.Qty,0) as Qty,isnull(vcd.Waste,0)*100 as Waste,
isnull(IIF(vd.LocalPurchase = 1,(select DescVI from VNNLCodeDesc WITH (NOLOCK) where NLCode = 'VNBUY'),(select DescVI from VNNLCodeDesc WITH (NOLOCK) where NLCode = 'NOVNBUY')),'') as Original,
isnull(s.Picture1,'') as Picture1,isnull(s.Picture2,'') as Picture2,(select StyleSketch from System WITH (NOLOCK) ) as StyleSketch,vc.StyleID
from VNConsumption vc WITH (NOLOCK) 
left join VNConsumption_Detail vcd WITH (NOLOCK) on vcd.ID = vc.ID
left join VNContract v WITH (NOLOCK) on v.ID = vc.VNContractID
left join VNContract_Detail vd WITH (NOLOCK) on vd.ID = vc.VNContractID and vd.NLCode = vcd.NLCode
left join VNNLCodeDesc vn WITH (NOLOCK) on vn.NLCode = vcd.NLCode
left join Style s WITH (NOLOCK) on s.Ukey = vc.StyleUKey
where vc.Status = 'Confirmed'
and 1=1 "));
                if (!MyUtility.Check.Empty(this.contractNo))
                {
                    sqlCmd.Append(string.Format(" and vc.VNContractID = '{0}' ", this.contractNo));
                }

                if (!MyUtility.Check.Empty(this.date1))
                {
                    sqlCmd.Append(string.Format(" and vc.CDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.date2))
                {
                    sqlCmd.Append(string.Format(" and vc.CDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.customSP1))
                {
                    sqlCmd.Append(string.Format(" and vc.CustomSP between '{0}' and '{1}'", this.customSP1, this.customSP2));
                }

                sqlCmd.Append(" order by CustomSP,NLCode");

                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }
            else
            {
                sqlCmd.Append(string.Format(@"
Select ROW_NUMBER() OVER (ORDER BY v.CustomSP) as STT,v.StyleID, v.SeasonID, v.Version, 
v.BrandID, v.Category,isnull(s.Gender,'') as StyleType, isnull(r1.Name,'') as FabricType,
isnull(r2.Name,'') as ApparelType, isnull(s.Lining,'') as Lining,v.SizeCode, v.CustomSP,
v.Qty,isnull(s.StyleUnit,'') as StyleUnit, (v.CPU*v.VNMultiple) as CMP,(v.CPU*v.VNMultiple*v.Qty) as TtlCMP,
[dbo].getOrderUnitPrice(1,v.StyleUKey,'','',v.SizeCode) as FOB,
[dbo].getOrderUnitPrice(1,v.StyleUKey,'','',v.SizeCode)*v.Qty as TtlFOB
from VNConsumption v WITH (NOLOCK) 
left join Style s WITH (NOLOCK) on s.Ukey = v.StyleUKey
left join Reason r1 WITH (NOLOCK) on r1.ReasonTypeID = 'Fabric_Kind' and r1.ID = s.FabricType
left join Reason r2 WITH (NOLOCK) on r2.ReasonTypeID = 'Style_Apparel_Type' and r2.ID = s.ApparelType
where v.Status = 'Confirmed' 
and 1=1"));

                if (!MyUtility.Check.Empty(this.contractNo))
                {
                    sqlCmd.Append(string.Format(" and v.VNContractID = '{0}' ", this.contractNo));
                }

                if (!MyUtility.Check.Empty(this.date1))
                {
                    sqlCmd.Append(string.Format(" and v.CDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.date2))
                {
                    sqlCmd.Append(string.Format(" and v.CDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.customSP1))
                {
                    sqlCmd.Append(string.Format(" and v.CustomSP between '{0}' and '{1}'", this.customSP1, this.customSP2));
                }

                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");

            // 填內容值
            if (this.reportType == "1")
            {
                bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "Shipping_B42_FormForCustomSystem.xltx", headerRow: 1);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                }
            }
            else if (this.reportType == "2")
            {
                string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_B42_EachConsumption.xltx";
                Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
                if (excel == null)
                {
                    return false;
                }

                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                // 有幾筆Custom SP就新增幾個WorkSheet
                for (int i = 1; i < this.recCount; i++)
                {
                    worksheet.Copy(Type.Missing, worksheet);
                }

                worksheet.Select();
                string customSP = MyUtility.Convert.GetString(this.printData.Rows[0]["CustomSP"]);
                worksheet.Name = MyUtility.Convert.GetString(this.printData.Rows[0]["CustomSP"]) + "-" + MyUtility.Convert.GetString(this.printData.Rows[0]["StyleID"]); // 更改Sheet Name
                int customSPCount = 1;
                int stt = 0;
                string StyleSketch = string.Empty, pic1 = string.Empty, pic2 = string.Empty;
                object[,] objArray = new object[1, 10];
                foreach (DataRow dr in this.printData.Rows)
                {
                    if (customSP != MyUtility.Convert.GetString(dr["CustomSP"]))
                    {
                        // 刪除多的一行
                        Microsoft.Office.Interop.Excel.Range rngToDelete = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(stt + 14)), Type.Missing).EntireRow;
                        rngToDelete.Delete(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);

                        // 貼圖
                        if (!MyUtility.Check.Empty(pic1) && File.Exists(StyleSketch + pic1))
                        {
                            string excelRng1 = string.Format("B{0}", MyUtility.Convert.GetString(stt + 20));
                            Microsoft.Office.Interop.Excel.Range rngToInsert1 = worksheet.get_Range(excelRng1, Type.Missing);
                            rngToInsert1.Select();
                            float picLeft, picTop;
                            picLeft = Convert.ToSingle(rngToInsert1.Left);
                            picTop = Convert.ToSingle(rngToInsert1.Top);
                            string targetFile = StyleSketch + pic1;
                            worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft, picTop, 450, 400);
                            Marshal.ReleaseComObject(rngToInsert1);
                        }

                        if (!MyUtility.Check.Empty(pic2) && File.Exists(StyleSketch + pic2))
                        {
                            string excelRng2 = string.Format("F{0}", MyUtility.Convert.GetString(stt + 20));
                            Microsoft.Office.Interop.Excel.Range rngToInsert2 = worksheet.get_Range(excelRng2, Type.Missing);
                            rngToInsert2.Select();
                            float picLeft1, picTop1;
                            picLeft1 = Convert.ToSingle(rngToInsert2.Left);
                            picTop1 = Convert.ToSingle(rngToInsert2.Top);
                            string targetFile = StyleSketch + pic2;
                            worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft1, picTop1, 450, 400);
                            Marshal.ReleaseComObject(rngToInsert2);
                        }

                        customSPCount++;
                        stt = 0;
                        customSP = MyUtility.Convert.GetString(dr["CustomSP"]);
                        worksheet = excel.ActiveWorkbook.Worksheets[customSPCount];

                        // 更改Sheet Name
                        worksheet.Name = MyUtility.Convert.GetString(dr["CustomSP"]) + "-" + MyUtility.Convert.GetString(dr["StyleID"]);
                        worksheet.Select();
                    }

                    if (stt == 0)
                    {
                        worksheet.Cells[3, 3] = MyUtility.Convert.GetString(dr["VNContractID"]);
                        worksheet.Cells[3, 7] = MyUtility.Check.Empty(dr["StartDate"]) ? string.Empty : Convert.ToDateTime(dr["StartDate"]).ToString("d");
                        worksheet.Cells[3, 9] = MyUtility.Check.Empty(dr["EndDate"]) ? string.Empty : Convert.ToDateTime(dr["EndDate"]).ToString("d");
                        worksheet.Cells[5, 2] = "Bªn thuª gia c«ng: " + MyUtility.Convert.GetString(dr["SubConName"]);
                        worksheet.Cells[5, 6] = "§Þa chØ: " + MyUtility.Convert.GetString(dr["SubConAddress"]);
                        worksheet.Cells[7, 7] = MyUtility.Convert.GetString(dr["TotalQty"]);
                        worksheet.Cells[8, 2] = "M· hµng gia c«ng: " + MyUtility.Convert.GetString(dr["CustomSP"]);
                        worksheet.Cells[8, 7] = MyUtility.Convert.GetString(dr["GMTQty"]);
                    }

                    stt++;
                    if (stt > 1)
                    {
                        Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(stt + 13)), Type.Missing).EntireRow;
                        rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                        Marshal.ReleaseComObject(rngToInsert);
                    }

                    objArray[0, 0] = stt;
                    objArray[0, 1] = dr["DescVI"];
                    objArray[0, 2] = dr["NLCode"];
                    objArray[0, 3] = dr["UnitVI"];
                    objArray[0, 4] = dr["Qty"];
                    objArray[0, 5] = string.Empty;
                    objArray[0, 6] = dr["Waste"];
                    objArray[0, 7] = string.Format("=E{0}+ROUND((E{0}*(G{0}/100)),3)", MyUtility.Convert.GetString(stt + 13));
                    objArray[0, 8] = dr["Original"];
                    objArray[0, 9] = string.Empty;
                    worksheet.Range[string.Format("A{0}:J{0}", stt + 13)].Value2 = objArray;
                    pic1 = MyUtility.Convert.GetString(dr["Picture1"]);
                    pic2 = MyUtility.Convert.GetString(dr["Picture2"]);
                    StyleSketch = MyUtility.Convert.GetString(dr["StyleSketch"]);
                }

                // 刪除多的一行
                Microsoft.Office.Interop.Excel.Range rngToDelete1 = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(stt + 14)), Type.Missing).EntireRow;
                rngToDelete1.Delete(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                Marshal.ReleaseComObject(rngToDelete1);

                // 貼圖
                if (!MyUtility.Check.Empty(pic1) && File.Exists(StyleSketch + pic1))
                {
                    string excelRng1 = string.Format("B{0}", MyUtility.Convert.GetString(stt + 20));
                    Microsoft.Office.Interop.Excel.Range rngToInsert1 = worksheet.get_Range(excelRng1, Type.Missing);
                    rngToInsert1.Select();
                    float picLeft2, picTop2;
                    picLeft2 = Convert.ToSingle(rngToInsert1.Left);
                    picTop2 = Convert.ToSingle(rngToInsert1.Top);
                    string targetFile = StyleSketch + pic1;
                    worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft2, picTop2, 450, 400);
                }

                if (!MyUtility.Check.Empty(pic2) && File.Exists(StyleSketch + pic2))
                {
                    string excelRng2 = string.Format("F{0}", MyUtility.Convert.GetString(stt + 20));
                    Microsoft.Office.Interop.Excel.Range rngToInsert2 = worksheet.get_Range(excelRng2, Type.Missing);
                    rngToInsert2.Select();
                    float picLeft3, picTop3;
                    picLeft3 = Convert.ToSingle(rngToInsert2.Left);
                    picTop3 = Convert.ToSingle(rngToInsert2.Top);
                    string targetFile = StyleSketch + pic2;
                    worksheet.Shapes.AddPicture(targetFile, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, picLeft3, picTop3, 450, 400);
                }

                // 最後顯示前將Sheet切換到第一頁
                worksheet = excel.ActiveWorkbook.Worksheets[1];
                worksheet.Select();

                #region Save & Show Excel
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_B42_EachConsumption");
                excel.ActiveWorkbook.SaveAs(strExcelName);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(worksheet);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "Shipping_B42_ANNEX.xltx", headerRow: 1);
                if (!result)
                {
                    MyUtility.Msg.WarningBox(result.ToString(), "Warning");
                }
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
