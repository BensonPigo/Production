using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class P12 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='C' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            ChangeDetailColor();
            //
            detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode&&e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    DataRow tmp = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    this.OnDetailGridInsert();
                    DataRow newrow = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    newrow.ItemArray = tmp.ItemArray;
                }
            };
        }

        public P12(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='C' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            ChangeDetailColor();

        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "C";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        private void ChangeDetailColor()
        {
            detailgrid.RowPostPaint += (s, e) =>
            {
                if (!this.EditMode)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (detailgrid.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                    int i = e.RowIndex;
                    if ( MyUtility.Check.Empty(dr["stockunit"]))
                    {
                        detailgrid.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                    }
                }
            };
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
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IP", "Issue", (DateTime)CurrentMaintain["Issuedate"]);
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
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region SP# Vaild 判斷

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            DataRow dr;
            ts4.CellValidating += (s, e) =>
            {
                if (this.EditMode && string.Compare(CurrentDetailData["poid"].ToString(),e.FormattedValue.ToString()) != 0)
                {
                    if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po where id = '{0}')", e.FormattedValue), null))
                    {
                        MyUtility.Msg.WarningBox("SP# is not exist!!", "Data not found");
                        e.Cancel = true;
                        return;
                    }
                    CurrentDetailData["poid"] = e.FormattedValue;
                    CurrentDetailData["seq"]="";
                    CurrentDetailData["seq1"] = "";
                    CurrentDetailData["seq2"] = "";
                }
            };

            #endregion SP# Vaild 判斷

            #region Seq 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    IList<DataRow> x;
                    
                    Sci.Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(CurrentDetailData["poid"].ToString(), CurrentDetailData["seq"].ToString(),"fabrictype !='F'");
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = selepoitem.GetSelecteds();
                    string productiontype = MyUtility.GetValue.Lookup(string.Format(@"select productiontype from fabric inner join mtltype on fabric.mtltypeid = mtltype.id where SCIRefno = '{0}'", x[0]["scirefno"]), null);
                    if (productiontype.ToUpper().TrimEnd() != "PACKING")
                    {
                        MyUtility.Msg.WarningBox(string.Format("Seq ({1}) : Production type is  {0}  not packing!!", productiontype, x[0]["seq"]), "Seq");
                        return;
                    }
                    else
                    {
                        CurrentDetailData["seq"] = x[0]["seq"];
                        CurrentDetailData["seq1"] = x[0]["seq1"];
                        CurrentDetailData["seq2"] = x[0]["seq2"];
                        CurrentDetailData["stockunit"] = x[0]["stockunit"];
                        CurrentDetailData["Description"] = x[0]["Description"];
                    }
                    
                }
            };
            ts.CellValidating += (s, e) =>
                {
                    if (!this.EditMode) return;
                    if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["seq"].ToString()) != 0)
                    {
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            CurrentDetailData["seq"] = "";
                            CurrentDetailData["seq1"] = "";
                            CurrentDetailData["seq2"] = "";
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
                            else
                            {
                                string productiontype = MyUtility.GetValue.Lookup(string.Format(@"select productiontype from fabric inner join mtltype on fabric.mtltypeid = mtltype.id where SCIRefno = '{0}'", dr["scirefno"]), null);
                                if (productiontype.ToUpper().TrimEnd() != "PACKING")
                                {
                                    MyUtility.Msg.WarningBox(string.Format("Seq ({1}) : Production type is  {0}  not packing!!", productiontype,e.FormattedValue), "Seq");
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    CurrentDetailData["seq"] = e.FormattedValue;
                                    CurrentDetailData["seq1"] = e.FormattedValue.ToString().Substring(0, 3);
                                    CurrentDetailData["seq2"] = e.FormattedValue.ToString().Substring(3, 2);
                                    CurrentDetailData["stockunit"] = dr["stockunit"];
                                    CurrentDetailData["Description"] = dr["description"];
                                }
                            }
                        }
                    }
                };

            #endregion Seq 右鍵開窗

            #region Location 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation("B", CurrentDetailData["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["location"] = item.GetSelectedString();
                }
            };

            #endregion Location 右鍵開窗

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13))  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts)  //1
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //2
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //3
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //4
            .Text("Location", header: "Bulk Location", iseditingreadonly: true)    //5
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

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString()
                    , (decimal)item["qty"]
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
            if (dResult.ToString().ToUpper() == "NO") return;
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

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString(), (decimal)item["qty"]
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

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id, a.mdivisionid, a.PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Roll
,a.Dyelot
,a.Qty
,a.StockType
,(select mtllocationid+',' from (select mtllocationid from dbo.ftyinventory_detail fd where ukey= ftyinventoryukey) t 
    for xml path('')) location
,a.ukey
from dbo.issue_detail a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
            
        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P12_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
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


        protected override bool ClickPrint()
        {
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select  b.name 
            from dbo.Issue  a 
            inner join dbo.mdivision  b 
            on b.id = a.mdivisionid
            where b.id = a.mdivisionid
            and a.id = @ID", pars, out dt);
            if (!result) { this.ShowErr(result); }
            string RptTitle = dt.Rows[0]["name"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dtt;
            result = DBProxy.Current.Select("",
            @"select a.POID
                    ,a.Seq1+'-'+a.seq2 as SEQ
	                ,[DESC]=dbo.getMtlDesc(a.poid,a.seq1,a.Seq2,2,0)
	                ,a.Qty
                    ,[BULKLocation]=dbo.Getlocation(a.FtyInventoryUkey)
	                ,unit = b.StockUnit
            from dbo.Issue_Detail a
            left join dbo.PO_Supp_Detail b
                on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2            
            where a.id= @ID", pars, out dtt);
            if (!result) { this.ShowErr(result); }
           
            // 傳 list 資料            
            List<P12_PrintData> data = dtt.AsEnumerable()
                .Select(row1 => new P12_PrintData()
                {
                    POID = row1["POID"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    DESC = row1["DESC"].ToString(),
                    unit = row1["unit"].ToString(),
                    QTY = row1["QTY"].ToString(),
                    BULKLocation = row1["BULKLocation"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P12_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P12_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;
            #endregion
            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();
  
           return true;

        }
    }
}