using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Subcon
{
    public partial class R51 : Win.Tems.PrintForm
    {
        // string DateFormate = "yyyy-MM-dd";
        // string StartDate, EndDate, ID, Factory, LocalSupplier;
        DataTable printData;

        public R51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        // protected override bool ValidateInput()
        // {
        //    #region Set Value
        //    StartDate = (dateDate.Value1.ToString().Empty()) ? "" : ((DateTime)dateDate.Value1).ToString(DateFormate);
        //    EndDate = (dateDate.Value2.ToString().Empty()) ? "" : ((DateTime)dateDate.Value2).ToString(DateFormate);
        //    ID = textID.Text;
        //    Factory = txtfactory1.Text;
        //    LocalSupplier = txtLocalSupp1.TextBox1.Text;
        //    #endregion
        //    return true;
        // }
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // #region SQl Parameters
            // List<SqlParameter> listSqlPar = new List<SqlParameter>();
            // listSqlPar.Add(new SqlParameter("@StartDate", StartDate));
            // listSqlPar.Add(new SqlParameter("@EndDate", EndDate));
            // listSqlPar.Add(new SqlParameter("@ID", ID));
            // listSqlPar.Add(new SqlParameter("@Factory", Factory));
            // listSqlPar.Add(new SqlParameter("@LocalSupplier", LocalSupplier));
            // #endregion
//            #region SQL Filte
//            List<string> filte = new List<string>();
//            if (!StartDate.Empty() && !EndDate.Empty())
//            {
//                filte.Add(@"
//            (Convert (date, AP.AddDate)  >= @StartDate
// Or Convert (date, AP.EditDate) >= @StartDate)
// And (Convert (date, AP.AddDate)  <= @EndDate
// Or Convert (date, AP.EditDate)  <= @EndDate)");
//            }
//            if (!ID.Empty())
//            {
//                filte.Add("AP.ID = @ID");
//            }
//            if (!Factory.Empty())
//            {
//                filte.Add("AP.FactoryId = @Factory");
//            }
//            if (!LocalSupplier.Empty())
//            {
//                filte.Add("AP.LocalSuppID = @LocalSupplier");
//            }
//            #endregion
            #region SQL CMD
            string sqlCmd = string.Format(@"
select	AP.ID
		,O.poid
		, AP.FactoryId
		, AP.Remark
		, AP.Handle
		, AP.CurrencyId
		, Amount = Sum(APD.PoQty * OA.Cost) over(partition by AP.ID)
		, AP.VatRate
		, AP.Vat
		, AP.AddName
		, AP.AddDate as APAddDate
		, AP.EditName
		, AP.EditDate as APEditDate
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
		, [Delivery] = o2.SciDelivery
		, OA.Article
		, QTY = APD.PoQty
		, UnitPrice = OA.Cost 
		, Detail_Amount = APD.PoQty * OA.Cost		
		, O.AddDate as OrderAdddate
		, O.EditDate as OrderEditDate
        , [BuyerDelivery] = o2.BuyerDelivery
From ArtworkPO AP
Inner join ArtworkPO_Detail APD on AP.ID=APD.ID
Inner join Orders O on APD.OrderID=O.ID
left join Cutting C on O.ID=C.ID    
Inner join dbo.View_Order_Artworks OA on  OA.ID=APD.OrderID and OA.PatternCode=APD.PatternCode and OA.ArtworkID=APD.ArtworkId and OA.ArtworkTypeID=APD.ArtworkTypeID
Inner join Order_Qty OQ on OQ.ID=OA.ID and OQ.SizeCode=OA.SizeCode and OQ.Article=OA.Article
outer apply (
	select [BuyerDelivery] = max(o2.BuyerDelivery), [SciDelivery] = min(o2.SciDelivery)
	from Orders o2 with (nolock)
	where o2.OrderComboID = o.OrderComboID
	group by o2.OrderComboID
) o2
Where	AP.POType='O' 
		and APD.ArtworkTypeID='PRINTING' 
		and AP.Status <> 'New' 
		And  AP.LocalSuppID = (SELECT TOP 1 PrintingSuppID FROM [Production].[dbo].SYSTEM )
		And 
            (Convert (date, AP.AddDate)  >= Convert(date, DATEADD(m, -2, GETDATE()))
		Or Convert (date, AP.EditDate) >=Convert(date, DATEADD(d, -7, GETDATE())) )

group by AP.ID, O.poid,AP.FactoryId, AP.Remark, AP.Handle, AP.CurrencyId, AP.VatRate, AP.Vat, AP.AddName, AP.AddDate, AP.EditName, AP.EditDate 
		 , APD.OrderID, O.StyleID, O.BrandID, O.SeasonID, APD.PatternCode, APD.PatternDesc, APD.Farmout, APD.Farmin, APD.PoQty, APD.ArtworkTypeID
		 , isnull(C.FirstCutDate,O.CutInLine), o2.SciDelivery, OA.Article,OA.Cost,O.AddDate,O.EditDate, o2.BuyerDelivery");

                // , (filte.Count > 0) ? "and " + filte.JoinToString("\n\r and ") : "");
            #endregion
            #region Get Data
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, out this.printData))
            {
                return result;
            }
            #endregion
            return Ict.Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region check printData
            if (this.printData == null || this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Data not found.");
                return false;
            }
            #endregion
            this.SetCount(this.printData.Rows.Count);
            this.ShowWaitMessage("Excel Processing");
            #region To Excel
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R51.xltx");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R51.xltx", 2, showExcel: false, excelApp: objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[1, 2] = DateTime.Today.AddMonths(-2).ToShortDateString();
            worksheet.Cells[1, 4] = DateTime.Today.ToShortDateString();
            worksheet.Cells[1, 6] = MyUtility.GetValue.Lookup(@"SELECT TOP 1 PrintingSuppID FROM [Production].[dbo].SYSTEM");
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subcon_R51");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            #endregion
            this.HideWaitMessage();
            return true;
        }
    }
}
