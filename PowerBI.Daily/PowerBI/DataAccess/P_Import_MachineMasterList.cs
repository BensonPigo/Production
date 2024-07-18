using Ict;
using PowerBI.Daily.PowerBI.Model;
using PowerBI.Daily.PowerBI.WebApi;
using Sci;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PowerBI.Daily.PowerBI.DataAccess
{
    public class P_Import_MachineMasterList
    {
        private DataTable dtAllData;
        public void P_MachineMasterList() 
        {
            DualResult result = new DualResult(true);

            #region 參數設定
            this.dtAllData = null;
            Dictionary<string, string> dicHeaders = new Dictionary<string, string>();
            dicHeaders.Add("StartMachineID", string.Empty);                                                 // Machine# 開始
            dicHeaders.Add("EndMachineID", string.Empty);                                                   // Machine# 結束
            dicHeaders.Add("MachineBrandID", string.Empty);                                                 // Machine Brand
            dicHeaders.Add("Model", string.Empty);                                                          // Model
            dicHeaders.Add("MachineGroup", string.Empty);                                                   // Macine Group
            dicHeaders.Add("StartSerial", string.Empty);                                                    // Serial 開始
            dicHeaders.Add("EndSerial", string.Empty);                                                      // Serial 結束
            dicHeaders.Add("LocationM", string.Empty);                                                      // Location M
            dicHeaders.Add("StartMachineArrivalDate", string.Empty);                                        // Machine arrival date 開始
            dicHeaders.Add("EndMachineArrivalDate", string.Empty);                                          // Machine arrival date 結束
            dicHeaders.Add("Condition", string.Empty);                                                      // Condition
            dicHeaders.Add("ExcludeDisposedData", "True");                                                  // ExcludeDisposedData
            dicHeaders.Add("IncludeCancelData", "True");                                                    // IncludeCancelData
            dicHeaders.Add("IsBI", "True");                                                                 // 是否BI
            dicHeaders.Add("IsTPE_BI", "True");                                                             // 是否台北BI
            #endregion

            #region 查詢全部伺服器名稱
            DualResult dualResult = DBProxy.Current.Select("PBIReportData", "select [SystemName] = Region from P_TransRegion", out DataTable dtRegion);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }
            #endregion
            
            #region 資料透過API方式撈取
            using (TransactionScope scope = new TransactionScope())
            {
                foreach (DataRow dataRow in dtRegion.Rows)
                {

                    DataTable dt = CallWebAPI.GetWebAPI<Machine_R01>(MyUtility.Convert.GetString(dataRow["SystemName"]), "api/PowerBI/Machine/R01/GetReportData", 600, dicHeaders);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (this.dtAllData == null)
                        {
                            this.dtAllData = dt;
                        }
                        else
                        {
                            this.dtAllData.Merge(dt);
                        }
                    }
                    
                }
                scope.Complete();
            }
            #endregion

            #region 資料新增、刪除至資料庫
            if (this.dtAllData!= null)
            {
                string sql = $@"
                delete p
                from P_MachineMasterList p
                where exists (select 1 from #tmp t where p.Month = t.YYYYMM and p.M = t.LocationM)

                insert into P_MachineMasterList
                (
                     Month
                    ,M
                    ,FTY
                    ,MachineLocationID
                    ,MachineID
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
                result = MyUtility.Tool.ProcessWithDatatable(this.dtAllData, null, sql, out DataTable dataTable, conn: sqlConn);
                if (!result)
                {
                    return;
                }
            }
            #endregion
        }
    }
}
