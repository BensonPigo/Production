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
    public partial class R04 : Win.Tems.PrintForm
    {
        private DateTime? dateIssueDate_Start;
        private DateTime? dateIssueDate_End;
        private string strSupplier;
        private string strStyle;
        private string strSP_Start;
        private string strSP_End;
        private string strMdivision;
        private string strFactoryID;
        private DataTable printData;

        /// <inheritdoc/>
        public R04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            DataTable factory;
            DBProxy.Current.Select(null, "select '' as ID union all select ID from Factory WITH (NOLOCK) ", out factory);
            MyUtility.Tool.SetupCombox(this.comboFactory, 1, factory);
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string strWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.dateIssueDate_Start) && !MyUtility.Check.Empty(this.dateIssueDate_End))
            {
                sqlParameters.Add(new SqlParameter("@Date_Start", Convert.ToDateTime(this.dateIssueDate_Start).ToString("yyyy/MM/dd")));
                sqlParameters.Add(new SqlParameter("@Date_End", Convert.ToDateTime(this.dateIssueDate_End).ToString("yyyy/MM/dd")));
                strWhere += " and s.IssueDate BETWEEN  @Date_Start and @Date_End ";
            }

            if (!MyUtility.Check.Empty(this.strMdivision))
            {
                sqlParameters.Add(new SqlParameter("@MDivisionID", this.strMdivision));
                strWhere += " and s.MDivisionID = @MDivisionID";
            }

            if (!MyUtility.Check.Empty(this.strFactoryID))
            {
                sqlParameters.Add(new SqlParameter("@FactoryID", this.strFactoryID));
                strWhere += " and s.FactoryID = @FactoryID";
            }

            if (!MyUtility.Check.Empty(this.strSupplier))
            {
                sqlParameters.Add(new SqlParameter("@Supplier", this.strSupplier));
                strWhere += " and s.SubConOutFty = @Supplier";
            }

            if (!MyUtility.Check.Empty(this.strSP_Start))
            {
                sqlParameters.Add(new SqlParameter("@SP_Start", this.strSP_Start));
                strWhere += " and sd.OrderId >= @SP_Start";
            }

            if (!MyUtility.Check.Empty(this.strSP_End))
            {
                sqlParameters.Add(new SqlParameter("@SP_End", this.strSP_End));
                strWhere += " and sd.OrderId <= @SP_End";
            }

            if (!MyUtility.Check.Empty(this.strStyle))
            {
                sqlParameters.Add(new SqlParameter("@StyleID",this.strStyle));
                strWhere += " and o.StyleID = @StyleID";
            }

            if (this.rbPandQry_SewingQty.Checked && this.radioPanelReportType.Enabled)
            {
                strWhere += " and (PaidQty.val > SewingQty.val)";
            }

            if (!this.rbPandQry_SewingQty.Checked && this.radioPanelReportType.Enabled)
            {
                strWhere += " and (PaidQty.val <> sd.OutputQty)";
            }

            string sqlcmd = $@"
            
            SELECT
              [M] = s.MDivisionID
            , [Factory] = s.Factoryid
            , [Supplier] = ls.ID + '-' + ls.[Name]
            , [Contract Number] = s.ContractNumber
            , [SP] = sd.OrderId
            , [Style] = o.StyleID
            , [ComboType] = sd.ComboType
            , [Article] = sd.Article
            , [ContractQty] = isnull(sd.OutputQty,0)
            , [PaidQty] = isnull(PaidQty.val,0)
            , [SewingQty] = isnull(SewingQty.val,0)
            , [Currency] = isnull(sd.LocalCurrencyID,'')
            , [UnitPrice] =isnull(sd.LocalUnitPrice,0)
            , [Status] = s.Status
            FROM SubconOutContract s WITH(NOLOCK)
            LEFT JOIN LocalSupp ls WITH(NOLOCK) ON ls.ID = s.SubConOutFty
            INNER JOIN SubconOutContract_Detail sd WITH(NOLOCK) ON s.SubConOutFty = sd.SubConOutFty and s.ContractNumber = sd.ContractNumber
            LEFT JOIN Orders o WITH(NOLOCK) ON o.ID = sd.OrderID
            OUTER APPLY
            (
	            select val = sum(Qty)
	            from SubconOutContractAP_Detail sad
	            inner join SubconOutContractAP sa on sad.ID = sa.ID
	            where sad.ContractNumber = s.ContractNumber
	            and sad.OrderId = sd.OrderId
	            and sad.ComboType = sd.ComboType
	            and sad.Article = sd.Article
	            and sa.LocalSuppID = s.SubConOutFty
	            and sa.Status <> 'New'
            )PaidQty
            OUTER APPLY
            (
	            select val = sum(sod.QAQty)
	            from SewingOutput so
	            inner join SewingOutput_Detail sod on so.ID = sod.ID
	            where so.SubconOutFty = s.SubConOutFty
	            and so.SubConOutContractNumber = s.ContractNumber
	            and sod.OrderId = sd.OrderId
	            and sod.ComboType = sd.ComboType
	            and sod.Article = sd.Article

            )SewingQty
            WHERE 1 = 1
            {strWhere}
            ";

            DualResult result = DBProxy.Current.Select(null, sqlcmd, sqlParameters, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateIssueDate.Value1) &&
                MyUtility.Check.Empty(this.dateIssueDate.Value2) &&
                MyUtility.Check.Empty(this.txtSupplier.TextBox1.Text) &&
                MyUtility.Check.Empty(this.txtstyle.Text) &&
                MyUtility.Check.Empty(this.txtSP_Start.Text) &&
                MyUtility.Check.Empty(this.txtSP_End.Text))
            {
                MyUtility.Msg.WarningBox("Issue Date and Supplier and Style and SP# can't be empty!!");
                return false;
            }

            if (!MyUtility.Check.Empty(this.dateIssueDate.Value1) ||
                !MyUtility.Check.Empty(this.dateIssueDate.Value2))
            {
                if ((!MyUtility.Check.Empty(this.dateIssueDate.Value1) &&
                    MyUtility.Check.Empty(this.dateIssueDate.Value2))  ||
                    (MyUtility.Check.Empty(this.dateIssueDate.Value1) &&
                    !MyUtility.Check.Empty(this.dateIssueDate.Value2)))
                {
                    MyUtility.Msg.WarningBox("Issue Date can't be empty!!");
                    return false;
                }
            }

            this.dateIssueDate_Start = this.dateIssueDate.Value1;
            this.dateIssueDate_End = this.dateIssueDate.Value2;
            this.strMdivision = this.txtMdivision.Text;
            this.strFactoryID = this.comboFactory.Text;
            this.strSupplier = this.txtSupplier.TextBox1.Text;
            this.strStyle = this.txtstyle.Text;
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        private void ChShow_CheckedChanged(object sender, EventArgs e)
        {
            this.radioPanelReportType.Enabled = this.chShow.Checked ? true : false;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            string excelName = "Subcon_R04";

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + $"\\{excelName}.xltx"); // 預先開啟excel app

            MyUtility.Excel.CopyToXls(this.printData, string.Empty, showExcel: false, xltfile: $"{excelName}.xltx", headerRow: 1, excelApp: objApp);


            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName(excelName);
            Microsoft.Office.Interop.Excel.Workbook workbook = objApp.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strExcelName.OpenFile();
            #endregion
            return true;
        }
    }
}
