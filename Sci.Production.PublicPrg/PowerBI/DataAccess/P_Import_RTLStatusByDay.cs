using Ict;
using Newtonsoft.Json;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_RTLStatusByDay
    {
        /// <summary>
        /// 資料來源Dashbaord - RTL Status，詳細說明請見ISP20240466
        /// </summary>
        public P_Import_RTLStatusByDay()
        {
            DBProxy.Current.DefaultTimeout = 7200;
            this.BiDt = null;
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                        new DataColumn("TransferDate", typeof(DateTime)),
                        new DataColumn("FactoryID", typeof(string)),
                        new DataColumn("CurrentWIPDays", typeof(decimal)),
            });
            return dt;
        }

        private DataTable _BiDt;

        /// <summary>
        /// 存放所有API回傳結果
        /// </summary>
        public DataTable BiDt
        {
            get => this._BiDt ?? (this._BiDt = this.CreateDataTable());
            set => this._BiDt = value;
        }

        /// <summary>
        /// BI資料表 P_RTLStatusByDay 寫入，只保留十天份的資料
        /// </summary>
        /// <param name="inputkDate">inputkDate</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel P_RTLStatusByDay(DateTime? inputkDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel()
            {
                Dt = this.CreateDataTable(),
            };

            try
            {
                finalResult = this.CreateTaskAPI(inputkDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                if (finalResult.Dt == null || finalResult.Dt.Rows.Count == 0)
                {
                    throw new Exception("No Data Found");
                }

                finalResult = this.UpdateData(finalResult.Dt);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        /// <summary>
        /// 取得查詢範圍，Call API 取得所有資料，
        /// </summary>
        /// <param name="inputkDate">inputkDate</param>
        /// <returns>None</returns>
        public Base_ViewModel CreateTaskAPI(DateTime? inputkDate)
        {
            Base_ViewModel result = new Base_ViewModel()
            {
                Dt = this.CreateDataTable(),
            };

            if (!inputkDate.HasValue)
            {
                inputkDate = DateTime.Now.AddDays(-10);
            }

            try
            {
                DBProxy.Current.Select("Production", "SELECT ID FROM Factory WITH(NOLOCK) WHERE Junk=0 AND IsProduceFty=1", out DataTable ftyTb);

                string url = MyUtility.GetValue.Lookup(
    $@"
select b.URL 
from System a 
inner join SystemWebAPIURL b on a.RgCode = b.SystemName
where Junk = 0 and Environment = 'Formal'", "Production");

                if (MyUtility.Check.Empty(url))
                {
                    throw new Exception("URL is empty");
                }

                DualResult finalResult = new DualResult(true);

                // 10 days, each Fty
                for (int i = 0; i < 10; i++)
                {
                    DateTime transferDate = inputkDate.Value.AddDays(i);
                    var models = ftyTb.AsEnumerable()
                        .AsParallel()
                        .WithDegreeOfParallelism(3) // 3 thread
                        .Select(row =>
                        {
                            string factoryID = MyUtility.Convert.GetString(row["ID"]);
                            Base_ViewModel model = this.GetDataByAPIAsync(url, factoryID, transferDate, 3);
                            return model;
                        });

                    foreach (var model in models.Where(x => x.Result))
                    {
                        if (model.Dt != null && model.Dt.Rows.Count > 0)
                        {
                            result.Dt.Merge(model.Dt);
                        }
                    }

                    if (models.Where(x => !x.Result).Count() > 0)
                    {
                        finalResult = models.Where(x => !x.Result).Select(x => x.Result).FirstOrDefault();
                        break;
                    }
                }

                result.Result = finalResult;
            }
            catch (Exception ex)
            {
                result.Result = new DualResult(false, ex);
            }

            return result;
        }

        /// <summary>
        /// 非同步Call API，並將回傳資料做整理，整理方式同Dashboard
        /// </summary>
        /// <param name="factory">factory</param>
        /// <param name="workDate">workDate</param>
        /// <param name="wipDay">wipDay</param>
        /// <returns>Base_ViewModel</returns>
        private Base_ViewModel GetDataByAPIAsync(string url, string factory, DateTime workDate, int wipDay)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                string apiURL = $@"{url}api/WIP/GetWIPDay";
                string para = $"FactoryID={factory}&Date={workDate:yyyy/MM/dd}&WipDay={wipDay}";
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = HttpHelpers.GetJsonDataHttpClient(apiURL, para))
                {
                    // api 呼叫失敗
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = "GetWIPDay ApiError：" + response.RequestMessage.ToString();
                        finalResult.Result = new DualResult(false, msg);
                        return finalResult;
                    }

                    string res = response.Content.ReadAsStringAsync().Result;
                    var jsonObject = JsonConvert.DeserializeObject<dynamic>(res);
                    string resultDtJson = JsonConvert.SerializeObject(jsonObject.resultDt);
                    DataTable resultDtJsonDt = JsonConvert.DeserializeObject<DataTable>(resultDtJson);

                    if (resultDtJsonDt == null || resultDtJsonDt.Rows.Count == 0)
                    {
                        finalResult.Result = new DualResult(true);
                        return finalResult;
                    }

                    List<Dashboard_RTLStatus> apiResult = DataTableToList.ConvertToClassList<Dashboard_RTLStatus>(resultDtJsonDt).ToList();

                    var apiJoinOrderIDArtSize = apiResult
                        .GroupBy(x => new { x.Line, x.StyleID })
                        .Select(x => new Dashboard_RTLStatus
                        {
                            Line = x.Key.Line,
                            StyleID = x.Key.StyleID,
                            OrderID = string.Join("/", x.GroupBy(y => new { y.Line, y.StyleID, y.OrderID, y.Article, y.SizeCode }).OrderBy(y => y.Key.OrderID).ThenBy(y => y.Key.Article).ThenBy(y => y.Key.SizeCode).Select(y => y.Key.OrderID)),
                            Article = string.Join("/", x.GroupBy(y => new { y.Line, y.StyleID, y.OrderID, y.Article, y.SizeCode }).OrderBy(y => y.Key.OrderID).ThenBy(y => y.Key.Article).ThenBy(y => y.Key.SizeCode).Select(y => y.Key.Article)),
                            SizeCode = string.Join("/", x.GroupBy(y => new { y.Line, y.StyleID, y.OrderID, y.Article, y.SizeCode }).OrderBy(y => y.Key.OrderID).ThenBy(y => y.Key.Article).ThenBy(y => y.Key.SizeCode).Select(y => y.Key.SizeCode)),
                        }).ToList();

                    var result = apiResult
                        .GroupBy(x => new { x.Line, x.StandardQty, x.WipDays, x.nWipDays, x.StyleID, x.nWipDaysQty, x.WIPQty })
                        .Select(x => new Dashboard_RTLStatus
                        {
                            Line = x.Key.Line,
                            APSNo = string.Join("/", x.Select(y => y.APSNo).Distinct()),
                            OrderID = string.Join("/", x.Select(y => y.OrderID)),
                            Article = string.Join("/", x.Select(y => y.Article)),
                            SizeCode = string.Join("/", x.Select(y => y.SizeCode)),
                            StyleID = x.Key.StyleID,
                            StandardQty = x.Key.StandardQty,
                            LoadingQty = x.Sum(y => y.LoadingQty),
                            SewingLineQty = x.Sum(y => y.SewingLineQty),
                            WIPQty = x.Key.WIPQty,
                            WipDays = x.Key.WipDays,
                            nWipDays = x.Key.nWipDays,
                            nWipDaysQty = x.Key.nWipDaysQty,
                        })
                        .GroupBy(x => new { x.Line, x.StandardQty, x.WipDays, x.nWipDays, x.StyleID, x.nWipDaysQty })
                        .Select(x => new Dashboard_RTLStatus
                        {
                            Line = x.Key.Line,
                            APSNo = string.Join("/", x.Select(y => y.APSNo).Distinct()),
                            OrderID = string.Join("/", x.Select(y => y.OrderID)),
                            Article = string.Join("/", x.Select(y => y.Article)),
                            SizeCode = string.Join("/", x.Select(y => y.SizeCode)),
                            StyleID = x.Key.StyleID,
                            StandardQty = x.Key.StandardQty,
                            LoadingQty = x.Sum(y => y.LoadingQty),
                            SewingLineQty = x.Sum(y => y.SewingLineQty),
                            WIPQty = x.Sum(y => y.WIPQty),
                            WipDays = x.Key.WipDays,
                            nWipDays = x.Key.nWipDays,
                            nWipDaysQty = x.Key.nWipDaysQty,
                        })
                        .GroupBy(x => new { x.Line, x.StyleID })
                        .Select(x => new Dashboard_RTLStatus
                        {
                            Line = x.Key.Line,
                            APSNo = string.Join("/", x.Select(y => y.APSNo)),
                            OrderID = string.Join("/", x.Select(y => y.OrderID)),
                            Article = string.Join("/", x.Select(y => y.Article)),
                            SizeCode = string.Join("/", x.Select(y => y.SizeCode)),
                            StyleIDSimple = x.Key.StyleID.Length > 10 ? x.Key.StyleID.Substring(0, 10) + ".." : x.Key.StyleID,
                            StyleIDTooltip = x.Key.StyleID,
                            StyleID = x.Key.StyleID,
                            StandardQty = x.Sum(y => y.StandardQty),
                            LoadingQty = x.Sum(y => y.LoadingQty),
                            SewingLineQty = x.Sum(y => y.SewingLineQty),
                            WIPQty = x.Sum(y => y.WIPQty),
                            WipDays = x.Sum(y => y.WipDays),
                            nWipDays = x.Sum(y => y.nWipDays),
                            nWipDaysQty = x.Sum(y => y.nWipDaysQty),
                        })
                        .Join(apiJoinOrderIDArtSize, x => new { x.Line, x.StyleID }, y => new { y.Line, y.StyleID }, (x, y) => new Dashboard_RTLStatus
                        {
                            APSNo = x.APSNo,
                            OrderID = y.OrderID,
                            Article = y.Article,
                            SizeCode = y.SizeCode,
                            StyleIDSimple = x.StyleIDSimple,
                            StyleIDTooltip = x.StyleIDTooltip,
                            StyleID = x.StyleID,
                            StandardQty = x.StandardQty,
                            LoadingQty = x.LoadingQty,
                            SewingLineQty = x.SewingLineQty,
                            WIPQty = x.WIPQty,
                            WipDays = x.WipDays,
                            nWipDays = x.nWipDays,
                            nWipDaysQty = x.nWipDaysQty,
                        })
                        .GroupBy(x => new { x.Line, x.APSNo, x.StandardQty, x.WipDays, x.nWipDays, x.StyleID, x.nWipDaysQty, x.WIPQty })
                        .Select(x => new Dashboard_RTLStatus
                        {
                            Line = x.Key.Line,
                            APSNo = x.Key.APSNo.ToString(),
                            StyleID = x.Key.StyleID,
                            StandardQty = x.Key.StandardQty,
                            LoadingQty = x.Sum(y => y.LoadingQty),
                            SewingLineQty = x.Sum(y => y.SewingLineQty),
                            WIPQty = x.Key.WIPQty,
                            WipDays = x.Key.WipDays,
                            nWipDays = x.Key.nWipDays,
                            nWipDaysQty = x.Key.nWipDaysQty,
                        })
                        .ToList();

                    int wIPQty = result.Sum(x => x.WIPQty);
                    int standardQty = result.Sum(x => x.StandardQty);
                    decimal currentWIPDays = standardQty == 0 || wIPQty == 0 ? 0 : (wIPQty / standardQty) + Math.Round(((decimal)wIPQty % standardQty) / standardQty, 2);

                    DataTable dt = this.BiDt.Clone();
                    dt.Rows.Add(workDate, factory, currentWIPDays);
                    finalResult.Dt = dt;
                    finalResult.Result = new DualResult(true);
                }
            }
            catch (Exception ex)
            {
                string msg = "Error：" + ex.Message;
                finalResult.Result = new DualResult(false, msg);
            }

            return finalResult;
        }

        /// <summary>
        /// 保留十天內的資料，全刪除之後再重新轉
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>Base_ViewModel</returns>
        private Base_ViewModel UpdateData(DataTable dt)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sql = @"	
---- 只保留十天內的資料，全刪除之後再重新轉
Delete p
from POWERBIReportData.dbo.P_RTLStatusByDay p


Insert Into POWERBIReportData.dbo.P_RTLStatusByDay ( TransferDate, FactoryID ,CurrentWIPDays )
select TransferDate
	, ISNULL(t.FactoryID, '')
	, ISNULL(t.CurrentWIPDays, 0)
from #tmp t 

if exists (select 1 from BITableInfo b where b.id = 'P_RTLStatusByDay')
begin
    update b
        set b.TransferDate = getdate()
    from BITableInfo b
    where b.id = 'P_RTLStatusByDay'
end
else 
begin
    insert into BITableInfo(Id, TransferDate)
    values('P_RTLStatusByDay', getdate())
end
";
            finalResult = new Base_ViewModel()
            {
                Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
            };

            return finalResult;
        }
    }
}
