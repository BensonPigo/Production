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
    public partial class P26 : Sci.Win.Tems.Base
    {
        private DataTable GridDt = new DataTable();
        private Dictionary<string, List<ZPL>> File_Name_Object = new Dictionary<string, List<ZPL>>();
        private File_Name_Object_List _File_Name_Object_List;
        private Dictionary<string, string> File_Name_PDF = new Dictionary<string, string>();
        private Dictionary<string, bool> packingList_IxMixed = new Dictionary<string, bool>();
        private List<string> wattingForConvert = new List<string>();
        private List<string> wattingForConvert_contentsOfZPL = new List<string>();
        private UploadType currentFileType;
        private DataTable NotSetB03_Table;
        private DataTable NotMapCustPo_Table;
        private DataTable existsCustCTN_Table;
        private DataTable FileCountError_Table;
        private List<string> mappingFailFileName = new List<string>();

        public bool canConvert = false;
        public string ErrorMessage = string.Empty;

        private List<List<UpdateModel>> UpdateModel_List = new List<List<UpdateModel>>();
        private List<NewFormModel> NewFormModels = new List<NewFormModel>();
        private List<NewFormModel> tmp_NewFormModels = new List<NewFormModel>();

        /// <inheritdoc/>
        public P26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "FileName", DataType = typeof(string) });
            this.GridDt.Columns.Add(new DataColumn() { ColumnName = "Result", DataType = typeof(string) });
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid1)
.Text("FileName", header: "File Name ", width: Widths.AnsiChars(35))
.Text("Result", header: "Result", width: Widths.AnsiChars(10))
;
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "ZPL files (*.zpl)|*.zpl|(*.pdf)|*.pdf";
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.ShowWaitMessage("Processing....");

                this.GridDt.Rows.Clear();
                this.listControlBindingSource1.DataSource = null;
                this.File_Name_Object.Clear();
                this._File_Name_Object_List = new File_Name_Object_List();
                this._File_Name_Object_List.File_Name_Object2s = new List<File_Name_Object2>();
                this.File_Name_PDF.Clear();
                this.wattingForConvert.Clear();

                if (this.wattingForConvert_contentsOfZPL != null)
                {
                    this.wattingForConvert_contentsOfZPL = new List<string>();
                }

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

                string[] files = openFileDialog1.FileNames;

                #region 路徑檢查

                string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from  System ");

                if (MyUtility.Check.Empty(shippingMarkPath))
                {
                    MyUtility.Msg.InfoBox("Please set <Shipping Mark Path> first.");
                    return;
                }

                if (!Directory.Exists(shippingMarkPath))
                {
                    MyUtility.Msg.InfoBox("Please Create Directory on Shipping Mark Path.");
                    return;
                }
                #endregion

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

                        // 用於顯示在表格的檔名
                        DataRow newDr = this.GridDt.NewRow();
                        newDr["FileName"] = openFileDialog1.SafeFileNames[i];
                        newDr["Result"] = string.Empty;
                        this.GridDt.Rows.Add(newDr);

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

                            // 3.透過API將ZPL檔轉成PDF，並存到指定路徑
                            this.wattingForConvert.AddRange(zPL_FileName_List);
                            this.wattingForConvert_contentsOfZPL.AddRange(contentsOfZPL.Where(o => o != string.Empty).ToList());

                            // 4.從單張ZPL內容中，拆解出需要的欄位資訊，用於Mapping方便
                            List<ZPL> zPL_Objects = this.Analysis_ZPL(fileName_with_Data, zPL_FileName_List);

                            this.File_Name_Object.Add(openFileDialog1.SafeFileNames[i], zPL_Objects);
                            this._File_Name_Object_List.File_Name_Object2s.Add(new File_Name_Object2()
                            {
                                FileName = openFileDialog1.SafeFileNames[i],
                                ZPLs = zPL_Objects,
                            });

                            i++;
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
                            this._File_Name_Object_List.File_Name_Object2s.Add(new File_Name_Object2()
                            {
                                FileName = openFileDialog1.SafeFileNames[i],
                                ZPLs = zPL_Objects,
                            });

                            this.File_Name_PDF.Add(fileInfo.FullName, zPL_Objects.FirstOrDefault().CustCTN);
                            i++;
                        }
                        #endregion

                    }

                    this.listControlBindingSource1.DataSource = this.GridDt;
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
                string custCD = this.GetCustCDFromZPL(content);

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

                    ZPL zz = new ZPL();
                    zz.CustPONo = custPONo;
                    zz.StyleID = styleID;
                    zz.Article = article;
                    zz.SizeCode = sizeCode;
                    zz.CTNStartNo = ctnStartno;
                    zz.ShipQty = shipQty;
                    zz.CustCTN = custCTN;
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

                /*
                // 根據ZPL的標題文字、座標，找到這張ZPL，屬於哪一種Type
                foreach (var type in this.ZplTypes)
                {
                    ZplTitlePosition titlePosition = type.Value;

                    bool po = content.ToUpper().Contains(titlePosition.PO);
                    bool sku = content.ToUpper().Contains(titlePosition.SKU);
                    bool qty = content.ToUpper().Contains(titlePosition.Qty);
                    bool box = content.ToUpper().Contains(titlePosition.BOX);

                    if (po && sku && qty && box)
                    {
                        zplType = type.Key;
                        break;
                    }
                }

                ZPL z = new ZPL();

                // 不同Type對應不同拆解方式
                switch (zplType)
                {
                    case "A":
                        z = this.SplitZLContent(content, custCTNno, isMixed,
                                custPONo_Spliter1: "^FDPO#:^FS^FT225,850^A0B,40,50^FD",
                                custPONo_Spliter2: "^FS^FT280,950^A0B,40,50^FDSKU:^FS",
                                detail_Spliter1: "^FS^FT280,950^A0B,40,50^FDSKU:^FS^FT280,850^A0B,40,50^FD",
                                detail_Spliter_Mixed: "^FS^FT",
                                detail_Spliter_NotMix: "^FS^FT400,950^A0B,40,50^FDQTY:",
                                detail_Spliter2: "^FS^FT400,950^A0B,40,50^FDQTY:^FS^FT400,850^A0B,75,100^FD",
                                detail_Spliter3: "^FS^FO425,700^BY3^B3B,N,75,N,N^FD",
                                detail_Spliter4: "^FDBOX:^FS^FT700,590^A0B,48,65^FD",
                                detail_Spliter5: "^FS^FO0,960^GB775,0,4^FS^FT115,995^A0N,34,47,^FD");
                        list.Add(z);
                        break;
                    case "B":
                        z = this.SplitZLContent(content, custCTNno, isMixed,
                                custPONo_Spliter1: "^FDPO#:^FS^FT325,790^A0B,75,80^FD",
                                custPONo_Spliter2: "^FS^FT460,950^A0B,75,80^FDSKU:^FS",
                                detail_Spliter1: "^FS^FT460,950^A0B,75,80^FDSKU:^FS^FT457,785^A0B,60,55^FD",
                                detail_Spliter_Mixed: "^FS^FT",
                                detail_Spliter_NotMix: "^FS^FT380,950^FT750,950^A0B,75,80^FDQTY:",
                                detail_Spliter2: "^FS^FT380,950^FT750,950^A0B,75,80^FDQTY:^FS^FT750,750^A0B,75,100^FD",
                                detail_Spliter3: "^FS^FT600,950^A0B,75,80^FD",
                                detail_Spliter4: "^FDBOX:^FS^FT600,750^A0B,75,100^FD",
                                detail_Spliter5: "^FS^FO0,960^GB775,0,4^FS^FT115,995^A0N,34,47,^FD");
                        list.Add(z);
                        break;
                    default:
                        break;
                }
                */
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

                this.UpdateModel_List.Clear();
                this.NewFormModels.Clear();
                this.mappingFailFileName.Clear();
                this.NotSetB03_Table = new DataTable();
                this.NotMapCustPo_Table = new DataTable();
                this.existsCustCTN_Table = new DataTable();
                this.FileCountError_Table = new DataTable();
                List<string> removePOs = new List<string>();
                List<string> removeFileNames = new List<string>();
                string msg = string.Empty;
                this.ErrorMessage = string.Empty;
                bool notMapping = false;
                this.canConvert = false;
                bool isUserSlect = false;

                this.NotSetB03_Table.ColumnsStringAdd("BrandID");
                this.NotSetB03_Table.ColumnsStringAdd("RefNo");
                this.NotMapCustPo_Table.ColumnsStringAdd("CustPO");
                this.existsCustCTN_Table.ColumnsStringAdd("CustCTN");
                this.FileCountError_Table.ColumnsStringAdd("PO#");
                this.FileCountError_Table.ColumnsStringAdd("SKU");
                this.FileCountError_Table.ColumnsStringAdd("Qty");
                #endregion

                this.ShowWaitMessage("Data Mapping....");
                #region 開始Mapping

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
                    List<PDF_Model> pDF_Models = new List<PDF_Model>();
                    int groupID = 1;
                    foreach (var key in keys)
                    {
                        PDF_Model m = new PDF_Model();
                        m.GroupID = groupID.ToString();
                        m.UpdateModels = zPLs.Where(o => o.CustPONo == key.CustPONo && o.StyleID == key.StyleID).ToList();

                        pDF_Models.Add(m);
                        groupID++;
                    }

                    foreach (var item in pDF_Models)
                    {
                        List<ZPL> ZPLs = item.UpdateModels;
                        this.tmp_NewFormModels = new List<NewFormModel>();
                        bool isMixed = ZPLs.Where(o => o.SizeCode == string.Empty || o.SizeCode.ToUpper() == "MIXED").Any();

                        List<UpdateModel> updateModels = this.PDF_Mapping(ZPLs, isMixed: isMixed);

                        if (this.tmp_NewFormModels.Count == 0)
                        {
                            this.UpdateModel_List.Add(updateModels);
                        }
                        else if (this.tmp_NewFormModels.Count != ZPLs.Count && (isMixed && this.tmp_NewFormModels.FirstOrDefault().ZPL_List.Select(o => o.CTNStartNo).Distinct().Count() != ZPLs.Count))
                        {
                            foreach (var zpl in ZPLs)
                            {
                                if (!this.ErrorMessage.Contains($"Carton count not mapping."))
                                {
                                    this.ErrorMessage += $" Carton count not mapping." + Environment.NewLine;
                                }

                                if (!this.mappingFailFileName.Contains(zpl.FileName))
                                {
                                    this.mappingFailFileName.Add(zpl.FileName);
                                }
                            }
                        }
                        else
                        {
                            // 每一張都mapping到兩個ID才需要開新視窗
                            if (ZPLs.Count == this.tmp_NewFormModels.Count || (isMixed && this.tmp_NewFormModels.FirstOrDefault().ZPL_List.Select(o => o.CTNStartNo).Distinct().Count() == ZPLs.Count))
                            {
                                this.NewFormModels.AddRange(this.tmp_NewFormModels);
                                isUserSlect = true;
                            }
                        }
                    }
                }

                if (this.currentFileType == UploadType.ZPL)
                {
                    this.NotSetB03_Table.ColumnsStringAdd("ShippingMarkCombination");
                    this.NotSetB03_Table.ColumnsStringAdd("ShippingMarkType");

                    foreach (var item in this._File_Name_Object_List.File_Name_Object2s)
                    {
                        // 根據上傳的ZPL展開
                        string fileName = item.FileName;
                        List<ZPL> zPLs = item.ZPLs;

                        // PackingList只要"有一箱"是混尺碼，則整張都是混尺碼
                        bool isMixed = zPLs.Where(o => o.SizeCode == string.Empty).Any();
                        this.tmp_NewFormModels = new List<NewFormModel>();

                        List<UpdateModel> updateModels = this.ZPL_Mapping(zPLs, fileName, isMixed: isMixed);

                        if (this.tmp_NewFormModels.Count == 0)
                        {
                            this.UpdateModel_List.Add(updateModels);
                        }
                        else if (this.tmp_NewFormModels.Count != zPLs.Count && (isMixed && this.tmp_NewFormModels.FirstOrDefault().ZPL_List.Select(o => o.CTNStartNo).Distinct().Count() != zPLs.Count))
                        {
                            if (!this.ErrorMessage.Contains($"File <{fileName}> Carton count not mapping."))
                            {
                                this.ErrorMessage += $"File <{fileName}> Carton count not mapping." + Environment.NewLine;
                            }

                            if (!this.mappingFailFileName.Contains(fileName))
                            {
                                this.mappingFailFileName.Add(fileName);
                            }
                        }
                        else
                        {
                            // 每一張都mapping到兩個ID才需要開新視窗
                            if (zPLs.Count == this.tmp_NewFormModels.Count || (isMixed && this.tmp_NewFormModels.FirstOrDefault().ZPL_List.Select(o => o.CTNStartNo).Distinct().Count() == zPLs.Count))
                            {
                                this.NewFormModels.AddRange(this.tmp_NewFormModels);
                                isUserSlect = true;
                            }
                        }
                    }
                }
                #endregion

                if (!MyUtility.Check.Empty(this.ErrorMessage))
                {
                    notMapping = true;
                }

                #region 如果有需要開啟新視窗的，則到下一個視窗，若無則直接修改DB

                this.HideWaitMessage();
                this.ShowWaitMessage("Processing....");

                if (notMapping || isUserSlect)
                {
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;

                    // 開啟新視窗 New Form
                    if (isUserSlect && this.NewFormModels.Count > 0)
                    {
                        Sci.Production.Packing.P26_AssignPackingList form = new P26_AssignPackingList(this.NewFormModels, (DataTable)this.listControlBindingSource1.DataSource, this.currentFileType.ToString(), this.wattingForConvert, this.File_Name_PDF, this.wattingForConvert_contentsOfZPL);
                        form.Width = 800;
                        form.ShowDialog();
                        this.canConvert = form.canConvert;
                        if (this.canConvert)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                dr["Result"] = "Pass";
                            }
                        }
                    }
                    else
                    {
                        this.canConvert = false;
                    }

                    List<DataRow> dl = dt.AsEnumerable().Where(o => this.mappingFailFileName.Contains(o["FileName"].ToString())).ToList();
                    foreach (DataRow dr in dl)
                    {
                        dr["Result"] = "Fail";
                    }

                    // 後續的資料還是要更新DB
                    if (this.UpdateModel_List.Count > 0)
                    {
                        bool reasult = false;

                        // 先完成P24資料寫入、轉圖片、將圖片存入P24表身
                        if (this.currentFileType == UploadType.ZPL)
                        {
                            reasult = this.P24_Database_Update(this.UpdateModel_List, "ZPL");
                        }
                        else
                        {
                            reasult = this.P24_Database_Update(this.UpdateModel_List, "PDF");
                        }

                        if (reasult)
                        {
                            // 修改PackingList_Detail
                            if (this.currentFileType == UploadType.ZPL)
                            {
                                this.P03_Database_Update(this.UpdateModel_List, "ZPL");
                            }
                            else
                            {
                                this.P03_Database_Update(this.UpdateModel_List, "PDF");
                            }
                        }
                    }
                }
                else
                {
                    bool reasult = false;

                    // 先完成P24資料寫入、轉圖片、將圖片存入P24表身
                    if (this.currentFileType == UploadType.ZPL)
                    {
                        reasult = this.P24_Database_Update(this.UpdateModel_List, "ZPL");
                    }
                    else
                    {
                        reasult = this.P24_Database_Update(this.UpdateModel_List, "PDF");
                    }

                    if (reasult)
                    {
                        // 修改PackingList_Detail
                        if (this.currentFileType == UploadType.ZPL)
                        {
                            this.P03_Database_Update(this.UpdateModel_List, "ZPL");
                        }
                        else
                        {
                            this.P03_Database_Update(this.UpdateModel_List, "PDF");
                        }
                    }
                }

                #region ISP20200757 資料交換 - Sunrise
                List<string> listPackingID = new List<string>();
                this.UpdateModel_List.ForEach(first => first.ForEach(second => listPackingID.Add(second.PackingListID)));
                if (listPackingID.Count > 0)
                {
                    Task.Run(() => new Sunrise_FinishingProcesses().SentPackingToFinishingProcesses(listPackingID.Distinct().JoinToString(","), string.Empty))
                        .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
                }
                #endregion

                this.HideWaitMessage();
                #endregion

                // 顯示錯誤訊息
                if (!MyUtility.Check.Empty(this.ErrorMessage))
                {
                    MyUtility.Msg.InfoBox(this.ErrorMessage);
                    this.ShowErrorMessage();
                }
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
        }

        public List<UpdateModel> ZPL_Mapping(List<ZPL> zPLs, string fileName, bool isMixed = false, string selected_PackingListID = "")
        {
            DataTable[] mappingInfo;
            List<UpdateModel> upateModel_List = new List<UpdateModel>();
            List<ZPL> mixedzPLs = new List<ZPL>();
            int totalFileCount = zPLs.Count;
            bool isBreak = false;

            // 事前檢查
            foreach (var zPL in zPLs)
            {
                string errorMsg = this.Check_Before_Mapping(zPL);
                if (errorMsg != string.Empty)
                {
                    isBreak = true;
                    if (!this.ErrorMessage.Contains(errorMsg))
                    {
                        this.ErrorMessage += errorMsg + Environment.NewLine;
                    }

                    if (!this.mappingFailFileName.Contains(fileName))
                    {
                        this.mappingFailFileName.Add(fileName);
                    }
                }
            }

            if (isBreak)
            {
                return upateModel_List;
            }

            // 若是混尺碼，則把ZPL物件拆開重新整理
            if (isMixed && MyUtility.Check.Empty(selected_PackingListID))
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
            }

            var keys = zPLs.Select(o => new
            {
                CustPONO = o.CustPONo,
                o.StyleID,
                o.Article,
                o.SizeCode,
                o.ShipQty,
            }).Distinct().ToList();

            #region 新視窗Mapping
            if (!MyUtility.Check.Empty(selected_PackingListID))
            {
                foreach (var key in keys)
                {
                    // SCICtnNo 與即將寫入的CustCTN必須對應到，因此加工處理
                    string custPONo = key.CustPONO;
                    string styleID = key.StyleID;
                    string article = key.Article;
                    string SizeCode = key.SizeCode;
                    string ShipQty = key.ShipQty;

                    // 相同Article SizeCode ShipQty可能不只有一箱，因此要把已經對應過的CustCTN記錄下來，不能重複
                    List<string> packingList_Detail_CustCTNs = new List<string>();

                    DataTable dt;

                    DBProxy.Current.Select(null, $@"
SELECT *
FROM PackingList_Detail
WHERE ID='{selected_PackingListID}'
AND Article='{article}'
AND SizeCode='{SizeCode}'
AND ShipQty={ShipQty} ", out dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        // 排除已經用過的 CustCTN
                        List<ZPL> sameZPL = zPLs.Where(o => o.Article == dr["Article"].ToString() && o.SizeCode == dr["SizeCode"].ToString() && o.ShipQty == dr["ShipQty"].ToString() && !packingList_Detail_CustCTNs.Contains(o.CustCTN)).ToList();

                        // 用過的紀錄下來
                        packingList_Detail_CustCTNs.Add(sameZPL.FirstOrDefault().CustCTN);

                        UpdateModel model = new UpdateModel()
                        {
                            PackingListID = dr["ID"].ToString(),
                            CustPONO = custPONo,
                            StyleID = styleID,
                            Article = dr["Article"].ToString(),
                            SizeCode = dr["SizeCode"].ToString(),
                            ShipQty = dr["ShipQty"].ToString(),
                            CustCTN = sameZPL.FirstOrDefault().CustCTN,
                            SCICtnNo = dr["SCICtnNo"].ToString(),
                            FileName = fileName,
                        };
                        upateModel_List.Add(model);
                    }
                }

                return upateModel_List;
            }
            #endregion

            bool packingB03DataError = false;

            foreach (var key in keys)
            {
                string custPONo = key.CustPONO;
                string styleID = key.StyleID;
                string article = key.Article;
                string SizeCode = key.SizeCode;
                string ShipQty = key.ShipQty;

                // 相同Article SizeCode ShipQty可能不只有一箱，因此要把已經對應過的CustCTN記錄下來，不能重複
                List<string> packingList_Detail_Ukeys = new List<string>();
                List<ZPL> sameZPL = zPLs.Where(o => o.CustPONo == custPONo && o.StyleID == styleID && o.Article == article && o.SizeCode == SizeCode && o.ShipQty == ShipQty).ToList();

                foreach (var zPL in sameZPL)
                {
                    string currentCustPONo = zPL.CustPONo;
                    string currentStyleID = zPL.StyleID;
                    string currentArticle = zPL.Article;
                    string currentSizeCode = zPL.SizeCode;
                    string currentShipQty = zPL.ShipQty;

                    string sqlCmd = string.Empty;

                    // 判斷該"箱"是否混尺碼
                    bool isCartonMix = this.Chk_Carton_Mix(currentCustPONo, currentStyleID, currentArticle, currentSizeCode, currentShipQty);

                    sqlCmd = this.Get_MappingSQL(zPL, isCartonMix);

                    DBProxy.Current.Select(null, sqlCmd, out mappingInfo);

                    // Mapping到多少個PackingList_ID
                    int mapped_PackingLisID_Count = mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count();

                    mappingInfo[0].AsEnumerable().Where(o => packingList_Detail_Ukeys.Contains(o["PackingList_Ukey"].ToString()));

                    // DB箱數(不分PackingListID)
                    int totalCount = mappingInfo[0].Rows.Count;

                    // 已經對應過的PackingList_Detail刪掉，不能重複
                    for (int i = 0; i <= totalCount - 1; i++)
                    {
                        if (packingList_Detail_Ukeys.Contains(mappingInfo[0].Rows[i]["PackingList_Ukey"].ToString()))
                        {
                            mappingInfo[0].Rows[i].Delete();
                        }
                    }

                    mappingInfo[0].AcceptChanges();

                    // ZPL張數
                    int fileCount = sameZPL.Count;

                    // B03缺少設定的數量
                    int brand_refno_NotMapping_Count = mappingInfo[2].Rows.Count;

                    // ShippingMarkPicture是否有建立好 相同 BrandID CTNRefno Side 不同Seq IsSCC的兩筆資料
                    bool packingB03_Lack = mappingInfo[1] == null ? true : (mappingInfo[1].Rows.Count == 2 ? false : true);

                    #region Packing B03未設定

                    // 有Mapping到PacingList_Detail才需要提示P03的檢查
                    if (mapped_PackingLisID_Count > 0 && brand_refno_NotMapping_Count > 0 && !this.ErrorMessage.Contains("The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings."))
                    {
                        this.ErrorMessage += "The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings." + Environment.NewLine;
                    }

                    // 準備新開的視窗
                    if (mapped_PackingLisID_Count > 0 && brand_refno_NotMapping_Count > 0 && !this.NotSetB03_Table.AsEnumerable().Where(o =>
                        o["BrandID"].ToString() == mappingInfo[2].Rows[0]["BrandID"].ToString()
                        && o["ShippingMarkCombination"].ToString() == mappingInfo[2].Rows[0]["ShippingMarkCombination"].ToString()
                        && o["ShippingMarkType"].ToString() == mappingInfo[2].Rows[0]["ShippingMarkType"].ToString()
                        && o["RefNO"].ToString() == mappingInfo[2].Rows[0]["RefNO"].ToString()).Any())
                    {
                        upateModel_List.RemoveAll(o => o.StyleID == currentStyleID && o.CustPONO == currentCustPONo);
                        foreach (DataRow dr in mappingInfo[2].Rows)
                        {
                            DataRow ndr = this.NotSetB03_Table.NewRow();
                            ndr["BrandID"] = dr["BrandID"].ToString();
                            ndr["ShippingMarkCombination"] = dr["ShippingMarkCombination"].ToString();
                            ndr["ShippingMarkType"] = dr["ShippingMarkType"].ToString();
                            ndr["RefNO"] = dr["RefNO"].ToString();
                            this.NotSetB03_Table.Rows.Add(ndr);
                        }

                        packingB03DataError = true;
                        break;
                    }

                    if (packingB03DataError)
                    {
                        if (!this.mappingFailFileName.Contains(fileName))
                        {
                            this.mappingFailFileName.Add(fileName);
                        }

                        break;
                    }

                    #endregion

                    // 取得PackingList ID 是否包含混尺碼 IsMixPack
                    // string packingID = mappingInfo[1].Rows[0]["PackingListID"].ToString();
                    // bool isMixPack = MyUtility.Convert.GetBool(mappingInfo[1].Rows[0]["IsMixPack"]);

                    // if (!this.packingList_IxMixed.Where(o => o.Key == packingID).Any())
                    // {
                    //    this.packingList_IxMixed.Add(packingID, isMixPack);
                    // }

                    if (mapped_PackingLisID_Count == 1)
                    {
                        if (totalCount == fileCount)
                        {
                            packingList_Detail_Ukeys.Add(mappingInfo[0].Rows[0]["PackingList_Ukey"].ToString());
                            UpdateModel model = new UpdateModel()
                            {
                                PackingListID = mappingInfo[0].Rows[0]["PackingListID"].ToString(),
                                CustPONO = zPL.CustPONo,
                                StyleID = zPL.StyleID,
                                Article = zPL.Article,
                                SizeCode = zPL.SizeCode,
                                ShipQty = zPL.ShipQty,
                                CustCTN = zPL.CustCTN,
                                FileName = fileName,
                                SCICtnNo = mappingInfo[0].Rows[0]["SCICtnNo"].ToString(),
                                RefNo = mappingInfo[0].Rows[0]["RefNo"].ToString(),
                            };
                            upateModel_List.Add(model);
                        }
                        else
                        {
                            // false
                            if (!this.ErrorMessage.Contains("Carton count not mapping."))
                            {
                                this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                            }

                            if (!this.FileCountError_Table.AsEnumerable().Where(o => o["PO#"].ToString() == zPL.CustPONo && o["SKU"].ToString() == (zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode) && o["Qty"].ToString() == zPL.ShipQty).Any())
                            {
                                DataRow ndr = this.FileCountError_Table.NewRow();
                                ndr["PO#"] = zPL.CustPONo;
                                ndr["SKU"] = zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode;
                                ndr["Qty"] = zPL.ShipQty;

                                this.FileCountError_Table.Rows.Add(ndr);
                            }

                            if (!this.mappingFailFileName.Contains(fileName))
                            {
                                this.mappingFailFileName.Add(fileName);
                            }
                        }
                    }
                    else if (mapped_PackingLisID_Count > 1)
                    {
                        if (totalCount == fileCount)
                        {
                            // 02 false
                            if (!this.ErrorMessage.Contains("Carton count not mapping."))
                            {
                                this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                            }

                            if (!this.FileCountError_Table.AsEnumerable().Where(o => o["PO#"].ToString() == zPL.CustPONo && o["SKU"].ToString() == (zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode) && o["Qty"].ToString() == zPL.ShipQty).Any())
                            {
                                DataRow ndr = this.FileCountError_Table.NewRow();
                                ndr["PO#"] = zPL.CustPONo;
                                ndr["SKU"] = zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode;
                                ndr["Qty"] = zPL.ShipQty;

                                this.FileCountError_Table.Rows.Add(ndr);
                            }

                            if (!this.mappingFailFileName.Contains(fileName))
                            {
                                this.mappingFailFileName.Add(fileName);
                            }
                        }
                        else
                        {
                            // 判斷是03 04 05哪一種
                            int sameCount = 0;
                            string trueID = string.Empty;
                            List<string> iDList = new List<string>();
                            foreach (string packingListID in mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"].ToString()).Distinct().ToList())
                            {
                                int ctn = mappingInfo[0].AsEnumerable().Where(o => o["PackingListID"].ToString() == packingListID).Count();

                                // 必須檢查總箱數都一致
                                int totalCount_DB = Convert.ToInt32(MyUtility.GetValue.Lookup($@"
SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{currentCustPONo}' AND StyleID='{currentStyleID}'

SELECT COUNT(pd.Ukey)
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders)
    AND pd.CustCTN = ''
	AND p.ID='{packingListID}'
"));
                                if (totalFileCount == totalCount_DB)
                                {
                                    sameCount++;
                                    trueID = packingListID;
                                    iDList.Add(packingListID);
                                }
                            }

                            if (sameCount > 1)
                            {
                                // 03
                                NewFormModel model = new NewFormModel()
                                {
                                    PackingListIDs = iDList,
                                    FileName = fileName,
                                    ZPL_Content = mappingInfo[0], // 用於新視窗呈現詳細資料用
                                    ZPL_List = zPLs, // 用於新視窗Call這邊
                                };
                                this.tmp_NewFormModels.Add(model);
                            }
                            else if (sameCount == 1)
                            {
                                DataRow okRow = mappingInfo[0].AsEnumerable().Where(o =>
                                o["PackingListID"].ToString() == iDList.FirstOrDefault()
                                && !packingList_Detail_Ukeys.Contains(o["PackingList_Ukey"].ToString())).FirstOrDefault();

                                packingList_Detail_Ukeys.Add(okRow["PackingList_Ukey"].ToString());

                                // 04
                                UpdateModel model = new UpdateModel()
                                {
                                    PackingListID = trueID,
                                    CustPONO = zPL.CustPONo,
                                    StyleID = zPL.StyleID,
                                    Article = zPL.Article,
                                    SizeCode = zPL.SizeCode,
                                    ShipQty = zPL.ShipQty,
                                    CustCTN = zPL.CustCTN,
                                    FileName = fileName,
                                    SCICtnNo = okRow["SCICtnNo"].ToString(),
                                    RefNo = okRow["RefNo"].ToString(),
                                };
                                upateModel_List.Add(model);
                            }
                            else
                            {
                                // 05 false
                                if (!this.ErrorMessage.Contains("Carton count not mapping."))
                                {
                                    this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                                }

                                if (!this.FileCountError_Table.AsEnumerable().Where(o => o["PO#"].ToString() == zPL.CustPONo && o["SKU"].ToString() == (zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode) && o["Qty"].ToString() == zPL.ShipQty).Any())
                                {
                                    DataRow ndr = this.FileCountError_Table.NewRow();
                                    ndr["PO#"] = zPL.CustPONo;
                                    ndr["SKU"] = zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode;
                                    ndr["Qty"] = zPL.ShipQty;

                                    this.FileCountError_Table.Rows.Add(ndr);
                                }

                                if (!this.mappingFailFileName.Contains(fileName))
                                {
                                    this.mappingFailFileName.Add(fileName);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!this.ErrorMessage.Contains("Carton count not mapping."))
                        {
                            this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                        }
                    }
                }
            }

            return upateModel_List;
        }

        public List<UpdateModel> PDF_Mapping(List<ZPL> zPLs, bool isMixed = false, string selected_PackingListID = "")
        {
            DataTable[] mappingInfo;
            List<UpdateModel> upateModel_List = new List<UpdateModel>();
            List<ZPL> mixedzPLs = new List<ZPL>();
            List<string> packingList_Detail_Ukeys = new List<string>();

            // 若是混尺碼，則把ZPL物件拆開重新整理
            if (isMixed && MyUtility.Check.Empty(selected_PackingListID))
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
            }

            int totalFileCount = zPLs.Count;

            var keys = zPLs.Select(o => new
            {
                CustPONO = o.CustPONo,
                o.StyleID,
                o.Article,
                o.SizeCode,
                o.ShipQty,
            }).Distinct().ToList();

            #region 新視窗Mapping
            if (!MyUtility.Check.Empty(selected_PackingListID))
            {
                foreach (var key in keys)
                {
                    string custPONo = key.CustPONO;
                    string styleID = key.StyleID;
                    string article = key.Article;
                    string SizeCode = key.SizeCode;
                    string ShipQty = key.ShipQty;
                    List<string> packingList_Detail_CustCTNs = new List<string>();
                    DataTable dt;

                    List<ZPL> sameZPL = zPLs.Where(o => o.CustPONo == custPONo && o.StyleID == styleID && o.Article == article && o.SizeCode == SizeCode && o.ShipQty == ShipQty).ToList();

                    DBProxy.Current.Select(null, $@"
SELECT *
FROM PackingList_Detail
WHERE ID='{selected_PackingListID}'
AND Article='{article}'
AND SizeCode='{SizeCode}'
AND ShipQty={ShipQty} ", out dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        List<ZPL> sameZPL2 = zPLs.Where(o => o.Article == dr["Article"].ToString() && o.SizeCode == dr["SizeCode"].ToString() && o.ShipQty == dr["ShipQty"].ToString() && !packingList_Detail_CustCTNs.Contains(o.CustCTN)).ToList();
                        packingList_Detail_CustCTNs.Add(sameZPL2.FirstOrDefault().CustCTN);
                        UpdateModel model = new UpdateModel()
                        {
                            PackingListID = dr["ID"].ToString(),
                            CustPONO = custPONo,
                            StyleID = styleID,
                            Article = dr["Article"].ToString(),
                            SizeCode = dr["SizeCode"].ToString(),
                            ShipQty = dr["ShipQty"].ToString(),
                            CustCTN = sameZPL2.FirstOrDefault().CustCTN,
                            SCICtnNo = dr["SCICtnNo"].ToString(),
                            FileName = sameZPL2.FirstOrDefault().FileName,
                        };
                        upateModel_List.Add(model);
                    }
                }

                return upateModel_List;
            }
            #endregion

            bool packingB03DataError = false;

            foreach (var zPL in zPLs)
            {
                string currentCustPONo = zPL.CustPONo;
                string currentStyleID = zPL.StyleID;
                string currentArticle = zPL.Article;
                string currentSizeCode = zPL.SizeCode;
                string currentShipQty = zPL.ShipQty;
                string fileName = zPL.FileName;

                string ss = this.Check_Before_Mapping(zPL);

                // 事前檢查
                if (ss != string.Empty)
                {
                    if (!this.ErrorMessage.Contains(ss))
                    {
                        this.ErrorMessage += ss + Environment.NewLine;
                    }

                    if (!this.mappingFailFileName.Contains(fileName))
                    {
                        this.mappingFailFileName.Add(fileName);
                    }

                    continue;
                }

                string sqlCmd = string.Empty;

                bool isCartonMix = this.Chk_Carton_Mix(currentCustPONo, currentStyleID, currentArticle, currentSizeCode, currentShipQty);

                sqlCmd = this.Get_MappingSQL(zPL, isCartonMix);

                DBProxy.Current.Select(null, sqlCmd, out mappingInfo);

                // Mapping到多少個PackingList_ID
                int mapped_PackingLisID_Count = mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count();

                // DB箱數(不分PackingListID)
                int totalCount = mappingInfo[0].Rows.Count;

                for (int i = 0; i <= totalCount - 1; i++)
                {
                    if (packingList_Detail_Ukeys.Contains(mappingInfo[0].Rows[i]["PackingList_Ukey"].ToString()))
                    {
                        mappingInfo[0].Rows[i].Delete();
                    }
                }

                mappingInfo[0].AcceptChanges();

                // ZPL張數
                int fileCount = zPLs.Where(o => o.SizeCode == currentSizeCode && o.ShipQty == currentShipQty).Count();

                // B03缺少的設定
                int brand_refno_NotMapping_Count = mappingInfo[2].AsEnumerable().Count();

                // ShippingMarkPicture是否有建立好 相同 BrandID CTNRefno Side 不同Seq IsSCC的兩筆資料
                bool packingB03_Lack = mappingInfo[1] == null ? true : (mappingInfo[1].Rows.Count == 1 ? false : true);

                #region Packing B03未設定

                // 有Mapping到PacingList_Detail才需要提示P03的檢查
                if (mapped_PackingLisID_Count > 0 && brand_refno_NotMapping_Count > 0 && !this.ErrorMessage.Contains("The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings."))
                {
                    this.ErrorMessage += "The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings." + Environment.NewLine;
                }

                // 準備新開的視窗
                if (mapped_PackingLisID_Count > 0 && brand_refno_NotMapping_Count > 0 && !this.NotSetB03_Table.AsEnumerable().Where(o =>
                            o["BrandID"].ToString() == mappingInfo[2].Rows[0]["BrandID"].ToString()
                            && o["ShippingMarkCombination"].ToString() == mappingInfo[2].Rows[0]["ShippingMarkCombination"].ToString()
                            && o["ShippingMarkType"].ToString() == mappingInfo[2].Rows[0]["ShippingMarkType"].ToString()
                            && o["RefNO"].ToString() == mappingInfo[2].Rows[0]["RefNO"].ToString()).Any())
                {
                    upateModel_List.RemoveAll(o => o.StyleID == currentStyleID && o.CustPONO == currentCustPONo);
                    foreach (DataRow dr in mappingInfo[2].Rows)
                    {
                        DataRow ndr = this.NotSetB03_Table.NewRow();
                        ndr["BrandID"] = dr["BrandID"].ToString();
                        ndr["ShippingMarkCombination"] = dr["ShippingMarkCombination"].ToString();
                        ndr["ShippingMarkType"] = dr["ShippingMarkType"].ToString();
                        ndr["RefNO"] = dr["RefNO"].ToString();
                        this.NotSetB03_Table.Rows.Add(ndr);
                    }

                    if (!this.mappingFailFileName.Contains(fileName))
                    {
                        this.mappingFailFileName.Add(fileName);
                    }

                    packingB03DataError = true;
                    continue;
                }

                if (packingB03DataError)
                {
                    if (!this.mappingFailFileName.Contains(fileName))
                    {
                        this.mappingFailFileName.Add(fileName);
                    }

                    continue;
                }

                #endregion

                if (mapped_PackingLisID_Count == 1)
                {
                    if (totalCount == fileCount)
                    {
                        // 1
                        packingList_Detail_Ukeys.Add(mappingInfo[0].Rows[0]["PackingList_Ukey"].ToString());
                        UpdateModel model = new UpdateModel()
                        {
                            PackingListID = mappingInfo[0].Rows[0]["PackingListID"].ToString(),
                            CustPONO = zPL.CustPONo,
                            StyleID = zPL.StyleID,
                            Article = zPL.Article,
                            SizeCode = zPL.SizeCode,
                            ShipQty = zPL.ShipQty,
                            CustCTN = zPL.CustCTN,
                            FileName = fileName,
                            SCICtnNo = mappingInfo[0].Rows[0]["SCICtnNo"].ToString(),
                            RefNo = mappingInfo[0].Rows[0]["RefNo"].ToString(),
                        };
                        upateModel_List.Add(model);
                    }
                    else
                    {
                        // false
                        if (!this.ErrorMessage.Contains("Carton count not mapping."))
                        {
                            this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                        }

                        if (!this.FileCountError_Table.AsEnumerable().Where(o => o["PO#"].ToString() == zPL.CustPONo && o["SKU"].ToString() == (zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode) && o["Qty"].ToString() == zPL.ShipQty).Any())
                        {
                            DataRow ndr = this.FileCountError_Table.NewRow();
                            ndr["PO#"] = zPL.CustPONo;
                            ndr["SKU"] = zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode;
                            ndr["Qty"] = zPL.ShipQty;

                            this.FileCountError_Table.Rows.Add(ndr);
                        }

                        foreach (var item in zPLs)
                        {
                            if (!this.mappingFailFileName.Contains(item.FileName))
                            {
                                this.mappingFailFileName.Add(item.FileName);
                            }
                        }
                    }
                }
                else if (mapped_PackingLisID_Count > 1)
                {
                    if (totalCount == fileCount)
                    {
                        // 02 false
                        if (!this.ErrorMessage.Contains("Carton count not mapping."))
                        {
                            this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                        }

                        if (!this.FileCountError_Table.AsEnumerable().Where(o => o["PO#"].ToString() == zPL.CustPONo && o["SKU"].ToString() == (zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode) && o["Qty"].ToString() == zPL.ShipQty).Any())
                        {
                            DataRow ndr = this.FileCountError_Table.NewRow();
                            ndr["PO#"] = zPL.CustPONo;
                            ndr["SKU"] = zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode;
                            ndr["Qty"] = zPL.ShipQty;

                            this.FileCountError_Table.Rows.Add(ndr);
                        }

                        if (!this.mappingFailFileName.Contains(fileName))
                        {
                            this.mappingFailFileName.Add(fileName);
                        }
                    }
                    else
                    {
                        // 判斷是03 04 05哪一種
                        int sameCount = 0;
                        string trueID = string.Empty;
                        List<string> iDList = new List<string>();
                        foreach (string packingListID in mappingInfo[0].AsEnumerable().Select(o => o["PackingListID"].ToString()).Distinct().ToList())
                        {
                            int ctn = mappingInfo[0].AsEnumerable().Where(o => o["PackingListID"].ToString() == packingListID).Count();

                            // 必須檢查總箱數都一致
                            int totalCount_DB = Convert.ToInt32(MyUtility.GetValue.Lookup($@"
SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{currentCustPONo}' AND StyleID='{currentStyleID}'

SELECT COUNT(pd.Ukey)
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders)
    AND pd.CustCTN = ''
	AND p.ID='{packingListID}'
"));

                            if (fileCount == ctn && totalFileCount == totalCount_DB)
                            {
                                sameCount++;
                                trueID = packingListID;
                                iDList.Add(packingListID);
                            }
                        }

                        if (sameCount > 1)
                        {
                            // 03
                            NewFormModel model = new NewFormModel()
                            {
                                PackingListIDs = iDList,
                                FileName = fileName,
                                ZPL_Content = mappingInfo[0], // 用於新視窗呈現詳細資料用
                                ZPL_List = zPLs, // 用於新視窗Call這邊
                            };
                            this.tmp_NewFormModels.Add(model);
                        }
                        else if (sameCount == 1)
                        {
                            packingList_Detail_Ukeys.Add(mappingInfo[0].Rows[0]["PackingList_Ukey"].ToString());

                            // 04
                            UpdateModel model = new UpdateModel()
                            {
                                PackingListID = trueID,
                                CustPONO = zPL.CustPONo,
                                StyleID = zPL.StyleID,
                                Article = zPL.Article,
                                SizeCode = zPL.SizeCode,
                                ShipQty = zPL.ShipQty,
                                CustCTN = zPL.CustCTN,
                                FileName = fileName,
                                SCICtnNo = mappingInfo[0].Rows[0]["SCICtnNo"].ToString(),
                                RefNo = mappingInfo[0].Rows[0]["RefNo"].ToString(),
                            };
                            upateModel_List.Add(model);
                        }
                        else
                        {
                            // 05 false
                            if (!this.ErrorMessage.Contains("Carton count not mapping."))
                            {
                                this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                            }

                            if (!this.FileCountError_Table.AsEnumerable().Where(o => o["PO#"].ToString() == zPL.CustPONo && o["SKU"].ToString() == (zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode) && o["Qty"].ToString() == zPL.ShipQty).Any())
                            {
                                DataRow ndr = this.FileCountError_Table.NewRow();
                                ndr["PO#"] = zPL.CustPONo;
                                ndr["SKU"] = zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode;
                                ndr["Qty"] = zPL.ShipQty;

                                this.FileCountError_Table.Rows.Add(ndr);
                            }

                            if (!this.mappingFailFileName.Contains(fileName))
                            {
                                this.mappingFailFileName.Add(fileName);
                            }
                        }
                    }
                }
                else
                {
                    if (!this.ErrorMessage.Contains("Carton count not mapping."))
                    {
                        this.ErrorMessage += "Carton count not mapping." + Environment.NewLine;
                    }

                    if (!this.FileCountError_Table.AsEnumerable().Where(o => o["PO#"].ToString() == zPL.CustPONo && o["SKU"].ToString() == (zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode) && o["Qty"].ToString() == zPL.ShipQty).Any())
                    {
                        DataRow ndr = this.FileCountError_Table.NewRow();
                        ndr["PO#"] = zPL.CustPONo;
                        ndr["SKU"] = zPL.StyleID + "-" + zPL.Article + "-" + zPL.SizeCode;
                        ndr["Qty"] = zPL.ShipQty;

                        this.FileCountError_Table.Rows.Add(ndr);
                    }

                    foreach (var item in zPLs)
                    {
                        if (!this.mappingFailFileName.Contains(item.FileName))
                        {
                            this.mappingFailFileName.Add(item.FileName);
                        }
                    }

                    break;
                }
            }

            return upateModel_List;
        }

        public bool P03_Database_Update(List<List<UpdateModel>> upateModel_List, string uploadType, bool isFromNewForm = false)
        {
            DualResult result;
            string updateCmd = string.Empty;
            string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from  System ");
            List<string> fileNames = new List<string>();
            int i = 0;

            foreach (List<UpdateModel> upateModels in upateModel_List)
            {
                if (upateModels.Count == 0)
                {
                    continue;
                }

                string fileName = upateModels.FirstOrDefault().FileName;
                fileNames.Add(fileName);

                foreach (var model in upateModels)
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
    AND pd.CustCTN='' 
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
    AND pd.CustCTN='' 
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
            }

            if (MyUtility.Check.Empty(updateCmd))
            {
                this.canConvert = false;
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

                    if (!isFromNewForm)
                    {
                        DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                        List<DataRow> dl = dt.AsEnumerable().Where(o => fileNames.Contains(o["FileName"].ToString())).ToList();
                        foreach (DataRow dr in dl)
                        {
                            dr["Result"] = "Pass";
                        }
                    }

                    if (isFromNewForm)
                    {
                        MyUtility.Msg.InfoBox("Assign Packing List successful!");
                    }
                    else
                    {
                        MyUtility.Msg.InfoBox("Data Mapping successful!");
                    }
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["Result"] = "Fail";
                    }

                    this.ShowErr(ex);
                }
            }

            return true;
        }

        public bool P24_Database_Update(List<List<UpdateModel>> upateModel_List, string uploadType, bool isFromNewForm = false, List<string> wattingForConvert = null, Dictionary<string, string> file_Name_PDF = null, List<string> wattingForConvert_contentsOfZPL = null)
        {
            DualResult result;
            string updateCmd = string.Empty;
            string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from  System ");
            List<string> fileNames = new List<string>();
            int i = 0;
            List<string> p24_HeadList = new List<string>();
            List<string> p24_BodyList = new List<string>();

            List<DataTable> dtList = new List<DataTable>();

            foreach (List<UpdateModel> upateModels in upateModel_List)
            {
                if (upateModels.Count == 0)
                {
                    continue;
                }

                #region 寫P24表頭

                var idList = upateModels.Select(o => new { o.PackingListID, o.RefNo }).Distinct().ToList();

                string p24_Head = string.Empty;
                int ii = 0;
                foreach (var item in idList)
                {
                    #region SQL
                    p24_Head += $@"
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

                p24_HeadList.Add(p24_Head);
                #endregion

                #region 寫P24表身
                DataTable dt = new DataTable();
                dt.ColumnsStringAdd("PackingListID");
                dt.ColumnsStringAdd("SCICtnNo");
                dt.ColumnsStringAdd("CustCTN");
                dt.ColumnsStringAdd("RefNo");

                // 混尺碼的話會重複，因此要DISTINCT
                var keys = upateModels.Select(o => new { o.PackingListID, o.SCICtnNo, o.CustCTN, o.RefNo }).Distinct().ToList();

                foreach (var model in keys)
                {
                    DataRow dr = dt.NewRow();

                    dr["PackingListID"] = model.PackingListID;
                    dr["SCICtnNo"] = model.SCICtnNo;
                    dr["CustCTN"] = model.CustCTN;
                    dr["RefNo"] = model.RefNo;

                    dt.Rows.Add(dr);
                }

                dtList.Add(dt);
                #endregion
            }

            // P24表身
            foreach (DataTable dt in dtList)
            {
                string cmd = string.Empty;

                cmd = $@"
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
WHERE  p.ID='{dt.Rows[0]["PackingListID"]}' AND pd.CustCTN='' 


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
	,[FileName]=dt.CustCTN 
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
INNER JOIN #tmp{i} dt ON t.PackingListID = dt.PackingListID AND t.RefNo = dt.RefNo AND t.SCICtnNo = dt.SCICtnNo
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
                        DataTable dd;
                        string cmd = p24_BodyList[idx];

                        if (!(result = MyUtility.Tool.ProcessWithDatatable(dt, null, cmd, out dd, temptablename: $"#tmp{idx}")))
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

                #region 若有新開視窗的部分，則會先轉新開視窗的圖檔，因此先把那些檔案的圖檔挑出來轉檔，新視窗結束後，剩下的再轉

                if (isFromNewForm)
                {
                    foreach (List<UpdateModel> upateModels in upateModel_List)
                    {
                        var keys = upateModels.Select(o => new { o.SCICtnNo, o.CustCTN }).Distinct().ToList();
                        foreach (var item in keys)
                        {
                            string findZpl = wattingForConvert_contentsOfZPL.Where(o => o.Contains($"^FD>;>8{item.CustCTN}^FS")).FirstOrDefault();
                            string indImg = wattingForConvert.Where(o => o.Contains(item.CustCTN)).FirstOrDefault();

                            if (!MyUtility.Check.Empty(indImg))
                            {
                                zplImg.Add(indImg);
                            }

                            if (!MyUtility.Check.Empty(findZpl))
                            {
                                zplContent.Add(findZpl);
                            }

                            if (file_Name_PDF.Where(o => o.Value == item.CustCTN).Any())
                            {
                                pdfImg.Add(file_Name_PDF.Where(o => o.Value == item.CustCTN).FirstOrDefault().Key, item.CustCTN);
                            }
                        }
                    }
                }
                else
                {
                    foreach (List<UpdateModel> upateModels in upateModel_List)
                    {
                        var keys = upateModels.Select(o => new { o.SCICtnNo, o.CustCTN }).Distinct().ToList();
                        foreach (var item in keys)
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
                    }
                }

                #endregion

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
                DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                foreach (DataRow dr in dt.Rows)
                {
                    dr["Result"] = "Fail";
                }

                this.ShowErr(ex);
                return false;
            }

            return true;
        }

        private string Check_Before_Mapping(ZPL zPL)
        {
            string errorMSG = string.Empty;

            // CustCTN是否已經存在
            bool existsCustCTN = MyUtility.Check.Seek($"SELECT 1 FROM PackingList_Detail WHERE CustCTN='{zPL.CustCTN}' ");

            if (existsCustCTN)
            {
                errorMSG += "CustCTN has existsed.";

                if (!this.existsCustCTN_Table.AsEnumerable().Where(o => o["CustCTN"].ToString() == zPL.CustCTN).Any())
                {
                    DataRow ndr = this.existsCustCTN_Table.NewRow();
                    ndr["CustCTN"] = zPL.CustCTN;

                    this.existsCustCTN_Table.Rows.Add(ndr);
                }

                return errorMSG;
            }

            // CustPONo是否存在
            bool hasCustPONo = MyUtility.Check.Seek($@"SELECT ID ,StyleID ,POID FROM Orders WHERE CustPONo='{zPL.CustPONo}' AND StyleID='{zPL.StyleID}' ");

            if (!hasCustPONo)
            {
                errorMSG += "The following PO# can't be found in PPIC_P01!!";

                if (!this.NotMapCustPo_Table.AsEnumerable().Where(o => o["CustPo"].ToString() == zPL.CustPONo).Any())
                {
                    DataRow ndr = this.NotMapCustPo_Table.NewRow();
                    ndr["CustPo"] = zPL.CustPONo;

                    this.NotMapCustPo_Table.Rows.Add(ndr);
                }

                return errorMSG;
            }

            return errorMSG;
        }

        private string Get_MappingSQL(ZPL currentZPL, bool isCartonMixed)
        {
            string sqlCmd = string.Empty;

            if (isCartonMixed)
            {
                sqlCmd = $@"
------開始 Mapping ZPL

SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{currentZPL.CustPONo}' AND StyleID='{currentZPL.StyleID}'

SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey ,o.CustPONo ,o.StyleID ,pd.Article   ,pd.SCICtnNo ,p.BrandID ,pd.RefNo ,p.CustCDID
,[SizeCode]='{currentZPL.SizeCode}' ,pd.ShipQty ,[CustCTN] ='{currentZPL.CustCTN}'
INTO #tmp_PackingListDetail
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders)
    AND pd.CustCTN = ''
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

SELECT * FROM #tmp_PackingListDetail
		

---整理出：混尺碼的標準組合

SELECT DISTINCT [StickerCombinationUkey]=ISNULL(c.StickerCombinationUkey_MixPack 
,	(
	SELECT Ukey 
	FROM ShippingMarkCombination
	WHERE BrandID = p.BrandID AND Category='PIC'  AND IsDefault = 1 AND IsMixPack = 1
	)
),[IsMixPack]=1 ,p.BrandID,p.RefNo
INTO #tmp
FROM #tmp_PackingListDetail p
INNER JOIN CustCD c ON p.BrandID = c.BrandID AND p.CustCDID = c.ID

SELECT DISTINCT st.*,t.StickerCombinationUkey ,t.RefNo,[ShippingMarkCombination]=comb.ID
INTO #StdComb
FROM #tmp t
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = t.StickerCombinationUkey
INNER JOIN ShippingMarkCombination_Detail combD ON comb.Ukey = combD.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType st ON st.Ukey = combD.ShippingMarkTypeUkey
--------------

----與標準比對，找出現存、缺少的B03設定

SELECT DISTINCT p.BrandId,[ShippingMarkCombination]=comb.ID,p.RefNo,[ShippingMarkType]=st.ID ,p.PackingListID ,st.Ukey,sp.ShippingMarkCombinationUkey ,comb.IsMixPack
INTO #Current_ShippingMarkPicture
FROM #tmp_PackingListDetail p 
INNER JOIN ShippingMarkPicture sp ON sp.BrandID = p.BrandID AND sp.CTNRefno = p.RefNo AND sp.Category = 'PIC'
INNER JOIN ShippingMarkPicture_Detail spd ON sp.Ukey = spd.ShippingMarkPictureUkey
INNER JOIN ShippingMarkType st ON st.Ukey = spd.ShippingMarkTypeUkey
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = sp.ShippingMarkCombinationUkey

SELECT * 
FROM #Current_ShippingMarkPicture t
WHERE --t.IsMixPack=1 AND
EXISTS(
	SELECT 1
	FROM #StdComb s
	WHERE s.Ukey=t.Ukey AND s.StickerCombinationUkey = t.ShippingMarkCombinationUkey
)

SELECT s.BrandID,s.ShippingMarkCombination,[ShippingMarkType]=s.ID,s.RefNo
FROM  #StdComb s
WHERE 
NOT EXISTS(
	SELECT *
	FROM #Current_ShippingMarkPicture t
	WHERE s.Ukey=t.Ukey AND s.StickerCombinationUkey = t.ShippingMarkCombinationUkey
)
DROP TABLE #tmpOrders,#tmp_PackingListDetail,#tmp,#StdComb ,#Current_ShippingMarkPicture

";
            }
            else
            {
                sqlCmd = $@"
------開始 Mapping ZPL

SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{currentZPL.CustPONo}' AND StyleID='{currentZPL.StyleID}'

SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey ,o.CustPONo ,o.StyleID ,pd.Article   ,pd.SCICtnNo ,p.BrandID ,pd.RefNo ,p.CustCDID
,[SizeCode]='{currentZPL.SizeCode}' ,pd.ShipQty ,[CustCTN] ='{currentZPL.CustCTN}'
INTO #tmp_PackingListDetail
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders)
    AND pd.CustCTN = ''
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

SELECT * FROM #tmp_PackingListDetail
		

---整理出：非混尺碼的標準組合

SELECT DISTINCT [StickerCombinationUkey]=ISNULL(c.StickerCombinationUkey
,	(
	SELECT Ukey 
	FROM ShippingMarkCombination
	WHERE BrandID = p.BrandID AND IsDefault = 1 AND IsMixPack = 0
	)
),[IsMixPack]=0 ,p.BrandID,p.RefNo
INTO #tmp
FROM #tmp_PackingListDetail p
INNER JOIN CustCD c ON p.BrandID = c.BrandID AND p.CustCDID = c.ID

SELECT DISTINCT st.*,t.StickerCombinationUkey ,t.RefNo,[ShippingMarkCombination]=comb.ID
INTO #StdComb
FROM #tmp t
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = t.StickerCombinationUkey
INNER JOIN ShippingMarkCombination_Detail combD ON comb.Ukey = combD.ShippingMarkCombinationUkey
INNER JOIN ShippingMarkType st ON st.Ukey = combD.ShippingMarkTypeUkey
--------------

----與標準比對，找出現存、缺少的B03設定

SELECT DISTINCT p.BrandId,[ShippingMarkCombination]=comb.ID,p.RefNo,[ShippingMarkType]=st.ID ,p.PackingListID ,st.Ukey,sp.ShippingMarkCombinationUkey ,comb.IsMixPack
INTO #Current_ShippingMarkPicture
FROM #tmp_PackingListDetail p 
INNER JOIN ShippingMarkPicture sp ON sp.BrandID = p.BrandID AND sp.CTNRefno = p.RefNo AND sp.Category = 'PIC'
INNER JOIN ShippingMarkPicture_Detail spd ON sp.Ukey = spd.ShippingMarkPictureUkey
INNER JOIN ShippingMarkType st ON st.Ukey = spd.ShippingMarkTypeUkey
INNER JOIN ShippingMarkCombination comb ON comb.Ukey = sp.ShippingMarkCombinationUkey

SELECT * 
FROM #Current_ShippingMarkPicture t
WHERE --t.IsMixPack=0 AND
EXISTS(
	SELECT 1
	FROM #StdComb s
	WHERE s.Ukey=t.Ukey AND s.StickerCombinationUkey = t.ShippingMarkCombinationUkey
)

SELECT s.BrandID,s.ShippingMarkCombination,[ShippingMarkType]=s.ID,s.RefNo
FROM  #StdComb s
WHERE 
NOT EXISTS(
	SELECT *
	FROM #Current_ShippingMarkPicture t
	WHERE s.Ukey=t.Ukey AND s.StickerCombinationUkey = t.ShippingMarkCombinationUkey
)

DROP TABLE #tmpOrders,#tmp_PackingListDetail,#tmp,#StdComb ,#Current_ShippingMarkPicture
";
            }

            return sqlCmd;
        }

        /// <summary>
        /// 檢查該箱是否為混尺碼
        /// </summary>
        /// <param name="custPONo"></param>
        /// <param name="styleID"></param>
        /// <param name="article"></param>
        /// <param name="sizeCode"></param>
        /// <param name="shipQty"></param>
        /// <returns>bool</returns>
        private bool Chk_Carton_Mix(string custPONo, string styleID, string article, string sizeCode, string shipQty)
        {
            bool res = false;
            DataRow dr;
            string cmd = $@"

SELECT ID ,StyleID ,POID ,CustPONo
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{custPONo}' AND StyleID='{styleID}'

SELECT pd.ID,pd.SCICtnNo
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID IN (SELECT ID FROM #tmpOrders)
    AND pd.CustCTN = ''
    AND Article = '{article}'
    AND pd.ShipQty={shipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{sizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{sizeCode}'
        )
		AND (SELECT COUNT(qq.Ukey) FROM PackingList_Detail qq 
				where qq.ID = p.ID 
				AND qq.OrderID = pd.OrderID 
				AND qq.CTNStartNo = pd.CTNStartNo
				AND (qq.Article = pd.Article 
				OR qq.SizeCode = pd.SizeCode )
				and qq.Ukey != pd.Ukey) > 0
";
            res = MyUtility.Check.Seek(cmd, out dr);

            return res;
        }

        private void ShowErrorMessage()
        {
            if (this.existsCustCTN_Table.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(this.existsCustCTN_Table, "Exists Cust CTN", "Exists Cust CTN");
                m.Width = 850;
                m.grid1.Columns[0].Width = 200;
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                m.TopMost = true;
            }

            if (this.NotMapCustPo_Table.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(this.NotMapCustPo_Table, "No found Cust PO#", "No found Cust PO#");
                m.Width = 850;
                m.grid1.Columns[0].Width = 200;
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                m.TopMost = true;
            }

            if (this.NotSetB03_Table.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(this.NotSetB03_Table, "No Set Packing_B03", "No Set Packing_B03");
                m.Width = 850;
                m.grid1.Columns[0].Width = 200;
                m.grid1.Columns[0].Width = 50;
                m.grid1.Columns[0].Width = 100;
                m.text_Find.Width = 150;
                m.btn_Find.Location = new Point(170, 6);

                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                m.TopMost = true;
            }

            if (this.FileCountError_Table.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(this.FileCountError_Table, "Carton Count Not Mapping", "Carton Count Not Mapping");

                m.Width = 1000;

                m.grid1.Columns[0].Width = 400;
                m.grid1.Columns[1].Width = 400;
                m.grid1.Columns[2].Width = 100;
                m.text_Find.Width = 150;
                m.btn_Find.Location = new Point(170, 6);

                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                m.TopMost = true;
            }
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

        public class ZipHelper
        {
            /// <summary>
            /// 將串入的ZPL檔案轉成Zip檔資料流存在記憶體
            /// </summary>
            /// <returns>Q</returns>
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

        public class MixedCompare
        {
            public string Text { get; set; }

            public bool IsInt { get; set; }
        }

        public class SizeObject
        {
            public string Size { get; set; }

            public int Qty { get; set; }

            public string StyleID { get; set; }

            public string Article { get; set; }
        }

        public class ZPL
        {
            public string CustPONo { get; set; }

            public string StyleID { get; set; }

            public string Article { get; set; }

            public string SizeCode { get; set; }

            public string ShipQty { get; set; }

            public string CustCTN { get; set; }

            public string CTNStartNo { get; set; }

            public List<SizeObject> Size_Qty_List { get; set; } // 混尺碼用

            public string FileName { get; set; }
        }

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
        }

        public class NewFormModel
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <inheritdoc/>
            public DataTable ZPL_Content { get; set; }

            /// <inheritdoc/>
            public List<string> PackingListIDs { get; set; }

            /// <inheritdoc/>
            public List<ZPL> ZPL_List { get; set; }
        }

        /// <summary>
        /// 由於PDF上傳的檔案是散裝的，這個物件用於裝同一包的PDF
        /// </summary>
        public class PDF_Model
        {
            /// <inheritdoc/>
            public string GroupID { get; set; }

            /// <inheritdoc/>
            public List<ZPL> UpdateModels { get; set; }
        }

        public class File_Name_Object_List
        {
            /// <inheritdoc/>
            public List<File_Name_Object2> File_Name_Object2s { get; set; }
        }

        public class File_Name_Object2
        {
            /// <inheritdoc/>
            public string FileName { get; set; }

            /// <inheritdoc/>
            public List<ZPL> ZPLs { get; set; }
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
        public void ConvertPDF2Image(string pdfInputPath, string imageOutputPath, string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition)
        {
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(pdfInputPath);

            // 提高解析度
            Image bmp = doc.SaveAsImage(0, PdfImageType.Bitmap, 300, 300);

            // bmp.Save(imageOutputPath + imageName + "_tmpOri.bmp");
            byte[] data = null;

            if (!System.IO.Directory.Exists(imageOutputPath))
            {
                Directory.CreateDirectory(imageOutputPath);
            }

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
            para.Add(new SqlParameter("@Image", (object)dataArry));

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

        public enum Definition
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

    }
}
