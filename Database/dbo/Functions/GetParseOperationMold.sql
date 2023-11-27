CREATE FUNCTION [dbo].[GetParseOperationMold]
(
	@MoldData varchar(200),
	@ParseType varchar(30)
)
RETURNS varchar(200)
AS
BEGIN
	declare @result varchar(200)

	if @ParseType = 'Template'
	begin
		set @result = STUFF((	select concat(',' ,s.Data)
								from SplitString(@MoldData, ';') s
								inner join Mold m WITH (NOLOCK) on s.Data = m.ID
								where m.IsTemplate = 1 and m.Junk = 0
								for xml path (''))
							,1,1,'')
	end
	else if @ParseType = 'Attachment'
	begin
		set @result = STUFF((	select concat(',' ,s.Data)
								from SplitString(@MoldData, ';') s
								inner join Mold m WITH (NOLOCK) on s.Data = m.ID
								where m.IsAttachment = 1 and m.Junk = 0
								for xml path (''))
							,1,1,'')
	end
	else
	begin
		set @result = @MoldData
	end

	return isnull(@result, '')

END
