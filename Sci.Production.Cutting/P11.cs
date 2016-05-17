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
        DataTable CutRefTb, ArticleSizeTb, ExcessTb, GarmentTb, allpartTb, patternTb, artTb, qtyTb,SizeRatioTb;
        string f_code;
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string cmd_st = "Select 0 as sel,PatternCode,PatternDesc, '' as annotation,parts,'' as cutref,'' as poid, 0 as iden from Bundle_detail_allpart where 1=0";
            DBProxy.Current.Select(null, cmd_st, out allpartTb);

            string pattern_cmd = "Select patternCode,PatternDesc,Parts,'' as art,0 AS parts, '' as cutref,'' as poid, 0 as iden from Bundle_Detail Where 1=0"; //左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out patternTb);


            string cmd_art = "Select PatternCode,subprocessid from Bundle_detail_art where 1=0";
            DBProxy.Current.Select(null, cmd_art, out artTb);

            string cmd_qty = "Select 0 as No,qty,'' as orderid,'' as cutref,'' as article, SizeCode, 0 as iden from Bundle_Detail_Qty where 1=0";
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
                 DataRow dr = ArticleSize_grid.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    sele = new SelectItem("Select id from Sewingline", "10", dr["SewingLine"].ToString());
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            Linecell.CellValidating += (s, e) =>
            {
                DataRow dr = ArticleSize_grid.GetDataRow(e.RowIndex);
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
                DataRow dr = ArticleSize_grid.GetDataRow(e.RowIndex);
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;
                    sele = new SelectItem("Select SewingCell from Sewingline group by SewingCell", "10", dr["SewingCell"].ToString());
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            Cellcell.CellValidating += (s, e) =>
            {
                DataRow dr = ArticleSize_grid.GetDataRow(e.RowIndex);
                string cell = e.FormattedValue.ToString();
                string oldvalue = dr["SewingCell"].ToString();
                if (oldvalue == cell) return;
                if (!MyUtility.Check.Seek(string.Format("Select * from SewingLine where sewingCell='{0}'",cell)))
                {
                    dr["SewingCell"] = "";
                    dr.EndEdit();
                }
            };
            Qtycell.CellValidating += (s, e) =>
            {
                DataRow dr = ArticleSize_grid.GetDataRow(e.RowIndex);
                int rowcount = qtyTb.Select(string.Format("iden='{0}'", dr["iden"]), "").Length;
                int newcount = Convert.ToInt16(e.FormattedValue);
                numericBox_noofbundle.Value = newcount;
                distSizeQty(rowcount, newcount, dr);

            };
            QtySizecell.CellValidating += (s, e) =>
            {
                DataRow dr = Qty_grid.GetDataRow(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr.EndEdit();
                int qty = MyUtility.Convert.GetInt(qtyTb.Compute("Sum(Qty)", string.Format("iden ='{0}'", dr["iden"])));
                label_TotalQty.Text = qty.ToString();

            };

            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = Cutpart_grid.GetDataRow(e.RowIndex);
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
                DataRow dr = Cutpart_grid.GetDataRow(e.RowIndex);
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
                DataRow dr = AllPart_grid.GetDataRow(e.RowIndex);
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
                DataRow dr = Cutpart_grid.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem2 sele;
                    sele = new SelectItem2("Select id from subprocess where junk=0 and IsProcess=1", "Subprocess", "23", dr["PatternCode"].ToString());
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
                DataRow dr = Cutpart_grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calpart();

            };
            partQtyCell2.CellValidating += (s, e) =>
            {
                DataRow dr = AllPart_grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calpart();
            };

            chcutref.CellValidating += (s, e) =>
            {
                DataRow dr = CutRef_grid.GetDataRow(e.RowIndex);
                int oldvalue = Convert.ToInt16(dr["sel"]);
                int newvalue = Convert.ToInt16(e.FormattedValue);
                DataRow[] ArticleAry = ArticleSizeTb.Select(string.Format("Cutref='{0}'", dr["Cutref"]));

                foreach (DataRow row in ArticleAry)
                {
                    row["Sel"] = newvalue;
                }
                dr["sel"] = newvalue;
                dr.EndEdit();
                ArticleSize_grid.Refresh();
            };
            charticle.CellValidating += (s, e) =>
            {
                DataRow dr = ArticleSize_grid.GetDataRow(e.RowIndex);
                int newvalue = Convert.ToInt16(e.FormattedValue);
                dr["sel"] = newvalue;
                dr.EndEdit();

                DataRow selectDr = ((DataRowView)CutRef_grid.GetSelecteds(SelectedSort.Index)[0]).Row;

                DataRow[] ArtAry = ArticleSizeTb.Select(string.Format("Sel=1 and Cutref='{0}'", dr["Cutref"]));
                if (ArtAry.Length == 0)
                {
                    selectDr["Sel"] = 0;
                }
                else
                {
                    selectDr["Sel"] = 1;
                }
                CutRef_grid.Refresh();
            };
            CutRef_grid.CellClick += (s, e) =>
            {
                if (e.RowIndex != -1) return; //判斷是Header
                if (e.ColumnIndex != 0) return;//判斷是Sel 欄位
                int sel = Convert.ToInt16(CutRefTb.Rows[0]["Sel"]);
                foreach (DataRow dr in ArticleSizeTb.Rows)
                {
                    dr["Sel"] = sel;
                }
                ArticleSize_grid.Refresh();
                
            };
            ArticleSize_grid.CellClick += (s, e) =>
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
                CutRef_grid.Refresh();
            };
            #endregion
            
            #region 左上一Grid
            this.CutRef_grid.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(CutRef_grid)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings: chcutref)
           .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("POID", header: "POID", width: Widths.AnsiChars(11), iseditingreadonly: true)
           .Date("estCutdate", header: "Est.CutDate", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Text("PatternPanel", header: "PatternPanel", width: Widths.AnsiChars(2), iseditingreadonly: true)
           .Text("Cutno", header: "Cut#", width: Widths.AnsiChars(3), iseditingreadonly: true)
           .Text("Item", header: "Item", width: Widths.AnsiChars(10), iseditingreadonly: true);
            CutRef_grid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            CutRef_grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);

            #endregion
            #region 右上一Grid
            this.ArticleSize_grid.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(ArticleSize_grid)
           .CheckBox("Sel", header: "", width: Widths.AnsiChars(2), iseditable: true, trueValue: 1, falseValue: 0, settings:charticle)
           .Text("OrderID", header: "Sub-SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
           .Text("Article", header: "Article", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("Colorid", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
           .Text("SizeCode", header: "Size", width: Widths.AnsiChars(6), iseditingreadonly: true)
           .Text("SewingLine", header: "Sewing Line", width: Widths.AnsiChars(2),settings: Linecell)
           .Text("SewingCell", header: "Sewing Cell", width: Widths.AnsiChars(2),settings: Cellcell)
           .Numeric("Qty", header: "No of Bundle", width: Widths.AnsiChars(3), integer_places: 3,settings: Qtycell)
           .Numeric("Cutoutput", header: "CutOutPut", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
           .Numeric("TotalParts", header: "Total Parts", width: Widths.AnsiChars(4), integer_places: 3, iseditingreadonly: true);
            ArticleSize_grid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            ArticleSize_grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            ArticleSize_grid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            ArticleSize_grid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            ArticleSize_grid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
            ArticleSize_grid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            #region 左下一 Qty
            this.Qty_grid.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(Qty_grid)
           .Numeric("No", header: "No", width: Widths.AnsiChars(3), integer_places: 2, iseditingreadonly: true)
           .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4), integer_places: 3, settings: QtySizecell);
            Qty_grid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            Qty_grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            Qty_grid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

            #region Cutpart-Pattern 下中
            Cutpart_grid.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.Cutpart_grid)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15))
            .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: subcell)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3,settings:partQtyCell);
            Cutpart_grid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            Cutpart_grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            Cutpart_grid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            Cutpart_grid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            Cutpart_grid.Columns[2].DefaultCellStyle.BackColor = Color.SkyBlue;
            Cutpart_grid.Columns[3].DefaultCellStyle.BackColor = Color.Pink;

            #endregion

            #region AllPart_grid 右下一
            this.AllPart_grid.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.AllPart_grid)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13))
            .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partQtyCell2);
            AllPart_grid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            AllPart_grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
            AllPart_grid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            AllPart_grid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            AllPart_grid.Columns[2].DefaultCellStyle.BackColor = Color.Pink;
            AllPart_grid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
        }

        private void Query_button_Click(object sender, EventArgs e)
        {
            MyUtility.Msg.WaitWindows("Query");
            DBProxy.Current.DefaultTimeout = 300;
            //判斷必須有一條件存在
            string cutref = Cutref_textBox.Text;
            string cutdate = CutDate_dateBox.Text;
            string poid = POID_TextBox.Text;
            if (CutRefTb != null)CutRefTb.Clear();
            if(ArticleSizeTb != null)ArticleSizeTb.Clear();
            if (allpartTb != null) allpartTb.Clear();
            if (ExcessTb != null) ExcessTb.Clear();
            if (GarmentTb != null) GarmentTb.Clear();
            if (patternTb != null) patternTb.Clear();
            if (qtyTb != null) qtyTb.Clear();
            if (artTb != null) artTb.Clear();

            if (MyUtility.Check.Empty(cutref) && MyUtility.Check.Empty(cutdate) && MyUtility.Check.Empty(poid))
            {
                MyUtility.Msg.WarningBox("The Condition can not empty.");
                return;
            }
            #region 條件式
            string query_cmd = string.Format(
            @"Select distinct 0 as sel,a.cutref,ord.poid,a.estcutdate,b.patternPanel,a.cutno,
                (Select Reason.Name 
                from Reason, Style 
                where Reason.Reasontypeid ='Style_Apparel_Type' and 
                Style.ukey = ord.styleukey and Style.ApparelType = Reason.id ) 
                as item
            from workorder a ,orders ord, workorder_PatternPanel b 
            Where a.ukey = b.workorderukey and a.orderid = ord.id and ord.mDivisionid = '{0}' and a.id = ord.cuttingsp", keyWord);
            string distru_cmd = string.Format(
            @"Select distinct 0 as sel,0 as iden,a.cutref,b.orderid,b.article,a.colorid,b.sizecode,c.PatternPanel, '' as Ratio,a.cutno,
            substring(ord.Sewline,1,charindex(',',ord.Sewline,1)) as Sewingline,
                isnull((Select SewingCell 
                from SewingLine 
                where id=substring(ord.Sewline,1,charindex(',',ord.Sewline,1)) and factoryid=ord.factoryid and junk=0) ,'')
                as  SewingCell,
                (Select Reason.Name 
                from Reason, Style 
                where Reason.Reasontypeid ='Style_Apparel_Type' and 
                Style.ukey = ord.styleukey and Style.ApparelType = Reason.id ) 
                as item,
           1 as Qty,isnull(sum(b.Qty),0) as cutoutput,0 as TotalParts,ord.poid, 0 as startno
            from workorder a ,orders ord, workorder_Distribute b ,workorder_PatternPanel c 
            Where a.ukey = b.workorderukey and ord.mDivisionid = '{0}' and a.ukey = c.workorderukey and b.orderid = ord.id and c.id = a.id and a.id = b.id and a.id = ord.cuttingsp", keyWord);

            string Excess_cmd = string.Format(
            @"Select distinct a.cutref,a.orderid
            from workorder a , workorder_Distribute b ,orders ord
            Where a.ukey = b.workorderukey and ord.mDivisionid = '{0}'   and a.id = b.id and b.orderid = 'EXCESS' and a.id = ord.cuttingsp", keyWord);

            if (!MyUtility.Check.Empty(cutref))
            {
                query_cmd = query_cmd + string.Format(" and a.cutref='{0}'",cutref);
                distru_cmd = distru_cmd + string.Format(" and a.cutref='{0}'", cutref);
                Excess_cmd = Excess_cmd + string.Format(" and a.cutref='{0}'", cutref);
            }
            if (!MyUtility.Check.Empty(CutDate_dateBox.Value))
            {
                query_cmd = query_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                distru_cmd = distru_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
                Excess_cmd = Excess_cmd + string.Format(" and a.estcutdate='{0}'", cutdate);
            }
            if (!MyUtility.Check.Empty(poid))
            {
                query_cmd = query_cmd + string.Format(" and ord.poid='{0}'", poid);
                distru_cmd = distru_cmd + string.Format(" and ord.poid='{0}'", poid);
                Excess_cmd = Excess_cmd + string.Format(" and  ord.poid='{0}'", poid);
            }
            #endregion

            DualResult query_dResult = DBProxy.Current.Select(null, query_cmd, out CutRefTb);
            if (!query_dResult)
            {
                ShowErr(query_cmd, query_dResult);
                return;
            }

            distru_cmd = distru_cmd + " and b.orderid !='EXCESS' group by a.cutref,b.orderid,b.article,a.colorid,b.sizecode,ord.Sewline,ord.factoryid,ord.poid,c.PatternPanel,a.cutno,ord.styleukey";
            query_dResult = DBProxy.Current.Select(null, distru_cmd, out ArticleSizeTb);
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
                MyUtility.Msg.ShowMsgGrid(ExcessTb, "Those detail had <EXCESS> not yet distribute to SP#","Warning");
            }
            #endregion
            #region GarmentList Table
            //將PO# Group by 
           // CutRefTb.DefaultView.
            DataTable dtDistinct = CutRefTb.DefaultView.ToTable(true, new string[] { "POID" });
            foreach(DataRow dr in dtDistinct.Rows)
            {
                DataTable tmpTb;
                PublicPrg.Prgs.GetGarmentListTable(dr["POID"].ToString(), out tmpTb);
                if (GarmentTb == null)
                {
                    GarmentTb = tmpTb;
                }
                else
                {
                    GarmentTb.Merge(tmpTb);
                }
            }
            #endregion 
            #region articleSizeTb 繞PO 找出QtyTb,PatternTb,AllPartTb
            
            int iden = 1;
            MyUtility.Tool.ProcessWithDatatable(ArticleSizeTb, "Cutref,Article,SizeCode", "Select b.Cutref,a.SizeCode,a.Qty From Workorder_SizeRatio a,#tmp b,workorder c where b.cutref = c.cutref and c.ukey = a.workorderukey and b.sizecode = a.sizecode", out SizeRatioTb);
            foreach (DataRow dr in ArticleSizeTb.Rows)
            {
                dr["iden"] = iden;
                createPattern(dr["POID"].ToString(),dr["Article"].ToString(),dr["PatternPanel"].ToString(),dr["Cutref"].ToString(),iden);

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
                int totalpart = MyUtility.Convert.GetInt(patternTb.Compute("sum(Parts)",string.Format("iden ={0}",iden)));
                dr["TotalParts"] = totalpart;
                iden++;
            }
            #endregion 

            CutRef_grid.DataSource = CutRefTb;
            ArticleSize_grid.DataSource = ArticleSizeTb;
            Qty_grid.DataSource = qtyTb;
            AllPart_grid.DataSource = allpartTb;
            Cutpart_grid.DataSource = patternTb;
            MyUtility.Msg.WaitClear();
            
        }
        public void createPattern(string poid,string article,string patternpanel,string cutref,int iden)
        {
            #region 撈取Pattern Ukey  找最晚Edit且Status 為Completed
            string Styleyukey = MyUtility.GetValue.Lookup("Styleukey", poid, "Orders", "ID");
            string patidsql = String.Format(
                            @"SELECT ukey
                              FROM [Production].[dbo].[Pattern]
                              WHERE STYLEUKEY = '{0}'  and Status = 'Completed' 
                              AND EDITdATE = 
                              (
                                SELECT MAX(EditDate) 
                                from pattern 
                                where styleukey = '{0}' and Status = 'Completed'
                              )
             ", Styleyukey);
            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            #endregion
            string sqlcmd = String.Format(
             @"Select a.ArticleGroup
            from Pattern_GL_Article a
            Where a.PatternUkey = '{0}' and article = '{1}'", patternukey, article);
            f_code = MyUtility.GetValue.Lookup(sqlcmd, null);
            if (f_code == "") f_code = "F_Code";

            //找出相同PatternPanel 的subprocessid
            int npart = 0; //allpart 數量
            DataRow[] garmentar = GarmentTb.Select(string.Format("{0} = '{1}'", f_code, patternpanel));
            foreach (DataRow dr in garmentar)
            {
                if (MyUtility.Check.Empty(dr["annotation"])) //若無ANNOTATion直接寫入All Parts
                {
                    DataRow ndr = allpartTb.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["parts"] = Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                    ndr["Cutref"] = cutref;
                    ndr["POID"] = poid;
                    ndr["iden"] = iden;
                    allpartTb.Rows.Add(ndr);
                    npart = npart + Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
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
                        art = PublicPrg.Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(),artTb, out lallpart);
                        #endregion
                        if (!lallpart)
                        {
                            if (dr["DV"].ToString() != "0" || dr["Pair"].ToString() != "0")
                            {
                                int count = Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
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
                                ndr2["art"] = art;
                                ndr2["POID"] = poid;
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
                            ndr["iden"] = iden;
                            ndr["parts"] = Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                            npart = npart + Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
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
                        ndr["iden"] = iden;
                        ndr["parts"] = Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
                        npart = npart + Convert.ToInt16(dr["alone"]) + Convert.ToInt16(dr["DV"]) * 2 + Convert.ToInt16(dr["Pair"]) * 2;
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

        private void CutRef_grid_SelectionChanged(object sender, EventArgs e)
        {
            changeRow();

        }

        public void changeRow()
        {
            DataRow selectDr_Cutref;
            DataRow selectDr_Artsize;
            if (CutRefTb.Rows.Count == 0) return;
            if (CutRef_grid.GetSelectedRowIndex() == -1)
            {
                selectDr_Cutref = CutRefTb.Rows[0];
            }
            else
            {
                selectDr_Cutref = ((DataRowView)CutRef_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            }
            ArticleSizeTb.DefaultView.RowFilter = string.Format("Cutref ='{0}'", selectDr_Cutref["Cutref"]);
            if (ArticleSizeTb.Rows.Count == 0) return;
            if (ArticleSize_grid.GetSelectedRowIndex() == -1)
            {
                selectDr_Artsize = ArticleSizeTb.Rows[0];
            }
            else
            {
                selectDr_Artsize = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            }
           
            
            
            qtyTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            allpartTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            patternTb.DefaultView.RowFilter = string.Format("iden ='{0}'", selectDr_Artsize["iden"]);
            label_TotalCutOutput.Text = selectDr_Artsize["Cutoutput"].ToString();
            numericBox_noofbundle.Value = Convert.ToInt16(selectDr_Artsize["Qty"]);
            totalpart_numericBox.Value = Convert.ToInt16(selectDr_Artsize["TotalParts"]);
            label_TotalQty.Text = qtyTb.Compute("Sum(Qty)",string.Format("iden={0}",selectDr_Artsize["iden"])).ToString();
        }

        public void distSizeQty(int rowcount, int newcount,DataRow dr)//計算Size Qty
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
                        if (modqty > 0) dr2["Qty"] = eachqty + 1;//每組分配一個Qty 當分配完表示沒了
                        else dr2["Qty"] = eachqty;
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

        private void numericBox_noofbundle_Validated(object sender, EventArgs e)
        {

            int oldcount = Convert.ToInt16(numericBox_noofbundle.OldValue);
            int newcount = Convert.ToInt16(numericBox_noofbundle.Value);
            if (ArticleSizeTb == null) return;
            if (ArticleSizeTb.Rows.Count == 0) return;
            DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr["Qty"] = newcount;
            distSizeQty(oldcount, newcount, selectDr);
        }

        private void button_LefttoRight_Click(object sender, EventArgs e)
        {
            gridvalid();
            DataRow selectartDr = ((DataRowView)Cutpart_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
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
                            if (dr["art"].ToString().IndexOf(dr2["subprocessid"].ToString()) == -1) dr2.Delete();
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

        private void button_RighttoLeft_Click(object sender, EventArgs e)
        {
            gridvalid();
            if (patternTb.Rows.Count == 0) return;
            DataRow selectartDr = ((DataRowView)Cutpart_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow selectallparteDr = ((DataRowView)AllPart_grid.GetSelecteds(SelectedSort.Index)[0]).Row;

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
                        art = PublicPrg.Prgs.BundleCardCheckSubprocess(ann, chdr["PatternCode"].ToString(),artTb, out lallpart);
                        #endregion
                    }
                    //新增PatternTb
                    DataRow ndr2 = patternTb.NewRow();
                    ndr2["PatternCode"] = chdr["PatternCode"];
                    ndr2["PatternDesc"] = chdr["PatternDesc"];
                    ndr2["iden"] = chdr["iden"];
                    ndr2["Parts"] = chdr["Parts"]; 
                    ndr2["art"] = art;
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
            ArticleSize_grid.ValidateControl();
            Qty_grid.ValidateControl();
            Cutpart_grid.ValidateControl();
            AllPart_grid.ValidateControl();
        }

        public void calpart() //計算Parts,TotalParts
        {
            DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            int allpart = MyUtility.Convert.GetInt(allpartTb.Compute("Sum(Parts)", string.Format("iden={0}", selectDr["iden"])));
            DataRow[] allpartdr = patternTb.Select(string.Format("PatternCode='ALLPARTS' and iden={0}",selectDr["iden"]));
            if (allpartdr.Length > 0)
            {
                allpartdr[0]["Parts"] = allpart;
            }
            int Totalpart =MyUtility.Convert.GetInt(patternTb.Compute("Sum(Parts)", string.Format("iden={0}", selectDr["iden"])));
            totalpart_numericBox.Value = Totalpart;
            selectDr["TotalParts"] = Totalpart;
        }

        private void insertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)//新增下中
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow ndr = patternTb.NewRow();
            ndr["iden"] = selectDr["iden"];
            patternTb.Rows.Add(ndr);
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)//刪除下中
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)Cutpart_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            if (selectDr["PatternCode"].ToString() == "ALLPARTS") return;
            selectDr.Delete();
        }

        private void allpart_insert_Click(object sender, EventArgs e) //新增右下
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow ndr = allpartTb.NewRow();
            ndr["iden"] = selectDr["iden"];
            allpartTb.Rows.Add(ndr);
        }

        private void allpart_delete_Click(object sender, EventArgs e)//刪除右下
        {
            gridvalid();
            DataRow selectDr = ((DataRowView)AllPart_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            string ukey = MyUtility.GetValue.Lookup("Styleukey", selectDr["poid"].ToString(), "Orders", "ID");
            Sci.Production.PublicForm.GarmentList callNextForm =
    new Sci.Production.PublicForm.GarmentList(ukey);
            callNextForm.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            Sci.Production.PublicForm.ColorCombination callNextForm =
new Sci.Production.PublicForm.ColorCombination(selectDr["poid"].ToString());
            callNextForm.ShowDialog(this);
        }

        private void copy_to_same_Cutref_Click(object sender, EventArgs e)
        {
            DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            string cutref = selectDr["Cutref"].ToString();
            int iden = Convert.ToInt16(selectDr["iden"]);
            DataRow[] ArtDrAy = ArticleSizeTb.Select(string.Format("Cutref='{0}' and iden<>{1}",cutref,iden));
            DataRow[] oldPatternDr = patternTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));
            DataRow[] oldAllPartDr = allpartTb.Select(string.Format("Cutref='{0}' and iden<>{1}", cutref, iden));
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
            }
        }

        private void ArticleSize_grid_SelectionChanged(object sender, EventArgs e)
        {
            changeRow();
        }

        private void Close_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copy_to_other_Cutref_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Cutting.P11_copytocutref();
            frm.ShowDialog(this);
            if (!MyUtility.Check.Empty(frm.copycutref))
            {
                string copycutref = frm.copycutref;
                DataRow selectDr = ((DataRowView)ArticleSize_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
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

        private void BatchCreate_Button_Click(object sender, EventArgs e)
        {
            DataRow[] CutrefAy = CutRefTb.Select("Sel=1");
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
            int autono = 0,qtycount = 0, patterncount = 0;
            if (withcuto.Value == "1") autono = 0; //自動根據之前的往下排
            if (begin1.Value == "1") autono = 1;//從1開始

            #region 計算Bundle數 並填入Ratio,Startno
            DataTable spStartnoTb = new DataTable(); //for Startno,分SP給
            spStartnoTb.Columns.Add("orderid", typeof(string));
            spStartnoTb.Columns.Add("startno", typeof(string));

            DataRow[] ArtAy = ArticleSizeTb.Select("Sel=1 ", "Orderid");
            foreach (DataRow artar in ArtAy)
            {
                #region 填入SizeRatio
                DataRow[] drRatio = SizeRatioTb.Select(string.Format("Cutref = '{0}' and SizeCode ='{1}'",artar["Cutref"],artar["SizeCode"]));
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
                if (QtyAry.Length>0) qtycount = QtyAry.Length;
                else qtycount = 0;
                if (PatternAry.Length > 0) patterncount = PatternAry.Length;
                else patterncount = 0;
                bundleofnum = bundleofnum + (qtycount * patterncount); //bundle 數
                #endregion

                #region Start no
                DataRow[] spdr = spStartnoTb.Select(string.Format("orderid='{0}'", artar["Orderid"]));
                if (spdr.Length ==0)
                {
                    DataRow new_spdr = spStartnoTb.NewRow();
                    new_spdr["Orderid"] = artar["Orderid"];
                    if (autono == 0)//auto
                    {
                        #region startno
                        string max_cmd = string.Format("Select isnull(Max(startno+Qty),0) as Start from Bundle Where OrderID = '{0}'", artar["Orderid"]);
                        DataTable max_st;
                        if (DBProxy.Current.Select(null, max_cmd, out max_st))
                        {
                            if (max_st.Rows.Count != 0) new_spdr["startno"] = Convert.ToInt16(max_st.Rows[0]["Start"]);
                            else  new_spdr["startno"] = 1;
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
            List<string> id_list = MyUtility.GetValue.GetBatchID(IDKeyword, "Bundle", batchNumber: idofnum);
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
                @"Insert Into Bundle
                (ID,POID,mDivisionid,SizeCode,Colorid,Article,PatternPanel,Cutno,
                cDate,OrderID,SewingLineid,Item,SewingCell,
                Ratio,Startno,Qty,AllPart,CutRef,AddName,AddDate) values
                ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},
                GetDate(),'{8}','{9}','{10}','{11}','{12}',
                '{13}',{14},{15},'{16}','{17}',GetDate())",
                 id_list[idcount], artar["POID"], keyWord, artar["SizeCode"], artar["colorid"], artar["Article"], artar["PatternPanel"], artar["Cutno"], artar["orderid"], artar["SewingLine"], artar["item"], artar["SewingCell"],
                 artar["Ratio"], startno, artar["Qty"], artar["TotalParts"], artar["Cutref"], loginID);
                Insert_Bundle.Rows.Add(nBundle_dr);
                #endregion

                qtycount = 0;
                patterncount = 0;
                
                DataRow[] QtyAry = qtyTb.Select(string.Format("iden={0}", artar["iden"]));
                DataRow[] PatternAry = patternTb.Select(string.Format("iden={0}", artar["iden"]));
                DataRow[] AllPartArt = allpartTb.Select(string.Format("iden={0}", artar["iden"]));
                foreach (DataRow rowqty in QtyAry)
                {
                    #region Bundle_Detail_Qty
                    DataRow nBundle_DetailQty_dr = Insert_Bundle_Detail_Qty.NewRow();
                    nBundle_DetailQty_dr["Insert"] = string.Format(
                        @"Insert into Bundle_Detail_qty(
                        ID,SizeCode,Qty) Values
                        ('{0}','{1}',{2})",
                         id_list[idcount],artar["SizeCode"],rowqty["Qty"]);
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
                            id_list[idcount], bundleno_list[bundlenocount],startno,rowPat["PatternCode"],
                            rowPat["PatternDesc"], artar["SizeCode"], rowqty["Qty"], rowPat["Parts"]);
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
                            foreach(DataRow rowall in AllPartArt)
                            {
                                DataRow nBundleDetailAllPart_dr = Insert_Bundle_Detail_AllPart.NewRow();
                                nBundleDetailAllPart_dr["Insert"] = string.Format(
                                    @"Insert Into Bundle_Detail_allpart
                                    (ID,Bundleno,PatternCode,PatternDesc,Parts) Values
                                    ('{0}','{1}','{2}','{3}',{4})",
                                     id_list[idcount], bundleno_list[bundlenocount], rowall["PatternCode"], rowall["PatternDesc"], rowall["Parts"]);
                                Insert_Bundle_Detail_AllPart.Rows.Add(nBundleDetailAllPart_dr);
                            }
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
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    foreach (DataRow dr in Insert_Bundle.Rows)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                        {
                            _transactionscope.Dispose();
                            return;
                        }                   
                    }
                    foreach (DataRow dr in Insert_Bundle_Detail.Rows)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                        {
                            _transactionscope.Dispose();
                            return;
                        }
                    }
                    foreach (DataRow dr in Insert_Bundle_Detail_Art.Rows)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                        {
                            _transactionscope.Dispose();
                            return;
                        }
                    }
                    foreach (DataRow dr in Insert_Bundle_Detail_AllPart.Rows)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                        {
                            _transactionscope.Dispose();
                            return;
                        }
                    }
                    foreach (DataRow dr in Insert_Bundle_Detail_Qty.Rows)
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, dr["Insert"].ToString())))
                        {
                            _transactionscope.Dispose();
                            return;
                        }
                    }
                    _transactionscope.Complete();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

        }

    }
}
