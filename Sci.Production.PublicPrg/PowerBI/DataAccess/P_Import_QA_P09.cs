using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/QA/P09已脫鉤 待討論
    /// </summary>
    public class P_Import_QA_P09
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_QA_P09(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                var today = DateTime.Now;
                var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
                sDate = firstDayOfCurrentMonth.AddMonths(-6);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now.AddMonths(3).Date;
            }

            try
            {
                Base_ViewModel resultReport = this.Get_QA_P09_Data((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate, (DateTime)eDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel Get_QA_P09_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@" 
            declare @sDate varchar(20) = '{sdate.ToString("yyyy/MM/dd")}'
            declare @eDate varchar(20) = '{edate.ToString("yyyy/MM/dd")}'

            ----準備基礎資料
	        Select RowNo = ROW_NUMBER() OVER(ORDER by Month), ID 
			Into #probablySeasonList
			From Production.dbo.SeasonSCI

	       	select distinct
			[WK#] = ed.id,
			[Invoice#]= ed.InvoiceNo,
			[ATA] = Export.WhseArrival,
			[ETA] = Export.ETA,
			[Season] = o.SeasonID,
			[SP#] = ed.PoID,
			[Seq#] = ed.seq1+''-''+ed.seq2,	
			[Brand] = o.BrandID,
			[Supp] = ps.SuppID,
			[Supp Name] = Supp.AbbEN,
			[Ref#] = psd.Refno,
			[Color] = c.ColorName,
			Qty = isnull(ed.Qty,0) + isnull(ed.Foc,0),	
			[1st Bulk Dyelot_Fty Received Date] = FirstDyelot.FirstDyelot,
			[1st Bulk Dyelot_Supp Sent Date]  = IIF(FirstDyelot.FirstDyelot is null and f.RibItem = 1
								,'RIB no need first dye lot'
								,IIF(FirstDyelot.SeasonID is null
										,'Still not received and under pushing T2. Please contact with PR if you need L/G first.'
										,format(FirstDyelot.FirstDyelot,'yyyy/MM/dd')
									)
							),
			[T1 Inspected Yards] = isnull(a.T1InspectedYards,0),
			[T1 Defect Points] = isnull(b.T1DefectPoints,0),
			[Fabric with clima] = isnull(f.Clima,0),
			ColorID = pc.SpecValue,
			ed.seq1,
			ed.seq2,
			ed.Ukey,
			[bitRefnoColor] = case when f.Clima = 1 then ROW_NUMBER() over(partition by f.Clima, ps.SuppID, psd.Refno, pc.SpecValue, Format(Export.CloseDate,'yyyyMM') order by Export.CloseDate) else 0 end,
			[FactoryID] = o.FactoryID,
			Export.Consignee
			into #tmpBasic
			from Production.dbo.Export_Detail ed with(nolock)
            inner join Production.dbo.Export with(nolock) on Export.id = ed.id and Export.Confirm = 1
            inner join Production.dbo.orders o with(nolock) on o.id = ed.PoID
            left join Production.dbo.Po_Supp_Detail psd with(nolock) on psd.id = ed.poid and psd.seq1 = ed.seq1 and psd.seq2 = ed.seq2
            left join Production.dbo.PO_Supp ps with(nolock) on ps.id = psd.id and ps.SEQ1 = psd. SEQ1
            left join Production.dbo.Supp with(nolock) on Supp.ID = ps.SuppID
            left join Production.dbo.Season s with(nolock) on s.ID = o.SeasonID and s.BrandID = o.BrandID
            left join Production.dbo.Factory fty with (nolock) on fty.ID = Export.Consignee
            left join Production.dbo.Fabric f with(nolock) on f.SCIRefno = psd.SCIRefno
            left join Production.dbo.PO_Supp_Detail_Spec pc with(nolock) on psd.ID = pc.ID and psd.SEQ1 = pc.SEQ1 and psd.SEQ1 = pc.SEQ2 and pc.SpecColumnID = 'Color'
            Left join #probablySeasonList seasonSCI on seasonSCI.ID = s.SeasonSCIID
            OUTER APPLY
            (
	            Select Top 1 FirstDyelot,SeasonID
	            From Production.dbo.FirstDyelot fd
	            Inner join #probablySeasonList season on fd.SeasonID = season.ID
	            WHERE fd.BrandRefno = psd.Refno and fd.ColorID = pc.SpecValue and fd.SuppID = ps.SuppID and fd.TestDocFactoryGroup = fty.TestDocFactoryGroup
		            And seasonSCI.RowNo >= season.RowNo
	            Order by season.RowNo Desc
            )FirstDyelot
            outer apply
            (
	            select T1InspectedYards=sum(fp.ActualYds)
	            from Production.dbo.fir f
	            left join Production.dbo.FIR_Physical fp on fp.id = f.id
	            left join Production.dbo.Receiving r on r.id = f.ReceivingID
	            where r.InvNo = ed.ID and f.POID = ed.PoID and f.SEQ1 =ed.Seq1 and f.SEQ2 = ed.Seq2
            )a
            outer apply
            (
	            select  T1DefectPoints = sum(fp.TotalPoint)
	            from Production.dbo.fir f
	            left join Production.dbo.FIR_Physical fp on fp.id = f.id
	            left join Production.dbo.Receiving r on r.id = f.ReceivingID
	            where r.InvNo = ed.ID and f.POID = ed.PoID and f.SEQ1 = ed.Seq1 and f.SEQ2 = ed.Seq2
            )b
            outer apply
            (
                select [ColorName] = iif(c.Varicolored > 1, c.Name, c.ID)
                from Production.dbo.Color c
                where c.ID = pc.SpecValue
                and c.BrandID = psd.BrandID 
            )c
            where  Export.ETA between @sDate and @eDate
            and psd.FabricType = 'F'
            and (ed.qty + ed.Foc) > 0
            and o.Category in('B','M')
            and o.BrandID = 'ADIDAS'

            select t.*
	        ,sr.documentName
	        ,sr.ReportDate
            ,sr.T2InspYds
            ,sr.T2DefectPoint
            ,sr.T2Grade
	        ,sr2.AWBno
	        ,sr2.TestReportCheckClima
            into #tmpReportDate
            from #tmpBasic t
            left join Production.dbo.NewSentReport sr with (nolock) on sr.exportID = t.WK# and sr.poid = t.SP# and sr.Seq1 =t.Seq1 and sr.Seq2 = t.Seq2
            outer apply 
            (
	            select sr2.AWBno,sr2.TestReportCheckClima
	            from Production.dbo.NewSentReport sr2 with (nolock) 
	            where sr2.exportID = t.WK# and sr2.poid = t.SP# and sr2.Seq1 =t.Seq1 and sr2.Seq2 = t.Seq2
	            and sr2.documentName = 'Continuity card'
            )sr2

            select t.*
	            ,sr.documentName
                ,sr.FTYReceivedReport
            into #tmpFTYReceivedReport
            from #tmpBasic t
            left join Production.dbo.NewSentReport sr with (nolock) on sr.exportID = t.WK# and sr.poid = t.SP# and sr.Seq1 =t.Seq1 and sr.Seq2 = t.Seq2

            select distinct  
	        a.[WK#],     
	        a.[Invoice#],   
	        a.[ATA],   
	        a.[ETA],      
	        a.[Season],
	        a.[SP#],   
	        a.[Seq#],      
	        a.[Brand],   
	        a.[Supp],   
	        a.[Supp Name],  
	        a.[Ref#],   
	        a.[Color],   
	        a.Qty,   
	        [Inspection Report_Fty Received Date] = c.[Inspection Report], 
	        [Inspection Report_Supp Sent Date] = b.[Inspection Report],   
	        [Test Report_Fty Received Date] = c.[Test report],   
	        [Test Report_ Check Clima] = 0, -- NewSentReport 沒有該欄位[TestReportCheckClima]   
	        [Test Report_Supp Sent Date] = b.[Test report], 
	        [Continuity Card_Fty Received Date] = c.[Continuity card],  
	        [Continuity Card_Supp Sent Date] = b.[Continuity card], 
	        [Continuity Card_AWB#] = b.AWBno,   
	        a.[1st Bulk Dyelot_Fty Received Date], 
	        a.[1st Bulk Dyelot_Supp Sent Date]  ,  
	        [T2 Inspected Yards] = b.T2InspYds,  
	        [T2 Defect Points] = b.T2DefectPoint,  
	        [Grade] =  b.T2Grade,  
	        a.[T1 Inspected Yards], 
	        a.[T1 Defect Points] , 
	        a.[Fabric with clima], 
	        a.ColorID,  
	        a.seq1,   
	        a.seq2,   
	        a.[bitRefnoColor], 
	        a.[FactoryID], 
	        a.Consignee  
	        from #tmpBasic a
	        inner join 
	        (    
		        SELECT * FROM 
		        (
			        SELECT *
			        FROM 
			        (
				        SELECT WK#,SP#,Seq1,Seq2,ReportDate ,documentname,AWBno,T2InspYds,T2DefectPoint,[T1 Inspected Yards],[T1 Defect Points],TestReportCheckClima
				        FROM #tmpReportDate
			        ) s
			        CROSS APPLY
			        (
				        SELECT MAX(T2Grade) AS T2Grade
				        FROM #tmpReportDate
				        WHERE WK# = s.WK# AND SP# = s.SP# AND Seq1 = s.Seq1 AND Seq2 = s.Seq2
			        ) p2
		        )AA
		        PIVOT
		        (
			        MAX(ReportDate) FOR documentname IN ([Continuity card], [Inspection Report], [Test report])
		        ) p1
	        )b on a.WK# = b.WK# and a.SP# = b.SP# and a.Seq1=b.Seq1 and a.Seq2=b.Seq2   
	        inner join 
	        (   
		        select *
		        from(
			        select WK#,SP#,Seq1,Seq2,FTYReceivedReport ,documentname
			        from #tmpFTYReceivedReport 
		        )s	
		        pivot(
			        max(FTYReceivedReport)
			        for documentname in([Continuity card],[Inspection Report],[Test report])
		        ) aa
	        )c on a.WK# = c.WK# and a.SP# = c.SP# and a.Seq1 = c.Seq1 and a.Seq2 = c.Seq2  
            drop table #probablySeasonList,#tmpBasic,#tmpFTYReceivedReport,#tmpReportDate
            ";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate, DateTime edate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
				declare @sDate varchar(20) = '{sdate.ToString("yyyy/MM/dd")}'
				declare @eDate varchar(20) = '{edate.ToString("yyyy/MM/dd")}'

                -----開始Merge 
				MERGE INTO dbo.P_QA_P09 t
				USING #tmpFinal s 
				ON t.WK#=s.WK# AND t.SP#=s.SP# AND t.Seq# = s.Seq#
				WHEN MATCHED THEN   
				UPDATE SET 
				t.WK# =  s.WK#,
				t.Invoice# =  s.Invoice#,
				t.ATA =  s.ATA,
				t.ETA =  s.ETA,
				t.Season =  s.Season,
				t.SP# =  s.SP#,
				t.Seq# =  s.Seq#,
				t.Brand =  s.Brand,
				t.Supp =  s.Supp,
				t.[Supp Name] =  s.[Supp Name],
				t.Ref# =  s.Ref#,
				t.Color =  s.Color,
				t.Qty =  s.Qty,
				t.[Inspection Report_Fty Received Date] =  s.[Inspection Report_Fty Received Date],
				t.[Inspection Report_Supp Sent Date] =  s.[Inspection Report_Supp Sent Date],
				t.[Test Report_Fty Received Date] =  s.[Test Report_Fty Received Date],
				t.[Test Report_ Check Clima] =  s.[Test Report_ Check Clima],
				t.[Test Report_Supp Sent Date] =  s.[Test Report_Supp Sent Date],
				t.[Continuity Card_Fty Received Date] =  s.[Continuity Card_Fty Received Date],
				t.[Continuity Card_Supp Sent Date] =  s.[Continuity Card_Supp Sent Date],
				t.[Continuity Card_AWB#] =  s.[Continuity Card_AWB#],
				t.[1st Bulk Dyelot_Fty Received Date] =  s.[1st Bulk Dyelot_Fty Received Date],
				t.[1st Bulk Dyelot_Supp Sent Date] =  s.[1st Bulk Dyelot_Supp Sent Date],
				t.[T2 Inspected Yards] =  s.[T2 Inspected Yards],
				t.[T2 Defect Points] =  s.[T2 Defect Points],
				t.[Grade] =  s.Grade,
				t.[T1 Inspected Yards] =  s.[T1 Inspected Yards],
				t.[T1 Defect Points] =  s.[T1 Defect Points],
				t.[Fabric with clima] =  s.[Fabric with clima],
				t.FactoryID = s.FactoryID,
				t.Consignee = s.Consignee
				WHEN NOT MATCHED BY TARGET THEN
					INSERT (WK#,Invoice#,ATA,ETA,Season,[SP#],[Seq#],Brand,Supp,[Supp Name],[Ref#],Color,Qty,[Inspection Report_Fty Received Date]
							,[Inspection Report_Supp Sent Date],[Test Report_Fty Received Date],[Test Report_ Check Clima],[Test Report_Supp Sent Date]
							,[Continuity Card_Fty Received Date],[Continuity Card_Supp Sent Date],[Continuity Card_AWB#],[1st Bulk Dyelot_Fty Received Date]
							,[1st Bulk Dyelot_Supp Sent Date],[T2 Inspected Yards],[T2 Defect Points],[Grade],[T1 Inspected Yards],[T1 Defect Points],[Fabric with clima]
							,FactoryID, Consignee
							)
					VALUES (s.WK#,s.Invoice#,s.ATA,s.ETA,s.Season,s.[SP#],s.[Seq#],s.Brand,s.Supp,s.[Supp Name],s.[Ref#],s.Color,s.Qty,s.[Inspection Report_Fty Received Date]
							,s.[Inspection Report_Supp Sent Date],s.[Test Report_Fty Received Date],s.[Test Report_ Check Clima],s.[Test Report_Supp Sent Date]
							,s.[Continuity Card_Fty Received Date],s.[Continuity Card_Supp Sent Date],s.[Continuity Card_AWB#],s.[1st Bulk Dyelot_Fty Received Date]
							,s.[1st Bulk Dyelot_Supp Sent Date],s.[T2 Inspected Yards],s.[T2 Defect Points],s.[Grade],s.[T1 Inspected Yards],s.[T1 Defect Points],s.[Fabric with clima]
							,s.FactoryID, s.Consignee
							);


				delete t 
				from dbo.P_QA_P09 t
				left join #tmpFinal s on t.WK#=s.WK#  AND t.SP#=s.SP# AND t.Seq# = s.Seq#
				where s.WK# is null
				and T.ETA between @sDate and @eDate

					DROP TABLE #tmpFinal

				update b set b.TransferDate = getdate(), b.IS_Trans = 1
				from BITableInfo b 
				where b.id = 'P_QA_P09'
                ";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#tmpFinal");
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
