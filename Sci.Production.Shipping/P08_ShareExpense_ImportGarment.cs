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
        BindingSource comboxbs1;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        public P08_ShareExpense_ImportGarment(DataTable DetailData)
        {
            InitializeComponent();
            detailData = DetailData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            comboBox1_RowSource.Add("Garment Booking");
            comboBox1_RowSource.Add("Packing FOC");
            comboBox1_RowSource.Add("Packing Local Order");
            comboxbs1 = new BindingSource(comboBox1_RowSource, null);
            comboBox1.DataSource = comboxbs1;
            comboBox1.SelectedIndex = -1;

            //Grid設定
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("InvNo", header: "GB#/Packing#", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 2, width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 2, width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(comboBox1.SelectedValue) && MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) &&
                MyUtility.Check.Empty(txtcountry1.TextBox1.Text) && MyUtility.Check.Empty(txtshipmode1.SelectedValue) && MyUtility.Check.Empty(txtbrand1.Text) &&
                MyUtility.Check.Empty(txtsubcon1.TextBox1.Text) && MyUtility.Check.Empty(textBox1.Text) && MyUtility.Check.Empty(dateRange2.Value1) && MyUtility.Check.Empty(dateRange2.Value2))
            {
                MyUtility.Msg.WarningBox("< FCR Date > or < Pullout Date > or < Destination > or < Ship Mode > or < Data from > or < Brand > or < Forwarder > or < Truck# > can not be empty!");
                dateRange1.TextBox1.Focus();
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            #region Garment Booking
            if (MyUtility.Check.Empty(comboBox1.SelectedValue) || comboBox1.SelectedValue.ToString() == "Garment Booking")
            {
                sqlCmd.Append(@"with GB 
as 
(select distinct 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
 '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
 '' as ShareBase, 0 as FtyWK 
 from GMTBooking g 
 left join GMTBooking_CTNR gc on gc.ID = g.ID 
 left Join PackingList p on p.INVNo = g.ID 
 where 1=1 ");

                if (!MyUtility.Check.Empty(dateRange1.Value1))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(dateRange1.Value2))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(txtcountry1.TextBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and g.Dest = '{0}' ", txtcountry1.TextBox1.Text));
                }

                if (!MyUtility.Check.Empty(txtshipmode1.SelectedValue))
                {
                    sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}' ", txtshipmode1.SelectedValue.ToString()));
                }

                if (!MyUtility.Check.Empty(txtbrand1.Text))
                {
                    sqlCmd.Append(string.Format(" and g.BrandID = '{0}' ", txtbrand1.Text));
                }

                if (!MyUtility.Check.Empty(txtsubcon1.TextBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and g.Forwarder = '{0}' ", txtsubcon1.TextBox1.Text));
                }

                if (!MyUtility.Check.Empty(textBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and gc.TruckNo = '{0}' ", textBox1.Text));
                }

                if (!MyUtility.Check.Empty(dateRange2.Value1))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}' ", Convert.ToDateTime(dateRange2.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(dateRange2.Value2))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}' ", Convert.ToDateTime(dateRange2.Value2).ToString("d")));
                }
                sqlCmd.Append("), ");
            }
            else
            {
                sqlCmd.Append(@"with GB 
as 
(select 0 as Selected,'' as InvNo,'' as ShipModeID,0 as GW, 0 as CBM, 
 '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount, 
 '' as ShareBase, 0 as FtyWK 
 from GMTBooking where 1=0 
), ");
            }
            #endregion

            #region PackingList
            if (!MyUtility.Check.Empty(comboBox1.SelectedValue) && comboBox1.SelectedValue.ToString() == "Garment Booking")
            {
                sqlCmd.Append(@"PL 
as 
(select 0 as Selected,'' as InvNo,'' as ShipModeID,0 as GW, 0 as CBM, 
 '' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount, 
 '' as ShareBase, 0 as FtyWK 
 from PackingList where 1=0 
) ");
            }
            else
            {
                sqlCmd.Append(@"PL 
as 
(select distinct 0 as Selected,ID as InvNo,ShipModeID,GW,CBM, 
'' as ShippingAPID, '' as BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount, 
'' as ShareBase, 0 as FtyWK 
 from PackingList 
 where ");
                if (MyUtility.Check.Empty(comboBox1.SelectedValue))
                {
                    sqlCmd.Append(" (Type = 'F' or Type = 'L') ");
                }
                else
                {
                    if (comboBox1.SelectedValue.ToString() == "Packing FOC")
                    {
                        sqlCmd.Append(" Type = 'F' ");
                    }
                    else
                    {
                        sqlCmd.Append(" Type = 'L' ");
                    }
                }

                if (!MyUtility.Check.Empty(dateRange2.Value1))
                {
                    sqlCmd.Append(string.Format(" and PulloutDate >= '{0}' ", Convert.ToDateTime(dateRange2.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(dateRange2.Value2))
                {
                    sqlCmd.Append(string.Format(" and PulloutDate <= '{0}' ", Convert.ToDateTime(dateRange2.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(txtshipmode1.SelectedValue))
                {
                    sqlCmd.Append(string.Format(" and ShipModeID = '{0}' ", txtshipmode1.SelectedValue.ToString()));
                }

                if (!MyUtility.Check.Empty(txtbrand1.Text))
                {
                    sqlCmd.Append(string.Format(" and BrandID = '{0}' ", txtbrand1.Text));
                }
                sqlCmd.Append(") ");
            }
            
            #endregion

            sqlCmd.Append(@"select * from GB 
union all 
select * from PL");
            #endregion

            DualResult result = DBProxy.Current.Select(null,Convert.ToString(sqlCmd),out gridData);
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
        private void button2_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            gridData = (DataTable)listControlBindingSource1.DataSource;
            if (gridData.Rows.Count > 0)
            {
                DataRow[] dr = gridData.Select("Selected = 1");
                if (dr.Length > 0)
                {
                    foreach (DataRow currentRow in dr)
                    {
                        DataRow[] findrow = detailData.Select(string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", currentRow["BLNo"].ToString(), currentRow["WKNo"].ToString(), currentRow["InvNo"].ToString()));
                        if (findrow.Length == 0)
                        {
                            currentRow.AcceptChanges();
                            currentRow.SetAdded();
                            detailData.ImportRow(currentRow);
                        }
                        else
                        {
                            findrow[0]["GW"] = Convert.ToDecimal(currentRow["GW"]);
                            findrow[0]["CBM"] = Convert.ToDecimal(currentRow["CBM"]);
                            findrow[0]["ShipModeID"] = currentRow["ShipModeID"].ToString();
                        }
                    }
                }
            }
            MyUtility.Msg.InfoBox("Import finished!");
        }
    }
}
