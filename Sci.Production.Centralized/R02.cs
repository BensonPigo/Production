using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Sci.Data;
using Ict;
using Sci.Production.Prg;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;
using Sci.Production.Report.GSchemas;
using System.Xml.Linq;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private string temfile;
        private Excel.Application excel = null;

        private decimal? gdclYear = 0;

        /// <summary>
        /// R02
        /// </summary>
        public R02()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Text = PrivUtilsPMS.GetVersion(this.Text);
            this.print.Visible = false;
            this.numericUpDown1.Value = Convert.ToInt32(DateTime.Today.ToString("yyyy"));
            this.gdclYear = Convert.ToInt32(DateTime.Today.ToString("yyyy"));
            this.Text = PrivUtilsPMS.GetVersion(this.Text);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            DualResult result = Ict.Result.True;
            if (this.excel == null)
            {
                return true;
            }

            this.ShowInfo("Completed.");
            #region Save & Show Excel
            Workbook workbook = this.excel.Workbooks[1];
            string strExcelName = Class.MicrosoftFile.GetName("Centralized_R02.CPULoadingReport", Class.ExcelFileNameExtension.Xlsm);
            workbook.SaveAs(strExcelName, XlFileFormat.xlOpenXMLWorkbookMacroEnabled);
            workbook.Close();
            this.excel.Quit();
            Marshal.ReleaseComObject(this.excel);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void SetRangeMerageCell(Range rngCell)
        {
            rngCell.MergeCells = true;
            rngCell.HorizontalAlignment = HorizontalAlignment.Center;
            rngCell.VerticalAlignment = HorizontalAlignment.Center;
        }

        private void SetRangeLineStyle(Range rngCell)
        {
            rngCell.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
            rngCell.Borders[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            try
            {
                string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                DualResult result = new DualResult(true);
                System.Data.DataTable dtData = null, dtMaster = null;

                #region strSQL_Loading
                string strSQL_Loading = string.Format(@"
SELECT  Orders.ID 
        , CONVERT(varchar(10), Orders.SCIDelivery, 111) SCIDelivery
        , Factory.KPICode 
        , Orders.FactoryID 
        , Factory.CountryID 
        , Orders.ProgramID 
        , Orders.OrderTypeID 
        , Orders.Qty 
        , Orders.CPU 
        , CPURate = (Select cpurate 
                     from dbo.GetCPURate (Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'O' )) 
        , TotalCPU = (Orders.Qty 
                      * (Select cpurate 
                         from dbo.GetCPURate (Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'O' )) 
                      * Orders.Cpu)  
        , Orders.Category 
        , LoadingMonth = Month (Dateadd (dd, -7, Orders.SCIDelivery)) 
        , CDCode.ComboPcs 
        , nm_Category = case 
                            when Orders.Category = 'S' then 'Sample' 
                            when Orders.Category = 'B' then 'Bulk' 
                            when Orders.Category = 'M' then 'Material' 
                            when Orders.Category = 'T' then 'SMTL' 
                            else Orders.Category 
                        end
FROM ORDERS 
left join Factory on  Factory.ID = Orders.FactoryID 
left join CDCode on Orders.CDCodeID = CDCode.ID 
WHERE   1=1 
        and Orders.Category NOT IN ('G','A')
        and Orders.LocalOrder = 0
        and Factory.IsProduceFty = '1'");
                string strSQL_Loading_b = strSQL_Loading;
                strSQL_Loading += $@" 
        and Orders.Category IN ('B', 'S')
        and Factory.KPICode <> ''  
        and Orders.SCIDELIVERY BETWEEN '{this.gdclYear}/01/08' AND '{this.gdclYear + 1}/01/07' ";

                if (!this.chkIncludeCancelOrder.Checked)
                {
                    strSQL_Loading += " and Orders.Junk = 0 ";
                }

                if (this.txtFactory1.Text != string.Empty)
                {
                    strSQL_Loading += string.Format(@"  and Factory.KPICode = '{0}'  ", this.txtFactory1.Text);
                }

                if (this.txtCountry1.TextBox1.Text != string.Empty)
                {
                    strSQL_Loading += string.Format(@"   and Factory.CountryID = '{0}'  ", this.txtCountry1.TextBox1.Text);
                }
                #endregion
                #region strSQL_Output
                string strSQL_Output = string.Format(
                    @" 
Select  ID = IIF (LoadingList.ID is null, SewingOutput.ID, LoadingList.ID)
        , SewingOutput.OutputDate 
        ,  Factory.KPICode 
        , SewingOutput.FactoryID
        , Factory.CountryID 
        , SewingOutput_Detail_Detail.Article 
        , SewingOutput_Detail_Detail.SizeCode 
        , SewingOutput_Detail_Detail.QAQty 
        , LoadingList.cpu 
        , LoadingList.cpurate
        , SuitRate = dbo.GetSuitRate (LoadingList.ComboPcs, SewingOutput_Detail_Detail.ComboType)
        , TotalCPU = LoadingList.CPU 
                     * SewingOutput_Detail_Detail.QAQty 
                     * CPURate 
                     * dbo.GetSuitRate (LoadingList.ComboPcs, SewingOutput_Detail_Detail.ComboType)
        , SewingOutput_Detail_Detail.ComboType 
        , OutputMonth = Month(SewingOutput.OutputDate) 
From SewingOutput_Detail_Detail 
inner join Orders on SewingOutput_Detail_Detail.OrderID = orders.ID
left join SewingOutput on SewingOutput.ID = SewingOutput_Detail_Detail.ID
left join Factory on Factory.ID = SewingOutput.FactoryID 
left join ( {0} )  LoadingList on LoadingList.ID = SewingOutput_Detail_Detail.OrderId
WHERE   1=1
        and orders.Category NOT IN ('G','A')
        and Sewingoutput.Shift <> 'I'
        and Factory.IsProduceFty = '1'
        and Factory.KPICode <> ''
        and SewingOutput.OutputDate between '{1}/01/01' and '{1}/12/31'",
                    strSQL_Loading_b,
                    this.gdclYear);
                if (this.txtFactory1.Text != string.Empty)
                {
                    strSQL_Output += string.Format(
                        @" 
        and Factory.KPICode = '{0}'  ", this.txtFactory1.Text);
                }

                if (this.txtCountry1.TextBox1.Text != string.Empty)
                {
                    strSQL_Output += string.Format(@"    and Factory.CountryID = '{0}'  ", this.txtCountry1.TextBox1.Text);
                }
                #endregion
                #region strSQL_Master
                string strSQL_Master = string.Format(
                    @" 
SELECT  *
FROM (
    Select  Sum(TotalCpu ) as SumLoadingCPU 
            , KPICode
            , LoadingMonth
            , '0' AS STRTYPE 
    from ( 
        {0} 
    ) LoadingList 
    Group by KPICode, LoadingMonth
    
    UNION ALL 
    Select  Sum(TotalCPU) as SumLoadingCPU  
            , KPICode
            , OutputMonth AS LoadingMonth
            , '1' AS STRTYPE 
    from (
        {1} 
    ) OutputList 
    Group by KPICode, OutputMonth
) AAA  
ORDER BY KPICode, LoadingMonth, STRTYPE",
                    strSQL_Loading,
                    strSQL_Output); // where and LoadingMonth='01'
                #endregion
                #region --由 appconfig 抓各個連線路徑
                this.SetLoadingText("Load connections... ");
                XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
                string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
                List<string> connectionString = new List<string>();
                foreach (string ss in strSevers)
                {
                    #region 選擇單一工廠時，只需保留該工廠的連線
                    if (!this.txtFactory1.Text.ToString().Empty())
                    {
                        /*
                         * 將 strSevers 切割成 0 : 連線 1 : 連線中所有的工廠
                         */
                        string[] m = ss.Split(new char[] { ':' });
                        if (m.Count() > 1)
                        {
                            /*
                             * 判斷該連線中，是否有與畫面中相同的工廠名稱
                             */
                            string[] mFactory = m[1].Split(new char[] { ',' });
                            if (!mFactory.AsEnumerable().Any(f => f.EqualString(this.txtFactory1.Text.ToString())))
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    #endregion
                    var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                    connectionString.Add(connections);
                }

                if (connectionString == null || connectionString.Count == 0)
                {
                    return new DualResult(false, "no connection loaded.");
                }
                #endregion
                #region set dtData & dtMaster
                for (int i = 0; i < connectionString.Count; i++)
                {
                    System.Data.DataTable connDtData, connDtMaster;
                    string conString = connectionString[i];

                    // 跳提示視窗顯示跑到第幾筆連線
                    this.SetLoadingText(
                        string.Format(
                            "Load data from connection {0}/{1} ",
                            i + 1,
                            connectionString.Count));
                    SqlConnection conn;
                    using (conn = new SqlConnection(conString))
                    {
                        #region tmpOrder1

                        result = DBProxy.Current.SelectByConn(conn, strSQL_Master, out connDtData);
                        if (!result)
                        {
                            return result;
                        }

                        if ((connDtData != null) && (connDtData.Rows.Count > 0))
                        {
                            if (dtData != null)
                            {
                                dtData.Merge(connDtData);
                            }
                            else
                            {
                                dtData = connDtData;
                            }
                        }
                        #endregion tmpOrder1
                        string strSQL_1 = @"
Select  A = Factory.KpiCode 
        , B = SPACE(20)
        , C = 0.00
        , D = 0.00
        , E = 0.00
        , F = 0.00
        , G = 0.00
        , H = 0.00
        , I = 0.00
        , J = 0.00
        , K = 0.00
        , L = 0.00
        , M = 0.00
        , N = 0.00
        , O = 0.00
from Orders WITH (NOLOCK) 
Left Join Factory WITH (NOLOCK) on Orders.FactoryID = Factory.ID 
WHERE 1 = 0 ";
                        result = DBProxy.Current.SelectByConn(conn, strSQL_1, out connDtMaster);
                        if (!result)
                        {
                            return result;
                        }

                        if (dtMaster != null)
                        {
                            dtMaster.Merge(connDtMaster);
                        }
                        else
                        {
                            dtMaster = connDtMaster;
                        }
                    }
                }
                #endregion

                if ((dtData == null) || (dtData.Rows.Count == 0))
                {
                    return new DualResult(false, "查不到任何資料！");
                }

                List<string> lstKeys = new List<string>();

                DataRow drMaster_Loding = null, drMaster_Output = null, drMaster_Variation = null, drMaster_AccVariation = null;
                Helper ghelper = new Helper();
                for (int intIndex = 0; intIndex < dtData.Rows.Count; intIndex++)
                {
                    #region initial drMaster_Loding, drMaster_Output, drMaster_Variation, drMaster_AccVariation;
                    int intIndex_R = lstKeys.IndexOf(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Loading");
                    if (intIndex_R >= 0)
                    {
                        drMaster_Loding = dtMaster.Rows[intIndex_R];
                        drMaster_Output = dtMaster.Rows[intIndex_R + 1];
                        drMaster_Variation = dtMaster.Rows[intIndex_R + 2];
                        drMaster_AccVariation = dtMaster.Rows[intIndex_R + 3];
                    }
                    else
                    {
                        drMaster_Loding = dtMaster.NewRow();
                        drMaster_Loding["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_Loding["B"] = "Loading";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_Loding[intIndex_C] = 0;
                        }

                        drMaster_Output = dtMaster.NewRow();
                        drMaster_Output["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_Output["B"] = "Output";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_Output[intIndex_C] = 0;
                        }

                        drMaster_Variation = dtMaster.NewRow();
                        drMaster_Variation["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_Variation["B"] = "Variation";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_Variation[intIndex_C] = 0;
                        }

                        drMaster_AccVariation = dtMaster.NewRow();
                        drMaster_AccVariation["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_AccVariation["B"] = "AccVariation";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_AccVariation[intIndex_C] = 0;
                        }

                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Loading");
                        intIndex_R = lstKeys.Count - 1; // 下一個 Factory 的起始index
                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Output");
                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Variation");
                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_AccVariation");
                        dtMaster.Rows.Add(drMaster_Loding);
                        dtMaster.Rows.Add(drMaster_Output);
                        dtMaster.Rows.Add(drMaster_Variation);
                        dtMaster.Rows.Add(drMaster_AccVariation);
                    }
                    #endregion initial drMaster_Loding, drMaster_Output, drMaster_Variation, drMaster_AccVariation;
                    decimal? orderCapacity = null;
                    orderCapacity = decimal.Parse(dtData.Rows[intIndex]["SumLoadingCPU"].ToString() == string.Empty ? "0" : dtData.Rows[intIndex]["SumLoadingCPU"].ToString());
                    if (dtData.Rows[intIndex]["STRTYPE"].ToString() == "0")
                    {
                        drMaster_Loding[Convert.ToInt32(dtData.Rows[intIndex]["LoadingMonth"].ToString()) + 1] = Convert.ToDecimal(drMaster_Loding[Convert.ToInt32(dtData.Rows[intIndex]["LoadingMonth"].ToString()) + 1]) + orderCapacity;
                    }
                    else if (dtData.Rows[intIndex]["STRTYPE"].ToString() == "1")
                    {
                        drMaster_Output[Convert.ToInt32(dtData.Rows[intIndex]["LoadingMonth"].ToString()) + 1] = Convert.ToDecimal(drMaster_Output[Convert.ToInt32(dtData.Rows[intIndex]["LoadingMonth"].ToString()) + 1]) + orderCapacity;
                    }
                }

                int intIndex_M = 0;
                foreach (string sKey in lstKeys)
                {
                    if (sKey.Contains("_Loading"))
                    {
                        if (dtMaster.Rows[intIndex_M]["B"].ToString() == "Loading")
                        {
                            drMaster_Loding = dtMaster.Rows[intIndex_M];
                        }

                        if (dtMaster.Rows[intIndex_M + 1]["B"].ToString() == "Output")
                        {
                            drMaster_Output = dtMaster.Rows[intIndex_M + 1];
                        }

                        if (dtMaster.Rows[intIndex_M + 2]["B"].ToString() == "Variation")
                        {
                            drMaster_Variation = dtMaster.Rows[intIndex_M + 2];
                        }

                        if (dtMaster.Rows[intIndex_M + 3]["B"].ToString() == "AccVariation")
                        {
                            drMaster_AccVariation = dtMaster.Rows[intIndex_M + 3];
                        }

                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_Variation[intIndex_C] = Convert.ToDecimal(drMaster_Output[intIndex_C].ToString()) - Convert.ToDecimal(drMaster_Loding[intIndex_C].ToString());
                            switch (intIndex_C)
                            {
                                case 4:
                                case 7:
                                case 10:
                                case 13:
                                    drMaster_AccVariation[intIndex_C] = Convert.ToDecimal(drMaster_Variation[intIndex_C - 2].ToString()) + Convert.ToDecimal(drMaster_Variation[intIndex_C - 1].ToString()) + Convert.ToDecimal(drMaster_Variation[intIndex_C].ToString());
                                    break;
                            }
                        }

                        decimal kk = 0;
                        for (int intIndex_C = 2; intIndex_C < 14; intIndex_C++)
                        {
                            kk += Convert.ToDecimal(drMaster_Loding[intIndex_C].ToString());
                            drMaster_Loding[14] = Convert.ToDecimal(drMaster_Loding[14].ToString()) + Convert.ToDecimal(drMaster_Loding[intIndex_C].ToString());
                            drMaster_Output[14] = Convert.ToDecimal(drMaster_Output[14].ToString()) + Convert.ToDecimal(drMaster_Output[intIndex_C].ToString());
                            drMaster_Variation[14] = Convert.ToDecimal(drMaster_Variation[14].ToString()) + Convert.ToDecimal(drMaster_Variation[intIndex_C].ToString());
                        }

                        intIndex_M += 4;
                    }
                }
                #region Export Sum Data
                string strPath = PrivUtilsPMS.GetPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
                this.temfile = strPath + @"\Centralized_R02.CPULoadingReport.xltm";

                string[] aryHeaders = new string[] { "Factory", "Data", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Total" };
                if (!(result = PrivUtilsPMS.Excels.CreateExcel(this.temfile, out this.excel)))
                {
                    return result;
                }

                Worksheet wsSheet = this.excel.Workbooks[1].Worksheets[1];
                wsSheet.Name = "Summary";

                for (int intIndex_e = 0; intIndex_e < aryHeaders.Length; intIndex_e++)
                {
                    wsSheet.Cells[1, intIndex_e + 1].Value = aryHeaders[intIndex_e];
                    this.SetRangeLineStyle(wsSheet.Range[aryAlpha[intIndex_e] + "1"]);
                    if (intIndex_e == 1)
                    {
                        wsSheet.Range[aryAlpha[intIndex_e] + "1"].ColumnWidth = 16;
                    }
                }

                for (int intIndex = 0; intIndex < dtMaster.Rows.Count; intIndex++)
                {
                    for (int intIndex_C = 0; intIndex_C < dtMaster.Columns.Count; intIndex_C++)
                    {
                        if ((intIndex_C == 0) && (dtMaster.Rows[intIndex][intIndex_C + 1].ToString() != "Loading"))
                        {
                            wsSheet.Cells[intIndex + 2, intIndex_C + 1].Value = string.Empty;
                        }
                        else
                        {
                            wsSheet.Cells[intIndex + 2, intIndex_C + 1].Value = dtMaster.Rows[intIndex][intIndex_C].ToString();
                        }

                        this.SetRangeLineStyle(wsSheet.Range[aryAlpha[intIndex_C] + (intIndex + 2).ToString()]);
                        if ((intIndex_C == 0) && (dtMaster.Rows[intIndex][intIndex_C + 1].ToString() == "AccVariation"))
                        {
                            this.SetRangeMerageCell(wsSheet.Range["A" + (intIndex - 1).ToString(), "A" + (intIndex + 2).ToString()]);
                        }
                    }
                }

                wsSheet.Range[string.Format("B:{0}", PrivUtilsPMS.GetPosition(aryHeaders.Length))].WrapText = false;
                wsSheet.get_Range(string.Format("B:{0}", PrivUtilsPMS.GetPosition(aryHeaders.Length))).EntireColumn.AutoFit();
                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(aryHeaders.Length))].HorizontalAlignment = Constants.xlCenter;
                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(aryHeaders.Length))].Interior.Color = 13434828; // 10092441;
                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(aryHeaders.Length))].AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlFilterValues);
                #endregion Export Sum Data

                if (this.checkBox1.Checked)
                {
                    int intSheetIndex = 2;
                    #region Export Loading Detail
                    dtData = null;
                    for (int i = 0; i < connectionString.Count; i++)
                    {
                        System.Data.DataTable connDtData;
                        string conString = connectionString[i];

                        // 跳提示視窗顯示跑到第幾筆連線
                        this.SetLoadingText(
                            string.Format(
                                "Load data from connection {0}/{1} ",
                                i + 1,
                                connectionString.Count));
                        SqlConnection conn;
                        using (conn = new SqlConnection(conString))
                        {
                            result = DBProxy.Current.SelectByConn(conn, strSQL_Loading, out connDtData);
                            if (!result)
                            {
                                return result;
                            }
                            else if (connDtData != null && connDtData.Rows.Count > 0)
                            {
                                if (dtData == null)
                                {
                                    dtData = connDtData;
                                }
                                else
                                {
                                    dtData.Merge(connDtData);
                                }
                            }
                        }
                    }

                    if ((dtData != null) && (dtData.Rows.Count != 0))
                    {
                        this.ExportDetailData(ref intSheetIndex, dtData, "Loading Detail. ", 0);
                    }
                    #endregion Export Loading Detail
                    #region Export OutPut Detail
                    dtData = null;
                    for (int i = 0; i < connectionString.Count; i++)
                    {
                        System.Data.DataTable connDtData;
                        string conString = connectionString[i];

                        // 跳提示視窗顯示跑到第幾筆連線
                        this.SetLoadingText(
                            string.Format("Load data from connection {0}/{1} ", i + 1, connectionString.Count));
                        SqlConnection conn;
                        using (conn = new SqlConnection(conString))
                        {
                            result = DBProxy.Current.SelectByConn(conn, strSQL_Output, out connDtData);
                            if (!result)
                            {
                                return result;
                            }
                            else if (connDtData != null && connDtData.Rows.Count > 0)
                            {
                                if (dtData == null)
                                {
                                    dtData = connDtData;
                                }
                                else
                                {
                                    dtData.Merge(connDtData);
                                }
                            }
                        }
                    }

                    if ((dtData != null) && (dtData.Rows.Count != 0))
                    {
                        this.ExportDetailData(ref intSheetIndex, dtData, "OutPut Detail. ", 1);
                    }
                    #endregion Export OutPut Detail
                }

                return result;
            }
            catch (Exception eEE)
            {
                return new DualResult(false, eEE);
            }
        }

        private void ExportDetailData(ref int intSheetindex, System.Data.DataTable dtData, string strSheetName, int intDetailType)
        {
            #region Export Detail Data
            if (this.checkBox1.Checked)
            {
                Worksheet wsSheet = null;

                // #region Export Detail Data
                string[] aryHeaders_D = new string[] { "SP No", "SCI Delivery", "KPI Group", "Factory", "Country", "Program", "OrderType", "Qty", "CPU", "Rate", "Total Cpu", "Category" };
                string[] aryFields_D = new string[] { "ID", "SCIDelivery", "KPICode", "FactoryID", "CountryID", "ProgramID", "OrderTypeID", "Qty", "CPU", "CPURate", "TotalCPU", "nm_Category" };
                string[] aryHeaders_D1 = new string[] { "SP No", "Output Date", "Factory Group", "Factory", "Country", "Color", "Size", "QAQty", "CPU", "Rate", "SuitRate ", "Total Cpu", "EType" };
                string[] aryFields_D1 = new string[] { "ID", "OutputDate", "KPICode", "FactoryID", "CountryID", "Article", "SizeCode", "QAQty", "CPU", "CPURate", "SuitRate", "TotalCPU", "ComboType" };

                #region 將資料放入陣列並寫入Excel範例檔
                int intRowsCount = dtData.Rows.Count;
                int intRowsStart = 2; // 匯入起始位置
                int rownum = intRowsStart; // 每筆資料匯入之位置
                int intColumns = aryHeaders_D.Length; // 匯入欄位數
                if (intDetailType == 1)
                {
                    intColumns = aryHeaders_D1.Length; // 匯入欄位數
                }

                object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                if (rownum + intRowsCount > 33000)
                {
                    objArray = new object[33000, intColumns]; // 每列匯入欄位區間
                }

                int intsheet = intSheetindex;
                if (this.excel.Workbooks[1].Worksheets.Count <= intsheet - 1)
                {
                    wsSheet = this.excel.Workbooks[1].Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, XlSheetType.xlWorksheet);
                    Worksheet wsSheet_o = this.excel.ActiveWorkbook.Worksheets[this.excel.Workbooks[1].Worksheets.Count];
                    ((Worksheet)wsSheet).Move(Type.Missing, wsSheet_o);
                }
                else
                {
                    wsSheet = this.excel.Workbooks[1].Worksheets[intsheet];
                }

                int intSheetIndex_0 = 1;
                wsSheet.Name = strSheetName + " " + intSheetIndex_0.ToString();

                // 欄位 title
                for (int intIndex_0 = 0; intIndex_0 < intColumns; intIndex_0++)
                {
                    if (intDetailType == 0)
                    {
                        objArray[0, intIndex_0] = aryHeaders_D[intIndex_0];
                    }
                    else
                    {
                        objArray[0, intIndex_0] = aryHeaders_D1[intIndex_0];
                    }
                }

                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray;
                wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].NumberFormatLocal = "@";
                wsSheet.Range[string.Format("B:B")].NumberFormatLocal = "yyyy/m/d"; // "yyyy/MM/dd";
                if (intDetailType == 0)
                {
                    wsSheet.Range[string.Format("H:H")].NumberFormatLocal = "#,##0";
                    wsSheet.Range[string.Format("I:I")].NumberFormatLocal = "#,##0.000";
                    wsSheet.Range[string.Format("J:J")].NumberFormatLocal = "#,##0.00";
                    wsSheet.Range[string.Format("K:K")].NumberFormatLocal = "#,##0.00";
                }
                else
                {
                    wsSheet.Range[string.Format("H:H")].NumberFormatLocal = "#,##0";
                    wsSheet.Range[string.Format("I:I")].NumberFormatLocal = "#,##0.000";
                    wsSheet.Range[string.Format("J:J")].NumberFormatLocal = "#,##0.00";
                    wsSheet.Range[string.Format("K:K")].NumberFormatLocal = "#,##0.00";
                    wsSheet.Range[string.Format("L:L")].NumberFormatLocal = "#,##0.000";
                }

                // 欄位Format 主要針對儲存格格式設定過日期,數值,文字,但未設定到位置之範例檔
                PrivUtilsPMS.Excels.SetFormat(wsSheet, rownum);
                int ii = 0, jj = 0;
                int intRowsCountT = intRowsCount; // 用於筆數超過一頁(65536)
                for (int i = 0; i < intRowsCountT; i += 1)
                {
                    DataRow dr = dtData.Rows[i];
                    if (i > 0 && (rownum + ii + jj) % PrivUtilsPMS.GetPageNum() == 0)
                    {
                        wsSheet.Range[string.Format("A{0}:{2}{1}", rownum + jj, rownum + jj + ii - 1, PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray;

                        // 欄寬調整
                        wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                        wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                        wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].Interior.Color = 13434828; // 10092441;
                        wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlFilterValues);

                        ii = 0;
                        jj = 0;
                        intsheet++;
                        if (this.excel.Workbooks[1].Worksheets.Count <= intsheet - 1)
                        {
                            wsSheet = this.excel.Workbooks[1].Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, XlSheetType.xlWorksheet);
                            Worksheet wsSheet_o = this.excel.ActiveWorkbook.Worksheets[this.excel.Workbooks[1].Worksheets.Count];
                            ((Worksheet)wsSheet).Move(Type.Missing, wsSheet_o);
                        }
                        else
                        {
                            wsSheet = this.excel.Workbooks[1].Worksheets[intsheet];
                        }

                        intSheetIndex_0++;
                        wsSheet.Name = strSheetName + " " + intSheetIndex_0.ToString();

                        // 欄寬調整
                        wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                        wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                        wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].Interior.Color = 13434828; // 10092441;
                        wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlFilterValues);

                        // 欄位 title
                        for (int intIndex_0 = 0; intIndex_0 < intColumns; intIndex_0++)
                        {
                            if (intDetailType == 0)
                            {
                                objArray[0, intIndex_0] = aryHeaders_D[intIndex_0];
                            }
                            else
                            {
                                objArray[0, intIndex_0] = aryHeaders_D1[intIndex_0];
                            }
                        }

                        wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray;
                        wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].NumberFormatLocal = "@";
                        wsSheet.Range[string.Format("B:B")].NumberFormatLocal = "yyyy/m/d"; // "yyyy/MM/dd";
                        if (intDetailType == 0)
                        {
                            wsSheet.Range[string.Format("H:H")].NumberFormatLocal = "#,##0";
                            wsSheet.Range[string.Format("I:I")].NumberFormatLocal = "#,##0.000";
                            wsSheet.Range[string.Format("J:J")].NumberFormatLocal = "#,##0.00";
                            wsSheet.Range[string.Format("K:K")].NumberFormatLocal = "#,##0.00";
                        }
                        else
                        {
                            wsSheet.Range[string.Format("H:H")].NumberFormatLocal = "#,##0";
                            wsSheet.Range[string.Format("I:I")].NumberFormatLocal = "#,##0.000";
                            wsSheet.Range[string.Format("J:J")].NumberFormatLocal = "#,##0.00";
                            wsSheet.Range[string.Format("K:K")].NumberFormatLocal = "#,##0.00";
                            wsSheet.Range[string.Format("L:L")].NumberFormatLocal = "#,##0.000";
                        }

                        // 欄位Format 主要針對儲存格格式設定過日期,數值,文字,但未設定到位置之範例檔
                        PrivUtilsPMS.Excels.SetFormat(wsSheet, rownum);
                    }

                    for (int k = 0; k < intColumns; k++)
                    {
                        objArray[ii, k] = string.Empty;
                    }

                    for (int k = 0; k < intColumns; k++)
                    {
                        if (intDetailType == 0)
                        {
                            objArray[ii, k] = dr[aryFields_D[k]];
                        }
                        else
                        {
                            objArray[ii, k] = dr[aryFields_D1[k]];
                        }
                    }

                    if (ii == 32999)
                    {
                        int rownumjj = rownum + jj;
                        jj += ii;
                        ii = 0;
                        object[,] objArray1 = new object[33000, intColumns]; // 每列匯入欄位區間
                        objArray1 = objArray;
                        wsSheet.Range[string.Format("A{0}:{2}{1}", rownumjj, rownum + jj - 1, PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray1;
                    }

                    ii++;
                }

                intRowsCount = ii; // 實際寫入筆數
                wsSheet.Range[string.Format("A{0}:{2}{1}", rownum + jj, rownum + jj + intRowsCount - 1, PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray;

                // 欄寬調整
                wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].Interior.Color = 13434828; // 10092441;
                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(intColumns))].AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlFilterValues);

                // 欄位Focus
                PrivUtilsPMS.Excels.SetPosition_Focus(wsSheet, rownum);
                #endregion
                intSheetindex = intsheet + 1;
            }
            #endregion Export Detail Data
        }

        /// <summary>
        /// 備份舊的程式碼… 2016.10.17 BLAKE
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected DualResult OnAsyncDataLoad_BK(ReportEventArgs e)
        {
            try
            {
                string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                DualResult result = new DualResult(true);
                System.Data.DataTable dtData, dtMaster, dtTradeSystem;

                #region tmpOrder1
                string strSQL = string.Format(
                    @"
Select  Orders.ID
        , Orders.FactoryID 
        , Orders.OrderTypeID 
        , Orders.ProgramID 
        , CPURate = (select CpuRate 
                     from GetCPURate (Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID,'')) 
        --, CPURate = 0.00  
        , Orders.CPU 
        , Orders.Qty as OrderQty 
        , OrderCapacity = (Orders.CPU 
                           * Orders.Qty 
                           * (select CpuRate 
                              from GetCPURate (Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID,'')))
        --, OrderCapacity = 0.00 
        , OrderYYMM = CONVERT (VARCHAR, DATEPART (YEAR, DATEADD (DAY, -7, Orders.SCIDelivery))) 
                      + REPLICATE ('0', 2 - LEN (CONVERT (VARCHAR, DATEPART (MONTH, DATEADD(DAY, -7, Orders.SCIDelivery))))) 
                      + CONVERT (VARCHAR, DATEPART (MONTH, DATEADD (DAY, -7, Orders.SCIDelivery)))
        , SCIDelivery = Convert(varchar(10),Orders.SCIDelivery,111)
        , FactoryCountryID  = Factory.CountryID
        , Factory.KpiCode
        , nm_Category = case 
                            when Orders.Category = 'S' then 'Sample' 
                            when Orders.Category = 'B' then 'Bulk' 
                            when Orders.Category = 'M' then 'Material' 
                            when Orders.Category = 'T' then 'SMTL' 
                            else Orders.Category 
                        end
        , Orders.BrandID
        , Orders.Category 
from Orders WITH (NOLOCK) 
Left Join Factory WITH (NOLOCK) on Orders.FactoryID = Factory.ID 
WHERE DATEPART(YEAR, DATEADD(DAY, -7, Orders.SCIDelivery)) = '{0}'
      and Orders.Category NOT IN ('G','A') ", this.gdclYear);
                if (this.txtFactory1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND Orders.FactoryID = '{0}'  ", this.txtFactory1.Text);
                }

                if (this.txtCountry1.TextBox1.Text != string.Empty)
                {
                    strSQL += string.Format(" AND Factory.CountryID = '{0}'  ", this.txtCountry1.TextBox1.Text);
                }

                result = DBProxy.Current.Select(null, strSQL, out dtData);
                if (!result)
                {
                    return result;
                }

                if ((dtData == null) || (dtData.Rows.Count == 0))
                {
                    return new DualResult(false, "查不到任何資料！");
                }
                #endregion tmpOrder1
                strSQL = @"
Select  A = Factory.KpiCode 
        , B = SPACE(20)
        , C = 0.00  
        , D = 0.00 
        , E = 0.00
        , F = 0.00
        , G = 0.00
        , H = 0.00
        , I = 0.00
        , J = 0.00
        , K = 0.00
        , L = 0.00
        , M = 0.00
        , N = 0.00
        , O = 0.00
from Orders WITH (NOLOCK) 
Left Join Factory WITH (NOLOCK) on Orders.FactoryID = Factory.ID 
WHERE 1 = 0 ";
                result = DBProxy.Current.Select(null, strSQL, out dtMaster);
                if (!result)
                {
                    return result;
                }

                strSQL = @"Select * from TradeSystem ";
                result = DBProxy.Current.Select(null, strSQL, out dtTradeSystem);
                if (!result)
                {
                    return result;
                }

                System.Data.DataTable dt_ref = null;
                strSQL = string.Format(
                    @"
Select  OrderID
        , Sewingoutput_Detail.QAQty 
        , Sewingoutput_Detail.InlineQty 
        , Sewingoutput.OutputDate 
        --SewingYYMM (依據OutputDate來決定月份落點; 日期的定義請參考前面說明) 
        , SewingYYMM = CONVERT (VARCHAR, DATEPART (YEAR, DATEADD (DAY, -7, Sewingoutput.OutputDate))) 
                       + REPLICATE ('0', 2 - LEN (CONVERT (VARCHAR, DATEPART (MONTH, DATEADD (DAY, -7, Sewingoutput.OutputDate))))) 
                       + CONVERT (VARCHAR, DATEPART (MONTH, DATEADD (DAY, -7, Sewingoutput.OutputDate)))
        --, (CPU * QAQty * CPURate ) as SewCapacity
        , 0.00 as SewCapacity
from Sewingoutput WITH (NOLOCK)
inner join Sewingoutput_Detail WITH (NOLOCK) on Sewingoutput.ID = Sewingoutput_Detail.ID 
inner join Orders With(NoLock) on SewingOutput_Detail.OrderID = orders.ID
where   orders.Category NOT IN ('G','A')
        and DATEPART (YEAR, DATEADD (DAY, -7, Sewingoutput.OutputDate)) = '{0}'  
        --Sewingoutput_Detail.OrderID = '{0}'
                                                            ", this.gdclYear);

                result = DBProxy.Current.Select(null, strSQL, out dt_ref);
                if (!result)
                {
                    return result;
                }

                IDictionary<string, IList<DataRow>> id_to_Sewingoutput_Detail = dt_ref.ToDictionaryList((x) => x.Val<string>("OrderID"));

                List<string> lstKeys = new List<string>();
                DataRow drMaster_Loding, drMaster_Output, drMaster_Variation, drMaster_AccVariation;
                Helper ghelper = new Helper();
                for (int intIndex = 0; intIndex < dtData.Rows.Count; intIndex++)
                {
                    #region initial drMaster_Loding, drMaster_Output, drMaster_Variation, drMaster_AccVariation;
                    int intIndex_R = lstKeys.IndexOf(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Loading");
                    if (intIndex_R >= 0)
                    {
                        drMaster_Loding = dtMaster.Rows[intIndex_R];
                        drMaster_Output = dtMaster.Rows[intIndex_R + 1];
                        drMaster_Variation = dtMaster.Rows[intIndex_R + 2];
                        drMaster_AccVariation = dtMaster.Rows[intIndex_R + 3];
                    }
                    else
                    {
                        drMaster_Loding = dtMaster.NewRow();
                        drMaster_Loding["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_Loding["B"] = "Loading";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_Loding[intIndex_C] = 0;
                        }

                        drMaster_Output = dtMaster.NewRow();
                        drMaster_Output["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_Output["B"] = "Output";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_Output[intIndex_C] = 0;
                        }

                        drMaster_Variation = dtMaster.NewRow();
                        drMaster_Variation["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_Variation["B"] = "Variation";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_Variation[intIndex_C] = 0;
                        }

                        drMaster_AccVariation = dtMaster.NewRow();
                        drMaster_AccVariation["A"] = dtData.Rows[intIndex]["KpiCode"].ToString();
                        drMaster_AccVariation["B"] = "AccVariation";
                        for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                        {
                            drMaster_AccVariation[intIndex_C] = 0;
                        }

                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Loading");
                        intIndex_R = lstKeys.Count - 1;
                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Output");
                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_Variation");
                        lstKeys.Add(dtData.Rows[intIndex]["KpiCode"].ToString() + "_AccVariation");
                        dtMaster.Rows.Add(drMaster_Loding);
                        dtMaster.Rows.Add(drMaster_Output);
                        dtMaster.Rows.Add(drMaster_Variation);
                        dtMaster.Rows.Add(drMaster_AccVariation);
                    }
                    #endregion initial drMaster_Loding, drMaster_Output, drMaster_Variation, drMaster_AccVariation;
                    decimal? mRate = 1, orderCapacity = null;

                    orderCapacity = decimal.Parse(dtData.Rows[intIndex]["OrderCapacity"].ToString() == string.Empty ? "0" : dtData.Rows[intIndex]["OrderCapacity"].ToString());
                    drMaster_Loding[Convert.ToInt32(dtData.Rows[intIndex]["OrderYYMM"].ToString().Substring(4, 2)) + 1] = Convert.ToDecimal(drMaster_Loding[Convert.ToInt32(dtData.Rows[intIndex]["OrderYYMM"].ToString().Substring(4, 2)) + 1]) + orderCapacity;

                    if (id_to_Sewingoutput_Detail.ContainsKey(dtData.Rows[intIndex]["ID"].ToString()))
                    {
                        decimal? dclSewCapacity = 0;
                        foreach (DataRow dr_temp in id_to_Sewingoutput_Detail[dtData.Rows[intIndex]["ID"].ToString()])
                        {
                            dclSewCapacity = Convert.ToDecimal(dtData.Rows[intIndex]["CPU"].ToString()) * Convert.ToDecimal(dr_temp["QAQty"].ToString()) * mRate;
                            drMaster_Output[Convert.ToInt32(dr_temp["SewingYYMM"].ToString().Substring(4, 2)) + 1] = Convert.ToDecimal(drMaster_Output[Convert.ToInt32(dr_temp["SewingYYMM"].ToString().Substring(4, 2)) + 1]) + dclSewCapacity;
                        }
                    }

                    for (int intIndex_C = 2; intIndex_C < 15; intIndex_C++)
                    {
                        drMaster_Variation[intIndex_C] = Convert.ToDecimal(drMaster_Output[intIndex_C].ToString()) - Convert.ToDecimal(drMaster_Loding[intIndex_C].ToString());
                        switch (intIndex_C)
                        {
                            case 4:
                            case 7:
                            case 10:
                            case 13:
                                drMaster_AccVariation[intIndex_C] = Convert.ToDecimal(drMaster_Variation[intIndex_C - 2].ToString()) + Convert.ToDecimal(drMaster_Variation[intIndex_C - 1].ToString()) + Convert.ToDecimal(drMaster_Variation[intIndex_C].ToString());
                                break;
                        }
                    }

                    for (int intIndex_C = 2; intIndex_C < 14; intIndex_C++)
                    {
                        drMaster_Loding[14] = Convert.ToDecimal(drMaster_Loding[14].ToString()) + Convert.ToDecimal(drMaster_Loding[intIndex_C].ToString());
                        drMaster_Output[14] = Convert.ToDecimal(drMaster_Output[14].ToString()) + Convert.ToDecimal(drMaster_Output[intIndex_C].ToString());
                        drMaster_Variation[14] = Convert.ToDecimal(drMaster_Variation[14].ToString()) + Convert.ToDecimal(drMaster_Variation[intIndex_C].ToString());
                    }
                }
                #region Export Sum Data
                string strPath = PrivUtilsPMS.GetPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
                this.temfile = strPath + @"\Centralized-R02.CPULoadingReport.xltm";

                string[] aryHeaders = new string[] { "Factory", "Data", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "Total" };
                if (!(result = PrivUtilsPMS.Excels.CreateExcel(this.temfile, out this.excel)))
                {
                    return result;
                }

                Worksheet wsSheet = this.excel.Workbooks[1].Worksheets[1];
                wsSheet.Name = "Summary";

                for (int intIndex_e = 0; intIndex_e < aryHeaders.Length; intIndex_e++)
                {
                    wsSheet.Cells[1, intIndex_e + 1].Value = aryHeaders[intIndex_e];
                    this.SetRangeLineStyle(wsSheet.Range[aryAlpha[intIndex_e] + "1"]);
                    if (intIndex_e == 1)
                    {
                        wsSheet.Range[aryAlpha[intIndex_e] + "1"].ColumnWidth = 16;
                    }
                }

                for (int intIndex = 0; intIndex < dtMaster.Rows.Count; intIndex++)
                {
                    for (int intIndex_C = 0; intIndex_C < dtMaster.Columns.Count; intIndex_C++)
                    {
                        if ((intIndex_C == 0) && (dtMaster.Rows[intIndex][intIndex_C + 1].ToString() != "Loading"))
                        {
                            wsSheet.Cells[intIndex + 2, intIndex_C + 1].Value = string.Empty;
                        }
                        else
                        {
                            wsSheet.Cells[intIndex + 2, intIndex_C + 1].Value = dtMaster.Rows[intIndex][intIndex_C].ToString();
                        }

                        this.SetRangeLineStyle(wsSheet.Range[aryAlpha[intIndex_C] + (intIndex + 2).ToString()]);
                        if ((intIndex_C == 0) && (dtMaster.Rows[intIndex][intIndex_C + 1].ToString() == "AccVariation"))
                        {
                            this.SetRangeMerageCell(wsSheet.Range["A" + (intIndex - 1).ToString(), "A" + (intIndex + 2).ToString()]);
                        }
                    }
                }

                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(aryHeaders.Length))].Interior.Color = 13434828; // 10092441;
                wsSheet.Range[string.Format("A1:{0}1", PrivUtilsPMS.GetPosition(aryHeaders.Length))].AutoFilter(1, Type.Missing, XlAutoFilterOperator.xlFilterValues);

                #endregion Export Sum Data

                this.excel.Workbooks[1].Worksheets[2].Visible = false;
                if (this.checkBox1.Checked)
                {
                    this.excel.Workbooks[1].Worksheets[2].Visible = true;

                    string[] aryHeaders_D = new string[] { "SP No", "SCI Delivery", "KPI Group", "Factory", "Country", "Program", "OrderType", "Qty", "Rate", "Total Cpu", "Category" };
                    string[] aryFields_D = new string[] { "ID", "SCIDelivery", "KPICode", "FactoryID", "FactoryCountryID", "ProgramID", "OrderTypeID", "OrderQty", "CPURate", "OrderCapacity", "nm_Category" };

                    #region 將資料放入陣列並寫入Excel範例檔
                    int intRowsCount = dtData.Rows.Count;
                    int intRowsStart = 2; // 匯入起始位置
                    int rownum = intRowsStart; // 每筆資料匯入之位置
                    int intColumns = 11; // 匯入欄位數
                    object[,] objArray = new object[intRowsCount, intColumns]; // 每列匯入欄位區間
                    if (rownum + intRowsCount > 33000)
                    {
                        objArray = new object[33000, intColumns]; // 每列匯入欄位區間
                    }

                    // 欄位Format 主要針對儲存格格式設定過日期,數值,文字,但未設定到位置之範例檔
                    PrivUtilsPMS.Excels.SetFormat(wsSheet, rownum);
                    int intsheet = 2;
                    if (this.excel.Workbooks[1].Worksheets.Count <= 1)
                    {
                        wsSheet = this.excel.Workbooks[1].Worksheets.Add(Type.Missing, wsSheet == null ? Type.Missing : wsSheet, Type.Missing, XlSheetType.xlWorksheet);
                    }
                    else
                    {
                        wsSheet = this.excel.Workbooks[1].Worksheets[intsheet];
                    }

                    wsSheet.Name = "Detail Sheet. 1";

                    int ii = 0, jj = 0;
                    int intRowsCountT = intRowsCount; // 用於筆數超過一頁(65536)

                    for (int i = 0; i < intRowsCountT; i += 1)
                    {
                        DataRow dr = dtData.Rows[i];
                        if (i > 0 && (rownum + ii + jj) % PrivUtilsPMS.GetPageNum() == 0)
                        {
                            wsSheet.Range[string.Format("A{0}:{2}{1}", rownum + jj, rownum + jj + ii - 1, PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray;

                            // 欄寬調整
                            wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                            wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();
                            ii = 0;
                            Worksheet wsSheet_o = this.excel.ActiveWorkbook.Worksheets[intsheet];
                            ((Worksheet)wsSheet_o).Select();
                            wsSheet.Copy(wsSheet_o);
                            intsheet = this.excel.ActiveWorkbook.Worksheets.Count;
                            ((Worksheet)wsSheet_o).Move(this.excel.Worksheets[intsheet - 1]); // copy 的sheet 會產生在前 , 因此原本的Sheet要往前移
                            wsSheet = this.excel.ActiveWorkbook.Worksheets[intsheet];
                            wsSheet.Name = "Detail Sheet. " + (intsheet - 1).ToString();
                            ((Worksheet)wsSheet).Select();
                            Range formatRange1 = wsSheet.get_Range(string.Format("A{0}:{2}{1}", rownum, PrivUtilsPMS.GetPageNum() - 1, PrivUtilsPMS.GetPosition(intColumns)));
                            formatRange1.Select();
                            formatRange1.Delete(XlDeleteShiftDirection.xlShiftUp);
                        }

                        for (int k = 0; k < intColumns; k++)
                        {
                            objArray[ii, k] = string.Empty;
                        }

                        for (int k = 0; k < intColumns; k++)
                        {
                            objArray[ii, k] = dr[aryFields_D[k]];
                        }

                        ii++;
                        if (ii == 32999)
                        {
                            int rownumjj = rownum + jj;
                            jj += ii;
                            ii = 0;
                            object[,] objArray1 = new object[33000, intColumns]; // 每列匯入欄位區間
                            objArray1 = objArray;
                            wsSheet.Range[string.Format("A{0}:{2}{1}", rownumjj, rownum + jj - 1, PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray1;
                        }
                    }

                    intRowsCount = ii; // 實際寫入筆數
                    wsSheet.Range[string.Format("A{0}:{2}{1}", rownum + jj, rownum + jj + intRowsCount - 1, PrivUtilsPMS.GetPosition(intColumns))].Value2 = objArray;

                    // 欄寬調整
                    wsSheet.Range[string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(string.Format("A:{0}", PrivUtilsPMS.GetPosition(intColumns))).EntireColumn.AutoFit();

                    // 欄位Focus
                    PrivUtilsPMS.Excels.SetPosition_Focus(wsSheet, rownum);
                    #endregion
                }

                return result;
            }
            catch (Exception eEE)
            {
                return new DualResult(false, eEE);
            }
        }

        private void NumericBox1_Leave(object sender, EventArgs e)
        {
            this.gdclYear = this.numericUpDown1.Value;
        }
    }
}
