--defect 形式: 空白, A1, AA4
--最後一碼數字最大為 4 所以只會有 1 碼
--default type = 0 排除數字, 取 defectID
--type = 1 排除字母取 point
--</summary>
CREATE FUNCTION SplitDefectNum
(
	@defect varchar(4), @type int
)
RETURNS varchar(2)
AS
BEGIN
	if @type = 0
	begin
		RETURN SUBSTRING(@defect, 0, PATINDEX('%[0-9]%',@defect))
	end
	else
	begin
		RETURN SUBSTRING(@defect, PATINDEX('%[0-9]%',@defect),4)
	end

	RETURN ''
END