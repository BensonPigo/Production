using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateArrStart; DateTime? DateArrEnd;
        List<SqlParameter> lis;
        DataTable[] allDatas;
        string cmd;
        string Supp;
        string refno;
        string brand;
        string season;
        string ReportType;

        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.print.Enabled = false;
            this.txtbrand.MultiSelect = true;
        }

        protected override bool ValidateInput()
        {
            this.lis = new List<SqlParameter>();
            bool DateArr_empty = !this.dateArriveWHDate.HasValue, Supplier_empty = !this.txtsupplier.Text.Empty(), refno_empty = !this.txtRef.Text.Empty(), brand_empty = !this.txtbrand.Text.Empty(),
                 season_empty = !this.txtseason.Text.Empty();
            if (DateArr_empty)
            {
                this.dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("<Arrive W/H> Cannot be empty");
                return false;
            }

            this.DateArrStart = this.dateArriveWHDate.Value1;
            this.DateArrEnd = this.dateArriveWHDate.Value2;
            this.brand = this.txtbrand.Text;
            this.refno = this.txtRef.Text.ToString();
            this.season = this.txtseason.Text;
            this.Supp = this.txtsupplier.TextBox1.Text;
            this.ReportType = this.radioPanel.Value;
            this.lis = new List<SqlParameter>();
            string sqlWhere = string.Empty;
            string sqlRWhere = string.Empty;
            string sqlSuppWhere = string.Empty;
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            #region --組WHERE--

            if (!this.dateArriveWHDate.Value1.Empty())
            {
                RWheres.Add("r.WhseArrival >= @DateArrStart");
                this.lis.Add(new SqlParameter("@DateArrStart", this.DateArrStart));
            }

            if (!this.dateArriveWHDate.Value2.Empty())
            {
                RWheres.Add("r.WhseArrival <= @DateArrEnd");
                this.lis.Add(new SqlParameter("@DateArrEnd", this.DateArrEnd));
            }

            if (!this.Supp.Empty())
            {
                sqlSuppWhere = " and a.SuppID = @Supp ";
                this.lis.Add(new SqlParameter("@Supp", this.Supp));
            }

            if (!this.refno.Empty())
            {
                sqlWheres.Add("psd.Refno = @refno");
                this.lis.Add(new SqlParameter("@refno", this.refno));
            }

            if (!this.brand.Empty())
            {
                string str_multi = string.Empty;
                foreach (string v_str in this.brand.Split(','))
                {
                    str_multi += "," + "'" + v_str + "'";
                }

                sqlWheres.Add(string.Format("o.brandid in ({0})", str_multi.Substring(1)));

                // lis.Add(new SqlParameter("@brand", brand));
            }

            if (!this.season.Empty())
            {
                sqlWheres.Add("o.Seasonid = @season");
                this.lis.Add(new SqlParameter("@season", this.season));
            }

            sqlWhere = string.Join(" and ", sqlWheres);
            sqlRWhere = string.Join(" and ", RWheres);

            if (!sqlWhere.Empty())
            {
                sqlWhere = " and " + sqlWhere;
            }

            if (!MyUtility.Check.Empty(sqlRWhere))
            {
                sqlRWhere = " and " + sqlRWhere;
            }

            #endregion
            #region --撈Excel資料--
            string groupby_col = this.ReportType.Equals("supplier") ? "SuppID" : "RefNo";
            string ttlqty_col = this.ReportType.Equals("supplier") ? "RefNo" : "SuppID";
            this.cmd = string.Format($@"

select distinct a.PoId,a.Seq1,a.Seq2,ps.SuppID,psd.Refno ,psd.ColorID,f.Clima
into #tmp1
from
(
	select r.PoId,r.Seq1,r.Seq2
	from dbo.View_AllReceivingDetail r with (nolock)
	where 1=1
    {sqlRWhere }
	and r.Status = 'Confirmed'
	union
	select sd.ToPOID as PoId,sd.ToSeq1 as Seq1,sd.ToSeq2 as Seq2
	from SubTransfer s
	inner join SubTransfer_Detail sd on s.Id = sd.ID
	where 1=1
    {sqlRWhere.Replace("r.WhseArrival", "s.IssueDate")}
	and s.Status = 'Confirmed'
	and s.Type = 'B'
	union
	select bd.ToPOID as PoId,bd.ToSeq1 as Seq1,bd.ToSeq2 as Seq2
	from BorrowBack b
	inner join BorrowBack_Detail bd on b.Id = bd.ID
	where 1=1
    {sqlRWhere.Replace("r.WhseArrival", "b.IssueDate")}
	and b.Status = 'Confirmed'
	and (b.Type = 'A' or b.Type = 'B')
) a
left join Orders o on o.ID = a.PoId
left join PO_Supp ps on ps.ID = a.PoId and ps.SEQ1 = a.Seq1
left join PO_Supp_Detail psd on psd.ID = a.PoId and psd.SEQ1 = a.Seq1 and psd.SEQ2 = a.Seq2
left join Fabric f on f.SCIRefno = psd.SCIRefno 
where psd.FabricType = 'F'
{sqlWhere }
------------Fabric Defect ----- 
select rd.PoId,rd.Seq1,rd.Seq2,rd.ActualQty,rd.Dyelot,rd.Roll,t.SuppID,t.Refno,t.Colorid ,t.Clima
into #tmpAllData
from #tmp1 t
inner join dbo.View_AllReceivingDetail rd on t.PoId = rd.PoId and t.Seq1 = rd.Seq1 and t.Seq2 = rd.Seq2
------------Group by Supp 
select PoId,Seq1,Seq2,{groupby_col},ActualQty = sum(ActualQty)  
into #GroupBySupp
from #tmpAllData
group by PoId,Seq1,Seq2,{groupby_col}
------------PhyscialEncode=1 group by Supp ---- 
select distinct t.{groupby_col},t.PoId,t.Seq1,t.Seq2 
into #tmpsuppEncode
from #GroupBySupp t
inner join FIR f on t.PoId = f.POID and t.Seq1 = f.SEQ1 and t.Seq2 = f.SEQ2
where f.PhysicalEncode = 1
------------Count Total Orders# Group by PoID ---- 
select t.{groupby_col},count(distinct o.ID)cnt 
into #tmpCountSP
from #tmpAllData t
inner join orders o on t.PoId=o.POID
group by t.{groupby_col}
------------Total Dyelot group by SuppID-------------
select distinct g.{groupby_col},fp.Dyelot  
into #tmpsd
from FIR f
inner join #GroupBySupp g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
inner join FIR_Physical fp on f.ID=fp.ID
where f.PhysicalEncode=1

select distinct g.{groupby_col},s.cnt 
into #tmpDyelot
from #tmpsuppEncode g
outer apply(
	select ss.{groupby_col},count(ss.Dyelot) cnt
		from #tmpsd ss 
		where g.{groupby_col}=ss.{groupby_col}
		group by ss.{groupby_col}
) s
order by g.{groupby_col}
------------Total dye lots accepted(Shadeband)-------------
{(this.ReportType.Equals("supplier") ? @"
----從篩選過的物料，找出他們的FIR紀錄
SELECT f.*
INTO #FirData
FROM #tmp1 t
INNER JOIN FIR f ON t.PoId=f.POID AND t.Seq1=f.SEQ1 AND t.Seq2 = f.Seq2 AND t.SuppID = f.Suppid AND t.Refno=f.Refno 

----從得到的FIR紀錄，取得Fir_shadebone紀錄
SELECT a.Suppid, b.ID,b.Roll,b.Dyelot,b.Result
INTO #All_Fir_shadebone
FROM #FirData a
INNER JOIN Fir_shadebone b ON a.id=b.id

----統計有哪些Dyelot，是全部Pass的
SELECT t.SuppID ,[PassCTN]=COUNT(Dyelot)
INTO #PassCountByDyelot
FROM #tmpsd t
/*OUTER APPLY(
	SELECT [PassCTN]=COUNT(a.ID)
	FROM #All_Fir_shadebone a
	WHERE a.Suppid=t.SuppID AND a.Dyelot = t.Dyelot AND a.Result = 'Pass'
)PassCTN*/
WHERE NOT EXISTS(SELECT * FROM #All_Fir_shadebone b WHERE  b.Suppid=t.SuppID AND b.Dyelot = t.Dyelot AND b.Result <> 'Pass')
GROUP BY t.SuppID
" : string.Empty)}

------------Total Point----------
select g.{groupby_col},sum(fp.TotalPoint) TotalPoint
into #tmpTotalPoint
from FIR f
inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
inner join FIR_Physical fp on f.ID=fp.ID
group by g.{groupby_col}
-----Total Roll Count----------
select g.{groupby_col},count(fp.Roll) TotalRoll
into #tmpTotalRoll
from FIR f
inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
inner join FIR_Physical fp on f.ID=fp.ID
group by g.{groupby_col}
---------Grade A Roll Count---------------------
select g.{groupby_col},count(fp.Grade) GradeA_Roll
into #tmpGrade_A
from FIR f
inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
inner join FIR_Physical fp on f.ID=fp.ID
where fp.Grade='A'
group by g.{groupby_col}
----------Grade B Roll Count---------------------
select g.{groupby_col},count(fp.Grade) GradeB_Roll
into #tmpGrade_B
from FIR f
inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
inner join FIR_Physical fp on f.ID=fp.ID
where fp.Grade='B'
group by g.{groupby_col}
----------Grade C Roll Count---------------------
select g.{groupby_col},count(fp.Grade) GradeC_Roll
into #tmpGrade_C
from FIR f
inner join #tmpsuppEncode g on f.POID=g.PoId and f.SEQ1=g.Seq1 and f.SEQ2=g.Seq2
inner join FIR_Physical fp on f.ID=fp.ID
where fp.Grade='C'
group by g.{groupby_col}
------------Kinds of Fabric Defects (Defect Name)---- 
select t.{groupby_col},fpd.DefectRecord,t.PoId,t.Seq1,t.Seq2 
into #tmpsuppdefect
from #GroupBySupp t
inner join FIR f on t.PoId = f.POID and t.Seq1 = f.SEQ1 and t.Seq2 = f.SEQ2
inner join FIR_Physical fp on fp.ID = f.ID
inner join FIR_Physical_Defect fpd on fpd.FIR_PhysicalDetailUKey = fp.DetailUkey
where f.PhysicalEncode = 1
------------Group by Dyelot------------- 
select PoId,Seq1,Seq2,SuppID,Refno,Dyelot,sum(ActualQty) as ActualQty 
into #tmp2groupbyDyelot
from #tmpAllData
group by PoId,Seq1,Seq2,SuppID,Refno,Dyelot
order by PoId,Seq1,Seq2,SuppID,Refno,Dyelot
------------Group by Roll--------------- 
select PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll,sum(ActualQty) as ActualQty 
into #tmp2groupByRoll
from #tmpAllData
group by PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll
order by PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll
-----#spr 
select distinct a.{groupby_col},
Defect = STUFF((
			select concat('/',s.DefectRecord) 
			from (
				select distinct DefectRecord
				from #tmpsuppdefect b
				WHERE b.{groupby_col} = a.{groupby_col}
			)s
			for xml path('')
		 ), 1, 1, '')
into #spr
from #tmpsuppdefect a
-------tmp 
select distinct s.SuppID
	,ref.Refno
	,brand.brandid
	,s.AbbEN
	,TotalInspYds = (select isnull(sum(TotalInspYds),0) from FIR a inner join #GroupBySupp b on a.POID = b.PoId and a.SEQ1 = b.Seq1 and a.Seq2 = b.Seq2 where a.Suppid = s.SuppID and a.refno=ref.Refno)
	,stockqty = stock.ActualQty
	,yrds = (
		select [Yrds] = count(*) * 5 
		from #tmpsuppdefect a inner join #GroupBySupp b on a.POID=b.PoId and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
		where b.{groupby_col} = gbs.{groupby_col}
	)
	,Point.Defect
	,Point.ID
	,Point.Point
    ,ref.Clima
into #tmp
from (select {groupby_col} from #GroupBySupp group by {groupby_col}) as gbs
outer apply(	
	SELECT Refno,Clima=cast(max(cast(Clima as int))as bit)
	FROM #tmpAllData 
	WHERE #tmpAllData.{groupby_col} = gbs.{groupby_col}
	group by RefNo

) as ref
cross apply(	
	SELECT distinct #tmpAllData.SuppID,Supp.AbbEN
	FROM #tmpAllData 
    inner join Supp WITH (NOLOCK) on  Supp.id = #tmpAllData.SuppID
	WHERE #tmpAllData.{groupby_col} = gbs.{groupby_col}
) as s
outer apply(
	select Defect = fd.DescriptionEN, fd.ID, Point = sum(a.point)  
	from (
		select substring(data,1,1) as Defect,CONVERT(int, substring(data,2,5)) as Point 
		from dbo.SplitString((select Defect from #spr WHERE {groupby_col} = gbs.{groupby_col}),'/') 
	)A 
	left join FabricDefect fd on id = a.Defect
	group by fd.DescriptionEN,fd.ID
) as Point
outer apply(
	select BrandID = stuff((
		select concat(',',BrandID)
		from (
			select distinct o.BrandID 
			from orders o
			inner join #tmpAllData t on o.poid=t.poid
            where t.refno=ref.refno
		) t
		for xml path('')
		),1,1,'')
)as Brand
outer apply(
	select isnull(sum(ActualQty),0) as ActualQty,{ttlqty_col} 
	from #tmpAllData
	where Refno=ref.Refno
	and SuppID=s.SuppID
	group by {ttlqty_col} 	
)as stock
order by Refno
-------tmp2 
select {groupby_col},defect,ID,
point  = sum(point) over(partition by {groupby_col},defect)
,ROW_NUMBER() over(partition by {groupby_col} order by {groupby_col},point desc ,ID desc) RN 
into #tmp2
from #tmp
order by {groupby_col},point desc,ID desc
-----#SHtmp 
select FL.POID, FL.SEQ1, FL.seq2, h.Dyelot
into #SH1
from FIR_Laboratory FL WITH (NOLOCK) 
inner join dbo.FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = FL.ID 	
where FL.HeatEncode=1 and h.Result = 'Fail'
select FL.POID, FL.SEQ1, FL.seq2, W.Dyelot
into #SH2
from dbo.FIR_Laboratory FL WITH (NOLOCK) 
inner join dbo.FIR_Laboratory_Wash W WITH (NOLOCK) on W.ID = FL.ID						
where FL.WashEncode=1 and W.Result = 'Fail'

select distinct SHRINKAGEyards = stockqty,{groupby_col}
into #SHtmp
from(
	select Sum(rd1.stockqty) stockqty, {groupby_col}	
	from  #tmp2groupbyDyelot rd WITH (NOLOCK) 
    inner join dbo.View_AllReceivingDetail rd1 with (nolock) on rd.PoId = rd1.PoId and rd.Seq1 = rd1.Seq1 and rd.Seq2 = rd1.Seq2
	Where exists(select * from #SH1 where POID = rd.PoId and SEQ1 = RD.seq1 and seq2 = seq2 and Dyelot = RD.dyelot)
	or exists(select * from #SH2 where POID = RD.poid and SEQ1 = RD.seq1 and seq2 = RD.seq2 and Dyelot = RD.dyelot)
	group by {groupby_col}
) a
-----#mtmp 
select l.POID,l.SEQ1,l.seq2, F.{groupby_col}
INTO #ea
from dbo.FIR_Laboratory l WITH (NOLOCK) 
inner join dbo.FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = l.ID
INNER JOIN FIR F WITH (NOLOCK) ON L.POID=F.POID
where l.CrockingEncode=1 and h.Result = 'Fail' 
select O.poid,OD.seq1,OD.seq2,F.{groupby_col}
INTO #eb
from Oven O WITH (NOLOCK) 
inner join Oven_Detail OD WITH (NOLOCK) on OD.ID = O.ID
INNER JOIN FIR F WITH (NOLOCK) ON O.POID=F.POID 
where O.Status ='Confirmed' and OD.Result = 'Fail'
select CF.poid,CFD.seq1,CFD.seq2,F.{groupby_col}
into #ec
from ColorFastness CF WITH (NOLOCK) 
inner join ColorFastness_Detail CFD WITH (NOLOCK) on CFD.ID = CF.ID
INNER JOIN FIR F WITH (NOLOCK) ON CF.POID=F.POID 
where CF.Status ='Confirmed' and CFD.Result = 'Fail' 

select [MIGRATIONyards] =sum(a.StockQty),tmp.{groupby_col}
into #mtmp
from(
	select rd.poid,rd.seq1,rd.seq2,rd.dyelot,rd.StockQty,ps.SuppID,psd.Refno 
	from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
	inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2
	left join PO_Supp_Detail psd on psd.id=r.PoId and psd.SEQ1 = r.Seq1 and psd.SEQ2 = r.Seq2
	left join  PO_Supp ps on ps.id=psd.Id and ps.seq1=psd.seq1
)a
inner join (select distinct {groupby_col} from #tmp) tmp on a.{groupby_col} = tmp.{groupby_col} 
Where exists(select * from #ea where POID = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2  AND a.{groupby_col} = Tmp.{groupby_col} ) 
or exists(select * from #eb where POID = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2  AND a.{groupby_col} = Tmp.{groupby_col} ) 
or exists(select * from #ec where POID = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2  AND a.{groupby_col} = Tmp.{groupby_col} )
group by tmp.{groupby_col}

-----#Stmp 
select f.poid,f.SEQ1,f.seq2,fs.Dyelot,f.{groupby_col}
into #sa
from fir f WITH (NOLOCK)
inner join FIR_Shadebone fs WITH (NOLOCK) on fs.ID = f.ID
where f.ShadebondEncode =1 and fs.Result = 'Fail' 
select f.poid,f.SEQ1,f.seq2,fc.Dyelot,f.{groupby_col}
into #sb
from fir f WITH (NOLOCK) 
inner join FIR_Continuity fc WITH (NOLOCK) on fc.ID = f.ID
where f.ContinuityEncode =1 and fc.Result = 'Fail' 

select [SHADINGyards] =sum(a.StockQty),tmp.{groupby_col}
into #Stmp
from(
	select rd.poid,rd.seq1,rd.seq2,rd.dyelot,rd.StockQty,ps.SuppID,psd.Refno 
	from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
	inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2
	left join PO_Supp_Detail psd on psd.id=r.PoId and psd.SEQ1 = r.Seq1 and psd.SEQ2 = r.Seq2
	left join  PO_Supp ps on ps.id=psd.Id and ps.seq1=psd.seq1
)a
inner join (select distinct {groupby_col} from #tmp) tmp on a.{groupby_col} = tmp.{groupby_col} 
Where (exists(select * from #sa where poid = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2 and Dyelot  = a.dyelot and a.{groupby_col} = Tmp.{groupby_col} ) 
or exists(select * from #sb where poid = a.poid and SEQ1 = a.seq1 and seq2 = a.seq2 and Dyelot  = a.dyelot and a.{groupby_col} = Tmp.{groupby_col} ))
group by tmp.{groupby_col}

-----#Ltmp 

select ActualYds = sum(fp.TicketYds - fp.ActualYds), t.{groupby_col}
into #Ltmp
from FIR f
inner join FIR_Physical fp on f.ID = fp.ID
inner join #GroupBySupp t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2
where f.PhysicalEncode = 1 and fp.ActualYds < fp.TicketYds
group by t.{groupby_col}
-----#Sdtmp 
select SHORTWIDTH = sum(t.ActualQty)/5, f.{groupby_col}
into #Sdtmp
from  fir f WITH (NOLOCK) 
inner join FIR_Physical fp on f.ID = fp.ID
inner join #tmp2groupByRoll t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2 and fp.Dyelot = t.Dyelot and fp.Roll = t.Roll
left join Fabric on Fabric.SCIRefno = f.SCIRefno
where f.PhysicalEncode = 1 and fp.ActualWidth < Fabric.Width
group by f.{groupby_col}
------#FabricInspDoc TestReport
select 
	a.{groupby_col}
	, iif(ccnt=0, 0, round(ccnt/bcnt,4)) [cnt] 
into #tmpTestReport
from(
	select tmp.{groupby_col}, count(b.PoId)*1.0 bcnt, count(c.TPETestReport)*1.0 ccnt 
	from (
	select distinct {groupby_col}
		   , poid
		   , seq1
		   , seq2
	from #tmpAllData
	) tmp
	left join Export_Detail b on tmp.PoId =b.PoID and tmp.Seq1 =b.Seq1 and tmp.Seq2 = b.Seq2 and tmp.{groupby_col} = b.{groupby_col}
	left join SentReport c on b.Ukey = c.Export_DetailUkey
	group by tmp.{groupby_col}
)a
------#FabricInspDoc Inspection Report
select 
	a.{groupby_col}
	, iif(ccnt=0, 0, round(ccnt/bcnt,4)) [cnt] 
into #InspReport
from(
	select tmp.{groupby_col}, count(b.PoId)*1.0 bcnt, count(c.TPEInspectionReport)*1.0 ccnt 
	from (
	select distinct {groupby_col}
		   , poid
		   , seq1
		   , seq2
	from #tmpAllData
	) tmp
	left join Export_Detail b on tmp.PoId =b.PoID and tmp.Seq1 =b.Seq1 and tmp.Seq2 = b.Seq2 and tmp.{groupby_col} = b.{groupby_col}
	left join SentReport c on b.Ukey = c.Export_DetailUkey
	group by tmp.{groupby_col}
)a

------#FabricInspDoc Approved Continuity Card Provided %
select a.{groupby_col}
	, iif(ccnt=0, 0, round(ccnt/bcnt,4)) [cnt] 
into #tmpContinuityCard
from(
	select tmp.{groupby_col}, count(b.PoId)*1.0 bcnt, count(c.ContinuityCard)*1.0 ccnt 
	from (
	select distinct {groupby_col}
		   , poid
		   , seq1
		   , seq2
	from #tmpAllData
	) tmp
	left join Export_Detail b on tmp.PoId =b.PoID and tmp.Seq1 =b.Seq1 and tmp.Seq2 = b.Seq2 and tmp.{groupby_col} = b.{groupby_col}
	left join SentReport c on b.Ukey = c.Export_DetailUkey
	group by tmp.{groupby_col}
)a

------#FabricInspDoc Approved 1st Bulk Dyelot Provided %
select b.Refno, b.ColorID, b.SuppID, d.Consignee, c.SeasonSCIID DyelotSeasion, c.FirstDyelot, e.SeasonSCIID, c.Period, f.RibItem 
into #tmp_DyelotMain 
from (
		select distinct SuppID
				, poid
				, seq1
				, seq2
		from #tmpAllData 
	) tmp
outer apply (
	select a.id,a.seq1,a.seq2,a.SCIRefno,a.ColorID,b.SuppID,a.RefNo 
	from PO_Supp_Detail a
	inner join PO_Supp b on a.ID = b.ID and a.seq1 = b.seq1 
	where a.id =  tmp.poid
	and a.seq1 = tmp.seq1
	and a.seq2 = tmp.seq2
)b
LEFT JOIN Export_Detail ED ON ED.PoID = TMP.POID AND ED.Seq1 = TMP.SEQ1 AND ED.Seq2 = TMP.SEQ2
left join Export d on ED.ID = D.ID AND D.Confirm = 1
left join Factory fty with (nolock) on fty.ID = d.Consignee
left join FIRSTDYELOT c on b.Refno = c.Refno and b.ColorID = c.ColorID and b.SuppID = c.SuppID  AND c.TestDocFactoryGroup = fty.TestDocFactoryGroup 
left join orders o on ed.PoID = o.id and o.Category in ('B','M')
left join Season e on o.SeasonID  = e.ID and o.BrandID = e.BrandID --and e.SeasonSCIID = c.SeasonSCIID
left join Fabric f on f.SCIRefno = b.SCIRefno 
group by b.Refno, b.ColorID, b.SuppID, d.Consignee, c.SeasonSCIID, c.FirstDyelot, e.SeasonSCIID,c.Period,f.RibItem 

 --分母
 select {groupby_col}, count(*)*1.0 Mcnt
 into #tmp_DyelotMcnt
 from (
	 select Refno, ColorID, SuppID, Consignee, SeasonSCIID, Period, RibItem  
	 from #tmp_DyelotMain
	 where not(FirstDyelot is null and RibItem = 1) 
	 group by Refno, ColorID, SuppID, Consignee, SeasonSCIID, Period, RibItem 
 )a
 group by {groupby_col}

--重新計算月份
select 
ROW_NUMBER() OVER(order by month ASC) as rid
,* 
into #tmp_newSeasonSCI
from SeasonSCI 

select a.*, b.rid, (b.rid + a.Period -1) maxID
into #tmp_DyelotMonth
from #tmp_DyelotMain a
left join  #tmp_newSeasonSCI b on a.DyelotSeasion = b.id
where a.FirstDyelot is not null

 --分子
select a.{groupby_col},count(*)*1.0 Dcnt
 into #tmp_DyelotDcnt
 from (
	 select Refno,ColorID,SuppID,Consignee,SeasonSCIID 
	 from #tmp_DyelotMain 
	 group by Refno,ColorID,SuppID,Consignee,SeasonSCIID 	
 ) a
 inner join 
 (
	 select a.Refno,a.ColorID,a.SuppID, a.Consignee, a.Period, a.RibItem, b.id 
	 from #tmp_DyelotMonth a
	 left join #tmp_newSeasonSCI b on b.rid between a.rid and a.maxID   
	 group by a.Refno,a.ColorID,a.SuppID, a.Consignee, a.Period, a.RibItem, b.id  
 )b on a.Refno = b.Refno and a.ColorID = b.ColorID and a.SuppID = b.SuppID and a.Consignee = b.Consignee and a.SeasonSCIID = b.id
 group by a.{groupby_col}

 select a.{groupby_col}
	, iif(isnull(b.Dcnt,0)=0, 0, round(b.Dcnt/a.Mcnt ,4)) [cnt]
 into #BulkDyelot
 from #tmp_DyelotMcnt a
 left join #tmp_DyelotDcnt b on a.{groupby_col} = b.{groupby_col}

------#TmpFinal 
select --distinct
{(this.ReportType.Equals("supplier") ? " Tmp.SuppID , Tmp.refno " : " Tmp.refno , Tmp.SuppID ")}
    ,Tmp.abben 
	,Tmp.BrandID
    ,Tmp.stockqty 
    ,totalYds.TotalInspYds 
    ,[Total PoCnt] = isnull(TLSP.cnt,0)
    ,[Total Dyelot] =isnull(TLDyelot.cnt,0)
    {(this.ReportType.Equals("supplier") ? " ,[Total dye lots accepted(Shadeband)] = ISNULL( PassCountByDyelot.PassCTN ,0)" : string.Empty)}
    ,[Insp Report] = isnull(InspReport.cnt,0)
	,[Test Report] = isnull(TestReport.cnt,0)
	,[Continuity Card] = isnull(Contcard.cnt,0)
	,[BulkDyelot] = isnull(BulkDyelot.cnt,0)
	,[Total Point] = isnull(TLPoint.TotalPoint,0)
	,[Total Roll]= isnull(TLRoll.TotalRoll,0)
	,[GradeA Roll]= isnull(GACount.GradeA_Roll,0)
	,[GradeB Roll]= isnull(GBCount.GradeB_Roll,0)
	,[GradeC Roll]= isnull(GCCount.GradeC_Roll,0)
    ,[Inspected] = iif(Tmp.stockqty = 0, 0, round(totalYds.TotalInspYds/totalStockqty.stockqty,4)) 
    ,[yds] = isnull(TLPoint.Fabric_yards,0)   
	,[Fabric(%)] = IIF(totalYds.TotalInspYds!=0, round((TLPoint.Fabric_yards/totalYds.TotalInspYds), 4), 0)        		
    ,id = sl.ID
    ,[Point] = point.defect
    ,[SHRINKAGEyards] = isnull(SHRINKAGE.SHRINKAGEyards,0)
    ,[SHRINKAGE (%)] = isnull(SHINGKAGELevel.SHINGKAGE ,0)
    ,SHINGKAGELevel = sl2.ID
    ,[MIGRATIONyards] = isnull(MIGRATION.MIGRATIONyards,0)
    ,[MIGRATION (%)] = isnull(MIGRATIONLevel.MIGRATION,0)
    ,MIGRATIONLevel = sl3.ID
    ,[SHADINGyards] = isnull(SHADING.SHADINGyards,0)
    ,[SHADING (%)] = isnull(SHADING,0)
    ,SHADINGLevel = sl4.ID
    ,[ActualYds] = isnull(LACKINGYARDAGE.ActualYds,0)
    ,[LACKINGYARDAGE(%)] = isnull(LACKINGYARDAGELevel.LACKINGYARDAGE ,0)
    ,LACKINGYARDAGELevel = sl5.ID
    ,[SHORTWIDTH] = isnull(SHORTyards.SHORTWIDTH,0)
    ,[SHORT WIDTH (%)] = isnull(SHORTWIDTHLevel.SHORTWIDTH,0)
    ,SHORTWIDTHLevel = sl6.ID
	,Tmp.Clima 
into #TmpFinal
from (
	select distinct SuppID, refno, abben, BrandID, stockqty, isnull(TotalInspYds,0)TotalInspYds,Clima 
	, yrds	
	from #tmp		
)Tmp
outer apply(select SuppID, sum(stockqty)totalStockqty from (select distinct SuppID, refno, abben, BrandID, stockqty, isnull(TotalInspYds,0)TotalInspYds, yrds from #tmp)a where tmp.suppid =a.suppid group by SuppID)TmpTotal
outer apply (select TotalPoint,[Fabric_yards] = isnull(TotalPoint,0)/4 from #tmpTotalPoint where {groupby_col}=tmp.{groupby_col}) TLPoint
outer apply
(
	select Defect = stuff(( 
		select concat(', ',Defect) 
		from #tmp2 ta2
		where ta2.{groupby_col} = Tmp.{groupby_col} and rn <=3
		for xml path('')
	),1,1,'') 
) point
outer apply(
select {groupby_col} ,sum(stockqty) stockqty from (
	select distinct suppid,refno,brandid,abben,stockqty
	from #tmp ) a
	where {groupby_col}=tmp.{groupby_col}
	group by {groupby_col}
) totalStockqty
outer apply(
select {groupby_col} ,sum(TotalInspYds) TotalInspYds from (
	select distinct suppid,refno,brandid,abben,TotalInspYds
	from #tmp ) a
	where {groupby_col}=tmp.{groupby_col}
	group by {groupby_col}
) totalYds
outer apply(
	select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 
	and IIF(totalYds.TotalInspYds!=0, round((TLPoint.Fabric_yards/totalYds.TotalInspYds), 4), 0) * 100 between range1 and range2 )sl
left join #SHtmp SHRINKAGE on SHRINKAGE.{groupby_col} = tmp.{groupby_col}
outer apply(select SHINGKAGE = iif(TmpTotal.totalStockqty = 0 , 0, round(SHRINKAGE.SHRINKAGEyards/TmpTotal.totalStockqty,4)))SHINGKAGELevel
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(SHINGKAGELevel.SHINGKAGE,0) * 100 between range1 and range2)sl2
left join #mtmp MIGRATION on MIGRATION.{groupby_col} = tmp.{groupby_col} 
outer apply(
	select MIGRATION =  iif(TmpTotal.totalStockqty = 0, 0, round(MIGRATION.MIGRATIONyards/TmpTotal.totalStockqty,4))
)MIGRATIONLevel 
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(MIGRATIONLevel.MIGRATION,0) * 100 between range1 and range2)sl3
left join #Stmp SHADING on SHADING.{groupby_col} = tmp.{groupby_col}
outer apply(select SHADING = iif(TmpTotal.totalStockqty=0, 0, round(SHADING.SHADINGyards/TmpTotal.totalStockqty,4)))SHADINGLevel 
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(SHADINGLevel.SHADING,0) * 100 between range1 and range2)sl4
left join #Ltmp LACKINGYARDAGE on LACKINGYARDAGE.{groupby_col}= Tmp.{groupby_col}
outer apply(select LACKINGYARDAGE = iif(totalYds.TotalInspYds=0, 0, round(LACKINGYARDAGE.ActualYds/totalYds.TotalInspYds,4)))LACKINGYARDAGELevel
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(LACKINGYARDAGELevel.LACKINGYARDAGE,0) * 100 between range1 and range2)sl5
left join #Sdtmp SHORTyards on SHORTyards.{groupby_col} = Tmp.{groupby_col}
outer apply(select SHORTWIDTH = iif(totalYds.TotalInspYds=0, 0, round(SHORTyards.SHORTWIDTH/totalYds.TotalInspYds,4)))SHORTWIDTHLevel 
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 
and isnull(SHORTWIDTHLevel.SHORTWIDTH,0) * 100 between range1 and range2
)sl6
outer apply (select cnt from #tmpDyelot where {groupby_col}=Tmp.{groupby_col} ) TLDyelot
{(this.ReportType.Equals("supplier") ? "outer apply (select PassCTN from #PassCountByDyelot where SuppID=Tmp.SuppID) PassCountByDyelot" : string.Empty)}
outer apply (select TotalRoll from #tmpTotalRoll where {groupby_col}=tmp.{groupby_col}) TLRoll
outer apply (select GradeA_Roll from #tmpGrade_A where {groupby_col}=tmp.{groupby_col}) GACount
outer apply (select GradeB_Roll from #tmpGrade_B where {groupby_col}=tmp.{groupby_col}) GBCount
outer apply (select GradeC_Roll from #tmpGrade_C where {groupby_col}=tmp.{groupby_col}) GCCount
outer apply (select cnt from #tmpCountSP where {groupby_col}=tmp.{groupby_col}) TLSP
outer apply (select cnt from #tmpTestReport where {groupby_col}=tmp.{groupby_col}) TestReport
outer apply (select cnt from #InspReport where {groupby_col}=tmp.{groupby_col}) InspReport
outer apply (select cnt from #tmpContinuityCard where {groupby_col}=tmp.{groupby_col}) ContCard
outer apply (select cnt from #BulkDyelot where {groupby_col}=tmp.{groupby_col}) BulkDyelot
order by Tmp.{groupby_col}

--準備比重#table不然每筆資料都要重撈7次  
select DISTINCT
	[Fabric Defect] = (select Weight from Inspweight WITH (NOLOCK) where id ='Fabric Defect')
	,[Lacking Yardage] =  (select Weight from Inspweight WITH (NOLOCK) where id ='Lacking Yardage')
	,[Migration] = (select Weight from Inspweight WITH (NOLOCK) where id ='Migration')
	,[Shading] = (select Weight from Inspweight WITH (NOLOCK) where id ='Shading')
	,[Sharinkage] =  (select Weight from Inspweight WITH (NOLOCK) where id ='Sharinkage')
	,[Short Width] = (select Weight from Inspweight WITH (NOLOCK) where id ='Short Width') 
	,sumWeight = SUM(Weight) OVER()
into #Weight
FROM Inspweight
--取加權平均數AVG & 取AVG值落在甚麼LEVEL區間 
select a.*,[TOTALLEVEL] = s.id,
       [Keycnt] = count(1) OVER (PARTITION BY {groupby_col} )
from(
	select t.*
	,[Avg] = CASE WHEN sumWeight=0 THEN 0 ELSE isnull((([Fabric(%)] * [Fabric Defect] + [LACKINGYARDAGE(%)] * [Lacking Yardage] +[MIGRATION (%)] * [Migration] + 
			   [SHADING (%)] * [Shading] + [SHRINKAGE (%)] * [Sharinkage] + [SHORT WIDTH (%)] *  [Short Width])/sumWeight ),0) END
	from #TmpFinal t,#Weight
) a
,SuppLevel s
where s.type='F' and s.Junk=0 and [AVG] * 100 between s.range1 and s.range2 {sqlSuppWhere }
  ORDER BY {(this.ReportType.Equals("supplier") ? " SUPPID,refno " : " refno, SuppID ")}

select distinct {groupby_col} from #TmpFinal order by {groupby_col} 

drop table #tmp1,#tmp,#tmp2,#tmpAllData,#GroupBySupp,#tmpsuppdefect,#tmp2groupbyDyelot,#tmp2groupByRoll,#spr
,#SH1,#SH2,#SHtmp,#mtmp,#ea,#eb,#ec,#sa,#sb,#Stmp,#Ltmp,#Sdtmp,#TmpFinal,#Weight
,#tmpsd,#tmpDyelot,#tmpTotalPoint,#tmpTotalRoll,#tmpGrade_A,#tmpGrade_B,#tmpGrade_C,#tmpsuppEncode
,#tmpCountSP,#tmpTestReport,#InspReport,#tmpContinuityCard,#BulkDyelot
,#tmp_DyelotMain,#tmp_DyelotMcnt,#tmp_newSeasonSCI,#tmp_DyelotMonth,#tmp_DyelotDcnt
{(this.ReportType.Equals("supplier") ? ",#PassCountByDyelot ,#FirData ,#All_Fir_shadebone" : string.Empty)}
");
            #endregion
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select(string.Empty, this.cmd, this.lis, out this.allDatas);
            if (!res)
            {
                return res;
            }

            return res;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.allDatas[0].Rows.Count);
            if (this.allDatas[0] == null || this.allDatas[0].Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string xltx_name = this.ReportType.Equals("supplier") ? "Quality_R06.xltx" : "Quality_R06_by_RefNo.xltx";
            Microsoft.Office.Interop.Excel._Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\" + xltx_name);

            DataTable toExcelDt = this.allDatas[0].Copy();
            toExcelDt.Columns.Remove("KeyCnt");
            toExcelDt.Columns.Remove("Clima");
            MyUtility.Excel.CopyToXls(toExcelDt, string.Empty, xltx_name, 5, false, null, objApp);

            // objApp.Visible = true;
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            objApp.DisplayAlerts = false; // 禁止Excel跳出合併提示視窗
            int line = 6;
            int coltype = this.ReportType.Equals("supplier") ? 2 : 1;
            string key_column = this.allDatas[1].Columns[0].ColumnName;

            var combineCheckDt = this.allDatas[0].AsEnumerable();
            Microsoft.Office.Interop.Excel.Range rang;
            for (int i = 0; i < this.allDatas[0].Rows.Count; i++)
            {
                int cnt = (int)this.allDatas[0].Rows[i]["KeyCnt"];
                if (cnt > 1)
                {
                    // 合併儲存格,尾端要-1不然會重疊
                    // 只合併最後一欄，其餘用複製格式方式合併
                    rang = objSheets.Range[objSheets.Cells[this.allDatas[0].Columns.Count][line], objSheets.Cells[this.allDatas[0].Columns.Count][line + cnt - 1]];
                    rang.Merge();
                    rang.Copy(Type.Missing);

                    for (int ii = 1; ii <= this.allDatas[0].Columns.Count; ii++)
                    {
                        Microsoft.Office.Interop.Excel.Range rang2;

                        // Columns=2,4,5 是不需要合併的
                        if (ii == 2 || ii == 4 || ii == 5 || (ii == 3 && this.ReportType.Equals("Refno")))
                        {
                            continue;
                        }

                        // 複製格式
                        // objSheets.Range[objSheets.Cells[ii][line], objSheets.Cells[ii][line + cnt - 1]].Style = rang.Style;
                        rang2 = objSheets.Range[objSheets.Cells[ii][line], objSheets.Cells[ii][line + cnt - 1]];
                        rang2.PasteSpecial(
                            Microsoft.Office.Interop.Excel.XlPasteType.xlPasteFormats,
                            Microsoft.Office.Interop.Excel.XlPasteSpecialOperation.xlPasteSpecialOperationNone, false, false);
                    }
                }

                bool Clima = (bool)this.allDatas[0].Rows[i]["Clima"];
                if (Clima)
                {
                    objSheets.get_Range((Microsoft.Office.Interop.Excel.Range)objSheets.Cells[line, coltype], (Microsoft.Office.Interop.Excel.Range)objSheets.Cells[line, coltype]).Interior.Color = Color.Yellow;
                }

                line = line + cnt;
                i = i + cnt - 1;
            }

            // 由於複製格式的關係，會把原本的型態也取代。重新再壓回型態。
            if (this.ReportType.Equals("supplier"))
            {
                for (int ii = 1; ii <= this.allDatas[0].Columns.Count; ii++)
                {
                    int col = (ii >= 20 && ii <= 38 && !(ii == 23)) ? 5 : 3;
                    objSheets.Columns[ii].NumberFormat = objSheets.Cells[ii][col].NumberFormat;
                }
            }
            else
            {
                for (int ii = 1; ii <= this.allDatas[0].Columns.Count; ii++)
                {
                    int col = (ii >= 19 && ii <= 37 && !(ii == 22)) ? 5 : 3;
                    objSheets.Columns[ii].NumberFormat = objSheets.Cells[ii][col].NumberFormat;
                }
            }

            objSheets.Cells[2, 1] = $"Date: {this.dateArriveWHDate.DateBox1.Text} ~ {this.dateArriveWHDate.DateBox2.Text}";
            objSheets.Range["B3"].Activate();

            #region 調整欄寬
            if (this.ReportType.Equals("supplier"))
            {
                objSheets.Columns[5].ColumnWidth = 7.38;
                objSheets.Columns[6].ColumnWidth = 7.38;
                objSheets.Columns[7].ColumnWidth = 7.38;

                objSheets.Columns[8].ColumnWidth = 7.63;
                objSheets.Columns[9].ColumnWidth = 11;
                objSheets.Columns[10].ColumnWidth = 9.13;
                objSheets.Columns[11].ColumnWidth = 11;
                objSheets.Columns[12].ColumnWidth = 13.13;
                objSheets.Columns[13].ColumnWidth = 13.13;

                objSheets.Columns[14].ColumnWidth = 11.88;
                objSheets.Columns[15].ColumnWidth = 6.63;
                objSheets.Columns[16].ColumnWidth = 6.63;
                objSheets.Columns[17].ColumnWidth = 6.63;
                objSheets.Columns[18].ColumnWidth = 6.63;

                objSheets.Columns[20].ColumnWidth = 8;
                objSheets.Columns[21].ColumnWidth = 8;
                objSheets.Columns[23].ColumnWidth = 15.63;
                objSheets.Columns[38].ColumnWidth = 11.88;
            }
            else
            {
                objSheets.Columns[5].ColumnWidth = 7.38;
                objSheets.Columns[6].ColumnWidth = 7.38;
                objSheets.Columns[7].ColumnWidth = 7.38;

                objSheets.Columns[8].ColumnWidth = 7.63;
                objSheets.Columns[9].ColumnWidth = 9.13;
                objSheets.Columns[10].ColumnWidth = 11;
                objSheets.Columns[11].ColumnWidth = 13.13;
                objSheets.Columns[12].ColumnWidth = 13.13;

                objSheets.Columns[13].ColumnWidth = 11.88;
                objSheets.Columns[14].ColumnWidth = 6.63;
                objSheets.Columns[15].ColumnWidth = 6.63;
                objSheets.Columns[16].ColumnWidth = 6.63;
                objSheets.Columns[17].ColumnWidth = 6.63;

                objSheets.Columns[19].ColumnWidth = 8;
                objSheets.Columns[20].ColumnWidth = 8;
                objSheets.Columns[22].ColumnWidth = 15.63;
                objSheets.Columns[37].ColumnWidth = 11.88;
            }
            #endregion
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R06");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
