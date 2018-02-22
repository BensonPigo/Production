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
    /// <summary>
    /// R12
    /// </summary>
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        private DateTime? FCR_date1;
        private DateTime? FCR_date2;
        private DateTime? Inv_date1;
        private DateTime? Inv_date2;
        private DateTime? Pull_date1;
        private DateTime? Pull_date2;
        private string GB1;
        private string GB2;
        private string Buyer;
        private string Brand;
        private string CustCD;
        private string Dest;
        private string category;
        private DataTable printData;

        /// <summary>
        /// R12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            // dateRange1.Value1 = DateTime.Today;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.FCR_date1 = this.dateFCRDate.Value1;
            this.FCR_date2 = this.dateFCRDate.Value2;
            this.Inv_date1 = this.dateInvoiceDate.Value1;
            this.Inv_date2 = this.dateInvoiceDate.Value2;
            this.Pull_date1 = this.datePulloutDate.Value1;
            this.Pull_date2 = this.datePulloutDate.Value2;
            this.GB1 = this.txtGBNoStart.Text;
            this.GB2 = this.txtGBNoEnd.Text;
            this.Buyer = this.txtbuyer.Text;
            this.Brand = this.txtbrand.Text;
            this.CustCD = this.txtcustcd.Text;
            this.Dest = this.txtcountryDestination.TextBox1.Text;
            this.category = this.comboCategory.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"with cte as (
Select DISTINCT
o.FactoryID
,g.BrandID
,o.OrigBuyerDelivery
,o.BuyerDelivery
,g.ID
,g.InvDate
,pd.OrderID
,o.CustPONo
,o.StyleID
,o.SeasonID
,Category=IIF(o.Category = 'B', 'Bulk','Sample')
,o.Qty
,ShipQty = (select SUM(PackingList_Detail.ShipQty) 
			from PackingList 
			inner join PackingList_Detail on PackingList.ID=PackingList_Detail.ID 
			where PackingList.INVNo=g.ID and PackingList_Detail.OrderID=pd.OrderID)
,o.PoPrice
,g.CustCDID
,g.Shipper
,g.Dest
,g.FCRDate
,ROUND(o.CPU,3) as CPU
,isnull(cpucost.cpucost,0) as cpucost
,SubPSCost=         ROUND((Select Isnull(sum(ot.Price),0) 
					from Order_TmsCost ot
					inner join ArtworkType a on ot.ArtworkTypeID = a.ID
					where ot.ID = pd.OrderID and (a.Classify = 'A' or ( a.Classify = 'I' and a.IsTtlTMS = 0) and a.IsTMS=0))
                    +
                    (Select Isnull(sum(ot.Price)*cpucost.cpucost,0) 
					from Order_TmsCost ot
					inner join ArtworkType a on ot.ArtworkTypeID = a.ID
					where ot.ID = pd.OrderID and ((a.Classify = 'A' or a.Classify = 'I') and a.IsTtlTMS = 0 and a.IsTMS=1)),3)
,LocalPSCost= ROUND(IIF ((select LocalCMT from dbo.Factory where Factory.ID = o.FactoryID) = 1, 
						(select Isnull(sum(ot.Price),0) 
						from Order_TmsCost ot
						inner join ArtworkType a on ot.ArtworkTypeID = a.ID
						where ot.ID = pd.OrderID and a.Classify = 'P')
					,0),3)
From GMTBooking g
Left join PackingList p on g.ID = p.InvNo
Left join PackingList_Detail pd on p.ID = pd.ID
Inner join Orders o on pd.OrderID = o.ID
inner join OrderType ot WITH (NOLOCK) on ot.BrandID = o.BrandID and ot.id = o.OrderTypeID
Left join Brand b on b.ID = o.BrandID
outer apply
(	
    select top 1 ROUND(fcd.CpuCost,3) as CpuCost
	from dbo.FtyShipper_Detail fd  
	inner join FSRCpuCost_Detail fcd on fd.ShipperID = fcd.ShipperID 
	where fd.BrandID=g.BrandID and fd.FactoryID=o.FactoryID and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate and o.OrigBuyerDelivery between fcd.BeginDate and fcd.EndDate 
) cpucost
Where 1=1 and ot.IsGMTMaster != 1");

            if (!MyUtility.Check.Empty(this.FCR_date1))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}'", Convert.ToDateTime(this.FCR_date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.FCR_date2))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}'", Convert.ToDateTime(this.FCR_date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Inv_date1))
            {
                sqlCmd.Append(string.Format(" and g.InvDate >= '{0}'", Convert.ToDateTime(this.Inv_date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Inv_date2))
            {
                sqlCmd.Append(string.Format(" and g.InvDate <= '{0}'", Convert.ToDateTime(this.Inv_date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Pull_date1))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(this.Pull_date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Pull_date2))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(this.Pull_date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.GB1))
            {
                sqlCmd.Append(string.Format(" and g.Id >= '{0}'", this.GB1));
            }

            if (!MyUtility.Check.Empty(this.GB2))
            {
                sqlCmd.Append(string.Format(" and g.Id <= '{0}'", this.GB2));
            }

            if (!MyUtility.Check.Empty(this.Buyer))
            {
                sqlCmd.Append(string.Format(" and  b.BuyerID = '{0}'", this.Buyer));
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", this.Brand));
            }

            if (!MyUtility.Check.Empty(this.CustCD))
            {
                sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", this.CustCD));
            }

            if (!MyUtility.Check.Empty(this.Dest))
            {
                sqlCmd.Append(string.Format(" and g.Dest = '{0}'", this.Dest));
            }

            sqlCmd.Append($" and o.Category in ({this.category})");

            sqlCmd.Append(@" ) 
                            select *
                            ,FtyCMPCostUnit=ROUND(cte.CPU * cte.CPUCost + cte.SubPSCost + cte.LocalPSCost, 2)
                            ,TotalCMPDeclaredtoCustomer=ROUND(cte.Qty*ROUND(cte.CPU * cte.CPUCost + cte.SubPSCost + cte.LocalPSCost, 2),5)
                            from cte");

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
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Shipping_R12_FactoryCMTReport.xltx", 2, true, null, null);
            this.HideWaitMessage();
            return true;
        }
    }
}
