using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class R21 : Win.Tems.PrintForm
    {
        private DataTable[] PrintTable;
        private string sqlCmd;
        private List<SqlParameter> paras = new List<SqlParameter>();

        /// <inheritdoc/>
        public R21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.dateCreateDate.DateBox1.Value = DateTime.Today.AddMonths(-1).GetFirstDayOfMonth();
            this.dateCreateDate.DateBox2.Value = DateTime.Today.GetLastDayOfMonth();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.paras.Clear();
            if (!this.dateCreateDate.HasValue &&
                !this.dateBuyerDelivery.HasValue &&
                !this.dateOnBoard.HasValue)
            {
                MyUtility.Msg.InfoBox("Please at least input a criteria for query report.");
                return false;
            }

            #region Where 條件
            string where = string.Empty;

            if (this.dateCreateDate.HasValue1 && this.dateCreateDate.HasValue2)
            {
                this.paras.Add(new SqlParameter("@CDate_1", SqlDbType.Date) { Value = this.dateCreateDate.Value1 });
                this.paras.Add(new SqlParameter("@CDate_2", SqlDbType.Date) { Value = this.dateCreateDate.Value2 });
                where = $"and aipp.CDate between @CDate_1 AND @CDate_2 " + Environment.NewLine;
            }

            if (this.dateBuyerDelivery.HasValue1 && this.dateBuyerDelivery.HasValue2)
            {
                this.paras.Add(new SqlParameter("@BuyerDelivery_1", SqlDbType.Date) { Value = this.dateBuyerDelivery.Value1 });
                this.paras.Add(new SqlParameter("@BuyerDelivery_2", SqlDbType.Date) { Value = this.dateBuyerDelivery.Value2 });
                where = $"and oqs.BuyerDelivery between @BuyerDelivery_1 AND @BuyerDelivery_2 " + Environment.NewLine;
            }

            if (this.dateOnBoard.HasValue1 && this.dateOnBoard.HasValue2)
            {
                this.paras.Add(new SqlParameter("@ActETD_1", SqlDbType.Date) { Value = this.dateOnBoard.Value1 });
                this.paras.Add(new SqlParameter("@ActETD_2", SqlDbType.Date) { Value = this.dateOnBoard.Value2 });
                where = $"and aipp.ActETD between @ActETD_1 AND @ActETD_2 " + Environment.NewLine;
            }

            #endregion

            #region SQL
            this.sqlCmd = $@"
select	aipp.ID,
		[Status] = case	when aipp.Status = 'New' then  'New'
						when aipp.Status = 'Junked' then  'Junk'
						when aipp.Status = 'Checked' then  'PPIC Checked'
						when aipp.Status = 'Approved' then 'FTY Approved'
						when aipp.Status = 'Confirmed' then  'SMR Comfirmed'
						when aipp.Status = 'Locked' then  'GM Team Locked'
						else '' end,
		o.FactoryID,
		o.BrandID,
		aipp.OrderID,
		aipp.OrderShipmodeSeq,
		oqs.ShipmodeID,
		aipp.ShipQty,
		vaipp.ActAmtUSD,
		aipp.CW,
		c.Alias,
		ls.Abb,
		[Responsibility] = ResponsibilityInfo.Responsibility,
		[Ratio] = ResponsibilityInfo.Ratio,
		oqs.BuyerDelivery,
		aipp.CDate,
		aipp.PPICMgrApvDate,
		aipp.FtyMgrApvDate,
		aipp.SMRApvDate,
		aipp.TaskApvDate,
		PulloutDate.val,
		aipp.ActETD,
		aipp.APReceiveDoxDate,
		aipp.APAmountEditDate,
		vaipp.VoucherID,
		vaipp.VoucherDate
from AirPP aipp with (nolock)
inner join Orders o with (nolock) on o.ID = aipp.OrderID
left join Order_QtyShip oqs with (nolock) on oqs.Id = aipp.OrderID and oqs.Seq = aipp.OrderShipmodeSeq
left join View_AirPP vaipp with (nolock) on vaipp.ID = aipp.ID
left join Country c with (nolock) on c.ID = o.Dest
left join LocalSupp ls with (nolock) on ls.id = aipp.Forwarder
outer apply (	select top 1 [val] = PulloutDate
				from PackingList p with (nolock)
				inner join PackingList_Detail pd with (nolock) on p.id = pd.ID
				where pd.OrderID = aipp.OrderID and pd.OrderShipmodeSeq = aipp.OrderShipmodeSeq
			) PulloutDate
outer apply (select [Responsibility] = Stuff(	iif(aipp.ResponsibleFty = 1, CONCAT('/', 'Factory-', aipp.ResponsibleFtyNo), '')+
												iif(aipp.ResponsibleSubcon = 1, CONCAT('/', 'Subcon-', aipp.SubconDBCNo), '')+
												iif(aipp.ResponsibleSCI = 1, CONCAT('/', 'SCI-', aipp.SCIICRNo), '')+
												iif(aipp.ResponsibleSupp = 1, CONCAT('/', 'Supplier-', aipp.SuppDBCNo), '')+
												iif(aipp.ResponsibleBuyer = 1, CONCAT('/', 'Buyer - Debit Memo(Customer)-', aipp.BuyerDBCNo, ', ICR No.', aipp.BuyerICRNo), '')
											,1,1,'') ,
					[Ratio] = Stuff(iif(aipp.ResponsibleFty = 1, CONCAT('/', aipp.RatioFty), '')+
									iif(aipp.ResponsibleSubcon = 1, CONCAT('/', aipp.RatioSubcon), '')+
									iif(aipp.ResponsibleSCI = 1, CONCAT('/', aipp.RatioSCI), '')+
									iif(aipp.ResponsibleSupp = 1, CONCAT('/', aipp.RatioSupp), '')+
									iif(aipp.ResponsibleBuyer = 1, CONCAT('/', aipp.RatioBuyer), '')
									,1,1,'')
			) ResponsibilityInfo
where 1 = 1 {where}
";
            #endregion
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlCmd, this.paras, out this.PrintTable);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            this.SetCount(this.PrintTable[0].Rows.Count);
            if (this.PrintTable[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                return false;
            }

            this.ShowWaitMessage("Excel processing...");

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Shipping_R21.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.PrintTable[0], string.Empty, "Shipping_R21.xltx", 1, false, null, objApp);

            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);

            this.HideWaitMessage();
            return true;
        }

        private void R21_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
