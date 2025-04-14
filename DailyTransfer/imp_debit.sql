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

	declare @Tdebit table(id varchar(13),[action] varchar(6))
-------------Merge update Production.Debit---------------------------
	Merge Production.dbo.Debit as t
	Using (select * from Trade_To_Pms.dbo.Debit WITH (NOLOCK) where ResponFTY in (select id from @Sayfty ) )as s
	on t.id = s.id
	when matched and (s.sysdate <> t.sysdate or t.EditDate <> s.EditDate) then
		update set 
			t.ID= isnull(s.ID ,''),
			t.CurrencyID= isnull(s.CurrencyID ,         ''),
			t.Amount= isnull(s.Amount ,                 0),
			t.Received= isnull(s.Received ,             0),
			t.BuyerID= isnull(s.BuyerID ,               ''),
			t.BrandID= isnull(s.BrandID ,               ''),
			t.BankID= isnull(s.BankID ,                 ''),
			t.LCFNO= isnull(s.LCFNO ,                   ''),
			t.LCFDate= s.LCFDate ,
			t.EstPayDate= s.EstPayDate ,
			t.Title= isnull(s.Title ,                   ''),
			t.SendFrom= isnull(s.SendFrom ,             ''),
			t.Lock= isnull(s.Lock ,                     0),
			t.Lockdate= s.Lockdate ,
			t.OldAmount= isnull(s.OldAmount ,           0),
			t.Type= isnull(s.Type ,                     ''),
			t.ShareFob= isnull(s.ShareFob ,             0),
			t.VoucherFactory= isnull(s.VoucherFactory , ''),
			t.VoucherSettle= isnull(s.VoucherSettle ,   ''),
			t.IsSubcon= isnull(s.IsSubcon ,             0),
			t.Attn= isnull(s.Attn ,                     ''),
			t.CC= isnull(s.CC ,                         ''),
			t.Subject= isnull(s.Subject ,               ''),
			t.Handle= isnull(s.Handle ,                 ''),
			t.SMR= isnull(s.SMR ,                       ''),
			t.VoucherID= isnull(s.VoucherID ,           ''),
			t.BadID= isnull(s.BadID ,                   ''),
			t.Status= isnull(s.Status ,                 ''),
			t.StatusRevise= s.StatusRevise ,
			t.StatusReviseNm= isnull(s.StatusReviseNm , ''),
			t.CustPayId= isnull(s.CustPayId ,           ''),
			t.Settled= isnull(s.Settled ,               ''),
			t.SettleDate= s.SettleDate ,
			t.Cfm= isnull(s.Cfm ,                       ''),
			t.CfmDate= s.CfmDate ,
			t.AddName= isnull(s.AddName ,               ''),
			t.issuedate= s.cdate ,
			t.AddDate= s.AddDate ,
			t.SysDate= s.SysDate , 
			t.MDivisionID=isnull((SELECT iif(MDivisionID is null,'',MDivisionID) FROM Production.dbo.scifty WITH (NOLOCK) where id=s.ResponFTY),''),
			t.ResponFTY = isnull(s.ResponFTY,''),
			t.SubName = isnull(s.SubName,''),
			t.SubconID = isnull(s.SubconID,''),
			t.OrderCompanyID = isnull(s.OrderCompany, 0)
	when not matched by target then 	
    INSERT
           (
                  id,
                  currencyid,
                  amount,
                  received,
                  buyerid,
                  brandid,
                  bankid,
                  lcfno,
                  lcfdate,
                  estpaydate,
                  title,
                  sendfrom,
                  attn,
                  cc,
                  subject,
                  handle,
                  smr,
                  voucherid,
                  badid,
                  status,
                  statusrevise,
                  statusrevisenm,
                  custpayid,
                  settled,
                  settledate,
                  cfm,
                  cfmdate,
                  lock,
                  lockdate,
                  oldamount,
                  type,
                  sharefob,
                  voucherfactory,
                  vouchersettle,
                  issubcon,
                  lclname,
                  lclcurrency,
                  lclamount,
                  lclrate,
                  addname,
                  issuedate,
                  adddate,
                  editname,
                  editdate,
                  sysdate,
                  mdivisionid,
                  responfty,
                  subname,
                  SubconID,
                  OrderCompanyID
           )
           VALUES
           (
                  isnull(s.id,             ''),
                  isnull(s.currencyid,     ''),
                  isnull(s.amount,        0),
                  isnull(s.received,       0),
                  isnull(s.buyerid,        ''),
                  isnull(s.brandid,        ''),
                  isnull(s.bankid,         ''),
                  isnull(s.lcfno,          ''),
                  s.lcfdate,
                  s.estpaydate,
                  isnull(s.title,          ''),
                  isnull(s.sendfrom,       ''),
                  isnull(s.attn,           ''),
                  isnull(s.cc,             ''),
                  isnull(s.subject,        ''),
                  isnull(s.handle,         ''),
                  isnull(s.smr,            ''),
                  isnull(s.voucherid,      ''),
                  isnull(s.badid,          ''),
                  isnull(s.status,         ''),
                  s.statusrevise,
                  isnull(s.statusrevisenm, ''),
                  isnull(s.custpayid,      ''),
                  isnull(s.settled,        ''),
                  s.settledate,
                  isnull(s.cfm,            ''),
                  s.cfmdate,
                  isnull(s.lock,           0),
                  s.lockdate,
                  isnull(s.oldamount,      0),
                  isnull(s.type,           ''),
                  isnull(s.sharefob,       0),
                  isnull(s.voucherfactory, ''),
                  isnull(s.vouchersettle,  ''),
                  isnull(s.issubcon,       0),
                  isnull(s.lclname,        ''),
                  isnull(s.lclcurrency,    ''),
                  isnull(s.lclamount,      0),
                  isnull(s.lclrate,        0),
                  isnull(s.addname,        ''),
                  s.cdate,
                  s.adddate,
                  isnull(s.editname,       ''),
                  s.editdate,
                  s.sysdate,
                  isnull((
                         SELECT iif(mdivisionid IS NULL,'',mdivisionid)
                         FROM   production.dbo.scifty WITH (nolock)
                         WHERE  id=s.responfty),''),
                  isnull(s.responfty,''),
                  isnull(subname,''),
                  isnull(SubconID,''),
                  isnull(OrderCompany, 0)                  
           )
		
		output inserted.id,$action into @Tdebit ;

-----------------Production.Debit_detail
	Merge Production.dbo.Debit_Detail as t
	Using (select * from  Trade_To_Pms.dbo.debit_detail WITH (NOLOCK) where id in (select id from @Tdebit where [action]in('INSERT','UPDATE')))as s
	on t.TaipeiUkey =s.ukey
	when matched then
		update set 
			t.ID = isnull( s.ID,                    ''),
			t.ORDERID = isnull( s.OrderID,          ''),
			t.REASONID = isnull( s.REASONID,        ''),
			t.Description = isnull( s.Description,  ''),
			t.PRICE = isnull( s.PRICE,              0),
			t.Amount = isnull( s.Amount,            0),
			t.UnitID = isnull( s.UnitID,            ''),
			t.SOURCEID = isnull( s.SOURCEID,        ''),
			t.QTY = isnull( s.QTY,                  0),
			t.ReasonNM = isnull( s.Reason,          ''),
			t.TaipeiUkey = isnull( s.ukey          ,0)
	when not matched by target then
    INSERT
       (
              id,
              orderid,
              reasonid,
              description,
              price,
              amount,
              unitid,
              sourceid,
              qty,
              reasonnm,
              taipeiukey
       )
       VALUES
       (
              isnull(s.id,          ''),
              isnull(s.orderid,     ''),
              isnull(s.reasonid,    ''),
              isnull(s.description, ''),
              isnull(s.price,       0),
              isnull(s.amount,      0),
              isnull(s.unitid,      ''),
              isnull(s.sourceid,    ''),
              isnull(s.qty,         0),
              isnull(s.reason,      ''),
              isnull(s.ukey        ,0)
       );

	--LocalDebit INSERT
	Merge Production.dbo.LocalDebit as t
	using (Select * from Trade_To_Pms.dbo.debit WITH (NOLOCK) where IsSubcon = 1 and ResponFTY in (select id from @Sayfty )) as s
	on t.id = s.id
	when not matched by target then
    INSERT
       (
              taipeidbc,
              id,
              factoryid,
              taipeiamt,
              taipeicurrencyid ,
              currencyid,
              adddate,
              addname,
              [status],
              mdivisionid,
              issuedate,
              responfty,
              OrderCompanyID
       )
       VALUES
       (
              '1' ,
              isnull(id,           ''),
              isnull(brandid ,     ''),
              isnull(amount ,      0),
              isnull(currencyid ,  ''),
              isnull(currencyid,   ''),
              adddate,
              'SCIMIS',
              'New',
              isnull(
              (
                     SELECT mdivisionid
                     FROM   production.dbo.factory WITH (nolock)
                     WHERE  id=s.responfty),'') ,
              s.adddate,
              isnull(responfty,''),
              isnull(OrderCompany,'')
       )
	;

	
	--LocalDebit UPDATE
	Merge Production.dbo.LocalDebit as t
	using (Select * from Trade_To_Pms.dbo.debit WITH (NOLOCK) where IsSubcon = 1 and ResponFTY in (select id from @Sayfty )) as s
	on t.id = s.id AND t.Status='New'
	when matched then
		update set 
			t.FactoryID = isnull( s.BrandID,            '') ,
			t.TaipeiAMT = isnull( s.Amount,             0) ,
			t.TaipeiCurrencyID = isnull( s.CurrencyID,  '') ,
			t.Currencyid = isnull( s.CurrencyID,        '') ,
			t.EditName = isnull( s.EditName,            '') ,
			t.EditDate = s.EditDate,
			t.MDivisionID = isnull((SELECT  MDivisionID FROM Production.dbo.Factory WITH (NOLOCK) WHERE ID=S.ResponFTY),''),
			t.issuedate = s.adddate,
			t.ResponFTY = isnull( s.ResponFTY,''),
			t.OrderCompanyID = isnull(s.OrderCompany,             0)
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
    INSERT
       (
              id,
              orderid,
              unitid,
              qty,
              amount,
              adddate,
              addname,
              taipeireason,
              description,
              taipeiukey
       )
       VALUES
       (
              isnull(s.id,          '') ,
              isnull(s.orderid,     '') ,
              isnull(s.unitid,      '') ,
              isnull(s.qty,         0) ,
              isnull(s.amount,      0) ,
              s.add1,
              'SCIMIS',
              isnull(s.reason,      '') ,
              isnull(s.description, '') ,
              isnull(ukey          ,0) 
       )
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
			t.id = isnull( s.id,                   '') ,
			t.Orderid = isnull( s.Orderid,         '') ,
			t.UnitID = isnull( s.UnitID,           '') ,
			t.qty = isnull( s.qty,                 0) ,
			t.amount = isnull( s.amount,           0) ,
			t.taipeireason = isnull( s.reason,     '') ,
			t.description = isnull( s.description, '') 
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




