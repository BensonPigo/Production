

-- 建立 FUNCTION
CREATE FUNCTION [dbo].[getMtlDesc](@poid varchar(13),@seq1 varchar(3),@seq2 varchar(2),@type int,@repeat bit) 
RETURNS nvarchar(max)  -- 回傳Description
BEGIN
	DECLARE @fabric_detaildesc nvarchar(max); -- 暫存fabric description
	DECLARE @suppcolor nvarchar(max); -- 暫存 supplier color
	DECLARE @po_desc nvarchar(max); -- 暫存 po_supp_detail 相關欄位
    DECLARE @string nvarchar(max)  -- 最後回傳的字串
	DECLARE @StockSP VARCHAR(25)	-- 暫存po_supp_detail.stockpoid
	DECLARE @scirefno varchar(35)	-- 暫存po_supp_detail.scirefno
	DECLARE @refno varchar(35)		-- 暫存po_supp_detail.refno
	Declare @Spec VarChar(max);
	Declare @BomZipperInsert VarChar(5);
	Declare @ZipperName NVarChar(500);

    SET @string = ''
	SET @po_desc=''

	SELECT @scirefno=p.SCIRefno
		, @refno = p.Refno
		, @suppcolor = Concat(iif(ISNULL(p.SuppColor,'') = '', '', p.SuppColor + CHAR(10)) 
							  , iif(ISNULL(p.ColorID,'') = '', '', p.ColorID + ' - ') + ISNULL(c.Name, ''))
		, @StockSP = isnull(concat(p.StockPOID,' ',p.StockSeq1,' ',p.StockSeq2),'')
		, @po_desc=@po_desc + iif(ISNULL(p.ColorDetail,'') = '', '', 'ColorDetail : ' + p.ColorDetail + CHAR(10))
		, @po_desc=@po_desc + iif(ISNULL(p.sizespec,'') = '', '', p.sizespec + ' ')
		, @po_desc=@po_desc + iif(ISNULL(p.SizeUnit,'') = '', '', p.SizeUnit) + iif(p.sizespec = '', '', CHAR(10))
		, @po_desc=@po_desc + iif(ISNULL(p.Special,'') = '', '', p.Special + CHAR(10))
		, @po_desc=@po_desc + iif(ISNULL(p.Spec,'') = '', '', p.Spec + CHAR(10))
		, @po_desc=@po_desc + ISNULL(p.Remark,'')
		, @Spec = stockPO3.Spec
		, @BomZipperInsert= stockPO3.BomZipperInsert 
		from dbo.po_supp_detail p WITH (NOLOCK)
		left join fabric f WITH (NOLOCK) on p.SCIRefno = f.SCIRefno
		left join Color c WITH (NOLOCK) on f.BrandID = c.BrandId and p.ColorID = c.ID 
		 outer apply ( select Spec, BomZipperInsert from PO_Supp_Detail tmpPO3
        where tmpPO3.ID = IIF(IsNull(p.StockPOID, '') = '' , p.ID, p.StockPOID)
			and tmpPO3.Seq1 = IIF(IsNull(p.StockPOID, '') = '' , p.Seq1, p.StockSeq1)
			and tmpPO3.Seq2 = IIF(IsNull(p.StockPOID, '') = '' , p.Seq2, p.StockSeq2)
			) stockPO3
		WHERE p.ID=@poid and seq1 = @seq1 and seq2=@seq2;

	IF  @type = 1
	BEGIN
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric WITH (NOLOCK) where SCIRefno = @scirefno;
			set @string = rtrim(iif(isnull(@fabric_detaildesc, '') = '','',@fabric_detaildesc + CHAR(13)+ CHAR(10)));
		END
		ELSE
			set @string = rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor + CHAR(13)+ CHAR(10)))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc+CHAR(13)+ CHAR(10)));
	END

	IF @type =2
	BEGIN
		if @repeat = 0
		BEGIN	
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric WITH (NOLOCK) where SCIRefno = @scirefno;
			set @string = concat(rtrim(iif(isnull(@fabric_detaildesc, '') = '','',iif(@suppcolor='',@fabric_detaildesc,@fabric_detaildesc + CHAR(10))))
								, rtrim(iif(isnull(@suppcolor, '') = '','',iif(@po_desc='',@suppcolor,@suppcolor + CHAR(10))))
								, rtrim(iif(isnull(@po_desc, '') = '','',replace(@po_desc,char(10),'') )));
		END
		ELSE
		set @string = rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor ))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc ));
	END

	IF @type =4
	BEGIN	
		
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL([Description],'') from fabric WITH (NOLOCK) where SCIRefno = @scirefno;
			set @string = 'Ref#'+ @refno + ', ' + rtrim(iif(isnull(@fabric_detaildesc, '') = '','',@fabric_detaildesc + CHAR(13)+ CHAR(10))) + rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor + CHAR(13)+ CHAR(10)))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc+CHAR(13)+ CHAR(10)));
		End
		ELSE
			set @string = rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor + CHAR(13) + CHAR(10)))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc+CHAR(13) + CHAR(10)));
	END

	IF left(@SEQ1,1) = '7'
	BEGIN
		SET @string = concat('**PLS USE STOCK FROM SP#:' + iif(isnull(@StockSP, '') = '','',@StockSP) + '**' 
							, CHAR(13) , CHAR(10)
							, isnull(@string, ''));
	END 

	------------------------
	--增加Zipper 
	Set @ZipperName = '';
	Select @ZipperName = DropDownList.Name
	  From Production.dbo.DropDownList
	 Where Type = 'Zipper'
	   And ID = @BomZipperInsert;
	
	Set @string += IIF(IsNull(@string,'') = '', '', char(13)+char(10)  + IIF(IsNull(@ZipperName, '') = '', '', 'Spec:'+ @ZipperName + Char(13) + Char(10))+ RTrim(@Spec));
	----------------------------



	-- @fabric_detaildesc 去除結尾過多的 Enter
	set @string = replace(@string, char(10), char(13));
	while (CHARINDEX(char(13) + char(13), @string, 0) > 0)
	Begin
		set @string = replace(@string, char(13) + char(13), char(13));
	end
	set @string = replace(@string, char(13), char(13) + char(10));
	-- end
    RETURN ltrim(rtrim(@string))
END
