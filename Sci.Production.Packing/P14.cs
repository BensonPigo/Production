using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P14
    /// </summary>
    public partial class P14 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// P14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private DataTable gridData;
        private string selectDataTable_DefaultView_Sort = string.Empty;

        /// <summary>
        /// OnFormLoaded
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.dateTimePicker1.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePicker2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd 08:00");
            this.dateTimePicker2.Text = DateTime.Now.ToString("yyyy/MM/dd 12:00");

            // Grid設定
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Date("TransferDate", header: "Transfer Date", iseditable: false)
                .Text("TransferSlipNo", header: "TransferSlipNo", width: Widths.AnsiChars(15), iseditable: false)
                .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(15), iseditable: false)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditable: false)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditable: false)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditable: false)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditable: false)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("CustPONo", header: "PO No.", width: Widths.AnsiChars(15), iseditable: false)
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(20), iseditable: false)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditable: false)
                .DateTime("AddDate", header: "Create Date", iseditable: false)
                .Date("ReceiveDate", header: "CLOG CFM", iseditable: false)
                .Date("ReturnDate", header: "Return Date", iseditable: false)
                .Text("AddName", header: "AddName", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
                .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.gridDetail.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.gridDetail.Sorted += (s, e) =>
            {
                if ((rowIndex == -1) & (columIndex == 4))
                {
                    this.listControlBindingSource1.DataSource = null;

                    if (this.selectDataTable_DefaultView_Sort == "DESC")
                    {
                        this.gridData.DefaultView.Sort = "rn1 DESC";
                        this.selectDataTable_DefaultView_Sort = string.Empty;
                    }
                    else
                    {
                        this.gridData.DefaultView.Sort = "rn1 ASC";
                        this.selectDataTable_DefaultView_Sort = "DESC";
                    }

                    this.listControlBindingSource1.DataSource = this.gridData;
                    return;
                }
            };
        }

        private string cmd;

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
select  *
        , rn = ROW_NUMBER() over(order by Id,OrderID,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
        , rn1 = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
from (
    select  1 as selected
            , t.TransferDate
            , t.TransferSlipNo
            , [PackingListID] = iif(pd.OrigID = '',pd.ID, pd.OrigID)
            , [OrderID] = iif(pd.OrigOrderID = '',pd.OrderID, pd.OrigOrderID)
            , [CTNStartNo] = iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo)
            , pd.Id
            , [CTN#]= iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo)
            , isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1
            , isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest
            , isnull(o.FactoryID,'') as FactoryID
            , convert(varchar, oq.BuyerDelivery, 111) as BuyerDelivery
            , t.AddDate
            , tid = t.id
            , pd.ReceiveDate
			, pd.ReturnDate
			, AddName = (select concat(id,'-',Name) from pass1 where id = t.AddName)
            , pd.SizeCode
            , [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
            , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
            , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
    from TransferToClog t WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on t.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join PackingList_Detail pd WITH (NOLOCK) on  pd.ID = t.PackingListID and pd.CTNStartNo = t.CTNStartNo
    left join Order_QtyShip oq WITH (NOLOCK) on  oq.Id = pd.OrderID 
                                                    and oq.Seq = pd.OrderShipmodeSeq
    where t.MDivisionID = '{0}' 
    --and pd.TransferDate is not null 
    ", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(this.dateTimePicker1.Text))
            {
                sqlCmd.Append(string.Format(" and t.AddDate >= '{0}'", this.dateTimePicker1.Text));
            }

            if (!MyUtility.Check.Empty(this.dateTimePicker2.Text))
            {
                sqlCmd.Append(string.Format(" and t.AddDate <= '{0}'", this.dateTimePicker2.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(" and (pd.ID = '{0}' or  pd.OrigID = '{0}')", MyUtility.Convert.GetString(this.txtPackID.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlCmd.Append(string.Format(" and (pd.OrderID = '{0}' or pd.OrigOrderID = '{0}')", MyUtility.Convert.GetString(this.txtSP.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlCmd.Append(string.Format(" and o.FactoryID = '{0}'", MyUtility.Convert.GetString(this.txtfactory.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtTransferSlipNo.Text))
            {
                sqlCmd.Append(string.Format(" and t.TransferSlipNo = '{0}'", MyUtility.Convert.GetString(this.txtTransferSlipNo.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtCtnStart.Text))
            {
                sqlCmd.Append(string.Format(" and ISNULL(TRY_CAST( pd.CTNStartNo AS int),0)>= {0} ", MyUtility.Convert.GetInt(this.txtCtnStart.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtCtnEnd.Text))
            {
                sqlCmd.Append(string.Format(" and ISNULL(TRY_CAST( pd.CTNStartNo AS int),0)<= {0} ", MyUtility.Convert.GetInt(this.txtCtnEnd.Text)));
            }

            sqlCmd.Append(@"
) X order by rn");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.cmd = sqlCmd.ToString();
            if (this.gridData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
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
            this.gridDetail.ValidateControl();
            this.listControlBindingSource1.EndEdit();

            // button1_Click(null, null);
            DataTable excelTable = (DataTable)this.listControlBindingSource1.DataSource;
            if (excelTable == null || excelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            // 如果沒勾選資料,會跳訊息
            DataRow[] selectedData = excelTable.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Checked item first before click ToExcel");
                return;
            }

            // 將Grid勾選的資料匯到#tmp table,再將資料丟進DataTable匯出Excel
            DataTable selectData = null;

            string sqlcmd = @"select TransferDate,TransferSlipNo,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,FactoryID,BuyerDelivery,AddDate,tid from #tmp where selected=1";
            MyUtility.Tool.ProcessWithDatatable(
                excelTable, @"Selected,TransferSlipNo,TransferDate,PackingListID,OrderID,CTNStartNo,StyleID,BrandID,Customize1,CustPONo,Dest,FactoryID,BuyerDelivery,AddDate,tid", sqlcmd, out selectData, "#tmp");

            string date1, date2, packID, sPNo;
            date1 = (!MyUtility.Check.Empty(this.dateTimePicker1.Text)) ? this.dateTimePicker1.Text : null;
            date2 = (!MyUtility.Check.Empty(this.dateTimePicker2.Text)) ? this.dateTimePicker2.Text : null;
            packID = (!MyUtility.Check.Empty(this.txtPackID.Text)) ? this.txtPackID.Text : null;
            sPNo = (!MyUtility.Check.Empty(this.txtSP.Text)) ? this.txtSP.Text : null;
            P14_Print_OrderList frm = new P14_Print_OrderList(selectData, date1, date2, packID, sPNo, this.cmd);
            frm.ShowDialog();
        }
    }
}
