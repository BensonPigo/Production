using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class B12_Layout : Win.Tems.QueryForm
    {
        private string moldID;
        private string strPath;

        /// <inheritdoc/>
        public B12_Layout(string MoldID)
        {
            this.InitializeComponent();
            this.moldID = MoldID;
            this.strPath = MyUtility.GetValue.Lookup("select PadPrintPath from Production.dbo.System");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("SourceFile", header: "File Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
            .Text("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("CreatedBy", header: "Created by", width: Widths.AnsiChars(28), iseditingreadonly: true)
            ;

            #region QueryData
            string sqlcmd = $@"
select *,[CreatedBy] = AddName +' '+ FORMAT(AddDate,'yyyy/MM/dd hh:mm:ss') 
from Clip 
where TableName = 'PadPrintNew_Detail' 
and UniqueKey = '{this.moldID}'
";
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlcmd, out DataTable dt)))
            {
                this.ShowErr(result.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = dt;
            #endregion
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || this.grid == null || this.grid.Rows.Count <= 0)
            {
                return;
            }

            DataRow drSelect = this.grid.GetDataRow(this.listControlBindingSource1.Position);
            string fullFilePath = Path.Combine(this.strPath, drSelect["SourceFile"].ToString());
            fullFilePath.OpenFile();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || this.grid == null || this.grid.Rows.Count <= 0)
            {
                return;
            }

            DataRow dr = this.grid.GetDataRow(this.listControlBindingSource1.Position);
            if (dr == null)
            {
                return;
            }

            if (!Directory.Exists(this.strPath))
            {
                MyUtility.Msg.WarningBox("Please check the path setting.");
                return;
            }

            DirectoryInfo diInfo = new DirectoryInfo(this.strPath);
            if (!diInfo.Exists)
            {
                MyUtility.Msg.WarningBox("Please check the path setting.");
                return;
            }

            FileInfo[] filelist = this.GetFile(diInfo, dr["SourceFile"].ToString()).ToArray();

            // if (!File.Exists(fullpath))
            if (filelist.Length == 0)
            {
                MyUtility.Msg.WarningBox("File not Exists!");
                return;
            }

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = "*|*",
                Title = "Save File",
                FileName = dr["SourceFile"].ToString(),
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.Copy(filelist[0].FullName, saveFileDialog1.FileName, true);
                }
                catch (IOException exception)
                {
                    MyUtility.Msg.ErrorBox("Error: Download file fail. Original error: " + exception.Message);
                }
            }
        }

        private List<FileInfo> GetFile(DirectoryInfo di, string filename)
        {
            List<FileInfo> listFile = new List<FileInfo>();

            foreach (DirectoryInfo sub_di in di.GetDirectories())
            {
                // Call itself to process any sub directories
                listFile.AddRange(this.GetFile(sub_di, filename));
            }

            listFile.AddRange(di.GetFiles().Where(x => x.EqualString(filename)).ToList());

            return listFile;
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string fileName = this.openFileDialog1.SafeFileName;
            string fullFileName = this.openFileDialog1.FileName;

            this.grid.DataSource = null;

            Excel.Application excel = new Excel.Application();
            excel.Workbooks.Open(MyUtility.Convert.GetString(fullFileName));

            excel.Visible = false;

            #region 確認是否有PO ITEM的Sheet

            Excel.Worksheet worksheet = new Excel.Worksheet();
            bool hasPOITEM_Sheet = false;
            for (int i = 1; i <= excel.Sheets.Count; i++)
            {
                Excel.Worksheet tmpSheet = excel.Sheets[i];

                if (tmpSheet.Name == "PO ITEM")
                {
                    hasPOITEM_Sheet = true;
                    worksheet = excel.Sheets[i];
                }

                Marshal.ReleaseComObject(tmpSheet);
            }

            if (!hasPOITEM_Sheet)
            {
                MyUtility.Msg.WarningBox("Could not found sheet <PO ITEM> in Excel.");
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                return;
            }
            #endregion

            #region 確認是否有缺少必要欄位
            int intColumnsCount = worksheet.UsedRange.Columns.Count;

            // 必要欄位
            List<string> mustColumn = new List<string>() { "PO_Number", "PO_Item", "Customer_PO", "AFS_STOCK_CATEGORY", "CABCode", "FinalDestination" };

            // 紀錄必要欄位橫向的欄位位置
            int idx_PO_Number = 0;
            int idx_PO_Item = 0;
            int idx_Customer_PO = 0;
            int idx_AFS_STOCK_CATEGORY = 0;
            int idx_CABCode = 0;
            int idx_FinalDestination = 0;

            for (int x = 1; x <= intColumnsCount; x++)
            {
                var colName = worksheet.Cells[1, x].Value;

                switch (colName)
                {
                    case "PO_Number":
                        idx_PO_Number = x;
                        mustColumn.Remove("PO_Number");
                        break;
                    case "PO_Item":
                        idx_PO_Item = x;
                        mustColumn.Remove("PO_Item");
                        break;
                    case "Customer_PO":
                        idx_Customer_PO = x;
                        mustColumn.Remove("Customer_PO");
                        break;
                    case "AFS_STOCK_CATEGORY":
                        idx_AFS_STOCK_CATEGORY = x;
                        mustColumn.Remove("AFS_STOCK_CATEGORY");
                        break;
                    case "CABCode":
                        idx_CABCode = x;
                        mustColumn.Remove("CABCode");
                        break;
                    case "FinalDestination":
                        idx_FinalDestination = x;
                        mustColumn.Remove("FinalDestination");
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
            DataTable gridData = new DataTable();
            gridData.ColumnsBooleanAdd("Selected");
            gridData.ColumnsStringAdd("PO_Number");
            gridData.ColumnsStringAdd("PO_Item");
            gridData.ColumnsStringAdd("ID");
            gridData.ColumnsStringAdd("CAB");
            gridData.ColumnsStringAdd("FinalDest");
            gridData.ColumnsStringAdd("Customer_PO");
            gridData.ColumnsStringAdd("AFS_STOCK_CATEGORY");

            // 正在讀取的行數，由於第一行是Header，因此起始值為2
            int intRowsReading = 2;

            while (intRowsReading <= intRowsCount)
            {
                // PO_Number
                var pO_Number = worksheet.Cells[intRowsReading, idx_PO_Number].Value;

                // PO_Item
                var pO_Item = worksheet.Cells[intRowsReading, idx_PO_Item].Value;

                // customer_PO
                var customer_PO = worksheet.Cells[intRowsReading, idx_Customer_PO].Value;

                // aFS_STOCK_CATEGORY
                var aFS_STOCK_CATEGORY = worksheet.Cells[intRowsReading, idx_AFS_STOCK_CATEGORY].Value;

                // cABCode
                var cABCode = worksheet.Cells[intRowsReading, idx_CABCode].Value;

                // finalDestination
                var finalDestination = worksheet.Cells[intRowsReading, idx_FinalDestination].Value;

                DataRow nRow = gridData.NewRow();
                nRow["PO_Number"] = pO_Number;
                nRow["PO_Item"] = pO_Item;
                nRow["CAB"] = cABCode;
                nRow["FinalDest"] = finalDestination;
                nRow["Customer_PO"] = customer_PO;
                nRow["AFS_STOCK_CATEGORY"] = aFS_STOCK_CATEGORY;

                string custPONO = pO_Number + "-" + pO_Item;
                string sp = MyUtility.GetValue.Lookup($"SELECT ID FROM Orders WHERE CustPONo = '{custPONO}' ");
                if (MyUtility.Check.Empty(sp))
                {
                    string sqlcmd = string.Empty;
                    DBProxy.Current.Select(null, "select Customize1,Customize2,Customize3 from Brand where ID = 'Nike'", out DataTable dt);
                    foreach (DataColumn c in dt.Columns)
                    {
                        DataRow[] dr = dt.Select($"{c.ColumnName} = 'TRADING PO'");
                        if (dr.Length > 0)
                        {
                            // pO_Item 要比對 CustPONo "-" 後的兩碼
                            sqlcmd = $"SELECT ID FROM Orders WHERE {c.ColumnName} = '{pO_Number}' and IIF(CustPONo not like '%-%','',SUBSTRING(CustPONo, CHARINDEX('-', CustPONo) + 1, 10) ) = '{pO_Item}'";
                            break;
                        }
                    }

                    if (!MyUtility.Check.Empty(sqlcmd))
                    {
                        sp = MyUtility.GetValue.Lookup(sqlcmd);
                    }
                }

                nRow["ID"] = sp;

                if (MyUtility.Check.Empty(sp))
                {
                    nRow["Selected"] = false;
                }
                else
                {
                    nRow["Selected"] = true;
                }

                gridData.Rows.Add(nRow);

                intRowsReading++;
            }

            this.grid.DataSource = gridData;

            excel.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excel);
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            DataTable dataTable;
            if (!MyUtility.Check.Empty(this.grid.DataSource))
            {
                dataTable = (DataTable)this.grid.DataSource;
            }
            else
            {
                return;
            }

            DataRow[] selecteds = dataTable.Select("Selected=1");

            if (selecteds.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first.");
                return;
            }

            bool result = Sci.Production.PPIC.B10.PPIC_B10_BatchUpdate(selecteds);
        }

        private void Grid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (MyUtility.Check.Empty(this.grid.DataSource))
            {
                return;
            }

            DataTable dt = (DataTable)this.grid.DataSource;

            foreach (DataRow item in dt.Rows)
            {
                if (!MyUtility.Check.Empty(item["ID"]))
                {
                    bool old = MyUtility.Convert.GetBool(item["Selected"]);
                    item["Selected"] = !old;
                }
                else
                {
                    item["Selected"] = false;
                }
            }
        }
    }
}
