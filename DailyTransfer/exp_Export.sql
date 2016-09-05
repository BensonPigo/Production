USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Export]    Script Date: 2016/9/2 上午 10:38:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_Export>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Export]
AS
BEGIN

IF OBJECT_ID(N'Export') IS NOT NULL
BEGIN
  DROP TABLE Export
END

IF OBJECT_ID(N'ShareExpense') IS NOT NULL
BEGIN
  DROP TABLE ShareExpense
END

IF OBJECT_ID(N'DeleeShareExpense') IS NOT NULL
BEGIN
  DROP TABLE DeleeShareExpense
END

IF OBJECT_ID(N'tmpDeleteWKNo') IS NOT NULL
BEGIN
  DROP TABLE tmpDeleteWKNo
END

	SELECT Id, PortArrival, WhseArrival, PackingArrival,DocArrival
INTO Export
FROM Production.dbo.Export
WHERE  EditDate  BETWEEN CONVERT(DATE, GetDate()-7) and CONVERT(date,(GetDate()+1) )

--撈取ShareExpense有被刪除過的記錄
select distinct WKNo into tmpDeleteWKNo from Production.cdc.dbo_ShareExpense_CT where __$operation = 1 and WKNo <> ''


--撈取要轉出的資料
Select se.BLNo,se.WKNo,se.AccountNo,SUM(se.Amount*exchange.rate) as Amount
Into ShareExpense
from [Production].[dbo].ShareExpense se
outer apply(select * from [Production].[dbo].[GetCurrencyRate](Null,se.CurrencyID,'USD',null)) as exchange
Where exists (select 1 from (Select distinct WKNo
From [Production].[dbo].ShareExpense
Where EditDate >= CONVERT(DATE,DATEADD(day,-15, GETDATE()))) a 
where a.WKNo = se.WKNo)
group by se.BLNo,se.WKNo,se.AccountNo,se.CurrencyID


--撈取要轉出的被刪除的資料
Select t.WKNo 
into DeletedShareExpense 
from tmpDeleteWKNo t 
where not exists (select 1 from [Production].[dbo].ShareExpense s where t.WKNo = s.WKNo)

--釋放tmpDeleteWKNo
IF OBJECT_ID(N'tmpDeleteWKNo') IS NOT NULL
BEGIN
  DROP TABLE tmpDeleteWKNo
END

END

GO


