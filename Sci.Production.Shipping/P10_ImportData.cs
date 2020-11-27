using System;
using System.Data;
using System.Text;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P10_ImportData
    /// </summary>
    public partial class P10_ImportData : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable plData;
        private DataTable gbData;
        private DataTable detailData;
        private DataTable detail2Data;
        private DataRow masterData;

        /// <summary>
        /// P10_ImportData
        /// </summary>
        /// <param name="masterData">masterData</param>
        /// <param name="detailData">detailData</param>
        /// <param name="detail2Data">detail2Data</param>
        public P10_ImportData(DataRow masterData, DataTable detailData, DataTable detail2Data)
        {
            this.InitializeComponent();
            this.txtshipmode.SelectedValue = string.Empty;
            this.masterData = masterData;
            this.detailData = detailData;
            this.detail2Data = detail2Data;
            MyUtility.Tool.SetupCombox(this.comboContainerType, 1, 1, ",CY-CY,CFS-CY,CFS-CFS");
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("ID", header: "GB#", width: Widths.AnsiChars(25), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("Forwarder", header: "Forwarder", width: Widths.AnsiChars(17), iseditingreadonly: true)
                .DateTime("CutOffdate", header: "Cut-off Date/Time", iseditingreadonly: true);
            this.grid1.SelectionChanged += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(this.grid1.GetSelectedRowIndex());
                if (dr != null)
                {
                    string filter = string.Format("InvNo = '{0}'", MyUtility.Convert.GetString(dr["ID"]));
                    this.plData.DefaultView.RowFilter = filter;
                }
            };

            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.IsEditingReadOnly = true;
            this.Helper.Controls.Grid.Generator(this.grid2)
                .Text("ID", header: "Packing No.", width: Widths.AnsiChars(13))
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13))
                .Date("BuyerDelivery", header: "Delivery")
                .Text("IDD", header: "Intended Delivery", width: Widths.AnsiChars(15), iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            #region 組SQL
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> listPar = new List<SqlParameter>();

            sqlCmd.Append(@"
SELECT DISTINCT [Selected] = 1, 
                g.id, 
                g.brandid, 
                g.shipmodeid, 
                [Forwarder] = ( g.forwarder + ' - ' + ls.abb ), 
                g.cutoffdate, 
                g.cycfs, 
                g.sono, 
                g.forwarderwhse_detailukey, 
                [WhseNo] = Isnull((SELECT whseno 
                        FROM   forwarderwhse_detail WITH (nolock) 
                        WHERE  ukey = g.forwarderwhse_detailukey), ''), 
                [Status] = Iif(
					g.status = 'Confirmed', 'GB Confirmed', 
					Iif(g.socfmdate IS NULL, '', 'S/O Confirmed')), 
                g.totalctnqty, 
                g.totalshipqty, 
                g.totalcbm, 
                [ClogCTNQty] = (SELECT Isnull(Sum(pd.ctnqty), 0) 
                 FROM   packinglist p WITH (nolock), 
                        packinglist_detail pd WITH (nolock) 
                 WHERE  p.invno = g.id 
                        AND p.id = pd.id 
                        AND pd.receivedate IS NOT NULL)
FROM   gmtbooking g WITH (nolock), 
       order_qtyship oq WITH (nolock), 
       packinglist p WITH (nolock), 
       packinglist_detail pd WITH (nolock), 
       localsupp ls WITH (nolock),
	   orders o WITH (nolock)
WHERE  g.shipplanid = '' 
       AND g.socfmdate IS NOT NULL 
       AND g.id = p.invno 
       AND p.id = pd.id 
       AND pd.orderid = oq.id 
       AND pd.ordershipmodeseq = oq.seq 
       AND g.forwarder = ls.id 
	   AND o.ID = oq.Id 
       AND o.junk = 0 
       AND o.GMTComplete != 'S'
");
            if (!MyUtility.Check.Empty(this.txtGBNoStart.Text))
            {
                sqlCmd.Append(string.Format(" and g.id >= '{0}'", this.txtGBNoStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtGBNoEnd.Text))
            {
                sqlCmd.Append(string.Format(" and g.id <= '{0}'", this.txtGBNoEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateCutoffDate.Value))
            {
                // 20161126 撈取DateBox1用法怪怪的
                // sqlCmd.Append(string.Format(" and g.CutOffDate >= '{0}' and g.CutOffDate < '{1}'", Convert.ToDateTime(dateRange1.Value1).ToString("d"), (Convert.ToDateTime(dateRange1.Value1).AddDays(1)).ToString("d")));
                sqlCmd.Append(string.Format(" and g.CutOffDate >= '{0}' and g.CutOffDate < '{1}'", Convert.ToDateTime(this.dateCutoffDate.Value).ToString("d"), Convert.ToDateTime(this.dateCutoffDate.Value).AddDays(1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.txtshipmode.SelectedValue))
            {
                sqlCmd.Append(string.Format(" and g.ShipModeID = '{0}'", MyUtility.Convert.GetString(this.txtshipmode.SelectedValue)));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.txtbrand.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSubconForwarder.TextBox1.Text))
            {
                sqlCmd.Append(string.Format(" and g.Forwarder = '{0}'", this.txtSubconForwarder.TextBox1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID = '{0}'", this.txtSPNo.Text));
            }

            if (!MyUtility.Check.Empty(this.comboContainerType.Text))
            {
                sqlCmd.Append(string.Format(" and g.CYCFS = '{0}'", this.comboContainerType.Text));
            }

            if (!MyUtility.Check.Empty(this.dateIDD.Value))
            {
                sqlCmd.Append($@"AND  oq.IDD = @IDD ");
                listPar.Add(new SqlParameter("@IDD", this.dateIDD.Value));
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), listPar, out this.gbData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query GB error:" + result.ToString());
                return;
            }

            StringBuilder allID = new StringBuilder();
            foreach (DataRow dr in this.gbData.Rows)
            {
                allID.Append(string.Format("'{0}',", MyUtility.Convert.GetString(dr["ID"])));
            }

            sqlCmd.Clear();
            if (allID.Length > 0)
            {
                sqlCmd.Append(string.Format(
                    @"
select distinct pd.ID, pd.OrderID,oq.BuyerDelivery,p.INVNo, oq.IDD
from PackingList p WITH (NOLOCK) 
INNER JOIN PackingList_Detail pd WITH (NOLOCK) ON p.id = pd.ID
INNER JOIN Order_QtyShip oq WITH (NOLOCK) ON  pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
where p.INVNo in ({0})
", allID.ToString().Substring(0, allID.Length - 1)));
                result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.plData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Query PL error:" + result.ToString());
                }
            }

            this.listControlBindingSource1.DataSource = this.gbData;
            this.listControlBindingSource2.DataSource = this.plData;
            if (this.gbData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }
        }

        // Import Data
        private void BtnImportData_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            if (MyUtility.Check.Empty(this.masterData["Remark"]))
            {
                this.masterData["Remark"] = string.Format(
                    @"GB#:{0}~{1}, Buyer Delivery:{2}~{3}, Cut-off Date:{4}, Ship Mode:{5}, Brand:{6}, Forwarder:{7}, SP#:{8}",
                    MyUtility.Check.Empty(this.txtGBNoStart.Text) ? " " : this.txtGBNoStart.Text,
                    MyUtility.Check.Empty(this.txtGBNoEnd.Text) ? " " : this.txtGBNoEnd.Text,
                    MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) ? " " : Convert.ToDateTime(this.dateBuyerDelivery.Value1).ToString("d"),
                    MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) ? " " : Convert.ToDateTime(this.dateBuyerDelivery.Value2).ToString("d"),
                    MyUtility.Check.Empty(this.dateCutoffDate.Value) ? " " : Convert.ToDateTime(this.dateCutoffDate.Value).ToString("d"),
                    MyUtility.Check.Empty(this.txtshipmode.SelectedValue) ? " " : this.txtshipmode.SelectedValue.ToString(),
                    MyUtility.Check.Empty(this.txtbrand.Text) ? " " : this.txtbrand.Text,
                    MyUtility.Check.Empty(this.txtSubconForwarder.TextBox1.Text) ? " " : this.txtSubconForwarder.TextBox1.Text,
                    MyUtility.Check.Empty(this.txtSPNo.Text) ? " " : this.txtSPNo.Text);
            }

            StringBuilder allID = new StringBuilder();
            if (MyUtility.Check.Empty(this.gbData))
            {
                return;
            }

            DataRow[] dra = this.gbData.Select("Selected = 1");

            if (dra.Length > 0)
            {
                foreach (DataRow dr in dra)
                {
                    DataRow[] findrow = this.detailData.Select(string.Format("ID = '{0}'", MyUtility.Convert.GetString(dr["ID"])));
                    if (findrow.Length == 0)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        this.detailData.ImportRow(dr);
                    }

                    allID.Append(string.Format("'{0}',", MyUtility.Convert.GetString(dr["ID"])));
                }
            }

            if (allID.Length > 0)
            {
                string sqlCmd = string.Format(
                    @"
select    p.ID
		, [OrderID]=iif(p.OrderID='',(select cast(a.OrderID as nvarchar) +',' from (select distinct OrderID from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.id) a for xml path('')),p.OrderID) 
		, [BuyerDelivery]=iif(p.type = 'B',(select BuyerDelivery from Order_QtyShip WITH (NOLOCK) where ID = p.OrderID and Seq = p.OrderShipmodeSeq),(select oq.BuyerDelivery from (select top 1 OrderID, OrderShipmodeSeq from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID) a, Order_QtyShip oq WITH (NOLOCK) where a.OrderID = oq.Id and a.OrderShipmodeSeq = oq.Seq)) 
		, p.Status
		, p.CTNQty
		, p.CBM
		, [ClogCTNQty]=(select sum(CTNQty) from PackingList_Detail pd WITH (NOLOCK) where pd.ID = p.ID and pd.ReceiveDate is not null) 
		, p.InspDate
		, p.InspStatus
		, [PulloutDate]=IIF(e.ShipDate IS NOT NULL,e.ShipDate, p.PulloutDate)
		, p.InvNo
		, p.MDivisionID
		, p.ShipQty
from PackingList p WITH (NOLOCK) 
LEFT JOIN Express e WITH (NOLOCK) ON e.ID=p.ExpressID
where p.InvNo in ({0})", allID.ToString().Substring(0, allID.Length - 1));
                DataTable packData;
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out packData);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Import query packinglist error:" + result.ToString());
                    return;
                }

                foreach (DataRow dr in packData.Rows)
                {
                    DataRow[] findrow = this.detail2Data.Select(string.Format("ID = '{0}'", MyUtility.Convert.GetString(dr["ID"])));
                    if (findrow.Length == 0)
                    {
                        dr.AcceptChanges();
                        dr.SetAdded();
                        this.detail2Data.ImportRow(dr);
                    }
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
