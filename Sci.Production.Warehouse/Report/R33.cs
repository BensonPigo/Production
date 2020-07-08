using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using Sci.Win;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Warehouse
{
    public partial class R33 : Win.Tems.PrintForm
    {
        private DataTable dtResult;

        public R33(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.txtSPFrom.Text) &&
               MyUtility.Check.Empty(this.txtSPTo.Text) &&
               !this.dateRangeIssueDate.HasValue)
            {
                MyUtility.Msg.WarningBox("<Issue Date> or <SP#> must be entered");
                return false;
            }

            return true;
        }

        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            string sqlWhere = string.Empty;
            string sqlGetData;
            List<SqlParameter> listPar = new List<SqlParameter>();
            if (this.dateRangeIssueDate.HasValue)
            {
                sqlWhere += " and I.IssueDate between @IssueDateFrom and @IssueDateTo ";
                listPar.Add(new SqlParameter("@IssueDateFrom", this.dateRangeIssueDate.DateBox1.Value));
                listPar.Add(new SqlParameter("@IssueDateTo", this.dateRangeIssueDate.DateBox2.Value));
            }

            if (!MyUtility.Check.Empty(this.txtSPFrom.Text))
            {
                sqlWhere += " and ID.POID >= @SPFrom ";
                listPar.Add(new SqlParameter("@SPFrom", this.txtSPFrom.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSPTo.Text))
            {
                sqlWhere += " and ID.POID <= @SPTo ";
                listPar.Add(new SqlParameter("@SPTo", this.txtSPTo.Text));
            }

            if (!MyUtility.Check.Empty(this.txtMdivision.Text))
            {
                sqlWhere += " and I.MDivisionid = @M ";
                listPar.Add(new SqlParameter("@M", this.txtMdivision.Text));
            }

            if (!MyUtility.Check.Empty(this.txtfactory.Text))
            {
                sqlWhere += " and I.Factoryid= @Factory ";
                listPar.Add(new SqlParameter("@Factory", this.txtfactory.Text));
            }

            sqlGetData = $@"
select  I.MDivisionid                ,
        I.FactoryID                  ,
        I.ID                         ,
        I.IssueDate                  ,
        ID.POID                ,
        ID.seq1                ,
        ID.Seq2                ,
        PD.Refno             ,
        PD.SuppColor         ,
        ID.Qty                 ,
        PD.StockUnit         ,
        U.Description                 ,
        [EditName] = dbo.getPass1(I.EditName)     ,
        I.Remark
from Issue I with (nolock)
inner join Issue_Detail ID with (nolock) on I.ID=ID.ID
Left join po_supp_detail PD with (nolock) on    ID.POID = PD.ID and 
                                                ID.seq1 = PD.seq1 and 
                                                ID.seq2 =PD.seq2
Left join Unit U with (nolock) on PD.StockUnit = U.id
where I.type= 'E' and I.Status = 'Confirmed'
{sqlWhere}


";
            DualResult result = DBProxy.Current.Select(null, sqlGetData, listPar, out this.dtResult);
            return result;
        }

        protected override bool OnToExcel(ReportDefinition report)
        {
            this.ShowWaitMessage("Excel Processing...");
            this.SetCount(this.dtResult.Rows.Count); // 顯示筆數

            if (this.dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                this.HideWaitMessage();
                return false;
            }

            Excel.Application objApp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\Warehouse_R33.xltx", objApp);
            com.UseInnerFormating = false;
            com.WriteTable(this.dtResult, 3);

            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = true;
            Marshal.ReleaseComObject(objApp);
            this.HideWaitMessage();
            return true;
        }
    }
}
