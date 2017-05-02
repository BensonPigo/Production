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
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        DateTime? FCR_date1, FCR_date2, Inv_date1, Inv_date2, Pull_date1, Pull_date2;
        string GB1, GB2, Buyer, Brand, CustCD, Dest, category;
        DataTable printData;
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();           
            //dateRange1.Value1 = DateTime.Today;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            FCR_date1 = dateFCRDate.Value1;
            FCR_date2 = dateFCRDate.Value2;
            Inv_date1 = dateInvoiceDate.Value1;
            Inv_date2 = dateInvoiceDate.Value2;
            Pull_date1 = datePulloutDate.Value1;
            Pull_date2 = datePulloutDate.Value2;
            GB1 = txtGBNoStart.Text;
            GB2 = txtGBNoEnd.Text;
            Buyer = txtbuyer.Text;
            Brand = txtbrand.Text;
            CustCD = txtcustcd.Text;
            Dest = txtcountryDestination.TextBox1.Text;
            category = txtdropdownlistCategory.SelectedValue.ToString();
            return base.ValidateInput();
        }

        // 非同步取資料
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
Left join Brand b on b.ID = o.BrandID
outer apply
(	
    select top 1 ROUND(fcd.CpuCost,3) as CpuCost
	from dbo.FtyShipper_Detail fd  
	inner join FSRCpuCost_Detail fcd on fd.ShipperID = fcd.ShipperID 
	where fd.BrandID=g.BrandID and fd.FactoryID=o.FactoryID and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate and o.OrigBuyerDelivery between fcd.BeginDate and fcd.EndDate 
) cpucost
Where 1=1 ");

            if (!MyUtility.Check.Empty(FCR_date1))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate >= '{0}'", Convert.ToDateTime(FCR_date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(FCR_date2))
            {
                sqlCmd.Append(string.Format(" and g.FCRDate <= '{0}'", Convert.ToDateTime(FCR_date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(Inv_date1))
            {
                sqlCmd.Append(string.Format(" and g.InvDate >= '{0}'", Convert.ToDateTime(Inv_date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(Inv_date2))
            {
                sqlCmd.Append(string.Format(" and g.InvDate <= '{0}'", Convert.ToDateTime(Inv_date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(Pull_date1))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate >= '{0}'", Convert.ToDateTime(Pull_date1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(Pull_date2))
            {
                sqlCmd.Append(string.Format(" and p.PulloutDate <= '{0}'", Convert.ToDateTime(Pull_date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(GB1))
            {
                sqlCmd.Append(string.Format(" and g.Id >= '{0}'", GB1));
            }
            if (!MyUtility.Check.Empty(GB2))
            {
                sqlCmd.Append(string.Format(" and g.Id <= '{0}'", GB2));
            }

            if (!MyUtility.Check.Empty(Buyer))
            {
                sqlCmd.Append(string.Format(" and  b.BuyerID = '{0}'", Buyer));
            }

            if (!MyUtility.Check.Empty(Brand))
            {
                sqlCmd.Append(string.Format(" and g.BrandID = '{0}'", Brand));
            }

            if (!MyUtility.Check.Empty(CustCD))
            {
                sqlCmd.Append(string.Format(" and g.CustCDID = '{0}'", CustCD));
            }

            if (!MyUtility.Check.Empty(Dest))
            {
                sqlCmd.Append(string.Format(" and g.Dest = '{0}'", Dest));
            }

            if (category == "B")
            {
                sqlCmd.Append(" and o.Category = 'B'");
            }
            else if (category == "S")
            {
                sqlCmd.Append(" and o.Category = 'S'");
            }
            else if (category == "BS")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
            }

            sqlCmd.Append(@" ) 
                            select *
                            ,FtyCMPCostUnit=ROUND(cte.CPU * cte.CPUCost + cte.SubPSCost + cte.LocalPSCost, 2)
                            ,TotalCMPDeclaredtoCustomer=ROUND(cte.Qty*ROUND(cte.CPU * cte.CPUCost + cte.SubPSCost + cte.LocalPSCost, 2),5)
                            from cte");

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
            MyUtility.Excel.CopyToXls(printData, "", "Shipping_R12_FactoryCMTReport.xltx", 2, true, null, null);
            this.HideWaitMessage();
            return true;
        }
    }
}
