﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P07
    /// </summary>
    public partial class P07 : Sci.Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable gridData;
        private DataTable printData;
        private DataTable ctnDim;
        private DataTable qtyBDown;

        /// <summary>
        /// P07
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.radioFormA.Checked = true;

            // Grid設定
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
               .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
               .Text("ID", header: "Packing No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("InvNo", header: "Garment Booking#", width: Widths.AnsiChars(25), iseditingreadonly: true)
               .Date("FCRDate", header: "FCR Date", iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(13), iseditingreadonly: true);
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtGarmentBookingStart.Text) && MyUtility.Check.Empty(this.txtGarmentBookingEnd.Text) && MyUtility.Check.Empty(this.dateFCRDate.Value1) && MyUtility.Check.Empty(this.dateFCRDate.Value2))
            {
                MyUtility.Msg.WarningBox("< Garment Booking# > and < FCR Date > can't both empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"with tmpPackingData
as (
select distinct p.ID,pd.OrderID,p.INVNo,g.FCRDate,o.BrandID,pd.OrderShipmodeSeq,isnull(oq.Qty,0) as Qty,
p.CustCDID,p.ShipModeID,p.Remark,p.CTNQty
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join GMTBooking g WITH (NOLOCK) on p.INVNo = g.ID
inner join Orders o WITH (NOLOCK) on pd.OrderID = o.ID
left join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id and oq.Seq = pd.OrderShipmodeSeq
where p.Type = 'B'
and p.MDivisionID = '{0}'", Sci.Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.txtGarmentBookingStart.Text))
            {
                sqlCmd.Append(string.Format(" and p.INVNo >= '{0}'", this.txtGarmentBookingStart.Text));
            }

            if (!MyUtility.Check.Empty(this.txtGarmentBookingEnd.Text))
            {
                sqlCmd.Append(string.Format(" and p.INVNo <= '{0}'", this.txtGarmentBookingEnd.Text));
            }

            if (!MyUtility.Check.Empty(this.dateFCRDate.Value1))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}'", Convert.ToDateTime(this.dateFCRDate.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateFCRDate.Value2))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}'", Convert.ToDateTime(this.dateFCRDate.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.txtbrand.Text));
            }

            sqlCmd.Append(@"),
MultipleOrder
as (
select ID,COUNT(ID) as cnt from tmpPackingData group by ID having COUNT(ID) > 1
)
select 0 as selected,* from tmpPackingData where NOT EXISTS (select 1 from MultipleOrder where ID = tmpPackingData.ID) order by ID");

            // 排除多SP#在同一張PL的資料
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (result)
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                this.gridData = null;
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading....");
            if (MyUtility.Check.Empty(this.gridData))
            {
                this.HideWaitMessage();
                return;
            }

            if (this.gridData.Rows.Count == 0)
            {
                this.HideWaitMessage();
                return;
            }

            foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["selected"]) == "1")
                {
                    DualResult result = PublicPrg.Prgs.QueryPackingListReportData(MyUtility.Convert.GetString(dr["ID"]), this.radioFormA.Checked ? "1" : "2", out this.printData, out this.ctnDim, out this.qtyBDown);
                    if (!result)
                    {
                        this.HideWaitMessage();
                        MyUtility.Msg.WarningBox("Query Data Fail --\r\n" + result.ToString());
                        return;
                    }

                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport.xltx", dr, this.radioFormA.Checked ? "1" : "2", this.printData, this.ctnDim, this.qtyBDown);
                }
            }

            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Complete.");
        }
    }
}
