using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Quality
{
    /// <summary>
    /// Quality_P25
    /// </summary>
    public partial class P25 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P25
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P25(ToolStripMenuItem menuitem)
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
            this.gridReceiveDate.IsEditingReadOnly = false;
            this.gridReceiveDate.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridReceiveDate)
            .Date("ReceiveDate", header: "Receive Date", iseditable: false)
            .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditable: false)
            .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditable: false)
            .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditable: false)
            .Text("CustPONo", header: "PO No.", width: Widths.Auto(), iseditable: false)
            .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditable: false)
            .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditable: false)
            .Text("Dest", header: "Destination", width: Widths.Auto(), iseditable: false)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditable: false)
            .Date("SciDelivery", header: "SCI Delivery", width: Widths.Auto(), iseditable: false)
            .Text("CFALocationID", header: "Location No.", width: Widths.Auto(), iseditable: false)
            .Text("AddName", header: "Received By", width: Widths.Auto(), iseditable: false)
            .Text("RepackPackID", header: "Repack To Pack ID", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackOrderID", header: "Repack To SP #", width: Widths.AnsiChars(15), iseditable: false)
            .Text("RepackCtnStartNo", header: "Repack To CTN #", width: Widths.AnsiChars(6), iseditable: false);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int rowIndex = 0;
            int columIndex = 0;
            this.gridReceiveDate.CellClick += (s, e) =>
            {
                rowIndex = e.RowIndex;
                columIndex = e.ColumnIndex;
            };

            this.gridReceiveDate.Sorted += (s, e) =>
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
        , ReceiveDate
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
        , CFALocationID
from (
    select  cr.ReceiveDate
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
            , cr.CFALocationID
    from CFAReceive cr WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on cr.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join PackingList_Detail pd WITH (NOLOCK) on  pd.SCICtnNo = cr.SCICtnNo 
    where   cr.MDivisionID = '{0}'
", Env.User.Keyword));

            if (!MyUtility.Check.Empty(this.dateReceiveDate.Value1))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.ReceiveDate >= '{0}'", Convert.ToDateTime(this.dateReceiveDate.Value1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.dateReceiveDate.Value2))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and cr.ReceiveDate <= '{0}'", Convert.ToDateTime(this.dateReceiveDate.Value2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and (pd.ID = '{0}' or pd.ID = '{0}')", MyUtility.Convert.GetString(this.txtPackID.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(
                    @" 
            and (pd.OrderID = '{0}' or pd.OrderID = '{0}')", MyUtility.Convert.GetString(this.txtSPNo.Text)));
            }

            sqlCmd.Append(@"
)a
order by PackingListID,rn,ReceiveDate");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = this.gridData;
            this.gridReceiveDate.AutoResizeColumns();
            this.HideWaitMessage();
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
