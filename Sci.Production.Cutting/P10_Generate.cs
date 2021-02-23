using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P10_Generate : Win.Subs.Base
    {
        private DataRow maindatarow;
        private DataTable allpartTbOri;
        private DataTable patternTbOri;
        private DataTable allpartTb;
        private DataTable patternTb;

        private DataTable artTb;
        private DataTable sizeTb;
        private DataTable garmentTb;
        private DataTable f_codeTb;
        private DataTable garmentarRC;

        // 不論新增/編輯，複製一份Load後的資料，用在勾選ComBine/取消ComBine塞回資料用
        private DataTable bundle_Detail_T;
        private DataTable bundle_Detail_allpart_T;
        private DataTable bundle_Detail_Art_T;
        private DataTable bundle_Detail_Qty_T;
        private DataTable bundle_Detail_CombineSubprocess_T;

        private DataTable bundle_Detail;
        private DataTable bundle_Detail_allpart;
        private DataTable bundle_Detail_Art;
        private DataTable bundle_Detail_CombineSubprocess;
        private DataTable bundle_Detail_Qty;

        private bool ByToneGenerate;
        private bool noneShellchk_ReadOnly;

        /// <inheritdoc/>
        public P10_Generate(DataRow maindr, DataTable bundle_Detail, DataTable bundle_Detail_allpart, DataTable bundle_Detail_Qty, DataTable bundle_Detail_Art, DataTable bundle_Detail_CombineSubprocess, bool noneShellchk_ReadOnly)
        {
            this.InitializeComponent();
            this.ByToneGenerate = MyUtility.Convert.GetBool(maindr["ByToneGenerate"]);
            this.noneShellchk_ReadOnly = noneShellchk_ReadOnly;

            #region 準備要處理的table 和原本的table
            this.maindatarow = maindr;

            this.bundle_Detail_T = bundle_Detail.Copy();
            this.bundle_Detail_allpart_T = bundle_Detail_allpart.Copy();
            this.bundle_Detail_Qty_T = bundle_Detail_Qty.Copy();
            this.bundle_Detail_Art_T = bundle_Detail_Art.Copy();
            this.bundle_Detail_CombineSubprocess_T = bundle_Detail_CombineSubprocess.Copy();

            this.bundle_Detail = bundle_Detail;
            this.bundle_Detail_allpart = bundle_Detail_allpart;
            this.bundle_Detail_Qty = bundle_Detail_Qty;
            this.bundle_Detail_Art = bundle_Detail_Art;
            this.bundle_Detail_CombineSubprocess = bundle_Detail_CombineSubprocess;
            #endregion

            #region 取tabel的結構
            string pattern_cmd = "Select top 0 PatternCode, PatternDesc ,Location , Parts,art = '', parts = 0, isPair, NoBundleCardAfterSubprocess_String='', PostSewingSubProcess_String='', isMain = cast(0 as bit), CombineSubprocessGroup = cast(0 as tinyint) from Bundle_Detail WITH (NOLOCK)"; // 左下的Table
            DBProxy.Current.Select(null, pattern_cmd, out this.patternTb);
            this.patternTbOri = this.patternTb.Clone();

            string cmd_st = "Select top 0 sel = cast(0 as bit), PatternCode, Location, PatternDesc, annotation = '', parts, isPair, isMain = cast(0 as bit), CombineSubprocessGroup = cast(0 as tinyint) from Bundle_detail_allpart WITH (NOLOCK)";
            DBProxy.Current.Select(null, cmd_st, out this.allpartTb);
            this.allpartTbOri = this.allpartTb.Clone();

            string cmd_art = "Select top 0 PatternCode, subprocessid, NoBundleCardAfterSubprocess_String = '', PostSewingSubProcess_String = '' from Bundle_detail_art WITH (NOLOCK)";
            DBProxy.Current.Select(null, cmd_art, out this.artTb);
            #endregion

            #region Size-CutQty
            int totalCutQty = 0;

            // 無CutRef 就直接抓取Order_Qty 的SizeCode
            if (MyUtility.Check.Empty(maindr["cutref"]))
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
                    maindr["cutref"],
                    maindr["Orderid"]);
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
            if (!MyUtility.Check.Empty(this.maindatarow["cutref"]) && this.bundle_Detail_Qty_T.Rows.Count == 0)
            {
                int j = 1;
                foreach (DataRow dr in this.sizeTb.Rows)
                {
                    if (this.numNoOfBundle.Value < j)
                    {
                        break;
                    }

                    DataRow row = this.bundle_Detail_Qty_T.NewRow();
                    row["No"] = j;
                    row["SizeCode"] = dr["SizeCode"];
                    row["Qty"] = dr["Qty"];
                    this.bundle_Detail_Qty_T.Rows.Add(row);
                    j++;
                }
            }
            else
            {
                int j = 1;
                foreach (DataRow dr in this.bundle_Detail_Qty_T.Rows)
                {
                    dr["No"] = j;
                    j++;
                }
            }
            #endregion

            #region 準備GarmentList & ArticleGroup
            string sizes = string.Empty;
            if (this.bundle_Detail_Qty_T != null)
            {
                var sizeList = this.bundle_Detail_Qty_T.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();
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
            string styleyukey = MyUtility.GetValue.Lookup("Styleukey", this.maindatarow["poid"].ToString(), "Orders", "ID");

            patidsql = $@"select s.PatternUkey from dbo.GetPatternUkey('{this.maindatarow["poid"].ToString()}','{this.maindatarow["cutref"].ToString()}','',{styleyukey},'{sizeGroup}')s";

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
            this.CalsumQty();

            int detailTbCnt = this.bundle_Detail_T.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted).Count();

            // patternTbOri,allpartTbOri 從 Garment 準備資料
            this.ProcessOriDatas();

            if (detailTbCnt > 0 || this.bundle_Detail_CombineSubprocess.Rows.Count > 0)
            {
                this.Exist_Table_Query();
            }
            else
            {
                this.chkTone.Checked = MyUtility.Convert.GetBool(MyUtility.GetValue.Lookup("select AutoGenerateByTone from System"));
                if (this.chkTone.Checked)
                {
                    this.numTone.Value = 1;
                }

                this.patternTb = this.patternTbOri.Copy();
                this.allpartTb = this.allpartTbOri.Copy();
            }

            this.Grid_setup();
            this.CalculateParts();
            this.displayPatternPanel.Text = maindr["PatternPanel"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.chkCombineSubprocess.Checked = this.bundle_Detail_CombineSubprocess.Rows.Count > 0;
            this.chkNoneShellNoCreateAllParts.ReadOnly = this.noneShellchk_ReadOnly;
        }

        /// <summary>
        /// 第一次產生時需全部重新撈值
        /// </summary>
        private void ProcessOriDatas()
        {
            // 找出相同PatternPanel 的subprocessid
            // allpart 數量
            int npart = 0;
            StringBuilder w = new StringBuilder();
            w.Append("1 = 0");
            foreach (DataRow dr in this.f_codeTb.Rows)
            {
                w.Append(string.Format(" or {0} = '{1}' ", dr[0], this.maindatarow["FabricPanelCode"]));
            }

            this.garmentTb.Columns.Add("CombineSubprocessGroup", typeof(int));
            this.garmentTb.Columns.Add("IsMain", typeof(bool));
            DataRow[] garmentar = this.garmentTb.Select(w.ToString());
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
                    ndr["isPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
                    ndr["CombineSubprocessGroup"] = 0;
                    this.allpartTbOri.Rows.Add(ndr);
                    npart = npart + MyUtility.Convert.GetInt(dr["alone"]) + (MyUtility.Convert.GetInt(dr["DV"]) * 2) + (MyUtility.Convert.GetInt(dr["Pair"]) * 2);
                }
                else
                {
                    // 取得哪些 annotation 是次要
                    List<string> notMainList = this.GetNotMain(dr, this.garmentTb.Select());
                    string noBundleCardAfterSubprocess_String = string.Join("+", notMainList);

                    // Annotation
                    string[] ann = Regex.Replace(dr["annotation"].ToString(), @"[\d]", string.Empty).Split('+'); // 剖析Annotation 去除字串中數字
                    string art = string.Empty;
                    #region Annotation有在Subprocess 內需要寫入bundle_detail_art，寫入Bundle_Detail_pattern
                    if (ann.Length > 0)
                    {
                        #region 算Subprocess
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
                            ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                            ndr["isPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
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
                        ndr["parts"] = Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        npart = npart + Convert.ToInt32(dr["alone"]) + (Convert.ToInt32(dr["DV"]) * 2) + (Convert.ToInt32(dr["Pair"]) * 2);
                        ndr["isPair"] = MyUtility.Convert.GetInt(dr["PAIR"]) == 1;
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
            pdr["CombineSubprocessGroup"] = 0;
            this.patternTbOri.Rows.Add(pdr);

            this.garmentarRC = null;
            this.garmentarRC = this.garmentTb.Clone();
            foreach (DataRow gdr in garmentar)
            {
                this.garmentarRC.ImportRow(gdr);
            }
        }

        /// <summary>
        /// 當bundle_allPart, bundle_art 存在時的對應資料
        /// </summary>
        private void Exist_Table_Query()
        {
            this.chkTone.Checked = this.ByToneGenerate;

            // 用來當判斷條件的DataTable,避免DetailTB dataRow被刪除後無法用index撈出資料
            DataTable detailAccept = this.bundle_Detail_T.Copy();
            detailAccept.AcceptChanges();
            string bundleGroup = detailAccept.Rows[0]["BundleGroup"].ToString();
            int seq = 0;
            this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().ForEach(f => f["tmpSeq"] = seq++);

            // 將Bundle_Detial_Art distinct PatternCode,
            string sqlCmd = $@"
Select PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String,tmpSeq=min(tmpSeq)
into #tmp2
from #tmp where BundleGroup='{bundleGroup}'
group by PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String

union all
select PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String,tmpSeq=min(tmpSeq)
from #tmp where BundleGroup='{bundleGroup}' and isPair = 1
group by PatternCode,PatternDesc,Parts,subProcessid,BundleGroup ,isPair ,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String

select *,isMain = cast(0 as bit),CombineSubprocessGroup = cast(0 as tinyint) 
from #tmp2
order by tmpSeq,iif(PatternCode='AllParts','ZZZZZZZ',PatternCode)

drop table #tmp,#tmp2";
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.bundle_Detail_T, "PatternCode,PatternDesc,parts,subProcessid,BundleGroup,isPair,Location,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String,tmpSeq", sqlCmd, out DataTable tmp);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            // 需要使用上一層表身的值,不可重DB撈不然新增的資料就不會存回DB
            result = MyUtility.Tool.ProcessWithDatatable(this.bundle_Detail_T, "PatternCode,SubProcessid,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String", "Select distinct PatternCode,SubProcessid,NoBundleCardAfterSubprocess_String,PostSewingSubProcess_String,CombineSubprocessGroup = cast(0 as tinyint)  from #tmp WHERE PatternCode <> 'ALLPARTS'", out this.artTb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

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

            #region 右下區塊資料
            result = MyUtility.Tool.ProcessWithDatatable(this.bundle_Detail_allpart_T, string.Empty, "Select distinct sel = cast(sel as bit), PatternCode, PatternDesc, parts, annotation, isPair, Location, isMain = cast(0 as bit), CombineSubprocessGroup = cast(0 as tinyint) from #tmp", out this.allpartTb);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            foreach (DataRow dr in this.allpartTb.Rows)
            {
                DataRow[] adr = this.garmentTb.Select(string.Format("PatternCode='{0}'", dr["PatternCode"]));
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
                        if (dr["PatternCode"].ToString() == drp["PatternCode"].ToString())
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
                        ndr["CombineSubprocessGroup"] = 0;
                        this.allpartTb.Rows.Add(ndr);
                    }
                }
            }
            #endregion

            StringBuilder w2 = new StringBuilder();
            w2.Append("1 = 0");
            foreach (DataRow dr in this.f_codeTb.Rows)
            {
                w2.Append(string.Format(" or {0} = '{1}' ", dr[0], this.maindatarow["FabricPanelCode"]));
            }

            this.garmentarRC = this.garmentTb.Select(w2.ToString()).TryCopyToDataTable(this.garmentTb);
        }

        private void Grid_setup()
        {
            DataGridViewGeneratorNumericColumnSettings noCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings qtyCell = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorTextColumnSettings subcell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patternDesc = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings patterncell2 = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell1 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings partsCell2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings isPair = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorTextColumnSettings noBundleCardAfterSubprocess_String = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings postSewingSubProcess_String = new DataGridViewGeneratorTextColumnSettings();

            #region 左上grid
            noCell.CellValidating += (s, e) =>
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
                this.CalsumQty();
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
                    this.CalculateParts();
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
                    #region 算Subprocess
                    if (ann.Length > 0)
                    {
                        art = Prgs.BundleCardCheckSubprocess(ann, dr["PatternCode"].ToString(), this.artTb, out bool lallpart);
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
                this.SynchronizeMain(0, "PatternCode");
                this.CheckNotMain(dr);
            };

            patternDesc.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                dr["PatternDesc"] = e.FormattedValue;
                dr.EndEdit();
                this.SynchronizeMain(0, "patternDesc");
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
            postSewingSubProcess_String.EditingMouseDown += (s, e) =>
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
            postSewingSubProcess_String.CellFormatting += (s, e) =>
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
            noBundleCardAfterSubprocess_String.EditingMouseDown += (s, e) =>
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
            noBundleCardAfterSubprocess_String.CellFormatting += (s, e) =>
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
                this.CalculateParts();
            };
            isPair.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_art.GetDataRow(e.RowIndex);
                if (MyUtility.Convert.GetString(dr["PatternCode"]).ToUpper() != "ALLPARTS")
                {
                    bool ispair = MyUtility.Convert.GetBool(e.FormattedValue);
                    dr["isPair"] = ispair;
                    dr.EndEdit();
                    if (this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}'").Count() > 0)
                    {
                        foreach (DataRow item in this.patternTb.Select($@"PatternCode = '{dr["PatternCode"]}'"))
                        {
                            item["isPair"] = ispair;
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
                    this.SynchronizeMain(1, "PatternCode");
                    this.CalculateParts();
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
                    dr["sel"] = 0;
                    dr["PatternCode"] = string.Empty;
                    dr["PatternDesc"] = string.Empty;
                    dr["Annotation"] = string.Empty;
                    dr["Parts"] = 0;
                }

                dr.EndEdit();
                this.SynchronizeMain(1, "PatternCode");
                this.CalculateParts();
            };

            partsCell2.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_allpart.GetDataRow(e.RowIndex);
                string oldvalue = dr["Parts"].ToString();
                string newvalue = e.FormattedValue.ToString();
                dr["Parts"] = newvalue;
                dr.EndEdit();
                this.CalculateParts();
            };

            DataGridViewGeneratorTextColumnSettings patternDesc2 = new DataGridViewGeneratorTextColumnSettings
            {
                CharacterCasing = CharacterCasing.Normal,
            };
            patternDesc2.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid_allpart.GetDataRow(e.RowIndex);
                dr["PatternDesc"] = e.FormattedValue;
                dr.EndEdit();
                this.SynchronizeMain(1, "PatternDesc");
                this.CalculateParts();
            };
            #endregion

            // 左上
            this.listControlBindingSource1.DataSource = this.bundle_Detail_Qty_T;
            this.grid_qty.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid_qty)
            .Numeric("No", header: "No", width: Widths.AnsiChars(4), integer_places: 5, settings: noCell)
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
            .CheckBox("isPair", header: "isPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: isPair)
            .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: postSewingSubProcess_String)
            .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: noBundleCardAfterSubprocess_String)
            ;
            this.grid_art.Columns["PatternCode"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_art.Columns["PatternDesc"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_art.Columns["art"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid_art.Columns["Parts"].DefaultCellStyle.BackColor = Color.Pink;

            // 右下
            this.grid_allpart.DataSource = this.allpartTb;
            this.grid_allpart.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.Helper.Controls.Grid.Generator(this.grid_allpart)
            .CheckBox("sel", header: "Chk", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("PatternCode", header: "CutPart", width: Widths.AnsiChars(10), settings: patterncell2)
            .Text("PatternDesc", header: "CutPart Name", width: Widths.AnsiChars(13), settings: patternDesc2)
            .Text("Location", header: "Location", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Annotation", header: "Annotation", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(3), integer_places: 3, settings: partsCell2)
            .CheckBox("isPair", header: "isPair", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0);
            this.grid_allpart.Columns["sel"].DefaultCellStyle.BackColor = Color.Pink;
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

        /// <summary>
        /// 賦予流水號
        /// </summary>
        private void QtyTb_serial()
        {
            int serial = 1;
            foreach (DataRow dr in this.bundle_Detail_Qty_T.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["No"] = serial;
                    serial++;
                }
            }
        }

        /// <summary>
        /// 分配Qty
        /// </summary>
        private void CalQty()
        {
            foreach (DataRow dr in this.sizeTb.Rows)
            {
                double totalCutQty = Convert.ToDouble(dr["Qty"]);
                DataRow[] qtyarry = this.bundle_Detail_Qty_T.Select(string.Format("SizeCode='{0}'", dr["SizeCode"]), string.Empty);
                double rowcount = qtyarry.Length;
                Prgs.AverageNumeric(qtyarry, "Qty", (int)totalCutQty, true);
            }

            this.CalsumQty();
        }

        /// <summary>
        /// Cal sum Qty
        /// </summary>
        private void CalsumQty()
        {
            if (this.bundle_Detail_Qty_T.Rows.Count > 0)
            {
                this.displayTotalQty.Value = Convert.ToInt32(this.bundle_Detail_Qty_T.Compute("sum(Qty)", string.Empty));
            }
        }

        private void Button_Qty_Click(object sender, EventArgs e)
        {
            if (this.bundle_Detail_Qty_T.Rows.Count != 0)
            {
                DataRow selectSizeDr = ((DataRowView)this.grid_Size.GetSelecteds(SelectedSort.Index)[0]).Row;
                DataRow selectQtyeDr = ((DataRowView)this.grid_qty.GetSelecteds(SelectedSort.Index)[0]).Row;
                selectQtyeDr["SizeCode"] = selectSizeDr["SizeCode"];
                if (!MyUtility.Check.Empty(this.maindatarow["cutref"]))
                {
                    this.CalQty();
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

        private void Grid_Size_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Button_Qty_Click(sender, e);
        }

        private void Button_LefttoRight_Click(object sender, EventArgs e)
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

            // 移動此筆
            DataRow ndr = this.allpartTb.NewRow();
            ndr["PatternCode"] = selectartDr["PatternCode"];
            ndr["PatternDesc"] = selectartDr["PatternDesc"];
            ndr["Location"] = selectartDr["Location"];
            ndr["Parts"] = selectartDr["Parts"];
            ndr["isPair"] = selectartDr["isPair"];

            // Annotation
            DataRow[] adr = this.garmentTb.Select(string.Format("PatternCode='{0}'", selectartDr["PatternCode"]));
            if (adr.Length > 0)
            {
                ndr["annotation"] = adr[0]["annotation"];
            }

            this.allpartTb.Rows.Add(ndr);
            selectartDr.Delete(); // 刪除此筆

            DataRow[] patterndr = this.patternTb.Select(string.Format("PatternCode='{0}'", pattern));
            DataRow[] artdr = this.artTb.Select(string.Format("PatternCode='{0}'", pattern));
            if (patterndr.Length > 0)
            {
                // 刪除後還有相同Pattern 需要判斷是否Subprocess都存在
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

            this.CalculateParts();
        }

        private void Button_RighttoLeft_Click(object sender, EventArgs e)
        {
            this.grid_allpart.ValidateControl();
            this.grid_art.ValidateControl();
            this.grid_qty.ValidateControl();
            if (this.patternTb.Rows.Count == 0 || this.grid_allpart.RowCount == 0)
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
                    ndr2["isMain"] = true;
                    int max = this.patternTb.AsEnumerable().Max(m => MyUtility.Convert.GetInt(m["CombineSubprocessGroup"]));
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
            }
            #endregion
            this.CalculateParts();
        }

        /// <summary>
        /// 計算all part
        /// </summary>
        private void CalculateParts()
        {
            string filter_ALLPARTS = $"CombineSubprocessGroup = 0";
            DataRow[] allpartdr = this.patternTb.Select($"PatternCode='ALLPARTS' and {filter_ALLPARTS}");
            int allpart = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", filter_ALLPARTS));
            if (allpartdr.Length > 0)
            {
                allpartdr[0]["Parts"] = allpart;
            }

            if (this.chkCombineSubprocess.Checked)
            {
                foreach (DataRow dr in this.patternTb.Rows)
                {
                    string fg = $"CombineSubprocessGroup = {dr["CombineSubprocessGroup"]}";
                    dr["Parts"] = MyUtility.Convert.GetInt(this.allpartTb.Compute("Sum(Parts)", fg));
                }
            }

            if (allpartdr.Length == 0 && allpart > 0)
            {
                DataRow drAll = this.patternTb.NewRow();
                drAll["PatternCode"] = "ALLPARTS";
                drAll["PatternDesc"] = "All Parts";
                drAll["Location"] = string.Empty;
                drAll["parts"] = allpart;
                this.patternTb.Rows.Add(drAll);
            }

            this.numTotalParts.Value = MyUtility.Convert.GetInt(this.patternTb.Compute("Sum(Parts)", string.Empty));
        }

        private void SynchronizeMain(int type, string columnName)
        {
            // tpye = 0 左同步到右， type = 1 右同步到左
            DataRow dr = this.grid_art.CurrentDataRow;
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
                        adr["isPair"] = dr["isPair"];
                        dr["isPair"] = false;
                    }
                }

                if (type == 1 && MyUtility.Convert.GetBool(this.grid_allpart.CurrentDataRow["isMain"]))
                {
                    DataRow adr = this.grid_allpart.CurrentDataRow;
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

        private void InsertIntoRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.grid_art.ValidateControl();
            DataRow ndr = this.patternTb.NewRow();
            int max = this.patternTb.AsEnumerable().Max(m => MyUtility.Convert.GetInt(m["CombineSubprocessGroup"]));
            ndr["CombineSubprocessGroup"] = max + 1;
            ndr["isMain"] = true;
            this.patternTb.Rows.Add(ndr);

            if (this.chkCombineSubprocess.Checked)
            {
                DataRow adr = this.allpartTb.NewRow();
                adr["CombineSubprocessGroup"] = max + 1;
                adr["isMain"] = true;
                this.allpartTb.Rows.Add(adr);
            }
        }

        private void NumNoOfBundle_Validating(object sender, CancelEventArgs e)
        {
            int newvalue = (int)this.numNoOfBundle.Value;
            int oldvalue = (int)this.numNoOfBundle.OldValue;
            if (newvalue == oldvalue)
            {
                return;
            }

            if (this.bundle_Detail_Qty_T.Rows.Count == 0)
            {
                for (int i = 0; i < newvalue; i++)
                {
                    DataRow ndr = this.bundle_Detail_Qty_T.NewRow();
                    ndr["Qty"] = 0;
                    this.bundle_Detail_Qty_T.Rows.Add(ndr);
                }

                this.QtyTb_serial();
            }
            else
            {
                if (!MyUtility.Check.Empty(this.maindatarow["cutref"]))
                {
                    int rowindex = this.grid_qty.CurrentRow.Index;
                    string sizeCode = this.grid_qty.Rows[rowindex].Cells["SizeCode"].Value.ToString();

                    if (!MyUtility.Check.Empty(newvalue))
                    {
                        this.bundle_Detail_Qty_T.Clear();
                    }

                    int count = 0;
                    foreach (DataRow dr in this.sizeTb.Rows)
                    {
                        if (count < newvalue)
                        {
                            DataRow ndr = this.bundle_Detail_Qty_T.NewRow();
                            ndr["SizeCode"] = dr["SizeCode"];
                            this.bundle_Detail_Qty_T.Rows.Add(ndr);
                            count++;
                        }
                    }

                    // 如果No of Bundle數量>右上SizeCode數量,就依照左上滑鼠選擇的SizeCode的值複製多出來的數量
                    if (newvalue > count)
                    {
                        for (int i = 0; i < newvalue - count; i++)
                        {
                            DataRow ndr = this.bundle_Detail_Qty_T.NewRow();
                            ndr["SizeCode"] = sizeCode;
                            this.bundle_Detail_Qty_T.Rows.Add(ndr);
                        }
                    }

                    this.QtyTb_serial();
                    this.CalQty();
                }
                else
                {
                    DataTable qtytmp = this.bundle_Detail_Qty_T.Copy();
                    this.bundle_Detail_Qty_T.Clear();
                    int count = 0;
                    foreach (DataRow dr in qtytmp.Rows)
                    {
                        if (count < newvalue)
                        {
                            DataRow ndr = this.bundle_Detail_Qty_T.NewRow();
                            ndr[0] = dr[0];
                            ndr[1] = dr[1];
                            ndr[2] = dr[2];
                            ndr[3] = dr[3];
                            ndr[4] = dr[4];
                            this.bundle_Detail_Qty_T.Rows.Add(ndr);
                            count++;
                        }
                    }

                    // 增加時
                    if (this.numNoOfBundle.OldValue != null)
                    {
                        for (int i = 0; i < newvalue - (int)this.numNoOfBundle.OldValue; i++)
                        {
                            DataRow ndr = this.bundle_Detail_Qty_T.NewRow();
                            ndr["Qty"] = 0;
                            this.bundle_Detail_Qty_T.Rows.Add(ndr);
                        }
                    }

                    this.QtyTb_serial();
                }
            }
        }

        private void DeleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
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

            if (this.chkCombineSubprocess.Checked)
            {
                this.allpartTb.Select($"CombineSubprocessGroup = {this.grid_art.CurrentDataRow["CombineSubprocessGroup"]}").Delete();
            }

            selectartDr.Delete();
            this.CalculateParts();
        }

        private void Allpart_insert_Click(object sender, EventArgs e)
        {
            DataRow ndr = this.allpartTb.NewRow();
            ndr["CombineSubprocessGroup"] = this.chkCombineSubprocess.Checked ? this.grid_art.CurrentDataRow["CombineSubprocessGroup"] : 0;
            ndr["isMain"] = false;
            this.allpartTb.Rows.Add(ndr);
        }

        private void Allpart_delete_Click(object sender, EventArgs e)
        {
            if (this.grid_allpart.Rows.Count == 0)
            {
                return;
            }

            if (this.chkCombineSubprocess.Checked && MyUtility.Convert.GetBool(this.grid_allpart.CurrentDataRow["isMain"]))
            {
                // 刪除右下資料,若點選是 isMain 那筆,則這組全部刪除
                this.allpartTb.Select($"CombineSubprocessGroup = {this.grid_art.CurrentDataRow["CombineSubprocessGroup"]}").Delete();
                this.grid_art.CurrentDataRow.Delete();
            }
            else
            {
                this.grid_allpart.CurrentDataRow.Delete();
            }

            this.CalculateParts();
        }

        private void OK_button_Click(object sender, EventArgs e)
        {
            this.grid_art.ValidateControl();
            if (this.numTone.Value > this.numNoOfBundle.Value)
            {
                MyUtility.Msg.WarningBox("Generate by Tone can not greater than No of Bunde");
                return;
            }

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
            #region 檢查 如果isPair =✔, 加總相同的Cut Part的Parts, 必需>0且可以被2整除
            var samePairCt = this.patternTb.AsEnumerable().Where(w => MyUtility.Convert.GetBool(w["isPair"]))
                .GroupBy(g => new { CutPart = g["PatternCode"] })
                .Select(s => new { s.Key.CutPart, Parts = s.Sum(i => MyUtility.Convert.GetDecimal(i["Parts"])) }).ToList();
            if (samePairCt.Where(w => w.Parts % 2 != 0).Any())
            {
                var mp = samePairCt.Where(w => w.Parts % 2 != 0).ToList();
                string msg = @"The following bundle is pair, but parts is not pair, please check Cut Part parts";
                DataTable dt = ListToDataTable.ToDataTable(mp);
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

            if (this.chkCombineSubprocess.Checked)
            {
                string styleUkey = MyUtility.GetValue.Lookup("Styleukey", this.maindatarow["poid"].ToString(), "Orders", "ID");
                foreach (DataRow drs in this.allpartTb.Select("CombineSubprocessGroup > 0"))
                {
                    DataRow newFtyCombineRow = P10.FtyStyleInnovationCombineSubprocess.NewRow();
                    newFtyCombineRow["MDivisionID"] = Sci.Env.User.Keyword;
                    newFtyCombineRow["StyleUkey"] = styleUkey;
                    newFtyCombineRow["FabricCombo"] = this.maindatarow["PatternPanel"];
                    newFtyCombineRow["Article"] = this.maindatarow["Article"];
                    newFtyCombineRow["PatternCode"] = drs["PatternCode"];
                    newFtyCombineRow["PatternDesc"] = drs["PatternDesc"];
                    newFtyCombineRow["Location"] = drs["Location"];
                    newFtyCombineRow["Parts"] = drs["Parts"];
                    newFtyCombineRow["IsPair"] = drs["IsPair"];
                    newFtyCombineRow["IsMain"] = drs["IsMain"];
                    newFtyCombineRow["CombineSubprocessGroup"] = drs["CombineSubprocessGroup"];
                    P10.FtyStyleInnovationCombineSubprocess.Rows.Add(newFtyCombineRow);
                }
            }

            DataTable bundle_detail_tmp = this.bundle_Detail_T.Clone();
            int bundlegroup = Convert.ToInt32(this.maindatarow["startno"]);
            int printGroup = 1;
            int ukey = 1;
            this.grid_qty.ValidateControl();
            foreach (DataRow dr in this.bundle_Detail_Qty_T.Rows)
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
                        nDetail["Location"] = MyUtility.Convert.GetString(dr2["Location"]);
                        nDetail["Parts"] = dr2["Parts"];
                        nDetail["Qty"] = dr["Qty"];
                        nDetail["SizeCode"] = dr["SizeCode"];
                        nDetail["bundlegroup"] = bundlegroup;
                        nDetail["printGroup"] = printGroup;
                        nDetail["CombineSubprocessGroup"] = dr2["CombineSubprocessGroup"];
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
                    printGroup++;
                }
            }

            this.bundle_Detail_allpart_T.Clear();
            this.bundle_Detail_Art_T.Clear();
            this.bundle_Detail_CombineSubprocess_T.Clear();
            bundle_detail_tmp.Columns.Add("ran", type: typeof(int));
            bundle_detail_tmp.AsEnumerable().ToList().ForEach(r => r["ran"] = 0);

            // 平行覆蓋資料
            int j = 0;
            int detailRow = 0;
            int tmpRow = bundle_detail_tmp.Rows.Count;
            bool notYetInsertAllPart = true;
            foreach (DataRow dr in this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
            {
                if (j < tmpRow)
                {
                    DataRow[] oridrs = bundle_detail_tmp.Select($"PatternCode = '{dr["PatternCode"]}' and sizecode = '{dr["sizecode"]}' and Qty = '{dr["Qty"]}' and ran = 0");
                    if (oridrs.Length == 0)
                    {
                        oridrs = bundle_detail_tmp.Select($"PatternCode = '{dr["PatternCode"]}' and sizecode = '{dr["sizecode"]}' and ran = 0");
                    }

                    DataRow tmpdr = bundle_detail_tmp.Select("ran = 0").OrderBy(o => MyUtility.Convert.GetLong(o["Ukey1"])).First();
                    if (oridrs.Length > 0)
                    {
                        tmpdr = oridrs.OrderBy(o => MyUtility.Convert.GetLong(o["Ukey1"])).First();
                    }

                    tmpdr["ran"] = 1;

                    dr["bundlegroup"] = tmpdr["bundlegroup"];
                    dr["printGroup"] = tmpdr["printGroup"];
                    dr["PatternCode"] = tmpdr["PatternCode"];
                    dr["PatternDesc"] = tmpdr["PatternDesc"];
                    dr["Location"] = MyUtility.Convert.GetString(tmpdr["Location"]);
                    dr["subprocessid"] = tmpdr["subprocessid"];
                    dr["NoBundleCardAfterSubprocess_String"] = tmpdr["NoBundleCardAfterSubprocess_String"];
                    dr["PostSewingSubProcess_String"] = tmpdr["PostSewingSubProcess_String"];
                    dr["Parts"] = tmpdr["Parts"];
                    dr["Qty"] = tmpdr["Qty"];
                    dr["SizeCode"] = tmpdr["SizeCode"];
                    dr["ukey1"] = tmpdr["ukey1"];
                    dr["isPair"] = tmpdr["isPair"];
                    dr["CombineSubprocessGroup"] = tmpdr["CombineSubprocessGroup"];

                    j++;

                    if (tmpdr["PatternCode"].ToString() == "ALLPARTS" && notYetInsertAllPart)
                    {
                        string fa = string.Empty;
                        if (this.chkCombineSubprocess.Checked)
                        {
                            fa = "CombineSubprocessGroup = 0";
                        }

                        foreach (DataRow aldr in this.allpartTb.Select(fa))
                        {
                            if (aldr.RowState == DataRowState.Deleted)
                            {
                                continue;
                            }

                            if (MyUtility.Check.Empty(aldr["Parts"]))
                            {
                                continue;
                            }

                            DataRow allpart_ndr = this.bundle_Detail_allpart_T.NewRow();
                            allpart_ndr["PatternCode"] = aldr["PatternCode"];
                            allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                            allpart_ndr["Location"] = MyUtility.Convert.GetString(aldr["Location"]);
                            allpart_ndr["Parts"] = aldr["Parts"];
                            allpart_ndr["ukey1"] = dr["ukey1"];
                            allpart_ndr["isPair"] = aldr["isPair"];
                            this.bundle_Detail_allpart_T.Rows.Add(allpart_ndr);
                        }

                        notYetInsertAllPart = false;
                    }
                    else
                    {
                        // bundle_Detail_Art_T
                        DataRow art_ndr = this.bundle_Detail_Art_T.NewRow();
                        art_ndr["Bundleno"] = dr["Bundleno"];
                        art_ndr["PatternCode"] = dr["PatternCode"];
                        art_ndr["Subprocessid"] = dr["subprocessid"];
                        art_ndr["NoBundleCardAfterSubprocess_String"] = dr["NoBundleCardAfterSubprocess_String"];
                        art_ndr["PostSewingSubProcess_String"] = dr["PostSewingSubProcess_String"];
                        art_ndr["ukey1"] = dr["ukey1"];
                        this.bundle_Detail_Art_T.Rows.Add(art_ndr);

                        // bundle_Detail_CombineSubprocess_T
                        if (this.chkCombineSubprocess.Checked)
                        {
                            foreach (DataRow aldr in this.allpartTb.Select($"CombineSubprocessGroup > 0 and CombineSubprocessGroup = '{tmpdr["CombineSubprocessGroup"]}'"))
                            {
                                DataRow newdr = this.bundle_Detail_CombineSubprocess_T.NewRow();
                                newdr["Bundleno"] = dr["Bundleno"];
                                newdr["PatternCode"] = aldr["PatternCode"];
                                newdr["PatternDesc"] = aldr["PatternDesc"];
                                newdr["Parts"] = aldr["Parts"];
                                newdr["Location"] = MyUtility.Convert.GetString(aldr["Location"]);
                                newdr["IsPair"] = aldr["IsPair"];
                                newdr["IsMain"] = aldr["IsMain"];
                                newdr["ukey1"] = tmpdr["ukey1"];
                                this.bundle_Detail_CombineSubprocess_T.Rows.Add(newdr);
                            }
                        }
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
            if (this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().Count() > 0)
            {
                dtCount = this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).CopyToDataTable();
            }
            else
            {
                dtCount = this.bundle_Detail_T.Clone();
            }

            dtCount.AcceptChanges();
            int detailrow = this.bundle_Detail_T.Rows.Count;
            int deleteCnt = dtCount.Rows.Count - tmpRow;
            for (int i = 1; i <= deleteCnt; i++)
            {
                this.bundle_Detail_T.Rows[detailrow - i].Delete();
            }

            // 表示新增的比較多需要Insert
            bundle_detail_tmp.AsEnumerable().ToList().ForEach(f => f["ran"] = 0);
            foreach (DataRow dr in this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
            {
                DataRow[] oridrs = bundle_detail_tmp.Select($"PatternCode = '{dr["PatternCode"]}' and sizecode = '{dr["sizecode"]}' and Qty = '{dr["Qty"]}' and ran = 0");
                if (oridrs.Length == 0)
                {
                    oridrs = bundle_detail_tmp.Select($"PatternCode = '{dr["PatternCode"]}' and sizecode = '{dr["sizecode"]}' and ran = 0");
                }

                DataRow tmpdr = bundle_detail_tmp.Select("ran = 0").OrderBy(o => MyUtility.Convert.GetLong(o["Ukey1"])).First();
                if (oridrs.Length > 0)
                {
                    tmpdr = oridrs.OrderBy(o => MyUtility.Convert.GetLong(o["Ukey1"])).First();
                }

                tmpdr["ran"] = 1;
            }

            if (tmpRow > j)
            {
                for (int i = 0; i < tmpRow - j; i++)
                {
                    DataRow ndr = this.bundle_Detail_T.NewRow();
                    DataRow tmpdr = bundle_detail_tmp.Select("ran = 0")[0];
                    tmpdr["ran"] = 1;
                    ndr["bundlegroup"] = tmpdr["bundlegroup"];
                    ndr["printGroup"] = tmpdr["printGroup"];
                    ndr["PatternCode"] = tmpdr["PatternCode"];
                    ndr["PatternDesc"] = tmpdr["PatternDesc"];
                    ndr["subprocessid"] = tmpdr["subprocessid"];
                    ndr["NoBundleCardAfterSubprocess_String"] = tmpdr["NoBundleCardAfterSubprocess_String"];
                    ndr["PostSewingSubProcess_String"] = tmpdr["PostSewingSubProcess_String"];
                    ndr["Location"] = MyUtility.Convert.GetString(tmpdr["Location"]);
                    ndr["Parts"] = tmpdr["Parts"];
                    ndr["Qty"] = tmpdr["Qty"];
                    ndr["SizeCode"] = tmpdr["SizeCode"];
                    ndr["ukey1"] = tmpdr["ukey1"];
                    ndr["isPair"] = tmpdr["isPair"];
                    ndr["CombineSubprocessGroup"] = tmpdr["CombineSubprocessGroup"];

                    this.bundle_Detail_T.Rows.Add(ndr);
                    if (tmpdr["PatternCode"].ToString() == "ALLPARTS" && notYetInsertAllPart)
                    {
                        string fa = string.Empty;
                        if (this.chkCombineSubprocess.Checked)
                        {
                            fa = "CombineSubprocessGroup = 0";
                        }

                        foreach (DataRow aldr in this.allpartTb.Select(fa))
                        {
                            if (aldr.RowState == DataRowState.Deleted)
                            {
                                continue;
                            }

                            if (MyUtility.Check.Empty(aldr["Parts"]))
                            {
                                continue;
                            }

                            DataRow allpart_ndr = this.bundle_Detail_allpart_T.NewRow();
                            allpart_ndr["PatternCode"] = aldr["PatternCode"];
                            allpart_ndr["PatternDesc"] = aldr["PatternDesc"];
                            allpart_ndr["Location"] = MyUtility.Convert.GetString(aldr["Location"]);
                            allpart_ndr["Parts"] = aldr["Parts"];
                            allpart_ndr["ukey1"] = tmpdr["ukey1"];
                            allpart_ndr["isPair"] = aldr["isPair"];
                            this.bundle_Detail_allpart_T.Rows.Add(allpart_ndr);
                        }

                        notYetInsertAllPart = false;
                    }
                    else
                    {
                        // bundle_Detail_Art_T
                        DataRow art_ndr = this.bundle_Detail_Art_T.NewRow();
                        art_ndr["PatternCode"] = tmpdr["PatternCode"];
                        art_ndr["Subprocessid"] = tmpdr["subprocessid"];
                        art_ndr["NoBundleCardAfterSubprocess_String"] = tmpdr["NoBundleCardAfterSubprocess_String"];
                        art_ndr["PostSewingSubProcess_String"] = tmpdr["PostSewingSubProcess_String"];
                        art_ndr["ukey1"] = tmpdr["ukey1"];
                        this.bundle_Detail_Art_T.Rows.Add(art_ndr);

                        // bundle_Detail_CombineSubprocess_T
                        if (this.chkCombineSubprocess.Checked)
                        {
                            foreach (DataRow aldr in this.allpartTb.Select($"CombineSubprocessGroup > 0 and CombineSubprocessGroup = '{tmpdr["CombineSubprocessGroup"]}'"))
                            {
                                DataRow newdr = this.bundle_Detail_CombineSubprocess_T.NewRow();
                                newdr["PatternCode"] = aldr["PatternCode"];
                                newdr["PatternDesc"] = aldr["PatternDesc"];
                                newdr["Parts"] = aldr["Parts"];
                                newdr["Location"] = MyUtility.Convert.GetString(aldr["Location"]);
                                newdr["IsPair"] = aldr["IsPair"];
                                newdr["IsMain"] = aldr["IsMain"];
                                newdr["ukey1"] = tmpdr["ukey1"];
                                this.bundle_Detail_CombineSubprocess_T.Rows.Add(newdr);
                            }
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
            DataTable allpartTb_Copy = this.allpartTb.Copy();
            allpartTb_Copy.AcceptChanges();
            if (!MyUtility.Check.Empty(allpartTb_Copy) && allpartTb_Copy.Rows.Count > 0)
            {
                decimal parts = 0;
                for (int i = 0; i < allpartTb_Copy.Rows.Count; i++)
                {
                    parts += MyUtility.Convert.GetDecimal(allpartTb_Copy.Rows[i]["Parts"]);
                }

                DataRow[] allPart = this.bundle_Detail_T.Select("PatternCode='ALLPARTS'");
                if (!MyUtility.Check.Empty(parts))
                {
                    if (allPart.Length == 0)
                    {
                        DataTable dtMax = this.bundle_Detail_T.Copy();
                        dtMax.AcceptChanges();
                        DataView view = new DataView(dtMax);
                        DataTable dtAllPart = view.ToTable(true, "BundleGroup", "printGroup", "SizeCode", "qty");
                        if (dtAllPart.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAllPart.Rows.Count; i++)
                            {
                                List<int> ukey1 = dtMax.AsEnumerable().Select(numb => numb.Field<int>("ukey1")).Distinct().ToList();
                                int maxUkey = ukey1.Max();
                                DataRow drAll = this.bundle_Detail_T.NewRow();
                                drAll["PatternCode"] = "ALLPARTS";
                                drAll["PatternDesc"] = "All Parts";
                                drAll["Qty"] = dtAllPart.Rows[i]["qty"].ToString();
                                drAll["SizeCode"] = dtAllPart.Rows[i]["SizeCode"].ToString();
                                drAll["parts"] = parts;
                                drAll["BundleGroup"] = dtAllPart.Rows[i]["BundleGroup"].ToString();
                                drAll["printGroup"] = dtAllPart.Rows[i]["printGroup"].ToString();
                                drAll["ukey1"] = maxUkey + 1;
                                this.bundle_Detail_T.Rows.Add(drAll);
                            }
                        }
                        else if (this.bundle_Detail != null && this.bundle_Detail.Rows.Count > 0)
                        {
                            DataView view2 = new DataView(this.bundle_Detail);
                            dtAllPart = view2.ToTable(true, "BundleGroup", "printGroup", "SizeCode", "qty");
                            DataRow drAll = this.bundle_Detail_T.NewRow();
                            drAll["PatternCode"] = "ALLPARTS";
                            drAll["PatternDesc"] = "All Parts";
                            drAll["Qty"] = dtAllPart.Rows[0]["qty"].ToString();
                            drAll["SizeCode"] = dtAllPart.Rows[0]["SizeCode"].ToString();
                            drAll["parts"] = parts;
                            drAll["BundleGroup"] = dtAllPart.Rows[0]["BundleGroup"].ToString();
                            drAll["printGroup"] = dtAllPart.Rows[0]["printGroup"].ToString();
                            drAll["ukey1"] = 1;
                            this.bundle_Detail_T.Rows.Add(drAll);
                        }
                    }
                }
            }
            #endregion

            this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().ForEach(f => f["Tone"] = string.Empty);

            #region Generate by Tone 有勾選再處理一次
            if (this.chkTone.Checked && this.numTone.Value > 0 && this.numNoOfBundle.Value > 0)
            {
                int seq = 0;
                this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().ForEach(f => f["tmpSeq"] = seq++);
                int bundlegroupS = Convert.ToInt32(this.maindatarow["startno"]);
                int tone = MyUtility.Convert.GetInt(this.numTone.Value);
                DataTable dtDetail = new DataTable();
                DataTable dtAllPart = new DataTable();
                DataTable dtAllPart2 = this.bundle_Detail_T.Clone();
                DataTable dtArt = this.bundle_Detail_Art_T.Copy();
                this.bundle_Detail_Art_T.Clear();
                DataTable dtCombine = this.bundle_Detail_CombineSubprocess_T.Copy();
                this.bundle_Detail_CombineSubprocess_T.Clear();

                int na = this.bundle_Detail_T.Select("PatternCode <> 'AllParts'").Length;
                int a = this.bundle_Detail_T.Select("PatternCode = 'AllParts'").Length;
                if (na > 0)
                {
                    dtDetail = this.bundle_Detail_T.Select("PatternCode <> 'AllParts'").OrderBy(o => MyUtility.Convert.GetLong(o["tmpSeq"])).CopyToDataTable();
                    dtDetail.Columns.Add("tmpNum", typeof(int));
                }

                if (a > 0)
                {
                    dtAllPart = this.bundle_Detail_T.Select("PatternCode = 'AllParts'").CopyToDataTable();
                }

                this.bundle_Detail_T.Clear();

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
                            item["PrintGroup"] = MyUtility.Convert.GetInt(item["PrintGroup"]) + (i * (int)this.numNoOfBundle.Value);
                            item["Tone"] = MyUtility.Excel.ConvertNumericToExcelColumn(i + 1);
                            item["tmpNum"] = tmpNum; // 暫時紀錄原本資料對應拆出去的資料,要用來重分配Qty
                            tmpNum++;

                            DataTable dtCopyArt = dtArt.Copy();
                            string fukey = $"Ukey1 = {item["Ukey1"]}";
                            DataRow artdr = dtCopyArt.Select(fukey)[0];
                            artdr["BundleNo"] = string.Empty;
                            artdr["Ukey1"] = ukeytone;
                            item["Ukey1"] = ukeytone;
                            this.bundle_Detail_Art_T.ImportRow(artdr);
                            if (this.chkCombineSubprocess.Checked)
                            {
                                DataTable dtCopyCombine = dtCombine.Copy();
                                DataRow combinedr = dtCopyCombine.Select(fukey)[0];
                                combinedr["BundleNo"] = string.Empty;
                                combinedr["Ukey1"] = ukeytone;
                                this.bundle_Detail_CombineSubprocess_T.ImportRow(combinedr);
                            }

                            ukeytone++;
                        }

                        this.bundle_Detail_T.Merge(dtCopy);
                    }

                    // 重分每一筆拆的Qty
                    int tmpNumF = 0;
                    for (int i = 0; i < this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Count() / tone; i++, tmpNumF++)
                    {
                        DataRow[] drD = this.bundle_Detail_T.Select($"tmpNum={tmpNumF}").AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToArray();
                        Prgs.AverageNumeric(drD, "Qty", MyUtility.Convert.GetInt(drD[0]["Qty"]), true);
                    }

                    this.bundle_Detail_T.Columns.Remove("tmpNum");
                }

                // 處理All Part筆數
                if (a > 0)
                {
                    int ttlAllPartQty = MyUtility.Convert.GetInt(this.displayTotalQty.Value);
                    int allpartPrintGroup = dtAllPart.AsEnumerable().Max(m => MyUtility.Convert.GetInt(m["PrintGroup"]));
                    DataRow row = dtAllPart.Rows[0];
                    for (int i = 0; i < tone; i++)
                    {
                        row["BundleGroup"] = bundlegroupS + i;
                        row["PrintGroup"] = (allpartPrintGroup * (i + 1)) - ((int)this.numNoOfBundle.Value - 1);
                        row["Tone"] = MyUtility.Excel.ConvertNumericToExcelColumn(i + 1);
                        int notAllpart = this.patternTb.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).ToList().Count() - 1;
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
                            int qty = this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted &&
                               MyUtility.Convert.GetInt(w["PrintGroup"]) >= MyUtility.Convert.GetInt(da[i]["PrintGroup"])).
                               Sum(s => MyUtility.Convert.GetInt(s["Qty"])) / this.patternTb.Select($"PatternCode <> 'Allparts'").Count();
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

                    this.bundle_Detail_T.Merge(dtAllPart2);
                }

                foreach (DataRow dr in this.bundle_Detail_T.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
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

            this.bundle_Detail_Art_T.Select("PatternCode = 'ALLPARTS'").Delete();

            #region 把處理好的資料塞回上層Table
            this.bundle_Detail.Clear();
            this.bundle_Detail_allpart.Clear();
            this.bundle_Detail_Qty.Clear();
            this.bundle_Detail_Art.Clear();
            this.bundle_Detail_CombineSubprocess.Clear();

            this.maindatarow["Qty"] = this.numNoOfBundle.Value;
            this.bundle_Detail.Merge(this.bundle_Detail_T);
            this.bundle_Detail_allpart.Merge(this.bundle_Detail_allpart_T);
            this.bundle_Detail_Qty.Merge(this.bundle_Detail_Qty_T);
            this.bundle_Detail_Art.Merge(this.bundle_Detail_Art_T);
            this.bundle_Detail_CombineSubprocess.Merge(this.bundle_Detail_CombineSubprocess_T);

            this.maindatarow["ByToneGenerate"] = this.chkTone.Checked;
            #endregion

            this.Close();
        }

        private void BtnGarment_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.maindatarow["poid"].ToString(), "Orders", "ID");
            var sizelist = ((DataTable)this.listControlBindingSource1.DataSource).AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            PublicForm.GarmentList callNextForm = new PublicForm.GarmentList(ukey, this.maindatarow["poid"].ToString(), this.maindatarow["cutref"].ToString(), sizelist);
            callNextForm.ShowDialog(this);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void P10_Generate_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
            this.Dispose();
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

            // 兩個陣列內容要完全一樣，不管順序
            if (!Prgs.CompareArr(ann, anns))
            {
                dr["NoBundleCardAfterSubprocess_String"] = string.Empty;
                dr.EndEdit();
                return;
            }

            List<string> notMainList = this.GetNotMain(dr1, this.garmentTb.Select()); // 帶入未去除數字的annotation資料
            string noBundleCardAfterSubprocess_String = string.Join("+", notMainList);
            dr["NoBundleCardAfterSubprocess_String"] = noBundleCardAfterSubprocess_String;
            dr.EndEdit();
        }

        private void ChkCombineSubprocess_CheckedChanged(object sender, EventArgs e)
        {
            if (this.bundle_Detail_T.Rows.Count > 0 && this.bundle_Detail_CombineSubprocess_T.Rows.Count == 0 && this.chkCombineSubprocess.Checked)
            {
                DialogResult result = MyUtility.Msg.QuestionBox("Warning: This bundle doesn't include combine subprocess. The bundle information will follow pattern room.");
                if (result == DialogResult.No)
                {
                    this.chkCombineSubprocess.Checked = false;
                    return;
                }
            }

            this.button_LefttoRight.Enabled = !this.chkCombineSubprocess.Checked;
            this.grid_allpart.Columns["Annotation"].Visible = !this.chkCombineSubprocess.Checked;
            this.grid_art.Columns["isPair"].Visible = !this.chkCombineSubprocess.Checked;
            this.label5.Text = this.chkCombineSubprocess.Checked ? "Combine Subprocess Detail" : "All Parts Detail";

            this.ChangeDefault();
        }

        private void ChkNoneShellNoCreateAllParts_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeDefault();
            this.DeleteAllpartsDatas();
        }

        private void DeleteAllpartsDatas()
        {
            if (this.chkNoneShellNoCreateAllParts.Checked)
            {
                this.allpartTb.Select($"CombineSubprocessGroup = 0").Delete();
                this.allpartTb.AcceptChanges();
                this.CalculateParts();
            }
        }

        private void ChangeDefault()
        {
            this.patternTb.Clear();
            this.allpartTb.Clear();
            DataTable pdt = this.patternTb.Clone();
            DataTable xdt = this.patternTbOri.Copy();
            DataTable adt = this.allpartTbOri.Copy();

            if (!this.chkCombineSubprocess.Checked)
            {
                pdt = xdt;
            }
            else
            {
                pdt = xdt.Select($"isMain = 1").TryCopyToDataTable(this.patternTb);
                pdt.AsEnumerable().ToList().ForEach(f => f["isPair"] = false);
                pdt.ImportRow(xdt.Select($"PatternCode = 'ALLPARTS'").FirstOrDefault());

                DataTable psdt = xdt.Select($"PatternCode <> 'ALLPARTS'").TryCopyToDataTable(this.patternTb);
                psdt.AsEnumerable().ToList().ForEach(f => this.allpartTb.ImportRow(f));
            }

            this.patternTb.Merge(pdt);
            this.allpartTb.Merge(adt);
            this.CalculateParts();
        }

        private void Grid_art_SelectionChanged(object sender, EventArgs e)
        {
            this.allpartTb.DefaultView.RowFilter = string.Empty;
            if (this.grid_art.CurrentDataRow == null || !this.chkCombineSubprocess.Checked)
            {
                return;
            }

            if (this.chkCombineSubprocess.Checked)
            {
                string filter = $"CombineSubprocessGroup = {this.grid_art.CurrentDataRow["CombineSubprocessGroup"]}";
                this.allpartTb.DefaultView.RowFilter = filter;
            }
        }
    }
}
