Create PROCEDURE [dbo].[P_Import_PPIC_R04]
As
Begin
	Set NoCount On;
		
	declare @current_ServerName varchar(50) = (SELECT [Server Name] = @@SERVERNAME)
	declare @current_PMS_ServerName nvarchar(50) 
	= (
		select [value] = 
			CASE WHEN @current_ServerName= 'PHL-NEWPMS-02' THEN 'PHL-NEWPMS' -- PH1
				 WHEN @current_ServerName= 'VT1-PH2-PMS2b' THEN 'VT1-PH2-PMS2' -- PH2
				 WHEN @current_ServerName= 'system2017BK' THEN 'SYSTEM2017' -- SNP
				 WHEN @current_ServerName= 'SPS-SQL2' THEN 'SPS-SQL.spscd.com' -- SPS
				 WHEN @current_ServerName= 'SQLBK' THEN 'PMS-SXR' -- SPR
				 WHEN @current_ServerName= 'newerp-bak' THEN 'newerp' -- HZG		
				 WHEN @current_ServerName= 'SQL' THEN 'MainServer' -- HXG
				 WHEN (select top 1 MDivisionID from Production.dbo.Factory) in ('VM2','VM1') then 'SYSTEM2016' -- ESP & SPT
				 WHEN @current_ServerName= 'TESTING\PH1' THEN 'TESTING\PH1' -- testing\ph1
			ELSE '' END
	)


	Declare @ExecSQL1 NVarChar(MAX);
	Declare @ExecSQL2 NVarChar(MAX);
	Set @ExecSQL1 =
		N'select distinct 
				[SewingCell] = isnull(s.SewingCell,'''')
				,[LineID] = l.SewingLineID
				,[ReplacementID] = l.ID
				,[StyleID] = isnull(o.StyleID,'''')
				,[SP] = l.OrderID
				,[Seq] = concat(ld.Seq1,'' '',ld.Seq2)
				,[FabricType] = l.FabricType
				,[Color] = isnull(c.Name,'''')
				,[Refno] = isnull(psd.Refno,'''')
				,l.ApvDate
				,[NoOfPcsRejected] = ld.RejectQty
				,[RequestQtyYrds] = ld.RequestQty
				,[IssueQtyYrds] = ld.IssueQty
				,[ReplacementFinishedDate] = IIF(l.Status= ''Received'',l.EditDate,null)
				,[Type] = IIF(l.Type=''R'',''Replacement'',''Lacking'')
				,ld.Process
				,[Description] = isnull(IIF(l.FabricType = ''F'',pr.Description,pr1.Description),PPICReasonID)
				,[OnTime] = IIF(l.Status = ''Received'',IIF(DATEDIFF(ss,l.ApvDate,l.EditDate) <= 10800,''Y'',''N''),''N'')
				,[Remark] = isnull(l.Remark,'''')    
			into #tmpFinal
			from ['+@current_PMS_ServerName+'].Production.dbo.Lack l 
			inner join ['+@current_PMS_ServerName+'].Production.dbo.Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
			left join ['+@current_PMS_ServerName+'].Production.dbo.SewingLine s WITH (NOLOCK) on s.ID = l.SewingLineID AND S.FactoryID=L.FactoryID
			left join ['+@current_PMS_ServerName+'].Production.dbo.Orders o WITH (NOLOCK) on o.ID = l.OrderID
			left join ['+@current_PMS_ServerName+'].Production.dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
			left join ['+@current_PMS_ServerName+'].Production.dbo.PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = ''Color''
			left join ['+@current_PMS_ServerName+'].Production.dbo.Color c WITH (NOLOCK) on c.BrandId = o.BrandID and c.ID = isnull(psdsC.SpecValue ,'''')
			left join ['+@current_PMS_ServerName+'].Production.dbo.PPICReason pr WITH (NOLOCK) on pr.Type = ''FL'' and (pr.ID = ld.PPICReasonID or pr.ID = concat(''FR'',''0'',ld.PPICReasonID))
			left join ['+@current_PMS_ServerName+'].Production.dbo.PPICReason pr1 WITH (NOLOCK) on pr1.Type = ''AL'' and (pr1.ID = ld.PPICReasonID or pr1.ID = concat(''AR'',''0'',ld.PPICReasonID))
			where l.EditDate >= DATEADD(MONTH, -1, GETDATE())
			';
	set @ExecSQL2 = N'	
	Merge P_FabricStatus_And_IssueFabricTracking t
	using( select * from #tmpFinal) as s
	on t.ReplacementID = s.ReplacementID and t.SP = s.SP and t.Seq = s.Seq and t.RefNo = s.RefNo
	WHEN MATCHED then UPDATE SET
	   t.[SewingCell] = s.[SewingCell]
      ,t.[LineID] = s.[LineID]
      ,t.[StyleID] = s.[StyleID]
      ,t.[FabricType] = s.[FabricType]
      ,t.[Color] = s.[Color]
      ,t.[ApvDate] = s.[ApvDate]
      ,t.[NoOfPcsRejected] = s.[NoOfPcsRejected]
      ,t.[RequestQtyYrds] = s.[RequestQtyYrds]
      ,t.[IssueQtyYrds] = s.[IssueQtyYrds]
      ,t.[ReplacementFinishedDate] = s.[ReplacementFinishedDate]
      ,t.[Type] = s.[Type]
      ,t.[Process] = s.[Process]
      ,t.[Description] = s.[Description]
      ,t.[OnTime] = s.[OnTime]
      ,t.[Remark] = s.[Remark]
	WHEN NOT MATCHED BY TARGET THEN
		insert ([SewingCell]      ,[LineID]      ,[ReplacementID]
		  ,[StyleID]      ,[SP]     ,[Seq]      ,[FabricType]
		  ,[Color]      ,[RefNo]      ,[ApvDate]
		  ,[NoOfPcsRejected]      ,[RequestQtyYrds]      ,[IssueQtyYrds]
		  ,[ReplacementFinishedDate]      ,[Type]
		  ,[Process]      ,[Description]      ,[OnTime]      ,[Remark])
		VALUES (s.[SewingCell]      ,s.[LineID]      ,s.[ReplacementID]
		  ,s.[StyleID]    ,s.[SP]     ,s.[Seq]      ,s.[FabricType]
		  ,s.[Color]      ,s.[RefNo]      ,s.[ApvDate]
		  ,s.[NoOfPcsRejected]      ,s.[RequestQtyYrds]      ,s.[IssueQtyYrds]
		  ,s.[ReplacementFinishedDate]      ,s.[Type]
		  ,s.[Process]      ,s.[Description]  ,s.[OnTime]      ,s.[Remark])
	when not matched by source and t.[ReplacementFinishedDate] >= DATEADD(MONTH, -1, GETDATE())
	then delete;
	'
	--print @ExecSQL1 + @ExecSQL2
	Exec (@ExecSQL1 + @ExecSQL2);

	if exists (select 1 from BITableInfo b where b.id = 'P_FabricStatus_And_IssueFabricTracking')
	begin
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = 'P_FabricStatus_And_IssueFabricTracking'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_FabricStatus_And_IssueFabricTracking', getdate())
	end
End


