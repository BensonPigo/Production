using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci;
using Ict;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.PPIC
{
    public partial class R09 : Sci.Win.Tems.PrintForm
    {
        string startUpdate, endUpdate, sp, mdivision, factory;
        DataTable dtPrint;
        public R09(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            #region Initialize
            DataRow drOC;
            if (MyUtility.Check.Seek(string.Format(@"
select top 1 UpdateDate 
from OrderComparisonList WITH (NOLOCK) 
where   MDivisionID = '{0}' 
        and UpdateDate = (select max(UpdateDate) 
                          from OrderComparisonList WITH (NOLOCK) 
                          where MDivisionID = '{0}')"
                , Sci.Env.User.Keyword), out drOC))
            {
                dateUpdate.Value1 = Convert.ToDateTime(drOC["UpdateDate"]);
                dateUpdate.Value2 = Convert.ToDateTime(drOC["UpdateDate"]);
            }
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
            #endregion 
        }

        protected override bool ValidateInput()
        {
            #region Set Input Data
            startUpdate = (!dateUpdate.Value1.ToString().Empty()) ? ((DateTime)dateUpdate.Value1).ToString("yyyy/MM/dd") : "";
            endUpdate = (!dateUpdate.Value2.ToString().Empty()) ? ((DateTime)dateUpdate.Value2).ToString("yyyy/MM/dd") : "";
            sp = textSP.Text;
            mdivision = txtMdivision.Text;
            factory = txtfactory.Text;
            #endregion 
            #region Validate Update & SP
            if (!startUpdate.Empty() || !endUpdate.Empty() || !sp.Empty())
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

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            List<string> listFilte = new List<string>();
            #region SqlParameter
            listSqlParameter.Add(new SqlParameter("@startUpdate", startUpdate));
            listSqlParameter.Add(new SqlParameter("@endUpdate", endUpdate));
            listSqlParameter.Add(new SqlParameter("@sp", sp));
            listSqlParameter.Add(new SqlParameter("@mdivision", mdivision));
            listSqlParameter.Add(new SqlParameter("@factory", factory));
            #endregion
            #region Sql Filte
            /*
             * UpdateDate
             */
            if (!startUpdate.Empty() && !endUpdate.Empty())
            {
                listFilte.Add("UpdateDate between @startUpdate and @endUpdate");
            }
            else if (!startUpdate.Empty())
            {
                listFilte.Add("@startUpdate <= UpdateDate");
            }
            else if (!endUpdate.Empty())
            {
                listFilte.Add("UpdateDate <= @endUpdate");
            }

            /*
             * SP
             */
            if (!sp.Empty())
            {
                listFilte.Add("OrderID = @SP");
            }

            /*
             * MDivision
             */
            if (!mdivision.Empty())
            {
                listFilte.Add("MDivisionID = @MDivision");
            }

            /*
             * Factory
             */
            if (!factory.Empty())
            {
                listFilte.Add("FactoryID = @Factory");
            }
            #endregion 
            #region Sql Commmand
            string strCmd = string.Format(@"
select	UpdateDate
		, FactoryID
		, OrderId
		, OriginalStyleID
		, OriginalQty			= iif (convert (varchar, OriginalQty) = 0, ''
																		 , convert (varchar, OriginalQty))
		, OriginalBuyerDelivery	= RIGHT (CONVERT (VARCHAR (20), OriginalBuyerDelivery, 111), 5) 
		, OriginalSCIDelivery	= RIGHT (CONVERT (VARCHAR (20), OriginalSCIDelivery,111), 5) 
		, OriginalLETA			= RIGHT (CONVERT (VARCHAR (20), OriginalLETA,111), 5) 
		, KPILETA				= RIGHT (CONVERT (VARCHAR (20), KPILETA,111), 5) 
		, TransferToFactory
		, NewQty				= iif (convert (varchar, NewQty) = 0, ''
																	, convert (varchar, NewQty)) 
		, NewBuyerDelivery		= RIGHT (CONVERT (VARCHAR (20), NewBuyerDelivery, 111), 5) 
		, NewSCIDelivery		= RIGHT (CONVERT (VARCHAR(20), NewSCIDelivery, 111), 5) 
		, NewLETA				= RIGHT (CONVERT (VARCHAR(20), NewLETA, 111), 5) 
		, NewOrder				= IIF (NewOrder = 1, 'V', '') 
		, DeleteOrder			= iif (DeleteOrder=1, 'V', '')  
		, JunkOrder				= iif (JunkOrder=1, 'V', '') 
		, CMPQDate				= iif (NewCMPQDate is null, '', 'V') 
		, EachConsApv			= iif (NewEachConsApv is null, iif (OriginalEachConsApv is null, '', '★'), 'V') 
		, NewMnorder			= iif (NewMnorderApv is null, '', 'V')
		, NewSMnorderApv		= iif (NewSMnorderApv is null, '', 'V')
		, MnorderApv2			= iif (MnorderApv2 is null, '', 'V')
from OrderComparisonList WITH (NOLOCK) 
{0}
order by FactoryID, OrderId"
                , (listFilte.Count == 0) ? "" : "where\t" + listFilte.JoinToString("\n\tand "));
            #endregion             
            #region Sql Process
            DualResult result;
            result = DBProxy.Current.Select(null, strCmd, listSqlParameter, out dtPrint);
            if (!result)
            {
                return result;
            }
            #endregion 
            return new Ict.DualResult(true);
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check Print Data
            if (dtPrint == null || dtPrint.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                this.SetCount(0);
                return false;
            }
            #endregion 
            this.SetCount(dtPrint.Rows.Count);
            this.ShowWaitMessage("Excel processing...");
            #region Export Excel
            Excel.Application objApp = null;
            objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\PPIC_R09.xltx");
            MyUtility.Excel.CopyToXls(dtPrint, "", "PPIC_R09.xltx", 1, showExcel: true, excelApp: objApp);
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            #endregion 
            this.HideWaitMessage();
            return true;
        }
    }
}
