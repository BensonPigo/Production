using Sci.Data;
using System.Data;

namespace Sci.Production.Class.Commons
{
    /// <inheritdoc />
    public class NotificationPrg
    {
        /// <summary>
        /// 取得提醒筆數
        /// </summary>
        /// <param name="module">模組</param>
        /// <param name="id">UserID</param>
        /// <param name="dt">DataTable</param>
        /// <returns>提醒筆數</returns>
        public static int GetNotifyCount(string module, string id, out DataTable dt)
        {
            string sql = string.Empty;

            if (module.EqualString("PPIC"))
            {
                switch (id)
                {
                    case "01":
                        sql = $@"select ID from ReplacementReport with(nolock) where Status= 'checked' and type= 'F' and MDivisionID = '{Env.User.Keyword}'";
                        break;
                    case "02":
                        sql = $@"select ID from ReplacementReport with(nolock) where Status= 'checked' and type= 'A' and MDivisionID = '{Env.User.Keyword}'";
                        break;
                }
            }
            else if (module.EqualString("Shipping"))
            {
                switch (id)
                {
                    case "01":
                        sql = $"Sselect ID from AirPP with(nolock) where Status= 'checked' and MDivisionID = '{Env.User.Keyword}'";
                        break;
                }
            }

            string userID = Env.User.UserID;
            dt = DBProxy.Current.SelectEx(sql).ExtendedData;
            if (dt == null || dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return dt.Rows.Count;
            }
        }

        /// <summary>
        /// 取得設定的提醒資料
        /// </summary>
        /// <returns>needNotify</returns>
        public static DataTable GetNotifyDataTable()
        {
            string sql = @"
Select distinct Module = n0.menuname
    , ID =  n0.ID
    , Name = n0.Name
    , ClassName =  n0.FormName
    , Parameter =  ''
From Pass1 with(nolock)
Left join Pass0_Notify pn0 with(nolock) on pn0.Pass0_Ukey = pass1.FKPass0
Left join NotificationList n0 with(nolock) on pn0.NotificationList_Ukey = n0.Ukey 
Where pass1.ID = @ID
    And isnull(n0.Junk, 0) = 0 
    And isnull(pn0.SystemNotify, 0) = 1
";

            DataTable dt = DBProxy.Current.SelectEx(sql, "ID", Env.User.UserID).ExtendedData;

            return dt;
        }

        /// <summary>
        /// 設定檔資料
        /// </summary>
        public static DataTable dtNotifySetting;

        /// <inheritdoc />
        public class SetData
        {
            /// <inheritdoc />
            public DataTable SettingData
            {
                get { return dtNotifySetting; }
                set { dtNotifySetting = value; }
            }
        }
    }
}
