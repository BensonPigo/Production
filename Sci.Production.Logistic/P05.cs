using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Logistic
{
    public partial class P05 : Sci.Win.Tems.QueryForm
    {
        public P05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        DataTable gridData;
        string selectDataTable_DefaultView_Sort = "";
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //Grid設定
            this.gridReceiveDate.IsEditingReadOnly = false;
            this.gridReceiveDate.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridReceiveDate)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Date("ReceiveDate", header: "Receive Date", iseditable: false)
                .Text("PackingListID", header: "Pack ID", width: Widths.Auto(), iseditable: false)
                .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditable: false)
                .Text("seq", header: "SEQ", width: Widths.Auto(), iseditable: false)
                .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditable: false)
                .Text("StyleID", header: "Style#", width: Widths.Auto(), iseditable: false)
                .Text("BrandID", header: "Brand", width: Widths.Auto(), iseditable: false)
                .Text("Customize1", header: "Order#", width: Widths.Auto(), iseditable: false)
                .Text("CustPONo", header: "PO No.", width: Widths.Auto(), iseditable: false)
                .Text("Dest", header: "Destination", width: Widths.Auto(), iseditable: false)
                .Text("FactoryID", header: "Factory", width: Widths.Auto(), iseditable: false)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.Auto(), iseditable: false)
                .CellClogLocation("ClogLocationId", header: "Location No", width: Widths.Auto(), iseditable: false)
                .DateTime("AddDate", header: "Create Date", width: Widths.Auto(), iseditable: false);

            // 增加CTNStartNo 有中文字的情況之下 按照我們希望的順序排
            int RowIndex = 0;
            int ColumIndex = 0;
            gridReceiveDate.CellClick += (s, e) =>
            {
                RowIndex = e.RowIndex;
                ColumIndex = e.ColumnIndex;
            };

            gridReceiveDate.Sorted += (s, e) =>
            {

                if ((RowIndex == -1) & (ColumIndex == 4))
                {

                    listControlBindingSource1.DataSource = null;

                    if (selectDataTable_DefaultView_Sort == "DESC")
                    {
                        gridData.DefaultView.Sort = "rn1 DESC";
                        selectDataTable_DefaultView_Sort = "";
                    }
                    else
                    {
                        gridData.DefaultView.Sort = "rn1 ASC";
                        selectDataTable_DefaultView_Sort = "DESC";
                    }
                    listControlBindingSource1.DataSource = gridData;
                    return;
                }


            };


            //

        }

        //Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.ShowWaitMessage("Data Loading...");
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select  1 as selected
        , ReceiveDate
        , PackingListID
        , OrderID
        , Seq
        , CTNStartNo
        , StyleID
        , BrandID
        , Customize1
        , CustPONo
        , Dest
        , FactoryID
        , BuyerDelivery
        , [SCI Delivery]
        , Qty
        , [TTQty] = isnull (TTQty, 0)
        , [Rec Qty]
        , [Ret Qty]
        , [Bal Qty] = CASE 
                          WHEN ([TTQty] - [Rec Qty] + [Ret Qty]) is null then 0 
                          else ([TTQty] - [Rec Qty] + [Ret Qty]) 
                      END
        , [%] = CASE 
                    WHEN TTQty = 0 then 0 
                    WHEN ([TTQty] - [Rec Qty] + [Ret Qty]) is null then 0 
                    ELSE round(1- (([TTQty] - [Rec Qty] + [Ret Qty]) / TTQty),2) * 100 
                END
        , ClogLocationId
        , AddDate
        , rn = ROW_NUMBER() over(order by Id,OrderID,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
        , rn1 = ROW_NUMBER() over(order by TRY_CONVERT(int, CTNStartNo) ,(RIGHT(REPLICATE('0', 6) + rtrim(ltrim(CTNStartNo)), 6)))
from (
    select  cr.ReceiveDate
            , cr.PackingListID
            , cr.OrderID
            , oq.Seq
            , cr.CTNStartNo
            , isnull(o.StyleID,'') as StyleID
            , isnull(o.BrandID,'') as BrandID
            , isnull(o.Customize1,'') as Customize1
            , isnull(o.CustPONo,'') as CustPONo
            , isnull(c.Alias,'') as Dest
            , isnull(o.FactoryID,'') as FactoryID
            , oq.BuyerDelivery
            , [SCI Delivery] = o.SciDelivery
            , o.Qty
            , [TTQty] = (select sum (isnull(pld.CTNQty, 0)) 
                         from PackingList_Detail pld
                         where  pld.ID = cr.PackingListID 
                                and pld.OrderID = cr.OrderID)
            , [Rec Qty] = ( select count(*) 
                            from ClogReceive CReceive
                            where   CReceive.PackingListID = pd.ID 
                                    and CReceive.OrderID = pd.OrderID)
            , [Ret Qty] = ( select count(*) 
                            from ClogReturn CReturn
                            where   CReturn.PackingListID = pd.ID 
                                    and CReturn.OrderID = pd.OrderID)
            , cr.ClogLocationId
            , cr.AddDate
            , pd.Id
    from ClogReceive cr WITH (NOLOCK) 
    left join Orders o WITH (NOLOCK) on cr.OrderID =  o.ID
    left join Country c WITH (NOLOCK) on o.Dest = c.ID
    left join PackingList_Detail pd WITH (NOLOCK) on pd.ID = cr.PackingListID 
                                                     and pd.OrderID = cr.OrderID 
                                                     and pd.CTNStartNo = cr.CTNStartNo 
                                                     and pd.CTNQty > 0
    left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID 
                                                and oq.Seq = pd.OrderShipmodeSeq
    where   cr.MDivisionID = '{0}'", Sci.Env.User.Keyword));

            if (!MyUtility.Check.Empty(dateReceiveDate.Value1))
            {
                sqlCmd.Append(string.Format(@" 
            and cr.ReceiveDate >= '{0}'", Convert.ToDateTime(dateReceiveDate.Value1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(dateReceiveDate.Value2))
            {
                sqlCmd.Append(string.Format(@" 
            and cr.ReceiveDate <= '{0}'", Convert.ToDateTime(dateReceiveDate.Value2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(txtPackID.Text))
            {
                sqlCmd.Append(string.Format(@" 
            and cr.PackingListID = '{0}'", MyUtility.Convert.GetString(txtPackID.Text)));
            }
            if (!MyUtility.Check.Empty(txtSPNo.Text))
            {
                sqlCmd.Append(string.Format(@" 
            and cr.OrderID = '{0}'", MyUtility.Convert.GetString(txtSPNo.Text)));
            }
            sqlCmd.Append(@"
) X order by rn");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
            }
            listControlBindingSource1.DataSource = gridData;
            gridReceiveDate.AutoResizeColumns();
            this.HideWaitMessage();
        }

        //Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //To Excel
        private void btnToExcel_Click(object sender, EventArgs e)
        {       
            DataTable ExcelTable = (DataTable)listControlBindingSource1.DataSource;
            DataTable PrintDT = ExcelTable.Clone();

            //判斷是否有資料
            if (ExcelTable == null || ExcelTable.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            //如果沒勾選資料,會跳訊息
            foreach (DataRow Dr in ExcelTable.Rows)
            {
                if (Dr["Selected"].EqualString("1"))
                {
                    PrintDT.ImportRow(Dr);
                }
            }

            if (PrintDT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Checked item first before click ToExcel");
                return;
            }

            this.ShowWaitMessage("Excel Processing...");

            /*
             * 輸出的資料中
             * 1. Selected，此欄位是為了判斷是否需要列印
             * 2. rn，此欄位是為了 SQL 排序
             * 3. rn1，同上
             */
            PrintDT.Columns.Remove("Selected");
            PrintDT.Columns.Remove("rn");
            PrintDT.Columns.Remove("rn1");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Logistic_P05.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(PrintDT, "", "Logistic_P05.xltx", 4, false, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            int r = ((DataTable)listControlBindingSource1.DataSource).Rows.Count;
            objSheets.get_Range(string.Format("A5:U{0}", r + 4)).Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;

            objSheets.Cells[2, 2] = Sci.Env.User.Keyword;
            DataRow dr;
            MyUtility.Check.Seek(string.Format(@"select NameEN from Factory where id = '{0}'", Sci.Env.User.Keyword), out dr, null);
            objSheets.Cells[1, 1] = dr["NameEN"].ToString() + "\r\n" + "CARTON RECEIVING REPORT";
            string d1 = "", d2 = "";
            if (!MyUtility.Check.Empty(dateReceiveDate.Value1))
            {
                d1 = Convert.ToDateTime(dateReceiveDate.Value1).ToString("d");
            }
            if (!MyUtility.Check.Empty(dateReceiveDate.Value2))
            {
                d2 = Convert.ToDateTime(dateReceiveDate.Value2).ToString("d");
            }
            string drange = d1 + "~" + d2;
            objSheets.Cells[3, 13] = drange;
            objSheets.get_Range("A1").RowHeight = 45;
            objApp.Visible = true;

            this.HideWaitMessage();
        }
    }
}
