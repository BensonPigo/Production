using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using Ict;
using Sci.Data;
namespace Sci.Trade.Report.GSchemas
{
    public class Helper
    {
        public Helper()
        {
        }

        public string ConnectionName { get; set; }

        IDictionary<string, DateTime?> _GetShip_Detail_MinETA = new Dictionary<string, DateTime?>();
        public DualResult GetShip_Detail_MinETA(string poid, string seq1, string seq2, string potype, out DateTime? eta)
        {
            eta = null;
            DualResult result;

            string key = poid + "|" + seq1 + "|" + seq2 + "|" + potype;
            if (_GetShip_Detail_MinETA.TryGetValue(key, out eta)) return Result.True;

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("poid",poid),
                new SqlParameter("seq1",seq1),
                new SqlParameter("seq2",seq2),
                new SqlParameter("potype",potype),
            };
            string cmdtext = "SELECT * FROM GetShip_Detail_MinETA(@poid,@seq1,@seq2,@potype)";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out datas))) return result;

            if (0 < datas.Rows.Count && 0 < datas.Columns.Count)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (DBNull.Value != v)
                {
                    if (v is DateTime) eta = (DateTime)v;
                }
            }

            _GetShip_Detail_MinETA.Add(key, eta);
            return Result.True;
        }
        IDictionary<string, IDictionary<string, decimal?>> _GetCurrencyRate = new Dictionary<string, IDictionary<string, decimal?>>();
        public DualResult GetCurrencyRate(string ExchangeTypeID, string FromCurrency, string ToCurrency, DateTime? Date, out decimal? rate, out decimal? Exact)
        {
            rate = null;
            Exact = null;
            IDictionary<string, decimal?> _CurrencyRate = new Dictionary<string, decimal?>();
            DualResult result;

            string key = ExchangeTypeID + "|" + FromCurrency + "|" + ToCurrency + "|" + Date.ToString();
            if (_GetCurrencyRate.TryGetValue(key, out _CurrencyRate))
            {
                rate = _CurrencyRate["A"];
                Exact = _CurrencyRate["B"];
                return Result.True;
            }
            else
            {
                _CurrencyRate = new Dictionary<string, decimal?>();
            }

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ExchangeTypeID",ExchangeTypeID),
                new SqlParameter("FromCurrency",FromCurrency),
                new SqlParameter("ToCurrency",ToCurrency),
                new SqlParameter("Date",Date),
            };
            string cmdtext = "SELECT * FROM GetCurrencyRate(@ExchangeTypeID,@FromCurrency,@ToCurrency,@Date)";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out datas))) return result;

            if (0 < datas.Rows.Count && 0 < datas.Columns.Count)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                var v2 = data[datas.Columns[1]];
                if (DBNull.Value != v)
                {
                    rate = (decimal)v;
                }
                if (DBNull.Value != v2)
                {
                    Exact = (decimal)v2;
                }
            }
            if(rate !=null)_CurrencyRate.Add("A", rate);
            if (Exact != null) _CurrencyRate.Add("B", Exact);
            _GetCurrencyRate.Add(key, _CurrencyRate);
            return Result.True;
        }

        IDictionary<string, Decimal?> _GetPoAmount = new Dictionary<string, Decimal?>();
        public DualResult GetPoAmount(string id, out decimal? PoAmount)
        {
            PoAmount = null;
            DualResult result;
            //id = "11090110GGS";
            string key = id;
            if (_GetPoAmount.TryGetValue(key, out PoAmount)) return Result.True;

            var paras = new List<SqlParameter>()
            {
                new SqlParameter("OrderID",id),
            };
            string cmdtext = "SELECT  GetPoAmount(@OrderID)";
            cmdtext = "SELECT  dbo.GetPoAmount(@OrderID) as amt ,StandardTms from TradeSystem ";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out datas))) return result;
            
            if (0 < datas.Rows.Count && 0 < datas.Columns.Count)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (DBNull.Value != v)
                {
                    if (v is decimal) PoAmount = (decimal)v;
                }
            }
            PoAmount = 1;
            _GetPoAmount.Add(key, PoAmount);
            return Result.True;
        }
        IDictionary<string, DateTime?> _GetOrderSciDate = new Dictionary<string, DateTime?>();
        public DualResult GetOrderSciDate(string type, string id, out DateTime? sValue)
        {
            sValue = null;
            DualResult result;
            //id = "11090110GGS";
            string key = type+id;
            if (_GetOrderSciDate.TryGetValue(key, out sValue)) return Result.True;

            var paras = new List<SqlParameter>()
            {
               new SqlParameter("Type",type),
               new SqlParameter("OrderID",id),
            };
            string cmdtext = "SELECT  GetOrderSciDate(@Type,@OrderID)";
            cmdtext = "SELECT  dbo.GetOrderSciDate(@Type,@OrderID) as value  from TradeSystem ";

            DataTable datas;
            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out datas))) return result;

            if (0 < datas.Rows.Count && 0 < datas.Columns.Count)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (DBNull.Value != v)
                {
                    if (v is DateTime) sValue = (DateTime)v;
                }
            }
            _GetOrderSciDate.Add(key, sValue);
            return Result.True;
        }

        public DualResult GetStandardTms(out decimal? StandardTms)
        {
            StandardTms = 0;
            DataTable datas;
            DualResult result;
            string cmdtext = "SELECT StandardTms FROM TradeSystem";

            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, out datas))) return result;
            if (0 < datas.Rows.Count && 0 < datas.Columns.Count)
            {
                var data = datas.Rows[0];
                var v = data[datas.Columns[0]];
                if (DBNull.Value != v)
                {
                    StandardTms = (decimal)v;
                }
            }

            return Result.True;
        }
        
        public DualResult GetAmountByUnit(decimal _price, decimal _Qty, string _Unit, decimal _Rate, out decimal? _Amount)
        {
            _Amount = null;
            DualResult result;                        
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("PRICE",_price),
                new SqlParameter("QTY",_Qty),
                new SqlParameter("UNIT",_Unit),
                new SqlParameter("RATE",_Rate),
            };
            string cmdtext = "SELECT  * from GetAmountByUnit(@PRICE, @QTY, @UNIT, @RATE)  ";

            DataTable _dtData;
            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            if (_dtData != null && 0 < _dtData.Rows.Count && 0 < _dtData.Columns.Count)
            {
                if (_dtData.Rows[0]["Amount"].ToString() != "")
                    _Amount = Convert.ToDecimal(_dtData.Rows[0]["Amount"].ToString());
            }
            return Result.True;
        }

        public DualResult CalculateCost(string _ExchangeTypeID, string _POID, string _SeasonID, int _IsCostError, out  DataTable[] _dtData)
        {
            _dtData = null;
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ExchangeTypeID", _ExchangeTypeID),
                new SqlParameter("POID", _POID),
                new SqlParameter("SeasonID", _SeasonID),
                new SqlParameter("IsCostError", _IsCostError),
            };
            //string cmdtext = "Execute CalculateCost @ExchangeTypeID, @POID, @SeasonID, @IsCostError  ";
            if (!(result = DBProxy.Current.SelectSP(ConnectionName, "CalculateCost", paras, out _dtData)))
            {
                System.Diagnostics.Debug.WriteLine("_POID :" + _POID);
                return result;
            }

            return Result.True;
        }

        public DualResult GetPulloutData(string _ReturnType, string _cOrderID, out  DataTable[] _dtData)
        {
            _dtData = null;
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ReturnType", _ReturnType),
                new SqlParameter("@cOrderID", _cOrderID),
            };

            if (!(result = DBProxy.Current.SelectSP(ConnectionName, "GetPulloutData", paras, out _dtData))) return result;

            return Result.True;
        }
        public DualResult getTradeSystem(out  DataTable _dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
            };
            string cmdtext = "SELECT * FROM TradeSystem";

            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            return Result.True;
        }
        public DualResult getProductionSystem(out  DataTable _dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
            };
            string cmdtext = "SELECT * FROM dbo.System";

            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            return Result.True;
        }
        public DualResult getSCI(string _ID, string _Category, out  DataTable _dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID",_ID),
                new SqlParameter("Category",_Category)
            };
            string cmdtext = "SELECT  * from getSCI(@ID,@Category)  ";

            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            return Result.True;
        }
        public DualResult getship_Order(string _ID, out  DataTable _dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID",_ID)
            };
            string cmdtext = "SELECT  * from getship_Order(@ID)  ";

            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            return Result.True;
        }
        public DualResult GetDescMtl_ByPoDetail(string _ID, string _SEQ1, string _SEQ2, out  DataTable _dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID",_ID),
                new SqlParameter("SEQ1",_SEQ1),
                new SqlParameter("SEQ2",_SEQ2)
            };
            string cmdtext = "SELECT  * from GetDescMtl_ByPoDetail(@ID,@SEQ1,@SEQ2)  ";

            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            return Result.True;
        }
 
        public DualResult GetApQty_AllStatus(string _ID, string _Seq1, string _Seq2, string _Exclude, out  DataTable _dtData)
        {
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("ID",_ID),
                new SqlParameter("Seq1",_Seq1),
                new SqlParameter("Seq2",_Seq2),
                new SqlParameter("Exclude",_Exclude)
            };
            string cmdtext = "SELECT  * from GetApQty_AllStatus(@ID,@Seq1,@Seq2,@Exclude)  ";

            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            return Result.True;
        }
       public DualResult GetCPURate(string _OrderTypeID, string _ProgramID, string _Category, string _BrandID, decimal? _SampleRate, string _TableName , out decimal? _CpuRate)
        {
            _CpuRate = null;
            DualResult result;
            var paras = new List<SqlParameter>()
            {
                new SqlParameter("OrderTypeID",_OrderTypeID),
                new SqlParameter("ProgramID", _ProgramID),
                new SqlParameter("Category",_Category),
                new SqlParameter("BrandID",_BrandID),
                new SqlParameter("SampleRate",_SampleRate),
                new SqlParameter("TableName",_TableName),
            };
            string cmdtext = "SELECT  * from GetCPURate(@OrderTypeID, @ProgramID, @Category, @BrandID, @SampleRate, @TableName)  ";

            DataTable _dtData;
            if (!(result = DBProxy.Current.Select(ConnectionName, cmdtext, paras, out _dtData))) return result;

            if (_dtData != null && 0 < _dtData.Rows.Count && 0 < _dtData.Columns.Count)
            {
                if (_dtData.Rows[0]["CpuRate"].ToString() != "")
                    _CpuRate = Convert.ToDecimal(_dtData.Rows[0]["CpuRate"].ToString());
            }
            return Result.True;
        }
    }
}
