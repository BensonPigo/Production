using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Sci.Win.UI;
using Ict;
using Sci.Win.Tools;
using System.Linq;
using System.Transactions;

namespace Sci.Production.Cutting
{
    public partial class P10_1 : Sci.Win.Tems.Input6
    {
        string keyword = Sci.Env.User.Keyword;
        string LoginId = Sci.Env.User.UserID;
        DataTable bundle_Detail_allpart_Tb;
        DataTable bundle_Detail_Art_Tb;
        DataTable bundle_Detail_Qty_Tb;
        string WorkOrder_Ukey;

        public P10_1(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.DefaultWhere = $@"(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = BundleReplacement.Orderid)  = '{Sci.Env.User.Factory}'";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory WITH (NOLOCK)
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);

            // 取得當前登入工廠index
            for (int i = 0; i < queryDT.Rows.Count; i++)
            {
                if (string.Compare(queryDT.Rows[i]["FTYGroup"].ToString(), Sci.Env.User.Factory) == 0)
                {
                    this.queryfors.SelectedIndex = i;
                }
            }

            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = BundleReplacement.Orderid)  = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.detailgrid)
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
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmdsql = string.Format(
                @"
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
order by bundlegroup",
                masterID);
            this.DetailSelectCommand = cmdsql;
            #endregion

            #region 先撈出底層其他Table
            if (!this.IsDetailInsertByCopy)
            {
                string allPart_cmd = string.Format(@"Select sel = 0,*, ukey1 = 0, annotation = '' from  BundleReplacement_Detail_Allpart  WITH (NOLOCK)  Where id ='{0}'", masterID);
                string art_cmd = string.Format(@"Select b.*, ukey1 = 0,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from BundleReplacement_Detail a WITH (NOLOCK) inner join BundleReplacement_Detail_art b WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id Where a.id ='{0}' ", masterID);
                string qty_cmd = string.Format(@"Select No = 0, a.* from BundleReplacement_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
                DualResult dRes = null;
                dRes = DBProxy.Current.Select(null, allPart_cmd, out this.bundle_Detail_allpart_Tb);
                if (!dRes)
                {
                    this.ShowErr(allPart_cmd, dRes);
                    return dRes;
                }

                dRes = DBProxy.Current.Select(null, art_cmd, out this.bundle_Detail_Art_Tb);
                if (!dRes)
                {
                    this.ShowErr(art_cmd, dRes);
                    return dRes;
                }

                dRes = DBProxy.Current.Select(null, qty_cmd, out this.bundle_Detail_Qty_Tb);
                if (!dRes)
                {
                    this.ShowErr(qty_cmd, dRes);
                    return dRes;
                }
            }
            #endregion

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.queryTable();
            string orderid = this.CurrentMaintain["OrderID"].ToString();
            string cuttingid = string.Empty;
            DataTable orders;
            if (DBProxy.Current.Select(null, string.Format("Select * from Orders WITH (NOLOCK) Where id='{0}'", orderid), out orders))
            {
                if (orders.Rows.Count != 0)
                {
                    this.displaySeason.Text = orders.Rows[0]["Seasonid"].ToString();
                    this.displayStyle.Text = orders.Rows[0]["Styleid"].ToString();
                    cuttingid = orders.Rows[0]["cuttingsp"].ToString();
                }
                else
                {
                    this.displaySeason.Text = string.Empty;
                    this.displayStyle.Text = string.Empty;
                }
            }
            else
            {
                this.displaySeason.Text = string.Empty;
                this.displayStyle.Text = string.Empty;
            }

            int qty = 0;
            if (this.bundle_Detail_Qty_Tb.Rows.Count == 0)
            {
                qty = 0;
            }
            else
            {
                qty = Convert.ToInt16(this.bundle_Detail_Qty_Tb.Compute("Sum(Qty)", string.Empty));
            }

            this.numQtyperBundleGroup.Value = qty;

            string factoryid = MyUtility.GetValue.Lookup(string.Format("SELECT o.FactoryID FROM BundleReplacement b WITH (NOLOCK) inner join orders o WITH (NOLOCK) on b.Orderid=o.ID where b.Orderid='{0}'", orderid), null);

            // txtsewingline1.factoryobjectName = (Control)factoryid;
            if (!MyUtility.Check.Empty(this.CurrentMaintain["printdate"]))
            {
                DateTime? lastTime = (DateTime?)this.CurrentMaintain["printdate"];
                string FtyLastupdate = lastTime == null ? string.Empty : ((DateTime)lastTime).ToString("yyyy/MM/dd HH:mm:ss");
                this.displayPrintDate.Text = FtyLastupdate;
            }
            else
            {
                this.displayPrintDate.Text = string.Empty;
            }
        }

        public void queryTable()
        {
            string masterID = (this.CurrentMaintain == null) ? string.Empty : this.CurrentMaintain["id"].ToString();
            string allPart_cmd = string.Format(@"Select 0 as sel,*, 0 as ukey1,'' as annotation from BundleReplacement_Detail_Allpart  WITH (NOLOCK) Where id ='{0}' ", masterID);
            string art_cmd = string.Format(@"Select b.*, 0 as ukey1,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from BundleReplacement_Detail a WITH (NOLOCK) ,BundleReplacement_Detail_art b WITH (NOLOCK) Where a.id ='{0}' and a.Bundleno = b.bundleno and a.id = b.id", masterID);
            string qty_cmd = string.Format(@"Select 0 as No,a.* from BundleReplacement_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out this.bundle_Detail_allpart_Tb);
            if (!dRes)
            {
                this.ShowErr(allPart_cmd, dRes);
            }

            dRes = DBProxy.Current.Select(null, art_cmd, out this.bundle_Detail_Art_Tb);
            if (!dRes)
            {
                this.ShowErr(art_cmd, dRes);
            }

            dRes = DBProxy.Current.Select(null, qty_cmd, out this.bundle_Detail_Qty_Tb);
            if (!dRes)
            {
                this.ShowErr(qty_cmd, dRes);
            }

            int ukey = 1;
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["ukey1"] = ukey;
                DataRow[] allar = this.bundle_Detail_allpart_Tb.Select(string.Format("id='{0}'", dr["ID"]));
                if (allar.Length > 0)
                {
                    foreach (DataRow dr2 in allar)
                    {
                        dr2["ukey1"] = ukey;
                    }
                }

                DataRow[] Artar = this.bundle_Detail_Art_Tb.Select(string.Format("Bundleno='{0}'", dr["Bundleno"]));
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
            this.CurrentMaintain["Cdate"] = DateTime.Today;
            this.CurrentMaintain["mDivisionid"] = this.keyword;
            this.displayPrintDate.Text = string.Empty;
            this.bundle_Detail_allpart_Tb.Clear();
            this.bundle_Detail_Art_Tb.Clear();
            this.bundle_Detail_Qty_Tb.Clear();
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["OrderID"]))
            {
                MyUtility.Msg.WarningBox("<SP#> can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
            {
                MyUtility.Msg.WarningBox("<Pattern Panel> can not be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["sewinglineid"]))
            {
                MyUtility.Msg.WarningBox("<Line> can not be empty!");
                return false;
            }

            if (this.IsDetailInserting)
            {
                string Keyword = this.keyword + "RP";
                string cid = MyUtility.GetValue.GetID(Keyword, "BundleReplacement", Convert.ToDateTime(this.CurrentMaintain["cdate"]), sequenceMode: 2);
                if (string.IsNullOrWhiteSpace(cid))
                {
                    return false;
                }

                this.CurrentMaintain["ID"] = cid;
                foreach (DataRow dr in this.bundle_Detail_allpart_Tb.Rows)
                {
                    dr["ID"] = cid;
                }

                foreach (DataRow dr in this.bundle_Detail_Art_Tb.Rows)
                {
                    dr["ID"] = cid;
                }

                foreach (DataRow dr in this.bundle_Detail_Qty_Tb.Rows)
                {
                    dr["ID"] = cid;
                }
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePre()
        {
            #region 填入Bundleno
            int drcount = this.DetailDatas.Count;
            IList<string> cListBundleno;
            cListBundleno = MyUtility.GetValue.GetBatchID("R", "BundleReplacement_Detail", MyUtility.Check.Empty(this.CurrentMaintain["cDate"]) ? default(DateTime) : Convert.ToDateTime(this.CurrentMaintain["cDate"]), 3, "Bundleno", batchNumber: drcount, sequenceMode: 2);
            if (cListBundleno.Count == 0)
            {
                return new DualResult(false, "Create Bundleno error.");
            }

            int nCount = 0;

            DataRow[] roway;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Bundleno"]) && dr.RowState != DataRowState.Deleted)
                {
                    dr["Bundleno"] = cListBundleno[nCount];

                    roway = this.bundle_Detail_Art_Tb.Select(string.Format("ukey1 = {0}", dr["ukey1"]));
                    foreach (DataRow dr2 in roway)
                    {
                        dr2["Bundleno"] = cListBundleno[nCount];
                    }

                    nCount++;
                }
            }
            #endregion

            // DataTable dt = (DataTable)detailgridbs.DataSource;
            return base.ClickSavePre();
        }

        protected override DualResult ClickSavePost()
        {
            string allpart_cmd = string.Empty, Art_cmd = string.Empty, Qty_cmd = string.Empty;
            #region 先撈出實體Table 為了平行判斷筆數 DataTable allparttmp, arttmp, qtytmp
            DataTable allparttmp, arttmp, qtytmp;
            string masterID = (this.CurrentMaintain == null) ? string.Empty : this.CurrentMaintain["id"].ToString();

            string allPart_cmd = string.Format(@"select * from BundleReplacement_Detail_Allpart WITH (NOLOCK) where id='{0}'  ", masterID);
            string art_cmd = string.Format(@"Select b.* from BundleReplacement_Detail_art b WITH (NOLOCK) left join BundleReplacement_Detail a WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id where b.id='{0}'", masterID);
            string qty_cmd = string.Format(@"Select a.* from BundleReplacement_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out allparttmp);
            if (!dRes)
            {
                this.ShowErr(allPart_cmd, dRes);
            }

            dRes = DBProxy.Current.Select(null, art_cmd, out arttmp);
            if (!dRes)
            {
                this.ShowErr(art_cmd, dRes);
            }

            dRes = DBProxy.Current.Select(null, qty_cmd, out qtytmp);
            if (!dRes)
            {
                this.ShowErr(qty_cmd, dRes);
            }
            #endregion

            // int row = 0;
            #region 處理Bundle_Detail_AllPart 修改版

            /*
             * 先刪除原有資料
             * 再新增更改的資料
             */
            allpart_cmd = allpart_cmd + string.Format(@"Delete from BundleReplacement_Detail_allpart where id ='{0}'", masterID);
            foreach (DataRow dr in this.bundle_Detail_allpart_Tb.Rows) // 處理Bundle_Detail_AllPart
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    allpart_cmd = allpart_cmd + string.Format(
                @"insert into BundleReplacement_Detail_allpart(ID,PatternCode,PatternDesc,Parts,isPair,Location) values('{0}','{1}','{2}','{3}','{4}','{5}');",
                this.CurrentMaintain["ID"], dr["PatternCode"], dr["PatternDesc"], dr["Parts"], dr["isPair"], dr["Location"]);
                }
            }
            #endregion

            #region 處理Bundle_Detail_Art 修改版
            /*
            * 先刪除原有資料
            * 再新增更改的資料
            */
            Art_cmd = Art_cmd + string.Format(@"delete from BundleReplacement_Detail_Art where id='{0}'", masterID);

            // 將SubProcessID不是單一筆的資料拆開
            DataTable bundle_Detail_Art_Tb_copy = this.bundle_Detail_Art_Tb.Copy();
            bundle_Detail_Art_Tb_copy.Clear(); // 只有結構,沒有資料
            int ukey = 1;
            foreach (DataRow dr1 in this.bundle_Detail_Art_Tb.Rows)
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

            // 新增資料
            foreach (DataRow dr in bundle_Detail_Art_Tb_copy.Rows) // 處理Bundle_Detail_Art
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    Art_cmd = Art_cmd + string.Format(
               @"insert into BundleReplacement_Detail_Art(ID,Bundleno,PatternCode,SubProcessid,NoBundleCardAfterSubprocess,PostSewingSubProcess) 
                        values('{0}','{1}','{2}','{3}',{4},{5});",
               this.CurrentMaintain["ID"], dr["Bundleno"], dr["PatternCode"], dr["SubProcessid"], MyUtility.Convert.GetBool(dr["NoBundleCardAfterSubprocess"]) ? 1 : 0, MyUtility.Convert.GetBool(dr["PostSewingSubProcess"]) ? 1 : 0);
                }
            }

            #endregion

            #region 處理Bundle_Detail_Qty 修改版

            /*
           * 先刪除原有資料
           * 再新增更改的資料
           */
            Qty_cmd = Qty_cmd + string.Format(@"Delete from BundleReplacement_Detail_Qty where id ='{0}'", masterID);
            foreach (DataRow dr in this.bundle_Detail_Qty_Tb.Rows) // 處理BundleReplacement_Detail_Art
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    Qty_cmd = Qty_cmd + string.Format(
                    @"insert into BundleReplacement_Detail_Qty(ID,SizeCode,Qty) 
                    values('{0}','{1}',{2});",
                    this.CurrentMaintain["ID"], dr["sizecode"], dr["Qty"]);
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
            this.displaySeason.Text = string.Empty;
            this.displayStyle.Text = string.Empty;
            this.displayPrintDate.Text = string.Empty;
            this.CurrentMaintain["Cutno"] = 0;
            this.CurrentMaintain["sewinglineid"] = string.Empty;
            this.CurrentMaintain["OrderID"] = string.Empty;
            this.CurrentMaintain["POID"] = string.Empty;
            this.CurrentMaintain["PatternPanel"] = string.Empty;
            this.CurrentMaintain["Sizecode"] = string.Empty;
            this.CurrentMaintain["Ratio"] = string.Empty;
            this.CurrentMaintain["Article"] = string.Empty;
            this.CurrentMaintain["Colorid"] = string.Empty;
            this.CurrentMaintain["Qty"] = 0;
            this.CurrentMaintain["SewingCell"] = string.Empty;
            this.CurrentMaintain["startno"] = 1;
            this.CurrentMaintain["OriginBundleNo"] = string.Empty;
            this.CurrentMaintain["ITEM"] = string.Empty;
            this.txtFabricPanelCode.Text = string.Empty;

            this.DetailDatas.Clear();
            this.bundle_Detail_allpart_Tb.Clear();
            this.bundle_Detail_Art_Tb.Clear();
            this.bundle_Detail_Qty_Tb.Clear();
            this.WorkOrder_Ukey = string.Empty;
            this.CurrentMaintain.EndEdit();
        }

        private void txtOriginBundleNo_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string newvalue = this.txtOriginBundleNo.Text;
            this.bundle_Detail_allpart_Tb.Clear();
            this.bundle_Detail_Art_Tb.Clear();
            this.bundle_Detail_Qty_Tb.Clear();

            // 先清除再重新帶入
            for (int i = ((DataTable)this.detailgridbs.DataSource).Rows.Count - 1; i >= 0; i--)
            {
                ((DataTable)this.detailgridbs.DataSource).Rows[i].Delete();
            }

            if (this.txtOriginBundleNo.Text == string.Empty)
            {
                this.clear();
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
                this.clear();
                e.Cancel = true;
                MyUtility.Msg.WarningBox("<Origin Bundle No> data not found!");
                return;
            }
            else
            {
                this.CurrentMaintain["Cutno"] = bundledr["Cutno"];
                this.CurrentMaintain["sewinglineid"] = bundledr["sewinglineid"];
                this.CurrentMaintain["OrderID"] = bundledr["OrderID"];
                this.CurrentMaintain["POID"] = bundledr["POID"];
                this.CurrentMaintain["PatternPanel"] = bundledr["PatternPanel"];
                this.CurrentMaintain["Sizecode"] = bundledr["Sizecode"];
                this.CurrentMaintain["Ratio"] = bundledr["Ratio"];
                this.CurrentMaintain["Article"] = bundledr["Article"];
                this.CurrentMaintain["Colorid"] = bundledr["Colorid"];
                this.CurrentMaintain["Qty"] = bundledr["Qty"];
                this.CurrentMaintain["FabricPanelCode"] = bundledr["FabricPanelCode"];
                this.CurrentMaintain["SewingCell"] = bundledr["SewingCell"];
                this.CurrentMaintain["startno"] = bundledr["startno"];
                this.CurrentMaintain["ITEM"] = bundledr["ITEM"];
                this.CurrentMaintain["OriginBundleNo"] = newvalue;

                this.displaySeason.Text = bundledr["Seasonid"].ToString();
                this.displayStyle.Text = bundledr["Styleid"].ToString();

                this.CurrentMaintain.EndEdit();
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
            if (MyUtility.Check.Seek(sqlcmd, out dtRow))
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
                    this.bundle_Detail_allpart_Tb.Merge(allpart); // 塞入 _Allpart
                    dtRow["Parts"] = allpart.Rows.Count; // 箱選擇的Part數量帶入, 表身的Parts數
                    foreach (DataRow dr in this.bundle_Detail_allpart_Tb.Rows)
                    {
                        dr["ID"] = this.CurrentMaintain["ID"];
                        dr["ukey"] = DBNull.Value;
                    }
                }

                // _qty帶入
                DataRow bdqRow = this.bundle_Detail_Qty_Tb.NewRow();
                bdqRow["No"] = 1;
                bdqRow["SizeCode"] = dtRow["SizeCode"];
                bdqRow["Qty"] = 1;
                this.bundle_Detail_Qty_Tb.Rows.Add(bdqRow); // 塞入 _Qtyallpart

                // _Art帶入
                sqlcmd = $@"select * from Bundle_Detail_Art where BundleNo = '{newvalue}'";
                result = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.bundle_Detail_Art_Tb.Merge(dt); // 塞入 _Art

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
            if (MyUtility.Check.Empty(this.CurrentMaintain["POID"]))
            {
                return;
            }

            Sci.Win.Tools.SelectItem item;
            string cuttingid = MyUtility.GetValue.Lookup("Cuttingsp", this.CurrentMaintain["POID"].ToString(), "Orders", "ID");

            string selectCommand = string.Format(
                @"
select distinct b.orderid 
from workorder a WITH (NOLOCK) , workorder_distribute b WITH (NOLOCK) 
where a.id = '{0}' and a.ukey = b.workorderukey",
                cuttingid);
            item = new Sci.Win.Tools.SelectItem(selectCommand, "20", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNo.Text = item.GetSelectedString();
        }

        private void txtSPNo_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string newvalue = this.txtSPNo.Text;
            if (this.txtSPNo.OldValue.ToString() == newvalue)
            {
                return;
            }

            string cuttingsp = MyUtility.GetValue.Lookup("Cuttingsp", newvalue, "Orders", "ID");

            // Issue#969 SP No 清空時，清空MasterID與Item、Line
            if (MyUtility.Check.Empty(newvalue))
            {
                this.CurrentMaintain["POID"] = string.Empty;
                this.CurrentMaintain["Item"] = string.Empty;
                this.CurrentMaintain["sewinglineid"] = string.Empty;
                this.DetailDatas.Clear();
                this.bundle_Detail_allpart_Tb.Clear();
                this.bundle_Detail_Art_Tb.Clear();
                this.bundle_Detail_Qty_Tb.Clear();
            }
            else
            {
                string selectCommand = string.Format("select a.* from orders a WITH (NOLOCK) where a.id = '{0}' and mDivisionid='{1}' ", newvalue, this.keyword);
                DataRow cutdr;
                if (MyUtility.Check.Seek(selectCommand, out cutdr, null))
                {
                    if (cutdr["Sewline"].ToString().Length > 2)
                    {
                        this.CurrentMaintain["sewinglineid"] = cutdr["Sewline"].ToString().Substring(0, 2);
                    }
                    else
                    {
                        this.CurrentMaintain["sewinglineid"] = cutdr["Sewline"].ToString();
                    }

                    this.CurrentMaintain["OrderID"] = cutdr["id"].ToString();
                    this.CurrentMaintain["POID"] = cutdr["POID"].ToString();
                    this.displaySeason.Text = cutdr["Seasonid"].ToString();
                    this.displayStyle.Text = cutdr["Styleid"].ToString();
                    #region Item
                    string item_cmd = string.Format("Select a.Name from Reason a WITH (NOLOCK) , Style b WITH (NOLOCK)  where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{0}' and b.ApparelType = a.id", cutdr["styleukey"]);
                    string item = MyUtility.GetValue.Lookup(item_cmd, null);
                    this.CurrentMaintain["ITEM"] = item;
                    #endregion
                    #region Cell
                    if (!MyUtility.Check.Empty(cutdr["sewline"].ToString()))
                    {
                        string cellid = MyUtility.GetValue.Lookup("SewingCell", cutdr["sewline"].ToString().Split('/')[0] + cutdr["Factoryid"].ToString(), "SewingLine", "ID+factoryid");

                        this.CurrentMaintain["SewingCell"] = cellid;
                    }
                    #endregion
                    #region startno
                    int startno = this.startNo_Function(this.CurrentMaintain["OrderID"].ToString());
                    int nGroup = startno - Convert.ToInt32(this.CurrentMaintain["startno"]);
                    this.CurrentMaintain["startno"] = startno;
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + nGroup;
                    }
                    #endregion
                    #region Article colorid
                    if (MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
                    {
                        string ColorComb_cmd = string.Format("Select top(1) article,colorid from order_colorcombo WITH (NOLOCK) where id ='{0}' and patternpanel = '{1}'", cutdr["POID"], this.CurrentMaintain["PatternPanel"]);
                        DataRow colordr;
                        if (MyUtility.Check.Seek(ColorComb_cmd, out colordr))
                        {
                            this.CurrentMaintain["Article"] = colordr["Article"];
                            this.CurrentMaintain["colorid"] = colordr["colorid"];
                        }
                    }
                    #endregion
                }
            }
        }

        private void btnGarmentList_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.CurrentMaintain["poid"].ToString(), "Orders", "ID");
            var Sizelist = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            Sci.Production.PublicForm.GarmentList callNextForm =
    new Sci.Production.PublicForm.GarmentList(ukey, this.CurrentMaintain["poid"].ToString(), this.txtOriginBundleNo.Text, Sizelist);
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
        }

        public int startNo_Function(string orderid) // Start No 計算
        {
            #region startno
            string max_cmd = string.Format("Select Max(startno+Qty) as Start from BundleReplacement  WITH (NOLOCK) Where OrderID = '{0}'", orderid);
            DataTable max_st;
            if (DBProxy.Current.Select(null, max_cmd, out max_st))
            {
                if (max_st.Rows[0][0] != DBNull.Value)
                {
                    return Convert.ToInt32(max_st.Rows[0]["Start"]);
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
            #endregion
        }

        protected override void ClickCopyAfter()
        {
            int oldstartno = MyUtility.Convert.GetInt(this.CurrentMaintain["startno"]);
            base.ClickCopyAfter();
            this.CurrentMaintain["ID"] = string.Empty;
            this.CurrentMaintain["Cdate"] = DateTime.Now;
            int startno = this.startNo_Function(this.CurrentMaintain["OrderID"].ToString());
            this.CurrentMaintain["startno"] = startno;
            #region 清除Detail Bundleno,ID
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["Bundleno"] = string.Empty;
                dr["ID"] = string.Empty;
                dr["BundleGroup"] = MyUtility.Convert.GetInt(dr["BundleGroup"]) + startno - oldstartno;
            }

            foreach (DataRow dr in this.bundle_Detail_allpart_Tb.Rows)
            {
                dr["ID"] = string.Empty;
            }

            foreach (DataRow dr in this.bundle_Detail_Art_Tb.Rows)
            {
                dr["Bundleno"] = string.Empty;
                dr["ID"] = string.Empty;
            }

            foreach (DataRow dr in this.bundle_Detail_Qty_Tb.Rows)
            {
                dr["ID"] = string.Empty;
            }
            #endregion
        }

        private void numBeginBundleGroup_Validated(object sender, EventArgs e)
        {
            decimal no = (decimal)this.numBeginBundleGroup.Value;
            decimal oldvalue = (decimal)this.numBeginBundleGroup.OldValue;
            decimal nGroup = no - oldvalue;
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + nGroup;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["article"]) || MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
            {
                this.ShowErr("Fabric Combo and Article can't empty!");
                return;
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            this.detailgrid.ValidateControl();
            var frm = new Sci.Production.Cutting.P10_1_Generate(this.CurrentMaintain, dt, this.bundle_Detail_allpart_Tb, this.bundle_Detail_Art_Tb, this.bundle_Detail_Qty_Tb);
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
                  update BundleReplacement_Detail set PrintDate = '{0}' where ID = '{1}';",
                dtn, this.CurrentMaintain["ID"]);
            DBProxy.Current.Execute(null, sqlcmd);
            return true;
        }

        int at;

        private void txtArticle_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            string selectCommand, sqlwhere = string.Empty;
            Sci.Win.Tools.SelectItem item;
            if (!this.EditMode)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
            {
                sqlwhere = string.Format(" and w.FabricCombo = '{0}'", this.txtFabricCombo.Text);
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["Orderid"]))
            {
                string scount = string.Format(
                    @"
select distinct count(Article)
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.OrderID = '{0}' and w.mDivisionid = '{1}' {2}",
                    this.CurrentMaintain["Orderid"].ToString(), this.keyword, sqlwhere);
                string count = MyUtility.GetValue.Lookup(scount, null);
                if (count != "0")
                {
                    selectCommand = string.Format(
                        @"
select distinct Article ,w.Colorid
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.OrderID = '{0}' and w.mDivisionid = '{1}' {2}",
                        this.CurrentMaintain["Orderid"].ToString(), this.keyword, sqlwhere);
                    this.at = 1;
                }
                else
                {
                    selectCommand = string.Format(
                        @"
SELECT OA.Article , color.ColorID
FROM Order_Article OA WITH (NOLOCK)
CROSS APPLY (SELECT TOP 1 ColorID FROM Order_ColorCombo OCC WITH (NOLOCK) WHERE OCC.Id=OA.id and OCC.Article=OA.Article) color
where OA.id = '{0}'
ORDER BY Seq",
                        this.CurrentMaintain["Orderid"].ToString());
                }

                item = new Sci.Win.Tools.SelectItem(selectCommand, "20", this.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["article"] = item.GetSelecteds()[0]["Article"].ToString().TrimEnd();
                this.CurrentMaintain["Colorid"] = item.GetSelecteds()[0]["Colorid"].ToString().TrimEnd();
                this.at = 2;
            }
        }

        private void txtArticle_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string newvalue = this.txtArticle.Text;
            if (this.txtArticle.OldValue.ToString() == newvalue)
            {
                return;
            }

            string sql;
            DataTable dtTEMP;

            if (!MyUtility.Check.Empty(this.CurrentMaintain["Orderid"]))
            {
                if (this.at == 1)
                {
                    sql = string.Format(@"select article from Order_Qty WITH (NOLOCK) where Id='{0}' and Article='{1}'", this.CurrentMaintain["Orderid"], newvalue);
                    if (DBProxy.Current.Select(null, sql, out dtTEMP))
                    {
                        if (dtTEMP.Rows.Count == 0)
                        {
                            this.txtArticle.Text = string.Empty;
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("<Article> can't find !!");
                            return;
                        }
                    }
                }
                else
                {
                    sql = string.Format(@"select OA.Article from Order_Article OA WITH (NOLOCK) where OA.id = '{0}'and OA.Article  ='{1}'", this.CurrentMaintain["Orderid"], newvalue);
                    if (DBProxy.Current.Select(null, sql, out dtTEMP))
                    {
                        if (dtTEMP.Rows.Count == 0)
                        {
                            this.txtArticle.Text = string.Empty;
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
            if (!this.EditMode)
            {
                return;
            }

            string sql = string.Format(
                @"Select ID,FactoryID,Description  From SewingLine WITH (NOLOCK) 
                                        where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}')", Sci.Env.User.Keyword);
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sql, "2,8,16", this.Text, false, ",");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtLineNo.Text = item.GetSelectedString();
        }

        private void BtnBatchDelete_Click(object sender, EventArgs e)
        {
            var form = new P10_1_BatchDelete();
            form.ShowDialog();
            this.RenewData();
        }

        private void txtLineNo_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string newvalue = this.txtLineNo.Text;
            if (this.txtLineNo.OldValue.ToString() == newvalue)
            {
                return;
            }

            string sql = string.Format(
                @"Select ID  From SewingLine WITH (NOLOCK)  
                    where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}') and ID='{1}'", Sci.Env.User.Keyword, newvalue);
            string tmp = MyUtility.GetValue.Lookup(sql);
            if (string.IsNullOrWhiteSpace(tmp))
            {
                this.txtLineNo.Text = string.Empty;
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Sewing Line> : {0} not found!!!", newvalue));
                return;
            }
        }

        protected override DualResult ClickDeletePost()
        {
            string id = this.CurrentMaintain["ID"].ToString();
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
