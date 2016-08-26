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
    public partial class R08 : Sci.Win.Tems.PrintForm
    {
        DateTime? buyerDlv1, buyerDlv2, sciDlv1, sciDlv2, cutoffDate1, cutoffDate2;
        string brand,custcd, mDivision,factory;
        int category;
        bool onlyirregular;
        DataTable printData;
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            comboBox1.Text = Sci.Env.User.Keyword;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox2, 1, factory);
            comboBox2.SelectedIndex = -1;
            MyUtility.Tool.SetupCombox(comboBox3, 1, 1, "Bulk,Sample,Bulk+Sample");
            comboBox3.SelectedIndex = 2;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) && MyUtility.Check.Empty(dateRange2.Value1) && MyUtility.Check.Empty(dateRange2.Value2) && MyUtility.Check.Empty(dateRange3.Value1) && MyUtility.Check.Empty(dateRange3.Value2) && MyUtility.Check.Empty(txtbrand1.Text) && MyUtility.Check.Empty(txtcustcd1.Text))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery, SCI Delivery, Cut-Off Date, Brand, Cust CD can't all empty!!");
                return false;
            }
            
            buyerDlv1 = dateRange1.Value1;
            buyerDlv2 = dateRange1.Value2;
            sciDlv1 = dateRange2.Value1;
            sciDlv2 = dateRange2.Value2;
            cutoffDate1 = dateRange3.Value1;
            cutoffDate2 = dateRange3.Value2;
            brand = txtbrand1.Text;
            custcd = txtcustcd1.Text;
            mDivision = comboBox1.Text;
            factory = comboBox2.Text;
            category = comboBox3.SelectedIndex;
            onlyirregular = checkBox1.Checked;
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"with tmpOrder
as (
select o.ID,o.BrandID,o.CustCDID,o.StyleID,o.SeasonID,o.Customize1,o.CustPONo,o.MDivisionID,
o.FactoryID,o.Dest+'-'+ISNULL(c.Alias,'') as Dest, oq.BuyerDelivery,o.SciDelivery,oq.SDPDate,
oqd.Article,oqd.SizeCode,oqd.Qty as ShipQty,oq.Seq,q.qty as ASQty
from Orders o
inner join Order_Qty q on q.ID = o.ID
inner join Order_QtyShip oq on o.ID = oq.Id
inner join Order_QtyShip_Detail oqd on oq.Id = oqd.Id and oq.Seq = oqd.Seq and q.Article = oqd.Article and q.SizeCode = oqd.SizeCode
left join Country c on o.Dest = c.ID
where 1=1");

            if (!MyUtility.Check.Empty(sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }

            if (!MyUtility.Check.Empty(custcd))
            {
                sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", custcd));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
            }

            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));
            }

            if (category == 0)
            {
                sqlCmd.Append(" and o.Category = 'B'");
            }
            else if (category == 1)
            {
                sqlCmd.Append(" and o.Category = 'S'");
            }
            else if (category == 2)
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
            }

            if (!MyUtility.Check.Empty(buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(cutoffDate1))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate >= '{0}'", Convert.ToDateTime(cutoffDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(cutoffDate2))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate <= '{0}'", Convert.ToDateTime(cutoffDate2).ToString("d")));
            }

            sqlCmd.Append(@"),
PackData
as (
select t.ID,t.Seq,pd.Article,pd.SizeCode,sum(pd.ShipQty) as PackQty,p.INVNo,p.PulloutDate,
pd.ID as PackID
from tmpOrder t
inner join PackingList_Detail pd on t.ID = pd.OrderID and t.Seq = pd.OrderShipmodeSeq
inner join PackingList p on pd.ID = p.ID
group by t.ID,t.Seq,pd.Article,pd.SizeCode,p.INVNo,p.PulloutDate,pd.ID
),
tempdata
as (
select t.*,isnull(p.PackQty,0) as PackQty,isnull(p.INVNo,'') as INVNo,p.PulloutDate,isnull(p.PackID,'') as PackID,
isnull([dbo].getMinCompleteSewQty(t.ID,t.Article,t.SizeCode),0) as SewQty,
isnull((select sum(isnull(pdd.ShipQty,0)) from Pullout_Detail pd, Pullout_Detail_Detail pdd where pd.UKey = pdd.Pullout_DetailUKey and pd.OrderID = t.ID and pd.OrderShipmodeSeq = t.Seq and pdd.Article = t.Article and pdd.SizeCode = t.SizeCode),0) as PullQty
from tmpOrder t
left join PackData p on t.ID = p.ID and t.Seq = p.Seq and t.Article = p.Article and t.SizeCode = p.SizeCode)
select *,IIF(ASQty <> SewQty,'Sewing Qty is not equal to Order Qty.','')+IIF(ShipQty <> PackQty,'Packing Qty '+IIF(ShipQty <> PullQty,'and Pullout Qty ','')+'is not equal to Order Qty by ship.',IIF(ShipQty <> PullQty,'Pullout Qty is not equal to Order Qty by ship.','')) as Reason
from tempdata");

            if (onlyirregular)
            {
                sqlCmd.Append(@" where ASQty = SewQty
and ShipQty = PackQty
and ShipQty = PullQty");
            }
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

            MyUtility.Msg.WaitWindows("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R08_PackingCheckList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 24];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["PackID"];
                objArray[0, 1] = dr["ID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["CustCDID"];
                objArray[0, 4] = dr["StyleID"];
                objArray[0, 5] = dr["SeasonID"];
                objArray[0, 6] = dr["Customize1"];
                objArray[0, 7] = dr["CustPONo"];
                objArray[0, 8] = dr["MDivisionID"];
                objArray[0, 9] = dr["FactoryID"];
                objArray[0, 10] = dr["Dest"];
                objArray[0, 11] = dr["INVNo"];
                objArray[0, 12] = dr["BuyerDelivery"];
                objArray[0, 13] = dr["SciDelivery"];
                objArray[0, 14] = dr["SDPDate"];
                objArray[0, 15] = dr["PulloutDate"];
                objArray[0, 16] = dr["Article"];
                objArray[0, 17] = dr["SizeCode"];
                objArray[0, 18] = dr["ASQty"];
                objArray[0, 19] = dr["SewQty"];
                objArray[0, 20] = dr["ShipQty"];
                objArray[0, 21] = dr["PackQty"];
                objArray[0, 22] = dr["PullQty"];
                objArray[0, 23] = dr["Reason"];
                
                worksheet.Range[String.Format("A{0}:X{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            MyUtility.Msg.WaitClear();
            excel.Visible = true;
            return true;
        }
    }
}
