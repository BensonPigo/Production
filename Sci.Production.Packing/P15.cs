using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <summary>
    /// Packing_P15
    /// </summary>
    public partial class P15 : Win.Tems.QueryForm
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable gridData;
        private string selectDataTable_DefaultView_Sort = string.Empty;

        /// <summary>
        /// P15
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P15(ToolStripMenuItem menuitem)
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
            this.gridDetail.IsEditingReadOnly = false;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Date("TransferDate", header: "Transfer Date", iseditingreadonly: true)
                .Text("TransferSlipNo", header: "TransferSlipNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("PackingListID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(15), iseditable: false)
                .Text("CustPONo", header: "PO No.", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Dest", header: "Destination", width: Widths.AnsiChars(20), iseditable: false)
                .Date("BuyerDelivery", header: "Buyer Delivery", iseditingreadonly: true)
                .DateTime("AddDate", header: "Create Date", iseditingreadonly: true);

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
        private void Query()
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
select  *
        , rn = ROW_NUMBER() over(order by Id,OrderID,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
        , rn1 = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
from (
    select  0 as selected
            , t.TransferDate
            , t.TransferSlipNo
            , t.PackingListID
            , t.OrderID
            , t.CTNStartNo
            , pd.Id
            , isnull(o.StyleID,'') as StyleID,isnull(o.BrandID,'') as BrandID,isnull(o.Customize1,'') as Customize1
            , isnull(o.CustPONo,'') as CustPONo,isnull(c.Alias,'') as Dest
            , isnull(o.FactoryID,'') as FactoryID
            , convert(varchar, oq.BuyerDelivery, 111) as BuyerDelivery
            , t.AddDate
            , tid = t.id
            , [PackingList_Detail_Ukey]=pd.Ukey
    from TransferToClog t WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on t.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    inner join PackingList_Detail pd WITH (NOLOCK) on  pd.ID = t.PackingListID 
                                                        and pd.OrderID = t.OrderID 
                                                        and pd.CTNStartNo = t.CTNStartNo 
                                                        and pd.CTNQty > 0
                                                        and pd.ReceiveDate is null             
    left join Order_QtyShip oq WITH (NOLOCK) on  oq.Id = pd.OrderID 
                                                    and oq.Seq = pd.OrderShipmodeSeq
    where t.TransferSlipNo <> ''
    and t.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(" and t.PackingListID = '{0}'", MyUtility.Convert.GetString(this.txtPackID.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlCmd.Append(string.Format(" and t.OrderID = '{0}'", MyUtility.Convert.GetString(this.txtSP.Text)));
            }

            if (!MyUtility.Check.Empty(this.txtTransferSlipNo.Text))
            {
                sqlCmd.Append(string.Format(" and t.TransferSlipNo = '{0}'", MyUtility.Convert.GetString(this.txtTransferSlipNo.Text)));
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
            }

            this.listControlBindingSource1.DataSource = this.gridData;
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSP.Text.Trim()) && MyUtility.Check.Empty(this.txtPackID.Text.Trim()) && MyUtility.Check.Empty(this.txtTransferSlipNo.Text.Trim()))
            {
                MyUtility.Msg.WarningBox("<SP#> <Pack ID> <TransferSlipNo> cannot be empty!");
                this.txtSP.Focus();
                return;
            }

            this.Query();
        }

        // 清空TransferSlipNo
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable dtGrid = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGrid) || dtGrid.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drCheck = dtGrid.Select("selected = 1");
            if (drCheck.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }
            else
            {
                this.ShowWaitMessage("Data Processing....");

                string updateCmd = @"
update TransferToClog
set TransferSlipNo=''
where id in (select id from #tmp) 
;
update PackingList_Detail
set TransferDate = NULL
where Ukey in (select PackingList_Detail_Ukey from #tmp) 
;
";
                DataTable dtUpdate = new DataTable();
                dtUpdate.Columns.Add("ID");
                dtUpdate.Columns.Add("PackingList_Detail_Ukey");
                foreach (DataRow item in drCheck)
                {
                    DataRow drNew = dtUpdate.NewRow();
                    drNew["ID"] = item["tid"].ToString();
                    drNew["PackingList_Detail_Ukey"] = item["PackingList_Detail_Ukey"].ToString();
                    dtUpdate.Rows.Add(drNew);
                }

                #region TransactionScope
                TransactionScope transactionscope = new TransactionScope();
                DualResult result;
                using (transactionscope)
                {
                    try
                    {
                        DataTable dtResult;
                        if ((result = MyUtility.Tool.ProcessWithDatatable(dtUpdate, null, updateCmd, out dtResult)) == false)
                        {
                            transactionscope.Dispose();
                            MyUtility.Msg.WarningBox(result.ToString(), "Save failed");
                            return;
                        }

                        transactionscope.Complete();
                        transactionscope.Dispose();
                        MyUtility.Msg.InfoBox("Update Successful!");
                    }
                    catch (Exception ex)
                    {
                        transactionscope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }

                transactionscope = null;
                #endregion

                // this.Query();
                if (dtGrid.AsEnumerable().Any(row => !row["selected"].EqualDecimal(1)))
                {
                    this.listControlBindingSource1.DataSource = dtGrid.AsEnumerable().Where(row => !row["selected"].EqualDecimal(1)).CopyToDataTable();
                }
                else
                {
                    this.listControlBindingSource1.DataSource = null;
                }

                this.HideWaitMessage();
            }
        }

        // Close
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
