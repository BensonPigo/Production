using Ict;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.CallPmsAPI.Model;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static PmsWebApiUtility20.WebApiTool;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_MachineMasterListByDays
    {
        /// <inheritdoc/>
        public Base_ViewModel P_MachineMasterListByDays(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                eDate = DateTime.Parse(Convert.ToDateTime(sDate).AddDays(1).ToString("yyyy/MM/dd"));
            }

            try
            {
                Base_ViewModel resultReport = this.LoadData(sDate, eDate);
                if (resultReport.Result)
                {
                    DataTable detailTable = resultReport.Dt;

                    // insert into PowerBI
                    finalResult = this.UpdateBIData(detailTable);
                    if (!finalResult.Result)
                    {
                        throw finalResult.Result.GetException();
                    }

                    finalResult.Result = new Ict.DualResult(true);
                }
                else
                {
                    finalResult.Result = new Ict.DualResult(false, null, resultReport.Result.ToMessages());
                }

            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel LoadData(DateTime? sDate, DateTime? eDate)
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
                SBIDate = sDate.Value.ToString("yyyy/MM/dd"),
                EBIDate = eDate.Value.ToString("yyyy/MM/dd"),
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

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
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
                ,[BIFactoryID]                   = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
                ,[BIInsertDate] = GETDATE()
                From P_MachineMasterListByDays p
                inner join #tmp t on p.MachineID = t.Machine 

insert into P_MachineMasterListByDays_History
Select Ukey,
       [BIFactoryID] =  (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]),
       [BIInsertDate] = getDate()
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
                ,[BIFactoryID]  = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
                ,[BIInsertDate] = GETDATE()
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

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn);
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
