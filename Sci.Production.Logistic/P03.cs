using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Transactions;
using Ict.Win;
using Ict;
using Sci.Data;
using Sci.Production.PublicPrg;

namespace Sci.Production.Logistic
{
    public partial class P03 : Sci.Win.Tems.Input6
    {
        protected DateTime returnDate;
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "MDivisionID = '" + Sci.Env.User.Keyword + "'";
            gridicon.Append.Visible = false;
            gridicon.Insert.Visible = false;
            InsertDetailGridOnDoubleClick = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"select distinct crd.*,o.StyleID,o.SeasonID,o.BrandID,o.Customize1,o.CustPONo,c.Alias,oqs.BuyerDelivery 
from ClogReturn_Detail crd
left join Orders o on o.ID = crd.OrderID
left join Country c on c.ID = o.Dest
left join PackingList_Detail pld on pld.ID = crd.PackingListId and pld.CTNQty = 1
left join Order_QtyShip oqs on oqs.Id = pld.OrderId and oqs.Seq = pld.OrderShipmodeSeq
where crd.ID = '{0}'", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //新增Import From Barcode按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Import From Barcode";
            btn.Click += new EventHandler(btn_Click);
            browsetop.Controls.Add(btn);
            btn.Size = new Size(165, 30);//預設是(80,30)
        }

        //Import From Barcode按鈕的Click事件
        private void btn_Click(object sender, EventArgs e)
        {
            Sci.Production.Logistic.P03_ImportFromBarCode callNextForm = new Sci.Production.Logistic.P03_ImportFromBarCode();
            DialogResult result = callNextForm.ShowDialog(this);
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.ReloadDatas();
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            ShowStatus();
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("TransferToClogId", header: "Trans. Slip#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("PackingListId", header: "Pack ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CustPONo", header: "P.O.#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Customize1", header: "Order#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Alias", header: "Destination#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //修改前檢查，如果已經Confirm了，就不可以被修改
        protected override bool ClickEditBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't modify!");
                return false;
            }
            return base.ClickEditBefore();
        }

        //刪除前檢查，如果已經Confirm了，就不可以被刪除
        protected override bool ClickDeleteBefore()
        {
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.WarningBox("Record is confirmed, can't delete!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        //新增時執行LOGISTIC->P02_InputDate
        protected override bool ClickNewBefore()
        {
            Sci.Production.Logistic.P02_InputDate callNextForm = new Sci.Production.Logistic.P02_InputDate("Input Return Date", "Return Date");
            DialogResult dr = callNextForm.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                returnDate = callNextForm.returnDate;
                return base.ClickNewBefore();
            }
            return false;
        }

        //新增帶入預設值後再執行LOGISTIC->P03_BatchReturn
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["ReturnDate"] = returnDate;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Status"] = "New";
            ShowStatus();
            CallBatchReturn();
        }

        //檢查表身不可以沒有資料
        protected override bool ClickSaveBefore()
        {
            DataRow[] detailData = ((DataTable)detailgridbs.DataSource).Select();
            if (detailData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!");
                return false;
            }
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "CN", "ClogReturn", Convert.ToDateTime(CurrentMaintain["ReturnDate"].ToString()), 2, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            return base.ClickSaveBefore();
        }

        protected override bool ClickPrint()
        {

            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                return false;
            }

            string sqlCmd = string.Format(@"with CTNData
as
(select distinct cd.ID,cd.TransferToClogId,cd.PackingListId,cd.OrderId,cd.CTNStartNo,isnull(pd.Seq,'999999') as Seq
 from ClogReturn_Detail cd
 left join PackingList_Detail pd on pd.ID = cd.PackingListId and pd.CTNStartNo = cd.CTNStartNo and pd.CTNQty >=1
 where cd.ID = '{0}'
),
CTNDataXML
as
(select distinct c.ID,c.TransferToClogId,c.PackingListId,c.OrderId, (select CTNStartNo+', ' from CTNData c1 where c1.ID = c.ID and c1.TransferToClogId = c.TransferToClogId and c1.PackingListId = c.PackingListId and c1.OrderId = c.OrderId order by c1.Seq for XML Path('')) as CtnNo
 from CTNData c
)
select distinct cd.ID,cd.TransferToClogId,cd.PackingListId,cd.OrderId,isnull(o.CustPONo,'') as CustPONo,isnull(o.Customize1,'') as Customize1,isnull(c.Alias,'') as Alias,cx.CtnNo
	,(select count(cd1.ID) from ClogReturn_Detail cd1 where cd1.ID = cd.ID and cd1.TransferToClogId = cd.TransferToClogId and cd1.PackingListId = cd.PackingListId and cd1.OrderId = cd.OrderId) as TtlCtn
	,(select ClogLocationId+', ' from (select distinct cr.ClogLocationId from ClogReceive_Detail cr where cr.TransferToClogId = cd.TransferToClogId and cr.PackingListId = cd.PackingListId and cr.OrderId = cd.OrderId) a for XML Path('')) as Location
from ClogReturn_Detail cd
left join Orders o on cd.OrderId = o.ID
left join Country c on o.Dest = c.ID
left join CTNDataXML cx on cx.ID = cd.ID and cx.TransferToClogId = cd.TransferToClogId and cx.PackingListId = cd.PackingListId and cx.OrderId = cd.OrderId
where cd.ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DataTable ExcelData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out ExcelData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail!!\r\n" + result.ToString());
                return false;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "Logistic_P03.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Cells[2, 2] = MyUtility.Convert.GetString(CurrentMaintain["ID"]);
            worksheet.Cells[2, 4] = Convert.ToDateTime(CurrentMaintain["ReturnDate"]).ToString("d");

            int intRowsStart = 4;
            int dataRowCount = ExcelData.Rows.Count;
            int rownum = 0;
            object[,] objArray = new object[1, 9];
            for (int i = 0; i < dataRowCount; i++)
            {
                DataRow dr = ExcelData.Rows[i];
                rownum = intRowsStart + i;
                objArray[0, 0] = dr["TransferToClogId"];
                objArray[0, 1] = dr["PackingListId"];
                objArray[0, 2] = dr["OrderId"];
                objArray[0, 3] = dr["CustPONo"];
                objArray[0, 4] = dr["Customize1"];
                objArray[0, 5] = dr["Alias"];
                objArray[0, 6] = dr["TtlCtn"];
                objArray[0, 7] = MyUtility.Convert.GetString(dr["CtnNo"]).Substring(0, MyUtility.Convert.GetString(dr["CtnNo"]).Length - 2);
                objArray[0, 8] = MyUtility.Convert.GetString(dr["Location"]).Substring(0, MyUtility.Convert.GetString(dr["Location"]).Length - 2);

                worksheet.Range[String.Format("A{0}:I{0}", rownum)].Value2 = objArray;
            }
            excel.Visible = true;
            return base.ClickPrint();
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string sqlCmd;
            DualResult result;
            DataTable selectDate;

            #region update ClogReturn & PackingList_Detail data
            sqlCmd = string.Format(@"select PackingListId,CTNStartNo
                                                        from ClogReturn_Detail 
                                                        where ID = '{0}'", CurrentMaintain["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out selectDate);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Connection fail!\r\n"+result.ToString());
                return;
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    IList<string> updateCmds = new List<string>();
                    foreach (DataRow eachRow in selectDate.Rows)
                    {
                        updateCmds.Add(string.Format(@"update PackingList_Detail 
                                             set ClogReturnID = '{0}', ReturnDate = '{1}', TransferToClogID = '', TransferDate = null, ClogReceiveID = '', ReceiveDate = null, ClogLocationId = '' 
                                             where ID = '{2}' and CTNStartNo = '{3}';", CurrentMaintain["ID"].ToString(), Convert.ToDateTime(CurrentMaintain["ReturnDate"].ToString()).ToString("d"),
                                                                                                         eachRow["PackingListId"].ToString(), eachRow["CTNStartNo"].ToString()));
                    }
                    updateCmds.Add(string.Format("update ClogReturn set Status = 'Confirmed', EditName = '{0}', EditDate = '{1}' where ID = '{2}';", Sci.Env.User.UserID, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), CurrentMaintain["ID"].ToString()));
                    result = DBProxy.Current.Executes(null, updateCmds);

                    #region Update Orders的資料
                    DataTable selectData;
                    sqlCmd = string.Format("select OrderID from ClogReturn_Detail where ID = '{0}' group by OrderID", CurrentMaintain["ID"].ToString());
                    DualResult result1 = DBProxy.Current.Select(null, sqlCmd, out selectData);
                    if (!result1)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Select update orders data fail!\r\n" + result1.ToString());
                        return;
                    }

                    DualResult prgResult = Prgs.UpdateOrdersCTN(selectData);
                    if (!prgResult)
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Update orders data fail!\r\n" + prgResult.ToString());
                        return;
                    }

                    #endregion

                    if (result)
                    {
                        transactionScope.Complete();
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Confirm failed !\r\n" + result.ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    ShowErr("Confirm transaction error.", ex);
                    return;
                }
            }
            #endregion

            RenewData();
            OnDetailEntered();
            EnsureToolbarExt();
        }

        //Batch Return，執行LOGISTIC->P03_BatchReturn
        private void button2_Click(object sender, EventArgs e)
        {
            CallBatchReturn();
        }

        //Status顯示
        private void ShowStatus()
        {
            this.label1.Text = CurrentMaintain["Status"].ToString();
        }

        //呼叫Batch Return
        private void CallBatchReturn()
        {
            Sci.Production.Logistic.P03_BatchReturn callNextForm = new Sci.Production.Logistic.P03_BatchReturn(Convert.ToDateTime(CurrentMaintain["ReturnDate"].ToString()), (DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }
    }
}
