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
using Sci.Utility.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    public partial class R13 : Sci.Win.Tems.PrintForm
    {
        DateTime? buyerDlv1, buyerDlv2;
        string brand, Shipper, factory,category;
        DataTable printData;
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();           
            DataTable ShipperID, factory;
            DBProxy.Current.Select(null, "select '' as ShipperID union all select ShipperID from FSRCpuCost WITH (NOLOCK) ", out ShipperID);
            MyUtility.Tool.SetupCombox(comboShipper, 1, ShipperID);
            comboShipper.Text = Sci.Env.User.Keyword;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(comboFactory, 1, factory);
            dateBuyerDelivery.Value1 = DateTime.Today;
            //comboBox2.SelectedIndex = -1;
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(dateBuyerDelivery.Value1) && MyUtility.Check.Empty(dateBuyerDelivery.Value2))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery can't all empty!!");
                return false;
            }
            if(MyUtility.Check.Empty(txtdropdownlistCategory.Text))
            {
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }
            
            buyerDlv1 = dateBuyerDelivery.Value1;
            buyerDlv2 = dateBuyerDelivery.Value2;
            brand = txtbrand.Text;
            Shipper = comboShipper.Text.ToString().Trim();
            factory = comboFactory.Text;
            category = txtdropdownlistCategory.SelectedValue.ToString();
            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"with cte as (
Select 
o.BuyerDelivery,o.OrigBuyerDelivery,o.ID
,Category=IIF(o.Category = 'B', 'Bulk',IIF(o.Category = 'S','Sample','Forecast'))
,o.CustPONo
,o.Qty
,o.CustCDID
,o.StyleID
,o.SeasonID
,o.Dest
,o.SCIDelivery
,o.BrandID
,o.FactoryID
,fd.ShipperID
,o.CPU
,isnull(cpucost.cpucost,0) as cpucost
,SubPSCost=         ((Select Isnull(sum(ot.Price),0) 
					from Order_TmsCost ot
					inner join ArtworkType a on ot.ArtworkTypeID = a.ID
					where ot.ID = o.ID and (a.Classify = 'A' or ( a.Classify = 'I' and a.IsTtlTMS = 0) and a.IsTMS=0))
                    +
                    (Select Isnull(sum(ot.Price)*cpucost.cpucost,0) 
					from Order_TmsCost ot
					inner join ArtworkType a on ot.ArtworkTypeID = a.ID
					where ot.ID = o.ID and ((a.Classify = 'A' or a.Classify = 'I') and a.IsTtlTMS = 0 and a.IsTMS=1)))
,LocalPSCost= IIF ((select LocalCMT from dbo.Factory where Factory.ID = o.FactoryID) = 1, 
						(select Isnull(sum(ot.Price),0) 
						from Order_TmsCost ot
						inner join ArtworkType a on ot.ArtworkTypeID = a.ID
						where ot.ID = o.id and a.Classify = 'P')
					,0)

From Orders o
Left join FtyShipper_Detail fd on o.BrandID = fd.BrandID and fd.FactoryID = o.FactoryID and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
outer apply
(
    select top 1 fcd.CpuCost
    from dbo.FSRCpuCost_Detail fcd 
    where fcd.ShipperID=fd.ShipperID and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate and o.OrigBuyerDelivery between fcd.BeginDate and fcd.EndDate	
) cpucost
Where o.LocalOrder = 0 ");

            if (!MyUtility.Check.Empty(buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(buyerDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(buyerDlv2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", brand));
            }

            if (!MyUtility.Check.Empty(Shipper))
            {
                sqlCmd.Append(string.Format(" and fd.ShipperID = '{0}'", Shipper));
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", factory));
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
            else if (category == "BF")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.IsForecast = 1)");
            }
            else if (category == "SF")
            {
                sqlCmd.Append(" and (o.Category = 'S' or o.IsForecast = 1)");
            }
            else if (category == "BSF")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S' or o.IsForecast = 1)");
            }

            sqlCmd.Append(@" ) 
                            select *
                            ,FtyCMPCostUnit=ROUND(cte.CPU * cte.CPUCost + cte.SubPSCost + cte.LocalPSCost, 2)
                            ,TotalCMPDeclaredtoCustomer=cte.Qty*ROUND(cte.CPU * cte.CPUCost + cte.SubPSCost + cte.LocalPSCost, 2)
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
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Shipping_R13_FactoryCMTForecast.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Shipping_R13_FactoryCMTForecast.xltx", 3, true, null, objApp);
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            string Buyer_Delivery=" ~ ";
            if (!MyUtility.Check.Empty(buyerDlv1)) Buyer_Delivery = buyerDlv1.Value.ToShortDateString() + Buyer_Delivery;
            if (!MyUtility.Check.Empty(buyerDlv2)) Buyer_Delivery = Buyer_Delivery + buyerDlv2.Value.ToShortDateString();
            objSheets.Cells[2, 2] = MyUtility.Convert.GetString(Buyer_Delivery);
            objSheets.Cells[2, 5] = MyUtility.Convert.GetString(brand);
            objSheets.Cells[2, 7] = MyUtility.Convert.GetString(Shipper);
            objSheets.Cells[2, 9] = MyUtility.Convert.GetString(factory);
            objSheets.Cells[2, 11] = MyUtility.Convert.GetString(txtdropdownlistCategory.Text.ToString().Replace(category+"-",""));
            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
