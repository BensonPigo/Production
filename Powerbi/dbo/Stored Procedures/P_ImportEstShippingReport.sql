CREATE PROCEDURE [dbo].[P_ImportEstShippingReport]	
	@StartDate Date,
	@EndDate Date,
	@LinkServerName varchar(50)
AS

BEGIN

declare @SqlCmd_Combin nvarchar(max) =''
declare @SqlCmd1 nvarchar(max) ='';
declare @SqlCmd2 nvarchar(max) ='';

declare @SDate varchar(20) = format(@StartDate, 'yyyyMMdd')
declare @EDate varchar(20) = format(@EndDate, 'yyyyMMdd')


SET @SqlCmd1 = '
SELECT * into #tmp FROM OPENQUERY(['+@LinkServerName+'], 
''exec Production.dbo.ImportEstShippingReport @StartDate = '''''+@SDate+''''', @EndDate= '''''+@EDate+''''' '')
'

SET @SqlCmd2 = '

BEGIN TRY
Begin tran

update t
set 
	t.BuyerDelivery =  s.BuyerDelivery,
	t.Brand =  s.Brand,
	t.SPNO =  s.SPNO,
	t.Category =  s.Category,
	t.Seq =  s.Seq,
	t.PackingNo =  s.PackingNo,
	t.PackingStatus =  s.PackingStatus,
	t.GBNo =  s.GBNo,
	t.PulloutDate =  s.PulloutDate,
	t.Order_TtlQty =  s.Order_TtlQty,
	t.SO_No =  s.SO_No,
	t.SO_CfmDate =  s.SO_CfmDate,
	t.CutOffDate =  s.CutOffDate,
	t.ShipPlanID =  s.ShipPlanID,
	t.M =  s.M,
	t.Factory =  s.Factory,
	t.Destination =  s.Destination,
	t.Price =  s.Price,
	t.GW =  s.GW,
	t.CBM =  s.CBM,
	t.ShipMode =  s.ShipMode,
	t.Handle =  s.Handle,
	t.LocalMR =  s.LocalMR,
	t.SP_SEQ =  s.SP_SEQ,
	t.If_Partial =  s.If_Partial,
	t.CartonQtyAtCLog =  s.CartonQtyAtCLog,
	t.SP_Prod_Output_Qty =  s.SP_Prod_Output_Qty,
	t.OrderType =  s.OrderType,
	t.BuyBack =  s.BuyBack,
	t.LoadingType =  s.LoadingType,
	t.Est_PODD =  s.Est_PODD,
	t.Origin =  s.Origin,
	t.Outstanding_Reason2 =  s.Outstanding_Reason2,
	t.Outstanding_Remark =  s.Outstanding_Remark,
	t.ReturnedQty_bySeq =  s.ReturnedQty_bySeq,
	t.HC# =  s.HC#,
	t.HCStatus =  s.HCStatus,
	t.ShipQty_by_Seq =  s.ShipQty_by_Seq,
	t.PackingQty_bySeq =  s.PackingQty_bySeq,
	t.CTNQty_bySeq =  s.CTNQty_bySeq,
	t.OrderQty_bySeq =  s.OrderQty_bySeq,
	t.FOC_Bal_Qty =  s.FOC_Bal_Qty,
	t.Pullout_ID =  s.Pullout_ID
from P_EstShippingReport t
inner join #tmp s on t.SPNO = s.SPNO
and t.Seq = s.Seq and t.Factory = s.Factory

insert into P_EstShippingReport
select [BuyerDelivery]
      ,[Brand]
      ,[SPNO]
      ,[Category]
      ,[Seq]
      ,[PackingNo]
      ,[PackingStatus]
      ,[GBNo]
      ,[PulloutDate]
      ,[Order_TtlQty]
      ,[SO_No]
      ,[SO_CfmDate]
      ,[CutOffDate]
      ,[ShipPlanID]
      ,[M]
      ,[Factory]
      ,[Destination]
      ,[Price]
      ,[GW]
      ,[CBM]
      ,[ShipMode]
      ,[Handle]
      ,[LocalMR]
      ,[SP_SEQ]
      ,[If_Partial]
      ,[CartonQtyAtCLog]
      ,[SP_Prod_Output_Qty]
      ,[OrderType]
      ,[BuyBack]
      ,[LoadingType]
      ,[Est_PODD]
      ,[Origin]
      ,[Outstanding_Reason2]
      ,[Outstanding_Remark]
      ,[ReturnedQty_bySeq]
      ,[HC#]
      ,[HCStatus]
      ,[ShipQty_by_Seq]
      ,[PackingQty_bySeq]
      ,[CTNQty_bySeq]
      ,[OrderQty_bySeq]
      ,[FOC_Bal_Qty]
      ,[Pullout_ID]
from #tmp s
where not exists(
	select 1 from P_EstShippingReport t
	where t.SPNO = s.SPNO
	and t.Seq = s.Seq
	and t.Factory = s.Factory
)

delete t
from P_EstShippingReport t
left join #tmp s on t.SPNO = s.SPNO
	and t.Seq = s.Seq and t.Factory = s.Factory
where t.BuyerDelivery between '''+@SDate+'''  and '''+@EDate+''' 
and t.Factory in (select distinct Factory from #tmp)
and s.SPNO is null

drop table #tmp

update b
	set b.TransferDate = getdate()
from BITableInfo b
where b.Id = ''P_EstShippingReport''

Commit tran

END TRY
BEGIN CATCH
	RollBack Tran
	declare @ErrMsg varchar(1000) = ''Err# : '' + ltrim(str(ERROR_NUMBER())) + 
				CHAR(10)+''Error Severity:''+ltrim(str(ERROR_SEVERITY()  )) +
				CHAR(10)+''Error State:'' + ltrim(str(ERROR_STATE() ))  +
				CHAR(10)+''Error Proc:'' + isNull(ERROR_PROCEDURE(),'''')  +
				CHAR(10)+''Error Line:''+ltrim(str(ERROR_LINE()  )) +
				CHAR(10)+''Error Msg:''+ ERROR_MESSAGE() ;
    
    RaisError( @ErrMsg ,16,-1)

END CATCH
'
 

SET @SqlCmd_Combin = @SqlCmd1 + @SqlCmd2
	EXEC sp_executesql @SqlCmd_Combin

End
