using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// R09
    /// </summary>
    public partial class R09 : Sci.Win.Tems.PrintForm
    {
        private string startUpdate;
        private string endUpdate;
        private string sp;
        private string mdivision;
        private string factory;
        private DataTable dtPrint;

        /// <summary>
        /// R09
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public R09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            #region Initialize
            DataRow drOC;
            if (MyUtility.Check.Seek(
                string.Format(
                    @"
select top 1 UpdateDate 
from OrderComparisonList WITH (NOLOCK) 
where   MDivisionID = '{0}' 
        and UpdateDate = (select max(UpdateDate) 
                          from OrderComparisonList WITH (NOLOCK) 
                          where MDivisionID = '{0}')",
                Sci.Env.User.Keyword), out drOC))
            {
                this.dateUpdate.Value1 = Convert.ToDateTime(drOC["UpdateDate"]);
                this.dateUpdate.Value2 = Convert.ToDateTime(drOC["UpdateDate"]);
            }

            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
            #endregion
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            #region Set Input Data
            this.startUpdate = (!this.dateUpdate.Value1.ToString().Empty()) ? ((DateTime)this.dateUpdate.Value1).ToString("yyyy/MM/dd") : string.Empty;
            this.endUpdate = (!this.dateUpdate.Value2.ToString().Empty()) ? ((DateTime)this.dateUpdate.Value2).ToString("yyyy/MM/dd") : string.Empty;
            this.sp = this.textSP.Text;
            this.mdivision = this.txtMdivision.Text;
            this.factory = this.txtfactory.Text;
            #endregion
            #region Validate Update & SP
            if (!this.startUpdate.Empty() || !this.endUpdate.Empty() || !this.sp.Empty())
            {
                return true;
            }
            else
            {
                MyUtility.Msg.InfoBox("[Updated Date] or [SP#] must enter one!");
                return false;
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            List<string> listFilte = new List<string>();
            #region SqlParameter
            listSqlParameter.Add(new SqlParameter("@startUpdate", this.startUpdate));
            listSqlParameter.Add(new SqlParameter("@endUpdate", this.endUpdate));
            listSqlParameter.Add(new SqlParameter("@sp", this.sp));
            listSqlParameter.Add(new SqlParameter("@mdivision", this.mdivision));
            listSqlParameter.Add(new SqlParameter("@factory", this.factory));
            #endregion
            #region Sql Filte
            /*
             * UpdateDate
             */
            if (!this.startUpdate.Empty() && !this.endUpdate.Empty())
            {
                listFilte.Add("UpdateDate between @startUpdate and @endUpdate");
            }
            else if (!this.startUpdate.Empty())
            {
                listFilte.Add("@startUpdate <= UpdateDate");
            }
            else if (!this.endUpdate.Empty())
            {
                listFilte.Add("UpdateDate <= @endUpdate");
            }

            /*
             * SP
             */
            if (!this.sp.Empty())
            {
                listFilte.Add("OrderID = @SP");
            }

            /*
             * MDivision
             */
            if (!this.mdivision.Empty())
            {
                listFilte.Add("MDivisionID = @MDivision");
            }

            /*
             * Factory
             */
            if (!this.factory.Empty())
            {
                listFilte.Add("FactoryID = @Factory");
            }
            #endregion
            #region Sql Commmand
            string strCmd = string.Format(
                @"
select	UpdateDate
		, FactoryID
		, OrderId
		, OriginalStyleID
        , OriginalCustPONo
		, OriginalQty			= iif (convert (varchar, OriginalQty) = 0, ''
																		 , convert (varchar, OriginalQty))
		, OriginalBuyerDelivery	= RIGHT (CONVERT (VARCHAR (20), OriginalBuyerDelivery, 111), 5) 
		, OriginalSCIDelivery	= RIGHT (CONVERT (VARCHAR (20), OriginalSCIDelivery,111), 5) 
		, OriginalLETA			= RIGHT (CONVERT (VARCHAR (20), OriginalLETA,111), 5) 
        , OriginalShipModeList
		, KPILETA				= RIGHT (CONVERT (VARCHAR (20), KPILETA,111), 5) 
		, TransferToFactory
        , NewCustPONo
		, NewQty				= iif (convert (varchar, NewQty) = 0, ''
																	, convert (varchar, NewQty)) 
		, NewBuyerDelivery		= RIGHT (CONVERT (VARCHAR (20), NewBuyerDelivery, 111), 5) 
		, NewSCIDelivery		= RIGHT (CONVERT (VARCHAR(20), NewSCIDelivery, 111), 5) 
		, NewLETA				= RIGHT (CONVERT (VARCHAR(20), NewLETA, 111), 5) 
        , NewShipModeList
		, NewOrder				= IIF (NewOrder = 1, 'V', '') 
		, DeleteOrder			= iif (DeleteOrder=1, 'V', '')  
		, JunkOrder				= iif (JunkOrder=1, 'V', '') 
		, EachConsApv			= iif (NewEachConsApv is null, iif (OriginalEachConsApv is null, '', '★'), 'V') 
		, NewMnorder			= iif (NewMnorderApv is null, '', 'V')
		, NewSMnorderApv		= iif (NewSMnorderApv is null, '', 'V')
		, MnorderApv2			= iif (MnorderApv2 is null, '', 'V')
from OrderComparisonList WITH (NOLOCK) 
{0}
order by FactoryID, OrderId",
                (listFilte.Count == 0) ? string.Empty : "where\t" + listFilte.JoinToString("\n\tand "));
            #endregion
            #region Sql Process
            DualResult result;
            result = DBProxy.Current.Select(null, strCmd, listSqlParameter, out this.dtPrint);
            if (!result)
            {
                return result;
            }
            #endregion
            return new Ict.DualResult(true);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check Print Data
            if (this.dtPrint == null || this.dtPrint.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                this.SetCount(0);
                return false;
            }
            #endregion
            this.SetCount(this.dtPrint.Rows.Count);
            this.ShowWaitMessage("Excel processing...");
            #region Export Excel
            Excel.Application objApp = null;
            objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R09.xltx");
            MyUtility.Excel.CopyToXls(this.dtPrint, string.Empty, "PPIC_R09.xltx", 1, showExcel: true, excelApp: objApp);
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
