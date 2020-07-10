using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P13
    /// </summary>
    public partial class P13 : Win.Tems.QueryForm
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
                .Text("FtyGroup", header: "Factory", width: Widths.AnsiChars(8))
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8))
                .Text("ID", header: "SP#", width: Widths.AnsiChars(16))
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(13))
                .Date("SciDelivery", header: "SCI Delivery")
                .Date("BuyerDelivery", header: "Buyer Delivery")
                .Date("SewInline", header: "Sewing Inline Date")
                .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(13))
                .Text("Supplier", header: "Supplier", width: Widths.AnsiChars(20))
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
            if (MyUtility.Check.Empty(this.dateEstBookingDate.Value1)
                    && MyUtility.Check.Empty(this.dateEstBookingDate.Value2)
                    && MyUtility.Check.Empty(this.dateEstArrivedDate.Value1)
                    && MyUtility.Check.Empty(this.dateEstArrivedDate.Value2))
            {
                sqlCmd = this.Query(false);
            }
            else
            {
                sqlCmd = this.Query(true);
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
                MyUtility.Msg.WarningBox("Please query data first!");
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

        private StringBuilder Query(bool isPackData)
        {
            StringBuilder sqlCmd = new StringBuilder();

            if (isPackData)
            {
                sqlCmd.Append(@"
select distinct pld.OrderID
into #tmp_PackData
from PackingList pl WITH (NOLOCK) , PackingList_Detail pld WITH (NOLOCK) 
where pl.ID = pld.ID
");
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
            }

            sqlCmd.Append(
 @"
select o.FtyGroup
	,o.BrandID
	,o.ID
	,o.SciDelivery
	,o.BuyerDelivery
	,o.SewInLine
	,o.Dest
	,c.Alias
	,ocd.RefNo
	,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension
	,li.CtnUnit
	,ocd.CTNQty
	,[Supplier] = li.LocalSuppid + '-' + ls.Abb
into #tmp_OrderData
from Orders o WITH (NOLOCK) 
left join Country c WITH (NOLOCK) on c.id = o.Dest
left join Order_CTNData ocd WITH (NOLOCK) on ocd.ID = o.ID
left join LocalItem li WITH (NOLOCK) on li.RefNo = ocd.RefNo
left join LocalSupp ls WITH (NOLOCK) on li.LocalSuppid = ls.ID
");
            if (isPackData)
            {
                sqlCmd.Append(" inner join #tmp_PackData pd on o.ID = pd.OrderID");
            }

            sqlCmd.Append(string.Format(" where o.MDivisionID = '{0}'", Env.User.Keyword));
            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= cast('{0}' as date)", Convert.ToDateTime(this.dateSCIDelivery.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= cast('{0}' as date)", Convert.ToDateTime(this.dateSCIDelivery.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value1))
            {
                sqlCmd.Append(string.Format(" and o.SewInLine >= cast('{0}' as date)", Convert.ToDateTime(this.dateSewingInlineDate.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateSewingInlineDate.Value2))
            {
                sqlCmd.Append(string.Format(" and o.SewInLine <= cast('{0}' as date)", Convert.ToDateTime(this.dateSewingInlineDate.Value2).ToString("d")));
            }

            sqlCmd.Append(@"  

SELECT FtyGroup,BrandID,ID,SciDelivery,BuyerDelivery,SewInLine,Alias,LocalPOID,Refno,Dimension,CtnUnit,SUM(POQty) AS POQty,Delivery,Supplier
into #tmp_POData
from 
(
	select DISTINCT od.FtyGroup
        ,od.BrandID
		,od.ID
		,od.SciDelivery
		,od.BuyerDelivery
		,od.SewInLine
		,od.Alias
		,ld.Id as LocalPOID
		,ld.Refno
		,STR(li.CtnLength,8,4)+'*'+STR(li.CtnWidth,8,4)+'*'+STR(li.CtnHeight,8,4) as Dimension
		,li.CtnUnit
		,ld.Qty as POQty
		,ld.Delivery 
		,[Supplier] = li.LocalSuppid + '-' + ls.Abb
	from #tmp_OrderData od
	inner join LocalPO_Detail ld WITH (NOLOCK) on od.ID = ld.OrderId 
	inner join LocalItem li WITH (NOLOCK) on li.RefNo = ld.Refno
	inner join LocalPO LP WITH (NOLOCK) on LP.id= ld.id 
	left join LocalSupp ls WITH (NOLOCK) on li.LocalSuppid = ls.ID
	where LP.category='CARTON'
)a
group by FtyGroup,BrandID,ID,SciDelivery,BuyerDelivery,SewInLine,Alias,LocalPOID,Refno,Dimension,CtnUnit,Delivery,Supplier;

SELECT RefNo,ID,SUM(POQty) as POQty 
into #tmp_AccuQty
FROM #tmp_POData 
GROUP BY RefNo,ID; 

select isnull(od.FtyGroup, pd.FtyGroup) as FtyGroup
    ,isnull(od.BrandID,pd.BrandID) as BrandID
	,isnull(od.ID,pd.ID) as ID
	,isnull(od.Alias
	,isnull(pd.Alias,'')) as Alias
	,isnull(od.SciDelivery,pd.SciDelivery) as SciDelivery
    ,isnull(od.BuyerDelivery, pd.BuyerDelivery) as BuyerDelivery
	,isnull(od.SewInLine,pd.SewInLine) as SewInLine
	,isnull(od.Refno,pd.Refno) as Refno
    ,isnull(od.Supplier, pd.Supplier) as Supplier
	,isnull(od.Dimension,isnull(pd.Dimension,'')) as Dimension
	,isnull(od.CtnUnit,isnull(pd.CtnUnit,'')) as CtnUnit
	,isnull(od.CTNQty,0) as CTNQty
	,isnull(od.CTNQty,0)-isnull((select a.POQty from #tmp_AccuQty a where a.ID = pd.ID and a.Refno = pd.RefNo),0) as AccuQty
	,isnull(pd.LocalPOID,'') as LocalPOID, pd.Delivery
	,isnull(pd.POQty,0) as POQty
from #tmp_OrderData od
full outer join #tmp_POData pd on pd.ID = od.ID and pd.Refno = od.RefNo
order by SciDelivery,ID,Refno;

drop table #tmp_AccuQty,#tmp_OrderData,#tmp_POData;
");

            if (isPackData)
            {
                sqlCmd.Append(" drop table #tmp_PackData;");
            }

            return sqlCmd;
        }
    }
}
