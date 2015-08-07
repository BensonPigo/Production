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
    public partial class P04_ImportLocalPO : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataTable detailData;
        public P04_ImportLocalPO(DataTable dt)
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
                .Text("LocalPOID", header: "Local Purchase#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ThreadColorID", header: "Color Shade", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UnitId", header: "Unit", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("Qty", header: "Import Q'ty", decimal_places: 2)
                .Numeric("NetKg", header: "N.W.(kg)", decimal_places: 2)
                .Numeric("WeightKg", header: "N.W.(kg)", decimal_places: 2);
        }

        //Qurey
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(textBox1.Text) && MyUtility.Check.Empty(textBox2.Text))
            {
                MyUtility.Msg.WarningBox("< SP# > or < Local Purchase# > can't be empty!");
                textBox1.Focus();
                return;
            }

            string sqlCmd = @"select lo.*, 1 as Selected, (lo.SuppID+' - '+ls.Abb) as Supp,isnull(li.Description,'') as Description,li.Category as MtlTypeID,
o.BuyerDelivery,isnull(o.BrandID,'') as BrandID,isnull(o.FactoryID,'') as FactoryID,o.SciDelivery,0.0 as NetKg,0.0 as WeightKg,
'' as Seq1,'' as Seq2,'' as Seq,'' as FabricType
from (select l.Id as LocalPOID,ld.OrderId as POID,l.LocalSuppID as SuppID,SUBSTRING(ld.Id+ld.ThreadColorID,1,26) as SCIRefno,ld.Refno,ld.ThreadColorID,ld.UnitId,ld.Qty
      from LocalPO l, LocalPO_Detail ld
	  where l.Id = ld.Id";
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and l.id = '{0}'",textBox2.Text.Trim());
            }
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and ld.OrderId = '{0}'",textBox1.Text.Trim());
            }
            if (!MyUtility.Check.Empty(txtsubcon1.TextBox1.Text))
            {
                sqlCmd = sqlCmd + string.Format(" and l.LocalSuppID = '{0}'",txtsubcon1.TextBox1.Text.Trim());
            }
            sqlCmd = sqlCmd + @") lo
left join Orders o on o.ID = lo.POID
left join LocalItem li on li.RefNo = lo.Refno
left join LocalSupp ls on ls.ID = lo.SuppID";
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
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
                    DataRow[] findrow = detailData.Select(string.Format("POID = '{0}' and SCIRefNo = '{1}' and RefNo = '{2}'", currentRow["POID"].ToString(), currentRow["SCIRefNo"].ToString(), currentRow["RefNo"].ToString()));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        detailData.ImportRow(currentRow);
                    }
                    else
                    {
                        findrow[0]["Qty"] = Convert.ToDouble(currentRow["Qty"]);
                        findrow[0]["NetKg"] = Convert.ToDouble(currentRow["NetKg"]);
                        findrow[0]["WeightKg"] = Convert.ToDouble(currentRow["WeightKg"]);
                    }
                }
            }
            MyUtility.Msg.InfoBox("Import completed!");
        }
    }
}
