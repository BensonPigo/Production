
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[insertMygridstd2] 
	-- Add the parameters for the stored procedure here
	(@programid as varchar(80), @table as varchar(50))
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @seq as int  = 10, @columNM varchar(100), @datatype varchar(30), @length int;
	
	DECLARE contact_cursor CURSOR FOR
	SELECT COLUMN_name,data_type,CHARACTER_MAXIMUM_LENGTH FROM Information_Schema.COLUMNS where TABLE_NAME=@table;

	
	OPEN contact_cursor;

	FETCH NEXT FROM contact_cursor INTO @columNM, @datatype, @length;

	WHILE @@FETCH_STATUS = 0
	BEGIN
	   -- This is executed as long as the previous fetch succeeds.
		INSERT INTO [dbo].[MYGRIDSTD2]
           ([FORMNAME]
           ,[SEQUENCE]
           ,[GRIDCONTROLSOURCE]
           ,[GRIDHEADER]
           ,[GRIDWIDTH]

           ,[HASDEFAULT]
           ,[SELEFIELD]
           ,[FIELDTYPE]
           ,[SELEORDER]
           ,[LOCATEFOR])
		 VALUES
			   (@programid
			   ,@seq
			   ,@table+'.'+ @columNM
			   ,@columNM
			   ,iif( @LENGTH <10 ,10,
					case when @datatype = 'datetime' then 20  
						when @datatype = 'bit' then 5 
						else  @length 
					end)
			   ,1
			   ,1
			   ,case when @datatype = 'char' then 'C' 
					when @datatype = 'varchar' then 'C' 
					when @datatype = 'datetime' then 'D' 
					when @datatype = 'int' then 'N' 
					when @datatype = 'numeric' then 'N' 
					when @datatype='bit' then 'L' 
					else '' 
				end
			   ,1
			   ,1);

	   set @seq +=10 ; 
	   FETCH NEXT FROM contact_cursor INTO @columNM, @datatype, @length;
	END

	CLOSE contact_cursor;
	DEALLOCATE contact_cursor;

END