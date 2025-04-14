using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P04_ImportTransferOut
    /// </summary>
    public partial class P04_ImportTransferOut : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable detailData;

        /// <summary>
        /// P04_ImportTransferOut
        /// </summary>
        /// <param name="dt">dt</param>
        public P04_ImportTransferOut(DataTable dt)
        {
            this.InitializeComponent();
            this.detailData = dt;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("TransactionID", header: "Transfer Out No./ Adjust No.", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("FabricType", header: "Type", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Q'ty", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "N.W.(kg)", decimal_places: 2)
                .Text("ToPOID", header: "To SP", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ToSeq", header: "To Seq", width: Widths.AnsiChars(6), iseditingreadonly: true);
        }

        // Qurey
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtTransferOutNo.Text))
            {
                this.txtTransferOutNo.Focus();
                MyUtility.Msg.WarningBox("< Transfer Out No. > can't be empty!");
                return;
            }

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", this.txtTransferOutNo.Text.Trim());
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@orderCompanyID", P04.orderCompanyID);

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>
            {
                sp1,
                sp2
            };

            string sqlCmd = @"
select Selected = 1
       , [TransactionID] = td.ID
	   , td.Poid
	   , td.Seq1
	   , td.Seq2
	   , Seq = (left(td.Seq1 + ' ', 3) + '-' + td.Seq2)
	   , SuppID = isnull(ps.SuppID, '')
	   , Supp = (isnull(ps.SuppID, '') + '-' + isnull(s.AbbEN, ''))
	   , RefNo = isnull(psd.Refno, '')
	   , SCIRefNo = isnull(psd.SCIRefno, '')
	   , Description = isnull(f.DescDetail, '') 
	   , FabricType = isnull(psd.FabricType, '')
	   , Type = (case 
	   				when psd.FabricType = 'F' then 'Fabric' 
	   				when psd.FabricType = 'A' then 'Accessory' 
   				 	else '' 
			 	 end)
	   , MtlTypeID = isnull(f.MtlTypeID, '') 
	   , UnitId = isnull(psd.StockUnit, '') 
	   , td.Qty
	   , NetKg = 0.0
	   , WeightKg = 0.0
	   , o.BuyerDelivery
	   , BrandID = isnull(o.BrandID, '')
	   , FactoryID = isnull(o.FactoryID, '')
	   , o.SciDelivery
	   ,td.ToPOID
	   ,td.ToSeq1
	   ,td.ToSeq2
       ,[ToSeq] = td.ToSeq1 + ' ' +td.ToSeq2
       ,td.TransferExportID
from TransferOut_Detail td WITH (NOLOCK) 
left join PO_Supp ps WITH (NOLOCK) on ps.ID = td.Poid and ps.SEQ1 = td.Seq1
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = td.Poid and psd.SEQ1= td.Seq1 and psd.SEQ2 = td.Seq2
left join Supp s WITH (NOLOCK) on s.ID = ps.SuppID
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join Orders o WITH (NOLOCK) on o.ID = td.Poid
where td.ID = @id
and o.OrderCompanyID = @orderCompanyID

union all

select Selected = 1
       , [TransactionID] = ad.ID
	   , ad.Poid
	   , ad.Seq1
	   , ad.Seq2
	   , Seq = (left(ad.Seq1 + ' ', 3) + '-' + ad.Seq2)
	   , SuppID = isnull(ps.SuppID, '')
	   , Supp = (isnull(ps.SuppID, '') + '-' + isnull(s.AbbEN, ''))
	   , RefNo = isnull(psd.Refno, '')
	   , SCIRefNo = isnull(psd.SCIRefno, '')
	   , Description = isnull(f.DescDetail, '') 
	   , FabricType = isnull(psd.FabricType, '')
	   , Type = (case 
	   				when psd.FabricType = 'F' then 'Fabric' 
	   				when psd.FabricType = 'A' then 'Accessory' 
   				 	else '' 
			 	 end)
	   , MtlTypeID = isnull(f.MtlTypeID, '') 
	   , UnitId = isnull(psd.StockUnit, '') 
	   , Qty = isnull(ad.QtyAfter,0.00) - isnull(ad.QtyBefore,0.00) 
	   , NetKg = 0.0
	   , WeightKg = 0.0
	   , o.BuyerDelivery
	   , BrandID = isnull(o.BrandID, '')
	   , FactoryID = isnull(o.FactoryID, '')
	   , o.SciDelivery
	   ,[ToPOID] = ad.POID
	   ,[ToSeq1] = ad.Seq1
	   ,[ToSeq2] = ad.Seq2
       ,[ToSeq] = ad.Seq1 + ' ' +ad.Seq2
       ,[TransferExportID] = ''
from Adjust_Detail ad WITH (NOLOCK) 
inner join Adjust a with (nolock) on ad.ID = a.ID
left join Reason r on r.ID = ad.ReasonId  and ReasonTypeID = 'Stock_Adjust'
left join PO_Supp ps WITH (NOLOCK) on ps.ID = ad.Poid and ps.SEQ1 = ad.Seq1
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = ad.Poid and psd.SEQ1= ad.Seq1 and psd.SEQ2 = ad.Seq2
left join Supp s WITH (NOLOCK) on s.ID = ps.SuppID
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join Orders o WITH (NOLOCK) on o.ID = ad.Poid
where ad.ID = @id
and r.TransferOut = 1
and a.Type in ('A','B') -- P34,P35
and o.OrderCompanyID = @orderCompanyID
";
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
                return;
            }

            if (selectData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }

            this.listControlBindingSource1.DataSource = selectData;
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(gridData) || gridData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return;
            }

            List<string> drChkExport = gridData.AsEnumerable()
                .Where(w => !MyUtility.Check.Empty(w["Selected"]) && !MyUtility.Check.Empty(w["TransferExportID"]))
                .Select(s => MyUtility.Convert.GetString(s["Poid"]))
                .Distinct().ToList();

            if (drChkExport.Count > 0)
            {
                string spList = string.Empty;
                foreach (var item in drChkExport)
                {
                    spList += $@"{item}" + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(@"The following SP# contains Transfer WK# and can not import into Shipping_P04." + Environment.NewLine + spList);
                return;
            }

            DataRow[] dr = gridData.Select("Selected = 1");
            if (dr.Length > 0)
            {
                DataTable dtComputeQty;
                string strComputeQtySQL = @"
select TransactionID
       , Poid
	   , Seq1
	   , Seq2
	   , Seq
	   , SuppID
	   , Supp
	   , RefNo
	   , SCIRefNo
	   , Description
	   , FabricType
	   , Type 
	   , MtlTypeID
	   , UnitId
	   , Qty = sum(Qty)
	   , NetKg = sum(NetKg)
	   , WeightKg = sum(WeightKg)
	   , BuyerDelivery
	   , BrandID
	   , FactoryID
	   , SciDelivery
	   , ToPOID
	   , ToSeq1
	   , ToSeq2

from #tmp
group by TransactionID,Poid, Seq1, Seq2, Seq, SuppID, Supp, RefNo
		 , SCIRefNo, Description, FabricType, Type 
	   	 , MtlTypeID, UnitId, BuyerDelivery, BrandID
	   	 , FactoryID, SciDelivery, ToPOID , ToSeq1, ToSeq2";
                MyUtility.Tool.ProcessWithDatatable(dr.CopyToDataTable(), string.Empty, strComputeQtySQL, out dtComputeQty);
                foreach (DataRow currentRow in dtComputeQty.Rows)
                {
                    string sqlChk = $@"select 1 from FtyExport_Detail where TransactionID = '{currentRow["TransactionID"]}'";
                    if (MyUtility.Check.Seek(sqlChk))
                    {
                        MyUtility.Msg.WarningBox("Transfer Out No. / Adjust No. <No> already exists in WK# <WK>.");
                        return;
                    }

                    DataRow[] findrow = this.detailData.Select($@"
TransactionID = '{MyUtility.Convert.GetString(currentRow["TransactionID"])}' 
AND POID = '{MyUtility.Convert.GetString(currentRow["POID"])}' 
AND Seq1 = '{MyUtility.Convert.GetString(currentRow["Seq1"])}'
AND Seq2 = '{MyUtility.Convert.GetString(currentRow["Seq2"])}'
AND ToPOID = '{MyUtility.Convert.GetString(currentRow["ToPOID"])}' 
AND ToSeq1 = '{MyUtility.Convert.GetString(currentRow["ToSeq1"])}'
AND ToSeq2 = '{MyUtility.Convert.GetString(currentRow["ToSeq2"])}'
");

                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        this.detailData.ImportRow(currentRow);
                    }
                    else
                    {
                        findrow[0]["Qty"] = currentRow["Qty"];
                        findrow[0]["NetKg"] = currentRow["NetKg"];
                        findrow[0]["WeightKg"] = currentRow["WeightKg"];
                    }
                }
            }

            MyUtility.Msg.InfoBox("Import completed!");
        }
    }
}
