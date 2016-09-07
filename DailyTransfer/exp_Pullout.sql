USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Pullout]    Script Date: 2016/9/2 ¤W¤È 10:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<LEO>
-- Create date: <2016/08/18>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[exp_Pullout]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'Pullout_Detail') IS NOT NULL
BEGIN
  DROP TABLE Pullout_Detail
END
IF OBJECT_ID(N'Pullout_Detail_Detail') IS NOT NULL
BEGIN
  DROP TABLE Pullout_Detail_Detail
END
IF OBJECT_ID(N'Pullout_Revise') IS NOT NULL
BEGIN
  DROP TABLE Pullout_Revise
END
IF OBJECT_ID(N'Pullout_Revise_Detail') IS NOT NULL
BEGIN
  DROP TABLE Pullout_Revise_Detail
END
IF OBJECT_ID(N'GMTBooking') IS NOT NULL
BEGIN
  DROP TABLE GMTBooking
END
SELECT * 
INTO  #CUR_PULLOUT1
FROM 
Production.dbo.Pullout
WHERE (LockDate BETWEEN (select DATEADD(DAY,1,PullLock) from Production.dbo.System) AND CONVERT(date, GETDATE()) OR LockDate IS NULL) 
AND Status <> 'New'
ORDER BY Id

SELECT B.* 
INTO Pullout_Detail
FROM #CUR_PULLOUT1  A, Production.dbo.Pullout_Detail  B 
WHERE A.ID = B.ID 
ORDER BY B.ID 

SELECT B.* 
INTO Pullout_Detail_Detail
FROM #CUR_PULLOUT1  A, Production.dbo.Pullout_Detail_Detail B 
WHERE A.ID = B.ID  
ORDER BY B.ID 

SELECT B.* 
INTO Pullout_Revise
FROM #CUR_PULLOUT1  A, Production.dbo.Pullout_Revise B 
WHERE A.ID = B.ID 
ORDER BY B.ID 

SELECT B.* 
INTO Pullout_Revise_Detail
FROM #CUR_PULLOUT1 A, Production.dbo.Pullout_Revise_Detail B 
WHERE A.ID = B.ID 
ORDER BY B.ID

SELECT b.* ,'G' AS DataFrom ,'             'AS HCID
INTO  #tmpFtyBooking1
FROM Production.dbo.Pullout_Detail  a, Production.dbo.GMTBooking b 
WHERE a. INVNo = b.id 
ORDER BY b.id 

Update #tmpFtyBooking1 set Status = 'N'

select a.PackingListID as ID
, po1.PulloutDate as  InvDate, po1.PulloutDate as ETD, po1.PulloutDate as FCRDate, p1.BrandID, 'ZZ' as Dest , p1. FactoryID as Shupper
, p1.ShipQty as TotalShipQty, IIF(po1.LockDate is null,'N','F') as Status, (select top(1) PackingListType from Production.dbo.Pullout_Detail where PackingListID = a.PackingListID) as DataFrom, p1. ExpressID as HCID
into #tmpFtyBooking2
 from (	
select distinct PackingListID from Production.dbo.Pullout_Detail where (PackingListType = 'F' or PackingListType = 'I') and PackingListID <> ''
except
select ID as PackingListID from #tmpFtyBooking1
) a
left join Production.dbo.PackingList p1 on a. PackingListID = p1.ID
left join Production.dbo.Pullout po1 on p1.PullOutID = po1.ID


select * 
into GMTBooking 
from (
select ID,Shipper, InvSerial ,InvDate,BrandID,CustCDID,Dest, ShipModeID, ShipTermID, PayTermARID, Forwarder 
,FCRDate
, Vessel, CutOffDate , ETD , ETA ,  SONo ,  SOCFMDate , ForwarderWhse_DetailUKey ,  Remark ,TotalShipQty, TotalCTNQty
, TotalNW ,  TotalGW ,  TotalNNW ,  TotalCBM
, Status, Handle, Description ,  SendToTPE
, ShipPlanID, CYCFS, AddName, AddDate , EditName , EditDate, DataFrom,HCID
from #tmpFtyBooking1
union all
select ID,Shupper as Shipper,'' as InvSerial ,InvDate,BrandID,'' as CustCDID,Dest,''as ShipModeID,''as ShipTermID,'' as PayTermARID,'' as Forwarder
,FCRDate
,'' as Vessel,'' as CutOffDate , ETD ,'' as ETA , '' as SONo , '' as SOCFMDate ,''as ForwarderWhse_DetailUKey , '' as Remark ,TotalShipQty,'' as TotalCTNQty
,0.00 as TotalNW , 0.00 as TotalGW , 0.00 as TotalNNW , 0.00 as TotalCBM
, Status,'' as Handle,'' as Description , '' as SendToTPE
,'' as ShipPlanID,'' as CYCFS,'' as AddName,'' as AddDate ,''as EditName ,''as EditDate, DataFrom,HCID
from #tmpFtyBooking2
) a 


UPDATE  Production.dbo.Pullout
SET SendToTPE = CONVERT(date, GETDATE())
FROM Production.dbo.Pullout a
INNER JOIN #CUR_PULLOUT1 b ON a.ID=b.ID
WHERE a.SendToTPE IS NULL 


DROP TABLE #CUR_PULLOUT1
DROP TABLE #tmpFtyBooking1
DROP TABLE #tmpFtyBooking2
END

GO


