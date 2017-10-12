using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R06 : Sci.Win.Tems.PrintForm
    {
        DateTime? DateArrStart; DateTime? DateArrEnd;
        List<SqlParameter> lis; 
        DataTable dt; 
        string cmd; 
        string Supp,refno,brand,season;

        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            print.Enabled = false;
        }

        protected override bool ValidateInput()
        {
            lis = new List<SqlParameter>();
            bool DateArr_empty = !dateArriveWHDate.HasValue, Supplier_empty = !this.txtsupplier.Text.Empty(), refno_empty = !this.txtRef.Text.Empty(),brand_empty = !this.txtbrand.Text.Empty(),
                 season_empty = !txtseason.Text.Empty();
            if (DateArr_empty)
            {
                dateArriveWHDate.Focus();
                MyUtility.Msg.ErrorBox("Please select 'Received Sample Date' or 'Arrive W/H Date' at least one field entry");
                return false;
            }
            DateArrStart = dateArriveWHDate.Value1;
            DateArrEnd = dateArriveWHDate.Value2;
            brand = txtbrand.Text;
            refno = txtRef.Text.ToString();
            season = txtseason.Text;
            Supp = txtsupplier.TextBox1.Text;
            lis = new List<SqlParameter>();
            string sqlWhere = "";
            string sqlRWhere = "";
            string sqlSuppWhere = "";
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            #region --組WHERE--

           if (!this.dateArriveWHDate.Value1.Empty())
           {
               RWheres.Add("r.WhseArrival >= @DateArrStart");
               lis.Add(new SqlParameter("@DateArrStart", DateArrStart));
           }
           if (!this.dateArriveWHDate.Value2.Empty())
           {
               RWheres.Add("r.WhseArrival <= @DateArrEnd");
               lis.Add(new SqlParameter("@DateArrEnd", DateArrEnd));
           }
            
            if (!this.Supp.Empty())
           {
                sqlSuppWhere= " and a.SuppID = @Supp ";
               lis.Add(new SqlParameter("@Supp", Supp));
              
           } if (!this.refno.Empty())
           {
               sqlWheres.Add("psd.Refno = @refno");
               lis.Add(new SqlParameter("@refno", refno));

           } if (!this.brand.Empty())
           {
               sqlWheres.Add("o.brandid = @brand");
               lis.Add(new SqlParameter("@brand", brand));
              
           } if (!this.season.Empty())
           {
               sqlWheres.Add("o.Seasonid = @season");
               lis.Add(new SqlParameter("@season", season)); 
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

           cmd = string.Format(@"

select distinct a.PoId,a.Seq1,a.Seq2,ps.SuppID,psd.Refno 
into #tmp1
from
(
	select rd.PoId,rd.Seq1,rd.Seq2
	from Receiving r
	inner join Receiving_Detail rd on r.Id = rd.Id
	where 1=1
    " + sqlRWhere + @"
	and r.Status = 'Confirmed'
	union
	select sd.ToPOID as PoId,sd.ToSeq1 as Seq1,sd.ToSeq2 as Seq2
	from SubTransfer s
	inner join SubTransfer_Detail sd on s.Id = sd.ID
	where 1=1
    " + sqlRWhere.Replace("r.WhseArrival", "s.IssueDate") + @"
	and s.Status = 'Confirmed'
	and s.Type = 'B'
	union
	select bd.ToPOID as PoId,bd.ToSeq1 as Seq1,bd.ToSeq2 as Seq2
	from BorrowBack b
	inner join BorrowBack_Detail bd on b.Id = bd.ID
	where 1=1
    " + sqlRWhere.Replace("r.WhseArrival", "b.IssueDate") + @"
	and b.Status = 'Confirmed'
	and (b.Type = 'A' or b.Type = 'B')
) a
left join Orders o on o.ID = a.PoId
left join PO_Supp ps on ps.ID = a.PoId and ps.SEQ1 = a.Seq1
left join PO_Supp_Detail psd on psd.ID = a.PoId and psd.SEQ1 = a.Seq1 and psd.SEQ2 = a.Seq2
where psd.FabricType = 'F'
" + sqlWhere + @"
------------Fabric Defect ----- 
select rd.PoId,rd.Seq1,rd.Seq2,rd.ActualQty,rd.Dyelot,rd.Roll,t.SuppID,t.Refno 
into #tmpAllData
from #tmp1 t
inner join Receiving_Detail rd on t.PoId = rd.PoId and t.Seq1 = rd.Seq1 and t.Seq2 = rd.Seq2
------------Group by Supp 
select PoId,Seq1,Seq2,SuppID,ActualQty = sum(ActualQty)  
into #GroupBySupp
from #tmpAllData
group by PoId,Seq1,Seq2,SuppID
------------Kinds of Fabric Defects (Defect Name)---- 
select t.SuppID,fpd.DefectRecord,t.PoId,t.Seq1,t.Seq2 
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
select distinct a.SuppID,
Defect = STUFF((
			select concat('/',s.DefectRecord) 
			from (
				select distinct DefectRecord
				from #tmpsuppdefect b
				WHERE b.Suppid = a.SuppID
			)s
			for xml path('')
		 ), 1, 1, '')
into #spr
from #tmpsuppdefect a
-------tmp 
select distinct gbs.SuppID
	,ref.Refno
	,s.AbbEN
	,TotalInspYds = (select sum(TotalInspYds) from FIR a inner join #GroupBySupp b on a.POID = b.PoId and a.SEQ1 = b.Seq1 and a.Seq2 = b.Seq2 where a.Suppid = gbs.SuppID)
	,stockqty = (select sum(ActualQty) from #tmpAllData where SuppID = gbs.SuppID) 
	,yrds = (
		select [Yrds] = count(*) * 5 
		from #tmpsuppdefect a inner join #GroupBySupp b on a.POID=b.PoId and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
		where b.SuppID = gbs.SuppID
	)
	,Point.Defect
	,Point.ID
	,Point.Point	
into #tmp
from (select SuppID from #GroupBySupp group by SuppID) as gbs
inner join Supp s WITH (NOLOCK) on s.id = gbs.SuppID
outer apply(
	select Refno = stuff((
		Select concat(',',Refno) 
		from (
			SELECT distinct Refno
			FROM #tmpAllData 
			WHERE #tmpAllData.SuppID = gbs.SuppID
		)t 
		for xml path('')				
	),1,1,'')
) as ref
outer apply(
	select Defect = fd.DescriptionEN, fd.ID, Point = sum(a.point)  
	from (
		select substring(data,1,1) as Defect,CONVERT(int, substring(data,2,5)) as Point 
		from dbo.SplitString((select Defect from #spr WHERE Suppid = gbs.SuppID),'/') 
	)A 
	left join FabricDefect fd on id = a.Defect
	group by fd.DescriptionEN,fd.ID
) as Point
order by Refno
-------tmp2 
select Suppid,defect,ID,
point  = sum(point) over(partition by suppid,defect)
,ROW_NUMBER() over(partition by suppid order by suppid,point desc ,ID desc) RN 
into #tmp2
from #tmp
order by Suppid,point desc,ID desc
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
select SHRINKAGEyards = Sum(rd1.stockqty), SuppID
into #SHtmp
from  #tmp2groupbyDyelot rd WITH (NOLOCK) inner join Receiving_Detail rd1 on rd.PoId = rd1.PoId and rd.Seq1 = rd1.Seq1 and rd.Seq2 = rd1.Seq2
Where exists(select * from #SH1 where POID = rd.PoId and SEQ1 = RD.seq1 and seq2 = seq2 and Dyelot = RD.dyelot)
or exists(select * from #SH2 where POID = RD.poid and SEQ1 = RD.seq1 and seq2 = RD.seq2 and Dyelot = RD.dyelot)
group by SuppID
-----#mtmp 
select l.POID,l.SEQ1,l.seq2, F.Suppid
INTO #ea
from dbo.FIR_Laboratory l WITH (NOLOCK) 
inner join dbo.FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = l.ID
INNER JOIN FIR F WITH (NOLOCK) ON L.POID=F.POID
where l.CrockingEncode=1 and h.Result = 'Fail' 
select O.poid,OD.seq1,OD.seq2,F.Suppid
INTO #eb
from Oven O WITH (NOLOCK) 
inner join Oven_Detail OD WITH (NOLOCK) on OD.ID = O.ID
INNER JOIN FIR F WITH (NOLOCK) ON O.POID=F.POID 
where O.Status ='Confirmed' and OD.Result = 'Fail'
select CF.poid,CFD.seq1,CFD.seq2,F.Suppid
into #ec
from ColorFastness CF WITH (NOLOCK) 
inner join ColorFastness_Detail CFD WITH (NOLOCK) on CFD.ID = CF.ID
INNER JOIN FIR F WITH (NOLOCK) ON CF.POID=F.POID 
where CF.Status ='Confirmed' and CFD.Result = 'Fail' 
select [MIGRATIONyards] = sum(rd.StockQty),tmp.Suppid
into #mtmp
from Receiving_Detail rd WITH (NOLOCK) 
inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2
,#tmp tmp	
Where exists(select * from #ea where POID = RD.poid and SEQ1 = RD.seq1 and seq2 = RD.seq2  AND Suppid = Tmp.SuppID ) 
or exists(select * from #eb where POID = RD.poid and SEQ1 = RD.seq1 and seq2 = RD.seq2  AND Suppid = Tmp.SuppID ) 
or exists(select * from #ec where POID = RD.poid and SEQ1 = RD.seq1 and seq2 = RD.seq2  AND Suppid = Tmp.SuppID ) 
group by tmp.Suppid
-----#Stmp 
select f.poid,f.SEQ1,f.seq2,fs.Dyelot,f.suppid
into #sa
from fir f WITH (NOLOCK)
inner join FIR_Shadebone fs WITH (NOLOCK) on fs.ID = f.ID
where f.ShadebondEncode =1 and fs.Result = 'Fail' 
select f.poid,f.SEQ1,f.seq2,fc.Dyelot,f.suppid 
into #sb
from fir f WITH (NOLOCK) 
inner join FIR_Continuity fc WITH (NOLOCK) on fc.ID = f.ID
where f.ContinuityEncode =1 and fc.Result = 'Fail' 
select [SHADINGyards]= sum(rd.StockQty),tmp.Suppid
into #Stmp
from Receiving_Detail rd WITH (NOLOCK) 
inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2
,#tmp tmp
Where exists(select * from #sa where poid = rd.poid and SEQ1 = rd.seq1 and seq2 = rd.seq2 and Dyelot  = rd.dyelot and suppid = Tmp.Suppid) 
or exists(select * from #sb where poid = rd.poid and SEQ1 = rd.seq1 and seq2 = rd.seq2 and Dyelot  = rd.dyelot and suppid = Tmp.Suppid) 
group by tmp.Suppid
-----#Ltmp 
select ActualYds = sum(fp.TicketYds - fp.ActualYds), t.SuppID
into #Ltmp
from FIR f
inner join FIR_Physical fp on f.ID = fp.ID
inner join #GroupBySupp t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2
where f.PhysicalEncode = 1 and fp.ActualYds < fp.TicketYds
group by t.Suppid
-----#Sdtmp 
select SHORTWIDTH = sum(t.ActualQty), f.Suppid
into #Sdtmp
from  fir f WITH (NOLOCK) 
inner join FIR_Physical fp on f.ID = fp.ID
inner join #tmp2groupByRoll t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2 and fp.Dyelot = t.Dyelot and fp.Roll = t.Roll
left join Fabric on Fabric.SCIRefno = f.SCIRefno
where f.PhysicalEncode = 1 and fp.ActualWidth <> Fabric.Width
group by f.Suppid
------#TmpFinal 
select distinct Tmp.SuppID 
	,Tmp.refno 
    ,Tmp.abben 
    ,Tmp.stockqty 
    ,Tmp.TotalInspYds 
    ,[Inspected] = iif(Tmp.stockqty = 0, 0, round(Tmp.TotalInspYds/Tmp.stockqty*100,2)) 
    ,Tmp.yrds 
    ,Tmp.[Fabric(%)]
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
into #TmpFinal
from (
	select SuppID, refno, abben, stockqty, TotalInspYds, yrds			
	,[Fabric(%)] = IIF(TotalInspYds!=0, round((yrds/TotalInspYds)*100, 2), 0)        		
	from #tmp	
)Tmp 	
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(tmp.[Fabric(%)],0) between range1 and range2 )sl
outer apply
(
	select Defect = stuff(( 
		select concat(', ',Defect) 
		from #tmp2 ta2
		where ta2.SuppID = Tmp.SuppID and rn <=3
		for xml path('')
	),1,1,'') 
) point
left join #SHtmp SHRINKAGE on SHRINKAGE.SuppID = tmp.SuppID
outer apply(select SHINGKAGE = iif(Tmp.stockqty = 0 , 0, round(SHRINKAGE.SHRINKAGEyards/Tmp.stockqty*100,2)))SHINGKAGELevel
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(SHINGKAGELevel.SHINGKAGE,0) between range1 and range2)sl2
left join #mtmp MIGRATION on MIGRATION.SuppID = tmp.SuppID
outer apply(select MIGRATION =  iif(Tmp.stockqty = 0, 0, round(MIGRATION.MIGRATIONyards/Tmp.stockqty*100,2)))MIGRATIONLevel 
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(MIGRATIONLevel.MIGRATION,0) between range1 and range2)sl3
left join #Stmp SHADING on SHADING.SuppID = tmp.SuppID
outer apply(select SHADING = iif(Tmp.stockqty=0, 0, round(SHADING.SHADINGyards/Tmp.stockqty*100,2)))SHADINGLevel 
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(SHADINGLevel.SHADING,0) between range1 and range2)sl4
left join #Ltmp LACKINGYARDAGE on LACKINGYARDAGE.SuppID= Tmp.SuppID
outer apply(select LACKINGYARDAGE = iif(Tmp.stockqty=0, 0, round(LACKINGYARDAGE.ActualYds/Tmp.stockqty*100,2)))LACKINGYARDAGELevel
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(LACKINGYARDAGELevel.LACKINGYARDAGE,0) between range1 and range2)sl5
left join #Sdtmp SHORTyards on SHORTyards.Suppid = Tmp.SuppID
outer apply(select SHORTWIDTH = iif(Tmp.stockqty=0, 0, round(SHORTyards.SHORTWIDTH/Tmp.TotalInspYds*100,2)))SHORTWIDTHLevel 
outer apply(select id from SuppLevel WITH (NOLOCK) where type='F' and Junk=0 and isnull(SHORTWIDTHLevel.SHORTWIDTH,0) between range1 and range2)sl6
order by Tmp.SuppID
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
select a.*,[TOTALLEVEL] = s.id
from(
	select t.*
	,[Avg] = isnull((([Fabric(%)] * [Fabric Defect] + [LACKINGYARDAGE(%)] * [Lacking Yardage] +[MIGRATION (%)] * [Migration] + 
			   [SHADING (%)] * [Shading] + [SHRINKAGE (%)] * [Sharinkage] + [SHORT WIDTH (%)] *  [Short Width])/sumWeight ),0)
	from #TmpFinal t,#Weight
) a
,SuppLevel s
where s.type='F' and s.Junk=0 and [AVG] between s.range1 and s.range2 "+ sqlSuppWhere +
@"  ORDER BY SUPPID

drop table #tmp1,#tmp,#tmp2,#tmpAllData,#GroupBySupp,#tmpsuppdefect,#tmp2groupbyDyelot,#tmp2groupByRoll,#spr
,#SH1,#SH2,#SHtmp,#mtmp,#ea,#eb,#ec,#sa,#sb,#Stmp,#Ltmp,#Sdtmp,#TmpFinal,#Weight
");
           #endregion
            return base.ValidateInput();
        }

        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DualResult res;
            res = DBProxy.Current.Select("", cmd, lis, out dt);
            if (!res)
            {
                return res;
            }
            return res;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(dt.Rows.Count);
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            Microsoft.Office.Interop.Excel._Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R06.xltx");
            MyUtility.Excel.CopyToXls(dt, "", "Quality_R06.xltx", 6, true, null, objApp);           
            return true;           
        }
    }
}
