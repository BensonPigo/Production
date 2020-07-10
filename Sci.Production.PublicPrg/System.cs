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
using EASendMail;
using System.Windows.Forms;

namespace Sci.Production.PublicPrg
{    
    public static partial class Prgs
    {
        /// <summary>
        /// 轉呼叫string.IsNullOrWhiteSpace(source)
        /// </summary>
        /// <inheritdoc/>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        private static DataTable dtPass1 = null;
        public enum Pass1Format
        {
            IDNameExtDateTime = 1, IDNameDateTime = 2, NameExtDateTime = 3, NameDateTime = 4, IDNameExtDate = 5, IDNameDate = 6, NameExtDate = 7, NameDate = 8, NameExt = 9
        }

        static Prgs()
        {
            DBProxy.Current.Select(null, "SELECT ID, Name, ExtNo FROM Pass1 WITH (NOLOCK) ", out dtPass1);
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
(select ID,Supervisor,Deputy from Pass1 WITH (NOLOCK) where ID = '{0}'),
superpass1
as
(select Pass1.ID,Pass1.Supervisor,Pass1.Deputy from Pass1 WITH (NOLOCK) ,handlepass1 where Pass1.ID = handlepass1.Supervisor),
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
                string sql = string.Format("select FKPass0 from Pass1 WITH (NOLOCK) where ID='{0}'", Sci.Env.User.UserID);
                PositionID = MyUtility.GetValue.Lookup(sql);

                DataTable dt;
                DualResult result = DBProxy.Current.Select(null, string.Format("select {0} as Result from Pass2 WITH (NOLOCK) where FKPass0 = {1} and UPPER(BarPrompt) = N'{2}'", pass2colname, PositionID, formcaption.ToUpper()), out dt);
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
(select ID,Supervisor,Deputy from Pass1 WITH (NOLOCK) where ID = '{0}'),
superpass1
as
(select Pass1.ID,Pass1.Supervisor,Pass1.Deputy from Pass1 WITH (NOLOCK) ,handlepass1 where Pass1.ID = handlepass1.Supervisor),
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

        /// <summary>
        /// GetGarmentList()
        /// </summary>
        /// <param name="string styleukey"></param>
        /// <param name="Out DataTable(GarmentList Table)"></param>
        public static void GetGarmentListTable(string cutref, string OrderID, string sizeGroup ,out DataTable OutTb)
        {
            DataTable garmentListTb;
            string Styleyukey = MyUtility.GetValue.Lookup("Styleukey", OrderID, "Orders", "ID");

            #region 撈取Pattern Ukey  找最晚Edit且Status 為Completed
            OutTb = null;
            string patidsql;

            patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{OrderID}','{cutref}','',{Styleyukey},'{sizeGroup}')s";

            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            #endregion
            DataTable headertb;
            #region 找ArticleGroup 當Table Header
            string headercodesql = string.Format("Select distinct ArticleGroup from Pattern_GL_LectraCode WITH (NOLOCK) where PatternUkey = '{0}' and ArticleGroup !='F_CODE' order by ArticleGroup", patternukey);

            DualResult headerResult = DBProxy.Current.Select(null, headercodesql, out headertb);
            if (!headerResult)
            {
                return;
            }
            #endregion
            #region 建立Table
            string tablecreatesql = string.Format("Select '{0}' as orderid,a.*,'' as F_CODE",OrderID);
            foreach (DataRow dr in headertb.Rows)
            {
                tablecreatesql = tablecreatesql + string.Format(" ,'' as {0}", dr["ArticleGroup"]);
            }
            tablecreatesql = tablecreatesql + string.Format(" from Pattern_GL a WITH (NOLOCK) Where PatternUkey = '{0}'", patternukey);
            DualResult tablecreateResult = DBProxy.Current.Select(null, tablecreatesql, out garmentListTb);
            if (!tablecreateResult)
            {
                return;
            }
            #endregion
            #region 寫入FCode~CodeA~CodeZ
            string lecsql = "";
            lecsql = string.Format("Select * from Pattern_GL_LectraCode a WITH (NOLOCK) where a.PatternUkey = '{0}'", patternukey);
            DataTable drtb;
            DualResult drre = DBProxy.Current.Select(null, lecsql, out drtb);
            if (!drre)
            {
                return;
            }
            foreach (DataRow dr in garmentListTb.Rows)
            {
                DataRow[] lecdrar = drtb.Select(string.Format("SEQ = '{0}'", dr["SEQ"]));
                foreach (DataRow lecdr in lecdrar)
                {
                    string artgroup = lecdr["ArticleGroup"].ToString().Trim();
                    //dr[artgroup] = lecdr["PatternPanel"].ToString().Trim();
                    //Mantis_7045 比照舊系統對應FabricPanelCode
                    dr[artgroup] = lecdr["FabricPanelCode"].ToString().Trim();
                }
                if (dr["SEQ"].ToString() == "0001") dr["PatternCode"] = dr["PatternCode"].ToString().Substring(10);
            }
            #endregion
            OutTb = garmentListTb;
        }
        
        // 測試mail是否真實存在
        public static bool TestMail(string mailTo)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            // Set sender email address, please change it to yours
            oMail.From = MyUtility.GetValue.Lookup("select Sendfrom from System", "Production");

            // Set recipient email address, please change it to yours
            oMail.To = mailTo;

            // Do not set SMTP server address
            SmtpServer oServer = new SmtpServer("");

            try
            {
                oSmtp.TestRecipients(oServer, oMail);
            }
            catch (Exception ep)
            {
                MyUtility.Msg.ErrorBox("Invalid email address !!\r\n" + ep.Message);
                return false;
            }
            return true;
        }

        public static void Delete(this IEnumerable<DataRow> rows)
        {
            foreach (var row in rows)
                row.Delete();
        }

        public static void TryRemoveColumn(string columns, DataTable dt)
        {
            if (columns.Contains(columns)) dt.Columns.Remove(columns);
        }
    }


    /// <summary>
    /// 開窗選擇一個檔案，用於替帶原生的OpenFileDialog
    /// </summary>
    public static class SciFileDialog
    {
        /// <summary>
        /// 每次都忘記怎麼設定Filter，做一個小東西來省大腦空間
        /// </summary>
        public struct OpenFileDialogFilterPair
        {
            /// <summary>
            /// 顯示出來給使用者看到的文字
            /// </summary>
            public string Display { get; set; }

            /// <summary>
            /// 實際用來篩選檔案的文字 ex: *.xls, *.xlsx
            /// </summary>
            public List<string> FilterText { get; set; }

            /// <summary>
            /// 每次都忘記怎麼設定Filter，做一個小東西來省大腦空間
            /// </summary>
            /// <inheritdoc />
            public OpenFileDialogFilterPair(string display, params string[] filterText)
            {
                this.Display = display;
                this.FilterText = filterText.Any() ? filterText.ToList() : new List<string>() { display };
            }
        }

        private static bool IsLoadded = false;

        /// <summary>
        /// 建立一個起檔案的視窗，交給來源端的moreSetting方法做設定，然後開窗，成功選擇檔案的話會記錄選擇位置，以供下次開啟
        /// </summary>
        /// <inheritdoc/>
        public static void ShowDialog(Action<OpenFileDialog> afterSelected, Action<OpenFileDialog> moreSetting = null, params OpenFileDialogFilterPair[] filterSetting)
        {
            if (afterSelected == null)
            {
                throw new ArgumentException("afterSelected argument can not be null");
            }

            using (var dlg = new OpenFileDialog())
            {
                if (filterSetting != null && filterSetting.Any())
                {
                    dlg.Filter = filterSetting
                        .Select(item => $"{item.Display}|{item.FilterText.Select(item2 => item2).JoinToString(";")}")
                        .JoinToString("|");
                }

                moreSetting?.Invoke(dlg);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    afterSelected(dlg);
                }
            }
        }

        /// <summary>
        /// 開窗選擇一個檔案，可以透過moresetting進行細部設定
        /// </summary>
        /// <param name="filterSetting">篩選檔案的設定</param>
        /// <returns>SelectSingleFileResult物件可以直接隱含轉型為bool或是string</returns>
        public static SelectSingleFileResult SelectFile(params OpenFileDialogFilterPair[] filterSetting)
        {
            SelectSingleFileResult result = null;
            ShowDialog(dlg => result = dlg.FileName, null, filterSetting);
            return result;
        }

        /// <summary>
        /// 開窗選擇一個檔案，可以透過moresetting進行細部設定
        /// </summary>
        /// <param name="moreSetting">透過存取傳入物件來改變細節設定</param>
        /// <param name="filterSetting">篩選檔案的設定</param>
        /// <returns>SelectSingleFileResult物件可以直接隱含轉型為bool或是string</returns>
        public static SelectSingleFileResult SelectFile(Action<OpenFileDialog> moreSetting = null, params OpenFileDialogFilterPair[] filterSetting)
        {
            SelectSingleFileResult result = null;
            ShowDialog(dlg => result = dlg.FileName, moreSetting, filterSetting);
            return result;
        }

        /// <summary>
        /// 開窗選擇複數檔案，可以透過moresetting進行細部設定
        /// </summary>
        /// <param name="filterSetting">篩選檔案的設定</param>
        /// <returns>SelectMultiFileResult物件可以直接隱含轉型為bool或是string</returns>
        public static SelectMultiFileResult SelectFiles(params OpenFileDialogFilterPair[] filterSetting)
        {
            SelectMultiFileResult result = null;
            ShowDialog(dlg => result = dlg.FileNames, null, filterSetting);
            return result;
        }

        /// <summary>
        /// 開窗選擇複數檔案，可以透過moresetting進行細部設定
        /// </summary>
        /// <param name="moreSetting">透過存取傳入物件來改變細節設定</param>
        /// <param name="filterSetting">篩選檔案的設定</param>
        /// <returns>SelectMultiFileResult物件可以直接隱含轉型為bool或是string</returns>
        public static SelectMultiFileResult SelectFiles(Action<OpenFileDialog> moreSetting = null, params OpenFileDialogFilterPair[] filterSetting)
        {
            SelectMultiFileResult result = null;
            ShowDialog(dlg => result = dlg.FileNames, moreSetting, filterSetting);
            return result;
        }
    }

    /// <summary>
    /// SciFileDialog.SelectFile的回傳物件，可以隱含轉換為string(FileName屬性)，也可隱含轉換為bool(FileName.IsNullOrWhiteSpace)
    /// </summary>
    public struct SelectSingleFileResult
    {
        /// <summary>
        /// 選到的檔案名稱
        /// </summary>
        public string FileName;

        /// <inheritdoc/>
        public static implicit operator string(SelectSingleFileResult v)
        {
            return v.FileName;
        }

        /// <inheritdoc/>
        public static implicit operator bool(SelectSingleFileResult v)
        {
            return v.FileName.IsNullOrWhiteSpace() == false;
        }

        /// <inheritdoc/>
        public static implicit operator SelectSingleFileResult(string v)
        {
            return new SelectSingleFileResult()
            {
                FileName = v,
            };
        }
    }

    /// <summary>
    /// SciFileDialog.SelectFiles的回傳物件，可以隱含轉換為string[](FileNames屬性)，也可隱含轉換為bool(FileNames != null and FileNames.Any())
    /// </summary>
    public struct SelectMultiFileResult
    {
        /// <inheritdoc/>
        public string[] FileNames;

        /// <inheritdoc/>
        public static implicit operator string[](SelectMultiFileResult v)
        {
            return v.FileNames;
        }

        /// <inheritdoc/>
        public static implicit operator bool(SelectMultiFileResult v)
        {
            return v.FileNames != null && v.FileNames.Any(item => item.IsNullOrWhiteSpace() == false);
        }

        /// <inheritdoc/>
        public static implicit operator SelectMultiFileResult(string[] v)
        {
            return new SelectMultiFileResult()
            {
                FileNames = v,
            };
        }
    }
}
