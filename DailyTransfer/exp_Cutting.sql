
-- =============================================
-- Author:		<Leo 01921>
-- Create date: <2016/08/17>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exp_Cutting] 
	-- Add the parameters for the stored procedure here
	--<@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	--<@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
IF OBJECT_ID(N'dbo.CuttingOutput') IS NOT NULL
BEGIN
  DROP TABLE CuttingOutput
END
IF OBJECT_ID(N'dbo.CuttingOutput_Detail') IS NOT NULL
BEGIN
  DROP TABLE CuttingOutput_Detail
END
IF OBJECT_ID(N'dbo.CuttingOutput_Detail_Detail') IS NOT NULL
BEGIN
  DROP TABLE CuttingOutput_Detail_Detail
END
SELECT ID, cDATE, MDivisionid,MANPOWER, MANHOURS, Actoutput, ActGarment, AddName, AddDate, EditName, EditDate
INTO CuttingOutput
 FROM Production.dbo.CuttingOutput  CUT1
WHERE CUT1. Status != 'New' 
AND  (CUT1.Lock  BETWEEN (SELECT DATEADD(DAY,1,SewLock) FROM Production.dbo.System) AND  getdate() or  CUT1.Lock IS NULL)


--SELECT 
--CUT1. ID,CUT2.CutRef,CUT2.CuttingID,
--(select isnull(FabricCombo,'') from Production.dbo.WorkOrder where UKey = CUT2.WorkOrderUKey) as FabricCombo,
--CUT2. Cutno,CUT2. MarkerName,CUT2. Markerlength,CUT2. Layer,CUT2. Cons,
--CUT2 .UKey, CUT2. WorkOrderUkey,CUT2. Colorid
--INTO CuttingOutput_Detail
--FROM Production.dbo.CuttingOutput CUT1 , Production.dbo.CuttingOutput_Detail CUT2  
--WHERE CUT1. ID = CUT2.ID
SELECT ID,CutRef,CuttingID,isnull(FabricCombo,'') as FabricCombo,Cutno,MarkerName,Markerlength,Layer,Cons,UKey,WorkOrderUkey,Colorid 
INTO CuttingOutput_Detail
FROM (
SELECT 
CUT1. ID,CUT2.CutRef,CUT2.CuttingID,
(select isnull(FabricCombo,'') from Production.dbo.WorkOrder where UKey = CUT2.WorkOrderUKey) as FabricCombo,
CUT2. Cutno,CUT2. MarkerName,CUT2. Markerlength,CUT2. Layer,CUT2. Cons,
CUT2 .UKey, CUT2. WorkOrderUkey,CUT2. Colorid
--INTO CuttingOutput_Detail
FROM Production.dbo.CuttingOutput CUT1 , Production.dbo.CuttingOutput_Detail CUT2  
WHERE CUT1. ID = CUT2.ID
)as aaa

SELECT CUT1.ID,CUT3.CuttingOutput_detailUkey,CUT3.CuttingID, CUT3.SizeCode,CUT3.Qty
INTO  CuttingOutput_Detail_Detail
FROM Production.dbo.CuttingOutput CUT1, Production.dbo.CuttingOutput_Detail_Detail CUT3 
WHERE CUT1. ID = CUT3. ID

END




