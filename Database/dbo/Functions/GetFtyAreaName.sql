Set Ansi_Nulls On
Go
Set Quoted_Identifier On
Go
-- =============================================
-- Author:		Edward
-- Create date: 2017/08/21
-- Description:
--		���ogetFtyAreaName�A���o�u�t�ϰ�W�١AFor Planning�����
-- =============================================
If Object_Id ( 'dbo.GetFtyAreaName') Is Not Null
    Drop Function dbo.GetFtyAreaName;
Go

CREATE FUNCTION GetFtyAreaName
(
	@FactoryID varchar(8)
)
RETURNS nvarchar(max)
AS
BEGIN
	
	return (select iif(Factory.Zone <> '', Factory.Zone, iif(Factory.Type = 'S', 'Sample', Factory.Zone)) as MDivisionID 
	from Factory where id = @FactoryID)

END
GO

