
Create PROCEDURE [dbo].[exp_Cutting] 

AS
BEGIN

	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.CuttingOutput') IS NOT NULL
BEGIN
  DROP TABLE CuttingOutput
END
IF OBJECT_ID(N'dbo.CuttingOutput_Detail') IS NOT NULL
BEGIN
  DROP TABLE CuttingOutput_Detail
END
IF OBJECT_ID(N'dbo.CuttingOutput_Detail_Detail') IS NOT NULL
BEGIN
  DROP TABLE CuttingOutput_Detail_Detail
END
IF OBJECT_ID(N'dbo.CuttingOutput_WIP') IS NOT NULL
BEGIN
  DROP TABLE CuttingOutput_WIP
END

IF OBJECT_ID(N'dbo.Workorder_Distribute') IS NOT NULL
BEGIN
  DROP TABLE Workorder_Distribute
END


declare @DateInfoName varchar(30) ='CUTTING';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
if @DateStart is Null
	set @DateStart= (SELECT DATEADD(DAY,1,SewLock) FROM Production.dbo.System)
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())
	
Delete Pms_To_Trade.dbo.dateInfo Where Name = @DateInfoName 
Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
values (@DateInfoName,@DateStart,@DateEnd);

SELECT ID, cDATE, MDivisionid,MANPOWER, MANHOURS, Actoutput, ActGarment, AddName, AddDate, EditName, EditDate
INTO CuttingOutput
 FROM Production.dbo.CuttingOutput  CUT1
WHERE CUT1. Status != 'New' 
AND  (CUT1.Lock  BETWEEN @DateStart AND  @DateEnd or  CUT1.Lock IS NULL)


SELECT ID,CutRef,CuttingID,isnull(FabricCombo,'') as FabricCombo,Cutno,MarkerName,Markerlength,Layer,Cons,UKey,WorkOrderUkey,Colorid 
INTO CuttingOutput_Detail
FROM (
SELECT 
CUT1. ID,CUT2.CutRef,CUT2.CuttingID,
(select isnull(FabricCombo,'') from Production.dbo.WorkOrder where UKey = CUT2.WorkOrderUKey) as FabricCombo,
CUT2. Cutno,CUT2. MarkerName,CUT2. Markerlength,CUT2. Layer,CUT2. Cons,
CUT2 .UKey, CUT2. WorkOrderUkey,CUT2. Colorid
FROM Pms_To_Trade.dbo.CuttingOutput CUT1 , Production.dbo.CuttingOutput_Detail CUT2  
WHERE CUT1. ID = CUT2.ID
)as aaa

SELECT CUT1.ID,CUT3.CuttingOutput_detailUkey,CUT3.CuttingID, CUT3.SizeCode,CUT3.Qty
INTO  CuttingOutput_Detail_Detail
FROM Pms_To_Trade.dbo.CuttingOutput CUT1, Production.dbo.CuttingOutput_Detail_Detail CUT3 
WHERE CUT1. ID = CUT3. ID

Select distinct orderid = o.ID,wd.SizeCode,wd.article
into #tmp1
from Production.dbo.Orders o WITH (NOLOCK)
inner join Production.dbo.WorkOrder_Distribute wd  on o.id = wd.OrderID
inner join Production.dbo.Order_ColorCombo occ on o.poid = occ.id and occ.Article = wd.Article
inner join Production.dbo.order_Eachcons cons on occ.id = cons.id and cons.FabricCombo = occ.PatternPanel and cons.CuttingPiece='0'
inner join Production.dbo.CuttingOutput_Detail cud on cud.WorkOrderUkey = wd.WorkOrderUkey
inner join Pms_To_Trade.dbo.CuttingOutput CUT1 on CUT1.ID = cud.ID
where occ.FabricCode !='' and occ.FabricCode is not null 

select WIP.OrderID,WIP.Article,WIP.Size,WIP.Qty
into CuttingOutput_WIP
FROM #tmp1 t, Production.dbo.CuttingOutput_WIP WIP 
WHERE  t.orderid = WIP.OrderID and t.Article = WIP.Article and t.SizeCode = WIP.Size
drop table #tmp1

select wd.WorkOrderUkey,wd.ID,wd.OrderID,wd.Article,wd.SizeCode,wd.Qty
into Workorder_Distribute
from Production.dbo.Workorder_Distribute wd
inner join Pms_To_Trade.dbo.CuttingOutput_Detail cud on cud.WorkOrderUkey = wd.WorkOrderUkey


END




