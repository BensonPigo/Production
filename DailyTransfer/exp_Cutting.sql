
CREATE PROCEDURE [dbo].[exp_Cutting] 

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

------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
declare @DateInfoName varchar(30) ='CUTTING';
declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
if @DateStart is Null
	set @DateStart= (SELECT DATEADD(DAY,1,SewLock) FROM Production.dbo.System)
if @DateEnd is Null
	set @DateEnd = CONVERT(DATE, GETDATE())	

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

SELECT ID, cDATE, MDivisionid,MANPOWER, MANHOURS, Actoutput, ActGarment, AddName, AddDate, EditName, EditDate
INTO CuttingOutput
 FROM Production.dbo.CuttingOutput  CUT1
WHERE CUT1. Status != 'New' 
AND  (CUT1.Lock  BETWEEN @DateStart AND  @DateEnd or  CUT1.Lock IS NULL)


SELECT
    d.ID,
    d.CutRef,
    d.CuttingID,
    ISNULL(w.FabricCombo, '') AS FabricCombo,
    d.Cutno,
    d.MarkerName,
    d.Markerlength,
    d.Layer,
    d.Cons,
    d.UKey,
    WorkOrderUkey = d.WorkOrderForOutputUkey,
    d.Colorid
INTO dbo.CuttingOutput_Detail
FROM Pms_To_Trade.dbo.CuttingOutput AS o
INNER JOIN Production.dbo.CuttingOutput_Detail AS d　ON o.ID = d.ID
LEFT JOIN Production.dbo.WorkOrderForOutput AS w　ON w.UKey = d.WorkOrderForOutputUkey;

SELECT CUT1.ID,CUT3.CuttingOutput_detailUkey,CUT3.CuttingID, CUT3.SizeCode,CUT3.Qty
INTO  CuttingOutput_Detail_Detail
FROM Pms_To_Trade.dbo.CuttingOutput CUT1, Production.dbo.CuttingOutput_Detail_Detail CUT3 
WHERE CUT1. ID = CUT3. ID

-- Only for CuttingOutput_WIP
select WIP.OrderID,WIP.Article,WIP.Size,WIP.Qty
into CuttingOutput_WIP
FROM Production.dbo.CuttingOutput_WIP WIP 
where WIP.EditDate between DATEADD(DAY,-7, GETDATE()) and GETDATE()


select WorkOrderUkey = wd.WorkorderForOutputUkey,wd.ID,wd.OrderID,wd.Article,wd.SizeCode,wd.Qty
into Workorder_Distribute
from Production.dbo.WorkorderForOutput_Distribute wd
inner join Pms_To_Trade.dbo.CuttingOutput_Detail cud on cud.WorkOrderUkey = wd.WorkorderForOutputUkey


END




