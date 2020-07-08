using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data.SqlClient;

namespace Sci.Production.Packing
{
    public partial class P24 : Win.Tems.Input6
    {
        private string destination_path; // 放的路徑

        public P24(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select ShippingMarkPath from System WITH (NOLOCK) ", null);
            this.gridicon.Visible = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["Ukey"]);
            this.DetailSelectCommand = $@"
select distinct
	pd.OrderID,
	o.CustPONO,
	pd.CTNStartNo,
	pd.CustCTN,
	pd.SCICtnNo,
	pd.Article,
	pd.Color,
	pd.SizeCode,
	sd.FileName,
    FileNameOri=sd.FileName,
    local_file_type='',
    FileSourcePath='',
    FileAction='',
    sd.ShippingMarkPicUkey,
    pd.RefNo,
    [ShippingMark]=IIF(sd.Image IS NOT NULL ,1 ,0 )
from ShippingMarkPic_Detail sd with(nolock)
inner join ShippingMarkPic s with(nolock) on sd.ShippingMarkPicUkey = s.Ukey
inner join PackingList_Detail pd with(nolock) on pd.ID = s.PackingListID and pd.SCICtnNo = sd.SCICtnNo
inner join orders o with(nolock) on o.id = pd.OrderID
where sd.ShippingMarkPicUkey = '{masterID}'
order by pd.SCICtnNo
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings itemSelect = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings fromLeft = new DataGridViewGeneratorNumericColumnSettings();

            this.detailgrid.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("OrderID", header: "SP No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CustPONO", header: "P.O. No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("CustCTN", header: "Cust #", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SCICtnNo", header: "SCI Ctn No.", width: Widths.AnsiChars(16), iseditingreadonly: true)
            .Text("RefNo", header: "Ref No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Article", header: "ColorWay", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)

            // .Text("FileName", header: "Shipping Mark File Name", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .CheckBox("ShippingMark", header: "Shipping Mark", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            .Button("Upload", null, header: string.Empty, width: Widths.AnsiChars(5), onclick: this.BtnUpload)
            .Button("Delete", null, header: string.Empty, width: Widths.AnsiChars(5), onclick: this.BtnDelete)
            ;
            this.detailgrid.CellPainting += this.Detailgrid_CellPainting;
            #region 關閉排序功能
            for (int i = 0; i < this.detailgrid.ColumnCount; i++)
            {
                this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion
            return base.OnGridSetup();
        }

        private void Detailgrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 8)
            {
                return;
            }

            if (e.RowIndex > 0)
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }

            if (this.IsTheSameCellValue(e.RowIndex))
            {
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Bottom = this.detailgrid.AdvancedCellBorderStyle.Bottom;
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.ChangCell();
            this.SetComboSeqAndSide();
            this.mtbs.ResetBindings(false);
        }

        private void ChangCell()
        {
            foreach (DataGridViewRow dr in this.detailgrid.Rows)
            {
                if (this.IsTheSameCellValueBefore(dr.Index))
                {
                    // dr.Cells[8] = new DataGridViewTextBoxCell();
                    dr.Cells[9] = new DataGridViewTextBoxCell();
                    dr.Cells[10] = new DataGridViewTextBoxCell();
                    dr.Cells[11] = new DataGridViewTextBoxCell();

                    // dr.Cells[8].Style.ForeColor = Color.White;
                    dr.Cells[9].Style.ForeColor = Color.White;
                    dr.Cells[10].Style.ForeColor = Color.White;
                    dr.Cells[11].Style.ForeColor = Color.White;

                    // dr.Cells[8].Style.SelectionForeColor = Color.White;
                    dr.Cells[9].Style.SelectionForeColor = Color.White;
                    dr.Cells[10].Style.SelectionForeColor = Color.White;
                    dr.Cells[11].Style.SelectionForeColor = Color.White;

                    // dr.Cells[8].Style.SelectionBackColor = Color.White;
                    dr.Cells[9].Style.SelectionBackColor = Color.White;
                    dr.Cells[10].Style.SelectionBackColor = Color.White;
                    dr.Cells[11].Style.SelectionBackColor = Color.White;

                    // dr.Cells[8].ReadOnly = true;
                    // dr.Cells[8].ReadOnly = true;
                    dr.Cells[9].ReadOnly = true;
                    dr.Cells[10].ReadOnly = true;
                    dr.Cells[11].ReadOnly = true;
                }
            }
        }

        private bool IsTheSameCellValue(int row)
        {
            if (row == this.detailgrid.Rows.Count - 1)
            {
                return false;
            }

            DataGridViewCell cell1 = this.detailgrid["SCICtnNo", row];
            DataGridViewCell cell2 = this.detailgrid["SCICtnNo", row + 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private bool IsTheSameCellValueBefore(int row)
        {
            if (row > this.detailgrid.Rows.Count || row == 0)
            {
                return false;
            }

            DataGridViewCell cell1 = this.detailgrid["SCICtnNo", row - 1];
            DataGridViewCell cell2 = this.detailgrid["SCICtnNo", row];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private void BtnUpload(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.destination_path))
            {
                MyUtility.Msg.WarningBox("ShippingMarkPath not set!");
                return;
            }

            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "Image Files(*.BMP;)|*.BMP"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                string local_path_file = file.FileName;
                string sCICtnNo = MyUtility.Convert.GetString(this.CurrentDetailData["SCICtnNo"]);
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Select($"SCICtnNo='{sCICtnNo}'"))
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        dr["FileSourcePath"] = MyUtility.Convert.GetString(local_path_file);
                        dr["local_file_type"] = Path.GetExtension(local_path_file);
                        dr["FileName"] = MyUtility.Convert.GetString(local_path_file);
                        dr["FileAction"] = "Upload";
                    }
                }

                // 存檔時才要組合檔名SCICtnNo+Seq+Side
            }
        }

        private void BtnDelete(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string sCICtnNo = MyUtility.Convert.GetString(this.CurrentDetailData["SCICtnNo"]);
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Select($"SCICtnNo='{sCICtnNo}'"))
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["FileSourcePath"] = string.Empty;
                    dr["local_file_type"] = string.Empty;
                    dr["FileName"] = string.Empty;
                    dr["FileAction"] = "Delete";
                }
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Seq"] = 1;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtPackingListID.ReadOnly = true;
            this.comboSeq.ReadOnly = true;
            this.cmbSide.ReadOnly = true;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["PackingListID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Seq"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Side"]))
            {
                MyUtility.Msg.WarningBox("Packing No. , Seq , Side can not empty!");
                return false;
            }

            #region 檢查detail資料是否存在於ShippingMarkPicture的設定中
            var checkHasFileNameData = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["FileName"]));
            if (checkHasFileNameData.Any())
            {
                DataTable dtHasFileNameDetail = checkHasFileNameData.CopyToDataTable();
                DataTable dtCheckResult;

                // foreach (DataRow drr in dtHasFileNameDetail.Rows)
                // {
                string sqlCheckDetail = $@"
   /* select distinct o.BrandID,o.CustCDID,t.Refno
    from #tmp t
    left join orders o with (nolock) on t.OrderID = o.ID
    where not exists (select 1 from ShippingMarkPicture smp with (nolock) 
                                where   smp.BrandID = o.BrandID and
                                        smp.CustCD = o.CustCDID and
                                        smp.CTNRefno = t.Refno and
                                        smp.Side = '{this.CurrentMaintain["Side"]}' and
                                        smp.Seq = '{this.CurrentMaintain["Seq"]}')
*/
 SELECT DISTINCT o.BrandID, o.CustCDID ,pd.Refno
 FROM ShippingMarkPic s
 INNER JOIN PackingList_Detail pd ON pd.ID = s.PackingListID
 INNER JOIN Orders o ON o.ID = pd.OrderID
 WHERE s.Ukey=42
 AND NOT EXISTS(
	 SELECT *
	 FROM ShippingMarkPicture  smp
	 WHERE   smp.BrandID = o.BrandID 
	 AND smp.CTNRefno = pd.Refno 
     AND smp.Side = '{this.CurrentMaintain["Side"]}' 
     AND smp.Seq = '{this.CurrentMaintain["Seq"]}'
 )
    ";

                // DualResult result = MyUtility.Tool.ProcessWithDatatable(dtHasFileNameDetail, string.Empty, sqlCheckDetail, out dtCheckResult);
                DualResult result = DBProxy.Current.Select(null, sqlCheckDetail, out dtCheckResult);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                string errMsg = string.Empty;
                foreach (DataRow dr in dtCheckResult.Rows)
                {
                    errMsg += $"<Brand>:{dr["BrandID"]}  <CTNRefno>:{dr["Refno"]} {Environment.NewLine}";
                }

                if (errMsg.Length > 0)
                {
                    errMsg = "Please go to [Packing B03] to complete setting!" + Environment.NewLine + errMsg;
                    MyUtility.Msg.WarningBox(errMsg);
                    return false;
                }

                // }
            }
            #endregion

            return base.ClickSaveBefore();
        }

        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            #region 取表身必須欄位,第一筆(有值)
            var detailList = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().GroupBy(s => s["SCICtnNo"], (ctn, a) => new
            {
                SCICtnNo = MyUtility.Convert.GetString(ctn),
                ShippingMarkPicUkey = MyUtility.Convert.GetLong(a.First()["ShippingMarkPicUkey"]),
                FileName = MyUtility.Convert.GetString(a.First()["FileName"]),
                FileSourcePath = MyUtility.Convert.GetString(a.First()["FileSourcePath"]),
                local_file_type = MyUtility.Convert.GetString(a.First()["local_file_type"]),
                FileNameOri = MyUtility.Convert.GetString(a.First()["FileNameOri"]),
                FileAction = MyUtility.Convert.GetString(a.First()["FileAction"]),
            }).ToList();
            #endregion

            DataTable dt = this.ToDataTable(detailList);
            dt.TableName = "detaildt";
            foreach (DataRow dr in dt.Rows)
            {
                if (!MyUtility.Check.Empty(dr["FileSourcePath"]))
                {
                    dr["FileName"] = MyUtility.Convert.GetString(dr["SCICtnNo"]) + MyUtility.Convert.GetString(this.CurrentMaintain["Seq"]) + MyUtility.Convert.GetString(this.CurrentMaintain["Side"]) + MyUtility.Convert.GetString(dr["local_file_type"]);
                    dr.EndEdit();
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                if (MyUtility.Convert.GetString(dr["FileAction"]).ToLower().EqualString("delete") && !MyUtility.Check.Empty(dr["FileNameOri"]))
                {
                    string destination = string.Empty;
                    try
                    {
                        destination = Path.Combine(this.destination_path, MyUtility.Convert.GetString(dr["FileNameOri"]));
                        if (System.IO.File.Exists(destination))
                        {
                            System.IO.File.Delete(destination);
                        }
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + "\r\n" + destination + "\r\n" + exception.Message);
                    }
                }
                else if (MyUtility.Convert.GetString(dr["FileAction"]).ToLower().EqualString("upload"))
                {
                    string local_path_file = MyUtility.Convert.GetString(dr["FileSourcePath"]);
                    string destination_fileName = MyUtility.Convert.GetString(dr["FileName"]);
                    try
                    {
                        // 轉換成Byte存, 不用再存圖片到server上
                        string destination = Path.Combine(this.destination_path, destination_fileName);

                        byte[] data = null;

                        FileInfo fInfo = new FileInfo(local_path_file);

                        long length = fInfo.Length;

                        FileStream fStream = new FileStream(local_path_file, FileMode.Open, FileAccess.Read);

                        BinaryReader br = new BinaryReader(fStream);

                        data = br.ReadBytes((int)length);

                        string cmd = $@"----ShippingMarkPic_Detail存入圖片
UPDATE sd
SET sd.Image=@Image
FROM ShippingMarkPic s
INNER JOIN ShippingMarkPic_Detail sd ON s.Ukey=sd.ShippingMarkPicUkey
WHERE s.Ukey ={this.CurrentMaintain["Ukey"]}
AND s.Side=@Side
AND s.Seq=@Seq
AND sd.SCICtnNo=@SCICtnNo
                    ";
                        List<SqlParameter> para = new List<SqlParameter>();
                        para.Add(new SqlParameter("@Side", this.CurrentMaintain["Side"].ToString()));
                        para.Add(new SqlParameter("@Seq", this.CurrentMaintain["Seq"].ToString()));
                        para.Add(new SqlParameter("@SCICtnNo", dr["SCICtnNo"].ToString()));
                        para.Add(new SqlParameter("@Image", (object)data));

                        DualResult result = DBProxy.Current.Execute(null, cmd, para);

                        if (!result)
                        {
                            this.ShowErr(result);
                        }

                        // if (System.IO.File.Exists(local_path_file))
                        // {
                        //    if (System.IO.File.Exists(destination))
                        //    {
                        //        System.IO.File.Delete(destination);
                        //    }

                        // System.IO.File.Copy(local_path_file, destination, true);
                        // }
                        // else
                        // {
                        //    if (MyUtility.Check.Empty(dr["FileNameOri"]))
                        //    {
                        //        dr["FileName"] = string.Empty;
                        //    }
                        //    else
                        //    {
                        //        dr["FileName"] = dr["FileNameOri"];
                        //    }

                        // //MyUtility.Msg.WarningBox($"File: {local_path_file} not exists!");
                        // }
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: update file fail. Original error: " + exception.Message);
                    }
                }
            }

            string saveDetail = $@"
merge ShippingMarkPic_Detail t
using #tmp s
on t.ShippingMarkPicUkey = s.ShippingMarkPicUkey and t.SCICtnNo = s.SCICtnNo
when matched then update set 
	t.FileName = s.FileName
when not matched by target then 	
	insert(ShippingMarkPicUkey,SCICtnNo,FileName)
	values(s.ShippingMarkPicUkey,s.SCICtnNo,s.FileName)
when not matched by source and t.ShippingMarkPicUkey in (select ShippingMarkPicUkey from #tmp) then 
	delete
;
";
            DataTable odt;
            return MyUtility.Tool.ProcessWithDatatable(dt, string.Empty, saveDetail, out odt);
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            this.ChangCell();
        }

        protected override DualResult ClickDelete()
        {
            string sqldelete = $@"
delete ShippingMarkPic where ukey = {this.CurrentMaintain["Ukey"]}
delete ShippingMarkPic_Detail where ShippingMarkPicUkey = {this.CurrentMaintain["Ukey"]}
";

            foreach (DataRow dr in this.DetailDatas)
            {
                string destination_fileName = MyUtility.Convert.GetString(dr["FileName"]);
                try
                {
                    string destination = Path.Combine(this.destination_path, destination_fileName);
                    if (System.IO.File.Exists(destination))
                    {
                        if (System.IO.File.Exists(destination))
                        {
                            System.IO.File.Delete(destination);
                        }
                    }
                }
                catch (IOException exception)
                {
                    MyUtility.Msg.ErrorBox("Error: update file fail. Original error: " + exception.Message);
                }
            }

            return DBProxy.Current.Execute(null, sqldelete);
        }

        protected override void ClickDeleteAfter()
        {
            base.ClickDeleteAfter();
            this.ReloadDatas();
        }

        private void txtPackingListID_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtPackingListID.Text))
            {
                ((DataTable)this.detailgridbs.DataSource).Clear();
                return;
            }

            string sqlcmd = $@"
select 
	pd.OrderID,
	o.CustPONO,
	pd.CTNStartNo,
	pd.CustCTN,
	pd.SCICtnNo,
	pd.Article,
	pd.Color,
	pd.SizeCode,
    pd.Refno,
    FileName='',
    FileNameOri='',
    local_file_type='',
    FileSourcePath='',
    FileAction='',
    ShippingMarkPicUkey=0
from PackingList_Detail pd with(nolock)
inner join orders o with(nolock) on o.id = pd.OrderID
where pd.id = '{this.txtPackingListID.Text}'
order by SCICtnNo
";
            DataTable ddt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out ddt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (ddt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            this.detailgridbs.DataSource = ddt;
            this.ChangCell();

            #region 帶出Side  & Seq選單

            this.SetComboSeqAndSide();

            this.CurrentMaintain["PackingListID"] = this.txtPackingListID.Text;
            this.CurrentMaintain["Seq"] = MyUtility.Check.Empty(this.comboSeq.Text) ? "0" : this.comboSeq.Text;
            this.CurrentMaintain["Side"] = this.cmbSide.Text;
            #endregion
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            if (this.detailgridbs.DataSource == null)
            {
                return;
            }

            if (((DataTable)this.detailgridbs.DataSource).Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return;
            }

            var detailList = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().GroupBy(s => s["SCICtnNo"], (ctn, a) => new
            {
                SCICtnNo = MyUtility.Convert.GetString(ctn),
                OrderID = MyUtility.Convert.GetString(a.First()["OrderID"]),
                CustPONO = MyUtility.Convert.GetString(a.First()["CustPONO"]),
                CTNStartNo = MyUtility.Convert.GetString(a.First()["CTNStartNo"]),
                CustCTN = MyUtility.Convert.GetString(a.First()["CustCTN"]),
                Article = MyUtility.Convert.GetString(a.First()["Article"]),
                Color = MyUtility.Convert.GetString(a.First()["Color"]),
                SizeCode = MyUtility.Convert.GetString(a.First()["SizeCode"]),
                FileName = MyUtility.Convert.GetString(a.First()["FileName"]),
            }).ToList();

            #region To Excel
            string excelName = "Packing_P24_Download";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{excelName}.xltx");
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];

            int rownum = 2;
            foreach (var dr in detailList)
            {
                worksheet.Cells[rownum, 1] = dr.OrderID;
                worksheet.Cells[rownum, 2] = dr.CustPONO;
                worksheet.Cells[rownum, 3] = this.CurrentMaintain["PackingListID"];
                worksheet.Cells[rownum, 4] = dr.CTNStartNo;
                worksheet.Cells[rownum, 5] = dr.CustCTN;
                worksheet.Cells[rownum, 6] = dr.SCICtnNo;
                worksheet.Cells[rownum, 7] = dr.Article;
                worksheet.Cells[rownum, 8] = dr.Color;
                worksheet.Cells[rownum, 9] = dr.SizeCode;
                worksheet.Cells[rownum, 10] = dr.FileName;
                rownum++;
            }

            worksheet.get_Range((Excel.Range)worksheet.Cells[2, 10], (Excel.Range)worksheet.Cells[rownum - 1, 15]).Interior.Color = Color.FromArgb(255, 199, 206);

            worksheet.Columns.AutoFit();
            #endregion
            #region 釋放上面開啟過excel物件
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Excel.Workbook workbook = excelApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excelApp.Quit();

            if (worksheet != null)
            {
                Marshal.FinalReleaseComObject(worksheet);
            }

            if (excelApp != null)
            {
                Marshal.FinalReleaseComObject(excelApp);
            }
            #endregion
            strExcelName.OpenFile();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dtexcel;
            if (!this.EditMode)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.destination_path))
            {
                if (MyUtility.Msg.QuestionBox("ShippingMarkPath is not set. The file will not be uploaded ! Continue Import?") == DialogResult.No)
                {
                    return;
                }
            }

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Excel files (*.xlsx;*.xls;*.xlt)|*.xlsx;*.xls;*.xlt";

            // 開窗且有選擇檔案
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string msg;

                dtexcel = this.GetExcel(openFileDialog1.FileName, out msg);
                if (!MyUtility.Check.Empty(msg))
                {
                    MyUtility.Msg.ErrorBox(msg);
                    return;
                }

                if (MyUtility.Check.Empty(dtexcel.Columns["Pack ID"]) || MyUtility.Check.Empty(dtexcel.Columns["SCI Ctn No."]) ||
                    MyUtility.Check.Empty(dtexcel.Columns["Shipping Mark Pic File"]))
                {
                    MyUtility.Msg.WarningBox("excel file format error !!");
                    return;
                }

                for (int i = dtexcel.Rows.Count - 1; i > 0; i--)
                {
                    if (MyUtility.Check.Empty(dtexcel.Rows[i]["Pack ID"]))
                    {
                        dtexcel.Rows[i].Delete();
                    }
                }

                #region 檢查PackingListID是否與表頭相同
                foreach (DataRow dr in dtexcel.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted &&
                        !MyUtility.Convert.GetString(dr["Pack ID"]).EqualString(MyUtility.Convert.GetString(this.CurrentMaintain["PackingListID"])))
                    {
                        MyUtility.Msg.WarningBox("Pack ID not the same !!");
                        return;
                    }
                }
                #endregion

                var excelist = dtexcel.AsEnumerable().GroupBy(s => s["SCI Ctn No."], (ctn, a) => new
                {
                    SCICtnNo = ctn,
                    FileName = a.First()["Shipping Mark Pic File"],
                });

                foreach (var item in excelist)
                {
                    foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Select($"SCICtnNo='{item.SCICtnNo}'"))
                    {
                        if (!MyUtility.Check.Empty(item.FileName) && !MyUtility.Check.Empty(this.destination_path))
                        {
                            dr["FileSourcePath"] = item.FileName;
                            dr["FileName"] = item.FileName;
                            dr["local_file_type"] = Path.GetExtension(MyUtility.Convert.GetString(item.FileName));
                            dr["FileAction"] = "Upload";
                        }
                        else if (MyUtility.Check.Empty(item.FileName))
                        {
                            dr["FileSourcePath"] = string.Empty;
                            dr["FileName"] = string.Empty;
                            dr["local_file_type"] = string.Empty;
                            dr["FileAction"] = "Delete";
                        }
                    }
                }

                MyUtility.Msg.InfoBox("Import Success !!");
            }
        }

        /// <summary>
        /// GetExcel
        /// </summary>
        /// <param name="strPath">strPath</param>
        /// <param name="strMsg">strMsg</param>
        /// <returns>DataTable</returns>
        public DataTable GetExcel(string strPath, out string strMsg)
        {
            try
            {
                Excel.Application xlsApp = new Excel.Application();
                xlsApp.Visible = false;
                Excel.Workbook xlsBook = xlsApp.Workbooks.Open(strPath);
                Excel.Worksheet xlsSheet = xlsBook.ActiveSheet;
                Excel.Range xlsRangeFirstCell = xlsSheet.get_Range("A1");
                Excel.Range xlsRangeLastCell = xlsSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);
                Excel.Range xlsRange = xlsSheet.get_Range(xlsRangeFirstCell, xlsRangeLastCell);
                object[,] objValue = xlsRange.Value2 as object[,];

                // Array[][] to DataTable
                long lngColumnCount = 10;
                long lngRowCount = objValue.GetLongLength(0);
                DataTable dtExcel = new DataTable();
                for (int j = 1; j <= lngColumnCount; j++)
                {
                    dtExcel.Columns.Add(objValue[1, j].ToString());
                }

                for (int i = 2; i <= lngRowCount; i++)
                {
                    DataRow drRow = dtExcel.NewRow();
                    for (int j = 1; j <= lngColumnCount; j++)
                    {
                        drRow[j - 1] = MyUtility.Check.Empty(objValue[i, j]) ? string.Empty : objValue[i, j].ToString();
                    }

                    dtExcel.Rows.Add(drRow);
                }

                xlsBook.Close();
                xlsApp.Quit();
                strMsg = string.Empty;
                return dtExcel;
            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                return null;
            }
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        private void SetComboSeqAndSide()
        {
            if (this.detailgrid == null)
            {
                return;
            }

            DualResult result;
            string firstDetailSP = string.Empty;
            DataRow drOrder = null;
            if (this.detailgrid.Rows.Count > 0)
            {
                firstDetailSP = this.detailgrid.Rows[0].Cells["OrderID"].Value.ToString();
                MyUtility.Check.Seek($"select BrandID from orders with (nolock) where ID = '{firstDetailSP}'", out drOrder);
            }

            if (drOrder == null)
            {
                return;
            }

            string sqlGetShippingMarkPicture = $@"select distinct Side,Seq from ShippingMarkPicture where BrandID = '{drOrder["BrandID"]}' order by Seq";
            DataTable dtSeqSideSource;
            result = DBProxy.Current.Select(null, sqlGetShippingMarkPicture, out dtSeqSideSource);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.comboSeq.DataSource = dtSeqSideSource.AsEnumerable().Select(s => new { Seq = s["Seq"] }).Distinct().ToList();
            this.comboSeq.DisplayMember = "Seq";
            this.comboSeq.ValueMember = "Seq";

            this.cmbSide.DataSource = dtSeqSideSource.AsEnumerable().Select(s => new { Side = s["Side"] }).Distinct().ToList();
            this.cmbSide.DisplayMember = "Side";
            this.cmbSide.ValueMember = "Side";
        }
    }
}
