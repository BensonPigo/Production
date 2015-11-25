using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;
using System.Reflection;
using Microsoft.Reporting.WinForms;

namespace Sci.Production.Warehouse
{
    public partial class P17 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Sci.Env.User.Keyword);
            //
            detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    DataRow tmp = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    this.OnDetailGridInsert();
                    DataRow newrow = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    newrow.ItemArray = tmp.ItemArray;
                }
            };
        }

        public P17(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Id='{0}'", transID);
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
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["IssueDate"] = DateTime.Now;
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
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        //print
        protected override bool ClickPrint()
        {


            return base.ClickPrint();
        }

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox3.Focus();
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty"
                        , row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Return Qty can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["ftyinventoryukey"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} doesn't exist in warehouse {5}"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"], row["stocktype"]) + Environment.NewLine);
                }

            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "RR", "IssueReturn", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["Stocktype"] = 'B';
            CurrentDetailData["MdivisionID"] = Sci.Env.User.Keyword;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            #region Seq 右鍵開窗
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    IList<DataRow> x;

                    Sci.Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(CurrentDetailData["poid"].ToString(), CurrentDetailData["seq"].ToString());
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = selepoitem.GetSelecteds();

                    CurrentDetailData["seq"] = x[0]["seq"];
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    CurrentDetailData["Description"] = x[0]["Description"];
                    string tmp = MyUtility.GetValue.Lookup(
                            string.Format(@"select isnull(ukey,0) ukey from dbo.ftyinventory 
                                                    where mdivisionid='{0}' and poid='{1}' and seq1='{2}' and seq2='{3}' and stocktype='{4}' and roll='{5}' "
                            , Sci.Env.User.Keyword, CurrentDetailData["poid"], x[0]["seq1"], x[0]["seq2"], CurrentDetailData["stocktype"], ""));
                    if (!MyUtility.Check.Empty(tmp)) CurrentDetailData["ftyinventoryukey"] = tmp;
                }
            };
#endregion
            #region -- Seq valied --
            ts.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                DataRow dr;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["seq"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["seq"] = "";
                        CurrentDetailData["seq1"] = "";
                        CurrentDetailData["seq2"] = "";
                        CurrentDetailData["Roll"] = "";
                        CurrentDetailData["Dyelot"] = "";
                        CurrentDetailData["stockunit"] = "";
                        CurrentDetailData["Description"] = "";
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(string.Format(@"select pounit, stockunit,fabrictype,qty,scirefno
,dbo.getmtldesc(id,seq1,seq2,2,0) as [description] from po_supp_detail
where id = '{0}' and seq1 ='{1}'and seq2 = '{2}'", CurrentDetailData["poid"], e.FormattedValue.ToString().PadRight(5).Substring(0, 3)
                                             , e.FormattedValue.ToString().PadRight(5).Substring(3, 2)), out dr, null))
                        {
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            e.Cancel = true;
                            return;
                        }

                        CurrentDetailData["seq"] = e.FormattedValue;
                        CurrentDetailData["seq1"] = e.FormattedValue.ToString().Substring(0, 3);
                        CurrentDetailData["seq2"] = e.FormattedValue.ToString().Substring(3, 2);
                        CurrentDetailData["Roll"] = "";
                        CurrentDetailData["Dyelot"] = "";
                        CurrentDetailData["stockunit"] = dr["stockunit"];
                        CurrentDetailData["Description"] = dr["description"];
                        
                        string tmp = MyUtility.GetValue.Lookup(
                            string.Format(@"select isnull(ukey,0) ukey from dbo.ftyinventory 
                                                    where mdivisionid='{0}' and poid='{1}' and seq1='{2}' and seq2='{3}' and stocktype='{4}' and roll='{5}' "
                            , Sci.Env.User.Keyword, CurrentDetailData["poid"], e.FormattedValue.ToString().Substring(0, 3), e.FormattedValue.ToString().Substring(3, 2), CurrentDetailData["stocktype"], ""));
                        if (!MyUtility.Check.Empty(tmp)) CurrentDetailData["ftyinventoryukey"] = tmp;
                    }
                }
            };
            #endregion Seq 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            #region Roll#右鍵開窗
            
            ts4.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Check.Empty(dr["POID"]))
                    {
                        MyUtility.Msg.WarningBox("Please fill SP# first");
                        return;
                    }
                    if (e.Button == System.Windows.Forms.MouseButtons.Right && e.RowIndex != -1)
                    {

                        string sqlcmd = string.Format(@"SELECT a.poid
,a.seq1
,a.seq2
,a.roll
,a.dyelot
,inqty - outqty + adjustqty balance
,ukey
                                                        FROM dbo.ftyinventory a
                                                        WHERE stocktype = 'B'
                                                        AND poID ='{0}'
                                                        AND seq1= '{1}' 
                                                        AND seq2= '{2}' 
order by poid,seq1,seq2,Roll", dr["poid"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString());
                        Sci.Win.Tools.SelectItem item
                            = new Sci.Win.Tools.SelectItem(sqlcmd, "13,4,3,10,5,10,10", dr["roll"].ToString(), "SP#,Seq1,Seq2,Roll,Dyelot,Balance,ukey");

                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel) { return; }
                        IList<DataRow> selectedData = item.GetSelecteds();
                        if (selectedData.Count > 0)
                        {
                            dr["roll"] = (selectedData[0])["roll"].ToString();
                            dr["dyelot"] = (selectedData[0])["dyelot"].ToString();
                            dr["ftyinventoryukey"] = (selectedData[0])["ukey"].ToString();
                        }
                    }
                }
            };
            #endregion
            #region -- Roll Vaild --
            ts4.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr;
                    if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["roll"].ToString()) != 0)
                    {

                        if (!MyUtility.Check.Seek(string.Format(@"select ukey from ftyinventory
where poid = '{0}' and seq1 ='{1}' and seq2 = '{2}' and mdivisionid = '{3}' and stocktype = '{4}' and roll = '{5}'"
                            , CurrentDetailData["poid"], e.FormattedValue.ToString().PadRight(5).Substring(0, 3)
                                             , e.FormattedValue.ToString().PadRight(5).Substring(3, 2), Sci.Env.User.Keyword, CurrentDetailData["stocktype"], CurrentDetailData["roll"]), out dr, null))
                        {
                            MyUtility.Msg.WarningBox("Data not found!", "Roll#");
                            e.Cancel = true;
                            return;
                        }
                        CurrentDetailData["ftyinventoryukey"] = dr["ukey"];
                    }
                }
            };
            #endregion
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13))  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6),settings:ts)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts4)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //5
            .Numeric("qty", header: "Return Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
            .Text("Location", header: "Bulk Location", iseditingreadonly: true)    //7
            ;     //
            #endregion 欄位設定
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查庫存項lock
            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueReturn_Detail d inner join FtyInventory f
on d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
from dbo.IssueReturn_Detail d left join FtyInventory f
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than return qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update IssueReturn set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString()
                    , 0m - (decimal)item["qty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), true, item["location"].ToString()));
            }

            sqlupd2.Append("drop table #tmp;" + Environment.NewLine);
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
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
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, 0m-item.qty, true, item.stocktype, item.mdivisionid));
            }

            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
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
from dbo.IssueReturn_Detail d inner join FtyInventory f
on d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
from dbo.IssueReturn_Detail d left join FtyInventory f
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than return qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update IssueReturn set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量  ftyinventory

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString(), 0m - (decimal)item["qty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), false, item["location"].ToString()));
            }
            sqlupd2.Append("drop table #tmp;" + Environment.NewLine);
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
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
                sqlupd2.Append(Prgs.UpdateMPoDetail(4, item.poid, item.seq1, item.seq2, 0m-item.qty, false, item.stocktype, item.mdivisionid));
            }

            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        ShowErr(sqlupd2.ToString(), result2);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
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

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.mdivisionid,a.PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,a.Roll
,a.Dyelot
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,p1.StockUnit
,a.Qty
,a.StockType
,a.ftyinventoryukey
,(select t.MtlLocationID+',' from (select mtllocationid from dbo.ftyinventory_detail fd where fd.Ukey = a.FtyInventoryUkey) t 
	for xml path('')) location
,a.ukey
from dbo.IssueReturn_Detail a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => ((DataTable)detailgridbs.DataSource).Rows.Remove(r));

        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P13_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P17_AccumulatedQty(CurrentMaintain);
            frm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return;
            }
            try
            {

                DataTable dtmaster = new DataTable();

                dtmaster.ImportRow(CurrentMaintain);

                viewer.LocalReport.ReportEmbeddedResource = "Sci.Production.Warehouse.P13Detail.rdlc";
                viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dtmaster));
                viewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(MySubreportEventHandler);
                //
                viewer.RefreshReport();
            }
            catch (Exception ex)
            {
                ShowErr("data loading error.");
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (!this.EditMode) return;

            if (!MyUtility.Check.Empty(textBox2.Text) && textBox2.Text != textBox2.OldValue)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();
                DataRow dr;
                DataTable dt;
                if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from dbo.issue where id='{0}' and status !='New')"
                    , textBox2.Text), out dr, null))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Transfer# is not found!!");
                    return;
                }
                else
                {
                    DBProxy.Current.Select(null, string.Format(@"select a.poid,a.seq1,a.seq2,a.Qty,a.StockType
,b.StockUnit
, a.seq1+a.seq2 as seq
, a.Roll as roll
, a.Dyelot as dyelot
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.ftyinventoryukey
,a.mdivisionid
from dbo.Issue_Detail a inner join dbo.PO_Supp_Detail b on a.PoID= b.id and a.Seq1 = b.SEQ1 and a.Seq2 = b.SEQ2
where a.id='{0}'", textBox2.Text), out dt);
                    foreach (var item in dt.ToList())
                    {
                        //DetailDatas.(item);
                        ((DataTable)detailgridbs.DataSource).ImportRow(item);
                    }
                }

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }
    }
}