
CREATE VIEW [dbo].[View_SewingInfoSP]
as
SELECT SD.OrderId,Max(S.OutputDate) AS [LastSewDate], Sum(SDD.QAQty) AS [SewQty]
FROM SewingOutput_Detail SD with (nolock)
INNER JOIN SewingOutput_Detail_Detail SDD with (nolock) ON SDD.SewingOutput_DetailUKey = SD.UKey
INNER JOIN SewingOutput S with (nolock) ON S.ID= SD.ID
group by SD.OrderId