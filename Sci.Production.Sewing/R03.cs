using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// R03
    /// </summary>
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        private DateTime? output1;
        private DateTime? output2;
        private DateTime? buyerDel1;
        private DateTime? buyerDel2;
        private DateTime? sciDel1;
        private DateTime? sciDel2;
        private string season;
        private string brand;
        private string ftyZone;
        private string factory;
        private string style;
        private string category;
        private DataTable Factory;
        private DataTable Brand;
        private DataTable BrandFactory;
        private DataTable Style;
        private DataTable CD;
        private DataTable FactoryLine;
        private DataTable BrandFactoryCD;
        private DataTable POCombo;
        private DataTable Program;
        private DataTable FactoryLineCD;

        /// <summary>
        /// R03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FTYGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.Text = Sci.Env.User.Factory;
            this.comboDropDownListCategory.SelectedIndex = 0;
            this.comboFtyZone.setDataSource();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (this.comboDropDownListCategory.SelectedIndex == -1)
            {
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }

            this.output1 = this.dateSewingOutputDate.Value1;
            this.output2 = this.dateSewingOutputDate.Value2;
            this.buyerDel1 = this.dateBuyerDelivery.Value1;
            this.buyerDel2 = this.dateBuyerDelivery.Value2;
            this.sciDel1 = this.dateSCIDelivery.Value1;
            this.sciDel2 = this.dateSCIDelivery.Value2;
            this.season = this.txtseason.Text;
            this.brand = this.txtbrand.Text;
            this.ftyZone = this.comboFtyZone.Text;
            this.factory = this.comboFactory.Text;
            this.style = this.txtstyle.Text;
            this.category = this.comboDropDownListCategory.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region 組撈基礎資料的SQL
            sqlCmd.Append(string.Format(
                @"
with tmp1stData as (
    select  o.ID
            , o.ProgramID
            , o.StyleID
            , o.SeasonID
            , [BrandID] = iif(o.BrandID='SUBCON-I' and Order2.BrandID is not null,Order2.BrandID,o.BrandID)
            , [FtyZone] = f.FtyZone
            , o.FactoryID
            , o.CdCodeID
            , o.CPU
            , o.POID
            , so.SewingLineID
            , so.Manpower
            , sod.WorkHour
            , sod.QAQty
            , o.CPUFactor
            , Rate = isnull([dbo].[GetOrderLocation_Rate]( o.id ,sod.ComboType)/100,1) 
            , StyleDesc = s.Description
            , CDDesc = c.Description
            , s.ModularParent
            , s.CPUAdjusted
            , o.Category,OutputDate,Shift,Team,OrderId,sod.ComboType,LocalOrder
			--,ManPower1= IIF(sod.QAQty = 0, so.Manpower, so.Manpower * sod.QAQty)
            , ActManPower= so.Manpower 
		, SCategory = so.Category
    from Orders o WITH (NOLOCK) 
    inner join SewingOutput_Detail sod WITH (NOLOCK) on sod.OrderId = o.ID
    inner join SewingOutput so WITH (NOLOCK) on so.ID = sod.ID
    inner join Style s WITH (NOLOCK) on s.Ukey = o.StyleUkey
    inner join CDCode c WITH (NOLOCK) on c.ID = o.CdCodeID
    left join Factory f WITH (NOLOCK) on o.FactoryID = f.id
    outer apply(
		    select BrandID from orders o1 
		    where o.CustPONo=o1.id
	)Order2
    where   1=1
            and so.Shift <> 'O'
            and o.Category in ({0})
            --排除non sister的資料o.LocalOrder = 1 and o.SubconInType = 0
            and ((o.LocalOrder = 1 and o.SubconInType in ('1','2')) or (o.LocalOrder = 0 and o.SubconInType = 0))",
                this.category));

            if (!MyUtility.Check.Empty(this.output1))
            {
                sqlCmd.Append(string.Format(" and so.OutputDate >= '{0}'", Convert.ToDateTime(this.output1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.output2))
            {
                sqlCmd.Append(string.Format(" and so.OutputDate <= '{0}'", Convert.ToDateTime(this.output2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDel1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDel1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDel2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDel2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDel1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDel1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDel2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDel2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(string.Format(" and o.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.ftyZone))
            {
                sqlCmd.Append(string.Format(" and f.FtyZone = '{0}'", this.ftyZone));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", this.style));
            }

            if (this.chkType.Checked)
            {
                sqlCmd.Append($@" AND f.Type <>'S' ");
            }

            sqlCmd.Append(@"
), forttlcpu as(
	select OutputDate,Category
		   , Shift
		   , SewingLineID
		   , Team
		   , OrderId
		   , ComboType
           , ActManPower= ActManPower
		   , WorkHour = sum(Round(WorkHour,3))
		   , QAQty = sum(QAQty)
		   , SCategory
		   , LocalOrder
		   , FactoryID
		   , ProgramID
		   , CPU
		   , CPUFactor
		   , StyleID
		   , Rate
		   , IIF(Shift <> 'O' and Category NOT IN ('M','A') and LocalOrder = 1, 'I',Shift) as LastShift
		   , FtyZone
	from tmp1stData
	group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType, SCategory, LocalOrder, FactoryID, ProgramID, CPU, CPUFactor, StyleID, Rate,FtyZone,ActManPower
),tmp2ndData as (
    Select  ProgramID
            , StyleID
            , SeasonID
            , BrandID
            , FtyZone
            , FactoryID
            , CdCodeID
            , StyleDesc
            , CDDesc
            , POID
            , CPUOutput = Round(CPU * CPUFactor * Rate * QAQty,2)
            , SewingLineID
            , ManHour = Round(Manpower * WorkHour ,2)
            , RateOutput = QAQty  * Rate
            , ModularParent,CPUAdjusted 
    from tmp1stData
),tmp3rdData as (
    Select  ProgramID
            , StyleID
            , SeasonID
            , BrandID
            , FtyZone
            , FactoryID
            , CdCodeID
            , StyleDesc
            , CDDesc
            , POID
            , SewingLineID
            , ModularParent
            , CPUAdjusted
            , TotalCPU = (select Sum(Round(CPU * CPUFactor * Rate * QAQty,2))  from forttlcpu f 
			where  f.FtyZone = a.FtyZone and f.FactoryID = a.FactoryID and f.ProgramID = a.ProgramID and f.StyleID = a.StyleID and f.SewingLineID = a.SewingLineID and (select POID from orders where id = f.OrderId) = a.POID)
            --, TtlManhour = Sum(ManHour)
            , TtlManhour = (select sum(ROUND( ActManPower * WorkHour, 2))  from forttlcpu f 
			where  f.FtyZone = a.FtyZone and f.FactoryID = a.FactoryID and f.ProgramID = a.ProgramID and f.StyleID = a.StyleID and f.SewingLineID = a.SewingLineID and (select POID from orders where id = f.OrderId) = a.POID)
            , Output = Sum(RateOutput)  
    from tmp2ndData a
    group by ProgramID, StyleID, SeasonID, BrandID, FtyZone, FactoryID
             , CdCodeID, StyleDesc, CDDesc, POID, SewingLineID, ModularParent
             , CPUAdjusted
),");
            #endregion

            string querySql;
            DualResult result;

            #region By Factory
            querySql = string.Format(
                @"
{0}tmp4thData as (
    select  FtyZone
            , FactoryID 
            , TtlQty = sum(Output)
            , TtlCPU = SUM(TotalCPU)
            , TtlManhour = SUM(TtlManhour)
    from tmp3rdData
    group by FtyZone, FactoryID
)
select  FtyZone
        , FactoryID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , PPH = IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2))
        ,EFF = IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2))
from tmp4thData
Order by FtyZone,FactoryID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.Factory);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Brand
            querySql = string.Format(
                @"
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
Order by BrandID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.Brand);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Brand-Factory
            querySql = string.Format(
                @"
{0}tmp4thData as (
    select  BrandID
            , FtyZone
            , FactoryID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by BrandID, FtyZone, FactoryID
)
select  BrandID
        , FtyZone
        , FactoryID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by BrandID, FtyZone, FactoryID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.BrandFactory);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand-Factory data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Style
            querySql = string.Format(
                @"
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
Order by StyleID, SeasonID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.Style);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Style data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By CD
            querySql = string.Format(
                @"
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
Order by CdCodeID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.CD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Factory-Line
            querySql = string.Format(
                @"
{0} tmp4thData as (
    select  FtyZone
            , FactoryID
            , SewingLineID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by FtyZone, FactoryID, SewingLineID
)
select  FtyZone
        , FactoryID
        , SewingLineID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by FtyZone, FactoryID, SewingLineID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.FactoryLine);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory-Line data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Brand-Factory-CD
            querySql = string.Format(
                @"
{0}tmp4thData as (
    select  BrandID
            , FtyZone
            , FactoryID
            , CdCodeID
            , CDDesc
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by BrandID, FtyZone, FactoryID, CdCodeID, CDDesc
)
select  BrandID
        , FtyZone
        , FactoryID
        , CdCodeID
        , CDDesc
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from tmp4thData
Order by BrandID, FtyZone, FactoryID, CdCodeID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.BrandFactoryCD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand-Factory-CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By PO Combo
            querySql = string.Format(
                @"
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
Order by POID, StyleID, BrandID, CdCodeID, SeasonID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.POCombo);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By PO Combo data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Program
            querySql = string.Format(
                @"
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
Order by ProgramID, StyleID, BrandID, CdCodeID, SeasonID",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.Program);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Program data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Factory-Line-CD
            querySql = string.Format(
                @"
{0}tmp4thData as (
    select  FtyZone
            , FactoryID
            , SewingLineID
            , CdCodeID
            , CDDesc
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from tmp3rdData
    group by FtyZone, FactoryID, SewingLineID, CdCodeID, CDDesc
)
select  FtyZone
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
Order by FtyZone, FactoryID, SewingLineID, CdCodeID, CDDesc",
                sqlCmd.ToString());

            result = DBProxy.Current.Select(null, querySql, out this.FactoryLineCD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory-Line-CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion
            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            int totalCount = this.Factory.Rows.Count + this.Brand.Rows.Count + this.BrandFactory.Rows.Count + this.Style.Rows.Count + this.CD.Rows.Count + this.FactoryLine.Rows.Count + this.BrandFactoryCD.Rows.Count + this.POCombo.Rows.Count + this.Program.Rows.Count;
            this.SetCount(totalCount);

            if (totalCount <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Sewing_R03_ProdEfficiencyAnalysisReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart;

            // 填內容值
            #region By Factory
            intRowsStart = 2;
            object[,] objArray = new object[1, 7];
            foreach (DataRow dr in this.Factory.Rows)
            {
                objArray[0, 0] = dr["FtyZone"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["TtlQty"];
                objArray[0, 3] = dr["TtlCPU"];
                objArray[0, 4] = dr["TtlManhour"];
                objArray[0, 5] = dr["PPH"];
                objArray[0, 6] = dr["EFF"];

                worksheet.Range[string.Format("A{0}:G{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.Brand.Rows)
            {
                objArray[0, 0] = dr["BrandID"];
                objArray[0, 1] = dr["TtlQty"];
                objArray[0, 2] = dr["TtlCPU"];
                objArray[0, 3] = dr["TtlManhour"];
                objArray[0, 4] = dr["PPH"];
                objArray[0, 5] = dr["EFF"];

                worksheet.Range[string.Format("A{0}:F{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.BrandFactory.Rows)
            {
                objArray[0, 0] = dr["BrandID"];
                objArray[0, 1] = dr["FtyZone"];
                objArray[0, 2] = dr["FactoryID"];
                objArray[0, 3] = dr["TtlQty"];
                objArray[0, 4] = dr["TtlCPU"];
                objArray[0, 5] = dr["TtlManhour"];
                objArray[0, 6] = dr["PPH"];
                objArray[0, 7] = dr["EFF"];

                worksheet.Range[string.Format("A{0}:H{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.Style.Rows)
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
                worksheet.Range[string.Format("A{0}:M{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.CD.Rows)
            {
                objArray[0, 0] = dr["CdCodeID"];
                objArray[0, 1] = dr["CDDesc"];
                objArray[0, 2] = dr["TtlQty"];
                objArray[0, 3] = dr["TtlCPU"];
                objArray[0, 4] = dr["TtlManhour"];
                objArray[0, 5] = dr["PPH"];
                objArray[0, 6] = dr["EFF"];
                worksheet.Range[string.Format("A{0}:G{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.FactoryLine.Rows)
            {
                objArray[0, 0] = dr["FtyZone"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["SewingLineID"];
                objArray[0, 3] = dr["TtlQty"];
                objArray[0, 4] = dr["TtlCPU"];
                objArray[0, 5] = dr["TtlManhour"];
                objArray[0, 6] = dr["PPH"];
                objArray[0, 7] = dr["EFF"];
                worksheet.Range[string.Format("A{0}:H{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.BrandFactoryCD.Rows)
            {
                objArray[0, 0] = dr["BrandID"];
                objArray[0, 1] = dr["FtyZone"];
                objArray[0, 2] = dr["FactoryID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDDesc"];
                objArray[0, 5] = dr["TtlQty"];
                objArray[0, 6] = dr["TtlCPU"];
                objArray[0, 7] = dr["TtlManhour"];
                objArray[0, 8] = dr["PPH"];
                objArray[0, 9] = dr["EFF"];
                worksheet.Range[string.Format("A{0}:J{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.POCombo.Rows)
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
                worksheet.Range[string.Format("A{0}:M{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.Program.Rows)
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
                worksheet.Range[string.Format("A{0}:L{0}", intRowsStart)].Value2 = objArray;
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
            foreach (DataRow dr in this.FactoryLineCD.Rows)
            {
                objArray[0, 0] = dr["FtyZone"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["SewingLineID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDDesc"];
                objArray[0, 5] = dr["TtlQty"];
                objArray[0, 6] = dr["TtlCPU"];
                objArray[0, 7] = dr["TtlManhour"];
                objArray[0, 8] = dr["PPH"];
                objArray[0, 9] = dr["EFF"];
                worksheet.Range[string.Format("A{0}:J{0}", intRowsStart)].Value2 = objArray;
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
