
CREATE function [dbo].[GetBOAExpend_NEW]
(
	  @ID				VarChar(13)				--採購母單
	 ,@Order_BOAUkey	BigInt		= 0			--BOA Ukey
	 ,@IsGetFabQuot		Bit			= 1
	 ,@IsExpendDetail	Bit			= 0			--是否一律展開至最詳細	 
	 ,@Tmp_Order_Qty    dbo.QtyBreakdown readonly
	 ,@IsExpendArticle	Bit			= 0			--add by Edward 是否展開至Article，For U.A轉單
	 ,@IncludeQtyZero	Bit			= 0			--add by Edward 是否包含數量為0
	 ,@IsForPMS Bit = 0 --有一些欄位PMS使用不需要，用這個傳入參數區分是否要抓
)
RETURNS @Tmp_BoaExpend table (  ExpendUkey BigInt Identity(1,1) Not Null, ID Varchar(13), Order_BOAUkey BigInt
	, RefNo VarChar(36), SCIRefNo VarChar(30), Article VarChar(8), ColorID VarChar(6), SuppColor NVarChar(Max)
	, SizeSeq VarChar(2), SizeCode VarChar(8), SizeSpec VarChar(15), SizeUnit VarChar(8), Remark NVarChar(Max)
	, OrderQty Numeric(6,0)
    --, Price Numeric(12,4)--pms does not use this column
    , UsageQty Numeric(11,2), UsageUnit VarChar(8), SysUsageQty  Numeric(11,2)
	, BomZipperInsert VarChar(5), BomCustPONo VarChar(30), Keyword VarChar(Max), Keyword_Original VARCHAR(MAX), Keyword_xml VARCHAR(MAX), OrderList varchar(max), ColorDesc varchar(150), Special nvarchar(max)
	, BomTypeColorID varchar(50), BomTypeSize varchar(50), BomTypeSizeUnit varchar(50), BomTypeZipperInsert varchar(50), BomTypeArticle varchar(50), BomTypeCOO varchar(50)
	, BomTypeGender varchar(50), BomTypeCustomerSize varchar(50), BomTypeDecLabelSize varchar(50), BomTypeBrandFactoryCode varchar(50), BomTypeStyle varchar(50)
    , BomTypeStyleLocation varchar(50), BomTypeSeason varchar(50), BomTypeCareCode varchar(50), BomTypeCustomerPO varchar(50), BomTypeBuyMonth varchar(50), BomTypeBuyerDlvMonth varchar(50)
	, Index Idx_ID NonClustered (ID, Order_BOAUkey, ColorID) -- table index
)
As
begin
	
	--if not exists(select 1 from @Tmp_Order_Qty)
	--begin
	--	insert into @Tmp_Order_Qty
	--	select Order_Qty.ID ,Article ,SizeCode ,Order_Qty.Qty ,SewOutputQty ,SewOutputUpdate ,OriQty
	--		From dbo.Order_Qty
	--		Inner Join dbo.Orders On Orders.ID = Order_Qty.ID
	--	 Where Orders.PoID = @ID
	--end

	--取得訂單基本資料:Brand, Style, Season, Program, Category, Cfm. Date
	Declare @BrandID VarChar(8);
	Declare @StyleID VarChar(15);
	Declare @SeasonID VarChar(10);
	Declare @FactoryID VarChar(8);
	Declare @Category VarChar(1);
	Declare @ProgramID VarChar(12);
	Declare @CfmDate Date;
	Declare @StyleUkey bigint;

	SELECT
		@BrandID = Orders.BrandId
	   ,@StyleID = Orders.StyleID
	   ,@SeasonID = Orders.SeasonID
	   ,@FactoryID = Orders.FactoryID
	   ,@ProgramID = Orders.ProgramID
	   ,@Category = Orders.Category
	   ,@CfmDate = Orders.CFMDate
	   ,@StyleUkey = Style.Ukey
	FROM dbo.Orders
	LEFT JOIN Production.dbo.Style
		ON Orders.BrandId = Style.BrandId
			AND Orders.SeasonID = Style.SeasonID
			AND Orders.StyleID = Style.ID
	WHERE Orders.ID = @ID;

	--定義欄位
	DECLARE @SCIRefNo VARCHAR(30);
	DECLARE @RefNo VARCHAR(36);
	--DECLARE @Price NUMERIC(12, 4);--pms does not use this column
	DECLARE @OrderQty NUMERIC(6, 0);
	DECLARE @UsageQty NUMERIC(11, 2);
	DECLARE @UsageUnit VARCHAR(8);
	DECLARE @Article VARCHAR(8);
	DECLARE @ColorID VARCHAR(6);
	DECLARE @SuppColor NVARCHAR(70);
	DECLARE @SizeSeq VARCHAR(2);
	DECLARE @SizeItem VARCHAR(3);
	DECLARE @SizeCode VARCHAR(8);
	DECLARE @SizeSpec VARCHAR(15);
	DECLARE @SizeUnit VARCHAR(8);
	DECLARE @SysUsageQty NUMERIC(11, 2)
	DECLARE @Remark NVARCHAR(MAX);
	--Declare @BomFactory VarChar(8);
	--Declare @BomCustCD VarChar(16);
	DECLARE @BomZipperInsert VARCHAR(5);
	DECLARE @BomCustPONo VARCHAR(30);
	--Declare @BomBuymonth VarChar(16);
	--Declare @BomCountry VarChar(2);
	--Declare @BomStyle VarChar(15);
	DECLARE @BomArticle VARCHAR(8);
	DECLARE @Keyword VARCHAR(MAX);
	DECLARE @Keyword_Trans VARCHAR(MAX);
	DECLARE @OrderList VARCHAR(MAX);

	DECLARE @OrderID VARCHAR(13);

	DECLARE @MtlTypeID VARCHAR(20);
	DECLARE @BomTypeCalculate BIT;
	DECLARE @BomTypeCalculateWeight BIT;
	DECLARE @NoSizeUnit BIT;
	DECLARE @SizeSpec_Cal NUMERIC(15, 4);

	DECLARE @tmpBOACustCdIsExist BIT;

	--取得採購組合的訂單Q'ty breakdown
	DECLARE @tmpID VARCHAR(13);
	DECLARE @tmpOrderComboID VARCHAR(13);
	DECLARE @tmpPoID VARCHAR(13);
	DECLARE @tmpFactoryID VARCHAR(8);
	DECLARE @tmpCustCDID VARCHAR(16);
	DECLARE @tmpZipperInsert VARCHAR(5);
	DECLARE @tmpCustPONo VARCHAR(30);
	DECLARE @tmpBuyMonth VARCHAR(16);
	DECLARE @tmpCountryID VARCHAR(2);
	DECLARE @tmpStyleID VARCHAR(15);
	DECLARE @tmpArticle VARCHAR(8);
	DECLARE @tmpSizeSeq VARCHAR(2);
	DECLARE @tmpSizeCode VARCHAR(8);
	DECLARE @tmpSizeUnit VARCHAR(8);
	DECLARE @tmpSizeSpec VARCHAR(15);
	DECLARE @tmpQty NUMERIC(6, 0);

	DECLARE @tmpOrder_Qty TABLE (
		RowID BIGINT IDENTITY (1, 1) NOT NULL
	   ,ID VARCHAR(13)
	   ,OrderComboID VARCHAR(13)
	   ,POID VARCHAR(13)
	   ,FactoryID VARCHAR(8)
	   ,CustCDID VARCHAR(16)
	   ,ZipperInsert VARCHAR(5)
	   ,Kit VARCHAR(10)
	   ,CustCDCountryID VARCHAR(2)
	   ,CustPONo VARCHAR(30)
	   ,BuyMonth VARCHAR(16)
	   ,CountryID VARCHAR(2)
	   ,StyleID VARCHAR(15)
	   ,Article VARCHAR(8)
	   ,SizeSeq VARCHAR(2)
	   ,SizeCode VARCHAR(8)
	   ,Qty NUMERIC(6, 0)
	);
	DECLARE @Order_QtyRowID INT;		--tmpOrder_Qty ID
	DECLARE @Order_QtyRowCount INT;		--tmpOrder_Qty總資料筆數

	INSERT INTO @tmpOrder_Qty (ID, OrderComboID, POID, FactoryID, CustCDID, ZipperInsert, Kit, CustCDCountryID
	, CustPONo, BuyMonth, CountryID, StyleID
	, Article, SizeSeq, SizeCode, Qty)
	SELECT
		Orders.ID
		,Orders.OrderComboID
		,Orders.POID
		,Orders.FactoryID
		,Orders.CustCDID
		,CustCD.ZipperInsert
		,CustCD.Kit
		,CustCD.CountryID
		,Orders.CustPONo
		,Orders.BuyMonth
		,Factory.CountryID
		,Orders.StyleID
		,Order_Article.Article
		,Order_SizeCode.Seq
		,Order_SizeCode.SizeCode
		,ISNULL(Tmp_Order_Qty.Qty, 0) Qty
	FROM dbo.Orders
	LEFT JOIN dbo.Order_SizeCode
		ON Order_SizeCode.ID = Orders.POID
	LEFT JOIN dbo.Order_Article
		ON Order_Article.ID = Orders.ID
	LEFT JOIN @Tmp_Order_Qty AS Tmp_Order_Qty
		ON Tmp_Order_Qty.ID = Orders.ID
			AND Tmp_Order_Qty.SizeCode = Order_SizeCode.SizeCode
			AND Tmp_Order_Qty.Article = Order_Article.Article
	LEFT JOIN Production.dbo.CustCD
		ON CustCD.BrandId = Orders.BrandId
			AND CustCD.ID = Orders.CustCDID
	LEFT JOIN Production.dbo.Factory
		ON Factory.ID = Orders.FactoryID
	WHERE Orders.POID = @ID
	AND dbo.CheckOrder_CalculateMtlUsage(Orders.ID) = 1
	ORDER BY ID, FactoryID, CustCDID, ZipperInsert, CustPONo, BuyMonth
	, Factory.CountryID, StyleID, Article, Seq, SizeCode;

	DECLARE @Sum_Qty TABLE (
		RowID BIGINT IDENTITY (1, 1) NOT NULL
	   ,OrderID VARCHAR(13)
	   ,ColorID VARCHAR(6)
	   ,Article VARCHAR(8)
	   ,BomZipperInsert VARCHAR(5)
	   ,BomCustPONo VARCHAR(30)
	   ,BomArticle VARCHAR(8)
	   ,SizeSeq VARCHAR(2)
	   ,SizeCode VARCHAR(8)
	   ,SizeSpec VARCHAR(15)
	   ,SizeUnit VARCHAR(8)
	   ,OrderQty NUMERIC(6, 0)
	   ,UsageQty NUMERIC(11, 2)
	   ,Keyword VARCHAR(MAX)
	   ,Keyword_Original VARCHAR(MAX)
	   ,Keyword_xml VARCHAR(MAX)
	   ,OrderList VARCHAR(MAX)
	   ,Special NVARCHAR(MAX)	   
	   ,BomTypeColorID varchar(50)
	   ,BomTypeSize varchar(50)
	   ,BomTypeSizeUnit varchar(50)
	   ,BomTypeZipperInsert varchar(50)
	   ,BomTypeArticle varchar(50)
	   ,BomTypeCOO varchar(50)
	   ,BomTypeGender varchar(50)
	   ,BomTypeCustomerSize varchar(50)
	   ,BomTypeDecLabelSize varchar(50)
	   ,BomTypeBrandFactoryCode varchar(50)
	   ,BomTypeStyle varchar(50)
	   ,BomTypeStyleLocation varchar(50)
	   ,BomTypeSeason varchar(50)
	   ,BomTypeCareCode varchar(50)
	   ,BomTypeCustomerPO varchar(50)
       ,BomTypeBuyMonth varchar(50)
       ,BomTypeBuyerDlvMonth varchar(50)
	);
	DECLARE @tmpTbl TABLE (
		RowID BIGINT IDENTITY (1, 1) NOT NULL
	   ,ID VARCHAR(13)
	   ,ColorID VARCHAR(6)
	   ,Article VARCHAR(8)
	   ,BomZipperInsert VARCHAR(5)
	   ,BomCustPONo VARCHAR(30)
	   ,SizeSeq VARCHAR(2)
	   ,SizeCode VARCHAR(8)
	   ,SizeSpec VARCHAR(15)
	   ,SizeUnit VARCHAR(8)
	   ,OrderQty NUMERIC(6, 0)
	   ,UsageQty NUMERIC(11, 2)
	   ,Keyword VARCHAR(MAX)
	   ,Keyword_Original VARCHAR(MAX)	   
	   ,Keyword_xml VARCHAR(MAX)
	   ,Special NVARCHAR(MAX)
	   ,BomTypeColorID varchar(50)
	   ,BomTypeSize varchar(50)
	   ,BomTypeSizeUnit varchar(50)
	   ,BomTypeZipperInsert varchar(50)
	   ,BomTypeArticle varchar(50)
	   ,BomTypeCOO varchar(50)
	   ,BomTypeGender varchar(50)
	   ,BomTypeCustomerSize varchar(50)
	   ,BomTypeDecLabelSize varchar(50)
	   ,BomTypeBrandFactoryCode varchar(50)
	   ,BomTypeStyle varchar(50)
	   ,BomTypeStyleLocation varchar(50)
	   ,BomTypeSeason varchar(50)
	   ,BomTypeCareCode varchar(50)
	   ,BomTypeCustomerPO varchar(50)
       ,BomTypeBuyMonth varchar(50)
       ,BomTypeBuyerDlvMonth varchar(50)
	);
	DECLARE @Sum_QtyRowID INT;		--Sum_Qty ID
	DECLARE @Sum_QtyRowCount INT;	--Sum_Qty總資料筆數

	DECLARE @BoaUkey BIGINT;
	DECLARE @BoaSuppID VARCHAR(6);
	DECLARE @BoaFabricPanelCode VARCHAR(2);
	DECLARE @BoaSizeItem VARCHAR(3);
	DECLARE @BoaSizeItem_PCS VARCHAR(3);
	DECLARE @BoaSizeItem_Elastic VARCHAR(3);
	DECLARE @BoaConsPC NUMERIC(8, 4);
	DECLARE @BoaIsForArticle BIT;

	--BOA BomType
	DECLARE @BoaBomTypeColor BIT;
	DECLARE @BoaBomTypeSize BIT;
	DECLARE @BoaBomTypeZipper BIT;
	DECLARE @BoaBomTypeArticle BIT;
	DECLARE @BoaBomTypeCOO BIT;
	DECLARE @BoaBomTypeGender BIT;
	DECLARE @BoaBomTypeCustomerSize BIT;
	DECLARE @BoaBomTypeDecLabelSize BIT;
	DECLARE @BoaBomTypeBrandFactoryCode BIT;
	DECLARE @BoaBomTypeStyle BIT;
	DECLARE @BoaBomTypeStyleLocation BIT;
	DECLARE @BoaBomTypeSeason BIT;
	DECLARE @BoaBomTypeCareCode BIT;
	DECLARE @BoaBomTypePo BIT;
    DECLARE @BomTypeBuyMonth BIT;
    DECLARE @BomTypeBuyerDlvMonth BIT;

	DECLARE @BomTypeCalculatePCS BIT;
	DECLARE @BomTypeMatching BIT;
	DECLARE @BoaIsCustCD NUMERIC(2, 0);
	DECLARE @BoaCursor TABLE (
		RowID BIGINT IDENTITY (1, 1) NOT NULL
	   ,Ukey BIGINT
	   ,SCIRefNo VARCHAR(30)
	   ,SuppID VARCHAR(6)
	   ,FabricPanelCode VARCHAR(2)
	   ,SizeItem VARCHAR(3)
	   ,SizeItem_Elastic VARCHAR(3)
	   ,ConsPC NUMERIC(8, 4)
	   ,Remark NVARCHAR(MAX)
	   ,BomTypeColor BIT
	   ,BomTypeSize BIT
	   ,BomTypeZipper BIT
	   ,BomTypeArticle BIT
	   ,BomTypeCOO BIT
	   ,BomTypeGender BIT
	   ,BomTypeCustomerSize BIT
	   ,BomTypeDecLabelSize BIT
	   ,BomTypeBrandFactoryCode BIT
	   ,BomTypeStyle BIT
	   ,BomTypeStyleLocation BIT
	   ,BomTypeSeason BIT
	   ,BomTypeCareCode BIT
	   ,BomTypePo BIT
       ,BomTypeBuyMonth BIT
       ,BomTypeBuyerDlvMonth BIT
	   ,BomTypeMatching BIT
	   ,Keyword VARCHAR(MAX)
	   ,IsCustCD NUMERIC(2, 0)
	   ,BomTypeCalculatePCS BIT
	   ,SizeItem_PCS VARCHAR(3)
	);
	DECLARE @BoaRowID INT;
	DECLARE @BoaRowCount INT;	--總資料筆數

	INSERT INTO @BoaCursor (Ukey, SCIRefNo, SuppID, FabricPanelCode, SizeItem, SizeItem_Elastic, ConsPC, Remark
	, BomTypeColor, BomTypeSize, BomTypeZipper, BomTypeArticle, BomTypeCOO, BomTypeGender, BomTypeCustomerSize
	, BomTypeDecLabelSize, BomTypeBrandFactoryCode, BomTypeStyle, BomTypeStyleLocation, BomTypeSeason, BomTypeCareCode, BomTypePo
    , BomTypeBuyMonth, BomTypeBuyerDlvMonth, BomTypeCalculatePCS, BomTypeMatching, Keyword, IsCustCD, SizeItem_PCS)
	SELECT
		Ukey
		,SCIRefNo
		,SuppID
		,FabricPanelCode
		,SizeItem
		,SizeItem_Elastic
		,ConsPC
		,Remark
		,BomTypeColor
		,BomTypeSize
		,BomTypeZipper
		,BomTypeArticle
		,BomTypeCOO
		,BomTypeGender
		,BomTypeCustomerSize
		,BomTypeDecLabelSize
		,BomTypeBrandFactoryCode
		,BomTypeStyle
		,BomTypeStyleLocation
		,BomTypeSeason
		,BomTypeCareCode
		,BomTypePo
        ,BomTypeBuyMonth
        ,BomTypeBuyerDlvMonth
		,BomTypeCalculatePCS
		,BomTypeMatching
		,Keyword
		,IsCustCD
		,SizeItem_PCS
	FROM Order_BOA
	WHERE ID = @ID
	AND (ISNULL(@Order_BOAUkey, 0) = 0
	OR Ukey = @Order_BOAUkey
	)
	AND SUBSTRING(Seq1, 1, 1) != '7'
	AND Refno != 'LABEL'
	ORDER BY Ukey;

	--取得此單的所有SP
	DECLARE @OrderList_Full VARCHAR(MAX) = (SELECT
			ID + ','
		FROM Orders
		WHERE POID = @ID
		ORDER BY ID
		FOR XML PATH (''))

	DECLARE @tmpA TABLE (
		RowID BIGINT
	   ,Seq VARCHAR(2)
	   ,UsageQty NUMERIC(6, 0)
	   ,IDX BIGINT
	);

	SET @BoaRowID = 1;
	SELECT
		@BoaRowID = MIN(RowID)
	   ,@BoaRowCount = MAX(RowID)
	FROM @BoaCursor;
	WHILE @BoaRowID <= @BoaRowCount
	BEGIN
		SELECT
			@BoaUkey = Ukey
		   ,@SCIRefNo = SCIRefNo
		   ,@BoaSuppID = SuppID
		   ,@BoaFabricPanelCode = FabricPanelCode
		   ,@BoaSizeItem = SizeItem
		   ,@BoaSizeItem_PCS = SizeItem_PCS
		   ,@BoaSizeItem_Elastic = SizeItem_Elastic
		   ,@BoaConsPC = ConsPC
		   ,@Remark = Remark

		   ,@BoaBomTypeColor = BomTypeColor
		   ,@BoaBomTypeSize = BomTypeSize
		   ,@BoaBomTypeZipper = BomTypeZipper
		   ,@BoaBomTypeArticle = BomTypeArticle
		   ,@BoaBomTypeCOO = BomTypeCOO
		   ,@BoaBomTypeGender = BomTypeGender
		   ,@BoaBomTypeCustomerSize = BomTypeCustomerSize
		   ,@BoaBomTypeDecLabelSize = BomTypeDecLabelSize
		   ,@BoaBomTypeBrandFactoryCode = BomTypeBrandFactoryCode
		   ,@BoaBomTypeStyle = BomTypeStyle
		   ,@BoaBomTypeStyleLocation = BomTypeStyleLocation
		   ,@BoaBomTypeSeason = BomTypeSeason
		   ,@BoaBomTypeCareCode = BomTypeCareCode
		   ,@BoaBomTypePo = BomTypePo
           ,@BomTypeBuyMonth = BomTypeBuyMonth
           ,@BomTypeBuyerDlvMonth = BomTypeBuyerDlvMonth

		   ,@BomTypeCalculatePCS = BomTypeCalculatePCS
		   ,@BomTypeMatching = BomTypeMatching
		   ,@Keyword = ISNULL(Keyword, '')
		   ,@BoaIsCustCD = IsCustCD
		FROM @BoaCursor
		WHERE RowID = @BoaRowID;

		--取得物料基本資料
		SELECT
			@RefNo = Refno
		   ,@MtlTypeID = MtltypeId
		   ,@UsageUnit = UsageUnit
		   ,@BomTypeCalculate = BomTypeCalculate
		   ,@BomTypeCalculateWeight = BomTypeCalculateWeight
		   ,@NoSizeUnit = NoSizeUnit
		FROM Production.dbo.Fabric
		WHERE SCIRefNo = @SCIRefNo;

		--取得SizeItem,當為Elastic，且SizeItem為S開頭時，改取SizeItem_Elastic
		SET @SizeItem = IIF(@BoaSizeItem_Elastic != '', @BoaSizeItem_Elastic, @BoaSizeItem);

		Declare @CalSizeItem varchar(3);
		set @CalSizeItem = IIF(@BomTypeCalculatePCS = 1, @BoaSizeItem_PCS, @SizeItem);

		--因有可能取不到資料,需先清除
        SET @tmpSizeUnit = null
		--取得SizeUnit
		SELECT
			@tmpSizeUnit = IIF(@BoaBomTypeSize = 1 and @NoSizeUnit = 0, Order_SizeItem.SizeUnit, null)
		FROM dbo.Order_SizeItem
		WHERE Order_SizeItem.ID = @ID
		AND Order_SizeItem.SizeItem = @CalSizeItem;

		--判斷是否有 For Article
		SET @BoaIsForArticle = 0
		If Exists (SELECT 1 FROM dbo.Order_BOA_Article WHERE Order_BOAUkey = @BoaUkey)
		BEGIN
			SET @BoaIsForArticle = 1;
		End;

		DELETE FROM @Sum_Qty;

		DELETE FROM @tmpTbl;

		INSERT INTO @tmpTbl
		SELECT
			tmpQtyBreakDown.ID
		   , ColorID = IIF(@BoaBomTypeColor = 1 OR @IsExpendDetail = 1, ISNULL(Order_ColorCombo.ColorID, ''), '')
		   , Article = IIF(@BoaBomTypeColor = 1 OR @IsExpendDetail = 1 OR (@IsExpendArticle = 1 and ExpendArticle = 1), tmpQtyBreakDown.Article, '')
		   , BomZipperInsert = IIF(@BoaBomTypeZipper = 1 OR @IsExpendDetail = 1, tmpQtyBreakDown.ZipperInsert, '')
		   , BomCustPONo = IIF(@BoaBomTypePo = 1 OR @IsExpendDetail = 1, iif(ky.Keyword like '%{PONo_Combine}%', '', tmpQtyBreakDown.CustPONo), '')
		   , SizeSeq = IIF(@BoaBomTypeSize = 1 OR @IsExpendDetail = 1, tmpQtyBreakDown.SizeSeq, '')
		   , SizeCode = tmpQtyBreakDown.SizeCode
		   , SizeSpec = IIF(@BoaBomTypeSize = 1 OR @IsExpendDetail = 1,
							IIF(ISNULL(tmpExist_SizeSpec_OrderCombo.IsExist, 0) = 1, ISNULL(tmpOrder_SizeSpec_OrderCombo.SizeSpec, ''), ISNULL(tmpOrder_SizeSpec.SizeSpec, ''))
							, '')
           , SizeUnit = isnull(@tmpSizeUnit, '')
		   , OrderQty = tmpQtyBreakDown.Qty
		   , UsageQty = (Qty * IIF(ISNULL(@CalSizeItem, '') = '', 1,
									ISNULL(IIF(ISNULL(tmpExist_SizeSpec_OrderCombo.IsExist, 0) = 1, tmpOrder_SizeSpec_OrderCombo.SizeSpec_Cal, tmpOrder_SizeSpec.SizeSpec_Cal),
											IIF(@BomTypeCalculate = 1 OR @BomTypeCalculateWeight = 1 or @BomTypeCalculatePCS = 1, 0, 1))))
						* iif(@BomTypeCalculatePCS = 1, 1, @BoaConsPC)
		   , Keyword = isnull(ky.Keyword, '')
		   , Keyword_Original = @Keyword
		   , Keyword_xml = isnull(ky.Keyword_xml, '')
		   , Special = ''
           
		   /* BomTypeValue Start */
		   --與ColorID相同
           , BomTypeColorID = IIF(@BoaBomTypeColor = 1, Order_ColorCombo.ColorID, null)
		   --與SizeSpec相同
           , BomTypeSize = IIF(@BoaBomTypeSize = 1,
                            IIF(ISNULL(tmpExist_SizeSpec_OrderCombo.IsExist, 0) = 1, tmpOrder_SizeSpec_OrderCombo.SizeSpec, tmpOrder_SizeSpec.SizeSpec)
							, null)
           , BomTypeSizeUnit = IIF(@BoaBomTypeSize = 1 and @NoSizeUnit = 0, @tmpSizeUnit, null)
		   --與BomZipperInsert相同
           , BomTypeZipperInsert = IIF(@BoaBomTypeZipper = 1, tmpQtyBreakDown.ZipperInsert, null)
		    --與Article相同
           , BomTypeArticle = IIF(@BoaBomTypeArticle = 1, tmpQtyBreakDown.Article, null)
           , BomTypeCOO = IIF(@BoaBomTypeCOO = 1, dbo.GetBomTypeValue(@BoaUkey, 'COO', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeGender = IIF(@BoaBomTypeGender = 1, dbo.GetBomTypeValue(@BoaUkey, 'Gender', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeCustomerSize = IIF(@BoaBomTypeCustomerSize = 1, dbo.GetBomTypeValue(@BoaUkey, 'CustomerSize', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeDecLabelSize = IIF(@BoaBomTypeDecLabelSize = 1, dbo.GetBomTypeValue(@BoaUkey, 'DecLabelSize', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeBrandFactoryCode = IIF(@BoaBomTypeBrandFactoryCode = 1, dbo.GetBomTypeValue(@BoaUkey, 'BrandFactoryCode', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeStyle = IIF(@BoaBomTypeStyle = 1, dbo.GetBomTypeValue(@BoaUkey, 'Style', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeStyleLocation = IIF(@BoaBomTypeStyleLocation = 1, dbo.GetBomTypeValue(@BoaUkey, 'StyleLocation', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeSeason = IIF(@BoaBomTypeSeason = 1, dbo.GetBomTypeValue(@BoaUkey, 'Season', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeCareCode = IIF(@BoaBomTypeCareCode = 1, dbo.GetBomTypeValue(@BoaUkey, 'CareCode', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
		   --與BomCustPONo相同
		   , BomTypeCustomerPO = IIF(@BoaBomTypePo = 1, iif(Keyword like '%{PONo_Combine}%', '', tmpQtyBreakDown.CustPONo), null)
		   , BomTypeBuyMonth = IIF(@BomTypeBuyMonth = 1, dbo.GetBomTypeValue(@BoaUkey, 'BuyMonth', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
           , BomTypeBuyerDlvMonth = IIF(@BomTypeBuyerDlvMonth = 1, dbo.GetBomTypeValue(@BoaUkey, 'BuyerDlvMonth', ky.Location, tmpQtyBreakDown.SizeCode, tmpQtyBreakDown.ID), null)
		   /* BomTypeValue End */
           
		FROM @tmpOrder_Qty AS tmpQtyBreakDown
		LEFT JOIN dbo.Order_ColorCombo
			ON Order_ColorCombo.ID = @ID
				AND Order_ColorCombo.Article = tmpQtyBreakDown.Article
				AND Order_ColorCombo.FabricPanelCode = @BoaFabricPanelCode
		outer apply (SELECT ExpendArticle = iif((@MtlTypeID = 'STICKER' AND @Category = 'B') OR @Category = 'M', 1, 0)) expArt
		/*
		Left Join (Select ID, SizeItem, SizeCode, SizeSpec
					  , IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' Or @UsageUnit = 'INCH', Production.dbo.GetDigitalValue(SizeSpec), 0), 1) as SizeSpec_Cal
				   From dbo.Order_SizeSpec ) tmpOrder_SizeSpec
		On tmpOrder_SizeSpec.ID = tmpQtyBreakDown.OrderComboID
		   And tmpOrder_SizeSpec.SizeItem = @SizeItem
		   And tmpOrder_SizeSpec.SizeCode = tmpQtyBreakDown.SizeCode
		*/
		OUTER APPLY (
			SELECT ID
			   , SizeItem
			   , SizeCode
			   , SizeSpec
			   , getCal.SizeSpec_Cal
			FROM dbo.Order_SizeSpec
			OUTER APPLY (
				SELECT IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' OR @UsageUnit = 'INCH',
				Production.dbo.GetUnitQty(Order_SizeItem.SizeUnit, @UsageUnit, Production.dbo.GetDigitalValue(tmpSS.SizeSpec)), 0),
				IIF(@BomTypeCalculateWeight = 1 AND @UsageUnit = 'G',
				Production.dbo.GetUnitQty(Order_SizeItem.SizeUnit, @UsageUnit, Production.dbo.GetDigitalValue(tmpSS.SizeSpec)),
				IIF(@BomTypeCalculatePCS = 1,
				Production.dbo.GetUnitQty(@UsageUnit, @UsageUnit, Production.dbo.GetDigitalValue(tmpSS.SizeSpec)), 1))) AS SizeSpec_Cal
				FROM dbo.Order_SizeSpec tmpSS			
				INNER JOIN dbo.Order_SizeItem
					ON tmpSS.ID = Order_SizeItem.ID
					AND tmpSS.SizeItem = Order_SizeItem.SizeItem
				WHERE tmpSS.ID = tmpQtyBreakDown.POID
				AND tmpSS.SizeItem = @CalSizeItem
				AND tmpSS.SizeCode = tmpQtyBreakDown.SizeCode
			) getCal
			WHERE Order_SizeSpec.ID = tmpQtyBreakDown.POID
			AND Order_SizeSpec.SizeItem = @CalSizeItem
			AND Order_SizeSpec.SizeCode = tmpQtyBreakDown.SizeCode
		) AS tmpOrder_SizeSpec
		-----------------------------------------------------------------------------------------------------
		--2017/09/14 add by Ben, 當by OrderCombo設定SizeSpec時，一律抓取該OrderCombo的設定進行Expand
		OUTER APPLY (SELECT
				IIF(EXISTS (SELECT TOP 1
						1
					FROM dbo.Order_SizeSpec_OrderCombo
					WHERE Order_SizeSpec_OrderCombo.ID = tmpQtyBreakDown.POID
					AND Order_SizeSpec_OrderCombo.OrderComboID = tmpQtyBreakDown.OrderComboID
					AND Order_SizeSpec_OrderCombo.SizeItem = @CalSizeItem)
				, 1, 0) AS IsExist) AS tmpExist_SizeSpec_OrderCombo
		-----------------------------------------------------------------------------------------------------
		OUTER APPLY (
			SELECT ID
			   , SizeItem
			   , SizeCode
			   , SizeSpec
			   , getCal.SizeSpec_Cal
			FROM dbo.Order_SizeSpec_OrderCombo
			OUTER APPLY (
				SELECT IIF(@BomTypeCalculate = 1, IIF(@UsageUnit = 'CM' OR @UsageUnit = 'INCH',
				Production.dbo.GetUnitQty(Order_SizeItem.SizeUnit, @UsageUnit, Production.dbo.GetDigitalValue(tmpSS.SizeSpec)), 0),
				IIF(@BomTypeCalculateWeight = 1 AND @UsageUnit = 'G',
				Production.dbo.GetUnitQty(Order_SizeItem.SizeUnit, @UsageUnit, Production.dbo.GetDigitalValue(tmpSS.SizeSpec)),
				IIF(@BomTypeCalculatePCS = 1,
				Production.dbo.GetUnitQty(@UsageUnit, @UsageUnit, Production.dbo.GetDigitalValue(tmpSS.SizeSpec)), 1))) AS SizeSpec_Cal
				FROM dbo.Order_SizeSpec_OrderCombo tmpSS
				INNER JOIN dbo.Order_SizeItem
					ON tmpSS.ID = Order_SizeItem.ID
					AND tmpSS.SizeItem = Order_SizeItem.SizeItem
				WHERE tmpSS.ID = tmpQtyBreakDown.POID
				AND tmpSS.OrderComboID = tmpQtyBreakDown.OrderComboID
				AND tmpSS.SizeItem = @CalSizeItem
				AND tmpSS.SizeCode = tmpQtyBreakDown.SizeCode
			) getCal
			WHERE Order_SizeSpec_OrderCombo.ID = tmpQtyBreakDown.POID
			AND Order_SizeSpec_OrderCombo.OrderComboID = tmpQtyBreakDown.OrderComboID
			AND Order_SizeSpec_OrderCombo.SizeItem = @CalSizeItem
			AND Order_SizeSpec_OrderCombo.SizeCode = tmpQtyBreakDown.SizeCode) AS tmpOrder_SizeSpec_OrderCombo
		OUTER APPLY (SELECT TOP 1
				SCIRefNo
			FROM dbo.Order_BOA_CustCD
			WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
			AND CustCDID = tmpQtyBreakDown.CustCDID) AS tmpReplaceSciRefNo
		OUTER APPLY (
			SELECT DISTINCT Keyword = dbo.GetKeyword_New(tmpQtyBreakDown.ID, @BoaUkey, @Keyword, tmpQtyBreakDown.Article, tmpQtyBreakDown.SizeCode, sl.Location, 1)
			, Keyword_xml = dbo.GetKeyword_New(tmpQtyBreakDown.ID, @BoaUkey, @Keyword, tmpQtyBreakDown.Article, tmpQtyBreakDown.SizeCode, sl.Location, 2)
			, Location = iif(@Keyword like '%{Location}%', sl.Location, '')
			FROM Production.dbo.Style_Location sl
			WHERE sl.StyleUkey = @StyleUkey
			and (not exists(select 1 from dbo.Order_BOA_Location ol with (nolock) where ol.Order_BOAUkey = @BoaUkey)
				or exists(select 1 from dbo.Order_BOA_Location ol with (nolock) where ol.Order_BOAUkey = @BoaUkey and ol.Location = sl.Location))
		) ky
		WHERE
		--排除For Article
		(@BoaIsForArticle = 0
			OR (@BoaIsForArticle = 1
				AND EXISTS (
					SELECT 1
					FROM dbo.Order_BOA_Article
					WHERE Order_BOAUkey = @BoaUkey
					AND Article = tmpQtyBreakDown.Article))
		)
		--符合CustCD設定，包含設定為1 = 全部
		AND ((@BoaIsCustCD = 0
			OR @BoaIsCustCD = 1)
			OR ((@BoaIsCustCD = 2)
			AND (EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.CustCDID)
			))
			OR ((@BoaIsCustCD = 3)
			AND (NOT EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.CustCDID)
			))
			OR ((@BoaIsCustCD = 4)
			AND (EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.CustCDCountryID)
			))
			OR ((@BoaIsCustCD = 5)
			AND (NOT EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.CustCDCountryID)
			))
			OR ((@BoaIsCustCD = 6)
			AND (EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.Kit)
			))
			OR ((@BoaIsCustCD = 7)
			AND (NOT EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.Kit)
			))
			OR ((@BoaIsCustCD = 8)
			AND (EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.CustPONo)
			))
			OR ((@BoaIsCustCD = 9)
			AND (NOT EXISTS (SELECT TOP 1
					1
				FROM dbo.Order_BOA_CustCD
				WHERE Order_BOA_CustCD.Order_BOAUkey = @BoaUkey
				AND Order_BOA_CustCD.ColumnValue = tmpQtyBreakDown.CustPONo)
			))
		)
		AND ((@BoaBomTypeColor = 0)
			OR ((@BoaBomTypeColor = 1 OR @IsExpendDetail = 1)
				AND ISNULL(Order_ColorCombo.ColorID, '') <> ''))
		AND ((@BoaBomTypeSize = 0)
			OR ((@BoaBomTypeSize = 1 OR @IsExpendDetail = 1)
				AND IIF(ISNULL(tmpExist_SizeSpec_OrderCombo.IsExist, 0) = 1, ISNULL(tmpOrder_SizeSpec_OrderCombo.SizeSpec, ''), ISNULL(tmpOrder_SizeSpec.SizeSpec, '')) != ''))
                
		IF @BomTypeMatching = 1
		BEGIN
			DELETE FROM @tmpA

			INSERT INTO @tmpA
			SELECT tmpTbl.RowID
			, Seq
			, T.UsageQty
			, ROW_NUMBER() over(partition by Seq, BomCustPONo order by t.UsageQty desc)
			FROM @tmpTbl tmpTbl
			OUTER APPLY (
				SELECT MatchingQty = SUM(getMatching.Qty)
				, MatchingSizeCode = getMatching.SizeCode
				, getMatching.Seq
				, getMatching.SizeCodeSeq
				FROM @tmpTbl tmpA
				INNER JOIN Order_BOA_Matching_Detail matching ON matching.ID = @ID AND matching.Order_BOAUkey = @BoaUkey AND matching.SizeCode = tmpA.SizeCode		
				OUTER APPLY (
					SELECT Qty = CEILING(tmpA.OrderQty / MatchingRatio)
					, SizeCode = md.SizeCode
					, Seq = md.Seq
					, SizeCodeSeq = sc.Seq
					FROM Order_BOA_Matching_Detail md
					INNER JOIN Order_SizeCode sc ON sc.ID = md.ID AND sc.SizeCode = md.SizeCode
					WHERE md.ID = matching.ID AND md.Order_BOAUkey = matching.Order_BOAUkey AND md.Seq = matching.Seq
				) getMatching
				WHERE tmpA.SizeCode = tmpTbl.SizeCode and tmpA.BomCustPONo = tmpTbl.BomCustPONo
				GROUP BY getMatching.SizeCode, getMatching.Seq, getMatching.SizeCodeSeq
			) getMatchingQty
			OUTER APPLY (SELECT TOP 1 RowID FROM @tmpTbl WHERE SizeCode = tmpTbl.SizeCode and OrderQty > 0 and BomCustPONo = tmpTbl.BomCustPONo ORDER BY RowID DESC) getMaxRowIDbySizeCode
			OUTER APPLY (SELECT UsageQty = IIF(@BomTypeMatching = 1, IIF(tmpTbl.SizeCode = getMatchingQty.MatchingSizeCode and getMaxRowIDbySizeCode.RowID = tmpTbl.RowID, getMatchingQty.MatchingQty * @BoaConsPC , 0), UsageQty)) T
			WHERE tmpTbl.SizeCode = MatchingSizeCode

			UPDATE @tmpA SET UsageQty = 0 WHERE IDX != 1

			UPDATE tmpTbl SET UsageQty = isnull(tmp.UsageQty, 0)
			FROM @tmpTbl tmpTbl
			OUTER APPLY (SELECT * FROM @tmpA tmpA WHERE tmpTbl.RowID = tmpA.RowID) tmp
		END
        
		UPDATE @tmpTbl SET SizeCode = IIF(@BoaBomTypeSize = 1 OR @IsExpendDetail = 1, SizeCode, '')
        
		INSERT INTO @Sum_Qty (ColorID, Article, BomZipperInsert, BomCustPONo
		, SizeSeq, SizeCode, SizeSpec, SizeUnit, Keyword, Keyword_Original, Keyword_xml, Special
		, OrderQty, UsageQty, OrderList		
		, BomTypeColorID, BomTypeSize, BomTypeSizeUnit, BomTypeZipperInsert, BomTypeArticle, BomTypeCOO, BomTypeGender, BomTypeCustomerSize
        , BomTypeDecLabelSize, BomTypeBrandFactoryCode, BomTypeStyle, BomTypeStyleLocation, BomTypeSeason, BomTypeCareCode, BomTypeCustomerPO
        , BomTypeBuyMonth, BomTypeBuyerDlvMonth)
		SELECT ColorID, Article, BomZipperInsert, BomCustPONo
		, SizeSeq, SizeCode, SizeSpec, SizeUnit, Keyword, Keyword_Original, Keyword_xml, Special
		, SUM(OrderQty), SUM(UsageQty), IIF(VasShas.OrderList IS NULL, tmp2.OrderList, VasShas.OrderList)		
		, BomTypeColorID, BomTypeSize, BomTypeSizeUnit, BomTypeZipperInsert, BomTypeArticle, BomTypeCOO, BomTypeGender, BomTypeCustomerSize
		, BomTypeDecLabelSize, BomTypeBrandFactoryCode, BomTypeStyle, BomTypeStyleLocation, BomTypeSeason, BomTypeCareCode, BomTypeCustomerPO
        , BomTypeBuyMonth, BomTypeBuyerDlvMonth
		FROM @tmpTbl tmp1
		CROSS APPLY (
			SELECT Orderlist = (
				SELECT DISTINCT	ID + ','
				FROM @tmpTbl AS tmp3
				WHERE tmp3.ColorID = tmp1.ColorID
					AND ISNULL(tmp3.Article, '') = ISNULL(tmp1.Article, '')
					AND ISNULL(tmp3.BomZipperInsert, '') = ISNULL(tmp1.BomZipperInsert, '')
					AND ISNULL(tmp3.BomCustPONo, '') = ISNULL(tmp1.BomCustPONo, '')
					AND ISNULL(tmp3.SizeSeq, '') = ISNULL(tmp1.SizeSeq, '')
					AND ISNULL(tmp3.SizeCode, '') = ISNULL(tmp1.SizeCode, '')
					AND ISNULL(tmp3.SizeSpec, '') = ISNULL(tmp1.SizeSpec, '')
					AND ISNULL(tmp3.SizeUnit, '') = ISNULL(tmp1.SizeUnit, '')
					AND ISNULL(tmp3.Keyword, '') = ISNULL(tmp1.Keyword, '')
					AND tmp3.OrderQty > 0
				FOR XML PATH (''))
		) AS tmp2
		OUTER APPLY (
			SELECT distinct ID
			FROM Order_Label_Detail
			WHERE Order_BOAUkey = @BoaUkey
		) FromVasShas
		OUTER APPLY (
			SELECT OrderList = (
				SELECT Distinct ID + ','
				FROM Order_Label_Detail
				WHERE Order_BOAUkey = @BoaUkey
				FOR XML PATH (''))
		) VasShas
		WHERE ((@IncludeQtyZero = 0 AND OrderQty > 0) OR @IncludeQtyZero = 1)
			AND (FromVasShas.ID IS NULL	OR (FromVasShas.ID = tmp1.ID))
		GROUP BY ColorID, Article, BomZipperInsert, BomCustPONo
		, SizeSeq, SizeCode, SizeSpec, SizeUnit, Keyword, Keyword_Original, Keyword_xml, Special
		, tmp2.OrderList, VasShas.OrderList
		, BomTypeColorID, BomTypeSize, BomTypeSizeUnit, BomTypeZipperInsert, BomTypeArticle, BomTypeCOO, BomTypeGender, BomTypeCustomerSize
		, BomTypeDecLabelSize, BomTypeBrandFactoryCode, BomTypeStyle, BomTypeStyleLocation, BomTypeSeason, BomTypeCareCode, BomTypeCustomerPO
        , BomTypeBuyMonth, BomTypeBuyerDlvMonth

		ORDER BY BomZipperInsert, BomCustPONo
		, Article, ColorID, SizeSeq, SizeCode, SizeSpec;
        
		INSERT INTO @Tmp_BoaExpend (ID, Order_BOAUkey, RefNo, SCIRefNo, Article, ColorID, SuppColor
		, SizeSeq, SizeCode, SizeSpec, SizeUnit, Remark, OrderQty
        --, Price
        , UsageQty
		, UsageUnit, SysUsageQty, BomZipperInsert, BomCustPONo, Keyword, Keyword_Original, Keyword_xml, OrderList, ColorDesc, Special
		, BomTypeColorID, BomTypeSize, BomTypeSizeUnit, BomTypeZipperInsert, BomTypeArticle, BomTypeCOO, BomTypeGender, BomTypeCustomerSize
		, BomTypeDecLabelSize, BomTypeBrandFactoryCode, BomTypeStyle, BomTypeStyleLocation, BomTypeSeason, BomTypeCareCode, BomTypeCustomerPO
        , BomTypeBuyMonth, BomTypeBuyerDlvMonth
		)
		SELECT @ID, @BoaUkey, @RefNo, @SCIRefNo, Sum_Qty.Article, Sum_Qty.ColorID, tmpSuppColor.SuppColor
		, Sum_Qty.SizeSeq, Sum_Qty.SizeCode, Sum_Qty.SizeSpec, Sum_Qty.SizeUnit, @Remark, Sum_Qty.OrderQty
        --, IIF(@IsGetFabQuot = 1, tmpPrice.Price, 0) AS Price
        , Sum_Qty.UsageQty
		, @UsageUnit, Sum_Qty.UsageQty, Sum_Qty.BomZipperInsert, BomCustPONo = tmpBomCustPONo.BomCustPONo, ReplaceKeyword.Keyword, Sum_Qty.Keyword_Original, ReplaceKeyword.Keyword_xml, Sum_Qty.OrderList, Color.Name, Special
		, BomTypeColorID, BomTypeSize, BomTypeSizeUnit, BomTypeZipperInsert, BomTypeArticle, BomTypeCOO, BomTypeGender, BomTypeCustomerSize
		, BomTypeDecLabelSize, BomTypeBrandFactoryCode, BomTypeStyle, BomTypeStyleLocation, BomTypeSeason, BomTypeCareCode, BomTypeCustomerPO
        , BomTypeBuyMonth, BomTypeBuyerDlvMonth
		FROM @Sum_Qty AS Sum_Qty
		LEFT JOIN Production.dbo.Color ON Color.BrandID = @BrandID AND Color.ID = Sum_Qty.ColorID
		OUTER APPLY (
			SELECT SuppColor = ISNULL(Production.dbo.GetSuppColorList(@SCIRefNo, @BoaSuppID, Sum_Qty.ColorID, @BrandID, @SeasonID, @ProgramID, @StyleID), '')
		) AS tmpSuppColor
		--OUTER APPLY (
		--	SELECT Price = ISNULL(Production.dbo.GetPriceFromMtl(@SCIRefNo, @BoaSuppID, @SeasonID, Sum_Qty.UsageQty, @Category, @CfmDate, '', @ColorID, @FactoryID), 0)
		--) AS tmpPrice				
		OUTER APPLY (
			SELECT Value = Replace(
				STUFF((
					select distinct ',' + CustPONo
					from Orders
					where ID in (select Data from dbo.SplitString(Sum_Qty.OrderList, ',') where Data != '')
					FOR XML PATH ('')), 1, 1, '')
			, ',', Char(13) + Char(10))
		) CustPonoList
		OUTER APPLY (
			-- Keyword 'PONo_Combine' 因需依照OrderList組合CustPono, 額外在此處理
			Select Keyword = IIF(Sum_Qty.Keyword like '%{PONo_Combine}%', Replace(Sum_Qty.Keyword, '{PONo_Combine}', CustPonoList.Value), Sum_Qty.Keyword)
			, Keyword_xml = Sum_Qty.Keyword_xml + IIF(Sum_Qty.Keyword like '%{PONo_Combine}%', '<row><KeywordField>PONo_Combine</KeywordField><KeywordValue>' + CustPonoList.Value + '</KeywordValue></row>', '')
		) ReplaceKeyword
		OUTER APPLY (
			Select BomCustPONo = iif(Sum_Qty.Keyword like '%{PONo_Combine}%', SUBSTRING(cast(CustPonoList.Value as varchar), 1, 50), Sum_Qty.BomTypeCustomerPO)
		) tmpBomCustPONo

		SET @BoaRowID += 1
	End;

	--Update OrderList 包含整組的sp，改為空白
	UPDATE @Tmp_BoaExpend
	SET OrderList = ''
	WHERE OrderList = @OrderList_Full

RETURN;
END
