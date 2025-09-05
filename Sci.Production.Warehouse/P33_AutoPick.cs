using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P33_AutoPick : Win.Subs.Base
    {
        private StringBuilder sbSizecode;
        private string poid;
        private string issueid;
        private string orderid;
        private DataTable BOA_PO;
        private DataTable dtIssueBreakDown;
        private DataTable tmpTable_IssueBreakDown;
        private Dictionary<DataRow, DataTable> dictionaryDatas = new Dictionary<DataRow, DataTable>();
        private List<IssueQtyBreakdown> _IssueQtyBreakdownList = new List<IssueQtyBreakdown>();
        private bool combo;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private bool selectAllMachineType = true;

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public DataRow[] importRows;

        /// <inheritdoc/>
        public P33_AutoPick(string issueid, string poid, string orderid, DataTable dtIssueBreakDown, StringBuilder sbSizecode, bool combo, List<IssueQtyBreakdown> issueQtyBreakdownList)
        {
            this.InitializeComponent();
            this.poid = poid;
            this.issueid = issueid;

            // cutplanid = _cutplanid;
            this.orderid = orderid;
            this.dtIssueBreakDown = dtIssueBreakDown;
            this.sbSizecode = sbSizecode;
            this.combo = combo;
            this._IssueQtyBreakdownList = issueQtyBreakdownList;
            this.Text += string.Format(" ({0})", this.poid);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            if (this.dtIssueBreakDown == null)
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            this.SetDefaultTxtMachineType();

            this.Query(true, this.txtMachineType.Text);

            DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();

            qty.CellMouseDoubleClick += (s, e) =>
            {
                DataTable detail = (DataTable)this.listControlBindingSource1.DataSource;
                DataRow currentRow = detail.Rows[e.RowIndex];

                string sCIRefNo = currentRow["SCIRefNo"].ToString();
                string colorID = currentRow["ColorID"].ToString();
                string sqmIsQuiting = this.chkQuiting.Checked ? "1" : "0";
                List<string> articles = this._IssueQtyBreakdownList.Where(o => o.Qty > 0).Select(o => o.Article).Distinct().ToList();
                string cmd = $@"


----------------Quiting用量計算--------------------------

select P.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,OE.ConsPC
,[Number of Needle for QT] =  ceiling(sqh.Width/sqh.HSize/sqh.NeedleDistance)
INTO #tmpQT
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0 
inner join Order_EachCons_SizeQty OEZ on OE.Ukey=OEZ.Order_EachConsUkey and OQ.SizeCode=OEZ.SizeCode 
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OE.FabricCode and SQH.FabricPanelCode=OE.FabricPanelCode and P.ThreadVersion=SQH.Version
where o.ID = '{this.poid}'

select O.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,SQHD.Seq,SQHD.SCIRefno,SQHD.ColorID,QTt.Val
INTO #tmpQTFinal
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OCC.FabricCode and SQH.FabricPanelCode=OCC.FabricPanelCode and P.ThreadVersion=SQH.Version
Inner join Style_QTThreadColorCombo_History_Detail SQHD on SQH.Ukey=SQHD.Style_QTThreadColorCombo_HistoryUkey and OQ.Article=SQHD.Article
OUTER APPLY(
	SELECT Val=[Number of Needle for QT]* SQHD.Ratio * 0.9144 * qt.ConsPC * s.Qty
	FROM #tmpQT qt
	LEFT JOIN #tmp s ON qt.Article = s.Article AND qt.SizeCode = s.SizeCode
	WHERE qt.ID=o.ID AND qt.Article=oq.Article AND qt.SizeCode=oq.SizeCode
	AND qt.FabricPanelCode=OCC.FabricPanelCode AND qt.FabricCode =OCC.FabricCode
)QTt
where o.ID = '{this.poid}'
AND SQHD.SCIRefno='{sCIRefNo}' AND SQHD.ColorID='{colorID}'
------------------------------------------

SELECT Article, [Qty]=sum (f.OpThreadQty) + ISNULL(Qt.Val,0)
FROM dbo.GetThreadUsedQtyByBOT('{this.poid}',default) f
OUTER APPLY(
    SELECT Val = SUM(Val)
    FROM #tmpQTFinal t
    WHERE t.SCIRefno = f.SCIRefno AND t.ColorID = f.ColorID AND 1={sqmIsQuiting}
)Qt
WHERE SCIRefNo='{sCIRefNo}' 
AND ColorID='{colorID}'
AND Article IN ('{articles.JoinToString("','")}')
GROUP BY Article, Qt.Val

DROP TABLE #tmpQT,#tmpQTFinal,#tmp
";

                DataTable dt;
                DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(this.tmpTable_IssueBreakDown, string.Empty, cmd, out dt, "#tmp");

                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                    return;
                }

                MyUtility.Msg.ShowMsgGrid_LockScreen(dt, caption: $"@Qty by Article");
            };

            #region --設定Grid1的顯示欄位--

            this.gridAutoPick.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridAutoPick.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridAutoPick)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                 .Text("SCIRefno", header: "SCIRefno", width: Widths.AnsiChars(25), iseditingreadonly: true)
                 .Text("RefNo", header: "RefNo", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("ColorID", header: "Color", width: Widths.AnsiChars(7), iseditingreadonly: true)
                 .Text("SuppColor", header: "SuppColor", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .Text("DescDetail", header: "Desc.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Numeric("@Qty", header: "@Qty", width: Widths.AnsiChars(15), decimal_places: 2, iseditingreadonly: true, settings: qty)
                 .Numeric("Use Qty By Stock Unit", header: "Use Qty\r\nBy Stock Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                 .Text("Stock Unit", header: "Stock Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("Use Qty By Use Unit", header: "Use Qty\r\nBy Use Unit", width: Widths.AnsiChars(6), decimal_places: 2, iseditingreadonly: true)
                 .Text("Use Unit", header: "Use Unit", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Text("Stock Unit Desc.", header: "Stock Unit Desc.", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("Output Qty(Garment)", header: "Output Qty\r\n(Garment)", width: Widths.AnsiChars(6), decimal_places: 0, iseditingreadonly: true)
                 .Text("Bulk Balance(Stock Unit)", header: "Bulk Balance\r\n(Stock Unit)", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 ;
            this.gridAutoPick.Columns["Selected"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        private void Query(bool isQuiting, string machineTypeIDs)
        {
            string sqlcmd;
            string sqmIsQuiting = isQuiting ? "1" : "0";
            string sqmMachineTypeIDs = MyUtility.Check.Empty(machineTypeIDs) ? "'NULL'" : "'" + machineTypeIDs + "'";
            decimal sum = 0;
            foreach (DataRow dr in this.dtIssueBreakDown.Rows)
            {
                foreach (DataColumn dc in this.dtIssueBreakDown.Columns)
                {
                    if (ReferenceEquals(sum.GetType(), dr[dc].GetType()))
                    {
                        sum += (decimal)dr[dc];
                    }
                }
            }

            if (sum == 0)
            {
                MyUtility.Msg.WarningBox("IssueBreakdown data no data!", "Warning");
                this.Close();
                return;
            }

            SqlConnection sqlConnection = null;
            DataSet dataSet = new DataSet();
            DataTable result = null;

            this.BOA_PO = null;

            // 整理出#tmp傳進 SQL 使用
            DataTable issueBreakDown_Dt = new DataTable();

            // IssueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "OrderID", DataType = typeof(string) });
            issueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "Article", DataType = typeof(string) });
            issueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "SizeCode", DataType = typeof(string) });
            issueBreakDown_Dt.Columns.Add(new DataColumn() { ColumnName = "Qty", DataType = typeof(int) });

            var groupByData = this._IssueQtyBreakdownList.GroupBy(o => new { o.Article, o.SizeCode }).Select(o => new
            {
                o.Key.Article,
                o.Key.SizeCode,
                Qty = o.Sum(x => x.Qty),
            }).ToList();

            foreach (var model in groupByData)
            {
                if (model.Qty > 0)
                {
                    DataRow newDr = issueBreakDown_Dt.NewRow();

                    // newDr["OrderID"] = model.OrderID;
                    newDr["Article"] = model.Article;
                    newDr["SizeCode"] = model.SizeCode;
                    newDr["Qty"] = model.Qty;

                    issueBreakDown_Dt.Rows.Add(newDr);
                }
            }

            this.tmpTable_IssueBreakDown = issueBreakDown_Dt;

            #region SQL
            sqlcmd = $@"
SELECt Article,[Qty]=SUM(Qty) 
INTO #tmp_sumQty
FROm #tmp
GROUP BY Article


--------------取得哪些要打勾--------------
---- 從Issue breakdown的Article，找到包含在哪些物料裡面
---- 傳入OrderID
Select distinct tcd.SCIRefNo,tcd.Article, tcd.ColorID, tcd.SuppColor
INTO #step1
From dbo.Orders as o
INNER JOIN PO WITH (NOLOCK) ON po.StyleUkey = o.StyleUkey
Inner Join dbo.Style as s On s.Ukey = o.StyleUkey
Inner Join dbo.Style_ThreadColorCombo_History as tc On tc.StyleUkey = s.Ukey AND po.ThreadVersion = tc.Version
Inner Join dbo.Style_ThreadColorCombo_History_Detail as tcd On tcd.Style_ThreadColorCombo_HistoryUkey  = tc.Ukey
WHERE O.ID='{this.poid}' AND tcd.Article IN ( SELECT Article FROM #tmp )

select
	SCIRefNo = iif(IsForOtherBrand = 1,TR.FromSCIRefno, psd.SCIRefno),
	SuppColor = iif(IsForOtherBrand = 1, TR.FromSuppColor, psd.SuppColor),
	psd.ID,psd.SEQ1,psd.SEQ2,ColorID = isnull(psdsC.SpecValue, '')
into #tmpPO_Supp_Detail
from PO_Supp_Detail psd
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	SELECT top 1 TR.FromSCIRefno,TR.FromSuppColor -- 理應是唯一
	FROM PO_Supp PS 
	INNER JOIN Thread_Replace_Detail_Detail TRDD ON PSD.SCIRefNo=TRDD.ToSCIRefno AND PSD.SuppColor=TRDD.ToBrandSuppColor AND PS.SuppID=TRDD.SuppID 
	INNER JOIN Thread_Replace_Detail TRD ON TRDD.Thread_Replace_DetailUkey = TRD.Ukey
	INNER JOIN Thread_Replace TR ON TRD.Thread_ReplaceUkey = TR.Ukey
	WHERE PS.ID = PSD.ID AND PSD.SEQ1=PS.SEQ1	
)TR1
OUTER APPLY(
	SELECT top 1 TR.FromSCIRefno,TR.FromSuppColor -- 理應是唯一
	FROM PO_Supp PS 
	INNER JOIN Thread_Replace_Detail_Detail TRDD ON PSD.SCIRefNo=TRDD.ToSCIRefno AND PSD.SuppColor=TRDD.ToBrandSuppColor
	INNER JOIN Thread_Replace_Detail TRD ON TRDD.Thread_Replace_DetailUkey = TRD.Ukey
	INNER JOIN Thread_Replace TR ON TRD.Thread_ReplaceUkey = TR.Ukey
	WHERE PS.ID = PSD.ID AND PSD.SEQ1=PS.SEQ1	
)TR2
OUTER APPLY(
	SELECT
		FromSCIRefno = iif(TR1.FromSCIRefno is not null, TR1.FromSCIRefno, TR2.FromSCIRefno),
		FromSuppColor = iif(TR1.FromSuppColor is not null, TR1.FromSuppColor, TR2.FromSuppColor)
)TR
WHERE psd.ID='{this.poid}'
AND psd.FabricType ='A'
AND EXISTS(
	SELECT 1
	FROM Fabric f
	INNER JOIN MtlType m on m.id= f.MtlTypeID
	WHERE f.SCIRefno = psd.SCIRefNo
	AND m.IsThread=1 
)
AND isnull(psdsC.SpecValue, '')<> ''

----取得這些物料的項次號，然後取得Order List，如果NULL就直接打勾不用判斷後續
---- 傳入OrderID
SELECT DISTINCT 
	pso.OrderID,a.SCIRefNo, a.ColorID, a.SuppColor,a.SEQ1,a.SEQ2,a.ID
INTO #step2
FROM #tmpPO_Supp_Detail a
LEFT JOIN  PO_Supp_Detail_OrderList pso ON a.ID = pso.ID AND a.SEQ1 = pso.SEQ1 AND a.SEQ2 = pso.SEQ2
WHERE EXISTS(
	SELECT * FROM #step1
	WHERE SCIRefNo = a.SCIRefNo AND SuppColor = a.SuppColor
)

SELECT DISTINCT SCIRefno,ColorID,SuppColor
INTO #SelectList1
FROM #step2 
WHERE OrderID IS NULL 

SELECT DISTINCT a.SCIRefno,a.ColorID,a.SuppColor--,b.* 
INTO #SelectList2
FROM #step2 a
INNER JOIN Order_Article b ON a.OrderID = b.id
WHERE OrderID IS NOT NULL 

----------------Quiting用量計算--------------------------

select P.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,OE.ConsPC
,[Number of Needle for QT] =  ceiling(sqh.Width/sqh.HSize/sqh.NeedleDistance)
INTO #tmpQT
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0 
inner join Order_EachCons_SizeQty OEZ on OE.Ukey=OEZ.Order_EachConsUkey and OQ.SizeCode=OEZ.SizeCode 
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OE.FabricCode and SQH.FabricPanelCode=OE.FabricPanelCode and P.ThreadVersion=SQH.Version
where o.ID = '{this.poid}'

select O.ID,OQ.Article,OQ.SizeCode,OCC.FabricPanelCode,OCC.FabricCode
,SQHD.Seq,SQHD.SCIRefno,SQHD.ColorID,SQHD.SuppColor,QTt.Val
INTO #tmpQTFinal
from Orders O
Inner join Order_Qty OQ on O.ID=OQ.ID
Inner join PO P on O.POID=P.ID
inner join Order_ColorCombo OCC on O.POID=OCC.Id and OQ.Article=OCC.Article and OCC.FabricCode is not null and OCC.FabricCode !=''
Inner join Style_QTThreadColorCombo_History SQH on O.styleUkey=SQH.styleUkey and SQH.FabricCode=OCC.FabricCode and SQH.FabricPanelCode=OCC.FabricPanelCode and P.ThreadVersion=SQH.Version
Inner join Style_QTThreadColorCombo_History_Detail SQHD on SQH.Ukey=SQHD.Style_QTThreadColorCombo_HistoryUkey and OQ.Article=SQHD.Article
OUTER APPLY(
	SELECT Val=[Number of Needle for QT]* SQHD.Ratio * 0.9144 * qt.ConsPC * s.Qty
	FROM #tmpQT qt
	LEFT JOIN #tmp s ON qt.Article = s.Article AND qt.SizeCode = s.SizeCode
	WHERE qt.ID=o.ID AND qt.Article=oq.Article AND qt.SizeCode=oq.SizeCode
	AND qt.FabricPanelCode=OCC.FabricPanelCode AND qt.FabricCode =OCC.FabricCode
)QTt
where o.ID = '{this.poid}'

--------------取得哪些要打勾--------------


SELECT  DISTINCT 
		[Selected] = IIF( (psd.IsForOtherBrand = 0 and 
                            (EXISTS(SELECT 1 FROM #SelectList1 WHERE SCIRefno =psd.SCIRefno AND SuppColor = psd.SuppColor) OR
							 EXISTS(SELECT 1 FROM #SelectList2 WHERE SCIRefno =psd.SCIRefno AND SuppColor = psd.SuppColor)))
                          OR
                          (psd.IsForOtherBrand = 1 and 
                            (EXISTS(SELECT 1 FROM #SelectList1 WHERE SCIRefno =TR.FromSCIRefno AND SuppColor = TR.FromSuppColor) OR
							 EXISTS(SELECT 1 FROM #SelectList2 WHERE SCIRefno =TR.FromSCIRefno AND SuppColor = TR.FromSuppColor)))
						,1 ,0)
		, psd.SCIRefno 
        , psd.Refno
        , ColorID = isnull(psdsC.SpecValue, '')
		, f.DescDetail
		, [@Qty]= ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0)
		, [Use Qty By Stock Unit] = CEILING (ISNULL(ThreadUsedQtyByBOT.Qty,0) * (ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0))/ 100 * ISNULL(UnitRate.RateValue,1) )--並轉換為Stock Unit
		, [Stock Unit]=StockUnit.StockUnit
		, [Use Qty By Use Unit]= (ISNULL(ThreadUsedQtyByBOT.Qty,0) *  (ISNULL(ThreadUsedQtyByBOT.Val,0) + ISNULL(QT.Val,0))  )
		, [Use Unit]='CM'
		, [Stock Unit Desc.]=StockUnit.Description
		, [Output Qty(Garment)] = ISNULL(ThreadUsedQtyByBOT.Qty,0)
		, [Bulk Balance(Stock Unit)] = 0
        , [POID]=psd.ID
		, [AccuIssued] = (
					select isnull(sum([IS].qty),0)
					from dbo.issue I2 WITH (NOLOCK) 
					inner join dbo.Issue_Summary [IS] WITH (NOLOCK) on I2.id = [IS].Id 
					where I2.type = 'E' and I2.Status = 'Confirmed' 
					and [IS].Poid=psd.ID AND [IS].SCIRefno=psd.SCIRefno AND [IS].ColorID=isnull(psdsC.SpecValue, '') and i2.[EditDate]<GETDATE()
				)
INTO #final
FROM PO_Supp_Detail psd
inner join #step2 tpsd on tpsd.ID = psd.id and tpsd.SEQ1 = psd.SEQ1 and tpsd.SEQ2 = psd.SEQ2
INNER JOIN Fabric f ON f.SCIRefno = psd.SCIRefno
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
OUTER APPLY(
	SELECT TOP 1 psd2.StockUnit ,u.Description
	FROM PO_Supp_Detail psd2
	INNER JOIN Unit u ON psd2.StockUnit = u.ID
    left join PO_Supp_Detail_Spec psdsC2 WITH (NOLOCK) on psdsC2.ID = psd2.id and psdsC2.seq1 = psd2.seq1 and psdsC2.seq2 = psd2.seq2 and psdsC2.SpecColumnID = 'Color'
	WHERE psd2.ID =psd.ID	AND psd2.SCIRefno=psd.SCIRefno AND isnull(psdsC2.SpecValue, '')=isnull(psdsC.SpecValue, '')
)StockUnit
OUTER APPLY(
	SELECT RateValue = IIF(Denominator = 0,0, Numerator / Denominator)
	FROM Unit_Rate
	WHERE UnitFrom='M' and  UnitTo = StockUnit.StockUnit
)UnitRate
OUTER APPLY(
	SELECT top 1 TR.FromSCIRefno,TR.FromSuppColor -- 理應是唯一
	FROM PO_Supp PS 
	INNER JOIN Thread_Replace_Detail_Detail TRDD ON PSD.SCIRefNo=TRDD.ToSCIRefno AND PSD.SuppColor=TRDD.ToBrandSuppColor AND PS.SuppID=TRDD.SuppID 
	INNER JOIN Thread_Replace_Detail TRD ON TRDD.Thread_Replace_DetailUkey = TRD.Ukey
	INNER JOIN Thread_Replace TR ON TRD.Thread_ReplaceUkey = TR.Ukey
	WHERE PS.ID = PSD.ID AND PSD.SEQ1=PS.SEQ1	
)TR1
OUTER APPLY(
	SELECT top 1 TR.FromSCIRefno,TR.FromSuppColor -- 理應是唯一
	FROM PO_Supp PS 
	INNER JOIN Thread_Replace_Detail_Detail TRDD ON PSD.SCIRefNo=TRDD.ToSCIRefno AND PSD.SuppColor=TRDD.ToBrandSuppColor
	INNER JOIN Thread_Replace_Detail TRD ON TRDD.Thread_Replace_DetailUkey = TRD.Ukey
	INNER JOIN Thread_Replace TR ON TRD.Thread_ReplaceUkey = TR.Ukey
	WHERE PS.ID = PSD.ID AND PSD.SEQ1=PS.SEQ1	
)TR2
OUTER APPLY(
	SELECT
		FromSCIRefno = iif(TR1.FromSCIRefno is not null, TR1.FromSCIRefno, TR2.FromSCIRefno),
		FromSuppColor = iif(TR1.FromSuppColor is not null, TR1.FromSuppColor, TR2.FromSuppColor)
)TR
OUTER APPLY(
	SELECT
		 [Val]=sum (g.OpThreadQty)
		,[Qty] = b.QTY
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID,{sqmMachineTypeIDs}) g
    INNER JOIN #step1 s1 on s1.SCIRefNo = g.SCIRefNo AND s1.SuppColor = g.SuppColor and s1.ColorID = g.ColorID AND s1.Article = g.Article
	INNER JOIN #tmp_sumQty b ON s1.Article = b.Article
	WHERE g.SCIRefNo= psd.SCIRefNo AND g.SuppColor = psd.SuppColor and g.ColorID = isnull(psdsC.SpecValue, '')
	GROUP BY g.SCIRefNo, g.ColorID, g.Article, b.QTY
)ThreadUsedQtyByBOT1
OUTER APPLY(
	SELECT
		 [Val]=sum (g.OpThreadQty)
		,[Qty] = b.QTY
	FROM DBO.GetThreadUsedQtyByBOT(psd.ID,{sqmMachineTypeIDs}) g
    INNER JOIN #step1 s1 on s1.SCIRefNo = g.SCIRefNo AND s1.SuppColor = g.SuppColor and s1.ColorID = g.ColorID and s1.Article = g.Article
	INNER JOIN #tmp_sumQty b ON s1.Article = b.Article
	WHERE g.SCIRefNo= TR.FromSCIRefno AND g.SuppColor = TR.FromSuppColor  
	GROUP BY g.SCIRefNo, g.ColorID, g.Article, b.QTY
)ThreadUsedQtyByBOT2
OUTER APPLY(
    select
        Qty = iif(psd.IsForOtherBrand = 1, ThreadUsedQtyByBOT2.Qty, ThreadUsedQtyByBOT1.Qty),
        Val = iif(psd.IsForOtherBrand = 1, ThreadUsedQtyByBOT2.Val, ThreadUsedQtyByBOT1.Val)
)ThreadUsedQtyByBOT
OUTER APPLY(
	SELECT Val = SUM(t.Val)
	FROM #tmpQTFinal　ｔ
	WHERE t.SCIRefNo = psd.SCIRefno AND t.SuppColor = psd.SuppColor AND 1 = {sqmIsQuiting}
)QT1
OUTER APPLY(
	SELECT Val = SUM(t.Val)
	FROM #tmpQTFinal　ｔ
	WHERE t.SCIRefNo = TR.FromSCIRefno AND t.SuppColor = TR.FromSuppColor AND 1 = {sqmIsQuiting}
)QT2
OUTER APPLY(select Val = iif(psd.IsForOtherBrand = 1, QT2.Val, QT1.Val))QT
WHERE psd.ID='{this.poid}'
AND psd.FabricType ='A'
AND EXISTS(
	SELECT 1
	FROM Fabric f
	INNER JOIN MtlType m on m.id= f.MtlTypeID
	WHERE f.SCIRefno = psd.SCIRefNo
	AND m.IsThread=1 
)
AND isnull(psdsC.SpecValue, '') <> ''


SELECT  [Selected] 
		, SCIRefno 
        , Refno
        , ColorID
		, DescDetail
		, [@Qty] = SUM([@Qty])
		, [Use Qty By Stock Unit] = CEILING (SUM([Use Qty By Stock Unit] ))
		, [Stock Unit]
		, [Use Qty By Use Unit] = SUM([Use Qty By Use Unit] )
		, [Use Unit]
		, [Stock Unit Desc.]
		, [Output Qty(Garment)] = SUM([Output Qty(Garment)])
		, [Bulk Balance(Stock Unit)] 
        , [POID]
		, [AccuIssued]
INTO #final2
FROM #final f
GROUP BY [Selected] 
		, SCIRefno 
        , Refno
        , ColorID
		, DescDetail
		, [Stock Unit]
		, [Use Unit]
		, [Stock Unit Desc.]
        , [POID]
		, [AccuIssued]
		, [Bulk Balance(Stock Unit)] 

SELECT  [Selected] 
		, SCIRefno 
        , Refno
        , ColorID
		, SuppColor = SuppCol.Val
		, DescDetail
		, [@Qty] 
		, [Use Qty By Stock Unit]
		, [Stock Unit]
		, [Use Qty By Use Unit]
		, [Use Unit]
		, [Stock Unit Desc.]
		, [Output Qty(Garment)]
		, [Bulk Balance(Stock Unit)] = Balance.Qty
        , [POID]
		, [AccuIssued]
FROM #final2 t
OUTER APPLY(
	SELECT [Qty]=ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0)
	FROM PO_Supp_Detail psd 
	LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
    left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
	WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '')=t.ColorID AND psd.ID='{this.poid}'
)Balance 
OUTER APPLY(
	----列出所有Seq1 Seq2對應到的SuppColor
	SELECT [Val]=STUFF((
		SELECT  DISTINCT ',' + SuppColor
		FROM PO_Supp_Detail y
        left join PO_Supp_Detail_Spec psdsCy WITH (NOLOCK) on psdsCy.ID = y.id and psdsCy.seq1 = y.seq1 and psdsCy.seq2 = y.seq2 and psdsCy.SpecColumnID = 'Color'
		WHERE EXISTS( 
			SELECT 1 --psd.Seq1,psd.Seq2 ,psd.ID ,psd.Seq1, psd.Seq2 ,isnull(psdsC.SpecValue, '')
			FROM PO_Supp_Detail psd 
			LEFT JOIN FtyInventory Fty ON  Fty.poid = psd.ID AND Fty.seq1 = psd.seq1 AND Fty.seq2 = psd.seq2 AND fty.StockType='B'
            left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
			WHERE psd.SCIRefno=t.SCIRefno AND isnull(psdsC.SpecValue, '') = t.ColorID AND psd.ID = '{this.poid}'
			AND psd.SCIRefno = y.SCIRefno AND isnull(psdsC.SpecValue, '') = isnull(psdsCy.SpecValue, '') AND psd.ID = y.ID
			AND psd.SEQ1 = y.SEQ1 AND psd.SEQ2 = y.SEQ2
			GROUP BY psd.seq1,psd.seq2
			--HAVING ISNULL(( SUM(Fty.InQty - Fty.OutQty + Fty.AdjustQty - Fty.ReturnQty)) ,0) > 0
		)
		FOR XML PATH('')
	),1,1,'')
)SuppCol

DROP TABLE #step1,#step2 ,#SelectList1 ,#SelectList2 ,#final,#final2,#tmp,#tmp_sumQty,#tmpQT,#tmpQTFinal
";
            #endregion

            try
            {
                // SqlConnection conn;
                DBProxy.Current.OpenConnection(null, out sqlConnection);
                var dualResult = MyUtility.Tool.ProcessWithDatatable(issueBreakDown_Dt, string.Empty, sqlcmd, out result, "#tmp", conn: sqlConnection);

                if (!dualResult)
                {
                    this.ShowErr(dualResult);
                }

                if (!dualResult)
                {
                    return;
                }

                this.BOA_PO = result;
                this.listControlBindingSource1.DataSource = result;
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox(ex.GetBaseException().ToString(), "Error");
            }
            finally
            {
                dataSet.Dispose();
                sqlConnection.Close();
            }

            this.gridAutoPick.DataSource = this.listControlBindingSource1;

            this.gridAutoPick.AutoResizeColumns();
        }

        /// <inheritdoc/>
        public void Sum_subDetail(DataRow target, DataTable source)
        {
            DataTable tmpDt = source;
            DataRow dr = target;
            if (tmpDt != null)
            {
                var output = string.Empty;
                decimal sumQTY = 0;
                foreach (DataRow dr2 in tmpDt.ToList())
                {
                    if (Convert.ToDecimal(dr2["qty"]) != 0)
                    {
                        output += dr2["sizecode"].ToString() + "*" + Convert.ToDecimal(dr2["qty"]).ToString("0.00") + ", ";
                        sumQTY += Convert.ToDecimal(dr2["qty"]);
                    }
                }

                dr["Output"] = output;
                dr["qty"] = Math.Round(sumQTY, 2);
            }

            if (Convert.ToDecimal(dr["qty"]) > 0)
            {
                dr["Selected"] = 1;
            }
            else
            {
                dr["Selected"] = 0;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPick_Click(object sender, EventArgs e)
        {
            this.gridAutoPick.ValidateControl();
            DataRow[] dr2 = this.BOA_PO.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.InfoBox("Please select rows first!", "Warnning");
                return;
            }

            this.importRows = dr2;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <inheritdoc/>
        public static void ProcessWithDatatable2(DataTable source, string tmp_columns, string sqlcmd, out DataTable[] result, string temptablename = "#tmp")
        {
            result = null;
            StringBuilder sb = new StringBuilder();
            if (temptablename.TrimStart().StartsWith("#"))
            {
                sb.Append(string.Format("create table {0} (", temptablename));
            }
            else
            {
                sb.Append(string.Format("create table #{0} (", temptablename));
            }

            string[] cols = tmp_columns.Split(',');
            for (int i = 0; i < cols.Length; i++)
            {
                if (MyUtility.Check.Empty(cols[i]))
                {
                    continue;
                }

                switch (Type.GetTypeCode(source.Columns[cols[i]].DataType))
                {
                    case TypeCode.Boolean:
                        sb.Append(string.Format("[{0}] bit", cols[i]));
                        break;

                    case TypeCode.Char:
                        sb.Append(string.Format("[{0}] varchar(1)", cols[i]));
                        break;

                    case TypeCode.DateTime:
                        sb.Append(string.Format("[{0}] datetime", cols[i]));
                        break;

                    case TypeCode.Decimal:
                        sb.Append(string.Format("[{0}] numeric(24,8)", cols[i]));
                        break;

                    case TypeCode.Int32:
                        sb.Append(string.Format("[{0}] int", cols[i]));
                        break;

                    case TypeCode.String:
                        sb.Append(string.Format("[{0}] varchar(max)", cols[i]));
                        break;

                    case TypeCode.Int64:
                        sb.Append(string.Format("[{0}] bigint", cols[i]));
                        break;
                    default:
                        break;
                }

                if (i < cols.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(")");

            SqlConnection conn;
            DBProxy.Current.OpenConnection(null, out conn);

            try
            {
                DualResult result2 = DBProxy.Current.ExecuteByConn(conn, sb.ToString());
                if (!result2)
                {
                    MyUtility.Msg.ShowException(null, result2);
                    return;
                }

                using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                {
                    bulkcopy.BulkCopyTimeout = 60;
                    if (temptablename.TrimStart().StartsWith("#"))
                    {
                        bulkcopy.DestinationTableName = temptablename.Trim();
                    }
                    else
                    {
                        bulkcopy.DestinationTableName = string.Format("#{0}", temptablename.Trim());
                    }

                    for (int i = 0; i < cols.Length; i++)
                    {
                        bulkcopy.ColumnMappings.Add(cols[i], cols[i]);
                    }

                    bulkcopy.WriteToServer(source);
                    bulkcopy.Close();
                }

                result2 = DBProxy.Current.SelectByConn(conn, sqlcmd, out result);
                if (!result2)
                {
                    MyUtility.Msg.ShowException(null, result2);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <inheritdoc/>
        public DataTable GetAutoDetailDataTable(int rowIndex)
        {
            DataTable tmpDt = this.dictionaryDatas[this.gridAutoPick.GetDataRow(rowIndex)];
            return tmpDt;
        }

        /// <inheritdoc/>
        public DataRow GetAutoDetailDataRow(int rowIndex)
        {
            DataRow tmpDt = this.gridAutoPick.GetDataRow<DataRow>(rowIndex);
            return tmpDt;
        }

        /// <inheritdoc/>
        public DataRow GetNeedChangeDataRow(int rowIndex)
        {
            DataRow tmpDt = this.gridAutoPick.GetDataRow<DataRow>(rowIndex);
            return tmpDt;
        }

        /// <inheritdoc/>
        public void DictionaryDatasRejectChanges()
        {
            var d = this.dictionaryDatas.AsEnumerable().ToList();
            for (int i = 0; i < d.Count; i++)
            {
                d[i].Value.RejectChanges();
            }

            return;
            ////批次RejectChanges
            // foreach (KeyValuePair<DataRow, DataTable> item in dictionaryDatas)
            // {
            //    item.Value.RejectChanges();
            // }
        }

        /// <inheritdoc/>
        public void DictionaryDatasAcceptChanges()
        {
            // 批次RejectChanges
            var d = this.dictionaryDatas.AsEnumerable().ToList();
            for (int i = 0; i < d.Count; i++)
            {
                d[i].Value.AcceptChanges();
            }

            return;

            // foreach (KeyValuePair<DataRow, DataTable> item in dictionaryDatas)
            // {
            //    item.Value.AcceptChanges();
            // }
        }

        private void TxtMachineType_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlcmd;

            sqlcmd = $@"
SELECT  DISTINCT sth.MachineTypeID
FROM  Orders o
INNER JOIN PO p ON o.POID = p.ID
INNER JOIN Style_ThreadColorCombo_History sth ON sth.StyleUkey=o.StyleUkey AND sth.Version=p.ThreadVersion
where o.ID = '{this.poid}'
";

            Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(sqlcmd, string.Empty, this.txtMachineType.Text);

            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtMachineType.Text = item.GetSelectedString();
            this.selectAllMachineType = false;
            this.chkSewingType.Checked = true;
        }

        private void TxtMachineType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string machineTypeIDs = this.txtMachineType.Text;

            if (!MyUtility.Check.Empty(machineTypeIDs))
            {
                var machineTypeID_List = machineTypeIDs.Replace("'", string.Empty).Split(',').ToList();

                string sqlcmd;

                sqlcmd = $@"
SELECT STUFF((
SELECT  DISTINCT ','+sth.MachineTypeID
FROM  Orders o
INNER JOIN PO p ON o.POID = p.ID
INNER JOIN Style_ThreadColorCombo_History sth ON sth.StyleUkey=o.StyleUkey AND sth.Version=p.ThreadVersion
where o.ID = '{this.poid}' AND sth.MachineTypeID IN ('{machineTypeID_List.JoinToString("','")}')
FOR XML PATH('')
),1,1,'')
";

                string result = MyUtility.GetValue.Lookup(sqlcmd);
                this.txtMachineType.Text = result;
            }
        }

        /// <summary>
        /// 取得所有MachineTypeID
        /// </summary>
        private void SetDefaultTxtMachineType()
        {
            string sqlcmd;

            sqlcmd = $@"
SELECT STUFF((
SELECT  DISTINCT ','+sth.MachineTypeID
FROM  Orders o
INNER JOIN PO p ON o.POID = p.ID
INNER JOIN Style_ThreadColorCombo_History sth ON sth.StyleUkey=o.StyleUkey AND sth.Version=p.ThreadVersion
where o.ID = '{this.poid}'
FOR XML PATH('')
),1,1,'')
";
            string machineType = MyUtility.GetValue.Lookup(sqlcmd);
            this.txtMachineType.Text = machineType;
        }

        private void BtnAutoCacu_Click(object sender, EventArgs e)
        {
            this.Query(this.chkQuiting.Checked, this.txtMachineType.Text);
        }

        private void ChkSewingType_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSewingType.Checked && this.selectAllMachineType)
            {
                this.SetDefaultTxtMachineType();
            }
            else if (this.chkSewingType.Checked && !this.selectAllMachineType)
            {
                this.selectAllMachineType = true;
            }
            else
            {
                this.txtMachineType.Text = string.Empty;
            }
        }
    }
}
