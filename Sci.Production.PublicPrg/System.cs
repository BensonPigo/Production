using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;

namespace Sci.Production.PublicPrg
{
    
    public static partial class Prgs
    {
        private static DataTable dtPass1 = null;
        public enum Pass1Format
        {
            IDNameExtDateTime = 1, IDNameDateTime = 2, NameExtDateTime = 3, NameDateTime = 4, IDNameExtDate = 5, IDNameDate = 6, NameExtDate = 7, NameDate = 8, NameExt = 9
        }

        static Prgs()
        {
            DBProxy.Current.Select(null, "SELECT ID, Name, ExtNo FROM Pass1", out dtPass1);
            if (dtPass1 != null) { dtPass1.PrimaryKey = new DataColumn[] { dtPass1.Columns["ID"] }; }
        }

        #region GetAuthority
        /// <summary>
        /// GetAuthority()
        /// </summary>
        /// <param name="checkid"></param>
        /// <returns>bool</returns>
        public static bool GetAuthority(string checkid)
        {
            if (Sci.Env.User.IsAdmin)
            {
                return true;
            }
            else
            {
                string sqlCmd = string.Format(@"with handlepass1
as
(select ID,Supervisor,Deputy from Pass1 where ID = '{0}'),
superpass1
as
(select Pass1.ID,Pass1.Supervisor,Pass1.Deputy from Pass1,handlepass1 where Pass1.ID = handlepass1.Supervisor),
allpass1
as
(select * from handlepass1
 union
 select * from superpass1
)
select * from allpass1 where ID = '{1}' or Supervisor = '{1}' or Deputy = '{1}'", checkid, Sci.Env.User.UserID);

                return MyUtility.Check.Seek(sqlCmd) ? true : false;
            }
        }

        /// <summary>
        /// GetAuthority()
        /// </summary>
        /// <param name="checkid"></param>
        /// <param name="formcaption"></param>
        /// <param name="pass2colname"></param>
        /// <returns>bool</returns>
        public static bool GetAuthority(string checkid, string formcaption, string pass2colname)
        {
            if (Sci.Env.User.IsAdmin)
            {
                return true;
            }
            else
            {
                //Sci.Env.User.PositionID
                string PositionID = "1";
                DataTable dt;
                DualResult result = DBProxy.Current.Select(null, string.Format("select {0} as Result from Pass2 where FKPass0 = {1} and UPPER(BarPrompt) = N'{2}'", pass2colname, PositionID, formcaption.ToUpper()), out dt);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }

                if (dt.Rows[0]["Result"].ToString().ToUpper() != "TRUE")
                {
                    return false;
                }

                string sqlCmd = string.Format(@"with handlepass1
as
(select ID,Supervisor,Deputy from Pass1 where ID = '{0}'),
superpass1
as
(select Pass1.ID,Pass1.Supervisor,Pass1.Deputy from Pass1,handlepass1 where Pass1.ID = handlepass1.Supervisor),
allpass1
as
(select * from handlepass1
 union
 select * from superpass1
)
select * from allpass1 where ID = '{1}' or Supervisor = '{1}' or Deputy = '{1}'", checkid, Sci.Env.User.UserID);

                return MyUtility.Check.Seek(sqlCmd) ? true : false;
            }
        }
        #endregion

        /// <summary>
        /// GetAddOrEditBy()
        /// </summary>
        /// <param name="string id"></param>
        /// <param name="[Object datetime = null]"></param>
        /// <param name="[int format = 1]"></param>
        /// <returns>string</returns>
        public static string GetAddOrEditBy(Object id, Object dateColumn = null, int format = 1)
        {
            if (dtPass1 == null) { return id.ToString().Trim(); }

            string strID = (Type.GetTypeCode(id.GetType()) == TypeCode.String) ? (string)id : id.ToString();
            DateTime dtDateColumn = (dateColumn == null || dateColumn == DBNull.Value) ? DateTime.MinValue : (DateTime)dateColumn;
            string strReturn = "";
            string strName = "";
            string extNo = "   Ext.";
            DataRow seekData = dtPass1.Rows.Find(strID); ;
            if (seekData == null)
            {
                return id.ToString().Trim();
            }
            else
            {
                strName = seekData["Name"].ToString().Trim();
                extNo = extNo + seekData["ExtNo"].ToString().Trim();
            }

            switch (format)
            {
                case 1:
                    strReturn = strID + " - " + strName + extNo + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 2:
                    strReturn = strID + " - " + strName + "   " + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 3:
                    strReturn = strName + extNo + "   " + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 4:
                    strReturn = strName + "   " + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 5:
                    strReturn = strID + " - " + strName + extNo + "   " + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 6:
                    strReturn = strID + " - " + strName + "   " + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 7:
                    strReturn = strName + extNo + "   " + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 8:
                    strReturn = strName + "   " + ((dtDateColumn == DateTime.MinValue) ? "" : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 9:
                    strReturn = strName + extNo;
                    break;
            }

            return strReturn;
        }
    }
    
}
