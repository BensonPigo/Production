﻿using Ict;
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
    /// <summary>
    /// R51
    /// </summary>
    public partial class R51 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private string supplierID;
        private string supplierAbb;

        /// <summary>
        /// R51
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R51(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtLocalSupp1.TextBox1.Text = MyUtility.GetValue.Lookup(@"SELECT TOP 1 PrintingSuppID FROM [Production].[dbo].SYSTEM");
        }

        // 驗證輸入條件

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.supplierID = this.txtLocalSupp1.TextBox1.Text;
            this.supplierAbb = this.txtLocalSupp1.DisplayBox1.Text;

            // Supplier 為必輸條件
            if (MyUtility.Check.Empty(this.supplierID))
            {
                MyUtility.Msg.InfoBox("Supplier cannot be empty.");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region SQL CMD
            string sqlCmd = string.Format($@"
select	distinct
        AP.ID
		,O.poid
		, AP.FactoryId
		, AP.Remark
		, AP.Handle
		, AP.CurrencyId
		, Amount = Sum(APD.PoQty * APD.Cost) over(partition by AP.ID)
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
		, OQ.Article
        , APD.SizeCode
		, QTY = APD.PoQty
		, UnitPrice = APD.Cost 
		, Detail_Amount = APD.PoQty * APD.Cost		
		, O.AddDate as OrderAdddate
		, O.EditDate as OrderEditDate
        , [BuyerDelivery] = o2.BuyerDelivery
        into #tmpArtworkPOBySize
From ArtworkPO AP
Inner join ArtworkPO_Detail APD on AP.ID=APD.ID
Inner join Orders O on APD.OrderID=O.ID
left join Cutting C on O.ID=C.ID    
Inner join Order_Qty OQ on  OQ.ID = APD.OrderID and 
                            (OQ.SizeCode = APD.SizeCode or APD.SizeCode = '') and 
                            (OQ.Article = APD.Article or APD.Article = '')
outer apply (
	select [BuyerDelivery] = max(o2.BuyerDelivery), [SciDelivery] = min(o2.SciDelivery)
	from Orders o2 with (nolock)
	where o2.OrderComboID = o.OrderComboID
	group by o2.OrderComboID
) o2
Where	AP.POType='O' 
		and APD.ArtworkTypeID='PRINTING' 
        and AP.LocalSuppID = '{this.supplierID}'
		and AP.Status <> 'New'
		And 
            (Convert (date, AP.AddDate)  >= Convert(date, DATEADD(m, -2, GETDATE()))
		Or Convert (date, AP.EditDate) >=Convert(date, DATEADD(d, -7, GETDATE())) )

--By Article作最後呈現
select  ID
		, poid
		, FactoryId
		, Remark
		, Handle
		, CurrencyId
		, Amount = sum(Amount)
		, VatRate
		, Vat
		, AddName
		, APAddDate
		, EditName
		, APEditDate
		, OrderID
		, StyleID
		, BrandID
		, SeasonID
		, PatternCode
		, PatternDesc
		, Farmout = sum(Farmout)
		, Farmin = sum(Farmin)
		, PoQty = sum(PoQty)
		, ArtworkTypeID
		, FirstCutDate
		, Delivery
		, Article
		, QTY = sum(QTY)
		, UnitPrice = AVG(UnitPrice)
		, Detail_Amount = sum(Detail_Amount)	
		, OrderAdddate
		, OrderEditDate
        , BuyerDelivery
from #tmpArtworkPOBySize
group by    ID
		    , poid
		    , FactoryId
		    , Remark
		    , Handle
		    , CurrencyId
            , VatRate
		    , Vat
		    , AddName
		    , APAddDate
		    , EditName
		    , APEditDate
		    , OrderID
		    , StyleID
		    , BrandID
		    , SeasonID
		    , PatternCode
		    , PatternDesc
            , ArtworkTypeID
		    , FirstCutDate
		    , Delivery
		    , Article
			, OrderAdddate
			, OrderEditDate
            , BuyerDelivery
");
            #endregion
            #region Get Data
            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlCmd, out this.printData))
            {
                return result;
            }
            #endregion
            return Result.True;
        }

        /// <inheritdoc/>
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
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Subcon_R51.xltx");
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R51.xltx", 2, showExcel: false, excelApp: objApp);
            Excel.Worksheet worksheet = objApp.Sheets[1];
            worksheet.Cells[1, 2] = DateTime.Today.AddMonths(-2).ToShortDateString();
            worksheet.Cells[1, 4] = DateTime.Today.ToShortDateString();
            worksheet.Cells[1, 6] = this.supplierID;
            worksheet.Cells[1, 7] = this.supplierAbb;
            worksheet.Columns.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Subcon_R51");
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
