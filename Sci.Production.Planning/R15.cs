using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Reflection;
using Sci.Production.PublicPrg;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;

namespace Sci.Production.Planning
{
    /// <summary>
    /// R15
    /// </summary>
    public partial class R15 : Win.Tems.PrintForm
    {
        private int sbyindex;
        private string category;
        private string factory;
        private string mdivision;
        private string orderby;
        private string spno1;
        private string spno2;
        private string custcd;
        private string brandid;
        private string styleId;
        private string subprocessID;
        private int subprocessInoutColumnCount;
        private string RFIDProcessLocation;
        private string formParameter;
        private DateTime? sciDelivery1;
        private DateTime? sciDelivery2;
        private DateTime? CustRqsDate1;
        private DateTime? CustRqsDate2;
        private DateTime? BuyerDelivery1;
        private DateTime? BuyerDelivery2;
        private DateTime? CutOffDate1;
        private DateTime? CutOffDate2;
        private DateTime? planDate1;
        private DateTime? planDate2;
        private DateTime? sewingInline1;
        private DateTime? sewingInline2;
        private DataTable printData;
        private DataTable dtArtworkType;
        private StringBuilder artworktypes = new StringBuilder();
        private bool isArtwork;

        /// <summary>
        /// R15
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        /// <param name="formParameter">1:R15 固定某些加工段, 2:R15-1 指定加工段</param>
        public R15(ToolStripMenuItem menuitem, string formParameter)
            : base(menuitem)
        {
            this.formParameter = formParameter;
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboOrderBy, 2, 1, "orderid,SPNO,brandid,Brand");
            this.comboOrderBy.SelectedIndex = 0;
            this.dateBuyerDelivery.Select();
            this.dateBuyerDelivery.Value1 = DateTime.Now;
            this.dateBuyerDelivery.Value2 = DateTime.Now.AddDays(30);
            DataTable dt;
            DBProxy.Current.Select(null, "select sby = 'SP#' union all select sby = 'Acticle / Size' union all select sby = 'By SP# , Line'", out dt);
            MyUtility.Tool.SetupCombox(this.comboBox1, 1, dt);
            this.comboBox1.SelectedIndex = 0;
            this.ReportType = "SP#";
            this.Text = formParameter == "1" ? "R15. WIP" : "R15-1. WIP By Specific Subprocess";
            this.panel1.Visible = formParameter == "2";
            this.chkSubProcessOrder.Visible = formParameter == "2";
            this.comboRFIDProcessLocation1.SetDataSource(false);
            this.comboRFIDProcessLocation1.SelectedIndex = 0;
            this.txtMdivision.Enabled = false;
        }

        /// <summary>
        /// ValidateInput
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (this.formParameter == "2" && MyUtility.Check.Empty(this.txtsubprocess1.Text))
            {
                MyUtility.Msg.WarningBox("Please select <Subprocess>.");
                return false;
            }

            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateCustRQSDate.Value1) &&
                MyUtility.Check.Empty(this.dateSewingInline.Value1) &&
                MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) &&
                MyUtility.Check.Empty(this.dateCutOffDate.Value1) &&
                MyUtility.Check.Empty(this.datePlanDate.Value1) &&
                !this.dateLastSewDate.HasValue &&
                (MyUtility.Check.Empty(this.txtSPNoStart.Text) || MyUtility.Check.Empty(this.txtSPNoEnd.Text)))
            {
                MyUtility.Msg.WarningBox("< SCI Delivery > \r\n< Buyer Delivery > \r\n< Sewing Inline > \r\n< Cut Off Date > \r\n< Cust RQS Date > \r\n< Plan Date > \r\n< SP# > \r\n< Last Sew. Date > \r\ncan't be empty!!");
                return false;
            }

            #region -- 擇一必輸的條件 --
            this.sciDelivery1 = this.dateSCIDelivery.Value1;
            this.sciDelivery2 = this.dateSCIDelivery.Value2;
            this.CustRqsDate1 = this.dateCustRQSDate.Value1;
            this.CustRqsDate2 = this.dateCustRQSDate.Value2;
            this.sewingInline1 = this.dateSewingInline.Value1;
            this.sewingInline2 = this.dateSewingInline.Value2;
            this.BuyerDelivery1 = this.dateBuyerDelivery.Value1;
            this.BuyerDelivery2 = this.dateBuyerDelivery.Value2;
            this.CutOffDate1 = this.dateCutOffDate.Value1;
            this.CutOffDate2 = this.dateCutOffDate.Value2;
            this.planDate1 = this.datePlanDate.Value1;
            this.planDate2 = this.datePlanDate.Value2;
            this.spno1 = this.txtSPNoStart.Text;
            this.spno2 = this.txtSPNoEnd.Text;
            #endregion
            this.brandid = this.txtbrand.Text;
            this.styleId = this.txtStyle.Text;
            this.custcd = this.txtCustCD.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.orderby = this.comboOrderBy.SelectedValue.ToString();
            this.isArtwork = this.checkIncludeArtowkData.Checked;
            this.sbyindex = this.comboBox1.SelectedIndex;
            this.subprocessID = this.txtsubprocess1.Text;

            this.RFIDProcessLocation = this.formParameter == "2" ? this.comboRFIDProcessLocation1.Text : string.Empty;
            if (this.isArtwork)
            {
                DualResult result;
                if (!(result = DBProxy.Current.Select(string.Empty, "select id from dbo.artworktype WITH (NOLOCK) where istms=1 or isprice= 1 order by seq", out this.dtArtworkType)))
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                    return false;
                }

                if (this.dtArtworkType.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Artwork Type data not found, Please inform MIS to check !");
                    return false;
                }

                this.artworktypes.Clear();
                for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                {
                    this.artworktypes.Append(string.Format(@"[{0}],", this.dtArtworkType.Rows[i]["id"].ToString()));
                }
            }

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string artworkTypes_2 = this.isArtwork ? this.artworktypes.ToString().Substring(0, this.artworktypes.ToString().Length - 1) : string.Empty;

            Planning_R15_ViewModel r15_vm = new Planning_R15_ViewModel()
            {
                StartSciDelivery = this.sciDelivery1,
                EndSciDelivery = this.sciDelivery2,
                StartBuyerDelivery = this.BuyerDelivery1,
                EndBuyerDelivery = this.BuyerDelivery2,
                StartSewingInline = this.sewingInline1,
                EndSewingInline = this.sewingInline2,
                StartCutOffDate = this.CutOffDate1,
                EndCutOffDate = this.CutOffDate2,
                StartCustRQSDate = this.CustRqsDate1,
                EndCustRQSDate = this.CustRqsDate2,
                StartPlanDate = this.planDate1,
                EndPlanDate = this.planDate2,
                StartSP = this.spno1,
                EndSP = this.spno2,
                StartLastSewDate = this.dateLastSewDate.DateBox1.Value,
                EndLastSewDate = this.dateLastSewDate.DateBox2.Value,
                StyleID = this.styleId,
                BrandID = this.brandid,
                CustCD = this.custcd,
                MDivisionID = this.mdivision,
                FactoryID = this.factory,
                Category = this.category,
                OrderBy = this.orderby,
                SummaryBy = MyUtility.Convert.GetString(this.sbyindex + 1),
                OnlyShowCheckedSubprocessOrder = this.chkSubProcessOrder.Checked,
                IncludeAtworkData = this.checkIncludeArtowkData.Checked,
                IncludeCancelOrder = this.chkIncludeCancelOrder.Checked,
                FormParameter = this.formParameter,
                ArtworkTypes = artworkTypes_2,
                RFIDProcessLocation = this.FormParameter == "2" ? this.RFIDProcessLocation : string.Empty,
                SubprocessID = this.subprocessID,
                IsBI = false,
            };

            Planning_R15 planning_R15 = new Planning_R15();
            Base_ViewModel resultReport = planning_R15.GetPlanning_R15(r15_vm, this.dtArtworkType);
            if (!resultReport.Result)
            {
                return resultReport.Result;
            }

            this.printData = resultReport.DtArr[0];
            this.subprocessInoutColumnCount = resultReport.DtArr[1].Rows.Count > 0 ? MyUtility.Convert.GetInt(resultReport.DtArr[1].Rows[0]["subprocessInoutColumnCount"]) : 0;

            if (this.formParameter == "1")
            {
                this.RemoveOtherColumn();
            }

            DBProxy.Current.DefaultTimeout = 300;
            return Ict.Result.True;
        }

        /// <summary>
        /// 移除不要看到欄位
        /// </summary>
        private void RemoveOtherColumn()
        {
            string[] columnsToRemove = { "RFID AUT Farm In Qty", "RFID AUT Farm Out Qty", "RFID FM Farm In Qty", "RFID FM Farm Out Qty", "RFID Emboss Farm In Qty", "RFID Emboss Farm Out Qty" };

            foreach (string column in columnsToRemove)
            {
                if (this.printData.Columns.Contains(column))
                {
                    this.printData.Columns.Remove(column);
                }
            }
        }

        /// <summary>
        /// OnToExcel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            if (this.printData.Columns.Count > 16384)
            {
                MyUtility.Msg.WarningBox("Columns of Data is over 16,384 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.printData.Rows.Count + 1 > 1048576)
            {
                MyUtility.Msg.WarningBox("Lines of Data is over 1,048,576 in excel file, please narrow down range of condition.");
                return false;
            }

            if (this.sbyindex == 0)
            {
                // by SP
                string filename = this.FormParameter == "1" ? "Planning_R15_WIP" : "Planning_R15_WIP_SingleSubprocess";
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{filename}.xltx"); // 預先開啟excel app
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                if (this.FormParameter == "2")
                {
                    if (this.subprocessInoutColumnCount > 1)
                    {
                        for (int i = 1; i < this.subprocessInoutColumnCount; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range range = objSheets.get_Range("AY1").EntireColumn;
                            range.EntireColumn.Insert(
                                Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight,
                                Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                        }
                    }

                    for (int i = 0; i < this.subprocessInoutColumnCount; i++)
                    {
                        objSheets.Cells[1, 50 + i] = this.printData.Columns[49 + i].ColumnName;
                    }
                }

                if (this.isArtwork)
                {
                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, this.printData.Columns.Count - this.dtArtworkType.Rows.Count + i + 1] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }
                }

                // 首列資料篩選
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{filename}.xltx", 1, false, null, objApp);      // 將datatable copy to excel

                objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                // 客製化欄位，記得設定this.IsSupportCopy = true
                this.CreateCustomizedExcel(ref objSheets);

                // 移除 CD Code欄位
                objSheets.get_Range("V:V").EntireColumn.Delete();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Planning_R15_WIP");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(firstRow);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            else if (this.sbyindex == 1)
            {
                // by Article Size
                string filename = this.FormParameter == "1" ? "Planning_R15_WIP_byArticleSize" : "Planning_R15_WIP_byArticleSize_SingleSubprocess";
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{filename}.xltx"); // 預先開啟excel app
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                if (this.FormParameter == "2")
                {
                    if (this.subprocessInoutColumnCount > 1)
                    {
                        for (int i = 1; i < this.subprocessInoutColumnCount; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range range = objSheets.get_Range("AZ1").EntireColumn;
                            range.EntireColumn.Insert(
                                Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight,
                                Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                        }
                    }

                    for (int i = 0; i < this.subprocessInoutColumnCount; i++)
                    {
                        objSheets.Cells[1, 51 + i] = this.printData.Columns[50 + i].ColumnName;
                    }
                }

                if (this.isArtwork)
                {
                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, this.printData.Columns.Count - this.dtArtworkType.Rows.Count + i + 1] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }
                }

                // 首列資料篩選
                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{filename}.xltx", 1, false, null, objApp);      // 將datatable copy to excel

                // 客製化欄位，記得設定this.IsSupportCopy = true
                this.CreateCustomizedExcel(ref objSheets);

                // 移除 CD Code欄位
                objSheets.get_Range("V:V").EntireColumn.Delete();

                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName("Planning_R15_WIP_byArticleSize");
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(firstRow);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }
            else
            {
                string filename = this.FormParameter == "1" ? "Planning_R15_WIP_bySPLine" : "Planning_R15_WIP_bySPLine_SingleSubprocess";
                Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"{filename}.xltx");
                Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

                if (this.FormParameter == "2")
                {
                    for (int i = 1; i < this.subprocessInoutColumnCount; i++)
                    {
                        if (this.subprocessInoutColumnCount > 1)
                        {
                            Microsoft.Office.Interop.Excel.Range range = objSheets.get_Range("AZ1").EntireColumn;
                            range.EntireColumn.Insert(
                                Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftToRight,
                                Microsoft.Office.Interop.Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                        }
                    }

                    for (int i = 0; i < this.subprocessInoutColumnCount; i++)
                    {
                        objSheets.Cells[1, 51 + i] = this.printData.Columns[50 + i].ColumnName;
                    }
                }

                MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{filename}.xltx", 1, false, null, objApp);
                if (this.isArtwork)
                {
                    // 列印動態欄位的表頭
                    for (int i = 0; i < this.dtArtworkType.Rows.Count; i++)
                    {
                        objSheets.Cells[1, this.printData.Columns.Count - this.dtArtworkType.Rows.Count + i + 1] = this.dtArtworkType.Rows[i]["id"].ToString();
                    }
                }

                Microsoft.Office.Interop.Excel.Range firstRow = (Microsoft.Office.Interop.Excel.Range)objSheets.Rows[1];
                firstRow.AutoFilter(1, Type.Missing, Microsoft.Office.Interop.Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                objApp.Cells.EntireColumn.AutoFit();  // 自動欄寬

                // 客製化欄位，記得設定this.IsSupportCopy = true
                this.CreateCustomizedExcel(ref objSheets);

                // 移除 CD Code欄位
                objSheets.get_Range("V:V").EntireColumn.Delete();
                #region Save & Show Excel
                string strExcelName = Class.MicrosoftFile.GetName(filename);
                Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
                workbook.SaveAs(strExcelName);
                workbook.Close();
                objApp.Quit();
                Marshal.ReleaseComObject(objApp);
                Marshal.ReleaseComObject(objSheets);
                Marshal.ReleaseComObject(firstRow);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion
            }

            return true;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.comboBox1.SelectedIndex)
            {
                case 0:
                         this.ReportType = "SP#";
                         break;
                case 1:
                         this.ReportType = "Acticle / Size";
                         break;
                default:
                         break;
            }
        }
    }
}
