using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class R32 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
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

        public R32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource();
        }

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
                    MyUtility.Check.Empty(this.sp2)
                )
            {
                MyUtility.Msg.InfoBox("Audit Date ,Buyer Delivery and SP# can't be all empty.");

                return false;
            }

            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();

            #region SQL
            if (this.reportType == "Summary")
            {
                sqlCmd.Append($@"
SELECT 
	 c.AuditDate
	,o.BuyerDelivery
	,c.OrderID
	,o.CustPoNo
	,o.StyleID
	,o.BrandID
	,o.Dest
	,oq.Seq
	,c.SewingLineID
	,[VasShas]=IIF(o.VasShas=1,'Y','N')
	,c.ClogReceivedPercentage
	,c.MDivisionid
	,c.FactoryID
	,c.Shift
	,c.Team
	,oq.Qty
	,c.Status
	,c.Carton
	,[CFA] = dbo.getPass1(c.CFA)
	,c.stage
	,c.Result
	,c.InspectQty
	,c.DefectQty
	,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty) * 100)
	,c.Remark
	,c.ID
	,[InsCtn]=IIF(c.stage = 'Final' OR c.Stage ='3rd party', InsCtn.Val,NULL)
INTO #tmp
FROm CFAInspectionRecord  c
INNER JOIN Order_QtyShip oq ON c.OrderID = oq.ID AND c.Seq = oq.Seq
INNER JOIN Orders o  On o.ID = oq.ID
OUTER APPLY(
	SELECT [Val]= COUNT(1) + 1
	FROM CFAInspectionRecord cr
	WHERE cr.OrderID=c.OrderID AND cr.SEQ=c.SEQ
	    AND cr.Status = 'Confirmed'
	    AND cr.Stage=c.Stage
	    AND cr.AuditDate <= c.AuditDate
	    AND cr.ID  != c.ID
)InsCtn
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
                    sqlCmd.Append($"AND oq.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Buyerdelivery1", this.Buyerdelivery1.Value));
                    paramList.Add(new SqlParameter("@BuyerDelivery2", this.Buyerdelivery2.Value));
                }

                if (!MyUtility.Check.Empty(this.sp1))
                {
                    sqlCmd.Append($"AND c.OrderID  >= @sp1" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@sp1", this.sp1));
                }

                if (!MyUtility.Check.Empty(this.sp2))
                {
                    sqlCmd.Append($"AND c.OrderID  <= @sp2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@sp2", this.sp2));
                }

                if (!MyUtility.Check.Empty(this.MDivisionID))
                {
                    sqlCmd.Append($"AND c.MDivisionID=@MDivisionID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@MDivisionID", this.MDivisionID));
                }
                if (!MyUtility.Check.Empty(this.FactoryID))
                {
                    sqlCmd.Append($"AND c.FactoryID =@FactoryID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@FactoryID", this.FactoryID));

                }
                if (!MyUtility.Check.Empty(this.Brand))
                {
                    sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Brand", this.Brand));

                }
                if (!MyUtility.Check.Empty(this.Stage))
                {
                    sqlCmd.Append($"AND c.Stage=@Stage " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Stage", this.Stage));

                }
                #endregion

                sqlCmd.Append($@"


SELECT *
INTO #PackingList_Detail
FROM PackingList_Detail
WHERE StaggeredCFAInspectionRecordID IN (SELECT ID FROM #tmp )


SELECT  AuditDate
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
		,InsCtn
		,Result
		,InspectQty
		,DefectQty
		,SQR
		,Remark
FROM  #tmp t
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM PackingList_Detail pd
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

DROP TABLE #tmp ,#PackingList_Detail
");
            }

            if (this.reportType == "Detail")
            {
                sqlCmd.Append($@"
SELECT 
	 c.AuditDate
	,o.BuyerDelivery
	,c.OrderID
	,o.CustPoNo
	,o.StyleID
	,o.BrandID
	,o.Dest
	,oq.Seq
	,c.SewingLineID
	,[VasShas]=IIF(o.VasShas=1,'Y','N')
	,c.ClogReceivedPercentage
	,c.MDivisionid
	,c.FactoryID
	,c.Shift
	,c.Team
	,oq.Qty
	,c.Status
	,c.Carton
	,[CFA] = dbo.getPass1(c.CFA)
	,c.stage
	,c.Result
	,c.InspectQty
	,c.DefectQty
	,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty) * 100)
	,[DefectDescription] = g.Description
	,[AreaCodeDesc]= cd.CFAAreaID + ' - ' + CfaArea.Description
	,[NoOfDefect] = cd.Qty
	,c.Remark
	,c.ID
	,[InsCtn]=IIF(c.stage = 'Final' OR c.Stage ='3rd party', InsCtn.Val,NULL)
	,cd.Action
INTO #tmp
FROm CFAInspectionRecord  c
Left  JOIN CFAInspectionRecord_Detail cd ON c.ID = cd.ID
LEFT JOIN GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
LEFT JOIN CfaArea ON CfaArea.ID = cd.CFAAreaID
INNER JOIN Order_QtyShip oq ON c.OrderID = oq.ID AND c.Seq = oq.Seq
INNER JOIN Orders o  On o.ID = oq.ID
OUTER APPLY(
	SELECT [Val]= COUNT(1) + 1
	FROM CFAInspectionRecord cr
	WHERE cr.OrderID=c.OrderID AND cr.SEQ=c.SEQ
	    AND cr.Status = 'Confirmed'
	    AND cr.Stage=c.Stage
	    AND cr.AuditDate <= c.AuditDate
	    AND cr.ID  != c.ID
)InsCtn
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
                    sqlCmd.Append($"AND oq.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Buyerdelivery1", this.Buyerdelivery1.Value));
                    paramList.Add(new SqlParameter("@BuyerDelivery2", this.Buyerdelivery2.Value));
                }

                if (!MyUtility.Check.Empty(this.sp1))
                {
                    sqlCmd.Append($"AND c.OrderID  >= @sp1" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@sp1", this.sp1));
                }

                if (!MyUtility.Check.Empty(this.sp2))
                {
                    sqlCmd.Append($"AND c.OrderID  <= @sp2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@sp2", this.sp2));
                }

                if (!MyUtility.Check.Empty(this.MDivisionID))
                {
                    sqlCmd.Append($"AND c.MDivisionID=@MDivisionID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@MDivisionID", this.MDivisionID));
                }
                if (!MyUtility.Check.Empty(this.FactoryID))
                {
                    sqlCmd.Append($"AND c.FactoryID =@FactoryID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@FactoryID", this.FactoryID));

                }
                if (!MyUtility.Check.Empty(this.Brand))
                {
                    sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Brand", this.Brand));

                }
                if (!MyUtility.Check.Empty(this.Stage))
                {
                    sqlCmd.Append($"AND c.Stage=@Stage " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Stage", this.Stage));

                }
                #endregion

                sqlCmd.Append($@"


SELECT *
INTO #PackingList_Detail
FROM PackingList_Detail
WHERE StaggeredCFAInspectionRecordID IN (SELECT ID FROM #tmp )


SELECT  AuditDate
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
		,InsCtn
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
	FROM PackingList_Detail pd
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

DROP TABLE #tmp ,#PackingList_Detail
");
            }

            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), paramList, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string templateName = string.Empty;

            switch (this.reportType)
            {
                case "Summary":
                    templateName = "Quality_R32_Summary";
                    break;
                case "Detail":
                    templateName = "Quality_R32_Detail";
                    break;
                default:
                    break;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{templateName}.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{templateName}.xltx", 2, false, null, objApp); // 將datatable copy to excel

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            // 客製化欄位，記得設定this.IsSupportCopy = true
            //this.CreateCustomizedExcel(ref objSheets);

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R32");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion 
            return true;
        }
    }
}
