using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R32 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DataTable final;
        private DateTime? Buyerdelivery1;
        private DateTime? Buyerdelivery2;
        private DateTime? AuditDate1;
        private DateTime? AuditDate2;
        private string sp1;
        private string sp2;
        private string MDivisionID;
        private string FactoryID;
        private string Brand;
        private string Stage;
        private string reportType;

        /// <inheritdoc/>
        public R32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource();
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sp1 = this.txtSP_s.Text;
            this.sp2 = this.txtSP_e.Text;
            this.MDivisionID = this.comboM.Text;
            this.FactoryID = this.comboFactory.Text;
            this.Brand = this.txtBrand.Text;
            this.Stage = this.comboStage.Text;
            this.Buyerdelivery1 = this.dateBuyerDev.Value1;
            this.Buyerdelivery2 = this.dateBuyerDev.Value2;
            this.AuditDate1 = this.AuditDate.Value1;
            this.AuditDate2 = this.AuditDate.Value2;
            this.reportType = this.radioSummary.Checked ? "Summary" : "Detail";

            if (MyUtility.Check.Empty(this.AuditDate1) &&
                    MyUtility.Check.Empty(this.AuditDate2) &&
                    MyUtility.Check.Empty(this.Buyerdelivery1) &&
                    MyUtility.Check.Empty(this.Buyerdelivery2) &&
                    MyUtility.Check.Empty(this.sp1) &&
                    MyUtility.Check.Empty(this.sp2))
            {
                MyUtility.Msg.InfoBox("Audit Date ,Buyer Delivery and SP# can't be all empty.");

                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();

            if (this.reportType == "Summary")
            {
                #region SQL

                sqlCmd.Append($@"
SELECT c.*,co.OrderID,co.SEQ
INTO #MainData1
FROm CFAInspectionRecord  c
INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
INNER JOIN Orders O ON o.ID = co.OrderID
WHERE 1=1
");

                #region Where
                if (!MyUtility.Check.Empty(this.AuditDate1) && !MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append($"AND c.AuditDate BETWEEN @AuditDate1 AND @AuditDate2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@AuditDate1", this.AuditDate1.Value));
                    paramList.Add(new SqlParameter("@AuditDate2", this.AuditDate2.Value));
                }

                if (!MyUtility.Check.Empty(this.Buyerdelivery1) && !MyUtility.Check.Empty(this.Buyerdelivery1))
                {
                    sqlCmd.Append($"AND o.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Buyerdelivery1", this.Buyerdelivery1.Value));
                    paramList.Add(new SqlParameter("@BuyerDelivery2", this.Buyerdelivery2.Value));
                }

                if (!MyUtility.Check.Empty(this.MDivisionID))
                {
                    sqlCmd.Append($"AND o.MDivisionID=@MDivisionID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@MDivisionID", this.MDivisionID));
                }

                if (!MyUtility.Check.Empty(this.FactoryID))
                {
                    sqlCmd.Append($"AND o.FtyGroup =@FactoryID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@FactoryID", this.FactoryID));
                }

                if (!MyUtility.Check.Empty(this.Brand))
                {
                    sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Brand", this.Brand));
                }

                if (!MyUtility.Check.Empty(this.sp1))
                {
                    sqlCmd.Append($"AND co.OrderID  >= @sp1" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@sp1", SqlDbType.VarChar, 13) { Value = this.sp1 });
                }

                if (!MyUtility.Check.Empty(this.sp2))
                {
                    sqlCmd.Append($"AND co.OrderID  <= @sp2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@sp2", SqlDbType.VarChar, 13) { Value = this.sp2 });
                }

                if (!MyUtility.Check.Empty(this.Stage))
                {
                    sqlCmd.Append($"AND c.Stage=@Stage " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Stage", this.Stage));
                }
                #endregion

                sqlCmd.Append($@"

SELECT c.*,co.OrderID,co.SEQ
INTO #MainData
FROm CFAInspectionRecord  c
INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
WHERE c.ID IN (SELECT ID FROM #MainData1)

SELECT 
	 c.AuditDate
	,BuyerDelivery = (SELECT BuyerDelivery FROM Orders WHERE ID = c.OrderID)
	,c.OrderID
	,CustPoNo = (SELECT CustPoNo FROM Orders WHERE ID = c.OrderID)
	,StyleID = (SELECT StyleID FROM Orders WHERE ID = c.OrderID)
	,BrandID = (SELECT BrandID FROM Orders WHERE ID = c.OrderID)
	,Dest = (SELECT Dest FROM Orders WHERE ID = c.OrderID)
	,Seq = (SELECT Seq FROM Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.SewingLineID
	,[VasShas]= (SELECT IIF(VasShas=1,'Y','N')  FROM Orders WHERE ID = c.OrderID)
	,c.ClogReceivedPercentage
	,c.MDivisionid
	,c.FactoryID
	,c.Shift
	,c.Team
	,Qty = (SELECT Qty FROM Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.Status
	,c.Carton
	,[CFA] = dbo.getPass1(c.CFA)
	,c.stage
	,c.Result
	,c.InspectQty
	,c.DefectQty
	,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty) * 100)
	,[DefectDescription] = (
		SELECT STUFF((
			SELECT DISTINCT  CHAR(10) + g.Description
			FROM CFAInspectionRecord_Detail cd
			LEFT JOIN GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
			WHERE cd.ID = c.ID
			 FOR XML PATH('')
			),1,1,'')
	)
	,[AreaCodeDesc]=(
		SELECT STUFF((
			SELECT DISTINCT  CHAR(10) + cd.CFAAreaID + ' - ' + CfaArea.Description
			FROM CFAInspectionRecord_Detail cd
			LEFT JOIN GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
			LEFT JOIN CfaArea ON CfaArea.ID = cd.CFAAreaID
			WHERE cd.ID = c.ID
			 FOR XML PATH('')
			),1,1,'')
	)
	,[NoOfDefect] = (SELECT SUM(Qty) FROM CFAInspectionRecord_Detail WHERE ID = c.ID)
	,c.Remark
	,c.ID
	,c.IsCombinePO
	,[InsCtn]=IIF(c.stage = 'Final' OR c.Stage ='3rd party',
	( 
		SELECT [Val]= COUNT(DISTINCT cr.ID) + 1
		FROM CFAInspectionRecord cr
		INNER JOIN CFAInspectionRecord_OrderSEQ crd ON cr.ID = crd.ID
		WHERE crd.OrderID=c.OrderID AND crd.SEQ=c.SEQ
	    AND cr.Status = 'Confirmed'
	    AND cr.Stage=c.Stage
	    AND cr.AuditDate <= c.AuditDate
	    AND cr.ID  != c.ID
	)
	,NULL)
	, Action = (
		SELECT STUFF((SELECT DISTINCT CHAR(10)+ Action FROM CFAInspectionRecord_Detail WHERE ID = c.ID FOR XML PATH('')),1,1,'')
	)
INTO #tmp
FROm #MainData  c

SELECT pd.*
INTO #PackingList_Detail
FROM PackingList_Detail pd
INNER JOIN #tmp t ON pd.OrderID = t.OrderID ANd pd.OrderShipmodeSeq = t.SEQ AND t.ID = pd.StaggeredCFAInspectionRecordID


SELECT DISTINCT pd.*
INTO #PackingList_Detail2
FROM PackingList_Detail pd 
INNER JOIN #tmp t ON pd.OrderID = t.OrderID ANd pd.OrderShipmodeSeq = t.SEQ 

");
                if (this.reportType == "Summary")
                {
                    sqlCmd.Append($@"
SELECT  t.ID
        ,AuditDate
		,BuyerDelivery
		,OrderID
		,CustPoNo
		,StyleID
		,BrandID
		,Dest
		,Seq
	    ,SewingLineID
		,VasShas
		,ClogReceivedPercentage
		,MDivisionid
		,FactoryID
		,Shift
		,Team
		,Qty
		,Status
		,[TTL CTN] = TtlCtn.Val
		,[Inspected Ctn] = InspectedCtn.Val
		,[Inspected PoQty]=InspectedPoQty.Val
		,Carton
		,CFA
		,Stage
		,InsCtn = IIF(IsCombinePO=1,NULL,InsCtn)
		,Result
		,InspectQty
		,DefectQty
		,SQR
		,Remark
FROM  #tmp t
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail2 pd
	WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq AND pd.CTNQty=1
)TtlCtn
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID AND pd.CTNQty=1
)InspectedCtn
OUTER APPLY(
	SELECT [Val] = SUM(DISTINCT pd.ShipQty)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID
)InspectedPoQty

DROP TABLE #tmp ,#PackingList_Detail ,#MainData ,#PackingList_Detail2,#MainData1
");
                }

                // ISP20201551 的Detail寫法，先保留
                if (this.reportType == "Detail")
                {
                    sqlCmd.Append($@"
SELECT  t.ID
        ,AuditDate
		,BuyerDelivery
		,OrderID
		,CustPoNo
		,StyleID
		,BrandID
		,Dest
		,Seq
        ,SewingLineID
		,VasShas
		,ClogReceivedPercentage
		,MDivisionid
		,FactoryID
		,Shift
		,Team
		,Qty
		,Status
		,[TTL CTN] = TtlCtn.Val
		,[Inspected Ctn] = InspectedCtn.Val
		,[Inspected PoQty]=InspectedPoQty.Val
		,Carton
		,CFA
		,Stage
		,InsCtn = IIF(IsCombinePO=1,NULL,InsCtn)
		,Result
		,InspectQty
		,DefectQty
		,SQR
		,DefectDescription
		,AreaCodeDesc
		,NoOfDefect
		,Remark
		,Action
FROM  #tmp t
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail2 pd
	WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq AND pd.CTNQty=1
)TtlCtn
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID AND pd.CTNQty=1
)InspectedCtn
OUTER APPLY(
	SELECT [Val] = SUM(DISTINCT pd.ShipQty)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID
)InspectedPoQty

DROP TABLE #tmp ,#PackingList_Detail ,#MainData ,#PackingList_Detail2,#MainData1
");
                }

                #endregion

            }

            if (this.reportType == "Detail")
            {
                #region SQL

                sqlCmd.Append($@"
SELECT c.*,co.OrderID,co.SEQ
INTO #MainData1
FROm CFAInspectionRecord  c
INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
INNER JOIN Orders O ON o.ID = co.OrderID
WHERE 1=1
");

                #region Where
                if (!MyUtility.Check.Empty(this.AuditDate1) && !MyUtility.Check.Empty(this.AuditDate1))
                {
                    sqlCmd.Append($"AND c.AuditDate BETWEEN @AuditDate1 AND @AuditDate2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@AuditDate1", this.AuditDate1.Value));
                    paramList.Add(new SqlParameter("@AuditDate2", this.AuditDate2.Value));
                }

                if (!MyUtility.Check.Empty(this.Buyerdelivery1) && !MyUtility.Check.Empty(this.Buyerdelivery1))
                {
                    sqlCmd.Append($"AND o.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Buyerdelivery1", this.Buyerdelivery1.Value));
                    paramList.Add(new SqlParameter("@BuyerDelivery2", this.Buyerdelivery2.Value));
                }

                if (!MyUtility.Check.Empty(this.MDivisionID))
                {
                    sqlCmd.Append($"AND o.MDivisionID=@MDivisionID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@MDivisionID", this.MDivisionID));
                }

                if (!MyUtility.Check.Empty(this.FactoryID))
                {
                    sqlCmd.Append($"AND o.FtyGroup =@FactoryID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@FactoryID", this.FactoryID));
                }

                if (!MyUtility.Check.Empty(this.Brand))
                {
                    sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Brand", this.Brand));
                }

                if (!MyUtility.Check.Empty(this.sp1))
                {
                    sqlCmd.Append($"AND co.OrderID  >= @sp1" + Environment.NewLine);
                    SqlParameter p = new SqlParameter("@sp1", SqlDbType.VarChar)
                    {
                        Value = this.sp1,
                    };
                    paramList.Add(p);
                }

                if (!MyUtility.Check.Empty(this.sp2))
                {
                    sqlCmd.Append($"AND co.OrderID  <= @sp2" + Environment.NewLine);
                    SqlParameter p = new SqlParameter("@sp2", SqlDbType.VarChar)
                    {
                        Value = this.sp2,
                    };
                    paramList.Add(p);
                }

                if (!MyUtility.Check.Empty(this.Stage))
                {
                    sqlCmd.Append($"AND c.Stage=@Stage " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Stage", this.Stage));
                }
                #endregion

                sqlCmd.Append($@"

SELECT c.*,co.OrderID,co.SEQ
INTO #MainData
FROm CFAInspectionRecord  c
INNER JOIN CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
WHERE c.ID IN (SELECT ID FROM #MainData1)

SELECT 
	 c.AuditDate
	,BuyerDelivery = (SELECT BuyerDelivery FROM Orders WHERE ID = c.OrderID)
	,c.OrderID
	,CustPoNo = (SELECT CustPoNo FROM Orders WHERE ID = c.OrderID)
	,StyleID = (SELECT StyleID FROM Orders WHERE ID = c.OrderID)
	,BrandID = (SELECT BrandID FROM Orders WHERE ID = c.OrderID)
	,Dest = (SELECT Dest FROM Orders WHERE ID = c.OrderID)
	,Seq = (SELECT Seq FROM Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.SewingLineID
	,[VasShas]= (SELECT IIF(VasShas=1,'Y','N')  FROM Orders WHERE ID = c.OrderID)
	,c.ClogReceivedPercentage
	,c.MDivisionid
	,c.FactoryID
	,c.Shift
	,c.Team
	,Qty = (SELECT Qty FROM Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
	,c.Status
	,c.Carton
	,[CFA] = dbo.getPass1(c.CFA)
	,c.stage
	,c.Result
	,c.InspectQty
	,c.DefectQty
	,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty) * 100)
	,[DefectDescription] = g.Description
	,[AreaCodeDesc] = cd.CFAAreaID + ' - ' + CfaArea.Description
	,[NoOfDefect] = cd.Qty
	,cd.Remark
	,c.ID
	,c.IsCombinePO
	,[InsCtn]=IIF(c.stage = 'Final' OR c.Stage ='3rd party',
	( 
		SELECT [Val]= COUNT(DISTINCT cr.ID) + 1
		FROM CFAInspectionRecord cr
		INNER JOIN CFAInspectionRecord_OrderSEQ crd ON cr.ID = crd.ID
		WHERE crd.OrderID=c.OrderID AND crd.SEQ=c.SEQ
	    AND cr.Status = 'Confirmed'
	    AND cr.Stage=c.Stage
	    AND cr.AuditDate <= c.AuditDate
	    AND cr.ID  != c.ID
	)
	,NULL)
	, [Action]= cd.Action
	,[CFAInspectionRecord_Detail_Key]= concat(c.ID,iif(isnull(cd.GarmentDefectCodeID, '') = '', concat(row_Number()over(order by c.ID),''), cd.GarmentDefectCodeID))
INTO #tmp
FROm #MainData  c
LEFT JOIN CFAInspectionRecord_Detail cd ON c.ID = cd.ID
LEFT JOIN GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
LEFT JOIN CfaArea ON CfaArea.ID = cd.CFAAreaID


SELECT pd.*
INTO #PackingList_Detail
FROM PackingList_Detail pd
INNER JOIN #tmp t ON pd.OrderID = t.OrderID ANd pd.OrderShipmodeSeq = t.SEQ AND t.ID = pd.StaggeredCFAInspectionRecordID


SELECT DISTINCT pd.*
INTO #PackingList_Detail2
FROM PackingList_Detail pd 
INNER JOIN #tmp t ON pd.OrderID = t.OrderID ANd pd.OrderShipmodeSeq = t.SEQ 


SELECT   t.ID
		,CFAInspectionRecord_Detail_Key
        ,AuditDate
		,BuyerDelivery
		,OrderID
		,CustPoNo
		,StyleID
		,BrandID
		,Dest
		,Seq
        ,SewingLineID
		,VasShas
		,ClogReceivedPercentage
		,MDivisionid
		,FactoryID
		,Shift
		,Team
		,Qty
		,Status
		,[TTL CTN] = TtlCtn.Val
		,[Inspected Ctn] = InspectedCtn.Val
		,[Inspected PoQty]=InspectedPoQty.Val
		,Carton
		,CFA
		,Stage
		,InsCtn = IIF(IsCombinePO=1,NULL,InsCtn)
		,Result
		,InspectQty
		,DefectQty
		,SQR
		,DefectDescription
		,AreaCodeDesc
		,NoOfDefect
		,Remark
		,Action
FROM  #tmp t
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail2 pd
	WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq AND pd.CTNQty=1
)TtlCtn
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID AND pd.CTNQty=1
)InspectedCtn
OUTER APPLY(
	SELECT [Val] = SUM(DISTINCT pd.ShipQty)
	FROM #PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= t.ID
)InspectedPoQty
Order by id

DROP TABLE #tmp ,#PackingList_Detail ,#MainData ,#PackingList_Detail2,#MainData1

");
                #endregion
            }

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), paramList, out this.final);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            this.printData = new DataTable();
            if (this.reportType == "Summary")
            {
                List<string> idList = this.final.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["ID"])).Distinct().ToList();
                List<DataRow> orderList = this.final.AsEnumerable().OrderBy(o => MyUtility.Convert.GetString(o["OrderID"])).ToList();

                this.printData.ColumnsStringAdd("AuditDate");
                this.printData.ColumnsStringAdd("BuyerDelivery");
                this.printData.ColumnsStringAdd("OrderID");
                this.printData.ColumnsStringAdd("CustPoNo");
                this.printData.ColumnsStringAdd("StyleID");

                this.printData.ColumnsStringAdd("BrandID");
                this.printData.ColumnsStringAdd("Dest");
                this.printData.ColumnsStringAdd("Seq");
                this.printData.ColumnsStringAdd("SewingLineID");
                this.printData.ColumnsStringAdd("VasShas");

                this.printData.ColumnsStringAdd("ClogReceivedPercentage");
                this.printData.ColumnsStringAdd("MDivisionid");
                this.printData.ColumnsStringAdd("FactoryID");
                this.printData.ColumnsStringAdd("Shift");
                this.printData.ColumnsStringAdd("Team");

                this.printData.ColumnsIntAdd("Qty");
                this.printData.ColumnsStringAdd("Status");
                this.printData.ColumnsIntAdd("TTL CTN");
                this.printData.ColumnsIntAdd("Inspected Ctn");
                this.printData.ColumnsIntAdd("Inspected PoQty");

                this.printData.ColumnsStringAdd("Carton");
                this.printData.ColumnsStringAdd("CFA");
                this.printData.ColumnsStringAdd("Stage");
                this.printData.ColumnsStringAdd("InsCtn");
                this.printData.ColumnsStringAdd("Result");

                this.printData.ColumnsIntAdd("InspectQty");
                this.printData.ColumnsIntAdd("DefectQty");
                this.printData.ColumnsDecimalAdd("SQR");
                this.printData.ColumnsStringAdd("Remark");

                foreach (string cFAInspectionRecord_ID in idList)
                {
                    DataRow nRow = this.printData.NewRow();
                    List<DataRow> sameIDs = orderList.Where(o => MyUtility.Convert.GetString(o["ID"]) == cFAInspectionRecord_ID).ToList();

                    nRow["AuditDate"] = MyUtility.Convert.GetDate(sameIDs.FirstOrDefault()["AuditDate"]).Value.ToShortDateString();
                    nRow["BuyerDelivery"] = sameIDs.Select(o => MyUtility.Convert.GetDate(o["BuyerDelivery"]).Value.ToShortDateString()).JoinToString(Environment.NewLine);
                    nRow["OrderID"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["OrderID"])).JoinToString(Environment.NewLine);
                    nRow["CustPoNo"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["CustPoNo"])).JoinToString(Environment.NewLine);
                    nRow["StyleID"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["StyleID"])).JoinToString(Environment.NewLine);

                    nRow["BrandID"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["BrandID"])).JoinToString(Environment.NewLine);
                    nRow["Dest"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["Dest"])).JoinToString(Environment.NewLine);
                    nRow["Seq"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["Seq"])).JoinToString(Environment.NewLine);
                    nRow["SewingLineID"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["SewingLineID"]);
                    nRow["VasShas"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["VasShas"])).JoinToString(Environment.NewLine);

                    nRow["ClogReceivedPercentage"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["ClogReceivedPercentage"]);
                    nRow["MDivisionid"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["MDivisionid"]);
                    nRow["FactoryID"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["FactoryID"]);
                    nRow["Shift"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Shift"]);
                    nRow["Team"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Team"]);

                    nRow["Qty"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["Qty"]));
                    nRow["Status"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Status"]);
                    nRow["TTL CTN"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["TTL CTN"]));
                    nRow["Inspected Ctn"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["Inspected Ctn"]));
                    nRow["Inspected PoQty"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["Inspected PoQty"]));

                    nRow["Carton"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Carton"]);
                    nRow["CFA"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["CFA"]);
                    nRow["Stage"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Stage"]);
                    nRow["InsCtn"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["InsCtn"]);
                    nRow["Result"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Result"]);

                    // 表頭 CFAInspectionRecord 的值, 不重複加總
                    nRow["InspectQty"] = MyUtility.Convert.GetInt(sameIDs.FirstOrDefault()["InspectQty"]);
                    nRow["DefectQty"] = MyUtility.Convert.GetInt(sameIDs.FirstOrDefault()["DefectQty"]);
                    nRow["SQR"] = MyUtility.Convert.GetDecimal(sameIDs.FirstOrDefault()["SQR"]);
                    nRow["Remark"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Remark"]);

                    this.printData.Rows.Add(nRow);
                }
            }

            if (this.reportType == "Detail")
            {
                List<string> idList = this.final.AsEnumerable().Select(o => MyUtility.Convert.GetString(o["CFAInspectionRecord_Detail_Key"])).Distinct().ToList();
                List<DataRow> orderList = this.final.AsEnumerable().OrderBy(o => MyUtility.Convert.GetString(o["OrderID"])).ToList();

                this.printData.ColumnsStringAdd("AuditDate");
                this.printData.ColumnsStringAdd("BuyerDelivery");
                this.printData.ColumnsStringAdd("OrderID");
                this.printData.ColumnsStringAdd("CustPoNo");
                this.printData.ColumnsStringAdd("StyleID");

                this.printData.ColumnsStringAdd("BrandID");
                this.printData.ColumnsStringAdd("Dest");
                this.printData.ColumnsStringAdd("Seq");
                this.printData.ColumnsStringAdd("SewingLineID");
                this.printData.ColumnsStringAdd("VasShas");

                this.printData.ColumnsStringAdd("ClogReceivedPercentage");
                this.printData.ColumnsStringAdd("MDivisionid");
                this.printData.ColumnsStringAdd("FactoryID");
                this.printData.ColumnsStringAdd("Shift");
                this.printData.ColumnsStringAdd("Team");

                this.printData.ColumnsIntAdd("Qty");
                this.printData.ColumnsStringAdd("Status");
                this.printData.ColumnsIntAdd("TTL CTN");
                this.printData.ColumnsIntAdd("Inspected Ctn");
                this.printData.ColumnsIntAdd("Inspected PoQty");

                this.printData.ColumnsStringAdd("Carton");
                this.printData.ColumnsStringAdd("CFA");
                this.printData.ColumnsStringAdd("Stage");
                this.printData.ColumnsStringAdd("InsCtn");
                this.printData.ColumnsStringAdd("Result");

                this.printData.ColumnsIntAdd("InspectQty");
                this.printData.ColumnsIntAdd("DefectQty");
                this.printData.ColumnsDecimalAdd("SQR");
                this.printData.ColumnsStringAdd("DefectDescription");
                this.printData.ColumnsStringAdd("AreaCodeDesc");

                this.printData.ColumnsIntAdd("NoOfDefect");
                this.printData.ColumnsStringAdd("Remark");
                this.printData.ColumnsStringAdd("Action");

                // this.printData = this.final.Copy();
                // ISP20201551 的Detail寫法，先保留
                foreach (var cFAInspectionRecord_ID in idList)
                {
                    DataRow nRow = this.printData.NewRow();
                    List<DataRow> sameIDs = orderList.Where(o => MyUtility.Convert.GetString(o["CFAInspectionRecord_Detail_Key"]) == cFAInspectionRecord_ID).ToList();

                    nRow["AuditDate"] = MyUtility.Convert.GetDate(sameIDs.FirstOrDefault()["AuditDate"]).Value.ToShortDateString();
                    nRow["BuyerDelivery"] = sameIDs.Select(o => MyUtility.Convert.GetDate(o["BuyerDelivery"]).Value.ToShortDateString()).JoinToString(Environment.NewLine);
                    nRow["OrderID"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["OrderID"])).JoinToString(Environment.NewLine);
                    nRow["CustPoNo"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["CustPoNo"])).JoinToString(Environment.NewLine);
                    nRow["StyleID"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["StyleID"])).JoinToString(Environment.NewLine);

                    nRow["BrandID"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["BrandID"])).JoinToString(Environment.NewLine);
                    nRow["Dest"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["Dest"])).JoinToString(Environment.NewLine);
                    nRow["Seq"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["Seq"])).JoinToString(Environment.NewLine);
                    nRow["SewingLineID"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["SewingLineID"]);
                    nRow["VasShas"] = sameIDs.Select(o => MyUtility.Convert.GetString(o["VasShas"])).JoinToString(Environment.NewLine);

                    nRow["ClogReceivedPercentage"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["ClogReceivedPercentage"]);
                    nRow["MDivisionid"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["MDivisionid"]);
                    nRow["FactoryID"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["FactoryID"]);
                    nRow["Shift"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Shift"]);
                    nRow["Team"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Team"]);

                    nRow["Qty"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["Qty"]));
                    nRow["Status"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Status"]);
                    nRow["TTL CTN"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["TTL CTN"]));
                    nRow["Inspected Ctn"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["Inspected Ctn"]));
                    nRow["Inspected PoQty"] = sameIDs.Sum(o => MyUtility.Convert.GetInt(o["Inspected PoQty"]));

                    nRow["Carton"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Carton"]);
                    nRow["CFA"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["CFA"]);
                    nRow["Stage"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Stage"]);
                    nRow["InsCtn"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["InsCtn"]);
                    nRow["Result"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Result"]);

                    nRow["InspectQty"] = MyUtility.Convert.GetInt(sameIDs.FirstOrDefault()["InspectQty"]);
                    nRow["DefectQty"] = MyUtility.Convert.GetInt(sameIDs.FirstOrDefault()["DefectQty"]);
                    nRow["SQR"] = MyUtility.Convert.GetDecimal(sameIDs.FirstOrDefault()["SQR"]);
                    nRow["DefectDescription"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["DefectDescription"]);
                    nRow["AreaCodeDesc"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["AreaCodeDesc"]);

                    nRow["NoOfDefect"] = MyUtility.Convert.GetInt(sameIDs.FirstOrDefault()["NoOfDefect"]);
                    nRow["Remark"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Remark"]);
                    nRow["Action"] = MyUtility.Convert.GetString(sameIDs.FirstOrDefault()["Action"]);

                    this.printData.Rows.Add(nRow);
                }
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string templateName = this.reportType == "Summary" ? "Quality_R32_Summary" : "Quality_R32_Detail";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{templateName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{templateName}.xltx", 2, false, null, objApp); // 將datatable copy to excel

            // 客製化欄位，記得設定this.IsSupportCopy = true
            // this.CreateCustomizedExcel(ref objSheets);
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Quality_R32");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            #endregion
            return true;
        }
    }
}
