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

namespace Sci.Production.PPIC
{
    public partial class R17 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private string sp_S;
        private string sp_E;
        private string MDivisionID;
        private string FactoryID;
        private DateTime? BuyerDev_S;
        private DateTime? BuyerDev_E;
        private DateTime? Cdate_S;
        private DateTime? Cdate_E;

        /// <inheritdoc/>
        public R17(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateRangeCDate.Value1.HasValue && !this.dateRangeCDate.Value2.HasValue &&
                !this.dateRangeByerDev.Value1.HasValue && !this.dateRangeByerDev.Value2.HasValue &&
                !MyUtility.Check.Empty(this.txtSP_S.Text) &&
                !MyUtility.Check.Empty(this.txtSP_E.Text))
            {
                MyUtility.Msg.InfoBox("Create Date & Buyer Delivery & SP# can not all be empty!");
                this.dateRangeByerDev.Focus();
                return false;
            }

            this.Cdate_S = this.dateRangeCDate.Value1;
            this.Cdate_E = this.dateRangeCDate.Value2;
            this.BuyerDev_S = this.dateRangeByerDev.Value1;
            this.BuyerDev_E = this.dateRangeByerDev.Value2;
            this.MDivisionID = this.txtMdivision.Text;
            this.sp_S = this.txtSP_S.Text;
            this.sp_E = this.txtSP_E.Text;
            this.FactoryID = this.txtfactory.Text;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            StringBuilder sqlWhereOutstanding = new StringBuilder();
            #region WHERE條件
            if (!MyUtility.Check.Empty(this.Cdate_S))
            {
                sqlWhere.Append($"AND ob.AddDate >= '{this.Cdate_S.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.Cdate_E))
            {
                sqlWhere.Append($"AND ob.AddDate <= '{this.Cdate_E.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BuyerDev_S))
            {
                sqlWhere.Append($"AND o.BuyerDelivery >= '{this.BuyerDev_S.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BuyerDev_E))
            {
                sqlWhere.Append($"AND o.BuyerDelivery <= '{this.BuyerDev_E.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.sp_S))
            {
                sqlWhere.Append($"AND o.ID >= '{this.sp_S}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.sp_E))
            {
                sqlWhere.Append($"AND o.ID <= '{this.sp_E}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                sqlWhere.Append($"AND o.MDivisionID = '{this.MDivisionID}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                sqlWhere.Append($"AND o.FtyGroup = '{this.FactoryID}'" + Environment.NewLine);
            }
            #endregion

            #region 組SQL

            sqlcmd.Append(string.Format(
                @"select o.MDivisionID
	                  ,o.FactoryID
	                  ,o.ID
	                  ,ob.OrderIDFrom
	                  ,o.StyleID
	                  ,o.SeasonID
	                  ,o.BrandID
	                  ,o.BuyerDelivery
	                  ,obq.Article
	                  ,obq.SizeCode
	                  ,obq.Qty
	                  ,[SewingOutputQty] = dbo.getMinCompleteSewQty(o.ID, obq.Article, obq.SizeCode)
                  from Order_BuyBack ob
                  inner join Orders o on ob.ID = o.ID
                  inner join Order_BuyBack_Qty obq on obq.ID = ob.ID and obq.OrderIDFrom = ob.OrderIDFrom
                  where 1=1
                  {0}",
                sqlWhere.ToString()));
            #endregion

            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return result;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R17.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "PPIC_R17.xltx", 1, false, null, objApp); // 將datatable copy to excel

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_R17");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);

            strExcelName.OpenFile();
            #endregion

            return true;
        }
    }
}
