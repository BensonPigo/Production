using Ict;
using Newtonsoft.Json;
using PowerBI.Daily.PowerBI.Model;
using PowerBI.Daily.PowerBI.WebApi;
using Sci;
using Sci.Data;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace PowerBI.Daily.PowerBI.DataAccess
{
    public class P_Import_MachineMasterList
    {
        public DataTable dtAllData;
        public string Msg;
       
        public void P_MachineMasterList(string SystemName) 
        {
            DualResult result = new DualResult(true);

            #region 參數設定
            this.dtAllData = null;
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
                IsBI = false,
                IsTPE_BI = true,
            };
            #endregion
            
            #region 資料透過API方式撈取

            ResultInfo resultInfo = CallWebAPI.GetWebAPI<Machine_R01_Report>(SystemName, "api/PowerBI/Machine/R01/GetReportData", 600, machine_R01_ViewModel);

            this.dtAllData = resultInfo.ResultDT;

            if (!MyUtility.Check.Empty(resultInfo.Result))
            {
                this.Msg = "WebAPI Error：" + resultInfo.Result; //JsonConvert.DeserializeObject<ResultInfo>(resultInfo.Result).Result;
                return;
            }

            #endregion

            #region 資料新增、刪除至資料庫
            if (this.dtAllData != null)
            {
                string sql = $@"
                delete p
                from P_MachineMasterList p
                where exists (select 1 from #tmp t where p.Month = t.YYYYMM and p.M = t.M)

                insert into P_MachineMasterList
                (
                     Month
                    ,MachineID                    
                    ,M
                    ,FTY
                    ,MachineLocationID
                    ,MachineGroup
                    ,BrandID
                    ,BrandName
                    ,Model
                    ,SerialNo
                    ,Condition
                    ,PendingCountryMangerApvDate
                    ,RepairStartDate
                    ,EstFinishRepairDate
                    ,MachineArrivalDate
                    ,TransferDate
                    ,LendTo
                    ,LendDate
                    ,LastEstReturnDate
                    ,Remark
                    ,FAID
                    ,Junk
                    ,POID
                    ,RefNo
                )
                select 
                 ISNULL([YYYYMM],'')
                ,ISNULL([Machine],'')
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
                from #tmp t
                drop table #tmp

                update b
                 set b.TransferDate = getdate()
                from BITableInfo b
                where b.Id = 'P_MachineMasterList'";
                DBProxy.Current.OpenConnection("PBIReportData", out SqlConnection sqlConn);
                DBProxy.Current.DefaultTimeout = 600;

                result = MyUtility.Tool.ProcessWithDatatable(this.dtAllData, null, sql, out DataTable dataTable, conn: sqlConn);
                if (!result)
                {
                    this.Msg = result.ToSimpleString();
                    return;
                }
            }
            #endregion
        }
    }
}
