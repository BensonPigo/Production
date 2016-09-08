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
            
            if (isAuto)
            {
                ClickExport();
            }
        }

        private void OnRequery()
        {
            DataTable _mailTo;
            String sqlCmd;
            sqlCmd = "Select * From dbo.MailTo Where ID = '001'";

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

            sqlCmd = "Update dbo.MailTo Set ToAddress = @ToAddress, CcAddress = @CcAddress, Content = @Content Where ID = '001'";
            paras.Add(new SqlParameter("@ToAddress", editToAddress.Text));
            paras.Add(new SqlParameter("@CcAddress", editCcAddress.Text));
            paras.Add(new SqlParameter("@Content", editContent.Text));
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
            ClickExport();
        }

        private void btnTestFTP_Click(object sender, EventArgs e)
        {
            DualResult result;
            result = transferPMS.Ftp_Ping(this.CurrentData["FtpIP"].ToString(), this.CurrentData["FtpID"].ToString(), this.CurrentData["FtpPwd"].ToString());

            if (!result)
            {
                ShowErr(result);
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
            //String toAddress = mailTo["ToAddress"].ToString();
            //String ccAddress = mailTo["CcAddress"].ToString();
            String toAddress = "ben.chen@sportscity.com.tw";
            String ccAddress = "";
            if (String.IsNullOrEmpty(subject))
            {
                subject = mailTo["Subject"].ToString();
            }
            if (String.IsNullOrEmpty(desc))
            {
                desc = mailTo["Content"].ToString();
            }
            Sci.Win.Tools.MailTo mail = new Sci.Win.Tools.MailTo(sendFrom, toAddress, ccAddress, subject, "", desc,true,true);

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
            if (!Directory.Exists(importDataPath))
            {
                Directory.CreateDirectory(importDataPath);
            }
            #endregion
            #region 判斷[Export datas path]路徑是否存在
            String exportDataPath = this.CurrentData["ExportDataPath"].ToString();
            if (!Directory.Exists(exportDataPath))
            {
                Directory.CreateDirectory(exportDataPath);
            }
            #endregion
            #region 判斷[Export datas path]路徑底下的轉入當日的Week
            String weekDayPath = GetWeekDayPath();
            exportDataPath = exportDataPath + weekDayPath;
            if (!Directory.Exists(exportDataPath))
            {
                Directory.CreateDirectory(exportDataPath);
            }
            #endregion
            #region 執行前發送通知mail
            subject = "Logon to  Mail Server from " + this.CurrentData["RgCode"].ToString();
            desc = "Logon to  Mail Server from " + this.CurrentData["RgCode"].ToString();
            SendMail(subject, desc);
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

            DataTable exportRegion;
            String dataBase = "";
            String exportFileName = "";
            String importFileName = "";
            List<SqlParameter> paras;
            TransRegion region;

            dataBase = "Pms_To_Trade";
            exportFileName = this.CurrentData["RgCode"].ToString().Trim() + "_Reports.rar";

            sqlCmd = "Select @Region as Region, @DirName as DirName, @RarName as RarName, @DBName as DBName, @DBFileName as DBFileName, @ConnectionName as ConnectionName, @Is_Export as Is_Export";
            paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Region",this.CurrentData["RgCode"].ToString()));
            paras.Add(new SqlParameter("@DirName", exportDataPath));
            paras.Add(new SqlParameter("@RarName", exportFileName));
            paras.Add(new SqlParameter("@DBName", dataBase));
            paras.Add(new SqlParameter("@DBFileName", dataBase));
            paras.Add(new SqlParameter("@ConnectionName", dataBase));
            paras.Add(new SqlParameter("@Is_Export", true));
            result = DBProxy.Current.SelectByConn(conn, sqlCmd, paras, out exportRegion);
            if (!result) { return result; }

            region = new TransRegion(exportRegion.Rows[0]);

            #region 開始執行轉出
            startDate = DateTime.Now;
            DailyExport(region);
            endDate = DateTime.Now;
            #endregion
            
            DataTable importRegion;
            dataBase = "Trade_To_Pms";
            importFileName = this.CurrentData["ImportDataFileName"].ToString();

            sqlCmd = "Select @Region as Region, @DirName as DirName, @RarName as RarName, @DBName as DBName, @DBFileName as DBFileName, @ConnectionName as ConnectionName, @Is_Export as Is_Export";
            paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@Region", importFileName));
            paras.Add(new SqlParameter("@DirName", importDataPath));
            paras.Add(new SqlParameter("@RarName", importFileName));
            paras.Add(new SqlParameter("@DBName", dataBase));
            paras.Add(new SqlParameter("@DBFileName", dataBase));
            paras.Add(new SqlParameter("@ConnectionName", dataBase));
            paras.Add(new SqlParameter("@Is_Export", true));
            result = DBProxy.Current.SelectByConn(conn, sqlCmd, paras, out importRegion);
            if (!result) { return result; }

            region = new TransRegion(importRegion.Rows[0]);

            #region 開始執行轉入
            DailyImport(region);
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
                transferDate = ((DateTime)orderComparisonList.Rows[0]["TransferDate"]).ToShortDateString();
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
            sqlCmd = "Select 1 as Seq, 'Export' as TransState, Name, DateStart, DateEnd From Pms_To_Trade.dbo.DateInfo" +
                     "Union All" +
                     "Select 2 as Seq, 'Upload' as TransState, Name, DateStart, DateEnd From Trade_To_Pms.dbo.DateInfo" +
                     " Order by Seq";
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

            return Result.True;
        }
        #endregion

        #region Export
        private void DailyExport(TransRegion region)
        {
            String ftpIP = "Ftp://" + this.CurrentData["FtpIP"].ToString().Trim() + "/";
            String ftpID = this.CurrentData["FtpID"].ToString().Trim();
            String ftpPwd = this.CurrentData["FtpPwd"].ToString().Trim();
            
            String exportFileName = this.CurrentData["RgCode"].ToString().Trim() + "_Reports.rar";
            #region 刪除原先的壓縮檔
            if (File.Exists(exportFileName + exportFileName))
            {
                File.Delete(exportFileName + exportFileName);
            }
            #endregion
            /*
            #region 刪除FTP的檔案
            //FtpControl("D", exportFileName);
            tramsferPMS.Delete_Rar_On_Ftp(region);
            #endregion

            #region 刪除所有資料
            tramsferPMS.Drop_Tables(region);
            #endregion
            #region Create [DateInfo]
            tramsferPMS.Create_DateInfo(region);
            #endregion
            */
            transferPMS.Before_Export_ByRegion(region);

            transferPMS.Export_Pms_To_Trade(ftpIP, ftpID, ftpPwd);
            #region 卸載DateBase
            transferPMS.Detach_Database(region);
            #endregion
            #region 壓縮mdf檔案
            List<String> fileList = new List<string>();
            fileList.Add(region.DirName + region.DBFileName + ".mdf");
            transferPMS.RAR_Files(region.DirName, exportFileName, fileList);
            #endregion
            #region 上傳FTP
            FtpControl("U", region.DirName + exportFileName);
            #endregion
        }
        #endregion

        #region Update
        private void DailyImport(TransRegion region)
        {
            String ftpIP = "Ftp://" + this.CurrentData["FtpIP"].ToString().Trim() + "/";
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
            if (!transferPMS.CheckRar_CreateDate(region, region.RarName, false))
            {
                String subject = "PMS transfer data (New) ERROR";
                String desc = "Wrong the downloaded file date!!,Pls contact with Taipei.";
                SendMail(subject, desc);
            }
            #endregion
            #region 刪除DataBase
            transferPMS.DeleteDatabase(region);
            #endregion
            #region 解壓縮檔案到資料夾裡
            transferPMS.UnRAR_To_ImportDir(region);
            #endregion
            #region 掛載資料庫
            transferPMS.Attach_DataBase(region, 1);
            #endregion

            transferPMS.Import_ByGroupID();

        }
        #endregion

        #region FTP
        /// <summary>
        /// Control FTP server file.
        /// </summary>
        /// <param name="type">U. Upload ; D. Delete</param>
        /// <param name="fileName">server file name.</param>
        private void FtpControl(String type, String fileName)
        {
            String ftpIP = "Ftp://" + this.CurrentData["FtpIP"].ToString().Trim() + "/";
            String ftpID = this.CurrentData["FtpID"].ToString().Trim();
            String ftpPwd = this.CurrentData["FtpPwd"].ToString().Trim();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpIP + fileName);
            request.Credentials = new NetworkCredential(ftpID, ftpPwd);
            if (type == "U")
            {
                request.Method = WebRequestMethods.Ftp.UploadFile;
            }
            else if (type == "D")
            {
                request.Method = WebRequestMethods.Ftp.DeleteFile;
            }
            request.Timeout = (60000 * 1);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
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
    }
}
