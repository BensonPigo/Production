CREATE FUNCTION GetSinglelineSP (@XML xml)
Returns varchar(max)
AS
Begin
	declare @SP1 varchar(max)='', @SP2 varchar(max)=''
	;with tb as(
		select
			RowNr=Row_Number() over (Order By (Select NULL)),
			OrderID = Tbl.Col.value('@OrderID', 'nvarchar(13)')
		FROM @xml.nodes('/row') Tbl(Col)
	),firstSP as(
		select OrderID from tb where RowNr = 1
	)
	,other as (
		select OrderIDs = IIF(LEN(OrderID) <= 10, OrderID , substring(OrderID,11,6)) FROM tb where RowNr > 1
	)
	select @SP1 = firstSP.OrderID, @SP2=@SP2+'/'+OrderIDs from firstSP left join other on 1=1

    Return concat(@SP1,@SP2)
End