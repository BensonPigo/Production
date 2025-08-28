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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <summary>
    /// P67
    /// </summary>
    public partial class P67 : Win.Tems.Input6
    {
        /// <summary>
        /// P67
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P67(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.gridicon.Append.Visible = false;
            this.gridicon.Insert.Visible = false;
            this.DefaultFilter = $" MDivisionID = '{Env.User.Keyword}'";
        }

        /// <inheritdoc/>
        public P67(ToolStripMenuItem menuitem, string transID)
        {
            this.InitializeComponent();
            this.DefaultFilter = $" id='{transID}' AND MDivisionID  = '{Sci.Env.User.Keyword}'";
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
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
from    SemiFinishedLocationTrans_Detail sfd
left join   SemiFinished sf with (nolock) on sf.Poid = sfd.Poid and sf.Seq = sfd.Seq
where   sfd.ID = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region -- To Location 右鍵開窗 --
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
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);
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
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .EditText("Desc", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
            .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Tone", header: "Tone/Grp", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("Qty", header: "Qty", decimal_places: 2, width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("Unit", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("FromLocation", header: "From Location", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ToLocation", header: "To Location", width: Widths.AnsiChars(15), settings: ts2)
            ;

            this.detailgrid.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
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

            string sqlUpdate = $@"
delete SemiFinishedInventory_Location
from SemiFinishedInventory_Location sl
inner join SemiFinishedLocationTrans_Detail sfd on sfd.POID         = sl.POID        and
												   sfd.Seq          = sl.Seq         and
												   sfd.Roll         = sl.Roll        and
												   sfd.Dyelot       = sl.Dyelot      and
												   sfd.Tone         = sl.Tone        and
												   sfd.StockType    = sl.StockType

INSERT INTO [dbo].[SemiFinishedInventory_Location]
           ([POID]
           ,[Roll]
           ,[Dyelot]
           ,[StockType]
           ,[MtlLocationID]
           ,[Seq]
           ,[Tone])
select
	sfd.POID
	,sfd.Roll
	,sfd.Dyelot
	,sfd.StockType
	,s.data
	,sfd.Seq
	,sfd.Tone
from SemiFinishedLocationTrans_Detail sfd
outer apply(select * from SplitString(sfd.ToLocation,','))s

update  SemiFinishedLocationTrans 
set Status = 'Confirmed'
    , editdate = getdate()
    , editname = '{Env.User.UserID}' 
where ID = '{this.CurrentMaintain["ID"]}'
";
            using (TransactionScope scope = new TransactionScope())
            {
                DualResult result = DBProxy.Current.Execute(null, sqlUpdate);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                scope.Complete();
            }

            MyUtility.Msg.InfoBox("Confirmed successful");
            base.ClickConfirm();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("No details");
                return false;
            }

            if (this.DetailDatas.Where(s => MyUtility.Check.Empty(s["ToLocation"])).Any())
            {
                MyUtility.Msg.WarningBox("To Location cannot be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                this.CurrentMaintain["ID"] = MyUtility.GetValue.GetID(Env.User.Keyword + "SU", "SemiFinishedLocationTrans", (DateTime)this.CurrentMaintain["AddDate"]);
            }

            return base.ClickSaveBefore();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.detailgrid.EndEdit();
            new P67_Import((DataTable)this.detailgridbs.DataSource).ShowDialog();
        }
    }
}
