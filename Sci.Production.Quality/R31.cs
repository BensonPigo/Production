﻿using Ict;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Sci.Production.Prg.PowerBI.Model;
using Sci.Production.Prg.PowerBI.Logic;

namespace Sci.Production.Quality
{
    /// <inheritdoc/>
    public partial class R31 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;

        private DateTime? Buyerdelivery1;
        private DateTime? Buyerdelivery2;
        private string sp1;
        private string sp2;
        private string MDivisionID;
        private string FactoryID;
        private string Brand;
        private string Stage;
        private bool exSis;
        private bool Outstanding;
        private List<string> categoryList = new List<string>();

        private DataTable dtQAR31_Outstanding;

        /// <inheritdoc/>
        public R31(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboM.SetDefalutIndex(true);
            this.comboFactory.SetDataSource();
            this.comboStage.Enabled = false;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.categoryList.Clear();
            this.sp1 = this.txtSP_s.Text;
            this.sp2 = this.txtSP_e.Text;
            this.MDivisionID = this.comboM.Text;
            this.FactoryID = this.comboFactory.Text;
            this.Brand = this.txtBrand.Text;
            this.Buyerdelivery1 = this.dateBuyerDev.Value1;
            this.Buyerdelivery2 = this.dateBuyerDev.Value2;
            this.exSis = this.chkExSis.Checked;

            if (this.chkBulk.Checked)
            {
                this.categoryList.Add("B");
            }

            if (this.chkSample.Checked)
            {
                this.categoryList.Add("S");
            }

            if (this.chkGarment.Checked)
            {
                this.categoryList.Add("G");
            }

            this.Outstanding = this.chkOutstanding.Checked;
            this.Stage = this.comboStage.Text;

            if (MyUtility.Check.Empty(this.Buyerdelivery1) &&
                    MyUtility.Check.Empty(this.Buyerdelivery2) &&
                    MyUtility.Check.Empty(this.sp1) &&
                    MyUtility.Check.Empty(this.sp2))
            {
                MyUtility.Msg.InfoBox("Buyer Delivery and SP# can't be all empty.");

                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();

            if (!this.Outstanding)
            {
                #region SQL

                sqlCmd.Append($@"

SELECT  
oq.CFAFinalInspectResult
,[CFAIs3rdInspect] = IIF(oq.CFAIs3rdInspect = 1, 'Y','N')
,oq.CFA3rdInspectResult 
,o.MDivisionID
,o.FactoryID
,oq.BuyerDelivery
,o.BrandID
,oq.ID
,Category = CASE   WHEN o.Category='B' THEN 'Bulk'
                   WHEN o.Category='S' THEN 'Sample'
                   WHEN o.Category='G' THEN 'Garment'
                   ELSE ''
              END
,o.OrderTypeID
,o.CustPoNo
,o.StyleID
,s.StyleName
,o.SeasonID
,[Dest]=c.Alias
,o.Customize1
,o.CustCDID
,oq.Seq
,oq.ShipModeID
,[ColorWay] = Articles.Val
,o.SewLine
,oq.Qty
,[StaggeredOutput] = ISNULL(StaggeredOutput.Val,0)
,[CMPoutput] = ISNULL(CMPoutput.Val,0)
,[CMPOutput%] = IIF(
						oq.Qty = 0 OR CMPoutput.Val =  'N/A' 
						, 'N/A' 
						, Cast(  CAST(  ROUND( CAST( ISNULL(CMPoutput.Val,0) as int) * 1.0  / oq.Qty * 100 ,0) as int)  as varchar)  
					)
,[ClogReceivedQty] =IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(ClogReceivedQty.Val,0) as varchar)  )
,[ClogReceivedQty%]=IIF( oq.Qty = 0 OR o.Category ='S' 
						,'N/A' 
						,Cast(CAST(  ROUND( (CAST( ISNULL(ClogReceivedQty.Val,0) as int) * 1.0 / oq.Qty * 100 ),0) as int)  as varchar)  
					)
,[TtlCtn] =  IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(TtlCtn.Val,0) as varchar)  )
,[StaggeredCtn] = IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(StaggeredCtn.Val,0) as varchar)  )
,[ClogCtn] = IIF(o.Category ='S' ,'N/A'  ,Cast( ISNULL(ClogCtn.Val,0) as varchar)  )
,[ClogCtn%]= IIF( ClogCtn.Val = 0 OR o.Category ='S'
					, 'N/A' 
					, CAST( CAST(ROUND((TtlCtn.Val *1.0 / TtlCtn.Val * 100),0) as int)  as varchar)  
				)
,[Last carton received date] = LastReceived.Val
,oq.CFAFinalInspectDate
,oq.CFA3rdInspectDate
,oq.CFARemark
FROM Order_QtyShip oq
INNER JOIN Orders o ON o.ID = oq.Id and exists (select 1 from Factory f where o.FactoryId = id and f.IsProduceFty = 1)
INNER JOIN Factory f ON o.FactoryID = f.ID
INNER JOIN Style s ON o.StyleUkey = s.Ukey
LEFT JOIN OrderType ot ON o.OrderTypeID = ot.ID AND o.BrandID = ot.BrandID
LEFT JOIN Country c ON o.Dest = c.ID
OUTER APPLY(
	SELECT [Val]=STUFF((
	SELECT DISTINCT ',' + Article
	FROM Order_QtyShip_Detail oqd
	WHERE oqd.ID = oq.Id ANd oqd.Seq = oq.Seq
	FOR XML PATH('')
	) ,1,1,'')
)Articles
OUTER APPLY(
	SELECT [Val]=Sum(pd.ShipQty)
	From PackingList_Detail pd
	INNER JOIN CFAInspectionRecord cfa on pd.StaggeredCFAInspectionRecordID	 = cfa.ID
	Where cfa.Status='Confirmed' and cfa.Stage='Stagger' and pd.OrderID = oq.Id and pd.OrderShipmodeSeq = oq.Seq
)StaggeredOutput
OUTER APPLY(
	SELECT [Val] = IIF( (SELECT COUNT(oq2.Seq) FROM Order_QtyShip oq2 WHERE oq2.ID = oq.ID) > 1
						,'N/A' 
						, Cast( dbo.getMinCompleteSewQty(oq.ID,NULL,NULL) as varchar)
					)
)CMPoutput
OUTER APPLY(
	SELECT [Val] = ISNULL( SUM(IIF
				( pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL
				,pd.ShipQty
				,0)
			),0)
	FROM PackingList_Detail pd
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq= oq.Seq
)ClogReceivedQty
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM PackingList_Detail pd
	WHERE pd.OrderID = oq.ID AND pd.OrderShipmodeSeq  = oq.Seq AND pd.CTNQty = 1
)TtlCtn
OUTER APPLY(
	SELECT [Val]=Count(DISTINCT pd.CTNStartNo)
	From PackingList_Detail pd 
	INNER JOIN CFAInspectionRecord CFA on pd.StaggeredCFAInspectionRecordID=CFA.ID
	Where CFA.Status='Confirmed' 
	AND CFA.Stage='Stagger'
	AND pd.CTNQty=1
	AND pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
)StaggeredCtn
OUTER APPLY(
	SELECT [Val]= COUNT(DISTINCT pd.CTNStartNo)
	FROM PackingList_Detail pd 
	where pd.OrderID = oq.ID 
	AND pd.OrderShipmodeSeq = oq.Seq
	AND pd.CTNQty=1
	AND (pd.CFAReceiveDate IS NOT NULL OR pd.ReceiveDate IS NOT NULL)
)ClogCtn
OUTER APPLY(
	SELECT [Val] = MAX(pd.ReceiveDate) 
	FROM PackingList_Detail pd
	WHERE pd.OrderID = oq.Id AND pd.OrderShipmodeSeq  = oq. Seq
	AND NOT exists (
		-- 每個紙箱必須放在 Clog（ReceiveDate 有日期）
		select 1 
		from Production.dbo.PackingList_Detail pdCheck
		where pd.OrderID = pdCheck.OrderID 
				and pd.OrderShipmodeSeq = pdCheck.OrderShipmodeSeq
				and pdCheck.ReceiveDate is null
	)
)LastReceived 
WHERE 1=1
");
                #endregion

                #region Where
                if (!MyUtility.Check.Empty(this.Buyerdelivery1))
                {
                    sqlCmd.Append($"AND oq.BuyerDelivery BETWEEN @Buyerdelivery1 AND @Buyerdelivery2" + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Buyerdelivery1", this.Buyerdelivery1.Value));
                    paramList.Add(new SqlParameter("@BuyerDelivery2", this.Buyerdelivery2.Value));
                }

                if (!MyUtility.Check.Empty(this.sp1))
                {
                    sqlCmd.Append($"AND oq.ID >= @sp1" + Environment.NewLine);
                    SqlParameter p = new SqlParameter("@sp1", SqlDbType.VarChar)
                    {
                        Value = this.sp1,
                    };
                    paramList.Add(p);
                }

                if (!MyUtility.Check.Empty(this.sp2))
                {
                    sqlCmd.Append($"AND oq.ID <= @sp2" + Environment.NewLine);
                    SqlParameter p = new SqlParameter("@sp2", SqlDbType.VarChar)
                    {
                        Value = this.sp2,
                    };
                    paramList.Add(p);
                }

                if (!MyUtility.Check.Empty(this.MDivisionID))
                {
                    sqlCmd.Append($"AND o.MDivisionID=@MDivisionID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@MDivisionID", this.MDivisionID));
                }

                if (!MyUtility.Check.Empty(this.FactoryID))
                {
                    sqlCmd.Append($"AND o.FtyGroup=@FactoryID " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@FactoryID", this.FactoryID));
                }

                if (!MyUtility.Check.Empty(this.Brand))
                {
                    sqlCmd.Append($"AND o.BrandID=@Brand " + Environment.NewLine);
                    paramList.Add(new SqlParameter("@Brand", this.Brand));
                }

                if (this.exSis)
                {
                    sqlCmd.Append($"AND f.IsProduceFty = 1" + Environment.NewLine);
                }

                if (this.categoryList.Count > 0)
                {
                    sqlCmd.Append($"AND o.Category IN ('{this.categoryList.JoinToString("','")}') " + Environment.NewLine);
                }
                else
                {
                    // 如果全部勾選，則無資料
                    sqlCmd.Append($"AND 1=0 " + Environment.NewLine);
                }

                #endregion
            }
            else
            {
                QA_R31 biModel = new QA_R31();

                #region 與BI共用Data Logic
                QA_R31_ViewModel qa_R31 = new QA_R31_ViewModel()
                {
                    BuyerDelivery1 = this.Buyerdelivery1,
                    BuyerDelivery2 = this.Buyerdelivery2,
                    SP1 = this.sp1,
                    SP2 = this.sp2,
                    MDivisionID = this.MDivisionID,
                    FactoryID = this.FactoryID,
                    Brand = this.Brand,
                    Category = this.categoryList.JoinToString("','"),
                    Exclude_Sister_Transfer_Out = this.exSis,
                    Outstanding = this.Outstanding,
                    InspStaged = this.Stage,
                };

                Base_ViewModel resultReport = biModel.GetQA_R31Data(qa_R31);
                if (!resultReport.Result)
                {
                    return resultReport.Result;
                }

                this.printData = resultReport.DtArr[0];

                #endregion
            }

            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            this.SetCount(this.printData.Rows.Count);
            StringBuilder c = new StringBuilder();
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            // Outstanding需要使用不同範本
            string template = !this.Outstanding ? "Quality_R31.xltx" : "Quality_R31_Outstanding.xltx";

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{template}"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, template, 1, false, null, objApp); // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            objSheets.Columns[3].ColumnWidth = 12;
            objSheets.Columns[4].ColumnWidth = 12;
            objSheets.Columns[5].ColumnWidth = 12;
            objSheets.Columns[6].ColumnWidth = 12;

            // 客製化欄位，記得設定this.IsSupportCopy = true
            // this.CreateCustomizedExcel(ref objSheets);
            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Quality_R31");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objSheets);
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            return true;
        }

        private void ChkOutstanding_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkOutstanding.Checked)
            {
                this.comboStage.Enabled = true;
            }
            else
            {
                this.comboStage.Text = string.Empty;
                this.comboStage.Enabled = false;
            }
        }
    }
}
