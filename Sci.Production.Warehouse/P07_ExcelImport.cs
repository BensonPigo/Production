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

namespace Sci.Production.Warehouse
{
    public partial class P07_ExcelImport : Sci.Win.Subs.Base
    {
        DataTable grid2Data = new DataTable();
        DataTable detailData;
        DataRow master;
        public P07_ExcelImport(DataRow _master, DataTable DetailData)
        {
            InitializeComponent();
            detailData = DetailData;
            master = _master;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable ExcelFile = new DataTable();
            ExcelFile.Columns.Add("Filename", typeof(String));
            ExcelFile.Columns.Add("Status", typeof(String));
            ExcelFile.Columns.Add("FullFileName", typeof(String));

            listControlBindingSource1.DataSource = ExcelFile;
            grid1.DataSource = listControlBindingSource1;
            grid1.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid1)
                .Text("Filename", header: "File Name", width: Widths.AnsiChars(15))
                .Text("Status", header: "Status", width: Widths.AnsiChars(100));

            //取Grid結構
            //string sqlCmd = "select SPACE(13) as OrderID, null as BuyerDelivery,SPACE(10) as ShipmodeID,SPACE(8) as Article,SPACE(6) as ColorID,SPACE(8) as SizeCode,0.0 as Qty,SPACE(100) as ErrMsg";
            //DualResult result = DBProxy.Current.Select(null, sqlCmd, out grid2Data);
            grid2Data.Columns.Add("wkno", typeof(String));
            grid2Data.Columns.Add("poid", typeof(String));
            grid2Data.Columns.Add("seq1", typeof(String));
            grid2Data.Columns.Add("seq2", typeof(String));
            grid2Data.Columns.Add("seq", typeof(String));
            grid2Data.Columns.Add("roll", typeof(String));
            grid2Data.Columns.Add("dyelot", typeof(String));
            grid2Data.Columns.Add("qty", typeof(decimal));
            grid2Data.Columns.Add("foc", typeof(decimal));
            grid2Data.Columns.Add("shipqty", typeof(decimal));
            grid2Data.Columns.Add("actualqty", typeof(decimal));
            grid2Data.Columns.Add("stockqty", typeof(decimal));
            grid2Data.Columns.Add("weight", typeof(decimal));
            grid2Data.Columns.Add("actualWeight", typeof(decimal));
            grid2Data.Columns.Add("pounit", typeof(String));
            grid2Data.Columns.Add("stockunit", typeof(String));
            grid2Data.Columns.Add("stocktype", typeof(String));
            grid2Data.Columns.Add("mdivisionid", typeof(String));
            grid2Data.Columns.Add("id", typeof(String));
            grid2Data.Columns.Add("location", typeof(String));
            grid2Data.Columns.Add("ErrMsg", typeof(String));

            listControlBindingSource2.DataSource = grid2Data;
            grid2.DataSource = listControlBindingSource2;

            grid2.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid2)
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13))
                .Text("seq1", header: "Seq1", width: Widths.AnsiChars(4))
                .Text("seq2", header: "Seq2", width: Widths.AnsiChars(3))
                .Text("roll", header: "Carton#", width: Widths.AnsiChars(8))
                .Text("dyelot", header: "Lot No", width: Widths.AnsiChars(4))
                .Text("pounit", header: "Po Unit", width: Widths.AnsiChars(4))
                .Text("stockunit", header: "Stock Unit", width: Widths.AnsiChars(4))
                .Numeric("Qty", header: "Qty")
                .Numeric("foc", header: "F.O.C")
                .Numeric("weight", header: "WeiKg")
                .Numeric("actualWeight", header: "NetKg")
                .Text("location", header: "Location", width: Widths.AnsiChars(8))
                .Text("ErrMsg", header: "Error Message", width: Widths.AnsiChars(100));

            for (int i = 0; i < grid2.ColumnCount; i++)
            {
                grid2.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        //Add Excel
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel files (*.xlsx)|*.xlsx";
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
        private void button2_Click(object sender, EventArgs e)
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
            grid2.SuspendLayout();
            #endregion

            /* 檢查1. Grid中的檔案是否存在，不存在時顯示於status欄位 
                 --   2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位
                 --   3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。當檢查都沒問題時，就將資料寫入第2個Grid*/
            #region
            
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (!MyUtility.Check.Empty(dr["Filename"]))
                {
                    if (!System.IO.File.Exists(MyUtility.Convert.GetString(dr["FullFileName"])))
                    {
                        dr["Status"] = string.Format("Excel file not found < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                    }
                    else
                    {
                        Microsoft.Office.Interop.Excel.Application excel;
                        try
                        {
                            excel = new Microsoft.Office.Interop.Excel.Application();
                        }
                        catch (Exception ex)
                        {
                            dr["Status"] = string.Format("Not able to open excel file < {0} >.", MyUtility.Convert.GetString(dr["Filename"]));
                            continue;
                        }

                        excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                        excel.Visible = false;
                        Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                        //檢查Excel格式
                        Microsoft.Office.Interop.Excel.Range range = worksheet.Range[String.Format("A{0}:L{0}", 1)];
                        object[,] objCellArray = range.Value;
                        string wkno = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                        string sp = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                        string seq1 = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                        string seq2 = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                        string ctnno = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                        string lotno = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");
                        string unit = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 7], "C");
                        string qty = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "C");
                        string foc = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "C");
                        string net = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "C");
                        string weight = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 11], "C");
                        string location = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 12], "C");


                        if (wkno.ToUpper() != "WK#" ||
                            sp.ToUpper() != "SP#" ||
                            seq1.ToUpper() != "SEQ1" ||
                            seq2.ToUpper() != "SEQ2" ||
                            ctnno.ToUpper() != "C/NO" ||
                            lotno.ToUpper() != "LOT NO." ||
                            (qty.ToUpper() != "QTY" && qty.ToUpper() != "Q'TY") ||
                            foc.ToUpper() != "F.O.C" ||
                            weight.ToUpper() != "WEIKG" ||
                            net.ToUpper() != "NETKG" ||
                            location.ToUpper() != "LOCATION"
                            )
                        {
                            #region 將不存在欄位顯示於status
                            StringBuilder columnName = new StringBuilder();
                            if (wkno.ToUpper() != "WK#") { columnName.Append("< WK# >, "); }
                            if (sp.ToUpper() != "SP#") { columnName.Append("< SP# >, "); }
                            if (seq1.ToUpper() != "SEQ1") { columnName.Append("< SEQ1 >, "); }
                            if (seq2.ToUpper() != "SEQ2") { columnName.Append("< SEQ2 >, "); }
                            if (ctnno.ToUpper() != "C/NO") { columnName.Append("< C/NO >, "); }
                            if (lotno.ToUpper() != "LOT NO.") { columnName.Append("< LOT NO. >, "); }
                            if (qty.ToUpper() != "QTY" && qty.ToUpper() != "Q'TY") { columnName.Append("< Qty >, "); }
                            if (foc.ToUpper() != "F.O.C") { columnName.Append("< F.O.C >, "); }
                            if (net.ToUpper() != "NETKG") { columnName.Append("< NetKg >, "); }
                            if (weight.ToUpper() != "WEIKG") { columnName.Append("< WeiKg >, "); }
                            if (location.ToUpper() != "LOCATION") { columnName.Append("< Location >, "); }

                            dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                            #endregion
                        }
                        else
                        {
                            int intRowsCount = worksheet.UsedRange.Rows.Count;
                            int intColumnsCount = worksheet.UsedRange.Columns.Count;
                            int intRowsStart = 2;
                            int intRowsRead = intRowsStart - 1;

                            while (intRowsRead < intRowsCount)
                            {
                                intRowsRead++;
                                range = worksheet.Range[String.Format("A{0}:L{0}", intRowsRead)];
                                objCellArray = range.Value;

                                DataRow newRow = grid2Data.NewRow();
                                newRow["MDivisionid"] = Env.User.Keyword;
                                newRow["wkno"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                                newRow["poid"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                                newRow["seq1"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                                newRow["seq2"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                                newRow["seq"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C").ToString().PadRight(3) + MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C").ToString();
                                newRow["roll"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                                newRow["dyelot"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");
                                newRow["qty"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 8], "N");
                                newRow["foc"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 9], "N");
                                newRow["shipqty"] = decimal.Parse(newRow["qty"].ToString()) + decimal.Parse(newRow["foc"].ToString());
                                newRow["actualqty"] = decimal.Parse(newRow["qty"].ToString()) + decimal.Parse(newRow["foc"].ToString());
                                newRow["actualWeight"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "N");
                                newRow["Weight"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 11], "N");
                                newRow["location"] = (objCellArray[1, 12] == null) ? "" : MyUtility.Excel.GetExcelCellValue(objCellArray[1, 12], "C");

                                if (!MyUtility.Check.Empty(newRow["wkno"]) && newRow["wkno"].ToString() != master["exportid"].ToString())
                                {
                                    dr["Status"] = string.Format("WK# is not match {0}", master["exportid"]);
                                    grid2Data.Clear();
                                    return;
                                }

                                if (MyUtility.Check.Empty(newRow["poid"]) || MyUtility.Check.Empty(newRow["seq1"]) || MyUtility.Check.Empty(newRow["seq2"]))
                                {
                                    continue;
                                }

                                if (newRow["seq1"].ToString().Substring(1, 1) == "7")
                                {
                                    continue;
                                    //MyUtility.Msg.WarningBox(string.Format("Can't not import 7X item (SP#:{0}-Seq1:{1}-Seq2:{2})!!", newRow["poid"], newRow["seq1"], newRow["seq2"]));
                                    //return;
                                }

                                DataRow dr2;
                                string sql = string.Format(@"
select fabrictype,POUnit,StockUnit,isnull(vu.Rate,0)*{3} as stockqty 
,(select o.Category from Orders o where o.id= pd.id) as category
from dbo.PO_Supp_Detail pd 
left join dbo.View_Unitrate vu on vu.FROM_U = pd.POUnit and vu.TO_U = pd.StockUnit
where id='{0}' and seq1 ='{1}' and seq2 = '{2}'", newRow["poid"], newRow["seq1"], newRow["seq2"], newRow["shipqty"]);

                                if (MyUtility.Check.Seek(sql, out dr2))
                                {
                                    // 非主料清空roll & dyelot
                                    if ("F" != dr2["fabrictype"].ToString())
                                    {
                                        newRow["roll"] = "";
                                        newRow["dyelot"] = "";
                                    }
                                    // po unit 空白不匯入
                                    if (MyUtility.Check.Empty(dr2["pounit"]))
                                    {
                                        MyUtility.Msg.WarningBox(string.Format("PO Unit of SP#:{0}-Seq1:{1}-Seq2:{2} is empty!!", newRow["poid"], newRow["seq1"], newRow["seq2"]));
                                        excel.Workbooks.Close();
                                        excel.Quit();
                                        return;
                                    }
                                    // stock unit空白不匯入
                                    if (MyUtility.Check.Empty(dr2["stockunit"]))
                                    {
                                        MyUtility.Msg.WarningBox(string.Format("Stock Unit of SP#:{0}-Seq1:{1}-Seq2:{2} is empty!!", newRow["poid"], newRow["seq1"], newRow["seq2"]));
                                        excel.Workbooks.Close();
                                        excel.Quit();
                                        return;
                                    }
                                    newRow["Pounit"] = dr2["pounit"].ToString();
                                    newRow["StockUnit"] = dr2["StockUnit"].ToString();
                                    newRow["stockqty"] = decimal.Parse(dr2["stockqty"].ToString());

                                    // 決定倉別
                                    if (dr2["category"].ToString() == "M")
                                    {
                                        newRow["stocktype"] = "B";
                                    }
                                    else
                                    {
                                        newRow["stocktype"] = "I";
                                    }

                                    //檢查location是否正確
                                    if (!MyUtility.Check.Empty(newRow["location"]))
                                    {
                                        string[] strA = Regex.Split(newRow["location"].ToString(), ",");
                                        foreach (string i in strA.Distinct())
                                        {
                                            if (!MyUtility.Check.Seek(string.Format(@"select * from dbo.mtllocation where stocktype='{0}' and id='{1}'", newRow["stocktype"], i)))
                                            {
                                                MyUtility.Msg.WarningBox(string.Format("Location ({3}) of SP#:{0}-Seq1:{1}-Seq2:{2} in stock ({4}) is not found!!"
                                                    , newRow["poid"], newRow["seq1"], newRow["seq2"], i, newRow["stocktype"]));
                                                excel.Workbooks.Close();
                                                excel.Quit();
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MyUtility.Msg.WarningBox(string.Format("SP#:{0}-Seq1:{1}-Seq2:{2} is not found!!", newRow["poid"], newRow["seq1"], newRow["seq2"]));
                                    return;
                                }
                                grid2Data.Rows.Add(newRow);
                            }
                            dr["Status"] = "Check & Import Completed.";
                        }

                        excel.Workbooks.Close();
                        excel.Quit();
                        excel = null;
                    }
                }
            }
            #endregion

            grid2.ResumeLayout();
        }

        //Write in
        private void button4_Click(object sender, EventArgs e)
        {
            DataTable tmpPacking = (DataTable)listControlBindingSource2.DataSource;
            try
            {
                var q = from p in tmpPacking.AsEnumerable()
                        group p by new
                        {
                            poid = p.Field<string>("poid"),
                            seq1 = p.Field<string>("seq1"),
                            seq2 = p.Field<string>("seq2"),
                            Roll = p.Field<string>("Roll")
                        } into m
                        where m.Count() > 1 //只顯示超過一次以上的
                        select new
                        {
                            poid = m.First().Field<string>("poid"),
                            seq1 = m.First().Field<string>("seq1"),
                            seq2 = m.First().Field<string>("seq2"),
                            Roll = m.First().Field<string>("Roll"),
                            count = m.Count()
                        };
                if (q.ToList().Count > 0)
                {
                    string warning = "";

                    foreach (var dr in q)
                    {
                        warning += string.Format("{0}-{1}-{2}-{3}" + Environment.NewLine, dr.poid, dr.seq1, dr.seq2, dr.Roll);
                    }
                    MyUtility.Msg.WarningBox(warning, "Roll# are duplicated!!");
                    return;
                }

                //刪除表身重新匯入
                foreach (DataRow del in detailData.Rows)
                {
                    del.Delete();
                }
                foreach (DataRow dr2 in tmpPacking.Rows)
                {
                    dr2["id"] = master["id"];
                    dr2.AcceptChanges();
                    dr2.SetAdded();
                    detailData.ImportRow(dr2);
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
    }
}
