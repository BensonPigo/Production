#pragma warning disable SA1652 // Enable XML documentation output
using System;
#pragma warning restore SA1652 // Enable XML documentation output
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P10
    /// </summary>
    public partial class P09 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P10
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private DataTable gridData;
        private string selectDataTable_DefaultView_Sort = string.Empty;

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.gridTransferDate.IsEditingReadOnly = false;
            this.gridTransferDate.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridTransferDate)
            .Date("TransferDate", header: "Transfer Date", iseditable: false)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditable: false)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditable: false)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditable: false)
            .Text("CustPONo", header: "PO No.", width: Widths.Auto(), iseditable: false)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditable: false)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditable: false)
            .Text("Dest", header: "Destination", width: Widths.Auto(), iseditable: false)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditable: false)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditable: false)
            .Text("AddName", header: "Received By", width: Widths.Auto(), iseditable: false)
            .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.gridTransferDate.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.gridTransferDate.Sorted += (s, e) =>
            {
                #region 如果準備排序的欄位 = "CTNStartNo" 則用以下方法排序
                if ((rowIndex == -1) && this.gridData.Columns[columIndex].ColumnName.ToString().EqualString("CTNStartNo"))
                {
                    this.listControlBindingSource1.DataSource = null;

                    if (this.selectDataTable_DefaultView_Sort == "DESC")
                    {
                        this.gridData.DefaultView.Sort = "rn DESC";
                        this.selectDataTable_DefaultView_Sort = string.Empty;
                    }
                    else
                    {
                        this.gridData.DefaultView.Sort = "rn ASC";
                        this.selectDataTable_DefaultView_Sort = "DESC";
                    }

                    this.listControlBindingSource1.DataSource = this.gridData;
                    return;
                }
                #endregion
            };
        }

        // Query
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading...");
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
select  1 as selected
        , TransferDate
        , PackingListID
        , CTNStartNo
        , OrderID
        , CustPONo
        , StyleID
        , BrandID
        , Dest
        , BuyerDelivery
        , SCIDelivery
        , AddName
        , rn = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
        , RepackPackID
        , RepackOrderID
        , RepackCtnStartNo
from (
        select  cr.TransferDate
            , [PackingListID] = iif(pd.OrigID = '',pd.ID, pd.OrigID)
            , [CTNStartNo] = iif(pd.OrigCTNStartNo = '',pd.CTNStartNo, pd.OrigCTNStartNo)
            , [OrderID] = iif(pd.OrigOrderID = '',pd.OrderID, pd.OrigOrderID)
            , isnull(o.CustPONo,'') as CustPONo
            , isnull(o.StyleID,'') as StyleID
            , isnull(o.BrandID,'') as BrandID
            , isnull(c.Alias,'') as Dest
            , o.BuyerDelivery
            , o.SciDelivery
            , AddName = dbo.getPass1(cr.AddName)
            , [RepackPackID] = iif(pd.OrigID != '',pd.ID, pd.OrigID)
            , [RepackOrderID] = iif(pd.OrigOrderID != '',pd.OrderID, pd.OrigOrderID)
            , [RepackCtnStartNo] = iif(pd.OrigCTNStartNo != '',pd.CTNStartNo, pd.OrigCTNStartNo)
    from PackingList_Detail pd WITH (NOLOCK) 
    inner join TransferToCFA cr WITH (NOLOCK) on  pd.SCICtnNo = cr.SCICtnNo 
    left join Orders o WITH (NOLOCK) on cr.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    where 1=1 
", Env.User.Keyword));

            if (!MyUtility.Check.Empty(this.dateTransferDate.Value1))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.TransferDate >= '{0}'", Convert.ToDateTime(this.dateTransferDate.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateTransferDate.Value2))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.TransferDate <= '{0}'", Convert.ToDateTime(this.dateTransferDate.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and (pd.ID = '{0}' or  pd.OrigID = '{0}')", MyUtility.Convert.GetString(this.txtPackID.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and (pd.OrderID = '{0}' or pd.OrigOrderID = '{0}')", MyUtility.Convert.GetString(this.txtSPNo.Text)));
            }

            sqlCmd.Append(@"
)a
order by PackingListID,rn,TransferDate");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.gridTransferDate.AutoResizeColumns();
            this.HideWaitMessage();
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
