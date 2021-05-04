using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P26 : Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();

        private class NowDetail
        {
            public string POID { get; set; }

            public string Seq1 { get; set; }

            public string Seq2 { get; set; }

            public List<string> DB_CLocations { get; set; }
        }

        /// <inheritdoc/>
        public P26(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("MDivisionID = '{0}'", Env.User.Keyword);

            this.gridicon.Append.Enabled = false;
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Enabled = false;
            this.gridicon.Insert.Visible = false;
        }

        /// <inheritdoc/>
        public P26(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            this.InitializeComponent();
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

        // 新增時預設資料

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["IssueDate"] = DateTime.Now;
            this.CurrentMaintain["Status"] = "New";
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
         // DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            // Check ToLocation is not empty
            for (int i = ((DataTable)this.detailgridbs.DataSource).Rows.Count - 1; i >= 0; i--)
            {
                if (((DataTable)this.detailgridbs.DataSource).Rows[i]["ToLocation"].Empty())
                {
                    ((DataTable)this.detailgridbs.DataSource).Rows[i].Delete();
                }
            }

            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            // 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = MyUtility.GetValue.GetID(Env.User.Keyword + "LH", "LocationTrans", (DateTime)this.CurrentMaintain["Issuedate"], sequenceMode: 2);
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
        }

        // Detail Grid 設定

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            #region stocktype validating
            DataGridViewGeneratorComboBoxColumnSettings stocktypeSet = new DataGridViewGeneratorComboBoxColumnSettings();
            stocktypeSet.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["stocktype"]))
                    {
                        return;
                    }

                    string getFtyInventorySql = $@"
select 
[Qty] = InQty - OutQty + AdjustQty - ReturnQty,
[fromlocation] = dbo.Getlocation(ukey) 
from FtyInventory
where
Poid = '{this.CurrentDetailData["poid"]}' and 
Seq1 = '{this.CurrentDetailData["Seq1"]}' and 
seq2  = '{this.CurrentDetailData["seq2"]}' and 
Roll = '{this.CurrentDetailData["Roll"]}' and 
stocktype = '{e.FormattedValue}'
";
                    DataRow dr;
                    if (MyUtility.Check.Seek(getFtyInventorySql, out dr))
                    {
                        this.CurrentDetailData["qty"] = dr["Qty"];
                        this.CurrentDetailData["FromLocation"] = dr["fromlocation"];
                        this.CurrentDetailData["stocktype"] = e.FormattedValue;
                        this.CurrentDetailData["ToLocation"] = string.Empty;
                    }
                    else
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"<Stock Type> data not found");
                        return;
                    }
                }
            };

            #endregion

            #region Location 右鍵開窗

            DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.CurrentDetailData["stocktype"].ToString(), this.CurrentDetailData["tolocation"].ToString());
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
        and junk != '1'", this.CurrentDetailData["stocktype"].ToString());
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
            #endregion Location 右鍵開窗

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            #region 欄位設定

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true) // 0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true) // 1
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true) // 2
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true) // 3
            .Text("Refno", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true) // 3
            .EditText("Description", header: "Description", width: Widths.AnsiChars(15), iseditingreadonly: true) // 4
            .Text("colorid", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true) // 5
            .Text("SizeSpec", header: "SizeSpec", width: Widths.AnsiChars(5), iseditingreadonly: true) // 6
            .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true) // 7
            .ComboBox("stocktype", header: "Stock" + Environment.NewLine + "Type", width: Widths.AnsiChars(8), iseditable: true, settings: stocktypeSet).Get(out cbb_stocktype) // 8
            .Text("FromLocation", header: "FromLocation", iseditingreadonly: true) // 9
            .Text("ToLocation", header: "ToLocation", settings: ts2, iseditingreadonly: false, width: Widths.AnsiChars(14)) // 10
            ;

            #endregion 欄位設定
            DataTable stocktypeSrc;
            string stocktypeGetSql = "select ID = replace(ID,'''',''), Name = rtrim(Name) from DropDownList WITH (NOLOCK) where Type = 'Pms_StockType' order by Seq";
            DBProxy.Current.Select(null, stocktypeGetSql, out stocktypeSrc);
            cbb_stocktype.DataSource = stocktypeSrc;
            cbb_stocktype.ValueMember = "ID";
            cbb_stocktype.DisplayMember = "Name";

            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["stocktype"].DefaultCellStyle.BackColor = Color.Pink;
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

            #region 排除Location 包含WMS & 非WMS資料

            DataTable dtToWMSChk = (DataTable)this.detailgridbs.DataSource;

            string sqlcmd = @"
select * from
(
select * 
	, rowCnt = ROW_NUMBER() over(Partition by POID,Seq1,Seq2,Roll,Dyelot,ToLocation order by IsWMS)
	from (
		select distinct t.POID,t.Seq1,t.Seq2,t.Roll,t.Dyelot,IsWMS = isnull( ml.IsWMS,0),t.ToLocation
		from #tmp t
		outer apply(
			select ml.IsWMS
			from MtlLocation ml
			inner join dbo.SplitString(t.ToLocation,',') sp on sp.Data = ml.ID
		)ml
	) a
) final
where rowCnt = 2

drop table #tmp
";
            DualResult result1;
            DataTable dtCheck;
            string errmsg = string.Empty;

            if (!(result1 = MyUtility.Tool.ProcessWithDatatable(dtToWMSChk, string.Empty, sqlcmd, out dtCheck)))
            {
                MyUtility.Msg.WarningBox(result1.Messages.ToString());
                return;
            }
            else
            {
                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in dtCheck.Rows)
                    {
                        errmsg += $@"SP#: {tmp["poid"]} Seq#: {tmp["seq1"]}-{tmp["seq2"]} Roll#: {tmp["roll"]} Dyelot: {tmp["Dyelot"]} ToLocation: {tmp["ToLocation"]}" + Environment.NewLine;
                    }

                    MyUtility.Msg.WarningBox("These material exists in WMS Location and non-WMS location in same time , please revise below detail location column data." + Environment.NewLine + errmsg, "Warning");
                    return;
                }
            }
            #endregion

            string sqlComfirmUpdate = string.Empty;
            sqlComfirmUpdate = string.Format(
                @"
update dbo.LocationTrans set status='Confirmed', editname = '{0}' , editdate = GETDATE() where id = '{1}'
",
                Env.User.UserID, this.CurrentMaintain["id"]);

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DualResult result = DBProxy.Current.Execute(null, sqlComfirmUpdate);
                    if (!result)
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    result = Prgs.UpdateFtyInventoryMDivisionPoDetail(this.DetailDatas);
                    if (!result)
                    {
                        transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }

            DataTable dt = this.CurrentMaintain.Table.AsEnumerable().Where(s => s["ID"] == this.CurrentMaintain["ID"]).CopyToDataTable();

            // AutoWHFabric WebAPI for Gensong
            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                Task.Run(() => new Gensong_AutoWHFabric().SentLocationTrans_Detail_New(dt, "New"))
               .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            // AutoWHACC WebAPI for Vstrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                Task.Run(() => new Vstrong_AutoWHAccessory().SentLocationTrans_detail_New(dt, "New"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }

            // 若location 不是自動倉,且Location 有變更, 要發給WMS做撤回(Delete)
            DataTable dtToWMS = ((DataTable)this.detailgridbs.DataSource).Clone();
            foreach (DataRow dr2 in this.DetailDatas)
            {
                string sqlchk = $@"
select * from MtlLocation m
inner join SplitString('{dr2["ToLocation"]}',',') sp on m.ID = sp.Data
where m.IsWMS = 0";
                if (MyUtility.Check.Seek(sqlchk))
                {
                    dtToWMS.ImportRow(dr2);
                }
            }

            if (Gensong_AutoWHFabric.IsGensong_AutoWHFabricEnable)
            {
                Task.Run(() => new Gensong_AutoWHFabric().SentReceive_Location_Update(dtToWMS))
           .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            }

            // AutoWH ACC WebAPI for VStrong
            if (Vstrong_AutoWHAccessory.IsVstrong_AutoWHAccessoryEnable)
            {
                Task.Run(() => new Vstrong_AutoWHAccessory().SentReceive_Location_Update(dtToWMS))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
        }

        // 寫明細撈出的sql command

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(
                @"
select a.id
	,a.PoId
	,a.Seq1
	,a.Seq2
	,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
	, p1.colorid
	, p1.sizespec
	,a.Roll
	,a.Dyelot
	,a.Qty
	,a.stocktype
	,a.FromLocation
	,a.ToLocation
	,a.ftyinventoryukey
	,a.ukey
	,p1.Refno
	,dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description]
from dbo.LocationTrans_detail a WITH (NOLOCK) 
outer apply
(
	select p1.colorid, p1.sizespec, p1.Refno
	from PO_Supp_Detail p1 WITH (NOLOCK) 
	where p1.ID = a.PoId 
	and p1.seq1 = a.SEQ1 
	and p1.SEQ2 = a.seq2
)p1
Where a.id = '{0}' ", masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            var frm = new P26_Import(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }
    }
}