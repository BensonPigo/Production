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
    /// ISP20220497: 1.移除Date From. 2. Export 增加On Board Date 篩選. 3. 排除Packing Local Order資料 
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

            // Type = 0 then Export else 1
            this.Type = t;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            if (this.Type == 1)
            {
                this.dateOnBoard.Visible = false;
                this.labOnBoardDate.Visible = false;
            }
            else
            {
                this.dateOnBoard.Visible = true;
                this.labOnBoardDate.Visible = true;
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
            if (MyUtility.Check.Empty(this.dateFCRDate.Value1) &&
                MyUtility.Check.Empty(this.dateFCRDate.Value2) &&
                MyUtility.Check.Empty(this.txtCountryDestination.TextBox1.Text) &&
                MyUtility.Check.Empty(this.txtShipmode.SelectedValue) &&
                MyUtility.Check.Empty(this.txtbrand.Text) &&
                MyUtility.Check.Empty(this.txtSubconForwarder.TextBox1.Text) &&
                MyUtility.Check.Empty(this.txtTruck.Text) &&
                MyUtility.Check.Empty(this.datePulloutDate.Value1) &&
                MyUtility.Check.Empty(this.datePulloutDate.Value2) &&
                MyUtility.Check.Empty(this.dateOnBoard.Value1) &&
                MyUtility.Check.Empty(this.dateOnBoard.Value2))
            {
                this.dateFCRDate.TextBox1.Focus();
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL

            sqlCmd.Append(@"
with GB as 
(
    select distinct 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
    '' as ShippingAPID, g.BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
    '' as ShareBase, 0 as FtyWK 
    from GMTBooking g  WITH (NOLOCK) 
    left join GMTBooking_CTNR gc WITH (NOLOCK) on gc.ID = g.ID 
    left Join PackingList p WITH (NOLOCK) on p.INVNo = g.ID 
    where 1=1 
    AND p.Type != 'L' -- 排除Packing Local Order by ISP20220497
");

            if (!MyUtility.Check.Empty(this.dateFCRDate.Value1))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(this.dateFCRDate.Value1).ToString("yyyy/MM/dd")));
            }

            if (this.Type == 0)
            {
                if (!MyUtility.Check.Empty(this.dateOnBoard.Value1))
                {
                    sqlCmd.Append(string.Format(" and g.ETD >= '{0}' ", Convert.ToDateTime(this.dateOnBoard.Value1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.dateOnBoard.Value2))
                {
                    sqlCmd.Append(string.Format(" and g.ETD <= '{0}' ", Convert.ToDateTime(this.dateOnBoard.Value2).ToString("yyyy/MM/dd")));
                }
                // A2B Data
                sqlCmd.Append(@"
union
    select distinct 0 as Selected,g.ID as InvNo,g.ShipModeID,g.TotalGW as GW, g.TotalCBM as CBM,
    '' as ShippingAPID, g.BLNo, '' as WKNo, '' as Type, '' as CurrencyID, 0 as Amount,
    '' as ShareBase, 0 as FtyWK 
    from GMTBooking g  WITH (NOLOCK) 
    left join GMTBooking_CTNR gc WITH (NOLOCK) on gc.ID = g.ID 
    inner join GMTBooking_Detail p WITH (NOLOCK) on p.ID = g.ID 
    where 1=1 ");

                if (!MyUtility.Check.Empty(this.dateFCRDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}' ", Convert.ToDateTime(this.dateFCRDate.Value1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.dateFCRDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(this.dateFCRDate.Value2).ToString("yyyy/MM/dd")));
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
                    sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value1).ToString("yyyy/MM/dd")));
                }

                if (!MyUtility.Check.Empty(this.datePulloutDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value2).ToString("yyyy/MM/dd")));
                }

                sqlCmd.Append("), ");
            }

            if (!MyUtility.Check.Empty(this.dateFCRDate.Value2))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}' ", Convert.ToDateTime(this.dateFCRDate.Value2).ToString("yyyy/MM/dd")));
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
                sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.datePulloutDate.Value2))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}' ", Convert.ToDateTime(this.datePulloutDate.Value2).ToString("yyyy/MM/dd")));
            }

            sqlCmd.Append(") ");

            sqlCmd.Append(@"
select * from GB 
");
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
