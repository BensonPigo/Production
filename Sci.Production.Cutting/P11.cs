using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using Ict.Data;
using Ict;
using Sci.Win.Tools;
using System.Linq;
using Sci.Production.PublicPrg;
using System.Transactions;

namespace Sci.Production.Cutting
{
    public partial class P11 : Sci.Win.Tems.QueryForm
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        DataTable CutRefTb, ArticleSizeTb, ExcessTb, GarmentTb, GarmentTb_CutRefEmpty, allpartTb, patternTb, artTb, qtyTb, SizeRatioTb, headertb;
        string f_code;
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string cmd_st = "Select 0 as sel,PatternCode,PatternDesc, '' as annotation,parts,'' as cutref,'' as poid, 0 as iden from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out allpartTb);

            string pattern_cmd = "Select patternCode,PatternDesc,Parts,'' as art,0 AS parts, '' as cutref,'' as poid, 0 as iden from Bundle_Detail WITH (NOLOCK) Where 1=0"; //左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out patternTb);

            string cmd_art = "Select PatternCode,subprocessid from Bundle_detail_art WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_art, out artTb);

            string cmd_qty = "Select 0 as No,qty,'' as orderid,'' as cutref,'' as article, SizeCode, 0 as iden from Bundle_Detail_Qty WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_qty, out qtyTb);
            InitializeComponent();
            gridSetup();
        }
        public void gridSetup()
        {
            #region 右鍵事件
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
            DataGridViewGeneratorCheckBoxColumnSettings chcutref = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings charticle = new DataGridViewGeneratorCheckBoxColumnSettings();

            Linecell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = gridArticleSize.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    string sql = string.Format(@"Select DISTINCT ID  From SewingLine WITH (NOLOCK) 
                        where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}')", Sci.Env.User.Keyword);
                    sele = new SelectItem(sql, "10", dr["SewingLine"].ToString());
                    sele.Width = 300;
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            Linecell.CellValidating += (s, e) =>
            {
                DataRow dr = gridArticleSize.GetDataRow(e.RowIndex);
                string line = e.FormattedValue.ToString();
                string oldvalue = dr["Sewingline"].ToString();
                if (oldvalue == line) return;
                if (!MyUtility.Check.Seek(line, "SewingLine", "ID"))
                {
                    dr["Sewingline"] = "";
                    dr.EndEdit();
                }
            };
            Cellcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = gridArticleSize.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    sele = new SelectItem("Select SewingCell from Sewingline WITH (NOLOCK) where SewingCell!='' group by SewingCell", "10", dr["SewingCell"].ToString());
                    sele.Width = 300;
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            Cellcell.CellValidating += (s, e) =>
            {
                DataRow dr = gridArticleSize.GetDataRow(e.RowIndex);
                string cell = e.FormattedValue.ToString();
                string oldvalue = dr["SewingCell"].ToString();
                if (oldvalue == cell) return;
                if (!MyUtility.Check.Seek(string.Format("Select * from SewingLine WITH (NOLOCK) where sewingCell='{0}'", cell)))
                {
                    dr["SewingCell"] = "";
                    dr.EndEdit();
                }
            };
            Qtycell.CellValidating += (s, e) =>
            {
                DataRow dr = gridArticleSize.GetDataRow(e.RowIndex);
                int old = MyUtility.Convert.GetInt(dr["Qty"]);
                int newvalue = MyUtility.Convert.GetInt(e.FormattedValue);
                if (old == newvalue) return;
                int rowcount = qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), "").Length;
                int newcount = Convert.ToInt16(e.FormattedValue);
                numNoOfBundle.Value = newcount;
                distSizeQty(rowcount, newcount, dr);
            };

            cutOutputCell.CellValidating += (s, e) =>
            {
                DataRow dr = gridArticleSize.GetDataRow(e.RowIndex);
                int oldCutOutput = Convert.ToInt32(dr["cutOutput"]);
                int newCutOutput = Convert.ToInt32(e.FormattedValue);
                dr["cutOutput"] = newCutOutput;
                dr.EndEdit();

                int newBalance = Convert.ToInt32(ArticleSizeTb.Compute("sum(CutOutput)-sum(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter));

                if (newBalance > 0)
                {
                    MyUtility.Msg.InfoBox("Balance can not more than zero.");
                    dr["cutOutput"] = oldCutOutput;
                    dr.EndEdit();
                    return;
                }

                if (oldCutOutput.EqualString(newCutOutput) == false)
                {
                    changeLabelBalanceValue();

                    int rowcount = qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), "").Length;
                    int newcount = MyUtility.Convert.GetInt(dr["Qty"]);
                    distSizeQty(rowcount, newcount, dr);
                }
            };

            QtySizecell.CellValidating += (s, e) =>
            {
                DataRow dr = gridQty.GetDataRow(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr.EndEdit();
                int qty = MyUtility.Convert.GetInt(qtyTb.Compute("Sum(Qty)", string.Format("iden ='{0}'", dr["iden"])));
                label_TotalQty.Text = qty.ToString();

            };

            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = gridCutpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;

                    sele = new SelectItem(GarmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    dr["PatternDesc"] = (sele.GetSelecteds()[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (sele.GetSelecteds()[0]["PatternCode"]).ToString();
                    string[] ann = (sele.GetSelecteds()[0]["Annotation"]).ToString().Split('+'); //剖析Annotation
                    string art = "";
                    bool lallpart;
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {

                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), artTb, out lallpart);

                    }
                    #endregion
                    dr["art"] = art;
                    dr["parts"] = 1;
                    dr.EndEdit();
                }
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = gridCutpart.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                string oldvalue = dr["PatternCode"].ToString();
                if (oldvalue == patcode) return;
                DataRow[] gemdr = GarmentTb.Select(string.Format("PatternCode ='{0}'", patcode), "");
                if (gemdr.Length > 0)
                {
                    dr["PatternDesc"] = (gemdr[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (gemdr[0]["PatternCode"]).ToString();
                    string[] ann = (gemdr[0]["Annotation"]).ToString().Split('+'); //剖析Annotation
                    string art = "";
                    bool lallpart;
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), artTb, out lallpart);

                    }
                    #endregion
                    dr["art"] = art;
                    dr["parts"] = 1;

                }
                dr.EndEdit();
                calpart();
            };
            patterncell2.EditingMouseDown += (s, e) =>
            {
                DataRow dr = gridAllPart.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {

                    SelectItem sele;

                    sele = new SelectItem(GarmentTb, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    dr["PatternDesc"] = (sele.GetSelecteds()[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (sele.GetSelecteds()[0]["PatternCode"]).ToString();
                    dr["Annotation"] = (sele.GetSelecteds()[0]["Annotation"]).ToString();
                    dr["parts"] = 1;
                    dr.EndEdit();
                    calpart();
                }
            };
            subcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = gridCutpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem2 sele;
                    sele = new SelectItem2("Select id from subprocess WITH (NOLOCK) where junk=0 and IsSelection=1", "Subprocess", "23", dr["PatternCode"].ToString(), null, null, null);
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    string subpro = sele.GetSelectedString().Replace(",", "+");

                    e.EditingControl.Text = subpro;
                    dr["art"] = subpro;
                    dr.EndEdit();
                }
            };
            partQtyCell.CellValidating += (s, e) =>
            {
                DataRow dr = gridCutpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calpart();

            };
            partQtyCell2.CellValidating += (s, e) =>
            {
                DataRow dr = gridAllPart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calpart();
            };
            chcutref.CellValidating += (s, e) =>
            {
                DataRow dr = gridCutRef.GetDataRow(e.RowIndex);
                if ((bool)e.FormattedValue == (dr["sel"].ToString() == "1" ? true : false)) return;
                int oldvalue = Convert.ToInt16(dr["sel"]);
                int newvalue = Convert.ToInt16(e.FormattedValue);
                DataRow[] ArticleAry = ArticleSizeTb.Select(string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", dr["Ukey"], dr["Fabriccombo"]));

                foreach (DataRow row in ArticleAry)
                {
                    row["Sel"] = newvalue;
                }
                dr["sel"] = newvalue;
                dr.EndEdit();
                gridArticleSize.Refresh();
            };
            charticle.CellValidating += (s, e) =>
            {
                DataRow dr = gridArticleSize.GetDataRow(e.RowIndex);
                int newvalue = Convert.ToInt16(e.FormattedValue);
                dr["sel"] = newvalue;
                dr.EndEdit();

                DataRow selectDr = ((DataRowView)gridCutRef.GetSelecteds(SelectedSort.Index)[0]).Row;

                DataRow[] ArtAry = ArticleSizeTb.Select(string.Format("Sel=1 and Cutref='{0}'", dr["Cutref"]));
                if (ArtAry.Length == 0)
                {
                    selectDr["Sel"] = 0;
                }
                else
                {
                    selectDr["Sel"] = 1;
                }
                gridCutRef.Refresh();
            };
            gridCutRef.CellClick += (s, e) =>
            {
                if (e.RowIndex != -1) return; //判斷是Header
                if (e.ColumnIndex != 0) return;//判斷是Sel 欄位
                DataRow dr = gridCutRef.GetDataRow(0);
                int oldvalue = Convert.ToInt16(dr["sel"]);
                int newvalue = Convert.ToInt16(((DataTable)gridCutRef.DataSource).DefaultView.ToTable().Rows[0]["sel"]);
                foreach (DataRow row in ArticleSizeTb.Rows)
                {
                    row["Sel"] = newvalue;
                }
                dr["sel"] = newvalue;
                dr.EndEdit();
                gridArticleSize.Refresh();

            };
            gridArticleSize.CellClick += (s, e) =>
            {
                if (e.RowIndex != -1) return; //判斷是Header
                if (e.ColumnIndex != 0) return;//判斷是Sel 欄位
                string cutref = ArticleSizeTb.DefaultView.ToTable().Rows[0]["Cutref"].ToString();
                int sel = Convert.ToInt16(ArticleSizeTb.DefaultView.ToTable().Rows[0]["Sel"]);
                DataRow[] dr = CutRefTb.Select(string.Format("Cutref='{0}'", cutref));
                if (dr.Length > 0)
                {
                    dr[0]["Sel"] = sel;
                }
                gridCutRef.Refresh();
            };
            #endregion

            #region 左上一Grid
            this.gridCutRef.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(gridCutRef)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: chcutref)
           .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("POID", header: "POID", width: Widths.AnsiChars(11), iseditingreadonly: true)
           .Date("estCutdate", header: "Est.CutDate", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("Fabriccombo", header: "Fabric" + Environment.NewLine + "Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
           .Text("FabricPanelCode", header: "Pattern" + Environment.NewLine + "Panel", width: Widths.AnsiChars(2), iseditingreadonly: true)
           .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3), iseditingreadonly: true)
           .Text("Item", header: "Item", width: Widths.AnsiChars(10), iseditingreadonly: true);
            gridCutRef.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridCutRef.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);

            #endregion
            #region 右上一Grid
            this.gridArticleSize.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(gridArticleSize)
           .CheckBox("Sel", header: "", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: charticle)
           .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("SewingLine", header: "Line#", width: Widths.AnsiChars(2), settings: Linecell)
           .Text("SewingCell", header: "Sew" + Environment.NewLine + "Cell", width: Widths.AnsiChars(2), settings: Cellcell)
           .Numeric("Qty", header: "No of" + Environment.NewLine + "Bundle", width: Widths.AnsiChars(3), integer_places: 3, settings: Qtycell)
           .Numeric("Cutoutput", header: "Cut" + Environment.NewLine + "OutPut", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: false, settings: cutOutputCell)
           .Numeric("TotalParts", header: "Total" + Environment.NewLine + "Parts", width: Widths.AnsiChars(4), integer_places: 3, iseditingreadonly: true);
            gridArticleSize.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridArticleSize.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridArticleSize.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            gridArticleSize.Columns["SewingLine"].DefaultCellStyle.BackColor = Color.Pink;
            gridArticleSize.Columns["SewingCell"].DefaultCellStyle.BackColor = Color.Pink;
            gridArticleSize.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            gridArticleSize.Columns["Cutoutput"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            #region 左下一Qty
            this.gridQty.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(gridQty)
           .Numeric("No", header: "No", width: Widths.AnsiChars(3), integer_places: 2, iseditingreadonly: true)
           .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4), integer_places: 3, settings: QtySizecell);
            gridQty.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridQty.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridQty.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            #region 下中一Cutpart-Pattern 
            gridCutpart.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridCutpart)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15))
            .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: subcell)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell);
            gridCutpart.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridCutpart.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridCutpart.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            gridCutpart.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            gridCutpart.Columns["art"].DefaultCellStyle.BackColor = Color.SkyBlue;
            gridCutpart.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;

            #endregion
            #region 右下一AllPart_grid 
            this.gridAllPart.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.gridAllPart)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13))
            .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell2);
            gridAllPart.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridAllPart.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            gridAllPart.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            gridAllPart.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            gridAllPart.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            gridAllPart.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;

            for (int i = 0; i < this.gridAllPart.ColumnCount; i++)
            {
                this.gridAllPart.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 300;
            string cutref = txtCutref.Text;
            string cutdate = dateEstCutDate.Value == null ? "" : dateEstCutDate.Value.Value.ToShortDateString();
            string factory = txtfactoryByM.Text;
            string poid = txtPOID.Text;
            #region Clear Table
            allpartTb.Clear();
            patternTb.Clear();
            artTb.Clear();
            qtyTb.Clear();
            GarmentTb = null;
            GarmentTb_CutRefEmpty = null;
            CutRefTb = null;
            ArticleSizeTb = null;
            ExcessTb = null;
            SizeRatioTb = null;
            headertb = null;
            #endregion
            //判斷必須有一條件存在
            if (MyUtility.Check.Empty(cutref) && MyUtility.Check.Empty(cutdate) && MyUtility.Check.Empty(poid))
            {
                MyUtility.Msg.WarningBox("The Condition can not empty.");
                return;
            }
            this.ShowWaitMessage("Query");
            #region 條件式
            string query_cmd = string.Format(@"
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
        , a.Ukey
from    workorder a WITH (NOLOCK) 
        ,orders ord WITH (NOLOCK) 
        , workorder_PatternPanel b WITH (NOLOCK)  
Where   a.ukey = b.workorderukey 
        and a.orderid = ord.id 
        and ord.mDivisionid = '{0}' 
        and a.id = ord.cuttingsp 
        and a.CutRef is not null ", keyWord);

            string distru_cmd = string.Format(@"
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
	    , ag.ArticleGroup
from workorder a WITH (NOLOCK) 
inner join orders ord WITH (NOLOCK) on a.id = ord.cuttingsp
inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey and a.id = b.id and b.orderid = ord.id
outer apply (
	select  a.ArticleGroup
	from pattern p WITH (NOLOCK)
	inner join Pattern_GL_Article a WITH (NOLOCK) on  a.PatternUkey = p.ukey
	where   p.STYLEUKEY = ord.Styleukey
	        and a.article = b.article
	        and Status = 'Completed' 
	        AND p.EDITdATE = (  SELECT MAX(EditDate) 
                                from pattern WITH (NOLOCK)
                                where   styleukey = ord.Styleukey 
                                        and Status = 'Completed')	
)ag
Where   a.CutRef is not null  
        and ord.mDivisionid = '{0}'", keyWord);

            string Excess_cmd = string.Format(@"
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
        and a.CutRef is not null ", keyWord);

            StringBuilder SizeRatio = new StringBuilder();
            SizeRatio.Append(string.Format(@"
;with tmp as(
	Select  distinct a.cutref
            , b.sizecode
            , a.Ukey
	from workorder a WITH (NOLOCK) 
	inner join orders ord WITH (NOLOCK) on a.id = ord.cuttingsp
	inner join workorder_Distribute b WITH (NOLOCK) on a.ukey = b.workorderukey and a.id = b.id and b.orderid = ord.id
	inner join workorder_PatternPanel c WITH (NOLOCK) on a.ukey = c.workorderukey and c.id = a.id
	Where   a.CutRef is not null 
            and ord.mDivisionid = '{0}'
", keyWord));
            #endregion
            #region where 條件
            if (!MyUtility.Check.Empty(cutref))
            {
                query_cmd = query_cmd + string.Format(" and a.cutref='{0}'", cutref);
                distru_cmd = distru_cmd + string.Format(" and a.cutref='{0}'", cutref);
                Excess_cmd = Excess_cmd + string.Format(" and a.cutref='{0}'", cutref);
                SizeRatio.Append(string.Format(" and a.cutref='{0}'", cutref));
            }
            if (!MyUtility.Check.Empty(dateEstCutDate.Value))
            {
                query_cmd = query_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                distru_cmd = distru_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                Excess_cmd = Excess_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                SizeRatio.Append(string.Format(" and a.estcutdate='{0}'", cutdate));
            }
            if (!MyUtility.Check.Empty(poid))
            {
                query_cmd = query_cmd + string.Format(" and ord.poid='{0}'", poid);
                distru_cmd = distru_cmd + string.Format(" and ord.poid='{0}'", poid);
                Excess_cmd = Excess_cmd + string.Format(" and  ord.poid='{0}'", poid);
                SizeRatio.Append(string.Format(" and ord.poid='{0}'", poid));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                query_cmd = query_cmd + string.Format(" and ord.FtyGroup='{0}'", factory);
                distru_cmd = distru_cmd + string.Format(" and ord.FtyGroup='{0}'", factory);
                Excess_cmd = Excess_cmd + string.Format(" and  ord.FtyGroup='{0}'", factory);
                SizeRatio.Append(string.Format(" and ord.FtyGroup='{0}'", factory));
            }
            #endregion

            query_cmd = query_cmd + " order by ord.poid,a.estcutdate,a.Fabriccombo,a.cutno";

            DualResult query_dResult = DBProxy.Current.Select(null, query_cmd, out CutRefTb);
            if (!query_dResult)
            {
                ShowErr(query_cmd, query_dResult);
                return;
            }

            if (CutRefTb.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data Found!");
                this.HideWaitMessage();
                return;
            }

            //Mantis_7045 將PatternPanel改成FabricPanelCode,不然會有些值不正確
            distru_cmd = distru_cmd + @" and b.orderid !='EXCESS' and a.CutRef is not null  
group by a.cutref,b.orderid,b.article,a.colorid,b.sizecode,ord.Sewline,ord.factoryid,ord.poid,a.Fabriccombo,a.FabricPanelCode,a.cutno,ord.styleukey,a.CutCellid,a.Ukey,ag.ArticleGroup
order by b.sizecode,b.orderid,a.FabricPanelCode";
            query_dResult = DBProxy.Current.Select(null, distru_cmd, out ArticleSizeTb);
            if (!query_dResult)
            {
                ShowErr(distru_cmd, query_dResult);
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
            query_dResult = DBProxy.Current.Select(null, SizeRatio.ToString(), out SizeRatioTb);
            if (!query_dResult)
            {
                ShowErr(distru_cmd, query_dResult);
                return;
            }

            #region 若有EXCESS 需顯示通知
            query_dResult = DBProxy.Current.Select(null, Excess_cmd, out ExcessTb);
            if (!query_dResult)
            {
                ShowErr(Excess_cmd, query_dResult);
                return;
            }
            if (ExcessTb.Rows.Count > 0)
            {
                var m = MyUtility.Msg.ShowMsgGrid(ExcessTb, "Those detail had <EXCESS> not yet distribute to SP#", "Warning");
                m.Width = 500;
                m.grid1.Columns[1].Width = 140;
                m.text_Find.Width = 140;
                m.btn_Find.Location = new Point(150, 6);
                m.btn_Find.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            }
            #endregion        

            #region articleSizeTb 繞PO 找出QtyTb,PatternTb,AllPartTb
            int iden = 1;

            foreach (DataRow dr in ArticleSizeTb.Rows)
            {
                dr["iden"] = iden;
                #region Create Qtytb
                DataRow qty_newRow = qtyTb.NewRow();
                qty_newRow["No"] = 1;
                qty_newRow["Qty"] = dr["cutoutput"];
                qty_newRow["OrderID"] = dr["OrderID"];
                qty_newRow["Cutref"] = dr["Cutref"];
                qty_newRow["Article"] = dr["Article"];
                qty_newRow["SizeCode"] = dr["SizeCode"];
                qty_newRow["iden"] = iden;
                qtyTb.Rows.Add(qty_newRow);
                #endregion           
                createPattern(dr["POID"].ToString(), dr["Article"].ToString(), dr["FabricPanelCode"].ToString(), dr["Cutref"].ToString(), iden, dr["ArticleGroup"].ToString());
                int totalpart = MyUtility.Convert.GetInt(patternTb.Compute("sum(Parts)", string.Format("iden ={0}", iden)));
                dr["TotalParts"] = totalpart;
                iden++;
            }
            #endregion


            gridCutRef.DataSource = CutRefTb;
            gridArticleSize.DataSource = ArticleSizeTb;
            gridQty.DataSource = qtyTb;
            gridAllPart.DataSource = allpartTb;
            gridCutpart.DataSource = patternTb;

            this.gridCutRef.AutoResizeColumns();
            this.gridArticleSize.AutoResizeColumns();
            this.gridCutpart.AutoResizeColumns();

            this.HideWaitMessage();
        }

        public void createPattern(string poid, string article, string patternpanel, string cutref, int iden, string ArticleGroup)
        {
            if (ArticleGroup == "") f_code = "F_Code";
            else f_code = ArticleGroup;
            //找出相同PatternPanel 的subprocessid
            int npart = 0; //allpart 數量
            string patidsql;
            DataTable garmentListTb;
            #region 輸出GarmentTb
            string Styleyukey = MyUtility.GetValue.Lookup("Styleukey", poid, "Orders", "ID");
            if (MyUtility.Check.Empty(cutref))
            {
                patidsql = String.Format(
                            @"SELECT ukey
                              FROM [Production].[dbo].[Pattern] WITH (NOLOCK) 
                              WHERE STYLEUKEY = '{0}'  and Status = 'Completed' 
                              AND EDITdATE = 
                              (
                                SELECT MAX(EditDate) 
                                from pattern WITH (NOLOCK) 
                                where styleukey = '{0}' and Status = 'Completed'
                              )
             ", Styleyukey);
            }
            else
            {
                patidsql = String.Format(
                            @"select top 1 Ukey 
                            from Pattern WITH (NOLOCK)
                            where PatternNo = (select top 1  substring(MarkerNo,1,9)+'N' from WorkOrder WITH (NOLOCK) where CutRef = '{0}' and ID='{1}')
                            and Status = 'Completed'
                            order by ActFinDate Desc
                            ", cutref, poid);
            }
            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            string headercodesql = string.Format(@"
Select distinct ArticleGroup 
from Pattern_GL_LectraCode WITH (NOLOCK) 
where PatternUkey = '{0}' 
order by ArticleGroup", patternukey);
            
            DualResult headerResult = DBProxy.Current.Select(null, headercodesql, out headertb);
            if (!headerResult)
            {
                return;
            }
            #region 建立Table
            string tablecreatesql = string.Format(@"Select '{0}' as orderid,a.*,'' as F_CODE", poid);
            foreach (DataRow dr in headertb.Rows)
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
            string lecsql = "";
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
                    //dr[artgroup] = lecdr["PatternPanel"].ToString().Trim();
                    //Mantis_7045 比照舊系統對應FabricPanelCode
                    dr[artgroup] = lecdr["FabricPanelCode"].ToString().Trim();
                }
                if (dr["SEQ"].ToString() == "0001") dr["PatternCode"] = dr["PatternCode"].ToString().Substring(10);
            }
            #endregion
            GarmentTb = garmentListTb;
            #endregion

            StringBuilder w = new StringBuilder();
            w.Append(string.Format("orderid = '{0}' and (1=0", poid));
            foreach (DataRow dr in headertb.Rows)
            {
                w.Append(string.Format(" or {0} = '{1}' ", dr[0], patternpanel));
            }
            w.Append(")");
            DataRow[] garmentar = GarmentTb.Select(w.ToString());
            foreach (DataRow dr in garmentar)
            {
                if (MyUtility.Check.Empty(dr["annotation"])) //若無ANNOTATion直接寫入All Parts
                {
                    DataRow ndr = allpartTb.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["parts"] = MyUtility.Convert.GetInt(dr["alone"]) + MyUtility.Convert.GetInt(dr["DV"]) * 2 + MyUtility.Convert.GetInt(dr["Pair"]) * 2;
                    ndr["Cutref"] = cutref;
                    ndr["POID"] = poid;
                    ndr["iden"] = iden;
                    allpartTb.Rows.Add(ndr);
                    npart = npart + MyUtility.Convert.GetInt(dr["alone"]) + MyUtility.Convert.GetInt(dr["DV"]) * 2 + MyUtility.Convert.GetInt(dr["Pair"]) * 2;
                }
                else
                {
                    //Annotation 
                    string[] ann = dr["annotation"].ToString().Split('+'); //剖析Annotation
                    string art = "";
                    #region Annotation有在Subprocess 內需要寫入bundle_detail_art，寫入Bundle_Detail_pattern
                    if (ann.Length > 0)
                    {

                        bool lallpart;
                        #region 算Subprocess
                        //artTb 用不到只是因為共用BundleCardCheckSubprocess PRG其他需要用到
                        art = PublicPrg.Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), artTb, out lallpart);
                        #endregion
                        if (!lallpart)
                        {
                            if (dr["DV"].ToString() != "0" || dr["Pair"].ToString() != "0")
                            {
                                int count = Convert.ToInt32(dr["DV"]) * 2 + Convert.ToInt32(dr["Pair"]) * 2;
                                for (int i = 0; i < count; i++)
                                {
                                    DataRow ndr2 = patternTb.NewRow();
                                    ndr2["PatternCode"] = dr["PatternCode"];
                                    ndr2["PatternDesc"] = dr["PatternDesc"];
                                    ndr2["Parts"] = 1;
                                    ndr2["art"] = art;
                                    ndr2["POID"] = poid;
                                    ndr2["Cutref"] = cutref;
                                    ndr2["iden"] = iden;
                                    patternTb.Rows.Add(ndr2);
                                }
                            }
                            else
                            {
                                DataRow ndr2 = patternTb.NewRow();
                                ndr2["PatternCode"] = dr["PatternCode"];
                                ndr2["PatternDesc"] = dr["PatternDesc"];
                                ndr2["art"] = art;
                                ndr2["Parts"] = dr["alone"];
                                ndr2["POID"] = poid;
                                ndr2["Cutref"] = cutref;
                                ndr2["iden"] = iden;
                                patternTb.Rows.Add(ndr2);
                            }
                        }
                        else
                        {
                            DataRow ndr = allpartTb.NewRow();
                            ndr["PatternCode"] = dr["PatternCode"];
                            ndr["PatternDesc"] = dr["PatternDesc"];
                            ndr["Annotation"] = dr["Annotation"];
                            ndr["POID"] = poid;
                            ndr["Cutref"] = cutref;
                            ndr["iden"] = iden;
                            ndr["parts"] = Convert.ToInt32(dr["alone"]) + Convert.ToInt32(dr["DV"]) * 2 + Convert.ToInt32(dr["Pair"]) * 2;
                            npart = npart + Convert.ToInt32(dr["alone"]) + Convert.ToInt32(dr["DV"]) * 2 + Convert.ToInt32(dr["Pair"]) * 2;
                            allpartTb.Rows.Add(ndr);
                        }

                    }
                    else
                    {
                        DataRow ndr = allpartTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["PatternDesc"] = dr["PatternDesc"];
                        ndr["Annotation"] = dr["Annotation"];
                        ndr["POID"] = poid;
                        ndr["Cutref"] = cutref;
                        ndr["iden"] = iden;
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + Convert.ToInt32(dr["DV"]) * 2 + Convert.ToInt32(dr["Pair"]) * 2;
                        npart = npart + Convert.ToInt32(dr["alone"]) + Convert.ToInt32(dr["DV"]) * 2 + Convert.ToInt32(dr["Pair"]) * 2;
                        allpartTb.Rows.Add(ndr);
                    }
                    #endregion
                }
            }

            DataRow pdr = patternTb.NewRow(); //預設要有ALLPARTS
            pdr["PatternCode"] = "ALLPARTS";
            pdr["PatternDesc"] = "All Parts";
            pdr["parts"] = npart;
            pdr["Cutref"] = cutref;
            pdr["POID"] = poid;
            pdr["iden"] = iden;
            patternTb.Rows.Add(pdr);


            DBProxy.Current.DefaultTimeout = 0;
        }

        private void gridCutRef_SelectionChanged(object sender, EventArgs e)
        {
            changeRow();
        }

        public void changeRow()
        {
            DataRow selectDr_Cutref;
            DataRow selectDr_Artsize;
            if (CutRefTb.Rows.Count == 0)
            {
                return;
            }

            if (gridCutRef.GetSelectedRowIndex() == -1)
            {
                selectDr_Cutref = CutRefTb.Rows[0];
            }
            else
            {
                selectDr_Cutref = ((DataRowView)gridCutRef.GetSelecteds(SelectedSort.Index)[0]).Row;
            }

            ArticleSizeTb.DefaultView.RowFilter
                = string.Format("Ukey ='{0}' and Fabriccombo = '{1}'", selectDr_Cutref["Ukey"], selectDr_Cutref["Fabriccombo"]);
            if (ArticleSizeTb.Rows.Count == 0)
            {
                return;
            }

            if (gridArticleSize.GetSelectedRowIndex() == -1)
            {
                selectDr_Artsize = ArticleSizeTb.Rows[0];
            }
            else
            {
                selectDr_Artsize = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            }

            qtyTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            allpartTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            patternTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            label_TotalCutOutput.Text = selectDr_Artsize["Cutoutput"].ToString();
            numNoOfBundle.Value = Convert.ToInt16(selectDr_Artsize["Qty"]);
            numTotalPart.Value = Convert.ToInt16(selectDr_Artsize["TotalParts"]);
            label_TotalQty.Text = qtyTb.Compute("Sum(Qty)", string.Format("iden={0}", selectDr_Artsize["iden"])).ToString();

            changeLabelTotalCutOutputValue();
            changeLabelBalanceValue();
        }

        public void distSizeQty(int rowcount, int newcount, DataRow dr)//計算Size Qty
        {
            if (rowcount <= newcount) //缺少需先新增
            {
                for (int j = 0; j < newcount - rowcount; j++)
                {
                    DataRow ndr = qtyTb.NewRow();
                    ndr["SizeCode"] = dr["SizeCode"];
                    ndr["iden"] = dr["iden"];
                    qtyTb.Rows.Add(ndr);
                }
            }
            int i = 1;
            DataRow[] qtyArr = qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), ""); //重新撈取
            foreach (DataRow dr2 in qtyArr)
            {
                if (dr2.RowState != DataRowState.Deleted)
                {
                    dr2["No"] = i;
                }
                if (i > newcount) dr2.Delete(); //多餘的筆數要刪掉

                i++;
            }
            Double TotalCutQty = Convert.ToDouble(dr["Cutoutput"]);
            DataRow[] qtyarry = qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), "");
            if (TotalCutQty % newcount == 0)
            {
                int qty = (int)(TotalCutQty / newcount); //每一個數量是多少
                foreach (DataRow dr2 in qtyarry)
                {
                    dr2["Qty"] = qty;
                }
            }
            else
            {
                int eachqty = (int)(Math.Floor(TotalCutQty / newcount));
                int modqty = (int)(TotalCutQty % newcount); //剩餘數

                foreach (DataRow dr2 in qtyarry)
                {
                    if (eachqty != 0)
                    {
                        if (modqty > 0)
                        {
                            dr2["Qty"] = eachqty + 1;//每組分配一個Qty 當分配完表示沒了
                        }
                        else
                        {
                            dr2["Qty"] = eachqty;
                        }

                        modqty--; //剩餘數一定小於rowcount所以會有筆數沒有拿到
                    }
                    else
                    {
                        if (modqty > 0)
                        {
                            dr2["Qty"] = 1;
                            modqty--;
                        }
                        else dr2.Delete();
                    }                    
                }
            }
        }

        private void numNoOfBundle_Validated(object sender, EventArgs e)
        {

            int oldcount = Convert.ToInt16(numNoOfBundle.OldValue);
            int newcount = Convert.ToInt16(numNoOfBundle.Value);
            if (ArticleSizeTb == null) return;
            if (ArticleSizeTb.Rows.Count == 0) return;
            DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr["Qty"] = newcount;
            distSizeQty(oldcount, newcount, selectDr);
        }

        private void btn_LefttoRight_Click(object sender, EventArgs e)
        {
            gridvalid();
            //避免沒資料造成當機
            if (patternTb.Rows.Count < 1)
            {
                return;
            }
            DataRow selectartDr = ((DataRowView)gridCutpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            string pattern = selectartDr["PatternCode"].ToString();
            if (pattern == "ALLPARTS") return;
            string art = selectartDr["art"].ToString();
            //移動此筆
            DataRow ndr = allpartTb.NewRow();
            ndr["PatternCode"] = selectartDr["PatternCode"];
            ndr["PatternDesc"] = selectartDr["PatternDesc"];
            ndr["iden"] = selectartDr["iden"];
            ndr["poid"] = selectartDr["poid"];
            ndr["Cutref"] = selectartDr["cutref"];
            ndr["Parts"] = selectartDr["Parts"];
            //Annotation
            DataRow[] adr = GarmentTb.Select(string.Format("PatternCode='{0}'", selectartDr["patternCode"]));
            if (adr.Length > 0)
            {
                ndr["annotation"] = adr[0]["annotation"];
            }
            allpartTb.Rows.Add(ndr);
            selectartDr.Delete(); //刪除此筆

            DataRow[] patterndr = patternTb.Select(string.Format("PatternCode='{0}'", pattern));
            DataRow[] artdr = artTb.Select(string.Format("PatternCode='{0}'", pattern));
            if (patterndr.Length > 0) //刪除後還有相同Pattern 需要判斷是否Subprocess都存在
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
                                { dr2.Delete(); }
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
            { //直接刪除全部同PatternCode 的Subprocessid
                foreach (DataRow dr in patterndr)
                {
                    dr.Delete();
                }
            }
            calpart();
        }

        private void btn_RighttoLeft_Click(object sender, EventArgs e)
        {
            gridvalid();
            if (patternTb.Rows.Count == 0 || gridAllPart.Rows.Count == 0) return;
            DataRow selectartDr = ((DataRowView)gridCutpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow selectallparteDr = ((DataRowView)gridAllPart.GetSelecteds(SelectedSort.Index)[0]).Row;

            DataRow[] checkdr = allpartTb.Select("sel=1");
            #region 確認有勾選
            if (checkdr.Length > 0)
            {
                foreach (DataRow chdr in checkdr)
                {
                    string art = "";
                    string[] ann = chdr["annotation"].ToString().Split('+'); //剖析Annotation
                    if (ann.Length > 0)
                    {
                        bool lallpart;
                        #region 算Subprocess
                        art = PublicPrg.Prgs.BundleCardCheckSubprocess(ann, chdr["PatternCode"].ToString(), artTb, out lallpart);
                        #endregion
                    }
                    //新增PatternTb
                    DataRow ndr2 = patternTb.NewRow();
                    ndr2["PatternCode"] = chdr["PatternCode"];
                    ndr2["PatternDesc"] = chdr["PatternDesc"];
                    ndr2["iden"] = chdr["iden"];
                    ndr2["Parts"] = chdr["Parts"];
                    //ndr2["art"] = art;
                    ndr2["art"] = "EMB";
                    ndr2["poid"] = chdr["poid"];
                    ndr2["Cutref"] = chdr["cutref"];
                    patternTb.Rows.Add(ndr2);
                    chdr.Delete();
                }
            }
            calpart();
            #endregion
        }

        public void gridvalid()
        {
            gridArticleSize.ValidateControl();
            gridQty.ValidateControl();
            gridCutpart.ValidateControl();
            gridAllPart.ValidateControl();
        }

        public void calpart() //計算Parts,TotalParts
        {
            DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            int allpart = MyUtility.Convert.GetInt(allpartTb.Compute("Sum(Parts)", string.Format("iden={0}", selectDr["iden"])));
            DataRow[] allpartdr = patternTb.Select(string.Format("PatternCode='ALLPARTS' and iden={0}", selectDr["iden"]));
            if (allpartdr.Length > 0)
            {
                allpartdr[0]["Parts"] = allpart;
            }
            int Totalpart = MyUtility.Convert.GetInt(patternTb.Compute("Sum(Parts)", string.Format("iden={0}", selectDr["iden"])));
            numTotalPart.Value = Totalpart;
            selectDr["TotalParts"] = Totalpart;
        }

        private void insertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)//新增下中
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow ndr = patternTb.NewRow();
            ndr["iden"] = selectDr["iden"];
            patternTb.Rows.Add(ndr);
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)//刪除下中
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)gridCutpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            if (selectDr["PatternCode"].ToString() == "ALLPARTS") return;
            selectDr.Delete();
        }

        private void allpart_insert_Click(object sender, EventArgs e) //新增右下
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow ndr = allpartTb.NewRow();
            ndr["iden"] = selectDr["iden"];
            allpartTb.Rows.Add(ndr);
        }

        private void allpart_delete_Click(object sender, EventArgs e)//刪除右下
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)gridAllPart.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();
        }

        private void btnGarmentList_Click(object sender, EventArgs e)
        {
            if (CutRefTb == null) return;
            if (CutRefTb.Rows.Count == 0) return;
            DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", selectDr["poid"].ToString(), "Orders", "ID");
            Sci.Production.PublicForm.GarmentList callNextForm = new Sci.Production.PublicForm.GarmentList(ukey, selectDr["poid"].ToString(), selectDr["Cutref"].ToString());
            callNextForm.ShowDialog(this);
        }

        private void btnColorComb_Click(object sender, EventArgs e)
        {
            if (CutRefTb == null) return;
            if (CutRefTb.Rows.Count == 0) return;
            DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", selectDr["poid"].ToString(), "Orders", "ID");
            Sci.Production.PublicForm.ColorCombination callNextForm =
            new Sci.Production.PublicForm.ColorCombination(selectDr["poid"].ToString(), ukey);
            callNextForm.ShowDialog(this);
        }

        private void btnCopy_to_same_Cutref_Click(object sender, EventArgs e)
        {
            if (CutRefTb == null) return;
            if (CutRefTb.Rows.Count == 0) return;
            DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
            string cutref = selectDr["Cutref"].ToString();
            int iden = Convert.ToInt16(selectDr["iden"]);
            DataRow[] ArtDrAy = ArticleSizeTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));
            DataRow[] oldPatternDr = patternTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));
            DataRow[] oldAllPartDr = allpartTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));

            DataRow[] oldPatternDr_selected = patternTb.Select(string.Format("Cutref='{0}' and iden={1}", cutref, iden));
            DataRow[] oldAllPartDr_selected = allpartTb.Select(string.Format("Cutref='{0}' and iden={1}", cutref, iden));
            foreach (DataRow dr in oldPatternDr)
            {
                dr.Delete();
            }
            foreach (DataRow dr in oldAllPartDr)
            {
                dr.Delete();
            }
            foreach (DataRow dr in ArtDrAy) //抓出iden
            {
                //新增Pattern
                foreach (DataRow dr2 in oldPatternDr_selected)
                {
                    DataRow ndr = patternTb.NewRow();
                    ndr["iden"] = dr["iden"];
                    ndr["cutref"] = dr["cutref"];
                    ndr["poid"] = dr["poid"];
                    ndr["PatternCode"] = dr2["PatternCode"];
                    ndr["PatternDesc"] = dr2["PatternDesc"];
                    ndr["art"] = dr2["art"];
                    ndr["Parts"] = dr2["Parts"];
                    patternTb.Rows.Add(ndr);
                }
                foreach (DataRow dr2 in oldAllPartDr_selected)
                {
                    DataRow ndr = allpartTb.NewRow();
                    ndr["iden"] = dr["iden"];
                    ndr["cutref"] = dr["cutref"];
                    ndr["poid"] = dr["poid"];
                    ndr["PatternCode"] = dr2["PatternCode"];
                    ndr["PatternDesc"] = dr2["PatternDesc"];
                    ndr["annotation"] = dr2["annotation"];
                    ndr["Parts"] = dr2["Parts"];

                    allpartTb.Rows.Add(ndr);
                }
            }
        }

        private void gridArticleSize_SelectionChanged(object sender, EventArgs e)
        {
            changeRow();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_to_other_Cutref_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Cutting.P11_copytocutref();
            frm.ShowDialog(this);
            if (!MyUtility.Check.Empty(frm.copycutref))
            {
                string copycutref = frm.copycutref;
                DataRow selectDr = ((DataRowView)gridArticleSize.GetSelecteds(SelectedSort.Index)[0]).Row;
                string cutref = selectDr["Cutref"].ToString();
                int iden = Convert.ToInt16(selectDr["iden"]);
                DataRow[] ArtDrAy = ArticleSizeTb.Select(string.Format("Cutref='{0}'", copycutref));
                DataRow[] oldPatternDr = patternTb.Select(string.Format("Cutref='{0}'", copycutref));
                DataRow[] oldAllPartDr = allpartTb.Select(string.Format("Cutref='{0}' ", copycutref));
                DataTable patternDv = patternTb.DefaultView.ToTable();
                DataTable allpartDv = allpartTb.DefaultView.ToTable();
                foreach (DataRow dr in oldPatternDr)
                {
                    dr.Delete();
                }
                foreach (DataRow dr in oldAllPartDr)
                {
                    dr.Delete();
                }
                foreach (DataRow dr in ArtDrAy) //抓出iden
                {
                    //新增Pattern
                    int npart = 0;
                    foreach (DataRow dr2 in patternDv.Rows)
                    {
                        DataRow ndr = patternTb.NewRow();
                        ndr["iden"] = dr["iden"];
                        ndr["cutref"] = dr["cutref"];
                        ndr["poid"] = dr["poid"];
                        ndr["PatternCode"] = dr2["PatternCode"];
                        ndr["PatternDesc"] = dr2["PatternDesc"];
                        ndr["art"] = dr2["art"];
                        ndr["Parts"] = dr2["Parts"];
                        patternTb.Rows.Add(ndr);
                        npart = npart + Convert.ToInt16(dr2["Parts"]);
                    }
                    foreach (DataRow dr2 in allpartDv.Rows)
                    {
                        DataRow ndr = allpartTb.NewRow();
                        ndr["iden"] = dr["iden"];
                        ndr["cutref"] = dr["cutref"];
                        ndr["poid"] = dr["poid"];
                        ndr["PatternCode"] = dr2["PatternCode"];
                        ndr["PatternDesc"] = dr2["PatternDesc"];
                        ndr["annotation"] = dr2["annotation"];
                        ndr["Parts"] = dr2["Parts"];

                        allpartTb.Rows.Add(ndr);
                    }

                    dr["TotalParts"] = npart;
                }
            }
        }

        private void btnBatchCreate_Click(object sender, EventArgs e)
        {
            if (CutRefTb == null) return;
            if (CutRefTb.Rows.Count == 0) return;
            DataRow[] CutrefAy = CutRefTb.Select("Sel=1");
            if (CutrefAy.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first !!");
                return;
            }
            #region 判斷Pattern(Cutpart_grid)的Artwork  不可為空
            DataRow[] findr = patternTb.Select("PatternCode<>'ALLPARTS' and (art='' or art is null)", "");
            if (findr.Length > 0)
            {
                MyUtility.Msg.WarningBox("<Art> can not be empty!");
                return;
            }
            #endregion
            DataTable Insert_Bundle = new DataTable();
            //Insert_Bundle,Insert_Bundle_Detail,Insert_Bundle_Detail_Art,Insert_Bundle_Detail_AllPart,Insert_Bundle_Detail_Qty
            #region Insert Table
            Insert_Bundle.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail = new DataTable();
            Insert_Bundle_Detail.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail_Art = new DataTable();
            Insert_Bundle_Detail_Art.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail_AllPart = new DataTable();
            Insert_Bundle_Detail_AllPart.Columns.Add("Insert", typeof(string));
            DataTable Insert_Bundle_Detail_Qty = new DataTable();
            Insert_Bundle_Detail_Qty.Columns.Add("Insert", typeof(string));
            #endregion
            //一共產生幾張單
            int idofnum = ArticleSizeTb.Select("Sel=1").Length; //會產生表頭數
            int bundleofnum = 0;
            int autono = 0, qtycount = 0, patterncount = 0;
            //if (radioWithcuto.Checked) autono = 0; //自動根據之前的往下排
            if (radiobegin1.Checked) autono = 1;//從1開始

            #region 計算Bundle數 並填入Ratio,Startno
            DataTable spStartnoTb = new DataTable(); //for Startno,分SP給
            spStartnoTb.Columns.Add("orderid", typeof(string));
            spStartnoTb.Columns.Add("startno", typeof(string));

            var ArtAy = ArticleSizeTb.Select("Sel=1", "Orderid").OrderBy(row => row["Fabriccombo"]).ThenBy(row => row["Cutno"]);
            foreach (DataRow artar in ArtAy)
            {
                #region 填入SizeRatio
                DataRow[] drRatio = SizeRatioTb.Select(string.Format("Cutref = '{0}' and SizeCode ='{1}'", artar["Cutref"], artar["SizeCode"]));
                if (drRatio.Length > 0)
                {
                    artar["Ratio"] = drRatio[0]["Qty"];
                }
                #endregion
                qtycount = 0;
                patterncount = 0;
                DataRow[] QtyAry = qtyTb.Select(string.Format("iden={0}", artar["iden"]));
                DataRow[] PatternAry = patternTb.Select(string.Format("iden={0} and parts<>0", artar["iden"]));
                #region Bundle 數
                if (QtyAry.Length > 0) qtycount = QtyAry.Length;
                else qtycount = 0;
                if (PatternAry.Length > 0) patterncount = PatternAry.Length;
                else patterncount = 0;
                bundleofnum = bundleofnum + (qtycount * patterncount); //bundle 數
                #endregion

                #region Start no
                DataRow[] spdr = spStartnoTb.Select(string.Format("orderid='{0}'", artar["Orderid"]));
                if (spdr.Length == 0)
                {
                    DataRow new_spdr = spStartnoTb.NewRow();
                    new_spdr["Orderid"] = artar["Orderid"];
                    if (autono == 0)//auto
                    {
                        #region startno
                        string max_cmd = string.Format("Select isnull(Max(startno+Qty),1) as Start from Bundle WITH (NOLOCK) Where OrderID = '{0}'", artar["Orderid"]);
                        DataTable max_st;
                        if (DBProxy.Current.Select(null, max_cmd, out max_st))
                        {
                            if (max_st.Rows.Count != 0) new_spdr["startno"] = Convert.ToInt16(max_st.Rows[0]["Start"]);
                            else new_spdr["startno"] = 1;
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
            string IDKeyword = keyWord + "BC";
            List<string> id_list = MyUtility.GetValue.GetBatchID(IDKeyword, "Bundle", batchNumber: idofnum, sequenceMode: 2);
            List<string> bundleno_list = MyUtility.GetValue.GetBatchID("", "Bundle_Detail", format: 3, checkColumn: "Bundleno", batchNumber: bundleofnum, sequenceMode: 2);
            #region Insert Table
            int idcount = 0;
            int bundlenocount = 0;
            foreach (DataRow artar in ArtAy)
            {
                #region Create Bundle
                // DataRow[] cutdr = CutRefTb.Select(string.Format("Cutref='{0}'",artar["Cutref"]));
                int startno = 1;
                DataRow[] startnoAry = spStartnoTb.Select(string.Format("orderid='{0}'", artar["OrderID"]));
                if (autono == 0)
                {

                    startno = Convert.ToInt16(startnoAry[0]["Startno"]);
                }

                DataRow nBundle_dr = Insert_Bundle.NewRow();
                nBundle_dr["Insert"] = string.Format(
                @"
Insert Into Bundle
(ID               , POID        , mDivisionid, SizeCode , Colorid
 , Article        , PatternPanel, Cutno      , cDate    , OrderID
 , SewingLineid   , Item        , SewingCell , Ratio    , Startno
 , Qty            , AllPart     , CutRef     , AddName  , AddDate
 , FabricPanelCode) 
values
('{0}'            , '{1}'       , '{2}'      , '{3}'    , '{4}'
 , '{5}'          , '{6}'       , {7}        , GetDate(), '{8}'
 , '{9}'          , '{10}'      , '{11}'     , '{12}'   , '{13}'
 , {14}           , {15}        , '{16}'     , '{17}'   , GetDate()
 , '{18}')",
                id_list[idcount],
                artar["POID"],
                keyWord,
                artar["SizeCode"],
                artar["colorid"],
                artar["Article"],
                artar["Fabriccombo"],
                artar["Cutno"],
                artar["orderid"],
                (artar["SewingLine"].Empty() ? string.Empty : artar["SewingLine"].ToString().Length > 2 ? artar["SewingLine"].ToString().Substring(0, 2) : artar["SewingLine"].ToString()),
                artar["item"],
                artar["SewingCell"],
                artar["Ratio"],
                startno,
                artar["Qty"],
                artar["TotalParts"],
                artar["Cutref"],
                loginID,
                artar["FabricPanelCode"]);

                Insert_Bundle.Rows.Add(nBundle_dr);
                #endregion

                qtycount = 0;
                patterncount = 0;

                DataRow[] QtyAry = qtyTb.Select(string.Format("iden={0}", artar["iden"]));
                DataRow[] PatternAry = patternTb.Select(string.Format("iden={0} and parts<>0", artar["iden"]));  //1404: CUTTING_P11_Batch Create Bundle Card，[Batch create]會出現錯誤訊息。
                DataRow[] AllPartArt = allpartTb.Select(string.Format("iden={0}", artar["iden"]));
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

                    foreach (DataRow rowPat in PatternAry)
                    {
                        #region Bundle_Detail
                        DataRow nBundleDetail_dr = Insert_Bundle_Detail.NewRow();
                        nBundleDetail_dr["Insert"] = string.Format(
                            @"Insert into Bundle_Detail
                            (ID,Bundleno,BundleGroup,PatternCode,
                            PatternDesc,SizeCode,Qty,Parts,Farmin,Farmout) Values
                            ('{0}','{1}',{2},'{3}',
                            '{4}','{5}',{6},{7},0,0)",
                            id_list[idcount], bundleno_list[bundlenocount], startno, rowPat["PatternCode"],
                            rowPat["PatternDesc"].ToString().Replace("'", "''"), artar["SizeCode"], rowqty["Qty"], rowPat["Parts"]);
                        Insert_Bundle_Detail.Rows.Add(nBundleDetail_dr);
                        #endregion
                        if (!MyUtility.Check.Empty(rowPat["art"])) //非空白的Art 才存在
                        {
                            #region Bundle_Detail_art
                            string[] ann = rowPat["art"].ToString().Split('+');
                            for (int i = 0; i < ann.Length; i++)
                            {
                                DataRow nBundleDetailArt_dr = Insert_Bundle_Detail_Art.NewRow();
                                nBundleDetailArt_dr["Insert"] = string.Format(
                                    @"Insert into Bundle_Detail_art
                                (ID,Bundleno,Subprocessid,PatternCode) Values
                                ('{0}','{1}','{2}','{3}')",
                                    id_list[idcount], bundleno_list[bundlenocount], ann[i], rowPat["PatternCode"]);
                                Insert_Bundle_Detail_Art.Rows.Add(nBundleDetailArt_dr);
                            }
                            #endregion
                        }
                        #region Bundle allPart
                        if (rowPat["PatternCode"].ToString() == "ALLPARTS")
                        {

                            #region 讓Bundle_Detail_Allpart只產生一份資料
                            if (allpartTb.Rows.Count > Insert_Bundle_Detail_AllPart.Rows.Count)
                            {
                                foreach (DataRow rowall in AllPartArt)
                                {
                                    DataRow nBundleDetailAllPart_dr = Insert_Bundle_Detail_AllPart.NewRow();
                                    nBundleDetailAllPart_dr["Insert"] = string.Format(@"Insert Into Bundle_Detail_allpart(ID,PatternCode,PatternDesc,Parts) Values('{0}','{1}','{2}','{3}')",
                                         id_list[idcount], rowall["PatternCode"], rowall["PatternDesc"], rowall["Parts"]);
                                    Insert_Bundle_Detail_AllPart.Rows.Add(nBundleDetailAllPart_dr);
                                }
                            }
                            #endregion

                        }
                        #endregion
                        bundlenocount++; //每一筆Bundleno 都不同
                    }
                    startno++;
                    if (autono == 0)
                    {
                        startnoAry[0]["Startno"] = Convert.ToInt16(startnoAry[0]["Startno"]) + 1; //續編Startno才需要
                    }
                }
                idcount++;

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
                        ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }
                foreach (DataRow dr in Insert_Bundle_Detail.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }
                foreach (DataRow dr in Insert_Bundle_Detail_Art.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }
                foreach (DataRow dr in Insert_Bundle_Detail_AllPart.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }
                foreach (DataRow dr in Insert_Bundle_Detail_Qty.Rows)
                {
                    if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(dr["Insert"].ToString(), upResult);
                        return;
                    }
                }
                MyUtility.Msg.InfoBox("Successfully");
                _transactionscope.Complete();
            }
        }

        private void gridArticleSize_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            gridQty.ValidateControl();
            gridCutpart.ValidateControl();
            gridAllPart.ValidateControl();
        }

        private void gridCutRef_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            gridArticleSize.ValidateControl();
            gridQty.ValidateControl();
            gridCutpart.ValidateControl();
            gridAllPart.ValidateControl();
        }

        private void changeLabelTotalCutOutputValue()
        {
            this.labelToalCutOutputValue.Text = ArticleSizeTb.Compute("sum(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter).ToString();
        }

        private void changeLabelBalanceValue()
        {
            this.labelBalanceValue.Text = ArticleSizeTb.Compute("sum(CutOutput)-sum(RealCutOutput)", this.ArticleSizeTb.DefaultView.RowFilter).ToString();
        }
    }
}
