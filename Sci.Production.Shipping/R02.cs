using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private DateTime? pulloutDate1;
        private DateTime? pulloutDate2;
        private DateTime? sdpDate1;
        private DateTime? sdpDate2;
        private DateTime? sciDlv1;
        private DateTime? sciDlv2;
        private string mDivisionID;
        private DataTable printData;
        private string sp1;
        private string sp2;

        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Pullout Date can't empty!!");
            //    return false;
            // }
            this.mDivisionID = this.comboM.Text;
            this.pulloutDate1 = this.datePulloutDate.Value1;
            this.pulloutDate2 = this.datePulloutDate.Value2;
            this.sp1 = this.txtSPNoStart.Text;
            this.sp2 = this.txtSPNoEnd.Text;
            this.sdpDate1 = this.dateSDPDate.Value1;
            this.sdpDate2 = this.dateSDPDate.Value2;
            this.sciDlv1 = this.dateSCIDelivery.Value1;
            this.sciDlv2 = this.dateSCIDelivery.Value2;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"select p.MDivisionID,pd.OrderID,isnull(o.BrandID,'') as BrandID,
isnull((g.Forwarder+'-'+ls.Abb),'') as Forwarder,isnull(o.StyleID,'') as StyleID,
isnull(o.SeasonID,'') as SeasonID,isnull(o.CustPONo,'') as CustPONo,isnull(o.Customize1,'') as Customize,
g.CustCDID,
isnull((o.Dest+'-'+c.Alias),'') as Dest,isnull(o.StyleUnit,'') as StyleUnit,o.SciDelivery,
oq.BuyerDelivery,p.PulloutDate,isnull(oq.Qty,0) as Qty,IIF(pl.Type = 'B' or pl.Type = 'F',pl.CTNQty,0) as TtlCtn,
pd.ShipQty,isnull(pl.ShipModeID,'') as ShipModeID,g.CYCFS,pd.INVNo,g.InvDate,
case pd.Status when 'P' then 'Partial' when 'C' then 'Complete' when 'E' then 'Exceed'when 'S' then 'Shortage' else '' end as StatusExp
from Pullout p WITH (NOLOCK) 
inner join Pullout_Detail pd WITH (NOLOCK) on p.ID = pd.ID
left join Orders o WITH (NOLOCK) on o.ID = pd.OrderID
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = pd.OrderID and oq.Seq = pd.OrderShipmodeSeq
left join Country c WITH (NOLOCK) on c.ID = o.Dest
left join GMTBooking g WITH (NOLOCK) on g.ID = pd.INVNo
left join PackingList pl WITH (NOLOCK) on pl.ID = pd.PackingListID
left join LocalSupp ls WITH (NOLOCK) on g.Forwarder = ls.ID
where 1=1"));

            if (!MyUtility.Check.Empty(this.pulloutDate1))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}' ", Convert.ToDateTime(this.pulloutDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.pulloutDate2))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}' ", Convert.ToDateTime(this.pulloutDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sdpDate1))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate >= '{0}' ", Convert.ToDateTime(this.sdpDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sdpDate2))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate <= '{0}' ", Convert.ToDateTime(this.sdpDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}' ", Convert.ToDateTime(this.sciDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}' ", Convert.ToDateTime(this.sciDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.mDivisionID))
            {
                sqlCmd.Append(string.Format(" and p.MDivisionID = '{0}'", this.mDivisionID));
            }

            if (!MyUtility.Check.Empty(this.sp1))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID >= '{0}'", this.sp1));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlCmd.Append(string.Format(" and pd.OrderID <= '{0}'", this.sp2));
            }

            sqlCmd.Append(" order by p.MDivisionID,pd.OrderID");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
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
            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_R02_PulloutReportList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 3;
            object[,] objArray = new object[1, 22];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["MDivisionID"];
                objArray[0, 1] = dr["OrderID"];
                objArray[0, 2] = dr["BrandID"];
                objArray[0, 3] = dr["Forwarder"];
                objArray[0, 4] = dr["StyleID"];
                objArray[0, 5] = dr["SeasonID"];
                objArray[0, 6] = dr["CustPONo"];
                objArray[0, 7] = dr["Customize"];
                objArray[0, 8] = dr["CustCDID"];
                objArray[0, 9] = dr["Dest"];
                objArray[0, 10] = dr["StyleUnit"];
                objArray[0, 11] = dr["SciDelivery"];
                objArray[0, 12] = dr["BuyerDelivery"];
                objArray[0, 13] = dr["PulloutDate"];
                objArray[0, 14] = dr["Qty"];
                objArray[0, 15] = dr["TtlCtn"];
                objArray[0, 16] = dr["ShipQty"];
                objArray[0, 17] = dr["StatusExp"];
                objArray[0, 18] = dr["ShipModeID"];
                objArray[0, 19] = dr["CYCFS"];
                objArray[0, 20] = dr["INVNo"];
                objArray[0, 21] = dr["InvDate"];
                worksheet.Range[string.Format("A{0}:V{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_R02_PulloutReportList");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
