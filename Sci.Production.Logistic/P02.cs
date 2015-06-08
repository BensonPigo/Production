using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Logistic
{
    public partial class P02 : Sci.Win.Tems.Input6
    {
        protected DateTime receiveDate;
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "FactoryID = '" + Sci.Env.User.Factory + "'";
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //新增Import From Barcode按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Import From Barcode";
            btn.Click += new EventHandler(btn_Click);
            browsetop.Controls.Add(btn);
            btn.Size = new Size(165,30);//預設是(80,30)
        }

        //Import From Barcode按鈕的Click事件
        private void btn_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P02_ImportFromBarCode callNextForm = new Sci.Production.Logistic.P02_ImportFromBarCode();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.ReloadDatas();
            }
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("TransferToClogId", header: "Trans. Slip#", width: Widths.AnsiChars(13),iseditingreadonly : true)
                .Text("PackingListId", header: "Pack Id", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.AnsiChars(10));
        }

        //加工資料
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            e.Details.Columns.Add("StyleID");
            e.Details.Columns.Add("SeasonID");
            e.Details.Columns.Add("BrandID");
            e.Details.Columns.Add("Customize1");
            e.Details.Columns.Add("CustPONo");
            e.Details.Columns.Add("Alias");
            e.Details.Columns.Add("BuyerDelivery", type: typeof(DateTime));
            foreach (DataRow gridData in e.Details.Rows)
            {
                string selectCmd = string.Format(@"select a.StyleID,a.SeasonID,a.BrandID,a.Customize1,a.CustPONo,b.Alias,a.BuyerDelivery 
                                                                           from Orders a 
                                                                           left join Country b on b.ID = a.Dest 
                                                                           where a.ID = '{0}'", gridData["OrderID"].ToString());
                DataTable orderData;
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
            }
            return base.OnRenewDataDetailPost(e);
        }

        //修改前檢查，如果已經Encode了，就不可以被修改
        protected override bool OnEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Encode"].ToString() == "True")
            {
                MessageBox.Show("Record is encoded, can't modify!");
                return false;
            }
            return base.OnEditBefore();
        }

        //刪除前檢查，如果已經Encode了，就不可以被刪除
        protected override bool OnDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Encode"].ToString() == "True")
            {
                MessageBox.Show("Record is encoded, can't delete!");
                return false;
            }
            return base.OnDeleteBefore();
        }

        //新增時執行LOGISTIC->P02_InputDate
        protected override bool OnNewBefore()
        {
            Sci.Production.Logistic.P02_InputDate callNextForm = new Sci.Production.Logistic.P02_InputDate("Input Receive Date","Receive Date");
            DialogResult dr = callNextForm.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                receiveDate = callNextForm.returnDate;
                return base.OnNewBefore();
            }
            return false;
        }

        //新增帶入預設值後再執行LOGISTIC->P02_BatchReceiving
        protected override void OnNewAfter()
        {
            base.OnNewAfter();
            CurrentMaintain["ReceiveDate"] = receiveDate;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            Sci.Production.Logistic.P02_BatchReceiving callNextForm = new Sci.Production.Logistic.P02_BatchReceiving(Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()), (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        //檢查表身不可以沒有資料
        protected override bool OnSaveBefore()
        {
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select();
            if (detailData.Length == 0)
            {
                MessageBox.Show("Detail can't empty!");
                return false;
            }
            if (IsDetailInserting)
            {
                string id = myUtility.GetID(ProductionEnv.Keyword + "CR", "ClogReceive", Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()), 2, "Id", null);
                if (myUtility.Empty(id))
                {
                    MessageBox.Show("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.OnSaveBefore();
        }

        //Batch Receive，執行LOGISTIC->P02_BatchReceiving
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P02_BatchReceiving callNextForm = new Sci.Production.Logistic.P02_BatchReceiving(Convert.ToDateTime(CurrentMaintain["ReceiveDate"].ToString()),(DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}

