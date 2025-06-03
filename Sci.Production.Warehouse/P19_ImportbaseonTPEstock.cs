using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P19_ImportbaseonTPEstock : Win.Subs.Base
    {
        private DataRow master;
        private DataTable detail;
        private DataTable masterdt;
        private DataTable detaildt;
        private string M;

        /// <inheritdoc/>
        public P19_ImportbaseonTPEstock(DataRow master, DataTable detail, string mdivisionid)
        {
            this.InitializeComponent();
            this.master = master;
            this.detail = detail;
            this.M = mdivisionid;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.GridSetup();
        }

        private void GridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings selected = new DataGridViewGeneratorCheckBoxColumnSettings();
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("selected", header: "Auto Pick", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: selected)
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4))
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("ToFactory", header: "ToFactory", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("InventoryPOID", header: "Inventory POID", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Inventoryseq1", header: "Inventory Seq1", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("InventorySEQ2", header: "Inventory Seq2", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("FabricType", header: "Material Type", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Date("TaipeiLastOutput", header: "Taipei" + Environment.NewLine + "Last Output", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("TaipeiOutput", header: "Taipei" + Environment.NewLine + "Output", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("AccuTransferred", header: "Accu Transferred", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("TotalTransfer", header: "Total Transfer", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("Balance", header: "Balance", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
               ;
            selected.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!dr["FabricType"].EqualString("Accessory"))
                {
                    return;
                }

                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
            };

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid2.GetDataRow(e.RowIndex);

                // 數量0要拉開判斷且不能自動勾選Selected
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    dr["qty"] = 0;
                    dr.EndEdit();
                }
                else if (MyUtility.Convert.GetDecimal(e.FormattedValue) > MyUtility.Convert.GetDecimal(dr["StockBalance"]))
                {
                    MyUtility.Msg.WarningBox("TransferQty can not more than Stock Balance!");
                    dr["qty"] = 0;
                    dr.EndEdit();
                }
                else
                {
                    // 先填入數值 > EndEdit > 才能自動勾選! 不然會觸發到Select事件
                    dr["qty"] = e.FormattedValue;
                    dr.EndEdit();
                    dr["Selected"] = 1;
                }

                this.CaculateTotalTransfer(selectAll: false);
            };

            DataGridViewGeneratorCheckBoxColumnSettings selectedSetting = new DataGridViewGeneratorCheckBoxColumnSettings();
            selectedSetting.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid2.GetDataRow<DataRow>(e.RowIndex);
                dr["selected"] = e.FormattedValue;

                // 手動勾選要單獨判斷是把StockBalance給Qty
                if (MyUtility.Convert.GetBool(dr["Selected"]))
                {
                    if (dr["qty"].Empty())
                    {
                        dr["qty"] = dr["StockBalance"];
                    }
                }

                dr.EndEdit();
                this.CaculateTotalTransfer(selectAll: false);
            };

            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: selectedSetting)
                .Text("ExportID", header: "WK#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("PoId", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("roll", header: "Roll#", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(15))
                .Text("Tone", header: "Tone/Grp", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Text("Refno", header: "Ref#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SizeSpec", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("StockUnit", header: "Unit", iseditingreadonly: true)
                .Text("stocktype", header: "Stock Type", iseditingreadonly: true)
                .Numeric("StockBalance", header: "Stock Balance", iseditingreadonly: true, decimal_places: 2, integer_places: 10)
                .Numeric("qty", header: "TransferQty", decimal_places: 2, integer_places: 10, settings: ns)
                .Text("location", header: "Location", iseditingreadonly: true)
                .Text("ToPOID", header: "To POID", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("ToSeq", header: "To Seq", iseditingreadonly: true, width: Widths.AnsiChars(6))
               ;
            this.grid2.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            this.grid2.ColumnHeaderMouseClick += this.Grid2_ColumnHeaderMouseClick;
        }

        private void Grid2_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.CaculateTotalTransfer(selectAll: true);
            }
        }

        private void CaculateTotalTransfer(bool selectAll)
        {
            decimal totalTransfer = 0;
            foreach (DataGridViewRow gridDr in this.grid2.Rows)
            {
                DataRow dr = this.grid2.GetDataRow(gridDr.Index);
                if (MyUtility.Convert.GetBool(dr["Selected"]))
                {
                    // 此功能for全選要自動給值
                    if (selectAll)
                    {
                        if (dr["qty"].Empty())
                        {
                            dr["qty"] = dr["StockBalance"];
                            dr.EndEdit();
                        }
                    }

                    totalTransfer += MyUtility.Convert.GetDecimal(gridDr.Cells["qty"].Value);
                }
            }

            DataRow drSelect = this.grid1.GetDataRow(this.listControlBindingSource1.Position);

            if (this.grid1.SelectedRows != null && this.grid1.SelectedRows.Count > 0)
            {
                this.grid1.SelectedRows[0].Cells["TotalTransfer"].Value = totalTransfer;
                this.grid1.SelectedRows[0].Cells["Balance"].Value = MyUtility.Convert.GetDecimal(drSelect["TaipeiOutput"]) - MyUtility.Convert.GetDecimal(drSelect["AccuTransferred"]) - totalTransfer;
                this.grid1.RefreshEdit();
            }
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;
            if (MyUtility.Check.Empty(this.txtsp.Text))
            {
                return;
            }

            List<SqlParameter> listPar = new List<SqlParameter>()
            {
                new SqlParameter("@IssueSP", SqlDbType.VarChar, 13) { Value = this.txtsp.Text },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 5) { Value = Sci.Env.User.Keyword },
            };

            string sqlcmd = $@"
select  POID = rtrim(i.seq70poid)
		, Seq1 = rtrim(i.seq70seq1)
		, Seq2 = i.seq70seq2
		, ToFactory = MAX(iif (i.type = 2, i.TransferFactory, ''))
		, InventoryPOID = i.InventoryPOID
        , Inventoryseq1 = i.InventorySeq1
        , InventorySEQ2 = i.InventorySeq2
		, psd.StockUnit
        , TaipeiLastOutput = max(i.confirmdate) 
        , TaipeiOutput = sum(iif(i.type='2',i.qty,0-i.qty)) 
		, psd.FabricType
		, PSD.Refno
		, Color
		, [AccuTransferred] = isnull(TransferOut.Qty,0)
		, SizeSpec= isnull(psdsS.SpecValue, '')
        , psd.SCIRefno
into #tmp
from dbo.Invtrans i WITH (NOLOCK) 
inner join PO_Supp_Detail psd WITH (NOLOCK) on i.InventoryPOID = psd.ID
												and i.InventorySeq1 = psd.SEQ1
												and i.InventorySeq2 = psd.SEQ2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
inner join View_WH_Orders o WITH (NOLOCK) on psd.ID = o.ID
left join Fabric on Fabric.SCIRefno = psd.SCIRefno
outer apply (
        select Color = IIF(Fabric.MtlTypeID = 'EMB THREAD' OR Fabric.MtlTypeID = 'SP THREAD' OR Fabric.MtlTypeID = 'THREAD' 
	        ,IIF( PSD.SuppColor = '' or PSD.SuppColor is null,dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, '')),PSD.SuppColor)
	        ,dbo.GetColorMultipleID(o.BrandID, isnull(psdsC.SpecValue, ''))
))c
outer apply(
	select Qty = sum(td.Qty)
	from TransferOut_Detail td 
	inner join TransferOut t on t.Id = td.ID
	where t.Status !='New'
	and td.POID = i.InventoryPOID
	and td.Seq1 = i.InventorySeq1 
	and td.Seq2 = i.InventorySeq2
	and td.ToPOID = i.seq70poid
	and td.ToSeq1 = i.seq70seq1
	and td.ToSeq2 = i.seq70seq2
) TransferOut
where (i.type='2' or i.type='6')
		and i.seq70poid = @IssueSP
		and o.MDivisionID = @MDivisionID
AND NOT EXISTS (--由工廠採購的物料是禁止轉出給其他廠(不管境內或跨國
    SELECT 1
    FROM Order_FtyMtlStdCost oc WITH (NOLOCK) 
    WHERE oc.OrderID = psd.ID
    AND oc.SCIRefno = psd.SCIRefno
)
group by i.seq70poid,rtrim(i.seq70poid), rtrim(i.seq70seq1), i.seq70seq2, i.InventoryPOID, i.InventorySeq1, i.InventorySeq2, psd.StockUnit, psd.FabricType,PSD.Refno, psdsS.SpecValue,c.Color,TransferOut.Qty, psd.SCIRefno

select  selected = 0
		, [ExportID] = Stuff((select distinct concat(',', r.ExportId)
				from Receiving r WITH (NOLOCK)
                inner join Receiving_Detail rd WITH (NOLOCK) on r.id = rd.id
                where rd.PoId = fi.POID and rd.Seq1 = fi.SEQ1 and rd.Seq2 = fi.SEQ2 and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
				FOR XML PATH('')),1,1,'') 
		, MDivisionID = '{this.M}'
        , '' id
        , ftyinventoryukey = FI.ukey
        , fi.POID 
        , fi.seq1 
        , fi.seq2 
        , Seq = concat(Ltrim(Rtrim(fi.seq1)), ' ', fi.seq2)
        , fi.roll 
        , fi.dyelot 
        , fi.stocktype
		, #tmp.StockUnit
        , StockBalance = isnull(fi.InQty, 0) - isnull(fi.OutQty, 0) + isnull(fi.AdjustQty, 0) - isnull(fi.ReturnQty, 0) 
        , Description = dbo.getmtldesc(FI.POID,FI.seq1,FI.seq2,2,0)
        , Qty = 0.00 -- TransferQty
        , [Location] = dbo.Getlocation(fi.ukey)
        , ToPOID = rtrim(#tmp.poid) 
        , ToSeq1 = rtrim(#tmp.seq1) 
        , ToSeq2 = #tmp.seq2 
        , ToSeq = concat(Ltrim(Rtrim(#tmp.seq1)), ' ', #tmp.seq2)
        , GroupQty = Sum(isnull(fi.InQty, 0) - isnull(fi.OutQty, 0) + isnull(fi.AdjustQty, 0) - isnull(fi.ReturnQty, 0)) over (partition by #tmp.poid, #tmp.seq1, #tmp.seq2, fi.dyelot)
        , #tmp.ToFactory
        , #tmp.InventoryPOID
        , #tmp.Inventoryseq1
        , #tmp.InventorySEQ2
        , #tmp.TaipeiLastOutput
        , #tmp.TaipeiOutput
		, #tmp.FabricType
        , Refno
        , Color
        , SizeSpec
        , fi.Tone
		, #tmp.[AccuTransferred]
        , fi.Lock
        , #tmp.SCIRefno
into    #tmpDetailResult
from    #tmp  
left join dbo.FtyInventory fi WITH (NOLOCK) on fi.POID = InventoryPOID 
                                                and fi.seq1 = Inventoryseq1 
                                                and fi.seq2 = InventorySEQ2 
                                                and fi.StockType = 'I'
Order by GroupQty desc, Dyelot, StockBalance desc

select  selected = cast(0 as bit)
		, ExportID
		, MDivisionID 
        , id
        , ftyinventoryukey
        , POID 
        , seq1 
        , seq2 
        , Seq
        , roll 
        , dyelot 
        , stocktype
		, StockUnit
        , StockBalance 
        , Description 
        , Qty = 0.00
        , [Location]
        , ToPOID 
        , ToSeq1 
        , ToSeq2
        , ToSeq
        , GroupQty
        , ToFactory
        , InventoryPOID
        , Inventoryseq1
        , InventorySEQ2
        , TaipeiLastOutput
        , TaipeiOutput
        , FabricType =  CASE 
                            WHEN FabricType = 'F' THEN 'Fabric' 
							WHEN FabricType = 'A' THEN 'Accessory' 
							WHEN FabricType = 'O' THEN 'Other' 
							ELSE '' 
						END
        , Refno
        , Color
        , SizeSpec
        , Tone
		, [AccuTransferred]
        , Lock
        , SCIRefno
into    #tmpDetail
from    #tmpDetailResult

select  selected
        , [POID] = ToPOID
		, [Seq1] = ToSeq1
		, [Seq2] = ToSeq2
		, ToFactory
		, InventoryPOID
        , Inventoryseq1
        , InventorySEQ2
		, FabricType
		, StockUnit
        , TaipeiLastOutput
        , TaipeiOutput
		, [AccuTransferred]
        , [TotalTransfer] = 0.00		
		, [Balance] = TaipeiOutput-AccuTransferred
        , SCIRefno
from    #tmpDetail
group by selected, ToPOID, ToSeq1, ToSeq2, ToFactory, InventoryPOID, Inventoryseq1, InventorySEQ2
        , FabricType, StockUnit, TaipeiLastOutput, TaipeiOutput, AccuTransferred, SCIRefno
order by ToPOID, ToSeq1, ToSeq2;

select  *
    , TransferExport_DetailUkey = cast(0 as bigint)
from    #tmpDetail
where Lock = 0
and StockBalance > 0


drop table #tmp, #tmpDetailResult, #tmpDetail
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, listPar, out DataTable[] dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dt[0]);
            dataSet.Tables.Add(dt[1]);

            this.masterdt = dataSet.Tables[0];
            this.masterdt.TableName = "masterdt";

            this.detaildt = dataSet.Tables[1];
            this.detaildt.TableName = "detaildt";

            DataRelation relation = new DataRelation(
                "rel1",
                new DataColumn[] { this.masterdt.Columns["POID"], this.masterdt.Columns["Seq1"], this.masterdt.Columns["Seq2"], this.masterdt.Columns["InventoryPOID"], this.masterdt.Columns["Inventoryseq1"], this.masterdt.Columns["InventorySEQ2"] },
                new DataColumn[] { this.detaildt.Columns["ToPOID"], this.detaildt.Columns["Toseq1"], this.detaildt.Columns["Toseq2"], this.detaildt.Columns["POID"], this.detaildt.Columns["seq1"], this.detaildt.Columns["seq2"] });
            dataSet.Relations.Add(relation);
            this.listControlBindingSource1.DataSource = dataSet;
            this.listControlBindingSource1.DataMember = "masterdt";
            this.listControlBindingSource2.DataMember = "rel1";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.Grid_Filter();
            this.Grid1Select_ReadOnly();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.grid2.ValidateControl();
            if (this.detaildt == null || this.detaildt.Rows.Count == 0)
            {
                return;
            }

            if (this.detaildt.Select("selected = 1").Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first");
                return;
            }

            if (this.detaildt.Select("qty = 0 and Selected = 1").Length > 0)
            {
                MyUtility.Msg.WarningBox("TransferQty of selected row can't be zero!");
                return;
            }

            foreach (DataRow dr in this.detaildt.Select("selected = 1"))
            {
                // 以 GridUniqueKey 來確認不能有重複
                var existsRows = this.detail.AsEnumerable()
                    .Where(w => w.RowState != DataRowState.Deleted
                        && w["mdivisionid"].EqualString(dr["mdivisionid"].ToString())
                        && w["poid"].EqualString(dr["poid"].ToString())
                        && w["seq1"].EqualString(dr["seq1"])
                        && w["seq2"].EqualString(dr["seq2"].ToString())
                        && w["roll"].EqualString(dr["roll"])
                        && w["dyelot"].EqualString(dr["dyelot"])
                        && w["stockType"].EqualString(dr["stockType"])
                        && w["ToPOID"].EqualString(dr["ToPOID"].ToString())
                        && w["Toseq1"].EqualString(dr["Toseq1"])
                        && w["Toseq2"].EqualString(dr["Toseq2"].ToString())
                        && (long)w["TransferExport_DetailUkey"] == (long)dr["TransferExport_DetailUkey"]);

                if (existsRows.Any())
                {
                    existsRows.First()["qty"] = dr["qty"];
                    existsRows.First()["ExportID"] = dr["ExportID"];
                }
                else
                {
                    dr["id"] = this.master["id"];
                    this.detail.ImportRowAdded(dr);
                }
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grid1Select_ReadOnly()
        {
            foreach (DataGridViewRow row in this.grid1.Rows)
            {
                string fabricType = row.Cells["FabricType"].Value.ToString();
                row.Cells["selected"].ReadOnly = !fabricType.EqualString("Accessory");
            }
        }

        private void BtnAutoPick_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            DataTable dt1 = ((DataSet)this.listControlBindingSource1.DataSource).Tables["masterdt"];
            DataTable dt2 = ((DataSet)this.listControlBindingSource1.DataSource).Tables["detaildt"];
            foreach (DataRow dr1 in dt1.AsEnumerable().Where(x => x.Field<bool>("selected")))
            {
                decimal totalTransfer = 0;
                foreach (DataRow dr2 in dt2.AsEnumerable().Where(x => x.Field<string>("ToPOID").EqualString(dr1["POID"]) &&
                                                                    x.Field<string>("Toseq1").EqualString(dr1["Seq1"]) &&
                                                                    x.Field<string>("Toseq2").EqualString(dr1["Seq2"]) &&
                                                                    x.Field<string>("POID").EqualString(dr1["InventoryPOID"]) &&
                                                                    x.Field<string>("seq1").EqualString(dr1["Inventoryseq1"]) &&
                                                                    x.Field<string>("seq2").EqualString(dr1["InventorySEQ2"])))
                {
                    dr2["selected"] = dr2["FabricType"].EqualString("Accessory");
                    if (MyUtility.Convert.GetBool(dr2["selected"]))
                    {
                        decimal taipeiOutput = MyUtility.Convert.GetDecimal(dr1["TaipeiOutput"]);
                        decimal stockBalance = MyUtility.Convert.GetDecimal(dr2["StockBalance"]);
                        totalTransfer = taipeiOutput <= (totalTransfer + stockBalance) ? taipeiOutput : totalTransfer + stockBalance;

                        dr2["qty"] = taipeiOutput <= stockBalance ? taipeiOutput : stockBalance;
                    }

                    dr2.EndEdit();
                }

                dr1["TotalTransfer"] = totalTransfer;
                dr1["Balance"] = MyUtility.Convert.GetDecimal(dr1["TaipeiOutput"]) - MyUtility.Convert.GetDecimal(dr1["AccuTransferred"]) - totalTransfer;
                dr1.EndEdit();
            }
        }

        private void Chk_includeJunk_CheckedChanged(object sender, EventArgs e)
        {
            this.Grid_Filter();
        }

        private void Grid_Filter()
        {
            this.grid1.ValidateControl();
            this.grid2.ValidateControl();

            string filter = string.Empty;
            if (this.grid1 != null)
            {
                switch (this.chk_Balance.Checked)
                {
                    case true:
                        if (MyUtility.Check.Empty(this.grid1))
                        {
                            break;
                        }

                        filter = @" Balance > 0";
                        this.listControlBindingSource1.Filter = filter;
                        break;
                    case false:
                        if (MyUtility.Check.Empty(this.grid1))
                        {
                            break;
                        }

                        filter = string.Empty;
                        this.listControlBindingSource1.Filter = filter;
                        break;
                }
            }
        }

        private void Grid1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.Grid1Select_ReadOnly();
        }
    }
}
