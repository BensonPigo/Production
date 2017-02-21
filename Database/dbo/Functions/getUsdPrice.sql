-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	Get Po_supp_detail USD Price
-- =============================================
CREATE FUNCTION dbo.getUsdPrice
(	
	-- Add the parameters for the function here
	@poid varchar(13),
	@seq1 varchar(3),
	@seq2 varchar(2)
)
RETURNS @USDPrice TABLE 
(
	poid varchar(13),
	seq1 varchar(3),
	seq2 varchar(2),
	usd_price numeric(11,4)
)
AS
begin
	insert into @USDPrice (poid,seq1,seq2,usd_price)
	select a.id,a.seq1,a.seq2,round(a.Price/d.PriceRate * (select StdRate from dbo.currency cur1 where id=c.CurrencyId ) / (select StdRate from dbo.currency cur1 where id='USD' ),4) as USD_Price 
	from dbo.PO_Supp_Detail a
	inner join dbo.po_supp b on b.ID= a.ID and b.SEQ1 = a.SEQ1
	inner join dbo.supp c on c.ID = b.SuppID
	inner join Unit d on d.ID = a.POUnit
	where a.id=@poid
	and a.seq1 = @seq1
	and a.seq2 = @seq2;
	RETURN
end