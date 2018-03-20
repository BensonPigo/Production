﻿using Ict;
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
            //MDivisionID 是 P15 寫入 => Sci.Env.User.Keyword
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

        //PPIC_P15 Called        
        public static void Call(string PPIC_id, ToolStripMenuItem menuitem)
        {
          
            P15 call = new P15(menuitem, PPIC_id);
            call.Show();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region 新增Batch Shipment Finished按鈕
            Sci.Win.UI.Button btnUnFinish = new Sci.Win.UI.Button();
            btnUnFinish.Text = "UnFinish";
            btnUnFinish.Click += new EventHandler(unfinish);
            browsetop.Controls.Add(btnUnFinish);
            btnUnFinish.Size = new Size(180, 30);//預設是(80,30)
            btnUnFinish.Visible = true;
            #endregion
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Dictionary<string, string> di_type = new Dictionary<string, string>();
            di_type.Add("L", "Lacking");
            di_type.Add("R", "Replacement");
            comboType.DataSource = new BindingSource(di_type, null);
            comboType.ValueMember = "Key";
            comboType.DisplayMember = "Value";
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["FabricType"] = "A";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
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
                dateIssueDate.Focus();
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Requestid"]))
            {
                MyUtility.Msg.WarningBox("< Request# >  can't be empty!", "Warning");
                txtRequest.Focus();
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
                DataTable dr = (DataTable)this.detailgridbs.DataSource;
                DataRow[] temp = dr.Select(string.Format("poid='{0}' and seq1='{1}' and seq2='{2}'", row["poid"], row["seq1"], row["seq2"]));
                if (temp.Count() > 1)
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be duplicate"
                        , row["poid"], row["seq1"], row["seq2"])
                        + Environment.NewLine);
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IL", "IssueLack", (DateTime)CurrentMaintain["Issuedate"]);
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
            if (!MyUtility.Check.Empty(MyUtility.GetValue.Lookup(string.Format(@"select apvdate from lack WITH (NOLOCK) where id = '{0}'", CurrentMaintain["requestid"]))))
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format(@"select apvdate,Shift,SubconName from lack WITH (NOLOCK) where id = '{0}'", CurrentMaintain["requestid"]), out dr))
                {
                    this.displayApvDate.Text = ((DateTime)dr["apvdate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
                    this.displayBoxShift.Text = dr["Shift"].Equals("D") ? "Day" : dr["Shift"].Equals("N") ? "Night" : "Subcon-Out";
                    this.displayBoxSubconName.Text = dr["SubconName"].ToString();
                }
            }
            else
            {
                this.displayApvDate.Text = "";
                this.displayBoxShift.Text = "";
                this.displayBoxSubconName.Text = "";
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
            string issuelackid = MyUtility.GetValue.Lookup(string.Format(@"Select issuelackid from dbo.lack WITH (NOLOCK) where id = '{0}'", CurrentMaintain["requestid"]));
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
from dbo.IssueLack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll
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
from dbo.IssueLack_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll
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
            string nowtime = DateTime.Now.ToAppDateTimeFormatString();
            sqlupd3 = string.Format(@"update IssueLack set status='Confirmed', editname = '{0}' , editdate = '{2}',apvname = '{0}',apvdate = '{2}'
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"], nowtime);

            #endregion 更新表頭狀態資料
            #region -- update Lack.issuelackid & Lack.issuelackdt

            sqlupd4.Append(string.Format(@"update dbo.Lack set dbo.Lack.IssueLackDT='{0}'
, IssueLackId = '{1}' where id = '{2}';", nowtime, CurrentMaintain["id"], CurrentMaintain["requestid"]));
            sqlupd4.Append(Environment.NewLine);

            sqlupd4.Append(string.Format(@"update dbo.Lack_Detail  set IssueQty = t.qty
from (select seq1,seq2,sum(qty) qty from dbo.IssueLack_Detail WITH (NOLOCK) where id='{0}' group by seq1,seq2) t
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
                                       poid = b.Field<string>("poid"),
                                       seq1 = b.Field<string>("seq1"),
                                       seq2 = b.Field<string>("seq2"),
                                       stocktype = b.Field<string>("stocktype")
                                   } into m
                                   select new Prgs_POSuppDetailData
                                   {
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
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
          
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
from dbo.IssueLack_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll
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
from dbo.IssueLack_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.poid = f.POID and d.Seq1 = f.Seq1 and d.seq2 = f.seq2 and d.StockType = f.StockType and d.Roll = f.Roll
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

            sqlupd3 = string.Format(@"update IssueLack set status='New', editname = '{0}' , editdate = GETDATE(),apvname = '',apvdate = null
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
                                       poid = b.Field<string>("poid"),
                                       seq1 = b.Field<string>("seq1"),
                                       seq2 = b.Field<string>("seq2"),
                                       stocktype = b.Field<string>("stocktype")
                                   } into m
                                   select new Prgs_POSuppDetailData
                                   {
                                       poid = m.First().Field<string>("poid"),
                                       seq1 = m.First().Field<string>("seq1"),
                                       seq2 = m.First().Field<string>("seq2"),
                                       stocktype = m.First().Field<string>("stocktype"),
                                       qty = - (m.Sum(w => w.Field<decimal>("qty")))
                                   }).ToList();

                        var bsfio = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                                     select new
                                     {
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
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
           
        }

        protected override void ClickClose()
        {
            base.ClickClose();
            String sqlcmd;
            sqlcmd = string.Format("update IssueLack set status = 'Closed' , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
        }

        protected override void ClickUnclose()
        {
            base.ClickUnclose();
            DialogResult dResult = MyUtility.Msg.QuestionBox("Are you sure to unclose it?", "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            if (dResult.ToString().ToUpper() == "NO") return;
            String sqlcmd;
            sqlcmd = string.Format("update IssueLack set status = 'Confirmed' , editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
        }
        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
--,a.Roll
--,a.Dyelot
,p1.stockunit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Qty
,a.StockType
,dbo.Getlocation(f.Ukey)  as location
,a.ukey
,a.FtyInventoryUkey
from dbo.IssueLack_Detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory f WITH (NOLOCK) on a.POID=f.POID and a.Seq1=f.Seq1 and a.Seq2=f.Seq2 and a.Roll=f.Roll and a.Dyelot=f.Dyelot and a.StockType=f.StockType
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //Delete empty qty
        private void btnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["requestid"]))
            {
                MyUtility.Msg.WarningBox("Request# can't be empty, Please fill it first!!");
                return;
            }
            //string aa = comboBox1.Text;
            var frm = new Sci.Production.Warehouse.P16_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource, comboType.Text, "P15_Import");
            frm.P16 = this;
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void btnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P15_AccumulatedQty(CurrentMaintain);
            frm.P15 = this;
            frm.ShowDialog(this);
        }

        // Unfinish
        private void unfinish(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P15_Unfinish(P15_Unfinish.TypeAccessory, "P15_Unfinish");
            frm.ShowDialog(this);
        }

        //Locate for (find)
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        // Request ID
        private void txtRequest_Validating(object sender, CancelEventArgs e)
        {
            DataRow dr;
            if (!MyUtility.Check.Seek(string.Format(@"select [type],[apvdate],[issuelackid],[Shift],[SubconName] from dbo.lack WITH (NOLOCK) 
where id='{0}' and fabrictype='A' and mdivisionid='{1}'"
                , txtRequest.Text, Sci.Env.User.Keyword), out dr, null))
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
                    MyUtility.Msg.WarningBox(string.Format("This request# ({0}) already issued by {1}.", txtRequest.Text, dr["issuelackid"]));
                    return;
                }

            }
            CurrentMaintain["requestid"] = txtRequest.Text;
            CurrentMaintain["type"] = dr["type"].ToString();
            this.displayApvDate.Text = ((DateTime)dr["apvdate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateTimeStringFormat));
            this.displayBoxShift.Text = dr["Shift"].Equals("D") ? "Day" : dr["Shift"].Equals("N") ? "Night" : "Subcon-Out";
            this.displayBoxSubconName.Text = dr["SubconName"].ToString();
        }

        protected override bool ClickPrint()
        {
            if (CurrentMaintain["Status"].ToString() != "Confirmed")
            { ShowErr("Data is not confirmed, can't print"); return false; }
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Remark = row["Remark"].ToString();
            string Requestid = row["Requestid"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            string appvdate = MyUtility.Check.Empty(this.displayApvDate.Text) ? string.Empty : ((DateTime)MyUtility.Convert.GetDate(this.displayApvDate.Text)).ToString("yyyy/MM/dd HH:mm:ss");

            #region  抓表頭資料
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("", @"
select NameEn
from MDivision
where id = @MDivision", pars, out dt);
            if (!result) { this.ShowErr(result); }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dt");
                return false;
            }

            string RptTitle = dt.Rows[0]["NameEN"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Requestid", Requestid));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("appvdate", appvdate));
            #endregion

            #region  抓表身資料
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            DataTable dd;
            result = DBProxy.Current.Select("",@"
select a.POID
    ,a.Seq1+'-'+a.seq2 as SEQ
	,IIF((b.ID =   lag(b.ID,1,'') over (order by b.ID,b.seq1,b.seq2) 
		AND(b.seq1 = lag(b.seq1,1,'')over (order by b.ID,b.seq1,b.seq2))
		AND(b.seq2 = lag(b.seq2,1,'')over (order by b.ID,b.seq1,b.seq2))) 
		,'',dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0))[DESC]
	,unit = b.StockUnit
	,ReqQty=c.Requestqty 
	,a.Qty
    ,[Location]=dbo.Getlocation(fi.ukey)
    ,[Total]=sum(a.Qty) OVER (PARTITION BY a.POID ,a.Seq1,a.Seq2 )	        
from dbo.IssueLack_Detail a WITH (NOLOCK) 
left join dbo.IssueLack d WITH (NOLOCK) on d.id=a.ID
left join dbo.PO_Supp_Detail b WITH (NOLOCK) on b.id=a.POID and b.SEQ1=a.Seq1 and b.SEQ2=a.seq2
left join dbo.Lack_Detail c WITH (NOLOCK) on c.id=d.RequestID and c.Seq1=a.Seq1 and c.Seq2=a.Seq2
left join dbo.FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype
where a.id= @ID", pars, out dd);
            if (!result) { this.ShowErr(result); }

            if (dd == null || dd.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found !!!", "DataTable dd");
                return false;
            }

            // 傳 list 資料            
            List<P15_PrintData> data = dd.AsEnumerable()
                .Select(row1 => new P15_PrintData()
                {
                    POID = row1["POID"].ToString().Trim(),
                    SEQ = row1["SEQ"].ToString().Trim(),
                    DESC = row1["DESC"].ToString().Trim(),
                    unit = row1["unit"].ToString().Trim(),
                    ReqQty = row1["ReqQty"].ToString().Trim(),
                    QTY = row1["QTY"].ToString().Trim(),
                    Location = row1["Location"].ToString().Trim(),
                    Total = row1["Total"].ToString().Trim()
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