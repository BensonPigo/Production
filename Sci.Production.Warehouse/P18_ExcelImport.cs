using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class P18_ExcelImport : Sci.Win.Subs.Base
    {
        DataTable grid2Data = new DataTable();
        DataTable detailData;
        DataRow master;
        public P18_ExcelImport(DataRow _master, DataTable DetailData)
        {
            InitializeComponent();
            detailData = DetailData;
            this.master = _master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable ExcelFile = new DataTable();
            ExcelFile.Columns.Add("Filename", typeof(String));
            ExcelFile.Columns.Add("Status", typeof(String));
            ExcelFile.Columns.Add("Count", typeof(String));
            ExcelFile.Columns.Add("FullFileName", typeof(String));

            listControlBindingSource1.DataSource = ExcelFile;
            gridAttachFile.DataSource = listControlBindingSource1;
            gridAttachFile.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.gridAttachFile)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(15))
                .Text("Status", header: "Status", width: Widths.AnsiChars(30))
                .Text("Count", header: "Count", width: Widths.AnsiChars(10))
                ;


            //取Grid結構
            //string sqlCmd = "select SPACE(13) as OrderID, null as BuyerDelivery,SPACE(10) as ShipmodeID,SPACE(8) as Article,SPACE(6) as ColorID,SPACE(8) as SizeCode,0.0 as Qty,SPACE(100) as ErrMsg";
            //DualResult result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            grid2Data.Columns.Add("ID", typeof(String));
            grid2Data.Columns.Add("poid", typeof(String));
            grid2Data.Columns.Add("seq", typeof(String));
            grid2Data.Columns.Add("seq1", typeof(String));
            grid2Data.Columns.Add("seq2", typeof(String));
            grid2Data.Columns.Add("Roll", typeof(String));
            grid2Data.Columns.Add("MDivisionID", typeof(String));
            grid2Data.Columns.Add("Dyelot", typeof(String));
            grid2Data.Columns.Add("stockunit", typeof(String));
            grid2Data.Columns.Add("Description", typeof(String));
            grid2Data.Columns.Add("fabrictype", typeof(String));
            grid2Data.Columns.Add("Weight", typeof(decimal));
            grid2Data.Columns.Add("qty", typeof(decimal));
            grid2Data.Columns.Add("OriQty", typeof(decimal));
            grid2Data.Columns.Add("stocktype", typeof(String));
            grid2Data.Columns.Add("location", typeof(String));
            grid2Data.Columns.Add("Remark", typeof(String));
            grid2Data.Columns.Add("DataFrom", typeof(String));
            grid2Data.Columns.Add("ErrMsg", typeof(String));
            grid2Data.Columns.Add("CanWriteIn", typeof(bool));
            grid2Data.Columns.Add("Fabric", typeof(String));

            listControlBindingSource2.DataSource = grid2Data;
            gridPoid.DataSource = listControlBindingSource2;


            gridPoid.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridPoid)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //1
                .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
                .Text("Fabric", header: "Fabric \r\n Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: false)    //3
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: false)    //4
                .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 7, iseditingreadonly: true)
                .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //6
                .Text("stocktype", header: "Stock Type", iseditingreadonly: true)//7
                .Text("Location", header: "Location", iseditingreadonly: true) //8
                .Text("Remark", header: "Remark", iseditingreadonly: true) //8
                .EditText("ErrMsg", header: "Error Message", width: Widths.AnsiChars(100), iseditingreadonly: true);

            for (int i = 0; i < gridPoid.ColumnCount; i++)
            {
                gridPoid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                gridPoid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        //Add Excel
        private void btnAddExcel_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //開窗且有選擇檔案
            {
                DataRow dr = ((DataTable)listControlBindingSource1.DataSource).NewRow();
                dr["Filename"] = openFileDialog1.SafeFileName;
                dr["Status"] = "";
                dr["FullFileName"] = openFileDialog1.FileName;
                ((DataTable)listControlBindingSource1.DataSource).Rows.Add(dr);
                listControlBindingSource1.MoveLast();
            }
        }

        //Remove Excel
        private void btnRemoveExcel_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.Position != -1)
            {
                listControlBindingSource1.RemoveCurrent();
            }
        }

        //Check & Import
        private void btnCheckImport_Click(object sender, EventArgs e)
        {
            #region -- 判斷第一個Grid是否有資料 --
            if (listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No excel data!!");
                return;
            }
            #endregion

            #region -- 清空Grid2資料
            if (grid2Data != null)
            {
                grid2Data.Clear();
            }
            gridPoid.SuspendLayout();
            #endregion

            /* 檢查1. Grid中的檔案是否存在，不存在時顯示於status欄位 
                 --   2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位
                 --   3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。當檢查都沒問題時，就將資料寫入第2個Grid*/
            #region
            int count = 0;
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Check.Empty(dr["Filename"]))
                {
                    continue;
                }
                if (!System.IO.File.Exists(MyUtility.Convert.GetString(dr["FullFileName"])))
                {
                    dr["Status"] = string.Format("Excel file not found < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                    continue;
                }

                Microsoft.Office.Interop.Excel.Application excel;
                try
                {
                    excel = new Microsoft.Office.Interop.Excel.Application();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    dr["Status"] = string.Format("Not able to open excel file < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                    continue;
                }

                excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                excel.Visible = false;
                Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
                int intColumnsCount = worksheet.UsedRange.Columns.Count;

                if (intColumnsCount >= 30)
                {
                    Marshal.ReleaseComObject(worksheet);
                    excel.ActiveWorkbook.Close(false, Type.Missing, Type.Missing);
                    //excel.Workbooks.Close();
                    excel.Quit();
                    Marshal.ReleaseComObject(excel);
                    excel = null;
                    dr["Status"] = "Column count can not more than 30!!";
                    continue;
                }
                //檢查Excel格式
                Microsoft.Office.Interop.Excel.Range range = worksheet.Range[String.Format("A{0}:AE{0}", 2)];
                object[,] objCellArray = range.Value;
                string[] ItemCheck = { "SP#", "SEQ1", "SEQ2", "Roll", "Dyelot", "G.W(kg)", "Qty", "Stock Type", "Location", "Remark" };
                int[] ItemPosition = new int[ItemCheck.Length];
                string[] ExcelItem = new string[intColumnsCount + 1];


                for (int y = 1; y <= intColumnsCount; y++)
                    ExcelItem[y] = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, y], "C").ToString();


                StringBuilder columnName = new StringBuilder();
                //確認Excel各Item是否存在，並儲存所在位置
                for (int x = 0; x < ItemCheck.Length; x++)
                {
                    for (int y = 1; y <= intColumnsCount; y++)
                    {
                        if (ExcelItem[y] == ItemCheck[x])
                        {
                            ItemPosition[x] = y;
                            break;
                        }
                    }
                    if (MyUtility.Check.Empty(ItemPosition[x])) columnName.Append("< " + ItemCheck[x].ToString() + " >, ");
                }

                if (!MyUtility.Check.Empty(columnName.Length))
                {
                    dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                    Marshal.ReleaseComObject(worksheet);
                    continue;
                }

                int intRowsCount = worksheet.UsedRange.Rows.Count;
                int intRowsStart = 3;
                int intRowsRead = intRowsStart - 1;

                while (intRowsRead < intRowsCount)
                {
                    intRowsRead++;
                    range = worksheet.Range[String.Format("A{0}:Z{0}", intRowsRead)];
                    objCellArray = range.Value;
                    Dictionary<string, bool> listNewRowErrMsg = new Dictionary<string, bool>();

                    DataRow newRow = grid2Data.NewRow();
                    string seq1 = (objCellArray[1, ItemPosition[1]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[1]].ToString().Trim(), "C").ToString();
                    string seq2 = (objCellArray[1, ItemPosition[2]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[2]].ToString().Trim(), "C").ToString();
                    string stockType = (objCellArray[1, ItemPosition[7]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[7]].ToString().Replace("'", "").Trim(), "C").ToString();

                    switch (stockType)
                    {
                        case "Bulk":
                            stockType = "B";
                            break;
                        case "Inventory":
                            stockType = "I";
                            break;
                        default:
                            listNewRowErrMsg.Add("<Stock Type> can only be [Bulk] or [Inventory]", false);
                            break;
                    }

                    newRow["poid"] = (objCellArray[1, ItemPosition[0]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[0]].ToString().Trim(), "C");
                    newRow["seq"] = seq1 + " " + seq2;
                    newRow["seq1"] = seq1;
                    newRow["seq2"] = seq2;
                    newRow["Roll"] = (objCellArray[1, ItemPosition[3]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[3]].ToString().Trim(), "C");
                    newRow["Dyelot"] = (objCellArray[1, ItemPosition[4]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[4]].ToString().Trim(), "C").ToString();
                    newRow["Weight"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[5]], "N");
                    newRow["qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[6]], "N");
                    newRow["stocktype"] = stockType;
                    newRow["location"] = (objCellArray[1, ItemPosition[8]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[8]].ToString().Replace("'", "").Trim(), "C");
                    newRow["Remark"] = (objCellArray[1, ItemPosition[9]] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, ItemPosition[9]].ToString().Trim(), "C");
                    newRow["CanWriteIn"] = true;
                    #region check Columns length
                    List<string> listColumnLengthErrMsg = new List<string>();

                    // Poid varchar(13)
                    if (Encoding.Default.GetBytes(newRow["poid"].ToString()).Length > 13)
                        listColumnLengthErrMsg.Add("<SP#> length can't be more than 13 Characters.");

                    // Seq1 varchar(3)
                    if (Encoding.Default.GetBytes(newRow["Seq1"].ToString()).Length > 3)
                        listColumnLengthErrMsg.Add("<SEQ1> length can't be more than 3 Characters.");

                    // Seq2 varchar(2)
                    if (Encoding.Default.GetBytes(newRow["Seq2"].ToString()).Length > 2)
                        listColumnLengthErrMsg.Add("<SEQ2> length can't be more than 2 Characters.");

                    // Roll varchar(8)
                    if (Encoding.Default.GetBytes(newRow["Roll"].ToString()).Length > 8)
                        listColumnLengthErrMsg.Add("<Roll> length can't be more than 8 Characters.");

                    // Dyelot varchar(8)
                    if (Encoding.Default.GetBytes(newRow["Dyelot"].ToString()).Length > 8)
                        listColumnLengthErrMsg.Add("<Dyelot> length can't be more than 8 Characters.");

                    // Weight  numeric (11, 2)
                    if (decimal.Parse(newRow["Weight"].ToString()) > 9999999)
                        listColumnLengthErrMsg.Add("<G.W(kg)> value can't be more than 9,999,999");

                    // qty  numeric (11, 2)
                    if (decimal.Parse(newRow["qty"].ToString()) > 999999999)
                        listColumnLengthErrMsg.Add("<Qty> value can't be more than 999,999,999");

                    // StockType varchar(1)
                    if (Encoding.Default.GetBytes(newRow["stocktype"].ToString()).Length > 1)
                        listColumnLengthErrMsg.Add("<StockType> length can't be more than 1 Characters.");

                    // Location varchar(60)
                    if (Encoding.Default.GetBytes(newRow["Location"].ToString()).Length > 60)
                        listColumnLengthErrMsg.Add("<Location> length can't be more than 60 Characters.");

                    // Remark nvarchar(100)
                    if (Encoding.Default.GetBytes(newRow["Remark"].ToString()).Length > 100)
                        listColumnLengthErrMsg.Add("<Remark> length can't be more than 100 Characters.");

                    if (listColumnLengthErrMsg.Count > 0)
                    {
                        listNewRowErrMsg.Add(listColumnLengthErrMsg.JoinToString(Environment.NewLine), false);
                    }
                    #endregion

                    #region P18表身檢查
                    if (MyUtility.Check.Empty(newRow["seq1"]) || MyUtility.Check.Empty(newRow["seq2"]))
                    {
                        listNewRowErrMsg.Add(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty", newRow["poid"], newRow["seq1"], newRow["seq2"]), false);
                    }

                    if (MyUtility.Check.Empty(newRow["Qty"]))
                    {
                        listNewRowErrMsg.Add(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Transfer In Qty can't be empty"
                            , newRow["poid"], newRow["seq1"], newRow["seq2"], newRow["roll"], newRow["dyelot"]), false);
                    }


                    // POID
                    string dataFrom = "Po_Supp_Detail";

                    DualResult checkResult = P18_Utility.CheckDetailPOID(newRow["poid"].ToString(), this.master["FromFtyID"].ToString(), out dataFrom);

                    if (!checkResult)
                    {
                        listNewRowErrMsg.Add(checkResult.Description, false);
                    }
                    else
                    {
                        newRow["DataFrom"] = dataFrom;
                    }

                    // Seq 
                    // 順便取得Fabric type
                    checkResult = P18_Utility.CheckDetailSeq(newRow["seq"].ToString(), this.master["FromFtyID"].ToString(), newRow);
                    if (!checkResult)
                    {
                        listNewRowErrMsg.Add(checkResult.Description, false);
                    }

                    // StockType
                    string newLocation = string.Empty;
                    checkResult = P18_Utility.CheckDetailStockTypeLocation(newRow["StockType"].ToString(), newRow["Location"].ToString(), out newLocation);
                    if (!checkResult)
                    {
                        listNewRowErrMsg.Add(checkResult.Description, true);
                    }
                    newRow["Location"] = newLocation;

                    checkResult = P18_Utility.CheckRollExists(master["id"].ToString(), newRow);

                    if (!checkResult)
                    {
                        listNewRowErrMsg.Add(checkResult.Description, false);
                    }

                    #endregion

                    if (listNewRowErrMsg.Count == 0)
                    {
                        count++;
                    }
                    else
                    {
                        newRow["ErrMsg"] = listNewRowErrMsg.Select(s => s.Key).JoinToString(Environment.NewLine);
                    }

                    bool canNotWriteIn = listNewRowErrMsg.Any(s => s.Value == false);
                    if(canNotWriteIn)
                    {
                        newRow["CanWriteIn"] = false;
                    }


                    newRow["OriQty"] = 0;
                    newRow["MDivisionID"] = Sci.Env.User.Keyword;

                    grid2Data.Rows.Add(newRow);
                }

                dr["Status"] = (intRowsCount - 2 == count) ? "Check & Import Completed." : "Some Data Faild. Please check Error Message.";
                dr["Count"] = count;


                Marshal.ReleaseComObject(worksheet);
                excel.ActiveWorkbook.Close(false, Type.Missing, Type.Missing);
                //excel.Workbooks.Close();
                excel.Quit();
                Marshal.ReleaseComObject(excel);
                excel = null;


            }
            #endregion

            gridPoid.ResumeLayout();
            foreach (DataGridViewRow dr in gridPoid.Rows)
            {
                if (!dr.Cells["ErrMsg"].Value.Empty())
                {
                    dr.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        //Write in
        private void btnWriteIn_Click(object sender, EventArgs e)
        {
            var tmpPacking = ((DataTable)listControlBindingSource2.DataSource).AsEnumerable();

            //如果資料中有錯誤不能WriteIn
            if (tmpPacking.Any(s => (bool)s["CanWriteIn"] == false))
            {
                MyUtility.Msg.WarningBox("Import data error, please check column [Error Message] information to fix Excel.");
                return;
            }

            try
            {
                var q = from p in tmpPacking
                        group p by new
                        {
                            poid = p.Field<string>("poid"),
                            seq1 = p.Field<string>("seq1"),
                            seq2 = p.Field<string>("seq2"),
                            Roll = p.Field<string>("Roll"),
                            Dyelot = p.Field<string>("Dyelot")
                        } into m
                        where m.Count() > 1 //只顯示超過一次以上的
                        select new
                        {
                            poid = m.First().Field<string>("poid"),
                            seq1 = m.First().Field<string>("seq1"),
                            seq2 = m.First().Field<string>("seq2"),
                            Roll = m.First().Field<string>("Roll"),
                            Dyelot = m.First().Field<string>("Dyelot"),
                            count = m.Count()
                        };
                if (q.ToList().Count > 0)
                {
                    string warning = "";

                    foreach (var dr in q)
                    {
                        warning += string.Format("{0}-{1}-{2}-{3}-{4}" + Environment.NewLine, dr.poid, dr.seq1, dr.seq2, dr.Roll, dr.Dyelot);
                    }
                    MyUtility.Msg.WarningBox(warning, "Roll# are duplicated!!");
                    return;
                }

                foreach (DataRow dr2 in tmpPacking)
                {
                    //刪除 Import 重複的資料 by SP# Seq Carton#
                    DataRow[] checkRow = detailData.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["poid"].EqualString(dr2["poid"])
                                                                                && row["seq1"].EqualString(dr2["seq1"]) && row["seq2"].EqualString(dr2["seq2"])
                                                                                && row["roll"].EqualString(dr2["roll"]) && row["Dyelot"].EqualString(dr2["Dyelot"])).ToArray();
                    // 開始檢查FtyInventory
                    string poid = MyUtility.Convert.GetString(dr2["poid"]);
                    string seq1 = MyUtility.Convert.GetString(dr2["seq1"]);
                    string seq2 = MyUtility.Convert.GetString(dr2["seq2"]);
                    string roll = MyUtility.Convert.GetString(dr2["roll"]);
                    string dyelot = MyUtility.Convert.GetString(dr2["dyelot"]);
                    string fabricType = MyUtility.Convert.GetString(dr2["fabrictype"]);
                    string stockType = MyUtility.Convert.GetString(dr2["stockType"]);


                    // 布料，且都有值了才檢查該Row
                    if (fabricType.ToUpper() == "F" && !MyUtility.Check.Empty(poid) && !MyUtility.Check.Empty(seq1) && !MyUtility.Check.Empty(seq2) && !MyUtility.Check.Empty(roll) && !MyUtility.Check.Empty(dyelot))
                    {
                        bool ChkFtyInventory = PublicPrg.Prgs.ChkFtyInventory(poid, seq1, seq2, roll, dyelot, stockType);

                        if (!ChkFtyInventory)
                        {
                            MyUtility.Msg.WarningBox($"The Roll & Deylot of <SP#>:{poid}, <Seq>:{seq1} {seq2}, <Roll>:{roll}, <Dyelot>:{dyelot} already exists.");

                            return;
                        }
                    }


                    if (checkRow.Length == 0)
                    {
                        dr2["id"] = master["id"];
                        detailData.ImportRow(dr2);
                    }
                }
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Process error.\r\n" + ex.ToString());
                return;
            }


            MyUtility.Msg.InfoBox("Write in completed!!");
            DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        private void BtnDownloadTempExcel_Click(object sender, EventArgs e)
        {
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P18_ExcelImport.xltx"); //預先開啟excel app
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P18_ExcelImport");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
        }
    }
}
