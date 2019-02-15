using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;
using System.Data.SqlClient;

namespace Sci.Production.PublicPrg
{
    public static partial class Prgs
    {
        #region CalculateShareExpense
        /// <summary>
        /// CalculateShareExpense(string)
        /// </summary>
        /// <param name="shippingAPID"></param>
        /// <returns>bool</returns>
        public static bool CalculateShareExpense(string shippingAPID)
        {
            string sqlCmd;

            sqlCmd = string.Format(@"--宣告變數: 
DECLARE @id VARCHAR(13),
		@ttlgw NUMERIC(10,3),
		@ttlcbm NUMERIC(10,3),
		@ttlcount INT,
		@accno VARCHAR(8),
		@login VARCHAR(10),
		@adddate DATETIME,
		@exact TINYINT

--設定變數值
SET @id = '{0}'
SET @login = '{1}'
SET @adddate = GETDATE()
SELECT @ttlgw = isnull(sum(GW),0), @ttlcbm = isnull(sum(CBM),0), @ttlcount = isnull(count(ShippingAPID),0) 
FROM (SELECT distinct ShippingAPID,BLNo,WKNo,InvNo,GW,CBM FROM ShareExpense WITH (NOLOCK) WHERE ShippingAPID = @id and junk = 0) a

SELECT @exact = isnull(c.Exact,0) FROM ShippingAP s WITH (NOLOCK) , Currency c WITH (NOLOCK) WHERE s.ID = @id and c.ID = s.CurrencyID

--撈出依會科加總的金額與要分攤的WK or GB
DECLARE cursor_ttlAmount CURSOR FOR
	select a.*,isnull(isnull(sr.ShareBase,sr1.ShareBase),'') as ShareBase
	from (select *
		  from (select isnull(se.AccountID,'') as AccountID, sum(sd.Amount) as Amount, s.CurrencyID
				from ShippingAP_Detail sd WITH (NOLOCK) 
				left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
				left join [FinanceEN].dbo.AccountNo a on a.ID = se.AccountID
				left join ShippingAP s WITH (NOLOCK) on s.ID = sd.ID
				where sd.ID = @id
				group by se.AccountID, a.Name, s.CurrencyID) a,
				(select distinct BLNo,WKNo,InvNo,Type,GW,CBM,ShipModeID,FtyWK
				from ShareExpense WITH (NOLOCK) 
				where ShippingAPID = @id and junk = 0) b) a
	left join ShareRule sr WITH (NOLOCK) on sr.AccountID = a.AccountID and sr.ExpenseReason = a.Type and (sr.ShipModeID = '' or sr.ShipModeID like '%'+a.ShipModeID+'%')
	left join ShareRule sr1 WITH (NOLOCK) on sr1.AccountID = left(a.AccountID,4) and sr1.ExpenseReason = a.Type and (sr1.ShipModeID = '' or sr1.ShipModeID like '%'+a.ShipModeID+'%')
	order by a.AccountID,GW,CBM

--撈出費用分攤中已不存在AP中的會科
DECLARE cursor_diffAccNo CURSOR FOR
	select distinct AccountID
	from ShareExpense WITH (NOLOCK) 
	where ShippingAPID = @id
	except
	select distinct se.AccountID
	from ShippingAP_Detail sd WITH (NOLOCK) 
	left join ShipExpense se WITH (NOLOCK) on se.ID = sd.ShipExpenseID
	where sd.ID = @id

--刪除已不存在AP中的會科資料
OPEN cursor_diffAccNo
FETCH NEXT FROM cursor_diffAccNo INTO @accno
WHILE @@FETCH_STATUS = 0
BEGIN
	delete from ShareExpense where ShippingAPID = @id and AccountID = @accno
	FETCH NEXT FROM cursor_diffAccNo INTO @accno
END
CLOSE cursor_diffAccNo

DECLARE @amount NUMERIC(15,4),
		@blno VARCHAR(20),
		@wkno VARCHAR(13),
		@invno VARCHAR(25),
		@type VARCHAR(15),
		@gw NUMERIC(9,3),
		@cbm NUMERIC(9,4),
		@shipmodeid VARCHAR(10),
		@sharebase VARCHAR(1),
		@count INT,
		@remainamount NUMERIC(15,4),
		@minusamount NUMERIC(15,4),
		@recno INT,
		@currency VARCHAR(3),
		@ftywk BIT,
		@inputamount NUMERIC(15,2),
		@maxblno VARCHAR(20),
		@maxwkno VARCHAR(13),
		@maxinvno VARCHAR(25),
		@maxdata NUMERIC(9,2),
		@1stsharebase VARCHAR(1)

SET @count = 1
SET @maxdata = 0
OPEN cursor_ttlAmount
FETCH NEXT FROM cursor_ttlAmount INTO @accno,@amount,@currency,@blno,@wkno,@invno,@type,@gw,@cbm,@shipmodeid,@ftywk,@sharebase
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @count = 1
		BEGIN
			SET @remainamount = @amount
			SET @maxdata = 0
			SET @maxblno = @blno
			SET @maxwkno = @wkno
			SET @maxinvno = @invno
			SET @1stsharebase = @sharebase
			IF @1stsharebase = 'C'
				BEGIN
					SET @minusamount = ROUND(@amount/@ttlcbm,4)
				END
			ELSE
				IF @1stsharebase = 'G'
					BEGIN
						SET @minusamount = ROUND(@amount/@ttlgw,4)
					END
				ELSE
					BEGIN
						SET @minusamount = ROUND(@amount/@ttlcount,4)
					END
		END
	ELSE
		BEGIN
			SET @remainamount = @remainamount - @inputamount
		END
	
	IF @1stsharebase = 'C'
		BEGIN
			SET @inputamount = ROUND((@minusamount * @cbm),@exact)
			IF @maxdata < @cbm
				BEGIN
					SET @maxblno = @blno
					SET @maxwkno = @wkno
					SET @maxinvno = @invno
				END
		END
	ELSE
		IF @1stsharebase = 'G'
			BEGIN
				SET @inputamount = ROUND((@minusamount * @gw),@exact)
				IF @maxdata < @gw
				BEGIN
					SET @maxblno = @blno
					SET @maxwkno = @wkno
					SET @maxinvno = @invno
				END
			END
		ELSE
			BEGIN
				SET @inputamount = ROUND(@minusamount,@exact)
			END

	select @recno = isnull(count(ShippingAPID),0) from ShareExpense WITH (NOLOCK) where ShippingAPID = @id and WKNo = @wkno and BLNo = @blno and InvNo = @invno and AccountID = @accno
	IF @recno = 0
		BEGIN
			INSERT INTO ShareExpense(ShippingAPID,BLNo,WKNo,InvNo,Type,GW,CBM,CurrencyID,Amount,ShipModeID,ShareBase,FtyWK,AccountID,EditName,EditDate)
				VALUES (@id, @blno, @wkno, @invno, @type, @gw, @cbm, @currency, @inputamount, @shipmodeid, @1stsharebase, @ftywk, @accno, @login, @adddate)
		END
	ELSE
		BEGIN
			UPDATE ShareExpense 
			SET CurrencyID = @currency, Amount = @inputamount, ShareBase = @1stsharebase, EditName = @login, EditDate = @adddate 
			where ShippingAPID = @id and WKNo = @wkno and BLNo = @blno and InvNo = @invno and AccountID = @accno
		END

	
	IF @count = @ttlcount
		BEGIN	
			SET @count = 1
			SET @remainamount = @remainamount - @inputamount
			IF @remainamount <> 0
				BEGIN
					UPDATE ShareExpense 
			SET CurrencyID = @currency, Amount = Amount + @remainamount, EditName = @login, EditDate = @adddate 
			where ShippingAPID = @id and WKNo = @maxwkno and BLNo = @maxblno and InvNo = @maxinvno and AccountID = @accno
				END
		END
	ELSE
		BEGIN
			SET @count = @count + 1
		END
	FETCH NEXT FROM cursor_ttlAmount INTO @accno,@amount,@currency,@blno,@wkno,@invno,@type,@gw,@cbm,@shipmodeid,@ftywk,@sharebase
END
CLOSE cursor_ttlAmount", shippingAPID, Sci.Env.User.UserID);
            DualResult result = DBProxy.Current.Execute(null, sqlCmd);
            if (!result)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region ReCalculateExpress
        /// <summary>
        /// ReCalculateExpress(string)
        /// </summary>
        /// <param name="expressID"></param>
        /// <returns>string</returns>
        public static string ReCalculateExpress(string expressID)
        {
            return string.Format(
@"update Express set NW = (select SUM(NW) from Express_Detail where ID = '{0}'),
CTNQty = (select COUNT(distinct CTNNo) from Express_Detail where ID = '{0}')
where ID = '{0}'", expressID);

        }
        #endregion

        #region GetNLCodeDataByRefno
        public static DataRow GetNLCodeDataByRefno(string refno, string stockQty, string styleUkey, string category)
        {
            string sqlGetNLCode = string.Empty;
            DataRow drNLCode = null;
            string inputStockQty = MyUtility.Check.Empty(stockQty) ? "0" : stockQty;
            List<SqlParameter> parGetNLCode  = new List<SqlParameter>() { new SqlParameter("@Refno", refno),
                                                                                new SqlParameter("@inputStockQty", stockQty) };
            List<SqlParameter> parGetFabricType = new List<SqlParameter>() { new SqlParameter("@inputRefno", refno),
                                                                                new SqlParameter("@inputStyleUkey", styleUkey),
                                                                                new SqlParameter("@inputCategory", category) };
            string fabricType = string.Empty;

            // 沒有傳入type則依refno判斷,並檢查此refno是否存在傳入style下的材料
            string sqlGetFabricType = $@"
declare @refno varchar(20) = @inputRefno
declare @styleUkey bigint = @inputStyleUkey
declare @category varchar(1) = @inputCategory
declare @type varchar(1) = ''

select @type = 'F'
        from Style_BOF sb
        left join Fabric f WITH (NOLOCK) on sb.SCIRefno = f.SCIRefno
        where sb.StyleUkey = @styleUkey and sb.Refno = @refno and (sb.SuppIDBulk <> 'FTY' or sb.SuppIDBulk <> 'FTY-C') and f.NoDeclare = 0

if(@type = '')
begin
    select @type = 'A' from Style_BOA where StyleUkey = @styleUkey and Refno = @refno and IsCustCD <> 2 and SuppIDBulk <> 'FTY' and SuppIDBulk <> 'FTY-C'
end

if(@type = '')
begin
    select @type = 'L' from LocalPO_Detail where OrderId = (select TOP 1 ID 
                                                            from Orders WITH (NOLOCK) 
                                                            where   StyleUkey = @styleUkey 
                                                                    and Category = @category 
                                                            order by BuyerDelivery, ID) and Refno = @refno
end

select @type
";
            fabricType = MyUtility.GetValue.Lookup(sqlGetFabricType, parGetFabricType);
            if (MyUtility.Check.Empty(fabricType))
            {
                return null;
            }

            if (fabricType == "F")
            {
                sqlGetNLCode = $@"
Declare @StockQty numeric(12,4) = @inputStockQty
select  NLCode ,
        [StockUnit] = 'YDS',
        [SCIRefno] = f.SCIRefno,
        [FabricBrandID] = f.BrandID,
        [HSCode] = f.HSCode,
        [UnitID] = f.CustomsUnit,
        [FabricType] = 'F',
        [LocalItem] = 0,
        [Qty] = [dbo].getVNUnitTransfer('F','YDS',f.CustomsUnit,@StockQty,f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(f.CustomsUnit = 'M2',M2Rate.value,isnull(Rate.value,1)),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
from Fabric f with (nolock)
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit) Rate
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') M2Rate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit) UnitRate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M') M2UnitRate
 where f.Refno = @Refno
";
            }
            else if (fabricType == "A")
            {
                sqlGetNLCode = $@"
Declare @StockQty numeric(12,4) = @inputStockQty
select  NLCode ,
        [StockUnit] = f.UsageUnit,
        [SCIRefno] = f.SCIRefno,
        [FabricBrandID] = f.BrandID,
        [HSCode] = f.HSCode,
        [UnitID] = f.CustomsUnit,
        [FabricType] = 'A',
        [LocalItem] = 0,
        [Qty] = [dbo].getVNUnitTransfer(f.Type,f.UsageUnit,f.CustomsUnit,@StockQty,0,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
from Fabric f with (nolock)
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = f.CustomsUnit) Rate
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = f.UsageUnit and TO_U = 'M') M2Rate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = f.CustomsUnit) UnitRate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = f.UsageUnit and UnitTo = 'M') M2UnitRate
 where f.Refno = @Refno
";
            }
            else if (fabricType == "L")
            {
                sqlGetNLCode = $@"
Declare @StockQty numeric(12,4) = @inputStockQty
select  li.NLCode,
        [StockUnit] = li.UnitID,
        [SCIRefno] = @Refno,
        [FabricBrandID] = '',
        [HSCode] =li.HSCode,
        [UnitID] = li.CustomsUnit,
        [FabricType] = 'L',
        [LocalItem] = 1,
        [Qty] = [dbo].getVNUnitTransfer('',li.UnitID,isnull(li.CustomsUnit,''),@StockQty,0,li.PcsWidth,li.PcsLength,li.PcsKg,IIF(li.CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(li.CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value))
from LocalItem li with (nolock) 
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = li.CustomsUnit) Rate
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = 'M') M2Rate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = li.CustomsUnit) UnitRate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = 'M') M2UnitRate
where li.Refno = @Refno";
            }
            bool isNLCodeExists = MyUtility.Check.Seek(sqlGetNLCode, parGetNLCode, out drNLCode);
            if (isNLCodeExists)
            {
                return drNLCode;
            }
            else
            {
                return null;
            }
            
        }
        #endregion

    }
}
