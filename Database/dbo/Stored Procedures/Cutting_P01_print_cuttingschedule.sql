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

	SELECT 
	[SP#] = C.ID
	,[Style] = O.StyleID
	,[Cutting Qty]=format(tmp2.cut_qty,'#,0.')
	,[Ttl. Cons] = tmp3.TTLCons
	,[Ttl. Layer] = tmp3.ttlLayer
	,[ETA]=O.LETA
	,[Earliest Sewing Inline] = O.SewInLine
	,[Earliest Sewing offline] = O.SewOffLine
	,[Cutting in-Line] = C.CutForOutputInline
	,[Cutting off-line] = C.CutForOutputOffLine
	,[No. of CutPlanID]=tmp.count_CutplanID
	,[First Cut Date]=C.FirstCutDate
	,[Last Cut Date]=C.LastCutDate
	,[Remark]=C.Remark
	FROM Cutting C
	LEFT JOIN Orders O on C.ID=O.ID
	OUTER APPLY 
	( 
		SELECT count(distinct CutplanID) count_CutplanID 
		FROM WorkOrderForPlanning 
		where ID=C.ID and CutplanID is not null and CutplanID!='' 
	) tmp 
	OUTER APPLY 
	( 
		SELECT isnull(sum(WD.Qty),0) cut_qty
		FROM WorkOrderForOutput W 
		inner join WorkOrderForPlanning P WITH(NOLOCK) ON W.WorkOrderForPlanningUkey = P.Ukey
		left join WorkOrderForOutput_Distribute WD on W.Ukey=WD.WorkOrderForOutputUkey 
		where W.ID=C.ID and p.CutplanID is not null and p.CutplanID!=''  
	) tmp2 
	outer apply 
	( 
		select 
		TTLCons = isnull(sum(Cons),0)
		,ttlLayer = isnull(sum(Layer),0) 
		from WorkOrderForOutput w with(nolock) 
		where W.ID=C.ID
	)tmp3

	WHERE C.MDivisionID = @M AND C.Finished = @Finished--依登入工廠及開啟的程式改變
	order by C.ID

END