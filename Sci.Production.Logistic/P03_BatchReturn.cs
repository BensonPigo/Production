using System;
using System.Data;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P03_BatchReturn
    /// </summary>
    public partial class P03_BatchReturn : Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private int allRecord = 0;
        private DataTable returnDetailData;

        /// <summary>
        /// AllRecord
        /// </summary>
        protected int AllRecord
        {
            get
            {
                return this.allRecord;
            }

            set
            {
                this.allRecord = value;
            }
        }

        /// <summary>
        /// P03_BatchReturn
        /// </summary>
        /// <param name="receiveDate">receiveDate</param>
        /// <param name="detailData">detailData</param>
        public P03_BatchReturn(DateTime receiveDate, DataTable detailData)
        {
            this.InitializeComponent();
            this.Text = "Carton Return - Batch Return (Return Date - " + receiveDate.ToString("d") + ")";
            this.returnDetailData = detailData;
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridCartonReturn.DataSource = this.listControlBindingSource1;
            this.gridCartonReturn.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridCartonReturn)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("PackingListId", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Date("ReceiveDate", header: "Receive Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSPNo.Text) && MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.txtPONo.Text) && MyUtility.Check.Empty(this.dateReceiveDate.Value))
            {
                this.txtSPNo.Focus();
                MyUtility.Msg.WarningBox("< SP# > or < Pack ID > or < Receive Date > or < P.O. No. > can not be empty!");
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();

            sqlCmd.Append(@"select '' as ID, 0 as selected, b.TransferToClogId, b.ClogLocationId,b.ReceiveDate, b.ID as PackingListId, b.OrderId,b.CTNStartNo,a.StyleID,a.SeasonID, a.BrandID, a.CustPONo, a.Customize1, a.BuyerDelivery, c.Alias
                                           from Orders a WITH (NOLOCK) , PackingList_Detail b WITH (NOLOCK) , Country c WITH (NOLOCK) 
                                           where a.ID = b.OrderID
                                           and b.CTNQty = 1
                                           and b.ReceiveDate is not null
                                           and a.Dest = c.ID");
            if (!MyUtility.Check.Empty(this.txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(" and a.ID = '{0}'", this.txtSPNo.Text.Trim()));
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                sqlCmd.Append(string.Format(" and b.ID = '{0}'", this.txtPackID.Text.Trim()));
            }

            if (!MyUtility.Check.Empty(this.dateReceiveDate.Value))
            {
                sqlCmd.Append(string.Format(" and b.ReceiveDate = '{0}'", Convert.ToDateTime(this.dateReceiveDate.Text).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.txtPONo.Text))
            {
                sqlCmd.Append(string.Format(" and a.CustPONo = '{0}'", this.txtPONo.Text.Trim()));
            }

            DataTable selectDataTable1;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, sqlCmd.ToString(), out selectDataTable1))
            {
                if (selectDataTable1.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.listControlBindingSource1.DataSource = selectDataTable1;
        }

        // Save，將有勾選的資料回寫回上一層的Detail
        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridCartonReturn.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            if (gridData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data!");
                return;
            }

            DataRow[] dr = gridData.Select("Selected = 1");
            if (dr.Length > 0)
            {
                foreach (DataRow currentRow in dr)
                {
                    DataRow[] findrow = this.returnDetailData.Select(string.Format("TransferToClogId = '{0}' and PackingListId = '{1}' and OrderId = '{2}' and CTNStartNo = '{3}'", currentRow["TransferToClogId"].ToString(), currentRow["PackingListId"].ToString(), currentRow["OrderId"].ToString(), currentRow["CTNStartNo"].ToString()));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        this.returnDetailData.ImportRow(currentRow);
                    }
                }
            }

            // 系統會自動有回傳值
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
