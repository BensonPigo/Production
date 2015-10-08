

-- 建立 FUNCTION
CREATE FUNCTION [dbo].[getMtlDesc](@poid varchar(13),@seq1 varchar(3),@seq2 varchar(2),@type int,@repeat bit) 
RETURNS nvarchar(max)  -- 回傳Description
BEGIN
	DECLARE @fabric_detaildesc nvarchar(max); -- 暫存fabric description
	DECLARE @suppcolor nvarchar(max); -- 暫存 supplier color
	DECLARE @po_desc nvarchar(max); -- 暫存 po_supp_detail 相關欄位
    DECLARE @string nvarchar(max)  -- 最後回傳的字串
	DECLARE @StockSP VARCHAR(18)	-- 暫存po_supp_detail.stockpoid
	DECLARE @scirefno varchar(26)	-- 暫存po_supp_detail.scirefno
	DECLARE @refno varchar(23)		-- 暫存po_supp_detail.refno

    SET @string = ''
	SET @po_desc=''

	SELECT @scirefno=p.SCIRefno
		, @refno = p.Refno
		, @suppcolor = ISNULL(p.SuppColor,'')
		, @StockSP = isnull(p.StockPOID,'')
		, @po_desc=@po_desc + ISNULL(p.ColorDetail,'')+ CHAR(13)+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.sizespec,'')+ CHAR(13)+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.SizeUnit,'')+ CHAR(13)+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.Special,'')+ CHAR(13)+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.Remark,'')
		from dbo.po_supp_detail p WHERE ID=@poid and seq1 = @seq1 and seq2=@seq2;

	IF @scirefno is null or @scirefno = ''
	BEGIN
		SELECT @scirefno=p.SCIRefno
		, @po_desc=@po_desc + ISNULL(p.sizespec,'')
		, @po_desc=@po_desc + ISNULL(p.Special,'')
		, @po_desc=@po_desc + ISNULL(p.Remark,'')
			from dbo.PO_Artwork p WHERE ID=@poid and seq1 = @seq1 and seq2=@seq2;
	END

	IF  @type = 1
	BEGIN
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric where SCIRefno = @scirefno;
			set @string = rtrim(ISNULL(@fabric_detaildesc,''))+CHAR(13)+CHAR(10);
		END
		ELSE
			set @string = rtrim(ISNULL(@suppcolor,''))+ CHAR(13)+CHAR(10)+rtrim(ISNULL(@po_desc,''));
	END

	IF @type =2
	BEGIN
		if @repeat = 0
		BEGIN	
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric where SCIRefno = @scirefno;
			set @string = rtrim(ISNULL(@fabric_detaildesc,''))+CHAR(13)+CHAR(10)+ rtrim(ISNULL(@suppcolor,''))+rtrim(ISNULL(@po_desc,''));
		END
		ELSE
			set @string = rtrim(ISNULL(@suppcolor,''))+ CHAR(13)+CHAR(10)+rtrim(ISNULL(@po_desc,''));
	END

	IF @type =4
	BEGIN	
		
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL([Description],'') from fabric where SCIRefno = @scirefno;
			set @string = 'Ref#'+ @refno + ', ' + rtrim(ISNULL(@fabric_detaildesc,''))+ CHAR(13)+CHAR(10) + rtrim(ISNULL(@suppcolor,''))+rtrim(ISNULL(@po_desc,''));
		End
		ELSE
			set @string = rtrim(ISNULL(@suppcolor,''))+ CHAR(13)+CHAR(10)+rtrim(ISNULL(@po_desc,''));
	END

	IF left(@SEQ1,1) = '7'
	BEGIN
		SET @string = '**PLS USE STOCK FROM SP#:' + ISNULL(@StockSP,'') + '**' + CHAR(13)+CHAR(10) + @string;
	END 

    RETURN @string
END
