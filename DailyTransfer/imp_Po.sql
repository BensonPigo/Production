
-- =============================================
-- Author:		LEO
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE imp_Po
	---- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT b.*
INTO #Trade_To_Pms_PO --���U�����PO�����u�t�O
 FROM  Trade_To_Pms.dbo.PO b inner join Production.dbo.Factory c on b.FactoryID = c.ID

--PO1 PO
--PMS�h��
--,[FIRRemark]
--,[AIRemark]
--,[FIRLaboratoryRemark]
--,[AIRLaboratoryRemark]
--,[OvenLaboratoryRemark]
--,[ColorFastnessLaboratoryRemark]
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       a.ID	    =b.ID	
      ,a.StyleID	      =b.StyleID	
      ,a.SeasonId	      =b.SeasonId	
      ,a.StyleUkey	      =b.StyleUkey	
      ,a.BrandID	      =b.BrandID	
      ,a.POSMR	      =b.POSMR	
      ,a.POHandle	      =b.POHandle	
      ,a.PCHandle	      =b.PCHandle	
      ,a.PCSMR	      =b.PCSMR	
      ,a.McHandle	      =b.McHandle	
      ,a.ShipMark	      =b.ShipMark	
      ,a.FTYMark	      =b.FTYMark	
      ,a.Complete	      =b.Complete	
      ,a.PoRemark	      =b.PoRemark	
      ,a.CostRemark	      =b.CostRemark	
      ,a.IrregularRemark	      =b.IrregularRemark	
      ,a.FirstPoError	      =b.FirstPoError	
      ,a.FirstEditName	      =b.FirstEditName	
      ,a.FirstEditDate	      =b.FirstEditDate	
      ,a.FirstAddDate	      =b.FirstAddDate	
      ,a.FirstCostDate	      =b.FirstCostDate	
      ,a.LastPoError	      =b.LastPoError	
      ,a.LastEditName	      =b.LastEditName	
      ,a.LastEditDate	      =b.LastEditDate	
      ,a.LastAddDate	      =b.LastAddDate	
      ,a.LastCostDate	      =b.LastCostDate	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.MTLDelay	      =b.MtlDelay
from Production.dbo.PO as a inner join #Trade_To_Pms_PO as b ON a.id=b.id
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.PO(
       ID
      ,StyleID
      ,SeasonId
      ,StyleUkey
      ,BrandID
      ,POSMR
      ,POHandle
      ,PCHandle
      ,PCSMR
      ,McHandle
      ,ShipMark
      ,FTYMark
      ,Complete
      ,PoRemark
      ,CostRemark
      ,IrregularRemark
      ,FirstPoError
      ,FirstEditName
      ,FirstEditDate
      ,FirstAddDate
      ,FirstCostDate
      ,LastPoError
      ,LastEditName
      ,LastEditDate
      ,LastAddDate
      ,LastCostDate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,MTLDelay

)
select 
       ID
     , StyleID
      ,SeasonId
      ,StyleUkey
      ,BrandID
      ,POSMR
      ,POHandle
      ,PCHandle
      ,PCSMR
      ,McHandle
      ,ShipMark
      ,FTYMark
      ,Complete
      ,PoRemark
      ,CostRemark
      ,IrregularRemark
      ,FirstPoError
      ,FirstEditName
      ,FirstEditDate
      ,FirstAddDate
      ,FirstCostDate
      ,LastPoError
      ,LastEditName
      ,LastEditDate
      ,LastAddDate
      ,LastCostDate
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,MTLDelay

from #Trade_To_Pms_PO as b
where not exists(
select id from Production.dbo.PO as a where a.id = b.id )

--PO2

------------------------------------------------------------------PO2 START
----------------------�R���DTABLE�h�����
Delete Production.dbo.PO_Supp
from Production.dbo.PO_Supp as a left join Trade_To_Pms.dbo.PO_Supp as b 
on a.id = b.id and a.SEQ1=b.SEQ1
where b.id is null
and  a.id in (select id from #Trade_To_Pms_PO)
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       a.ID	          =b.ID	
      ,a.SEQ1	      =b.SEQ1	
      ,a.SuppID	      =b.SuppID	
      ,a.Remark	      =b.Remark	
      ,a.Description	      =b.Description	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	

from Production.dbo.PO_Supp as a inner join Trade_To_Pms.dbo.PO_Supp as b ON a.id=b.id and a.SEQ1=b.SEQ1
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID


-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.PO_Supp(
       ID
      ,SEQ1
      ,SuppID
      ,Remark
      ,Description
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate

)
select 
       b.ID
      ,SEQ1
      ,SuppID
      ,Remark
      ,Description
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate

from Trade_To_Pms.dbo.PO_Supp as b inner join  #Trade_To_Pms_PO c ON b.ID = c.ID
where not exists(select id from Production.dbo.PO_Supp as a where a.id = b.id and a.SEQ1=b.SEQ1)


------------------------------------------------------------------PO2 END
------------------------------------------------------------------PO3 START
--Po3 pms�h�����
--,[BrandId] ,[ColorID_Old]
--,[BomFactory]
--      ,[BomCountry]
--      ,[BomStyle]
--      ,[BomCustCD]
--      ,[BomArticle]
--,[BomBuymonth]
--,[StockUnit]
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.ID	     =b.ID	
      --,a.Seq1	      =b.Seq1	
      --,a.Seq2	      =b.Seq2	
      a.FactoryID	      =(select top 1 a.FactoryID from Orders a where a.POID=b.ID)
      ,a.RefNo	      =b.RefNo	
      ,a.SCIRefNo	      =b.SCIRefNo	
      ,a.FabricType	      =b.FabricType	
      ,a.Price	      =b.Price	
      ,a.UsedQty	      =b.UsedQty	
      ,a.Qty	      =b.Qty	
      ,a.POUnit	      =b.POUnit	
      ,a.Complete	      =b.Complete	
      ,a.SystemETD	      =b.SystemETD	
      ,a.CFMETD	      =b.CFMETD	
      ,a.RevisedETA	      =b.RevisedETA	
      ,a.FinalETD	      =b.FinalETD	
      ,a.ShipETA	      =b.ShipETA	
      ,a.ETA	      =b.EstETA	
      ,a.FinalETA	      =b.FinalETA	
      ,a.ShipModeID	      =b.ShipModeID	
      ,a.SMRLock	      =b.SMRLock	
      ,a.SystemLock	      =b.SystemLock	
      ,a.PrintDate	      =b.PrintDate	
      ,a.PINO	      =b.PINO	
      ,a.PIDate	      =b.PIDate	
      ,a.ColorID	      =b.ColorID	
      ,a.SuppColor	      =b.SuppColor	
      ,a.SizeSpec	      =b.SizeSpec	
      ,a.SizeUnit	      =b.SizeUnit	
      ,a.Remark	      =b.Remark	
      ,a.Special	      =b.Special	
      ,a.Width	      =b.Width	
      ,a.StockQty	      =b.StockQty	
      ,a.NetQty	      =b.NetQty	
      ,a.LossQty	      =b.LossQty	
      ,a.SystemNetQty	      =b.SystemNetQty	
      ,a.StockPOID	      =b.StockPOID	
      ,a.StockSeq1	      =b.StockSeq1	
      ,a.StockSeq2	      =b.StockSeq2	
      ,a.InventoryUkey	      =b.InventoryUkey	
      ,a.OutputSeq1	      =b.OutputSeq1	
      ,a.OutputSeq2	      =b.OutputSeq2	
      ,a.SystemCreate	      =b.SystemCreate	
      ,a.FOC	      =b.FOC	
      ,a.Junk	      =b.Junk	
      ,a.ColorDetail	      =b.ColorDetail	
      ,a.BomZipperInsert	      =b.BomZipperInsert	
      ,a.BomCustPONo	      =b.BomCustPONo	
      ,a.ShipQty	      =b.ShipQty	
      ,a.Shortage	      =b.Shortage	
      ,a.ShipFOC	      =b.ShipFOC	
      ,a.ApQty	      =b.ApQty	
      ,a.Spec	      =b.Spec	
      ,a.InputQty	      =b.InputQty	
      ,a.OutputQty	      =b.OutputQty	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
	  ,a.RevisedETD = b.RevisedETD
	  ,a.CfmETA =b.CfmETA,
	   a.BrandId = (select top 1 a.BrandID from Orders a where a.POID=b.ID)
	

from Production.dbo.PO_Supp_Detail as a 
inner join Trade_To_Pms.dbo.PO_Supp_Detail as b ON a.id=b.id and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID 



-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.PO_Supp_Detail(
ID
      ,Seq1
      ,Seq2
      ,FactoryID
      ,RefNo
      ,SCIRefNo
      ,FabricType
      ,Price
      ,UsedQty
      ,Qty
      ,POUnit
      ,Complete
      ,SystemETD
      ,CFMETD
      ,RevisedETA
      ,FinalETD
      ,ShipETA
      ,ETA
      ,FinalETA
      ,ShipModeID
      ,SMRLock
      ,SystemLock
      ,PrintDate
      ,PINO
      ,PIDate
      ,ColorID
      ,SuppColor
      ,SizeSpec
      ,SizeUnit
      ,Remark
      ,Special
      ,Width
      ,StockQty
      ,NetQty
      ,LossQty
      ,SystemNetQty
      ,StockPOID
      ,StockSeq1
      ,StockSeq2
      ,InventoryUkey
      ,OutputSeq1
      ,OutputSeq2
      ,SystemCreate
      ,FOC
      ,Junk
      ,ColorDetail
      ,BomZipperInsert
      ,BomCustPONo
      ,ShipQty
      ,Shortage
      ,ShipFOC
      ,ApQty
      ,Spec
      ,InputQty
      ,OutputQty
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
	  ,RevisedETD
	  ,CfmETA 
	  ,BrandId
)
select 
       b.ID
      ,Seq1
      ,Seq2
      ,(select top 1 a.FactoryID from Orders a where a.POID=b.ID)
      ,RefNo
      ,SCIRefNo
      ,FabricType
      ,Price
      ,UsedQty
      ,Qty
      ,POUnit
      ,b.Complete
      ,SystemETD
      ,CFMETD
      ,RevisedETA
      ,FinalETD
      ,ShipETA
      ,EstETA
      ,FinalETA
      ,ShipModeID
      ,SMRLock
      ,SystemLock
      ,PrintDate
      ,PINO
      ,PIDate
      ,ColorID
      ,SuppColor
      ,SizeSpec
      ,SizeUnit
      ,Remark
      ,Special
      ,Width
      ,StockQty
      ,NetQty
      ,LossQty
      ,SystemNetQty
      ,StockPOID
      ,StockSeq1
      ,StockSeq2
      ,InventoryUkey
      ,OutputSeq1
      ,OutputSeq2
      ,SystemCreate
      ,FOC
      ,Junk
      ,ColorDetail
      ,BomZipperInsert
      ,BomCustPONo
      ,ShipQty
      ,Shortage
      ,ShipFOC
      ,ApQty
      ,Spec
      ,InputQty
      ,OutputQty
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate
	  ,b.RevisedETD
	  ,b.CfmETA 
	  ,(select top 1 a.BrandID from Orders a where a.POID=b.ID)
from Trade_To_Pms.dbo.PO_Supp_Detail as b inner join  #Trade_To_Pms_PO c ON b.ID = c.ID
where not exists(select id from Production.dbo.PO_Supp_Detail as a where a.id = b.id and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2	)

----------------------�R���DTABLE�h�����
Delete Production.dbo.PO_Supp_Detail
from Production.dbo.PO_Supp_Detail as a left join Trade_To_Pms.dbo.PO_Supp_Detail as b 
on a.id = b.id and a.SEQ1=b.Seq1 and a.SEQ2=b.Seq2
where b.id is null
and  a.id in (select id from #Trade_To_Pms_PO)
and a.InputQty = 0

UPDATE a
SET  
Junk = 1,
QTY = 0
from Production.dbo.PO_Supp_Detail as a inner join #Trade_To_Pms_PO as b ON a.id=b.id
where not exists(select id from Trade_To_Pms.dbo.PO_Supp_Detail as c where a.id = c.id)
and InputQty <> 0


------------------------------------------------------------------PO3 END
------------------------------------------------------



------------------------------------------------------------------PO4 START
--PO4
----------------------�R���DTABLE�h�����
Delete Production.dbo.PO_Supp_Detail_OrderList
from Production.dbo.PO_Supp_Detail_OrderList as a left join Trade_To_Pms.dbo.PO_Supp_Detail_OrderList as b 
on a.id = b.id and a.SEQ1 = b.SEQ1 and a.SEQ2 = b.SEQ2 and a.OrderID=b.OrderID
where b.id is null
and  a.id in (select id from #Trade_To_Pms_PO)
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       --a.ID	     =b.ID		
      --,a.SEQ1	             =b.SEQ1		
      --,a.SEQ2	      =b.SEQ2		
      --,a.OrderID	      =b.OrderID		
      a.AddName	      =b.AddName		
      ,a.AddDate	      =b.AddDate		
      ,a.EditName	      =b.EditName		
      ,a.EditDate	      =b.EditDate		

from Production.dbo.PO_Supp_Detail_OrderList as a inner join Trade_To_Pms.dbo.PO_Supp_Detail_OrderList as b 
ON a.id=b.id and a.SEQ1 = b.SEQ1 and a.SEQ2 = b.SEQ2 and a.OrderID=b.OrderID
inner join  #Trade_To_Pms_PO c ON b.ID = c.ID


-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.PO_Supp_Detail_OrderList(
       ID
      ,SEQ1
      ,SEQ2
      ,OrderID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate


)
select 
       b.ID
      ,SEQ1
      ,SEQ2
      ,OrderID
      ,b.AddName
      ,b.AddDate
      ,b.EditName
      ,b.EditDate


from Trade_To_Pms.dbo.PO_Supp_Detail_OrderList as b inner join  #Trade_To_Pms_PO c ON b.ID = c.ID 
where not exists(select id from Production.dbo.PO_Supp_Detail_OrderList as a where a.id = b.id and a.SEQ1 = b.SEQ1 and a.SEQ2 = b.SEQ2 and a.OrderID=b.OrderID)

------------------------------------------------------------------PO4 END
------------------------------------------------------

------------�̫�n�M�Ŧh��TEMP TABLE
drop table #Trade_To_Pms_PO 
--Fabric
--PMS�h�����
--,[NLCode]
--      ,[HSCode]
--      ,[CustomsUnit]
--      ,[PcsWidth]
--      ,[PcsLength]
--      ,[PcsKg]
--      ,[NoDeclare]
--      ,[NLCodeEditName]
--      ,[NLCodeEditDate]

---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       -- a.SCIRefno	 =b.SCIRefno
      a.BrandID	      =b.BrandID
      ,a.Refno	      =b.Refno
      ,a.Width	      =b.Width
      ,a.Junk	      =b.Junk
      ,a.Type	      =b.Type
      ,a.MtltypeId	      =b.MtltypeId
      ,a.BomTypeCalculate	      =b.BomTypeCalculate
      ,a.Description	      =b.Description
      ,a.DescDetail	      =b.DescDetail
      ,a.LossType	      =b.LossType
      ,a.LossPercent	      =b.LossPercent
      ,a.LossQty	      =b.LossQty
      ,a.LossStep	      =b.LossStep
      ,a.UsageUnit	      =b.UsageUnit
      ,a.Weight	      =b.Weight
      ,a.WeightM2	      =b.WeightM2
      ,a.CBMWeight	      =b.CBMWeight
      ,a.CBM	      =b.CBM
      ,a.NoSizeUnit	      =b.NoSizeUnit
      ,a.BomTypeSize	      =b.BomTypeSize
      ,a.BomTypeColor	      =b.BomTypeColor
      ,a.ConstructionID	      =b.ConstructionID
      ,a.MatchFabric	      =b.MatchFabric
      ,a.WeaveTypeID	      =b.WeaveTypeID
      ,a.AddName	      =b.AddName
      ,a.AddDate	      =b.AddDate
      ,a.EditName	      =b.EditName
      ,a.EditDate	      =b.EditDate
from Production.dbo.Fabric as a inner join Trade_To_Pms.dbo.Fabric as b ON a.SCIRefno=b.SCIRefno
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Fabric(
       SCIRefno
      ,BrandID
      ,Refno
      ,Width
      ,Junk
      ,Type
      ,MtltypeId
      ,BomTypeCalculate
      ,Description
      ,DescDetail
      ,LossType
      ,LossPercent
      ,LossQty
      ,LossStep
      ,UsageUnit
      ,Weight
      ,WeightM2
      ,CBMWeight
      ,CBM
      ,NoSizeUnit
      ,BomTypeSize
      ,BomTypeColor
      ,ConstructionID
      ,MatchFabric
      ,WeaveTypeID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
      SCIRefno
      ,BrandID
      ,Refno
      ,Width
      ,Junk
      ,Type
      ,MtltypeId
      ,BomTypeCalculate
      ,Description
      ,DescDetail
      ,LossType
      ,LossPercent
      ,LossQty
      ,LossStep
      ,UsageUnit
      ,Weight
      ,WeightM2
      ,CBMWeight
      ,CBM
      ,NoSizeUnit
      ,BomTypeSize
      ,BomTypeColor
      ,ConstructionID
      ,MatchFabric
      ,WeaveTypeID
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.Fabric as b
where not exists(select SCIRefno from Production.dbo.Fabric as a where a.SCIRefno = b.SCIRefno)


--Fab_Content
--Fabric_Content

----------------------�R���DTABLE�h�����
Delete Production.dbo.Fabric_Content
from Production.dbo.Fabric_Content as a left join Trade_To_Pms.dbo.Fabric_Content as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
       a.SCIRefno	    =b.SCIRefno	
      --,a.Ukey	      =b.Ukey	
      ,a.Layerno	      =b.Layerno	
      ,a.percentage	      =b.percentage	
      ,a.MtltypeId	      =b.MtltypeId	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.OldSys_GroupKey	      =b.OldSys_GroupKey	

from Production.dbo.Fabric_Content as a inner join Trade_To_Pms.dbo.Fabric_Content as b ON a.Ukey=b.Ukey
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Fabric_Content(
       SCIRefno
      ,Ukey
      ,Layerno
      ,percentage
      ,MtltypeId
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_GroupKey
)
select 
      SCIRefno
      ,Ukey
      ,Layerno
      ,percentage
      ,MtltypeId
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_GroupKey
from Trade_To_Pms.dbo.Fabric_Content as b
where not exists(select Ukey from Production.dbo.Fabric_Content as a where a.Ukey = b.Ukey)








--FabricTax
--Fabric_HsCode
----------------------�R���DTABLE�h�����
Delete Production.dbo.Fabric_HsCode
from Production.dbo.Fabric_HsCode as a left join Trade_To_Pms.dbo.Fabric_HsCode as b
on a.SCIRefno = b.SCIRefno and  a.SuppID=b.SuppID and a.Year =b.Year
where b.SCIRefno is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
      -- a.SCIRefno	     =b.SCIRefno	
      a.Ukey	      =b.Ukey	
      --,a.SuppID	      =b.SuppID	
      --,a.Year	      =b.Year	
      ,a.HsCode	      =b.HsCode	
      ,a.ImportDuty	      =b.ImportDuty	
      ,a.ECFADuty	      =b.ECFADuty	
      ,a.ASEANDuty	      =b.ASEANDuty	
      ,a.AddName	      =b.AddName	
      ,a.AddDate	      =b.AddDate	
      ,a.EditName	      =b.EditName	
      ,a.EditDate	      =b.EditDate	
      ,a.OldSys_Ukey	      =b.OldSys_Ukey	
      ,a.OldSys_Ver	      =b.OldSys_Ver	

from Production.dbo.Fabric_HsCode as a inner join Trade_To_Pms.dbo.Fabric_HsCode as b ON a.SCIRefno=b.SCIRefno and  a.SuppID=b.SuppID and a.Year =b.Year
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Fabric_HsCode(
       SCIRefno
      ,Ukey
      ,SuppID
      ,Year
      ,HsCode
      ,ImportDuty
      ,ECFADuty
      ,ASEANDuty
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_Ukey
      ,OldSys_Ver

)
select
       SCIRefno
      ,Ukey
      ,SuppID
      ,Year
      ,HsCode
      ,ImportDuty
      ,ECFADuty
      ,ASEANDuty
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,OldSys_Ukey
      ,OldSys_Ver

from Trade_To_Pms.dbo.Fabric_HsCode as b
where not exists(select SCIRefno from Production.dbo.Fabric_HsCode as a where a.SCIRefno = b.SCIRefno and  a.SuppID=b.SuppID and a.Year =b.Year)




END

