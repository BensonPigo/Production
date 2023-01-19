using Ict;
using Ict.Win;
using Ict.Win.Defs;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P19_TKSeparateHistory : Win.Subs.Base
    {
        private int indexFirst = 0;
        private string strID;
        private DataTable dt_PK = new DataTable();
        private DataTable dt_TK = new DataTable();

        /// <inheritdoc/>
        public P19_TKSeparateHistory(string ID)
        {
            this.InitializeComponent();
            this.strID = ID;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            #region combo
            string sqlcmd_combo = $@"
                                    select [ID] = '',[NewID] =''
                                    union
                                    select distinct
                                    [ID] = tes.NewID,
                                    tes.NewID
                                    from TransferOut_Detail tod with(nolock)
                                    left join TransferExport_SeparateHistory tes with(nolock) on tod.TransferExport_DetailUkey = tes.NewDetailUkey
                                    where tod.id = '{this.strID}'";
            DualResult dualResult = DBProxy.Current.Select(null, sqlcmd_combo, out DataTable dt);
            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.comboTK.DataSource = dt;
            this.comboTK.ValueMember = "ID";
            this.comboTK.DisplayMember = "NewID";
            #endregion

            #region TK List
            this.Helper.Controls.Grid.Generator(this.gridTransferWK)
            .Text("FromSP", header: "From SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("ToSP", header: "To SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ToSeq", header: "To SEQ", width: Widths.AnsiChars(11), iseditingreadonly: true)
            .Numeric("OriPoQty", header: "Ori Po Q'ty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("OriExportQty", header: "Ori Export Q'ty", iseditingreadonly: true, width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10)
            .Text("OriTK", header: "Ori TK#", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("NewTK", header: "New TK#", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Numeric("NewPoQty", header: "New Po Q'ty", width: Widths.AnsiChars(11), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
            .Numeric("NewExportQty", header: "New Export Q'ty", iseditingreadonly: true, width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10)
            .Text("StockUnit", header: "Stock Unit", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("Supplier", header: "Supplier", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25))
            .Text("MaterialType", header: "Material Type", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("Color", header: "Color", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("Reason", header: "Reason", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("ReasonDesc", header: "Reason Desc", iseditingreadonly: true, width: Widths.AnsiChars(15))
            ;

            // gridTransferWK的表頭排序鎖定
            for (int i = 0; i < this.gridTransferWK.ColumnCount; i++)
            {
                this.gridTransferWK.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            string sqlcmd_TK = $@"select distinct
                                [FromSP] = ted.InventoryPOID
                                ,[FromSEQ] = Concat (ted.InventorySeq1, ' ', ted.InventorySeq2)
                                ,[ToSP] = ted.PoID
                                ,[ToSeq] = CONCAT(ted.seq1,' ',ted.seq2)
                                ,[OriPoQty] = tes.PoQty
                                ,[OriExportQty] = tes.StockExportQty
                                ,[OriTK] = tes.ID
                                ,[NewTK] = tes.[NewID]
                                ,[NewPoQty] = ted.PoQty
                                ,[NewExportQty] = tdc.StockQty
                                ,[StockUnit] = tes.StockUnitID
                                ,[Supplier] =ted.SuppID
                                ,[Description] = ted.[Description]
                                ,[MaterialType] = Concat(
					                                case ted.FabricType
						                                when 'F' then 'Fabric'
						                                when 'A' then 'Accessory'
						                                when 'O' then 'Other'
					                                end
					                                , '-' +  'Fabric.MtlTypeID')
                                ,[Color] = SpecValue.val
                                ,[Reason] = ted.TransferExportReason
                                ,[ReasonDesc] = Reasondesc.val
                                from TransferOut_Detail tod with(nolock) 
                                inner join TransferExport_SeparateHistory tes with(nolock) on tod.TransferExport_DetailUkey = tes.NewDetailUkey
                                left join TransferExport_Detail ted with(nolock) on ted.Ukey = tes.NewDetailUkey
                                inner join TransferExport_Detail_Carton tdc with(nolock) on ted.Ukey = tdc.TransferExport_DetailUkey
                                outer apply
                                (
	                                select val = psds.SpecValue
	                                from PO_Supp_Detail psd
	                                left join PO_Supp_Detail_Spec psds on psd.ID = psds.id
									                                and psd.Seq1 = psds.seq1
									                                and psd.Seq2 = psds.Seq2
									                                and psds.SpecColumnID = 'Color'
	                                where psd.ID = ted.InventoryPOID
	                                  and psd.Seq1 = ted.InventorySeq1
	                                  and psd.Seq2 = ted.InventorySeq2
                                )SpecValue
                                outer apply
                                (
	                                select val = Description
	                                from WhseReason
	                                where Type ='TE' and id =ted.TransferExportReason
                                )Reasondesc
                                where tod.id = '{this.strID}' and tod.TransferExport_DetailUkey in (select max(TransferExport_DetailUkey) from TransferOut_Detail group by TransferExport_DetailUkey)
                                order by [FromSP],[FromSEQ],[ToSP],[ToSEQ],[NewTK] asc";
            DualResult dualResult_TK = DBProxy.Current.Select(null, sqlcmd_TK, out this.dt_TK);
            if (!dualResult_TK)
            {
                MyUtility.Msg.WarningBox(dualResult_TK.ToString());
                return;
            }

            this.gridTransferWK.DataSource = this.dt_TK;
            #endregion TK List
            #region Packing List
            this.Helper.Controls.Grid.Generator(this.gridPacking)
            .Text("FromSP", header: "From SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("FromSEQ", header: "From SEQ", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("ToSP", header: "To SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("ToSEQ", header: "To SEQ", width: Widths.AnsiChars(11), iseditingreadonly: true)
            .Text("NewTK", header: "New TK#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("FtyGroupID", header: "Fty Group ID", iseditingreadonly: true, width: Widths.AnsiChars(15))
            .Text("CartonNo", header: "Carton No", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("Roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(20))
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(11), iseditingreadonly: true)
            .Numeric("ExportQty", header: "Export Q'ty", iseditingreadonly: true, width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10)
            .Numeric("NetKg", header: "N.W. (kg)", iseditingreadonly: true, width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10)
            .Numeric("GWKg", header: "G.W. (Kg)", iseditingreadonly: true, width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10)
            .Numeric("CBM", header: "CBM", iseditingreadonly: true, width: Widths.AnsiChars(15), decimal_places: 2, integer_places: 10)
            ;

            // gridTransferWK的表頭排序鎖定
            for (int i = 0; i < this.gridPacking.ColumnCount; i++)
            {
                this.gridPacking.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            string sqlcmd_PK = $@"select
                                [FromSP] = ted.InventoryPOID
                                ,[FromSEQ] = Concat (ted.InventorySeq1, ' ', ted.InventorySeq2)
                                ,[ToSP] = ted.PoID
                                ,[ToSEQ] = Concat (ted.Seq1, ' ', ted.Seq2)
                                ,[NewTK] =tdc.ID
                                ,[FtyGroupID] = tdc.GroupID
                                ,[CartonNo] = tdc.Carton
                                ,[Roll] = tdc.Roll
                                ,[Dyelot] = tdc.LotNo
                                ,[ExportQty] = tdc.StockQty
                                ,[NetKg] = tdc.NetKg
                                ,[GWKg] = tdc.WeightKg
                                ,[CBM] = tdc.CBM
                                from TransferOut_Detail tod with(nolock)
                                inner join TransferExport_SeparateHistory tes with(nolock) on tod.TransferExport_DetailUkey = tes.NewDetailUkey
                                inner join TransferExport_Detail ted with(nolock) on ted.Ukey = tes.NewDetailUkey
                                inner join TransferExport_Detail_Carton tdc with(nolock) on ted.Ukey = tdc.TransferExport_DetailUkey
                                where tod.id = '{this.strID}' and tod.TransferExport_DetailUkey in (select max(TransferExport_DetailUkey) from TransferOut_Detail group by TransferExport_DetailUkey)
                                order by [FromSP],[FromSEQ],[ToSP],[ToSEQ],[NewTK] asc";
            DualResult dualResult_PK = DBProxy.Current.Select(null, sqlcmd_PK, out this.dt_PK);
            if (!dualResult_PK)
            {
                MyUtility.Msg.WarningBox(dualResult_PK.ToString());
                return;
            }

            this.gridPacking.DataSource = this.dt_PK;
            #endregion Packing List
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboTK_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.dt_TK == null || this.dt_TK.Rows.Count == 0)
            {
                return;
            }

            if (this.dt_PK == null || this.dt_PK.Rows.Count == 0)
            {
                return;
            }

            var cbVal = this.comboTK.SelectedValue.ToString();
            if (MyUtility.Check.Empty(cbVal))
            {
                this.gridTransferWK.DataSource = this.dt_TK;
                this.gridPacking.DataSource = this.dt_PK;
            }
            else
            {
                this.gridTransferWK.DataSource = this.dt_TK.Select($"NewTK = '{cbVal}'").CopyToDataTable();
                this.gridPacking.DataSource = this.dt_PK.Select($"NewTK = '{cbVal}'").CopyToDataTable();
            }
        }
        
        private void GridTransferWK_SelectionChanged(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.gridTransferWK.DataSource))
            {
                return;
            }

            if (this.dt_TK == null || this.dt_TK.Rows.Count == 0)
            {
                return;
            }

            if (this.dt_PK == null || this.dt_PK.Rows.Count == 0)
            {
                return;
            }

            DataRow tk_Value = this.gridTransferWK.CurrentDataRow;
            DataGridViewRow dgvrPacking = this.gridPacking.Rows.OfType<DataGridViewRow>().Where(x =>
                                            (string)x.Cells["NewTK"].Value == tk_Value["NewTK"].ToString()).FirstOrDefault();

            if (dgvrPacking == null)
            {
                return;
            }
            this.gridPacking.Rows[indexFirst].Selected = false;
            this.indexFirst = dgvrPacking.Index;
            this.gridPacking.Rows[dgvrPacking.Index].Selected = true;
        }
    }
}
