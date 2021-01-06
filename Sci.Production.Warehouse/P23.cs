using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using Sci.Win;
using static Sci.Production.PublicPrg.Prgs;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P23 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        /// <inheritdoc/>
        public P23(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Env.User.Keyword);
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
            this.di_stocktype.Add("B", "Bulk");
            this.di_stocktype.Add("I", "Inventory");
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Type"] = "B";
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
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
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        // save前檢查 & 取id

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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

            #endregion 必輸檢查

            foreach (DataRow row in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(row["fromseq1"]) || MyUtility.Check.Empty(row["fromseq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} can't be empty",
                        row["frompoid"], row["fromseq1"], row["fromseq2"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["Qty"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Issue Qty can't be empty",
                        row["frompoid"], row["fromseq1"], row["fromseq2"], row["fromroll"], row["fromdyelot"]) + Environment.NewLine);
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

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "PI", "SubTransfer", (DateTime)this.CurrentMaintain["Issuedate"]);
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

            this.label25.Text = this.CurrentMaintain["status"].ToString();

            #endregion Status Label

            if (this.CurrentMaintain["status"].ToString().EqualString("Confirmed"))
            {
                this.btnPrintFabricSticker.Enabled = true;
            }
            else
            {
                this.btnPrintFabricSticker.Enabled = false;
            }
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.IsSupportNegative = true;
            Ict.Win.UI.DataGridViewNumericBoxColumn col_Qty;
            Ict.Win.UI.DataGridViewTextBoxColumn col_tolocation;
            #region -- To Location 右鍵開窗 --
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.CurrentDetailData["tostocktype"].ToString(), this.CurrentDetailData["tolocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["tolocation"] = item.GetSelectedString();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["tolocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(
                        @"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK)
WHERE   StockType='{0}'
        and junk != '1'", this.CurrentDetailData["tostocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = this.CurrentDetailData["tolocation"].ToString().Split(',').Distinct().ToArray();
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
                    this.CurrentDetailData["tolocation"] = string.Join(",", trueLocation.ToArray());

                    // 去除錯誤的Location將正確的Location填回
                }
            };
            #endregion
            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("frompoid", header: "Inventory" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("fromseq", header: "Inventory" + Environment.NewLine + "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("fromroll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true) // 2
            .Text("fromdyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 4
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(5)) // 5
            .Text("Location", header: "From" + Environment.NewLine + "Location", iseditingreadonly: true) // 6
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, settings: ns).Get(out col_Qty) // 7
            .Text("topoid", header: "Bulk" + Environment.NewLine + "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 8
            .Text("toseq", header: "Bulk" + Environment.NewLine + " Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 9
            .Text("toLocation", header: "To Location", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(18)).Get(out col_tolocation) // 10
            ;
            #endregion 欄位設定
            col_Qty.DefaultCellStyle.BackColor = Color.Pink;
            col_tolocation.DefaultCellStyle.BackColor = Color.Pink;
        }

        // Confirm

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            DualResult result = Prgs.P23confirm(this.CurrentMaintain["ID"].ToString());
            if (!result)
            {
                this.ShowErr(result);
            }
            else
            {
                this.FtyBarcodeData(true);
                this.SentToGensong_AutoWHFabric(true);
                this.SentToVstrong_AutoWH_ACC(true);
                MyUtility.Msg.InfoBox("Confirmed successful");
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
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

            string upd_MD_4F = string.Empty;
            string upd_MD_8F = string.Empty;
            string upd_MD_2F = string.Empty;
            string upd_Fty_4F = string.Empty;
            string upd_Fty_2F = string.Empty;

            string sqlcmd = string.Empty, sqlupd3 = string.Empty, ids = string.Empty;
            DualResult result, result2;

            #region -- 檢查庫存項lock --
            sqlcmd = string.Format(
                @"
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
        , f.Dyelot
from dbo.SubTransfer_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on  d.toPoId = f.PoId
                                            and d.toSeq1 = f.Seq1
                                            and d.toSeq2 = f.seq2
                                            and d.toStocktype = f.StockType
                                            and d.toRoll = f.Roll
                                            and d.toDyelot = f.Dyelot
where   f.lock = 1 
        and d.Id = '{0}'", this.CurrentMaintain["id"]);
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
                            tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["Dyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Material Locked!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 檢查負數庫存 --

            sqlcmd = string.Format(
                @"
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
        , d.toDyelot
from (
		Select  topoid
                , toseq1
                , toseq2
                , toRoll
                , Qty = sum(Qty)
                , toStocktype
                , toDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty > 0
		Group by topoid, toseq1, toseq2, toRoll, toStocktype, toDyelot
) as d
left join FtyInventory f WITH (NOLOCK) on   d.toPoId = f.PoId 
                                            and d.toSeq1 = f.Seq1 
                                            and d.toSeq2 = f.seq2 
                                            and d.toStocktype = f.StockType 
                                            and d.toRoll = f.Roll
                                            and d.toDyelot = f.Dyelot
where (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) + d.Qty < 0) ", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine,
                            tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["toroll"], tmp["balanceqty"], tmp["qty"], tmp["toDyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Bulk balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            sqlcmd = string.Format(
                @"
Select  d.frompoid
        , d.fromseq1
        , d.fromseq2
        , d.fromRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
        , d.FromDyelot
from (
		Select  frompoid
                , fromseq1
                , fromseq2
                , fromRoll
                , Qty = sum (Qty)
                , FromStockType
                , FromDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty < 0
		Group by frompoid, fromseq1, fromseq2, fromRoll, FromStockType, FromDyelot
) as d 
left join FtyInventory f WITH (NOLOCK) on   d.FromPOID = f.POID 
                                            and d.FromRoll = f.Roll 
                                            and d.FromSeq1 =f.Seq1 
                                            and d.FromSeq2 = f.Seq2 
                                            AND D.FromStockType = F.StockType
                                            and d.FromDyelot = f.Dyelot
where (isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0) < -d.Qty) ", this.CurrentMaintain["id"]);
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
                            "SP#: {0} Seq#: {1}-{2} Roll#: {3} Dyelot: {6}'s balance: {4} is less than transfer qty: {5}" + Environment.NewLine,
                            tmp["frompoid"], tmp["fromseq1"], tmp["fromseq2"], tmp["fromroll"], tmp["balanceqty"], tmp["qty"], tmp["FromDyelot"]);
                    }

                    MyUtility.Msg.WarningBox("Inventory balance Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 檢查InQty - 轉倉數量 + 調整數量 < 轉出數量, 則不能unconfirmed
            sqlcmd = string.Format(
                @"
Select  d.topoid
        , d.toseq1
        , d.toseq2
        , d.toRoll
        , d.Qty
        , balanceQty = isnull (f.InQty, 0) - isnull (f.OutQty, 0) + isnull (f.AdjustQty, 0)
        , d.toDyelot
from (
		Select  topoid
                , toseq1
                , toseq2
                , toRoll
                , Qty = sum(Qty)
                , toStocktype
                , toDyelot
		from dbo.SubTransfer_Detail d WITH (NOLOCK) 
		where   Id = '{0}' 
                and Qty > 0
		Group by topoid, toseq1, toseq2, toRoll, toStocktype, toDyelot
) as d
left join FtyInventory f WITH (NOLOCK) on   d.toPoId = f.PoId 
                                            and d.toSeq1 = f.Seq1 
                                            and d.toSeq2 = f.seq2 
                                            and d.toStocktype = f.StockType 
                                            and d.toRoll = f.Roll
                                            and d.toDyelot = f.Dyelot
where (isnull (f.InQty, 0) - d.Qty + isnull (f.AdjustQty, 0)) < isnull (f.OutQty, 0) 
  ", this.CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                this.ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    ids = string.Empty;
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format(
                            "Bulk SP#: {0} {1}-{2} already exceeded the release qty({3}), cann't be unconfirm!!" + Environment.NewLine,
                            tmp["topoid"], tmp["toseq1"], tmp["toseq2"], tmp["qty"]);
                    }

                    MyUtility.Msg.WarningBox(ids, "Warning");
                    return;
                }
            }
            #endregion

            #region -- 更新表頭狀態資料 --

            sqlupd3 = string.Format(
                @"
update SubTransfer 
set status = 'New'
    , editname = '{0}' 
    , editdate = GETDATE()
where id = '{1}'", Env.User.UserID, this.CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region -- 更新MdivisionPoDetail B倉數量 --
            var data_MD_4F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("frompoid"),
                           seq1 = b.Field<string>("fromseq1"),
                           seq2 = b.Field<string>("fromseq2"),
                           stocktype = b.Field<string>("fromstocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("frompoid"),
                           Seq1 = m.First().Field<string>("fromseq1"),
                           Seq2 = m.First().Field<string>("fromseq2"),
                           Stocktype = m.First().Field<string>("fromstocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                       }).ToList();

            var data_MD_8F = data_MD_4F.Select(data => new Prgs_POSuppDetailData
            {
                Poid = data.Poid,
                Seq1 = data.Seq1,
                Seq2 = data.Seq2,
                Stocktype = data.Stocktype,
                Qty = -data.Qty,
            }).ToList();

            #endregion
            #region -- 更新mdivisionpodetail A倉數 --
            var data_MD_2F = (from b in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("Topoid"),
                           seq1 = b.Field<string>("Toseq1"),
                           seq2 = b.Field<string>("Toseq2"),
                           stocktype = b.Field<string>("Tostocktype"),
                       }
                        into m
                       select new Prgs_POSuppDetailData
                       {
                           Poid = m.First().Field<string>("Topoid"),
                           Seq1 = m.First().Field<string>("Toseq1"),
                           Seq2 = m.First().Field<string>("Toseq2"),
                           Stocktype = m.First().Field<string>("Tostocktype"),
                           Qty = -m.Sum(w => w.Field<decimal>("qty")),
                           Location = string.Join(",", m.Select(r => r.Field<string>("tolocation")).Distinct()),
                       }).ToList();

            #endregion

            #region -- 更新庫存數量  ftyinventory --
            var data_Fty_4F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                               group m by new
                               {
                                   poid = m.Field<string>("frompoid"),
                                   seq1 = m.Field<string>("fromseq1"),
                                   seq2 = m.Field<string>("fromseq2"),
                                   stocktype = m.Field<string>("fromstocktype"),
                                   location = m.Field<string>("tolocation"),
                                   roll = m.Field<string>("fromroll"),
                                   dyelot = m.Field<string>("fromdyelot"),
                               }
                                into g
                               select new
                               {
                                   poid = g.Key.poid,
                                   seq1 = g.Key.seq1,
                                   seq2 = g.Key.seq2,
                                   stocktype = g.Key.stocktype,
                                   qty = -g.Sum(m => m.Field<decimal>("qty")),
                                   location = g.Key.location,
                                   roll = g.Key.roll,
                                   dyelot = g.Key.dyelot,
                               }).ToList();

            var data_Fty_2F = (from m in ((DataTable)this.detailgridbs.DataSource).AsEnumerable()
                           select new
                           {
                               poid = m.Field<string>("topoid"),
                               seq1 = m.Field<string>("toseq1"),
                               seq2 = m.Field<string>("toseq2"),
                               stocktype = m.Field<string>("tostocktype"),
                               qty = -m.Field<decimal>("qty"),
                               location = m.Field<string>("tolocation"),
                               roll = m.Field<string>("toroll"),
                               dyelot = m.Field<string>("todyelot"),
                           }).ToList();
            upd_Fty_4F = Prgs.UpdateFtyInventory_IO(4, null, false);
            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion 更新庫存數量  ftyinventory

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
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_4F, string.Empty, upd_Fty_4F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, string.Empty, upd_Fty_2F, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDeivisionPoDetail
                    upd_MD_4F = Prgs.UpdateMPoDetail(4, null, false, sqlConn: sqlConn);
                    upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false, sqlConn: sqlConn);
                    upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false, sqlConn: sqlConn);

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_4F, string.Empty, upd_MD_4F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, string.Empty, upd_MD_8F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, string.Empty, upd_MD_2F.ToString(), out resulttb, "#TmpSource", conn: sqlConn)))
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
                    this.FtyBarcodeData(false);
                    this.SentToGensong_AutoWHFabric(false);
                    this.SentToVstrong_AutoWH_ACC(false);
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

        private void SentToGensong_AutoWHFabric(bool isConfirmed)
        {
            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                dtMain.ImportRow(this.CurrentMaintain);
                Task.Run(() => new Gensong_AutoWHFabric().SentSubTransfer_DetailToGensongAutoWHFabric(dtMain, isConfirmed))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        /// <summary>
        ///  AutoWH ACC WebAPI for Vstrong
        /// </summary>
        private void SentToVstrong_AutoWH_ACC(bool isConfirmed)
        {
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                DataTable dtMain = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();
                Task.Run(() => new Vstrong_AutoWHAccessory().SentSubTransfer_DetailToVstrongAutoWHAccessory(dtMain, isConfirmed))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private void FtyBarcodeData(bool isConfirmed)
        {
            DualResult result;
            DataTable dt = new DataTable();
            string sqlcmd = string.Empty;
            string upd_Fty_Barcode_V1 = string.Empty;
            string upd_Fty_Barcode_V2 = string.Empty;

            #region From
            sqlcmd = $@"
select i2.ID
,[Barcode1] = f.Barcode
,[OriBarcode] = fbOri.Barcode
,[balanceQty] = f.InQty-f.OutQty+f.AdjustQty
,[NewBarcode] = iif(f.Barcode ='',fbOri.Barcode,f.Barcode)
,[Poid] = i2.FromPOID
,[Seq1] = i2.FromSeq1
,[Seq2] = i2.FromSeq2
,[Roll] = i2.FromRoll
,[Dyelot] = i2.FromDyelot
,[StockType] = i2.FromStockType
from Production.dbo.SubTransfer_Detail i2
inner join Production.dbo.SubTransfer i on i2.Id=i.Id 
inner join FtyInventory f on f.POID = i2.FromPOID
    and f.Seq1 = i2.FromSeq1 and f.Seq2 = i2.FromSeq2
    and f.Roll = i2.FromRoll and f.Dyelot = i2.FromDyelot
    and f.StockType = i2.FromStockType
outer apply(
	select *
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
	and t.TransactionID = '{this.CurrentMaintain["ID"]}'
)fbOri
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.FromPoid and seq1=i2.FromSeq1 and seq2=i2.FromSeq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);
            var data_From_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                        select new
                                        {
                                            TransactionID = this.CurrentMaintain["ID"].ToString(),
                                            poid = m.Field<string>("poid"),
                                            seq1 = m.Field<string>("seq1"),
                                            seq2 = m.Field<string>("seq2"),
                                            stocktype = m.Field<string>("stocktype"),
                                            roll = m.Field<string>("roll"),
                                            dyelot = m.Field<string>("dyelot"),
                                            Barcode = m.Field<string>("NewBarcode"),
                                        }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            upd_Fty_Barcode_V1 = isConfirmed ? Prgs.UpdateFtyInventory_IO(70, null, !isConfirmed) : Prgs.UpdateFtyInventory_IO(72, null, true);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resultFrom;
            if (data_From_FtyBarcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultFrom, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_From_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultFrom, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion

            #region To

            sqlcmd = $@"
select f.Ukey
,[ToBarcode] = isnull(f.Barcode,'')
,[ToBarcode2] = isnull(Tofb.Barcode,'')
,[FromBarcode] = isnull(fromBarcode.Barcode,'')
,[FromBarcode2] = isnull(Fromfb.Barcode,'')
,[ToBalanceQty] = f.InQty-f.OutQty+f.AdjustQty
,[FromBalanceQty] = fromBarcode.BalanceQty
,[NewBarcode] = ''
,[Poid] = i2.ToPOID
,[Seq1] = i2.ToSeq1
,[Seq2] = i2.ToSeq2
,[Roll] = i2.ToRoll
,[Dyelot] = i2.ToDyelot
,[StockType] = i2.ToStockType
from Production.dbo.SubTransfer_Detail i2
inner join Production.dbo.SubTransfer i on i2.Id=i.Id 
left join FtyInventory f on f.POID = i2.ToPOID
    and f.Seq1 = i2.ToSeq1 and f.Seq2 = i2.ToSeq2
    and f.Roll = i2.ToRoll and f.Dyelot = i2.ToDyelot
    and f.StockType = i2.ToStockType

outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = f.Ukey
)Tofb
outer apply(
	select f2.Barcode 
	,BalanceQty = f2.InQty-f2.OutQty+f2.AdjustQty
	,f2.Ukey
	from FtyInventory f2	
	where f2.POID = i2.FromPOID
	and f2.Seq1 = i2.FromSeq1 and f2.Seq2 = i2.FromSeq2
	and f2.Roll = i2.FromRoll and f2.Dyelot = i2.FromDyelot
	and f2.StockType = i2.FromStockType
)fromBarcode
outer apply(
	select Barcode = MAX(Barcode)
	from FtyInventory_Barcode t
	where t.Ukey = fromBarcode.Ukey
)Fromfb
where 1=1
and exists(
	select 1 from Production.dbo.PO_Supp_Detail 
	where id = i2.ToPoid and seq1=i2.ToSeq1 and seq2=i2.ToSeq2 
	and FabricType='F'
)
and i2.id ='{this.CurrentMaintain["ID"]}'
";
            DBProxy.Current.Select(string.Empty, sqlcmd, out dt);

            foreach (DataRow dr in dt.Rows)
            {
                string strBarcode = string.Empty;

                // 目標有自己的Barcode, 則Ftyinventory跟記錄都是用自己的
                if (!MyUtility.Check.Empty(dr["ToBarcode"]) || !MyUtility.Check.Empty(dr["ToBarcode2"]))
                {
                    strBarcode = MyUtility.Check.Empty(dr["ToBarcode2"]) ? dr["ToBarcode"].ToString() : dr["ToBarcode2"].ToString();
                    dr["NewBarcode"] = strBarcode;
                }
                else
                {
                    // 目標沒Barcode, 則 來源有餘額(部分轉)就用來源Barocde_01++, 如果全轉就用來源Barocde
                    strBarcode = MyUtility.Check.Empty(dr["FromBarcode2"]) ? dr["FromBarcode"].ToString() : dr["FromBarcode2"].ToString();

                    // InQty-Out+Adj != 0 代表非整卷, 要在Barcode後+上-01,-02....
                    if (!MyUtility.Check.Empty(dr["FromBalanceQty"]))
                    {
                        if (strBarcode.Contains("-"))
                        {
                            dr["NewBarcode"] = strBarcode.Substring(0, 13) + "-" + Prgs.GetNextValue(strBarcode.Substring(14, 2), 1);
                        }
                        else
                        {
                            dr["NewBarcode"] = MyUtility.Check.Empty(strBarcode) ? string.Empty : strBarcode + "-01";
                        }
                    }
                    else
                    {
                        // 如果InQty-Out+Adj = 0 代表整卷發出就使用原本Barcode
                        dr["NewBarcode"] = strBarcode;
                    }
                }
            }

            var data_To_FtyBarcode = (from m in dt.AsEnumerable().Where(s => s["NewBarcode"].ToString() != string.Empty)
                                      select new
                                      {
                                          TransactionID = this.CurrentMaintain["ID"].ToString(),
                                          poid = m.Field<string>("poid"),
                                          seq1 = m.Field<string>("seq1"),
                                          seq2 = m.Field<string>("seq2"),
                                          stocktype = m.Field<string>("stocktype"),
                                          roll = m.Field<string>("roll"),
                                          dyelot = m.Field<string>("dyelot"),
                                          Barcode = m.Field<string>("NewBarcode"),
                                      }).ToList();

            // confirmed 要刪除Barcode, 反之則從Ftyinventory_Barcode補回
            upd_Fty_Barcode_V1 = Prgs.UpdateFtyInventory_IO(70, null, isConfirmed);
            upd_Fty_Barcode_V2 = Prgs.UpdateFtyInventory_IO(71, null, isConfirmed);
            DataTable resultTo;
            if (data_To_FtyBarcode.Count >= 1)
            {
                // 需先更新upd_Fty_Barcode_V1, 才能更新upd_Fty_Barcode_V2, 順序不能變
                if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V1, out resultTo, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }

                if (!(result = MyUtility.Tool.ProcessWithObject(data_To_FtyBarcode, string.Empty, upd_Fty_Barcode_V2, out resultTo, "#TmpSource")))
                {
                    this.ShowErr(result);
                    return;
                }
            }
            #endregion
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
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
                             and a.fromDyelot = fi.Dyelot
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        // delete all
        private void BtnClearQtyIsEmpty_Click(object sender, EventArgs e)
        {
            this.detailgrid.ValidateControl();

            // detailgridbs.EndEdit();
            ((DataTable)this.detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P23_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        // Accumulated
        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            var frm = new P23_AccumulatedQty(this.CurrentMaintain);
            frm.P23 = this;
            frm.ShowDialog(this);
        }

        // Locate
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.detailgridbs.DataSource))
            {
                return;
            }

            int index = this.detailgridbs.Find("frompoid", this.txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            {
                MyUtility.Msg.WarningBox("Data was not found!!");
            }
            else
            {
                this.detailgridbs.Position = index;
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override bool ClickPrint()
        {
            // DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            DataRow row = this.CurrentMaintain;
            string id = row["ID"].ToString();
            string remark = this.CurrentMaintain["Remark"].ToString();
            string m = this.CurrentMaintain["MdivisionID"].ToString();
            string issuedate = ((DateTime)MyUtility.Convert.GetDate(row["issuedate"])).ToShortDateString();
            #region -- 撈表頭資料 --
            List<SqlParameter> pars = new List<SqlParameter>();
            pars.Add(new SqlParameter("@MDivision", Env.User.Keyword));
            DataTable dt;
            DualResult result = DBProxy.Current.Select(
                string.Empty,
                @"select NameEn from MDivision where id = @MDivision", pars, out dt);
            if (!result)
            {
                this.ShowErr(result);
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found!!!", "DataTable dt");
                return false;
            }

            string rptTitle = dt.Rows[0]["NameEn"].ToString();
            ReportDefinition report = new ReportDefinition();
            report.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
            report.ReportParameters.Add(new ReportParameter("ID", id));
            report.ReportParameters.Add(new ReportParameter("Remark", remark));
            report.ReportParameters.Add(new ReportParameter("issuedate", issuedate));
            report.ReportParameters.Add(new ReportParameter("Factory", m));
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
                                 and t.fromStocktype = FI.stocktype
                                 and t.fromDyelot = FI.Dyelot
where t.id= @ID
order by t.frompoid,SEQ,BULKLOCATION,t.fromroll,t.FromDyelot
";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, pars, out dtDetail);
            if (!result)
            {
                this.ShowErr(sqlcmd, result);
            }

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
                    QTY = Convert.ToDecimal(row1["Qty"].ToString()),
                    TotalQTY = row1["Total"].ToString().Trim(),
                }).ToList();

            report.ReportDataSource = data;
            #endregion

            // 指定是哪個 RDLC
            // DualResult result;
            Type reportResourceNamespace = typeof(P23_PrintData);
            Assembly reportResourceAssembly = reportResourceNamespace.Assembly;
            string reportResourceName = "P23_Print.rdlc";

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

        private void BtnPrintFabricSticker_Click(object sender, EventArgs e)
        {
            new P23_FabricSticker(this.CurrentMaintain["ID"]).ShowDialog();
        }
    }
}