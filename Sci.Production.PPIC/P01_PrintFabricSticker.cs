using System;
using System.Data;
using System.Drawing;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using System.Linq;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class P01_PrintFabricSticker : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P01_PrintFabricSticker(DataTable data)
        {
            this.InitializeComponent();
            DataTable dtdata = data.AsEnumerable()
            .Where(row => row.Field<string>("Article") != "TTL")
            .CopyToDataTable();
            if (!dtdata.Columns.Contains("Sel"))
            {
                dtdata.Columns.Add("Sel", typeof(int)); // 加欄位
            }

            foreach (DataRow row in dtdata.Rows)
            {
                row["Sel"] = 1; // 全部打勾
            }

            this.gridPrintFabricSticker.DataSource = dtdata;
            this.gridPrintFabricSticker.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridPrintFabricSticker)
            .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("ID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Article", header: "Colorway", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("ColorID", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("XS", header: "XS", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("S", header: "S", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("M", header: "M", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("L", header: "L", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("XL", header: "XL", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("XXL", header: "XXL", width: Widths.AnsiChars(8), iseditingreadonly: true);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtprintData = this.gridPrintFabricSticker.DataSource as DataTable;
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("SP#", typeof(string));
            resultTable.Columns.Add("PO#", typeof(string));
            resultTable.Columns.Add("SIZE", typeof(string));
            resultTable.Columns.Add("QUANTITY", typeof(int));

            // 尺寸欄位名稱清單
            string[] sizes = new[] { "XS", "S", "M", "L", "XL", "XXL" };

            // 篩選 Sel = 1 的列
            var selectedRows = dtprintData.AsEnumerable()
                .Where(row => row.Field<int>("Sel") == 1).ToList();

            if (selectedRows.Count == 0)
            {
                return;
            }

            // 將每筆資料依尺寸展開為 6 筆
            foreach (var row in selectedRows)
            {
                foreach (var size in sizes)
                {
                    // 如果是空的或轉換失敗或無存在sizes清單，預設為 0
                    int qty = 0;
                    if (dtprintData.Columns.Contains(size))
                    {
                        if (int.TryParse(row[size]?.ToString(), out int parsedQty))
                        {
                            qty = parsedQty < 0 ? 0 : parsedQty;
                        }
                    }

                    DataRow newRow = resultTable.NewRow();
                    newRow["SP#"] = row["ID"].ToString();
                    newRow["PO#"] = row["CustPONo"].ToString();
                    newRow["SIZE"] = size;
                    newRow["QUANTITY"] = qty;
                    resultTable.Rows.Add(newRow);
                }
            }

            Word._Application winword = null;
            Word._Document document = null;
            try
            {
                // 初始化 Word 應用程式
                winword = new Word.Application
                {
                    FileValidation = Microsoft.Office.Core.MsoFileValidationMode.msoFileValidationSkip,
                    Visible = false,
                };

                // 顯示word
                // winword.Visible = true;

                // 加載模板文件
                object printFile = Env.Cfg.XltPathDir + "\\Warehouse_P01.dotx";
                document = winword.Documents.Add(ref printFile);

                document.Activate();
                Word.Tables table = document.Tables;

                #region 計算頁數
                winword.Selection.Tables[1].Select();
                winword.Selection.Copy();
                for (int i = 1; i < resultTable.Rows.Count; i++)
                {
                    winword.Selection.MoveDown();
                    if (resultTable.Rows.Count > 1)
                    {
                        winword.Selection.InsertNewPage();
                    }

                    winword.Selection.Paste();
                }
                #endregion

                #region 填入資料
                for (int i = 0; i < resultTable.Rows.Count; i++)
                {
                    DataRow row = resultTable.Rows[i];

                    string contian = $@"SP#: {row["SP#"]}
PO#: {row["PO#"]}
SIZE: {row["SIZE"]}
QUANTITY: {row["QUANTITY"]}";

                    Word.Table tables = table[i + 1];
                    tables.Cell(1, 1).Range.Text = contian;
                    Word.Range cellRange = tables.Cell(1, 1).Range;
                }
                #endregion

                // 打印文件
                PrintDialog pd = new PrintDialog();
                if (pd.ShowDialog() == DialogResult.OK)
                {
                    string printer = pd.PrinterSettings.PrinterName;
                    winword.ActivePrinter = printer;
                    document.PrintOut(Background: false);

                    // 釋放資源
                    if (document != null)
                    {
                        document.Close(false);
                        Marshal.ReleaseComObject(document);
                    }

                    if (winword != null)
                    {
                        winword.Quit(false);
                        Marshal.ReleaseComObject(winword);
                    }

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.ToString());
            }
        }
    }
}
