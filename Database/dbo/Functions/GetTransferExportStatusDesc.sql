CREATE FUNCTION [dbo].[GetTransferExportStatusDesc]
(
	@Status varchar(20),
	@StatusColumn varchar(10)
)
RETURNS varchar(100)
as
begin
	declare @result varchar(150)
	if (@StatusColumn = 'Status')
	begin
		select @result = case	when @Status = 'New' then 'New'
								when @Status = 'Sent' then '[Sent] From fty can start to Transfer Out'
								when @Status = 'Request Separate' then '[Request Separate] wait to TPE approve'
								when @Status = 'Separate Approved' then '[Separate Approved] wait to Fty confirm'
								when @Status = 'Separate Reject' then '[Separate Reject] please revise group list'
								when @Status = 'Fty Confirm' then 'Fty Confirm'
								when @Status = 'Confirm' then '[Confirm] To fty can start to Transfer In'
								when @Status = 'Closed' then 'Closed'
								when @Status = 'Junk' then 'Junk'
								else @Status end
	end
	else
	begin
		select @result = case	when @Status = 'New' then 'New'
								when @Status = 'Send' then '[Send] From fty WH Confirm'
								when @Status = 'Confirmed' then '[Confirmed] From fty Shipping Confirm'
								when @Status = 'Request Separate' then '[Request Separate] wait to TPE approve'
								when @Status = 'WH Separate Confirm' then 'WH Separate Confirm'
								when @Status = 'Shipping Separate Confirm' then 'Shipping Separate Confirm'
								else @Status end
	end
	return @result
end
