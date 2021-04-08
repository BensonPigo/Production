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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Automation.Guozi_AGV;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P11 : Win.Tems.QueryForm
    {
        private readonly string loginID = Env.User.UserID;
        private readonly string keyWord = Env.User.Keyword;
        private bool isCombineSubProcess;
        private bool isNoneShellNoCreateAllParts;
        private DataTable CutRefTb;
        private DataTable ArticleSizeTb;
        private DataTable ExcessTb;
        private DataTable GarmentTb;
        private DataTable patternTb;
        private DataTable patternTbOri; // 此 Table 在 Query 之後不再變更
        private DataTable allpartTb;
        private DataTable allpartTbOri; // 此 Table 在 Query 之後不再變更
        private DataTable artTb;
        private DataTable qtyTb;
        private DataTable SizeRatioTb;
        private DataTable headertb;
        private DataTable ArticleSizeTb_View;

        /// <inheritdoc/>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string cmd_st = "Select 0 as sel,PatternCode,PatternDesc, '' as annotation,parts,'' as cutref,'' as poid, 0 as iden,IsPair ,Location,isMain = cast(0 as bit),CombineSubprocessGroup = cast(0 as tinyint)  from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out this.allpartTb);

            string pattern_cmd = "Select patternCode,PatternDesc,Parts,'' as art,0 AS parts, '' as cutref,'' as poid, 0 as iden,IsPair ,Location,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='',isMain = cast(0 as bit),CombineSubprocessGroup = cast(0 as tinyint)  from Bundle_Detail WITH (NOLOCK) Where 1=0"; // 左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out this.patternTb);

            string cmd_art = "Select PatternCode,subprocessid,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_detail_art WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_art, out this.artTb);

            string cmd_qty = "Select 0 as No,qty,'' as orderid,'' as cutref,'' as article, SizeCode, 0 as iden from Bundle_Detail_Qty WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_qty, out this.qtyTb);

            this.GridSetup();
        }

        /// <inheritdoc/>
        public void GridSetup()
        {
            #region 上方兩 grid 勾選連動事件
            this.gridCutRef.CellClick += (s, e) =>
            {
                if (e.RowIndex != -1)
                {
                    return; // 判斷是Header
                }

                if (e.ColumnIndex != 0)
                {
                    return; // 判斷是Sel 欄位
                }

                DataRow dr = this.gridCutRef.GetDataRow(0);
                int oldvalue = Convert.ToInt16(dr["sel"]);
                int newvalue = Convert.ToInt16(((DataTable)this.gridCutRef.DataSource).DefaultView.ToTable().Rows[0]["sel"]);
                foreach (DataRow row in this.ArticleSizeTb.Rows)
                {
                    row["Sel"] = newvalue;
                }

                dr["sel"] = newvalue;
                dr.EndEdit();
                this.gridArticleSize.Refresh();
            };
            this.gridArticleSize.CellClick += (s, e) =>
            {
                if (e.RowIndex != -1)
                {
                    return; // 判斷是Header
                }

                if (e.ColumnIndex != 0)
                {
                    return; // 判斷是Sel 欄位
                }

                string cutref = this.ArticleSizeTb.DefaultView.ToTable().Rows[0]["Cutref"].ToString();
                int sel = Convert.ToInt16(this.ArticleSizeTb.DefaultView.ToTable().Rows[0]["Sel"]);
                DataRow[] dr = this.CutRefTb.Select(string.Format("Cutref='{0}'", cutref));
                if (dr.Length > 0)
                {
                    dr[0]["Sel"] = sel;
                }

                this.gridCutRef.Refresh();
            };
            #endregion

            #region 左上 gridCutRef 事件
            DataGridViewGeneratorCheckBoxColumnSettings chcutref = new DataGridViewGeneratorCheckBoxColumnSettings();
            chcutref.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutRef.GetDataRow(e.RowIndex);
                if (this.ArticleSizeTb != null)
                {
                    if ((bool)e.FormattedValue == MyUtility.Convert.GetBool(dr["sel"]))
                    {
                        return;
                    }

                    int oldvalue = Convert.ToInt16(dr["sel"]);
                    int newvalue = Convert.ToInt16(e.FormattedValue);
                    DataRow[] articleAry = this.ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", dr["Ukey"], dr["Fabriccombo"]));

                    foreach (DataRow row in articleAry)
                    {
                        row["Sel"] = newvalue;
                    }

                    dr["sel"] = newvalue;
                    dr.EndEdit();
                    this.gridArticleSize.Refresh();
                }
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
            #region 左上一Grid
            this.gridCutRef.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.gridCutRef)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: chcutref)
                .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("POID", header: "POID", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Date("estCutdate", header: "Est.CutDate", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Fabriccombo", header: "Fabric" + Environment.NewLine + "Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("FabricPanelCode", header: "Pattern" + Environment.NewLine + "Panel", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Item", header: "Item", width: Widths.AnsiChars(20), iseditingreadonly: false, settings: itemsetting)
                    .Get(out Ict.Win.UI.DataGridViewTextBoxColumn item)
                .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("FabricKind", header: "Fabric Kind", width: Widths.AnsiChars(5), iseditingreadonly: true)
                ;

            this.gridCutRef.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutRef.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutRef.Columns["Item"].DefaultCellStyle.BackColor = Color.Pink;
            item.MaxLength = 20;
            #endregion

            #region 右上 gridArticleSize 事件
            DataGridViewGeneratorCheckBoxColumnSettings charticle = new DataGridViewGeneratorCheckBoxColumnSettings();
            charticle.CellValidating += (s, e) =>
            {
                if (this.ArticleSizeTb == null)
                {
                    return;
                }

                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                int newvalue = Convert.ToInt16(e.FormattedValue);
                dr["sel"] = newvalue;
                dr.EndEdit();

                DataRow selectDr = ((DataRowView)this.gridCutRef.GetSelecteds(SelectedSort.Index)[0]).Row;

                DataRow[] artAry = this.ArticleSizeTb.Select(string.Format("Sel=1 and Cutref='{0}'", dr["Cutref"]));
                if (artAry.Length == 0)
                {
                    selectDr["Sel"] = 0;
                }
                else
                {
                    selectDr["Sel"] = 1;
                }

                this.gridCutRef.Refresh();
            };

            DataGridViewGeneratorTextColumnSettings selectExcess = new DataGridViewGeneratorTextColumnSettings();
            selectExcess.EditingMouseDown += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return; // 判斷是Header
                }

                if (e.Button != MouseButtons.Right)
                {
                    return;
                }

                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                if (!MyUtility.Convert.GetString(dr["isExcess"]).EqualString("Y"))
                {
                    return;
                }

                string scalecmd = $@"
select wd.Orderid,Article,w.Colorid,SizeCode,o.SewLine,w.CutCellid
from workorder_Distribute wd WITH (NOLOCK) 
inner join WorkOrder w WITH (NOLOCK) on wd.WorkOrderUkey = w.Ukey
inner join orders o WITH (NOLOCK) on w.id = o.cuttingsp and wd.OrderID = o.id
where workorderukey = '{dr["Ukey"]}'and wd.orderid <>'EXCESS'
";
                SelectItem item1 = new SelectItem(scalecmd, string.Empty, dr["Orderid"].ToString());
                DialogResult result = item1.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                dr["Orderid"] = item1.GetSelectedString(); // 將選取selectitem value帶入GridView
                dr["Article"] = item1.GetSelecteds()[0]["Article"];
                dr["Colorid"] = item1.GetSelecteds()[0]["Colorid"];
                dr["SizeCode"] = item1.GetSelecteds()[0]["SizeCode"];
                dr["SewingLine"] = item1.GetSelecteds()[0]["SewLine"];
                dr["SewingCell"] = item1.GetSelecteds()[0]["CutCellid"];
            };

            DataGridViewGeneratorTextColumnSettings linecell = new DataGridViewGeneratorTextColumnSettings();
            linecell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    string sql = string.Format(
                        @"Select DISTINCT ID  From SewingLine WITH (NOLOCK) 
                        where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}')", Env.User.Keyword);
                    sele = new SelectItem(sql, "10", dr["SewingLine"].ToString())
                    {
                        Width = 300,
                    };
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            linecell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                string line = e.FormattedValue.ToString();
                string oldvalue = dr["Sewingline"].ToString();
                if (oldvalue == line)
                {
                    return;
                }

                if (!MyUtility.Check.Seek(line, "SewingLine", "ID"))
                {
                    dr["Sewingline"] = string.Empty;
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorTextColumnSettings cellcell = new DataGridViewGeneratorTextColumnSettings();
            cellcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    sele = new SelectItem("Select SewingCell from Sewingline WITH (NOLOCK) where SewingCell!='' group by SewingCell", "10", dr["SewingCell"].ToString())
                    {
                        Width = 300,
                    };
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            cellcell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                string cell = e.FormattedValue.ToString();
                string oldvalue = dr["SewingCell"].ToString();
                if (oldvalue == cell)
                {
                    return;
                }

                if (!MyUtility.Check.Seek(string.Format("Select * from SewingLine WITH (NOLOCK) where sewingCell='{0}'", cell)))
                {
                    dr["SewingCell"] = string.Empty;
                    dr.EndEdit();
                }
            };

            DataGridViewGeneratorNumericColumnSettings qtycell = new DataGridViewGeneratorNumericColumnSettings();
            qtycell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                int old = MyUtility.Convert.GetInt(dr["Qty"]);
                int newvalue = MyUtility.Convert.GetInt(e.FormattedValue);
                if (old == newvalue)
                {
                    return;
                }

                int rowcount = this.qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), string.Empty).Length;
                int newcount = Convert.ToInt16(e.FormattedValue);
                this.numNoOfBundle.Value = newcount;
                this.DistSizeQty(rowcount, newcount, dr);
            };

            DataGridViewGeneratorNumericColumnSettings cutOutputCell = new DataGridViewGeneratorNumericColumnSettings();
            cutOutputCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                int oldCutOutput = Convert.ToInt32(dr["cutOutput"]);
                int newCutOutput = Convert.ToInt32(e.FormattedValue);
                dr["cutOutput"] = newCutOutput;
                dr.EndEdit();

                if (oldCutOutput.EqualString(newCutOutput) == false)
                {
                    this.ChangeLabelBalanceValue();

                    int rowcount = this.qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), string.Empty).Length;
                    int newcount = MyUtility.Convert.GetInt(dr["Qty"]);
                    this.DistSizeQty(rowcount, newcount, dr);
                }
            };
            #endregion
            #region 右上一Grid
            this.gridArticleSize.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridArticleSize)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: charticle)
                .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: selectExcess)
                .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: selectExcess)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: selectExcess)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: selectExcess)
                .Text("SewingLine", header: "Line#", width: Widths.AnsiChars(2), settings: linecell)
                .Text("SewingCell", header: "Sew" + Environment.NewLine + "Cell", width: Widths.AnsiChars(2), settings: cellcell)
                .Numeric("Qty", header: "No of" + Environment.NewLine + "Bundle", width: Widths.AnsiChars(3), integer_places: 3, settings: qtycell)
                .Numeric("Cutoutput", header: "Cut" + Environment.NewLine + "OutPut", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: false, settings: cutOutputCell)
                .Numeric("TotalParts", header: "Total" + Environment.NewLine + "Parts", width: Widths.AnsiChars(4), integer_places: 3, iseditingreadonly: true)
                .Text("isEXCESS", header: "EXCESS", width: Widths.AnsiChars(2), iseditingreadonly: true)
                ;
            this.gridArticleSize.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridArticleSize.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridArticleSize.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridArticleSize.Columns["SewingLine"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridArticleSize.Columns["SewingCell"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridArticleSize.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridArticleSize.Columns["Cutoutput"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region  左下 gridQty 事件
            DataGridViewGeneratorNumericColumnSettings qtySizecell = new DataGridViewGeneratorNumericColumnSettings();
            qtySizecell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridQty.GetDataRow(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr.EndEdit();
                int qty = MyUtility.Convert.GetInt(this.qtyTb.Compute("Sum(Qty)", string.Format("iden ='{0}'", dr["iden"])));
                this.label_TotalQty.Text = qty.ToString();
            };
            #endregion
            #region 左下 Qty
            this.gridQty.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridQty)
                .Numeric("No", header: "No", width: Widths.AnsiChars(3), integer_places: 2, iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4), integer_places: 3, settings: qtySizecell)
                ;
            this.gridQty.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region 下中 gridPattern 事件
            DataGridViewGeneratorTextColumnSettings patterncell = new DataGridViewGeneratorTextColumnSettings();
            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;

                    sele = new SelectItem(this.GarmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}' and iden = '{dr["iden"]}'").Count() > 0)
                    {
                        dr["IsPair"] = this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}' and iden = '{dr["iden"]}'")[0]["IsPair"];
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
                    this.SynchronizeMain(0, "PatternCode");
                    this.CombineSubprocessIspair(MyUtility.Convert.GetLong(dr["iden"]));
                    this.Calpart();
                    Prgs.CheckNotMain(dr, this.GarmentTb);
                }
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridPattern.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                string oldvalue = dr["PatternCode"].ToString();
                if (oldvalue == patcode)
                {
                    return;
                }

                if (this.patternTb.Select($@"PatternCode = '{patcode}' and iden = '{dr["iden"]}'").Count() > 0)
                {
                    dr["IsPair"] = this.patternTb.Select($@"PatternCode = '{patcode}' and iden = '{dr["iden"]}'")[0]["IsPair"];
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
                this.SynchronizeMain(0, "PatternCode");
                this.CombineSubprocessIspair(MyUtility.Convert.GetLong(dr["iden"]));
                this.Calpart();
                Prgs.CheckNotMain(dr, this.GarmentTb);
            };

            DataGridViewGeneratorTextColumnSettings patternDesc = new DataGridViewGeneratorTextColumnSettings() { CharacterCasing = CharacterCasing.Normal };
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
                    Prgs.CheckNotMain(dr, this.GarmentTb);
                }
            };

            DataGridViewGeneratorNumericColumnSettings partQtyCell = new DataGridViewGeneratorNumericColumnSettings();
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
                if (MyUtility.Convert.GetString(dr["PatternCode"]).ToUpper() != "ALLPARTS")
                {
                    bool ispair = MyUtility.Convert.GetBool(e.FormattedValue);
                    dr["IsPair"] = ispair;
                    dr.EndEdit();
                    if (this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}' and iden = '{dr["iden"]}'").Count() > 0)
                    {
                        foreach (DataRow row in this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}'and iden = '{dr["iden"]}'"))
                        {
                            row["IsPair"] = ispair;
                        }
                    }
                }
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
            #region 下中 gridPattern
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
                    this.SynchronizeMain(1, "PatternCode");
                    this.Calpart();
                }
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
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.Calpart();
            };
            #endregion
            #region 右下一 gridAllPart
            this.gridAllPart.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridAllPart)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
                .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13), settings: patternDesc2)
                .Text("Location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(5))
                .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell2)
                .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);
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
            this.qtyTb.Clear();
            this.GarmentTb = null;
            this.CutRefTb = null;
            this.ArticleSizeTb = null;
            this.ArticleSizeTb_View = null;
            this.ExcessTb = null;
            this.SizeRatioTb = null;
            this.headertb = null;
            this.gridPattern.DataSource = null;
            this.gridAllPart.DataSource = null;
            this.gridQty.DataSource = null;
            this.gridArticleSize.DataSource = null;
            this.gridCutRef.DataSource = null;
        }

        private bool BeforeQuery()
        {
            if (MyUtility.Check.Empty(this.txtCutref.Text) && MyUtility.Check.Empty(this.dateEstCutDate.Value) && MyUtility.Check.Empty(this.txtPOID.Text))
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
                this.isCombineSubProcess = MyUtility.Convert.GetBool(sysDr["IsCombineSubProcess"]);
                this.isNoneShellNoCreateAllParts = MyUtility.Convert.GetBool(sysDr["IsNoneShellNoCreateAllParts"]);
            }

            DBProxy.Current.DefaultTimeout = 300;
            string cutref = this.txtCutref.Text;
            string cutdate = this.dateEstCutDate.Value == null ? string.Empty : this.dateEstCutDate.Value.Value.ToShortDateString();
            string factory = this.txtfactoryByM.Text;
            string poid = this.txtPOID.Text;
            string spreadingNoID = this.txtSpreadingNo1.Text;
            string where = MyUtility.Check.Empty(cutref) ? string.Empty : Environment.NewLine + $"and w.cutref='{cutref}'";
            where += MyUtility.Check.Empty(cutdate) ? string.Empty : Environment.NewLine + $"and w.estcutdate='{cutdate}'";
            where += MyUtility.Check.Empty(poid) ? string.Empty : Environment.NewLine + $"and o.poid='{poid}'";
            where += MyUtility.Check.Empty(factory) ? string.Empty : Environment.NewLine + $"and o.FtyGroup='{factory}'";
            where += MyUtility.Check.Empty(spreadingNoID) ? string.Empty : Environment.NewLine + $"and w.SpreadingNoID='{spreadingNoID}'";
            string distru_where = this.chkAEQ.Checked ? string.Empty : " and wd.orderid <>'EXCESS'";
            this.gridArticleSize.Columns["isEXCESS"].Visible = this.chkAEQ.Checked;

            // 左上
            string query_cmd = $@"
Select
	 sel = cast(0 as bit)
	, w.cutref
	, o.poid
	, w.estcutdate
	, w.Fabriccombo
	, w.FabricPanelCode
	, w.cutno
	, item.item
	, w.SpreadingNoID
	, w.colorid
	, w.Ukey
    , FabricKind = FabricKind.FabricKind
    , o.StyleUkey
	, w.MDivisionId
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

            string distru_cmd = $@"
Select
	 sel = cast(0 as bit)
	, w.Ukey
	, iden = 0
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
    , cutoutput = isnull(wd.Qty,0)
	, RealCutOutput = isnull(wd.Qty,0)
    , CreatedBundleQty = isnull((select SUM(bdq.qty) from Bundle b with(nolock) inner join bundle_detail_Qty bdq on bdq.id = b.id where b.cutref = w.cutref), 0)
	, TotalParts = 0
	, o.poid
	, startno = 0
	, o.StyleUkey
    , byTone = cast((select AutoGenerateByTone from system)as int)
    , IsCombineSubProcess = cast({(this.isCombineSubProcess ? "1" : "0")} as bit)
    , isNoneShellNoCreateAllParts = cast(iif(FabricKind.FabricKindID <> '1' and {(this.isNoneShellNoCreateAllParts ? "1" : "0")} = 1, 1, 0) as bit)
    , FabricKind.FabricKindID
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
outer apply (SELECT TOP 1 wp.patternpanel FROM workorder_PatternPanel wp WITH (NOLOCK) WHERE w.ukey = wp.workorderukey)wp
outer apply(
    SELECT TOP 1 FabricKind = DD.id + '-' + DD.NAME, FabricKindID = DD.id
    FROM order_colorcombo OCC WITH (NOLOCK)
	inner join order_bof OB WITH (NOLOCK) on OCC.id = OB.id AND OCC.fabriccode = OB.fabriccode
	inner join  dropdownlist DD WITH (NOLOCK) on  DD.id = OB.kind
    WHERE OCC.id = w.id
	and OCC.patternpanel = wp.patternpanel
	and DD.[type] = 'FabricKind'
)FabricKind
outer apply(select article = iif(wd.OrderID = 'EXCESS',isnull(l.article,l2.article),wd.article))article
outer apply(select sizecode = iif(wd.OrderID = 'EXCESS',isnull(l.sizecode,l2.sizecode),wd.sizecode))sizecode
Where isnull(w.CutRef,'') <> ''
and o.mDivisionid = '{this.keyWord}'
{where}
{distru_where}
order by article.article,wd.sizecode,wd.orderid,w.FabricPanelCode
";
            result = DBProxy.Current.Select(null, distru_cmd, out this.ArticleSizeTb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

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

            #region articleSizeTb 繞PO 找出QtyTb,PatternTb,AllPartTb
            int iden = 1;
            this.patternTbOri = this.patternTb.Clone();
            this.allpartTbOri = this.allpartTb.Clone();

            foreach (DataRow dr in this.ArticleSizeTb.Rows)
            {
                dr["iden"] = iden;
                #region Create Qtytb
                DataRow qty_newRow = this.qtyTb.NewRow();
                qty_newRow["No"] = 1;
                qty_newRow["Qty"] = dr["cutoutput"];
                qty_newRow["OrderID"] = dr["OrderID"];
                qty_newRow["Cutref"] = dr["Cutref"];
                qty_newRow["Article"] = dr["Article"];
                qty_newRow["SizeCode"] = dr["SizeCode"];
                qty_newRow["iden"] = iden;
                this.qtyTb.Rows.Add(qty_newRow);
                #endregion

                this.ArticleSizeTb_View = this.ArticleSizeTb.Select($"Ukey ='{dr["Ukey"]}' and Fabriccombo = '{dr["Fabriccombo"]}'").CopyToDataTable();
                this.CreatePattern(dr);
                int totalpart = MyUtility.Convert.GetInt(this.patternTb.Compute("sum(Parts)", $"iden ={iden}"));
                dr["TotalParts"] = totalpart;
                iden++;
            }

            this.ArticleSizeTb.AsEnumerable().ToList().ForEach(r => this.AddPatternAllpart(r));
            #endregion

            this.gridCutRef.DataSource = this.CutRefTb; // 左上
            this.gridArticleSize.DataSource = this.ArticleSizeTb; // 右上
            this.gridQty.DataSource = this.qtyTb;
            this.gridPattern.DataSource = this.patternTb; // 左下
            this.gridAllPart.DataSource = this.allpartTb; // 右下

            this.GridAutoResizeColumns();
            this.ShowExcessDatas(where);
        }

        private void AddPatternAllpart(DataRow r)
        {
            long iden = MyUtility.Convert.GetLong(r["iden"]);
            bool isCombineSubProcess = MyUtility.Convert.GetBool(r["isCombineSubProcess"]);
            bool isNoneShellNoCreateAllParts = MyUtility.Convert.GetBool(r["isNoneShellNoCreateAllParts"]);
            DataTable dtp = this.patternTbOri.Clone();
            DataTable dta = this.allpartTbOri.Clone();
            DataTable oriP = this.patternTbOri.Select($"iden = {iden}").TryCopyToDataTable(this.patternTbOri);
            DataTable oriA = this.allpartTbOri.Select($"iden = {iden}").TryCopyToDataTable(this.allpartTbOri);

            if (!isCombineSubProcess)
            {
                dtp = oriP;
                dta = oriA;
            }
            else
            {
                dtp = oriP.Select($"isMain = 1").TryCopyToDataTable(this.patternTb);
                dtp.AsEnumerable().ToList().ForEach(f => f["isPair"] = false);
                dtp.ImportRow(oriP.Select($"PatternCode = 'ALLPARTS'").FirstOrDefault());

                // 右邊除了 ALLPart 資訊，  資訊全部都要，不論 IsMain
                DataTable psdt = oriP.Select($"PatternCode <> 'ALLPARTS'").TryCopyToDataTable(this.patternTb);
                psdt.AsEnumerable().ToList().ForEach(f => dta.ImportRow(f));
                dta.Merge(oriA);
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
            if (isCombineSubProcess)
            {
                this.patternTb.AsEnumerable().ToList().ForEach(f => f["Parts"] = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", $"iden = {f["iden"]} and CombineSubProcessGroup = {f["CombineSubProcessGroup"]}")));
            }

            r["TotalParts"] = MyUtility.Convert.GetInt(this.patternTb.Compute("Sum(Parts)", $"iden = {r["iden"]}"));
        }

        private void CreatePattern(DataRow row)
        {
            string poid = row["POID"].ToString();
            string patternpanel = row["FabricPanelCode"].ToString();
            string cutref = row["Cutref"].ToString();
            int iden = MyUtility.Convert.GetInt(row["iden"]);

            // 找出相同PatternPanel 的subprocessid
            int npart = 0; // allpart 數量
            string patidsql;
            #region 輸出GarmentTb
            string styleyukey = MyUtility.GetValue.Lookup("Styleukey", poid, "Orders", "ID");

            var sizelist = this.ArticleSizeTb_View.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();
            string sizes = "'" + string.Join("','", sizelist) + "'";
            string sqlSizeGroup = $@"SELECT TOP 1 IIF(ISNULL(SizeGroup,'')='','N',SizeGroup) FROM Order_SizeCode WHERE ID ='{poid}' and SizeCode IN ({sizes})";
            string sizeGroup = MyUtility.GetValue.Lookup(sqlSizeGroup);
            patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{poid}','{cutref}','',{styleyukey},'{sizeGroup}')s";

            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            string headercodesql = $@"
Select distinct ArticleGroup 
from Pattern_GL_LectraCode WITH (NOLOCK) 
where PatternUkey = '{patternukey}' 
order by ArticleGroup";

            DualResult headerResult = DBProxy.Current.Select(null, headercodesql, out this.headertb);
            if (!headerResult)
            {
                return;
            }
            #region 建立Table
            string tablecreatesql = string.Format(@"Select '{0}' as orderid,a.*,'' as F_CODE", poid);
            foreach (DataRow dr in this.headertb.Rows)
            {
                tablecreatesql += string.Format(" ,'' as {0}", dr["ArticleGroup"]);
            }

            tablecreatesql += string.Format(" from Pattern_GL a WITH (NOLOCK) Where PatternUkey = '{0}'", patternukey);
            DualResult tablecreateResult = DBProxy.Current.Select(null, tablecreatesql, out DataTable garmentListTb);
            if (!tablecreateResult)
            {
                return;
            }
            #endregion

            #region 寫入FCode~CodeA~CodeZ
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

                    // dr[artgroup] = lecdr["PatternPanel"].ToString().Trim();
                    // Mantis_7045 比照舊系統對應FabricPanelCode
                    dr[artgroup] = lecdr["FabricPanelCode"].ToString().Trim();
                }

                if (dr["SEQ"].ToString() == "0001")
                {
                    dr["PatternCode"] = dr["PatternCode"].ToString().Substring(10);
                }
            }
            #endregion
            this.GarmentTb = garmentListTb;
            #endregion

            StringBuilder w = new StringBuilder();
            w.Append(string.Format("orderid = '{0}' and (1=0", poid));
            foreach (DataRow dr in this.headertb.Rows)
            {
                w.Append(string.Format(" or {0} = '{1}' ", dr[0], patternpanel));
            }

            w.Append(")");

            this.GarmentTb.Columns.Add("CombineSubprocessGroup", typeof(int));
            this.GarmentTb.Columns.Add("IsMain", typeof(bool));
            DataRow[] garmentar = this.GarmentTb.Select(w.ToString());
            Prgs.SetCombineSubprocessGroup_IsMain(garmentar);

            foreach (DataRow dr in garmentar)
            {
                // 若無ANNOTATion直接寫入All Parts
                if (MyUtility.Check.Empty(dr["annotation"]))
                {
                    DataRow ndr = this.allpartTbOri.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["Location"] = dr["Location"];
                    ndr["parts"] = MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                    ndr["Cutref"] = cutref;
                    ndr["POID"] = poid;
                    ndr["iden"] = iden;
                    ndr["IsPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
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
                                    ndr2["iden"] = iden;
                                    ndr2["IsPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
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
                                ndr2["iden"] = iden;
                                ndr2["IsPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
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
                            ndr["iden"] = iden;
                            ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            ndr["IsPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
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
                        ndr["iden"] = iden;
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        ndr["IsPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                        ndr["CombineSubprocessGroup"] = 0;
                        this.allpartTbOri.Rows.Add(ndr);
                    }
                    #endregion
                }
            }

            DataRow pdr = this.patternTbOri.NewRow(); // 預設要有ALLPARTS
            pdr["PatternCode"] = "ALLPARTS";
            pdr["PatternDesc"] = "All Parts";
            pdr["parts"] = npart;
            pdr["Cutref"] = cutref;
            pdr["POID"] = poid;
            pdr["iden"] = iden;
            pdr["CombineSubprocessGroup"] = 0;
            this.patternTbOri.Rows.Add(pdr);

            DBProxy.Current.DefaultTimeout = 0;
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

        private void GridAutoResizeColumns()
        {
            this.gridCutRef.AutoResizeColumns();
            this.gridQty.AutoResizeColumns();
            this.gridArticleSize.AutoResizeColumns();
            this.gridPattern.AutoResizeColumns();
            this.gridAllPart.AutoResizeColumns();
        }

        #region Grid change
        private void GridCutRef_SelectionChanged(object sender, EventArgs e)
        {
            if (this.ArticleSizeTb == null || this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            this.ArticleSizeTb.DefaultView.RowFilter = $"Ukey ='{this.gridCutRef.CurrentDataRow["Ukey"]}' and Fabriccombo = '{this.gridCutRef.CurrentDataRow["Fabriccombo"]}'";
            this.GridArticleSize_SelectionChanged(null, null);
        }

        private void GridArticleSize_SelectionChanged(object sender, EventArgs e)
        {
            if (this.ArticleSizeTb == null || this.gridArticleSize.CurrentDataRow == null)
            {
                return;
            }

            var articleSizeTb_tmp = this.ArticleSizeTb.Select($"Ukey ='{this.gridCutRef.CurrentDataRow["Ukey"]}' and Fabriccombo = '{this.gridCutRef.CurrentDataRow["Fabriccombo"]}'");
            if (articleSizeTb_tmp.Any())
            {
                this.ArticleSizeTb_View = articleSizeTb_tmp.CopyToDataTable();
            }
            else
            {
                this.ArticleSizeTb_View = null;
            }

            string filter = $"iden ='{this.gridArticleSize.CurrentDataRow["iden"]}'";
            this.qtyTb.DefaultView.RowFilter = filter;
            this.allpartTb.DefaultView.RowFilter = filter;
            this.patternTb.DefaultView.RowFilter = filter;
            this.label_TotalCutOutput.Text = this.gridArticleSize.CurrentDataRow["Cutoutput"].ToString();
            this.numNoOfBundle.Value = Convert.ToInt16(this.gridArticleSize.CurrentDataRow["Qty"]);
            this.numTotalPart.Value = Convert.ToInt16(this.gridArticleSize.CurrentDataRow["TotalParts"]);
            this.numTone.Value = Convert.ToInt16(this.gridArticleSize.CurrentDataRow["byTone"]);
            this.chkTone.Checked = this.numTone.Value > 0;
            this.label_TotalQty.Text = MyUtility.Convert.GetString(this.qtyTb.Compute("Sum(Qty)", filter));

            this.chkCombineSubprocess.Checked = MyUtility.Convert.GetBool(this.gridArticleSize.CurrentDataRow["IsCombineSubProcess"]);
            this.chkNoneShellNoCreateAllParts.Checked = MyUtility.Convert.GetBool(this.gridArticleSize.CurrentDataRow["IsNoneShellNoCreateAllParts"]);
            this.chkNoneShellNoCreateAllParts.ReadOnly = MyUtility.Convert.GetString(this.gridArticleSize.CurrentDataRow["FabricKindID"]) == "1";

            this.ChangeLabelTotalCutOutputValue();
            this.ChangeLabelBalanceValue();
            this.GridPattern_SelectionChanged(null, null);
        }

        private void GridPattern_SelectionChanged(object sender, EventArgs e)
        {
            this.ChangeRightLabel();
            if (this.gridPattern.CurrentDataRow == null)
            {
                return;
            }

            string filter = $"iden = {this.gridArticleSize.CurrentDataRow["iden"]}";
            if (this.chkCombineSubprocess.Checked)
            {
                filter += $" and CombineSubprocessGroup = {this.gridPattern.CurrentDataRow["CombineSubprocessGroup"]}";
            }

            this.allpartTb.DefaultView.RowFilter = filter;
        }
        #endregion

        private void DistSizeQty(int rowcount, int newcount, DataRow dr) // 計算Size Qty
        {
            // 缺少需先新增
            if (rowcount <= newcount)
            {
                for (int j = 0; j < newcount - rowcount; j++)
                {
                    DataRow ndr = this.qtyTb.NewRow();
                    ndr["SizeCode"] = dr["SizeCode"];
                    ndr["iden"] = dr["iden"];
                    this.qtyTb.Rows.Add(ndr);
                }
            }

            int i = 1;
            DataRow[] qtyArr = this.qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), string.Empty); // 重新撈取
            foreach (DataRow dr2 in qtyArr)
            {
                if (dr2.RowState != DataRowState.Deleted)
                {
                    dr2["No"] = i;
                }

                if (i > newcount)
                {
                    dr2.Delete(); // 多餘的筆數要刪掉
                }

                i++;
            }

            // Double TotalCutQty = Convert.ToDouble(dr["Cutoutput"]);
            double totalCutQty = MyUtility.Convert.GetDouble(this.label_TotalQty.Text);
            DataRow[] qtyarry = this.qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), string.Empty);
            if (totalCutQty % newcount == 0)
            {
                int qty = (int)(totalCutQty / newcount); // 每一個數量是多少
                foreach (DataRow dr2 in qtyarry)
                {
                    dr2["Qty"] = qty;
                }
            }
            else
            {
                int eachqty = (int)Math.Floor(totalCutQty / newcount);
                int modqty = (int)(totalCutQty % newcount); // 剩餘數

                foreach (DataRow dr2 in qtyarry)
                {
                    if (eachqty != 0)
                    {
                        if (modqty > 0)
                        {
                            dr2["Qty"] = eachqty + 1; // 每組分配一個Qty 當分配完表示沒了
                        }
                        else
                        {
                            dr2["Qty"] = eachqty;
                        }

                        modqty--; // 剩餘數一定小於rowcount所以會有筆數沒有拿到
                    }
                    else
                    {
                        if (modqty > 0)
                        {
                            dr2["Qty"] = 1;
                            modqty--;
                        }
                        else
                        {
                            dr2.Delete();
                        }
                    }
                }
            }

            this.qtyTb.AcceptChanges();
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
            ndr["iden"] = selectartDr["iden"];
            ndr["poid"] = selectartDr["poid"];
            ndr["Cutref"] = selectartDr["cutref"];
            ndr["Parts"] = selectartDr["Parts"];
            ndr["IsPair"] = selectartDr["IsPair"];

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

            this.patternTb.AcceptChanges();
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
                foreach (DataRow chdr in checkdr)
                {
                    bool isPair = MyUtility.Convert.GetBool(chdr["IsPair"]);
                    if (this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}' and iden = '{chdr["iden"]}'").Count() > 0)
                    {
                        isPair = MyUtility.Convert.GetBool(this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}' and iden = '{chdr["iden"]}'")[0]["IsPair"]);
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
                    ndr2["IsPair"] = isPair;
                    ndr2["isMain"] = true;
                    int max = this.patternTb.Select($"iden = {chdr["iden"]}").Max(m => MyUtility.Convert.GetInt(m["CombineSubprocessGroup"]));
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

                this.CombineSubprocessIspair(MyUtility.Convert.GetInt(this.gridArticleSize.CurrentDataRow["iden"]));
            }

            this.allpartTb.AcceptChanges();
            this.Calpart();
            #endregion
        }

        private void Gridvalid()
        {
            this.gridArticleSize.ValidateControl();
            this.gridQty.ValidateControl();
            this.gridPattern.ValidateControl();
            this.gridAllPart.ValidateControl();
        }

        private void Calpart() // 計算Parts,TotalParts
        {
            if (this.gridArticleSize.GetSelecteds(SelectedSort.Index).Count == 0)
            {
                string cutref = MyUtility.Convert.GetString(this.gridCutRef.CurrentDataRow["cutref"]);
                MyUtility.Msg.WarningBox($@"Distribution no data!!
Please check the cut refno#：{cutref} distribution data in workOrder(Cutting P02)");
                return;
            }

            string filteriden = $"iden = {this.gridArticleSize.CurrentDataRow["iden"]}";
            string filter_ALLPARTS = filteriden + $" and CombineSubprocessGroup = 0";
            DataRow[] allpartdr = this.patternTb.Select($"PatternCode='ALLPARTS' and {filter_ALLPARTS}");
            if (allpartdr.Length > 0)
            {
                allpartdr[0]["Parts"] = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", filter_ALLPARTS));
            }

            DataRow[] patternDrs = this.patternTb.Select(filteriden);
            if (this.chkCombineSubprocess.Checked)
            {
                foreach (DataRow dr in patternDrs)
                {
                    string fg = filteriden + $" and CombineSubprocessGroup = {dr["CombineSubprocessGroup"]}";
                    dr["Parts"] = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", fg));
                }
            }

            int totalpart = MyUtility.Convert.GetInt(this.patternTb.Compute("Sum(Parts)", filteriden));
            this.numTotalPart.Value = totalpart;
            this.gridArticleSize.CurrentDataRow["TotalParts"] = totalpart;
        }

        #region 右鍵 Menu 新增/刪除
        private void InsertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            DataRow ndr = this.patternTb.NewRow();
            ndr["iden"] = this.gridArticleSize.CurrentDataRow["iden"];
            ndr["cutref"] = this.gridArticleSize.CurrentDataRow["cutref"];
            int max = this.patternTb.Select($"iden = {ndr["iden"]}").Max(m => MyUtility.Convert.GetInt(m["CombineSubprocessGroup"]));
            ndr["CombineSubprocessGroup"] = max + 1;
            ndr["isMain"] = true;
            this.patternTb.Rows.Add(ndr);

            if (this.chkCombineSubprocess.Checked)
            {
                DataRow adr = this.allpartTb.NewRow();
                adr["cutref"] = this.gridArticleSize.CurrentDataRow["cutref"];
                adr["iden"] = this.gridArticleSize.CurrentDataRow["iden"];
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
                this.allpartTb.Select($"iden = {this.gridPattern.CurrentDataRow["iden"]} and CombineSubprocessGroup = {this.gridPattern.CurrentDataRow["CombineSubprocessGroup"]}").Delete();
            }

            this.gridPattern.CurrentDataRow.Delete();
            this.allpartTb.AcceptChanges();
            this.patternTb.AcceptChanges();
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
            ndr["iden"] = this.gridArticleSize.CurrentDataRow["iden"];
            ndr["cutref"] = this.gridArticleSize.CurrentDataRow["cutref"];
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
                this.allpartTb.Select($"iden = {this.gridPattern.CurrentDataRow["iden"]} and CombineSubprocessGroup = {this.gridPattern.CurrentDataRow["CombineSubprocessGroup"]}").Delete();
                this.gridPattern.CurrentDataRow.Delete();
            }
            else
            {
                this.gridAllPart.CurrentDataRow.Delete();
            }

            this.allpartTb.AcceptChanges();
            this.patternTb.AcceptChanges();
            this.Calpart();
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

        private void BtnGarmentList_Click(object sender, EventArgs e)
        {
            if (this.CutRefTb == null)
            {
                return;
            }

            if (this.CutRefTb.Rows.Count == 0)
            {
                return;
            }

            if (this.ArticleSizeTb_View == null)
            {
                return;
            }

            // if (ArticleSizeTb_View.Rows.Count == 0) return;
            if (this.gridArticleSize.GetSelecteds().Count == 0)
            {
                MyUtility.Msg.InfoBox("No distrubed data to create CutPart data");
                return;
            }

            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", selectDr["poid"].ToString(), "Orders", "ID");
            var sizelist = this.ArticleSizeTb_View.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            PublicForm.GarmentList callNextForm = new PublicForm.GarmentList(ukey, selectDr["poid"].ToString(), selectDr["Cutref"].ToString(), sizelist);
            callNextForm.ShowDialog(this);
        }

        private void BtnColorComb_Click(object sender, EventArgs e)
        {
            if (this.CutRefTb == null)
            {
                return;
            }

            if (this.CutRefTb.Rows.Count == 0)
            {
                return;
            }

            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", selectDr["poid"].ToString(), "Orders", "ID");
            PublicForm.ColorCombination callNextForm =
            new PublicForm.ColorCombination(selectDr["poid"].ToString(), ukey);
            callNextForm.ShowDialog(this);
        }

        private void BtnCopy_to_same_Cutref_Click(object sender, EventArgs e)
        {
            if (this.CutRefTb == null)
            {
                return;
            }

            if (this.CutRefTb.Rows.Count == 0)
            {
                return;
            }

            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            string cutref = selectDr["Cutref"].ToString();
            int iden = Convert.ToInt16(selectDr["iden"]);
            DataRow[] artDrAy = this.ArticleSizeTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));
            DataRow[] oldPatternDr = this.patternTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));
            DataRow[] oldAllPartDr = this.allpartTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));

            DataRow[] oldPatternDr_selected = this.patternTb.Select(string.Format("Cutref='{0}' and iden={1}", cutref, iden));
            DataRow[] oldAllPartDr_selected = this.allpartTb.Select(string.Format("Cutref='{0}' and iden={1}", cutref, iden));
            foreach (DataRow dr in oldPatternDr)
            {
                dr.Delete();
            }

            foreach (DataRow dr in oldAllPartDr)
            {
                dr.Delete();
            }

            this.allpartTb.AcceptChanges();
            this.patternTb.AcceptChanges();

            // 抓出iden
            foreach (DataRow dr in artDrAy)
            {
                // 新增Pattern
                foreach (DataRow dr2 in oldPatternDr_selected)
                {
                    DataRow ndr = this.patternTb.NewRow();
                    ndr["iden"] = dr["iden"];
                    ndr["cutref"] = dr["cutref"];
                    ndr["poid"] = dr["poid"];
                    ndr["PatternCode"] = dr2["PatternCode"];
                    ndr["PatternDesc"] = dr2["PatternDesc"];
                    ndr["Location"] = dr2["Location"];
                    ndr["art"] = dr2["art"];
                    ndr["Parts"] = dr2["Parts"];
                    ndr["IsPair"] = dr2["IsPair"];
                    this.patternTb.Rows.Add(ndr);
                }

                foreach (DataRow dr2 in oldAllPartDr_selected)
                {
                    DataRow ndr = this.allpartTb.NewRow();
                    ndr["iden"] = dr["iden"];
                    ndr["cutref"] = dr["cutref"];
                    ndr["poid"] = dr["poid"];
                    ndr["PatternCode"] = dr2["PatternCode"];
                    ndr["PatternDesc"] = dr2["PatternDesc"];
                    ndr["Location"] = dr2["Location"];
                    ndr["annotation"] = dr2["annotation"];
                    ndr["Parts"] = dr2["Parts"];
                    ndr["IsPair"] = dr2["IsPair"];

                    this.allpartTb.Rows.Add(ndr);
                }
            }

            this.CopyGridCutRef(true, string.Empty);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NumNoOfBundle_Validating(object sender, CancelEventArgs e)
        {
            int oldcount = Convert.ToInt16(this.numNoOfBundle.OldValue);
            int newcount = Convert.ToInt16(this.numNoOfBundle.Value);
            if (this.ArticleSizeTb == null)
            {
                return;
            }

            if (this.ArticleSizeTb.Rows.Count == 0)
            {
                return;
            }

            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr["Qty"] = newcount;
            this.DistSizeQty(oldcount, newcount, selectDr);
        }

        private void NumTone_Validating(object sender, CancelEventArgs e)
        {
            this.RecordTone();
        }

        private void ChkTone_Validating(object sender, CancelEventArgs e)
        {
            this.RecordTone();
        }

        private void RecordTone()
        {
            int newcount = Convert.ToInt16(this.numTone.Value);
            if (this.ArticleSizeTb == null)
            {
                return;
            }

            if (this.ArticleSizeTb.Rows.Count == 0)
            {
                return;
            }

            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            if (this.chkTone.Checked && this.numTone.Value > 0)
            {
                selectDr["byTone"] = newcount;
            }
            else
            {
                selectDr["byTone"] = 0;
            }
        }

        private void BtnCopy_to_other_Cutref_Click(object sender, EventArgs e)
        {
            var frm = new P11_copytocutref();
            frm.ShowDialog(this);
            if (!MyUtility.Check.Empty(frm.Copycutref))
            {
                string copycutref = frm.Copycutref;
                DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
                string cutref = selectDr["Cutref"].ToString();

                if (cutref.Equals(frm.Copycutref))
                {
                    MyUtility.Msg.WarningBox("<CutRef> can not input selected CutRef itself");
                    return;
                }

                DataRow[] artDrAy = this.ArticleSizeTb.Select(string.Format("Cutref='{0}'", copycutref));
                DataRow[] oldPatternDr = this.patternTb.Select(string.Format("Cutref='{0}'", copycutref));
                DataRow[] oldAllPartDr = this.allpartTb.Select(string.Format("Cutref='{0}' ", copycutref));
                DataTable patternDv = this.patternTb.DefaultView.ToTable();
                DataTable allpartDv = this.allpartTb.DefaultView.ToTable();
                foreach (DataRow dr in oldPatternDr)
                {
                    dr.Delete();
                }

                foreach (DataRow dr in oldAllPartDr)
                {
                    dr.Delete();
                }

                this.allpartTb.AcceptChanges();
                this.patternTb.AcceptChanges();

                // 抓出iden
                foreach (DataRow dr in artDrAy)
                {
                    // 新增Pattern
                    int npart = 0;
                    foreach (DataRow dr2 in patternDv.Rows)
                    {
                        DataRow ndr = this.patternTb.NewRow();
                        ndr["iden"] = dr["iden"];
                        ndr["cutref"] = dr["cutref"];
                        ndr["poid"] = dr["poid"];
                        ndr["PatternCode"] = dr2["PatternCode"];
                        ndr["PatternDesc"] = dr2["PatternDesc"];
                        ndr["Location"] = dr2["Location"];
                        ndr["art"] = dr2["art"];
                        ndr["Parts"] = dr2["Parts"];
                        ndr["IsPair"] = dr2["IsPair"];
                        this.patternTb.Rows.Add(ndr);
                        if (!MyUtility.Check.Empty(dr2["Parts"]))
                        {
                            npart += Convert.ToInt16(dr2["Parts"]);
                        }
                    }

                    foreach (DataRow dr2 in allpartDv.Rows)
                    {
                        DataRow ndr = this.allpartTb.NewRow();
                        ndr["iden"] = dr["iden"];
                        ndr["cutref"] = dr["cutref"];
                        ndr["poid"] = dr["poid"];
                        ndr["PatternCode"] = dr2["PatternCode"];
                        ndr["PatternDesc"] = dr2["PatternDesc"];
                        ndr["Location"] = dr2["Location"];
                        ndr["annotation"] = dr2["annotation"];
                        ndr["Parts"] = dr2["Parts"];
                        ndr["IsPair"] = dr2["IsPair"];

                        this.allpartTb.Rows.Add(ndr);
                    }

                    dr["TotalParts"] = npart;
                }

                this.CopyGridCutRef(false, copycutref);
            }
        }

        private void BtnBatchCreate_Click(object sender, EventArgs e)
        {
            this.Gridvalid();
            if (this.CutRefTb == null || this.CutRefTb.Rows.Count == 0)
            {
                return;
            }

            DataRow[] cutrefAy = this.CutRefTb.Select("Sel=1");
            if (cutrefAy.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first !!");
                return;
            }

            if (cutrefAy.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["item"]).Length > 20).Any())
            {
                DataTable wdt = this.CutRefTb.Select("Sel=1 and len(item) > 20").CopyToDataTable();
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

                return;
            }

            #region 判斷 Pattern 非 Allparts 為0筆, Allparts 項次的 Parts 不可0
            foreach (int iden in this.ArticleSizeTb.Select($"Sel = 1").AsEnumerable().Select(s => (int)s["iden"]))
            {
                if (!this.patternTb.Select($"iden = {iden} and PatternCode<>'ALLPARTS'").Any() &&
                    this.patternTb.Select($"iden = {iden} and PatternCode='ALLPARTS' and Parts = 0").Any())
                {
                    MyUtility.Msg.WarningBox("All Part Detail can't empty, Please check with Pattern Team.");
                    return;
                }
            }
            #endregion

            #region 判斷Pattern(Cutpart_grid)的Artwork  不可為空
            DataRow[] findr = this.patternTb.Select("PatternCode<>'ALLPARTS' and (art='' or art is null)", string.Empty);
            var tmpArticleSizeTb = this.ArticleSizeTb.AsEnumerable();

            foreach (DataRow dr in findr)
            {
                bool isArticleSizeSelected = tmpArticleSizeTb.Where(s => (int)s["iden"] == (int)dr["iden"] && (int)s["Sel"] == 1).Any();
                if (isArticleSizeSelected)
                {
                    MyUtility.Msg.WarningBox("<Art> can not be empty!");
                    return;
                }
            }

            #endregion
            #region 檢查 如果IsPair =✔, 加總相同的Cut Part的Parts, 必需>0且可以被2整除
            var idenlist = this.ArticleSizeTb.Select("Sel=1").AsEnumerable().Select(s => MyUtility.Convert.GetString(s["iden"])).ToList();
            var patternSaveList = this.patternTb.AsEnumerable().Where(w => idenlist.Contains(MyUtility.Convert.GetString(w["iden"]))).ToList();

            var samePairCt = patternSaveList.Where(w => MyUtility.Convert.GetBool(w["IsPair"])).GroupBy(g => new { CutPart = g["PatternCode"], iden = g["iden"] })
                .Select(s => new { s.Key.CutPart, s.Key.iden, Parts = s.Sum(i => MyUtility.Convert.GetDecimal(i["Parts"])) }).ToList();
            if (samePairCt.Where(w => w.Parts % 2 != 0).Any())
            {
                var mp = samePairCt.Where(w => w.Parts % 2 != 0).ToList();
                string msg = @"The following bundle is pair, but parts is not pair, please check Cut Part parts";
                DataTable dt = ListToDataTable.ToDataTable(mp);
                dt.Columns.Remove("iden");
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }
            #endregion

            #region Insert Table
            DataTable insert_Bundle = new DataTable();
            insert_Bundle.Columns.Add("Insert", typeof(string));
            DataTable insert_Bundle_Detail = new DataTable();
            insert_Bundle_Detail.Columns.Add("Insert", typeof(string));
            DataTable insert_Bundle_Detail_Order = new DataTable();
            insert_Bundle_Detail_Order.Columns.Add("Insert", typeof(string));
            DataTable insert_Bundle_Detail_Art = new DataTable();
            insert_Bundle_Detail_Art.Columns.Add("Insert", typeof(string));
            DataTable insert_Bundle_Detail_Combinesubprocess = new DataTable();
            insert_Bundle_Detail_Combinesubprocess.Columns.Add("Insert", typeof(string));
            DataTable insert_Bundle_Detail_AllPart = new DataTable();
            insert_Bundle_Detail_AllPart.Columns.Add("Insert", typeof(string));
            DataTable insert_Bundle_Detail_Qty = new DataTable();
            insert_Bundle_Detail_Qty.Columns.Add("Insert", typeof(string));
            DataTable insert_BundleNo = new DataTable();
            insert_BundleNo.Columns.Add("BundleNo", typeof(string));
            #endregion

            #region
            string sqlcmdclone = $@"select top 0 * from Bundle_Detail";
            DualResult result = DBProxy.Current.Select(null, sqlcmdclone, out DataTable bundle_Detail);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlcmdclone = $@"select top 0 * from Bundle_Detail_Art";
            result = DBProxy.Current.Select(null, sqlcmdclone, out DataTable bundle_Detail_Art);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlcmdclone = $@"select top 0 * from Bundle_Detail_CombineSubprocess";
            result = DBProxy.Current.Select(null, sqlcmdclone, out DataTable bundle_Detail_CombineSubprocess);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            #endregion

            // 一共產生幾張單
            int idofnum = this.ArticleSizeTb.Select("Sel=1").Length; // 會產生表頭數
            int bundleofnum = 0;
            int autono = 0, qtycount = 0, patterncount = 0;

            #region 計算Bundle數 並填入Ratio,Startno
            DataTable spStartnoTb = new DataTable(); // for Startno,分SP給
            spStartnoTb.Columns.Add("POID", typeof(string));
            spStartnoTb.Columns.Add("startno", typeof(string));

            var artAy = this.ArticleSizeTb.Select("Sel=1", "Orderid").OrderBy(row => row["Fabriccombo"]).ThenBy(row => row["Cutno"]);
            if (artAy.Any(a => MyUtility.Convert.GetInt(a["bytone"]) > MyUtility.Convert.GetInt(a["Qty"])))
            {
                MyUtility.Msg.WarningBox("Generate by Tone can not greater than No of Bunde");
                return;
            }

            foreach (DataRow artar in artAy)
            {
                int tone = MyUtility.Convert.GetInt(artar["byTone"]);
                #region 填入SizeRatio
                DataRow[] drRatio = this.SizeRatioTb.Select(string.Format("ukey = '{0}' and SizeCode ='{1}'", artar["ukey"], artar["SizeCode"]));
                if (drRatio.Length > 0)
                {
                    artar["Ratio"] = drRatio[0]["Qty"];
                }
                #endregion
                qtycount = 0;
                patterncount = 0;
                DataRow[] qtyAry = this.qtyTb.Select(string.Format("iden={0}", artar["iden"]));
                DataRow[] patternAry = this.patternTb.Select(string.Format("iden={0} and parts<>0", artar["iden"]));
                #region Bundle 數
                if (qtyAry.Length > 0)
                {
                    qtycount = qtyAry.Length;
                }
                else
                {
                    qtycount = 0;
                }

                if (patternAry.Length > 0)
                {
                    patterncount = patternAry.Length;
                }
                else
                {
                    patterncount = 0;
                }

                if (tone == 0)
                {
                    bundleofnum += qtycount * patterncount; // bundle 數
                }
                else
                {
                    int na = this.patternTb.Select($"iden={artar["iden"]} and parts<>0 and PatternCode <> 'AllParts'").Length;
                    int a = this.patternTb.Select($"iden={artar["iden"]} and parts<>0 and PatternCode = 'AllParts'").Length;

                    bundleofnum = bundleofnum + (a * tone) + (qtycount * na * tone);
                }
                #endregion

                #region Start no
                DataRow[] spdr = spStartnoTb.Select(string.Format("POID='{0}'", artar["POID"]));
                if (spdr.Length == 0)
                {
                    DataRow new_spdr = spStartnoTb.NewRow();
                    new_spdr["POID"] = artar["POID"];

                    // auto
                    if (autono == 0)
                    {
                        #region startno
                        string max_cmd = string.Format("Select isnull(Max(startno+Qty),1) as Start from Bundle WITH (NOLOCK) Where POID = '{0}'", artar["POID"]);
                        if (DBProxy.Current.Select(null, max_cmd, out DataTable max_st))
                        {
                            if (max_st.Rows.Count != 0)
                            {
                                new_spdr["startno"] = Convert.ToInt16(max_st.Rows[0]["Start"]);
                            }
                            else
                            {
                                new_spdr["startno"] = 1;
                            }
                        }
                        else
                        {
                            new_spdr["startno"] = 1;
                        }
                        #endregion
                    }
                    else
                    {
                        new_spdr["startno"] = 1;
                    }

                    spStartnoTb.Rows.Add(new_spdr);
                }
                #endregion
            }

            #endregion
            string iDKeyword = this.keyWord + "BC";
            List<string> id_list = MyUtility.GetValue.GetBatchID(iDKeyword, "Bundle", batchNumber: idofnum, sequenceMode: 2);
            List<string> bundleno_list = MyUtility.GetValue.GetBatchID(string.Empty, "Bundle_Detail", format: 3, checkColumn: "Bundleno", batchNumber: bundleofnum, sequenceMode: 2);

            #region Insert Table
            int idcount = 0;
            int bundlenocount = 0;
            int bundlenoCount_Record = 0;
            foreach (DataRow artar in artAy)
            {
                int printGroup = 1;
                int startno = 1;
                int startno_bytone = 1;
                DataRow[] startnoAry = spStartnoTb.Select(string.Format("POID='{0}'", artar["POID"]));
                if (autono == 0)
                {
                    startno = Convert.ToInt16(startnoAry[0]["Startno"]);
                    startno_bytone = startno;
                }

                string subCut = Prgs.GetSubCutNo(artar["CutRef"].ToString(), artar["Fabriccombo"].ToString(), artar["FabricPanelCode"].ToString(), artar["Cutno"].ToString());
                #region Create Bundle

                DataRow nBundle_dr = insert_Bundle.NewRow();
                nBundle_dr["Insert"] = string.Format(
                @"
Insert Into Bundle
(ID               , POID        , mDivisionid, SizeCode , Colorid
 , Article        , PatternPanel, Cutno      , cDate    , OrderID
 , SewingLineid   , Item        , SewingCell , Ratio    , Startno
 , Qty            , AllPart     , CutRef     , AddName  , AddDate
 , FabricPanelCode, IsEXCESS    , SubCutNo) 
values
('{0}'            , '{1}'       , '{2}'      , '{3}'    , '{4}'
 , '{5}'          , '{6}'       , {7}        , GetDate(), '{8}'
 , '{9}'          , '{10}'      , '{11}'     , '{12}'   , '{13}'
 , {14}           , {15}        , '{16}'     , '{17}'   , GetDate()
 , '{18}'         , {19}        , '{20}')",
                id_list[idcount],
                artar["POID"],
                this.keyWord,
                artar["SizeCode"],
                artar["colorid"],
                artar["Article"],
                artar["Fabriccombo"],
                artar["Cutno"],
                artar["orderid"],
                artar["SewingLine"].Empty() ? string.Empty : artar["SewingLine"].ToString().Length > 2 ? artar["SewingLine"].ToString().Substring(0, 2) : artar["SewingLine"].ToString(),
                artar["item"],
                artar["SewingCell"],
                artar["Ratio"],
                startno,
                artar["Qty"],
                artar["TotalParts"],
                artar["Cutref"],
                this.loginID,
                artar["FabricPanelCode"],
                artar["isEXCESS"].EqualString("Y") ? 1 : 0,
                subCut);

                insert_Bundle.Rows.Add(nBundle_dr);
                #endregion

                qtycount = 0;
                patterncount = 0;

                DataRow[] qtyAry = this.qtyTb.Select($"iden={artar["iden"]}");
                DataTable patternAry = this.patternTb.Select($"iden={artar["iden"]} and parts <> 0").CopyToDataTable();
                DataRow[] allPartArt = this.allpartTb.Select($"iden={artar["iden"]} and CombineSubprocessGroup = 0");

                if (patternAry.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Bundle Card info cannot be empty.");
                    return;
                }

                DataTable tmpBundle_Detail = bundle_Detail.Clone();
                DataTable tmpBundle_Detail_Art = bundle_Detail_Art.Clone();
                DataTable tmpbundle_Detail_CombineSubprocess = bundle_Detail_CombineSubprocess.Clone();
                tmpBundle_Detail.Columns.Add("Ukey1", typeof(int));
                tmpBundle_Detail.Columns.Add("tmpNum", typeof(int));
                tmpBundle_Detail.Columns.Add("IsCombineSubProcess", typeof(bool));
                tmpBundle_Detail_Art.Columns.Add("Ukey1", typeof(int));
                tmpbundle_Detail_CombineSubprocess.Columns.Add("Ukey1", typeof(int));
                bool notYetInsertAllPart = true;
                foreach (DataRow rowqty in qtyAry)
                {
                    #region Bundle_Detail_Qty
                    DataRow nBundle_DetailQty_dr = insert_Bundle_Detail_Qty.NewRow();
                    nBundle_DetailQty_dr["Insert"] = string.Format(
                        @"Insert into Bundle_Detail_qty(
                        ID,SizeCode,Qty) Values
                        ('{0}','{1}',{2})",
                        id_list[idcount],
                        artar["SizeCode"],
                        rowqty["Qty"]);
                    insert_Bundle_Detail_Qty.Rows.Add(nBundle_DetailQty_dr);
                    #endregion

                    foreach (DataRow rowPat in patternAry.Rows)
                    {
                        // 不為 Allparts 的依照 Pattern_GL 紀錄 Location，否則帶入空白
                        string location = rowPat["PatternCode"].ToString() == "ALLPARTS" ? string.Empty : rowPat["Location"].ToString();

                        if (MyUtility.Check.Empty(rowPat["PatternCode"]))
                        {
                            MyUtility.Msg.WarningBox("CutPart cannot be empty.");
                            return;
                        }

                        #region Bundle_Detail 改成先準備Datatable
                        DataRow bundleDetail_pre = tmpBundle_Detail.NewRow();
                        bundleDetail_pre["ID"] = id_list[idcount];
                        bundleDetail_pre["Bundleno"] = string.Empty; // bundleno_list[bundlenocount];
                        bundleDetail_pre["BundleGroup"] = startno;
                        bundleDetail_pre["PrintGroup"] = printGroup;
                        bundleDetail_pre["PatternCode"] = rowPat["PatternCode"];
                        bundleDetail_pre["PatternDesc"] = rowPat["PatternDesc"].ToString().Replace("'", "''");
                        bundleDetail_pre["SizeCode"] = artar["SizeCode"];
                        bundleDetail_pre["Qty"] = rowqty["Qty"];
                        bundleDetail_pre["Parts"] = rowPat["Parts"];
                        bundleDetail_pre["Farmin"] = 0;
                        bundleDetail_pre["Farmout"] = 0;
                        bundleDetail_pre["IsPair"] = rowPat["IsPair"];
                        bundleDetail_pre["Location"] = location;
                        bundleDetail_pre["Ukey1"] = bundlenocount;
                        bundleDetail_pre["IsCombineSubProcess"] = artar["IsCombineSubProcess"];
                        tmpBundle_Detail.Rows.Add(bundleDetail_pre);
                        #endregion

                        // Bundle_Detail_art 非空白的Art 才存在
                        if (!MyUtility.Check.Empty(rowPat["art"]))
                        {
                            string[] ann = rowPat["art"].ToString().Split('+');
                            for (int i = 0; i < ann.Length; i++)
                            {
                                int nb = MyUtility.Convert.GetString(rowPat["NoBundleCardAfterSubprocess_String"]).Split('+').ToList().Contains(ann[i].ToString()) ? 1 : 0;
                                int ps = MyUtility.Convert.GetString(rowPat["PostSewingSubProcess_String"]).Split('+').ToList().Contains(ann[i].ToString()) ? 1 : 0;

                                DataRow tmpBundle_Detail_Art_pre = tmpBundle_Detail_Art.NewRow();
                                tmpBundle_Detail_Art_pre["ID"] = id_list[idcount];
                                tmpBundle_Detail_Art_pre["Bundleno"] = string.Empty; // bundleno_list[bundlenocount];
                                tmpBundle_Detail_Art_pre["Subprocessid"] = ann[i];
                                tmpBundle_Detail_Art_pre["PatternCode"] = rowPat["PatternCode"];
                                tmpBundle_Detail_Art_pre["PostSewingSubProcess"] = ps;
                                tmpBundle_Detail_Art_pre["NoBundleCardAfterSubprocess"] = nb;
                                tmpBundle_Detail_Art_pre["Ukey1"] = bundlenocount;
                                tmpBundle_Detail_Art.Rows.Add(tmpBundle_Detail_Art_pre);
                            }
                        }

                        // Bundle_Detail_allpart
                        if (rowPat["PatternCode"].ToString() == "ALLPARTS" && notYetInsertAllPart)
                        {
                            foreach (DataRow rowall in allPartArt)
                            {
                                if (MyUtility.Check.Empty(rowall["Parts"]))
                                {
                                    continue;
                                }

                                if (MyUtility.Check.Empty(rowall["PatternCode"]))
                                {
                                    MyUtility.Msg.WarningBox("All Parts Detail CutPart cannot be empty.");
                                    return;
                                }

                                DataRow nBundleDetailAllPart_dr = insert_Bundle_Detail_AllPart.NewRow();
                                nBundleDetailAllPart_dr["Insert"] = string.Format(
                                    @"Insert Into Bundle_Detail_allpart(ID,PatternCode,PatternDesc,Parts,IsPair,Location) Values('{0}','{1}','{2}','{3}','{4}','{5}')",
                                    id_list[idcount],
                                    rowall["PatternCode"],
                                    rowall["PatternDesc"],
                                    rowall["Parts"],
                                    rowall["IsPair"],
                                    rowall["Location"]);
                                insert_Bundle_Detail_AllPart.Rows.Add(nBundleDetailAllPart_dr);
                            }

                            notYetInsertAllPart = false;
                        }

                        // Bundle_Detail_CombineSubprocess
                        if (MyUtility.Convert.GetBool(artar["IsCombineSubProcess"]))
                        {
                            DataRow[] combineDrs = this.allpartTb.Select($"iden={artar["iden"]} and CombineSubprocessGroup = {rowPat["CombineSubprocessGroup"]} and CombineSubprocessGroup > 0");
                            foreach (DataRow combineDr in combineDrs)
                            {
                                DataRow newdr = tmpbundle_Detail_CombineSubprocess.NewRow();
                                newdr["ID"] = id_list[idcount];
                                newdr["Bundleno"] = string.Empty;
                                newdr["PatternCode"] = combineDr["PatternCode"];
                                newdr["PatternDesc"] = combineDr["PatternDesc"];
                                newdr["Parts"] = combineDr["Parts"];
                                newdr["Location"] = MyUtility.Convert.GetString(combineDr["Location"]);
                                newdr["IsPair"] = combineDr["IsPair"];
                                newdr["IsMain"] = combineDr["IsMain"];
                                newdr["ukey1"] = bundlenocount;
                                tmpbundle_Detail_CombineSubprocess.Rows.Add(newdr);
                            }
                        }

                        bundlenocount++; // 每一筆Bundleno 都不同
                    }

                    startno++;
                    printGroup++;
                    if (autono == 0)
                    {
                        startnoAry[0]["Startno"] = Convert.ToInt16(startnoAry[0]["Startno"]) + 1; // 續編Startno才需要
                    }
                }

                idcount++;

                #region by tone 重新處理 Bundle_Detail, Bundle_Detail_art.  1.< Bundlegroup此圈重鞭馬, by sp重紀錄最大值 >,  2.< Bundle數量會改變, 影響全部 >
                int new_startno = 0; // 紀錄重新處理後bundlegroup最大值
                decimal tone = MyUtility.Convert.GetDecimal(artar["byTone"]);
                decimal noofbundle = MyUtility.Convert.GetDecimal(artar["qty"]);

                // byTone, 即 Bundlegroup 分幾個
                if (tone > 0 && noofbundle > 1)
                {
                    DataTable dtDetail = tmpBundle_Detail.Clone();
                    DataTable dtAllPart = tmpBundle_Detail.Clone();
                    DataTable dtAllPart2 = tmpBundle_Detail.Clone();
                    DataTable dtArt = tmpBundle_Detail_Art.Copy();
                    DataTable dtCombine = tmpbundle_Detail_CombineSubprocess.Copy();
                    int na = tmpBundle_Detail.Select("PatternCode <> 'AllParts'").Length;
                    int a = tmpBundle_Detail.Select("PatternCode = 'AllParts'").Length;
                    if (na > 0)
                    {
                        dtDetail = tmpBundle_Detail.Select("PatternCode <> 'AllParts'").OrderBy(o => o["Ukey1"]).CopyToDataTable();
                    }

                    if (a > 0)
                    {
                        dtAllPart = tmpBundle_Detail.Select("PatternCode = 'AllParts'").OrderBy(o => o["Ukey1"]).CopyToDataTable();
                    }

                    tmpBundle_Detail.Clear();
                    tmpBundle_Detail_Art.Clear();
                    tmpbundle_Detail_CombineSubprocess.Clear();
                    int ukeytone = 1;
                    if (na > 0)
                    {
                        for (int i = 0; i < tone; i++)
                        {
                            int tmpNum = 0;
                            DataTable dtCopy = dtDetail.Copy();
                            foreach (DataRow item in dtCopy.Rows)
                            {
                                item["bundlegroup"] = startno_bytone + i; // 重設bundlegroup
                                item["PrintGroup"] = MyUtility.Convert.GetInt(item["PrintGroup"]) + (i * noofbundle);
                                item["Tone"] = MyUtility.Excel.ConvertNumericToExcelColumn(i + 1);
                                new_startno = startno_bytone + i;

                                item["tmpNum"] = tmpNum; // 暫時紀錄原本資料對應拆出去的資料,要用來重分配Qty
                                tmpNum++;

                                DataTable dtCopyArt = dtArt.Copy();
                                string fukey = $"Ukey1 = {item["Ukey1"]}";
                                foreach (DataRow aarr in dtCopyArt.Select(fukey))
                                {
                                    aarr["Ukey1"] = ukeytone;
                                    tmpBundle_Detail_Art.ImportRow(aarr);
                                }

                                item["Ukey1"] = ukeytone;
                                if (MyUtility.Convert.GetBool(item["IsCombineSubProcess"]))
                                {
                                    DataTable dtCopyCombine = dtCombine.Copy();
                                    foreach (DataRow cbdr in dtCopyCombine.Select(fukey))
                                    {
                                        cbdr["BundleNo"] = string.Empty;
                                        cbdr["Ukey1"] = ukeytone;
                                        tmpbundle_Detail_CombineSubprocess.ImportRow(cbdr);
                                    }
                                }

                                ukeytone++;
                            }

                            tmpBundle_Detail.Merge(dtCopy);
                        }

                        // 重分每一筆拆的Qty
                        int tmpNumF = 0;
                        for (int i = 0; i < tmpBundle_Detail.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Count() / tone; i++, tmpNumF++)
                        {
                            DataRow[] drD = tmpBundle_Detail.Select($"tmpNum={tmpNumF}").AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToArray();
                            Prgs.AverageNumeric(drD, "Qty", MyUtility.Convert.GetInt(drD[0]["Qty"]), true);
                        }

                        tmpBundle_Detail.Columns.Remove("tmpNum");
                    }

                    // 處理All Part筆數
                    if (a > 0)
                    {
                        int ttlAllPartQty = dtAllPart.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted)
                            .Sum(s => MyUtility.Convert.GetInt(s["Qty"]));
                        int allpartPrintGroup = dtAllPart.AsEnumerable().Max(m => MyUtility.Convert.GetInt(m["PrintGroup"]));
                        DataRow row = dtAllPart.Rows[0];
                        for (int i = 0; i < tone; i++)
                        {
                            row["BundleGroup"] = startno_bytone + i;
                            row["PrintGroup"] = (allpartPrintGroup * (i + 1)) - (noofbundle - 1);
                            row["Tone"] = MyUtility.Excel.ConvertNumericToExcelColumn(i + 1);
                            new_startno = startno_bytone + i;
                            int notAllpart = patternAry.Rows.Count - 1;
                            notAllpart = notAllpart == 0 ? 1 : notAllpart;
                            dtAllPart2.ImportRow(row);
                        }

                        // 重分每一筆拆的Qty
                        if (na > 0)
                        {
                            var da = dtAllPart2.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).
                                OrderBy(o => MyUtility.Convert.GetInt(o["PrintGroup"])).ToList();
                            int beforei = da.Count();
                            int upallqty = 0;
                            for (int i = da.Count() - 1; i >= 0; i--)
                            {
                                int qty = tmpBundle_Detail.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted &&
                                   MyUtility.Convert.GetInt(w["PrintGroup"]) >= MyUtility.Convert.GetInt(da[i]["PrintGroup"])).
                                   Sum(s => MyUtility.Convert.GetInt(s["Qty"])) / patternAry.Select($"patternCode <> 'Allparts'").Count();
                                da[i]["qty"] = qty - upallqty;
                                upallqty = qty;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dtAllPart2.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Count() / tone; i++)
                            {
                                DataRow[] drD = dtAllPart2.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).OrderBy(o => o["BundleGroup"]).ToArray();
                                Prgs.AverageNumeric(drD, "Qty", ttlAllPartQty, true);
                            }
                        }

                        DataRow[] drA = dtAllPart2.AsEnumerable().ToArray();
                        foreach (DataRow item in dtAllPart2.Rows)
                        {
                            item["Ukey1"] = ukeytone;
                            ukeytone++;
                        }

                        tmpBundle_Detail.Merge(dtAllPart2);
                    }

                    // 進入by tone 重新處理一次, by sp的startno (BundleGroup) 也要重新記錄
                    if (autono == 0)
                    {
                        startnoAry[0]["Startno"] = new_startno + 1; // 續編Startno才需要
                    }
                }
                #endregion

                #region BundleNo
                int bundlenoNewcount = tmpBundle_Detail.Rows.Count; // 取綁包數量
                foreach (DataRow item in tmpBundle_Detail.Rows)
                {
                    item["Bundleno"] = bundleno_list[bundlenoCount_Record];
                    DataRow[] b_art = tmpBundle_Detail_Art.Select($"Ukey1 = '{item["Ukey1"]}'");
                    foreach (DataRow item2 in b_art)
                    {
                        item2["Bundleno"] = bundleno_list[bundlenoCount_Record];
                    }

                    DataRow[] b_combine = tmpbundle_Detail_CombineSubprocess.Select($"Ukey1 = '{item["Ukey1"]}'");
                    foreach (DataRow item2 in b_combine)
                    {
                        item2["Bundleno"] = bundleno_list[bundlenoCount_Record];
                    }

                    bundlenoCount_Record++;
                }

                #endregion

                // 將這一輪bundle的Bundle_Detail, Bundle_Detail_Art 準備成sql字串
                foreach (DataRow item in tmpBundle_Detail.Rows)
                {
                    DataRow nBundleDetail_dr = insert_Bundle_Detail.NewRow();
                    nBundleDetail_dr["Insert"] = string.Format(
                        @"Insert into Bundle_Detail
                            (ID,Bundleno,BundleGroup,PatternCode,
                            PatternDesc,SizeCode,Qty,Parts,Farmin,Farmout,IsPair ,Location,Tone,PrintGroup) Values
                            ('{0}','{1}',{2},'{3}',
                            '{4}','{5}',{6},{7},0,0,'{8}','{9}','{10}','{11}')",
                        item["ID"],
                        item["Bundleno"],
                        item["BundleGroup"],
                        item["PatternCode"],
                        item["PatternDesc"],
                        item["SizeCode"],
                        item["Qty"],
                        item["Parts"],
                        MyUtility.Convert.GetBool(item["IsPair"]) ? 1 : 0,
                        item["Location"],
                        item["Tone"],
                        item["PrintGroup"]);
                    insert_Bundle_Detail.Rows.Add(nBundleDetail_dr);

                    DataRow drBundleNo = insert_BundleNo.NewRow();
                    drBundleNo["BundleNo"] = item["Bundleno"];
                    insert_BundleNo.Rows.Add(drBundleNo);

                    DataRow nBundleDetail_Order_dr = insert_Bundle_Detail_Order.NewRow();
                    nBundleDetail_Order_dr["Insert"] = $@"
INSERT INTO [dbo].[Bundle_Detail_Order]([ID],[BundleNo],[OrderID],[Qty])
VALUES('{item["ID"]}','{item["Bundleno"]}','{artar["orderid"]}','{item["Qty"]}')
";
                    insert_Bundle_Detail_Order.Rows.Add(nBundleDetail_Order_dr);
                }

                foreach (DataRow item in tmpBundle_Detail_Art.Rows)
                {
                    DataRow nBundleDetailArt_dr = insert_Bundle_Detail_Art.NewRow();
                    nBundleDetailArt_dr["Insert"] = string.Format(
                        @"Insert into Bundle_Detail_art
                        (ID,Bundleno,Subprocessid,PatternCode,PostSewingSubProcess,NoBundleCardAfterSubprocess) Values
                        ('{0}','{1}','{2}','{3}',{4},{5})",
                        item["ID"],
                        item["Bundleno"],
                        item["Subprocessid"],
                        item["PatternCode"],
                        MyUtility.Convert.GetBool(item["PostSewingSubProcess"]) ? 1 : 0,
                        MyUtility.Convert.GetBool(item["NoBundleCardAfterSubprocess"]) ? 1 : 0);
                    insert_Bundle_Detail_Art.Rows.Add(nBundleDetailArt_dr);
                }

                foreach (DataRow item in tmpbundle_Detail_CombineSubprocess.Rows)
                {
                    DataRow newdr = insert_Bundle_Detail_Combinesubprocess.NewRow();
                    newdr["Insert"] = $@"
INSERT INTO [dbo].[Bundle_Detail_CombineSubprocess]([ID],[BundleNo],[PatternCode],[PatternDesc],[Parts],[Location],[IsPair],[IsMain])
Values('{item["ID"]}','{item["Bundleno"]}', '{item["PatternCode"]}', '{item["PatternDesc"]}', '{item["Parts"]}', '{item["Location"]}', '{item["Ispair"]}','{item["IsMain"]}');";
                    insert_Bundle_Detail_Combinesubprocess.Rows.Add(newdr);
                }
            }
            #endregion
            DualResult upResult;
            using (TransactionScope transactionscope = new TransactionScope())
            {
                foreach (DataRow dr in insert_Bundle.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in insert_Bundle_Detail.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in insert_Bundle_Detail_Order.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in insert_Bundle_Detail_Art.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in insert_Bundle_Detail_Combinesubprocess.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in insert_Bundle_Detail_AllPart.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in insert_Bundle_Detail_Qty.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                transactionscope.Complete();
            }

            #region sent data to GZ WebAPI
            Func<List<BundleToAGV_PostBody>> funListBundle = () =>
            {
                string sqlGetData = $@"
select  bd.ID          ,
        b.POID,
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
            };
            Task.Run(() => new Guozi_AGV().SentBundleToAGV(funListBundle))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

            #endregion

            MyUtility.Msg.InfoBox("Successfully");
        }

        private void ChangeLabelTotalCutOutputValue()
        {
            this.labelToalCutOutputValue.Text = this.ArticleSizeTb.Compute("sum(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter).ToString();
            this.labelAccumulateQty.Text = this.ArticleSizeTb.Compute("max(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter).ToString();
        }

        private void ChangeLabelBalanceValue()
        {
            this.labelBalanceValue.Text = this.ArticleSizeTb.Compute("sum(RealCutOutput)-sum(CutOutput)-max(CreatedBundleQty)", this.ArticleSizeTb.DefaultView.RowFilter).ToString();
        }

        private void CopyGridCutRef(bool isSame, string copyCutref = "")
        {
            DataRow selectDr = ((DataRowView)this.gridCutRef.GetSelecteds(SelectedSort.Index)[0]).Row;
            string cutref = selectDr["Cutref"].ToString();
            string filter = string.Empty;
            if (isSame)
            {
                filter += $"Cutref='{cutref}' and ukey<>{selectDr["ukey"]}";
            }
            else
            {
                filter += $"Cutref='{copyCutref}' ";
            }

            DataRow[] cutRefDr = this.CutRefTb.Select(filter);

            foreach (DataRow dr in cutRefDr)
            {
                dr["item"] = selectDr["item"];

                DataRow[] articleAry = this.ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", dr["Ukey"], dr["Fabriccombo"]));
                foreach (DataRow row in articleAry)
                {
                    row["item"] = dr["item"];
                }
            }

            this.gridArticleSize.Refresh();
        }

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

            if (MyUtility.Convert.GetBool(this.gridArticleSize.CurrentDataRow["IsCombineSubprocess", DataRowVersion.Original]) == this.chkCombineSubprocess.Checked)
            {
                return;
            }

            this.gridArticleSize.CurrentDataRow["IsCombineSubprocess"] = this.chkCombineSubprocess.Checked;
            this.gridArticleSize.CurrentDataRow.AcceptChanges();
            this.ChangeDefault();
            this.DeleteAllpartsDatas();
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

        private void ChkNoneShellNoCreateAllParts_CheckedChanged(object sender, EventArgs e)
        {
            if (this.gridCutRef.CurrentDataRow == null)
            {
                return;
            }

            if (MyUtility.Convert.GetBool(this.gridArticleSize.CurrentDataRow["IsNoneShellNoCreateAllParts", DataRowVersion.Original]) == this.chkNoneShellNoCreateAllParts.Checked)
            {
                return;
            }

            this.gridArticleSize.CurrentDataRow["IsNoneShellNoCreateAllParts"] = this.chkNoneShellNoCreateAllParts.Checked;
            this.gridArticleSize.CurrentDataRow.AcceptChanges();

            this.ChangeDefault();
            this.DeleteAllpartsDatas();
            this.GridAutoResizeColumns();
        }

        private void ChangeDefault()
        {
            if (this.CutRefTb == null || this.gridCutRef.CurrentDataRow == null || this.gridArticleSize.CurrentDataRow == null)
            {
                return;
            }

            int iden = MyUtility.Convert.GetInt(this.gridArticleSize.CurrentDataRow["iden"]);
            this.patternTb.AcceptChanges();
            this.allpartTb.AcceptChanges();
            this.patternTb.Select($"iden = {iden}").Delete();
            this.allpartTb.Select($"iden = {iden}").Delete();
            this.patternTb.AcceptChanges();
            this.allpartTb.AcceptChanges();
            DataTable pdt = this.patternTb.Clone();
            DataTable adt = this.allpartTbOri.Select($"iden = {iden}").TryCopyToDataTable(this.allpartTb);
            DataTable xdt = this.patternTbOri.Select($"iden = {iden}").TryCopyToDataTable(this.patternTb);

            if (!this.chkCombineSubprocess.Checked)
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
            this.CombineSubprocessIspair(iden);
            this.Calpart();
        }

        private void CombineSubprocessIspair(long iden)
        {
            if (MyUtility.Convert.GetBool(this.gridArticleSize.CurrentDataRow["IsCombineSubprocess"]))
            {
                this.patternTb.Select($@"iden = {iden}").ToList().ForEach(f => f["isPair"] = false);
            }
        }

        private void DeleteAllpartsDatas()
        {
            if (MyUtility.Convert.GetString(this.gridCutRef.CurrentDataRow["FabricKind"]) != "1" && this.chkNoneShellNoCreateAllParts.Checked)
            {
                this.allpartTb.Select($"iden = {this.gridArticleSize.CurrentDataRow["iden"]}  and CombineSubprocessGroup = 0").Delete();
                this.allpartTb.AcceptChanges();
                this.Calpart();
            }
        }
    }
}
