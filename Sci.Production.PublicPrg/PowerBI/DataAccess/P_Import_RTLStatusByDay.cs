using Ict;
using Newtonsoft.Json;
using Sci.Data;
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
            Base_ViewModel finalResult = new Base_ViewModel();
            this.BiDt = null;

            try
            {
                this.CreateTaskAPI(inputkDate).GetAwaiter().GetResult();

                if (this.BiDt != null && this.BiDt.Rows.Count > 0)
                {
                    // DB 異動
                    finalResult = this.UpdateData(this.BiDt);
                    if (!finalResult.Result)
                    {
                        return finalResult;
                    }
                }

                finalResult.Result = new Ict.DualResult(true);
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
        public async Task CreateTaskAPI(DateTime? inputkDate)
        {
            if (!inputkDate.HasValue)
            {
                inputkDate = DateTime.Now.AddDays(-10);
            }

            DataTable ftyTb;
            DBProxy.Current.Select("Production", "SELECT ID FROM Factory WITH(NOLOCK) WHERE Junk=0 AND IsProduceFty=1", out ftyTb);

            List<Task<Base_ViewModel>> apiTasks = new List<Task<Base_ViewModel>>();

            // 10 days, each Fty
            for (int i = 0; i < 10; i++)
            {
                DateTime transferDate = inputkDate.Value.AddDays(i);
                foreach (DataRow row in ftyTb.Rows)
                {
                    string factoryID = MyUtility.Convert.GetString(row["ID"]);

                    apiTasks.Add(this.GetDataByAPIAsync(factoryID, transferDate, 3));
                }
            }

            var apiResults = await Task.WhenAll(apiTasks);

            // 將結果保存
            foreach (var apiResult in apiResults)
            {
                if (apiResult != null && apiResult.Dt != null)
                {
                    this.BiDt.Merge(apiResult.Dt);
                }
            }
        }

        /// <summary>
        /// 非同步Call API，並將回傳資料做整理，整理方式同Dashboard
        /// </summary>
        /// <param name="factory">factory</param>
        /// <param name="workDate">workDate</param>
        /// <param name="wipDay">wipDay</param>
        /// <returns>Base_ViewModel</returns>
        private async Task<Base_ViewModel> GetDataByAPIAsync(string factory, DateTime workDate, int wipDay)
        {
            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
                string nowConnection = DBProxy.Current.DefaultModuleName;
                string url = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.EqualString(nowConnection)).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("PMSAPIuri")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();

                if (MyUtility.Check.Empty(url))
                {
                    finalResult.Result = new DualResult(true);
                    return finalResult;
                }

                string apiURL = $@"{url}api/WIP/GetWIPDay";
                string para = "FactoryID=" + factory + "&Date=" + workDate.ToString("yyyy/MM/dd") + "&WipDay=" + wipDay.ToString();
                HttpClient client = new HttpClient();
                using (HttpResponseMessage response = await client.GetAsync(apiURL + "?" + para))
                {
                    // api 呼叫失敗
                    if (!response.IsSuccessStatusCode)
                    {
                        string msg = "GetWIPDay ApiError：" + response.RequestMessage.ToString();
                        finalResult.Result = new DualResult(false, msg);
                        return finalResult;
                    }

                    string res = await response.Content.ReadAsStringAsync();

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
";
            finalResult = new Base_ViewModel()
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
            };

            return finalResult;
        }
    }
}
