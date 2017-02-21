using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R30 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        string sp1, sp2, Style, Season, Brand, Factory, Inspected;
        DateTime? Buyerdelivery1, Buyerdelivery2, InspectionDate1, InspectionDate2;

        public R30(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            DataTable dtfactory;
            InitializeComponent();
            DBProxy.Current.Select(null, "select distinct factoryid from Orders WITH (NOLOCK) ", out dtfactory);//要預設空白
            comboFactory.Empty();
            MyUtility.Tool.SetupCombox(comboFactory, 1, dtfactory);
            comboFactory.Text = Sci.Env.User.Keyword;
            comboInspected.SelectedItem = "ALL";
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            sp1 = textSP1.Text;
            sp2 = textSP2.Text;
            Buyerdelivery1 = dateBuyerdelivery.Value1;
            Buyerdelivery2 = dateBuyerdelivery.Value2;
            InspectionDate1 = dateInspectionDate.Value1;
            InspectionDate2 = dateInspectionDate.Value2;
            Style = textStyle.Text;
            Season = textSeason.Text;
            Brand = textBrand.Text;
            Factory = comboFactory.Text;
            Inspected = comboInspected.Text;

            return base.ValidateInput();
        }

        //非同步取資料
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
            if (!MyUtility.Check.Empty(sp1))
            {
                sqlCmd.Append(string.Format(" and a.id >='{0}'", sp1));
            }
            if (!MyUtility.Check.Empty(sp2))
            {
                sqlCmd.Append(string.Format(" and a.id <='{0}'", sp2));
            }

            if (!MyUtility.Check.Empty(Buyerdelivery1))
            {
                sqlCmd.Append(string.Format(@" and a.BuyerDelivery >= '{0}'", Convert.ToDateTime(Buyerdelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(Buyerdelivery2))
            {
                sqlCmd.Append(string.Format(@" and a.BuyerDelivery <= '{0}'", Convert.ToDateTime(Buyerdelivery2).ToString("d")));
            }
            if (!MyUtility.Check.Empty(Style))
            {
                sqlCmd.Append(string.Format(" and a.StyleID <='{0}'", Style));
            }
            if (!MyUtility.Check.Empty(Season))
            {
                sqlCmd.Append(string.Format(" and a.SeasonID <='{0}'", Season));
            }
            if (!MyUtility.Check.Empty(Brand))
            {
                sqlCmd.Append(string.Format(" and a.BrandID <='{0}'", Brand));
            }
            if (!MyUtility.Check.Empty(Factory))
            {
                sqlCmd.Append(string.Format(" and a.FactoryID <='{0}'", Factory));
            }
            #endregion

            #region 根據Inspected Append畫面上的條件
            if (Inspected == "With Inspection Date")
            {
                sqlCmd.Append(@" and b.InspDate is not null");
            }
            if (Inspected == "Without Inspection Date")
            {
                sqlCmd.Append(@" and b.InspDate is null");
            }
            if (!MyUtility.Check.Empty(InspectionDate1))
            {
                sqlCmd.Append(string.Format(@" and a.InspDate >= '{0}'", Convert.ToDateTime(InspectionDate1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(InspectionDate2))
            {
                sqlCmd.Append(string.Format(@" and a.InspDate <= '{0}'", Convert.ToDateTime(InspectionDate2).ToString("d")));
            }
            #endregion

            sqlCmd.Append(@"
order by a.ID
");

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out printData);
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
            SetCount(printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Quality_R30.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(printData, "", "Quality_R30.xltx", 5, true, null, objApp);// 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            if (!MyUtility.Check.Empty(sp1))
            {
                objSheets.Cells[2, 2] = sp1 + "~" + sp2;
            }
            if (!MyUtility.Check.Empty(Buyerdelivery1))
            {
                c.Append(string.Format(@"{0}", Convert.ToDateTime(Buyerdelivery1).ToString("d")));
            }
            if (!MyUtility.Check.Empty(Buyerdelivery2))
            {
                c.Append(string.Format(@"~ {0}", Convert.ToDateTime(Buyerdelivery2).ToString("d")));
            }
            objSheets.Cells[3, 2] = c.ToString();

            if (!MyUtility.Check.Empty(InspectionDate1))
            {
                objSheets.Cells[4, 2] = Convert.ToDateTime(InspectionDate1).ToString("d") + "~" + Convert.ToDateTime(InspectionDate2).ToString("d");
            }
            objSheets.Cells[2, 6] = Style;
            objSheets.Cells[3, 6] = Brand;
            objSheets.Cells[4, 6] = comboInspected.Text;
            objSheets.Cells[2, 8] = Season;
            objSheets.Cells[3, 8] = Factory;

            if (objSheets != null) Marshal.FinalReleaseComObject(objSheets);    //釋放sheet
            if (objApp != null) Marshal.FinalReleaseComObject(objApp);          //釋放objApp
            return true;
        }
    }
}
