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
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        DateTime? pulloutDate1, pulloutDate2;
        string brand, mDivision, factory, category;
        DataTable printData;
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory", out factory);
            MyUtility.Tool.SetupCombox(comboBox2, 1, factory);
            MyUtility.Tool.SetupCombox(comboBox3, 1, 1, "Bulk+Sample,Bulk,Sample");
            comboBox1.Text = Sci.Env.User.Keyword;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = 0;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateRange1.Value1))
            {
                MyUtility.Msg.WarningBox("Pullout Date can't empty!!");
                return false;
            }

            mDivision = comboBox1.Text;
            pulloutDate1 = dateRange1.Value1;
            pulloutDate2 = dateRange1.Value2;
            brand = txtbrand1.Text;
            factory = comboBox2.Text;
            category = comboBox3.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select p.PulloutDate,oq.BuyerDelivery,pd.OrderID,isnull(o.CustPONo,'') as CustPONo,
isnull(o.StyleID,'') as StyleID,isnull(oq.Qty,0) as Qty,pd.ShipQty,IIF(ct.WorkType = '1','Y','') as byCombo,
case pd.Status when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed'when 'S' then 'Shortage' else '' end as StatusExp,
isnull(IIF(o.LocalOrder = 1, o.PoPrice,o.CMPPrice),0) as CMP,
isnull(IIF(o.LocalOrder = 1, Round(o.PoPrice*pd.ShipQty,3),Round(o.CPU*o.CPUFactor*pd.ShipQty,3)),0) as CMPAmt,
isnull(o.PoPrice,0) as PoPrice,isnull(o.PoPrice,0)*pd.ShipQty as FOBAmt, isnull(o.BrandID,'') as BrandID,isnull(o.MDivisionID,'') as MDivisionID,
isnull(o.FactoryID,'') as FactoryID,isnull(oq.ShipmodeID,'') as ShipmodeID,isnull(c.Alias,'') as Alias
from Pullout p
inner join Pullout_Detail pd on p.ID = pd.ID
left join Orders o on pd.OrderID = o.ID
left join Order_QtyShip oq on pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
left join Country c on o.Dest = c.ID
left join Cutting ct on ct.ID = o.CuttingSP
where p.Status = 'Confirmed'
and p.PulloutDate between '{0}' and '{1}'", Convert.ToDateTime(pulloutDate1).ToString("d"), Convert.ToDateTime(pulloutDate2).ToString("d")));
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }
            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", mDivision));
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
            }
            if (category == "Bulk")
            {
                sqlCmd.Append(" and o.Category = 'B'");
            }
            else if (category == "Sample")
            {
                sqlCmd.Append(" and o.Category = 'S'");
            }
            else
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
            }


            sqlCmd.Append(" order by p.PulloutDate,pd.OrderID");

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
            string strXltName = Sci.Env.Cfg.XltPathDir + "Shipping_R03_ActualShipmentRecord.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 19];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["PulloutDate"];
                objArray[0, 1] = dr["BuyerDelivery"];
                objArray[0, 2] = dr["OrderID"];
                objArray[0, 3] = dr["CustPONo"];
                objArray[0, 4] = dr["StyleID"];
                objArray[0, 5] = dr["Qty"];
                objArray[0, 6] = 0; //此欄位等Cutting Output結構決定後再補
                objArray[0, 7] = dr["byCombo"];
                objArray[0, 8] = dr["ShipQty"];
                objArray[0, 9] = dr["StatusExp"];
                objArray[0, 10] = dr["CMP"];
                objArray[0, 11] = dr["CMPAmt"];
                objArray[0, 12] = dr["PoPrice"];
                objArray[0, 13] = dr["FOBAmt"];
                objArray[0, 14] = dr["BrandID"];
                objArray[0, 15] = dr["MDivisionID"];
                objArray[0, 16] = dr["FactoryID"];
                objArray[0, 17] = dr["ShipmodeID"];
                objArray[0, 18] = dr["Alias"];
                worksheet.Range[String.Format("A{0}:S{0}", intRowsStart)].Value2 = objArray;
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
