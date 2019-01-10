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
    }
}
