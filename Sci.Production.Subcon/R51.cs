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

namespace Sci.Production.Subcon
{
    public partial class R51 : Sci.Win.Tems.PrintForm
    {
        string DateFormate = "yyyy-MM-dd";
        string StartDate, EndDate, ID, Factory, LocalSupplier;
        DataTable printData;

        public R51(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            #region Set Value
            StartDate = (dateDate.Value1.ToString().Empty()) ? "" : ((DateTime)dateDate.Value1).ToString(DateFormate);
            EndDate = (dateDate.Value2.ToString().Empty()) ? "" : ((DateTime)dateDate.Value2).ToString(DateFormate);
            ID = textID.Text;
            Factory = txtfactory1.Text;
            LocalSupplier = txtLocalSupp1.TextBox1.Text;
            #endregion 
            return true;
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQl Parameters
            List<SqlParameter> listSqlPar = new List<SqlParameter>();
            listSqlPar.Add(new SqlParameter("@StartDate", StartDate));
            listSqlPar.Add(new SqlParameter("@EndDate", EndDate));
            listSqlPar.Add(new SqlParameter("@ID", ID));
            listSqlPar.Add(new SqlParameter("@Factory", Factory));
            listSqlPar.Add(new SqlParameter("@LocalSupplier", LocalSupplier));
            #endregion
            #region SQL Filte
            List<string> filte = new List<string>();
            if (!StartDate.Empty() && !EndDate.Empty())
            {
                filte.Add(@"
            (Convert (date, AP.AddDate)  >= @StartDate
			Or Convert (date, AP.EditDate) >= @StartDate)
		And (Convert (date, AP.AddDate)  <= @EndDate
			Or Convert (date, AP.EditDate)  <= @EndDate)");
            }
            if (!ID.Empty())
            {
                filte.Add("AP.ID = @ID");
            }
            if (!Factory.Empty())
            {
                filte.Add("AP.FactoryId = @Factory");
            }
            if (!LocalSupplier.Empty())
            {
                filte.Add("AP.LocalSuppID = @LocalSupplier");
            }
            #endregion
            #region SQL CMD
            string sqlCmd = string.Format(@"
select	AP.ID
		, AP.FactoryId
		, AP.Remark
		, AP.Handle
		, AP.CurrencyId
		, Amount = Sum(sum(OQ.Qty)*OA.Cost) over(partition by AP.ID)
		, AP.VatRate
		, AP.Vat
		, AP.AddName
		, AP.AddDate
		, AP.EditName
		, AP.EditDate 
		, APD.OrderID
		, O.StyleID
		, O.BrandID
		, O.SeasonID
		, APD.PatternCode
		, APD.PatternDesc
		, APD.Farmout
		, APD.Farmin
		, APD.PoQty
		, APD.ArtworkTypeID
		, FirstCutDate = isnull(C.FirstCutDate,O.CutInLine)
		, AP.Delivery
		, OA.Article,QTY = sum(OQ.Qty) 
		, UnitPrice = OA.Cost 
		, Detail_Amount = sum(OQ.Qty)*OA.Cost		
From ArtworkPO AP
Inner join ArtworkPO_Detail APD on AP.ID=APD.ID
Inner join Orders O on APD.OrderID=O.ID
left join Cutting C on O.ID=C.ID    
Inner join dbo.View_Order_Artworks OA on  OA.ID=APD.OrderID and OA.PatternCode=APD.PatternCode and OA.ArtworkID=APD.ArtworkId and OA.ArtworkTypeID=APD.ArtworkTypeID
Inner join Order_Qty OQ on OQ.ID=OA.ID and OQ.SizeCode=OA.SizeCode
Where	AP.POType='O' 
		and APD.ArtworkTypeID='PRINTING' 
		and AP.Status <> 'New' 
		{0}
group by AP.ID, AP.FactoryId, AP.Remark, AP.Handle, AP.CurrencyId, AP.VatRate, AP.Vat, AP.AddName, AP.AddDate, AP.EditName, AP.EditDate 
		 , APD.OrderID, O.StyleID, O.BrandID, O.SeasonID, APD.PatternCode, APD.PatternDesc, APD.Farmout, APD.Farmin, APD.PoQty, APD.ArtworkTypeID
		 , isnull(C.FirstCutDate,O.CutInLine), AP.Delivery, OA.Article,OA.Cost"
                , (filte.Count > 0) ? "and " + filte.JoinToString("\n\r and ") : "");
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
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R51.xltx");
            MyUtility.Excel.CopyToXls(printData, "", "Subcon_R51.xltx", 1, showExcel: true, excelApp: objApp);
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
