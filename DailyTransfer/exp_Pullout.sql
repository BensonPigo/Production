
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

------------------------------------------------------------------------------------------------------
declare @DateInfoName varchar(30) ='Pullout';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= (select DATEADD(DAY,1,PullLock) from Production.dbo.System)
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);
------------------------------------------------------------------------------------------------------

SELECT * 
INTO  #CUR_PULLOUT1
FROM 
Production.dbo.Pullout
WHERE (LockDate BETWEEN @DateStart AND @DateEnd OR LockDate IS NULL) 
AND Status <> 'New'
ORDER BY Id

SELECT B.* 
	,PackingList.ExpressID
	, PackingList.CTNQTY
	, PackingList.GW
	, PackingList.NW
	, PackingList.NNW
	,PackingList.CBM
INTO Pullout_Detail
FROM #CUR_PULLOUT1  A
inner join Production.dbo.Pullout_Detail  B on A.ID = B.ID 
left join Production.dbo.PackingList on PackingList.id = b.PackingListID
ORDER BY B.ID 

SELECT B.* 
INTO Pullout_Detail_Detail
FROM #CUR_PULLOUT1  A, Production.dbo.Pullout_Detail_Detail B 
WHERE A.ID = B.ID  
ORDER BY B.ID 

SELECT B.* , OldPulloutDate=iif(B.Type = 'D'or B.Type = 'M',A.PulloutDate,Null)
INTO Pullout_Revise
FROM #CUR_PULLOUT1  A, Production.dbo.Pullout_Revise B 
WHERE A.ID = B.ID 
ORDER BY B.ID 

SELECT B.* 
INTO Pullout_Revise_Detail
FROM #CUR_PULLOUT1 A, Production.dbo.Pullout_Revise_Detail B 
WHERE A.ID = B.ID 
ORDER BY B.ID

SELECT distinct b.* 
	   , DataFrom = 'G'
	   , HCID = '             '
	   , c.Abb
	   , [IsShippingAPApprove] = isnull(IsShippingAPApprove.Value,0)
INTO  #tmpFtyBooking1
FROM Pullout_Detail  a, Production.dbo.GMTBooking b 
left join Production.dbo.LocalSupp c on b.Forwarder = c.id
outer apply(
	select top 1 [Value]  = 1
	from Production.dbo.ShareExpense se
	inner join Production.dbo.ShippingAP sa on sa.ID = se.ShippingAPID
	where se.InvNo = b.ID
	and sa.Status = 'Approved'
	and se.Junk = 0
) IsShippingAPApprove
WHERE a. INVNo = b.id 
ORDER BY b.id 

Update #tmpFtyBooking1 set Status = 'N'

select ID = a.PackingListID 
	   , InvDate = po1.PulloutDate 
	   , ETD = po1.PulloutDate 
	   , FCRDate = po1.PulloutDate
	   , p1.BrandID
	   , Dest = 'ZZ'
	   , Shupper = p1.FactoryID 
	   , TotalShipQty = p1.ShipQty
	   , Status = IIF(po1.LockDate is null, 'N', 'F')
	   , DataFrom = (select top(1) PackingListType 
					 from Production.dbo.Pullout_Detail 
					 where PackingListID = a.PackingListID) 
	   , HCID = p1. ExpressID
	   , p1.AddDate
	   , p1.EditDate
	   , [IsShippingAPApprove] = 0
into #tmpFtyBooking2
from (	
	select distinct PackingListID 
	from Production.dbo.Pullout_Detail 
	where (PackingListType = 'F' or PackingListType = 'I'or PackingListType = 'L') 
		  and PackingListID <> ''
	
	except
	select PackingListID = ID
	from #tmpFtyBooking1
) a
left join Production.dbo.PackingList p1 on a. PackingListID = p1.ID
left join Production.dbo.Pullout po1 on p1.PullOutID = po1.ID


select * 
into GMTBooking 
from (
	select ID
		   , Shipper
		   , InvSerial 
		   , InvDate
		   , BrandID
		   , CustCDID
		   , Dest
		   , ShipModeID
		   , ShipTermID
		   , PayTermARID
		   , Forwarder 
		   ,FCRDate
		   , Vessel
		   , CutOffDate 
		   , ETD 
		   , ETA 
		   , SONo 
		   , SOCFMDate 
		   , ForwarderWhse_DetailUKey 
		   , Remark 
		   , TotalShipQty
		   , TotalCTNQty
		   , TotalNW 
		   , TotalGW 
		   , TotalNNW 
		   , TotalCBM
		   , Status
		   , Handle
		   , Description 
		   , SendToTPE
		   , ShipPlanID
		   , CYCFS
		   , AddName
		   , AddDate 
		   , EditName 
		   , EditDate
		   , DataFrom
		   , HCID
		   , Abb
		   , [IsShippingAPApprove] 
	from #tmpFtyBooking1

	union all
	select ID
		   , Shipper = Shupper
		   , InvSerial = ''
		   , InvDate
		   , BrandID
		   , CustCDID = null
		   , Dest
		   , ShipModeID = ''
		   , ShipTermID = ''
		   , PayTermARID = ''
		   , Forwarder = ''
		   , FCRDate
		   , Vessel = '' 
		   , CutOffDate = null
		   , ETD 
		   , ETA = null
		   , SONo = ''
		   , SOCFMDate = null
		   , ForwarderWhse_DetailUKey = ''
		   , Remark = '' 
		   , TotalShipQty
		   , TotalCTNQty = ''
		   , TotalNW = 0.00
		   , TotalGW = 0.00
		   , TotalNNW = 0.00
		   , TotalCBM = 0.00
		   , Status
		   , Handle = ''
		   , Description = ''
		   , SendToTPE = null
		   , ShipPlanID = ''
		   , CYCFS = ''
		   , AddName = ''
		   , AddDate-- = null
		   , EditName = ''
		   , EditDate-- = null
		   , DataFrom
		   , HCID
		   , Abb = null
		   , [IsShippingAPApprove]
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




