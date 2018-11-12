-- =============================================
-- Author:		<JEFF S01952>
-- Create date: <2017/03/07>
-- Description:	<Cutting_P01_print_cuttingschedule 01>
-- =============================================
CREATE PROCEDURE [dbo].[Cutting_P01_print_cuttingschedule]
	@M VARCHAR(8),
	@Finished int
AS
BEGIN	

	SELECT SP#=C.ID
		,Style=O.StyleID
		,[Order Qty]=format(tmp2.cut_qty,'#,0.')
		,tmp3.TTLCons
		,tmp3.ttlLayer
		,ETA=O.LETA
		,[Earliest Sewing Inline]=O.SewInLine
		,[Earliest Sewing offline]=O.SewOffLine
		,[Cutting in-Line]=O.CutInLine
		,[Cutting off-line]=O.CutOffLine
		,[No. of Cut]=tmp.count_CutplanID
		,[Remark]=C.Remark
		,[First Cut Date]=C.FirstCutDate
		,[Last Cut Date]=C.LastCutDate
	FROM Cutting C
	LEFT JOIN Orders O on C.ID=O.ID
	OUTER APPLY ( SELECT count(distinct CutplanID) count_CutplanID FROM WorkOrder where ID=C.ID and CutplanID is not null and CutplanID!='' ) tmp 
	OUTER APPLY ( SELECT isnull(sum(WD.Qty),0) cut_qty FROM WorkOrder W left join WorkOrder_Distribute WD on W.Ukey=WD.WorkOrderUkey 
	where W.ID=C.ID and W.CutplanID is not null and W.CutplanID!='' ) tmp2 
	outer apply ( select TTLCons = isnull(sum(Cons),0),ttlLayer = isnull(sum(Layer),0) from WorkOrder w with(nolock) where W.ID=C.ID)tmp3

	WHERE C.MDivisionID = @M AND C.Finished = @Finished  --依登入工廠及開啟的程式改變
	order by C.ID

END