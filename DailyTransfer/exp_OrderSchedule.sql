USE [Pms_To_Trade]
GO

/****** Object:  StoredProcedure [dbo].[exp_OrderSchedule]    Script Date: 2016/9/2 ¤W¤È 10:39:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Leo S01912>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[exp_OrderSchedule]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.OrderSchedule') IS NOT NULL
BEGIN
DROP TABLE OrderSchedule
END

Select o.ID, o.SewInLine, o.SewOffLine, o.PulloutDate, o.InspDate, (select CutInLine from [Production].dbo.Cutting where ID = o.CuttingSP) as CutInLine, (select CutOffLine from [Production].dbo.Cutting where ID = o.CuttingSP) as CutOffLine
INTO OrderSchedule
from [Production].dbo.Orders o
where o.SCIDelivery <= EOMONTH(GETDATE(),3)
and o.Finished = 0
and o.Qty > 0
and o.IsForecast = 0
and o.LocalOrder = 0
and o.SewInLine is not null
and o.SewOffLine is not null

END

GO


