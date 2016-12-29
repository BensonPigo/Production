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
using Sci.Win;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P15 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P15(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("FabricType='A' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            //
        }

        public P15(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("FabricType='A' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Dictionary<string, string> di_type = new Dictionary<string, string>();
            di_type.Add("L", "Lacking");
            di_type.Add("R", "Replacement");
            comboBox1.DataSource = new BindingSource(di_type, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["FabricType"] = "A";
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

        // Print - subreport
        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
        }

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();
            #region -- 必輸檢查 --

            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateBox3.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Requestid"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                textBox2.Focus();
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IP", "IssueLack", (DateTime)CurrentMaintain["Issuedate"]);
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
            //Lack.ApvDate
            if (!MyUtility.Check.Empty(MyUtility.GetValue.Lookup(string.Format(@"select apvdate from lack where id = '{0}'", CurrentMaintain["requestid"]))))
            {
                DateTime dt = Convert.ToDateTime(MyUtility.GetValue.Lookup(string.Format(@"select apvdate from lack where id = '{0}'", CurrentMaintain["requestid"])));
                this.displayBox3.Text = dt.ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            }
            else
            {
                this.displayBox3.Text = "";
            }
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["Stocktype"] = 'B';
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
                //.Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
                //.Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Unit", iseditingreadonly: true)    //5
            .Numeric("qty", header: "Issue Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10)    //6
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
            StringBuilder sqlupd4 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2, result3;
            DataTable datacheck;
            string sqlupd2_FIO = "";
            string sqlupd2_B = "";
            string issuelackid = MyUtility.GetValue.Lookup(string.Format(@"Select issuelackid from dbo.lack where id = '{0}'", CurrentMaintain["requestid"]));
            if (!MyUtility.Check.Empty(issuelackid))
            {
                MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", CurrentMaintain["requestid"], issuelackid), "Can't Confirmed");
                return;
            }

            if (CurrentMaintain["type"].ToString() == "R")
            {
                #region -- 檢查庫存項lock --
                sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueLack_Detail d inner join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.MDivisionID = f.MDivisionID and d.Roll = f.Roll
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
                #region -- 檢查負數庫存 --

                sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueLack_Detail d left join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.MDivisionID = f.MDivisionID and d.Roll = f.Roll
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
                #region -- 更新庫存數量  ftyinventory --
                sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, true);
                sqlupd2_B=Prgs.UpdateMPoDetail(4, null, true);                
                #endregion 更新庫存數量  ftyinventory
            }
            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update IssueLack set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- update Lack.issuelackid & Lack.issuelackdt

            sqlupd4.Append(string.Format(@"update dbo.Lack set dbo.Lack.IssueLackDT='{0}'
, IssueLackId = '{1}' where id = '{2}';", DateTime.Parse(CurrentMaintain["issuedate"].ToString()).ToShortDateString(), CurrentMaintain["id"], CurrentMaintain["requestid"]));
            sqlupd4.Append(Environment.NewLine);

            sqlupd4.Append(string.Format(@"update dbo.Lack_Detail  set IssueQty = t.qty
from (select seq1,seq2,sum(qty) qty from dbo.IssueLack_Detail where id='{0}' group by seq1,seq2) t
where dbo.Lack_Detail.id = '{1}' and dbo.Lack_Detail.seq1 = t.Seq1 and dbo.Lack_Detail.seq2 = t.Seq2", CurrentMaintain["id"], CurrentMaintain["requestid"]));

            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (CurrentMaintain["type"].ToString() == "R")
                    {
                        DataTable resulttb;
                        var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                                   group b by new
                                   {
                                       mdivisionid = b.Field<string>("mdivisionid"),
                                       poid = b.Field<string>("poid"),
                                       seq1 = b.Field<string>("seq1"),
                                       seq2 = b.Field<string>("seq2"),
                                       stocktype = b.Field<string>("stocktype")
                                   } into m
                                   select new Prgs_POSuppDetailData
                                   {
                                       mdivisionid = m.First().Field<string>("mdivisionid"),
                                       poid = m.First().Field<string>("poid"),
                                       seq1 = m.First().Field<string>("seq1"),
                                       seq2 = m.First().Field<string>("seq2"),
                                       stocktype = m.First().Field<string>("stocktype"),
                                       qty = m.Sum(w => w.Field<decimal>("qty"))
                                   }).ToList();
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb
                            , "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                        if (!(result = MyUtility.Tool.ProcessWithDatatable
                            ((DataTable)detailgridbs.DataSource, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }
                    if (!(result3 = DBProxy.Current.Execute(null, sqlupd4.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd4.ToString(), result3);
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
            StringBuilder sqlupd4 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2, result3;
            string sqlupd2_FIO = "";
            string sqlupd2_B = "";

            if (CurrentMaintain["type"].ToString() == "R")
            {
                #region -- 檢查庫存項lock --
                sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueLack_Detail d inner join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.MDivisionID = f.MDivisionID and d.Roll = f.Roll
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
                #region -- 檢查負數庫存 --

                sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueLack_Detail d left join FtyInventory f
--on d.ftyinventoryukey = f.ukey
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.MDivisionID = f.MDivisionID and d.Roll = f.Roll
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
                #region -- 更新庫存數量  ftyinventory --
                sqlupd2_FIO = Prgs.UpdateFtyInventory_IO(4, null, false);
                sqlupd2_B = Prgs.UpdateMPoDetail(4, null, false);  
                #endregion 更新庫存數量  ftyinventory
            }
            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"update IssueLack set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料
            #region -- update Lack.issuelackid & Lack.issuelackdt
            sqlupd4.Append(string.Format(@"update dbo.Lack set dbo.Lack.IssueLackDT= DEFAULT
, IssueLackId = DEFAULT where id = '{2}';", CurrentMaintain["issuedate"], CurrentMaintain["id"], CurrentMaintain["requestid"]));
            sqlupd4.Append(Environment.NewLine);
            sqlupd4.Append(string.Format(@"update dbo.Lack_Detail  set IssueQty = DEFAULT where dbo.Lack_Detail.id = '{0}' ", CurrentMaintain["requestid"]));
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (CurrentMaintain["type"].ToString() == "R")
                    {
                        DataTable resulttb;
                        var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                                   group b by new
                                   {
                                       mdivisionid = b.Field<string>("mdivisionid"),
                                       poid = b.Field<string>("poid"),
                                       seq1 = b.Field<string>("seq1"),
                                       seq2 = b.Field<string>("seq2"),
                                       stocktype = b.Field<string>("stocktype")
                                   } into m
                                   select new Prgs_POSuppDetailData
                                   {
                                       mdivisionid = m.First().Field<string>("mdivisionid"),
                                       poid = m.First().Field<string>("poid"),
                                       seq1 = m.First().Field<string>("seq1"),
                                       seq2 = m.First().Field<string>("seq2"),
                                       stocktype = m.First().Field<string>("stocktype"),
                                       qty = - (m.Sum(w => w.Field<decimal>("qty")))
                                   }).ToList();

                        var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                                     select new
                                     {
                                         mdivisionid = m.Field<string>("mdivisionid"),
                                         poid = m.Field<string>("poid"),
                                         seq1 = m.Field<string>("seq1"),
                                         seq2 = m.Field<string>("seq2"),
                                         stocktype = m.Field<string>("stocktype"),
                                         qty = -(m.Field<decimal>("qty")),
                                         roll = m.Field<string>("roll"),
                                         dyelot = m.Field<string>("dyelot"),
                                     }).ToList();
                        if (!(result = MyUtility.Tool.ProcessWithObject(bs1, "", sqlupd2_B, out resulttb
                            , "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                        DataTable dx = ((DataTable)detailgridbs.DataSource);
                        if (!(result = MyUtility.Tool.ProcessWithObject(bsfio, "", sqlupd2_FIO, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!(result3 = DBProxy.Current.Execute(null, sqlupd4.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd4.ToString(), result3);
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
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.mdivisionid,a.PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
--,a.Roll
--,a.Dyelot
,p1.stockunit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Qty
,a.StockType
,stuff((select ',' + t.MtlLocationID from (select mtllocationid from dbo.ftyinventory_detail fd where fd.Ukey = a.FtyInventoryUkey) t 
	for xml path('')), 1, 1, '') location
,a.ukey
,a.FtyInventoryUkey
from dbo.IssueLack_Detail a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //Delete empty qty
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["requestid"]))
            {
                MyUtility.Msg.WarningBox("Request# can't be empty, Please fill it first!!");
                return;
            }
            //string aa = comboBox1.Text;
            var frm = new Sci.Production.Warehouse.P16_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource, comboBox1.Text);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P15_AccumulatedQty(CurrentMaintain);
            frm.ShowDialog(this);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        //Locate for (find)
        private void button8_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        // Request ID
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            DataRow dr;
            if (!MyUtility.Check.Seek(string.Format(@"select [type],[apvdate],[issuelackid] from dbo.lack 
where id='{0}' and fabrictype='A' and mdivisionid='{1}'"
                , textBox2.Text, Sci.Env.User.Keyword), out dr, null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Please check requestid is Accessory.", "Data not found!!");
                return;
            }
            else
            {
                if (MyUtility.Check.Empty(dr["apvdate"]))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Request is not approved!!");
                    return;
                }

                if (!MyUtility.Check.Empty(dr["issuelackid"]))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", textBox2.Text, dr["issuelackid"]));
                    return;
                }

            }
            CurrentMaintain["requestid"] = textBox2.Text;
            CurrentMaintain["type"] = dr["type"].ToString();
        }


        protected override bool ClickPrint()
        {
            if (CurrentMaintain["Status"].ToString() != "Confirmed")
            { ShowErr("Data is not confirmed, can't print"); return false; }
            DataRow row = this.CurrentDataRow;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string Requestid = row["Requestid"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select b.name 
            from dbo.IssueLack  a 
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
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Requestid", Requestid));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result = DBProxy.Current.Select("",
            @"select a.POID
                    ,a.Seq1+'-'+a.seq2 as SEQ
	                ,[DESC]=dbo.getMtlDesc(a.poid,a.seq1,a.Seq2,2,0)
		            ,unit = b.StockUnit
		            ,ReqQty=c.Requestqty 
	                ,a.Qty
                    ,[Location]=dbo.Getlocation(a.FtyInventoryUkey)
                    ,[Total]=sum(a.Qty) OVER (PARTITION BY a.POID ,a.Seq1,a.Seq2 )	        
             from dbo.IssueLack_Detail a 
		     left join dbo.IssueLack d 
                 on d.id=a.ID
             left join dbo.PO_Supp_Detail b 
                 on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
		     left join dbo.Lack_Detail c
		         on c.id=d.RequestID and c.Seq1=a.Seq1 and c.Seq2=a.Seq2
             where a.id= @ID", pars, out dd);
            if (!result) { this.ShowErr(result); }

            // 傳 list 資料            
            List<P15_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P15_PrintData()
                {
                    POID = row1["POID"].ToString(),
                    SEQ = row1["SEQ"].ToString(),
                    DESC = row1["DESC"].ToString(),
                    unit = row1["unit"].ToString(),
                    ReqQty = row1["ReqQty"].ToString(),
                    QTY = row1["QTY"].ToString(),
                    Location = row1["Location"].ToString(),
                    Total = row1["Total"].ToString()
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            #region  指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P15_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P15_Print.rdlc";

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