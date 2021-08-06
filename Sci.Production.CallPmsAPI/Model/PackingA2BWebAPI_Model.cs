using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.CallPmsAPI
{
    /// <summary>
    /// PackingA2BWebAPI_Model
    /// </summary>
    public static class PackingA2BWebAPI_Model
    {
        /// <summary>
        /// ToDataTable
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="items">items</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// IsNullable
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>bool</returns>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// GetCoreType
        /// </summary>
        /// <param name="t">t</param>
        /// <returns>Type</returns>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static List<SqlPar> ToListSqlPar(this List<SqlParameter> listPar)
        {
            List<SqlPar> resultList = new List<SqlPar>();

            foreach (SqlParameter par in listPar)
            {
                SqlPar addItem = new SqlPar()
                {
                    ParameterName = par.ParameterName,
                    ParameterValue = par.Value,
                    ParameterType = par.Value.GetType().Name
                };
                resultList.Add(addItem);
            }
            return resultList;
        }

        /// <summary>
        /// RegionFactory
        /// </summary>
        public class RegionFactory
        {
            public int Selected { get; set; } = 0;
            public string Factory { get; set; }
        }

        /// <summary>
        /// P05_ImportFromPackingListQuery
        /// </summary>
        public class P05_ImportFromPackingListQuery
        {
            public string ShipModeID;
            public string BrandID;
            public string Dest;
            public string CustCDID;
            public string DateSDPDateFrom;
            public string DateSDPDateTo;
            public string BuyerDeliveryFrom;
            public string BuyerDeliveryTo;
            public string OrderIDFrom;
            public string OrderIDTo;
            public string DateIDD;
            public string MultifactoryFactory;
        }

        public class SqlPar
        {
            public string ParameterName;
            public object ParameterValue;
            public string ParameterType;

            public SqlPar()
            { 
            
            }

            public SqlPar(string parameterName, object parameterValue, string parameterType)
            {
                this.ParameterName = parameterName;
                this.ParameterType = parameterType;
                this.ParameterValue = parameterValue;
            }
        }

        public class DataBySql
        {
            public string SqlString;
            public List<SqlPar> SqlParameter;
            public string TmpTable;
            public string TmpTableName;
            public string TmpCols;
        }

        public class P05_ImportFromPackingListQueryResult
        {
            public int Selected { get; set; } = 0;
            public string ID { get; set; }
            public string Factory { get; set; }
            public string OrderID { get; set; }
            public string IDD { get; set; }
            public string CustCDID { get; set; }
            public DateTime? SDPDate { get; set; }
            public DateTime? BuyerDelivery { get; set; }
            public int ShipQty { get; set; }
            public int CTNQty { get; set; }
            public decimal NW { get; set; }
            public decimal NNW { get; set; }
            public string GMTBookingLock { get; set; }
            public string MDivisionID { get; set; }
            public DateTime? CargoReadyDate { get; set; }
            public DateTime? PulloutDate { get; set; }
            public decimal GW { get; set; }
            public decimal CBM { get; set; }
            public string Status { get; set; }
            public DateTime? InspDate { get; set; }
            public int ClogCTNQty { get; set; }
            public decimal APPBookingVW { get; set; }
            public decimal APPEstAmtVW { get; set; }
        }

        /// <summary>
        /// RegionFactory
        /// </summary>
        public class SeekDataResult
        {
            public bool isExists { get; set; }
            public DataTable resultDt { get; set; }
        }

        public class P05_ImportFromPackingList_CheckSpInDiffPacking
        {
            public string OrderID { get; set; }
            public string ID { get; set; }
        }

        public class P05_DetailData
        {
            public string GMTBookingLock { get; set; }
            public string FactoryID { get; set; }
            public string ID { get; set; }
            public string OrderID { get; set; }
            public string OrderShipmodeSeq { get; set; }
            public string IDD { get; set; }
            public string PONo { get; set; }
            public string AirPPID { get; set; }
            public DateTime? CargoReadyDate { get; set; }
            public DateTime? BuyerDelivery { get; set; }
            public DateTime? SDPDate { get; set; }
            public DateTime? PulloutDate { get; set; }
            public string ShipQty { get; set; }

        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字串</param>
        /// <returns></returns>
        public static string Base64Encrypt(this string input)
        {
            return Base64Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="input">需要加密的字串</param>
        /// <param name="encode">字元編碼</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input, Encoding encode)
        {
            return Convert.ToBase64String(encode.GetBytes(input));
        }

        public static void MergeTo(this DataTable targetDt, ref DataTable sourceDt)
        {
            if (targetDt == null)
            {
                return;
            }

            if (targetDt.Rows.Count == 0)
            {
                return;
            }

            if (sourceDt == null)
            {
                sourceDt = targetDt;
                return;
            }

            if (sourceDt.Rows.Count == 0)
            {
                sourceDt = targetDt;
                return;
            }

            foreach (DataRow dr in targetDt.Rows)
            {
                sourceDt.ImportRow(dr);
            }

            sourceDt.AcceptChanges();
        }

        public static void MergeBySyncColType(this DataTable sourceDt, DataTable targetDt)
        {
            if (targetDt == null)
            {
                return;
            }

            if (targetDt.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow dr in targetDt.Rows)
            {
                sourceDt.ImportRow(dr);
            }

            sourceDt.AcceptChanges();
        }
    }
}
