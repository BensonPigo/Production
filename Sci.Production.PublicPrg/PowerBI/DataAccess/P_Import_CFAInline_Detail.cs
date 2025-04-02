using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/QA/R21已脫鉤 待討論
    /// </summary>
    public class P_Import_CFAInline_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_CFAInline_Detail(DateTime? sDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.Year.ToString());
            }

            try
            {
                Base_ViewModel resultReport = this.GetCFAInline_Detail_Data((DateTime)sDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable);
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

        private Base_ViewModel GetCFAInline_Detail_Data(DateTime sdate)
        {
            string sqlcmd = $@" 
			declare @sDate varchar(20) = '{sdate.ToString("yyyy/MM/dd")}'
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
			from Production.dbo.Cfa a WITH (NOLOCK) 
			inner join Production.dbo.Cfa_Detail b WITH (NOLOCK) on b.id = a.ID 
			inner join Production.dbo.orders c WITH (NOLOCK) on c.id = a.OrderID
			inner JOIN Production.dbo.Country ct WITH (NOLOCK) ON ct.ID=c.Dest
			outer apply(select Description from Production.dbo.GarmentDefectCode a WITH(NOLOCK) where a.id=b.GarmentDefectCodeID) as gd
			outer apply(select description from Production.dbo.cfaarea a WITH(NOLOCK) where a.id=b.CFAAreaID) as ar
			where a.Status = 'Confirmed' and a.cDate >= @sDate";
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

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
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
                from #Final_P_CFAInline_Detail

                update b set b.TransferDate = getdate() , b.IS_Trans = 1
                from BITableInfo b
                where b.id = 'P_CFAInline_Detail'";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_P_CFAInline_Detail");
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
