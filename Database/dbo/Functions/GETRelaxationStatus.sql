
CREATE FUNCTION [dbo].[GETRelaxationStatus]
(
    @Barcode varchar(255)
)
Returns varchar(20)
As
BEGIN 
	-- Return the result of the function
	RETURN
    (
        select
            case
            when RelaxationStartTime is null then ''
            when RelaxationStartTime is not null and RelaxationEndTime > GETDATE() then 'Ongoing'
            when RelaxationStartTime is not null and RelaxationEndTime <= GETDATE() then 'Done'
            end
        from Fabric_UnrollandRelax
        where barcode =  @Barcode
    )
END