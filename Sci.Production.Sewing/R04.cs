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
    /// <summary>
    /// R04
    /// </summary>
    public partial class R04 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? date1;
        private DateTime? date2;
        private string category;
        private string mDivision;
        private string factory;
        private string brand;
        private string cdcode;
        private bool show_Accumulate_output;

        /// <summary>
        /// R04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboCategory, 1, 1, ",Bulk,Sample,Local Order,Garment,Mockup,Bulk+Sample,Bulk+Sample+Garment");
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboCategory.SelectedIndex = 0;
            this.comboM.Text = Sci.Env.User.Keyword;
            this.comboFactory.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.dateOoutputDate.Value1;
            this.date2 = this.dateOoutputDate.Value2;
            this.category = this.comboCategory.Text;
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.brand = this.txtbrand.Text;
            this.cdcode = this.txtCDCode.Text;
            this.show_Accumulate_output = this.chk_Accumulate_output.Checked;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
         DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"--根據條件撈基本資料
select s.id,s.OutputDate,s.Category,s.Shift,s.SewingLineID,s.Team,s.MDivisionID,s.FactoryID
	,sd.OrderId,sd.ComboType,ActManPower = IIF(sd.QAQty=0, s.Manpower, s.Manpower * sd.QAQty),sd.WorkHour,sd.QAQty,sd.InlineQty
	,o.LocalOrder,o.CustPONo,OrderCategory = isnull(o.Category,''),OrderType = isnull(o.OrderTypeID,''), CASE WHEN ot.IsDevSample =1 THEN 'Y' ELSE 'N' END AS IsDevSample
	,OrderBrandID = isnull(o.BrandID,'')    ,OrderCdCodeID = isnull(o.CdCodeID,'')
	,OrderProgram = isnull(o.ProgramID,'')  ,OrderCPU = isnull(o.CPU,0) ,OrderCPUFactor = isnull(o.CPUFactor,0) ,OrderStyle = isnull(o.StyleID,'') ,OrderSeason = isnull(o.SeasonID,'')
	,MockupBrandID= isnull(mo.BrandID,'')   ,MockupCDCodeID= isnull(mo.MockupID,'')
	,MockupProgram= isnull(mo.ProgramID,'') ,MockupCPU= isnull(mo.Cpu,0),MockupCPUFactor= isnull(mo.CPUFactor,0),MockupStyle= isnull(mo.StyleID,''),MockupSeason= isnull(mo.SeasonID,'')	
    ,Rate = isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100,System.StdTMS
	,InspectQty = isnull(r.InspectQty,0),RejectQty = isnull(r.RejectQty,0)
    ,BuyerDelivery = format(o.BuyerDelivery,'yyyy/MM/dd')
    ,OrderQty = o.Qty
    ,s.SubconOutFty
    ,s.SubConOutContractNumber
    ,o.SubconInSisterFty
into #tmpSewingDetail
from System WITH (NOLOCK),SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join OrderType ot WITH (NOLOCK) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
--left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType 
outer apply
(
    select top 1 InspectQty,RejectQty 
    from Rft r WITH (NOLOCK) 
    where r.OrderID = sd.OrderId and r.CDate = s.OutputDate and r.SewinglineID = s.SewingLineID and r.FactoryID = s.FactoryID and r.Shift = s.Shift and r.Team = s.Team
) r
where 1=1 "));

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.category) && this.category.ToUpper() == "MOCKUP")
            {
                sqlCmd.Append(" and s.Category = 'M'");
            }

            sqlCmd.Append(@"--By Sewing單號 & SewingDetail的Orderid,ComboType 作加總 ActManPower,WorkHour,QAQty,InlineQty
select distinct OutputDate,Category,Shift,SewingLineID,Team,FactoryID,MDivisionID,OrderId,ComboType
	,ActManPower = Sum(ActManPower)over(partition by id,OrderId,ComboType),WorkHour = sum(Round(WorkHour,3))over(partition by id,OrderId,ComboType)
	,QAQty = sum(QAQty)over(partition by id,OrderId,ComboType),InlineQty = sum(InlineQty)over(partition by id,OrderId,ComboType)
	,LocalOrder,CustPONo,OrderCategory,OrderType,IsDevSample
	,OrderBrandID ,OrderCdCodeID ,OrderProgram ,OrderCPU ,OrderCPUFactor ,OrderStyle ,OrderSeason
	,MockupBrandID,MockupCDCodeID,MockupProgram,MockupCPU,MockupCPUFactor,MockupStyle,MockupSeason
	,Rate,StdTMS,InspectQty,RejectQty
    ,BuyerDelivery
    ,OrderQty
    ,SubconOutFty
    ,SubConOutContractNumber
    ,SubconInSisterFty
into #tmpSewingGroup
from #tmpSewingDetail
--↓計算累計天數 function table太慢直接寫在這
select distinct scOutputDate = s.OutputDate ,style = IIF(t.Category <> 'M',OrderStyle,MockupStyle),t.SewingLineID,t.FactoryID,t.Shift,t.Team,t.OrderId,t.ComboType
into #stmp
from #tmpSewingGroup t
inner join SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID and s.FactoryID = t.FactoryID
inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID 
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
where (o.StyleID = OrderStyle or mo.StyleID = MockupStyle)
--
select w.Hours, w.Date, style = IIF(t.Category <> 'M',OrderStyle,MockupStyle),t.SewingLineID,t.FactoryID,t.Shift,t.Team,t.OrderId,t.ComboType
into #wtmp
from #tmpSewingGroup t
inner join  WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID and w.SewingLineID = t.SewingLineID and w.Date between dateadd(day,-90,t.OutputDate) and t.OutputDate and isnull(w.Hours,0) != 0
--
select s.scOutputDate,cumulate = IIF(Count(1)=0, 1, Count(1)over(partition by s.style,s.SewingLineID,s.FactoryID,s.Shift,s.Team,s.OrderId,s.ComboType order by s.scOutputDate)),
s.style,s.SewingLineID,s.FactoryID,s.Shift,s.Team,s.OrderId,s.ComboType
into #cl
from #stmp s
where s.scOutputDate >
isnull((
	select date = max(Date)
	from #wtmp w 
	left join #stmp s2 on s2.scOutputDate = w.Date and w.style = s2.style and w.SewingLineID = s2.SewingLineID and w.FactoryID = s2.FactoryID and w.Shift = s2.Shift and w.Team = s2.Team
	and w.OrderId = s2.OrderId and w.ComboType = s2.ComboType
	where s2.scOutputDate is null
	and w.style = s.style and w.SewingLineID = s.SewingLineID and w.FactoryID = s.FactoryID and w.Shift = s.Shift and w.Team = s.Team and w.OrderId = s.OrderId 
	and w.ComboType = s.ComboType
),'1900/01/01')
group by s.scOutputDate,s.style,s.SewingLineID,s.FactoryID,s.Shift,s.Team,s.OrderId,s.ComboType
--↑計算累計天數
select t.*,IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,
f.Type as FtyType,f.CountryID as FtyCountry
,CumulateDate=c.cumulate
into #tmp1stFilter
from #tmpSewingGroup t
left join #cl c on c.style = IIF(t.Category <> 'M',OrderStyle,MockupStyle) and c.SewingLineID = t.SewingLineID and c.FactoryID = t.FactoryID 
				and c.Shift = t.Shift and c.Team = t.Team and c.OrderId = t.OrderId and c.ComboType = t.ComboType and c.scOutputDate = t.OutputDate
left join Factory f on t.FactoryID = f.ID
where 1=1");
            if (!MyUtility.Check.Empty(this.category) && this.category != "Mockup")
            {
                if (this.category == "Bulk")
                {
                    sqlCmd.Append(" and t.OrderCategory = 'B'");
                }
                else if (this.category == "Sample")
                {
                    sqlCmd.Append(" and t.OrderCategory = 'S'");
                }
                else if (this.category == "Garment")
                {
                    sqlCmd.Append(" and t.OrderCategory in ('G')");
                }
                else if (this.category == "Bulk+Sample")
                {
                    sqlCmd.Append(" and (t.OrderCategory = 'B' or t.OrderCategory = 'S')");
                }
                else if (this.category == "Bulk+Sample+Garment")
                {
                    sqlCmd.Append(" and t.OrderCategory in ('B', 'S', 'G')");
                }
                else
                {
                    sqlCmd.Append(" and t.LocalOrder = 1");
                }
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and (t.OrderBrandID = '{0}' or t.MockupBrandID = '{0}')", this.brand));
            }

            if (!MyUtility.Check.Empty(this.cdcode))
            {
                sqlCmd.Append(string.Format(" and (t.OrderCdCodeID = '{0}' or t.MockupCDCodeID = '{0}')", this.cdcode));
            }

            sqlCmd.Append($@"-----Artwork
{(this.chk_Include_Artwork.Checked ? @"select ID,Seq,ArtworkUnit,ProductionUnit
into #AT
from ArtworkType WITH (NOLOCK)
where Classify in ('I','A','P') and IsTtlTMS = 0 and Junk = 0

select ID,Seq
	,ArtworkType_Unit = concat(ID,iif(Unit='QTY','(Price)',iif(Unit = '','','('+Unit+')'))),Unit
	,ArtworkType_CPU = iif(Unit = 'TMS',concat(ID,'(CPU)'),'')
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

select *
into #atall2
from(
	select a.ID,a.Seq,c=1,a.ArtworkType_Unit,a.Unit from #atall a
	UNION
	select a.ID,a.Seq,2,a.ArtworkType_CPU,iif(a.ArtworkType_CPU='','','CPU')from #atall a
	where a.ArtworkType_CPU !=''
)b

-----orderid & ArtworkTypeID & Seq
select distinct ot.ID,ot.ArtworkTypeID,ot.Seq,ot.Qty,ot.Price,ot.TMS,t.QAQty,t.FactoryID,t.Team,t.OutputDate,t.SewingLineID,
                IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,t.Category,t.ComboType
into #idat
from #tmpSewingGroup t
inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = t.OrderId
inner join #AT A on A.ID = ot.ArtworkTypeID

declare @columnsName nvarchar(max) = stuff((select concat(',[',ArtworkType_Unit,']') from #atall2 for xml path('')),1,1,'')
declare @NameZ nvarchar(max) = (select concat(',[',ArtworkType_Unit,']=isnull([',ArtworkType_Unit,'],0)')from #atall2 for xml path(''))

declare @TTLZ nvarchar(max) = 
(select concat(',[',ArtworkType_Unit,']=sum(isnull(Rate*[',ArtworkType_Unit,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType)'
,iif(ArtworkType_CPU = '', '', concat(',[',ArtworkType_CPU,']=sum(isnull(Rate*[',ArtworkType_CPU,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType)'))
,',[TTL_',ArtworkType_Unit,']=Round(sum(o.QAQty*Rate*[',ArtworkType_Unit,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType),2)'
,iif(ArtworkType_CPU = '', '', concat(',[TTL_',ArtworkType_CPU,']=Round(sum(o.QAQty*Rate*[',ArtworkType_CPU,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType),2)'))
)from #atall for xml path(''))" : " ")}
-----by orderid & all ArtworkTypeID
declare @lastSql nvarchar(max) =N'
{(this.chk_Include_Artwork.Checked ? @"select orderid,FactoryID,Team,OutputDate,SewingLineID,LastShift,Category,ComboType,qaqty '+@NameZ+N'
into #oid_at
from
(
	select orderid = i.ID,a.ArtworkType_Unit,i.qaqty,ptq=iif(a.Unit=''QTY'',i.Price,iif(a.Unit=''TMS'',i.TMS,iif(a.Unit=''CPU'',i.Price,i.Qty))),
           i.FactoryID,i.Team,i.OutputDate,i.SewingLineID,i.LastShift,i.Category,i.ComboType
	from #atall2 a left join #idat i on i.ArtworkTypeID = a.ID and i.Seq = a.Seq
)a
PIVOT(min(ptq) for ArtworkType_Unit in('+@columnsName+N'))as pt
where orderid is not null
--group by orderid" : " ")}
'
+N'
select * from(
	select distinct
		 MDivisionID,t.FactoryID
		,FtyType = iif(FtyType=''B'',''Bulk'',iif(FtyType=''S'',''Sample'',FtyType))
		,FtyCountry
        ,t.OutputDate
        ,t.SewingLineID
		,Shift =    CASE    WHEN t.LastShift=''D'' then ''Day''
                            WHEN t.LastShift=''N'' then ''Night''
                            WHEN t.LastShift=''O'' then ''Subcon-Out''
                            WHEN t.LastShift=''I'' and SubconInSisterFty = 1 then ''Subcon-In(Sister)''
                            else ''Subcon-In(Non Sister)'' end
		,t.SubconOutFty
        ,t.SubConOutContractNumber
        ,t.Team
        ,t.OrderId
        ,CustPONo
        ,t.BuyerDelivery
        ,t.OrderQty
		,Brand = IIF(t.Category=''M'',MockupBrandID,OrderBrandID)
		,Category = IIF(t.OrderCategory=''M'',''Mockup'',IIF(LocalOrder = 1,''Local Order'',IIF(t.OrderCategory=''B'',''Bulk'',IIF(t.OrderCategory=''S'',''Sample'',IIF(t.OrderCategory=''G'',''Garment'','''')))))
		,Program = IIF(t.Category=''M'',MockupProgram,OrderProgram)
		,OrderType
        ,IsDevSample
		,CPURate = IIF(t.Category=''M'',MockupCPUFactor,OrderCPUFactor)
		,Style = IIF(t.Category=''M'',MockupStyle,OrderStyle)
		,Season = IIF(t.Category=''M'',MockupSeason,OrderSeason)
		,CDNo = IIF(t.Category=''M'',MockupCDCodeID,OrderCdCodeID)+''-''+t.ComboType
		,ActManPower = IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)
		,WorkHour
		,ManHour = ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)
		,TargetCPU = ROUND(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2)
		,TMS = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS
		,CPUPrice = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)
		,TargetQty = IIF(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)*3600/StdTMS,2)/IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0)
		,t.QAQty
		,TotalCPU = ROUND(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty,2)
		,CPUSewer = IIF(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)>0,(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2),0)
		,EFF = ROUND(IIF(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)>0,((IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/(ROUND(IIF(t.QAQty>0,ActManPower/t.QAQty,ActManPower)*WorkHour,2)*3600/StdTMS))*100,0),1)
		,RFT = IIF(InspectQty>0,ROUND((InspectQty-RejectQty)/InspectQty*100,2),0)
		,CumulateDate
		,DateRange = IIF(CumulateDate>=10,''>=10'',CONVERT(VARCHAR,CumulateDate))
		,InlineQty");
            if (this.show_Accumulate_output == true)
            {
                sqlCmd.Append(@",acc_output.value
                                ,Balance =  t.OrderQty -  acc_output.value 
                            ");
            }

            sqlCmd.Append($@",Diff = t.QAQty-InlineQty
		,rate
		{(this.chk_Include_Artwork.Checked ? "'+@TTLZ+N'" : " ")}
    from #tmp1stFilter t");
            if (this.show_Accumulate_output == true)
            {
                sqlCmd.Append(@"
                                    outer  apply(select value = Sum(SD.QAQty)
                                             from SewingOutput_Detail SD
                                             inner join SewingOutput S on SD.ID=S.ID
                                             where SD.ComboType=t.ComboType
                                               and SD.orderid=t.OrderId
                                               and S.OutputDate <= t.OutputDate) acc_output");
            }

            if (this.chk_Include_Artwork.Checked)
            {
                sqlCmd.Append(@" left join #oid_at o on o.orderid = t.OrderId and 
                           o.FactoryID = t.FactoryID and
                           o.Team       = t.Team and
                           o.OutputDate           = t.OutputDate    and
                           o.SewingLineID          = t.SewingLineID and
                           o.LastShift          = t.LastShift       and
                           o.Category          = t.Category and
                           o.ComboType      =   t.ComboType");
            }

 sqlCmd.Append($@" )a
order by MDivisionID,FactoryID,OutputDate,SewingLineID,Shift,Team,OrderId

drop table #tmpSewingDetail,#tmp1stFilter,#tmpSewingGroup,#cl,#stmp,#wtmp
{(this.chk_Include_Artwork.Checked ? "drop table #atall2,#AT,#atall,#idat,#oid_at" : " ")}
'
EXEC sp_executesql @lastSql
");
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

         DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
         return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            int start_column;
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Sewing_R04_SewingDailyOutputList.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (this.show_Accumulate_output == true)
            {
                start_column = 40;
            }
            else
            {
                start_column = 38;
                objSheets.get_Range("AK:AL").EntireColumn.Delete();
            }

            for (int i = start_column; i < this.printData.Columns.Count; i++)
            {
                objSheets.Cells[1, i + 1] = this.printData.Columns[i].ColumnName;
            }

            string r = MyUtility.Excel.ConvertNumericToExcelColumn(this.printData.Columns.Count);
            objSheets.get_Range("A1", r + "1").Cells.Interior.Color = Color.LightGreen;
            objSheets.get_Range("A1", r + "1").AutoFilter(1);
            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
