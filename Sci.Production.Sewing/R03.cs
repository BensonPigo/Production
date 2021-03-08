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
    public partial class R03 : Win.Tems.PrintForm
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
        private decimal months;
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
            this.comboFactory.Text = Env.User.Factory;
            this.comboDropDownListCategory.SelectedIndex = 0;
            this.comboFtyZone.SetDataSource();
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
            this.months = this.numNewStyleBaseOn.Value;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            DualResult result;
            DataTable dt = new DataTable();
            #region 組撈基礎資料的SQL
            sqlCmd.Append(string.Format(
                @"
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
    into #tmp_1stData
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

            if (this.months != 0)
            {
                sqlCmd.Append(string.Format(@" and dateadd(month, {0}, o.SciDelivery ) < so.OutputDate", -this.months));
            }

            if (this.chkType.Checked)
            {
                sqlCmd.Append($@" AND f.Type <>'S' ");
            }

            sqlCmd.Append(@"
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
    into #tmp_forttlcpu
	from #tmp_1stData
	group by OutputDate, Category, Shift, SewingLineID, Team, OrderId, ComboType, SCategory, LocalOrder, FactoryID, ProgramID, CPU, CPUFactor, StyleID, Rate,FtyZone,ActManPower

    Select  ProgramID
            , StyleID
            , SeasonID
            , BrandID
            , FtyZone
            , FactoryID
            , CdCodeID
            , sty.CDCodeNew
	        , sty.ProductType
	        , sty.FabricType
	        , sty.Lining
	        , sty.Gender
	        , sty.Construction          
            , StyleDesc
            , CDDesc
            , POID
            , CPUOutput = Round(CPU * CPUFactor * Rate * QAQty,2)
            , SewingLineID
            , ManHour = Round(Manpower * WorkHour ,2)
            , RateOutput = QAQty  * Rate
            , ModularParent
            , CPUAdjusted 
            , Category
            , OutputDate
    into #tmp_2ndData
    from #tmp_1stData t
    Outer apply (
	    SELECT s.CDCodeNew
            , ProductType = r2.Name
		    , FabricType = r1.Name
		    , Lining
		    , Gender
		    , Construction = d1.Name
	    FROM Style s WITH(NOLOCK)
	    left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	    left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	    left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	    where s.ID = t.StyleID 
	    and s.SeasonID = t.SeasonID 
	    and s.BrandID = t.BrandID
    )sty

    Select  ProgramID
            , StyleID
            , SeasonID
            , BrandID
            , FtyZone
            , FactoryID
            , CdCodeID
            , CDCodeNew
	        , ProductType
	        , FabricType
	        , Lining
	        , Gender
	        , Construction 
            , StyleDesc
            , CDDesc
            , POID
            , SewingLineID
            , ModularParent
            , CPUAdjusted
            , TotalCPU = (select Sum(Round(CPU * CPUFactor * Rate * QAQty,2))  
                          from #tmp_forttlcpu f 
                          where f.FtyZone = a.FtyZone 
                          and f.FactoryID = a.FactoryID 
                          and f.ProgramID = a.ProgramID 
                          and f.StyleID = a.StyleID 
                          and f.SewingLineID = a.SewingLineID 
                          and (select POID from orders where id = f.OrderId) = a.POID)
            , TtlManhour = (select sum(ROUND( ActManPower * WorkHour, 2)) 
                            from #tmp_forttlcpu f 
                            where  f.FtyZone = a.FtyZone 
                            and f.FactoryID = a.FactoryID 
                            and f.ProgramID = a.ProgramID 
                            and f.StyleID = a.StyleID 
                            and f.SewingLineID = a.SewingLineID 
                            and (select POID from orders where id = f.OrderId) = a.POID)
            , Output = Sum(RateOutput)  
            , Category
            , OutputDate = Max(OutputDate)
    from #tmp_2ndData a
    group by ProgramID, StyleID, SeasonID, BrandID, FtyZone, FactoryID
             , CdCodeID, StyleDesc, CDDesc, POID, SewingLineID, ModularParent
             , CPUAdjusted, CDCodeNew, ProductType, FabricType, Lining
	         , Gender, Construction, Category
");

            result = DBProxy.Current.Select(null, sqlCmd.ToString(), null, out dt);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            string querySql;

            #region By Factory
            querySql = @"
select  FtyZone
        , FactoryID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , PPH = IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2))
        , EFF = IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK)))*100, 2))
from (
     select  FtyZone
            , FactoryID 
            , TtlQty = sum(Output)
            , TtlCPU = SUM(TotalCPU)
            , TtlManhour = SUM(TtlManhour)
    from #tmp
    group by FtyZone, FactoryID
)t
Order by FtyZone,FactoryID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.Factory);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Brand
            querySql = @"
select  BrandID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from (
    select BrandID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by BrandID
)t
Order by BrandID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.Brand);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Brand-Factory
            querySql = @"
select  BrandID
        , FtyZone
        , FactoryID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour * 3600/(select StdTMS from System WITH (NOLOCK)))*100, 2)) as EFF 
from (
    select  BrandID
            , FtyZone
            , FactoryID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by BrandID, FtyZone, FactoryID
)t
Order by BrandID, FtyZone, FactoryID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.BrandFactory);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand-Factory data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Style
            querySql = @"
select t.StyleID, t.BrandID, t.StyleDesc, t.SeasonID, t.FtyZone, OutputDate = Max(t.OutputDate)
into #tmp_LineMaxOutputDate
from #tmp t
group by t.StyleID, t.BrandID, t.StyleDesc, t.SeasonID, t.FtyZone

select  StyleID
        , ModularParent
        , [CPUAdjusted] = CPUAdjusted*100
        , BrandID
        , CdCodeID
        , CDCodeNew
	    , ProductType
	    , FabricType
	    , Lining
	    , Gender
	    , Construction 
        , CDDesc
        , StyleDesc
        , SeasonID
        , TtlQty = SUM(TtlQty)
        , TtlCPU = SUM(TtlCPU)
        , TtlManhour = SUM(TtlManhour)
        , PPH = SUM(IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)))
        , EFF = SUM(IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour * 3600/ 1400)*100, 2)))
		, [Remark] = Stuff((select concat('/', a.FtyZone + ' ' + (case when Max(a.OutputDate) is null then 'New Style'
																	   else concat((Stuff((
																						select distinct concat(' ', a2.SewingLineID)
																						from #tmp a2
																						where a2.StyleID = t.StyleID
																						and a2.BrandID = t.BrandID
																						and a2.StyleDesc = t.StyleDesc
                                                                                        and a2.SeasonID = t.SeasonID
                                                                                        and a2.FtyZone = a.FtyZone
                                                                                        and exists( select 1 
                                                                                                     from #tmp_LineMaxOutputDate t2 
                                                                                                     where t2.OutputDate = a2.OutputDate 
                                                                                                     and t2.StyleID = a2.StyleID
                                                                                                     and t2.BrandID = a2.BrandID 
                                                                                                     and t2.StyleDesc = a2.StyleDesc
																									 and t2.SeasonID = a2.SeasonID
																									 and t2.FtyZone = a2.FtyZone)
																						FOR XML PATH('')) ,1,1,'')),'(',format(Max(a.OutputDate), 'yyyy/MM/dd'),')')
																	   end))
						from #tmp a where a.StyleID = t.StyleID and 
											a.BrandID = t.BrandID and
											a.StyleDesc = t.StyleDesc and
                                            a.SeasonID = t.SeasonID 
						group by a.FtyZone FOR XML PATH(''))
				,1,1,'') 
from (
    select  StyleID
            , ModularParent
            , CPUAdjusted
            , BrandID
            , CdCodeID
            , CDCodeNew
	        , ProductType
	        , FabricType
	        , Lining
	        , Gender
	        , Construction 
            , CDDesc
            , StyleDesc
            , SeasonID
            , FtyZone
            , sum(Output) AS TtlQty, SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by StyleID, ModularParent, CPUAdjusted, BrandID, CdCodeID
             , CDDesc, StyleDesc, SeasonID, CDCodeNew, ProductType, FabricType
	         , Lining, Gender, Construction, FtyZone
) t
GROUP by StyleID, ModularParent, CPUAdjusted, BrandID, CdCodeID, CDCodeNew
	    , ProductType, FabricType, Lining, Gender, Construction, CDDesc, StyleDesc, SeasonID
Order by StyleID, SeasonID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.Style);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Style data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By CD
            querySql = @"
select  CdCodeID
        , CDCodeNew
	    , ProductType
	    , FabricType
	    , Lining
	    , Gender
	    , Construction 
        , CDDesc
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from (
    select  CdCodeID
            , CDCodeNew
	        , ProductType
	        , FabricType
	        , Lining
	        , Gender
	        , Construction 
            , CDDesc
            , sum(Output) AS TtlQty, SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by CdCodeID, CDDesc, CDCodeNew
	        , ProductType, FabricType, Lining
            , Gender, Construction 
)t
Order by CdCodeID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.CD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Factory-Line
            querySql = @"
select  FtyZone
        , FactoryID
        , SewingLineID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from (
    select  FtyZone
            , FactoryID
            , SewingLineID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by FtyZone, FactoryID, SewingLineID
)t
Order by FtyZone, FactoryID, SewingLineID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.FactoryLine);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory-Line data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Brand-Factory-CD
            querySql = string.Format(
                @"
select  BrandID
        , FtyZone
        , FactoryID
        , CdCodeID
        , CDCodeNew
	    , ProductType
	    , FabricType
	    , Lining
	    , Gender
	    , Construction 
        , CDDesc
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
from (
    select  BrandID
            , FtyZone
            , FactoryID
            , CdCodeID
            , CDCodeNew
	        , ProductType
	        , FabricType
	        , Lining
	        , Gender
	        , Construction 
            , CDDesc
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by BrandID, FtyZone, FactoryID, CdCodeID, CDDesc
           , CDCodeNew, ProductType , FabricType, Lining, Gender, Construction 
)t
Order by BrandID, FtyZone, FactoryID, CdCodeID",
                sqlCmd.ToString());

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.BrandFactoryCD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Brand-Factory-CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By PO Combo
            querySql = @"
select t.StyleID, t.BrandID, t.StyleDesc, t.SeasonID, t.POID, t.FtyZone, OutputDate = Max(t.OutputDate)
into #tmp_LineMaxOutputDatePOID
from #tmp t
group by t.StyleID, t.BrandID, t.StyleDesc, t.SeasonID, t.POID, t.FtyZone

select  POID
        , StyleID
        , BrandID
        , CdCodeID
        , CDCodeNew
	    , ProductType
	    , FabricType
	    , Lining
	    , Gender
	    , Construction 
        , CDDesc
        , StyleDesc
        , SeasonID
        , ProgramID
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF 
		, [Remark] = Stuff((select concat('/', a.FtyZone + ' ' + (case when Max(a.OutputDate) is null then 'New Style'
																	   when sum(iif(a.Category = 'S',1,0)) > 0 AND sum(iif(a.Category = 'B',1,0)) = 0 then 'New Style'
																	   else concat((Stuff((
																						select distinct concat(' ', a2.SewingLineID)
																						from #tmp a2
																						where a2.StyleID = t.StyleID
																						and a2.BrandID = t.BrandID
																						and a2.StyleDesc = t.StyleDesc
                                                                                        and a2.SeasonID = t.SeasonID
                                                                                        and a2.POID = t.POID
                                                                                        and a2.FtyZone = t.FtyZone
                                                                                        and exists( select 1 
                                                                                                     from #tmp_LineMaxOutputDatePOID t2 
                                                                                                     where t2.OutputDate = a2.OutputDate 
                                                                                                     and t2.StyleID = a2.StyleID
                                                                                                     and t2.BrandID = a2.BrandID 
                                                                                                     and t2.StyleDesc = a2.StyleDesc
																									 and t2.SeasonID = a2.SeasonID
																									 and t2.POID = a2.POID
																									 and t2.FtyZone = a2.FtyZone)
																						FOR XML PATH('')) ,1,1,'')),'(',format(Max(a.OutputDate), 'yyyy/MM/dd'),')')
																	   end))
						from #tmp a where a.StyleID = t.StyleID and 
											a.BrandID = t.BrandID and
											a.StyleDesc = t.StyleDesc and 
											a.SeasonID = t.SeasonID and
											a.POID = t.POID and
                                            a.FtyZone = t.FtyZone
						group by a.FtyZone FOR XML PATH(''))
				,1,1,'') 
from (
    select  POID
            , FtyZone
            , StyleID
            , BrandID
            , CdCodeID
            , CDCodeNew
	        , ProductType
	        , FabricType
	        , Lining
	        , Gender
	        , Construction 
            , CDDesc
            , StyleDesc
            , SeasonID
            , ProgramID
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by POID, FtyZone, StyleID, BrandID, CdCodeID, CDDesc, StyleDesc, SeasonID
             , ProgramID, CDCodeNew, ProductType, FabricType, Lining, Gender, Construction 
)t
Order by POID, StyleID, BrandID, CdCodeID, SeasonID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.POCombo);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By PO Combo data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Program
            querySql = @"
select t.StyleID, t.BrandID, t.StyleDesc, t.SeasonID, t.FtyZone, OutputDate = Max(t.OutputDate)
into #tmp_LineMaxOutputDate
from #tmp t
group by t.StyleID, t.BrandID, t.StyleDesc, t.SeasonID, t.FtyZone

select  ProgramID
        , StyleID
        , BrandID
        , CdCodeID
        , CDCodeNew
	    , ProductType
	    , FabricType
	    , Lining
	    , Gender
	    , Construction 
        , CDDesc
        , StyleDesc
        , SeasonID
        , [TtlQty] = SUM(TtlQty)
        , [TtlCPU] = SUM(TtlCPU)
        , [TtlManhour] = SUM(TtlManhour)
        , [PPH] = SUM(IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)))
        , [EFF] = SUM(IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/StdTMS)*100, 2)))
		, [Remark] = Stuff((select concat('/', a.FtyZone + ' ' + (case when Max(a.OutputDate) is null then 'New Style'
																	   else concat((Stuff((
																						select distinct concat(' ', a2.SewingLineID)
																						from #tmp a2
																						where a2.StyleID = t.StyleID
																						and a2.BrandID = t.BrandID
																						and a2.StyleDesc = t.StyleDesc
                                                                                        and a2.SeasonID = t.SeasonID
                                                                                        and a2.FtyZone = a.FtyZone
                                                                                        and exists( select 1 
                                                                                                     from #tmp_LineMaxOutputDate t2 
                                                                                                     where t2.OutputDate = a2.OutputDate 
                                                                                                     and t2.StyleID = a2.StyleID
                                                                                                     and t2.BrandID = a2.BrandID 
                                                                                                     and t2.StyleDesc = a2.StyleDesc
																									 and t2.SeasonID = a2.SeasonID
																									 and t2.FtyZone = a2.FtyZone)
																						FOR XML PATH('')) ,1,1,'')),'(',format(Max(a.OutputDate), 'yyyy/MM/dd'),')')
																	   end))
						from #tmp a where a.StyleID = t.StyleID and 
											a.BrandID = t.BrandID and
											a.StyleDesc = t.StyleDesc and 
											a.SeasonID = t.SeasonID and
                                            a.FtyZone = t.FtyZone
						group by a.FtyZone FOR XML PATH(''))
				,1,1,'') 
from (
    select  ProgramID
            , StyleID
            , BrandID
            , CdCodeID
            , CDCodeNew
	        , ProductType
	        , FabricType
	        , Lining
	        , Gender
	        , Construction 
            , CDDesc
            , StyleDesc
            , SeasonID
            , FtyZone
            , StdTMS
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU, SUM(TtlManhour) AS TtlManhour
    from #tmp, System
    group by ProgramID, StyleID, BrandID, CdCodeID, CDDesc, StyleDesc, SeasonID, FtyZone
           , CDCodeNew, ProductType, FabricType, Lining, Gender, Construction, StdTMS 
)t
group by  ProgramID, StyleID, BrandID, CdCodeID, CDCodeNew, ProductType, FabricType
	    , Lining, Gender, Construction, CDDesc, StyleDesc, SeasonID
Order by ProgramID, StyleID, BrandID, CdCodeID, SeasonID";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.Program);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Program data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion

            #region By Factory-Line-CD
            querySql = @"
select  FtyZone
        , FactoryID
        , SewingLineID
        , CdCodeID
        , CDCodeNew
	    , ProductType
	    , FabricType
	    , Lining
	    , Gender
	    , Construction 
        , CDDesc
        , TtlQty
        , TtlCPU
        , TtlManhour
        , IIF(TtlManhour = 0,0,Round(TtlCPU/TtlManhour, 2)) as PPH
        , IIF(TtlManhour = 0,0,Round(TtlCPU/(TtlManhour*3600/(select StdTMS from System WITH (NOLOCK) ))*100, 2)) as EFF
from (
    select  FtyZone
            , FactoryID
            , SewingLineID
            , CdCodeID
            , CDCodeNew
	        , ProductType
	        , FabricType
	        , Lining
	        , Gender
	        , Construction 
            , CDDesc
            , sum(Output) AS TtlQty
            , SUM(TotalCPU) AS TtlCPU
            , SUM(TtlManhour) AS TtlManhour
    from #tmp
    group by FtyZone, FactoryID, SewingLineID, CdCodeID, CDDesc
         , CDCodeNew, ProductType, FabricType, Lining, Gender, Construction 
)t
Order by FtyZone, FactoryID, SewingLineID, CdCodeID, CDDesc";

            result = MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, querySql, out this.FactoryLineCD);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query By Factory-Line-CD data fail\r\n" + result.ToString());
                return failResult;
            }
            #endregion
            return Ict.Result.True;
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
            string strXltName = Env.Cfg.XltPathDir + "\\Sewing_R03_ProdEfficiencyAnalysisReport.xltx";
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
            objArray = new object[1, 20];
            foreach (DataRow dr in this.Style.Rows)
            {
                objArray[0, 0] = dr["StyleID"];
                objArray[0, 1] = dr["ModularParent"];
                objArray[0, 2] = dr["CPUAdjusted"];
                objArray[0, 3] = dr["BrandID"];
                objArray[0, 4] = dr["CdCodeID"];
                objArray[0, 5] = dr["CDCodeNew"];
                objArray[0, 6] = dr["ProductType"];
                objArray[0, 7] = dr["FabricType"];
                objArray[0, 8] = dr["Lining"];
                objArray[0, 9] = dr["Gender"];
                objArray[0, 10] = dr["Construction"];
                objArray[0, 11] = dr["CDDesc"];
                objArray[0, 12] = dr["StyleDesc"];
                objArray[0, 13] = dr["SeasonID"];
                objArray[0, 14] = dr["TtlQty"];
                objArray[0, 15] = dr["TtlCPU"];
                objArray[0, 16] = dr["TtlManhour"];
                objArray[0, 17] = dr["PPH"];
                objArray[0, 18] = dr["EFF"];
                objArray[0, 19] = dr["Remark"];
                worksheet.Range[string.Format("A{0}:T{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By CD
            worksheet = excel.ActiveWorkbook.Worksheets[5];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 13];
            foreach (DataRow dr in this.CD.Rows)
            {
                objArray[0, 0] = dr["CdCodeID"];
                objArray[0, 1] = dr["CDCodeNew"];
                objArray[0, 2] = dr["ProductType"];
                objArray[0, 3] = dr["FabricType"];
                objArray[0, 4] = dr["Lining"];
                objArray[0, 5] = dr["Gender"];
                objArray[0, 6] = dr["Construction"];
                objArray[0, 7] = dr["CDDesc"];
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
            objArray = new object[1, 16];
            foreach (DataRow dr in this.BrandFactoryCD.Rows)
            {
                objArray[0, 0] = dr["BrandID"];
                objArray[0, 1] = dr["FtyZone"];
                objArray[0, 2] = dr["FactoryID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDCodeNew"];
                objArray[0, 5] = dr["ProductType"];
                objArray[0, 6] = dr["FabricType"];
                objArray[0, 7] = dr["Lining"];
                objArray[0, 8] = dr["Gender"];
                objArray[0, 9] = dr["Construction"];
                objArray[0, 10] = dr["CDDesc"];
                objArray[0, 11] = dr["TtlQty"];
                objArray[0, 12] = dr["TtlCPU"];
                objArray[0, 13] = dr["TtlManhour"];
                objArray[0, 14] = dr["PPH"];
                objArray[0, 15] = dr["EFF"];
                worksheet.Range[string.Format("A{0}:P{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By PO Combo
            worksheet = excel.ActiveWorkbook.Worksheets[8];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 20];
            foreach (DataRow dr in this.POCombo.Rows)
            {
                objArray[0, 0] = dr["POID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDCodeNew"];
                objArray[0, 5] = dr["ProductType"];
                objArray[0, 6] = dr["FabricType"];
                objArray[0, 7] = dr["Lining"];
                objArray[0, 8] = dr["Gender"];
                objArray[0, 9] = dr["Construction"];
                objArray[0, 10] = dr["CDDesc"];
                objArray[0, 11] = dr["StyleDesc"];
                objArray[0, 12] = dr["SeasonID"];
                objArray[0, 13] = dr["ProgramID"];
                objArray[0, 14] = dr["TtlQty"];
                objArray[0, 15] = dr["TtlCPU"];
                objArray[0, 16] = dr["TtlManhour"];
                objArray[0, 17] = dr["PPH"];
                objArray[0, 18] = dr["EFF"];
                objArray[0, 19] = dr["Remark"];
                worksheet.Range[string.Format("A{0}:T{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Program
            worksheet = excel.ActiveWorkbook.Worksheets[9];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 19];
            foreach (DataRow dr in this.Program.Rows)
            {
                objArray[0, 0] = dr["ProgramID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDCodeNew"];
                objArray[0, 5] = dr["ProductType"];
                objArray[0, 6] = dr["FabricType"];
                objArray[0, 7] = dr["Lining"];
                objArray[0, 8] = dr["Gender"];
                objArray[0, 9] = dr["Construction"];
                objArray[0, 10] = dr["CDDesc"];
                objArray[0, 11] = dr["StyleDesc"];
                objArray[0, 12] = dr["SeasonID"];
                objArray[0, 13] = dr["TtlQty"];
                objArray[0, 14] = dr["TtlCPU"];
                objArray[0, 15] = dr["TtlManhour"];
                objArray[0, 16] = dr["PPH"];
                objArray[0, 17] = dr["EFF"];
                objArray[0, 18] = dr["Remark"];
                worksheet.Range[string.Format("A{0}:S{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            #region By Factory Line CD
            worksheet = excel.ActiveWorkbook.Worksheets[10];
            worksheet.Select();
            intRowsStart = 2;
            objArray = new object[1, 16];
            foreach (DataRow dr in this.FactoryLineCD.Rows)
            {
                objArray[0, 0] = dr["FtyZone"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["SewingLineID"];
                objArray[0, 3] = dr["CdCodeID"];
                objArray[0, 4] = dr["CDCodeNew"];
                objArray[0, 5] = dr["ProductType"];
                objArray[0, 6] = dr["FabricType"];
                objArray[0, 7] = dr["Lining"];
                objArray[0, 8] = dr["Gender"];
                objArray[0, 9] = dr["Construction"];
                objArray[0, 10] = dr["CDDesc"];
                objArray[0, 11] = dr["TtlQty"];
                objArray[0, 12] = dr["TtlCPU"];
                objArray[0, 13] = dr["TtlManhour"];
                objArray[0, 14] = dr["PPH"];
                objArray[0, 15] = dr["EFF"];
                worksheet.Range[string.Format("A{0}:P{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            #endregion

            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Select();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Sewing_R03_ProdEfficiencyAnalysisReport");
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
