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

           if (!this.DateArriveWH.Value1.Empty() && !this.DateArriveWH.Value2.Empty())
            {
                sqlWheres.Add("r.WhseArrival between @DateArrStart and @DateArrEnd");
                lis.Add(new SqlParameter("@DateArrStart", DateArrStart));
                lis.Add(new SqlParameter("@DateArrEnd", DateArrEnd));
            } if (!this.Supp.Empty())
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
        ;With tmpAllData as ( 
		select 
			ps.SuppID, ps.SEQ1, ps.ID as PoId,
			rd.stockqty,
			f.TotalInspYds,
			fpd.Point
		from Receiving r 
        inner join Receiving_Detail rd on r.id=rd.id 
        inner join fir f on rd.PoId=f.POID and rd.Seq1=f.SEQ1 and rd.Seq2=f.seq2
        inner join PO_Supp ps on rd.poid=ps.ID and ps.SEQ1=rd.Seq1 
        inner join PO p on p.ID = ps.ID
        inner join FIR_Physical fp on fp.id=f.id 
        inner join FIR_Physical_Defect fpd on fpd.FIR_PhysicalDetailUKey=fp.DetailUkey
        " + sqlWhere + @"
	) 
	select SID.* ,
		ref.*,
		s.AbbEN,
		tmpSum.*
    into #tmp
	from (
		select distinct SuppID
		from tmpAllData
	)as SID
    inner join Supp s on s.id=SID.SuppID
	outer apply(
		select (
			Select t.Refno +',' 
			from (SELECT DISTINCT PSD.Refno 
					FROM tmpAllData , dbo.PO_Supp_Detail PSD 
					WHERE tmpAllData.poid = psd.id AND tmpAllData.SEQ1=PSD.SEQ1 and tmpAllData.SuppID = SID.SuppID
					
			)t 
			order by t.Refno
			for xml path('')	
			
		) as Refno
	) as ref
	outer apply(
		SELECT  sum(stockqty)stockqty
		   ,sum(TotalInspYds)TotalInspYds
		   ,iif(sum(stockqty)!=0,round(sum(TotalInspYds)/sum(stockqty)*100,0),0)Inspected 
		   ,count(*)*5 as yrds
		   ,iif(sum(TotalInspYds)!=0,round(count(*)*5/sum(TotalInspYds)*100,2),0)[Fabric(%)] 
		   ,sum(Point)Point	
		FROM tmpAllData
		where tmpAllData.SuppID = SID.SuppID
	)as tmpSum
	order by Refno

        declare @W_FabricDefect numeric(2,0)= (select Weight from dbo.Inspweight where id ='Fabric Defect'),
        @W_LackingYardage numeric(2,0)= (select Weight from dbo.Inspweight where id ='Lacking Yardage'),
        @W_Migration numeric(2,0)= (select Weight from dbo.Inspweight where id ='Migration'),
        @W_ShortWidth numeric(2,0)= (select Weight from dbo.Inspweight where id ='Short Width'),
        @W_Shading numeric(2,0)= (select Weight from dbo.Inspweight where id ='Shading'),
        @W_Sharinkage numeric(2,0)= (select Weight from dbo.Inspweight where id ='Sharinkage'),
     	@W_Total numeric(2,0)= (select sum(Weight) from dbo.Inspweight )

       select  Tmp.SuppID
	   	   ,Tmp.refno
       	   ,Tmp.abben
       	   ,Tmp.stockqty
       	   ,Tmp.TotalInspYds
       	   ,Tmp.Inspected
       	   ,Tmp.yrds
       	   ,Tmp.[Fabric(%)]
       	   ,Tmp.ID
       	   ,left(Tmp.Point,len(Tmp.Point)-1)as Point
       	   ,SHRINKAGE.SHRINKAGEyards
       	   ,SHINGKAGE[SHRINKAGE (%)] 
       	   ,(select id from SuppLevel where type='F' and range1 <= SHINGKAGE and range2 >= SHINGKAGE)SHINGKAGELevel
       	   ,MIGRATION.MIGRATIONyards
       	   ,MIGRATION[MIGRATION (%)]
       	   ,(select id from SuppLevel where type='F' and range1 <= MIGRATION and range2 >= MIGRATION)MIGRATIONLevel
       	   ,SHADING.SHADINGyards
       	   ,SHADING[SHADING (%)]
       	   ,(select id from SuppLevel where type='F' and range1 <= SHADING and range2 >= SHADING)SHADINGLevel
       	   ,LACKINGYARDAGE.ActualYds
       	   ,LACKINGYARDAGELevel.LACKINGYARDAGE[LACKINGYARDAGE(%)]
       	   ,(select id from SuppLevel where type='F' and range1 <= LACKINGYARDAGELevel.LACKINGYARDAGE and range2 >= LACKINGYARDAGELevel.LACKINGYARDAGE)LACKINGYARDAGELevel
       	   ,SHORTyards.SHORTWIDTH
       	   ,SHORTWIDTHLevel.SHORTWIDTH[SHORT WIDTH (%)]
       	   ,(select id from SuppLevel where type='F' and range1 <= SHORTWIDTHLevel.SHORTWIDTH and range2 >= SHORTWIDTHLevel.SHORTWIDTH)SHORTWIDTHLevel
       	   ,TOTAL.avg
       	   ,(select id from SuppLevel where type='F' and range1 <= TOTAL.avg and range2 >= TOTAL.avg)TOTALLEVEL
       from (select SuppID
       	        ,refno
       	        ,abben
       	        ,stockqty
       	        ,TotalInspYds
       	        ,Inspected
       	        ,yrds
       	        ,[Fabric(%)]
       	        ,FABRIC_Level.ID
       	        ,(select convert(varchar(20),topthree.Point)+';'
       			  from (select top 3 Point 
       			        from #tmp 
       				    order by Point desc)topthree 
       				    for xml path('')
       			 )Point
              from  #tmp tmp
              outer apply(select sl.id from SuppLevel sl where type='F' and range1 <= [Fabric(%)] and range2 >= [Fabric(%)])FABRIC_Level
              )Tmp
       outer apply(select Sum(RD.stockqty)SHRINKAGEyards 
       	        from Receiving_Detail rd 
                   Where exists(select FL.* 
       		                 from FIR_Laboratory FL
       			             inner join dbo.FIR_Laboratory_Heat h on h.ID = FL.ID 
							 INNER JOIN FIR F ON FL.POID=F.POID AND F.Suppid=Tmp.SuppID 
       			             where FL.HeatEncode=1 and h.Result = 'Fail' and FL.POID = rd.PoId and FL.SEQ1 = RD.seq1 
       			             and FL.seq2 = RD.seq2 and h.Dyelot = RD.dyelot )
       			   or exists(select * 
       					     from dbo.FIR_Laboratory FL
       					     inner join dbo.FIR_Laboratory_Wash W on W.ID = FL.ID
							 INNER JOIN FIR F ON FL.POID=F.POID AND F.Suppid=Tmp.SuppID
       					     where FL.WashEncode=1 and W.Result = 'Fail' and FL.POID = RD.poid and FL.SEQ1 = RD.seq1 
       					     and FL.seq2 = RD.seq2 and W.Dyelot = RD.dyelot ) 
       		      )SHRINKAGE
       outer apply(select iif(Tmp.stockqty!=0,round(SHRINKAGE.SHRINKAGEyards/Tmp.stockqty*100,2),0)SHINGKAGE)SHINGKAGELevel 
       outer apply(SELECT Sum(RD.stockqty)MIGRATIONyards
       		    from Receiving_Detail rd
       			Where exists(select * from dbo.FIR_Laboratory l
                                inner join dbo.FIR_Laboratory_Heat h on h.ID = l.ID
								INNER JOIN FIR F ON L.POID=F.POID AND F.Suppid=Tmp.SuppID 
                                where l.CrockingEncode=1 and h.Result = 'Fail' and l.POID = RD.poid and l.SEQ1 = RD.seq1 
                                and l.seq2 = RD.seq2) 
                      or exists(select * from Oven O 
					            inner join Oven_Detail OD on OD.ID = O.ID
					            INNER JOIN FIR F ON O.POID=F.POID AND F.Suppid=Tmp.SuppID 
                                where O.Status ='Confirmed' and OD.Result = 'Fail' 
                                and O.poid = RD.poid and OD.seq1 = RD.seq1 and OD.seq2 = RD.seq2 )
                      or exists(select * from ColorFastness CF 
								inner join ColorFastness_Detail CFD on CFD.ID = CF.ID
								INNER JOIN FIR F ON CF.POID=F.POID AND F.Suppid=Tmp.SuppID 
                                where CF.Status ='Confirmed' and CFD.Result = 'Fail' 
                                and CF.poid = RD.poid and CFD.seq1 = RD.seq1 and CFD.seq2 = RD.seq2)
       			  )MIGRATION 
       outer apply(select iif(Tmp.stockqty!=0,round(MIGRATION.MIGRATIONyards/Tmp.stockqty*100,2),0)MIGRATION)MIGRATIONLevel 
       outer apply(select Sum(rd.stockqty)SHADINGyards
       			from Receiving_Detail rd
                   Where exists(select * from fir f 
       						 inner join FIR_Shadebone fs on fs.ID = f.ID
                                where f.ShadebondEncode =1 and fs.Result = 'Fail' 
                                and f.poid =rd.poid and f.SEQ2 = rd.seq1 and f.seq2 = rd.seq2 and fs.Dyelot  = rd.dyelot and f.suppid=Tmp.SuppID) 
                       or exists(select * from fir f 
       				          inner join FIR_Continuity fc on fc.ID = f.ID
                                 where f.ContinuityEncode =1 and fc.Result = 'Fail' 
                                 and f.poid = rd.poid and f.seq1 = rd.seq1 and f.seq2 = rd.seq2 and fc.Dyelot  = rd.dyelot and f.suppid=Tmp.SuppID)
								 
       			  )SHADING
       outer apply(select iif(Tmp.stockqty!=0,round(SHADING.SHADINGyards/Tmp.stockqty*100,2),0)SHADING )SHADINGLevel 
       outer apply(select sum(fp.ActualYds)ActualYds from fir f 
       			inner join FIR_Physical fp on fp.ID = f.ID 
				where f.PhysicalEncode =1 and fp.Result = 'Fail'and f.Suppid=Tmp.SuppID 
				and  fp.ActualYds < fp.TicketYds )LACKINGYARDAGE 
       outer apply(select iif(Tmp.stockqty!=0,round(LACKINGYARDAGE.ActualYds/Tmp.stockqty*100,2),0)LACKINGYARDAGE )LACKINGYARDAGELevel 
       outer apply(select sum(fp.ActualYds)SHORTWIDTH
                   from  fir f 
       			inner join FIR_Physical fp on fp.ID = f.ID
       			where f.PhysicalEncode = 1 and fp.Result = 'Fail'  
				and isNull(fp.ActualWidth,0) < isNull((select Fabric.Width from dbo.fabric where fabric.SCIRefno = f.SCIRefno),0)
		)SHORTyards 
       outer apply(select iif(Tmp.stockqty!=0,round(SHORTyards.SHORTWIDTH/Tmp.stockqty*100,2),0)SHORTWIDTH )SHORTWIDTHLevel 
       outer apply(select 
       		(	Tmp.[Fabric(%)]*@W_FabricDefect +
       			SHINGKAGE*  @W_Sharinkage +
       			MIGRATION* @W_Migration +
       			SHADING* @W_Shading +
       			LACKINGYARDAGELevel.LACKINGYARDAGE*  @W_LackingYardage +
       			SHORTWIDTHLevel.SHORTWIDTH* @W_ShortWidth 
       		)   /@W_Total  [avg])TOTAL
       order by Tmp.SuppID
       
       drop table #tmp");
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
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found");
                return false;
            }
            var saveDialog = Sci.Utility.Excel.MyExcelPrg.GetSaveFileDialog(Sci.Utility.Excel.MyExcelPrg.filter_Excel);
            saveDialog.ShowDialog();
            string outpath = saveDialog.FileName;
            if (outpath.Empty())
            {
                return false;
            }

            Sci.Utility.Excel.SaveXltReportCls xl = new Sci.Utility.Excel.SaveXltReportCls("Quality_R06.xltx");
            xl.dicDatas.Add("##body", dt);
            xl.Save(outpath, false);
            return true;
           
        }
    }
}
