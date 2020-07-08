using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R02
    /// </summary>
    public partial class R02 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? sciDate1;
        private DateTime? sciDate2;
        private DateTime? prodiveDate1;
        private DateTime? prodiveDate2;
        private DateTime? rcvDate1;
        private DateTime? rcvDate2;
        private string brand;
        private string style;
        private string season;
        private string mDivision;
        private string mr;
        private string smr;
        private string pohandle;
        private string posmr;
        private int printType;

        /// <summary>
        /// R02
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable mDivision;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from MDivision WITH (NOLOCK) ", out mDivision);
            MyUtility.Tool.SetupCombox(this.comboM, 1, mDivision);
            this.comboM.Text = Sci.Env.User.Keyword;
            MyUtility.Tool.SetupCombox(this.comboPrintType, 1, 1, "ALL,MR Not Send,MR Send Not Receive,Factory Receive");
            this.comboPrintType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateSCIDelivery.Value1))
            {
                MyUtility.Msg.WarningBox("SCI Delivery can't empty!!");
                return false;
            }

            this.mDivision = this.comboM.Text;
            this.sciDate1 = this.dateSCIDelivery.Value1;
            this.sciDate2 = this.dateSCIDelivery.Value2;
            this.prodiveDate1 = this.dateProvideDate.Value1;
            this.prodiveDate2 = this.dateProvideDate.Value2;
            this.rcvDate1 = this.dateFtyMRRcvDate.Value1;
            this.rcvDate2 = this.dateFtyMRRcvDate.Value2;
            this.brand = this.txtbrand.Text;
            this.style = this.txtstyle.Text;
            this.season = this.txtseason.Text;
            this.mr = this.txttpeuser_caneditMR.TextBox1.Text;
            this.smr = this.txttpeuser_caneditSMR.TextBox1.Text;
            this.pohandle = this.txttpeuser_caneditPOHandle.TextBox1.Text;
            this.posmr = this.txttpeuser_caneditPOSMR.TextBox1.Text;
            this.printType = this.comboPrintType.SelectedIndex;

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(string.Format(@"
select s.BrandID,s.ID,s.SeasonID,sp.MDivisionID,sp.FactoryID
	,Doc = CONCAT(sp.DOC,'-',r.Name)
	,sp.SendDate,sp.ReceiveDate,sp.SendToQA,sp.QAReceived,
    [UnnecessaryToSend] = iif(Len(isnull(sp.ReasonID,'')) = 0,'N','Y')
    ,sp.ProvideDate,sp.OrderId,sp.SCIDelivery,sp.BuyerDelivery
	,PullForward = IIF(sp.IsPF = 1,'Y','N')
	,Handle		 = CONCAT(sp.SendName,'-',(select Name from TPEPass1 WITH (NOLOCK) where ID = sp.SendName))
	,MRHandle	 = CONCAT(sp.MRHandle,'-',(select Name from TPEPass1 WITH (NOLOCK) where ID = sp.MRHandle))
	,SMR		 = CONCAT(sp.SMR,'-',(select Name from TPEPass1 WITH (NOLOCK) where ID = sp.SMR))
	,POHandle	 = CONCAT(sp.PoHandle,'-',(select Name from TPEPass1 WITH (NOLOCK) where ID = sp.PoHandle))
	,POSMR		 = CONCAT(sp.POSMR,'-',(select Name from TPEPass1 WITH (NOLOCK) where ID = sp.POSMR))
	,FtyHandle	 = CONCAT(sp.FtyHandle,'-',(select Name from Pass1 WITH (NOLOCK) where ID = sp.FtyHandle))
from Style_ProductionKits sp WITH (NOLOCK) 
inner join Style s WITH (NOLOCK) on s.Ukey = sp.StyleUkey
left join Reason r WITH (NOLOCK) on r.ReasonTypeID = 'ProductionKits' and r.ID = sp.DOC
where 1=1 "));

            if (!MyUtility.Check.Empty(this.sciDate1))
            {
                sqlCmd.Append(string.Format(@" and sp.SCIDelivery >= '{0}'", Convert.ToDateTime(this.sciDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.sciDate2))
            {
                sqlCmd.Append(string.Format(@" and sp.SCIDelivery <= '{0}'", Convert.ToDateTime(this.sciDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.prodiveDate1))
            {
                sqlCmd.Append(string.Format(@" and sp.ProvideDate >= '{0}'", Convert.ToDateTime(this.prodiveDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.prodiveDate2))
            {
                sqlCmd.Append(string.Format(@" and sp.ProvideDate <= '{0}'", Convert.ToDateTime(this.prodiveDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.rcvDate1))
            {
                sqlCmd.Append(string.Format(@" and sp.ReceiveDate >= '{0}'", Convert.ToDateTime(this.rcvDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.rcvDate2))
            {
                sqlCmd.Append(string.Format(@" and sp.ReceiveDate <= '{0}'", Convert.ToDateTime(this.rcvDate2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.brand))
            {
                sqlCmd.Append(string.Format(" and s.BrandID = '{0}'", this.brand));
            }

            if (!MyUtility.Check.Empty(this.style))
            {
                sqlCmd.Append(string.Format(" and s.ID = '{0}'", this.style));
            }

            if (!MyUtility.Check.Empty(this.season))
            {
                sqlCmd.Append(string.Format(" and s.SeasonID = '{0}'", this.season));
            }

            if (!MyUtility.Check.Empty(this.mDivision))
            {
                sqlCmd.Append(string.Format(" and sp.ProductionKitsGroup = '{0}'", this.mDivision));
            }

            if (!MyUtility.Check.Empty(this.mr))
            {
                sqlCmd.Append(string.Format(" and sp.MRHandle = '{0}'", this.mr));
            }

            if (!MyUtility.Check.Empty(this.smr))
            {
                sqlCmd.Append(string.Format(" and sp.SMR = '{0}'", this.smr));
            }

            if (!MyUtility.Check.Empty(this.pohandle))
            {
                sqlCmd.Append(string.Format(" and sp.PoHandle = '{0}'", this.pohandle));
            }

            if (!MyUtility.Check.Empty(this.posmr))
            {
                sqlCmd.Append(string.Format(" and sp.POSMR = '{0}'", this.posmr));
            }

            if (this.printType == 1)
            {
                sqlCmd.Append(" and sp.SendDate is null");
            }
            else if (this.printType == 2)
            {
                sqlCmd.Append(" and sp.SendDate is not null");
            }
            else if (this.printType == 3)
            {
                sqlCmd.Append(" and sp.ReceiveDate is not null");
            }

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

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R02_ProductionKits.xltx");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "PPIC_R02_ProductionKits.xltx", 1, true, string.Empty, objApp);

            return true;
        }
    }
}
