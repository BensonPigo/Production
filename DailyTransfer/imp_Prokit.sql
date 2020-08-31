
Create PROCEDURE imp_Prokit	
AS
BEGIN
	SET NOCOUNT ON;
	
------------------------------------------------------------------------------------------------------
--***資料交換的條件限制***
--1. 優先取得Production.dbo.DateInfo
	declare @DateInfoName varchar(30) ='ProductionKits';
	declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
	declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
	declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.取得預設值
	if @DateStart is Null
		set @DateStart= (Select DateStart From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)
	if @DateEnd is Null
		set @DateEnd = (Select DateEnd From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)

--3.更新Pms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

----------------------刪除主TABLE多的資料
Delete Production.dbo.Style_ProductionKits
from Production.dbo.Style_ProductionKits as a left join Trade_To_Pms.dbo.Style_ProductionKits as b
on a.Ukey = b.Ukey
where b.Ukey is null
and ((a.AddDate between @DateStart and @DateEnd) 
or (a.EditDate between @DateStart and @DateEnd))

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
a.StyleUkey	= b.StyleUkey
,a.ProductionKitsGroup = b.ProductionKitsGroup
,a.MDivisionID = c.MDivisionID
,a.FactoryID = b.FactoryID
,a.Article = b.Article
,a.DOC = b.DOC
,a.SendDate = b.SendDate
--,a.ReceiveDate = b.ReceiveDate
,a.ProvideDate = b.ProvideDate
,a.SendName = b.SendName
--,a.FtyHandle = b.FtyHandle
,a.MRHandle = b.MRHandle
,a.SMR = b.SMR
,a.PoHandle = b.PoHandle
,a.POSMR = b.POSMR
,a.OrderId = b.OrderId
,a.SCIDelivery = b.SCIDelivery
,a.IsPF = b.IsPF
,a.BuyerDelivery = b.BuyerDelivery
,a.AddOrderId = b.AddOrderId
,a.AddSCIDelivery = b.AddSCIDelivery
,a.AddIsPF = b.AddIsPF
,a.AddBuyerDelivery = b.AddBuyerDelivery
,a.MRLastDate = b.MRLastDate
--,a.FtyLastDate = b.FtyLastDate
,a.MRRemark = b.MRRemark
--,a.FtyRemark = b.FtyRemark
,a.FtyList = b.FtyList
,a.ReasonID = b.ReasonID
--,a.SendToQA = b.SendToQA
--,a.QAReceived = b.QAReceived
,a.StyleCUkey1_Old = b.StyleCUkey1_Old
,a.[AddName]=b.[AddName]
,a.[AddDate]=b.[AddDate]
,a.[EditName]=b.[EditName]
,a.[EditDate]=b.[EditDate]
from Production.dbo.Style_ProductionKits as a 
inner join Trade_To_Pms.dbo.Style_ProductionKits as b ON a.ukey=b.ukey --AND a.FactoryID=b.FactoryID
left join Trade_To_Pms.dbo.Factory as c ON c.ID=b.FactoryID
-------------------------- INSERT INTO 抓
RAISERROR('imp_Style - Starts',0,0)
INSERT INTO Production.dbo.Style_ProductionKits(
Ukey
,StyleUkey
,ProductionKitsGroup
,MDivisionID
,FactoryID
,Article
,DOC
,SendDate
,ReceiveDate
,ProvideDate
,SendName
,FtyHandle
,MRHandle
,SMR
,PoHandle
,POSMR
,OrderId
,SCIDelivery
,IsPF
,BuyerDelivery
,AddOrderId
,AddSCIDelivery
,AddIsPF
,AddBuyerDelivery
,MRLastDate
,FtyLastDate
,MRRemark
,FtyRemark
,FtyList
,ReasonID
--,SendToQA
--,QAReceived
,StyleCUkey1_Old
,[AddName]
,[AddDate]
,[EditName]
,[EditDate])

select 
b.Ukey
,b.StyleUkey
,b.ProductionKitsGroup
,c.MDivisionID
,b.FactoryID
,b.Article
,b.DOC
,b.SendDate
,b.ReceiveDate
,b.ProvideDate
,b.SendName
,b.FtyHandle
,b.MRHandle
,b.SMR
,b.PoHandle
,b.POSMR
,b.OrderId
,b.SCIDelivery
,b.IsPF
,b.BuyerDelivery
,b.AddOrderId
,b.AddSCIDelivery
,b.AddIsPF
,b.AddBuyerDelivery
,b.MRLastDate
,b.FtyLastDate
,b.MRRemark
,b.FtyRemark
,b.FtyList
,b.ReasonID
--,SendToQA
--,QAReceived
,b.StyleCUkey1_Old
,b.[AddName]
,b.[AddDate]
,b.[EditName]
,b.[EditDate]
from Trade_To_Pms.dbo.Style_ProductionKits as b WITH (NOLOCK)
left join Trade_To_Pms.dbo.Factory as c WITH (NOLOCK) ON c.ID=b.FactoryID
where not exists(select 1 from Production.dbo.Style_ProductionKits as a WITH (NOLOCK) where a.ukey=b.ukey-- AND a.FactoryID=b.FactoryID
)
	and b.FactoryID in (select id from Production.dbo.Factory WITH (NOLOCK) where IsProduceFty=1)



  
END

