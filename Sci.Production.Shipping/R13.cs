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
    /// <summary>
    /// R13
    /// </summary>
    public partial class R13 : Sci.Win.Tems.PrintForm
    {
        private DateTime? buyerDlv1;
        private DateTime? buyerDlv2;
        private string brand;
        private string Shipper;
        private string factory;
        private string category;
        private DataTable printData;

        /// <summary>
        /// R13
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R13(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable shipperID, factory;
            DBProxy.Current.Select(null, "select '' as ShipperID union all select ShipperID from FSRCpuCost WITH (NOLOCK) ", out shipperID);
            MyUtility.Tool.SetupCombox(this.comboShipper, 1, shipperID);
            this.comboShipper.Text = Sci.Env.User.Keyword;
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.dateBuyerDelivery.Value1 = DateTime.Today;

            // comboBox2.SelectedIndex = -1;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery can't all empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.txtdropdownlistCategory.Text))
            {
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }

            this.buyerDlv1 = this.dateBuyerDelivery.Value1;
            this.buyerDlv2 = this.dateBuyerDelivery.Value2;
            this.brand = this.txtbrand.Text;
            this.Shipper = this.comboShipper.Text.ToString().Trim();
            this.factory = this.comboFactory.Text;
            this.category = this.txtdropdownlistCategory.SelectedValue.ToString();
            return base.ValidateInput();
        }

        /// <inheritdoc/>
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
,ROUND(o.CPU,3) as cpu
,ROUND(isnull(cpucost.cpucost,0),3) as cpucost
,SubPSCost=         ROUND((Select Isnull(sum(ot.Price),0) 
					from Order_TmsCost ot
					inner join ArtworkType a on ot.ArtworkTypeID = a.ID
					where ot.ID = o.ID and (a.Classify = 'A' or ( a.Classify = 'I' and a.IsTtlTMS = 0) and a.IsTMS=0))
                    +
                    (Select Isnull(sum(ot.Price)*cpucost.cpucost,0) 
					from Order_TmsCost ot
					inner join ArtworkType a on ot.ArtworkTypeID = a.ID
					where ot.ID = o.ID and ((a.Classify = 'A' or a.Classify = 'I') and a.IsTtlTMS = 0 and a.IsTMS=1)),3)
,LocalPSCost= ROUND(IIF ((select LocalCMT from dbo.Factory where Factory.ID = o.FactoryID) = 1, 
						(select Isnull(sum(ot.Price),0) 
						from Order_TmsCost ot
						inner join ArtworkType a on ot.ArtworkTypeID = a.ID
						where ot.ID = o.id and a.Classify = 'P')
					,0),3)

From Orders o
Left join FtyShipper_Detail fd on o.BrandID = fd.BrandID and fd.FactoryID = o.FactoryID and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
outer apply
(
    select top 1 ROUND(fcd.CpuCost,3) AS CpuCost
    from dbo.FSRCpuCost_Detail fcd 
    where fcd.ShipperID=fd.ShipperID and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate and o.OrigBuyerDelivery between fcd.BeginDate and fcd.EndDate	
) cpucost
Where o.LocalOrder = 0 ");

            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.buyerDlv1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.buyerDlv2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.Shipper))
            {
                sqlCmd.Append(string.Format(" and fd.ShipperID = '{0}'", this.Shipper));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FtyGroup = '{0}'", this.factory));
            }

            if (this.category == "B")
            {
                sqlCmd.Append(" and o.Category = 'B'");
            }
            else if (this.category == "S")
            {
                sqlCmd.Append(" and o.Category = 'S'");
            }
            else if (this.category == "BS")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S')");
            }
            else if (this.category == "BF")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.IsForecast = 1)");
            }
            else if (this.category == "SF")
            {
                sqlCmd.Append(" and (o.Category = 'S' or o.IsForecast = 1)");
            }
            else if (this.category == "BSF")
            {
                sqlCmd.Append(" and (o.Category = 'B' or o.Category = 'S' or o.IsForecast = 1)");
            }

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
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Shipping_R13_FactoryCMTForecast.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Shipping_R13_FactoryCMTForecast.xltx", 3, false, null, objApp);
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];
            string buyer_Delivery = " ~ ";
            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                buyer_Delivery = this.buyerDlv1.Value.ToShortDateString() + buyer_Delivery;
            }

            if (!MyUtility.Check.Empty(this.buyerDlv2))
            {
                buyer_Delivery = buyer_Delivery + this.buyerDlv2.Value.ToShortDateString();
            }

            objSheets.Cells[2, 2] = MyUtility.Convert.GetString(buyer_Delivery);
            objSheets.Cells[2, 5] = MyUtility.Convert.GetString(this.brand);
            objSheets.Cells[2, 7] = MyUtility.Convert.GetString(this.Shipper);
            objSheets.Cells[2, 9] = MyUtility.Convert.GetString(this.factory);
            objSheets.Cells[2, 11] = MyUtility.Convert.GetString(this.txtdropdownlistCategory.Text.ToString().Replace(this.category + "-", string.Empty));

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_R13_FactoryCMTForecast");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
