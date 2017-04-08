using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//請有興趣使用但是目前沒有那個領域的控制項，而想加新東西的人；
//或是目前有那個領域東西，但是一些通用行為不支援的情況
//找Evaon，不要自己加，感謝
namespace Sci.Production.Class
{    
    /// <summary>
    /// 延伸方法
    /// </summary>
    public static class Extension
    {
        private static SqlConnection ObtainSqlConnection(Sci.Data.IDBProxy proxy)
        {
            SqlConnection cn;
            if (proxy.OpenConnection("", out cn) == false)
            {
                throw new ApplicationException("can't open Connection");
            }
            return cn;
        }

        /// <summary>
        /// <para>透過原本的Select做查詢把結果包成DualDisposableResult(DataTable)</para>
        /// <para>使用ExtendedData取出結果資料表。會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DualDisposableResult<DataTable> SelectEx(this Sci.Data.IDBProxy proxy, string sql, params object[] args)
        {
            return SelectEx(proxy, sql, false, args);
        }

        /// <summary>
        /// <para>透過原本的Select做查詢把結果包成DualDisposableResult(DataTable)</para>
        /// <para>使用ExtendedData取出結果資料表。會把傳入的Args兩兩一組的轉為SqlParameter，所以如果傳入的Args不是偶數會跳Exeception</para>
        /// </summary>
        /// <param name="proxy"></param>
        /// <param name="sql"></param>
        /// <param name="withSchema"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static DualDisposableResult<DataTable> SelectEx(this Sci.Data.IDBProxy proxy, string sql, bool withSchema, params object[] args)
        {
            using (var cn = ObtainSqlConnection(proxy))
            {
                DualDisposableResult<DataTable> dr;
                var ps = new List<SqlParameter>();
                var argsClone = args.ToList();
                while (argsClone.Any())
                {
                    object v = argsClone[1];
                    ps.Add(new SqlParameter((string)argsClone[0], v == null ? DBNull.Value : v));
                    argsClone = argsClone.Skip(2).ToList();
                }
                using (var adapter = new SqlDataAdapter(sql, cn))
                {
                    adapter.SelectCommand.Parameters.AddRange(ps.ToArray());

                    var dt = new DataTable();
                    try
                    {
                        adapter.Fill(dt);
                        if (withSchema)
                            adapter.FillSchema(dt, SchemaType.Source);
                        dr = new DualDisposableResult<DataTable>(new DualResult(true));
                        dr.ExtendedData = dt;
                    }
                    catch (Exception ex)
                    {
                        var mixEx = new AggregateException(ex.Message + "\r\nsql: " + sql, ex);
                        dr = new DualDisposableResult<DataTable>(new DualResult(false, mixEx));
                    }
                }
                return dr;
            }
        }
        
        /// <summary>
        /// 合併DateTime.ToString()的功能
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringEx(this DateTime? date, string format)
        {
            if (date.HasValue)
                return date.Value.ToString(format);
            else
                return null;
        }
    }
    
    /// <summary>
    /// 合併原本的DualResult物件，並且讓裡面含有一個ExtendedData屬性來放置要回傳的主要結果(該結果必須有實作IDisposable)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DualDisposableResult<T> : IDisposable where T : class, IDisposable
    {
        /// <summary>
        /// 是否要隨著Dispose事件，連帶把ExtendedData也呼叫Dispose()，預設為true，代表殼物件Dispoes時，會連帶Dispose ExtendedData
        /// </summary>
        public bool? DisposeExtendedData { get; set; }
        /// <summary>
        /// 主要回傳結果，型態由殼層定義，有實作IDisposable
        /// </summary>
        public T ExtendedData { get; set; }
        /// <summary>
        /// 儲存真正的DualResult，以供呼叫端地取用(例如BaseForm.ShowErr就會使用這個屬性)
        /// </summary>
        public DualResult InnerResult { get; set; }
        /// <summary>
        /// DualDisposableResult
        /// </summary>
        /// <param name="innerResult"></param>
        public DualDisposableResult(DualResult innerResult)
        {
            this.InnerResult = innerResult;
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public string Description
        {
            get
            {
                return this.InnerResult.Description;
            }
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
        public Exception GetException()
        {
            return this.InnerResult.GetException();
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.InnerResult.IsEmpty;
            }
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public BaseResult.MessageInfos Messages
        {
            get
            {
                return this.InnerResult.Messages;
            }
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public IResult Parent
        {
            get
            {
                return this.InnerResult.Parent;
            }
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public bool Result
        {
            get
            {
                return this.InnerResult.Result;
            }
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public int StatusCode
        {
            get
            {
                return this.InnerResult.StatusCode;
            }
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public string StatusDesc
        {
            get
            {
                return this.InnerResult.StatusDesc;
            }
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
        public BaseResult.MessageInfos ToMessages()
        {
            return this.InnerResult.ToMessages();
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
        public string ToSimpleString()
        {
            return this.InnerResult.ToSimpleString();
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.InnerResult.ToString();
        }
        /// <summary>
        /// 轉呼叫InnerResult的同名方法或屬性
        /// </summary>
        public void Dispose()
        {
            if (this.DisposeExtendedData.GetValueOrDefault(true) == true)
            {
                if (this.ExtendedData != null)
                {
                    this.ExtendedData.Dispose();
                    this.ExtendedData = null;
                }
            }
        }
        /// <summary>
        /// 隱含轉換布林結果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static implicit operator bool(DualDisposableResult<T> result)
        {
            return result.Result;
        }
    }
}