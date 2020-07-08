using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R30 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string sp1;
        string sp2;
        string Style;
        string Season;
        string Brand;
        string Factory;
        string Inspected;
        DateTime? Buyerdelivery1;
        DateTime? Buyerdelivery2;
        DateTime? InspectionDate1;
        DateTime? InspectionDate2;

        public R30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            DataTable dtfactory;
            this.InitializeComponent();
            DBProxy.Current.Select(null, "select distinct factoryid from Orders WITH (NOLOCK) ", out dtfactory); // 要預設空白
            this.comboFactory.Empty();
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, dtfactory);
            this.comboFactory.Text = Sci.Env.User.Keyword;
            this.comboInspected.SelectedItem = "ALL";
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            this.sp1 = this.txtSPStart.Text;
            this.sp2 = this.txtSPEnd.Text;
            this.Buyerdelivery1 = this.dateBuyerdelivery.Value1;
            this.Buyerdelivery2 = this.dateBuyerdelivery.Value2;
            this.InspectionDate1 = this.dateInspectionDate.Value1;
            this.InspectionDate2 = this.dateInspectionDate.Value2;
            this.Style = this.txtStyle.Text;
            this.Season = this.txtSeason.Text;
            this.Brand = this.txtBrand.Text;
            this.Factory = this.comboFactory.Text;
            this.Inspected = this.comboInspected.Text;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"
select 
 [SP#]=a.id
,[Style#]=a.StyleID
,[Season]=a.SeasonID
,[Brand]=a.BrandID
,[Factory]=a.FactoryID
,[Buyer Delivery]=a.BuyerDelivery
,[Item]=b.Item
,[Color]=b.Colorid
,[Inspection Date]=b.InspDate
,[Result]=b.Result

from dbo.orders a WITH (NOLOCK) 
LEFT join dbo.MD b WITH (NOLOCK) on b.ID = a.ID
where 1=1
");
            #region Append畫面上的條件
            if (!MyUtility.Check.Empty(this.sp1))
            {
                sqlCmd.Append(string.Format(" and a.id >='{0}'", this.sp1));
            }

            if (!MyUtility.Check.Empty(this.sp2))
            {
                sqlCmd.Append(string.Format(" and a.id <='{0}'", this.sp2));
            }

            if (!MyUtility.Check.Empty(this.Buyerdelivery1))
            {
                sqlCmd.Append(string.Format(@" and a.BuyerDelivery >= '{0}'", Convert.ToDateTime(this.Buyerdelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Buyerdelivery2))
            {
                sqlCmd.Append(string.Format(@" and a.BuyerDelivery <= '{0}'", Convert.ToDateTime(this.Buyerdelivery2).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Style))
            {
                sqlCmd.Append(string.Format(" and a.StyleID <='{0}'", this.Style));
            }

            if (!MyUtility.Check.Empty(this.Season))
            {
                sqlCmd.Append(string.Format(" and a.SeasonID <='{0}'", this.Season));
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                sqlCmd.Append(string.Format(" and a.BrandID <='{0}'", this.Brand));
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                sqlCmd.Append(string.Format(" and a.FactoryID ='{0}'", this.Factory));
            }
            #endregion

            #region 根據Inspected Append畫面上的條件
            if (this.Inspected == "With Inspection Date")
            {
                sqlCmd.Append(@" and b.InspDate is not null");
            }

            if (this.Inspected == "Without Inspection Date")
            {
                sqlCmd.Append(@" and b.InspDate is null");
            }

            if (!MyUtility.Check.Empty(this.InspectionDate1))
            {
                sqlCmd.Append(string.Format(@" and a.InspDate >= '{0}'", Convert.ToDateTime(this.InspectionDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.InspectionDate2))
            {
                sqlCmd.Append(string.Format(@" and a.InspDate <= '{0}'", Convert.ToDateTime(this.InspectionDate2).ToString("d")));
            }
            #endregion

            sqlCmd.Append(@"
order by a.ID
");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        // 產生Excel
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R30.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Quality_R30.xltx", 5, false, null, objApp); // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (!MyUtility.Check.Empty(this.sp1))
            {
                objSheets.Cells[2, 2] = this.sp1 + "~" + this.sp2;
            }

            if (!MyUtility.Check.Empty(this.Buyerdelivery1))
            {
                c.Append(string.Format(@"{0}", Convert.ToDateTime(this.Buyerdelivery1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(this.Buyerdelivery2))
            {
                c.Append(string.Format(@"~ {0}", Convert.ToDateTime(this.Buyerdelivery2).ToString("d")));
            }

            objSheets.Cells[3, 2] = c.ToString();

            if (!MyUtility.Check.Empty(this.InspectionDate1))
            {
                objSheets.Cells[4, 2] = Convert.ToDateTime(this.InspectionDate1).ToString("d") + "~" + Convert.ToDateTime(this.InspectionDate2).ToString("d");
            }

            objSheets.Cells[2, 6] = this.Style;
            objSheets.Cells[3, 6] = this.Brand;
            objSheets.Cells[4, 6] = this.comboInspected.Text;
            objSheets.Cells[2, 8] = this.Season;
            objSheets.Cells[3, 8] = this.Factory;

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R30");
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
