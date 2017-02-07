using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Check = Sci.MyUtility.Check;
using Msg = Sci.MyUtility.Msg;

namespace Sci.Production.Class.Commons
{
    public class AuthPrg
    {
        public const String Auth_StockObsolescence = "Stock_Obsol";


        //private static String ignoreValue = "!@#$!@#$!#$%!#$%";
        private static Dictionary<String, bool> brandAuths;
        private static Dictionary<String, bool> specialAuths;
        static AuthPrg() { Reload(); }
        public static void Reload(){
            //  重新load user可用的Brands
            brandAuths = new Dictionary<String, bool>(StringComparer.OrdinalIgnoreCase);
            DataTable brandData;
            String sqlCmd = "select BrandID from dbo.PASS_AuthBrand where id = '" + Env.User.UserID + "'";
            DBProxy.Current.SelectByConn(SQL.queryConn, sqlCmd, out brandData);
            foreach (DataRow row in brandData.Rows)
            {
                brandAuths.Add(row["BrandID"].ToString().TrimEnd(), true);
            }
            //  重新load user有的特殊權限
            specialAuths = new Dictionary<String, bool>(StringComparer.OrdinalIgnoreCase);
            DataTable specialAuthData;
            String sqlCmd2 = "select AuthID from dbo.PASS_AuthSpecial where id = '" + Env.User.UserID + "' and HasAuth='Y' ";
            DBProxy.Current.Select(null, sqlCmd2, out specialAuthData);
            foreach (DataRow row in specialAuthData.Rows)
            {
                specialAuths.Add(row["AuthID"].ToString().TrimEnd(), true);
            }
            
        }
        public static bool hasBrandAuth(String brandID) { return brandAuths.ContainsKey(brandID.TrimEnd()); }
        public static bool hasSpecialAuth(String authID) { return specialAuths.ContainsKey(authID.TrimEnd()); }

        public static bool hasHandleOrDeputyAuth_popupWarningBox(Object handleID) {

            bool hasAuth = AuthPrg.hasHandleOrDeputyAuth(handleID);
            if (!hasAuth) {

                String _handle = handleID == null ? "" : handleID.ToString();
                String userName;
                UserPrg.GetName(_handle, out userName, UserPrg.NameType.idAndName);
                Msg.WarningBox("this is only authorised for [" + userName + "]"); 
            }

            return hasAuth;
        }

        /// <summary>
        /// 檢查userID是否有 
        /// <para>Handle , Handle的代理 , Handle的主管 , Handle的 主管代裡權</para>
        /// </summary>
        /// <param name="handleID"></param>
        /// <returns></returns>
        public static bool hasHandleOrDeputyAuth(Object handleID) { 
            // null 仍要往下判斷是否有 login user 是否為 admin 
            return hasHandleOrDeputyAuth(handleID == null ? null : handleID.ToString()); 
        }        
        public static bool hasHandleOrDeputyAuth(String handleID)
        {
            if (Env.User.IsAdmin) { return true; }
            if (handleID == null) { return false;}

            String sqlCmd = SqlCmd_hasHandleAuth;

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
        /// <param name="handleableUsers"></param>
        /// <param name="handleID"></param>
        /// <param name="myID"></param>
        /// <returns></returns>
        public static bool hasHandleOrDeputyAuth(Object handleID, ref IEnumerable<String> handleableUsers, String myID = null)
        {
            if (Env.User.IsAdmin) { return true; }
            if (handleID == null) { return false; } 
            if (null == handleableUsers){
                if(Check.Empty(myID)){ myID = Env.User.UserID ;}
                handleableUsers = ListHandleableUserID(myID);
            }

            return handleableUsers.Contains(handleID.ToString().TrimEnd(), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 已 userID的角度, 列出此user的userID + 可代理對象 + 組員 + 代理別組主管的組員
        /// </summary>
        /// <param name="userID"></param>
        public static IEnumerable<String> ListHandleableUserID(String userID){
            String sqlCmd = SqlCmd_listHandleableUserID;
            DataTable ids ;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@userID",userID));
            if (!SQL.Select("", sqlCmd, out ids, pars)) { return new List<String>(); }
            return ids.AsEnumerable().Select(r => r["ID"].ToString().TrimEnd());
            //return hasAuth;
        }

        private const String SqlCmd_listHandleableUserID =
@"
SELECT a.id  --,a.name ,a.Deputy,a.Supervisor,a.Manager
 from dbo.pass1 as a
 left join dbo.pass1 as smr on smr.ID = a.Supervisor
 where a.id = @userid or a.Deputy = @userid or a.Supervisor = @userid or a.Manager = @userid
    or smr.Deputy = @userid or smr.Supervisor = @userid
";
        private const String SqlCmd_hasHandleAuth =
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

        private const String SqlCmd_hasHandle_O_SMR_Auth =
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
        /// <param name="Handle"></param>
        /// <param name="Smr"></param>
        /// <returns></returns>
        public static bool HasHandle_O_Smr_Auth(String Handle, String Smr)
        {
            if (Check.Empty(Handle) && Check.Empty(Smr)) { return false; }
            if (Env.User.IsAdmin) { return true; }
            String sqlCmd = SqlCmd_hasHandle_O_SMR_Auth;
            SqlCommand cmd = new SqlCommand(sqlCmd, SQL.queryConn);
            cmd.Parameters.Add(new SqlParameter("@userid", Env.User.UserID));
            cmd.Parameters.Add(new SqlParameter("@Smr", Smr == null ? "" : Smr));
            cmd.Parameters.Add(new SqlParameter("@Handle", Handle == null ? "" : Handle));
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
        /// <param name="poid"></param>
        /// <returns></returns>
        public static bool HasEditPOAuth(String poid){
            if (Check.Empty(poid) ) { return false; }
            if (Env.User.IsAdmin) { return true; }
            String sqlCmd = @"
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
            try{
                hasAuth = (bool)cmd.ExecuteScalar();
            }catch(Exception e){
                Msg.ShowException(null, e);
            }
            return hasAuth;

        }

        /// <summary>
        /// 檢查userID是否有 
        /// <para>POHandle , POHandle的代理 ,PO Handle的主管 , POHandle的 主管代裡權</para>
        /// <para>POSMR , POSMR的代理 , POSMR的主管 , POSMR的 主管代裡權</para>
        /// </summary>
        /// <param name="POHandle"></param>
        /// <param name="POSmr"></param>
        /// <returns></returns>
        public static bool HasEditPOAuth(String POHandle,String POSmr){
            if (Check.Empty(POHandle) && Check.Empty(POSmr)) { return false; }
            if (Env.User.IsAdmin) { return true; }
            String sqlCmd = SqlCmd_hasHandle_O_SMR_Auth;
            SqlCommand cmd = new SqlCommand(sqlCmd, SQL.queryConn);
            cmd.Parameters.Add(new SqlParameter("@userid", Env.User.UserID));
            cmd.Parameters.Add(new SqlParameter("@Smr", POSmr == null ? "" : POSmr));
            cmd.Parameters.Add(new SqlParameter("@Handle", POHandle == null ? "" : POHandle));
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
