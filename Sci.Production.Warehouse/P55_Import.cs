using Ict;
using Ict.Win;
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
    public partial class P55_Import : Win.Subs.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private DataTable dtArtwork;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        public P55_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.CellValueChanged += (s, e) =>
            {
                if (this.grid1.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.grid1.GetDataRow(e.RowIndex);
                    dr.EndEdit();
                    this.Sum_checkedqty();
                }
            };

            this.grid1.IsEditingReadOnly = false; // 開啟CheckBox圖示
            this.grid1.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("POID", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(12)) // 1
                .Text("Seq", header: "Seq", iseditingreadonly: true) // 2
                .Text("roll", header: "Roll", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 3
                .Text("dyelot", header: "Dyelot", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 4
                .Text("Refno", header: "Refno", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 5
                .Numeric("ReceivingQty", header: "Receiving Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(15), integer_places: 10) // 6
                .Numeric("Qty", header: "Transfer Out Qty", decimal_places: 2, iseditingreadonly: true, width: Widths.AnsiChars(15), integer_places: 10) // 7
                .Text("StockUnit", header: "Stock Unit", iseditingreadonly: true, width: Widths.AnsiChars(8)) // 8
                .Text("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)); // 9
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            object localPrice = dt.Compute("Sum(Qty)", "Selected = 1");
            this.displayTotal.Value = localPrice.ToString();
        }

        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp = this.txtSP.Text.TrimEnd();
            string tsi = this.txtTransfertoSubconID.Text.Trim();
            string tod = this.dateTransferOutDate.Value.ToString();
            string strSubCon = this.dr_master["SubCon"].ToString();
            string strID = this.dr_master["ID"].ToString();

            if (MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(tsi) && MyUtility.Check.Empty(tod))
            {
                MyUtility.Msg.WarningBox("Transfer to Sub con ID, Transfer Out Date and SP# cannot all be empty.");
                this.txtSP.Focus();
                return;
            }

            strSQLCmd.Append($@"
            select 
            selected = 0 
            ,f.POID
            ,[Seq] = Concat (f.Seq1, ' ',f.Seq2 )
            ,f.Seq1
            ,f.Seq2
            ,f.Roll
            ,f.Dyelot
            ,psd.Refno
            ,[ReceivingQty] = isnull( rdQty.ActualQty,0) + isnull(tidQty.Qty,0)
            ,[Qty] =td.Qty
            ,[StockUnit] = psd.StockUnit
            ,[Description] = Dbo.getMtlDesc (f.POID, f.Seq1, f.Seq2, 2, 0 )
            ,f.StockType
            ,f.SubConStatus
            ,[TransferToSubcon_DetailUkey] = td.Ukey
            ,ID = ''
            from FtyInventory f with(nolock)
            left join PO_Supp_Detail psd with(nolock) on f.POID = psd.ID and
											                f.Seq1 = psd.SEQ1 and
											                f.Seq2 = psd.SEQ2
            left join TransferToSubcon_Detail td with(nolock) on td.POID = f.POID and
													                td.Seq1 = f.Seq1 and
													                td.Seq2 = f.Seq2 and
													                td.Roll = f.Roll and
													                td.Dyelot = f.Dyelot and
													                td.StockType = f.StockType
            inner join TransferToSubcon t with(nolock) on t.ID = td.ID 
                                                          and t.Subcon = f.SubConStatus
            outer apply
            (
	            select rd.ActualQty 
	            from Receiving_Detail rd with(nolock)
	            inner join Receiving r with(nolock) on rd.Id = r.id
	            where r.Type = 'A' and
		                f.POID = rd.PoId and
		                f.Seq1 = rd.Seq1 and 
		                f.Seq2 = rd.Seq2 and 
		                f.Roll = rd.Roll and 
		                f.Dyelot = rd.Dyelot
            )rdQty
            outer apply
            (
	            select tid.Qty
	            from TransferIn_Detail tid with(nolock)
	            where  f.POID = tid.PoId and 
		                f.Seq1 = tid.Seq1 and 
		                f.Seq2 = tid.Seq2 and
		                f.Roll = tid.Roll and 
		                f.Dyelot = tid.Dyelot
            )tidQty
            where 
            not exists(
			select 1
	        from SubconReturn t with(nolock)
            inner join SubconReturn_Detail td with(nolock) on t.ID = td.ID
            where f.POID = td.PoId and 
		            f.Seq1 = td.Seq1 and
		            f.Seq2  = td.Seq2 and 
		            f.Roll = td.Roll and 
		            f.Dyelot = td.Dyelot and
		            f.StockType = td.StockType and 
		            t.Subcon = '{strSubCon}' and
                    t.ID != '{strID}'
			) 
            and psd.FabricType ='F' 
            and f.StockType ='B' 
            and f.SubConStatus = '{strSubCon}'");

            if (!MyUtility.Check.Empty(this.txtTransfertoSubconID.Text))
            {
                strSQLCmd.Append($@" and t.ID = '{this.txtTransfertoSubconID.Text}'");
            }

            if (!MyUtility.Check.Empty(this.dateTransferOutDate.Value))
            {
                strSQLCmd.Append($@" and t.TransferOutDate = '{this.dateTransferOutDate.Text}'");
            }

            if (!this.txtSeq1.Seq1.Empty())
            {
                strSQLCmd.Append($@" and f.seq1 = '{this.txtSeq1.Seq1}'");
            }

            if (!this.txtSeq1.Seq2.Empty())
            {
                strSQLCmd.Append($@" and f.seq2 = '{this.txtSeq1.Seq2}'");
            }

            if (!MyUtility.Check.Empty(this.txtRefno.Text))
            {
                strSQLCmd.Append($@" and psd.Refno = '{this.txtRefno.Text}'");
            }

            DualResult dualResult = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtArtwork);

            if (!dualResult)
            {
                MyUtility.Msg.WarningBox(dualResult.ToString());
                return;
            }

            this.listControlBindingSource1.DataSource = this.dtArtwork;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();

            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");

            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                          && row["poid"].EqualString(tmp["poid"].ToString())
                                                                          && row["seq1"].EqualString(tmp["seq1"])
                                                                          && row["seq2"].EqualString(tmp["seq2"])
                                                                          && row["roll"].EqualString(tmp["roll"])
                                                                          && row["dyelot"].EqualString(tmp["dyelot"])
                                                                        ).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["seq"] = tmp["seq"];
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["StockUnit"] = tmp["StockUnit"];
                    findrow[0]["StockType"] = tmp["StockType"];
                    findrow[0]["Description"] = tmp["Description"];
                    findrow[0]["TransferToSubcon_DetailUkey"] = tmp["TransferToSubcon_DetailUkey"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }
    }
}
