using System;
using System.Collections.Generic;
using System.Data;
using SciConvert = Sci.MyUtility.Convert;

namespace Sci.Production.Class.Commons
{
    /// <summary>
    /// UserPrg
    /// </summary>
    public class UserPrg
    {
        private static Dictionary<string, User> usersBook;

        /// <summary>
        /// NameType
        /// </summary>
        public enum NameType
        {
            /// <summary>
            /// Name
            /// </summary>
            NameOnly = 1,

            /// <summary>
            /// Name And Ext
            /// </summary>
            NameAndExt = 2,

            /// <summary>
            /// Id And Name And Ext
            /// </summary>
            IdAndNameAndExt = 3,

            /// <summary>
            /// Id And Name
            /// </summary>
            IdAndName = 4,
        }

        static UserPrg()
        {
            Reload();
        }

        /// <summary>
        /// Reload Data
        /// </summary>
        public static void Reload()
        {
            usersBook = new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);
            DataTable users;

            // String sqlCmd = "select id,name,Ext_no,Email,Supervisor,Manager,EmailID,DepartmentID from dbo.Account  ";
            string sqlCmd =
@"Select Pass1.ID , Pass1.Name, Pass1.Factory, Pass1.ExtNo as Ext_No
, Pass1.Email
From Production.dbo.Pass1 WITH (NOLOCK) 
Left Join Production.dbo.Factory WITH (NOLOCK) 
    On Factory.ID =  Pass1.Factory ";
            if (!SQL.Select(SQL.queryConn, sqlCmd, out users))
            {
                return;
            }

            foreach (DataRow row in users.Rows)
            {
                usersBook.Add(row["ID"].ToString().TrimEnd().ToUpper(), new User(row));
            }
        }

        /// <summary>
        /// UserList Reloaded
        /// </summary>
        /// <returns>Dictionary<string, User></returns>
        public static Dictionary<string, User> GetUserList_Reloaded()
        {
            Reload();
            return usersBook;
        }

        /// <summary>
        /// UserList_No Reloaded
        /// </summary>
        /// <returns>Dictionary<string, User></returns>
        public static Dictionary<string, User> GetUserList_No_Reloaded()
        {
            return usersBook;
        }

        /// <summary>
        /// Has User
        /// </summary>
        /// <param name="datas">Datas</param>
        /// <param name="id">User ID</param>
        /// <returns>bool</returns>
        public static bool HasUser(Dictionary<string, User> datas, object id)
        {
            return datas.ContainsKey(id.ToString().TrimEnd().ToUpper());
        }

        /// <summary>
        /// Has User
        /// </summary>
        /// <param name="datas">Datas</param>
        /// <param name="id">User ID</param>
        /// <returns>bool</returns>
        public static bool HasUser(Dictionary<string, User> datas, string id)
        {
            return datas.ContainsKey(id.TrimEnd().ToUpper());
        }

        /// <summary>
        /// Get User
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User</returns>
        public static User GetUser(string id)
        {
            id = id.TrimEnd().ToUpper();
            return usersBook.ContainsKey(id) ? usersBook[id] : new User();
        }

        /// <summary>
        /// GetUser
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User</returns>
        public static User GetUser(object id)
        {
            return GetUser(id.ToString());
        }

        /// <summary>
        /// Get User Name
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="type">NameType</param>
        /// <returns>string</returns>
        public static string GetName(object id, NameType type)
        {
            string retrivedName;

            // name = name.ToString();
            GetName(id, out retrivedName, type);
            return retrivedName;
        }

        /// <summary>
        /// Get User Name
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="name">User Name</param>
        /// <param name="type">NameType</param>
        /// <param name="a">a</param>
        /// <returns>bool</returns>
        public static bool GetName(object id, out object name, NameType type, int a)
        {
            string retrivedName;

            // name = name.ToString();
            bool ok = GetName(id, out retrivedName, type);
            name = retrivedName;
            return ok;
        }

        /// <summary>
        /// Get User Name
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="name">User Name</param>
        /// <param name="type">NameType</param>
        /// <returns>bool</returns>
        public static bool GetName(object id, out string name, NameType type)
        {
            return GetName(SciConvert.GetString(id), out name, type);
        }

        /// <summary>
        /// Get User Name
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="name">User Name</param>
        /// <param name="type">NameType</param>
        /// <returns>bool</returns>
        public static bool GetName(string id, out string name, NameType type)
        {
            bool returnBool = true;
            id = id.TrimEnd().ToUpper();
            if (usersBook.ContainsKey(id))
            {
                User theUser = usersBook[id];
                switch (type)
                {
                    case NameType.NameOnly:
                        name = theUser.Name;
                        break;
                    case NameType.NameAndExt:
                        name = theUser.Name + (MyUtility.Check.Empty(theUser.Ext_No) ? string.Empty : " #" + theUser.Ext_No);
                        break;
                    case NameType.IdAndNameAndExt:
                        name = theUser.Id + "-" + theUser.Name + (MyUtility.Check.Empty(theUser.Ext_No) ? string.Empty : " #" + theUser.Ext_No);
                        break;
                    case NameType.IdAndName:
                        name = theUser.Id + "-" + theUser.Name;
                        break;
                    default:
                        name = theUser.Id;
                        break;
                }
            }
            else
            {
                name = id;
                returnBool = false;
            }

            return returnBool;
        }
    }
}
