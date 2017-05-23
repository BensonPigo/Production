﻿using System;
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
    public partial class P04_ImportTransferIn : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataTable detailData;
        public P04_ImportTransferIn(DataTable dt)
        {
            InitializeComponent();
            detailData = dt;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false;
            gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
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
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtSPNo.Text))
            {
                txtSPNo.Focus();
                MyUtility.Msg.WarningBox("< SP# > can't be empty!");
                return;
            }
            if (MyUtility.Check.Empty(txtScifactoryFromFactory.Text))
            {
                txtScifactoryFromFactory.Focus();
                MyUtility.Msg.WarningBox("< From Factory > can't be empty!");
                return;
            }

            if (MyUtility.Check.Empty(txtScifactoryToFactory.Text))
            {
                txtScifactoryToFactory.Focus();
                MyUtility.Msg.WarningBox("< To Factory > can't be empty!");
                return;
            }

            string sqlCmd;
            if (MyUtility.Check.Seek(txtSPNo.Text.Trim(), "PO_Supp", "ID"))
            {
                sqlCmd = string.Format(@"select 1 as Selected,ps.ID as POID,ps.SEQ1,psd.SEQ2,(left(ps.SEQ1+' ',3)+'-'+isnull(psd.SEQ2,'')) as Seq,ps.SuppID,
(ps.SuppID+'-'+ isnull(s.AbbEN,'')) as Supp,psd.Refno,psd.SCIRefno,f.DescDetail as Description,
psd.FabricType, (case when psd.FabricType = 'F' then 'Fabric' when psd.FabricType = 'A' then 'Accessory' else '' end) as Type,
isnull(f.MtlTypeID,'') as MtlTypeID,psd.POUnit as UnitID,(isnull(psd.ShipQty,0)+isnull(psd.ShipFOC,0)) as Qty,0.0 as NetKg,0.0 as WeightKg,
o.BuyerDelivery,isnull(o.BrandID,'') as BrandID,isnull(o.FactoryID,'') as FactoryID,o.SciDelivery
from PO_Supp ps WITH (NOLOCK) 
left join PO_Supp_Detail psd WITH (NOLOCK) on ps.ID = psd.ID and ps.SEQ1 = psd.SEQ1
left join Supp s WITH (NOLOCK) on s.ID = ps.SuppID
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join Orders o WITH (NOLOCK) on o.ID = ps.ID
where ps.ID = '{0}'", txtSPNo.Text.Trim());
            }
            else
            {
                sqlCmd = string.Format(@"select 1 as Selected,i.InventoryPOID as POID,i.InventorySeq1 as Seq1,i.InventorySeq2 as Seq2,
(SUBSTRING(i.InventorySeq1,1,3)+'-'+InventorySeq2) as Seq,'' as SuppID,'' as Supp,
i.Refno,'' as SCIRefNo,'' as Description,(select top 1 type from Fabric WITH (NOLOCK) where Refno = i.Refno) as FabricType,
(select top 1 MtlTypeID from Fabric WITH (NOLOCK) where Refno = i.Refno) as MtlTypeID,i.UnitID,i.Qty,0.0 as NetKg,0.0 as WeightKg,
null as BuyerDelivery,'' as BrandID,i.FactoryID,null as SciDelivery
from Invtrans i WITH (NOLOCK) 
where i.InventoryPOID = '{0}'
and (i.Type = '2' or i.Type = '3')
and i.FactoryID = '{1}'
and i.TransferFactory = '{2}'", txtSPNo.Text.Trim(), txtScifactoryFromFactory.Text.Trim(), txtScifactoryToFactory.Text.Trim());
            }
            DataTable selectData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query error." + result.ToString());
                return;
            }
            if (selectData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }
            listControlBindingSource1.DataSource = selectData;
        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            listControlBindingSource1.EndEdit();
            DataTable gridData = (DataTable)listControlBindingSource1.DataSource;
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
