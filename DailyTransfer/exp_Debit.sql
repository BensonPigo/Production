USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Debit]    Script Date: 2016/9/2 ¤W¤È 10:38:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_debit>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Debit]
AS

IF OBJECT_ID(N'dbo.Debit') IS NOT NULL
BEGIN
  DROP TABLE Debit
END


IF OBJECT_ID(N'Debit5') IS NOT NULL
BEGIN
  DROP TABLE Debit5
END


SELECT * 
INTO Debit
FROM Production.dbo.Debit
WHERE EditDate 
BETWEEN Convert(DATE,DATEADD(day,-30,GETDATE()))AND CONVERT(date,Getdate()) OR SysDate BETWEEN Convert(DATE,DATEADD(day,-30,GETDATE())) AND CONVERT(date,Getdate())
ORDER BY ID 


Select ds.* 
INTO Debit5
from Production.dbo.Debit_Schedule ds
inner join Production.dbo.Debit d on ds.ID = d.ID
where exists (select 1 from Production.dbo.Debit_Schedule tds 
where tds.ID = ds.ID 
and (tds.AddDate between Convert(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date,Getdate())
or tds.EditDate between  Convert(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date,Getdate())
or tds.SysDate between  Convert(DATE,DATEADD(day,-30,GETDATE())) AND  CONVERT(date,Getdate()))
)


GO


