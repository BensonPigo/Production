CREATE VIEW [dbo].[P_Factory]
	AS 
	SELECT	[FtyCode] = ID, [SdpKpiCode] = KPICode, Junk
	FROM [MainServer].Production.dbo.SCIFty
