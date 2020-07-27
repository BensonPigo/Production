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
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using Sci.Win.UI;
using static Sci.Production.Automation.Guozi_AGV;

namespace Sci.Production.Cutting
{
    public partial class P11 : Win.Tems.QueryForm
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;
        DataTable CutRefTb;
        DataTable ArticleSizeTb;
        DataTable ExcessTb;
        DataTable GarmentTb;
        DataTable allpartTb;
        DataTable patternTb;
        DataTable artTb;
        DataTable qtyTb;
        DataTable SizeRatioTb;
        DataTable headertb;
        DataTable ArticleSizeTb_View;
        string f_code;

        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string cmd_st = "Select 0 as sel,PatternCode,PatternDesc, '' as annotation,parts,'' as cutref,'' as poid, 0 as iden,ispair ,Location from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out this.allpartTb);

            string pattern_cmd = "Select patternCode,PatternDesc,Parts,'' as art,0 AS parts, '' as cutref,'' as poid, 0 as iden,ispair ,Location,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_Detail WITH (NOLOCK) Where 1=0"; // 左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out this.patternTb);

            string cmd_art = "Select PatternCode,subprocessid,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_detail_art WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_art, out this.artTb);

            string cmd_qty = "Select 0 as No,qty,'' as orderid,'' as cutref,'' as article, SizeCode, 0 as iden from Bundle_Detail_Qty WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_qty, out this.qtyTb);
            this.InitializeComponent();
            this.gridSetup();
        }

        public void gridSetup()
        {
            #region 右鍵事件
            Ict.Win.UI.DataGridViewTextBoxColumn item;
            DataGridViewGeneratorTextColumnSettings Linecell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings Cellcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings Qtycell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cutOutputCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings QtySizecell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partQtyCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partQtyCell2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings subcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patternDesc = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings chcutref = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings itemsetting = new DataGridViewGeneratorTextColumnSettings();

            DataGridViewGeneratorCheckBoxColumnSettings charticle = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings selectExcess = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings isPair = new DataGridViewGeneratorCheckBoxColumnSettings();

            DataGridViewGeneratorTextColumnSettings NoBundleCardAfterSubprocess_String = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings PostSewingSubProcess_String = new DataGridViewGeneratorTextColumnSettings();

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
            Linecell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    string sql = string.Format(
                        @"Select DISTINCT ID  From SewingLine WITH (NOLOCK) 
                        where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}')", Env.User.Keyword);
                    sele = new SelectItem(sql, "10", dr["SewingLine"].ToString());
                    sele.Width = 300;
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            Linecell.CellValidating += (s, e) =>
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
            Cellcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    sele = new SelectItem("Select SewingCell from Sewingline WITH (NOLOCK) where SewingCell!='' group by SewingCell", "10", dr["SewingCell"].ToString());
                    sele.Width = 300;
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            Cellcell.CellValidating += (s, e) =>
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
            Qtycell.CellValidating += (s, e) =>
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
                this.distSizeQty(rowcount, newcount, dr);
            };

            cutOutputCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridArticleSize.GetDataRow(e.RowIndex);
                int oldCutOutput = Convert.ToInt32(dr["cutOutput"]);
                int newCutOutput = Convert.ToInt32(e.FormattedValue);
                dr["cutOutput"] = newCutOutput;
                dr.EndEdit();

                int newBalance = Convert.ToInt32(this.ArticleSizeTb.Compute("sum(CutOutput)-sum(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter));

                if (newBalance > 0)
                {
                    MyUtility.Msg.InfoBox("Balance can not more than zero.");
                    dr["cutOutput"] = oldCutOutput;
                    dr.EndEdit();
                    return;
                }

                if (oldCutOutput.EqualString(newCutOutput) == false)
                {
                    this.changeLabelBalanceValue();

                    int rowcount = this.qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), string.Empty).Length;
                    int newcount = MyUtility.Convert.GetInt(dr["Qty"]);
                    this.distSizeQty(rowcount, newcount, dr);
                }
            };

            QtySizecell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridQty.GetDataRow(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr.EndEdit();
                int qty = MyUtility.Convert.GetInt(this.qtyTb.Compute("Sum(Qty)", string.Format("iden ='{0}'", dr["iden"])));
                this.label_TotalQty.Text = qty.ToString();
            };

            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
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
                        dr["isPair"] = this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}' and iden = '{dr["iden"]}'")[0]["isPair"];
                    }

                    e.EditingControl.Text = sele.GetSelectedString();
                    dr["PatternDesc"] = sele.GetSelecteds()[0]["PatternDesc"].ToString();
                    dr["PatternCode"] = sele.GetSelecteds()[0]["PatternCode"].ToString();
                    string[] ann = Regex.Replace(sele.GetSelecteds()[0]["Annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation
                    string art = string.Empty;
                    bool lallpart;
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), this.artTb, out lallpart);
                    }
                    #endregion
                    dr["art"] = art;
                    dr["parts"] = 1;
                    dr.EndEdit();
                    this.CheckNotMain(dr);
                }
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                string oldvalue = dr["PatternCode"].ToString();
                if (oldvalue == patcode)
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
                    bool lallpart;
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), this.artTb, out lallpart);
                    }
                    #endregion
                    dr["art"] = art;
                    dr["parts"] = 1;
                }

                dr.EndEdit();
                this.calpart();
                this.CheckNotMain(dr);
            };
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
                    this.calpart();
                }
            };

            patternDesc.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                dr["PatternDesc"] = e.FormattedValue;
                dr.EndEdit();
                this.CheckNotMain(dr);
            };

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
            PostSewingSubProcess_String.EditingMouseDown += (s, e) =>
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
            PostSewingSubProcess_String.CellFormatting += (s, e) =>
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
            NoBundleCardAfterSubprocess_String.EditingMouseDown += (s, e) =>
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
            NoBundleCardAfterSubprocess_String.CellFormatting += (s, e) =>
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

            partQtyCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.calpart();
            };
            partQtyCell2.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridAllPart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.calpart();
            };
            chcutref.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutRef.GetDataRow(e.RowIndex);
                if (this.ArticleSizeTb != null)
                {
                    if ((bool)e.FormattedValue == (dr["sel"].ToString() == "1" ? true : false))
                    {
                        return;
                    }

                    int oldvalue = Convert.ToInt16(dr["sel"]);
                    int newvalue = Convert.ToInt16(e.FormattedValue);
                    DataRow[] ArticleAry = this.ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", dr["Ukey"], dr["Fabriccombo"]));

                    foreach (DataRow row in ArticleAry)
                    {
                        row["Sel"] = newvalue;
                    }

                    dr["sel"] = newvalue;
                    dr.EndEdit();
                    this.gridArticleSize.Refresh();
                }
            };
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
                    DataRow[] ArticleAry = this.ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", dr["Ukey"], dr["Fabriccombo"]));
                    foreach (DataRow row in ArticleAry)
                    {
                        row["item"] = dr["item"];
                    }

                    this.gridArticleSize.Refresh();
                }
            };
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

                DataRow[] ArtAry = this.ArticleSizeTb.Select(string.Format("Sel=1 and Cutref='{0}'", dr["Cutref"]));
                if (ArtAry.Length == 0)
                {
                    selectDr["Sel"] = 0;
                }
                else
                {
                    selectDr["Sel"] = 1;
                }

                this.gridCutRef.Refresh();
            };
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

            isPair.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridCutpart.GetDataRow(e.RowIndex);
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
           .Text("Item", header: "Item", width: Widths.AnsiChars(20), iseditingreadonly: false, settings: itemsetting).Get(out item)
           .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(5), iseditingreadonly: true)
           .Text("FabricKind", header: "Fabric Kind", width: Widths.AnsiChars(5), iseditingreadonly: true)
           ;

            this.gridCutRef.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutRef.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutRef.Columns["Item"].DefaultCellStyle.BackColor = Color.Pink;
            item.MaxLength = 20;

            #endregion
            #region 右上一Grid

            this.gridArticleSize.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridArticleSize)
           .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: charticle)
           .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true, settings: selectExcess)
           .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: selectExcess)
           .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: selectExcess)
           .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: selectExcess)
           .Text("SewingLine", header: "Line#", width: Widths.AnsiChars(2), settings: Linecell)
           .Text("SewingCell", header: "Sew" + Environment.NewLine + "Cell", width: Widths.AnsiChars(2), settings: Cellcell)
           .Numeric("Qty", header: "No of" + Environment.NewLine + "Bundle", width: Widths.AnsiChars(3), integer_places: 3, settings: Qtycell)
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
            #region 左下一Qty
            this.gridQty.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridQty)
           .Numeric("No", header: "No", width: Widths.AnsiChars(3), integer_places: 2, iseditingreadonly: true)
           .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4), integer_places: 3, settings: QtySizecell);
            this.gridQty.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridQty.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            #region 下中一Cutpart-Pattern
            this.gridCutpart.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridCutpart)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15), settings: patternDesc)
            .Text("Location", header: "Location", iseditingreadonly: true, width: Widths.AnsiChars(5))
            .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: subcell)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell)
            .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: isPair)
            .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: PostSewingSubProcess_String)
            .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: NoBundleCardAfterSubprocess_String)
            ;
            this.gridCutpart.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutpart.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            this.gridCutpart.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCutpart.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCutpart.Columns["art"].DefaultCellStyle.BackColor = Color.SkyBlue;
            this.gridCutpart.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCutpart.Columns["IsPair"].DefaultCellStyle.BackColor = Color.Pink;

            #endregion
            #region 右下一AllPart_grid
            this.gridAllPart.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridAllPart)
            .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13))
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

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 300;
            string cutref = this.txtCutref.Text;
            string cutdate = this.dateEstCutDate.Value == null ? string.Empty : this.dateEstCutDate.Value.Value.ToShortDateString();
            string factory = this.txtfactoryByM.Text;
            string poid = this.txtPOID.Text;
            string SpreadingNoID = this.txtSpreadingNo1.Text;
            #region Clear Table
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
            this.gridCutpart.DataSource = null;
            this.gridAllPart.DataSource = null;
            this.gridQty.DataSource = null;
            this.gridArticleSize.DataSource = null;
            this.gridCutRef.DataSource = null;
            #endregion

            // 判斷必須有一條件存在
            if (MyUtility.Check.Empty(cutref) && MyUtility.Check.Empty(cutdate) && MyUtility.Check.Empty(poid))
            {
                MyUtility.Msg.WarningBox("The Condition can not empty.");
                return;
            }

            if (this.chkAEQ.Checked)
            {
                this.gridArticleSize.Columns["isEXCESS"].Visible = true;
            }
            else
            {
                this.gridArticleSize.Columns["isEXCESS"].Visible = false;
            }

            this.ShowWaitMessage("Query");
            #region 條件式
            string query_cmd = $@"
Select  distinct 0 as sel
        , a.cutref
        , ord.poid
        , a.estcutdate
        , a.Fabriccombo
        , a.FabricPanelCode
        , a.cutno
        , item = ( Select Reason.Name 
                   from   Reason WITH (NOLOCK) 
                          , Style WITH (NOLOCK) 
                   where   Reason.Reasontypeid ='Style_Apparel_Type' 
                           and Style.ukey = ord.styleukey 
                           and Style.ApparelType = Reason.id ) 
        , a.SpreadingNoID
        , a.Ukey
        , FabricKind = FabricKind.value
from  workorder a WITH (NOLOCK) 
inner join orders ord WITH (NOLOCK) on a.orderid = ord.id and a.id = ord.cuttingsp 
inner join workorder_PatternPanel b WITH (NOLOCK) on a.ukey = b.workorderukey 
outer apply(
    SELECT TOP 1 value = DD.id + '-' + DD.NAME 
    FROM dropdownlist DD 
    OUTER apply(
	    SELECT
		    OB.kind, 
		    OCC.id, 
		    OCC.article, 
		    OCC.colorid, 
		    OCC.fabricpanelcode, 
		    OCC.patternpanel 
	    FROM order_colorcombo OCC 
	    INNER JOIN order_bof OB 
	    ON OCC.id = OB.id 
	    AND OCC.fabriccode = OB.fabriccode
    ) LIST 
    WHERE LIST.id = a.id
    AND LIST.patternpanel = b.patternpanel
    AND DD.[type] = 'FabricKind'
    AND DD.id = LIST.kind
)FabricKind
where  ord.mDivisionid = '{this.keyWord}' 
and isnull(a.CutRef,'') <> ''
";

            string distru_cmd = string.Format(
                @"
Select  distinct 0 as sel
        , 0 as iden
        , a.cutref
        , b.orderid
        , b.article
        , a.colorid
        , b.sizecode
        , a.Fabriccombo
		, a.FabricPanelCode
        , '' as Ratio
        , a.cutno
        , Sewingline = ord.SewLine
        , SewingCell= a.CutCellid
        , item = (  Select Reason.Name 
		            from Reason WITH (NOLOCK) 
                         , Style WITH (NOLOCK) 
		            where   Reason.Reasontypeid = 'Style_Apparel_Type' 
                            and Style.ukey = ord.styleukey 
                            and Style.ApparelType = Reason.id )
	    , 1 as Qty
        , cutoutput = isnull(sum(b.Qty),0)
        , RealCutOutput = isnull(sum(b.Qty),0)
        , 0 as TotalParts
        , ord.poid
        , 0 as startno
	    , a.Ukey
	    , ord.StyleUkey
		, isEXCESS = ''
        , byTone = cast((select AutoGenerateByTone from system)as int)
from workorder a WITH (NOLOCK) 
inner join orders ord WITH (NOLOCK) on a.id = ord.cuttingsp
inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey and a.id = b.id and b.orderid = ord.id
Where   isnull(a.CutRef,'') <> ''
        and ord.mDivisionid = '{0}'", this.keyWord);

            string distru_cmd_Excess = $@"
union all
Select  distinct 0 as sel
        , 0 as iden
        , a.cutref
        , l.orderid
        , l.article
        , a.colorid
        , l.sizecode
        , a.Fabriccombo
		, a.FabricPanelCode
        , '' as Ratio
        , a.cutno
        , Sewingline = ord.SewLine
        , SewingCell= a.CutCellid
        , item = (  Select Reason.Name 
		            from Reason WITH (NOLOCK) 
                         , Style WITH (NOLOCK) 
		            where   Reason.Reasontypeid = 'Style_Apparel_Type' 
                            and Style.ukey = ord.styleukey 
                            and Style.ApparelType = Reason.id )
	    , 1 as Qty
        , cutoutput = isnull(b.Qty,0)
        , RealCutOutput = isnull(b.Qty,0)
        , 0 as TotalParts
        , ord.poid
        , 0 as startno
	    , a.Ukey
	    , ord.StyleUkey
		, isEXCESS = 'Y'
        , byTone = cast((select AutoGenerateByTone from system)as int)
from workorder a WITH (NOLOCK) 
inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey and a.id = b.id 
outer apply(
	select top 1 wd.OrderID,wd.Article,wd.SizeCode
	from workorder_Distribute wd WITH(NOLOCK)
	where wd.WorkOrderUkey = a.Ukey and wd.orderid <>'EXCESS'
	order by wd.OrderID desc
)l
inner join orders ord WITH (NOLOCK) on a.id = ord.cuttingsp and l.OrderID = ord.id
Where   isnull(a.CutRef,'') <> '' 
        and ord.mDivisionid = '{this.keyWord}'  and b.orderid ='EXCESS'
";

            string Excess_cmd = string.Format(
                @"
Select  distinct a.cutref
        , a.orderid
from    workorder a WITH (NOLOCK) 
        , workorder_Distribute b WITH (NOLOCK) 
        , orders ord WITH (NOLOCK) 
Where   a.ukey = b.workorderukey 
        and ord.mDivisionid = '{0}'   
        and a.id = b.id 
        and b.orderid = 'EXCESS' 
        and a.id = ord.cuttingsp 
        and isnull(a.CutRef,'') <> '' ", this.keyWord);

            StringBuilder SizeRatio = new StringBuilder();
            SizeRatio.Append(string.Format(
                @"
;with tmp as(
	Select  distinct a.cutref
            , b.sizecode
            , a.Ukey
	from workorder a WITH (NOLOCK) 
	inner join orders ord WITH (NOLOCK) on a.id = ord.cuttingsp
	inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey and a.id = b.id and b.orderid = ord.id
	inner join workorder_PatternPanel c WITH (NOLOCK) on a.ukey = c.workorderukey and c.id = a.id
	Where   isnull(a.CutRef,'') <> '' 
            and ord.mDivisionid = '{0}'
", this.keyWord));
            #endregion
            #region where 條件
            if (!MyUtility.Check.Empty(cutref))
            {
                query_cmd = query_cmd + string.Format(" and a.cutref='{0}'", cutref);
                distru_cmd = distru_cmd + string.Format(" and a.cutref='{0}'", cutref);
                distru_cmd_Excess += string.Format(" and a.cutref='{0}'", cutref);
                Excess_cmd = Excess_cmd + string.Format(" and a.cutref='{0}'", cutref);
                SizeRatio.Append(string.Format(" and a.cutref='{0}'", cutref));
            }

            if (!MyUtility.Check.Empty(this.dateEstCutDate.Value))
            {
                query_cmd = query_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                distru_cmd = distru_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                distru_cmd_Excess += string.Format(" and a.estcutdate='{0}'", cutdate);
                Excess_cmd = Excess_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                SizeRatio.Append(string.Format(" and a.estcutdate='{0}'", cutdate));
            }

            if (!MyUtility.Check.Empty(poid))
            {
                query_cmd = query_cmd + string.Format(" and ord.poid='{0}'", poid);
                distru_cmd = distru_cmd + string.Format(" and ord.poid='{0}'", poid);
                distru_cmd_Excess += string.Format(" and ord.poid='{0}'", poid);
                Excess_cmd = Excess_cmd + string.Format(" and  ord.poid='{0}'", poid);
                SizeRatio.Append(string.Format(" and ord.poid='{0}'", poid));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                query_cmd = query_cmd + string.Format(" and ord.FtyGroup='{0}'", factory);
                distru_cmd = distru_cmd + string.Format(" and ord.FtyGroup='{0}'", factory);
                distru_cmd_Excess += string.Format(" and ord.FtyGroup='{0}'", factory);
                Excess_cmd = Excess_cmd + string.Format(" and  ord.FtyGroup='{0}'", factory);
                SizeRatio.Append(string.Format(" and ord.FtyGroup='{0}'", factory));
            }

            if (!MyUtility.Check.Empty(SpreadingNoID))
            {
                query_cmd = query_cmd + string.Format(" and a.SpreadingNoID='{0}'", SpreadingNoID);
                distru_cmd = distru_cmd + string.Format(" and a.SpreadingNoID='{0}'", SpreadingNoID);
                distru_cmd_Excess += string.Format(" and a.SpreadingNoID='{0}'", SpreadingNoID);
                Excess_cmd = Excess_cmd + string.Format(" and  a.SpreadingNoID='{0}'", SpreadingNoID);
                SizeRatio.Append(string.Format(" and a.SpreadingNoID='{0}'", SpreadingNoID));
            }
            #endregion

            query_cmd = query_cmd + " order by ord.poid,a.estcutdate,a.Fabriccombo,a.cutno";

            DualResult query_dResult = DBProxy.Current.Select(null, query_cmd, out this.CutRefTb);
            if (!query_dResult)
            {
                this.ShowErr(query_cmd, query_dResult);
                return;
            }

            if (this.CutRefTb.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data Found!");
                this.HideWaitMessage();
                return;
            }

            // Mantis_7045 將PatternPanel改成FabricPanelCode,不然會有些值不正確
            distru_cmd = distru_cmd + @" and b.orderid !='EXCESS' and isnull(a.CutRef,'') <> '' 
group by a.cutref,b.orderid,b.article,a.colorid,b.sizecode,ord.Sewline,ord.factoryid,ord.poid,a.Fabriccombo,a.FabricPanelCode,a.cutno,ord.styleukey,a.CutCellid,a.Ukey  --,ag.ArticleGroup
";
            if (this.chkAEQ.Checked)
            {
                distru_cmd += distru_cmd_Excess;
            }
            else
            {
                distru_cmd += " order by b.sizecode,b.orderid,a.FabricPanelCode";
            }

            query_dResult = DBProxy.Current.Select(null, distru_cmd, out this.ArticleSizeTb);
            if (!query_dResult)
            {
                this.ShowErr(distru_cmd, query_dResult);
                return;
            }

            SizeRatio.Append(@"
)
Select  b.Cutref
        , a.SizeCode
        , a.Qty
From Workorder_SizeRatio a WITH (NOLOCK)
inner join workorder c WITH (NOLOCK) on c.ukey = a.workorderukey 
inner join tmp b WITH (NOLOCK) on  b.sizecode = a.sizecode and b.Ukey = c.Ukey");
            query_dResult = DBProxy.Current.Select(null, SizeRatio.ToString(), out this.SizeRatioTb);
            if (!query_dResult)
            {
                this.ShowErr(distru_cmd, query_dResult);
                return;
            }

            #region 若有EXCESS 需顯示通知
            query_dResult = DBProxy.Current.Select(null, Excess_cmd, out this.ExcessTb);
            if (!query_dResult)
            {
                this.ShowErr(Excess_cmd, query_dResult);
                return;
            }
            #endregion

            #region articleSizeTb 繞PO 找出QtyTb,PatternTb,AllPartTb
            int iden = 1;

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

                // MANTIS 9044
                // createPattern(dr["POID"].ToString(), dr["Article"].ToString(), dr["FabricPanelCode"].ToString(), dr["Cutref"].ToString(), iden, dr["ArticleGroup"].ToString());
                this.ArticleSizeTb_View = this.ArticleSizeTb.Select($"Ukey ='{dr["Ukey"]}' and Fabriccombo = '{dr["Fabriccombo"]}'").CopyToDataTable();
                this.createPattern(dr["POID"].ToString(), dr["Article"].ToString(), dr["FabricPanelCode"].ToString(), dr["Cutref"].ToString(), iden, string.Empty);
                int totalpart = MyUtility.Convert.GetInt(this.patternTb.Compute("sum(Parts)", string.Format("iden ={0}", iden)));
                dr["TotalParts"] = totalpart;
                iden++;
            }
            #endregion

            this.gridCutRef.DataSource = this.CutRefTb;
            this.gridArticleSize.DataSource = this.ArticleSizeTb; // 右上
            this.gridQty.DataSource = this.qtyTb;
            this.gridAllPart.DataSource = this.allpartTb;
            this.gridCutpart.DataSource = this.patternTb;

            this.gridCutRef.AutoResizeColumns();
            this.gridArticleSize.AutoResizeColumns();
            this.gridCutpart.AutoResizeColumns();

            this.HideWaitMessage();

            // 若有勾則自動分配Excess
            if (!this.chkAEQ.Checked && this.ExcessTb.Rows.Count > 0)
            {
                MsgGridForm m = new MsgGridForm(this.ExcessTb, "Those detail had <EXCESS> not yet distribute to SP#", "Warning");

                // var m = MyUtility.Msg.ShowMsgGrid(ExcessTb, "Those detail had <EXCESS> not yet distribute to SP#", "Warning");
                m.Width = 650;
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

        public void createPattern(string poid, string article, string patternpanel, string cutref, int iden, string ArticleGroup)
        {
            if (ArticleGroup == string.Empty)
            {
                this.f_code = "F_Code";
            }
            else
            {
                this.f_code = ArticleGroup;
            }

            // 找出相同PatternPanel 的subprocessid
            int npart = 0; // allpart 數量
            string patidsql;
            DataTable garmentListTb;
            #region 輸出GarmentTb
            string Styleyukey = MyUtility.GetValue.Lookup("Styleukey", poid, "Orders", "ID");

            var Sizelist = this.ArticleSizeTb_View.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();
            string sizes = "'" + string.Join("','", Sizelist) + "'";
            string sqlSizeGroup = $@"SELECT TOP 1 IIF(ISNULL(SizeGroup,'')='','N',SizeGroup) FROM Order_SizeCode WHERE ID ='{poid}' and SizeCode IN ({sizes})";
            string sizeGroup = MyUtility.GetValue.Lookup(sqlSizeGroup);
            patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{poid}','{cutref}','',{Styleyukey},'{sizeGroup}')s";

            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            string headercodesql = string.Format(
                @"
Select distinct ArticleGroup 
from Pattern_GL_LectraCode WITH (NOLOCK) 
where PatternUkey = '{0}' 
order by ArticleGroup", patternukey);

            DualResult headerResult = DBProxy.Current.Select(null, headercodesql, out this.headertb);
            if (!headerResult)
            {
                return;
            }
            #region 建立Table
            string tablecreatesql = string.Format(@"Select '{0}' as orderid,a.*,'' as F_CODE", poid);
            foreach (DataRow dr in this.headertb.Rows)
            {
                tablecreatesql = tablecreatesql + string.Format(" ,'' as {0}", dr["ArticleGroup"]);
            }

            tablecreatesql = tablecreatesql + string.Format(" from Pattern_GL a WITH (NOLOCK) Where PatternUkey = '{0}'", patternukey);
            DualResult tablecreateResult = DBProxy.Current.Select(null, tablecreatesql, out garmentListTb);
            if (!tablecreateResult)
            {
                return;
            }
            #endregion

            #region 寫入FCode~CodeA~CodeZ
            string lecsql = string.Empty;
            lecsql = string.Format("Select * from Pattern_GL_LectraCode a WITH (NOLOCK) where a.PatternUkey = '{0}'", patternukey);
            DataTable drtb;
            DualResult drre = DBProxy.Current.Select(null, lecsql, out drtb);
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
            DataRow[] garmentar = this.GarmentTb.Select(w.ToString());
            foreach (DataRow dr in garmentar)
            {
                if (MyUtility.Check.Empty(dr["annotation"])) // 若無ANNOTATion直接寫入All Parts
                {
                    DataRow ndr = this.allpartTb.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["Location"] = dr["Location"];
                    ndr["parts"] = MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                    ndr["Cutref"] = cutref;
                    ndr["POID"] = poid;
                    ndr["iden"] = iden;
                    ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                    this.allpartTb.Rows.Add(ndr);
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
                        bool lallpart;
                        #region 算Subprocess

                        // artTb 用不到只是因為共用BundleCardCheckSubprocess PRG其他需要用到
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), this.artTb, out lallpart);
                        #endregion
                        if (!lallpart)
                        {
                            if (dr["DV"].ToString() != "0" || dr["Pair"].ToString() != "0")
                            {
                                int count = (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                                for (int i = 0; i < count; i++)
                                {
                                    DataRow ndr2 = this.patternTb.NewRow();
                                    ndr2["PatternCode"] = dr["PatternCode"];
                                    ndr2["PatternDesc"] = dr["PatternDesc"];
                                    ndr2["Location"] = dr["Location"];
                                    ndr2["Parts"] = 1;
                                    ndr2["art"] = art;
                                    ndr2["POID"] = poid;
                                    ndr2["Cutref"] = cutref;
                                    ndr2["iden"] = iden;
                                    ndr2["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                                    ndr2["NoBundleCardAfterSubprocess_String"] = noBundleCardAfterSubprocess_String;
                                    this.patternTb.Rows.Add(ndr2);
                                }
                            }
                            else
                            {
                                DataRow ndr2 = this.patternTb.NewRow();
                                ndr2["PatternCode"] = dr["PatternCode"];
                                ndr2["PatternDesc"] = dr["PatternDesc"];
                                ndr2["Location"] = dr["Location"];
                                ndr2["art"] = art;
                                ndr2["Parts"] = dr["alone"];
                                ndr2["POID"] = poid;
                                ndr2["Cutref"] = cutref;
                                ndr2["iden"] = iden;
                                ndr2["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                                ndr2["NoBundleCardAfterSubprocess_String"] = noBundleCardAfterSubprocess_String;
                                this.patternTb.Rows.Add(ndr2);
                            }
                        }
                        else
                        {
                            DataRow ndr = this.allpartTb.NewRow();
                            ndr["PatternCode"] = dr["PatternCode"];
                            ndr["PatternDesc"] = dr["PatternDesc"];
                            ndr["Annotation"] = dr["Annotation"];
                            ndr["Location"] = dr["Location"];
                            ndr["POID"] = poid;
                            ndr["Cutref"] = cutref;
                            ndr["iden"] = iden;
                            ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                            this.allpartTb.Rows.Add(ndr);
                        }
                    }
                    else
                    {
                        DataRow ndr = this.allpartTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["PatternDesc"] = dr["PatternDesc"];
                        ndr["Annotation"] = dr["Annotation"];
                        ndr["Location"] = dr["Location"];
                        ndr["POID"] = poid;
                        ndr["Cutref"] = cutref;
                        ndr["iden"] = iden;
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        ndr["isPair"] = MyUtility.Convert.GetInt(dr["Pair"]) == 1;
                        this.allpartTb.Rows.Add(ndr);
                    }
                    #endregion
                }
            }

            DataRow pdr = this.patternTb.NewRow(); // 預設要有ALLPARTS
            pdr["PatternCode"] = "ALLPARTS";
            pdr["PatternDesc"] = "All Parts";
            pdr["parts"] = npart;
            pdr["Cutref"] = cutref;
            pdr["POID"] = poid;
            pdr["iden"] = iden;
            this.patternTb.Rows.Add(pdr);

            DBProxy.Current.DefaultTimeout = 0;
        }

        private void gridCutRef_SelectionChanged(object sender, EventArgs e)
        {
            this.changeRow();
        }

        public void changeRow()
        {
            DataRow selectDr_Cutref;
            DataRow selectDr_Artsize;

            if (this.CutRefTb == null)
            {
                return;
            }

            if (this.CutRefTb.Rows.Count == 0)
            {
                return;
            }

            if (this.gridCutRef.GetSelectedRowIndex() == -1)
            {
                selectDr_Cutref = this.CutRefTb.Rows[0];
            }
            else
            {
                selectDr_Cutref = ((DataRowView)this.gridCutRef.GetSelecteds(SelectedSort.Index)[0]).Row;
            }

            this.ArticleSizeTb.DefaultView.RowFilter
                = string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", selectDr_Cutref["Ukey"], selectDr_Cutref["Fabriccombo"]);
            if (this.ArticleSizeTb.Rows.Count == 0)
            {
                return;
            }

            this.ArticleSizeTb_View = this.ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", selectDr_Cutref["Ukey"], selectDr_Cutref["Fabriccombo"])).CopyToDataTable();

            if (this.gridArticleSize.GetSelectedRowIndex() == -1)
            {
                selectDr_Artsize = this.ArticleSizeTb.Rows[0];
            }
            else
            {
                selectDr_Artsize = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            }

            this.qtyTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            this.allpartTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            this.patternTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            this.label_TotalCutOutput.Text = selectDr_Artsize["Cutoutput"].ToString();
            this.numNoOfBundle.Value = Convert.ToInt16(selectDr_Artsize["Qty"]);
            this.numTotalPart.Value = Convert.ToInt16(selectDr_Artsize["TotalParts"]);
            this.numTone.Value = Convert.ToInt16(selectDr_Artsize["byTone"]);
            this.chkTone.Checked = this.numTone.Value > 0;
            this.label_TotalQty.Text = this.qtyTb.Compute("Sum(Qty)", string.Format("iden={0}", selectDr_Artsize["iden"])).ToString();

            this.changeLabelTotalCutOutputValue();
            this.changeLabelBalanceValue();
        }

        public void distSizeQty(int rowcount, int newcount, DataRow dr) // 計算Size Qty
        {
            if (rowcount <= newcount) // 缺少需先新增
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
            double TotalCutQty = MyUtility.Convert.GetDouble(this.label_TotalQty.Text);
            DataRow[] qtyarry = this.qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), string.Empty);
            if (TotalCutQty % newcount == 0)
            {
                int qty = (int)(TotalCutQty / newcount); // 每一個數量是多少
                foreach (DataRow dr2 in qtyarry)
                {
                    dr2["Qty"] = qty;
                }
            }
            else
            {
                int eachqty = (int)Math.Floor(TotalCutQty / newcount);
                int modqty = (int)(TotalCutQty % newcount); // 剩餘數

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
        }

        private void btn_LefttoRight_Click(object sender, EventArgs e)
        {
            this.gridvalid();

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

            string art = selectartDr["art"].ToString();

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
            if (patterndr.Length > 0) // 刪除後還有相同Pattern 需要判斷是否Subprocess都存在
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

            this.calpart();
        }

        private void btn_RighttoLeft_Click(object sender, EventArgs e)
        {
            this.gridvalid();
            if (this.patternTb.Rows.Count == 0 || this.gridAllPart.Rows.Count == 0)
            {
                return;
            }

            DataRow selectartDr = ((DataRowView)this.gridCutpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow selectallparteDr = ((DataRowView)this.gridAllPart.GetSelecteds(SelectedSort.Index)[0]).Row;

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
                        bool lallpart;
                        #region 算Subprocess
                        art = Prgs.BundleCardCheckSubprocess(ann, chdr["PatternCode"].ToString(), this.artTb, out lallpart);
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

                    // ndr2["art"] = art;
                    ndr2["art"] = "EMB";
                    ndr2["poid"] = chdr["poid"];
                    ndr2["Cutref"] = chdr["cutref"];
                    ndr2["isPair"] = isPair;
                    this.patternTb.Rows.Add(ndr2);
                    chdr.Delete();
                }
            }

            this.calpart();
            #endregion
        }

        public void gridvalid()
        {
            this.gridArticleSize.ValidateControl();
            this.gridQty.ValidateControl();
            this.gridCutpart.ValidateControl();
            this.gridAllPart.ValidateControl();
        }

        public void calpart() // 計算Parts,TotalParts
        {
            if (this.gridArticleSize.GetSelecteds(SelectedSort.Index).Count == 0)
            {
                string cutref = MyUtility.Convert.GetString(((DataRowView)this.gridCutRef.GetSelecteds(SelectedSort.Index)[0]).Row["cutref"]);
                MyUtility.Msg.WarningBox($@"Distribution no data!!
Please check the cut refno#：{cutref} distribution data in workOrder(Cutting P02)");
                return;
            }

            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            int allpart = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", string.Format("iden={0}", selectDr["iden"])));
            DataRow[] allpartdr = this.patternTb.Select(string.Format("PatternCode='ALLPARTS' and iden={0}", selectDr["iden"]));
            if (allpartdr.Length > 0)
            {
                allpartdr[0]["Parts"] = allpart;
            }

            int Totalpart = MyUtility.Convert.GetInt(this.patternTb.Compute("Sum(Parts)", string.Format("iden={0}", selectDr["iden"])));
            this.numTotalPart.Value = Totalpart;
            selectDr["TotalParts"] = Totalpart;
        }

        private void insertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e) // 新增下中
        {
            this.gridvalid();
            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow ndr = this.patternTb.NewRow();
            ndr["iden"] = selectDr["iden"];
            ndr["cutref"] = selectDr["cutref"];
            this.patternTb.Rows.Add(ndr);
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e) // 刪除下中
        {
            this.gridvalid();
            DataRow selectDr = ((DataRowView)this.gridCutpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            if (selectDr["PatternCode"].ToString() == "ALLPARTS")
            {
                return;
            }

            selectDr.Delete();
        }

        private void allpart_insert_Click(object sender, EventArgs e) // 新增右下
        {
            this.gridvalid();
            DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow ndr = this.allpartTb.NewRow();
            ndr["iden"] = selectDr["iden"];
            ndr["cutref"] = selectDr["cutref"];
            this.allpartTb.Rows.Add(ndr);
        }

        private void allpart_delete_Click(object sender, EventArgs e) // 刪除右下
        {
            if (this.gridAllPart.Rows.Count == 0)
            {
                return;
            }

            this.gridvalid();
            DataRow selectDr = ((DataRowView)this.gridAllPart.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();
            this.calpart();
        }

        private void btnGarmentList_Click(object sender, EventArgs e)
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
            var Sizelist = this.ArticleSizeTb_View.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            PublicForm.GarmentList callNextForm = new PublicForm.GarmentList(ukey, selectDr["poid"].ToString(), selectDr["Cutref"].ToString(), Sizelist);
            callNextForm.ShowDialog(this);
        }

        private void btnColorComb_Click(object sender, EventArgs e)
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

        private void btnCopy_to_same_Cutref_Click(object sender, EventArgs e)
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
            DataRow[] ArtDrAy = this.ArticleSizeTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));
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

            foreach (DataRow dr in ArtDrAy) // 抓出iden
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
                    ndr["isPair"] = dr2["isPair"];
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
                    ndr["isPair"] = dr2["isPair"];

                    this.allpartTb.Rows.Add(ndr);
                }
            }

            this.CopyGridCutRef(true, string.Empty);
        }

        private void gridArticleSize_SelectionChanged(object sender, EventArgs e)
        {
            this.changeRow();
        }

        private void btnClose_Click(object sender, EventArgs e)
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
            this.distSizeQty(oldcount, newcount, selectDr);
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

        private void btnCopy_to_other_Cutref_Click(object sender, EventArgs e)
        {
            var frm = new P11_copytocutref();
            frm.ShowDialog(this);
            if (!MyUtility.Check.Empty(frm.copycutref))
            {
                string copycutref = frm.copycutref;
                DataRow selectDr = ((DataRowView)this.gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
                string cutref = selectDr["Cutref"].ToString();

                if (cutref.Equals(frm.copycutref))
                {
                    MyUtility.Msg.WarningBox("<CutRef> can not input selected CutRef itself");
                    return;
                }

                int iden = Convert.ToInt16(selectDr["iden"]);
                DataRow[] ArtDrAy = this.ArticleSizeTb.Select(string.Format("Cutref='{0}'", copycutref));
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

                foreach (DataRow dr in ArtDrAy) // 抓出iden
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
                        ndr["isPair"] = dr2["isPair"];
                        this.patternTb.Rows.Add(ndr);
                        if (!MyUtility.Check.Empty(dr2["Parts"]))
                        {
                            npart = npart + Convert.ToInt16(dr2["Parts"]);
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
                        ndr["isPair"] = dr2["isPair"];

                        this.allpartTb.Rows.Add(ndr);
                    }

                    dr["TotalParts"] = npart;
                }

                this.CopyGridCutRef(false, copycutref);
            }
        }

        private void btnBatchCreate_Click(object sender, EventArgs e)
        {
            if (this.CutRefTb == null)
            {
                return;
            }

            if (this.CutRefTb.Rows.Count == 0)
            {
                return;
            }

            DataRow[] CutrefAy = this.CutRefTb.Select("Sel=1");
            if (CutrefAy.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first !!");
                return;
            }

            if (CutrefAy.AsEnumerable().Where(w => MyUtility.Convert.GetString(w["item"]).Length > 20).Any())
            {
                DataTable wdt = this.CutRefTb.Select("Sel=1 and len(item) > 20").CopyToDataTable();
                MsgGridForm m = new MsgGridForm(wdt, "Item string length can not more 20", "Warning");

                m.Width = 800;

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

            this.gridCutRef.ValidateControl();
            this.gridArticleSize.ValidateControl();
            this.gridQty.ValidateControl();
            this.gridCutpart.ValidateControl();
            this.gridAllPart.ValidateControl();
            #endregion
            #region 檢查 如果IsPair =✔, 加總相同的Cut Part的Parts, 必需>0且可以被2整除
            var idenlist = this.ArticleSizeTb.Select("Sel=1").AsEnumerable().Select(s => MyUtility.Convert.GetString(s["iden"])).ToList();
            var patternSaveList = this.patternTb.AsEnumerable().Where(w => idenlist.Contains(MyUtility.Convert.GetString(w["iden"]))).ToList();

            var SamePairCt = patternSaveList.Where(w => MyUtility.Convert.GetBool(w["isPair"])).GroupBy(g => new { CutPart = g["PatternCode"], iden = g["iden"] })
                .Select(s => new { s.Key.CutPart, s.Key.iden, Parts = s.Sum(i => MyUtility.Convert.GetDecimal(i["Parts"])) }).ToList();
            if (SamePairCt.Where(w => w.Parts % 2 != 0).Any())
            {
                var mp = SamePairCt.Where(w => w.Parts % 2 != 0).ToList();
                string msg = @"The following bundle is pair, but parts is not pair, please check Cut Part parts";
                DataTable dt = this.ToDataTable(mp);
                dt.Columns.Remove("iden");
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }
            #endregion

            #region Insert Table
            DataTable Insert_Bundle = new DataTable();
            Insert_Bundle.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail = new DataTable();
            Insert_Bundle_Detail.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail_Art = new DataTable();
            Insert_Bundle_Detail_Art.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail_AllPart = new DataTable();
            Insert_Bundle_Detail_AllPart.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail_Qty = new DataTable();
            Insert_Bundle_Detail_Qty.Columns.Add("Insert", typeof(string));
            DataTable Insert_BundleNo = new DataTable();
            Insert_BundleNo.Columns.Add("BundleNo", typeof(string));
            #endregion

            #region
            DataTable Bundle_Detail;
            DataTable Bundle_Detail_Art;
            string sqlcmdclone = $@"select top 0 * from Bundle_Detail";
            DualResult result = DBProxy.Current.Select(null, sqlcmdclone, out Bundle_Detail);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            sqlcmdclone = $@"select top 0 * from Bundle_Detail_Art";
            result = DBProxy.Current.Select(null, sqlcmdclone, out Bundle_Detail_Art);
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

            // if (radioWithcuto.Checked) autono = 0; //自動根據之前的往下排
            if (this.radiobegin1.Checked)
            {
                autono = 1; // 從1開始
            }

            #region 計算Bundle數 並填入Ratio,Startno
            DataTable spStartnoTb = new DataTable(); // for Startno,分SP給
            spStartnoTb.Columns.Add("orderid", typeof(string));
            spStartnoTb.Columns.Add("startno", typeof(string));

            var ArtAy = this.ArticleSizeTb.Select("Sel=1", "Orderid").OrderBy(row => row["Fabriccombo"]).ThenBy(row => row["Cutno"]);
            if (ArtAy.Any(a => MyUtility.Convert.GetInt(a["bytone"]) > MyUtility.Convert.GetInt(a["Qty"])))
            {
                MyUtility.Msg.WarningBox("Generate by Tone can not greater than No of Bunde");
                return;
            }

            foreach (DataRow artar in ArtAy)
            {
                int tone = MyUtility.Convert.GetInt(artar["byTone"]);
                #region 填入SizeRatio
                DataRow[] drRatio = this.SizeRatioTb.Select(string.Format("Cutref = '{0}' and SizeCode ='{1}'", artar["Cutref"], artar["SizeCode"]));
                if (drRatio.Length > 0)
                {
                    artar["Ratio"] = drRatio[0]["Qty"];
                }
                #endregion
                qtycount = 0;
                patterncount = 0;
                DataRow[] QtyAry = this.qtyTb.Select(string.Format("iden={0}", artar["iden"]));
                DataRow[] PatternAry = this.patternTb.Select(string.Format("iden={0} and parts<>0", artar["iden"]));
                #region Bundle 數
                if (QtyAry.Length > 0)
                {
                    qtycount = QtyAry.Length;
                }
                else
                {
                    qtycount = 0;
                }

                if (PatternAry.Length > 0)
                {
                    patterncount = PatternAry.Length;
                }
                else
                {
                    patterncount = 0;
                }

                if (tone == 0)
                {
                    bundleofnum = bundleofnum + (qtycount * patterncount); // bundle 數
                }
                else
                {
                    int na = this.patternTb.Select($"iden={artar["iden"]} and parts<>0 and PatternCode <> 'AllParts'").Length;
                    int a = this.patternTb.Select($"iden={artar["iden"]} and parts<>0 and PatternCode = 'AllParts'").Length;

                    bundleofnum = bundleofnum + (a * tone) + (qtycount * na * tone);
                }
                #endregion

                #region Start no
                DataRow[] spdr = spStartnoTb.Select(string.Format("orderid='{0}'", artar["Orderid"]));
                if (spdr.Length == 0)
                {
                    DataRow new_spdr = spStartnoTb.NewRow();
                    new_spdr["Orderid"] = artar["Orderid"];
                    if (autono == 0) // auto
                    {
                        #region startno
                        string max_cmd = string.Format("Select isnull(Max(startno+Qty),1) as Start from Bundle WITH (NOLOCK) Where OrderID = '{0}'", artar["Orderid"]);
                        DataTable max_st;
                        if (DBProxy.Current.Select(null, max_cmd, out max_st))
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
            string IDKeyword = this.keyWord + "BC";
            List<string> id_list = MyUtility.GetValue.GetBatchID(IDKeyword, "Bundle", batchNumber: idofnum, sequenceMode: 2);
            List<string> bundleno_list = MyUtility.GetValue.GetBatchID(string.Empty, "Bundle_Detail", format: 3, checkColumn: "Bundleno", batchNumber: bundleofnum, sequenceMode: 2);
            #region Insert Table
            int idcount = 0;
            int bundlenocount = 0;
            int bundlenoCount_Record = 0;
            foreach (DataRow artar in ArtAy)
            {
                int startno = 1;
                int startno_bytone = 1;
                DataRow[] startnoAry = spStartnoTb.Select(string.Format("orderid='{0}'", artar["OrderID"]));
                if (autono == 0)
                {
                    startno = Convert.ToInt16(startnoAry[0]["Startno"]);
                    startno_bytone = startno;
                }

                #region Create Bundle

                DataRow nBundle_dr = Insert_Bundle.NewRow();
                nBundle_dr["Insert"] = string.Format(
                @"
Insert Into Bundle
(ID               , POID        , mDivisionid, SizeCode , Colorid
 , Article        , PatternPanel, Cutno      , cDate    , OrderID
 , SewingLineid   , Item        , SewingCell , Ratio    , Startno
 , Qty            , AllPart     , CutRef     , AddName  , AddDate
 , FabricPanelCode, IsEXCESS) 
values
('{0}'            , '{1}'       , '{2}'      , '{3}'    , '{4}'
 , '{5}'          , '{6}'       , {7}        , GetDate(), '{8}'
 , '{9}'          , '{10}'      , '{11}'     , '{12}'   , '{13}'
 , {14}           , {15}        , '{16}'     , '{17}'   , GetDate()
 , '{18}'         , {19})",
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
                artar["isEXCESS"].EqualString("Y") ? 1 : 0);

                Insert_Bundle.Rows.Add(nBundle_dr);
                #endregion

                qtycount = 0;
                patterncount = 0;

                DataRow[] QtyAry = this.qtyTb.Select(string.Format("iden={0}", artar["iden"]));
                DataTable PatternAry = this.patternTb.Select(string.Format("iden={0} and parts<>0", artar["iden"])).CopyToDataTable();  // 1404: CUTTING_P11_Batch Create Bundle Card，[Batch create]會出現錯誤訊息。
                DataRow[] AllPartArt = this.allpartTb.Select(string.Format("iden={0}", artar["iden"]));

                if (PatternAry.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Bundle Card info cannot be empty.");
                    return;
                }

                DataTable tmpBundle_Detail = Bundle_Detail.Clone();
                DataTable tmpBundle_Detail_Art = Bundle_Detail_Art.Clone();
                tmpBundle_Detail.Columns.Add("Ukey1", typeof(int));
                tmpBundle_Detail.Columns.Add("tmpNum", typeof(int));
                tmpBundle_Detail_Art.Columns.Add("Ukey1", typeof(int));

                bool notYetInsertAllPart = true;
                foreach (DataRow rowqty in QtyAry)
                {
                    #region Bundle_Detail_Qty
                    DataRow nBundle_DetailQty_dr = Insert_Bundle_Detail_Qty.NewRow();
                    nBundle_DetailQty_dr["Insert"] = string.Format(
                        @"Insert into Bundle_Detail_qty(
                        ID,SizeCode,Qty) Values
                        ('{0}','{1}',{2})",
                        id_list[idcount], artar["SizeCode"], rowqty["Qty"]);
                    Insert_Bundle_Detail_Qty.Rows.Add(nBundle_DetailQty_dr);
                    #endregion

                    foreach (DataRow rowPat in PatternAry.Rows)
                    {
                        // 不為 Allparts 的依照 Pattern_GL 紀錄 Location，否則帶入空白
                        string location = rowPat["PatternCode"].ToString() == "ALLPARTS" ? string.Empty : rowPat["Location"].ToString();

                        if (MyUtility.Check.Empty(rowPat["PatternCode"]))
                        {
                            MyUtility.Msg.WarningBox("CutPart cannot be empty.");
                            return;
                        }

                        #region Bundle_Detail 改成先準備Datatable
                        DataRow BundleDetail_pre = tmpBundle_Detail.NewRow();
                        BundleDetail_pre["ID"] = id_list[idcount];
                        BundleDetail_pre["Bundleno"] = string.Empty; // bundleno_list[bundlenocount];
                        BundleDetail_pre["BundleGroup"] = startno;
                        BundleDetail_pre["PatternCode"] = rowPat["PatternCode"];
                        BundleDetail_pre["PatternDesc"] = rowPat["PatternDesc"].ToString().Replace("'", "''");
                        BundleDetail_pre["SizeCode"] = artar["SizeCode"];
                        BundleDetail_pre["Qty"] = rowqty["Qty"];
                        BundleDetail_pre["Parts"] = rowPat["Parts"];
                        BundleDetail_pre["Farmin"] = 0;
                        BundleDetail_pre["Farmout"] = 0;
                        BundleDetail_pre["isPair"] = rowPat["isPair"];
                        BundleDetail_pre["Location"] = location;
                        BundleDetail_pre["Ukey1"] = bundlenocount;
                        tmpBundle_Detail.Rows.Add(BundleDetail_pre);
                        #endregion

                        if (!MyUtility.Check.Empty(rowPat["art"])) // 非空白的Art 才存在
                        {
                            #region Bundle_Detail_art
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
                            #endregion
                        }
                        #region Bundle allPart
                        if (rowPat["PatternCode"].ToString() == "ALLPARTS" && notYetInsertAllPart)
                        {
                            foreach (DataRow rowall in AllPartArt)
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

                                DataRow nBundleDetailAllPart_dr = Insert_Bundle_Detail_AllPart.NewRow();
                                nBundleDetailAllPart_dr["Insert"] = string.Format(
                                    @"Insert Into Bundle_Detail_allpart(ID,PatternCode,PatternDesc,Parts,isPair,Location) Values('{0}','{1}','{2}','{3}','{4}','{5}')",
                                    id_list[idcount], rowall["PatternCode"], rowall["PatternDesc"], rowall["Parts"], rowall["isPair"], rowall["Location"]);
                                Insert_Bundle_Detail_AllPart.Rows.Add(nBundleDetailAllPart_dr);
                            }

                            notYetInsertAllPart = false;
                        }
                        #endregion
                        bundlenocount++; // 每一筆Bundleno 都不同
                    }

                    startno++;
                    if (autono == 0)
                    {
                        startnoAry[0]["Startno"] = Convert.ToInt16(startnoAry[0]["Startno"]) + 1; // 續編Startno才需要
                    }
                }

                idcount++;

                #region by tone 重新處理 Bundle_Detail, Bundle_Detail_art.  1.< Bundlegroup此圈重鞭馬, by sp重紀錄最大值 >,  2.< Bundle數量會改變, 影響全部 >
                int new_startno = 0; // 紀錄重新處理後bundlegroup最大值
                decimal tone = MyUtility.Convert.GetDecimal(artar["byTone"]);
                if (tone > 0) // byTone, 即 Bundlegroup 分幾個
                {
                    DataTable dtDetail = tmpBundle_Detail.Clone();
                    DataTable dtAllPart = tmpBundle_Detail.Clone();
                    DataTable dtAllPart2 = tmpBundle_Detail.Clone();
                    DataTable dtArt = tmpBundle_Detail_Art.Copy();
                    int na = tmpBundle_Detail.Select("PatternCode <> 'AllParts'").Length;
                    int a = tmpBundle_Detail.Select("PatternCode = 'AllParts'").Length;
                    if (na > 0)
                    {
                        dtDetail = tmpBundle_Detail.Select("PatternCode <> 'AllParts'").CopyToDataTable();
                    }

                    if (a > 0)
                    {
                        dtAllPart = tmpBundle_Detail.Select("PatternCode = 'AllParts'").CopyToDataTable();
                    }

                    tmpBundle_Detail.Clear();
                    tmpBundle_Detail_Art.Clear();

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
                                new_startno = startno_bytone + i;

                                item["tmpNum"] = tmpNum; // 暫時紀錄原本資料對應拆出去的資料,要用來重分配Qty
                                tmpNum++;

                                DataTable dtCopyArt = dtArt.Copy();
                                DataRow[] artdr = dtCopyArt.Select($"Ukey1 = '{item["Ukey1"]}'");
                                foreach (DataRow aarr in artdr)
                                {
                                    aarr["Ukey1"] = ukeytone;
                                    tmpBundle_Detail_Art.ImportRow(aarr);
                                }

                                item["Ukey1"] = ukeytone;
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
                        DataRow row = dtAllPart.Rows[0];
                        for (int i = 0; i < tone; i++)
                        {
                            row["BundleGroup"] = startno_bytone + i;
                            new_startno = startno_bytone + i;
                            int notAllpart = PatternAry.Rows.Count - 1;
                            notAllpart = notAllpart == 0 ? 1 : notAllpart;
                            row["Qty"] = ttlAllPartQty / tone;

                            dtAllPart2.ImportRow(row);
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
                    DataRow[] B_art = tmpBundle_Detail_Art.Select($"Ukey1 = '{item["Ukey1"]}'");
                    foreach (DataRow item2 in B_art)
                    {
                        item2["Bundleno"] = bundleno_list[bundlenoCount_Record];
                    }

                    bundlenoCount_Record++;
                }

                #endregion

                // 將這一輪bundle的Bundle_Detail, Bundle_Detail_Art 準備成sql字串
                foreach (DataRow item in tmpBundle_Detail.Rows)
                {
                    DataRow nBundleDetail_dr = Insert_Bundle_Detail.NewRow();
                    nBundleDetail_dr["Insert"] = string.Format(
                        @"Insert into Bundle_Detail
                            (ID,Bundleno,BundleGroup,PatternCode,
                            PatternDesc,SizeCode,Qty,Parts,Farmin,Farmout,isPair ,Location) Values
                            ('{0}','{1}',{2},'{3}',
                            '{4}','{5}',{6},{7},0,0,'{8}','{9}')",
                        item["ID"], item["Bundleno"], item["BundleGroup"], item["PatternCode"],
                        item["PatternDesc"], item["SizeCode"], item["Qty"], item["Parts"], MyUtility.Convert.GetBool(item["isPair"]) ? 1 : 0, item["Location"]);
                    Insert_Bundle_Detail.Rows.Add(nBundleDetail_dr);

                    DataRow drBundleNo = Insert_BundleNo.NewRow();
                    drBundleNo["BundleNo"] = item["Bundleno"];
                    Insert_BundleNo.Rows.Add(drBundleNo);
                }

                foreach (DataRow item in tmpBundle_Detail_Art.Rows)
                {
                    DataRow nBundleDetailArt_dr = Insert_Bundle_Detail_Art.NewRow();
                    nBundleDetailArt_dr["Insert"] = string.Format(
                        @"Insert into Bundle_Detail_art
                        (ID,Bundleno,Subprocessid,PatternCode,PostSewingSubProcess,NoBundleCardAfterSubprocess) Values
                        ('{0}','{1}','{2}','{3}',{4},{5})",
                        item["ID"], item["Bundleno"], item["Subprocessid"], item["PatternCode"],
                        MyUtility.Convert.GetBool(item["PostSewingSubProcess"]) ? 1 : 0,
                        MyUtility.Convert.GetBool(item["NoBundleCardAfterSubprocess"]) ? 1 : 0);
                    Insert_Bundle_Detail_Art.Rows.Add(nBundleDetailArt_dr);
                }
            }
            #endregion
            DualResult upResult;
            using (TransactionScope _transactionscope = new TransactionScope())
            {
                foreach (DataRow dr in Insert_Bundle.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in Insert_Bundle_Detail.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in Insert_Bundle_Detail_Art.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in Insert_Bundle_Detail_AllPart.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                foreach (DataRow dr in Insert_Bundle_Detail_Qty.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }

                _transactionscope.Complete();
            }

            #region sent data to GZ WebAPI
            Func<List<BundleToAGV_PostBody>> funListBundle = () =>
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
                DataTable dtBundleGZ;
                result = MyUtility.Tool.ProcessWithDatatable(Insert_BundleNo, "BundleNo", sqlGetData, out dtBundleGZ);

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
            };
            Task.Run(() => new Guozi_AGV().SentBundleToAGV(funListBundle))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);

            #endregion

            MyUtility.Msg.InfoBox("Successfully");
        }

        private void gridArticleSize_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.gridQty.ValidateControl();
            this.gridCutpart.ValidateControl();
            this.gridAllPart.ValidateControl();
        }

        private void gridCutRef_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.gridArticleSize.ValidateControl();
            this.gridQty.ValidateControl();
            this.gridCutpart.ValidateControl();
            this.gridAllPart.ValidateControl();
        }

        private void changeLabelTotalCutOutputValue()
        {
            this.labelToalCutOutputValue.Text = this.ArticleSizeTb.Compute("sum(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter).ToString();
        }

        private void changeLabelBalanceValue()
        {
            this.labelBalanceValue.Text = this.ArticleSizeTb.Compute("sum(CutOutput)-sum(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter).ToString();
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

                DataRow[] ArticleAry = this.ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", dr["Ukey"], dr["Fabriccombo"]));
                foreach (DataRow row in ArticleAry)
                {
                    row["item"] = dr["item"];
                }
            }

            this.gridArticleSize.Refresh();
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
                bool IsBoundedProcess = MyUtility.Check.Seek(sqlcmd);

                // 是否有主裁片存在
                bool hasMain = drs.AsEnumerable().
                    Where(w => MyUtility.Convert.GetString(w["annotation"]).Split('+').Contains(item) && MyUtility.Convert.GetBool(w["Main"])).Any();

                if (IsBoundedProcess && hasMain)
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
            if (!compareArr(ann, anns)) // 兩個陣列內容要完全一樣，不管順序
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

        public static bool compareArr(string[] arr1, string[] arr2)
        {
            var q = from a in arr1 join b in arr2 on a equals b select a;
            bool flag = arr1.Length == arr2.Length && q.Count() == arr1.Length;

            return flag; // 內容相同返回true,反之返回false。
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
    }
}
