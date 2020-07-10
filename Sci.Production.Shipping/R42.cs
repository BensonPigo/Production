using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// R42
    /// </summary>
    public partial class R42 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? date1;
        private DateTime? date2;
        private string mDivision;
        private string factory;
        private string category;

        /// <summary>
        /// R42
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision, factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            DBProxy.Current.Select(null, "select '' as ID union all select distinct FtyGroup from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
            this.comboM.Text = Env.User.Keyword;
            this.comboFactory.SelectedIndex = -1;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            // if (MyUtility.Check.Empty(dateRange1.Value1))
            // {
            //    MyUtility.Msg.WarningBox("Date can't empty!!");
            //    dateRange1.TextBox1.Focus();
            //    return false;
            // }
            if (this.comboCategory.SelectedIndex == -1)
            {
                this.comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }

            this.date1 = this.dateBuyerDelivery.Value1;
            this.date2 = this.dateBuyerDelivery.Value2;
            this.category = this.comboCategory.SelectedValue.ToString();
            this.mDivision = this.comboM.Text;
            this.factory = this.comboFactory.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(
                @"
select MDivisionID,FactoryID,ID,StyleID,SeasonID,BrandID,Category,SciDelivery,BuyerDelivery,OrderTypeID,Article,SizeCode,
[qty]= sum(Qty),
IIF(CustomSP = '','',SUBSTRING(CustomSP,0,len(CustomSP))) as CustomSP
from (
	select o.MDivisionID,o.FactoryID,o.ID,o.StyleID,o.SeasonID,o.BrandID,IIF(o.Category  = 'B','Bulk','Sample') as Category,
	o.SciDelivery,oq.BuyerDelivery,o.OrderTypeID,oqd.Article,oqd.SizeCode,
	oqd.Qty,oa.Seq as ASeq,os.Seq as SSeq,
	isnull((select CONCAT(CustomSP, ',') 
	from (
		select distinct v.CustomSP+'('+v.VNContractID+')' as CustomSP 
		from VNConsumption v WITH (NOLOCK) ,VNConsumption_Article va WITH (NOLOCK) ,VNConsumption_SizeCode vs WITH (NOLOCK) 
		where v.ID = va.ID and v.ID = vs.ID and v.StyleID = o.StyleID and v.BrandID = o.BrandID and va.Article = oqd.Article and vs.SizeCode = oqd.SizeCode
		) a
	ORDER BY CustomSP
	FOR XML PATH('')),'') as CustomSP
	from Order_QtyShip oq WITH (NOLOCK) 
	inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.Id = oqd.Id and oq.Seq = oqd.Seq
	inner join Orders o WITH (NOLOCK) on o.ID = oq.Id
	left join Order_Article oa WITH (NOLOCK) on o.ID = oa.ID and oa.Article = oqd.Article
	left join Order_SizeCode os WITH (NOLOCK) on o.ID = os.ID and os.SizeCode = oqd.SizeCode
where 1=1
and o.LocalOrder = 0
and o.Junk = 0
and oqd.Qty > 0"));

            sqlCmd.Append($" and o.Category in ({this.category})");

            if (!MyUtility.Check.Empty(this.date1))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}' ", Convert.ToDateTime(this.date1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.date2))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}' ", Convert.ToDateTime(this.date2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and o.MDivisionID = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(string.Format(" and o.FactoryID = '{0}'", this.factory));
            }

            sqlCmd.Append(@") a
group by  MDivisionID,FactoryID,ID,StyleID,SeasonID,BrandID,Category,SciDelivery,BuyerDelivery,OrderTypeID,Article,SizeCode,CustomSP,a.ASeq,a.SSeq
order by a.ID,a.BuyerDelivery,a.ASeq,a.SSeq");

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
            bool result = MyUtility.Excel.CopyToXls(this.printData, string.Empty, xltfile: "Shipping_R42_QtyBDownByColorwaySize.xltx", headerRow: 1);
            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }

            this.HideWaitMessage();
            return true;
        }
    }
}
