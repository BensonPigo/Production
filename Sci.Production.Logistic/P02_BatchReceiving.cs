using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P02_BatchReceiving
    /// </summary>
    public partial class P02_BatchReceiving : Sci.Win.Subs.Base
    {
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private int allRecord = 0;
        private DataTable receiveDetailData;

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
        /// P02_BatchReceiving
        /// </summary>
        /// <param name="receiveDate">receiveDate</param>
        /// <param name="detailData">detailData</param>
        public P02_BatchReceiving(DateTime receiveDate, DataTable detailData)
        {
            this.InitializeComponent();
            this.Text = "Carton Receiving - Batch Receive (Receive Date - " + receiveDate.ToString("d") + ")";
            this.receiveDetailData = detailData;
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridCartonReceiving.CellValueChanged += (s, e) =>
                {
                    if (this.gridCartonReceiving.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                    {
                        this.numNoOfSelected.Value = this.gridCartonReceiving.GetCheckeds(this.col_chk).Count;
                    }
                };

            this.gridCartonReceiving.IsEditingReadOnly = false;
            this.gridCartonReceiving.DataSource = this.bindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridCartonReceiving)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10))
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        // Find
        private void BtnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtTransferClogNoStart) && MyUtility.Check.Empty(this.txtTransferClogNoEnd))
            {
                this.txtTransferClogNoStart.Focus();
                MyUtility.Msg.WarningBox("< Transfer Clog No > can not be empty!");
                return;
            }

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id1";
            sp1.Value = this.txtTransferClogNoStart.Text;
            sp2.ParameterName = "@id2";
            sp2.Value = this.txtTransferClogNoEnd.Text;
            sp3.ParameterName = "@mdivisionid";
            sp3.Value = Sci.Env.User.Keyword;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);

            string selectCommand1 = this.GridDataSQL(); // Grid Data
            string selectCommand2 = this.TTLCTNSQL(); // TTL CTN
            string selectCommand3 = this.ReceivedCTNSQL(); // Received CTN

            DataTable selectDataTable1, selectDataTable2, selectDataTable3;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, selectCommand1, cmds, out selectDataTable1))
            {
                if (selectDataTable1.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            this.AllRecord = 0;
            foreach (DataRow dr in selectDataTable1.Rows)
            {
                this.AllRecord = this.AllRecord + 1;
            }

            this.bindingSource1.DataSource = selectDataTable1;
            if (selectResult = DBProxy.Current.Select(null, selectCommand2, cmds, out selectDataTable2))
            {
                this.numTTLCTN.Text = selectDataTable2.Rows[0]["TTLCTN"].ToString();
            }

            if (selectResult = DBProxy.Current.Select(null, selectCommand3, cmds, out selectDataTable3))
            {
                this.numReceivedCTN.Text = selectDataTable3.Rows[0]["ReceivedCTN"].ToString();
            }

            this.numNotYetReceivedCTN.Value = this.numTTLCTN.Value - this.numReceivedCTN.Value;
            this.numNoOfSelected.Value = 0;
        }

        // 組撈Grid資料的SQL
        private string GridDataSQL()
        {
            return string.Format(
                @"
Select '' as ID, 0 as selected,'' as ClogLocationId,a.*,b.StyleID,b.SeasonID,b.BrandID,b.Customize1,b.CustPONo,b.BuyerDelivery,c.Alias 
from ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
	   from TransferToClog_Detail as a  , TransferToClog as b WITH (NOLOCK) 
	   where a.Id = b.ID
	   {0}
	   {1}
	   and b.MDivisionID = @mdivisionid) 
	  except 
	  (Select TransferToClogId, PackingListId, OrderId, CTNStartNo 
	   from ClogReceive_Detail 
	   where 1=1
	   {2}
	   {3})) as a 
left join Orders b WITH (NOLOCK) On a.OrderID = b.ID 
left join Country c WITH (NOLOCK) On b.Dest = c.ID",
                MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? string.Empty : " and a.Id >= @id1",
                MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? string.Empty : " and a.Id <= @id2",
                MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? string.Empty : " and TransferToClogId >= @id1",
                MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? string.Empty : " and TransferToClogId <= @id2");
        }

        // 組算TTL CTN的SQL
        private string TTLCTNSQL()
        {
            return string.Format(
                @"
Select count(TransferToClogId) as TTLCTN 
from ((Select a.Id as TransferToClogId, PackingListId, OrderId, CTNStartNo 
	   from TransferToClog_Detail as a, TransferToClog as b WITH (NOLOCK) 
	   where a.Id = b.ID
	   {0} 
	   {1}
	   and b.MDivisionID = @mdivisionid) 
	  except 
	  (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
	   from ClogReturn_Detail a, ClogReturn b WITH (NOLOCK) 
	   where a.Id = b.Id
	   and a.TransferToClogId >= '{2}' 
	   and a.TransferToClogId <= '{3}' 
	   and b.Status = 'Confirmed')) as result",
                MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? string.Empty : " and a.Id >= @id1",
                MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? string.Empty : " and a.Id <= @id2",
                MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? string.Empty : " and a.TransferToClogId >= @id1",
                MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? string.Empty : " and a.TransferToClogId <= @id2");
        }

        // 組算Received CTN的SQL
        private string ReceivedCTNSQL()
        {
            return string.Format(
                @"
Select count(TransferToClogId) as ReceivedCTN 
from ((Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
	   from ClogReceive_Detail a, ClogReceive b WITH (NOLOCK) 
	   where a.Id = b.Id 
	   {0}
	   {1}
	   and b.Status = 'Confirmed' 
	   and b.MDivisionID = @mdivisionid) 
	  except
	  (Select a.TransferToClogId, a.PackingListId, a.OrderId, a.CTNStartNo 
	   from ClogReturn_Detail a, ClogReturn b WITH (NOLOCK) 
	   where a.Id = b.Id
	   {0}
	   {1}
	   and b.Status = 'Confirmed')) as result",
                MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? string.Empty : " and a.TransferToClogId >= @id1",
                MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? string.Empty : " and a.TransferToClogId <= @id2");
        }

        // Update All Location
        private void BtnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            string location = this.txtcloglocationLocationNo.Text.Trim();
            int pos = this.bindingSource1.Position;     // 記錄目前指標位置
            DataTable dt = (DataTable)this.bindingSource1.DataSource;
            foreach (DataRow currentRecord in dt.Rows)
            {
                currentRecord["ClogLocationId"] = location;
            }

            this.bindingSource1.Position = pos;
            this.gridCartonReceiving.SuspendLayout();
            this.gridCartonReceiving.DataSource = null;
            this.gridCartonReceiving.DataSource = this.bindingSource1;
            this.bindingSource1.Position = pos;
            this.gridCartonReceiving.ResumeLayout();
        }

        // Save，將有勾選的資料回寫回上一層的Detail
        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridCartonReceiving.ValidateControl();
            this.bindingSource1.EndEdit();
            DataTable gridData = (DataTable)this.bindingSource1.DataSource;

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
                    DataRow[] findrow = this.receiveDetailData.Select(string.Format("TransferToClogId = '{0}' and PackingListId = '{1}' and OrderId = '{2}' and CTNStartNo = '{3}'", currentRow["TransferToClogId"].ToString(), currentRow["PackingListId"].ToString(), currentRow["OrderId"].ToString(), currentRow["CTNStartNo"].ToString()));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        this.receiveDetailData.ImportRow(currentRow);
                    }
                }
            }

            // 系統會自動有回傳值
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
