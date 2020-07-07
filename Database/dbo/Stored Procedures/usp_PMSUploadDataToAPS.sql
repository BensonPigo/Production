
CREATE PROCEDURE [dbo].[usp_PMSUploadDataToAPS]

AS
BEGIN
Set ARITHABORT ON
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

	BEGIN--4部分字串
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
,o.NeedProduction
,o.IsBuyBack
,o.BuyBackReason
,[sDELF] = case when o.Junk = 1 and o.NeedProduction = 0 then ''Y''
				  when o.Junk = 0 and o.IsBuyBack = 1 and o.BuyBackReason = ''Garment'' then ''Y''
				  else ''N''
				  end
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
	SELECT [PRGM]=FabricType
	FROM Style s
	WHERE s.Ukey = o.StyleUkey 
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

IF OBJECT_ID(''tempdb.dbo.#tmp'', ''U'') IS NOT NULL DROP TABLE #tmp

update t set
	 [DELF] = s.[sDELF]
	,[SONO] = s.[sSONO]
	,[LOT] = s.[sLOT]
	,[CRNM] = s.[sCRNM]
	,[PRIO] = s.[sPRIO]
	,[ODST] = s.[sODST]
	,[NCTR] = s.[sNCTR]
	,[CSSE] = s.[sCSSE]
	,[CSNM] = s.[sCSNM]
	,[CUNM] = s.[sCUNM]
	,[CFTY] = s.[sCFTY]
	,[SYD1] = s.[sSYD1]
	,[GTMH] = s.[sGTMH]
	,[OTDD] = s.[sOTDD]
	,[COTD] = s.[sCOTD]
	,[OTTD] = s.[sOTTD]
	,[QTYN] = s.[sQTYN]
	,[FIRM] = s.[sFIRM]
	,[COLR] = s.[sCOLR]
	,[SZE] = s.[sSZE]
	,[SHIP] = s.[sSHIP]
	,[SMOD] = s.[sSMOD]
	,[PlcOrdDate] = s.[sPlcOrdDate]
	,[REMK] = s.[sREMK]
	,[AOTT] = s.[sAOTT]
	,[UPUS] = s.[sUPUS]
	,[UPNM] = s.[sUPNM]
	,[SYCO] = s.[sSYCO]
	,[MASTERMATERIALDATE] = s.[sMASTERMATERIALDATE]
	,[MASTERMATERIALRECEIVEDDATE] = s.[sMASTERMATERIALRECEIVEDDATE]
	,[MATERIALDATE] = s.[sMATERIALDATE]
	,[MATERIALRECEIVEDDATE] = s.[sMATERIALRECEIVEDDATE]
	,[PPRO] = s.[sPPRO]
	,[PRGM] = s.[sPRGM]
	,UPDT= format(GETDATE(),''yyyy-MM-dd'')
	,[CUSY] =[sCUSY]
	,[CUSTOMERORDERNO] = [sCUSTOMERORDERNO]
from #tmp2 s,'+@SerDbDboTb+N't
where C is not null and PulloutComplete = 0 and Finished = 0
and t.RCID collate Chinese_Taiwan_Stroke_CI_AS = s.sRCID
and (
	isnull(t.[SONO],'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(s.sSONO,'''')
	or isnull(QTYN,0) != isnull(sQTYN,0)
	or isnull(COTD,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sCOTD,'''')
	or isnull(OTDD,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sOTDD,'''')
	or isnull(AOTT,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sAOTT,'''')
	or isnull(MASTERMATERIALRECEIVEDDATE,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sMASTERMATERIALRECEIVEDDATE,'''')
	or isnull(MASTERMATERIALDATE,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sMASTERMATERIALDATE,'''')
	or isnull(MATERIALDATE,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sMATERIALDATE,'''')
	or isnull(GTMH,0) != isnull(sGTMH,0)
	or isnull(PPRO,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sPPRO,'''')
	or isnull(ODST,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sODST,'''')
	or isnull(SMOD,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sSMOD,'''')
	or isnull(SHIP,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sSHIP,'''')
	or isnull(PRGM,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sPRGM,'''')
	or isnull(REMK,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sREMK,'''')	
	or isnull(DELF,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sDELF,'''')	
	or isnull(CUSY,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sCUSY,'''')
	or isnull(CUSTOMERORDERNO,'''') collate Chinese_Taiwan_Stroke_CI_AS != isnull(sCUSTOMERORDERNO,'''')
)

insert into '+@SerDbDboTb+N'
([RCID],[DELF],[SONO],[LOT],[CRNM],[PRIO],[ODST],[NCTR],[CSSE],[CSNM],[CUNM],[CFTY],[SYD1]
,[GTMH],[OTDD],[COTD],[OTTD],[QTYN],[FIRM],[COLR],[SZE],[SHIP],[SMOD],[PlcOrdDate],[REMK],[AOTT]
,[UPUS],[UPNM],[SYCO],[MASTERMATERIALDATE],[MASTERMATERIALRECEIVEDDATE],[MATERIALDATE],[MATERIALRECEIVEDDATE]
,[PPRO],[PRGM],UPDT
,[CUSY],[CUSTOMERORDERNO])
select [sRCID], [sDELF] ,[sSONO],[sLOT],[sCRNM],[sPRIO],[sODST],[sNCTR],[sCSSE],[sCSNM],[sCUNM],[sCFTY],[sSYD1]
,[sGTMH],[sOTDD],[sCOTD],[sOTTD],[sQTYN],[sFIRM],[sCOLR],[sSZE],[sSHIP],[sSMOD],[sPlcOrdDate],[sREMK],[sAOTT]
,[sUPUS],[sUPNM],[sSYCO],[sMASTERMATERIALDATE],[sMASTERMATERIALRECEIVEDDATE],[sMATERIALDATE],[sMATERIALRECEIVEDDATE]
,[sPPRO],[sPRGM],UPDT = format(GETDATE(),''yyyy-MM-dd'')
,[sCUSY],[sCUSTOMERORDERNO]
from #tmp2
where C is null--目標沒有

Update t
set DELF=''Y'',
	UPDT= format(GETDATE(),''yyyy-MM-dd'')
FROM '+@SerDbDboTb+N' t
where 
not exists(select 1 from #tmp2 s where t.RCID collate Chinese_Taiwan_Stroke_CI_AS =s.sRCID)
and CONVERT(date, t.OTDD collate Chinese_Taiwan_Stroke_CI_AS) >= DATEADD(DAY, -15, GETDATE())
and DELF <> ''Y''

IF OBJECT_ID(''tempdb.dbo.#tmp'', ''U'') IS NOT NULL DROP TABLE #tmp2
'

-------------------------------------------------第三部分-------------------------------------------------
--Style圖檔資料：APS的中間表Table Name為IMAGEMAPPING
--key要注意production這有3個key,目標table 只有兩個key,多筆取top 1
Declare @SerDbDboTb2 varchar(66)
Set @SerDbDboTb2 = concat('[',@ServerName,'].[',@DatabaseName,N'].[dbo].[IMAGEMAPPING]')
Declare @cmd3 varchar(max)
set @cmd3 =N'
update '+@SerDbDboTb2+N'
set
[FULLPATH] = [sFULLPATH]
From (
	Select Distinct [sSTYLENO] = ID
	,[sSEASONCD] = SeasonID
	,[sFULLPATH] = concat((select PicPath from System),Picture1)
	from Style s
	inner join '+@SerDbDboTb2+N' t 
	on t.STYLENO collate Chinese_Taiwan_Stroke_CI_AS = s.ID
	and t.SEASONCD collate Chinese_Taiwan_Stroke_CI_AS = s.SeasonID
)s
where STYLENO collate Chinese_Taiwan_Stroke_CI_AS = s.sSTYLENO 
and SEASONCD collate Chinese_Taiwan_Stroke_CI_AS = s.sSEASONCD
and FULLPATH collate Chinese_Taiwan_Stroke_CI_AS != s.sFULLPATH

IF OBJECT_ID(''tempdb.dbo.#tmps'', ''U'') IS NOT NULL DROP TABLE #tmps
Select Distinct  ID
,SeasonID
,BrandID
into #tmps
from Style s
left join '+@SerDbDboTb2+N' t 
on t.STYLENO collate Chinese_Taiwan_Stroke_CI_AS = s.ID 
and t.SEASONCD collate Chinese_Taiwan_Stroke_CI_AS = s.SeasonID
where t.STYLENO is null
insert into '+@SerDbDboTb2+N'
([STYLENO],[SEASONCD],[FULLPATH])
select distinct s.ID,s.SeasonID
,[FULLPATH] = 
(
	select top 1 concat((select PicPath from System),s2.Picture1) 
	from Style s2 where s2.id = s.id  and s2.SeasonID = s.SeasonID
)
from #tmps s
IF OBJECT_ID(''tempdb.dbo.#tmps'', ''U'') IS NOT NULL DROP TABLE #tmps'


-------------------------------------------------第四部分-------------------------------------------------
--Sewing Daily Output：APS的中間表Table Name為OPDWF220
Declare @SerDbDboTb3 varchar(66)
Set @SerDbDboTb3 = concat('[',@ServerName,'].[',@DatabaseName,N'].[dbo].[OPDWF220]')
Declare @cmd4 varchar(max)
set @cmd4 =N'
Delete from '+@SerDbDboTb3+N'
INSERT INTO '+@SerDbDboTb3+N'
([POCODE],[PROCESS],[FACILITY],[PDATE],[COLOR],[XSIZE],[QTY],[WORKERS],[HOURS])
select 
	[POCode],[Process],[Facility],[PDate],[Color],[XSize]
	,[Qty] = sum([Qty])
	,[Workers]=AVG([Workers])
	,[Hours]=SUM([Hours])
from
(
	Select
	[POCode] = s.FactoryID+sd.OrderID+IIF((select StyleUnit from Orders where ID = sd.OrderID) = ''PCS'', '''', ''(''+sd.ComboType+'')'')
	,[Process] = ''SEWING''
	,[Facility] = s.SewingLineID
	,[PDate] = CONVERT(varchar(10), s.OutputDate, 120)
	,[Color] = sd.Article
	,[XSize] = sdd.SizeCode
	,[Qty] = sdd.QAQty
	,[Workers] = s.Manpower
	,[Hours] = round( (cast(sdd.QAQty as float) / cast(sd.QAQty as float)) * sd.WorkHour , 3)
	from SewingOutput s
	inner join SewingOutput_Detail sd on s.ID = sd.ID
	inner join SewingOutput_Detail_Detail sdd on sdd.SewingOutput_DetailUKey = sd.UKey 
	where sdd.OrderID in (Select distinct sd.OrderID 
						  from SewingOutput s, SewingOutput_Detail sd
						  where (s.LockDate is null or s.LockDate >= DATEADD(DAY, -7, CONVERT(date,GETDATE())))
						  and s.ID = sd.ID)
		  and sd.QAQty > 0
)l
group by [POCode],[Process],[Facility],[PDate],[Color],[XSize]
'
	END
	EXEC(@Cmd)
	EXEC(@Cmd2)
	EXEC(@Cmd3)
	EXEC(@Cmd4)

	--Begin Try
	--	EXEC(@Cmd)		
	--End Try
	--Begin Catch
	--	PRINT 'ERROR command1' 
	--	rollback tran
	--	EXECUTE [usp_GetErrorString];
	--End Catch
	--Begin Try
	--	EXEC(@Cmd2)
	--End Try
	--Begin Catch
	--	PRINT 'ERROR command2' 
	--	EXECUTE [usp_GetErrorString];
	--End Catch
	--Begin Try
	--	EXEC(@Cmd3)
	--End Try
	--Begin Catch
	--	PRINT 'ERROR command3' 
	--	EXECUTE [usp_GetErrorString];
	--End Catch
	--Begin Try
	--	EXEC(@Cmd4)
	--End Try
	--Begin Catch
	--	PRINT 'ERROR command4'
	--	EXECUTE [usp_GetErrorString];
	--End Catch
END