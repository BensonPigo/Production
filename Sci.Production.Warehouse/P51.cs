using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P51 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();

        /// <inheritdoc/>
        public P51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("Type='B' and MDivisionID = '{0}'", Env.User.Keyword);
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.di_fabrictype.Add("O", "Other");
        }

        /// <inheritdoc/>
        public P51(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("id='{0}'", transID);
            this.di_fabrictype.Add("F", "Fabric");
            this.di_fabrictype.Add("A", "Accessory");
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            MyUtility.Tool.SetupCombox(this.comboStockType, 2, 1, "B,Bulk,I,Inventory");
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
            this.CurrentMaintain["stocktype"] = "B";
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

        private void MySubreportEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            e.DataSources.Add(new ReportDataSource("DataSet1", (DataTable)this.detailgridbs.DataSource));
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
                if (MyUtility.Check.Empty(row["poid"]) || MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Seq1 or Seq2 can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"])
                        + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["stocktype"]))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Stock Type can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                {
                    warningmsg.Append(string.Format(
                        @"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Roll and Dyelot can't be empty",
                        row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["roll"] = string.Empty;
                    row["dyelot"] = string.Empty;
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "SB", "StockTaking", (DateTime)this.CurrentMaintain["Issuedate"]);
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
        }

        // detail 新增時設定預設值

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            this.CurrentDetailData["stocktype"] = this.CurrentMaintain["stocktype"].ToString();
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:ParametersMustBeOnSameLineOrSeparateLines", Justification = "Reviewed.")]
        protected override void OnDetailGridSetup()
        {
            DataRow dr;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            #region Seq 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    // string sqlcmd = "";
                    IList<DataRow> x;

                    Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(this.CurrentDetailData["poid"].ToString(), this.CurrentDetailData["seq"].ToString(), "f.MDivisionID = '{1}'");
                    DialogResult result = selepoitem.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = selepoitem.GetSelecteds();

                    this.CurrentDetailData["seq"] = x[0]["seq"];
                    this.CurrentDetailData["seq1"] = x[0]["seq1"];
                    this.CurrentDetailData["seq2"] = x[0]["seq2"];
                    this.CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    this.CurrentDetailData["fabrictype"] = x[0]["fabrictype"];
                    this.CurrentDetailData["colorid"] = x[0]["colorid"];
                    this.CurrentDetailData["refno"] = x[0]["refno"];
                    this.CurrentDetailData["qtybefore"] = 0m;
                    this.CurrentDetailData["qtyafter"] = 0m;
                    this.CurrentDetailData["ftyinventoryukey"] = 0;
                    this.CurrentDetailData["roll"] = string.Empty;
                    this.CurrentDetailData["dyelot"] = string.Empty;
                    this.CurrentDetailData["Location"] = string.Empty;
                    this.CurrentDetailData["description"] = string.Empty;
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
                        this.CurrentDetailData["stockunit"] = string.Empty;
                        this.CurrentDetailData["fabrictype"] = string.Empty;
                        this.CurrentDetailData["colorid"] = string.Empty;
                        this.CurrentDetailData["refno"] = string.Empty;
                        this.CurrentDetailData["qtybefore"] = 0m;
                        this.CurrentDetailData["qtyafter"] = 0m;
                        this.CurrentDetailData["ftyinventoryukey"] = 0;
                        this.CurrentDetailData["roll"] = string.Empty;
                        this.CurrentDetailData["dyelot"] = string.Empty;
                        this.CurrentDetailData["Location"] = string.Empty;
                        this.CurrentDetailData["description"] = string.Empty;
                    }
                    else
                    {
                        // check Seq Length
                        string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' });
                        if (seq.Length < 2)
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");

                            return;
                        }

                        if (!MyUtility.Check.Seek(
                            string.Format(
                            Prgs.SelePoItemSqlCmd() +
                                    @"and f.MDivisionID = '{1}' and p.seq1 ='{2}' and p.seq2 = '{3}'", this.CurrentDetailData["poid"], Env.User.Keyword, seq[0], seq[1]), out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Seq");
                            return;
                        }
                        else
                        {
                            this.CurrentDetailData["seq"] = seq[0] + " " + seq[1];
                            this.CurrentDetailData["seq1"] = seq[0];
                            this.CurrentDetailData["seq2"] = seq[1];
                            this.CurrentDetailData["stockunit"] = dr["stockunit"];
                            this.CurrentDetailData["fabrictype"] = dr["fabrictype"];
                            this.CurrentDetailData["colorid"] = dr["colorid"];
                            this.CurrentDetailData["refno"] = dr["refno"];
                            this.CurrentDetailData["qtybefore"] = 0m;
                            this.CurrentDetailData["qtyafter"] = 0m;
                            this.CurrentDetailData["ftyinventoryukey"] = 0;
                            this.CurrentDetailData["roll"] = string.Empty;
                            this.CurrentDetailData["dyelot"] = string.Empty;
                            this.CurrentDetailData["Location"] = string.Empty;
                            this.CurrentDetailData["description"] = string.Empty;
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗

            #region roll# 右鍵開窗 & valid
            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right
                    && !MyUtility.Check.Empty(this.CurrentDetailData["poid"])
                    && !MyUtility.Check.Empty(this.CurrentDetailData["seq"]))
                {
                    // bug fix:364: WAREHOUSE_P51_Warehouse Backward Stocktaking
                    if (MyUtility.Check.Empty(this.CurrentDetailData["stocktype"]))
                    {
                        this.CurrentDetailData["stocktype"] = this.CurrentMaintain["stocktype"].ToString();
                    }

                    string sqlcmd = string.Format(
                        @"select a.ukey,a.roll,a.dyelot, a.inqty - a.outqty + a.adjustqty - a.ReturnQty qty 
                                 ,dbo.Getlocation(a.ukey) as location 
                                 ,dbo.getmtldesc('{0}','{1}','{2}',2,0) as [description]
                                        from dbo.ftyinventory a WITH (NOLOCK) 
                                        where poid='{0}' and seq1='{1}' and seq2='{2}' 
                                        and stocktype='{3}' and lock =0",
                        this.CurrentDetailData["poid"],
                        this.CurrentDetailData["seq1"],
                        this.CurrentDetailData["seq2"],
                        this.CurrentDetailData["stocktype"]);
                    IList<DataRow> x;
                    Win.Tools.SelectItem selepoitem2 = new Win.Tools.SelectItem(
                        sqlcmd,
                        "Ukey,Roll,Dyelot,Balance,Location", this.CurrentDetailData["roll"].ToString());

                    DialogResult result = selepoitem2.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    x = selepoitem2.GetSelecteds();

                    this.CurrentDetailData["ftyinventoryukey"] = x[0]["ukey"];
                    this.CurrentDetailData["roll"] = x[0]["roll"];
                    this.CurrentDetailData["dyelot"] = x[0]["dyelot"];
                    this.CurrentDetailData["qtybefore"] = x[0]["qty"];
                    this.CurrentDetailData["qtyafter"] = 0m;
                    this.CurrentDetailData["Location"] = x[0]["Location"];
                    this.CurrentDetailData["description"] = x[0]["description"];
                }
            };
            ts2.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(this.CurrentDetailData["poid"]) || MyUtility.Check.Empty(this.CurrentDetailData["seq"]))
                {
                    MyUtility.Msg.WarningBox("Please fill < SP# > , < Seq > first!", "Warning");
                    return;
                }

                if (string.Compare(e.FormattedValue.ToString(), this.CurrentDetailData["roll"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        this.CurrentDetailData["ftyinventoryukey"] = 0;
                        this.CurrentDetailData["roll"] = string.Empty;
                        this.CurrentDetailData["dyelot"] = string.Empty;
                        this.CurrentDetailData["qtybefore"] = 0m;
                        this.CurrentDetailData["qtyafter"] = 0m;
                        this.CurrentDetailData["Location"] = string.Empty;
                        this.CurrentDetailData["description"] = string.Empty;
                    }
                    else
                    {
                        // bug fix:364: WAREHOUSE_P51_Warehouse Backward Stocktaking
                        if (MyUtility.Check.Empty(this.CurrentDetailData["stocktype"]))
                        {
                            this.CurrentDetailData["stocktype"] = this.CurrentMaintain["stocktype"].ToString();
                        }

                        string cmd = $@"
select a.ukey,a.roll,a.dyelot
    ,a.inqty - a.outqty + a.adjustqty - a.ReturnQty qty
    ,dbo.Getlocation(a.ukey) as location 
    ,dbo.getmtldesc('{this.CurrentDetailData["poid"]}','{this.CurrentDetailData["seq1"]}','{this.CurrentDetailData["seq2"]}',2,0) as [description] 
from dbo.ftyinventory a WITH (NOLOCK) where poid='{this.CurrentDetailData["poid"]}' and seq1='{this.CurrentDetailData["seq1"]}' and seq2='{this.CurrentDetailData["seq2"]}' 
and stocktype='{this.CurrentDetailData["stocktype"]}' and roll='{e.FormattedValue.ToString()}' and lock =0
";

                        if (!MyUtility.Check.Seek(cmd, out dr, null))
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found! or Item is lock!!", "Roll#");
                            return;
                        }
                        else
                        {
                            this.CurrentDetailData["ftyinventoryukey"] = dr["ukey"];
                            this.CurrentDetailData["roll"] = e.FormattedValue;
                            this.CurrentDetailData["dyelot"] = dr["dyelot"];
                            this.CurrentDetailData["qtybefore"] = dr["qty"];
                            this.CurrentDetailData["qtyafter"] = 0m;
                            this.CurrentDetailData["Location"] = dr["Location"];
                            this.CurrentDetailData["description"] = dr["description"];
                        }
                    }
                }
            };
            #endregion

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .CellPOIDWithSeqRollDyelot("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: false, alignment: null, checkMDivisionID: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: false, settings: ts) // 1
            .Text("roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: false, settings: ts2) // 2
            .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Text("Location", header: "Book Location", iseditingreadonly: true) // 4
            .Numeric("qtybefore", header: "Book Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 5
            .Numeric("qtyafter", header: "Actual Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10) // 6
            .Numeric("variance", header: "Variance", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
            .Text("refno", header: "Ref#", iseditingreadonly: true) // 8
            .Text("Colorid", header: "Color", iseditingreadonly: true) // 9
            .Text("stockunit", header: "Stock Unit", iseditingreadonly: true) // 10
            .ComboBox("FabricType", header: "Fabric Type", iseditable: false).Get(out cbb_fabrictype) // 11
            .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true) // 12
            ;
            #endregion 欄位設定

            cbb_fabrictype.DataSource = new BindingSource(this.di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
        }

        // Confirm

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return;
            }

            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            DualResult result;
            #region store procedure parameters
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            System.Data.SqlClient.SqlParameter sp_StocktakingID = new System.Data.SqlClient.SqlParameter();
            sp_StocktakingID.ParameterName = "@StocktakingID";
            sp_StocktakingID.Value = dr["id"].ToString();
            cmds.Add(sp_StocktakingID);
            System.Data.SqlClient.SqlParameter sp_mdivision = new System.Data.SqlClient.SqlParameter();
            sp_mdivision.ParameterName = "@MDivisionid";
            sp_mdivision.Value = Env.User.Keyword;
            cmds.Add(sp_mdivision);
            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@Factoryid";
            sp_factory.Value = Env.User.Factory;
            cmds.Add(sp_factory);
            System.Data.SqlClient.SqlParameter sp_loginid = new System.Data.SqlClient.SqlParameter();
            sp_loginid.ParameterName = "@loginid";
            sp_loginid.Value = Env.User.UserID;
            cmds.Add(sp_loginid);
            #endregion
            if (!(result = DBProxy.Current.ExecuteSP(string.Empty, "dbo.usp_StocktakingEncode", cmds)))
            {
                // MyUtility.Msg.WarningBox(result.Messages[1].ToString());
                Exception ex = result.GetException();
                MyUtility.Msg.WarningBox(ex.Message);
                return;
            }
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"select a.id
,a.PoId,a.Seq1,a.Seq2
,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Roll
,a.Dyelot
,dbo.Getlocation(fi.ukey) location
,a.QtyBefore
,a.QtyAfter
,a.QtyAfter - a.QtyBefore as variance
,a.StockType
,p1.Refno
,p1.colorid
,p1.FabricType
,p1.stockunit
,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
,a.ukey
,a.ftyinventoryukey
from dbo.StockTaking_detail as a WITH (NOLOCK) 
left join PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
left join FtyInventory FI on a.poid = fi.poid and a.seq1 = fi.seq1 and a.seq2 = fi.seq2
    and a.roll = fi.roll and a.stocktype = fi.stocktype and a.Dyelot = fi.Dyelot
Where a.id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void ComboStockType_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.comboStockType.SelectedValue) && this.comboStockType.SelectedValue != this.comboStockType.OldValue)
            {
                if (this.detailgridbs.DataSource != null && this.DetailDatas.Count > 0)
                {
                    foreach (DataRow dr in this.DetailDatas)
                    {
                        dr.Delete();
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P51_Print p = new P51_Print();
            p.CurrentDataRow = this.CurrentDataRow;
            p.ShowDialog();

            return true;
        }
    }
}