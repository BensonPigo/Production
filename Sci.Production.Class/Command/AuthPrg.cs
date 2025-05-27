using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Sci.Data;
using Sci.Production.Class.Command;
using Check = Sci.MyUtility.Check;
using Msg = Sci.MyUtility.Msg;

namespace Sci.Production.Class.Commons
{
    /// <inheritdoc/>
    public class AuthPrg
    {
        private const string SqlCmd_listHandleableUserID =
@"
--Declare @userid varchar(10) = 'S01947';

Declare @dt table (ID varchar(10));

insert into @dt (ID) Values (@userid);

Declare @Supervisor varchar(10);
Declare @Deputy varchar(10);

Select 
	@Supervisor = IsNull(Supervisor, ''),
	@Deputy = IsNull(Deputy, '') 
from Pass1 where ID = @userid;

if (@Supervisor <> '')
	insert into @dt (ID) Values (@Supervisor);
		
--if (@Deputy <> '')
--	insert into @dt (ID) Values (@Deputy);

While (@Supervisor <> '' and @Supervisor <> @userid)
begin
	Declare @tmpID varchar(10) = @Supervisor;
	Select 
		@Supervisor = Supervisor,
		@Deputy = Deputy 
	from Pass1 where ID = @tmpID;

	if (exists(select 1 From @dt where ID = @Supervisor))
	BEGIN
		Select * From @dt;
		return
	end

	if (@Supervisor <> '')
		insert into @dt (ID) Values (@Supervisor);
		
--	if (@Deputy <> '')
--		insert into @dt (ID) Values (@Deputy);
end

Select * From @dt;
";

        private const string SqlCmd_hasHandleAuth =
@"
-- Declare @handle varchar(10) = 'S01947';
-- Declare @userId varchar(10) = 'S00553';

if (@handle = @userId)
begin
    Select Value = Cast(1 as bit)
end

Declare @dt table (ID varchar(10));

Declare @Supervisor varchar(10);
Declare @Deputy varchar(10);

Select 
	@Supervisor = IsNull(Supervisor, ''),
	@Deputy = IsNull(Deputy, '') 
from Pass1 where ID = @handle;

if (@Supervisor <> '')
	insert into @dt (ID) Values (@Supervisor);
		
if (@Deputy <> '')
	insert into @dt (ID) Values (@Deputy);

While (@Supervisor <> '' and @Supervisor <> @handle)
begin
	Declare @tmpID varchar(10) = @Supervisor;
	Select 
		@Supervisor = Supervisor,
		@Deputy = Deputy 
	from Pass1 where ID = @tmpID;

	if (exists(select 1 From @dt where ID = @Supervisor))
	BEGIN
		Select value = Cast(IIF(exists(Select * From @dt Where ID = @userId), 1, 0) as bit);
		return
	end

	if (@Supervisor <> '')
		insert into @dt (ID) Values (@Supervisor);
		
--	if (@Deputy <> '')
--		insert into @dt (ID) Values (@Deputy);
end

Select value = Cast(IIF(exists(Select * From @dt Where ID = @userId), 1, 0) as bit);
";

        private const string SqlCmd_hasHandle_O_SMR_Auth =
@"
select cast(iif(
		exists(
			 select 1
			 from dbo.Pass1 as handle
			 left join dbo.Pass1 as smr on smr.id = handle.Supervisor
			 where (handle.id = @Handle or handle.id = @SMR)
			 and ( handle.ID = @userid or handle.Deputy = @userid or handle.Supervisor = @userid or smr.Deputy = @userid or smr.Supervisor = @userid)
		 )
	,1,0) as bit) as HasEditPOAuth 
";

        /// <inheritdoc/>
        public const string Auth_StockObsolescence = "Stock_Obsol";

        private static Dictionary<string, bool> brandAuths;
        private static Dictionary<string, bool> specialAuths;

        static AuthPrg()
        {
            Reload();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public static bool hasBrandAuth(string brandID)
        {
            return brandAuths.ContainsKey(brandID.TrimEnd());
        }

        /// <inheritdoc/>
        public static bool hasSpecialAuth(string authID)
        {
            return specialAuths.ContainsKey(authID.TrimEnd());
        }

        /// <inheritdoc/>
        public static bool hasHandleOrDeputyAuth_popupWarningBox(object handleID)
        {
            bool hasAuth = hasHandleOrDeputyAuth(handleID);
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
        /// <inheritdoc />
        public static bool hasHandleOrDeputyAuth(object handleID)
        {
            // null 仍要往下判斷是否有 login user 是否為 admin
            return hasHandleOrDeputyAuth(handleID == null ? null : handleID.ToString());
        }

        /// <inheritdoc/>
        public static bool hasHandleOrDeputyAuth(string handleID)
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
        /// <inheritdoc />
        public static bool hasHandleOrDeputyAuth(object handleID, ref IEnumerable<string> handleableUsers, string myID = null)
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
        /// <inheritdoc />
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
        /// 檢查userID是否有
        /// <para>Handle , Handle的代理 , Handle的主管 , Handle的 主管代裡權</para>
        /// <para>SMR , SMR的代理 , SMR的主管 , SMR的 主管代裡權</para>
        /// </summary>
        /// <inheritdoc/>
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
        /// <inheritdoc/>
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
        /// <inheritdoc/>
        public static bool HasEditPOAuth(string poHandle, string poSmr)
        {
            if (Check.Empty(poHandle) && Check.Empty(poSmr))
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
            cmd.Parameters.Add(new SqlParameter("@Smr", poSmr == null ? string.Empty : poSmr));
            cmd.Parameters.Add(new SqlParameter("@Handle", poHandle == null ? string.Empty : poHandle));
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
        /// 檢查使用者是否指定的Position(之一)
        /// </summary>
        /// <inheritdoc/>
        public static bool IsPosition(params long[] positionIds)
        {
            return positionIds.Contains(Env.User.PositionID);
        }

        public static DualResult<long?> GetPositionPKey(string position)
        {
            var sql = @"
Select PKey
From Pass0
Where ID = @ID
";

            return DBProxy.Current.LookupEx<long?>(sql, "ID", position);
        }
    }
}