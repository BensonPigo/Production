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
	   , Mc.LastTransferDate
	   , LendTo = (Mc.LendTo+''''-''''+LS.Abb)
	   , Mc.LendDate
       , Mc.LastestReturnDate
       , Mc.Remark 
       , Mc.FAID
	   , Junk = iif(Mc.junk = 1, ''''Y'''', ''''N'''')
from Machine.dbo.Machine Mc
left join Machine.dbo.SciProduction_LocalSupp LS on LS.ID = Mc.LendTo
left join Machine.dbo.MachineGroup on mc.MachineGroupID = MachineGroup.ID AND mc.MasterGroupID=MachineGroup.MasterGroupID
left join Machine.dbo.MachineMasterGroup mmg on mmg.id = MachineGroup.masterGroupID
left join Machine.dbo.MachineRepair r on Mc.id =r.MachineID and r.Status = ''''Repairing''''
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
where exists(select 1 from #tmp t where p.Month = t.YYYYMM and p.MachineID = t.MachineID)

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
								,TransferDate
								,LendTo
								,LendDate
								,LastEstReturnDate
								,Remark
								,FAID
								,Junk
)
select *
from #tmp

drop table #tmp
'
exec (@finalInsertSql)


end