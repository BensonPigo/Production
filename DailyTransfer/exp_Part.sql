
Create PROCEDURE [dbo].[exp_Part]
	
AS
BEGIN
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.PartPO') IS NOT NULL
BEGIN
  DROP TABLE PartPO
END

IF OBJECT_ID(N'dbo.Part') IS NOT NULL
BEGIN
  DROP TABLE Part
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

IF OBJECT_ID(N'dbo.MachinePending') IS NOT NULL
BEGIN
  DROP TABLE MachinePending
END

IF OBJECT_ID(N'dbo.MachinePending_Detail') IS NOT NULL
BEGIN
  DROP TABLE MachinePending_Detail
END

declare @DateInfoName varchar(30) ='SciMachine_Part';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = NULL--(select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-7,GETDATE()))
if @DateEnd is Null
	set @DateEnd = NULL-- CONVERT(DATE, GETDATE())
	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);

SELECT * 
INTO  PartPO
FROM Production.dbo.SciMachine_PartPO 
WHERE Approve IS NOT NULL
AND (cdate>=@DateStart OR TranstoTPE IS NULL OR EditDate >= @DateStart)
And Status = 'Approved'
AND PurchaseFrom = 'T'

SELECT ID ,Minstock ,Consumable
INTO  Part
FROM Production.dbo.SciMachine_Part
WHERE EditDate >= @DateStart

SELECT * 
INTO  RepairPO
FROM Production.dbo.SciMachine_RepairPO 
WHERE (cdate>=@DateStart OR TranstoTPE IS NULL OR EditDate >= @DateStart)
And Status = 'Confirmed'

SELECT * 
INTO  MiscPO
FROM Production.dbo.SciMachine_MiscPO 
WHERE Approve IS NOT NULL
AND (cdate>=@DateStart OR TranstoTPE IS NULL OR EditDate >= @DateStart)
And Status = 'Approved'
AND PurchaseFrom = 'T'

SELECT * 
INTO  MachinePO
FROM Production.dbo.SciMachine_MachinePO 
WHERE Approve IS NOT NULL
AND (cdate>=@DateStart  OR EditDate >= @DateStart)
And Status = 'Approved'
AND PurchaseFrom = 'T'
------------------------------------------------

SELECT pod.ID,pod.seq1,pod.SEQ2,pod.PartID, pod.UnitID, pod.PRICE, pod.QTY, pod.PartBrandID, pod.suppid ,pod.PartReqID,pod.InQty
	,prd.MinQty,prd.StockQty,prd.RoadQty
	,pr.FactoryApprove, pr.FactoryApproveDate, pr.CmdApprove, pr.CmdApproveDate, pr.MgApprove, pr.MgApproveDate,prd.Consumable
INTO  PartPO_Detail
FROM Pms_To_Trade.dbo.PartPO, Production.dbo.SciMachine_PartPO_Detail  pod
left join Production.dbo.SciMachine_PartReq_Detail prd on prd.ID= pod.PartReqID and prd.PartID=pod.PartID
left join Production.dbo.SciMachine_PartReq pr on pr.id=prd.ID
WHERE PartPO.id= pod.id  
ORDER BY PartPO.id 

SELECT 
rpod.ID,rpod.Seq2,rpod.Type,rpod.MachineGroupID,rpod.BrandID,rpod.Refno,rpod.Model,rpod.SerialNo,rpod.MfgDate,rpod.RepairTypeID,
rpod.BoxName,rpod.BoxType,rpod.BoardNo,rpod.Reason,rpod.UnitID,rpod.CurrencyID,rpod.Qty,rpod.Remark,rpod.MasterGroupID
INTO  RepairPO_Detail
FROM Pms_To_Trade.dbo.RepairPO, Production.dbo.SciMachine_RepairPO_Detail  rpod
WHERE RepairPO.id= rpod.id  
ORDER BY RepairPO.id 

SELECT pod.ID,pod.seq1, pod.SEQ2 , pod.PRICE, pod.QTY, pod.MachineBrandID, pod.suppid
INTO  MachinePO_Detail
FROM Pms_To_Trade.dbo.MachinePO, Production.dbo.SciMachine_MachinePO_Detail  pod
WHERE MachinePO.id= pod.id  
ORDER BY MachinePO.id 

SELECT pod.ID, pod.SEQ1, pod.SEQ2, pod.MiscID, pod.UnitID, pod.PRICE, pod.QTY, pod.MiscBrandID, pod.suppid, pod.MiscReqID, pod.DepartmentID
	,MiscReqApv =m.Approve, pod.InQty,md.Reason, m.DeptApprove, m.DeptApproveDate, m.Approve, m.ApproveDate
INTO  MiscPO_Detail
FROM Pms_To_Trade.dbo.MiscPO, Production.dbo.SciMachine_MiscPO_Detail pod, Production.dbo.SciMachine_MiscReq m, Production.dbo.SciMachine_MiscReq_Detail md
WHERE MiscPO.id= pod.id  and pod.MiscReqID = m.ID and m.id = md.id and md.MiscID = pod.MiscID
ORDER BY MiscPO.id 

----------------------------------------------------------------
SELECT * 
INTO  PartReturnReceive
FROM Production.dbo.SciMachine_PartReturnReceive 
Where Status = 'Confirmed'
AND (cdate>=@DateStart or EditDate>=@DateStart)

SELECT Partrcvre2.* 
INTO   PartReturnReceive_Detail
FROM Pms_To_Trade.dbo.PartReturnReceive, Production.dbo.SciMachine_PartReturnReceive_Detail as Partrcvre2 
WHERE PartReturnReceive.id=Partrcvre2.id  
ORDER BY  PartReturnReceive.id 

SELECT  * 
INTO  NewPart
FROM Production.dbo.SciMachine_NewPart
WHERE Status = 'Confirmed'
AND (cdate>=@DateStart or EditDate>=@DateStart)

SELECT Partapp2.* 
INTO  NewPart_Detail
FROM Pms_To_Trade.dbo.NewPart, Production.dbo.SciMachine_NewPart_Detail as Partapp2 
WHERE NewPart.id=Partapp2.id 
ORDER BY NewPart.id 

SELECT * 
INTO PartStock
FROM Production.dbo.SciMachine_PartStock 


SELECT mr.* 
Into MachineReturn
FROM Production.dbo.SciMachine_MachineReturn mr
Left join Production.dbo.SciMachine_MachinePO mp on mr.ID = mp.ID
Where mr.Status = 'Confirmed'
AND (mr.cdate>=@DateStart OR mr.TranstoTPE is null)
AND mp.PurchaseFrom = 'T'

SELECT Machreturn2.* 
INTO MachineReturn_Detail
FROM Pms_To_Trade.dbo.MachineReturn, Production.dbo.SciMachine_MachineReturn_Detail as Machreturn2 
WHERE MachineReturn.id=Machreturn2.id 
ORDER BY Machreturn2.id 

---------------------------------------------------------------------------------------------------

SELECT ID 
INTO #TPI_PartPO1
FROM Production.dbo.SciMachine_MachinePO WHERE PurchaseFrom = 'T'

SELECT DISTINCT MachIn3.ID
INTO #TPI_MachIn1
FROM Production.dbo.SciMachine_MachineIn_Detail_Inspect AS MachIn3, #TPI_PartPO1 
WHERE MachIn3. MachinePOID = #TPI_PartPO1.ID 

SELECT MachIn1.* 
INTO MachineIn
FROM Production.dbo.SciMachine_MachineIn AS MachIn1, #TPI_MachIn1 WHERE MachIn1.ID = #TPI_MachIn1.ID 
AND MachIn1. Status = 'Received'
AND MachIn1. EditInspDate >=@DateStart

SELECT MachIn2.* 
INTO MachineIn_Detail
FROM Pms_To_Trade.dbo.MachineIn, Production.dbo.SciMachine_MachineIn_Detail AS MachIn2
WHERE MachineIn.ID = MachIn2.ID ORDER BY MachineIn.ID 

-----------------------------------Machine-------------------------------------
select LocationM, MachineGroupID 
INTO Machine
from Production.dbo.SciMachine_Machine
where Status in ('Good', 'Repairing', 'Lent')
and Junk = 0
group by LocationM, MachineGroupID


drop table #TPI_MachIn1
drop table #TPI_PartPO1
---------------------------------------------------------------------------------------------------

declare @DateInfoName varchar(30) ='MachinePending';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= CONVERT(DATE,DATEADD(day,-30,GETDATE()))
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE,DATEADD(day,30,GETDATE()))
	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);

----MachinePending----
select [ID]
    ,[cDate]
    ,[MDivisionID]
    ,[FtyApvName]
    ,[FtyApvDate]
    ,[CyApvName]
    ,[CyApvDate]
    ,[ReasonID]
    ,[Remark]
    ,[Status]
    ,[AddName]
    ,[AddDate]
    ,[EditName]
    ,[EditDate]
    ,[TPEComplete]
into MachinePending
from Production.dbo.SciMachine_MachinePending
where SciMachine_MachinePending.Status='Confirmed'
and SciMachine_MachinePending.CyApvDate between @DateStart and @DateEnd
and SciMachine_MachinePending.SendToTPE is null
and SciMachine_MachinePending.TPEComplete=0

----MachinePending_Detail----
select 
  SciMachine_MachinePending_Detail.ID
  ,SciMachine_MachinePending_Detail.Seq
  ,SciMachine_MachinePending_Detail.MachineID
  ,SciMachine_MachinePending_Detail.OldStatus
  ,SciMachine_MachinePending_Detail.Results
  ,SciMachine_MachinePending_Detail.Remark
  ,MasterGroupID=SciMachine_Machine.MasterGroupID+SciMachine_Machine.MachineGroupID
  ,SciMachine_Machine.MachineBrandID
  ,SciMachine_Machine.Model
  ,SciMachine_Machine.SerialNo
  ,SciMachine_Machine.LocationM
  ,SciMachine_Machine.ArriveDate
  ,UsageTime = concat(ym.UsageTime/360,'Y',(ym.UsageTime%360)/30,'M')
  ,SciMachine_MachinePending_Detail.MachineDisposeID
into MachinePending_Detail
from Production.dbo.SciMachine_MachinePending_Detail
inner join (
	---- 除了申請 Machine to Dispose 等待台北 Approved的資料以外
	---- 再將30天內從台北 approve 後, 從工廠端轉 Dispose的MachinePending detail資料傳回台北更新 Result , MachineDisposeID
	select id, Cdate from MachinePending
	union
	select m.id , m.Cdate
	from Production.dbo.SciMachine_MachinePending m
	INNER JOIN Production.dbo.SciMachine_MachinePending_Detail md ON m.ID = md.ID
	INNER JOIN Production.dbo.SciMachine_MachineDispose d ON  md.MachineDisposeID = d.ID
	where (d.AddDate  between @DateStart and @DateEnd   OR
		  d.EditDate  between @DateStart and @DateEnd ) 
	
)MP on MP.ID = SciMachine_MachinePending_Detail.ID
left join Production.dbo.SciMachine_Machine with (nolock) on SciMachine_Machine.ID = SciMachine_MachinePending_Detail.MachineID
outer apply(select UsageTime=DATEDIFF(DAY,SciMachine_Machine.ArriveDate, MP.cDate)+1)ym


END


