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
    public partial class P08_ShareExpense_ImportGarment : Sci.Win.Subs.Base
    {
        DataTable detailData, gridData;
        IList<string> comboBox1_RowSource = new List<string>();
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        int Type;
        public P08_ShareExpense_ImportGarment(DataTable DetailData,int T)
        {
            InitializeComponent();
            detailData = DetailData;
            Type = T;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (Type==0)
            {
                MyUtility.Tool.SetupCombox(comboDatafrom, 1, 1, "Garment Booking,Packing Local Order");
                comboDatafrom.SelectedIndex = -1;
            }
            else
            {
                MyUtility.Tool.SetupCombox(comboDatafrom, 1, 1, "Garment Booking,Packing FOC,Packing Local Order");
                comboDatafrom.SelectedIndex = -1;
            }

            //Grid設定
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("InvNo", header: "GB#/Packing#", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 2, width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 2, width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(comboDatafrom.SelectedValue) && MyUtility.Check.Empty(dateFCRDate.Value1) && MyUtility.Check.Empty(dateFCRDate.Value2) &&
                MyUtility.Check.Empty(txtCountryDestination.TextBox1.Text) && MyUtility.Check.Empty(txtShipmode.SelectedValue) && MyUtility.Check.Empty(txtbrand.Text) &&
                MyUtility.Check.Empty(txtSubconForwarder.TextBox1.Text) && MyUtility.Check.Empty(txtTruck.Text) && MyUtility.Check.Empty(datePulloutDate.Value1) && MyUtility.Check.Empty(datePulloutDate.Value2))
            {
                
                dateFCRDate.TextBox1.Focus();
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            #region Garment Booking
            if (MyUtility.Check.Empty(comboDatafrom.SelectedValue) || MyUtility.Convert.GetString(comboDatafrom.SelectedValue) == "Garment Booking")
            {
                sqlCmd.Append(@"
with GB as 
(
    select distinct 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
    '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
    '' as ShareBase, 0 as FtyWK 
    from GMTBooking g  WITH (NOLOCK) 
    left join GMTBooking_CTNR gc WITH (NOLOCK) on gc.ID = g.ID 
    left Join PackingList p WITH (NOLOCK) on p.INVNo = g.ID 
    where 1=1 ");

                if (!MyUtility.Check.Empty(dateFCRDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(dateFCRDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(dateFCRDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(dateFCRDate.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(txtCountryDestination.TextBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and g.Dest = '{0}' ", txtCountryDestination.TextBox1.Text));
                }

                if (!MyUtility.Check.Empty(txtShipmode.SelectedValue))
                {
                    sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}' ", MyUtility.Convert.GetString(txtShipmode.SelectedValue)));
                }

                if (!MyUtility.Check.Empty(txtbrand.Text))
                {
                    sqlCmd.Append(string.Format(" and g.BrandID = '{0}' ", txtbrand.Text));
                }

                if (!MyUtility.Check.Empty(txtSubconForwarder.TextBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and g.Forwarder = '{0}' ", txtSubconForwarder.TextBox1.Text));
                }

                if (!MyUtility.Check.Empty(txtTruck.Text))
                {
                    sqlCmd.Append(string.Format(" and gc.TruckNo = '{0}' ", txtTruck.Text));
                }

                if (!MyUtility.Check.Empty(datePulloutDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}' ", Convert.ToDateTime(datePulloutDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(datePulloutDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}' ", Convert.ToDateTime(datePulloutDate.Value2).ToString("d")));
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
            if (!MyUtility.Check.Empty(comboDatafrom.SelectedValue) && MyUtility.Convert.GetString(comboDatafrom.SelectedValue) == "Garment Booking")
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
                if (MyUtility.Check.Empty(comboDatafrom.SelectedValue))
                {
                    if (Type ==0)
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
                    if (MyUtility.Convert.GetString(comboDatafrom.SelectedValue) == "Packing FOC")
                    {
                        sqlCmd.Append(" Type = 'F' ");
                    }
                    else
                    {
                        sqlCmd.Append(" Type = 'L' ");
                    }
                }

                if (!MyUtility.Check.Empty(datePulloutDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and PulloutDate >= '{0}' ", Convert.ToDateTime(datePulloutDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(datePulloutDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and PulloutDate <= '{0}' ", Convert.ToDateTime(datePulloutDate.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(txtShipmode.SelectedValue))
                {
                    sqlCmd.Append(string.Format(" and ShipModeID = '{0}' ", MyUtility.Convert.GetString(txtShipmode.SelectedValue)));
                }

                if (!MyUtility.Check.Empty(txtbrand.Text))
                {
                    sqlCmd.Append(string.Format(" and BrandID = '{0}' ", txtbrand.Text));
                }
                sqlCmd.Append(") ");
            }
            
            #endregion

            sqlCmd.Append(@"select * from GB 
union all 
select * from PL");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            listControlBindingSource1.EndEdit();
            gridData = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(gridData)) return;
            if (gridData.Rows.Count > 0)
            {
                DataRow[] dr = gridData.Select("Selected = 1");
                if (dr.Length > 0)
                {
                    foreach (DataRow currentRow in dr)
                    {
                        DataRow[] findrow = detailData.Select(string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(currentRow["BLNo"]), MyUtility.Convert.GetString(currentRow["WKNo"]), MyUtility.Convert.GetString(currentRow["InvNo"])));
                        if (findrow.Length == 0)
                        {
                            currentRow.AcceptChanges();
                            currentRow.SetAdded();
                            detailData.ImportRow(currentRow);
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
