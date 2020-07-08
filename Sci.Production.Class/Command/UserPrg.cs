using System;
using System.Collections.Generic;
using System.Data;
using SciConvert = Sci.MyUtility.Convert;

namespace Sci.Production.Class.Commons
{
    public class UserPrg
    {
        private static Dictionary<string, User> usersBook;

        public enum NameType
        {
            nameOnly = 1,
            nameAndExt = 2,
            idAndNameAndExt = 3,
            idAndName = 4,
        }

        static UserPrg()
        {
            Reload();
        }

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

        public static Dictionary<string, User> GetUserList_Reloaded()
        {
            Reload();
            return usersBook;
        }

        public static Dictionary<string, User> GetUserList_No_Reloaded()
        {
            // Reload();
            return usersBook;
        }

        public static bool HasUser(Dictionary<string, User> datas, object id)
        {
            return datas.ContainsKey(id.ToString().TrimEnd().ToUpper());
        }

        public static bool HasUser(Dictionary<string, User> datas, string id)
        {
            return datas.ContainsKey(id.TrimEnd().ToUpper());
        }

        public static User GetUser(string id)
        {
            id = id.TrimEnd().ToUpper();
            return usersBook.ContainsKey(id) ? usersBook[id] : new User();
        }

        public static User GetUser(object id)
        {
            return GetUser(id.ToString());
        }

        public static string GetName(object id, NameType type)
        {
            string retrivedName;

            // name = name.ToString();
            GetName(id, out retrivedName, type);
            return retrivedName;
        }

        public static bool GetName(object id, out object name, NameType type, int a)
        {
            string retrivedName;

            // name = name.ToString();
            bool ok = GetName(id, out retrivedName, type);
            name = retrivedName;
            return ok;
        }

        public static bool GetName(object id, out string name, NameType type)
        {
            return GetName(SciConvert.GetString(id), out name, type);
        }

        public static bool GetName(string id, out string name, NameType type)
        {
            bool returnBool = true;
            id = id.TrimEnd().ToUpper();
            if (usersBook.ContainsKey(id))
            {
                User theUser = usersBook[id];
                switch (type)
                {
                    case NameType.nameOnly:
                        name = theUser.name;
                        break;
                    case NameType.nameAndExt:
                        name = theUser.name + (MyUtility.Check.Empty(theUser.ext_No) ? string.Empty : " #" + theUser.ext_No);
                        break;
                    case NameType.idAndNameAndExt:
                        name = theUser.id + "-" + theUser.name + (MyUtility.Check.Empty(theUser.ext_No) ? string.Empty : " #" + theUser.ext_No);
                        break;
                    case NameType.idAndName:
                        name = theUser.id + "-" + theUser.name;
                        break;
                    default:
                        name = theUser.id;
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

    public class User
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string ext_No = string.Empty;
        public string Email = string.Empty;
        public string Factory = string.Empty;

        // public String email = "";
        // public String supervisor = "";
        // public String manager = "";
        // public String DepartmentID = "";
        // public String Country = "";
        // public String OrderGroup = "";
        // public String PWDWindows = "";
        // public String PWDSystem = "";
        // public String PWDEmail = "";
        // public DateTime? CreateDate = null;
        // public DateTime? Onboard = null;
        // public DateTime? Resign = null;
        // public bool Sample = false;
        // public bool Trade = false;
        // public bool Accounting = false;
        // public bool FromFactory = false;
        // public String Department = "";
        // public String Deputy = "";
        // public bool Kpi = false;
        // public bool TeamAvg = false;
        // public bool MultipleRank = false;
        // public String KpiRemark = "";
        // public String Duty = "";
        // public string CodePage = "";
        public User(DataRow row)
        {
            this.id = row["ID"].ToString().TrimEnd();
            this.name = row["name"].ToString().TrimEnd();
            this.ext_No = row["Ext_no"].ToString().TrimEnd();
            this.Email = row["Email"].ToString().TrimEnd();
            this.Factory = row["Factory"].ToString().TrimEnd();

            // this.Country = row["Country"].ToString().TrimEnd();
            // this.OrderGroup = row["OrderGroup"].ToString().TrimEnd();
            // this.DepartmentID = row["DepartmentID"].ToString().TrimEnd();
            //  this.email = row["Email"].ToString().TrimEnd();
            // this.supervisor = row["Supervisor"].ToString().TrimEnd();
            // this.manager = row["Manager"].ToString().TrimEnd();
            // this.PWDWindows = row["OrderGroup"].ToString().TrimEnd();
            // this.PWDSystem = row["OrderGroup"].ToString().TrimEnd();
            // this.PWDEmail = row["OrderGroup"].ToString().TrimEnd();
            // this.CreateDate = SciConvert.GetDate(row["CreateDate"]);
            // this.Onboard = SciConvert.GetDate(row["Onboard"]);
            // this.Resign = SciConvert.GetDate(row["Resign"]);
            // this.Sample = Check.isTrue( row["Sample"]);
            // this.Trade = Check.isTrue( row["Trade"]);
            // this.Accounting = Check.isTrue( row["Accounting"]);
            // this.FromFactory = Check.isTrue( row["FromFactory"]);
            // this.Department = row["OrderGroup"].ToString().TrimEnd();
            // this.Deputy = row["OrderGroup"].ToString().TrimEnd();
            // this.Kpi = Check.isTrue( row["Kpi"]);
            // this.TeamAvg = Check.isTrue( row["TeamAvg"]);
            // this.MultipleRank = Check.isTrue( row["MultipleRank"]);
            // this.KpiRemark = row["OrderGroup"].ToString().TrimEnd();
            // this.Duty = row["Duty"].ToString().TrimEnd();
            // this.CodePage = row["CodePage"].ToString().TrimEnd();
        }

        public User()
        {
        }
    }
}
