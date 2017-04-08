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
        DataTable dt; string cmd; string Supp,refno,brand,season;
        public R06(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            print.Enabled = false;
        }
        protected override bool ValidateInput()
        {
           
            lis = new List<SqlParameter>();
            bool DateArr_empty = !DateArriveWH.HasValue, Supplier_empty = !this.txtsupplier.Text.Empty(), refno_empty = !this.txtRefNo.Text.Empty(),brand_empty = !this.txtbrand.Text.Empty(),
                 season_empty = !txtseason.Text.Empty();
            if (DateArr_empty)
            {
                MyUtility.Msg.ErrorBox("Please select 'Received Sample Date' or 'Arrive W/H Date' at least one field entry");

                DateArriveWH.Focus();
                return false;
            }
            DateArrStart = DateArriveWH.Value1;
            DateArrEnd = DateArriveWH.Value2;
            brand = txtbrand.Text;
            refno = txtRefNo.Text.ToString();
            season = txtseason.Text;
            Supp = txtsupplier.TextBox1.Text;
            lis = new List<SqlParameter>();
            string sqlWhere = "";
            string sqlRWhere = "";
            List<string> sqlWheres = new List<string>();
            List<string> RWheres = new List<string>();
            #region --組WHERE--

           if (!this.DateArriveWH.Value1.Empty())
           {
               RWheres.Add("r.WhseArrival >= @DateArrStart");
               lis.Add(new SqlParameter("@DateArrStart", DateArrStart));
           }
           if (!this.DateArriveWH.Value2.Empty())
           {
               RWheres.Add("r.WhseArrival <= @DateArrEnd");
               lis.Add(new SqlParameter("@DateArrEnd", DateArrEnd));
           }
            
            if (!this.Supp.Empty())
           {
               sqlWheres.Add("ps.suppid = @Supp");
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
	select distinct rd.PoId,rd.Seq1,rd.Seq2
	from Receiving r
	inner join Receiving_Detail rd on r.Id = rd.Id
	where 1=1
    " + sqlRWhere + @"
	and r.Status = 'Confirmed'
	union all
	select distinct sd.ToPOID as PoId,sd.ToSeq1 as Seq1,sd.ToSeq2 as Seq2
	from SubTransfer s
	inner join SubTransfer_Detail sd on s.Id = sd.ID
	where 1=1
    " + sqlRWhere.Replace("r.WhseArrival", "s.IssueDate") + @"
	and s.Status = 'Confirmed'
	and s.Type = 'B'
	union all
	select distinct bd.ToPOID as PoId,bd.ToSeq1 as Seq1,bd.ToSeq2 as Seq2
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



	-------------Fabric Defect -----
select rd.PoId,rd.Seq1,rd.Seq2,rd.ActualQty,rd.Dyelot,rd.Roll,t.SuppID,t.Refno 
into #tmpAllData
from #tmp1 t
inner join Receiving_Detail rd on t.PoId = rd.PoId and t.Seq1 = rd.Seq1 and t.Seq2 = rd.Seq2

------------Group by Supp
select PoId,Seq1,Seq2,SuppID,sum(ActualQty) as ActualQty 
into #GroupBySupp
from #tmpAllData
group by PoId,Seq1,Seq2,SuppID
order by PoId,Seq1,Seq2,SuppID

-----------Kinds of Fabric Defects (Defect Name)----
select t.SuppID,fpd.DefectRecord,t.PoId,t.Seq1,t.Seq2 
into #tmpsuppdefect
from #GroupBySupp t
inner join FIR f on t.PoId = f.POID and t.Seq1 = f.SEQ1 and t.Seq2 = f.SEQ2
inner join FIR_Physical fp on fp.ID = f.ID
inner join FIR_Physical_Defect fpd on fpd.FIR_PhysicalDetailUKey = fp.DetailUkey
where f.PhysicalEncode = 1

--------Group by Dyelot-------------
select PoId,Seq1,Seq2,SuppID,Refno,Dyelot,sum(ActualQty) as ActualQty 
into #tmp2groupbyDyelot
from #tmpAllData
group by PoId,Seq1,Seq2,SuppID,Refno,Dyelot
order by PoId,Seq1,Seq2,SuppID,Refno,Dyelot


-------Group by Roll---------------
select PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll,sum(ActualQty) as ActualQty 
into #tmp2groupByRoll
from #tmpAllData
group by PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll
order by PoId,Seq1,Seq2,SuppID,Refno,Dyelot,Roll

select distinct gbs.SuppID ,
ref.*,
s.AbbEN,
TtlYds.TotalInspYds,
TtlQty.stockqty,
Yard.yrds,
Point.Defect,
Point.ID,
Point.Point
into #tmp
from 
	 (select distinct SuppID from #GroupBySupp) gbs
inner join Supp s WITH (NOLOCK) on s.id=gbs.SuppID
outer apply(
select (
	Select t.Refno +',' 
	from (
            SELECT DISTINCT Refno
			FROM #tmpAllData 
			WHERE  #tmpAllData.SuppID=gbs.SuppID						
	)t 
	order by t.Refno
	for xml path('')				
	) as Refno
) as ref
outer apply(
	select Defect =defect.DescriptionEN ,defect.ID,sum(point) as Point 
	from (
			select substring(data,1,1) as Defect,CONVERT(int, substring(data,2,5)) as Point 
			from dbo.SplitString(
				(
					select concat('/',DefectRecord) 
					from #tmpsuppdefect					
					WHERE Suppid=gbs.SuppID
					for xml path('')
				),
			'/') 
		)A 
	OUTER APPLY(select DescriptionEN,ID from FabricDefect where id=defect) AS defect
	 group by defect.DescriptionEN,defect.ID
) as Point
outer apply(
	select sum(TotalInspYds) as TotalInspYds
    from FIR a,#GroupBySupp b
    where a.POID = b.PoId 
    and a.SEQ1 = b.Seq1 
    and a.Seq2 = b.Seq2
	and a.Suppid=gbs.SuppID
)as TtlYds
outer apply(
	select sum(ActualQty) as stockqty
	from #tmpAllData
	where SuppID=gbs.SuppID
) as TtlQty
outer apply (
select [Yrds]=count(*)*5 
from #tmpsuppdefect a,#GroupBySupp b
where a.POID=b.PoId and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
and a.SuppID=gbs.SuppID
)  as Yard
order by Refno

select Suppid,defect,ID,
point  = sum(point) over(partition by suppid,defect)
,ROW_NUMBER() over(partition by suppid order by suppid,point desc ,ID desc) RN 
into #tmp2
from #tmp
order by Suppid,point desc,ID desc

    select distinct Tmp.SuppID 
	,Tmp.refno 
    ,Tmp.abben 
    ,Tmp.stockqty 
    ,Tmp.TotalInspYds 
    ,[Inspected]= iif(Tmp.stockqty<>0,round(Tmp.TotalInspYds/Tmp.stockqty*100,2),0) 
    ,Tmp.yrds 
    ,Tmp.[Fabric(%)]
    ,(select sl.id from SuppLevel sl WITH (NOLOCK) where type='F' and range1 <= isnull(tmp.[Fabric(%)],0) and range2 >= isnull(tmp.[Fabric(%)],0)) id		
    ,[Point]=point.defect
    ,[SHRINKAGEyards]= isnull(SHRINKAGE.SHRINKAGEyards,0)
    ,[SHRINKAGE (%)] = isnull(SHINGKAGE ,0)
    ,(select id from SuppLevel WITH (NOLOCK) where type='F' and range1 <= isnull(SHINGKAGE,0) and range2 >= isnull(SHINGKAGE,0))SHINGKAGELevel
    ,[MIGRATIONyards]= isnull(MIGRATION.MIGRATIONyards,0)
    ,[MIGRATION (%)]= isnull(MIGRATION,0)
    ,(select id from SuppLevel WITH (NOLOCK) where type='F' and range1 <= isnull(MIGRATION,0) and range2 >= isnull(MIGRATION,0))MIGRATIONLevel
    ,[SHADINGyards]= isnull(SHADING.SHADINGyards,0)
    ,[SHADING (%)]= isnull(SHADING,0)
    ,(select id from SuppLevel WITH (NOLOCK) where type='F' and range1 <= isnull(SHADING,0) and range2 >= isnull(SHADING,0))SHADINGLevel
    ,[ActualYds]= isnull(LACKINGYARDAGE.ActualYds,0)
    ,[LACKINGYARDAGE(%)]= isnull(LACKINGYARDAGELevel.LACKINGYARDAGE ,0)
    ,(select id from SuppLevel WITH (NOLOCK) where type='F' and range1 <= isnull(LACKINGYARDAGELevel.LACKINGYARDAGE,0) and range2 >= isnull(LACKINGYARDAGELevel.LACKINGYARDAGE,0))LACKINGYARDAGELevel
    ,[SHORTWIDTH]= isnull(SHORTyards.SHORTWIDTH,0)
    ,[SHORT WIDTH (%)]= isnull(SHORTWIDTHLevel.SHORTWIDTH,0)
    ,(select id from SuppLevel WITH (NOLOCK) where type='F' and range1 <= isnull(SHORTWIDTHLevel.SHORTWIDTH,0) and range2 >= isnull(SHORTWIDTHLevel.SHORTWIDTH,0))SHORTWIDTHLevel            
	into #TmpFinal
from (
	select 
		SuppID
       	,refno
       	,abben
       	,stockqty
       	,TotalInspYds
       	,yrds			
       	,[Fabric(%)]= IIF(TotalInspYds!=0, round((yrds/TotalInspYds)*100,2),0)        		
		from  #tmp tmp	
	)Tmp 	
outer apply
		(								
				select distinct SuppID,
								defect=stuff(B.Defect,1,1,'') 
				from #tmp2 as ta1
				outer apply
				(
					select Defect=( select concat(', ',convert(varchar(50),Defect)) 
					from #tmp2 ta2
					where ta1.SuppID=ta2.SuppID 
					and rn <=3
					for xml path(''))	
				) B 
				WHERE ta1.SuppID=Tmp.SuppID
		) as point
outer apply(
		select Sum(rd1.stockqty)SHRINKAGEyards 
       	from  #tmp2groupbyDyelot rd WITH (NOLOCK) 
		inner join Receiving_Detail rd1 on rd.PoId=rd1.PoId and rd.Seq1=rd1.Seq1 and rd.Seq2=rd1.Seq2
			Where exists(select FL.* 
       					from FIR_Laboratory FL WITH (NOLOCK) 
       					inner join dbo.FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = FL.ID 						
       					where FL.HeatEncode=1 and h.Result = 'Fail' and FL.POID = rd.PoId and FL.SEQ1 = RD.seq1 
       					and FL.seq2 = RD.seq2 and h.Dyelot = RD.dyelot 
						)
       		or exists(select * 
       					from dbo.FIR_Laboratory FL WITH (NOLOCK) 
       					inner join dbo.FIR_Laboratory_Wash W WITH (NOLOCK) on W.ID = FL.ID						
       					where FL.WashEncode=1 and W.Result = 'Fail' and FL.POID = RD.poid and FL.SEQ1 = RD.seq1 
       					and FL.seq2 = RD.seq2 and W.Dyelot = RD.dyelot 
						)
						group by rd1.PoId,rd1.Seq1,rd1.Seq2,rd1.Dyelot 
       		)SHRINKAGE
outer apply(
				select iif(Tmp.stockqty!=0,round(SHRINKAGE.SHRINKAGEyards/Tmp.stockqty*100,2),0)SHINGKAGE
			)SHINGKAGELevel 
outer apply(
	SELECT [MIGRATIONyards]= Sum(stockqty) from (
       	select distinct rd.PoId,rd.Seq1,rd.Seq2,rd.Roll,rd.Dyelot,rd.StockQty
		from Receiving_Detail rd WITH (NOLOCK) 
		inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2
       	Where exists(select * from dbo.FIR_Laboratory l WITH (NOLOCK) 
						inner join dbo.FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = l.ID
						INNER JOIN FIR F WITH (NOLOCK) ON L.POID=F.POID AND F.Suppid=Tmp.SuppID 
						where l.CrockingEncode=1 and h.Result = 'Fail' and l.POID = RD.poid and l.SEQ1 = RD.seq1 
						and l.seq2 = RD.seq2) 
				or exists(select * from Oven O WITH (NOLOCK) 
						inner join Oven_Detail OD WITH (NOLOCK) on OD.ID = O.ID
						INNER JOIN FIR F WITH (NOLOCK) ON O.POID=F.POID AND F.Suppid=Tmp.SuppID 
						where O.Status ='Confirmed' and OD.Result = 'Fail' 
						and O.poid = RD.poid and OD.seq1 = RD.seq1 and OD.seq2 = RD.seq2 )
				or exists(select * from ColorFastness CF WITH (NOLOCK) 
						inner join ColorFastness_Detail CFD WITH (NOLOCK) on CFD.ID = CF.ID
						INNER JOIN FIR F WITH (NOLOCK) ON CF.POID=F.POID AND F.Suppid=Tmp.SuppID 
						where CF.Status ='Confirmed' and CFD.Result = 'Fail' 
						and CF.poid = RD.poid and CFD.seq1 = RD.seq1 and CFD.seq2 = RD.seq2)
						) a
       		)MIGRATION 
outer apply(select iif(Tmp.stockqty!=0,round(MIGRATION.MIGRATIONyards/Tmp.stockqty*100,2),0)MIGRATION)MIGRATIONLevel 
outer apply(
	 select [SHADINGyards]= sum(StockQty) from (
	select distinct rd.PoId,rd.Seq1,rd.Seq2,rd.Roll,rd.Dyelot,rd.StockQty
	from Receiving_Detail rd WITH (NOLOCK) 
	inner join #GroupBySupp r on rd.PoId=r.POID AND rd.Seq1=r.SEQ1 and rd.Seq2=r.SEQ2
	Where exists(
	select * from fir f WITH (NOLOCK) 
		inner join FIR_Shadebone fs WITH (NOLOCK) on fs.ID = f.ID
		where f.ShadebondEncode =1 and fs.Result = 'Fail' 
		and f.poid =rd.poid and f.SEQ1 = rd.seq1 and f.seq2 = rd.seq2 and fs.Dyelot  = rd.dyelot 
		and f.suppid=Tmp.Suppid
		) 
	or exists(
	select * from fir f WITH (NOLOCK) 
		inner join FIR_Continuity fc WITH (NOLOCK) on fc.ID = f.ID
			where f.ContinuityEncode =1 and fc.Result = 'Fail' 
			and f.poid = rd.poid and f.seq1 = rd.seq1 and f.seq2 = rd.seq2 and fc.Dyelot  = rd.dyelot 
			and f.suppid=Tmp.Suppid
		)	
	)a						 
)SHADING
outer apply(select iif(Tmp.stockqty!=0,round(SHADING.SHADINGyards/Tmp.stockqty*100,2),0)SHADING )SHADINGLevel 
outer apply(
select sum(fp.TicketYds - fp.ActualYds) as ActualYds
    from FIR f
    inner join FIR_Physical fp on f.ID = fp.ID
	inner join #GroupBySupp t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2
    where f.PhysicalEncode = 1
    and fp.ActualYds < fp.TicketYds
    and t.SuppID=Tmp.SuppID
		)LACKINGYARDAGE 
outer apply(select iif(Tmp.stockqty!=0,round(LACKINGYARDAGE.ActualYds/Tmp.stockqty*100,2),0)LACKINGYARDAGE )LACKINGYARDAGELevel 
outer apply(
	select sum(t.ActualQty)SHORTWIDTH
		from  fir f WITH (NOLOCK) 
       	inner join FIR_Physical fp on f.ID = fp.ID
		left join Fabric on Fabric.SCIRefno = f.SCIRefno
		inner join #tmp2groupByRoll t on f.POID = t.PoId and f.SEQ1 = t.Seq1 and f.SEQ2 = t.Seq2 and fp.Dyelot = t.Dyelot and fp.Roll = t.Roll
       	where f.PhysicalEncode = 1 
		and f.Suppid=Tmp.SuppID
		and fp.ActualWidth <> Fabric.Width
)SHORTyards 
outer apply(select iif(Tmp.stockqty!=0,round(SHORTyards.SHORTWIDTH/Tmp.TotalInspYds*100,2),0)SHORTWIDTH )SHORTWIDTHLevel 
order by Tmp.SuppID


-----加總匯出Report
select *, 
	[TOTALLEVEL]=(select id from SuppLevel WITH (NOLOCK) where type='F' and range1 <= isnull([AVG],0) and range2 >= isnull([AVG],0))
from(
	select * ,[Avg] = (select 
       	(	[Fabric(%)]* (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Fabric Defect') +
       		[SHRINKAGE (%)] *  (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Sharinkage') +
       		[MIGRATION (%)] * (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Migration') +
       		[SHADING (%)] * (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Shading') +
       		[LACKINGYARDAGE(%)] *  (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Lacking Yardage') +
       		[SHORT WIDTH (%)] * (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Short Width') 
		)   /(select sum(Weight) from dbo.Inspweight WITH (NOLOCK) )  ) 
		from #TmpFinal) a
ORDER BY SUPPID
       
drop table #tmp1,#tmp,#tmp2,#tmpAllData,#GroupBySupp,#tmpsuppdefect,#tmp2groupbyDyelot,#tmp2groupByRoll,#TmpFinal

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
            
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
           
        }
    }
}
