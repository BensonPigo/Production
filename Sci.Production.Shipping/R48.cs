using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R48
    /// </summary>
    public partial class R48 : Win.Tems.PrintForm
    {
        private DateTime? buyerDlv1;
        private DateTime? buyerDlv2;
        private DateTime? sciDlv1;
        private DateTime? sciDlv2;
        private string brand;
        private string contract;
        private string orderID;
        private DataTable printData;

        /// <summary>
        /// R48
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R48(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateBuyerDelivery.Value1) && MyUtility.Check.Empty(this.dateBuyerDelivery.Value2)
                && MyUtility.Check.Empty(this.dateSCIDelivery.Value1) && MyUtility.Check.Empty(this.dateSCIDelivery.Value2))
            {
                MyUtility.Msg.WarningBox("Buyer Delivery and SCI Delivery cannot all empty!!");
                return false;
            }

            this.buyerDlv1 = this.dateBuyerDelivery.Value1;
            this.buyerDlv2 = this.dateBuyerDelivery.Value2;
            this.sciDlv1 = this.dateSCIDelivery.Value1;
            this.sciDlv2 = this.dateSCIDelivery.Value2;
            this.contract = this.txtCustomsContract.Text;
            this.orderID = this.txtOrderID.Text;
            this.brand = this.txtbrand.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
SELECT distinct c.NameEN
	, o.ID
	, o.StyleID
	, o.SeasonID
	, o.BuyerDelivery
	, o.BrandID
	, o.Category
	, o.Qty
	, psd.Refno
	, oq.Article
	, oq.SizeCode
	, cf.NameEN
	, s.NameEN
	, ofs.CurrencyID
	, ofs.PurchasePrice
	, ofs.PurchaseUnit
	, vc.VNContractID
	, vcdd.NLCode
	, vcdd.FabricType
	, vcdd.UsageUnit
	, vcdd.UnitID
	, vcdd.SystemQty
	, psd.ShipQty
	, vcdd.Qty
	, vcdd.Waste
	, vcdd.UserCreate
	, vcdd.Qty
	, vcdd.SystemQty
	, psd.Remark
FROM Orders o WITH(NOLOCK)
INNER JOIN Company c WITH(NOLOCK) ON o.OrderCompanyID = c.ID
INNER JOIN Order_Qty oq WITH(NOLOCK) ON o.ID = oq.ID
LEFT JOIN Order_FtyMtlStdCost ofs WITH(NOLOCK) ON o.ID = ofs.OrderID
LEFT JOIN Company cf WITH(NOLOCK) ON ofs.PurchaseCompanyID = cf.ID
LEFT JOIN PO_Supp_Detail psd WITH(NOLOCK) ON o.ID = psd.ID
LEFT JOIN PO_Supp ps WITH(NOLOCK) ON psd.ID = ps.ID and psd.SEQ1 = ps.SEQ1
LEFT JOIN Supp s WITH(NOLOCK) ON s.ID = ps.SuppID 
LEFT JOIN (
	select distinct	vc.VNContractID
			, vc.ID
			, vc.StyleID
			, vc.BrandID
			, vc.SeasonID			
            , va.Article
            , vs.SizeCode
	from VNConsumption vc WITH (NOLOCK) 
    inner join VNConsumption_Article va WITH (NOLOCK)on  va.ID = vc.ID 
    inner join VNConsumption_SizeCode vs WITH (NOLOCK) on vs.ID = vc.ID 
) vc on vc.StyleID = o.StyleID and vc.SeasonID = o.SeasonID and vc.BrandID = o.BrandID  and vc.Article = oq.Article and vc.SizeCode = oq.SizeCode
LEFT JOIN VNConsumption_Detail_Detail vcdd WITH(NOLOCK) ON vc.ID = vcdd.ID
Where 1=1
");

            if (!MyUtility.Check.Empty(this.buyerDlv1))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.buyerDlv1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.buyerDlv2))
            {
                sqlCmd.Append(string.Format(" and o.BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.buyerDlv2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}' ", Convert.ToDateTime(this.sciDlv1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.sciDlv2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}' ", Convert.ToDateTime(this.sciDlv2).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.contract))
            {
                sqlCmd.Append(string.Format(" and vc.VNContractID = '{0}' ", this.contract));
            }

            if (!MyUtility.Check.Empty(this.orderID))
            {
                sqlCmd.Append(string.Format(" and o.ID = '{0}' ", this.orderID));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}' ", this.brand));
            }

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
            Microsoft.Office.Interop.Excel.Application objApp = new Microsoft.Office.Interop.Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Shipping_R48.xltx", objApp);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = objApp.Sheets[1];
            com.WriteTable(this.printData, 2);
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Sheets[1].Columns[9].ColumnWidth = 18;
            objApp.Sheets[1].Columns[13].ColumnWidth = 30;
            objApp.Sheets[1].Columns[29].ColumnWidth = 30;
            objApp.Visible = true;
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }

        private void TxtOrderID_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCustomsContract.Text))
            {
                MyUtility.Msg.WarningBox("Please enter <Contract no.> first!");
                return;
            }

            List<SqlParameter> parameter = new List<SqlParameter>()
            {
                new SqlParameter("@ContractID", this.txtCustomsContract.Text),
            };

            string sqlCmd = @"
select distinct vedd.OrderID
from VNExportDeclaration ved
inner join VNExportDeclaration_Detail vedd on ved.ID = vedd.ID
where VNContractID = @ContractID
";
            DBProxy.Current.Select(null, sqlCmd, parameters: parameter, datas: out DataTable dt);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "OrderID", "13", this.txtOrderID.Text);

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            List<DataRow> dr = item.GetSelecteds().ToList();

            this.txtOrderID.Text = MyUtility.Convert.GetString(dr[0]["OrderID"]);
        }
    }
}
