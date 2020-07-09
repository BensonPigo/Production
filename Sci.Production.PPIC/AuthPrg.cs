using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Check = Sci.MyUtility.Check;
using Msg = Sci.MyUtility.Msg;

namespace Sci.Production.Class.Commons
{
    /// <summary>
    /// AuthPrg
    /// </summary>
    public class AuthPrg
    {
        /// <summary>
        /// Auth_StockObsolescence
        /// </summary>
        public const string Auth_StockObsolescence = "Stock_Obsol";

        // private static String ignoreValue = "!@#$!@#$!#$%!#$%";
        private static Dictionary<string, bool> brandAuths;
        private static Dictionary<string, bool> specialAuths;

        static AuthPrg()
        {
            Reload();
        }

        /// <summary>
        /// Reload
        /// </summary>
        public static void Reload()
        {
            // 重新load user可用的Brands
            brandAuths = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            DataTable brandData;
            string sqlCmd = "select BrandID from dbo.PASS_AuthBrand where id = '" + Env.User.UserID + "'";
            DBProxy.Current.SelectByConn(SQL.queryConn, sqlCmd, out brandData);
            foreach (DataRow row in brandData.Rows)
            {
                brandAuths.Add(row["BrandID"].ToString().TrimEnd(), true);
            }

            // 重新load user有的特殊權限
            specialAuths = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            DataTable specialAuthData;
            string sqlCmd2 = "select AuthID from dbo.PASS_AuthSpecial where id = '" + Env.User.UserID + "' and HasAuth='Y' ";
            DBProxy.Current.Select(null, sqlCmd2, out specialAuthData);
            foreach (DataRow row in specialAuthData.Rows)
            {
                specialAuths.Add(row["AuthID"].ToString().TrimEnd(), true);
            }
        }

        /// <summary>
        /// HasBrandAuth
        /// </summary>
        /// <param name="brandID">string</param>
        /// <returns>bool</returns>
        public static bool HasBrandAuth(string brandID)
        {
            return brandAuths.ContainsKey(brandID.TrimEnd());
        }

        /// <summary>
        /// HasSpecialAuth
        /// </summary>
        /// <param name="authID">string</param>
        /// <returns>bool</returns>
        public static bool HasSpecialAuth(string authID)
        {
            return specialAuths.ContainsKey(authID.TrimEnd());
        }

        /// <summary>
        /// HasHandleOrDeputyAuth_popupWarningBox
        /// </summary>
        /// <param name="handleID">object</param>
        /// <returns>bool</returns>
        public static bool HasHandleOrDeputyAuth_popupWarningBox(object handleID)
        {
            bool hasAuth = HasHandleOrDeputyAuth(handleID);
            if (!hasAuth)
            {
                string handle = handleID == null ? string.Empty : handleID.ToString();
                string userName;
                UserPrg.GetName(handle, out userName, UserPrg.NameType.IdAndName);
                Msg.WarningBox("this is only authorised for [" + userName + "]");
            }

            return hasAuth;
        }

        /// <summary>
        /// 檢查userID是否有
        /// <para>Handle , Handle的代理 , Handle的主管 , Handle的 主管代裡權</para>
        /// </summary>
        /// <param name="handleID">object</param>
        /// <returns>bool</returns>
        public static bool HasHandleOrDeputyAuth(object handleID)
        {
            // null 仍要往下判斷是否有 login user 是否為 admin
            return HasHandleOrDeputyAuth1(handleID == null ? null : handleID.ToString());
        }

        /// <summary>
        /// HasHandleOrDeputyAuth1
        /// </summary>
        /// <param name="handleID">string</param>
        /// <returns>bool</returns>
        public static bool HasHandleOrDeputyAuth1(string handleID)
        {
            if (Env.User.IsAdmin)
            {
                return true;
            }

            if (handleID == null)
            {
                return false;
            }

            string sqlCmd = SqlCmd_hasHandleAuth;

            SqlCommand cmd = new SqlCommand(sqlCmd, SQL.queryConn);
            cmd.Parameters.Add(new SqlParameter("@handle", handleID));
            cmd.Parameters.Add(new SqlParameter("@userid", Env.User.UserID));
            bool hasAuth = false;
            try
            {
                hasAuth = (bool)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Msg.ShowException(null, e);
            }

            return hasAuth;
        }

        /// <summary>
        /// 先列出本人可代理的所有對象後, 再檢查 HandleID 是否在本人可代理對象內
        /// </summary>
        /// <param name="handleID">object</param>
        /// <param name="handleableUsers"><![CDATA[IEnumerable<string>]]></param>
        /// <param name="myID">string</param>
        /// <returns>bool</returns>
        public static bool HasHandleOrDeputyAuth2(object handleID, ref IEnumerable<string> handleableUsers, string myID = null)
        {
            if (Env.User.IsAdmin)
            {
                return true;
            }

            if (handleID == null)
            {
                return false;
            }

            if (handleableUsers == null)
            {
                if (Check.Empty(myID))
                {
                    myID = Env.User.UserID;
                }

                handleableUsers = ListHandleableUserID(myID);
            }

            return handleableUsers.Contains(handleID.ToString().TrimEnd(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 已 userID的角度, 列出此user的userID + 可代理對象 + 組員 + 代理別組主管的組員
        /// </summary>
        /// <param name="userID">string</param>
        /// <returns><![CDATA[IEnumerable<string>]]></returns>
        public static IEnumerable<string> ListHandleableUserID(string userID)
        {
            string sqlCmd = SqlCmd_listHandleableUserID;
            DataTable ids;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@userID", userID));
            if (!SQL.Select(string.Empty, sqlCmd, out ids, pars))
            {
                return new List<string>();
            }

            return ids.AsEnumerable().Select(r => r["ID"].ToString().TrimEnd());

            // return hasAuth;
        }

        /// <summary>
        /// SqlCmd_listHandleableUserID
        /// </summary>
        public const string SqlCmd_listHandleableUserID =
@"
SELECT a.id  --,a.name ,a.Deputy,a.Supervisor,a.Manager
 from dbo.pass1 as a
 left join dbo.pass1 as smr on smr.ID = a.Supervisor
 where a.id = @userid or a.Deputy = @userid or a.Supervisor = @userid or a.Manager = @userid
    or smr.Deputy = @userid or smr.Supervisor = @userid
";

        private const string SqlCmd_hasHandleAuth =
@"
select cast(iif(
		exists(
			 select 1
			 from dbo.pass1 as handle
			 left join pass1 as smr on smr.id = handle.Supervisor
			 where (handle.id = @Handle)
			 and ( handle.ID = @userid or handle.Deputy = @userid or smr.Deputy = @userid or smr.Supervisor = @userid)
		 )
	,1,0) as bit) as HasEditPOAuth 
";

        private const string SqlCmd_hasHandle_O_SMR_Auth =
@"
select cast(iif(
		exists(
			 select 1
			 from dbo.Pass1 as handle
			 left join dbo.Pass1 as smr on smr.id = handle.Supervisor
			 where (handle.id = @Handle or handle.id = @SMR)
			 and ( handle.ID = @userid or handle.Deputy = @userid or smr.Deputy = @userid or smr.Supervisor = @userid)
		 )
	,1,0) as bit) as HasEditPOAuth 
";

        /// <summary>
        /// 檢查userID是否有
        /// <para>Handle , Handle的代理 , Handle的主管 , Handle的 主管代裡權</para>
        /// <para>SMR , SMR的代理 , SMR的主管 , SMR的 主管代裡權</para>
        /// </summary>
        /// <param name="handle">string Handle</param>
        /// <param name="smr">string Smr</param>
        /// <returns>bool</returns>
        public static bool HasHandle_O_Smr_Auth(string handle, string smr)
        {
            if (Check.Empty(handle) && Check.Empty(smr))
            {
                return false;
            }

            if (Env.User.IsAdmin)
            {
                return true;
            }

            string sqlCmd = SqlCmd_hasHandle_O_SMR_Auth;
            SqlCommand cmd = new SqlCommand(sqlCmd, SQL.queryConn);
            cmd.Parameters.Add(new SqlParameter("@userid", Env.User.UserID));
            cmd.Parameters.Add(new SqlParameter("@Smr", smr == null ? string.Empty : smr));
            cmd.Parameters.Add(new SqlParameter("@Handle", handle == null ? string.Empty : handle));
            bool hasAuth = false;
            try
            {
                hasAuth = (bool)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Msg.ShowException(null, e);
            }

            return hasAuth;
        }

        /// <summary>
        /// 檢查userID是否有
        /// <para>POHandle , POHandle的代理 ,PO Handle的主管 , POHandle的 主管代裡權</para>
        /// <para>POSMR , POSMR的代理 , POSMR的主管 , POSMR的 主管代裡權</para>
        /// </summary>
        /// <param name="poid">string</param>
        /// <returns>bool</returns>
        public static bool HasEditPOAuth(string poid)
        {
            if (Check.Empty(poid))
            {
                return false;
            }

            if (Env.User.IsAdmin)
            {
                return true;
            }

            string sqlCmd = @"
declare @Handle varchar(10),
	@SMR varchar(10) 

select 
	@Handle=po1.POHandle, @SMR=po1.POSMR
from dbo.PO as po1
where po1.ID = @poid
" + Environment.NewLine + SqlCmd_hasHandle_O_SMR_Auth;

            SqlCommand cmd = new SqlCommand(sqlCmd, SQL.queryConn);
            cmd.Parameters.Add(new SqlParameter("@userid", Env.User.UserID));
            cmd.Parameters.Add(new SqlParameter("@poid", poid));
            bool hasAuth = false;
            try
            {
                hasAuth = (bool)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Msg.ShowException(null, e);
            }

            return hasAuth;
        }

        /// <summary>
        /// 檢查userID是否有
        /// <para>POHandle , POHandle的代理 ,PO Handle的主管 , POHandle的 主管代裡權</para>
        /// <para>POSMR , POSMR的代理 , POSMR的主管 , POSMR的 主管代裡權</para>
        /// </summary>
        /// <param name="pOHandle">string pOHandle</param>
        /// <param name="pOSmr">string pOSmr</param>
        /// <returns>bool</returns>
        public static bool HasEditPOAuth(string pOHandle, string pOSmr)
        {
            if (Check.Empty(pOHandle) && Check.Empty(pOSmr))
            {
                return false;
            }

            if (Env.User.IsAdmin)
            {
                return true;
            }

            string sqlCmd = SqlCmd_hasHandle_O_SMR_Auth;
            SqlCommand cmd = new SqlCommand(sqlCmd, SQL.queryConn);
            cmd.Parameters.Add(new SqlParameter("@userid", Env.User.UserID));
            cmd.Parameters.Add(new SqlParameter("@Smr", pOSmr == null ? string.Empty : pOSmr));
            cmd.Parameters.Add(new SqlParameter("@Handle", pOHandle == null ? string.Empty : pOHandle));
            bool hasAuth = false;
            try
            {
                hasAuth = (bool)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
                Msg.ShowException(null, e);
            }

            return hasAuth;
        }
    }
}
