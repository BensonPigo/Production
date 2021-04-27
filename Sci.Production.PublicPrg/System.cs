using EASendMail;
using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        /// <inheritdoc/>
        public static void SetGridColumnsColor(Win.UI.Grid grid, string tag, Color color)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.Name.Contains(tag))
                {
                    column.CellTemplate.Style.BackColor = color;
                }
            }
        }

        /// <inheritdoc/>
        public static Dictionary<string, string> GetColumnsDataPropertyNameWithTag(Win.UI.Grid grid, string tag)
        {
            Dictionary<string, string> columnHeaderandName = new Dictionary<string, string>();
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.Name.Contains(tag))
                {
                    columnHeaderandName.Add(column.DataPropertyName, column.HeaderText);
                }
            }

            return columnHeaderandName;
        }

        /// <inheritdoc/>
        public static bool CheckEmptyColumn(DataTable dt, Dictionary<string, string> columns, bool showmsg = false)
        {
            foreach (var keyValue in columns)
            {
                if (keyValue.Key != string.Empty && dt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && MyUtility.Check.Empty(w[keyValue.Key])).Any())
                {
                    if (showmsg)
                    {
                        MyUtility.Msg.WarningBox($"<{keyValue.Value}> can not be empty.");
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compare Arr 內容相同返回true,反之返回false。
        /// </summary>
        /// <param name="arr1">arr1</param>
        /// <param name="arr2">arr2</param>
        /// <returns>bool</returns>
        public static bool CompareArr(string[] arr1, string[] arr2)
        {
            var q = from a in arr1 join b in arr2 on a equals b select a;
            bool flag = arr1.Length == arr2.Length && q.Count() == arr1.Length;

            return flag;
        }

        /// <summary>
        /// 將 ExcelColumnName 轉 int
        /// </summary>
        /// <param name="columnName">A~</param>
        /// <returns>1~</returns>
        public static int ExcelColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return 0;
            }

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += columnName[i] - 'A' + 1;
            }

            return sum;
        }

        /// <inheritdoc/>
        public static bool CheckFloat(string svalue, int dp1, int dp2)
        {
            // 檢查值是否為浮點數字,dp1 ~ dp2 位小數
            return Regex.IsMatch(svalue, @"^[0-9]+(.[0-9]{" + dp1.ToString() + "," + dp2.ToString() + "})?$");
        }

        /// <summary>
        /// 轉呼叫string.IsNullOrWhiteSpace(source)
        /// </summary>
        /// <inheritdoc/>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        private static DataTable dtPass1 = null;

        /// <summary>
        /// Pass1Format
        /// </summary>
        public enum Pass1Format
        {
            /// <summary>
            /// IDNameExtDateTime
            /// </summary>
            IDNameExtDateTime = 1,

            /// <summary>
            /// IDNameDateTime
            /// </summary>
            IDNameDateTime = 2,

            /// <summary>
            /// NameExtDateTime
            /// </summary>
            NameExtDateTime = 3,

            /// <summary>
            /// NameDateTime
            /// </summary>
            NameDateTime = 4,

            /// <summary>
            /// IDNameExtDate
            /// </summary>
            IDNameExtDate = 5,

            /// <summary>
            /// IDNameDate
            /// </summary>
            IDNameDate = 6,

            /// <summary>
            /// NameExtDate
            /// </summary>
            NameExtDate = 7,

            /// <summary>
            /// NameDate
            /// </summary>
            NameDate = 8,

            /// <summary>
            /// NameExt
            /// </summary>
            NameExt = 9,
        }

        static Prgs()
        {
            DBProxy.Current.Select(null, "SELECT ID, Name, ExtNo FROM Pass1 WITH (NOLOCK) ", out dtPass1);
            if (dtPass1 != null)
            {
                dtPass1.PrimaryKey = new DataColumn[] { dtPass1.Columns["ID"] };
            }
        }

        #region GetAuthority

        /// <summary>
        /// GetAuthority()
        /// </summary>
        /// <param name="checkid">checkid</param>
        /// <returns>bool</returns>
        public static bool GetAuthority(string checkid)
        {
            if (Env.User.IsAdmin)
            {
                return true;
            }
            else
            {
                string sqlCmd = $@"with handlepass1
as
(select ID,Supervisor,Deputy from Pass1 WITH (NOLOCK) where ID = '{checkid}'),
superpass1
as
(select Pass1.ID,Pass1.Supervisor,Pass1.Deputy from Pass1 WITH (NOLOCK) ,handlepass1 where Pass1.ID = handlepass1.Supervisor),
allpass1
as
(select * from handlepass1
 union
 select * from superpass1
)
select * from allpass1 where ID = '{Env.User.UserID}' or Supervisor = '{Env.User.UserID}' or Deputy = '{Env.User.UserID}'";

                return MyUtility.Check.Seek(sqlCmd);
            }
        }

        /// <summary>
        /// GetAuthority()
        /// </summary>
        /// <param name="checkid">checkid</param>
        /// <param name="formcaption">formcaption</param>
        /// <param name="pass2colname">pass2colname</param>
        /// <returns>bool</returns>
        public static bool GetAuthority(string checkid, string formcaption, string pass2colname)
        {
            if (Env.User.IsAdmin)
            {
                return true;
            }
            else
            {
                string sql = string.Format("select FKPass0 from Pass1 WITH (NOLOCK) where ID='{0}'", Env.User.UserID);

                // Sci.Env.User.PositionID
                string positionID = MyUtility.GetValue.Lookup(sql);

                DualResult result = DBProxy.Current.Select(null, string.Format("select {0} as Result from Pass2 WITH (NOLOCK) where FKPass0 = {1} and UPPER(BarPrompt) = N'{2}'", pass2colname, positionID, formcaption.ToUpper()), out DataTable dt);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return false;
                }

                if (dt.Rows[0]["Result"].ToString().ToUpper() != "TRUE")
                {
                    return false;
                }

                string sqlCmd = $@"with handlepass1
as
(select ID,Supervisor,Deputy from Pass1 WITH (NOLOCK) where ID = '{checkid}'),
superpass1
as
(select Pass1.ID,Pass1.Supervisor,Pass1.Deputy from Pass1 WITH (NOLOCK) ,handlepass1 where Pass1.ID = handlepass1.Supervisor),
allpass1
as
(select * from handlepass1
 union
 select * from superpass1
)
select * from allpass1 where ID = '{Env.User.UserID}' or Supervisor = '{Env.User.UserID}' or Deputy = '{Env.User.UserID}'";

                return MyUtility.Check.Seek(sqlCmd);
            }
        }
        #endregion

        /// <summary>
        /// GetAddOrEditBy()
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="dateColumn">dateColumn</param>
        /// <param name="format">format</param>
        /// <returns>string</returns>
        public static string GetAddOrEditBy(object id, object dateColumn = null, int format = 1)
        {
            if (dtPass1 == null)
            {
                return id.ToString().Trim();
            }

            string strID = (Type.GetTypeCode(id.GetType()) == TypeCode.String) ? (string)id : id.ToString();
            DateTime dtDateColumn = (dateColumn == null || dateColumn == DBNull.Value) ? DateTime.MinValue : (DateTime)dateColumn;
            string strReturn = string.Empty;
            string extNo = "   Ext.";
            DataRow seekData = dtPass1.Rows.Find(strID);
            string strName;
            if (seekData == null)
            {
                return id.ToString().Trim();
            }
            else
            {
                strName = seekData["Name"].ToString().Trim();
                extNo += seekData["ExtNo"].ToString().Trim();
            }

            switch (format)
            {
                case 1:
                    strReturn = strID + " - " + strName + extNo + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 2:
                    strReturn = strID + " - " + strName + "   " + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 3:
                    strReturn = strName + extNo + "   " + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 4:
                    strReturn = strName + "   " + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateTimeFormatString());
                    break;
                case 5:
                    strReturn = strID + " - " + strName + extNo + "   " + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 6:
                    strReturn = strID + " - " + strName + "   " + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 7:
                    strReturn = strName + extNo + "   " + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 8:
                    strReturn = strName + "   " + ((dtDateColumn == DateTime.MinValue) ? string.Empty : "   " + dtDateColumn.ToAppDateFormatString());
                    break;
                case 9:
                    strReturn = strName + extNo;
                    break;
            }

            return strReturn;
        }

        /// <inheritdoc/>
        public static string GetPatternUkey(string cutref, string poid, string sizes)
        {
            string styleyukey = MyUtility.GetValue.Lookup("Styleukey", poid, "Orders", "ID");
            string sqlSizeGroup = $@"SELECT TOP 1 IIF(ISNULL(SizeGroup,'')='','N',SizeGroup) FROM Order_SizeCode WHERE ID ='{poid}' and SizeCode IN ({sizes})";
            string sizeGroup = MyUtility.GetValue.Lookup(sqlSizeGroup);
            string patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{poid}','{cutref}','',{styleyukey},'{sizeGroup}')s";
            string patternukey = MyUtility.GetValue.Lookup(patidsql);

            return patternukey;
        }

        /// <summary>
        /// 取得篩選 F_Code.CodeA,B...條件
        /// </summary>
        /// <param name="cutref">cutref</param>
        /// <param name="poid">poid</param>
        /// <param name="articles">article</param>
        /// <param name="fabricPanelCode">patternpanel</param>
        /// <param name="sizes">EX:S,M,L</param>
        /// <param name="sizeCount">sizeCount</param>
        /// <returns>EX: F_Code ='A' or CodeA = 'A' or ...</returns>
        public static string WhereArticleGroupColumn(string cutref, string poid, string articles, string fabricPanelCode, string sizes, int sizeCount)
        {
            string whereCode = string.Empty;
            string patternukey = GetPatternUkey(cutref, poid, sizes);
            string sqlPattern_GL_Article = $@"
if exists (Select ArticleGroup from Pattern_GL_Article WITH (NOLOCK) where PatternUkey = '{patternukey}' and rtrim(ltrim(Article)) in ({articles}) and SizeRange like '%,%')
Begin	
	Select distinct ArticleGroup
	from Pattern_GL_Article WITH (NOLOCK)
	where PatternUkey = '{patternukey}'
	and rtrim(ltrim(Article)) in ({articles})
	and SizeRange  = (
        select SizeRange
        from Pattern_GL_Article
        outer apply(select * from SplitString(SizeRange, ',') s)s
	    where PatternUkey = '{patternukey}'
	    and rtrim(ltrim(Article)) in ({articles})
        and rtrim(ltrim(Data)) in ({sizes})
        group by SizeRange
        having COUNT(1) = {sizeCount}
	)
End
";
            DualResult result = DBProxy.Current.Select(null, sqlPattern_GL_Article, out DataTable articleGroupDT);
            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.ToString());
                return whereCode;
            }

            if (articleGroupDT.Rows.Count == 0)
            {
                sqlPattern_GL_Article = $@"
if exists (Select * from Pattern_GL_Article WITH (NOLOCK) where PatternUkey = '{patternukey}' and rtrim(ltrim(Article)) in({articles})  and rtrim(ltrim(SizeRange)) in ({sizes}))
	Select distinct ArticleGroup from Pattern_GL_Article WITH (NOLOCK) where PatternUkey = '{patternukey}' and rtrim(ltrim(Article)) in({articles}) and rtrim(ltrim(SizeRange)) in ({sizes})    
else if exists (Select 1 from Pattern_GL_Article WITH (NOLOCK) where PatternUkey = '{patternukey}' and rtrim(ltrim(Article)) in ({articles}))
	Select distinct ArticleGroup from Pattern_GL_Article WITH (NOLOCK) where PatternUkey = '{patternukey}' and rtrim(ltrim(Article)) in ({articles})
else
	Select distinct ArticleGroup from Pattern_GL_Article WITH (NOLOCK) where PatternUkey = '{patternukey}' 
";
                result = DBProxy.Current.Select(null, sqlPattern_GL_Article, out articleGroupDT);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox(result.ToString());
                    return whereCode;
                }
            }

            foreach (DataRow dr in articleGroupDT.Rows)
            {
                whereCode += $" or {dr[0]} = '{fabricPanelCode}' ";
            }

            return whereCode;
        }

        /// <summary>
        /// GetGarmentListTable
        /// </summary>
        /// <param name="cutref">cutref</param>
        /// <param name="poid">poid</param>
        /// <param name="sizes">EX:'S','M','L'</param>
        /// <param name="outTb">outTb</param>
        /// <param name="articleGroupDT">articleGroupDT</param>
        public static void GetGarmentListTable(string cutref, string poid, string sizes, out DataTable outTb, out DataTable articleGroupDT)
        {
            outTb = null;
            string patternukey = GetPatternUkey(cutref, poid, sizes);

            #region 找 ArticleGroup 當Table Header
            string headercodesql = $"Select distinct ArticleGroup from Pattern_GL_LectraCode WITH (NOLOCK) where PatternUkey = '{patternukey}' and ArticleGroup !='F_CODE' order by ArticleGroup";
            DualResult headerResult = DBProxy.Current.Select(null, headercodesql, out articleGroupDT);
            if (!headerResult)
            {
                return;
            }
            #endregion
            #region 建立Table
            string tablecreatesql = $"Select a.*, orderid = '{poid}', nLocation = b.ID + '-' + b.Name, F_CODE = ''";
            foreach (DataRow dr in articleGroupDT.Rows)
            {
                tablecreatesql += $", {dr["ArticleGroup"]} = ''";
            }

            tablecreatesql += $" from Pattern_GL a WITH (NOLOCK) left join DropDownList b on b.Type='Location' and a.Location = b.ID Where PatternUkey = '{patternukey}'";
            DualResult tablecreateResult = DBProxy.Current.Select(null, tablecreatesql, out DataTable garmentListTb);
            if (!tablecreateResult)
            {
                return;
            }
            #endregion
            #region 寫入FCode~CodeA~CodeZ
            string lecsql = $"Select * from Pattern_GL_LectraCode a WITH (NOLOCK) where a.PatternUkey = '{patternukey}'";
            DualResult drre = DBProxy.Current.Select(null, lecsql, out DataTable drtb);
            if (!drre)
            {
                return;
            }

            foreach (DataRow dr in garmentListTb.Rows)
            {
                DataRow[] lecdrar = drtb.Select($"SEQ = '{dr["SEQ"]}'");
                foreach (DataRow lecdr in lecdrar)
                {
                    string artgroup = lecdr["ArticleGroup"].ToString().Trim();
                    dr[artgroup] = lecdr["FabricPanelCode"].ToString().Trim();
                }

                if (dr["SEQ"].ToString() == "0001")
                {
                    dr["PatternCode"] = dr["PatternCode"].ToString().Substring(10);
                }
            }
            #endregion
            outTb = garmentListTb;
        }

        /// <summary>
        /// 測試mail是否真實存在
        /// </summary>
        /// <param name="mailTo">mailTo</param>
        /// <returns>bool</returns>
        public static bool TestMail(string mailTo)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            // Set sender email address, please change it to yours
            oMail.From = MyUtility.GetValue.Lookup("select Sendfrom from System", "Production");

            // Set recipient email address, please change it to yours
            oMail.To = mailTo;

            // Do not set SMTP server address
            SmtpServer oServer = new SmtpServer(string.Empty);

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

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="rows">rows</param>
        public static void Delete(this IEnumerable<DataRow> rows)
        {
            foreach (var row in rows)
            {
                row.AcceptChanges();
                row.Delete();
            }
        }

        /// <summary>
        /// TryRemoveColumn
        /// </summary>
        /// <param name="columns">columns</param>
        /// <param name="dt">dt</param>
        public static void TryRemoveColumn(string columns, DataTable dt)
        {
            if (columns.Contains(columns))
            {
                dt.Columns.Remove(columns);
            }
        }
    }

    /// <summary>
    /// 開窗選擇一個檔案，用於替帶原生的OpenFileDialog
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
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

        /// <summary>
        /// 建立一個起檔案的視窗，交給來源端的moreSetting方法做設定，然後開窗，成功選擇檔案的話會記錄選擇位置，以供下次開啟
        /// </summary>
        /// <param name="afterSelected">afterSelected</param>
        /// <param name="moreSetting">moreSetting</param>
        /// <param name="filterSetting">filterSetting</param>
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
