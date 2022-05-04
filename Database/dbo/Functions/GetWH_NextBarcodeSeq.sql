
CREATE FUNCTION GetWH_NextBarcodeSeq
(
	@barcode varchar(255)
)
RETURNS varchar(2)
AS
BEGIN
	declare @BarcodeSeq varchar(2)
	declare @BarcodeSeqInt int
	
	select @BarcodeSeqInt = isnull(cast(Max(Seq) as int), 0) + 1
	from(
		select Seq = From_OldBarcodeSeq from WHBarcodeTransaction where From_OldBarcode = @barcode

		union all
		select From_NewBarcodeSeq from WHBarcodeTransaction where From_NewBarcode = @barcode

		union all
		select To_OldBarcodeSeq from WHBarcodeTransaction where To_OldBarcode = @barcode

		union all
		select To_NewBarcodeSeq from WHBarcodeTransaction where To_NewBarcode = @barcode
		
		-- 舊資料 FtyInventory_Barcode
		union all		
		select Seq = iif(barcode like '%-%', SUBSTRING(barcode, CHARINDEX('-', barcode, 0) + 1, 3), 0)
		from(
			select barcode = MAX(barcode)
			from FtyInventory_Barcode
			where Barcode like @barcode+'%'
		)x
	)x
	
	set @BarcodeSeq = RIGHT(REPLICATE('0', 2) + CAST(@BarcodeSeqInt as NVARCHAR), 2)

	RETURN @BarcodeSeq

END