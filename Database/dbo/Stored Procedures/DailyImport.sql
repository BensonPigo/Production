
-- =============================================
-- Author:		<Ben S01571>
-- Create date: <2016/09/07>
-- Description:	<>
-- =============================================
Create Procedure [dbo].[DailyImport]
(
	@GroupID	Int = 0
)
As

Begin
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	Set NoCount On;
	
	Declare @Progress VarChar(Max);
	Declare @RowID Int = 0 ;
	Declare @RowCount Int = 0;
	Declare @Name NVarChar(100);
	Declare @TSql NVarChar(Max);

	Select Identity(BigInt, 1, 1) as RowID, *
	  Into #tmpExport
	  From Production.dbo.TransImport
	 Where (   (IsNull(@GroupID, 0) = 0)
			Or (IsNull(@GroupID, 0) > 0 And TransImport.GroupID = @GroupID)
		   )
	 Order by GroupID, Seq;
	
	Select @RowID = Min(RowID), @RowCount = Max(RowID) From #tmpExport;
	While @RowID <= @RowCount
	Begin
		Select @Name = #tmpExport.Name
			 , @TSql = #tmpExport.TSql
		  From #tmpExport
		 Where #tmpExport.RowID = @RowID;
		
		Set @Progress = @Name;
		RaisError (@Progress, 0, 1) With NoWait;

		Exec Sp_ExecuteSql @TSql;

		Set @RowID += 1;
	End;
End