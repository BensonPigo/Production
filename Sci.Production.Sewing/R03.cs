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
using System.Runtime.InteropServices;

namespace Sci.Production.Sewing
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        DateTime? output1, output2, buyerDel1, buyerDel2, sciDel1, sciDel2;
        string season, brand, mDivision, factory, chx, style, category;
        DataTable Factory, Brand, BrandFactory, Style, CD, FactoryLine, BrandFactoryCD, POCombo, Program, FactoryLineCD;

        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FTYGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboM.Text = Sci.Env.User.Keyword;
            comboFactory.Text = Sci.Env.User.Factory;
            comboDropDownListCategory.SelectedIndex = 0;

            MyUtility.Tool.SetupCombox(comboLocal, 1, 1, "Exclude,Include");
            comboLocal.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (comboDropDownListCategory.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }
            output1 = dateSewingOutputDate.Value1;
            output2 = dateSewingOutputDate.Value2;
            buyerDel1 = dateBuyerDelivery.Value1;
            buyerDel2 = dateBuyerDelivery.Value2;
            sciDel1 = dateSCIDelivery.Value1;
            sciDel2 = dateSCIDelivery.Value2;
            season = txtseason.Text;
            brand = txtbrand.Text;
            mDivision = comboM.Text;
            factory = comboFactory.Text;
            chx = comboLocal.Text;
            style = txtstyle.Text;
            category = comboDropDownListCategory.SelectedValue.ToString();
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組撈基礎資料的SQL
            sqlCmd.Append(string.Format(@"
with tmp1stData as (
    select  o.ID
            , o.ProgramID
            , o.StyleID
            , o.SeasonID
            , o.BrandID
            , o.MDivisionID
            , o.FactoryID
            , o.CdCodeID
            , o.CPU
            , o.POID
            , so.SewingLineID
            , so.Manpower
            , sod.WorkHour
            , sod.QAQty
            , o.CPUFactor
            , Rate = isnull([dbo].[GetStyleLocation_Rate]( s.Ukey ,sod.ComboType)/100,0) 
            , StyleDesc = s.Description
            , CDDesc = c.Description
            , s.ModularParent
            , s.CPUAdjusted
            ,o.Category,OutputDate,Shift,Team,OrderId,sod.ComboType,LocalOrder
			,ManPower1= IIF(sod.QAQty = 0, so.Manpower, so.Manpower * sod.QAQty)
		, SCategory = so.Category
    from Orders o WITH (NOLOCK) 
    inner join SewingOutput_Detail sod WITH (NOLOCK) on sod.OrderId = o.ID
    inner join SewingOutput so WITH (NOLOCK) on so.ID = sod.ID
    inner join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
    inner join CDCode c WITH (NOLOCK) on c.ID = o.CdCodeID
--    left join Style_Location sl WITH (NOLOCK) on  sl.StyleUkey = s.Ukey 
--                                                  and sl.Location = sod.ComboType
    where   1=1
            and so.Shift <> 'O'
            and o.Category in ({0})", category));
            if (chx == "Exclude")
                sqlCmd.Append(" and o.LocalOrder = 0 ");

            if (!MyUtility.Check.Empty(output1))
                sqlCmd.Append(string.Format(" and so.OutputDate >= '{0}'", Convert.ToDateTime(output1).ToString("d")));

            if (!MyUtility.Check.Empty(output2))
                sqlCmd.Append(string.Format(" and so.OutputDate <= '{0}'", Convert.ToDateTime(output2).ToString("d")));

            if (!MyUtility.Check.Empty(buyerDel1))
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDel1).ToString("d")));

            if (!MyUtility.Check.Empty(buyerDel2))
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDel2).ToString("d")));

            if (!MyUtility.Check.Empty(sciDel1))
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDel1).ToString("d")));

            if (!MyUtility.Check.Empty(sciDel2))
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDel2).ToString("d")));

            if (!MyUtility.Check.Empty(season))
                sqlCmd.Append(string.Format(" and o.SeasonID = '{0}'", season));

            if (!MyUtility.Check.Empty(brand))
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));

            if (!MyUtility.Check.Empty(mDivision))
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));

            if (!MyUtility.Check.Empty(factory))
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));

            if (!MyUtility.Check.Empty(style))
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", style));

            sqlCmd.Append(@"
), forttlcpu as(
	select OutputDate,Category
		   , Shift
		   , SewingLineID
		   , Team
		   , OrderId
		   , ComboType
		   , WorkHour = sum(WorkHour)
		   , QAQty = sum(QAQty)
		   , SCategory
		   , LocalOrder
		   , FactoryID
		   , ProgramID
		   , CPU
		   , CPUFactor
		   , StyleID
		   , Rate
		   , IIF(Shift <> 'O' and Category <> 'M' and LocalOrder = 1, 'I',Shift) as LastShift
		   , MDivisionID
	from tmp1stData
	where Shift <> 'O' and LocalOrder = 0  
	group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType, SCategory, LocalOrder, FactoryID, ProgramID, CPU, CPUFactor, StyleID, Rate,MDivisionID
),tmp2ndData as (
    Select  ProgramID
            , StyleID
            , SeasonID
            , BrandID
            , MDivisionID
            , FactoryID
            , CdCodeID
            , StyleDesc
            , CDDesc
            , POID
            , CPUOutput = Round(CPU * CPUFactor * Rate * QAQty,2)
            , SewingLineID
            , ManHour = Manpower * WorkHour 
            , RateOutput = QAQty -- * Rate
            , ModularParent,CPUAdjusted 
    from tmp1stData
),tmp3rdData as (
    Select  ProgramID
            , StyleID
            , SeasonID
            , BrandID
            , MDivisionID
            , FactoryID
            , CdCodeID
            , StyleDesc
            , CDDesc
            , POID
            , SewingLineID
            , ModularParent
            , CPUAdjusted
            , TotalCPU = (select Sum(Round(CPU * CPUFactor * Rate * QAQty,2))  from forttlcpu f 
			where  f.MDivisionID = a.MDivisionID and f.FactoryID = a.FactoryID and f.ProgramID = a.ProgramID and f.StyleID = a.StyleID and f.SewingLineID = a.SewingLineID and (select POID from orders where id = f.OrderId) = a.POID)
            , TtlManhour = Sum(ManHour)
            , Output = Sum(RateOutput)  
    from tmp2ndData a
    group by ProgramID, StyleID, SeasonID, BrandID, MDivisionID, FactoryID
             , CdCodeID, StyleDesc, CDDesc, POID, SewingLineID, ModularParent
             , CPUAdjusted
),");
            #endregion

            string querySql;
            DualResult result;

            # region By Factory
            querySql = string.Format(@"
{0}tmp4thData as (
    select  MDivisionID
            , FactoryID 
            , TtlQty = sum(Output)
            , TtlCPU = SUM(TotalCPU)
            , TtlManhour = SUM(TtlManhour)
    from tmp3rdData
    group by MDivisionID, FactoryID
)
select  MDivisionID
        , FactoryID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , PPH = IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2))
        ,EFF = IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2))
from tmp4thData
Order by MDivisionID,FactoryID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out Factory);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By Brand
            querySql = string.Format(@"
{0}tmp4thData as (
    select  BrandID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by BrandID
)
select  BrandID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by BrandID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out Brand);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By Brand-Factory
            querySql = string.Format(@"
{0}tmp4thData as (
    select  BrandID
            , MDivisionID
            , FactoryID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by BrandID, MDivisionID, FactoryID
)
select  BrandID
        , MDivisionID
        , FactoryID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by BrandID, MDivisionID, FactoryID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out BrandFactory);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand-Factory data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By Style
            querySql = string.Format(@"
{0}tmp4thData as (
    select  StyleID
            , ModularParent
            , CPUAdjusted
            , BrandID
            , CdCodeID
            , CDDesc
            , StyleDesc
            , SeasonID
            , sum(Output) AS TtlQty, SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by StyleID, ModularParent, CPUAdjusted, BrandID, CdCodeID
             , CDDesc, StyleDesc, SeasonID
)
select  StyleID
        , ModularParent
        , [CPUAdjusted] = CPUAdjusted*100
        , BrandID
        , CdCodeID
        , CDDesc
        , StyleDesc
        , SeasonID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by StyleID, SeasonID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out Style);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Style data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By CD
            querySql = string.Format(@"
{0} tmp4thData as (
    select  CdCodeID
            , CDDesc
            , sum(Output) AS TtlQty, SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by CdCodeID, CDDesc
)
select  CdCodeID
        , CDDesc
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by CdCodeID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out CD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By Factory-Line
            querySql = string.Format(@"
{0} tmp4thData as (
    select  MDivisionID
            , FactoryID
            , SewingLineID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by MDivisionID, FactoryID, SewingLineID
)
select  MDivisionID
        , FactoryID
        , SewingLineID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by MDivisionID, FactoryID, SewingLineID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out FactoryLine);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory-Line data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By Brand-Factory-CD
            querySql = string.Format(@"
{0}tmp4thData as (
    select  BrandID
            , MDivisionID
            , FactoryID
            , CdCodeID
            , CDDesc
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by BrandID, MDivisionID, FactoryID, CdCodeID, CDDesc
)
select  BrandID
        , MDivisionID
        , FactoryID
        , CdCodeID
        , CDDesc
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by BrandID, MDivisionID, FactoryID, CdCodeID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out BrandFactoryCD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand-Factory-CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By PO Combo
            querySql = string.Format(@"
{0}tmp4thData as (
    select  POID
            , StyleID
            , BrandID
            , CdCodeID
            , CDDesc
            , StyleDesc
            , SeasonID
            , ProgramID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by POID, StyleID, BrandID, CdCodeID, CDDesc, StyleDesc, SeasonID
             , ProgramID
)
select  POID
        , StyleID
        , BrandID
        , CdCodeID
        , CDDesc
        , StyleDesc
        , SeasonID
        , ProgramID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by POID, StyleID, BrandID, CdCodeID, SeasonID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out POCombo);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By PO Combo data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            # region By Program
            querySql = string.Format(@"
{0}tmp4thData as (
    select  ProgramID
            , StyleID
            , BrandID
            , CdCodeID
            , CDDesc
            , StyleDesc
            , SeasonID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by ProgramID, StyleID, BrandID, CdCodeID, CDDesc, StyleDesc, SeasonID
)
select  ProgramID
        , StyleID
        , BrandID
        , CdCodeID
        , CDDesc
        , StyleDesc
        , SeasonID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by ProgramID, StyleID, BrandID, CdCodeID, SeasonID", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out Program);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Program data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Factory-Line-CD
            querySql = string.Format(@"
{0}tmp4thData as (
    select  MDivisionID
            , FactoryID
            , SewingLineID
            , CdCodeID
            , CDDesc
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by MDivisionID, FactoryID, SewingLineID, CdCodeID, CDDesc
)
select  MDivisionID
        , FactoryID
        , SewingLineID
        , CdCodeID
        , CDDesc
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF
from tmp4thData
Order by MDivisionID, FactoryID, SewingLineID, CdCodeID, CDDesc", sqlCmd.ToString());
            result = DBProxy.Current.Select(null, querySql, out FactoryLineCD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory-Line-CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion 
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            int totalCount = Factory.Rows.Count + Brand.Rows.Count + BrandFactory.Rows.Count + Style.Rows.Count + CD.Rows.Count + FactoryLine.Rows.Count + BrandFactoryCD.Rows.Count + POCombo.Rows.Count + Program.Rows.Count;
            SetCount(totalCount);

            if (totalCount <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Sewing_R03_ProdEfficiencyAnalysisReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart;
            //填內容值
            #region By Factory
            intRowsStart = 2;
            object[,] objArray = new object[1, 7];
            foreach (DataRow dr in Factory.Rows)
            {
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["TtlQty"];
                objArray[0, 3] = dr["TtlCPU"];
                objArray[0, 4] = dr["TtlManhour"];
                objArray[0, 5] = dr["PPH"];
                objArray[0, 6] = dr["EFF"];
                
                worksheet.Range[String.Format("A{0}:G{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Brand
            worksheet = excel.ActiveWorkbook.Worksheets[2];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 6];
            foreach (DataRow dr in Brand.Rows)
            {
                objArray[0, 0] = dr["BrandID"];
                objArray[0, 1] = dr["TtlQty"];
                objArray[0, 2] = dr["TtlCPU"];
                objArray[0, 3] = dr["TtlManhour"];
                objArray[0, 4] = dr["PPH"];
                objArray[0, 5] = dr["EFF"];

                worksheet.Range[String.Format("A{0}:F{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Brand-Factory
            worksheet = excel.ActiveWorkbook.Worksheets[3];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 8];
            foreach (DataRow dr in BrandFactory.Rows)
            {
                objArray[0, 0] = dr["BrandID"];
                objArray[0, 1] = dr["MDivisionID"];
                objArray[0, 2] = dr["FactoryID"];
                objArray[0, 3] = dr["TtlQty"];
                objArray[0, 4] = dr["TtlCPU"];
                objArray[0, 5] = dr["TtlManhour"];
                objArray[0, 6] = dr["PPH"];
                objArray[0, 7] = dr["EFF"];

                worksheet.Range[String.Format("A{0}:H{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Style
            worksheet = excel.ActiveWorkbook.Worksheets[4];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 13];
            foreach (DataRow dr in Style.Rows)
            {
                objArray[0, 0] = dr["StyleID"];
                objArray[0, 1] = dr["ModularParent"];
                objArray[0, 2] = dr["CPUAdjusted"];
                objArray[0, 3] = dr["BrandID"];
                objArray[0, 4] = dr["CdCodeID"];
                objArray[0, 5] = dr["CDDesc"];
                objArray[0, 6] = dr["StyleDesc"];
                objArray[0, 7] = dr["SeasonID"];
                objArray[0, 8] = dr["TtlQty"];
                objArray[0, 9] = dr["TtlCPU"];
                objArray[0, 10] = dr["TtlManhour"];
                objArray[0, 11] = dr["PPH"];
                objArray[0, 12] = dr["EFF"];
                worksheet.Range[String.Format("A{0}:M{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By CD
            worksheet = excel.ActiveWorkbook.Worksheets[5];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 7];
            foreach (DataRow dr in CD.Rows)
            {
                objArray[0, 0] = dr["CdCodeID"];
                objArray[0, 1] = dr["CDDesc"];
                objArray[0, 2] = dr["TtlQty"];
                objArray[0, 3] = dr["TtlCPU"];
                objArray[0, 4] = dr["TtlManhour"];
                objArray[0, 5] = dr["PPH"];
                objArray[0, 6] = dr["EFF"];
                worksheet.Range[String.Format("A{0}:G{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Factory-Line
            worksheet = excel.ActiveWorkbook.Worksheets[6];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 8];
            foreach (DataRow dr in FactoryLine.Rows)
            {
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["SewingLineID"];
                objArray[0, 3] = dr["TtlQty"];
                objArray[0, 4] = dr["TtlCPU"];
                objArray[0, 5] = dr["TtlManhour"];
                objArray[0, 6] = dr["PPH"];
                objArray[0, 7] = dr["EFF"];
                worksheet.Range[String.Format("A{0}:H{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Brand-Factory-CD
            worksheet = excel.ActiveWorkbook.Worksheets[7];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 10];
            foreach (DataRow dr in BrandFactoryCD.Rows)
            {
                objArray[0, 0] = dr["BrandID"];
                objArray[0, 1] = dr["MDivisionID"];
                objArray[0, 2] = dr["FactoryID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDDesc"];
                objArray[0, 5] = dr["TtlQty"];
                objArray[0, 6] = dr["TtlCPU"];
                objArray[0, 7] = dr["TtlManhour"];
                objArray[0, 8] = dr["PPH"];
                objArray[0, 9] = dr["EFF"];
                worksheet.Range[String.Format("A{0}:J{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By PO Combo
            worksheet = excel.ActiveWorkbook.Worksheets[8];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 13];
            foreach (DataRow dr in POCombo.Rows)
            {
                objArray[0, 0] = dr["POID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDDesc"];
                objArray[0, 5] = dr["StyleDesc"];
                objArray[0, 6] = dr["SeasonID"];
                objArray[0, 7] = dr["ProgramID"];
                objArray[0, 8] = dr["TtlQty"];
                objArray[0, 9] = dr["TtlCPU"];
                objArray[0, 10] = dr["TtlManhour"];
                objArray[0, 11] = dr["PPH"];
                objArray[0, 12] = dr["EFF"];
                worksheet.Range[String.Format("A{0}:M{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Program
            worksheet = excel.ActiveWorkbook.Worksheets[9];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 12];
            foreach (DataRow dr in Program.Rows)
            {
                objArray[0, 0] = dr["ProgramID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDDesc"];
                objArray[0, 5] = dr["StyleDesc"];
                objArray[0, 6] = dr["SeasonID"];
                objArray[0, 7] = dr["TtlQty"];
                objArray[0, 8] = dr["TtlCPU"];
                objArray[0, 9] = dr["TtlManhour"];
                objArray[0, 10] = dr["PPH"];
                objArray[0, 11] = dr["EFF"];
                worksheet.Range[String.Format("A{0}:L{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Factory Line CD
            worksheet = excel.ActiveWorkbook.Worksheets[10];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 10];
            foreach (DataRow dr in FactoryLineCD.Rows)
            { 
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["SewingLineID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDDesc"];
                objArray[0, 5] = dr["TtlQty"];
                objArray[0, 6] = dr["TtlCPU"];
                objArray[0, 7] = dr["TtlManhour"];
                objArray[0, 8] = dr["PPH"];
                objArray[0, 9] = dr["EFF"];
                worksheet.Range[String.Format("A{0}:J{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }
            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Select();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Sewing_R03_ProdEfficiencyAnalysisReport");
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
