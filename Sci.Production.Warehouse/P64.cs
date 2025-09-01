using Ict;
using Ict.Win;
using Microsoft.Reporting.WinForms;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win;
using Sci.Win.Tems;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private DualResult result;

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

        /// <inheritdoc/>
        public P64(ToolStripMenuItem menuitem, string transID)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" id='{transID}' AND MDivisionID  = '{Sci.Env.User.Keyword}'";
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
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
        sf.[Desc],
        sf.Unit,
        sf.Color
from    SemiFinishedReceiving_Detail sfd
left join   SemiFinished sf with (nolock) on sf.PoID = sfd.Poid
and sf.Seq = sfd.Seq
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

            #region Seq event
            DataGridViewGeneratorTextColumnSettings colSeq = new DataGridViewGeneratorTextColumnSettings();

            colSeq.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["Seq"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                List<SqlParameter> par = new List<SqlParameter>()
                {
                    new SqlParameter("@POID", this.CurrentDetailData["POID"]),
                    new SqlParameter("@Seq", newvalue),
                };
                DataRow drSeq;
                bool isPOIDnotExists = !MyUtility.Check.Seek("select [Desc], Unit,Color from SemiFinished with (nolock) where Seq = @Seq and Poid = @POID", par, out drSeq);

                if (isPOIDnotExists)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Seq is not exist.");
                    return;
                }

                this.CurrentDetailData["Seq"] = newvalue;
                this.CurrentDetailData["Desc"] = drSeq["Desc"];
                this.CurrentDetailData["Unit"] = drSeq["Unit"];
                this.CurrentDetailData["Color"] = drSeq["Color"];
            };

            colSeq.EditingMouseDown += (s, e) =>
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
                    SelectItem item = new SelectItem($@"select Seq, [Desc], Unit, Color from SemiFinished with (nolock) where poid = '{this.CurrentDetailData["POID"]}'", null, null);
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Seq"] = item.GetSelecteds()[0]["Seq"];
                    this.CurrentDetailData["Desc"] = item.GetSelecteds()[0]["Desc"];
                    this.CurrentDetailData["Unit"] = item.GetSelecteds()[0]["Unit"];
                    this.CurrentDetailData["Color"] = item.GetSelecteds()[0]["Color"];
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
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), settings: colSP)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), settings: colSeq)
              .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8))
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8))
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8))
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
            // 從DB取得最新Status, 避免多工時, 畫面上資料不是最新的狀況
            this.RenewData();
            if (this.CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");

                // 重新整理畫面
                this.OnRefreshClick();
                return false;
            }

            // 重新整理畫面
            this.OnRefreshClick();
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
ALTER TABLE #tmp ALTER COLUMN Seq varchar(6)
ALTER TABLE #tmp ALTER COLUMN Roll varchar(8)
ALTER TABLE #tmp ALTER COLUMN Dyelot varchar(8)
ALTER TABLE #tmp ALTER COLUMN Tone varchar(8)
ALTER TABLE #tmp ALTER COLUMN StockType char(1)

--更新半成品庫存
update sfi set sfi.InQty = sfi.InQty + t.Qty
from    SemiFinishedInventory sfi
inner join #tmp t on sfi.POID         = t.POID        and
                     sfi.Seq          = t.Seq         and
                     sfi.Roll         = t.Roll        and
                     sfi.Dyelot       = t.Dyelot      and
                     sfi.Tone         = t.Tone        and
                     sfi.StockType    = t.StockType

insert into SemiFinishedInventory(POID, Seq, Roll, Dyelot, Tone, StockType, InQty)
            select  t.POID, t.Seq, t.Roll, t.Dyelot, t.Tone, t.StockType, t.Qty
            from    #tmp t
            where   not exists( select 1 
                                from SemiFinishedInventory sfi 
                                where sfi.POID         = t.POID        and
                                      sfi.Seq          = t.Seq         and
                                      sfi.Roll         = t.Roll        and
                                      sfi.Dyelot       = t.Dyelot      and
                                      sfi.Tone         = t.Tone        and
                                      sfi.StockType    = t.StockType)

--SemiFinishedInventory_Location
insert into SemiFinishedInventory_Location(POID, Seq, Roll, Dyelot, Tone, StockType, MtlLocationID)
            select  t.POID, t.Seq, t.Roll, t.Dyelot, t.Tone, t.StockType, isnull(location.data, '')
            from    #tmp t
            outer apply(select data from dbo.SplitString(t.Location,',')) location
            where   location.data <> '' and
                    not exists( select 1 
                                from SemiFinishedInventory_Location sfil 
                                where sfil.POID         = t.POID        and
                                      sfil.Seq          = t.Seq         and
                                      sfil.Roll         = t.Roll        and
                                      sfil.Dyelot       = t.Dyelot      and
                                      sfil.StockType    = t.StockType   and
                                      sfil.Tone         = t.Tone        and
                                      sfil.MtlLocationID    = isnull(location.data, ''))

update  SemiFinishedReceiving 
set Status = 'Confirmed'
    , editdate = getdate()
    , editname = '{Env.User.UserID}' 
where ID = '{this.CurrentMaintain["ID"]}'
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
ALTER TABLE #tmp ALTER COLUMN Seq varchar(21)
ALTER TABLE #tmp ALTER COLUMN Roll varchar(8)
ALTER TABLE #tmp ALTER COLUMN Dyelot varchar(8)
ALTER TABLE #tmp ALTER COLUMN StockType char(1)
ALTER TABLE #tmp ALTER COLUMN Tone varchar(8)

select  sfi.POID, sfi.Seq, sfi.Roll, sfi.Dyelot, sfi.Tone
into    #tmpCheckSemiInventory
from    SemiFinishedInventory sfi
inner join #tmp t on sfi.POID         = t.POID        and
                     sfi.Seq          = t.Seq         and
                     sfi.Roll         = t.Roll        and
                     sfi.Dyelot       = t.Dyelot      and
                     sfi.Tone         = t.Tone        and
                     sfi.StockType    = t.StockType
where   (sfi.InQty - sfi.OutQty + sfi.AdjustQty) < t.Qty

select * from #tmpCheckSemiInventory

--如果沒有超過庫存就做庫存還原
if not exists (select 1 from #tmpCheckSemiInventory)
begin
    update sfi  set sfi.InQty = sfi.InQty - t.Qty
    from    SemiFinishedInventory sfi
    inner join #tmp t on sfi.POID         = t.POID        and
                         sfi.Seq          = t.Seq         and
                         sfi.Roll         = t.Roll        and
                         sfi.Dyelot       = t.Dyelot      and
                         sfi.Tone         = t.Tone        and
                         sfi.StockType    = t.StockType

    update  SemiFinishedReceiving 
    set Status = 'New'
        , editdate = getdate()
        , editname = '{Env.User.UserID}' 
    where ID = '{this.CurrentMaintain["ID"]}'
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
                                        .Where(s => MyUtility.Check.Empty(s["POID"]) || MyUtility.Check.Empty(s["Seq"]) || MyUtility.Check.Empty(s["Qty"]))
                                        .Any();

            if (isDetailKeyColEmpty)
            {
                MyUtility.Msg.WarningBox("<SP#>, <Seq>, <Qty> cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "SR", "SemiFinishedReceiving", (DateTime)this.CurrentMaintain["AddDate"]);
            }

            return base.ClickSaveBefore();
        }

        private void BtnAccumulatedQty_Click(object sender, EventArgs e)
        {
            if (this.DetailDatas.Count == 0)
            {
                return;
            }

            var frm = new P64_AccumulatedQty((DataTable)this.detailgridbs.DataSource);
            frm.ShowDialog();
        }

        private void BtnDownloadSampleFile_Click(object sender, EventArgs e)
        {
            // 呼叫執行檔絕對路徑
            DirectoryInfo dir = new DirectoryInfo(Application.StartupPath);
            string strXltName = Env.Cfg.XltPathDir + "\\Warehouse_P64_DownloadSampleFile.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            excel.Visible = true;
        }

        private void BtnImportFromExcel_Click(object sender, EventArgs e)
        {
            P64_ExcelImport callNextForm = new P64_ExcelImport((DataTable)this.detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            if (this.CurrentMaintain == null || !this.DetailDatas.Any())
            {
                return false;
            }

            if (this.CurrentMaintain["status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("Data is not confirmed, can't print.", "Warning");
                return false;
            }

            ReportDefinition rd = new ReportDefinition();
            if (!(this.result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(this.GetType()), this.GetType(), "P64_Print.rdlc", out IReportResource reportresource)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
            }
            else
            {
                #region -- 整理表頭資料 --
                // 抓M的EN NAME
                string rptTitle = MyUtility.GetValue.Lookup($@"select NameEN from MDivision where ID='{Env.User.Keyword}'");
                DataRow row = this.CurrentMaintain;
                rd.ReportParameters.Add(new ReportParameter("RptTitle", rptTitle));
                rd.ReportParameters.Add(new ReportParameter("ID", row["ID"].ToString()));
                rd.ReportParameters.Add(new ReportParameter("Remark", row["Remark"].ToString()));
                rd.ReportParameters.Add(new ReportParameter("IssueDate", ((DateTime)MyUtility.Convert.GetDate(row["IssueDate"])).ToString("yyyy/MM/dd")));
                #endregion

                string sqlcmd = $@"
select  sfd.*,
	    [Desc] = IIF((sfd.ID =   lag(sfd.ID,1,'') over (order by sfd.ID,sfd.seq) 
			                   AND (sfd.seq = lag(sfd.seq,1,'')over (order by sfd.ID,sfd.seq))) 
			                  , ''
                              , concat(sf.[Desc],char(10),'Color : ', sf.Color)),
        sf.Unit,
        sf.Color
from    SemiFinishedReceiving_Detail sfd
left join   SemiFinished sf with (nolock) on sf.PoID = sfd.Poid
and sf.Seq = sfd.Seq
where   sfd.ID = '{row["ID"]}'
";
                DualResult result = DBProxy.Current.Select(null, sqlcmd, out DataTable dataTable);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                List<P64_PrintData> data = dataTable.AsEnumerable()
                    .Select(row1 => new P64_PrintData()
                    {
                        POID = row1["POID"].ToString().Trim(),
                        SEQ = row1["Seq"].ToString().Trim(),
                        Roll = row1["Roll"].ToString().Trim(),
                        DYELOT = row1["Dyelot"].ToString().Trim(),
                        DESC = row1["Desc"].ToString().Trim(),
                        Unit = row1["Unit"].ToString().Trim(),
                        QTY = row1["QTY"].ToString().Trim(),
                        ToneGrp = row1["Tone"].ToString().Trim(),
                        Location = row1["Location"].ToString().Trim(),
                    }).ToList();

                rd.ReportDataSource = data;
                rd.ReportResource = reportresource;
                var frm1 = new Win.Subs.ReportView(rd)
                {
                    MdiParent = this.MdiParent,
                    TopMost = true,
                };
                frm1.Show();
            }

            return base.ClickPrint();
        }
    }
}
