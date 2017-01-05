

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
		, @StockSP = isnull(concat(p.StockPOID,' ',p.StockSeq1,' ',p.StockSeq2),'')
		, @po_desc=@po_desc + ISNULL(p.ColorDetail,'')+ CHAR(13)--+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.sizespec,'')+ CHAR(13)--+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.SizeUnit,'')+ CHAR(13)--+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.Special,'')+ CHAR(13)--+CHAR(10)
		, @po_desc=@po_desc + ISNULL(p.Remark,'')
		from dbo.po_supp_detail p WHERE ID=@poid and seq1 = @seq1 and seq2=@seq2;

	IF  @type = 1
	BEGIN
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric where SCIRefno = @scirefno;
			set @string = rtrim(iif(@fabric_detaildesc='','',@fabric_detaildesc + CHAR(13)));
		END
		ELSE
			set @string = rtrim(iif(@suppcolor = '','',@suppcolor + CHAR(13)))+rtrim(iif(@po_desc = '','',@po_desc+CHAR(13)));
	END

	IF @type =2
	BEGIN
		if @repeat = 0
		BEGIN	
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric where SCIRefno = @scirefno;
			set @string = rtrim(iif(@fabric_detaildesc='','',@fabric_detaildesc + CHAR(13)))+ rtrim(iif(@suppcolor = '','',@suppcolor + CHAR(13)))+rtrim(iif(@po_desc = '','',@po_desc+CHAR(13)));
		END
		ELSE
			set @string = rtrim(iif(@suppcolor = '','',@suppcolor + CHAR(13)))+rtrim(iif(@po_desc = '','',@po_desc+CHAR(13)));
	END

	IF @type =4
	BEGIN	
		
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL([Description],'') from fabric where SCIRefno = @scirefno;
			set @string = 'Ref#'+ @refno + ', ' + rtrim(iif(@fabric_detaildesc='','',@fabric_detaildesc + CHAR(13))) + rtrim(iif(@suppcolor = '','',@suppcolor + CHAR(13)))+rtrim(iif(@po_desc = '','',@po_desc+CHAR(13)));
		End
		ELSE
			set @string = rtrim(iif(@suppcolor = '','',@suppcolor + CHAR(13)))+rtrim(iif(@po_desc = '','',@po_desc+CHAR(13)));
	END

	IF left(@SEQ1,1) = '7'
	BEGIN
		SET @string = '**PLS USE STOCK FROM SP#:' + iif(@StockSP='','',@StockSP) + '**' + CHAR(13) + @string;
	END 

    RETURN rtrim(@string)
END
