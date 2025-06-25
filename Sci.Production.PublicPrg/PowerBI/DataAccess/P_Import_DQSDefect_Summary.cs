using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 MES/Endline/R08 已脫鉤 待討論
    /// </summary>
    public class P_Import_DQSDefect_Summary
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_DQSDefect_Summary(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            try
            {
                finalResult = this.GetDQSDefect_Summary_Data(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                DataTable dataTable = finalResult.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetDQSDefect_Summary_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", item.SDate),
                new SqlParameter("@eDate", item.EDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@" 
			select 
			 [InspectionDate] = ins.InspectionDate
			,[FirstInspectionDate] = cast(Ins.AddDate as date)
			,[FactoryID] = ins.FactoryID
			,[Brand] = ord.BrandID
			,[Style] = ord.styleid
			,[PO#] = ord.custpono
			,[SP#] = ins.OrderId
			,ins.Article
			,ins.Size
			,[Destination] = Cou.Alias
			,ord.CdCodeID
			,cdc.ProductionFamilyID
			,ins.Team
			,ins.AddName
			,ins.Shift
			,ins.Line
			,s.SewingCell
			,[PassQty] = SUM(IIF(ins.Status = ('Pass') , 1 ,0))
			,[RejectAndFixedQty] = SUM(IIF(ins.Status in ('Reject','Fixed','Dispose') , 1 ,0))
			,[TtlQty] = SUM(IIF(ins.Status = ('Pass') , 1 ,0)) + SUM(IIF(ins.Status in ('Reject','Fixed','Dispose') , 1 ,0))
			,[CDCodeNew] = sty.CDCodeNew
			,[ProductType] = sty.ProductType
			,[FabricType] = sty.FabricType
			,[Lining] = sty.Lining
			,[Gender] = sty.Gender
			,[Construction] = sty.Construction
			,ord.SewInLine
			into #tmp_summy_first
			from [ExtendServer].ManufacturingExecution.dbo.Inspection ins WITH(NOLOCK) 
			inner join Production.dbo.Orders ord WITH(NOLOCK) on ins.OrderId=ord.id
			inner join Production.dbo.Factory fac WITH(NOLOCK) on ins.FactoryID=fac.ID
			left  join Production.dbo.SewingLine s WITH(NOLOCK) on s.FactoryID = ins.FactoryID and s.ID = ins.Line
			left  join Production.dbo.Country Cou WITH(NOLOCK) on ord.Dest = Cou.ID
			left  join Production.dbo.CDCode cdc WITH(NOLOCK) on ord.CdCodeID = cdc.ID
			Outer apply (
				SELECT s.[ID]
					, ProductType = r2.Name
					, FabricType = r1.Name
					, Lining
					, Gender
					, Construction = d1.Name
					, s.CDCodeNew
				FROM Production.dbo.Style s WITH(NOLOCK)
				left join Production.dbo.DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
				left join Production.dbo.Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
				left join Production.dbo.Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
				where s.Ukey = ord.StyleUkey
			)sty
			where ((ins.EditDate >= @sDate and ins.EditDate <= @eDate) or (ins.Adddate >= @sDate and ins.Adddate <= @eDate))  
			group by ins.InspectionDate, cast(Ins.AddDate as date), ins.FactoryID, ord.BrandID, ord.styleid, ord.custpono, 
			ins.OrderId, ins.Article, ins.Size, Cou.Alias, ord.CdCodeID, cdc.ProductionFamilyID,
			ins.Team, ins.AddName, ins.Shift, ins.Line, s.SewingCell, sty.CDCodeNew,
			sty.ProductType, sty.FabricType, sty.Lining, sty.Gender, sty.Construction,ord.SewInLine
			

			select t.InspectionDate 
				, t.FirstInspectionDate
				, t.FactoryID
				, t.Brand
				, t.Style
				, t.[PO#]
				, t.[SP#]
				, t.Article
				, t.Size
				, t.[Destination]
				, t.CdCodeID
				, t.ProductionFamilyID
				, t.Team
				, t.AddName
				, t.Shift
				, t.Line
				, t.SewingCell	
				, t.TtlQty
				, t.RejectAndFixedQty
				,[EndlineWFT] = iif(t.TtlQty = 0, 0, ROUND( (t.RejectAndFixedQty *1.0) / (t.TtlQty *1.0) *100,3))
				,[Endline RFT(%)] =isnull(RftValue.VAL,0)
				, t.CDCodeNew
				, t.ProductType
				, t.FabricType
				, t.Lining
				, t.Gender
				, t.Construction
				, detail.DefectQty
				, [BIFactoryID] = @BIFactoryID
				, [BIInsertDate] = GETDATE()
			from #tmp_summy_first t
			outer apply
			(
				SELECT 
				VAL = isnull(Convert(float(50),Convert(FLOAT(50), round(((R.InspectQty-R.RejectQty)/ nullif(R.InspectQty, 0))*100,2))),0)
				FROM Production.dbo.SewingOutput_Detail sod 
				inner join Production.dbo.SewingOutput so with(nolock) on so.id = sod.id
				inner join Production.dbo.Rft r on r.OrderID = sod.OrderId AND
														   r.SewinglineID = so.SewingLineID AND
														   r.Team = so.Team AND
														   r.Shift = so.Shift AND
														   r.CDate = so.OutputDate
				WHERE sod.OrderId = t.[SP#] and so.SewinglineID = t.Line and so.FactoryID = t.FactoryID and sod.Article = t.Article
				and so.Shift= iif(t.Shift = 'Day','D','N') 
				and r.CDate = t.SewInLine
			)RftValue
			outer apply (
				select DefectQty = COUNT(*)
				from [ExtendServer].ManufacturingExecution.dbo.Inspection insp WITH(NOLOCK) 
				inner join [ExtendServer].ManufacturingExecution.dbo.Inspection_Detail ind WITH(NOLOCK) on insp.ID = ind.ID
				where insp.Adddate >= @sDate
				and insp.Status <> 'Pass'
				and t.InspectionDate = insp.InspectionDate
				and t.FirstInspectionDate = cast(insp.AddDate as Date)
				and t.FactoryID = insp.FactoryID
				and t.[SP#] = insp.OrderId
				and t.Article = insp.Article
				and t.Size = insp.Size
				and t.Team = insp.Team
				and t.AddName = insp.AddName
				and t.Shift = insp.Shift
				and t.Line = insp.Line
			) detail

			drop table #tmp_summy_first";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable dt),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dt;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@"

				UPDATE P SET
				P.[BrandID]			= isnull(T.Brand,'')
				,P.[StyleID]			= isnull(T.Style,'')
				,P.[POID]				= isnull(T.[PO#], '')					
				,P.[Destination]		= isnull(T.Destination, '')
				,P.[CDCode]				= isnull(T.CdCodeID, '')
				,P.[ProductionFamilyID]	= isnull(T.ProductionFamilyID,'')
				,P.[Team]				= isnull(T.Team, '')
				,P.[Cell]				= isnull(T.SewingCell, '')	
				,P.[InspectQty]			= isnull(T.TtlQty, 0)
				,P.[RejectQty]			= isnull(T.RejectAndFixedQty, 0)
				,P.[WFT]				= isnull(T.[EndlineWFT], 0) 
				,P.[RFT]				= isnull(T.[Endline RFT(%)], 0) 
				,P.[CDCodeNew]			= isnull(T.CDCodeNew, '')
				,P.[ProductType]		= isnull(T.ProductType,'')
				,P.[FabricType]			= isnull(T.FabricType, '')
				,P.[Lining]				= isnull(T.Lining,'')
				,P.[Gender]				= isnull(T.Gender,'')
				,P.[Construction]		= isnull(T.Construction,'')
				,P.[DefectQty]			= isnull(T.DefectQty, 0)
				FROM P_DQSDefect_Summary P 
				INNER JOIN #Final_DQSDefect_Summary T ON P.[FirstInspectDate] = T.FirstInspectionDate AND 
														 P.[SPNO] = T.[SP#] AND 
														 P.[Article] = T.[Article] AND
														 P.[SizeCode] = T.[Size] AND
														 P.[QCName] = T.[AddName] AND
														 P.[Shift] = T.[Shift] AND
														 P.[Line] = T.[Line] AND
														 P.[InspectionDate] = T.[InspectionDate] AND
														 P.[FactoryID] = T.[FactoryID]													


				INSERT INTO [dbo].[P_DQSDefect_Summary]
				(
					[InspectionDate]
					,[FirstInspectDate]
					,[FactoryID]
					,[BrandID]
					,[StyleID]
					,[POID]
					,[SPNO]
					,[Article]
					,[SizeCode]
					,[Destination]
					,[CDCode]
					,[ProductionFamilyID]
					,[Team]
					,[QCName]
					,[Shift]
					,[Line]
					,[Cell]
					,[InspectQty]
					,[RejectQty]
					,[WFT]
					,[RFT]
					,[CDCodeNew]
					,[ProductType]
					,[FabricType]
					,[Lining]
					,[Gender]
					,[Construction]
					,[DefectQty]
					,[BIFactoryID]
					,[BIInsertDate]
				)
				select　
				  InspectionDate 
				, FirstInspectionDate
				, isnull(FactoryID,'')
				, isnull(Brand,'')
				, isnull(Style,'')
				, isnull([PO#], '')
				, isnull([SP#], '')
				, isnull(Article, '')
				, isnull(Size, '')
				, isnull(Destination, '')
				, isnull(CdCodeID, '')
				, isnull(ProductionFamilyID,'')
				, isnull(Team, '')
				, isnull(AddName ,'')
				, isnull([Shift], '')
				, isnull(Line, '')
				, isnull(SewingCell, '')	
				, isnull(TtlQty, 0)
				, isnull(RejectAndFixedQty, 0)
				, isnull([EndlineWFT], 0) 
				, isnull([Endline RFT(%)], 0) 
				, isnull(CDCodeNew, '')
				, isnull(ProductType,'')
				, isnull(FabricType, '')
				, isnull(Lining,'')
				, isnull(Gender,'')
				, isnull(Construction,'')
				, isnull(DefectQty, 0)
				, ISNULL(BIFactoryID, '')
                , ISNULL(BIInsertDate, GetDate())
				from #Final_DQSDefect_Summary T
				where not exists (
					select 1 from P_DQSDefect_Summary P 
					where
					P.[FirstInspectDate] = T.FirstInspectionDate AND 
					P.[SPNO] = T.[SP#] AND
					P.[Article] = T.[Article] AND
					P.[SizeCode] = T.[Size] AND
					P.[QCName] = T.[AddName] AND
					P.[Shift] = T.[Shift] AND
					P.[Line] = T.[Line] AND
					P.[InspectionDate] = T.[InspectionDate] AND
					P.[FactoryID] = T.[FactoryID]	
				)

				Insert Into P_DQSDefect_Summary_History([FirstInspectDate], [SPNO], [Article], [SizeCode], [QCName], [Shift], [Line], [InspectionDate], [FactoryID], [BIFactoryID], [BIInsertDate])
				Select [FirstInspectDate], [SPNO], [Article], [SizeCode], [QCName], [Shift], [Line], [InspectionDate], [FactoryID], BIFactoryID, GETDATE()
				FROM P_DQSDefect_Summary T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)

				Delete p
				from P_DQSDefect_Summary p
				where not exists 
				(
					select 1 from #Final_DQSDefect_Summary t 
					where
					P.[FirstInspectDate] = T.FirstInspectionDate AND 
					P.[SPNO] = T.[SP#] AND
					P.[Article] = T.[Article] AND
					P.[SizeCode] = T.[Size] AND
					P.[QCName] = T.[AddName] AND
					P.[Shift] = T.[Shift] AND
					P.[Line] = T.[Line] AND
					P.[InspectionDate] = T.[InspectionDate] AND
					P.[FactoryID] = T.[FactoryID]
				)
";
                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_DQSDefect_Summary");
            }

            return finalResult;
        }
    }
}
