using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Centralized
{
    /// <inheritdoc/>
    public partial class Shipping_B08 : Sci.Win.Tems.Input1
    {
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


            DataTable excelDataTable;
            DualResult result;
            string sqlCmd = @"select id, name ,CountryID,AirPort,SeaPort,InternationalCode from PulloutPort where 1=0";
            result = DBProxy.Current.Select("ProductionTPE", sqlCmd, out excelDataTable);

            this.ShowWaitMessage("Starting Import EXCEL...");

            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intColumnsCount = worksheet.UsedRange.Columns.Count;

            // 必要欄位
            List<string> mustColumn = new List<string>() { "Country", "Port Name", "Port Code", "Air Port", "Sea Port", "International Code" };

            // 紀錄必要欄位橫向的欄位位置
            int idx_Brand = 0;
            int idx_Continent = 0;
            int idx_Country = 0;
            int idx_Port_Code = 0;
            int idx_Air_Port = 0;
            int idx_Sea_Port = 0;

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
                return;
            }
            #endregion

            int intRowsCount = worksheet.UsedRange.Rows.Count;
            int intRowsStart = 2;
            int intRowsRead = intRowsStart - 1;

            Microsoft.Office.Interop.Excel.Range range;
            object[,] objCellArray;
            int AirPort = 0;
            int SeaPort = 0;

            while (intRowsRead < intRowsCount)
            {
                intRowsRead++;
                range = worksheet.Range[string.Format("A{0}:F{0}", intRowsRead)];
                objCellArray = range.Value;
                DataRow newRow = excelDataTable.NewRow();
                newRow["id"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                newRow["name"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                newRow["CountryID"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");

                if ((MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C").ToString() == "Y") ||
                    (MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C").ToString() == "T"))
                {
                    AirPort = 1;
                }
                else
                {
                    AirPort = 0;
                }

                if ((MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C").ToString() == "Y") ||
                    (MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C").ToString() == "T"))
                {
                    SeaPort = 1;
                }
                else
                {
                    SeaPort = 0;
                }

                newRow["AirPort"] = AirPort;
                newRow["SeaPort"] = SeaPort;
                newRow["InternationalCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");
                excelDataTable.Rows.Add(newRow);
            }

            excel.Workbooks.Close();
            excel.Quit();
            excel = null;

            this.HideWaitMessage();

            // 匯入到DB
            string sqlUpdate = $@"
merge ProductionTPE.dbo.PulloutPort as t
using (
	select t1.* from #tmp t1
	inner join Trade.dbo.Country c on t1.CountryID = c.id
) as s
on Ltrim(s.id) = t.id
when matched then update set
	t.name = s.name,
	t.CountryID = s.CountryID,
	t.AirPort = s.AirPort,
	t.SeaPort = s.SeaPort,	
	t.InternationalCode = iif(s.InternationalCode='',s.id,s.InternationalCode), -- excel [International]是空的.. 就填入ID
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
	   Ltrim(s.[ID])
	   ,s.name
	   ,s.CountryID
	   ,s.AirPort
	   ,s.SeaPort
	   ,iif(s.InternationalCode='',s.id,s.InternationalCode)
	   ,GetDate()
	   ,'{Sci.Env.User.UserID}'
	   );

";
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection("ProductionTPE", out sqlConn);
            DataTable dtresult;
            if (!(result = MyUtility.Tool.ProcessWithDatatable(excelDataTable, string.Empty, sqlUpdate, out dtresult, conn: sqlConn)))
            {
                this.ShowErr(result);
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("import excel successful!");
                this.ReloadDatas();
            }
        }
    }
}
