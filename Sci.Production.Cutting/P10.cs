using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static Sci.Production.Automation.Guozi_AGV;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P10 : Win.Tems.Input6
    {
        private readonly string keyword = Env.User.Keyword;
        private DataTable bundle_Detail_allpart_Tb;
        private DataTable bundle_Detail_Art_Tb;
        private DataTable bundle_Detail_Qty_Tb;
        private string WorkOrder_Ukey;
        private DataTable dtBundle_Detail_Order;

        /// <inheritdoc/>
        public P10(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();
            if (history == "0")
            {
                this.DefaultFilter = $"Orderid in (Select id from orders WITH (NOLOCK) where finished=0) and mDivisionid='{this.keyword}'";
            }
            else
            {
                this.DefaultFilter = $"Orderid in (Select id from orders WITH (NOLOCK) where finished=1) and mDivisionid='{this.keyword}'";
            }

            this.DefaultWhere = $@"(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = Bundle.Orderid)  = '{Env.User.Factory}'";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string querySql = $@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory WITH (NOLOCK)
where MDivisionID = '{Env.User.Keyword}'";
            DBProxy.Current.Select(null, querySql, out DataTable queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);

            // 取得當前登入工廠index
            for (int i = 0; i < queryDT.Rows.Count; i++)
            {
                if (string.Compare(queryDT.Rows[i]["FTYGroup"].ToString(), Env.User.Factory) == 0)
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
                        this.DefaultWhere = $"(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = Bundle.Orderid)  = '{this.queryfors.SelectedValue}'";
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings bundleno = new DataGridViewGeneratorTextColumnSettings();
            bundleno.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                DataTable curdtBundle_Detail_Order = this.GetBundle_Detail_Order(dr["Bundleno"].ToString());
                this.ShowBundle_Detail_Order(curdtBundle_Detail_Order);
            };

            bundleno.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                DataTable curdtBundle_Detail_Order = this.GetBundle_Detail_Order(dr["Bundleno"].ToString());
                if (curdtBundle_Detail_Order.Rows.Count > 1)
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("BundleGroup", header: "Group", width: Widths.AnsiChars(4), integer_places: 5, iseditingreadonly: true)
            .Text("Tone", header: "Tone", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .Text("Bundleno", header: "Bundle No", width: Widths.AnsiChars(10), iseditingreadonly: true, settings: bundleno)
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

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            #region 主 Detail
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmdsql = $@"
Select a.*	,s.subProcessid, ps.NoBundleCardAfterSubprocess_String, nbs.PostSewingSubProcess_String, ukey1 = 0,tmpseq=0
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
where a.id = '{masterID}' 
order by bundlegroup,bundleno";
            this.DetailSelectCommand = cmdsql;
            #endregion

            #region 先撈出底層其他Table
            if (!this.IsDetailInsertByCopy)
            {
                string allPart_cmd = $@"Select sel = 0,*, ukey1 = 0, annotation = '' from  Bundle_Detail_Allpart  WITH (NOLOCK)  Where id ='{masterID}'";
                string art_cmd = $@"Select b.*, ukey1 = 0,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_Detail a WITH (NOLOCK) inner join Bundle_Detail_art b WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id Where a.id ='{masterID}' ";
                string qty_cmd = $@"Select No = 0, a.* from Bundle_Detail_qty a WITH (NOLOCK) Where a.id ='{masterID}'";
                DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out this.bundle_Detail_allpart_Tb);
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

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain == null)
            {
                return;
            }

            this.QueryTable();
            string orderid = this.CurrentMaintain["OrderID"].ToString();
            string cutref = (this.CurrentMaintain["Cutref"] == null) ? string.Empty : this.CurrentMaintain["Cutref"].ToString();
            string cuttingid = string.Empty;
            if (DBProxy.Current.Select(null, $"Select * from Orders WITH (NOLOCK) Where id='{orderid}'", out DataTable orders))
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

            string estcutdate = MyUtility.GetValue.Lookup($"Select estcutdate from workorder WITH (NOLOCK) where id='{cuttingid}' and cutref = '{cutref}'", null);
            if (!MyUtility.Check.Empty(estcutdate))
            {
                this.displayEstCutDate.Text = Convert.ToDateTime(estcutdate).ToString("yyyy/MM/dd");
            }

            int qty;
            if (this.bundle_Detail_Qty_Tb.Rows.Count == 0)
            {
                qty = 0;
            }
            else
            {
                qty = Convert.ToInt16(this.bundle_Detail_Qty_Tb.Compute("Sum(Qty)", string.Empty));
            }

            this.numQtyperBundleGroup.Value = qty;

            if (!MyUtility.Check.Empty(this.CurrentMaintain["printdate"]))
            {
                DateTime? lastTime = (DateTime?)this.CurrentMaintain["printdate"];
                string ftyLastupdate = lastTime == null ? string.Empty : ((DateTime)lastTime).ToString("yyyy/MM/dd HH:mm:ss");
                this.displayPrintDate.Text = ftyLastupdate;
            }
            else
            {
                this.displayPrintDate.Text = string.Empty;
            }

            this.GetFabricKind();

            this.btnSPs.ForeColor = this.dtBundle_Detail_Order.Rows.Count > 1 ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
        }

        private void QueryTable()
        {
            string masterID = (this.CurrentMaintain == null) ? string.Empty : this.CurrentMaintain["id"].ToString();
            string allPart_cmd = $@"Select 0 as sel,*, 0 as ukey1,'' as annotation from Bundle_Detail_Allpart  WITH (NOLOCK) Where id ='{masterID}' ";
            string art_cmd = $@"Select b.*, 0 as ukey1,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_Detail a WITH (NOLOCK) ,Bundle_Detail_art b WITH (NOLOCK) Where a.id ='{masterID}' and a.Bundleno = b.bundleno and a.id = b.id";
            string qty_cmd = $@"Select 0 as No,a.* from Bundle_Detail_qty a WITH (NOLOCK) Where a.id ='{masterID}'";
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
                DataRow[] allar = this.bundle_Detail_allpart_Tb.Select($"id='{dr["ID"]}'");
                if (allar.Length > 0)
                {
                    foreach (DataRow dr2 in allar)
                    {
                        dr2["ukey1"] = ukey;
                    }
                }

                DataRow[] artar = this.bundle_Detail_Art_Tb.Select($"Bundleno='{dr["Bundleno"]}'");
                if (allar.Length > 0)
                {
                    foreach (DataRow dr2 in artar)
                    {
                        dr2["ukey1"] = ukey;
                    }
                }
            }

            this.QueryBundle_Detail_Order();
        }

        private void QueryBundle_Detail_Order()
        {
            string sqlcmd = $@"select distinct [SP#] = OrderID from Bundle_Detail_Order where ID = '{this.CurrentMaintain["ID"]}' order by OrderID";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtBundle_Detail_Order);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Cdate"] = DateTime.Today;
            this.CurrentMaintain["mDivisionid"] = this.keyword;
            this.displayEstCutDate.Text = string.Empty;
            this.displayPrintDate.Text = string.Empty;
            this.bundle_Detail_allpart_Tb.Clear();
            this.bundle_Detail_Art_Tb.Clear();
            this.bundle_Detail_Qty_Tb.Clear();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.QueryBundle_Detail_Order();
            if (this.CurrentMaintain != null && this.dtBundle_Detail_Order.Rows.Count > 1)
            {
                MyUtility.Msg.WarningBox($"Cannot edit if SP# more than one, please delete this ID:{this.CurrentMaintain["ID"]} and create a new one.");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
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

            DualResult result = DBProxy.Current.Select(null, $"select CuttingP10mustCutRef from system", out DataTable dt);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (MyUtility.Convert.GetBool(dt.Rows[0]["CuttingP10mustCutRef"]) && MyUtility.Check.Empty(this.CurrentMaintain["Cutref"]))
            {
                MyUtility.Msg.WarningBox("<Cut Ref#> can't be empty!!");
                return false;
            }

            if (this.IsDetailInserting)
            {
                string keyword = this.keyword + "BC";
                string cid = MyUtility.GetValue.GetID(keyword, "Bundle", Convert.ToDateTime(this.CurrentMaintain["cdate"]), sequenceMode: 2);
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

        /// <inheritdoc/>
        protected override DualResult ClickSavePre()
        {
            #region 填入Bundleno
            int drcount = this.DetailDatas.Count;
            IList<string> cListBundleno;
            cListBundleno = MyUtility.GetValue.GetBatchID(string.Empty, "Bundle_Detail", MyUtility.Check.Empty(this.CurrentMaintain["cDate"]) ? default(DateTime) : Convert.ToDateTime(this.CurrentMaintain["cDate"]), 3, "Bundleno", batchNumber: drcount, sequenceMode: 2);
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
                    roway = this.bundle_Detail_Art_Tb.Select($"ukey1 = {dr["ukey1"]}");
                    foreach (DataRow dr2 in roway)
                    {
                        dr2["Bundleno"] = cListBundleno[nCount];
                    }

                    nCount++;
                }
            }
            #endregion
            return base.ClickSavePre();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePost()
        {
            string allpart_cmd = string.Empty, art_cmd1 = string.Empty, qty_cmd1 = string.Empty;
            #region 先撈出實體Table 為了平行判斷筆數 DataTable allparttmp, arttmp, qtytmp
            string masterID = (this.CurrentMaintain == null) ? string.Empty : this.CurrentMaintain["id"].ToString();

            // 直接撈Bundle_Detail_Allpart就行,不然在算新舊資料筆數來判斷新刪修時,會因為表頭表身join造成count過多
            string allPart_cmd = $@"select * from Bundle_Detail_Allpart WITH (NOLOCK) where id='{masterID}'  ";
            string art_cmd = $@"Select b.* from Bundle_Detail_art b WITH (NOLOCK) left join Bundle_Detail a WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id where b.id='{masterID}'";
            string qty_cmd = $@"Select a.* from Bundle_Detail_qty a WITH (NOLOCK) Where a.id ='{masterID}'";
            DualResult dRes = DBProxy.Current.Select(null, allPart_cmd, out DataTable allparttmp);
            if (!dRes)
            {
                this.ShowErr(allPart_cmd, dRes);
            }

            dRes = DBProxy.Current.Select(null, art_cmd, out DataTable arttmp);
            if (!dRes)
            {
                this.ShowErr(art_cmd, dRes);
            }

            dRes = DBProxy.Current.Select(null, qty_cmd, out DataTable qtytmp);
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
            allpart_cmd += $@"Delete from bundle_Detail_allpart where id ='{masterID}'";
            foreach (DataRow dr in this.bundle_Detail_allpart_Tb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    allpart_cmd += $@"insert into bundle_Detail_allpart(ID,PatternCode,PatternDesc,Parts,isPair,Location) values('{this.CurrentMaintain["ID"]}','{dr["PatternCode"]}','{dr["PatternDesc"]}','{dr["Parts"]}','{dr["isPair"]}','{dr["Location"]}');";
                }
            }
            #endregion

            #region 處理Bundle_Detail_Art 修改版
            /*
            * 先刪除原有資料
            * 再新增更改的資料
            */
            art_cmd1 += $@"
select ID,Ukey into #tmpOldBundle_Detail_Art from bundle_Detail_Art with (nolock) where  id='{masterID}'
delete from bundle_Detail_Art where id='{masterID}'";

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
            // 處理Bundle_Detail_Art
            foreach (DataRow dr in bundle_Detail_Art_Tb_copy.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    art_cmd1 += $@"insert into bundle_Detail_Art(ID,Bundleno,PatternCode,SubProcessid,NoBundleCardAfterSubprocess,PostSewingSubProcess) 
                        values('{this.CurrentMaintain["ID"]}','{dr["Bundleno"]}','{dr["PatternCode"]}','{dr["SubProcessid"]}',{(MyUtility.Convert.GetBool(dr["NoBundleCardAfterSubprocess"]) ? 1 : 0)},{(MyUtility.Convert.GetBool(dr["PostSewingSubProcess"]) ? 1 : 0)});";
                }
            }

            art_cmd1 += $@"select Ukey  
from #tmpOldBundle_Detail_Art tda
where  not exists(select 1 from bundle_Detail_Art bda with (nolock) where tda.ID = bda.ID and tda.Ukey = bda.Ukey) ";
            #endregion

            #region 處理Bundle_Detail_Qty 修改版

            /*
           * 先刪除原有資料
           * 再新增更改的資料
           */
            qty_cmd1 += $@"Delete from bundle_Detail_Qty where id ='{masterID}'";
            foreach (DataRow dr in this.bundle_Detail_Qty_Tb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    qty_cmd1 += $@"insert into bundle_Detail_Qty(ID,SizeCode,Qty) 
                    values('{this.CurrentMaintain["ID"]}','{dr["sizecode"]}',{dr["Qty"]});";
                }
            }

            #endregion

            #region [Bundle_Detail_Order] 不處理(多筆P15)狀況
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    qty_cmd1 += $@"
Delete from [Bundle_Detail_Order] where BundleNo ='{dr["BundleNo", DataRowVersion.Original]}';";
                }

                if (dr.RowState == DataRowState.Added)
                {
                    qty_cmd1 += $@"
INSERT INTO [dbo].[Bundle_Detail_Order]([ID],[BundleNo],[OrderID],[Qty])
VALUES('{this.CurrentMaintain["ID"]}','{dr["BundleNo"]}','{this.CurrentMaintain["OrderID"]}','{dr["Qty"]}');
";
                }

                if (dr.RowState == DataRowState.Modified)
                {
                    qty_cmd1 += $@"
update [Bundle_Detail_Order] set qty = {dr["qty"]} where BundleNo ='{dr["BundleNo"]}' and (select count(1) from Bundle_Detail_Order where  BundleNo ='{dr["BundleNo"]}') = 1;";
                }
            }
            #endregion

            DataTable deleteBundle_Detail_Art = new DataTable();
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

                if (!MyUtility.Check.Empty(art_cmd1))
                {
                    if (!(upResult = DBProxy.Current.Select(null, art_cmd1, out deleteBundle_Detail_Art)))
                    {
                        return upResult;
                    }
                }

                if (!MyUtility.Check.Empty(qty_cmd1))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, qty_cmd1)))
                    {
                        return upResult;
                    }
                }

                scope.Complete();
            }

            #region sent data to GZ WebAPI
            string compareCol = "CutRef,OrderID,Article,PatternPanel,FabricPanelCode,SewingLineID,AddDate";
            string compareDetailCol = "ID,BundleNo,PatternCode,PatternDesc,BundleGroup,SizeCode,Qty,SubProcessID";
            var listChangedDetail = ((DataTable)this.detailgridbs.DataSource).AsEnumerable();
            if (this.CurrentMaintain.CompareDataRowVersionValue(compareCol))
            {
                listChangedDetail = listChangedDetail
                    .Where(s => s.RowState == DataRowState.Added || s.RowState == DataRowState.Modified);
            }
            else
            {
                listChangedDetail = listChangedDetail
                        .Where(s => s.RowState == DataRowState.Added || (s.RowState == DataRowState.Modified && s.CompareDataRowVersionValue(compareDetailCol)));
            }

            if (listChangedDetail.Any())
            {
                List<BundleToAGV_PostBody> listBundleToAGV_PostBody = listChangedDetail.Select(
                    dr => new BundleToAGV_PostBody()
                    {
                        ID = dr["ID"].ToString(),
                        POID = this.CurrentMaintain["POID"].ToString(),
                        BundleNo = dr["BundleNo"].ToString(),
                        CutRef = this.CurrentMaintain["CutRef"].ToString(),
                        OrderID = this.CurrentMaintain["OrderID"].ToString(),
                        Article = this.CurrentMaintain["Article"].ToString(),
                        PatternPanel = this.CurrentMaintain["PatternPanel"].ToString(),
                        FabricPanelCode = this.CurrentMaintain["FabricPanelCode"].ToString(),
                        PatternCode = dr["PatternCode"].ToString(),
                        PatternDesc = dr["PatternDesc"].ToString(),
                        BundleGroup = (decimal)dr["BundleGroup"],
                        SizeCode = dr["SizeCode"].ToString(),
                        Qty = (decimal)dr["Qty"],
                        SewingLineID = this.CurrentMaintain["SewingLineID"].ToString(),
                        AddDate = (DateTime?)this.CurrentMaintain["AddDate"],
                    })
                    .ToList();

                Task.Run(() => new Guozi_AGV().SentBundleToAGV(() => listBundleToAGV_PostBody))
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            var listDeletedDetail = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(s => s.RowState == DataRowState.Deleted && s["BundleNo", DataRowVersion.Original] != DBNull.Value);

            if (listDeletedDetail.Any())
            {
                DataTable deletedDetail = ((DataTable)this.detailgridbs.DataSource).Clone();

                deletedDetail = listDeletedDetail.Select(s =>
                {
                    DataRow dr = deletedDetail.NewRow();
                    dr["BundleNo"] = s["BundleNo", DataRowVersion.Original];
                    return dr;
                })
                .CopyToDataTable();
                Task.Run(() => new Guozi_AGV().SentDeleteBundle(deletedDetail));
            }

            if (deleteBundle_Detail_Art.Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentDeleteBundle_SubProcess(deleteBundle_Detail_Art));
            }
            #endregion

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
        }

        private void Clear()
        {
            this.displaySeason.Text = string.Empty;
            this.displayStyle.Text = string.Empty;
            this.displayPrintDate.Text = string.Empty;
            this.CurrentMaintain["Cutno"] = 0;
            this.CurrentMaintain["subCutno"] = string.Empty;
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
            this.CurrentMaintain["cutref"] = string.Empty;
            this.CurrentMaintain["ITEM"] = string.Empty;
            this.txtFabricPanelCode.Text = string.Empty;
            this.dispFabricKind.Text = string.Empty;
            this.DetailDatas.Clear();
            this.bundle_Detail_allpart_Tb.Clear();
            this.bundle_Detail_Art_Tb.Clear();
            this.bundle_Detail_Qty_Tb.Clear();
            this.dtBundle_Detail_Order.Clear();
            this.WorkOrder_Ukey = string.Empty;
            this.CurrentMaintain.EndEdit();
        }

        private void TxtCutRef_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string newvalue = this.txtCutRef.Text;
            if (this.txtCutRef.OldValue.ToString() == newvalue)
            {
                return;
            }

            if (this.txtCutRef.Text == string.Empty)
            {
                int oldstartno = MyUtility.Convert.GetInt(this.CurrentMaintain["startno"]);
                this.Clear();
                int diffGroup = MyUtility.Convert.GetInt(this.CurrentMaintain["startno"]) - oldstartno;
                this.DetailDatas.ToList().ForEach(dr => dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + diffGroup);
                return;
            }

            string cmd = $@"
Select a.*
--,substring(b.Sewline,1,charindex(',',b.Sewline,1)) as Sewline 
,b.Sewline
,b.poid,b.seasonid,b.styleid,b.styleukey,b.factoryid,
(
    Select Top(1) OrderID
    From Workorder_Distribute WD WITH (NOLOCK) 
    Where a.ukey =WD.workorderukey --and a.orderid=WD.orderid
) as Workorder_Distribute_OrderID,

    (
    Select d.SizeCode+'/' 
    From Workorder_SizeRatio d WITH (NOLOCK) 
    Where a.ukey =d.workorderukey
    For XML path('')
) as SizeCode,
    (
    Select convert(varchar,Qty)+'/' 
    From Workorder_SizeRatio e WITH (NOLOCK) 
    Where a.ukey =e.workorderukey
    For XML path('')
) as Ratio,
(
    Select Top(1) Article
    From Workorder_Distribute f WITH (NOLOCK) 
    Where a.ukey =f.workorderukey --and a.orderid=f.orderid
) as article,
(
    Select count(id)
    From Workorder_Distribute g WITH (NOLOCK) 
    Where a.ukey =g.workorderukey and g.OrderID=(Select Top(1) OrderID From Workorder_Distribute WD WITH (NOLOCK) Where a.ukey =WD.workorderukey)
) as Qty
From workorder a WITH (NOLOCK) ,orders b WITH (NOLOCK) 
Where a.cutref='{this.txtCutRef.Text}' and a.mDivisionid = '{this.keyword}' and a.orderid = b.id";
            if (!MyUtility.Check.Seek(cmd, out DataRow cutdr, null))
            {
                this.Clear();
                e.Cancel = true;
                MyUtility.Msg.WarningBox("<Cut Ref#> data not found!");
                return;
            }
            else
            {
                this.CurrentMaintain["Cutno"] = Convert.ToInt32(cutdr["Cutno"].ToString());
                this.CurrentMaintain["sewinglineid"] = cutdr["Sewline"].Empty() ? string.Empty : cutdr["Sewline"].ToString().Substring(0, 2);
                this.CurrentMaintain["OrderID"] = cutdr["Workorder_Distribute_OrderID"].ToString();    // cutdr["OrderID"].ToString()
                this.CurrentMaintain["POID"] = cutdr["POID"].ToString();
                this.CurrentMaintain["PatternPanel"] = cutdr["Fabriccombo"].ToString();
                this.CurrentMaintain["Sizecode"] = cutdr["Sizecode"].ToString().Substring(0, cutdr["Sizecode"].ToString().Length - 1);
                this.CurrentMaintain["Ratio"] = cutdr["Ratio"].ToString().Substring(0, cutdr["Ratio"].ToString().Length - 1);
                this.CurrentMaintain["Article"] = cutdr["Article"].ToString();
                this.CurrentMaintain["Colorid"] = cutdr["Colorid"].ToString();
                this.CurrentMaintain["Qty"] = cutdr["Qty"];

                this.CurrentMaintain["FabricPanelCode"] = cutdr["FabricPanelCode"].ToString();
                this.displaySeason.Text = cutdr["Seasonid"].ToString();
                this.displayStyle.Text = cutdr["Styleid"].ToString();
                this.displayEstCutDate.Text = MyUtility.Check.Empty(cutdr["Estcutdate"]) ? string.Empty : ((DateTime)cutdr["Estcutdate"]).ToString("yyyy/MM/dd");

                string cellid = MyUtility.GetValue.Lookup("SewingCell", this.CurrentMaintain["sewinglineid"].ToString() + cutdr["factoryid"].ToString(), "SewingLine", "ID+factoryid");
                this.CurrentMaintain["SewingCell"] = cellid;
                this.CurrentMaintain["startno"] = this.StartNo_Function(this.CurrentMaintain["POID"].ToString());

                string item_cmd = $"Select a.Name from Reason a WITH (NOLOCK) , Style b WITH (NOLOCK) where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{cutdr["styleukey"]}' and b.ApparelType = a.id";
                string item = MyUtility.GetValue.Lookup(item_cmd, null);
                this.CurrentMaintain["ITEM"] = item;
                this.CurrentMaintain["Cutref"] = newvalue;
                /*
                 *如果相同Refno 卻有不同的workorder.ukey
                 *就需要包含所有ukey
                 *避免一張馬克兩個Article 在Validating 時會判斷出錯
                 */
                this.WorkOrder_Ukey = string.Empty;
                foreach (DataRow dr in cutdr.Table.Rows)
                {
                    this.WorkOrder_Ukey = this.WorkOrder_Ukey + dr["Ukey"].ToString() + ",";
                }

                this.GetFabricKind();
                this.CurrentMaintain["SubCutNo"] = Prgs.GetSubCutNo(newvalue, cutdr["Fabriccombo"].ToString(), cutdr["FabricPanelCode"].ToString(), cutdr["Cutno"].ToString());
                this.CurrentMaintain.EndEdit();
            }

            ((DataTable)this.detailgridbs.DataSource).Clear();
        }

        private void TxtSPNo_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Cutref"]) || MyUtility.Check.Empty(this.CurrentMaintain["POID"]))
            {
                return;
            }

            string selectCommand = $@"
select distinct wd.orderid 
from workorder w WITH (NOLOCK)
inner join workorder_distribute wd WITH (NOLOCK) on wd.workorderukey = w.ukey
inner join orders o with(nolock) on o.Cuttingsp = w.id
where  wd.orderid <> 'EXCESS'
and w.cutref = '{this.CurrentMaintain["Cutref"]}'
and o.id = '{this.CurrentMaintain["POID"]}'
";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(selectCommand, "20", this.Text);
            if (item.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            this.txtSPNo.Text = item.GetSelectedString();
        }

        private void TxtSPNo_Validating(object sender, CancelEventArgs e)
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
            if (!MyUtility.Check.Empty(this.CurrentMaintain["Cutref"]))
            {
                string findcuttingid = $@"select id from workorder where cutref = '{this.CurrentMaintain["Cutref"]}' and MDivisionId = '{Env.User.Keyword}' ";
                string cuttingid = MyUtility.GetValue.Lookup(findcuttingid);
                if (cuttingsp.Trim() != cuttingid.Trim())
                {
                    this.txtSPNo.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("<Cutref> is different.");
                    return;
                }

                string work_cmd = $"Select * from workorder a WITH (NOLOCK) ,workorder_Distribute b WITH (NOLOCK) Where a.ukey = b.workorderukey and a.cutref = '{this.CurrentMaintain["Cutref"]}' and b.orderid ='{newvalue}' and a.MDivisionId = '{Env.User.Keyword}' and b.orderid <> 'EXCESS'";
                if (DBProxy.Current.Select(null, work_cmd, out DataTable articleTb))
                {
                    if (articleTb.Rows.Count == 0)
                    {
                        this.txtSPNo.Text = string.Empty;
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("<Cutref> is different.");
                        return;
                    }

                    this.CurrentMaintain["Qty"] = articleTb.Rows.Count; // 一筆distribute 表示一個bundle
                    this.CurrentMaintain["article"] = articleTb.Rows[0]["Article"].ToString().TrimEnd();
                    this.CurrentMaintain["Colorid"] = articleTb.Rows[0]["Colorid"].ToString().TrimEnd();
                }

                this.CurrentMaintain["OrderID"] = newvalue;
            }

            // Issue#969 當CutRef# 為空時，SP No 清空時，清空MasterID與Item、Line
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
                string selectCommand = $"select a.* from orders a WITH (NOLOCK) where a.id = '{newvalue}' and mDivisionid='{this.keyword}' ";
                if (MyUtility.Check.Seek(selectCommand, out DataRow cutdr, null))
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
                    string item_cmd = $"Select a.Name from Reason a WITH (NOLOCK) , Style b WITH (NOLOCK)  where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{cutdr["styleukey"]}' and b.ApparelType = a.id";
                    string item = MyUtility.GetValue.Lookup(item_cmd, null);
                    this.CurrentMaintain["ITEM"] = item;
                    #endregion
                    #region Cell
                    if (!MyUtility.Check.Empty(cutdr["sewline"].ToString()))
                    {
                        string cellid = MyUtility.GetValue.Lookup("SewingCell", this.CurrentMaintain["sewinglineid"].ToString() + cutdr["Factoryid"].ToString(), "SewingLine", "ID+factoryid");

                        this.CurrentMaintain["SewingCell"] = cellid;
                    }
                    #endregion

                    #region startno & BundleGroup
                    int startno = this.StartNo_Function(this.CurrentMaintain["POID"].ToString());
                    int diffGroup = startno - MyUtility.Convert.GetInt(this.CurrentMaintain["startno"]);
                    this.CurrentMaintain["startno"] = startno;
                    this.DetailDatas.ToList().ForEach(dr => dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + diffGroup);
                    #endregion

                    #region Article colorid
                    if (MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
                    {
                        string colorComb_cmd = $"Select top(1) article,colorid from order_colorcombo WITH (NOLOCK) where id ='{cutdr["POID"]}' and patternpanel = '{this.CurrentMaintain["PatternPanel"]}'";
                        if (MyUtility.Check.Seek(colorComb_cmd, out DataRow colordr))
                        {
                            this.CurrentMaintain["Article"] = colordr["Article"];
                            this.CurrentMaintain["colorid"] = colordr["colorid"];
                        }
                    }
                    #endregion
                }
            }

            this.GetFabricKind();
        }

        private void BtnGarmentList_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.CurrentMaintain["poid"].ToString(), "Orders", "ID");
            var sizelist = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            PublicForm.GarmentList callNextForm =
    new PublicForm.GarmentList(ukey, this.CurrentMaintain["poid"].ToString(), this.txtCutRef.Text, sizelist);
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
        }

        private int StartNo_Function(string poid) // Start No 計算
        {
            string sqlcmd = $@"
select isnull(MAX(bd.BundleGroup), 0) + 1
from Bundle b with(nolock)
inner join Bundle_Detail bd with(nolock) on bd.id = b.id
where b.poid = '{poid}'
";
            return MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(sqlcmd));
        }

        /// <inheritdoc/>
        protected override void ClickCopyAfter()
        {
            base.ClickCopyAfter();
            this.CurrentMaintain["ID"] = string.Empty;
            this.CurrentMaintain["Cdate"] = DateTime.Now;
            int startno = this.StartNo_Function(this.CurrentMaintain["POID"].ToString());
            int diffGroup = startno - MyUtility.Convert.GetInt(this.CurrentMaintain["startno"]);
            this.CurrentMaintain["startno"] = startno;
            #region 清除Detail Bundleno,ID
            foreach (DataRow dr in this.DetailDatas)
            {
                // 複製後的 BundleGroup ,加上 startno 新舊相差值
                dr["BundleGroup"] = MyUtility.Convert.GetInt(dr["BundleGroup"]) + diffGroup;
                dr["Bundleno"] = string.Empty;
                dr["ID"] = string.Empty;
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

        private void NumBeginBundleGroup_Validated(object sender, EventArgs e)
        {
            decimal no = (decimal)this.numBeginBundleGroup.Value;
            decimal oldvalue = (decimal)this.numBeginBundleGroup.OldValue;
            decimal nGroup = no - oldvalue;
            foreach (DataRow dr in this.DetailDatas)
            {
                dr["BundleGroup"] = Convert.ToDecimal(dr["BundleGroup"]) + nGroup;
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["article"]) || MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
            {
                this.ShowErr("Fabric Combo and Article can't empty!");
                return;
            }

            DataTable dt = (DataTable)this.detailgridbs.DataSource;
            this.detailgrid.ValidateControl();
            var frm = new P10_Generate(this.CurrentMaintain, dt, this.bundle_Detail_allpart_Tb, this.bundle_Detail_Art_Tb, this.bundle_Detail_Qty_Tb);
            frm.ShowDialog(this);
            dt.DefaultView.Sort = "BundleGroup";
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P10_Print p = new P10_Print(this.CurrentMaintain);
            p.ShowDialog();
            string dtn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.displayPrintDate.Text = dtn;
            string topID = this.CurrentMaintain["ID"].ToString();
            this.ReloadDatas();
            int newDataIdx = this.gridbs.Find("ID", topID);
            this.gridbs.Position = newDataIdx;
            return true;
        }

        private int at;

        private void TxtArticle_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            string selectCommand, sqlwhere = string.Empty;
            Win.Tools.SelectItem item;
            if (!this.EditMode)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
            {
                sqlwhere = $" and w.FabricCombo = '{this.txtFabricCombo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["cutref"]))
            {
                selectCommand = $@"
select distinct Article ,w.Colorid
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.cutref='{this.CurrentMaintain["cutref"].ToString()}' and w.mDivisionid = '{this.keyword}' {sqlwhere}";
                item = new Win.Tools.SelectItem(selectCommand, "20", this.Text);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel)
                {
                    return;
                }

                this.CurrentMaintain["article"] = item.GetSelecteds()[0]["Article"].ToString().TrimEnd();
                this.CurrentMaintain["Colorid"] = item.GetSelecteds()[0]["Colorid"].ToString().TrimEnd();
            }
            else
            {
                if (!MyUtility.Check.Empty(this.CurrentMaintain["Orderid"]))
                {
                    string scount = $@"
select distinct count(Article)
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.OrderID = '{this.CurrentMaintain["Orderid"].ToString()}' and w.mDivisionid = '{this.keyword}' {sqlwhere}";
                    string count = MyUtility.GetValue.Lookup(scount, null);
                    if (count != "0")
                    {
                        selectCommand = $@"
select distinct Article ,w.Colorid
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.OrderID = '{this.CurrentMaintain["Orderid"].ToString()}' and w.mDivisionid = '{this.keyword}' {sqlwhere}";
                        this.at = 1;
                    }
                    else
                    {
                        selectCommand = $@"
SELECT OA.Article , color.ColorID
FROM Order_Article OA WITH (NOLOCK)
CROSS APPLY (SELECT TOP 1 ColorID FROM Order_ColorCombo OCC WITH (NOLOCK) WHERE OCC.Id=OA.id and OCC.Article=OA.Article) color
where OA.id = '{this.CurrentMaintain["Orderid"].ToString()}'
ORDER BY Seq";
                    }

                    item = new Win.Tools.SelectItem(selectCommand, "20", this.Text);
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
        }

        private void TxtArticle_Validating(object sender, CancelEventArgs e)
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
            if (!MyUtility.Check.Empty(this.CurrentMaintain["cutref"]))
            {
                sql = $@"
select Article 
from Workorder_Distribute WITH (NOLOCK) 
where Article!='' and WorkorderUkey in ({(MyUtility.Check.Empty(this.WorkOrder_Ukey) ? string.Empty : this.WorkOrder_Ukey.Trim().Substring(0, this.WorkOrder_Ukey.Length - 1))}) and Article='{newvalue}'";
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
                if (!MyUtility.Check.Empty(this.CurrentMaintain["Orderid"]))
                {
                    if (this.at == 1)
                    {
                        sql = $@"select article from Order_Qty WITH (NOLOCK) where Id='{this.CurrentMaintain["Orderid"]}' and Article='{newvalue}'";
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
                        sql = $@"select OA.Article from Order_Article OA WITH (NOLOCK) where OA.id = '{this.CurrentMaintain["Orderid"]}'and OA.Article  ='{newvalue}'";
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
        }

        private void TxtLineNo_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string sql = $@"Select ID,FactoryID,Description  From SewingLine WITH (NOLOCK) 
                                        where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{Env.User.Keyword}')";
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sql, "2,8,16", this.Text, false, ",");
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtLineNo.Text = item.GetSelectedString();
        }

        private void BtnBatchDelete_Click(object sender, EventArgs e)
        {
            var form = new P10_BatchDelete();
            form.ShowDialog();
            this.RenewData();
        }

        private void TxtLineNo_Validating(object sender, CancelEventArgs e)
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

            string sql = $@"Select ID  From SewingLine WITH (NOLOCK)  
                    where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{Env.User.Keyword}') and ID='{newvalue}'";
            string tmp = MyUtility.GetValue.Lookup(sql);
            if (string.IsNullOrWhiteSpace(tmp))
            {
                this.txtLineNo.Text = string.Empty;
                e.Cancel = true;
                MyUtility.Msg.WarningBox($"< Sewing Line> : {newvalue} not found!!!");
                return;
            }
        }

        private void TxtFabricCombo_Validating(object sender, CancelEventArgs e)
        {
            this.GetFabricKind();
        }

        private void GetFabricKind()
        {
            string sqlcmd = $@"
SELECT TOP 1 DD.id + '-' + DD.NAME 
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
WHERE LIST.id = '{this.displayPOID.Text}'--bundle.poid 
AND LIST.patternpanel = '{this.txtFabricCombo.Text}'--bundle.patternpanel 
AND DD.[type] = 'FabricKind' 
AND DD.id = LIST.kind ";
            this.dispFabricKind.Text = MyUtility.GetValue.Lookup(sqlcmd);
        }

        /// <inheritdoc/>
        protected override DualResult ClickDeletePost()
        {
            string id = this.CurrentMaintain["ID"].ToString();
            string deleteBundleDetailQty = string.Format(
                    @"
select Ukey from Bundle_Detail_Art with (nolock) where id = '{0}'
select Ukey from Bundle_Detail_qty with (nolock) where id = '{0}'
",
                    id);

            DualResult result = DBProxy.Current.Select(null, deleteBundleDetailQty, out DataTable[] dtDeleteResults);
            if (!result)
            {
                return result;
            }

            result = new Guozi_AGV().SentDeleteBundle((DataTable)this.detailgridbs.DataSource);
            if (!result)
            {
                return result;
            }

            if (dtDeleteResults[0].Rows.Count > 0)
            {
                result = new Guozi_AGV().SentDeleteBundle_SubProcess(dtDeleteResults[0]);
                if (!result)
                {
                    return result;
                }
            }

            if (dtDeleteResults[1].Rows.Count > 0)
            {
                result = new Guozi_AGV().SentDeleteBundle_Detail_Order(dtDeleteResults[1]);
                if (!result)
                {
                    return result;
                }
            }

            deleteBundleDetailQty = string.Format(
                    @"
delete bundle where id = '{0}';
delete Bundle_Detail where id = '{0}';
delete Bundle_Detail_Art where id = '{0}';
delete Bundle_Detail_AllPart where id = '{0}';
delete Bundle_Detail_qty where id = '{0}';
delete Bundle_Detail_Order where id = '{0}';
",
                    id);
            result = DBProxy.Current.Execute(null, deleteBundleDetailQty);
            if (!result)
            {
                return result;
            }

            return base.ClickDeletePost();
        }

        private void BtnSPs_Click(object sender, EventArgs e)
        {
            this.ShowBundle_Detail_Order(this.dtBundle_Detail_Order);
        }

        private DataTable GetBundle_Detail_Order(string bundleno)
        {
            string sqlcmd = $@"
select [SP#] = OrderID, Qty from Bundle_Detail_Order where Bundleno = '{bundleno}' order by OrderID";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable curdtBundle_Detail_Order);
            if (!result)
            {
                this.ShowErr(result);
            }

            return curdtBundle_Detail_Order;
        }

        private void ShowBundle_Detail_Order(DataTable dt)
        {
            if (dt.AsEnumerable().Any())
            {
                MsgGridForm m = new MsgGridForm(dt, "SP# List")
                {
                    Width = 650,
                };
                m.grid1.Columns[0].Width = 140;
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
    }
}
