CREATE PROCEDURE [dbo].[P_Import_MachineMaserList]
	@LinkServerName varchar(50),
	@YYYYMM varchar(6) = ''
AS
begin

declare @CreateYYYYMM varchar(6)

if(@YYYYMM = '' or TRY_CONVERT(date, @YYYYMM + '01') is null )
begin
	set @CreateYYYYMM = FORMAT(GETDATE(), 'yyyyMM')
end
else
begin
	set @CreateYYYYMM = @YYYYMM
end

declare @openQuerySql varchar(4000) = '
select [YYYYMM] = ''''' + @CreateYYYYMM + ''''' 
	   , Mc.LocationM
	   , FactoryID = (select L.FactoryID 
	   			  from Machine.dbo.MachineLocation L 
	   			  where Mc.MachineLocationID = L.ID 
	   			  		and Mc.LocationM = L.MDivisionID)
	   , Mc.MachineLocationID
	   , [MachineID] = Mc.ID
	   , MachineGroup = concat( mmg.ID ,concat(MachineGroup.ID , ''''-'''', MachineGroup.Description))
	   , Mc.MachineBrandID
       , Name = (select Name from Machine.dbo.MachineBrand where id = Mc.MachineBrandID)
	   , Mc.Model
	   , Mc.Serialno
	   , Mc.Status
       , iif(Mc.Status=''''Pending'''',mp.CyApvDate ,null) [Pending-Country Manger Apv Date]
	   , r.CDate as [Repair Start Date]
	   , mc.EstFinishRepairDate
	   , Mc.ArriveDate
	   , [ObtainedDate] = A.CreateDate
	   , Mc.LastTransferDate
	   , LendTo = (Mc.LendTo+''''-''''+LS.Abb)
	   , Mc.LendDate
       , Mc.LastestReturnDate
       , Mc.Remark 
       , Mc.FAID
	   , Junk = iif(Mc.junk = 1, ''''Y'''', ''''N'''')
	   , [POID] = midi.MachinePOID
	   , mpd.RefNo
from Machine.dbo.Machine Mc with(nolock)
left join Machine.dbo.SciProduction_LocalSupp LS with(nolock) on LS.ID = Mc.LendTo
left join Machine.dbo.MachineGroup with(nolock) on mc.MachineGroupID = MachineGroup.ID AND mc.MasterGroupID=MachineGroup.MasterGroupID
left join Machine.dbo.MachineMasterGroup mmg with(nolock) on mmg.id = MachineGroup.masterGroupID
left join Machine.dbo.MachineRepair r with(nolock) on Mc.id =r.MachineID and r.Status = ''''Repairing''''
left join Machine.dbo.MachineIn_Detail_Inspect midi with(nolock) on mc.ID = midi.MachineID and midi.Result = ''''P''''
left join Machine.dbo.MachinePO_Detail mpd with(nolock) on mpd.ID = midi.MachinePOID and mpd.Seq1 = midi.SEQ1 and mpd.Seq2 = midi.SEQ2
left join FixedAssets.dbo.Asset A on iif(LEN(Mc.FAID) > 0,LEFT(Mc.FAID,LEN(Mc.FAID)-4),'''''''') = A.ID
outer apply(
    select top 1 mp.CyApvDate 
    from Machine.dbo.MachinePending mp with(nolock)
    inner join Machine.dbo.MachinePending_Detail mpd with(nolock) on mp.id = mpd.id
    where mpd.MachineID =  mc.ID  and mp.Status = ''''Confirmed'''' and mp.CyApvDate is not null
)mp
'

declare @finalInsertSql varchar(4000) = '
select *
into #tmp
from OPENQUERY ([' + @LinkServerName + '], ''' + @openQuerySql + ''')

delete p
from P_MachineMasterList p
where exists (select 1 from #tmp t where p.Month = t.YYYYMM and p.M = t.LocationM)
--and not exists (select 1 from #tmp t where p.Month = t.YYYYMM and p.M = t.LocationM and p.MachineID = t.MachineID)

insert into P_MachineMasterList(Month
								,M
								,FTY
								,MachineLocationID
								,MachineID
								,MachineGroup
								,BrandID
								,BrandName
								,Model
								,SerialNo
								,Condition
								,PendingCountryMangerApvDate
								,RepairStartDate
								,EstFinishRepairDate
								,MachineArrivalDate
								,ObtainedDate
								,TransferDate
								,LendTo
								,LendDate
								,LastEstReturnDate
								,Remark
								,FAID
								,Junk
								,POID
								,RefNo
)
select *
from #tmp t
--where not exists (select 1 from P_MachineMasterList p where p.Month = t.YYYYMM and p.M = t.LocationM and p.MachineID = t.MachineID)

drop table #tmp

update b
	set b.TransferDate = getdate()
from BITableInfo b
where b.Id = ''P_MachineMasterList''
'
exec (@finalInsertSql)



end
