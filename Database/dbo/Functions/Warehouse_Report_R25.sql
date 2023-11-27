
CREATE FUNCTION Warehouse_Report_R25
(
     @ImportBI BIT = 0
    , @StartDate date = NULL
    , @EndDate date = NULL
    , @WK1 VARCHAR(13) = NULL
    , @WK2 VARCHAR(13) = NULL
    , @POID1 VARCHAR(13) = NULL
    , @POID2 VARCHAR(13) = NULL
    , @SuppID VARCHAR(6) = NULL
    , @FabricType VARCHAR(255) = NULL
    , @WhseArrival1 DATE = NULL
    , @WhseArrival2 DATE = NULL
    , @Eta1 DATE = NULL
    , @Eta2 DATE = NULL
    , @BrandID VARCHAR(8) = NULL
    , @MDivisionID VARCHAR(8) = NULL
    , @FactoryID VARCHAR(255) = NULL
    , @KPILETA1 DATE = NULL
    , @KPILETA2 DATE = NULL
    , @RecLessArv BIT = 0
)
RETURNS TABLE
AS
RETURN
(
--此報表以 Export_Detail.Ukey 展開
SELECT
    WK = ed.ID
   ,ExportDetailUkey = ed.Ukey
   ,e.ETA
   ,o.MDivisionID
   ,o.FactoryID
   ,e.Consignee
   ,e.ShipModeID
   ,e.CYCFS
   ,e.Blno
   ,Packages = ISNULL((SELECT SUM(e2.Packages) FROM Export e2 WITH (NOLOCK) WHERE e2.Blno = e.Blno), 0)
   ,e.Vessel
   ,ProdFactory = o.FactoryID
   ,o.OrderTypeID
   ,o.ProjectID
   ,Category =
        CASE
            WHEN o.Category = 'B' THEN 'Bulk'
            WHEN o.Category = 'M' THEN 'Material'
            WHEN o.Category = 'S' THEN 'Sample'
            WHEN o.Category = 'T' THEN 'SMLT'
            WHEN o.Category = 'G' THEN 'Garment'
            WHEN o.Category = 'O' THEN 'Other'
        END
   ,o.BrandID
   ,o.SeasonID
   ,o.StyleID
   ,StyleName = ISNULL(s.StyleName, '')
   ,ed.POID
   ,Seq = ed.Seq1 + ' ' + ed.Seq2
   ,ed.Refno
   ,Color = ISNULL(psdsC.SpecValue, '')
   ,ColorName = ISNULL(colName.value, '')
   ,[Description] = dbo.getmtldesc(ed.POID, ed.seq1, ed.seq2, 2, 0)
   ,MtlType =
        CASE
            WHEN ed.FabricType = 'F' THEN 'Fabric'
            WHEN ed.FabricType = 'A' THEN 'Accessory'
        END
   ,SubMtlType = ISNULL(f.MtlTypeID, '')
   ,WeaveType = ISNULL(f.WeaveTypeID, '')
   ,ed.SuppID
   ,SuppName = ISNULL(supp.AbbEN, '')
   ,ed.UnitID
   ,SizeSpec = ISNULL(psdsS.SpecValue, '')
   ,ShipQty = ed.Qty
   ,ed.FOC
   ,ed.NetKg
   ,ed.WeightKg
   ,ArriveQty = ISNULL(ed.Qty, 0) + ISNULL(ed.FOC, 0)
   ,ArriveQtyStockUnit
   ,FirstBulkSewInLine = (SELECT val = MIN(SewInLine) FROM Orders WHERE POID = o.POID AND Category = 'B')
   ,c.FirstCutDate
   ,ReceiveQty = ISNULL(rd.ReceiveQty, 0)
   ,TotalRollsCalculated = IIF(ed.FabricType = 'F', ISNULL(t.TotalRollsCalculated, 0), 0)
   ,psd.StockUnit
   ,MCHandle = CONCAT(o.MCHandle, ' ' + mc.Name)
   ,ContainerType = ISNULL(ContainerType, '')
   ,ContainerNo = ISNULL(ContainerNo, '')
   ,e.PortArrival
   ,e.WhseArrival
   ,o.KPILETA
   ,PFETA = (SELECT MIN(PFETA) FROM Orders WITH (NOLOCK) WHERE POID = o.POID)
   ,EarliestSCIDelivery = (SELECT MinSciDelivery FROM DBO.GetSCI(ed.POID, o.Category))
   ,EarlyDays = ISNULL(DATEDIFF(DAY, e.WhseArrival, o.KPILETA), 0)
   ,EarliestPFETA = ISNULL(DATEDIFF (day, e.WhseArrival, PFETA), 0)
   ,MRMail = ISNULL(TEPPOHandle.Email, '')
   ,SMRMail = ISNULL(TEPPOSMR.Email, '')
   ,EditName = ISNULL(dbo.getTPEPass1(e.EditName), '')
   ,e.AddDate
   ,e.EditDate

FROM Export_Detail ed WITH (NOLOCK)
--以下皆是串各表 Pkey 沒有再展開
INNER JOIN Export e WITH (NOLOCK) ON e.ID = ed.ID
INNER JOIN Orders o WITH (NOLOCK) ON o.ID = ed.POID
INNER JOIN PO_Supp_Detail psd ON psd.id = ed.POID AND psd.SEQ1 = ed.Seq1  AND psd.SEQ2 = ed.Seq2
INNER JOIN PO ON PO.id = ed.POID
LEFT JOIN Style s WITH (NOLOCK) ON s.Ukey = o.StyleUkey
LEFT JOIN Supp ON supp.id = ed.suppid
LEFT JOIN TPEPass1 TEPPOHandle ON TEPPOHandle.id = po.POHandle
LEFT JOIN TPEPass1 TEPPOSMR  ON TEPPOSMR.id = po.POSMR
LEFT JOIN Pass1 mc WITH (NOLOCK) ON mc.ID = o.MCHandle
LEFT JOIN Fabric f WITH (NOLOCK) ON f.SCIRefno = psd.SCIRefno
LEFT JOIN Cutting c WITH (NOLOCK) ON c.id = o.CuttingSP
LEFT JOIN PO_Supp_Detail_Spec psdsC WITH (NOLOCK)
    ON psdsC.ID = psd.id
    AND psdsC.seq1 = psd.seq1
    AND psdsC.seq2 = psd.seq2
    AND psdsC.SpecColumnID = 'Color'
LEFT JOIN PO_Supp_Detail_Spec psdsS WITH (NOLOCK)
    ON psdsS.ID = psd.id
    AND psdsS.seq1 = psd.seq1
    AND psdsS.seq2 = psd.seq2
    AND psdsS.SpecColumnID = 'Size'
OUTER APPLY(SELECT ArriveQtyStockUnit = dbo.[GetUnitQty](UnitId, StockUnit, ISNULL(ed.Qty, 0) + ISNULL(ed.FOC, 0)))ArriveQtyStockUnit
OUTER APPLY ( 
    SELECT [value] = stuff((
        SELECT concat('/', c2.Name)
        FROM dbo.Color c
        LEFT JOIN dbo.Color_multiple m ON m.ID = c.ID AND m.BrandID = c.BrandId
        LEFT JOIN color c2 ON c2.id = m.ColorID AND c2.BrandID = m.BrandID
        WHERE c.ID = ISNULL(psdsC.SpecValue, '')
        AND c.BrandId = o.BrandID
        ORDER BY m.Seqno
        FOR XML PATH ('')
    ), 1, 1, '')
) colName
OUTER APPLY (
    SELECT ContainerType = STUFF((
        SELECT DISTINCT ',' + esc.ContainerType
        FROM Export_ShipAdvice_Container esc
        WHERE esc.Export_DetailUkey = ed.Ukey
        AND esc.ContainerType <> ''
        AND esc.ContainerNo <> ''
        FOR XML PATH ('')
    ), 1, 1, '')
) ContainerType
OUTER APPLY (
    SELECT ContainerNo = STUFF((
        SELECT DISTINCT ',' + esc.ContainerNo
        FROM Export_ShipAdvice_Container esc
        WHERE esc.Export_DetailUkey = ed.Ukey
        AND esc.ContainerType <> ''
        AND esc.ContainerNo <> ''
        FOR XML PATH ('')
    ), 1, 1, '')
) ContainerNo
OUTER APPLY (
    SELECT
        TotalRollsCalculated = COUNT(1)
    FROM Receiving r WITH (NOLOCK)
    INNER JOIN Receiving_Detail rd WITH (NOLOCK) ON r.id = rd.id
    WHERE r.ExportID = ed.ID
    AND r.status = 'Confirmed'
    AND rd.POID = ed.POID
    AND rd.Seq1 = ed.SEQ1
    AND rd.Seq2 = ed.SEQ2
) t
OUTER APPLY(
	SELECT ReceiveQty = SUM(rd.StockQty)
	FROM Receiving r 
	INNER JOIN Receiving_Detail rd WITH (NOLOCK) ON rd.ID = r.ID AND rd.POID = ed.POID AND rd.Seq1 = ed.Seq1 AND rd.Seq2 = ed.Seq2
	WHERE ed.ID = r.ExportID
    AND r.Status = 'Confirmed'
)rd
WHERE ed.PoType = 'G'
AND EXISTS (SELECT 1 FROM Factory WHERE IsProduceFty = 1 AND ID = o.FactoryID)
AND (ISNULL(@WK1, '') = '' OR (@WK1 <> '' AND ed.ID >= @WK1))
AND (ISNULL(@WK2, '') = '' OR (@WK2 <> '' AND ed.ID <= @WK2))
AND (ISNULL(@POID1, '') = '' OR (@POID1 <> '' AND ed.POID >= @POID1))
AND (ISNULL(@POID2, '') = '' OR (@POID2 <> '' AND ed.POID <= @POID2))
AND (ISNULL(@SuppID, '') = '' OR (@SuppID <> '' AND ed.SuppID = @SuppID))
AND (ISNULL(@FabricType, '') = '' OR (@FabricType <> '' AND ed.FabricType IN (SELECT Data FROM dbo.SplitString(@FabricType, ','))))

AND (@WhseArrival1 IS NULL OR (@WhseArrival1 IS NOT NULL AND e.WhseArrival BETWEEN @WhseArrival1 AND @WhseArrival2))
AND (@Eta1 IS NULL OR (@Eta1 IS NOT NULL AND e.Eta BETWEEN @Eta1 AND @Eta2))

AND (ISNULL(@BrandID, '') = '' OR (@BrandID <> '' AND o.BrandID = @BrandID))
AND (ISNULL(@MDivisionID, '') = '' OR (@MDivisionID <> '' AND o.MDivisionID = @MDivisionID))
AND (ISNULL(@FactoryID, '') = '' OR (@FactoryID <> '' AND o.FactoryID IN (SELECT Data FROM dbo.SplitString(@FactoryID, ','))))

AND (@KPILETA1 IS NULL OR (@KPILETA1 IS NOT NULL AND o.KPILETA BETWEEN @KPILETA1 AND @KPILETA2))
AND (@RecLessArv = 0 OR (@RecLessArv = 1 AND (rd.ReceiveQty is null or (rd.ReceiveQty < ArriveQtyStockUnit))))
AND (@ImportBI = 0
    OR (@ImportBI = 1 AND(
            e.AddDate BETWEEN @StartDate AND @EndDate
        OR e.EditDate BETWEEN @StartDate AND @EndDate)))
);