using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace Sci.Production.IE
{
    /// <summary>
    /// IE_R08
    /// </summary>
    public partial class R08 : Win.Tems.PrintForm
    {
        private string buyerDelivery_s;
        private string buyerDelivery_e;
        private List<string> operations;
        private string season;
        private string brand;
        private string mDivision;
        private string factory;
        private bool bolBulk;
        private bool bolSample;
        private bool bolForecast;
        private string sql;
        private List<SqlParameter> paras;
        private DataTable printData;

        /// <summary>
        /// R08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.chkBulk.Checked = true;
            this.comboMDivision.SetDefalutIndex(true);
            DBProxy.Current.Select(null, "select '' as ID union all select DISTINCT ftygroup from Factory WITH (NOLOCK) ", out DataTable factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboFactory.SelectedValue = Env.User.Factory;
        }

        /// <summary>
        /// ValidateInput 驗證輸入條件
        /// </summary>
        /// <returns>bool</returns>
        protected override bool ValidateInput()
        {
            if (!this.dateBuyerDelivery.HasValue1 || !this.dateBuyerDelivery.HasValue2)
            {
                MyUtility.Msg.InfoBox("Please input <Buyer Delivery>.");
                return false;
            }


            if (string.IsNullOrEmpty(this.txtmulitOperation1.Text))
            {
                MyUtility.Msg.InfoBox("Please input <Operation>.");
                return false;
            }

            if (!this.chkBulk.Checked && !this.chkSample.Checked && !this.chkForecast.Checked)
            {
                MyUtility.Msg.InfoBox("Please input <Category>.");
                return false;
            }

            this.buyerDelivery_s = ((DateTime)this.dateBuyerDelivery.Value1).ToString("yyyy/MM/dd");
            this.buyerDelivery_e = ((DateTime)this.dateBuyerDelivery.Value2).ToString("yyyy/MM/dd");
            this.operations = this.txtmulitOperation1.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
            this.season = this.txtseason.Text;
            this.brand = this.txtbrand.Text;
            this.mDivision = this.comboMDivision.Text;
            this.factory = this.comboFactory.Text;
            this.bolBulk = this.chkBulk.Checked;
            this.bolSample = this.chkSample.Checked;
            this.bolForecast = this.chkForecast.Checked;

            return base.ValidateInput();
        }

        /// <summary>
        /// OnAsyncDataLoad 非同步取資料
        /// </summary>
        /// <param name="e">e</param>
        /// <returns>DualResult</returns>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.sql = $@"
select o.MDivisionID
	, o.FactoryID
	, o.BuyerDelivery
	, o.BrandID
	, o.SeasonID
	, o.StyleID
	, [ProductType] = r.Name
	, o.Qty
	, op.SMV
	, op.MachineTypeID
	, op.DescEN
from Orders o
inner join Style s on o.StyleUkey = s.Ukey
inner join IETMS i on s.IETMSID = i.ID and s.IETMSVersion = i.Version
inner join IETMS_Detail id on i.Ukey = id.IETMSUkey
inner join Operation op on id.OperationID = op.ID
left join Reason r on s.ApparelType = r.ID and r.ReasonTypeID = 'Style_Apparel_Type'
where o.BuyerDelivery >= '{this.buyerDelivery_s}'
and o.BuyerDelivery <= '{this.buyerDelivery_e}'
and op.ID in ('{string.Join("','", this.operations)}')
";

            if (!string.IsNullOrEmpty(this.season))
            {
                this.sql += $"and o.SeasonID = '{this.season}' " + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(this.brand))
            {
                this.sql += $"and o.BrandID = '{this.brand}' " + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(this.mDivision))
            {
                this.sql += $"and o.MDivisionID = '{this.mDivision}' " + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(this.factory))
            {
                this.sql += $"and o.FactoryID = '{this.factory}' " + Environment.NewLine;
            }

            this.sql += "and o.Category in (";
            string sqlWhere = string.Empty;
            if (this.bolBulk)
            {
                sqlWhere += sqlWhere.Length > 0 ? ",'B'" : "'B'";
            }

            if (this.bolSample)
            {
                sqlWhere += sqlWhere.Length > 0 ? ",'S'" : "'S'";
            }

            if (this.bolForecast)
            {
                sqlWhere += sqlWhere.Length > 0 ? ",''" : "''";
            }

            this.sql += sqlWhere + ")" + Environment.NewLine;

            return new DualResult(true);
        }

        /// <summary>
        /// OnToExcel 產生Excel
        /// </summary>
        /// <param name="report">report</param>
        /// <returns>bool</returns>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            DualResult result = DBProxy.Current.Select(null, this.sql, null, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Env.Cfg.XltPathDir + "\\IE_R08.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(strXltName); // 預先開啟excel app
            if (objApp == null)
            {
                return false;
            }

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "IE_R08.xltx", 1, false, null, objApp, wSheet: objApp.Sheets[1]); // 將datatable copy to excel
            objApp.Cells.EntireRow.AutoFit();
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
