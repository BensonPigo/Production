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
    /// P64
    /// </summary>
    public partial class P64 : Win.Tems.Input6
    {
        /// <summary>
        /// P64
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P64(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" MDivisionID = '{Env.User.Keyword}'";
            this.detailgrid.RowsAdded += this.Detailgrid_RowsAdded;
        }

        private void Detailgrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataRow curDr = this.detailgrid.GetDataRow(e.RowIndex);
            curDr["StockType"] = "B";
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();

            this.DetailSelectCommand = $@"
select  sfd.*,
        sf.Description,
        sf.Unit
from    SemiFinishedReceiving_Detail sfd
left join   SemiFinished sf with (nolock) on sf.Refno = sfd.Refno
where   sfd.ID = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region SP event
            DataGridViewGeneratorTextColumnSettings colSP = new DataGridViewGeneratorTextColumnSettings();

            colSP.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["POID"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                List<SqlParameter> par = new List<SqlParameter>() { new SqlParameter("@poid", newvalue) };
                bool isPOIDnotExists = !MyUtility.Check.Seek("select 1 from orders with (nolock) where poid = @poid", par);

                if (isPOIDnotExists)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("SP# is not exist.");
                    return;
                }

                this.CurrentDetailData["POID"] = newvalue;
            };
            #endregion

            #region Refno event
            DataGridViewGeneratorTextColumnSettings colRefno = new DataGridViewGeneratorTextColumnSettings();

            colRefno.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Refno"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                List<SqlParameter> par = new List<SqlParameter>() { new SqlParameter("@Refno", newvalue) };
                DataRow drRefno;
                bool isPOIDnotExists = !MyUtility.Check.Seek("select Description, Unit from SemiFinished with (nolock) where Refno = @Refno", par, out drRefno);

                if (isPOIDnotExists)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Refno is not exist.");
                    return;
                }

                this.CurrentDetailData["Refno"] = newvalue;
                this.CurrentDetailData["Description"] = drRefno["Description"];
                this.CurrentDetailData["Unit"] = drRefno["Unit"];
            };

            colRefno.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (!this.EditMode)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    SelectItem item = new SelectItem("select Refno, Description, Unit from SemiFinished with (nolock)", null, null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Refno"] = item.GetSelecteds()[0]["Refno"];
                    this.CurrentDetailData["Description"] = item.GetSelecteds()[0]["Description"];
                    this.CurrentDetailData["Unit"] = item.GetSelecteds()[0]["Unit"];
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion
            #region Location event

            DataGridViewGeneratorTextColumnSettings colLocation = new DataGridViewGeneratorTextColumnSettings();
            colLocation.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Win.Tools.SelectItem2 item = Prgs.SelectLocation(this.CurrentDetailData["StockType"].ToString(), this.CurrentDetailData["Location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Location"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                }
            };

            colLocation.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Location"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (this.EditMode && e.FormattedValue != null)
                {
                    this.CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = $@"
SELECT  id 
FROM    DBO.MtlLocation WITH (NOLOCK)
WHERE   StockType='{this.CurrentDetailData["stocktype"].ToString()}'
        and junk != '1'";
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
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
                    this.CurrentDetailData["Location"] = string.Join(",", trueLocation.ToArray());
                }
            };

            #endregion Location 右鍵開窗
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(11), settings: colSP)
            .Text("Refno", header: "Refno", width: Widths.AnsiChars(15), settings: colRefno)
            .EditText("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8))
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
            .Numeric("Qty", header: "Qty", decimal_places: 2, width: Widths.AnsiChars(8))
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Location", header: "Location", width: Widths.AnsiChars(15), settings: colLocation)
            ;
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.detailgrid.Rows.RemoveAt(0);
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

            using (TransactionScope transactionScope = new TransactionScope())
            {
                string sqlUpdateSemiInventory = $@"
ALTER TABLE #tmp ALTER COLUMN POID varchar(13)
ALTER TABLE #tmp ALTER COLUMN Refno varchar(21)
ALTER TABLE #tmp ALTER COLUMN Roll varchar(8)
ALTER TABLE #tmp ALTER COLUMN Dyelot varchar(8)
ALTER TABLE #tmp ALTER COLUMN StockType char(1)

--更新半成品庫存
update sfi set sfi.InQty = sfi.InQty + t.Qty
from    SemiFinishedInventory sfi
inner join #tmp t on sfi.POID         = t.POID        and
                     sfi.Refno        = t.Refno       and
                     sfi.Roll         = t.Roll        and
                     sfi.Dyelot       = t.Dyelot      and
                     sfi.StockType    = t.StockType

insert into SemiFinishedInventory(POID, Refno, Roll, Dyelot, StockType, InQty)
            select  t.POID, t.Refno, t.Roll, t.Dyelot, t.StockType, t.Qty
            from    #tmp t
            where   not exists( select 1 
                                from SemiFinishedInventory sfi 
                                where sfi.POID         = t.POID        and
                                      sfi.Refno        = t.Refno       and
                                      sfi.Roll         = t.Roll        and
                                      sfi.Dyelot       = t.Dyelot      and
                                      sfi.StockType    = t.StockType)

--SemiFinishedInventory_Location
insert into SemiFinishedInventory_Location(POID, Refno, Roll, Dyelot, StockType, MtlLocationID)
            select  t.POID, t.Refno, t.Roll, t.Dyelot, t.StockType, isnull(location.data, '')
            from    #tmp t
            outer apply(select data from dbo.SplitString(t.Location,',')) location
            where   location.data <> '' and
                    not exists( select 1 
                                from SemiFinishedInventory_Location sfil 
                                where sfil.POID         = t.POID        and
                                      sfil.Refno        = t.Refno       and
                                      sfil.Roll         = t.Roll        and
                                      sfil.Dyelot       = t.Dyelot      and
                                      sfil.StockType    = t.StockType   and
                                      sfil.MtlLocationID    = isnull(location.data, ''))

update  SemiFinishedReceiving set Status = 'Confirmed' where ID = '{this.CurrentMaintain["ID"]}'
";
                DataTable dtEmpty;
                DualResult result = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), null, sqlUpdateSemiInventory, out dtEmpty);
                if (!result)
                {
                    transactionScope.Dispose();
                    this.ShowErr(result);
                    return;
                }

                transactionScope.Complete();
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
            base.ClickConfirm();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            string sqlCheckSemiInventory = $@"
ALTER TABLE #tmp ALTER COLUMN POID varchar(13)
ALTER TABLE #tmp ALTER COLUMN Refno varchar(21)
ALTER TABLE #tmp ALTER COLUMN Roll varchar(8)
ALTER TABLE #tmp ALTER COLUMN Dyelot varchar(8)
ALTER TABLE #tmp ALTER COLUMN StockType char(1)

select  sfi.POID, sfi.Refno, sfi.Roll, sfi.Dyelot
into    #tmpCheckSemiInventory
from    SemiFinishedInventory sfi
inner join #tmp t on sfi.POID         = t.POID        and
                     sfi.Refno        = t.Refno       and
                     sfi.Roll         = t.Roll        and
                     sfi.Dyelot       = t.Dyelot      and
                     sfi.StockType    = t.StockType
where   (sfi.InQty - sfi.OutQty + sfi.AdjustQty) < t.Qty

select * from #tmpCheckSemiInventory

--如果沒有超過庫存就做庫存還原
if not exists (select 1 from #tmpCheckSemiInventory)
begin
    update sfi  set sfi.InQty = sfi.InQty - t.Qty
    from    SemiFinishedInventory sfi
    inner join #tmp t on sfi.POID         = t.POID        and
                         sfi.Refno        = t.Refno       and
                         sfi.Roll         = t.Roll        and
                         sfi.Dyelot       = t.Dyelot      and
                         sfi.StockType    = t.StockType

    update  SemiFinishedReceiving set Status = 'New' where ID = '{this.CurrentMaintain["ID"]}'
end

";
            DataTable dtInvShort;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(this.DetailDatas.CopyToDataTable(), null, sqlCheckSemiInventory, out dtInvShort);
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
                                        .Where(s => MyUtility.Check.Empty(s["POID"]) || MyUtility.Check.Empty(s["Refno"]) || MyUtility.Check.Empty(s["Qty"]))
                                        .Any();

            if (isDetailKeyColEmpty)
            {
                MyUtility.Msg.WarningBox("<SP#>, <Refno>, <Qty> cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "SR", "SemiFinishedReceiving", (DateTime)this.CurrentMaintain["AddDate"]);
            }

            return base.ClickSaveBefore();
        }
    }
}
