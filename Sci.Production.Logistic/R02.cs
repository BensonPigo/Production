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
using System.Runtime.InteropServices;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_R02
    /// </summary>
    public partial class R02 : Sci.Win.Tems.PrintForm
    {
        private string po1;
        private string po2;
        private string sp1;
        private string sp2;
        private string brand;
        private string mDivision;
        private string location1;
        private string location2;
        private DateTime? bdate1;
        private DateTime? bdate2;
        private DataTable printData;

        /// <summary>R02</summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Sci.Env.User.Keyword;
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.po1 = this.txtPONoStart.Text;
            this.po2 = this.txtPONoEnd.Text;
            this.sp1 = this.txtSPNoStart.Text;
            this.sp2 = this.txtSPNoEnd.Text;
            this.brand = this.txtbrand.Text;
            this.mDivision = this.comboM.Text;
            this.location1 = this.txtcloglocationLocationStart.Text;
            this.location2 = this.txtcloglocationLocationEnd.Text;
            this.bdate1 = this.dateBuyerDelivery.Value1;
            this.bdate2 = this.dateBuyerDelivery.Value2;

            return base.ValidateInput();
        }

        /// <summary>
        /// 非同步取資料
        /// </summary>
        /// <param name="e">ReportEventArgs</param>
        /// <returns>Result</returns>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select 
p.MDivisionID
,o.FactoryID
,pd.OrderID
,pd.CTNStartNo
,pd.ReceiveDate
,o.CustPONo
,pd.ClogLocationId
,p.BrandID
from PackingList p WITH (NOLOCK) 
inner join PackingList_Detail pd WITH (NOLOCK) on p.ID = pd.ID
inner join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join Pullout po WITH (NOLOCK) on p.PulloutID = po.ID
where pd.CTNQty > 0
and pd.ReceiveDate is not null
and (p.PulloutID = '' or po.Status = 'New')
and o.PulloutComplete = 0
");

            if (!MyUtility.Check.Empty(this.po1))
            {
                sqlCmd.Append(string.Format(" and o.CustPONo >= '{0}'", this.po1));
            }

            if (!MyUtility.Check.Empty(this.po2))
            {
                sqlCmd.Append(string.Format(" and o.CustPONo <= '{0}'", this.po2));
            }

            if (!MyUtility.Check.Empty(this.sp1))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID >= '{0}'", this.sp1));
            }

            if (!MyUtility.Check.Empty(this.bdate1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery between '{0}' and '{1}'", Convert.ToDateTime(this.bdate1).ToString("d"), Convert.ToDateTime(this.bdate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID <= '{0}'", this.sp2));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and p.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and p.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.location1))
            {
                sqlCmd.Append(string.Format(" and pd.ClogLocationId >= '{0}'", this.location1));
            }

            if (!MyUtility.Check.Empty(this.location2))
            {
                sqlCmd.Append(string.Format(" and pd.ClogLocationId <= '{0}'", this.location2));
            }

            sqlCmd.Append(" order by pd.ClogLocationId,p.MDivisionID,o.FactoryID,pd.OrderID,pd.ID,pd.Seq");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        /// <summary>
        /// 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Logistic_R02_ClogAuditList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[2, 2] = this.po1 + " ~ " + this.po2;
            worksheet.Cells[3, 2] = this.sp1 + " ~ " + this.sp2;
            worksheet.Cells[4, 2] = this.location1 + " ~ " + this.location2;
            worksheet.Cells[2, 8] = this.brand;
            worksheet.Cells[3, 8] = this.mDivision;

            // 填內容值
            int intRowsStart = 6;
            object[,] objArray = new object[1, 8];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["FactoryID"];
                objArray[0, 2] = dr["OrderID"];
                objArray[0, 3] = dr["CTNStartNo"];
                objArray[0, 4] = dr["ReceiveDate"];
                objArray[0, 5] = dr["CustPONo"];
                objArray[0, 6] = dr["ClogLocationId"];
                objArray[0, 7] = dr["BrandID"];
                worksheet.Range[string.Format("A{0}:H{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Logistic_R02_ClogAuditList");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
