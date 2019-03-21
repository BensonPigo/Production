USE [Production]
GO

/****** Object:  View [dbo].[View_tOutputTotal]    Script Date: 3/14/2019 5:57:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



/*
If Object_Id ( 'GetName', 'V' ) Is Not Null
   Drop View GetName;
*/

CREATE VIEW [dbo].[View_tOutputTotal]
AS
		SELECT 
				 [nid] 
				,[dDate]
				,[dTime] 
				,[MONo] = [MONo] collate Chinese_Taiwan_Stroke_CI_AS
				,[ColorName] = [ColorName] collate Chinese_Taiwan_Stroke_CI_AS
				,[SizeName] = [SizeName] collate Chinese_Taiwan_Stroke_CI_AS
				,[ProPart] = [ProPart] collate Chinese_Taiwan_Stroke_CI_AS
				,[WorkShop] = [WorkShop] collate Chinese_Taiwan_Stroke_CI_AS
				,[WorkLine] = [WorkLine] collate Chinese_Taiwan_Stroke_CI_AS
				,[InputQty]
				,[OutputQty] 
				,[FailQty]
				,[GenerateTime] 
				,[InterfaceTime] 
		FROM [SUNRISE].SUNRISEEXCH.dbo.tOutputTotal

GO

