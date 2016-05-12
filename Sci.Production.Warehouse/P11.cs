using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Transactions;
using Sci.Production.PublicPrg;
using Sci.Trade.Class.Commons;

namespace Sci.Production.Warehouse
{
    public partial class P11 : Sci.Win.Tems.Input8
    {
        DataRow dr;
        string poid = "";

        P11_Detail subform = new P11_Detail();
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);

            WorkAlias = "Issue";                        // PK: ID
            GridAlias = "Issue_detail";           // PK: ID+UKey
            SubGridAlias = "Issue_size";          // PK: ID+Issue_DetailUkey+SizeCode

            KeyField1 = "ID"; //Issue PK
            KeyField2 = "ID"; // Summary FK

            //SubKeyField1 = "Ukey";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField1 = "ID";    // 將第2層的PK欄位傳給第3層的FK。
            SubKeyField2 = "Ukey";  // 將第2層的PK欄位傳給第3層的FK。

            SubDetailKeyField1 = "id,Ukey";    // second PK
            SubDetailKeyField2 = "id,Issue_DetailUkey"; // third FK
            //SubDetailKeyField1 = "Ukey";    // second PK
            //SubDetailKeyField2 = "Issue_SummaryUkey"; // third FK

            DoSubForm = subform;
        }

        public P11(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {

            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;

        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {

            #region -- outqty 開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.CellMouseDoubleClick += (s, e) =>
            {
                DoSubForm.IsSupportDelete = false;
                DoSubForm.IsSupportNew = false;
                OpenSubDetailPage();
            };
            #endregion

            #region -- Seq 右鍵開窗 --

            Ict.Win.DataGridViewGeneratorMaskedTextColumnSettings ts2 = new DataGridViewGeneratorMaskedTextColumnSettings();
            ts2.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable bulkItems;
                    string sqlcmd = string.Format(@"select a.*,b.FabricType,b.SCIRefno,f.MtlTypeID,m.IssueType,left(a.seq1+'   ',3)+a.seq2 seq
from dbo.ftyinventory a inner join dbo.po_supp_detail b on b.id=a.POID and b.seq1=a.seq1 and b.seq2 = a.Seq2
inner join Fabric f on f.SCIRefno = b.SCIRefno
inner join MtlType m on m.ID = f.MtlTypeID
where lock=0 and inqty-outqty+adjustqty > 0 
and mdivisionid='{0}' and poid='{1}' and stocktype='B'
and b.FabricType='A'
and m.IssueType='Sewing' order by poid,seq1,seq2", Sci.Env.User.Keyword, CurrentDetailData["poid"]);
                    IList<DataRow> x;
                    DualResult result2;
                    if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out bulkItems)))
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }

                    Sci.Win.Tools.SelectItem selepoitem = new Win.Tools.SelectItem(bulkItems
                            , "Type,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,inqty,outqty,adjustqty,ukey"
                            , "4,14,10,10,13,4,3,6,6,6,10", CurrentDetailData["seq"].ToString()
                            , "FabricType,SCIRefno,MtlTypeID,IssueType,Poid,Seq1,Seq2,In Qty,Out Qty,Adjust Qty,Ukey");
                    selepoitem.Width = 1024;

                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = selepoitem.GetSelecteds();
                    CurrentDetailData["seq"] = x[0]["seq"];
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    CurrentDetailData["mdivisionid"] = x[0]["mdivisionid"];
                    CurrentDetailData["stocktype"] = x[0]["stocktype"];
                    CurrentDetailData["ftyinventoryukey"] = x[0]["ukey"];
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["seq"] = "";
                        CurrentDetailData["seq1"] = "";
                        CurrentDetailData["seq2"] = "";
                        CurrentDetailData["mdivisionid"] = "";
                        CurrentDetailData["stocktype"] = "";
                        CurrentDetailData["ftyinventoryukey"] = 0;
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(string.Format(@"select a.*,b.FabricType,b.SCIRefno,f.MtlTypeID,m.IssueType,left(a.seq1+'   ',3)+a.seq2 seq
from dbo.ftyinventory a inner join dbo.po_supp_detail b on b.id=a.POID and b.seq1=a.seq1 and b.seq2 = a.Seq2
inner join Fabric f on f.SCIRefno = b.SCIRefno
inner join MtlType m on m.ID = f.MtlTypeID
where poid = '{0}' and a.seq1 ='{1}' and a.seq2 = '{2}' and lock=0 and mdivisionid='{3}' and stocktype='B' and inqty-outqty+adjustqty > 0"
                            , CurrentDetailData["poid"], e.FormattedValue.ToString().PadRight(5).Substring(0, 3)
                                                 , e.FormattedValue.ToString().PadRight(5).Substring(3, 2), Sci.Env.User.Keyword), out dr, null))
                        {
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            CurrentDetailData["seq"] = e.FormattedValue;
                            CurrentDetailData["seq1"] = e.FormattedValue.ToString().Substring(0, 3);
                            CurrentDetailData["seq2"] = e.FormattedValue.ToString().Substring(3, 2);
                            CurrentDetailData["mdivisionid"] = dr["mdivisionid"];
                            CurrentDetailData["stocktype"] = dr["stocktype"];
                            CurrentDetailData["ftyinventoryukey"] = dr["ukey"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .MaskedText("seq", "CCC-CC", "Seq#", width: Widths.AnsiChars(6), settings: ts2)  //1
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //2
            .Text("Colorid", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)  //3
            .Text("SizeSpec", header: "SizeSpec", width: Widths.AnsiChars(8), iseditingreadonly: true)  //4
            .Text("SizeUnit", header: "SizeUnit", width: Widths.AnsiChars(6), iseditingreadonly: true)  //5
            .Numeric("usedqty", header: "@Qty", width: Widths.AnsiChars(6), decimal_places: 4, integer_places: 10, iseditingreadonly: true)    //6
            .Text("location", header: "Location", width: Widths.AnsiChars(6), iseditingreadonly: true)  //7
            .Numeric("accu_issue", header: "Accu. Issued", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //8
            .Text("output", header: "Output", width: Widths.AnsiChars(20), iseditingreadonly: true, settings: ts) //9
            .Numeric("Qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //10
            .Numeric("balanceqty", header: "Bulk Balance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //11
            .Text("ftyinventoryukey", header: "FtyInventoryUkey", width: Widths.AnsiChars(10), iseditingreadonly: true)  //12
            ;     //
            #endregion 欄位設定

            #region 可編輯欄位變色

            detailgrid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns[9].DefaultCellStyle.BackColor = Color.Pink;

            #endregion 可編輯欄位變色
        }

        protected override void OpenSubDetailPage()
        {
            subform.master = CurrentMaintain;
            base.OpenSubDetailPage();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            string cutplanID = (e.Master == null) ? "" : e.Master["cutplanID"].ToString();
            this.DetailSelectCommand = string.Format(@"  select
a.Id
,isnull(a.FtyInventoryUkey,0) [FtyInventoryUkey]
,a.MDivisionID
,a.Poid
,a.seq1
,a.seq2
,left(a.seq1+'   ',3)+a.seq2 as seq
,a.StockType
,a.Qty
,p.Colorid
,p.SizeSpec
,p.UsedQty
,p.SizeUnit
,isnull((select t.MtlLocationID+',' from (select mtllocationid from dbo.FtyInventory_Detail where ukey=a.FtyInventoryUkey)t for xml path('')),'') [location]
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0)[description]
,isnull((select sum(Issue_Detail.qty) from dbo.issue inner join dbo.Issue_Detail on Issue_Detail.id = Issue.Id where Issue.type = 'B' and Issue.Status='Confirmed' and issue.id!=a.Id and Issue_Detail.FtyInventoryUkey = a.FtyInventoryUkey),0.00) [accu_issue]
,isnull((select v.sizeqty+', ' from (select (rtrim(Issue_Size.SizeCode) +'*'+convert(varchar,Issue_Size.Qty)) as sizeqty from dbo.Issue_Size where Issue_Size.Issue_DetailUkey = a.ukey) v for xml path('')),'') [output]
,a.Ukey
,isnull((select inqty-outqty+adjustqty from dbo.ftyinventory where ukey = a.ftyinventoryukey),0.00) as balanceqty
from dbo.Issue_Detail a left join dbo.po_supp_detail p on p.id  = a.poid and p.seq1= a.seq1 and p.seq2 =a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
            CurrentMaintain["issuedate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["cutplanId"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                txtRequest.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            #endregion 必輸檢查

            

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IS", "Issue", (DateTime)CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            DataTable dt;
            foreach (DataRow dr in DetailDatas)
            {
                if (GetSubDetailDatas(dr, out dt))
                {
                    sum_subDetail(dr, dt);
                }
            }
            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        static void sum_subDetail(DataRow target, DataTable source)
        {
            target["qty"] = (source.Rows.Count == 0) ? 0m : source.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted)
                .Sum(r => r.Field<decimal>("qty"));
        }

        private void btnAutoPick_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P11_AutoPick(CurrentMaintain["id"].ToString(), this.poid, CurrentMaintain["cutplanid"].ToString());
            DialogResult result = frm.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                DataRow tmp;
                DataTable _detail, _subDetail;
                _detail = (DataTable)detailgridbs.DataSource;
                
                //刪除表身重新匯入
                foreach (DataRow del in DetailDatas)
                {
                    del.Delete();
                }

                //批次匯入
                foreach (KeyValuePair<DataRow, DataTable> item in frm.dictionaryDatas)
                {
                    tmp = item.Key;
                    if (tmp["selected"].ToString() == "1")
                    {
                        // 匯入Issue_detail layer
                        tmp["id"] = CurrentMaintain["id"];
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        _detail.ImportRow(tmp);

                        // 匯入Issue_Size layer
                        if (GetSubDetailDatas(_detail.Rows[_detail.Rows.Count-1], out _subDetail))
                        {
                            foreach (DataRow dr2 in item.Value.Rows)
                            {
                                dr2.AcceptChanges();
                                dr2.SetAdded();
                                _subDetail.ImportRow(dr2);
                            }
                        }
                    }
                }
            }
        }

        private void txtRequest_Validating(object sender, CancelEventArgs e)
        {
            if (!MyUtility.Check.Empty(txtRequest.Text) && txtRequest.Text != txtRequest.OldValue)
            {
                CurrentMaintain["cutplanid"] = txtRequest.Text;
                getpoid();
                this.disPOID.Text = this.poid;
                CurrentMaintain["orderid"] = this.poid;
            }
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataTable dt;
            if (!(CurrentMaintain == null))
            {
                disCutCell.Text = MyUtility.GetValue.Lookup(string.Format("select CutCellID from dbo.cutplan  where id='{0}'", CurrentMaintain["cutplanid"]));
                disLine.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.SewLine+','  from (select distinct o.SewLine 
from dbo.Issue_detail a inner join dbo.orders o on a.Poid = o.POID where a.id='{0}' and o.sewline !='') t for xml path('')", CurrentMaintain["id"]));

                DBProxy.Current.Select(null, string.Format(@";with cte as
(Select WorkOrder.FabricCombo,Cutplan_Detail.CutNo from Cutplan_Detail inner join dbo.workorder on WorkOrder.Ukey = Cutplan_Detail.WorkorderUkey 
where Cutplan_Detail.ID='{0}' )
select distinct FabricCombo ,(select convert(varchar,CutNo)+',' 
from (select CutNo from cte where cte.FabricCombo = a.FabricCombo )t order by CutNo for xml path('')) cutnos from cte a
", CurrentMaintain["cutplanid"]), out dt);
                ebCut.Text = String.Join(" / ", dt.AsEnumerable().Select(row => row["FabricCombo"].ToString() + "-" + row["cutnos"].ToString()));
                //ebArticle.Text = MyUtility.GetValue.Lookup(string.Format(@"select t.article+','  from (select distinct article 
//from dbo.cutplan_detail  where id='{0}') t for xml path('')", CurrentMaintain["cutplanid"]));


                #region -- Status Label --

                label25.Text = CurrentMaintain["status"].ToString();

                #endregion Status Label
                #region -- POID
                this.getpoid();
                this.disPOID.Text = this.poid;
                #endregion

                #region -- matrix breakdown
                DualResult result;
                if (!(result = matrix_Reload()))
                {
                    ShowErr(result);
                }
                #endregion
            }
        }

        private DualResult matrix_Reload()
        {
            DualResult result;
            DataTable dtSizeCode, dtIssueBreakDown=null;

            gridIssueBreakDown.AutoGenerateColumns = true;
            gridIssueBreakDownBS.DataSource = dtIssueBreakDown;
            gridIssueBreakDown.DataSource = gridIssueBreakDownBS;
            gridIssueBreakDown.IsEditingReadOnly = true;
            gridIssueBreakDown.ReadOnly = true;

            string sqlcmd = string.Format(@"select sizecode from dbo.order_sizecode 
where id = (select poid from dbo.orders where id='{0}') order by seq", CurrentMaintain["orderid"]);

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dtSizeCode)))
            {
                ShowErr(sqlcmd, result);
                return result;
            }
            if (dtSizeCode.Rows.Count == 0)
            {
                return Result.True;
            }

            StringBuilder sbSizecode = new StringBuilder();
            sbSizecode.Clear();

            for (int i = 0; i < dtSizeCode.Rows.Count; i++)
            {
                sbSizecode.Append(string.Format(@"[{0}],", dtSizeCode.Rows[i]["sizecode"].ToString().TrimEnd()));
            }

            StringBuilder sbIssueBreakDown = new StringBuilder();
            sbIssueBreakDown.Append(string.Format(@"select * from Issue_Breakdown
pivot
(
	sum(qty)
	for sizecode in ({2})
)as pvt
where id='{1}'
order by [OrderID],[Article]", CurrentMaintain["orderid"], CurrentMaintain["id"], sbSizecode.ToString().Substring(0, sbSizecode.ToString().Length - 1)));
            if (!(result = DBProxy.Current.Select(null, sbIssueBreakDown.ToString(), out dtIssueBreakDown)))
            {
                ShowErr(sqlcmd, result);
                return result;
            }

            gridIssueBreakDown.AutoGenerateColumns = true;
            gridIssueBreakDownBS.DataSource = dtIssueBreakDown;
            gridIssueBreakDown.DataSource = gridIssueBreakDownBS;
            gridIssueBreakDown.IsEditingReadOnly = true;
            gridIssueBreakDown.ReadOnly = true;

            return Result.True;
        }
        private void getpoid()
        {
            //CurrentMaintain["cutplanid"] = txtRequest.Text;
            this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.cutplan where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));
            if (MyUtility.Check.Empty(this.poid))
            {
                this.poid = MyUtility.GetValue.Lookup(string.Format("select poid from dbo.orders where id='{0}' and mdivisionid = '{1}'", CurrentMaintain["cutplanid"], Sci.Env.User.Keyword));
            }

        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
            CurrentDetailData["poid"] = this.poid;
            DataTable sizeRange, subDetails;
            if (GetSubDetailDatas(CurrentDetailData, out subDetails))
            {
                DBProxy.Current.Select(null, string.Format(@"select a.SizeCode,b.Id,b.Issue_DetailUkey,isnull(b.Qty,0) QTY
from dbo.Order_SizeCode a left join dbo.Issue_Size b on b.SizeCode = a.SizeCode and b.id = '{1}' and b.Issue_DetailUkey = {2}
where a.id='{0}' order by Seq", this.poid, CurrentMaintain["id"], CurrentDetailData["ukey"]), out sizeRange);
                foreach (DataRow dr in sizeRange.Rows)
                {
                    dr.AcceptChanges();
                    dr.SetAdded();
                    subDetails.ImportRow(dr);
                }
            }

        }

        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "" : e.Detail["ID"].ToString();
            string ukey = (e.Detail == null || MyUtility.Check.Empty(e.Detail["ukey"])) ? "0" : e.Detail["ukey"].ToString();
            this.getpoid();
            this.SubDetailSelectCommand = string.Format(@"select a.SizeCode,b.Id,b.Issue_DetailUkey,isnull(b.Qty,0) QTY
from dbo.Order_SizeCode a left join dbo.Issue_Size b on b.SizeCode = a.SizeCode and b.id = '{1}' and b.Issue_DetailUkey = {2}
where a.id='{0}' order by Seq", this.poid, masterID, ukey);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return ;
            }
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d inner join FtyInventory f
on d.ftyinventoryukey = f.ukey
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d left join FtyInventory f
on d.ftyinventoryukey = f.ukey
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }

            foreach (DataRow item in datacheck.Rows)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString()
                    , (decimal)item["qty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), true));
            }

            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, item.qty, true, item.stocktype, item.mdivisionid));
            }

            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Confirmed successful");
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult == DialogResult.No) return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d inner join FtyInventory f
on d.ftyinventoryukey = f.ukey
where f.lock=1 and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3} is locked!!" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Issue_Detail d left join FtyInventory f
on d.ftyinventoryukey = f.ukey
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than issue qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Issue set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory
            sqlcmd = string.Format(@"select * from issue_detail where id='{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            //sqlupd2.Append("declare @iden as bigint;");
            //sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in datacheck.Rows)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString(), (decimal)item["qty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), false));
            }
            //sqlupd2.Append("drop table #tmp;" + Environment.NewLine);
            var bs1 = (from b in datacheck.AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, item.qty, false, item.stocktype, item.mdivisionid));
            }

            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        private void btnBOA_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P11_BOA(CurrentMaintain["id"].ToString(), this.poid, CurrentMaintain["cutplanid"].ToString());
            frm.ShowDialog(this);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                dr.Delete();
            }
        }

        private void btnBreakDown_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P11_IssueBreakDown(CurrentMaintain);
            frm.ShowDialog(this);
            this.OnDetailEntered();
        }
    }
}
