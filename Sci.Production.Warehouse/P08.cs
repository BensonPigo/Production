using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P08 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        /// <inheritdoc/>
        public P08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='B'");
            string factory = Env.User.Factory;
            string mdvision = Env.User.Keyword;

          // ChangeDetailColor();
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
            this.detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    DataRow tmp = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
                    this.OnDetailGridInsert();
                    DataRow newrow = this.detailgrid.GetDataRow(this.detailgrid.GetSelectedRowIndex());
                    newrow.ItemArray = tmp.ItemArray;
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[this.detailgrid.RowCount - 1].Cells[0];
                }
            };
        }

        /// <inheritdoc/>
        public P08(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;

          // ChangeDetailColor();
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "B";
            this.CurrentMaintain["Third"] = 1;
            this.CurrentMaintain["WhseArrival"] = DateTime.Now;
        }

        // delete前檢查

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        // edit前檢查

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            // !EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(this.CurrentMaintain["WhseArrival"]))
            {
                MyUtility.Msg.WarningBox("< Warehouse Receive Date >  can't be empty!", "Warning");
                this.dateArriveWHDate.Focus();
                return false;
            }

            #endregion 必輸檢查
            foreach (DataRow row in this.DetailDatas)
            {
                bool isFabric = row["fabrictype"].EqualString("F");
                string warningString = string.Empty;

                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningString += " Seq1 or Seq2";
                }

                if (row["seq1"].ToString().TrimStart().StartsWith("7"))
                {
                    warningString += warningString.EqualString(string.Empty) ? " Seq1 can't start with '7'" : " ,Seq1 can't start with '7'";
                }

                if (MyUtility.Check.Empty(row["StockQty"]))
                {
                    warningString += warningString.EqualString(string.Empty) ? " Receiving Qty" : ", Receiving Qty";
                }

                if (MyUtility.Check.Empty(row["stocktype"]))
                {
                    warningString += warningString.EqualString(string.Empty) ? " Stock Type" : ", Stock Type";
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                {
                    warningString += warningString.EqualString(string.Empty) ? " Roll and Dyelot" : ", Roll and Dyelot";
                }

                if (!warningString.EqualString(string.Empty))
                {
                    warningString = string.Format(@"SP#: {0} Seq#: {1}-{2} :", row["poid"], row["seq1"], row["seq2"]) + warningString + " can't be empty";
                    warningmsg.Append(warningString + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["dyelot"] = string.Empty;
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());

                return false;
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            #region 表身的資料存在Po_Supp_Detail中但是已被Junk，就要跳出訊息告知且不做任何動作
            string sqlchkPSDJunk = $@"
select distinct concat('SP#: ',p.id,', Seq#: ',Ltrim(Rtrim(p.seq1)), '-', p.seq2) as seq
from dbo.PO_Supp_Detail p WITH (NOLOCK) 
inner join #tmp t on p.id =t.poid and p.SEQ1 = t.SEQ1 and p.SEQ2 = t.SEQ2
where p.junk = 1
";

            DataTable junkdt;
            DualResult dualResult = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, sqlchkPSDJunk, out junkdt);
            if (!dualResult)
            {
                this.ShowErr(dualResult);
                return false;
            }

            if (junkdt.Rows.Count > 0)
            {
                var v = junkdt.AsEnumerable().Select(s => MyUtility.Convert.GetString(s["seq"])).ToList();
                string msgjunk = @"Below item already junk can't be receive.
" + string.Join("\r\n", v);

                MyUtility.Msg.WarningBox(msgjunk);
                return false;
            }
            #endregion

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "RF", "Receiving", (DateTime)this.CurrentMaintain["WhseArrival"]);
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

        /// <inheritdoc/>
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        // refresh

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            #region Status Label

            this.labelNotApprove.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (this.EditMode)
            {
                this.btnImport.Enabled = true;
            }
            else
            {
                this.btnImport.Enabled = false;
            }
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["stocktype"] = "B";
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnDetailGridSetup()
        {
            DataRow dr;
            #region Seq 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    IList<DataRow> x;

                    Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(this.CurrentDetailData["poid"].ToString(), this.CurrentDetailData["seq"].ToString(), "left(p.seq1,1) !='7'");
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = selepoitem.GetSelecteds();

                    this.CurrentDetailData["seq"] = x[0]["seq"];
                    this.CurrentDetailData["seq1"] = x[0]["seq1"];
                    this.CurrentDetailData["seq2"] = x[0]["seq2"];
                    this.CurrentDetailData["pounit"] = x[0]["pounit"];
                    this.CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    this.CurrentDetailData["Description"] = x[0]["Description"];
                    this.CurrentDetailData["useqty"] = x[0]["qty"];
                    this.CurrentDetailData["fabrictype"] = x[0]["fabrictype"];
                    this.CurrentDetailData.EndEdit();
                }
            };

            ts.CellValidating += (s, e) =>
                {
                    if (!this.EditMode)
                    {
                        return;
                    }

                    if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["seq"].ToString()) != 0)
                    {
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            this.CurrentDetailData["seq"] = string.Empty;
                            this.CurrentDetailData["seq1"] = string.Empty;
                            this.CurrentDetailData["seq2"] = string.Empty;
                            this.CurrentDetailData["pounit"] = string.Empty;
                            this.CurrentDetailData["stockunit"] = string.Empty;
                            this.CurrentDetailData["Description"] = string.Empty;
                            this.CurrentDetailData["useqty"] = 0m;
                        }
                        else
                        {
                            // check Seq Length
                            string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (seq.Length < 2)
                            {
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("Data not found!", "Seq");
                                return;
                            }

                            // jimmy 105/11/14
                            // gird的StockUnit照新規則 抓取值
                            if (!MyUtility.Check.Seek(
                                string.Format(
                                Prgs.SelePoItemSqlCmd() +
                                    @"and p.seq1 ='{2}'and p.seq2 = '{3}' and left(p.seq1,1) !='7'", this.CurrentDetailData["poid"], Env.User.Keyword, seq[0], seq[1]), out dr, null))
                            {
                                this.CurrentDetailData["seq"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("Data not found!", "Seq");
                                return;
                            }
                            else
                            {
                                this.CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                                this.CurrentDetailData["seq1"] = seq[0];
                                this.CurrentDetailData["seq2"] = seq[1];
                                this.CurrentDetailData["pounit"] = dr["pounit"];
                                this.CurrentDetailData["stockunit"] = dr["stockunit"];
                                this.CurrentDetailData["Description"] = dr["description"];
                                this.CurrentDetailData["useqty"] = dr["qty"];
                                this.CurrentDetailData["fabrictype"] = dr["fabrictype"];
                            }
                        }
                    }
                };

            #endregion Seq 右鍵開窗

            #region Location 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
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
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", this.CurrentDetailData["stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
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
                    this.CurrentDetailData["location"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion Location 右鍵開窗

            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Roll;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Dyelot;

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13)) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts) // 1
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9)).Get(out cbb_Roll) // 2
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8)).Get(out cbb_Dyelot) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true) // 5
            .Numeric("useqty", header: "Use Qty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 6
            .Numeric("stockqty", header: "Receiving Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 7
            .Text("Location", header: "Bulk Location", settings: ts2, iseditingreadonly: false) // 8
            .EditText("Remark", header: "Remark", width: Widths.AnsiChars(10))
            ;

            cbb_Roll.MaxLength = 8;
            cbb_Dyelot.MaxLength = 8;
            #endregion 欄位設定
        }

        // Confirm

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string upd_MD_2T = string.Empty;
            string upd_Fty_2T = string.Empty;

            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;
            DataTable datacheck;

            #region 檢查必輸欄位

            if (MyUtility.Check.Empty(this.CurrentMaintain["whseArrival"]))
            {
                MyUtility.Msg.WarningBox("< Warehouse Receive Date >  can't be empty!", "Warning");
                this.dateArriveWHDate.Focus();
                return;
            }
            #endregion

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Receiving_Detail d  WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Receiving_Detail"))
            {
                return;
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Receiving_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.StockQty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than stock qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["stockqty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            // jimmy 105/11/15
            // CONFIRM時 需抓取 Receiving_detail的StockUnit 蓋到PO_SUPP_DETAIL
            DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
            DataTable dtOut;

            sqlupd3 = string.Format(
                @"
update Receiving 
set status='Confirmed', editname = '{0}' , editdate = GETDATE()
where id = '{1}'
---detail的StockUnit 蓋到PO_SUPP_DETAIL
select distinct poid, seq1, seq2, StockUnit into #tmp2 from #tmp

merge dbo.PO_SUPP_DETAIL as a
using #tmp2 as b
on a.ID = b.poid and a.SEQ1 = b.seq1 and a.SEQ2 = b.seq2
when matched then 
update
set a.StockUnit = b.StockUnit;

drop table #tmp2
", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新倉數量
            var data_MD_2T = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = m.Sum(w => w.Field<decimal>("stockqty")),
                       }).ToList();

            #endregion

            #region 更新庫存數量  ftyinventory

            var data_Fty_2T = (from b in this.DetailDatas
                         select new
                         {
                             poid = b.Field<string>("poid"),
                             seq1 = b.Field<string>("seq1"),
                             seq2 = b.Field<string>("seq2"),
                             stocktype = b.Field<string>("stocktype"),
                             qty = b.Field<decimal>("stockqty"),
                             location = b.Field<string>("location"),
                             roll = b.Field<string>("roll"),
                             dyelot = b.Field<string>("dyelot"),
                         }).ToList();
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (transactionscope)
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, string.Empty, upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, string.Empty, upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    if (// !(result = DBProxy.Current.Execute(null, sqlupd3)))  jimmy 105/11/15
                        !(result = MyUtility.Tool.ProcessWithDatatable(detailDt, string.Join(",", detailDt.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray()), sqlupd3, out dtOut)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
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
            this.SentToVstrong_AutoWH_ACC(true);
        }

        // Unconfirm

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)this.detailgridbs.DataSource;

            string upd_MD_2F = string.Empty;
            string upd_Fty_2F = string.Empty;

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

            #region 檢查庫存項lock
            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Receiving_Detail d WITH (NOLOCK) inner join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot 
where f.lock=1 and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {4} is locked!!" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region 檢查庫存項WMSLock
            if (!Prgs.ChkWMSLock(this.CurrentMaintain["id"].ToString(), "Receiving_Detail"))
            {
                return;
            }
            #endregion

            #region UnConfirmed 先檢查WMS是否傳送成功
            DataTable dtDetail = new DataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                string sqlGetData = string.Empty;
                sqlGetData = $@"
SELECT 
 [ID] = rd.id
,[InvNo] = r.InvNo
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[StockUnit] = rd.StockUnit
,[StockQty] = rd.StockQty
,[PoUnit] = rd.PoUnit
,[ShipQty] = rd.ShipQty
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[Weight] = rd.Weight
,[StockType] = rd.StockType
,[MtlType] = Fabric.MtlTypeID
,[Ukey] = rd.Ukey
,[ETA] = r.ETA
,[WhseArrival] = r.WhseArrival
,[Status] = 'Delete'
FROM Production.dbo.Receiving_Detail rd
inner join Production.dbo.Receiving r on rd.id = r.id
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
	and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
left join Production.dbo.FtyInventory f on f.POID = rd.PoId
	and f.Seq1=rd.Seq1 and f.Seq2=rd.Seq2 
	and f.Dyelot = rd.Dyelot and f.Roll = rd.Roll
	and f.StockType = rd.StockType
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='A'
)
and r.id = '{this.CurrentMaintain["id"]}'
";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                if (!Vstrong_AutoWHAccessory.SentReceive_Detail_Delete(dtDetail, "P08"))
                {
                    return;
                }
            }
            #endregion

            #region 檢查負數庫存

            sqlcmd = string.Format(
                @"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty,d.Dyelot
from dbo.Receiving_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.POID and d.Seq1 = f.Seq1 and d.Seq2 = f.Seq2 and d.Roll = f.Roll and d.StockType = f.StockType and d.Dyelot = f.Dyelot 
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.StockQty < 0) and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than stock qty: {5}" + Environment.NewLine,
                            tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["stockqty"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(
                @"update Receiving set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新倉數量
            var data_MD_2F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("poid"),
                           Seq1 = m.First().Field<string>("seq1"),
                           Seq2 = m.First().Field<string>("seq2"),
                           Stocktype = m.First().Field<string>("stocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("stockqty")),
                       }).ToList();

            #endregion

            #region 更新庫存數量  ftyinventory
            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = -m.Field<decimal>("stockqty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

            #region 檢查資料有任一筆WMS已完成, 就不能unConfirmed
            if (!Prgs.ChkWMSCompleteTime(this.CurrentMaintain["id"].ToString(), "Receiving_Detail"))
            {
                return;
            }
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (transactionscope)
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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetal
                    upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(sqlupd3, result);
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

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
select  a.id
        , a.PoId
        , a.Seq1
        , a.Seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , (select p1.FabricType from PO_Supp_Detail p1 WITH (NOLOCK) where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as fabrictype
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as Description
        , a.Roll
        , a.Dyelot
        , ( select Round(sum(dbo.GetUnitQty(b.POUnit, StockUnit, b.Qty)), 2) as useqty 
            from po_supp_detail b WITH (NOLOCK) 
            where b.id= a.poid and b.seq1 = a.seq1 and b.seq2 = a.seq2) useqty
        , a.StockQty
        , a.StockUnit
        , a.StockType
        , a.Location
        , a.ukey 
        , a.Remark
from dbo.Receiving_Detail a WITH (NOLOCK) 
Where a.id = '{0}' ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void BtnDeleteAll_Click(object sender, EventArgs e)
        {
            ((DataTable)this.detailgridbs.DataSource).Rows.Clear();  // 清空表身資料
        }

        // Accumulated Qty
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P08_AccumulatedQty(this.CurrentMaintain);
            frm.P08 = this;
            frm.ShowDialog(this);
        }

        // find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("poid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            P08_Import form = new P08_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            form.ShowDialog(this);
            this.RenewData();
        }

        private void BtnPrintFabricSitcker_Click(object sender, EventArgs e)
        {
            var frm = new P08_PrintFabircSticker(this.CurrentMaintain["ID"].ToString());
            frm.ShowDialog(this);
        }

        /// <summary>
        ///  AutoWH Acc WebAPI for Vstrong
        /// </summary>
        private void SentToVstrong_AutoWH_ACC(bool isConfirmed)
        {
            DataTable dtDetail = new DataTable();
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                string sqlGetData = string.Empty;
                sqlGetData = $@"
SELECT 
 [ID] = rd.id
,[InvNo] = ''
,[PoId] = rd.Poid
,[Seq1] = rd.Seq1
,[Seq2] = rd.Seq2
,[Refno] = po3.Refno
,[StockUnit] = dbo.GetStockUnitBySPSeq(rd.PoId,rd.Seq1,rd.Seq2)
,[StockQty] = rd.StockQty
,[PoUnit] = ''
,[ShipQty] = 0.00
,[Color] = po3.ColorID
,[SizeCode] = po3.SizeSpec
,[Weight] = 0.00
,[StockType] = rd.StockType
,[MtlType] = Fabric.MtlTypeID
,[Ukey] = rd.Ukey
,[ETA] = null
,[WhseArrival] = null
,[Status] = case '{isConfirmed}' when 'True' then 'New' 
    when 'False' then 'Delete' end
FROM Production.dbo.Receiving_Detail rd
inner join Production.dbo.Receiving r on rd.id = r.id
inner join Production.dbo.PO_Supp_Detail po3 on po3.ID= rd.PoId 
	and po3.SEQ1=rd.Seq1 and po3.SEQ2=rd.Seq2
left join Production.dbo.FtyInventory f on f.POID = rd.PoId
	and f.Seq1=rd.Seq1 and f.Seq2=rd.Seq2 
	and f.Dyelot = rd.Dyelot and f.Roll = rd.Roll
	and f.StockType = rd.StockType
LEFT JOIN Fabric WITH (NOLOCK) ON po3.SCIRefNo=Fabric.SCIRefNo
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = rd.Poid and seq1=rd.seq1 and seq2=rd.seq2 
	and FabricType='A'
)
and r.id = '{this.CurrentMaintain["id"]}'
";

                DualResult drResult = DBProxy.Current.Select(string.Empty, sqlGetData, out dtDetail);
                if (!drResult)
                {
                    this.ShowErr(drResult);
                }

                Task.Run(() => new Vstrong_AutoWHAccessory().SentReceive_Detail_New(dtDetail, "P08"))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }
    }
}