USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_Order]    Script Date: 2016/9/2 ¤W¤È 10:39:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Willy S01910>
-- Create date: <2016/08/17>
-- Description:	<exp_Order>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Order]
	
AS
BEGIN

IF OBJECT_ID(N'Orders') IS NOT NULL
BEGIN
  DROP TABLE Orders
END

IF OBJECT_ID(N'FactoryOrder') IS NOT NULL
BEGIN
  DROP TABLE FactoryOrder
END

IF OBJECT_ID(N'OutStandingRecord') IS NOT NULL
BEGIN
  DROP TABLE OutStandingRecord
END

SELECT Orders.ID, Orders.MCHandle, 
Isnull(Pass1.Email,'') as MCEMail, isnull(Pass1.ExtNo,'') as MCEXT
INTO Orders
FROM Production.dbo.Orders 
left join Pass1 on Pass1.ID= Orders.MCHandle
WHERE Orders.SciDelivery BETWEEN Convert(DATE,DATEADD(day,-60,GETDATE())) AND CONVERT(date, DATEADD(month,1 ,DATEADD(month,  3, GetDate())-10+1)-1) 
AND Orders. LocalOrder = 0 
ORDER BY Orders.ID 

SELECT FactoryID , StyleUkey,ID, BrandID, StyleID, SeasonID, BuyerDelivery, SciDelivery, CFMDate, Junk, CdCodeID, CPU, Qty, StyleUnit, MCHandle, AddName, AddDate, EditName, EditDate,
(SELECT isnull(Email,'') FROM Pass1 WHERE ID = Orders.MCHandle) AS MCEMAIL,
(SELECT isnull(ExtNo,'') FROM Pass1 WHERE ID = Orders.MCHandle)AS MCEXT,
ProgramID,  SubconInSisterFty
INTO FactoryOrder	
 FROM  Production.dbo.Orders 
WHERE SciDelivery >= Convert(DATE,DATEADD(day,-60,GETDATE())) AND Orders. LocalOrder = 1 
 ORDER BY ID 

SELECT ID, OutstandingReason, OutstandingRemark, OutstandingInCharge,OutstandingDate 
INTO OutStandingRecord
FROM Production.dbo.Orders 
WHERE OutstandingDate BETWEEN Convert(DATE,DATEADD(day,-60,GETDATE())) AND CONVERT(date, DATEADD(month,1 ,DATEADD(month,  3, GetDate())-10+1)-1) AND  Orders. LocalOrder = 0 
 ORDER BY Orders.ID


	
END

GO


