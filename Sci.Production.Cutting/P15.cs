using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Automation.Guozi_AGV;
using static Sci.Production.Cutting.BatchCreateData;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P15 : Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public static DataTable SpreadingDT { get; set; }

        /// <inheritdoc/>
        public static string SpreadingType { get; set; }

        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private string tone;
        private bool numNoOfBundle_Validating = true;
        private bool isCombineSubProcess;
        private bool isNoneShellNoCreateAllParts;
        private DataTable CutRefTb;
        private DataTable qtyTb;
        private DataTable ArticleSizeTb;
        private DataTable ArticleSizeTbOri; // 此 Table 在 Query 之後不再變更, 只用來開窗選擇用
        private DataTable ExcessTb;
        private DataTable GarmentTb;
        private DataTable patternTb;
        private DataTable patternTbOri; // 此 Table 在 Query 之後不再變更
        private DataTable allpartTb;
        private DataTable allpartTbOri; // 此 Table 在 Query 之後不再變更
        private DataTable artTb;
        private DataTable SizeRatioTb;
        private DataTable fsDt;
        private DataTable faDt;
        private DataTable fcDt;

        /// <inheritdoc/>
        public P15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string cmd_st = "Select 0 as sel,PatternCode,PatternDesc, '' as annotation,Parts,'' as cutref,'' as poid, ukey = cast(0 as bigint),ispair ,Location,isMain = cast(0 as bit),CombineSubprocessGroup = cast(0 as tinyint) from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out this.allpartTb);

            string pattern_cmd = "Select PatternCode,PatternDesc,Parts,'' as art, '' as cutref,'' as poid, ukey = cast(0 as bigint), ispair ,Location,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='',isMain = cast(0 as bit),CombineSubprocessGroup = cast(0 as tinyint) from Bundle_Detail WITH (NOLOCK) Where 1=0"; // 左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out this.patternTb);

            string cmd_art = "Select PatternCode,subprocessid,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_detail_art WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_art, out this.artTb);

            this.GridSetup();
        }

        private void GridSetup()
        {
            #region 左上 gridCutRef 事件
            DataGridViewGeneratorCheckBoxColumnSettings chcutref = new DataGridViewGeneratorCheckBoxColumnSettings();
            chcutref.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridCutRef.GetDataRow(e.RowIndex);
                if (this.ArticleSizeTb == null || (bool)e.FormattedValue == (bool)dr["sel"])
                {
                    return;
                }

                DataRow[] articleAry = this.ArticleSizeTb.Select($"Ukey ='{dr["Ukey"]}'");
                foreach (DataRow row in articleAry)
                {
                    row["Sel"] = e.FormattedValue;
                }

                dr["sel"] = e.FormattedValue;
                dr.EndEdit();
                this.gridArticleSize.Refresh();
            };

            DataGridViewGeneratorTextColumnSettings itemsetting = new DataGridViewGeneratorTextColumnSettings();
            itemsetting.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return; // 判斷是Header
                }

                if (this.ArticleSizeTb != null)
                {
                    DataRow dr = this.gridCutRef.GetDataRow(e.RowIndex);
                    dr["item"] = e.FormattedValue;
                    dr.EndEdit();
                    DataRow[] articleAry = this.ArticleSizeTb.Select($"Ukey ='{dr["Ukey"]}' and Fabriccombo = '{dr["Fabriccombo"]}'");
                    foreach (DataRow row in articleAry)
                    {
                        row["item"] = dr["item"];
                    }

                    this.gridArticleSize.Refresh();
                }
            };
            #endregion
            #region 左上 gridCutRef
            this.gridCutRef.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridCutRef)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: chcutref)
                .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("POID", header: "PO ID", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Date("estCutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Fabriccombo", header: "Fabric" + Environment.NewLine + "Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("FabricPanelCode", header: "Pattern" + Environment.NewLine + "Panel", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Item", header: "Item", width: Widths.AnsiChars(20), settings: itemsetting).Get(out Ict.Win.UI.DataGridViewTextBoxColumn item)
                .Text("FabricKind", header: "Fabric Kind", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;

            item.MaxLength = 20;
            this.gridCutRef.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutRef.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutRef.Columns["Item"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region 中上 gridQty 事件
            DataGridViewGeneratorNumericColumnSettings qtySizecell = new DataGridViewGeneratorNumericColumnSettings();
            qtySizecell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1 || e.Button != MouseButtons.Right)
                {
                    return;
                }

                this.SelectWorkOrderDistribute(1);
            };

            DataGridViewGeneratorTextColumnSettings tone = new DataGridViewGeneratorTextColumnSettings() { MaxLength = 1 };
            tone.EditingKeyPress += (s, e) =>
            {
                var regex = new Regex(@"[^A-Z\b\s]");
                if (regex.IsMatch(e.KeyChar.ToString()))
                {
                    e.Handled = true;
                }
            };
            tone.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridQty.GetDataRow(e.RowIndex);
                dr["Tone"] = this.LetterToNumber(e.FormattedValue.ToString());
                dr["ToneChar"] = e.FormattedValue;
                dr.EndEdit();
            };
            #endregion
            #region 中上 gridQty
            this.gridQty.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridQty)
                .Numeric("No", header: "No", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: qtySizecell)
                .Text("ToneChar", header: "Tone", width: Widths.AnsiChars(1), settings: tone)
                ;
            this.gridQty.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridQty.Columns["ToneChar"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region 右上 gridArticleSize 事件
            DataGridViewGeneratorTextColumnSettings orderID = new DataGridViewGeneratorTextColumnSettings();
            orderID.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1 || e.Button != MouseButtons.Right)
                {
                    return;
                }

                this.SelectWorkOrderDistribute(2);
            };

            DataGridViewGeneratorTextColumnSettings sewingLine = new DataGridViewGeneratorTextColumnSettings();
            sewingLine.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1 || e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                string sql = $@"
Select DISTINCT ID
From SewingLine WITH (NOLOCK)
where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{this.keyWord}')";
                SelectItem sele = new SelectItem(sql, "10", dr["SewingLine"].ToString()) { Width = 300 };
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = sele.GetSelectedString();
                this.ByArticleSizeSetValue("SewingLine", sele.GetSelectedString());
            };
            sewingLine.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                if (e.FormattedValue.ToString() == dr["Sewingline"].ToString())
                {
                    return;
                }

                dr["Sewingline"] = MyUtility.Check.Seek(e.FormattedValue.ToString(), "SewingLine", "ID") ? e.FormattedValue.ToString() : string.Empty;
                dr.EndEdit();
                this.ByArticleSizeSetValue("SewingLine", dr["Sewingline"].ToString());
            };

            DataGridViewGeneratorTextColumnSettings sewingCell = new DataGridViewGeneratorTextColumnSettings();
            sewingCell.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1 || e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                string sqlcmd = "Select distinct SewingCell from Sewingline WITH (NOLOCK) where SewingCell!=''";
                SelectItem sele = new SelectItem(sqlcmd, "10", dr["SewingCell"].ToString()) { Width = 300 };
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = sele.GetSelectedString();
                this.ByArticleSizeSetValue("SewingCell", sele.GetSelectedString());
            };
            sewingCell.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                if (e.FormattedValue.ToString() == dr["SewingCell"].ToString())
                {
                    return;
                }

                dr["sewingCell"] = MyUtility.Check.Seek(e.FormattedValue.ToString(), "SewingLine", "sewingCell") ? e.FormattedValue.ToString() : string.Empty;
                dr.EndEdit();
                this.ByArticleSizeSetValue("SewingCell", dr["sewingCell"].ToString());
            };

            DataGridViewGeneratorNumericColumnSettings cutoutput = new DataGridViewGeneratorNumericColumnSettings();
            cutoutput.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                int oldValue = MyUtility.Convert.GetInt(dr["cutOutput"]);
                if (this.GetBalancebyDistribute(dr, MyUtility.Convert.GetInt(e.FormattedValue)) > 0)
                {
                    MyUtility.Msg.InfoBox($"[{dr["OrderID"]}][{dr["Article"]}][{dr["SizeCode"]}] can't exceed the work order distribute qty.");
                    dr["cutOutput"] = oldValue;
                    dr.EndEdit();
                    return;
                }

                dr["cutOutput"] = e.FormattedValue;
                dr.EndEdit();
                this.GetBalancebyWorkOrder();
                this.CalGridQty();
            };
            #endregion
            #region 右上 gridArticleSize
            this.gridArticleSize.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridArticleSize)
                .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: orderID)
                .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SewingLine", header: "Line#", width: Widths.AnsiChars(2), settings: sewingLine)
                .Text("SewingCell", header: "Sew" + Environment.NewLine + "Cell", width: Widths.AnsiChars(2), settings: sewingCell)
                .Numeric("Cutoutput", header: "Qty", width: Widths.AnsiChars(5), integer_places: 5, settings: cutoutput)
                .Numeric("TotalParts", header: "Parts", width: Widths.AnsiChars(4), integer_places: 3, iseditingreadonly: true)
                .Text("isEXCESS", header: "EXCESS", width: Widths.AnsiChars(2), iseditingreadonly: true)
                ;
            this.gridArticleSize.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridArticleSize.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridArticleSize.Columns["OrderID"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridArticleSize.Columns["SewingLine"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridArticleSize.Columns["SewingCell"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridArticleSize.Columns["Cutoutput"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region 左下 gridCutpart 事件
            DataGridViewGeneratorTextColumnSettings patterncell = new DataGridViewGeneratorTextColumnSettings();
            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS" || e.Button != MouseButtons.Right)
                {
                    return;
                }

                SelectItem sele = new SelectItem(this.GarmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                if (sele.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                if (this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}' and ukey = '{dr["ukey"]}'").Count() > 0)
                {
                    dr["isPair"] = this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}' and ukey = '{dr["ukey"]}'")[0]["isPair"];
                }

                e.EditingControl.Text = sele.GetSelectedString();
                dr["PatternDesc"] = sele.GetSelecteds()[0]["PatternDesc"].ToString();
                dr["PatternCode"] = sele.GetSelecteds()[0]["PatternCode"].ToString();
                string[] ann = Regex.Replace(sele.GetSelecteds()[0]["Annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation
                string art = string.Empty;
                #region 算Subprocess
                if (ann.Length > 0)
                {
                    art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), this.artTb, out bool lallpart);
                }
                #endregion
                dr["art"] = art;
                dr["Parts"] = 1;
                dr.EndEdit();
                this.SynchronizeMain(0, "PatternCode");
                this.CombineSubprocessIspair(MyUtility.Convert.GetLong(dr["ukey"]));
                this.Calpart();
                Prgs.CheckNotMain(dr, this.GarmentTb);
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                if (dr["PatternCode"].ToString() == patcode)
                {
                    return;
                }

                if (this.patternTb.Select($@"PatternCode = '{patcode}' and ukey = '{dr["ukey"]}'").Count() > 0)
                {
                    dr["isPair"] = this.patternTb.Select($@"PatternCode = '{patcode}' and ukey = '{dr["ukey"]}'")[0]["isPair"];
                }

                DataRow[] gemdr = this.GarmentTb.Select($"PatternCode ='{patcode}'", string.Empty);
                if (gemdr.Length > 0)
                {
                    dr["PatternDesc"] = gemdr[0]["PatternDesc"].ToString();
                    dr["PatternCode"] = gemdr[0]["PatternCode"].ToString();
                    string[] ann = Regex.Replace(gemdr[0]["Annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation
                    string art = string.Empty;
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), this.artTb, out bool lallpart);
                    }
                    #endregion
                    dr["art"] = art;
                    dr["Parts"] = 1;
                }

                dr.EndEdit();
                this.SynchronizeMain(0, "PatternCode");
                this.CombineSubprocessIspair(MyUtility.Convert.GetLong(dr["ukey"]));
                this.Calpart();
                Prgs.CheckNotMain(dr, this.GarmentTb);
            };

            DataGridViewGeneratorTextColumnSettings patternDesc = new DataGridViewGeneratorTextColumnSettings { CharacterCasing = CharacterCasing.Normal };
            patternDesc.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                dr["PatternDesc"] = e.FormattedValue;
                dr.EndEdit();
                this.SynchronizeMain(0, "patternDesc");
                Prgs.CheckNotMain(dr, this.GarmentTb);
            };

            DataGridViewGeneratorTextColumnSettings subcell = new DataGridViewGeneratorTextColumnSettings();
            subcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                SelectItem2 sele;
                sele = new SelectItem2("Select id from subprocess WITH (NOLOCK) where junk=0 and IsSelection=1", "Subprocess", "23", dr["PatternCode"].ToString(), null, null, null);
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                string subpro = sele.GetSelectedString().Replace(",", "+");

                e.EditingControl.Text = subpro;
                dr["art"] = subpro;
                dr.EndEdit();

                string[] arts = MyUtility.Convert.GetString(dr["art"]).Split('+');
                string[] pssps = MyUtility.Convert.GetString(dr["PostSewingSubProcess_String"]).Split('+');
                string nbcass = MyUtility.Convert.GetString(dr["NoBundleCardAfterSubprocess_String"]);
                if (!arts.Contains(nbcass))
                {
                    dr["NoBundleCardAfterSubprocess_String"] = string.Empty;
                }

                List<string> recordPS = new List<string>();
                foreach (var art in arts)
                {
                    if (pssps.Contains(art))
                    {
                        recordPS.Add(art);
                    }
                }

                dr["PostSewingSubProcess_String"] = string.Join("+", recordPS);
                dr.EndEdit();
                Prgs.CheckNotMain(dr, this.GarmentTb);
            };

            DataGridViewGeneratorNumericColumnSettings partQtyCell = new DataGridViewGeneratorNumericColumnSettings();
            partQtyCell.CellEditable += (s, e) =>
            {
                e.IsEditable = !this.chkCombineSubprocess.Checked;
            };
            partQtyCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.Calpart();
            };

            DataGridViewGeneratorCheckBoxColumnSettings isPair = new DataGridViewGeneratorCheckBoxColumnSettings();
            isPair.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(dr["PatternCode"]).ToUpper() == "ALLPARTS")
                {
                    return;
                }

                bool ispair = MyUtility.Convert.GetBool(e.FormattedValue);
                dr["IsPair"] = ispair;
                dr.EndEdit();
                this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}' and ukey = '{dr["ukey"]}'").ToList().ForEach(r => r["IsPair"] = ispair);
            };

            DataGridViewGeneratorTextColumnSettings postSewingSubProcess_String = new DataGridViewGeneratorTextColumnSettings();
            postSewingSubProcess_String.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["art"]))
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                string inArt = "'" + string.Join("','", MyUtility.Convert.GetString(dr["art"]).Split('+')) + "'";
                string sqlcmd = $"Select id from subprocess WITH (NOLOCK) where junk=0 and IsSelection=1 and id in({inArt})";
                SelectItem2 sele = new SelectItem2(sqlcmd, "Subprocess", "23", dr["PostSewingSubProcess_String"].ToString(), null, null, null);
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["PostSewingSubProcess_String"] = sele.GetSelectedString().Replace(",", "+");
                dr.EndEdit();
            };
            postSewingSubProcess_String.CellFormatting += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["art"]) || dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            DataGridViewGeneratorTextColumnSettings noBundleCardAfterSubprocess_String = new DataGridViewGeneratorTextColumnSettings();
            noBundleCardAfterSubprocess_String.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["art"]))
                {
                    return;
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                string inArt = "'" + string.Join("','", MyUtility.Convert.GetString(dr["art"]).Split('+')) + "'";
                string sqlcmd = $"select id = '' union all Select id from subprocess WITH (NOLOCK) where IsBoundedProcess = 1 and id in({inArt})";
                SelectItem sele = new SelectItem(sqlcmd, "23", MyUtility.Convert.GetString(dr["NoBundleCardAfterSubprocess_String"]), "Subprocess");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["NoBundleCardAfterSubprocess_String"] = sele.GetSelectedString();
                dr.EndEdit();
            };
            noBundleCardAfterSubprocess_String.CellFormatting += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["art"]) || dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            #endregion
            #region 左下 gridCutpart
            this.gridPattern.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridPattern)
                .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
                .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15), settings: patternDesc)
                .Text("Location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: subcell)
                .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell)
                .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: isPair)
                .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: postSewingSubProcess_String)
                .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: noBundleCardAfterSubprocess_String)
                ;
            this.gridPattern.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridPattern.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridPattern.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridPattern.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridPattern.Columns["art"].DefaultCellStyle.BackColor = Color.SkyBlue;
            this.gridPattern.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridPattern.Columns["IsPair"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region 右下 gridAllPart 事件
            DataGridViewGeneratorTextColumnSettings patterncell2 = new DataGridViewGeneratorTextColumnSettings();
            patterncell2.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridAllPart.GetDataRow(e.RowIndex);
                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                SelectItem sele;
                sele = new SelectItem(this.GarmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                DialogResult result = sele.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                e.EditingControl.Text = sele.GetSelectedString();
                dr["PatternDesc"] = sele.GetSelecteds()[0]["PatternDesc"].ToString();
                dr["PatternCode"] = sele.GetSelecteds()[0]["PatternCode"].ToString();
                dr["Annotation"] = sele.GetSelecteds()[0]["Annotation"].ToString();
                dr["Parts"] = 1;
                dr.EndEdit();
                this.SynchronizeMain(1, "PatternCode");
                this.Calpart();
            };
            patterncell2.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridAllPart.GetDataRow(e.RowIndex);
                dr["PatternCode"] = e.FormattedValue;
                dr.EndEdit();
                this.SynchronizeMain(1, "PatternCode");
                this.Calpart();
            };

            DataGridViewGeneratorTextColumnSettings patternDesc2 = new DataGridViewGeneratorTextColumnSettings { CharacterCasing = CharacterCasing.Normal };
            patternDesc2.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridAllPart.GetDataRow(e.RowIndex);
                dr["PatternDesc"] = e.FormattedValue;
                dr.EndEdit();
                this.SynchronizeMain(1, "PatternDesc");
                this.Calpart();
            };

            DataGridViewGeneratorNumericColumnSettings partQtyCell2 = new DataGridViewGeneratorNumericColumnSettings();
            partQtyCell2.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridAllPart.GetDataRow(e.RowIndex);
                dr["Parts"] = e.FormattedValue;
                dr.EndEdit();
                this.Calpart();
            };
            #endregion
            #region 右下 gridAllPart
            this.gridAllPart.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridAllPart)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
                .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13), settings: patternDesc2)
                .Text("Location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell2)
                .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
;
            this.gridAllPart.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridAllPart.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridAllPart.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridAllPart.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridAllPart.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridAllPart.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridAllPart.Columns["IsPair"].DefaultCellStyle.BackColor = Color.Pink;

            for (int i = 0; i < this.gridAllPart.ColumnCount; i++)
            {
                this.gridAllPart.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion
        }

        private void SelectWorkOrderDistribute(int type)
        {
            DataRow dr = this.gridQty.CurrentDataRow;
            string filter = $"Ukey = {dr["ukey"]}";
            var allAS = this.ArticleSizeTbOri.Select(filter).TryCopyToDataTable(this.ArticleSizeTbOri);
            var seleAS = this.ArticleSizeTb.DefaultView.ToTable().AsEnumerable().ToList();
            var selectpkeys = seleAS.Select(s => MyUtility.Convert.GetLong(s["Pkey"])).ToList();
            allAS.AsEnumerable().Where(w => selectpkeys.Contains((long)w["Pkey"])).ToList().ForEach(f => f["Sel"] = 1);
            if (type == 2)
            {
                // 排除在此 No 下已選過的 WorkOrder_Distribute
                string pkeys = this.ArticleSizeTb.DefaultView.ToTable().AsEnumerable().Where(w => !MyUtility.Check.Empty(w["Pkey"]))
                    .Select(o => o["Pkey"].ToString()).JoinToString("','");
                filter += pkeys.Empty() ? string.Empty : $" and Pkey not in ('{pkeys}')";
                filter += MyUtility.Check.Empty(dr["Article"]) ? string.Empty : $" and Article = '{dr["Article"]}' and SizeCode = '{dr["SizeCode"]}'";
                allAS = allAS.Select(filter).TryCopyToDataTable(allAS);
            }

            // 取得已選過資料在不同 No 下 Qty 加總 (包含當前點選)
            var sel = this.ArticleSizeTb.Select(filter).AsEnumerable().GroupBy(g => new { Pkey = MyUtility.Convert.GetLong(g["Pkey"]) })
                .Select(s => new { s.Key.Pkey, Qty = s.Sum(sum => MyUtility.Convert.GetInt(sum["cutoutput"])) }).ToList();

            // 取得已選過資料在不同 No 下 Qty 加總 (排除當前點選)
            var otherSel = this.ArticleSizeTb.Select(filter + $" and iden <> {dr["iden"]}").AsEnumerable()
                .GroupBy(g => new { Pkey = MyUtility.Convert.GetLong(g["Pkey"]) })
                .Select(s => new { s.Key.Pkey, Qty = s.Sum(sum => MyUtility.Convert.GetInt(sum["cutoutput"])) }).ToList();
            foreach (DataRow row in allAS.Rows)
            {
                int selQty = sel.Where(w => w.Pkey == (long)row["Pkey"]).Select(s => s.Qty).FirstOrDefault();
                if (!MyUtility.Convert.GetBool(row["sel"]))
                {
                    row["cutoutput"] = MyUtility.Convert.GetInt(row["cutoutput"]) - selQty;
                }
                else
                {
                    row["cutoutput"] = seleAS.Where(w => MyUtility.Convert.GetLong(w["Pkey"]) == (long)row["Pkey"])
                        .Select(s => MyUtility.Convert.GetInt(s["cutoutput"])).FirstOrDefault();
                }

                if (row["cutoutput"] == DBNull.Value)
                {
                    row.Delete();
                }
                else
                {
                    row["otherSelQty"] = otherSel.Where(w => w.Pkey == (long)row["Pkey"]).Select(s => s.Qty).FirstOrDefault();
                    int realbalanceQty = MyUtility.Convert.GetInt(row["RealCutOutput"]) - MyUtility.Convert.GetInt(row["CreatedBundleQty"]) - MyUtility.Convert.GetInt(row["OtherSelQty"]) - MyUtility.Convert.GetInt(row["cutOutput"]);
                    row["RealbalanceQty"] = realbalanceQty < 0 ? 0 : realbalanceQty;
                }
            }

            DataTable finaltable = allAS.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).TryCopyToDataTable(allAS);
            var form = new P15_SelectDistribute(finaltable, this, type);
            if (form.ShowDialog() == DialogResult.OK)
            {
                // 選取的資料的 No, 填入新值
                form.Sel_Distribute.AsEnumerable().ToList().ForEach(row =>
                {
                    row["No"] = dr["No"];
                    row["iden"] = dr["iden"];
                });

                if (type == 1)
                {
                    this.ArticleSizeTb.Select($"iden = {dr["iden"]}").Delete();
                    this.ArticleSizeTb.Merge(form.Sel_Distribute);
                }
                else
                {
                    this.gridArticleSize.CurrentDataRow.Delete();
                    this.ArticleSizeTb.Merge(form.Sel_Distribute);
                }

                dr["POID"] = form.Sel_Distribute.Rows[0]["POID"];
                dr["Article"] = form.Sel_Distribute.Rows[0]["Article"];
                dr["SizeCode"] = form.Sel_Distribute.Rows[0]["SizeCode"];
                dr["Qty"] = this.ArticleSizeTb.DefaultView.ToTable().AsEnumerable().Sum(sum => MyUtility.Convert.GetInt(sum["cutoutput"]));
                this.ArticleSizeTb.DefaultView.Sort = "OrderID";
            }

            this.GetBalancebyWorkOrder();
            this.GridAutoResizeColumns();
            this.Calpart();
        }

        private void ByArticleSizeSetValue(string columnName, string columnValue)
        {
            DataRow dr = this.gridQty.CurrentDataRow;
            this.ArticleSizeTb.Select($"iden = {dr["iden"]}").ToList().ForEach(row => row[columnName] = columnValue);
        }

        private void CalGridQty()
        {
            if (this.ArticleSizeTb == null)
            {
                return;
            }

            this.gridQty.CurrentDataRow["Qty"] = this.ArticleSizeTb.DefaultView.ToTable().AsEnumerable().Sum(sum => MyUtility.Convert.GetInt(sum["cutOutput"]));
        }

        private void SynchronizeMain(int type, string columnName)
        {
            // tpye = 0 左同步到右， type = 1 右同步到左
            DataRow dr = this.gridPattern.CurrentDataRow;
            if (!this.chkCombineSubprocess.Checked || MyUtility.Convert.GetString(dr["PatternCode"]) == "ALLPARTS")
            {
                return;
            }

            try
            {
                if (type == 0)
                {
                    DataRow[] adrs = this.allpartTb.Select($"CombineSubprocessGroup = {dr["CombineSubprocessGroup"]} and isMain = 1");
                    if (adrs.Length == 0)
                    {
                        return;
                    }

                    DataRow adr = adrs[0];
                    adr["PatternDesc"] = dr["PatternDesc"];
                    if (columnName == "PatternCode")
                    {
                        adr["PatternCode"] = dr["PatternCode"];
                        adr["Parts"] = dr["Parts"];
                        adr["Location"] = dr["Location"];
                        adr["IsPair"] = dr["IsPair"];
                        dr["IsPair"] = false;
                    }
                }

                if (type == 1 && MyUtility.Convert.GetBool(this.gridAllPart.CurrentDataRow["isMain"]))
                {
                    DataRow adr = this.gridAllPart.CurrentDataRow;
                    dr["PatternDesc"] = adr["PatternDesc"];
                    dr["PatternCode"] = adr["PatternCode"];
                    dr["Location"] = adr["Location"];
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        #region 按下 Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.ClearAll();
            if (this.BeforeQuery())
            {
                this.ShowWaitMessage("Query");
                DBProxy.Current.DefaultTimeout = 300;
                this.Query();
                DBProxy.Current.DefaultTimeout = 0;
                this.HideWaitMessage();
            }
        }

        private void ClearAll()
        {
            this.allpartTb.Clear();
            this.patternTb.Clear();
            this.artTb.Clear();
            this.GarmentTb = null;
            this.CutRefTb = null;
            this.ArticleSizeTb = null;
            this.ExcessTb = null;
            this.SizeRatioTb = null;
            this.gridPattern.DataSource = null;
            this.gridAllPart.DataSource = null;
            this.gridQty.DataSource = null;
            this.gridArticleSize.DataSource = null;
            this.gridCutRef.DataSource = null;
        }

        private bool BeforeQuery()
        {
            if (MyUtility.Check.Empty(this.txtCutRef.Text) && MyUtility.Check.Empty(this.dateEstCutDate.Value) && MyUtility.Check.Empty(this.txtPOID.Text))
            {
                MyUtility.Msg.WarningBox("[CutRef#] or [Est. Cut Date] or [PO ID] can't be empty!!");
                return false;
            }

            return true;
        }

        private void Query()
        {
            if (MyUtility.Check.Seek("select AutoGenerateByTone,IsCombineSubProcess,IsNoneShellNoCreateAllParts from SYSTEM", out DataRow sysDr))
            {
                this.tone = MyUtility.Convert.GetString(sysDr["AutoGenerateByTone"]);
                this.isCombineSubProcess = MyUtility.Convert.GetBool(sysDr["IsCombineSubProcess"]);
                this.isNoneShellNoCreateAllParts = MyUtility.Convert.GetBool(sysDr["IsNoneShellNoCreateAllParts"]);
            }

            string cutref = this.txtCutRef.Text;
            string cutdate = this.dateEstCutDate.Value == null ? string.Empty : this.dateEstCutDate.Value.Value.ToShortDateString();
            string poid = this.txtPOID.Text;
            string factory = this.txtfactoryByM.Text;
            string where = MyUtility.Check.Empty(cutref) ? string.Empty : Environment.NewLine + $"and w.cutref='{cutref}'";
            where += MyUtility.Check.Empty(cutdate) ? string.Empty : Environment.NewLine + $"and w.estcutdate='{cutdate}'";
            where += MyUtility.Check.Empty(poid) ? string.Empty : Environment.NewLine + $"and o.poid='{poid}'";
            where += MyUtility.Check.Empty(factory) ? string.Empty : Environment.NewLine + $"and o.FtyGroup='{factory}'";
            string distru_where = this.chkAEQ.Checked ? string.Empty : " and wd.orderid <>'EXCESS'";
            this.gridArticleSize.Columns["isEXCESS"].Visible = this.chkAEQ.Checked;

            // 左上
            string query_cmd = $@"
Select
	 sel = cast(0 as bit)
	, w.cutref
	, o.poid
	, w.OrderID
	, w.estcutdate
	, w.Fabriccombo
	, w.FabricPanelCode
	, w.cutno
	, item.item
	, w.SpreadingNoID
	, w.colorid
	, w.Ukey
	, FabricKind.FabricKind
	, FabricKind.FabricKindID
	, TTLCutQty = (select SUM(qty) from WorkOrder_Distribute wd with(nolock) where wd.WorkOrderUkey = w.Ukey {distru_where})
	, CreatedBundleQty = isnull((select SUM(bdq.qty) from Bundle wd with(nolock) inner join bundle_detail_Qty bdq on bdq.id = wd.id where wd.cutref = w.cutref), 0)
    , o.StyleUkey
	, w.MDivisionId
    , IsCombineSubProcess = cast({(this.isCombineSubProcess ? "1" : "0")} as bit)
    , isNoneShellNoCreateAllParts = cast(iif(FabricKind.FabricKindID <> '1' and {(this.isNoneShellNoCreateAllParts ? "1" : "0")} = 1, 1, 0) as bit)
from  workorder w WITH (NOLOCK) 
inner join orders o WITH (NOLOCK) on o.ID = w.id and o.cuttingsp = w.id
outer apply(
	Select item = Reason.Name 
	from Reason WITH (NOLOCK)
	inner join Style WITH (NOLOCK) on Style.ApparelType = Reason.id
	where Reason.Reasontypeid = 'Style_Apparel_Type' 
	and Style.ukey = o.styleukey 
)item
outer apply (SELECT TOP 1 wd.patternpanel FROM workorder_PatternPanel wd WITH (NOLOCK) WHERE w.ukey = wd.workorderukey)wd
outer apply(
    SELECT TOP 1 FabricKind = DD.id + '-' + DD.NAME, FabricKindID = DD.id
    FROM order_colorcombo OCC WITH (NOLOCK)
	inner join order_bof OB WITH (NOLOCK) on OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
	inner join  dropdownlist DD WITH (NOLOCK) on  DD.id = OB.kind
    WHERE OCC.id = w.id
	and OCC.patternpanel = wd.patternpanel
	and DD.[type] = 'FabricKind'
)FabricKind
where o.mDivisionid = '{this.keyWord}' 
and isnull(w.CutRef,'') <> ''
{where}
order by o.poid,w.estcutdate,w.Fabriccombo,w.cutno
";
            DualResult result = DBProxy.Current.Select(null, query_cmd, out this.CutRefTb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.CutRefTb.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data Found!");
                return;
            }

            // 右上, 中上用此撈出資料再group
            string distru_cmd = $@"
Select
	 sel = cast(0 as bit)
	, w.Ukey
	, No = DENSE_RANK() over (partition by w.ukey order by article.article,wd.sizecode) -- 對應 GridQty 的欄位
	, iden = 0
	, Pkey = ROW_NUMBER() over (order by wd.sizecode,wd.orderid,w.FabricPanelCode) -- 為 workorder_Distribute 的 Key, 計算已選總和用
	, w.cutref
	, orderid = iif(wd.OrderID = 'EXCESS', isnull(l.orderid,l2.OrderID), wd.OrderID)
	, article.article
	, sizecode.sizecode
	, isEXCESS = iif(wd.OrderID = 'EXCESS','Y','')
	, w.colorid
	, w.Fabriccombo
	, w.FabricPanelCode
	, Ratio = ''
	, w.cutno
	, Sewingline = o.SewLine
	, SewingCell= w.CutCellid
	, item.item
	, Qty = 1
	, RealCutOutput = isnull(wd.Qty,0)
	, TotalParts = 0
	, o.poid
	, startno = 0
	, o.StyleUkey
	, w.MDivisionId
    , o.BuyerDelivery
into #tmp
from workorder w WITH (NOLOCK) 
inner join workorder_Distribute wd WITH (NOLOCK) on w.ukey = wd.workorderukey
inner join orders o WITH (NOLOCK) on o.ID = w.id and o.cuttingsp = w.id
outer apply(
	select top 1 wd.OrderID,wd.Article,wd.SizeCode
	from workorder_Distribute wd WITH(NOLOCK)
	where wd.WorkOrderUkey = w.Ukey and wd.orderid <>'EXCESS' and wd.SizeCode = wd.SizeCode 
	order by wd.OrderID desc
)l
outer apply(
	select top 1 wd.OrderID,wd.Article,wd.SizeCode
	from workorder_Distribute wd WITH(NOLOCK)
	where wd.WorkOrderUkey = w.Ukey and wd.orderid <>'EXCESS'
	order by wd.OrderID desc
)l2
outer apply(
	Select item = Reason.Name 
	from Reason WITH (NOLOCK)
	inner join Style WITH (NOLOCK) on Style.ApparelType = Reason.id
	where Reason.Reasontypeid = 'Style_Apparel_Type' 
	and Style.ukey = o.styleukey 
)item
outer apply(select article = iif(wd.OrderID = 'EXCESS',isnull(l.article,l2.article),wd.article))article
outer apply(select sizecode = iif(wd.OrderID = 'EXCESS',isnull(l.sizecode,l2.sizecode),wd.sizecode))sizecode
Where isnull(w.CutRef,'') <> ''
and o.mDivisionid = '{this.keyWord}'
{where}
{distru_where}
order by article.article,wd.sizecode,wd.orderid,w.FabricPanelCode

select bdo.qty,wd.id,bdo.BundleNo,bd.PatternCode,bd.BundleGroup,wd.CutRef,wd.Article,wd.Sizecode,bdo.OrderID
into #tmpx
from Bundle wd with(nolock)
inner join Bundle_Detail bd with(nolock) on bd.Id = wd.ID
inner join Bundle_Detail_Order bdo on bdo.BundleNo = bd.BundleNo
where exists(select 1 from #tmp t where wd.cutref = t.CutRef and wd.Article = t.article and bd.Sizecode = t.sizecode and bdo.OrderID = t.orderid)

select CutRef,Article,Sizecode,OrderID,qty=SUM(Qty)
into #bundleSPCreatedQty
from (
	select CutRef,Article,Sizecode,OrderID,qty from #tmpx where PatternCode = 'ALLPARTS'

	union all
	select CutRef,Article,Sizecode,OrderID,qty=SUM(Qty)
	from (
		select id,BundleGroup,CutRef,Article,Sizecode,OrderID, Qty = min(x.Qty)
		from(
			select t.ID,t.PatternCode,t.BundleGroup,CutRef,Article,Sizecode,OrderID, Qty = sum(t.Qty)
			from #tmpx t
			where not exists(select 1 from #tmpx where PatternCode = 'ALLPARTS' and id = t.id)
			group by t.ID,t.PatternCode,t.BundleGroup,CutRef,Article,Sizecode,OrderID
		)x
		group by id,BundleGroup,CutRef,Article,Sizecode,OrderID
	)x
	group by CutRef,Article,Sizecode,OrderID
)x
group by CutRef,Article,Sizecode,OrderID

select t.*,
	cutoutput = pb.PositiveBalQty, -- 預設帶出剩下數量 = WorkOrder數量 - Bundle已建立數量
	CreatedBundleQty = bd.qty,
	RealbalanceQty = pb.PositiveBalQty,
	OtherSelQty = 0
from #tmp t
left join #bundleSPCreatedQty bd on t.CutRef = bd.CutRef and t.article = bd.Article and t.sizecode = bd.Sizecode and t.orderid = bd.OrderID
outer apply(select balQty = isnull(t.RealCutOutput, 0) - isnull(bd.qty, 0))bal
outer apply(select PositiveBalQty = IIF(isnull(bal.balQty, 0) < 0, 0, isnull(bal.balQty, 0)))pb

select distinct MDivisionId, StyleUkey, Fabriccombo, Article, cutref, POID, ukey into #msfa from #tmp

select f.PatternCode,f.PatternDesc,f.Parts, art = isnull(art, ''), m.cutref, m.poid, m.ukey, f.ispair, f.Location,
	NoBundleCardAfterSubprocess_String = ISNULL(ns, ''),
	PostSewingSubProcess_String=ISNULL(ps, ''),
    m.MDivisionId,m.StyleUkey,m.Fabriccombo,m.Article
into #bundleinfo
from #msfa m
inner join FtyStyleInnovation f with(NOLOCK)
    on f.MDivisionID = m.MDivisionId and f.StyleUkey = m.StyleUkey and f.Fabriccombo = m.Fabriccombo and f.Article = m.Article
outer apply(
	select art = STUFF((
		select CONCAT('+', fa.SubprocessId)
		from FtyStyleInnovation_Artwork fa where FtyStyleInnovationUkey = f.Ukey
		for xml path('')
	),1,1,'')
)art
outer apply(
	select ns = STUFF((
		select CONCAT('+', fa.SubprocessId)
		from FtyStyleInnovation_Artwork fa where FtyStyleInnovationUkey = f.Ukey
		and fa.NoBundleCardAfterSubprocess = 1
		for xml path('')
	),1,1,'')
)n
outer apply(
	select ps = STUFF((
		select CONCAT('+', fa.SubprocessId)
		from FtyStyleInnovation_Artwork fa where FtyStyleInnovationUkey = f.Ukey
		and fa.PostSewingSubProcess = 1
		for xml path('')
	),1,1,'')
)p

select
    wd.PatternCode,wd.PatternDesc,wd.Parts,wd.art,wd.cutref,wd.poid,wd.ukey,wd.ispair,wd.Location,
    wd.NoBundleCardAfterSubprocess_String,wd.PostSewingSubProcess_String,
    wd.MDivisionId,wd.StyleUkey,wd.Fabriccombo,wd.Article,
    isMain = cast(0 as bit),
    CombineSubprocessGroup = cast(0 as tinyint)
from #bundleinfo wd

union all
select distinct
	PatternCode='ALLPARTS',
	PatternDesc='All Parts',
	Parts=0,art='',wd.cutref,wd.poid,wd.ukey,wd.ispair,
	Location='',wd.NoBundleCardAfterSubprocess_String,wd.PostSewingSubProcess_String,
    wd.MDivisionId,wd.StyleUkey,wd.Fabriccombo,wd.article,
    isMain = cast(0 as bit),
    CombineSubprocessGroup = cast(0 as tinyint)
from #bundleinfo wd
where not exists(select 1 from #bundleinfo c
    where c.MDivisionId = wd.MDivisionId and c.StyleUkey = wd.StyleUkey and c.Fabriccombo = wd.Fabriccombo and c.article = wd.article
    and c.CutRef = wd.CutRef and c.PatternCode = 'ALLPARTS')

select 0 as sel,f.PatternCode,f.PatternDesc, '' as annotation,f.Parts, m.cutref, m.poid, m.ukey, f.ispair, f.Location,
    m.MDivisionId,m.StyleUkey,m.Fabriccombo,m.Article,
    isMain = cast(0 as bit),
    CombineSubprocessGroup = cast(0 as tinyint)
from #msfa m
inner join FtyStyleInnovationAllPart f with(NOLOCK)
    on f.MDivisionID = m.MDivisionId and f.StyleUkey = m.StyleUkey and f.Fabriccombo = m.Fabriccombo and f.Article = m.Article

select 0 as sel,f.PatternCode,f.PatternDesc, '' as annotation,f.Parts, m.cutref, m.poid, m.ukey, f.ispair, f.Location,
    m.MDivisionId,m.StyleUkey,m.Fabriccombo,m.Article,
    isMain,
    CombineSubprocessGroup
from #msfa m
inner join FtyStyleInnovationCombineSubprocess f with(NOLOCK)
    on f.MDivisionID = m.MDivisionId and f.StyleUkey = m.StyleUkey and f.Fabriccombo = m.Fabriccombo and f.Article = m.Article

drop table #tmp, #msfa
";
            result = DBProxy.Current.Select(null, distru_cmd, out DataTable[] rightUpDt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.ArticleSizeTb = rightUpDt[0];
            this.fsDt = rightUpDt[1];
            this.faDt = rightUpDt[2];
            this.fcDt = rightUpDt[3];

            string sizeRatio = $@"
Select distinct w.ukey, ws.SizeCode, ws.Qty
from workorder w WITH (NOLOCK) 
inner join workorder_Distribute wd WITH (NOLOCK) on w.ukey = wd.workorderukey
inner join orders o WITH (NOLOCK) on  o.ID = w.id and o.cuttingsp = w.id
inner join WorkOrder_SizeRatio ws WITH (NOLOCK) on ws.WorkOrderUkey = w.Ukey and ws.SizeCode = wd.SizeCode
Where isnull(w.CutRef,'') <> '' 
and o.mDivisionid = '{this.keyWord}'
{where}
";
            result = DBProxy.Current.Select(null, sizeRatio, out this.SizeRatioTb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.patternTbOri = this.patternTb.Clone();
            this.allpartTbOri = this.allpartTb.Clone();
            this.CutRefTb.AsEnumerable().ToList().ForEach(dr => this.CreatePattern(dr)); // 先依據左上資料建立下方兩個資料表

            // 中上每一筆下塞入一組由(iden)對應的下方資料
            this.qtyTb = this.GetNoofBundle(); // 依據右上撈出資料彙整出中上

            // 將下方兩表加入, 下方兩表改為直接對應左上(ISP20201755)
            this.CutRefTb.AsEnumerable().ToList().ForEach(r => this.AddPatternAllpart(r));
            foreach (DataRow dr in this.ArticleSizeTb.Rows)
            {
                dr["iden"] = this.qtyTb.Select($"ukey={dr["ukey"]} and no={dr["no"]}")[0]["iden"];
                dr["TotalParts"] = this.patternTb.Compute("sum(Parts)", $"ukey ={dr["ukey"]}");
            }

            this.ArticleSizeTbOri = this.ArticleSizeTb.Copy(); // 紀錄第一次撈出資料
            this.gridCutRef.DataSource = this.CutRefTb; // 左上
            this.gridQty.DataSource = this.qtyTb; // 中上
            this.gridArticleSize.DataSource = this.ArticleSizeTb; // 右上
            this.gridPattern.DataSource = this.patternTb; // 左下
            this.gridAllPart.DataSource = this.allpartTb; // 右下

            this.GridAutoResizeColumns();
            this.ShowExcessDatas(where);
        }

        private void AddPatternAllpart(DataRow r)
        {
            long ukey = MyUtility.Convert.GetLong(r["ukey"]);
            string m = MyUtility.Convert.GetString(r["MDivisionID"]);
            long styleukey = MyUtility.Convert.GetLong(r["styleukey"]);
            string fabriccombo = MyUtility.Convert.GetString(r["Fabriccombo"]);
            bool isNoneShellNoCreateAllParts = MyUtility.Convert.GetBool(r["isNoneShellNoCreateAllParts"]);
            string article = this.ArticleSizeTb.Select($"ukey = {r["ukey"]}").AsEnumerable().Select(s => s["article"].ToString()).FirstOrDefault();

            string filter = $"Ukey = {ukey} and MDivisionID = '{m}' and StyleUkey = {styleukey} and fabriccombo = '{fabriccombo}' and Article = '{article}'";
            DataTable dtp = this.fsDt.Select(filter).TryCopyToDataTable(this.fsDt);
            DataTable dta = this.faDt.Select(filter).TryCopyToDataTable(this.faDt);
            DataTable dtc = this.fcDt.Select(filter).TryCopyToDataTable(this.fcDt);
            this.RemoveFtyColumn(dtp);
            this.RemoveFtyColumn(dta);
            this.RemoveFtyColumn(dtc);

            DataTable oriP = this.patternTbOri.Select($"Ukey = {ukey}").TryCopyToDataTable(this.patternTbOri);
            DataTable oriA = this.allpartTbOri.Select($"Ukey = {ukey}").TryCopyToDataTable(this.allpartTbOri);

            // 非 CombineSubProcess 且 FtyStyleInnovation 資料沒有
            if (!this.isCombineSubProcess && dtp.Rows.Count == 0)
            {
                dtp = oriP;
                dta = oriA;
            }

            // CombineSubProcess 且無 FtyStyleInnovation 資料，則用原資料
            if (this.isCombineSubProcess && dtc.Rows.Count == 0)
            {
                dtp = oriP.Select($"isMain = 1").TryCopyToDataTable(this.patternTb);
                dtp.AsEnumerable().ToList().ForEach(f => f["isPair"] = false);
                dtp.ImportRow(oriP.Select($"PatternCode = 'ALLPARTS'").FirstOrDefault());

                // 右邊除了 ALLPart 資訊，  資訊全部都要，不論 IsMain
                DataTable psdt = oriP.Select($"PatternCode <> 'ALLPARTS'").TryCopyToDataTable(this.patternTb);
                psdt.AsEnumerable().ToList().ForEach(f => dta.ImportRow(f));
                dta.Merge(oriA);
            }

            // CombineSubProcess 且有 FtyStyleInnovation 資料，用 FtyStyleInnovation 資料
            if (this.isCombineSubProcess && dtc.Rows.Count > 0)
            {
                // 右邊除了 ALLPart 資訊， FtyStyleInnovationCombineSubprocess 資訊全部都要，不論 IsMain
                dta.Merge(dtc);

                // 將 FtyStyleInnovationCombineSubprocess 出來的資料，依據 IsMain 找出顯示在左邊資訊
                DataRow adr = dtp.Select($"PatternCode = 'ALLPARTS'").FirstOrDefault();
                dtc.Columns.Add("art", typeof(string));
                foreach (DataRow cdr in dtc.Rows)
                {
                    DataRow[] drps = oriP.Select($"PatternCode = '{cdr["PatternCode"]}'");
                    if (drps.Length > 0)
                    {
                        cdr["art"] = drps[0]["art"];
                    }
                }

                dtp = dtc.Select($@"isMain = 1").TryCopyToDataTable(this.patternTbOri);
                if (adr != null)
                {
                    dtp.ImportRow(adr);
                }
            }

            // 標記 isNoneShellNoCreateAllParts 不加上 ALLPARTS 細項
            if (isNoneShellNoCreateAllParts)
            {
                this.allpartTb.Merge(dta.Select($"CombineSubProcessGroup <> 0").TryCopyToDataTable(dta));
                DataRow[] drps = dtp.Select($"PatternCode = 'ALLPARTS'");
                if (drps.Length > 0)
                {
                    drps[0]["Parts"] = 0;
                }
            }
            else
            {
                this.allpartTb.Merge(dta);
            }

            this.patternTb.Merge(dtp);
            this.CombineSubprocessIspair(ukey);
        }

        private void RemoveFtyColumn(DataTable dt)
        {
            dt.Columns.Remove("StyleUkey");
            dt.Columns.Remove("Article");
            dt.Columns.Remove("Fabriccombo");
        }

        private DataTable GetNoofBundle()
        {
            // by Article, Size 整理出中上 No of Bundle 的資料表, 並從 1 開始依序給 No 值 (index). 唯一值:Ukey, No
            var result = this.ArticleSizeTb.AsEnumerable()
                .GroupBy(s => new { Ukey = (long)s["Ukey"], No = (long)s["No"], POID = s["POID"].ToString(), Article = s["Article"].ToString(), SizeCode = s["SizeCode"].ToString(), StyleUkey = (long)s["StyleUkey"] })
                .Select((g, i) => new
                {
                    g.Key.Ukey, iden = ++i, g.Key.No, Tone = this.tone,
                    ToneChar = this.tone == "1" ? "A" : string.Empty,
                    g.Key.POID,
                    g.Key.Article,
                    g.Key.SizeCode,
                    g.Key.StyleUkey,
                    Qty = g.Sum(s => (decimal?)s["cutoutput"]),
                })
                .OrderBy(o => o.Article)
                .ThenBy(o => o.SizeCode)
                .ToList();

            return ListToDataTable.ToDataTable(result);
        }

        private void ShowExcessDatas(string where)
        {
            string excess_cmd = $@"
Select distinct w.cutref, w.orderid
from workorder w WITH (NOLOCK) 
inner join workorder_Distribute wd WITH (NOLOCK) on w.ukey = wd.workorderukey
inner join orders o WITH (NOLOCK) on o.ID = w.id and o.cuttingsp = w.id
Where o.mDivisionid = '{this.keyWord}'   
and isnull(w.CutRef,'') <> '' 
and wd.orderid = 'EXCESS' 
{where}
";
            DualResult query_dResult = DBProxy.Current.Select(null, excess_cmd, out this.ExcessTb);
            if (!query_dResult)
            {
                this.ShowErr("ShowExcessDatas() Error", query_dResult);
                return;
            }

            // 沒勾選 Allocate excess 且有 excess 彈窗提醒
            if (!this.chkAEQ.Checked && this.ExcessTb.Rows.Count > 0)
            {
                MsgGridForm m = new MsgGridForm(this.ExcessTb, "Those detail had <EXCESS> not yet distribute to SP#", "Warning")
                {
                    Width = 650,
                };
                m.grid1.Columns[1].Width = 140;
                m.text_Find.Width = 140;
                m.btn_Find.Location = new Point(150, 6);
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                this.FormClosing += (s, args) =>
                {
                    if (m.Visible)
                    {
                        m.Close();
                    }
                };
                m.Show(this);
            }
        }

        private void CreatePattern(DataRow row)
        {
            // 依據 Ukey, Fabriccombo 整理出 Pattern 資料
            string poid = row["POID"].ToString();
            string patternpanel = row["FabricPanelCode"].ToString();
            string cutref = row["Cutref"].ToString();
            string ukey = row["ukey"].ToString();

            int npart = 0; // allpart 數量

            #region 輸出 GarmentTb
            string styleyukey = MyUtility.GetValue.Lookup("Styleukey", poid, "Orders", "ID");
            var sizelist = this.ArticleSizeTb.Select($"Ukey = '{row["Ukey"]}' and Fabriccombo = '{row["Fabriccombo"]}'").AsEnumerable()
                .Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();
            string sizes = "'" + string.Join("','", sizelist) + "'";
            string sqlSizeGroup = $@"SELECT TOP 1 IIF(ISNULL(SizeGroup,'')='','N',SizeGroup) FROM Order_SizeCode WHERE ID ='{poid}' and SizeCode IN ({sizes})";
            string sizeGroup = MyUtility.GetValue.Lookup(sqlSizeGroup);

            string patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{poid}','{cutref}','',{styleyukey},'{sizeGroup}')s";
            string patternukey = MyUtility.GetValue.Lookup(patidsql);

            string headercodesql = $@"
Select distinct ArticleGroup 
from Pattern_GL_LectraCode WITH (NOLOCK) 
where PatternUkey = '{patternukey}' 
order by ArticleGroup";
            DualResult headerResult = DBProxy.Current.Select(null, headercodesql, out DataTable headertb);
            if (!headerResult)
            {
                return;
            }

            string tablecreatesql = $@"Select '{poid}' as orderid,a.*,'' as F_CODE";
            foreach (DataRow dr in headertb.Rows)
            {
                tablecreatesql += $" ,'' as {dr["ArticleGroup"]}";
            }

            tablecreatesql += $" from Pattern_GL a WITH (NOLOCK) Where PatternUkey = '{patternukey}'";
            DualResult tablecreateResult = DBProxy.Current.Select(null, tablecreatesql, out DataTable garmentListTb);
            if (!tablecreateResult)
            {
                return;
            }

            string lecsql = string.Empty;
            lecsql = $"Select * from Pattern_GL_LectraCode a WITH (NOLOCK) where a.PatternUkey = '{patternukey}'";
            DualResult drre = DBProxy.Current.Select(null, lecsql, out DataTable drtb);
            if (!drre)
            {
                return;
            }

            foreach (DataRow dr in garmentListTb.Rows)
            {
                DataRow[] lecdrar = drtb.Select($"SEQ = '{dr["SEQ"]}'");
                foreach (DataRow lecdr in lecdrar)
                {
                    string artgroup = lecdr["ArticleGroup"].ToString().Trim();
                    dr[artgroup] = lecdr["FabricPanelCode"].ToString().Trim();
                }

                if (dr["SEQ"].ToString() == "0001")
                {
                    dr["PatternCode"] = dr["PatternCode"].ToString().Substring(10);
                }
            }

            this.GarmentTb = garmentListTb;
            #endregion

            // 找出相同 PatternPanel 的 subprocessid
            StringBuilder w = new StringBuilder();
            w.Append($"orderid = '{poid}' and (1=0");
            foreach (DataRow dr in headertb.Rows)
            {
                w.Append($" or {dr[0]} = '{patternpanel}' ");
            }

            w.Append(")");

            this.GarmentTb.Columns.Add("CombineSubprocessGroup", typeof(int));
            this.GarmentTb.Columns.Add("IsMain", typeof(bool));
            DataRow[] garmentar = this.GarmentTb.Select(w.ToString());
            Prgs.SetCombineSubprocessGroup_IsMain(garmentar);

            foreach (DataRow dr in garmentar)
            {
                // 若無 annotation 直接寫入 allpartTbOri
                if (MyUtility.Check.Empty(dr["annotation"]))
                {
                    DataRow ndr = this.allpartTbOri.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["Location"] = dr["Location"];
                    ndr["Parts"] = MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                    ndr["Cutref"] = cutref;
                    ndr["POID"] = poid;
                    ndr["ukey"] = ukey;
                    ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                    ndr["CombineSubprocessGroup"] = 0;
                    this.allpartTbOri.Rows.Add(ndr);
                    npart = npart + MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                }
                else
                {
                    // 取得哪些 annotation 是次要
                    List<string> notMainList = Prgs.GetNotMain(dr, this.GarmentTb.Select());
                    string noBundleCardAfterSubprocess_String = string.Join("+", notMainList);

                    // Annotation
                    string[] ann = Regex.Replace(dr["annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation
                    string art = string.Empty;
                    #region Annotation有在Subprocess 內需要寫入bundle_detail_art，寫入Bundle_Detail_pattern
                    if (ann.Length > 0)
                    {
                        #region 算Subprocess

                        // artTb 用不到只是因為共用BundleCardCheckSubprocess PRG其他需要用到
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), this.artTb, out bool lallpart);
                        #endregion
                        if (!lallpart)
                        {
                            if (dr["DV"].ToString() != "0" || dr["Pair"].ToString() != "0")
                            {
                                int count = (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                                bool ismain = MyUtility.Convert.GetBool(dr["isMain"]);
                                for (int i = 0; i < count; i++)
                                {
                                    DataRow ndr2 = this.patternTbOri.NewRow();
                                    ndr2["PatternCode"] = dr["PatternCode"];
                                    ndr2["PatternDesc"] = dr["PatternDesc"];
                                    ndr2["Location"] = dr["Location"];
                                    ndr2["Parts"] = 1;
                                    ndr2["art"] = art;
                                    ndr2["POID"] = poid;
                                    ndr2["Cutref"] = cutref;
                                    ndr2["ukey"] = ukey;
                                    ndr2["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                                    ndr2["isMain"] = ismain && i == 0;
                                    ndr2["CombineSubprocessGroup"] = dr["CombineSubprocessGroup"];
                                    ndr2["NoBundleCardAfterSubprocess_String"] = noBundleCardAfterSubprocess_String;
                                    this.patternTbOri.Rows.Add(ndr2);
                                }
                            }
                            else
                            {
                                DataRow ndr2 = this.patternTbOri.NewRow();
                                ndr2["PatternCode"] = dr["PatternCode"];
                                ndr2["PatternDesc"] = dr["PatternDesc"];
                                ndr2["Location"] = dr["Location"];
                                ndr2["art"] = art;
                                ndr2["Parts"] = dr["alone"];
                                ndr2["POID"] = poid;
                                ndr2["Cutref"] = cutref;
                                ndr2["ukey"] = ukey;
                                ndr2["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                                ndr2["isMain"] = dr["isMain"];
                                ndr2["CombineSubprocessGroup"] = dr["CombineSubprocessGroup"];
                                ndr2["NoBundleCardAfterSubprocess_String"] = noBundleCardAfterSubprocess_String;
                                this.patternTbOri.Rows.Add(ndr2);
                            }
                        }
                        else
                        {
                            DataRow ndr = this.allpartTbOri.NewRow();
                            ndr["PatternCode"] = dr["PatternCode"];
                            ndr["PatternDesc"] = dr["PatternDesc"];
                            ndr["Annotation"] = dr["Annotation"];
                            ndr["Location"] = dr["Location"];
                            ndr["POID"] = poid;
                            ndr["Cutref"] = cutref;
                            ndr["ukey"] = ukey;
                            ndr["Parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                            ndr["CombineSubprocessGroup"] = 0;
                            this.allpartTbOri.Rows.Add(ndr);
                        }
                    }
                    else
                    {
                        DataRow ndr = this.allpartTbOri.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["PatternDesc"] = dr["PatternDesc"];
                        ndr["Annotation"] = dr["Annotation"];
                        ndr["Location"] = dr["Location"];
                        ndr["POID"] = poid;
                        ndr["Cutref"] = cutref;
                        ndr["ukey"] = ukey;
                        ndr["Parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                        ndr["CombineSubprocessGroup"] = 0;
                        this.allpartTbOri.Rows.Add(ndr);
                    }
                    #endregion
                }
            }

            DataRow pdr = this.patternTbOri.NewRow(); // 預設要有ALLPARTS
            pdr["PatternCode"] = "ALLPARTS";
            pdr["PatternDesc"] = "All Parts";
            pdr["Location"] = string.Empty;
            pdr["Parts"] = npart;
            pdr["Cutref"] = cutref;
            pdr["POID"] = poid;
            pdr["ukey"] = ukey;
            pdr["CombineSubprocessGroup"] = 0;
            this.patternTbOri.Rows.Add(pdr);

            DBProxy.Current.DefaultTimeout = 0;
        }
        #endregion

        #region Grid RowChange
        private void GridCutRef_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            // 中上,右上TTL值,下方兩個 依據左上 Ukey
            string filter = $"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}";
            this.qtyTb.DefaultView.RowFilter = filter;
            this.patternTb.DefaultView.RowFilter = filter;
            this.allpartTb.DefaultView.RowFilter = filter;
            this.numNoOfBundle.Value = this.qtyTb.DefaultView.Count;
            this.labelToalCutOutputValue.Text = this.gridCutRef.CurrentDataRow["TTLCutQty"].ToString();
            this.labelAccumulateQty.Text = this.gridCutRef.CurrentDataRow["CreatedBundleQty"].ToString();
            this.chkCombineSubprocess.Checked = MyUtility.Convert.GetBool(this.gridCutRef.CurrentDataRow["IsCombineSubProcess"]);
            this.GridPattern_SelectionChanged(null, null);
            this.chkNoneShellNoCreateAllParts.Checked = MyUtility.Convert.GetBool(this.gridCutRef.CurrentDataRow["IsNoneShellNoCreateAllParts"]);
            this.chkNoneShellNoCreateAllParts.ReadOnly = MyUtility.Convert.GetString(this.gridCutRef.CurrentDataRow["FabricKindID"]) == "1";
            this.ChkNoneShellNoCreateAllParts_CheckedChanged(null, null);
            this.GetBalancebyWorkOrder();
            this.ChangeRowGridQty();
            this.CheckwithOri();
            this.Calpart();
        }

        private void GridQty_SelectionChanged(object sender, EventArgs e)
        {
            this.ChangeRowGridQty();
        }

        private void GridPattern_SelectionChanged(object sender, EventArgs e)
        {
            this.ChangeRightLabel();
            if (this.gridPattern.CurrentDataRow == null)
            {
                return;
            }

            string filter = $"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}";
            if (this.chkCombineSubprocess.Checked)
            {
                filter += $" and CombineSubprocessGroup = {this.gridPattern.CurrentDataRow["CombineSubprocessGroup"]}";
            }

            this.allpartTb.DefaultView.RowFilter = filter;
        }

        private void ChangeRowGridQty()
        {
            if (this.gridQty.CurrentDataRow == null || this.ArticleSizeTb == null)
            {
                return;
            }

            // 右上 依據中上 Ukey & 中上 No (iden)
            this.ArticleSizeTb.DefaultView.RowFilter = $"iden ={this.gridQty.CurrentDataRow["iden"]}";
            this.GridAutoResizeColumns();
        }

        private void GetBalancebyWorkOrder()
        {
            if (this.gridCutRef.CurrentDataRow == null || this.ArticleSizeTb == null)
            {
                this.labelBalanceValue.Text = string.Empty;
                return;
            }

            DataRow dr = this.gridCutRef.CurrentDataRow;
            this.labelBalanceValue.Text = (MyUtility.Convert.GetInt(dr["TTLCutQty"]) -
                MyUtility.Convert.GetInt(dr["CreatedBundleQty"]) -
                this.ArticleSizeTb.Select($"Ukey = '{dr["Ukey"]}'").AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted)
                .Select(s => MyUtility.Convert.GetInt(s["CutOutput"]))
                .Sum()).ToString();
        }

        /// <inheritdoc/>
        public int GetBalancebyDistribute(DataRow drAS, int newQty)
        {
            if (this.ArticleSizeTb == null)
            {
                return 0;
            }

            int no = MyUtility.Convert.GetInt(this.gridQty.CurrentDataRow["No"]);
            int realCutOutput = MyUtility.Convert.GetInt(drAS["RealCutOutput"]);

            // pkey 找到已分配原本是同一筆 WorkOrder_Distribute
            string curfilter = $"OrderID='{drAS["OrderID"]}'and Article='{drAS["Article"]}' and SizeCode='{drAS["SizeCode"]}' and isExcess='{drAS["isExcess"]}'";
            string filter = $"Pkey = '{drAS["pkey"]}' and ((No={no} and not ({curfilter}) ) or (No<>{no}))";
            int cutOutput = this.ArticleSizeTb.Select(filter).AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted)
                .Select(s => MyUtility.Convert.GetInt(s["CutOutput"]))
                .Sum()
                + newQty
                ;

            return cutOutput - realCutOutput;
        }

        private void CheckwithOri()
        {
            if (this.CutRefTb == null || this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            var pori = this.patternTbOri.Select($"ukey = '{this.gridCutRef.CurrentDataRow["ukey"]}'").AsEnumerable().Select(s => new
            {
                Cutref = MyUtility.Convert.GetString(s["Cutref"]),
                Poid = MyUtility.Convert.GetString(s["Poid"]),
                Ukey = MyUtility.Convert.GetLong(s["Ukey"]),
                PatternCode = MyUtility.Convert.GetString(s["PatternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["Ispair"]),
                Art = MyUtility.Convert.GetString(s["Art"]),
                NoBundleCardAfterSubprocess_String = MyUtility.Convert.GetString(s["NoBundleCardAfterSubprocess_String"]),
                PostSewingSubProcess_String = MyUtility.Convert.GetString(s["PostSewingSubProcess_String"]),
            }).OrderBy(o => o.PatternCode).ToList();

            var pnow = this.patternTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new
            {
                Cutref = MyUtility.Convert.GetString(s["cutref"]),
                Poid = MyUtility.Convert.GetString(s["poid"]),
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                PatternCode = MyUtility.Convert.GetString(s["PatternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["ispair"]),
                Art = MyUtility.Convert.GetString(s["art"]),
                NoBundleCardAfterSubprocess_String = MyUtility.Convert.GetString(s["NoBundleCardAfterSubprocess_String"]),
                PostSewingSubProcess_String = MyUtility.Convert.GetString(s["PostSewingSubProcess_String"]),
            }).Where(w => w.Ukey == MyUtility.Convert.GetLong(this.gridCutRef.CurrentDataRow["ukey"]))
            .OrderBy(o => o.PatternCode).ToList();

            var aori = this.allpartTbOri.Select($"ukey = '{this.gridCutRef.CurrentDataRow["ukey"]}'").AsEnumerable().Select(s => new
            {
                Cutref = MyUtility.Convert.GetString(s["Cutref"]),
                Poid = MyUtility.Convert.GetString(s["Poid"]),
                Ukey = MyUtility.Convert.GetLong(s["Ukey"]),
                PatternCode = MyUtility.Convert.GetString(s["PatternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["Ispair"]),
            }).OrderBy(o => o.PatternCode).ToList();

            var anow = this.allpartTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new
            {
                Cutref = MyUtility.Convert.GetString(s["cutref"]),
                Poid = MyUtility.Convert.GetString(s["poid"]),
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                PatternCode = MyUtility.Convert.GetString(s["PatternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["ispair"]),
            }).Where(w => w.Ukey == MyUtility.Convert.GetLong(this.gridCutRef.CurrentDataRow["ukey"]))
            .OrderBy(o => o.PatternCode).ToList();

            // 兩個不一樣, 顯示固定字串提醒
            this.lbinfo.Visible = pori.Except(pnow).Any() || pnow.Except(pori).Any() || aori.Except(anow).Any() || anow.Except(aori).Any();
        }
        #endregion

        private void GridAutoResizeColumns()
        {
            this.gridCutRef.AutoResizeColumns();
            this.gridQty.AutoResizeColumns();
            this.gridArticleSize.AutoResizeColumns();
            this.gridPattern.AutoResizeColumns();
            this.gridAllPart.AutoResizeColumns();
        }

        private void Gridvalid()
        {
            this.gridCutRef.ValidateControl();
            this.gridArticleSize.ValidateControl();
            this.gridQty.ValidateControl();
            this.gridPattern.ValidateControl();
            this.gridAllPart.ValidateControl();
        }

        private void NumNoOfBundle_Validating(object sender, CancelEventArgs e)
        {
            if (this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            if (this.numNoOfBundle.Value == 0)
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("No of Bundle must greater than 0");
                return;
            }

            if (this.numNoOfBundle.Value == this.numNoOfBundle.OldValue && this.numNoOfBundle_Validating)
            {
                return;
            }

            this.numNoOfBundle_Validating = true;

            // 記錄使用者輸入的 Tone
            DataTable tmpqtyTb = this.qtyTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").CopyToDataTable();

            // 對應中上的Key欄位 No 先清除, 右鍵選取時再重新寫入
            this.ArticleSizeTbOri.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").AsEnumerable().ToList().ForEach(row => row["No"] = 0);
            this.ArticleSizeTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").Delete();
            this.qtyTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").Delete();
            long m = this.qtyTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => MyUtility.Convert.GetLong(s["iden"])).DefaultIfEmpty(0).Max();
            for (int i = 1; i <= this.numNoOfBundle.Value; i++)
            {
                DataRow qty_newRow = this.qtyTb.NewRow();
                qty_newRow["No"] = i;
                qty_newRow["iden"] = ++m;
                qty_newRow["Ukey"] = this.gridCutRef.CurrentDataRow["Ukey"];
                qty_newRow["Tone"] = tmpqtyTb.Rows.Count >= i && !MyUtility.Check.Empty(tmpqtyTb.Rows[i - 1]["Tone"]) ? tmpqtyTb.Rows[i - 1]["Tone"] : this.tone;
                qty_newRow["ToneChar"] = MyUtility.Convert.GetInt(qty_newRow["Tone"]) == 0 ? string.Empty : MyUtility.Excel.ConvertNumericToExcelColumn(MyUtility.Convert.GetInt(qty_newRow["Tone"]));
                qty_newRow["StyleUkey"] = this.gridCutRef.CurrentDataRow["StyleUkey"];
                this.qtyTb.Rows.Add(qty_newRow);
            }

            this.GetBalancebyWorkOrder();
            this.ChangeRowGridQty();
        }

        private void Btn_LefttoRight_Click(object sender, EventArgs e)
        {
            this.Gridvalid();

            // 避免沒資料造成當機
            if (this.patternTb.Rows.Count < 1)
            {
                return;
            }

            DataRow selectartDr = ((DataRowView)this.gridPattern.GetSelecteds(SelectedSort.Index)[0]).Row;
            string pattern = selectartDr["PatternCode"].ToString();
            if (pattern == "ALLPARTS")
            {
                return;
            }

            // 移動此筆
            DataRow ndr = this.allpartTb.NewRow();
            ndr["PatternCode"] = selectartDr["PatternCode"];
            ndr["PatternDesc"] = selectartDr["PatternDesc"];
            ndr["Location"] = selectartDr["Location"];
            ndr["poid"] = selectartDr["poid"];
            ndr["Cutref"] = selectartDr["cutref"];
            ndr["Parts"] = selectartDr["Parts"];
            ndr["isPair"] = selectartDr["isPair"];
            ndr["Ukey"] = selectartDr["Ukey"];

            // Annotation
            DataRow[] adr = this.GarmentTb.Select($"PatternCode='{selectartDr["PatternCode"]}'");
            if (adr.Length > 0)
            {
                ndr["annotation"] = adr[0]["annotation"];
            }

            this.allpartTb.Rows.Add(ndr);
            selectartDr.Delete(); // 刪除此筆

            DataRow[] patterndr = this.patternTb.Select($"PatternCode='{pattern}'");
            DataRow[] artdr = this.artTb.Select($"PatternCode='{pattern}'");

            // 刪除後還有相同Pattern 需要判斷是否Subprocess都存在
            if (patterndr.Length > 0)
            {
                foreach (DataRow dr in patterndr)
                {
                    if (artdr.Length > 0)
                    {
                        foreach (DataRow dr2 in artdr)
                        {
                            try
                            {
                                if (dr["art"].ToString().IndexOf(dr2["subprocessid"].ToString()) == -1)
                                {
                                    dr2.Delete();
                                }
                            }
                            catch (Exception)
                            {
                                return;
                                throw;
                            }
                        }
                    }
                }
            }
            else
            { // 直接刪除全部同PatternCode 的Subprocessid
                foreach (DataRow dr in patterndr)
                {
                    dr.Delete();
                }
            }

            if (!this.patternTb.Select($"ukey = {this.gridCutRef.CurrentDataRow["ukey"]}").Any())
            {
                // 新增PatternTb
                DataRow ndr2 = this.patternTb.NewRow();
                ndr2["PatternCode"] = "ALLPARTS";
                ndr2["PatternDesc"] = "All Parts";
                ndr2["Location"] = string.Empty;
                ndr2["Parts"] = 0;
                ndr2["art"] = string.Empty;
                ndr2["poid"] = this.gridCutRef.CurrentDataRow["poid"];
                ndr2["Cutref"] = this.gridCutRef.CurrentDataRow["cutref"];
                ndr2["isPair"] = 0;
                ndr2["ukey"] = this.gridCutRef.CurrentDataRow["ukey"];
                this.patternTb.Rows.Add(ndr2);
            }

            this.Calpart();
        }

        private void Btn_RighttoLeft_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.patternTb.Rows.Count == 0 || this.gridAllPart.Rows.Count == 0)
            {
                return;
            }

            string filter = "sel = 1";
            if (this.chkCombineSubprocess.Checked)
            {
                filter += " and (IsMain <> 1 or CombineSubprocessGroup = 0)";
            }

            DataRow[] checkdr = this.allpartTb.Select(filter);
            #region 確認有勾選
            if (checkdr.Length > 0)
            {
                long ukey = MyUtility.Convert.GetLong(checkdr[0]["ukey"]);
                foreach (DataRow chdr in checkdr)
                {
                    bool isPair = MyUtility.Convert.GetBool(chdr["isPair"]);
                    if (this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}' and ukey = '{chdr["ukey"]}'").Count() > 0)
                    {
                        isPair = MyUtility.Convert.GetBool(this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}' and ukey = '{chdr["ukey"]}'")[0]["isPair"]);
                    }

                    // 新增PatternTb
                    DataRow ndr2 = this.patternTb.NewRow();
                    ndr2["PatternCode"] = chdr["PatternCode"];
                    ndr2["PatternDesc"] = chdr["PatternDesc"];
                    ndr2["Location"] = chdr["Location"];
                    ndr2["Parts"] = chdr["Parts"];
                    ndr2["art"] = "EMB";
                    ndr2["poid"] = chdr["poid"];
                    ndr2["Cutref"] = chdr["cutref"];
                    ndr2["isPair"] = isPair;
                    ndr2["ukey"] = chdr["ukey"];
                    ndr2["isMain"] = true;
                    int max = this.patternTb.Select($"ukey = {chdr["ukey"]}").Max(m => MyUtility.Convert.GetInt(m["CombineSubprocessGroup"]));
                    ndr2["CombineSubprocessGroup"] = max + 1;
                    this.patternTb.Rows.Add(ndr2);
                    if (this.chkCombineSubprocess.Checked)
                    {
                        chdr["CombineSubprocessGroup"] = max + 1;
                        chdr["isMain"] = true;
                    }
                    else
                    {
                        chdr.Delete();
                    }
                }

                this.ReAddALLPARTS(ukey);
                this.CombineSubprocessIspair(ukey);
            }

            this.Calpart();
            #endregion
        }

        private void Calpart() // 計算Parts,TotalParts
        {
            if (this.gridCutRef.CurrentDataRow == null || this.ArticleSizeTb == null)
            {
                return;
            }

            // Allpart 的 CombineSubprocessGroup=0
            string filterUkey = $"ukey = {this.gridCutRef.CurrentDataRow["ukey"]}";
            string filter_ALLPARTS = filterUkey + $" and CombineSubprocessGroup = 0";
            DataRow[] allpartdr = this.patternTb.Select($"PatternCode='ALLPARTS' and {filter_ALLPARTS}");
            if (allpartdr.Length > 0)
            {
                allpartdr[0]["Parts"] = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", filter_ALLPARTS));
            }

            DataRow[] patternDrs = this.patternTb.Select(filterUkey);
            if (this.chkCombineSubprocess.Checked)
            {
                foreach (DataRow dr in patternDrs)
                {
                    string fg = filterUkey + $" and CombineSubprocessGroup = {dr["CombineSubprocessGroup"]}";
                    dr["Parts"] = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", fg));
                }
            }

            int ttlallpart = MyUtility.Convert.GetInt(this.patternTb.Compute("Sum(Parts)", filterUkey));
            this.ArticleSizeTb.Select(filterUkey).ToList().ForEach(r => r["TotalParts"] = ttlallpart);
        }

        private void ReAddALLPARTS(long ukey)
        {
            // 維持 ALLPARTS 在最後一列, 以便建立時順序
            this.patternTb.AcceptChanges();
            var drs = this.patternTb.Select($"Ukey = {ukey} and PatternCode = 'ALLPARTS'");
            DataTable cdt = drs.TryCopyToDataTable(this.patternTb);
            drs.Delete();
            this.patternTb.Merge(cdt);
            this.patternTb.AcceptChanges();
        }

        #region 右鍵 Menu 新增/刪除
        private void InsertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            DataRow ndr = this.patternTb.NewRow();
            ndr["cutref"] = this.gridCutRef.CurrentDataRow["cutref"];
            ndr["ukey"] = this.gridCutRef.CurrentDataRow["ukey"];
            int max = this.patternTb.Select($"ukey = {ndr["ukey"]}").Max(m => MyUtility.Convert.GetInt(m["CombineSubprocessGroup"]));
            ndr["CombineSubprocessGroup"] = max + 1;
            ndr["isMain"] = true;
            this.patternTb.Rows.Add(ndr);

            if (this.chkCombineSubprocess.Checked)
            {
                DataRow adr = this.allpartTb.NewRow();
                adr["cutref"] = this.gridCutRef.CurrentDataRow["cutref"];
                adr["ukey"] = this.gridCutRef.CurrentDataRow["ukey"];
                adr["CombineSubprocessGroup"] = max + 1;
                adr["isMain"] = true;
                this.allpartTb.Rows.Add(adr);
            }
        }

        private void DeleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.gridPattern.CurrentDataRow["PatternCode"].ToString() == "ALLPARTS")
            {
                return;
            }

            if (this.chkCombineSubprocess.Checked)
            {
                this.allpartTb.Select($"Ukey = {this.gridPattern.CurrentDataRow["Ukey"]} and CombineSubprocessGroup = {this.gridPattern.CurrentDataRow["CombineSubprocessGroup"]}").Delete();
            }

            this.gridPattern.CurrentDataRow.Delete();
            this.Calpart();
        }

        private void Allpart_insert_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.gridPattern.CurrentDataRow == null || this.IschkNonShellAllPart())
            {
                return;
            }

            DataRow ndr = this.allpartTb.NewRow();
            ndr["cutref"] = this.gridCutRef.CurrentDataRow["cutref"];
            ndr["ukey"] = this.gridCutRef.CurrentDataRow["ukey"];
            ndr["CombineSubprocessGroup"] = this.chkCombineSubprocess.Checked ? this.gridPattern.CurrentDataRow["CombineSubprocessGroup"] : 0;
            ndr["isMain"] = false;
            this.allpartTb.Rows.Add(ndr);
        }

        private void Allpart_delete_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.gridAllPart.CurrentDataRow == null || this.IschkNonShellAllPart())
            {
                return;
            }

            if (this.chkCombineSubprocess.Checked && MyUtility.Convert.GetBool(this.gridAllPart.CurrentDataRow["isMain"]))
            {
                // 刪除右下資料,若點選是 isMain 那筆,則這組全部刪除
                this.allpartTb.Select($"Ukey = {this.gridPattern.CurrentDataRow["Ukey"]} and CombineSubprocessGroup = {this.gridPattern.CurrentDataRow["CombineSubprocessGroup"]}").Delete();
                this.gridPattern.CurrentDataRow.Delete();
            }
            else
            {
                this.gridAllPart.CurrentDataRow.Delete();
            }

            this.Calpart();
        }

        private void InsertAS_MenuItem_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.gridQty.CurrentDataRow == null)
            {
                return;
            }

            DataRow ndr = this.ArticleSizeTb.NewRow();
            ndr["Ukey"] = this.gridQty.CurrentDataRow["Ukey"];
            ndr["No"] = this.gridQty.CurrentDataRow["No"];
            ndr["iden"] = this.gridQty.CurrentDataRow["iden"];
            this.ArticleSizeTb.Rows.Add(ndr);
        }

        private void DeleteAS_MenuItem_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.gridArticleSize.CurrentDataRow == null)
            {
                return;
            }

            this.gridArticleSize.CurrentDataRow.Delete();
            this.CalGridQty();
            this.GetBalancebyWorkOrder();

            if (this.ArticleSizeTb.DefaultView.Count == 0)
            {
                DataRow row = this.gridQty.CurrentDataRow;
                row["Article"] = string.Empty;
                row["SizeCode"] = string.Empty;
            }
        }

        private bool IschkNonShellAllPart()
        {
            if ((this.chkNoneShellNoCreateAllParts.Checked && MyUtility.Convert.GetInt(this.gridPattern.CurrentDataRow["CombineSubprocessGroup"]) == 0) ||
                (this.chkNoneShellNoCreateAllParts.Checked && !this.chkCombineSubprocess.Checked))
            {
                return true;
            }

            return false;
        }
        #endregion

        private void ChkCombineSubprocess_CheckedChanged(object sender, EventArgs e)
        {
            this.btn_LefttoRight.Enabled = !this.chkCombineSubprocess.Checked;
            this.gridAllPart.Columns["Annotation"].Visible = !this.chkCombineSubprocess.Checked;
            this.gridPattern.Columns["IsPair"].Visible = !this.chkCombineSubprocess.Checked;
            this.ChangeRightLabel();
            if (this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            if (MyUtility.Convert.GetBool(this.gridCutRef.CurrentDataRow["IsCombineSubprocess", DataRowVersion.Original]) == this.chkCombineSubprocess.Checked)
            {
                return;
            }

            this.gridCutRef.CurrentDataRow["IsCombineSubprocess"] = this.chkCombineSubprocess.Checked;
            this.gridCutRef.CurrentDataRow.AcceptChanges();
            this.ChangeDefault();
            this.DeleteAllpartsDatas();
            this.GridAutoResizeColumns();
        }

        private void ChkNoneShellNoCreateAllParts_CheckedChanged(object sender, EventArgs e)
        {
            if (this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            if (MyUtility.Convert.GetBool(this.gridCutRef.CurrentDataRow["IsNoneShellNoCreateAllParts", DataRowVersion.Original]) == this.chkNoneShellNoCreateAllParts.Checked)
            {
                return;
            }

            this.gridCutRef.CurrentDataRow["IsNoneShellNoCreateAllParts"] = this.chkNoneShellNoCreateAllParts.Checked;
            this.gridCutRef.CurrentDataRow.AcceptChanges();

            this.ChangeDefault();
            this.DeleteAllpartsDatas();
            this.GridAutoResizeColumns();
        }

        private void ChangeRightLabel()
        {
            if (this.gridPattern.CurrentDataRow == null)
            {
                this.label5.Text = this.chkCombineSubprocess.Checked ? "Combine Subprocess Detail" : "All Parts Detail";
            }
            else
            {
                this.label5.Text = this.chkCombineSubprocess.Checked && MyUtility.Convert.GetString(this.gridPattern.CurrentDataRow["PatternCode"]) != "ALLPARTS" ?
                    "Combine Subprocess Detail" : "All Parts Detail";
            }
        }

        private void DeleteAllpartsDatas()
        {
            if (MyUtility.Convert.GetString(this.gridCutRef.CurrentDataRow["FabricKind"]) != "1" && this.chkNoneShellNoCreateAllParts.Checked)
            {
                this.allpartTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}  and CombineSubprocessGroup = 0").Delete();
                this.allpartTb.AcceptChanges();
                this.Calpart();
            }
        }

        private void BtnDefault_Click(object sender, EventArgs e)
        {
            this.chkCombineSubprocess.Checked = false;
            this.chkNoneShellNoCreateAllParts.Checked = false;
            this.ChangeDefault();
        }

        private void ChangeDefault()
        {
            if (this.CutRefTb == null || this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            long ukey = (long)this.gridCutRef.CurrentDataRow["ukey"];
            this.patternTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && (long)w["ukey"] == ukey).Delete();
            this.allpartTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && (long)w["ukey"] == ukey).Delete();
            this.patternTb.AcceptChanges();
            this.allpartTb.AcceptChanges();
            DataTable pdt = this.patternTb.Clone();
            DataTable adt = this.allpartTbOri.Select($"ukey = {ukey}").TryCopyToDataTable(this.allpartTb);
            DataTable xdt = this.patternTbOri.Select($"ukey = {ukey}").TryCopyToDataTable(this.patternTb);

            if (!MyUtility.Convert.GetBool(this.gridCutRef.CurrentDataRow["IsCombineSubprocess"]))
            {
                pdt = xdt;
            }
            else
            {
                pdt = xdt.Select($"isMain = 1").TryCopyToDataTable(this.patternTb);
                pdt.ImportRow(xdt.Select($"PatternCode = 'ALLPARTS'").FirstOrDefault());

                DataTable psdt = xdt.Select($"PatternCode <> 'ALLPARTS'").TryCopyToDataTable(this.patternTb);
                psdt.AsEnumerable().ToList().ForEach(f => this.allpartTb.ImportRow(f));
            }

            this.patternTb.Merge(pdt);
            this.allpartTb.Merge(adt);
            this.CombineSubprocessIspair(ukey);
            this.ReAddALLPARTS(ukey);
            this.CheckwithOri();
            this.Calpart();
        }

        private void CombineSubprocessIspair(long ukey)
        {
            if (MyUtility.Convert.GetBool(this.gridCutRef.CurrentDataRow["IsCombineSubprocess"]))
            {
                this.patternTb.Select($@"Ukey = {ukey}").ToList().ForEach(f => f["isPair"] = false);
            }
        }

        private void BtnGarmentList_Click(object sender, EventArgs e)
        {
            if (this.CutRefTb == null || this.CutRefTb.Rows.Count == 0 || this.ArticleSizeTb == null)
            {
                return;
            }

            DataRow dr = this.gridCutRef.CurrentDataRow;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", dr["poid"].ToString(), "Orders", "ID");
            var sizelist = this.ArticleSizeTbOri.Select($"Ukey = {dr["Ukey"]}").AsEnumerable().Select(s => s["SizeCode"].ToString()).Distinct().ToList();
            PublicForm.GarmentList callNextForm = new PublicForm.GarmentList(ukey, dr["poid"].ToString(), dr["Cutref"].ToString(), sizelist);
            callNextForm.ShowDialog(this);
        }

        private void BtnColorComb_Click(object sender, EventArgs e)
        {
            if (this.CutRefTb == null || this.CutRefTb.Rows.Count == 0)
            {
                return;
            }

            DataRow dr = this.gridCutRef.CurrentDataRow;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", dr["poid"].ToString(), "Orders", "ID");
            PublicForm.ColorCombination callNextForm = new PublicForm.ColorCombination(dr["poid"].ToString(), ukey);
            callNextForm.ShowDialog(this);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBatchCreate_Click(object sender, EventArgs e)
        {
            try
            {
                this.BatchCreateData();
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void BatchCreateData()
        {
            this.Gridvalid();

            #region Insert Table
            DataTable insert_BundleNo = new DataTable();
            insert_BundleNo.Columns.Add("BundleNo", typeof(string));
            #endregion

            // ukey, Article, SizeCode & 左下Bundle資訊 一樣, 則建立成同一張單. Tone (有輸入)也一樣時, 則 Allpart 那筆合一
            var ukeyList = this.CutRefTb.Select("sel=1").ToList().Select(s => MyUtility.Convert.GetLong(s["ukey"])).ToList(); // 有勾選的Ukey
            var qtydataList = this.qtyTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new NoofBundle
            {
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                No = MyUtility.Convert.GetInt(s["No"]),
                Iden = MyUtility.Convert.GetInt(s["iden"]),
                PrintGroup = -1,
                Tone = MyUtility.Convert.GetInt(s["tone"]),
                ToneChar = MyUtility.Convert.GetString(s["ToneChar"]),
                POID = MyUtility.Convert.GetString(s["POID"]),
                Article = MyUtility.Convert.GetString(s["Article"]),
                SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                Qty = MyUtility.Convert.GetInt(s["Qty"]),
                Dup = -1, // 紀錄是否完全一樣的組別 Ukey, Article, Size, 左下資料
                StyleUkey = MyUtility.Convert.GetLong(s["StyleUkey"]),
                SubCut = string.Empty,
            }).ToList();
            var asList = this.ArticleSizeTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new ArticleSize
            {
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                No = MyUtility.Convert.GetInt(s["No"]),
                Iden = MyUtility.Convert.GetInt(s["iden"]),
                Pkey = MyUtility.Convert.GetLong(s["Pkey"]),
                Cutref = MyUtility.Convert.GetString(s["Cutref"]),
                POID = MyUtility.Convert.GetString(s["POID"]),
                OrderID = MyUtility.Convert.GetString(s["OrderID"]),
                Article = MyUtility.Convert.GetString(s["Article"]),
                SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                IsEXCESS = MyUtility.Convert.GetString(s["IsEXCESS"]),
                ColorID = MyUtility.Convert.GetString(s["ColorID"]),
                Fabriccombo = MyUtility.Convert.GetString(s["Fabriccombo"]),
                FabricPanelCode = MyUtility.Convert.GetString(s["FabricPanelCode"]),
                Ratio = MyUtility.Convert.GetString(s["Ratio"]),
                Cutno = MyUtility.Convert.GetInt(s["Cutno"]),
                Sewingline = MyUtility.Convert.GetString(s["Sewingline"]),
                SewingCell = MyUtility.Convert.GetString(s["SewingCell"]),
                Item = MyUtility.Convert.GetString(s["Item"]),
                Qty = MyUtility.Convert.GetInt(s["Qty"]),
                Cutoutput = MyUtility.Convert.GetInt(s["Cutoutput"]),
                RealCutOutput = MyUtility.Convert.GetInt(s["RealCutOutput"]),
                TotalParts = MyUtility.Convert.GetInt(s["TotalParts"]),
                Startno = MyUtility.Convert.GetInt(s["Startno"]),
                StyleUkey = MyUtility.Convert.GetLong(s["StyleUkey"]),
            }).ToList();
            var patternList = this.patternTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new Pattern
            {
                Cutref = MyUtility.Convert.GetString(s["cutref"]),
                Poid = MyUtility.Convert.GetString(s["poid"]),
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                PatternCode = MyUtility.Convert.GetString(s["PatternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["ispair"]),
                Art = MyUtility.Convert.GetString(s["art"]),
                NoBundleCardAfterSubprocess_String = MyUtility.Convert.GetString(s["NoBundleCardAfterSubprocess_String"]),
                PostSewingSubProcess_String = MyUtility.Convert.GetString(s["PostSewingSubProcess_String"]),
                IsMain = MyUtility.Convert.GetBool(s["IsMain"]),
                CombineSubprocessGroup = MyUtility.Convert.GetInt(s["CombineSubprocessGroup"]),
            }).ToList();
            var allPartList = this.allpartTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new Pattern
            {
                Cutref = MyUtility.Convert.GetString(s["cutref"]),
                Poid = MyUtility.Convert.GetString(s["poid"]),
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                PatternCode = MyUtility.Convert.GetString(s["PatternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["ispair"]),
                IsMain = MyUtility.Convert.GetBool(s["IsMain"]),
                CombineSubprocessGroup = MyUtility.Convert.GetInt(s["CombineSubprocessGroup"]),
            }).ToList();
            var selList = qtydataList.Where(w => ukeyList.Contains(w.Ukey) && !w.Qty.Empty()).ToList(); // 要寫入的中上表
            var idenList = selList.Select(s => s.Iden).ToList();
            var selASList = asList.Where(w => ukeyList.Contains(w.Ukey) && idenList.Contains(w.Iden) && w.Qty > 0).ToList();
            var selpatternList = patternList.Where(w => ukeyList.Contains(w.Ukey) && w.Parts > 0).ToList(); // 要寫入的左下表
            var selallPartList = allPartList.Where(w => ukeyList.Contains(w.Ukey)).ToList(); // 要寫入的右下表

            if (!this.BeforeBarchCreate(idenList, selpatternList))
            {
                return;
            }

            // ISP20201755 因結構層次改變,下方兩grid直接對應左上,所以不用判斷同Ukey下每一組資料是否相同,故相同 Ukey,Article,Sizecode 都會標記相同Dup合併建單
            // 標記 dup 數字一樣為同一組需合併建立在同一張 P10
            foreach (var selq in selList)
            {
                // 若這筆已被前面標記(重複組),則跳下筆ID
                if (selq.Dup > -1)
                {
                    continue;
                }

                int maxDup = selList.Select(s => s.Dup).Max() + 1;
                selList.Where(w => w.Ukey == selq.Ukey && w.Article == selq.Article && w.SizeCode == selq.SizeCode).ToList().ForEach(f => f.Dup = maxDup);
            }

            var dupList = selList.Select(s => s.Dup).Distinct().OrderBy(o => o).ToList();

            foreach (var item in selList)
            {
                DataRow dr = this.CutRefTb.Select($"Ukey = {item.Ukey}").First();
                item.SubCut = Prgs.GetSubCutNo(dr["CutRef"].ToString(), dr["Fabriccombo"].ToString(), dr["FabricPanelCode"].ToString(), dr["Cutno"].ToString());
            }

            // 準備 Bundle.[StartNo], Bundle_Detail.[BundleGroup], 在同一個 POID 下,依序編碼
            dupList.ForEach(dup =>
            {
                string poid = selList.Where(w => w.Dup == dup).Select(s => s.POID).First();

                // 找出此此單 POID 下,編碼最大 BundleGroup,若還沒有編碼, 則去 DB 撈最大
                int maxBuundleGroup = selList.Where(w => w.POID == poid).Select(s => s.BuundleGroup).DefaultIfEmpty(0).Max();
                if (maxBuundleGroup.Empty())
                {
                    string sqlcmd = $@"
select max(BundleGroup)
from Bundle b WITH (NOLOCK)
inner join Bundle_Detail bd WITH (NOLOCK) on bd.Id = b.ID
where b.POID = '{poid}'
";
                    maxBuundleGroup = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlcmd));
                }

                maxBuundleGroup++;

                // 紀錄 Startno, 合併建單先給同樣 Startno
                selList.Where(w2 => w2.Dup == dup).ToList().ForEach(r => r.Startno = maxBuundleGroup);

                // 紀錄 BuundleGroup, 同 Tone(合併Allpart) 要相同 BuundleGroup. 排序 Tone 和下方準備 Bundle_Detail 排序一樣, 目的是讓 BundleNo 和 BuundleGroup 順序一起由小到大, 不影響資料正確性
                int beforeTone = 0;
                bool thisDup1st = true;
                selList.Where(w2 => w2.Dup == dup).OrderBy(o => o.Tone).ToList().ForEach(f =>
                {
                    if (thisDup1st)
                    {
                        thisDup1st = false;
                        f.BuundleGroup = maxBuundleGroup;
                    }
                    else
                    {
                        if (f.Tone == beforeTone && f.Tone > 0)
                        {
                            f.BuundleGroup = maxBuundleGroup;
                        }
                        else
                        {
                            maxBuundleGroup++;
                            f.BuundleGroup = maxBuundleGroup;
                        }
                    }

                    beforeTone = f.Tone;
                });
            });

            // Print Group 編碼
            int printGroup = 1;
            var beforeitem = selList.FirstOrDefault();
            foreach (var item in selList.OrderBy(o => o.Ukey).ThenBy(o => o.Dup).ThenBy(o => o.Tone))
            {
                // 不是建立在同一張 Bundle 則再從 1 開始
                if (!(beforeitem.Ukey == item.Ukey && beforeitem.Dup == item.Dup))
                {
                    printGroup = 1;
                }

                item.PrintGroup = printGroup++;
                beforeitem = item;
            }

            // 總建單數.
            int num_Bundle = selList.Select(s => s.Dup).Distinct().Count();

            // ALLPARTS 合併減少數
            int dallparts = selList.Where(w2 => w2.Tone > 0).GroupBy(g => new { g.Dup, g.Tone })
                .Select(s => new { s.Key.Dup, s.Key.Tone, ct = s.Count() - 1 }).Sum(s => s.ct);

            // 總建 BundleNo 數量
            int num_BundleNo = (patternList.Where(w => ukeyList.Contains(w.Ukey)).Count() * selList.Count()) - dallparts;

            // 批次取得 BundleN.ID, BundleNo
            List<string> id_list = MyUtility.GetValue.GetBatchID(this.keyWord + "BC", "Bundle", batchNumber: num_Bundle, sequenceMode: 2);
            List<string> bundleno_list = MyUtility.GetValue.GetBatchID(string.Empty, "Bundle_Detail", format: 3, checkColumn: "Bundleno", batchNumber: num_BundleNo, sequenceMode: 2);

            int idcount = 0;
            int bundlenoCount = 0;
            long beforeUkey = -1;
            StringBuilder insertSql = new StringBuilder();
            insertSql.Append("declare @inertkey TABLE ( Ukey bigint);\r\n");

            // 將 Dup 重複縮減, 為 Bundle 建立幾張 Cutting_P10, 準備寫入字串
            foreach (int dup in dupList)
            {
                var seldupList = selList.Where(w => w.Dup == dup).ToList();
                var first = seldupList.First();

                #region 各欄位值 集中區
                string bundleID = id_list[idcount];
                idcount++;
                DataRow drCut = this.CutRefTb.Select($"ukey = {first.Ukey}").First();
                int sizeRatio = this.SizeRatioTb.AsEnumerable()
                    .Where(w => MyUtility.Convert.GetLong(w["ukey"]) == first.Ukey && MyUtility.Convert.GetString(w["SizeCode"]) == first.SizeCode)
                    .Select(s => MyUtility.Convert.GetInt(s["Qty"])).FirstOrDefault();
                var firstAS = selASList.Where(w => w.Iden == first.Iden).OrderBy(o => o.OrderID).First();
                string sewingLine = firstAS.Sewingline.Empty() ? string.Empty : firstAS.Sewingline.Length > 2 ? firstAS.Sewingline.Substring(0, 2) : firstAS.Sewingline;
                bool isEXCESS = selASList.Where(w => seldupList.Select(s => s.Iden).Contains(w.Iden) && w.IsEXCESS == "Y").Any();
                bool byToneGenerate = selList.Where(w => w.Dup == dup && w.Tone == first.Tone).Count() > 1;
                int bundleQty = selList.Where(w => w.Dup == dup).Count(); // 合併建單筆數, 寫入 P10 表頭 No of Bundle
                #endregion

                // bundle
                insertSql.Append($@"
Insert Into Bundle
    (ID
    ,POID
    ,mDivisionid
    ,SizeCode
    ,Colorid
    ,Article
    ,PatternPanel
    ,Cutno
    ,cDate
    ,OrderID
    ,SewingLineid
    ,Item
    ,SewingCell
    ,Ratio
    ,Startno
    ,Qty
    ,AllPart
    ,CutRef
    ,AddName
    ,AddDate
    ,FabricPanelCode
    ,IsEXCESS
    ,ByToneGenerate
    ,SubCutNo)
values
    ('{bundleID}'
    ,'{drCut["POID"]}'
    ,'{this.keyWord}'
    ,'{first.SizeCode}'
    ,'{drCut["Colorid"]}'
    ,'{first.Article}'
    ,'{drCut["Fabriccombo"]}'
    ,{drCut["Cutno"]}
    ,GETDATE()
    ,'{firstAS.OrderID}'
    ,'{sewingLine}'
    , '{drCut["Item"]}'
    ,'{firstAS.SewingCell}'
    ,'{sizeRatio}'
    ,{first.Startno}
    ,{bundleQty}
    ,{firstAS.TotalParts}
    ,'{drCut["CutRef"]}'
    ,'{this.loginID}'
    ,GETDATE()
    ,'{drCut["FabricPanelCode"]}'
    ,'{isEXCESS}'
    ,'{byToneGenerate}'
    ,'{first.SubCut}');
");

                // Bundle_Detail_allpart
                foreach (var allPart in selallPartList.Where(w => w.Ukey == first.Ukey && w.CombineSubprocessGroup == 0))
                {
                    insertSql.Append($@"
Insert Into Bundle_Detail_allpart(ID, PatternCode, PatternDesc, Parts, isPair, Location) 
Values('{bundleID}', '{allPart.PatternCode}', '{allPart.PatternDesc}', '{allPart.Parts}', '{allPart.Ispair}', '{allPart.Location}');");
                }

                // 合併, 只有 bundle, Bundle_Detail_allpart 合併. 其它的資料表有幾組就按實寫入. 排序 Tone 和上方準備 BuundleGroup 排序一樣
                int bct = 1;
                int allPartPrintGroup = 0;
                foreach (var selitem in seldupList.OrderBy(o => o.Tone))
                {
                    // Bundle_Detail_Qty
                    insertSql.Append($@"
Insert into Bundle_Detail_qty(ID,SizeCode,Qty) Values('{bundleID}', '{selitem.SizeCode}', {selitem.Qty});");

                    // Bundle_Detail, Bundle_Detail_Art, &  P15才有的寫的 Bundle_Detail_Order
                    foreach (var pattern in selpatternList.Where(w => w.Ukey == selitem.Ukey))
                    {
                        // 若 tone 相同 寫入 Bundle_Detail 時 ALLPARTS 合為一筆寫入, 若有合併的 ALLPARTS, BundleNo 順序要在最後面
                        // Tone > 0 使用者有設定, 同 Tone 有兩筆以上, 此次 ALLPARTS 不是合併的最後一筆則跳過
                        // 合併 ALLPARTS 的 PrintGroup 取第一組
                        int printGroup_x = selitem.PrintGroup;
                        int sct = seldupList.Where(w => selitem.Tone > 0 && w.Tone == selitem.Tone).Count();
                        if (pattern.PatternCode.Equals("ALLPARTS") && sct > 1 && bct < sct)
                        {
                            if (allPartPrintGroup == 0)
                            {
                                allPartPrintGroup = selitem.PrintGroup;
                            }

                            bct++;
                            continue;
                        }

                        if (pattern.PatternCode.Equals("ALLPARTS"))
                        {
                            bct = 1;
                            if (allPartPrintGroup == 0)
                            {
                                allPartPrintGroup = selitem.PrintGroup;
                            }

                            printGroup_x = allPartPrintGroup;
                            allPartPrintGroup = 0;
                        }

                        string bundleNo = bundleno_list[bundlenoCount];
                        bundlenoCount++;

                        // 有寫入的 bundleNo 記錄下, 下方 GZ API 使用
                        DataRow bdr = insert_BundleNo.NewRow();
                        bdr["Bundleno"] = bundleNo;
                        insert_BundleNo.Rows.Add(bdr);

                        // 相同 Tone ,且 ALLPARTS 的數量總和
                        var selDTAPList = seldupList.Where(w => w.Tone == selitem.Tone && w.Tone > 0 && pattern.PatternCode.Equals("ALLPARTS")).ToList();
                        int bdQty = pattern.PatternCode.Equals("ALLPARTS") && selitem.Tone > 0 ? selDTAPList.Sum(s => s.Qty) : selitem.Qty;
                        insertSql.Append($@"
Insert into Bundle_Detail (ID, Bundleno, BundleGroup, PatternCode, PatternDesc, SizeCode, Qty, Parts, Farmin, Farmout, isPair, Location, Tone, PrintGroup)
Values
    ('{bundleID}'
    ,'{bundleNo}'
    ,{selitem.BuundleGroup}
    ,'{pattern.PatternCode}'
    ,'{pattern.PatternDesc.Replace("'", "''")}'
    ,'{selitem.SizeCode}'
    ,{bdQty}
    ,{pattern.Parts}
    ,0,0 -- Farmin, Farmout
    ,'{pattern.Ispair}'
    ,'{pattern.Location}'
    ,'{selitem.ToneChar}'
    ,{printGroup_x});
");

                        // Bundle_Detail_Art 將 Art 以+號拆開寫入, 且ALLPARTS 不寫入
                        if (!pattern.PatternCode.Equals("ALLPARTS"))
                        {
                            string[] ann = pattern.Art.Split('+');
                            for (int i = 0; i < ann.Length; i++)
                            {
                                bool nb = pattern.NoBundleCardAfterSubprocess_String.Split('+').Contains(ann[i]);
                                bool ps = pattern.PostSewingSubProcess_String.Split('+').Contains(ann[i]);
                                insertSql.Append($@"
Insert into Bundle_Detail_art (ID,Bundleno,Subprocessid,PatternCode,PostSewingSubProcess,NoBundleCardAfterSubprocess)
Values('{bundleID}','{bundleNo}','{ann[i]}','{pattern.PatternCode}','{ps}','{nb}');
");
                            }
                        }

                        // Bundle_Detail_Order
                        if (pattern.PatternCode.Equals("ALLPARTS") && selitem.Tone > 0)
                        {
                            var spSumQtyList = selASList.Where(w => selDTAPList.Select(s => s.Iden).Distinct().Contains(w.Iden))
                                .GroupBy(g => g.OrderID)
                                .Select(s => new { OrderID = s.Key, Cutoutput = s.Sum(su => su.Cutoutput) })
                                .ToList();
                            foreach (var idrAS in spSumQtyList)
                            {
                                insertSql.Append($@"
INSERT INTO [dbo].[Bundle_Detail_Order]([ID],[BundleNo],[OrderID],[Qty]) Values('{bundleID}','{bundleNo}','{idrAS.OrderID}','{idrAS.Cutoutput}')
");
                            }
                        }
                        else
                        {
                            foreach (var idrAS in selASList.Where(w => w.Iden == selitem.Iden))
                            {
                                insertSql.Append($@"
INSERT INTO [dbo].[Bundle_Detail_Order]([ID],[BundleNo],[OrderID],[Qty]) Values('{bundleID}','{bundleNo}','{idrAS.OrderID}','{idrAS.Cutoutput}')
");
                            }
                        }

                        // Bundle_Detail_CombineSubprocess
                        foreach (var allPart in selallPartList.Where(w => w.Ukey == first.Ukey && w.CombineSubprocessGroup == pattern.CombineSubprocessGroup && w.CombineSubprocessGroup > 0))
                        {
                            insertSql.Append($@"
INSERT INTO [dbo].[Bundle_Detail_CombineSubprocess]([ID],[BundleNo],[PatternCode],[PatternDesc],[Parts],[Location],[IsPair],[IsMain])
Values('{bundleID}','{bundleNo}', '{allPart.PatternCode}', '{allPart.PatternDesc}', '{allPart.Parts}', '{allPart.Location}', '{allPart.Ispair}','{allPart.IsMain}');");
                        }
                    }
                }

                insertSql.Append($@"
delete FtyStyleInnovation_Artwork
where FtyStyleInnovationUkey in(
    select ukey from FtyStyleInnovation
    where MDivisionID = '{Sci.Env.User.Keyword}' and StyleUkey = {first.StyleUkey} and FabricCombo = '{drCut["Fabriccombo"]}' and Article = '{first.Article}')

delete FtyStyleInnovation
where MDivisionID = '{Sci.Env.User.Keyword}' and StyleUkey = {first.StyleUkey} and FabricCombo = '{drCut["Fabriccombo"]}' and Article = '{first.Article}'

delete FtyStyleInnovationAllPart
where MDivisionID = '{Sci.Env.User.Keyword}' and StyleUkey = {first.StyleUkey} and FabricCombo = '{drCut["Fabriccombo"]}' and Article = '{first.Article}'
");

                // 寫入 [FtyStyleInnovation]
                foreach (var pattern in selpatternList.Where(w => w.Ukey == first.Ukey))
                {
                    insertSql.Append($@"
delete @inertkey
INSERT INTO [dbo].[FtyStyleInnovation]([MDivisionID],[StyleUkey],[FabricCombo],[Article],[PatternCode],[PatternDesc],[Location],[Parts],[IsPair])
output inserted.Ukey into @inertkey
VALUES ('{Sci.Env.User.Keyword}','{first.StyleUkey}','{drCut["Fabriccombo"]}','{first.Article}'
,'{pattern.PatternCode}','{pattern.PatternDesc}','{pattern.Location}','{pattern.Parts}','{pattern.Ispair}')");

                    if (!pattern.PatternCode.Equals("ALLPARTS"))
                    {
                        string[] ann = pattern.Art.Split('+');
                        for (int i = 0; i < ann.Length; i++)
                        {
                            bool nb = pattern.NoBundleCardAfterSubprocess_String.Split('+').Contains(ann[i]);
                            bool ps = pattern.PostSewingSubProcess_String.Split('+').Contains(ann[i]);
                            insertSql.Append($@"
Insert into FtyStyleInnovation_Artwork ([FtyStyleInnovationUkey],[SubprocessId],[PostSewingSubProcess],[NoBundleCardAfterSubprocess])
Values((select ukey from @inertkey),'{ann[i]}','{ps}','{nb}');
");
                        }
                    }
                }

                foreach (var allPart in selallPartList.Where(w => w.Ukey == first.Ukey && w.CombineSubprocessGroup == 0))
                {
                    insertSql.Append($@"
INSERT INTO [dbo].[FtyStyleInnovationAllPart]([MDivisionID],[StyleUkey],[Fabriccombo],[Article],[PatternCode],[PatternDesc],[Location],[Parts],[IsPair])
VALUES('{Sci.Env.User.Keyword}','{first.StyleUkey}','{drCut["Fabriccombo"]}','{first.Article}'
,'{allPart.PatternCode}','{allPart.PatternDesc}','{allPart.Location}','{allPart.Parts}','{allPart.Ispair}')");
                }

                if (MyUtility.Convert.GetBool(drCut["IsCombineSubProcess"]))
                {
                    insertSql.Append($@"
delete FtyStyleInnovationCombineSubprocess
where MDivisionID = '{Sci.Env.User.Keyword}' and StyleUkey = {first.StyleUkey} and FabricCombo = '{drCut["Fabriccombo"]}' and Article = '{first.Article}'
");

                    foreach (var allPart in selallPartList.Where(w => w.Ukey == first.Ukey && w.CombineSubprocessGroup > 0))
                    {
                        insertSql.Append($@"
INSERT INTO [dbo].[FtyStyleInnovationCombineSubprocess]([MDivisionID],[StyleUkey],[FabricCombo],[Article],[PatternCode],[PatternDesc],[Location],[Parts],[IsPair],[IsMain],[CombineSubprocessGroup])
VALUES('{Sci.Env.User.Keyword}','{first.StyleUkey}','{drCut["Fabriccombo"]}','{first.Article}'
,'{allPart.PatternCode}','{allPart.PatternDesc}','{allPart.Location}','{allPart.Parts}','{allPart.Ispair}','{allPart.IsMain}',{allPart.CombineSubprocessGroup})");
                    }
                }

                beforeUkey = first.Ukey;
            }

            DualResult result;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                if (!(result = DBProxy.Current.Execute(null, insertSql.ToString())))
                {
                    transactionscope.Dispose();
                    this.ShowErr(result);
                    return;
                }

                transactionscope.Complete();
            }

            #region sent data to GZ WebAPI
            List<BundleToAGV_PostBody> FunListBundle()
            {
                string sqlGetData = $@"
            select  bd.ID          ,
                    b.POID          ,
                    bd.BundleNo    ,
                    b.CutRef             ,
                    b.OrderID            ,
                    b.Article            ,
                    b.PatternPanel       ,
                    b.FabricPanelCode    ,
                    bd.PatternCode ,
                    bd.PatternDesc ,
                    bd.BundleGroup ,
                    bd.SizeCode    ,
                    bd.Qty         ,
                    b.SewingLineID       ,
                    b.AddDate
            from #tmp t
            inner join Bundle_Detail bd with (nolock) on t.BundleNo = bd.BundleNo
            inner join Bundle b with (nolock) on bd.ID = b.ID
            ";
                result = MyUtility.Tool.ProcessWithDatatable(insert_BundleNo, "BundleNo", sqlGetData, out DataTable dtBundleGZ);

                if (dtBundleGZ.Rows.Count > 0)
                {
                    return dtBundleGZ.AsEnumerable().Select(
                       dr => new BundleToAGV_PostBody()
                       {
                           ID = dr["ID"].ToString(),
                           POID = dr["POID"].ToString(),
                           BundleNo = dr["BundleNo"].ToString(),
                           CutRef = dr["CutRef"].ToString(),
                           OrderID = dr["OrderID"].ToString(),
                           Article = dr["Article"].ToString(),
                           PatternPanel = dr["PatternPanel"].ToString(),
                           FabricPanelCode = dr["FabricPanelCode"].ToString(),
                           PatternCode = dr["PatternCode"].ToString(),
                           PatternDesc = dr["PatternDesc"].ToString(),
                           BundleGroup = (decimal)dr["BundleGroup"],
                           SizeCode = dr["SizeCode"].ToString(),
                           Qty = (decimal)dr["Qty"],
                           SewingLineID = dr["SewingLineID"].ToString(),
                           AddDate = (DateTime?)dr["AddDate"],
                       })
                       .ToList();
                }
                else
                {
                    return null;
                }
            }

            Task.Run(() => new Guozi_AGV().SentBundleToAGV(FunListBundle))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            #endregion

            MyUtility.Msg.InfoBox("Successfully");
        }

        private bool BeforeBarchCreate(List<int> idenList, List<Pattern> selpatternList)
        {
            if (this.CutRefTb == null || this.CutRefTb.Rows.Count == 0)
            {
                return false;
            }

            DataRow[] cutrefAy = this.CutRefTb.Select("Sel=1");
            if (cutrefAy.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first !!");
                return false;
            }

            if (idenList.Count == 0)
            {
                return false;
            }

            // item 自動帶入有可能超過20碼
            if (cutrefAy.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["item"]).Length > 20).Any())
            {
                DataTable wdt = this.CutRefTb.Select("Sel=1 and len(item) > 20").TryCopyToDataTable(this.CutRefTb);
                MsgGridForm m = new MsgGridForm(wdt, "Item string length can not more 20", "Warning")
                {
                    Width = 800,
                };

                m.grid1.Columns[0].Visible = false;
                m.grid1.Columns[2].Visible = false;
                m.grid1.Columns[3].Visible = false;
                m.grid1.Columns[4].Visible = false;
                m.grid1.Columns[5].Visible = false;
                m.grid1.Columns[6].Visible = false;
                m.grid1.Columns[7].Width = 200;
                m.grid1.Columns[8].Visible = false;
                m.grid1.Columns[9].Visible = false;
                m.grid1.Columns[10].Visible = false;
                m.text_Find.Width = 140;
                m.btn_Find.Location = new Point(150, 6);
                m.btn_Find.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                this.FormClosing += (s, args) =>
                {
                    if (m.Visible)
                    {
                        m.Close();
                    }
                };
                m.ShowDialog(this);

                return false;
            }

            // 準備有勾選需要清單
            var ukeyList = cutrefAy.ToList().Select(s => MyUtility.Convert.GetLong(s["ukey"])).ToList();
            var patternList = this.patternTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && ukeyList.Contains(MyUtility.Convert.GetLong(w["ukey"]))).ToList();

            // 有勾選, 判斷Pattern(Cutpart_grid)的Artwork  不可為空
            if (patternList.TryCopyToDataTable(this.patternTb).Select("PatternCode<>'ALLPARTS' and (art='' or art is null) ").Any())
            {
                MyUtility.Msg.WarningBox("<Art> can not be empty!");
                return false;
            }

            // 檢查 如果IsPair =✔, 加總相同的 Cut Part 的 Parts, 必需>0且可以被2整除
            var samePairCt = patternList.Where(w => MyUtility.Convert.GetBool(w["isPair"])).GroupBy(g => new { CutPart = g["PatternCode"], ukey = g["ukey"] })
                .Select(s => new { s.Key.CutPart, s.Key.ukey, Parts = s.Sum(i => MyUtility.Convert.GetDecimal(i["Parts"])) }).ToList();
            if (samePairCt.Where(w => w.Parts % 2 != 0).Any())
            {
                DataTable dt = ListToDataTable.ToDataTable(samePairCt.Where(w => w.Parts % 2 != 0).ToList());
                string msg = @"The following bundle is pair, but Parts is not pair, please check Cut Part Parts";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return false;
            }

            // 要建立的單 左下表 至少一筆 Parts 數量大於0
            foreach (var ukey in ukeyList)
            {
                if (!selpatternList.Where(w => w.Ukey == ukey && w.Parts > 0).Any())
                {
                    MyUtility.Msg.WarningBox("Bundle Card info cannot be empty.");
                    return false;
                }
            }

            // 要建立的單 PatternCode 不能空
            if (selpatternList.Where(w => w.PatternCode.Empty()).Any())
            {
                MyUtility.Msg.WarningBox("Bundle Card info cannot be empty.");
                return false;
            }

            var allpartList = this.allpartTb.AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted && ukeyList.Contains(MyUtility.Convert.GetLong(w["ukey"]))).ToList();
            if (allpartList.Where(w => !MyUtility.Check.Empty(w["Parts"]) && MyUtility.Check.Empty(w["PatternCode"])).Any())
            {
                MyUtility.Msg.WarningBox("All Parts Detail CutPart cannot be empty.");
                return false;
            }

            return true;
        }

        private int LetterToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return 0;
            }

            columnName = columnName.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < columnName.Length; i++)
            {
                sum *= 26;
                sum += columnName[i] - 'A' + 1;
            }

            return sum;
        }

        private void BtnSpreadingStauts_Click(object sender, EventArgs e)
        {
            if (this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            P15.SpreadingDT = null;
            P15.SpreadingType = null;
            string filter = $"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}";
            string sizeRatio = this.SizeRatioTb.Select(filter)
                .Select(s => MyUtility.Convert.GetString(s["SizeCode"]) + "/" + MyUtility.Convert.GetString(s["Qty"]))
                .ToList().JoinToString(",");
            var spList = this.ArticleSizeTb.Select(filter).AsEnumerable().Select(s => new { OrderID = MyUtility.Convert.GetString(s["OrderID"]) }).Distinct().ToList();
            DataTable spDt = PublicPrg.ListToDataTable.ToDataTable(spList);
            var frm = new P15_SpreadingStauts(this.gridCutRef.CurrentDataRow, sizeRatio, spDt);
            DialogResult result = frm.ShowDialog();
            if (P15.SpreadingDT != null)
            {
                this.Spreading();
            }
        }

        private void Spreading()
        {
            var list = SpreadingDT.AsEnumerable().Select(s => new { NoofLayer = (int)s["NoofLayer"], ToneChar = s["ToneChar"].ToString() })
                .GroupBy(g => g.ToneChar).Select(s => new { ToneChar = s.Key, NoofLayer = s.Sum(su => su.NoofLayer) }).ToList();
            int ttlLayer = list.Sum(s => s.NoofLayer);
            int ttlRatio = this.SizeRatioTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").Sum(s => MyUtility.Convert.GetInt(s["Qty"]));
            int ttlQty = ttlLayer * ttlRatio;

            string filter = $"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}";
            int ttlbal = this.ArticleSizeTbOri.Select(filter).Sum(s => MyUtility.Convert.GetInt(s["cutoutput"]));
            if (ttlbal == 0)
            {
                return;
            }

            DataTable processArticleSizeTb = this.ArticleSizeTbOri.Select(filter).TryCopyToDataTable(this.ArticleSizeTbOri);

            // 若 By Buyer Delivery 且要裁的數量小於剩餘數，先從最晚日期的數量扣到一樣，之後分配與by Article相同
            int noCutQty = ttlbal - ttlQty;
            if (SpreadingType == "B" && noCutQty > 0)
            {
                foreach (DataRow item in processArticleSizeTb.AsEnumerable()
                            .OrderByDescending(o => MyUtility.Convert.GetDate(o["BuyerDelivery"]))
                            .ThenByDescending(o => MyUtility.Convert.GetString(o["orderid"])))
                {
                    int cutoutput = MyUtility.Convert.GetInt(item["cutoutput"]);
                    if (cutoutput >= noCutQty)
                    {
                        item["cutoutput"] = cutoutput - noCutQty;
                        noCutQty = 0;
                    }
                    else
                    {
                        item["cutoutput"] = 0;
                        noCutQty -= cutoutput;
                    }

                    if (noCutQty == 0)
                    {
                        break;
                    }
                }
            }

            processArticleSizeTb.Columns.Add("ToneChar", typeof(string));
            processArticleSizeTb.Columns.Add("running", typeof(bool));
            DataTable tmpArticleSizeTb = processArticleSizeTb.Clone();

            // by tone 分配
            string beforeArticle = string.Empty;
            string beforeSizeCode = string.Empty;
            int overNoofLayer = 0;
            int no = 0;
            foreach (var item in list)
            {
                overNoofLayer = item.NoofLayer;
                int toneQty = item.NoofLayer * ttlRatio;
                while (toneQty > 0)
                {
                    // 找到最早BuyerDelivery
                    // 前一輪數量還沒分配完的繼續
                    DataRow process1stBuyerDelivery = processArticleSizeTb.Select("running = 1 and cutoutput > 0").FirstOrDefault();
                    if (process1stBuyerDelivery == null)
                    {
                        // 前一輪 article 還沒分完的繼續
                        process1stBuyerDelivery = processArticleSizeTb.Select($"article = '{beforeArticle}' and cutoutput > 0").FirstOrDefault();
                        if (process1stBuyerDelivery == null)
                        {
                            process1stBuyerDelivery = processArticleSizeTb.Select("cutoutput > 0").AsEnumerable()
                            .OrderBy(o => MyUtility.Convert.GetDate(o["BuyerDelivery"]))
                            .ThenBy(o => MyUtility.Convert.GetString(o["orderid"])).FirstOrDefault();
                            if (process1stBuyerDelivery == null)
                            {
                                break;
                            }
                        }
                    }

                    string article = MyUtility.Convert.GetString(process1stBuyerDelivery["Article"]);

                    // 取數量最大那筆
                    // 先找上一輪沒分完(只會有1筆或0筆)
                    DataRow[] sameArticle = processArticleSizeTb.Select("running = 1 and cutoutput > 0");
                    if (sameArticle.Length == 0)
                    {
                        sameArticle = processArticleSizeTb.Select($"Article = '{article}'").AsEnumerable()
                            .OrderByDescending(o => MyUtility.Convert.GetInt(o["cutoutput"])).ToArray();
                    }

                    foreach (DataRow articleRow in sameArticle)
                    {
                        if (toneQty == 0)
                        {
                            break;
                        }

                        string sizeCode = MyUtility.Convert.GetString(articleRow["SizeCode"]);
                        articleRow["running"] = 1;
                        int cutoutput = MyUtility.Convert.GetInt(articleRow["cutoutput"]);
                        if (overNoofLayer > 0 && overNoofLayer < item.NoofLayer)
                        {
                            DataRow newrow = tmpArticleSizeTb.NewRow();
                            articleRow.CopyTo(newrow);
                            if (article != beforeArticle || beforeSizeCode != sizeCode)
                            {
                                no++;
                            }

                            newrow["No"] = no;
                            int mincutoutput = Math.Min(cutoutput, overNoofLayer);
                            newrow["cutoutput"] = mincutoutput;
                            newrow["ToneChar"] = item.ToneChar;
                            tmpArticleSizeTb.Rows.Add(newrow);
                            cutoutput -= mincutoutput;
                            toneQty -= mincutoutput;
                            overNoofLayer -= mincutoutput;
                        }

                        beforeArticle = article;
                        beforeSizeCode = sizeCode;
                        while (cutoutput >= item.NoofLayer && toneQty > 0)
                        {
                            DataRow newrow = tmpArticleSizeTb.NewRow();
                            articleRow.CopyTo(newrow);
                            newrow["No"] = ++no;
                            newrow["cutoutput"] = item.NoofLayer;
                            newrow["ToneChar"] = item.ToneChar;
                            tmpArticleSizeTb.Rows.Add(newrow);
                            cutoutput -= item.NoofLayer;
                            toneQty -= item.NoofLayer;
                        }

                        if (cutoutput > 0 && toneQty > 0)
                        {
                            DataRow newrow = tmpArticleSizeTb.NewRow();
                            articleRow.CopyTo(newrow);
                            if (overNoofLayer == 0 || overNoofLayer == item.NoofLayer)
                            {
                                no++;
                                overNoofLayer = item.NoofLayer;
                            }

                            newrow["No"] = no;
                            newrow["cutoutput"] = cutoutput;
                            newrow["ToneChar"] = item.ToneChar;
                            tmpArticleSizeTb.Rows.Add(newrow);
                            toneQty -= cutoutput;
                            overNoofLayer -= cutoutput;
                            cutoutput = 0;
                        }

                        articleRow["cutoutput"] = cutoutput;
                    }
                }
            }

            // 透過 NumNoOfBundle_Validating 產生 中上的資料，包含iden
            this.numNoOfBundle.Value = tmpArticleSizeTb.AsEnumerable().Max(m => MyUtility.Convert.GetInt(m["No"]));
            this.numNoOfBundle_Validating = false;
            this.NumNoOfBundle_Validating(null, null);
            int i = 0;

            foreach (DataRow dr in this.qtyTb.Select(filter))
            {
                tmpArticleSizeTb.Select($"No = {dr["No"]}").ToList().ForEach(f => f["iden"] = dr["iden"]);

                dr["POID"] = tmpArticleSizeTb.Rows[i]["POID"];
                dr["Article"] = tmpArticleSizeTb.Rows[i]["Article"];
                dr["SizeCode"] = tmpArticleSizeTb.Rows[i]["SizeCode"];
                dr["Qty"] = tmpArticleSizeTb.Select($"No = {dr["No"]}").AsEnumerable().Sum(sum => MyUtility.Convert.GetInt(sum["cutoutput"]));
                dr["ToneChar"] = tmpArticleSizeTb.Select($"No = {dr["No"]}").Select(s => MyUtility.Convert.GetString(s["ToneChar"])).FirstOrDefault();
                dr["Tone"] = this.LetterToNumber(MyUtility.Convert.GetString(dr["ToneChar"]));
                i++;
            }

            tmpArticleSizeTb.Columns.Remove("ToneChar");
            this.ArticleSizeTb.Merge(tmpArticleSizeTb);
            this.GetBalancebyWorkOrder();
            this.GridAutoResizeColumns();
            this.Calpart();
        }
    }
}
