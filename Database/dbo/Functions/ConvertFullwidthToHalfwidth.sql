CREATE FUNCTION dbo.ConvertFullwidthToHalfwidth (@InputString VARCHAR(MAX))
RETURNS VARCHAR(MAX)
AS
BEGIN
    RETURN 
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
		REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(
        REPLACE(@InputString, N'０', N'0'),N'１', N'1'), 
                      N'２', N'2'), 
                      N'３', N'3'), 
                      N'４', N'4'), 
                      N'５', N'5'), 
                      N'６', N'6'), 
                      N'７', N'7'), 
                      N'８', N'8'), 
                      N'９', N'9'), 
					  N'ｄ', N'd'),
                      N'Ｙ', N'Y'), 
                      N'－', N'-'), 
                      N'＋', N'+'), 
                      N'＂', N'"'), 
                      N'／', N'/')
END;
GO