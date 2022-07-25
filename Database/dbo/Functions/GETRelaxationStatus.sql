
CREATE FUNCTION GETRelaxationStatus
(
    @Issue_DetailUkey bigint
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
        from Issue_Detail
        where ukey =  @Issue_DetailUkey
    )
END