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

namespace Sci.Production.Class.Command
{
    /// <inheritdoc/>
    public class ProductionEnv
    {
        /// <inheritdoc/>
        public static string lastOpenDir = Directory.Exists(@"\\TsClient\D") ? @"\\TsClient\D" : @"D:\";

        private static bool userDirChanged = false;

        /// <inheritdoc/>
        public const string filter_Excel = "Excel Files|*.xlsx;*.xls;"; // files (*.xls)|*.xls| files (*.xlsx)|*.xlsx";

        /// <inheritdoc/>
        public const string filter_Pdf = "PDF Files|*.pdf;";

        /// <inheritdoc/>
        public Division Division = Command.Division.Local;

        /// <inheritdoc/>
        public string xlt_HC_Carton
        {
            get { return ProductionSystem.Env.XltPathDir + "\\HC_Carton.xlt"; }
        }

        /// <summary>
        /// 此路徑是將Sci.Env.Cfg.XltPathDir轉換為目錄的絕對路徑, 結尾沒有 \
        /// </summary>
        public string XltPathDir;

        /// <inheritdoc/>
        public ProductionEnv()
        {
            this.XltPathDir = new DirectoryInfo(Sci.Env.Cfg.XltPathDir).FullName;
        }

        /// <summary>
        /// 會自動指定前一次的選擇檔案位置，如果沒有指定過，則會用D槽作為預設值(如果是遠端使用者，會以網路磁碟機，自己的D槽作為預設值))
        /// </summary>
        /// <inheritdoc/>
        public static OpenFileDialog GetOpenFileDialog(string filter = "")
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

        /// <inheritdoc/>
        public static SaveFileDialog GetSaveFileDialog(string filter = "")
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

        /// <inheritdoc/>
        public static FolderBrowserDialog GetFolderBrowserDialog(string filter = "")
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

        /// <summary>
        /// 用Season的Month去確認，是否要走CIPF的規則
        /// </summary>
        /// <inheritdoc/>
        [Obsolete("此方法於上線後將逐漸調整，最後捨棄，是過渡時期的方法")]
        public static bool IsApplyNewCipfRule(object brandId, object seasondId)
        {
            var ym = "201809";
            var sql = @"Select Month From Season Where BrandID = @BrandID and ID = @ID";
            var seasonYYYYMM = DBProxy.Current.LookupEx<string>(
                sql,
                "BrandID",
                brandId,
                "ID",
                seasondId).ExtendedData;
            if (seasonYYYYMM == null)
            {
                return false;
            }
            else
            {
                return seasonYYYYMM.CompareTo(ym) >= 0;
            }
        }

        /// <summary>
        /// 用SCISeason的Month去確認，是否要使用NewCDCode的CIPFType
        /// </summary>
        /// <inheritdoc/>
        [Obsolete("此方法於上線後將逐漸調整，最後捨棄，是過渡時期的方法")]
        public static bool IsApplyNewCIPFType(object brandId, object seasondId)
        {
            var ym = "202203";
            var sql = @"Select Month = replace(SeasonSCI.Month,'/','')
From Season
Left join SeasonSCI on season.SeasonSCIID = SeasonSCI.ID
Where Season.BrandID = @BrandID
And Season.ID = @SeasonID";
            var seasonYYYYMM = DBProxy.Current.LookupEx<string>(
                sql,
                "BrandID",
                brandId,
                "SeasonID",
                seasondId).ExtendedData;
            if (seasonYYYYMM == null)
            {
                return false;
            }
            else
            {
                return seasonYYYYMM.CompareTo(ym) >= 0;
            }
        }
    }
}
