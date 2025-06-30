using Ict;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.CallPmsAPI.Model;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_MachineMasterListByDays
    {
        /// <inheritdoc/>
        public Base_ViewModel P_MachineMasterListByDays(ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            if (!item.SDate.HasValue)
            {
                item.SDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                item.EDate = DateTime.Parse(Convert.ToDateTime(item.SDate).AddDays(1).ToString("yyyy/MM/dd"));
            }

            try
            {
                finalResult = this.LoadData(item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                DataTable detailTable = finalResult.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, item);
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

        private Base_ViewModel LoadData(ExecutedList item)
        {
            Machine_R01 machine_R01_ViewModel = new Machine_R01()
            {
                StartMachineID = string.Empty,
                EndMachineID = string.Empty,
                MachineBrandID = string.Empty,
                Model = string.Empty,
                MachineGroup = string.Empty,
                StartSerial = string.Empty,
                EndSerial = string.Empty,
                LocationM = string.Empty,
                StartMachineArrivalDate = string.Empty,
                EndMachineArrivalDate = string.Empty,
                Condition = string.Empty,
                ExcludeDisposedData = false,
                IncludeCancelData = true,
                SBIDate = item.SDate.Value.ToString("yyyy/MM/dd"),
                EBIDate = item.EDate.Value.ToString("yyyy/MM/dd"),
                IsBI = true,
                IsTPE_BI = false,
            };

            string setRgCode = MyUtility.GetValue.Lookup("select RgCode from system witch(nolock)  ", "Production");
            ResultInfo resultInfo = PackingA2BWebAPI.GetWebAPI<Machine_R01_Report>(setRgCode, "api/PowerBI/Machine/R01/GetReportData", 300, machine_R01_ViewModel);
            Base_ViewModel resultReport = new Base_ViewModel()
            {
                Result = new DualResult(resultInfo.Result.isSuccess, resultInfo.ErrCode),
                Dt = resultInfo.ResultDT.Empty() ? new DataTable() : CallWebAPI.ToTable<Machine_R01_Report>(resultInfo.ResultDT),
            };
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = $@" 
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
                ,[BIFactoryID]                   = @BIFactoryID
                ,[BIInsertDate]                  = GETDATE()
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
                    ,[BIFactoryID]
                    ,[BIInsertDate]
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
                    ,@BIFactoryID
                    ,GETDATE()
                FROM #TMP T 
                WHERE NOT EXISTS(SELECT 1 FROM P_MachineMasterListByDays P WITH(NOLOCK) WHERE P.MachineID = T.MACHINE)
                ";

                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: sqlParameters);
            }

            return finalResult;
        }
    }
}
