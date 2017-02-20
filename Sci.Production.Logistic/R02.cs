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

namespace Sci.Production.Logistic
{
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        string po1, po2, sp1, sp2, brand, mDivision, location1, location2;
        DataTable printData;
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(comboBox1, 1, mDivision);
            comboBox1.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            po1 = textBox1.Text;
            po2 = textBox2.Text;
            sp1 = textBox3.Text;
            sp2 = textBox4.Text;
            brand = txtbrand1.Text;
            mDivision = comboBox1.Text;
            location1 = txtcloglocation1.Text;
            location2 = txtcloglocation2.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select p.MDivisionID,o.FactoryID,pd.OrderID,pd.CTNStartNo,pd.ReceiveDate,o.CustPONo,pd.ClogLocationId,p.BrandID
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join Pullout po WITH (NOLOCK) on p.PulloutID = po.ID
where pd.CTNQty > 0
and pd.ReceiveDate is not null
and (p.PulloutID = '' or po.Status = 'New')");

            if (!MyUtility.Check.Empty(po1))
            {
                sqlCmd.Append(string.Format(" and o.CustPONo >= '{0}'", po1));
            }

            if (!MyUtility.Check.Empty(po2))
            {
                sqlCmd.Append(string.Format(" and o.CustPONo <= '{0}'", po2));
            }

            if (!MyUtility.Check.Empty(sp1))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID >= '{0}'", sp1));
            }

            if (!MyUtility.Check.Empty(sp2))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID <= '{0}'", sp2));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", brand));
            }

            if (!MyUtility.Check.Empty(mDivision))
            {
                sqlCmd.Append(string.Format(" and p.MDivisionID = '{0}'", mDivision));
            }

            if (!MyUtility.Check.Empty(location1))
            {
                sqlCmd.Append(string.Format(" and pd.ClogLocationId >= '{0}'", location1));
            }

            if (!MyUtility.Check.Empty(location2))
            {
                sqlCmd.Append(string.Format(" and pd.ClogLocationId <= '{0}'", location2));
            }

            sqlCmd.Append(" order by pd.ClogLocationId,p.MDivisionID,o.FactoryID,pd.OrderID,pd.ID,pd.Seq");

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
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Logistic_R02_ClogAuditList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 2] = po1 + " ~ " + po2;
            worksheet.Cells[3, 2] = sp1 + " ~ " + sp2;
            worksheet.Cells[4, 2] = location1 + " ~ " + location2;
            worksheet.Cells[2, 8] = brand;
            worksheet.Cells[3, 8] = mDivision;

            //填內容值
            int intRowsStart = 6;
            object[,] objArray = new object[1, 8];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["OrderID"];
                objArray[0, 3] = dr["CTNStartNo"];
                objArray[0, 4] = dr["ReceiveDate"];
                objArray[0, 5] = dr["CustPONo"];
                objArray[0, 6] = dr["ClogLocationId"];
                objArray[0, 7] = dr["BrandID"];
                worksheet.Range[String.Format("A{0}:H{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            //excel.Cells.EntireColumn.AutoFit();
            //excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
