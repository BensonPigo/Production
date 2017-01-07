using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;


using Ict;

using Sci.Data;
using Sci.Production.Class;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using SciConvert = Sci.MyUtility.Convert;
using Msg = Sci.MyUtility.Msg;

namespace Sci.Production.Class.Commons
{
    public enum Division { Local, Formal, Mis, Training, Testing }
    public class TradeSystem
    {
        public static TradeEnv Env = new TradeEnv();
        //public static TradeSystem sysData ;
        public static String StandardTms = "";
        public static String AllowRate = "";
        public static String ShipModeID = "";
        public static String SampleRate = "";
        public static String DailyLock = "";
        public static String WKInvoicePlus = "";
        public static String StockMonth = "";
        public static DateTime StockClose;
        public static String SewLock = "";
        public static String PmsLock = "";
        public static String PulloutLock = "";
        public static String RgCode = "";
        public static String SysLock = "";
        public static String CLog = "";
        public static String SuppDlv = "";
        public static String PartsLT = "";
        public static String FileUKey = "";
        public static String OrderTransLocker = "";
        public static String ProphetStartDate = "";
        public static String ProphetSizeCount = "";
        public static String ProphetSingleSizeDeduct = "";
        public static String AccountPath = "";
        public static String ApparelPath = "";
        public static String ClipDir = "";
        public static String DnsPath = "";
        public static String FarbicPath = "";
        public static String PicturePath = "";
        public static String SamplePath = "";
        public static String TempFilePath = "";
        public static String ToPmsPath = "";
        public static String UploadPath = "";
        public static String UmsPath = "";
        public static String ZipBakPath = "";
        public static String Terminal = "";
        public static String ITerminal = "";
        public static String Mailserver = "";
        public static String SendFrom = "";
        public static String EmailID = "";
        public static String EmailPwd = "";
        public static String EmailDesc = "";
        public static String FtpIP = "";
        public static String FtpID = "";
        public static String FtpPwd = "";
        public static String HisBuyer = "";
        public static String HisCdate = "";
        public static String HisSCI = "";
        public static String HisSpno = "";

        public static String Jpg4CuttingDerection = "";
        public static String Jpg4MachineType = "";
        public static String Jpg4MachineGroup = "";
        public static String Jpg4Operation = "";
        public static String AVI4Operation = "";
        public static String PDF4Operation = ""; 
        public static String Jpg4Feature = "";
        public static String Jpg4StyleIE = "";
        public static String Jpg4IETemplate = "";        
        public static String Jpg4Macro = "";
        public static String AVI4Macro = "";
        public static String Jpg4Mold = "";

        public enum Columns
        {
            StandardTms, AllowRate, ShipModeID, SampleRate, DailyLock, WKInvoicePlus, StockMonth, StockClose, SewLock,
            PmsLock, PulloutLock, RgCode, SysLock, CLog, SuppDlv, PartsLT, FileUKey, OrderTransLocker, ProphetStartDate,
            ProphetSizeCount, ProphetSingleSizeDeduct, AccountPath, ApparelPath, ClipDir, DnsPath, FarbicPath, PicturePath,
            SamplePath, TempFilePath, ToPmsPath, UploadPath, UmsPath, ZipBakPath, Terminal, ITerminal, Mailserver, SendFrom,
            EmailID, EmailPwd, EmailDesc, FtpIP, FtpID, FtpPwd, HisBuyer, HisCdate, HisSCI, HisSpno,
            Jpg4CuttingDerection, Jpg4MachineType, Jpg4MachineGroup, Jpg4Operation, AVI4Operation, PDF4Operation,
            Jpg4Feature, Jpg4StyleIE, Jpg4IETemplate, Jpg4Macro, AVI4Macro, Jpg4Mold
        };


        static TradeSystem()
        {
            Reload();
        }
        public static void Reload()
        {
            //  重新load user可用的Brands

            DataTable systemData;
            String sqlCmd = "select * from Trade.dbo.TradeSystem ";
            if (!SQL.Select("", sqlCmd, out systemData))
            {
                return;
            };

            StandardTms = systemData.Rows[0]["StandardTms"].ToString();
            AllowRate = systemData.Rows[0]["AllowRate"].ToString();
            ShipModeID = systemData.Rows[0]["ShipModeID"].ToString();
            SampleRate = systemData.Rows[0]["SampleRate"].ToString();
            DailyLock = systemData.Rows[0]["DailyLock"].ToString();
            WKInvoicePlus = systemData.Rows[0]["WKInvoicePlus"].ToString();
            StockMonth = systemData.Rows[0]["StockMonth"].ToString();
            StockClose = (DateTime)systemData.Rows[0]["StockClose"];
            SewLock = systemData.Rows[0]["SewLock"].ToString();
            PmsLock = systemData.Rows[0]["PmsLock"].ToString();
            PulloutLock = systemData.Rows[0]["PulloutLock"].ToString();
            RgCode = systemData.Rows[0]["RgCode"].ToString();
            SysLock = systemData.Rows[0]["SysLock"].ToString();
            CLog = systemData.Rows[0]["CLog"].ToString();
            SuppDlv = systemData.Rows[0]["SuppDlv"].ToString();
            PartsLT = systemData.Rows[0]["PartsLT"].ToString();
            FileUKey = systemData.Rows[0]["FileUKey"].ToString();
            OrderTransLocker = systemData.Rows[0]["OrderTransLocker"].ToString();
            ProphetStartDate = systemData.Rows[0]["ProphetStartDate"].ToString();
            ProphetSizeCount = systemData.Rows[0]["ProphetSizeCount"].ToString();
            ProphetSingleSizeDeduct = systemData.Rows[0]["ProphetSingleSizeDeduct"].ToString();
            AccountPath = systemData.Rows[0]["AccountPath"].ToString();
            ApparelPath = systemData.Rows[0]["ApparelPath"].ToString();
            ClipDir = systemData.Rows[0]["ClipDir"].ToString();
            DnsPath = systemData.Rows[0]["DnsPath"].ToString();
            FarbicPath = systemData.Rows[0]["FarbicPath"].ToString();
            PicturePath = systemData.Rows[0]["PicturePath"].ToString();
            SamplePath = systemData.Rows[0]["SamplePath"].ToString();
            TempFilePath = systemData.Rows[0]["TempFilePath"].ToString();
            ToPmsPath = systemData.Rows[0]["ToPmsPath"].ToString();
            UploadPath = systemData.Rows[0]["UploadPath"].ToString();
            UmsPath = systemData.Rows[0]["UmsPath"].ToString();
            ZipBakPath = systemData.Rows[0]["ZipBakPath"].ToString();
            Terminal = systemData.Rows[0]["Terminal"].ToString();
            ITerminal = systemData.Rows[0]["ITerminal"].ToString();
            Mailserver = systemData.Rows[0]["Mailserver"].ToString();
            SendFrom = systemData.Rows[0]["SendFrom"].ToString();
            EmailID = systemData.Rows[0]["EmailID"].ToString();
            EmailPwd = systemData.Rows[0]["EmailPwd"].ToString();
            EmailDesc = systemData.Rows[0]["EmailDesc"].ToString();
            FtpIP = systemData.Rows[0]["FtpIP"].ToString();
            FtpID = systemData.Rows[0]["FtpID"].ToString();
            FtpPwd = systemData.Rows[0]["FtpPwd"].ToString();
            HisBuyer = systemData.Rows[0]["HisBuyer"].ToString();
            HisCdate = systemData.Rows[0]["HisCdate"].ToString();
            HisSCI = systemData.Rows[0]["HisSCI"].ToString();
            HisSpno = systemData.Rows[0]["HisSpno"].ToString();

            Jpg4CuttingDerection = systemData.Rows[0]["Jpg4CuttingDerection"].ToString();
            Jpg4MachineType = systemData.Rows[0]["Jpg4MachineType"].ToString();
            Jpg4MachineGroup = systemData.Rows[0]["Jpg4MachineGroup"].ToString();
            Jpg4Operation = systemData.Rows[0]["Jpg4Operation"].ToString();
            AVI4Operation = systemData.Rows[0]["AVI4Operation"].ToString();
            PDF4Operation = systemData.Rows[0]["PDF4Operation"].ToString();
            Jpg4Feature = systemData.Rows[0]["Jpg4Feature"].ToString();
            Jpg4StyleIE = systemData.Rows[0]["Jpg4StyleIE"].ToString();
            Jpg4IETemplate = systemData.Rows[0]["Jpg4IETemplate"].ToString();
            Jpg4Macro = systemData.Rows[0]["Jpg4Macro"].ToString();
            AVI4Macro = systemData.Rows[0]["AVI4Macro"].ToString();
            Jpg4Mold = systemData.Rows[0]["Jpg4Mold"].ToString();

        }

        public static String Get(Columns singleColumn)
        {
            String columnName = singleColumn.ToString();
            DataTable systemData;
            String sqlCmd = "select " + singleColumn + " from dbo.TradeSystem ";
            DBProxy.Current.Select(null, sqlCmd, out systemData);

            var type = typeof(TradeSystem);
            var field = type.GetField(columnName);
            field.SetValue(null, systemData.Rows[0][columnName].ToString());

            return systemData.Rows[0][columnName].ToString();
            //StandardTms = systemData.Rows[0][singleColumn].ToString();
        }

        public static bool Mail(string to, string subject, string description, string from = null, string cc = null, String attachment = null)
        {
            List<String> files = null;
            if (!MyUtility.Check.Empty(attachment))
            {
                files = new List<string>();
                files.Add(attachment);
            }
            return Mail(to, subject, description, from, cc, files);
        }
        public static bool Mail(string to, string subject, string description, string from = null, string cc = null, IEnumerable<String> attachments = null)
        {

            var tos = ParseMail(to);
            if (0 == tos.Count)
            {
                Msg.WarningBox("沒有設定任何收件人");
                return false;
            }

            if (null == from)
            {
                from = Sci.Env.Cfg.MailFrom;
            }

            var ip = Sci.Env.Cfg.MailServerIP; //if (0 == ip.Length) return new DualResult(false, "尚未設定 SMTP 主機");
            var port = Sci.Env.Cfg.MailServerPort;
            var act = Sci.Env.Cfg.MailServerAccount; //if (0 == ip.Length) return new DualResult(false, "尚未設定 SMTP 使用帳號。");
            var pwd = Sci.Env.Cfg.MailServerPassword;

            var ccs = ParseMail(cc);
            bool mailOk = false;

            var mail = new MailMessage();
            try
            {
                mail.From = new MailAddress(from);
                foreach (var it in tos) mail.To.Add(it);
                foreach (var it in ccs) mail.ReplyToList.Add(it);

                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.Body = description;
                mail.BodyEncoding = Encoding.UTF8;

                if (null != attachments)
                {
                    foreach (String fileName in attachments)
                    {
                        mail.Attachments.Add(new Attachment(fileName));
                    }
                }
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = ip;
                    if (port.HasValue && 0 != port) smtp.Port = port.Value;
                    smtp.Credentials = new NetworkCredential(act, pwd);

                    if (smtp.Host.EndsWith(".gmail.com", StringComparison.OrdinalIgnoreCase))
                    {
                        smtp.EnableSsl = true;
                    }

                    smtp.Send(mail);
                }

                mailOk = true;
            }
            catch (Exception ex)
            {
                Msg.ShowException(null, ex);

            }
            mail.Dispose();
            return mailOk;

        }
        private static IList<string> ParseMail(string text)
        {
            var mails = new List<string>();
            if (null != text && 0 < text.Length)
            {
                foreach (var it in text.Split(',', ';'))
                {
                    var mail = it.Trim();
                    if (0 == mail.Length) continue;
                    mails.Add(mail);
                }
            }
            return mails;
        }

        public static String SetTransferLock(bool isStartTransfer)
        {
            String errMsg = "";
            String sqlCmd = "Update dbo.TradeSystem Set OrderTransLocker = @OrderTransLocker";
            List<SqlParameter> paras = new List<SqlParameter>();
            if (isStartTransfer)
            {
                String transLocker = TradeSystem.Get(TradeSystem.Columns.OrderTransLocker);
                if (MyUtility.Check.Empty(transLocker))
                {
                    paras.Add(new SqlParameter("@OrderTransLocker", Sci.Env.User.UserID));
                }
                else
                {
                    errMsg = "Can not Transfer to ORDER ,(" + UserPrg.GetName(transLocker, UserPrg.NameType.idAndNameAndExt) + ") While using !";
                    return errMsg;
                }
            }
            else
            {
                paras.Add(new SqlParameter("@OrderTransLocker", ""));
            }
            DualResult result = DBProxy.Current.Execute(null, sqlCmd, paras);

            if (!result) { return result.ToString(); }

            return "";
        }
    }

    public partial class TradeEnv
    {
        public static String lastOpenDir = Directory.Exists(@"\\TsClient\C") ? @"\\TsClient\C" : @"C:\";
        private static bool userDirChanged = false;
        public const string filter_Excel = "Excel Files|*.xlsx;*.xls;";//files (*.xls)|*.xls| files (*.xlsx)|*.xlsx";

        public Division Division = Commons.Division.Local;

        public string xlt_HC_Carton
        {
            get { return TradeSystem.Env.XltPathDir + "\\HC_Carton.xlt"; }
        }

        /// <summary>
        /// 此路徑是將Sci.Env.Cfg.XltPathDir轉換為目錄的絕對路徑, 結尾沒有 \
        /// </summary>
        public String XltPathDir;
        public TradeEnv()
        {
            this.XltPathDir = new DirectoryInfo(Sci.Env.Cfg.XltPathDir).FullName;
        }

        public static OpenFileDialog GetOpenFileDialog(String filter = "")
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;

            if (userDirChanged)
            {
                return dialog;
            }

            dialog.InitialDirectory = lastOpenDir;
            dialog.FileOk += (s, e) =>
            {
                userDirChanged = true;
            };
            return dialog;

        }

        public static SaveFileDialog GetSaveFileDialog(String filter = "")
        {

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = filter;
            dialog.RestoreDirectory = true;
            if (userDirChanged)
            {
                return dialog;
            }

            dialog.InitialDirectory = lastOpenDir;
            dialog.FileOk += (s, e) =>
            {
                userDirChanged = true;
            };
            return dialog;

        }

        public static FolderBrowserDialog GetFolderBrowserDialog(String filter = "")
        {

            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (userDirChanged)
            {
                return dialog;
            }

            dialog.SelectedPath = lastOpenDir;
            /*dialog.FileOk += (s, e) =>
            {
                userDirChanged = true;
            };*/
            return dialog;

        }
    }
}
