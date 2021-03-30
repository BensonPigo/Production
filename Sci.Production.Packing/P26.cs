using Bytescout.BarCodeReader;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        private List<Match> MatchList = new List<Match>();
        private List<Result> ConfirmMsg = new List<Result>();
        private List<BarcodeObj> BarcodeObjsPerFile = new List<BarcodeObj>();
        private List<BarcodeObj> BarcodeObjs = new List<BarcodeObj>();
        private string BarcodeReader_RegistrationName = MyUtility.Convert.GetString(ConfigurationManager.AppSettings["BarcodeReader_RegistrationName"]);
        private string BarcodeReader_RegistrationKey = MyUtility.Convert.GetString(ConfigurationManager.AppSettings["BarcodeReader_RegistrationKey"]);
        private bool GridBool = false;

        /// <summary>
        /// 目前處理的檔案格式
        /// </summary>
        private UploadType currentFileType;

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
            this.Grid_Match_Dt.ColumnsStringAdd("SelectedPackingID");
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
            this.backgroundWorker.RunWorkerAsync();
            this.gridSelectedFile.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.gridSelectedFile)
.Numeric("FileSeq", header: $"File{Environment.NewLine}Seq", width: Widths.AnsiChars(3), minimum: null)
.Text("FileName", header: $"File{Environment.NewLine}Name ", width: Widths.AnsiChars(10))
.Text("Result", header: "Result", width: Widths.AnsiChars(20))
;

            DataGridViewGeneratorCheckBoxColumnSettings col_Overwrite = new DataGridViewGeneratorCheckBoxColumnSettings();

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
.Numeric("FileSeq", header: "File" + Environment.NewLine + "Seq ", width: Widths.AnsiChars(5), iseditingreadonly: true)
.Text("SelectedPackingID", header: "P/L", width: Widths.AnsiChars(13), iseditingreadonly: false)
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
                string[] fullFileNames = openFileDialog1.FileNames;
                string[] safeFileNames = openFileDialog1.SafeFileNames;

                try
                {
                    int i = 0;
                    foreach (string safeFileName in safeFileNames)
                    {
                        // 取得CustCTN，作為檔名
                        List<string> zPL_FileName_List = new List<string>();
                        this.BarcodeObjsPerFile = new List<BarcodeObj>();

                        string oriZplConten; // 原始的ZPL檔內容
                        string tmpzplContent; // 將原始內容去除換行符號

                        string[] tmpArray; // 取得CustCTN過程中，暫存用
                        string[] contentsOfZPL; // 從原始ZPL檔拆出來的多個ZPL檔

                        string fullFileName = fullFileNames.Where(o => o.Contains(safeFileName)).FirstOrDefault();

                        try
                        {
                            #region ZPL

                            // 若上傳的ZPL檔，包含多張ZPL，先拆成個別ZPL
                            if (this.currentFileType == UploadType.ZPL)
                            {
                                using (StreamReader reader = new StreamReader(MyUtility.Convert.GetString(fullFileName), System.Text.Encoding.UTF8))
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

                                // 切分ZPL，Call API轉圖片，透過Barcode Reader讀取
                                foreach (var singleZPL in contentsOfZPL)
                                {
                                    if (MyUtility.Check.Empty(singleZPL))
                                    {
                                        continue;
                                    }

                                    // 一份ZP，非混尺碼2張，混尺碼有3張圖片，用stringSeparators會先切出2張，是混尺碼的第2張可以再細切出2張，
                                    string[] stringSeparators = new string[] { "^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH10,0^LRN" };
                                    List<string> content = singleZPL.Split(stringSeparators, StringSplitOptions.None).ToList();

                                    for (int idx = 0; idx < content.Count; idx++)
                                    {
                                        if (idx == 1)
                                        {
                                            string separators_s = "^XA^MMT^XZ^XA";
                                            string separators_e = "^FS^FT0314,0058^A0N,0036,0036^FR^FDCarton Contents^FS";

                                            for (int q = 1; q <= 26; q++)
                                            {
                                                string separators_mid = "^PR" + MyUtility.Excel.ConvertNumericToExcelColumn(q);

                                                if (content[idx].Contains(separators_s + separators_mid + separators_e))
                                                {
                                                    stringSeparators = new string[] { separators_s + separators_mid + separators_e };
                                                }
                                            }

                                            stringSeparators = new string[] { "FS^FT0314,0058^A0N,0036,0036^FR^FDCarton Contents^FS" };
                                            string[] secondAndThirdPic = content[idx].Split(stringSeparators, StringSplitOptions.None);

                                            // 切得出第三張表示是混尺碼，才進行後續動作
                                            if (secondAndThirdPic.Count() == 1)
                                            {
                                                continue;
                                            }

                                            content[idx] = secondAndThirdPic[0];
                                            content.Add("^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH10,0^LRN" + secondAndThirdPic[1]);
                                        }
                                        else
                                        {
                                            content[idx] = "^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH10,0^LRN" + content[idx];
                                        }
                                    }

                                    // 每一張ZPL Call API轉圖片
                                    foreach (var c in content)
                                    {
                                        byte[] zpl = Encoding.UTF8.GetBytes(c);

                                        var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");
                                        request.Method = "POST";

                                        // request.Accept = "application/pdf"; //如果要PDF，把這行解開
                                        request.ContentType = "application/x-www-form-urlencoded";
                                        request.ContentLength = zpl.Length;

                                        var requestStream = request.GetRequestStream();
                                        requestStream.Write(zpl, 0, zpl.Length);
                                        requestStream.Close();

                                        // 透過API取得圖片
                                        var response = (HttpWebResponse)request.GetResponse();
                                        var responseStream = response.GetResponseStream();

                                        byte[] zPLImage = null;
                                        using (MemoryStream ms = new MemoryStream())
                                        {
                                            responseStream.CopyTo(ms);

                                            zPLImage = ms.ToArray();
                                        }

                                        Image oImage = null;
                                        Bitmap oBitmap = null;

                                        // BarcodeReader
                                        Reader reader = new Reader(this.BarcodeReader_RegistrationName, this.BarcodeReader_RegistrationKey);
                                        reader.BarcodeTypesToFind.Code128 = true;

                                        // 轉成圖片
                                        MemoryStream oMemoryStream = new MemoryStream(zPLImage);

                                        // 設定資料流位置
                                        oMemoryStream.Position = 0;

                                        oImage = System.Drawing.Image.FromStream(oMemoryStream);

                                        // 建立副本
                                        oBitmap = new Bitmap(oImage);
                                        oBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

                                        bool hasBarcode = false;

                                        // 第一次讀取
                                        reader.ReadFrom(oBitmap);
                                        hasBarcode = this.BarcodeRead_ZPL(reader, zPLImage, safeFileName);

                                        // 旋轉90度
                                        oBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);

                                        // 第二次讀取
                                        reader.ReadFrom(oBitmap);
                                        hasBarcode = this.BarcodeRead_ZPL(reader, zPLImage, safeFileName);

                                        // 無法取得barcode，將上一張圖隸屬的 SSCC 與 Packing List ID 寫入這一張圖
                                        if (!hasBarcode)
                                        {
                                            if (!this.BarcodeObjsPerFile.Any())
                                            {
                                                continue;
                                            }

                                            var lastBarcode = this.BarcodeObjsPerFile.Last();
                                            this.BarcodeObjsPerFile.Add(new BarcodeObj()
                                            {
                                                Image = zPLImage,
                                                Barcode = lastBarcode.Barcode,
                                                PackingListID = lastBarcode.PackingListID,
                                                CTNStartNO = lastBarcode.CTNStartNO,
                                            });
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region PDF

                            // PDF一個檔案只有一張
                            if (this.currentFileType == UploadType.PDF)
                            {
                                // BarcodeReader
                                Reader reader = new Reader(this.BarcodeReader_RegistrationName, this.BarcodeReader_RegistrationKey);
                                reader.BarcodeTypesToFind.Code128 = true;

                                bool hasBarcode = false;
                                reader.ReadFrom(fullFileName);
                                hasBarcode = this.BarcodeRead_PDF(reader, safeFileName, fullFileName);
                            }
                            #endregion

                            // 判斷這個檔案，是否有任意 1 張圖沒有填入 SSCC 與 Packing List ID
                            if (this.BarcodeObjsPerFile.Where(o => MyUtility.Check.Empty(o.Barcode)).Any() || this.BarcodeObjsPerFile.Where(o => MyUtility.Check.Empty(o.PackingListID)).Any() || !this.BarcodeObjsPerFile.Any())
                            {
                                Exception ex = new Exception("Could not find SSCC.");
                                throw ex;
                            }

                            // 確認這個檔案所有對到的Packing List ID 是否一致
                            if (this.BarcodeObjsPerFile.Select(o => MyUtility.Convert.GetString(o.PackingListID)).Distinct().Count() > 1)
                            {
                                Exception ex = new Exception("ZPL mapping multiple Packing List.");
                                throw ex;
                            }

                            // 解析成功
                            DataRow newDr = this.Grid_SelectedFile_Dt.NewRow();
                            newDr["FileSeq"] = DBNull.Value;
                            newDr["FileName"] = safeFileName;
                            newDr["Result"] = string.Empty;
                            this.Grid_SelectedFile_Dt.Rows.Add(newDr);
                            i++;
                            this.BarcodeObjs.AddRange(this.BarcodeObjsPerFile);
                        }
                        catch (Exception q)
                        {
                            string msg = q.Message;
                            if (msg != "ZPL mapping multiple Packing List." && msg != "SSCC mapping multiple Packing List." && msg != "SSCC not updated in Packing List yet." && msg != "Could not find SSCC.")
                            {
                                // 解析失敗：Result寫入訊息
                                // msg = "Analysis failed.";
                            }

                            DataRow newDr = this.Grid_SelectedFile_Dt.NewRow();
                            newDr["FileSeq"] = DBNull.Value;
                            newDr["FileName"] = safeFileName;
                            newDr["Result"] = msg;
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

        private DataTable BarcodeInfoCheck(string barcode, string brandID)
        {
            DataTable dt;
            string cmd = $@"
SELECT DISTINCT pd.ID, pd.CTNStartNo , pd.SCICtnNo
FROM PackingList p
LEFT JOIN Pullout pu ON pu.ID = p.PulloutID
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
WHERE pd.CustCTN = '{barcode}'
AND p.BrandID='{brandID}'
AND (pu.Status IS NULL OR pu.Status NOT IN ('Confirmed', 'Locked'))
";

            DBProxy.Current.Select(null, cmd, out dt);
            return dt;
        }

        private bool BarcodeRead_ZPL(Reader reader, byte[] imageData, string fileName)
        {
            bool hasBarcode = false;
            List<BarcodeObj> currentZPL = new List<BarcodeObj>();

            // 逐一存下來
            foreach (FoundBarcode barcode in reader.FoundBarcodes)
            {
                string strBarcode = barcode.Value.Replace("<FNC1>", string.Empty);

                if (strBarcode.Length < 20)
                {
                    continue;
                }

                DataTable dt = this.BarcodeInfoCheck(strBarcode, "U.ARMOUR");

                if (dt.Rows.Count == 0)
                {
                    Exception ex = new Exception("SSCC not updated in Packing List yet.");
                    throw ex;
                }

                if (dt.Rows.Count > 1)
                {
                    Exception ex = new Exception("SSCC mapping multiple Packing List.");
                    throw ex;
                }

                // 同一張ZPL的重複Barcode只要存一個就好
                if (!currentZPL.Any(o => o.Barcode == strBarcode))
                {
                    hasBarcode = true;
                    currentZPL.Add(new BarcodeObj()
                    {
                        Image = imageData,
                        Barcode = strBarcode,
                        FileName = fileName,
                        PackingListID = MyUtility.Convert.GetString(dt.Rows[0]["ID"]),
                        CTNStartNO = MyUtility.Convert.GetString(dt.Rows[0]["CTNStartNo"]),
                        SCICtnNo = MyUtility.Convert.GetString(dt.Rows[0]["SCICtnNo"]),
                    });
                }
            }

            this.BarcodeObjsPerFile.AddRange(currentZPL);
            return hasBarcode;
        }

        private bool BarcodeRead_PDF(Reader reader, string fileName, string fullFileName)
        {
            bool hasBarcode = false;
            List<BarcodeObj> currentZPL = new List<BarcodeObj>();

            // 找不到 SSCC 直接 Fail
            if (reader.FoundBarcodes.Count() == 0)
            {
                Exception ex = new Exception("Could not find SSCC.");
                throw ex;
            }

            // 逐一存下來
            foreach (FoundBarcode barcode in reader.FoundBarcodes)
            {
                string strBarcode = barcode.Value.Replace("<FNC1>", string.Empty);

                if (strBarcode.Length < 20)
                {
                    continue;
                }

                DataTable dt = this.BarcodeInfoCheck(strBarcode, "NIKE");

                if (dt.Rows.Count == 0)
                {
                    Exception ex = new Exception("SSCC not updated in Packing List yet.");
                    throw ex;
                }

                if (dt.Rows.Count > 1)
                {
                    Exception ex = new Exception("SSCC mapping multiple Packing List.");
                    throw ex;
                }

                // 同一張PDF的重複Barcode只要存一個就好
                if (!currentZPL.Any(o => o.Barcode == strBarcode))
                {
                    hasBarcode = true;
                    currentZPL.Add(new BarcodeObj()
                    {
                        Barcode = strBarcode,
                        FileName = fileName,
                        FullFileName = fullFileName,
                        PackingListID = MyUtility.Convert.GetString(dt.Rows[0]["ID"]),
                        CTNStartNO = MyUtility.Convert.GetString(dt.Rows[0]["CTNStartNo"]),
                        SCICtnNo = MyUtility.Convert.GetString(dt.Rows[0]["SCICtnNo"]),
                    });
                }
            }

            this.BarcodeObjsPerFile.AddRange(currentZPL);
            return hasBarcode;
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
                List<Match> match_List = new List<Match>();
                string msg = string.Empty;
                this.listControlBindingSource2.DataSource = null;
                this.Grid_Match_Dt.Clear();
                this.MatchList.Clear();
                this.ConfirmMsg.Clear();
                this.GridBool = false;
                #endregion

                this.ShowWaitMessage("Data Match....");
                #region 開始Match

                var packingListIDs = this.BarcodeObjs.Select(o => o.PackingListID).Distinct().ToList();
                #region 相同 Packing 寫入相同的 File Seq

                int fileSeq = 1;
                foreach (var packingListID in packingListIDs)
                {
                    Match m = new Match()
                    {
                        SelectedPackingID = packingListID,
                        FileSeq = fileSeq,
                        UpdateModels = new List<UpdateModel>(),
                        NoStickerBasicSetting = false,
                        StickerAlreadyExisted = false,
                        CtnInClog = false,
                        Overwrite = false,
                        IsMixPack = false,
                    };

                    // 相同Packing的箱子資料
                    var samePack = this.BarcodeObjs.Where(o => o.PackingListID == packingListID);

                    // 相同 Packing List的檔案數量與 PMS 的資料箱數是否吻合
                    string fileCount = samePack.Select(o => o.CTNStartNO).Distinct().Count().ToString();
                    string fileCountInDB = MyUtility.GetValue.Lookup($"SELECT COUNT(DISTINCT CTNStartNo) FROM PackingList_Detail WHERE ID='{packingListID}'");

                    // 逐箱執行
                    foreach (var item in samePack)
                    {
                        // 一箱一個Barcode
                        string sscc = item.Barcode;

                        // 找出這箱在左邊檔案Grid的位置並加上FileSeq
                        foreach (var dataRow in this.Grid_SelectedFile_Dt.AsEnumerable().Where(o => MyUtility.Convert.GetString(o["FileName"]) == item.FileName))
                        {
                            dataRow["FileSeq"] = fileSeq;
                            item.FileSeq = fileSeq;

                            if (fileCount != fileCountInDB)
                            {
                                dataRow["Result"] = "Cannot mapping current P/L";
                                item.NeedMatch = false;
                                break;
                            }
                            else
                            {
                                item.NeedMatch = true;
                            }
                        }

                        if (!samePack.Any(o => o.NeedMatch))
                        {
                            continue;
                        }

                        // 找出這個Barcode的資訊，並存下Ukey（混尺碼一個Barcode會對應到多個Ukey，因此用Datatable)
                        string matchSQL = this.Get_MatchSQL(packingListID, sscc);
                        DBProxy.Current.Select(null, matchSQL, out DataTable mappingInfo);

                        mappingInfo.AsEnumerable().ToList().ForEach(dataRow =>
                        {
                            UpdateModel u = new UpdateModel()
                            {
                                PackingListID = packingListID,
                                SCICtnNo = dataRow["SCICtnNo"].ToString(),
                                RefNo = dataRow["RefNo"].ToString(),
                                CustCTN = sscc,
                                PackingListUkey = dataRow["Ukey"].ToString(),
                            };

                            if (MyUtility.Convert.GetBool(dataRow["IsMixed"]))
                            {
                                m.IsMixPack = true;
                            }

                            m.UpdateModels.Add(u);
                        });
                    }

                    if (m.UpdateModels.Count > 0)
                    {
                        match_List.Add(m);
                    }

                    fileSeq++;
                }
                #endregion
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
                    nRow["FileSeq"] = matchData.FileSeq;
                    nRow["SelectedPackingID"] = matchData.SelectedPackingID;
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

        private bool P24_Database(List<UpdateModel> updateModelList, string uploadType)
        {
            string updateCmd = string.Empty;
            string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from  System ");
            List<string> fileNames = new List<string>();
            int i = 0;
            List<string> p24_HeadList = new List<string>();
            List<string> p24_BodyList = new List<string>();

            List<DataTable> dtList = new List<DataTable>();
            DualResult result;

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
SELECT DISTINCT t.PackingListID
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
INTO #MixCTN{i}
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
SELECT DISTINCT [StickerCombinationUkey]= IIF(  (IIF(EXISTS (SELECT 1 FROM #MixCTN0 t WHERE t.PackingListID = pd.ID AND t.SCICtnNo = pd.SCICtnNo ) ,1 ,0)) = 0
,(ISNULL(c.StickerCombinationUkey,	
	(
	SELECT Ukey 
	FROM ShippingMarkCombination
	WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack = 0   
	)
))
,(ISNULL(c.StickerCombinationUkey_MixPack,	
	(
	SELECT Ukey 
	FROM ShippingMarkCombination
	WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack = 1
	)
))
)
,[IsMixPack] = (IIF(EXISTS (SELECT 1 FROM #MixCTN{i} t WHERE t.PackingListID = pd.ID AND t.SCICtnNo = pd.SCICtnNo ) ,1 ,0))   
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
		,Side,Seq,Is2Side,IsHorizontal,IsSSCC,FromRight,FromBottom,Width,Length, CtnHeight, IsOverCtnHt, NotAutomate)
SELECT 
	 [ShippingMarkPicUkey]=pic.Ukey
	,t.SCICtnNo
	,t.ShippingMarkCombinationUkey
	,t.ShippingMarkTypeUkey
	--,[FileName]=dt.CustCTN 
	,[FileName]=''  ----(SELECT TOP 1 CustCTN FROM #tmp0 dt WHERE t.PackingListID = dt.PackingListID AND t.RefNo = dt.RefNo AND t.SCICtnNo = dt.SCICtnNo)
	,b.Side
	,b.Seq
	,b.Is2Side
	,b.IsHorizontal
	,b.IsSSCC
	,b.FromRight
	,b.FromBottom
	,s.Width
	,s.Length
    ,a.CtnHeight
    ,b.IsOverCtnHt
    ,b.NotAutomate
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

            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
            {
                using (sqlConn)
                {
                    try
                    {
                        // 開始更新DB
                        foreach (var p24_Head in p24_HeadList)
                        {
                            if (!(result = DBProxy.Current.ExecuteByConn(sqlConn, p24_Head.ToString())))
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

                            if (!(result = DBProxy.Current.ExecuteByConn(sqlConn, cmd)))
                            {
                                transactionscope.Dispose();
                                this.ShowErr(result);
                                return false;
                            }

                            idx++;
                        }

                        DataTable dt_ShippingMarkPic_Detail;
                        string cc = $@"
select a.PackingListID
, b.SCICtnNo 
, b.ShippingMarkTypeUkey
, b.ShippingMarkPicUkey
, b.Seq
, [Rank]=RANK() OVER (PARTITION BY b.ShippingMarkPicUkey,b.SCICtnNo  ORDER BY b.Seq)
from ShippingMarkPic a with(nolock)
inner join ShippingMarkPic_Detail b with(nolock) on a.Ukey = b.ShippingMarkPicUkey
where a.PackingListID IN ('{idList.Select(o => o.PackingListID).JoinToString("','")}')
ORDER BY  a.PackingListID , b.SCICtnNo 
";

                        DBProxy.Current.Select(null, cc, out dt_ShippingMarkPic_Detail);

                        List<string> filenamee = new List<string>();
                        List<string> sQLs = new List<string>();
                        List<List<string>> sQLList = new List<List<string>>();
                        List<byte[]> images = new List<byte[]>();
                        int idxx = 0;

                        int rank = 0;
                        string currentSCICtnNo = string.Empty;
                        foreach (DataRow dr in dt_ShippingMarkPic_Detail.Rows)
                        {
                            string packID = MyUtility.Convert.GetString(dr["PackingListID"]);
                            string sCICtnNo = MyUtility.Convert.GetString(dr["SCICtnNo"]);

                            if (currentSCICtnNo == sCICtnNo)
                            {
                                rank++;
                            }
                            else
                            {
                                rank = 1;
                                currentSCICtnNo = sCICtnNo;
                            }

                            BarcodeObj barcodeObj = this.BarcodeObjs.Where(o => o.PackingListID == packID && o.SCICtnNo == sCICtnNo).FirstOrDefault();

                            string barcode = barcodeObj.Barcode;

                            // 如果Image是null，代表是PDF
                            if (barcodeObj.Image != null)
                            {

                            }
                            else
                            {
                                PdfDocument doc = new PdfDocument();
                                doc.LoadFromFile(barcodeObj.FullFileName);
                                Image bmp = doc.SaveAsImage(0, PdfImageType.Bitmap, 300, 300);

                                byte[] pDFImage = null;

                                // Note : 工廠換了變出PDF的軟體，因此不需要裁切圖片了，直接把Source轉出
                                Bitmap pic = new Bitmap(bmp);

                                // 準備要寫入DB的資料
                                using (var stream = new MemoryStream())
                                {
                                    pic.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                                    pDFImage = stream.ToArray();
                                }

                                // 將切割後的圖片存檔
                                doc.Dispose();
                                bmp.Dispose();
                                pic.Dispose();

                                string cmd = this.InsertImageToDatabase_List(idxx.ToString(), pDFImage, packID, sCICtnNo, rank.ToString());
                                filenamee.Add(barcode);
                                sQLs.Add(cmd);
                                images.Add(pDFImage);
                                idxx++;
                            }
                        }

                        int idxw = 0;
                        List<SqlParameter> para = new List<SqlParameter>();
                        string finalSQL = string.Empty;
                        int limit = 1000;
                        int limitCounter = 0;

                        List<DualResult> dualResults = new List<DualResult>();

                        foreach (var sql in sQLs)
                        {
                            finalSQL += sql + Environment.NewLine;

                            string name = filenamee[idxw];
                            byte[] image = images[idxw];

                            para.Add(new SqlParameter($"@FileName{idxw}", name));
                            para.Add(new SqlParameter($"@Image{idxw}", image));
                            idxw++;

                            limitCounter += 2;
                            if (limitCounter >= limit || limitCounter >= (sQLs.Count * 2))
                            {
                                DualResult test = Task.Run(() => this.InsertImage(finalSQL, para, sqlConn)).Result;
                                dualResults.Add(test);

                                finalSQL = string.Empty;
                                para = new List<SqlParameter>();
                                limitCounter = 0;
                            }
                        }

                        if (!dualResults.Any(o => o.Result == false))
                        {
                            transactionscope.Complete();
                        }

                        transactionscope.Dispose();
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        this.ShowErr(ex);
                        return false;
                    }
                }
            }

            return true;
        }

        private DualResult InsertImage(string sql, List<SqlParameter> para, SqlConnection sqlConn)
        {
            DualResult result = DBProxy.Current.ExecuteByConn(sqlConn, sql, para);

            return result;
        }

        private string Get_MatchSQL(string packingListID, string sSCC)
        {
            string sqlCmd = string.Empty;

            sqlCmd = $@"
SELECT DISTINCT [PackingListID]=pd.ID 
    ,[StyleID] = o.StyleID
    ,pd.Article
    ,pd.SizeCode
    ,pd.ShipQty
    ,pd.SCICtnNo
    ,pd.RefNo
    ,pd.CustCTN
    ,pd.Ukey
	,pd.CTNStartNo
	,[IsMixed] = Cast( IIF(isMixed.OrderID IS NOT NULL, 1 ,0) as bit)
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
OUTER APPLY(
	SELECT OrderID,CTNStartNo,[ArticleCTN] = COUNT(DISTINCT Article),[SizeCodeCTN]=COUNT(DISTINCT SizeCode)
	FROM  PackingList_Detail
	WHERE ID = p.ID
	GROUP BY OrderID,CTNStartNo
	HAVING COUNT(DISTINCT Article) > 1 OR COUNT(DISTINCT SizeCode) > 1
)isMixed
WHERE p.Type ='B'
AND p.ID = '{packingListID}'
AND pd.CustCTN = '{sSCC}'

";

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
            string packingListID = MyUtility.Convert.GetString(current["SelectedPackingID"]);
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
FROM ShippingMarkPic a WITH(NOLOCK)
INNER JOIN ShippingMarkPic_Detail b WITH(NOLOCK) ON a.Ukey = b.ShippingMarkPicUkey
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
            this.GridBool = false;
            this.BarcodeObjs = new List<BarcodeObj>();
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
                // 初始化
                this.imageIdx = 0;
                Result p24Result = new Result();

                // 同一次上傳中不可選擇相同的 P/L
                if (this.MatchList.Select(o => o.SelectedPackingID).Distinct().Count() != this.MatchList.Select(o => o.SelectedPackingID).Count())
                {
                    MyUtility.Msg.WarningBox("Cannot choose duplicate P/L.");
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

                //// 挑出勾選Overwrite

                string cmd = string.Empty;
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
                    bool p24_Html = true;

                    match.UpdateModels.RemoveAll(o => o.PackingListID != match.SelectedPackingID);

                    #region HTML事前檢查
                    DataRow[] selecteds = new DataRow[match.UpdateModels.Select(o => o.PackingListID).Distinct().Count()];

                    int qq = 0;
                    foreach (string p in match.UpdateModels.Select(o => o.PackingListID).Distinct())
                    {
                        DataTable d = new DataTable();
                        d.ColumnsStringAdd("ID");
                        DataRow t = d.NewRow();
                        t["ID"] = p;
                        selecteds[qq] = t;
                        qq++;
                    }

                    P24_Generate p24_Generate = new P24_Generate();
                    p24_Generate.Generate(selecteds, ref p24Result, true, "P26");
                    #endregion

                    // 沒有錯誤訊息表示檢查成功
                    if (!MyUtility.Check.Empty(p24Result.ResultMsg))
                    {
                        string stampMsg = MyUtility.Check.Empty(p24Result.ResultMsg) ? string.Empty : p24Result.ResultMsg;

                        Result r = new Result()
                        {
                            FileSeq = match.FileSeq,
                            ResultMsg = stampMsg,
                        };
                        this.ConfirmMsg.Add(r);

                        p24_Html = false;
                    }

                    // HTML檢查通過才進行P24
                    if (p24_Html)
                    {
                        p24 = this.P24_Database(match.UpdateModels, this.currentFileType.ToString());
                    }
                    else
                    {
                        p24 = false;
                    }

                    if (!p24)
                    {
                        failFileSeqs.Add(match.FileSeq);
                    }
                    else
                    {
                        cmd = $@"
UPDATE b
SET FileName = '' , FilePath = ''
from ShippingMarkPic a
inner join ShippingMarkPic_Detail b ON a.Ukey = b.ShippingMarkPicUkey
WHERE a.PackingListID IN ('{selecteds.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["ID"])).JoinToString("','")}')
";

                        #region Call P24 產生HTML檔

                        using (TransactionScope transactionscope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.MaxValue))
                        {
                            try
                            {
                                DualResult result;

                                result = DBProxy.Current.Execute(null, cmd);
                                if (!result)
                                {
                                    throw result.GetException();
                                }

                                p24_Generate.Generate(selecteds, ref p24Result, false, "P26");

                                // 由於API產生圖片耗時很久，因此前面通過的HTML驗證，過程中可能有變數，因此加上收集錯誤訊息，若有錯誤訓則把整個P24刪掉
                                if (!MyUtility.Check.Empty(p24Result.ResultMsg))
                                {
                                    string stampMsg = MyUtility.Check.Empty(p24Result.ResultMsg) ? string.Empty : p24Result.ResultMsg;

                                    Result r = new Result()
                                    {
                                        FileSeq = match.FileSeq,
                                        ResultMsg = stampMsg,
                                    };
                                    this.ConfirmMsg.Add(r);

                                    string disposeSQL = $@"
DELETE FROM ShippingMarkPic_Detail
WHERE ShippingMarkTypeUkey IN (
    SELECT a.Ukey 
    FROM ShippingMarkPic a 
    WHERE a.PackingListID IN ('{string.Join("','", selecteds.ToList().Select(o => o["ID"].ToString()))}') 
)

DELETE FROM ShippingMarkPic
WHERE PackingListID IN ('{string.Join("','", selecteds.ToList().Select(o => o["ID"].ToString()))}') 
";
                                    result = DBProxy.Current.Execute(null, disposeSQL);

                                    if (!result)
                                    {
                                        throw result.GetException();
                                    }
                                }

                                transactionscope.Complete();
                            }
                            catch (Exception ex)
                            {
                                transactionscope.Dispose();
                                this.ShowErr(ex);
                            }
                        }
                        #endregion

                        bool stickerAlreadyExisted = this.MatchList.Where(o => o.FileSeq == match.FileSeq).FirstOrDefault().StickerAlreadyExisted;

                        if (stickerAlreadyExisted)
                        {
                            Result r = new Result()
                            {
                                FileSeq = match.FileSeq,
                                ResultMsg = "Overwrite success. ",
                            };
                            this.ConfirmMsg.Add(r);
                        }
                        else
                        {
                            Result r = new Result()
                            {
                                FileSeq = match.FileSeq,
                                ResultMsg = "Upload success. ",
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
                            .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    #endregion

                    #region ISP20201607 資料交換 - Gensong

                    // 不透過Call API的方式，自己組合，傳送API
                    if (listPackingID.Count > 0)
                    {
                        foreach (var packing in listPackingID)
                        {
                            Task.Run(() => new Gensong_FinishingProcesses().SentPackingListToFinishingProcesses(packing, string.Empty))
                            .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
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

        /// <summary>
        /// 進度條
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (this.backgroundWorker == null || this.backgroundWorker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
            {
            }
        }

        #region 轉圖片相關 現在沒有在用，留著當作範例參考

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

        private int imageIdx = 0;
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
	    where sd.FileName='{fileName}' AND st.FromTemplate = 0
    )a
    WHERE Rank={seq}

";

            string cmd = $@"
----寫入圖片
UPDATE sd
SET sd.Image=@Image{this.imageIdx}
FROM ShippingMarkPic_Detail sd 
INNER JOIN ShippingMarkPic s ON s.Ukey = sd.ShippingMarkPicUkey
INNER JOIN ShippingMarkType st ON st.Ukey = sd.ShippingMarkTypeUkey
WHERE 1=1 
AND sd.FileName=@FileName{this.imageIdx}
AND sd.Seq = (
    {seqCmd}
)

---- 更新P24表頭
UPDATE s
SET s.EditDate=GETDATE() , s.EditName='{Sci.Env.User.UserID}'
FROM ShippingMarkPic s WITH(NOLOCK)
INNER JOIN ShippingMarkPic_Detail sd WITH(NOLOCK) ON s.Ukey=sd.ShippingMarkPicUkey
INNER JOIN ShippingMarkType st ON st.Ukey = sd.ShippingMarkTypeUkey
WHERE 1=1 
AND sd.FileName=@FileName{this.imageIdx}
AND sd.Seq = (
    {seqCmd}
)
";

            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter($"@FileName{this.imageIdx}", fileName));
            para.Add(new SqlParameter($"@Image{this.imageIdx}", dataArry));
            this.imageIdx++;

            DualResult result = DBProxy.Current.Execute(null, cmd, para);

            if (!result)
            {
                this.ShowErr(result);
                throw result.GetException();
            }
        }


        private string InsertImageToDatabase_List(string counter, byte[] dataArry,string packingListID, string sCICtnNo, string rank)
        {
            // 第一張圖片，對應Combnation的最小Seq，第二張圖片對應第二小Seq，以此類推

            string cmd = $@"
----找出P24表身的唯一值
select a.PackingListID
, b.SCICtnNo 
, b.ShippingMarkTypeUkey
, b.ShippingMarkPicUkey
, b.Seq
, [Rank]=RANK() OVER (PARTITION BY b.ShippingMarkPicUkey,b.SCICtnNo  ORDER BY b.Seq)
INTO #tmp{counter}
from ShippingMarkPic a with(nolock)
inner join ShippingMarkPic_Detail b with(nolock) on a.Ukey = b.ShippingMarkPicUkey
where a.PackingListID = '{packingListID}'  and b.SCICtnNo ='{sCICtnNo}'


----寫入圖片
UPDATE sd
SET sd.Image=@Image{counter}
FROM ShippingMarkPic_Detail sd 
INNER JOIN #tmp{counter} t on sd.ShippingMarkPicUkey = t.ShippingMarkPicUkey 
                    AND sd.SCICtnNo = t.SCICtnNo 
                    AND sd.ShippingMarkTypeUkey = t.ShippingMarkTypeUkey
                    AND sd.Seq = t.Seq
WHERE t.Rank = {rank}

---- 更新P24表頭
UPDATE s
SET s.EditDate=GETDATE() , s.EditName='{Sci.Env.User.UserID}'
FROM ShippingMarkPic s WITH(NOLOCK)
INNER JOIN ShippingMarkPic_Detail sd WITH(NOLOCK) ON s.Ukey=sd.ShippingMarkPicUkey
INNER JOIN #tmp{counter} t on sd.ShippingMarkPicUkey = t.ShippingMarkPicUkey 
                    AND sd.SCICtnNo = t.SCICtnNo 
                    AND sd.ShippingMarkTypeUkey = t.ShippingMarkTypeUkey
                    AND sd.Seq = t.Seq
WHERE t.Rank = {rank}
";
            return cmd;
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

        #region 類別定義

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

        /// <inheritdoc/>
        public class Match
        {
            /// <inheritdoc/>
            public int FileSeq { get; set; }

            /// <inheritdoc/>
            public bool NoStickerBasicSetting { get; set; }

            /// <inheritdoc/>
            public bool StickerAlreadyExisted { get; set; }

            /// <inheritdoc/>
            public bool CtnInClog { get; set; }

            /// <inheritdoc/>
            public bool Overwrite { get; set; }

            /// <inheritdoc/>
            public bool IsMixPack { get; set; }

            /// <inheritdoc/>
            public string SelectedPackingID { get; set; }

            // 用於更新DB

            /// <inheritdoc/>
            public List<UpdateModel> UpdateModels { get; set; }
        }

        /// <inheritdoc/>
        public class Result
        {
            /// <inheritdoc/>
            public int FileSeq { get; set; }

            /// <inheritdoc/>
            public string ResultMsg { get; set; }
        }

        /// <inheritdoc/>
        public class BarcodeObj
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <summary>
            /// 用於PDF後續轉圖片時抓取
            /// </summary>
            public string FullFileName { get; set; }

            /// <inheritdoc/>
            public string PackingListID { get; set; }

            /// <inheritdoc/>
            public string SCICtnNo { get; set; }

            /// <inheritdoc/>
            public string CTNStartNO { get; set; }

            /// <summary>
            /// ZPL 透過API 轉出的圖片
            /// </summary>
            public byte[] Image { get; set; }
            /// <summary>
            /// 透過Barcode Reader讀取圖片得到的Barcode
            /// </summary>
            public string Barcode { get; set; }

            /// <summary>
            /// FileSeq
            /// </summary>
            public int FileSeq { get; set; }

            /// <summary>
            /// 是否要進行Match程序
            /// </summary>
            public bool NeedMatch { get; set; }
        }
        #endregion

        private void GridMatch_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.gridMatch.ValidateControl();

            for (int i = 0; i <= this.gridMatch.Rows.Count - 1; i++)
            {
                DataRow dr = this.gridMatch.GetDataRow(i);

                if (!MyUtility.Convert.GetBool(dr["StickerAlreadyExisted"]))
                {
                    dr["Overwrite"] = false;
                }
                else
                {
                    dr["Overwrite"] = true;
                }

                dr.EndEdit();
                this.MatchGridColor();
                this.GetInfoByPackingList(dr);
            }
        }
    }
}
