using Ict;
using Ict.Win;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using Sci.Data;
using Sci.Production.Automation;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P26 : Sci.Win.Tems.Base
    {
        private DataTable Grid_SelectedFile_Dt = new DataTable();
        private DataTable Grid_Match_Dt = new DataTable();
        private Dictionary<string, List<ZPL>> File_Name_Object = new Dictionary<string, List<ZPL>>();
        private File_Name_Object_List _File_Name_Object_List;
        private Dictionary<string, string> File_Name_PDF = new Dictionary<string, string>();
        private List<string> wattingForConvert = new List<string>();
        private List<string> wattingForConvert_contentsOfZPL = new List<string>();
        private List<Match> MatchList = new List<Match>();
        private List<PackingListCandidate_Datasource> PackingListCandidate_Datasources = new List<PackingListCandidate_Datasource>();
        private List<Result> ConfirmMsg = new List<Result>();

        /// <summary>
        /// 目前處理的檔案格式
        /// </summary>
        private UploadType currentFileType;
        private List<string> mappingFailFileName = new List<string>();

        /// <inheritdoc/>
        public P26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.Grid_SelectedFile_Dt.ColumnsIntAdd("FileSeq");
            this.Grid_SelectedFile_Dt.ColumnsStringAdd("FileName");
            this.Grid_SelectedFile_Dt.ColumnsStringAdd("Result");

            this.Grid_Match_Dt.ColumnsStringAdd("PO");
            this.Grid_Match_Dt.ColumnsIntAdd("FileSeq");
            this.Grid_Match_Dt.ColumnsBooleanAdd("MultipleMatches");
            this.Grid_Match_Dt.ColumnsStringAdd("PackingListCandidate");
            this.Grid_Match_Dt.ColumnsBooleanAdd("NoStickerBasicSetting");
            this.Grid_Match_Dt.ColumnsBooleanAdd("StickerAlreadyexisted");
            this.Grid_Match_Dt.ColumnsBooleanAdd("CtnInClog");
            this.Grid_Match_Dt.ColumnsBooleanAdd("Overwrite");
            this.Grid_Match_Dt.ColumnsBooleanAdd("IsMixPack");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            this.EditMode = true;
            base.OnFormLoaded();
            this.gridSelectedFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridSelectedFile)
.Numeric("FileSeq", header: $"File{Environment.NewLine}Seq", width: Widths.AnsiChars(3), minimum: null)
.Text("FileName", header: $"File{Environment.NewLine}Name ", width: Widths.AnsiChars(10))
.Text("Result", header: "Result", width: Widths.AnsiChars(20))
;

            DataGridViewGeneratorTextColumnSettings col_PackingListCandidate = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings col_Overwrite = new DataGridViewGeneratorCheckBoxColumnSettings();

            col_PackingListCandidate.CellMouseClick += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridMatch.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;

                    int fileSeq = MyUtility.Convert.GetInt(dr["FileSeq"]);

                    PackingListCandidate_Datasource pData = this.PackingListCandidate_Datasources.Where(o => o.FileSeq == fileSeq).FirstOrDefault();

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn() { ColumnName = "PackingList ID", DataType = typeof(string) });

                    foreach (var packingListID in pData.PackingList_Candidate)
                    {
                        dt.Rows.Add(dt.NewRow()["PackingList ID"] = packingListID);
                    }

                    item = new Win.Tools.SelectItem(dt, "PackingList ID", "15", this.Text, false, ",", "PackingList ID");

                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var selectedData = item.GetSelecteds();
                    dr["PackingListCandidate"] = selectedData[0]["PackingList ID"].ToString();
                    dr["OverWrite"] = false;
                    this.GetInfoByPackingList(dr);
                    this.MatchList.Where(o => o.FileSeq == MyUtility.Convert.GetInt(dr["FileSeq"])).FirstOrDefault().SelectedPackingID = MyUtility.Convert.GetString(dr["PackingListCandidate"]);
                }
            };

            col_PackingListCandidate.EditingMouseDown += (s, e) =>
            {
                if (!this.EditMode || e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridMatch.GetDataRow<DataRow>(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem item;

                    int fileSeq = MyUtility.Convert.GetInt(dr["FileSeq"]);

                    PackingListCandidate_Datasource pData = this.PackingListCandidate_Datasources.Where(o => o.FileSeq == fileSeq).FirstOrDefault();

                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn() { ColumnName = "PackingList ID", DataType = typeof(string) });

                    foreach (var packingListID in pData.PackingList_Candidate)
                    {
                        dt.Rows.Add(dt.NewRow()["PackingList ID"] = packingListID);
                    }

                    item = new Win.Tools.SelectItem(dt, "PackingList ID", "15", this.Text, false, ",", "PackingList ID");

                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    var selectedData = item.GetSelecteds();
                    dr["PackingListCandidate"] = selectedData[0]["PackingList ID"].ToString();
                    dr["OverWrite"] = false;
                    this.GetInfoByPackingList(dr);
                    this.MatchList.Where(o => o.FileSeq == MyUtility.Convert.GetInt(dr["FileSeq"])).FirstOrDefault().SelectedPackingID = MyUtility.Convert.GetString(dr["PackingListCandidate"]);
                }
            };

            col_PackingListCandidate.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridMatch.GetDataRow(e.RowIndex);
                string oldvalue = dr["PackingListCandidate"].ToString();
                string newvalue = e.FormattedValue.ToString();
                int fileSeq = MyUtility.Convert.GetInt(dr["FileSeq"]);

                if (this.EditMode && oldvalue.ToUpper() != newvalue.ToUpper() /* && newvalue.ToUpper() != "PLEASE SELECT"*/)
                {
                    if (!this.PackingListCandidate_Datasources.Where(o => o.FileSeq == fileSeq && o.PackingList_Candidate.Contains(newvalue)).Any() && newvalue != string.Empty)
                    {
                        this.MatchList.Where(o => o.FileSeq == MyUtility.Convert.GetInt(dr["FileSeq"])).FirstOrDefault().SelectedPackingID = MyUtility.Convert.GetString(dr["PackingListCandidate"]);
                        dr["PackingListCandidate"] = oldvalue;
                        dr.EndEdit();
                        this.MatchGridColor();
                        this.GetInfoByPackingList(dr);
                        MyUtility.Msg.WarningBox("Data not found");
                        return;
                    }
                    else
                    {
                        dr["PackingListCandidate"] = newvalue;
                        dr["OverWrite"] = false;
                        this.MatchList.Where(o => o.FileSeq == MyUtility.Convert.GetInt(dr["FileSeq"])).FirstOrDefault().SelectedPackingID = MyUtility.Convert.GetString(dr["PackingListCandidate"]);
                        dr.EndEdit();
                        this.GetInfoByPackingList(dr);
                    }
                }
                this.MatchGridColor();
            };

            col_Overwrite.CellEditable += (s, e) =>
            {
                DataRow dr = this.gridMatch.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetBool(dr["StickerAlreadyExisted"]))
                {
                    e.IsEditable = true;
                }
                else
                {
                    e.IsEditable = false;
                }
            };

            col_Overwrite.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridMatch.GetDataRow(e.RowIndex);
                dr["OverWrite"] = e.FormattedValue;
                dr.EndEdit();
                this.MatchGridColor();
                this.GetInfoByPackingList(dr);
            };

            this.gridMatch.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridMatch)
.Text("PO", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
.Numeric("FileSeq", header: "File" + Environment.NewLine + "Seq ", width: Widths.AnsiChars(5), iseditingreadonly: true)
.CheckBox("MultipleMatches", header: "Multiple" + Environment.NewLine + "Matches", width: Widths.AnsiChars(4), iseditable: false)
.Text("PackingListCandidate", header: "P/L" + Environment.NewLine + "Candidate", width: Widths.AnsiChars(13), iseditingreadonly: false, settings: col_PackingListCandidate)
.CheckBox("NoStickerBasicSetting", header: "No Sticker" + Environment.NewLine + "Basic Setting", width: Widths.AnsiChars(4), iseditable: false)
.CheckBox("StickerAlreadyexisted", header: "Sticker" + Environment.NewLine + "already existed", width: Widths.AnsiChars(4), iseditable: false)
.CheckBox("CtnInClog", header: "Ctn" + Environment.NewLine + "In Clog", width: Widths.AnsiChars(4), iseditable: false)
.CheckBox("Overwrite", header: "Overwrite", trueValue: 1, falseValue: 0, width: Widths.AnsiChars(5), iseditable: true, settings: col_Overwrite)
;

            this.Helper.Controls.Grid.Generator(this.gridErrorMsg)
.Numeric("FileSeq", header: "File Seq", width: Widths.AnsiChars(5), minimum: null)
.Text("Result", header: "Result", width: Widths.AnsiChars(10))
;
            this.gridErrorMsg.Columns["Result"].DefaultCellStyle.ForeColor = Color.Red;
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = "ZPL files (*.zpl)|*.zpl|(*.pdf)|*.pdf",
                Multiselect = true,
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.ShowWaitMessage("Processing....");
                this.ResetAll();

                // ZPL格式
                if (openFileDialog1.SafeFileNames.Any(o => !o.ToString().ToLower().EndsWith(".pdf") && o.ToString().ToLower().EndsWith(".zpl")))
                {
                    this.currentFileType = UploadType.ZPL;
                }

                // PDF格式
                if (openFileDialog1.SafeFileNames.Any(o => o.ToString().ToLower().EndsWith(".pdf") && !o.ToString().ToLower().EndsWith(".zpl")))
                {
                    this.currentFileType = UploadType.PDF;
                }

                // 取得所有檔名
                string[] files = openFileDialog1.FileNames;

                try
                {
                    int i = 0;
                    foreach (string file in files)
                    {
                        // 取得CustCTN，作為檔名
                        List<string> zPL_FileName_List = new List<string>();

                        string oriZplConten; // 原始的ZPL檔內容
                        string tmpzplContent; // 將原始內容去除換行符號

                        string[] tmpArray; // 取得CustCTN過程中，暫存用
                        string[] contentsOfZPL; // 從原始ZPL檔拆出來的多個ZPL檔

                        try
                        {
                            #region ZPL

                            // 若上傳的ZPL檔，包含多張ZPL，先拆成個別ZPL
                            if (this.currentFileType == UploadType.ZPL)
                            {
                                using (StreamReader reader = new StreamReader(MyUtility.Convert.GetString(file), System.Text.Encoding.UTF8))
                                {
                                    // 1-1.讀取內容
                                    oriZplConten = reader.ReadToEnd();

                                    // 1-2.去除換行符號
                                    tmpzplContent = oriZplConten.Replace("\r\n", string.Empty).Replace("^LH0,0", "^LH10,0");

                                    // 1-3.先取得檔名，CustCTN被包在 ^FD>;>8 和 ^FS之間，取得CustCTN，作為檔名
                                    tmpArray = tmpzplContent.Split(new string[] { "^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH10,0^LRN^XZ^XA^CI0^FO80,50^BY4^BCN,200,N,N,^FD>;>8", "^FS^FT115,280^A0N,34,47^FD" }, StringSplitOptions.RemoveEmptyEntries);
                                    zPL_FileName_List = tmpArray.Where(o => !o.Contains("^")).Distinct().ToList();

                                    // 1-4.拆出多個ZPL檔的內容，每一個ZPL都是以 ^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH10,0^LRN^XZ^XA^CI0 開頭
                                    tmpzplContent = tmpzplContent.Replace("^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH10,0^LRN^XZ^XA^CI0", "\r\n^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH10,0^LRN^XZ^XA^CI0");

                                    string[] stringSeparators = new string[] { "\r\n" };

                                    // 1-5.最後拆出來的每一個ZPL，包含三張圖片
                                    contentsOfZPL = tmpzplContent.Split(stringSeparators, StringSplitOptions.None);
                                }

                                // 2.根據ZPL檔名，取得對應的內容
                                Dictionary<string, string> fileName_with_Data = new Dictionary<string, string>();

                                foreach (string singleFileName in zPL_FileName_List)
                                {
                                    string contentString = contentsOfZPL.Where(o => o.Contains("^FD>;>8" + singleFileName + "^FS")).FirstOrDefault();
                                    fileName_with_Data.Add(singleFileName, contentString);
                                }

                                // 3.從單張ZPL內容中，拆解出需要的欄位資訊，用於Mapping方便
                                List<ZPL> zPL_Objects = this.Analysis_ZPL(fileName_with_Data, zPL_FileName_List);

                                // 4.成功解析的ZPL存起來，用於後續轉圖片
                                this.wattingForConvert.AddRange(zPL_FileName_List);
                                this.wattingForConvert_contentsOfZPL.AddRange(contentsOfZPL.Where(o => o != string.Empty).ToList());

                                this.File_Name_Object.Add(openFileDialog1.SafeFileNames[i], zPL_Objects);
                                this._File_Name_Object_List.File_Name_Object2s.Add(new FileName_Key_Model()
                                {
                                    FileName = openFileDialog1.SafeFileNames[i],
                                    ZPLs = zPL_Objects,
                                });
                            }
                            #endregion

                            #region PDF

                            // PDF一個檔案只有一張
                            if (this.currentFileType == UploadType.PDF)
                            {
                                FileInfo fileInfo = new FileInfo(file);
                                PDDocument doc = PDDocument.load(fileInfo.FullName);
                                PDFTextStripper pdfStripper = new PDFTextStripper();
                                oriZplConten = pdfStripper.getText(doc);
                                string[] stringSeparators = new string[] { "\r\n" };
                                string[] tmppArray = oriZplConten.Split(stringSeparators, StringSplitOptions.None);
                                bool isMixed = false;
                                List<ZPL> zPL_Objects = new List<ZPL>();

                                foreach (var s in tmppArray)
                                {
                                    if (s.ToUpper() == "MIXED")
                                    {
                                        isMixed = true;
                                    }
                                }

                                // 若是混尺碼則分開處理
                                if (isMixed)
                                {
                                    string[] sizes = tmppArray[Array.IndexOf(tmppArray, tmppArray.Where(o => o == "Size/Qty").FirstOrDefault()) + 1].Split(' ');
                                    string[] qtyOfSizes = tmppArray[Array.IndexOf(tmppArray, tmppArray.Where(o => o == "Size/Qty").FirstOrDefault()) + 2].Split(' ');

                                    List<SizeObject> size_qty = new List<SizeObject>();
                                    List<MixedCompare> mixedCompares = new List<MixedCompare>();

                                    // 判斷是否有另外小包裝，# of Prepacks: 2 of 12這類的文字，沒有的話全部都算 1 就好
                                    string tmpStr = string.Empty;
                                    int q = 0;
                                    int getMixInfoIndex = tmppArray.ToList().IndexOf(tmppArray.Where(o => o.Contains("of ")).FirstOrDefault());

                                    if (getMixInfoIndex != -1)
                                    {
                                        for (int ix = 0; ix <= tmppArray[getMixInfoIndex].Length - 1; ix++)
                                        {
                                            mixedCompares.Add(new MixedCompare()
                                            {
                                                Text = tmppArray[getMixInfoIndex][ix].ToString(),
                                                IsInt = int.TryParse(tmppArray[getMixInfoIndex][ix].ToString(), out q),
                                            });
                                        }

                                        foreach (var item in mixedCompares)
                                        {
                                            if (tmpStr != string.Empty && !item.IsInt)
                                            {
                                                break;
                                            }

                                            if (item.IsInt)
                                            {
                                                tmpStr += item.Text;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tmpStr = "1";
                                    }

                                    // 得到每個小包裝的數量，再拿下去乘
                                    int qtyPerSmallPack = Convert.ToInt32(tmpStr);

                                    // 每個尺寸的數量
                                    for (int ix = 0; ix <= sizes.Length - 1; ix++)
                                    {
                                        size_qty.Add(new SizeObject()
                                        {
                                            Size = sizes[ix],
                                            Qty = Convert.ToInt32(qtyOfSizes[ix]) * qtyPerSmallPack,
                                        });
                                    }

                                    // 每個小包的總和，跟Qty的數字沒對上直接return
                                    int mIXED_textindex = tmppArray.ToList().IndexOf("MIXED");
                                    int qty_Index = mIXED_textindex + 4;
                                    if (size_qty.Sum(o => o.Qty) != Convert.ToInt32(tmppArray[qty_Index]))
                                    {
                                        MyUtility.Msg.InfoBox($"CustCTN: {tmppArray[0].Split(' ')[1]}, Total of <Size/Qty> is not equal <Qty> !!");
                                        this.HideWaitMessage();
                                        return;
                                    }

                                    int qty_textindex = tmppArray.ToList().IndexOf("Qty:");
                                    int sSCC_textindex = tmppArray.ToList().IndexOf("SSCC");

                                    int custCTN_Index = sSCC_textindex - 1;
                                    int custPONo_index = qty_textindex + 1;
                                    int style_index = qty_textindex + 2;
                                    int sizeCode_index = qty_textindex + 4;
                                    int shipQty_index = qty_textindex + 8;
                                    zPL_Objects.Add(
                                         new ZPL()
                                         {
                                             CustCTN = tmppArray[custCTN_Index].Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty),
                                             CustPONo = tmppArray[custPONo_index],
                                             StyleID = tmppArray[style_index].Split('-')[0],
                                             Article = tmppArray[style_index].Split('-')[1],
                                             SizeCode = tmppArray[sizeCode_index], // MIXED
                                             ShipQty = tmppArray[shipQty_index],
                                             Size_Qty_List = size_qty,
                                         });
                                }
                                else
                                {
                                    int qty_textindex = tmppArray.ToList().IndexOf("Qty:");
                                    int sSCC_textindex = tmppArray.ToList().IndexOf("SSCC");

                                    int custCTN_Index = sSCC_textindex - 1;
                                    int custPONo_index = qty_textindex + 1;
                                    int style_index = qty_textindex + 2;
                                    int sizeCode_index = qty_textindex + 4;
                                    int shipQty_index = qty_textindex + 8;

                                    zPL_Objects.Add(
                                         new ZPL()
                                         {
                                             CustCTN = tmppArray[custCTN_Index].Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty),
                                             CustPONo = tmppArray[custPONo_index],
                                             StyleID = tmppArray[style_index].Split('-')[0],
                                             Article = tmppArray[style_index].Split('-')[1],
                                             SizeCode = tmppArray[sizeCode_index],
                                             ShipQty = tmppArray[shipQty_index],
                                         });
                                }

                                this.File_Name_Object.Add(openFileDialog1.SafeFileNames[i], zPL_Objects);
                                this._File_Name_Object_List.File_Name_Object2s.Add(new FileName_Key_Model()
                                {
                                    FileName = openFileDialog1.SafeFileNames[i],
                                    ZPLs = zPL_Objects,
                                });

                                this.File_Name_PDF.Add(fileInfo.FullName, zPL_Objects.FirstOrDefault().CustCTN);
                            }
                            #endregion

                            // 解析成功
                            DataRow newDr = this.Grid_SelectedFile_Dt.NewRow();
                            newDr["FileSeq"] = DBNull.Value;
                            newDr["FileName"] = openFileDialog1.SafeFileNames[i];
                            newDr["Result"] = string.Empty;
                            this.Grid_SelectedFile_Dt.Rows.Add(newDr);
                            i++;
                        }
                        catch (Exception q)
                        {
                            // 解析失敗：Result寫入訊息
                            DataRow newDr = this.Grid_SelectedFile_Dt.NewRow();
                            newDr["FileSeq"] = DBNull.Value;
                            newDr["FileName"] = openFileDialog1.SafeFileNames[i];
                            newDr["Result"] = "Analysis failed.";
                            this.Grid_SelectedFile_Dt.Rows.Add(newDr);
                            i++;
                        }
                    }

                    this.listControlBindingSource1.DataSource = this.Grid_SelectedFile_Dt;
                }
                catch (Exception ex)
                {
                    this.ShowErr(ex);
                }

                this.HideWaitMessage();
            }
        }

        /// <summary>
        /// 從單張ZPL內容中，拆解出需要的欄位資訊
        /// </summary>
        /// <returns>整理完的ZPL物件</returns>
        private List<ZPL> Analysis_ZPL(Dictionary<string, string> fileName_with_Data, List<string> zPL_FileName_List)
        {
            List<ZPL> list = new List<ZPL>();

            foreach (string custCTNno in zPL_FileName_List)
            {
                string content = fileName_with_Data[custCTNno];
                bool isMixed = false;
                string zplType = string.Empty;

                // 是否混尺碼
                isMixed = content.ToUpper().Contains("^FD" + "MIXED" + "^FS");

                // 取得CustCD，後續用於判斷ZPL的格式
                // string custCD = this.GetCustCDFromZPL(content);

                if (!isMixed)
                {
                    // 非混尺碼
                    string custPONo = this.GetPOnoFromZPL(content);
                    string sku = this.GetSKUFromZPL(content);

                    string styleID = sku.Split('-')[0];
                    string article = sku.Split('-')[1];
                    string sizeCode = sku.Split('-')[2];
                    string ctnStartno = this.GetCTNStartnoFromZPL(content);
                    string shipQty = this.GetShipQtyFromZPL(content);
                    string custCTN = this.GetCustCTNFromZPL(content);

                    ZPL zz = new ZPL
                    {
                        CustPONo = custPONo,
                        StyleID = styleID,
                        Article = article,
                        SizeCode = sizeCode,
                        CTNStartNo = ctnStartno,
                        ShipQty = shipQty,
                        CustCTN = custCTN,
                    };
                    list.Add(zz);
                }
                else
                {
                    // 混尺碼
                    string custPONo = this.GetPOnoFromZPL(content);
                    string ctnStartno = this.GetCTNStartnoFromZPL(content);
                    string custCTN = this.GetCustCTNFromZPL(content);
                    List<SizeObject> sizeObjects = this.GetMixSizeObjectFromZPL(content);
                    string shipQty = this.GetShipQtyFromZPL(content);

                    if (sizeObjects.Select(o => o.StyleID).Distinct().Count() > 1 || sizeObjects.Select(o => o.Article).Distinct().Count() > 1)
                    {
                        MyUtility.Msg.WarningBox($"Not support Mix Article! PO#: {custPONo}, Cust. Carton No.: {custCTN}");
                        return null;
                    }

                    ZPL zz = new ZPL();
                    zz.CustPONo = custPONo;
                    zz.ShipQty = string.Empty;
                    zz.SizeCode = string.Empty;

                    // 現階段只有混尺碼情況，若有多色組則排除
                    zz.StyleID = sizeObjects.FirstOrDefault().StyleID;
                    zz.Article = sizeObjects.FirstOrDefault().Article;

                    zz.CTNStartNo = ctnStartno;
                    zz.CustCTN = custCTN;
                    zz.Size_Qty_List = sizeObjects;
                    list.Add(zz);
                }
            }

            return list;
        }

        private void BtnProcessing_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            try
            {
                #region 初始設定、宣告
                this.mappingFailFileName.Clear();
                List<string> removePOs = new List<string>();
                List<string> removeFileNames = new List<string>();
                List<Match> match_List = new List<Match>();
                string msg = string.Empty;
                this.listControlBindingSource2.DataSource = null;
                this.Grid_Match_Dt.Clear();
                this.PackingListCandidate_Datasources.Clear();
                this.MatchList.Clear();
                this.ConfirmMsg.Clear();
                this.GridBool = false;
                #endregion

                this.ShowWaitMessage("Data Match....");
                #region 開始Match

                if (this.currentFileType == UploadType.PDF)
                {
                    // 所有檔案拆解後
                    List<ZPL> zPLs = new List<ZPL>();
                    foreach (var item in this._File_Name_Object_List.File_Name_Object2s)
                    {
                        // 根據上傳的PDF展開
                        string fileName = item.FileName;
                        ZPL zPL = item.ZPLs.FirstOrDefault();
                        zPL.FileName = fileName;
                        zPLs.Add(zPL);
                    }

                    // 將所有檔案分組，CustPONO、StyleID相同為一組
                    var keys = zPLs.Select(o => new { o.CustPONo, o.StyleID }).Distinct().ToList();
                    List<FileSeq_Key_Model> pDF_Models = new List<FileSeq_Key_Model>();

                    foreach (var key in keys)
                    {
                        FileSeq_Key_Model m = new FileSeq_Key_Model();
                        m.UpdateModels = zPLs.Where(o => o.CustPONo == key.CustPONo && o.StyleID == key.StyleID).ToList();

                        pDF_Models.Add(m);
                    }

                    int fileSeq = 1;
                    foreach (var item in pDF_Models)
                    {
                        Match matchData = new Match()
                        {
                            PDFFile = item,
                        };

                        List<ZPL> ZPLs = item.UpdateModels;
                        bool isMixed = ZPLs.Where(o => o.SizeCode == string.Empty || o.SizeCode.ToUpper() == "MIXED").Any();
                        matchData = this.PDF_Match(ZPLs, matchData, isMixed);

                        // 至少對應到一個箱子才需要顯示在右邊
                        if (matchData.PackingList_Candidate.Count > 0)
                        {
                            item.FileSeq = fileSeq;
                            matchData.FileSeq = fileSeq;
                            PackingListCandidate_Datasource pDatas = new PackingListCandidate_Datasource()
                            {
                                FileSeq = item.FileSeq,
                            };
                            pDatas.PackingList_Candidate = matchData.PackingList_Candidate.Distinct().ToList();
                            this.PackingListCandidate_Datasources.Add(pDatas);
                            match_List.Add(matchData);

                            foreach (var file in item.UpdateModels)
                            {
                                // 左邊Grid寫入FileSeq
                                this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == file.FileName).FirstOrDefault()["FileSeq"] = matchData.FileSeq;

                                // 左邊Result寫入資訊：沒有對應的Packing List
                                if (matchData.PackingList_Candidate.Count == 0)
                                {
                                    this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == file.FileName).FirstOrDefault()["Result"] = "Cannot mapping current P/L.";
                                }
                            }

                            fileSeq++;
                        }
                        else
                        {
                            foreach (var obj in item.UpdateModels)
                            {
                                string filename = obj.FileName;
                                this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == filename).FirstOrDefault()["Result"] = "Cannot mapping current P/L.";
                            }
                        }
                    }
                }

                if (this.currentFileType == UploadType.ZPL)
                {
                    int fileSeq = 1;
                    foreach (var item in this._File_Name_Object_List.File_Name_Object2s)
                    {
                        Match matchData = new Match()
                        {
                            ZPLFile = item,
                        };

                        // 根據上傳的ZPL展開
                        string fileName = item.FileName;
                        List<ZPL> zPLs = item.ZPLs;

                        // PackingList只要"有一箱"是混尺碼，則整張都是混尺碼
                        bool isMixed = zPLs.Where(o => o.SizeCode == string.Empty).Any();

                        matchData = this.ZPL_Match(zPLs, fileName, matchData, isMixed);

                        // 至少對應到一個箱子才需要顯示在右邊
                        if (matchData.PackingList_Candidate.Count() > 0)
                        {
                            matchData.FileSeq = fileSeq;

                            PackingListCandidate_Datasource pDatas = new PackingListCandidate_Datasource()
                            {
                                FileSeq = fileSeq,
                            };

                            pDatas.PackingList_Candidate = matchData.PackingList_Candidate.Distinct().ToList();

                            this.PackingListCandidate_Datasources.Add(pDatas);
                            match_List.Add(matchData);

                            // 左邊Grid寫入FileSeq
                            this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == fileName).FirstOrDefault()["FileSeq"] = fileSeq;

                            // 左邊Result寫入資訊：沒有對應的Packing List
                            if (matchData.PackingList_Candidate.Count == 0)
                            {
                                this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == fileName).FirstOrDefault()["Result"] = "Cannot mapping current P/L.";
                            }

                            fileSeq++;
                        }
                        else
                        {
                            string filename = item.FileName;
                            this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == filename).FirstOrDefault()["Result"] = "Cannot mapping current P/L.";
                        }
                    }
                }
                #endregion

                #region 同時上傳相同PO + SSCC的處理方式
                var ee = match_List.ToList();
                List<int> dplicateFileSeq = new List<int>();

                foreach (Match match in ee)
                {
                    var pO_CustCTNs = match.UpdateModels.Select(o => new { o.CustPONO, o.CustCTN }).Distinct().ToList();

                    // 取得其他FileSeq，有相同PO + CustCTN的資料
                    var isDuplicateCustCTN = match_List/*.Where(o => o.FileSeq != match.FileSeq)*/.Where(o => o.UpdateModels.Any(x => pO_CustCTNs.Any(t => t.CustPONO == x.CustPONO && t.CustCTN == x.CustCTN)));

                    // 包含自己，所以要 > 1
                    if (isDuplicateCustCTN.Count() > 1)
                    {
                        // 取得FileSeq
                        string sameFileSeq = isDuplicateCustCTN.OrderBy(o => o.FileSeq).Select(o => o.FileSeq.ToString()).JoinToString(",");

                        this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetInt(o["FileSeq"]) == match.FileSeq).FirstOrDefault()["Result"] = $"File Seq [{sameFileSeq}] have duplicate PO# + SSCC";
                        dplicateFileSeq.Add(match.FileSeq);
                    }
                }

                // 這些 File Seq 不進入 Match
                match_List.RemoveAll(o => dplicateFileSeq.Contains(o.FileSeq));
                #endregion

                this.Grid_SelectedFile_Dt = this.Grid_SelectedFile_Dt.AsEnumerable().OrderBy(o => MyUtility.Convert.GetInt(o["FileSeq"])).CopyToDataTable();

                this.listControlBindingSource1.DataSource = this.Grid_SelectedFile_Dt;
                this.MatchList.AddRange(match_List);
                DataTable tmp = this.Grid_Match_Dt.Clone();
                foreach (Match matchData in match_List)
                {
                    DataRow nRow = tmp.NewRow();
                    nRow["PO"] = matchData.PO;
                    nRow["FileSeq"] = matchData.FileSeq;
                    nRow["MultipleMatches"] = matchData.MultipleMatches;
                    nRow["PackingListCandidate"] = matchData.PackingList_Candidate.FirstOrDefault();
                    nRow["NoStickerBasicSetting"] = matchData.NoStickerBasicSetting;
                    nRow["StickerAlreadyExisted"] = matchData.StickerAlreadyExisted;
                    nRow["CtnInClog"] = matchData.CtnInClog;
                    nRow["Overwrite"] = matchData.Overwrite;
                    nRow["IsMixPack"] = matchData.IsMixPack;
                    tmp.Rows.Add(nRow);
                }

                foreach (DataRow dr in tmp.Rows)
                {
                    DataRow newDr = this.Grid_Match_Dt.NewRow();

                    var colList = this.Grid_Match_Dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
                    string columnsName = string.Join(",", colList);

                    this.GetInfoByPackingList(dr).CopyTo(newDr, columnsName);
                    this.Grid_Match_Dt.Rows.Add(newDr);
                }

                this.listControlBindingSource2.DataSource = this.Grid_Match_Dt;

                this.MatchGridColor();
            }
            catch (Exception exp)
            {
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Result"] = "Fail";
                }

                this.ShowErr(exp);
            }
            finally
            {
                this.HideWaitMessage();
            }
        }

        private Match ZPL_Match(List<ZPL> zPLs, string fileName, Match matchData, bool isMixed = false)
        {
            DataTable[] mappingInfo;
            List<ZPL> mixedzPLs = new List<ZPL>();

            matchData.PackingList_Candidate = new List<string>();
            matchData.UpdateModels = new List<UpdateModel>();

            // 若是混尺碼，則把ZPL物件拆開重新整理
            if (isMixed)
            {
                int ctnStartNo = 1;
                foreach (var zPL in zPLs)
                {
                    if (zPL.Size_Qty_List.Count > 0)
                    {
                        foreach (var size_Qty in zPL.Size_Qty_List)
                        {
                            string sizeCode = size_Qty.Size;
                            string shipQty = size_Qty.Qty.ToString();

                            ZPL m = new ZPL()
                            {
                                CustCTN = zPL.CustCTN,
                                CustPONo = zPL.CustPONo,
                                StyleID = zPL.StyleID,
                                Article = zPL.Article,
                                SizeCode = sizeCode,
                                ShipQty = shipQty,
                                CTNStartNo = ctnStartNo.ToString(), // 目前沒用到，不過先編號寫好
                            };
                            mixedzPLs.Add(m);
                        }
                    }
                    else
                    {
                        ZPL m = new ZPL()
                        {
                            CustCTN = zPL.CustCTN,
                            CustPONo = zPL.CustPONo,
                            StyleID = zPL.StyleID,
                            Article = zPL.Article,
                            SizeCode = zPL.SizeCode,
                            ShipQty = zPL.ShipQty,
                            CTNStartNo = ctnStartNo.ToString(), // 目前沒用到，不過先編號寫好
                        };
                        mixedzPLs.Add(m);
                    }

                    ctnStartNo++;
                }

                zPLs = mixedzPLs;

                var keys = zPLs.Select(o => new
                {
                    CustPONO = o.CustPONo,
                    o.StyleID,
                    o.Article,
                    o.SizeCode,
                    o.ShipQty,
                    o.CTNStartNo,
                }).Distinct().ToList();

                foreach (var key in keys)
                {
                    string custPONo = key.CustPONO;
                    string styleID = key.StyleID;
                    string article = key.Article;
                    string sizeCode = key.SizeCode;
                    string shipQty = key.ShipQty;
                    string cTNStartNo = key.CTNStartNo;

                    matchData.PO = custPONo;

                    List<ZPL> sameZPL = zPLs.Where(o => o.CustPONo == custPONo && o.StyleID == styleID && o.Article == article && o.SizeCode == sizeCode && o.ShipQty == shipQty && o.CTNStartNo == cTNStartNo).ToList();

                    foreach (var zPL in sameZPL)
                    {
                        string sqlCmd = string.Empty;

                        // 判斷該"箱"是否混尺碼
                        // bool isCartonMix = this.Chk_Carton_Mix(currentCustPONo, currentStyleID, currentArticle, currentSizeCode, currentShipQty);
                        sqlCmd = this.Get_MatchSQL(zPL, true);

                        DBProxy.Current.Select(null, sqlCmd, out mappingInfo);

                        // Mapping到多少個PackingList_ID
                        int mapped_PackingLisID_Count = mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count();

                        // 準備Multipie Matches
                        matchData.MultipleMatches = mapped_PackingLisID_Count > 1 ? true : false;

                        // 準備P/L Candidate清單
                        if (mapped_PackingLisID_Count > 1)
                        {
                            matchData.PackingList_Candidate.Add("Please select");
                        }

                        foreach (string packingListID in mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct())
                        {
                            // 避免重複加入
                            if (!matchData.PackingList_Candidate.Where(o => o == packingListID).Any())
                            {
                                matchData.PackingList_Candidate.Add(packingListID);
                            }

                            // 相同PackingListID且Ukey還未加入matchData.UpdateModels的，才需要存下來，否則會重複
                            var obj = mappingInfo[0].AsEnumerable().Where(o => MyUtility.Convert.GetString(o["PackingListID"]) == packingListID && !matchData.UpdateModels.Where(x => x.PackingListUkey == MyUtility.Convert.GetString(o["Ukey"])).Any()).FirstOrDefault();

                            UpdateModel u = new UpdateModel()
                            {
                                PackingListID = packingListID,
                                CustPONO = custPONo,
                                StyleID = styleID,
                                Article = article,
                                SizeCode = sizeCode,
                                ShipQty = shipQty,
                                SCICtnNo = obj["SCICtnNo"].ToString(),
                                RefNo = obj["RefNo"].ToString(),
                                CustCTN = zPL.CustCTN,
                                PackingListUkey = obj["Ukey"].ToString(),
                            };
                            matchData.UpdateModels.Add(u);
                        }

                        matchData.SelectedPackingID = matchData.PackingList_Candidate.FirstOrDefault();
                        matchData.NoStickerBasicSetting = false;
                        matchData.StickerAlreadyExisted = false;
                        matchData.CtnInClog = false;
                        matchData.Overwrite = false;
                        matchData.IsMixPack = isMixed;
                    }
                }
            }
            else
            {
                var keys = zPLs.Select(o => new
                {
                    CustPONO = o.CustPONo,
                    o.StyleID,
                    o.Article,
                    o.SizeCode,
                    o.ShipQty,
                }).Distinct().ToList();

                // 確認ZPL上的每一個資訊，都能找到對應的PackingList_Detail
                foreach (var key in keys)
                {
                    string custPONo = key.CustPONO;
                    string styleID = key.StyleID;
                    string article = key.Article;
                    string sizeCode = key.SizeCode;
                    string shipQty = key.ShipQty;

                    matchData.PO = custPONo;

                    // 相同Article SizeCode ShipQty可能不只有一箱，因此要把已經對應過的CustCTN記錄下來，不能重複
                    List<ZPL> sameZPL = zPLs.Where(o => o.CustPONo == custPONo && o.StyleID == styleID && o.Article == article && o.SizeCode == sizeCode && o.ShipQty == shipQty).ToList();

                    foreach (var zPL in sameZPL)
                    {
                        string sqlCmd = string.Empty;

                        // 判斷該"箱"是否混尺碼
                        // bool isCartonMix = this.Chk_Carton_Mix(currentCustPONo, currentStyleID, currentArticle, currentSizeCode, currentShipQty);
                        sqlCmd = this.Get_MatchSQL(zPL);

                        DBProxy.Current.Select(null, sqlCmd, out mappingInfo);

                        // Mapping到多少個PackingList_ID
                        int mapped_PackingLisID_Count = mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count();

                        // 準備Multipie Matches
                        matchData.MultipleMatches = mapped_PackingLisID_Count > 1 ? true : false;

                        // 準備P/L Candidate清單
                        if (mapped_PackingLisID_Count > 1)
                        {
                            matchData.PackingList_Candidate.Add("Please select");
                        }

                        foreach (string packingListID in mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct())
                        {
                            // 避免重複加入
                            if (!matchData.PackingList_Candidate.Where(o => o == packingListID).Any())
                            {
                                matchData.PackingList_Candidate.Add(packingListID);
                            }

                            // 相同PackingListID且Ukey還未加入matchData.UpdateModels的，才需要存下來，否則會重複
                            var obj = mappingInfo[0].AsEnumerable().Where(o => MyUtility.Convert.GetString(o["PackingListID"]) == packingListID && !matchData.UpdateModels.Where(x => x.PackingListUkey == MyUtility.Convert.GetString(o["Ukey"])).Any()).FirstOrDefault();

                            if (obj != null)
                            {
                                UpdateModel u = new UpdateModel()
                                {
                                    PackingListID = packingListID,
                                    CustPONO = custPONo,
                                    StyleID = styleID,
                                    Article = article,
                                    SizeCode = sizeCode,
                                    ShipQty = shipQty,
                                    SCICtnNo = obj["SCICtnNo"].ToString(),
                                    RefNo = obj["RefNo"].ToString(),
                                    CustCTN = zPL.CustCTN,
                                    PackingListUkey = obj["Ukey"].ToString(),
                                };
                                matchData.UpdateModels.Add(u);
                            }
                        }

                        matchData.SelectedPackingID = matchData.PackingList_Candidate.FirstOrDefault();
                        matchData.NoStickerBasicSetting = false;
                        matchData.StickerAlreadyExisted = false;
                        matchData.CtnInClog = false;
                        matchData.Overwrite = false;
                        matchData.IsMixPack = isMixed;
                    }
                }
            }

            // 若上傳檔案數與DB的筆數不相符則刪除
            foreach (string packindListID in matchData.PackingList_Candidate.ToList())
            {
                int fileCount = matchData.UpdateModels.Where(o => o.PackingListID == packindListID).Count();

                int dBCount = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($"selecT COUNT(1) from PackingList_Detail where id='{packindListID}'"));

                if (dBCount != fileCount)
                {
                    matchData.PackingList_Candidate.RemoveAll(o => o == packindListID);
                    matchData.UpdateModels.RemoveAll(o => o.PackingListID == packindListID);
                }
            }

            return matchData;
        }

        private Match PDF_Match(List<ZPL> zPLs, Match matchData, bool isMixed = false)
        {
            DataTable[] mappingInfo;
            List<ZPL> mixedzPLs = new List<ZPL>();

            matchData.PackingList_Candidate = new List<string>();
            matchData.UpdateModels = new List<UpdateModel>();

            // 若是混尺碼，則把ZPL物件拆開重新整理
            if (isMixed)
            {
                int ctnStartNo = 1;
                foreach (var zPL in zPLs)
                {
                    foreach (var size_Qty in zPL.Size_Qty_List)
                    {
                        string sizeCode = size_Qty.Size;
                        string shipQty = size_Qty.Qty.ToString();

                        ZPL m = new ZPL()
                        {
                            CustCTN = zPL.CustCTN,
                            CustPONo = zPL.CustPONo,
                            StyleID = zPL.StyleID,
                            Article = zPL.Article,
                            SizeCode = sizeCode,
                            ShipQty = shipQty,
                            FileName = zPL.FileName, // PDF才會用到
                            CTNStartNo = ctnStartNo.ToString(), // 目前沒用到，不過先編號寫好
                        };
                        mixedzPLs.Add(m);
                    }

                    ctnStartNo++;
                }

                zPLs = mixedzPLs;

                var keys = zPLs.Select(o => new
                {
                    CustPONO = o.CustPONo,
                    o.StyleID,
                    o.Article,
                    o.SizeCode,
                    o.ShipQty,
                    o.CTNStartNo,
                }).Distinct().ToList();

                foreach (var zPL in zPLs)
                {
                    string custPONo = zPL.CustPONo;
                    string styleID = zPL.StyleID;
                    string article = zPL.Article;
                    string sizeCode = zPL.SizeCode;
                    string shipQty = zPL.ShipQty;
                    string fileName = zPL.FileName;

                    matchData.PO = custPONo;

                    string sqlCmd = string.Empty;

                    // sqlCmd = this.Get_MappingSQL(zPL, isCartonMix);
                    sqlCmd = this.Get_MatchSQL(zPL, true);

                    DBProxy.Current.Select(null, sqlCmd, out mappingInfo);

                    // Mapping到多少個PackingList_ID
                    int mapped_PackingLisID_Count = mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count();

                    // 準備Multipie Matches
                    matchData.MultipleMatches = mapped_PackingLisID_Count > 1 ? true : false;

                    // 準備P/L Candidate清單
                    if (mapped_PackingLisID_Count > 1)
                    {
                        matchData.PackingList_Candidate.Add("Please select");
                    }

                    foreach (string packingListID in mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct())
                    {
                        // 避免重複加入
                        if (!matchData.PackingList_Candidate.Where(o => o == packingListID).Any())
                        {
                            matchData.PackingList_Candidate.Add(packingListID);
                        }

                        // 相同PackingListID且Ukey還未加入matchData.UpdateModels的，才需要存下來，否則會重複
                        var obj = mappingInfo[0].AsEnumerable().Where(o => MyUtility.Convert.GetString(o["PackingListID"]) == packingListID && !matchData.UpdateModels.Where(x => x.PackingListUkey == MyUtility.Convert.GetString(o["Ukey"])).Any()).FirstOrDefault();

                        UpdateModel u = new UpdateModel()
                        {
                            PackingListID = packingListID,
                            CustPONO = custPONo,
                            StyleID = styleID,
                            Article = article,
                            SizeCode = sizeCode,
                            ShipQty = shipQty,
                            SCICtnNo = obj["SCICtnNo"].ToString(),
                            RefNo = obj["RefNo"].ToString(),
                            CustCTN = zPL.CustCTN,
                            PackingListUkey = obj["Ukey"].ToString(),
                        };
                        matchData.UpdateModels.Add(u);
                    }
                }
            }
            else
            {
                var keys = zPLs.Select(o => new
                {
                    CustPONO = o.CustPONo,
                    o.StyleID,
                    o.Article,
                    o.SizeCode,
                    o.ShipQty,
                }).Distinct().ToList();

                foreach (var zPL in zPLs)
                {
                    string custPONo = zPL.CustPONo;
                    string styleID = zPL.StyleID;
                    string article = zPL.Article;
                    string sizeCode = zPL.SizeCode;
                    string shipQty = zPL.ShipQty;
                    string fileName = zPL.FileName;

                    matchData.PO = custPONo;

                    string sqlCmd = string.Empty;

                    // bool isCartonMix = this.Chk_Carton_Mix(currentCustPONo, currentStyleID, currentArticle, currentSizeCode, currentShipQty);

                    // sqlCmd = this.Get_MappingSQL(zPL, isCartonMix);
                    sqlCmd = this.Get_MatchSQL(zPL);

                    DBProxy.Current.Select(null, sqlCmd, out mappingInfo);

                    // Mapping到多少個PackingList_ID
                    int mapped_PackingLisID_Count = mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count();

                    // 準備Multipie Matches
                    matchData.MultipleMatches = mapped_PackingLisID_Count > 1 ? true : false;

                    // 準備P/L Candidate清單
                    if (mapped_PackingLisID_Count > 1)
                    {
                        matchData.PackingList_Candidate.Add("Please select");
                    }

                    foreach (string packingListID in mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct())
                    {
                        // 避免重複加入
                        if (!matchData.PackingList_Candidate.Where(o => o == packingListID).Any())
                        {
                            matchData.PackingList_Candidate.Add(packingListID);
                        }

                        // 相同PackingListID且Ukey還未加入matchData.UpdateModels的，才需要存下來，否則會重複
                        var obj = mappingInfo[0].AsEnumerable().Where(o => MyUtility.Convert.GetString(o["PackingListID"]) == packingListID && !matchData.UpdateModels.Where(x => x.PackingListUkey == MyUtility.Convert.GetString(o["Ukey"])).Any()).FirstOrDefault();

                        // 無論matchData.UpdateModels有沒有重複，一律存進去，若PackingList檔案數量與上傳的檔案數不一致，後面會清除
                        if (obj != null)
                        {
                            UpdateModel u = new UpdateModel()
                            {
                                PackingListID = packingListID,
                                CustPONO = custPONo,
                                StyleID = styleID,
                                Article = article,
                                SizeCode = sizeCode,
                                ShipQty = shipQty,
                                SCICtnNo = obj["SCICtnNo"].ToString(),
                                RefNo = obj["RefNo"].ToString(),
                                CustCTN = zPL.CustCTN,
                                PackingListUkey = obj["Ukey"].ToString(),
                            };
                            matchData.UpdateModels.Add(u);
                        }
                        else
                        {
                            // 會進到這邊，代表有重複的
                            UpdateModel u = new UpdateModel()
                            {
                                PackingListID = packingListID,
                                CustPONO = custPONo,
                                StyleID = styleID,
                                Article = article,
                                SizeCode = sizeCode,
                                ShipQty = shipQty,
                                SCICtnNo = mappingInfo[0].Rows[0]["SCICtnNo"].ToString(),
                                RefNo = mappingInfo[0].Rows[0]["RefNo"].ToString(),
                                CustCTN = zPL.CustCTN,
                                PackingListUkey = mappingInfo[0].Rows[0]["Ukey"].ToString(),
                            };
                            matchData.UpdateModels.Add(u);
                        }
                    }
                }
            }

            // 若上傳檔案數與DB的筆數不相符則刪除
            foreach (string packindListID in matchData.PackingList_Candidate.ToList())
            {
                int fileCount = matchData.UpdateModels.Where(o => o.PackingListID == packindListID).Count();

                int dBCount = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup($"selecT COUNT(1) from PackingList_Detail where id='{packindListID}'"));

                if (dBCount != fileCount)
                {
                    matchData.PackingList_Candidate.RemoveAll(o => o == packindListID);
                    matchData.UpdateModels.RemoveAll(o => o.PackingListID == packindListID);
                }
            }

            matchData.SelectedPackingID = matchData.PackingList_Candidate.FirstOrDefault();
            matchData.NoStickerBasicSetting = false;
            matchData.StickerAlreadyExisted = false;
            matchData.CtnInClog = false;
            matchData.Overwrite = false;
            matchData.IsMixPack = isMixed;

            return matchData;
        }

        private bool P24_Database(List<UpdateModel> updateModelList, string uploadType)
        {
            DualResult result;
            string updateCmd = string.Empty;
            string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from  System ");
            List<string> fileNames = new List<string>();
            int i = 0;
            List<string> p24_HeadList = new List<string>();
            List<string> p24_BodyList = new List<string>();

            List<DataTable> dtList = new List<DataTable>();

            #region 寫P24表頭

            var idList = updateModelList.Select(o => new { o.PackingListID, o.RefNo }).Distinct().ToList();

            string p24_Head_cmd = string.Empty;
            int ii = 0;
            foreach (var item in idList)
            {
                #region SQL
                p24_Head_cmd += $@"
--找出哪個箱子種類包含混尺碼
SELECT [PackingListID]=pd.ID ,pd.Ukey
INTO #MixCarton{ii}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
AND  p.ID = '{item.PackingListID}'
AND pd.RefNo = '{item.RefNo}'
AND (SELECT COUNT(qq.Ukey) FROM PackingList_Detail qq 
		where qq.ID = p.ID 
		AND qq.OrderID = pd.OrderID 
		AND qq.CTNStartNo = pd.CTNStartNo
		AND qq.Article = pd.Article 
		AND qq.SizeCode <> pd.SizeCode 
		and qq.Ukey != pd.Ukey) > 0

----
SELECT DISTINCT [StickerCombinationUkey]=s.ShippingMarkCombinationUkey,p.BrandID,pd.RefNo, [PackingListID]=p.ID
INTO #tmp_Combination{ii}
FROM ShippingMarkPicture s
INNER JOIN PackingList p ON p.BrandID = s.BrandID
INNER JOIN PackingList_Detail pd ON p.Id = pd.ID AND s.CTNRefno = pd.RefNo
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = s.ShippingMarkCombinationUkey
WHERE p.ID = '{item.PackingListID}'
AND pd.RefNo = '{item.RefNo}'
AND comb.IsMixPack=0
AND comb.Category='PIC'
AND NOT EXISTS (SELECT 1 FROM #MixCarton0 WHERE PackingListID=p.ID AND Ukey=pd.Ukey)
UNION
SELECT DISTINCT [StickerCombinationUkey]=s.ShippingMarkCombinationUkey,p.BrandID,pd.RefNo, [PackingListID]=p.ID
FROM ShippingMarkPicture s
INNER JOIN PackingList p ON p.BrandID = s.BrandID
INNER JOIN PackingList_Detail pd ON p.Id = pd.ID AND s.CTNRefno = pd.RefNo
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = s.ShippingMarkCombinationUkey
WHERE p.ID = '{item.PackingListID}'
AND pd.RefNo = '{item.RefNo}'
AND comb.IsMixPack=1
AND comb.Category='PIC'
AND EXISTS (SELECT 1 FROM #MixCarton0 WHERE PackingListID=p.ID AND Ukey=pd.Ukey)

----寫入ShippingMarkPic 表頭
SELECT t.PackingListID
    ,[AddName]='{Sci.Env.User.UserID}'
    ,[AddDate]=GETDATE()
INTO #tmp_Pic{ii}
FROM #tmp_Combination{ii} t

----刪除ShippingMarkPic表頭表身
DELETE picd
FROM ShippingMarkPic pic
INNER JOIN ShippingMarkPic_Detail picd ON pic.Ukey = picd.ShippingMarkPicUkey
INNER JOIN #tmp_Pic{ii} t ON pic.PackingListID = t.PackingListID 

DELETE p
FROM ShippingMarkPic p
INNER JOIN  #tmp_Pic{ii} t ON p.PackingListID = t.PackingListID 
;
INSERT INTO ShippingMarkPic (PackingListID ,AddName ,AddDate,EditName)
SELECT *,EditName='' FROM #tmp_Pic{ii}

DROP TABLE #tmp_Combination{ii} ,#tmp_Pic{ii}
";
                #endregion

                ii++;
            }

            p24_HeadList.Add(p24_Head_cmd);
            #endregion

            #region 寫P24表身
            DataTable bodyDt = new DataTable();
            bodyDt.ColumnsStringAdd("PackingListID");
            bodyDt.ColumnsStringAdd("SCICtnNo");
            bodyDt.ColumnsStringAdd("CustCTN");
            bodyDt.ColumnsStringAdd("RefNo");

            // 混尺碼的話會重複，因此要DISTINCT
            var keys = updateModelList.Select(o => new { o.PackingListID, o.SCICtnNo, o.CustCTN, o.RefNo }).Distinct().ToList();

            foreach (var model in keys)
            {
                DataRow dr = bodyDt.NewRow();

                dr["PackingListID"] = model.PackingListID;
                dr["SCICtnNo"] = model.SCICtnNo;
                dr["CustCTN"] = model.CustCTN;
                dr["RefNo"] = model.RefNo;

                bodyDt.Rows.Add(dr);
            }

            dtList.Add(bodyDt);
            #endregion

            // P24表身
            foreach (DataTable dt in dtList)
            {
                string tmpTable = string.Empty;

                // 不想用ProcessWithDatatable，把物件手動弄成temp table
                int count = 1;
                foreach (DataRow dr in dt.Rows)
                {
                    string tmp = $"SELECT [PackingListID]='{dr["PackingListID"]}',[SCICtnNO]='{dr["SCICtnNO"]}',[CustCTN]='{dr["CustCTN"]}' ,[Refno]='{dr["Refno"]}'";

                    tmpTable += tmp + Environment.NewLine;

                    if (count == 1)
                    {
                        tmpTable += $"INTO #tmp{i}" + Environment.NewLine;
                    }

                    if (dt.Rows.Count > count)
                    {
                        tmpTable += "UNION" + Environment.NewLine;
                    }

                    count++;
                }

                string cmd = string.Empty;

                cmd = $@"
{tmpTable}

----開始寫入ShippingMarkPic_Detail

----先整理出IsMix的箱子
SELECT [PackingListID]=pd.ID ,pd.SCICtnNo
INTO #MixCarton{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
AND EXISTS(
	SELECT 1 FROM #tmp{i} t WHERE t.PackingListID = pd.ID AND t.SCICtnNo = pd.SCICtnNo 
)
AND (SELECT COUNT(qq.Ukey) FROM PackingList_Detail qq 
		where qq.ID = p.ID 
		AND qq.OrderID = pd.OrderID 
		AND qq.CTNStartNo = pd.CTNStartNo
		AND qq.Article = pd.Article 
		AND qq.SizeCode <> pd.SizeCode 
		and qq.Ukey != pd.Ukey) > 0

----先找該PackingList的CustCD
SELECT DISTINCT [StickerCombinationUkey]=ISNULL(c.StickerCombinationUkey_MixPack 
,	(
	SELECT Ukey 
	FROM ShippingMarkCombination
	WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack = (IIF( EXISTS (SELECT 1 FROM #MixCarton{i} t WHERE t.PackingListID = pd.ID AND t.SCICtnNo = pd.SCICtnNo ) ,1 ,0))   
	)
),[IsMixPack] = (IIF(EXISTS (SELECT 1 FROM #MixCarton{i} t WHERE t.PackingListID = pd.ID AND t.SCICtnNo = pd.SCICtnNo ) ,1 ,0))   
,p.BrandID,pd.RefNo,[PackingListID]=p.ID ,pd.SCICtnNo
INTO #tmp_Combination_D{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN CustCD c ON p.BrandID = c.BrandID AND p.CustCDID = c.ID
WHERE  p.ID='{dt.Rows[0]["PackingListID"]}'


----找到CustCD對應的ShippingMarkCombination
SELECT DISTINCT t.PackingListID
	,[ShippingMarkCombinationUkey]=comb.Ukey
	,[ShippingMarkTypeUkey]=st.Ukey
	,t.RefNo
	,t.BrandID,t.IsMixPack ,t.SCICtnNo
INTO #tmp_Pic_Detail{i}
FROM #tmp_Combination_D{i} t
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = t.StickerCombinationUkey
INNER JOIN ShippingMarkCombination_Detail combD ON comb.Ukey = combD.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType st ON st.Ukey = combD.ShippingMarkTypeUkey
INNER JOIN ShippingMarkPicture pict ON pict.BrandID=t.BrandID AND pict.ShippingMarkCombinationUkey = comb.Ukey AND pict.CTNRefno=t.RefNo AND pict.Category='PIC'
INNER JOIN ShippingMarkPicture_Detail pictD ON pict.Ukey = pictD.ShippingMarkPictureUkey

----開始INSERT ShippingMarkPic_Detail

INSERT ShippingMarkPic_Detail 
		(ShippingMarkPicUkey,SCICtnNo,ShippingMarkCombinationUkey,ShippingMarkTypeUkey,FileName
		,Side,Seq,Is2Side,IsHorizontal,IsSSCC,FromRight,FromBottom,Width,Length)
SELECT 
	 [ShippingMarkPicUkey]=pic.Ukey
	,t.SCICtnNo
	,t.ShippingMarkCombinationUkey
	,t.ShippingMarkTypeUkey
	--,[FileName]=dt.CustCTN 
	,[FileName]=(SELECT TOP 1 CustCTN FROM #tmp0 dt WHERE t.PackingListID = dt.PackingListID AND t.RefNo = dt.RefNo AND t.SCICtnNo = dt.SCICtnNo)
	,b.Side
	,b.Seq
	,b.Is2Side
	,b.IsHorizontal
	,b.IsSSCC
	,b.FromRight
	,b.FromBottom
	,s.Width
	,s.Length
FROM #tmp_Pic_Detail{i} t
INNER JOIN ShippingMarkPicture a ON a.BrandID = t.BrandID AND a.CTNRefno = t.RefNo AND a.ShippingMarkCombinationUkey = t.ShippingMarkCombinationUkey AND a.Category ='PIC' 
INNER JOIN ShippingMarkPicture_Detail b ON a.Ukey = b.ShippingMarkPictureUkey AND b.ShippingMarkTypeUkey = t.ShippingMarkTypeUkey
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = a.ShippingMarkCombinationUkey
INNER JOIN StickerSize s ON s.ID = b.StickerSizeID
--INNER JOIN #tmp{i} dt ON t.PackingListID = dt.PackingListID AND t.RefNo = dt.RefNo AND t.SCICtnNo = dt.SCICtnNo
INNER JOIN ShippingMarkPic pic ON pic.PackingListID = t.PackingListID

";
                p24_BodyList.Add(cmd);
                i++;
            }

            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                try
                {
                    // 開始更新DB
                    foreach (var p24_Head in p24_HeadList)
                    {
                        if (!(result = DBProxy.Current.Execute(null, p24_Head.ToString())))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return false;
                        }
                    }

                    int idx = 0;
                    foreach (DataTable dt in dtList)
                    {
                        string cmd = p24_BodyList[idx];

                        if (!(result = DBProxy.Current.Execute(null, cmd)))
                        {
                            transactionscope.Dispose();
                            this.ShowErr(result);
                            return false;
                        }

                        idx++;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr(ex);
                    return false;
                }
            }

            // 轉出圖片檔，以及寫入Packing P24 的Image
            try
            {
                List<string> zplImg = new List<string>();
                List<string> zplContent = new List<string>();
                Dictionary<string, string> pdfImg = new Dictionary<string, string>();

                var key = updateModelList.Select(o => new { o.SCICtnNo, o.CustCTN }).Distinct().ToList();
                foreach (var item in key)
                {
                    string findZpl = this.wattingForConvert_contentsOfZPL.Where(o => o.Contains($"^FD>;>8{item.CustCTN}^FS")).FirstOrDefault();
                    string indImg = this.wattingForConvert.Where(o => o.Contains(item.CustCTN)).FirstOrDefault();

                    if (!MyUtility.Check.Empty(indImg))
                    {
                        zplImg.Add(indImg);
                    }

                    if (!MyUtility.Check.Empty(findZpl))
                    {
                        zplContent.Add(findZpl);
                    }

                    if (this.File_Name_PDF.Where(o => o.Value == item.CustCTN).Any())
                    {
                        pdfImg.Add(this.File_Name_PDF.Where(o => o.Value == item.CustCTN).FirstOrDefault().Key, item.CustCTN);
                    }
                }

                if (uploadType == "ZPL")
                {
                    foreach (string singleFileName in zplImg)
                    {
                        string contentString = zplContent.Where(o => o.Contains(singleFileName)).FirstOrDefault();
                        if (contentString != string.Empty)
                        {
                            this.CallAPI(singleFileName, contentString, shippingMarkPath, contentString.ToUpper().Contains("MIXED"));
                        }
                    }
                }

                if (uploadType == "PDF")
                {
                    foreach (var item in pdfImg)
                    {
                        string fileName = item.Key;
                        string custCTN = item.Value;

                        FileInfo file = new FileInfo(fileName);
                        this.ConvertPDF2Image(fileName, shippingMarkPath, custCTN, 1, 5, ImageFormat.Jpeg, Definition.One);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                return false;
            }

            return true;
        }

        private bool P03_Database(List<UpdateModel> updateModelList, string uploadType)
        {
            DualResult result;
            string updateCmd = string.Empty;
            string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from  System ");
            List<string> fileNames = new List<string>();
            int i = 0;

            string fileName = updateModelList.FirstOrDefault().FileName;
            fileNames.Add(fileName);

            foreach (var model in updateModelList)
            {
                string cmd = string.Empty;

                if (uploadType == "ZPL")
                {
                    #region SQL
                    cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmpOrders{i}
FROM Orders 
WHERE CustPONo='{model.CustPONO}' AND StyleID='{model.StyleID}'

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
	AND p.ID='{model.PackingListID}'
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders{i})
    AND Article = '{model.Article}'
    AND pd.ShipQty={model.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders{i}) AND SizeSpec IN ('{model.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{model.SizeCode}'
        )
	AND SCICtnNo = (	
		SELECT TOP 1 b.SCICtnNo 
		FROM ShippingMarkPic a WITH(NOLOCK)
		INNER JOIN ShippingMarkPic_Detail b WITH(NOLOCK) ON a.Ukey=b.ShippingMarkPicUkey
		WHERE a.PackingListID='{model.PackingListID}' AND b. FileName = '{model.CustCTN}'
	)
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN，PackingList.EditDate和EditName
UPDATE pd
SET pd.CustCTN='{model.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey

UPDATE PackingList
SET EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
WHERE ID ='{model.PackingListID}'

DROP TABLE #tmpOrders{i},#tmp{i}
";
                    #endregion
                }

                if (uploadType == "PDF")
                {
                    #region SQL
                    cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmpOrders{i}
FROM Orders 
WHERE CustPONo='{model.CustPONO}' AND StyleID='{model.StyleID}'

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
	AND p.ID='{model.PackingListID}'
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders{i})
    AND Article = '{model.Article}'
    AND pd.ShipQty={model.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders{i}) AND SizeSpec IN ('{model.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{model.SizeCode}'
        )
	AND SCICtnNo = (	
		SELECT TOP 1 b.SCICtnNo 
		FROM ShippingMarkPic a WITH(NOLOCK)
		INNER JOIN ShippingMarkPic_Detail b WITH(NOLOCK) ON a.Ukey=b.ShippingMarkPicUkey
		WHERE a.PackingListID='{model.PackingListID}' AND b. FileName = '{model.CustCTN}'
	)
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN，PackingList.EditDate和EditName
UPDATE pd
SET pd.CustCTN='{model.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey

UPDATE PackingList
SET EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
WHERE ID ='{model.PackingListID}'

DROP TABLE #tmpOrders{i},#tmp{i}
";
                    #endregion
                    fileNames.Add(model.FileName);
                }

                i++;

                updateCmd += cmd + Environment.NewLine + "---------";
            }

            if (MyUtility.Check.Empty(updateCmd))
            {
                return false;
            }

            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                try
                {
                    // Mapping資料寫入
                    if (!(result = DBProxy.Current.Execute(null, updateCmd.ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return false;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr(ex);
                }
            }

            return true;
        }

        private string Get_MatchSQL(ZPL currentZPL, bool isMixPack = false)
        {
            string sqlCmd = string.Empty;

            if (isMixPack)
            {
                sqlCmd = $@"
------開始 Match ZPL

SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{currentZPL.CustPONo}' AND StyleID='{currentZPL.StyleID}'

SELECT DISTINCT [PackingListID]=pd.ID 
    ,[CustPONO]='{currentZPL.CustPONo}'
    ,[StyleID] = '{currentZPL.StyleID}'
    ,pd.Article
    ,pd.SizeCode
    ,pd.ShipQty
    ,pd.SCICtnNo
    ,pd.RefNo
    ,[CustCTN] = '{currentZPL.CustCTN}'
    ,pd.Ukey
	,pd.CTNStartNo
INTO #tmp
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders)
    AND Article = '{currentZPL.Article}'
    AND pd.ShipQty={currentZPL.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{currentZPL.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{currentZPL.SizeCode}'
        )

/*
說明：由於混尺碼的時候，需要透過CTNStartNo才能得知總共有幾箱。
ZPL檔案上的Box 代表第1、2、3....N箱，但PackingList_Detail.CTNStartNo不一定會是從1開始。
舉例：若有10張ZPL，在ZPL的Box會寫1~10，但在PackingList_Detail.CTNStartNo是18~27。
原因：一張Packing底下「可能」不只這10箱，因此我們要先找出這10箱最小的CTNStartNo，再一箱一箱加上去找出唯一一筆PackingList_Detail

*/
SELECT  [PackingListID]
    ,[CustPONO]
    ,[StyleID] 
    ,Article
    ,SizeCode
    ,ShipQty
    ,RefNo
    ,[CustCTN]
	,[CTNStartNo]=CAST(MIN(CTNStartNo) as int) + {currentZPL.CTNStartNo} - 1
INTO #MinCTNStartNO
FROM #tmp 
GROUP BY  PackingListID
    ,CustPONO
    ,StyleID
    ,Article
    ,SizeCode
    ,ShipQty
    ,RefNo
    ,CustCTN

SELECT t.* 
FROM #tmp t
INNER JOIN #MinCTNStartNO c ON t.PackingListID = c.PackingListID AND t.CustPONO = c.CustPONO AND t.StyleID = c.StyleID AND t.Article = c.Article
AND t.SizeCode = c.SizeCode AND t.ShipQty = c.ShipQty AND t.RefNo = c.RefNo AND t.CustCTN = c.CustCTN 
AND t.CTNStartNo = c.CTNStartNo


DROP TABLE #tmpOrders,#tmp,#MinCTNStartNO

";
            }
            else
            {
                sqlCmd = $@"
------開始 Match ZPL

SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{currentZPL.CustPONo}' AND StyleID='{currentZPL.StyleID}'

SELECT DISTINCT [PackingListID]=pd.ID 
    ,[CustPONO]='{currentZPL.CustPONo}'
    ,[StyleID] = '{currentZPL.StyleID}'
    ,pd.Article
    ,pd.SizeCode
    ,pd.ShipQty
    ,pd.SCICtnNo
    ,pd.RefNo
    ,[CustCTN] = '{currentZPL.CustCTN}'
    ,pd.Ukey
	,pd.CTNStartNo
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders)
    AND Article = '{currentZPL.Article}'
    AND pd.ShipQty={currentZPL.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{currentZPL.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{currentZPL.SizeCode}'
        )

DROP TABLE #tmpOrders

";
            }

            return sqlCmd;
        }

        private void ShowConfirmMsg()
        {
            if (!this.ConfirmMsg.Any())
            {
                return;
            }

            DataTable dt = new DataTable();
            dt.ColumnsIntAdd("FileSeq");
            dt.ColumnsStringAdd("Result");

            foreach (Result r in this.ConfirmMsg)
            {
                DataRow dr = dt.NewRow();
                dr["FileSeq"] = r.FileSeq;
                dr["Result"] = r.ResultMsg;
                dt.Rows.Add(dr);
            }

            dt = dt.AsEnumerable().OrderBy(o => MyUtility.Convert.GetInt(o["FileSeq"])).CopyToDataTable();
            this.listControlBindingSource3.DataSource = dt;

            // var m = MyUtility.Msg.ShowMsgGrid(dt, "Please check below message.", "Result");
            var m = MyUtility.Msg.ShowMsgGrid(this.gridErrorMsg, "Please check below message.", "Result");
            m.Width = 850;
            m.grid1.Columns[1].Width = 600;
            m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            m.TopMost = true;
        }

        /// <summary>
        /// 取得Match Grid的資訊
        /// </summary>
        /// <param name="current">當下異動的DataRow</param>
        /// <returns>DataRow</returns>
        private DataRow GetInfoByPackingList(DataRow current)
        {
            string packingListID = MyUtility.Convert.GetString(current["PackingListCandidate"]);
            int fileSeq = MyUtility.Convert.GetInt(current["FileSeq"]);
            var now = this.MatchList.Where(o => o.FileSeq == fileSeq).FirstOrDefault();
            now.SelectedPackingID = packingListID;

            string isMixPack = MyUtility.Convert.GetBool(current["IsMixPack"]) ? "1" : "0";
            string cmd = string.Empty;

            // No Sticker Basic Setting
            cmd = $@"
SELECT [PicSetting]=(dbo.CheckShippingMarkSetting(
	 p.ID
	,pd.SCICtnNo 
	,pd.RefNo 
	,{isMixPack}
	,p.CustCDID 
	, p.BrandID 
))
INTO #tmp
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
WHERE p.ID='{packingListID}'

SELECT * FROM #tmp WHERE PicSetting = 0

DRop TABLE #tmp
";
            bool noBasicSetting = MyUtility.Check.Seek(cmd);
            now.NoStickerBasicSetting = noBasicSetting;

            // 勾選 : 代表貼標基本設定未完成
            current["NoStickerBasicSetting"] = noBasicSetting;

            cmd = $@"
SELECT 1
FROM ShippingMarkPic a
INNER JOIN ShippingMarkPic_Detail b ON a.Ukey = b.ShippingMarkPicUkey
WHERE a.PackingListID = '{packingListID}'
";
            bool stickerAlreadyExisted = MyUtility.Check.Seek(cmd);
            now.StickerAlreadyExisted = stickerAlreadyExisted;

            // 勾選 : 代表已存在P24資料
            current["StickerAlreadyExisted"] = stickerAlreadyExisted;

            cmd = $@"
SELECT 1
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
WHERE p.ID='{packingListID}' AND pd.ReceiveDate IS NOT NULL
";
            bool ctnInClog = MyUtility.Check.Seek(cmd);
            now.CtnInClog = ctnInClog;

            // 勾選 : 代表至少有一箱已送至成品倉
            current["CtnInClog"] = ctnInClog;

            current["Overwrite"] = MyUtility.Convert.GetBool(current["Overwrite"]);
            now.Overwrite = MyUtility.Convert.GetBool(current["Overwrite"]);

            return current;
        }

        private void ResetAll()
        {
            this.Grid_SelectedFile_Dt.Rows.Clear();
            this.Grid_Match_Dt.Rows.Clear();
            this.listControlBindingSource1.DataSource = null;
            this.File_Name_Object.Clear();
            this._File_Name_Object_List = new File_Name_Object_List();
            this._File_Name_Object_List.File_Name_Object2s = new List<FileName_Key_Model>();
            this.File_Name_PDF.Clear();
            this.wattingForConvert.Clear();
            this.GridBool = false;
            if (this.wattingForConvert_contentsOfZPL != null)
            {
                this.wattingForConvert_contentsOfZPL = new List<string>();
            }
        }

        /// <summary>
        /// Match Grid部分欄位變色
        /// </summary>
        private void MatchGridColor()
        {
            if (this.gridMatch.Rows == null)
            {
                return;
            }

            foreach (DataGridViewRow item in this.gridMatch.Rows)
            {
                if (MyUtility.Convert.GetString(item.Cells["PackingListCandidate"].Value).ToLower() == "please select" || MyUtility.Check.Empty(item.Cells["PackingListCandidate"].Value))
                {
                    item.Cells["MultipleMatches"].Style.BackColor = Color.Pink;
                }
                else
                {
                    item.Cells["MultipleMatches"].Style.BackColor = Color.White;
                }

                if (MyUtility.Convert.GetBool(item.Cells["NoStickerBasicSetting"].Value))
                {
                    item.Cells["NoStickerBasicSetting"].Style.BackColor = Color.Pink;
                }
                else
                {
                    item.Cells["NoStickerBasicSetting"].Style.BackColor = Color.White;
                }

                if (MyUtility.Convert.GetBool(item.Cells["CtnInClog"].Value))
                {
                    item.Cells["CtnInClog"].Style.BackColor = Color.Pink;
                }
                else
                {
                    item.Cells["CtnInClog"].Style.BackColor = Color.White;
                }
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                // 同一次上傳中不可選擇相同的 P/L
                if (this.MatchList.Select(o => o.SelectedPackingID).Distinct().Count() != this.MatchList.Select(o => o.SelectedPackingID).Count())
                {
                    MyUtility.Msg.WarningBox("Cannot choose duplicate P/L Candidate.");
                    return;
                }

                if (this.MatchList.Where(o => o.SelectedPackingID == string.Empty).Any())
                {
                    MyUtility.Msg.WarningBox("P/L Candidate can't be empty.");
                    return;
                }

                this.ShowWaitMessage("Processing...");

                #region 若以下狀況未排除直接按下 Confirm，系統只執行可完成上傳的項次，其餘無法完成的項次跳出視窗提示

                this.MatchList.RemoveAll(o => o.StickerAlreadyExisted && !o.Overwrite);

                this.ConfirmMsg.Clear();
                List<Match> errorList = this.CollectErrorMsg();

                // 錯誤的Match筆數 > 0 && 可以Match筆數 = 0，表示沒有任何一筆需要轉檔
                if (errorList.Select(o => o.FileSeq).Distinct().Count() > 0 && this.MatchList.Select(o => o.FileSeq).Distinct().Count() == 0)
                {
                    this.ShowConfirmMsg();
                    return;
                }
                #endregion

                // 挑出勾選Overwrite
                var isOverwrite = this.MatchList.Where(o => o.Overwrite == true);

                string cmd = string.Empty;
                foreach (Match match in isOverwrite)
                {
                    cmd += $"UPDATE PackingList_Detail SET CustCTN = '' WHERE ID = '{match.SelectedPackingID}' " + Environment.NewLine;
                }

                // 將 CustCtn already existed 列出的 PL 所有的 CustCTN 全數清空
                DBProxy.Current.Execute(null, cmd);

                List<int> failFileSeqs = new List<int>();

                // 若StickerAlreadyExisted = true，則Overwrite必須為true才需要抓出來轉檔
                var needUpdateList = this.MatchList.Where(o => (o.StickerAlreadyExisted && o.Overwrite) || !o.StickerAlreadyExisted).ToList();

                // 紀錄哪些File Seq需要顯示Result
                List<int> resultList = needUpdateList.Select(o => o.FileSeq).ToList();

                foreach (Match match in needUpdateList)
                {
                    if (errorList.Any(o => o.FileSeq == match.FileSeq))
                    {
                        continue;
                    }

                    bool p24 = true;
                    bool p03 = true;

                    match.UpdateModels.RemoveAll(o => o.PackingListID != match.SelectedPackingID);

                    p24 = this.P24_Database(match.UpdateModels, this.currentFileType.ToString());
                    p03 = this.P03_Database(match.UpdateModels, this.currentFileType.ToString());
                    if (!p24 || !p03)
                    {
                        failFileSeqs.Add(match.FileSeq);
                    }
                    else
                    {
                        bool stickerAlreadyExisted = this.MatchList.Where(o => o.FileSeq == match.FileSeq).FirstOrDefault().StickerAlreadyExisted;

                        if (stickerAlreadyExisted)
                        {
                            Result r = new Result()
                            {
                                FileSeq = match.FileSeq,
                                ResultMsg = "Overwrite success",
                            };
                            this.ConfirmMsg.Add(r);
                        }
                        else
                        {
                            Result r = new Result()
                            {
                                FileSeq = match.FileSeq,
                                ResultMsg = "Upload success",
                            };
                            this.ConfirmMsg.Add(r);
                        }
                    }

                    // 有實際執行SQL的PackingListID，才需要進行API資料交換
                    List<string> listPackingID = match.UpdateModels.Select(o => o.PackingListID).Distinct().ToList();

                    #region ISP20200757 資料交換 - Sunrise

                    if (listPackingID.Count > 0)
                    {
                        Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(listPackingID.Distinct().JoinToString(","), string.Empty))
                            .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                    }
                    #endregion

                    #region ISP20201607 資料交換 - Gensong

                    // 不透過Call API的方式，自己組合，傳送API
                    if (listPackingID.Count > 0)
                    {
                        foreach (var packing in listPackingID)
                        {
                            Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(packing, string.Empty))
                            .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                        }
                    }
                    #endregion
                }

                this.ShowConfirmMsg();

                // 將左邊Grid 的Result更新
                foreach (DataRow dr in this.Grid_SelectedFile_Dt.Rows)
                {
                    if (!resultList.Contains(MyUtility.Convert.GetInt(dr["FileSeq"])))
                    {
                        continue;
                    }

                    if (failFileSeqs.Where(o => o == MyUtility.Convert.GetInt(dr["FileSeq"])).Any())
                    {
                        dr["Result"] = "Fail";
                    }
                    else
                    {
                        int fileSeq = MyUtility.Convert.GetInt(dr["FileSeq"]);
                        bool stickerAlreadyExisted = this.MatchList.Where(o => o.FileSeq == fileSeq).FirstOrDefault().StickerAlreadyExisted;

                        if (stickerAlreadyExisted)
                        {
                            dr["Result"] = "Overwrite success";
                        }
                        else
                        {
                            dr["Result"] = "Upload success";
                        }
                    }

                    dr.EndEdit();
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
            finally
            {
                this.HideWaitMessage();
                this.btnProcessing.PerformClick();
            }

        }

        /// <summary>
        /// 收集錯誤訊息
        /// </summary>
        private List<Match> CollectErrorMsg()
        {
            List<Match> errorList = new List<Match>();

            // Multiple Matches = True 並且沒有選擇 P/L
            var a = this.MatchList.Where(o => o.MultipleMatches == true && (o.SelectedPackingID == string.Empty || o.SelectedPackingID.ToUpper() == "PLEASE SELECT"));
            if (a.Any())
            {
                foreach (var item in a)
                {
                    Result r = new Result()
                    {
                        FileSeq = item.FileSeq,
                        ResultMsg = "Multiple Matches and not choose the P/L.",
                    };
                    this.ConfirmMsg.Add(r);
                }

                // 記錄下無法執行上傳的項目
                errorList.AddRange(a);
                this.MatchList.RemoveAll(o => o.MultipleMatches == true && (o.SelectedPackingID == string.Empty || o.SelectedPackingID.ToUpper() == "PLEASE SELECT"));
            }

            // No Sticker Basic Setting = True
            var b = this.MatchList.Where(o => o.NoStickerBasicSetting == true);
            if (b.Any())
            {
                foreach (var item in b)
                {
                    Result r = new Result()
                    {
                        FileSeq = item.FileSeq,
                        ResultMsg = "Sticker basic setting not yet complete.",
                    };
                    this.ConfirmMsg.Add(r);
                }

                // 記錄下無法執行上傳的項目
                errorList.AddRange(b);
                this.MatchList.RemoveAll(o => o.NoStickerBasicSetting == true);
            }

            // Ctn in Clog = True
            var c = this.MatchList.Where(o => o.CtnInClog == true && ((o.StickerAlreadyExisted && o.Overwrite) || !o.StickerAlreadyExisted));
            if (c.Any())
            {
                foreach (var item in c)
                {
                    Result r = new Result()
                    {
                        FileSeq = item.FileSeq,
                        ResultMsg = "Carton already in Clog.",
                    };
                    this.ConfirmMsg.Add(r);
                }

                // 記錄下無法執行上傳的項目
                errorList.AddRange(c);
                this.MatchList.RemoveAll(o => o.CtnInClog == true && ((o.StickerAlreadyExisted && o.Overwrite) || !o.StickerAlreadyExisted));
            }

            // 檢查現存Cust CTN：CustCTN + CustPONO不能重複
            var d = this.MatchList;
            foreach (Match match in d)
            {
                bool isOtherPacking = false;
                string selectedPackingID = match.SelectedPackingID;

                if (this.currentFileType == UploadType.ZPL)
                {
                    foreach (var zPL in match.ZPLFile.ZPLs)
                    {
                        string custCTN = zPL.CustCTN;
                        string custPOno = zPL.CustPONo;

                        string cmd = $@"
SELECT 1 
FROM PackingList_Detail pd
INNER JOIN Orders o ON pd.OrderID = o.ID
WHERE pd.CustCTN='{custCTN}' AND pd.ID <> '{selectedPackingID}'  
AND o.CustPONo='{custPOno}'
";

                        // 檢查現有DB資料，CustCTN + CustPONO不能重複
                        if (MyUtility.Check.Seek(cmd))
                        {
                            isOtherPacking = true;
                        }
                    }
                }

                if (this.currentFileType == UploadType.PDF)
                {
                    foreach (var pDF in match.PDFFile.UpdateModels)
                    {
                        string custCTN = pDF.CustCTN;
                        string custPOno = pDF.CustPONo;

                        string cmd = $@"
SELECT 1 
FROM PackingList_Detail pd
INNER JOIN Orders o ON pd.OrderID = o.ID
WHERE pd.CustCTN='{custCTN}' AND pd.ID <> '{selectedPackingID}'  
AND o.CustPONo='{custPOno}'
";

                        // 檢查現有DB資料，CustCTN + CustPONO不能重複
                        if (MyUtility.Check.Seek(cmd))
                        {
                            isOtherPacking = true;
                        }
                    }
                }

                if (isOtherPacking)
                {
                    Result r = new Result()
                    {
                        FileSeq = match.FileSeq,
                        ResultMsg = "Exists Cust CTN.",
                    };
                    this.ConfirmMsg.Add(r);

                    // 記錄下無法執行上傳的項目
                    errorList.AddRange(this.MatchList.Where(o => o.FileSeq == match.FileSeq));
                }
            }

            // 確認這次上傳的檔案中是否有重複的CustCTN
            var e = this.MatchList.ToList();
            foreach (Match match in e)
            {
                List<string> custCTNs = match.UpdateModels.Select(o => o.CustCTN).Distinct().ToList();

                bool isDuplicateCustCTN = this.MatchList.Where(o => o.FileSeq != match.FileSeq).Where(o => o.UpdateModels.Any(x => custCTNs.Contains(x.CustCTN))).Any();

                if (isDuplicateCustCTN)
                {
                    Result r = new Result()
                    {
                        FileSeq = match.FileSeq,
                        ResultMsg = "Duplicate CustCTN.",
                    };
                    this.ConfirmMsg.Add(r);

                    // 記錄下無法執行上傳的項目
                    errorList.AddRange(this.MatchList.Where(o => o.FileSeq == match.FileSeq));
                }
            }

            this.MatchList.RemoveAll(o => errorList.Where(x => x.FileSeq == o.FileSeq).Any());

            return errorList;
        }

        #region 從ZPL取得資訊

        private string GetCustCDFromZPL(string zplContent)
        {
            string pono = this.GetPOnoFromZPL(zplContent);

            DataTable dt;
            string cmd = $@"
select distinct p.CustCDID
from PackingList p 
inner join PackingList_Detail pd  On p.ID=pd.ID
inner join Orders o On o.ID = pd.OrderID
where o.CustPONo='{pono}'
";

            DualResult r = DBProxy.Current.Select(null, cmd, out dt);

            if (!r)
            {
                this.ShowErr(r);
                return string.Empty;
            }

            if (dt.Rows.Count > 1)
            {
                return string.Empty;
            }

            return MyUtility.Convert.GetString(dt.Rows[0]["CustCDID"]);
        }

        private string GetPOnoFromZPL(string zplContent)
        {
            string[] tmp1 = zplContent.Split(new string[] { "^FDPO#:^FS", "^FDSKU:^FS" }, StringSplitOptions.RemoveEmptyEntries);
            string p = tmp1[1];
            string pono = p.Split(new string[] { "^FD", "^FS" }, StringSplitOptions.RemoveEmptyEntries)[1];

            return pono;
        }

        private string GetSKUFromZPL(string zplContent)
        {
            string[] tmp1 = zplContent.Split(new string[] { "^FDSKU:^FS", "^FDQTY:^FS" }, StringSplitOptions.RemoveEmptyEntries);
            string p = tmp1[1];
            string sku = p.Split(new string[] { "^FD", "^FS" }, StringSplitOptions.RemoveEmptyEntries)[1];

            string[] tmp2 = zplContent.Split(new string[] { "^FDBOX:^FS", "^FDBOX:^FS" }, StringSplitOptions.RemoveEmptyEntries);
            string s = tmp2[1];

            return sku;
        }

        private string GetCTNStartnoFromZPL(string zplContent)
        {
            string[] tmp2 = zplContent.Split(new string[] { "^FDBOX:^FS", "^FDBOX:^FS" }, StringSplitOptions.RemoveEmptyEntries);
            string s = tmp2[1];

            int sIdx = s.IndexOf("^FD") + 3; // ^FD共三個字元，因此+3
            int eIdx = s.IndexOf("^FS");

            string ctnStartno = string.Empty;
            for (int i = 0; i < (eIdx - sIdx); i++)
            {
                ctnStartno += s[i + sIdx].ToString();
            }

            return ctnStartno;
        }

        private string GetShipQtyFromZPL(string zplContent)
        {
            string[] tmp2 = zplContent.Split(new string[] { "^FDQTY:^FS", "^FDQTY:^FS" }, StringSplitOptions.RemoveEmptyEntries);
            string s = tmp2[1];

            int sIdx = s.IndexOf("^FD") + 3; // ^FD共三個字元，因此+3
            int eIdx = s.IndexOf("^FS");

            string shipQty = string.Empty;
            for (int i = 0; i < (eIdx - sIdx); i++)
            {
                shipQty += s[i + sIdx].ToString();
            }

            return shipQty;
        }

        private string GetCustCTNFromZPL(string zplContent)
        {
            string[] tmp2 = zplContent.Split(new string[] { "^FD>;>8", "^FS" }, StringSplitOptions.RemoveEmptyEntries);
            string custCTN = tmp2[1];
            return custCTN;
        }

        private List<SizeObject> GetMixSizeObjectFromZPL(string content)
        {
            List<SizeObject> sizeObjects = new List<SizeObject>();

            string[] strArray; // 取得CustCTN過程中，暫存用
            string article = string.Empty;
            string styleID = string.Empty;

            strArray = content.Split(new string[] { "^FDSKU:^FS", "^FS^FT" }, StringSplitOptions.RemoveEmptyEntries);

            int startIndex = Array.IndexOf(strArray, strArray.Where(o => o.ToString().Contains("Style-Color-Size")).FirstOrDefault());
            int endIndex = Array.IndexOf(strArray, strArray.Where(o => o.ToString().Contains("^FDTotal Qty")).FirstOrDefault());

            // 根據幾行，得知有多少種Size
            int sizeCount = (endIndex - startIndex - 1) / 4;
            List<string> tmpSizes = new List<string>();
            int ii = 0;

            for (int i = startIndex + 1; i <= endIndex; i++)
            {
                string tmpSize = string.Empty;
                if (i % 4 == 3)
                {
                    string[] stringSeparators = new string[] { "^FD" };
                    string sku = strArray[i].Split(stringSeparators, StringSplitOptions.None)[1];

                    styleID = sku.Split('-')[0];
                    article = sku.Split('-')[1];
                    string size = sku.Split('-')[2];
                    tmpSize = size;
                    tmpSizes.Add(tmpSize);
                }

                if (i % 4 == 1)
                {
                    string[] stringSeparators = new string[] { "^FD" };
                    string qty = strArray[i].Split(stringSeparators, StringSplitOptions.None)[1];
                    sizeObjects.Add(new SizeObject()
                    {
                        StyleID = styleID,
                        Article = article,
                        Size = tmpSizes[ii].Trim(),
                        Qty = Convert.ToInt32(qty.TrimStart().TrimEnd()),
                    });
                    ii++;
                }
            }

            return sizeObjects;
        }

        #endregion

        #region 類別定義

        /// <inheritdoc/>
        public class ZipHelper
        {
            /// <inheritdoc/>
            /// <summary>
            /// 將串入的ZPL檔案轉成Zip檔資料流存在記憶體
            /// </summary>
            /// <param name="data">data</param>
            public static byte[] ZipData(Dictionary<string, byte[]> data)
            {
                using (var zipStream = new MemoryStream())
                {
                    using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                    {
                        foreach (var fileName in data.Keys)
                        {
                            var entry = zipArchive.CreateEntry(fileName);
                            using (var entryStream = entry.Open())
                            {
                                var buff = data[fileName];
                                entryStream.Write(buff, 0, buff.Length);
                            }
                        }
                    }

                    return zipStream.ToArray();
                }
            }
        }

        /// <inheritdoc/>
        public class MixedCompare
        {
            /// <inheritdoc/>
            public string Text { get; set; }

            /// <inheritdoc/>
            public bool IsInt { get; set; }
        }

        /// <inheritdoc/>
        public class SizeObject
        {
            /// <inheritdoc/>
            public string Size { get; set; }

            /// <inheritdoc/>
            public int Qty { get; set; }

            /// <inheritdoc/>
            public string StyleID { get; set; }

            /// <inheritdoc/>
            public string Article { get; set; }
        }

        /// <inheritdoc/>
        public class ZPL
        {
            /// <inheritdoc/>
            public string CustPONo { get; set; }

            /// <inheritdoc/>
            public string StyleID { get; set; }

            /// <inheritdoc/>
            public string Article { get; set; }

            /// <inheritdoc/>
            public string SizeCode { get; set; }

            /// <inheritdoc/>
            public string ShipQty { get; set; }

            /// <inheritdoc/>
            public string CustCTN { get; set; }

            /// <inheritdoc/>
            public string CTNStartNo { get; set; }

            /// <inheritdoc/>
            public List<SizeObject> Size_Qty_List { get; set; } // 混尺碼用

            /// <inheritdoc/>
            public string FileName { get; set; }
        }

        /// <inheritdoc/>
        public class UpdateModel
        {
            /// <inheritdoc/>
            public string CustPONO { get; set; }

            /// <inheritdoc/>
            public string StyleID { get; set; }

            /// <inheritdoc/>
            public string Article { get; set; }

            /// <inheritdoc/>
            public string SizeCode { get; set; }

            /// <inheritdoc/>
            public string ShipQty { get; set; }

            /// <inheritdoc/>
            public string SCICtnNo { get; set; }

            /// <inheritdoc/>
            public string RefNo { get; set; }

            /// <inheritdoc/>
            public string CustCTN { get; set; }

            /// <inheritdoc/>
            public string PackingListID { get; set; }

            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <inheritdoc/>
            public string PackingListUkey { get; set; }
        }

        /// <summary>
        /// 由於PDF上傳的檔案是散裝的，這個物件用於裝同一包的PDF
        /// </summary>
        public class FileSeq_Key_Model
        {
            /// <inheritdoc/>
            public int FileSeq { get; set; }

            /// <inheritdoc/>
            public List<ZPL> UpdateModels { get; set; }
        }

        /// <inheritdoc/>
        public class File_Name_Object_List
        {
            /// <inheritdoc/>
            public List<FileName_Key_Model> File_Name_Object2s { get; set; }
        }

        public class FileName_Key_Model
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <inheritdoc/>
            public List<ZPL> ZPLs { get; set; }
        }

        public class Match
        {
            public string PO { get; set; }

            public int FileSeq { get; set; }

            public bool MultipleMatches { get; set; }

            // 重複的PackingListID
            public List<string> PackingList_Candidate { get; set; }

            public bool NoStickerBasicSetting { get; set; }

            public bool StickerAlreadyExisted { get; set; }

            public bool CtnInClog { get; set; }

            public bool Overwrite { get; set; }

            public bool IsMixPack { get; set; }

            public string SelectedPackingID { get; set; }

            // ZPL用
            public FileName_Key_Model ZPLFile { get; set; }

            // PDF用
            public FileSeq_Key_Model PDFFile { get; set; }

            // 用於更新DB
            public List<UpdateModel> UpdateModels { get; set; }
        }

        // PackingListCandidate的資料來源
        public class PackingListCandidate_Datasource
        {
            // PKey
            public int FileSeq { get; set; }

            public List<string> PackingList_Candidate { get; set; }
        }

        public class Result
        {
            public int FileSeq { get; set; }

            public string ResultMsg { get; set; }
        }
        #endregion

        #region 轉圖片相關

        /// <summary>
        /// 透過Web API，將ZPL轉成圖片並下載
        /// </summary>
        /// <param name="zplFileName">CustCTN</param>
        /// <param name="zplContentString">ZPL文字內容</param>
        /// <param name="shippingMarkPath">指定下載到哪裡</param>
        /// <param name="isMixed">是否混尺碼</param>
        private void CallAPI(string zplFileName, string zplContentString, string shippingMarkPath, bool isMixed)
        {
            // 一份ZPL有3張圖片，因此再拆一次
            string[] stringSeparators = new string[] { "^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH10,0^LRN" };
            List<string> content = zplContentString.Split(stringSeparators, StringSplitOptions.None).ToList();

            for (int i = 0; i < content.Count; i++)
            {
                try
                {
                    if (i == 1 && isMixed)
                    {
                        // stringSeparators = new string[] { "^XA^MMT^XZ^XA^PRE^FS^FT0314,0058^A0N,0036,0036^FR^FDCarton Contents^FS" };

                        // 說明：上面註解的是原本的拆解字串，不過^PR 這個標籤後面接的參數有可能不一樣(^PRE或^PRC)，且不像一些定義文字距離那些參數比較容易看得出差別，因此修改
                        // ^PR 標籤為列印速度參數，把寫死^PRE改掉，如下
                        string separators_s = "^XA^MMT^XZ^XA";
                        string separators_e = "^FS^FT0314,0058^A0N,0036,0036^FR^FDCarton Contents^FS";

                        for (int q = 1; q <= 26; q++)
                        {
                            string separators_mid = "^PR" + MyUtility.Excel.ConvertNumericToExcelColumn(q);

                            if (content[i].Contains(separators_s + separators_mid + separators_e))
                            {
                                stringSeparators = new string[] { separators_s + separators_mid + separators_e };
                            }
                        }

                        stringSeparators = new string[] { "FS^FT0314,0058^A0N,0036,0036^FR^FDCarton Contents^FS" };
                        string[] aa = content[i].Split(stringSeparators, StringSplitOptions.None);
                        content[i] = aa[0];
                        content.Add("^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH10,0^LRN" + aa[1]);
                    }
                    else
                    {
                        content[i] = "^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH10,0^LRN" + content[i];
                    }
                }
                catch (Exception ex)
                {
                    // this.ShowErr(ex);
                    throw ex;
                }

                bool isSSCC = content[i].Contains("^BCN");
                bool isContentLabel = !isMixed ? false : content[i].Contains("Style-Color-Size");

                // 不是SSCC 和 Content Label，就是Shipping Mark
                bool isShippingmark = !isSSCC & !isContentLabel;

                byte[] zpl = Encoding.UTF8.GetBytes(content[i]);

                // 使用API，相關說明：http://labelary.com/service.html
                var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");
                request.Method = "POST";

                // request.Accept = "application/pdf"; //如果要PDF，把這行解開
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = zpl.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(zpl, 0, zpl.Length);
                requestStream.Close();

                try
                {
                    // 透過API取得圖片
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseStream = response.GetResponseStream();

                    #region 寫入Image欄位

                    byte[] data = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        responseStream.CopyTo(ms);

                        data = ms.ToArray();
                    }

                    //// 寫入DB
                    this.InsertImageToDatabase(zplFileName, (i + 1).ToString(), data);
                    #endregion

                    // 存入[System].ShippingMarkPath 路徑下
                    // var fileStream = File.Create($@"{shippingMarkPath}\{zplFileName}_{(i + 1).ToString()}.bmp"); // 如果要PDF，把副檔名改成pdf

                    // responseStream.CopyTo(fileStream);
                    // responseStream.Close();
                    // fileStream.Close();
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 將PDF文件轉換為圖片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF檔案路徑</param>
        /// <param name="imageOutputPath">圖片輸出路徑</param>
        /// <param name="imageName">生成圖片的名字</param>
        /// <param name="startPageNum">從PDF文件的第幾頁開始轉換</param>
        /// <param name="endPageNum">從PDF文件的第幾頁開始停止轉換</param>
        /// <param name="imageFormat">設定所需圖片格式</param>
        /// <param name="definition">設定圖片的清晰度，數字越大越清晰</param>
        private void ConvertPDF2Image(string pdfInputPath, string imageOutputPath, string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(pdfInputPath);

            // 提高解析度
            Image bmp = doc.SaveAsImage(0, PdfImageType.Bitmap, 300, 300);

            // bmp.Save(imageOutputPath + imageName + "_tmpOri.bmp");
            byte[] data = null;

            //if (!System.IO.Directory.Exists(imageOutputPath))
            //{
            //    Directory.CreateDirectory(imageOutputPath);
            //}

            // int picWidth = 1300;
            // int picHeight = 1838;

            // Note : 工廠換了變出PDF的軟體，因此不需要裁切圖片了，直接把Source轉出
            Bitmap pic = new Bitmap(bmp);

            #region 裁切圖片

            // Bitmap pic = new Bitmap(picWidth, picHeight);

            // 建立圖片
            // Graphics graphic = Graphics.FromImage(pic);

            // int cutWidth = 95;
            // int cutHeight = 160;

            // 建立畫板
            // graphic.DrawImage(bmp,
            //         //將被切割的圖片畫在新圖片上面，第一個參數是被切割的原圖片
            //         new Rectangle(0, 0, picWidth, picHeight),
            //         //指定繪製影像的位置和大小，基本上是同pic大小
            //         new Rectangle(cutWidth, cutHeight, picWidth, picHeight),
            //         //指定被切割的圖片要繪製的部分
            //         GraphicsUnit.Pixel);
            #endregion

            // 準備要寫入DB的資料
            using (var stream = new MemoryStream())
            {
                pic.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                data = stream.ToArray();
            }

            // 將切割後的圖片存檔
            // pic.Save(imageOutputPath + imageName + "_Cut.bmp");
            doc.Dispose();
            bmp.Dispose();

            // graphic.Dispose();
            pic.Dispose();

            // 寫入DB
            this.InsertImageToDatabase(imageName, "1", data);
        }

        /// <summary>
        /// 寫入Image欄位
        /// </summary>
        /// <param name="fileName">圖片檔名</param>
        /// <param name="seq">單一個標籤的圖片檔順序</param>
        /// <param name="dataArry">圖片檔</param>
        private void InsertImageToDatabase(string fileName, string seq, byte[] dataArry)
        {
            // 第一張圖片，對應Combnation的最小Seq，第二張圖片對應第二小Seq，以此類推
            string seqCmd = $@"
SELECT Seq FROM (
	SELECT [Rank]=RANK() OVER (ORDER BY sd.Seq ),sd.*
	FROM ShippingMarkPic_Detail sd 
	INNER JOIN ShippingMarkPic s ON s.Ukey = sd.ShippingMarkPicUkey
	INNER JOIN ShippingMarkType st ON st.Ukey = sd.ShippingMarkTypeUkey
	INNER JOIN PackingList p ON p.ID = s.PackingListID
	where sd.FileName='{fileName}'
)a
WHERE Rank={seq}

";

            string cmd = $@"
----寫入圖片
UPDATE sd
SET sd.Image=@Image
FROM ShippingMarkPic_Detail sd 
INNER JOIN ShippingMarkPic s ON s.Ukey = sd.ShippingMarkPicUkey
INNER JOIN ShippingMarkType st ON st.Ukey = sd.ShippingMarkTypeUkey
WHERE 1=1 
AND sd.FileName=@FileName
AND sd.Seq = (
    {seqCmd}
)

UPDATE s
SET s.EditDate=GETDATE() , s.EditName='{Sci.Env.User.UserID}'
FROM ShippingMarkPic s WITH(NOLOCK)
INNER JOIN ShippingMarkPic_Detail sd WITH(NOLOCK) ON s.Ukey=sd.ShippingMarkPicUkey
INNER JOIN ShippingMarkType st ON st.Ukey = sd.ShippingMarkTypeUkey
WHERE 1=1 
AND sd.FileName=@FileName
AND sd.Seq = (
    {seqCmd}
)
";

            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter("@FileName", fileName));
            para.Add(new SqlParameter("@Image", dataArry));

            DualResult result = DBProxy.Current.Execute(null, cmd, para);

            if (!result)
            {
                this.ShowErr(result);
                throw result.GetException();
            }
        }

        private enum UploadType
        {
            ZPL,
            PDF,
        }

        private enum Definition
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
        }
        #endregion

        private bool GridBool = false;

        private void GridMatch_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnIndex == 7 && e.RowIndex == -1)
            {
                this.gridMatch.ValidateControl();
                DataTable dt = (DataTable)this.listControlBindingSource2.DataSource;
                if (dt != null || dt.Rows.Count > 0)
                {
                    this.GridBool = !this.GridBool;
                    foreach (DataRow item in dt.Rows)
                    {
                        if (MyUtility.Convert.GetBool(item["StickerAlreadyExisted"]))
                        {
                            item["OverWrite"] = this.GridBool;
                        }
                        else
                        {
                            item["OverWrite"] = false;
                        }

                        item.EndEdit();

                        this.MatchGridColor();
                        this.GetInfoByPackingList(item);
                    }

                    this.listControlBindingSource2.DataSource = dt;
                }
            }
        }
    }
}
