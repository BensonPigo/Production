using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    public partial class P11_ExcelImport : Win.Subs.Base
    {
        private DataTable detailData;
        private DataRow master;
        private DataTable GridData = new DataTable();

        public P11_ExcelImport(DataRow _master, DataTable DetailData)
        {
            this.InitializeComponent();
            this.detailData = DetailData;
            this.master = _master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // DataTable excelFile = new DataTable();
            this.GridData.Columns.Add("ID", typeof(string));
            this.GridData.Columns.Add("Status", typeof(string));
            this.GridData.Columns.Add("PackingListID", typeof(string));
            this.GridData.Columns.Add("CTNStartNO", typeof(string));
            this.GridData.Columns.Add("OrderID", typeof(string));
            this.GridData.Columns.Add("CustPoNo", typeof(string));
            this.GridData.Columns.Add("StyleID", typeof(string));
            this.GridData.Columns.Add("Article", typeof(string));
            this.GridData.Columns.Add("Color", typeof(string));
            this.GridData.Columns.Add("Size", typeof(string));
            this.GridData.Columns.Add("QtyPerCTN", typeof(string));
            this.GridData.Columns.Add("ClogLocationID", typeof(string));

            this.listControlBindingSource1.DataSource = this.GridData;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.IsEditingReadOnly = true;

            #region -- 欄位設定 --
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("Status", header: "Status", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CTNStartNO", header: "CTN#", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("StyleID", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Article", header: "ColorWay", width: Widths.AnsiChars(9), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Size", header: "Size", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("QtyPerCTN", header: "Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .CellClogLocation("ClogLocationID", header: "Location", width: Widths.AnsiChars(8), iseditingreadonly: true);

            #endregion 欄位設定

        }

        private void btnExcelImport_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK) // 開窗且有選擇檔案
            {
                this.ShowWaitMessage("Loading...");
                string filename = this.openFileDialog1.FileName;
                string errMsg = string.Empty;

                #region -- 清空Grid資料
                if (this.GridData != null)
                {
                    this.GridData.Clear();
                }

                this.grid1.SuspendLayout();

                #endregion
                Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                excel.Workbooks.Open(MyUtility.Convert.GetString(filename));
                excel.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                int intColumnsCount = worksheet.UsedRange.Columns.Count;

                // 檢查Excel格式
                Microsoft.Office.Interop.Excel.Range range = worksheet.Range[$"A{1}:AE{1}"];
                object[,] objCellArray = range.Value;
                int[] ItemPosition = new int[intColumnsCount + 1];
                string[] ItemCheck = { string.Empty, "Packing No.", "CTN#" };
                string[] ExcelItem = new string[intColumnsCount + 1];

                for (int y = 1; y <= intColumnsCount; y++)
                {
                    ExcelItem[y] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, y], "C").ToString().ToUpper();
                }

                StringBuilder columnName = new StringBuilder();

                // 確認Excel各Item是否存在，並儲存所在位置
                for (int x = 1; x <= 2; x++)
                {
                    for (int y = 1; y <= intColumnsCount; y++)
                    {
                        if (ExcelItem[y] == ItemCheck[x].ToUpper())
                        {
                            ItemPosition[x] = y;
                            break;
                        }
                    }

                    if (ItemPosition[x] == 0)
                    {
                        columnName.Append("< " + ItemCheck[x].ToString() + " >, ");
                    }
                }

                if (!MyUtility.Check.Empty(columnName.Length))
                {
                    errMsg = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                    MyUtility.Msg.WarningBox(errMsg);
                }
                else
                {
                    int intRowsCount = worksheet.UsedRange.Rows.Count;
                    int intRowsStart = 2;
                    int intRowsRead = intRowsStart - 1;

                    while (intRowsRead < intRowsCount)
                    {
                        intRowsRead++;

                        range = worksheet.Range[$"A{intRowsRead}:AE{intRowsRead}"];
                        objCellArray = range.Value;
                        List<string> listNewRowErrMsg = new List<string>();

                        DataRow newRow = this.GridData.NewRow();
                        newRow["ID"] = this.master["ID"].ToString();
                        newRow["PackingListID"] = (objCellArray[1, ItemPosition[1]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[1]].ToString().Trim(), "C");
                        newRow["CTNStartNO"] = (objCellArray[1, ItemPosition[1]] == null) ? string.Empty : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[2]].ToString().Trim(), "C");

                        newRow = this.StatusInsert(newRow);

                        #region check Columns length
                        List<string> listColumnLengthErrMsg = new List<string>();

                        // PackingListID varchar(13)
                        if (Encoding.Default.GetBytes(newRow["PackingListID"].ToString()).Length > 13)
                        {
                            listColumnLengthErrMsg.Add("<PackingListID> length can't be more than 13 Characters.");
                        }

                        if (Encoding.Default.GetBytes(newRow["CTNStartNO"].ToString()).Length > 6)
                        {
                            listColumnLengthErrMsg.Add("<CTNStartNO> length can't be more than 6 Characters.");
                        }
                        #endregion

                        this.GridData.Rows.Add(newRow);
                    }
                }

                Marshal.ReleaseComObject(worksheet);
                excel.ActiveWorkbook.Close(false, Type.Missing, Type.Missing);
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                excel = null;

                this.grid1.ResumeLayout();
                this.HideWaitMessage();
            }
        }

        private DataRow StatusInsert(DataRow newRow)
        {
            string PackingListID = newRow["PackingListID"].ToString();
            string CTNStartNo = newRow["CTNStartNO"].ToString();

            DataTable dt;
            DualResult result;
            string sqlCmd = $"SELECT * FROM PackingList_Detail WHERE ID='{PackingListID}' AND CTNStartNO='{CTNStartNo}' AND CTNQty = 1 AND QtyPerCTN > 0";
            result = DBProxy.Current.Select(null, sqlCmd, out dt);

            // 1
            if (dt.Rows.Count == 0)
            {
                newRow["Status"] = "Pack ID + CTN# cannot found.";
                return newRow;
            }

            DataRow PackingList_Detail = dt.Rows[0];

            // 2
            if (!MyUtility.Check.Empty(PackingList_Detail["TransferCFADate"]))
            {
                newRow["Status"] = "Catron in CFA.";
            }

            // 3
            if (MyUtility.Check.Empty(PackingList_Detail["ReceiveDate"]))
            {
                newRow["Status"] = "Catron not in Clog.";
            }

            // 4
            if (MyUtility.Check.Seek($"SELECT PulloutID FROM PackingList WHERE ID='{PackingListID}' AND PulloutID  <> '' AND PulloutID IS NOT NULL "))
            {
                newRow["Status"] = "Catron already pullout.";
            }

            string existIData = MyUtility.GetValue.Lookup($"SELECT TOP 1 ID  FROM ClogGarmentDispose_Detail WHERE PackingListID='{PackingListID}' AND CTNStartNO='{CTNStartNo}' AND ID != '{this.master["ID"]}'");

            // 5
            if (!MyUtility.Check.Empty(existIData))
            {
                newRow["Status"] = $"Pack ID + CTN# already exist({existIData}).";
            }

            dt.Clear();
            sqlCmd = $@"  
select  
        pd.ID,
        PD.CTNStartNO,
        pd.OrderID,
        o.CustPoNo,
        o.StyleID,
        pd.Article,
        pd.Color,
        [Size] =  (SELECT Stuff((select  concat( '/',SizeCode)
						from (select distinct pda.SizeCode,osca.Seq
								from PackingList_Detail pda with (nolock)
								inner join Orders o2 with (nolock) on pda.OrderID = o2.ID
						        inner join Order_SizeCode osca with (nolock) on o2.POID = osca.ID and pda.SizeCode = osca.SizeCode
								where pda.ID = pd.ID and pda.CTNStartNO = pd.CTNStartNO ) a  order by Seq
					FOR XML PATH('')),1,1,'')) ,
        [QtyPerCTN] = (SELECT Stuff((select  concat( '/',QtyPerCTN)
						from (select [QtyPerCTN] = sum(pda.QtyPerCTN),osca.Seq
								from PackingList_Detail pda with (nolock)
								inner join Orders o2 with (nolock) on pda.OrderID = o2.ID
						        inner join Order_SizeCode osca with (nolock) on o2.POID = osca.ID and pda.SizeCode = osca.SizeCode
								where pda.ID = pd.ID and pda.CTNStartNO =pd.CTNStartNO group by  pda.SizeCode,osca.Seq ) a  order by Seq
							FOR XML PATH('')),1,1,'')) ,
        pd.ClogLocationID
from PackingList_Detail pd with (nolock) 
left join Orders o with (nolock) on o.ID = pd.OrderID
WHERE pd.ID='{PackingListID}' AND pd.CTNStartNO='{CTNStartNo}'
";
            result = DBProxy.Current.Select(null, sqlCmd, out dt);

            if (dt.Rows.Count > 0)
            {
                if (PackingListID == dt.Rows[0]["ID"].ToString() && CTNStartNo == dt.Rows[0]["CTNStartNO"].ToString())
                {
                    newRow["OrderID"] = dt.Rows[0]["OrderID"].ToString();
                    newRow["CustPoNo"] = dt.Rows[0]["CustPoNo"].ToString();
                    newRow["StyleID"] = dt.Rows[0]["StyleID"].ToString();
                    newRow["Article"] = dt.Rows[0]["Article"].ToString();
                    newRow["Color"] = dt.Rows[0]["Color"].ToString();
                    newRow["Size"] = dt.Rows[0]["Size"].ToString();
                    newRow["QtyPerCTN"] = dt.Rows[0]["QtyPerCTN"].ToString();
                    newRow["ClogLocationID"] = dt.Rows[0]["ClogLocationID"].ToString();
                }
            }

            return newRow;
        }

        private void btnImportDate_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1 == null)
            {
                return;
            }

            DataTable gridDate = (DataTable)this.listControlBindingSource1.DataSource;

            try
            {
                List<DataRow> okDataList = gridDate.AsEnumerable().Where(o => o["Status"].ToString() == string.Empty).ToList();

                foreach (DataRow signleData in okDataList)
                {
                    bool isExistsMasterDetail = this.detailData.AsEnumerable().Where(o => o["PackingListID"].Equals(signleData["PackingListID"]) && o["CTNStartNO"].Equals(signleData["CTNStartNO"])).Any();
                    if (!isExistsMasterDetail)
                    {
                        this.detailData.ImportRow(signleData);
                        gridDate.Rows.Remove(signleData);
                    }
                    else
                    {
                        gridDate.Rows.Remove(signleData);
                    }
                }

                if (okDataList.Count > 0)
                {
                    MyUtility.Msg.InfoBox("Success!");
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }
    }
}
