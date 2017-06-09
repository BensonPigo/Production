using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R20 : Sci.Win.Tems.PrintForm
    {
        string dateFormate = "yyyy-MM-dd";
        string StartBuyerDelivery, EndBuyerDelivery, StartDeadLine, EndDeadLine, StartETA, EndETA
            , StartSPNo, EndSPNo, MDivision, Factory, StartRefno, EndRefno;
        bool boolCheckQty;
        DataTable printData;

        public R20(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            #region Set Value
            StartBuyerDelivery = (dateBuyerDelivery.Value1.ToString().Empty()) ? "" : ((DateTime)dateBuyerDelivery.Value1).ToString(dateFormate);
            EndBuyerDelivery = (dateBuyerDelivery.Value2.ToString().Empty()) ? "" : ((DateTime)dateBuyerDelivery.Value2).ToString(dateFormate);
            StartDeadLine = (dateDeadLine.Value1.ToString().Empty()) ? "" : ((DateTime)dateDeadLine.Value1).ToString(dateFormate);
            EndDeadLine = (dateDeadLine.Value2.ToString().Empty()) ? "" : ((DateTime)dateDeadLine.Value2).ToString(dateFormate);
            StartETA = (dateInventoryETA.Value1.ToString().Empty()) ? "" : ((DateTime)dateInventoryETA.Value1).ToString(dateFormate);
            EndETA = (dateInventoryETA.Value2.ToString().Empty()) ? "" : ((DateTime)dateInventoryETA.Value2).ToString(dateFormate);
            StartSPNo = textStartSP.Text;
            EndSPNo = textEndSP.Text;
            MDivision = txtMdivision1.Text;
            Factory = txtfactory1.Text;
            StartRefno = textStartRefno.Text;
            EndRefno = textEndRefno.Text;
            boolCheckQty = checkQty.Checked;
            #endregion
            #region 判斷必輸條件  < Buyer Delivery >, < DeadLine >, < ETA >擇一必輸
            if ((!StartBuyerDelivery.Empty() && !EndBuyerDelivery.Empty()) ||
                (!StartDeadLine.Empty() && !EndDeadLine.Empty()) ||
                (!StartETA.Empty() && !EndETA.Empty()))
            {
                return true;
            }
            else
            {
                MyUtility.Msg.InfoBox("Buyer Delivery, DeadLine, ETA can't all be empty.");
                return false;
            }
            #endregion
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQl Parameters
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@StartBuyerDelivery", StartBuyerDelivery));
            listSqlPar.Add(new SqlParameter("@EndBuyerDelivery", EndBuyerDelivery));
            listSqlPar.Add(new SqlParameter("@StartDeadLine", StartDeadLine));
            listSqlPar.Add(new SqlParameter("@EndDeadLine", EndDeadLine));
            listSqlPar.Add(new SqlParameter("@StartETA", StartETA));
            listSqlPar.Add(new SqlParameter("@EndETA", EndETA));
            listSqlPar.Add(new SqlParameter("@StartSPNo", StartSPNo));
            listSqlPar.Add(new SqlParameter("@EndSPNo", EndSPNo));
            listSqlPar.Add(new SqlParameter("@MDivision", MDivision));
            listSqlPar.Add(new SqlParameter("@Factory", Factory));
            listSqlPar.Add(new SqlParameter("@StartRefno", StartRefno));
            listSqlPar.Add(new SqlParameter("@EndRefno", EndRefno));
            #endregion
            #region SQL Filte
            List<string> filte = new List<string>();
            if (!StartBuyerDelivery.Empty() && !EndBuyerDelivery.Empty())
            {
                filte.Add("o.BuyerDelivery between @StartBuyerDelivery and @EndBuyerDelivery");
            }
            if (!StartDeadLine.Empty() && !EndDeadLine.Empty())
            {
                filte.Add("inv.Deadline between @StartDeadLine and @EndDeadLine");
            }
            if (!StartETA.Empty() && !EndETA.Empty())
            {
                filte.Add("inv.ETA between @StartETA and @EndETA");
            }
            if (!StartSPNo.Empty() && !EndSPNo.Empty())
            {
                filte.Add("o.ID between @StartSPNo and @EndSPNo");
            }
            if (!MDivision.Empty())
            {
                filte.Add("o.MDivisionID = @MDivision");
            }
            if (!Factory.Empty())
            {
                filte.Add("o.FactoryID = @Factory");
            }
            if (!StartRefno.Empty() && !EndRefno.Empty())
            {
                filte.Add("inv.Refno between @StartRefno and @EndRefno");
            }
            if (boolCheckQty)
            {
                filte.Add("inv.Qty > 0");
            }
            #endregion
            #region SQL CMD
            string sqlCmd = string.Format(@"
select	SPNo = inv.POID
		, Seq1 = inv.Seq1
		, Seq2 = inv.Seq2
		, ProjectID = inv.ProjectID
		, Category = case 
						when o.Category = 'm' then 'MATERIAL'
						when o.Category = 'b' then 'BULK'
						when o.Category = 's' then 'SAMPLE'
					 end
		, Buyer = o.BrandID
		, CustomerPayable = inv.Payable
		, RefNo = inv.Refno
		, SuppID = fs.SuppID
		, Width = invRef.Width
		, ColorID = invRef.ColorID
		, Size = invRef.SizeSpec
		, SizeUnit = invRef.SizeUnit
		, Unit = inv.UnitID
		, Quantity = inv.Qty
		, WHActualStockQty = isnull(mpd.LInvQty, 0)
		, StockFactory = inv.FactoryID
		, Special = invref.BomZipperInsert
		, Deadline = inv.Deadline
		, OrderComfirmDate = o.CFMDate
		, AgingDays = DATEDIFF(dd, o.CFMDate, inv.Deadline)
		, ETA = inv.ETA
		, PROD = inv.InventoryRefnoID
		, HandleMR = inv.OrderHandle
from Orders o
inner join Inventory inv on o.ID = inv.POID
left join Fabric_Supp fs on inv.SCIRefno = fs.SCIRefno
left join InventoryRefno invRef on inv.InventoryRefnoID = invRef.ID
left join MDivisionPoDetail mpd on inv.POID = mpd.POID and inv.Seq2 = mpd.Seq1 and inv.Seq2 = mpd.Seq2
{0}", (filte.Count > 0) ? "Where " + filte.JoinToString("\n\r and ") : "");
            #endregion
            #region Get Data
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, listSqlPar, out printData))
            {
                return result;
            }
            #endregion
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (printData == null || printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R20.xltx");
            MyUtility.Excel.CopyToXls(printData, "", "Warehouse_R20.xltx", 2, showExcel: true, excelApp: objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Columns.AutoFit();

            if (objApp != null) Marshal.FinalReleaseComObject(objApp);
            if (worksheet != null) Marshal.FinalReleaseComObject(worksheet);
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
