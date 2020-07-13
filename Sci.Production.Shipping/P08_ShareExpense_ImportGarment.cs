using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P08_ShareExpense_ImportGarment
    /// </summary>
    public partial class P08_ShareExpense_ImportGarment : Win.Subs.Base
    {
        private DataTable detailData;
        private DataTable gridData;
        private IList<string> comboBox1_RowSource = new List<string>();
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private int Type;

        /// <summary>
        /// P08_ShareExpense_ImportGarment
        /// </summary>
        /// <param name="detailData">detailData</param>
        /// <param name="t">t</param>
        public P08_ShareExpense_ImportGarment(DataTable detailData, int t)
        {
            this.InitializeComponent();
            this.detailData = detailData;
            this.Type = t;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.Type == 0)
            {
                MyUtility.Tool.SetupCombox(this.comboDatafrom, 1, 1, "Garment Booking,Packing Local Order");
                this.comboDatafrom.SelectedIndex = -1;
            }
            else
            {
                MyUtility.Tool.SetupCombox(this.comboDatafrom, 1, 1, "Garment Booking,Packing FOC,Packing Local Order");
                this.comboDatafrom.SelectedIndex = -1;
            }

            // Grid設定
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("InvNo", header: "GB#/Packing#", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 3, width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 4, width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.comboDatafrom.SelectedValue) && MyUtility.Check.Empty(this.dateFCRDate.Value1) && MyUtility.Check.Empty(this.dateFCRDate.Value2) &&
                MyUtility.Check.Empty(this.txtCountryDestination.TextBox1.Text) && MyUtility.Check.Empty(this.txtShipmode.SelectedValue) && MyUtility.Check.Empty(this.txtbrand.Text) &&
                MyUtility.Check.Empty(this.txtSubconForwarder.TextBox1.Text) && MyUtility.Check.Empty(this.txtTruck.Text) && MyUtility.Check.Empty(this.datePulloutDate.Value1) && MyUtility.Check.Empty(this.datePulloutDate.Value2))
            {
                this.dateFCRDate.TextBox1.Focus();
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            #region Garment Booking
            if (MyUtility.Check.Empty(this.comboDatafrom.SelectedValue) || MyUtility.Convert.GetString(this.comboDatafrom.SelectedValue) == "Garment Booking")
            {
                sqlCmd.Append(@"
with GB as 
(
    select distinct 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
    '' as ShippingAPID, g.BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
    '' as ShareBase, 0 as FtyWK 
    from GMTBooking g  WITH (NOLOCK) 
    left join GMTBooking_CTNR gc WITH (NOLOCK) on gc.ID = g.ID 
    left Join PackingList p WITH (NOLOCK) on p.INVNo = g.ID 
    where 1=1 ");

                if (!MyUtility.Check.Empty(this.dateFCRDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(this.dateFCRDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateFCRDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(this.dateFCRDate.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.txtCountryDestination.TextBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and g.Dest = '{0}' ", this.txtCountryDestination.TextBox1.Text));
                }

                if (!MyUtility.Check.Empty(this.txtShipmode.SelectedValue))
                {
                    sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}' ", MyUtility.Convert.GetString(this.txtShipmode.SelectedValue)));
                }

                if (!MyUtility.Check.Empty(this.txtbrand.Text))
                {
                    sqlCmd.Append(string.Format(" and g.BrandID = '{0}' ", this.txtbrand.Text));
                }

                if (!MyUtility.Check.Empty(this.txtSubconForwarder.TextBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and g.Forwarder = '{0}' ", this.txtSubconForwarder.TextBox1.Text));
                }

                if (!MyUtility.Check.Empty(this.txtTruck.Text))
                {
                    sqlCmd.Append(string.Format(" and gc.TruckNo = '{0}' ", this.txtTruck.Text));
                }

                if (!MyUtility.Check.Empty(this.datePulloutDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.datePulloutDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value2).ToString("d")));
                }

                sqlCmd.Append("), ");
            }
            else
            {
                sqlCmd.Append(@"
with GB as 
(
    select 0 as Selected,'' as InvNo,'' as ShipModeID,0 as GW, 0 as CBM, 
    '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount, 
    '' as ShareBase, 0 as FtyWK 
    from GMTBooking WITH (NOLOCK) where 1=0 
), ");
            }
            #endregion

            #region PackingList
            if (!MyUtility.Check.Empty(this.comboDatafrom.SelectedValue) && MyUtility.Convert.GetString(this.comboDatafrom.SelectedValue) == "Garment Booking")
            {
                sqlCmd.Append(@"
PL as 
(
    select 0 as Selected,'' as InvNo,'' as ShipModeID,0 as GW, 0 as CBM, 
    '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount, 
    '' as ShareBase, 0 as FtyWK 
    from PackingList WITH (NOLOCK) where 1=0 
) ");
            }
            else
            {
                sqlCmd.Append(@"
PL as 
(
    select distinct 0 as Selected,ID as InvNo,ShipModeID,GW,CBM, 
    '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount, 
    '' as ShareBase, 0 as FtyWK 
    from PackingList WITH (NOLOCK) 
where ");
                if (MyUtility.Check.Empty(this.comboDatafrom.SelectedValue))
                {
                    if (this.Type == 0)
                    {
                        sqlCmd.Append(" Type = 'L' ");
                    }
                    else
                    {
                        sqlCmd.Append(" (Type = 'F' or Type = 'L') ");
                    }
                }
                else
                {
                    if (MyUtility.Convert.GetString(this.comboDatafrom.SelectedValue) == "Packing FOC")
                    {
                        sqlCmd.Append(" Type = 'F' ");
                    }
                    else
                    {
                        sqlCmd.Append(" Type = 'L' ");
                    }
                }

                if (!MyUtility.Check.Empty(this.datePulloutDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and PulloutDate >= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.datePulloutDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and PulloutDate <= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.txtShipmode.SelectedValue))
                {
                    sqlCmd.Append(string.Format(" and ShipModeID = '{0}' ", MyUtility.Convert.GetString(this.txtShipmode.SelectedValue)));
                }

                if (!MyUtility.Check.Empty(this.txtbrand.Text))
                {
                    sqlCmd.Append(string.Format(" and BrandID = '{0}' ", this.txtbrand.Text));
                }

                sqlCmd.Append(") ");
            }

            #endregion

            sqlCmd.Append(@"select * from GB 
union all 
select * from PL");
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
            if (MyUtility.Check.Empty(this.gridData))
            {
                return;
            }

            if (this.gridData.Rows.Count > 0)
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
