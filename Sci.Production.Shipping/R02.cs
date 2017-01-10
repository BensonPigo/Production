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


namespace Sci.Production.Shipping
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        DateTime? pulloutDate1, pulloutDate2, sdpDate1, sdpDate2, sciDlv1, sciDlv2;
        string sp1, sp2, mDivisionID;
        DataTable printData;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            comboBox1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Pullout Date can't empty!!");
                return false;
            }

            mDivisionID = comboBox1.Text;
            pulloutDate1 = dateRange1.Value1;
            pulloutDate2 = dateRange1.Value2;
            sp1 = textBox1.Text;
            sp2 = textBox2.Text;
            sdpDate1 = dateRange2.Value1;
            sdpDate2 = dateRange2.Value2;
            sciDlv1 = dateRange3.Value1;
            sciDlv2 = dateRange3.Value2;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select p.MDivisionID,pd.OrderID,isnull(o.BrandID,'') as BrandID,
isnull((g.Forwarder+'-'+ls.Abb),'') as Forwarder,isnull(o.StyleID,'') as StyleID,
isnull(o.SeasonID,'') as SeasonID,isnull(o.CustPONo,'') as CustPONo,isnull(o.Customize1,'') as Customize,
isnull((o.Dest+'-'+c.Alias),'') as Dest,isnull(o.StyleUnit,'') as StyleUnit,o.SciDelivery,
oq.BuyerDelivery,p.PulloutDate,isnull(oq.Qty,0) as Qty,IIF(pl.Type = 'B' or pl.Type = 'F',pl.CTNQty,0) as TtlCtn,
pd.ShipQty,isnull(pl.ShipModeID,'') as ShipModeID,pd.INVNo,g.InvDate,
case pd.Status when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed'when 'S' then 'Shortage' else '' end as StatusExp
from Pullout p
inner join Pullout_Detail pd on p.ID = pd.ID
left join Orders o on o.ID = pd.OrderID
left join Order_QtyShip oq on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join Country c on c.ID = o.Dest
left join GMTBooking g on g.ID = pd.INVNo
left join PackingList pl on pl.ID = pd.PackingListID
left join LocalSupp ls on g.Forwarder = ls.ID
where p.PulloutDate between '{0}' and '{1}'", Convert.ToDateTime(pulloutDate1).ToString("d"), Convert.ToDateTime(pulloutDate2).ToString("d")));

            if (!MyUtility.Check.Empty(sdpDate1))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate between '{0}' and '{1}'", Convert.ToDateTime(sdpDate1).ToString("d"), Convert.ToDateTime(sdpDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery between '{0}' and '{1}'", Convert.ToDateTime(sciDlv1).ToString("d"), Convert.ToDateTime(sciDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(mDivisionID))
            {
                sqlCmd.Append(string.Format(" and p.MDivisionID = '{0}'", mDivisionID));
            }

            if (!MyUtility.Check.Empty(sp1))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID >= '{0}'", sp1));
            }

            if (!MyUtility.Check.Empty(sp2))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID <= '{0}'", sp2));
            }

            sqlCmd.Append(" order by p.MDivisionID,pd.OrderID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }
            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            SetCount(printData.Rows.Count);

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R02_PulloutReportList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, 20];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["OrderID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["Forwarder"];
                objArray[0, 4] = dr["StyleID"];
                objArray[0, 5] = dr["SeasonID"];
                objArray[0, 6] = dr["CustPONo"];
                objArray[0, 7] = dr["Customize"];
                objArray[0, 8] = dr["Dest"];
                objArray[0, 9] = dr["StyleUnit"];
                objArray[0, 10] = dr["SciDelivery"];
                objArray[0, 11] = dr["BuyerDelivery"];
                objArray[0, 12] = dr["PulloutDate"];
                objArray[0, 13] = dr["Qty"];
                objArray[0, 14] = dr["TtlCtn"];
                objArray[0, 15] = dr["ShipQty"];
                objArray[0, 16] = dr["StatusExp"];
                objArray[0, 17] = dr["ShipModeID"];
                objArray[0, 18] = dr["INVNo"];
                objArray[0, 19] = dr["InvDate"];
                worksheet.Range[String.Format("A{0}:T{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
