﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using System.Transactions;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P14
    /// </summary>
    public partial class P14 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Grid Setting
            this.Helper.Controls.Grid.Generator(this.grid)
                .Date("ScanDate", header: "Scan Date", iseditingreadonly: false)
                .Text("PackID", header: "Pack ID", iseditingreadonly: false)
                .Text("CTN", header: "CTN#", iseditingreadonly: false)
                .Numeric("Qty", header: "Qty", iseditingreadonly: false)
                .Text("SP", header: "SP#", iseditingreadonly: false)
                .Text("PO", header: "PO#", iseditingreadonly: false)
                .Text("Style", header: "Style#", iseditingreadonly: false)
                .Text("Brand", header: "Brand", iseditingreadonly: false)
                .Text("Destination", header: "Destination", iseditingreadonly: false)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: false)
                .Date("SCIDelivery", header: "SCI Delivery", iseditingreadonly: false)
                .Text("Barcode", header: "Barcode", iseditingreadonly: false)
                .Text("ScanBy", header: "Scan By", iseditingreadonly: false)
                .Text("ScanTime", header: "Scan Time", iseditingreadonly: false);
            #endregion
        }

        private void ButtonFindNow_Click(object sender, EventArgs e)
        {
            this.bindingSource.DataSource = null;

            if (MyUtility.Check.Empty(this.dateScanDate.TextBox1.Value) &&
                MyUtility.Check.Empty(this.dateScanDate.TextBox2.Value) &&
                MyUtility.Check.Empty(this.txtPackID.Text) &&
                MyUtility.Check.Empty(this.txtSP.Text))
            {
                MyUtility.Msg.WarningBox("Conditions cannot be all empty!!");
                return;
            }

            this.FindNow();
        }

        private void FindNow()
        {
            #region SQL Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@PackID", this.txtPackID.Text));
            listSqlParameter.Add(new SqlParameter("@SP", this.txtSP.Text));
            listSqlParameter.Add(new SqlParameter("@ScanDate_S", this.dateScanDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateScanDate.Value1).ToString("yyyy/MM/dd 00:00:00")));
            listSqlParameter.Add(new SqlParameter("@ScanDate_E", this.dateScanDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateScanDate.Value2).ToString("yyyy/MM/dd 23:59:59")));
            #endregion

            #region BuyerDelivery Filte
            string strWhere = string.Empty;
            if (!this.dateScanDate.Value1.Empty() && !this.dateScanDate.Value2.Empty())
            {
                strWhere = " and c.HaulingDate between @ScanDate_S and @ScanDate_E";
            }
            else if (!this.dateScanDate.Value1.Empty() && this.dateScanDate.Value2.Empty())
            {
                strWhere = " and @ScanDate_S <= c.HaulingDate";
            }
            else if (this.dateScanDate.Value1.Empty() && !this.dateScanDate.Value2.Empty())
            {
                strWhere = " and c.HaulingDate <= @ScanDate_E";
            }

            if (!string.IsNullOrEmpty(this.txtPackID.Text))
            {
                strWhere += " and c.PackingListID = @PackID";
            }

            if (!string.IsNullOrEmpty(this.txtSP.Text))
            {
                strWhere += " and c.OrderID = @SP";
            }

            #endregion

            #region SQL Command
            string strSqlCmd = $@"
select [ScanDate] = c.HaulingDate
	,[PackID] = c.PackingListID
	,[CTN] = c.CTNStartNo
	,[Qty] = pd_QtyPerCTN.Qty
	,[SP] = c.OrderID
	,[PO] = o.CustPONo
	,[Style] = o.StyleID
	,[Brand] = o.BrandID
	,[Destination] = co.Alias
	,[BuyerDelivery] = o.BuyerDelivery
	,[SCIDelivery] = o.SciDelivery
	,[Barcode] = pd_Barcode.Barcode
	,[ScanBy] = g.IdAndName
	,[ScanTime] = Format(c.AddDate, 'yyyy/MM/dd HH:mm:ss')
from CTNHauling c WITH(NOLOCK)
inner join Orders o WITH(NOLOCK) on c.OrderID = o.ID
left join Country co WITH(NOLOCK) on o.Dest = co.ID
outer apply (
	select IdAndName
	from dbo.GetPassName(c.AddName)
)g
outer apply (
	select Qty = SUM(QtyPerCTN)
	from PackingList_Detail pd WITH(NOLOCK)
	where pd.SCICtnNo = c.SCICtnNo
)pd_QtyPerCTN
outer apply (
	SELECT Barcode = Stuff((
		select distinct concat('/',Barcode) 
		from PackingList_Detail pd WITH(NOLOCK)
		where pd.SCICtnNo = c.SCICtnNo
		FOR XML PATH(''))
	,1,1,'')
)pd_Barcode
where 1 = 1
{strWhere}
order by c.PackingListID, c.CTNStartNo, c.HaulingDate";
            #endregion

            this.ShowWaitMessage("Data Loading...");
            #region Set Grid Data
            DataTable dtGridData;
            DualResult result = DBProxy.Current.Select(null, strSqlCmd, listSqlParameter, out dtGridData);
            if (result)
            {
                this.bindingSource.DataSource = dtGridData;
            }
            else
            {
                MyUtility.Msg.WarningBox(result.ToString());
                this.bindingSource.DataSource = null;
            }
            #endregion
            this.HideWaitMessage();
        }
    }
}
