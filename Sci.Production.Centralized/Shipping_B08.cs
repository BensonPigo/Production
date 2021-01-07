using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Shipping_B08 : Sci.Win.Tems.Input1
    {
        private List<PulloutPort> PulloutPortList;

        /// <inheritdoc/>
        public Shipping_B08(ToolStripMenuItem menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.txtPort.ReadOnly = !this.EditMode;
            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.txtPort.ReadOnly = true;
            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.txtPort.ReadOnly = false;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtPort.Text))
            {
                MyUtility.Msg.WarningBox("Port cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtName.Text))
            {
                MyUtility.Msg.WarningBox("Name cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtcountry.TextBox1.Text))
            {
                MyUtility.Msg.WarningBox("Country cannot be empty!");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            this.txtPort.ReadOnly = true;
            base.ClickSaveAfter();
        }

        private void BtnImportfromExcel_Click(object sender, EventArgs e)
        {
            this.PulloutPortList = new List<PulloutPort>();
            this.openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            // 開窗且有選擇檔案
            if (this.openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string fullFileName = this.openFileDialog.FileName;
            Excel.Application excel = new Excel.Application();
            excel.Workbooks.Open(MyUtility.Convert.GetString(fullFileName));
            Excel.Worksheet worksheet = excel.Sheets[1];
            this.ShowWaitMessage("Starting Import EXCEL...");
            excel.Visible = false;

            #region 確認是否有缺少必要欄位
            int intColumnsCount = worksheet.UsedRange.Columns.Count;

            // 必要欄位
            List<string> mustColumn = new List<string>() { "Port Code", "Port Name", "Country", "Air Port", "Sea Port" };

            // 紀錄必要欄位橫向的欄位位置
            int idx_Port_Code = 0;
            int idx_Port_Name = 0;
            int idx_Country = 0;
            int idx_Air_Port = 0;
            int idx_Sea_Port = 0;
            int idx_International_Code = 0;

            for (int x = 1; x <= intColumnsCount; x++)
            {
                var colName = worksheet.Cells[1, x].Value;

                switch (colName)
                {
                    case "Port Code":
                        idx_Port_Code = x;
                        mustColumn.Remove("Port Code");
                        break;
                    case "Port Name":
                        idx_Port_Name = x;
                        mustColumn.Remove("Port Name");
                        break;
                    case "Country":
                        idx_Country = x;
                        mustColumn.Remove("Country");
                        break;
                    case "Air Port":
                        idx_Air_Port = x;
                        mustColumn.Remove("Air Port");
                        break;
                    case "Sea Port":
                        idx_Sea_Port = x;
                        mustColumn.Remove("Sea Port");
                        break;
                    case "International Code":
                        idx_International_Code = x;
                        break;
                    default:
                        break;
                }
            }

            if (mustColumn.Count > 0)
            {
                string msg = $"Could not found column <{mustColumn.JoinToString(",")}> .";
                this.ShowErr(msg);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                this.HideWaitMessage();
                return;
            }
            #endregion

            int intRowsCount = worksheet.UsedRange.Rows.Count;

            // 正在讀取的行數，由於第一行是Header，因此起始值為2
            int intRowsReading = 2;
            List<string> notExistsCountry = new List<string>();
            List<string> existsID = new List<string>();

            while (intRowsReading < intRowsCount)
            {
                // Port_Code
                var pPort_Code = MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Port_Code].Value);

                // Port_Name
                var pPort_Name = worksheet.Cells[intRowsReading, idx_Port_Name].Value;

                // International_Code
                var pInternational_Code = string.Empty;
                if (idx_International_Code != 0)
                {
                    pInternational_Code = worksheet.Cells[intRowsReading, idx_International_Code].Value;
                }

                // Country
                var pCountry = worksheet.Cells[intRowsReading, idx_Country].Value;

                // Air_Port
                bool pAir_Port = true;
                if ((MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Air_Port].Value) == "T") ||
                    (MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Air_Port].Value) == "Y"))
                {
                    pAir_Port = true;
                }
                else
                {
                    pAir_Port = false;
                }

                // Sea_Port
                bool pSea_Port = true;
                if ((MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Sea_Port].Value) == "T") ||
                    (MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Sea_Port].Value) == "Y"))
                {
                    pSea_Port = true;
                }
                else
                {
                    pSea_Port = false;
                }

                // brand, continent, Country, Port 若有空的直接跳過，不進DB驗證
                if (pPort_Name == null || pPort_Code == null || pCountry == null ||
                    MyUtility.Check.Empty(pPort_Name) || MyUtility.Check.Empty(pPort_Code) || MyUtility.Check.Empty(pCountry))
                {
                    intRowsReading++;
                    continue;
                }

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Country", pCountry),
                };

                #region 驗證是否存在

                // 檢查Country 在Trade是否存在
                bool isCountryExists = MyUtility.Check.Seek("SELECT 1 FROM Country WHERE ID=@Country", parameters, "Trade");

                if (!isCountryExists)
                {
                    notExistsCountry.Add(pCountry);
                }

                // 檢查ID 是否重複
                var isExistsID = from data in this.PulloutPortList
                           where data.PortCode == pPort_Code
                           select data;

                if (isExistsID.Any())
                {
                    existsID.Add(pPort_Code);
                }
                #endregion

                if (isCountryExists && !isExistsID.Any())
                {
                    this.PulloutPortList.Add(new PulloutPort()
                    {
                        PortCode = pPort_Code,
                        PortName = pPort_Name,
                        Country = pCountry,
                        AirPort = pAir_Port,
                        SeaPort = pSea_Port,
                        InternationalCode = pInternational_Code,
                    });
                }

                intRowsReading++;
            }

            excel.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excel);

            this.HideWaitMessage();

            string errorMsg = string.Empty;
            if (notExistsCountry.Count > 0)
            {
                errorMsg += "Below data Could not found" + Environment.NewLine;
                errorMsg += "Country : " + notExistsCountry.JoinToString(",") + Environment.NewLine;
            }

            if (existsID.Count > 0)
            {
                errorMsg += "Cannot duplicate import [Port Code]" + Environment.NewLine;
                errorMsg += "Port Code : " + existsID.JoinToString(",") + Environment.NewLine;
            }

            // 匯入到DB
            this.MergeDB(this.PulloutPortList);

            if (notExistsCountry.Count > 0)
            {
                this.ShowErr(errorMsg);
            }
        }

        private void MergeDB(List<PulloutPort> pulloutPortList)
        {
            string tmpTable = string.Empty;

            if (pulloutPortList == null || pulloutPortList.Count == 0)
            {
                this.ShowErr("No Data Import");
                return;
            }

            string sqlUpdate = $@"
merge ProductionTPE.dbo.PulloutPort as t
using #tmp as s
on Ltrim(s.PortCode) = t.id
when matched then update set
	t.name = s.PortName,
	t.CountryID = upper(s.Country),
	t.AirPort = s.AirPort,
	t.SeaPort = s.SeaPort,	
	t.InternationalCode = iif(s.InternationalCode='',upper(s.PortCode),upper(s.InternationalCode)), -- excel [International]是空的.. 就填入ID
	t.EditDate = GetDate(),
	t.EditName = '{Sci.Env.User.UserID}'
when not matched by target then
	insert (
		id
	   ,name
	   ,CountryID
	   ,AirPort
	   ,SeaPort
	   ,InternationalCode
	   ,AddDate
	   ,AddName
      )
	values (  
	   upper(Ltrim(s.PortCode))
	   ,s.PortName
	   ,upper(s.Country)
	   ,s.AirPort
	   ,s.SeaPort
	   , iif(s.InternationalCode='', upper(s.PortCode), upper(s.InternationalCode))
	   ,GetDate()
	   ,'{Sci.Env.User.UserID}'
	   );

";
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection("ProductionTPE", out sqlConn);
            DataTable dtresult;
            DualResult result;
            if (!(result = MyUtility.Tool.ProcessWithObject(pulloutPortList, string.Empty, sqlUpdate, out dtresult, conn: sqlConn)))
            {
                this.ShowErr(result);
            }
            else
            {
                MyUtility.Msg.InfoBox("import excel successful!");
                this.ReloadDatas();
            }
        }
    }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public class PulloutPort
    {
        /// <inheritdoc/>
        public string PortCode { get; set; }

        /// <inheritdoc/>
        public string PortName { get; set; }

        /// <inheritdoc/>
        public string Country { get; set; }

        /// <inheritdoc/>
        public bool AirPort { get; set; }

        /// <inheritdoc/>
        public bool SeaPort { get; set; }

        /// <inheritdoc/>
        public string InternationalCode { get; set; }
    }
}
