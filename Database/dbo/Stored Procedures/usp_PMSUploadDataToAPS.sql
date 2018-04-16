
-- =============================================
-- Author:		<JEFF S01952>
-- Create date: <2016/11/19>
-- Description:	<PMSUploadDataToAPS>
-- =============================================
CREATE PROCEDURE [dbo].[usp_PMSUploadDataToAPS]

AS
BEGIN
declare @ServerName varchar(50)='', @DatabaseName varchar(20)='', @loginId varchar(20)='', @LoginPwd varchar(20)=''
	--select出目標table所在位置
	BEGIN
		select TOP 1
			@ServerName = [SQLServerName],
			@DatabaseName = [APSDatabaseName],
			@loginId = [APSLoginId],
			@LoginPwd = [APSLoginPwd]
		from [Production].[dbo].system
	END
	
	--若其中一欄位空白則不執行此程式
	IF @ServerName ='' or @DatabaseName = '' or @loginId = '' or @LoginPwd = '' 
	BEGIN
		PRINT 'Connection information has not set' 
		RETURN  
	END
	
	--若不存在則新增連線	
	IF NOT EXISTS (SELECT * FROM sys.servers WHERE name = @ServerName)
	BEGIN
		--新建連線
		EXEC master.dbo.sp_addlinkedserver @server = @ServerName, @srvproduct=N'SQL Server'
		--設定連線登入資訊
		EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname = @ServerName, @locallogin = NULL , @useself = N'False', @rmtuser = @loginId, @rmtpassword = @LoginPwd
	END

	BEGIN
--cmd,cmd2連的Table
--...[dbo].[OPDWF510]
-------------------------------------------------第一部分-------------------------------------------------
Declare @SerDbDboTb varchar(66)
Set @SerDbDboTb = concat('[',@ServerName,'].[',@DatabaseName,N'].[dbo].[OPDWF510]')
Declare @cmd varchar(max)
Declare @cmd2 varchar(max)
--Update
set @cmd =N'	
Update t
set DELF=''Y'',
	UPDT= format(GETDATE(),''yyyy-MM-dd'')
FROM '+@SerDbDboTb+N' t
where 
not exists(select 1 from [Production].dbo.Orders o where t.SONO collate Chinese_Taiwan_Stroke_CI_AS =o.ID and t.NCTR collate Chinese_Taiwan_Stroke_CI_AS = o.FtyGroup)
and(CONVERT(date, OTDD collate Chinese_Taiwan_Stroke_CI_AS) >= DATEADD(DAY, -60, GETDATE()) 
or CONVERT(date, COTD collate Chinese_Taiwan_Stroke_CI_AS) >= DATEADD(DAY, -60, GETDATE()))
and DELF <> ''Y'''
-------------------------------------------------第二部分-------------------------------------------------
set @cmd2 =N'
IF OBJECT_ID(''tempdb.dbo.#tmp'', ''U'') IS NOT NULL DROP TABLE #tmp
Select 
[sRCID] = concat(o.FtyGroup, o.ID,oq.Article, oq.SizeCode, ''_'', IIF(o.StyleUnit = ''PCS'', '''', sl.Location))
,[sSONO] = o.ID
,[sLOT] = 1
,[sCRNM] = concat(o.LocalMR ,''-'', P1.Name)
,[sPRIO] = 1
,[sODST] = o.CDCodeID
,[sNCTR] = o.FtyGroup
,[sCSSE] = o.SeasonID
,[sCSNM] = o.BrandID
,[sCUNM] = CUNM.NameEN
,[sCFTY] = o.StyleID
,[sSYD1] = SYD1.Description
,[sGTMH] = GTMH.GTMH
,[sOTDD] = format(o.SCIDelivery,''yyyy-MM-dd'')
,[sCOTD] = format(o.BuyerDelivery,''yyyy-MM-dd'')
,[sOTTD] = IIF(o.LETA is null, '''', format(o.LETA,''yyyy-MM-dd''))
,[sQTYN] = oq.Qty
,[sFIRM] = ''Y''
,[sCOLR] = oq.Article
,[sSZE] = oq.SizeCode
,[sSHIP] = SHIP.Alias
,[sSMOD] = o.ShipModeList
,[sPlcOrdDate] = IIF(o.CFMDate is null,'''', format(o.CFMDate,''yyyy-MM-dd''))
,[sREMK] = REMK.REMK
,[sAOTT] = IIF(o.MTLETA is null, '''', format(o.MTLETA, ''yyyy-MM-dd''))
,[sUPUS] = ''SCIMIS''
,[sUPNM] = ''SCIMIS''
,[sSYCO] = IIF(o.StyleUnit = ''PCS'', '''', sl.Location)
,[sMASTERMATERIALDATE] = IIF(o.SewETA is null, null, format(o.SewETA, ''yyyy-MM-dd''))
,[sMASTERMATERIALRECEIVEDDATE] = IIF(o.MTLETA is null, null, format(o.MTLETA, ''yyyy-MM-dd''))
,[sMATERIALDATE] = IIF(o.PackETA is null, null,format(o.PackETA, ''yyyy-MM-dd''))
,[sMATERIALRECEIVEDDATE] = IIF(o.MTLETA is null, null, format(o.MTLETA, ''yyyy-MM-dd''))
,[sPPRO] = IIF(o.EachConsApv is null, IIF(o.KPIEachConsApprove is null, '''', format(o.KPIEachConsApprove, ''yyyy-MM-dd'')), format(o.EachConsApv, ''yyyy-MM-dd''))
,[sPRGM] = PRGM.PRGM
,o.Junk
,o.PulloutComplete
,o.Finished
,[sCUSY] = isnull(MasterStyleID1.MasterStyleID,MasterStyleID2.MasterStyleID)
,[sCUSTOMERORDERNO] = o.orderTypeID
into #tmp
From [Production].dbo.Orders o
inner join Factory on Factory.id = o.FactoryID and Factory.IsProduceFty = 1
outer apply (select [dbo].getMTLExport(o.POID,o.MTLExport) as mtlOk )as mtlExport
outer apply (select [dbo].GetHaveDelaySupp(o.POID)  as SuppDelay) as SDelay
Inner Join [Production].dbo.Order_Qty oq on o.ID = oq.ID
left join Style_Location sl on sl.StyleUkey = o.StyleUkey
outer apply(select Name from Pass1 where ID = o.LocalMR) P1
outer apply(Select NameEN from Brand where ID = o.BrandID) CUNM
outer apply(Select Description from Style where Ukey = o.StyleUKey) SYD1
outer apply(Select Alias from Country where ID = o.Dest) SHIP
outer apply(
	select GTMH = (
		SELECT
		IIF(o.StyleUnit = ''PCS'',(
		Select TotalSewingTime = sum(t.TotalSewingTime)
		from TimeStudy t
		where T.StyleID = o.StyleID 
		and T.SeasonID = o.SeasonID 
		and T.BrandID = o.BrandID 
		),(
		Select t.TotalSewingTime
		from TimeStudy t
		where T.StyleID = o.StyleID 
		and T.SeasonID = o.SeasonID 
		and T.BrandID = o.BrandID 
		and T.ComboType = sl.Location
		))
	)
)GTMH
outer apply(
	select REMK = (
		concat(
			''KPIL/ETA: '' 
			,IIF(o.KPILETA is null, '''', format(o.KPILETA, ''yyyy-MM-dd'')) , '', SCHD L/ETA(Master SP): '' 			
			,IIF(o.LETA is null, '''',format(o.LETA, ''yyyy-MM-dd'')) ,'','',char(13),''Sew. MTL ETA (SP): '' 			
			,IIF(o.SewETA is null, '''',format(o.SewETA, ''yyyy-MM-dd'')) , '', Pkg. MTL ETA (SP): '' 
			,IIF(o.PackETA is null, '''',format( o.PackETA, ''yyyy-MM-dd'')) ,'','',char(13),''R/MTL ETA(Master SP): '' 
			,IIF(o.MTLETA is null, '''',format(o.MTLETA, ''yyyy-MM-dd'')) , ''('' ,isnull(mtlExport.mtlOk,'''') ,''), MTL Cmplt (SP): '' 
			,IIF(o.MTLComplete = 1,''Y'',''''),char(13), ''VAS/SHAS: '' 
			,IIF(o.VasShas = 1,''Y'',''N''), '', MTL continuous delay: '' 
			,IIF([dbo].GetHaveDelaySupp(o.POID) = 1, ''Y'', ''N'')
		)
	)
)REMK
outer apply(
	select PRGM = (
		Select IIF(d.Qty = '''',d.TMS, d.Qty + IIF(d.TMS = '''', '''', '',''+d.TMS)) 
		from (
			Select 
				IIF(ot.Qty <> 0,a.Abbreviation+'':''+CONVERT(varchar,ot.Qty),'''') as Qty, 
				IIF(ot.TMS <> 0 and a.Classify = ''O'' ,a.Abbreviation+'':''+CONVERT(varchar,ot.TMS),'''') as TMS 
			from Order_TmsCost ot, ArtworkType a 
			where ot.ID  = o.ID 
			and a.ID = ot.ArtworkTypeID 
			and (a.Classify = ''S'' or a.IsSubprocess = 1) 
			and not (ot.Price = 0 and a.Classify <> ''O'')		
		) d
		Where d.Qty <> '''' and d.TMS <> ''''
		for xml path('''')
	)
) PRGM
outer apply(select top 1 MasterStyleID from Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey = o.StyleUkey)MasterStyleID1
outer apply(select top 1 MasterStyleID from Style_SimilarStyle WITH (NOLOCK) where ChildrenStyleUkey = o.StyleUkey)MasterStyleID2
Where 
(o.SCIDelivery >= DATEADD(DAY, -15, CONVERT(date,GETDATE())) or o.EditDate >= DATEADD(DAY, -7, CONVERT(date,GETDATE())))
and (o.Category = ''B'' or o.Category = ''S'')

IF OBJECT_ID(''tempdb.dbo.#tmp2'', ''U'') IS NOT NULL DROP TABLE #tmp2

select DISTINCT s.*, t.RCID as C
into #tmp2
from #tmp s
left join '+@SerDbDboTb+N' t on t.RCID collate Chinese_Taiwan_Stroke_CI_AS = s.sRCID


insert into '+@SerDbDboTb+N'
([RCID],[DELF],[SONO],[LOT],[CRNM],[PRIO],[ODST],[NCTR],[CSSE],[CSNM],[CUNM],[CFTY],[SYD1]
,[GTMH],[OTDD],[COTD],[OTTD],[QTYN],[FIRM],[COLR],[SZE],[SHIP],[SMOD],[PlcOrdDate],[REMK],[AOTT]
,[UPUS],[UPNM],[SYCO],[MASTERMATERIALDATE],[MASTERMATERIALRECEIVEDDATE],[MATERIALDATE],[MATERIALRECEIVEDDATE]
,[PPRO],[PRGM],UPDT
,[CUSY],[CUSTOMERORDERNO])
select [sRCID], iif(Junk = 0,''N'',''Y'') ,[sSONO],[sLOT],[sCRNM],[sPRIO],[sODST],[sNCTR],[sCSSE],[sCSNM],[sCUNM],[sCFTY],[sSYD1]
,[sGTMH],[sOTDD],[sCOTD],[sOTTD],[sQTYN],[sFIRM],[sCOLR],[sSZE],[sSHIP],[sSMOD],[sPlcOrdDate],[sREMK],[sAOTT]
,[sUPUS],[sUPNM],[sSYCO],[sMASTERMATERIALDATE],[sMASTERMATERIALRECEIVEDDATE],[sMATERIALDATE],[sMATERIALRECEIVEDDATE]
,[sPPRO],[sPRGM],UPDT = format(GETDATE(),''yyyy-MM-dd'')
,[sCUSY],[sCUSTOMERORDERNO]
from #tmp2
where C is null--目標沒有

select * from #tmp2 where C is null
'
Declare @cmd3 varchar(max)
set @cmd =N'select * from #tmp2 where C is null'
	END
	EXEC(@Cmd2)
	EXEC(@Cmd3)
END