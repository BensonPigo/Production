using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Sci.Production.Prg
{
    /// <summary>
    /// 泛用工具型類別
    /// </summary>
    public class VFP
    {
        /// <summary>
        /// 若傳入物件為null，則回傳空字串，若否則回傳物件的ToString()結果
        /// </summary>
        /// <param name="o">o</param>
        /// <returns>string</returns>
        public static string GetStr(object o)
        {
            return (o == null) ? string.Empty : o.ToString();
        }

        /// <summary>
        ///  強制用 String 的 TrimEnd + IgnoreCase方式作字串比對
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns>bool</returns>
        public static bool EqualString(object a, object b)
        {
            return StringEqual(GetStr(a), GetStr(b));
        }

        /// <summary>
        /// 強制用 String 的 TrimEnd + IgnoreCase方式作字串比對
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns>bool</returns>
        public static bool StringEqual(string a, string b)
        {
            return (a == null || b == null)
                ? (a == b)
                : a.TrimEnd().Equals(b.TrimEnd(), StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 強制用 String 的 TrimEnd + IgnoreCase方式作字串比對
        /// </summary>
        /// <param name="a">a</param>
        /// <param name="b">b</param>
        /// <returns>回傳值小於0 代表param_1 &lt; param_2 ; 0 代表param_1 = param_2 ;大於0 代表param_1 &gt; param_2 ;
        /// </returns>
        public static int StringCompare(object a, object b)
        {
            return StringCompare(a.ToString(), b.ToString());
        }

        /// <inheritdoc />
        public static int StringCompare(string a, string b)
        {
            return string.Compare(a.TrimEnd(), b.TrimEnd(), true);
        }

        /// <inheritdoc />
        public static decimal GetDecimal(object obj)
        {
            return Empty(obj) ? 0 : (decimal)obj;
        }

        /// <inheritdoc />
        public static decimal GetDecimal(decimal obj)
        {
            return obj;
        }

        /// <inheritdoc />
        public static bool EqualDecimal(object a, object b)
        {
            return DecimalEqual(GetDecimal(a), GetDecimal(b));
        }

        /// <inheritdoc />
        public static bool DecimalEqual(decimal a, decimal b)
        {
            return a == b;
        }

        /// <inheritdoc />
        public static int GetInt(object obj)
        {
            return Empty(obj) ? 0 : Convert.ToInt32(obj);
        }

        /// <inheritdoc />
        public static int GetInt(int obj)
        {
            return obj;
        }

        /// <summary>
        /// true if the value IsNullOrWhiteSpace; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(string vars)
        {
            return string.IsNullOrWhiteSpace(vars);
        }

        /// <summary>
        /// true if the value Is 0; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(int vars)
        {
            return vars == 0;
        }

        /// <summary>
        /// true if the value is 0; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(float vars)
        {
            return vars == 0;
        }

        /// <summary>
        /// true if the value is 0; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(double vars)
        {
            return vars == 0;
        }

        /// <summary>
        /// true if the value Is 0; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(long vars)
        {
            return vars == 0;
        }

        /// <summary>
        /// true if the value Is 0; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(decimal vars)
        {
            return vars == 0;
        }

        /// <summary>
        /// true if the value Is 0; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(bool vars)
        {
            return vars == false;
        }

        /// <summary>
        /// true if the value Is Null ; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(DateTime vars)
        {
            return vars == null;
        }

        /// <summary>
        /// true if the Object Is Null ; otherwise, false.
        /// </summary>
        /// <inheritdoc />
        public static bool Empty(object vars)
        {
            if (vars == null)
            {
                return true;
            }

            switch (Type.GetTypeCode(vars.GetType()))
            {
                case TypeCode.String:
                    return Empty(vars.ToString());
                case TypeCode.Decimal:
                    return Empty((decimal)vars);
                case TypeCode.Boolean:
                    return Empty((bool)vars);
                case TypeCode.DateTime:
                    return Empty((DateTime)vars);
                case TypeCode.DBNull:
                    return Empty((DBNull)vars);
                case TypeCode.Int32:
                    return Empty((int)vars);
                case TypeCode.Double:
                    return Empty((double)vars);
            }

            return false;
        }

        /// <inheritdoc />
        public static bool Empty(DBNull vars)
        {
            return true;
        }

        /// <inheritdoc />
        public static string GetID_YYYYMM_5(string prefix, string tableName, DateTime refDate, string columnName)
        {
            return MyUtility.GetValue.GetID(prefix, tableName, refDate, (int)MyUtility.IDFormat.AyyyyMMxxxx, columnName, null, 1, 5);
        }

        /// <inheritdoc />
        public static string GetID_YYYYMM_5(string prefix, string tableName, DateTime refDate, string columnName, SqlConnection conn)
        {
            return MyUtility.GetValue
                .GetIDByConnection(prefix, tableName, refDate, (int)MyUtility.IDFormat.AyyyyMMxxxx, columnName, conn, 1, 5);
        }

        /// <summary>
        /// 此產生ID之後 , 直接將ID塞入 Table / #Table / @Table 中
        /// </summary>
        /// <inheritdoc />
        public static string CreateID_YYYYMM_5(string prefix, string tableName, DateTime refDate, string columnName, bool useTransaction = true, SqlConnection conn = null, bool needInsert = false)
        {
            TransactionScope scope = null;
            if (useTransaction)
            {
                scope = new TransactionScope();
            }

            if (conn == null)
            {
                SQL.GetConnection(out conn);
            }

            string id = null;
            bool create_ok = false;
            id = MyUtility.GetValue
                .GetIDByConnection(prefix, tableName, refDate, (int)MyUtility.IDFormat.AyyyyMMxxxx, columnName, conn, 1, 5);
            string sqlCmd = string.Format("insert into {0} ({1}) values ('{2}')", tableName, columnName, id);
            DualResult result;
            if (needInsert && !(result = DBProxy.Current.ExecuteByConn(conn, sqlCmd)))
            {
                if (useTransaction)
                {
                    scope.Dispose();
                    scope = null;
                }

                throw result.GetException();
            }
            else
            {
                create_ok = true;
            }

            if (useTransaction)
            {
                if (create_ok)
                {
                    scope.Complete();
                }

                scope.Dispose();
                scope = null;
            }

            return id;
        }
    }
}
