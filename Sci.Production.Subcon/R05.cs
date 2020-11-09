using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        private DataTable printData;
        private DateTime? ReqDate_s;
        private DateTime? ReqDate_e;
        private string Sp;
        private string FactoryID;
        private string Supplier;
        private string Status;
        private string Style;
        private string ArtworkType;
        private bool isShowIrregularOnly;

        /// <inheritdoc/>
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.comboStatus.SelectedIndex = 0;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Sp = this.txtSP.Text;
            this.ArtworkType = this.txtArtworkType.Text;
            this.ReqDate_s = this.ReqDate.Value1;
            this.ReqDate_e = this.ReqDate.Value2;
            this.FactoryID = this.txtfactory.Text;
            this.Supplier = this.txtSupplier.Text;
            this.Style = this.txtstyle.Text;
            this.Status = this.comboStatus.Text;
            this.isShowIrregularOnly = this.chkOnlyIrregular.Checked;

            if (MyUtility.Check.Empty(this.ReqDate_s) || MyUtility.Check.Empty(this.ReqDate_e))
            {
                MyUtility.Msg.InfoBox("< Req. Date > cannot be empty!");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            StringBuilder sqlCmd = new StringBuilder();
            List<SqlParameter> paramList = new List<SqlParameter>();

            #region SQL
            sqlCmd.Append($@"

select r.FactoryID
	,r.ArtworkTypeID
	,r.ID
	,r.LocalSuppID
	,l.Abb
	,r.ReqDate
	,rd.OrderID
	,o.StyleID
	,o.SewInLine	
	,o.SciDelivery
	,rd.Article
	,rd.ArtworkID
	,rd.SizeCode
	,rd.PatternCode	
	,rd.PatternDesc
	,StdQty.Val
	,rd.ReqQty
	,rd.Stitch
	,rd.QtyGarment	
	,pod.Farmout
	,pod.Farmin
	,pod.ApQty
	,rd.ExceedQty
	,[SubconReasonID]=SubconReason.ID	
	,[SubconReason]=SubconReason.Reason
	,rd.ArtworkPOID
	,r.Status
	,[HandleBy]=(select Name from Pass1 where Pass1.id= r.Handle)
	,r.DeptApvDate	
	,[DeptMgrApvBy]=(select Name from Pass1 where Pass1.id= r.DeptApvName)
	,r.MgApvDate
	,[MgMgrApvBy]=(select Name from Pass1 where Pass1.id=r.MgApvName)
	,r.CloseUnCloseDate
	,[ClosedUnclosedBy]=(select Name from Pass1 where Pass1.id=r.CloseUnCloseName)	
	,r.Remark
from ArtworkReq r
inner join ArtworkReq_Detail rd ON r.ID = rd.ID
left join LocalSupp l ON l.ID = r.LocalSuppID
left join Orders o ON o.ID = rd.OrderID
left join ArtworkPO_Detail pod ON pod.ID = rd.ArtworkPOID
AND pod.OrderID = rd.OrderID 
AND pod.Article = rd.Article 
AND pod.SizeCode = rd.SizeCode    
AND pod.ArtworkId  = rd.ArtworkId 
AND pod.PatternCode= rd.PatternCode
AND pod.ArtworkReqID = rd.ID

OUTER APPLY(
	SELECT [Val]=IIF(rd.Article = '' OR rd.SizeCode = ''
		,(
			SELECT SUM(Qty) FROM Order_Qty oq WHERE oq.ID = rd.OrderID
		)
		,(
			SELECT SUM(Qty) FROM Order_Qty oq WHERE oq.ID = rd.OrderID AND oq.Article = rd.Article AND oq.SizeCode = rd.SizeCode
		)
	)
)StdQty
OUTER APPLY(
	SELECT s.ID, s.Reason
	FROM ArtworkReq_IrregularQty ai 
	INNER JOIN SubconReason s ON ai.SubconReasonID = s.ID
	WHERE s.Type = 'SQ' AND  ai.ArtworkTypeID = r.ArtworkTypeID AND ai.OrderID = rd.OrderID
)SubconReason
where 1=1

");
            #endregion

            #region Where
            if (!MyUtility.Check.Empty(this.ReqDate_s) && !MyUtility.Check.Empty(this.ReqDate_e))
            {
                sqlCmd.Append($"AND r.ReqDate BETWEEN @ReqDate_s AND @ReqDate_e" + Environment.NewLine);
                paramList.Add(new SqlParameter("@ReqDate_s", this.ReqDate_s.Value));
                paramList.Add(new SqlParameter("@ReqDate_e", this.ReqDate_e.Value));
            }

            if (!MyUtility.Check.Empty(this.ArtworkType))
            {
                sqlCmd.Append($"AND r.ArtworkTypeID=@ArtworkType " + Environment.NewLine);
                paramList.Add(new SqlParameter("@ArtworkType", this.ArtworkType));
            }

            if (!MyUtility.Check.Empty(this.FactoryID))
            {
                sqlCmd.Append($"AND r.FactoryID=@FactoryID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@FactoryID", this.FactoryID));
            }

            if (!MyUtility.Check.Empty(this.Supplier))
            {
                sqlCmd.Append($"AND r.LocalSuppID=@LocalSuppID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@LocalSuppID", this.Supplier));
            }

            if (!MyUtility.Check.Empty(this.Sp))
            {
                sqlCmd.Append($"AND rd.OrderID=@OrderID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@OrderID", this.Sp));
            }

            if (!MyUtility.Check.Empty(this.Style))
            {
                sqlCmd.Append($"AND o.StyleID=@StyleID " + Environment.NewLine);
                paramList.Add(new SqlParameter("@StyleID", this.Style));
            }

            if (!MyUtility.Check.Empty(this.Status))
            {
                sqlCmd.Append($"AND r.Status=@Status " + Environment.NewLine);
                paramList.Add(new SqlParameter("@Status", this.Status));
            }

            if (this.isShowIrregularOnly)
            {
                sqlCmd.Append($"AND r.Exceed=1 " + Environment.NewLine);
            }

            #endregion

            sqlCmd.Append($"ORDER BY r.FactoryID, r.ID, rd.OrderID, rd.SizeCode, rd.PatternCode" + Environment.NewLine);

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), paramList, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
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

            string templateName = "Subcon_R05";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + $"\\{templateName}.xltx"); // 預先開啟excel app
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, $"{templateName}.xltx", 1, false, null, objApp); // 將datatable copy to excel

            // 客製化欄位，記得設定this.IsSupportCopy = true
            // this.CreateCustomizedExcel(ref objSheets);
            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Subcon_R05");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            #endregion
            return true;
        }
    }
}
