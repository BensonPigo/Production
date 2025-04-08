using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using System.Runtime.InteropServices;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class R02 : Win.Tems.PrintForm
    {
        private string Factory;
        private string M;
        private string strIssueDate1;
        private string strIssueDate2;
        private string strApvDate1;
        private string strApvDate2;
        private string SubCon;
        private string ContractNumb;
        private string SP;
        private string Status;
        private DataTable printData;

        /// <inheritdoc/>
        public R02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            MyUtility.Tool.SetupCombox(this.comboStatus, 2, 1, ",,New,New,Confirmed,Confirmed,Closed,Closed");
            this.comboStatus.SelectedIndex = 0;
            this.txtMdivisionM.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Factory = this.txtfactory.Text;
            this.M = this.txtMdivisionM.Text;
            this.strIssueDate1 = this.dateIssueDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateIssueDate.Value1).ToString("yyyy/MM/dd");
            this.strIssueDate2 = this.dateIssueDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateIssueDate.Value2).ToString("yyyy/MM/dd");
            this.strApvDate1 = this.dateRangeApvDate.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeApvDate.Value1).ToString("yyyy/MM/dd");
            this.strApvDate2 = this.dateRangeApvDate.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeApvDate.Value2).ToString("yyyy/MM/dd");
            this.SubCon = this.txtSubCon.Text;
            this.ContractNumb = this.txtContract.Text;
            this.SP = this.txtSPNO.Text;
            this.Status = this.comboStatus.SelectedValue.ToString();

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            List<string> listSQLFilter = new List<string>();
            #region Filter

            if (!MyUtility.Check.Empty(this.Factory))
            {
                listSQLFilter.Add($"and s.Factoryid = '{this.Factory}'");
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                listSQLFilter.Add($"and s.MDivisionID = '{this.M}'");
            }

            if (!MyUtility.Check.Empty(this.SubCon))
            {
                listSQLFilter.Add($"and s.SubConOutFty = '{this.SubCon}'");
            }

            if (!MyUtility.Check.Empty(this.ContractNumb))
            {
                listSQLFilter.Add($"and s.ContractNumber = '{this.ContractNumb}'");
            }

            if (!MyUtility.Check.Empty(this.SP))
            {
                listSQLFilter.Add($"and sd.Orderid = '{this.SP}'");
            }

            if (!MyUtility.Check.Empty(this.Status))
            {
                listSQLFilter.Add($"and s.Status = '{this.Status}'");
            }

            if (!MyUtility.Check.Empty(this.strIssueDate1))
            {
                listSQLFilter.Add($"and s.IssueDate >= '{this.strIssueDate1}'");
            }

            if (!MyUtility.Check.Empty(this.strIssueDate2))
            {
                listSQLFilter.Add($"and s.IssueDate <= '{this.strIssueDate2}'");
            }

            if (!MyUtility.Check.Empty(this.strApvDate1))
            {
                listSQLFilter.Add($"and s.ApvDate >= '{this.strApvDate1}'");
            }

            if (!MyUtility.Check.Empty(this.strApvDate2))
            {
                listSQLFilter.Add($"and s.ApvDate <= '{this.strApvDate2}'");
            }
            #endregion

            string sqlcmd = $@"
SELECT s.SubConOutFty, s.ContractNumber, sd.OrderId, sd.ComboType, sd.Article, o.StyleUkey
INTO #Main
FROM dbo.SubconOutContract_Detail sd WITH (NOLOCK)
LEFT JOIN SubconOutContract s WITH (NOLOCK) ON sd.SubConOutFty = s.SubConOutFty AND sd.ContractNumber = s.ContractNumber
LEFT JOIN Orders o WITH (NOLOCK) ON sd.Orderid = o.ID
where 1=1
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}

-- 主查詢（有資料時執行）
SELECT 
	[Ukey] = i.StyleUkey,
    i.Location, 
    i.ArtworkTypeID,
    type = IIF(i.Location = 'T','Top', IIF(i.Location = 'B','Bottom', IIF(i.Location = 'I','Inner', IIF(i.Location = 'O','Outer','')))),
    tms = CEILING(SUM(i.ProSMV) * 60),
	[Rate] = CAST(1 AS decimal(7,6))
into #Tmp
FROM IETMS_Summary i
WHERE i.IETMSUkey IN (
    SELECT DISTINCT ietms.Ukey
    FROM Style s WITH (NOLOCK)
    INNER JOIN #Main m WITH (NOLOCK) ON m.StyleUkey = s.Ukey
    INNER JOIN IETMS ietms WITH (NOLOCK) ON s.IETMSID = ietms.ID AND s.IETMSVersion = ietms.Version
)
AND EXISTS (
    SELECT 1
    FROM IETMS_Summary i2
    WHERE i2.IETMSUkey IN (
        SELECT DISTINCT ietms.Ukey
        FROM Style s WITH (NOLOCK)
        INNER JOIN #Main m WITH (NOLOCK) ON m.StyleUkey = s.Ukey
        INNER JOIN IETMS ietms WITH (NOLOCK) ON s.IETMSID = ietms.ID AND s.IETMSVersion = ietms.Version
    )
)
GROUP BY i.Location, i.ArtworkTypeID,i.StyleUkey

UNION ALL

-- 備案查詢（僅在主查詢沒資料時執行）
SELECT 
	[Ukey] = s.Ukey,
    id.Location,
    m.ArtworkTypeID,
    IIF(id.Location = 'T','Top', IIF(id.Location = 'B','Bottom', IIF(id.Location = 'I','Inner', IIF(id.Location = 'O','Outer','')))) AS Type,
    ROUND(SUM(ISNULL(id.SMV,0) * id.Frequency * (ISNULL(id.MtlFactorRate,0)/100 + 1) * 60), 0) AS tms,
	[Rate] = CAST(1 AS decimal(7,6))
FROM Style s WITH (NOLOCK)
INNER JOIN IETMS i WITH (NOLOCK) ON s.IETMSID = i.ID AND s.IETMSVersion = i.Version
INNER JOIN IETMS_Detail id WITH (NOLOCK) ON i.Ukey = id.IETMSUkey
INNER JOIN Operation o WITH (NOLOCK) ON id.OperationID = o.ID
INNER JOIN MachineType m WITH (NOLOCK) ON o.MachineTypeID = m.ID
WHERE s.Ukey IN (
    SELECT DISTINCT StyleUkey FROM #Main
)
AND NOT EXISTS (
    SELECT 1
    FROM IETMS_Summary i2
    WHERE i2.IETMSUkey IN (
        SELECT DISTINCT ietms.Ukey
        FROM Style s2 WITH (NOLOCK)
        INNER JOIN #Main m2 WITH (NOLOCK) ON m2.StyleUkey = s2.Ukey
        INNER JOIN IETMS ietms WITH (NOLOCK) ON s2.IETMSID = ietms.ID AND s2.IETMSVersion = ietms.Version
    )
)

GROUP BY id.Location, m.ArtworkTypeID,s.Ukey

SELECT 
    Ukey,
    Location,
    ArtworkTypeID,
    [Rate] = TMS / NULLIF(SUM(TMS) OVER (PARTITION BY Ukey, ArtworkTypeID), 0)
INTO #RateTmp
FROM #Tmp
ORDER BY Ukey

-- 優先從 Order_Location 更新資料
UPDATE rt
SET rt.Location = ol.Location,
    rt.Rate = ol.Rate/100
FROM #RateTmp rt
INNER JOIN #Main m WITH (NOLOCK) ON rt.Ukey = m.StyleUkey
LEFT JOIN Order_Location ol WITH (NOLOCK) 
    ON m.OrderId = ol.OrderID AND m.ComboType = ol.Location
WHERE (rt.Location IS NULL or rt.Location = '') AND ol.Location IS NOT NULL

-- 補用 Style_Location 更新資料（僅當 Order_Location 無資料時）
UPDATE rt
SET rt.Location = sl.Location,
    rt.Rate = sl.Rate/100
FROM #RateTmp rt
INNER JOIN #Main m WITH (NOLOCK) ON rt.Ukey = m.StyleUkey
LEFT JOIN Order_Location ol WITH (NOLOCK) 
    ON m.OrderId = ol.OrderID AND m.ComboType = ol.Location
LEFT JOIN Style_Location sl WITH (NOLOCK) 
    ON m.StyleUkey = sl.StyleUkey
WHERE (rt.Location IS NULL or rt.Location = '') AND ol.Location IS NULL AND sl.Location IS NOT NULL

select
[Factory] = s.Factoryid
,[OrderFactory]  = o.FactoryID
,[Subcon Name]  = sd.SubConOutFty
,[Contract No] = sd.ContractNumber
,[Style] = o.StyleID
,[SP] = o.ID
,[Buyer Delivery] = o.BuyerDelivery
,[OrderQty] = Order_Qty.Qty
,[SubconOutQty] = sd.Outputqty
,[Sewing CPU (Contract CFM date)] = sCPU.[Sewing CPU (Contract CFM date)]
,[Sewing_CPU] = ROUND(tms.SewingCPU * r.rate,4,4)
,[CPU] = totalTMS.CPU
,LocalCurrencyID = LocalCurrencyID
,LocalUnitPrice = isnull(LocalUnitPrice,0)
,Vat = isnull(Vat,0)
,UPIncludeVAT = isnull(LocalUnitPrice,0)+isnull(Vat,0)
,KpiRate = isnull(KpiRate,0)
,[SubConPrice/CPU] = ROUND(sd.UnitPrice,4,4)
,[SubConPrice/CPUByComboType] = ROUND(sd.UnitPrice * r.rate, 4, 4)
,[Sub (TMS)] = SubTMS.TotalPrice
,[EMB] = ROUND(tms.EMBPrice,4,4)
,[Print] = ROUND(tms.PrintingPrice,4,4)
,[OtherAmt] = ROUND(tms.OtherAmt,4,4)
,[Price/CPU] = ROUND(iif((tms.CuttingCPU * r.rate +tms.HeatTransfer+tms.InspectionCPU * r.rate +tms.OtherCPU * r.rate )=0,0, (sd.UnitPrice-tms.EMBPrice-tms.PrintingPrice-tms.OtherAmt) / (tms.CuttingCPU * r.rate +tms.HeatTransfer+tms.InspectionCPU * r.rate +tms.OtherCPU * r.rate )),4,4)
,[TTL Sewing CPU] = ROUND(sd.Outputqty * ROUND(tms.SewingCPU * r.rate,4,4),4,4)
,[Total CPU] = ROUND(sd.Outputqty * totalTMS.CPU,4,4)
,[Contract Amt] = ROUND(sd.Outputqty * (isnull(LocalUnitPrice,0)+isnull(Vat,0)),4,4)
,[ExchangeRate]=''
,[Contract Amt_usd]=ROUND(sd.Outputqty * ROUND(sd.UnitPrice * r.rate, 4, 4),4,4)
,[SR]=''
,[Remark]=''
from dbo.SubconOutContract_Detail sd with (nolock)
left join Orders o with (nolock) on sd.Orderid = o.ID
left join SubconOutContract s with (nolock) on sd.SubConOutFty=s.SubConOutFty  and sd.ContractNumber=s.ContractNumber
outer apply(
	select isnull(sum(Qty),0) Qty
	from Order_Qty with (nolock) 
	where ID = sd.OrderID and Article = sd.Article 
)Order_Qty
outer apply (
    select  
    [SewingCPU] = sum(iif(Order_TmsCost.ArtworkTypeID = 'SEWING',TMS/1400,0)),
    [CuttingCPU]= sum(iif(Order_TmsCost.ArtworkTypeID = 'CUTTING',TMS/1400,0)),
    [InspectionCPU]= sum(iif(Order_TmsCost.ArtworkTypeID = 'INSPECTION',TMS/1400,0)),
    [OtherCPU]= sum(iif(Order_TmsCost.ArtworkTypeID in ('INSPECTION','CUTTING','SEWING'),0,TMS/1400)),
	[OtherAmt]= sum(iif(LEFT(Seq,1) = '3' AND Seq NOT IN ('3010', '3020'), Price*#RateTmp.Rate, 0)),
    [EMBAmt] = sum(iif(Order_TmsCost.ArtworkTypeID = 'EMBROIDERY',Price,0)) * sd.OutputQty,
    [PrintingAmt] = sum(iif(Seq = '3020',Price,0)) * sd.OutputQty,
    [OtherPrice]= sum(iif(Order_TmsCost.ArtworkTypeID in ('PRINTING','EMBROIDERY'),0,Price)),
    [EMBPrice] = sum(iif(Seq = '3010',Price,0)*#RateTmp.Rate),
    [PrintingPrice] = sum(iif(Order_TmsCost.ArtworkTypeID = 'PRINTING',Price,0)*#RateTmp.Rate),
	[HeatTransfer] = sum(iif(Order_TmsCost.ArtworkTypeID = 'HEAT TRANSFER',TMS/1400,0))
    from Order_TmsCost with (nolock)
	left join #RateTmp on #RateTmp.Ukey = o.StyleUkey and #RateTmp.Location = sd.ComboType and #RateTmp.ArtworkTypeID = Order_TmsCost.ArtworkTypeID
    where ID = sd.OrderID
) as tms
outer apply(
	select rate = isnull(dbo.GetOrderLocation_Rate(o.ID,sd.ComboType)
	,(select rate = rate from Style_Location sl with (nolock) where sl.StyleUkey = o.StyleUkey and sl.Location = sd.ComboType))/100)r
outer apply(
	select CPU = sum(ot.TMS) 
	from Order_TmsCost ot with (nolock)
	left join ArtworkType on ot.ArtworkTypeID = ArtworkType.ID
	where ot.ID = sd.OrderID and ArtworkType.IsTtlTMS  = 1
	group by ot.ID
) as totalTMS
outer apply(
	select [Sewing CPU (Contract CFM date)] = iif(s.Status = 'Confirmed',totalTMS.CPU*(ol.Rate/100),0)
	from SubconOutContract_Detail_TMSCost sdt
	left join Order_Location ol on ol.OrderId = sd.OrderId and ol.Location = sd.ComboType
	where sd.SubConOutFty = sdt.SubConOutFty and sd.ContractNumber = sdt.ContractNumber and sd.OrderId = sdt.OrderId 
			and sdt.ArtworkTypeID = 'Sewing'
) as sCPU
outer apply(
	SELECT 
		SUM(ot.Price* #RateTmp.Rate) AS TotalPrice
	FROM 
		Order_TmsCost ot WITH (NOLOCK)
	LEFT JOIN ArtworkType ON ot.ArtworkTypeID = ArtworkType.ID
	left join orders on orders.ID = ot.ID
	left join #RateTmp on #RateTmp.Ukey = orders.StyleUkey and #RateTmp.Location = sd.ComboType and #RateTmp.ArtworkTypeID = ot.ArtworkTypeID
	WHERE 
		ArtworkType.Classify = 'I'
		AND ot.ID = sd.OrderID
		AND ot.Seq LIKE '1%'
		AND 
		(
			NOT (ot.Seq IN (1200, 1210)) OR
			ot.Price = 
			(
				SELECT MAX(ot2.Price)
				FROM Order_TmsCost ot2
				WHERE ot2.ID = ot.ID 
				AND ot2.ArtworkTypeID = ot.ArtworkTypeID
				AND ot2.Seq IN (1200, 1210)
			)
		)
) as SubTMS
where 1=1
{listSQLFilter.JoinToString($"{Environment.NewLine} ")}

DROP TABLE #Main
DROP TABLE #RateTmp
";

            DualResult result = DBProxy.Current.Select(null, sqlcmd.ToString(),  out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Subcon_R02.xltx");
            objApp.Visible = false;
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Subcon_R02.xltx", 3, false, null, objApp);
            Microsoft.Office.Interop.Excel.Worksheet wks = objApp.ActiveWorkbook.Worksheets[1];
            for (int c = 1; c <= this.printData.Rows.Count; c++)
            {
                int tr = c + 3;
                wks.Cells[c + 3, 30] = $"=W{tr}/X{tr}";
            }

            objApp.Cells.EntireColumn.AutoFit();    // 自動欄寬
            objApp.Cells.EntireRow.AutoFit();       // 自動欄高

            // 畫框線
            int rowcnt = this.printData.Rows.Count + 3;
            Microsoft.Office.Interop.Excel.Range rg1;
            rg1 = wks.get_Range("A4", $"AE{this.printData.Rows.Count + 3}");
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeBottom].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeTop].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal].Weight = 2;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlLineStyleNone;
            rg1.Borders[Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical].Weight = 2;

            #region Save Excel
            string excelFile = Class.MicrosoftFile.GetName("Subcon_R02");
            objApp.ActiveWorkbook.SaveAs(excelFile);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objApp);
            excelFile.OpenFile();
            #endregion

            return true;
        }
    }
}
