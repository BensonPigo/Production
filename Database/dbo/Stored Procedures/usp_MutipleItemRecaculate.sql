CREATE PROCEDURE usp_MutipleItemRecaculate
	@MaterialItemList dbo.MaterialItemList  READONLY    
AS
BEGIN
	SET NOCOUNT ON;
    
    SELECT
         MDivisionPoDetailUkey = m.Ukey
        ,ml.POID
        ,ml.Seq1
        ,ml.Seq2    
    INTO #MtlList
    FROM @MaterialItemList ml
    LEFT JOIN MDivisionPoDetail m WITH (NOLOCK) ON m.POID = ml.POID AND m.SEQ1 = ml.SEQ1 AND m.SEQ2 = ml.SEQ2
    
    ---- 補上 MDivisionPoDetailUkey 缺失
    INSERT INTO MDivisionPoDetail (POID, SEQ1, SEQ2)
    SELECT mtl.POID, mtl.SEQ1, mtl.SEQ2
    FROM #MtlList mtl
    WHERE MDivisionPoDetailUkey IS NULL

    ---- 更新 #MtlList. MDivisionPoDetailUkey
    UPDATE mtl
    SET mtl.MDivisionPoDetailUkey = m.Ukey
    FROM #MtlList mtl
    INNER JOIN MDivisionPoDetail m ON mtl.POID = m.POID AND mtl.SEQ1 = m.SEQ1 AND mtl.SEQ2 = m.SEQ2
    WHERE mtl.MDivisionPoDetailUkey IS NULL
    
    /*
	    2. 根據 View 找出對應庫存的累積交易數, 展開到 Roll + Dyelot
	    InQty, OutQty, AdjustQty, ReturnQty
    */
    SELECT
        a.MDivisionPoDetailUkey
        ,a.POID, a.SEQ1, a.SEQ2
        ,b.Roll, b.Dyelot
        ,AInQty = SUM(AInQty), AOutQty = SUM(AOutQty), AAdjustQty = SUM(AAdjustQty), AReturnQty = SUM(AReturnQty)
        ,BInQty = SUM(BInQty), BOutQty = SUM(BOutQty), BAdjustQty = SUM(BAdjustQty), BReturnQty = SUM(BReturnQty)
        ,CInQty = SUM(CInQty), COutQty = SUM(COutQty), CAdjustQty = SUM(CAdjustQty), CReturnQty = SUM(CReturnQty)
    INTO #TransactionGroupByRollDyelot
    FROM #MtlList a
    INNER JOIN View_TransactionList b WITH (NOLOCK) ON a.POID = b.POID AND a.SEQ1 = b.SEQ1 AND a.SEQ2 = b.SEQ2
    WHERE b.status IN ('Confirmed', 'Closed')
    GROUP BY a.MDivisionPoDetailUkey
        ,a.POID, a.SEQ1, a.SEQ2
        ,b.Roll, b.Dyelot

    /*
	    3. 根據交易彙整清單更新 FtyInventory (B, I, O)
    */
	---- B : Bulk ----
    MERGE DBO.FtyInventory AS T
    USING #TransactionGroupByRollDyelot AS S
    ON T.POID = S.POID AND T.SEQ1 = S.SEQ1 AND T.SEQ2 = S.SEQ2 AND T.Roll = S.Roll and T.Dyelot = S.Dyelot and t.stocktype = 'B'
    WHEN MATCHED THEN 
	    UPDATE SET InQty = S.AInQty, OutQty = s.AOutQty , AdjustQty = AAdjustQty, ReturnQty = AReturnQty
    WHEN NOT MATCHED BY TARGET THEN 
	    INSERT (  MDivisionPoDetailUkey,   POID,   SEQ1,   SEQ2,   Roll, Stocktype, Dyelot,   InQty,    OutQty,    AdjustQty,  ReturnQty)
	    VALUES (s.MDivisionPoDetailUkey, S.POID, S.SEQ1, S.SEQ2, S.Roll, 'B',     S.Dyelot, S.AInQty, S.AOutQty, S.AAdjustQty, AReturnQty)
    ;
	
	---- I : Inventory ----
	MERGE  DBO.FTYINVENTORY AS T
	USING #TransactionGroupByRollDyelot AS S
	ON T.POID = S.POID AND T.SEQ1 = S.SEQ1 AND T.SEQ2 = S.SEQ2 AND T.Roll = S.Roll and T.Dyelot = S.Dyelot and t.stocktype = 'I'
	WHEN MATCHED THEN 
		UPDATE SET InQty = S.BInQty, OutQty = s.BOutQty , AdjustQty = BAdjustQty, ReturnQty = BReturnQty
	WHEN NOT MATCHED BY TARGET THEN 
	    INSERT (  MDivisionPoDetailUkey,   POID,   SEQ1,   SEQ2,   Roll, Stocktype, Dyelot,   InQty,    OutQty,    AdjustQty,  ReturnQty)
		VALUES (s.MDivisionPoDetailUkey, S.POID, S.SEQ1, S.SEQ2, S.Roll, 'I',     S.Dyelot, S.BInQty, S.BOutQty, S.BAdjustQty, BReturnQty)
    ;

	---- O : Scrap ----
	MERGE DBO.FtyInventory AS T
	USING #TransactionGroupByRollDyelot AS S
	ON T.POID = S.POID AND T.SEQ1 = S.SEQ1 AND T.SEQ2 = S.SEQ2 AND T.Roll = S.Roll and T.Dyelot = S.Dyelot and t.stocktype = 'O'
	WHEN MATCHED THEN 
		UPDATE SET InQty = S.CInQty, OutQty = s.COutQty , AdjustQty = CAdjustQty, ReturnQty = CReturnQty
	WHEN NOT MATCHED BY TARGET THEN 
	    INSERT (  MDivisionPoDetailUkey,   POID,   SEQ1,   SEQ2,   Roll, Stocktype, Dyelot,   InQty,    OutQty,    AdjustQty,  ReturnQty)
		VALUES (s.MDivisionPoDetailUkey, S.POID, S.SEQ1, S.SEQ2, S.Roll, 'O',     S.Dyelot, S.CInQty, S.COutQty, S.CAdjustQty, CReturnQty)
    ;

    /*
	    4. 根據 View 找出對應庫存的累積交易數 By Seq
    */
    SELECT
        a.MDivisionPoDetailUkey
       ,b.POID
       ,b.SEQ1
       ,b.SEQ2
       ,InQty = SUM(InQty)
       ,OutQty = SUM(OutQty)
       ,AdjustQty = SUM(AdjustQty)
       ,ReturnQty = SUM(ReturnQty)
       ,BBalance = SUM(BInQty) - SUM(BOutQty) + SUM(BAdjustQty) - SUM(BReturnQty)
       ,CBalance = SUM(CInQty) - SUM(COutQty) + SUM(CAdjustQty) - SUM(CReturnQty)
    INTO #TransactionGroupBySeq
    FROM #MtlList a
    INNER JOIN View_TransactionList b WITH (NOLOCK) ON a.POID = b.POID AND a.SEQ1 = b.SEQ1 AND a.SEQ2 = b.SEQ2
    WHERE b.status IN ('Confirmed', 'Closed')
    GROUP BY a.MDivisionPoDetailUkey, b.POID, b.SEQ1, b.SEQ2
    
    /*
	    5. 根據交易彙整清單更新 MDivisionPoDetail
    */
    UPDATE MDivisionPoDetail
    SET InQty = ISNULL(t.InQty, 0)
       ,OutQty = ISNULL(t.OutQty, 0)
       ,AdjustQty = ISNULL(t.AdjustQty, 0)
       ,LInvQty = ISNULL(t.BBalance, 0)
       ,LObQty = ISNULL(t.CBalance, 0)
       ,ReturnQty = ISNULL(t.ReturnQty, 0)
    FROM MDivisionPoDetail WITH (NOLOCK)
    INNER JOIN #TransactionGroupBySeq t
        ON t.POID = MDivisionPoDetail.POID
        AND t.SEQ1 = MDivisionPoDetail.SEQ1
        AND t.SEQ2 = MDivisionPoDetail.SEQ2

    DROP TABLE #MtlList, #TransactionGroupByRollDyelot, #TransactionGroupBySeq

END