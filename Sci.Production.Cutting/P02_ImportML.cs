using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    public partial class P02_ImportML : Sci.Win.Tems.QueryForm
    {
        /// <summary>
        /// L:Lectra；G:Gerber
        /// </summary>
        public string fileType;

        private class MarkerItemSet
        {
            /// <summary>
            /// 此隻是從 trade Sci.Sample.Pattern P03_ImportML 複製改來的，只有留下 L
            /// 這是用來區別，是否同版片，富有多尺碼資料
            /// ，如果FirstInsert = true，就代表是該版片的第一個尺碼，也就代表這一次是要Insert ML+Size+Qty
            /// ，否則，就只要Insert Size+Qty
            /// </summary>
            public bool FirstInsert { get; set; } = true;

            public string Version { get; set; }

            public string Fab_Type { get; set; }

            public string Fab_Width { get; set; }

            public string STYLE { get; set; }

            public string Fab_Length { get; set; }

            public string Efficiency { get; set; }

            public string Perimeter { get; set; }

            public string ActCuttingPerimeter { get; set; }

            public string Straight { get; set; }

            public string Curved { get; set; }

            public string TotalNum { get; set; }

            public string SizeCode { get; set; }

            public string Qty { get; set; }

            public string CurrentMarkerName { get; set; }

            public bool CheckItemComplete()
            {
                return new string[]
                {
                    this.Fab_Type,
                    this.Fab_Width,
                    this.Fab_Length,
                    this.Efficiency,
                    this.Perimeter,
                    this.Straight,
                    this.Curved,
                    this.TotalNum,
                    this.SizeCode,
                    this.Qty,
                    this.CurrentMarkerName,
                }
                .All(item => string.IsNullOrWhiteSpace(item) == false);
            }
        }

        /// <summary>
        /// 檢查匯入馬克檔size是否皆存在於對應Pattern SizeRange中的版片資料
        /// </summary>
        private class LatestMatchPattern
        {
            public string ID { get; set; }

            public string Version { get; set; }
        }

        private LatestMatchPattern pattern = new LatestMatchPattern();
        DataRow lastVerData;
        private string SizeRange = string.Empty;
        private string StyleUKey = string.Empty;
        private string MarkerNo = string.Empty;
        private string WorkOrderID = string.Empty;
        private DataTable WorkOrder;
        private Dictionary<string, string> SizeCodeCaches { get; set; }

        public P02_ImportML(string styleUKey, string id, DataRow drSMNotice, DataTable workOrder)
        {
            InitializeComponent();
            this.StyleUKey = styleUKey;
            this.WorkOrderID = id;
            this.lastVerData = drSMNotice;
            this.WorkOrder = workOrder;
            this.fileType = "L";
            this.MarkerNo = lastVerData["markerNo"].ToString();
        }

        /// <inheritdoc />
        protected override void OnFormLoaded()
        {
            // Grid setup
            this.Helper.Controls.Grid.Generator(this.ui_grid)
                .Text("FileName", header: "FileName", width: Widths.AnsiChars(45));

            var gridDataSource = new DataTable();
            gridDataSource.ColumnsStringAdd("FileName");
            gridDataSource.ColumnsStringAdd("FileType");
            this.ui_grid.DataSource = gridDataSource;

            using (var dr = DBProxy.Current.SelectEx("Select SizeCode, Seq From Style_SizeCode Where StyleUKey = @UKey and SizeGroup = @SizeGroup", "UKey", this.StyleUKey, "SizeGroup", lastVerData["SizeGroup"].ToString()))
            {
                if (dr == true)
                {
                    try
                    {
                        this.SizeCodeCaches = dr.ExtendedData.AsEnumerable().ToDictionary(row => ((string)row["sizecode"]).ToUpper(), row => (string)row["seq"]);
                    }
                    catch (Exception ex)
                    {
                        MyUtility.Msg.ErrorBox(ex.ToString(), "Sizecode have duplicate seq.");
                    }
                }
                else
                {
                    MyUtility.Msg.ErrorBox(dr.ToString(), "load sizecode error.");
                }
            }
        }

        private void Ui_btnImportFiles_Click(object sender, EventArgs e)
        {
            var type = this.fileType;
            string filter;

            filter = $"DAT Files({lastVerData["PatternNo"]}*.WRI)|*.WRI|All files (*.*)|*.*";

            string[] fileNames = null;
            SciFileDialog.ShowDialog(
                afterSelected: dlg => fileNames = dlg.FileNames,
                moreSetting: dlg =>
                {
                    dlg.Multiselect = true;
                    dlg.Filter = filter;
                    dlg.FilterIndex = 1;
                    dlg.CheckFileExists = true;
                });
            if (fileNames == null)
            {
                return;
            }

            var dt = (System.Data.DataTable)this.ui_grid.DataSource;
            fileNames
                .ToList()
                .Where(w => !dt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["fileName"])).ToList().Contains(w)) // 同路徑同檔案不重複加入
                .ToList()
                .ForEach(fileName => dt.Rows.Add(new object[] { fileName, type }));
        }

        private void Ui_btnDelete_Click(object sender, EventArgs e)
        {
            if (this.ui_grid.SelectedRows.Count > 0)
            {
                this.ui_grid.Rows.Remove(this.ui_grid.SelectedRows[0]);
            }
        }

        private void Ui_btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void Ui_btnConfirm_Click(object sender, EventArgs e)
        {
            if (!((DataTable)this.ui_grid.DataSource).AsEnumerable().Any())
            {
                return;
            }

            if (this.DoImpot() == false)
            {
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private bool DoImpot()
        {
            var inserts_marker_ML = new List<DataRow>();
            var inserts_marker_SizeCode = new List<DataRow>();
            var inserts_marker_qty = new List<DataRow>();
            for (int i = 0; i < this.ui_grid.Rows.Count; i++)
            {
                var fileName = this.ui_grid.GetDataRow<DataRow>(i)["FileName"].ToString();
                var type = this.ui_grid.GetDataRow<DataRow>(i)["FileType"].ToString();
                if (type == "L")
                {
                    if (this.SingleFileProcess(fileName, inserts_marker_ML, inserts_marker_SizeCode, inserts_marker_qty) == false)
                    {
                        return false;
                    }
                }
            }

            if (inserts_marker_ML.Count > 0)
            {
                using (var transactionScope = new TransactionScope())
                using (var helper = new AutoUpdator())
                {
                    foreach (DataRow currentRow_marker_ml in inserts_marker_ML)
                    {
                        string inch = currentRow_marker_ml["MarkerLength"].ToString().ToUpper();
                        if (!VFP.Empty(inch) && !inch.Contains("YD"))
                        {
                            var tmpLength = Convert.ToDecimal(inch.Trim('Y'));
                            var tmpYard = Math.Floor(tmpLength);
                            var tmpDecimal = (tmpLength - tmpYard) * 36m;
                            var tmpInt = Math.Floor(tmpDecimal);

                            inch = $"{tmpYard}YD{tmpInt.ToString().PadLeft(2, '0')}\"00";
                        }

                        var pattern = @"^((?<gy>\d*)YD){0,1}((?<gd>\d{2})\"")((?<gp>\d{2})){0,1}$";
                        var ms = System.Text.RegularExpressions.Regex.Match(inch, pattern);
                        if (ms.Success)
                        {
                            var groupYard = ms.Groups["gy"];
                            var groupInt = ms.Groups["gd"];
                            var groupPer = ms.Groups["gp"];
                            var valueYard = Convert.ToDecimal(groupYard.Value ?? "0");
                            var valueInt = Convert.ToDecimal(groupInt.Value ?? "0");
                            var valuePer = Convert.ToDecimal(groupPer.Success ? groupPer.Value ?? "0" : "0");

                            // 最小分母為1/16 但匯入要用1/8進位
                            var valuePer2 = Convert.ToInt32(Math.Ceiling(valuePer / 32m / 0.0625m / 2m) * 2);

                            var tmpInch = currentRow_marker_ml["MarkerLength"].ToString().ToUpper();
                            if (!VFP.Empty(inch) && !tmpInch.Contains("YD"))
                            {
                                var tmpLength = Convert.ToDecimal(tmpInch.Trim('Y'));
                                var tmpYard = Math.Floor(tmpLength);
                                var tmpDecimal = (tmpLength - tmpYard) * 36m;
                                var tmpInt = Math.Floor(tmpDecimal);
                                valuePer2 = Convert.ToInt32(Math.Ceiling((tmpDecimal - tmpInt) * 16));

                                if (valuePer2 % 2 != 0)
                                {
                                    valuePer2++;
                                }
                            }

                            var inchValue = new Inch() { Piece = valuePer2 };

                            if (valuePer2 == 0)
                            {
                                currentRow_marker_ml["MarkerLength"] = string.Format("{0}Yd{1:00}\"", valueYard, valueInt);
                            }
                            else if (valuePer2 == 16)
                            {
                                valuePer2 = 0;
                                valueInt += 1;
                                inchValue = new Inch() { Piece = valuePer2 };
                                currentRow_marker_ml["MarkerLength"] = string.Format("{0}Yd{1}\"", valueYard, valueInt);
                            }
                            else
                            {
                                var inchText = inchValue.ToInchText();
                                currentRow_marker_ml["MarkerLength"] = string.Format("{0}Yd{1:00}{2}", valueYard, valueInt, inchText);
                            }

                            var markerLength = (valueYard * 36M) + valueInt + inchValue.ToInchDecimal();

                            decimal vRepeat = VFP.Empty(currentRow_marker_ml["V_Repeat"]) ? 0M : System.Convert.ToDecimal(currentRow_marker_ml["V_Repeat"].ToString()); // V_Repeat
                            decimal qty = 0M;
                            foreach (DataRow currentRow_qty in inserts_marker_qty)
                            {
                                if (currentRow_qty["markerName"].ToString().Equals(currentRow_marker_ml["markerName"].ToString()))
                                {
                                    qty += System.Convert.ToDecimal(string.Empty + currentRow_qty["qty"]);
                                }
                            }

                            if (qty == 0m)
                            {
                                continue;
                            }

                            // 此處 Production 的 ConsPC 只有數值， 確認 trade 那邊的轉換 ConsPC 是去掉yd

                            //MatchFabric = 0 or 4
                            string matchFabric = currentRow_marker_ml["MatchFabric"].ToString();
                            if (matchFabric.Equals("0") || matchFabric.Equals("4"))
                            {
                                if (!qty.EqualDecimal(0))
                                {
                                    currentRow_marker_ml["ConsPC"] = MyUtility.Math.Round(MyUtility.Math.Round((markerLength + 1M) / qty, 8) / 36M, 4);// + "yd";
                                }
                            }
                            else if (matchFabric.Equals("1"))
                            {
                                if (!vRepeat.EqualDecimal(0) && !qty.EqualDecimal(0))
                                {
                                    currentRow_marker_ml["ConsPC"] = MyUtility.Math.Round(MyUtility.Math.Round(Math.Ceiling(markerLength / vRepeat) * vRepeat / qty, 8) / 36M, 4);// + "yd";
                                }
                            }
                            else if (matchFabric.Equals("2") || matchFabric.Equals("3"))
                            {
                                if (!vRepeat.EqualDecimal(0) && !qty.EqualDecimal(0))
                                {
                                    currentRow_marker_ml["ConsPC"] = MyUtility.Math.Round(MyUtility.Math.Round(Math.Ceiling((markerLength + 1M) / vRepeat) * vRepeat / qty, 8) / 36M, 4);// + "yd";
                                }
                            }
                        }
                    }

                    int maxKey = MyUtility.Convert.GetInt(this.WorkOrder.Compute("Max(newkey)", ""));
                    var mergeInTable = inserts_marker_ML.CopyToDataTable();

                    #region 判斷是否已有重複資料，[Fabric Combo]+[Fab_Panel Code]+[Marker No]+[Marker Name]
                    mergeInTable.Columns.Add("continue", typeof(bool));
                    string msgQ = string.Empty;
                    mergeInTable.Select().ToList().ForEach(row =>
                    {
                        row["continue"] = true;
                        if (this.WorkOrder.Select($@"Fabriccombo='{row["Fabriccombo"]}' and FabricPanelCode ='{row["FabricPanelCode"]}' and MarkerNo = '{this.MarkerNo}' and MarkerName = '{row["MarkerName"]}'").Any())
                        {
                            row["continue"] = false;
                            msgQ += "\r\n" + $"Fabriccombo:{row["Fabriccombo"]}, FabricPanelCode:{row["FabricPanelCode"]}, MarkerNo:{this.MarkerNo}, MarkerName:{row["MarkerName"]}";
                        }
                    });

                    bool continueb = true;
                    if (!MyUtility.Check.Empty(msgQ))
                    {
                        continueb = MyUtility.Msg.QuestionBox("The following workorder data duplicated. Do you want to continue?\r\n" + msgQ) == DialogResult.Yes;
                    }
                    #endregion

                    mergeInTable.AsEnumerable().Where(w => continueb || MyUtility.Convert.GetBool(w["continue"])).ToList()
                        .ForEach(row =>
                        {
                            row["Ukey"] = 0;
                            row["NewKey"] = maxKey++;
                            row["Type"] = this.WorkOrder.AsEnumerable().Select(s => s["Type"]).FirstOrDefault();
                            row["MDivisionId"] = Sci.Env.User.Keyword;
                            row["FactoryID"] = this.WorkOrder.AsEnumerable().Select(s => s["FactoryID"]).FirstOrDefault();
                            row["OrderID"] = this.WorkOrderID; // 此處不用管 Type，因為用檔案匯入沒有dist
                            row["MarkerNo"] = this.MarkerNo;
                            row["EachconsMarkerNo"] = this.MarkerNo;
                            row["EachconsMarkerVersion"] = lastVerData["Version"];
                            row["Cutno"] = DBNull.Value;
                            row["isbyAdditionalRevisedMarker"] = 0;
                            row["ImportML"] = true;
                            row["MarkerLength"] = Prgs.MarkerLengthSampleTOTrade(row["MarkerLength"].ToString(), row["MatchFabric"].ToString());
                            row["ConsPC"] = MyUtility.Check.Empty(row["ConsPC"]) ? 0 : row["ConsPC"];
                            P02.ProcessColumns(row);
                            row.AcceptChanges();
                            row.SetAdded();
                            this.WorkOrder.ImportRow(row);
                        });
                }
            }

            return true;
        }

        /// <summary>
        /// 單一檔案處理
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="inserts_marker_ML">inserts_marker_ML</param>
        /// <param name="inserts_marker_SizeCode">inserts_marker_SizeCode</param>
        /// <param name="inserts_marker_qty">inserts_marker_qty</param>
        /// <returns>bool</returns>
        private bool SingleFileProcess(string fileName, List<DataRow> inserts_marker_ML, List<DataRow> inserts_marker_SizeCode, List<DataRow> inserts_marker_qty)
        {
            /*******************************************************************
            * 參數說明:
            *   mFile: 立克系統匯出的複雜版檔名
            * 回傳值: .T. = 成功 .F. = 失敗
            *
            * 功能說明:
            *   1.可處理"中文版"&"英文版"
            *   2.若資料處理成功會產生一暫存檔 tMKList
            *       MKNO C(20)          版號
            *       MKName C(20)        馬克名
            *       Fab_Type C(10)      布料種類
            *       Fab_Width C(15)     寬度
            *       Fab_Length C(15)    長度
            *       Efficiency C(10)    使用率
            *       Perimeter C(15)     周長
            *       Straight C(15)      直線
            *       Curved C(15)        曲線
            *       ActCuttingPerimeter C(15)       實際裁割長度
            *       TotalNum C(10)      裁片總數
            *       Variants C(240)     數量配比- 格式為 固定長度 <尺碼(8)><數量(4)><尺碼(8)><數量(4)>..... (右靠)
            *******************************************************************/
            if (VFP.Empty(fileName))
            {
                return false;
            }

            if (fileName.IndexOf(this.MarkerNo) < 0)
            {
                MyUtility.Msg.InfoBox("File doesn't belong to this Marker!!");
                return false;
            }

            bool chkfileDo = true;

            var allError = new List<string>();
            using (StreamReader reader = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                string mTmp;
                string mCrLf = string.Empty + (char)13 + (char)10;            // && Chr(13)+Chr(10) -> Chr(10)

                var markerItemSet = new MarkerItemSet();
                string mKeyword = string.Empty;
                var linesAtThisItemSet = new List<string>();
                while ((mTmp = reader.ReadLine()) != null)
                {
                    linesAtThisItemSet.Add(mTmp);
                    if (VFP.Empty(mTmp.Trim()) || mTmp.IndexOf("====") > 1)
                    {
                        continue;
                    }

                    while (mTmp.IndexOf("  ") > 1)
                    {
                        mTmp = mTmp.Replace("  ", " ");
                    }

                    while (mTmp.IndexOf(" :") > 1)
                    {
                        mTmp = mTmp.Replace(" :", ":");
                    }

                    while (mTmp.IndexOf(": ") > 1)
                    {
                        mTmp = mTmp.Replace(": ", ":");
                    }

                    while (mTmp.IndexOf(" /") > 1)
                    {
                        mTmp = mTmp.Replace(" /", "/");
                    }

                    while (mTmp.IndexOf("/ ") > 1)
                    {
                        mTmp = mTmp.Replace("/ ", "/");
                    }

                    while (mTmp.IndexOf(mCrLf + mCrLf) > 1)
                    {
                        mTmp = mTmp.Replace(mCrLf + mCrLf, mCrLf);
                    }

                    if (mTmp.LastIndexOf(".PLX---") > -1)
                    {
                        markerItemSet = new MarkerItemSet();
                        linesAtThisItemSet = new List<string>();
                        mTmp = mTmp.Substring(0, mTmp.LastIndexOf(".PLX---"));
                        var plxName = mTmp.Split("\\".ToCharArray()).LastOrDefault();
                        if (plxName.IndexOf(this.MarkerNo) < 0)
                        {
                            MyUtility.Msg.InfoBox("File doesn't belong to this Marker!!");
                            return false;
                        }

                        var markerNo = this.MarkerNo;
                        var rawMarkerName = mTmp.Substring(mTmp.IndexOf(markerNo) + markerNo.Length, mTmp.Length - mTmp.IndexOf(markerNo) - markerNo.Length);
                        var refinedMarkerName = rawMarkerName;
                        if (rawMarkerName.Length > 10)
                        {
                            refinedMarkerName = rawMarkerName.Substring(0, 5);
                            refinedMarkerName += rawMarkerName.Substring(rawMarkerName.Length - 5);
                        }

                        markerItemSet.CurrentMarkerName = refinedMarkerName;
                    }
                    else if (string.IsNullOrWhiteSpace(markerItemSet.Version))
                    {
                        if (mTmp.IndexOf("代號") > -1)
                        {
                            markerItemSet.Version = "1";
                        }
                        else if (mTmp.IndexOf("Code") > -1)
                        {
                            markerItemSet.Version = "2";
                        }

                        mKeyword = markerItemSet.Version == "1" ? "代號:" : "Code:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.Fab_Type = this.GetValue(mKeyword, mTmp);
                        }
                    }
                    else if (mTmp.IndexOf("種類") > -1 || mTmp.IndexOf("Type") > -1 || mTmp.IndexOf("呈現方式") > -1 || mTmp.IndexOf("整體") > -1 || mTmp.IndexOf("移動容許量") > -1)
                    {
                        continue;
                    }
                    else
                    {
                        mKeyword = markerItemSet.Version == "1" ? "註解:" : "Style:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.STYLE = this.GetValue(mKeyword, mTmp);
                        }

                        mKeyword = markerItemSet.Version == "1" ? "寬度:" : "Width:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.Fab_Width = this.GetValue(mKeyword, mTmp);
                        }

                        mKeyword = markerItemSet.Version == "1" ? "長度:" : "Length:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.Fab_Length = this.GetValue(mKeyword, mTmp);
                        }

                        mKeyword = markerItemSet.Version == "1" ? "使用率:" : "Efficiency:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.Efficiency = this.GetValue(mKeyword, mTmp);
                        }

                        mKeyword = markerItemSet.Version == "1" ? "實際裁割周長:" : "Real cut perimeter:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.ActCuttingPerimeter = this.GetValue(mKeyword, mTmp);
                        }
                        else
                        {
                            mKeyword = markerItemSet.Version == "1" ? "周長:" : "Perimeter:";
                            if (mTmp.IndexOf(mKeyword) >= 0)
                            {
                                markerItemSet.Perimeter = this.GetValue(mKeyword, mTmp);
                            }
                        }

                        mKeyword = markerItemSet.Version == "1" ? "直線:" : "Straight:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.Straight = this.GetValue(mKeyword, mTmp);
                        }

                        mKeyword = markerItemSet.Version == "1" ? "曲線:" : "Curved:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.Curved = this.GetValue(mKeyword, mTmp);
                        }

                        mKeyword = markerItemSet.Version == "1" ? "/總數:" : "/total number:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.TotalNum = this.GetValue(mKeyword, mTmp);
                            if (string.IsNullOrWhiteSpace(markerItemSet.TotalNum) == false)
                            {
                                markerItemSet.TotalNum = markerItemSet.TotalNum.Substring(markerItemSet.TotalNum.IndexOf("/") + 1, markerItemSet.TotalNum.Length - markerItemSet.TotalNum.IndexOf("/") - 1);
                            }
                        }

                        mKeyword = markerItemSet.Version == "1" ? "尺碼:" : "Size:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.SizeCode = this.GetValue(mKeyword, mTmp);
                        }

                        mKeyword = markerItemSet.Version == "1" ? "數量:" : "Repet.:";
                        if (mTmp.IndexOf(mKeyword) >= 0)
                        {
                            markerItemSet.Qty = (Convert.ToInt32(markerItemSet.Qty) + Convert.ToInt32(this.GetValue(mKeyword, mTmp))).ToString();
                        }

                        if (markerItemSet.CheckItemComplete() == true)
                        {
                            System.Diagnostics.Debug.WriteLine(linesAtThisItemSet.JoinToString("\r\n"));
                            this.SingleMarkerItem(inserts_marker_ML, inserts_marker_SizeCode, inserts_marker_qty, markerItemSet, allError);
                            markerItemSet.SizeCode = null; // 將這個欄位值清空，回到UnComplete的狀態，準備讓也許下一行直接又是尺碼接數量
                            markerItemSet.Qty = null;
                        }
                    }
                }

                if (markerItemSet.CheckItemComplete() == true)
                {
                    System.Diagnostics.Debug.WriteLine(linesAtThisItemSet.JoinToString("\r\n"));
                    this.SingleMarkerItem(inserts_marker_ML, inserts_marker_SizeCode, inserts_marker_qty, markerItemSet, allError);

                    markerItemSet.SizeCode = null; // 將這個欄位值清空，準備讓也許下一行直接又是尺碼接數量
                    markerItemSet.Qty = null;
                }
            }

            if (allError.Count != 0)
            {
                MyUtility.Msg.WarningBox(allError.JoinToString("\r\n"));
            }

            if (chkfileDo == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool SingleMarkerItem(List<DataRow> inserts_marker_ML, List<DataRow> inserts_marker_SizeCode, List<DataRow> inserts_marker_qty, MarkerItemSet item, List<string> allError)
        {
            DataTable TableSchema_WorkOrder = this.WorkOrder.Clone();
            TableSchema_WorkOrder.Columns.Add("MatchFabric", typeof(string));
            TableSchema_WorkOrder.Columns.Add("V_Repeat", typeof(decimal));
            var rowML = TableSchema_WorkOrder.NewRow();
            if (item.FirstInsert == true)
            {
                #region ML
                {
                    // 10.當匯入的MarkerName =’K’開頭,就沒有 FabricPanelCode ,則無對應的Pattern Panel , Fabric Code , 直接匯入
                    // 11.當　MarkerName <> ‘K’ 開頭，就利用FabricPanelCode至Style_BOF中帶出最新的MatchFabric, V_Repeat , H_Repeat , HorizontalCutting, OneTwoWay , Pattern Panel , Fabric Code , SCIRefno

                    rowML["MarkerName"] = item.CurrentMarkerName;
                    rowML["ID"] = this.WorkOrderID;
                    rowML["MarkerVersion"] = lastVerData["Version"];
                    rowML["FabricPanelCode"] = item.Fab_Type;
                    rowML["StraightLength"] = item.Straight;
                    rowML["CurvedLength"] = item.Curved;
                    rowML["ActCuttingPerimeter"] = string.IsNullOrEmpty(item.ActCuttingPerimeter) ? item.Perimeter : item.ActCuttingPerimeter;
                    var isItemK = item.CurrentMarkerName.ToUpper().StartsWith("K");
                    if (isItemK)
                    {
                    }
                    else
                    {
                        rowML["MarkerLength"] = item.Fab_Length;

                        #region non-K item
                        var isNewData = item.Fab_Type[0].IsOneOfThe("0123456789".ToArray());
                        var fabricPanelCode = isNewData ? item.Fab_Type.Split(':')[1] : item.Fab_Type.Substring(0, 1);
                        var sql = @"
Select 
	  fc.PatternPanel
	, fc.FabricCode
	, bof.SCIRefno
	, bof.MatchFabric
	, GetVRepeat.Value as VRepeat
	, bof.HRepeat
	, bof.HorizontalCutting
	, f.Width
	, bof.Refno
From Style_BOF bof
Inner Join Style_FabricCode fc on fc.StyleUkey = bof.StyleUKey and fc.FabricCode = bof.FabricCode and fc.FabricPanelCode = @FabricPanelCode
Left Join Fabric f on f.SciRefno = bof.SciRefno
Outer APPLY(Select iif(UseFor = 'B',bof.VRepeat,bof.VRepeat_C) as value 
			From SMNotice_Detail 
			Where Type = 'M' 
			and SMNotice_Detail.ID = @ID) as GetVRepeat
Where bof.StyleUkey = @StyleUkey";
                        using (var dr = DBProxy.Current.SelectEx(
                            sql,
                            "StyleUkey",
                            this.StyleUKey,
                            "FabricPanelCode",
                            fabricPanelCode,
                            "ID",
                            lastVerData["ID"].ToString()))
                        {
                            if (dr == false)
                            {
                                this.ShowErr(dr.InnerResult);
                                return false;
                            }

                            var datatable = dr.ExtendedData;
                            if (datatable != null && datatable.Rows.Count > 0)
                            {
                                var row = datatable.Rows[0];
                                rowML["PatternPanel"] = row["PatternPanel"];
                                rowML["FabricCombo"] = row["PatternPanel"];
                                rowML["FabricCode"] = row["FabricCode"];
                                rowML["SCIRefno"] = row["SCIRefno"];
                                rowML["Refno"] = row["Refno"];
                                rowML["MatchFabric"] = row["MatchFabric"];
                                rowML["V_Repeat"] = row["VRepeat"];
                            }
                            else
                            {
                                allError.Add(string.Format("{0} BOF沒有該項資料!!", item.CurrentMarkerName));
                                rowML = null;
                            }
                        }

                        #endregion
                    }

                    if (rowML != null)
                    {
                        inserts_marker_ML.Add(rowML);
                    }

                    item.FirstInsert = false;
                }
                #endregion
            }

            #region SizeCode

            List<SqlParameter> listPar = new List<SqlParameter>()
            {
               new SqlParameter("@UKey", this.StyleUKey),
               new SqlParameter("@sizeCodev", item.SizeCode),
            };
            string sqlcmd = $@"Select 1 From Style_SizeCode Where StyleUkey = {this.StyleUKey} And SizeCode = '{item.SizeCode}'";
            // 匯入的Size必須存在於Style – SizeSpec之中
            if (!MyUtility.Check.Seek(sqlcmd))
            {
                MyUtility.Msg.InfoBox("SizeCode:" + item.SizeCode + ", doesn't belong to this SizeSpec.");
                return false;
            }

            if (!this.SizeRange.Trim().Equals(string.Empty) && this.SizeRange.IndexOf(item.SizeCode) < 0)
            {
                MyUtility.Msg.InfoBox($"Compare with Pattern:{this.pattern.ID}-{this.pattern.Version}, sizeCode doesn't belong to SizeRange.");
                return false;
            }

            // 數量不為空,就個別sizecode 加總數量
            if (!VFP.Empty(item.Qty))
            {
                bool notExist = true;
                for (int i = 0; i < inserts_marker_qty.Count; i++)
                {
                    DataRow currentRow = inserts_marker_qty[i];
                    if (currentRow["sizecode"].Equals(item.SizeCode) && currentRow["markerName"].Equals(item.CurrentMarkerName))
                    {
                        // sizecode 相同 ,加總
                        notExist = false;
                        currentRow["qty"] = string.Empty + (int.Parse(currentRow["qty"].ToString()) + int.Parse(item.Qty));
                    }
                }

                if (notExist)
                {
                    DataTable Marker_Qty = new DataTable();
                    Marker_Qty.Columns.Add("MarkerName", typeof(string));
                    Marker_Qty.Columns.Add("sizecode", typeof(string));
                    Marker_Qty.Columns.Add("qty", typeof(decimal));
                    var marker_qty_datarow = Marker_Qty.NewRow();
                    marker_qty_datarow["MarkerName"] = item.CurrentMarkerName;
                    marker_qty_datarow["sizeCode"] = item.SizeCode;
                    marker_qty_datarow["qty"] = item.Qty;
                    inserts_marker_qty.Add(marker_qty_datarow);
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// 取得指定欄位的值
        /// </summary>
        /// <param name="mKeyWord">查詢的 - "關鍵字"</param>
        /// <param name="mDesc">被查詢的內文</param>
        /// <returns>根據"關鍵字"傳回後面所接的文字，空白 或 Chr(13)+Chr(10) 為截止點</returns>
        private string GetValue(string mKeyWord, string mDesc)
        {
            int mAT;
            string mTmp1;
            string mTmp2;
            if (mDesc.IndexOf(mKeyWord) > -1)
            {
                mAT = mDesc.IndexOf(mKeyWord);
                mTmp1 = mDesc.Substring(mAT + mKeyWord.Length, mDesc.Length - mKeyWord.Length - mAT);
                mAT = mTmp1.IndexOf(" ");

                if (mAT > 0)
                {
                    mTmp2 = mTmp1.Substring(0, mAT);
                }
                else
                {
                    mTmp2 = mTmp1;
                }

                // sepcial case
                if (mKeyWord.Equals("直線:") && mTmp2.IndexOf("曲線:") > -1)
                {
                    mTmp2 = mTmp2.Substring(0, mTmp2.IndexOf("曲線:"));
                }
                else if (mKeyWord.Equals("Straight:") && mTmp2.IndexOf("Curved:") > -1)
                {
                    mTmp2 = mTmp2.Substring(0, mTmp2.IndexOf("Curved:"));
                }

                return mTmp2;
            }
            else
            {
                return "No found";
            }
        }
    }
}
