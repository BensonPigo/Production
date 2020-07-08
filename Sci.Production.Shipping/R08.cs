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
    /// R08
    /// </summary>
    public partial class R08 : Win.Tems.PrintForm
    {
        private DateTime? buyerDlv1;
        private DateTime? buyerDlv2;
        private DateTime? sciDlv1;
        private DateTime? sciDlv2;
        private DateTime? cutoffDate1;
        private DateTime? cutoffDate2;
        private DateTime? pulloutDate1;
        private DateTime? pulloutDate2;
        private string brand;
        private string custcd;
        private string mDivision;
        private string factory;
        private string category;
        private bool onlyirregular;
        private bool excludeLocalOrder;
        private bool excludeFOC;
        private bool onlyFOC;
        private DataTable printData;

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Sci.Env.User.Keyword;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.SelectedIndex = -1;
            this.checkExcludeLocalOrder.Checked = true;
            this.checkExcludeFOC.Checked = true;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2) && MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2) && MyUtility.Check.Empty(this.dateCutOffDate.Value1) && MyUtility.Check.Empty(this.dateCutOffDate.Value2) && MyUtility.Check.Empty(this.txtbrand.Text) && MyUtility.Check.Empty(this.txtCustcd.Text))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery, SCI Delivery, Cut-Off Date, Brand, Cust CD can't all empty!!");
                return false;
            }

            this.buyerDlv1 = this.dateBuyerDelivery.Value1;
            this.buyerDlv2 = this.dateBuyerDelivery.Value2;
            this.sciDlv1 = this.dateSCIDelivery.Value1;
            this.sciDlv2 = this.dateSCIDelivery.Value2;
            this.cutoffDate1 = this.dateCutOffDate.Value1;
            this.cutoffDate2 = this.dateCutOffDate.Value2;
            this.pulloutDate1 = this.datePulloutDate.Value1;
            this.pulloutDate2 = this.datePulloutDate.Value2;
            this.brand = this.txtbrand.Text;
            this.custcd = this.txtCustcd.Text;
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.onlyirregular = this.checkOnlyPrintTheIrregularData.Checked;
            this.excludeLocalOrder = this.checkExcludeLocalOrder.Checked;
            this.excludeFOC = this.checkExcludeFOC.Checked;
            this.onlyFOC = this.checkOnlyFOC.Checked;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select o.ID,o.BrandID,o.CustCDID,o.StyleID,o.SeasonID,o.Customize1,o.CustPONo,o.MDivisionID,
o.FactoryID,o.Dest+'-'+ISNULL(c.Alias,'') as Dest, oq.BuyerDelivery,o.SciDelivery,oq.SDPDate,
oqd.Article,oqd.SizeCode,oqd.Qty as ShipQty,oq.Seq,q.qty as ASQty
into #tmpOrder
from Orders o WITH (NOLOCK) 
inner join Order_Qty q WITH (NOLOCK) on q.ID = o.ID
inner join Order_QtyShip oq WITH (NOLOCK) on o.ID = oq.Id
inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.Id = oqd.Id and oq.Seq = oqd.Seq and q.Article = oqd.Article and q.SizeCode = oqd.SizeCode
left join OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
left join Country c WITH (NOLOCK) on o.Dest = c.ID
where 1=1 and isnull(ot.IsGMTMaster,0) != 1");

            if (!MyUtility.Check.Empty(this.sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(this.sciDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(this.sciDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.custcd))
            {
                sqlCmd.Append(string.Format(" and o.CustCDID = '{0}'", this.custcd));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", this.mDivision));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");

            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.cutoffDate1))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate >= '{0}'", Convert.ToDateTime(this.cutoffDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.cutoffDate2))
            {
                sqlCmd.Append(string.Format(" and oq.SDPDate <= '{0}'", Convert.ToDateTime(this.cutoffDate2).ToString("d")));
            }

            if (this.excludeLocalOrder)
            {
                sqlCmd.Append(string.Format(" and o.LocalOrder = 0"));
            }

            if (this.excludeFOC)
            {
                sqlCmd.Append(string.Format(" and o.FOC  = 0"));
            }

            if (this.onlyFOC)
            {
                sqlCmd.Append(string.Format(" and o.FOC  = 1"));
            }

            sqlCmd.Append(@"
select distinct t.ID,t.Seq into #tmpOrderdis from #tmpOrder t

select t.ID,t.Seq,pd.Article,pd.SizeCode,sum(pd.ShipQty) as PackQty,p.INVNo,p.PulloutDate,
pd.ID as PackID,p.ExpressID,p.PulloutID
into #PackData
from #tmpOrderdis t
inner join PackingList_Detail pd WITH (NOLOCK) on t.ID = pd.OrderID and t.Seq = pd.OrderShipmodeSeq
inner join PackingList p WITH (NOLOCK) on pd.ID = p.ID
group by t.ID,t.Seq,pd.Article,pd.SizeCode,p.INVNo,p.PulloutDate,pd.ID,p.ExpressID,p.PulloutID

select t.*
    ,isnull(p.PackQty,0) as PackQty
    ,isnull(p.INVNo,'') as INVNo
    ,p.PulloutDate
    ,isnull(p.PackID,'') as PackID
    ,isnull([dbo].getMinCompleteSewQty(t.ID,t.Article,t.SizeCode),0) as SewQty
    ,isnull(
        (
            select sum(isnull(pdd.ShipQty,0)) 
            from Pullout_Detail pd WITH (NOLOCK) , Pullout_Detail_Detail pdd WITH (NOLOCK) 
            where pd.UKey = pdd.Pullout_DetailUKey
            and pd.OrderID = t.ID 
            and pd.OrderShipmodeSeq = t.Seq 
            and pdd.Article = t.Article 
            and pdd.SizeCode = t.SizeCode
    ),0) as PullQty
    ,p.ExpressID
    ,p.PulloutID
into #tempdata
from #tmpOrder t
left join #PackData p on t.ID = p.ID and t.Seq = p.Seq and t.Article = p.Article and t.SizeCode = p.SizeCode
where 1=1");

            if (!MyUtility.Check.Empty(this.pulloutDate1))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.pulloutDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.pulloutDate2))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.pulloutDate2).ToString("d")));
            }

            sqlCmd.Append(@"
select *,IIF(ASQty <> SewQty,'Sewing Qty is not equal to Order Qty.','')+IIF(ShipQty <> PackQty,'Packing Qty '+IIF(ShipQty <> PullQty,'and Pullout Qty ','')+'is not equal to Order Qty by ship.',IIF(ShipQty <> PullQty,'Pullout Qty is not equal to Order Qty by ship.','')) as Reason
from #tempdata");

            if (this.onlyirregular)
            {
                sqlCmd.Append(@" where ASQty = SewQty
and ShipQty = PackQty
and ShipQty = PullQty");
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
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
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_R08_PackingCheckList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            // 填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 27];
            foreach (DataRow dr in this.printData.Rows)
            {
                objArray[0, 0] = dr["PackID"];
                objArray[0, 1] = dr["ID"];
                objArray[0, 2] = dr["Seq"];
                objArray[0, 3] = dr["BrandID"];
                objArray[0, 4] = dr["CustCDID"];
                objArray[0, 5] = dr["StyleID"];
                objArray[0, 6] = dr["SeasonID"];
                objArray[0, 7] = dr["Customize1"];
                objArray[0, 8] = dr["CustPONo"];
                objArray[0, 9] = dr["MDivisionID"];
                objArray[0, 10] = dr["FactoryID"];
                objArray[0, 11] = dr["Dest"];
                objArray[0, 12] = dr["INVNo"];
                objArray[0, 13] = dr["ExpressID"];
                objArray[0, 14] = dr["BuyerDelivery"];
                objArray[0, 15] = dr["SciDelivery"];
                objArray[0, 16] = dr["SDPDate"];
                objArray[0, 17] = dr["PulloutDate"];
                objArray[0, 18] = dr["PulloutID"];
                objArray[0, 19] = dr["Article"];
                objArray[0, 20] = dr["SizeCode"];
                objArray[0, 21] = dr["ASQty"];
                objArray[0, 22] = dr["SewQty"];
                objArray[0, 23] = dr["ShipQty"];
                objArray[0, 24] = dr["PackQty"];
                objArray[0, 25] = dr["PullQty"];
                objArray[0, 26] = dr["Reason"];

                worksheet.Range[string.Format("A{0}:AA{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_R08_PackingCheckList");
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
