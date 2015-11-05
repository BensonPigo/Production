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
using Sci.Production.PublicPrg;

namespace Sci.Production.Packing
{
    public partial class P10 : Sci.Win.Tems.Input6
    {
        DataTable detailOrderID;
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select td.*, o.StyleID,o.SeasonID,o.BrandID,o.Customize1,o.CustPONo,c.Alias,oqs.BuyerDelivery,cr.ReceiveDate
from TransferToClog_Detail td
left join Orders o on o.ID = td.OrderID
left join Country c on c.ID = o.Dest
left join PackingList pl on pl.ID = td.PackingListId
left join Order_QtyShip oqs on oqs.Id = pl.OrderId and oqs.Seq = pl.OrderShipmodeSeq
left join ClogReceive_Detail crd on crd.TransferToClogId = td.Id and crd.PackingListId = td.PackingListID and crd.OrderId = td.OrderID and crd.CTNStartNo = td.CTNStartNo
left join ClogReceive cr on cr.Status = 'Confirmed' and cr.ID = crd.ID
where td.Id = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly:true)
                .Text("PackingListID", header: "PackId", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("ReceiveDate", header: "Clog CFM", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //表身Grid的Delete
        protected override void OnDetailGridRemoveClick()
        {
            //檢查此筆記錄是否已被Clog Receive，若是則出訊息告知且無法刪除
            string sqlCmd = string.Format(@"select a.Status 
                                                                  from ClogReceive a, ClogReceive_Detail b
                                                                  where a.ID = b.ID
                                                                  and b.TransferToClogId = '{0}'
                                                                  and b.PackingListId = '{1}'
                                                                  and b.OrderId = '{2}'
                                                                  and b.CTNStartNo = '{3}'", CurrentDetailData["ID"].ToString(), CurrentDetailData["PackingListId"].ToString(), CurrentDetailData["OrderId"].ToString(), CurrentDetailData["CTNStartNo"].ToString());
            DataRow receivdData;
            if (MyUtility.Check.Seek(sqlCmd, out receivdData))
            {
                if (receivdData["Status"].ToString() == "Confirmed")
                {
                    MyUtility.Msg.WarningBox("This carton has receive record, can't delete!");
                    return;
                }
            }
            base.OnDetailGridRemoveClick();
        }

        //新增資料時直接呼叫Form:P10_ImportData，P10_ImportData會將資料直接存進實體Table，回到Form:P10時再將剛新增的那筆資料寫入Browse的Cursor中
        protected override bool ClickNewBefore()
        {
            Sci.Production.Packing.P10_ImportData callNextForm = new Sci.Production.Packing.P10_ImportData("");
            DialogResult dr = callNextForm.ShowDialog(this);

            //當Form:P10_ImportData是按Save時，要新增一筆資料進Cursor
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                if (CurrentDataRow != null)
                {
                    DataRow newrow = CurrentDataRow.Table.NewRow();
                    newrow["ID"] = callNextForm.newID;
                    newrow["TransferDate"] = callNextForm.transDate;
                    newrow["FactoryID"] = Sci.Env.User.Factory;

                    CurrentDataRow.Table.Rows.Add(newrow);
                    newrow.AcceptChanges();
                    gridbs.MoveLast();  //因為新增資料一定會在最後一筆，所以直接把指標移至最後一筆
                }
            }
            return false;
        }

        //刪除前檢查，如果表身有任一筆資料已被Clog Receive的話，就不可以被刪除
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            DataTable selectData;
            string sqlCmd = string.Format("select * from ClogReceive_Detail where TransferToClogID = '{0}'", dr["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (result)
            {
                if (selectData.Rows.Count > 0)
                {
                    MyUtility.Msg.WarningBox("This record has received, can not be delete!");
                    return false;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Check transaction error.");
                return false;
            }

            //先記錄要被Delete的表身資料OrderID
            sqlCmd = string.Format("select OrderID from TransferToClog_Detail where ID = '{0}' group by OrderID", dr["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out detailOrderID);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query detail data fail!");
            }

            return base.ClickDeleteBefore();
        }

        //刪除更新PalcingList_Detail資料，資料刪除後也要更新PackingList_Detail的值
        protected override bool ClickDeletePost()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            string sqlUpdatePackingList = string.Format(@"update PackingList_Detail 
                                                                                        set TransferToClogID = '', 
                                                                                              TransferDate = null, 
                                                                                              ClogReturnID = (select max(a.ID)
                                                                                                                         from ClogReturn a, ClogReturn_Detail b 
					                                                                                                     where a.id = b.id 
					                                                                                                     and b.PackingListId = PackingList_Detail.ID
					                                                                                                     and b.OrderId = PackingList_Detail.OrderID
					                                                                                                     and b.CTNStartNo = PackingList_Detail.CTNStartNo), 
                                                                                              ReturnDate = (select ReturnDate
                                                                                                                      from ClogReturn 
                                                                                                                      where ID = (select max(a.ID) 
                                                                                                                                          from ClogReturn a, ClogReturn_Detail b 
                                                                                                                                          where a.id = b.id 
                                                                                                                                          and b.PackingListId = PackingList_Detail.ID
                                                                                                                                          and b.OrderId = PackingList_Detail.OrderID
                                                                                                                                          and b.CTNStartNo = PackingList_Detail.CTNStartNo))
                                                                                        where TransferToClogID = '{0}';", dr["ID"].ToString());
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlUpdatePackingList)))
            {
                return false;
            }
            return base.ClickDeletePost();
        }

        //刪除後要Update Orders的資料
        protected override void ClickDeleteAfter()
        {
            if (detailOrderID.Rows.Count > 0)
            {
                bool prgResult, lastResult = false;
                foreach (DataRow currentRow in detailOrderID.Rows)
                {
                    prgResult = Prgs.UpdateOrdersCTN(currentRow["OrderID"].ToString());
                    if (!prgResult)
                    {
                        lastResult = true;
                    }

                }
                if (lastResult)
                {
                    MyUtility.Msg.WarningBox("Update orders data fail!");
                }
            }
            base.ClickDeleteAfter();
        }

        //存檔後要更新PalcingList_Detail資料，表身資料刪除後也要更新PackingList_Detail的值
        protected override DualResult ClickSavePre()
        {
            DataTable t = (DataTable)detailgridbs.DataSource;

            string sqlUpdatePackingList = "";
            DualResult result;
            foreach (DataRow dr in t.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    sqlUpdatePackingList = string.Format(@"update PackingList_Detail 
                                                                                       set TransferToClogID = '', 
                                                                                             TransferDate = null,
                                                                                             ClogReturnID = (select max(a.ID)
                                                                                                                        from ClogReturn a, ClogReturn_Detail b 
					                                                                                                    where a.id = b.id 
					                                                                                                    and b.PackingListId = PackingList_Detail.ID
					                                                                                                    and b.OrderId = PackingList_Detail.OrderID
					                                                                                                    and b.CTNStartNo = PackingList_Detail.CTNStartNo), 
                                                                                             ReturnDate = (select ReturnDate
                                                                                                                     from ClogReturn 
                                                                                                                     where ID = (select max(a.ID) 
                                                                                                                                         from ClogReturn a, ClogReturn_Detail b 
                                                                                                                                         where a.id = b.id 
                                                                                                                                         and b.PackingListId = PackingList_Detail.ID
                                                                                                                                         and b.OrderId = PackingList_Detail.OrderID
                                                                                                                                         and b.CTNStartNo = PackingList_Detail.CTNStartNo))
                                                                                       where ID = '{0}' and OrderID = '{1}' and CTNStartNo = '{2}';
                                                                                        ", dr["PackingListID", DataRowVersion.Original].ToString(), dr["OrderID", DataRowVersion.Original].ToString(), dr["CTNStartNo", DataRowVersion.Original].ToString());

                    if (!(result = DBProxy.Current.Execute(null, sqlUpdatePackingList)))
                    {
                        return result;
                    }
                }
            }
            return Result.True;
        }

        //存檔後要Update Orders的資料
        protected override void ClickSaveAfter()
        {
            //Update Orders的資料
            DataTable selectData;
            string sqlCmd = string.Format("select OrderID from TransferToClog_Detail where ID = '{0}' group by OrderID", CurrentMaintain["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Update orders data fail!");
            }

            bool prgResult, lastResult = false;
            foreach (DataRow currentRow in selectData.Rows)
            {
                prgResult = Prgs.UpdateOrdersCTN(currentRow["OrderID"].ToString());
                if (!prgResult)
                {
                    lastResult = true;
                }

            }
            if (lastResult)
            {
                MyUtility.Msg.WarningBox("Update orders data fail!");
            }
            base.ClickSaveAfter();
        }

        //Select Trans PO or Import From Barcode
        private void button1_Click(object sender, EventArgs e)
        {
            //如果此張單已經有一筆名細被Clog Receive就出訊息告知且不可使用此功能
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr["ReceiveDate"] != DBNull.Value)
                {
                    MyUtility.Msg.WarningBox("This CTN has received, can not be import data!");
                    return;
                }
            }
            Sci.Production.Packing.P10_ImportData callNextForm = new Sci.Production.Packing.P10_ImportData(CurrentMaintain["ID"].ToString().Trim());
            callNextForm.ShowDialog(this);
            this.RenewData();
        }
    }
}
