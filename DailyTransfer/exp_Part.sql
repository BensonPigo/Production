

-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/08/17>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[exp_Part]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.PartPO') IS NOT NULL
BEGIN
  DROP TABLE PartPO
END

IF OBJECT_ID(N'dbo.RepairPO') IS NOT NULL
BEGIN
  DROP TABLE RepairPO
END

IF OBJECT_ID(N'dbo.MiscPO') IS NOT NULL
BEGIN
  DROP TABLE MiscPO
END

IF OBJECT_ID(N'dbo.MachinePO') IS NOT NULL
BEGIN
  DROP TABLE MachinePO
END

IF OBJECT_ID(N'dbo.PartPO_Detail') IS NOT NULL
BEGIN
  DROP TABLE PartPO_Detail
END

IF OBJECT_ID(N'dbo.RepairPO_Detail') IS NOT NULL
BEGIN
  DROP TABLE RepairPO_Detail
END

IF OBJECT_ID(N'dbo.MachinePO_Detail') IS NOT NULL
BEGIN
  DROP TABLE MachinePO_Detail
END

IF OBJECT_ID(N'dbo.MiscPO_Detail') IS NOT NULL
BEGIN
  DROP TABLE MiscPO_Detail
END

IF OBJECT_ID(N'dbo.PartReturnReceive') IS NOT NULL
BEGIN
  DROP TABLE PartReturnReceive
END
IF OBJECT_ID(N'dbo.PartReturnReceive_Detail') IS NOT NULL
BEGIN
  DROP TABLE PartReturnReceive_Detail
END
IF OBJECT_ID(N'dbo.NewPart') IS NOT NULL
BEGIN
  DROP TABLE NewPart
END
IF OBJECT_ID(N'dbo.NewPart_Detail') IS NOT NULL
BEGIN
  DROP TABLE NewPart_Detail
END
IF OBJECT_ID(N'dbo.PartStock') IS NOT NULL
BEGIN
  DROP TABLE PartStock
END
IF OBJECT_ID(N'dbo.MachineReturn') IS NOT NULL
BEGIN
  DROP TABLE MachineReturn
END
IF OBJECT_ID(N'dbo.MachineReturn_Detail') IS NOT NULL
BEGIN
  DROP TABLE MachineReturn_Detail
END
IF OBJECT_ID(N'dbo.#TPI_PartPO1') IS NOT NULL
BEGIN
  DROP TABLE #TPI_PartPO1
END

IF OBJECT_ID(N'dbo.#TPI_MachIn1') IS NOT NULL
BEGIN
  DROP TABLE #TPI_MachIn1
END

IF OBJECT_ID(N'dbo.MachineIn') IS NOT NULL
BEGIN
  DROP TABLE MachineIn
END

IF OBJECT_ID(N'dbo.MachineIn_Detail') IS NOT NULL
BEGIN
  DROP TABLE MachineIn_Detail
END
IF OBJECT_ID(N'dbo.Machine') IS NOT NULL
BEGIN
  DROP TABLE Machine
END

SELECT * 
INTO  PartPO
FROM Machine.dbo.PartPO 
WHERE Approve IS NOT NULL
AND (cdate>=DATEADD(DAY,-7,GETDATE()) OR TranstoTPE IS NULL OR EditDate >= DATEADD(DAY,-7,GETDATE()))
And Status = 'Approved'
AND PurchaseFrom = 'T'

SELECT * 
INTO  RepairPO
FROM Machine.dbo.RepairPO 
WHERE (cdate>=DATEADD(DAY,-7,GETDATE()) OR TranstoTPE IS NULL OR EditDate >= DATEADD(DAY,-7,GETDATE()))
And Status = 'Confirmed'

SELECT * 
INTO  MiscPO
FROM Machine.dbo.MiscPO 
WHERE Approve IS NOT NULL
AND (cdate>=DATEADD(DAY,-7,GETDATE()) OR TranstoTPE IS NULL OR EditDate >= DATEADD(DAY,-7,GETDATE()))
And Status = 'Approved'
AND PurchaseFrom = 'T'

SELECT * 
INTO  MachinePO
FROM Machine.dbo.MachinePO 
WHERE Approve IS NOT NULL
AND (cdate>=DATEADD(DAY,-7,GETDATE())  OR EditDate >= DATEADD(DAY,-7,GETDATE()))
And Status = 'Approved'
AND PurchaseFrom = 'T'
------------------------------------------------

SELECT pod.ID,pod.seq1,pod.SEQ2,pod.PartID, pod.UnitID, pod.PRICE, pod.QTY, pod.PartBrandID, pod.suppid ,pod.PartReqID,pod.InQty
INTO  PartPO_Detail
FROM Pms_To_Trade.dbo.PartPO, Machine.dbo.PartPO_Detail  pod
WHERE PartPO.id= pod.id  
ORDER BY PartPO.id 

SELECT 
rpod.ID,rpod.Seq2,rpod.Type,rpod.MachineGroupID,rpod.BrandID,rpod.Refno,rpod.Model,rpod.SerialNo,rpod.MfgDate,rpod.RepairTypeID,
rpod.BoxName,rpod.BoxType,rpod.BoardNo,rpod.Reason,rpod.UnitID,rpod.CurrencyID,rpod.Qty,rpod.Remark
INTO  RepairPO_Detail
FROM Pms_To_Trade.dbo.RepairPO, Machine.dbo.RepairPO_Detail  rpod
WHERE RepairPO.id= rpod.id  
ORDER BY RepairPO.id 

SELECT pod.ID,pod.seq1, pod.SEQ2 , pod.PRICE, pod.QTY, pod.MachineBrandID, pod.suppid
INTO  MachinePO_Detail
FROM Pms_To_Trade.dbo.MachinePO, Machine.dbo.MachinePO_Detail  pod
WHERE MachinePO.id= pod.id  
ORDER BY MachinePO.id 

SELECT pod.ID,pod.SEQ1, pod.SEQ2, pod.MiscID, pod.UnitID, pod.PRICE, pod.QTY,pod.MiscBrandID, pod.suppid,pod.MiscReqID,pod.DepartmentID
	,MiscReqApv =m.Approve,pod.InQty
INTO  MiscPO_Detail
FROM Pms_To_Trade.dbo.MiscPO, Machine.dbo.MiscPO_Detail  pod,Machine.dbo.MiscReq m
WHERE MiscPO.id= pod.id  and pod.MiscReqID = m.ID 
ORDER BY MiscPO.id 
--------------------------------------------------------------
UPDATE Machine.dbo.PartPO
SET TranstoTPE = CONVERT(date, GETDATE())
FROM Machine.dbo.PartPO AS Partpo1
LEFT JOIN PartPO ON Partpo1.ID = PartPO.ID
WHERE Partpo1.TranstoTPE  IS NULL

UPDATE Machine.dbo.RepairPO
SET TranstoTPE = CONVERT(date, GETDATE())
FROM Machine.dbo.RepairPO AS RepairPO1
INNER JOIN RepairPO ON RepairPO1.ID = RepairPO.ID
WHERE RepairPO1.TranstoTPE  IS NULL

UPDATE Machine.dbo.MiscPO
SET TranstoTPE = CONVERT(date, GETDATE())
FROM Machine.dbo.MiscPO AS MiscPO1
LEFT JOIN MiscPO ON MiscPO1.ID = MiscPO.ID
WHERE MiscPO1.TranstoTPE  IS NULL

----------------------------------------------------------------
SELECT * 
INTO  PartReturnReceive
FROM Machine.dbo.PartReturnReceive 
Where Status = 'Confirmed'
AND (cdate>=DATEADD(DAY,-7,GETDATE()) or EditDate>=DATEADD(DAY,-7,GETDATE()))

SELECT Partrcvre2.* 
INTO   PartReturnReceive_Detail
FROM Pms_To_Trade.dbo.PartReturnReceive, Machine.dbo.PartReturnReceive_Detail as Partrcvre2 
WHERE PartReturnReceive.id=Partrcvre2.id  
ORDER BY  PartReturnReceive.id 

SELECT  * 
INTO  NewPart
FROM Machine.dbo.NewPart
WHERE Status = 'Confirmed'
AND (cdate>=DATEADD(DAY,-7,GETDATE()) or EditDate>=DATEADD(DAY,-7,GETDATE()))

SELECT Partapp2.* 
INTO  NewPart_Detail
FROM Pms_To_Trade.dbo.NewPart, Machine.dbo.NewPart_Detail as Partapp2 
WHERE NewPart.id=Partapp2.id 
ORDER BY NewPart.id 

SELECT * 
INTO PartStock
FROM Machine.dbo.PartStock 


SELECT mr.* 
Into MachineReturn
FROM Machine.dbo.MachineReturn mr
Left join Machine.dbo.MachinePO mp on mr.ID = mp.ID
Where mr.Status = 'Confirmed'
AND (mr.cdate>=DATEADD(DAY,-7,GETDATE()) OR mr.TranstoTPE is null)
AND mp.PurchaseFrom = 'T'

SELECT Machreturn2.* 
INTO MachineReturn_Detail
FROM Pms_To_Trade.dbo.MachineReturn, Machine.dbo.MachineReturn_Detail as Machreturn2 
WHERE MachineReturn.id=Machreturn2.id 
ORDER BY Machreturn2.id 

Update a
Set TranstoTPE = CONVERT(date, GETDATE())
From Machine.dbo.MachineReturn as a  inner join  Machine.dbo.MachineReturn as b on a.ID= b.ID
Where  a.TranstoTPE  is null
---------------------------------------------------------------------------------------------------

SELECT ID 
INTO #TPI_PartPO1
FROM Machine.dbo.MachinePO WHERE PurchaseFrom = 'T'

SELECT DISTINCT MachIn3.ID
INTO #TPI_MachIn1
FROM Machine.dbo.MachineIn_Detail_Inspect AS MachIn3, #TPI_PartPO1 
WHERE MachIn3. MachinePOID = #TPI_PartPO1.ID 

SELECT MachIn1.* 
INTO MachineIn
FROM Machine.dbo.MachineIn AS MachIn1, #TPI_MachIn1 WHERE MachIn1.ID = #TPI_MachIn1.ID 
AND MachIn1. Status = 'Received'
AND MachIn1. EditInspDate >=DATEADD(DAY,-7,GETDATE())

SELECT MachIn2.* 
INTO MachineIn_Detail
FROM Pms_To_Trade.dbo.MachineIn, Machine.dbo.MachineIn_Detail AS MachIn2
WHERE MachineIn.ID = MachIn2.ID ORDER BY MachineIn.ID 

-----------------------------------Machine-------------------------------------
select MDivisionID, MachineGroupID 
INTO Machine
from Machine.dbo.Machine
where Status in ('Good', 'Repairing', 'Lent')
and Junk = 0
group by MDivisionID, MachineGroupID


drop table #TPI_MachIn1
drop table #TPI_PartPO1
---------------------------------------------------------------------------------------------------

END




