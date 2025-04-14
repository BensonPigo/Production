CREATE FUNCTION [dbo].[GetSCICtnNoFromBarcode]
(
	@scanResult varchar(50)
)
RETURNS varchar(16)
AS
BEGIN
	declare @PackID varchar(13) = SUBSTRING(@scanResult, 1, 13)
	declare @CTNStartNo varchar(6) = REPLACE(SUBSTRING(@scanResult, 14, 6),'^','')
	declare @SCICtnNo16 varchar(16) = SUBSTRING(@scanResult, 1, 16)
	declare @SCICtnNo15 varchar(15) = SUBSTRING(@scanResult, 1, 15)
	declare @SCICtnNo varchar(16) = ''

	if exists(select 1 from PackingList_Detail with (nolock) where SCICtnNo = @SCICtnNo16)
	begin
		return @SCICtnNo16
	end

	if exists(select 1 from PackingList_Detail with (nolock) where SCICtnNo = @SCICtnNo15)
	begin
		return @SCICtnNo15
	end

	select @SCICtnNo = SCICtnNo
	from PackingList_Detail with (nolock)
	where ID = @PackID and CTNStartNo = @CTNStartNo and CTNQty > 0

	if isnull(@SCICtnNo, '') <> ''
	begin
		return @SCICtnNo
	end

	select @SCICtnNo = SCICtnNo
	from PackingList_Detail with (nolock)
	where CustCTN = @scanResult and CTNQty > 0

	return isnull(@SCICtnNo, '')
END
