using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P14 : Win.Tems.Input6
    {
        private string gridExportInvKey;
        private bool canEdit = false;

        /// <summary>
        /// P14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.detailgrid.Sorted += this.Detailgrid_Sorted;

            // this.detailgrid.SelectionChanged += this.Detailgrid_SelectionChanged;
            this.detailgrid.RowSelecting += this.Detailgrid_RowSelecting;
            this.canEdit = Prgs.GetAuthority(Env.User.UserID, "P14. Material C/O Maintenance", "CanEdit");
            this.btnBatchUpdate.Enabled = this.canEdit;
        }

        private void Detailgrid_RowSelecting(object sender, Ict.Win.UI.DataGridViewRowSelectingEventArgs e)
        {
            if (this.CurrentDetailData == null)
            {
                return;
            }

            string invNo = this.detailgrid.Rows[e.RowIndex].Cells["InvoiceNo"].Value.ToString();

            if (this.gridExportInvKey == invNo)
            {
                return;
            }

            this.gridExportInvKey = invNo;
            string sqlGetDate = $@"
select ed.ID, e.FactoryID, e.Consignee, e.ShipModeID, e.LoadDate, e.Etd, e.Eta, e.InvNo, e.Vessel, e.Blno,[ShipQty] = Sum(ed.Qty)
from Export_Detail ed
inner join Export e on e.ID = ed.ID
where ed.FormXPayINV = '{this.gridExportInvKey}'
group by ed.ID,e.FactoryID,e.Consignee,e.ShipModeID,e.LoadDate,e.Etd,e.Eta,e.Vessel,e.Blno,e.InvNo
order by ed.ID
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetDate, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridExport.DataSource = dtResult;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = $@"
select
md.Ukey            ,
md.ID              ,
md.SuppID          ,
md.InvoiceNo       ,
md.FormType        ,
md.FormNo          ,
md.TPEReceiveDate  ,
md.TPERemark       ,
md.Junk            ,
md.FtyReceiveDate  ,
md.FtyReceiveName  ,
md.FtyRemark       ,
md.TPEAddName      ,
md.TPEAddDate      ,
md.TPEEditName     ,
md.TPEEditDate     ,
md.FtyEditName     ,
md.FtyEditDate,
[FormName] = ft.Name,
[FtyReceiveNameDesc] = md.FtyReceiveName + '-' + ftyReceive.Name,
[TPECreateBy] = dbo.getTPEPass1_ExtNo(md.TPEAddName) + ' ' + Format(md.TPEAddDate,'yyyy/MM/dd'),
[TPEEditBy] = dbo.getTPEPass1_ExtNo(md.TPEEditName) + ' ' + Format(md.TPEEditDate,'yyyy/MM/dd')
from MtlCertificate_Detail md with (nolock)
left join FormType ft with (nolock) on ft.ID = md.FormType
left join pass1 ftyReceive with (nolock) on ftyReceive.ID = md.FtyReceiveName
where md.ID = '{masterID}'

";
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtHandle.TextBox1.ReadOnly = true;
        }

        private void Detailgrid_SelectionChanged(object sender, EventArgs e)
        {
            if (this.CurrentDetailData == null)
            {
                return;
            }

            if (this.gridExportInvKey == this.CurrentDetailData["InvoiceNo"].ToString())
            {
                return;
            }

            this.gridExportInvKey = this.CurrentDetailData["InvoiceNo"].ToString();
            string sqlGetDate = $@"
select ed.ID, e.FactoryID, e.Consignee, e.ShipModeID, e.LoadDate, e.Etd, e.Eta, e.InvNo, e.Vessel, e.Blno,[ShipQty] = Sum(ed.Qty)
from Export_Detail ed
inner join Export e on e.ID = ed.ID
where ed.FormXPayINV = '{this.gridExportInvKey}'
group by ed.ID,e.FactoryID,e.Consignee,e.ShipModeID,e.LoadDate,e.Etd,e.Eta,e.Vessel,e.Blno,e.InvNo
order by ed.ID
";
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetDate, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridExport.DataSource = dtResult;
        }

        private void Detailgrid_Sorted(object sender, EventArgs e)
        {
            this.ChangeDetailgridFormat();
        }

        protected override DualResult ClickSavePre()
        {
            // 若FtyReceiveDate有更新則EditName,EditDate也一併更新
            foreach (DataRow detailRow in this.DetailDatas)
            {
                if (detailRow.RowState == DataRowState.Modified &&
                   MyUtility.Check.Empty(detailRow["FtyEditName"]) &&
                   !MyUtility.Check.Empty(detailRow["FtyReceiveDate"]))
                {
                    detailRow["FtyEditName"] = Env.User.UserID;
                    detailRow["FtyEditDate"] = DateTime.Now;
                }
            }

            return base.ClickSavePre();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.ChangeDetailgridFormat();

            DataRow drExport;
            bool isExistsExport = MyUtility.Check.Seek($"select CloseDate,ETD,ETA from Export with (nolock) where ID = '{this.CurrentMaintain["ExportID"]}'", out drExport);
            if (isExistsExport)
            {
                this.dateETC.Value = (DateTime)drExport["CloseDate"];
                this.dateETA.Value = (DateTime)drExport["ETA"];
                this.dateETD.Value = (DateTime)drExport["ETD"];
            }
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            #region FtyReceiveDate
            DataGridViewGeneratorDateColumnSettings dsFtyReceiveDate = new DataGridViewGeneratorDateColumnSettings();
            dsFtyReceiveDate.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["FtyReceiveNameDesc"] = string.Empty;
                    this.CurrentDetailData["FtyReceiveName"] = string.Empty;
                    this.CurrentDetailData["FtyReceiveDate"] = DBNull.Value;
                    return;
                }

                DateTime ftyReceiveDate = (DateTime)e.FormattedValue;

                if (ftyReceiveDate < (DateTime)this.CurrentDetailData["TPEReceiveDate"])
                {
                   this.CurrentDetailData["FtyReceiveDate"] = DBNull.Value;
                   e.Cancel = true;
                   MyUtility.Msg.WarningBox("<Fty Receive Date> cannot be earlier than <TPE Receive Date>");
                   return;
                }

                this.CurrentDetailData["FtyReceiveDate"] = ftyReceiveDate;
                this.CurrentDetailData["FtyReceiveNameDesc"] = MyUtility.GetValue.Lookup($"select [FtyReceiveName] = ID + '-' + Name from Pass1 with (nolock) where ID = '{Env.User.UserID}'");
                this.CurrentDetailData["FtyReceiveName"] = Env.User.UserID;
            };
            #endregion

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("InvoiceNo", header: "Invoice#", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("FormName", header: "Form Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("FormNo", header: "Form#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Date("FtyReceiveDate", header: "Fty Receive Date", width: Widths.AnsiChars(15), iseditingreadonly: false, settings: dsFtyReceiveDate)
                .Text("FtyReceiveNameDesc", header: "Fty Receive Name", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .EditText("FtyRemark", header: "Fty Remark", width: Widths.AnsiChars(25), iseditingreadonly: false)
                .Date("TPEReceiveDate", header: "TPE Receive Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("TPERemark", header: "TPE Remark", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("TPECreateBy", header: "TPE Create By", width: Widths.AnsiChars(35), iseditingreadonly: true)
                .Text("TPEEditBy", header: "TPE Edit By", width: Widths.AnsiChars(35), iseditingreadonly: true);

            this.detailgrid.Columns["FtyReceiveDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["FtyRemark"].DefaultCellStyle.BackColor = Color.Pink;

            this.Helper.Controls.Grid.Generator(this.gridExport)
                .Text("ID", header: "WKID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Consignee", header: "Consignee", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("ShipModeID", header: "Shipmode", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Date("LoadDate", header: "Loading Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("Etd", header: "ETD", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("Eta", header: "ETA", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("InvNo", header: "Invoice", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("Vessel", header: "Vessel Name", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Text("Blno", header: "BL#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(13), decimal_places: 2, iseditingreadonly: true);
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查表身資料當MtlCertificate_Detail.FtyReceiveDate不為空值時，MtlCertificate_Detail.FtyReceiveName欄位也不得為空值
            var checkFtyReceiveValueList = this.DetailDatas.Where(s => this.CehckFtyReceiveData(s));
            if (checkFtyReceiveValueList.Any())
            {
                MyUtility.Msg.WarningBox("If <Fty Receive Date> has a value, <Fty Receive Name> must not be empty");
                return false;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        private bool CehckFtyReceiveData(DataRow tarRow)
        {
            return !MyUtility.Check.Empty(tarRow["FtyReceiveDate"]) && MyUtility.Check.Empty(tarRow["FtyReceiveName"]);
        }

        private void ChangeDetailgridFormat()
        {
            foreach (DataGridViewRow gridRow in this.detailgrid.Rows)
            {
                DataRow detailRow = this.detailgrid.GetDataRow(gridRow.Index);
                if (MyUtility.Check.Empty(detailRow["TPEReceiveDate"]) ||
                    !MyUtility.Check.Empty(detailRow["FtyEditName"]))
                {
                    gridRow.Cells["FtyReceiveDate"].ReadOnly = true;
                    gridRow.Cells["FtyReceiveDate"].Style.BackColor = Color.White;
                }
            }
        }

        private void BtnBatchUpdate_Click(object sender, EventArgs e)
        {
            new P14_BatchUpdate().ShowDialog();
            this.ReloadDatas();
        }
    }
}
