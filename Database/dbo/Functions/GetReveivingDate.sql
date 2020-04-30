-- =============================================
-- Author:		alger
-- Create date: 2017/03/21
-- Description:	[planning][r16]用
-- =============================================
CREATE FUNCTION [dbo].[GetReveivingDate]
(
	@POID		varchar(13)
	,@TYPE		varchar(20)  --Fabric, Accessory, Packing
)
RETURNS date
AS
BEGIN
	Declare @ReturnValue date;

	If @TYPE = 'Fabric'
	Begin
		select @ReturnValue = MAX([Fabric receiving])
		from
		(
		--Receiving
		select max(RD.WhseArrival) as [Fabric receiving]
		from dbo.View_AllReceivingDetail RD with (nolock)
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and FabricType='F' and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType!='Packing' and RD.PoId=PD.ID and RD.Seq1=PD.SEQ1 and RD.Seq2=PD.SEQ2 and RD.StockType !='O'

		union

		--SubTransfer
		select max(issuedate) as [Fabric receiving]
		from SubTransfer_Detail SD
		left join  SubTransfer S on S.Id=SD.Id
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and FabricType='F' and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType!='Packing' and SD.ToPOID=PD.ID and SD.ToSeq1=PD.SEQ1 and SD.ToSeq2=PD.SEQ2 and ToStockType!='O'

		union

		--BorrowBack
		select max(issuedate) as [Fabric receiving]
		from BorrowBack_Detail BD
		left join  BorrowBack B on B.Id=BD.Id
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and FabricType='F' and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType!='Packing' and BD.ToPOID=PD.ID and BD.ToSeq1=PD.SEQ1 and BD.ToSeq2=PD.SEQ2 and ToStockType!='O'
		) t
	End;
	else if @TYPE = 'Accessory'
	Begin
		select @ReturnValue = MAX([Accessory receiving])
		from
		(
		--Receiving
		select max(RD.WhseArrival) as [Accessory receiving]
		from dbo.View_AllReceivingDetail RD with (nolock)
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and FabricType='A' and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType!='Packing' and RD.PoId=PD.ID and RD.Seq1=PD.SEQ1 and RD.Seq2=PD.SEQ2 and RD.StockType !='O'

		union

		--SubTransfer
		select max(issuedate) as [Accessory receiving]
		from SubTransfer_Detail SD
		left join  SubTransfer S on S.Id=SD.Id
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and FabricType='A' and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType!='Packing' and SD.ToPOID=PD.ID and SD.ToSeq1=PD.SEQ1 and SD.ToSeq2=PD.SEQ2 and ToStockType!='O'

		union

		--BorrowBack
		select max(issuedate) as [Accessory receiving]
		from BorrowBack_Detail BD
		left join  BorrowBack B on B.Id=BD.Id
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and FabricType='A' and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType!='Packing' and BD.ToPOID=PD.ID and BD.ToSeq1=PD.SEQ1 and BD.ToSeq2=PD.SEQ2 and ToStockType!='O'
		) t
	End;
	else if @TYPE = 'Packing'
	Begin
		select @ReturnValue = MAX([Packing receiving])
		from
		(
		--Receiving
		select max(RD.PackingReceive) as [Packing receiving]
		from dbo.View_AllReceivingDetail RD with (nolock)
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType='Packing' and RD.PoId=PD.ID and RD.Seq1=PD.SEQ1 and RD.Seq2=PD.SEQ2 and RD.StockType !='O'

		union

		--SubTransfer
		select max(issuedate) as [Packing receiving]
		from SubTransfer_Detail SD
		left join  SubTransfer S on S.Id=SD.Id
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType='Packing' and SD.ToPOID=PD.ID and SD.ToSeq1=PD.SEQ1 and SD.ToSeq2=PD.SEQ2 and ToStockType!='O'

		union

		--BorrowBack
		select max(issuedate) as [Packing receiving]
		from BorrowBack_Detail BD
		left join  BorrowBack B on B.Id=BD.Id
		outer apply (select Id,SEQ1,SEQ2,SCIRefno from PO_Supp_Detail where ID=@POID and Complete =0) PD
		inner join fabric f WITH (NOLOCK) on f.SCIRefno=PD.SCIRefno 
		inner join MtlType m on m.id = f.MtlTypeID 
		where m.IssueType='Packing' and BD.ToPOID=PD.ID and BD.ToSeq1=PD.SEQ1 and BD.ToSeq2=PD.SEQ2 and ToStockType!='O'
		) t
	End;

	Return @ReturnValue;
END