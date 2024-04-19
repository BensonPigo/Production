
Create PROCEDURE imp_Prokit	
AS
BEGIN
	SET NOCOUNT ON;

	---- Check TransferDate before everything
	IF NOT  EXISTS(
		select 1 from Trade_To_PMS..DateInfo 
		where Name = 'TransferDate'
		AND DateStart in (CAST(DATEADD(DAY,-1,GETDATE()) AS date), CAST(GETDATE() AS DATE))
	)
	BEGIN
		-- �ߥX���~
		RAISERROR ('The DB transferdate is wrong. Trade_To_PMS..DateInfo  �����s�b�ŦX���� TransferDate �O���C', 16, 1); -- 16�O���~���Y���{�סA1�O���~���A	
		RETURN; 
	END
	
	
------------------------------------------------------------------------------------------------------
--***��ƥ洫�����󭭨�***
--1. �u�����oProduction.dbo.DateInfo
	declare @DateInfoName varchar(30) ='ProductionKits';
	declare @DateStart date= (select DateStart from Production.dbo.DateInfo where name = @DateInfoName);
	declare @DateEnd date  = (select DateEnd   from Production.dbo.DateInfo where name = @DateInfoName);
	declare @Remark nvarchar(max) = (select Remark from Production.dbo.DateInfo where name = @DateInfoName);

--2.���o�w�]��
	if @DateStart is Null
		set @DateStart= (Select DateStart From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)
	if @DateEnd is Null
		set @DateEnd = (Select DateEnd From Trade_To_Pms.dbo.DateInfo Where Name = @DateInfoName)

--3.��sPms_To_Trade.dbo.dateInfo
if exists(select 1 from Pms_To_Trade.dbo.dateInfo where Name = @DateInfoName )
	update Pms_To_Trade.dbo.dateInfo  set DateStart = @DateStart,DateEnd = @DateEnd, Remark=@Remark where Name = @DateInfoName 
else
	Insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd,Remark)
	values (@DateInfoName,@DateStart,@DateEnd,@Remark);
------------------------------------------------------------------------------------------------------

----------------------�R���DTABLE�h�����
Delete Production.dbo.Style_ProductionKits
from Production.dbo.Style_ProductionKits as a left join Trade_To_Pms.dbo.Style_ProductionKits as b
on a.Ukey = b.Ukey
where b.Ukey is null
and ((a.AddDate between @DateStart and @DateEnd) 
or (a.EditDate between @DateStart and @DateEnd))

---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
RAISERROR('imp_Style - Starts',0,0)
UPDATE a
SET  
a.StyleUkey	= isnull( b.StyleUkey                     ,0)
,a.ProductionKitsGroup = isnull( b.ProductionKitsGroup,'')
,a.MDivisionID = isnull( c.MDivisionID                ,'')
,a.FactoryID = isnull( b.FactoryID                    ,'')
,a.Article = isnull( b.Article                        ,'')
,a.DOC = isnull( b.DOC                                ,'')
,a.SendDate =  b.SendDate
,a.ProvideDate =  b.ProvideDate
,a.SendName = isnull( b.SendName                      ,'')
,a.MRHandle = isnull( b.MRHandle                      ,'')
,a.SMR = isnull( b.SMR                                ,'')
,a.PoHandle = isnull( b.PoHandle                      ,'')
,a.POSMR = isnull( b.POSMR                            ,'')
,a.OrderId = isnull( b.OrderId                        ,'')
,a.SCIDelivery = b.SCIDelivery
,a.IsPF = isnull( b.IsPF                              ,0)
,a.BuyerDelivery = b.BuyerDelivery
,a.AddOrderId = isnull( b.AddOrderId                  ,'')
,a.AddSCIDelivery = b.AddSCIDelivery
,a.AddIsPF = isnull( b.AddIsPF                        ,0)
,a.AddBuyerDelivery = b.AddBuyerDelivery
,a.MRLastDate = b.MRLastDate
,a.MRRemark = isnull(b.MRRemark, '')
,a.FtyList = isnull(b.FtyList, '')
,a.ReasonID = isnull(b.ReasonID, '')
,a.[AddName]=isnull(b.[AddName], '')
,a.[AddDate]=b.[AddDate]
,a.[EditName]=isnull(b.[EditName], '')
,a.[EditDate]=b.[EditDate]
from Production.dbo.Style_ProductionKits as a 
inner join Trade_To_Pms.dbo.Style_ProductionKits as b ON a.ukey=b.ukey --AND a.FactoryID=b.FactoryID
left join Trade_To_Pms.dbo.Factory as c ON c.ID=b.FactoryID
-------------------------- INSERT INTO ��
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
,[AddName]
,[AddDate]
,[EditName]
,[EditDate])

select 
b.Ukey
,b.StyleUkey
,isnull(b.ProductionKitsGroup, '')
,isnull(c.MDivisionID        , '')
,isnull(b.FactoryID          , '')
,isnull(b.Article            , '')
,isnull(b.DOC                , '')
,b.SendDate
,b.ReceiveDate
,b.ProvideDate
,isnull(b.SendName           , '')
,isnull(b.FtyHandle          , '')
,isnull(b.MRHandle           , '')
,isnull(b.SMR                , '')
,isnull(b.PoHandle           , '')
,isnull(b.POSMR              , '')
,isnull(b.OrderId            , '')
,b.SCIDelivery
,isnull(b.IsPF               , 0)
,b.BuyerDelivery
,isnull(b.AddOrderId         , '')
,b.AddSCIDelivery
,isnull(b.AddIsPF            , 0)
,b.AddBuyerDelivery
,b.MRLastDate
,b.FtyLastDate
,isnull(b.MRRemark           , '')
,isnull(b.FtyRemark          , '')
,isnull(b.FtyList            , '')
,isnull(b.ReasonID           , '')
,isnull(b.[AddName]          , '')
,b.[AddDate]
,isnull(b.[EditName]         , '')
,b.[EditDate]
from Trade_To_Pms.dbo.Style_ProductionKits as b WITH (NOLOCK)
left join Trade_To_Pms.dbo.Factory as c WITH (NOLOCK) ON c.ID=b.FactoryID
where not exists(select 1 from Production.dbo.Style_ProductionKits as a WITH (NOLOCK) where a.ukey=b.ukey-- AND a.FactoryID=b.FactoryID
)
	and b.FactoryID in (select id from Production.dbo.Factory WITH (NOLOCK) where IsProduceFty=1)
END

