using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Shipping_B07 : Win.Tems.Input1
    {
        private List<PortByBrandShipmode> portByBrandShipmodeList;
        /// <inheritdoc/>
        public Shipping_B07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.txtcountry.TextBox1.ReadOnly = true;
            this.txtcountry.TextBox1.IsSupportEditMode = false;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            this.txtcountry.TextBox1.Text = MyUtility.GetValue.Lookup($@"SELECT CountryID FROM Port WHERE ID='{this.CurrentMaintain["PortID"]}'");

            bool canEdit = Prgs.GetAuthority(Env.User.UserID, "Shipping B07. Port of Discharge", "CanEdit");

            this.btnImport.Enabled = canEdit && !this.EditMode;

            base.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override void OnDetailDetached()
        {
            base.OnDetailDetached();
            bool canEdit = Prgs.GetAuthority(Env.User.UserID, "Shipping B07. Port of Discharge", "CanEdit");
            this.btnImport.Enabled = canEdit && !this.EditMode;
        }

        private void TxtPort_Leave(object sender, EventArgs e)
        {
            this.CurrentMaintain["CountryID"] = MyUtility.GetValue.Lookup($@"SELECT CountryID FROM Port WHERE ID='{this.CurrentMaintain["PortID"]}'");
            this.txtcountry.TextBox1.Text = MyUtility.Convert.GetString(this.CurrentMaintain["CountryID"]);

            this.CurrentMaintain["ContinentID"] = MyUtility.GetValue.Lookup($@"select c.Continent from Country c INNER JOIN DropDownList d ON d.Type = 'Continent' and d.ID = c.Continent where c.id='{this.txtcountry.TextBox1.Text}' ");
            this.txtContinent.Text = MyUtility.Convert.GetString(this.CurrentMaintain["ContinentID"]);

            this.CurrentMaintain["ContinentName"] = MyUtility.GetValue.Lookup($@"select d.Name from Country c INNER JOIN DropDownList d ON d.Type = 'Continent' and d.ID = c.Continent where c.id='{this.txtcountry.TextBox1.Text}' ");
            this.displayContinent.Text = MyUtility.Convert.GetString(this.CurrentMaintain["ContinentName"]);

            this.chkIsAirPort.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select AirPort from Port where id='{this.CurrentMaintain["PortID"]}'"));

            this.chkIsSeaPort.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup($"select SeaPort from Port where id='{this.CurrentMaintain["PortID"]}'"));
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.portByBrandShipmodeList = new List<PortByBrandShipmode>();
            this.openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            // 開窗且有選擇檔案
            if (this.openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string fileName = this.openFileDialog.SafeFileName;
            string fullFileName = this.openFileDialog.FileName;

            Excel.Application excel = new Excel.Application();
            excel.Workbooks.Open(MyUtility.Convert.GetString(fullFileName));
            Excel.Worksheet worksheet = excel.Sheets[1];

            excel.Visible = false;

            #region 確認是否有缺少必要欄位
            int intColumnsCount = worksheet.UsedRange.Columns.Count;

            // 必要欄位
            List<string> mustColumn = new List<string>() { "Brand", "Continent", "Country", "Port Code", "Air Port", "Sea Port", "Junk" };

            // 紀錄必要欄位橫向的欄位位置
            int idx_Brand = 0;
            int idx_Continent = 0;
            int idx_Country = 0;
            int idx_Port_Code = 0;
            int idx_Air_Port = 0;
            int idx_Sea_Port = 0;
            int idx_Junk = 0;

            for (int x = 1; x <= intColumnsCount; x++)
            {
                var colName = worksheet.Cells[1, x].Value;

                switch (colName)
                {
                    case "Brand":
                        idx_Brand = x;
                        mustColumn.Remove("Brand");
                        break;
                    case "Continent":
                        idx_Continent = x;
                        mustColumn.Remove("Continent");
                        break;
                    case "Country":
                        idx_Country = x;
                        mustColumn.Remove("Country");
                        break;
                    case "Port Code":
                        idx_Port_Code = x;
                        mustColumn.Remove("Port Code");
                        break;
                    case "Air Port":
                        idx_Air_Port = x;
                        mustColumn.Remove("Air Port");
                        break;
                    case "Sea Port":
                        idx_Sea_Port = x;
                        mustColumn.Remove("Sea Port");
                        break;
                    case "Junk":
                        idx_Junk = x;
                        mustColumn.Remove("Junk");
                        break;
                    default:
                        break;
                }
            }

            if (mustColumn.Count > 0)
            {
                string msg = $"Could not found column <{mustColumn.JoinToString(",")}> .";

                MyUtility.Msg.WarningBox(msg);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                return;
            }
            #endregion

            int intRowsCount = worksheet.UsedRange.Rows.Count;

            // 正在讀取的行數，由於第一行是Header，因此起始值為2
            int intRowsReading = 2;

            List<string> notExistsBrand = new List<string>();
            List<string> notExistsPort = new List<string>();
            List<string> notExistsCountry = new List<string>();
            List<string> notExistsContinent = new List<string>();

            while (intRowsReading <= intRowsCount)
            {
                // Brand
                var pBrand = worksheet.Cells[intRowsReading, idx_Brand].Value;

                // Continent
                var pContinent = worksheet.Cells[intRowsReading, idx_Continent].Value;

                // Country
                var pCountry = worksheet.Cells[intRowsReading, idx_Country].Value;

                // Port_Code
                var pPort_Code = worksheet.Cells[intRowsReading, idx_Port_Code].Value;

                // Air_Port
                var pAir_Port = MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Air_Port].Value) == "T" ? true : false;

                // Sea_Port
                var pSea_Port = MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Sea_Port].Value) == "T" ? true : false;

                // Junk
                var pJunk = MyUtility.Convert.GetString(worksheet.Cells[intRowsReading, idx_Junk].Value) == "N" ? false : true;

                // brand, continent, Country, Port 若有空的直接跳過，不進DB驗證
                if (pBrand == null || pPort_Code == null || pContinent == null || pCountry == null ||
                    MyUtility.Check.Empty(pBrand) || MyUtility.Check.Empty(pPort_Code) || MyUtility.Check.Empty(pContinent) || MyUtility.Check.Empty(pCountry))
                {
                    intRowsReading++;
                    continue;
                }

                this.portByBrandShipmodeList.Add(new PortByBrandShipmode()
                {
                    PortID = pPort_Code,
                    BrandID = pBrand,
                    Junk = pJunk,
                });

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Brand", pBrand),
                    new SqlParameter("@Continent", pContinent),
                    new SqlParameter("@Country", pCountry),
                    new SqlParameter("@Port_Code", pPort_Code),
                    new SqlParameter("@Air_Port", pAir_Port),
                    new SqlParameter("@Sea_Port", pSea_Port),
                    new SqlParameter("@Junk", pJunk),
                };

                #region 驗證是否存在

                bool isBrandExists = MyUtility.Check.Seek("SELECT 1 FROM Brand WHERE ID=@Brand", parameters, "Production");
                bool isPortExists = MyUtility.Check.Seek("SELECT 1 FROM Port WHERE ID=@Port_Code ", parameters, "Production");
                bool isCountryExists = MyUtility.Check.Seek("SELECT 1 FROM Country WHERE ID=@Country", parameters, "Production");
                bool isContinentExists = MyUtility.Check.Seek("SELECT 1 FROM DropDownList WHERE Type='Continent' AND ID = @Continent", parameters, "Production");

                if (!isBrandExists)
                {
                    notExistsBrand.Add(pBrand);
                }

                if (!isPortExists)
                {
                    notExistsPort.Add(pPort_Code);
                }

                if (!isCountryExists)
                {
                    notExistsCountry.Add(pCountry);
                }

                if (!isContinentExists)
                {
                    notExistsContinent.Add(pContinent);
                }

                #endregion

                intRowsReading++;
            }

            excel.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excel);

            string errorMsg = "Below data Could not found" + Environment.NewLine;

            if (notExistsBrand.Count > 0)
            {
                errorMsg += "Brand : " + notExistsBrand.JoinToString(",") + Environment.NewLine;
            }

            if (notExistsPort.Count > 0)
            {
                errorMsg += "Port : " + notExistsPort.JoinToString(",") + Environment.NewLine;
            }

            if (notExistsCountry.Count > 0)
            {
                errorMsg += "Country : " + notExistsCountry.JoinToString(",") + Environment.NewLine;
            }

            if (notExistsContinent.Count > 0)
            {
                errorMsg += "Continent : " + notExistsContinent.JoinToString(",") + Environment.NewLine;
            }

            if (notExistsBrand.Count > 0 || notExistsPort.Count > 0 || notExistsCountry.Count > 0 || notExistsContinent.Count > 0)
            {
                MyUtility.Msg.WarningBox(errorMsg);
                return;
            }

            this.MergeDatabase(this.portByBrandShipmodeList);
        }

        private void MergeDatabase(List<PortByBrandShipmode> portByBrandShipmodeList)
        {
            string tmpTable = string.Empty;

            int count = 1;
            foreach (PortByBrandShipmode portByBrandShipmode in portByBrandShipmodeList)
            {
                string tmp = $"SELECT [PortID]='{portByBrandShipmode.PortID}',[BrandID]='{portByBrandShipmode.BrandID}',Junk={(portByBrandShipmode.Junk ? "1" : "0")}";

                tmpTable += tmp + Environment.NewLine;

                if (count == 1)
                {
                    tmpTable += "INTO #source" + Environment.NewLine;
                }

                if (portByBrandShipmodeList.Count > count)
                {
                    tmpTable += "UNION" + Environment.NewLine;
                }

                count++;
            }

            string cmd = $@"
{tmpTable}

MERGE ProductionTPE.dbo.PortByBrandShipmode t 
USING #source s
on t.PortID = s.PortID AND t.BrandID = s.BrandID 
WHEN MATCHED THEN UPDATE SET
	 t.Junk		   =s.Junk
	,t.EditName	   = '{Sci.Env.User.UserID}'
	,t.EditDate	   = GETDATE()
when not matched by target then
	INSERT(PortID,BrandID,Junk,AddName,AddDate)
	VALUES(s.PortID,s.BrandID,s.Junk,'{Sci.Env.User.UserID}',GETDATE())
;

";

            DualResult r = DBProxy.Current.Execute("ProductionTPE", cmd);

            if (!r)
            {
                this.ShowErr(r);
            }
            else
            {
                MyUtility.Msg.InfoBox("Success!!");
                this.ReloadDatas();
            }
        }
    }

    public class PortByBrandShipmode
    {
        public string PortID { get; set; }

        public string BrandID { get; set; }

        public string Remark { get; set; }

        public bool Junk { get; set; }

        public string AddName { get; set; }

        public string EditName { get; set; }

        public DateTime? AddDate { get; set; }

        public DateTime? EditDate { get; set; }
    }
}