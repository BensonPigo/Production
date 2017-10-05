using System.Linq;
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
using Sci.Production.PublicPrg;


namespace Sci.Production.Cutting
{
    public partial class P10_Generate : Sci.Win.Subs.Base
    {
        DataRow maindatarow;
        DataTable allpartTb, patternTb, artTb, sizeTb, garmentTb;
        DataTable detailTb, alltmpTb, bundle_detail_artTb, qtyTb;
        DataTable detailTb2, alltmpTb2, bundle_detail_artTb2, qtyTb2;
        DataTable f_codeTb;
        DataTable garmentarRC;
        public P10_Generate(DataRow maindr, DataTable table_bundle_Detail, DataTable bundle_Detail_allpart_Tb, DataTable bundle_Detail_Art_Tb, DataTable bundle_Detail_Qty_Tb)
        {
            InitializeComponent();

            #region 準備要處理的table 和原本的table
            detailTb = table_bundle_Detail.Copy();
            alltmpTb = bundle_Detail_allpart_Tb.Copy();
            bundle_detail_artTb = bundle_Detail_Art_Tb.Copy();
            qtyTb = bundle_Detail_Qty_Tb.Copy();
            maindatarow = maindr;

            detailTb2 = table_bundle_Detail;
            alltmpTb2 = bundle_Detail_allpart_Tb;
            bundle_detail_artTb2 = bundle_Detail_Art_Tb;
            qtyTb2 = bundle_Detail_Qty_Tb;
            #endregion

            #region 取tabel的結構
            string cmd_st = "Select 0 as Sel, PatternCode,PatternDesc, '' as annotation,parts from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out allpartTb);
            string pattern_cmd = "Select patternCode,PatternDesc,Parts,'' as art,0 AS parts from Bundle_Detail WITH (NOLOCK) Where 1=0"; //左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out patternTb);
            string cmd_art = "Select PatternCode,subprocessid from Bundle_detail_art WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_art, out artTb);
            #endregion

            #region 準備GarmentList & ArticleGroup
            //GarmentList
            PublicPrg.Prgs.GetGarmentListTable(maindr["cutref"].ToString(), maindatarow["poid"].ToString(), out garmentTb);
            //ArticleGroup
            string patidsql;
            string Styleyukey = MyUtility.GetValue.Lookup("Styleukey", maindatarow["poid"].ToString(), "Orders", "ID");
            if (MyUtility.Check.Empty(maindr["cutref"]))
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
                            ", maindr["cutref"].ToString(), maindatarow["poid"].ToString());
            }
            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            string headercodesql = string.Format(@"
Select distinct ArticleGroup 
from Pattern_GL_LectraCode WITH (NOLOCK) 
where PatternUkey = '{0}'
order by ArticleGroup", patternukey);
            DBProxy.Current.Select(null, headercodesql, out f_codeTb);
            #endregion

            #region Size-CutQty
            int totalCutQty = 0;
            if (MyUtility.Check.Empty(maindr["cutref"])) //無CutRef 就直接抓取Order_Qty 的SizeCode
            {
                labelTotalCutOutput.Visible = false;
                displayTotalCutOutput.Visible = false;
                string size_cmd = string.Format("Select distinct sizecode,0  as Qty from order_Qty WITH (NOLOCK) where id='{0}'", maindr["Orderid"]);
                DualResult dResult = DBProxy.Current.Select(null, size_cmd, out sizeTb);
            }
            else
            {
                string size_cmd = string.Format(@"
Select b.sizecode,isnull(sum(b.Qty),0)  as Qty 
from Workorder a WITH (NOLOCK) 
inner join Workorder_distribute b WITH (NOLOCK) on a.ukey = b.workorderukey
where a.cutref='{0}' and b.orderid='{1}'
group by sizeCode"
                    , maindr["cutref"], maindr["Orderid"]);
                DualResult dResult = DBProxy.Current.Select(null, size_cmd, out sizeTb);
                if (sizeTb.Rows.Count != 0) totalCutQty = Convert.ToInt32(sizeTb.Compute("Sum(Qty)", ""));
                else
                {
                    size_cmd = string.Format("Select distinct sizecode,0  as Qty from order_Qty WITH (NOLOCK) where id='{0}'", maindr["Orderid"]);
                    dResult = DBProxy.Current.Select(null, size_cmd, out sizeTb);
                }
                displayTotalCutOutput.Value = totalCutQty;
            }
            #endregion

            #region 左上qtyTb
            numNoOfBundle.Value = (decimal)maindr["Qty"];
            if (!MyUtility.Check.Empty(maindatarow["cutref"]) && qtyTb.Rows.Count == 0)
            {
                int j = 1;
                foreach (DataRow dr in sizeTb.Rows)
                {
                    if (numNoOfBundle.Value < j) break;
                    DataRow row = qtyTb.NewRow();
                    row["No"] = j;
                    row["SizeCode"] = dr["SizeCode"];
                    row["Qty"] = dr["Qty"];
                    qtyTb.Rows.Add(row);
                    j++;
                }
            }
            else
            {
                int j = 1;
                foreach (DataRow dr in qtyTb.Rows)
                {
                    dr["No"] = j;
                    j++;
                }
            }
            #endregion
            //計算左上TotalQty
            calsumQty();
            //if (detailTb.Rows.Coun!= 0 && maindatarow.RowState!=DataRowState.Added) 
            if (detailTb.Rows.Count != 0 ) exist_Table_Query();
            else noexist_Table_Query();

            grid_setup();
            calAllPart();
            caltotalpart();

            displayPatternPanel.Text = maindr["PatternPanel"].ToString();
        }
        //第一次產生時需全部重新撈值
        public void noexist_Table_Query() 
        {
            //找出相同PatternPanel 的subprocessid
            int npart = 0; //allpart 數量
            StringBuilder w = new StringBuilder();
            w.Append("1 = 0");
            foreach (DataRow dr in f_codeTb.Rows)
            {
                w.Append(string.Format(" or {0} = '{1}' ", dr[0], maindatarow["FabricPanelCode"]));
            }
            DataRow[] garmentar = garmentTb.Select(w.ToString());
            foreach (DataRow dr in garmentar)
            {
                if (MyUtility.Check.Empty(dr["annotation"])) //若無ANNOTATion直接寫入All Parts
                {
                    DataRow ndr = allpartTb.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["parts"] = MyUtility.Convert.GetInt(dr["alone"]) + MyUtility.Convert.GetInt(dr["DV"]) * 2 + MyUtility.Convert.GetInt(dr["Pair"]) * 2;
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
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), artTb, out lallpart);
                        #endregion
                        if (!lallpart)
                        {
                            if (dr["DV"].ToString() != "0" || dr["Pair"].ToString() != "0")
                            {
                                int count = Convert.ToInt32(dr["DV"]) * 2 + Convert.ToInt32(dr["Pair"]) * 2;
                                for (int i = 0; i < count; i++) { 
                                    DataRow ndr2 = patternTb.NewRow();
                                    ndr2["PatternCode"] = dr["PatternCode"];
                                    ndr2["PatternDesc"] = dr["PatternDesc"];
                                    ndr2["Parts"] = 1;
                                    ndr2["art"] = art;
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
                                patternTb.Rows.Add(ndr2);
                            }
                        }
                        else
                        {
                            DataRow ndr = allpartTb.NewRow();
                            ndr["PatternCode"] = dr["PatternCode"];
                            ndr["PatternDesc"] = dr["PatternDesc"];
                            ndr["Annotation"] = dr["Annotation"];
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
            patternTb.Rows.Add(pdr);

            garmentarRC = null;
            garmentarRC = garmentTb.Clone();
            foreach (DataRow gdr in garmentar)
            {
                garmentarRC.ImportRow(gdr);
            }
        }
        //當bundle_allPart, bundle_art 存在時的對應資料
        public void exist_Table_Query()
        {
            //將Bundle_Detial_Art distinct PatternCode,
            DataTable tmp;
            //用來當判斷條件的DataTable,避免DetailTB dataRow被刪除後無法用index撈出資料
            DataTable detailAccept = detailTb.Copy();
            detailAccept.AcceptChanges();
            string BundleGroup = detailAccept.Rows[0]["BundleGroup"].ToString();
            MyUtility.Tool.ProcessWithDatatable(detailTb, "PatternCode,PatternDesc,parts,subProcessid,BundleGroup", string.Format(@"
Select  PatternCode,PatternDesc,Parts,subProcessid,BundleGroup 
from #tmp where BundleGroup='{0}'", BundleGroup) , out tmp);
            //需要使用上一層表身的值,不可重DB撈不然新增的資料就不會存回DB
            MyUtility.Tool.ProcessWithDatatable(detailTb, "PatternCode,SubProcessid", "Select distinct PatternCode,SubProcessid from #tmp WHERE PatternCode<>'ALLPARTS'", out artTb);
            //foreach (DataRow dr in tmp.Select("BundleNO<>''"))
            foreach (DataRow dr in tmp.Rows)
            {
                DataRow ndr = patternTb.NewRow();
                ndr["PatternCode"] = dr["PatternCode"];
                ndr["PatternDesc"] = dr["PatternDesc"];
                ndr["Parts"] = dr["Parts"];
                ndr["art"] = MyUtility.Check.Empty(dr["SubProcessid"]) ? "" : dr["SubProcessid"].ToString().Substring(0, dr["SubProcessid"].ToString().Length - 1);
                string art = "";
                DataRow[] dray = artTb.Select(string.Format("PatternCode = '{0}'", dr["PatternCode"]));
                if (dray.Length != 0)
                {
                    foreach (DataRow dr2 in dray)
                    {
                        if (art != "") art = art + "+" + dr2["Subprocessid"].ToString();
                        else art = dr2["Subprocessid"].ToString();
                    }
                    ndr["art"] = art;
                }
                patternTb.Rows.Add(ndr);
            }

            MyUtility.Tool.ProcessWithDatatable(alltmpTb, "sel,PatternCode,PatternDesc,parts,annotation", "Select distinct sel,PatternCode,PatternDesc,parts,annotation from #tmp", out allpartTb);
            foreach (DataRow dr in allpartTb.Rows)
            {
                DataRow[] adr = garmentTb.Select(string.Format("PatternCode='{0}'", dr["patternCode"]));
                if (adr.Length > 0)
                {
                    dr["annotation"] = adr[0]["annotation"];
                }
            }
            if (allpartTb.Rows.Count == 0)
            {
                StringBuilder w = new StringBuilder();
                w.Append("1 = 0");
                foreach (DataRow dr in f_codeTb.Rows)
                {
                    w.Append(string.Format(" or {0} = '{1}' ", dr[0], maindatarow["PatternPanel"]));
                }
                DataRow[] garmentar = garmentTb.Select(w.ToString());
                foreach (DataRow dr in garmentar)
                {
                    bool f = false;
                    foreach (DataRow drp in patternTb.Rows)
                    {
                        if (dr["patternCode"].ToString() == drp["patternCode"].ToString()) f = true;
                    }
                    if (!f)
                    {
                        DataRow ndr = allpartTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["PatternDesc"] = dr["PatternDesc"];
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + Convert.ToInt32(dr["DV"]) * 2 + Convert.ToInt32(dr["Pair"]) * 2;
                        allpartTb.Rows.Add(ndr);
                    }
                }
            }
            garmentarRC = garmentTb.Copy();
        }

        public void grid_setup()
        {
            DataGridViewGeneratorNumericColumnSettings NoCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings qtyCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings subcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell1 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell2 = new DataGridViewGeneratorNumericColumnSettings();

            #region 左上grid
            NoCell.CellValidating += (s, e) =>
            {
                if (MyUtility.Convert.GetInt(numNoOfBundle.Text) < MyUtility.Convert.GetInt(e.FormattedValue))
                {                                    
                    MyUtility.Msg.WarningBox(string.Format("<No: {0} >  can't greater than <No of Bundle>", e.FormattedValue));                    
                    return;
                }
            };
            qtyCell.CellValidating += (s, e) =>
            {
                DataRow dr = grid_qty.GetDataRow(listControlBindingSource1.Position);
                string oldvalue = dr["qty"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["qty"] = newvalue;
                dr.EndEdit();
                calsumQty();
            };
            #endregion

            #region 左下grid
            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;

                    sele = new SelectItem(garmentarRC, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
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
                    calAllPart();
                    caltotalpart();
                }
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                string oldvalue = dr["PatternCode"].ToString();
                if (oldvalue == patcode) return;

                DataRow[] gemdr = garmentarRC.Select(string.Format("PatternCode ='{0}'", patcode), "");
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
                else
                {
                    MyUtility.Msg.WarningBox(string.Format("<CutPart: {0} >  can't found!", e.FormattedValue));
                    dr["PatternCode"] = "";
                    dr["PatternDesc"] = "";
                    dr["art"] = "";
                    dr["Parts"] = 0;
                }
                dr.EndEdit();
            };
            subcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
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
                    DataRow[] artdr = artTb.Select(string.Format("PatternCode='{0}'", dr["PatternCode"]));
                    foreach (DataRow adr in artdr)
                    {
                        adr.Delete();
                    }
                    foreach (DataRow dt in sele.GetSelecteds())
                    {
                        DataRow ndr = artTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["subprocessid"] = dt["id"];
                        artTb.Rows.Add(ndr);
                    }
                }
            };
            partsCell1.CellValidating += (s, e) =>
            {
                DataRow dr = grid_art.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calAllPart();
                caltotalpart();
            };
            #endregion

            #region 右下grid
            patterncell2.EditingMouseDown += (s, e) =>
            {
                DataRow dr = grid_allpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS") return;
                if (e.Button == MouseButtons.Right)
                {

                    SelectItem sele;

                    sele = new SelectItem(garmentarRC, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    dr["PatternDesc"] = (sele.GetSelecteds()[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (sele.GetSelecteds()[0]["PatternCode"]).ToString();
                    dr["Annotation"] = (sele.GetSelecteds()[0]["Annotation"]).ToString();
                    dr["parts"] = 1;
                    dr.EndEdit();
                    calAllPart();
                    caltotalpart();
                }
            };

            patterncell2.CellValidating += (s, e) =>
            {
                DataRow dr = grid_allpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();

                DataRow[] gemdr = garmentarRC.Select(string.Format("PatternCode ='{0}'", newvalue), "");
                if (gemdr.Length > 0)
                {
                    dr["PatternDesc"] = (gemdr[0]["PatternDesc"]).ToString();
                    dr["PatternCode"] = (gemdr[0]["PatternCode"]).ToString();
                    dr["Annotation"] = (gemdr[0]["Annotation"]).ToString();
                    dr["parts"] = 1;
                    dr.EndEdit();
                    calAllPart();
                    caltotalpart();
                }
                else
                {
                    MyUtility.Msg.WarningBox(string.Format("<CutPart: {0} >  can't found!", e.FormattedValue));
                    dr["Sel"] = 0;
                    dr["PatternCode"] = "";
                    dr["PatternDesc"] = "";
                    dr["Annotation"] = "";
                    dr["Parts"] = 0;
                }
            };
            
            partsCell2.CellValidating += (s, e) =>
            {
                DataRow dr = grid_allpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                calAllPart();
                caltotalpart();                
            };
            #endregion

            //左上
            listControlBindingSource1.DataSource = qtyTb;
            grid_qty.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid_qty)
            .Numeric("No", header: "No", width: Widths.AnsiChars(4), integer_places: 5, settings: NoCell)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(7), integer_places: 5, settings: qtyCell);
            grid_qty.Columns["No"].DefaultCellStyle.BackColor = Color.Pink;
            grid_qty.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

            //左下
            grid_art.DataSource = patternTb;
            grid_art.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid_art)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15))
            .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: subcell)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partsCell1);
            grid_art.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            grid_art.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            grid_art.Columns["art"].DefaultCellStyle.BackColor = Color.Pink;
            grid_art.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;

            //右下
            grid_allpart.DataSource = allpartTb;
            this.grid_allpart.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            Helper.Controls.Grid.Generator(this.grid_allpart)
            .CheckBox("Sel", header: "Chk", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13))
            .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partsCell2);
            grid_allpart.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            grid_allpart.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            grid_allpart.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            grid_allpart.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;

            //右上
            grid_Size.DataSource = sizeTb;
            grid_Size.IsEditingReadOnly = true;
            Helper.Controls.Grid.Generator(this.grid_Size)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8))
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 5);

            //左上因為資料顯示已有排序，所以按Grid Header不可以做排序
            for (int i = 0; i < grid_qty.ColumnCount; i++)
            {
                grid_qty.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        
        private void numNoOfBundle_Validated(object sender, EventArgs e)
        {
            int newvalue = (int)numNoOfBundle.Value;
            int oldvalue = (int)numNoOfBundle.OldValue;
            if (newvalue == oldvalue) return;

            if (qtyTb.Rows.Count == 0)
            {
                for (int i = 0; i < newvalue; i++)
                {
                    DataRow ndr = qtyTb.NewRow();
                    ndr["Qty"] = 0;
                    qtyTb.Rows.Add(ndr);
                }
                qtyTb_serial();
            }
            else
            {
                if (!MyUtility.Check.Empty(maindatarow["cutref"]))
                {
                    int rowindex = grid_qty.CurrentRow.Index;
                    string SizeCode = grid_qty.Rows[rowindex].Cells["SizeCode"].Value.ToString();

                    if (!MyUtility.Check.Empty(newvalue)) qtyTb.Clear();

                    int count = 0;
                    foreach (DataRow dr in sizeTb.Rows)
                    {
                        if (count < newvalue)
                        {
                            DataRow ndr = qtyTb.NewRow();
                            ndr["SizeCode"] = dr["SizeCode"];
                            qtyTb.Rows.Add(ndr);
                            count++;
                        }
                    }
                    //如果No of Bundle數量>右上SizeCode數量,就依照左上滑鼠選擇的SizeCode的值複製多出來的數量
                    if (newvalue > count)
                    {
                        for (int i = 0; i < newvalue - count; i++)
                        {
                            DataRow ndr = qtyTb.NewRow();
                            ndr["SizeCode"] = SizeCode;
                            qtyTb.Rows.Add(ndr);
                        }
                    }

                    qtyTb_serial();
                    calQty();
                }
                else
                {
                    DataTable qtytmp = qtyTb.Copy();
                    qtyTb.Clear();
                    int count = 0;
                    foreach (DataRow dr in qtytmp.Rows)
                    {
                        if (count < newvalue)
                        {
                            DataRow ndr = qtyTb.NewRow();
                            ndr[0] = dr[0];
                            ndr[1] = dr[1];
                            ndr[2] = dr[2];
                            ndr[3] = dr[3];
                            ndr[4] = dr[4];
                            qtyTb.Rows.Add(ndr);
                            count++;
                        }
                    }

                    //增加時
                    if (numNoOfBundle.OldValue != null)
                    {
                        for (int i = 0; i < newvalue - (int)numNoOfBundle.OldValue; i++)
                        {
                            DataRow ndr = qtyTb.NewRow();
                            ndr["Qty"] = 0;
                            qtyTb.Rows.Add(ndr);
                        }
                    }
                    qtyTb_serial();
                }
            }
        }
        //賦予流水號
        private void qtyTb_serial()
        {
            int serial = 1;
            foreach (DataRow dr in qtyTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["No"] = serial;
                    serial++;
                }
            }
        }
        //分配Qty
        public void calQty() 
        {
            foreach (DataRow dr in sizeTb.Rows)
            {
                Double TotalCutQty = Convert.ToDouble(dr["Qty"]);
                DataRow[] qtyarry = qtyTb.Select(string.Format("SizeCode='{0}'", dr["SizeCode"]), "");
                Double rowcount = qtyarry.Length;
                if (TotalCutQty % rowcount == 0)
                {
                    int qty = (int)(TotalCutQty / rowcount); //每一個數量是多少
                    foreach (DataRow dr2 in qtyarry)
                    {
                        dr2["Qty"] = qty;
                    }
                }
                else
                {
                    int eachqty = (int)(Math.Floor(TotalCutQty / rowcount));
                    int modqty = (int)(TotalCutQty % rowcount); //剩餘數

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
            calsumQty();
        }

        public void calsumQty()
        {
            if (qtyTb.Rows.Count > 0) displayTotalQty.Value = Convert.ToInt32(qtyTb.Compute("sum(Qty)", ""));
        }

        private void button_Qty_Click(object sender, EventArgs e)
        {
            if (qtyTb.Rows.Count != 0)
            {
                DataRow selectSizeDr = ((DataRowView)grid_Size.GetSelecteds(SelectedSort.Index)[0]).Row;
                DataRow selectQtyeDr = ((DataRowView)grid_qty.GetSelecteds(SelectedSort.Index)[0]).Row;
                selectQtyeDr["SizeCode"] = selectSizeDr["SizeCode"];
                if (!MyUtility.Check.Empty(maindatarow["cutref"])) calQty();
                else selectQtyeDr["Qty"] = 0;//cutref為空指定行qty為0

                #region 把左上的grid移至下一筆
                int currentRowIndexInt = grid_qty.CurrentRow.Index;
                if (currentRowIndexInt + 1 < grid_qty.RowCount)
                {
                    grid_qty.CurrentCell = grid_qty[0, currentRowIndexInt + 1];
                    grid_qty.FirstDisplayedScrollingRowIndex = currentRowIndexInt + 1;
                }
                #endregion
            }
        }

        private void grid_Size_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            button_Qty_Click(sender, e);
        }

        private void button_LefttoRight_Click(object sender, EventArgs e)
        {
            grid_allpart.ValidateControl();
            grid_art.ValidateControl();
            grid_qty.ValidateControl();
            if (MyUtility.Check.Empty(grid_art.DataSource)|| grid_art.Rows.Count == 0) return;            
            DataRow selectartDr = ((DataRowView)grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            string pattern = selectartDr["PatternCode"].ToString();
            if (pattern == "ALLPARTS") return;
            string art = selectartDr["art"].ToString();
            //移動此筆
            DataRow ndr = allpartTb.NewRow();
            ndr["PatternCode"] = selectartDr["PatternCode"];
            ndr["PatternDesc"] = selectartDr["PatternDesc"];
            ndr["Parts"] = selectartDr["Parts"];
            //Annotation
            DataRow[] adr = garmentTb.Select(string.Format("PatternCode='{0}'", selectartDr["patternCode"]));
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
            calAllPart();
            caltotalpart();
        }

        private void button_RighttoLeft_Click(object sender, EventArgs e)
        {
            grid_allpart.ValidateControl();
            grid_art.ValidateControl();
            grid_qty.ValidateControl();
            if (patternTb.Rows.Count == 0) return;
            if (grid_allpart.RowCount == 0) return;
            DataRow selectartDr = ((DataRowView)grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow selectallparteDr = ((DataRowView)grid_allpart.GetSelecteds(SelectedSort.Index)[0]).Row;

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
                        art = Prgs.BundleCardCheckSubprocess(ann, chdr["PatternCode"].ToString(), artTb, out lallpart);
                        #endregion
                    }
                    //新增PatternTb
                    DataRow ndr2 = patternTb.NewRow();
                    ndr2["PatternCode"] = chdr["PatternCode"];
                    ndr2["PatternDesc"] = chdr["PatternDesc"];
                    ndr2["Parts"] = chdr["Parts"]; ;
                    ndr2["art"] = "EMB";
                    patternTb.Rows.Add(ndr2);
                    chdr.Delete(); //刪除
                }
            }
            else if (checkdr.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data !!");
            }
            #endregion
            calAllPart();
            caltotalpart();
        }

        public void caltotalpart() //計算total part
        {
            if (patternTb.Rows.Count > 0) numTotalParts.Value = Convert.ToDecimal(patternTb.Compute("Sum(Parts)", ""));           
        }

        public void calAllPart() //計算all part
        {
            int allpart = 0;
            if (allpartTb.AsEnumerable().Count(row => row.RowState != DataRowState.Deleted) > 0)
                allpart = allpartTb.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted)
                    .Sum(row => Convert.ToInt32(row["Parts"]));
            DataRow[] dr = patternTb.Select("PatternCode='ALLPARTS'");
            if (dr.Length > 0)
            {
                dr[0]["Parts"] = allpart;
            }
            if (dr.Length == 0 && allpart > 0)
            {
                DataRow drAll = patternTb.NewRow();
                drAll["PatternCode"] = "ALLPARTS";
                drAll["PatternDesc"] = "All Parts";
                drAll["parts"] = allpart;
                patternTb.Rows.Add(drAll);

            }

            //將AllPart Parts=0給刪除
            for (int i = 0; i < patternTb.Rows.Count; i++)
            {
                if (MyUtility.Check.Empty(patternTb.Rows[i]["Parts"]))
                {
                    patternTb.Rows[i].Delete();
                }
            }           
            
        }

        private void insertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = patternTb.NewRow();
            patternTb.Rows.Add();
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow selectartDr = ((DataRowView)grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            if (selectartDr["PatternCode"].ToString() == "ALLPARTS")
            {
                MyUtility.Msg.WarningBox("Please remove all right grid's parts to instead of removeing ALLPARTS directly!");
                return;
            }
            selectartDr.Delete();
            caltotalpart();
        }

        private void allpart_insert_Click(object sender, EventArgs e)
        {
            allpartTb.NewRow();
            allpartTb.Rows.Add();
        }

        private void allpart_delete_Click(object sender, EventArgs e)
        {
            DataRow selectartDr = ((DataRowView)grid_allpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectartDr.Delete();
            calAllPart();
            caltotalpart();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            DataTable at = artTb.Copy();
            #region 判斷Pattern的Artwork  不可為空
            DataRow[] findr = patternTb.Select("PatternCode<>'ALLPARTS' and (art='' or art is null)", "");
            if (findr.Length > 0)
            {
                MyUtility.Msg.WarningBox("<Art> can not be empty!");
                return;
            }
            #endregion

            DataTable bundle_detail_tmp;
            DBProxy.Current.Select(null, "Select *,0 as ukey1,'' as subprocessid from bundle_Detail WITH (NOLOCK) where 1=0", out bundle_detail_tmp);
            int bundlegroup = Convert.ToInt32(maindatarow["startno"]);
            int ukey = 1;
            grid_qty.ValidateControl();
            foreach (DataRow dr in qtyTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    foreach (DataRow dr2 in patternTb.Rows)
                    {
                        if (Convert.ToInt32(dr2["Parts"]) == 0) continue;  //若Parts=0，則不需產生資料至Bundle card明細
                        DataRow nDetail = bundle_detail_tmp.NewRow();
                        nDetail["PatternCode"] = dr2["PatternCode"];
                        nDetail["PatternDesc"] = dr2["PatternDesc"];
                        nDetail["Parts"] = dr2["Parts"];
                        nDetail["Qty"] = dr["Qty"];
                        nDetail["SizeCode"] = dr["SizeCode"];
                        nDetail["bundlegroup"] = bundlegroup;
                        nDetail["ukey1"] = ukey;

                        if (dr2["PatternCode"].ToString() != "ALLPARTS")
                        {
                            nDetail["subprocessid"] = dr2["art"].ToString();
                            //if (dr2["art"].ToString().Substring(dr2["art"].ToString().Length - 1) != "+")
                            //{
                            //    nDetail["subprocessid"] = dr2["art"].ToString() + "+";
                            //}
                            //else
                            //{
                            //    nDetail["subprocessid"] = dr2["art"].ToString();
                            //}
                        }                       
                        ukey++;

                        bundle_detail_tmp.Rows.Add(nDetail);
                    }
                    bundlegroup++;
                }

            }
            alltmpTb.Clear();
            bundle_detail_artTb.Clear();
            //平行覆蓋資料
            int j = 0;
            int detailRow = 0;
            int tmpRow = bundle_detail_tmp.Rows.Count;
            foreach (DataRow dr in detailTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {

                    if (j < tmpRow)
                    {
                        DataRow tmpdr = bundle_detail_tmp.Rows[j];
                        dr["bundlegroup"] = tmpdr["bundlegroup"];
                        dr["PatternCode"] = tmpdr["PatternCode"];
                        dr["PatternDesc"] = tmpdr["PatternDesc"];
                        dr["subprocessid"] = tmpdr["subprocessid"];
                        dr["Parts"] = tmpdr["Parts"];
                        dr["Qty"] = tmpdr["Qty"];
                        dr["SizeCode"] = tmpdr["SizeCode"];
                        dr["ukey1"] = tmpdr["ukey1"];
                        j++;
                        if (tmpdr["PatternCode"].ToString() == "ALLPARTS")
                        {

                            #region 讓Bundle_Detail_Allpart只產生一份資料
                            if (allpartTb.Rows.Count > alltmpTb.Rows.Count)
                            {
                                foreach (DataRow aldr in allpartTb.Rows)
                                {
                                    if (aldr.RowState != DataRowState.Deleted)
                                    {
                                        DataRow allpart_ndr = alltmpTb.NewRow();
                                        //allpart_ndr["Bundleno"] = dr["Bundleno"];
                                        allpart_ndr["PatternCode"] = aldr["PatternCode"];
                                        allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                                        allpart_ndr["Parts"] = aldr["Parts"];
                                        allpart_ndr["ukey1"] = dr["ukey1"];
                                        alltmpTb.Rows.Add(allpart_ndr);
                                    }
                                }
                            }
                            #endregion

                        }
                        DataRow[] artary = patternTb.Select(string.Format("PatternCode='{0}'", tmpdr["PatternCode"]));
                        foreach (DataRow artdr in artary)
                        {
                            if (artdr.RowState != DataRowState.Deleted)
                            {
                                DataRow art_ndr = bundle_detail_artTb.NewRow();
                                art_ndr["Bundleno"] = dr["Bundleno"];
                                art_ndr["PatternCode"] = artdr["PatternCode"]; 
                                art_ndr["Subprocessid"] = artdr["art"];
                                art_ndr["ukey1"] = dr["ukey1"];
                                bundle_detail_artTb.Rows.Add(art_ndr);
                            }
                        }
                    }
                }
                detailRow++;
            }
            //判斷當前表身的筆數(排除掉已刪除的Row)
            DataTable dtCount = detailTb.Copy();
            dtCount.AcceptChanges();
            if (tmpRow < dtCount.Rows.Count) //當舊的比較多就要刪除
            {
                int detailrow = detailTb.Rows.Count;
                for (int i = detailrow - 1; i >= tmpRow; i--) //因為delete時Rowcount 會改變所以要重後面往前刪
                {
                    detailTb.Rows[i].Delete();
                }
            }
            if (tmpRow > j) //表示新增的比較多需要Insert
            {
                for (int i = 0; i < tmpRow - j; i++)
                {
                    DataRow ndr = detailTb.NewRow();
                    DataRow tmpdr = bundle_detail_tmp.Rows[j + i];
                    ndr["bundlegroup"] = tmpdr["bundlegroup"];
                    ndr["PatternCode"] = tmpdr["PatternCode"];
                    ndr["PatternDesc"] = tmpdr["PatternDesc"];
                    ndr["subprocessid"] = tmpdr["subprocessid"];
                    ndr["Parts"] = tmpdr["Parts"];
                    ndr["Qty"] = tmpdr["Qty"];
                    ndr["SizeCode"] = tmpdr["SizeCode"];
                    ndr["ukey1"] = tmpdr["ukey1"];
                    detailTb.Rows.Add(ndr);
                    if (tmpdr["PatternCode"].ToString() == "ALLPARTS")
                    {

                        #region 讓Bundle_Detail_Allpart只產生一份資料
                        if (allpartTb.Rows.Count > alltmpTb.Rows.Count)
                        {
                            foreach (DataRow aldr in allpartTb.Rows)
                            {
                                if (aldr.RowState != DataRowState.Deleted)
                                {
                                    DataRow allpart_ndr = alltmpTb.NewRow();

                                    allpart_ndr["PatternCode"] = aldr["PatternCode"];
                                    allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                                    allpart_ndr["Parts"] = aldr["Parts"];
                                    allpart_ndr["ukey1"] = tmpdr["ukey1"];
                                    alltmpTb.Rows.Add(allpart_ndr);
                                }
                            }
                        }
                        #endregion

                    }
                    DataRow[] artary = patternTb.Select(string.Format("PatternCode='{0}'", tmpdr["PatternCode"]));
                    foreach (DataRow artdr in artary)
                    {
                        if (artdr.RowState != DataRowState.Deleted)
                        {
                            DataRow art_ndr = bundle_detail_artTb.NewRow();

                            art_ndr["PatternCode"] = artdr["PatternCode"];
                            art_ndr["Subprocessid"] = artdr["art"];
                            art_ndr["ukey1"] = tmpdr["ukey1"];
                            bundle_detail_artTb.Rows.Add(art_ndr);
                        }
                    }
                }
            }

            #region 新增AllParts
            /*
           *確認右下All Part 有無資料
           *有資料就加總右下Parts數量
           *並且新增一個AllPart在左下 並填上AllPart總數量
           */
            DataTable allpartTb_Copy = allpartTb.Copy();
            allpartTb_Copy.AcceptChanges();
            if (!MyUtility.Check.Empty(allpartTb_Copy) && allpartTb_Copy.Rows.Count > 0)
            {
                decimal Parts = 0;
                for (int i = 0; i < allpartTb_Copy.Rows.Count; i++)
                {
                    Parts = Parts + MyUtility.Convert.GetDecimal(allpartTb_Copy.Rows[i]["Parts"]);
                }
                DataRow[] AllPart = detailTb.Select("PatternCode='ALLPARTS'");
                if (!MyUtility.Check.Empty(Parts))
                {
                    if (AllPart.Length == 0)                   
                    {
                        DataTable dtAllPart;
                        DataTable dtMax = detailTb.Copy();
                        dtMax.AcceptChanges();
                        MyUtility.Tool.ProcessWithDatatable(dtMax, @"BundleGroup,SizeCode,qty", @"select distinct BundleGroup,SizeCode,qty from #tmp", out dtAllPart);
                        if (dtAllPart.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAllPart.Rows.Count; i++)
                            {

                                List<int> ukey1 = dtMax.AsEnumerable().Select(numb => numb.Field<int>("ukey1")).Distinct().ToList();
                                int MaxUkey = ukey1.Max();
                                DataRow drAll = detailTb.NewRow();
                                drAll["PatternCode"] = "ALLPARTS";
                                drAll["PatternDesc"] = "All Parts";
                                drAll["Qty"] = dtAllPart.Rows[i]["qty"].ToString();
                                drAll["SizeCode"] = dtAllPart.Rows[i]["SizeCode"].ToString();
                                drAll["parts"] = Parts;
                                drAll["BundleGroup"] = dtAllPart.Rows[i]["BundleGroup"].ToString();
                                drAll["ukey1"] = MaxUkey + 1;
                                detailTb.Rows.Add(drAll);

                            }

                        }
                        else if (detailTb2 != null && detailTb2.Rows.Count > 0)
                        {
                            MyUtility.Tool.ProcessWithDatatable(detailTb2, @"BundleGroup,SizeCode,qty", @"select distinct BundleGroup,SizeCode,qty from #tmp", out dtAllPart);
                            DataRow drAll = detailTb.NewRow();
                            drAll["PatternCode"] = "ALLPARTS";
                            drAll["PatternDesc"] = "All Parts";
                            drAll["Qty"] = dtAllPart.Rows[0]["qty"].ToString();
                            drAll["SizeCode"] = dtAllPart.Rows[0]["SizeCode"].ToString();
                            drAll["parts"] = Parts;
                            drAll["BundleGroup"] = dtAllPart.Rows[0]["BundleGroup"].ToString();
                            drAll["ukey1"] = 1;
                            detailTb.Rows.Add(drAll);

                        }

                    }
                }
            }
            #endregion


            #region 把處理好的資料塞回上層Table
            detailTb2.Clear();
            alltmpTb2.Clear();
            bundle_detail_artTb2.Clear();
            qtyTb2.Clear();

            maindatarow["Qty"] = numNoOfBundle.Value;
            detailTb2.Merge(detailTb);
            alltmpTb2.Merge(alltmpTb);
            bundle_detail_artTb2.Merge(bundle_detail_artTb);
            qtyTb2.Merge(qtyTb);
            #endregion

            this.Close();
        }
        
        private void btnGarment_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", maindatarow["poid"].ToString(), "Orders", "ID");
            Sci.Production.PublicForm.GarmentList callNextForm = new Sci.Production.PublicForm.GarmentList(ukey, maindatarow["poid"].ToString(), maindatarow["cutref"].ToString());
            callNextForm.ShowDialog(this);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void P10_Generate_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
        }
    }
}
