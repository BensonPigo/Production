using System;
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
    public partial class P07 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataTable gridData, printData, ctnDim, qtyBDown;
        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            radioFormA.Checked = true;

            //Grid設定
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridDetail)
               .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
               .Text("ID", header: "Packing No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("InvNo", header: "Garment Booking#", width: Widths.AnsiChars(25), iseditingreadonly: true)
               .Date("FCRDate", header: "FCR Date", iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(13), iseditingreadonly: true);
        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(txtGarmentBookingStart.Text) && MyUtility.Check.Empty(txtGarmentBookingEnd.Text) && MyUtility.Check.Empty(dateFCRDate.Value1) && MyUtility.Check.Empty(dateFCRDate.Value2))
            {
                MyUtility.Msg.WarningBox("< Garment Booking# > and < FCR Date > can't both empty!");
                return;
            }
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"with tmpPackingData
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
            if (!MyUtility.Check.Empty(txtGarmentBookingStart.Text))
            {
                sqlCmd.Append(string.Format(" and p.INVNo >= '{0}'", txtGarmentBookingStart.Text));
            }
            if (!MyUtility.Check.Empty(txtGarmentBookingEnd.Text))
            {
                sqlCmd.Append(string.Format(" and p.INVNo <= '{0}'", txtGarmentBookingEnd.Text));
            }
            if (!MyUtility.Check.Empty(dateFCRDate.Value1))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}'", Convert.ToDateTime(dateFCRDate.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateFCRDate.Value2))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}'", Convert.ToDateTime(dateFCRDate.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(txtbrand.Text))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", txtbrand.Text));
            }
            sqlCmd.Append(@"),
MultipleOrder
as (
select ID,COUNT(ID) as cnt from tmpPackingData group by ID having COUNT(ID) > 1
)
select 0 as selected,* from tmpPackingData where NOT EXISTS (select 1 from MultipleOrder where ID = tmpPackingData.ID) order by ID"); //排除多SP#在同一張PL的資料
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (result)
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                gridData = null;
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading....");
            if (MyUtility.Check.Empty(listControlBindingSource1)) return;
            foreach (DataRow dr in ((DataTable)listControlBindingSource1.DataSource).Rows)
            {
                if (MyUtility.Convert.GetString(dr["selected"]) == "1")
                {
                    DualResult result = PublicPrg.Prgs.QueryPackingListReportData(MyUtility.Convert.GetString(dr["ID"]), radioFormA.Checked?"1":"2", out printData, out ctnDim, out qtyBDown);
                    if (!result)
                    {
                        MyUtility.Msg.WarningBox("Query Data Fail --\r\n" + result.ToString());
                        this.HideWaitMessage();
                        return;
                    }
                    PublicPrg.Prgs.PackingListToExcel_PackingListReport("\\Packing_P03_PackingListReport.xltx", dr, radioFormA.Checked ? "1" : "2", printData, ctnDim, qtyBDown);
                }
            }
            this.HideWaitMessage();
            MyUtility.Msg.InfoBox("Complete.");
        }
    }
}
