using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P08_ShareExpense_ImportMaterial
    /// </summary>
    public partial class P08_ShareExpense_ImportMaterial : Win.Subs.Base
    {
        private DataTable detailData;
        private DataTable gridData;
        private DataRow apData;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// P08_ShareExpense_ImportMaterial
        /// </summary>
        /// <param name="detailData">detailData</param>
        /// <param name="aPDate">aPDate</param>
        public P08_ShareExpense_ImportMaterial(DataTable detailData, DataRow aPDate)
        {
            this.InitializeComponent();
            this.detailData = detailData;
            this.apData = aPDate;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("BLNo", header: "B/L No.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("WKNo", header: "WK#/Fty WK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 3, width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 4, width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateArrivePortDate.Value1) && MyUtility.Check.Empty(this.dateArrivePortDate.Value2) && MyUtility.Check.Empty(this.txtInvoiceNo.Text) &&
                MyUtility.Check.Empty(this.txtBLNo.Text) && MyUtility.Check.Empty(this.txtWKNo.Text))
            {
                this.dateArrivePortDate.TextBox1.Focus();
                MyUtility.Msg.WarningBox("< Arrive Port Date > or < Invoice No. > or < B/L No. > or < WK No. > can not be empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            if (MyUtility.Convert.GetString(this.apData["Type"]) == "EXPORT")
            {
                #region FtyExport (Type = 3)
                sqlCmd.Append(@"select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, ID as InvNo, '' as ShippingAPID, 
'' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
from FtyExport WITH (NOLOCK) 
where Type = 3 ");
                if (!MyUtility.Check.Empty(this.dateArrivePortDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and PortArrival >= '{0}' ", Convert.ToDateTime(this.dateArrivePortDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateArrivePortDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and PortArrival <= '{0}' ", Convert.ToDateTime(this.dateArrivePortDate.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.txtInvoiceNo.Text))
                {
                    sqlCmd.Append(string.Format(" and INVNo = '{0}' ", this.txtInvoiceNo.Text));
                }

                if (!MyUtility.Check.Empty(this.txtBLNo.Text))
                {
                    sqlCmd.Append(string.Format(" and BLNo = '{0}' ", this.txtBLNo.Text));
                }

                if (!MyUtility.Check.Empty(this.txtWKNo.Text))
                {
                    sqlCmd.Append(string.Format(" and ID = '{0}' ", this.txtWKNo.Text));
                }
                #endregion
            }
            else
            {
                #region Export, FtyExport(Type != 3)
                if (MyUtility.Convert.GetString(this.apData["SubType"]) == "MATERIAL" || MyUtility.Convert.GetString(this.apData["SubType"]).ToUpper() == "OTHER")
                {
                    sqlCmd.Append(@"with ExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK
 from Export WITH (NOLOCK) 
 where 1 = 1 ");
                    if (!MyUtility.Check.Empty(this.dateArrivePortDate.Value1))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival >= '{0}' ", Convert.ToDateTime(this.dateArrivePortDate.Value1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.dateArrivePortDate.Value2))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival <= '{0}' ", Convert.ToDateTime(this.dateArrivePortDate.Value2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.txtInvoiceNo.Text))
                    {
                        sqlCmd.Append(string.Format(" and INVNo = '{0}' ", this.txtInvoiceNo.Text));
                    }

                    if (!MyUtility.Check.Empty(this.txtBLNo.Text))
                    {
                        sqlCmd.Append(string.Format(" and BLNo = '{0}' ", this.txtBLNo.Text));
                    }

                    if (!MyUtility.Check.Empty(this.txtWKNo.Text))
                    {
                        sqlCmd.Append(string.Format(" and ID = '{0}' ", this.txtWKNo.Text));
                    }

                    sqlCmd.Append("), ");
                }
                else
                {
                    sqlCmd.Append(@"with ExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK
 from Export WITH (NOLOCK) where 1 = 0), ");
                }

                if (MyUtility.Convert.GetString(this.apData["SubType"]) == "SISTER FACTORY TRANSFER" || MyUtility.Convert.GetString(this.apData["SubType"]).ToUpper() == "OTHER")
                {
                    sqlCmd.Append(@"FtyExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, ID as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
 from FtyExport WITH (NOLOCK) 
 where Type <> 3 ");
                    if (!MyUtility.Check.Empty(this.dateArrivePortDate.Value1))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival >= '{0}' ", Convert.ToDateTime(this.dateArrivePortDate.Value1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.dateArrivePortDate.Value2))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival <= '{0}' ", Convert.ToDateTime(this.dateArrivePortDate.Value2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(this.txtInvoiceNo.Text))
                    {
                        sqlCmd.Append(string.Format(" and INVNo = '{0}' ", this.txtInvoiceNo.Text));
                    }

                    if (!MyUtility.Check.Empty(this.txtBLNo.Text))
                    {
                        sqlCmd.Append(string.Format(" and BLNo = '{0}' ", this.txtBLNo.Text));
                    }

                    if (!MyUtility.Check.Empty(this.txtWKNo.Text))
                    {
                        sqlCmd.Append(string.Format(" and ID = '{0}' ", this.txtWKNo.Text));
                    }

                    sqlCmd.Append(") ");
                }
                else
                {
                    sqlCmd.Append(@"FtyExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
 from FtyExport WITH (NOLOCK) 
 where 1=0 ) ");
                }

                sqlCmd.Append(@"select * from ExportData 
union all 
select * from FtyExportData");
                #endregion
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            this.gridData = (DataTable)this.listControlBindingSource1.DataSource;

            if (!MyUtility.Check.Empty(this.gridData) && this.gridData.Rows.Count > 0)
            {
                DataRow[] dr = this.gridData.Select("Selected = 1");
                if (dr.Length > 0)
                {
                    foreach (DataRow currentRow in dr)
                    {
                        DataRow[] findrow = this.detailData.Select(string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(currentRow["BLNo"]), MyUtility.Convert.GetString(currentRow["WKNo"]), MyUtility.Convert.GetString(currentRow["InvNo"])));
                        if (findrow.Length == 0)
                        {
                            currentRow.AcceptChanges();
                            currentRow.SetAdded();
                            this.detailData.ImportRow(currentRow);
                        }
                        else
                        {
                            findrow[0]["GW"] = MyUtility.Convert.GetDecimal(currentRow["GW"]);
                            findrow[0]["CBM"] = MyUtility.Convert.GetDecimal(currentRow["CBM"]);
                            findrow[0]["ShipModeID"] = MyUtility.Convert.GetString(currentRow["ShipModeID"]);
                        }
                    }
                }
            }

            MyUtility.Msg.InfoBox("Import finished!");
        }
    }
}
