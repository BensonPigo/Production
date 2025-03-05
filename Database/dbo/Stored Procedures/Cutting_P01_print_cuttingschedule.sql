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
	--Cutting_P01.Cutting Master List
	[SP#] = C.ID
	,[Style] = O.StyleID
	,[First Cut Date] = FORMAT(c.FirstCutDate,'yyyy/MM/dd')
	,[Last Cut Date] = FORMAT(c.LastCutDate,'yyyy/MM/dd')
	,[Remark]=C.Remark
	-- PPIC_P01. PPIC Master List
	,[ETA] = FORMAT(o.LETA, 'yyyy/MM/dd')
	,[Earliest Sewing Inline] = FORMAT(o.SewInLine, 'yyyy/MM/dd')
	,[Earliest Sewing Offline] = FORMAT(o.SewOffLine, 'yyyy/MM/dd')
	--Cutting_P02. WorkOrder For Planning
	,[Cutting In-Line] = FORMAT(c.CutForPlanningInLine, 'yyyy/MM/dd')
	,[Cutting Off-line] = FORMAT(c.CutForPlanningOffLine, 'yyyy/MM/dd')
	,[No. of CutPlan] = (
		SELECT count(distinct CutplanID)
		FROM WorkOrderForPlanning
		where ID = C.ID and CutplanID is not null and CutplanID!=''
	)
	,[Cutting Qty] = (
		SELECT isnull(sum(wpd.Qty),0)
		FROM WorkOrderForPlanning wp
		left join WorkOrderForPlanning_Distribute wpd on wp.Ukey=wpd.WorkOrderForPlanningUkey
		where wp.ID=c.ID
	)
	,[Ttl. Cons] = (
		select isnull(sum(WorkOrderForPlanning.Cons),0)
		from WorkOrderForPlanning with(nolock)
		where WorkOrderForPlanning.ID=c.ID
	)
	,[Ttl. Layer] = (
		select isnull(sum(WorkOrderForPlanning.Layer),0)
		from WorkOrderForPlanning with(nolock)
		where WorkOrderForPlanning.ID=c.ID
	)
	-- Cutting_P09. WorkOrder For Output
	,[Cutting In-Line] = FORMAT(c.CutForOutputInline, 'yyyy/MM/dd')
	,[Cutting Off-line] = FORMAT(c.CutForOutputOffLine, 'yyyy/MM/dd')
	,[Cutting Qty] = (
		SELECT isnull(sum(WorkOrderForOutput_Distribute.Qty),0)
		FROM WorkOrderForOutput
		left join WorkOrderForOutput_Distribute on WorkOrderForOutput.Ukey=WorkOrderForOutput_Distribute.WorkOrderForOutputUkey
		where WorkOrderForOutput.ID = c.ID
	)
	,[Ttl. Cons] = (
		select isnull(sum(WorkOrderForOutput.Cons),0)
		from WorkOrderForOutput with(nolock)
		where WorkOrderForOutput.ID=c.ID
	)
	,[Ttl. Layer] = (
		select isnull(sum(WorkOrderForOutput.Layer),0)
		from WorkOrderForOutput with(nolock)
		where WorkOrderForOutput.ID=c.ID
	)	
	FROM Cutting C
	LEFT JOIN Orders O on C.ID=O.ID
	WHERE C.MDivisionID = @M AND C.Finished = @Finished--依登入工廠及開啟的程式改變
	order by C.ID

END