using System;
using System.Data;
using Ict;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// B03_PrintReviseList
    /// </summary>
    public partial class B03_PrintReviseList : Sci.Win.Tems.PrintForm
    {
        private string reviseDate1;
        private string reviseDate2;
        private DataTable printData;

        /// <summary>
        /// B03_PrintReviseList
        /// </summary>
        public B03_PrintReviseList()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.reviseDate1 = MyUtility.Check.Empty(this.dateReviseDate.Value1) ? string.Empty : Convert.ToDateTime(this.dateReviseDate.Value1).ToString("d");
            this.reviseDate2 = MyUtility.Check.Empty(this.dateReviseDate.Value2) ? string.Empty : Convert.ToDateTime(this.dateReviseDate.Value2).ToString("d");

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            string sqlCmd = string.Format(
                @"with tmpCode
as(
select a.ID,a.EditDate from (
select ID, max(EditDate) as EditDate
from ShipExpense_CanVass WITH (NOLOCK) 
where Status = 'Confirmed'
group by ID) a
where 1=1{0}{1}),
LastRecord 
as(
select s.ID,s.EditDate, 
(case s.ChooseSupp when 1 then s.LocalSuppID1 when 2 then s.LocalSuppID2 when 3 then s.LocalSuppID3 else s.LocalSuppID4 end) as LastLocalSuppID,
(case s.ChooseSupp when 1 then s.CurrencyID1 when 2 then s.CurrencyID2 when 3 then s.CurrencyID3 else s.CurrencyID4 end) as LastCurrencyID,
(case s.ChooseSupp when 1 then s.Price1 when 2 then s.Price2 when 3 then s.Price3 else s.Price4 end) as LastPrice
from ShipExpense_CanVass s WITH (NOLOCK) , tmpCode t where s.ID = t.ID and s.EditDate = t.EditDate),
Last2Record
as(
select s.ID, 
(case s.ChooseSupp when 1 then s.LocalSuppID1 when 2 then s.LocalSuppID2 when 3 then s.LocalSuppID3 else s.LocalSuppID4 end) as LocalSuppID,
(case s.ChooseSupp when 1 then s.CurrencyID1 when 2 then s.CurrencyID2 when 3 then s.CurrencyID3 else s.CurrencyID4 end) as CurrencyID,
(case s.ChooseSupp when 1 then s.Price1 when 2 then s.Price2 when 3 then s.Price3 else s.Price4 end) as Price
from ShipExpense_CanVass s WITH (NOLOCK) , (select sc.ID,max(sc.EditDate) as EditDate from ShipExpense_CanVass sc WITH (NOLOCK) , tmpCode t where sc.Status = 'Confirmed' and sc.ID = t.ID and sc.EditDate < t.EditDate group by sc.ID) a
where s.ID = a.ID and s.EditDate = a.EditDate)

select l.*,l2.LocalSuppID,l2.CurrencyID,l2.Price,s.Description,
(select StdRate from Currency WITH (NOLOCK) where ID = l.LastCurrencyID)*l.LastPrice as StdRateLPrice,
(select StdRate from Currency WITH (NOLOCK) where ID = l2.CurrencyID)*l2.Price as StdRateL2Price
from LastRecord l
left join Last2Record l2 on l.ID = l2.ID
left join ShipExpense s WITH (NOLOCK) on s.ID = l.ID
WHERE s.Junk = 0
and l2.LocalSuppID is not null
order by l.EditDate,l.ID",
                MyUtility.Check.Empty(this.reviseDate1) ? string.Empty : " and a.EditDate >= '" + this.reviseDate1 + "'",
                MyUtility.Check.Empty(this.reviseDate2) ? string.Empty : " and a.EditDate <= '" + this.reviseDate2 + "'");

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out this.printData);
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

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Shipping_B03_PrintReviseList.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            int rownum = 0, counter = 0;
            object[,] objArray = new object[1, 7];
            foreach (DataRow dr in this.printData.Rows)
            {
                rownum = intRowsStart + counter;
                objArray[0, 0] = Convert.ToDateTime(dr["EditDate"]).ToString("d");
                objArray[0, 1] = dr["ID"];
                objArray[0, 2] = dr["Description"];
                objArray[0, 3] = MyUtility.Convert.GetString(dr["LocalSuppID"]) + " / " + MyUtility.Convert.GetString(dr["LastLocalSuppID"]);
                objArray[0, 4] = MyUtility.Convert.GetString(dr["CurrencyID"]) + " / " + MyUtility.Convert.GetString(dr["LastCurrencyID"]);
                objArray[0, 5] = MyUtility.Convert.GetString(dr["Price"]) + " / " + MyUtility.Convert.GetString(dr["LastPrice"]);
                objArray[0, 6] = MyUtility.Check.Empty(dr["Price"]) ? 0 : (MyUtility.Convert.GetDecimal(dr["LastPrice"]) - MyUtility.Convert.GetDecimal(dr["Price"])) / MyUtility.Convert.GetDecimal(dr["Price"]);

                worksheet.Range[string.Format("A{0}:G{0}", rownum)].Value2 = objArray;
                counter++;
            }

            excel.Cells.EntireColumn.AutoFit();
            excel.Cells.EntireRow.AutoFit();

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Shipping_B03_PrintReviseList");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
