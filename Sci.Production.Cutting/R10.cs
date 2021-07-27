using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win;
using Sci.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class R10 : Win.Tems.PrintForm
    {
        private DataTable printData;
        private StringBuilder sqlcmd = new StringBuilder();
        private List<SqlParameter> sqlParameters = new List<SqlParameter>();

        /// <inheritdoc/>
        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.txtMdivision.Text = Sci.Env.User.Keyword;
            this.txtfactory.Text = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.sqlcmd.Clear();
            this.sqlParameters.Clear();
            if (!this.dateEstCutDate.Value1.HasValue)
            {
                MyUtility.Msg.WarningBox("Please input <Est. Cut Date>first!");
                return false;
            }

            string where = string.Empty;
            if (this.dateEstCutDate.Value1.HasValue)
            {
                where += "\r\nand mr.EstCutdate between @EstCutdate1 and @EstCutdate2";
                this.sqlParameters.Add(new SqlParameter("@EstCutdate1", SqlDbType.Date, 13) { Value = this.dateEstCutDate.Value1 });
                this.sqlParameters.Add(new SqlParameter("@EstCutdate2", SqlDbType.Date, 13) { Value = this.dateEstCutDate.Value2 });
            }

            if (!this.txtMdivision.Text.Empty())
            {
                where += "\r\nand mr.MDivisionid = @M";
                this.sqlParameters.Add(new SqlParameter("@M", SqlDbType.VarChar, 8) { Value = this.txtMdivision.Text });
            }

            if (!this.txtfactory.Text.Empty())
            {
                where += "\r\nand o.FtyGroup = @FtyGroup";
                this.sqlParameters.Add(new SqlParameter("@FtyGroup", SqlDbType.VarChar, 8) { Value = this.txtfactory.Text });
            }

            if (!this.txtSP1.Text.Empty())
            {
                where += "\r\nand mrd.OrderID >= @SP1";
                this.sqlParameters.Add(new SqlParameter("@SP1", SqlDbType.VarChar, 13) { Value = this.txtSP1.Text });
            }

            if (!this.txtSP2.Text.Empty())
            {
                where += "\r\nand mrd.OrderID <= @SP2";
                this.sqlParameters.Add(new SqlParameter("@SP2", SqlDbType.VarChar, 13) { Value = this.txtSP2.Text });
            }

            if (!this.txtCutCell1.Text.Empty())
            {
                where += "\r\nand mr.CutCellID >= @Cell1";
                this.sqlParameters.Add(new SqlParameter("@Cell1", SqlDbType.VarChar, 3) { Value = this.txtCutCell1.Text });
            }

            if (!this.txtCutCell2.Text.Empty())
            {
                where += "\r\nand mr.CutCellID <= @Cell2";
                this.sqlParameters.Add(new SqlParameter("@Cell2", SqlDbType.VarChar, 3) { Value = this.txtCutCell2.Text });
            }

            this.sqlcmd.Append($@"
select
	mr.ID,
	mr.Cutplanid,
	mr.EstCutdate,
	mr.MDivisionid,
	mr.CutCellID,
	RequestedBy = concat(pass1.id, '-' + pass1.Name),
	mr.Status,
	o.StyleID,
	mrd.OrderID,
	o.SeasonID,
	mrd.SizeRatio,
	mrd.MarkerNo,
	mrd.Layer,
	mrd.PatternPanel,
	mrd.FabricCombo,
	mrd.CuttingWidth,
	mrd.ReqQty,
	mrd.ReleaseQty,
	mrd.ReleaseDate
from MarkerReq mr
inner join MarkerReq_Detail mrd on mrd.id = mr.id
inner join orders o with(nolock) on o.id = mrd.OrderID
left join pass1 on pass1.id = mr.AddName
where 1=1
{where}");

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(ReportEventArgs e)
        {
            return DBProxy.Current.Select(null, this.sqlcmd.ToString(), this.sqlParameters, out this.printData);
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);
            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Excel.Application excelapp = new Excel.Application();
            Utility.Report.ExcelCOM com = new Utility.Report.ExcelCOM(Env.Cfg.XltPathDir + "\\Cutting_R10.xltx", excelapp);
            Excel.Worksheet worksheet = excelapp.Sheets[1];
            com.WriteTable(this.printData, 2);
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            excelapp.Columns.AutoFit();
            excelapp.Visible = true;

            string excelfile = Class.MicrosoftFile.GetName("Cutting_R10");
            excelapp.ActiveWorkbook.SaveAs(excelfile);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(excelapp);
            return true;
        }
    }
}
