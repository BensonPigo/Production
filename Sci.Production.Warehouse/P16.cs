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
    public partial class P16 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P16(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            viewer = new ReportViewer();
            viewer.Dock = DockStyle.Fill;
            Controls.Add(viewer);

            this.DefaultFilter = string.Format("FabricType='F' and FactoryID = '{0}'", Sci.Env.User.Factory);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            //
        }

        public P16(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("FabricType='F' and id='{0}'", transID);
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
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["FabricType"] = "F";
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
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
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

            return base.ClickPrint();
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IF", "IssueLack", (DateTime)CurrentMaintain["Issuedate"]);
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
            this.displayBox3.Text = MyUtility.GetValue.Lookup(string.Format(@"select cast(apvdate as date) from lack where id = '{0}'", CurrentMaintain["requestid"]));
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
            #region SP# Vaild 判斷

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            DataRow dr;
            ts4.CellValidating += (s, e) =>
            {
                if (this.EditMode && string.Compare(CurrentDetailData["poid"].ToString(), e.FormattedValue.ToString()) != 0)
                {
                    if (!MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po where id = '{0}')", e.FormattedValue), null))
                    {
                        MyUtility.Msg.WarningBox("SP# is not exist!!", "Data not found");
                        e.Cancel = true;
                        return;
                    }
                    CurrentDetailData["poid"] = e.FormattedValue;
                    CurrentDetailData["seq"] = "";
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

                    Sci.Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(CurrentDetailData["poid"].ToString(), CurrentDetailData["seq"].ToString(), "fabrictype !='F'");
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
                                    MyUtility.Msg.WarningBox(string.Format("Seq ({1}) : Production type is  {0}  not packing!!", productiontype, e.FormattedValue), "Seq");
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
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
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
                #region -- 檢查負數庫存 --

                sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueLack_Detail d left join FtyInventory f
on d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                #region -- 更新庫存數量 Po_artwork & ftyinventory --

                sqlupd2.Append("declare @iden as bigint;");
                sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
                foreach (DataRow item in DetailDatas)
                {
                    sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString()
                        , (decimal)item["qty"]
                        , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), true, item["location"].ToString()));
                }
                sqlupd2.Append("drop table #tmp;" + Environment.NewLine);
                var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                           group b by new
                           {
                               poid = b.Field<string>("poid"),
                               seq1 = b.Field<string>("seq1"),
                               seq2 = b.Field<string>("seq2"),
                               stocktype = b.Field<string>("stocktype")
                           } into m
                           select new
                           {
                               poid = m.First().Field<string>("poid"),
                               seq1 = m.First().Field<string>("seq1"),
                               seq2 = m.First().Field<string>("seq2"),
                               stocktype = m.First().Field<string>("stocktype"),
                               qty = m.Sum(w => w.Field<decimal>("qty"))
                           }).ToList();

                foreach (var item in bs1)
                {
                    sqlupd2.Append(Prgs.UpdatePO_Supp_Detail(4, item.poid, item.seq1, item.seq2, item.qty, true, item.stocktype));
                }

                #endregion 更新庫存數量 Po_artwork & ftyinventory
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
            DualResult result, result2,result3;

            if (CurrentMaintain["type"].ToString() == "R")
            {
                #region -- 檢查庫存項lock --
                sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueLack_Detail d inner join FtyInventory f
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
                #region -- 檢查負數庫存 --

                sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.Qty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.IssueLack_Detail d left join FtyInventory f
on d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
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
                #region -- 更新庫存數量 Po_artwork & ftyinventory --

                sqlupd2.Append("declare @iden as bigint;");
                sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
                foreach (DataRow item in DetailDatas)
                {
                    sqlupd2.Append(Prgs.UpdateFtyInventory(4, item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString(), (decimal)item["qty"]
                        , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), false, item["location"].ToString()));
                }
                sqlupd2.Append("drop table #tmp;" + Environment.NewLine);
                var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                           group b by new
                           {
                               poid = b.Field<string>("poid"),
                               seq1 = b.Field<string>("seq1"),
                               seq2 = b.Field<string>("seq2"),
                               stocktype = b.Field<string>("stocktype")
                           } into m
                           select new
                           {
                               poid = m.First().Field<string>("poid"),
                               seq1 = m.First().Field<string>("seq1"),
                               seq2 = m.First().Field<string>("seq2"),
                               stocktype = m.First().Field<string>("stocktype"),
                               qty = m.Sum(w => w.Field<decimal>("qty"))
                           }).ToList();

                foreach (var item in bs1)
                {
                    sqlupd2.Append(Prgs.UpdatePO_Supp_Detail(4, item.poid, item.seq1, item.seq2, item.qty, false, item.stocktype));
                }

                #endregion 更新庫存數量 Po_artwork & ftyinventory
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
            this.DetailSelectCommand = string.Format(@"select a.id,a.PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,a.Roll
,a.Dyelot
,p1.stockunit
,dbo.getMtlDesc(a.poid,a.seq1,a.seq2,2,0) as [Description]
,a.Qty
,a.StockType
,(select c.ukey from dbo.ftyinventory c 
    where c.poid = a.poid and c.seq1 = a.seq1 and c.seq2  = a.seq2 and c.stocktype = a.stocktype and c.roll=a.roll and c.dyelot = a.dyelot) as ukey
,'' location
from dbo.IssueLack_Detail a left join PO_Supp_Detail p1 on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //Delete empty qty
        private void button9_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => ((DataTable)detailgridbs.DataSource).Rows.Remove(r));
        }

        //Import
        private void button5_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["requestid"]))
            {
                MyUtility.Msg.WarningBox("Request# can't be empty, Please fill it first!!");
                return;
            }
            var frm = new Sci.Production.Warehouse.P16_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated Qty
        private void button1_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P16_AccumulatedQty(CurrentMaintain);
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
            if (!MyUtility.Check.Seek(string.Format(@"select [type] from dbo.lack 
where id='{0}' and fabrictype='F' and apvdate is not null and (issuelackid ='' or issuelackid is null)"
                , textBox2.Text), out dr, null))
            {
                e.Cancel = true;
                MyUtility.Msg.WarningBox("Data not found!!");
                return;
            }
            CurrentMaintain["requestid"] = textBox2.Text;
            CurrentMaintain["type"] = dr["type"].ToString();
        }
    }
}