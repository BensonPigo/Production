using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    public partial class P04_ImportMiscellaneous : Sci.Win.Subs.Base
    {
        private DataTable detailData;
        public P04_ImportMiscellaneous(DataTable dt)
        {
            this.InitializeComponent();
            this.detailData = dt;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToInt32(e.FormattedValue) > Convert.ToInt32(dr["Old_Qty"]))
                    {
                        MyUtility.Msg.WarningBox($"<Qty>:{e.FormattedValue} cannot exceed original Qty:{dr["Old_Qty"]}");
                        e.Cancel = true;
                    }
                }
            };

            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("TransactionID", header: "Local Misc. PO", width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("Supp", header: "Local Supp.", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Type", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 2, settings: ns)
                ;
            this.gridImport.Columns["Qty"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            #region sql where
            string sqlwhere = string.Empty;
            if (!MyUtility.Check.Empty(this.txtLocalMiscPoNo.Text))
            {
                sqlwhere += $" and mpd.ID = '{this.txtLocalMiscPoNo.Text}'" + Environment.NewLine;
            }

            if (!MyUtility.Check.Empty(this.dateDeliveryDate.Value1) && !MyUtility.Check.Empty(this.dateDeliveryDate.Value2))
            {
                sqlwhere += $" and mp.DeliveryDate between '{((DateTime)this.dateDeliveryDate.Value1).ToString("yyyy/MM/dd")}' and '{((DateTime)this.dateDeliveryDate.Value2).ToString("yyyy/MM/dd")}'" + Environment.NewLine;
            }

            #endregion
            string sqlcmd = $@"
select
[Selected] = 0
,[TransactionID] = mp.ID
,mp.FactoryID
,[POID] = ''
,mpd.SEQ1 ,mpd.SEQ2
,[Seq] = mpd.SEQ1+'-'+mpd.SEQ2
,[BrandID] = m.MiscBrandID
,[BuyerDelivery] = mp.DeliveryDate
,[ToPOID] = ''
,[ToSEQ] = ''
,[Supp] = mp.LocalSuppID
,[Refno]=mpd.MiscID
,m.Description
,[Type] = ''
,[MtlTypeID] = m.PurchaseType
,[UnitId] = m.UnitID
,[Qty] = mpd.InQty - ftyExport.Qty
,[Old_Qty] = mpd.InQty - ftyExport.Qty
,[NW]=0.00
,[GW]=0.00
from Machine.dbo.MiscPO mp 
inner join Machine.dbo.MiscPO_Detail mpd on mpd.ID = mp.ID
left join Machine.dbo.Misc m on m.ID = mpd.MiscID
left join Production.dbo.LocalSupp ls on ls.ID = mp.LocalSuppID
outer apply(
	select Qty = isnull(sum(Qty),0)
	from Production.dbo.FtyExport_Detail f
	where f.TransactionID = mpd.ID 
	and f.Seq1=mpd.SEQ1
	and f.Seq2=mpd.SEQ2
)ftyExport
where mp.PurchaseFrom = 'L'
and ls.IsMiscOverseas = 1
and mpd.InQty - ftyExport.Qty > 0
{sqlwhere}
";
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out selectData);
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

        // import
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

            DataRow[] dr = gridData.Select("Selected = 1");
            if (dr.Length > 0)
            {
                foreach (DataRow currentRow in dr)
                {
                    DataRow[] findrow = this.detailData.Select($@"
TransactionID = '{MyUtility.Convert.GetString(currentRow["TransactionID"])}' 
AND SEQ1 = '{MyUtility.Convert.GetString(currentRow["SEQ1"])}'
AND SEQ2 = '{MyUtility.Convert.GetString(currentRow["SEQ2"])}'
AND RefNo = '{MyUtility.Convert.GetString(currentRow["Refno"])}'");

                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        this.detailData.ImportRow(currentRow);
                    }
                    else
                    {
                        findrow[0]["Qty"] = MyUtility.Convert.GetDouble(currentRow["Qty"]);
                        findrow[0]["NetKg"] = MyUtility.Convert.GetDouble(currentRow["NW"]);
                        findrow[0]["WeightKg"] = MyUtility.Convert.GetDouble(currentRow["GW"]);
                    }
                }
            }

            MyUtility.Msg.InfoBox("Import completed!");
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
