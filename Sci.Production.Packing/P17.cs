using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.IO;
using System.Linq;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P17
    /// </summary>
    public partial class P17 : Sci.Win.Tems.QueryForm
    {
        private DataTable grid2Data = new DataTable();

        /// <summary>
        /// P17
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.comboBrand.SelectedIndex = 0;
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region excelgrid
            DataTable excelFile = new DataTable();
            excelFile.Columns.Add("Filename", typeof(string));
            excelFile.Columns.Add("Status", typeof(string));
            excelFile.Columns.Add("FullFileName", typeof(string));

            this.listControlBindingSource1.DataSource = excelFile;
            this.gridAttachFile.DataSource = this.listControlBindingSource1;
            this.gridAttachFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridAttachFile)
            .Text("Filename", header: "File Name", width: Widths.AnsiChars(20))
            .Text("Status", header: "Status", width: Widths.AnsiChars(100))
            ;
            #endregion

            #region Grid2結構
            this.grid2Data.Columns.Add("selected", typeof(bool));
            this.grid2Data.Columns.Add("CustPoNo", typeof(string));
            this.grid2Data.Columns.Add("Brand", typeof(string));
            this.grid2Data.Columns.Add("Styleid", typeof(string));
            this.grid2Data.Columns.Add("StyleName", typeof(string));
            this.grid2Data.Columns.Add("Article", typeof(string));
            this.grid2Data.Columns.Add("Size", typeof(string));
            this.grid2Data.Columns.Add("BarCode", typeof(string));
            this.grid2Data.Columns.Add("PackID", typeof(string));
            this.grid2Data.Columns.Add("CTN", typeof(string));
            this.grid2Data.Columns.Add("CustCTN", typeof(string));
            this.grid2Data.Columns.Add("Status", typeof(string));

            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("Brand", header: "Brand", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("StyleName", header: "Style", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Article", header: "Colorway", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Size", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("BarCode", header: "BarCode", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("PackID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CTN", header: "CTN#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CustCTN", header: "Cust CTN#", width: Widths.AnsiChars(15))
            .Text("Status", header: "Status", width: Widths.AnsiChars(20), iseditingreadonly: true)
            ;
            #endregion
            #region 關閉排序功能
            for (int i = 0; i < this.gridDetail.ColumnCount; i++)
            {
                this.gridDetail.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
            this.gridDetail.Columns["CustCTN"].DefaultCellStyle.BackColor = Color.Pink;
        }

        // Add Excel
        private void BtnAddExcel_Click(object sender, EventArgs e)
        {
            if (this.comboBrand.Text.Empty())
            {
                MyUtility.Msg.WarningBox("Please select Brand first");
                return;
            }

            if (this.comboBrand.Text.EqualString("N.FACE"))
            {
                this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            }
            else
            {
                this.openFileDialog1.Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt";
            }

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DataRow dr = ((DataTable)this.listControlBindingSource1.DataSource).NewRow();
                dr["Filename"] = this.openFileDialog1.SafeFileName;
                dr["Status"] = string.Empty;
                dr["FullFileName"] = this.openFileDialog1.FileName;
                ((DataTable)this.listControlBindingSource1.DataSource).Rows.Add(dr);
                this.listControlBindingSource1.MoveLast();
            }

            this.gridAttachFile.AutoResizeColumns();
        }

        // Remove Excel
        private void BtnRemoveExcel_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.Position != -1)
            {
                this.listControlBindingSource1.RemoveCurrent();
            }
        }

        // Check & Import
        private void BtnCheckImport_Click(object sender, EventArgs e)
        {
            #region 判斷第一個Grid是否有資料
            if (this.listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No excel data!!");
                return;
            }
            #endregion

            // 清空Grid2資料
            if (this.grid2Data != null)
            {
                this.grid2Data.Clear();
            }

            #region 檢查1. Grid中的檔案是否存在，不存在時顯示於status欄位；2. Grid中的檔案都可以正常開啟，無法開啟時顯示於status欄位；3.檢查開啟的excel檔存在必要的欄位，將不存在欄位顯示於status。當檢查都沒問題時，就將資料寫入第2個Grid
            DataTable notdist = this.grid2Data.Copy();
            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (!MyUtility.Check.Empty(dr["Filename"]))
                {
                    if (!System.IO.File.Exists(MyUtility.Convert.GetString(dr["FullFileName"])))
                    {
                        dr["Status"] = "can not find file!!";
                    }
                    else
                    {
                        if (this.comboBrand.Text.EqualString("N.FACE"))
                        {
                            using (StreamReader reader = new StreamReader(MyUtility.Convert.GetString(dr["FullFileName"]), System.Text.Encoding.UTF8))
                            {
                                string line;
                                try
                                {
                                    string forsizesplit = string.Empty;
                                    while ((line = reader.ReadLine()) != null)
                                    {
                                        DataRow newRow = notdist.NewRow();
                                        newRow["selected"] = 1;
                                        string custPoNo = line.Substring(520, 15).TrimEnd();
                                        if (custPoNo.Length <= 10)
                                        {
                                            custPoNo = custPoNo.Substring(0, 7);
                                        }
                                        else
                                        {
                                            custPoNo = custPoNo.Substring(0, 10);
                                        }

                                        newRow["CustPoNo"] = custPoNo;
                                        newRow["Brand"] = this.comboBrand.Text;
                                        newRow["styleid"] = line.Substring(550, 8).TrimEnd();
                                        newRow["stylename"] = newRow["styleid"] + "-" + MyUtility.GetValue.Lookup($"select stylename from style where id = '{newRow["styleid"]}'");
                                        newRow["Article"] = line.Substring(558, 10).TrimEnd();
                                        newRow["CustCTN"] = line.Substring(726, 20).TrimEnd();
                                        string size = line.Substring(595, 15).TrimEnd().TrimStart('0');
                                        IList<string> sizeSplit = size.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (sizeSplit.Count > 1)
                                        {
                                            forsizesplit = sizeSplit[1].Trim();
                                        }

                                        string size2 = sizeSplit[0].TrimEnd().TrimStart('0');
                                        if (!forsizesplit.Empty() && size2.Contains(forsizesplit))
                                        {
                                            size2 = size2.Replace(forsizesplit, string.Empty);
                                        }

                                        newRow["Size"] = size2;
                                        string xxxBarCode = line.Substring(632, 40).Trim();
                                        IList<string> sl = xxxBarCode.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                                        newRow["BarCode"] = sl[sl.Count - 1].Substring(sl[sl.Count - 1].Length - 12, 12);
                                        notdist.Rows.Add(newRow);
                                    }
                                }
                                catch (Exception)
                                {
                                    dr["Status"] = "Error Import File";
                                }
                            }
                        }
                        else if (this.comboBrand.Text.EqualString("DOME"))
                        {
                            #region DOME
                            Microsoft.Office.Interop.Excel.Application excel;
                            try
                            {
                                excel = new Microsoft.Office.Interop.Excel.Application();
                            }
                            catch (Exception)
                            {
                                dr["Status"] = "can not find file!!";
                                continue;
                            }

                            excel.DisplayAlerts = false;
                            excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                            excel.Visible = false;
                            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                            // 檢查Excel格式
                            Microsoft.Office.Interop.Excel.Range range = worksheet.Range[string.Format("A{0}:AE{0}", 1)];
                            object[,] objCellArray = range.Value;
                            string sp = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "C");
                            string styleid = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 23], "C");
                            string article = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 24], "C");
                            string size = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 25], "C");
                            string barCode = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 28], "C");

                            if (!sp.ToUpper().EqualString("PO") ||
                                !styleid.ToUpper().EqualString("Detail-Style Id") ||
                                !article.ToUpper().EqualString("Detail-Color") ||
                                !size.ToUpper().EqualString("Detail-Size") ||
                                !barCode.ToUpper().EqualString("Detail-Alternate Item Id"))
                            {
                                #region 將不存在欄位顯示於status
                                StringBuilder columnName = new StringBuilder();
                                if (sp.ToUpper() != "PO")
                                {
                                    columnName.Append("< PO >, ");
                                }

                                if (styleid.ToUpper() != "Detail-Style Id")
                                {
                                    columnName.Append("< Detail-Style Id >, ");
                                }

                                if (article.ToUpper() != "Detail-Color")
                                {
                                    columnName.Append("< Detail-Color >, ");
                                }

                                if (size.ToUpper() != "Detail-Size")
                                {
                                    columnName.Append("< Detail-Size >, ");
                                }

                                if (barCode.ToUpper() != "Detail-Alternate Item Id")
                                {
                                    columnName.Append("< Detail-Alternate Item Id >, ");
                                }

                                dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                                #endregion
                            }
                            else
                            {
                                int intRowsCount = worksheet.UsedRange.Rows.Count;
                                int intRowsRead = 1;
                                string custPoNo = string.Empty;

                                while (intRowsRead < intRowsCount)
                                {
                                    intRowsRead++;

                                    range = worksheet.Range[string.Format("A{0}:AE{0}", intRowsRead)];
                                    objCellArray = range.Value;

                                    DataRow newRow = notdist.NewRow();
                                    if (intRowsRead == 2)
                                    {
                                        custPoNo = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 10], "C"));
                                    }

                                    newRow["selected"] = 1;
                                    newRow["CustPoNo"] = custPoNo;
                                    newRow["Brand"] = this.comboBrand.Text;
                                    newRow["styleid"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 23], "C");
                                    newRow["stylename"] = newRow["styleid"] + "-" + MyUtility.GetValue.Lookup($"select stylename from style where id = '{newRow["styleid"]}'");
                                    newRow["CustCTN"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");

                                    // article抓取 - - 中間的值
                                    // **eg01:MTR2315 - FUG / BON - MD 請捉取 FUG/ BON
                                    // **eg02:XXX - 5A6S - 4D5 - XXX 請捉取 A6S-4D5
                                    newRow["Article"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 26], "C").ToString().Split('-')[1];
                                    string size2 = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 25], "C"));
                                    if (size2.Contains("("))
                                    {
                                        size2 = size2.Substring(0, size2.IndexOf("("));
                                    }

                                    newRow["Size"] = size2.Replace("/", string.Empty);
                                    newRow["BarCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 28], "C");

                                    notdist.Rows.Add(newRow);
                                }

                                dr["Status"] = "Check Completed.";
                            }

                            excel.Workbooks.Close();
                            excel.Quit();
                            excel = null;
                            #endregion
                        }
                        else
                        {
                            #region Other
                            Microsoft.Office.Interop.Excel.Application excel;
                            try
                            {
                                excel = new Microsoft.Office.Interop.Excel.Application();
                            }
                            catch (Exception)
                            {
                                dr["Status"] = "can not find file!!";
                                continue;
                            }

                            excel.DisplayAlerts = false;
                            excel.Workbooks.Open(MyUtility.Convert.GetString(dr["FullFileName"]));
                            excel.Visible = false;
                            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

                            // 檢查Excel格式
                            Microsoft.Office.Interop.Excel.Range range = worksheet.Range[string.Format("A{0}:AE{0}", 1)];
                            object[,] objCellArray = range.Value;
                            string sp = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                            string brandid = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                            string styleid = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                            string article = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                            string size = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C");
                            string barCode = (string)MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");

                            if (!sp.ToUpper().EqualString("PO#") ||
                                !brandid.ToUpper().EqualString("Brand") ||
                                !styleid.ToUpper().EqualString("Style") ||
                                !article.ToUpper().EqualString("Colorway") ||
                                !size.ToUpper().EqualString("Size") ||
                                !barCode.ToUpper().EqualString("Barcode"))
                            {
                                #region 將不存在欄位顯示於status
                                StringBuilder columnName = new StringBuilder();
                                if (sp.ToUpper() != "PO")
                                {
                                    columnName.Append("< PO >, ");
                                }

                                if (brandid.ToUpper() != "Brand")
                                {
                                    columnName.Append("< Brand >, ");
                                }

                                if (styleid.ToUpper() != "Style")
                                {
                                    columnName.Append("< Style >, ");
                                }

                                if (article.ToUpper() != "Colorway")
                                {
                                    columnName.Append("< Colorway >, ");
                                }

                                if (size.ToUpper() != "Size")
                                {
                                    columnName.Append("< Size >, ");
                                }

                                if (barCode.ToUpper() != "Barcode")
                                {
                                    columnName.Append("< Barcode >, ");
                                }

                                dr["Status"] = columnName.ToString().Substring(0, columnName.ToString().Length - 2) + "column not found in the excel.";
                                #endregion
                            }
                            else
                            {
                                int intRowsCount = worksheet.UsedRange.Rows.Count;
                                int intRowsRead = 1;
                                string custPoNo = string.Empty;

                                while (intRowsRead < intRowsCount)
                                {
                                    intRowsRead++;

                                    range = worksheet.Range[string.Format("A{0}:F{0}", intRowsRead)];
                                    objCellArray = range.Value;

                                    DataRow newRow = notdist.NewRow();
                                    if (intRowsRead == 2)
                                    {
                                        custPoNo = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C"));
                                    }

                                    newRow["selected"] = 1;
                                    newRow["CustPoNo"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 1], "C");
                                    newRow["Brand"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 2], "C");
                                    newRow["styleid"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 3], "C");
                                    newRow["stylename"] = newRow["styleid"] + "-" + MyUtility.GetValue.Lookup($"select stylename from style where id = '{newRow["styleid"]}'");
                                    newRow["Article"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 4], "C");
                                    string size2 = MyUtility.Convert.GetString(MyUtility.Excel.GetExcelCellValue(objCellArray[1, 5], "C"));
                                    if (size2.Contains("("))
                                    {
                                        size2 = size2.Substring(0, size2.IndexOf("("));
                                    }

                                    newRow["Size"] = size2;
                                    newRow["BarCode"] = MyUtility.Excel.GetExcelCellValue(objCellArray[1, 6], "C");

                                    notdist.Rows.Add(newRow);
                                }

                                dr["Status"] = "Check Completed.";
                            }

                            excel.Workbooks.Close();
                            excel.Quit();
                            excel = null;
                            #endregion
                        }
                    }
                }
            }

            // 取得Pack ID,CTN#
            if (notdist.Rows.Count > 0)
            {
                string sql_cmd = $@"
;with keyTable as (
select distinct CustPoNo,Brand,StyleID ,StyleName,Article  ,Barcode, Size
from #tmp
),
PackingList_Detailtmp as
(
select t.CustPoNo,t.Brand,t.Styleid,t.StyleName,t.Article,t.Barcode,t.Size,[PackID] = PL.ID,[CTN] = PL.CTNStartNo,
[mapSeq] = ROW_NUMBER() OVER (PARTITION BY t.CustPoNo,t.Brand,t.Styleid,t.Article,t.Size ORDER BY PL.ID, PL.seq),pl.Seq
from keyTable as t
left join ORDERS O  WITH (NOLOCK) ON  O.custpono= t.CustPoNo and O.StyleID= t.Styleid
left join PackingList_Detail PL  WITH (NOLOCK) on    PL.Article= t.Article and PL.SizeCode=t.Size and O.ID = PL.OrderID
left join PackingList P WITH (NOLOCK) ON P.BrandID= t.Brand and p.id = pl.id
),
excelData as
(
select *,[mapSeq] = ROW_NUMBER() OVER (PARTITION BY CustPoNo,Brand,Styleid,Article,Size ORDER BY CustCTN)
from   #tmp
)
select  [selected] = isnull(ed.selected,1),pd.CustPoNo,pd.Brand,pd.Styleid,pd.StyleName,pd.Article,pd.Barcode,pd.Size,pd.PackID,pd.CTN,[CustCTN]= isnull(ed.CustCTN,''),
        [Status] = iif(pd.PackID is null, 'Cust CTN can not mapping!! Please check <Size>,<Colorway>' ,isnull(ed.Status,'')),
        pd.Seq
from PackingList_Detailtmp pd
left join excelData ed on pd.CustPoNo =  ed.CustPoNo and pd.Brand = ed.Brand and pd.Styleid = ed.Styleid and 
pd.Article = ed.Article and pd.Size =ed.Size and pd.mapSeq = ed.mapSeq
order by pd.PackID,pd.Seq
";
                DualResult result = MyUtility.Tool.ProcessWithDatatable(notdist, string.Empty, sql_cmd, out notdist);

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion
            this.grid2Data = notdist.DefaultView.ToTable(false, new string[] { "selected", "CustPoNo", "Brand", "Styleid", "StyleName", "Article", "Size", "BarCode", "PackID", "CTN", "CustCTN", "Status" });
            this.listControlBindingSource2.DataSource = this.grid2Data;
            this.gridAttachFile.AutoResizeColumns();
            this.gridDetail.AutoResizeColumns();
        }

        private void ComboBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            // gridAttachFile
            if (((DataTable)this.listControlBindingSource1.DataSource) != null)
            {
                ((DataTable)this.listControlBindingSource1.DataSource).Clear();
            }

            // 清空Grid2資料
            if (this.grid2Data != null)
            {
                this.grid2Data.Clear();
            }
        }

        private void BtnWriteIn_Click_1(object sender, EventArgs e)
        {
            #region 判斷第一個Grid是否有資料
            if (this.listControlBindingSource1.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No excel data!!");
                return;
            }
            #endregion

            // 清空 status
            foreach (DataRow item in this.grid2Data.Rows)
            {
                item["status"] = string.Empty;
            }

            DualResult result;
            DataRow[] selectrows = this.grid2Data.Select("selected = 1");
            foreach (DataRow item in selectrows)
            {
                if (MyUtility.Check.Empty(item["PackID"]) || MyUtility.Check.Empty(item["CTN"]) || MyUtility.Check.Empty(item["CustCTN"]))
                {
                    item["status"] = "Cust CTN can not mapping!!";
                    continue;
                }

                string upd_cmd = $@"update PackingList_Detail set CustCTN = '{item["CustCTN"]}' where id = '{item["PackID"]}' and CTNStartNo = '{item["CTN"]}' ";
                result = DBProxy.Current.Execute(null, upd_cmd);
                if (!result)
                {
                    item["status"] = result.Messages;
                }
            }

            foreach (var item in selectrows.Select(s => new
            {
                CustPoNo = s["CustPoNo"].ToString(),
                Brand = s["Brand"].ToString(),
                StyleID = s["StyleID"].ToString(),
                Article = s["Article"].ToString(),
                Size = s["Size"].ToString(),
                Barcode = s["Barcode"].ToString(),
            }).Distinct())
            {
                string checkorderexists = $@"
select 1
from orders o with(nolock)
inner join Order_Article oa with(nolock) on oa.id = o.id
inner join order_sizecode os with(nolock) on os.id = o.poid
where o.CustPoNo = '{item.CustPoNo}' and o.BrandID = '{item.Brand}' and o.StyleID = '{item.StyleID}' 
and oa.article = '{item.Article}' and os.SizeCode = '{item.Size}'";

                if (!MyUtility.Check.Seek(checkorderexists))
                {
                    this.Updategrid2DataStatus(item.Brand, item.CustPoNo, item.StyleID, item.Article, item.Size, "The system cannot find this data!!");
                    continue;
                }

                string checkCustBarCodeExists = $@"
select Barcode
from CustBarCode with(nolock)
where CustPoNo = '{item.CustPoNo}' and BrandID = '{item.Brand}' and StyleID = '{item.StyleID}' 
and article = '{item.Article}' and SizeCode = '{item.Size}'";

                DataRow custBarCodeDr;
                if (MyUtility.Check.Seek(checkCustBarCodeExists, out custBarCodeDr))
                {
                    string updateCustBarCode = $@"
update CustBarCode set
    barcode='{item.Barcode}',
    EditName='{Sci.Env.User.UserID}',
    EditDate= getdate()
where CustPoNo = '{item.CustPoNo}' and BrandID = '{item.Brand}' and StyleID = '{item.StyleID}' 
and article = '{item.Article}' and SizeCode = '{item.Size}'
";
                    result = DBProxy.Current.Execute(null, updateCustBarCode);
                    this.Updategrid2DataStatus(item.Brand, item.CustPoNo, item.StyleID, item.Article, item.Size, $"This data already exists ,update barcode {custBarCodeDr["barcode"]} to {item.Barcode}");
                }
                else
                {
                    string insertCustBarCode = $@"
insert CustBarCode(BrandID,CustPONo,StyleID,Article,SizeCode,BarCode,EditName,EditDate)
values('{item.Brand}','{item.CustPoNo}','{item.StyleID}','{item.Article}','{item.Size}','{item.Barcode}','{Sci.Env.User.UserID}',getdate())
";
                    result = DBProxy.Current.Execute(null, insertCustBarCode);
                    this.Updategrid2DataStatus(item.Brand, item.CustPoNo, item.StyleID, item.Article, item.Size, $"Created success!!");
                }

                string checkPackingList_DetailExists = $@"
select * 
from PackingList_Detail pd with(nolock)
inner join Orders o with(nolock) on o.id = pd.orderid
inner join PackingList p with(nolock) on p.id = pd.id and p.type in ('B','L')
left join Pullout with(nolock) on Pullout.id = p.PulloutID
where  p.BrandID = '{item.Brand}' and o.CustPoNo ='{item.CustPoNo}' and o.StyleID = '{item.StyleID}' and pd.Article = '{item.Article}' and pd.SizeCode = '{item.Size}'
and (Pullout.Status = 'New' or Pullout.Status is null)
";
                DataRow packingList_Detail;
                if (MyUtility.Check.Seek(checkPackingList_DetailExists, out packingList_Detail))
                {
                    string updatePackingList_Detai = $@"
update pd set
    pd.BarCode ='{item.Barcode}'
from PackingList_Detail pd with(nolock)
inner join Orders o with(nolock) on o.id = pd.orderid
inner join PackingList p with(nolock) on p.id = pd.id and p.type in ('B','L')
left join Pullout with(nolock) on Pullout.id = p.PulloutID
where  p.BrandID = '{item.Brand}' and o.CustPoNo ='{item.CustPoNo}' and o.StyleID = '{item.StyleID}' and pd.Article = '{item.Article}' and pd.SizeCode = '{item.Size}'
and (Pullout.Status = 'New' or Pullout.Status is null)
";
                    result = DBProxy.Current.Execute(null, updatePackingList_Detai);
                    this.Updategrid2DataStatus(item.Brand, item.CustPoNo, item.StyleID, item.Article, item.Size, " Barcode has been updated to packingList");
                }
            }

            this.gridDetail.AutoResizeColumns();
            MyUtility.Msg.InfoBox("Created success, Refer to the Status field description");
        }

        private void Updategrid2DataStatus(string brand, string custPoNo, string styleID, string article, string size, string status)
        {
            foreach (DataRow item in this.grid2Data.Select($@"selected = 1 and Brand = '{brand}' and CustPoNo = '{custPoNo}' and StyleID = '{styleID}' and Article = '{article}' and Size = '{size}'"))
            {
                if (MyUtility.Check.Empty(item["status"]))
                {
                    item["status"] = status;
                }
                else
                {
                    item["status"] = item["status"] + " " + status;
                }
            }
        }

        private void Btnclose_click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btndowload_click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Packing_P17Template.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }
    }
}
