using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

namespace Sci.Production.Shipping
{
    public partial class P04_ImportTransferOut : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataTable detailData;
        public P04_ImportTransferOut(DataTable dt)
        {
            InitializeComponent();
            detailData = dt;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.grid1.IsEditingReadOnly = false;
            grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Seq", header: "SEQ", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("FabricType", header: "Type", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Import Q'ty", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "N.W.(kg)", decimal_places: 2);
        }

        //Qurey
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(textBox1.Text))
            {
                MyUtility.Msg.WarningBox("< Transfer Out No. > can't be empty!");
                textBox1.Focus();
                return;
            }
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", textBox1.Text.Trim());

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string sqlCmd = @"select 1 as Selected,td.Poid,td.Seq1,td.Seq2,(left(td.Seq1+' ',3)+'-'+td.Seq2) as Seq,isnull(ps.SuppID,'') as SuppID,
(isnull(ps.SuppID,'')+'-'+isnull(s.AbbEN,'')) as Supp,isnull(psd.Refno,'') as RefNo,isnull(psd.SCIRefno,'') as SCIRefNo,
isnull(f.DescDetail,'') as Description,isnull(psd.FabricType,'') as FabricType,
(case when psd.FabricType = 'F' then 'Fabric' when psd.FabricType = 'A' then 'Accessory' else '' end) as Type,
isnull(f.MtlTypeID,'') as MtlTypeID,isnull(psd.StockUnit,'') as UnitId,td.Qty,0.0 as NetKg,0.0 as WeightKg,
o.BuyerDelivery,isnull(o.BrandID,'') as BrandID,isnull(o.FactoryID,'') as FactoryID,o.SciDelivery
from TransferOut_Detail td
left join PO_Supp ps on ps.ID = td.Poid and ps.SEQ1 = td.Seq1
left join PO_Supp_Detail psd on psd.ID = td.Poid and psd.SEQ1= td.Seq1 and psd.SEQ2 = td.Seq2
left join Supp s on s.ID = ps.SuppID
left join Fabric f on f.SCIRefno = psd.SCIRefno
left join Orders o on o.ID = td.Poid
where td.ID = @id";
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error."+result.ToString());
                return;
            }
            if (selectData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }
            listControlBindingSource1.DataSource = selectData;
        }

        //Import
        private void button2_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            DataTable gridData = (DataTable)listControlBindingSource1.DataSource;
            if (gridData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return;
            }

            DataRow[] dr = gridData.Select("Selected = 1");
            if (dr.Length > 0)
            {
                foreach (DataRow currentRow in dr)
                {
                    DataRow[] findrow = detailData.Select(string.Format("POID = '{0}' and Seq1 = '{1}' and Seq2 = '{2}'", MyUtility.Convert.GetString(currentRow["POID"]), MyUtility.Convert.GetString(currentRow["Seq1"]), MyUtility.Convert.GetString(currentRow["Seq2"])));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        detailData.ImportRow(currentRow);
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
