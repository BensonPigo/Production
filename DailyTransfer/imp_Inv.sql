-- =============================================
-- Author:		LEO
-- Create date: 20160903
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE imp_Inv
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
		a.POID				= isnull( b.POID            , '')
		,a.Seq1				= isnull( b.Seq1            , '')
		 ,a.Seq2				= isnull( b.Seq2        , '')
		,a.ProjectID			= isnull( b.ProjectID   , '')
		,a.FactoryID			= isnull( b.FactoryID   , '')
		 ,a.UnitID			= isnull( b.UnitID          , '')
		,a.InventoryRefnoId	= isnull( b.InventoryRefnoId, 0)
		, a.SuppID				= isnull( b.SuppID      , '')
		, a.Refno				= isnull( b.Refno       , '')
		, a.BrandGroup			= isnull( b.BrandGroup  , '')
		, a.BrandID				= isnull( b.BrandID     , '')
		, a.LimitHandle			= isnull( b.LimitHandle , '')
		, a.LimitSmr			= isnull( b.LimitSmr    , '')
		, a.AuthMr				= isnull( b.AuthMr      , '')
		, a.Payable				= isnull( b.Payable     , '')
		, a.Qty					= isnull( b.Qty         , 0)
		, a.InputQty			= isnull( b.InputQty    , 0)
		, a.OutputQty			= isnull( b.OutputQty   , 0)
		, a.Deadline			=  b.Deadline
		, a.PoFactory			= isnull( b.PoFactory	, '')
		, a.OrderHandle			= isnull( b.OrderHandle , '')
		, a.OrderSmr			= isnull( b.OrderSmr    , '')
		, a.PoHandle			= isnull( b.PoHandle    , '')
		, a.PoSmr				= isnull( b.PoSmr       , '')
		, a.StyleID				= isnull( b.StyleID     , '')
		, a.SeasonID			= isnull( b.SeasonID    , '')
		, a.FabricType			= isnull( b.FabricType  , '')
		, a.MtlTypeID			= isnull( b.MtlTypeID   , '')
		, a.ReasonID			= isnull( b.ReasonID    , '')
		, a.Remark				= isnull( b.Remark      , '')
		, a.IcrNo				= isnull( b.IcrNo       , '')
		, a.DebitID				= isnull( b.DebitID     , '')
		, a.Lock				= isnull( b.Lock        , 0)
		, a.AddName				= isnull( b.AddName     , '')
		, a.AddDate				=  b.AddDate
		, a.EditName			= isnull( b.EditName    , '')
		, a.EditDate			=  b.EditDate
		, a.ETA					=  b.ETA
		, a.SCIRefno			= isnull( b.SCIRefno    , '')
		, A.MDivisionID			= isnull(C.MDivisionID,'')
from Production.dbo.Inventory as a 
inner join Trade_To_Pms.dbo.Inventory as b ON	a.ukey = b.ukey
INNER JOIN Production.DBO.SCIFty C ON A.FactoryID = C.ID 
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Inventory(
	Ukey
	, POID
	, Seq1
	, Seq2
	, ProjectID
	, FactoryID
	, UnitID
	, InventoryRefnoId
	, Refno
	, BrandGroup
	, BrandID
	, LimitHandle
	, LimitSmr
	, AuthMr
	, Payable
	, Qty
	, InputQty
	, OutputQty
	, Deadline
	, PoFactory
	, OrderHandle
	, OrderSmr
	, PoHandle
	, PoSmr
	, StyleID
	, SeasonID
	, FabricType
	, MtlTypeID
	, ReasonID
	, Remark
	, IcrNo
	, DebitID
	, Lock
	, AddName
	, AddDate
	, EditName
	, EditDate
	, ETA
	, SCIRefno
	, MDivisionID
	, SuppID
)
select	  isnull(Ukey            , '')
		, isnull(POID            , '')
		, isnull(Seq1            , '')
		, isnull(Seq2            , '')
		, isnull(ProjectID       , '')
		, isnull(FactoryID       , '')
		, isnull(UnitID          , '')
		, isnull(InventoryRefnoId, 0)
		, isnull(Refno           , '')
		, isnull(BrandGroup      , '')
		, isnull(BrandID         , '')
		, isnull(LimitHandle     , '')
		, isnull(LimitSmr        , '')
		, isnull(AuthMr          , '')
		, isnull(Payable         , '')
		, isnull(Qty             , 0)
		, isnull(InputQty        , 0)
		, isnull(OutputQty       , 0)
		, Deadline
		, isnull(PoFactory       , '')
		, isnull(OrderHandle     , '')
		, isnull(OrderSmr        , '')
		, isnull(PoHandle        , '')
		, isnull(PoSmr           , '')
		, isnull(StyleID         , '')
		, isnull(SeasonID        , '')
		, isnull(FabricType      , '')
		, isnull(MtlTypeID       , '')
		, isnull(ReasonID        , '')
		, isnull(Remark          , '')
		, isnull(IcrNo           , '')
		, isnull(DebitID         , '')
		, isnull(Lock            , 0)
		, isnull(AddName         , '')
		, AddDate
		, isnull(EditName        , '')
		, EditDate
		, ETA
		, isnull(SCIRefno        , '')
		, isnull((SELECT MDivisionID FROM Production.dbo.SCIFty WITH (NOLOCK) WHERE ID= A.FactoryID ),'')	
		, isnull(SuppID, '')
		from Trade_To_Pms.dbo.Inventory as a WITH (NOLOCK)
		where not exists(select 1 from Production.dbo.Inventory  WITH (NOLOCK) where a.Ukey = Ukey)

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET		-- a.ID				= b.ID
		a.Refno				= isnull( b.Refno         , '')
		, a.Width			= isnull( b.Width         , 0)
		, a.ColorID			= isnull( b.ColorID       , '')
		, a.SizeSpec	    = isnull( b.SizeSpec      , '')
		, a.SizeUnit	    = isnull( b.SizeUnit      , '')
		, a.BomCustPONo	    = isnull( b.BomCustPONo   , '')
		, a.BomZipperInsert	= isnull( b.BomZipperInsert, '')
		, a.Special_Old	    = isnull( b.Special_Old   , '')
		, a.Spec_Old	    = isnull( b.Spec_Old      , '')
		, a.ProdID_Old	    = isnull( b.ProdID_Old    , '')
		, a.AddName			= isnull( b.AddName       , '')
		, a.AddDate			= b.AddDate
from Production.dbo.InventoryRefno as a 
inner join Trade_To_Pms.dbo.InventoryRefno as b ON a.id = b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.InventoryRefno(
	ID
	, Refno
	, Width
	, ColorID
	, SizeSpec
	, SizeUnit
	, BomCustPONo
	, BomZipperInsert
	, Special_Old
	, Spec_Old
	, ProdID_Old
	, AddName
	, AddDate
)
select 
	  isnull(ID             , 0)
	, isnull(Refno          , '')
	, isnull(Width          , 0)
	, isnull(ColorID        , '')
	, isnull(SizeSpec       , '')
	, isnull(SizeUnit       , '')
	, isnull(BomCustPONo    , '')
	, isnull(BomZipperInsert, '')
	, isnull(Special_Old    , '')
	, isnull(Spec_Old       , '')
	, isnull(ProdID_Old     , '')
	, isnull(AddName        , '')
	, AddDate
from Trade_To_Pms.dbo.InventoryRefno as b WITH (NOLOCK)
where not exists(
		select id 
		from Production.dbo.InventoryRefno as a WITH (NOLOCK) 
		where a.id = b.id
	  )

--Invtrans
----------------------刪除主TABLE多的資料
declare @ID varchar(13)
select top 1 @ID = id from  Trade_To_Pms.dbo.Invtrans order by id
Delete Production.dbo.Invtrans 
from Production.dbo.Invtrans  as a
left join Trade_To_Pms.dbo.Invtrans  as b on a.id = b.id 
											 and a.Ukey = b.Ukey
where	b.id is null
		AND a.ID > @ID

---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET		--a.ID					= b.ID
		--,a.Ukey				= b.Ukey
		  a.ConfirmDate			= b.ConfirmDate
		, a.ConfirmHandle		= isnull( b.ConfirmHandle, '')
		, a.Confirmed			= isnull( b.Confirmed    , 0)
		, a.Qty					= isnull( b.Qty          , 0)
		, a.Type				= isnull(b.Type,'')
		, a.TransferMDivisionID	= isnull(c.MDivisionID ,'') 
		, a.TransferFactory		= case b.Type 
										when '2' then isnull(b.PoFactory,'')
										when  '3' then isnull(b.TransferFactory,'')
										else isnull(b.FactoryID,'') end
		, a.InventoryUkey		= isnull(b.InventoryUkey    , 0)
		, a.InventoryRefnoId	= isnull(b.InventoryRefnoId , 0)
		, a.PoID				= isnull(b.PoID             , '')
		, a.Seq1				= isnull(b.Seq1             , '')
		, a.Seq2				= isnull(b.Seq2             , '')
		, a.InventoryPOID		= isnull(b.InventoryPOID    , '')
		, a.InventorySeq1		= isnull(b.InventorySeq1    , '')
		, a.InventorySeq2		= isnull(b.InventorySeq2    , '')
		, a.Remark				= isnull(b.Remark           , '')
		, a.JunkPo3				= isnull(b.JunkPo3          , '')
		, a.Deadline			= isnull(b.Deadline         , '')
		, a.ReasonID			= isnull( b.ReasonID        , '')
		, a.Payable				= isnull( b.Payable         , '')
		, a.PoHandle			= isnull( b.PoHandle        , '')
		, a.PoSmr				= isnull( b.PoSmr           , '')
		, a.OrderHandle			= isnull( b.OrderHandle     , '')
		, a.OrderSmr			= isnull( b.OrderSmr        , '')
		, a.PoFactory			= isnull( b.PoFactory       , '')
		, a.LimitHandle			= isnull( b.LimitHandle     , '')
		, a.LimitSmr			= isnull( b.LimitSmr        , '')
		, a.AuthMr				= isnull( b.AuthMr          , '')
		, a.VoucherID			= isnull( b.VoucherID       , '')
		, a.TransferUkey		= isnull( b.TransferUkey    , 0)
		, a.Po3QtyOld			= isnull( b.Po3QtyOld       , 0)
		, a.InventoryQtyOld		= isnull( b.InventoryQtyOld , 0)
		, a.ProjectOld			= isnull( b.ProjectOld      , '')
		, a.BrandID				= isnull( b.BrandID         , '')
		, a.BrandGroup			= isnull( b.BrandGroup      , '')
		, a.Refno				= isnull( b.Refno           , '')
		, a.FabricType			= isnull( b.FabricType      , '')
		, a.FactoryID			= isnull( b.FactoryID       , '')
		, a.MtlTypeID			= isnull( b.MtlTypeID       , '')
		, a.ProjectID			= isnull( b.ProjectID       , '')
		, a.SeasonID			= isnull( b.SeasonID        , '')
		, a.StyleID				= isnull( b.StyleID         , '')
		, a.UnitID				= isnull( b.UnitID          , '')
		, a.BomCustPONo			= isnull( b.BomCustPONo     , '')
		, a.BomZipperInsert		= isnull( b.BomZipperInsert , '')
		, a.AddName				= isnull( b.AddName         , '')
		, a.AddDate				= b.AddDate
		, a.EditDate			= b.EditDate
		, a.EditName			= isnull( b.EditName        , '')
		, a.Seq70PoID			= isnull( b.Seq70PoID       , '')
		, a.Seq70Seq1			= isnull( b.Seq70Seq1       , '')
		, a.Seq70Seq2			= isnull( b.Seq70Seq2       , '')
from Production.dbo.Invtrans  as a 
inner join Trade_To_Pms.dbo.Invtrans  as b ON a.id=b.id   and a.Ukey = b.Ukey
left JOIN Production.dbo.SCIFty c on b.TransferFactory=c.ID
where b.Confirmed=1


-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.Invtrans (
	ID
	, Ukey
	, ConfirmDate
	, ConfirmHandle
	, Confirmed
	, Qty
	, Type
	, TransferFactory
	, TransferMDivisionID
	, InventoryUkey
	, InventoryRefnoId
	, PoID
	, Seq1
	, Seq2
	, InventoryPOID
	, InventorySeq1
	, InventorySeq2
	, Remark
	, JunkPo3
	, Deadline
	, ReasonID
	, Payable
	, PoHandle
	, PoSmr
	, OrderHandle
	, OrderSmr
	, PoFactory
	, LimitHandle
	, LimitSmr
	, AuthMr
	, VoucherID
	, TransferUkey
	, Po3QtyOld
	, InventoryQtyOld
	, ProjectOld
	, BrandID
	, BrandGroup
	, Refno
	, FabricType
	, FactoryID
	, MtlTypeID
	, ProjectID
	, SeasonID
	, StyleID
	, UnitID
	, BomCustPONo
	, BomZipperInsert
	, AddName
	, AddDate
	, EditDate
	, EditName
	, Seq70PoID
	, Seq70Seq1
	, Seq70Seq2
)
select	  ISNULL(b.ID, '')
		, ISNULL(Ukey         ,0)
		, ConfirmDate
		, ISNULL(ConfirmHandle,0)
		, ISNULL(Confirmed,0)
		, ISNULL(Qty,0)
		, ISNULL(b.Type,'')
		, case b.Type 
				when '2' then isnull(b.PoFactory,'')
				when  '3' then isnull(b.TransferFactory,'')
				else isnull(b.FactoryID,'') end
		, isnull(c.MDivisionID,'')
		, ISNULL(InventoryUkey   , 0)
		, ISNULL(InventoryRefnoId, 0)
		, ISNULL(PoID            , '')
		, ISNULL(Seq1            , '')
		, ISNULL(Seq2            , '')
		, ISNULL(InventoryPOID   , '')
		, ISNULL(InventorySeq1   , '')
		, ISNULL(InventorySeq2   , '')
		, ISNULL(Remark          , '')
		, ISNULL(JunkPo3         , '')
		, Deadline
		, ISNULL(ReasonID        , '')
		, ISNULL(Payable         , '')
		, ISNULL(PoHandle        , '')
		, ISNULL(PoSmr           , '')
		, ISNULL(OrderHandle     , '')
		, ISNULL(OrderSmr        , '')
		, ISNULL(PoFactory       , '')
		, ISNULL(LimitHandle     , '')
		, ISNULL(LimitSmr        , '')
		, ISNULL(AuthMr          , '')
		, ISNULL(VoucherID       , '')
		, ISNULL(TransferUkey    , 0)
		, ISNULL(Po3QtyOld       , 0)
		, ISNULL(InventoryQtyOld , 0)
		, ISNULL(ProjectOld      , '')
		, ISNULL(BrandID         , '')
		, ISNULL(BrandGroup      , '')
		, ISNULL(Refno           , '')
		, ISNULL(FabricType      , '')
		, ISNULL(FactoryID       , '')
		, ISNULL(MtlTypeID       , '')
		, ISNULL(ProjectID       , '')
		, ISNULL(SeasonID        , '')
		, ISNULL(StyleID         , '')
		, ISNULL(UnitID          , '')
		, ISNULL(BomCustPONo     , '')
		, ISNULL(BomZipperInsert , '')
		, ISNULL(b.AddName       , '')
		, b.AddDate
		, b.EditDate
		, ISNULL(b.EditName      , '')
		, ISNULL(Seq70PoID       , '')
		, ISNULL(Seq70Seq1       , '')
		, ISNULL(Seq70Seq2       , '')
from Trade_To_Pms.dbo.Invtrans as b WITH (NOLOCK)
left JOIN Production.dbo.SCIFty c WITH (NOLOCK) on b.TransferFactory=c.ID
where	not exists(
			select id 
			from Production.dbo.Invtrans as a WITH (NOLOCK) 
			where	a.id = b.id  
					and a.Ukey = b.Ukey
		)
		AND b.Confirmed=1

update Production.dbo.invtrans
set TransferMDivisionID = isnull(c.MDivisionID ,'')
from Production.dbo.invtrans b
left JOIN Production.dbo.SCIFty c on b.TransferFactory=c.ID
where b.TransferMDivisionID is null


----------------------刪除主TABLE多的資料
Delete Production.dbo.InvtransReason
from Production.dbo.InvtransReason as a 
left join Trade_To_Pms.dbo.InvtransReason as b on a.id = b.id
where b.id is null
---------------------------UPDATE 主TABLE跟來源TABLE 為一樣(主TABLE多的話 記起來 ~來源TABLE多的話不理會)
UPDATE a
SET		-- a.ID				= b.ID
		a.ReasonEN			= isnull( b.ReasonEN    , '')
		, a.ReasonCH		= isnull( b.ReasonCH    , '')
		, a.IsDefault		= isnull( b.IsDefault   , 0)
		, a.Junk			= isnull( b.Junk        , 0)
		, a.AdjustFields	= isnull( b.AdjustFields, '')
		, a.AdjustDesc		= isnull( b.AdjustDesc  , '')
		, a.AddName			= isnull( b.AddName     , '')
		, a.AddDate			= b.AddDate
		, a.EditName		= isnull( b.EditName    , '')
		, a.EditDate		= b.EditDate
from Production.dbo.InvtransReason as a 
inner join Trade_To_Pms.dbo.InvtransReason as b ON a.id = b.id
-------------------------- INSERT INTO 抓
INSERT INTO Production.dbo.InvtransReason(
	ID
	, ReasonEN
	, ReasonCH
	, IsDefault
	, Junk
	, AdjustFields
	, AdjustDesc
	, AddName
	, AddDate
	, EditName
	, EditDate
)
select 
	  isnull(ID          , '')
	, isnull(ReasonEN    , '')
	, isnull(ReasonCH    , '')
	, isnull(IsDefault   , 0)
	, isnull(Junk        , 0)
	, isnull(AdjustFields, '')
	, isnull(AdjustDesc  , '')
	, isnull(AddName     , '')
	, AddDate
	, isnull(EditName    , '')
	, EditDate
from Trade_To_Pms.dbo.InvtransReason as b WITH (NOLOCK)
where	not exists (
			select id 
			from Production.dbo.InvtransReason as a WITH (NOLOCK) 
			where a.id = b.id
		)
END

