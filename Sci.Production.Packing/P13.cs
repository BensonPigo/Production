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

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P13
    /// </summary>
    public partial class P13 : Sci.Win.Tems.QueryForm
    {
        private DataTable gridData;
        private DataGridViewGeneratorNumericColumnSettings ctnqty = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings accuqty = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings poqty = new DataGridViewGeneratorNumericColumnSettings();

        /// <summary>
        /// P13
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P13(ToolStripMenuItem menuitem)
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

            // Grid設定
            this.gridDetail.IsEditingReadOnly = true;
            this.gridDetail.DataSource = this.listControlBindingSource1;

            // 當欄位值為0時，顯示空白
            this.ctnqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            this.accuqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;
            this.poqty.CellZeroStyle = Ict.Win.UI.DataGridViewNumericBoxZeroStyle.Empty;

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("ID", header: "SP#", width: Widths.AnsiChars(16))
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(13))
                .Date("SciDelivery", header: "SCI Delivery")
                .Date("SewInline", header: "Sewing Inline Date")
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(13))
                .Text("Dimension", header: "L * W * H", width: Widths.AnsiChars(25))
                .Text("CTNUnit", header: "Carton Unit", width: Widths.AnsiChars(8))
                .Numeric("CTNQty", header: "Carton Qty", settings: this.ctnqty)
                .Numeric("AccuQty", header: "Accu. Qty", settings: this.accuqty)
                .Text("LocalPOID", header: "Local PO#", width: Widths.AnsiChars(13))
                .Date("Delivery", header: "Delivery")
                .Numeric("POQty", header: "PO Qty", settings: this.poqty);

            this.gridDetail.Columns["LocalPOID"].DefaultCellStyle.BackColor = Color.LightGreen;
            this.gridDetail.Columns["Delivery"].DefaultCellStyle.BackColor = Color.LightGreen;
            this.gridDetail.Columns["POQty"].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) && MyUtility.Check.Empty(this.dateSewingInlineDate.Value1) && MyUtility.Check.Empty(this.dateSewingInlineDate.Value2) && MyUtility.Check.Empty(this.dateEstBookingDate.Value1) && MyUtility.Check.Empty(this.dateEstBookingDate.Value2) && MyUtility.Check.Empty(this.dateEstArrivedDate.Value1) && MyUtility.Check.Empty(this.dateEstArrivedDate.Value2))
            {
                this.dateSCIDelivery.TextBox1.Focus();
                MyUtility.Msg.WarningBox("< SCI Delivery > or < Sewing Inline Date > or < Carton Est. Booking > or < Carton Est. Arrived > can not empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            if (MyUtility.Check.Empty(this.dateEstBookingDate.Value1) && MyUtility.Check.Empty(this.dateEstBookingDate.Value2) && MyUtility.Check.Empty(this.dateEstArrivedDate.Value1) && MyUtility.Check.Empty(this.dateEstArrivedDate.Value2))
            {
                sqlCmd.Append(string.Format(
                    @"with OrderData
as
(select o.BrandID,o.ID,o.SciDelivery,o.SewInLine,o.Dest,c.Alias,ocd.RefNo,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,ocd.CTNQty
 from Orders o WITH (NOLOCK) 
 left join Country c WITH (NOLOCK) on c.id = o.Dest
 left join Order_CTNData ocd WITH (NOLOCK) on ocd.ID = o.ID
 left join LocalItem li WITH (NOLOCK) on li.RefNo = ocd.RefNo
 where o.MDivisionID = '{0}'", Sci.Env.User.Keyword));
                if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
                {
                    sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
                {
                    sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and o.SewInLine >= '{0}'", Convert.ToDateTime(this.dateSewingInlineDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and o.SewInLine <= '{0}'", Convert.ToDateTime(this.dateSewingInlineDate.Value2).ToString("d")));
                }

                sqlCmd.Append(@" ),
POData
as
(SELECT BrandID,ID,SciDelivery,SewInLine,Alias,LocalPOID,Refno,Dimension,CtnUnit,SUM(POQty) AS POQty,Delivery
 FROM (select DISTINCT od.BrandID,od.ID,od.SciDelivery,od.SewInLine,od.Alias,ld.Id as LocalPOID,ld.Refno,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,ld.Qty as POQty,ld.Delivery
 from OrderData od , LocalPO_Detail ld WITH (NOLOCK) , LocalItem li WITH (NOLOCK) ,LocalPO LP WITH (NOLOCK) 
 where od.ID = ld.OrderId
 and li.RefNo = ld.Refno
 and LP.id= ld.id
 and LP.category='CARTON') a
 group by BrandID,ID,SciDelivery,SewInLine,Alias,LocalPOID,Refno,Dimension,CtnUnit,Delivery
),
AccuQty
as
(SELECT RefNo,ID,SUM(POQty) as POQty 
 FROM POData 
 GROUP BY RefNo,ID
)
select isnull(od.BrandID,pd.BrandID) as BrandID,isnull(od.ID,pd.ID) as ID,isnull(od.Alias,isnull(pd.Alias,'')) as Alias,isnull(od.SciDelivery,pd.SciDelivery) as SciDelivery,isnull(od.SewInLine,pd.SewInLine) as SewInLine,isnull(od.Refno,pd.Refno) as Refno,
isnull(od.Dimension,isnull(pd.Dimension,'')) as Dimension,isnull(od.CtnUnit,isnull(pd.CtnUnit,'')) as CtnUnit,isnull(od.CTNQty,0) as CTNQty,isnull(od.CTNQty,0)-isnull((select a.POQty from AccuQty a where a.ID = pd.ID and a.Refno = pd.RefNo),0) as AccuQty,isnull(pd.LocalPOID,'') as LocalPOID, pd.Delivery,isnull(pd.POQty,0) as POQty
from OrderData od
full outer join POData pd on pd.ID = od.ID and pd.Refno = od.RefNo
order by SciDelivery,ID,Refno");
            }
            else
            {
                sqlCmd.Append(@"with PackData
as
(select distinct pld.OrderID
 from PackingList pl WITH (NOLOCK) , PackingList_Detail pld WITH (NOLOCK) 
 where pl.ID = pld.ID");
                if (!MyUtility.Check.Empty(this.dateEstBookingDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and pl.EstCTNBooking >= '{0}'", Convert.ToDateTime(this.dateEstBookingDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateEstBookingDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and pl.EstCTNBooking <= '{0}'", Convert.ToDateTime(this.dateEstBookingDate.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateEstArrivedDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and pl.EstCTNArrive >= '{0}'", Convert.ToDateTime(this.dateEstArrivedDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateEstArrivedDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and pl.EstCTNArrive <= '{0}'", Convert.ToDateTime(this.dateEstArrivedDate.Value2).ToString("d")));
                }

                sqlCmd.Append(string.Format(
                    @"),
OrderData
as
(select o.BrandID,o.ID,o.SciDelivery,o.SewInLine,o.Dest,c.Alias,ocd.RefNo,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,ocd.CTNQty
 from PackData pd
 left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
 left join Country c WITH (NOLOCK) on c.id = o.Dest
 left join Order_CTNData ocd WITH (NOLOCK) on ocd.ID = o.ID
 left join LocalItem li WITH (NOLOCK) on li.RefNo = ocd.RefNo
 where o.MDivisionID = '{0}'", Sci.Env.User.Keyword));
                if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
                {
                    sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
                {
                    sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value1))
                {
                    sqlCmd.Append(string.Format(" and o.SewInLine >= '{0}'", Convert.ToDateTime(this.dateSewingInlineDate.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value2))
                {
                    sqlCmd.Append(string.Format(" and o.SewInLine <= '{0}'", Convert.ToDateTime(this.dateSewingInlineDate.Value2).ToString("d")));
                }

                sqlCmd.Append(@" ),
POData
as
(SELECT BrandID,ID,SciDelivery,SewInLine,Alias,LocalPOID,Refno,Dimension,CtnUnit,SUM(POQty) AS POQty,Delivery
 FROM (select DISTINCT od.BrandID,od.ID,od.SciDelivery,od.SewInLine,od.Alias,ld.Id as LocalPOID,ld.Refno,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension,li.CtnUnit,ld.Qty as POQty,ld.Delivery
 from OrderData od, LocalPO_Detail ld WITH (NOLOCK) , LocalItem li WITH (NOLOCK) ,LocalPO LP WITH (NOLOCK) 
 where od.ID = ld.OrderId
 and li.RefNo = ld.Refno
 and LP.id= ld.id
 and LP.category='CARTON') a
 group by BrandID,ID,SciDelivery,SewInLine,Alias,LocalPOID,Refno,Dimension,CtnUnit,Delivery
),
AccuQty
as
(SELECT RefNo,ID,SUM(POQty) as POQty 
 FROM POData 
 GROUP BY RefNo,ID
)
select isnull(od.BrandID,pd.BrandID) as BrandID,isnull(od.ID,pd.ID) as ID,isnull(od.Alias,isnull(pd.Alias,'')) as Alias,isnull(od.SciDelivery,pd.SciDelivery) as SciDelivery,isnull(od.SewInLine,pd.SewInLine) as SewInLine,isnull(od.Refno,pd.Refno) as Refno,
isnull(od.Dimension,isnull(pd.Dimension,'')) as Dimension,isnull(od.CtnUnit,isnull(pd.CtnUnit,'')) as CtnUnit,isnull(od.CTNQty,0) as CTNQty,isnull(od.CTNQty,0)-isnull((select a.POQty from AccuQty a where a.ID = pd.ID and a.Refno = pd.RefNo),0) as AccuQty,isnull(pd.LocalPOID,'') as LocalPOID,pd.Delivery,isnull(pd.POQty,0) as POQty
from OrderData od
full outer join POData pd on pd.ID = od.ID and pd.Refno = od.RefNo
order by SciDelivery,ID,Refno");
            }

            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData))
            {
                if (this.gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable excelTable = (DataTable)this.listControlBindingSource1.DataSource;

            if (excelTable == null || excelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            bool result = MyUtility.Excel.CopyToXls(excelTable, string.Empty, xltfile: "Packing_P13.xltx", headerRow: 2);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
