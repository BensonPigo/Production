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
using System.Transactions;

using Ict;

using Sci.Data;
using Sci.Production.Class;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using SciConvert = Sci.MyUtility.Convert;
using Msg = Sci.MyUtility.Msg;
using Sci.Production.Class.Commons;

namespace Sci.Production.Class.Command
{
    /// <inheritdoc/>
    public enum Division
    {
        /// <inheritdoc/>
        Local,

        /// <inheritdoc/>
        Formal,

        /// <inheritdoc/>
        Mis,

        /// <inheritdoc/>
        Training,

        /// <inheritdoc/>
        Testing,

        /// <inheritdoc/>
        Planning
    }

    /// <inheritdoc/>
    public class ProductionSystem
    {
        /// <summary>
        /// 於Login完成後，會記錄相關登入資訊，以供新開視窗時候取用顯示
        /// </summary>
        public static string FormSuffix;

        /// <inheritdoc/>
        public static ProductionEnv Env = new ProductionEnv();

        /// <inheritdoc/>
        public static string StandardTms = string.Empty;

        /// <inheritdoc/>
        public static string AllowRate = string.Empty;

        /// <inheritdoc/>
        public static string ShipModeID = string.Empty;

        /// <inheritdoc/>
        public static string SampleRate = string.Empty;

        /// <inheritdoc/>
        public static string DailyLock = string.Empty;

        /// <inheritdoc/>
        public static string WKInvoicePlus = string.Empty;

        /// <inheritdoc/>
        public static string StockMonth = string.Empty;

        /// <inheritdoc/>
        public static DateTime StockClose;

        /// <inheritdoc/>
        public static string SewLock = string.Empty;

        /// <inheritdoc/>
        public static string PmsLock { get; set; } = string.Empty;

        /// <inheritdoc/>
        public static string PulloutLock = string.Empty;

        /// <inheritdoc/>
        public static string RgCode = string.Empty;

        /// <inheritdoc/>
        public static string SysLock = string.Empty;

        /// <inheritdoc/>
        public static string CLog = string.Empty;

        /// <inheritdoc/>
        public static string SuppDlv = string.Empty;

        /// <inheritdoc/>
        public static string PartsLT = string.Empty;

        /// <inheritdoc/>
        public static string FileUKey = string.Empty;

        /// <inheritdoc/>
        public static string OrderTransLocker = string.Empty;

        /// <inheritdoc/>
        public static string ProphetStartDate = string.Empty;

        /// <inheritdoc/>
        public static string ProphetSizeCount = string.Empty;

        /// <inheritdoc/>
        public static string ProphetSingleSizeDeduct = string.Empty;

        /// <inheritdoc/>
        public static string AccountPath = string.Empty;

        /// <inheritdoc/>
        public static string ApparelPath = string.Empty;

        /// <inheritdoc/>
        public static string ClipDir = string.Empty;

        /// <inheritdoc/>
        public static string DnsPath = string.Empty;

        /// <inheritdoc/>
        public static string FarbicPath = string.Empty;

        /// <inheritdoc/>
        public static string PicturePath = string.Empty;

        /// <inheritdoc/>
        public static string ArtworkPicturePath = string.Empty;

        /// <inheritdoc/>
        public static string SamplePath = string.Empty;

        /// <inheritdoc/>
        public static string TempFilePath = string.Empty;

        /// <inheritdoc/>
        public static string ToPmsPath = string.Empty;

        /// <inheritdoc/>
        public static string UploadPath = string.Empty;

        /// <inheritdoc/>
        public static string UmsPath = string.Empty;

        /// <inheritdoc/>
        public static string ZipBakPath = string.Empty;

        /// <inheritdoc/>
        public static string Terminal = string.Empty;

        /// <inheritdoc/>
        public static string ITerminal = string.Empty;

        /// <inheritdoc/>
        public static string Mailserver = string.Empty;

        /// <inheritdoc/>
        public static string SendFrom = string.Empty;

        /// <inheritdoc/>
        public static string EmailID = string.Empty;

        /// <inheritdoc/>
        public static string EmailPwd = string.Empty;

        /// <inheritdoc/>
        public static string EmailDesc = string.Empty;

        /// <inheritdoc/>
        public static string FtpIP = string.Empty;

        /// <inheritdoc/>
        public static string FtpID = string.Empty;

        /// <inheritdoc/>
        public static string FtpPwd = string.Empty;

        /// <inheritdoc/>
        public static string HisBuyer = string.Empty;

        /// <inheritdoc/>
        public static string HisCdate = string.Empty;

        /// <inheritdoc/>
        public static string HisSCI = string.Empty;

        /// <inheritdoc/>
        public static string HisSpno = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4CuttingDerection = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4MachineType = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4MachineGroup = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4Operation = string.Empty;

        /// <inheritdoc/>
        public static string AVI4Operation = string.Empty;

        /// <inheritdoc/>
        public static string PDF4Operation = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4Feature = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4StyleIE = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4IETemplate = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4Macro = string.Empty;

        /// <inheritdoc/>
        public static string AVI4Macro = string.Empty;

        /// <inheritdoc/>
        public static string Jpg4Mold = string.Empty;

        /// <inheritdoc/>
        public static string StyleFDFilePath = string.Empty;

        /// <inheritdoc/>
        public static string StyleRRLRPath = string.Empty;

        /// <inheritdoc/>
        public static string ColorPath = string.Empty;

        /// <inheritdoc/>
        public enum Columns
        {
            /// <inheritdoc/>
            StandardTms,

            /// <inheritdoc/>
            AllowRate,

            /// <inheritdoc/>
            ShipModeID,

            /// <inheritdoc/>
            SampleRate,

            /// <inheritdoc/>
            DailyLock,

            /// <inheritdoc/>
            WKInvoicePlus,

            /// <inheritdoc/>
            StockMonth,

            /// <inheritdoc/>
            StockClose,

            /// <inheritdoc/>
            SewLock,

            /// <inheritdoc/>
            PmsLock,

            /// <inheritdoc/>
            PulloutLock,

            /// <inheritdoc/>
            RgCode,

            /// <inheritdoc/>
            SysLock,

            /// <inheritdoc/>
            CLog,

            /// <inheritdoc/>
            SuppDlv,

            /// <inheritdoc/>
            PartsLT,

            /// <inheritdoc/>
            FileUKey,

            /// <inheritdoc/>
            OrderTransLocker,

            /// <inheritdoc/>
            ProphetStartDate,

            /// <inheritdoc/>
            ProphetSizeCount,

            /// <inheritdoc/>
            ProphetSingleSizeDeduct,

            /// <inheritdoc/>
            AccountPath,

            /// <inheritdoc/>
            ApparelPath,

            /// <inheritdoc/>
            ClipDir,

            /// <inheritdoc/>
            DnsPath,

            /// <inheritdoc/>
            FarbicPath,

            /// <inheritdoc/>
            PicturePath,

            /// <inheritdoc/>
            ArtworkPicturePath,

            /// <inheritdoc/>
            SamplePath,

            /// <inheritdoc/>
            TempFilePath,

            /// <inheritdoc/>
            ToPmsPath,

            /// <inheritdoc/>
            UploadPath,

            /// <inheritdoc/>
            UmsPath,

            /// <inheritdoc/>
            ZipBakPath,

            /// <inheritdoc/>
            Terminal,

            /// <inheritdoc/>
            ITerminal,

            /// <inheritdoc/>
            Mailserver,

            /// <inheritdoc/>
            SendFrom,

            /// <inheritdoc/>
            EmailID,

            /// <inheritdoc/>
            EmailPwd,

            /// <inheritdoc/>
            EmailDesc,

            /// <inheritdoc/>
            FtpIP,

            /// <inheritdoc/>
            FtpID,

            /// <inheritdoc/>
            FtpPwd,

            /// <inheritdoc/>
            HisBuyer,

            /// <inheritdoc/>
            HisCdate,

            /// <inheritdoc/>
            HisSCI,

            /// <inheritdoc/>
            HisSpno,

            /// <inheritdoc/>
            Jpg4CuttingDerection,

            /// <inheritdoc/>
            Jpg4MachineType,

            /// <inheritdoc/>
            Jpg4MachineGroup,

            /// <inheritdoc/>
            Jpg4Operation,

            /// <inheritdoc/>
            AVI4Operation,

            /// <inheritdoc/>
            PDF4Operation,

            /// <inheritdoc/>
            Jpg4Feature,

            /// <inheritdoc/>
            Jpg4StyleIE,

            /// <inheritdoc/>
            Jpg4IETemplate,

            /// <inheritdoc/>
            Jpg4Macro,

            /// <inheritdoc/>
            AVI4Macro,

            /// <inheritdoc/>
            Jpg4Mold,

            /// <inheritdoc/>
            StyleFDFilePath,

            /// <inheritdoc/>
            StyleRRLRPath,

            /// <inheritdoc/>
            ColorPath,
        }

        static ProductionSystem()
        {
            Reload();
        }

        /// <inheritdoc/>
        public static void Reload()
        {
            // 重新load user可用的Brands
            DataTable systemData;
            string sqlCmd = "select * from Trade.dbo.TradeSystem ";
            if (!SQL.Select(string.Empty, sqlCmd, out systemData))
            {
                return;
            }

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
            ArtworkPicturePath = systemData.Rows[0]["ArtworkPicturePath"].ToString();
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
            StyleFDFilePath = systemData.Rows[0]["StyleFDFilePath"].ToString();
            StyleRRLRPath = systemData.Rows[0]["StyleRRLRPath"].ToString();
            ColorPath = systemData.Rows[0]["ColorPath"].ToString();
        }

        /// <inheritdoc/>
        public static string Get(Columns singleColumn)
        {
            string columnName = singleColumn.ToString();
            DataTable systemData;
            string sqlCmd = "select " + singleColumn + " from dbo.TradeSystem ";
            DBProxy.Current.Select(null, sqlCmd, out systemData);

            var type = typeof(ProductionSystem);
            var field = type.GetField(columnName);
            field.SetValue(null, systemData.Rows[0][columnName].ToString());

            return systemData.Rows[0][columnName].ToString();
        }

        /// <inheritdoc/>
        public static string SetTransferLock(bool isStartTransfer)
        {
            string errMsg = string.Empty;
            string sqlCmd = "Update dbo.TradeSystem Set OrderTransLocker = @OrderTransLocker";
            List<SqlParameter> paras = new List<SqlParameter>();
            if (isStartTransfer)
            {
                string transLocker = ProductionSystem.Get(ProductionSystem.Columns.OrderTransLocker);
                if (MyUtility.Check.Empty(transLocker))
                {
                    paras.Add(new SqlParameter("@OrderTransLocker", Sci.Env.User.UserID));
                }
                else
                {
                    errMsg = "Can not Transfer to ORDER ,(" + UserPrg.GetName(transLocker, UserPrg.NameType.IdAndNameAndExt) + ") While using !";
                    return errMsg;
                }
            }
            else
            {
                paras.Add(new SqlParameter("@OrderTransLocker", string.Empty));
            }

            DualResult result = DBProxy.Current.Execute(null, sqlCmd, paras);

            if (!result)
            {
                return result.ToString();
            }

            return string.Empty;
        }
    }
}
