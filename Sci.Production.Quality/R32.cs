using Ict;
using Sci.Data;
using Sci.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace Sci.Production.Quality
{
    public partial class R32 : Sci.Win.Tems.PrintForm
    {
        DataTable printData;

        DateTime? Buyerdelivery1, Buyerdelivery2;
        DateTime? AuditDate1, AuditDate2;
        string sp1, sp2, MDivisionID, FactoryID, Brand ,Stage;

        public R32(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.comboM.setDefalutIndex(true);
            this.comboFactory.setDataSource();
        }


        protected override bool ValidateInput()
        {

            this.sp1 = txtSP_s.Text;
            this.sp2 = txtSP_e.Text;
            this.MDivisionID = comboM.Text;
            this.FactoryID = comboFactory.Text;
            this.Brand = txtBrand.Text;
            this.Stage = comboStage.Text;
            this.Buyerdelivery1 = dateBuyerDev.Value1;
            this.Buyerdelivery2 = dateBuyerDev.Value2;
            this.AuditDate1 = AuditDate.Value1;
            this.AuditDate2 = AuditDate.Value2;

            if (MyUtility.Check.Empty(AuditDate1) &&
                    MyUtility.Check.Empty(AuditDate2) && 
                    MyUtility.Check.Empty(Buyerdelivery1) &&
                    MyUtility.Check.Empty(Buyerdelivery2) &&
                    MyUtility.Check.Empty(sp1) &&
                    MyUtility.Check.Empty(sp2)
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
            sqlCmd.Append($@"
SELECT c.OrderID
,o.CustPoNo
,o.StyleID
,o.BrandID
,o.Dest
,oq.Seq
,o.VasShas
,c.ClogReceivedPercentage
,c.MDivisionid
,c.FactoryID
,c.Shift
,c.Team
,oq.Qty
,c.Status
,[TTL CTN] = TtlCtn.Val
,[Inspected Ctn] = InspectedCtn.Val
,[Inspected PoQty]=InspectedPoQty.Val
,c.Carton
,[CFA] = dbo.getPass1(c.CFA)
,c.stage
,c.Result
,c.InspectQty
,c.DefectQty
,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty))
,c.Remark


FROm CFAInspectionRecord  c
INNER JOIN Order_QtyShip oq ON c.OrderID = oq.ID AND c.Seq = oq.Seq
INNER JOIN Orders o  On o.ID = oq.ID
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM PackingList_Detail pd
	WHERE pd.OrderID = c.OrderID AND pd.OrderShipmodeSeq = c.SEQ AND pd.CTNQty=1
)TtlCtn
OUTER APPLY(
	SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
	FROM PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= c.ID AND pd.CTNQty=1
)InspectedCtn

OUTER APPLY(
	SELECT [Val] = SUM(DISTINCT pd.ShipQty)
	FROM PackingList_Detail pd
	WHERE pd.StaggeredCFAInspectionRecordID= c.ID 
)InspectedPoQty
WHERE 1=1
");
            #endregion

            #region Where
            if (!MyUtility.Check.Empty(AuditDate1) && !MyUtility.Check.Empty(AuditDate1))
            {
                sqlCmd.Append($"AND c.AuditDate BETWEEN @AuditDate1 AND @AuditDate2" + Environment.NewLine);
                paramList.Add(new SqlParameter("@AuditDate1", AuditDate1.Value));
                paramList.Add(new SqlParameter("@AuditDate2", AuditDate2.Value));
            }

            if (!MyUtility.Check.Empty(Buyerdelivery1) && !MyUtility.Check.Empty(Buyerdelivery1))
            {
                sqlCmd.Append($"AND oq.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                paramList.Add(new SqlParameter("@Buyerdelivery1", Buyerdelivery1.Value));
                paramList.Add(new SqlParameter("@BuyerDelivery2", Buyerdelivery2.Value));
            }

            if (!MyUtility.Check.Empty(sp1))
            {
                sqlCmd.Append($"AND c.OrderID  >= @sp1" + Environment.NewLine);
                paramList.Add(new SqlParameter("@sp1", sp1));
            }

            if (!MyUtility.Check.Empty(sp2))
            {
                sqlCmd.Append($"AND c.OrderID  <= @sp2" + Environment.NewLine);
                paramList.Add(new SqlParameter("@sp2", sp2));
            }

            if (!MyUtility.Check.Empty(MDivisionID))
            {
                sqlCmd.Append($"AND c.MDivisionID=@MDivisionID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@MDivisionID", MDivisionID));
            }
            if (!MyUtility.Check.Empty(FactoryID))
            {
                sqlCmd.Append($"AND c.FactoryID =@FactoryID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@FactoryID", FactoryID));

            }
            if (!MyUtility.Check.Empty(Brand))
            {
                sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                paramList.Add(new SqlParameter("@Brand", Brand));

            }
            if (!MyUtility.Check.Empty(Stage))
            {
                sqlCmd.Append($"AND c.Stage=@Stage " + Environment.NewLine);
                paramList.Add(new SqlParameter("@Stage", Stage));

            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), paramList, out printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Result.True;
        }

    }
}
