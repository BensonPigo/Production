﻿

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
	DECLARE @refno varchar(36)		-- 暫存po_supp_detail.refno
	Declare @Spec VarChar(max);
	Declare @BomZipperInsert VarChar(5);
	Declare @ZipperName NVarChar(500);
	Declare @POSpec nvarchar(max);

    SET @string = ''
	SET @po_desc=''

	SELECT @scirefno=p.SCIRefno
		, @refno = p.Refno
		, @suppcolor = CASE WHEN f.MtlTypeID='SP THREAD' AND ThreadColor.SuppColor IS NOT NULL  --繡線顏色可能要從另外的項次號取
							THEN Concat(iif(ISNULL(ThreadColor.SuppColor,'') = '', '', ThreadColor.SuppColor + CHAR(10)) 
												  , iif(ISNULL(psdsC.SpecValue,'') = '', '', psdsC.SpecValue + ' - ') + dbo.GetColorMultipleID (p.BrandID, psdsC.SpecValue) )
							ELSE Concat(iif(ISNULL(p.SuppColor,'') = '', '', p.SuppColor + CHAR(10)) 
												  , iif(ISNULL(psdsC.SpecValue,'') = '', '', psdsC.SpecValue + ' - ') + dbo.GetColorMultipleID (p.BrandID, psdsC.SpecValue) )
							END
		, @StockSP = isnull(concat(p.StockPOID,' ',p.StockSeq1,' ',p.StockSeq2),'')
		, @po_desc=@po_desc + iif(ISNULL(p.ColorDetail,'') = '', '', 'ColorDetail : ' + p.ColorDetail + CHAR(10))
		, @po_desc=@po_desc + iif(ISNULL(psdsS.SpecValue,'') = '', '', psdsS.SpecValue + ' ')
		, @po_desc=@po_desc + iif(ISNULL(psdsSU.SpecValue,'') = '', '', psdsSU.SpecValue) + iif(psdsS.SpecValue = '', '', CHAR(10))
		, @po_desc=@po_desc + iif(ISNULL(p.Special,'') = '', '', p.Special + CHAR(10))
		, @POSpec=iif(ISNULL(p.Spec,'') = '', '', p.Spec + CHAR(10))
		, @po_desc=@po_desc + ISNULL(p.Remark,'')
		, @Spec = stockPO3.Spec
		, @BomZipperInsert= stockPO3.BomZipperInsert 
		from dbo.po_supp_detail p WITH (NOLOCK)
        left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = p.ID and psdsC.seq1 = p.seq1 and psdsC.seq2 = p.seq2 and psdsC.SpecColumnID = 'Color'
        left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = p.id and psdsS.seq1 = p.seq1 and psdsS.seq2 = p.seq2 and psdsS.SpecColumnID = 'Size'
        left join PO_Supp_Detail_Spec psdsSU WITH (NOLOCK) on psdsSU.ID = p.id and psdsSU.seq1 = p.seq1 and psdsSU.seq2 = p.seq2 and psdsSU.SpecColumnID = 'SizeUnit'
		left join fabric f WITH (NOLOCK) on p.SCIRefno = f.SCIRefno
		outer apply ( 
			select Spec, BomZipperInsert = tmpPO3.SpecValue from PO_Supp_Detail_Spec tmpPO3
			where tmpPO3.ID = IIF(IsNull(p.StockPOID, '') = '' , p.ID, p.StockPOID)
			and tmpPO3.Seq1 = IIF(IsNull(p.StockPOID, '') = '' , p.Seq1, p.StockSeq1)
			and tmpPO3.Seq2 = IIF(IsNull(p.StockPOID, '') = '' , p.Seq2, p.StockSeq2)
            and tmpPO3.SpecColumnID = 'ZipperInsert'
		) stockPO3
		outer apply(
			SELECT DISTINCT pp.SuppColor
			FROM po_supp_detail pp
			WHERE pp.ID=p.StockPOID AND pp.Seq1 = p.StockSeq1 AND pp.Seq2 = p.StockSeq2
		)ThreadColor
		WHERE p.ID=@poid and p.seq1 = @seq1 and p.seq2=@seq2;
	
	if isnull(@POSpec, '') <> ''
		set @string = @POSpec + char(13) + char(10) + '-----------------' + char(13) + char(10)

	IF  @type = 1
	BEGIN
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric WITH (NOLOCK) where SCIRefno = @scirefno;
			set @string += rtrim(iif(isnull(@fabric_detaildesc, '') = '','',@fabric_detaildesc + char(13) + char(10) + '-----------------' + CHAR(13)+ CHAR(10)));
		END
		ELSE
			set @string += rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor + CHAR(13)+ CHAR(10) + '-----------------' + CHAR(13)+ CHAR(10)))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc+CHAR(13)+ CHAR(10) + '-----------------' + CHAR(13)+ CHAR(10)));
	END

	IF @type =2
	BEGIN
		if @repeat = 0
		BEGIN	
			select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric WITH (NOLOCK) where SCIRefno = @scirefno;
			set @string += concat(rtrim(iif(isnull(@fabric_detaildesc, '') = '','',iif(@suppcolor='',@fabric_detaildesc,@fabric_detaildesc + CHAR(10) + '-----------------' + + CHAR(10))))
								, rtrim(iif(isnull(@suppcolor, '') = '','',iif(@po_desc='',@suppcolor,@suppcolor+ CHAR(10) + '-----------------' + + CHAR(10))))
								, rtrim(iif(isnull(@po_desc, '') = '','',replace(@po_desc,char(10),'') )));
		END
		ELSE
		set @string += rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor ))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc ));
	END

	IF @type =4
	BEGIN	
		
		if @repeat = 0
		BEGIN
			select @fabric_detaildesc= ISNULL([Description],'') from fabric WITH (NOLOCK) where SCIRefno = @scirefno;
			set @string += 'Ref#'+ @refno + ', ' + rtrim(iif(isnull(@fabric_detaildesc, '') = '','',@fabric_detaildesc + CHAR(13)+ CHAR(10) + '-----------------' + CHAR(13)+ CHAR(10))) + rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor + CHAR(13)+ CHAR(10) + '-----------------' + CHAR(13)+ CHAR(10)))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc+CHAR(13)+ CHAR(10) + '-----------------' + CHAR(13)+ CHAR(10)));
		End
		ELSE
			set @string += rtrim(iif(isnull(@suppcolor, '') = '','',@suppcolor + CHAR(13) + CHAR(10)))+rtrim(iif(isnull(@po_desc, '') = '','',@po_desc+CHAR(13) + CHAR(10)));
	END

	IF left(@SEQ1,1) = '7'
	BEGIN
		SET @string = concat('**PLS USE STOCK FROM SP#:' + iif(isnull(@StockSP, '') = '','',@StockSP) + '**' 
							, char(13) + char(10) + '-----------------' + char(13) + char(10)
							, isnull(@string, ''));
	END 

	------------------------
	--增加Zipper 
	Set @ZipperName = '';
	Select @ZipperName = DropDownList.Name
	  From Production.dbo.DropDownList
	 Where Type = 'Zipper'
	   And ID = @BomZipperInsert;

		Set @string += IIF(IsNull(@string,'') = '', '', char(13)+char(10)  + IIF(IsNull(@ZipperName, '') = '', '', 'Spec:'+ @ZipperName + Char(13) + Char(10))+ iif(isnull(@Spec,'')='','',RTrim(@Spec)));
	----------------------------


    
	-- @fabric_detaildesc 去除結尾過多的 Enter
	set @string = replace(@string, char(10), char(13));
	while (CHARINDEX(char(13) + char(13), @string, 0) > 0)
	Begin
		set @string = replace(@string, char(13) + char(13), char(13));
	end
	set @string = replace(@string, char(13), char(13) + char(10));
    
	--去掉最後的-----------------
	if CHARINDEX('-----------------', @string , len(@string) - 19) > 0
		set @string = SUBSTRING(@string , 0, CHARINDEX('-----------------', @string , len(@string) - 19))
	-- end
    RETURN ltrim(rtrim(@string))
END
