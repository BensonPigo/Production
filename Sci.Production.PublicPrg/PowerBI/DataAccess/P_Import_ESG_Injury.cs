using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using Sci.Production.CallPmsAPI.Model;
using Sci.Production.CallPmsAPI;
using Ict;
using Sci.Production.Prg.PowerBI.Logic;
using System.Data.SqlClient;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_ESG_Injury
    {
        /// <inheritdoc/>
        public Base_ViewModel P_ESG_Injury(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Parse(Convert.ToDateTime(sDate).AddDays(1).ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.LoadData(sDate, eDate);
                if (!resultReport.Result)
                {
                    throw finalResult.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        /// <inheritdoc/>
        private Base_ViewModel LoadData(DateTime? sDate, DateTime? eDate)
        {
            OHS_R01 ohs_R01 = new OHS_R01()
            {
                SBIDate = sDate.Value,
                EBIDate = eDate.Value,
                IsBI = true,
            };

            string setRgCode = MyUtility.GetValue.Lookup("select RgCode from system witch(nolock)  ", "Production");
            ResultInfo resultInfo = PackingA2BWebAPI.GetWebAPI<OHS_R01_ViewModel>(setRgCode, "api/PowerBI/OHS/R01/GetReportData", 300, ohs_R01);
            Base_ViewModel resultReport = new Base_ViewModel()
            {
                Result = new DualResult(resultInfo.Result.isSuccess, resultInfo.ErrCode),
                Dt = resultInfo.ResultDT.Empty() ? new DataTable() : CallWebAPI.ToTable<OHS_R01_ViewModel>(resultInfo.ResultDT),
            };
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
                Update p Set 
                 p.[InjuryType]        = ISNULL( t.[InjuryType]       ,'')
                ,p.[CDate]             = t.[CDate]
                ,p.[LossHours]         = ISNULL( t.[LossHours]        ,'')
                ,p.[IncidentType]      = ISNULL( t.[IncidentType]     ,'')
                ,p.[IncidentRemark]    = ISNULL( t.[IncidentRemark]   ,'')
                ,p.[SevereLevel]       = ISNULL( t.[SevereLevel]      ,'')
                ,p.[SevereRemark]      = ISNULL( t.[SevereRemark]     ,'')
                ,p.[CAP]               = ISNULL( t.[CAP]              ,'')
                ,p.[Incharge]          = ISNULL( t.[Incharge]         ,'')
                ,p.[InchargeCheckDate] = t.[InchargeCheckDate]
                ,p.[Approver]          = ISNULL( t.[Approver]         ,'')
                ,p.[ApproveDate]       = t.[ApproveDate]
                ,p.[ProcessDate]       = t.[ProcessDate]
                ,p.[ProcessTime]       = t.[ProcessTime]
                ,p.[ProcessUpdate]     = ISNULL( t.[ProcessUpdate]    ,'')
                ,p.[Status]            = ISNULL( t.[Status]           ,'')
                From P_ESG_Injury p
                inner join #tmp t on p.ID = t.ID and p.FactoryID = t.FactoryID 
                   
                INSERT INTO P_ESG_Injury
                (
	             [ID]
                ,[FactoryID]
                ,[InjuryType]
                ,[CDate]
                ,[LossHours]
                ,[IncidentType]
                ,[IncidentRemark]
                ,[SevereLevel]
                ,[SevereRemark]
                ,[CAP]
                ,[Incharge]
                ,[InchargeCheckDate]
                ,[Approver]
                ,[ApproveDate]
                ,[ProcessDate]
                ,[ProcessTime]
                ,[ProcessUpdate]
                ,[Status]
                )
                SELECT 
                 ISNULL([ID]                 ,'')
                ,ISNULL([FactoryID]          ,'')
                ,ISNULL([InjuryType]         ,'')
                ,[CDate]
                ,ISNULL([LossHours]          ,'')
                ,ISNULL([IncidentType]       ,'')
                ,ISNULL([IncidentRemark]     ,'')
                ,ISNULL([SevereLevel]        ,'')
                ,ISNULL([SevereRemark]       ,'')
                ,ISNULL([CAP]                ,'')
                ,ISNULL([Incharge]           ,'')
                ,[InchargeCheckDate]
                ,ISNULL([Approver]           ,'')
                ,[ApproveDate]
                ,[ProcessDate]
                ,[ProcessTime]
                ,ISNULL([ProcessUpdate]      ,'')
                ,ISNULL([Status]             ,'')
                FROM #TMP T 
                WHERE NOT EXISTS(SELECT 1 FROM P_ESG_Injury P WITH(NOLOCK) WHERE P.ID = T.ID AND P.FactoryID = T.FactoryID)
                ";
                sql += new Base().SqlBITableInfo("P_ESG_Injury", true);
                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn);
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
