using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class R10 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private string MDivisionID;
        private string FactoryID;
        private DateTime? BuyerDev_S;
        private DateTime? BuyerDev_E;
        private DateTime? SCiDev_S;
        private DateTime? SCiDev_E;
        private DateTime? SewingOutputDate_S;
        private DateTime? SewingOutputDate_E;
        private bool bolOutstanding;

        /// <inheritdoc/>
        public R10(ToolStripMenuItem menuitem)
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
            if (!this.dateRangeByerDev.Value1.HasValue && !this.dateRangeByerDev.Value2.HasValue &&
                !this.dateSCIDelivery.Value1.HasValue && !this.dateSCIDelivery.Value2.HasValue &&
                !this.dateRangeSewingOutputDate.Value1.HasValue && !this.dateRangeSewingOutputDate.Value2.HasValue)
            {
                MyUtility.Msg.InfoBox("Buyer Delivery & SCI Delivery & Sewing Output Date can not all be empty!");
                this.dateRangeByerDev.Focus();
                return false;
            }

            this.BuyerDev_S = this.dateRangeByerDev.Value1;
            this.BuyerDev_E = this.dateRangeByerDev.Value2;
            this.SCiDev_S = this.dateSCIDelivery.Value1;
            this.SCiDev_E = this.dateSCIDelivery.Value2;
            this.SewingOutputDate_S = this.dateRangeSewingOutputDate.Value1;
            this.SewingOutputDate_E = this.dateRangeSewingOutputDate.Value2;
            this.MDivisionID = this.txtMdivision.Text;
            this.FactoryID = this.txtfactory.Text;
            this.bolOutstanding = this.chkOutstanding.Checked;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlcmd = new StringBuilder();
            StringBuilder sqlWhere = new StringBuilder();
            StringBuilder sqlWhere2 = new StringBuilder();
            StringBuilder sqlWhereOutstanding = new StringBuilder();
            #region WHERE條件
            if (!MyUtility.Check.Empty(this.SewingOutputDate_S) || !MyUtility.Check.Empty(this.SewingOutputDate_E))
            {
                if (!MyUtility.Check.Empty(this.SewingOutputDate_S))
                {
                    sqlWhere.Append($"AND s.OutputDate >= '{this.SewingOutputDate_S.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(this.SewingOutputDate_E))
                {
                    sqlWhere.Append($"AND s.OutputDate <= '{this.SewingOutputDate_E.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
                }

                sqlcmd.Append(string.Format(
                    @"
select distinct ssd.OrderId,ssd.Article,ssd.SizeCode
into #tmp_sewingSP
from SewingOutput s 
inner join SewingOutput_Detail_Detail ssd on s.ID = ssd.ID
where 1=1
{0};",
                    sqlWhere.ToString()));

                sqlWhere.Clear();
                sqlWhere.Append($"AND exists (select 1 from #tmp_sewingSP where orderid = o.ID)" + Environment.NewLine);
                sqlWhere2.Append($"AND exists (select 1 from #tmp_sewingSP where orderid = o.ID and Article = o.Article and SizeCode=o.SizeCode)" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BuyerDev_S))
            {
                sqlWhere.Append($"AND o.BuyerDelivery >= '{this.BuyerDev_S.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.BuyerDev_E))
            {
                sqlWhere.Append($"AND o.BuyerDelivery <= '{this.BuyerDev_E.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.SCiDev_S))
            {
                sqlWhere.Append($"AND o.SCIDelivery >= '{this.SCiDev_S.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.SCiDev_E))
            {
                sqlWhere.Append($"AND o.SCIDelivery <= '{this.SCiDev_E.Value.ToString("yyyyMMdd")}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.MDivisionID))
            {
                sqlWhere.Append($"AND o.MDivisionID = '{this.MDivisionID}'" + Environment.NewLine);
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                sqlWhere.Append($"AND o.FtyGroup = '{this.FactoryID}'" + Environment.NewLine);
            }

            if (this.bolOutstanding)
            {
                sqlWhereOutstanding.Append("And (o.OrderQty - o.SewingOutputQty) < 0" + Environment.NewLine);
            }
            #endregion

            #region 組SQL

            sqlcmd.Append(string.Format(
                @"
select distinct
    o.MDivisionID
    ,o.FactoryID
    ,o.ID
    ,Cancelorder = iif(o.junk=1,'Y','')
    ,Category= case 
            when o.Category = 'S' then 'Sample' 
            when o.Category = 'B' then 'Bulk' 
            when o.Category = 'M' then 'Material' 
            when o.Category = 'T' then 'SMTL' 
            else o.Category 
        end
    ,o.CustPONo
    ,o.StyleID
    ,o.SeasonID
    ,o.BrandID
    ,o.BuyerDelivery
    ,o.SCIDelivery
    ,o.SewLine
into #tmp_orders
from Orders o WITH (NOLOCK) 
where o.Category in ('B','S')
{0}

select o.*
    ,[BalanceQty] = o.OrderQty - o.SewingOutputQty
from 
(
	select distinct 
        o.MDivisionID
        ,o.FactoryID
        ,o.ID
        ,o.Cancelorder
        ,o.Category
        ,o.CustPONo
        ,o.StyleID
        ,o.SeasonID
        ,o.BrandID
        ,o.BuyerDelivery
        ,o.SCIDelivery
		,[Article] = oq.Article
		,[SizeCode] = oq.SizeCode
        ,Garmentdye = iif((select ot.Price from Order_TmsCost ot with(nolock) where ot.id =o.id and ot.ArtworkTypeID = 'Garment Dye' ) > 0, 'Y', '')
        ,o.SewLine
		,[OrderQty] =oq.Qty
		,[SewingOutputQty] = isnull(dbo.getMinCompleteSewQty(o.id, oq.Article, oq.SizeCode),0)
	from #tmp_orders o
	outer apply
	(
		select ID, Article, SizeCode, [Qty] = Sum(Qty)
		from Order_Qty WITH (NOLOCK)
		where ID = o.ID
		group by ID, Article, SizeCode
	)oq
	outer apply
	(
		select distinct sdd.OrderId, sdd.Article, sdd.SizeCode, oq.Qty
		from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 		
		where o.ID = sdd.OrderId
		and oq.ID = sdd.OrderId and oq.Article = sdd.Article and oq.SizeCode = sdd.SizeCode
	)sdd
)o 
where 1=1
{1}
{2}
order by o.MDivisionID, o.FactoryID, o.ID, o.Article, o.SizeCode

IF object_id('tempdb..#tmp_sewingSP') IS NOT NULL drop table #tmp_sewingSP
IF object_id('tempdb..#tmp_orders') IS NOT NULL drop table #tmp_orders",
                sqlWhere.ToString(),
                sqlWhereOutstanding.ToString(),
                sqlWhere2.ToString()));
            #endregion

            DBProxy.Current.DefaultTimeout = 900;  // timeout時間改為15分鐘
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd.ToString(), out this.printData);
            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Sewing_R10.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Sewing_R10.xltx", 1, false, null, objApp); // 將datatable copy to excel

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Sewing_R10");
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
