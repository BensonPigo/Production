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


namespace Sci.Production.Sewing
{
    public partial class R04 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? date1, date2;
        string category, mDivision, factory, brand, cdcode;
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboCategory, 1, 1, ",Bulk,Sample,Local Order,Mockup,Bulk+Sample");
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            comboCategory.SelectedIndex = 0;
            comboM.Text = Sci.Env.User.Keyword;
            comboFactory.SelectedIndex = 0;
        }
        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            date1 = dateOoutputDate.Value1;
            date2 = dateOoutputDate.Value2;
            category = comboCategory.Text;
            mDivision = comboM.Text;
            factory = comboFactory.Text;
            brand = txtbrand.Text;
            cdcode = txtCDCode.Text;

            return base.ValidateInput();
        }
        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"--根據條件撈基本資料
select s.id,s.OutputDate,s.Category,s.Shift,s.SewingLineID,s.Team,s.MDivisionID,s.FactoryID
	,sd.OrderId,sd.ComboType,ActManPower = IIF(sd.QAQty=0, s.Manpower, s.Manpower * sd.QAQty),sd.WorkHour,sd.QAQty,sd.InlineQty
	,o.LocalOrder,o.CustPONo,OrderCategory = isnull(o.Category,''),OrderType = isnull(o.OrderTypeID,'')
	,OrderBrandID = isnull(o.BrandID,'')    ,OrderCdCodeID = isnull(o.CdCodeID,'')
	,OrderProgram = isnull(o.ProgramID,'')  ,OrderCPU = isnull(o.CPU,0) ,OrderCPUFactor = isnull(o.CPUFactor,0) ,OrderStyle = isnull(o.StyleID,'') ,OrderSeason = isnull(o.SeasonID,'')
	,MockupBrandID= isnull(mo.BrandID,'')   ,MockupCDCodeID= isnull(mo.MockupID,'')
	,MockupProgram= isnull(mo.ProgramID,'') ,MockupCPU= isnull(mo.Cpu,0),MockupCPUFactor= isnull(mo.CPUFactor,0),MockupStyle= isnull(mo.StyleID,''),MockupSeason= isnull(mo.SeasonID,'')	
	,Rate = isnull(sl.Rate,100)/100,System.StdTMS
	,InspectQty = isnull(r.InspectQty,0),RejectQty = isnull(r.RejectQty,0)
into #tmpSewingDetail
from System WITH (NOLOCK),SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType 
left join Rft r WITH (NOLOCK) on r.OrderID = sd.OrderId and r.CDate = s.OutputDate and r.SewinglineID = s.SewingLineID and r.FactoryID = s.FactoryID and r.Shift = s.Shift and r.Team = s.Team
where 1=1 "));

            if (!MyUtility.Check.Empty(date1))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate >= '{0}' ", Convert.ToDateTime(date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(date2))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate <= '{0}' ", Convert.ToDateTime(date2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", factory));
            }
            if (!MyUtility.Check.Empty(category) && category.ToUpper() == "MOCKUP")
            {
                sqlCmd.Append(" and s.Category = 'M'");
            }

            sqlCmd.Append(@"--By Sewing單號 & SewingDetail的Orderid,ComboType 作加總 ActManPower,WorkHour,QAQty,InlineQty
select distinct OutputDate,Category,Shift,SewingLineID,Team,FactoryID,MDivisionID,OrderId,ComboType
	,ActManPower = Sum(ActManPower)over(partition by id,OrderId,ComboType),WorkHour = sum(WorkHour)over(partition by id,OrderId,ComboType)
	,QAQty = sum(QAQty)over(partition by id,OrderId,ComboType),InlineQty = sum(InlineQty)over(partition by id,OrderId,ComboType)
	,LocalOrder,CustPONo,OrderCategory,OrderType
	,OrderBrandID ,OrderCdCodeID ,OrderProgram ,OrderCPU ,OrderCPUFactor ,OrderStyle ,OrderSeason
	,MockupBrandID,MockupCDCodeID,MockupProgram,MockupCPU,MockupCPUFactor,MockupStyle,MockupSeason
	,Rate,StdTMS,InspectQty,RejectQty
into #tmpSewingGroup
from #tmpSewingDetail
--↓計算累計天數 function table太慢直接寫在這
select distinct scOutputDate = s.OutputDate ,style = IIF(t.Category <> 'M',OrderStyle,MockupStyle),t.SewingLineID,t.FactoryID,t.Shift,t.Team,t.OrderId,t.ComboType
into #stmp
from #tmpSewingGroup t
inner join SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID and s.OutputDate between dateadd(day,-90,t.OutputDate) and  t.OutputDate and s.FactoryID = t.FactoryID
inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
where (o.StyleID = OrderStyle or mo.StyleID = MockupStyle)
--
select w.Hours, w.Date, style = IIF(t.Category <> 'M',OrderStyle,MockupStyle),t.SewingLineID,t.FactoryID,t.Shift,t.Team,t.OrderId,t.ComboType
into #wtmp
from #tmpSewingGroup t
inner join  WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID and w.SewingLineID = t.SewingLineID and w.Date between dateadd(day,-90,t.OutputDate) and t.OutputDate
--
select cumulate = IIF(Count(1)=0, 1, Count(1)),s.style,s.SewingLineID,s.FactoryID,s.Shift,s.Team,s.OrderId,s.ComboType
into #cl
from #stmp s
where s.scOutputDate >
(
	select date = max(Date)
	from #wtmp w 
	left join #stmp s2 on s2.scOutputDate = w.Date and w.style = s2.style and w.SewingLineID = s2.SewingLineID and w.FactoryID = s2.FactoryID and w.Shift = s2.Shift and w.Team = s2.Team
	and w.OrderId = s2.OrderId and w.ComboType = s2.ComboType
	where s2.scOutputDate is null
	and w.style = s.style and w.SewingLineID = s.SewingLineID and w.FactoryID = s.FactoryID and w.Shift = s.Shift and w.Team = s.Team and w.OrderId = s.OrderId and w.ComboType = s.ComboType
)
group by s.style,s.SewingLineID,s.FactoryID,s.Shift,s.Team,s.OrderId,s.ComboType
--↑計算累計天數
select t.*,IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,
f.Type as FtyType,f.CountryID as FtyCountry
,CumulateDate=c.cumulate
into #tmp1stFilter
from #tmpSewingGroup t
left join #cl c on c.style = IIF(t.Category <> 'M',OrderStyle,MockupStyle) and c.SewingLineID = t.SewingLineID and c.FactoryID = t.FactoryID 
				and c.Shift = t.Shift and c.Team = t.Team and c.OrderId = t.OrderId and c.ComboType = t.ComboType
left join Factory f on t.FactoryID = f.ID
where 1=1");
            if (!MyUtility.Check.Empty(category) && category != "Mockup")
            {
                if (category == "Bulk")
                {
                    sqlCmd.Append(" and t.OrderCategory = 'B'");
                }
                else if (category == "Sample")
                {
                    sqlCmd.Append(" and t.OrderCategory = 'S'");
                }
                else if (category == "Bulk+Sample")
                {
                    sqlCmd.Append(" and (t.OrderCategory = 'B' or t.OrderCategory = 'S')");
                }
                else
                {
                    sqlCmd.Append(" and t.LocalOrder = 1");
                }
            }
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and (t.OrderBrandID = '{0}' or t.MockupBrandID = '{0}')", brand));
            }
            if (!MyUtility.Check.Empty(cdcode))
            {
                sqlCmd.Append(string.Format(" and (t.OrderCdCodeID = '{0}' or t.MockupCDCodeID = '{0}')", cdcode));
            }

            sqlCmd.Append(@"
-----Artwork
select ID,Seq,ArtworkUnit,ProductionUnit
into #AT
from ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') and IsTtlTMS = 0 and Junk = 0
--
select ID,Seq,ArtworkType_Unit = concat(ID,iif(Unit='QTY','(Price)',iif(Unit = '','','('+Unit+')'))),Unit
into #atall
from(
	Select ID,Seq,Unit = ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,ProductionUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit =''
	UNION
	Select ID,Seq,ProductionUnit from #AT where ArtworkUnit ='' AND ProductionUnit !=''
	UNION
	Select ID,Seq,'' from #AT where ArtworkUnit ='' AND ProductionUnit =''
)a
-----orderid & ArtworkTypeID & Seq
select distinct ot.ID,ot.ArtworkTypeID,ot.Seq,ot.Qty,ot.Price,ot.TMS
into #idat
from #tmpSewingDetail t
inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = t.OrderId
-----by orderid & all ArtworkTypeID
select orderid
	,[AT(TMS)]					=isnull([AT(TMS)]				  ,0)
	,[BONDING (HAND)(TMS)]		=isnull([BONDING (HAND)(TMS)]	  ,0)
	,[BONDING (MACHINE)(PANEL)]	=isnull([BONDING (MACHINE)(PANEL)],0)
	,[BONDING (MACHINE)(TMS)]	=isnull([BONDING (MACHINE)(TMS)]  ,0)
	,[CARTON(Price)]			=isnull([CARTON(Price)]			  ,0)
	,[CUTTING(TMS)]				=isnull([CUTTING(TMS)]			  ,0)
	,[DIE CUT(TMS)]				=isnull([DIE CUT(TMS)]			  ,0)
	,[DOWN(TMS)]				=isnull([DOWN(TMS)]				  ,0)
	,[EMB_THREAD]				=isnull([EMB_THREAD]			  ,0)
	,[EMBOSS/DEBOSS(PCS)]		=isnull([EMBOSS/DEBOSS(PCS)]	  ,0)
	,[EMBOSS/DEBOSS(Price)]		=isnull([EMBOSS/DEBOSS(Price)]	  ,0)
	,[EMBROIDERY(Price)]		=isnull([EMBROIDERY(Price)]		  ,0)
	,[EMBROIDERY(STITCH)]		=isnull([EMBROIDERY(STITCH)]	  ,0)
	,[Garment Dye(PCS)]			=isnull([Garment Dye(PCS)]		  ,0)
	,[Garment Dye(Price)]		=isnull([Garment Dye(Price)]	  ,0)
	,[GMT WASH(PCS)]			=isnull([GMT WASH(PCS)]			  ,0)
	,[GMT WASH(Price)]			=isnull([GMT WASH(Price)]		  ,0)
	,[HEAT SET PLEAT(PCS)]		=isnull([HEAT SET PLEAT(PCS)]	  ,0)
	,[HEAT SET PLEAT(Price)]	=isnull([HEAT SET PLEAT(Price)]	  ,0)
	,[HEAT TRANSFER(TMS)]		=isnull([HEAT TRANSFER(TMS)]	  ,0)
	,[INSPECTION(TMS)]			=isnull([INSPECTION(TMS)]		  ,0)
	,[LABEL(Price)]				=isnull([LABEL(Price)]			  ,0)
	,[LASER(PANEL)]				=isnull([LASER(PANEL)]			  ,0)
	,[LASER(TMS)]				=isnull([LASER(TMS)]			  ,0)
	,[MODEL]					=isnull([MODEL]					  ,0)
	,[PAD PRINTING(PCS)]		=isnull([PAD PRINTING(PCS)]		  ,0)
	,[PAD PRINTING(Price)]		=isnull([PAD PRINTING(Price)]	  ,0)
	,[PATTERN]					=isnull([PATTERN]				  ,0)
	,[PIPING(Price)]			=isnull([PIPING(Price)]			  ,0)
	,[POLYBAG(Price)]			=isnull([POLYBAG(Price)]		  ,0)
	,[PRINTING(PCS)]			=isnull([PRINTING(PCS)]			  ,0)
	,[PRINTING(Price)]			=isnull([PRINTING(Price)]		  ,0)
	,[QUILTING(AT)(TMS)]		=isnull([QUILTING(AT)(TMS)]		  ,0)
	,[SP_THREAD(Price)]			=isnull([SP_THREAD(Price)]		  ,0)
	,[SUBLIMATION PRINT(TMS)]	=isnull([SUBLIMATION PRINT(TMS)]  ,0)
	,[SUBLIMATION ROLLER(TMS)]	=isnull([SUBLIMATION ROLLER(TMS)] ,0)
	,[SUBLIMATION SPRAY(TMS)]	=isnull([SUBLIMATION SPRAY(TMS)]  ,0)
	,[WELDED(TMS)]				=isnull([WELDED(TMS)]			  ,0)
into #oid_at
from
(
	select orderid = i.ID,a.ArtworkType_Unit,ptq=iif(a.Unit='QTY',i.Price,iif(a.Unit='TMS',i.TMS,i.Qty))
	from #atall a
	left join #idat i on i.ArtworkTypeID = a.ID and i.Seq = a.Seq
)a
PIVOT
(
	min(ptq) for ArtworkType_Unit in(
	[AT(TMS)]
	,[BONDING (HAND)(TMS)]
	,[BONDING (MACHINE)(PANEL)]
	,[BONDING (MACHINE)(TMS)]
	,[CARTON(Price)]
	,[CUTTING(TMS)]
	,[DIE CUT(TMS)]
	,[DOWN(TMS)]
	,[EMB_THREAD]
	,[EMBOSS/DEBOSS(PCS)]
	,[EMBOSS/DEBOSS(Price)]
	,[EMBROIDERY(Price)]
	,[EMBROIDERY(STITCH)]
	,[Garment Dye(PCS)]
	,[Garment Dye(Price)]
	,[GMT WASH(PCS)]
	,[GMT WASH(Price)]
	,[HEAT SET PLEAT(PCS)]
	,[HEAT SET PLEAT(Price)]
	,[HEAT TRANSFER(TMS)]
	,[INSPECTION(TMS)]
	,[LABEL(Price)]
	,[LASER(PANEL)]
	,[LASER(TMS)]
	,[MODEL]
	,[PAD PRINTING(PCS)]
	,[PAD PRINTING(Price)]
	,[PATTERN]
	,[PIPING(Price)]
	,[POLYBAG(Price)]
	,[PRINTING(PCS)]
	,[PRINTING(Price)]
	,[QUILTING(AT)(TMS)]
	,[SP_THREAD(Price)]
	,[SUBLIMATION PRINT(TMS)]
	,[SUBLIMATION ROLLER(TMS)]
	,[SUBLIMATION SPRAY(TMS)]
	,[WELDED(TMS)]
	)
)as pt
where orderid is not null
-----
select MDivisionID,FactoryID
	,FtyType = iif(FtyType='B','Bulk',iif(FtyType='S','Sample',FtyType))
	,FtyCountry,OutputDate,SewingLineID
	,Shift = IIF(LastShift='D','Day',IIF(LastShift='N','Night',IIF(LastShift='O','Subcon-Out','Subcon-In')))
	,Team,t.OrderId,CustPONo
	,Brand = IIF(Category='M',MockupBrandID,OrderBrandID)
	,Category = IIF(Category='M','Mockup',IIF(LocalOrder = 1,'Local Order',IIF(OrderCategory='B','Bulk',IIF(OrderCategory='S','Sample',''))))
	,Program = IIF(Category='M',MockupProgram,OrderProgram)
	,OrderType
	,CPURate = IIF(Category='M',MockupCPUFactor,OrderCPUFactor)
	,Style = IIF(Category='M',MockupStyle,OrderStyle)
	,Season = IIF(Category='M',MockupSeason,OrderSeason)
	,CDNo = IIF(Category='M',MockupCDCodeID,OrderCdCodeID)+'-'+ComboType
	,ActManPower = IIF(QAQty>0,ActManPower/QAQty,ActManPower)
	,WorkHour
	,ManHour = ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)
	,TargetCPU = ROUND(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2)
	,TMS = IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS
	,CPUPrice = IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)
	,TargetQty = IIF(IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2)/IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0)
	,QAQty
	,TotalCPU = IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*QAQty
	,CPUSewer = IIF(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)>0,(IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*QAQty)/ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2),0)
	,EFF = ROUND(IIF(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)>0,((IIF(Category='M',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*QAQty)/(ROUND(IIF(QAQty>0,ActManPower/QAQty,ActManPower)*WorkHour,2)*3600/StdTMS))*100,0),1)
	,RFT = IIF(InspectQty>0,ROUND((InspectQty-RejectQty)/InspectQty*100,2),0)
	,CumulateDate,
	DateRange = IIF(CumulateDate>=10,'>=10',CONVERT(VARCHAR,CumulateDate))
	,InlineQty
	,Diff = QAQty-InlineQty
	,rate
	,[AT(TMS)],[BONDING (HAND)(TMS)],[BONDING (MACHINE)(PANEL)],[BONDING (MACHINE)(TMS)],[CARTON(Price)],[CUTTING(TMS)],[DIE CUT(TMS)],[DOWN(TMS)][EMB_THREAD],[EMBOSS/DEBOSS(PCS)]
	,[EMBOSS/DEBOSS(Price)],[EMBROIDERY(Price)],[EMBROIDERY(STITCH)],[Garment Dye(PCS)],[Garment Dye(Price)][GMT WASH(PCS)],[GMT WASH(Price)],[HEAT SET PLEAT(PCS)],[HEAT SET PLEAT(Price)]
	,[HEAT TRANSFER(TMS)],[INSPECTION(TMS)],[LABEL(Price)][LASER(PANEL)],[LASER(TMS)],[MODEL],[PAD PRINTING(PCS)],[PAD PRINTING(Price)],[PATTERN],[PIPING(Price)],[POLYBAG(Price)]
	,[PRINTING(PCS)],[PRINTING(Price)],[QUILTING(AT)(TMS)],[SP_THREAD(Price)],[SUBLIMATION PRINT(TMS)],[SUBLIMATION ROLLER(TMS)],[SUBLIMATION SPRAY(TMS)],[WELDED(TMS)]
	,[TTL_AT(TMS)]					=Round(QAQty*Rate*[AT(TMS)]					,2)
	,[TTL_BONDING (HAND)(TMS)]		=Round(QAQty*Rate*[BONDING (HAND)(TMS)]		,2)
	,[TTL_BONDING (MACHINE)(PANEL)]	=Round(QAQty*Rate*[BONDING (MACHINE)(PANEL)],2)
	,[TTL_BONDING (MACHINE)(TMS)]	=Round(QAQty*Rate*[BONDING (MACHINE)(TMS)]	,2)
	,[TTL_CARTON(Price)]			=Round(QAQty*Rate*[CARTON(Price)]			,2)
	,[TTL_CUTTING(TMS)]				=Round(QAQty*Rate*[CUTTING(TMS)]			,2)
	,[TTL_DIE CUT(TMS)]				=Round(QAQty*Rate*[DIE CUT(TMS)]			,2)
	,[TTL_DOWN(TMS)]				=Round(QAQty*Rate*[DOWN(TMS)]				,2)
	,[TTL_EMB_THREAD]				=Round(QAQty*Rate*[EMB_THREAD]				,2)
	,[TTL_EMBOSS/DEBOSS(PCS)]		=Round(QAQty*Rate*[EMBOSS/DEBOSS(PCS)]		,2)
	,[TTL_EMBOSS/DEBOSS(Price)]		=Round(QAQty*Rate*[EMBOSS/DEBOSS(Price)]	,2)
	,[TTL_EMBROIDERY(Price)]		=Round(QAQty*Rate*[EMBROIDERY(Price)]		,2)
	,[TTL_EMBROIDERY(STITCH)]		=Round(QAQty*Rate*[EMBROIDERY(STITCH)]		,2)
	,[TTL_Garment Dye(PCS)]			=Round(QAQty*Rate*[Garment Dye(PCS)]		,2)
	,[TTL_Garment Dye(Price)]		=Round(QAQty*Rate*[Garment Dye(Price)]		,2)
	,[TTL_GMT WASH(PCS)]			=Round(QAQty*Rate*[GMT WASH(PCS)]			,2)
	,[TTL_GMT WASH(Price)]			=Round(QAQty*Rate*[GMT WASH(Price)]			,2)
	,[TTL_HEAT SET PLEAT(PCS)]		=Round(QAQty*Rate*[HEAT SET PLEAT(PCS)]		,2)
	,[TTL_HEAT SET PLEAT(Price)]	=Round(QAQty*Rate*[HEAT SET PLEAT(Price)]	,2)
	,[TTL_HEAT TRANSFER(TMS)]		=Round(QAQty*Rate*[HEAT TRANSFER(TMS)]		,2)
	,[TTL_INSPECTION(TMS)]			=Round(QAQty*Rate*[INSPECTION(TMS)]			,2)
	,[TTL_LABEL(Price)]				=Round(QAQty*Rate*[LABEL(Price)]			,2)
	,[TTL_LASER(PANEL)]				=Round(QAQty*Rate*[LASER(PANEL)]			,2)
	,[TTL_LASER(TMS)]				=Round(QAQty*Rate*[LASER(TMS)]				,2)
	,[TTL_MODEL]					=Round(QAQty*Rate*[MODEL]					,2)
	,[TTL_PAD PRINTING(PCS)]		=Round(QAQty*Rate*[PAD PRINTING(PCS)]		,2)
	,[TTL_PAD PRINTING(Price)]		=Round(QAQty*Rate*[PAD PRINTING(Price)]		,2)
	,[TTL_PATTERN]					=Round(QAQty*Rate*[PATTERN]					,2)
	,[TTL_PIPING(Price)]			=Round(QAQty*Rate*[PIPING(Price)]			,2)
	,[TTL_POLYBAG(Price)]			=Round(QAQty*Rate*[POLYBAG(Price)]			,2)
	,[TTL_PRINTING(PCS)]			=Round(QAQty*Rate*[PRINTING(PCS)]			,2)
	,[TTL_PRINTING(Price)]			=Round(QAQty*Rate*[PRINTING(Price)]			,2)
	,[TTL_QUILTING(AT)(TMS)]		=Round(QAQty*Rate*[QUILTING(AT)(TMS)]		,2)
	,[TTL_SP_THREAD(Price)]			=Round(QAQty*Rate*[SP_THREAD(Price)]		,2)
	,[TTL_SUBLIMATION PRINT(TMS)]	=Round(QAQty*Rate*[SUBLIMATION PRINT(TMS)]	,2)
	,[TTL_SUBLIMATION ROLLER(TMS)]	=Round(QAQty*Rate*[SUBLIMATION ROLLER(TMS)]	,2)
	,[TTL_SUBLIMATION SPRAY(TMS)]	=Round(QAQty*Rate*[SUBLIMATION SPRAY(TMS)]	,2)
	,[TTL_WELDED(TMS)]				=Round(QAQty*Rate*[WELDED(TMS)]				,2)
from #tmp1stFilter t
left join #oid_at o on o.orderid = t.OrderId
order by MDivisionID,FactoryID,OutputDate,SewingLineID,LastShift,Team,t.OrderId

drop table #AT,#atall,#idat,#tmpSewingDetail,#oid_at,#tmp1stFilter,#tmpSewingGroup
");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Sewing_R04_SewingDailyOutputList.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + excelFile);//開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            for (int i = 35; i < printData.Columns.Count; i++)
            {
                objSheets.Cells[1, i + 1] = printData.Columns[i].ColumnName;
            }
            bool result = MyUtility.Excel.CopyToXls(printData, "", xltfile: excelFile, headerRow: 1, excelApp: objApp);
            
            
            if (!result) { MyUtility.Msg.WarningBox(result.ToString(), "Warning"); }
            this.HideWaitMessage();
            return true;
        }
    }
}
