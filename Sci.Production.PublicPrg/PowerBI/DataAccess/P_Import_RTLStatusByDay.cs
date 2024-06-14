using Ict;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_RTLStatusByDay
    {
        /// <inheritdoc/>
        public P_Import_RTLStatusByDay()
        {
            DBProxy.Current.DefaultTimeout = 7200;
        }

        /// <inheritdoc/>
        public Base_ViewModel P_RTLStatusByDay(string factory, DateTime? workDate, int wipDay)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            try
            {
                if (!workDate.HasValue)
                {
                    workDate = DateTime.Now.AddDays(-10);
                }

                Base_ViewModel resultReport = this.GetRTLStatusByDayData(factory, workDate.Value, wipDay);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(resultReport.Dt);
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

        private Base_ViewModel GetRTLStatusByDayData(string factory, DateTime workDate, int wipDay)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                string apiURL = ConfigurationManager.ConnectionStrings["PMSSewingAPIuri"].ConnectionString + "api/WIP/GetWIPDay";
                string para = "FactoryID=" + factory + "&Date=" + workDate.ToString("yyyy/MM/dd") + "&WipDay=" + wipDay.ToString();
                HttpClient client = new HttpClient();
                using (HttpResponseMessage response = client.GetAsync(apiURL + "?" + para).Result)
                {
                    // api 呼叫失敗
                    if (!response.StatusCode.ToString().Equals("OK"))
                    {
                        string msg = "GetWIPDay ApiError：" + response.RequestMessage.ToString();
                        finalResult.Result = new DualResult(false, msg);
                        return finalResult;
                    }

                    string res = response.Content.ReadAsStringAsync().Result;
                    List<RTLStatus> apiResult = JsonConvert.DeserializeObject<List<RTLStatus>>(res);

                    var result

                    //finalResult.Result = new DualResult(true);
                    //finalResult.Dt = dataTable;
                }
            }
            catch (Exception ex)
            {
                string msg = "Error：" + ex.Message;
                finalResult.Result = new DualResult(false, msg);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = @"	
---- 只保留十天內的資料
Delete p
from POWERBIReportData.dbo.P_RTLStatusByDay p

Insert Into POWERBIReportData.dbo.P_RTLStatusByDay ( TransferDate, FactoryID ,CurrentWIPDays )
select TransferDate
	, ISNULL(t.FactoryID, '')
	, ISNULL(t.CurrentWIPDays, '')
from #tmp t 
";
                finalResult = new Base_ViewModel()
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
                };
            }

            return finalResult;
        }
    }
}
