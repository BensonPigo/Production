USE [Production]
GO

/****** Object:  View [dbo].[GetName]    Script Date: 3/14/2019 5:49:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
If Object_Id ( 'GetName', 'V' ) Is Not Null
   Drop View GetName;
*/

CREATE VIEW [dbo].[View_tReworkTotal]
AS

	SELECT [nid]
		  ,[dDate]
		  ,[EmpID] = [EmpID] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[EmpName] = [EmpName] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[MONo] = [MONo] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[ColorName] = [ColorName] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[SizeName] = [SizeName] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[SeqCode] = [SeqCode] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[SeqNo]
		  ,[ProPart] = [ProPart] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[WorkShop] = [WorkShop] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[WorkLine] = [WorkLine] collate Chinese_Taiwan_Stroke_CI_AS
		  ,[StationID]
		  ,[Qty]
		  ,[FailCode] 
		  ,[GenerateTime]
		  ,[InterfaceTime]
	FROM [SUNRISE].SUNRISEEXCH.dbo.[tReworkTotal]
GO

