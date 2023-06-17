
CREATE FUNCTION [dbo].[GetUnrollRelaxationCompletion]
(
    @IssueID varchar(13)
)
Returns float
As
BEGIN 
	-- Return the result of the function
	RETURN
    (
        select
        round(
            (
                select count(1)
                from Issue_Detail id with(nolock)
                left join WHBarcodeTransaction w with (nolock) on w.TransactionID = id.ID
                                                                and w.TransactionUkey = id.Ukey
                                                                and w.Action = 'Confirm'
                left join Fabric_UnrollandRelax fu with (nolock) on fu.Barcode = w.To_NewBarcode
                where id.Id = @IssueID
                and NeedUnroll = 1
                and (
                    (fu.UnrollStatus = 'Done' and fu.RelaxationStartTime  is null)
                    or
                    (fu.UnrollStatus = 'Done' and fu.RelaxationEndTime <= GetDate())
                )
            )
            /

            cast((
                select count(1)
                from Issue_Detail id with(nolock)
                where id.Id = @IssueID
                and NeedUnroll = 1
            ) as float)
            * 100, 2)
    )
END