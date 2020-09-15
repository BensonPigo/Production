using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class R41 : Win.Tems.PrintForm
    {
        private string SubProcess;
        private string SP;
        private string M;
        private string Factory;
        private string CutRef1;
        private string CutRef2;
        private string processLocation;
        private DateTime? dateBundle1;
        private DateTime? dateBundle2;
        private DateTime? dateBundleScanDate1;
        private DateTime? dateBundleScanDate2;
        private DateTime? dateEstCutDate1;
        private DateTime? dateEstCutDate2;
        private DateTime? dateBDelivery1;
        private DateTime? dateBDelivery2;
        private DateTime? dateSewInLine1;
        private DateTime? dateSewInLine2;

        /// <inheritdoc/>
        public R41(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.Comboload();
            this.comboFactory.SetDataSource();
            this.comboRFIDProcessLocation.SetDataSource();
            this.comboRFIDProcessLocation.SelectedIndex = 0;
        }

        private void Comboload()
        {
            DualResult result;
            if (result = DBProxy.Current.Select(null, "select '' as id union select MDivisionID from factory WITH (NOLOCK) ", out DataTable dtM))
            {
                this.comboM.DataSource = dtM;
                this.comboM.DisplayMember = "ID";
            }
            else
            {
                this.ShowErr(result);
            }
        }

        #region ToExcel3步驟

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.SubProcess = this.txtsubprocess.Text;
            this.SP = this.txtSPNo.Text;
            this.M = this.comboM.Text;
            this.Factory = this.comboFactory.Text;
            this.CutRef1 = this.txtCutRefStart.Text;
            this.CutRef2 = this.txtCutRefEnd.Text;
            this.dateBundle1 = this.dateBundleCDate.Value1;
            this.dateBundle2 = this.dateBundleCDate.Value2;
            this.dateBundleScanDate1 = this.dateBundleScanDate.Value1;
            this.dateBundleScanDate2 = this.dateBundleScanDate.Value2;
            this.dateEstCutDate1 = this.dateEstCutDate.Value1;
            this.dateEstCutDate2 = this.dateEstCutDate.Value2;
            this.dateBDelivery1 = this.dateBDelivery.Value1;
            this.dateBDelivery2 = this.dateBDelivery.Value2;
            this.dateSewInLine1 = this.dateSewInLine.Value1;
            this.dateSewInLine2 = this.dateSewInLine.Value2;
            this.processLocation = this.comboRFIDProcessLocation.Text;
            if (MyUtility.Check.Empty(this.CutRef1) && MyUtility.Check.Empty(this.CutRef2) &&
                MyUtility.Check.Empty(this.SP) &&
                MyUtility.Check.Empty(this.dateEstCutDate.Value1) && MyUtility.Check.Empty(this.dateEstCutDate.Value2) &&
                MyUtility.Check.Empty(this.dateBundleCDate.Value1) && MyUtility.Check.Empty(this.dateBundleCDate.Value2) &&
                MyUtility.Check.Empty(this.dateBundleScanDate.Value1) && MyUtility.Check.Empty(this.dateBundleScanDate.Value2) &&
                MyUtility.Check.Empty(this.dateSewInLine.Value1) && MyUtility.Check.Empty(this.dateSewInLine.Value2) &&
                !this.dateLastSewDate.HasValue)
            {
                MyUtility.Msg.WarningBox("[Cut Ref#][SP#][Est. Cutting Date][Bundle CDate][Bundle Scan Date],[Sewing Inline],[Last Sew. Date] cannot all empty !!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region Append畫面上的條件
            StringBuilder sqlWhere = new StringBuilder();
            StringBuilder sqlWhereFirstQuery = new StringBuilder();
            string joinWorkOrder = string.Empty;
            string whereExistsLastSewDate = string.Empty;
            string whereSewDate = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();
            if (!MyUtility.Check.Empty(this.SubProcess))
            {
                sqlWhere.Append($@" and s.id in ('{this.SubProcess.Replace(",", "','")}') ");
            }

            if (!MyUtility.Check.Empty(this.CutRef1))
            {
                joinWorkOrder = "inner join Workorder w WITH (NOLOCK, index(CutRefNo)) on b.CutRef = w.CutRef and w.MDivisionId = b.MDivisionid ";
                sqlWhereFirstQuery.Append(string.Format(@" and w.CutRef >= '{0}' ", this.CutRef1));
            }

            if (!MyUtility.Check.Empty(this.CutRef2))
            {
                joinWorkOrder = "inner join Workorder w WITH (NOLOCK, index(CutRefNo)) on b.CutRef = w.CutRef and w.MDivisionId = b.MDivisionid ";
                sqlWhereFirstQuery.Append(string.Format(@" and w.CutRef <= '{0}' ", this.CutRef2));
            }

            if (!MyUtility.Check.Empty(this.SP))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and BDO.Orderid = '{0}'", this.SP));
            }

            if (!MyUtility.Check.Empty(this.dateBundle1))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and b.Cdate >= '{0}'", Convert.ToDateTime(this.dateBundle1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateBundle2))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and b.Cdate <= '{0}'", Convert.ToDateTime(this.dateBundle2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateBundleScanDate1) && !MyUtility.Check.Empty(this.dateBundleScanDate2))
            {
                sqlWhere.Append(string.Format(@" and ((convert (date, bio.InComing) >= '{0}' and convert (date, bio.InComing) <= '{1}' ) or (convert (date, bio.OutGoing) >= '{0}' and convert (date, bio.OutGoing) <= '{1}'))", Convert.ToDateTime(this.dateBundleScanDate1).ToString("d"), Convert.ToDateTime(this.dateBundleScanDate2).ToString("d")));
                sqlWhereFirstQuery.Append(string.Format(@" and ((convert (date, bio.InComing) >= '{0}' and convert (date, bio.InComing) <= '{1}' ) or (convert (date, bio.OutGoing) >= '{0}' and convert (date, bio.OutGoing) <= '{1}'))", Convert.ToDateTime(this.dateBundleScanDate1).ToString("d"), Convert.ToDateTime(this.dateBundleScanDate2).ToString("d")));
            }
            else
            {
                if (!MyUtility.Check.Empty(this.dateBundleScanDate1))
                {
                    sqlWhere.Append(string.Format(@" and (convert (date, bio.InComing)  >= '{0}' or convert (date, bio.OutGoing) >= '{0}')", Convert.ToDateTime(this.dateBundleScanDate1).ToString("d")));
                    sqlWhereFirstQuery.Append(string.Format(@" and (convert (date, bio.InComing)  >= '{0}' or convert (date, bio.OutGoing) >= '{0}')", Convert.ToDateTime(this.dateBundleScanDate1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateBundleScanDate2))
                {
                    sqlWhere.Append(string.Format(@" and (convert (date, bio.InComing)  <= '{0}' or convert (date, bio.OutGoing) <= '{0}')", Convert.ToDateTime(this.dateBundleScanDate2).ToString("d")));
                    sqlWhereFirstQuery.Append(string.Format(@" and (convert (date, bio.InComing)  <= '{0}' or convert (date, bio.OutGoing) <= '{0}')", Convert.ToDateTime(this.dateBundleScanDate2).ToString("d")));
                }
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and b.MDivisionid = '{0}'", this.M));
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and o.FtyGroup = '{0}'", this.Factory));
            }

            if (this.processLocation != "ALL")
            {
                sqlWhere.Append(string.Format(@" and isnull(bio.RFIDProcessLocationID,'') = '{0}'", this.processLocation));
                sqlWhereFirstQuery.Append(string.Format(@" and isnull(bio.RFIDProcessLocationID,'') = '{0}'", this.processLocation));
            }

            if (!MyUtility.Check.Empty(this.dateBDelivery1))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and o.BuyerDelivery >= convert(date,'{0}')", Convert.ToDateTime(this.dateBDelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateBDelivery2))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and o.BuyerDelivery <= convert(date,'{0}')", Convert.ToDateTime(this.dateBDelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateSewInLine1))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and o.SewInLine >= convert(date,'{0}')", Convert.ToDateTime(this.dateSewInLine1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateSewInLine2))
            {
                sqlWhereFirstQuery.Append(string.Format(@" and o.SewInLine <= convert(date,'{0}')", Convert.ToDateTime(this.dateSewInLine2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate1))
            {
                joinWorkOrder = "inner join Workorder w WITH (NOLOCK, index(CutRefNo)) on b.CutRef = w.CutRef and w.MDivisionId = b.MDivisionid ";
                sqlWhereFirstQuery.Append(string.Format(@" and w.EstCutDate >= convert(date,'{0}')", Convert.ToDateTime(this.dateEstCutDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate2))
            {
                joinWorkOrder = "inner join Workorder w WITH (NOLOCK, index(CutRefNo)) on b.CutRef = w.CutRef and w.MDivisionId = b.MDivisionid ";
                sqlWhereFirstQuery.Append(string.Format(@" and w.EstCutDate <= convert(date,'{0}')", Convert.ToDateTime(this.dateEstCutDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate1) || !MyUtility.Check.Empty(this.dateEstCutDate2))
            {
                sqlWhereFirstQuery.Append(@" and w.CutRef <> '' ");
            }

            if (this.dateLastSewDate.HasValue)
            {
                whereExistsLastSewDate = $@" and exists(select 1 from View_SewingInfoLocation vsis with (nolock) where vsis.OrderID = o.ID and vsis.LastSewDate between @LastSewDateFrom and @LastSewDateTo)";
                whereSewDate = " and (tsi.LastSewDate between @LastSewDateFrom and @LastSewDateTo)";
                listPar.Add(new SqlParameter("@LastSewDateFrom", this.dateLastSewDate.DateBox1.Value));
                listPar.Add(new SqlParameter("@LastSewDateTo", this.dateLastSewDate.DateBox2.Value));
            }
            #endregion

            #region sqlcmd
            string sqlCmd = string.Empty;
            sqlCmd += $@"
select distinct bd.BundleNo,
                bd.Location,
                b.Article,
                b.ColorID,
                bd.SizeCode,
                bd.Patterncode,
                b.OrderID
into #tmp_Workorder
from Bundle b WITH (NOLOCK)
{joinWorkOrder}
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.Id 
INNER JOIN Bundle_Detail_Order BDO on BDO.BundleNo = BD.BundleNo
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
left join BundleInOut bio WITH (NOLOCK) on bio.Bundleno=bd.Bundleno --and bio.SubProcessId = s.Id
where 1=1
{sqlWhereFirstQuery} 
{whereExistsLastSewDate}

--取得Last Sew. Date相關資料
select  vsl.OrderId, vsl.Article, vsl.SizeCode, vsl.ComboType, vsl.LastSewDate, vsl.SewQty
into #tmpSewingInfo
from dbo.View_SewingInfoLocation vsl with (nolock)
where   exists(select 1 from #tmp_Workorder tw 
                        where   tw.OrderID = vsl.OrderId and
                                tw.Article = vsl.Article and
                                tw.SizeCode = vsl.SizeCode and
                                tw.Location = vsl.ComboType)

alter table #tmpSewingInfo alter column ComboType varchar(8)

insert into #tmpSewingInfo(OrderId, Article, SizeCode, ComboType, LastSewDate, SewQty)
select  OrderId, 
        Article, 
        SizeCode,
        'ALLPARTS',
        [LastSewDate] = max(LastSewDate), 
        [SewQty] = sum(SewQty)
from #tmpSewingInfo
group by OrderId, Article, SizeCode



";

            sqlCmd += $@" 
Select 
    [Bundleno] = bd.BundleNo,
    [RFIDProcessLocationID] = isnull(bio.RFIDProcessLocationID,''),
    [EXCESS] = iif(b.IsEXCESS = 0, '','Y'),
    [Cut Ref#] = isnull(b.CutRef,''),
    [SP#] = b.Orderid,
	sps = (select concat('/',IIF(LEN(OrderID) <= 10, OrderID , substring(OrderID,11,6))) from Bundle_Detail_Order where BundleNo = bd.BundleNo order by OrderID offset 1 rows for xml path('')),
    [Master SP#] = b.POID,
    [M] = b.MDivisionid,
    [Factory] = o.FtyGroup,
	[Category]=o.Category,
	[Program]=o.ProgramID,
    [Style] = o.StyleID,
    [Season] = o.SeasonID,
    [Brand] = o.BrandID,
    [Comb] = b.PatternPanel,
    b.Cutno,
	[Fab_Panel Code] = b.FabricPanelCode,
    [Article] = b.Article,
    [Color] = b.ColorId,
    [Line] = b.SewinglineId,
    bio.SewingLineID,
    [Cell] = b.SewingCell,
    [Pattern] = bd.PatternCode,
    [PtnDesc] = bd.PatternDesc,
    [Group] = bd.BundleGroup,
    [Size] = bd.SizeCode,
    [Artwork] = sub.sub,
    [Qty] = bd.Qty,
    [Sub-process] = s.Id,
    [Post Sewing SubProcess]= iif(ps.sub = 1,N'✔',''),
    [No Bundle Card After Subprocess]= iif(nbs.sub= 1,N'✔',''),
    bio.LocationID,
    b.Cdate,
    o.BuyerDelivery,
    o.SewInLine,
    [InComing] = bio.InComing,
    [Out (Time)] = bio.OutGoing,
    [POSupplier] = iif(PoSuppFromOrderID.Value = '',PoSuppFromPOID.Value,PoSuppFromOrderID.Value),
    [AllocatedSubcon]=Stuff((					
					select concat(',',ls.abb)
					from order_tmscost ot
					inner join LocalSupp ls on ls.id = ot.LocalSuppID
					 where ot.id = o.id and ot.ArtworkTypeID=s.ArtworkTypeId 
					for xml path('')
					),1,1,''),
	AvgTime = case  when s.InOutRule = 1 then iif(bio.InComing is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.InComing,''))/24.0,2))
					when s.InOutRule = 2 then iif(bio.OutGoing is null, null, round(Datediff(Hour,isnull(b.Cdate,''),isnull(bio.OutGoing,''))/24.0,2))
					when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then null
					when s.InOutRule = 3 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.InComing,''),isnull(bio.OutGoing,''))/24.0,2))
					when s.InOutRule = 4 then iif(bio.OutGoing is null or bio.InComing is null, null, round(Datediff(Hour,isnull(bio.OutGoing,''),isnull(bio.InComing,''))/24.0,2))
					end,
	TimeRangeFail = case	when s.InOutRule = 1 and bio.InComing is null then 'No Scan'
						when s.InOutRule = 2 and bio.OutGoing is null then 'No Scan'
						when s.InOutRule in (3,4) and bio.OutGoing is null and bio.InComing is null then 'No Scan'
						when s.InOutRule = 3 and (bio.OutGoing is null or bio.InComing is null) then 'Not Valid'
						when s.InOutRule = 4 and (bio.OutGoing is null or bio.InComing is null) then 'Not Valid'
						else '' end,
	s.InOutRule
	,b.Item
	,bio.PanelNo
	,bio.CutCellID
	,[SpreadingNo] = wk.SpreadingNo
	,[FabricKind] = FabricKind.val
    ,[BundleLocation] = w.Location
into #result
from #tmp_Workorder w 
inner join Bundle_Detail bd WITH (NOLOCK, Index(PK_Bundle_Detail)) on bd.BundleNo = w.BundleNo 
inner join Bundle b WITH (NOLOCK, index(PK_Bundle)) on b.ID = bd.ID
inner join orders o WITH (NOLOCK) on o.Id = b.OrderId and o.MDivisionID  = b.MDivisionID 
inner join factory f WITH (NOLOCK) on o.FactoryID= f.id and f.IsProduceFty=1
outer apply(
    select s.ID,s.InOutRule,s.ArtworkTypeId
    from SubProcess s
        where exists (
                        select 1 from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
                                where   bda.BundleNo = bd.BundleNo    and
                                        bda.ID = b.ID   and
                                        bda.SubProcessID = s.ID
                        ) or  (s.IsSelection = 0 AND s.IsRFIDDefault = 1)
) s
left join BundleInOut bio WITH (NOLOCK, index(PK_BundleInOut)) on bio.Bundleno=bd.Bundleno and bio.SubProcessId = s.Id
outer apply(
	    select sub= stuff((
		    Select distinct concat('+', bda.SubprocessId)
		    from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
		    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno
		    for xml path('')
	    ),1,1,'')
) as sub 
outer apply(
    select sub = 1
    from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.PostSewingSubProcess = 1
    and bda.SubprocessId = s.ID
) as ps
outer apply(
    select sub = 1
    from Bundle_Detail_Art bda with (nolock, index(ID_Bundleno_SubID))
    where bda.Id = bd.Id and bda.Bundleno = bd.Bundleno and bda.NoBundleCardAfterSubprocess = 1
    and bda.SubprocessId = s.ID
) as nbs 
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from ArtworkPO ap with (nolock)
	                                                            inner join ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = b.OrderId 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromOrderID
outer apply (
select [Value] =  case when isnull(bio.RFIDProcessLocationID,'') = '' and isnull(PoSuppFromOrderID.Value,'') = '' then Stuff((select distinct concat( ',',ls.Abb)
	                                                            from ArtworkPO ap with (nolock)
	                                                            inner join ArtworkPO_Detail apd with (nolock) on ap.ID = apd.ID
	                                                            inner join LocalSupp ls with (nolock) on ap.LocalSuppID = ls.ID
	                                                            where ap.POType = 'O' and ap.ArtworkTypeID = s.ArtworkTypeId and apd.OrderID = o.POID 
	                                                            AND (ap.Status ='Approved' OR (ap.Status ='Closed' AND apd.Farmout > 0))                        
	                                                            FOR XML PATH('')),1,1,'')  
                    else '' end
) PoSuppFromPOID
outer apply(
	 select SpreadingNo = stuff((
		    Select distinct concat(',', wo.SpreadingNoID)
		    from WorkOrder wo WITH (NOLOCK, Index(CutRefNo)) 
		    where   wo.CutRef = b.CutRef 
                    and wo.ID = b.POID
                    and wo.MDivisionID = b.MDivisionID
            and wo.SpreadingNoID is not null
            and wo.SpreadingNoID != ''
		    for xml path('')
	    ),1,1,'')
)wk
outer apply(
	SELECT top 1 [val] = DD.id + '-' + DD.NAME 
	FROM dropdownlist DD 
	OUTER apply(
			SELECT OB.kind, 
				OCC.id, 
				OCC.article, 
				OCC.colorid, 
				OCC.fabricpanelcode, 
				OCC.patternpanel 
			FROM order_colorcombo OCC WITH (NOLOCK)
			INNER JOIN order_bof OB WITH (NOLOCK) ON OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
		) LIST 
		WHERE LIST.id = b.poid 
		AND LIST.patternpanel = b.patternpanel 
		AND DD.[type] = 'FabricKind' 
		AND DD.id = LIST.kind 
)FabricKind
";

            sqlCmd += $@" where 1=1 {sqlWhere} ";

            string sqlResult = $@"
{sqlCmd}

;with GetCutDateTmp as
(
	select	r.[Cut Ref#],
			r.M,
			[EstCutDate] = MAX(w.EstCutDate),
			[CuttingOutputDate] = MAX(co.cDate)
	from #result r
	inner join WorkOrder w with (nolock) on w.CutRef = r.[Cut Ref#] and w.MDivisionId = r.M and w.id = r.[Master SP#]
	left join CuttingOutput_Detail cod with (nolock) on cod.WorkOrderUkey = w.Ukey
	left join CuttingOutput co  with (nolock) on co.ID = cod.ID
    where r.[Cut Ref#] <> ''
	group by r.[Cut Ref#],r.M
)
select
    r.[Bundleno] ,
    r.[RFIDProcessLocationID],
	r.[EXCESS],
	r.[FabricKind],
    r.[Cut Ref#] ,
    [SP#] = concat(r.[SP#],r.sps),
    r.[Master SP#],
    r.[M],
    r.[Factory],
	r.[Category],
	r.[Program],
    r.[Style],
    r.[Season],
    r.[Brand],
    r.[Comb],
    r.Cutno,
	r.[Fab_Panel Code],
    r.[Article],
    r.[Color],
    r.[Line],
    r.SewingLineID,
    r.[Cell],
    r.[Pattern],
    r.[PtnDesc],
    r.[Group],
    r.[Size],
    r.[Artwork],
    r.[Qty],
    r.[Sub-process],
    r.[Post Sewing SubProcess],
    r.[No Bundle Card After Subprocess],
    r.LocationID,
    r.Cdate,
    r.[BuyerDelivery],
    r.[SewInLine],
    r.[InComing],
    r.[Out (Time)],
    r.[POSupplier],
    r.[AllocatedSubcon],
	r.AvgTime,
    [TimeRange] = case	when TimeRangeFail <> '' then TimeRangeFail
                        when AvgTime < 0 then 'Not Valid'
						when AvgTime >= 0 and AvgTime < 1 then '<1'
						when AvgTime >= 1 and AvgTime < 2 then '1-2'
						when AvgTime >= 2 and AvgTime < 3 then '2-3'
						when AvgTime >= 3 and AvgTime < 4 then '3-4'
						when AvgTime >= 4 and AvgTime < 5 then '4-5'
						when AvgTime >= 5 and AvgTime < 10 then '5-10'
						when AvgTime >= 10 and AvgTime < 20 then '10-20'
						when AvgTime >= 20 and AvgTime < 30 then '20-30'
						when AvgTime >= 30 and AvgTime < 40 then '30-40'
						when AvgTime >= 40 and AvgTime < 50 then '40-50'
						when AvgTime >= 50 and AvgTime < 60 then '50-60'
						else '>60' end,
    gcd.EstCutDate,
    gcd.CuttingOutputDate
	,r.Item
	,r.PanelNo
	,r.CutCellID
    ,r.SpreadingNo
    ,[LastSewDate] = tsi.LastSewDate
    ,[SewQty] = tsi.SewQty
from #result r
left join GetCutDateTmp gcd on r.[Cut Ref#] = gcd.[Cut Ref#] and r.M = gcd.M 
left join #tmpSewingInfo tsi on tsi.OrderId =   r.[SP#] and 
                                tsi.Article = r.[Article]     and
                                tsi.SizeCode  = r.[Size]   and
                                (tsi.ComboType = r.BundleLocation or tsi.ComboType = r.Pattern)
where 1 = 1 {whereSewDate}
order by [Bundleno],[Sub-process],[RFIDProcessLocationID] 

drop table #result
drop table #tmp_Workorder
";

            #endregion

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R41_Bundle tracking list (RFID).xltx"); // 預先開啟excel app

            // 勿動!! 超過這個數字，DY的電腦會跑不動
            int excelMaxrow = 1000000;

            Microsoft.Office.Interop.Excel.Worksheet worksheet1 = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Worksheet worksheetn = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[2];
            worksheet1.Copy(worksheetn);

            int sheet = 1;

            // 因為一次載入太多筆資料到DataTable 會造成程式佔用大量記憶體，改為每1萬筆載入一次並貼在excel上
            #region 分段抓取資料填入excel
            this.ShowLoadingText($"Data Loading , please wait …");
            DataTable tmpDatas = new DataTable();
            SqlConnection conn = null;
            DBProxy.Current.OpenConnection(this.ConnectionName, out conn);
            var cmd = new SqlCommand(sqlResult, conn);
            foreach (SqlParameter sqlPar in listPar)
            {
                cmd.Parameters.Add(sqlPar);
            }

            cmd.CommandTimeout = 3000;
            var reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
            int loadCounts = 0;
            int loadCounts2 = 0;
            int eachCopy = 100000;
            using (conn)
            {
                using (reader)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        tmpDatas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                    }

                    while (reader.Read())
                    {
                        object[] items = new object[reader.FieldCount];
                        reader.GetValues(items);
                        tmpDatas.LoadDataRow(items, true);
                        loadCounts++;
                        loadCounts2++;
                        if (loadCounts % eachCopy == 0)
                        {
                            this.ShowLoadingText($"Data Loading – {loadCounts} , please wait …");
                            MyUtility.Excel.CopyToXls(tmpDatas, string.Empty, "Subcon_R41_Bundle tracking list (RFID).xltx", loadCounts2 - (eachCopy - 1), false, null, objApp, wSheet: objApp.Sheets[sheet]); // 將datatable copy to excel

                            this.DataTableClearAll(tmpDatas);
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                tmpDatas.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                            }

                            if (loadCounts % excelMaxrow == 0)
                            {
                                Microsoft.Office.Interop.Excel.Worksheet worksheetA = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[sheet + 1];
                                Microsoft.Office.Interop.Excel.Worksheet worksheetB = (Microsoft.Office.Interop.Excel.Worksheet)objApp.ActiveWorkbook.Worksheets[sheet + 2];
                                worksheetA.Copy(worksheetB);
                                sheet++;
                                loadCounts2 = 0;
                            }

                            // loadCounts2 += eachPast;
                        }
                    }

                    if (loadCounts > 0)
                    {
                        MyUtility.Excel.CopyToXls(tmpDatas, string.Empty, "Subcon_R41_Bundle tracking list (RFID).xltx", loadCounts2 - (loadCounts2 % eachCopy) + 1, false, null, objApp, wSheet: objApp.Sheets[sheet]); // 將datatable copy to excel
                        this.DataTableClearAll(tmpDatas);
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Data not found!");
                        this.HideLoadingText();
                        return false;
                    }
                }
            }

            this.SetCount((long)loadCounts);
            objApp.DisplayAlerts = false;
            ((Microsoft.Office.Interop.Excel.Worksheet)objApp.Sheets[sheet + 1]).Delete();
            ((Microsoft.Office.Interop.Excel.Worksheet)objApp.Sheets[1]).Select();
            objApp.DisplayAlerts = true;
            this.HideLoadingText();
            #endregion

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subcon_R41_Bundle tracking list (RFID)");
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);          // 釋放objApp
            Marshal.ReleaseComObject(workbook);

            // printData.Clear();
            // printData.Dispose();
            strExcelName.OpenFile();
            #endregion
            return true;
        }

        #endregion
        private void DataTableClearAll(DataTable target)
        {
            target.Rows.Clear();
            target.Constraints.Clear();
            target.Columns.Clear();
            target.ExtendedProperties.Clear();
            target.ChildRelations.Clear();
            target.ParentRelations.Clear();
        }
    }
}
