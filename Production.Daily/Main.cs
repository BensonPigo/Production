using Sci.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Net;
using System.IO;
using Sci;
using System.Configuration;
using PostJobLog;
using System.Threading;

namespace Production.Daily
{
    public partial class Main : Sci.Win.Tems.Input7
    {
        private string Sftp_Path = @"/SFTP/FactoryFTP/PMS/NewPMS/";
        bool isAuto = false;
        DataRow mailTo;
        TransferPms transferPMS = new TransferPms();
        public int groupID_BeforeTransfer = -99999999;
        public int groupID_AfterTransfer = 99999999;
        string region = string.Empty;
        string tpeMisMail = string.Empty;
        bool isTestJobLog = false;
        bool isSkipRarCheckDate = false;
        Int64 procedureID_Export = 250;
        Int64 procedureID_Import = 251;

        private string sftpIP = string.Empty;
        private string sftpID = string.Empty;
        private string sftpPwd = string.Empty;
        private ushort sftpPort = 0;

        private string importDataPath = string.Empty;
        private string importDataFileName = string.Empty;
        private string exportDataPath = string.Empty;
        private bool isDummy = false;

        public Main()
        {
            InitializeComponent();
            isAuto = false;
            this.chk_export.Checked = true;
            this.chk_import.Checked = true;

        }

        public Main(String _isAuto)
        {
            InitializeComponent();
            if (String.IsNullOrEmpty(_isAuto))
            {
                isAuto = true;
                this.chk_export.Checked = true;
                this.chk_import.Checked = true;
                this.OnFormLoaded();
            }
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region ISP20240102 區分Formal Dummy改變參數
            if (DBProxy.Current.DefaultModuleName.Contains("Dummy"))
            {
                this.isDummy = true;
                this.sftpIP = this.CurrentData["SFtpIPDummy"].ToString();
                this.sftpPort = ushort.Parse(this.CurrentData["SFtpPortDummy"].ToString());
                this.sftpID = this.CurrentData["SFtpIDDummy"].ToString();
                this.sftpPwd = this.CurrentData["SFtpPwdDummy"].ToString();
                this.importDataPath = this.CurrentData["ImportDataPathDummy"].ToString();
                this.importDataFileName = this.CurrentData["ImportDataFileNameDummy"].ToString();
                this.exportDataPath = this.CurrentData["ExportDataPathDummy"].ToString();
            }
            else
            {
                this.isDummy = false;
                this.sftpIP = this.CurrentData["SFtpIP"].ToString();
                this.sftpPort = ushort.Parse(this.CurrentData["SFtpPort"].ToString());
                this.sftpID = this.CurrentData["SFtpID"].ToString();
                this.sftpPwd = this.CurrentData["SFtpPwd"].ToString();
                this.importDataPath = this.CurrentData["ImportDataPath"].ToString();
                this.importDataFileName = this.CurrentData["ImportDataFileName"].ToString();
                this.exportDataPath = this.CurrentData["ExportDataPath"].ToString();
            }
            this.isTestJobLog = this.isDummy;
            #endregion

            if (DBProxy.Current.DefaultModuleName == "PMS_Formal")
            {
                this.isSkipRarCheckDate = true;
            }
            else
            {
                this.isSkipRarCheckDate = false;
            }

            OnRequery();

            transferPMS.FromSystem = "Production";

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

            sqlCmd = "Select * From dbo.MailTo Where ID = '001'";

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out _mailTo);

            if (!result)
            {
                if (this.isAuto)
                {
                    throw result.GetException();
                }
                else
                {
                    ShowErr(result);
                    return;
                }
            }

            mailTo = _mailTo.Rows[0];

            this.tpeMisMail = MyUtility.GetValue.Lookup("Select ToAddress From dbo.MailTo Where ID = '100'");

            editToAddress.Text = mailTo["ToAddress"].ToString();
            editCcAddress.Text = mailTo["CcAddress"].ToString();
            editContent.Text = mailTo["Content"].ToString();

            this.region = MyUtility.GetValue.Lookup("select RgCode from system");
        }

        protected override DualResult ClickSavePost()
        {
            DualResult result;
            String sqlCmd;
            List<SqlParameter> paras = new List<SqlParameter>();
            sqlCmd = "Update dbo.MailTo Set ToAddress = @ToAddress, CcAddress = @CcAddress, Content = @Content Where ID = '001'";


            paras.Add(new SqlParameter("@ToAddress", editToAddress.Text));
            paras.Add(new SqlParameter("@CcAddress", editCcAddress.Text));
            paras.Add(new SqlParameter("@Content", editContent.Text));
            result = DBProxy.Current.Execute(null, sqlCmd, paras);
            if (!result) { return result; }

            sqlCmd = "Update dbo.TransRegion Set RarName = @RarName Where Is_Export = 0";
            paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@RarName", this.importDataFileName));
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
            // MyUtility.FTP
            DualResult result;
            result = transferPMS.SFtp_Ping(Sftp_Path, sftpIP, this.sftpPort, this.sftpID, this.sftpPwd);

            string rarFile = ConfigurationSettings.AppSettings["rarexefile"].ToString();

            if (!result)
            {
                if (this.isAuto)
                {
                    throw result.GetException();
                }
                else
                {
                    ShowErr(result);
                    return;
                }
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
        private void SendMail(String subject = "", String desc = "", bool isFail = true)
        {
            if (!isFail && this.isDummy)
            {
                return;
            }

            String mailServer = this.CurrentData["MailServer"].ToString();
            String eMailID = this.CurrentData["EMailID"].ToString();
            String eMailPwd = this.CurrentData["EMailPwd"].ToString();
            ushort mailServerPort = MyUtility.Check.Empty(this.CurrentData["MailServerPort"]) ? Convert.ToUInt16(25) : Convert.ToUInt16(this.CurrentData["MailServerPort"]);
            transferPMS.SetSMTP(mailServer, mailServerPort, eMailID, eMailPwd);

            String sendFrom = this.CurrentData["SendFrom"].ToString();
            String toAddress = mailTo["ToAddress"].ToString();
            String ccAddress = mailTo["CcAddress"].ToString();

            if (isFail)
            {
                toAddress += MyUtility.Check.Empty(toAddress) ? this.tpeMisMail : ";" + this.tpeMisMail;
            }

            if (this.isDummy)
            {
                toAddress = this.tpeMisMail;
                ccAddress = string.Empty;
            }

            //String toAddress = "willy.wei@sportscity.com.tw";
            //String ccAddress = "";
            if (MyUtility.Check.Empty(toAddress))
            {
                return;
            }

            if (String.IsNullOrEmpty(subject))
            {
                subject = mailTo["Subject"].ToString();
            }
            if (String.IsNullOrEmpty(desc))
            {
                desc = mailTo["Content"].ToString();
            }

            if (this.isDummy)
            {
                subject = "Dummy-" + subject;
            }

            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc, true, true);
            DualResult mailResult = mail.Send();
            if (!mailResult)
            {
                if (this.isAuto)
                {
                    throw mailResult.GetException();
                }
                else
                {
                    this.ShowErr(mailResult);
                }
            }
            //mail.ShowDialog();
        }
        #endregion

        #region Call JobLog web api回傳執行結果
        private void CallJobLogApi(string subject, string desc, string startDate, string endDate, bool isTest, bool succeeded, Int64? procedureID = null)
        {
            JobLog jobLog = new JobLog()
            {
                GroupID = "P",
                SystemID = "PMS",
                Region = this.region,
                MDivisionID = this.region,
                OperationName = subject,
                StartTime = startDate,
                EndTime = endDate,
                Description = desc,
                FileName = new List<string>(),
                FilePath = string.Empty,
                Succeeded = succeeded,
                ProcedureID = isTest ? null : procedureID,
            };
            CallTPEWebAPI callTPEWebAPI = new CallTPEWebAPI(isTest);
            callTPEWebAPI.CreateJobLogAsnc(jobLog, null);
        }

        #endregion

        #region Update/Update動作
        private void ClickExport()
        {
            SqlConnection conn;

            if (!DBProxy.Current.OpenConnection("Production", out conn)) { return; }
            conn.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);

            DualResult result;

            if (isAuto)
            {
                result = AsyncUpdateExport(conn);
            }
            else
            {
                result = AsyncHelper.Current.DataProcess(this, () =>
                {
                    return AsyncUpdateExport(conn);
                });
            }


            if (!result)
            {
                if (this.isAuto)
                {
                    throw result.GetException();
                }
                else
                {
                    ShowErr(result);
                    return;
                }
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

            string exangeDate = DateTime.Now.Hour >= 20 ? DateTime.Now.ToString("yyyy/MM/dd")
                                                        : DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");

            #region 寫入ExchangeDate Table
            this.UpdateExchangeDate(conn, exangeDate, false);
            #endregion
            #region [File Name (with ZIP)]不可為空白
            if (String.IsNullOrEmpty(this.importDataFileName))
            {
                return new DualResult(false, "File Name(with Zip) can not be empty!");
            }
            #endregion
            #region 若[Rg Code]為空，則用[File Name (with ZIP)]的值補上
            if (String.IsNullOrEmpty(this.CurrentData["RgCode"].ToString()))
            {
                int posStart = this.importDataFileName.IndexOf('_') + 1;
                int posEnd = this.importDataFileName.IndexOf('.');

                this.CurrentData["RgCode"] = this.importDataFileName.Substring(posStart, posEnd - posStart);

                String updCmd = String.Format("Update dbo.System Set RgCode = {0}", this.CurrentData["RgCode"].ToString());

                result = DBProxy.Current.ExecuteByConn(conn, updCmd);
                if (!result)
                {
                    return result;
                }
            }
            #endregion
            #region 判斷[Updatae datas path(Taipei)]路徑是否存在
            String importDataPath = this.importDataPath;
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
            String exportDataPath = this.exportDataPath;
            if (exportDataPath.Substring(exportDataPath.Length - 1, 1) != "\\")
            {
                exportDataPath += "\\";
            }
            if (!Directory.Exists(exportDataPath))
            {
                Directory.CreateDirectory(exportDataPath);
            }
            #endregion
            #region 執行前發送通知mail
            subject = "Logon to  Mail Server from " + this.CurrentData["RgCode"].ToString();
            desc = "Logon to  Mail Server from " + this.CurrentData["RgCode"].ToString();
            SendMail(subject, desc, false);
            this.CallJobLogApi("Daily transfer-test", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, true);
            #endregion
            #region CHECK THE FIRST NEED MAPPING A DISK,CAN'T USING \\ UNC
            if (importDataPath.Substring(0, 2) == "////" || exportDataPath.Substring(0, 2) == "////")
            {
                return new DualResult(false, "You can't setup the path using by \\ (UNC),pls mapping for a disk then can do it");
            }
            #endregion

            result = new DualResult(true);
            for (int i = 0; i < 3; i++)
            {

                result = transferPMS.SFtp_Ping(Sftp_Path, this.sftpIP, this.sftpPort, this.sftpID, this.sftpPwd);
                if (result)
                {
                    break;
                }
                Thread.Sleep(2500);
            }

            if (!result)
            {
                this.CallJobLogApi("Daily transfer-test", "FTP, " + result.GetException().ToString(), DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, result);
                return result;
            }

            String exportRgCode = "";
            String importRgCode = "";
            String exportFileName = "";
            String importFileName = "";
            TransRegion exportRegion;
            TransRegion importRegion;

            #region 取得轉出/轉入用的Region
            if (DBProxy.Current.DefaultModuleName == "PMS_Formal")
            {
                exportFileName = "PMS_TEST_REPORTS.RAR";
            }
            else
            {
                exportFileName = this.CurrentData["RgCode"].ToString().Trim() + "_Reports.rar";
            }
            importFileName = this.importDataFileName;

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

            string rarFile = ConfigurationSettings.AppSettings["rarexefile"].ToString();
            if (!File.Exists(rarFile))
            {
                subject = "PMS transfer data (New) ERROR";
                desc = "not found Winrar in your PC ,pls check and re-download.";
                SendMail(subject, desc);
                this.CallJobLogApi("Daily transfer-download data", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, false);
                return new DualResult(false, "Win_RAR File does not exist !");
            }

            startDate = DateTime.Now;

            #region 開始執行轉出
            result = DailyExport(exportRegion, importRegion);

            if (!result)
            {
                ErrMail("Export", transferPMS.Regions_All);
            }
            #endregion
            #region check Export File
            bool checkExportFile = File.Exists(exportRegion.DirName + exportRegion.RarName);
            if (!checkExportFile)
            {
                subject = "PMS transfer data (New) ERROR";
                desc = $"Not found the ZIP(rar) file,pls advice Taipei's Programer: {exportRegion.DirName + exportRegion.RarName}";
                SendMail(subject, desc);
                this.CallJobLogApi("Daily transfer-download data", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, false);
            }
            #endregion
            #region Export Job log 寫入
            bool exportResult = result && checkExportFile;
            string exportDest = !exportResult ? result ? desc : result.Description : string.Empty;
            this.CallJobLogApi("Daily transfer-export", exportDest, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, exportResult, procedureID: this.procedureID_Export);
            #endregion

            #region 開始執行轉入
            result = DailyImport(importRegion);

            #region Import Job log 寫入
            string importDest = !result ? result.Description : string.Empty;
            this.CallJobLogApi("Daily transfer-import", importDest, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, result, procedureID: this.procedureID_Import);
            #endregion

            if (!result)
            {
                ErrMail("Import", transferPMS.Regions_All); //importRegion);
                return result;
            }

            endDate = DateTime.Now;
            this.UpdateExchangeDate(conn, exangeDate, result);
            #endregion

            #region 當Export失敗，這邊就要結束。              
            if (!exportResult)
            {
                return new DualResult(exportResult, exportDest);
            }
            #endregion

            #region check lock date
            checkLockDailyOutput();
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
                //if (MyUtility.Check.Empty(((DateTime)orderComparisonList.Rows[0]["TransferDate"]).ToShortDateString()))
                if (MyUtility.Check.Empty((orderComparisonList.Rows[0]["TransferDate"]).ToString()))
                {
                    transferDate = "DATA IS EMPTY! ";
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
            subject = mailTo["Subject"].ToString().TrimEnd() + " " + this.CurrentData["RgCode"].ToString();
            // 改call system job log api 將資料回傳至台北紀錄
            SendMail(subject, desc, false);
            this.CallJobLogApi("Daily transfer", desc, startDate.ToString("yyyyMMdd HH:mm"), endDate.ToString("yyyyMMdd HH:mm"), isTestJobLog, true);
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
            string title_joblog = string.Format("Daily transfer-{0}", Type.ToLower());
            if (!isAuto) title = "<<手動執行>> " + title;

            SendMail(title, formatStr);
            // this.CallJobLogApi(title_joblog, formatStr, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, false);
            #endregion
        }

        #endregion

        #region Export
        private DualResult DailyExport(TransRegion exportRegion, TransRegion importRegion)
        {
            DualResult result;
            String sqlCmd = "";

            //String ftpIP = this.ftpIP.Trim();
            //String ftpID = this.ftpID.Trim();
            //String ftpPwd = this.ftpPwd.Trim();

            if (this.chk_export.Checked == false)
            {
                return Ict.Result.True;
            }
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
                    //by TransRegion
                    //_fromPath = row["DirName"].ToString();
                    //row["DirName"] = exportRegion.DirName;

                    //by System
                    _fromPath = this.exportDataPath;
                    row["DirName"] = exportRegion.DirName;
                }

                transferPMS.SetupData(transExport);
            }
            #endregion



            #region 檢查FTP檔案的日期是否正確
            bool fileExists = true; // 用來判斷檔案是否存在 importRegion.RarName
            if (isAuto)
            {
                DualResult result1;
                result1 = transferPMS.SFtp_Ping(Sftp_Path, this.sftpIP, this.sftpPort, this.sftpID, this.sftpPwd);
                if (!transferPMS.CheckRar_CreateDate(importRegion, Sftp_Path + importRegion.RarName, isSkipRarCheckDate))
                {
                    fileExists = false;
                    String subject = "PMS transfer data (New) ERROR";
                    String desc = "Wrong the downloaded file date, FileName:(" + importRegion.RarName + ")!!,Pls contact with Taipei.";
                    SendMail(subject, desc);
                    this.CallJobLogApi("Daily transfer-download data", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, false);
                    //return Ict.Result.F("Wrong the downloaded file date, FileName(" + importRegion.RarName + ")!!,Pls contact with Taipei.");
                }
            }
            else
            {
                //手動rar檔路徑改為system.importdatapath
                string sourceFile = importRegion.DirName.ToString() + importRegion.RarName;
                string RaRLastEditDate = File.GetLastWriteTime(sourceFile).ToString("yyyyMMdd");
                string Today = DateTime.Now.ToString("yyyyMMdd");
                if (!File.Exists(sourceFile))
                {
                    fileExists = false;
                    //return new DualResult(false, importRegion.RarName+" Document is not found!!");
                }
                else
                {
                    if (RaRLastEditDate != Today)
                    {
                        fileExists = false;
                        //String subject = "PMS transfer data (New) ERROR";
                        //String desc = "Wrong the downloaded file date!!,Pls Check File(" + importRegion.RarName + ") is New";
                        //SendMail(subject, desc);
                        //return Ict.Result.F("Wrong the downloaded file date!!,Pls Check File(" + importRegion.RarName + ") is New");
                    }
                }
            }

            #endregion
            #region 解壓縮檔案到資料夾裡
            if (fileExists) // 檔案存在才做解壓縮
            {
                if (isAuto)
                {
                    result = transferPMS.SFtp_Ping(Sftp_Path, this.sftpIP, this.sftpPort, this.sftpID, this.sftpPwd);
                    string UnRARpath = Path.Combine(importRegion.DirName);
                    string targetRar = Path.Combine(UnRARpath, importRegion.RarName);
                    //自動的話, 去FTP下載
                    #region 1. 清空已存在的 targetDir 下的檔案
                    var cleanFiles = Directory.GetFiles(UnRARpath);
                    cleanFiles.Select(fileName => transferPMS.Try_DeleteFile(fileName, importRegion)).ToList().All(deleted => deleted);
                    #endregion
                    #region 2. 從FTP下載檔案
                    for (int i = 0; i < Convert.ToInt16(ConfigurationManager.AppSettings["RetryTimes"]); i++)
                    {

                        //string ftp = "ftp://" + Sci.Env.Cfg.FtpServerIP;

                        //if (0 != Sci.Env.Cfg.FtpServerPort)
                        //{
                        //    ftp = ftp + ":" + Sci.Env.Cfg.FtpServerPort;
                        //}

                        if (transferPMS.SFTP_Download(this.Sftp_Path + this.importDataFileName, targetRar))
                        {
                            break;
                        }
                    }
                    #endregion
                    #region 3. 解壓縮
                    DualResult unRARResult = MyUtility.File.UnRARFile(targetRar, true, UnRARpath);//waitRarFinished: true);
                    if (unRARResult)
                    {
                        importRegion.Logs.Add(new KeyValuePair<DateTime?, DualResult>(DateTime.Now, result));
                    }
                    #endregion
                }
                else
                {
                    if (!UnRaR(importRegion))
                    {
                        return new DualResult(false, "rar file Download failed!");
                    }
                }
            }

            #endregion

            #region 刪除原先的壓縮檔
            if (File.Exists(exportRegion.DirName + exportRegion.RarName))
            {
                File.Delete(exportRegion.DirName + exportRegion.RarName);
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



            #region Export_Pms_To_Trade(包含上傳功能)：續傳失敗，等待1.5秒後，在試1次(共5次)，用RetryTimes去判斷次數
            for (int i = 1; i <= Convert.ToInt16(ConfigurationManager.AppSettings["RetryTimes"]); i++)
            {
                if (!transferPMS.Export_Pms_To_Trade(Sftp_Path, sftpIP, sftpID, sftpPwd, _fromPath, exportRegion.DBName, sftpPort))
                {
                    if (i == Convert.ToInt16(ConfigurationManager.AppSettings["RetryTimes"]))
                    {
                        string subject = "PMS Export PMS_To_Trade Error";
                        string desc = "Export PMS_To_Trade failed! Please check the DB TransLog for any error messages.";
                        SendMail(subject, desc, isFail: true);
                        this.CallJobLogApi("Daily transfer Region error", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, succeeded: false);
                        return new DualResult(false, "Export failed!");
                    }
                }
                else
                {
                    break;
                }
                Thread.Sleep(2500);
            }
            #endregion
            return Ict.Result.True;
        }
        #endregion

        #region Import
        private DualResult DailyImport(TransRegion region)
        {
            DualResult result;
            if (this.chk_import.Checked == false)
            {
                return Ict.Result.True;
            }
            #region Setup Data
            DataTable transImport;
            String sqlCmd = $@"Use [Production];
Select	TransRegion.Region
		,TransRegion.DirName
		,TransRegion.RarName
		,TransRegion.Is_Export
		,TransRegion.ConnectionName
		,TransRegion.DBName
		,TransRegion.DBFileName
		,TransImport.GroupID
		,TransImport.Seq
		,TransImport.Name
		,TransImport.TSQL
From dbo.TransRegion 
Left Join dbo.TransImport On TransRegion.ConnectionName = TransImport.ImportConnectionName
Where TransRegion.Is_Export = 0";

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

            //手動執行,才去判斷執行
            if (!isAuto)
            {
                string path = region.DirName;
                string sourceFile = path + region.RarName;
                string RaRLastEditDate = File.GetLastWriteTime(sourceFile).ToString("yyyyMMdd");
                string Today = DateTime.Now.ToString("yyyyMMdd");
                if (!File.Exists(sourceFile))
                {
                    return new DualResult(false, region.RarName + " is not found!!");
                }
                if (RaRLastEditDate != Today)
                {
                    String subject = "PMS transfer data (New) ERROR";
                    String desc = "Wrong the downloaded file date!!,Pls Check File(" + region.RarName + ") is New";
                    SendMail(subject, desc);
                    this.CallJobLogApi("Daily transfer-download data", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, false);
                    return Ict.Result.F("Wrong the downloaded file date!!,Pls Check File(" + region.RarName + ") is New");
                }
            }

            #region 刪除DataBase
            result = transferPMS.DeleteDatabase(region);
            if (!result) { return result; }
            #endregion

            #region 將資料Copy To DB資料夾以掛載
            String fromPath = this.importDataPath;
            String toPath = transImport.Rows[0]["DirName"].ToString();

            //1.清空已存在的檔案 2.從ftp 下載檔案 3.解壓縮
            //transferPMS.UnRAR_To_ImportDir(region);

            //手動執行時,解壓縮RAR檔
            if (!isAuto)
            {
                if (!UnRaR(region))
                {
                    return new DualResult(false, "rar file Download failed!");
                };
            }


            #endregion
            #region 掛載資料庫
            transferPMS.Attach_DataBase(region, 1);
            #endregion

            #region 掛載LockDate 
            Dictionary<string, KeyValuePair<DateTime?, DualResult>> resDic1 = Deploy_LockDate(region.DirName, prefix: "update_", connectionName: transferPMS.FromSystem);
            foreach (var item in resDic1)
            {
                region.Logs.Add(item.Value);
            }

            #endregion

            #region Deploy procedure
            Dictionary<string, KeyValuePair<DateTime?, DualResult>> resDic;
            var deployImportProcedure = transImport.AsEnumerable().Where(s => s["Name"].ToString().StartsWith("imp_"));
            foreach (DataRow installItem in deployImportProcedure)
            {
                resDic = transferPMS.Deploy_Procedure(region.DirName, prefix: installItem["Name"].ToString(), connectionName: installItem["ConnectionName"].ToString());
                foreach (var item in resDic)
                {
                    region.Logs.Add(item.Value);
                }
            }

            #endregion

            #region Region檢核

            string chkSqlcmd = @"
            select 1 from Production.dbo.System s
            where   exists(
	                        select 1 from Trade_To_PMS.dbo.TransRegion t
	                        where RTRIM(s.ImportDataFileName) = RTRIM(t.[FileName])
                        ) or
                    s.ImportDataFileName = 'wt_PMS.rar'
            ;";
            if (!MyUtility.Check.Seek(chkSqlcmd))
            {
                String subject = "PMS Trans Region Error";
                String desc = "Wrong Trans Region!!,Pls Check File(" + region.RarName + ") is correct";
                SendMail(subject, desc);
                this.CallJobLogApi("Daily transfer Region error", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, false);
                return Ict.Result.F("Wrong TransRegion!!,Pls Check File(" + region.RarName + ") is correct");
            }

            #endregion

            #region DateInfo 的 Transfer Date 檢核

            chkSqlcmd = @"
---- Trade_To_PMS.dbo.DateInfo，Name = TransferDate 的資料的DateStart，必須是昨天或今天，是的話才進行後續的動作
select 1 from Trade_To_PMS.dbo.DateInfo 
where Name = 'TransferDate'
AND DateStart in (CAST(DATEADD(DAY,-1,GETDATE()) AS date), CAST(GETDATE() AS DATE))	
;";
            if (!MyUtility.Check.Seek(chkSqlcmd))
            {
                String subject = "PMS Trans Date Error";
                String desc = "The DB transferdate is wrong!!,Pls Check File(" + region.RarName + ") is correct";
                SendMail(subject, desc);
                this.CallJobLogApi("Daily transfer Trans Date error", desc, DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), isTestJobLog, false);
                return Ict.Result.F("Wrong transferdate!!,Pls Check File(" + region.RarName + ") is correct");
            }

            #endregion

            if (!transferPMS.Import_Trade_To_Pms(Sftp_Path, sftpIP, sftpID, sftpPwd))
            {
                return new DualResult(false, "Update failed!");
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
            String ftpIP = @"sftp://" + this.sftpIP.Trim() + @"/";
            String ftpID = this.sftpID.Trim();
            String ftpPwd = this.sftpPwd.Trim();
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
                    errors.Add(spname, new KeyValuePair<DateTime?, DualResult>(DateTime.Now, result));
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
        public bool UnRaR(TransRegion region)
        {

            string destFile = region.DirName + region.RarName;
            string destPath = region.DirName.ToString().Substring(0, region.DirName.Length - 1);

            string UnRARpath = region.DirName.ToString().Substring(0, region.DirName.Length - 1);
            string targetRar = Path.Combine(UnRARpath, region.RarName);

            if (File.Exists(destFile))
            {

                if (MyUtility.Check.Empty(UnRARpath))
                {
                    UnRARpath = targetRar.Substring(0, targetRar.LastIndexOf("\\")) + "\\";
                }
                else
                {
                    if (!System.IO.Directory.Exists(UnRARpath))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(UnRARpath);
                        }
                        catch (System.IO.IOException ex)
                        {
                            return new DualResult(false, "Create directory fail!", ex);
                        }
                    }
                }
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(ConfigurationSettings.AppSettings["rarexefile"].ToString());
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                string argu = (true ? "-o+ " : "") +
                    string.Format(@"x {0} {1}", targetRar, UnRARpath);
                startInfo.Arguments = argu;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(startInfo);

                // 強制等待process 解壓縮完畢
                process.WaitForExit();

            }
            return true;
        }

        //刪除目錄下所有檔案
        private static void DeleteDirectory(string fileName)
        {
            try
            {
                foreach (string document in Directory.GetFileSystemEntries(fileName))
                {
                    if (File.Exists(document))
                    {
                        FileInfo fi = new FileInfo(document);
                        fi.Attributes = FileAttributes.Normal;
                        File.Delete(document);

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void displayBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ForPath = @"D:\SQL_DB\SourceFile\";
            string ToDirPath = @"D:\SQL_DB\";
            string firName = "Test.txt";

            System.IO.FileInfo fi = new System.IO.FileInfo(ForPath + firName);
            fi.CopyTo(ToDirPath + fi.Name);
        }

        private void checkLockDailyOutput()
        {
            string checkLockSQL = @"
use Production
declare @Lockdate date = (select sewlock from Trade_To_Pms.dbo.TradeSystem WITH (NOLOCK))
declare @PullOutLock date = (select PullOutLock from Trade_To_Pms.dbo.TradeSystem WITH (NOLOCK))

select distinct p.*
from Pullout p
inner join Pullout_Detail pd on p.id = pd.ID
where p.PulloutDate <= @PullOutLock
	  and LockDate is null";
            DataTable tableCheckLock;

            DBProxy.Current.Select("", checkLockSQL, out tableCheckLock);

            if (tableCheckLock != null && tableCheckLock.Rows.Count > 0)
            {
                #region Save Excel
                // 儲存路徑
                string path = Sci.Env.Cfg.ReportTempDir;
                string fileName = "Pullout Report is pending Lock - " + this.CurrentData["RgCode"].ToString().Trim() + " - " + DateTime.Now.ToString("yyyyMMdd");
                int lastIndex = 1;

                //判斷 流水號 = ReCode + " Pullout Report " + Date + 流水號 ( 4 碼 )
                while (System.IO.File.Exists(path + fileName + lastIndex.ToString().PadLeft(4, '0') + ".xlsx"))
                {
                    lastIndex++;
                }

                fileName += lastIndex.ToString().PadLeft(4, '0') + ".xlsx";
                // 新增Excel物件
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                // 新增workbook
                Microsoft.Office.Interop.Excel.Workbook workbook = excel.Application.Workbooks.Add(true);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = workbook.Worksheets[1];
                for (int i = 0; i < tableCheckLock.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = tableCheckLock.Columns[i].ColumnName;
                }
                int index = 0;
                foreach (DataRow dr in tableCheckLock.Rows)
                {
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        worksheet.Cells[2 + index, i + 1] = dr[i];
                    }
                    index++;
                }

                if (path.EndsWith("\\"))
                {
                    workbook.SaveCopyAs(path + fileName);
                }
                else
                {
                    workbook.SaveCopyAs(path + "\\" + fileName);
                }

                #endregion
                string subject = "Pullout Report is pending Lock. - " + this.CurrentData["RgCode"].ToString().Trim();
                string desc = "Attached is the data should be Lock but Pullout Report not yet encode.";
                Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(this.CurrentData["SendFrom"].ToString(), "fin-ar@sportscity.com.tw", "", subject, path + "\\" + fileName, desc, true, true);
                DualResult mailResult = mail.Send();
                if (!mailResult)
                {
                    if (this.isAuto)
                    {
                        throw mailResult.GetException();
                    }
                    else
                    {
                        this.ShowErr(mailResult);
                    }
                }
                //mail.ShowDialog();
            }
        }

        /// <summary> 
        /// Download FTP File 檔案續傳
        /// </summary>
        /// <param name="ftpPath">Ftp + FileName</param>
        /// <param name="downloadPath">DownloadPath + FileName</param>
        /// <returns></returns>
        private bool autoDownloadFtpFile(string ftpPath, string downloadPath)
        {
            bool result = false;
            FileStream localfileStream = null;
            Stream responseStream = null;
            try
            {
                FileInfo file = new FileInfo(downloadPath);
                FtpWebRequest request = WebRequest.Create(ftpPath) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(Sci.Env.Cfg.FtpServerAccount, Sci.Env.Cfg.FtpServerPassword);
                if (file.Exists)
                {
                    request.ContentOffset = file.Length;
                }
                using (localfileStream = new FileStream(downloadPath, FileMode.Append, FileAccess.Write))
                {
                    WebResponse response = request.GetResponse();
                    using (responseStream = response.GetResponseStream())
                    {
                        byte[] buffer = new byte[1024];
                        int bytesRead = responseStream.Read(buffer, 0, 1024);
                        while (bytesRead != 0)
                        {
                            localfileStream.Write(buffer, 0, bytesRead);
                            bytesRead = responseStream.Read(buffer, 0, 1024);
                        }
                        localfileStream.Close();
                        responseStream.Close();
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            finally
            {
                if (localfileStream != null)
                    localfileStream.Close();
                if (responseStream != null)
                    responseStream.Close();
            }
            return result;
        }

        private void btnTestWebApi_Click(object sender, EventArgs e)
        {
            this.CallJobLogApi("Daily transfer-API test", "test joblog connection", DateTime.Now.ToString("yyyyMMdd HH:mm"), DateTime.Now.ToString("yyyyMMdd HH:mm"), true, true);
        }

        #region 紀錄每日資料交換結果 for 工廠
        private void UpdateExchangeDate(SqlConnection conn, string exangeDate, bool exangeBolResult)
        {
            int exangeResult = exangeBolResult ? 1 : 0;
            string sqlCmd = $@"
if exists (select 1 
	from DailyDataExchangeResult
	where ExchangeDate = '{exangeDate}'
)
begin
	update d
		 set d.Result = {exangeResult}
			, d.EditDate = iif('{exangeDate}' = format(Getdate(), 'yyyy/MM/dd'), d.EditDate, Getdate())
	from DailyDataExchangeResult d
	where ExchangeDate = '{exangeDate}'
end
else
begin
	insert into DailyDataExchangeResult([ExchangeDate], [Result])
	values('{exangeDate}', {exangeResult})
end
";
            DBProxy.Current.ExecuteByConn(conn, sqlCmd);
        }
        #endregion

        private void btnFileCopy_Click(object sender, EventArgs e)
        {
            DualResult result;
            result = transferPMS.SFtp_Ping(Sftp_Path, this.sftpIP, this.sftpPort, this.sftpID, this.sftpPwd);

            TransRegion importRegion;
            DualResult result1 = GetTransRegion("I", this.importDataFileName, this.importDataPath, this.importDataFileName, out importRegion);
            string targetRar = Path.Combine(this.importDataPath, this.importDataFileName);
            #region 1. 清空已存在的 targetDir 下的檔案
            var cleanFiles = Directory.GetFiles(this.importDataPath);
            cleanFiles.Select(fileName => transferPMS.Try_DeleteFile(fileName, importRegion)).ToList().All(deleted => deleted);
            #endregion
            #region 2. 從FTP下載檔案
            for (int i = 0; i < Convert.ToInt16(ConfigurationManager.AppSettings["RetryTimes"]); i++)
            {
                DualResult dual = MyUtility.SFTP.SFTP_Download(Sftp_Path + this.importDataFileName, targetRar);  // transferPMS.SFTP_Download();
                if (!dual)
                {
                    MyUtility.Msg.WarningBox(dual.ToString());
                    break;
                }
            }
            #endregion
            #region 3. 解壓縮
            DualResult unRARResult = MyUtility.File.UnRARFile(targetRar, true, this.importDataPath);//waitRarFinished: true);
            if (unRARResult)
            {
                MyUtility.Msg.WarningBox(unRARResult.ToString());
                //  importRegion.Logs.Add(new KeyValuePair<DateTime?, DualResult>(DateTime.Now, result));
            }
            #endregion
        }

        private void btnRARCheck_Click(object sender, EventArgs e)
        {
            TransRegion importRegion;
            DualResult result1 = GetTransRegion("I", this.importDataFileName, this.importDataPath, this.importDataFileName, out importRegion);
            if (!transferPMS.CheckRar_CreateDate(importRegion, Sftp_Path + importRegion.RarName, false))
            {
                MyUtility.Msg.WarningBox(importRegion.Logs[0].ToString() + Environment.NewLine + "importRegion.RarName :" + importRegion.RarName + Environment.NewLine + "this.importDataFileName :" + this.importDataFileName + Environment.NewLine + "this.importDataPath:" + this.importDataPath);
                return;
            }
        }
    }
}
