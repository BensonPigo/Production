using Ict;
using Ict.Win;
using Ict.Win.Defs;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.Prg.Entity;
using Sci.Win.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.PublicForm
{
    /// <summary>
    /// P06_Packing
    /// </summary>
    public partial class TK_PackingList : Sci.Win.Tems.QueryForm
    {
        private string transferExportID;
        private DataTable dtCarton = new DataTable();

        /// <summary>
        /// P06_Packing
        /// </summary>
        /// <param name="transferExportID">transferExportID</param>
        public TK_PackingList(string transferExportID)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.transferExportID = transferExportID;
            this.btnAssignCarton.Visible = this.EditMode;
            this.btnCancelAssingCarton.Visible = this.EditMode;

            this.dtCarton.Columns.Add("Carton", typeof(string));
            this.dtCarton.Columns.Add("NetKg", typeof(decimal));
            this.dtCarton.Columns.Add("WeightKg", typeof(decimal));
            this.dtCarton.Columns.Add("CBM", typeof(decimal));

            this.gridNotAssignCarton.DataSource = this.bindingSourceNotAssignCarton;
            this.gridAssignedCarton.DataSource = this.bindingSourceAssignedCarton;
            this.gridCartonList.DataSource = this.bindingSourceCartonList;
            this.bindingSourceAssignedCarton.Filter = "StockQty > 0";
            this.bindingSourceNotAssignCarton.Filter = "StockQty > 0";
            this.bindingSourceCartonList.DataSource = this.dtCarton;

            this.gridAssignedCarton.RowHeadersVisible = false;
            this.gridCartonList.RowHeadersVisible = false;
            this.gridNotAssignCarton.RowHeadersVisible = false;

            string ftyStatus = MyUtility.GetValue.Lookup($"select FtyStatus from TransferExport with (nolock) where ID = '{transferExportID}'");
            this.btnEditSave.Visible = ftyStatus == TK_FtyStatus.New;
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (this.btnEditSave == null)
            {
                return;
            }

            this.btnEditSave.Text = this.EditMode ? "Save" : "Edit";
            this.btnAssignCarton.Visible = this.EditMode;
            this.btnCancelAssingCarton.Visible = this.EditMode;

            if (!this.EditMode)
            {
                this.Query();
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataGridViewGeneratorNumericColumnSettings colAssignQty = new DataGridViewGeneratorNumericColumnSettings();
            colAssignQty.CellValidating += (s, e) =>
            {
                decimal assignQty = MyUtility.Convert.GetDecimal(e.FormattedValue);
                decimal stockQty = MyUtility.Convert.GetDecimal(this.gridNotAssignCarton.GetDataRow(e.RowIndex)["StockQty"]);

                if (assignQty > stockQty)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("[Assign Export Q'ty] cannot be bigger than [Export Q'ty].");
                    return;
                }

                if (assignQty == 0)
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(" [Assign Export Q'ty] cannot be 0.");
                    return;
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridNotAssignCarton)
                .CheckBox("select", trueValue: true, falseValue: false, iseditable: true)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("FromSEQ", header: "From" + Environment.NewLine + "SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("StockQty", header: "Export Qty", width: Widths.AnsiChars(7), decimal_places: 2, iseditingreadonly: true)
                .Text("Carton", header: "Assign" + Environment.NewLine + "Carton No", width: Widths.AnsiChars(6), iseditingreadonly: false)
                .Numeric("AssignQty", header: "Assign" + Environment.NewLine + "Export Q'ty", width: Widths.AnsiChars(9), decimal_places: 2, iseditingreadonly: false, settings: colAssignQty);

            this.Helper.Controls.Grid.Generator(this.gridAssignedCarton)
                .CheckBox("select", trueValue: true, falseValue: false, iseditable: true)
                .Text("InventoryPOID", header: "From SP#", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("POID", header: "To SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Roll", header: "Roll", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Carton", header: "Assign" + Environment.NewLine + "Carton No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("StockQty", header: "Export Qty", width: Widths.AnsiChars(7), decimal_places: 2, iseditingreadonly: true);

            this.Helper.Controls.Grid.Generator(this.gridCartonList)
                .Text("Carton", header: "Carton No", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Numeric("NetKg", header: "N.W. (kg)", width: Widths.AnsiChars(7), decimal_places: 2, iseditingreadonly: false)
                .Numeric("WeightKg", header: "G.W. (kg)", width: Widths.AnsiChars(7), decimal_places: 2, iseditingreadonly: false)
                .Numeric("CBM", header: "CBM", width: Widths.AnsiChars(7), decimal_places: 5, iseditingreadonly: false);

            // 設定grid cell color
            this.gridNotAssignCarton.ColumnFrozen(0);
            this.gridAssignedCarton.ColumnFrozen(0);
            this.gridNotAssignCarton.Columns["Carton"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridNotAssignCarton.Columns["AssignQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCartonList.Columns["NetKg"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCartonList.Columns["WeightKg"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCartonList.Columns["CBM"].DefaultCellStyle.BackColor = Color.Pink;

            this.Query();
        }

        private void BtnEditSave_Click(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                DualResult result = this.SaveTransferExport_Detail_Carton();
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
                MyUtility.Msg.InfoBox("Save success");
            }

            this.EditMode = !this.EditMode;
        }

        private DualResult SaveTransferExport_Detail_Carton()
        {
            bool isCartonInfoMissed = this.dtCarton.AsEnumerable()
                .Any(s =>
                MyUtility.Check.Empty(s["NetKg"]) ||
                MyUtility.Check.Empty(s["WeightKg"]) ||
                MyUtility.Check.Empty(s["CBM"]));
            if (isCartonInfoMissed)
            {
                return new DualResult(false, "[Carton List] N.W.(kg), G.W.(kg) and CBM cannot be 0.");
            }

            bool transferExportFtyStatusNotNew = MyUtility.Check.Seek($"select 1 from TransferExport with (nolock) where ID = '{this.transferExportID}' and FtyStatus <> '{TK_FtyStatus.New}'");

            if (transferExportFtyStatusNotNew)
            {
                this.EditMode = false;
                return new DualResult(false, "Transfer WK# Fty Status not new, warehouse team cannot modify carton list.");
            }

            string sqlGetOriCartonStockQty = $@"
select  TransferExport_DetailUkey,
        Roll,
        [Dyelot] = LotNo,
        [StockQty] = isnull(Sum(StockQty), 0)
from TransferExport_Detail_Carton with (nolock)
where   ID = '{this.transferExportID}'
group by TransferExport_DetailUkey, Roll, LotNo
";
            DataTable dtOriCartonStockQty;

            DualResult result = DBProxy.Current.Select(null, sqlGetOriCartonStockQty, out dtOriCartonStockQty);
            if (!result)
            {
                return result;
            }

            DataTable dtNotAssignCarton = (DataTable)this.bindingSourceNotAssignCarton.DataSource;
            DataTable dtAssignCarton = (DataTable)this.bindingSourceAssignedCarton.DataSource;

            foreach (DataRow dr in dtNotAssignCarton.Rows)
            {
                dr["Carton"] = string.Empty;
            }

            DataTable dtAllCarton = dtNotAssignCarton.Clone();
            dtAllCarton.Merge(dtNotAssignCarton);
            dtAllCarton.Merge(dtAssignCarton);

            bool isTransferOutDataChanged = dtOriCartonStockQty.AsEnumerable()
                .Any(s =>
                {
                    decimal stockQtyCurrent = dtAllCarton.AsEnumerable()
                        .Where(cartonItem => MyUtility.Convert.GetLong(cartonItem["TransferExport_DetailUkey"]) == MyUtility.Convert.GetLong(s["TransferExport_DetailUkey"]) &&
                                             cartonItem["Roll"].ToString() == s["Roll"].ToString() &&
                                             cartonItem["Dyelot"].ToString() == s["Dyelot"].ToString())
                        .Select(cartonItem => MyUtility.Convert.GetDecimal(cartonItem["StockQty"]))
                        .DefaultIfEmpty(0)
                        .Sum();

                    return MyUtility.Convert.GetDecimal(s["StockQty"]) != stockQtyCurrent;
                });

            if (isTransferOutDataChanged)
            {
                this.EditMode = false;
                return new DualResult(false, "Transfer out data already changed, system will back to view mode, please check again.");
            }

            foreach (DataRow dr in dtAllCarton.Rows)
            {
                dr["NetKg"] = 0;
                dr["WeightKg"] = 0;
                dr["CBM"] = 0;
            }

            foreach (DataRow drCartonItem in this.dtCarton.Rows)
            {
                DataRow drUpdateCartonInfo = dtAllCarton.Select($"Carton = '{drCartonItem["Carton"]}' and StockQty > 0")[0];
                drUpdateCartonInfo["NetKg"] = drCartonItem["NetKg"];
                drUpdateCartonInfo["WeightKg"] = drCartonItem["WeightKg"];
                drUpdateCartonInfo["CBM"] = drCartonItem["CBM"];
            }

            DataTable dtNeedUpdate = dtAllCarton.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["StockQty"])).CopyToDataTable();

            string sqlUpdateTransferExport_Detail_Carton = $@"
alter table #tmp alter column Roll varchar(30)
alter table #tmp alter column Carton varchar(50)
alter table #tmp alter column Dyelot varchar(30)

update  tdc set     tdc.StockQty = t.StockQty,
                    tdc.Qty = round(dbo.GetUnitQty(tdc.StockUnitID, td.UnitID, t.StockQty), 2),
                    tdc.EditName = '{Env.User.UserID}',
                    tdc.EditDate = getdate()
from    #tmp t
inner join TransferExport_Detail_Carton tdc with (nolock) on    t.TransferExport_DetailUkey = tdc.TransferExport_DetailUkey and
                                                                t.Roll = tdc.Roll and
                                                                t.Carton = tdc.Carton and
                                                                t.Dyelot = tdc.LotNo and
                                                                t.StockQty <> tdc.StockQty
inner join TransferExport_Detail td with (nolock) on tdc.TransferExport_DetailUkey = td.Ukey

insert into TransferExport_Detail_Carton(
TransferExport_DetailUkey
,ID
,PoID
,Seq1
,Seq2
,Carton
,LotNo
,Qty
,Foc
,StockUnitID
,StockQty
,Tone
,MINDQRCode
,Roll
,EditName
,EditDate
)
select  t.TransferExport_DetailUkey
        ,[ID] = '{this.transferExportID}'
        ,t.PoID
        ,t.Seq1
        ,t.Seq2
        ,t.Carton
        ,t.Dyelot
        ,[Qty] = round(dbo.GetUnitQty(t.StockUnitID, td.UnitID, t.StockQty), 2)
        ,t.Foc
        ,t.StockUnitID
        ,t.StockQty
        ,t.Tone
        ,t.MINDQRCode
        ,t.Roll
        ,'{Env.User.UserID}'
        ,getdate()
from    #tmp t
inner   join TransferExport_Detail td with (nolock) on t.TransferExport_DetailUkey = td.Ukey
where   not exists(select 1 from TransferExport_Detail_Carton tdc with (nolock) 
                            where   t.TransferExport_DetailUkey = tdc.TransferExport_DetailUkey and
                                    t.Roll = tdc.Roll and
                                    t.Carton = tdc.Carton and
                                    t.Dyelot = tdc.LotNo) and
        t.StockQty > 0

-- update 重量裁積
update  tdc set     tdc.NetKg = t.NetKg,
                    tdc.WeightKg = t.WeightKg,
                    tdc.CBM = t.CBM
from    #tmp t
inner join TransferExport_Detail_Carton tdc with (nolock) on    t.TransferExport_DetailUkey = tdc.TransferExport_DetailUkey and
                                                                t.Roll = tdc.Roll and
                                                                t.Carton = tdc.Carton and
                                                                t.Dyelot = tdc.LotNo
where   t.NetKg > 0 and t.WeightKg > 0 and t.CBM > 0

-- 抓出有維護Carton的資料，刪除時只針對有多筆carton的資料，避免刪掉原本Carton = '', Qty = 0的資料
select  TransferExport_DetailUkey,
        LotNo,
        Roll
into #mutiCarton
from TransferExport_Detail_Carton with (nolock)
where ID = '{this.transferExportID}'
group by TransferExport_DetailUkey, LotNo, Roll
having count(*) > 1

delete  tdc
from TransferExport_Detail_Carton tdc
where   not exists(select 1 from #tmp t with (nolock) 
                            where   t.TransferExport_DetailUkey = tdc.TransferExport_DetailUkey and
                                    t.Roll = tdc.Roll and
                                    t.Carton = tdc.Carton and
                                    t.Dyelot = tdc.LotNo and
                                    t.StockQty > 0
                    ) and
        exists(select 1 from #mutiCarton t  with (nolock) 
                        where   t.TransferExport_DetailUkey = tdc.TransferExport_DetailUkey and
                                t.Roll = tdc.Roll and
                                t.LotNo = tdc.LotNo) and
        tdc.ID = '{this.transferExportID}'



drop table #tmp, #mutiCarton
";

            result = MyUtility.Tool.ProcessWithDatatable(dtNeedUpdate, null, sqlUpdateTransferExport_Detail_Carton, out DataTable dtEmpty);

            if (!result)
            {
                return result;
            }

            return new DualResult(true);
        }

        private void Query()
        {
            this.dtCarton.Clear();
            string sqlQuery = $@"
select	[select] = cast(0 as bit),
		ted.InventoryPOID,
		[FromSeq] = Concat(ted.InventorySeq1, ' ', ted.InventorySeq2),
		ted.PoID,
		[ToSEQ] = Concat(ted.Seq1, ' ', ted.Seq2),
		ted.Seq1,
		ted.Seq2,
		tedc.Roll,
		[Dyelot] = tedc.LotNo,
		tedc.StockQty,
		tedc.Carton,
		[AssignQty] = tedc.StockQty,
		tedc.TransferExport_DetailUkey,
        ted.FabricType,
        tedc.Foc,
        tedc.Tone,
        tedc.MINDQRCode,
        tedc.GroupID,
        tedc.StockUnitID,
        tedc.NetKg,
        tedc.WeightKg,
        tedc.CBM
from TransferExport_Detail ted with (nolock)
inner join TransferExport_Detail_Carton tedc with (nolock) on tedc.TransferExport_DetailUkey = ted.Ukey
where ted.ID = '{this.transferExportID}'
";

            DataTable dtResult;

            DualResult result = DBProxy.Current.Select(null, sqlQuery, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No Data Found");
                return;
            }

            DataTable dtNotAssignCarton = dtResult.AsEnumerable().Where(s => MyUtility.Check.Empty(s["Carton"])).TryCopyToDataTable(dtResult);
            DataTable dtAssignCarton = dtResult.AsEnumerable().Where(s => !MyUtility.Check.Empty(s["Carton"])).TryCopyToDataTable(dtResult);

            if (dtAssignCarton.Rows.Count > 0)
            {
                var listCarton = dtAssignCarton.AsEnumerable()
                            .Where(s => !MyUtility.Check.Empty(s["NetKg"]) || !MyUtility.Check.Empty(s["WeightKg"]) || !MyUtility.Check.Empty(s["CBM"]))
                            .GroupBy(s => s["Carton"].ToString());

                foreach (var item in listCarton)
                {
                    DataRow newRow = this.dtCarton.NewRow();
                    newRow["Carton"] = item.Key;
                    newRow["NetKg"] = item.First()["NetKg"];
                    newRow["WeightKg"] = item.First()["WeightKg"];
                    newRow["CBM"] = item.First()["CBM"];

                    this.dtCarton.Rows.Add(newRow);
                }
            }

            this.bindingSourceNotAssignCarton.DataSource = dtNotAssignCarton;
            this.bindingSourceAssignedCarton.DataSource = dtAssignCarton;

            this.GridRowStatusChange();
        }

        private void GridRowStatusChange()
        {
            foreach (DataGridViewRow dr in this.gridNotAssignCarton.Rows)
            {
                if (MyUtility.Check.Empty(dr.Cells["StockQty"].Value))
                {
                    dr.ReadOnly = true;
                }

                if (this.gridNotAssignCarton.GetDataRow(dr.Index)["FabricType"].ToString() == "F")
                {
                    dr.Cells["AssignQty"].ReadOnly = true;
                }
            }
        }

        private void BtnCancelAssingCarton_Click(object sender, EventArgs e)
        {
            var selectRow = ((DataTable)this.bindingSourceAssignedCarton.DataSource).AsEnumerable().Where(s => MyUtility.Convert.GetBool(s["select"]));

            if (!selectRow.Any())
            {
                MyUtility.Msg.WarningBox("Please tick one data row on below grid first .");
                return;
            }

            DataTable dtNotAssignedCarton = (DataTable)this.bindingSourceNotAssignCarton.DataSource;

            foreach (DataRow dr in selectRow)
            {
                string filterExpression = $@"
InventoryPOID = '{dr["InventoryPOID"]}' and
FromSEQ = '{dr["FromSEQ"]}' and
POID = '{dr["POID"]}' and
ToSEQ = '{dr["ToSEQ"]}' and
Roll = '{dr["Roll"]}' and
Dyelot = '{dr["Dyelot"]}'";
                DataRow[] drResult = dtNotAssignedCarton.Select(filterExpression);

                if (drResult.Length == 0)
                {
                    DataRow newRow = dtNotAssignedCarton.NewRow();

                    newRow["select"] = false;
                    newRow["InventoryPOID"] = dr["InventoryPOID"];
                    newRow["FromSeq"] = dr["FromSeq"];
                    newRow["PoID"] = dr["PoID"];
                    newRow["ToSEQ"] = dr["ToSEQ"];
                    newRow["Seq1"] = dr["Seq1"];
                    newRow["Seq2"] = dr["Seq2"];
                    newRow["Roll"] = dr["Roll"];
                    newRow["Dyelot"] = dr["Dyelot"];
                    newRow["StockQty"] = dr["StockQty"];
                    newRow["Carton"] = string.Empty;
                    newRow["AssignQty"] = 0;
                    newRow["TransferExport_DetailUkey"] = dr["TransferExport_DetailUkey"];
                    newRow["FabricType"] = dr["FabricType"];
                    newRow["Foc"] = dr["Foc"];
                    newRow["Tone"] = dr["Tone"];
                    newRow["MINDQRCode"] = dr["MINDQRCode"];
                    newRow["GroupID"] = dr["GroupID"];
                    newRow["StockUnitID"] = dr["StockUnitID"];

                    dtNotAssignedCarton.Rows.Add(newRow);
                }
                else
                {
                    drResult[0]["StockQty"] = (decimal)dr["StockQty"] + (decimal)drResult[0]["StockQty"];
                }

                dr["StockQty"] = 0;
                dr["select"] = false;
                dr.EndEdit();
            }

            this.CartonListRefresh();
        }

        private void BtnAssignCarton_Click(object sender, EventArgs e)
        {
            var selectRow = ((DataTable)this.bindingSourceNotAssignCarton.DataSource).AsEnumerable().Where(s => MyUtility.Convert.GetBool(s["select"]));

            if (!selectRow.Any())
            {
                MyUtility.Msg.WarningBox("Please tick one data row on top grid first .");
                return;
            }

            if (selectRow.Any(s => MyUtility.Check.Empty(s["Carton"])))
            {
                MyUtility.Msg.WarningBox("[Assign Carton No] cannot be empty.");
                return;
            }

            if (selectRow.Any(s => MyUtility.Check.Empty(s["AssignQty"])))
            {
                MyUtility.Msg.WarningBox("[Assign Export Qty] cannot be 0.");
                return;
            }

            DataTable dtAssignedCarton = (DataTable)this.bindingSourceAssignedCarton.DataSource;

            foreach (DataRow dr in selectRow)
            {
                string filterExpression = $@"
InventoryPOID = '{dr["InventoryPOID"]}' and
FromSEQ = '{dr["FromSEQ"]}' and
POID = '{dr["POID"]}' and
ToSEQ = '{dr["ToSEQ"]}' and
Roll = '{dr["Roll"]}' and
Dyelot = '{dr["Dyelot"]}' and
Carton = '{dr["Carton"]}'";
                DataRow[] drResult = dtAssignedCarton.Select(filterExpression);

                if (drResult.Length == 0)
                {
                    DataRow newRow = dtAssignedCarton.NewRow();

                    newRow["select"] = false;
                    newRow["InventoryPOID"] = dr["InventoryPOID"];
                    newRow["FromSeq"] = dr["FromSeq"];
                    newRow["PoID"] = dr["PoID"];
                    newRow["ToSEQ"] = dr["ToSEQ"];
                    newRow["Seq1"] = dr["Seq1"];
                    newRow["Seq2"] = dr["Seq2"];
                    newRow["Roll"] = dr["Roll"];
                    newRow["Dyelot"] = dr["Dyelot"];
                    newRow["StockQty"] = dr["AssignQty"];
                    newRow["Carton"] = dr["Carton"];
                    newRow["AssignQty"] = 0;
                    newRow["TransferExport_DetailUkey"] = dr["TransferExport_DetailUkey"];
                    newRow["FabricType"] = dr["FabricType"];
                    newRow["Foc"] = dr["Foc"];
                    newRow["Tone"] = dr["Tone"];
                    newRow["MINDQRCode"] = dr["MINDQRCode"];
                    newRow["GroupID"] = dr["GroupID"];
                    newRow["StockUnitID"] = dr["StockUnitID"];

                    dtAssignedCarton.Rows.Add(newRow);
                }
                else
                {
                    drResult[0]["StockQty"] = (decimal)dr["AssignQty"] + (decimal)drResult[0]["StockQty"];
                }

                dr["StockQty"] = (decimal)dr["StockQty"] - (decimal)dr["AssignQty"];
                dr["select"] = false;
                dr.EndEdit();
            }

            this.CartonListRefresh();
        }

        private void CartonListRefresh()
        {
            var allCarton = ((DataTable)this.bindingSourceAssignedCarton.DataSource).AsEnumerable()
                .Where(s => MyUtility.Convert.GetDecimal(s["StockQty"]) > 0)
                .Select(s => s["Carton"].ToString()).Distinct();

            var listCarton = this.dtCarton.AsEnumerable();

            foreach (string newCarton in allCarton)
            {
                if (!listCarton.Any(s => s["Carton"].ToString() == newCarton))
                {
                    DataRow newRow = this.dtCarton.NewRow();
                    newRow["Carton"] = newCarton;
                    this.dtCarton.Rows.Add(newRow);
                }
            }

            for (int i = this.dtCarton.Rows.Count - 1; i >= 0; i--)
            {
                if (!allCarton.Any(s => s == this.dtCarton.Rows[i]["Carton"].ToString()))
                {
                    this.dtCarton.Rows.RemoveAt(i);
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
