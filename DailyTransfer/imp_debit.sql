-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/19>
-- Description:	<import debit>
-- =============================================
Create PROCEDURE [dbo].[imp_Debit]

AS
BEGIN

declare @Sayfty table(id varchar(10)) --工廠代碼
insert @Sayfty select id from Production.dbo.Factory

	declare @Tdebit table(id varchar(13),isinsert bit)
-------------Merge update Production.Debit---------------------------
	Merge Production.dbo.Debit as t
	Using (select * from Trade_To_Pms.dbo.Debit WITH (NOLOCK) where ResponFTY in (select id from @Sayfty ) )as s
	on t.id = s.id
	when matched and (s.sysdate <> t.sysdate or t.EditDate <> s.EditDate) then
		update set 
			t.ID=s.ID ,
			t.CurrencyID=s.CurrencyID ,
			t.Amount=s.Amount ,
			t.Received=s.Received ,
			t.BuyerID=s.BuyerID ,
			t.BrandID=s.BrandID ,
			t.BankID=s.BankID ,
			t.LCFNO=s.LCFNO ,
			t.LCFDate=s.LCFDate ,
			t.EstPayDate=s.EstPayDate ,
			t.Title=s.Title ,
			t.SendFrom=s.SendFrom ,
			t.Lock=s.Lock ,
			t.Lockdate=s.Lockdate ,
			t.OldAmount=s.OldAmount ,
			t.Type=s.Type ,
			t.ShareFob=s.ShareFob ,
			t.VoucherFactory=s.VoucherFactory ,
			t.VoucherSettle=s.VoucherSettle ,
			t.IsSubcon=s.IsSubcon ,
			t.Attn=s.Attn ,
			t.CC=s.CC ,
			t.Subject=s.Subject ,
			t.Handle=s.Handle ,
			t.SMR=s.SMR ,
			t.VoucherID=s.VoucherID ,
			t.BadID=s.BadID ,
			t.Status=s.Status ,
			t.StatusRevise=s.StatusRevise ,
			t.StatusReviseNm=s.StatusReviseNm ,
			t.CustPayId=s.CustPayId ,
			t.Settled=s.Settled ,
			t.SettleDate=s.SettleDate ,
			t.Cfm=s.Cfm ,
			t.CfmDate=s.CfmDate ,
			t.AddName=s.AddName ,
			t.issuedate=s.cdate ,
			t.AddDate=s.AddDate ,
			t.SysDate=s.SysDate ,
			t.MDivisionID=(SELECT iif(MDivisionID is null,'',MDivisionID) FROM Production.dbo.scifty WITH (NOLOCK) where id=s.ResponFTY),
			t.ResponFTY = s.ResponFTY,
			t.SubName = s.SubName
	when not matched by target then 	
		insert(	 ID,  CurrencyID,  Amount,  Received,  BuyerID,  BrandID,  BankID,  LCFNO,  LCFDate,  EstPayDate,  Title,  SendFrom,  Attn,  CC,
				  Subject,  Handle,  SMR,  VoucherID,  BadID,  Status,  StatusRevise,  StatusReviseNm, CustPayId,   Settled,  SettleDate,  Cfm,  CfmDate,
				  Lock,  Lockdate,  OldAmount,  Type,  ShareFob,  VoucherFactory,  VoucherSettle,  IsSubcon,  LCLName,  LCLCurrency,  LCLAmount,  LCLRate,
				  AddName,issuedate,  AddDate,  EditName,  EditDate,
				  SysDate,MDivisionID,
				  ResponFTY,SubName
				  )
		values(s.ID,s.CurrencyID,s.Amount,s.Received,s.BuyerID,s.BrandID,s.BankID,s.LCFNO,s.LCFDate,s.EstPayDate,s.Title,s.SendFrom,s.Attn,s.CC,
				s.Subject,s.Handle,s.SMR,s.VoucherID,s.BadID,s.Status,s.StatusRevise,s.StatusReviseNm,s.CustPayId,s.Settled,s.SettleDate,s.Cfm,s.CfmDate,
				s.Lock,s.Lockdate,s.OldAmount,s.Type,s.ShareFob,s.VoucherFactory,s.VoucherSettle,s.IsSubcon,s.LCLName,s.LCLCurrency,s.LCLAmount,s.LCLRate,
				s.AddName,s.cdate,s.AddDate,s.EditName,s.EditDate,
				s.SysDate,(SELECT iif(MDivisionID is null,'',MDivisionID) FROM Production.dbo.scifty WITH (NOLOCK) where id=s.ResponFTY),
				s.ResponFTY,SubName
				)
		
		output inserted.id,iif(deleted.id is null,1,0) into @Tdebit ;

-----------------Production.Debit_detail
	Merge Production.dbo.Debit_Detail as t
	Using (select * from  Trade_To_Pms.dbo.debit_detail WITH (NOLOCK) where id in (select id from @Tdebit where isinsert=1))as s
	on t.TaipeiUkey =s.ukey
	when matched then
		update set 
			t.ID = s.ID,
			t.ORDERID = s.OrderID,
			t.REASONID = s.REASONID,
			t.Description = s.Description,
			t.PRICE = s.PRICE,
			t.Amount = s.Amount,
			t.UnitID = s.UnitID,
			t.SOURCEID = s.SOURCEID,
			t.QTY = s.QTY,
			t.ReasonNM = s.Reason,
			t.TaipeiUkey = s.ukey
	when not matched by target then
		insert(ID,ORDERID,REASONID,Description,PRICE,Amount,UnitID,SOURCEID,QTY,ReasonNM,TaipeiUkey)
		values(s.ID,s.ORDERID,s.REASONID,s.Description,s.PRICE,s.Amount,s.UnitID,s.SOURCEID,s.QTY,s.Reason,s.ukey );
		

declare @tLocalDebit table (id varchar(13),isinsert bit)

	--LocalDebit INSERT
	Merge Production.dbo.LocalDebit as t
	using (Select * from Trade_To_Pms.dbo.debit WITH (NOLOCK) where IsSubcon = 1 and ResponFTY in (select id from @Sayfty )) as s
	on t.id = s.id
	when not matched by target then
		insert(TaipeiDBC,id, FactoryID, TaipeiAMT, TaipeiCurrencyID ,CurrencyID, AddDate, AddName,[status],MDivisionID,issuedate,ResponFTY)
		values( '1'    , Id, BrandID  , Amount   , CurrencyID		,CurrencyID, AddDate,'SCIMIS','New',
			isnull((SELECT  MDivisionID FROM Production.dbo.Factory WITH (NOLOCK) WHERE ID=S.ResponFTY),'')
			,s.adddate,ResponFTY )
	output inserted.id,iif(deleted.id='',1,0) into @tLocalDebit;

	
	--LocalDebit UPDATE
	Merge Production.dbo.LocalDebit as t
	using (Select * from Trade_To_Pms.dbo.debit WITH (NOLOCK) where IsSubcon = 1 and ResponFTY in (select id from @Sayfty )) as s
	on t.id = s.id AND t.Status='New'
	when matched then
		update set 
			t.FactoryID = s.BrandID,
			t.TaipeiAMT = s.Amount,
			t.TaipeiCurrencyID = s.CurrencyID,
			t.Currencyid = s.CurrencyID,
			t.EditName = s.EditName,
			t.EditDate = s.EditDate,
			t.MDivisionID = isnull((SELECT  MDivisionID FROM Production.dbo.Factory WITH (NOLOCK) WHERE ID=S.ResponFTY),''),
			t.issuedate = s.adddate,
			t.ResponFTY = s.ResponFTY
	;
	
	--LocalDebit_Detail INSERT
	Merge Production.dbo.LocalDebit_Detail as t
	using( 
			select a.*,b.adddate as add1 
			from Trade_To_Pms.dbo.debit_detail a WITH (NOLOCK)
			inner join Trade_To_Pms.dbo.debit b WITH (NOLOCK) on a.id=b.id
			where a.id in (select id from  Production.dbo.LocalDebit WITH (NOLOCK) where TaipeiDBC=1)
			and b.ResponFTY in (select id from @Sayfty )
	 ) as s
	on t.TaipeiUkey=s.ukey
	when not matched by target then
		insert(	 id,   Orderid,   UnitID,   qty,   amount,  AddDate, AddName,taipeireason,description,TaipeiUkey)
		values(s.Id, s.orderid, s.UnitID, s.qty, s.Amount,s.add1, 'SCIMIS',s.reason,s.description,ukey)
	;
		
	--LocalDebit_Detail UPDATE
	Merge Production.dbo.LocalDebit_Detail as t
	using( 
			select a.*,b.adddate as add1 
			from Trade_To_Pms.dbo.debit_detail a WITH (NOLOCK)
			inner join Trade_To_Pms.dbo.debit b WITH (NOLOCK) on a.id=b.id
			where a.id in (select id from  Production.dbo.LocalDebit WITH (NOLOCK) where TaipeiDBC=1)
			and b.ResponFTY in (select id from @Sayfty )
	 ) as s
	on t.TaipeiUkey=s.ukey AND t.ID IN (SELECT ID FROM Production.dbo.LocalDebit WHERE Status='New')
	when matched then
		update set 
			t.id = s.id,
			t.Orderid = s.Orderid,
			t.UnitID = s.UnitID,
			t.qty = s.qty,
			t.amount = s.amount,
			t.taipeireason = s.reason,
			t.description = s.description
	;

	select * into #Debit_source from Trade_To_Pms.dbo.Debit WITH (NOLOCK) where ResponFTY in (select id from @Sayfty )

	--先刪除Debit_Detail,再刪除Debit , Status = 'Junked' 且Debit_schedule無資料
	delete Production.dbo.Debit_Detail
	from Production.dbo.Debit 
	inner join Production.dbo.Debit_Detail on Debit.ID = Debit_Detail.ID
	inner join #Debit_source s on s.id = Debit.ID
	where s.Status = 'Junked'
	and not exists(select 1 from Production.dbo.Debit_schedule where Debit_schedule.id = Debit.id)

	delete Production.dbo.Debit
	from Production.dbo.Debit 
	inner join #Debit_source s on s.id = Debit.ID
	where s.Status = 'Junked'
	and not exists(select 1 from Production.dbo.Debit_schedule where Debit_schedule.id = Debit.id)
	
	--先刪除LocalDebit_Detail,再刪除LocalDebit , Status = 'Junked' 且Debit_schedule無資料
	delete Production.dbo.LocalDebit_Detail
	from Production.dbo.LocalDebit 
	inner join Production.dbo.LocalDebit_Detail on LocalDebit.ID = LocalDebit_Detail.ID
	inner join #Debit_source s on s.id = LocalDebit.ID
	where s.Status = 'Junked'
	and not exists(select 1 from Production.dbo.Debit_schedule where Debit_schedule.id = LocalDebit.id)

	delete Production.dbo.LocalDebit
	from Production.dbo.LocalDebit 
	inner join #Debit_source s on s.id = LocalDebit.ID
	where s.Status = 'Junked'
	and not exists(select 1 from Production.dbo.Debit_schedule where Debit_schedule.id = LocalDebit.id)
	
	--先刪除LocalDebit_Detail,再刪除LocalDebit , IsSubcon=0 且Debit_schedule無資料
	delete Production.dbo.LocalDebit_Detail
	from Production.dbo.LocalDebit 
	inner join Production.dbo.LocalDebit_Detail on LocalDebit.ID = LocalDebit_Detail.ID
	inner join #Debit_source s on s.id = LocalDebit.ID
	where s.IsSubcon=0
	and not exists(select 1 from Production.dbo.Debit_schedule where Debit_schedule.id = LocalDebit.id)

	delete Production.dbo.LocalDebit
	from Production.dbo.LocalDebit 
	inner join #Debit_source s on s.id = LocalDebit.ID
	where s.IsSubcon=0
	and not exists(select 1 from Production.dbo.Debit_schedule where Debit_schedule.id = LocalDebit.id)

	drop table #Debit_source
END




