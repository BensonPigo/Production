-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE imp_Inv
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   -- INVENTORY
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       a.Ukey	       =b.Ukey
      --,a.POID	      =b.POID
      --,a.Seq1	      =b.Seq1
     -- ,a.Seq2	      =b.Seq2
      --,a.ProjectID	      =b.ProjectID
     -- ,a.FactoryID	      =b.FactoryID
     -- ,a.UnitID	      =b.UnitID
     -- ,a.InventoryRefnoId	      =b.InventoryRefnoId
      ,a.Refno	      =b.Refno
      ,a.BrandGroup	      =b.BrandGroup
      ,a.BrandID	      =b.BrandID
      ,a.LimitHandle	      =b.LimitHandle
      ,a.LimitSmr	      =b.LimitSmr
      ,a.AuthMr	      =b.AuthMr
      ,a.Payable	      =b.Payable
      ,a.Qty	      =b.Qty
      ,a.InputQty	      =b.InputQty
      ,a.OutputQty	      =b.OutputQty
      ,a.Deadline	      =b.Deadline
      ,a.PoFactory	      =b.PoFactory
      ,a.OrderHandle	      =b.OrderHandle
      ,a.OrderSmr	      =b.OrderSmr
      ,a.PoHandle	      =b.PoHandle
      ,a.PoSmr	      =b.PoSmr
      ,a.StyleID	      =b.StyleID
      ,a.SeasonID	      =b.SeasonID
      ,a.FabricType	      =b.FabricType
      ,a.MtlTypeID	      =b.MtlTypeID
      ,a.ReasonID	      =b.ReasonID
      ,a.Remark	      =b.Remark
      ,a.IcrNo	      =b.IcrNo
      ,a.DebitID	      =b.DebitID
      ,a.Lock	      =b.Lock
      ,a.AddName	      =b.AddName
      ,a.AddDate	      =b.AddDate
      ,a.EditName	      =b.EditName
      ,a.EditDate	      =b.EditDate
      ,a.ETA	      =b.ETA
	  ,a.SCIRefno = b.SCIRefno
from Production.dbo.Inventory as a 
inner join Trade_To_Pms.dbo.Inventory as b ON a.POID=b.POID and a.Seq1=b.Seq1 and a.Seq2=b.Seq2 and a.FactoryID=b.FactoryID and a.UnitID=b.UnitID and a.ProjectID=b.ProjectID and a.InventoryRefnoId=b.InventoryRefnoId
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Inventory(
       Ukey
      ,POID
      ,Seq1
      ,Seq2
      ,ProjectID
      ,FactoryID
      ,UnitID
      ,InventoryRefnoId
      ,Refno
      ,BrandGroup
      ,BrandID
      ,LimitHandle
      ,LimitSmr
      ,AuthMr
      ,Payable
      ,Qty
      ,InputQty
      ,OutputQty
      ,Deadline
      ,PoFactory
      ,OrderHandle
      ,OrderSmr
      ,PoHandle
      ,PoSmr
      ,StyleID
      ,SeasonID
      ,FabricType
      ,MtlTypeID
      ,ReasonID
      ,Remark
      ,IcrNo
      ,DebitID
      ,Lock
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,ETA
	  ,SCIRefno
)
select 
       Ukey
      ,POID
      ,Seq1
      ,Seq2
      ,ProjectID
      ,FactoryID
      ,UnitID
      ,InventoryRefnoId
      ,Refno
      ,BrandGroup
      ,BrandID
      ,LimitHandle
      ,LimitSmr
      ,AuthMr
      ,Payable
      ,Qty
      ,InputQty
      ,OutputQty
      ,Deadline
      ,PoFactory
      ,OrderHandle
      ,OrderSmr
      ,PoHandle
      ,PoSmr
      ,StyleID
      ,SeasonID
      ,FabricType
      ,MtlTypeID
      ,ReasonID
      ,Remark
      ,IcrNo
      ,DebitID
      ,Lock
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
      ,ETA
	  ,SCIRefno
from Trade_To_Pms.dbo.Inventory as b
where not exists(select POID from Production.dbo.Inventory as a where a.POID=b.POID and a.Seq1=b.Seq1 and a.Seq2=b.Seq2 and a.FactoryID=b.FactoryID and a.UnitID=b.UnitID and a.ProjectID=b.ProjectID and a.InventoryRefnoId=b.InventoryRefnoId)



--invref
--InventoryRefno
--      ,[BomArticle]
--      ,[BomBuymonth]
--      ,[BomCountry]
--      ,[BomCustCD]
--,[BomFactory]
--      ,[BomStyle]
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.ID	       =b.ID
      a.Refno	      =b.Refno
      ,a.Width	      =b.Width
      ,a.ColorID	      =b.ColorID
      ,a.SizeSpec	      =b.SizeSpec
      ,a.SizeUnit	      =b.SizeUnit
      ,a.BomCustPONo	      =b.BomCustPONo
      ,a.BomZipperInsert	      =b.BomZipperInsert
      ,a.Special_Old	      =b.Special_Old
      ,a.Spec_Old	      =b.Spec_Old
      ,a.ProdID_Old	      =b.ProdID_Old
      ,a.AddName	      =b.AddName
      ,a.AddDate	      =b.AddDate
from Production.dbo.InventoryRefno as a 
inner join Trade_To_Pms.dbo.InventoryRefno as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.InventoryRefno(
       ID
      ,Refno
      ,Width
      ,ColorID
      ,SizeSpec
      ,SizeUnit
      ,BomCustPONo
      ,BomZipperInsert
      ,Special_Old
      ,Spec_Old
      ,ProdID_Old
      ,AddName
      ,AddDate
)
select 
       ID
      ,Refno
      ,Width
      ,ColorID
      ,SizeSpec
      ,SizeUnit
      ,BomCustPONo
      ,BomZipperInsert
      ,Special_Old
      ,Spec_Old
      ,ProdID_Old
      ,AddName
      ,AddDate
from Trade_To_Pms.dbo.InventoryRefno as b
where not exists(select id from Production.dbo.InventoryRefno as a where a.id = b.id)

--Invtrans
----------------------刪除主TABLE多的資料
Delete Production.dbo.Invtrans 
from Production.dbo.Invtrans  as a
left join Trade_To_Pms.dbo.Invtrans  as b on a.id = b.id and a.Ukey = b.Ukey
where b.id is null
AND a.ID > (select top 1 id from  Trade_To_Pms.dbo.Invtrans order by id)
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
       --a.ID	 =b.ID
      --,a.Ukey	  =b.Ukey
      a.ConfirmDate	      =b.ConfirmDate
      ,a.ConfirmHandle	      =b.ConfirmHandle
      ,a.Confirmed	      =b.Confirmed
      ,a.Qty	      =b.Qty
      ,a.Type	      =b.Type
      ,a.TransferMDivisionID	      = iif(c.MDivisionID is null,'',c.MDivisionID) 
	  ,a.TransferFactory	      = b.TransferFactory
      ,a.InventoryUkey	      =b.InventoryUkey
      ,a.InventoryRefnoId	      =b.InventoryRefnoId
      ,a.PoID	      =b.PoID
      ,a.Seq1	      =b.Seq1
      ,a.Seq2	      =b.Seq2
      ,a.InventoryPOID	      =b.InventoryPOID
      ,a.InventorySeq1	      =b.InventorySeq1
      ,a.InventorySeq2	      =b.InventorySeq2
      ,a.Remark	      =b.Remark
      ,a.JunkPo3	      =b.JunkPo3
      ,a.Deadline	      =b.Deadline
      ,a.ReasonID	      =b.ReasonID
      ,a.Payable	      =b.Payable
      ,a.PoHandle	      =b.PoHandle
      ,a.PoSmr	      =b.PoSmr
      ,a.OrderHandle	      =b.OrderHandle
      ,a.OrderSmr	      =b.OrderSmr
      ,a.PoFactory	      =b.PoFactory
      ,a.LimitHandle	      =b.LimitHandle
      ,a.LimitSmr	      =b.LimitSmr
      ,a.AuthMr	      =b.AuthMr
      ,a.VoucherID	      =b.VoucherID
      ,a.TransferUkey	      =b.TransferUkey
      ,a.Po3QtyOld	      =b.Po3QtyOld
      ,a.InventoryQtyOld	      =b.InventoryQtyOld
      ,a.ProjectOld	      =b.ProjectOld
      ,a.BrandID	      =b.BrandID
      ,a.BrandGroup	      =b.BrandGroup
      ,a.Refno	      =b.Refno
      ,a.FabricType	      =b.FabricType
      ,a.FactoryID	      =b.FactoryID
      ,a.MtlTypeID	      =b.MtlTypeID
      ,a.ProjectID	      =b.ProjectID
      ,a.SeasonID	      =b.SeasonID
      ,a.StyleID	      =b.StyleID
      ,a.UnitID	      =b.UnitID
      ,a.BomCustPONo	      =b.BomCustPONo
      ,a.BomZipperInsert	      =b.BomZipperInsert
      ,a.AddName	      =b.AddName
      ,a.AddDate	      =b.AddDate
      ,a.EditDate	      =b.EditDate
      ,a.EditName	      =b.EditName
      ,a.Seq70PoID	      =b.Seq70PoID
      ,a.Seq70Seq1	      =b.Seq70Seq1
      ,a.Seq70Seq2	      =b.Seq70Seq2
from Production.dbo.Invtrans  as a 
inner join Trade_To_Pms.dbo.Invtrans  as b ON a.id=b.id   and a.Ukey = b.Ukey
left JOIN Production.dbo.SCIFty c on b.TransferFactory=c.ID
where b.Confirmed=1
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Invtrans (
       ID
	  ,Ukey
      ,ConfirmDate
      ,ConfirmHandle
      ,Confirmed
      ,Qty
      ,Type
      ,TransferFactory
	  ,TransferMDivisionID
      ,InventoryUkey
      ,InventoryRefnoId
      ,PoID
      ,Seq1
      ,Seq2
      ,InventoryPOID
      ,InventorySeq1
      ,InventorySeq2
      ,Remark
      ,JunkPo3
      ,Deadline
      ,ReasonID
      ,Payable
      ,PoHandle
      ,PoSmr
      ,OrderHandle
      ,OrderSmr
      ,PoFactory
      ,LimitHandle
      ,LimitSmr
      ,AuthMr
      ,VoucherID
      ,TransferUkey
      ,Po3QtyOld
      ,InventoryQtyOld
      ,ProjectOld
      ,BrandID
      ,BrandGroup
      ,Refno
      ,FabricType
      ,FactoryID
      ,MtlTypeID
      ,ProjectID
      ,SeasonID
      ,StyleID
      ,UnitID
      ,BomCustPONo
      ,BomZipperInsert
      ,AddName
      ,AddDate
      ,EditDate
      ,EditName
      ,Seq70PoID
      ,Seq70Seq1
      ,Seq70Seq2
)
select 
 b.ID
	  ,Ukey
      ,ConfirmDate
      ,ConfirmHandle
      ,Confirmed
      ,Qty
      ,Type
	  ,TransferFactory
      ,iif(c.MDivisionID is null,'',c.MDivisionID) as TransferMDivisionID
      ,InventoryUkey
      ,InventoryRefnoId
      ,PoID
      ,Seq1
      ,Seq2
      ,InventoryPOID
      ,InventorySeq1
      ,InventorySeq2
      ,Remark
      ,JunkPo3
      ,Deadline
      ,ReasonID
      ,Payable
      ,PoHandle
      ,PoSmr
      ,OrderHandle
      ,OrderSmr
      ,PoFactory
      ,LimitHandle
      ,LimitSmr
      ,AuthMr
      ,VoucherID
      ,TransferUkey
      ,Po3QtyOld
      ,InventoryQtyOld
      ,ProjectOld
      ,BrandID
      ,BrandGroup
      ,Refno
      ,FabricType
      ,FactoryID
      ,MtlTypeID
      ,ProjectID
      ,SeasonID
      ,StyleID
      ,UnitID
      ,BomCustPONo
      ,BomZipperInsert
      ,b.AddName
      ,b.AddDate
      ,b.EditDate
      ,b.EditName
      ,Seq70PoID
      ,Seq70Seq1
      ,Seq70Seq2
from Trade_To_Pms.dbo.Invtrans as b
left JOIN Production.dbo.SCIFty c on b.TransferFactory=c.ID
where not exists(select id from Production.dbo.Invtrans  as a where a.id = b.id  and a.Ukey = b.Ukey)
AND b.Confirmed=1
--InReason  InvtransReason
----------------------刪除主TABLE多的資料
Delete Production.dbo.InvtransReason
from Production.dbo.InvtransReason as a left join Trade_To_Pms.dbo.InvtransReason as b
on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET  
      -- a.ID	       =b.ID
      a.ReasonEN	      =b.ReasonEN
      ,a.ReasonCH	      =b.ReasonCH
      ,a.IsDefault	      =b.IsDefault
      ,a.Junk	      =b.Junk
      ,a.AdjustFields	      =b.AdjustFields
      ,a.AdjustDesc	      =b.AdjustDesc
      ,a.AddName	      =b.AddName
      ,a.AddDate	      =b.AddDate
      ,a.EditName	      =b.EditName
      ,a.EditDate	      =b.EditDate

from Production.dbo.InvtransReason as a inner join Trade_To_Pms.dbo.InvtransReason as b ON a.id=b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.InvtransReason(
       ID
      ,ReasonEN
      ,ReasonCH
      ,IsDefault
      ,Junk
      ,AdjustFields
      ,AdjustDesc
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
)
select 
       ID
      ,ReasonEN
      ,ReasonCH
      ,IsDefault
      ,Junk
      ,AdjustFields
      ,AdjustDesc
      ,AddName
      ,AddDate
      ,EditName
      ,EditDate
from Trade_To_Pms.dbo.InvtransReason as b
where not exists(select id from Production.dbo.InvtransReason as a where a.id = b.id)



END

