
-- 建立 FUNCTION
CREATE FUNCTION getMtlDesc(@poid varchar(13),@seq1 varchar(3),@seq2 varchar(2),@type int) 
RETURNS nvarchar(max)  -- 500若不夠，請加大 
BEGIN
	declare @fabric_detaildesc nvarchar(max);
	declare @suppcolor nvarchar(max);
	declare @po_desc nvarchar(max);
    DECLARE @string nvarchar(max)  -- 500若不夠，請加大 
	declare @scirefno varchar(26)
    SET @string = ''
	SET @po_desc=''
   
	IF  @type = 1
	BEGIN
		SELECT @string = @string + p.SuppColor
				from dbo.po_supp_detail p WHERE ID=@poid and seq1 = @seq1 and seq2=@seq2;
	END

	if @type =2
	begin	
		SELECT @scirefno=p.SCIRefno
		, @suppcolor = ISNULL(p.SuppColor,'')
		, @po_desc=@po_desc + ISNULL(p.ColorDetail,'')
		, @po_desc=@po_desc + ISNULL(p.sizespec,'')
		, @po_desc=@po_desc + ISNULL(p.SizeUnit,'')
		, @po_desc=@po_desc + ISNULL(p.Special,'')
		, @po_desc=@po_desc + ISNULL(p.Remark,'')
			from dbo.po_supp_detail p WHERE ID=@poid and seq1 = @seq1 and seq2=@seq2;
		if @scirefno is null or @scirefno = ''
		begin
			SELECT @scirefno=p.SCIRefno
			, @po_desc=@po_desc + ISNULL(p.sizespec,'')
			, @po_desc=@po_desc + ISNULL(p.Special,'')
			, @po_desc=@po_desc + ISNULL(p.Remark,'')
				from dbo.PO_Artwork p WHERE ID=@poid and seq1 = @seq1 and seq2=@seq2;
		end

		select @fabric_detaildesc= ISNULL(DescDetail,'') from fabric where SCIRefno = @scirefno;
		
		set @string = rtrim(ISNULL(@fabric_detaildesc,'')) + rtrim(ISNULL(@suppcolor,''))+rtrim(ISNULL(@po_desc,''))
	end

	IF @type =3
	BEGIN
		SELECT @string = @string + p.colorid
			from dbo.po_supp_detail p WHERE ID=@poid and seq1 = @seq1 and seq2=@seq2;
	end
    
    RETURN @string
END
