USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_3rd]    Script Date: 2016/9/2 上午 10:34:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_3rd>
-- =============================================
CREATE PROCEDURE [dbo].[exp_3rd]

AS

IF OBJECT_ID(N'ThirdReport') IS NOT NULL
BEGIN
  DROP TABLE ThirdReport
END

BEGIN
	with
p07 as
(SELECT 
		Receiving.Id,
		Receiving. WhseArrival as CDate,
		Receiving_Detail. PoId as OrderId,
		Receiving_Detail.Seq1,
		Receiving_Detail.Seq2,
		Receiving.ETA,
		LocalReceiving.LocalSuppID AS SuppID,
		Receiving.MDivisionID ,
		(select SUM(Receiving_Detail.StockQty)  from Production.dbo.Receiving_Detail where Receiving.id= Receiving_Detail.Id  )as Qty,
		Receiving_Detail. StockUnit as FtyUnit,
		'' as OrderQty,
		'' as OrderFOC,
		'' as OrderUnit,
		'' as [Over]
FROM  Production.dbo.Receiving,  Production.dbo.Receiving_Detail, Production.dbo.LocalReceiving
WHERE Receiving.id= Receiving_Detail.Id 
AND Receiving.id=LocalReceiving.Id
AND Receiving.Status=' Confirmed' 
AND Receiving.TYPE = 'A' 
AND (Receiving.WhseArrival BETWEEN CONVERT(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date, GETDATE())
OR Receiving.EditDate BETWEEN CONVERT(DATE,DATEADD(day,-30,GETDATE())) AND CONVERT(date, GETDATE()) )
AND  IsNull(Receiving_Detail.PoId,'')='' 
AND (Receiving_Detail.seq1 IS NOT NULL or Receiving_Detail.seq1 IS NOT NULL)
AND Receiving_Detail.StockQty is not null AND Receiving.THIRD = 1
),
p23 as
(
SELECT 	SubTransfer.Id,
		SubTransfer. IssueDate as CDate,	
		SubTransfer_Detail.ToPOID as ISSUEDSP,
		SubTransfer_Detail.FromSeq1,
		SubTransfer_Detail.FromSeq2,
		'' as ETA, 
		'' as SuppID, 
		SubTransfer.MDivisionID,
		(Select SUM(SubTransfer_Detail.Qty) from Production.dbo.SubTransfer_Detail where SubTransfer.id= SubTransfer_Detail.Id ) as Qty,
		Po_Supp_Detail.StockUnit  as FtyUnit,
		'' as OrderQty,
		'' as OrderFOC,
		'' as OrderUnit,
		'' as [Over]
FROM  Production.dbo.SubTransfer,  Production.dbo.SubTransfer_Detail,  Production.dbo.Po_Supp_Detail
WHERE SubTransfer.id= SubTransfer_Detail.Id 
And SubTransfer_Detail.FromPOID =Po_Supp_Detail.ID and SubTransfer_Detail.FromSeq1=Po_Supp_Detail.seq1 and SubTransfer_Detail.FromSeq2= Po_Supp_Detail.Seq2
AND SubTransfer.Status='Confirmed'  And SubTransfer.type='B'
AND 
(SubTransfer.IssueDate  BETWEEN CONVERT(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date, GETDATE())
OR SubTransfer.EditDate BETWEEN CONVERT(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date, GETDATE())
) 
AND SubTransfer_Detail.ToPOID is not null 
AND SubTransfer_Detail.FromSeq1 is not null
AND SubTransfer_Detail.FromSeq2 is not null
AND SubTransfer_Detail.Qty is not null
),
p18 as
(
SELECT 	TransferIn.id,
		TransferIn. IssueDate as CDate,
		TransferIn_Detail.poid as OrderId,
		TransferIn_Detail.seq1,
		TransferIn_Detail .Seq2,
		'' as ETA, 
		'' as SuppID, 
		TransferIn. MDivisionID,
		(Select SUM(TransferIn_Detail.qty) from Production.dbo.TransferIn_Detail where TransferIn.id=TransferIn_Detail.id) as Qty,
		Po_Supp_Detail.StockUnit as FtyUnit,
		'' as OrderQty,
		'' as OrderFOC,
		'' as OrderUnit,
		'' as [Over]
FROM  Production.dbo.TransferIn,  Production.dbo.TransferIn_Detail,  Production.dbo.Po_Supp_Detail
WHERE TransferIn.id=TransferIn_Detail.id 
And TransferIn_Detail.POID = Po_Supp_Detail.ID and TransferIn_Detail.seq1= Po_Supp_Detail.seq1 and TransferIn_Detail.seq2= Po_Supp_Detail.seq2
AND TransferIn.Status='Confirmed'
AND TransferIn. IssueDate  BETWEEN CONVERT(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date, GETDATE())
AND TransferIn_Detail.poid is not null
AND TransferIn_Detail.seq1 is not null
AND TransferIn_Detail.seq2 is not null
AND TransferIn_Detail.Seq1 like'7%'
AND TransferIn_Detail.Qty is not null
),
p37 as
(
SELECT    ReturnReceipt.id,
		ReturnReceipt.IssueDate as CDate,
		ReturnReceipt_Detail.POID as OrderId,
		ReturnReceipt_Detail .seq1,
		ReturnReceipt_Detail .Seq2,
		'' as ETA, 
		'' as SuppID, 
		ReturnReceipt.MDivisionID,
		0 - (Select SUM(ReturnReceipt_Detail.qty) from Production.dbo.ReturnReceipt_Detail where ReturnReceipt.id=ReturnReceipt_Detail.id ) as Qty,
		LocalPO_Detail.UnitId,
		'' as OrderQty,
		'' as OrderFOC,
		'' as OrderUnit,
		'' as [Over]
FROM Production.dbo.ReturnReceipt,  Production.dbo.ReturnReceipt_Detail	,Production.dbo.LocalPO_Detail
WHERE ReturnReceipt.id=ReturnReceipt_Detail.id 
AND LocalPO_Detail.OrderId=ReturnReceipt_Detail.POID 
AND LocalPO_Detail.OldSeq1=ReturnReceipt_Detail.Seq1
AND LocalPO_Detail.OldSeq2=ReturnReceipt_Detail.Seq2
AND ReturnReceipt.Status=' Confirmed' 
AND ReturnReceipt.IssueDate BETWEEN CONVERT(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date, GETDATE())
OR ReturnReceipt.EditDate BETWEEN CONVERT(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date, GETDATE())
AND ReturnReceipt_Detail.POID IS NOT NULL 
AND ReturnReceipt_Detail.seq1 IS NOT NULL
AND ReturnReceipt_Detail.seq2 IS NOT NULL
AND ReturnReceipt_Detail.Qty IS NOT NULL
)
SELECT * 
INTO ThirdReport
from p07
union (select p23.* from p23 inner join p07 on p23.ISSUEDSP=p07.OrderId)
union (select * from p18)
union (select * from p37)

--修改欄位型態
alter table ThirdReport
alter column OrderQty numeric(8,1)
alter table ThirdReport
alter column OrderFOC numeric(8,1)
alter table ThirdReport
alter column OrderUnit varchar(50)
alter table ThirdReport
alter column [Over] varchar(20)



-----UPDATE 
update ThirdReport
set ThirdReport.suppid = PO_Supp.SuppID,
OrderQty = PO_Supp_Detail.Qty, 
OrderFOC = 0,
OrderUnit = PO_Supp_Detail.POUnit, 
[Over] = IIF(PO_Supp_Detail.InputQty  >=PO_Supp_Detail.Qty,'Y','N')
From Pms_To_Trade.dbo.ThirdReport 
INNER join Production.dbo.PO_Supp on ThirdReport.orderid = PO_Supp.id and ThirdReport.Seq1= PO_Supp.seq1
INNER join Production.dbo.PO_Supp_Detail on ThirdReport.orderid = PO_Supp_Detail.id and ThirdReport.Seq1 = PO_Supp_Detail.seq1
And ThirdReport.Seq2 =PO_Supp_Detail.seq2

------update QTY * unit.Rate-------
Update ThirdReport
Set Qty=ROUND(Qty * UnitRate.rate, 2)
From Pms_To_Trade.dbo.ThirdReport 
outer apply
(
select rate= [Production].[dbo].[getUnitRate](ThirdReport. FtyUnit,ThirdReport.OrderUnit) 
)as UnitRate
where UnitRate.rate is not null


-- Receiving  SubTransfer
UPDATE Receiving  
SET Receiving.Transfer2Taipei= CONVERT(date, GETDATE())
From Production.dbo.Receiving
inner join Pms_To_Trade.dbo.ThirdReport on Receiving.id = ThirdReport.id
Where IsNull(Transfer2Taipei, '' ) = ''
And  Receiving.id <> (Select top 1 Receiving.id from Production.dbo.Receiving
inner join Pms_To_Trade.dbo.ThirdReport on Receiving.id = ThirdReport.id
Order by Receiving.id desc
)

--&& 只在最後一筆壓結清

update Pms_To_Trade.dbo.ThirdReport
set [over]='New'
where [over]='confrim'
and CDate=(select MAX(CDate) from ThirdReport)

END


GO


