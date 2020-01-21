

CREATE FUNCTION [dbo].[getMinCompleteSewQtyByDate]
(
	@orderid varchar(16),
	@article varchar(8)=null,
	@sizecode varchar(8)=null,
	@excludesister bit = 0
)
RETURNS @Tmp table ( 
	OutputDate date,
	QAQty int
)
as
BEGIN
	DECLARE @minSeqQty int
	DECLARE @StyleUnit as varchar(5), @StyleUkey as varchar(20)
	SET @minSeqQty = 0

	select @StyleUkey = StyleUkey,@StyleUnit = StyleUnit from Orders WITH (NOLOCK) where ID = @orderid
	IF @StyleUnit = 'PCS'
		BEGIN
		IF @article is null and @sizecode is null
			BEGIN
				insert into @Tmp
				select so.OutputDate,QAQty=sum(sdd.QAQty) 
				from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
				inner join SewingOutput so with(nolock) on so.id = sdd.id
				where sdd.OrderId = @orderid
				and (@excludesister = 0 or (@excludesister = 1 and  not exists(select 1 from SCIFty WITH (NOLOCK) where SCIFty.ID=so.SubconOutFty and so.Shift = 'O')))
				group by so.OutputDate
			END
		Else
			IF @sizecode is null
				BEGIN
					insert into @Tmp
					select so.OutputDate,QAQty=sum(sdd.QAQty) 
					from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
					inner join SewingOutput so with(nolock) on so.id = sdd.id
					where sdd.OrderId = @orderid
					and sdd.Article = @article
					and (@excludesister = 0 or (@excludesister = 1 and  not exists(select 1 from SCIFty WITH (NOLOCK) where SCIFty.ID=so.SubconOutFty and so.Shift = 'O')))
					group by so.OutputDate
				END
			ELSE
				BEGIN
					insert into @Tmp
					select so.OutputDate,QAQty=sum(sdd.QAQty) 
					from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
					inner join SewingOutput so with(nolock) on so.id = sdd.id
					where sdd.OrderId = @orderid
					and sdd.Article = @article
					and sdd.SizeCode = @sizecode
					and (@excludesister = 0 or (@excludesister = 1 and  not exists(select 1 from SCIFty WITH (NOLOCK) where SCIFty.ID=so.SubconOutFty and so.Shift = 'O')))
					group by so.OutputDate
				END 
		END	
	ELSE
		BEGIN
		IF @article is null and @sizecode is null
			BEGIN
				insert into @Tmp
				select a.OutputDate,QAQty= min(a.QAQty)
				from (
					select so.OutputDate,sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
					from Style_Location sl WITH (NOLOCK)
					left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = sl.Location
					inner join SewingOutput so with(nolock) on so.id = sdd.id
					where StyleUkey = @StyleUkey
					and (@excludesister = 0 or (@excludesister = 1 and  not exists(select 1 from SCIFty WITH (NOLOCK) where SCIFty.ID=so.SubconOutFty and so.Shift = 'O')))
					group by so.OutputDate,sl.Location
				) a
				group by OutputDate
			END
		ELSE
			IF @sizecode is null
				BEGIN
				insert into @Tmp
				select a.OutputDate,QAQty= min(a.QAQty)
				from (
					select so.OutputDate,sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
					from Style_Location sl WITH (NOLOCK)
					left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article
					inner join SewingOutput so with(nolock) on so.id = sdd.id
					where StyleUkey = @StyleUkey
					and (@excludesister = 0 or (@excludesister = 1 and  not exists(select 1 from SCIFty WITH (NOLOCK) where SCIFty.ID=so.SubconOutFty and so.Shift = 'O')))
					group by so.OutputDate,sl.Location
				) a
				group by OutputDate
				END
			ELSE
				BEGIN
					insert into @Tmp
					select a.OutputDate,QAQty= min(a.QAQty)
					from (
						select so.OutputDate,sl.Location, sum(isnull(sdd.QAQty,0)) as QAQty
						from Style_Location sl WITH (NOLOCK)
						left join SewingOutput_Detail_Detail sdd WITH (NOLOCK) on sdd.OrderId = @orderid and sdd.ComboType = sl.Location and sdd.Article = @article and sdd.SizeCode = @sizecode
						inner join SewingOutput so with(nolock) on so.id = sdd.id
						where StyleUkey = @StyleUkey
						and (@excludesister = 0 or (@excludesister = 1 and  not exists(select 1 from SCIFty WITH (NOLOCK) where SCIFty.ID=so.SubconOutFty and so.Shift = 'O')))
						group by so.OutputDate,sl.Location
					) a
					group by OutputDate
				END
		END
	return ;
END