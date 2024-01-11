using PostJobLog;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Base
    {
        /// <summary>
        /// Is Test
        /// </summary>
        /// <returns>bool</returns>
        public bool IsTest()
        {
            bool isTest = true;
            if (DBProxy.Current.DefaultModuleName.Contains("Formal"))
            {
                isTest = false;
            }

            return isTest;
        }

        /// <summary>
        /// Call JobLog web api回傳執行結果
        /// </summary>
        /// <param name="job">JobLog</param>
        /// <returns>Job Log Ukey</returns>
        public string CallJobLogApi(JobLog job)
        {
            string ukey = string.Empty;
            JobLog jobLog = new JobLog()
            {
                GroupID = job.GroupID,              // "P"
                SystemID = job.SystemID,            // "Power BI"
                Region = job.Region,
                MDivisionID = job.MDivisionID,
                OperationName = job.OperationName,  // "Import BI Data"
                StartTime = job.StartTime,
                EndTime = job.EndTime,
                Description = job.Description,
                FileName = job.FileName,
                FilePath = job.FilePath,
                Succeeded = job.Succeeded,
            };

            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(this.IsTest());
            ukey = callTPEWebAPI.CreateJobLogAsnc(jobLog, null);
            return ukey;
        }

        /// <summary>
        /// Get System Region
        /// </summary>
        /// <returns>RgCode</returns>
        public string GetRegion()
        {
            string sql = $"select [RgCode] = REPLACE(s.RgCode, 'PHI', 'PH1') from System s";
            return MyUtility.GetValue.Lookup(sql, connectionName: "Production");
        }
    }
}
