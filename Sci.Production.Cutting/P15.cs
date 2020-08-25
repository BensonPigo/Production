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
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private readonly string tone;
        private DataTable CutRefTb;
        private DataTable qtyTb;
        private DataTable ArticleSizeTb;
        private DataTable ArticleSizeTbOri; // 此 Table 在 Query 之後不再變更, 只用來開窗選擇用
        private DataTable ExcessTb;
        private DataTable GarmentTb;
        private DataTable patternTb;
        private DataTable patternTbOri;
        private DataTable allpartTb;
        private DataTable allpartTbOri;
        private DataTable artTb;
        private DataTable SizeRatioTb;

        /// <inheritdoc/>
        public P15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.tone = MyUtility.GetValue.Lookup("select iif(AutoGenerateByTone=1,1,0) from SYSTEM");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string cmd_st = "Select 0 as sel,PatternCode,PatternDesc, '' as annotation,parts,'' as cutref,'' as poid, 0 as iden, ukey = 0,ispair ,Location from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out this.allpartTb);

            string pattern_cmd = "Select patternCode,PatternDesc,Parts,'' as art, '' as cutref,'' as poid, 0 as iden, ukey = 0, ispair ,Location,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_Detail WITH (NOLOCK) Where 1=0"; // 左下的Table
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
                    DataRow[] articleAry = this.ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", dr["Ukey"], dr["Fabriccombo"]));
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

            DataGridViewGeneratorNumericColumnSettings tone = new DataGridViewGeneratorNumericColumnSettings();
            tone.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.gridQty.GetDataRow(e.RowIndex);
                dr["Tone"] = e.FormattedValue.Empty() ? DBNull.Value : e.FormattedValue;
                dr.EndEdit();
            };
            #endregion
            #region 中上 gridQty
            this.gridQty.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridQty)
                .Numeric("No", header: "No", width: Widths.AnsiChars(1), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, settings: qtySizecell)
                .Numeric("Tone", header: "Tone", width: Widths.AnsiChars(1), settings: tone)
                ;
            this.gridQty.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridQty.Columns["Tone"].DefaultCellStyle.BackColor = Color.Pink;
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
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS" || e.Button != MouseButtons.Right)
                {
                    return;
                }

                SelectItem sele = new SelectItem(this.GarmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                if (sele.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                if (this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}' and iden = '{dr["iden"]}'").Count() > 0)
                {
                    dr["isPair"] = this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}' and iden = '{dr["iden"]}'")[0]["isPair"];
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
                dr["parts"] = 1;
                dr.EndEdit();
                this.CheckNotMain(dr);
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                if (dr["PatternCode"].ToString() == patcode)
                {
                    return;
                }

                if (this.patternTb.Select($@"PatternCode = '{patcode}' and iden = '{dr["iden"]}'").Count() > 0)
                {
                    dr["isPair"] = this.patternTb.Select($@"PatternCode = '{patcode}' and iden = '{dr["iden"]}'")[0]["isPair"];
                }

                DataRow[] gemdr = this.GarmentTb.Select(string.Format("PatternCode ='{0}'", patcode), string.Empty);
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
                    dr["parts"] = 1;
                }

                dr.EndEdit();
                this.Calpart();
                this.CheckNotMain(dr);
            };

            DataGridViewGeneratorTextColumnSettings patternDesc = new DataGridViewGeneratorTextColumnSettings
            {
                CharacterCasing = CharacterCasing.Normal,
            };
            patternDesc.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                dr["PatternDesc"] = e.FormattedValue;
                dr.EndEdit();
                this.CheckNotMain(dr);
            };

            DataGridViewGeneratorTextColumnSettings subcell = new DataGridViewGeneratorTextColumnSettings();
            subcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
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
                    this.CheckNotMain(dr);
                }
            };

            DataGridViewGeneratorNumericColumnSettings partQtyCell = new DataGridViewGeneratorNumericColumnSettings();
            partQtyCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.Calpart();
            };

            DataGridViewGeneratorCheckBoxColumnSettings isPair = new DataGridViewGeneratorCheckBoxColumnSettings();
            isPair.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(dr["PatternCode"]).ToUpper() == "ALLPARTS")
                {
                    return;
                }

                bool ispair = MyUtility.Convert.GetBool(e.FormattedValue);
                dr["IsPair"] = ispair;
                dr.EndEdit();
                this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}' and iden = '{dr["iden"]}'").ToList().ForEach(r => r["IsPair"] = ispair);
            };

            DataGridViewGeneratorTextColumnSettings postSewingSubProcess_String = new DataGridViewGeneratorTextColumnSettings();
            postSewingSubProcess_String.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["art"]))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
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
                }
            };
            postSewingSubProcess_String.CellFormatting += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
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
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (MyUtility.Check.Empty(dr["art"]))
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
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
                }
            };
            noBundleCardAfterSubprocess_String.CellFormatting += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
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
            this.gridCutpart.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridCutpart)
                .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
                .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15), settings: patternDesc)
                .Text("Location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: subcell)
                .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell)
                .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: isPair)
                .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: postSewingSubProcess_String)
                .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: noBundleCardAfterSubprocess_String)
                ;
            this.gridCutpart.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutpart.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutpart.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCutpart.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCutpart.Columns["art"].DefaultCellStyle.BackColor = Color.SkyBlue;
            this.gridCutpart.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCutpart.Columns["IsPair"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region 右下 gridAllPart 事件
            DataGridViewGeneratorTextColumnSettings patterncell2 = new DataGridViewGeneratorTextColumnSettings();
            patterncell2.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridAllPart.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
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
                    dr["parts"] = 1;
                    dr.EndEdit();
                    this.Calpart();
                }
            };

            DataGridViewGeneratorNumericColumnSettings partQtyCell2 = new DataGridViewGeneratorNumericColumnSettings();
            partQtyCell2.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridAllPart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.Calpart();
            };
            #endregion
            #region 右下 gridAllPart
            this.gridAllPart.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridAllPart)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
                .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13))
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

            // 取得已選過資料在不同 No 下 Qty 加總
            var sel = this.ArticleSizeTb.Select(filter).AsEnumerable().GroupBy(g => new { Pkey = MyUtility.Convert.GetLong(g["Pkey"]) })
                .Select(s => new { s.Key.Pkey, Qty = s.Sum(sum => MyUtility.Convert.GetInt(sum["cutoutput"])) }).ToList();
            foreach (DataRow row in allAS.Rows)
            {
                int selQty = sel.Where(w => w.Pkey == (long)row["Pkey"]).Select(s => s.Qty).FirstOrDefault();
                if (!MyUtility.Convert.GetBool(row["sel"]))
                {
                    row["cutoutput"] = MyUtility.Convert.GetInt(row["RealCutOutput"]) - selQty;
                }
                else
                {
                    row["cutoutput"] = seleAS.Where(w => (long)w["Pkey"] == (long)row["Pkey"])
                        .Select(s => MyUtility.Convert.GetInt(s["cutoutput"])).FirstOrDefault();
                }

                if (MyUtility.Check.Empty(row["cutoutput"]))
                {
                    row.Delete();
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
            this.gridCutpart.DataSource = null;
            this.gridAllPart.DataSource = null;
            this.gridQty.DataSource = null;
            this.gridArticleSize.DataSource = null;
            this.gridCutRef.DataSource = null;
        }

        private bool BeforeQuery()
        {
            if (!MyUtility.Check.Empty(this.txtCutRef.Text) && !MyUtility.Check.Empty(this.dateEstCutDate.Value) && !MyUtility.Check.Empty(this.txtPOID.Text))
            {
                MyUtility.Msg.WarningBox("[CutRef#] or [Est. Cut Date] or [PO ID] can’t be empty!!");
                return false;
            }

            return true;
        }

        private void Query()
        {
            string cutref = this.txtCutRef.Text;
            string cutdate = this.dateEstCutDate.Value == null ? string.Empty : this.dateEstCutDate.Value.Value.ToShortDateString();
            string poid = this.txtPOID.Text;
            string factory = this.txtfactoryByM.Text;
            string where = MyUtility.Check.Empty(cutref) ? string.Empty : Environment.NewLine + $"and a.cutref='{cutref}'";
            where += MyUtility.Check.Empty(cutdate) ? string.Empty : Environment.NewLine + $"and a.estcutdate='{cutdate}'";
            where += MyUtility.Check.Empty(poid) ? string.Empty : Environment.NewLine + $"and ord.poid='{poid}'";
            where += MyUtility.Check.Empty(factory) ? string.Empty : Environment.NewLine + $"and ord.FtyGroup='{factory}'";
            string distru_where = this.chkAEQ.Checked ? string.Empty : " and b.orderid <>'EXCESS'";
            this.gridArticleSize.Columns["isEXCESS"].Visible = this.chkAEQ.Checked;

            // 左上
            string query_cmd = $@"
Select
	 sel = cast(0 as bit)
	, a.cutref
	, ord.poid
	, a.estcutdate
	, a.Fabriccombo
	, a.FabricPanelCode
	, a.cutno
	, item.item
	, a.SpreadingNoID
	, a.colorid
	, a.Ukey
	, FabricKind.FabricKind
	, TTLCutQty = (select SUM(qty) from WorkOrder_Distribute b with(nolock) where b.WorkOrderUkey = a.Ukey {distru_where})
from  workorder a WITH (NOLOCK) 
inner join orders ord WITH (NOLOCK) on ord.ID = a.id and ord.cuttingsp = a.id
outer apply(
	Select item = Reason.Name 
	from Reason WITH (NOLOCK)
	inner join Style WITH (NOLOCK) on Style.ApparelType = Reason.id
	where Reason.Reasontypeid = 'Style_Apparel_Type' 
	and Style.ukey = ord.styleukey 
)item
outer apply (SELECT TOP 1 b.patternpanel FROM workorder_PatternPanel b WITH (NOLOCK) WHERE a.ukey = b.workorderukey)b
outer apply(
    SELECT TOP 1 FabricKind = DD.id + '-' + DD.NAME 
    FROM order_colorcombo OCC WITH (NOLOCK)
	inner join order_bof OB WITH (NOLOCK) on OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
	inner join  dropdownlist DD WITH (NOLOCK) on  DD.id = OB.kind
    WHERE OCC.id = a.id
	and OCC.patternpanel = b.patternpanel
	and DD.[type] = 'FabricKind'

)FabricKind
where ord.mDivisionid = '{this.keyWord}' 
and isnull(a.CutRef,'') <> ''
{where}
order by ord.poid,a.estcutdate,a.Fabriccombo,a.cutno
";
            DualResult query_dResult = DBProxy.Current.Select(null, query_cmd, out this.CutRefTb);
            if (!query_dResult)
            {
                this.ShowErr(query_cmd, query_dResult);
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
	, a.Ukey
	, No = DENSE_RANK() over (partition by a.ukey order by article.article,b.sizecode) -- 對應 GridQty 的欄位
	, iden = 0
	, Pkey = ROW_NUMBER() over (order by b.sizecode,b.orderid,a.FabricPanelCode) -- 為 workorder_Distribute 的 Key, 計算已選總和用
	, a.cutref
	, orderid = iif(b.OrderID = 'EXCESS', isnull(l.orderid,l2.OrderID), b.OrderID)
	, article.article
	, sizecode.sizecode
	, isEXCESS = iif(b.OrderID = 'EXCESS','Y','')
	, a.colorid
	, a.Fabriccombo
	, a.FabricPanelCode
	, Ratio = ''
	, a.cutno
	, Sewingline = ord.SewLine
	, SewingCell= a.CutCellid
	, item.item
	, Qty = 1
	, cutoutput = isnull(b.Qty,0)
	, RealCutOutput = isnull(b.Qty,0)
	, TotalParts = 0
	, ord.poid
	, startno = 0
	, ord.StyleUkey
from workorder a WITH (NOLOCK) 
inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey
inner join orders ord WITH (NOLOCK) on ord.ID = a.id and ord.cuttingsp = a.id
outer apply(
	select top 1 wd.OrderID,wd.Article,wd.SizeCode
	from workorder_Distribute wd WITH(NOLOCK)
	where wd.WorkOrderUkey = a.Ukey and wd.orderid <>'EXCESS' and wd.SizeCode = b.SizeCode 
	order by wd.OrderID desc
)l
outer apply(
	select top 1 wd.OrderID,wd.Article,wd.SizeCode
	from workorder_Distribute wd WITH(NOLOCK)
	where wd.WorkOrderUkey = a.Ukey and wd.orderid <>'EXCESS'
	order by wd.OrderID desc
)l2
outer apply(
	Select item = Reason.Name 
	from Reason WITH (NOLOCK)
	inner join Style WITH (NOLOCK) on Style.ApparelType = Reason.id
	where Reason.Reasontypeid = 'Style_Apparel_Type' 
	and Style.ukey = ord.styleukey 
)item
outer apply(select article = iif(b.OrderID = 'EXCESS',isnull(l.article,l2.article),b.article))article
outer apply(select sizecode = iif(b.OrderID = 'EXCESS',isnull(l.sizecode,l2.sizecode),b.sizecode))sizecode
Where isnull(a.CutRef,'') <> ''
and ord.mDivisionid = '{this.keyWord}'
{where}
{distru_where}
order by article.article,b.sizecode,b.orderid,a.FabricPanelCode
";
            query_dResult = DBProxy.Current.Select(null, distru_cmd, out this.ArticleSizeTb);
            if (!query_dResult)
            {
                this.ShowErr(distru_cmd, query_dResult);
                return;
            }

            string sizeRatio = $@"
Select distinct a.ukey, ws.SizeCode, ws.Qty
from workorder a WITH (NOLOCK) 
inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey
inner join orders ord WITH (NOLOCK) on  ord.ID = a.id and ord.cuttingsp = a.id
inner join WorkOrder_SizeRatio ws WITH (NOLOCK) on ws.WorkOrderUkey = a.Ukey and ws.SizeCode = b.SizeCode
Where isnull(a.CutRef,'') <> '' 
and ord.mDivisionid = '{this.keyWord}'
{where}
";
            query_dResult = DBProxy.Current.Select(null, sizeRatio, out this.SizeRatioTb);
            if (!query_dResult)
            {
                this.ShowErr(distru_cmd, query_dResult);
                return;
            }

            this.patternTbOri = this.patternTb.Clone();
            this.allpartTbOri = this.allpartTb.Clone();
            this.CutRefTb.AsEnumerable().ToList().ForEach(dr => this.CreatePattern(dr)); // 先依據左上資料建立下方兩個資料表

            // 中上每一筆下塞入一組由(iden)對應的下方資料
            this.qtyTb = this.GetNoofBundle(); // 依據右上撈出資料彙整出中上
            this.qtyTb.AsEnumerable().ToList().ForEach(r => this.AddPatternAllpart(MyUtility.Convert.GetLong(r["ukey"]), MyUtility.Convert.GetLong(r["iden"])));
            foreach (DataRow dr in this.ArticleSizeTb.Rows)
            {
                dr["iden"] = this.qtyTb.Select($"ukey={dr["ukey"]} and no={dr["no"]}")[0]["iden"];
                dr["TotalParts"] = this.patternTb.Compute("sum(Parts)", $"iden ={dr["iden"]}");
            }

            this.ArticleSizeTbOri = this.ArticleSizeTb.Copy(); // 紀錄第一次撈出資料
            this.gridCutRef.DataSource = this.CutRefTb; // 左上
            this.gridQty.DataSource = this.qtyTb; // 中上
            this.gridArticleSize.DataSource = this.ArticleSizeTb; // 右上
            this.gridCutpart.DataSource = this.patternTb; // 左下
            this.gridAllPart.DataSource = this.allpartTb; // 右下

            this.GridAutoResizeColumns();
            this.ShowExcessDatas(where);
        }

        private void AddPatternAllpart(long ukey, long iden)
        {
            DataTable dtp = this.patternTbOri.Select($"Ukey = {ukey}").TryCopyToDataTable(this.patternTbOri);
            DataTable dta = this.allpartTbOri.Select($"Ukey = {ukey}").TryCopyToDataTable(this.allpartTbOri);
            dtp.AsEnumerable().ToList().ForEach(r => r["iden"] = iden);
            dta.AsEnumerable().ToList().ForEach(r => r["iden"] = iden);
            this.patternTb.Merge(dtp);
            this.allpartTb.Merge(dta);
        }

        private DataTable GetNoofBundle()
        {
            // by Article, Size 整理出中上 No of Bundle 的資料表, 並從 1 開始依序給 No 值 (index). 唯一值:Ukey, No
            var result = this.ArticleSizeTb.AsEnumerable()
                .GroupBy(s => new { Ukey = (long)s["Ukey"], No = (long)s["No"], POID = s["POID"].ToString(), Article = s["Article"].ToString(), SizeCode = s["SizeCode"].ToString() })
                .Select((g, i) => new { g.Key.Ukey, iden = ++i, g.Key.No, Tone = this.tone, g.Key.POID, g.Key.Article, g.Key.SizeCode, Qty = g.Sum(s => (decimal?)s["cutoutput"]) })
                .OrderBy(o => o.Article)
                .ThenBy(o => o.SizeCode)
                .ToList();

            return ListToDataTable.ToDataTable(result);
        }

        private void ShowExcessDatas(string where)
        {
            string excess_cmd = $@"
Select  distinct a.cutref, a.orderid
from workorder a WITH (NOLOCK) 
inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey
inner join orders ord WITH (NOLOCK) on ord.ID = a.id and ord.cuttingsp = a.id
Where ord.mDivisionid = '{this.keyWord}'   
and isnull(a.CutRef,'') <> '' 
and b.orderid = 'EXCESS' 
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

            string tablecreatesql = string.Format(@"Select '{0}' as orderid,a.*,'' as F_CODE", poid);
            foreach (DataRow dr in headertb.Rows)
            {
                tablecreatesql += string.Format(" ,'' as {0}", dr["ArticleGroup"]);
            }

            tablecreatesql += string.Format(" from Pattern_GL a WITH (NOLOCK) Where PatternUkey = '{0}'", patternukey);
            DualResult tablecreateResult = DBProxy.Current.Select(null, tablecreatesql, out DataTable garmentListTb);
            if (!tablecreateResult)
            {
                return;
            }

            string lecsql = string.Empty;
            lecsql = string.Format("Select * from Pattern_GL_LectraCode a WITH (NOLOCK) where a.PatternUkey = '{0}'", patternukey);
            DualResult drre = DBProxy.Current.Select(null, lecsql, out DataTable drtb);
            if (!drre)
            {
                return;
            }

            foreach (DataRow dr in garmentListTb.Rows)
            {
                DataRow[] lecdrar = drtb.Select(string.Format("SEQ = '{0}'", dr["SEQ"]));
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
            w.Append(string.Format("orderid = '{0}' and (1=0", poid));
            foreach (DataRow dr in headertb.Rows)
            {
                w.Append(string.Format(" or {0} = '{1}' ", dr[0], patternpanel));
            }

            w.Append(")");
            DataRow[] garmentar = this.GarmentTb.Select(w.ToString());
            foreach (DataRow dr in garmentar)
            {
                // 若無 annotation 直接寫入 allpartTbOri
                if (MyUtility.Check.Empty(dr["annotation"]))
                {
                    DataRow ndr = this.allpartTbOri.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["Location"] = dr["Location"];
                    ndr["parts"] = MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                    ndr["Cutref"] = cutref;
                    ndr["POID"] = poid;
                    ndr["ukey"] = ukey;
                    ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                    this.allpartTbOri.Rows.Add(ndr);
                    npart = npart + MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                }
                else
                {
                    // 取得哪些 annotation 是次要
                    List<string> notMainList = this.GetNotMain(dr, garmentar);
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
                            ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
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
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                        this.allpartTbOri.Rows.Add(ndr);
                    }
                    #endregion
                }
            }

            DataRow pdr = this.patternTbOri.NewRow(); // 預設要有ALLPARTS
            pdr["PatternCode"] = "ALLPARTS";
            pdr["PatternDesc"] = "All Parts";
            pdr["Location"] = string.Empty;
            pdr["parts"] = npart;
            pdr["Cutref"] = cutref;
            pdr["POID"] = poid;
            pdr["ukey"] = ukey;
            this.patternTbOri.Rows.Add(pdr);

            DBProxy.Current.DefaultTimeout = 0;
        }
        #endregion

        #region 上方2個 Grid RowChange
        private void GridCutRef_SelectionChanged(object sender, EventArgs e)
        {
            if (this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            // 中上,右上TTL值 依據左上 Ukey
            this.qtyTb.DefaultView.RowFilter = $"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}";
            this.numNoOfBundle.Value = this.qtyTb.DefaultView.Count;
            this.labelToalCutOutputValue.Text = this.gridCutRef.CurrentDataRow["TTLCutQty"].ToString();
            this.GetBalancebyWorkOrder();
            this.ChangeRowGridQty();
        }

        private void GridQty_SelectionChanged(object sender, EventArgs e)
        {
            this.ChangeRowGridQty();
        }

        private void ChangeRowGridQty()
        {
            if (this.gridQty.CurrentDataRow == null || this.ArticleSizeTb == null)
            {
                return;
            }

            // 右上 依據中上 Ukey & 中上 No
            string filter = $"iden ={this.gridQty.CurrentDataRow["iden"]}";
            this.ArticleSizeTb.DefaultView.RowFilter = filter;
            this.patternTb.DefaultView.RowFilter = filter;
            this.allpartTb.DefaultView.RowFilter = filter;
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
            this.labelBalanceValue.Text = (this.ArticleSizeTb.Select($"Ukey = '{dr["Ukey"]}'").AsEnumerable()
                .Where(w => w.RowState != DataRowState.Deleted)
                .Select(s => MyUtility.Convert.GetInt(s["CutOutput"]))
                .Sum()
                 - MyUtility.Convert.GetInt(dr["TTLCutQty"])).ToString();
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
        #endregion

        private void GridAutoResizeColumns()
        {
            this.gridCutRef.AutoResizeColumns();
            this.gridQty.AutoResizeColumns();
            this.gridArticleSize.AutoResizeColumns();
            this.gridCutpart.AutoResizeColumns();
            this.gridAllPart.AutoResizeColumns();
        }

        private void Gridvalid()
        {
            this.gridCutRef.ValidateControl();
            this.gridArticleSize.ValidateControl();
            this.gridQty.ValidateControl();
            this.gridCutpart.ValidateControl();
            this.gridAllPart.ValidateControl();
        }

        private void NumNoOfBundle_Validating(object sender, CancelEventArgs e)
        {
            if (this.numNoOfBundle.Value == 0)
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("No of Bundle must greater than 0");
                return;
            }

            if (this.numNoOfBundle.Value == this.numNoOfBundle.OldValue || this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            // 記錄使用者輸入的 Tone
            DataTable tmpqtyTb = this.qtyTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").CopyToDataTable();

            // 對應中上的Key欄位 No 先清除, 右鍵選取時再重新寫入
            this.ArticleSizeTbOri.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").AsEnumerable().ToList().ForEach(row => row["No"] = 0);
            this.ArticleSizeTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").Delete();
            this.qtyTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").Delete();
            this.patternTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").Delete();
            this.allpartTb.Select($"Ukey = {this.gridCutRef.CurrentDataRow["Ukey"]}").Delete();
            long m = this.qtyTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => MyUtility.Convert.GetLong(s["iden"])).DefaultIfEmpty(0).Max();
            for (int i = 1; i <= this.numNoOfBundle.Value; i++)
            {
                DataRow qty_newRow = this.qtyTb.NewRow();
                qty_newRow["No"] = i;
                qty_newRow["iden"] = ++m;
                qty_newRow["Ukey"] = this.gridCutRef.CurrentDataRow["Ukey"];
                qty_newRow["Tone"] = tmpqtyTb.Rows.Count >= i && !MyUtility.Check.Empty(tmpqtyTb.Rows[i - 1]["Tone"]) ? tmpqtyTb.Rows[i - 1]["Tone"] : this.tone;
                this.qtyTb.Rows.Add(qty_newRow);
                this.AddPatternAllpart(MyUtility.Convert.GetLong(qty_newRow["ukey"]), MyUtility.Convert.GetLong(qty_newRow["iden"]));
            }

            this.allpartTb.DefaultView.RowFilter = "1=0";
            this.patternTb.DefaultView.RowFilter = "1=0";
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

            DataRow selectartDr = ((DataRowView)this.gridCutpart.GetSelecteds(SelectedSort.Index)[0]).Row;
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
            ndr["iden"] = selectartDr["iden"];
            ndr["poid"] = selectartDr["poid"];
            ndr["Cutref"] = selectartDr["cutref"];
            ndr["Parts"] = selectartDr["Parts"];
            ndr["isPair"] = selectartDr["isPair"];
            ndr["Ukey"] = selectartDr["Ukey"];

            // Annotation
            DataRow[] adr = this.GarmentTb.Select(string.Format("PatternCode='{0}'", selectartDr["patternCode"]));
            if (adr.Length > 0)
            {
                ndr["annotation"] = adr[0]["annotation"];
            }

            this.allpartTb.Rows.Add(ndr);
            selectartDr.Delete(); // 刪除此筆

            DataRow[] patterndr = this.patternTb.Select(string.Format("PatternCode='{0}'", pattern));
            DataRow[] artdr = this.artTb.Select(string.Format("PatternCode='{0}'", pattern));

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

            this.Calpart();
        }

        private void Btn_RighttoLeft_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.patternTb.Rows.Count == 0 || this.gridAllPart.Rows.Count == 0)
            {
                return;
            }

            DataRow[] checkdr = this.allpartTb.Select("sel=1");
            #region 確認有勾選
            if (checkdr.Length > 0)
            {
                foreach (DataRow chdr in checkdr)
                {
                    string art = string.Empty;
                    string[] ann = Regex.Replace(chdr["annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation
                    if (ann.Length > 0)
                    {
                        #region 算Subprocess
                        art = Prgs.BundleCardCheckSubprocess(ann, chdr["PatternCode"].ToString(), this.artTb, out bool lallpart);
                        #endregion
                    }

                    bool isPair = MyUtility.Convert.GetBool(chdr["isPair"]);
                    if (this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}' and iden = '{chdr["iden"]}'").Count() > 0)
                    {
                        isPair = MyUtility.Convert.GetBool(this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}' and iden = '{chdr["iden"]}'")[0]["isPair"]);
                    }

                    // 新增PatternTb
                    DataRow ndr2 = this.patternTb.NewRow();
                    ndr2["PatternCode"] = chdr["PatternCode"];
                    ndr2["PatternDesc"] = chdr["PatternDesc"];
                    ndr2["Location"] = chdr["Location"];
                    ndr2["iden"] = chdr["iden"];
                    ndr2["Parts"] = chdr["Parts"];
                    ndr2["art"] = "EMB";
                    ndr2["poid"] = chdr["poid"];
                    ndr2["Cutref"] = chdr["cutref"];
                    ndr2["isPair"] = isPair;
                    ndr2["ukey"] = chdr["ukey"];
                    this.patternTb.Rows.Add(ndr2);
                    chdr.Delete();
                }
            }

            this.Calpart();
            #endregion
        }

        private void Calpart() // 計算Parts,TotalParts
        {
            if (this.gridQty.CurrentDataRow == null || this.ArticleSizeTb == null)
            {
                return;
            }

            DataRow dr = this.gridQty.CurrentDataRow;
            string filter = $"iden={dr["iden"]}";
            int allpart = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", filter));
            this.ArticleSizeTb.Select(filter).ToList().ForEach(r => r["TotalParts"] = allpart);
            DataRow[] allpartdr = this.patternTb.Select($"PatternCode='ALLPARTS' and {filter}");
            if (allpartdr.Length > 0)
            {
                allpartdr[0]["Parts"] = allpart;
            }

            int ttlallpart = MyUtility.Convert.GetInt(this.patternTb.Compute("Sum(Parts)", filter));
            this.ArticleSizeTb.Select(filter).ToList().ForEach(r => r["TotalParts"] = ttlallpart);
        }

        #region 右鍵 Menu 新增/刪除
        private void InsertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            DataRow ndr = this.patternTb.NewRow();
            ndr["iden"] = this.gridQty.CurrentDataRow["iden"];
            ndr["cutref"] = this.gridCutRef.CurrentDataRow["cutref"];
            this.patternTb.Rows.Add(ndr);
        }

        private void DeleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.gridCutpart.CurrentDataRow["PatternCode"].ToString() == "ALLPARTS")
            {
                return;
            }

            this.gridCutpart.CurrentDataRow.Delete();
            this.Calpart();
        }

        private void Allpart_insert_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            DataRow ndr = this.allpartTb.NewRow();
            ndr["iden"] = this.gridQty.CurrentDataRow["iden"];
            ndr["cutref"] = this.gridCutRef.CurrentDataRow["cutref"];
            this.allpartTb.Rows.Add(ndr);
        }

        private void Allpart_delete_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.gridAllPart.CurrentDataRow == null)
            {
                return;
            }

            this.gridAllPart.CurrentDataRow.Delete();
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
        #endregion

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
            this.Gridvalid();

            #region Insert Table
            DataTable insert_BundleNo = new DataTable();
            insert_BundleNo.Columns.Add("BundleNo", typeof(string));
            #endregion

            // ukey, Article, SizeCode & 左下Bundle資訊 一樣, 則建立成同一張單. Tone (有輸入)也一樣時, 則 Allpart 那筆合一
            var ukeyList = this.CutRefTb.Select("sel=1").ToList().Select(s => MyUtility.Convert.GetLong(s["ukey"])).ToList(); // 有勾選的Ukey
            var qtydataList = this.qtyTb.AsEnumerable().Select(s => new NoofBundle
            {
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                No = MyUtility.Convert.GetInt(s["No"]),
                Iden = MyUtility.Convert.GetInt(s["iden"]),
                Tone = MyUtility.Convert.GetInt(s["tone"]),
                POID = MyUtility.Convert.GetString(s["POID"]),
                Article = MyUtility.Convert.GetString(s["Article"]),
                SizeCode = MyUtility.Convert.GetString(s["SizeCode"]),
                Qty = MyUtility.Convert.GetInt(s["Qty"]),
                Dup = -1, // 紀錄是否完全一樣的組別 Ukey, Article, Size, 左下資料
                Ran = false,
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
                Iden = MyUtility.Convert.GetInt(s["iden"]),
                PatternCode = MyUtility.Convert.GetString(s["patternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["ispair"]),
                Art = MyUtility.Convert.GetString(s["art"]),
                NoBundleCardAfterSubprocess_String = MyUtility.Convert.GetString(s["NoBundleCardAfterSubprocess_String"]),
                PostSewingSubProcess_String = MyUtility.Convert.GetString(s["PostSewingSubProcess_String"]),
                BuundleGroup = 0,
                Ran = false,
            }).ToList();
            var allPartList = this.allpartTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Select(s => new Pattern
            {
                Cutref = MyUtility.Convert.GetString(s["cutref"]),
                Poid = MyUtility.Convert.GetString(s["poid"]),
                Ukey = MyUtility.Convert.GetLong(s["ukey"]),
                Iden = MyUtility.Convert.GetInt(s["iden"]),
                PatternCode = MyUtility.Convert.GetString(s["patternCode"]),
                PatternDesc = MyUtility.Convert.GetString(s["PatternDesc"]),
                Location = MyUtility.Convert.GetString(s["Location"]),
                Parts = MyUtility.Convert.GetInt(s["Parts"]),
                Ispair = MyUtility.Convert.GetBool(s["ispair"]),
            }).ToList();
            var selList = qtydataList.Where(w => ukeyList.Contains(w.Ukey) && !w.Qty.Empty()).ToList(); // 要寫入的中上表
            var idenList = selList.Select(s => s.Iden).ToList();
            var selASList = asList.Where(w => ukeyList.Contains(w.Ukey) && idenList.Contains(w.Iden)).ToList();
            var selpatternList = patternList.Where(w => ukeyList.Contains(w.Ukey) && idenList.Contains(w.Iden) && w.Parts > 0).ToList(); // 要寫入的左下表
            var selallPartList = allPartList.Where(w => ukeyList.Contains(w.Ukey) && idenList.Contains(w.Iden)).ToList(); // 要寫入的右下表

            if (!this.BeforeBarchCreate(idenList, selpatternList))
            {
                return;
            }

            // 標記 dup 數字一樣為同一組需合併建立在同一張 P10
            foreach (var selq in selList)
            {
                // 若這筆已被前面標記(重複組),則跳下筆ID
                if (selq.Dup > -1)
                {
                    continue;
                }

                int maxDup = selList.Select(s => s.Dup).Max() + 1;
                selq.Dup = maxDup;

                // 當前這筆向下比較, 去 iden 欄位, 比較不同 iden 的資料組是否完全一樣
                var sourList = selpatternList.Where(w => w.Iden == selq.Iden).Select(s => new
                {
                    s.Cutref,
                    s.Poid,
                    s.Ukey,
                    s.PatternCode,
                    s.PatternDesc,
                    s.Location,
                    s.Parts,
                    s.Ispair,
                    s.Art,
                    s.NoBundleCardAfterSubprocess_String,
                    s.PostSewingSubProcess_String,
                }).OrderBy(o => o.PatternCode).ToList();

                foreach (var nnext in selList.Where(w => w.Iden != selq.Iden && w.Dup == -1 && w.Ukey == selq.Ukey && w.Article == selq.Article && w.SizeCode == selq.SizeCode))
                {
                    var otherList = selpatternList.Where(w => w.Iden == nnext.Iden).Select(s => new
                    {
                        s.Cutref,
                        s.Poid,
                        s.Ukey,
                        s.PatternCode,
                        s.PatternDesc,
                        s.Location,
                        s.Parts,
                        s.Ispair,
                        s.Art,
                        s.NoBundleCardAfterSubprocess_String,
                        s.PostSewingSubProcess_String,
                    }).OrderBy(o => o.PatternCode).ToList();

                    // A, B 完全一樣
                    if (!(sourList.Except(otherList).Any() || otherList.Except(sourList).Any()))
                    {
                        nnext.Dup = maxDup;
                    }
                }
            }

            var dupList = selList.Select(s => s.Dup).Distinct().OrderBy(o => o).ToList();

            // 準備 Bundle.[StartNo], Bundle_Detail.[BundleGroup], 在同一個 POID 下,依序編碼
            dupList.ForEach(dup =>
            {
                string poid = selList.Where(w => w.Dup == dup).Select(s => s.POID).First();

                // 找出此此單 POID 下,編碼最大 BundleGroup,若還沒有編碼, 則去 DB 撈最大
                int maxBuundleGroup = selpatternList.Where(w => w.Poid == poid).Select(s => s.BuundleGroup).DefaultIfEmpty(0).Max();
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

                maxBuundleGroup = this.radiobegin1.Checked ? 1 : maxBuundleGroup + 1;

                // 紀錄 Startno, 合併建單先給同樣 Startno
                selList.Where(w2 => w2.Dup == dup).ToList().ForEach(r => r.Startno = maxBuundleGroup);

                // 紀錄 BuundleGroup, 同 Tone(合併Allpart) 要相同 BuundleGroup. 排序 Tone 和下方準備 Bundle_Detail 排序一樣, 目的是讓 BundleNo 和 BuundleGroup 順序一起由小到大, 不影響資料正確性
                selList.Where(w2 => w2.Dup == dup).OrderBy(o => o.Tone).ToList().ForEach(f =>
                {
                    // 先找相同 Tone 的編碼
                    int toneBG = selpatternList
                    .Where(w2 => selList.Where(w => w.Dup == f.Dup && w.Tone == f.Tone && w.Tone > 0).Select(s => s.Iden).Contains(w2.Iden))
                    .Select(s => s.BuundleGroup).DefaultIfEmpty(0).Max();
                    if (toneBG.Empty())
                    {
                        selpatternList.Where(w => w.Iden == f.Iden).ToList().ForEach(r => r.BuundleGroup = maxBuundleGroup);
                        maxBuundleGroup++;
                    }
                    else
                    {
                        selpatternList.Where(w => w.Iden == f.Iden).ToList().ForEach(r => r.BuundleGroup = toneBG);
                    }
                });
            });

            // 總建單數.
            int num_Bundle = selList.Select(s => s.Dup).Distinct().Count();

            // ALLPARTS 合併減少數
            int dallparts = selList.Where(w2 => w2.Tone > 0).GroupBy(g => new { g.Dup, g.Tone }).Select(s => new { s.Key.Dup, s.Key.Tone, ct = s.Count() - 1 }).Sum(s => s.ct);

            // 總建 BundleNo 數量
            int num_BundleNo = selpatternList.Count() - dallparts;

            // 批次取得 BundleN.ID, BundleNo
            List<string> id_list = MyUtility.GetValue.GetBatchID(this.keyWord + "BC", "Bundle", batchNumber: num_Bundle, sequenceMode: 2);
            List<string> bundleno_list = MyUtility.GetValue.GetBatchID(string.Empty, "Bundle_Detail", format: 3, checkColumn: "Bundleno", batchNumber: num_BundleNo, sequenceMode: 2);

            int idcount = 0;
            int bundlenoCount = 0;
            StringBuilder insertSql = new StringBuilder();

            // 將 Dup 重複縮減, 為 Bundle 建立幾張 Cutting_P10, 準備寫入字串
            foreach (int dup in dupList)
            {
                var seldupList = selList.Where(w => w.Dup == dup).ToList();
                var first = seldupList.First();

                #region 各欄位值 集中區
                string bundleID = id_list[idcount];
                idcount++;
                DataRow drCut = this.CutRefTb.Select($"ukey = {first.Ukey}").First();
                DataRow drRatio = this.SizeRatioTb.Select($"ukey = '{first.Ukey}' and SizeCode ='{first.SizeCode}'").First();
                var firstAS = selASList.Where(w => w.Iden == first.Iden).OrderBy(o => o.OrderID).First();
                string sewingLine = firstAS.Sewingline.Empty() ? string.Empty : firstAS.Sewingline.Length > 2 ? firstAS.Sewingline.Substring(0, 2) : firstAS.Sewingline;
                bool isEXCESS = selASList.Where(w => w.Iden == first.Iden && w.IsEXCESS == "Y").Any();
                bool byToneGenerate = selList.Where(w => w.Dup == dup && w.Tone == first.Tone).Count() > 1;
                int bundleQty = seldupList.Where(w => w.Tone == 0).Count() + seldupList.Where(w => w.Tone > 0).Select(s => s.Tone).Distinct().Count(); // BundleGroup 數
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
    ,ByToneGenerate)
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
    ,'{drRatio["Qty"]}'
    ,{first.Startno}
    ,{bundleQty}
    ,{firstAS.TotalParts}
    ,'{drCut["CutRef"]}'
    ,'{this.loginID}'
    ,GETDATE()
    ,'{drCut["FabricPanelCode"]}'
    ,'{isEXCESS}'
    ,'{byToneGenerate}');
");

                // Bundle_Detail_allpart
                foreach (var allPart in selallPartList.Where(w => w.Iden == first.Iden && !w.Ran))
                {
                    insertSql.Append($@"
Insert Into Bundle_Detail_allpart(ID, PatternCode, PatternDesc, Parts, isPair, Location) 
Values('{bundleID}', '{allPart.PatternCode}', '{allPart.PatternDesc}', '{allPart.Parts}', '{allPart.Ispair}', '{allPart.Location}');");
                }

                selallPartList.Where(w => seldupList.Select(s => s.Iden).Contains(w.Iden)).ToList().ForEach(r => r.Ran = true);

                // 合併, 只有 bundle, Bundle_Detail_allpart 合併. 其它的資料表有幾組就按實寫入. 排序 Tone 和上方準備 BuundleGroup 排序一樣
                int beforeTone = 0;
                foreach (var selitem in seldupList.OrderBy(o => o.Tone))
                {
                    int currTone = selitem.Tone;

                    // Bundle_Detail_Qty
                    insertSql.Append($@"
Insert into Bundle_Detail_qty(ID,SizeCode,Qty) Values('{bundleID}', '{selitem.SizeCode}', {selitem.Qty});");

                    // Bundle_Detail, Bundle_Detail_Art, &  P15才有的寫的 Bundle_Detail_Order
                    foreach (var pattern in selpatternList.Where(w => w.Iden == selitem.Iden))
                    {
                        // 若 dup, tone 相同 寫入Bundle_Detail 時 PatternCode = ALLPARTS 合為一筆 Qty 數量加起來. 且標記已準備
                        var selDTAPList = seldupList.Where(w => w.Tone == selitem.Tone && w.Tone > 0 && pattern.PatternCode.Equals("ALLPARTS")).ToList();
                        var selptternDTAllPartList = selpatternList.Where(w => selDTAPList.Select(s => s.Iden).Contains(w.Iden)).ToList();
                        if (selptternDTAllPartList.Where(w => !w.Ran).Count() > 1)
                        {
                            pattern.Ran = true;
                            continue;
                        }

                        pattern.Ran = true;
                        string bundleNo = bundleno_list[bundlenoCount];
                        bundlenoCount++;

                        // 有寫入的 bundleNo 記錄下, 下方 GZ API 使用
                        DataRow bdr = insert_BundleNo.NewRow();
                        bdr["Bundleno"] = bundleNo;
                        insert_BundleNo.Rows.Add(bdr);

                        int bdQty = pattern.PatternCode.Equals("ALLPARTS") && selitem.Tone > 0 ? selDTAPList.Sum(s => s.Qty) : selitem.Qty;
                        insertSql.Append($@"
Insert into Bundle_Detail (ID, Bundleno, BundleGroup, PatternCode, PatternDesc, SizeCode, Qty, Parts, Farmin, Farmout, isPair, Location)
Values
    ('{bundleID}'
    ,'{bundleNo}'
    ,{pattern.BuundleGroup}
    ,'{pattern.PatternCode}'
    ,'{pattern.PatternDesc.Replace("'", "''")}'
    ,'{selitem.SizeCode}'
    ,{bdQty}
    ,{pattern.Parts}
    ,0,0 -- Farmin, Farmout
    ,'{pattern.Ispair}'
    ,'{pattern.Location}');
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
                        if (selptternDTAllPartList.Count() > 1)
                        {
                            var spSumQtyList = selASList.Where(w => selptternDTAllPartList.Select(s => s.Iden).Distinct().Contains(w.Iden))
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
                    }

                    beforeTone = selitem.Tone;
                }
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
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);

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
            var samePairCt = patternList.Where(w => MyUtility.Convert.GetBool(w["isPair"])).GroupBy(g => new { CutPart = g["PatternCode"], iden = g["iden"] })
                .Select(s => new { s.Key.CutPart, s.Key.iden, Parts = s.Sum(i => MyUtility.Convert.GetDecimal(i["Parts"])) }).ToList();
            if (samePairCt.Where(w => w.Parts % 2 != 0).Any())
            {
                DataTable dt = ListToDataTable.ToDataTable(samePairCt.Where(w => w.Parts % 2 != 0).ToList());
                dt.Columns.Remove("iden");
                string msg = @"The following bundle is pair, but parts is not pair, please check Cut Part parts";
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return false;
            }

            // 要建立的單 左下表 至少一筆 Parts 數量大於0
            foreach (var iden in idenList)
            {
                if (!selpatternList.Where(w => w.Iden == iden && w.Parts != 0).Any())
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

            var allpartList = this.allpartTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted && ukeyList.Contains(MyUtility.Convert.GetLong(w["ukey"]))).ToList();
            if (allpartList.Where(w => !MyUtility.Check.Empty(w["Parts"]) && MyUtility.Check.Empty(w["PatternCode"])).Any())
            {
                MyUtility.Msg.WarningBox("All Parts Detail CutPart cannot be empty.");
                return false;
            }

            return true;
        }

        private List<string> GetNotMain(DataRow dr, DataRow[] drs)
        {
            List<string> annList = new List<string>();
            if (MyUtility.Convert.GetBool(dr["Main"]))
            {
                return annList;
            }

            string[] ann = MyUtility.Convert.GetString(dr["annotation"]).Split('+'); // 剖析Annotation 不去除數字 EX:AT01

            // 每一筆 Annotation 去回找是否有標記主裁片
            foreach (string item in ann)
            {
                string anno = Regex.Replace(item, @"[\d]", string.Empty);

                // 判斷此 Annotation 在Cutting B01 是否為 IsBoundedProcess
                string sqlcmd = $@"select 1 from Subprocess with(nolock) where id = '{anno}' and IsBoundedProcess =1 ";
                bool isBoundedProcess = MyUtility.Check.Seek(sqlcmd);

                // 是否有主裁片存在
                bool hasMain = drs.AsEnumerable().
                    Where(w => MyUtility.Convert.GetString(w["annotation"]).Split('+').Contains(item) && MyUtility.Convert.GetBool(w["Main"])).Any();

                if (isBoundedProcess && hasMain)
                {
                    annList.Add(anno); // 去除字串中數字並加入List
                }
            }

            return annList;
        }

        private void CheckNotMain(DataRow dr)
        {
            // 1.先判斷 PatternCode + PatternDesc 是否存在 GarmentTb
            // 2.判斷選擇的 Artwork  EX:選擇 AT+HT, 在PatternCode + PatternDes找到 HT+AT01, 才算此筆為 GarmentTb 內的資料
            // 3.判斷是否為次要裁
            DataRow[] drs = this.GarmentTb.Select($"PatternCode='{dr["PatternCode"]}'and PatternDesc = '{dr["PatternDesc"]}'");
            if (drs.Length == 0)
            {
                dr["NoBundleCardAfterSubprocess_String"] = string.Empty;
                dr.EndEdit();
                return;
            }

            DataRow dr1 = drs[0]; // 找到也只會有一筆
            string[] ann = Regex.Replace(dr1["annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation 去除字串中數字
            string[] anns = dr["art"].ToString().Split('+'); // 剖析Annotation, 已經是去除數字

            // 兩個陣列內容要完全一樣，不管順序
            if (!this.CompareArr(ann, anns))
            {
                dr["NoBundleCardAfterSubprocess_String"] = string.Empty;
                dr.EndEdit();
                return;
            }

            List<string> notMainList = this.GetNotMain(dr1, this.GarmentTb.Select()); // 帶入未去除數字的annotation資料
            string noBundleCardAfterSubprocess_String = string.Join("+", notMainList);
            dr["NoBundleCardAfterSubprocess_String"] = noBundleCardAfterSubprocess_String;
            dr.EndEdit();
        }

        private bool CompareArr(string[] arr1, string[] arr2)
        {
            var q = from a in arr1 join b in arr2 on a equals b select a;
            bool flag = arr1.Length == arr2.Length && q.Count() == arr1.Length;

            return flag; // 內容相同返回true,反之返回false。
        }
    }
}
