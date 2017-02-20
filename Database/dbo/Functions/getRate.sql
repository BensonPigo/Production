
-- =============================================
-- Author:		Mike
-- Create date: 2016/03/02
-- Description:	依匯率別抓取工廠財務系統的匯率
-- =============================================
CREATE FUNCTION [dbo].[getRate]
(
	@rateid varchar(2), -- ex. System.ExchangeId
	@from_currencyid varchar(3),
	@to_currencyid varchar(3),
	@rateDate date
)
RETURNS decimal(18,8) 
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	if @from_currencyid = @to_currencyid
	begin
		return 1;
	end
	
	DECLARE @rate decimal(18,8), @tmpRate decimal(18,8)
	
	-- 正向取值
	select @rate = rate from FinanceEN.dbo.rate 
	where RateTypeID = @rateid and OriginalCurrency = @from_currencyid and ExchangeCurrency = @to_currencyid
	and BeginDate <= @rateDate and EndDate >= @rateDate
	if @rate is null
	begin
		-- 反向取值
		select @tmpRate = rate from FinanceEN.dbo.rate
		where RateTypeID = @rateid and OriginalCurrency = @to_currencyid and ExchangeCurrency = @from_currencyid
		and BeginDate <= @rateDate and EndDate >= @rateDate
		if @tmpRate is null
			begin
				--先轉台幣再轉欲取得的幣別
				select @tmpRate = rate from FinanceEN.dbo.rate
				where RateTypeID = @rateid and OriginalCurrency = @from_currencyid and ExchangeCurrency = 'TWD'
				and BeginDate <= @rateDate and EndDate >= @rateDate
				set @rate = @tmpRate
				select @tmpRate = rate from FinanceEN.dbo.rate
				where RateTypeID = @rateid and OriginalCurrency = 'TWD' and ExchangeCurrency = @to_currencyid
				and BeginDate <= @rateDate and EndDate >= @rateDate
				IF @tmpRate IS NULL OR @rate IS NULL
				begin
					--先轉台幣再轉欲取得的幣別(反向搜尋)
					select @tmpRate = rate from FinanceEN.dbo.rate
					where RateTypeID = @rateid and OriginalCurrency = 'TWD' and ExchangeCurrency = @from_currencyid
					and BeginDate <= @rateDate and EndDate >= @rateDate
					set @rate = @tmpRate
					select @tmpRate = rate from FinanceEN.dbo.rate
					where RateTypeID = @rateid and OriginalCurrency = @to_currencyid and ExchangeCurrency = 'TWD'
					and BeginDate <= @rateDate and EndDate >= @rateDate
					IF @tmpRate IS NULL OR @rate IS NULL
						set @rate = 0.0
					else
						SET @rate = round(1.0 / (@rate * @tmpRate),8)
				end
				ELSE
					SET @rate = @rate * @tmpRate
			end
		else
			begin
				set @rate = round(1.0/@tmpRate,8)
			end
	end

	
	RETURN @rate

END