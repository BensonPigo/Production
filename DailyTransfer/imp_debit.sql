-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/19>
-- Description:	<import debit>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Debit]

AS
BEGIN

declare @Sayfty table(id varchar(10)) --�u�t�N�X
insert @Sayfty select id from Production.dbo.Factory

	declare @Tdebit table(id varchar(13),isinsert bit)

	--Merge update Production.Debit
	Merge Production.dbo.Debit as t
	Using (select * from Trade_To_Pms.dbo.Debit where BrandID in (select id from @Sayfty ) )as s
	on t.id = s.id
	when matched and (s.sysdate <> t.sysdate or t.EditDate <> s.EditDate) then
		update set 
		t.VoucherSettle = s.VoucherSettle ,
		t.EditName = s.EditName,
		t.EditDate = s.EditDate,
		t.SysDate = s.SysDate
	when not matched by target then 	
		insert(	 ID,  CurrencyID,  Amount,  Received,  BuyerID,  BrandID,  BankID,  LCFNO,  LCFDate,  EstPayDate,  Title,  SendFrom,  Attn,  CC,  Subject,  Handle,  SMR,  VoucherID,  BadID,  Status,  StatusRevise,  StatusReviseNm, CustPayId,   Settled,  SettleDate,  Cfm,  CfmDate,  Lock,  Lockdate,  OldAmount,  Type,  ShareFob,  VoucherFactory,  VoucherSettle,  IsSubcon,  LCLName,  LCLCurrency,  LCLAmount,  LCLRate,  AddName,  AddDate,  EditName,  EditDate,  SysDate,MDivisionID)
		values(s.ID,s.CurrencyID,s.Amount,s.Received,s.BuyerID,s.BrandID,s.BankID,s.LCFNO,s.LCFDate,s.EstPayDate,s.Title,s.SendFrom,s.Attn,s.CC,s.Subject,s.Handle,s.SMR,s.VoucherID,s.BadID,s.Status,s.StatusRevise,s.StatusReviseNm,s.CustPayId,s.Settled,s.SettleDate,s.Cfm,s.CfmDate,s.Lock,s.Lockdate,s.OldAmount,s.Type,s.ShareFob,s.VoucherFactory,s.VoucherSettle,s.IsSubcon,s.LCLName,s.LCLCurrency,s.LCLAmount,s.LCLRate,s.AddName,s.AddDate,s.EditName,s.EditDate,s.SysDate,(SELECT iif(MDivisionID is null,'',MDivisionID) FROM Production.dbo.scifty where id=s.BrandID))
		output inserted.id,iif(deleted.id is null,1,0) into @Tdebit ;



	Merge Production.dbo.Debit_Detail as t
	Using (select * from  Trade_To_Pms.dbo.debit_detail where id in (select id from @Tdebit where isinsert=1))as s
	on t.ukey =s.ukey
	when not matched by target then
		insert(ID,ORDERID,REASONID,Description,PRICE,Amount,UnitID,SOURCEID,QTY)
		values(s.ID,s.ORDERID,s.REASONID,s.Description,s.PRICE,s.Amount,s.UnitID,s.SOURCEID,s.QTY );

declare @tLocalDebit table (id varchar(13),isinsert bit)

	Merge Production.dbo.LocalDebit as t
	using (Select * from Trade_To_Pms.dbo.debit where id in (select id from @Tdebit where isinsert=1)) as s
	on t.id = s.id
	when not matched by target and s.IsSubcon = 1 then
		insert(TaipeiDBC,id, FactoryID, TaipeiAMT, TaipeiCurrencyID, AddDate, AddName,[status],MDivisionID)
		values( '1', Id, BrandID, Amount, CurrencyID, AddDate,'SCIMIS','New',
		(SELECT  iif(MDivisionID is null,'',MDivisionID) FROM SCIFTY WHERE ID=S.BRANDID) )
		output inserted.id,iif(deleted.id='',1,0) into @tLocalDebit;


	Merge Production.dbo.LocalDebit_Detail as t
	using( select a.*,b.adddate as add1 from Trade_To_Pms.dbo.debit_detail a
	inner join Trade_To_Pms.dbo.debit b on a.id=b.id
	 where a.id in (select id from @tLocalDebit where isinsert=1)) as s
	on t.ukey=s.ukey
	when not matched by target then
		insert(	 id,   Orderid,   UnitID,   qty,   amount,  AddDate, AddName)
		values(s.Id, s.orderid, s.UnitID, s.qty, s.Amount,s.add1, 'SCIMIS');


END




