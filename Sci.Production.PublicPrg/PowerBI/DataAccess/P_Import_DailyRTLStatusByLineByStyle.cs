﻿using Ict;
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
    public class P_Import_DailyRTLStatusByLineByStyle
    {
        /// <summary>
        /// 資料來源Dashbaord - RTL Status，詳細說明請見 ISP20240475
        /// </summary>
        public P_Import_DailyRTLStatusByLineByStyle()
        {
            DBProxy.Current.DefaultTimeout = 7200;
            this.BiDt = this.CreateDataTable();
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("TransferDate", typeof(DateTime)),
                new DataColumn("MDivisionID", typeof(string)),
                new DataColumn("FactoryID", typeof(string)),
                new DataColumn("APSNo", typeof(int)),
                new DataColumn("SewingLineID", typeof(string)),
                new DataColumn("BrandID", typeof(string)),
                new DataColumn("SeasonID", typeof(string)),
                new DataColumn("OrderID", typeof(string)),
                new DataColumn("CurrentWIP", typeof(int)),
                new DataColumn("StdQty", typeof(int)),
                new DataColumn("WIP", typeof(decimal)),
                new DataColumn("nWIP", typeof(decimal)),
                new DataColumn("InLine", typeof(DateTime)),
                new DataColumn("OffLine", typeof(DateTime)),
                new DataColumn("NewCdCode", typeof(string)),
                new DataColumn("ProductType", typeof(string)),
                new DataColumn("FabricType", typeof(string)),
                new DataColumn("AlloQty", typeof(int)),
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
        /// BI資料表 P_DailyRTLStatusByLineByStyle 寫入
        /// </summary>
        /// <param name="inputkDate">inputkDate</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel P_DailyRTLStatusByLineByStyle(DateTime? inputkDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            this.BiDt = null;

            try
            {
                this.CreateTaskAPI(inputkDate).Wait();

                if (this.BiDt != null && this.BiDt.Rows.Count > 0)
                {
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

            DBProxy.Current.Select("Production", "SELECT ID FROM Factory WITH(NOLOCK) WHERE Junk=0 AND IsProduceFty=1", out DataTable ftyTb);

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
                if (apiResult?.Dt != null)
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
                string url = docx.Descendants("modules")
                    .Elements()
                    .Where(y => y.FirstAttribute.Value.EqualString(nowConnection))
                    .Descendants("connectionStrings")
                    .Elements()
                    .FirstOrDefault(x => x.FirstAttribute.Value.Contains("PMSSewingAPIuri"))
                    ?.LastAttribute.Value;

                string apiURL = $@"{url}api/WIP/GetWIPDay";
                string para = $"FactoryID={factory}&Date={workDate:yyyy/MM/dd}&WipDay={wipDay}";
                using (HttpClient client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(apiURL + "?" + para))
                {
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

                    var apiResult = DataTableToList.ConvertToClassList<Dashboard_RTLStatus>(resultDtJsonDt).ToList();

                    var apiJoinOrderIDArtSize = apiResult
                        .GroupBy(x => new { x.Line, x.OrderID, x.StyleID })
                        .Select(x => new Dashboard_RTLStatus
                        {
                            Line = x.Key.Line,
                            OrderID = x.Key.OrderID,
                            StyleID = x.Key.StyleID,
                            Article = string.Join("/", x.Select(y => y.Article).Distinct().OrderBy(a => a)),
                            SizeCode = string.Join("/", x.Select(y => y.SizeCode).Distinct().OrderBy(s => s)),
                        }).ToList();

                    var result = apiResult
                        .GroupBy(x => new { x.APSNo })
                        .Select(x => new Dashboard_RTLStatus
                        {
                            APSNo = x.Key.APSNo,
                            Line = x.First().Line,
                            OrderID = x.First().OrderID,
                            WIPQty = x.First().WIPQty,
                            StandardQty = x.First().StandardQty,
                            WipDays = x.First().WipDays,
                            nWipDays = x.First().nWipDays,
                            Inline = x.First().Inline,
                            Offline = x.First().Offline,
                            AlloQty = x.Sum(y => y.AlloQty),
                        })
                        .Join(apiJoinOrderIDArtSize, x => new { x.Line, x.OrderID }, y => new { y.Line, y.OrderID }, (x, y) => new Dashboard_RTLStatus
                        {
                            APSNo = x.APSNo,
                            Line = x.Line,
                            OrderID = y.OrderID,
                            Article = y.Article,
                            SizeCode = y.SizeCode,
                            StandardQty = x.StandardQty,
                            WIPQty = x.WIPQty,
                            WipDays = x.WipDays,
                            nWipDays = x.nWipDays,
                            Inline = x.Inline,
                            Offline = x.Offline,
                            AlloQty = x.AlloQty,
                        }).ToList();

                    DataTable dt = this.BiDt.Clone();
                    foreach (var item in result)
                    {
                        DataRow row = dt.NewRow();
                        row["TransferDate"] = workDate;
                        row["FactoryID"] = factory;
                        row["APSNo"] = item.APSNo;
                        row["SewingLineID"] = item.Line;
                        row["OrderID"] = item.OrderID;
                        row["StdQty"] = item.StandardQty;
                        row["CurrentWIP"] = item.WIPQty;
                        row["WIP"] = item.WipDays;
                        row["nWIP"] = item.nWipDays;
                        row["Inline"] = item.Inline;
                        row["OffLine"] = item.Offline;
                        row["AlloQty"] = item.AlloQty;

                        dt.Rows.Add(row);
                    }

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
        /// 保留十天內的資料
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>Base_ViewModel</returns>
        private Base_ViewModel UpdateData(DataTable dt)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sql = @"
---- UPDATE
UPDATE t
SET 
    t.MDivisionID = f.MDivisionID,
    t.SewingLineID = a.SewingLineID,
    t.BrandID = s.BrandID,
    t.SeasonID = s.SeasonID,
    t.StyleID = o.StyleID,
    t.CurrentWIP = a.CurrentWIP,
    t.StdQty = a.StdQty,
    t.WIP = a.WIP,
    t.nWIP = a.nWIP,
    t.InLine = a.InLine,
    t.OffLine = a.OffLine,
    t.NewCdCode = s.CDCodeNew,
    t.ProductType = ISNULL(r2.Name, ''),
    t.FabricType = ISNULL(r1.Name, ''),
    t.AlloQty = a.AlloQty
FROM POWERBIReportData.dbo.P_DailyRTLStatusByLineByStyle t
INNER JOIN #tmp a ON t.TransferDate = a.TransferDate AND t.FactoryID = a.FactoryID AND t.APSNo = a.APSNo
INNER JOIN MainServer.Production.dbo.Factory f ON f.ID = a.FactoryID
INNER JOIN MainServer.Production.dbo.Orders o ON o.ID = a.OrderID
INNER JOIN MainServer.Production.dbo.Style s ON s.Ukey = o.StyleUkey
LEFT JOIN MainServer.Production.dbo.Reason r1 ON r1.ReasonTypeID = 'Fabric_Kind' AND r1.ID = s.FabricType
LEFT JOIN MainServer.Production.dbo.Reason r2 ON r2.ReasonTypeID = 'Style_Apparel_Type' AND r2.ID = s.ApparelType

---- INSERT
INSERT INTO POWERBIReportData.dbo.P_DailyRTLStatusByLineByStyle
    (TransferDate, MDivisionID, FactoryID, APSNo, SewingLineID, BrandID, SeasonID, StyleID, CurrentWIP, StdQty, WIP, nWIP, InLine, OffLine, NewCdCode, ProductType, FabricType, AlloQty)
SELECT
    a.TransferDate, f.MDivisionID, a.FactoryID, a.APSNo, a.SewingLineID, s.BrandID, s.SeasonID, o.StyleID,
    a.CurrentWIP, a.StdQty, a.WIP, a.nWIP, a.InLine, a.OffLine, s.CDCodeNew, ISNULL(r2.Name, ''), ISNULL(r1.Name, ''), a.AlloQty
FROM #tmp a 
INNER JOIN MainServer.Production.dbo.Factory f ON f.ID = a.FactoryID
INNER JOIN MainServer.Production.dbo.Orders o ON o.ID = a.OrderID
INNER JOIN MainServer.Production.dbo.Style s ON s.Ukey = o.StyleUkey
LEFT JOIN MainServer.Production.dbo.Reason r1 ON r1.ReasonTypeID = 'Fabric_Kind' AND r1.ID = s.FabricType
LEFT JOIN MainServer.Production.dbo.Reason r2 ON r2.ReasonTypeID = 'Style_Apparel_Type' AND r2.ID = s.ApparelType
WHERE NOT EXISTS (
    SELECT 1 FROM POWERBIReportData.dbo.P_DailyRTLStatusByLineByStyle ori 
    WHERE ori.TransferDate = a.TransferDate AND ori.FactoryID = a.FactoryID AND ori.APSNo = a.APSNo
);
";
            finalResult = new Base_ViewModel()
            {
                Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn),
            };

            return finalResult;
        }
    }
}
