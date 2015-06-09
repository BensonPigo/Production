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

namespace Sci.Production.Packing
{
    public partial class P10 : Sci.Win.Tems.Input6
    {
        public P10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
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

        protected override Ict.DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            e.Details.Columns.Add("StyleID");
            e.Details.Columns.Add("SeasonID");
            e.Details.Columns.Add("BrandID");
            e.Details.Columns.Add("Customize1");
            e.Details.Columns.Add("CustPONo");
            e.Details.Columns.Add("Alias");
            e.Details.Columns.Add("BuyerDelivery", type: typeof(DateTime));
            e.Details.Columns.Add("ReceiveDate", type: typeof(DateTime));
            foreach (DataRow gridData in e.Details.Rows)
            {
                string selectCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,a.BuyerDelivery 
                                                                            from Orders a 
                                                                            left join Country b on b.ID = a.Dest  
                                                                            where a.ID = '{0}'", gridData["OrderID"].ToString());
                DataTable orderData, receiveData;
                DualResult dr;
                if (dr = DBProxy.Current.Select(null, selectCmd, out orderData))
                {
                    if (orderData.Rows.Count > 0)
                    {
                        gridData["StyleID"] = orderData.Rows[0]["StyleID"].ToString().Trim();
                        gridData["SeasonID"] = orderData.Rows[0]["SeasonID"].ToString().Trim();
                        gridData["BrandID"] = orderData.Rows[0]["BrandID"].ToString().Trim();
                        gridData["Customize1"] = orderData.Rows[0]["Customize1"].ToString().Trim();
                        gridData["CustPONo"] = orderData.Rows[0]["CustPONo"].ToString().Trim();
                        gridData["Alias"] = orderData.Rows[0]["Alias"].ToString().Trim();
                        gridData["BuyerDelivery"] = orderData.Rows[0]["BuyerDelivery"];
                    }
                }

                selectCmd = string.Format(@"Select a.ReceiveDate from ClogReceive a, ClogReceive_Detail b 
                                                                where b.TransferToClogId = '{0}' 
                                                                and b.PackingListId = '{1}' 
                                                                and b.OrderId = '{2}'  
                                                                and b.CTNStartNo = '{3}' and a.Id = b.Id and a.Encode = 1
                                                                ", gridData["ID"].ToString(), gridData["PackingListID"].ToString(), gridData["OrderID"].ToString(), gridData["CTNStartNo"].ToString());
                if (dr = DBProxy.Current.Select(null, selectCmd, out receiveData))
                {
                    if (receiveData.Rows.Count > 0)
                    {
                        gridData["ReceiveDate"] = receiveData.Rows[0]["ReceiveDate"];
                    }
                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        //新增資料時直接呼叫Form:P10_ImportData，P10_ImportData會將資料直接存進實體Table，回到Form:P10時再將剛新增的那筆資料寫入Browse的Cursor中
        protected override bool OnNewBefore()
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
        protected override bool OnDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            DataTable selectData;
            string sqlCmd = string.Format("select * from ClogReceive_Detail where TransferToClogID = '{0}'", dr["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out selectData);
            if (result)
            {
                if (selectData.Rows.Count > 0)
                {
                    MessageBox.Show("This record has received, can not be delete!");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Check transaction error.");
                return false;
            }
            return base.OnDeleteBefore();
        }

        //刪除更新PalcingList_Detail資料，資料刪除後也要更新PackingList_Detail的值
        protected override bool OnDeletePost()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            string sqlUpdatePackingList = string.Format(@"update PackingList_Detail 
                                                                                        set TransferToClogID = '', TransferDate = null 
                                                                                        where TransferToClogID = '{0}';", dr["ID"].ToString());
            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlUpdatePackingList)))
            {
                return false;
            }
            return base.OnDeletePost();
        }

        //存檔後要更新PalcingList_Detail資料，表身資料刪除後也要更新PackingList_Detail的值
        protected override bool OnSavePre()
        {
            DataTable t = (DataTable) detailgridbs.DataSource;

            string sqlUpdatePackingList = "";
            DualResult result;
            foreach (DataRow dr in t.Rows)
            {
                if (dr.RowState == DataRowState.Deleted)
                {
                    sqlUpdatePackingList = string.Format(@"update PackingList_Detail 
                                                                                       set TransferToClogID = '', TransferDate = null 
                                                                                       where ID = '{0}' and OrderID = '{1}' and CTNStartNo = '{2}';
                                                                                        ", dr["PackingListID", DataRowVersion.Original].ToString(), dr["OrderID", DataRowVersion.Original].ToString(), dr["CTNStartNo", DataRowVersion.Original].ToString());

                    if (!(result = DBProxy.Current.Execute(null, sqlUpdatePackingList)))
                    {
                        return false;
                    }
                }
            }
            return base.OnSavePre();
        }

        //Select Trans PO or Import From Barcode
        private void button1_Click(object sender, EventArgs e)
        {
            //如果此張單已經有一筆名細被Clog Receive就出訊息告知且不可使用此功能
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr["ReceiveDate"] != DBNull.Value)
                {
                    MessageBox.Show("This CTN has received, can not be import data!");
                    return;
                }
            }
            Sci.Production.Packing.P10_ImportData callNextForm = new Sci.Production.Packing.P10_ImportData(CurrentMaintain["ID"].ToString().Trim());
            callNextForm.ShowDialog(this);
            this.RenewData();
        }
    }
}
