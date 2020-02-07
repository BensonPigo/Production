using Ict;
using Ict.Win;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using PDFLibNet32;
using Sci.Data;
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
using System.Text.RegularExpressions;
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
        private List<string> wattingForConvert = new List<string>();
        private List<string> wattingForConvert_contentsOfZPL = new List<string>();
        private UploadType currentFileType;
        private DataTable NotSetB03_Table;
        private DataTable NotMapCustPo_Table;
        private DataTable existsCustCTN_Table;

        public bool canConvert = false;

        private List<MappingModel> MappingModels = new List<MappingModel>();
        private List<MappingModel_PDF> MappingModel_PDFs = new List<MappingModel_PDF>();

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
                    //Array.Clear(this.wattingForConvert_contentsOfZPL, 0, this.wattingForConvert_contentsOfZPL.Length);
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
                        List<string> ZPL_FileName_List = new List<string>();

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
                                tmpzplContent = oriZplConten.Replace("\r\n", string.Empty);

                                // 1-3.先取得檔名，CustCTN被包在 ^FD>;>8 和 ^FS之間，取得CustCTN，作為檔名
                                tmpArray = tmpzplContent.Split(new string[] { "^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH0,0^LRN^XZ^XA^CI0^FO80,50^BY4^BCN,200,N,N,^FD>;>8", "^FS^FT115,280^A0N,34,47^FD" }, StringSplitOptions.RemoveEmptyEntries);
                                ZPL_FileName_List = tmpArray.Where(o => !o.Contains("^")).Distinct().ToList();

                                // 1-4.拆出多個ZPL檔的內容，每一個ZPL都是以 ^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH0,0^LRN^XZ^XA^CI0 開頭
                                tmpzplContent = tmpzplContent.Replace("^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH0,0^LRN^XZ^XA^CI0", "\r\n^XA^SZ2^JMA^MCY^PMN^PW796~JSN^JZY^LH0,0^LRN^XZ^XA^CI0");

                                string[] stringSeparators = new string[] { "\r\n" };

                                // 1-5.最後拆出來的每一個ZPL，包含三張圖片
                                contentsOfZPL = tmpzplContent.Split(stringSeparators, StringSplitOptions.None);
                            }

                            // 2.根據ZPL檔名，取得對應的內容
                            Dictionary<string, string> FileName_with_Data = new Dictionary<string, string>();

                            foreach (string singleFileName in ZPL_FileName_List)
                            {
                                string contentString = contentsOfZPL.Where(o => o.Contains(singleFileName)).FirstOrDefault();
                                FileName_with_Data.Add(singleFileName, contentString);
                            }

                            // 3.透過API將ZPL檔轉成PDF，並存到指定路徑
                            this.wattingForConvert.AddRange( ZPL_FileName_List);
                            this.wattingForConvert_contentsOfZPL.AddRange(contentsOfZPL.Where(o=>o != string.Empty).ToList());

                            // 4.從單張ZPL內容中，拆解出需要的欄位資訊，用於Mapping方便
                            List<ZPL> zPL_Objects = this.Analysis_ZPL(FileName_with_Data, ZPL_FileName_List);

                            this.File_Name_Object.Add(openFileDialog1.SafeFileNames[i], zPL_Objects);
                            this._File_Name_Object_List.File_Name_Object2s.Add(new File_Name_Object2()
                            {
                                FileName = openFileDialog1.SafeFileNames[i],
                                ZPLs = zPL_Objects
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

                            List<ZPL> zPL_Objects = new List<ZPL>();

                            // 若是混尺碼則分開處理
                            if (tmppArray.Contains("MIXED"))
                            {
                                string[] Sizes = tmppArray[Array.IndexOf(tmppArray, tmppArray.Where(o => o == "Size/Qty").FirstOrDefault()) + 1].Split(' ');
                                string[] qtyOfSizes = tmppArray[Array.IndexOf(tmppArray, tmppArray.Where(o => o == "Size/Qty").FirstOrDefault()) + 2].Split(' ');

                                List<SizeObject> size_qty = new List<SizeObject>();
                                List<MixedCompare> MixedCompares = new List<MixedCompare>();

                                // 判斷是否有另外小包裝，# of Prepacks: 2 of 12這類的文字，沒有的話全部都算 1 就好
                                string tmpStr = string.Empty;
                                int q = 0;
                                int getMixInfoIndex = tmppArray.ToList().IndexOf(tmppArray.Where(o => o.Contains("of ")).FirstOrDefault());

                                if (getMixInfoIndex != -1)
                                {
                                    for (int ix = 0; ix <= tmppArray[getMixInfoIndex].Length - 1; ix++)
                                    {
                                        MixedCompares.Add(new MixedCompare() {
                                            Text = tmppArray[getMixInfoIndex][ix].ToString(),
                                            IsInt = int.TryParse(tmppArray[getMixInfoIndex][ix].ToString(), out q)
                                        });
                                    }

                                    foreach (var item in MixedCompares)
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
                                int QtyPerSmallPack = Convert.ToInt32(tmpStr);

                                // 每個尺寸的數量
                                for (int ix = 0; ix <= Sizes.Length - 1; ix++)
                                {
                                    size_qty.Add(new SizeObject() {
                                        Size = Sizes[ix],
                                        Qty = Convert.ToInt32(qtyOfSizes[ix]) * QtyPerSmallPack
                                    });
                                }

                                // 每個小包的總和，跟Qty的數字沒對上直接return
                                int MIXED_textindex = tmppArray.ToList().IndexOf("MIXED");
                                int Qty_Index = MIXED_textindex + 4;
                                if (size_qty.Sum(o => o.Qty) != Convert.ToInt32(tmppArray[Qty_Index]))
                                {
                                    MyUtility.Msg.InfoBox($"CustCTN: {tmppArray[0].Split(' ')[1]}, Total of <Size/Qty> is not equal <Qty> !!");
                                    this.HideWaitMessage();
                                    return;
                                }

                                int Qty_textindex = tmppArray.ToList().IndexOf("Qty:");
                                int CustPONo_index = Qty_textindex + 1;
                                int Style_index = Qty_textindex + 2;
                                int SizeCode_index = Qty_textindex + 4;
                                int ShipQty_index = Qty_textindex + 8;
                                zPL_Objects.Add(
                                     new ZPL()
                                     {
                                         CustCTN = tmppArray[0].Split(' ')[1],
                                         CustPONo = tmppArray[CustPONo_index],
                                         StyleID = tmppArray[Style_index].Split('-')[0],
                                         Article = tmppArray[Style_index].Split('-')[1],
                                         SizeCode = tmppArray[SizeCode_index], //MIXED
                                         ShipQty = tmppArray[ShipQty_index],
                                         Size_Qty_List = size_qty
                                     }
                                 );
                            }
                            else
                            {
                                zPL_Objects.Add(
                                     new ZPL()
                                     {
                                         CustCTN = tmppArray[0].Split(' ')[1],
                                         CustPONo = tmppArray[21],
                                         StyleID = tmppArray[22].Split('-')[0],
                                         Article = tmppArray[22].Split('-')[1],
                                         SizeCode = tmppArray[24],
                                         ShipQty = tmppArray[28]
                                     }
                                 );
                            }

                            this.File_Name_Object.Add(openFileDialog1.SafeFileNames[i], zPL_Objects);
                            this._File_Name_Object_List.File_Name_Object2s.Add(new File_Name_Object2()
                            {
                                FileName = openFileDialog1.SafeFileNames[i],
                                ZPLs = zPL_Objects
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
        private List<ZPL> Analysis_ZPL(Dictionary<string, string> FileName_with_Data, List<string> ZPL_FileName_List)
        {
            List<ZPL> list = new List<ZPL>();

            foreach (string custCTNno in ZPL_FileName_List)
            {
                string[] strArray; // 取得CustCTN過程中，暫存用
                string[] strArray2; // 取得CustCTN過程中，暫存用
                bool IsMiexed = false;
                string content = FileName_with_Data[custCTNno];

                // 是否混尺碼
                IsMiexed = content.ToUpper().Contains("MIXED");

                // Orders.CustPONo
                strArray = content.Split(new string[] { "^FDPO#:^FS^FT225,850^A0B,40,50^FD", "^FS^FT280,950^A0B,40,50^FDSKU:^FS" }, StringSplitOptions.RemoveEmptyEntries);
                string CustPONo = strArray[1];

                if (IsMiexed)
                {
                    strArray2 = content.Split(new string[] { "^FS^FT280,950^A0B,40,50^FDSKU:^FS^FT280,850^A0B,40,50^FD", "^FS^FT" }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    strArray2 = content.Split(new string[] { "^FS^FT280,950^A0B,40,50^FDSKU:^FS^FT280,850^A0B,40,50^FD", "^FS^FT400,950^A0B,40,50^FDQTY:" }, StringSplitOptions.RemoveEmptyEntries);
                }

                // 使用規則運算是，從陣列中找出 OO-OO-OO 的文字，O代表任意字元
                //Regex r = new Regex(@"^\w{1,}\-\w{1,}\-\w{1,}$");

                string StyleID_Article_SizeCode = string.Empty ;
                string StyleID = string.Empty;
                string Article = string.Empty;
                string SizeCode = string.Empty;
                string ShipQty = string.Empty;
                string CTNStartNo = string.Empty;

                List <SizeObject> SizeObjects = new List<SizeObject>();

                if (IsMiexed)
                {
                    int startIndex = Array.IndexOf(strArray2, strArray2.Where(o => o.ToString().Contains("Style-Color-Size")).FirstOrDefault());
                    int endIndex = Array.IndexOf(strArray2, strArray2.Where(o => o.ToString().Contains("Total Qty")).FirstOrDefault());
                    int sizeCount = (endIndex - startIndex - 1) / 4;
                    List<string> tmpSizes = new List<string>();
                    int ii = 0;
                    for (int i = startIndex+1; i <= endIndex; i++)
                    {
                        string tmpSize = string.Empty;
                        if (i % 4 == 2 )
                        {
                            string[] stringSeparators = new string[] { "^FD" };
                            string sku = strArray2[i].Split(stringSeparators, StringSplitOptions.None)[1];
                            StyleID = sku.Split('-')[0];
                            Article = sku.Split('-')[1];
                            tmpSize = sku.Split('-')[2];
                            tmpSizes.Add(tmpSize);
                        }
                        if (i % 4 == 0)
                        {
                            string[] stringSeparators = new string[] { "^FD" };
                            string qty = strArray2[i].Split(stringSeparators, StringSplitOptions.None)[1];
                            SizeObjects.Add(new SizeObject()
                            {
                                Size= tmpSizes[ii],
                                Qty=Convert.ToInt32(qty.TrimStart().TrimEnd())
                            });
                            ii++;
                        }
                    }

                }
                else
                {
                    StyleID_Article_SizeCode = strArray2[1]; // strArray2.Where(o => r.IsMatch(o)).FirstOrDefault();
                    // Orders.StyleID
                    StyleID = StyleID_Article_SizeCode.Split('-')[0];
                    // Orders.Article
                    Article = StyleID_Article_SizeCode.Split('-')[1];
                    // Orders.SizeCode
                    SizeCode = StyleID_Article_SizeCode.Split('-')[2];
                    // PackingList_Detail.ShipQty
                    strArray = content.Split(new string[] { "^FS^FT400,950^A0B,40,50^FDQTY:^FS^FT400,850^A0B,75,100^FD", "^FS^FO425,700^BY3^B3B,N,75,N,N^FD" }, StringSplitOptions.RemoveEmptyEntries);
                    ShipQty = strArray[1];

                    // PackingList_Detail.CTNStartNo
                    strArray = content.Split(new string[] { "^FDBOX:^FS^FT700,590^A0B,48,65^FD", "^FS^FO0,960^GB775,0,4^FS^FT115,995^A0N,34,47,^FD" }, StringSplitOptions.RemoveEmptyEntries);
                    CTNStartNo = strArray[1];
                }

                // PackingList_Detail.CustCTN
                string CustCTN = custCTNno;

                list.Add(new ZPL()
                {
                    CustPONo = CustPONo,
                    CustCTN = CustCTN,
                    StyleID = StyleID,
                    Article = Article,
                    SizeCode = SizeCode,
                    ShipQty = ShipQty,
                    CTNStartNo = CTNStartNo,
                    Size_Qty_List = SizeObjects
                });
            }

            return list;
        }

        private void BtnProcessing_Click(object sender, EventArgs e)
        {
            try
            {
                bool isPackingLisID_CountError = false;
                this.MappingModels.Clear();
                this.MappingModel_PDFs.Clear();
                this.NotSetB03_Table = new DataTable();
                this.NotMapCustPo_Table = new DataTable();
                this.existsCustCTN_Table = new DataTable();
                List<string> removePOs = new List<string>();
                List<string> removeFileNames = new List<string>();
                string msg = string.Empty;

                this.ShowWaitMessage("Data Mapping....");

                #region 開始Mapping

                if (this.currentFileType == UploadType.PDF)
                {
                    List<ZPL> Packingist_Details = new List<ZPL>();
                    List<string> fileNames = new List<string>();
                    foreach (var item in this._File_Name_Object_List.File_Name_Object2s)
                    {
                        foreach (ZPL zPL in item.ZPLs)
                        {
                            zPL.FileName = item.FileName;
                            if (zPL.Size_Qty_List != null)
                            {
                                foreach (var size_Qty in zPL.Size_Qty_List)
                                {
                                    Packingist_Details.Add(zPL);
                                }
                            }
                            else
                            {
                                Packingist_Details.Add(zPL);
                            }
                        }

                        fileNames.Add(item.FileName);
                    }

                    msg = this.PDF_Mapping(Packingist_Details, fileNames);
                }

                if (this.currentFileType == UploadType.ZPL)
                {

                    this.NotSetB03_Table.ColumnsStringAdd("BrandID");
                    this.NotSetB03_Table.ColumnsStringAdd("RefNo");
                    this.NotSetB03_Table.ColumnsStringAdd("Side");
                    this.NotMapCustPo_Table.ColumnsStringAdd("CustPO");
                    this.existsCustCTN_Table.ColumnsStringAdd("CustCTN");

                    foreach (var item in this._File_Name_Object_List.File_Name_Object2s)
                    {
                        // 根據上傳的ZPL展開
                        string fileName = item.FileName;
                        List<ZPL> ZPLs = item.ZPLs;
                        DataTable[] tmpDts;

                        var chkCount = ZPLs.Select(o => new
                        {
                            CustPONo = o.CustPONo,
                            StyleID = o.StyleID,
                            Article = o.Article
                        }).Distinct().ToList();

                        foreach (var obj in chkCount)
                        {
                            string cmd = $@"
----統計PackingList的筆數，與分析出來的檔案數量至否一致
SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{obj.CustPONo}' AND StyleID='{obj.StyleID}'

SELECT COUNT(DISTINCT CTNStartNo)
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID = (SELECT ID FROM #tmpOrders)
    --AND pd.CustCTN = ''
    AND Article = '{obj.Article}'

DROP TABLE #tmpOrders 
";

                            string packingListCount = MyUtility.GetValue.Lookup(cmd);

                            string fileCount = ZPLs.Where(o => o.CustPONo == obj.CustPONo && o.StyleID == obj.StyleID && o.Article == obj.Article).Count().ToString();

                            if (packingListCount != fileCount)
                            {
                                List<string> removePO = ZPLs.Where(o => o.CustPONo == obj.CustPONo && o.StyleID == obj.StyleID && o.Article == obj.Article).Select(o => o.CustPONo).Distinct().ToList();

                                removePOs.AddRange(removePO);
                                removeFileNames.Add(item.FileName);
                            }

                        }

                        if (removeFileNames.Count > 0 && removePOs.Count > 0)
                        {
                            foreach (var pono in removePOs)
                            {
                                this.wattingForConvert_contentsOfZPL = this.wattingForConvert_contentsOfZPL.Where(o => !o.Contains(pono)).ToList();
                            }

                            foreach (var obj in ZPLs)
                            {
                                this.wattingForConvert.Remove(obj.CustCTN);
                            }
                            //foreach (var obj in this.wattingForConvert)
                            //{
                            //    ZPL remove = new ZPL() { CustCTN = obj };
                            //    ZPLs.Remove(remove);
                            //}
                            continue;
                        }

                        foreach (var zpl in ZPLs)
                        {

                            // 確認一個ZPL檔，對應到幾個PackingList
                            #region SQL檢查對應到幾個PackingList

                            string sqlCmd = string.Empty;
                            List<string> sqlMixed = new List<string>();
                            bool IsMixed = zpl.SizeCode.ToUpper().Contains("MIX") || (MyUtility.Check.Empty(zpl.SizeCode) && zpl.Size_Qty_List != null);

                            if (IsMixed)
                            {
                                sqlCmd = $@"
----開始Mapping
SELECT ID ,StyleID ,POID
INTO #tmoOrders
FROM Orders 
WHERE CustPONo='{zpl.CustPONo}' AND StyleID='{zpl.StyleID}'
";

                                int i = 0;
                                foreach (var data in zpl.Size_Qty_List)
                                {
                                    sqlCmd += $@"

SELECT  CTNStartNo,[CartonCount]=COUNT(pd.Ukey)
INTO #tmpCount{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
WHERE p.Type ='B'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders)
    AND pd.CustCTN='' 
    AND Article = '{zpl.Article}'
	AND ( SizeCode='{data.Size}' OR SizeCode in(
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders) AND SizeSpec IN ('{data.Size}')
	        ))  
	AND pd.ShipQty={data.Qty}
GROUP BY CTNStartNo

";
                                    i++;
                                }

                                sqlCmd += $@"
SELECT a.CTNStartNo,[CartonCount]=SUM(CartonCount) 
INTO #tmpMappingCartonNo
FROM (
";
                                i = 0;
                                foreach (var data in zpl.Size_Qty_List)
                                {
                                    sqlMixed.Add($@"
	SELECT  *
	FROM #tmpCount{i}
");
                                    i++;
                                }

                                sqlCmd += string.Join(" UNION ALL" + Environment.NewLine, sqlMixed);
                                sqlCmd += $@"

)a
GROUP BY CTNStartNo

----SQL檢查對應到幾個PackingList
SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders)
    AND pd.CustCTN='' 
    AND Article = '{zpl.Article}'
    AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo) 
	AND pd.SCICtnNo <> ''

----ShippingMarkPicture
SELECT DISTINCT o.BrandID ,pd.RefNo
INTO #tmp
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders)
    AND pd.CustCTN='' 
    AND Article = '{zpl.Article}'
    AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo) 
	AND pd.SCICtnNo <> ''



----檢查是否A面一張 D面一張，A面的會是 IsSSCC
SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
INTO #ExistsB03
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='D'
WHERE IsSSCC = 0
UNION
SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='A'
WHERE IsSSCC = 1

SELECT * FROM #ExistsB03

----列出缺少的B03檔案
SELECT  * FROM (
SELECT t.BrandID ,t.RefNo , Side='D'
FROM #tmp t
LEFT JOIN ShippingMarkPicture s ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='D'
UNION
SELECT t.BrandID ,t.RefNo , Side='A'
FROM #tmp t
LEFT JOIN ShippingMarkPicture s ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='A'
) a
WHERE NOT EXISTS (SELECT 1 FROM #ExistsB03 t WHERE a.BrandID=t.BrandID AND a.RefNo=t.RefNo AND a.Side=t.Side)

";
                            }
                            else
                            {
                                sqlCmd = $@"
----開始Mapping

SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{zpl.CustPONo}' AND StyleID='{zpl.StyleID}'

SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID = (SELECT ID FROM #tmpOrders)
    AND pd.CustCTN = ''
    AND Article = '{zpl.Article}'
    AND pd.ShipQty={zpl.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{zpl.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{zpl.SizeCode}'
        )
		
SELECT DISTINCT o.BrandID ,pd.RefNo
INTO #tmp
FROM PackingList p
INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type = 'B'
AND pd.OrderID = (SELECT ID FROM #tmpOrders)
AND pd.CustCTN = ''
AND Article = '{zpl.Article}'
AND pd.ShipQty ={ zpl.ShipQty}
AND(
    pd.SizeCode in
    (
        SELECT SizeCode

        FROM Order_SizeSpec

        WHERE SizeItem = 'S01' AND ID IN(SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{zpl.SizeCode}')
        )

    OR

    pd.SizeCode = '{zpl.SizeCode}'
)


----檢查是否A面一張 D面一張，A面的會是 IsSSCC
SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
INTO #ExistsB03
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='D'
WHERE IsSSCC = 0
UNION
SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
FROM ShippingMarkPicture s
INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='A'
WHERE IsSSCC = 1

SELECT * FROM #ExistsB03

----列出缺少的B03檔案
SELECT  * FROM (
SELECT t.BrandID ,t.RefNo , Side='D'
FROM #tmp t
LEFT JOIN ShippingMarkPicture s ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='D'
UNION
SELECT t.BrandID ,t.RefNo , Side='A'
FROM #tmp t
LEFT JOIN ShippingMarkPicture s ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo AND s.Side='A'
) a
WHERE NOT EXISTS (SELECT 1 FROM #ExistsB03 t WHERE a.BrandID=t.BrandID AND a.RefNo=t.RefNo AND a.Side=t.Side)

DROP TABLE #tmpOrders ,#tmp ,#ExistsB03
";
                            }

                            #endregion

                            DBProxy.Current.Select(null, sqlCmd, out tmpDts);
                            sqlMixed.Clear();

                            // Mapping到多少個PackingList_ID，只能有一個
                            int packingLisID_Count = tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count();

                            int brand_refno_Count = tmpDts[2].AsEnumerable().Count();

                            // 計算「一個檔案內」，相同 CustPONo StyleID Article SizeCode ShipQty的檔案數，一個ZPL可能包含多個ZPL
                            int file_Count_inOneZPL = ZPLs.Where(o => o.Article == zpl.Article && o.SizeCode == zpl.SizeCode && o.ShipQty == zpl.ShipQty).Count();

                            // 檔案數量 是否等於 PackingList_Detail的筆數

                            // 計算「不同圖檔案之間」，相同 CustPONo StyleID Article SizeCode ShipQty的檔案數  PDF
                            int file_Count = 0;

                            foreach (File_Name_Object2 t in this._File_Name_Object_List.File_Name_Object2s)
                            {
                                if (t.ZPLs.Where(z => z.Article == zpl.Article
                                     && z.SizeCode == zpl.SizeCode
                                     && z.ShipQty == zpl.ShipQty
                                     && z.CustPONo == zpl.CustPONo
                                     && z.StyleID == zpl.StyleID).Any())
                                {
                                    file_Count++;
                                }
                            }

                            // CustCTN是否已經存在
                            bool existsCustCTN = MyUtility.Check.Seek($"SELECT 1 FROM PackingList_Detail WHERE CustCTN='{zpl.CustCTN}' ");

                            // ShippingMarkPicture是否有建立好 相同 BrandID CTNRefno Side 不同Seq IsSCC的兩筆資料
                            bool packingB03DataError = tmpDts[1] == null ? true : (tmpDts[1].Rows.Count == 2 ? false : true);

                            bool contuineCheck = true;

                            bool hasCustPONo = MyUtility.Check.Seek($@"   
    SELECT ID ,StyleID ,POID
    FROM Orders 
    WHERE CustPONo='{zpl.CustPONo}' AND StyleID='{zpl.StyleID}' ");

                            if (this.currentFileType == UploadType.PDF)
                            {
                                packingB03DataError = false;
                            }

                            if (packingLisID_Count > 1 ||
                                !hasCustPONo ||
                                existsCustCTN ||
                                packingB03DataError)
                            {
                                #region 1.CustCTN已存在
                                if (existsCustCTN && !msg.Contains("CustCTN has existsed."))
                                {
                                    msg += "CustCTN has existsed." + Environment.NewLine;
                                }

                                if (existsCustCTN)
                                {
                                    contuineCheck = false;
                                }

                                if (existsCustCTN && !this.existsCustCTN_Table.AsEnumerable().Where(o => o["CustCTN"].ToString() == zpl.CustCTN).Any() && !contuineCheck)
                                {
                                    DataRow ndr = this.existsCustCTN_Table.NewRow();
                                    ndr["CustCTN"] = zpl.CustCTN;

                                    this.existsCustCTN_Table.Rows.Add(ndr);
                                }

                                #endregion

                                #region 2.CustPO不符合

                                // 準備要跳出來的資料
                                if (!hasCustPONo && tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && contuineCheck && !msg.Contains("The following PO# can't be found in PPIC_P01!!"))
                                {
                                    msg += "The following PO# can't be found in PPIC_P01!!" + Environment.NewLine;
                                }

                                if (!hasCustPONo && tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && contuineCheck)
                                {
                                    contuineCheck = false;
                                }

                                // 準備要跳出來的資料
                                if (!hasCustPONo && tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && !this.NotMapCustPo_Table.AsEnumerable().Where(o => o["CustPo"].ToString() == zpl.CustPONo).Any() && (tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && !contuineCheck))
                                {
                                    DataRow ndr = this.NotMapCustPo_Table.NewRow();
                                    ndr["CustPo"] = zpl.CustPONo;

                                    this.NotMapCustPo_Table.Rows.Add(ndr);
                                }
                                #endregion

                                #region 3.Packing B03未設定

                                // 有Mapping到PacingList_Detail才需要提示P03的檢查
                                if (packingLisID_Count > 0 && packingB03DataError && !msg.Contains("The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings.") && contuineCheck)
                                {
                                    msg += "The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings." + Environment.NewLine;
                                }

                                if (packingLisID_Count > 0 && packingB03DataError && contuineCheck)
                                {
                                    contuineCheck = false;
                                }

                                // 準備新開的視窗
                                if (packingLisID_Count > 0 && packingB03DataError && brand_refno_Count > 0 && !this.NotSetB03_Table.AsEnumerable().Where(o => o["BrandID"].ToString() == tmpDts[2].Rows[0]["BrandID"].ToString() && o["RefNO"].ToString() == tmpDts[2].Rows[0]["RefNO"].ToString() && o["Side"].ToString() == tmpDts[2].Rows[0]["Side"].ToString() ).Any() && !contuineCheck)
                                {
                                    foreach (DataRow dr in tmpDts[2].Rows)
                                    {
                                        DataRow ndr = this.NotSetB03_Table.NewRow();
                                        ndr["BrandID"] = dr["BrandID"].ToString();
                                        ndr["RefNO"] = dr["RefNO"].ToString();
                                        ndr["Side"] = dr["Side"].ToString();
                                        this.NotSetB03_Table.Rows.Add(ndr);
                                    }

                                }

                                #endregion

                                #region 4.其餘Not Mapping狀況
                                if ((tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0)  && !msg.Contains("Data Not Mapping.") && contuineCheck)
                                {
                                    msg += "Data Not Mapping." + Environment.NewLine;
                                }

                                if (tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && contuineCheck)
                                {
                                    contuineCheck = false;
                                }
                                #endregion

                                if (!this.MappingModels.Where(o => o.FileName == fileName).Any())
                                {
                                    // 1個以上PackingList_Detail
                                    MappingModel model = new MappingModel()
                                    {
                                        FileName = fileName,
                                        ZPL_Content = ZPLs,
                                        PackingListID = string.Empty,
                                        IsMixed = IsMixed
                                    };
                                    this.MappingModels.Add(model);
                                }
                            }
                            else
                            {
                                // PackingList_Detail的箱數夠
                                if (!this.MappingModels.Where(o => o.FileName == fileName).Any())
                                {
                                    MappingModel model = new MappingModel()
                                    {
                                        FileName = fileName,
                                        ZPL_Content = ZPLs,
                                        PackingListID = tmpDts[0].Rows[0]["PackingListID"].ToString(),
                                        IsMixed = IsMixed
                                    };
                                    this.MappingModels.Add(model);
                                }
                            }

                        }
                    }

                }
                #endregion

                if (removeFileNames.Count > 0 && removePOs.Count > 0)
                {
                    MyUtility.Msg.InfoBox("The following PO# mapping failed, please check the import file!!" + Environment.NewLine + string.Join(",", removePOs));
                    foreach (var removeFileName in removeFileNames)
                    {
                        DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                        List<DataRow> dl = dt.AsEnumerable().Where(o => removeFileNames.Contains(o["FileName"].ToString())).ToList();
                        foreach (DataRow dr in dl)
                        {
                            dr["Result"] = "Fail";
                        }
                    }
                }

                this.HideWaitMessage();

                bool notMapping = false;

                if (this.currentFileType == UploadType.ZPL)
                {
                    this.MappingModels.Where(o => o.PackingListID == string.Empty).Any();
                }

                if (!MyUtility.Check.Empty(msg))
                {
                    //MyUtility.Msg.InfoBox(msg);
                    notMapping = true;
                }
                if (this.existsCustCTN_Table.Rows.Count > 0)
                {
                    MyUtility.Msg.ShowMsgGrid(this.existsCustCTN_Table, msg,  "Exists Cust CTN");

                }

                if (this.NotMapCustPo_Table.Rows.Count > 0)
                {
                    MyUtility.Msg.ShowMsgGrid(this.NotMapCustPo_Table, msg, "No found Cust PO#");
                }

                if (this.NotSetB03_Table.Rows.Count > 0)
                {
                    var m = MyUtility.Msg.ShowMsgGrid(this.NotSetB03_Table, msg, "No Set Packing_B03");
                    m.Width = 650;
                    m.grid1.Columns[0].Width = 200;
                    m.grid1.Columns[0].Width = 50;
                    m.grid1.Columns[0].Width = 100;
                    m.text_Find.Width = 150;
                    m.btn_Find.Location = new Point(170, 6);

                    m.btn_Find.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
                    //m.ShowDialog();
                }

                if (msg == "Data Not Mapping." + Environment.NewLine)
                {
                    MyUtility.Msg.InfoBox(msg);
                }

                #region 如果有Mapping失敗的，則到下一個視窗，若成功則直接修改DB

                if (notMapping)
                {
                    if (this.currentFileType != UploadType.PDF && isPackingLisID_CountError)
                    {
                        Sci.Production.Packing.P26_AssignPackingList form = new P26_AssignPackingList(this.MappingModels, (DataTable)this.listControlBindingSource1.DataSource, this.currentFileType.ToString());
                        form.ShowDialog();
                        this.canConvert = form.canConvert;
                    }
                    else
                    {
                        this.canConvert = false;
                    }
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                    foreach (var obj in this.MappingModels)
                    {
                        List<DataRow> dl = dt.AsEnumerable().Where(o => o["FileName"].ToString() == obj.FileName).ToList();
                        foreach (DataRow dr in dl)
                        {
                            dr["Result"] = "Fail";
                        }
                    }
                    //this.HideWaitMessage();
                }
                else
                {
                    // 如果全部Mapping成功，直接修改
                    this.ShowWaitMessage("Processing....");
                    DualResult result;

                    string updateCmd = string.Empty;
                    int i = 0;
                    int ii = 0;
                    int iii = 0;
                    List<string> fileNamess = new List<string>();

                    if (this.currentFileType == UploadType.PDF)
                    {
                        foreach (var item in this.MappingModel_PDFs)
                        {

                            string fileName = item.FileName;
                            string packingListID = item.PackingListID;
                            bool IsMixed = item.IsMixed;

                            fileNamess.Add(fileName);

                            MappingModel_PDF current = this.MappingModel_PDFs.Where(o => o.FileName == fileName).FirstOrDefault();

                            string cmd = string.Empty;
                            List<string> sqlMixed = new List<string>();

                            // 相同PackingListID Article SizeCode ShipQty，照順序寫入CustCTN、P24表頭 + 表身
                            ZPL ZPL = current.ZPL_Content;

                            if (this.currentFileType == UploadType.PDF)
                            {
                                if (IsMixed)
                                {

                                    foreach (var data in ZPL.Size_Qty_List)
                                    {
                                        sqlMixed.Add($@"
	( SizeCode='{data.Size}' OR SizeCode in(
		    SELECT SizeCode 
		    FROM Order_SizeSpec 
		    WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders{i}) AND SizeSpec IN ('{data.Size}')
	    )  
		AND pd.ShipQty={data.Qty})
");
                                    }

                                    cmd = $@"

SELECT ID ,StyleID ,POID
INTO #tmoOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'

----1. 整理Mapping的資料
SELECT CTNStartNo,[CartonCount]=COUNT(pd.Ukey)
INTO #tmpMappingCartonNo{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
AND pd.OrderID = (SELECT ID FROM #tmoOrders{i})
AND pd.CustCTN='' 
AND Article = '{ZPL.Article}'
AND (
";
                                    cmd += string.Join("		OR" + Environment.NewLine, sqlMixed);
                                    cmd += $@"
	)
GROUP BY CTNStartNo
HAVING COUNT(pd.Ukey)={ZPL.Size_Qty_List.Count}

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
AND p.ID='{packingListID}'
AND pd.OrderID = (SELECT ID FROM #tmoOrders{i})
AND pd.CustCTN='' 
AND Article = '{ZPL.Article}'
AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo{i}) 
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN，PackingList.EditDate和EditName
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.CTNStartNo=pd.CTNStartNo AND pd.ID=t.ID

UPDATE PackingList
SET EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
WHERE ID ='{packingListID}'

----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
INSERT INTO ShippingMarkPic
	([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

SELECT TOP 1 [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
FROM ShippingMarkPicture s
INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CTNRefno=t.RefNo --AND s.Side='D'
INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
ORDER BY ISNULL(s.EditDate,s.AddDate) DESC
END



INSERT INTO [dbo].[ShippingMarkPic_Detail]
        ([ShippingMarkPicUkey]
        ,[SCICtnNo]
        ,[FileName])
    VALUES
        ( ( SELECT  MAX(Ukey) FROM ShippingMarkPic WHERE PackingListID='{packingListID}' )
        ,(
			SELECT TOP 1 pd.SCICtnNo
			FROM PackingList_Detail pd
			INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
		)
        ,'{ZPL.CustCTN}' 
 		)

DROP TABLE #tmoOrders{i},#tmpMappingCartonNo{i},#tmp{i}

";
                                }
                                else
                                {

                                    cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmpOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
AND p.ID='{packingListID}'
AND pd.CustCTN='' 
AND pd.OrderID = (SELECT ID FROM #tmpOrders{i})
AND Article = '{ZPL.Article}'
AND pd.ShipQty={ZPL.ShipQty}
AND (
	    pd.SizeCode in
	    (
		    SELECT SizeCode 
		    FROM Order_SizeSpec 
		    WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders{i}) AND SizeSpec IN ('{ZPL.SizeCode}')
	    ) 
	    OR 
	    pd.SizeCode='{ZPL.SizeCode}'
    )
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN，PackingList.EditDate和EditName
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey

UPDATE PackingList
SET EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
WHERE ID ='{packingListID}'


----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
INSERT INTO ShippingMarkPic
	([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

SELECT TOP 1 [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
FROM ShippingMarkPicture s
INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CTNRefno=t.RefNo --AND s.Side='D'
INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
ORDER BY ISNULL(s.EditDate,s.AddDate) DESC
END


----PDF只會有一張貼紙
INSERT INTO [dbo].[ShippingMarkPic_Detail]
        ([ShippingMarkPicUkey]
        ,[SCICtnNo]
        ,[FileName])
    VALUES
        ( ( SELECT  MAX(Ukey) FROM ShippingMarkPic WHERE PackingListID='{packingListID}' )
        ,(
			SELECT TOP 1 pd.SCICtnNo
			FROM PackingList_Detail pd
			INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
		)
        ,'{ZPL.CustCTN}' 
 		)

DROP TABLE #tmpOrders{i},#tmp{i}
";
                                }
                            }

                            i++;

                            updateCmd += cmd + Environment.NewLine + "---------";
                        }
                    }

                    if (this.currentFileType == UploadType.ZPL)
                    {
                        foreach (var item in this.MappingModels)
                        {

                            string fileName = item.FileName;
                            string packingListID = item.PackingListID;
                            bool IsMixed = item.IsMixed;

                            fileNamess.Add(fileName);

                            MappingModel current = this.MappingModels.Where(o => o.FileName == fileName).FirstOrDefault();

                            string cmd = string.Empty;
                            List<string> sqlMixed = new List<string>();

                            // 相同PackingListID Article SizeCode ShipQty，照順序寫入CustCTN、P24表頭 + 表身
                            foreach (var ZPL in current.ZPL_Content)
                            {
                                if (this.currentFileType == UploadType.ZPL)
                                {

                                    if (IsMixed)
                                    {
                                        sqlMixed.Clear();

                                        cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmoOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'
";
                                        foreach (var data in ZPL.Size_Qty_List)
                                        {
                                            cmd += $@"

SELECT CTNStartNo,[CartonCount]=COUNT(pd.Ukey)
INTO #tmpCount{ii}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders0)
    AND pd.CustCTN='' 
    AND Article = '{ZPL.Article}'
	AND ( SizeCode='{data.Size}' OR SizeCode in(
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders0) AND SizeSpec IN ('{data.Size}')
	        )  )
	AND pd.ShipQty={data.Qty}
GROUP BY CTNStartNo
";
                                            ii++;
                                        }

                                        cmd += $@"
SELECT a.CTNStartNo,[CartonCount]=SUM(CartonCount) 
INTO #tmpMappingCartonNo{i}
FROM (
";
                                        foreach (var data in ZPL.Size_Qty_List)
                                        {
                                            sqlMixed.Add($@"
	SELECT  *
	FROM #tmpCount{iii}
");
                                            iii++;
                                        }

                                        cmd += string.Join(" UNION ALL" + Environment.NewLine, sqlMixed);
                                        cmd += $@"

)a
GROUP BY CTNStartNo";
                                        cmd += $@"

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B'
	AND p.ID='{packingListID}'
    AND pd.OrderID = (SELECT ID FROM #tmoOrders{i})
    AND pd.CustCTN='' 
    AND Article = '{ZPL.Article}'
    AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo{i}) 
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN，PackingList.EditDate和EditName
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.CTNStartNo=pd.CTNStartNo AND pd.ID=t.ID

UPDATE PackingList
SET EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
WHERE ID ='{packingListID}'

----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CTNRefno=t.RefNo AND s.Side='D'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
    ;
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CTNRefno=t.RefNo AND s.Side='A'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
END

----ShippingMarkPic_Detail  (Side=D)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
            ([ShippingMarkPicUkey]
            ,[SCICtnNo]
            ,[FileName])
        VALUES
            ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='D' )
            ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
            ,'{ZPL.CustCTN}' 
 			)


----ShippingMarkPic_Detail  (Side=A)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='A' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='A' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='A' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)


--DROP TABLE #tmoOrders{i},#tmpMappingCartonNo{i},#tmp{i}
----------------------------------------------------------------------------------------------------
";
                                    }
                                    else
                                    {

                                        cmd += $@"
----1. 整理Mapping的資料
SELECT ID ,StyleID ,POID
INTO #tmpOrders{i}
FROM Orders 
WHERE CustPONo='{ZPL.CustPONo}' AND StyleID='{ZPL.StyleID}'

SELECT TOP 1 pd.ID, pd.Ukey ,pd.CTNStartNo ,o.BrandID ,pd.RefNo
INTO #tmp{i}
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
	AND p.ID='{packingListID}'
    AND pd.CustCTN='' 
    AND pd.OrderID = (SELECT ID FROM #tmpOrders{i})
    AND Article = '{ZPL.Article}'
    AND pd.ShipQty={ZPL.ShipQty}
    AND (
	        pd.SizeCode in
	        (
		        SELECT SizeCode 
		        FROM Order_SizeSpec 
		        WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders{i}) AND SizeSpec IN ('{ZPL.SizeCode}')
	        ) 
	        OR 
	        pd.SizeCode='{ZPL.SizeCode}'
        )
ORDER BY CONVERT ( int ,pd.CTNStartNo)

----2. 更新PackingList_Detail的CustCTN，PackingList.EditDate和EditName
UPDATE pd
SET pd.CustCTN='{ZPL.CustCTN}'
FROM PackingList_Detail pd
INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey

UPDATE PackingList
SET EditDate=GETDATE(),EditName='{Sci.Env.User.UserID}'
WHERE ID ='{packingListID}'

----3. 寫入ShippingMarkPic、ShippingMarkPic_Detail資料
IF NOT EXISTS( SELECT 1 FROM ShippingMarkPic WHERE PackingListID='{packingListID}')
BEGIN
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CTNRefno=t.RefNo AND s.Side='D'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
    ;
	INSERT INTO ShippingMarkPic
		([PackingListID]           ,[Seq]           ,[Side]           ,[AddDate]           ,[AddName] )

	SELECT [PackingListID]=pd.id ,S.Seq ,S.Side ,[AddDate]=GETDATE() ,[AddName]='{Sci.Env.User.UserID}'	
	FROM ShippingMarkPicture s
	INNER JOIN #tmp{i} t ON s.BrandID=t.BrandID AND s.CTNRefno=t.RefNo AND s.Side='A'
	INNER JOIN PackingList_Detail pd ON t.Ukey=pd.Ukey 
    ;
END


----ShippingMarkPic_Detail  (Side=D)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='D' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='D' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)



----ShippingMarkPic_Detail  (Side=A)
IF EXISTS(
    SELECT 1 FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='A' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
)
BEGIN
    DELETE FROM ShippingMarkPic_Detail 
    WHERE ShippingMarkPicUkey=( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='A' )
    AND SCICtnNo = ( SELECT TOP 1 pd.SCICtnNo FROM PackingList_Detail pd INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey)
END

INSERT INTO [dbo].[ShippingMarkPic_Detail]
           ([ShippingMarkPicUkey]
           ,[SCICtnNo]
           ,[FileName])
     VALUES
           ( ( SELECT Ukey FROM ShippingMarkPic WHERE PackingListID='{packingListID}' AND Side='A' )
           ,(
				SELECT TOP 1 pd.SCICtnNo
				FROM PackingList_Detail pd
				INNER JOIN #tmp{i} t ON t.Ukey=pd.Ukey
			)
           ,'{ZPL.CustCTN}' 
 			)

DROP TABLE #tmpOrders{i},#tmp{i}
";
                                    }
                                }


                                i++;
                            }

                            updateCmd += cmd + Environment.NewLine + "---------";

                        }
                    }

                    if ((this.MappingModels.Count > 0 && this.currentFileType == UploadType.ZPL) || this.currentFileType == UploadType.PDF)
                    {

                        using (TransactionScope transactionscope = new TransactionScope())
                        {
                            if (!(result = DBProxy.Current.Execute(null, updateCmd.ToString())))
                            {
                                transactionscope.Dispose();
                                this.ShowErr(result);
                                return;
                            }

                            transactionscope.Complete();
                            transactionscope.Dispose();

                            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                            List<DataRow> dl = dt.AsEnumerable().Where(o => fileNamess.Contains(o["FileName"].ToString())).ToList();
                            foreach (DataRow dr in dl)
                            {
                                dr["Result"] = "Pass";
                            }
                        }
                        MyUtility.Msg.InfoBox("Data Mapping successful!");

                        this.canConvert = true;

                    }
                    this.HideWaitMessage();
                }

                #endregion

                string shippingMarkPath = MyUtility.GetValue.Lookup("select ShippingMarkPath from  System ");

                if (this.canConvert)
                {
                    // 然後開始轉圖片
                    this.ShowWaitMessage("Convert to Image....");

                    if (this.currentFileType == UploadType.ZPL)
                    {
                        foreach (string singleFileName in this.wattingForConvert)
                        {
                            string contentString = this.wattingForConvert_contentsOfZPL.Where(o => o.Contains(singleFileName)).FirstOrDefault();
                            if (contentString != string.Empty)
                            {
                                this.CallAPI(singleFileName, contentString, shippingMarkPath, contentString.ToUpper().Contains("MIXED"));
                            }
                        }
                    }

                    if (this.currentFileType == UploadType.PDF)
                    {
                        foreach (var item in this.File_Name_PDF)
                        {

                            string fileName = item.Key;
                            string custCTN = item.Value;

                            FileInfo file = new FileInfo(fileName);
                            PDDocument doc = PDDocument.load(file.FullName);
                            this.ConvertPDF2Image(fileName, shippingMarkPath, custCTN, 1, 5, ImageFormat.Jpeg, Definition.One);
                        }
                    }

                    this.HideWaitMessage();

                }
            }
            catch (Exception exp)
            {
                this.ShowErr(exp);
            }
        }

        private string PDF_Mapping(List<ZPL> packingist_Details, List<string> fileNames)
        {
            string msg = string.Empty;

            this.NotSetB03_Table.ColumnsStringAdd("BrandID");
            this.NotSetB03_Table.ColumnsStringAdd("RefNo");
            this.NotMapCustPo_Table.ColumnsStringAdd("CustPO");
            this.existsCustCTN_Table.ColumnsStringAdd("CustCTN");

            var chkCount = packingist_Details.Select(o => new
            {
                CustPONo = o.CustPONo,
                StyleID = o.StyleID,
                Article = o.Article
            }).Distinct().ToList();

            List<string> removePOs = new List<string>();
            List<string> removeFileNames = new List<string>();

            foreach (var item in chkCount)
            {
                string cmd = $@"
----統計PackingList的筆數，與分析出來的檔案數量至否一致
SELECT ID ,StyleID ,POID
INTO #tmpOrders
FROM Orders 
WHERE CustPONo='{item.CustPONo}' AND StyleID='{item.StyleID}'

SELECT COUNT(CTNStartNo)
FROM PackingList p 
INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
INNER JOIN Orders o ON o.ID = pd.OrderID
WHERE p.Type ='B' 
    AND pd.OrderID = (SELECT ID FROM #tmpOrders)
    --AND pd.CustCTN = ''
    AND Article = '{item.Article}'

DROP TABLE #tmpOrders 
";

                string packingListCount = MyUtility.GetValue.Lookup(cmd);

                string fileCount = packingist_Details.Where(o => o.CustPONo == item.CustPONo && o.StyleID == item.StyleID && o.Article == item.Article).Count().ToString();

                if (packingListCount != fileCount)
                {
                    // List<ZPL> removeList = packingist_Details.Where(o => o.CustPONo == item.CustPONo && o.StyleID == item.StyleID && o.Article == item.Article).ToList();
                    List<string> removePO = packingist_Details.Where(o => o.CustPONo == item.CustPONo && o.StyleID == item.StyleID && o.Article == item.Article).Select(o => o.CustPONo).Distinct().ToList();
                    List<string> tmpFileName = packingist_Details.Where(o => o.CustPONo == item.CustPONo && o.StyleID == item.StyleID && o.Article == item.Article).Select(o => o.FileName).Distinct().ToList();

                    removePOs.AddRange(removePO);
                    removeFileNames.AddRange(tmpFileName);
                }

            }

            if (removeFileNames.Count > 0 && removePOs.Count > 0)
            {

                MyUtility.Msg.InfoBox("The following PO# mapping failed, please check the import file!!" + Environment.NewLine + string.Join(",", removePOs));
                foreach (var removeFileName in removeFileNames)
                {
                    fileNames.Remove(removeFileName);
                    DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
                    List<DataRow> dl = dt.AsEnumerable().Where(o => removeFileNames.Contains(o["FileName"].ToString())).ToList();
                    foreach (DataRow dr in dl)
                    {
                        dr["Result"] = "Fail";
                    }
                }
            }

            if (fileNames.Count == 0)
            {
                return "The following PO# mapping failed, please check the import file!!" + Environment.NewLine + string.Join(",", removePOs);
            }

            for (int fileIndex = 0; fileIndex <= fileNames.Count - 1; fileIndex++)
            {
                DataTable[] tmpDts;
                string sqlCmd = string.Empty;
                string fileName = fileNames[fileIndex];

                // 取得這個檔案包含的PackingList Detail
                List<ZPL> zPLsByFileName = packingist_Details.Where(o => o.FileName == fileName).ToList();

                foreach (ZPL zPL in zPLsByFileName)
                {

                    // 確認一個ZPL檔，對應到幾個PackingList
                    #region SQL檢查對應到幾個PackingList

                    List<string> sqlMixed = new List<string>();
                    bool isMixed = zPL.SizeCode.ToUpper().Contains("MIX") || (MyUtility.Check.Empty(zPL.SizeCode) && zPL.Size_Qty_List != null);

                    if (isMixed)
                    {
                        sqlCmd = $@"

    SELECT ID ,StyleID ,POID
    INTO #tmoOrders
    FROM Orders 
    WHERE CustPONo='{zPL.CustPONo}' AND StyleID='{zPL.StyleID}'
    ";

                        int i = 0;
                        foreach (var data in zPL.Size_Qty_List)
                        {
                            sqlCmd += $@"

    SELECT  CTNStartNo,[CartonCount]=COUNT(pd.Ukey)
    INTO #tmpCount{i}
    FROM PackingList p 
    INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
    WHERE p.Type ='B'
        AND pd.OrderID = (SELECT ID FROM #tmoOrders)
        AND pd.CustCTN='' 
        AND Article = '{zPL.Article}'
	    AND ( SizeCode='{data.Size}' OR SizeCode in(
		            SELECT SizeCode 
		            FROM Order_SizeSpec 
		            WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmoOrders) AND SizeSpec IN ('{data.Size}')
	            ))  
	    AND pd.ShipQty={data.Qty}
    GROUP BY CTNStartNo

    ";
                            i++;
                        }

                        sqlCmd += $@"
    SELECT a.CTNStartNo,[CartonCount]=SUM(CartonCount) 
    INTO #tmpMappingCartonNo
    FROM (
    ";
                        i = 0;
                        foreach (var data in zPL.Size_Qty_List)
                        {
                            sqlMixed.Add($@"
	    SELECT  *
	    FROM #tmpCount{i}
    ");
                            i++;
                        }

                        sqlCmd += string.Join(" UNION ALL" + Environment.NewLine, sqlMixed);
                        sqlCmd += $@"

    )a
    GROUP BY CTNStartNo

    ----SQL檢查對應到幾個PackingList
    SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey
    FROM PackingList p 
    INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
    INNER JOIN Orders o ON o.ID = pd.OrderID
    WHERE p.Type ='B'
        AND pd.OrderID = (SELECT ID FROM #tmoOrders)
        AND pd.CustCTN='' 
        AND Article = '{zPL.Article}'
        AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo) 
	    AND pd.SCICtnNo <> ''

    ----ShippingMarkPicture
    SELECT DISTINCT o.BrandID ,pd.RefNo
    INTO #tmp
    FROM PackingList p 
    INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
    INNER JOIN Orders o ON o.ID = pd.OrderID
    WHERE p.Type ='B'
        AND pd.OrderID = (SELECT ID FROM #tmoOrders)
        AND pd.CustCTN='' 
        AND Article = '{zPL.Article}'
        AND pd.CTNStartNo IN (SELECT CTNStartNo FROM #tmpMappingCartonNo) 
	    AND pd.SCICtnNo <> ''



    SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
	INTO #ExistsB03
    FROM ShippingMarkPicture s
    INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo
    UNION
    SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
    FROM ShippingMarkPicture s
    INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo
	
	SELECT * FROM #ExistsB03

    SELECT * 
	FROM #tmp a
	WHERE NOT EXISTS (SELECT 1 FROM #ExistsB03 t WHERE a.BrandID=t.BrandID AND a.RefNo=t.RefNo )


    ";
                    }
                    else
                    {
                        sqlCmd = $@"


    SELECT ID ,StyleID ,POID
    INTO #tmpOrders
    FROM Orders 
    WHERE CustPONo='{zPL.CustPONo}' AND StyleID='{zPL.StyleID}'

    SELECT [PackingListID]=pd.ID ,[PackingList_Ukey]=pd.Ukey
    FROM PackingList p 
    INNER JOIN PackingList_Detail pd ON p.ID=pd.ID
    INNER JOIN Orders o ON o.ID = pd.OrderID
    WHERE p.Type ='B' 
        AND pd.OrderID = (SELECT ID FROM #tmpOrders)
        AND pd.CustCTN = ''
        AND Article = '{zPL.Article}'
        AND pd.ShipQty={zPL.ShipQty}
        AND (
	            pd.SizeCode in
	            (
		            SELECT SizeCode 
		            FROM Order_SizeSpec 
		            WHERE SizeItem='S01' AND ID IN (SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{zPL.SizeCode}')
	            ) 
	            OR 
	            pd.SizeCode='{zPL.SizeCode}'
            )
		
    SELECT DISTINCT o.BrandID ,pd.RefNo
    INTO #tmp
    FROM PackingList p
    INNER JOIN PackingList_Detail pd ON p.ID = pd.ID
    INNER JOIN Orders o ON o.ID = pd.OrderID
    WHERE p.Type = 'B'
    AND pd.OrderID = (SELECT ID FROM #tmpOrders)
    AND pd.CustCTN = ''
    AND Article = '{zPL.Article}'
    AND pd.ShipQty ={ zPL.ShipQty}
    AND(
        pd.SizeCode in
        (
            SELECT SizeCode

            FROM Order_SizeSpec

            WHERE SizeItem = 'S01' AND ID IN(SELECT POID FROM #tmpOrders) AND SizeSpec IN ('{zPL.SizeCode}')
            )

        OR

        pd.SizeCode = '{zPL.SizeCode}'
    )



    SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
	INTO #ExistsB03
    FROM ShippingMarkPicture s
    INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo
    UNION
    SELECT  t.BrandID ,t.RefNo , Side,IsSSCC
    FROM ShippingMarkPicture s
    INNER JOIN #tmp t ON s.BrandID=t.BrandID  AND s.CTNRefno=t.RefNo
	
	SELECT * FROM #ExistsB03

    SELECT * 
	FROM #tmp a
	WHERE NOT EXISTS (SELECT 1 FROM #ExistsB03 t WHERE a.BrandID=t.BrandID AND a.RefNo=t.RefNo )

    DROP TABLE #tmpOrders ,#tmp ,#ExistsB03
    ";
                    }
                    #endregion

                    DBProxy.Current.Select(null, sqlCmd, out tmpDts);
                    sqlMixed.Clear();

                    // 對應到DB的PackingList_Detail的資料筆數
                    int packingListDetail_Count = tmpDts[0].Rows.Count;
                    int brand_refno_Count = tmpDts[2].AsEnumerable().Count();

                    // CustCTN是否已經存在
                    bool existsCustCTN = MyUtility.Check.Seek($"SELECT 1 FROM PackingList_Detail WHERE CustCTN='{zPL.CustCTN}' ");

                    // ShippingMarkPicture是否有建立好 相同 BrandID CTNRefno Side 不同Seq IsSCC的兩筆資料
                    bool packingB03DataError = tmpDts[1] == null || tmpDts[1].Rows.Count == 0 ? true : false;

                    bool contuineCheck = true;

                    bool hasCustPONo = MyUtility.Check.Seek($@"   
    SELECT ID ,StyleID ,POID
    FROM Orders 
    WHERE CustPONo='{zPL.CustPONo}' AND StyleID='{zPL.StyleID}' ");

                    // 收集Mapping 失敗的資訊，依照1、2、3的順序檢查，若1沒檢查過，只顯示1的錯誤訊息；若1檢查過，2沒檢查過，只顯示2的錯誤訊息
                    if ((tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() > 1) ||
                        !hasCustPONo ||
                        existsCustCTN ||
                        packingB03DataError
                        )
                    {
                        #region 1.CustCTN已存在
                        if (existsCustCTN && !msg.Contains("CustCTN has existsed."))
                        {
                            msg += "CustCTN has existsed." + Environment.NewLine;
                        }

                        if (existsCustCTN)
                        {
                            contuineCheck = false;
                        }

                        if (existsCustCTN && !this.existsCustCTN_Table.AsEnumerable().Where(o => o["CustCTN"].ToString() == zPL.CustCTN).Any() && !contuineCheck)
                        {
                            DataRow ndr = this.existsCustCTN_Table.NewRow();
                            ndr["CustCTN"] = zPL.CustCTN;

                            this.existsCustCTN_Table.Rows.Add(ndr);
                        }

                        #endregion

                        #region 2.CustPO不符合

                        // 準備要跳出來的資料
                        if (!hasCustPONo && tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && contuineCheck && !msg.Contains("The following PO# can't be found in PPIC_P01!!"))
                        {
                            msg += "The following PO# can't be found in PPIC_P01!!" + Environment.NewLine;
                        }

                        if (!hasCustPONo && tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && contuineCheck)
                        {
                            contuineCheck = false;
                        }

                        // 準備要跳出來的資料
                        if (!hasCustPONo && tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && !this.NotMapCustPo_Table.AsEnumerable().Where(o => o["CustPo"].ToString() == zPL.CustPONo).Any() && (tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0 && !contuineCheck) )
                        {
                            DataRow ndr = this.NotMapCustPo_Table.NewRow();
                            ndr["CustPo"] = zPL.CustPONo;

                            this.NotMapCustPo_Table.Rows.Add(ndr);
                        }
                        #endregion

                        #region 3.Packing B03未設定

                        // 有Mapping到PacingList_Detail才需要提示P03的檢查
                        if (tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() > 0 && packingB03DataError && !msg.Contains("The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings.") && contuineCheck)
                        {
                            msg += "The following carton has not yet set carton sticker location. Please go to [Packing_B03] settings." + Environment.NewLine;
                        }

                        if (tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() > 0 && packingB03DataError && contuineCheck)
                        {
                            contuineCheck = false;
                        }

                        // 準備新開的視窗
                        if (tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() > 0 && packingB03DataError && brand_refno_Count > 0 && !this.NotSetB03_Table.AsEnumerable().Where(o => o["BrandID"].ToString() == tmpDts[2].Rows[0]["BrandID"].ToString() && o["RefNO"].ToString() == tmpDts[2].Rows[0]["RefNO"].ToString()).Any() && !contuineCheck)
                        {
                            DataRow ndr = this.NotSetB03_Table.NewRow();
                            ndr["BrandID"] = tmpDts[2].Rows[0]["BrandID"].ToString();
                            ndr["RefNO"] = tmpDts[2].Rows[0]["RefNO"].ToString();

                            this.NotSetB03_Table.Rows.Add(ndr);
                        }

                        #endregion

                        #region 4.其餘Not Mapping狀況
                        if ((tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0) && !msg.Contains("Data Not Mapping.") && contuineCheck)
                        {
                            msg += "Data Not Mapping." + Environment.NewLine;
                        }

                        if ((tmpDts[0].AsEnumerable().Select(o => o["PackingListID"]).Distinct().Count() == 0)  && contuineCheck)
                        {
                            contuineCheck = false;
                        }
                        #endregion

                    }
                    else
                    {
                        // PackingList_Detail的箱數夠
                        if (!this.MappingModel_PDFs.Where(o => o.FileName == fileName).Any())
                        {
                            MappingModel_PDF model = new MappingModel_PDF()
                            {
                                FileName = fileName,
                                ZPL_Content = zPL,
                                PackingListID = tmpDts[0].Rows[0]["PackingListID"].ToString(),
                                IsMixed = isMixed
                            };
                            this.MappingModel_PDFs.Add(model);
                        }
                    }
                }
            }


            return msg;
        }

        #region 類別定義

        public class ZipHelper
        {
            /// <summary>
            /// 將串入的ZPL檔案轉成Zip檔資料流存在記憶體
            /// </summary>
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

        public class NotSetB03
        {
            public string BrandID { get; set; }

            public string RefNo { get; set; }

        }

        public class SizeObject
        {
            public string Size { get; set; }

            public int Qty{ get; set; }
        }

        public class P25_Object
        {
            public string PackingList_Detail_CustCTN { get; set; }

            public string PackingList_Detail_ID { get; set; }

            public string PackingList_Detail_OrderId { get; set; }

            public string PackingList_Detail_CTNStartNo { get; set; }

            public string PackingList_Detail_SCICtnNo { get; set; }

            public string PackingList_Detail_Article { get; set; }

            public string PackingList_Detail_SizeCode { get; set; }
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

            public List<SizeObject> Size_Qty_List { get; set; }  //混尺碼用

            public string FileName { get; set; }
        }

        public class MappingModel
        {
            public string FileName { get; set; }

            public List<ZPL> ZPL_Content { get; set; }

            public string PackingListID { get; set; }

            public string PackingList_Ukey { get; set; }

            public bool IsMixed { get; set; }
        }

        public class MappingModel_PDF
        {
            public string FileName { get; set; }

            public ZPL ZPL_Content { get; set; }

            public string PackingListID { get; set; }

            public string PackingList_Ukey { get; set; }

            public bool IsMixed { get; set; }
        }


        public class File_Name_Object_List
        {

            public List<File_Name_Object2> File_Name_Object2s { get; set; }
        }

        public class File_Name_Object2
        {
            public string FileName { get; set; }

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
        private void CallAPI(string zplFileName, string zplContentString, string shippingMarkPath, bool IsMixed)
        {
            // 一份ZPL有3張圖片，因此再拆一次
            string[] stringSeparators = new string[] { "^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH0,0^LRN" };
            List<string> content = zplContentString.Split(stringSeparators, StringSplitOptions.None).ToList();

            for (int i = 0; i < content.Count; i++)
            {
                if (i == 1 && IsMixed)
                {
                    stringSeparators = new string[] { "^XA^MMT^XZ^XA^PRE^FS^FT0314,0058^A0N,0036,0036^FR^FDCarton Contents^FS" };
                    string[] aa = content[i].Split(stringSeparators, StringSplitOptions.None);
                    content[i] = aa[0];
                    content.Add("^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH0,0^LRN" + aa[1]);
                }
                else
                {
                    content[i] = "^XA^SZ2^JMA^MCY^PMN^PW786~JSN^JZY^LH0,0^LRN" + content[i];
                }

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
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseStream = response.GetResponseStream();
                    // 存入[System].ShippingMarkPath 路徑下
                    var fileStream = File.Create($@"{shippingMarkPath}\{zplFileName}_{(i + 1).ToString()}.bmp"); // 如果要PDF，把副檔名改成pdf

                    responseStream.CopyTo(fileStream);
                    responseStream.Close();
                    fileStream.Close();

                    this.InsertImageToDatabase($@"{shippingMarkPath}\{zplFileName}_{(i + 1).ToString()}.bmp", zplFileName, (i + 1).ToString());
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
            PDFWrapper pdfWrapper = new PDFWrapper();

            pdfWrapper.LoadPDF(pdfInputPath);
            pdfWrapper.ZoomIN();
            if (!System.IO.Directory.Exists(imageOutputPath))
            {
                Directory.CreateDirectory(imageOutputPath);
            }

            // validate pageNum
            if (startPageNum <= 0)
            {
                startPageNum = 1;
            }

            if (endPageNum > pdfWrapper.PageCount)
            {
                endPageNum = pdfWrapper.PageCount;
            }

            if (startPageNum > endPageNum)
            {
                int tempPageNum = startPageNum;
                startPageNum = endPageNum;
                endPageNum = startPageNum;
            }

            // start to convert each page
            for (int i = startPageNum; i <= endPageNum; i++)
            {
                string tmpIMG = imageOutputPath + imageName + "_tmp.bmp";
                pdfWrapper.ExportJpg(tmpIMG, i, i, 180, 80);//這裡可以設定輸出圖片的頁數、大小和圖片畫質
                //if (pdfWrapper.IsJpgBusy) { System.Threading.Thread.Sleep(500); }
                System.Threading.Thread.Sleep(500);
                Bitmap sourceImage = new Bitmap(tmpIMG);
                int picWidth = 759;
                int picHeight = 1207;

                Bitmap pic = new Bitmap(picWidth, picHeight);
                //建立圖片
                Graphics graphic = Graphics.FromImage(pic);
                //建立畫板

                graphic.DrawImage(sourceImage,
                         //將被切割的圖片畫在新圖片上面，第一個參數是被切割的原圖片
                         new Rectangle(0, 0, picWidth, picHeight),
                         //指定繪製影像的位置和大小，基本上是同pic大小
                         new Rectangle(33, 110, picWidth, picHeight),
                         //指定被切割的圖片要繪製的部分
                         GraphicsUnit.Pixel);
                pic.Save(imageOutputPath + imageName + ".bmp");
                graphic.Dispose();
                pic.Dispose();
                sourceImage.Dispose();
                //File.Delete(tmpIMG);
            }

            pdfWrapper.Dispose();

            this.InsertImageToDatabase(imageOutputPath + imageName + ".bmp", imageName, "1");
            this.InsertImageToDatabase(imageOutputPath + imageName + ".bmp", imageName, "2");
        }

        /// <summary>
        /// 寫入Image欄位
        /// </summary>
        /// <param name="path">圖片暫存路徑</param>
        /// <param name="FileName">寫入ShippingMarkPic_Detail.FileName的檔名</param>
        /// <param name="seq">ShippingMarkPic的Seq</param>
        private void InsertImageToDatabase(string path, string FileName, string seq)
        {
            byte[] data = null;

            FileInfo fInfo = new FileInfo(path);

            long length = fInfo.Length;

            FileStream fStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fStream);

            data = br.ReadBytes((int)length);

            string cmd = $@"
                    UPDATE sd
                    SET sd.Image=@Image
                    FROM ShippingMarkPic s
                    INNER JOIN ShippingMarkPic_Detail sd ON s.Ukey=sd.ShippingMarkPicUkey
                    WHERE sd.FileName=@FileName AND s.Seq=@Seq
                    ";
            List<SqlParameter> para = new List<SqlParameter>();
            para.Add(new SqlParameter("@FileName", FileName));
            para.Add(new SqlParameter("@Seq", seq));
            para.Add(new SqlParameter("@Image", (object)data));

            DualResult result = DBProxy.Current.Execute(null, cmd, para);

            if (!result)
            {
                this.ShowErr(result);
            }
        }

        private enum UploadType
        {
            ZPL,
            PDF
        }

        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }
        #endregion

    }
}
