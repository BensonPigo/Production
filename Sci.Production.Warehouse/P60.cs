using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P60 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P60(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.gridicon.RemoveClick += (s, e) =>
            {
                this.ComputeTotalQty();
            };
        }

        public P60(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format(" id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                this.dateIssueDate.ReadOnly = true;
                this.txtsubconLocalSupplier.TextBox1.ReadOnly = true;
                this.editRemark.ReadOnly = true;
                this.btnDelete.Enabled = false;
                this.btnImport.Enabled = false;
                this.gridicon.Enabled = false;
            }
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                this.dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("< Local Supplier >  can't be empty!", "Warning");
                this.txtsubconLocalSupplier.Focus();
                return false;
            }

            #endregion 必輸檢查

            #region -- 刪除數量為零或refno為空的資料 --
            var select = ((DataTable)this.detailgridbs.DataSource).Select("qty = 0 or refno=''");
            foreach (DataRow dr in select)
            {
                dr.Delete();
            }
            #endregion

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "RL", "LocalReceiving", (DateTime)this.CurrentMaintain["IssueDate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }

                this.CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain["Status"].Equals("Confirmed"))
            {
                this.detailgrid.IsEditingReadOnly = true;
            }
            else
            {
                this.detailgrid.IsEditingReadOnly = false;
            }
            #region Status Label

            this.label25.Text = this.CurrentMaintain["status"].ToString();
            this.txtTotal.Text = MyUtility.GetValue.Lookup(string.Format(
                @"
select sum(Qty) 
from LocalReceiving_Detail
where ID = '{0}'", this.CurrentMaintain["ID"].ToString()));
            #endregion Status Label
        }

        private Ict.Win.UI.DataGridViewNumericBoxColumn col_qty;

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region -- QTY 不可超過 On Road --

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = this.CurrentDetailData;
                    var v = this.detailgrid.Rows[e.RowIndex].Cells["onRoad"].Value;
                    string st = v.Empty() ? "0" : v.ToString();
                    if (decimal.Parse(e.FormattedValue.ToString()) > decimal.Parse(st))
                    {
                        MyUtility.Msg.WarningBox("Qty can't be over on road qty!!");
                        dr["Qty"] = dr["onRoad"];
                    }
                    else
                    {
                        dr["Qty"] = e.FormattedValue;
                    }

                    dr.EndEdit();
                    this.ComputeTotalQty();
                }
            };
            #endregion
            #region Location Setting
            DataGridViewGeneratorTextColumnSettings locationSet = new DataGridViewGeneratorTextColumnSettings();
            locationSet.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["Location"] = e.FormattedValue;
                    string sqlcmd = @"
select * 
from MtlLocation
where	Junk != 1
		and StockType = 'B'";
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["Location"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !location.EqualString(string.Empty))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!location.EqualString(string.Empty))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", errLocation.ToArray()) + "  Data not found !!", "Data not found");
                    }

                    trueLocation.Sort();
                    this.CurrentDetailData["Location"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                    this.detailgrid.RefreshEdit();
                }
            };

            locationSet.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation("B", this.CurrentDetailData["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["location"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("localpoid", header: "Local PO", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("ORDERid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("refno", header: "Refno", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Text("threadcolorid", header: "Color Shade", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Numeric("poqty", header: "PO Qty", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 6, iseditingreadonly: true)
            .Text("unitId", header: "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))
            .Numeric("onRoad", header: "On Road", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 6, iseditingreadonly: true)
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(6), decimal_places: 2, integer_places: 6, settings: ns).Get(out this.col_qty)
            .Text("location", header: "Location", width: Widths.AnsiChars(20), settings: locationSet)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(20))
            ;
            #endregion 欄位設定

            this.detailgrid.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["location"].DefaultCellStyle.BackColor = Color.Pink;
            this.ColumnColorChange();
        }

        private void ColumnColorChange()
        {
            this.col_qty.CellFormatting += (s, e) =>
            {
                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                if (e.RowIndex == -1)
                {
                    return;
                }

                Color newColor;
                if (MyUtility.Convert.GetString(this.CurrentMaintain["status"]).EqualString("New") && MyUtility.Convert.GetDecimal(dr["inqty"]) + MyUtility.Convert.GetDecimal(dr["qty"]) > MyUtility.Convert.GetDecimal(dr["poqty"]))
                {
                    // 黃色
                    newColor = Color.Yellow;
                }
                else
                {
                    newColor = Color.Pink;
                }

                this.detailgrid.Rows[e.RowIndex].Cells["qty"].Style.BackColor = newColor;
            };
        }

        // Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            DataTable dtt;
            string chklocalqty =
                @"
select t.orderid,t.refno,lpd.inqty,lpd.qty
from LocalPO_Detail lpd with(nolock),#tmp t
where t.LocalPo_detailukey = lpd .ukey
and lpd.inqty + t.qty > lpd.qty
";
            result = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), string.Empty, chklocalqty, out dtt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            StringBuilder chk = new StringBuilder();
            foreach (DataRow item in dtt.Rows)
            {
                chk.Append(string.Format("{0}# - {1} already received {2} can not exceed PO Qty {3}!! \r\n", item["orderid"], item["refno"], item["inqty"], item["qty"]));
            }

            if (chk.Length > 0)
            {
                MyUtility.Msg.WarningBox(chk.ToString());
                return;
            }

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select   d.OrderId
         ,d.Refno
         ,d.ThreadColorID
         ,d.Qty
         ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
from dbo.LocalReceiving_Detail d WITH (NOLOCK) 
left join dbo.LocalInventory f WITH (NOLOCK) on d.OrderId = f.OrderID and d.Refno = f.Refno 
      and d.ThreadColorID = f.ThreadColorID
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Refno#: {1} ThreadColorId: {2}'s balance: {3} is less than receiving qty: {4}" + Environment.NewLine,
                            tmp["orderid"], tmp["refno"], tmp["threadColorId"], tmp["balanceqty"], tmp["qty"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update LocalReceiving set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  Local Inventory & Location

            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           orderid = b.Field<string>("orderid"),
                           refno = b.Field<string>("refno"),
                           threadcolorid = b.Field<string>("threadcolorid"),
                           unitid = b.Field<string>("unitid"),
                       }
                        into m
                       select new
                       {
                           orderid = m.First().Field<string>("orderid"),
                           refno = m.First().Field<string>("refno"),
                           threadcolorid = m.First().Field<string>("threadcolorid"),
                           unitid = m.First().Field<string>("unitid"),
                           location = string.Join(",", m.Select(row => row["location"].ToString())),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            sqlupd2.Append(@"
merge into dbo.LocalInventory t
using (
	select	#tmp.OrderId
			, #tmp.Refno
			, #tmp.ThreadColorID
			, #tmp.UnitID
			, #tmp.Qty
			, NewLocation = stuff((select distinct ',' + ADDLocation.value
								   from (
				 					  select value = Data
			 						  from dbo.SplitString(#tmp.Location, ',')
						
									  union all
									  select value = Data
			 						  from dbo.SplitString(LInv.ALocation, ',')						
			 					   ) ADDLocation
								   for xml path('')
								 ), 1, 1, '')
	from #tmp
	left join LocalInventory LInv on  #tmp.OrderId = LInv.OrderID
									   and #tmp.Refno = LInv.Refno
									   and #tmp.ThreadColorID = LInv.ThreadColorID
) s on  t.OrderID = s.OrderID
		and t.Refno = s.Refno
		and t.ThreadColorID = s.ThreadColorID
when matched then 
	update set	inqty = inqty + s.qty
				, ALocation = s.NewLocation
when not matched then
	insert (orderid		, refno		, threadcolorid		, inqty	, unitid    , ALocation) 
    values (s.orderid	, s.refno	, s.threadcolorid	, s.qty	, s.unitid  , s.NewLocation);
");

            #endregion 更新庫存數量  Local Inventory

            #region -- 更新local po inqty --
            sqlcmd = this.DoUpdateLocalPoInQty();
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DataTable resultDt;
                    if (!(result2 = MyUtility.Tool.ProcessWithObject(bs1, string.Empty, sqlupd2.ToString(), out resultDt, "#tmp")))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlcmd, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        // Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO")
            {
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"
Select  d.OrderId
        ,d.Refno
        ,d.ThreadColorID
        ,d.Qty
        ,balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) 
from dbo.LocalReceiving_Detail d WITH (NOLOCK) 
left join dbo.LocalInventory f WITH (NOLOCK) on d.OrderId = f.OrderID and d.Refno = f.Refno 
      and d.ThreadColorID = f.ThreadColorID
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.Qty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "SP#: {0} Refno#: {1} ThreadColorId: {2}'s balance: {3} is less than receiving qty: {4}" + Environment.NewLine,
                            tmp["orderid"], tmp["refno"], tmp["threadColorId"], tmp["balanceqty"], tmp["qty"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update dbo.LocalReceiving set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  Local Inventory

            var bs1 = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           orderid = b.Field<string>("orderid"),
                           refno = b.Field<string>("refno"),
                           threadcolorid = b.Field<string>("threadcolorid"),
                           unitid = b.Field<string>("unitid"),
                       }
                        into m
                       select new
                       {
                           orderid = m.First().Field<string>("orderid"),
                           refno = m.First().Field<string>("refno"),
                           threadcolorid = m.First().Field<string>("threadcolorid"),
                           unitid = m.First().Field<string>("unitid"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(string.Format(
                    @"
merge into dbo.localinventory t
using (select '{1}','{2}','{3}',{0},'{4}') as s (orderid,refno,threadcolorid,qty,unitid)
on t.orderid = s.orderid and t.refno = s.refno and t.threadcolorid = s.threadcolorid 
when matched then
      update set inqty = inqty - s.qty
when not matched then
      insert (orderid,refno,threadcolorid,inqty,unitid) 
      values (s.orderid,s.refno,s.threadcolorid,0 - s.qty,s.unitid);",
                    item.qty, item.orderid, item.refno, item.threadcolorid, item.unitid));
            }

            #endregion 更新庫存數量  Local Inventory

            #region -- 更新local po inqty --
            sqlcmd = this.DoUpdateLocalPoInQty();
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlcmd, result);
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        private string DoUpdateLocalPoInQty()
        {
            string sqlcmd = string.Format(
                @"
update dbo.LocalPO_Detail 
      set InQty = isnull((select sum(d.Qty) ttl_receiving 
                          from dbo.LocalReceiving c 
                          inner join dbo.LocalReceiving_Detail d on c.id = d.id  
                          where c.Status = 'Confirmed' and d.LocalPo_detailukey = LocalPO_Detail.Ukey 
                              and d.LocalPoId = LocalPO_Detail.Id)
                        , 0)
from LocalPO_Detail 
join (select LocalPoId,LocalPo_detailukey 
      from dbo.LocalReceiving a 
      inner join dbo.LocalReceiving_Detail b on b.id = a.id where a.id='{0}'
) s on LocalPO_Detail.id = s.LocalPoId and LocalPO_Detail.ukey = s.LocalPo_detailukey", this.CurrentMaintain["id"]);

            return sqlcmd;
        }

        // 寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  a.Id
       ,a.OrderId
       ,Rtrim(a.Refno) Refno
       ,a.ThreadColorID
       ,a.OnRoad
       ,a.Qty
       ,a.LocalPoId
       ,a.Remark
       ,a.LocalPo_detailukey
       ,a.Location
       ,a.Category
       ,a.Mdivisionid
       ,a.OldSeq1
       ,a.OldSeq2
       ,a.Ukey
        , b.qty - b.inqty [onRoad]
        , b.Qty poqty,b.Price
        , dbo.getItemDesc(a.category,a.Refno) [description],b.UnitId
        , b.inqty
from dbo.LocalReceiving_Detail a WITH (NOLOCK) 
left join dbo.LocalPO_Detail b WITH (NOLOCK) on b.id = a.LocalPoId 
                                                and b.Ukey = a.LocalPo_detailukey
Where a.id = '{0}' ", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            ((DataTable)this.detailgridbs.DataSource).Select(string.Empty).ToList().ForEach(r => r.Delete());
        }

        // find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("OrderId", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        // Batch Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["localsuppid"]))
            {
                MyUtility.Msg.WarningBox("Please select local supplier first");
                this.txtsubconLocalSupplier.Focus();
                return;
            }

            var frm = new P60_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.ComputeTotalQty();
            this.RenewData();
        }

        private void ComputeTotalQty()
        {
            this.txtTotal.Text = this.detailgrid.Rows.Cast<DataGridViewRow>().AsEnumerable().Sum(row => decimal.Parse(row.Cells["Qty"].Value.ToString())).ToString();
        }

        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string invoice = row["InvNo"].ToString();
            string remarks = row["Remark"].ToString();
            string rpttitle = Env.User.Factory;

            #region -- 撈表頭資料 --
            DataTable dt;
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DualResult result = DBProxy.Current.Select(
                string.Empty,
                @"select l.localsuppid + s.Abb as Supplier
            from Localreceiving l WITH (NOLOCK) 
            left join localsupp s WITH (NOLOCK) on l.LocalSuppID=s.id
            where l.id = @ID", pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            string supplier = dt.Rows[0]["Supplier"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Invoice", invoice));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", remarks));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Supplier", supplier));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", rpttitle));

            #endregion
            #region -- 撈表身資料 --
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtDetail;
            string sqlcmd = @"
            select ld.LocalPoId,
	               ld.OrderId,
	               ld.Refno,
	               IIF((ld.OrderId = lag(ld.OrderId,1,'')over (order by ld.LocalPoId,ld.OrderId,lpd.refno)
				       AND (ld.refno = lag(ld.refno,1,'')over (order by ld.LocalPoId,ld.OrderId,lpd.refno))
				       ),'',dbo.getItemDesc(ld.Category,ld.Refno))[desc],
	               lpd.qty [poqty],
	               lpd.UnitId,
	               lpd.Price,
	               ld.OnRoad,
	               ld.qty,
	               ld.Remark
            from LocalReceiving_Detail ld WITH (NOLOCK) 
            left join LocalPO_Detail lpd WITH (NOLOCK) on ld.LocalPoId=lpd.Id and ld.LocalPo_detailukey=lpd.Ukey
            where ld.ID= @ID";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dtDetail);
            if (!result)
            {
                this.ShowErr(sqlcmd, result);
            }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", string.Empty);
                return false;
            }

            // 傳 list 資料
            List<P60_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P60_PrintData()
                {
                    LocalPOID = row1["LocalPoId"].ToString(),
                    SPNo = row1["OrderId"].ToString(),
                    RefNo = row1["Refno"].ToString(),
                    Desc = row1["desc"].ToString(),
                    POQTY = row1["poqty"].ToString(),
                    Unit = row1["UnitId"].ToString(),
                    POPrice = row1["Price"].ToString(),
                    OnRoad = row1["OnRoad"].ToString(),
                    QTY = row1["qty"].ToString(),
                    Remark = row1["Remark"].ToString(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P60_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P60_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(reportResourceAssembly, reportResourceNamespace, reportResourceName, out reportresource)))
            {
                // this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Win.Subs.ReportView(report);
            frm.MdiParent = this.MdiParent;
            frm.Show();

            return true;
        }
    }
}