using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tems;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P66
    /// </summary>
    public partial class P66 : Win.Tems.Input6
    {
        /// <summary>
        /// P66
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P66(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            this.DefaultFilter = $" MDivisionID = '{Env.User.Keyword}'";
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
select  sfd.*,
        [AdjustQty] = sfd.QtyAfter - sfd.QtyBefore,
        sf.[Desc],
        sf.Unit,
        sf.Color,
        [Location] = Location.val
from    SemiFinishedAdjust_Detail sfd
left join   SemiFinished sf with (nolock) on sf.Poid = sfd.Poid and sf.Seq = sfd.Seq
outer apply (SELECT val =  Stuff((select distinct concat( ',',MtlLocationID)   
                                from SemiFinishedInventory_Location sfl with (nolock)
                                where sfl.POID         = sfd.POID        and
                                      sfl.Seq          = sfd.Seq         and
                                      sfl.Roll         = sfd.Roll        and
                                      sfl.Dyelot       = sfd.Dyelot      and
                                      sfl.Tone         = sfd.Tone        and
                                      sfl.StockType    = sfd.StockType
                                FOR XML PATH('')),1,1,'')  ) Location
where   sfd.ID = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorNumericColumnSettings colQtyAfter = new DataGridViewGeneratorNumericColumnSettings();
            colQtyAfter.CellValidating += (s, e) =>
            {
                this.detailgrid.EndEdit();
                decimal qtyAfter = (decimal)this.detailgrid.Rows[e.RowIndex].Cells["QtyAfter"].Value;
                decimal qtyBefore = (decimal)this.detailgrid.Rows[e.RowIndex].Cells["QtyBefore"].Value;
                this.detailgrid.Rows[e.RowIndex].Cells["AdjustQty"].Value = qtyAfter - qtyBefore;
                this.detailgrid.RefreshEdit();
            };
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(11), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("QtyBefore", header: "Original Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("QtyAfter", header: "Current Qty", decimal_places: 2, width: Widths.AnsiChars(8), settings: colQtyAfter)
            .Numeric("AdjustQty", header: "Adjust Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            ;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["IssueDate"]))
            {
                MyUtility.Msg.WarningBox("Issue Date cannot be empty.");
                return;
            }

            string sqlUpdateSemiInventory = $@"
ALTER TABLE #tmp ALTER COLUMN POID varchar(13)
ALTER TABLE #tmp ALTER COLUMN Seq varchar(6)
ALTER TABLE #tmp ALTER COLUMN Roll varchar(8)
ALTER TABLE #tmp ALTER COLUMN Tone varchar(8)
ALTER TABLE #tmp ALTER COLUMN Dyelot varchar(8)
ALTER TABLE #tmp ALTER COLUMN StockType char(1)

select  sfi.POID, sfi.Seq, sfi.Roll, sfi.Dyelot, sfi.Tone
into    #tmpCheckSemiInventory
from    SemiFinishedInventory sfi
inner join #tmp t on sfi.POID         = t.POID        and
                    sfi.Seq           = t.Seq         and
                    sfi.Roll          = t.Roll        and
                    sfi.Dyelot        = t.Dyelot      and
                    sfi.Tone          = t.Tone        and
                    sfi.StockType     = t.StockType
where   (sfi.InQty - sfi.OutQty + sfi.AdjustQty) + (t.QtyAfter - t.QtyBefore) < 0

select * from #tmpCheckSemiInventory

--如果沒有超過庫存就做庫存還原
if not exists (select 1 from #tmpCheckSemiInventory)
begin
    update sfi  set sfi.AdjustQty = sfi.AdjustQty + (t.QtyAfter - t.QtyBefore)
    from    SemiFinishedInventory sfi
    inner join #tmp t on sfi.POID          = t.POID        and
                         sfi. Seq          = t.Seq         and
                         sfi.Roll          = t.Roll        and
                         sfi.Dyelot        = t.Dyelot      and
                         sfi.Tone          = t.Tone        and
                         sfi.StockType     = t.StockType

    update  SemiFinishedAdjust 
    set Status = 'Confirmed'
        , editdate = getdate()
        , editname = '{Env.User.UserID}' 
    where ID = '{this.CurrentMaintain["ID"]}'
end
";
            DataTable dtInvShort;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), null, sqlUpdateSemiInventory, out dtInvShort);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtInvShort.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid(dtInvShort, "Balacne Qty is not enough!!");
                return;
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
            base.ClickConfirm();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            string sqlUpdateSemiInventory = $@"
ALTER TABLE #tmp ALTER COLUMN POID varchar(13)
ALTER TABLE #tmp ALTER COLUMN Seq varchar(21)
ALTER TABLE #tmp ALTER COLUMN Roll varchar(8)
ALTER TABLE #tmp ALTER COLUMN Dyelot varchar(8)
ALTER TABLE #tmp ALTER COLUMN Tone varchar(8)
ALTER TABLE #tmp ALTER COLUMN StockType char(1)

select  sfi.POID, sfi.Seq, sfi.Roll, sfi.Dyelot, sfi.Tone
into    #tmpCheckSemiInventory
from    SemiFinishedInventory sfi
inner join #tmp t on sfi.POID         = t.POID        and
                     sfi.Seq          = t.Seq         and
                     sfi.Roll         = t.Roll        and
                     sfi.Dyelot       = t.Dyelot      and
                     sfi.Tone         = t.Tone        and
                     sfi.StockType    = t.StockType
where   (sfi.InQty - sfi.OutQty + sfi.AdjustQty) - (t.QtyAfter - t.QtyBefore) < 0

select * from #tmpCheckSemiInventory

--如果沒有超過庫存就做庫存還原
if not exists (select 1 from #tmpCheckSemiInventory)
begin
    update sfi  set sfi.AdjustQty = sfi.AdjustQty - (t.QtyAfter - t.QtyBefore)
    from    SemiFinishedInventory sfi
    inner join #tmp t on sfi.POID         = t.POID        and
                         sfi.Seq          = t.Seq         and
                         sfi.Roll         = t.Roll        and
                         sfi.Dyelot       = t.Dyelot      and
                         sfi.Tone         = t.Tone        and
                         sfi.StockType    = t.StockType

    update  SemiFinishedAdjust
    set Status = 'New'
        , editdate = getdate()
        , editname = '{Env.User.UserID}' 
    where ID = '{this.CurrentMaintain["ID"]}'
end
";
            DataTable dtInvShort;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), null, sqlUpdateSemiInventory, out dtInvShort);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtInvShort.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid(dtInvShort, "Balacne Qty is not enough!!");
                return;
            }

            MyUtility.Msg.InfoBox("UnConfirmed successful");
            base.ClickUnconfirm();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("No details");
                return false;
            }

            bool isDetailKeyColEmpty = this.DetailDatas
                                        .Where(s => MyUtility.Check.Empty(s["POID"]) || MyUtility.Check.Empty(s["Seq"]) || (decimal)s["QtyAfter"] < 0)
                                        .Any();

            if (isDetailKeyColEmpty)
            {
                MyUtility.Msg.WarningBox("<SP#>, <Seq> cannot be empty, <Current Qty> cannot be less than 0.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "SJ", "SemiFinishedAdjust", (DateTime)this.CurrentMaintain["AddDate"]);
            }

            return base.ClickSaveBefore();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.detailgrid.EndEdit();
            new P66_Import((DataTable)this.detailgridbs.DataSource).ShowDialog();
        }
    }
}
