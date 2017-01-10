using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Planning
{
    public partial class R03 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;
        DateTime? sciDate1, sciDate2;
        string style, brand, season, smr, subcon;
        public R03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        // 驗證輸入條件
        protected override bool ValidateInput()
        {
            style = txtstyle1.Text;
            brand = txtbrand1.Text;
            season = txtseason1.Text;
            smr = txttpeuser_canedit1.TextBox1.Text;
            subcon = txtsubcon1.TextBox1.Text;
            sciDate1 = dateRange1.Value1;
            sciDate2 = dateRange1.Value2;

            return base.ValidateInput();
        }

        // 非同步取資料
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append(@"select distinct s.ID,s.BrandID,s.SeasonID,sa.ArtworkTypeID,sa.Article,sa.ArtworkID,sa.ArtworkName,
sa.PatternCode,sa.PatternDesc,sa.TMS,sa.Qty,isnull(a.ArtworkUnit,'') as ArtworkUnit,sa.Cost,
saq.LocalSuppId+'-'+isnull(ls.Abb,'') as LocalSupp,saq.CurrencyId,saq.Price,saq.Oven,saq.Wash,saq.Mockup,saq.PriceApv
from Orders o
inner join Style s on o.StyleUkey = s.Ukey
inner join Style_Artwork sa on sa.StyleUkey = o.StyleUkey and sa.StyleUkey = s.Ukey
inner join Style_Artwork_Quot saq on sa.Ukey = saq.Ukey
left join LocalSupp ls on ls.ID = saq.LocalSuppId
left join ArtworkType a on a.ID = sa.ArtworkTypeID
where 1 = 1");

            if (!MyUtility.Check.Empty(style))
            {
                sqlCmd.Append(string.Format(@" and s.ID = '{0}'
and o.StyleID = '{0}'", style));
            }

            if (!MyUtility.Check.Empty(brand))
            {
                sqlCmd.Append(string.Format(@" and s.BrandID = '{0}'
and o.BrandID = '{0}'", brand));
            }

            if (!MyUtility.Check.Empty(season))
            {
                sqlCmd.Append(string.Format(@" and s.SeasonID = '{0}'
and o.SeasonID = '{0}'", season));
            }

            if (!MyUtility.Check.Empty(smr))
            {
                sqlCmd.Append(string.Format(@" and ((s.Phase = 'Sample' and s.SampleSMR = '{0}') or (s.Phase ='Bulk' and s.BulkSMR = '{0}'))", smr));
            }

            if (!MyUtility.Check.Empty(subcon))
            {
                sqlCmd.Append(string.Format(" and saq.LocalSuppId = '{0}'", subcon));
            }

            if (!MyUtility.Check.Empty(sciDate1))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery >= '{0}'", Convert.ToDateTime(sciDate1).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sciDate2))
            {
                sqlCmd.Append(string.Format(" and o.SciDelivery <= '{0}'", Convert.ToDateTime(sciDate2).ToString("d")));
            }

            sqlCmd.Append(" order by s.ID,s.BrandID,s.SeasonID,sa.ArtworkTypeID");

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

            if (printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.ShowWaitMessage("Starting EXCEL...");
            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Planning_R03_LocalQuotationList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return false;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            //填內容值
            int intRowsStart = 2;
            object[,] objArray = new object[1, 20];
            foreach (DataRow dr in printData.Rows)
            {
                objArray[0, 0] = dr["ID"];
                objArray[0, 1] = dr["BrandID"];
                objArray[0, 2] = dr["SeasonID"];
                objArray[0, 3] = dr["ArtworkTypeID"];
                objArray[0, 4] = dr["Article"];
                objArray[0, 5] = dr["ArtworkID"];
                objArray[0, 6] = dr["ArtworkName"];
                objArray[0, 7] = dr["PatternCode"];
                objArray[0, 8] = dr["PatternDesc"];
                objArray[0, 9] = dr["TMS"];
                objArray[0, 10] = dr["Qty"];
                objArray[0, 11] = dr["ArtworkUnit"];
                objArray[0, 12] = dr["Cost"];
                objArray[0, 13] = dr["LocalSupp"];
                objArray[0, 14] = dr["CurrencyId"];
                objArray[0, 15] = dr["Price"];
                objArray[0, 16] = dr["Oven"];
                objArray[0, 17] = dr["Wash"];
                objArray[0, 18] = dr["Mockup"];
                objArray[0, 19] = dr["PriceApv"];

                worksheet.Range[String.Format("A{0}:T{0}", intRowsStart)].Value2 = objArray;
                intRowsStart++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();
            this.HideWaitMessage();
            excel.Visible = true;
            return true;
        }
    }
}
