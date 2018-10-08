-- =============================================
-- Author:		<SPIN>
-- Create date: <Create Date,,>
-- Description:	<列出Thread Transaction>
-- =============================================
CREATE PROCEDURE [dbo].[usp_ThreadTransactionList]
	-- Add the parameters for the stored procedure here
	(
	 @Refno  varChar(21),
	 @MDivisionid  varChar(8),
	 @Date1 varChar(10),
	 @Date2 varChar(10)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

      --Insert statements for procedure here
	Create table #tmp(cdate date, id varchar(13),name varchar(50),ThreadColorid varchar(15),ThreadLocationid varchar(10),Newin numeric(5),Newout numeric(5),Usedin numeric(5),Usedout numeric(5),
		Newbalance numeric(5),Usedbalance numeric(5) ,editname varchar(50),editdate datetime)

	insert into #tmp
	Select a.cdate,a.ID,'P03.Thread In-Coming From Factory ' as name,b.ThreadColorid,b.ThreadLocationid,
	 isnull(b.NewCone,0) as Newin,0 as Newout ,isnull(b.UsedCone,0) as UsedIn, 0 as Usedout,0 as newBalance,0 as usedBalance,
	a.editname+'-'+c.name +' '+convert(varchar, a.editdate,120) as editname ,a.editdate
		From ThreadIncoming a WITH (NOLOCK) left join Pass1 c WITH (NOLOCK) on c.id=a.editname , ThreadIncoming_Detail b  
			Where b.refno =@Refno and a.id = b.id  and status = 'Confirmed' and cdate > = @Date1 and cdate < = @Date2 
			
		
	insert into #tmp
	Select d.cdate,d.ID,'P04.Thread Issue' as name,e.Threadcolorid,e.ThreadLocationid,
	 0 as Newin,isnull(e.NewCone,0) as Newout , 0 as Usedin,isnull(e.UsedCone,0) as Usedout,0 as newBalance,0 as usedBalance,
	d.editname+'-'+f.name +' '+convert(varchar,d.editdate,120) as editname ,d.editdate
		From ThreadIssue d WITH (NOLOCK) left join Pass1 f WITH (NOLOCK) on f.id=d.editname , ThreadIssue_Detail e  
			Where e.Refno =@Refno and d.id = e.id  and status = 'Confirmed' and cdate > = @Date1 and cdate < = @Date2 
			
	
	insert into #tmp
	Select a.cdate ,a.ID,'P06.Thread Stock Adjust' as name,b.ThreadColorid,b.ThreadLocationid,
	iif((isnull(NewCone,0)-isnull(NewConeBook,0)) > 0,(isnull(NewCone,0)-isnull(NewConeBook,0)),0) as Newin,
	iif((isnull(NewCone,0)-isnull(NewConeBook,0)) < 0,-(isnull(NewCone,0)-isnull(NewConeBook,0)),0) as NewOut,
	iif((isnull(UsedCone,0)-isnull(UsedConeBook,0)) > 0,(isnull(UsedCone,0)-isnull(UsedConeBook,0)),0) as UsedIn,
	iif((isnull(UsedCone,0)-isnull(UsedConeBook,0)) < 0,-(isnull(UsedCone,0)-isnull(UsedConeBook,0)),0) as UsedOut,
	0 as newBalance,0 as usedBalance,
	a.editname+'-'+c.name +' '+convert(varchar, a.editdate,120) as editname ,a.editdate
		From ThreadAdjust a WITH (NOLOCK) left join Pass1 c WITH (NOLOCK) on c.id=a.editname , ThreadAdjust_Detail b  
			Where b.Refno =@Refno and a.id = b.id  and status = 'Confirmed' and cdate > = @Date1 and cdate < = @Date2 
			and ((isnull(newCone,0)-isnull(NewConeBook,0))!=0 or (isnull(UsedCone,0)-isnull(UsedConeBook,0))!=0 )
	
	insert into #tmp
	Select a.cdate,a.ID,'P07.Part Location Transfer(In)' as name, b.ThreadColorid,b.Locationto as ThreadLocationid,
	NewCone as NewIn,0 as NewOut, UsedCone as Usedin,0 as UsedOut,0 as newBalance,0 as usedBalance,
	a.editname+'-'+c.name +' '+convert(varchar,a.editdate,120) as editname,a.EditDate
		From ThreadTransfer a WITH (NOLOCK) left join Pass1 c WITH (NOLOCK) on c.id=a.editname , ThreadTransfer_Detail b  
			Where b.Refno =@Refno and a.id = b.id  and status = 'Confirmed' and cdate > = @Date1 and cdate < = @Date2
			

	insert into #tmp
	Select a.cdate,a.ID,'P07.Part Location Transfer(Out)' as name,b.ThreadColorid,b.Locationfrom as ThreadLocationid,
	0 as Newin, NewCone as Newout,0 as Usedin, UsedCone as Usedout ,0 as newBalance,0 as usedBalance,
	a.editname+'-'+c.name +' '+convert(varchar,a.editdate,120) as editname,a.EditDate
		From ThreadTransfer a WITH (NOLOCK) left join Pass1 c WITH (NOLOCK) on c.id=a.editname , ThreadTransfer_Detail b  
			Where b.Refno =@Refno and a.id = b.id  and status = 'Confirmed' and cdate > = @Date1 and cdate < = @Date2
			

	Select * from #tmp order by cdate,editdate,ThreadColorid,ThreadLocationid,name,id

END