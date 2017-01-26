using Sci.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Net;
using System.IO;
using Sci;
using System.Diagnostics;
using System.Configuration;

namespace Production.Daily
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        bool isAuto = false;
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();

        public Main()
        {
            InitializeComponent();
            isAuto = false;
        }

        public Main(String _isAuto)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(_isAuto))
            {
                isAuto = true;
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            OnRequery();

            transferPMS.fromSystem = "Production";

            if (isAuto)
            {
                ClickExport();
                this.Close();
            }
        }

        private void OnRequery()
        {
            DataTable _mailTo;
            String sqlCmd;
            //需要改2個地方, 1是撈取 2是save          
            
            if (MyUtility.Check.Seek("select * from Production.dbo.MailTo where id='099'"))
            {
                sqlCmd = "Select * From dbo.MailTo Where ID = '099'";    
            }
            else
            {
                sqlCmd = "Select * From dbo.MailTo Where ID = '001'";
            }


            DualResult result = DBProxy.Current.Select(null, sqlCmd, out _mailTo);

            if (!result) { ShowErr(result); return; }

            mailTo = _mailTo.Rows[0];

            editToAddress.Text = mailTo["ToAddress"].ToString();
            editCcAddress.Text = mailTo["CcAddress"].ToString();
            editContent.Text = mailTo["Content"].ToString();
        }

        protected override DualResult ClickSavePost()
        {
            DualResult result;
            String sqlCmd;
            List<SqlParameter> paras = new List<SqlParameter>();
            if (MyUtility.Check.Seek("select * from Production.dbo.MailTo where id='099'"))
            {
                sqlCmd = "Update dbo.MailTo Set ToAddress = @ToAddress, CcAddress = @CcAddress, Content = @Content Where ID = '099'";
            }
            else
            {
                sqlCmd = "Update dbo.MailTo Set ToAddress = @ToAddress, CcAddress = @CcAddress, Content = @Content Where ID = '001'";
            }

            
            paras.Add(new SqlParameter("@ToAddress", editToAddress.Text));
            paras.Add(new SqlParameter("@CcAddress", editCcAddress.Text));
            paras.Add(new SqlParameter("@Content", editContent.Text));
            result = DBProxy.Current.Execute(null, sqlCmd, paras);
            if (!result) { return result; }

            sqlCmd = "Update dbo.TransRegion Set RarName = @RarName Where Is_Export = 0";
            paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@RarName", this.CurrentData["ImportDataFileName"]));
            result = DBProxy.Current.Execute(null, sqlCmd, paras);
            if (!result) { return result; }

            return base.ClickSavePost();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            OnRequery();
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            OnRequery();
        }

        #region 接Sql Server 進度訊息用
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            BeginInvoke(() => { this.labelProgress.Text = e.Message; });
        }
        #endregion

        public List<SqlParameter> GetSqlParameter()
        {
            return null;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DateTime Now = DateTime.Today;
            ClickExport();
        }

        private void btnTestFTP_Click(object sender, EventArgs e)
        {
            DualResult result;
            result = transferPMS.Ftp_Ping(this.CurrentData["FtpIP"].ToString(), this.CurrentData["FtpID"].ToString(), this.CurrentData["FtpPwd"].ToString());

            string rarFile =ConfigurationSettings.AppSettings["rarexefile"].ToString();
           
            if (!result)
            {
                ShowErr(result);
            }
            else if (!File.Exists(rarFile))
            {
                MyUtility.Msg.WarningBox("Win_RAR File does not exist.");
            }
            else
            {
                MyUtility.Msg.InfoBox("Connecting is Successful");
            }
        }

        private void BtnTestMail_Click(object sender, EventArgs e)
        {
            SendMail();
        }

        private void btnGetImportDataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                this.displayImportDataPath.Value = path.SelectedPath;
            }
        }

        private void btnGetExportDataPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            if (path.ShowDialog() == DialogResult.OK)
            {
                this.displayExportDataPath.Value = path.SelectedPath;
            }
        }

        #region Send Mail
        private void SendMail(String subject = "", String desc = "")
        {
            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            transferPMS.SetSMTP(mailServer, 25, eMailID, eMailPwd);

            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String toAddress = mailTo["ToAddress"].ToString();
            String ccAddress = mailTo["CcAddress"].ToString();
            //String toAddress = "willy.wei@sportscity.com.tw";
            //String ccAddress = "";
            if (String.IsNullOrEmpty(subject))
            {
                subject = mailTo["Subject"].ToString();
            }
            if (String.IsNullOrEmpty(desc))
            {
                desc = mailTo["Content"].ToString();
            }
            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc, true, true);

            mail.ShowDialog();
        }
        #endregion

        #region Update/Update動作
        private void ClickExport()
        {
            SqlConnection conn;

            if (!Sci.SQL.GetConnection(out conn)) { return; }
            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result = AsyncHelper.Current.DataProcess(this, () =>
            {
                return AsyncUpdateExport(conn);
            });

            if (!result)
            {
                ShowErr(result);
            }

            conn.Close();
        }
        #endregion

        #region Export/Update (非同步)
        private DualResult AsyncUpdateExport(SqlConnection conn)
        {
            DualResult result;
            DateTime startDate;
            DateTime endDate;
            String subject = "";
            String desc = "";
            String sqlCmd = "";

            String ftpIP = this.CurrentData["FtpIP"].ToString().Trim() + "/";
            String ftpID = this.CurrentData["FtpID"].ToString().Trim();
            String ftpPwd = this.CurrentData["FtpPwd"].ToString().Trim();

            #region [File Name (with ZIP)]不可為空白
            if (String.IsNullOrEmpty(this.CurrentData["ImportDataFileName"].ToString()))
            {
                return new DualResult(false, "File Name(with Zip) can not be empty!");
            }
            #endregion
            #region 若[Rg Code]為空，則用[File Name (with ZIP)]的值補上
            if (String.IsNullOrEmpty(this.CurrentData["RgCode"].ToString()))
            {
                int posStart = this.CurrentData["ImportDataFileName"].ToString().IndexOf('_') + 1;
                int posEnd = this.CurrentData["ImportDataFileName"].ToString().IndexOf('.');

                this.CurrentData["RgCode"] = this.CurrentData["ImportDataFileName"].ToString().Substring(posStart, posEnd - posStart);

                String updCmd = String.Format("Update dbo.System Set RgCode = {0}", this.CurrentData["RgCode"].ToString());

                result = DBProxy.Current.ExecuteByConn(conn, updCmd);
                if (!result)
                {
                    return result;
                }
            }
            #endregion
            #region 判斷[Updatae datas path(Taipei)]路徑是否存在
            String importDataPath = this.CurrentData["ImportDataPath"].ToString();
            if (importDataPath.Substring(importDataPath.Length - 1, 1) != "\\")
            {
                importDataPath += "\\";
            }
            if (!Directory.Exists(importDataPath))
            {
                Directory.CreateDirectory(importDataPath);
            }
            #endregion
            #region 判斷[Export datas path]路徑是否存在
            String exportDataPath = this.CurrentData["ExportDataPath"].ToString();
            if (exportDataPath.Substring(exportDataPath.Length - 1, 1) != "\\")
            {
                exportDataPath += "\\";
            }
            if (!Directory.Exists(exportDataPath))
            {
                Directory.CreateDirectory(exportDataPath);
            }
            #endregion
            #region 判斷[Export datas path]路徑底下的轉入當日的Week
            String weekDayPath = GetWeekDayPath();

            //exportDataPath = exportDataPath + weekDayPath;

            if (!Directory.Exists(exportDataPath))
            {
                Directory.CreateDirectory(exportDataPath);
            }
            #endregion
            #region 執行前發送通知mail
            /*
            subject = "Logon to  Mail Server from " + this.CurrentData["RgCode"].ToString();
            desc = "Logon to  Mail Server from " + this.CurrentData["RgCode"].ToString();
            SendMail(subject, desc);
            */
            #endregion
            #region CHECK THE FIRST NEED MAPPING A DISK,CAN'T USING \\ UNC
            if (importDataPath.Substring(0, 2) == "////" || exportDataPath.Substring(0, 2) == "////")
            {
                return new DualResult(false, "You can't setup the path using by \\ (UNC),pls mapping for a disk then can do it");
            }
            #endregion
            #region 若有勾選[Delete file from exporting folder]，則先刪除local之資料匣中所有檔案((上傳時))
            if (this.checkDeleteFile.Checked)
            {
                Directory.Delete(exportDataPath, true);
                Directory.CreateDirectory(exportDataPath);
            }
            #endregion

            result = transferPMS.Ftp_Ping(ftpIP, ftpID, ftpPwd);
            if (!result) { return result; }

            String exportRgCode = "";
            String importRgCode = "";
            String exportFileName = "";
            String importFileName = "";
            TransRegion exportRegion;
            TransRegion importRegion;

            #region 取得轉出/轉入用的Region
            exportFileName = this.CurrentData["RgCode"].ToString().Trim() + "_Reports.rar";
            importFileName = this.CurrentData["ImportDataFileName"].ToString();

            exportRgCode = this.CurrentData["RgCode"].ToString();
            importRgCode = importFileName;

            result = GetTransRegion("E", exportRgCode, exportDataPath, exportFileName, out exportRegion);
            if (!result) { return result; }

            result = GetTransRegion("I", importRgCode, importDataPath, importFileName, out importRegion);
            if (!result) { return result; }
            #endregion

            #region 先把Trade_To_Pms的DB drop掉
            transferPMS.DeleteDatabase(importRegion);
            #endregion

            #region 解壓縮檔案到資料夾裡
            if (!transferPMS.UnRAR_To_ImportDir(importRegion))
            {
                return new DualResult(false, "FTP Download failed!");
            };
            string rarFile = ConfigurationSettings.AppSettings["rarexefile"].ToString();
            if (!File.Exists(rarFile))
            {                
                return new DualResult(false, "Win_RAR File does not exist !");
            }
            #endregion
            startDate = DateTime.Now;

            #region 開始執行轉出
            result = DailyExport(exportRegion, importRegion);

            if (!result)
            {
                ErrMail("Export", transferPMS.Regions_All); //exportRegion);
                return result;
            }
            #endregion

            

            #region 開始執行轉入
            result = DailyImport(importRegion);

            endDate = DateTime.Now;

            if (!result) 
            {
                ErrMail("Import", transferPMS.Regions_All); //importRegion);
                return result;             
            }
            #endregion

            #region 完成後發送Mail
            DataTable orderComparisonList;
            DataTable dateInfo;
            #region 增列comparison list 日期
            String transferDate;
            String updateDate;

            sqlCmd = "Select Top 1 TransferDate, UpdateDate" +
                     "  From Production.dbo.OrderComparisonList" +
                     " Where UpdateDate = (Select Max(UpdateDate) From Production.dbo.OrderComparisonList as tmp)" +
                     " Order by Ukey desc";

            result = DBProxy.Current.Select(null, sqlCmd, out orderComparisonList);
            if (!result || orderComparisonList.Rows.Count == 0)
            {
                transferDate = "";
                updateDate = "";
            }
            else
            {
                if (MyUtility.Check.Empty(((DateTime)orderComparisonList.Rows[0]["TransferDate"]).ToShortDateString()))
                {
                    transferDate = "";
                }
                else
                {
                    transferDate = ((DateTime)orderComparisonList.Rows[0]["TransferDate"]).ToShortDateString();
                }               
                updateDate = ((DateTime)orderComparisonList.Rows[0]["UpdateDate"]).ToShortDateString();
            }
            #endregion

            //subject = "Auto Download and Updated for PMS!! from " + this.CurrentData["RgCode"].ToString();
            #region 組合 Desc
            desc = "Dear All:" + Environment.NewLine + Environment.NewLine +
                   "             (**Please don't reply this mail. **)" + Environment.NewLine + Environment.NewLine +
                   "PMS system already Downloaded and Updated From Taipei's FTP." + Environment.NewLine +
                   "Pls confirm and take notes." + Environment.NewLine +
                   "================================" + Environment.NewLine +
                   "Factory Name: " + this.CurrentData["RgCode"].ToString().ToUpper() + Environment.NewLine +
                   "--------------------------------------------------------" + Environment.NewLine +
                   "    Start Time: " + startDate.ToString() + Environment.NewLine +
                   "    Ending Time: " + endDate.ToString() + Environment.NewLine +
                   "    Comparison List: OldDate:" + transferDate.ToString() + "  Update:" + updateDate.ToString() + Environment.NewLine +
                   "--------------------------------------------------------"
                   ;
            #region 取得各個function的起訖日期，寫入mail desc.
            sqlCmd = "Select 1 as Seq, 'Export' as TransState, Name, DateStart, DateEnd From Pms_To_Trade.dbo.DateInfo \n" +
                     " Union All \n" +
                     " Select 2 as Seq, 'Upload' as TransState, Name, DateStart, DateEnd From Trade_To_Pms.dbo.DateInfo \n" +
                     " Order by Seq ";
            result = DBProxy.Current.Select(null, sqlCmd, out dateInfo);
            if (result && orderComparisonList.Rows.Count > 0)
            {
                foreach (DataRow row in dateInfo.Rows)
                {
                    desc += Environment.NewLine + row["TransState"].ToString() + ":" + row["Name"].ToString() + "    From :" + row["DateEnd"].ToString() + " To :" + row["Name"].ToString();
                }
            }
            #endregion
            desc += Environment.NewLine + "================================" +
                    Environment.NewLine + mailTo["Content"].ToString();
            #endregion
            subject = mailTo["Subject"].ToString().TrimEnd() + this.CurrentData["RgCode"].ToString();

            SendMail(subject, desc);
            #endregion

            return Ict.Result.True;
        }

        void ErrMail(string Type, BindingList<TransRegion> Regions_All)
        {
            #region --export mail--
            string formatStr = @"Dear  All
                (**Please don't reply this mail. **)

{0} data from PMS system.

Region      Succeeded       Message
***--------------------------------------------------------***
{1}
***--------------------------------------------------------***
";
            string totalMsg = "";
            TransRegion Region = Regions_All[0];
            string RegionStr = Region.Region;            
            string Msg = "";//tfTrade.Regions_All[i].Message;
            bool success = Region.Succeeded;
            for (int k = 0; k < Region.Logs.Count; k++)
            {
                Msg += string.Format("time: {0} {2} message:{1} {3}"
                    , Region.Logs[k].Key
                    , Region.Logs[k].Value.ToString()
                    , Environment.NewLine
                    , Environment.NewLine + "--------------------------------------------------------" + Environment.NewLine);
            }
            totalMsg += RegionStr + "      " + success + "      " + Environment.NewLine + Msg;
            formatStr = string.Format(formatStr, Type, totalMsg);

            string title = string.Format("Trans{0}_{1} {2}", Type, RegionStr, DateTime.Now.ToShortDateString());
            if (!isAuto) title = "<<手動執行>> " + title;

            SendMail(title, formatStr);
            #endregion
        }

        #endregion

        #region Export
        private DualResult DailyExport(TransRegion exportRegion, TransRegion importRegion)
        {
            DualResult result;
            String sqlCmd = "";
            String ftpIP = this.CurrentData["FtpIP"].ToString().Trim();
            String ftpID = this.CurrentData["FtpID"].ToString().Trim();
            String ftpPwd = this.CurrentData["FtpPwd"].ToString().Trim();

            #region 刪除原先的壓縮檔
            if (File.Exists(exportRegion.DirName + exportRegion.RarName))
            {
                File.Delete(exportRegion.DirName + exportRegion.RarName);
            }
            #endregion

            #region 刪除FTP的檔案

            if (IsFtpFileExist(exportRegion.RarName))
            {
                if (!transferPMS.Delete_Rar_On_Ftp(exportRegion))
                {
                    return new DualResult(false, "Delete FTP File failed!");
                }
            }

            #endregion

            #region 判斷若DB不存在，就掛載
            DataTable isDbExist;
            sqlCmd = "Select Name From master.dbo.sysdatabases Where ('[' + name + ']' = @DbName OR name = @DbName)";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@DbName", exportRegion.DBName));
            result = DBProxy.Current.Select(null, sqlCmd, paras, out isDbExist);
            if (!result) { return result; }
            else
            {
                if (isDbExist == null || isDbExist.Rows.Count == 0)
                {
                    transferPMS.Attach_DataBase(exportRegion, 1);
                }
            }
            #endregion

            #region Deploy procedure 到Pms_To_Trade
            Dictionary<string, KeyValuePair<DateTime?, DualResult>> resDic = transferPMS.Deploy_Procedure(importRegion.DirName, prefix: "exp_", connectionName: exportRegion.DBName);
            foreach (var item in resDic)
            {
                exportRegion.Logs.Add(item.Value);
            }
            #endregion

            #region 刪除Table所有資料
            if (!transferPMS.Drop_Tables(exportRegion))
            {
                return new DualResult(false, "Delete Table failed!");
            }
            #endregion
            #region 停用CDC
            sqlCmd = "Use [Pms_To_Trade];" +
                     "Exec sys.sp_cdc_disable_db;";
            #endregion
            #region Setup Data
            String _fromPath = "";
            DataTable transExport;
            sqlCmd = "Use [Production];" +
                     "Select * From dbo.TransRegion Left Join dbo.TransExport On 1 = 1 Where TransRegion.Is_Export = 1";

            result = DBProxy.Current.Select(null, sqlCmd, out transExport);
            if (!result) { return result; }
            if (transExport.Rows.Count > 0)
            {
                foreach (DataRow row in transExport.Rows)
                {
                    _fromPath = row["DirName"].ToString();
                    row["DirName"] = exportRegion.DirName;
                }

                transferPMS.SetupData(transExport);
            }
            #endregion

            //if (!transferPMS.Export_ByRegion())
            if (!transferPMS.Export_Pms_To_Trade(ftpIP, ftpID, ftpPwd, _fromPath, exportRegion.DBName))
            {
                return new DualResult(false, "Export failed!");
            }
            /*
            #region 卸載DateBase
            result = transferPMS.Detach_Database(exportRegion);
            if (!result) { return result; }
            #endregion
            #region 壓縮mdf檔案
            List<String> fileList = new List<string>();
            fileList.Add(exportRegion.DirName + exportRegion.DBFileName + ".mdf");
            transferPMS.RAR_Files(exportRegion.DirName, exportRegion.RarName, fileList);
            #endregion
            #region 上傳FTP
            transferPMS.Export_EndTransfer_RarUpload(exportRegion.DirName, exportRegion.RarName, exportRegion);
            #endregion
            */
            return Ict.Result.True;
        }
        #endregion

        #region Update
        private DualResult DailyImport(TransRegion region)
        {
            DualResult result;
            String ftpIP = this.CurrentData["FtpIP"].ToString().Trim();
            String ftpID = this.CurrentData["FtpID"].ToString().Trim();
            String ftpPwd = this.CurrentData["FtpPwd"].ToString().Trim();

            /*
            TransRegion region = new TransRegion();
            region.Region = importFileName;
            region.DirName = importDataPath;
            region.RarName = importFileName;
            region.ConnectionName = dataBase;
            region.DBName = dataBase;
            region.DBFileName = dataBase;
            region.Is_Export = true;
            */
            #region 檢查FTP檔案的日期是否正確
            //if (!transferPMS.CheckRar_CreateDate(region, region.RarName, false))
            //{
            //    String subject = "PMS transfer data (New) ERROR";
            //    String desc = "Wrong the downloaded file date!!,Pls contact with Taipei.";
            //    SendMail(subject, desc);
            //    return Ict.Result.F("Wrong the downloaded file date!!,Pls contact with Taipei.");
            //}
            #endregion
            #region 刪除DataBase
            result = transferPMS.DeleteDatabase(region);
            if (!result) { return result; }
            #endregion
            #region Setup Data
            DataTable transImport;
            String sqlCmd = "Use [Production];" +
                            "Select * From dbo.TransRegion Left Join dbo.TransImport On 1 = 1 Where TransRegion.Is_Export = 0";

            result = DBProxy.Current.Select(null, sqlCmd, out transImport);
            if (!result) { return result; }
            if (transImport.Rows.Count > 0)
            {
                /*
                foreach (DataRow row in transImport.Rows)
                {
                    row["ConnectionName"] = region.ConnectionName;
                    row["DBName"] = region.DBName;
                    row["DBFileName"] = region.DBFileName;
                }
                */
                transferPMS.SetupData(transImport);
            }
            #endregion
            #region 將資料Copy To DB資料夾以掛載
            String fromPath = this.CurrentData["ImportDataPath"].ToString();
            String toPath = transImport.Rows[0]["DirName"].ToString();
            transferPMS.UnRAR_To_ImportDir(region);
            // File.Copy(Path.Combine(fromPath, region.DBFileName + ".mdf"), Path.Combine(toPath, region.DBFileName + ".mdf"),true);
            #endregion
            #region 掛載資料庫
            transferPMS.Attach_DataBase(region, 1);
            #endregion

            #region 掛載LockDate 
            Dictionary<string, KeyValuePair<DateTime?, DualResult>> resDic1 = Deploy_LockDate(region.DirName, prefix: "update_", connectionName: transferPMS.fromSystem);
            foreach (var item in resDic1)
            {
                region.Logs.Add(item.Value);
            }
            
            #endregion

            #region Deploy procedure 到Production
            Dictionary<string, KeyValuePair<DateTime?, DualResult>> resDic = transferPMS.Deploy_Procedure(region.DirName, prefix: "imp_", connectionName: transferPMS.fromSystem);
            foreach (var item in resDic)
            {
                region.Logs.Add(item.Value);
            }
            #endregion

            if (!transferPMS.Import_Trade_To_Pms(ftpIP, ftpID, ftpPwd))
            {
                return new DualResult(false, "Update failed!");
                String subject = "PMS transfer data (New) ERROR";
                String desc = "Wrong the Update failed!!,Pls contact with Taipei.";
                SendMail(subject, desc);
                return Ict.Result.F("Wrong the Update failed!!,Pls contact with Taipei.");
            }
            return Ict.Result.True;
        }
        #endregion

        #region FTP File Exists
        /// <summary>
        /// Check FTP File Exists
        /// </summary>
        /// <param name="fileName">server file name.</param>
        private bool IsFtpFileExist(String fileName)
        {
            bool isExists = false;
            String ftpIP = @"ftp://" + this.CurrentData["FtpIP"].ToString().Trim() + @"/";
            String ftpID = this.CurrentData["FtpID"].ToString().Trim();
            String ftpPwd = this.CurrentData["FtpPwd"].ToString().Trim();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpIP + fileName);
            request.Credentials = new NetworkCredential(ftpID, ftpPwd);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Timeout = (60000 * 1);

            FtpWebResponse response = null;
            try
            {
                response = (FtpWebResponse)request.GetResponse();
                isExists = true;
            }
            catch (WebException ex)
            {
                response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    isExists = false;
                }
            }
            finally
            {
                response.Close();
            }

            return isExists;
        }
        #endregion

        #region 取得每周的簡稱
        private String GetWeekDayPath()
        {
            DayOfWeek weekDayNow = DateTime.Now.DayOfWeek;
            String weekDayPath = "";
            switch (weekDayNow)
            {
                case DayOfWeek.Sunday:
                    weekDayPath = "Sun\\";
                    break;
                case DayOfWeek.Monday:
                    weekDayPath = "Mon\\";
                    break;
                case DayOfWeek.Tuesday:
                    weekDayPath = "Tue\\";
                    break;
                case DayOfWeek.Wednesday:
                    weekDayPath = "Wed\\";
                    break;
                case DayOfWeek.Thursday:
                    weekDayPath = "Thu\\";
                    break;
                case DayOfWeek.Friday:
                    weekDayPath = "Fri\\";
                    break;
                case DayOfWeek.Saturday:
                    weekDayPath = "Sat\\";
                    break;
            }

            return weekDayPath;
        }
        #endregion

        #region 取得 Export/Import用的Region
        /// <summary>
        /// Get Export/Import TransRegion
        /// </summary>
        /// <param name="type">E. Export; I. Import</param>
        private DualResult GetTransRegion(String type, String regionName, String dirName, String rarName, out TransRegion region)
        {
            DualResult result;

            String dataBase = "";
            DataTable getRegion;

            if (type == "E")
            {
                dataBase = "Pms_To_Trade";
            }
            else
            {
                dataBase = "Trade_To_Pms";
            }

            region = null;

            String sqlCmd = "Select @Region as Region, @DirName as DirName, @RarName as RarName, @DBName as DBName, @DBFileName as DBFileName, @ConnectionName as ConnectionName, @Is_Export as Is_Export";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Region", regionName));
            paras.Add(new SqlParameter("@DirName", dirName));
            paras.Add(new SqlParameter("@RarName", rarName));
            paras.Add(new SqlParameter("@DBName", dataBase));
            paras.Add(new SqlParameter("@DBFileName", dataBase));
            paras.Add(new SqlParameter("@ConnectionName", dataBase));
            paras.Add(new SqlParameter("@Is_Export", true));
            result = DBProxy.Current.Select(null, sqlCmd, paras, out getRegion);
            if (!result) { return result; }

            region = new TransRegion(getRegion.Rows[0]);

            return result;
        }
        #endregion
        public virtual Dictionary<string, KeyValuePair<DateTime?, DualResult>> Deploy_LockDate(string procedureDir, string prefix, string connectionName = null)
        {
            var sqlFiles = Directory.GetFiles(procedureDir, prefix + "*.sql");
            Dictionary<string, KeyValuePair<DateTime?, DualResult>> errors = new Dictionary<string, KeyValuePair<DateTime?, DualResult>>();
            SqlConnection conn;
            DualResult result = DBProxy.Current.OpenConnection(connectionName, out conn);
            if (!result)
            {
                errors.Add("Open Connection Fail : " + connectionName,
                   new KeyValuePair<DateTime?, DualResult>(DateTime.Now, result)
                   );
            }
            foreach (string sqlFile in sqlFiles)
            {
                string spname = Path.GetFileNameWithoutExtension(sqlFile);
                result = Deploy_LockDate(sqlFile, conn);
                if (!result)
                {
                    errors.Add(spname,new KeyValuePair<DateTime?, DualResult>(DateTime.Now, result)); 
                }
            }
            return errors;
        }
        public virtual DualResult Deploy_LockDate(string sqlFile, SqlConnection conn)
        {
            string spname = Path.GetFileNameWithoutExtension(sqlFile);
            string dropSP = @"
                    If Object_Id ( 'dbo.@spname@') Is Not Null
                        Drop Procedure dbo.@spname@;
                    ".Replace("@spname@", spname);
            DualResult rr = DBProxy.Current.ExecuteByConn(conn, dropSP);
            string script = File.ReadAllText(sqlFile);
            DualResult result = DBProxy.Current.ExecuteByConn(conn, script);
            return result;
        }
    }
}
