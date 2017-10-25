using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Reflection;
using Microsoft.Office.Interop.Excel;

using Sci.Data;
using Ict;
using Ict.Win;
using Sci.Production.Class.Commons;
using Sci.Win;
using System.Xml.Linq;
using System.Configuration;
using System.Linq;
using System.Data.SqlClient;
using Sci.Production.Prg;
using System.Runtime.InteropServices;
//from trade  planning R14
namespace Sci.Production.Centralized
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        string temfile;
        Microsoft.Office.Interop.Excel.Application excel = null;

        string gstrMRTeam = "", gstrCategory = "", chx="";
        System.Data.DataTable gdtData1o, gdtData2o, gdtData3o, gdtData4o, gdtData5o, gdtData6o, gdtData7o, gdtData8o, gdtData9o;
        System.Data.DataTable gdtData1, gdtData2, gdtData3, gdtData4, gdtData5, gdtData6, gdtData7, gdtData8, gdtData9;
        System.Data.DataTable gdtData;
        public R03()
        {
            InitializeComponent();
        }

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            EditMode = true;
            print.Visible = false;

            MyUtility.Tool.SetupCombox(comboLocal, 1, 1, "Exclude,Include");
            comboLocal.SelectedIndex = 0;
        }

        protected override void OnFormLoaded()
        {
            this.Text = PrivUtils.getVersion(this.Text);
            DualResult result;
            base.OnFormLoaded();
            comboDropDownListCategory.SelectedIndex = 0;

            #region 取得 MR Team 資料
            System.Data.DataTable dt_ref = null;
            string sql = @"select * from Department WITH (NOLOCK) where Department.Type = 'MR'";
            result = DBProxy.Current.Select("Trade", sql, out dt_ref);
            if (dt_ref != null && dt_ref.Rows.Count > 0)
            {
                comboBox1.Add("ALL", "");
                for (int j = 0; j < dt_ref.Rows.Count; j++)
                {
                    DataRow dr2 = dt_ref.Rows[j];
                    comboBox1.Add(dr2["Name"].ToString(), dr2["ID"].ToString());
                }
                comboBox1.SelectedIndex = 0;
            }
            #endregion
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            chx = comboLocal.Text;
            gstrCategory = comboDropDownListCategory.SelectedValue.ToString();
            return base.ValidateInput();
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            DualResult result = Result.True;
            if (excel == null) return true; 
            gdtData1 = null; gdtData2 = null; gdtData3 = null; gdtData4 = null; 
            gdtData5 = null; gdtData6 = null; gdtData7 = null; gdtData8 = null; gdtData9 = null;
            return true;
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(System.Windows.Forms.Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
                var Connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(Connections);

            }
            if (null == connectionString || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion
            
            string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            DualResult result = new DualResult(true);
            try
            {
                string strSQL = @"
Select Orders.ID, Orders.ProgramID, Orders.StyleID, Orders.SeasonID, Orders.BrandID , Orders.FactoryID, 
Orders.POID , Orders.Category, Orders.CdCodeID , Orders.CPU, 
CPURate = (SELECT * FROM GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'Order')) * Orders.CPU  , 
Orders.BuyerDelivery, Orders.SCIDelivery, 
SewingOutput.SewingLineID , SewingOutput.ManPower, SewingOutput_Detail.ComboType, SewingOutput_Detail.WorkHour, SewingOutput_Detail.QAQty , 
QARate = (SELECT dbo.GetSuitRate(CDCODE.combopcs, SewingOutput_Detail.ComboType)) * SewingOutput_Detail.QAQty  
, TotalCPUOut  = Round(((SELECT  dbo.GetSuitRate(CDCODE.combopcs, SewingOutput_Detail.ComboType)) * SewingOutput_Detail.QAQty ) * ((SELECT * FROM GetCPURate(Orders.OrderTypeID, Orders.ProgramID, Orders.Category, Orders.BrandID, 'Order')) * Orders.CPU),6) 
, SewingOutput_Detail.WorkHour * SewingOutput.ManPower as TotalManHour 
, CDCode.Description AS CDDesc, Style.Description AS StyleDesc
, STYLE.ModularParent, STYLE.CPUAdjusted
FROM Orders WITH (NOLOCK), SewingOutput WITH (NOLOCK), SewingOutput_Detail WITH (NOLOCK) , Brand WITH (NOLOCK) , Factory WITH (NOLOCK), CDCode WITH (NOLOCK) , Style WITH (NOLOCK)
Where SewingOutput_Detail.OrderID = Orders.ID 
And SewingOutput.ID = SewingOutput_Detail.ID And SewingOutput.Shift <> 'O'  
And  Orders.BrandID = Brand.ID AND Orders.FactoryID  = Factory.ID AND Orders.CdCodeID = CDCode.ID AND Orders.StyleUkey  = Style.Ukey 
and Factory.IsProduceFty = '1'
";
                if (dateRange1.Value1.HasValue)
                    strSQL += string.Format(" and SewingOutput.OutputDate >= '{0}'", ((DateTime)dateRange1.Value1).ToString("yyyy-MM-dd"));
                if (dateRange1.Value2.HasValue)
                    strSQL += string.Format(" and SewingOutput.OutputDate <= '{0}'", ((DateTime)dateRange1.Value2).ToString("yyyy-MM-dd"));
                if (dateRange2.Value1.HasValue)
                    strSQL += string.Format(" and Orders.BuyerDelivery  >= '{0}'", ((DateTime)dateRange2.Value1).ToString("yyyy-MM-dd"));
                if (dateRange2.Value2.HasValue)
                    strSQL += string.Format(" and Orders.BuyerDelivery  <= '{0}'", ((DateTime)dateRange2.Value2).ToString("yyyy-MM-dd"));
                if (dateRange3.Value1.HasValue)
                    strSQL += string.Format(" and Orders.SCIDelivery  >= '{0}'", ((DateTime)dateRange3.Value1).ToString("yyyy-MM-dd"));
                if (dateRange3.Value2.HasValue)
                    strSQL += string.Format(" and Orders.SCIDelivery  <= '{0}'", ((DateTime)dateRange3.Value2).ToString("yyyy-MM-dd"));
                if (txtSeason1.Text != "")
                    strSQL += string.Format(" AND Orders.SeasonID = '{0}' ", txtSeason1.Text);
                if (txtBrand1.Text != "")
                    strSQL += string.Format(" AND Orders.BrandID = '{0}' ", txtBrand1.Text);
                if (txtstyle1.Text != "")
                    strSQL += string.Format(" AND Orders.StyleID = '{0}' ", txtstyle1.Text);
                if (gstrMRTeam != "")
                    strSQL += string.Format(" AND Brand.MRTeam = '{0}' ", gstrMRTeam);
                if (txtCentralizedFactory1.Text != "")
                    strSQL += string.Format(" AND Orders.FactoryID = '{0}' ", txtCentralizedFactory1.Text);
                if (gstrCategory != "")
                {
                    if (gstrCategory == "BS")
                        strSQL += " AND Orders.Category IN ( 'B', 'S') ";
                    else
                        strSQL += string.Format(" AND Orders.Category in ({0})", gstrCategory);
                }
                if (chx == "Exclude")
                    strSQL += " and o.LocalOrder = 0 ";
                if (txtCountry1.TextBox1.Text != "")
                    strSQL += string.Format(" AND Factory.CountryID = '{0}' ", txtCountry1.TextBox1.Text);
                #region 1.	By Factory
                string strFactory = string.Format(@"Select FactoryID AS A, QARate, TotalCPUOut, TotalManHour FROM ({0} ) AAA ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFactory, null, out gdtData);
                    if(gdtData1o ==null)gdtData1o = gdtData.Clone();
                    gdtData1o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData1o, "", @"select A,B=sum(QARate),C=sum(TotalCPUOut),D=sum(TotalManHour)
,E=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,F=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A order by A", out gdtData1);
                #endregion 1.	By Factory

                #region 2.	By Brand
                string strBrand = string.Format(@" Select BrandID AS A, QARate,TotalCPUOut,TotalManHour
FROM ({0} ) AAA ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strBrand, null, out gdtData);
                    if (gdtData2o == null) gdtData2o = gdtData.Clone();
                    gdtData2o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData2o, "", @"select A,B=sum(QARate),C=sum(TotalCPUOut),D=sum(TotalManHour)
,E=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,F=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A order by A", out gdtData2);
                #endregion 2.	By Brand

                #region 3.	By Brand + Factory
                string strFBrand = string.Format(@" 
Select BrandID AS A, FactoryID AS B, QARate, TotalCPUOut,TotalManHour
FROM ({0} ) AAA ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFBrand, null, out gdtData);
                    if (gdtData3o == null) gdtData3o = gdtData.Clone();
                    gdtData3o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData3o, "", @"select A,B,C=sum(QARate),D=sum(TotalCPUOut),E=sum(TotalManHour)
,F=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,G=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)  
from #tmp Group BY A,B order by A,B", out gdtData3);
                #endregion 3.	By Brand + Factory

                #region 4.	By Style
                string strStyle = string.Format(@" Select StyleID AS A, BrandID AS B, CDCodeID AS C, CDDesc AS D, StyleDesc AS E, SeasonID AS F
, QARate, TotalCPUOut,TotalManHour, ModularParent AS L, CPUAdjusted AS M
FROM ({0} ) AAA ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strStyle, null, out gdtData);
                    if (gdtData4o == null) gdtData4o = gdtData.Clone();
                    gdtData4o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData4o, "", @"select A,B,C,D,E,F
,G=sum(QARate)
,H=sum(TotalCPUOut),I=sum(TotalManHour)
,J=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)
,K=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)
,L,M 

from #tmp Group BY A,B,C,D,E,F,L,M order by A,B,C,E", out gdtData4);
                #endregion 4.	By Style

                #region 5.	By CD
                string strCdCodeID = string.Format(@" Select CdCodeID AS A, CDDesc AS B, QARate, TotalCPUOut,TotalManHour FROM ({0} ) AAA ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strCdCodeID, null, out gdtData);
                    if (gdtData5o == null) gdtData5o = gdtData.Clone();
                    gdtData5o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData5o, "", @"select A,B,C=sum(QARate),D=sum(TotalCPUOut),E=sum(TotalManHour)
,F=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,G=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2)
from #tmp Group BY A,B order by A", out gdtData5);
                #endregion 5.	By CD

                #region 6.	By Factory Line
                string strFactoryLine = string.Format(@" Select FactoryID AS A, SewingLineID AS B, QARate, TotalCPUOut,TotalManHour FROM ({0} ) AAA ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFactoryLine, null, out gdtData);
                    if (gdtData6o == null) gdtData6o = gdtData.Clone();
                    gdtData6o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData6o, "", @"select A,B, Sum(QARate) AS C, Sum(TotalCPUOut) AS D, SUM(TotalManHour) AS E
,F=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,G=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B order by A,B", out gdtData6);
                #endregion 6.	By Factory Line

                #region 7.	By Factory, Brand , CDCode
                string strFBCDCode = string.Format(@" Select BrandID AS A, FactoryID AS B, CdCodeID AS C, CDDesc AS D, QARate, TotalCPUOut,TotalManHour
FROM ({0} ) AAA  ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strFBCDCode, null, out gdtData);
                    if (gdtData7o == null) gdtData7o = gdtData.Clone();
                    gdtData7o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData7o, "", @"select A,B,C,D,E=sum(QARate),F=sum(TotalCPUOut),G=sum(TotalManHour)
,H=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,I=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C,D order by A,B,C", out gdtData7);
                #endregion 7.	By Factory, Brand , CDCode

                #region 8.	By PO Combo
                string strPOCombo = string.Format(@" Select POID AS A, StyleID AS B, BrandID AS C, CdCodeID AS D, CDDesc AS E, StyleDesc AS F, SeasonID AS G, ProgramID AS H, QARate, TotalCPUOut, TotalManHour
FROM ({0} ) AAA  ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strPOCombo, null, out gdtData);
                    if (gdtData8o == null) gdtData8o = gdtData.Clone();
                    gdtData8o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData8o, "", @"select A,B,C,D,E,F,G,H
,I=sum(QARate),J=sum(TotalCPUOut),K=sum(TotalManHour)
,L=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end),2)
,M=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C,D,E,F,G,H order by A,B,C,D,G", out gdtData8);
                #endregion 8.	By PO Combo
                
                #region 9.	By Program
                string strProgram = string.Format(@" Select ProgramID AS A, StyleID AS B, FactoryID AS C, BrandID AS D, CdCodeID AS E, CDDesc AS F, StyleDesc AS G, SeasonID AS H, QARate,TotalCPUOut, TotalManHour FROM ({0} ) AAA ", strSQL);
                foreach (string conString in connectionString)
                {
                    SqlConnection conn = new SqlConnection(conString);
                    result = DBProxy.Current.SelectByConn(conn, strProgram, null, out gdtData);
                    if (gdtData9o == null) gdtData9o = gdtData.Clone();
                    gdtData9o.Merge(gdtData);
                    if (!result)
                        return result;
                }
                MyUtility.Tool.ProcessWithDatatable(gdtData9o, "", @"select A,B,C,D,E,F,G,H
,I=sum(QARate),J=sum(TotalCPUOut),K=sum(TotalManHour)
,L=Round((Sum(TotalCPUOut) / case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end) ,2)
,M=Round((Sum(TotalCPUOut) / (case when Sum(TotalManHour) is null or Sum(TotalManHour) = 0 then 1 else Sum(TotalManHour) end * 3600 / 1400) * 100),2) 
from #tmp Group BY A,B,C,D,E,F,G,H order by A,B,C,D,E,H", out gdtData9);
                #endregion 9.	By Program

                if (((gdtData1 != null) && (gdtData1.Rows.Count > 0)) || ((gdtData2 != null) && (gdtData2.Rows.Count > 0)) || ((gdtData3 != null) && (gdtData3.Rows.Count > 0))
                     || ((gdtData4 != null) && (gdtData4.Rows.Count > 0)) || ((gdtData5 != null) && (gdtData5.Rows.Count > 0)) || ((gdtData6 != null) && (gdtData6.Rows.Count > 0))
                     || ((gdtData7 != null) && (gdtData7.Rows.Count > 0)) || ((gdtData8 != null) && (gdtData8.Rows.Count > 0)) || ((gdtData9 != null) && (gdtData9.Rows.Count > 0)))
                {
                    if (!(result = transferToExcel()))
                    {
                        return result;
                    }
                }
                else
                {
                    return new DualResult(false, "Datas not found!");
                }
            }
            catch (Exception ex)
            {
                return new DualResult(false, "data loading error.", ex);
            }
            return result;
        }

        private DualResult transferToExcel()
        {
            string[] aryAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            DualResult result = Result.True;
            string strPath = PrivUtils.getPath_XLT(AppDomain.CurrentDomain.BaseDirectory);
            temfile = strPath + @"\Centralized-R03.Prod. Efficiency Analysis Report.xltx";

            try
            {
                if (!(result = PrivUtils.Excels.CreateExcel(temfile, out excel))) return result;
                Microsoft.Office.Interop.Excel.Worksheet wsSheet;
                #region 1.	By Factory
                int intRowsCount = gdtData1.Rows.Count;
                int intRowsStart = 2;//匯入起始位置
                int rownum = intRowsStart; //每筆資料匯入之位置 
                int intColumns = 6;//匯入欄位數
                if ((gdtData1 != null) && (gdtData1.Rows.Count > 0))
                {
                    wsSheet = excel.ActiveWorkbook.Worksheets[1];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData1.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData1.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:F{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 1.	By Factory

                #region 2.	By Brand
                if ((gdtData2 != null) && (gdtData2.Rows.Count > 0))
                {
                    intColumns = 6;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[2];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData2.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData2.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:F{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 2.	By Brand

                #region 3.	By Brand-Factory
                if ((gdtData3 != null) && (gdtData3.Rows.Count > 0))
                {
                    intColumns = 7;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[3];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData3.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData3.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:G{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 3.	By Brand-Factory

                #region 4.	By Style
                if ((gdtData4 != null) && (gdtData4.Rows.Count > 0))
                {
                    intColumns = 13;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[4];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData4.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData4.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:M{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 4.	By Style

                #region 5.	By CD
                if ((gdtData5 != null) && (gdtData5.Rows.Count > 0))
                {
                    intColumns = 7;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[5];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData5.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData5.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:G{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 5.	By CD

                #region 6.	By Factory Line
                if ((gdtData6 != null) && (gdtData6.Rows.Count > 0))
                {
                    intColumns = 7;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[6];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData6.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData6.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:G{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 6.	By Factory Line

                #region 7.	By Brand-Factory-CD
                if ((gdtData7 != null) && (gdtData7.Rows.Count > 0))
                {
                    intColumns = 9;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[7];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData7.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData7.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:I{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 7.	By Brand-Factory-CD

                #region 8.	By PO Combo
                if ((gdtData8 != null) && (gdtData8.Rows.Count > 0))
                {
                    intColumns = 13;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[8];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData8.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData8.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:M{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 8.	By PO Combo

                #region 9.	By Program
                if ((gdtData9 != null) && (gdtData9.Rows.Count > 0))
                {
                    intColumns = 13;//匯入欄位數
                    wsSheet = excel.ActiveWorkbook.Worksheets[9];
                    object[,] objArray = new object[intRowsCount, intColumns];//每列匯入欄位區間
                    for (int intIndex = 0; intIndex < gdtData9.Rows.Count; intIndex++)
                    {
                        for (int intIndex_C = 0; intIndex_C < intColumns; intIndex_C++)
                        {
                            objArray[0, intIndex_C] = gdtData9.Rows[intIndex][aryAlpha[intIndex_C]];
                        }
                        wsSheet.Range[String.Format("A{0}:M{0}", intIndex + rownum)].Value2 = objArray;
                    }
                    //欄寬調整  
                    wsSheet.Range[String.Format("A:{0}", PrivUtils.getPosition(intColumns))].WrapText = false;
                    wsSheet.get_Range(String.Format("A:{0}", PrivUtils.getPosition(intColumns))).EntireColumn.AutoFit();
                }
                #endregion 9.	By Program

                #region Save & Show Excel
                excel.Visible = true;
                Workbook workbook = excel.Workbooks[1];
                string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Centralized-R03.Prod. Efficiency Analysis Report");
                workbook.SaveAs(strExcelName);
                workbook.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(workbook);

                strExcelName.OpenFile();
                #endregion 
            }
            catch (Exception ex)
            {
               if (null != excel) { excel.DisplayAlerts = false; excel.Quit(); }
               clear();
               return new DualResult(false, "Export excel error.", ex);
            }
            clear();
            return result;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gstrMRTeam = (comboBox1.SelectedIndex == -1 ? "" : comboBox1.SelectedValue2.ToString());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void clear()
        {
            gdtData1o = null; gdtData2o = null; gdtData3o = null; gdtData4o = null; gdtData5o = null; gdtData6o = null; gdtData7o = null; gdtData8o = null; gdtData9o = null; 
            return;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
