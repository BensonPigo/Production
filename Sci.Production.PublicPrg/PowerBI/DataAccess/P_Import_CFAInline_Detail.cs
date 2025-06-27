using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/QA/R21已脫鉤 待討論
    /// </summary>
    public class P_Import_CFAInline_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_CFAInline_Detail(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.Year.ToString());
            }

            try
            {
                Base_ViewModel resultReport = this.GetCFAInline_Detail_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, item);
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

        private Base_ViewModel GetCFAInline_Detail_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@sDate", item.SDate),
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@" 
			select DISTINCT
			[Action]= b.Action
			,[Area]= b.CFAAreaID +' - '+ar.Description
			,a.cDate
			,c.BrandID
			, c.BuyerDelivery 
			,[Cfa] = isnull((select CONCAT(a.CFA, ':', Name) from Production.dbo.Pass1  WITH (NOLOCK) where ID = a.CFA),'')
			,[Defect Description]= gd.Description
			,a.DefectQty
			, [Destination]=ct.Alias
			,c.FactoryID
			,[GarmentOutput]= round(a.GarmentOutput/100,2)	
			,[Stage]= 
					case a.Stage when 'I' then 'Comments/Roving'
						when 'C' then 'Change Over'
						when 'P' then 'Stagger'
						when 'R' then 'Re-Stagger'
						when 'F' then 'Final'
						when 'B' then 'Buyer'
			else '' end
			,a.SewingLineID
			,[No. Of Defect]=b.Qty
			,c.Qty
			,c.CustPONo
			,[Remark]= b.Remark
			,[Result]= case a.result 
						when 'P' then 'Pass'
						when 'F' then 'Fail'
			else '' end 
			,a.InspectQty		
			,[shift]= case a.shift 
						when 'D' then 'DAY'
						when 'N' then 'NIGHT'
						when 'O' then 'SUBCON OUT'
						when 'I' then 'SUBCON IN'
			else '' end
			,a.OrderID
			,[SQR]= iif(a.InspectQty=0,0,round(a.DefectQty/a.InspectQty,3)) 	
			,c.StyleID
			,a.Team
			,[VAS/SHAS]= iif(c.VasShas=0,'','v') 
            ,[BIFactoryID] = @BIFactoryID
            ,[BIInsertDate] = GetDate()
			from Production.dbo.Cfa a WITH (NOLOCK) 
			inner join Production.dbo.Cfa_Detail b WITH (NOLOCK) on b.id = a.ID 
			inner join Production.dbo.orders c WITH (NOLOCK) on c.id = a.OrderID
			inner JOIN Production.dbo.Country ct WITH (NOLOCK) ON ct.ID=c.Dest
			outer apply(select Description from Production.dbo.GarmentDefectCode a WITH(NOLOCK) where a.id=b.GarmentDefectCodeID) as gd
			outer apply(select description from Production.dbo.cfaarea a WITH(NOLOCK) where a.id=b.CFAAreaID) as ar
			where a.Status = 'Confirmed' and a.cDate >= @sDate";
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

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
                if @IsTrans = 1
                begin
				    Insert Into P_CFAInline_Detail_History (Ukey, FactoryID, BIFactoryID, BIInsertDate)
				    Select Ukey, FactoryID, BIFactoryID, GETDATE()
				    FROM P_CFAInline_Detail T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)
                end

				DELETE T FROM P_CFAInline_Detail T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)

				INSERT INTO [dbo].[P_CFAInline_Detail]
                (
                    [Action]
                    ,[Area]
                    ,[AuditDate]
                    ,[BrandID]
                    ,[BuyerDelivery]
                    ,[CfaName]
                    ,[DefectDesc]
                    ,[DefectQty]
                    ,[Destination]
                    ,[FactoryID]
                    ,[GarmentOutput]
                    ,[InspectionStage]
                    ,[Line]
                    ,[NumberDefect]
                    ,[OrderQty]
                    ,[POID]
                    ,[Remark]
                    ,[Result]
                    ,[InspectQty]
                    ,[Shift]
                    ,[SPNO]
                    ,[SQR]
                    ,[StyleID]
                    ,[Team]
                    ,[VASSHAS]
                    ,[BIFactoryID]
                    ,[BIInsertDate]
                )
                select 
	             [Action]= isnull([Action] ,'' )
	            ,[Area]= isnull([Area] ,'')
	            ,cDate
	            ,isnull(BrandID ,'')
	            ,BuyerDelivery 
	            ,[Cfa] = isnull([Cfa],'')
	            ,[Defect Description]= isnull([Defect Description],'')
	            ,isnull(DefectQty, 0)
	            ,[Destination]=isnull([Destination],'')
	            ,isnull(FactoryID,'') 
	            ,[GarmentOutput]= isnull([GarmentOutput],0)
	            ,[Stage]= isnull([Stage],'')
	            ,isnull(SewingLineID,'')
	            ,isnull([No. Of Defect],0)
	            ,isnull(Qty,0)
	            ,isnull(CustPONo, '')
	            ,isnull([Remark], '')
	            ,isnull([Result], '')
	            ,isnull(InspectQty, 0)		
	            ,isnull([shift], '')
	            ,isnull(OrderID, '')
	            ,isnull([SQR], 0)
	            ,isnull(StyleID, '')
	            ,isnull(Team, '')
	            ,isnull([VAS/SHAS], '') 
                ,isnull(BIFactoryID, '')
                ,isnull(BIInsertDate, GetDate())
                from #Final_P_CFAInline_Detail";

                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@BIFactoryID", item.RgCode),
                    new SqlParameter("@IsTrans", item.IsTrans),
                };

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_P_CFAInline_Detail", paramters: sqlParameters);
            }

            return finalResult;
        }
    }
}
