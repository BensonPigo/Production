﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Transactions;
using Sci;

namespace Sci.Production.Logistic
{
    public partial class P02_BatchReceiving : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected int allRecord = 0;
        DataTable receiveDetailData;
        public P02_BatchReceiving(DateTime receiveDate, DataTable detailData)
        {
            InitializeComponent();
            this.Text = "Carton Receiving - Batch Receive (Receive Date - " + receiveDate.ToString("d") + ")";
            receiveDetailData = detailData;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.gridCartonReceiving.CellValueChanged += (s, e) =>
                {
                    if (gridCartonReceiving.Columns[e.ColumnIndex].Name == col_chk.Name)
                    {
                        this.numNoOfSelected.Value = gridCartonReceiving.GetCheckeds(col_chk).Count;
                    }
                };

            this.gridCartonReceiving.IsEditingReadOnly = false;
            this.gridCartonReceiving.DataSource = bindingSource1;

            Helper.Controls.Grid.Generator(this.gridCartonReceiving)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
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

        //Find
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtTransferClogNoStart) && MyUtility.Check.Empty(this.txtTransferClogNoEnd))
            {
                MyUtility.Msg.WarningBox("< Transfer Clog No > can not be empty!");
                this.txtTransferClogNoStart.Focus();
                return;
            }

            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter();
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@id1";
            sp1.Value = txtTransferClogNoStart.Text;
            sp2.ParameterName = "@id2";
            sp2.Value = txtTransferClogNoEnd.Text;
            sp3.ParameterName = "@mdivisionid";
            sp3.Value = Sci.Env.User.Keyword;

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);

            string selectCommand1 = GridDataSQL(); //Grid Data
            string selectCommand2 = TTLCTNSQL(); //TTL CTN
            string selectCommand3 = ReceivedCTNSQL(); //Received CTN
            
            DataTable selectDataTable1, selectDataTable2, selectDataTable3;
            DualResult selectResult;
            if (selectResult = DBProxy.Current.Select(null, selectCommand1, cmds, out selectDataTable1))
            {
                if (selectDataTable1.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }

            allRecord = 0;
            foreach (DataRow dr in selectDataTable1.Rows)
            {
                allRecord = allRecord + 1;
            }
            bindingSource1.DataSource = selectDataTable1;
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

        //組撈Grid資料的SQL
        private string GridDataSQL()
        {
            return string.Format(@"
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
 MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? "" : " and a.Id >= @id1",
 MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? "" : " and a.Id <= @id2",
 MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? "" : " and TransferToClogId >= @id1",
 MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? "" : " and TransferToClogId <= @id2");
        }

        //組算TTL CTN的SQL
        private string TTLCTNSQL()
        {
            return string.Format(@"
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
 MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? "" : " and a.Id >= @id1",
 MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? "" : " and a.Id <= @id2", 
 MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? "" : " and a.TransferToClogId >= @id1",
 MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? "" : " and a.TransferToClogId <= @id2");
        }

        //組算Received CTN的SQL
        private string ReceivedCTNSQL()
        {
            return string.Format(@"
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
MyUtility.Check.Empty(this.txtTransferClogNoStart.Text.Trim()) ? "" : " and a.TransferToClogId >= @id1",
 MyUtility.Check.Empty(this.txtTransferClogNoEnd.Text.Trim()) ? "" : " and a.TransferToClogId <= @id2");
        }

        //Update All Location
        private void btnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            string location = this.txtcloglocationLocationNo.Text.Trim();
            int pos = this.bindingSource1.Position;     //記錄目前指標位置
            DataTable dt = (DataTable)bindingSource1.DataSource;
            foreach (DataRow currentRecord in dt.Rows)
            {
                currentRecord["ClogLocationId"] = location;
            }
            this.bindingSource1.Position = pos;
            gridCartonReceiving.SuspendLayout();
            this.gridCartonReceiving.DataSource = null;
            this.gridCartonReceiving.DataSource = bindingSource1;
            this.bindingSource1.Position = pos;
            gridCartonReceiving.ResumeLayout();
        }

        //Save，將有勾選的資料回寫回上一層的Detail
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.gridCartonReceiving.ValidateControl();
            bindingSource1.EndEdit();
            DataTable gridData = (DataTable)bindingSource1.DataSource;

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
                    DataRow[] findrow = receiveDetailData.Select(string.Format("TransferToClogId = '{0}' and PackingListId = '{1}' and OrderId = '{2}' and CTNStartNo = '{3}'", currentRow["TransferToClogId"].ToString(), currentRow["PackingListId"].ToString(), currentRow["OrderId"].ToString(), currentRow["CTNStartNo"].ToString()));
                    if (findrow.Length == 0)
                    {
                        currentRow.AcceptChanges();
                        currentRow.SetAdded();
                        receiveDetailData.ImportRow(currentRow);
                    }
                }
            }
            //系統會自動有回傳值
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
