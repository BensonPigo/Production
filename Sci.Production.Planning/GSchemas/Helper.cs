using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Ict;
using Sci.Data;

namespace Sci.Production.Report.GSchemas
{
    /// <summary>
    /// Helper
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// ConnectionName
        /// </summary>
        public string ConnectionName { get; set; }

        private IDictionary<string, DateTime?> _GetShip_Detail_MinETA = new Dictionary<string, DateTime?>();

        /// <summary>
        /// GetShip_Detail_MinETA
        /// </summary>
        /// <param name="poid">poid</param>
        /// <param name="seq1">seq1</param>
        /// <param name="seq2">seq2</param>
        /// <param name="potype">potype</param>
        /// <param name="eta">DateTime</param>
        /// <returns>DualResult</returns>
        public DualResult GetShip_Detail_MinETA(string poid, string seq1, string seq2, string potype, out DateTime? eta)
        {
            eta = null;
            DualResult result;

            string key = poid + "|" + seq1 + "|" + seq2 + "|" + potype;
            if (this._GetShip_Detail_MinETA.TryGetValue(key, out eta))
            {
                return Ict.Result.True;
            }

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("poid", poid),
                new SqlParameter("seq1", seq1),
                new SqlParameter("seq2", seq2),
                new SqlParameter("potype", potype),
            };
            string cmdtext = "SELECT * FROM GetShip_Detail_MinETA(@poid,@seq1,@seq2,@potype)";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out datas)))
            {
                return result;
            }

            if (datas.Rows.Count > 0 && datas.Columns.Count > 0)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (v != DBNull.Value)
                {
                    if (v is DateTime)
                    {
                        eta = (DateTime)v;
                    }
                }
            }

            this._GetShip_Detail_MinETA.Add(key, eta);
            return Ict.Result.True;
        }

        private IDictionary<string, IDictionary<string, decimal?>> _GetCurrencyRate = new Dictionary<string, IDictionary<string, decimal?>>();

        /// <summary>
        /// GetCurrencyRate
        /// </summary>
        /// <param name="exchangeTypeID">ExchangeTypeID</param>
        /// <param name="fromCurrency">FromCurrency</param>
        /// <param name="toCurrency">ToCurrency</param>
        /// <param name="date">Date</param>
        /// <param name="rate">rate</param>
        /// <param name="exact">Exact</param>
        /// <returns>DualResult</returns>
        public DualResult GetCurrencyRate(string exchangeTypeID, string fromCurrency, string toCurrency, DateTime? date, out decimal? rate, out decimal? exact)
        {
            rate = null;
            exact = null;
            IDictionary<string, decimal?> currencyRate = new Dictionary<string, decimal?>();
            DualResult result;

            string key = exchangeTypeID + "|" + fromCurrency + "|" + toCurrency + "|" + date.ToString();
            if (this._GetCurrencyRate.TryGetValue(key, out currencyRate))
            {
                rate = currencyRate["A"];
                exact = currencyRate["B"];
                return Ict.Result.True;
            }
            else
            {
                currencyRate = new Dictionary<string, decimal?>();
            }

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ExchangeTypeID", exchangeTypeID),
                new SqlParameter("FromCurrency", fromCurrency),
                new SqlParameter("ToCurrency", toCurrency),
                new SqlParameter("Date", date),
            };
            string cmdtext = "SELECT * FROM GetCurrencyRate(@ExchangeTypeID,@FromCurrency,@ToCurrency,@Date)";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out datas)))
            {
                return result;
            }

            if (datas.Rows.Count > 0 && datas.Columns.Count > 0)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                var v2 = data[datas.Columns[1]];
                if (v != DBNull.Value)
                {
                    rate = (decimal)v;
                }

                if (v2 != DBNull.Value)
                {
                    exact = (decimal)v2;
                }
            }

            if (rate != null)
            {
                currencyRate.Add("A", rate);
            }

            if (exact != null)
            {
                currencyRate.Add("B", exact);
            }

            this._GetCurrencyRate.Add(key, currencyRate);
            return Ict.Result.True;
        }

        private IDictionary<string, decimal?> _GetPoAmount = new Dictionary<string, decimal?>();

        /// <summary>
        /// GetPoAmount
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="poAmount">PoAmount</param>
        /// <returns>DualResult</returns>
        public DualResult GetPoAmount(string id, out decimal? poAmount)
        {
            poAmount = null;
            DualResult result;
            string key = id;
            if (this._GetPoAmount.TryGetValue(key, out poAmount))
            {
                return Ict.Result.True;
            }

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("OrderID", id),
            };
            string cmdtext = "SELECT  GetPoAmount(@OrderID)";
            cmdtext = "SELECT  dbo.GetPoAmount(@OrderID) as amt ,StandardTms from TradeSystem ";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out datas)))
            {
                return result;
            }

            if (datas.Rows.Count > 0 && datas.Columns.Count > 0)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (v != DBNull.Value)
                {
                    if (v is decimal)
                    {
                        poAmount = (decimal)v;
                    }
                }
            }

            poAmount = 1;
            this._GetPoAmount.Add(key, poAmount);
            return Ict.Result.True;
        }

        private IDictionary<string, DateTime?> _GetOrderSciDate = new Dictionary<string, DateTime?>();

        /// <summary>
        /// GetOrderSciDate
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="id">id</param>
        /// <param name="sValue">sValue</param>
        /// <returns>DualResult</returns>
        public DualResult GetOrderSciDate(string type, string id, out DateTime? sValue)
        {
            sValue = null;
            DualResult result;
            string key = type + id;
            if (this._GetOrderSciDate.TryGetValue(key, out sValue))
            {
                return Ict.Result.True;
            }

            var paras = new List<SqlParameter>()
            {
               new SqlParameter("Type", type),
               new SqlParameter("OrderID", id),
            };
            string cmdtext = "SELECT  GetOrderSciDate(@Type,@OrderID)";
            cmdtext = "SELECT  dbo.GetOrderSciDate(@Type,@OrderID) as value  from TradeSystem ";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out datas)))
            {
                return result;
            }

            if (datas.Rows.Count > 0 && datas.Columns.Count > 0)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (v != DBNull.Value)
                {
                    if (v is DateTime)
                    {
                        sValue = (DateTime)v;
                    }
                }
            }

            this._GetOrderSciDate.Add(key, sValue);
            return Ict.Result.True;
        }

        /// <summary>
        /// GetStandardTms
        /// </summary>
        /// <param name="standardTms">StandardTms</param>
        /// <returns>DualResult</returns>
        public DualResult GetStandardTms(out decimal? standardTms)
        {
            standardTms = 0;
            DataTable datas;
            DualResult result;
            string cmdtext = "SELECT StandardTms FROM TradeSystem";

            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, out datas)))
            {
                return result;
            }

            if (datas.Rows.Count > 0 && datas.Columns.Count > 0)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (v != DBNull.Value)
                {
                    standardTms = (decimal)v;
                }
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// GetAmountByUnit
        /// </summary>
        /// <param name="price">_price</param>
        /// <param name="qty">_Qty</param>
        /// <param name="unit">_Unit</param>
        /// <param name="rate">_Rate</param>
        /// <param name="amount">_Amount</param>
        /// <returns>DualResult</returns>
        public DualResult GetAmountByUnit(decimal price, decimal qty, string unit, decimal rate, out decimal? amount)
        {
            amount = null;
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("PRICE", price),
                new SqlParameter("QTY", qty),
                new SqlParameter("UNIT", unit),
                new SqlParameter("RATE", rate),
            };
            string cmdtext = "SELECT  * from GetAmountByUnit(@PRICE, @QTY, @UNIT, @RATE)  ";

            DataTable dtData;
            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            if (dtData != null && dtData.Rows.Count > 0 && dtData.Columns.Count > 0)
            {
                if (dtData.Rows[0]["Amount"].ToString() != string.Empty)
                {
                    amount = Convert.ToDecimal(dtData.Rows[0]["Amount"].ToString());
                }
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// CalculateCost
        /// </summary>
        /// <param name="exchangeTypeID">_ExchangeTypeID</param>
        /// <param name="pOID">_POID</param>
        /// <param name="seasonID">_SeasonID</param>
        /// <param name="isCostError">_IsCostError</param>
        /// <param name="dtData">_dtData</param>
        /// <returns>DualResult</returns>
        public DualResult CalculateCost(string exchangeTypeID, string pOID, string seasonID, int isCostError, out DataTable[] dtData)
        {
            dtData = null;
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ExchangeTypeID", exchangeTypeID),
                new SqlParameter("POID", pOID),
                new SqlParameter("SeasonID", seasonID),
                new SqlParameter("IsCostError", isCostError),
            };
            if (!(result = DBProxy.Current.SelectSP(this.ConnectionName, "CalculateCost", paras, out dtData)))
            {
                System.Diagnostics.Debug.WriteLine("_POID :" + pOID);
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// GetPulloutData
        /// </summary>
        /// <param name="returnType">returnType</param>
        /// <param name="cOrderID">cOrderID</param>
        /// <param name="dtData">dtData</param>
        /// <returns>DualResult</returns>
        public DualResult GetPulloutData(string returnType, string cOrderID, out DataTable[] dtData)
        {
            dtData = null;
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ReturnType", returnType),
                new SqlParameter("@cOrderID", cOrderID),
            };

            if (!(result = DBProxy.Current.SelectSP(this.ConnectionName, "GetPulloutData", paras, out dtData)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// GetTradeSystem
        /// </summary>
        /// <param name="dtData">dtData</param>
        /// <returns>DualResult</returns>
        public DualResult GetTradeSystem(out DataTable dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
            };
            string cmdtext = "SELECT * FROM TradeSystem";

            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// getProductionSystem
        /// </summary>
        /// <param name="dtData">dtData</param>
        /// <returns>DualResult</returns>
        public DualResult GetProductionSystem(out DataTable dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
            };
            string cmdtext = "SELECT * FROM dbo.System";

            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// getSCI
        /// </summary>
        /// <param name="iD">iD</param>
        /// <param name="category">category</param>
        /// <param name="dtData">dtData</param>
        /// <returns>DualResult</returns>
        public DualResult GetSCI(string iD, string category, out DataTable dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID", iD),
                new SqlParameter("Category", category),
            };
            string cmdtext = "SELECT  * from getSCI(@ID,@Category)  ";

            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// Getship_Order
        /// </summary>
        /// <param name="iD">iD</param>
        /// <param name="dtData">dtData</param>
        /// <returns>DualResult</returns>
        public DualResult Getship_Order(string iD, out DataTable dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID", iD),
            };
            string cmdtext = "SELECT  * from getship_Order(@ID)  ";

            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// GetDescMtl_ByPoDetail
        /// </summary>
        /// <param name="iD">iD</param>
        /// <param name="sEQ1">sEQ1</param>
        /// <param name="sEQ2">sEQ2</param>
        /// <param name="dtData">dtData</param>
        /// <returns>DualResult</returns>
        public DualResult GetDescMtl_ByPoDetail(string iD, string sEQ1, string sEQ2, out DataTable dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID", iD),
                new SqlParameter("SEQ1", sEQ1),
                new SqlParameter("SEQ2", sEQ2),
            };
            string cmdtext = "SELECT  * from GetDescMtl_ByPoDetail(@ID,@SEQ1,@SEQ2)  ";

            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// GetApQty_AllStatus
        /// </summary>
        /// <param name="iD">iD</param>
        /// <param name="seq1">seq1</param>
        /// <param name="seq2">seq2</param>
        /// <param name="exclude">exclude</param>
        /// <param name="dtData">dtData</param>
        /// <returns>DualResult</returns>
        public DualResult GetApQty_AllStatus(string iD, string seq1, string seq2, string exclude, out DataTable dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID", iD),
                new SqlParameter("Seq1", seq1),
                new SqlParameter("Seq2", seq2),
                new SqlParameter("Exclude", exclude),
            };
            string cmdtext = "SELECT  * from GetApQty_AllStatus(@ID,@Seq1,@Seq2,@Exclude)  ";

            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            return Ict.Result.True;
        }

        /// <summary>
        /// GetCPURate
        /// </summary>
        /// <param name="orderTypeID">orderTypeID</param>
        /// <param name="programID">programID</param>
        /// <param name="category">category</param>
        /// <param name="brandID">brandID</param>
        /// <param name="sampleRate">sampleRate</param>
        /// <param name="tableName">tableName</param>
        /// <param name="cpuRate">cpuRate</param>
        /// <returns>DualResult</returns>
        public DualResult GetCPURate(string orderTypeID, string programID, string category, string brandID, decimal? sampleRate, string tableName, out decimal? cpuRate)
        {
            cpuRate = null;
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("OrderTypeID", orderTypeID),
                new SqlParameter("ProgramID", programID),
                new SqlParameter("Category", category),
                new SqlParameter("BrandID", brandID),
                new SqlParameter("SampleRate", sampleRate),
                new SqlParameter("TableName", tableName),
            };
            string cmdtext = "SELECT  * from GetCPURate(@OrderTypeID, @ProgramID, @Category, @BrandID, @SampleRate, @TableName)  ";

            DataTable dtData;
            if (!(result = DBProxy.Current.Select(this.ConnectionName, cmdtext, paras, out dtData)))
            {
                return result;
            }

            if (dtData != null && dtData.Rows.Count > 0 && dtData.Columns.Count > 0)
            {
                if (dtData.Rows[0]["CpuRate"].ToString() != string.Empty)
                {
                    cpuRate = Convert.ToDecimal(dtData.Rows[0]["CpuRate"].ToString());
                }
            }

            return Ict.Result.True;
        }
    }
}
