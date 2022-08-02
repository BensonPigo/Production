


CREATE view  [dbo].[View_Unitrate]
as
SELECT TMP.*,ISNULL(R.Rate,1) Rate, ISNULL( IIF(Denominator = 0,0, R.Numerator / R.Denominator),1) RateValue
FROM (SELECT A.ID FROM_U,B.ID TO_U FROM DBO.Unit A,DBO.UNIT B) TMP 
LEFT JOIN DBO.Unit_Rate R ON TMP.FROM_U = R.UnitFrom AND TMP.TO_U = R.UnitTo