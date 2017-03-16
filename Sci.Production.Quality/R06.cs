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
            Supp = txtsupplier.Text;
            lis = new List<SqlParameter>();
            string sqlWhere = "";
            List<string> sqlWheres = new List<string>();
            List<string> OWheres = new List<string>();
            #region --組WHERE--

           if (!this.DateArriveWH.Value1.Empty())
           {
               sqlWheres.Add("r.WhseArrival >= @DateArrStart");
               lis.Add(new SqlParameter("@DateArrStart", DateArrStart));
           }
           if (!this.DateArriveWH.Value2.Empty())
           {
               sqlWheres.Add("r.WhseArrival <= @DateArrEnd");
               lis.Add(new SqlParameter("@DateArrEnd", DateArrEnd));
           }
            
            if (!this.Supp.Empty())
           {
               sqlWheres.Add("ps.suppid = @Supp");
               lis.Add(new SqlParameter("@Supp", Supp));
              
           } if (!this.refno.Empty())
           {
               sqlWheres.Add("f.Refno = @refno");
               lis.Add(new SqlParameter("@refno", refno));

           } if (!this.brand.Empty())
           {
               sqlWheres.Add("p.brandid = @brand");
               lis.Add(new SqlParameter("@brand", brand));
              
           } if (!this.season.Empty())
           {
               sqlWheres.Add("p.Seasonid = @season");
               lis.Add(new SqlParameter("@season", season)); 
           }
           sqlWhere = string.Join(" and ", sqlWheres);
           
           if (!sqlWhere.Empty())
           {
               sqlWhere = " where " + sqlWhere;
           }
         
            #endregion
           #region --撈Excel資料--

           cmd = string.Format(@"
   
	select 
	ps.SuppID, rd.SEQ1,rd.Seq2, ps.ID as PoId,rd.Dyelot,
	rd.stockqty,
	f.TotalInspYds,
	f.id as Fir_id,
	f.Refno
	into #tmpAllData
from Receiving r WITH (NOLOCK) 
inner join Receiving_Detail rd WITH (NOLOCK) on r.id=rd.id 
inner join fir f WITH (NOLOCK) on rd.PoId=f.POID and rd.Seq1=f.SEQ1 and rd.Seq2=f.seq2
inner join PO_Supp ps WITH (NOLOCK) on rd.poid=ps.ID and ps.SEQ1=rd.Seq1 
inner join PO p WITH (NOLOCK) on p.ID = ps.ID
    " + sqlWhere + @"

	select SID.* ,
	ref.*,
	s.AbbEN,
	tmpSum.*,
    Yard.yrds,
	Point.Defect,
	Point.Point
into #tmp
from (
	select distinct SuppID
	from #tmpAllData
)as SID
inner join Supp s WITH (NOLOCK) on s.id=SID.SuppID
outer apply(
	select (
		Select t.Refno +',' 
		from (
               	SELECT DISTINCT Refno
				FROM #tmpAllData 
				WHERE  #tmpAllData.SuppID=SID.SuppID						
		)t 
		order by t.Refno
		for xml path('')	
			
	) as Refno
) as ref
outer apply(
	select Defect = (select DescriptionEN from FabricDefect where id=defect),sum(point) as Point 
	from (
			select substring(data,1,1) as Defect,CONVERT(int, substring(data,2,5)) as Point 
			from dbo.SplitString(
				(
					select concat('/',DefectRecord) 
					from FIR_Physical_Defect,#tmpAllData
						where FIR_Physical_Defect.id=#tmpAllData.Fir_id 
						and #tmpAllData.SuppID=SID.SuppID
						for xml path('')
				),'/')
		) a group by defect
) as Point
outer apply(
	SELECT  
        [stockqty]= sum(stockqty)
		,[TotalInspYds]= sum(TotalInspYds) 
		,[Inspected]= iif(sum(stockqty)!=0,round(sum(TotalInspYds)/sum(stockqty)*100,2),0) 				
	FROM #tmpAllData
	where #tmpAllData.SuppID = SID.SuppID
)as tmpSum
outer apply (
	select [yrds]=count (*) *5	
	from Receiving r WITH (NOLOCK) 
	inner join Receiving_Detail rd WITH (NOLOCK) on r.id=rd.id 
	inner join fir f WITH (NOLOCK) on rd.PoId=f.POID and rd.Seq1=f.SEQ1 and rd.Seq2=f.seq2
	inner join FIR_Physical fp on fp.ID=f.ID
	inner join FIR_Physical_Defect fpd on fpd.FIR_PhysicalDetailUKey = fp.DetailUkey
	 " + sqlWhere + @"
	and f.Suppid=SID.SuppID
)  as Yard
order by Refno

	select Suppid,defect,
	point  = sum(point) over(partition by suppid,defect)
	,ROW_NUMBER() over(partition by suppid order by suppid,point desc) RN 
	into #tmp2
	from #tmp
	order by Suppid,point desc

        declare @W_FabricDefect numeric(2,0)= (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Fabric Defect'),
        @W_LackingYardage numeric(2,0)= (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Lacking Yardage'),
        @W_Migration numeric(2,0)= (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Migration'),
        @W_ShortWidth numeric(2,0)= (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Short Width'),
        @W_Shading numeric(2,0)= (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Shading'),
        @W_Sharinkage numeric(2,0)= (select Weight from dbo.Inspweight WITH (NOLOCK) where id ='Sharinkage'),
     	@W_Total numeric(2,0)= (select sum(Weight) from dbo.Inspweight WITH (NOLOCK) )

       	select distinct Tmp.SuppID
	   	,Tmp.refno
       	,Tmp.abben
       	,Tmp.stockqty
       	,Tmp.TotalInspYds
       	,Tmp.Inspected
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
		select tmp.SuppID
       		,refno
       		,abben
       		,stockqty
       		,TotalInspYds
       		,Inspected
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
			select Sum(RD.stockqty)SHRINKAGEyards 
       		from #tmpAllData rd WITH (NOLOCK) 
				Where exists(select FL.* 
       						from FIR_Laboratory FL WITH (NOLOCK) 
       						inner join dbo.FIR_Laboratory_Heat h WITH (NOLOCK) on h.ID = FL.ID 
							INNER JOIN FIR F WITH (NOLOCK) ON FL.POID=F.POID AND F.Suppid=Tmp.SuppID 
       						where FL.HeatEncode=1 and h.Result = 'Fail' and FL.POID = rd.PoId and FL.SEQ1 = RD.seq1 
       						and FL.seq2 = RD.seq2 and h.Dyelot = RD.dyelot 
							)
       			or exists(select * 
       						from dbo.FIR_Laboratory FL WITH (NOLOCK) 
       						inner join dbo.FIR_Laboratory_Wash W WITH (NOLOCK) on W.ID = FL.ID
							INNER JOIN FIR F WITH (NOLOCK) ON FL.POID=F.POID AND F.Suppid=Tmp.SuppID
       						where FL.WashEncode=1 and W.Result = 'Fail' and FL.POID = RD.poid and FL.SEQ1 = RD.seq1 
       						and FL.seq2 = RD.seq2 and W.Dyelot = RD.dyelot )
							group by PoId,Seq1,Seq2,Dyelot 
       			)SHRINKAGE
	outer apply(
					select iif(Tmp.stockqty!=0,round(SHRINKAGE.SHRINKAGEyards/Tmp.stockqty*100,2),0)SHINGKAGE
				)SHINGKAGELevel 
	outer apply(SELECT Sum(RD.stockqty)MIGRATIONyards
       		from #tmpAllData rd WITH (NOLOCK) 
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
       			)MIGRATION 
	outer apply(select iif(Tmp.stockqty!=0,round(MIGRATION.MIGRATIONyards/Tmp.stockqty*100,2),0)MIGRATION)MIGRATIONLevel 
	outer apply(
			select Sum(rd.stockqty)SHADINGyards
       		from #tmpAllData rd WITH (NOLOCK) 
				Where exists(
					select * from fir f WITH (NOLOCK) 
       						inner join FIR_Shadebone fs WITH (NOLOCK) on fs.ID = f.ID
							where f.ShadebondEncode =1 and fs.Result = 'Fail' 
							and f.poid =rd.poid and f.SEQ2 = rd.seq1 and f.seq2 = rd.seq2 and fs.Dyelot  = rd.dyelot 
							and f.suppid=Tmp.SuppID
							) 
					or exists(
					select * from fir f WITH (NOLOCK) 
       						inner join FIR_Continuity fc WITH (NOLOCK) on fc.ID = f.ID
								where f.ContinuityEncode =1 and fc.Result = 'Fail' 
								and f.poid = rd.poid and f.seq1 = rd.seq1 and f.seq2 = rd.seq2 and fc.Dyelot  = rd.dyelot 
								and f.suppid=Tmp.SuppID
								)								 
       			)SHADING
	outer apply(select iif(Tmp.stockqty!=0,round(SHADING.SHADINGyards/Tmp.stockqty*100,2),0)SHADING )SHADINGLevel 
	outer apply(
		select sum(fp.ActualYds)ActualYds 
		from fir f WITH (NOLOCK) 
       		inner join FIR_Physical fp WITH (NOLOCK) on fp.ID = f.ID 
			where f.PhysicalEncode =1 and fp.Result = 'Fail'and f.Suppid=Tmp.SuppID 
			and  fp.ActualYds < fp.TicketYds 
            and exists(select distinct Fir_id from  #tmpAllData where Fir_id=f.ID)
			)LACKINGYARDAGE 
	outer apply(select iif(Tmp.stockqty!=0,round(LACKINGYARDAGE.ActualYds/Tmp.stockqty*100,2),0)LACKINGYARDAGE )LACKINGYARDAGELevel 
	outer apply(
			select sum(fp.ActualYds)SHORTWIDTH
				from  fir f WITH (NOLOCK) 
       		inner join FIR_Physical fp WITH (NOLOCK) on fp.ID = f.ID
       		where f.PhysicalEncode = 1 and fp.Result = 'Fail'  
			and f.Suppid=Tmp.SuppID
			and isNull(fp.ActualWidth,0) < isNull((select Fabric.Width from dbo.fabric WITH (NOLOCK) where fabric.SCIRefno = f.SCIRefno),0)
            and exists(select distinct Fir_id from  #tmpAllData where Fir_id=f.ID)
	)SHORTyards 
	outer apply(select iif(Tmp.stockqty!=0,round(SHORTyards.SHORTWIDTH/Tmp.stockqty*100,2),0)SHORTWIDTH )SHORTWIDTHLevel 
	order by Tmp.SuppID

	select *, 
		[TOTALLEVEL]=(select id from SuppLevel WITH (NOLOCK) where type='F' and range1 <= isnull([AVG],0) and range2 >= isnull([AVG],0))
	from(
		select * ,[Avg] = (select 
       		(	[Fabric(%)]* @W_FabricDefect +
       			[SHRINKAGE (%)] *  @W_Sharinkage +
       			[MIGRATION (%)] * @W_Migration +
       			[SHADING (%)] * @W_Shading +
       			[LACKINGYARDAGE(%)] *  @W_LackingYardage +
       			[SHORT WIDTH (%)] * @W_ShortWidth 
			)   /@W_Total  ) 
			from #TmpFinal) a
ORDER BY SUPPID
       
	drop table #tmp,#tmpAllData,#tmp2,#TmpFinal



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
