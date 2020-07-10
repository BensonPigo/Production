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
using Ict;
using Sci.Win.Tools;
using Sci.Production.PublicPrg;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Sci.Production.Cutting
{
    public partial class P10_Generate : Win.Subs.Base
    {
        DataRow maindatarow;
        DataTable allpartTb;
        DataTable patternTb;
        DataTable artTb;
        DataTable sizeTb;
        DataTable garmentTb;
        DataTable detailTb;
        DataTable alltmpTb;
        DataTable bundle_detail_artTb;
        DataTable qtyTb;
        DataTable detailTb2;
        DataTable alltmpTb2;
        DataTable bundle_detail_artTb2;
        DataTable qtyTb2;
        DataTable f_codeTb;
        DataTable garmentarRC;

        public P10_Generate(DataRow maindr, DataTable table_bundle_Detail, DataTable bundle_Detail_allpart_Tb, DataTable bundle_Detail_Art_Tb, DataTable bundle_Detail_Qty_Tb)
        {
            this.InitializeComponent();
            this.chkTone.Checked = MyUtility.Convert.GetBool(maindr["ByToneGenerate"]);
            #region 準備要處理的table 和原本的table
            this.detailTb = table_bundle_Detail.Copy();
            this.alltmpTb = bundle_Detail_allpart_Tb.Copy();
            this.bundle_detail_artTb = bundle_Detail_Art_Tb.Copy();
            this.qtyTb = bundle_Detail_Qty_Tb.Copy();
            this.maindatarow = maindr;

            this.detailTb2 = table_bundle_Detail;
            this.alltmpTb2 = bundle_Detail_allpart_Tb;
            this.bundle_detail_artTb2 = bundle_Detail_Art_Tb;
            this.qtyTb2 = bundle_Detail_Qty_Tb;
            #endregion

            #region 取tabel的結構
            string cmd_st = "Select 0 as Sel, PatternCode ,Location ,PatternDesc, '' as annotation,parts,IsPair from Bundle_detail_allpart WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_st, out this.allpartTb);
            string pattern_cmd = "Select patternCode,PatternDesc ,Location ,Parts,'' as art,0 AS parts,IsPair,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_Detail WITH (NOLOCK) Where 1=0"; // 左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out this.patternTb);
            string cmd_art = "Select PatternCode,subprocessid,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_detail_art WITH (NOLOCK) where 1=0";
            DBProxy.Current.Select(null, cmd_art, out this.artTb);
            #endregion

            #region Size-CutQty
            int totalCutQty = 0;
            if (MyUtility.Check.Empty(maindr["cutref"])) // 無CutRef 就直接抓取Order_Qty 的SizeCode
            {
                this.labelTotalCutOutput.Visible = false;
                this.displayTotalCutOutput.Visible = false;
                string size_cmd = string.Format("Select distinct sizecode,0  as Qty from order_Qty WITH (NOLOCK) where id='{0}'", maindr["Orderid"]);
                DualResult dResult = DBProxy.Current.Select(null, size_cmd, out this.sizeTb);
            }
            else
            {
                string size_cmd = string.Format(
                    @"
Select b.sizecode,isnull(sum(b.Qty),0)  as Qty 
from Workorder a WITH (NOLOCK) 
inner join Workorder_distribute b WITH (NOLOCK) on a.ukey = b.workorderukey
where a.cutref='{0}' and b.orderid='{1}'
group by sizeCode",
                    maindr["cutref"], maindr["Orderid"]);
                DualResult dResult = DBProxy.Current.Select(null, size_cmd, out this.sizeTb);
                if (this.sizeTb.Rows.Count != 0)
                {
                    totalCutQty = Convert.ToInt32(this.sizeTb.Compute("Sum(Qty)", string.Empty));
                }
                else
                {
                    size_cmd = string.Format("Select distinct sizecode,0  as Qty from order_Qty WITH (NOLOCK) where id='{0}'", maindr["Orderid"]);
                    dResult = DBProxy.Current.Select(null, size_cmd, out this.sizeTb);
                }

                this.displayTotalCutOutput.Value = totalCutQty;
            }
            #endregion

            #region 左上qtyTb
            this.numNoOfBundle.Value = (decimal)maindr["Qty"];
            if (!MyUtility.Check.Empty(this.maindatarow["cutref"]) && this.qtyTb.Rows.Count == 0)
            {
                int j = 1;
                foreach (DataRow dr in this.sizeTb.Rows)
                {
                    if (this.numNoOfBundle.Value < j)
                    {
                        break;
                    }

                    DataRow row = this.qtyTb.NewRow();
                    row["No"] = j;
                    row["SizeCode"] = dr["SizeCode"];
                    row["Qty"] = dr["Qty"];
                    this.qtyTb.Rows.Add(row);
                    j++;
                }
            }
            else
            {
                int j = 1;
                foreach (DataRow dr in this.qtyTb.Rows)
                {
                    dr["No"] = j;
                    j++;
                }
            }
            #endregion

            #region 準備GarmentList & ArticleGroup
            string sizes = string.Empty;
            if (this.qtyTb != null)
            {
                var sizeList = this.qtyTb.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();
                sizes = "'" + string.Join("','", sizeList) + "'";
            }

            string sizeGroup = string.Empty;
            if (!MyUtility.Check.Empty(sizes))
            {
                string sqlSizeGroup = $@"SELECT TOP 1 IIF(ISNULL(SizeGroup,'')='','N',SizeGroup) FROM Order_SizeCode WHERE ID ='{this.maindatarow["poid"].ToString()}' and SizeCode IN ({sizes})";
                sizeGroup = MyUtility.GetValue.Lookup(sqlSizeGroup);
            }

            // GarmentList
            Prgs.GetGarmentListTable(maindr["cutref"].ToString(), this.maindatarow["poid"].ToString(), sizeGroup, out this.garmentTb);

            // ArticleGroup
            string patidsql;
            string Styleyukey = MyUtility.GetValue.Lookup("Styleukey", this.maindatarow["poid"].ToString(), "Orders", "ID");

            patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{this.maindatarow["poid"].ToString()}','{this.maindatarow["cutref"].ToString()}','',{Styleyukey},'{sizeGroup}')s";

            string patternukey = MyUtility.GetValue.Lookup(patidsql);
            string headercodesql = string.Format(
                @"
Select distinct ArticleGroup 
from Pattern_GL_LectraCode WITH (NOLOCK) 
where PatternUkey = '{0}'
order by ArticleGroup", patternukey);
            DBProxy.Current.Select(null, headercodesql, out this.f_codeTb);
            #endregion

            // 計算左上TotalQty
            this.calsumQty();

            // if (detailTb.Rows.Coun!= 0 && maindatarow.RowState!=DataRowState.Added)
            int detailTbCnt = this.detailTb.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Count();
            if (detailTbCnt > 0)
            {
                this.exist_Table_Query();
            }
            else
            {
                this.noexist_Table_Query();
            }

            this.grid_setup();
            this.calAllPart();
            this.caltotalpart();

            this.displayPatternPanel.Text = maindr["PatternPanel"].ToString();
        }

        // 第一次產生時需全部重新撈值
        public void noexist_Table_Query()
        {
            // 找出相同PatternPanel 的subprocessid
            int npart = 0; // allpart 數量
            StringBuilder w = new StringBuilder();
            w.Append("1 = 0");
            foreach (DataRow dr in this.f_codeTb.Rows)
            {
                w.Append(string.Format(" or {0} = '{1}' ", dr[0], this.maindatarow["FabricPanelCode"]));
            }

            DataRow[] garmentar = this.garmentTb.Select(w.ToString());
            foreach (DataRow dr in garmentar)
            {
                if (MyUtility.Check.Empty(dr["annotation"])) // 若無ANNOTATion直接寫入All Parts
                {
                    DataRow ndr = this.allpartTb.NewRow();
                    ndr["PatternCode"] = dr["PatternCode"];
                    ndr["PatternDesc"] = dr["PatternDesc"];
                    ndr["Location"] = dr["Location"];
                    ndr["parts"] = MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                    ndr["isPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
                    this.allpartTb.Rows.Add(ndr);
                    npart = npart + MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                }
                else
                {
                    // 取得哪些 annotation 是次要
                    List<string> notMainList = this.GetNotMain(dr, garmentar);
                    string noBundleCardAfterSubprocess_String = string.Join("+", notMainList);

                    // Annotation
                    string[] ann = Regex.Replace(dr["annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation 去除字串中數字
                    string art = string.Empty;
                    #region Annotation有在Subprocess 內需要寫入bundle_detail_art，寫入Bundle_Detail_pattern
                    if (ann.Length > 0)
                    {
                        bool lallpart;
                        #region 算Subprocess
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
                                    ndr2["IsPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
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
                                ndr2["IsPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
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
                            ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            ndr["IsPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
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
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        ndr["IsPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
                        this.allpartTb.Rows.Add(ndr);
                    }
                    #endregion
                }
            }

            DataRow pdr = this.patternTb.NewRow(); // 預設要有ALLPARTS
            pdr["PatternCode"] = "ALLPARTS";
            pdr["PatternDesc"] = "All Parts";
            pdr["parts"] = npart;
            this.patternTb.Rows.Add(pdr);

            this.garmentarRC = null;
            this.garmentarRC = this.garmentTb.Clone();
            foreach (DataRow gdr in garmentar)
            {
                this.garmentarRC.ImportRow(gdr);
            }
        }

        // 當bundle_allPart, bundle_art 存在時的對應資料
        public void exist_Table_Query()
        {
            // 將Bundle_Detial_Art distinct PatternCode,
            DataTable tmp;

            // 用來當判斷條件的DataTable,避免DetailTB dataRow被刪除後無法用index撈出資料
            DataTable detailAccept = this.detailTb.Copy();
            detailAccept.AcceptChanges();
            string BundleGroup = detailAccept.Rows[0]["BundleGroup"].ToString();
            MyUtility.Tool.ProcessWithDatatable(this.detailTb, "PatternCode,PatternDesc,parts,subProcessid,BundleGroup,isPair,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String", $@"
Select PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String
into #tmp2
from #tmp where BundleGroup='{BundleGroup}'
group by PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String

union all
select PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String
from #tmp where BundleGroup='{BundleGroup}' and IsPair = 1
group by PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String

select *
from #tmp2
order by iif(PatternCode='AllParts','ZZZZZZZ',PatternCode)

drop table #tmp,#tmp2
", out tmp);

            // 需要使用上一層表身的值,不可重DB撈不然新增的資料就不會存回DB
            MyUtility.Tool.ProcessWithDatatable(this.detailTb, "PatternCode,SubProcessid,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String", "Select distinct PatternCode,SubProcessid,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String from #tmp WHERE PatternCode<>'ALLPARTS'", out this.artTb);

            // foreach (DataRow dr in tmp.Select("BundleNO<>''"))
            foreach (DataRow dr in tmp.Rows)
            {
                DataRow ndr = this.patternTb.NewRow();
                ndr["PatternCode"] = dr["PatternCode"];
                ndr["PatternDesc"] = dr["PatternDesc"];
                ndr["Location"] = dr["Location"];
                ndr["Parts"] = dr["Parts"];
                ndr["isPair"] = dr["isPair"];
                ndr["art"] = dr["SubProcessid"].ToString();
                ndr["NoBundleCardAfterSubprocess_String"] = dr["NoBundleCardAfterSubprocess_String"];
                ndr["PostSewingSubProcess_String"] = dr["PostSewingSubProcess_String"];
                this.patternTb.Rows.Add(ndr);
            }

            MyUtility.Tool.ProcessWithDatatable(this.alltmpTb, "sel,PatternCode,PatternDesc,parts,annotation,isPair,Location", "Select distinct sel,PatternCode,PatternDesc,parts,annotation,isPair,Location from #tmp", out this.allpartTb);
            foreach (DataRow dr in this.allpartTb.Rows)
            {
                DataRow[] adr = this.garmentTb.Select(string.Format("PatternCode='{0}'", dr["patternCode"]));
                if (adr.Length > 0)
                {
                    dr["annotation"] = adr[0]["annotation"];
                }
            }

            if (this.allpartTb.Rows.Count == 0)
            {
                StringBuilder w = new StringBuilder();
                w.Append("1 = 0");
                foreach (DataRow dr in this.f_codeTb.Rows)
                {
                    w.Append(string.Format(" or {0} = '{1}' ", dr[0], this.maindatarow["PatternPanel"]));
                }

                DataRow[] garmentar = this.garmentTb.Select(w.ToString());
                foreach (DataRow dr in garmentar)
                {
                    bool f = false;
                    foreach (DataRow drp in this.patternTb.Rows)
                    {
                        if (dr["patternCode"].ToString() == drp["patternCode"].ToString())
                        {
                            f = true;
                        }
                    }

                    if (!f)
                    {
                        DataRow ndr = this.allpartTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["PatternDesc"] = dr["PatternDesc"];
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        this.allpartTb.Rows.Add(ndr);
                    }
                }
            }

            StringBuilder w2 = new StringBuilder();
            w2.Append("1 = 0");
            foreach (DataRow dr in this.f_codeTb.Rows)
            {
                w2.Append(string.Format(" or {0} = '{1}' ", dr[0], this.maindatarow["FabricPanelCode"]));
            }

            if (this.garmentTb.Rows.Count > 0)
            {
                this.garmentarRC = this.garmentTb.Select(w2.ToString()).CopyToDataTable();
            }
            else
            {
                this.garmentarRC = this.garmentTb.Clone();
            }
        }

        public void grid_setup()
        {
            DataGridViewGeneratorNumericColumnSettings NoCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings qtyCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings subcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patternDesc = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell1 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings isPair = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings NoBundleCardAfterSubprocess_String = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings PostSewingSubProcess_String = new DataGridViewGeneratorTextColumnSettings();

            #region 左上grid
            NoCell.CellValidating += (s, e) =>
            {
                if (MyUtility.Convert.GetInt(this.numNoOfBundle.Text) < MyUtility.Convert.GetInt(e.FormattedValue))
                {
                    MyUtility.Msg.WarningBox(string.Format("<No: {0} >  can't greater than <No of Bundle>", e.FormattedValue));
                    return;
                }
            };
            qtyCell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_qty.GetDataRow(this.listControlBindingSource1.Position);
                string oldvalue = dr["qty"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["qty"] = newvalue;
                dr.EndEdit();
                this.calsumQty();
            };
            #endregion

            #region 左下grid
            patterncell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;

                    sele = new SelectItem(this.garmentarRC, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}'").Count() > 0)
                    {
                        dr["isPair"] = this.patternTb.Select($@"PatternCode = '{sele.GetSelectedString()}'")[0]["isPair"];
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
                    this.calAllPart();
                    this.caltotalpart();
                    this.CheckNotMain(dr);
                }
            };
            patterncell.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                string patcode = e.FormattedValue.ToString();
                string oldvalue = dr["PatternCode"].ToString();
                if (oldvalue == patcode)
                {
                    return;
                }

                if (this.patternTb.Select($@"PatternCode = '{patcode}'").Count() > 0)
                {
                    dr["isPair"] = this.patternTb.Select($@"PatternCode = '{patcode}'")[0]["isPair"];
                }

                DataRow[] gemdr = this.garmentarRC.Select(string.Format("PatternCode ='{0}'", patcode), string.Empty);
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
                else
                {
                    MyUtility.Msg.WarningBox(string.Format("<CutPart: {0} >  can't found!", e.FormattedValue));
                    dr["PatternCode"] = string.Empty;
                    dr["PatternDesc"] = string.Empty;
                    dr["art"] = string.Empty;
                    dr["Parts"] = 0;
                }

                dr.EndEdit();
                this.CheckNotMain(dr);
            };

            patternDesc.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                dr["PatternDesc"] = e.FormattedValue;
                dr.EndEdit();
                this.CheckNotMain(dr);
            };

            subcell.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
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

                    // string nbcass = MyUtility.Convert.GetString(dr["NoBundleCardAfterSubprocess_String"]);
                    // if (!arts.Contains(nbcass))
                    // {
                    //    dr["NoBundleCardAfterSubprocess_String"] = string.Empty;
                    // }
                    List<string> recordPS = new List<string>();
                    foreach (var item in arts)
                    {
                        if (pssps.Contains(item))
                        {
                            recordPS.Add(item);
                        }
                    }

                    dr["PostSewingSubProcess_String"] = string.Join("+", recordPS);
                    dr.EndEdit();

                    DataRow[] artdr = this.artTb.Select(string.Format("PatternCode='{0}'", dr["PatternCode"]));
                    foreach (DataRow adr in artdr)
                    {
                        adr.Delete();
                    }

                    foreach (DataRow dt in sele.GetSelecteds())
                    {
                        DataRow ndr = this.artTb.NewRow();
                        ndr["PatternCode"] = dr["PatternCode"];
                        ndr["subprocessid"] = dt["id"];
                        this.artTb.Rows.Add(ndr);
                    }

                    this.CheckNotMain(dr);
                }
            };
            PostSewingSubProcess_String.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
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
                    SelectItem2 item = new SelectItem2(sqlcmd, "Subprocess", "23", dr["PostSewingSubProcess_String"].ToString(), null, null, null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["PostSewingSubProcess_String"] = item.GetSelectedString().Replace(",", "+");
                    dr.EndEdit();
                }
            };
            PostSewingSubProcess_String.CellFormatting += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
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
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
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
                    SelectItem item = new SelectItem(sqlcmd, "23", MyUtility.Convert.GetString(dr["NoBundleCardAfterSubprocess_String"]), "Subprocess");
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["NoBundleCardAfterSubprocess_String"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            NoBundleCardAfterSubprocess_String.CellFormatting += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["art"]) || dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    e.CellStyle.BackColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };

            partsCell1.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.calAllPart();
                this.caltotalpart();
            };
            isPair.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(dr["PatternCode"]).ToUpper() != "ALLPARTS")
                {
                    bool ispair = MyUtility.Convert.GetBool(e.FormattedValue);
                    dr["IsPair"] = ispair;
                    dr.EndEdit();
                    if (this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}'").Count() > 0)
                    {
                        foreach (DataRow item in this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}'"))
                        {
                            item["IsPair"] = ispair;
                        }
                    }
                }
            };
            #endregion

            #region 右下grid
            patterncell2.EditingMouseDown += (s, e) =>
            {
                DataRow dr = this.grid_allpart.GetDataRow(e.RowIndex);
                if (dr["PatternCode"].ToString() == "ALLPARTS")
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    SelectItem sele;

                    sele = new SelectItem(this.garmentarRC, "PatternCode,PatternDesc,Annotation", "10,20,20", dr["PatternCode"].ToString(), false, ",");
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
                    this.calAllPart();
                    this.caltotalpart();
                }
            };

            patterncell2.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_allpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();

                DataRow[] gemdr = this.garmentarRC.Select(string.Format("PatternCode ='{0}'", newvalue), string.Empty);
                if (gemdr.Length > 0)
                {
                    dr["PatternDesc"] = gemdr[0]["PatternDesc"].ToString();
                    dr["PatternCode"] = gemdr[0]["PatternCode"].ToString();
                    dr["Annotation"] = gemdr[0]["Annotation"].ToString();
                    dr["parts"] = 1;
                }
                else
                {
                    MyUtility.Msg.WarningBox(string.Format("<CutPart: {0} >  can't found!", e.FormattedValue));
                    dr["Sel"] = 0;
                    dr["PatternCode"] = string.Empty;
                    dr["PatternDesc"] = string.Empty;
                    dr["Annotation"] = string.Empty;
                    dr["Parts"] = 0;
                }

                dr.EndEdit();
                this.calAllPart();
                this.caltotalpart();
            };

            partsCell2.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_allpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.calAllPart();
                this.caltotalpart();
            };

            #endregion

            // 左上
            this.listControlBindingSource1.DataSource = this.qtyTb;
            this.grid_qty.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid_qty)
            .Numeric("No", header: "No", width: Widths.AnsiChars(4), integer_places: 5, settings: NoCell)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(7), integer_places: 5, settings: qtyCell);
            this.grid_qty.Columns["No"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_qty.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;

            // 左下
            this.grid_art.DataSource = this.patternTb;
            this.grid_art.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid_art)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(15), settings: patternDesc)
            .Text("Location", header: "Location", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("art", header: "Artwork", width: Widths.AnsiChars(15), iseditingreadonly: true, settings: subcell)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partsCell1)
            .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: isPair)
            .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: PostSewingSubProcess_String)
            .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: NoBundleCardAfterSubprocess_String)
            ;
            this.grid_art.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_art.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_art.Columns["art"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_art.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;

            // 右下
            this.grid_allpart.DataSource = this.allpartTb;
            this.grid_allpart.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.grid_allpart)
            .CheckBox("Sel", header: "Chk", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13))
            .Text("Location", header: "Location", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partsCell2)
            .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);
            this.grid_allpart.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_allpart.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_allpart.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_allpart.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;

            // 右上
            this.grid_Size.DataSource = this.sizeTb;
            this.grid_Size.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid_Size)
            .Text("SizeCode", header: "SizeCode", width: Widths.AnsiChars(8))
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 5);

            // 左上因為資料顯示已有排序，所以按Grid Header不可以做排序
            for (int i = 0; i < this.grid_qty.ColumnCount; i++)
            {
                this.grid_qty.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        // 賦予流水號
        private void qtyTb_serial()
        {
            int serial = 1;
            foreach (DataRow dr in this.qtyTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["No"] = serial;
                    serial++;
                }
            }
        }

        // 分配Qty
        public void calQty()
        {
            foreach (DataRow dr in this.sizeTb.Rows)
            {
                double TotalCutQty = Convert.ToDouble(dr["Qty"]);
                DataRow[] qtyarry = this.qtyTb.Select(string.Format("SizeCode='{0}'", dr["SizeCode"]), string.Empty);
                double rowcount = qtyarry.Length;
                Prgs.AverageNumeric(qtyarry, "Qty", (int)TotalCutQty, true);
            }

            this.calsumQty();
        }

        public void calsumQty()
        {
            if (this.qtyTb.Rows.Count > 0)
            {
                this.displayTotalQty.Value = Convert.ToInt32(this.qtyTb.Compute("sum(Qty)", string.Empty));
            }
        }

        private void button_Qty_Click(object sender, EventArgs e)
        {
            if (this.qtyTb.Rows.Count != 0)
            {
                DataRow selectSizeDr = ((DataRowView)this.grid_Size.GetSelecteds(SelectedSort.Index)[0]).Row;
                DataRow selectQtyeDr = ((DataRowView)this.grid_qty.GetSelecteds(SelectedSort.Index)[0]).Row;
                selectQtyeDr["SizeCode"] = selectSizeDr["SizeCode"];
                if (!MyUtility.Check.Empty(this.maindatarow["cutref"]))
                {
                    this.calQty();
                }
                else
                {
                    selectQtyeDr["Qty"] = 0; // cutref為空指定行qty為0
                }

                #region 把左上的grid移至下一筆
                int currentRowIndexInt = this.grid_qty.CurrentRow.Index;
                if (currentRowIndexInt + 1 < this.grid_qty.RowCount)
                {
                    this.grid_qty.CurrentCell = this.grid_qty[0, currentRowIndexInt + 1];
                    this.grid_qty.FirstDisplayedScrollingRowIndex = currentRowIndexInt + 1;
                }
                #endregion
            }
        }

        private void grid_Size_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.button_Qty_Click(sender, e);
        }

        private void button_LefttoRight_Click(object sender, EventArgs e)
        {
            this.grid_allpart.ValidateControl();
            this.grid_art.ValidateControl();
            this.grid_qty.ValidateControl();
            if (MyUtility.Check.Empty(this.grid_art.DataSource) || this.grid_art.Rows.Count == 0)
            {
                return;
            }

            DataRow selectartDr = ((DataRowView)this.grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
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
            ndr["Parts"] = selectartDr["Parts"];
            ndr["isPair"] = selectartDr["isPair"];

            // Annotation
            DataRow[] adr = this.garmentTb.Select(string.Format("PatternCode='{0}'", selectartDr["patternCode"]));
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
                            if (dr["art"].ToString().IndexOf(dr2["subprocessid"].ToString()) == -1)
                            {
                                dr2.Delete();
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

            this.calAllPart();
            this.caltotalpart();
        }

        private void button_RighttoLeft_Click(object sender, EventArgs e)
        {
            this.grid_allpart.ValidateControl();
            this.grid_art.ValidateControl();
            this.grid_qty.ValidateControl();
            if (this.patternTb.Rows.Count == 0)
            {
                return;
            }

            if (this.grid_allpart.RowCount == 0)
            {
                return;
            }

            DataRow selectartDr = ((DataRowView)this.grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            DataRow selectallparteDr = ((DataRowView)this.grid_allpart.GetSelecteds(SelectedSort.Index)[0]).Row;

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
                    if (this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}'").Count() > 0)
                    {
                        isPair = MyUtility.Convert.GetBool(this.patternTb.Select($@"PatternCode = '{chdr["PatternCode"]}'")[0]["isPair"]);
                    }

                    // 新增PatternTb
                    DataRow ndr2 = this.patternTb.NewRow();
                    ndr2["PatternCode"] = chdr["PatternCode"];
                    ndr2["PatternDesc"] = chdr["PatternDesc"];
                    ndr2["Location"] = chdr["Location"];
                    ndr2["Parts"] = chdr["Parts"];
                    ndr2["art"] = "EMB";
                    ndr2["isPair"] = isPair;

                    this.patternTb.Rows.Add(ndr2);
                    chdr.Delete(); // 刪除
                }
            }
            else if (checkdr.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data !!");
            }
            #endregion
            this.calAllPart();
            this.caltotalpart();
        }

        public void caltotalpart() // 計算total part
        {
            if (this.patternTb.Rows.Count > 0)
            {
                this.numTotalParts.Value = Convert.ToDecimal(this.patternTb.Compute("Sum(Parts)", "Parts IS NOT NULL"));
            }
        }

        public void calAllPart() // 計算all part
        {
            int allpart = 0;
            if (this.allpartTb.AsEnumerable().Count(row => row.RowState != DataRowState.Deleted) > 0)
            {
                allpart = this.allpartTb.AsEnumerable()
                   .Where(row => row.RowState != DataRowState.Deleted)
                   .Sum(row => row["Parts"] == null || row["Parts"] == DBNull.Value ? 0 : Convert.ToInt32(row["Parts"]));
            }

            DataRow[] dr = this.patternTb.Select("PatternCode='ALLPARTS'");
            if (dr.Length > 0)
            {
                dr[0]["Parts"] = allpart;
            }

            if (dr.Length == 0 && allpart > 0)
            {
                DataRow drAll = this.patternTb.NewRow();
                drAll["PatternCode"] = "ALLPARTS";
                drAll["PatternDesc"] = "All Parts";
                drAll["Location"] = string.Empty;
                drAll["parts"] = allpart;
                this.patternTb.Rows.Add(drAll);
            }
        }

        private void insertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = this.patternTb.NewRow();
            this.patternTb.Rows.Add();
            this.grid_art.ValidateControl();
        }

        private void NumNoOfBundle_Validating(object sender, CancelEventArgs e)
        {
            int newvalue = (int)this.numNoOfBundle.Value;
            int oldvalue = (int)this.numNoOfBundle.OldValue;
            if (newvalue == oldvalue)
            {
                return;
            }

            if (this.qtyTb.Rows.Count == 0)
            {
                for (int i = 0; i < newvalue; i++)
                {
                    DataRow ndr = this.qtyTb.NewRow();
                    ndr["Qty"] = 0;
                    this.qtyTb.Rows.Add(ndr);
                }

                this.qtyTb_serial();
            }
            else
            {
                if (!MyUtility.Check.Empty(this.maindatarow["cutref"]))
                {
                    int rowindex = this.grid_qty.CurrentRow.Index;
                    string SizeCode = this.grid_qty.Rows[rowindex].Cells["SizeCode"].Value.ToString();

                    if (!MyUtility.Check.Empty(newvalue))
                    {
                        this.qtyTb.Clear();
                    }

                    int count = 0;
                    foreach (DataRow dr in this.sizeTb.Rows)
                    {
                        if (count < newvalue)
                        {
                            DataRow ndr = this.qtyTb.NewRow();
                            ndr["SizeCode"] = dr["SizeCode"];
                            this.qtyTb.Rows.Add(ndr);
                            count++;
                        }
                    }

                    // 如果No of Bundle數量>右上SizeCode數量,就依照左上滑鼠選擇的SizeCode的值複製多出來的數量
                    if (newvalue > count)
                    {
                        for (int i = 0; i < newvalue - count; i++)
                        {
                            DataRow ndr = this.qtyTb.NewRow();
                            ndr["SizeCode"] = SizeCode;
                            this.qtyTb.Rows.Add(ndr);
                        }
                    }

                    this.qtyTb_serial();
                    this.calQty();
                }
                else
                {
                    DataTable qtytmp = this.qtyTb.Copy();
                    this.qtyTb.Clear();
                    int count = 0;
                    foreach (DataRow dr in qtytmp.Rows)
                    {
                        if (count < newvalue)
                        {
                            DataRow ndr = this.qtyTb.NewRow();
                            ndr[0] = dr[0];
                            ndr[1] = dr[1];
                            ndr[2] = dr[2];
                            ndr[3] = dr[3];
                            ndr[4] = dr[4];
                            this.qtyTb.Rows.Add(ndr);
                            count++;
                        }
                    }

                    // 增加時
                    if (this.numNoOfBundle.OldValue != null)
                    {
                        for (int i = 0; i < newvalue - (int)this.numNoOfBundle.OldValue; i++)
                        {
                            DataRow ndr = this.qtyTb.NewRow();
                            ndr["Qty"] = 0;
                            this.qtyTb.Rows.Add(ndr);
                        }
                    }

                    this.qtyTb_serial();
                }
            }
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.grid_art.Rows.Count == 0)
            {
                return;
            }

            DataRow selectartDr = ((DataRowView)this.grid_art.GetSelecteds(SelectedSort.Index)[0]).Row;
            if (selectartDr["PatternCode"].ToString() == "ALLPARTS")
            {
                MyUtility.Msg.WarningBox("Please remove all right grid's parts to instead of removeing ALLPARTS directly!");
                return;
            }

            selectartDr.Delete();
            this.caltotalpart();
        }

        private void allpart_insert_Click(object sender, EventArgs e)
        {
            this.allpartTb.NewRow();
            this.allpartTb.Rows.Add();
        }

        private void allpart_delete_Click(object sender, EventArgs e)
        {
            if (this.grid_allpart.Rows.Count == 0)
            {
                return;
            }

            DataRow selectartDr = ((DataRowView)this.grid_allpart.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectartDr.Delete();
            this.calAllPart();
            this.caltotalpart();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            if (this.numTone.Value > this.numNoOfBundle.Value)
            {
                MyUtility.Msg.WarningBox("Generate by Tone can not greater than No of Bunde");
                return;
            }

            DataTable at = this.artTb.Copy();
            #region 判斷Pattern的Artwork  不可為空
            DataRow[] findr = this.patternTb.Select("PatternCode<>'ALLPARTS' and (art='' or art is null)", string.Empty);
            if (findr.Length > 0)
            {
                MyUtility.Msg.WarningBox("<Art> can not be empty!");
                return;
            }
            #endregion
            #region 判斷Pattern的CutPart  不可為空
            bool isEmptyCutPart = this.patternTb.AsEnumerable().Where(s => MyUtility.Check.Empty(s["PatternCode"])).Any();
            if (isEmptyCutPart)
            {
                MyUtility.Msg.WarningBox("<CutPart> can not be empty!");
                return;
            }
            #endregion
            #region 檢查 如果IsPair =✔, 加總相同的Cut Part的Parts, 必需>0且可以被2整除
            var SamePairCt = this.patternTb.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["isPair"]))
                .GroupBy(g => new { CutPart = g["PatternCode"] })
                .Select(s => new { s.Key.CutPart, Parts = s.Sum(i => MyUtility.Convert.GetDecimal(i["Parts"])) }).ToList();
            if (SamePairCt.Where(w => w.Parts % 2 != 0).Any())
            {
                var mp = SamePairCt.Where(w => w.Parts % 2 != 0).ToList();
                string msg = @"The following bundle is pair, but parts is not pair, please check Cut Part parts";
                DataTable dt = this.ToDataTable(mp);
                MyUtility.Msg.ShowMsgGrid(dt, msg: msg, caption: "Warning");
                return;
            }
            #endregion

            #region 判斷AllPartDetail的CutPart  不可為空
            bool isEmptyAllPartDetailCutPart = this.allpartTb.AsEnumerable()
                                                .Where(s =>
                                                {
                                                    if (s.RowState == DataRowState.Deleted)
                                                    {
                                                        return false;
                                                    }
                                                    else
                                                    {
                                                        return MyUtility.Check.Empty(s["PatternCode"]);
                                                    }
                                                }).Any();
            if (isEmptyAllPartDetailCutPart)
            {
                MyUtility.Msg.WarningBox("All Parts Detail <CutPart> can not be empty!");
                return;
            }
            #endregion

            DataTable bundle_detail_tmp = this.detailTb.Clone();
            int bundlegroup = Convert.ToInt32(this.maindatarow["startno"]);
            int ukey = 1;
            this.grid_qty.ValidateControl();
            foreach (DataRow dr in this.qtyTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    foreach (DataRow dr2 in this.patternTb.Rows)
                    {
                        if (dr2["Parts"] == DBNull.Value)
                        {
                            continue;
                        }

                        if (Convert.ToInt32(dr2["Parts"]) == 0)
                        {
                            continue;  // 若Parts=0，則不需產生資料至Bundle card明細
                        }

                        DataRow nDetail = bundle_detail_tmp.NewRow();
                        nDetail["PatternCode"] = dr2["PatternCode"];
                        nDetail["PatternDesc"] = dr2["PatternDesc"];
                        nDetail["Location"] = dr2["Location"];
                        nDetail["Parts"] = dr2["Parts"];
                        nDetail["Qty"] = dr["Qty"];
                        nDetail["SizeCode"] = dr["SizeCode"];
                        nDetail["bundlegroup"] = bundlegroup;
                        nDetail["ukey1"] = ukey;
                        nDetail["isPair"] = dr2["isPair"];

                        if (dr2["PatternCode"].ToString() != "ALLPARTS")
                        {
                            nDetail["subprocessid"] = dr2["art"].ToString();
                            nDetail["NoBundleCardAfterSubprocess_String"] = dr2["NoBundleCardAfterSubprocess_String"];
                            nDetail["PostSewingSubProcess_String"] = dr2["PostSewingSubProcess_String"];
                        }

                        ukey++;

                        bundle_detail_tmp.Rows.Add(nDetail);
                    }

                    bundlegroup++;
                }
            }

            this.alltmpTb.Clear();
            this.bundle_detail_artTb.Clear();

            // 平行覆蓋資料
            int j = 0;
            int detailRow = 0;
            int tmpRow = bundle_detail_tmp.Rows.Count;
            bool notYetInsertAllPart = true;
            foreach (DataRow dr in this.detailTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
            {
                if (j < tmpRow)
                {
                    DataRow tmpdr = bundle_detail_tmp.Rows[j];
                    dr["bundlegroup"] = tmpdr["bundlegroup"];
                    dr["PatternCode"] = tmpdr["PatternCode"];
                    dr["PatternDesc"] = tmpdr["PatternDesc"];
                    dr["Location"] = tmpdr["Location"];
                    dr["subprocessid"] = tmpdr["subprocessid"];
                    dr["NoBundleCardAfterSubprocess_String"] = tmpdr["NoBundleCardAfterSubprocess_String"];
                    dr["PostSewingSubProcess_String"] = tmpdr["PostSewingSubProcess_String"];
                    dr["Parts"] = tmpdr["Parts"];
                    dr["Qty"] = tmpdr["Qty"];
                    dr["SizeCode"] = tmpdr["SizeCode"];
                    dr["ukey1"] = tmpdr["ukey1"];
                    dr["isPair"] = tmpdr["isPair"];
                    j++;
                    if (tmpdr["PatternCode"].ToString() == "ALLPARTS" && notYetInsertAllPart)
                    {
                        foreach (DataRow aldr in this.allpartTb.Rows)
                        {
                            if (aldr.RowState == DataRowState.Deleted)
                            {
                                continue;
                            }

                            if (aldr["Parts"] == DBNull.Value)
                            {
                                continue;
                            }

                            if (Convert.ToInt32(aldr["Parts"]) == 0)
                            {
                                continue;
                            }

                            DataRow allpart_ndr = this.alltmpTb.NewRow();
                            allpart_ndr["PatternCode"] = aldr["PatternCode"];
                            allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                            allpart_ndr["Location"] = aldr["Location"];
                            allpart_ndr["Parts"] = aldr["Parts"];
                            allpart_ndr["ukey1"] = dr["ukey1"];
                            allpart_ndr["ispair"] = aldr["ispair"];
                            this.alltmpTb.Rows.Add(allpart_ndr);
                        }

                        notYetInsertAllPart = false;
                    }
                    else
                    {
                        DataRow art_ndr = this.bundle_detail_artTb.NewRow();
                        art_ndr["Bundleno"] = dr["Bundleno"];
                        art_ndr["PatternCode"] = dr["PatternCode"];
                        art_ndr["Subprocessid"] = dr["subprocessid"];
                        art_ndr["NoBundleCardAfterSubprocess_String"] = dr["NoBundleCardAfterSubprocess_String"];
                        art_ndr["PostSewingSubProcess_String"] = dr["PostSewingSubProcess_String"];
                        art_ndr["ukey1"] = dr["ukey1"];
                        this.bundle_detail_artTb.Rows.Add(art_ndr);
                    }
                }
                else
                {
                    dr.AcceptChanges();
                    dr.Delete();
                }

                detailRow++;
            }

            // 判斷當前表身的筆數(排除掉已刪除的Row)
            DataTable dtCount;
            if (this.detailTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().Count() > 0)
            {
                dtCount = this.detailTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).CopyToDataTable();
            }
            else
            {
                dtCount = this.detailTb.Clone();
            }

            dtCount.AcceptChanges();
            int detailrow = this.detailTb.Rows.Count;
            int deleteCnt = dtCount.Rows.Count - tmpRow;
            for (int i = 1; i <= deleteCnt; i++)
            {
                this.detailTb.Rows[detailrow - i].Delete();
            }

            if (tmpRow > j) // 表示新增的比較多需要Insert
            {
                for (int i = 0; i < tmpRow - j; i++)
                {
                    DataRow ndr = this.detailTb.NewRow();
                    DataRow tmpdr = bundle_detail_tmp.Rows[j + i];
                    ndr["bundlegroup"] = tmpdr["bundlegroup"];
                    ndr["PatternCode"] = tmpdr["PatternCode"];
                    ndr["PatternDesc"] = tmpdr["PatternDesc"];
                    ndr["subprocessid"] = tmpdr["subprocessid"];
                    ndr["NoBundleCardAfterSubprocess_String"] = tmpdr["NoBundleCardAfterSubprocess_String"];
                    ndr["PostSewingSubProcess_String"] = tmpdr["PostSewingSubProcess_String"];
                    ndr["Location"] = tmpdr["Location"];
                    ndr["Parts"] = tmpdr["Parts"];
                    ndr["Qty"] = tmpdr["Qty"];
                    ndr["SizeCode"] = tmpdr["SizeCode"];
                    ndr["ukey1"] = tmpdr["ukey1"];
                    ndr["isPair"] = tmpdr["isPair"];
                    this.detailTb.Rows.Add(ndr);
                    if (tmpdr["PatternCode"].ToString() == "ALLPARTS" && notYetInsertAllPart)
                    {
                        foreach (DataRow aldr in this.allpartTb.Rows)
                        {
                            if (aldr.RowState == DataRowState.Deleted)
                            {
                                continue;
                            }

                            if (aldr["Parts"] == DBNull.Value)
                            {
                                continue;
                            }

                            if (Convert.ToInt32(aldr["Parts"]) == 0)
                            {
                                continue;
                            }

                            DataRow allpart_ndr = this.alltmpTb.NewRow();

                            allpart_ndr["PatternCode"] = aldr["PatternCode"];
                            allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                            allpart_ndr["Location"] = aldr["Location"];
                            allpart_ndr["Parts"] = aldr["Parts"];
                            allpart_ndr["ukey1"] = tmpdr["ukey1"];
                            allpart_ndr["isPair"] = aldr["isPair"];
                            this.alltmpTb.Rows.Add(allpart_ndr);
                        }

                        notYetInsertAllPart = false;
                    }
                    else
                    {
                        DataRow art_ndr = this.bundle_detail_artTb.NewRow();
                        art_ndr["PatternCode"] = tmpdr["PatternCode"];
                        art_ndr["Subprocessid"] = tmpdr["subprocessid"];
                        art_ndr["NoBundleCardAfterSubprocess_String"] = tmpdr["NoBundleCardAfterSubprocess_String"];
                        art_ndr["PostSewingSubProcess_String"] = tmpdr["PostSewingSubProcess_String"];
                        art_ndr["ukey1"] = tmpdr["ukey1"];
                        this.bundle_detail_artTb.Rows.Add(art_ndr);
                    }
                }
            }

            #region 新增AllParts
            /*
           *確認右下All Part 有無資料
           *有資料就加總右下Parts數量
           *並且新增一個AllPart在左下 並填上AllPart總數量
           */
            DataTable allpartTb_Copy = this.allpartTb.Copy();
            allpartTb_Copy.AcceptChanges();
            if (!MyUtility.Check.Empty(allpartTb_Copy) && allpartTb_Copy.Rows.Count > 0)
            {
                decimal Parts = 0;
                for (int i = 0; i < allpartTb_Copy.Rows.Count; i++)
                {
                    Parts = Parts + MyUtility.Convert.GetDecimal(allpartTb_Copy.Rows[i]["Parts"]);
                }

                DataRow[] AllPart = this.detailTb.Select("PatternCode='ALLPARTS'");
                if (!MyUtility.Check.Empty(Parts))
                {
                    if (AllPart.Length == 0)
                    {
                        DataTable dtAllPart;
                        DataTable dtMax = this.detailTb.Copy();
                        dtMax.AcceptChanges();
                        MyUtility.Tool.ProcessWithDatatable(dtMax, @"BundleGroup,SizeCode,qty", @"select distinct BundleGroup,SizeCode,qty from #tmp", out dtAllPart);
                        if (dtAllPart.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAllPart.Rows.Count; i++)
                            {
                                List<int> ukey1 = dtMax.AsEnumerable().Select(numb => numb.Field<int>("ukey1")).Distinct().ToList();
                                int MaxUkey = ukey1.Max();
                                DataRow drAll = this.detailTb.NewRow();
                                drAll["PatternCode"] = "ALLPARTS";
                                drAll["PatternDesc"] = "All Parts";
                                drAll["Qty"] = dtAllPart.Rows[i]["qty"].ToString();
                                drAll["SizeCode"] = dtAllPart.Rows[i]["SizeCode"].ToString();
                                drAll["parts"] = Parts;
                                drAll["BundleGroup"] = dtAllPart.Rows[i]["BundleGroup"].ToString();
                                drAll["ukey1"] = MaxUkey + 1;
                                this.detailTb.Rows.Add(drAll);
                            }
                        }
                        else if (this.detailTb2 != null && this.detailTb2.Rows.Count > 0)
                        {
                            MyUtility.Tool.ProcessWithDatatable(this.detailTb2, @"BundleGroup,SizeCode,qty", @"select distinct BundleGroup,SizeCode,qty from #tmp", out dtAllPart);
                            DataRow drAll = this.detailTb.NewRow();
                            drAll["PatternCode"] = "ALLPARTS";
                            drAll["PatternDesc"] = "All Parts";
                            drAll["Qty"] = dtAllPart.Rows[0]["qty"].ToString();
                            drAll["SizeCode"] = dtAllPart.Rows[0]["SizeCode"].ToString();
                            drAll["parts"] = Parts;
                            drAll["BundleGroup"] = dtAllPart.Rows[0]["BundleGroup"].ToString();
                            drAll["ukey1"] = 1;
                            this.detailTb.Rows.Add(drAll);
                        }
                    }
                }
            }
            #endregion

            #region Generate by Tone 有勾選再處理一次
            if (this.chkTone.Checked && this.numTone.Value > 0)
            {
                int bundlegroupS = Convert.ToInt32(this.maindatarow["startno"]);
                int tone = MyUtility.Convert.GetInt(this.numTone.Value);
                DataTable dtDetail = new DataTable();
                DataTable dtAllPart = new DataTable();
                DataTable dtAllPart2 = this.detailTb.Clone();
                DataTable dtArt = this.bundle_detail_artTb.Copy();
                this.bundle_detail_artTb.Clear();

                int na = this.detailTb.Select("PatternCode <> 'AllParts'").Length;
                int a = this.detailTb.Select("PatternCode = 'AllParts'").Length;
                if (na > 0)
                {
                    dtDetail = this.detailTb.Select("PatternCode <> 'AllParts'").CopyToDataTable();
                    dtDetail.Columns.Add("tmpNum", typeof(int));
                }

                if (a > 0)
                {
                    dtAllPart = this.detailTb.Select("PatternCode = 'AllParts'").CopyToDataTable();
                }

                for (int i = this.detailTb.Rows.Count - 1; i >= 0; i--)
                {
                    this.detailTb.Rows[i].Delete();
                }

                int ukeytone = 1;
                if (na > 0)
                {
                    for (int i = 0; i < tone; i++)
                    {
                        int tmpNum = 0;
                        DataTable dtCopy = dtDetail.Copy();
                        foreach (DataRow item in dtCopy.Rows)
                        {
                            item["bundlegroup"] = bundlegroupS + i; // 重設bundlegroup

                            item["tmpNum"] = tmpNum; // 暫時紀錄原本資料對應拆出去的資料,要用來重分配Qty
                            tmpNum++;

                            DataTable dtCopyArt = dtArt.Copy();
                            DataRow artdr = dtCopyArt.Select($"Ukey1 = {item["Ukey1"]}")[0];
                            artdr["BundleNo"] = string.Empty;
                            artdr["Ukey1"] = ukeytone;
                            item["Ukey1"] = ukeytone;
                            this.bundle_detail_artTb.ImportRow(artdr);
                            ukeytone++;
                        }

                        this.detailTb.Merge(dtCopy);
                    }

                    // 重分每一筆拆的Qty
                    int tmpNumF = 0;
                    for (int i = 0; i < this.detailTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Count() / tone; i++, tmpNumF++)
                    {
                        DataRow[] drD = this.detailTb.Select($"tmpNum={tmpNumF}").AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToArray();
                        Prgs.AverageNumeric(drD, "Qty", MyUtility.Convert.GetInt(drD[0]["Qty"]), true);
                    }

                    this.detailTb.Columns.Remove("tmpNum");
                }

                // 處理All Part筆數
                if (a > 0)
                {
                    DataRow row = dtAllPart.Rows[0];
                    for (int i = 0; i < tone; i++)
                    {
                        row["BundleGroup"] = bundlegroupS + i;
                        int notAllpart = this.patternTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().Count() - 1;
                        notAllpart = notAllpart == 0 ? 1 : notAllpart;
                        row["Qty"] = this.detailTb.AsEnumerable().
                            Where(w => w.RowState != DataRowState.Deleted &&
                            MyUtility.Convert.GetString(w["PatternCode"]) != "ALLPARTS" &&
                            MyUtility.Convert.GetInt(w["BundleGroup"]) == MyUtility.Convert.GetInt(row["BundleGroup"])).
                            Sum(s => MyUtility.Convert.GetInt(s["Qty"]))
                            / notAllpart;
                        dtAllPart2.ImportRow(row);
                    }

                    DataRow[] drA = dtAllPart2.AsEnumerable().ToArray();
                    foreach (DataRow item in dtAllPart2.Rows)
                    {
                        item["Ukey1"] = ukeytone;
                        ukeytone++;
                    }

                    this.detailTb.Merge(dtAllPart2);
                }

                foreach (DataRow dr in this.detailTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
                {
                    dr["BundleNo"] = string.Empty;
                    dr.AcceptChanges();
                    dr.SetAdded();
                }
            }
            else
            {
                this.chkTone.Checked = false;
            }
            #endregion

            DataRow[] art_dr = this.bundle_detail_artTb.Select("PatternCode = 'ALLPARTS'");
            for (int i = art_dr.Count() - 1; i >= 0; i--)
            {
                art_dr[i].Delete();
            }
            #region 把處理好的資料塞回上層Table
            this.detailTb2.Clear();
            this.alltmpTb2.Clear();
            this.bundle_detail_artTb2.Clear();
            this.qtyTb2.Clear();
            this.maindatarow["Qty"] = this.numNoOfBundle.Value;
            this.detailTb2.Merge(this.detailTb);
            this.alltmpTb2.Merge(this.alltmpTb);
            this.bundle_detail_artTb2.Merge(this.bundle_detail_artTb);
            this.qtyTb2.Merge(this.qtyTb);
            #endregion

            this.maindatarow["ByToneGenerate"] = this.chkTone.Checked;

            this.Close();
        }

        private void btnGarment_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.maindatarow["poid"].ToString(), "Orders", "ID");
            var Sizelist = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            PublicForm.GarmentList callNextForm = new PublicForm.GarmentList(ukey, this.maindatarow["poid"].ToString(), this.maindatarow["cutref"].ToString(), Sizelist);
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
            // 1.先判斷 PatternCode + PatternDesc 是否存在 garmentarRC (為 garmentTb 篩選後)
            // 2.判斷選擇的 Artwork  EX:選擇 AT+HT, 在PatternCode + PatternDes找到 HT+AT01, 才算此筆為 garmentarRC 內的資料
            // 3.判斷是否為次要裁
            DataRow[] drs = this.garmentarRC.Select($"PatternCode='{dr["PatternCode"]}'and PatternDesc = '{dr["PatternDesc"]}'");
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

            List<string> notMainList = this.GetNotMain(dr1, this.garmentarRC.Select()); // 帶入未去除數字的annotation資料
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
