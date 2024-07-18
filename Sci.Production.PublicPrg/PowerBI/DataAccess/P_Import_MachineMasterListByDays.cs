using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_MachineMasterListByDays
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_MachineMasterListByDays(DateTime? sDate,DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                DateTime currentDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                DateTime edt = currentDate.AddDays(1);
                eDate = DateTime.Parse(edt.ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.LoadData((DateTime)sDate, (DateTime)eDate);

                DataTable detailTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, (DateTime)sDate);
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

        private Base_ViewModel LoadData(DateTime sDate, DateTime eDate)
        {
            Dictionary<string, string> dicHeaders = new Dictionary<string, string>();
            dicHeaders.Add("StartMachineID", string.Empty);                                             // Machine# 開始
            dicHeaders.Add("EndMachineID", string.Empty);                                               // Machine# 結束
            dicHeaders.Add("MachineBrandID", string.Empty);                                             // Machine Brand
            dicHeaders.Add("Model", string.Empty);                                                      // Model
            dicHeaders.Add("MachineGroup", string.Empty);                                               // Macine Group
            dicHeaders.Add("StartSerial", string.Empty);                                                // Serial 開始
            dicHeaders.Add("EndSerial", string.Empty);                                                  // Serial 結束
            dicHeaders.Add("LocationM", string.Empty);                                                  // Location M
            dicHeaders.Add("StartMachineArrivalDate", string.Empty);                                    // Machine arrival date 開始
            dicHeaders.Add("EndMachineArrivalDate", string.Empty);
            dicHeaders.Add("Condition", string.Empty);                                                  // Condition
            dicHeaders.Add("ExcludeDisposedData", "False");                                             // ExcludeDisposedData
            dicHeaders.Add("IncludeCancelData", "True");                                                // IncludeCancelData
            dicHeaders.Add("sBIDate", sDate.ToString("yyyy/MM/dd"));                                    // BI查詢日期
            dicHeaders.Add("eBIDate", eDate.ToString("yyyy/MM/dd"));                                    // BI查詢日期
            dicHeaders.Add("IsBI", "True");                                                             // 是否BI
            dicHeaders.Add("IsTPE_BI", "False");                                                        // 是否台北BI

            string setRgCode = MyUtility.GetValue.Lookup("select RgCode from system ", "Production");
            Base_ViewModel resultReport = new Base_ViewModel();
            resultReport.Dt = CallWebAPI.GetWebAPI<Machine_R01>(setRgCode, "api/PowerBI/Machine/R01/GetReportData", 300, dicHeaders);
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@Date", sdate));

            using (sqlConn)
            {
                string sql = $@" 
                DELETE P_MachineMasterListByDays
                WHERE Not exists (SELECT 1 FROM #tmp t WITH(NOLOCK) WHERE MachineID = MACHINE) 

                Update p Set 
                 [MachineID]					 = ISNULL(t.[Machine],'')
                ,[M]							 = ISNULL(t.[M],'')
                ,[FTY]							 = ISNULL(t.[FactoryID],'')
                ,[MachineLocationID]			 = ISNULL(t.[Location],'')
                ,[MachineGroup]					 = ISNULL(t.[MachineGroup],'')
                ,[BrandID]						 = ISNULL(t.[BrandID],'')
                ,[BrandName]					 = ISNULL(t.[BrandName],'')
                ,[Model]						 = ISNULL(t.[Model],'')
                ,[SerialNo]						 = ISNULL(t.[SerialNo],'')
                ,[Condition]					 = ISNULL(t.[Condition],'')
                ,[PendingCountryMangerApvDate]	 = ISNULL(t.[PendingCountry],'')
                ,[RepairStartDate]				 = t.[RepairStartDate]
                ,[EstFinishRepairDate]			 = t.[EstFinishRepairDate]
                ,[MachineArrivalDate]			 = t.[MachineArrivalDate]
                ,[TransferDate]					 = t.[TransferDate]
                ,[LendTo]						 = ISNULL(t.[LendTo],'')
                ,[LendDate]						 = t.[LendDate]
                ,[LastEstReturnDate]			 = t.[LastEstReturnDate]
                ,[Remark]						 = ISNULL(t.[Remark],'')
                ,[FAID]							 = ISNULL(t.[FA],'')
                ,[Junk]							 = ISNULL(t.[Junk],'')
                ,[POID]							 = ISNULL(t.[PO],'')
                ,[RefNo]						 = ISNULL(t.[Ref],'')
                From P_MachineMasterListByDays p
                inner join #tmp t on p.MachineID = t.Machine 
                   


                INSERT INTO P_MachineMasterListByDays
                (
	                 [MachineID]
	                ,[M]
	                ,[FTY]
	                ,[MachineLocationID]
	                ,[MachineGroup]
	                ,[BrandID]
	                ,[BrandName]
	                ,[Model]
	                ,[SerialNo]
	                ,[Condition]
	                ,[PendingCountryMangerApvDate]
	                ,[RepairStartDate]
	                ,[EstFinishRepairDate]
	                ,[MachineArrivalDate]
	                ,[TransferDate]
	                ,[LendTo]
	                ,[LendDate]
	                ,[LastEstReturnDate]
	                ,[Remark]
	                ,[FAID]
	                ,[Junk]
	                ,[POID]
	                ,[RefNo]
                )
                SELECT 
                 ISNULL([Machine],'')
                ,ISNULL([M],'')
                ,ISNULL([FactoryID],'')
                ,ISNULL([Location],'')
                ,ISNULL([MachineGroup],'')
                ,ISNULL([BrandID],'')
                ,ISNULL([BrandName],'')
                ,ISNULL([Model],'')
                ,ISNULL([SerialNo],'')
                ,ISNULL([Condition],'')
                ,ISNULL([PendingCountry],'')
                ,[RepairStartDate]
                ,[EstFinishRepairDate]
                ,[MachineArrivalDate]
                ,[TransferDate]
                ,ISNULL([LendTo],'')
                ,[LendDate]
                ,[LastEstReturnDate]
                ,ISNULL([Remark],'')
                ,ISNULL([FA],'')
                ,ISNULL([Junk],'')
                ,ISNULL([PO],'')
                ,ISNULL([Ref],'')
                FROM #TMP T 
                WHERE NOT EXISTS(SELECT 1 FROM P_MachineMasterListByDays P WITH(NOLOCK) WHERE P.MachineID = T.MACHINE)

                IF EXISTS (select 1 from BITableInfo b where b.id = 'P_MachineMasterListByDays')
                BEGIN
	                update BITableInfo set TransferDate = getdate()
	                where ID = 'P_MachineMasterListByDays'
                END
                ELSE 
                BEGIN
	                insert into BITableInfo(Id, TransferDate)
	                values('P_MachineMasterListByDays', getdate())
                END
                ";

                result = MyUtility.Tool.ProcessWithDatatable(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter);
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
