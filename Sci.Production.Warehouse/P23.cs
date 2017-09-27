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
using System.Data.SqlClient;
using Sci.Win;

namespace Sci.Production.Warehouse
{
    public partial class P23 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        protected ReportViewer viewer;
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            //
        }

        public P23(ToolStripMenuItem menuitem, string transID)
            : this(menuitem)
        {

            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
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
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "B";
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

        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("< Issue Date >  can't be empty!", "Warning");
                dateIssueDate.Focus();
                return false;
            }

            #endregion 必輸檢查

            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} can't be empty"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty"
                        , row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PI", "SubTransfer", (DateTime)CurrentMaintain["Issuedate"]);
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
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;
            #region -- To Location 右鍵開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(CurrentDetailData["tostocktype"].ToString(), CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK)
WHERE   StockType='{0}'
        and junk != '1'", CurrentDetailData["tostocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = CurrentDetailData["tolocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                       e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        
                    }
                    trueLocation.Sort();
                    CurrentDetailData["tolocation"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "Inventory"+Environment.NewLine+"SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //0
            .Text("fromseq", header: "Inventory" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //1
            .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
            .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(6), iseditingreadonly: true)  //3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) //4
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5))    //5
            .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true)    //6
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns).Get(out col_Qty)    //7
            .Text("topoid", header: "Bulk" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //8
            .Text("toseq", header: "Bulk" + Environment.NewLine + " Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //9
            .Text("toLocation", header: "To Location", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(18)).Get(out col_tolocation)    //10
            ;     //
            #endregion 欄位設定
            col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            string upd_MD_4T = "";
            string upd_MD_8T = "";
            string upd_MD_2T = "";
            String upd_Fty_4T = "";
            String upd_Fty_2T = "";

            string sqlcmd = "";
            string sqlupd3, ids = "";
            DualResult result, result2;
            DataTable datacheck;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"
Select  d.frompoid
        , d.fromseq1
        , d.fromseq2
        , d.fromRoll
        , d.Qty
        , balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID 
                                           and D.FromStockType = F.StockType
                                           and d.FromRoll = f.Roll 
                                           and d.FromSeq1 =f.Seq1 
                                           and d.FromSeq2 = f.Seq2
where   f.lock=1 
        and d.Id = '{0}'", CurrentMaintain["id"]);
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
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"
Select  d.frompoid
        , d.fromseq1
        , d.fromseq2
        , d.fromRoll
        , d.Qty
        , balanceQty = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
from (
		Select  frompoid
                , fromseq1
                , fromseq2
                , fromRoll
                , Qty = sum(Qty)
                , FromStockType
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty > 0
		Group by frompoid, fromseq1, fromseq2, fromRoll, FromStockType
	 ) as d 
left join FtyInventory f WITH (NOLOCK) on d.FromPOID = f.POID 
                                          and d.FromRoll = f.Roll 
                                          and d.FromSeq1 = f.Seq1 
                                          and d.FromSeq2 = f.Seq2 
                                          and D.FromStockType = F.StockType
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) < d.Qty) ", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Inventory balance Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            sqlcmd = string.Format(@"
Select  d.topoid
        , d.toseq1  
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
from (
		Select  topoid
                , toseq1
                , toseq2
                , toRoll
                , Qty = sum(Qty)
                , toStocktype
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty < 0
		Group by topoid, toseq1, toseq2, toRoll, toStocktype
	 ) as d
left join FtyInventory f WITH (NOLOCK) on   d.toPoId = f.PoId 
                                            and d.toSeq1 = f.Seq1 
                                            and d.toSeq2 = f.seq2 
                                            and d.toStocktype = f.StockType 
                                            and d.toRoll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.Qty < 0) ", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Bulk balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion -- 檢查負數庫存 --

            #region -- 檢查目的Roll是否已存在資料 --

            sqlcmd = string.Format(@"
Select  d.ToPoid
        , d.ToSeq1
        , d.toseq2
        , d.ToRoll
        , d.ToDyelot
        , d.Qty
        , f.InQty    
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.ToPoid = f.PoId
                                            and d.ToSeq1 = f.Seq1
                                            and d.toseq2 = f.seq2
                                            and d.ToStocktype = f.StockType
                                            and d.ToRoll = f.Roll
                                            and d.ToDyelot != f.dyelot
where   f.InQty > 0 
        and toroll != '' 
        and toroll is not null 
        and d.Id = '{0}'", CurrentMaintain["id"]);
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
                        ids += string.Format("Seq#: {1}-{2} Roll#: {3} exist in SP#: {0} but dyelot is not {4}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["todyelot"]);
                    }
                    MyUtility.Msg.WarningBox(ids + Environment.NewLine + "Please change roll# !!", "Warning");
                    return;
                }
            }

            #endregion

            #region -- 更新表頭狀態資料 --
            sqlupd3 = string.Format(@"
update SubTransfer 
set status = 'Confirmed'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);
            #endregion 更新表頭狀態資料

            #region -- 更新mdivisionpodetail B倉數 --
            var data_MD_4T =  (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct())
                       }).ToList();

            var data_MD_8T = data_MD_4T.Select(data => new Prgs_POSuppDetailData
            {
                poid = data.poid,
                seq1 = data.seq1,
                seq2 = data.seq2,
                stocktype = data.stocktype,
                qty = -(data.qty)
            }).ToList();

            #endregion

            #region -- 更新mdivisionpodetail A倉數 --
            var data_MD_2T = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("Topoid"),
                           seq1 = b.Field<string>("Toseq1"),
                           seq2 = b.Field<string>("Toseq2"),
                           stocktype = b.Field<string>("Tostocktype")
                       } into m
                       select new Prgs_POSuppDetailData 
                       {
                           poid = m.First().Field<string>("Topoid"),
                           seq1 = m.First().Field<string>("Toseq1"),
                           seq2 = m.First().Field<string>("Toseq2"),
                           stocktype = m.First().Field<string>("Tostocktype"),
                           qty = m.Sum(w => w.Field<decimal>("qty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct())
                       }).ToList();

            #endregion
            
            #region -- 更新庫存數量 ftyinventory --
            var data_Fty_4T = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                               group m by new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot")
                               } into g
                               select new
                               {
                                   poid = g.Key.poid,
                                   seq1 = g.Key.seq1,
                                   seq2 = g.Key.seq2,
                                   stocktype = g.Key.stocktype,
                                   qty = g.Sum(m => m.Field<decimal>("qty")),
                                   location = g.Key.location,
                                   roll = g.Key.roll,
                                   dyelot = g.Key.dyelot,
                               }).ToList();

            DataTable newDt = ((DataTable)detailgridbs.DataSource).Clone();
            foreach (DataRow dtr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                string[] dtrLocation = dtr["ToLocation"].ToString().Split(',');
                dtrLocation = dtrLocation.Distinct().ToArray();

                if (dtrLocation.Length == 1)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["ToLocation"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2T = (from b in newDt.AsEnumerable()
                               select new
                               {
                                   poid = b.Field<string>("topoid"),
                                   seq1 = b.Field<string>("toseq1"),
                                   seq2 = b.Field<string>("toseq2"),
                                   stocktype = b.Field<string>("tostocktype"),
                                   qty = b.Field<decimal>("qty"),
                                   location = b.Field<string>("ToLocation"),
                                   roll = b.Field<string>("toroll"),
                                   dyelot = b.Field<string>("todyelot"),
                               }).ToList();
            upd_Fty_4T = Prgs.UpdateFtyInventory_IO(4, null, true);
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量 ftyinventory

            #region 更新 Po_Supp_Detail StockUnit
            string sql_UpdatePO_Supp_Detail = @";
alter table #Tmp alter column ToPoid varchar(20)
alter table #Tmp alter column ToSeq1 varchar(3)
alter table #Tmp alter column ToSeq2 varchar(3)
alter table #Tmp alter column StockUnit varchar(20)

select  distinct ToPoid
        , ToSeq1
        , ToSeq2
        , StockUnit 
into #tmpD 
from #Tmp

merge dbo.PO_Supp_Detail as target
using #tmpD as src on   target.ID = src.ToPoid 
                        and target.seq1 = src.ToSeq1 
                        and target.seq2 =src.ToSeq2 
when matched then
    update
    set target.StockUnit = src.StockUnit;
";
            #endregion 
            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (_transactionscope)
            using (sqlConn) 
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4T, "", upd_Fty_4T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, "", upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_4T = (Prgs.UpdateMPoDetail(4, null, true, sqlConn: sqlConn));
                    upd_MD_8T = (Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn));
                    upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4T, "", upd_MD_4T, out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, "", upd_MD_8T, out resulttb
                        , "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, "", upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion

                    if (!(result = MyUtility.Tool.ProcessWithDatatable
                        ((DataTable)detailgridbs.DataSource, "", sql_UpdatePO_Supp_Detail, out resulttb, "#tmp", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
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
            _transactionscope.Dispose();
            _transactionscope = null;
         
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;

            string upd_MD_4F = "";
            string upd_MD_8F = "";
            string upd_MD_2F = "";
            String upd_Fty_4F = "";
            String upd_Fty_2F = "";

            string sqlcmd = "", sqlupd3 = "", ids = "";
            DualResult result, result2;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(@"
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.toPoId = f.PoId
                                            and d.toSeq1 = f.Seq1
                                            and d.toSeq2 = f.seq2
                                            and d.toStocktype = f.StockType
                                            and d.toRoll = f.Roll
where   f.lock = 1 
        and d.Id = '{0}'", CurrentMaintain["id"]);
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
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"]);
                    }
                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(@"
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
from (
		Select  topoid
                , toseq1
                , toseq2
                , toRoll
                , Qty = sum(Qty)
                , toStocktype
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty > 0
		Group by topoid, toseq1, toseq2, toRoll, toStocktype
) as d
left join FtyInventory f WITH (NOLOCK) on   d.toPoId = f.PoId 
                                            and d.toSeq1 = f.Seq1 
                                            and d.toSeq2 = f.seq2 
                                            and d.toStocktype = f.StockType 
                                            and d.toRoll = f.Roll
where (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) - d.Qty < 0) ", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Bulk balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            sqlcmd = string.Format(@"
Select  d.frompoid
        , d.fromseq1
        , d.fromseq2
        , d.fromRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
from (
		Select  frompoid
                , fromseq1
                , fromseq2
                , fromRoll
                , Qty = sum (Qty)
                , FromStockType
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty < 0
		Group by frompoid, fromseq1, fromseq2, fromRoll, FromStockType
) as d 
left join FtyInventory f WITH (NOLOCK) on   d.FromPOID = f.POID 
                                            and d.FromRoll = f.Roll 
                                            and d.FromSeq1 =f.Seq1 
                                            and d.FromSeq2 = f.Seq2 
                                            AND D.FromStockType = F.StockType
where (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) < -d.Qty) ", CurrentMaintain["id"]);
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
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine
                            , tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"]);
                    }
                    MyUtility.Msg.WarningBox("Inventory balance Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(@"
update SubTransfer 
set status = 'New'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新MdivisionPoDetail B倉數量 --
            var data_MD_4F = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("frompoid"),
                           seq1 = m.First().Field<string>("fromseq1"),
                           seq2 = m.First().Field<string>("fromseq2"),
                           stocktype = m.First().Field<string>("fromstocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty")))
                       }).ToList();

            var data_MD_8F = data_MD_4F.Select(data => new Prgs_POSuppDetailData {
                poid = data.poid,
                seq1 = data.seq1,
                seq2 = data.seq2,
                stocktype = data.stocktype,
                qty = -(data.qty)
            }).ToList();           

            #endregion
            #region -- 更新mdivisionpodetail A倉數 --
            var data_MD_2F = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("Topoid"),
                           seq1 = b.Field<string>("Toseq1"),
                           seq2 = b.Field<string>("Toseq2"),
                           stocktype = b.Field<string>("Tostocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("Topoid"),
                           seq1 = m.First().Field<string>("Toseq1"),
                           seq2 = m.First().Field<string>("Toseq2"),
                           stocktype = m.First().Field<string>("Tostocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("qty"))),
                           location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct())
                       }).ToList();

            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4F = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                               group m by new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot")
                               } into g
                               select new
                               {
                                   poid = g.Key.poid,
                                   seq1 = g.Key.seq1,
                                   seq2 = g.Key.seq2,
                                   stocktype = g.Key.stocktype,
                                   qty = - g.Sum(m => m.Field<decimal>("qty")),
                                   location = g.Key.location,
                                   roll = g.Key.roll,
                                   dyelot = g.Key.dyelot,
                               }).ToList();

            var data_Fty_2F = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = - (m.Field<decimal>("qty")),
                               location = m.Field<string>("tolocation"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            upd_Fty_4F = Prgs.UpdateFtyInventory_IO(4, null, false);
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);

            using (_transactionscope)
            using (sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F, "", upd_Fty_4F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, "", upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion 

                    #region MDeivisionPoDetail
                    upd_MD_4F = (Prgs.UpdateMPoDetail(4, null, false, sqlConn: sqlConn));
                    upd_MD_8F = (Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn));
                    upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F, "", upd_MD_4F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, "", upd_MD_8F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, "", upd_MD_2F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion 

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
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
            _transactionscope.Dispose();
            _transactionscope = null;
           
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select  a.id    
        , a.FromPoId
        , a.FromSeq1
        , a.FromSeq2
        , Fromseq = concat (Ltrim (Rtrim (a.FromSeq1)), ' ', a.FromSeq2)
        , p1.FabricType
        , stockunit = dbo.GetStockUnitBySPSeq (p1.ID, p1.seq1, p1.seq2)
        , [description] = dbo.getmtldesc (a.FromPoId, a.FromSeq1, a.FromSeq2, 2, 0)
        , a.FromRoll
        , a.FromDyelot
        , a.FromStocktype
        , a.Qty
        , a.ToPoid
        , a.ToSeq1
        , a.ToSeq2
        , toseq = concat (Ltrim (Rtrim (a.ToSeq1)), ' ', a.ToSeq2)
        , a.ToRoll
        , a.ToDyelot
        , a.ToStocktype
        , a.ToLocation
        , a.fromftyinventoryukey
        , a.ukey
        , location = dbo.Getlocation (fi.ukey)
from dbo.SubTransfer_detail a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.FromPoId 
                                             and p1.seq1 = a.FromSeq1 
                                             and p1.SEQ2 = a.FromSeq2
left join FtyInventory FI on a.FromPoid = fi.poid 
                             and a.fromSeq1 = fi.seq1 
                             and a.fromSeq2 = fi.seq2
                             and a.fromRoll = fi.roll 
                             and a.fromStocktype = fi.stocktype
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        //delete all
        private void btnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            //detailgridbs.EndEdit();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());

        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P23_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        //Accumulated
        private void btnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P23_AccumulatedQty(CurrentMaintain);
            frm.P23 = this;
            frm.ShowDialog(this);
        }

        // Locate
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("frompoid", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        protected override bool ClickPrint()
        {
            //DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }
            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string Remark = CurrentMaintain["Remark"].ToString();
            string M = CurrentMaintain["MdivisionID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Sci.Env.User.Keyword));
            DataTable dt;
            DualResult result = DBProxy.Current.Select("",
            @"select NameEn from MDivision where id = @MDivision", pars, out dt);
            if (!result) { this.ShowErr(result); }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dt");
                return false;
            }

            string RptTitle = dt.Rows[0]["NameEn"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("RptTitle", RptTitle));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", id));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", Remark));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Factory", M));
            pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@ID", id));
            #endregion
            #region -- 撈表身資料 --
            DataTable dtDetail;
            string sqlcmd = @"
select  t.frompoid
        ,t.fromseq1 + '-' +t.fromseq2 as SEQ
        ,t.topoid,t.toseq1  + '-' +t.toseq2 as TOSEQ
        ,[desc] = IIF ((p.ID = lag(p.ID,1,'')over (order by p.ID,p.seq1,p.seq2) 
                        and (p.seq1 = lag(p.seq1,1,'') over (order by p.ID,p.seq1,p.seq2)) 
                        and (p.seq2 = lag(p.seq2,1,'')over (order by p.ID,p.seq1,p.seq2))
                       ) 
                       , ''
                       , dbo.getMtlDesc(t.FromPOID,t.FromSeq1,t.FromSeq2,2,0)
                      )
        , t.fromroll
        , t.fromdyelot
        , p.StockUnit
        , [BULKLOCATION] = dbo.Getlocation(fi.ukey) 
        , t.Tolocation
        , t.Qty
        , [Total] = sum(t.Qty) OVER (PARTITION BY t.frompoid ,t.FromSeq1,t.FromSeq2 )         
from dbo.Subtransfer_detail t WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on p.id= t.FromPOID 
                                                and p.SEQ1 = t.FromSeq1 
                                                and p.seq2 = t.FromSeq2 
left join dbo.FtyInventory FI on t.fromPoid = fi.poid 
                                 and t.fromSeq1 = fi.seq1 
                                 and t.fromSeq2 = fi.seq2
                                 and t.fromRoll = fi.roll 
                                 and t.fromStocktype = stocktype
where t.id= @ID
";
            result = DBProxy.Current.Select("", sqlcmd, pars, out dtDetail);
            if (!result) { this.ShowErr(sqlcmd, result); }

            if (dtDetail == null || dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!", "DataTable dtDetail");
                return false;
            }

            // 傳 list 資料            
            List<P23_PrintData> data = dtDetail.AsEnumerable()
                .Select(row1 => new P23_PrintData()
                {
                    StockSP = row1["frompoid"].ToString().Trim(),
                    StockSEQ = row1["SEQ"].ToString().Trim(),
                    IssueSP = row1["topoid"].ToString().Trim(),
                    SEQ = row1["TOSEQ"].ToString().Trim(),
                    DESC = row1["desc"].ToString().Trim(),
                    Roll = row1["fromroll"].ToString().Trim(),
                    DYELOT = row1["fromdyelot"].ToString().Trim(),
                    Unit = row1["StockUnit"].ToString().Trim(),
                    BULKLOCATION = row1["BULKLOCATION"].ToString().Trim(),
                    INVENTORYLOCATION = row1["Tolocation"].ToString().Trim(),
                    QTY = row1["Qty"].ToString().Trim(),
                    TotalQTY = row1["Total"].ToString().Trim()
                }).ToList();

            report.ReportDataSource = data;
            #endregion
            // 指定是哪個 RDLC
            //DualResult result;
            Type ReportResourceNamespace = typeof(P23_PrintData);
            Assembly ReportResourceAssembly = ReportResourceNamespace.Assembly;
            string ReportResourceName = "P23_Print.rdlc";

            IReportResource reportresource;
            if (!(result = ReportResources.ByEmbeddedResource(ReportResourceAssembly, ReportResourceNamespace, ReportResourceName, out reportresource)))
            {
                //this.ShowException(result);
                return false;
            }

            report.ReportResource = reportresource;

            // 開啟 report view
            var frm = new Sci.Win.Subs.ReportView(report);
            frm.MdiParent = MdiParent;
            frm.Show();
            return true;
        }
    }
}