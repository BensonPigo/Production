
Create PROCEDURE [dbo].[P_ImportSubProInsReport]
AS
BEGIN
declare @StartDate Date = Format(getdate(), 'yyyy') + '0101'
declare @EndDate Date = Format(getdate(), 'yyyy') + '1231'

SELECT * into #tmp FROM OPENQUERY([MainServer], '
declare @StartDate Date = Format(getdate(), ''yyyy'') + ''0101''
declare @EndDate Date = Format(getdate(), ''yyyy'') + ''1231''

select
    SR.FactoryID,
    SR.SubProLocationID,
	SR.InspectionDate,
    O.SewInLine,
	B.Sewinglineid,
    SR.Shift,
	[RFT] = isnull(Convert(float(50),Convert(FLOAT(50), round(((A.InspectQty-A.RejectQty)/ nullif(A.InspectQty, 0))*100,2))),0),
	SR.SubProcessID,
	SR.BundleNo,
    [Artwork] = Artwork.val,
	B.OrderID,
    Country.Alias,
    o.BuyerDelivery,
	BD.BundleGroup,
    o.SeasonID,
	O.styleID,
	B.Colorid,
	BD.SizeCode,
    BD.PatternDesc,
    B.Item,
	BD.Qty,
	SR.RejectQty,
	SR.Machine,
	m.Serial,
	m.Junk,
	m.Description,
	SRD.DefectCode,                                
	SRD.DefectQty,
	Inspector = (SELECT CONCAT(a.ID, '':'', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = SR.AddName),
	SR.Remark,
    SR.AddDate,
    SR.RepairedDatetime,
	RepairedTime = iif(RepairedDatetime is null, null, ttlSecond),
    
	ResolveTime = iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	SubProResponseTeamID
    ,CustomColumn1
	,Fac.MDivisionID
from Production.dbo.SubProInsRecord SR WITH (NOLOCK)
Left join Production.dbo.Bundle_Detail BD WITH (NOLOCK) on SR.BundleNo=BD.BundleNo
Left join Production.dbo.Bundle B WITH (NOLOCK) on BD.ID=B.ID
Left join Production.dbo.Orders O WITH (NOLOCK) on B.OrderID=O.ID
left join Production.dbo.Country on Country.ID = o.Dest
outer apply(SELECT val =  Stuff((select distinct concat( ''+'',SubprocessId)   
                                    from Production.dbo.Bundle_Detail_Art bda with (nolock) 
                                    where bda.Bundleno = BD.Bundleno
                                    FOR XML PATH('''')),1,1,'''') ) Artwork
left join Production.dbo.SubProMachine m on SR.Machine = m.ID and SR.FactoryID = m.FactoryID and SR.SubProcessID = m.SubProcessID
left join Production.dbo.SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey
outer apply(select ttlSecond_RD = sum(DATEDIFF(Second, StartResolveDate, EndResolveDate)) from Production.dbo.SubProInsRecord_ResponseTeam where EndResolveDate is not null and SubProInsRecordUkey = SR.Ukey)ttlSecond_RD
outer apply(
	select SubProResponseTeamID = STUFF((
		select CONCAT('','', SubProResponseTeamID)
		from Production.dbo.SubProInsRecord_ResponseTeam
		where SubProInsRecordUkey = SR.Ukey
		order by SubProResponseTeamID
		for xml path('''')
	),1,1,'''')
)SubProResponseTeamID
outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
outer apply(select MDivisionID from Production.dbo.Factory f where f.ID = SR.FactoryID ) Fac
left join Production.dbo.RFT A with (nolock) on A.OrderID=B.OrderId
								 and A.SewinglineID=B.SewinglineID
								 and A.FactoryID=SR.FactoryID
								 and A.Shift=SR.Shift
Where SR.InspectionDate between @StartDate and @EndDate
UNION
select
    SR.FactoryID,
    SR.SubProLocationID,
	SR.InspectionDate,
    O.SewInLine,
    BR.Sewinglineid,
    SR.Shift,
	[RFT] = isnull(Convert(float(50),Convert(FLOAT(50), round(((A.InspectQty-A.RejectQty)/ nullif(A.InspectQty, 0))*100,2))),0),
	SR.SubProcessID,
	SR.BundleNo,
    [Artwork] = Artwork.val,
	BR.OrderID,
    Country.Alias,
    o.BuyerDelivery,
	BRD.BundleGroup,
    o.SeasonID,
	O.styleID,
	BR.Colorid,
	BRD.SizeCode,
    BRD.PatternDesc,
    BR.Item,
	BRD.Qty,
	SR.RejectQty,
	SR.Machine,
	m.Serial,
	m.Junk,
	m.Description,
	SRD.DefectCode,                                
	SRD.DefectQty,
	Inspector = (SELECT CONCAT(a.ID, '':'', a.Name) from [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) where a.ID = SR.AddName),
	SR.Remark,
    SR.AddDate,
    SR.RepairedDatetime,
	iif(RepairedDatetime is null, null, ttlSecond),
	iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	SubProResponseTeamID
    ,CustomColumn1--自定義欄位, 在最後一個若有變動,則輸出Excel部分也要一起改
	,Fac.MDivisionID
from Production.dbo.SubProInsRecord SR WITH (NOLOCK)
Left join Production.dbo.BundleReplacement_Detail BRD WITH (NOLOCK) on SR.BundleNo=BRD.BundleNo
Left join Production.dbo.BundleReplacement BR WITH (NOLOCK) on BRD.ID=BR.ID
Left join Production.dbo.Orders O WITH (NOLOCK) on BR.OrderID=O.ID
left join Production.dbo.Country on Country.ID = o.Dest
outer apply(SELECT val =  Stuff((select distinct concat( ''+'',SubprocessId)   
                                    from Production.dbo.Bundle_Detail_Art bda with (nolock) 
                                    where bda.Bundleno = SR.BundleNo
                                    FOR XML PATH('''')),1,1,'''') ) Artwork
left join Production.dbo.SubProMachine m on SR.Machine = m.ID and SR.FactoryID = m.FactoryID and SR.SubProcessID = m.SubProcessID
left join Production.dbo.SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey
outer apply(select ttlSecond_RD = sum(DATEDIFF(Second, StartResolveDate, EndResolveDate)) from Production.dbo.SubProInsRecord_ResponseTeam where EndResolveDate is not null and SubProInsRecordUkey = SR.Ukey)ttlSecond_RD
outer apply(
	select SubProResponseTeamID = STUFF((
		select CONCAT('','', SubProResponseTeamID)
		from Production.dbo.SubProInsRecord_ResponseTeam
		where SubProInsRecordUkey = SR.Ukey
		order by SubProResponseTeamID
		for xml path('''')
	),1,1,'''')
)SubProResponseTeamID
outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
outer apply(select MDivisionID from Production.dbo.Factory f where f.ID = SR.FactoryID ) Fac
left join Production.dbo.RFT A with (nolock) on A.OrderID=BR.OrderId
								 and A.SewinglineID=BR.SewinglineID
								 and A.FactoryID=SR.FactoryID
								 and A.Shift=SR.Shift
Where SR.InspectionDate between @StartDate and @EndDate
')
 
select *
into #tmp2
from (
	select *, BundleNoCT = COUNT(1) over(partition by t.BundleNo)
	from #tmp t
) t
where BundleNoCT = 1--綁包/補料都沒有,在第一段union會合併成一筆
or (BundleNoCT > 1 and isnull(t.Orderid, '') <> '')--綁包/補料其中一個有

delete P_SubProInsReport where InspectionDate between @StartDate and @EndDate

insert into P_SubProInsReport(
FactoryID						   
,SubProLocationID				   
,InspectionDate						
,SewInLine						   
,SewinglineID						
,Shift							   
,RFT								
,SubProcessID						
,BundleNo							
,Artwork						   
,OrderID							
,Alias							   
,BuyerDelivery					   
,BundleGroup						
,SeasonID						   
,StyleID							
,ColorID							
,SizeCode							
,PatternDesc					   
,Item							   
,Qty								
,RejectQty							
,Machine							
,Serial								
,Junk								
,Description						
,DefectCode							
,DefectQty							
,Inspector							
,Remark								
,AddDate						   
,RepairedDatetime				   
,RepairedTime						
,ResolveTime						
,SubProResponseTeamID				
,CustomColumn1	
,MDivisionID
)
select	isnull(FactoryID, '')					
		,isnull(SubProLocationID, '')		
		,InspectionDate					
		,SewInLine					   
		,isnull(SewinglineID, '')			
		,isnull(Shift, '')					
		,isnull(RFT, 0)						
		,isnull(SubProcessID, '')			
		,isnull(BundleNo, '')				
		,isnull(Artwork, '')				
		,isnull(OrderID, '')				
		,isnull(Alias, '')					
		,BuyerDelivery					   
		,isnull(BundleGroup, 0)				
		,isnull(SeasonID, '')				
		,isnull(StyleID, '')				
		,isnull(ColorID, '')				
		,isnull(SizeCode, '')				
		,isnull(PatternDesc, '')			  
		,isnull(Item, '')					
		,isnull(Qty, 0)						
		,isnull(RejectQty, 0)				
		,isnull(Machine, '')				
		,isnull(Serial, '')					
		,isnull(Junk, 0)					
		,isnull(Description, '')				
		,isnull(DefectCode, '')				
		,isnull(DefectQty, 0)				
		,isnull(Inspector, '')				
		,isnull(Remark, '')					
		,AddDate 					
		,RepairedDatetime			
		,isnull(RepairedTime, 0)			
		,isnull(ResolveTime, 0)				
		,isnull(SubProResponseTeamID, '')	
		,isnull(CustomColumn1, '')	
		,isnull(MDivisionID, '')
from #tmp2

declare @Cnt bigint = (select count(1) from #tmp2)
declare @powebiCnt bigint = (select count(1) from P_SubProInsReport where InspectionDate between @StartDate and @EndDate);

print @cnt
print @powebiCnt
if ((@Cnt > 0 and @powebiCnt > 0) or (@Cnt = @powebiCnt))
begin
	update b
		set b.TransferDate = getdate()
			, b.IS_Trans = 1
	from BITableInfo b
	where b.Id = 'P_SubProInsReport'
end

end
GO








