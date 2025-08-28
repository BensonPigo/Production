using Ict;
using Sci.Data;
using System;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R68 : Win.Tems.PrintForm
    {
        private string sqlcmd;
        private DataTable[] dts;

        /// <inheritdoc/>
        public R68(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (!this.dateEstCuttingDate.Value1.HasValue && MyUtility.Check.Empty(this.txtCutPlanID.Text))
            {
                MyUtility.Msg.WarningBox("< Est. Cutting Date > and < Cutplan ID > cannot all be empty.");
                return false;
            }
            #region where
            string where = string.Empty;
            if (!this.txtMdivision.Text.Empty())
            {
                where += $"\r\nAND o.MDivisionID = '{this.txtMdivision.Text}'";
            }

            if (!this.txtmultifactory1.Text.Empty())
            {
                where += $"\r\nAND o.FactoryID IN ('{string.Join("','", this.txtmultifactory1.Text.Split(',').ToList())}')";
            }

            if (this.dateEstCuttingDate.Value1.HasValue)
            {
                where += $"\r\nAND EstCutdate.EstCutdate BETWEEN '{(DateTime)this.dateEstCuttingDate.Value1:yyyy/MM/dd}' AND '{(DateTime)this.dateEstCuttingDate.Value2:yyyy/MM/dd}'";
            }

            if (!this.txtCutPlanID.Text.Empty())
            {
                where += $"\r\nAND cp.ID = '{this.txtCutPlanID.Text}'";
            }

            if (!this.txtstyle1.Text.Empty())
            {
                where += $"\r\nAND o.StyleID = '{this.txtstyle1.Text}'";
            }

            if (!this.txtSP1.Text.Empty())
            {
                where += $"\r\nAND cp.POID >= '{this.txtSP1.Text}'";
            }

            if (!this.txtSP2.Text.Empty())
            {
                where += $"\r\nAND cp.POID <= '{this.txtSP2.Text}'";
            }

            if (!this.txtCutCell1.Text.Empty())
            {
                where += $"\r\nAND cp.CutCellID >= '{this.txtCutCell1.Text}'";
            }

            if (!this.txtCutCell2.Text.Empty())
            {
                where += $"\r\nAND cp.CutCellID <= '{this.txtCutCell2.Text}'";
            }
            #endregion

            // Summary
            this.sqlcmd = $@"
USE Production
SELECT
    cp.ID  --裁剪計畫單號
    ,o.FactoryID
    ,o.StyleID
    ,cp.POID
    ,cp.CutCellID
    ,EstCutdate.EstCutdate
    ,EditDate.EditDate
    ,psd.Refno
    ,[Color] = psdsC.SpecValue
    ,rfrt.FabricRelaxationID
    ,frlx.NeedUnroll
    ,frlx.Relaxtime
    ,[Request Cons] = SUM(cpdc.Cons)
    ,FinalETA.ActETA
    ,psd.SCIRefno
    ,cpi.RequestorRemark
INTO #CutList
FROM Cutplan cp
INNER JOIN Cutplan_Detail_Cons cpdc
    ON cp.ID = cpdc.ID
INNER JOIN PO_Supp_Detail psd
    ON cpdc.POID = psd.ID
        AND cpdc.SEQ1 = psd.SEQ1
        AND cpdc.SEQ2 = psd.SEQ2
INNER JOIN PO_Supp_Detail_Spec psdsC
    ON psd.ID = psdsC.ID
        AND psd.SEQ1 = psdsC.Seq1
        AND psd.SEQ2 = psdsC.Seq2
        AND psdsC.SpecColumnID = 'Color'
LEFT JOIN SciMES_RefnoRelaxtime rfrt
    ON psd.Refno = rfrt.Refno
LEFT JOIN SciMES_FabricRelaxation frlx
    ON rfrt.FabricRelaxationID = frlx.ID
LEFT JOIN Orders o
    ON cp.POID = o.ID
LEFT JOIN CutPlan_IssueCutDate cpi WITH(NOLOCK) ON cpi.ID = cp.id AND cpi.Refno = psd.Refno AND cpi.Colorid = psdsC.SpecValue
OUTER APPLY(SELECT EstCutdate = IIF(cpi.EstCutDate IS NOT NULL, cpi.EstCutDate, cp.EstCutdate))EstCutdate
OUTER APPLY(SELECT EditDate = IIF(cpi.EditDate IS NOT NULL, cpi.EditDate, cp.EditDate))EditDate
OUTER APPLY (
    SELECT
        ActETA = MAX(p3.FinalETA)
    FROM PO_Supp_Detail p3 WITH (NOLOCK)
    INNER JOIN PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK)
        ON psdsC2.ID = p3.id
        AND psdsC2.seq1 = p3.seq1
        AND psdsC2.seq2 = p3.seq2
        AND psdsC2.SpecColumnID = 'Color'
    WHERE p3.id = psd.ID
    AND p3.SCIRefno = psd.SCIRefno
    AND psdsC2.SpecValue = psdsC.SpecValue
    AND p3.Junk = 0
    AND p3.Seq1 NOT LIKE 'A%') FinalETA
WHERE cp.Status = 'Confirmed'
--黃底區為篩選條件部份，cp.EstCutdate及cp.ID為擇一必輸，其他的USER有填才加入
{where}
GROUP BY cp.ID
        ,o.FactoryID
        ,cp.POID
        ,cp.CutCellID
        ,EstCutdate.EstCutdate
        ,EditDate.EditDate
        ,psd.Refno
        ,psdsC.SpecValue
        ,rfrt.FabricRelaxationID
        ,frlx.NeedUnroll
        ,frlx.Relaxtime
        ,o.StyleID
        ,FinalETA.ActETA
        ,psd.SCIRefno
        ,cpi.RequestorRemark
/*
	發料準備清單
*/
SELECT
    [issueid] = isu.id
   ,cl.ID
   ,cl.Refno
   ,cl.Color
   ,cl.NeedUnroll
   ,cl.Relaxtime
   ,isud.POID
   ,isud.Seq1
   ,isud.Seq2
   ,isud.Roll
   ,isud.Dyelot
   ,isud.Qty
   ,isud.MINDReleaseDate
   ,fur.UnrollStartTime
   ,fur.UnrollEndTime
   ,[UnrollDone] = IIF(cl.NeedUnroll = 1 AND UnrollStatus != '', 1, 0)
   ,fur.RelaxationStartTime
   ,fur.RelaxationEndTime
   ,[RelaxationDone] = IIF(cl.Relaxtime > 0 AND fur.RelaxationStartTime IS NOT NULL, 1, 0)
   ,mmd.DispatchTime
   ,mmd.FactoryReceivedTime
   ,f.Tone
   ,LockDate = IIF(f.Lock = 0, f.LockDate, NULL)
   ,[Location] = dbo.Getlocation(f.ukey)
   ,cl.SCIRefno
INTO #issueDtl
FROM #CutList cl
INNER JOIN Issue isu
    ON cl.ID = isu.CutplanID
INNER JOIN Issue_Detail isud
    ON isu.Id = isud.Id
INNER JOIN PO_Supp_Detail psd
    ON isud.POID = psd.ID
        AND isud.Seq1 = psd.SEQ1
        AND isud.Seq2 = psd.SEQ2
        AND cl.Refno = psd.Refno
INNER JOIN PO_Supp_Detail_Spec psdsC
    ON psd.ID = psdsC.ID
        AND psd.SEQ1 = psdsC.Seq1
        AND psd.SEQ2 = psdsC.Seq2
        AND psdsC.SpecColumnID = 'Color'
        AND cl.Color = psdsC.SpecValue
INNER JOIN WHBarcodeTransaction wbt
    ON isud.Id = wbt.TransactionID
        AND isud.ukey = wbt.TransactionUkey
        AND wbt.Action = 'Confirm'
LEFT JOIN Fabric_UnrollandRelax fur
    ON wbt.To_NewBarcode = fur.Barcode
LEFT JOIN M360MINDDispatch mmd
    ON isud.M360MINDDispatchUkey = mmd.Ukey
LEFT JOIN FtyInventory f
    ON f.POID = isud.POID
        AND f.Seq1 = isud.Seq1
        AND f.Seq2 = isud.Seq2
        AND f.Roll = isud.Roll
        AND f.Dyelot = isud.Dyelot
        AND f.StockType = isud.StockType
WHERE isu.Status = 'Confirmed'
/*
	計算各狀態已完成數
	此為彙整總表
*/
SELECT
    [Factory] = cl.FactoryID
   ,[CutCell] = cl.CutCellID
   ,cl.EditDate
   ,cl.ID
   ,cl.POID
   ,cl.StyleID
   ,cl.EstCutdate
   ,cl.Refno
   ,cl.Color
   ,cl.ActETA
   ,cl.FabricRelaxationID
   ,[NeedUnroll] = IIF(cl.NeedUnroll = 1, 'Y', '')
   ,cl.Relaxtime
   ,[Status] =
    CASE
        WHEN Received.Cons >= FLOOR(cl.[Request Cons]) THEN 'Received'
        WHEN Dispatched.Cons >= FLOOR(cl.[Request Cons]) THEN 'Dispatched'
        WHEN Relaxation.Cons >= FLOOR(cl.[Request Cons]) THEN 'Relaxation'
        WHEN Unroll.Cons >= FLOOR(cl.[Request Cons]) THEN 'Unroll'
        ELSE 'Preparing'
    END
   ,[RequestCons] = cl.[Request Cons]
   ,[PreparingCons] = ISNULL(Preparing.Cons, 0)
   ,[BalanceQty] = cl.[Request Cons] - ISNULL(Preparing.Cons, 0)
   ,[UnrollCons] = ISNULL(Unroll.Cons, 0)
   ,[RelaxationCons] = ISNULL(Relaxation.Cons, 0)
   ,[DispatchedCons] = ISNULL(Dispatched.Cons, 0)
   ,[ReceivedCons] = ISNULL(Received.Cons, 0)
   ,[PickTime] = FinPickTime.FinDate
   ,[UnrollTime] = FinUnrollTime.FinDate
   ,[RelaxTime] = FinRelaxTime.FinDate
   ,[DispatchTime] = FinDispatchTime.FinDate
   ,[FactoryReceiveTime] = FinFtyRcvTime.FinDate
   ,cl.RequestorRemark
   ,[WHRemark] = IssueSummary.WHRemark
FROM #CutList cl
OUTER APPLY (SELECT
        Cons = SUM(Qty)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color) Preparing
OUTER APPLY (SELECT
        Cons = SUM(Qty)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color
    AND idt.UnrollDone = 1) Unroll
OUTER APPLY (SELECT
        Cons = SUM(Qty)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color
    AND idt.RelaxationDone = 1) Relaxation
OUTER APPLY (SELECT
        Cons = SUM(Qty)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color
    AND idt.DispatchTime IS NOT NULL) Dispatched
OUTER APPLY (SELECT
        Cons = SUM(Qty)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color
    AND idt.FactoryReceivedTime IS NOT NULL) Received
OUTER APPLY (SELECT
        [FinDate] = MAX(MINDReleaseDate)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color) FinPickTime
OUTER APPLY (SELECT
        [FinDate] = MAX(UnrollStartTime)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color) FinUnrollTime
OUTER APPLY (SELECT
        [FinDate] = MAX(RelaxationEndTime)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color) FinRelaxTime
OUTER APPLY (SELECT
        [FinDate] = MAX(DispatchTime)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color) FinDispatchTime
OUTER APPLY (SELECT
        [FinDate] = MAX(FactoryReceivedTime)
    FROM #issueDtl idt
    WHERE cl.ID = idt.ID
    AND cl.Refno = idt.Refno
    AND cl.Color = idt.Color) FinFtyRcvTime
OUTER APPLY (
    SELECT WHRemark = STUFF((
        SELECT DISTINCT CONCAT(',', CHAR(10), '[', iss.RemarkEditName, ' ', FORMAT(iss.RemarkEditDate, 'yyyy/MM/dd HH:mm:ss'), '] ', iss.Remark)
        FROM issue i
        INNER JOIN Issue_Summary iss ON iss.Id = i.ID
        WHERE cl.ID = i.CutplanID
        AND cl.POID = iss.POID
        AND cl.SCIRefno = iss.SCIRefno
        AND cl.Color = iss.ColorID
        AND iss.Remark <> ''
        FOR XML PATH('')), 1, 2, '')
) IssueSummary　 --串Issue Summary取Remark
ORDER BY cl.ID, cl.Refno, cl.Color
";

            // Detail
            this.sqlcmd += @"
SELECT
    cl.FactoryID
   ,cl.CutCellID
   ,cl.EditDate
   ,cl.ID
   ,cl.POID
   ,cl.StyleID
   ,cl.EstCutdate
   ,isud.Seq1
   ,isud.Seq2
   ,cl.Refno
   ,cl.Color
   ,cl.ActETA
   ,cl.FabricRelaxationID
   ,[NeedUnroll] = IIF(cl.NeedUnroll = 1, 'Y', '')
   ,cl.Relaxtime
   ,isud.Roll
   ,isud.Dyelot
   ,f.Tone
   ,[Location] = dbo.Getlocation(f.ukey)
   ,[Requset Cons] = cpdc.Cons
   ,isud.Qty
   ,isud.MINDReleaseDate
   --pick time
   ,fur.UnrollStartTime
   ,[Releaser] = Pass1.ID + ' - ' + Pass1.Name
   ,[UnrollMachine] = MIOT.MachineID
   --,fur.UnrollEndTime
   --,[UnrollDone] = IIF(cl.NeedUnroll = 1 AND UnrollStatus != '', 1, 0)
   ,fur.RelaxationStartTime
   ,fur.RelaxationEndTime
   --,[RelaxationDone] = IIF(cl.Relaxtime > 0 AND fur.RelaxationStartTime IS NOT NULL, 1, 0)
   ,mmd.DispatchTime
   ,mmd.FactoryReceivedTime
FROM #CutList cl
INNER JOIN Issue isu
    ON cl.ID = isu.CutplanID
INNER JOIN Issue_Detail isud
    ON isu.Id = isud.Id
INNER JOIN PO_Supp_Detail psd
    ON isud.POID = psd.ID
        AND isud.Seq1 = psd.SEQ1
        AND isud.Seq2 = psd.SEQ2
        AND cl.Refno = psd.Refno
INNER JOIN PO_Supp_Detail_Spec psdsC
    ON psd.ID = psdsC.ID
        AND psd.SEQ1 = psdsC.Seq1
        AND psd.SEQ2 = psdsC.Seq2
        AND psdsC.SpecColumnID = 'Color'
        AND cl.Color = psdsC.SpecValue
INNER JOIN WHBarcodeTransaction wbt
    ON isud.Id = wbt.TransactionID
        AND isud.ukey = wbt.TransactionUkey
        AND wbt.Action = 'Confirm'
LEFT JOIN Fabric_UnrollandRelax fur
    ON wbt.To_NewBarcode = fur.Barcode
left join [ExtendServer].ManufacturingExecution.dbo.MachineIoT MIOT with (nolock) on MIOT.Ukey = fur.MachineIoTUkey and MIOT.MachineIoTType= 'unroll'
LEFT JOIN M360MINDDispatch mmd
    ON isud.M360MINDDispatchUkey = mmd.Ukey
LEFT JOIN FtyInventory f
    ON f.POID = isud.POID
        AND f.Seq1 = isud.Seq1
        AND f.Seq2 = isud.Seq2
        AND f.Roll = isud.Roll
        AND f.Dyelot = isud.Dyelot
        AND f.StockType = isud.StockType
inner join Cutplan_Detail_Cons cpdc on cl.ID = cpdc.ID and isud.Seq1 = cpdc.SEQ1 and isud.Seq2 = cpdc.SEQ2
left join Pass1 with(nolock) on Pass1.ID = isud.MINDReleaser
WHERE isu.Status = 'Confirmed'
ORDER BY cl.ID, cl.Refno, cl.Color

DROP TABLE #CutList, #issueDtl
";
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd, out this.dts);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.dts[1].Rows.Count);
            if (this.dts[0].Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
                return false;
            }

            this.ShowWaitMessage("Excel Processing...");
            string fileName = "Warehouse_R68_FabricIssuanceStatusReport.xltx";
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\" + fileName); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.dts[0], null, fileName, headerRow: 1, showExcel: false, showSaveMsg: false, excelApp: excelApp, wSheet: excelApp.Sheets[1]);
            if (this.dts[1].Rows.Count > 0)
            {
                MyUtility.Excel.CopyToXls(this.dts[1], null, fileName, headerRow: 1, showExcel: false, showSaveMsg: false, excelApp: excelApp, wSheet: excelApp.Sheets[2]);
            }

            excelApp.Columns.AutoFit();
            excelApp.Rows.AutoFit();

            string excelfile = Class.MicrosoftFile.GetName("Warehouse_R68_FabricIssuanceStatusReport");
            excelApp.ActiveWorkbook.SaveAs(excelfile);
            excelApp.Visible = true;
            Marshal.ReleaseComObject(excelApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
