using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R04
    /// </summary>
    public partial class R04 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DataTable dtAllData;
        private DateTime? date1;
        private DateTime? date2;
        private string category;
        private string mDivision;
        private string factory;
        private string brand;
        private string cdcode;
        private string shift;
        private bool show_Accumulate_output;
        private bool where_reason;

        /// <summary>
        /// R04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            #region load combobox Location M 預設顯示登入的M
            this.comboM.SetDefalutIndex();
            this.comboM.Text = Env.User.Keyword;
            #endregion

            #region load combobox Factory 預設顯示空白
            this.comboFactory.SetDefalutIndex(string.Empty);
            #endregion

            MyUtility.Tool.SetupCombox(this.comboShift, 1, 1, ",Day+Night,Subcon-In,Subcon-Out");
            this.comboCategory.SelectedIndex = 0;
            base.OnFormLoaded();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.date1 = this.dateOoutputDate.Value1;
            this.date2 = this.dateOoutputDate.Value2;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.brand = this.txtbrand.Text;
            this.cdcode = this.txtCDCode.Text;
            this.shift = this.comboShift.Text;
            this.show_Accumulate_output = this.chk_Accumulate_output.Checked;
            this.where_reason = this.chkSewingReasonID.Checked;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘
            this.dtAllData = null;
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"--根據條件撈基本資料
select s.id
	,s.OutputDate
	,s.Category
	,s.Shift
	,s.SewingLineID
	,s.Team
	,s.MDivisionID
	,s.FactoryID
	,sd.OrderId
	,sd.ComboType
	,[ActManPower] = s.Manpower
	,sd.WorkHour
	,sd.QAQty
	,sd.InlineQty
	,o.LocalOrder
	,o.CustPONo
	,[OrderCategory] = isnull(o.Category,'')
	,[OrderType] = isnull(o.OrderTypeID,'')
	,[IsDevSample] = CASE WHEN ot.IsDevSample =1 THEN 'Y' ELSE 'N' END
	,[OrderBrandID] = case 
		when o.BrandID != 'SUBCON-I' then o.BrandID
		when Order2.BrandID is not null then Order2.BrandID
		when StyleBrand.BrandID is not null then StyleBrand.BrandID
		else o.BrandID end    
    ,[OrderCdCodeID] = isnull(o.CdCodeID,'')
	,[OrderProgram] = isnull(o.ProgramID,'')  
	,[OrderCPU] = isnull(o.CPU,0) 
	,[OrderCPUFactor] = isnull(o.CPUFactor,0) 
	,[OrderStyle] = isnull(o.StyleID,'') 
	,[OrderSeason] = isnull(o.SeasonID,'')
	,[MockupBrandID] = isnull(mo.BrandID,'')   
	,[MockupCDCodeID] = isnull(mo.MockupID,'')
	,[MockupProgram] = isnull(mo.ProgramID,'') 
	,[MockupCPU] = isnull(mo.Cpu,0)
	,[MockupCPUFactor] = isnull(mo.CPUFactor,0)
	,[MockupStyle] = isnull(mo.StyleID,'')
	,[MockupSeason] = isnull(mo.SeasonID,'')	
    ,[Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id,sd.ComboType),100)/100
	,System.StdTMS
	,[InspectQty] = isnull(r.InspectQty,0)
	,[RejectQty] = isnull(r.RejectQty,0)
    ,[BuyerDelivery] = format(o.BuyerDelivery,'yyyy/MM/dd')
    ,[OrderQty] = o.Qty
    ,s.SubconOutFty
    ,s.SubConOutContractNumber
    ,o.SubconInType
    ,[SewingReasonDesc] = isnull(sr.SewingReasonDesc,'')
	,[Remark] = isnull(ssd.SewingOutputRemark,'')
    ,o.SciDelivery 
    ,Cancel=iif(o.Junk=1,'Y','' )
into #tmpSewingDetail
from System WITH (NOLOCK),SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId
left join Factory f WITH (NOLOCK) on o.FactoryID = f.id
left join OrderType ot WITH (NOLOCK) on o.OrderTypeID = ot.ID and o.BrandID = ot.BrandID
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
--left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType 
outer apply
(
    select top 1 InspectQty,RejectQty 
    from Rft r WITH (NOLOCK) 
    where r.OrderID = sd.OrderId and r.CDate = s.OutputDate and r.SewinglineID = s.SewingLineID and r.FactoryID = s.FactoryID and r.Shift = s.Shift and r.Team = s.Team
) r
outer apply
(
	select [SewingReasonDesc]=stuff((
		select concat(',',sr.ID+'-'+sr.Description)
		from SewingReason sr
		inner join SewingOutput_Detail sd2 WITH (NOLOCK) on sd2.SewingReasonID=sr.ID
		where sr.Type='SO' 
		and sd2.id = sd.id 
		and sd2.OrderId = sd.OrderId
		for xml path('')
	),1,1,'')
)sr
outer apply
(
	select [SewingOutputRemark]=stuff((
		select concat(',',ssd.Remark)
		from SewingOutput_Detail ssd WITH (NOLOCK) 
		where ssd.ID = sd.ID
		and ssd.OrderId = sd.OrderId
		and isnull(ssd.Remark ,'') <> ''
		for xml path('')
	),1,1,'')
)ssd
outer apply( select BrandID from orders o1 where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from Style where id = o.StyleID 
    and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
where 1=1
"));

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate >= '{0}'" + Environment.NewLine, Convert.ToDateTime(this.date1).ToString("yyyyMMdd")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCmd.Append(string.Format(" and s.OutputDate <= '{0}'" + Environment.NewLine, Convert.ToDateTime(this.date2).ToString("yyyyMMdd")));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and s.MDivisionID = '{0}'" + Environment.NewLine, this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and s.FactoryID = '{0}'" + Environment.NewLine, this.factory));
            }

            if (!MyUtility.Check.Empty(this.category) && this.category.ToUpper() == "MOCKUP")
            {
                sqlCmd.Append(" and s.Category = 'M'" + Environment.NewLine);
            }

            if (this.where_reason)
            {
                sqlCmd.Append(" and sd.SewingReasonID <>''" + Environment.NewLine);
            }

            if (this.chkType.Checked)
            {
                sqlCmd.Append(" and f.Type <> 'S' " + Environment.NewLine);
            }

            if (this.chkOnlyCancelOrder.Checked)
            {
                sqlCmd.Append($@" and o.Junk = 1 " + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.shift))
            {
                switch (this.shift)
                {
                    case "Day+Night":
                        sqlCmd.Append(" and s.Shift <> 'O' and o.LocalOrder <> 1 and o.SubconInType not in (1, 2)" + Environment.NewLine);
                        break;
                    case "Subcon-In":
                        sqlCmd.Append(" and s.Shift <> 'O' and (o.LocalOrder = 1 and o.SubconInType <> 0)" + Environment.NewLine);
                        break;
                    case "Subcon-Out":
                        sqlCmd.Append(" and s.Shift = 'O'" + Environment.NewLine);
                        break;
                }
            }

            sqlCmd.Append(@"--By Sewing單號 & SewingDetail的Orderid,ComboType 作加總 ActManPower,WorkHour,QAQty,InlineQty
select distinct OutputDate
	,Category
	,Shift
	,SewingLineID
	,Team
	,FactoryID
	,MDivisionID
	,OrderId
	,ComboType
	,[ActManPower] = s.Manpower
	,[WorkHour] = sum(Round(WorkHour,3))over(partition by id,OrderId,ComboType)
	,[QAQty] = sum(QAQty)over(partition by id,OrderId,ComboType)
	,[InlineQty] = sum(InlineQty)over(partition by id,OrderId,ComboType)
	,LocalOrder
	,CustPONo
	,OrderCategory
	,OrderType
	,IsDevSample
	,OrderBrandID 
	,OrderCdCodeID
	,OrderProgram
	,OrderCPU
	,OrderCPUFactor
	,OrderStyle
	,OrderSeason
	,MockupBrandID
	,MockupCDCodeID
	,MockupProgram
	,MockupCPU
	,MockupCPUFactor
	,MockupStyle
	,MockupSeason
	,Rate
	,StdTMS
	,InspectQty
	,RejectQty
    ,BuyerDelivery
    ,SciDelivery
    ,OrderQty
    ,SubconOutFty
    ,SubConOutContractNumber
    ,SubconInType
    ,SewingReasonDesc
    ,Remark
    ,Cancel
into #tmpSewingGroup
from #tmpSewingDetail t
outer apply(
	select s.Manpower from SewingOutput s
	where s.ID = t.ID
)s

select t.*
    ,[LastShift] = IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) 
    ,[FtyType] = f.Type
    ,[FtyCountry] = f.CountryID 
    ,[CumulateDate] = (select cumulate from dbo.getSewingOutputCumulateOfDays(IIF(t.Category <> 'M',OrderStyle,MockupStyle),t.SewingLineID,t.OutputDate,t.FactoryID))
into #tmp1stFilter
from #tmpSewingGroup t
left join Factory f on t.FactoryID = f.ID
where 1=1");
            if (!MyUtility.Check.Empty(this.category))
            {
                sqlCmd.Append($" and t.OrderCategory in ({this.category})");
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
                IIF(t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1, 'I',t.Shift) as LastShift,t.Category,t.ComboType,t.SubconOutFty
into #idat
from #tmpSewingGroup t
inner join Order_TmsCost ot WITH (NOLOCK) on ot.id = t.OrderId
inner join #AT A on A.ID = ot.ArtworkTypeID

declare @columnsName nvarchar(max) = stuff((select concat(',[',ArtworkType_Unit,']') from #atall2 for xml path('')),1,1,'')
declare @NameZ nvarchar(max) = (select concat(',[',ArtworkType_Unit,']=isnull([',ArtworkType_Unit,'],0)')from #atall2 for xml path(''))

declare @TTLZ nvarchar(max) = 
(select concat(',[',ArtworkType_Unit,']=sum(isnull(Rate*[',ArtworkType_Unit,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty)'
,iif(ArtworkType_CPU = '', '', concat(',[',ArtworkType_CPU,']=sum(isnull(Rate*[',ArtworkType_CPU,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty)'))
,',[TTL_',ArtworkType_Unit,']=Round(sum(o.QAQty*Rate*[',ArtworkType_Unit,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty),',iif(Unit='QTY','4','3'),')'
,iif(ArtworkType_CPU = '', '', concat(',[TTL_',ArtworkType_CPU,']=Round(sum(o.QAQty*Rate*[',ArtworkType_CPU,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty),',iif(Unit='QTY','4','3'),')'))
)from #atall for xml path(''))" : " ")}

-----by orderid & all ArtworkTypeID
declare @lastSql nvarchar(max) =N'
{(this.chk_Include_Artwork.Checked ?
@"select orderid,SubconOutFty,FactoryID,Team,OutputDate,SewingLineID,LastShift,Category,ComboType,qaqty '+@NameZ+N'
into #oid_at
from
(
	select orderid = i.ID,a.ArtworkType_Unit,i.qaqty,ptq=iif(a.Unit=''QTY'',i.Price,iif(a.Unit=''TMS'',i.TMS,iif(a.Unit=''CPU'',i.Price,i.Qty))),
           i.FactoryID,i.Team,i.OutputDate,i.SewingLineID,i.LastShift,i.Category,i.ComboType,i.SubconOutFty
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
                            WHEN t.LastShift=''I'' and SubconInType in (''1'',''2'') then ''Subcon-In(Sister)''
                            else ''Subcon-In(Non Sister)'' end
		,t.SubconOutFty
        ,t.SubConOutContractNumber
        ,t.Team
        ,t.OrderId
        ,CustPONo
        ,t.BuyerDelivery
        ,t.SciDelivery
        ,t.Cancel
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
		,ActManPower = ActManPower
		,WorkHour
		,ManHour = ROUND(ActManPower*WorkHour,2)
		,TargetCPU = ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)
		,TMS = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*StdTMS
		,CPUPrice = IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)
		,TargetQty = IIF(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)>0,ROUND(ROUND(ActManPower*WorkHour,2)*3600/StdTMS,2)/IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate),0)
		,t.QAQty
		,TotalCPU = ROUND(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty,3)
		,CPUSewer = IIF(ROUND(ActManPower*WorkHour,2)>0,(IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/ROUND(ActManPower*WorkHour,2),0)
		,EFF = ROUND(IIF(ROUND(ActManPower*WorkHour,2)>0,((IIF(t.Category=''M'',MockupCPU*MockupCPUFactor,OrderCPU*OrderCPUFactor*Rate)*t.QAQty)/(ROUND(ActManPower*WorkHour,2)*3600/StdTMS))*100,0),1)
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
        ,t.Remark
        ,t.SewingReasonDesc
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
                           o.Team = t.Team and
                           o.OutputDate = t.OutputDate and
                           o.SewingLineID = t.SewingLineID and
                           o.LastShift = t.LastShift  and
                           o.Category = t.Category and
                           o.ComboType = t.ComboType and 
                           o.SubconOutFty = t.SubconOutFty");
            }

            sqlCmd.Append($@" )a
order by MDivisionID,FactoryID,OutputDate,SewingLineID,Shift,Team,OrderId

drop table #tmpSewingDetail,#tmp1stFilter,#tmpSewingGroup
{(this.chk_Include_Artwork.Checked ? "drop table #atall2,#AT,#atall,#idat,#oid_at" : " ")}
'
EXEC sp_executesql @lastSql
");

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            DualResult result = new DualResult(true);

            foreach (string conString in connectionString)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    result = DBProxy.Current.SelectByConn(conn, sqlCmd.ToString(), null, out this.printData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    if (this.printData != null && this.printData.Rows.Count > 0)
                    {
                        if (this.dtAllData == null)
                        {
                            this.dtAllData = this.printData;
                        }
                        else
                        {
                            this.dtAllData.Merge(this.printData);
                        }
                    }
                }
            }

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            int start_column;
            if (this.dtAllData == null || this.dtAllData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.dtAllData.Rows.Count);

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Sewing_R04_SewingDailyOutputList.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (this.show_Accumulate_output == true)
            {
                start_column = 42;
            }
            else
            {
                start_column = 40;
                objSheets.get_Range("AM:AN").EntireColumn.Delete();
            }

            for (int i = start_column; i < this.dtAllData.Columns.Count; i++)
            {
                objSheets.Cells[1, i + 1] = this.dtAllData.Columns[i].ColumnName;
            }

            string r = MyUtility.Excel.ConvertNumericToExcelColumn(this.dtAllData.Columns.Count);
            objSheets.get_Range("A1", r + "1").Cells.Interior.Color = Color.LightGreen;
            objSheets.get_Range("A1", r + "1").AutoFilter(1);
            bool result = MyUtility.Excel.CopyToXls(this.dtAllData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
