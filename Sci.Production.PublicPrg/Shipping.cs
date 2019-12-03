﻿using System;
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
using Sci.Win.UI;

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
		@exact TINYINT,
		@CurrencyID VARCHAR(3)

--設定變數值
SET @id = '{0}'
set @CurrencyID=(select CurrencyID from ShippingAP where id = @id)
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
				left join SciFMS_AccountNo a on a.ID = se.AccountID
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
CLOSE cursor_ttlAmount

--以下為Airpp 拆分Factory與Other部分
--只有AirPP的資料需要在往下分攤
select se.InvNo,se.AccountID,[Amount] = sum(se.Amount)
into #InvNoSharedAmt
from ShareExpense se with (nolock)
where	se.ShippingAPID = @ID and se.Junk = 0 and
		exists(select 1 from GMTBooking gmt with (nolock)
							 inner join ShipMode sm with (nolock) on gmt.ShipModeID = sm.ID
						     where gmt.ID = se.InvNo and sm.NeedCreateAPP = 1)
group by se.InvNo,se.AccountID

select	t.InvNo,[PackID] = pl.ID,t.AccountID,t.Amount,[PLSharedAmt] = Round(t.Amount / SUM(pl.GW) over(PARTITION BY t.InvNo,t.AccountID) * pl.GW,2)
into #PLSharedAmtStep1
from #InvNoSharedAmt t
inner join PackingList pl with (nolock) on pl.INVNo = t.InvNo

select * ,[AccuPLSharedAmt] = SUM(PLSharedAmt) over(PARTITION BY InvNo,AccountID order BY InvNo,PackID,AccountID )
into #PLSharedAmtStep2
from #PLSharedAmtStep1

select *,
	  [PLSharedAmtFin] = case	when count(1) over(partition by invno,AccountID ) = 1 then Amount
								when ROW_NUMBER() over(partition by invno,AccountID order BY InvNo,PackID,AccountID) < count(1) over(partition by invno,AccountID ) then PLSharedAmt
								else Amount -  LAG(AccuPLSharedAmt) over(partition by invno,AccountID order by invno,PackID,AccountID) end
into #PLSharedAmt
from #PLSharedAmtStep2

select  t.InvNo,pld.ID,AirPPID=app.ID,t.AccountID,pld.OrderID,pld.OrderShipmodeSeq, t.PLSharedAmtFin
    , [TtlNW] = ROUND(sum(pld.NWPerPcs * pld.ShipQty),3)
    , [OrderSharedAmt] =iif(TtlNW.Value = 0,0,ROUND(t.PLSharedAmtFin / TtlNW.Value * sum(pld.NWPerPcs * pld.ShipQty),2))  
    , [QtyPerCTN] = sum(QtyPerCTN), [RatioFty] = isnull(app.RatioFty,0)		
into #OrderSharedAmtStep1
from #PLSharedAmt t
inner join PackingList_Detail pld with (nolock) on t.PackID = pld.ID
inner join AirPP app with (nolock) on pld.OrderID = app.OrderID and pld.OrderShipmodeSeq = app.OrderShipmodeSeq
outer apply (select [Value] = isnull(sum(NWPerPcs * ShipQty),0) from PackingList_Detail where ID = t.PackID) TtlNW
group by t.InvNo,pld.ID,app.ID,t.AccountID, pld.OrderID, pld.OrderShipmodeSeq, TtlNW.Value, t.PLSharedAmtFin, app.RatioFty

select * ,[AccuOrderSharedAmt] = SUM(OrderSharedAmt) over(PARTITION BY ID,AccountID order BY AccountID,OrderID,OrderShipmodeSeq )
into #OrderSharedAmtStep2
from #OrderSharedAmtStep1

select	*,
		[OrderSharedAmtFin] =  case	when OrderSharedAmt = 0 then 0
                                    when count(1) over(partition by ID,AccountID ) = 1 then PLSharedAmtFin
									when ROW_NUMBER() over(partition by ID,AccountID order BY AccountID,OrderID,OrderShipmodeSeq) < count(1) over(partition by ID,AccountID ) then OrderSharedAmt
									else PLSharedAmtFin -  LAG(AccuOrderSharedAmt) over(partition by ID,AccountID order by AccountID,OrderID,OrderShipmodeSeq) end
into #OrderSharedAmt
from #OrderSharedAmtStep2

declare @SharedAmtFactory numeric (12, 2) 
declare @SharedAmtOther numeric (12, 2) 

select *,RatioOther=100-RatioFty,
	SharedAmtFactory=ROUND(OrderSharedAmtFin / 100 * RatioFty,2),
	SharedAmtOther=OrderSharedAmtFin - ROUND(OrderSharedAmtFin / 100 * RatioFty,2)
into #source
from #OrderSharedAmt

merge ShareExpense_APP t
using #source s
on @id = t.ShippingAPID and s.InvNo=t.InvNo and s.ID = t.PackingListID and s.AirPPID = t.AirPPID and s.AccountID = t.AccountID
when matched then update set 
	t.[CurrencyID]	  =@CurrencyID
	,t.[NW]			  =s.ttlNw
	,t.[RatioFty]	  =s.[RatioFty]
	,t.[AmtFty]		  =s.SharedAmtFactory
	,t.[RatioOther]	  =s.[RatioOther]
	,t.[AmtOther]	  =s.SharedAmtOther
	,t.[Junk]		  =0
	,t.[EditName]	  =@login
	,t.[EditDate]	  =getdate()
when not matched by target then
insert([ShippingAPID],[InvNo],[PackingListID],[AirPPID],[AccountID],[CurrencyID],[NW],[RatioFty],[AmtFty],[RatioOther],[AmtOther],[Junk])
VALUES(@id,s.[InvNo],s.id,s.[AirPPID],s.[AccountID],@CurrencyID,s.ttlNw,s.[RatioFty],s.SharedAmtFactory,s.[RatioOther],s.SharedAmtOther,0)
;

select	@SharedAmtFactory = isnull(sum(ROUND(OrderSharedAmtFin / 100 * RatioFty,2)),0),
		@SharedAmtOther = isnull(sum(OrderSharedAmtFin - ROUND(OrderSharedAmtFin / 100 * RatioFty,2)),0)
from #OrderSharedAmt

update ShippingAP set SharedAmtFactory = @SharedAmtFactory,SharedAmtOther = @SharedAmtOther where ID = @id
 

drop table #InvNoSharedAmt,#PLSharedAmtStep1,#PLSharedAmtStep2,#PLSharedAmt,#OrderSharedAmtStep1,#OrderSharedAmtStep2,#OrderSharedAmt
--以上為Airpp 拆分Factory與Other部分

", shippingAPID, Sci.Env.User.UserID);
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
        public static DataRow GetNLCodeDataByRefno(string refno, string usageQty, string brandID, string type, string sciRefno = "", string nlCode = "", string usageUnit = "")
        {
            string sqlGetNLCode = string.Empty;
            string sqlFA = string.Empty;
            string whereSciRefno = MyUtility.Check.Empty(sciRefno) ? string.Empty : " and f.SciRefno = @SciRefno";
            string whereNLCode = MyUtility.Check.Empty(nlCode) ? string.Empty : " and f.NLCode = @NLCode";
            //string whereUsageUnit = MyUtility.Check.Empty(usageUnit) ? string.Empty : " and f.UsageUnit = @usageUnit";
            DataRow drNLCode = null;
            string inputUsageQty = MyUtility.Check.Empty(usageQty) ? "0" : usageQty;
            List<SqlParameter> parGetNLCode = new List<SqlParameter>() {    new SqlParameter("@Refno", refno),
                                                                            new SqlParameter("@inputUsageQty", usageQty),
                                                                            new SqlParameter("@BrandID", brandID),
                                                                            new SqlParameter("@SciRefno", sciRefno),
                                                                            new SqlParameter("@NLCode", nlCode),
                                                                            new SqlParameter("@usageUnit", usageUnit)};
            string fabricType = type;

            if (fabricType == "F")
            {
                sqlFA = $@"
Declare @UsageQty numeric(12,4) = @inputUsageQty
select  top 1
        NLCode ,
        [StockUnit] = StockUnit.val,
        [SCIRefno] = f.SCIRefno,
        [FabricBrandID] = f.BrandID,
        [HSCode] = f.HSCode,
        [UnitID] = f.CustomsUnit,
        [FabricType] = 'F',
        [LocalItem] = 0,
        [Qty] = [dbo].getVNUnitTransfer('F',StockUnit.val,f.CustomsUnit,StockQty.val,f.Width,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(f.CustomsUnit = 'M2',M2Rate.value,isnull(Rate.value,1)),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),default),
        [UsageUnit] = 'YDS',
		[StockQty] = StockQty.val
from Fabric f with (nolock)
inner join Brand b with (nolock) on b.{{0}} = @BrandID 
inner join Brand b2 with (nolock) on b2.BrandGroup = b.BrandGroup and f.BrandID = b2.ID 
outer apply(select [val] = dbo.getStockUnit(f.SCIRefNo,default)) as StockUnit
outer apply(select [val] = dbo.getUnitRate('YDS',StockUnit.val) * @UsageQty) as StockQty
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = f.CustomsUnit) Rate
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = 'M') M2Rate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = f.CustomsUnit) UnitRate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = 'M') M2UnitRate
 where f.Refno = @Refno and f.Type = 'F' and f.Junk = 0 {whereSciRefno} {whereNLCode} and f.UsageUnit = @usageUnit
order by iif(f.BrandID = @BrandID,0,1 ),f.NLCode,f.EditDate desc
";
            }
            else if (fabricType == "A")
            {
                sqlFA = $@"
Declare @UsageQty numeric(12,4) = @inputUsageQty
select  NLCode ,
        [StockUnit] = StockUnit.val,
        [SCIRefno] = f.SCIRefno,
        [FabricBrandID] = f.BrandID,
        [HSCode] = f.HSCode,
        [UnitID] = f.CustomsUnit,
        [FabricType] = 'A',
        [LocalItem] = 0,
        [Qty] = [dbo].getVNUnitTransfer(f.Type,StockUnit.val,f.CustomsUnit,StockQty.val,0,f.PcsWidth,f.PcsLength,f.PcsKg,IIF(CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),default),
        [UsageUnit] = f.UsageUnit,
		[StockQty] = StockQty.val
from Fabric f with (nolock)
inner join Brand b with (nolock) on b.{{0}} = @BrandID 
inner join Brand b2 with (nolock) on b2.BrandGroup = b.BrandGroup and f.BrandID = b2.ID 
outer apply(select [val] = dbo.getStockUnit(f.SCIRefNo,default)) as StockUnit
outer apply(select [val] = dbo.getUnitRate(f.UsageUnit,StockUnit.val) * @UsageQty) as StockQty
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = f.CustomsUnit) Rate
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = 'M') M2Rate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = f.CustomsUnit) UnitRate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = 'M') M2UnitRate
 where f.Refno = @Refno and f.Type = 'A' and f.Junk = 0 {whereSciRefno} {whereNLCode} and f.UsageUnit = @usageUnit
order by iif(f.BrandID = @BrandID,0,1 ),f.NLCode,f.EditDate desc
";
            }
            else if (fabricType == "L")
            {
                sqlGetNLCode = $@"
Declare @UsageQty numeric(12,4) = @inputUsageQty
select  li.NLCode,
        [StockUnit] = li.UnitID,
        [SCIRefno] = @Refno,
        [FabricBrandID] = '',
        [HSCode] =li.HSCode,
        [UnitID] = li.CustomsUnit,
        [FabricType] = 'L',
        [LocalItem] = 1,
        [Qty] = [dbo].getVNUnitTransfer(li.Category,li.UnitID,isnull(li.CustomsUnit,''),@UsageQty,0,li.PcsWidth,li.PcsLength,li.PcsKg,IIF(li.CustomsUnit = 'M2',M2Rate.value,Rate.value),IIF(li.CustomsUnit = 'M2',M2UnitRate.value,UnitRate.value),li.Refno),
        [UsageUnit] = li.UnitID,
		[StockQty] = @UsageQty
from LocalItem li with (nolock) 
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = li.CustomsUnit) Rate
outer apply (select [value] = RateValue from dbo.View_Unitrate where FROM_U = li.UnitID and TO_U = 'M') M2Rate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = li.CustomsUnit) UnitRate
outer apply (select [value] = Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = li.UnitID and UnitTo = 'M') M2UnitRate
where Ltrim(li.Refno) = @Refno";
            }
            else if (fabricType == "Misc")
            {
                sqlGetNLCode = $@"
Declare @UsageQty numeric(12,4) = @inputUsageQty
select  Misc.NLCode,
        [StockUnit] = Misc.UsageUnit,
        [SCIRefno] = @Refno,
        [FabricBrandID] = '',
        [HSCode] =Misc.HSCode,
        [UnitID] = Misc.CustomsUnit,
        [FabricType] = 'Misc',
        [LocalItem] = 1,
        [Qty] = [dbo].getVNUnitTransfer('MISC',Misc.UsageUnit,isnull(Misc.CustomsUnit,''),@UsageQty,Misc.PcsWidth,Misc.PcsWidth,Misc.PcsLength,Misc.PcsKg,Misc.MiscRate,0,Misc.ID),
        [UsageUnit] = Misc.UsageUnit,
		[StockQty] = @UsageQty
from  SciMachine_Misc Misc with (nolock) 
where Ltrim(Misc.ID)  = @Refno";
            }
            bool isNLCodeExists = false;
            if (fabricType == "F" || fabricType == "A")
            {
                sqlGetNLCode = string.Format(sqlFA, "id");
                isNLCodeExists = MyUtility.Check.Seek(sqlGetNLCode, parGetNLCode, out drNLCode);
                if (!isNLCodeExists)
                {
                    sqlGetNLCode = string.Format(sqlFA, "BrandGroup");
                    isNLCodeExists = MyUtility.Check.Seek(sqlGetNLCode, parGetNLCode, out drNLCode);
                }
            }
            else
            {
                isNLCodeExists = MyUtility.Check.Seek(sqlGetNLCode, parGetNLCode, out drNLCode);
            }

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

        public static string GetNeedCreateAppShipMode()
        {
            return MyUtility.GetValue.Lookup("SELECT Stuff((select concat( ', ',ID)   from ShipMode where NeedCreateAPP = 1 FOR XML PATH('')),1,1,'') ");
        }

        #region P02 檢查狀態是否為Approved/Junk
        public static bool checkP02Status(string HCNo)
        {  // 該單Approved / Junk都不允許調整資料
            if (MyUtility.Check.Seek($@"select 1 from Express where id='{HCNo}' and status in ('Junk','Approved')"))
            {
                MyUtility.Msg.WarningBox($@"HC# {HCNo} already Approved or Junked, 
please check again.");
                return false;
            }
            else
            {
                return true;
            }
        }

          
    #endregion

    #region B42 檢查ID,NLCode,HSCode,UnitID Group後是否有ID,NLCode重複的資料
    public static bool CheckVNConsumption_Detail_Dup(DataRow[] checkList, bool isShowID)
        {
            var listDupNLCodeData = checkList
                                   .GroupBy(s => new { ID = s["ID"], NLCode = s["NLCode"], HSCode = s["HSCode"], UnitID = s["UnitID"] })
                                   .GroupBy(y => new { y.Key.ID, y.Key.NLCode })
                                   .Select(z => new { z.Key.ID, z.Key.NLCode, duplicateData = z.ToList() })
                                   .Where(x => x.duplicateData.Count > 1) // 抓出ID,NLCode相同，但HSCode,UnitID不同的資料
                                                                          // 回串原本的detail datatable抓出明細資料
                                   .Join(checkList,
                                            dupData => new { dupData.ID, dupData.NLCode },
                                            drDetail => new { ID = drDetail["ID"], NLCode = drDetail["NLCode"] },
                                            (dupData, drDetail) => new
                                            {
                                                dupData.ID,
                                                dupData.NLCode,
                                                Refno = drDetail["Refno"],
                                                HSCode = drDetail["HSCode"],
                                                UnitID = drDetail["UnitID"]
                                            });


            if (listDupNLCodeData.Any())
            {
                DataTable dtDuplicate = new DataTable();
                if (isShowID)
                {
                    dtDuplicate.ColumnsStringAdd("ID");
                }
                dtDuplicate.ColumnsStringAdd("NLCode");
                dtDuplicate.ColumnsStringAdd("Refno");
                dtDuplicate.ColumnsStringAdd("HSCode");
                dtDuplicate.ColumnsStringAdd("UnitID");

                foreach (var item in listDupNLCodeData)
                {
                    DataRow newDr = dtDuplicate.NewRow();
                    if (isShowID)
                    {
                        newDr["ID"] = item.ID;
                    }
                    newDr["NLCode"] = item.NLCode;
                    newDr["Refno"] = item.Refno;
                    newDr["HSCode"] = item.HSCode;
                    newDr["UnitID"] = item.UnitID;

                    dtDuplicate.Rows.Add(newDr);
                }

                MsgGridForm msgGridForm = new MsgGridForm(dtDuplicate);
                msgGridForm.Text = "The following data has different HSCode or Unit data from NLcode.";
                msgGridForm.grid1.AutoResizeColumns();
                msgGridForm.ShowDialog();
                return false;
            }
            return true;
        }
        #endregion

        #region B42 Batch Create 與Calculate用相同邏輯取資料
        public class ParGetVNConsumption_Detail_Detail
        {
            public DateTime? dateBuyerDeliveryFrom;
            public DateTime? dateBuyerDeliveryTo;
            public string Style;
            public string Category;
            public string BrandID;
            public string StyleUkey;
            public string SizeCode;
            public string Article;
        }
        public static DualResult GetVNConsumption_Detail_Detail(ParGetVNConsumption_Detail_Detail sqlPar, out DataTable dataTable)
        {
            StringBuilder sqlCmd = new StringBuilder();

            #region 組撈所有明細SQL
            sqlCmd.Append(@"
select  o.StyleUkey
        , oqd.SizeCode
        , oqd.Article
        , o.Category
        , o.StyleID
        , o.SeasonID
        , o.BrandID as OrderBrandID
		, s.FabricType
		, s.ThickFabric
        , sum(oqd.Qty) as GMTQty
        , isnull(s.CPU,0) as StyleCPU
        , isnull(s.CTNQty,0) as CTNQty
into #tmpAllStyle
from Order_QtyShip oq WITH (NOLOCK) 
inner join Orders o WITH (NOLOCK) on oq.ID = o.ID
inner join Order_QtyShip_Detail oqd WITH (NOLOCK) on oq.ID = oqd.ID 
                                                     and oq.Seq = oqd.Seq
left join Style s WITH (NOLOCK) on o.StyleUkey = s.Ukey
where   1=1");

            if (!MyUtility.Check.Empty(sqlPar.dateBuyerDeliveryFrom))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery >= '{0}' ", Convert.ToDateTime(sqlPar.dateBuyerDeliveryFrom).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sqlPar.dateBuyerDeliveryTo))
            {
                sqlCmd.Append(string.Format(" and oq.BuyerDelivery <= '{0}' ", Convert.ToDateTime(sqlPar.dateBuyerDeliveryTo).ToString("d")));
            }

            if (!MyUtility.Check.Empty(sqlPar.Style))
            {
                sqlCmd.Append(string.Format(" and o.StyleID = '{0}'", sqlPar.Style));
            }

            if (!MyUtility.Check.Empty(sqlPar.Category))
            {
                sqlCmd.Append(string.Format(" and o.Category = '{0}'", sqlPar.Category == "Bulk" ? "B" : "S"));
            }

            if (!MyUtility.Check.Empty(sqlPar.BrandID))
            {
                sqlCmd.Append(string.Format(" and o.BrandID = '{0}'", sqlPar.BrandID));
            }

            if (!MyUtility.Check.Empty(sqlPar.StyleUkey))
            {
                sqlCmd.Append(string.Format(" and o.StyleUkey = '{0}'", sqlPar.StyleUkey));
            }

            if (!MyUtility.Check.Empty(sqlPar.SizeCode))
            {
                sqlCmd.Append(string.Format(" and oqd.SizeCode = '{0}'", sqlPar.SizeCode));
            }

            if (!MyUtility.Check.Empty(sqlPar.Article))
            {
                sqlCmd.Append(string.Format(" and oqd.Article = '{0}'", sqlPar.Article));
            }
            sqlCmd.Append(@"
group by o.StyleUkey, oqd.SizeCode, oqd.Article, o.Category, o.StyleID
         , o.SeasonID, o.BrandID, isnull(s.CPU,0), isnull(s.CTNQty,0), s.FabricType, s.ThickFabric
		 
--------------------------------------------------------------------------------------------------------------------------------------------------
select  ts.*
        , sm.MarkerName
        , sm.FabricPanelCode
        , dbo.MarkerLengthToYDS(sm.MarkerLength) as markerYDS
        , sm.Width
        , sms.Qty
        , sc.FabricCode
        , sfqt.QTFabricCode
into #tmpMarkerData
from #tmpAllStyle ts
inner join Style_MarkerList sm WITH (NOLOCK) on sm.StyleUkey = ts.StyleUkey
inner join Style_MarkerList_SizeQty sms WITH (NOLOCK) on sm.Ukey = sms.Style_MarkerListUkey 
                                                         and sms.SizeCode = ts.SizeCode
inner join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sm.StyleUkey 
                                                and sc.FabricPanelCode = sm.FabricPanelCode
left join Style_MarkerList_Article sma WITH (NOLOCK) on sm.Ukey = sma.Style_MarkerListUkey 
left join Style_FabricCode_QT sfqt WITH (NOLOCK) on sm.FabricPanelCode = sfqt.FabricPanelCode 
                                                    and sm.StyleUkey = sfqt.StyleUkey
where   sm.MixedSizeMarker = 1 
        and (sma.Article is null or sma.Article = ts.Article) 
        and sc.Article = ts.Article
        and CHARINDEX('+',sm.MarkerLength) > 0

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.StyleID
        , t.SeasonID
        , t.OrderBrandID
        , t.Category
        , t.SizeCode
        , t.Article
        , t.GMTQty
        , t.markerYDS
        , t.Width
        , t.Qty
        , IIF(t.QTFabricCode is null, sb.SCIRefno, sb1.SCIRefno) as SCIRefNo
        , IIF(t.QTFabricCode is null, sb.SuppIDBulk, sb1.SuppIDBulk) as SuppIDBulk
        , t.StyleCPU
        , t.StyleUKey
into #tmpFabricCode
from #tmpMarkerData t
left join Style_BOF sb WITH (NOLOCK) on sb.StyleUkey = t.StyleUkey 
                                        and sb.FabricCode = t.FabricCode
left join Style_BOF sb1 WITH (NOLOCK) on sb1.StyleUkey = t.StyleUkey 
                                         and sb1.FabricCode = t.QTFabricCode

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.StyleID
        , t.SeasonID
        , t.OrderBrandID
        , t.Category
        , t.SizeCode
        , t.Article
        , t.GMTQty
        , t.StyleCPU
        , t.StyleUKey
        , t.markerYDS
        , 'YDS' as UsageUnit
        , t.Qty
        , f.SCIRefno
        , f.Refno
        , f.BrandID
        , f.NLCode
        , f.HSCode
        , f.CustomsUnit
        , f.Width,f.Type
        , f.PcsWidth
        , f.PcsLength
        , f.PcsKg
        , f.Description
        , isnull((select RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = f.CustomsUnit),1) as RateValue
        , (select RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = 'M') as M2RateValue
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = f.CustomsUnit),0) as UnitRate
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = 'M'),0) as M2UnitRate
		, [UsageQty] = t.markerYDS/t.Qty
        , [StockUnit] = StockUnit.val
		, [StockQty] = StockQty.val
into #tmpBOFRateData
from #tmpFabricCode t
inner join Fabric f WITH (NOLOCK) on f.SCIRefno = t.SCIRefno
outer apply(select [val] = dbo.getStockUnit(f.SCIRefno, default)) as StockUnit
outer apply(select [val] = dbo.getUnitRate('YDS',StockUnit.val) * t.markerYDS) as StockQty
where   (t.SuppIDBulk <> 'FTY' or t.SuppIDBulk <> 'FTY-C')
        and f.NoDeclare = 0

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , StyleCPU
        , StyleUKey
        , Description
        , IIF(Type = 'F','Fabric',IIF(Type = 'A','Accessory','')) as Type
        , ([dbo].getVNUnitTransfer(Type,StockUnit,CustomsUnit,StockQty,Width,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),iif(Qty=0.000,0.000,IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)),default)/Qty) as NewQty
		, StockUnit
		, [StockQty] = StockQty / Qty
        , UsageUnit
		, UsageQty
into #tmpBOFNewQty
from #tmpBOFRateData

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , sum(isnull(NewQty,0)) as Qty
        , 0 as LocalItem
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , '' as SuppID
		, StockUnit
		, [StockQty] = sum(isnull(StockQty,0))
        , [FabricType] = 'F' 
        , UsageUnit
		, [UsageQty] = sum(isnull(UsageQty,0))
into #tmpBOFData
from #tmpBOFNewQty
group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article
         , GMTQty, SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit
         , StyleCPU, StyleUKey, Description, Type, StockUnit, UsageUnit

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.*
        , sb.Ukey
        , sb.Refno
        , sb.SCIRefno
        , sb.SuppIDBulk
        , sb.SizeItem
        , sb.PatternPanel
        , sb.BomTypeArticle
        , sb.BomTypeColor
        , sb.ConsPC
        , sc.ColorID
        , f.UsageUnit
        , HSCode = isnull (f.HSCode, '')
        , NLCode = isnull (f.NLCode, '')
        , CustomsUnit = isnull (f.CustomsUnit, '')
        , f.PcsWidth
        , f.PcsLength
        , f.PcsKg
        , f.BomTypeCalculate
        , f.Type
        , f.BrandID
        , f.Description
into #tmpBOA
from #tmpAllStyle t
inner join Style_BOA sb WITH (NOLOCK) on t.StyleUkey = sb.StyleUkey
left join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sb.StyleUkey 
                                               and sc.PatternPanel = sb.PatternPanel 
                                               and sc.Article = t.Article
inner join Fabric f WITH (NOLOCK) on sb.SCIRefno = f.SCIRefno
where   sb.IsCustCD <> 2
        and (sb.SuppIDBulk <> 'FTY' and sb.SuppIDBulk <> 'FTY-C')
        and not exists(select 1 from MtlType mtl with (nolock) where f.MtlTypeID = mtl.ID and mtl.IsThread = 1)

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.*
        , [SizeSpec] = SizeSpec.val
        , isnull((select RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = t.CustomsUnit),1) as RateValue
        , (select RateValue from dbo.View_Unitrate where FROM_U = StockUnit.val and TO_U = 'M') as M2RateValue
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = t.CustomsUnit),'') as UnitRate
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = StockUnit.val and UnitTo = 'M'),'') as M2UnitRate
        , [StockUnit] = StockUnit.val
		, [StockQty] = StockQty.val
        , [UsageQty] = SizeSpec.val
into #tmpBOAPrepareData
from #tmpBOA t
outer apply(
	select SizeSpec
	from Style_SizeSpec 
	where   StyleUkey = t.StyleUkey 
            and SizeItem = t.SizeItem 
            and SizeCode = t.SizeCode 
)S
outer apply(select [val] = IIF(t.BomTypeCalculate = 1, isnull(dbo.GetDigitalValue(s.SizeSpec),0), ConsPC)) as SizeSpec
outer apply(select [val] = dbo.getStockUnit(t.SCIRefno,default)) as StockUnit
outer apply(select [val] = dbo.getUnitRate(t.UsageUnit,StockUnit.val) * SizeSpec.val) as StockQty
where   (t.BomTypeArticle = 0 and t.BomTypeColor = 0) 
        or ((t.BomTypeArticle = 1 or t.BomTypeColor = 1) and t.ColorID is not null)

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , StyleCPU
        , StyleUKey
        , Description
        , IIF(Type = 'F','Fabric',IIF(Type = 'A','Accessory','')) as Type
        , [dbo].getVNUnitTransfer(Type, StockUnit,CustomsUnit,StockQty,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate),default) as NewQty
        , StockUnit
		, StockQty
        , UsageUnit
		, UsageQty
into #tmpBOANewQty
from #tmpBOAPrepareData
--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , sum(ISNULL(NewQty,0)) as Qty
        , 0 as LocalItem
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , '' as SuppID
		, StockUnit
		, [StockQty] = sum(isnull(StockQty,0))
        , [FabricType] = 'A'
        , UsageUnit
		, [UsageQty] = sum(isnull(UsageQty,0))
into #tmpBOAData
from #tmpBOANewQty
group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article
         , GMTQty, SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit
         , StyleCPU, StyleUKey, Description, Type, StockUnit, UsageUnit

--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.*
        , ld.Refno
        , ld.Qty
        , ld.UnitId
        , li.MeterToCone
        , li.NLCode
        , li.HSCode
        , CustomsUnit = isnull (li.CustomsUnit, '')
        , li.PcsWidth
        , li.PcsLength
        , li.PcsKg
        , o.Qty as OrderQty
        , li.Description
        , li.Category as Type
        , li.LocalSuppid as SuppID
into #tmpLocalPO
from #tmpAllStyle t
inner join LocalPO_Detail ld WITH (NOLOCK) on ld.OrderId = (select TOP 1 ID 
                                                            from Orders WITH (NOLOCK) 
                                                            where   StyleUkey = t.StyleUkey 
                                                                    and Category = t.Category 
                                                            order by BuyerDelivery, ID)
inner join  LocalPO l with (nolock) on ld.ID = l.ID and l.Category not in ('SP_THREAD','EMB_THREAD')
left join LocalItem li WITH (NOLOCK) on li.RefNo = ld.Refno
left join Orders o WITH (NOLOCK) on ld.OrderId = o.ID
left join View_VNNLCodeWaste vd WITH (NOLOCK) on  vd.NLCode = li.NLCode
where li.NoDeclare = 0

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , StyleCPU
        , StyleUKey
        , Refno
        , Qty
        , OrderQty
        , IIF(UnitId = 'CONE','M',UnitId) as UnitId
        , NLCode
        , HSCode
        , CustomsUnit
        , PcsWidth
        , PcsLength
        , PcsKg
        , Description
        , Type
        , SuppID
into #tmpConeToM
from #tmpLocalPO

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , StyleCPU
        , StyleUKey
        , Refno
        , iif(OrderQty=0.000,0.000,Qty/OrderQty) as Qty,UnitId
        , NLCode
        , HSCode
        , CustomsUnit
        , PcsWidth
        , PcsLength
        , PcsKg
        , Description
        , Type
        , SuppID
        , isnull((select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = CustomsUnit),1) as RateValue
        , (select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = 'M') as M2RateValue
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = CustomsUnit),'') as UnitRate
        , isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = 'M'),'') as M2UnitRate
into #tmpPrepareRate
from #tmpConeToM

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , Refno as SCIRefno
        , Refno
        , '' as BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , SuppID
        , [dbo].getVNUnitTransfer(Category,UnitId,CustomsUnit,Qty,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate),Refno) as NewQty
		, [StockUnit] = UnitId
		, [StockQty] = Qty
        , [UsageUnit] = UnitId
		, [UsageQty] = Qty
into #tmpLocalNewQty
from #tmpPrepareRate
--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , sum(isnull(NewQty,0)) as Qty
        , 1 as LocalItem
        , StyleCPU
        , StyleUKey
        , Description
        , Type
        , SuppID
		, StockUnit
		, [StockQty] = sum(isnull(StockQty,0))
        , [FabricType] = 'L' 
        , UsageUnit
        , [UsageQty] = sum(isnull(UsageQty,0)) 
into #tmpLocalData
from #tmpLocalNewQty
group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article, GMTQty
         , SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit ,StyleCPU 
         , StyleUKey, Description, Type, SuppID, StockUnit, UsageUnit
----Get Thread Data---------------------------------------------------------------------------------------------------------------------------------
select 
	t.OrderBrandID,
	st.StyleUkey,
	st.Ukey,
	st.MachineTypeID,
	OpThreadQty = sum(sto.Frequency * op.SeamLength),
	[UseRatioRule] = iif(t.ThickFabric = 0,
		isnull(bt.UseRatioRule, b.UseRatioRule), 
		isnull(bt.UseRatioRule_Thick, b.UseRatioRule_Thick))
into #tmpOpThread
from Style_ThreadColorCombo st
inner join #tmpAllStyle t on t.StyleUkey = st.StyleUkey
left join Brand_ThreadCalculateRules bt with (nolock) on t.OrderBrandID = bt.ID and bt.FabricType = t.FabricType
left join Brand b with (nolock) on b.ID = t.OrderBrandID
inner join Style_ThreadColorCombo_Operation sto with (nolock) on st.Ukey = sto.Style_ThreadColorComboUkey
inner join Operation op with (nolock) on op.ID = sto.OperationID
group by t.OrderBrandID,st.StyleUkey,st.Ukey, st.MachineTypeID, 
			iif(t.ThickFabric = 0,isnull(bt.UseRatioRule, b.UseRatioRule),isnull(bt.UseRatioRule_Thick, b.UseRatioRule_Thick))

select	tot.StyleUkey,
		std.SCIRefNo,
		f.Refno,
		f.NLCode,
		f.HSCode,
		f.CustomsUnit,
		f.Description,
		f.Type,
		f.UsageUnit,
		f.PcsWidth,
		f.PcsLength,
		f.PcsKg,
		[StockQty] = sum(tot.OpThreadQty * isnull(mtor.UseRatio, mto.UseRatio) ) * vu.RateValue,
		[RateValue] = UnitRate.RateValue,
		[UnitRate] = UnitRate.Rate
into #tmpThread
from #tmpOpThread tot
inner join Style_ThreadColorCombo_Detail std with (nolock) on tot.Ukey = std.Style_ThreadColorComboUkey
inner join Fabric f with (nolock) on std.SCIRefNo = f.SCIRefno
inner join MachineType_ThreadRatio mto with (nolock) on mto.ID = tot.MachineTypeID and mto.Seq = std.Seq
left join MachineType_ThreadRatio_Regular mtor with (nolock) on mto.ID = mtor.ID and mto.Seq = mtor.Seq and mtor.UseRatioRule = tot.UseRatioRule
inner join View_Unitrate vu with (nolock) on vu.FROM_U = 'CM' and vu.TO_U = f.UsageUnit
outer apply(select RateValue,Rate  from  View_Unitrate where FROM_U = f.UsageUnit and TO_U = iif(f.CustomsUnit = 'M2','M',f.CustomsUnit)) UnitRate
group by tot.StyleUkey,
		std.SCIRefNo,
		f.Refno,
		f.NLCode,
		f.HSCode,
		f.CustomsUnit,
		f.Description,
		f.Type,
		f.UsageUnit,
		f.PcsWidth,
		f.PcsLength,
		f.PcsKg,
		UnitRate.RateValue,
		UnitRate.Rate,
		vu.RateValue

select  t.StyleID,
        t.SeasonID,
        t.OrderBrandID,
        t.Category,
        t.SizeCode,
        t.Article,
        t.GMTQty,
        th.SCIRefNo,
        th.Refno,
        [BrandID] = t.OrderBrandID,
        th.NLCode,
        th.HSCode,
        th.CustomsUnit,
        [Qty] = [dbo].getVNUnitTransfer(th.Type,StockUnit.val,th.CustomsUnit,StockQty.val,0,th.PcsWidth,th.PcsLength,th.PcsKg,th.RateValue,th.UnitRate,default),
        0 as LocalItem,
        t.StyleCPU,
        t.StyleUKey,
        th.Description,
        th.Type,
        '' as SuppID,
        [StockUnit] = StockUnit.val,
        [StockQty] = StockQty.val,
        [FabricType] = 'A',
        [UsageUnit] = th.UsageUnit,
		[UsageQty] = th.StockQty
into #tmpThreadData
from #tmpAllStyle t
inner join #tmpThread th on t.StyleUkey = th.StyleUkey
outer apply(select [val] = dbo.getStockUnit(th.SCIRefNo,default)) as StockUnit
outer apply(select [val] = dbo.getUnitRate(th.UsageUnit,StockUnit.val) * th.StockQty) as StockQty
--------------------------------------------------------------------------------------------------------------------------------------------------
select  t.StyleID
        , t.SeasonID
        , t.OrderBrandID
        , t.Category
        , t.SizeCode
        , Article = isnull (t.Article, '')
        , t.GMTQty
        , vfd.* 
        , sa.TissuePaper as ArticleTissuePaper
        , t.CTNQty
        , t.StyleCPU
        , t.StyleUKey
into #tmpFixDeclare
from VNFixedDeclareItem vfd WITH (NOLOCK) 
left join #tmpAllStyle t on 1 = 1
left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = t.StyleUkey 
                                            and sa.Article = t.Article

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , '' as SCIRefno
        , Refno
        , '' as BrandID
        , '' as SuppID
        , NLCode
        , HSCode
        , UnitID as CustomsUnit
        , IIF(Type = 1, Qty, IIF(CTNQty = 0,0,ROUND(Qty/CTNQty,3))) as Qty
        , 1 as LocalItem
        , StyleCPU
        , StyleUKey
        , '' as Description
        , '' as Type
		, StockUnit
		, [StockQty] = 0
        , FabricType
        , [UsageUnit] = StockUnit
		, [UsageQty] = 0
into #tmpFinalFixDeclare
from #tmpFixDeclare
where   TissuePaper = 0 
        or (TissuePaper = 1 and ArticleTissuePaper = 1)

--------------------------------------------------------------------------------------------------------------------------------------------------
select  StyleID
        , SeasonID
        , OrderBrandID
        , Category
        , SizeCode
        , Article
        , GMTQty
        , SCIRefno
        , Refno
        , BrandID
        , NLCode
        , HSCode
        , CustomsUnit
        , Qty
        , LocalItem
        , StyleCPU
        , StyleUKey
		, StockUnit
		, StockQty
        , FabricType
        , UsageUnit
        , UsageQty
into #tlast
from (
    select  StyleID
            , SeasonID
            , OrderBrandID
            , Category
            , SizeCode
            , Article
            , GMTQty
            , SCIRefno
            , Refno
            , BrandID
            , NLCode
            , HSCode
            , CustomsUnit
            , Qty
            , LocalItem
            , StyleCPU
            , StyleUKey
            , Description
            , Type
            , SuppID 
			, StockUnit
			, StockQty
            , FabricType
            , UsageUnit
            , UsageQty
    from #tmpFinalFixDeclare
    union
    select  StyleID
            , SeasonID
            , OrderBrandID
            , Category
            , SizeCode
            , Article
            , GMTQty
            , SCIRefno
            , Refno
            , BrandID
            , NLCode
            , HSCode
            , CustomsUnit
            , sum(Qty) as Qty
            , LocalItem
            , StyleCPU
            , StyleUKey
            , Description
            , Type
            , SuppID
			, StockUnit
			, [StockQty] = sum(StockQty)
            , FabricType
            , UsageUnit
            , [UsageQty] = sum(UsageQty)
    from (
        select * 
        from #tmpBOFData

        union all
        select * 
        from #tmpBOAData

        union all
        select * 
        from #tmpLocalData

        union all
        select * 
        from #tmpThreadData
    ) a
    group by StyleID, SeasonID, OrderBrandID, Category, SizeCode, Article
             , GMTQty, SCIRefno, Refno, BrandID, NLCode, HSCode, CustomsUnit
             , LocalItem, StyleCPU, StyleUKey,Description,Type,SuppID,StockUnit, FabricType, UsageUnit
)x

--------------------------------------------------------------------------------------------------------------------------------------------------
select 	[ID] = ''
        ,[UnitID] = t.CustomsUnit
        ,t.*
from #tlast t 

--------------------------------------------------------------------------------------------------------------------------------------------------
drop table #tmpAllStyle
drop table #tmpMarkerData
drop table #tmpFabricCode
drop table #tmpBOFRateData
drop table #tmpBOFNewQty
drop table #tmpBOFData
drop table #tmpBOA
drop table #tmpBOAPrepareData
drop table #tmpBOANewQty
drop table #tmpBOAData
drop table #tmpLocalPO
drop table #tmpConeToM
drop table #tmpPrepareRate
drop table #tmpLocalNewQty
drop table #tmpLocalData
drop table #tmpFixDeclare
drop table #tmpFinalFixDeclare
drop table #tlast
drop table #tmpOpThread
drop table #tmpThread
drop table #tmpThreadData");
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out dataTable);

            return result;
        }

        #endregion


    }
}
