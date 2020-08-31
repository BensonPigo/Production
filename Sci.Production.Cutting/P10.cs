using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.Prg;
using Sci.Win.UI;
using static Sci.Production.Automation.Guozi_AGV;

namespace Sci.Production.Cutting
{
    public partial class P10 : Win.Tems.Input6
    {
        string keyword = Env.User.Keyword;
        string LoginId = Env.User.UserID;
        DataTable bundle_Detail_allpart_Tb;
        DataTable bundle_Detail_Art_Tb;
        DataTable bundle_Detail_Qty_Tb;
        string WorkOrder_Ukey;
        DataTable dtBundle_Detail_Order;

        public P10(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            this.InitializeComponent();
            if (history == "0")
            {
                this.DefaultFilter = string.Format("Orderid in (Select id from orders WITH (NOLOCK) where finished=0) and mDivisionid='{0}'", this.keyword);
            }
            else
            {
                this.DefaultFilter = string.Format("Orderid in (Select id from orders WITH (NOLOCK) where finished=1) and mDivisionid='{0}'", this.keyword);
            }

            this.DefaultWhere = $@"(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = Bundle.Orderid)  = '{Env.User.Factory}'";
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
where MDivisionID = '{0}'", Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
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
                        this.DefaultWhere = string.Format("(select O.FtyGroup from Orders O WITH (NOLOCK) Where O.ID = Bundle.Orderid)  = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings bundleno = new DataGridViewGeneratorTextColumnSettings();
            bundleno.EditingMouseDoubleClick += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataTable curdtBundle_Detail_Order = this.GetBundle_Detail_Order(this.CurrentDetailData["Bundleno"].ToString());
                this.ShowBundle_Detail_Order(curdtBundle_Detail_Order);
            };

            bundleno.CellFormatting += (s, e) =>
            {
                DataTable curdtBundle_Detail_Order = this.GetBundle_Detail_Order(this.CurrentDetailData["Bundleno"].ToString());
                if (curdtBundle_Detail_Order.Rows.Count > 1)
                {
                    e.CellStyle.BackColor = Color.Yellow;
                }
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Numeric("BundleGroup", header: "Group", width: Widths.AnsiChars(4), integer_places: 5, iseditingreadonly: true)
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

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            #region 主 Detail
            string masterID = (e.Master == null) ? string.Empty : e.Master["id"].ToString();
            string cmdsql = string.Format(
                @"
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
where a.id = '{0}' 
order by bundlegroup",
                masterID);
            this.DetailSelectCommand = cmdsql;
            #endregion

            #region 先撈出底層其他Table
            if (!this.IsDetailInsertByCopy)
            {
                string allPart_cmd = string.Format(@"Select sel = 0,*, ukey1 = 0, annotation = '' from  Bundle_Detail_Allpart  WITH (NOLOCK)  Where id ='{0}'", masterID);
                string art_cmd = string.Format(@"Select b.*, ukey1 = 0,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_Detail a WITH (NOLOCK) inner join Bundle_Detail_art b WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id Where a.id ='{0}' ", masterID);
                string qty_cmd = string.Format(@"Select No = 0, a.* from Bundle_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
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
            string cutref = (this.CurrentMaintain["Cutref"] == null) ? string.Empty : this.CurrentMaintain["Cutref"].ToString();
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

            string estcutdate = MyUtility.GetValue.Lookup(string.Format("Select estcutdate from workorder WITH (NOLOCK) where id='{0}' and cutref = '{1}'", cuttingid, cutref), null);
            if (!MyUtility.Check.Empty(estcutdate))
            {
                this.displayEstCutDate.Text = Convert.ToDateTime(estcutdate).ToString("yyyy/MM/dd");
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

            string factoryid = MyUtility.GetValue.Lookup(string.Format("SELECT o.FactoryID FROM Bundle b WITH (NOLOCK) inner join orders o WITH (NOLOCK) on b.Orderid=o.ID where b.Orderid='{0}'", orderid), null);

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

            this.getFabricKind();

            string sqlcmd = $@"select distinct [SP#] = OrderID from Bundle_Detail_Order where ID = '{this.CurrentMaintain["ID"]}' order by OrderID";
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out this.dtBundle_Detail_Order);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.btnSPs.ForeColor = this.dtBundle_Detail_Order.Rows.Count > 1 ? System.Drawing.Color.Blue : System.Drawing.Color.Black;
        }

        public void queryTable()
        {
            string masterID = (this.CurrentMaintain == null) ? string.Empty : this.CurrentMaintain["id"].ToString();
            string allPart_cmd = string.Format(@"Select 0 as sel,*, 0 as ukey1,'' as annotation from Bundle_Detail_Allpart  WITH (NOLOCK) Where id ='{0}' ", masterID);
            string art_cmd = string.Format(@"Select b.*, 0 as ukey1,NoBundleCardAfterSubprocess_String='',PostSewingSubProcess_String='' from Bundle_Detail a WITH (NOLOCK) ,Bundle_Detail_art b WITH (NOLOCK) Where a.id ='{0}' and a.Bundleno = b.bundleno and a.id = b.id", masterID);
            string qty_cmd = string.Format(@"Select 0 as No,a.* from Bundle_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
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
            this.displayEstCutDate.Text = string.Empty;
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

            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, $"select CuttingP10mustCutRef from system", out dt);
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
                string Keyword = this.keyword + "BC";
                string cid = MyUtility.GetValue.GetID(Keyword, "Bundle", Convert.ToDateTime(this.CurrentMaintain["cdate"]), sequenceMode: 2);
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

                    // roway = bundle_Detail_allpart_Tb.Select(string.Format("ukey1 = {0}", dr["ukey1"]));
                    // foreach (DataRow dr2 in roway)
                    // {
                    //    dr2["Bundleno"] = cListBundleno[nCount];
                    // }
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

            // string allPart_cmd = string.Format(@"Select b.* from Bundle_Detail_Allpart b WITH (NOLOCK) left join Bundle_Detail a WITH (NOLOCK) on a.id = b.id where b.id='{0}' ", masterID);
            // 直接撈Bundle_Detail_Allpart就行,不然在算新舊資料筆數來判斷新刪修時,會因為表頭表身join造成count過多
            string allPart_cmd = string.Format(@"select * from Bundle_Detail_Allpart WITH (NOLOCK) where id='{0}'  ", masterID);
            string art_cmd = string.Format(@"Select b.* from Bundle_Detail_art b WITH (NOLOCK) left join Bundle_Detail a WITH (NOLOCK) on a.Bundleno = b.bundleno and a.id = b.id where b.id='{0}'", masterID);
            string qty_cmd = string.Format(@"Select a.* from Bundle_Detail_qty a WITH (NOLOCK) Where a.id ='{0}'", masterID);
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
            allpart_cmd = allpart_cmd + string.Format(@"Delete from bundle_Detail_allpart where id ='{0}'", masterID);
            foreach (DataRow dr in this.bundle_Detail_allpart_Tb.Rows) // 處理Bundle_Detail_AllPart
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    allpart_cmd = allpart_cmd + string.Format(
                @"insert into bundle_Detail_allpart(ID,PatternCode,PatternDesc,Parts,isPair,Location) values('{0}','{1}','{2}','{3}','{4}','{5}');",
                this.CurrentMaintain["ID"], dr["PatternCode"], dr["PatternDesc"], dr["Parts"], dr["isPair"], dr["Location"]);
                }
            }
            #endregion

            #region 處理Bundle_Detail_Art 修改版
            /*
            * 先刪除原有資料
            * 再新增更改的資料
            */
            Art_cmd = Art_cmd + string.Format(
                @"
select ID,Ukey into #tmpOldBundle_Detail_Art from bundle_Detail_Art with (nolock) where  id='{0}'
delete from bundle_Detail_Art where id='{0}'", masterID);

            // 將SubProcessID不是單一筆的資料拆開
            DataTable bundle_Detail_Art_Tb_copy = this.bundle_Detail_Art_Tb.Copy();
            bundle_Detail_Art_Tb_copy.Clear();// 只有結構,沒有資料
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
                    Art_cmd = Art_cmd + string.Format(
               @"insert into bundle_Detail_Art(ID,Bundleno,PatternCode,SubProcessid,NoBundleCardAfterSubprocess,PostSewingSubProcess) 
                        values('{0}','{1}','{2}','{3}',{4},{5});",
               this.CurrentMaintain["ID"], dr["Bundleno"], dr["PatternCode"], dr["SubProcessid"], MyUtility.Convert.GetBool(dr["NoBundleCardAfterSubprocess"]) ? 1 : 0, MyUtility.Convert.GetBool(dr["PostSewingSubProcess"]) ? 1 : 0);
                }
            }

            Art_cmd = Art_cmd + $@"select Ukey  
from #tmpOldBundle_Detail_Art tda
where  not exists(select 1 from bundle_Detail_Art bda with (nolock) where tda.ID = bda.ID and tda.Ukey = bda.Ukey) ";
            #endregion
            #region 處理Bundle_Detail_Art

            // int art_old_rowCount = arttmp.Rows.Count;
            // foreach (DataRow dr in bundle_Detail_Art_Tb.Rows) //處理Bundle_Detail_Art
            // {
            //    if (bundle_Detail_Art_Tb.Rows.Count > art_old_rowCount)
            //    {
            //        Art_cmd = Art_cmd + string.Format(
            //       @"insert into bundle_Detail_Art(ID,Bundleno,PatternCode,SubProcessid)
            //            values('{0}','{1}','{2}','{3}');"
            //        , CurrentMaintain["ID"], dr["Bundleno"], dr["PatternCode"], dr["SubProcessid"]);
            //    }
            //    //if (row >= art_old_rowCount) //新增
            //    //{
            //    //    Art_cmd = Art_cmd + string.Format(
            //    //   @"insert into bundle_Detail_Art(ID,Bundleno,PatternCode,SubProcessid)
            //    //    values('{0}','{1}','{2}','{3}');"
            //    //    , CurrentMaintain["ID"], dr["Bundleno"], dr["PatternCode"], dr["SubProcessid"]);
            //    //}
            //    else //覆蓋
            //    {
            //        Art_cmd = Art_cmd + string.Format(
            //        @"update bundle_Detail_Art set PatternCode ='{0}',SubProcessid = '{1}',bundleno ='{2}'
            //        Where ukey ={3};", dr["PatternCode"], dr["SubProcessid"], dr["Bundleno"], arttmp.Rows[row]["ukey"]);
            //    }
            //    //row++;
            // }
            // bundle_Detail_Art_Tb.AcceptChanges();//變更Row Status 狀態
            // int art_new_rowCount = bundle_Detail_Art_Tb.Rows.Count;
            // if (art_old_rowCount < art_new_rowCount) //舊的筆數小於新的筆數 表示要先覆蓋再刪除多餘的
            // {
            //    for (int i = art_new_rowCount; i < art_old_rowCount; i++)
            //    {
            //        Art_cmd = Art_cmd + string.Format(@"Delete from bundle_Detail_Art where ukey ='{0}';", arttmp.Rows[i]["ukey"]);
            //    }
            // }
            #endregion

            #region 處理Bundle_Detail_Qty 修改版

            /*
           * 先刪除原有資料
           * 再新增更改的資料
           */
            Qty_cmd = Qty_cmd + string.Format(@"Delete from bundle_Detail_Qty where id ='{0}'", masterID);
            foreach (DataRow dr in this.bundle_Detail_Qty_Tb.Rows) // 處理Bundle_Detail_Art
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    Qty_cmd = Qty_cmd + string.Format(
                    @"insert into bundle_Detail_Qty(ID,SizeCode,Qty) 
                    values('{0}','{1}',{2});",
                    this.CurrentMaintain["ID"], dr["sizecode"], dr["Qty"]);
                }
            }

            #endregion
            #region 處理Bundle_Detail_Qty

            // row = 0;
            // int Qty_old_rowCount = qtytmp.Rows.Count;

            // foreach (DataRow dr in bundle_Detail_Qty_Tb.Rows) //處理Bundle_Detail_Art
            // {
            //    if (dr.RowState != DataRowState.Deleted)
            //    {
            //        if (bundle_Detail_Qty_Tb.Rows.Count> Qty_old_rowCount)
            //        {
            //            Qty_cmd = Qty_cmd + string.Format(
            //            @"insert into bundle_Detail_Qty(ID,SizeCode,Qty)
            //        values('{0}','{1}',{2});"
            //            , CurrentMaintain["ID"], dr["sizecode"], dr["Qty"]);
            //        }
            //        //if (row >= Qty_old_rowCount) //新增
            //        //{
            //        //    Qty_cmd = Qty_cmd + string.Format(
            //        //    @"insert into bundle_Detail_Qty(ID,SizeCode,Qty)
            //        //values('{0}','{1}',{2});"
            //        //    , CurrentMaintain["ID"], dr["sizecode"], dr["Qty"]);
            //        //}
            //        else //覆蓋
            //        {
            //            Qty_cmd = Qty_cmd + string.Format(
            //            @"update bundle_Detail_Qty set SizeCode ='{0}',Qty = {1}
            //        Where ukey = {2};", dr["SizeCode"], dr["Qty"], qtytmp.Rows[row]["ukey"]);
            //        }
            //    }
            //    row++;
            // }
            // bundle_Detail_Qty_Tb.AcceptChanges();//變更Row Status 狀態
            // int Qty_new_rowCount = bundle_Detail_Qty_Tb.Rows.Count;
            // if (Qty_old_rowCount > Qty_new_rowCount) //舊的筆數小於新的筆數 表示要先覆蓋再刪除多餘的
            // {
            //    for (int i = Qty_new_rowCount; i < Qty_old_rowCount; i++)
            //    {
            //        Qty_cmd = Qty_cmd + string.Format(@"Delete from bundle_Detail_Qty where ukey ={0};", qtytmp.Rows[i]["ukey"]);
            //    }
            // }
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

                if (!MyUtility.Check.Empty(Art_cmd))
                {
                    if (!(upResult = DBProxy.Current.Select(null, Art_cmd, out deleteBundle_Detail_Art)))
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
                    .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
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
                }
                ).CopyToDataTable();
                Task.Run(() => new Guozi_AGV().SentDeleteBundle(deletedDetail));
            }

            if (deleteBundle_Detail_Art.Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentDeleteBundle_SubProcess(deleteBundle_Detail_Art));
            }
            #endregion

            return base.ClickSavePost();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
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
            this.CurrentMaintain["cutref"] = string.Empty;
            this.CurrentMaintain["ITEM"] = string.Empty;
            this.txtFabricPanelCode.Text = string.Empty;
            this.dispFabricKind.Text = string.Empty;
            this.DetailDatas.Clear();
            this.bundle_Detail_allpart_Tb.Clear();
            this.bundle_Detail_Art_Tb.Clear();
            this.bundle_Detail_Qty_Tb.Clear();
            this.WorkOrder_Ukey = string.Empty;
            this.CurrentMaintain.EndEdit();
        }

        private void txtCutRef_Validating(object sender, CancelEventArgs e)
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
                this.clear();
                return;
            }

            string cmd = string.Format(
                @"
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
Where a.cutref='{0}' and a.mDivisionid = '{1}' and a.orderid = b.id",
                this.txtCutRef.Text, this.keyword);
            DataRow cutdr;
            if (!MyUtility.Check.Seek(cmd, out cutdr, null))
            {
                this.clear();
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

                #region Startno
                int startno = this.startNo_Function(this.CurrentMaintain["OrderID"].ToString());
                this.CurrentMaintain["startno"] = startno;
                #endregion

                string item_cmd = string.Format("Select a.Name from Reason a WITH (NOLOCK) , Style b WITH (NOLOCK) where a.Reasontypeid ='Style_Apparel_Type' and b.ukey = '{0}' and b.ApparelType = a.id", cutdr["styleukey"]);
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

                this.getFabricKind();
                this.CurrentMaintain.EndEdit();
            }

            ((DataTable)this.detailgridbs.DataSource).Clear();
        }

        private void txtSPNo_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["Cutref"]) || MyUtility.Check.Empty(this.CurrentMaintain["POID"]))
            {
                return;
            }

            Win.Tools.SelectItem item;
            string cuttingid = MyUtility.GetValue.Lookup("Cuttingsp", this.CurrentMaintain["POID"].ToString(), "Orders", "ID");

            // string selectCommand = string.Format("select b.orderid from workorder a, workorder_distribute b where a.cutref = '{0}' and a.id = '{1}' and a.ukey = b.workorderukey and a.id = b.id and b.id = '{1}'", CurrentMaintain["Cutref"], cuttingid);
            string selectCommand = string.Format(
                @"
select distinct b.orderid 
from workorder a WITH (NOLOCK) , workorder_distribute b WITH (NOLOCK) 
where a.cutref = '{0}' and a.id = '{1}' and a.ukey = b.workorderukey",
                this.CurrentMaintain["Cutref"], cuttingid);
            item = new Win.Tools.SelectItem(selectCommand, "20", this.Text);
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

                string work_cmd = string.Format("Select * from workorder a WITH (NOLOCK) ,workorder_Distribute b WITH (NOLOCK) Where a.ukey = b.workorderukey and a.cutref = '{0}' and b.orderid ='{1}' and a.MDivisionId = '{2}'", this.CurrentMaintain["Cutref"], newvalue, Env.User.Keyword);
                DataTable articleTb;
                if (DBProxy.Current.Select(null, work_cmd, out articleTb))
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
                        string cellid = MyUtility.GetValue.Lookup("SewingCell", this.CurrentMaintain["sewinglineid"].ToString() + cutdr["Factoryid"].ToString(), "SewingLine", "ID+factoryid");

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

            this.getFabricKind();
        }

        private void btnGarmentList_Click(object sender, EventArgs e)
        {
            string ukey = MyUtility.GetValue.Lookup("Styleukey", this.CurrentMaintain["poid"].ToString(), "Orders", "ID");
            var Sizelist = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Select(s => MyUtility.Convert.GetString(s["SizeCode"])).Distinct().ToList();

            PublicForm.GarmentList callNextForm =
    new PublicForm.GarmentList(ukey, this.CurrentMaintain["poid"].ToString(), this.txtCutRef.Text, Sizelist);
            callNextForm.ShowDialog(this);
            this.OnDetailEntered();
        }

        public int startNo_Function(string orderid) // Start No 計算
        {
            #region startno
            string max_cmd = string.Format("Select Max(startno+Qty) as Start from Bundle  WITH (NOLOCK) Where OrderID = '{0}'", orderid);
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
                // dr["Bundleno"] = "";
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
            var frm = new P10_Generate(this.CurrentMaintain, dt, this.bundle_Detail_allpart_Tb, this.bundle_Detail_Art_Tb, this.bundle_Detail_Qty_Tb);
            frm.ShowDialog(this);
            dt.DefaultView.Sort = "BundleGroup";
        }

        protected override bool ClickPrint()
        {
            P10_Print p = new P10_Print(this.CurrentMaintain);
            p.ShowDialog();
            string dtn = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.displayPrintDate.Text = dtn;
            string sqlcmd = string.Format(
                @"update Bundle set PrintDate = '{0}' where ID = '{1}';
                  update Bundle_Detail set PrintDate = '{0}' where ID = '{1}';",
                dtn, this.CurrentMaintain["ID"]);
            DBProxy.Current.Execute(null, sqlcmd);
            return true;
        }

        int at;

        private void txtArticle_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            string selectCommand, sqlwhere = string.Empty;
            Win.Tools.SelectItem item;
            if (!this.EditMode)
            {
                return;
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["PatternPanel"]))
            {
                sqlwhere = string.Format(" and w.FabricCombo = '{0}'", this.txtFabricCombo.Text);
            }

            if (!MyUtility.Check.Empty(this.CurrentMaintain["cutref"]))
            {
                selectCommand = string.Format(
                    @"
select distinct Article ,w.Colorid
from workorder w WITH (NOLOCK) 
inner join Workorder_Distribute wd WITH (NOLOCK) on w.Ukey = wd.WorkorderUkey
where Article!='' and w.cutref='{0}' and w.mDivisionid = '{1}' {2}",
                    this.CurrentMaintain["cutref"].ToString(), this.keyword, sqlwhere);
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
            if (!MyUtility.Check.Empty(this.CurrentMaintain["cutref"]))
            {
                sql = string.Format(
                    @"
select Article 
from Workorder_Distribute WITH (NOLOCK) 
where Article!='' and WorkorderUkey in ({0}) and Article='{1}'",
                    MyUtility.Check.Empty(this.WorkOrder_Ukey) ? string.Empty : this.WorkOrder_Ukey.Trim().Substring(0, this.WorkOrder_Ukey.Length - 1), newvalue);
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
        }

        private void txtLineNo_PopUp(object sender, TextBoxPopUpEventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            string sql = string.Format(
                @"Select ID,FactoryID,Description  From SewingLine WITH (NOLOCK) 
                                        where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}')", Env.User.Keyword);
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
                    where FactoryID in (select ID from Factory WITH (NOLOCK) where MDivisionID='{0}') and ID='{1}'", Env.User.Keyword, newvalue);
            string tmp = MyUtility.GetValue.Lookup(sql);
            if (string.IsNullOrWhiteSpace(tmp))
            {
                this.txtLineNo.Text = string.Empty;
                e.Cancel = true;
                MyUtility.Msg.WarningBox(string.Format("< Sewing Line> : {0} not found!!!", newvalue));
                return;
            }
        }

        private void TxtFabricCombo_Validating(object sender, CancelEventArgs e)
        {
            this.getFabricKind();
        }

        private void getFabricKind()
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

delete bundle where id = '{0}';
delete Bundle_Detail where id = '{0}';
delete Bundle_Detail_Art where id = '{0}';
delete Bundle_Detail_AllPart where id = '{0}';
delete Bundle_Detail_qty where id = '{0}';
delete Bundle_Detail_Order where id = '{0}';
", id);

            DualResult result = DBProxy.Current.Select(null, deleteBundleDetailQty, out DataTable[] dtDeleteResults);

            if (!result)
            {
                return result;
            }

            Task.Run(() => new Guozi_AGV().SentDeleteBundle((DataTable)this.detailgridbs.DataSource));
            if (dtDeleteResults[0].Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentDeleteBundle_SubProcess(dtDeleteResults[0]));
            }

            if (dtDeleteResults[1].Rows.Count > 0)
            {
                Task.Run(() => new Guozi_AGV().SentDeleteBundle_Detail_Order(dtDeleteResults[1]));
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
