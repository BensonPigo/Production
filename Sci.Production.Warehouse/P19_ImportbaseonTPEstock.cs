using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using System.Data.SqlClient;
using Sci.Production.Prg;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P19_ImportbaseonTPEstock : Win.Subs.Base
    {
        private DataRow master;
        private DataTable detail;
        private DataTable masterdt;
        private DataTable detaildt;

        /// <inheritdoc/>
        public P19_ImportbaseonTPEstock(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.master = master;
            this.detail = detail;
            this.GridSetup();
        }

        private void GridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid1)
                .Text("poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13))
                .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4))
                .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("ToFactory", header: "ToFactory", iseditingreadonly: true, width: Widths.AnsiChars(3))
                .Text("InventoryPOID", header: "Inventory POID", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("Inventoryseq1", header: "Inventory Seq1", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("InventorySEQ2", header: "Inventory Seq2", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Date("TaipeiLastOutput", header: "Taipei" + Environment.NewLine + "Last Output", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("TaipeiOutput", header: "Taipei" + Environment.NewLine + "Output", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
                .Numeric("TotalTransfer", header: "Total Transfer", integer_places: 8, decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(8))
               ;

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = this.grid2.GetDataRow(e.RowIndex);
                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) > MyUtility.Convert.GetDecimal(dr["StockBalance"]))
                    {
                        MyUtility.Msg.WarningBox("TransferQty can not more than Stock Balance!");
                        dr["qty"] = 0;
                    }
                    else
                    {
                        dr["qty"] = e.FormattedValue;
                    }
                    dr.EndEdit();
                    this.CaculateTotalTransfer();
                }
            };

            DataGridViewGeneratorCheckBoxColumnSettings selectedSetting = new DataGridViewGeneratorCheckBoxColumnSettings();
            selectedSetting.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid2.GetDataRow<DataRow>(e.RowIndex);
                dr["Selected"] = e.FormattedValue;
                dr.EndEdit();

                this.CaculateTotalTransfer();
            };

            this.grid2.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid2)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: selectedSetting)
                .Text("WK", header: "WK#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("PoId", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(14))
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6))
                .Text("roll", header: "Roll#", iseditingreadonly: true, width: Widths.AnsiChars(10))
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8))
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(15))
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
                this.CaculateTotalTransfer();
            }
        }

        private void CaculateTotalTransfer()
        {
            decimal totalTransfer = 0;
            foreach (DataGridViewRow gridDr in this.grid2.Rows)
            {
                DataRow dr = this.grid2.GetDataRow(gridDr.Index);
                if (MyUtility.Convert.GetBool(dr["Selected"]))
                {
                    if (dr["qty"].Empty())
                    {
                        dr["qty"] = dr["StockBalance"];
                        dr.EndEdit();
                    }

                    totalTransfer += MyUtility.Convert.GetDecimal(gridDr.Cells["qty"].Value);
                }
            }

            this.grid1.SelectedRows[0].Cells["TotalTransfer"].Value = totalTransfer;
            this.grid1.RefreshEdit();
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
		, ToFactory = i.TransferFactory
		, InventoryPOID = i.InventoryPOID
        , Inventoryseq1 = i.InventorySeq1
        , InventorySEQ2 = i.InventorySeq2
		, psd.StockUnit
        , TaipeiLastOutput = max(i.confirmdate) 
        , TaipeiOutput = sum(iif(i.type='2',i.qty,0-i.qty)) 
into #tmp
from dbo.Invtrans i WITH (NOLOCK) 
inner join PO_Supp_Detail psd WITH (NOLOCK) on i.InventoryPOID = psd.ID
												and i.InventorySeq1 = psd.SEQ1
												and i.InventorySeq2 = psd.SEQ2
inner join Orders o WITH (NOLOCK) on psd.ID = o.ID
where (i.type='2' or i.type='6')
		and i.seq70poid = @IssueSP
		and o.MDivisionID = @MDivisionID
group by rtrim(i.seq70poid), rtrim(i.seq70seq1), i.seq70seq2, i.TransferFactory, i.InventoryPOID, i.InventorySeq1, i.InventorySeq2, psd.StockUnit


select  selected = cast(0 as bit)
		, [WK] = Stuff((select distinct concat(',', r.ExportId)
				from Receiving r WITH (NOLOCK)
                inner join Receiving_Detail rd WITH (NOLOCK) on r.id = rd.id
                where rd.PoId = fi.POID and rd.Seq1 = fi.SEQ1 and rd.Seq2 = fi.SEQ2 and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
				FOR XML PATH('')),1,1,'') 
		, MDivisionID = @MDivisionID
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
into #tmpDetailResult
from #tmp  
inner join dbo.FtyInventory fi WITH (NOLOCK) on fi.POID = InventoryPOID 
                                                and fi.seq1 = Inventoryseq1 
                                                and fi.seq2 = InventorySEQ2 
                                                and fi.StockType = 'I'
where fi.Lock = 0
Order by GroupQty desc, Dyelot, StockBalance desc

select distinct  [POID] = ToPOID
		        , [Seq1] = ToSeq1
		        , [Seq2] = ToSeq2
		        , ToFactory
		        , InventoryPOID
                , Inventoryseq1
                , InventorySEQ2
		        , StockUnit
                , TaipeiLastOutput
                , TaipeiOutput
                , [TotalTransfer] = 0.0
from #tmpDetailResult
where   StockBalance > 0
order by ToPOID, ToSeq1, ToSeq2;

select  *
from    #tmpDetailResult
where   StockBalance > 0


drop table #tmp, #tmpDetailResult
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
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.grid2.ValidateControl();
            if (this.detaildt == null || this.detaildt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drs = this.detaildt.Select("selected = 1");
            if (drs.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select row(s) first!");
                return;
            }

            drs = this.detaildt.Select("qty = 0 and Selected = 1");
            if (drs.Length > 0)
            {
                MyUtility.Msg.WarningBox("TransferQty of selected row can't be zero!", "Warning");
                return;
            }

            foreach (DataRow dr in this.detaildt.Select("selected = 1"))
            {
                DataRow[] findrow = this.detail.AsEnumerable()
                    .Where(w => w.RowState != DataRowState.Deleted
                        && w["poid"].EqualString(dr["poid"].ToString())
                        && w["seq1"].EqualString(dr["seq1"])
                        && w["seq2"].EqualString(dr["seq2"].ToString())
                        && w["ToPOID"].EqualString(dr["ToPOID"].ToString())
                        && w["Toseq1"].EqualString(dr["Toseq1"])
                        && w["Toseq2"].EqualString(dr["Toseq2"].ToString())
                        && w["roll"].EqualString(dr["roll"])
                        && w["dyelot"].EqualString(dr["dyelot"])
                        && w["stockType"].EqualString(dr["stockType"])).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = dr["qty"];
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
    }
}
