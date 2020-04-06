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
using System.Transactions;
using Sci.Production.PublicPrg;

namespace Sci.Production.Cutting
{
    public partial class P10_1 : Sci.Win.Tems.Input6
    {
        string keyword = Sci.Env.User.Keyword;
        string LoginId = Sci.Env.User.UserID;
        DataTable bundle_Detail_allpart_Tb, bundle_Detail_Art_Tb, bundle_Detail_Qty_Tb;
        string WorkOrder_Ukey;

        public P10_1(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();

            this.DefaultWhere = $@"(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = BundleReplacement.Orderid)  = '{Sci.Env.User.Factory}'";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory WITH (NOLOCK)
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            // 取得當前登入工廠index
            for (int i = 0; i < queryDT.Rows.Count; i++)
            {   
                if (String.Compare(queryDT.Rows[i]["FTYGroup"].ToString(), Sci.Env.User.Factory) == 0)
                {
                    queryfors.SelectedIndex = i;
                }
            }
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = BundleReplacement.Orderid)  = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("BundleGroup", header: "Group", width: Widths.AnsiChars(4), integer_places: 5, iseditingreadonly: true)
            .Text("Bundleno", header: "Bundle No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("PatternCode", header: "Cutpart", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("PatternDesc", header: "Cutpart name", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("subProcessid", header: "SubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Parts", header: "Parts", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(6), integer_places: 5, iseditingreadonly: true)
            .CheckBox("IsPair", header: "IsPair", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0)
            .Text("PostSewingSubProcess_String", header: "Post Sewing\r\nSubProcess", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("NoBundleCardAfterSubprocess_String", header: "No Bundle Card\r\nAfter Subprocess", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;
            return base.OnGridSetup();
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            #region 主 Detail
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(@"
Select a.*	,s.subProcessid, ps.NoBundleCardAfterSubprocess_String, nbs.PostSewingSubProcess_String, ukey1 = 0
From BundleReplacement_Detail a WITH (NOLOCK) 
outer apply
(
	select subProcessid =
	stuff((
		Select concat('+',Subprocessid)
		From BundleReplacement_Detail_art c WITH (NOLOCK) 
		Where c.bundleno =a.bundleno and c.id = a.id 
		Order by Subprocessid
		For XML path('')
	),1,1,'')
) as s
outer apply
(
	select NoBundleCardAfterSubprocess_String =
	stuff((
		Select concat('+',Subprocessid)
		From BundleReplacement_Detail_art c WITH (NOLOCK) 
		Where c.bundleno =a.bundleno and c.id = a.id and c.NoBundleCardAfterSubprocess = 1
		Order by Subprocessid
		For XML path('')
	),1,1,'')
) as ps
outer apply
(
	select PostSewingSubProcess_String =
	stuff((
		Select concat('+',Subprocessid)
		From BundleReplacement_Detail_art c WITH (NOLOCK) 
		Where c.bundleno =a.bundleno and c.id = a.id and c.PostSewingSubProcess = 1
		Order by Subprocessid
		For XML path('')
	),1,1,'')
) as nbs
where a.id = '{0}' 
order by bundlegroup"
                , masterID);
            this.DetailSelectCommand = cmdsql;
            #endregion

            #region 先撈出底層其他Table
            if (!IsDetailInsertByCopy)
            {
                string allPart_cmd = string.Format(@"Select sel = 0,*, ukey1 = 0, annotation = '' from  BundleReplacement_Detail_Allpart  WITH (NOLOCK)  Where id ='{0}'", masterID);
                string art_cmd = string.Format(@"Select b.*, ukey1 = 0,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from BundleReplacement_Detail a WITH (NOLOCK) inner join BundleReplacement_Detail_art b WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id Where a.id ='{0}' ", masterID);
                string qty_cmd = string.Format(@"Select No = 0, a.* from BundleReplacement_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
                DualResult dRes = null;
                dRes = DBProxy.Current.Select(null, allPart_cmd, out bundle_Detail_allpart_Tb);
                if (!dRes)
                {
                    ShowErr(allPart_cmd, dRes);
                    return dRes;
                }
                dRes = DBProxy.Current.Select(null, art_cmd, out bundle_Detail_Art_Tb);
                if (!dRes)
                {
                    ShowErr(art_cmd, dRes);
                    return dRes;
                }
                dRes = DBProxy.Current.Select(null, qty_cmd, out bundle_Detail_Qty_Tb);
                if (!dRes)
                {
                    ShowErr(qty_cmd, dRes);
                    return dRes;
                }
            }
            #endregion

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (CurrentMaintain == null) return;
            queryTable();
            string orderid = CurrentMaintain["OrderID"].ToString();
            string cuttingid = "";
            DataTable orders;
            if (DBProxy.Current.Select(null, string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", orderid), out orders))
            {
                if (orders.Rows.Count != 0)
                {
                    displaySeason.Text = orders.Rows[0]["Seasonid"].ToString();
                    displayStyle.Text = orders.Rows[0]["Styleid"].ToString();
                    cuttingid = orders.Rows[0]["cuttingsp"].ToString();
                }
                else
                {
                    displaySeason.Text = "";
                    displayStyle.Text = "";
                }
            }
            else
            {
                displaySeason.Text = "";
                displayStyle.Text = "";
            }

            int qty = 0;
            if (bundle_Detail_Qty_Tb.Rows.Count == 0) qty = 0;
            else qty = Convert.ToInt16(bundle_Detail_Qty_Tb.Compute("Sum(Qty)", ""));

            numQtyperBundleGroup.Value = qty;

            string factoryid = MyUtility.GetValue.Lookup(string.Format("SELECT o.FactoryID FROM BundleReplacement b WITH (NOLOCK) inner join orders o WITH (NOLOCK) on b.Orderid=o.ID where b.Orderid='{0}'", orderid), null);

            //txtsewingline1.factoryobjectName = (Control)factoryid;

            if (!MyUtility.Check.Empty(this.CurrentMaintain["printdate"]))
            {
                DateTime? lastTime = (DateTime?)this.CurrentMaintain["printdate"];
                string FtyLastupdate = lastTime == null ? "" : ((DateTime)lastTime).ToString("yyyy/MM/dd HH:mm:ss");
                this.displayPrintDate.Text = FtyLastupdate;
            }
            else
            {
                this.displayPrintDate.Text = "";
            }
        }

        public void queryTable()
        {
            string masterID = (CurrentMaintain == null) ? "" : CurrentMaintain["id"].ToString();
            string allPart_cmd = string.Format(@"Select 0 as sel,*, 0 as ukey1,'' as annotation from BundleReplacement_Detail_Allpart  WITH (NOLOCK) Where id ='{0}' ", masterID);
            string art_cmd = string.Format(@"Select b.*, 0 as ukey1,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from BundleReplacement_Detail a WITH (NOLOCK) ,BundleReplacement_Detail_art b WITH (NOLOCK) Where a.id ='{0}' and a.Bundleno = b.bundleno and a.id = b.id", masterID);
            string qty_cmd = string.Format(@"Select 0 as No,a.* from BundleReplacement_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out bundle_Detail_allpart_Tb);
            if (!dRes)
            {
                ShowErr(allPart_cmd, dRes);
            }
            dRes = DBProxy.Current.Select(null, art_cmd, out bundle_Detail_Art_Tb);
            if (!dRes)
            {
                ShowErr(art_cmd, dRes);

            }
            dRes = DBProxy.Current.Select(null, qty_cmd, out bundle_Detail_Qty_Tb);
            if (!dRes)
            {
                ShowErr(qty_cmd, dRes);

            }
            int ukey = 1;
            foreach (DataRow dr in DetailDatas)
            {
                dr["ukey1"] = ukey;
                DataRow[] allar = bundle_Detail_allpart_Tb.Select(string.Format("id='{0}'", dr["ID"]));
                if (allar.Length > 0)
                {
                    foreach (DataRow dr2 in allar)
                    {
                        dr2["ukey1"] = ukey;
                    }
                }
                DataRow[] Artar = bundle_Detail_Art_Tb.Select(string.Format("Bundleno='{0}'", dr["Bundleno"]));
                if (allar.Length > 0)
                {
                    foreach (DataRow dr2 in Artar)
                    {
                        dr2["ukey1"] = ukey;
                    }
                }
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Cdate"] = DateTime.Today;
            CurrentMaintain["mDivisionid"] = keyword;
            displayPrintDate.Text = "";
            bundle_Detail_allpart_Tb.Clear();
            bundle_Detail_Art_Tb.Clear();
            bundle_Detail_Qty_Tb.Clear();

        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("<SP#> can not be empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["PatternPanel"]))
            {
                MyUtility.Msg.WarningBox("<Pattern Panel> can not be empty!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["sewinglineid"]))
            {
                MyUtility.Msg.WarningBox("<Line> can not be empty!");
                return false;
            }

            if (IsDetailInserting)
            {
                string Keyword = keyword + "RP";
                string cid = MyUtility.GetValue.GetID(Keyword, "BundleReplacement", Convert.ToDateTime(CurrentMaintain["cdate"]), sequenceMode: 2);
                if (string.IsNullOrWhiteSpace(cid))
                {
                    return false;
                }
                CurrentMaintain["ID"] = cid;
                foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows)
                {
                    dr["ID"] = cid;
                }
                foreach (DataRow dr in bundle_Detail_Art_Tb.Rows)
                {
                    dr["ID"] = cid;
                }
                foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows)
                {
                    dr["ID"] = cid;
                }
            }

            return base.ClickSaveBefore();
        }
        protected override DualResult ClickSavePre()
        {
            #region 填入Bundleno
            int drcount = DetailDatas.Count;
            IList<string> cListBundleno;
            cListBundleno = MyUtility.GetValue.GetBatchID("R", "BundleReplacement_Detail", MyUtility.Check.Empty(CurrentMaintain["cDate"])? default(DateTime) : Convert.ToDateTime(CurrentMaintain["cDate"]), 3, "Bundleno", batchNumber: drcount, sequenceMode: 2);
            if (cListBundleno.Count == 0)
            {
                return new DualResult(false, "Create Bundleno error.");
            }
            int nCount = 0;

            DataRow[] roway;
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Bundleno"]) && dr.RowState != DataRowState.Deleted)
                {
                    dr["Bundleno"] = cListBundleno[nCount];

                    roway = bundle_Detail_Art_Tb.Select(string.Format("ukey1 = {0}", dr["ukey1"]));
                    foreach (DataRow dr2 in roway)
                    {
                        dr2["Bundleno"] = cListBundleno[nCount];
                    }
                    nCount++;

                }
            }
            #endregion
            //DataTable dt = (DataTable)detailgridbs.DataSource;
            return base.ClickSavePre();
        }
        protected override DualResult ClickSavePost()
        {

            string allpart_cmd = "", Art_cmd = "", Qty_cmd = "";
            #region 先撈出實體Table 為了平行判斷筆數 DataTable allparttmp, arttmp, qtytmp
            DataTable allparttmp, arttmp, qtytmp;
            string masterID = (CurrentMaintain == null) ? "" : CurrentMaintain["id"].ToString();

            string allPart_cmd = string.Format(@"select * from BundleReplacement_Detail_Allpart WITH (NOLOCK) where id='{0}'  ", masterID);
            string art_cmd = string.Format(@"Select b.* from BundleReplacement_Detail_art b WITH (NOLOCK) left join BundleReplacement_Detail a WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id where b.id='{0}'", masterID);
            string qty_cmd = string.Format(@"Select a.* from BundleReplacement_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out allparttmp);
            if (!dRes)
            {
                ShowErr(allPart_cmd, dRes);
            }
            dRes = DBProxy.Current.Select(null, art_cmd, out arttmp);
            if (!dRes)
            {
                ShowErr(art_cmd, dRes);

            }
            dRes = DBProxy.Current.Select(null, qty_cmd, out qtytmp);
            if (!dRes)
            {
                ShowErr(qty_cmd, dRes);

            }
            #endregion
            //int row = 0;
            #region 處理Bundle_Detail_AllPart 修改版

            /*
             * 先刪除原有資料
             * 再新增更改的資料             
             */
            allpart_cmd = allpart_cmd + string.Format(@"Delete from BundleReplacement_Detail_allpart where id ='{0}'", masterID);
            foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows) //處理Bundle_Detail_AllPart
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    allpart_cmd = allpart_cmd + string.Format(
                @"insert into BundleReplacement_Detail_allpart(ID,PatternCode,PatternDesc,Parts,isPair,Location) values('{0}','{1}','{2}','{3}','{4}','{5}');"
                , CurrentMaintain["ID"], dr["PatternCode"], dr["PatternDesc"], dr["Parts"], dr["isPair"], dr["Location"]);
                }
            }
            #endregion
            
            #region 處理Bundle_Detail_Art 修改版
            /*
            * 先刪除原有資料
            * 再新增更改的資料             
            */
            Art_cmd = Art_cmd + string.Format(@"delete from BundleReplacement_Detail_Art where id='{0}'", masterID);
            //將SubProcessID不是單一筆的資料拆開 
            DataTable bundle_Detail_Art_Tb_copy = bundle_Detail_Art_Tb.Copy();
            bundle_Detail_Art_Tb_copy.Clear();// 只有結構,沒有資料
            int ukey=1;
            foreach (DataRow dr1 in bundle_Detail_Art_Tb.Rows)
            {
                string[] subprocss = dr1["subprocessid"].ToString().Split('+');
                for (int i = 0; i < subprocss.Length; i++)
                {
                    DataRow drArt = bundle_Detail_Art_Tb_copy.NewRow();
                    drArt["Bundleno"] = dr1["Bundleno"];
                    drArt["SubProcessID"] = subprocss[i].ToString();
                    drArt["PatternCode"] = dr1["PatternCode"];
                    drArt["NoBundleCardAfterSubprocess"] = MyUtility.Convert.GetString(dr1["NoBundleCardAfterSubprocess_String"]).Split('+').ToList().Contains(subprocss[i].ToString());
                    drArt["PostSewingSubProcess"] = MyUtility.Convert.GetString(dr1["PostSewingSubProcess_String"]).Split('+').ToList().Contains(subprocss[i].ToString());
                    drArt["id"] = dr1["id"];
                    drArt["ukey"] = dr1["ukey"];
                    drArt["ukey1"] = ukey;
                    bundle_Detail_Art_Tb_copy.Rows.Add(drArt);
                    ukey++;
                }
               
            }
            //新增資料
            foreach (DataRow dr in bundle_Detail_Art_Tb_copy.Rows) //處理Bundle_Detail_Art
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    Art_cmd = Art_cmd + string.Format(
               @"insert into BundleReplacement_Detail_Art(ID,Bundleno,PatternCode,SubProcessid,NoBundleCardAfterSubprocess,PostSewingSubProcess) 
                        values('{0}','{1}','{2}','{3}',{4},{5});"
                , CurrentMaintain["ID"], dr["Bundleno"], dr["PatternCode"], dr["SubProcessid"], MyUtility.Convert.GetBool(dr["NoBundleCardAfterSubprocess"]) ? 1 : 0, MyUtility.Convert.GetBool(dr["PostSewingSubProcess"]) ? 1 : 0);
                }
            }

            #endregion

            #region 處理Bundle_Detail_Qty 修改版

            /*
           * 先刪除原有資料
           * 再新增更改的資料             
           */
            Qty_cmd = Qty_cmd + string.Format(@"Delete from BundleReplacement_Detail_Qty where id ='{0}'", masterID);
            foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows) //處理BundleReplacement_Detail_Art
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    Qty_cmd = Qty_cmd + string.Format(
                    @"insert into BundleReplacement_Detail_Qty(ID,SizeCode,Qty) 
                    values('{0}','{1}',{2});"
                    , CurrentMaintain["ID"], dr["sizecode"], dr["Qty"]);
                }
                   
            }

            #endregion

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(allpart_cmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, allpart_cmd)))
                    {
                        return upResult;
                    }
                }
                if (!MyUtility.Check.Empty(Art_cmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, Art_cmd)))
                    {
                        return upResult;
                    }
                }
                if (!MyUtility.Check.Empty(Qty_cmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, Qty_cmd)))
                    {
                        return upResult;
                    }
                }
                scope.Complete();
            }
            return base.ClickSavePost();
        }

        private void clear()
        {
            displaySeason.Text = "";
            displayStyle.Text = "";
            displayPrintDate.Text = "";
            CurrentMaintain["Cutno"] = 0;
            CurrentMaintain["sewinglineid"] = "";
            CurrentMaintain["OrderID"] = "";
            CurrentMaintain["POID"] = "";
            CurrentMaintain["PatternPanel"] = "";
            CurrentMaintain["Sizecode"] = "";
            CurrentMaintain["Ratio"] = "";
            CurrentMaintain["Article"] = "";
            CurrentMaintain["Colorid"] = "";
            CurrentMaintain["Qty"] = 0;
            CurrentMaintain["SewingCell"] = "";
            CurrentMaintain["startno"] = 1;
            CurrentMaintain["OriginBundleNo"] = "";
            CurrentMaintain["ITEM"] = "";
            txtFabricPanelCode.Text = "";

            DetailDatas.Clear();
            bundle_Detail_allpart_Tb.Clear();
            bundle_Detail_Art_Tb.Clear();
            bundle_Detail_Qty_Tb.Clear();
            WorkOrder_Ukey = "";
            CurrentMaintain.EndEdit();
        }

        private void txtOriginBundleNo_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = txtOriginBundleNo.Text;
            bundle_Detail_allpart_Tb.Clear();
            bundle_Detail_Art_Tb.Clear();
            bundle_Detail_Qty_Tb.Clear();
            // 先清除再重新帶入
            for (int i = ((DataTable)this.detailgridbs.DataSource).Rows.Count - 1; i >= 0; i--)
            {
                ((DataTable)this.detailgridbs.DataSource).Rows[i].Delete();
            }
            if (txtOriginBundleNo.Text == "")
            {
                clear();
                return;
            }

            string cmd = $@"
select b.* ,o.StyleID,o.SeasonID
from Bundle_Detail bd with(nolock) 
join Bundle b with(nolock) on b.id=bd.id 
join Orders o with(nolock) on o.ID = b.Orderid
where bd.BundleNo = '{newvalue}'
";
            DataRow bundledr;
            if (!MyUtility.Check.Seek(cmd, out bundledr, null))
            {
                clear();
                e.Cancel = true;
                MyUtility.Msg.WarningBox("<Origin Bundle No> data not found!");
                return;
            }
            else
            {
                CurrentMaintain["Cutno"] = bundledr["Cutno"];
                CurrentMaintain["sewinglineid"] = bundledr["sewinglineid"];
                CurrentMaintain["OrderID"] = bundledr["OrderID"];
                CurrentMaintain["POID"] = bundledr["POID"];
                CurrentMaintain["PatternPanel"] = bundledr["PatternPanel"];
                CurrentMaintain["Sizecode"] = bundledr["Sizecode"];
                CurrentMaintain["Ratio"] = bundledr["Ratio"];
                CurrentMaintain["Article"] = bundledr["Article"];
                CurrentMaintain["Colorid"] = bundledr["Colorid"];
                CurrentMaintain["Qty"] = bundledr["Qty"];
                CurrentMaintain["FabricPanelCode"] = bundledr["FabricPanelCode"];
                CurrentMaintain["SewingCell"] = bundledr["SewingCell"];
                CurrentMaintain["startno"] = bundledr["startno"];
                CurrentMaintain["ITEM"] = bundledr["ITEM"];
                CurrentMaintain["OriginBundleNo"] = newvalue;

                displaySeason.Text = bundledr["Seasonid"].ToString();
                displayStyle.Text = bundledr["Styleid"].ToString();

                CurrentMaintain.EndEdit();
            }


            string sqlcmd = $@"
Select a.*	,s.subProcessid, ps.NoBundleCardAfterSubprocess_String, nbs.PostSewingSubProcess_String, ukey1 = 0
From Bundle_Detail a WITH (NOLOCK) 
outer apply
(
	select subProcessid =
	stuff((
		Select concat('+',Subprocessid)
		From Bundle_Detail_art c WITH (NOLOCK) 
		Where c.bundleno =a.bundleno and c.id = a.id 
		Order by Subprocessid
		For XML path('')
	),1,1,'')
) as s
outer apply
(
	select NoBundleCardAfterSubprocess_String =
	stuff((
		Select concat('+',Subprocessid)
		From Bundle_Detail_art c WITH (NOLOCK) 
		Where c.bundleno =a.bundleno and c.id = a.id and c.NoBundleCardAfterSubprocess = 1
		Order by Subprocessid
		For XML path('')
	),1,1,'')
) as ps
outer apply
(
	select PostSewingSubProcess_String =
	stuff((
		Select concat('+',Subprocessid)
		From Bundle_Detail_art c WITH (NOLOCK) 
		Where c.bundleno =a.bundleno and c.id = a.id and c.PostSewingSubProcess = 1
		Order by Subprocessid
		For XML path('')
	),1,1,'')
) as nbs
where a.bundleNo = '{newvalue}'
order by bundlegroup";
            DataRow dtRow;
            if (MyUtility.Check.Seek(sqlcmd,out dtRow))
            {
                // 先處理 ..._Allpart, ..._qty, ..._Art
                DataTable dt;
                DualResult result;

                // ALLPARTS才要帶入..._Allpart
                if (MyUtility.Convert.GetString(dtRow["PatternCode"]).ToUpper() == "ALLPARTS")
                {
                    sqlcmd = $@"select * from Bundle_Detail_Allpart where ID = '{bundledr["id"]}'";
                    result = DBProxy.Current.Select(null, sqlcmd, out dt);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }
                    SelectItem2 item2 = new SelectItem2(dt, "Patterncode,PatternDesc", "Pattern Code,PatternDesc,Parts,IsPair", "15,20,5,5", null);
                    DialogResult dresult = item2.ShowDialog();
                    if (dresult == DialogResult.Cancel)
                    {
                        // 不帶入表身和 ..._Allpart, ..._qty, ..._Art
                        return;
                    }

                    var allpart = dt.AsEnumerable().
                        Where(w => item2.GetSelectedList().Contains(MyUtility.Convert.GetString(w["Patterncode"]))).
                        ToList().CopyToDataTable();
                    bundle_Detail_allpart_Tb.Merge(allpart); // 塞入 _Allpart
                    dtRow["Parts"] = allpart.Rows.Count; // 箱選擇的Part數量帶入, 表身的Parts數
                    foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows)
                    {
                        dr["ID"] = CurrentMaintain["ID"];
                        dr["ukey"] = DBNull.Value;
                    }
                }

                // _qty帶入
                DataRow bdqRow = bundle_Detail_Qty_Tb.NewRow();
                bdqRow["No"] = 1;
                bdqRow["SizeCode"] = dtRow["SizeCode"];
                bdqRow["Qty"] = 1;
                bundle_Detail_Qty_Tb.Rows.Add(bdqRow); // 塞入 _Qtyallpart

                // _Art帶入
                sqlcmd = $@"select * from Bundle_Detail_Art where BundleNo = '{newvalue}'";
                result = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
                bundle_Detail_Art_Tb.Merge(dt); // 塞入 _Art

                // 表身
                dtRow["BundleNo"] = string.Empty; // 表身的BundleNo不用帶
                dtRow["Qty"] = 1;

                dtRow.AcceptChanges();
                dtRow.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dtRow);
            }
        }

        private void txtSPNo_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["POID"])) return;
            Sci.Win.Tools.SelectItem item;
            string cuttingid = MyUtility.GetValue.Lookup("Cuttingsp", CurrentMaintain["POID"].ToString(), "Orders", "ID");

            string selectCommand = string.Format(@"
select distinct b.orderid 
from workorder a WITH (NOLOCK) , workorder_distribute b WITH (NOLOCK) 
where a.id = '{0}' and a.ukey = b.workorderukey"
                                                , cuttingid);
            item = new Sci.Win.Tools.SelectItem(selectCommand, "20", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtSPNo.Text = item.GetSelectedString();
        }

        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = txtSPNo.Text;
            if (txtSPNo.OldValue.ToString() == newvalue) return;
            string cuttingsp = MyUtility.GetValue.Lookup("Cuttingsp", newvalue, "Orders", "ID");
            //Issue#969 SP No 清空時，清空MasterID與Item、Line            
            if (MyUtility.Check.Empty(newvalue))
            {
                CurrentMaintain["POID"] = "";
                CurrentMaintain["Item"] = "";
                CurrentMaintain["sewinglineid"] = "";
                DetailDatas.Clear();
                bundle_Detail_allpart_Tb.Clear();
                bundle_Detail_Art_Tb.Clear();
                bundle_Detail_Qty_Tb.Clear();
            }
            else
            {
                string selectCommand = string.Format("select a.* from orders a WITH (NOLOCK) where a.id = '{0}' and mDivisionid='{1}' ", newvalue, keyword);
                DataRow cutdr;
                if (MyUtility.Check.Seek(selectCommand, out cutdr, null))
                {
                    if (cutdr["Sewline"].ToString().Length > 2)
                    {
                        CurrentMaintain["sewinglineid"] = cutdr["Sewline"].ToString().Substring(0, 2);
                    }
                    else
                    {
                        CurrentMaintain["sewinglineid"] = cutdr["Sewline"].ToString();
                    }

                    CurrentMaintain["OrderID"] = cutdr["id"].ToString();
                    CurrentMaintain["POID"] = cutdr["POID"].ToString();
                    displaySeason.Text = cutdr["Seasonid"].ToString();
                    displayStyle.Text = cutdr["Styleid"].ToString();
                    #region Item
                    string item_cmd = string.Format("Select a.Name from Reason a WITH (NOLOCK) , Style b WITH (NOLOCK)  where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{0}' and b.ApparelType = a.id", cutdr["styleukey"]);
                    string item = MyUtility.GetValue.Lookup(item_cmd, null);
                    CurrentMaintain["ITEM"] = item;
                    #endregion
                    #region Cell
                    if (!MyUtility.Check.Empty(cutdr["sewline"].ToString()))
                    {
                        string cellid = MyUtility.GetValue.Lookup("SewingCell", cutdr["sewline"].ToString().Split('/')[0] + cutdr["Factoryid"].ToString(), "SewingLine", "ID+factoryid");

                        CurrentMaintain["SewingCell"] = cellid;
                    }
                    #endregion
                    #region startno
                    int startno = startNo_Function(CurrentMaintain["OrderID"].ToString());
                    int nGroup = startno - Convert.ToInt32(CurrentMaintain["startno"]);
                    CurrentMaintain["startno"] = startno;
                    foreach (DataRow dr in DetailDatas)
                    {
                        dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + nGroup;
                    }
                    #endregion
                    #region Article colorid
                    if (MyUtility.Check.Empty(CurrentMaintain["PatternPanel"]))
                    {
                        string ColorComb_cmd = string.Format("Select top(1) article,colorid from order_colorcombo WITH (NOLOCK) where id ='{0}' and patternpanel = '{1}'", cutdr["POID"], CurrentMaintain["PatternPanel"]);
                        DataRow colordr;
                        if (MyUtility.Check.Seek(ColorComb_cmd, out colordr))
                        {
                            CurrentMaintain["Article"] = colordr["Article"];
                            CurrentMaintain["colorid"] = colordr["colorid"];
                        }

                    }
                    #endregion
                }
            }
        }

        private void btnGarmentList_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", CurrentMaintain["poid"].ToString(), "Orders", "ID");
            var Sizelist = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            Sci.Production.PublicForm.GarmentList callNextForm =
    new Sci.Production.PublicForm.GarmentList(ukey, CurrentMaintain["poid"].ToString(), txtOriginBundleNo.Text, Sizelist);
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }

        public int startNo_Function(string orderid) //Start No 計算
        {
            #region startno
            string max_cmd = string.Format("Select Max(startno+Qty) as Start from BundleReplacement  WITH (NOLOCK) Where OrderID = '{0}'", orderid);
            DataTable max_st;
            if (DBProxy.Current.Select(null, max_cmd, out max_st))
            {
                if (max_st.Rows[0][0] != DBNull.Value) return Convert.ToInt32(max_st.Rows[0]["Start"]);
                else return 1;
            }
            else
            {
                return 1;
            }
            #endregion
        }

        protected override void ClickCopyAfter()
        {
            int oldstartno = MyUtility.Convert.GetInt(CurrentMaintain["startno"]);
            base.ClickCopyAfter();
            CurrentMaintain["ID"] = "";
            CurrentMaintain["Cdate"] = DateTime.Now;
            int startno = startNo_Function(CurrentMaintain["OrderID"].ToString());
            CurrentMaintain["startno"] = startno;
            #region 清除Detail Bundleno,ID
            foreach (DataRow dr in DetailDatas)
            {
                dr["Bundleno"] = "";
                dr["ID"] = "";
                dr["BundleGroup"] = MyUtility.Convert.GetInt(dr["BundleGroup"]) + startno - oldstartno;
            }
            foreach (DataRow dr in bundle_Detail_allpart_Tb.Rows)
            {
                dr["ID"] = "";
            }
            foreach (DataRow dr in bundle_Detail_Art_Tb.Rows)
            {
                dr["Bundleno"] = "";
                dr["ID"] = "";
            }
            foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows)
            {
                dr["ID"] = "";
            }
            #endregion
        }

        private void numBeginBundleGroup_Validated(object sender, EventArgs e)
        {
            decimal no = (decimal)numBeginBundleGroup.Value;
            decimal oldvalue = (decimal)numBeginBundleGroup.OldValue;
            decimal nGroup = no - oldvalue;
            foreach (DataRow dr in DetailDatas)
            {
                dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + nGroup;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["article"]) || MyUtility.Check.Empty(CurrentMaintain["PatternPanel"]))
            {
                ShowErr("Fabric Combo and Article can't empty!");
                return;
            }
            DataTable dt = ((DataTable)detailgridbs.DataSource);
            detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P10_1_Generate(CurrentMaintain, dt, bundle_Detail_allpart_Tb, bundle_Detail_Art_Tb, bundle_Detail_Qty_Tb);
            frm.ShowDialog(this);
            dt.DefaultView.Sort = "BundleGroup";

        }
        protected override bool ClickPrint()
        {
            P10_1_Print p = new P10_1_Print(this.CurrentMaintain);
            p.ShowDialog();
            string dtn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.displayPrintDate.Text = dtn;
            string sqlcmd = string.Format(
                @"update BundleReplacement set PrintDate = '{0}' where ID = '{1}';
                  update BundleReplacement_Detail set PrintDate = '{0}' where ID = '{1}';"
                , dtn, CurrentMaintain["ID"]);
            DBProxy.Current.Execute(null, sqlcmd);
            return true;

        }
        int at;
        private void txtArticle_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            string selectCommand, sqlwhere = "";
            Sci.Win.Tools.SelectItem item;
            if (!EditMode) return;

            if (!MyUtility.Check.Empty(CurrentMaintain["PatternPanel"]))
            {
                sqlwhere = string.Format(" and w.FabricCombo = '{0}'", txtFabricCombo.Text);
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["Orderid"]))
            {
                string scount = string.Format(@"
select distinct count(Article)
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.OrderID = '{0}' and w.mDivisionid = '{1}' {2}"
                    , CurrentMaintain["Orderid"].ToString(), keyword, sqlwhere);
                string count = MyUtility.GetValue.Lookup(scount, null);
                if (count != "0")
                {
                    selectCommand = string.Format(@"
select distinct Article ,w.Colorid
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.OrderID = '{0}' and w.mDivisionid = '{1}' {2}"
                    , CurrentMaintain["Orderid"].ToString(), keyword, sqlwhere);
                    at = 1;
                }
                else
                {
                    selectCommand = string.Format(@"
SELECT OA.Article , color.ColorID
FROM Order_Article OA WITH (NOLOCK)
CROSS APPLY (SELECT TOP 1 ColorID FROM Order_ColorCombo OCC WITH (NOLOCK) WHERE OCC.Id=OA.id and OCC.Article=OA.Article) color
where OA.id = '{0}'
ORDER BY Seq"
                   , CurrentMaintain["Orderid"].ToString());
                }
                item = new Sci.Win.Tools.SelectItem(selectCommand, "20", this.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }

                CurrentMaintain["article"] = item.GetSelecteds()[0]["Article"].ToString().TrimEnd();
                CurrentMaintain["Colorid"] = item.GetSelecteds()[0]["Colorid"].ToString().TrimEnd();
                at = 2;
            }
        }

        private void txtArticle_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = txtArticle.Text;
            if (txtArticle.OldValue.ToString() == newvalue) return;
            string sql;
            DataTable dtTEMP;

            if (!MyUtility.Check.Empty(CurrentMaintain["Orderid"]))
            {
                if (at == 1)
                {
                    sql = string.Format(@"select article from Order_Qty WITH (NOLOCK) where Id='{0}' and Article='{1}'", CurrentMaintain["Orderid"], newvalue);
                    if (DBProxy.Current.Select(null, sql, out dtTEMP))
                    {
                        if (dtTEMP.Rows.Count == 0)
                        {
                            txtArticle.Text = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("<Article> can't find !!");
                            return;
                        }
                    }
                }
                else
                {
                    sql = string.Format(@"select OA.Article from Order_Article OA WITH (NOLOCK) where OA.id = '{0}'and OA.Article  ='{1}'", CurrentMaintain["Orderid"], newvalue);
                    if (DBProxy.Current.Select(null, sql, out dtTEMP))
                    {
                        if (dtTEMP.Rows.Count == 0)
                        {
                            txtArticle.Text = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("<Article> can't find !!");
                            return;
                        }
                    }
                }
            }
        }

        private void txtLineNo_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode) return;
            string sql = string.Format(@"Select ID,FactoryID,Description  From SewingLine WITH (NOLOCK) 
                                        where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}')", Sci.Env.User.Keyword);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "2,8,16", this.Text, false, ",");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtLineNo.Text = item.GetSelectedString();
        }

        private void BtnBatchDelete_Click(object sender, EventArgs e)
        {
            var form = new P10_1_BatchDelete();
            form.ShowDialog();
            this.RenewData();
        }

        private void txtLineNo_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;
            string newvalue = txtLineNo.Text;
            if (txtLineNo.OldValue.ToString() == newvalue) return;
            string sql = string.Format(@"Select ID  From SewingLine WITH (NOLOCK)  
                    where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}') and ID='{1}'", Sci.Env.User.Keyword, newvalue);
            string tmp = MyUtility.GetValue.Lookup(sql);
            if (string.IsNullOrWhiteSpace(tmp))
            {
                txtLineNo.Text = "";
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Sewing Line> : {0} not found!!!", newvalue));
                return;
            }
        }

        protected override DualResult ClickDeletePost()
        {
            string id = CurrentMaintain["ID"].ToString();
            string deleteBundleDetailQty = $@"
delete 
from BundleReplacement_Detail_Qty
where id = '{id}'
delete 
from BundleReplacement_Detail_Allpart
where id = '{id}'
";

            DualResult result = DBProxy.Current.Execute(null, deleteBundleDetailQty);

            if (result == false)
            {
                return result;
            }

            return base.ClickDeletePost();
        }
    }
}
