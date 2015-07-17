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
            DBProxy.Current.Select(null, "SELECT * FROM Pass1", out dtPass1);
            if (dtPass1 != null) { dtPass1.PrimaryKey = new DataColumn[] { dtPass1.Columns["ID"] }; }
        }

        #region GetAuthority
        /// <summary>
        /// GetAuthority()
        /// </summary>
        /// <param name="strLogin"></param>
        /// <returns>bool</returns>
        public static bool GetAuthority(string login)
        {
            return true;
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
